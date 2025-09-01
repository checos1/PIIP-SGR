(function () {
    'use strict';

    angular.module('backbone').factory('serviciosComponenteNotificacionCantidadAlertas', serviciosComponenteNotificacionCantidadAlertas);


    serviciosComponenteNotificacionCantidadAlertas.$inject = ['$http', '$location', 'constantesBackbone', 'sesionServicios'];


    function serviciosComponenteNotificacionCantidadAlertas($http, $location, constantesBackbone, sesionServicios) {

        return {
            obtenerNotificacionesCantidadDeAlertas: obtenerNotificacionesCantidadDeAlertas
        }

        function obtenerNotificacionesCantidadDeAlertas() {
            var peticionObtenerInbox = {
                // ReSharper disable once UndeclaredGlobalVariableUsing
                IdUsuario: usuarioDNP,
                IdObjeto: idTipoTramite,
                // ReSharper disable once UndeclaredGlobalVariableUsing
                Aplicacion: nombreAplicacionBackbone,
                ListaIdsRoles: sesionServicios.obtenerUsuarioIdsRoles()
            };

            const tramiteDto = {
                parametrosInboxDto: peticionObtenerInbox,
                tramiteFiltroDto: {
                    tokenAutorizacion: '',
                    idUsuarioDNP: peticionObtenerInbox.IdUsuario,
                    filtroGradeDtos: [],
                    IdTipoObjetoNegocio: peticionObtenerInbox.IdObjeto,
                    IdsRoles: peticionObtenerInbox.ListaIdsRoles
                },
                columnasVisibles: []
            };

            return $http.post(apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerTramite + "?noblockui", tramiteDto);
        }
    }
})();
