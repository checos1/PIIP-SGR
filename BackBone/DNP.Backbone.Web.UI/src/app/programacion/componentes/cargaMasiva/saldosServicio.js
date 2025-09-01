(function () {
    'use strict';
    angular.module('backbone').factory('saldosServicio', saldosServicio);
    saldosServicio.$inject = ['$http', 'constantesBackbone'];

    function saldosServicio($http, constantesBackbone) {
        return {
            catalogoTodosSectores: catalogoTodosSectores,
            ObtenerEntidadesSector: ObtenerEntidadesSector,
            ObtenerProyectosGenerarPresupuestal: ObtenerProyectosGenerarPresupuestal,
            ObtenerCargaMasivaCuotas: ObtenerCargaMasivaCuotas,
            RegistrarProyectosSinPresupuestal: RegistrarProyectosSinPresupuestal,
            ValidarConsecutivoPresupuestal: ValidarConsecutivoPresupuestal,
            registrarCargaMasivaSaldos: registrarCargaMasivaSaldos,
            obtenerLogErrorCargaMasivaSaldos: obtenerLogErrorCargaMasivaSaldos,
            obtenerCargaMasivaSaldos: obtenerCargaMasivaSaldos,
            obtenerTipoCargaMasiva: obtenerTipoCargaMasiva,
            validarCargaMasiva: validarCargaMasiva,
            obtenerDetalleCargaMasivaSaldos : obtenerDetalleCargaMasivaSaldos
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

        function registrarCargaMasivaSaldos(TipoCargueId) {
            try {
                var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneRegistrarCargaMasivaSaldos;
                url = url + '?TipoCargueId=' + parseInt(TipoCargueId);
                return $http.get(url);
            }
            catch (exception) {
                throw { message: `registro_cargue_saldos: ${exception.message}` };
            }
        }

        function obtenerLogErrorCargaMasivaSaldos(tipoCargueDetalleId, carguesIntegracionId) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackBoneObtenerLogErrorCargaMasivaSaldos + "?tipoCargueDetalleId=" + tipoCargueDetalleId + "&carguesIntegracionId=" + carguesIntegracionId ;
            return $http.get(url);
        }

        function obtenerCargaMasivaSaldos(tipoCargue) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerCargaMasivaSaldos + "?tipoCargue=" + tipoCargue ;
            return $http.get(url);
        }

        function obtenerTipoCargaMasiva(tipoCargue) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerTipoCargaMasiva + "?tipoCargue=" + tipoCargue;
            return $http.get(url);
        }

        function validarCargaMasiva(jsonListaRegistros) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneValidarCargaMasiva;
            var rta = $http.post(url, jsonListaRegistros);
            return rta;
        }

        function obtenerDetalleCargaMasivaSaldos(id) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneDetalleCargaMasivaSaldos;
            url = url + '?CargueId=' + id;
            var rta = $http.get(url);
            return rta;
        }
        
     
    }

})();