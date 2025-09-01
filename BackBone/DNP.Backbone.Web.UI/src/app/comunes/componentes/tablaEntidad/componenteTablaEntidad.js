(function () {

    angular.module('backbone').component('tablaEntidad', {
        templateUrl: '/src/app/comunes/componentes/tablaEntidad/tablaEntidad.template.html',
        controller: controladorTablaEntidad,
        controllerAs: 'vm',
        bindings: {
            opciones: '=',
            perfilUsuario1: '=',
            datos: '=',
            definicionColumnaDatos: '=',
            definicionColumnaSubDatos: '=',
            accion1: '&',
            accion2: '&',
            accion3: '&',
            accion4: '&',
            accion5: '&',
        }
    });

    controladorTablaEntidad.$inject = [
        '$scope',
        'autorizacionServicios',
        'servicioUsuarios', 'servicioEntidades', 'constantesBackbone', 'constantesAutorizacion', 'utilidades', '$uibModal', '$window', 'FileSaver', '$location', 'Blob', 'backboneServicios'
    ];

    function controladorTablaEntidad($scope, autorizacionServicios, servicioUsuarios, servicioEntidades, constantesBackbone, constantesAutorizacion, utilidades, $uibModal, $window, FileSaver, $location, Blob, backboneServicios) {
        var vm = this;

        // metodos view-model

        // variables two-away databind
        vm.gridOptions;
        vm.alturaTabla = 'height: calc(80vh - 100px);';
        vm.hayResultadoParaTipoEntidad;

        // private variables
        _agrupadorEntidadTemplate = 'src/app/comunes/componentes/tablaEntidad/plantillas/agrupadorEntidad.template.html';
        _subTablaTemplate = 'src/app/comunes/componentes/tablaEntidad/plantillas/subTabla.template.html';
        _datosTemplate = 'src/app/comunes/componentes/tablaEntidad/plantillas/datos.template.html';
        _columnDefNivelJerarquico = [{
            field: 'entidad',
            displayName: 'Entidad',
            enableFiltering: false,
            showHeader: false,
            enableHiding: false,
            width: '100%',
            cellTemplate: _agrupadorEntidadTemplate
        }];
        vm.abrirModalEditarIndex = abrirModalEditarIndex;
        vm.abrirModalEliminarIndex = abrirModalEliminarIndex;
        vm.accederPantallaPerfilesUsuarioIndex = accederPantallaPerfilesUsuarioIndex;
        vm.consultarPermisoIndex = backboneServicios.consultarPermiso;
        vm.cambiarEstadoUsuarioIndex = cambiarEstadoUsuarioIndex;
        vm.cambiarEstadoUsuarioIndexPerfil = cambiarEstadoUsuarioIndexPerfil;
        vm.abrirModalRolesIndexPerfil = abrirModalRolesIndexPerfil;
        vm.abrirModalProyectosIndexPerfil = abrirModalProyectosIndexPerfil;
        function abrirModalRolesIndexPerfil(objPerfil) {
            let perfil = {
                perfil: objPerfil.Nombre,
                idPerfil: objPerfil.Id,
            }

            $uibModal.open({
                templateUrl: 'src/app/usuarios/usuarioPerfil/modales/roles/modalRolesPerfil.html',
                controller: 'modalRolesPerfilController',
                resolve: {
                    perfilSeleccionado: perfil
                }
            })
                .result.then(function () { /*modal manually closed*/ }, function (reason) { /*modal dismissed*/ });
        };

        /// Modal Proyectos
        function abrirModalProyectosIndexPerfil(objPerfil) {
            let perfil = {
                perfil: objPerfil.Nombre,
                idUsuarioPerfil: objPerfil.IdUsuarioPerfil,
                entidad: objPerfil.entidad,
                idEntidad: objPerfil.idEntidad
            }

            $uibModal.open({
                templateUrl: 'src/app/usuarios/usuarioPerfil/modales/proyectos/modalProyectosPerfil.html',
                controller: 'modalProyectosPerfilController',
                openedClass: 'modal-proyecto-perfil',
                resolve: {
                    perfilSeleccionado: perfil
                }
            });
        }
        function cambiarEstadoUsuarioIndexPerfil(obj) {
            obj.Activo = !obj.Activo;
            debugger
            servicioUsuarios.setActivoUsuarioPerfil(obj.IdUsuarioPerfil, obj.Activo, vm.perfilUsuario1, obj.idEntidad)
                .then(function (response) {
                    if (response.data) {
                        utilidades.mensajeSuccess("Operación realizada con éxito!", false, false, false);
                    } else
                        utilidades.mensajeError("Error al realizar la operación", false);
                });
        }
        function abrirModalEditarIndex(obj) {
            $uibModal.open({
                templateUrl: 'src/app/usuarios/usuarios/modales/modalAccionUsuario.html',
                controller: 'modalAccionUsuarioController',
                resolve: {
                    obj: obj,
                }
            }).result.then(function (result) {
                listarUsuarios();
            }, function (reason) {
                listarUsuarios();
            });
        }
        function abrirModalEliminarIndex(obj) {
            utilidades.mensajeWarning("Confirma la exclusión del registro?", function funcionContinuar() {
                servicioUsuarios.eliminarUsuarioPerfil(obj.IdUsuarioPerfil, obj.IdEntidad, obj.IdUsuarioDNP)
                    .then(function (response) {
                        if (response.data.Exito) {
                            utilidades.mensajeSuccess("Operación realizada con éxito!", false, false, false);
                            listarUsuarios();
                        } else {
                            utilidades.mensajeError("Error al realizar la operación", false);
                        }

                    }, function (response) {
                        if (response.status == 409) {
                            utilidades.mensajeError("No es posible eliminar este perfil de usuario, ya que tiene proyectos vinculados.", false);
                        } else {
                            utilidades.mensajeError("Error al realizar la operación", false);
                        }
                    })
            }, function funcionCancelar() {
            })
        }
        function cambiarEstadoUsuarioIndex(obj) {
            obj.Activo = !obj.Activo;

            servicioUsuarios.setActivoUsuarioPerfilPorEntidad(obj.Id, obj.IdEntidad, obj.Activo, obj.IdUsuarioDNP, vm.tipoEntidad)
                .then(function (response) {
                    if (response.data) {
                        utilidades.mensajeSuccess("Operación realizada con éxito!", false, false, false);
                    } else
                        utilidades.mensajeError("Error al realizar la operación", false);
                });
        }
        vm.AbrilNivel = function (idEntidad) {
            vm.datos.forEach(function (value, index) {
                if (value.idEntidad == idEntidad) {
                    if (value.estadoEntidad == '+')
                        value.estadoEntidad = '-';
                    else
                        value.estadoEntidad = '+';
                }
            });
        }
        function accederPantallaPerfilesUsuarioIndex(usuario) {
            $location.path(`/usuarios/${usuario.Id}`);
        }
        //Implementaciones
        $scope.$watch('vm.datos', function () {
            if (vm.datos) {
                vm.gridOptions.data = vm.datos;
                if (vm.opciones && vm.opciones.nivelJerarquico) {
                    vm.gridOptions.data.forEach(data => {
                        confugurarTablaJerarquica(vm.gridOptions, vm.opciones.nivelJerarquico, data);
                    });
                }
            }
            vm.hayResultados = vm.datos && vm.datos.length > 0;
        });


        vm.$onInit = function () {
            if (!vm.gridOptions) {
                vm.gridOptions = {
                    onRegisterApi: onRegisterApi,
                    data: []
                }

                if (vm.opciones && vm.opciones.gridOptions) {
                    Object.keys(vm.opciones.gridOptions).forEach(opt => {
                        vm.gridOptions[`${opt}`] = vm.opciones.gridOptions[`${opt}`];
                    });
                }

                if (vm.opciones && vm.opciones.nivelJerarquico) {
                    vm.gridOptions.expandableRowTemplate = _subTablaTemplate;
                }

                if (!vm.opciones || !vm.opciones.nivelJerarquico) {
                    vm.gridOptions.columnDefs = vm.definicionColumnaDatos;
                }
            }            

            if (vm.opciones.cambiarTipoEntidadFiltro) {

                vm.tiposEntidad = autorizacionServicios.obtenerTiposEntidad();
                vm.cambiarFiltroEntidad = function (tipoEntidad) {
                    if (vm.tipoEntidadSeleccionada === tipoEntidad)
                        return;

                    vm.tipoEntidadSeleccionada = tipoEntidad;
                    return vm.opciones.cambiarTipoEntidadFiltro(tipoEntidad);

                }
                vm.cambiarFiltroEntidad(vm.tiposEntidad[0].clave);
            }
        };

        function confugurarTablaJerarquica(gridOptions, nivel, dataRow) {
            if (!gridOptions.hasOwnProperty('data')) {
                throw new NotFoundPropertyException('data');
            }

            if (nivel >= 1) {
                const data = dataRow;//gridOptions.data[0];
                if (!data.hasOwnProperty('subGridOptions')) {
                    throw new NotFoundPropertyException('subGridOptions');
                }

                if (!data.hasOwnProperty('agrupadorEntidad')) {
                    throw new NotFoundPropertyException('agrupadorEntidad');
                }

                if (!data.hasOwnProperty('entidad')) {
                    throw new NotFoundPropertyException('entidad');
                }

                if (!data.hasOwnProperty('tipoEntidad')) {
                    throw new NotFoundPropertyException('tipoEntidad');
                }

                gridOptions.enableFiltering = false;
                gridOptions.showHeader = false;
                gridOptions.enableHiding = false;
                gridOptions.enableColumnMenus = false;
                gridOptions.columnDefs = _columnDefNivelJerarquico;
                gridOptions.appScopeProvider = $scope;

                if (nivel == 1) {
                    gridOptions.expandableRowTemplate = _datosTemplate;
                } else {
                    gridOptions.expandableRowTemplate = _subTablaTemplate;
                }

                gridOptions.expandableRowScope = {
                    subGridVariable: 'subGridScopeVariable'
                }

                confugurarTablaJerarquica(data.subGridOptions, (nivel - 1));
            }

            if (nivel == 0) {
                //gridOptions.columnDefs = gridOptions.columnDefs != null ? gridOptions.columnDefs : vm.definicionColumnaDatos;
                gridOptions.columnDefs = vm.definicionColumnaDatos;
                gridOptions.appScopeProvider = $scope;
                // gridOptions.enableExpandableRowHeader = false;
                // gridOptions.expandableRowTemplate = _subTablaTemplate;
                // gridOptions.expandableRowScope = {
                //     subGridVariable: 'subGridScopeVariable'
                // }
            }
        }

        function onRegisterApi(gridApi) {
            $scope.gridApi = gridApi;
        }

        function hayResultadoParaTipoEntidad() {
            // adicionar regra para mostrar a mensagem 'No hay ningún resultado para su búsqueda por el tipo de entidad <nombre tipo entidad>
            // quando clicar em alguma das abas de filtro como Territorial, Nacional, SGR, Públicas e Privadas'
            // definir o valor booleano para a variavel vm.hayResultadoParaTipoEntidad
            return vm.gridOptions.data.length == 0;
        }

        function NotFoundPropertyException(value) {
            this.value = value;
            this.message = `Not found property: ${value}`;
            this.toString = function () {
                return this.message;
            };
        }

    }
})();