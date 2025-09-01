(function () {
    'use strict';

    angular.module('backbone').factory('documentoSoporteServicios', documentoSoporteServicios);

    documentoSoporteServicios.$inject = ['$http', 'constantesBackbone', 'utilidades'];

    function documentoSoporteServicios($http, constantesBackbone, utilidades) {

        return {
            ObtenerListaTipoDocumentosSoportePorRolTrv: ObtenerListaTipoDocumentosSoportePorRolTrv,
            GetNombreMacroproceso: GetNombreMacroproceso,
            ObtenerListadoArchivosPIIP: ObtenerListadoArchivosPIIP,
            ObtenerListadoArchivosMGA: ObtenerListadoArchivosMGA,
            ObtenerListadoArchivosSUIFP: ObtenerListadoArchivosSUIFP
        }

        function ObtenerListaTipoDocumentosSoportePorRolTrv(tipoTramiteId, rol, tramiteId, nivelId, instanciaId, accionId) {
            var url = `${apiBackboneServicioBaseUri}${constantesBackbone.apiBackbone_ObtenerListaTipoDocumentoSoporteTrv}` + '?tipoTramiteId=' + tipoTramiteId +
                "&roles=" + rol + "&tramiteId=" + tramiteId + "&nivelId=" + nivelId + "&instanciaId=" + instanciaId + "&accionId=" + accionId;
            return $http.get(url);
        }

        function ObtenerListadoArchivosPIIP(params, coleccion) {
            var urlApi = apiBackboneServicioBaseUri + constantesBackbone.apiBackbone_ObtenerListadoArchivosPIIP + coleccion;

            var config = {
                headers: { 'Content-Type': 'application/json' }
            };

            return $http.post(urlApi, JSON.stringify(params), config)
                .then(utilidades.httpRequestComplete, utilidades.httpRequestError);
        }

        function ObtenerListadoArchivosSUIFP(params) {
            return $http.post(apiBackboneServicioBaseUri + constantesBackbone.apiBackbone_ObtenerListadoArchivosSUIFP, params);
        }

        function ObtenerListadoArchivosMGA(params, coleccion) {
            var urlApi = apiBackboneServicioBaseUri + constantesBackbone.apiBackbone_ObtenerListadoArchivosMGA + coleccion;

            var config = {
                headers: { 'Content-Type': 'application/json' }
            };

            return $http.post(urlApi, JSON.stringify(params), config)
                .then(utilidades.httpRequestComplete, utilidades.httpRequestError);
        }

        function GetNombreMacroproceso(etapa) {
            var nombreEtapa = [];
            switch (etapa) {
                case 'pl':
                    nombreEtapa = "Planeacion";
                    break;
                case 'pr':
                    nombreEtapa = "Programacion";
                    break;
                case 'gr':
                    nombreEtapa = "Gestion de Recursos";
                    break;
                case 'ej':
                    nombreEtapa = "Ejecucion";
                    break;
                case 'se':
                    nombreEtapa = "Ejecucion";
                    break;
                case 'ev':
                    nombreEtapa = "Evaluacion";
                    break;
            }
            return nombreEtapa;
        }
    }
})();
