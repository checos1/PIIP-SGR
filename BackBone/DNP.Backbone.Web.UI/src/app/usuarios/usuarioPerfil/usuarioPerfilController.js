(function () {
    'use strict';

    usuarioPerfilController.$inject = ['$scope', 'servicioUsuarios', 'servicioEntidades', 'autorizacionServicios', 'constantesAutorizacion', 'utilidades', '$uibModal', '$routeParams'];

    function usuarioPerfilController($scope, servicioUsuarios, servicioEntidades, autorizacionServicios, constantesAutorizacion, utilidades, $uibModal, $routeParams) {
        var vm = this;

        vm.id = $routeParams.id;
        vm.isEdicion = !!$routeParams.id;

        vm.cambiarEstadoUsuario = cambiarEstadoUsuario;

        vm.obtenerListadoEntidadesXUsuarioAutenticado = obtenerListadoEntidadesXUsuarioAutenticado;
        /// Modais
        vm.abrirModalCrearEditar = abrirModalCrearEditar;

        vm.abrirModalRoles = abrirModalRoles;
        vm.abrirModalProyectos = abrirModalProyectos;
        vm.abrirModalEditar = abrirModalEditar;

        /// Plantilla de acciones de la tabla
        vm.plantillaAccionesTabla = 'src/app/usuarios/usuarioPerfil/plantillas/plantillaAccionesTabla.html';
        vm.columnaEstado = 'src/app/usuarios/usuarioPerfil/plantillas/columnaEstado.html';

        /// Definiciones de componente
        vm.opciones = {
            cambiarTipoEntidadFiltro: cambiarTipoEntidadFiltro,
            nivelJerarquico: 1,
            BanderaTabla: 2,
            gridOptions: {
                //showHeader: true,
                paginationPageSizes: [5, 10, 15, 25, 50, 100],
                paginationPageSize: 5,
                expandableRowHeight: 220
            }
        };
        /// Definiciones de columna componente
        vm.columnDef = [{
            field: 'Nombre',
            displayName: 'Perfil',
            enableHiding: false,
            width: '25%'
        }, {
            field: 'NombreSubEntidad',
            displayName: 'Sub-Entidad',
            enableHiding: false,
            width: '25%',
        }, {
            field: 'Estado',
            displayName: 'Estado',
            enableFiltering: false,
            enableHiding: false,
            enableSorting: false,
            enableColumnMenu: false,
            cellTemplate: vm.columnaEstado,
            width: '15%'
        }, {
            field: 'Accion',
            displayName: 'Acción',
            enableFiltering: false,
            enableHiding: false,
            enableSorting: false,
            enableColumnMenu: false,
            headerCellClass: 'text-center',
            cellTemplate: vm.plantillaAccionesTabla,
            width: '23%'
        }];

        /// Comienzo
        vm.init = function () {
            obtenerDatosPerfilUsuario();
        }
        vm.perfilUsuario1 = {
            usuario: 1
        };
        vm.obtenerNombreUsuario = function (nombre) {
            let arrayNombre = nombre.split(':');

            if (arrayNombre.length > 0) {
                nombre = arrayNombre[0];
            }

            return nombre;
        };
        vm.AbrilNivel = function (idEntidad) {
            vm.datos.forEach(function (value, index) {
                if (value.IdEntidad == idEntidad) {
                    if (value.estadoEntidad == '+')
                        value.estadoEntidad = '-';
                    else
                        value.estadoEntidad = '+';
                }
            });
        }
        vm.obtenerApellidoUsuario = function (apellido) {
            let arrayNombre = apellido.split(':');

            if (arrayNombre.length > 1) {
                apellido = arrayNombre[1];
            }

            return apellido;
        };


        /// Getters
        function obtenerDatosPerfilUsuario() {
            const usuarioId = vm.isEdicion && vm.id || "";
            servicioUsuarios.obtenerDatosUsuario(usuarioId, usuarioDNP)
                .then(function (response) {
                    if(!response.data)
                        return;

                    const data = response.data;
                    const nombre = vm.obtenerNombreUsuario(data.Nombre);
                    const sobreNombre = vm.obtenerApellidoUsuario(data.Nombre);

                    vm.perfilUsuario = {
                        usuario: data.IdUsuarioDnp,
                        nombre: nombre,
                        apellido: sobreNombre
                    };
                }, function (error) {
                    console.log(error);
                    toastr.error("Hubo un error al cargar las informaciones del usuário");
                });
        }

        /// Actions
        function cambiarTipoEntidadFiltro(tipoEntidad) {
            vm.datos = [];
            var usuarioId = vm.isEdicion ? vm.id : null;
            servicioUsuarios.obtenerPerfilesPorUsuarioXUsuarioAutenticado(tipoEntidad, usuarioId)
                .then(function (response) {
                    {
                        let datos = response.data;
                        if (datos != null && datos.length > 0) {
                            datos.forEach(item => {
                                item.agrupadorEntidad = item.AgrupadorEntidad;
                                item.entidad = item.Entidad;
                                item.tipoEntidad = item.TipoEntidad;
                                item.estadoEntidad = "+";
                                item.Perfiles.forEach(perfil => {
                                    perfil.idEntidad = item.IdEntidad;
                                    perfil.entidad = item.Entidad || item.AgrupadorEntidad;
                                    perfil.isEdicion = vm.isEdicion;
                                });

                                item.subGridOptions = {
                                    //columnDefs: vm.columnDef,
                                    paginationPageSizes: [5, 10, 15, 25, 50, 100],
                                    paginationPageSize: 5,
                                    expandableRowHeight: '200px',
                                    data: item.Perfiles
                                };
                            });
                            
                            vm.datos = datos;
                        }

                    }
                }, function (error) {
                    console.log("error");
                    //TODO: erro?
                });
        }

        function obtenerListadoEntidadesXUsuarioAutenticado() {
            servicioEntidades.obtenerListadoEntidadesXUsuarioAutenticado()
                .then(function (response) {
                    vm.listadoEntidades = response.data;
                }, function (error) {
                    console.log("error: obtenerListadoEntidadesXUsuarioAutenticado", error);
                });
        }

        function abrirModalCrearEditar(obj) {
            $uibModal.open({
                templateUrl: 'src/app/usuarios/usuarioPerfil/modales/modalAccionUsuarioPerfil.html',
                controller: 'modalAccionUsuarioPerfilController',
                resolve: {
                    obj: {
                        idUsuario: vm.id,
                        idUsuarioDnp: vm.perfilUsuario.usuario,
                        obtenerDatosPerfilUsuario: obtenerDatosPerfilUsuario
                    }
                }
            }).result.then(function (result) {
                obtenerDatosPerfilUsuario();
                cambiarTipoEntidadFiltro("Nacional");
            }, function (reason) {
                    obtenerDatosPerfilUsuario();
                    cambiarTipoEntidadFiltro("Nacional");
            });
        };

        function cambiarEstadoUsuario(obj) {
            obj.Activo = !obj.Activo;
            
            servicioUsuarios.setActivoUsuarioPerfil(obj.IdUsuarioPerfil, obj.Activo, vm.perfilUsuario.usuario, obj.idEntidad)
                .then(function (response) {
                    if (response.data) {
                        utilidades.mensajeSuccess("Operación realizada con éxito!", false, false, false);
                    } else
                        utilidades.mensajeError("Error al realizar la operación", false);
                });
        }

        /// Modal Roles
        function abrirModalRoles(objPerfil) {
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
        function abrirModalProyectos(objPerfil) {
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

        function abrirModalEditar(obj) {
            $uibModal.open({
                templateUrl: 'src/app/usuarios/usuarios/modales/modalAccionUsuario.html',
                controller: 'modalAccionUsuarioController',
                openedClass: "consola-modal-soportesDNP",
                resolve: {
                    obj: obj,
                }
            }).result.then(function (result) {
                listarUsuarios();
            }, function (reason) {
                listarUsuarios();
            });
        }

    }

    // ReSharper disable once UndeclaredGlobalVariableUsing
    angular.module('backbone.usuarios').controller('usuarioPerfilController', usuarioPerfilController);
})();
