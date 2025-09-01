(function () {
    'use strict';

    consolaMonitoreoController.$inject = ['$scope', 'constantesAutorizacion', 'configurarEntidadRolSectorServicio', 'constantesCondicionFiltro',
        'servicioConsolaMonitoreo', 'sesionServicios', 'backboneServicios', 'FileSaver', 'Blob', 'servicioPanelPrincipal', '$location', '$uibModal', 'uiGridConstants', 'constantesTipoFiltro', '$localStorage', '$routeParams'];

    function consolaMonitoreoController($scope, constantesAutorizacion, configurarEntidadRolSectorServicio, constantesCondicionFiltro,
        servicioConsolaMonitoreo, sesionServicios, backboneServicios, FileSaver, Blob, servicioPanelPrincipal, $location, $uibModal, uiGridConstants, constantesTipoFiltro, $localStorage, $routeParams) {

        var vm = this;

        //variables
        vm.listaTipoEntidad = [];
        vm.listaFiltroEstadoProyectos = [];
        vm.proyectosIdsConAlertas = [];
        vm.tipoEntidad = null;
        vm.mostrarFiltro = false;
        vm.peticion;
        vm.gridOptions;
        vm.plantillaProyectoBpin = 'src/app/monitoreo/plantillas/plantillaProyectoBpin.html';
        vm.mostrarMensaje = false;
        vm.Mensaje;
        vm.accionesFilaProyectoTemplate = 'src/app/monitoreo/plantillas/plantillaAccionesProyecto.html';
        vm.estadoProyectoTemplate = 'src/app/monitoreo/plantillas/plantillaEstadoProyecto.html';
        vm.roles = obtenerRoles();
        vm.columnas = servicioConsolaMonitoreo.columnasPorDefecto;
        vm.columnasDisponiblesPorAgregar = servicioConsolaMonitoreo.columnasDisponibles;
        vm.puedeVerFiltroBpin;
        vm.puedeVerFiltroNombreProyecto;
        vm.puedeVerFiltroEstadoProyecto;
        vm.puedeVerFiltroAvanceFinanciero;
        vm.puedeVerFiltroAvanceFisico;
        vm.puedeVerFiltroAvanceProyecto;
        vm.puedeVerFiltroDuracion;
        vm.puedeVerFiltroPeriodoEjecucion;
        vm.puedeVerFiltroSector;
        vm.tipoFiltro = constantesTipoFiltro.proyecto;

        //Métodos
        vm.cambioTipoEntidad = cambioTipoEntidad;
        vm.listaConfiguracionesRolSector = obtenerConfiguracionesRolSector();
        vm.conmutadorFiltro = conmutadorFiltro;
        vm.init = init;
        vm.limpiarCamposFiltro = limpiarCamposFiltro;
        vm.buscar = buscar;
        vm.downloadPdf = downloadPdf;
        vm.downloadExcel = downloadExcel;
        vm.abreReporte = abreReporte;
        vm.abrirModalListaAlertas = abrirModalListaAlertas;
        vm.obtenerCondicionesParaAccionAlertas = obtenerCondicionesParaAccionAlertas;
        vm.proyectoTieneAlerta = proyectoTieneAlerta;
        vm.abrirModalAdicionarColumnas = abrirModalAdicionarColumnas;

        vm.filasFiltradas = null;
        vm.sectorEntidadFilaTemplate = 'src/app/monitoreo/plantillas/plantillaSectorEntidad.html';
        vm.proyectoFilaTemplate = 'src/app/monitoreo/plantillas/plantillaProyectoResumen.html';

        function abreReporte(proyectoId, codigoBpin, proyectoNombre) {
            var path = "/consolaMonitoreo/" + (proyectoId || "") + "/" + (codigoBpin || "") + "/" + (proyectoNombre || "")
            $location.path(path);
        }

        function downloadPdf() {
            servicioConsolaMonitoreo.obtenerPdfConsolaMonitoreo(vm.peticion, vm.filtro, buscarColumnasPorColumnasFiltroSeleccionadas()).then(
                function (data) {
                    servicioConsolaMonitoreo.imprimirPdfConsolaMonitoreo(data.data).then(function (retorno) {
                        FileSaver.saveAs(retorno.data, nombreDelArchivo(retorno));
                    });

                }, function (error) {
                    vm.Mensaje = error.data.Message;
                    mostrarMensajeRespuesta();
                }
            );
        };

        function nombreDelArchivo(response) {
            var filename = "";
            var disposition = response.headers("content-disposition");
            if (disposition && disposition.indexOf('attachment') !== -1) {
                var filenameRegex = /filename[^;=\n]*=((['"]).*?\2|[^;\n]*)/;
                var matches = filenameRegex.exec(disposition);
                if (matches != null && matches[1]) {
                    filename = matches[1].replace(/['"]/g, '');
                }
            }
            return filename;
        }

        function downloadExcel() {

            servicioConsolaMonitoreo.obtenerExcelConsolaMonitoreo(vm.peticion, vm.filtro, buscarColumnasPorColumnasFiltroSeleccionadas()).then(function (retorno) {
                var blob = new Blob([retorno.data], {
                    type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
                });
                FileSaver.saveAs(blob, "ConsolaMonitoreoProyectos.xls");
            }, function (error) {
                vm.Mensaje = error.data.Message;
                mostrarMensajeRespuesta();
            });

        }

        vm.filtro = {
            nombre: null,
            bpin: null,
            avanceFinanciero: null,
            avanceFisico: null,
            avanceProyecto: null,
            duracion: null,
            periodoEjecucion: null,
            estadoProyecto: null,
            estadoProyectoId: null,
            sectorId: null,
            ProyectosIds: null,
            entidadId: null
        };

        vm.columnDefPrincial = [{
            field: 'entidad',
            displayName: 'Entidad',
            enableHiding: false,
            width: '96%',
            cellTemplate: vm.sectorEntidadFilaTemplate,
        }];

        vm.columnDef = [
            {
                field: 'ProyectoNombre',
                displayName: "BPIN/Proyecto",
                width: "40%",
                cellTemplate: vm.plantillaProyectoBpin,
                cellClass: function (grid, row, col, rowRenderIndex, colRenderIndex) {
                    return 'ui-grid-description';
                },
                cellTooltip: (row, col) => row.entity[col.field]
            },
            {
                field: 'EstadoProyecto',
                displayName: "Estado",
                cellTemplate: vm.estadoProyectoTemplate,
                width: "10%",
                cellTooltip: (row, col) => row.entity[col.field]
            },
            {
                field: 'ProyectoId',
                displayName: 'ProyectoId',
                enableFiltering: false,
                enableHiding: true,
                enableSorting: false,
                enableColumnMenu: false,
                visible: false,
                width: "12%",
                cellTooltip: (row, col) => row.entity[col.field]
            },
            {
                field: 'AvanceFinanciero',
                displayName: 'Avance Financiero',
                enableHiding: false,
                width: '13%',
                cellTooltip: (row, col) => row.entity[col.field]
            },
            {
                field: 'AvanceFisico',
                displayName: 'Avance Fisico',
                enableHiding: false,
                width: '12%',
                cellTooltip: (row, col) => row.entity[col.field]
            },
            {
                field: 'AvanceProyecto',
                displayName: 'Avance Proyecto',
                enableHiding: false,
                width: '13%',
                cellTooltip: (row, col) => row.entity[col.field]
            },
            {
                field: 'Duracion',
                displayName: 'Duración',
                enableHiding: false,
                width: '12%',
                cellTooltip: (row, col) => row.entity[col.field]
            },
            {
                field: 'PeriodoEjecucion',
                displayName: 'Periodo Ejecución',
                enableHiding: false,
                width: '15%',
                cellTooltip: (row, col) => row.entity[col.field]
            },
            {
                field: 'SectorNombre',
                displayName: 'Sector',
                enableHiding: false,
                width: '10%',
                cellTooltip: (row, col) => row.entity[col.field]
            },
            {
                field: 'accion',
                displayName: 'Acción',
                enableFiltering: false,
                enableHiding: false,
                enableSorting: false,
                enableColumnMenu: false,
                pinnedRight: false,
                cellTemplate: vm.accionesFilaProyectoTemplate,
                width: '120'
            }
        ];

        vm.todasColumnasDefinicion = Object.assign([], vm.columnDef);
        vm.listaDatos = [];
        vm.listaEntidades = [];
        vm.listaProyectosIds = [];

        function obtenerRoles() {
            var roles = sesionServicios.obtenerUsuarioIdsRoles();

            if (backboneServicios.estaAutorizado() && roles != null && roles.length > 0) {
                vm.peticion = {
                    IdUsuario: usuarioDNP,
                    Aplicacion: nombreAplicacionBackbone,
                    IdObjeto: "bc154cba-50a5-4209-81ce-7c0ff0aec2ce",
                    ListaIdsRoles: roles
                };

            }

        }

        function crearListaTipoEntidad() {
            return [{
                Nombre: constantesAutorizacion.tipoEntidadNacional,
                Descripcion: constantesAutorizacion.tipoEntidadNacional,
                Deshabilitado: !tipoEntidadPresenteEnLaConfiguracion(constantesAutorizacion.tipoEntidadNacional)
            },
            {
                Nombre: constantesAutorizacion.tipoEntidadTerritorial,
                Descripcion: constantesAutorizacion.tipoEntidadTerritorial,
                Deshabilitado: !tipoEntidadPresenteEnLaConfiguracion(constantesAutorizacion.tipoEntidadTerritorial)
            },
            {
                Nombre: constantesAutorizacion.tipoEntidadSGR,
                Descripcion: constantesAutorizacion.tipoEntidadSGR,
                Deshabilitado: !tipoEntidadPresenteEnLaConfiguracion(constantesAutorizacion.tipoEntidadSGR)
            },
            {
                nombre: constantesAutorizacion.tipoEntidadPrivadas,
                Descripcion: constantesAutorizacion.tipoEntidadPrivadas,
                Deshabilitado: !tipoEntidadPresenteEnLaConfiguracion(constantesAutorizacion.tipoEntidadPrivadas)
            },
            {
                Nombre: constantesAutorizacion.tipoEntidadPublicas,
                Descripcion: "Públicas",
                Deshabilitado: !tipoEntidadPresenteEnLaConfiguracion(constantesAutorizacion.tipoEntidadPublicas)
            }
            ];
        }

        function cambioTipoEntidad(tipoEntidad) {
            vm.tipoEntidad = tipoEntidad;
            vm.filtro.tipoEntidad = vm.tipoEntidad;
            limpiarCamposFiltro(false);
            listaConsoleMonitoreo().then(function () {
                recargarFiltrosSeleccionables();
            });
        }

        function recargarFiltrosSeleccionables() {
            listarFiltroEstadoProyecto();
            listarFiltroSectores();
            vm.establecerListaFiltroEntidades(vm.listaEntidades);
        }

        function tipoEntidadPresenteEnLaConfiguracion(tipoEntidad) {
            var tipoConfiguracion = _.find(vm.listaConfiguracionesRolSector, {
                TipoEntidad: tipoEntidad
            });
            return tipoConfiguracion ? true : false;
        }

        function obtenerConfiguracionesRolSector() {
            var parametros = {
                usuarioDnp: usuarioDNP,
                nombreAplicacion: nombreAplicacionBackbone
            }
            return configurarEntidadRolSectorServicio.obtenerConfiguracionesRolSector(parametros).then(function (respuesta) {
                vm.listaConfiguracionesRolSector = respuesta;
                vm.listaTipoEntidad = crearListaTipoEntidad();
            });
        }

        function conmutadorFiltro() {
            limpiarCamposFiltro(false);
            vm.mostrarFiltro = !vm.mostrarFiltro;
        }

        function limpiarCamposFiltro(limpiarUrl = true) {
            vm.filtro.nombre = null;
            vm.filtro.bpin = null;
            vm.filtro.avanceFinanciero = null;
            vm.filtro.avanceFisico = null;
            vm.filtro.avanceProyecto = null;
            vm.filtro.duracion = null;
            vm.filtro.periodoEjecucion = null;
            vm.filtro.estadoProyecto = null;
            vm.filtro.estadoProyectoId = null;
            vm.filtro.sectorId = null;
            vm.filtro.entidadId = null;
            if (!$location.search())
                vm.filtro.ProyectosIds = null;

            vm.proyectosIdsConAlertas = [];
            vm.ProyectosIds = [];

            if (limpiarUrl)
                window.history.replaceState({}, document.title, "/tableroControlProyectos");
        }

        function init() {

            buscarColumnasLocalStorage();

            const { params } = $location.search();
            if (params)
                vm.proyectosIdsConAlertas = decodeURIComponent(params).split(",") || [];

            if (vm.gridOptions)
                return;

            vm.gridOptions = {
                expandableRowTemplate: vm.proyectoFilaTemplate,
                expandableRowScope: {
                    subGridVariable: 'subGridScopeVariable'
                },
                expandableRowHeight: 550,
                enableColumnResizing: false,
                showGridFooter: false,
                enablePaginationControls: true,
                useExternalPagination: false,
                useExternalSorting: false,
                paginationCurrentPage: 1,
                enableVerticalScrollbar: 1,
                enableFiltering: false,
                showHeader: false,
                useExternalFiltering: false,
                paginationPageSizes: [10, 15, 25, 50, 100],
                paginationPageSize: 10,
                onRegisterApi: onRegisterApi
            }

            vm.gridOptions.columnDefs = vm.columnDefPrincial;
            vm.gridOptions.data = vm.listaEntidades;

            listaConsoleMonitoreo().then(function () {
                let tipoEntidad;
                for (let i = 0; i < vm.listaTipoEntidad.length; i++) {
                    tipoEntidad = vm.listaTipoEntidad[i];

                    if (!tipoEntidad.Deshabilitado) {
                        vm.tipoEntidad = tipoEntidad.Nombre;
                        //cambioTipoEntidad(vm.tipoEntidad);
                        break;
                    }
                }
                recargarFiltrosSeleccionables();
            });
        }

        function onRegisterApi(gridApi) {
            $scope.gridApi = gridApi;
        }

        function listaConsoleMonitoreo() {

            if (!vm.filtro.tipoEntidad) {
                vm.filtro.tipoEntidad = constantesAutorizacion.tipoEntidadNacional;
            }

            if (vm.proyectosIdsConAlertas.length)
                vm.filtro.ProyectosIds = vm.proyectosIdsConAlertas;

            return servicioConsolaMonitoreo.obtenerConsolaMonitoreo(vm.peticion, vm.filtro).then(

                function (respuesta) {

                    vm.listaEntidades = [];

                    if (respuesta.data.GruposEntidades && respuesta.data.GruposEntidades.length > 0) {
                        const listaGrupoEntidades = respuesta.data.GruposEntidades;
                        listaGrupoEntidades.forEach(grupoEntidade => {
                            grupoEntidade.ListaEntidades.forEach(entidad => {
                                const nombreEntidade = entidad.NombreEntidad;
                                const tipoEntidad = entidad.TipoEntidad;
                                const nombreSector = entidad.SectorEntidad;
                                const idSector = entidad.ObjetosNegocio[0].SectorId;

                                vm.listaDatos = [];
                                entidad.ObjetosNegocio.forEach(negocio => {
                                    vm.listaDatos.push({
                                        ProyectoId: negocio.ProyectoId,
                                        CodigoBpin: negocio.CodigoBpin,
                                        ProyectoNombre: negocio.ProyectoNombre,
                                        NombreEntidad: negocio.NombreEntidad,
                                        DescripcionCR: negocio.DescripcionCR,
                                        FechaCreacion: negocio.FechaCreacion,
                                        Criticidad: negocio.Criticidad,
                                        EstadoProyecto: negocio.EstadoProyecto,
                                        Horizonte: negocio.Horizonte,
                                        SectorNombre: negocio.SectorNombre,
                                        NombreAccion: negocio.NombreAccion,
                                        IdEntidad: negocio.IdEntidad,
                                        IdInstancia: negocio.IdInstancia,
                                        AvanceFinanciero: negocio.AvanceFinanciero,
                                        AvanceFisico: negocio.AvanceFisico,
                                        AvanceProyecto: negocio.AvanceProyecto,
                                        Duracion: negocio.Duracion,
                                        PeriodoEjecucion: negocio.PeriodoEjecucion,
                                        TieneAlertas: negocio.TieneAlertas,
                                        Sector: negocio.SectorNombre
                                    });

                                    if (!vm.listaProyectosIds.find(x => x.id == negocio.ProyectoId))
                                        vm.listaProyectosIds.push({ id: negocio.ProyectoId })
                                });

                                vm.listaEntidades.push({
                                    sector: nombreSector,
                                    entidad: nombreEntidade,
                                    tipoEntidad: tipoEntidad,
                                    sectorId: idSector,
                                    subGridOptions: {
                                        columnDefs: vm.columnDef,
                                        appScopeProvider: $scope,
                                        paginationPageSizes: [5, 10, 15, 25, 50, 100],
                                        enableHorizontalScrollbar: uiGridConstants.scrollbars.ALWAYS,
                                        scrollDirection: uiGridConstants.scrollDirection.RIGHT,
                                        paginationPageSize: 5,
                                        data: vm.listaDatos
                                    }
                                });
                            });
                        });

                        if (vm.listaProyectosIds.length && vm.proyectosIdsConAlertas.length)
                            vm.listaProyectosIds = vm.proyectosIdsConAlertas.map(x => ({
                                id: x,
                                TieneAlerta: true
                            }));

                        else
                            vm.obtenerCondicionesParaAccionAlertas(vm.listaProyectosIds.map(x => x.id))
                                .then(response => {
                                    vm.listaProyectosIds = vm.listaProyectosIds.distinctBy(x => x.id, (value) => ({
                                        id: value,
                                        TieneAlerta: false
                                    }));

                                    for (const key in response.data) {
                                        const proyecto = vm.listaProyectosIds.find(x => x.id == key)
                                        if (proyecto)
                                            proyecto.TieneAlerta = response.data[key];
                                    }
                                });

                        configurarColumnas();
                    }

                    vm.gridOptions.data = vm.listaEntidades;
                    vm.filasFiltradas = vm.gridOptions.data.length > 0;
                },
                function (error) {
                    vm.gridOptions.data = [];
                    if (error) {
                        if (error.status) {
                            switch (error.status) {
                                case 401:
                                    vm.Mensaje = $filter('language')('ErrorUsuarioSinPermisosAplicacion');
                                    break;
                                case 500:
                                    vm.Mensaje = $filter('language')('ErrorObtenerDatos');
                                    break;
                                default:
                                    vm.Mensaje = error.statusText;
                                    break;
                            }
                        }
                    }
                }
            );
        }

        function mostrarMensajeRespuesta() {
            if (vm.Mensaje) {
                vm.mostrarMensaje = true;
            } else {
                vm.mostrarMensaje = false;
            }
        }

        function buscar() {
            vm.filtro.tipoEntidad = vm.tipoEntidad;
            listaConsoleMonitoreo();
        }

        function listarFiltroEstadoProyecto() {
            servicioPanelPrincipal.obtenerEstadoProyectos(vm.peticion).then(exito, error);

            function exito(respuesta) {
                let listaEstadoProyectoGrid = [];
                vm.listaEntidades.forEach(entidade => {
                    entidade.subGridOptions.data.forEach(item =>
                        listaEstadoProyectoGrid.push(item.EstadoProyecto));
                });
                let listaFiltroEstadoProyecto = [];
                if (respuesta.data && respuesta.data.length > 0) {
                    listaFiltroEstadoProyecto = respuesta.data;
                }
                vm.listaFiltroEstadoProyectos = listaFiltroEstadoProyecto.filter(item => listaEstadoProyectoGrid.includes(item.Estado));
            }

            function error() {
                vm.listaFiltroEstadoProyectos = [];
            }
        }


        vm.listaEntidadesFiltro = [];
        /**
         * 
         * @description. Obtiene y establece la lista de entidaes desde la lista de entidades actual en forma gerárquica. 
         * @param {array} origenDatosPadre. Origen de datos como una Array, que contiene las propiedades para obtener los datos de entidades
         */
        vm.establecerListaFiltroEntidades = function (origenDatosPadre) {
            try {

                vm.listaEntidadesFiltro = [];

                if (typeof origenDatosPadre !== 'object' && origenDatosPadre.length === 0)
                    throw { message: 'El origen de datos no tiene el formato correcto' };
                let mapEntidades = []; mapEntidades = origenDatosPadre.map(p => p.subGridOptions.data);
                mapEntidades = mapEntidades.length > 0 ? mapEntidades.reduce((a, b) => a.concat(b)).map(p => ({ Id: p.IdEntidad, Nombre: p.NombreEntidad })) : [];

                // hacer un distinct
                let uniqueId = mapEntidades.map(p => p.Id).filter((value, index, self) => { return self.indexOf(value) === index });

                uniqueId.forEach(p => {
                    vm.listaEntidadesFiltro.push(mapEntidades.filter(q => q.Id === p)[0]);
                });
            }
            catch (exception) {
                console.log('consolaMonitoreoController.establecerListaFiltroEntidades => ', exception.message);
                toaster('Ocurrió un error al establecer la lista de Entidades del filtro');
            }
        };

        function listarFiltroSectores() {
            let listaSectoresGrid = [];

            // TODO: Modificar forma de filtrar sectores de entidades tipoEntidad propiedad es nula en vm.listaEntidades
            // vm.listaEntidades.forEach(entidade => {
            //     if (entidade.tipoEntidad == vm.tipoEntidad) {
            //         listaSectoresGrid.push({ Id: entidade.sectorId, Name: entidade.sector });
            //     }
            // });

            listaSectoresGrid = vm.listaEntidades.map(p => ({ Id: p.sectorId, Name: p.sector })).filter((value, index, self) => {
                return self.indexOf(value) === index
            });

            const seen = new Set();
            vm.listaFiltroSectores = [];
            vm.listaFiltroSectores = listaSectoresGrid.filter(el => {
                const duplicate = seen.has(el.Id);
                seen.add(el.Id);
                return !duplicate;
            });
        }

        function abrirModalListaAlertas(proyecto = null) {

            const modalInstance = $uibModal.open({
                animation: $scope.animationsEnabled,
                templateUrl: 'src/app/monitoreo/template/modales/alertas/alertasGeneradas.html',
                controller: 'alertasGeneradasController',
                controllerAs: "vm",
                size: 'lg',
                openedClass: "dialog-modal-alerta",
                resolve: {
                    proyectoId: proyecto.ProyectoId, peticion: {
                        IdUsuarioDNP: vm.peticion.IdUsuario,
                        Aplicacion: vm.peticion.Aplicacion,
                        IdsRoles: vm.peticion.ListaIdsRoles
                    }
                },
            });

            modalInstance.result.then(() => { }, () => { });
        };

        function obtenerCondicionesParaAccionAlertas(ids) {
            return servicioConsolaMonitoreo.obtenerCondicionesParaAccionAlertas(vm.peticion, ids)
        }

        function proyectoTieneAlerta(id) {
            return (vm.listaProyectosIds.find(x => x.id == id) || {}).TieneAlerta;
        }


        function abrirModalAdicionarColumnas() {

            var modalInstance = $uibModal.open({
                animation: $scope.animationsEnabled,
                templateUrl: 'src/app/configurarColumnas/plantillaConfigurarColumnas.html',
                controller: 'controladorConfigurarColumnas',
                controllerAs: "vm",
                size: 'lg',
                resolve: {
                    items: function () {
                        return {
                            'columnasActivas': vm.columnas,
                            'columnasDisponibles': vm.columnasDisponiblesPorAgregar,
                            'tipoFiltro': vm.tipoFiltro
                        };
                    }
                }
            });

            modalInstance.result.then(function (selectedItem) {
                if (!$localStorage.tipoFiltro) {
                    $localStorage.tipoFiltro = {
                        'monitoreo': {
                            'columnasActivas': selectedItem.columnasActivas,
                            'columnasDisponibles': selectedItem.columnasDisponibles
                        }
                    };
                } else {
                    $localStorage.tipoFiltro['monitoreo'] = {
                        'columnasActivas': selectedItem.columnasActivas,
                        'columnasDisponibles': selectedItem.columnasDisponibles
                    }
                }

                buscarColumnasLocalStorage();
                configurarColumnas();


            }, function () {
                $log.info('Modal dismissed at: ' + new Date());
            });
        }

        function buscarColumnasLocalStorage() {
            if ($localStorage.tipoFiltro) {
                if ($localStorage.tipoFiltro.monitoreo) {
                    vm.columnas = $localStorage.tipoFiltro.monitoreo.columnasActivas;
                    vm.columnasDisponiblesPorAgregar = $localStorage.tipoFiltro.monitoreo.columnasDisponibles;
                }
            }

            vm.puedeVerFiltroBpin = vm.columnas.indexOf('BPIN') > -1;
            vm.puedeVerFiltroNombreProyecto = vm.columnas.indexOf('Nombre Proyecto') > -1;
            vm.puedeVerFiltroEstadoProyecto = vm.columnas.indexOf('Estado') > -1;
            vm.puedeVerFiltroAvanceFinanciero = vm.columnas.indexOf('Avance Financiero') > -1;
            vm.puedeVerFiltroAvanceFisico = vm.columnas.indexOf('Avance Fisico') > -1;
            vm.puedeVerFiltroAvanceProyecto = vm.columnas.indexOf('Avance Proyecto') > -1;
            vm.puedeVerFiltroDuracion = vm.columnas.indexOf('Duración') > -1;
            vm.puedeVerFiltroPeriodoEjecucion = vm.columnas.indexOf('Periodo Ejecución') > -1;

            vm.puedeVerFiltroSector = vm.columnas.indexOf('Sector') > -1;

        }

        function configurarColumnas() {
            // vuelve a las columnas originales primero
            borrarColumnas();
            // agrega nuevas columnas en todas las filas del modelo
            agregarColumnas();
        }

        function borrarColumnas() {
            let lista = vm.listaEntidades;
            for (var i = 0, len = lista.length; i < len; i++) {
                var entidad = lista[i];

                for (var j = 0, lenCol = vm.columnasDisponiblesPorAgregar.length; j < lenCol; j++) {
                    var indexEliminar = -1;
                    var col = vm.columnasDisponiblesPorAgregar[j];

                    if (col == 'BPIN' || col == 'Nombre Proyecto') {
                        if (entidad.subGridOptions.columnDefs.map(x => x.displayName).indexOf('BPIN/Proyecto') > -1) {
                            indexEliminar = entidad.subGridOptions.columnDefs.map(x => x.displayName).indexOf('BPIN/Proyecto');
                        }
                    } else {
                        indexEliminar = entidad.subGridOptions.columnDefs.filter(x => x !== undefined).map(x => x.displayName).indexOf(col);
                    }

                    if (indexEliminar >= 0) entidad.subGridOptions.columnDefs.splice(indexEliminar, 1);
                }

                vm.listaEntidades[i] = entidad;
            }
        }

        function agregarColumnas() {
            let lista = vm.listaEntidades;
            var addColFechaCreacion;
            var colAcciones;
            var addCol;
            for (var i = 0, len = lista.length; i < len; i++) {
                var entidad = lista[i];

                for (var j = 0, lenCol = vm.columnas.length; j < lenCol; j++) {
                    var col = vm.columnas[j];

                    if (col == 'BPIN' || col == 'Nombre Proyecto') {
                        if (vm.columnDef.map(x => x.displayName).indexOf('BPIN/Proyecto') == -1) {
                            addColFechaCreacion = vm.todasColumnasDefinicion.filter(x => x.displayName == 'BPIN/Proyecto')[0]
                            colAcciones = vm.todasColumnasDefinicion.filter(x => x.field == 'accion')[0]

                            entidad.subGridOptions.columnDefs.pop();
                            entidad.subGridOptions.columnDefs.push(addColFechaCreacion);
                            entidad.subGridOptions.columnDefs.push(colAcciones);
                        }
                    } else {
                        if (vm.columnDef.map(x => x && x.displayName).indexOf(col) == -1) {
                            addCol = vm.todasColumnasDefinicion.filter(x => x.displayName == col)[0]
                            colAcciones = vm.todasColumnasDefinicion.filter(x => x.field == 'accion')[0]

                            entidad.subGridOptions.columnDefs.pop();
                            entidad.subGridOptions.columnDefs.push(addCol);
                            entidad.subGridOptions.columnDefs.push(colAcciones);
                        }
                    }
                }

                vm.listaEntidades[i] = entidad;
            }
        }

        function buscarColumnasPorColumnasFiltroSeleccionadas() {
            let listaColumnas = [];
            let columna = ''
            for (let i = 0; i < vm.columnas.length; i++) {
                var nombreColumnasSeleccionadaFiltro = vm.columnas[i];

                if (nombreColumnasSeleccionadaFiltro == 'BPIN' || nombreColumnasSeleccionadaFiltro == 'Nombre Proyecto') {
                    nombreColumnasSeleccionadaFiltro = 'BPIN/Proyecto';
                }

                columna = vm.todasColumnasDefinicion.filter(x => x.displayName == nombreColumnasSeleccionadaFiltro)[0].field;
                if (listaColumnas.indexOf(columna) == -1) {
                    listaColumnas.push(columna);
                }
            }

            return listaColumnas;
        }



    };

    angular.module('backbone').controller('consolaMonitoreoController', consolaMonitoreoController);
})();