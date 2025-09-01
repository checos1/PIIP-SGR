(function () {
    'use strict';

    consolaTramitesController.$inject = [
        '$scope',
        'servicioConsolaTramites',
        '$uibModal',
        '$log',
        '$q',
        '$sessionStorage',
        '$localStorage',
        '$timeout',
        '$location',
        '$filter',
        'estadoAplicacionServicios',
        'constantesBackbone',
        'sesionServicios',
        'backboneServicios',
        'constantesAutorizacion',
        'configurarEntidadRolSectorServicio',
        'constantesTipoFiltro',
        'uiGridConstants',
        'FileSaver',
        'Blob',
        'constantesCondicionFiltro'
    ];

    function consolaTramitesController(
        $scope,
        servicioConsolaTramites,
        $uibModal,
        $log,
        $q,
        $sessionStorage,
        $localStorage,
        $timeout,
        $location,
        $filter,
        estadoAplicacionServicios,
        constantesBackbone,
        sesionServicios,
        backboneServicios,
        constantesAutorizacion,
        configurarEntidadRolSectorServicio,
        constantesTipoFiltro,
        uiGridConstants,
        FileSaver,
        Blob,
        constantesCondicionFiltro
    ) {


        var vm = this;
        vm.consultarPermiso = backboneServicios.consultarPermiso;

        //Métodos
        vm.conmutadorFiltro = conmutadorFiltro;
        vm.obtenerInbox = obtenerInbox;
        vm.abrirModalAdicionarColumnas = abrirModalAdicionarColumnas;
        vm.cargarEntidades = cargarEntidades;
        vm.mostrarMensajeRespuesta = mostrarMensajeRespuesta;
        vm.cambioTipoEntidad = cambioTipoEntidad;
        vm.buscar = buscar;
        vm.limpiarCamposFiltro = limpiarCamposFiltro;
        vm.init = init;
        vm.consultarAccion = consultarAccion;
        vm.abrirModalArchivos = abrirModalArchivos;
        vm.abrirModalFichas = abrirModalFichas;

        vm.obtenerExcel = '/home/ObtenerExcel?tipoProyecto=IdTipoTramite';

        //variables
        vm.tipoFiltro = constantesTipoFiltro.tramites;
        vm.cantidadDeProyectos = 0;
        vm.cantidadDeTramites = 0;
        vm.mostrarFiltro = false;
        vm.mostrarFiltroTramites = false;
        vm.busquedaNombreConId = "";
        vm.Mensaje = "";
        vm.columnas = servicioConsolaTramites.columnasPorDefecto;
        vm.columnasDisponiblesPorAgregar = servicioConsolaTramites.columnasDisponibles;
        vm.mostrarCrearEditarLista = false;
        vm.gruposEntidadesProyectos = false;
        vm.gruposEntidadesTramites = false;
        vm.mostrarMensajeProyectos = false;
        vm.mostrarMensajeTramites = false;
        vm.idTipoProyecto = "";
        vm.idTipoTramite = "";
        vm.yaSeCargoInbox = false;
        vm.tipoEntidad = null;
        vm.filasFiltradas = null;
        vm.listaConfiguracionesRolSector = obtenerConfiguracionesRolSector();
        vm.listaTipoEntidad = [];
        vm.listaFiltroSectores = [];
        vm.listaFiltroEntidades = [];
        vm.listaFiltroEstadoTramites = [];
        vm.listaFiltroTipoTramites = [];

        vm.plantillaTramites = 'src/app/consola/tramites/plantillas/plantillaTramites.html';
        vm.plantillaFilaTramites = 'src/app/consola/tramites/plantillas/plantillaFilaTramites.html';
        vm.sectorEntidadFilaTemplate = 'src/app/consola/tramites/plantillas/plantillaSectorEntidad.html';
        vm.accionesFilaTramitesTemplate = 'src/app/consola/tramites/plantillas/plantillaAccionesTramite.html';
        vm.consultarAccionFlujoTemplate = 'src/app/panelPrincial/componentes/inbox/plantillas/plantillaConsultarAccionFlujo.html';

        vm.puedeVerFiltroCodigo;
        vm.puedeVerFiltroDescripcion;
        vm.puedeVerFiltroFecha;
        vm.puedeVerFiltroValorProprio;
        vm.puedeVerFiltroValorSGR;
        vm.puedeVerFiltroIdentificador;
        vm.puedeVerFiltroSector;
        vm.puedeVerFiltroEntidad;
        vm.puedeVerFiltroIdentificadorCR;
        vm.puedeVerFiltroEstadoTramite;
        vm.downloadPdf = downloadPdf;
        vm.downloadExcel = downloadExcel;
        vm.puedeVerFiltroTipoTramite;
        vm.puedeVerFiltroNombreFlujo;
        vm.puedeVerFiltroAccionFlujo;

        vm.peticionObtenerInbox = {
            IdUsuario: usuarioDNP,
            IdObjeto: idTipoTramite,
            Aplicacion: nombreAplicacionBackbone,
            ListaIdsRoles: sesionServicios.obtenerUsuarioIdsRoles()
        };

        function downloadExcel() {

            servicioConsolaTramites.obtenerExcel(vm.peticionObtenerInbox, vm.tramiteFiltro, buscarColumnasPorColumnasFiltroSeleccionadas()).then(function (retorno) {
                var blob = new Blob([retorno.data], {
                    type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
                });
                FileSaver.saveAs(blob, "ConsolaTramites.xls");
            }, function (error) {
                vm.Mensaje = error.data.Message;
                mostrarMensajeRespuesta();
            });

        }

        function downloadPdf() {
            var peticionObtenerInbox = {
                IdUsuario: usuarioDNP,
                IdObjeto: idTipoTramite,
                Aplicacion: nombreAplicacionBackbone,
                ListaIdsRoles: sesionServicios.obtenerUsuarioIdsRoles()
            };
            servicioConsolaTramites.obtenerPdf(peticionObtenerInbox, vm.tramiteFiltro, buscarColumnasPorColumnasFiltroSeleccionadas()).then(
                function (data) {
                    servicioConsolaTramites.imprimirPdf(data.data).then(function (retorno) {
                        FileSaver.saveAs(retorno.data, nombreDelArchivo(retorno));
                    });
                }, function (error) {
                    vm.Mensaje = error.data.Message;
                    mostrarMensajeRespuesta();
                });
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

        vm.tramiteFiltro = {
            //codigo: {
            //    campo: 'Id',
            //    valor: null,
            //    tipo: constantesCondicionFiltro.igual,
            //    width: '20%'
            //},
            descripcion: {
                campo: 'Descripcion',
                valor: null,
                tipo: constantesCondicionFiltro.contiene
            },
            fechaDesde: {
                campo: 'FechaCreacion',
                valor: null,
                tipo: constantesCondicionFiltro.mayorIgual
            },
            fechaHasta: {
                campo: 'FechaCreacion',
                valor: null,
                tipo: constantesCondicionFiltro.menorIgual
            },
            sectorId: {
                campo: 'SectorId',
                valor: null,
                tipo: constantesCondicionFiltro.igual
            },
            entidadId: {
                campo: 'EntidadId',
                valor: null,
                tipo: constantesCondicionFiltro.igual
            },
            tipoEntidad: {
                campo: 'NombreTipoEntidad',
                valor: null,
                tipo: constantesCondicionFiltro.contiene
            },
            nombreFlujo: {
                campo: 'TipoTramite.Nombre',
                valor: null,
                tipo: constantesCondicionFiltro.contiene
            },
            accionFlujo: {
                campo: 'NombreAccion',
                valor: null,
                tipo: constantesCondicionFiltro.contiene
            }
        };

        vm.columnDefPrincial = [{
            field: 'composto',
            displayName: 'Entidad',
            enableHiding: false,
            width: '96%',
            cellTemplate: vm.sectorEntidadFilaTemplate,
        }];

        vm.columnDefTramite = [{
            field: 'composto',
            displayName: 'Trámite',
            enableFiltering: false,
            showHeader: false,
            enableHiding: false,
            width: '95%',
            cellTemplate: vm.sectorEntidadFilaTemplate,
            headerCellTemplate: '<div></div>'
        }];

        vm.columnDef = [
            //{
            //    field: 'codigo',
            //    displayName: 'Codigo',
            //    enableHiding: false,
            //    width: '9%'
            //},
            {
                field: 'descripcion',
                displayName: 'Descripción',
                enableHiding: false,
                width: '33%',
                cellTooltip: (row, col) => row.entity[col.field]
            },
            {
                field: 'fecha',
                displayName: 'Fecha',
                enableHiding: false,
                width: '9%',
                type: "date",
                cellFilter: 'date:\'dd/MM/yyyy\''
            },
            {
                field: 'entidad',
                displayName: 'Entidad',
                enableHiding: false,
                width: '10%',
                cellTooltip: (row, col) => row.entity[col.field]
            },
            {
                field: 'accionFlujo',
                displayName: 'Nombre/Accion Flujo',
                enableHiding: false,
                enableColumnMenu: false,
                width: '30%',
                cellTemplate: vm.consultarAccionFlujoTemplate,
            },
            {
                field: 'sector',
                displayName: 'Sector',
                enableHiding: false,
                width: '9%',
                cellTooltip: (row, col) => row.entity[col.field]

            },
            {
                field: 'accion',
                displayName: 'Acción',
                enableFiltering: false,
                enableHiding: false,
                enableSorting: false,
                enableColumnMenu: false,
                // pinnedRight: true,
                cellTemplate: vm.accionesFilaTramitesTemplate,
                width: '200'
            }
        ];

        vm.todasColumnasDefinicion = Object.assign([], vm.columnDef);
        vm.listaEntidades = [];
        vm.listaTramites = [];
        vm.listaDatos = [];

        function onRegisterApi(gridApi) {
            $scope.gridApi = gridApi;
        }
                  


        function agregarColumnas() {
            let lista = vm.listaTramites;
            var colAcciones;
            var addCol;
            for (var i = 0, len = lista.length; i < len; i++) {
                var tramite = lista[i];

                for (var j = 0, lenCol = vm.columnas.length; j < lenCol; j++) {
                    var col = vm.columnas[j];

                    if (col !== "Nombre Flujo" && col !== "Accion Flujo" && vm.columnDef.map(x => x && x.displayName).indexOf(col) == -1) {
                        addCol = vm.todasColumnasDefinicion.filter(x => x.displayName == col)[0]
                        colAcciones = vm.todasColumnasDefinicion.filter(x => x.field == 'accion')[0]

                        tramite.subGridOptions.columnDefs.pop();
                        tramite.subGridOptions.columnDefs.push(addCol);
                        tramite.subGridOptions.columnDefs.push(colAcciones);
                    }
                }

                vm.listaTramites[i] = tramite;
            }
        }

        function borrarColumnas() {
            let lista = vm.listaTramites;
            for (var i = 0, len = lista.length; i < len; i++) {
                var tramite = lista[i];

                for (var j = 0, lenCol = vm.columnasDisponiblesPorAgregar.length; j < lenCol; j++) {
                    var indexEliminar = -1;
                    var col = vm.columnasDisponiblesPorAgregar[j];

                    if (col == 'Identificador') {
                        if (tramite.subGridOptions.columnDefs.map(x => x.displayName).indexOf('Tipo de Trámite') > -1) {
                            indexEliminar = tramite.subGridOptions.columnDefs.map(x => x.displayName).indexOf('Tipo de Trámite');
                        }
                    } else {
                        indexEliminar = tramite.subGridOptions.columnDefs.map(x => x.displayName).indexOf(col);
                    }

                    if (indexEliminar >= 0) tramite.subGridOptions.columnDefs.splice(indexEliminar, 1);
                }

                vm.listaTramites[i] = tramite;
            }
        }

        function configurarColumnas() {
            // vuelve a las columnas originales primero
            borrarColumnas();
            // agrega nuevas columnas en todas las filas del modelo
            agregarColumnas();
        }

        function buscarColumnasLocalStorage() {
            if ($localStorage.tipoFiltro) {
                if ($localStorage.tipoFiltro.consolaTramites) {
                    vm.columnas = $localStorage.tipoFiltro.consolaTramites.columnasActivas;
                    vm.columnasDisponiblesPorAgregar = $localStorage.tipoFiltro.consolaTramites.columnasDisponibles;
                }
            }

            vm.puedeVerFiltroCodigo = vm.columnas.indexOf('Codigo') > -1;
            vm.puedeVerFiltroDescripcion = vm.columnas.indexOf('Descripción') > -1;
            vm.puedeVerFiltroFecha = vm.columnas.indexOf('Fecha') > -1;
            vm.puedeVerFiltroValorProprio = vm.columnas.indexOf('Valor Proprio') > -1;
            vm.puedeVerFiltroValorSGR = vm.columnas.indexOf('Valor SGR') > -1;
            vm.puedeVerFiltroIdentificador = vm.columnas.indexOf('Identificador') > -1;
            vm.puedeVerFiltroSector = vm.columnas.indexOf('Sector') > -1;
            vm.puedeVerFiltroEntidad = vm.columnas.indexOf('Entidad') > -1;
            vm.puedeVerFiltroIdentificadorCR = vm.columnas.indexOf('Identificador CR') > -1;
            vm.puedeVerFiltroEstadoTramite = vm.columnas.indexOf('Estado Trámite') > -1;
            vm.puedeVerFiltroTipoTramite = vm.columnas.indexOf('Tipo de Trámite') > -1;
            vm.puedeVerFiltroNombreFlujo = vm.columnas.indexOf('Nombre Flujo') > -1;
            vm.puedeVerFiltroAccionFlujo = vm.columnas.indexOf('Accion Flujo') > -1;
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
            limpiarCamposFiltro();
            vm.tipoEntidad = tipoEntidad;
            vm.tramiteFiltro.tipoEntidad.valor = vm.tipoEntidad;
            cargarEntidades(idTipoProyecto).then(function () {
                mostrarMensajeRespuesta();
                listarFiltroSectores();
                listarFiltroEntidades();
                listarFiltroEstadoTramites();
                listarFiltroTipoTramites();
            });
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

        function tipoEntidadPresenteEnLaConfiguracion(tipoEntidad) {
            var tipoConfiguracion = _.find(vm.listaConfiguracionesRolSector, {
                TipoEntidad: tipoEntidad
            });
            return tipoConfiguracion ? true : false;
        }

        function buscar() {
            return cargarEntidades(idTipoProyecto).then(function () {
                mostrarMensajeRespuesta();
            });
        }

        function limpiarCamposFiltro() {
            //vm.tramiteFiltro.codigo.valor = null;
            vm.tramiteFiltro.descripcion.valor = null;
            vm.tramiteFiltro.fechaDesde.valor = null;
            vm.tramiteFiltro.fechaHasta.valor = null;
            vm.tramiteFiltro.sectorId.valor = null;
            vm.tramiteFiltro.entidadId.valor = null;
            vm.tramiteFiltro.nombreFlujo.valor = null;
            vm.tramiteFiltro.accionFlujo.valor = null;
        }

        function listarFiltroSectores() {
            let listaSectoresGrid = [];
            vm.listaEntidades.forEach(entidade => {
                if (entidade.tipoEntidad == vm.tipoEntidad) {
                    listaSectoresGrid.push({ Id: entidade.idSector, Name: entidade.sector });
                }
            });

            vm.listaFiltroSectores = [];
            $.each(listaSectoresGrid, function (i, el) {
                if ($.inArray(el, vm.listaFiltroSectores) === -1) {
                    vm.listaFiltroSectores.push(el);
                }
            });

        }

        function listarFiltroEntidades() {
            let listaEntidadesGrid = [];
            vm.listaEntidades.forEach(entidade => {
                if (entidade.tipoEntidad == vm.tipoEntidad) {
                    listaEntidadesGrid.push({ Id: entidade.entidadId, Name: entidade.entidad });
                }
            });

            vm.listaFiltroEntidades = [];
            $.each(listaEntidadesGrid, function (i, el) {
                if ($.inArray(el, vm.listaFiltroEntidades) === -1) {
                    vm.listaFiltroEntidades.push(el);
                }
            });
        }

        function listarFiltroEstadoTramites() {
            let listaEstadoTramitesGrid = [];

            vm.listaEntidades.forEach(entidade => {
                if (entidade.tipoEntidad == vm.tipoEntidad) {
                    entidade.subGridOptions.data.forEach(item => {

                        item.subGridOptions.data.forEach(tramite => {

                            listaEstadoTramitesGrid.push({ value: tramite.estadoId, text: tramite.estadoTramite });
                        });

                    });
                }
            });

            const seen = new Set();
            vm.listaFiltroEstadoTramites = [];
            vm.listaFiltroEstadoTramites = listaEstadoTramitesGrid.filter(el => {
                const duplicate = seen.has(el.value);
                seen.add(el.value);
                return !duplicate;
            });

        }

        function listarFiltroTipoTramites() {
            let listaTipoTramitesGrid = [];

            vm.listaEntidades.forEach(entidade => {
                if (entidade.tipoEntidad == vm.tipoEntidad) {
                    entidade.subGridOptions.data.forEach(item => {

                        item.subGridOptions.data.forEach(tramite => {

                            listaTipoTramitesGrid.push({ value: tramite.tipoTramiteId, text: tramite.tipoTramite });
                        });

                    });
                }
            });

            const seen = new Set();
            vm.listaFiltroTipoTramites = [];
            vm.listaFiltroTipoTramites = listaTipoTramitesGrid.filter(el => {
                const duplicate = seen.has(el.value);
                seen.add(el.value);
                return !duplicate;
            });

        }

        //Implementaciones
        $scope.$on('AutorizacionConfirmada', function () {
            $timeout(function () {
                if (vm.yaSeCargoInbox === false) {
                    var roles = sesionServicios.obtenerUsuarioIdsRoles();
                    if (roles != null && roles.length > 0) {
                        activar(); //Realizar la carga
                    } else {
                        vm.Mensaje = $filter('language')('ErrorUsuarioSinPermisosAplicacion');
                        mostrarMensajeRespuesta(idTipoProyecto);
                    }
                }
            });
        });

        function init() {
            // buscar columnas en el localstorage
            buscarColumnasLocalStorage();

            if (!vm.gridOptions) {
                vm.gridOptions = {
                    expandableRowTemplate: vm.plantillaTramites,
                    expandableRowScope: {
                        subGridVariable: 'subGridScopeVariable'
                    },
                    expandableRowHeight: 250,
                    enableFiltering: false,
                    showHeader: false,
                    paginationPageSizes: [10, 15, 25, 50, 100],
                    paginationPageSize: 10,
                    enableVerticalScrollbar: uiGridConstants.scrollbars.WHEN_NEEDED,
                    enableHorizontalScrollbar: uiGridConstants.scrollbars.WHEN_NEEDED,
                    onRegisterApi: onRegisterApi
                };

                vm.gridOptions.columnDefs = vm.columnDefPrincial;
                vm.gridOptions.data = vm.listaEntidades;
            }

            vm.idTipoTramite = idTipoTramite;
            if (vm.yaSeCargoInbox === false) {
                var roles = sesionServicios.obtenerUsuarioIdsRoles();
                if (backboneServicios.estaAutorizado() && roles != null && roles.length > 0) {
                    activar();
                }
                //else {
                //    vm.Mensaje = $filter('language')('ErrorUsuarioSinPermisosAplicacion');
                //    mostrarMensajeRespuesta(idTipoProyecto);
                //}
            }

            //configurarColumnas();
        };


        function activar() {
            vm.yaSeCargoInbox = true;

            obtenerInbox(idTipoProyecto);
        }

        function buscarColumnasPorColumnasFiltroSeleccionadas() {
            let listaColumnas = [];
            let columna = ''

            for (let i = 0; i < vm.columnas.length; i++) {
                var nombreColumnasSeleccionadaFiltro = vm.columnas[i];

                if (nombreColumnasSeleccionadaFiltro == 'Nombre Flujo' || nombreColumnasSeleccionadaFiltro == 'Accion Flujo') {
                    nombreColumnasSeleccionadaFiltro = 'Nombre/Accion Flujo';
                }

                columna = vm.todasColumnasDefinicion.filter(x => x.displayName == nombreColumnasSeleccionadaFiltro)[0].field;
                if (listaColumnas.indexOf(columna) == -1) {
                    listaColumnas.push(columna);
                }
            }

            return listaColumnas;
        }

        function cargarEntidades(idObjeto) {
            const columnasVisibles = buscarColumnasPorColumnasFiltroSeleccionadas();

            return servicioConsolaTramites.obtenerTramites(vm.peticionObtenerInbox, vm.tramiteFiltro, columnasVisibles).then(
                function (respuesta) {

                    vm.listaEntidades = [];
                    vm.listaTramites = [];
                    vm.listaDatos = [];

                    if (respuesta.data.ListaGrupoTramiteEntidad && respuesta.data.ListaGrupoTramiteEntidad.length > 0) {
                        vm.listaEntidades = [];
                        const listaGrupoEntidades = respuesta.data.ListaGrupoTramiteEntidad;
                        listaGrupoEntidades.forEach(entidad => {
                            const nombreEntidad = entidad.NombreEntidad;
                            const tipoEntidad = entidad.GrupoTramites[0].NombreTipoEntidad;
                            const nombreSector = entidad.Sector;
                            const idSector = entidad.IdSector;
                            const idEntidad = entidad.EntidadId;

                            vm.listaTramites = [];
                            entidad.GrupoTramites.forEach(tramite => {

                                const nombreTipoTramite = tramite.NombreTipoTramite;
                                vm.listaDatos = [];
                                tramite.ListaTramites.forEach(instancia => {
                                    vm.listaDatos.push({
                                        codigo: instancia.Id,
                                        descripcion: instancia.Descripcion,
                                        fecha: instancia.FechaCreacion,
                                        valorProprio: instancia.ValorProprio,
                                        valorSGR: instancia.ValorSGP,
                                        tipoTramite: instancia.NombreTipoTramite,
                                        entidad: nombreEntidad,
                                        identificadorCR: instancia.IdentificadorCR,
                                        estadoTramite: instancia.DescEstado,
                                        sector: instancia.NombreSector,
                                        estadoId: instancia.EstadoId,
                                        tipoTramiteId: instancia.TipoTramiteId,
                                        IdObjetoNegocio: instancia.IdObjetoNegocio,
                                        NombreObjetoNegocio: instancia.NombreObjetoNegocio,
                                        NombreAccion: instancia.NombreAccion,
                                        IdInstancia: instancia.IdInstancia,
                                        NombreFlujo: instancia.NombreTipoTramite,
                                        entidadId: idEntidad,
                                        IdAccion: instancia.IdAccion
                                    });
                                });

                                vm.listaTramites.push({
                                    sector: 'Tramite',
                                    entidad: nombreTipoTramite,
                                    subGridOptions: {
                                        columnDefs: vm.columnDef,
                                        appScopeProvider: $scope,
                                        paginationPageSizes: [5, 10, 15, 25, 50, 100],
                                        paginationPageSize: 5,
                                        enableVerticalScrollbar: uiGridConstants.scrollbars.WHEN_NEEDED,
                                        enableHorizontalScrollbar: uiGridConstants.scrollbars.WHEN_NEEDED,
                                        data: vm.listaDatos
                                    }
                                });
                            });

                            vm.listaEntidades.push({
                                sector: nombreSector,
                                entidad: nombreEntidad,
                                tipoEntidad: tipoEntidad,
                                idSector: idSector,
                                entidadId: idEntidad,
                                subGridOptions: {
                                    columnDefs: vm.columnDefTramite,
                                    appScopeProvider: $scope,
                                    expandableRowTemplate: vm.plantillaFilaTramites,
                                    expandableRowScope: {
                                        subGridVariable: 'subGridScopeVariable'
                                    },
                                    expandableRowHeight: 210,
                                    enableFiltering: false,
                                    showHeader: false,
                                    enableVerticalScrollbar: uiGridConstants.scrollbars.WHEN_NEEDED,
                                    enableHorizontalScrollbar: uiGridConstants.scrollbars.WHEN_NEEDED,
                                    data: vm.listaTramites
                                }
                            });
                        });

                        configurarColumnas();
                    }

                    vm.gridOptions.data = vm.listaEntidades;
                    vm.filasFiltradas = vm.gridOptions.data.length > 0;
                    vm.Mensaje = respuesta.data.Mensaje;
                },
                function (error) {
                    if (error) {
                        if (error.status) {
                            switch (error.status) {
                                case 401:
                                    vm.Mensaje = $filter('language')('ErrorUsuarioSinPermisosAplicacion');
                                    break;
                                case 500:
                                    vm.Mensaje = $filter('language')('ErrorObtenerDatos');
                                default:
                                    vm.Mensaje = error.statusText;
                                    break;
                            }
                        }
                    }
                    mostrarMensajeRespuesta(idObjeto);
                }
            );
        }

        function conmutadorFiltro() {
            limpiarCamposFiltro();
            vm.mostrarFiltro = !vm.mostrarFiltro;
        }

        function mostrarMensajeRespuesta(idObjeto) {
            if (idObjeto === vm.idTipoProyecto && vm.cantidadDeProyectos === 0) {
                vm.mostrarMensajeProyectos = true;
            } else {
                vm.mostrarMensajeProyectos = false;
            }

            if (idObjeto === idTipoTramite && vm.cantidadDeTramites === 0) {
                vm.mostrarMensajeTramites = true;
            } else {
                vm.mostrarMensajeTramites = false;
            }
        }

        function conmutadorFiltro() {
            limpiarCamposFiltro();
            vm.mostrarFiltro = !vm.mostrarFiltro;
        }

        function obtenerInbox(idObjeto) {
            limpiarCamposFiltro();

            return cargarEntidades(idObjeto).then(function () {

                let tipoEntidad;
                for (let i = 0; i < vm.listaTipoEntidad.length; i++) {
                    tipoEntidad = vm.listaTipoEntidad[i];

                    if (!tipoEntidad.Deshabilitado) {
                        vm.tipoEntidad = tipoEntidad.Nombre;
                        cambioTipoEntidad(vm.tipoEntidad);
                        break;
                    }
                }
                listarFiltroSectores();
                listarFiltroEntidades();
                listarFiltroEstadoTramites();
                listarFiltroTipoTramites();
                mostrarMensajeRespuesta(idObjeto);
            });
        };

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
                        'consolaTramites': {
                            'columnasActivas': selectedItem.columnasActivas,
                            'columnasDisponibles': selectedItem.columnasDisponibles
                        }
                    };
                } else {
                    $localStorage.tipoFiltro['consolaTramites'] = {
                        'columnasActivas': selectedItem.columnasActivas,
                        'columnasDisponibles': selectedItem.columnasDisponibles
                    }
                }
                buscarColumnasLocalStorage();
                configurarColumnas();
            }, function () {
                $log.info('Modal dismissed at: ' + new Date());
            });
        };

        function consultarAccion(idInstancia, nombreProyecto, idObjetoNegocio, idEntidad) {

            $sessionStorage.idInstancia = $sessionStorage.idInstanciaFlujoPrincipal = idInstancia;
            $sessionStorage.nombreProyecto = nombreProyecto;
            $sessionStorage.idObjetoNegocio = idObjetoNegocio;
            $sessionStorage.idEntidad = idEntidad;
            $timeout(function () {
                $location.path('/Acciones/ConsultarAcciones');
            }, 300);
        }

        function abrirModalArchivos(row) {

            $uibModal.open({
                animation: $scope.animationsEnabled,
                templateUrl: 'src/app/consola/tramites/modales/modalDocumentos.html',
                controller: 'modalDocumentosController',
                controllerAs: "vm",
                //openedClass: "consola-modal-instancias",
                //size: 'lg',
                resolve: {
                    objTramite: row
                }
            }).result.then(function (result) {
                consol.log(result)
            }, function (reason) {

                consol.log(reason)
            });
        }

        function abrirModalFichas(row) {
            servicioConsolaTramites.obtenerIdAplicacion(row.IdInstancia).then(res => {
                $sessionStorage.idAplicacion = res.data;
                $uibModal.open({
                    animation: $scope.animationsEnabled,
                    templateUrl: 'src/app/consola/modales/fichas/modalFichas.html',
                    controller: 'modalFichasController',
                    openedClass: "dialog-modal-archivo",
                    controllerAs: "vm",
                    resolve: {
                        entity: () => row,
                        esProyecto: () => false
                    }
                });
            }, err => {
                toastr.error("Ocurrió un error al consultar el idAplicacion");
            })
        }
        
    }

    angular.module('backbone').controller('consolaTramitesController', consolaTramitesController)

})()