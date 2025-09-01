(function () {
    'use strict';

    angular.module('backbone').factory('servicioConsolaReportes', servicioConsolaReportes);

    servicioConsolaReportes.$inject = ['$q', '$http', '$location', 'constantesBackbone'];


    function servicioConsolaReportes($q, $http, $location, constantesBackbone) {

        return {
            obtenerAgrupadorReportes: obtenerAgrupadorReportes,
            obtenerListadoReportes: obtenerListadoReportes,
            ObtenerFiltrosReportes: ObtenerFiltrosReportes,
            ObtenerDatosReportePIIP: ObtenerDatosReportePIIP
        }

        function obtenerAgrupadorReportes(usuarioDnp) {

            return $http.get(apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerAgrupadorReportes + "?usuarioDnp=" + usuarioDnp);
        }

        function obtenerListadoReportes(usuarioDnp, idRoles) {

            //return $http.get(apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerListadoReportes + "?usuarioDnp=" + usuarioDnp + "&idRoles=" + idRoles);
            var lsTemp = JSON.stringify(idRoles);
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerListadoReportes;
            var params = {
                'usuarioDnp': usuarioDnp,
                'idRoles': lsTemp,
            };
            return $http.get(url, { params });
        }

        function ObtenerFiltrosReportes(idReporte, usuarioDnp) {

            return $http.get(apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerFiltrosReportesPIIP + "?idReporte=" + idReporte + "&usuarioDnp=" + usuarioDnp);
        }

        function ObtenerDatosReportePIIP(idReporte, filtros, usuarioDnp, detalleReporte, idEntidades) {

            if (detalleReporte.esquema == constantesBackbone.apiEsquemaAutorizacion)
                return $http.get(apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerDatosReportePIIP + "?idReporte=" + idReporte + '&filtros=' + filtros + "&usuarioDnp=" + usuarioDnp + "&idEntidades=" + idEntidades);
            //return $http.get(apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerDatosReportePIIP + "?idReporte=" + idReporte + '&filtros=' + JSON.stringify(filtros) + "&usuarioDnp=" + usuarioDnp + "&idEntidades=" + idEntidades);
            else if (detalleReporte.otroEsquema && detalleReporte.esquema == constantesBackbone.apiEsquemaMga) {
                //Guid idReporte, string filtros, string usuarioDNP, string idEntidades, string tokenAutorizacion
                //return $http.get(apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerDatosReportePIIPMga + "?idReporte=" + idReporte + '&filtros=' + JSON.stringify(filtros) + "&usuarioDnp=" + usuarioDnp + "&idEntidades=" + idEntidades + "&tokenAutorizacion=" + "tokenAutorizacion");
                return $http.get(apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerDatosReportePIIPMga + "?idReporte=" + idReporte + '&filtros=' + filtros + "&usuarioDnp=" + usuarioDnp + "&idEntidades=" + idEntidades + "&tokenAutorizacion=" + "tokenAutorizacion");
            }
            else if (detalleReporte.otroEsquema && detalleReporte.esquema == constantesBackbone.apiEsquemaFlujos) {
                return null;
            }

        }

    }

})();