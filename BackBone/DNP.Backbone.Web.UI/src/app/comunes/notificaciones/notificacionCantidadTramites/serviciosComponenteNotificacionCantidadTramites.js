(function () {
    'use strict';

    angular.module('backbone').factory('serviciosComponenteNotificacionCantidadTramites', serviciosComponenteNotificacionCantidadTramites);


    serviciosComponenteNotificacionCantidadTramites.$inject = ['$http', '$location', 'constantesBackbone', 'sesionServicios'];


    function serviciosComponenteNotificacionCantidadTramites($http, $location, constantesBackbone, sesionServicios) {

        return {
            obtenerNotificacionesCantidadDeTramites: obtenerNotificacionesCantidadDeTramites
        }

        function obtenerNotificacionesCantidadDeTramites() {
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
