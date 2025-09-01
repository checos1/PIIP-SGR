(function (usuarioDNP, idTipoProyecto, idTipoTramite) {

    angular.module('backbone').component('notificaciones', {
        templateUrl: '/src/app/comunes/notificaciones/plantillaNotificacionesDropdown.html',
        controller: controladorNotificacionesDropdown,
        controllerAs: 'vm'
    });

    controladorNotificacionesDropdown.$inject = [
        '$q',
        'serviciosComponenteNotificaciones',
        'flujoServicios',
        'estadoAplicacionServicios',
        '$uibModal',
        'autorizacionServicios',
        'utilidades',
        'sesionServicios',
        '$location',
        '$filter',
        '$scope',
        'backboneServicios',
        'servicioNotificacionesMensajes',
        '$route',
        'blockUI'
    ];

    function controladorNotificacionesDropdown(
        $q,
        serviciosComponenteNotificaciones,
        flujoServicios,
        estadoAplicacionServicios,
        $uibModal,
        autorizacionServicios,
        utilidades,
        sesionServicios,
        $location,
        $filter,
        $scope,
        backboneServicios,
        servicioNotificacionesMensajes,
        $route,
        blockUI
    ) {
        const vm = this;
        vm.cantidadDeAlertas = 0;
        vm.proyectosIds = [];
        vm.notificacionesCargando = false;
        vm.notificaciones = null;
        vm.cantidadDeNotificaciones = 0;
        vm.cantidadDeMensajes = 0;
        vm.flujosAutorizados = [];

        vm.mostrarModal = mostrarModal;
        vm.mostrarNotifacion = mostrarNotifacion;
        vm.mostrarFlujosDisponibles = mostrarFlujosDisponibles;
        vm.tienePermisos = tienePermisos;
        vm.notificacionesYaFueronCargadas = false;
        vm.esTramite = esTramite;
        vm.crearSolicitud = crearSolicitud;
        //vm.crearProyectos = crearProyectos;        
        vm.administracionURL = administracionURL; //TODO Localizar donde se asigna esta variable     
        vm.nombreEntornoPIIP = nombreEntornoPIIP;
        vm.cambiarModulos = cambiarModulos;
        //CODIGO ANTERIOR
        //vm.cambiarModulos = cambiarModulos;

        //vm.peticion;
        //vm.listaEntidades = [];
        //vm.listarFiltroEntidades = listarFiltroEntidades;
        //vm.cerrarTramite = cerrarTramite;
        //vm.guardarSolicitud = guardarSolicitud;
        //vm.cargarFlujosTramites = cargarFlujosTramites;
        //vm.tramites = [];
        //vm.flujos;
        //vm.isOpen = false;

        //vm.popOverOptionsCrearTramite = {
        //    isOpen: false,
        //    templateUrl: 'myPopoverTemplate.html',
        //    toggle: function () {
        //        vm.popOverOptionsCrearTramite.isOpen = !vm.popOverOptionsCrearTramite.isOpen;
        //    }
        //};

        //vm.objSolicitud = {
        //    codTramite: null,
        //    codEntidade: null,
        //    descripcion: null
        //};

        //function cambiarModulos() {
        //    blockUI.start();
        //    autorizacionServicios.cambiarModulo();
        //}

        vm.$onInit = function () {
            if (backboneServicios.estaAutorizado() && vm.notificacionesYaFueronCargadas == false) {
                vm.notificacionesYaFueronCargadas = true;
                cargarNotificaciones();
                cargarNotificacionesMensajes();
                obtenerNotificacionesCantidadDeAlertas();
            }
            //CODIGO ANTERIOR
            //listarFiltroEntidades();
            //cargarFlujosTramites();
        };

        $scope.$on('AutorizacionConfirmada', function () {
            if (vm.notificacionesYaFueronCargadas == false) {
                vm.notificacionesYaFueronCargadas = true;
                cargarNotificaciones();
                cargarNotificacionesMensajes();
                obtenerNotificacionesCantidadDeAlertas();
            }
        });

        ////////////

        var modalConfigDefault = {
            animation: true,
            controllerAs: "vm",
            backdrop: false,
            keyboard: false,
            size: 'lg'
        }

        function cambiarModulos() {
            autorizacionServicios.cambiarModulo();
        }

        function esTramite() {
            if ($location.$$url == "/tramites") {
                return true;
            } else {
                return false;
            }
        }

        function crearSolicitud() {

            $uibModal.open({
                animation: $scope.animationsEnabled,
                templateUrl: 'src/app/comunes/notificaciones/modales/modalCrearEditarTareaSolicitud.html',
                controller: 'modalCrearEditarTareaSolicitudController',
                controllerAs: "vm",
                size: 'xl',
                // openedClass: "dialog-modal-alerta",
                // resolve: {
                //    // idAlerta: idAlerta, peticion: vm.peticion, listarGrid: function () { return listaConsolaAlertas; }
                // },
            });

        }

        //function crearProyectos() {
        //    $uibModal.open({
        //        animation: $scope.animationsEnabled,
        //        templateUrl: 'src/app/formulario/modales/selecionarProyectosCreditos.html',
        //        controller: 'selecionarProyectosCreditosController',
        //        controllerAs: "vm",
        //        size: 'xl',
        //         resolve: {
        //             parametros: {
        //                 idFlujo: '9FA107AF-6F1D-44B1-B613-737051611FD2',
        //                 tipoEntidad: 'Nacional',
        //                 idInstancia: 'AA275569-90F9-4CA5-98F8-444F993D1FB1'
        //             }
        //         }
        //    });
        //}

        function mostrarFlujosDisponibles() {

            vm.flujosAutorizados = [];

            cargarFlujos().then(
                function (flujosAutorizados) {
                    if (flujosAutorizados) {
                        if (flujosAutorizados.length === 0)
                            utilidades.mensajeError($filter('language')('ErrorUsuarioConRolSinOpciones'));
                        else
                            vm.flujosAutorizados = flujosAutorizados;
                    }
                }
            )

        }

        function cargarFlujos() {
            return flujoServicios.obtenerFlujosPorRoles().then(
                function (flujos) {
                    return flujos.filter(filtrarFlujosPorTipoObjetoSeleccionado);
                },
                function (error) {
                    if (error) {
                        if (error.status) {
                            switch (error.status) {
                                case 401:
                                    utilidades.mensajeError($filter('language')('ErrorUsuarioSinPermisoAccion'));
                                    break;
                                case 500:
                                    utilidades.mensajeError($filter('language')('ErrorObtenerDatos'));
                                    break;
                                case 404:
                                    return [];
                            }
                            return;
                        }
                    }
                }
            );

            ////////////
            function filtrarFlujosPorTipoObjetoSeleccionado(flujo) {
                return flujo.TipoObjeto && flujo.TipoObjeto.Id === estadoAplicacionServicios.tipoObjetoSeleccionado.Id && flujo.Activo !== false;
            };
        }

        function mostrarModal(flujoSeleccionado) {

            autorizacionServicios.obtenerEntidadesPorRoles(flujoSeleccionado.Roles)
                .then(crearModal)
                .then(invocarCrearInstancia)
                .catch(controlarError)

            ////////////
            function crearModal(entidades) {
                if (!entidades || entidades.length === 0)
                    throw new Error($filter('language')('ErrorRolesSinEntidades'));

                switch (flujoSeleccionado.TipoObjeto.Id) {
                    case idTipoProyecto:
                        modal = modalCrearInstanciaProyecto(entidades, flujoSeleccionado.NombreOpcion);
                        break;

                    case idTipoTramite:
                        modal = modalCrearInstanciaTramite(entidades);
                        break;

                    default:
                        throw new Error('No se puede abrir el modal.');
                }

                return modal;
            }

            function invocarCrearInstancia(modalDatos) {
                return crearInstancia(flujoSeleccionado, modalDatos);
            }

            function controlarError(error) {
                if (error) {
                    if (error.status) {
                        switch (error.status) {
                            case 401:
                                return utilidades.mensajeError($filter('language')('ErrorUsuarioSinPermisoAccion'));
                            case 404:
                                return;
                            case 500:
                                return utilidades.mensajeError($filter('language')('ErrorObtenerDatos'));
                        }
                    } else {
                        if (error.message)
                            utilidades.mensajeError(error.message);
                        else
                            utilidades.mensajeError(error);
                    }
                }
            }
        };

        function mostrarNotifacion(notificacion) {
            window.location.href = "/notificaciones/" + notificacion.IdNotificacion;
        };

        function cargarNotificaciones() {
            serviciosComponenteNotificaciones.obtenerNotificacionesSinLeer().then(
                function (result) {
                    vm.notificaciones = result.data;
                    vm.cantidadDeNotificaciones = vm.notificaciones.length;
                },
                function (error) { //toastr.error(`Ocurrió un error al cargar el número de notificaciones ${error}`) 
                }
            );
        }

        function modalCrearInstanciaProyecto(entidades, nombreOpcion) {
            var idsEntidadesMga = entidades.map(function (entidad) {
                return entidad.IdEntidadMGA;
            });
           
            return flujoServicios.obtenerProyectosPorEntidadesyEstados(idsEntidadesMga).then(mostrarModalProyectos, mostrarModalProyectos);

            function mostrarModalProyectos(proyectos) {
                if (proyectos !== undefined) {
                    if (!Array.isArray(proyectos)) {
                        if (proyectos.status !== undefined && proyectos.status === 404) { //No se encontraron resultados
                            proyectos = [];
                        } else {
                            return $q.reject(proyectos); //Retornamos la excepcion ocurrida
                        }
                    }
                }

                var modalConfig = {
                    templateUrl: '/src/app/comunes/templates/modales/seleccionarProyectos/seleccionarProyectos.html',
                    controller: 'seleccionarProyectosCtrl',
                    size: 'xl',
                    resolve: {
                        proyectos: $q.resolve(proyectos),
                        nombreOpcion: $q.resolve(nombreOpcion),
                        idsEntidadesMga: $q.resolve(idsEntidadesMga)
                    }
                }
                modalConfig = Object.assign({}, modalConfigDefault, modalConfig);
                var instanciaModal = $uibModal.open(modalConfig);

                return instanciaModal.result;
            }
        }

        function modalCrearInstanciaTramite(entidades) {
            var modalConfig = {
                templateUrl: '/src/app/comunes/templates/modales/seleccionarEntidades/seleccionarEntidades.html',
                controller: 'seleccionarEntidadesController',
                resolve: {
                    entidades: $q.resolve(entidades)
                }
            }
            modalConfig = Object.assign({}, modalConfigDefault, modalConfig);
            var instanciaModal = $uibModal.open(modalConfig);

            return instanciaModal.result;
        }

        function crearInstancia(flujo, datos) {
            var usuarioRolId = sesionServicios.obtenerUsuarioIdsRoles()[0];

            var instanciaDto = {
                FlujoId: flujo.IdOpcionDnp,
                ObjetoId: datos.CodigoBpin || datos.IdEntidadMGA,
                UsuarioId: usuarioDNP,
                RolId: usuarioRolId,
                TipoObjetoId: flujo.TipoObjeto.Id,
                ListaEntidades: [datos.EntidadId || datos.IdEntidadMGA]
            }

            flujoServicios.crearInstancia(instanciaDto).then(
                function (resultado) {
                    if (!resultado.length) {
                        utilidades.mensajeError('No se creó instancia');
                        return;
                    }

                    var instanciasFallidas = resultado.filter(function (instancia) {
                        return !instancia.Exitoso;
                    });
                    var cantidadInstanciasFallidas = instanciasFallidas.lenght;

                    if (cantidadInstanciasFallidas) {
                        utilidades.mensajeError('Se crearon ' + (resultado.length - cantidadInstanciasFallidas).toString() + ' instancias de ' + resultado.length.toString());
                    } else {
                        utilidades.mensajeSuccess('Se crearon instancias exitosamente');
                    }
                },
                function (error) {
                    if (error) {
                        utilidades.mensajeError(error);
                    }
                }
            );
        }

        function tienePermisos() {
            var rolesUsuario = sesionServicios.obtenerUsuarioRoles();
            if (rolesUsuario.length > 0) {
                if ($location.$$url == "/tramites" || $location.$$url == "/")
                    return true;
            }
            return false;
        }

        vm.tienesAlertas = function () {
            return vm.cantidadDeAlertas > 0;
        }

        vm.tienesNotificaciones = function () {
            return vm.cantidadDeNotificaciones > 0;
        }

        vm.tienesMensajes = function () {
            return vm.cantidadDeMensajes > 0;
        }

        vm.redireccionarUrl = function () {
            if (!vm.proyectosIds.length)
                return;

            const url = "/tableroControlProyectos"
            const tieneParametros = ($location.url().split("?") || []).length >= 2;

            if (tieneParametros)
                $route.reload();

            const proyectosIdsConAlertas = vm.proyectosIds.filter(x => x.TieneAlerta).map(x => x.id);
            const parametros = encodeURIComponent(proyectosIdsConAlertas.join(","));
            $location.path(url).search({
                params: parametros
            });
        }

        async function obtenerNotificacionesCantidadDeAlertas() {
            vm.cantidadDeAlertas = 0;
            await serviciosComponenteNotificaciones.obtenerNotificacionesCantidadAlertas()
                .then(exito)
                .catch(error);

            function exito(respuesta) {
                const data = respuesta || [];
                vm.notificacionesCargando = false;

                if (!data.length) {
                    vm.cantidadDeAlertas = 0;
                    return;
                }

                vm.cantidadDeNotificaciones += vm.cantidadDeAlertas += data.filter(x => x.TieneAlerta).length;
                vm.proyectosIds = data;
            }

            function error(error) {
                //toastr.error("Se produjo un error al cargar las notificaciones de alerta.");
                vm.cantidadDeAlertas = 0;
            }
        }

        function cargarFlujosTramites() {
            return flujoServicios.obtenerFlujosPorRoles().then(
                function (flujos) {

                    flujos = flujos.filter(filtrarFlujosPorTipoObjetoSeleccionado);

                    var listaAuxFlujos = [];

                    flujos.forEach(element => {
                        listaAuxFlujos.push({
                            id: element.IdOpcionDnp,
                            nombre: element.NombreOpcion
                        })
                    });
                    vm.flujos = flujos;
                    vm.tramites = listaAuxFlujos;
                },
                function (error) {
                    if (error) {
                        if (error.status) {
                            switch (error.status) {
                                case 401:
                                    utilidades.mensajeError($filter('language')('ErrorUsuarioSinPermisoAccion'));
                                    break;
                                case 500:
                                    utilidades.mensajeError($filter('language')('ErrorObtenerDatos'));
                                    break;
                                case 404:
                                    return [];
                            }
                            return;
                        }
                    }
                }
            );
            ////////////
            function filtrarFlujosPorTipoObjetoSeleccionado(flujo) {
                return flujo.TipoObjeto && flujo.TipoObjeto.Id === idTipoTramite;
            };
        }

        function listarFiltroEntidades() {
            autorizacionServicios.obtenerListaEntidad(usuarioDNP).then(exito, error);

            function exito(respuesta) {

                var listaAuxEntidades = [];

                respuesta.forEach(element => {
                    listaAuxEntidades.push({
                        id: element.EntityTypeCatalogOptionId,
                        nombre: element.Entidad
                    })
                });
                vm.listaEntidades = listaAuxEntidades;
            }

            function error() {
                vm.listaEntidades = [];
            }
        }

        function guardarSolicitud() {

            if (!vm.objSolicitud.codTramite) {
                utilidades.mensajeError('El campo Tipo Tramite es obligatorio', false);
                return;
            }
            if (!vm.objSolicitud.codEntidade) {
                utilidades.mensajeError('El campo Entidad es obligatorio', false);
                return;
            }

            var usuarioRolId = sesionServicios.obtenerUsuarioIdsRoles()[0];

            var instanciaDto = {
                FlujoId: vm.objSolicitud.codTramite,
                ObjetoId: vm.objSolicitud.codEntidade,
                UsuarioId: usuarioDNP,
                RolId: usuarioRolId,
                TipoObjetoId: idTipoTramite,
                ListaEntidades: [vm.objSolicitud.codEntidade],
                Descripcion: vm.objSolicitud.descripcion
            }

            flujoServicios.crearInstancia(instanciaDto).then(
                function (resultado) {
                    if (!resultado.length) {
                        utilidades.mensajeError('No se creó instancia');
                        return;
                    }

                    vm.cerrarTramite();
                    utilidades.mensajeSuccess('Se crearon instancias exitosamente');
                    $("#tramits").scope().vm.obtenerInbox(idTipoTramite);
                    $("#cantidadTramites").scope().vm.obtenerNotificacionesCantidadDeTramites();
                    $("#tramitesCantidad").scope().vm.obtenerNotificacionesCantidadDeTramites();
                },
                function (error) {
                    if (error) {
                        utilidades.mensajeError(error);
                    }
                }
            );
        }

        function cerrarTramite() {
            vm.objSolicitud = {
                codTramite: null,
                codEntidade: null,
                descripcion: null
            };
            vm.popOverOptionsCrearTramite.toggle();
        }

        function cargarNotificacionesMensajes() {
            servicioNotificacionesMensajes.obtenerListaMensajesNotificaciones({ estado: null, fecha: null, notificacion: null })
                .then(result => {
                    vm.cantidadDeMensajes = result.data.length;
                }, error => {
                   // toastr.error('Error al cargar la información');
                });
        }
    }
})(usuarioDNP, idTipoProyecto, idTipoTramite);