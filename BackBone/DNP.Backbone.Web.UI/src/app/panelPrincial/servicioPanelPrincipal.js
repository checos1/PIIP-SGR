(function () {
    'use strict';

    // ReSharper disable once UndeclaredGlobalVariableUsing
    angular.module('backbone').factory('servicioPanelPrincipal', servicioPanelPrincipal);

    servicioPanelPrincipal.$inject = ['$q', '$http', '$location', 'constantesBackbone', '$sessionStorage', '$timeout', 'sesionServicios'];


    // descripción de la columnas
    var columnasPorDefectoProyecto = ['Accion Flujo', 'Identificador', 'BPIN', 'Nombre', 'Entidad', 'Identificador CR', 'Año de inicio', 'Año Fin', 'Prioridad', 'Estado del Proyecto', 'Nombre Flujo', 'Sector'];
    var columnasDisponiblesProyecto = [];
    var columnasPorDefectoTramites = ['Descripción', 'Fecha', 'Entidad', 'Estado Trámite', 'Accion Flujo', 'Nombre Flujo'];
    var columnasDisponiblesTramites = ['Sector'];

    var columnasPorDefectoProyectoConsolaProcesos = ['Identificador', 'BPIN', 'Nombre', 'Entidad', 'Identificador CR', 'Año de inicio', 'Año Fin', 'Prioridad', 'Estado del Proyecto', 'Nombre Flujo', 'Sector', 'Estado', 'Macroproceso', 'Codigo Proceso', 'FechaCreacion', 'Accion Flujo', 'FechaPaso'];
    var columnasDisponiblesProyectoConsolaProcesos = [];

    function servicioPanelPrincipal($q, $http, $location, constantesBackbone, $sessionStorage, $timeout, sesionServicios) {
        return {
            columnasDisponiblesProyecto: columnasDisponiblesProyecto,
            columnasPorDefectoProyecto: columnasPorDefectoProyecto,
            columnasDisponiblesTramites: columnasDisponiblesTramites,
            columnasPorDefectoTramites: columnasPorDefectoTramites,
            obtenerInbox: obtenerInbox,
            obtenerTramites: obtenerTramites,
            obtenerProgramaciones: obtenerProgramaciones,
            obtenerSectores: obtenerSectores,
            obtenerEntidades: obtenerEntidades,
            obtenerPrioridades: obtenerPrioridades,
            obtenerEstadoProyectos: obtenerEstadoProyectos,
            obtenerEstadoTramites: obtenerEstadoTramites,
            obtenerPdfInboxProyectos: obtenerPdfInboxProyectos,
            imprimirPdfProyectos: imprimirPdfProyectos,
            obtenerPdfInboxTramites: obtenerPdfInboxTramites,
            imprimirPdfTramites: imprimirPdfTramites,
            imprimirPdfConsolaProyectos: imprimirPdfConsolaProyectos,
            obtenerPdfInboxProyectosTramites: obtenerPdfInboxProyectosTramites,
            imprimirPdfProyectosTramites: imprimirPdfProyectosTramites,
            obtenerExcelProyetos: obtenerExcelProyetos,
            obtenerProyectosPorTramite: obtenerProyectosPorTramite,
            obtenerMacroprocesos: obtenerMacroprocesos,
            obtenerProcesos: obtenerProcesos,
            obtenerMacroprocesosCantidad: obtenerMacroprocesosCantidad,
            obtenerExcelTramites: obtenerExcelTramites,
            obtenerExcelTramitesProyectos: obtenerExcelTramitesProyectos,
            obtenerEstadoInstancia: obtenerEstadoInstancia,
            activarInstancia: activarInstancia,
            pausarInstancia: pausarInstancia,
            detenerInstancia: detenerInstancia,
            cancelarInstanciaMisProcesos: cancelarInstanciaMisProcesos,
            obtenerExcelLogInstancia: obtenerExcelLogInstancia,
            obtenerInboxConsolaProcesos: obtenerInboxConsolaProcesos,
            obtenerExcelProyetosConsolaProcesos: obtenerExcelProyetosConsolaProcesos,
            obtenerPdfInboxProyectosConsolaProcesos: obtenerPdfInboxProyectosConsolaProcesos,
            obtenerTramitesConsolaProcesos: obtenerTramitesConsolaProcesos,
            obtenerExcelTramitesConsolaProcesos: obtenerExcelTramitesConsolaProcesos,
            obtenerPdfInboxTramitesConsolaProcesos: obtenerPdfInboxTramitesConsolaProcesos,
            columnasPorDefectoProyectoConsolaProcesos: columnasPorDefectoProyectoConsolaProcesos,
            columnasDisponiblesProyectoConsolaProcesos: columnasDisponiblesProyectoConsolaProcesos,
            obtenerPasoDelTramite: obtenerPasoDelTramite,
            devolverInstanciasHijas: devolverInstanciasHijas,
            obtenerTiposTramite: obtenerTiposTramite,
            obtenerEntidadesVisualizador: obtenerEntidadesVisualizador,
            ObtenerEntidadesPorSector: ObtenerEntidadesPorSector,
            ObtenerFlujosPorTipoObjeto: ObtenerFlujosPorTipoObjeto,
            ObtenerAccionesFlujoPorFlujoId: ObtenerAccionesFlujoPorFlujoId,
            ObtenerVigencias: ObtenerVigencias,
            obtenerProgramacionConsolaProcesos: obtenerProgramacionConsolaProcesos,
            obtenerInboxVerificacionSgr: obtenerInboxVerificacionSgr
        }

        function obtenerInbox(peticionObtenerInbox, proyectoFiltro, columnasVisibles) {
            const inboxDto = {
                ProyectoParametrosDto: peticionObtenerInbox,
                ProyectoFiltroDto: proyectoFiltro,
                columnasVisibles: columnasVisibles
            };

            return $http.post(apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerProyecto, inboxDto);
        }

        function obtenerTramites(peticionObtenerInbox, tramiteFiltro, columnasVisibles) {
            var listaFiltrados = [];
            Object.keys(tramiteFiltro).forEach(filtro => {
                try {
                    if (tramiteFiltro[`${filtro}`].valor) {
                        listaFiltrados.push(tramiteFiltro[`${filtro}`]);
                    }
                }
                catch { }

            });

            const tramiteDto = {
                parametrosInboxDto: peticionObtenerInbox,
                tramiteFiltroDto: {
                    tokenAutorizacion: '',
                    idUsuarioDNP: peticionObtenerInbox.IdUsuario,
                    filtroGradeDtos: listaFiltrados,
                    IdsRoles: peticionObtenerInbox.ListaIdsRoles,
                    IdTipoObjetoNegocio: peticionObtenerInbox.IdObjeto,
                    IdsEtapas: peticionObtenerInbox.IdsEtapas,
                    NumeroTramite: tramiteFiltro.numeroTramite
                },
                columnasVisibles: columnasVisibles
            };

            return $http.post(apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerTramite, tramiteDto);
        }

        function obtenerProgramaciones(peticionObtenerInbox, tramiteFiltro, columnasVisibles) {
            var listaFiltrados = [];
            Object.keys(tramiteFiltro).forEach(filtro => {
                try {
                    if (tramiteFiltro[`${filtro}`].valor) {
                        listaFiltrados.push(tramiteFiltro[`${filtro}`]);
                    }
                }
                catch { }

            });

            const tramiteDto = {
                parametrosInboxDto: peticionObtenerInbox,
                tramiteFiltroDto: {
                    tokenAutorizacion: '',
                    idUsuarioDNP: peticionObtenerInbox.IdUsuario,
                    filtroGradeDtos: listaFiltrados,
                    IdsRoles: peticionObtenerInbox.ListaIdsRoles,
                    IdTipoObjetoNegocio: peticionObtenerInbox.IdObjeto,
                    IdsEtapas: peticionObtenerInbox.IdsEtapas,
                    NumeroTramite: tramiteFiltro.numeroTramite
                },
                columnasVisibles: columnasVisibles
            };

            return $http.post(apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerTramiteProgramacion, tramiteDto);
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

            return $http.post(apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerProyectosPorTramite, tramiteDto);
        }

        function obtenerMacroprocesos() {
            return $http.get(apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerMacroProcesos);
        }

        function obtenerProcesos() {
            return $http.get(apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerProcesos);
        }

        function obtenerMacroprocesosCantidad() {
            //var parametrosCantidades = {
            //    rolesIds: sesionServicios.obtenerUsuarioIdsRoles(),
            //    etapasIds: getIdEtapa()
            //};
            return $http.get(apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerMacroProcesosCantidad);
        }

        function obtenerSectores(peticion) {
            return $http.post(apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerProyectoListaSectores, peticion);
        }

        function obtenerEntidades(peticion) {
            return $http.post(apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerProyectoListaEntidades, peticion);
        }

        function obtenerEntidadesVisualizador(usuarioDNP) {
            return $http.post(apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerEntidadesConRoleVisualizador + "?usuarioDNP=" + usuarioDNP);
        }

        function ObtenerEntidadesPorSector(sectorId, tipoEntidad) {
            return $http.get(apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerEntidadesPorSector + "?sectorId=" + sectorId + "&tipoEntidad="+tipoEntidad+"&usuarioDNP=" + usuarioDNP);
        }
        function ObtenerFlujosPorTipoObjeto(tipoObjetoId, usuarioDNP) {
            return $http.get(apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerFlujosPorTipoObjeto + "?tipoObjetoId=" + tipoObjetoId + "&usuarioDNP=" + usuarioDNP);
        }
        function ObtenerAccionesFlujoPorFlujoId(flujoId, usuarioDNP) {
            return $http.get(apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerAccionesFlujoPorFlujoId + "?flujoId=" + flujoId + "&usuarioDNP=" + usuarioDNP);
        }
        function ObtenerVigencias(tipoObjetoId, usuarioDNP) {
            return $http.get(apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerVigencias + "?tipoObjetoId=" + tipoObjetoId + "&usuarioDNP=" + usuarioDNP);
        }

        function obtenerPrioridades(peticion) {
            return $http.post(apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerProyectoListaCriticidad, peticion);
        }

        function obtenerEstadoProyectos(peticion) {
            return $http.post(apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerProyectoListaEstadoProyecto, peticion);
        }

        function obtenerEstadoTramites() {
    /*  *//*//return $http.get*/(apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerInbox);
            return $q(function (resolve, reject) {
                var result = [{
                    value: 1,
                    text: 'estado 1'
                },
                {
                    value: 2,
                    text: 'estado 2'
                },
                {
                    value: 3,
                    text: 'estado 3'
                }
                ];

                resolve(result);
            });
        }

        function obtenerExcelProyetos(peticionObtenerInbox, proyectoFiltro, columnasVisibles) {
            const inboxDto = {
                ProyectoParametrosDto: peticionObtenerInbox,
                ProyectoFiltroDto: proyectoFiltro,
                columnasVisibles: columnasVisibles
            };
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerExcelProyectos;
            return $http.post(url, inboxDto, {
                responseType: 'arraybuffer'
            });
        }

        function obtenerExcelTramites(peticionObtenerInbox, tramiteFiltro, columnasVisibles) {

            var listaFiltrados = [];
            Object.keys(tramiteFiltro).forEach(filtro => {
                try {
                    if (tramiteFiltro[`${filtro}`].valor) {
                        listaFiltrados.push(tramiteFiltro[`${filtro}`]);
                    }
                }
                catch { }

            });

            const tramiteDto = {
                parametrosInboxDto: peticionObtenerInbox,
                tramiteFiltroDto: {
                    tokenAutorizacion: '',
                    idUsuarioDNP: peticionObtenerInbox.IdUsuario,
                    filtroGradeDtos: listaFiltrados,
                    IdsRoles: peticionObtenerInbox.ListaIdsRoles,
                    IdTipoObjetoNegocio: peticionObtenerInbox.IdObjeto,
                    IdsEtapas: peticionObtenerInbox.IdsEtapas,
                    NumeroTramite: tramiteFiltro.numeroTramite,
                    Macroproceso: tramiteFiltro.Macroproceso
                },
                columnasVisibles: columnasVisibles
            };

            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerExcelTramites;
            return $http.post(url, tramiteDto, {
                responseType: 'arraybuffer'
            });
        }

        function obtenerExcelTramitesProyectos(peticionObtenerInbox, idTramite) {
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


            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerExcelTramitesProyectos;
            return $http.post(url, tramiteDto, {
                responseType: 'arraybuffer'
            });
        }

        function obtenerExcelLogInstancia(instanciasLog) {
            const logsInstanciasDto = [];
            instanciasLog.forEach(element => {
                logsInstanciasDto.push({
                    Descripcion: element.Descripcion,
                    BPIN: element.BPIN,
                    EntidadId: element.EntidadId,
                    EntityCatalogOptionId: element.EntityCatalogOptionId,
                    Estado: element.Estado,
                    Fecha: element.Fecha,
                    Id: element.Id,
                    Entidad: element.NombreEntidad,
                    Usuario: element.NombreUsuario,
                    TipoObjetoId: element.TipoObjetoId,
                    UsuarioId: element.UsuarioId
                });
            });

            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerExcelLogInstancia;
            return $http.post(url, logsInstanciasDto, {
                responseType: 'arraybuffer'
            });
        }

        function obtenerPdfInboxProyectos(peticionObtenerInbox, proyectoFiltro, columnasVisibles) {
            const inboxDto = {
                ProyectoParametrosDto: peticionObtenerInbox,
                ProyectoFiltroDto: proyectoFiltro,
                columnasVisibles: columnasVisibles
            };
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerProyectosPdfProyecto;

            return $http.post(url, inboxDto);
        }

        function imprimirPdfProyectos(inboxDto) {
            var url = urlPDFBackbone + constantesBackbone.apiBackboneProyectosImprimirPDF;
            return $http.post(url, inboxDto, {
                responseType: 'blob'
            });
        }

        function imprimirPdfConsolaProyectos(inboxDto) {
            var url = urlPDFBackbone + constantesBackbone.apiBackboneProyectosConsolaImprimirPDF;
            return $http.post(url, inboxDto, {
                responseType: 'blob'
            });
        }

        function obtenerPdfInboxTramites(peticionObtenerInbox, tramiteFiltro, columnasVisibles) {
            var listaFiltrados = [];
            Object.keys(tramiteFiltro).forEach(filtro => {
                try {
                    if (tramiteFiltro[`${filtro}`].valor) {
                        listaFiltrados.push(tramiteFiltro[`${filtro}`]);
                    }
                }
                catch { }
            });

            const tramiteDto = {
                parametrosInboxDto: peticionObtenerInbox,
                tramiteFiltroDto: {
                    tokenAutorizacion: '',
                    idUsuarioDNP: peticionObtenerInbox.IdUsuario,
                    filtroGradeDtos: listaFiltrados,
                    IdsRoles: peticionObtenerInbox.ListaIdsRoles,
                    IdTipoObjetoNegocio: peticionObtenerInbox.IdObjeto,
                    IdsEtapas: peticionObtenerInbox.IdsEtapas,
                    NumeroTramite: tramiteFiltro.numeroTramite,
                    Macroproceso: tramiteFiltro.Macroproceso
                },
                columnasVisibles: columnasVisibles
            };

            //const tramiteDto = {
            //    parametrosInboxDto: peticionObtenerInbox,
            //    tramiteFiltroDto: {
            //        tokenAutorizacion: '',
            //        idUsuarioDNP: peticionObtenerInbox.IdUsuario,
            //        filtroGradeDtos: listaFiltrados,
            //        IdsRoles: peticionObtenerInbox.ListaIdsRoles,
            //        IdTipoObjetoNegocio: peticionObtenerInbox.IdObjeto
            //    },
            //    columnasVisibles: columnasVisibles
            //};

            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneTramitesObtenerPdfInbox;

            return $http.post(url, tramiteDto);
        }

        function imprimirPdfTramites(inboxDto) {
            var url = urlPDFBackbone + constantesBackbone.apiBackboneTramitesImprimirPDF;
            return $http.post(url, inboxDto, {
                responseType: 'blob'
            });
        }

        function obtenerPdfInboxProyectosTramites(peticionObtenerInbox, idTramite) {
            var tramiteFiltroDto = {
                IdUsuarioDNP: peticionObtenerInbox.IdUsuarioDNP,
                TramiteId: idTramite,
                InstanciaId: idTramite
            }
            const inboxDto = {
                parametrosInboxDto: peticionObtenerInbox,
                tramiteFiltroDto: tramiteFiltroDto
            };

            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneTramitesProyectosPdfInbox;
            return $http.post(url, inboxDto);
        }

        function imprimirPdfProyectosTramites(inboxDto) {
            var url = urlPDFBackbone + constantesBackbone.apiBackboneTramitesProyectosImprimirPDF;
            return $http.post(url, inboxDto, {
                responseType: 'blob'
            });
        }

        function obtenerEstadoInstancia(peticion) {
            return $http.post(apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerProyectoListaEstadoInstancia, peticion);
        }

        function activarInstancia(peticionObtenerInbox, idInstancia) {
            peticionObtenerInbox.InstanciaId = idInstancia;
            const inboxDto = {
                ProyectoParametrosDto: peticionObtenerInbox,
            };

            return $http.post(apiBackboneServicioBaseUri + constantesBackbone.apiBackboneActivarInstancia, inboxDto);
        }

        function pausarInstancia(peticionObtenerInbox, idInstancia) {
            peticionObtenerInbox.InstanciaId = idInstancia;
            const inboxDto = {
                ProyectoParametrosDto: peticionObtenerInbox,
            };

            return $http.post(apiBackboneServicioBaseUri + constantesBackbone.apiBackbonePausarInstancia, inboxDto);
        }

        function detenerInstancia(peticionObtenerInbox, idInstancia) {
            peticionObtenerInbox.InstanciaId = idInstancia;
            const inboxDto = {
                ProyectoParametrosDto: peticionObtenerInbox,
            };

            return $http.post(apiBackboneServicioBaseUri + constantesBackbone.apiBackboneDetenerInstancia, inboxDto);
        }

        function cancelarInstanciaMisProcesos(peticionObtenerInbox, idInstancia) {
            peticionObtenerInbox.InstanciaId = idInstancia;
            const inboxDto = {
                ProyectoParametrosDto: peticionObtenerInbox,
            };

            return $http.post(apiBackboneServicioBaseUri + constantesBackbone.apiBackboneCancelarInstanciaMisProcesos, inboxDto);
        }

        function obtenerInboxConsolaProcesos(peticionObtenerInbox, proyectoFiltro, columnasVisibles) {
            const inboxDto = {
                ProyectoParametrosDto: peticionObtenerInbox,
                ProyectoFiltroDto: proyectoFiltro,
                columnasVisibles: columnasVisibles
            };

            return $http.post(apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerProyectoConsolaProcesos, inboxDto);
        }

        function obtenerExcelProyetosConsolaProcesos(peticionObtenerInbox, proyectoFiltro, columnasVisibles) {
            const inboxDto = {
                ProyectoParametrosDto: peticionObtenerInbox,
                ProyectoFiltroDto: proyectoFiltro,
                columnasVisibles: columnasVisibles
            };
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerExcelProyectosConsolaProcesos;
            return $http.post(url, inboxDto, {
                responseType: 'arraybuffer'
            });
        }

        function obtenerPdfInboxProyectosConsolaProcesos(peticionObtenerInbox, proyectoFiltro, columnasVisibles) {
            const inboxDto = {
                ProyectoParametrosDto: peticionObtenerInbox,
                ProyectoFiltroDto: proyectoFiltro,
                columnasVisibles: columnasVisibles
            };
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerProyectosPdfProyectoConsolaProcesos;

            return $http.post(url, inboxDto);
        }

        function obtenerTramitesConsolaProcesos(peticionObtenerInbox, tramiteFiltro, columnasVisibles) {
            var listaFiltrados = [];
            Object.keys(tramiteFiltro).forEach(filtro => {
                try {
                    if (tramiteFiltro[`${filtro}`].valor) {
                        listaFiltrados.push(tramiteFiltro[`${filtro}`]);
                    }
                }
                catch { }

            });

            const tramiteDto = {
                parametrosInboxDto: peticionObtenerInbox,
                tramiteFiltroDto: {
                    tokenAutorizacion: '',
                    idUsuarioDNP: peticionObtenerInbox.IdUsuario,
                    filtroGradeDtos: listaFiltrados,
                    IdsRoles: peticionObtenerInbox.ListaIdsRoles,
                    IdTipoObjetoNegocio: peticionObtenerInbox.IdObjeto,
                    IdsEtapas: peticionObtenerInbox.IdsEtapas,
                    NumeroTramite: tramiteFiltro.numeroTramite.valor
                },
                columnasVisibles: columnasVisibles
            };

            return $http.post(apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerTramiteConsolaProcesos, tramiteDto);
        }

        function obtenerProgramacionConsolaProcesos(peticionObtenerInbox, tramiteFiltro, columnasVisibles) {
            var listaFiltrados = [];
            Object.keys(tramiteFiltro).forEach(filtro => {
                try {
                    if (tramiteFiltro[`${filtro}`].valor) {
                        listaFiltrados.push(tramiteFiltro[`${filtro}`]);
                    }
                }
                catch { }

            });

            const tramiteDto = {
                parametrosInboxDto: peticionObtenerInbox,
                tramiteFiltroDto: {
                    tokenAutorizacion: '',
                    idUsuarioDNP: peticionObtenerInbox.IdUsuario,
                    filtroGradeDtos: listaFiltrados,
                    IdsRoles: peticionObtenerInbox.ListaIdsRoles,
                    IdTipoObjetoNegocio: peticionObtenerInbox.IdObjeto,
                    IdsEtapas: peticionObtenerInbox.IdsEtapas,
                    NumeroTramite: tramiteFiltro.numeroTramite.valor
                },
                columnasVisibles: columnasVisibles
            };

            return $http.post(apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerProgramacionConsolaProcesos, tramiteDto);
        }

        function obtenerTiposTramite(peticionObtenerInbox, tramiteFiltro) {
            var listaFiltrados = [];
            Object.keys(tramiteFiltro).forEach(filtro => {
                try {
                    if (tramiteFiltro[`${filtro}`].valor) {
                        listaFiltrados.push(tramiteFiltro[`${filtro}`]);
                    }
                }
                catch { }

            });

            const tramiteDto = {
                parametrosInboxDto: peticionObtenerInbox,
                tramiteFiltroDto: {
                    tokenAutorizacion: '',
                    idUsuarioDNP: peticionObtenerInbox.IdUsuario,
                    filtroGradeDtos: listaFiltrados,
                    IdsRoles: peticionObtenerInbox.ListaIdsRoles,
                    IdTipoObjetoNegocio: peticionObtenerInbox.IdObjeto,
                    IdsEtapas: peticionObtenerInbox.IdsEtapas,
                    NumeroTramite: tramiteFiltro.numeroTramite
                }
            };
            return $http.post(apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerTiposTramite, tramiteDto);
        }

        function obtenerExcelTramitesConsolaProcesos(peticionObtenerInbox, tramiteFiltro, columnasVisibles) {

            var listaFiltrados = [];
            Object.keys(tramiteFiltro).forEach(filtro => {
                try {
                    if (tramiteFiltro[`${filtro}`].valor) {
                        listaFiltrados.push(tramiteFiltro[`${filtro}`]);
                    }
                } catch { }
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

            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerExcelTramitesConsolaProcesos;
            return $http.post(url, tramiteDto, {
                responseType: 'arraybuffer'
            });
        }

        function obtenerPdfInboxTramitesConsolaProcesos(peticionObtenerInbox, tramiteFiltro, columnasVisibles) {
            var listaFiltrados = [];
            Object.keys(tramiteFiltro).forEach(filtro => {
                try {
                    if (tramiteFiltro[`${filtro}`].valor) {
                        listaFiltrados.push(tramiteFiltro[`${filtro}`]);
                    }

                } catch { }
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

            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneTramitesObtenerPdfInboxConsolaProcesos;

            return $http.post(url, tramiteDto);
        }

        function obtenerPasoDelTramite(tramiteId) {
            return $http.get(apiBackboneServicioBaseUri + constantesBackbone.apiBackBoneTramiteEnPasoUno + "?InstanciaId=" + tramiteId);
        }

        function devolverInstanciasHijas(peticionObtenerInbox, idInstancia) {
            peticionObtenerInbox.InstanciaId = idInstancia;
            const inboxDto = {
                ProyectoParametrosDto: peticionObtenerInbox,
            };

            return $http.post(apiBackboneServicioBaseUri + constantesBackbone.apiBackBoneDevolverInstanciasHijas, inboxDto);
        }

        function obtenerInboxVerificacionSgr(peticionObtenerInbox, proyectoFiltro, columnasVisibles) {
            const inboxDto = {
                ProyectoParametrosDto: peticionObtenerInbox,
                ProyectoFiltroDto: proyectoFiltro,
                columnasVisibles: columnasVisibles
            };

            return $http.post(apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerProyectoVerificacionOcadPazSgr, inboxDto);
        }

    }
})();