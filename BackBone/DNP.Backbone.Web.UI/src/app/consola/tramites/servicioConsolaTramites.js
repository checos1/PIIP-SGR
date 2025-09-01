(function () {
    'use strict';

    angular.module('backbone').factory('servicioConsolaTramites', servicioConsolaTramites);

    servicioConsolaTramites.$inject = ['$q', '$http', '$location', 'constantesBackbone'];

    var columnasPorDefecto = ['Descripción', 'Fecha', 'Entidad', 'Accion Flujo', 'Nombre Flujo'];
    var columnasDisponibles = ['Sector'];

    function servicioConsolaTramites($q, $http, $location, constantesBackbone) {

        return {
            columnasDisponibles: columnasDisponibles,
            columnasPorDefecto: columnasPorDefecto,
            obtenerPdf: obtenerPdf,
            imprimirPdf: imprimirPdf,
            obtenerTramites: obtenerTramites,
            obtenerExcel: obtenerExcel,
            obtenerProyectosPorTramite: obtenerProyectosPorTramite,
            obtenerPdfProyectosTramites: obtenerPdfProyectosTramites,
            imprimirPdfProyectosTramites: imprimirPdfProyectosTramites,
            obtenerExcelProyectosTramites: obtenerExcelProyectosTramites,
            obtenerDocumentos: obtenerDocumentos,
            obtenerDocumento: obtenerDocumento,
            obtenerIdAplicacion,
        }

        function obtenerDocumentos(idInstancia) {
            return $http.post(apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerConsolaTramiteDocumentos +"?idInstancia="+ idInstancia);
        }

        function obtenerDocumento(id) {
            return $http.get(apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerConsolaTramiteDocumento + "?id=" + id);
        }

        function obtenerExcel(peticionObtenerInbox, tramiteFiltro, columnasVisibles) {

            var listaFiltrados = [];
            Object.keys(tramiteFiltro).forEach(filtro => {
                if (tramiteFiltro[`${filtro}`].valor) {
                    listaFiltrados.push(tramiteFiltro[`${filtro}`]);
                }
            });

            const tramiteDto = {
                parametrosInboxDto: peticionObtenerInbox,
                tramiteFiltroDto: {
                    tokenAutorizacion: '',
                    idUsuarioDNP: peticionObtenerInbox.IdUsuario,
                    filtroGradeDtos: listaFiltrados,
                    IdsRoles: peticionObtenerInbox.ListaIdsRoles,
                    IdTipoObjetoNegocio: peticionObtenerInbox.IdObjeto
                },
                columnasVisibles: columnasVisibles
            };

            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerExcelTramites;
            return $http.post(url, tramiteDto, { responseType: 'arraybuffer' });
        }   

        function obtenerPdf(peticionObtenerInbox, tramiteFiltro, columnasVisibles) {
            var listaFiltrados = [];
            Object.keys(tramiteFiltro).forEach(filtro => {
                if (tramiteFiltro[`${filtro}`].valor) {
                    listaFiltrados.push(tramiteFiltro[`${filtro}`]);
                }
            });

            const tramiteDto = {
                parametrosInboxDto: peticionObtenerInbox,
                tramiteFiltroDto: {
                    tokenAutorizacion: '',
                    idUsuarioDNP: peticionObtenerInbox.IdUsuario,
                    filtroGradeDtos: listaFiltrados,
                    IdsRoles: peticionObtenerInbox.ListaIdsRoles,
                    IdTipoObjetoNegocio: peticionObtenerInbox.IdObjeto
                },
                columnasVisibles: columnasVisibles
            };

            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneConsolaTramitesObtenerPdf;

            return $http.post(url, tramiteDto);
        }

        function imprimirPdf(inboxDto) {
            var url = urlPDFBackbone + constantesBackbone.apiBackboneConsolaTramitesImprimirPDF;
            return $http.post(url, inboxDto, { responseType: 'blob' });
        }

        function obtenerTramites(peticionObtenerInbox, tramiteFiltro, columnasVisibles) {
            var listaFiltrados = [];
            Object.keys(tramiteFiltro).forEach(filtro => {
                if (tramiteFiltro[`${filtro}`].valor) {
                    listaFiltrados.push(tramiteFiltro[`${filtro}`]);
                }
            });

            const tramiteDto = {
                parametrosInboxDto: peticionObtenerInbox,
                tramiteFiltroDto: {
                    tokenAutorizacion: '',
                    idUsuarioDNP: peticionObtenerInbox.IdUsuario,
                    filtroGradeDtos: listaFiltrados,
                    IdsRoles: peticionObtenerInbox.ListaIdsRoles,
                    IdTipoObjetoNegocio: peticionObtenerInbox.IdObjeto
                },
                columnasVisibles: columnasVisibles
            };

            return $http.post(apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerConsolaTramite, tramiteDto);
        }

        function obtenerProyectosPorTramite(peticionObtenerInbox, tramiteId) {
            const tramiteDto = {
                parametrosInboxDto: peticionObtenerInbox,
                tramiteFiltroDto: {
                    tokenAutorizacion: '',
                    idUsuarioDNP: peticionObtenerInbox.IdUsuario,
                    tramiteId: tramiteId,
                    filtroGradeDtos: [],
                    InstanciaId: tramiteId
                },
                columnasVisibles: []
            };

            return $http.post(apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerProyectosPorTramiteConsola, tramiteDto);
        }

        function obtenerPdfProyectosTramites(peticionObtenerInbox, idTramite) {
            var tramiteFiltroDto =
            {
                IdUsuarioDNP: peticionObtenerInbox.IdUsuarioDNP,
                TramiteId: idTramite,
                InstanciaId: idTramite
            }
            const inboxDto = {
                parametrosInboxDto: peticionObtenerInbox,
                tramiteFiltroDto: tramiteFiltroDto
            };

            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneProyectosTramitesPdfConsola;
            return $http.post(url, inboxDto);
        }

        function imprimirPdfProyectosTramites(inboxDto) {
            var url = urlPDFBackbone + constantesBackbone.apiBackboneProyectosTramitesImprimirPDFConsola;
            return $http.post(url, inboxDto, { responseType: 'blob' });
        }

        function obtenerExcelProyectosTramites(peticionObtenerInbox, idTramite) {
            const tramiteDto = {
                parametrosInboxDto: peticionObtenerInbox,
                tramiteFiltroDto: {
                    tokenAutorizacion: '',
                    idUsuarioDNP: peticionObtenerInbox.IdUsuario,
                    TramiteId: idTramite,
                    filtroGradeDtos: [],
                    InstanciaId: idTramite
                },
                columnasVisibles: []
            };


            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerExcelProyectosTramitesConsola;
            return $http.post(url, tramiteDto, { responseType: 'arraybuffer' });
        }

        function obtenerIdAplicacion(idInstancia) {
            return $http.get(apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerIdAplicacionPorInstancia + "?idInstancia=" + idInstancia);
        }
    }

})();