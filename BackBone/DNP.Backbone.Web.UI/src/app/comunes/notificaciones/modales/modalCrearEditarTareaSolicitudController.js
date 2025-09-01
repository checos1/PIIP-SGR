
(function () {
    'use strict';

    modalCrearEditarTareaSolicitudController.$inject = [
        '$scope',
        '$uibModalInstance',
        'utilidades',
        'autorizacionServicios',
        'flujoServicios',
        'estadoAplicacionServicios',
        'sesionServicios'
    ];

    function modalCrearEditarTareaSolicitudController(
        $scope,
        $uibModalInstance,
        utilidades,
        autorizacionServicios,
        flujoServicios,
        estadoAplicacionServicios,
        sesionServicios
    ) {
        var vm = this;
        vm.peticion;
        vm.listaEntidades = [];
        vm.listarFiltroEntidades = listarFiltroEntidades;
        vm.cerrar = $uibModalInstance.dismiss;
        vm.guardarSolicitud = guardarSolicitud;
        vm.cargarFlujos = cargarFlujos;
        vm.tramites = [];
        vm.flujos;

        vm.objSolicitud = {
            codTramite: null,
            codEntidade: null,
            descripcion: null
        };
        
        /// Comienzo
        vm.$onInit = function () {
            listarFiltroEntidades();
            cargarFlujos();
        }

        function cargarFlujos() {
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

                    vm.cerrar();
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
    }

    angular.module('backbone').controller('modalCrearEditarTareaSolicitudController', modalCrearEditarTareaSolicitudController);
})();