(function () {
    'use strict';
    angular.module('backbone').factory('calendarioServicio', calendarioServicio);
    calendarioServicio.$inject = ['$http', 'constantesBackbone'];

    function calendarioServicio($http, constantesBackbone) {
        return {
            catalogoTodosSectores: catalogoTodosSectores,
            ObtenerEntidadesSector: ObtenerEntidadesSector,
            ObtenerCalendarioProgramacion: ObtenerCalendarioProgramacion,
            RegistrarCalendarioProgramacion: RegistrarCalendarioProgramacion,
        };

        function catalogoTodosSectores(sectorId) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneProgramacionSectores + "?sectorId=" + sectorId;
            return $http.get(url);
        }

        function ObtenerEntidadesSector(sectorId) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneProgramacionEntidadesSector + "?sectorId=" + sectorId;
            return $http.get(url);
        }

        function ObtenerCalendarioProgramacion(FlujoId) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneProgramacionObtenerCalendarioProgramacion + "?FlujoId=" + FlujoId;
            return $http.get(url);
        }

        function RegistrarCalendarioProgramacion(calendarioProgramacionDto) {
            return $http.post(apiBackboneServicioBaseUri + constantesBackbone.apiBackboneProgramacionRegistrarCalendarioProgramacion, calendarioProgramacionDto);
        }
    

    }

})();