(function () {
    'use strict';

    angular.module('backbone').factory('serviciosComponenteNotificacionCantidadProyectos', serviciosComponenteNotificacionCantidadProyectos);


    serviciosComponenteNotificacionCantidadProyectos.$inject = ['$http', '$location', 'constantesBackbone', 'sesionServicios'];


    function serviciosComponenteNotificacionCantidadProyectos($http, $location, constantesBackbone, sesionServicios) {

        return {
            obtenerNotificacionesCantidadDeProyectos: obtenerNotificacionesCantidadDeProyectos,
            obtenerMacroProcesos
        }

        function obtenerNotificacionesCantidadDeProyectos() {
            
            var proyectoFiltro = {                
                bpin: null                
            };

            var peticionObtenerInbox = {
                // ReSharper disable once UndeclaredGlobalVariableUsing
                IdUsuario: usuarioDNP,
                IdObjeto: idTipoProyecto,
                // ReSharper disable once UndeclaredGlobalVariableUsing
                Aplicacion: nombreAplicacionBackbone,
                ListaIdsRoles: sesionServicios.obtenerUsuarioIdsRoles()
            };

            const inboxDto = {
                ProyectoParametrosDto: peticionObtenerInbox,
                ProyectoFiltroDto: proyectoFiltro,
                columnasVisibles: []
            };

            return $http.post(apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerProyecto + "?noblockui", inboxDto);
        }

        function obtenerMacroProcesos() {
            return $http.get(apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerMacroProcesosCantidad);
        }
    }
})();
