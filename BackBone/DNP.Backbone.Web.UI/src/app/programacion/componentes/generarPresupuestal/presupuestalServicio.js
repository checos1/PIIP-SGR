(function () {
    'use strict';
    angular.module('backbone').factory('presupuestalServicio', presupuestalServicio);
    presupuestalServicio.$inject = ['$http', 'constantesBackbone'];

    function presupuestalServicio($http, constantesBackbone) {
        return {
            catalogoTodosSectores: catalogoTodosSectores,
            ObtenerEntidadesSector: ObtenerEntidadesSector,
            ObtenerProyectosGenerarPresupuestal: ObtenerProyectosGenerarPresupuestal,
            ObtenerCargaMasivaCuotas: ObtenerCargaMasivaCuotas,
            RegistrarProyectosSinPresupuestal: RegistrarProyectosSinPresupuestal,
            ValidarConsecutivoPresupuestal: ValidarConsecutivoPresupuestal,
        };
      
        function catalogoTodosSectores(sectorId) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneProgramacionSectores + "?sectorId=" + sectorId;
            return $http.get(url);
        }

        function ObtenerEntidadesSector(sectorId) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneProgramacionEntidadesSector + "?sectorId=" + sectorId;
            return $http.get(url);
        }

        function ObtenerProyectosGenerarPresupuestal(sectorId, entidadId, proyectoId) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneProgramacionConsultarProyectoGenerarPresupuestal + "?sectorId=" + sectorId + "&entidadId=" + entidadId + "&proyectoId=" + proyectoId;
            return $http.get(url);
        }

        function ObtenerCargaMasivaCuotas(Vigencia, EntityTypeCatalogOptionId) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneProgramacionObtenerCargaMasivaCuotas + "?Vigencia=" + Vigencia + "&EntityTypeCatalogOptionId=" + EntityTypeCatalogOptionId;
            return $http.get(url);
        }
        function RegistrarProyectosSinPresupuestal(proyectoSinPresupuestalDto) {
            try {
                var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneProgramacionRegistrarProyectosSinPresupuestal;
                return $http.post(url, proyectoSinPresupuestalDto);
            }
            catch (exception) {
                throw { message: `presupuestalServicio.RegistrarProyectosSinPresupuestal: ${exception.message}` };
            }
        }

        function ValidarConsecutivoPresupuestal(proyectoSinPresupuestalDto) {
            try {
                var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneProgramacionValidarConsecutivoPresupuestal;
                return $http.post(url, proyectoSinPresupuestalDto);
            }
            catch (exception) {
                throw { message: `presupuestalServicio.ValidarConsecutivoPresupuestal: ${exception.message}` };
            }
        }
    }

})();