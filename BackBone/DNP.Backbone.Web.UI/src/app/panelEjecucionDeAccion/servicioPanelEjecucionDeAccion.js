(function () {
    'use strict';

    angular.module('backbone').factory('servicioPanelEjecucionDeAccion', servicioPanelEjecucionDeAccion);

    servicioPanelEjecucionDeAccion.$inject = ['$http', '$location','constantesBackbone'];

    function servicioPanelEjecucionDeAccion($http, $location, constantesBackbone) {

        return {
            actualizarEstadoDelProyecto: actualizarEstadoDelProyecto
        }
        
        function actualizarEstadoDelProyecto(bpin, estado) {
// ReSharper disable UndeclaredGlobalVariableUsing
            var params = '?bpin=' + bpin + '&estado=' + estado + '&usuario=' + usuarioDNP;
            return $http.post(apiBackboneServicioBaseUri + constantesBackbone.apiBackboneActualizarEstadoProyecto + params);
                      
        }


        
    }

})();
