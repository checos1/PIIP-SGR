(function () {
    'use strict';

    angular.module('backbone').factory('servicioConsolaMonitoreo', servicioConsolaMonitoreo);

    servicioConsolaMonitoreo.$inject = ['$q', '$http', '$location', 'constantesBackbone'];

    var columnasPorDefecto = ['BPIN', 'Nombre Proyecto', 'Estado', 'Avance Financiero', 'Avance Fisico', 'Avance Proyecto', 'Duración', 'Periodo Ejecución'];
    var columnasDisponibles = ['Sector'];

    function servicioConsolaMonitoreo($q, $http, $location, constantesBackbone) {

        return {
            obtenerConsolaMonitoreo: obtenerConsolaMonitoreo,
            obtenerPdfConsolaMonitoreo: obtenerPdfConsolaMonitoreo,
            imprimirPdfConsolaMonitoreo: imprimirPdfConsolaMonitoreo,
            obtenerExcelConsolaMonitoreo: obtenerExcelConsolaMonitoreo,
            obtenerConsolaMonitoreoReportes: obtenerConsolaMonitoreoReportes,
            obtenerConsolaMonitoreoDashboards: obtenerConsolaMonitoreoDashboards,
            obtenerReportesPowerBI: obtenerReportesPowerBI,
            descargarReportesPowerBI: descargarReportesPowerBI,
            obtenerListaFileFormat: obtenerListaFileFormat,
            obtenerCondicionesParaAccionAlertas: obtenerCondicionesParaAccionAlertas,
            columnasPorDefecto: columnasPorDefecto,
            columnasDisponibles: columnasDisponibles
        }

        function descargarReportesPowerBI(peticion, filtro) {
            const embedParametrosDto = {
                ParametrosInboxDto: peticion,
                EmbedFiltroDto: filtro
            };

            return $http.post(apiBackboneServicioBaseUri + constantesBackbone.apiBackboneDescargarReportesPowerBI, embedParametrosDto, { responseType: 'blob' });
        }

        function obtenerReportesPowerBI(peticion, filtro) {
            const embedParametrosDto = {
                ParametrosInboxDto: peticion,
                EmbedFiltroDto: filtro
            };

            return $http.post(apiBackboneServicioBaseUri + constantesBackbone.obtenerReportesPowerBI, embedParametrosDto);
        }

        function obtenerConsolaMonitoreo(peticion, filtro) {
            const instanciaDto = {
                ProyectoParametrosDto: peticion,
                ProyectoFiltroDto: filtro,
                columnasVisibles: []
            };
            
            return $http.post(apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerConsolaMonitoreo, instanciaDto);
        }

        function obtenerConsolaMonitoreoDashboards(peticion, filtro) {
            const embedParametrosDto = {
                ParametrosInboxDto: peticion,
                EmbedFiltroDto: filtro
            };

            return $http.post(apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerMonitoreoDashboards, embedParametrosDto);
        }

        function obtenerConsolaMonitoreoReportes(peticion, filtro) {
            const embedParametrosDto = {
                ParametrosInboxDto: peticion,
                EmbedFiltroDto: filtro
            };

            return $http.post(apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerMonitoreoReportes, embedParametrosDto);
        }

        function obtenerPdfConsolaMonitoreo(peticion, filtro, columnasVisibles) {

            const instanciaDto = {
                ProyectoParametrosDto: peticion,
                ProyectoFiltroDto: filtro,
                columnasVisibles: columnasVisibles
            };

            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerProyectosPdfConsolaMonitoreo;

            return $http.post(url, instanciaDto);
        }

        function imprimirPdfConsolaMonitoreo(inboxDto) {
            var url = urlPDFBackbone + constantesBackbone.apiBackboneConsolaMonitoreoImprimirPDF;
            
            return $http.post(url, inboxDto, { responseType: 'blob' });
        }

        function obtenerExcelConsolaMonitoreo(peticion, filtro, columnasVisibles) {
            const instanciaDto = {
                ProyectoParametrosDto: peticion,
                ProyectoFiltroDto: filtro,
                columnasVisibles: columnasVisibles
            };
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerExcelConsolaMonitoreo;
            return $http.post(url, instanciaDto, { responseType: 'arraybuffer' });
        }

        function obtenerListaFileFormat(peticion) {
            const embedParametrosDto = {
                ParametrosInboxDto: peticion
            };
            return $http.post(apiBackboneServicioBaseUri + constantesBackbone.apiBackboneConsolaMonitoreoListaFileFormat, embedParametrosDto);
        }

        function obtenerCondicionesParaAccionAlertas(peticion, ids) {
            const parametros = {
                ProyectoParametrosDto: peticion,
                ProyectoFiltroDto: {
                    ProyectosIds: ids
                },
                columnasVisibles: []
            };

            var url = `${apiBackboneServicioBaseUri}${constantesBackbone.apiBackboneObtenerSituacaoAlertasProyectos}?noblockui`;
            return $http.post(url, parametros)
        }

    }

})();