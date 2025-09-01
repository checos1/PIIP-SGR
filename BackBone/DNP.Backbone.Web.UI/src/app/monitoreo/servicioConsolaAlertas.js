(function () {
    'use strict';

    angular.module('backbone').factory('servicioConsolaAlertas', servicioConsolaAlertas);

    servicioConsolaAlertas.$inject = [
        '$q', 
        '$http', 
        '$location', 
        'constantesBackbone'
    ];

    function servicioConsolaAlertas(
        $q, 
        $http, 
        $location, 
        constantesBackbone) {

        return {
            obtenerConsolaAlertas: obtenerConsolaAlertas,
            obtenerPdfConsolaAlertas: obtenerPdfConsolaAlertas,
            imprimirPdfConsolaAlertas: imprimirPdfConsolaAlertas,
            obtenerExcelConsolaAlertas: obtenerExcelConsolaAlertas,
            obtenerAlertaPorId: obtenerAlertaPorId,
            obtenerListaTipoAlerta: obtenerListaTipoAlerta,
            obtenerListaEstado: obtenerListaEstado,
            obtenerListaColumnas: obtenerListaColumnas,
            guardarAlertaConfig: guardarAlertaConfig,
            eliminarAlertaConfiguracion: eliminarAlertaConfiguracion,
            listarAlertasPorProyectoId: listarAlertasPorProyectoId
        }

        function obtenerConsolaAlertas(peticion, filtro) {
            var listaFiltrados = [];
            Object.keys(filtro).forEach(f => {
                if (filtro[`${f}`].valor) {
                    listaFiltrados.push(filtro[`${f}`]);
                }
            });

            const alertasConfigFiltroDto = {
                ParametrosDto: peticion,
                FiltroGradeDtos: listaFiltrados
            };
            
            return $http.post(apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerConsolaAlertas, alertasConfigFiltroDto);
        }

        function obtenerPdfConsolaAlertas(peticion, filtro) {
            var listaFiltrados = [];
            Object.keys(filtro).forEach(f => {
                if (filtro[`${f}`].valor) {
                    listaFiltrados.push(filtro[`${f}`]);
                }
            });

            const alertasConfigFiltroDto = {
                ParametrosDto: peticion,
                FiltroGradeDtos: listaFiltrados
            };
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerPdfConsolaAlertas;
            return $http.post(url, alertasConfigFiltroDto);
 
        }

        function imprimirPdfConsolaAlertas(inboxDto) {
            var url = urlPDFBackbone + constantesBackbone.apiBackboneConsolaAlertasImprimirPDF;
            return $http.post(url, inboxDto, { responseType: 'blob' });
        }

        function obtenerExcelConsolaAlertas(peticion, filtro) {
            var listaFiltrados = [];
            Object.keys(filtro).forEach(f => {
                if (filtro[`${f}`].valor) {
                    listaFiltrados.push(filtro[`${f}`]);
                }
            });

            const alertasConfigFiltroDto = {
                ParametrosDto: peticion,
                FiltroGradeDtos: listaFiltrados
            };

            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerExcelConsolaAlertaConfig;
            return $http.post(url, alertasConfigFiltroDto, { responseType: 'arraybuffer' });
        }

        function obtenerAlertaPorId(peticion, idAlerta){
            
            var listaFiltrados = [];

            listaFiltrados.push(
            {
                Campo: "Id",
                Valor: idAlerta,
                Tipo: 0,
                Combinacao: 0
            });

            const alertasConfigFiltroDto = {
                ParametrosDto: peticion,
                FiltroGradeDtos: listaFiltrados
            };

            return $http.post(apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerConsolaAlertas, alertasConfigFiltroDto);
            
        }

        function obtenerListaTipoAlerta(peticion) {
            return $http.post(apiBackboneServicioBaseUri + constantesBackbone.apiBackboneConsolaAlertasListaTipoAlerta, peticion);
        }

        function obtenerListaColumnas(peticion) {
            const mapColumnasFiltroDto = {
                ParametrosDto: peticion
            };
            return $http.post(apiBackboneServicioBaseUri + constantesBackbone.apiBackboneConsolaAlertasListaColumnas, mapColumnasFiltroDto);
        }

        function obtenerListaEstado(peticion) {
            return $http.post(apiBackboneServicioBaseUri + constantesBackbone.apiBackboneConsolaAlertasListaEstado, peticion);
        }

        function guardarAlertaConfig(peticion, model) {
            const alertasConfigFiltroDto = {
                ParametrosDto: peticion,
                AlertasConfigDto: model
            };
            
            return $http.post(apiBackboneServicioBaseUri + constantesBackbone.apiBackboneConsolaCrearActualizarAlertaConfig, alertasConfigFiltroDto);
        }

        function eliminarAlertaConfiguracion(peticion, idAlerta) {
            const alertasConfigFiltroDto = {
                ParametrosDto: peticion,
                AlertasConfigDto: { Id: idAlerta }
            };

            return $http.post(apiBackboneServicioBaseUri + constantesBackbone.apiBackboneEliminarConsolaAlertas, alertasConfigFiltroDto);
        }

        function listarAlertasPorProyectoId(peticion, proyectoId){
            const filtro = {
                ParametrosDto: peticion,
                FiltroGradeDtos: [
                    {
                        Campo: "ProyectoId",
                        Valor: proyectoId,
                        Tipo: 0,
                        Combinacao: 0
                    }
                ]
            };

            const url = `${apiBackboneServicioBaseUri}${constantesBackbone.apiBackboneObtenerAlertasGeneradas}`;

            return $http.post(url, filtro);
        }

    }

})();