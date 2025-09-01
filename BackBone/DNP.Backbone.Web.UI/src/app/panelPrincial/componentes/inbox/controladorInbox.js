var inboxCtrl = null;

(function () {
    'use strict';
    angular.module('backbone').controller('inboxController', inboxController);

    inboxController.$inject = [
        '$scope',
        'servicioPanelPrincipal',
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
        'FileSaver',
        'Blob',
        'utilidades',
        '$routeParams',
        'flujoServicios',
        'autorizacionServicios',
        'transversalSgrServicio'
    ];

    function inboxController(
        $scope,
        servicioPanelPrincipal,
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
        FileSaver,
        Blob,
        utilidades,
        $routeParams,
        flujoServicios,
        autorizacionServicios,
        transversalSgrServicio) {

        var vm = this;
        inboxCtrl = vm;


        //Filtro

        vm.arrowIcoDown2 = "glyphicon-chevron-down-busqDNP";
        vm.arrowIcoUp2 = "glyphicon-chevron-up-busqDNP";
        vm.BusquedaRealizada = false;
        //Métodos
        vm.downloadPdf = downloadPdf;
        vm.downloadExcel = downloadExcel;
        vm.conmutadorFiltro = conmutadorFiltro;
        vm.obtenerInbox = obtenerInbox;
        vm.abrirModalAdicionarColumnas = abrirModalAdicionarColumnas;
        vm.cargarEntidades = cargarEntidades;
        vm.mostrarMensajeRespuesta = mostrarMensajeRespuesta;
        vm.cambioTipoEntidad = cambioTipoEntidad;
        vm.buscar = buscar;
        vm.limpiarCamposFiltro = limpiarCamposFiltro;
        vm.borrarFila = borrarFila;
        vm.consultarAccion = consultarAccion;
        vm.activarInstancia = activarInstancia;
        vm.pausarInstancia = pausarInstancia;
        vm.detenerInstancia = detenerInstancia;
        vm.cancelarInstanciaMisProcesos = cancelarInstanciaMisProcesos;
        vm.mostrarLog = mostrarLog;
        vm.obtenerIdAP = obtenerIdAP;
        vm.restablecerBusqueda = restablecerBusqueda;
        vm.cantidadFlujosProgramacion = 0;

        vm.abrirLogInstancias = abrirLogInstancias;
        vm.redirectPaginaActual = redirectPaginaActual;
        vm.toggleEvento = toggleEvento;
        vm.VisiblePlaneacion = false;
        vm.VisibleRecursos = false;
        vm.VisibleEjecucion = false;
        vm.VisibleEvaluacion = false;
        vm.classProcesos = "unProceso";
        vm.classDivSeleccionado = "u1923_div";
        vm.classIcoSeleccionado = "u1923";
        vm.classFlechas = "";
        vm.imgenSeleccion = "Img/etapas/macroprocesos/u829.svg";

        //#region Componente Crear Proceso

        vm.mostrarFlujosDisponibles = mostrarFlujosDisponibles;
        vm.mostrarModal = mostrarModal;
        vm.flujosAutorizados = [];
        vm.listaProcesos = [];
        var modalConfigDefault = {
            animation: true,
            controllerAs: "vm",
            backdrop: false,
            keyboard: false,
            size: 'lg'
        }
        vm.flujosInboxSGR = '';

        //#endregion

        //Acordeon

        function redirectPaginaActual() {
            var url = $location.url();
            $location.url(url);
        }

        vm.AbrilNivel = function (idEntidad) {
            vm.listaEntidades.forEach(function (value, index) {
                if (value.idEntidad == idEntidad) {
                    if (value.estadoEntidad == '+')
                        value.estadoEntidad = '-';
                    else
                        value.estadoEntidad = '+';
                }
            });
        }


        //variables
        vm.tipoFiltro = constantesTipoFiltro.proyecto;
        vm.cantidadDeProyectos = 0;
        vm.cantidadDeTramites = 0;
        vm.mostrarFiltro = false;
        vm.mostrarFiltroTramites = false;
        vm.busquedaNombreConId = "";
        vm.Mensaje = "";
        vm.columnas = servicioPanelPrincipal.columnasPorDefectoProyecto;
        vm.columnasDisponiblesPorAgregar = servicioPanelPrincipal.columnasDisponiblesProyecto;
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
        vm.nombreTipoEntidad = '';
        vm.listaFiltroSectores = [];
        vm.listaFiltroEntidades = [];
        vm.listaFiltroPrioridades = [];
        vm.listaFiltroEstadoProyectos = [];
        vm.listaFiltroEstadoInstancias = [];
        vm.macroProcesosCantidad = [];
        vm.cantidades = [];

        vm.obtenerExcel = '/home/ObtenerExcel?tipoProyecto=IdTipoProyecto';
        vm.proyectoFilaTemplate = 'src/app/panelPrincial/componentes/inbox/plantillas/plantillaProyecto.html';
        vm.sectorEntidadFilaTemplate = 'src/app/panelPrincial/componentes/inbox/plantillas/plantillaSectorEntidad.html';
        vm.accionesFilaProyectoTemplate = 'src/app/panelPrincial/componentes/inbox/plantillas/plantillaAccionesProyecto.html';
        vm.consultarAccionFlujoTemplate = 'src/app/panelPrincial/componentes/inbox/plantillas/plantillaConsultarAccionFlujo.html';
        vm.criticidadTemplate = 'src/app/panelPrincial/componentes/inbox/plantillas/plantillaCriticidad.html';

        vm.puedeVerFiltroIdentificador;
        vm.puedeVerFiltroBpin;
        vm.puedeVerFiltroNombre;
        vm.puedeVerFiltroEntidad;
        vm.puedeVerFiltroIdentificadorCR;
        vm.puedeVerFiltroAnoInicio;
        vm.puedeVerFiltroAnoFin;
        vm.puedeVerFiltroPrioridad;
        vm.puedeVerFiltroEstadoProyecto;
        vm.puedeVerFiltroSector;
        vm.puedeVerFiltroNombreFlujo;
        vm.puedeVerFiltroAccionFlujo;
        vm.puedeVerFiltroEstadoInstancia;

        vm.peticionObtenerInbox;

        vm.etapa = $routeParams['etapa'];
        vm.tipoProceso = 'proyectos';
        var botonesacciones = $sessionStorage.usuario.permisos.BotonesOpciones;
        vm.puedeVerCampoAccionesGrid = botonesacciones.length > 0 && botonesacciones.find(x => x.Nombre === constantesAutorizacion.btnOcultarAccionesProyecto) ? true : false;

        if (vm.etapa == "pl") {
            vm.nombreEtapa = "Planeación";
            toggleEvento(1);
        }
        if (vm.etapa == "gr") {
            vm.nombreEtapa = "Gestión de recursos";
            toggleEvento(2);
        }
        if (vm.etapa == "ej") {
            vm.nombreEtapa = "Ejecución";
            toggleEvento(3);
        }
        if (vm.etapa == "ev") {
            vm.nombreEtapa = "Evaluación";
            toggleEvento(4);
        }


        vm.columnasReporte = ['SectorNombre', 'NombreEntidad', 'ProyectoId', 'IdObjetoNegocio', 'EstadoProyecto', 'NombreObjetoNegocio', 'NombreFlujo', 'CodigoProceso', 'EstadoInstancia', 'FechaCreacion', 'NombreAccion', 'FechaPaso'];

        function toggleEvento(proceso) {


            if (proceso === 1 && !vm.VisiblePlaneacion) {
                vm.VisiblePlaneacion = true;
                vm.classFlechas = "planeacionSeleccionado";
                vm.imgenSeleccionPlaneacion = "Img/etapas/macroprocesos/u829.svg";
            }
            else {
                vm.VisiblePlaneacion = false;
                vm.imgenSeleccionPlaneacion = "Img/etapas/macroprocesos/u838.svg";
            }
            if (proceso === 2 && !vm.VisibleRecursos) {
                vm.VisibleRecursos = true;
                vm.classFlechas = "recursosSeleccionado";
                vm.imgenSeleccionRecursos = "Img/etapas/macroprocesos/u840.svg";
            }
            else {
                vm.VisibleRecursos = false;
                vm.imgenSeleccionRecursos = "Img/etapas/macroprocesos/u821.svg";
            }
            if (proceso === 3 && !vm.VisibleEjecucion) {
                vm.VisibleEjecucion = true;
                vm.classFlechas = "ejecucionSeleccionado";
                vm.imgenSeleccionEjecucion = "Img/etapas/macroprocesos/u840.svg";
            }
            else {
                vm.VisibleEjecucion = false;
                vm.imgenSeleccionEjecucion = "Img/etapas/macroprocesos/u821.svg";
            }
            if (proceso === 4 && !vm.VisibleEvaluacion) {
                vm.VisibleEvaluacion = true;
                vm.classFlechas = "evaluacionSeleccionado";
                vm.imgenSeleccionEvaluacion = "Img/etapas/macroprocesos/u840.svg";
            }
            else {
                vm.VisibleEvaluacion = false;
                vm.imgenSeleccionEvaluacion = "Img/etapas/macroprocesos/u821.svg";
            }


        }
        function downloadExcel() {
            var peticionObtenerInbox = {
                IdUsuario: usuarioDNP,
                IdObjeto: idTipoProyecto,
                Aplicacion: nombreAplicacionBackbone,
                ListaIdsRoles: sesionServicios.obtenerUsuarioIdsRoles()
            };
            servicioPanelPrincipal.obtenerExcelProyetos(peticionObtenerInbox, vm.proyectoFiltro, vm.columnasReporte).then(function (retorno) {
                var blob = new Blob([retorno.data], {
                    type: "application/octet-stream"
                });
                // FileSaver.saveAs(blob,nombreDelArchivo(retorno));
                FileSaver.saveAs(blob, "Proyectos.xlsx");
            }, function (error) {
                vm.Mensaje = error.data.Message;
                mostrarMensajeRespuesta();
            });

        }

        function downloadPdf() {
            var peticionObtenerInbox = {
                IdUsuario: usuarioDNP,
                IdObjeto: idTipoProyecto,
                Aplicacion: nombreAplicacionBackbone,
                ListaIdsRoles: sesionServicios.obtenerUsuarioIdsRoles()
            };
            servicioPanelPrincipal.obtenerPdfInboxProyectos(peticionObtenerInbox, vm.proyectoFiltro, vm.columnasReporte).then(
                function (data) {
                    servicioPanelPrincipal.imprimirPdfProyectos(data.data).then(function (retorno) {
                        FileSaver.saveAs(retorno.data, nombreDelArchivo(retorno));
                    });
                },
                function (error) {
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

        vm.proyectoFiltro = {
            identificador: null,
            nombre: null,
            sectorId: null,
            entidadId: null,
            identificadorCR: null,
            horizonteInicio: null,
            horizonteFin: null,
            prioridad: null,
            estadoProyecto: null,
            tipoEntidadId: null,
            bpin: null,
            estadoProyectoId: null,
            tipoEntidad: null,
            nombreFlujo: null,
            accionFlujo: null,
            estadoInstanciaId: null,
            IdsEtapas: getIdEtapa(),
            procesoId: null,
            macroproceso: vm.nombreEtapa,
            codigoProceso: null
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
                field: 'accionFlujo',
                displayName: 'Proceso/Paso',
                enableHiding: false,
                enableColumnMenu: false,
                width: '20%',
                cellTemplate: vm.consultarAccionFlujoTemplate,
                cellTooltip: (row, col) => row.entity[col.field]
            },
            {
                field: 'ProyectoId',
                displayName: 'Identificador',
                enableHiding: false,
                width: '10%',
                cellTooltip: (row, col) => row.entity[col.field]
            },
            {
                field: 'IdObjetoNegocio',
                displayName: 'BPIN',
                enableHiding: false,
                width: '10%',
                cellTooltip: (row, col) => row.entity[col.field]
            },
            {
                field: 'NombreObjetoNegocio',
                displayName: 'Nombre',
                enableHiding: false,
                width: '25%',
                cellTooltip: (row, col) => row.entity[col.field]
            },
            {
                field: 'NombreEntidad',
                displayName: 'Entidad',
                enableHiding: false,
                width: '25%',
                cellTooltip: (row, col) => row.entity[col.field]
            },
            {
                field: 'DescripcionCR',
                displayName: 'Identificador CR',
                enableHiding: false,
                width: '20%',
                cellTooltip: (row, col) => row.entity[col.field]
            },
            {
                field: 'Criticidad',
                displayName: 'Prioridad',
                enableHiding: false,
                enableColumnMenu: false,
                width: '8%',
                cellTemplate: vm.criticidadTemplate,
                cellTooltip: (row, col) => row.entity[col.field]
            },
            {
                field: 'EstadoProyecto',
                displayName: 'Estado del Proyecto',
                enableHiding: false,
                width: '10%',
                cellTooltip: (row, col) => row.entity[col.field]
            },
            {
                field: 'EstadoInstancia',
                displayName: 'Estado',
                enableHiding: false,
                width: '10%',
                cellTooltip: (row, col) => row.entity[col.field]
            },
            {
                field: 'Horizonte',
                displayName: 'Horizonte',
                enableHiding: false,
                width: '10%',
                cellTooltip: (row, col) => row.entity[col.field]
            },
            {
                field: 'SectorNombre',
                displayName: 'Sector',
                enableHiding: false,
                width: '20%',
                cellTooltip: (row, col) => row.entity[col.field]
            },
            {
                field: 'accion',
                displayName: 'Acción',
                enableFiltering: false,
                enableHiding: false,
                enableSorting: false,
                enableColumnMenu: false,
                pinnedRight: true,
                cellTemplate: vm.accionesFilaProyectoTemplate,
                width: '200',
                visible: vm.puedeVerCampoAccionesGrid
            }
        ];

        vm.todasColumnasDefinicion = Object.assign([], vm.columnDef);
        vm.listaDatos = [];
        vm.listaEntidades = [];


        // grid main
        vm.gridOptions;

        function abrirLogInstancias(row) {


            var modalInstance = $uibModal.open({
                animation: $scope.animationsEnabled,
                templateUrl: 'src/app/comunes/log/modalLogInstancias.html',
                controller: 'modalLogInstanciasController',
                controllerAs: "vm",
                openedClass: "consola-modal-soportesDNP",
                resolve: {
                    idInstancia: () => row.IdInstancia,
                    BPIN: () => row.IdObjetoNegocio,
                    nombreFlujo: () => row.NombreFlujo,
                    codigoProceso: () => row.CodigoProceso
                }
            });

            modalInstance.result.then(function (selectedItem) {


            }, function () {

            });
        }

        function restablecerBusqueda() {
            vm.limpiarCamposFiltro();
            vm.buscar(true);
            var iconoLimpiar = document.getElementById('icoLimpiarBusqueda');
            iconoLimpiar.src = "Img/IcolimpiarBusqD.svg";
        }

        function consultarAccion(idInstancia, nombreProyecto, idObjetoNegocio, idEntidad, nombreFlujo, row) {
            $sessionStorage.idInstancia = $sessionStorage.idInstanciaFlujoPrincipal = idInstancia;
            $sessionStorage.nombreProyecto = nombreProyecto;
            $sessionStorage.idObjetoNegocio = idObjetoNegocio;
            $sessionStorage.idEntidad = idEntidad;
            $sessionStorage.crTypeId = row.CRTypeId;
            $sessionStorage.resourceGroupId = row.ResourceGroupId;
            if (nombreFlujo !== null && row !== null) {
                $sessionStorage.nombreFlujo = nombreFlujo;
                $sessionStorage.etapa = vm.etapa;
                $sessionStorage.InstanciaSeleccionada = row;
                $sessionStorage.proyectoId = row.ProyectoId;
                $sessionStorage.flujoSeleccionado = row.FlujoId;
            }
            $timeout(function () {
                $location.path('/Acciones/ConsultarAcciones');
            }, 300);
        }

        function onRegisterApi(gridApi) {
            $scope.gridApi = gridApi;

            $scope.gridApi.expandable.on.rowExpandedBeforeStateChanged($scope, function (row) {
                if (row.entity.subGridOptions.data.length === 1)
                    $scope.vm.gridOptions.expandableRowHeight = 105;
                else
                    $scope.vm.gridOptions.expandableRowHeight = 65 + row.entity.subGridOptions.data.length * 37;
            })
        }

        function borrarFila(fila) {

        }

        function cargueInicial() {
            limpiarCamposFiltro();
            servicioPanelPrincipal.obtenerMacroprocesosCantidad().then(exitoCantidad, errorCantidad);
            function exitoCantidad(respuesta) {
                if (respuesta.data != null) {
                    vm.macroProcesosCantidad = [];
                    vm.cantidades = [];
                    for (var i = 0; i < respuesta.data.length; i++) {
                        vm.macroProcesosCantidad.push(respuesta.data[i]);
                    }
                    $localStorage.cantidadMacroprocesosList = vm.macroProcesosCantidad;
                    var data = vm.macroProcesosCantidad.filter(x => (String(x.NivelPadreId).toUpperCase() == constantesBackbone.idEtapaPlaneacion ||
                        String(x.NivelPadreId).toUpperCase() == constantesBackbone.idEtapaViabilidadRegistro ||
                        String(x.NivelPadreId).toUpperCase() == constantesBackbone.idEtapaAjustes ||
                        String(x.NivelPadreId).toUpperCase() == constantesBackbone.idEtapaPriorizacion) && x.TipoObjeto == 'Proyecto');
                    var data = vm.macroProcesosCantidad.filter(x => (String(x.NivelPadreId).toUpperCase() == constantesBackbone.idEtapaPlaneacion ||
                        String(x.NivelPadreId).toUpperCase() == constantesBackbone.idEtapaViabilidadRegistro ||
                        String(x.NivelPadreId).toUpperCase() == constantesBackbone.idEtapaAjustes ||
                        String(x.NivelPadreId).toUpperCase() == constantesBackbone.idEtapaPriorizacion) && x.TipoObjeto == 'Proyecto');
                    var cantidadPlaneacionProyecto = 0;
                    if (data.length > 0) {
                        for (var i = 0; i < data.length; i++) {
                            cantidadPlaneacionProyecto = cantidadPlaneacionProyecto + data[i].Cantidad;
                        }
                    }
                    if (vm.etapa == 'pl') {
                        vm.listaProcesos = $localStorage.procesosList.filter(x => String(x.NivelPadreId).toUpperCase() == constantesBackbone.idEtapaPlaneacion);
                    }
                    var dataGR = vm.macroProcesosCantidad.filter(x => (String(x.NivelPadreId).toUpperCase() == constantesBackbone.idEtapaGestionRecursos ||
                        String(x.NivelPadreId).toUpperCase() == constantesBackbone.idEtapaSolicitudRecursos ||
                        String(x.NivelPadreId).toUpperCase() == constantesBackbone.idEtapaRevisionRequisitos ||
                        String(x.NivelPadreId).toUpperCase() == constantesBackbone.idEtapaAprobacion ||
                        String(x.NivelPadreId).toUpperCase() == constantesBackbone.idEtapaAjustesGR) && x.TipoObjeto == 'Proyecto');
                    var cantidadRecursosProyecto = 0;
                    if (dataGR.length > 0) {
                        for (var i = 0; i < dataGR.length; i++) {
                            cantidadRecursosProyecto = cantidadRecursosProyecto + dataGR[i].Cantidad;
                        }
                    }
                    if (vm.etapa == 'gr') {
                        vm.listaProcesos = $localStorage.procesosList.filter(x => String(x.NivelPadreId).toUpperCase() == constantesBackbone.idEtapaGestionRecursos);
                    }
                    var dataE = vm.macroProcesosCantidad.filter(x => (String(x.NivelPadreId).toUpperCase() == constantesBackbone.idEtapaNuevaEjecucion ||
                        String(x.NivelPadreId).toUpperCase() == constantesBackbone.idEtapaProgramacionEjecucion ||
                        String(x.NivelPadreId).toUpperCase() == constantesBackbone.idEtapaAjustesEjecucion ||
                        String(x.NivelPadreId).toUpperCase() == constantesBackbone.idEtapaTramitesEjecucion ||
                        String(x.NivelPadreId).toUpperCase() == constantesBackbone.idEtapaSeguimientoControl) && x.TipoObjeto == 'Proyecto');
                    var cantidadEjecucionProyecto = 0;
                    if (dataE.length > 0) {
                        for (var i = 0; i < dataE.length; i++) {
                            cantidadEjecucionProyecto = cantidadEjecucionProyecto + dataE[i].Cantidad;
                        }
                    }
                    if (vm.etapa == 'ej') {
                        vm.listaProcesos = $localStorage.procesosList.filter(x => String(x.NivelPadreId).toUpperCase() == constantesBackbone.idEtapaNuevaEjecucion);

                    }
                    var dataEV = vm.macroProcesosCantidad.filter(x => (String(x.NivelPadreId).toUpperCase() == constantesBackbone.idEtapaEvaluacion ||
                        String(x.NivelPadreId).toUpperCase() == constantesBackbone.idEtapaCortoPlazo ||
                        String(x.NivelPadreId).toUpperCase() == constantesBackbone.idEtapaMedianoPlazo ||
                        String(x.NivelPadreId).toUpperCase() == constantesBackbone.idEtapaLargoPlazo) && x.TipoObjeto == 'Proyecto');
                    var cantidadEvaluacionProyecto = 0;
                    if (dataEV.length > 0) {
                        for (var i = 0; i < dataEV.length; i++) {
                            cantidadEvaluacionProyecto = cantidadEvaluacionProyecto + dataEV[i].Cantidad;
                        }
                    }
                    if (vm.etapa == 'ev') {
                        vm.listaProcesos = $localStorage.procesosList.filter(x => String(x.NivelPadreId).toUpperCase() == constantesBackbone.idEtapaEvaluacion);
                    }


                    var cantidadTramiteEjecucion = 0;

                    var dataTramite = vm.macroProcesosCantidad.filter(x => (String(x.IdNivelPadre == constantesBackbone.idEtapaNuevaEjecucion)) && x.TipoObjeto == 'Trámite');
                    if (dataTramite.length > 0) {
                        for (var i = 0; i < dataTramite.length; i++) {
                            cantidadTramiteEjecucion = cantidadTramiteEjecucion + dataTramite[i].Cantidad;
                        }
                    }

                    var cantidadProgramacionEjecucion = 0;

                    var dataPE = vm.macroProcesosCantidad.filter(x => (String(x.IdNivelPadre == constantesBackbone.idEtapaProgramacionEjecucion)) && x.TipoObjeto == 'Programación');
                    if (dataPE.length > 0) {
                        for (var i = 0; i < dataPE.length; i++) {
                            cantidadProgramacionEjecucion = cantidadProgramacionEjecucion + dataPE[i].Cantidad;
                        }
                    }
                    if (cantidadProgramacionEjecucion > 0) {
                        vm.cantidadFlujosProgramacion = cantidadProgramacionEjecucion;
                    }
                    vm.cantidades.push({ 'PProyecto': cantidadPlaneacionProyecto, 'GRProyecto': cantidadRecursosProyecto, 'EJProyecto': cantidadEjecucionProyecto, 'EVProyecto': cantidadEvaluacionProyecto, 'EJTramite': cantidadTramiteEjecucion, 'EJProgramacion': cantidadProgramacionEjecucion, 'EJTotal': cantidadTramiteEjecucion + cantidadEjecucionProyecto + cantidadProgramacionEjecucion });
                    $localStorage.cantidadesMisproyectos = vm.cantidades;
                }
            }
            function errorCantidad(respuesta) {
                console.log(`error: ${respuesta}`);
            }


            vm.idTipoProyecto = idTipoProyecto;
            vm.idTipoTramite = idTipoTramite;
            // buscar columnas en el localstorage
            buscarColumnasLocalStorage();

            if (!vm.gridOptions) {
                vm.gridOptions = {
                    expandableRowTemplate: vm.proyectoFilaTemplate,
                    expandableRowScope: {
                        subGridVariable: 'subGridScopeVariable'
                    },
                    expandableRowHeight: 400,
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
                };

                vm.gridOptions.columnDefs = vm.columnDefPrincial;
                vm.gridOptions.data = vm.listaEntidades;
            }
            var roles = sesionServicios.obtenerUsuarioIdsRoles();
            if (backboneServicios.estaAutorizado() && roles != null && roles.length > 0) {
                vm.peticionObtenerInbox = {
                    // ReSharper disable once UndeclaredGlobalVariableUsing
                    IdUsuario: usuarioDNP,
                    IdObjeto: idTipoProyecto,
                    // ReSharper disable once UndeclaredGlobalVariableUsing
                    Aplicacion: nombreAplicacionBackbone,
                    ListaIdsRoles: roles
                };

                activar(); //Realizar la carga
            }

            configurarNombreEtapa();
            cargarFlujosProgramacion();
            $sessionStorage.InstanciaSeleccionada = undefined;
        }

        this.$onInit = function () {
            cargueInicial();
        };

        function agregarColumnas() {
            let lista = vm.listaEntidades;
            var addColFechaCreacion;
            var colAcciones;
            var addCol;
            for (var i = 0, len = lista.length; i < len; i++) {
                var entidad = lista[i];

                for (var j = 0, lenCol = vm.columnas.length; j < lenCol; j++) {
                    var col = vm.columnas[j];

                    if (col == 'Año de inicio' || col == 'Año Fin') {
                        if (vm.columnDef.map(x => x.displayName).indexOf('Horizonte') == -1) {
                            addColFechaCreacion = vm.todasColumnasDefinicion.filter(x => x.displayName == 'Horizonte')[0]
                            colAcciones = vm.todasColumnasDefinicion.filter(x => x.field == 'accion')[0]

                            entidad.subGridOptions.columnDefs.pop();
                            entidad.subGridOptions.columnDefs.push(addColFechaCreacion);
                            entidad.subGridOptions.columnDefs.push(colAcciones);
                        }
                    } else {
                        if (col !== "Nombre Flujo" && col !== "Accion Flujo" && vm.columnDef.map(x => x && x.displayName).indexOf(col) == -1) {
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

        function borrarColumnas() {
            let lista = vm.listaEntidades;
            for (var i = 0, len = lista.length; i < len; i++) {
                var entidad = lista[i];

                for (var j = 0, lenCol = vm.columnasDisponiblesPorAgregar.length; j < lenCol; j++) {
                    var indexEliminar = -1;
                    var col = vm.columnasDisponiblesPorAgregar[j];

                    if (col == 'Año de inicio' || col == 'Año Fin') {
                        if (entidad.subGridOptions.columnDefs.map(x => x.displayName).indexOf('Horizonte') > -1) {
                            indexEliminar = entidad.subGridOptions.columnDefs.map(x => x.displayName).indexOf('Horizonte');
                        }
                    } else {
                        indexEliminar = entidad.subGridOptions.columnDefs.map(x => x.displayName).indexOf(col);
                    }

                    if (indexEliminar >= 0) entidad.subGridOptions.columnDefs.splice(indexEliminar, 1);
                }

                vm.listaEntidades[i] = entidad;
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
                if ($localStorage.tipoFiltro.proyectos) {
                    vm.columnas = $localStorage.tipoFiltro.proyectos.columnasActivas;
                    vm.columnasDisponiblesPorAgregar = $localStorage.tipoFiltro.proyectos.columnasDisponibles;
                }
            }

            vm.puedeVerFiltroIdentificador = vm.columnas.indexOf('Identificador') > -1;
            vm.puedeVerFiltroBpin = vm.columnas.indexOf('BPIN') > -1;
            vm.puedeVerFiltroNombre = vm.columnas.indexOf('Nombre') > -1;
            vm.puedeVerFiltroEntidad = vm.columnas.indexOf('Entidad') > -1;
            vm.puedeVerFiltroIdentificadorCR = vm.columnas.indexOf('Identificador CR') > -1;
            vm.puedeVerFiltroAnoInicio = vm.columnas.indexOf('Año de inicio') > -1;
            vm.puedeVerFiltroAnoFin = vm.columnas.indexOf('Año Fin') > -1;
            vm.puedeVerFiltroPrioridad = vm.columnas.indexOf('Prioridad') > -1;
            vm.puedeVerFiltroEstadoProyecto = vm.columnas.indexOf('Estado del Proyecto') > -1;
            vm.puedeVerFiltroSector = vm.columnas.indexOf('Sector') > -1;
            vm.puedeVerFiltroNombreFlujo = vm.columnas.indexOf('Nombre Flujo') > -1;
            vm.puedeVerFiltroAccionFlujo = vm.columnas.indexOf('Accion Flujo') > -1;
            vm.puedeVerFiltroEstadoInstancia = vm.columnas.indexOf('Estado') > -1;
        }

        function buscar(restablecer) {
            var iconoLimpiar = document.getElementById('icoLimpiarBusqueda');
            iconoLimpiar.src = "Img/IcoBusq.svg";
            return cargarEntidades(idTipoProyecto).then(function () {
                mostrarMensajeRespuesta();
                if (restablecer)
                    vm.BusquedaRealizada = false;
                else
                    vm.BusquedaRealizada = true;
            });
        }

        function limpiarCamposFiltro() {
            vm.proyectoFiltro.identificador = null;
            vm.proyectoFiltro.nombre = null;
            vm.proyectoFiltro.sectorId = null;
            vm.proyectoFiltro.entidadId = null;
            vm.proyectoFiltro.identificadorCR = null;
            vm.proyectoFiltro.horizonteInicio = null;
            vm.proyectoFiltro.horizonteFin = null;
            vm.proyectoFiltro.prioridad = null;
            vm.proyectoFiltro.estadoProyecto = null;
            vm.proyectoFiltro.tipoEntidadId = null;
            vm.proyectoFiltro.bpin = null;
            vm.proyectoFiltro.estadoProyectoId = null;
            vm.proyectoFiltro.nombreFlujo = null;
            vm.proyectoFiltro.accionFlujo = null;
            vm.proyectoFiltro.estadoInstanciaId = null;
            vm.proyectoFiltro.procesoId = null;
            vm.proyectoFiltro.codigoProceso = null;
        }

        function listarFiltroSectores() {
            servicioPanelPrincipal.obtenerSectores(vm.peticionObtenerInbox).then(exito, error);

            function exito(respuesta) {
                let listaSectoresGrid = [];
                //vm.listaEntidades.forEach(entidade => {
                //    entidade.subGridOptions.data.forEach(item =>
                //        listaSectoresGrid.push(item.SectorNombre));
                //});
                let listaFiltroSectores = [];
                if (respuesta.data && respuesta.data.length > 0) {
                    listaFiltroSectores = respuesta.data;
                    vm.listaFiltroSectores = listaFiltroSectores; //.filter(item => listaSectoresGrid.includes(item.Name));
                }
            }

            function error() {
                vm.listaFiltroSectores = [];
            }
        }

        function listarFiltroEntidades() {
            servicioPanelPrincipal.obtenerEntidadesVisualizador(usuarioDNP).then(exito, error);

            function exito(respuesta) {
                let listaEntidadesGrid = [];
                //vm.listaEntidades.forEach(entidade => {
                //    entidade.subGridOptions.data.forEach(item =>
                //        listaEntidadesGrid.push(item.NombreEntidad));
                //});
                let listaFiltroEntidades = [];
                if (respuesta.data && respuesta.data.length > 0) {
                    listaFiltroEntidades = respuesta.data;
                }
                vm.listaFiltroEntidades = listaFiltroEntidades;
            }

            function error() {
                vm.listaFiltroEntidades = [];
            }
        }

        function listarFiltroPrioridades() {
            servicioPanelPrincipal.obtenerPrioridades(vm.peticionObtenerInbox).then(exito, error);

            function exito(respuesta) {
                let listaPrioridadesGrid = [];
                vm.listaEntidades.forEach(entidade => {
                    entidade.subGridOptions.data.forEach(item =>
                        listaPrioridadesGrid.push(item.Criticidad));
                });
                let listaFiltroPrioridades = [];
                if (respuesta.data && respuesta.data.length > 0) {
                    listaFiltroPrioridades = respuesta.data;
                }
                vm.listaFiltroPrioridades = listaFiltroPrioridades.filter(item => listaPrioridadesGrid.includes(item.Name));
            }

            function error() {
                vm.listaFiltroPrioridades = [];
            }
        }

        function listarFiltroEstadoProyecto() {
            servicioPanelPrincipal.obtenerEstadoProyectos(vm.peticionObtenerInbox).then(exito, error);

            function exito(respuesta) {
                //let listaEstadoProyectoGrid = [];
                //vm.listaEntidades.forEach(entidade => {
                //    entidade.subGridOptions.data.forEach(item =>
                //        listaEstadoProyectoGrid.push(item.EstadoProyecto));
                //});
                let listaFiltroEstadoProyecto = [];
                if (respuesta.data && respuesta.data.length > 0) {
                    listaFiltroEstadoProyecto = respuesta.data;
                }
                vm.listaFiltroEstadoProyectos = listaFiltroEstadoProyecto;
                vm.proyectoFiltro.estadoProyectoId = 6; //Default En Ejecución
            }

            function error() {
                vm.listaFiltroEstadoProyectos = [];
            }
        }

        function listarFiltroEstadoInstancia() {
            servicioPanelPrincipal.obtenerEstadoInstancia(vm.peticionObtenerInbox).then(exito, error);

            function exito(respuesta) {
                vm.listaFiltroEstadoInstancias = respuesta.data || [];
                vm.proyectoFiltro.estadoInstanciaId = "1"; //Defecto Activo
            }

            function error() {
                vm.listaFiltroEstadoInstancias = [];
            }
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
                        'proyectos': {
                            'columnasActivas': selectedItem.columnasActivas,
                            'columnasDisponibles': selectedItem.columnasDisponibles
                        }
                    };
                } else {
                    $localStorage.tipoFiltro['proyectos'] = {
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

        function obtenerInbox(idObjeto) {

            if (!vm.tipoEntidad) {
                vm.tipoEntidad = constantesAutorizacion.tipoEntidadNacional;
            }
            if (!vm.proyectoFiltro.tipoEntidad) {
                vm.proyectoFiltro.tipoEntidad = constantesAutorizacion.tipoEntidadNacional;
            }
            return cargarEntidades(idObjeto).then(function () {

                let tipoEntidad;
                for (let i = 0; i < vm.listaTipoEntidad.length; i++) {
                    tipoEntidad = vm.listaTipoEntidad[i];

                    if (!tipoEntidad.Deshabilitado) {
                        vm.tipoEntidad = tipoEntidad.Nombre;
                        //cambioTipoEntidad(vm.tipoEntidad);
                        break;
                    }
                }
                listarFiltroSectores();
                listarFiltroEntidades();
                listarFiltroPrioridades();
                listarFiltroEstadoProyecto();
                listarFiltroEstadoInstancia();
                mostrarMensajeRespuesta();
            });
        };

        $scope.$on('AutorizacionConfirmada', function () {
            $timeout(function () {
                if (vm.yaSeCargoInbox === false) {
                    var roles = sesionServicios.obtenerUsuarioIdsRoles();
                    if (roles != null && roles.length > 0) {
                        vm.peticionObtenerInbox = {
                            // ReSharper disable once UndeclaredGlobalVariableUsing
                            IdUsuario: usuarioDNP,
                            IdObjeto: idTipoProyecto,
                            // ReSharper disable once UndeclaredGlobalVariableUsing
                            Aplicacion: nombreAplicacionBackbone,
                            ListaIdsRoles: roles
                        };

                        activar(); //Realizar la carga
                    } else {
                        vm.Mensaje = $filter('language')('ErrorUsuarioSinPermisosAplicacion');
                        mostrarMensajeRespuesta();
                    }
                }
            });
        });

        function activar() {
            vm.yaSeCargoInbox = true;
            // ReSharper disable once UndeclaredGlobalVariableUsing
            obtenerInbox(idTipoProyecto);
            //escucharEventos();
        }

        function buscarColumnasPorColumnasFiltroSeleccionadas() {
            let listaColumnas = [];
            let columna = ''
            for (let i = 0; i < vm.columnas.length; i++) {
                var nombreColumnasSeleccionadaFiltro = vm.columnas[i];

                if (nombreColumnasSeleccionadaFiltro == 'Año de inicio' || nombreColumnasSeleccionadaFiltro == 'Año Fin') {
                    nombreColumnasSeleccionadaFiltro = 'Horizonte';
                }

                if (nombreColumnasSeleccionadaFiltro == 'Nombre Flujo' || nombreColumnasSeleccionadaFiltro == 'Accion Flujo') {
                    nombreColumnasSeleccionadaFiltro = 'Nombre/Accion Flujo';
                }

                let columnasTemp = vm.todasColumnasDefinicion.filter(x => x.displayName == nombreColumnasSeleccionadaFiltro);
                if (columnasTemp && columnasTemp.length > 0)
                    columna = columnasTemp[0].field;
                if (listaColumnas.indexOf(columna) == -1) {
                    listaColumnas.push(columna);
                }
            }

            return listaColumnas;
        }

        function cargarEntidades(idObjeto) {
            const columnasVisibles = buscarColumnasPorColumnasFiltroSeleccionadas();
            var listaopciones = autorizacionServicios.obtenerOpcionesconfiltro(obtenerIdAP($sessionStorage.usuario.roles));

            if (vm.proyectoFiltro.estadoProyectoId == 6 && vm.proyectoFiltro.macroproceso == "Gestión de recursos" && vm.proyectoFiltro.tipoEntidad == "Territorial") {
                vm.proyectoFiltro.estadoProyectoId = null;
            }

            return servicioPanelPrincipal.obtenerInbox(vm.peticionObtenerInbox, vm.proyectoFiltro, columnasVisibles).then(
                function (respuesta) {
                    vm.cantidadDeProyectos = 0;
                    vm.listaEntidades = [];

                    if (respuesta.data.GruposEntidades && respuesta.data.GruposEntidades.length > 0) {
                        const listaGrupoEntidades = respuesta.data.GruposEntidades;
                        listaGrupoEntidades.forEach(grupoEntidade => {
                            grupoEntidade.ListaEntidades.forEach(entidad => {
                                const nombreEntidade = entidad.NombreEntidad;
                                const tipoEntidad = entidad.TipoEntidad;
                                const nombreSector = entidad.ObjetosNegocio[0].AgrupadorEntidad;
                                const idEntidad = entidad.IdEntidad;

                                vm.listaDatos = [];
                                vm.cantidadDeProyectos += entidad.ObjetosNegocio.length;

                                entidad.ObjetosNegocio.forEach(negocio => {
                                    //var nombretmpflujo = listaopciones.$$state.value.find(x => x.IdOpcionDnp === negocio.FlujoId);
                                    //var mostrarbpin = vm.etapa.toUpperCase() === 'PL' && constantesAutorizacion.idEtapaViabilidadRegistro == negocio.Etapa ? false : true;
                                    vm.listaDatos.push({
                                        ProyectoId: negocio.ProyectoId, //mostrarbpin ? negocio.ProyectoId : '',
                                        IdObjetoNegocio: negocio.IdObjetoNegocio,
                                        NombreObjetoNegocio: negocio.NombreObjetoNegocio,
                                        NombreEntidad: negocio.NombreEntidad,
                                        DescripcionCR: negocio.DescripcionCR,
                                        CodigoProceso: negocio.CodigoProceso,
                                        FechaCreacion: negocio.FechaCreacionStr,
                                        Criticidad: negocio.Criticidad,
                                        EstadoProyecto: negocio.EstadoProyecto,
                                        Horizonte: negocio.Horizonte,
                                        SectorNombre: negocio.SectorNombre,
                                        AgrupadorEntidad: negocio.AgrupadorEntidad,
                                        NombreAccion: negocio.NombreAccion,
                                        IdEntidad: negocio.IdEntidad,
                                        IdInstancia: negocio.IdInstancia,
                                        FlujoId: negocio.FlujoId,
                                        NombreFlujo: negocio.NombreFlujo, // nombretmpflujo ? nombretmpflujo.Nombre : negocio.NombreFlujo,
                                        EstadoInstancia: negocio.EstadoInstancia,
                                        FechaPaso: negocio.FechaPasoStr,
                                        CodigoTramite: negocio.CodigoTramite,
                                        InstanciaPadreId: negocio.InstanciaPadreId,
                                        IdNivel: negocio.IdNivel,
                                        CRTypeId: negocio.CRTypeId,
                                        ResourceGroupId: negocio.ResourceGroupId,
                                        permiteEliminar: negocio.aplicaAccion
                                    });
                                });


                                vm.listaEntidades.push({
                                    idEntidad: idEntidad,
                                    sector: nombreSector,
                                    entidad: nombreEntidade,
                                    tipoEntidad: tipoEntidad,
                                    estadoEntidad: "+",
                                    subGridOptions: {
                                        columnDefs: vm.columnDef,
                                        enableVerticalScrollbar: 1,
                                        appScopeProvider: $scope,
                                        paginationPageSizes: [5, 10, 15, 25, 50, 100],
                                        paginationPageSize: 5,
                                        data: vm.listaDatos,
                                        excessRows: vm.listaDatos.length,
                                        rowTemplate: '<div grid="grid" class="ui-grid-draggable-row ui-grid-cell-fixed-height" style="min-height: 37px;max-height:37px;" draggable="true"><div ng-repeat="(colRenderIndex, col) in colContainer.renderedColumns track by col.colDef.name" style="min-height: 37px;max-height:37px;" class="ui-grid-cell" ng-class="{ \'ui-grid-row-header-cell\': col.isRowHeader, \'custom\': true }" ui-grid-cell></div></div>'
                                    }
                                });

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

                    mostrarMensajeRespuesta();
                }
            );
        }

        function mostrarMensajeRespuesta() {
            // ReSharper disable once UndeclaredGlobalVariableUsing
            if (vm.Mensaje) {
                vm.mostrarMensajeProyectos = true;
            } else {
                vm.mostrarMensajeProyectos = false;
            }
        }


        function conmutadorFiltro() {
            limpiarCamposFiltro();
            vm.mostrarFiltro = !vm.mostrarFiltro;
            var idSpanArrow = 'arrow-IdPanelBuscador';
            var arrowCapitulo = document.getElementById(idSpanArrow);
            var arrowClasses = arrowCapitulo.classList;
            for (var i = 0; i < arrowClasses.length; i++) {
                if (arrowClasses[i] == vm.arrowIcoDown2) {
                    document.getElementById(idSpanArrow).src = "Img/IcoBusqDow.svg";
                    document.getElementById(idSpanArrow).classList.add(vm.arrowIcoUp2);
                    document.getElementById(idSpanArrow).classList.remove(vm.arrowIcoDown2);
                    break;
                } else if (arrowClasses[i] == vm.arrowIcoUp2) {
                    document.getElementById(idSpanArrow).src = "Img/IcoBusqUp.svg";
                    document.getElementById(idSpanArrow).classList.add(vm.arrowIcoDown2);
                    document.getElementById(idSpanArrow).classList.remove(vm.arrowIcoUp2);
                    break;
                }
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
            vm.proyectoFiltro.tipoEntidad = vm.tipoEntidad;
            limpiarCamposFiltro();
            buscar().then(function () {
                listarFiltroSectores();
                listarFiltroEntidades();
                listarFiltroPrioridades();
                listarFiltroEstadoProyecto();
                listarFiltroEstadoInstancia();
                vm.BusquedaRealizada = false;
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

        function activarInstancia(proyecto) {
            servicioPanelPrincipal.activarInstancia(vm.peticionObtenerInbox, proyecto.IdInstancia).then(
                function (respuesta) {
                    cargarEntidades();
                    utilidades.mensajeSuccess("Operación realizada con éxito!", false, false, false);
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

                    mostrarMensajeRespuesta();
                }
            );
        }

        function pausarInstancia(proyecto) {

            servicioPanelPrincipal.pausarInstancia(vm.peticionObtenerInbox, proyecto.IdInstancia).then(
                function (respuesta) {
                    cargarEntidades();
                    utilidades.mensajeSuccess("Operación realizada con éxito!", false, false, false);
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

                    mostrarMensajeRespuesta();
                }
            );

        }

        function detenerInstancia(proyecto) {

            utilidades.mensajeWarning("¿Desea eliminar el proceso " + proyecto.NombreFlujo + "/" + proyecto.NombreAccion + " y todos sus detalles asociados al BPIN" + proyecto.IdObjetoNegocio + "?", function funcionContinuar() {

                servicioPanelPrincipal.detenerInstancia(vm.peticionObtenerInbox, proyecto.IdInstancia).then(
                    function (respuesta) {
                        cargarEntidades();
                        utilidades.mensajeSuccess("El proyecto queda disponible para que le puedan asociar otro proceso!", false, false, false);
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

                        mostrarMensajeRespuesta();
                    }
                );

            }, function funcionCancelar() {
                return;
            });

        }

        function cancelarInstanciaMisProcesos(proyecto) {

            utilidades.mensajeWarning("¿Desea eliminar el proceso " + proyecto.NombreFlujo + "/" + proyecto.NombreAccion + " y todos sus detalles asociados al BPIN" + proyecto.IdObjetoNegocio + "?", function funcionContinuar() {

                servicioPanelPrincipal.cancelarInstanciaMisProcesos(vm.peticionObtenerInbox, proyecto.IdInstancia).then(
                    function (respuesta) {
                        cargarEntidades();
                        utilidades.mensajeSuccess("El proyecto queda disponible para que le puedan asociar otro proceso!", false, false, false);
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

                        mostrarMensajeRespuesta();
                    }
                );

            }, function funcionCancelar() {
                return;
            });

        }

        function mostrarLog(row) {
            $uibModal.open({
                animation: $scope.animationsEnabled,
                templateUrl: 'src/app/panelPrincial/modales/logs/logsModal.html',
                controller: 'logsModalController',
                controllerAs: "vm",
                size: 'lg',
                openedClass: "entidad-modal-adherencia",
                resolve: {
                    IdInstancia: () => row.IdInstancia
                },
            });
        }


        //Implementaciones




        // function contarCantidadObjetosNegocio(idObjeto) {

        //     var gruposEntidades = null;

        //     // ReSharper disable once UndeclaredGlobalVariableUsing
        //     if (idObjeto === vm.idTipoProyecto) {
        //         gruposEntidades = vm.gruposEntidadesProyectos;
        //     } else {
        //         gruposEntidades = vm.gruposEntidadesTramites;
        //     }

        //     var cantidadDeObjetosDeNegocio = 0;

        //     // ReSharper disable once UndeclaredGlobalVariableUsing
        //     angular.forEach(gruposEntidades, function (grupoEntidad) {
        //         // ReSharper disable once UndeclaredGlobalVariableUsing
        //         angular.forEach(grupoEntidad.ListaEntidades, function (entidad) {
        //             cantidadDeObjetosDeNegocio += entidad.ObjetosNegocio.length;
        //         });
        //     });

        //     // ReSharper disable once UndeclaredGlobalVariableUsing
        //     if (idObjeto === vm.idTipoProyecto) {
        //         vm.cantidadDeProyectos = cantidadDeObjetosDeNegocio;
        //     } else {
        //         vm.cantidadDeTramites = cantidadDeObjetosDeNegocio;
        //     }
        // }

        // function conmutadorFiltroTramites() {
        //     limpiarCampos();
        //     vm.mostrarFiltroTramites = !vm.mostrarFiltroTramites;
        // }

        // function cambiarEstado(bpin, estado) {
        //     window.location.href = "/ejecutarAccion/" + bpin + "/" + estado;
        // }

        // function mostrarInformacionDePropiedad(propiedad) {
        //     var debeMostrar = false;
        //     // ReSharper disable once UndeclaredGlobalVariableUsing
        //     angular.forEach(vm.columnas, function (columna) {
        //         if (columna === propiedad) {
        //             debeMostrar = true;
        //         }
        //     });
        //     return debeMostrar;
        // }

        // function mostrarModal() {
        //     vm.mostrarCrearEditarLista = true;
        // };

        // function seleccionarTipoObjeto(idObjeto) {

        //     var tipoObjeto = {
        //         Id: idObjeto
        //     }
        //     var roles = sesionServicios.obtenerUsuarioIdsRoles();
        //     if (roles != undefined && roles.length > 0) {
        //         estadoAplicacionServicios.tipoObjetoSeleccionado = tipoObjeto;
        //         obtenerInbox(idObjeto);
        //     }
        //     else {
        //         vm.Mensaje = $filter('language')('ErrorUsuarioSinPermisosAplicacion');
        //         mostrarMensajeRespuesta(idObjeto);
        //     }
        // }

        // function limpiarCampos() {
        //     vm.busquedaNombreConId = "";
        // };

        // function filtrarPorNombreObjetoNegocio(objetoNegocio) {

        //     var nombreConId = objetoNegocio.NombreObjetoNegocio + " - " + objetoNegocio.IdObjetoNegocio;

        //     if (!vm.busquedaNombreConId || (nombreConId.toLowerCase().indexOf(vm.busquedaNombreConId) !== -1) || (nombreConId.toLowerCase().indexOf(vm.busquedaNombreConId.toLowerCase()) !== -1)) {
        //         return true;
        //     }
        //     return false;

        // };

        // function consultarAccion(idInstancia, nombreProyecto, idObjetoNegocio, idEntidad) {
        //     $sessionStorage.idInstancia = $sessionStorage.idInstanciaFlujoPrincipal = idInstancia;
        //     $sessionStorage.nombreProyecto = nombreProyecto;
        //     $sessionStorage.idObjetoNegocio = idObjetoNegocio;
        //     $sessionStorage.idEntidad = idEntidad;
        //     $timeout(function () {
        //         $location.path('/Acciones/ConsultarAcciones');
        //     }, 300);
        // }

        // function escucharEventos() {
        //     $scope.$on(constantesBackbone.eventoInstanciaCreada, cuandoSeCreaUnaInstancia);

        //     ////////////

        //     function cuandoSeCreaUnaInstancia(evento, idTipoObjeto) {
        //         obtenerInbox(idTipoObjeto);
        //     }
        // }

        function configurarNombreEtapa() {
            switch (vm.etapa) {
                case 'pl':
                    vm.nombreEtapa = 'Planeación';
                    break;
                case 'pr':
                    vm.nombreEtapa = 'Programación';
                    break;
                case 'gr':
                    vm.nombreEtapa = 'Gestión de Recursos';
                    break;
                case 'ej':
                    vm.nombreEtapa = 'Ejecución';
                    break;
                case 'se':
                    vm.nombreEtapa = 'Seguimiento';
                    break;
                case 'ev':
                    vm.nombreEtapa = 'Evaluación';
                    break;
            }
        }

        function getIdEtapa() {
            var idEtapa = [];
            switch (vm.etapa) {
                case 'pl':
                    idEtapa = [constantesBackbone.idEtapaPlaneacion, constantesBackbone.idEtapaViabilidadRegistro, constantesBackbone.idEtapaAjustes, constantesBackbone.idEtapaPriorizacion];
                    break;
                case 'pr':
                    idEtapa = [constantesBackbone.idEtapaProgramacion];
                    break;
                case 'gr':
                    idEtapa = [constantesBackbone.idEtapaGestionRecursos, constantesBackbone.idEtapaSolicitudRecursos, constantesBackbone.idEtapaRevisionRequisitos, constantesBackbone.idEtapaAprobacion, constantesBackbone.idEtapaAjustesGR];
                    break;
                case 'ej':
                    idEtapa = [constantesBackbone.idEtapaNuevaEjecucion, constantesBackbone.idEtapaProgramacionEjecucion, constantesBackbone.idEtapaAjustesEjecucion, constantesBackbone.idEtapaTramitesEjecucion, constantesBackbone.idEtapaSeguimientoControl];
                    break;
                case 'se':
                    idEtapa = [];
                    break;
                case 'ev':
                    idEtapa = [constantesBackbone.idEtapaEvaluacion, constantesBackbone.idEtapaCortoPlazo, constantesBackbone.idEtapaMedianoPlazo, constantesBackbone.idEtapaLargoPlazo];
                    break;
            }
            return idEtapa;
        }

        //#region Componente Crear Proceso

        function mostrarFlujosDisponibles() {

            vm.flujosAutorizados = [];

            cargarFlujos().then(
                function (flujosAutorizados) {
                    if (flujosAutorizados) {
                        if (flujosAutorizados.length === 0)
                            utilidades.mensajeError($filter('language')('ErrorUsuarioConRolSinOpciones'));
                        else {
                            for (var i = 0; i < flujosAutorizados.length; i++) {
                                flujosAutorizados[i].NombreOpcionCompleto = flujosAutorizados[i].NombreOpcion;
                                if (flujosAutorizados[i].NombreOpcion.length > 53) {
                                    flujosAutorizados[i].NombreOpcion = flujosAutorizados[i].NombreOpcion.substring(0, 53) + "...";
                                }
                            }
                            vm.flujosAutorizados = flujosAutorizados;

                        }
                    }
                }
            ).then(function () {
                transversalSgrServicio.SGR_Transversal_LeerParametro("FlujosInboxSGR")
                    .then(function (respuestaParametro) {
                        if (respuestaParametro.data) {
                            vm.flujosInboxSGR = respuestaParametro.data.Valor;
                        }
                    }, function (error) {
                        utilidades.mensajeError(error);
                    });
            })

        }

        function cargarFlujosProgramacion() {

            return flujoServicios.obtenerFlujosPorRoles().then(
                function (flujos) {
                    flujos = flujos.filter(filtrarFlujosPorProgramacion);
                    var listaAuxFlujos = [];

                    flujos.forEach(element => {
                        if (element.PadreTramiteId == null) {
                            listaAuxFlujos.push({
                                id: element.IdOpcionDnp,
                                nombre: element.NombreOpcion,
                                alias: element.AliasTipoTramite
                            })
                        }
                    });
                    vm.cantidadFlujosProgramacion = listaAuxFlujos.length;
                    if (vm.cantidadFlujosProgramacion > 0) {
                        vm.classProcesos = "conProgramacion";
                    }
                },
                function (error) {
                    if (error) {
                        if (error.status) {
                            switch (error.status) {
                                case 401:
                                    utilidades.mensajeError($filter('language')('ErrorUsuarioSinPermisoAccion'));
                                    break;
                                case 500:
                                    utilidades.mensajeError($filter('language')('ErrorObtenerDatos'));
                                    break;
                                case 404:
                                    return [];
                            }
                            return;
                        }
                    }
                }
            );
            ////////////
            function filtrarFlujosPorProgramacion(flujo) {
                return flujo.TipoObjeto && flujo.TipoObjeto.Id === idTipoTramiteProgramacion;
            };
        }

        function cargarFlujos() {

            return flujoServicios.obtenerFlujosPorRoles().then(
                function (flujos) {
                    return flujos.filter(filtrarFlujosPorTipoObjetoSeleccionado);
                },
                function (error) {
                    if (error) {
                        if (error.status) {
                            switch (error.status) {
                                case 401:
                                    utilidades.mensajeError($filter('language')('ErrorUsuarioSinPermisoAccion'));
                                    break;
                                case 500:
                                    utilidades.mensajeError($filter('language')('ErrorObtenerDatos'));
                                    break;
                                case 404:
                                    return [];
                            }
                            return;
                        }
                    }
                }
            );

            function filtrarFlujosPorTipoObjetoSeleccionado(flujo) {
                var idopciondnp = '';
                switch (vm.etapa) {
                    case 'pl':
                        idopciondnp = constantesBackbone.idEtapaPlaneacion;
                        break;
                    case 'pr':
                        idopciondnp = constantesBackbone.idEtapaProgramacion;
                        break;
                    case 'gr':
                        idopciondnp = constantesBackbone.idEtapaGestionRecursos;
                        break;
                    case 'ej':
                        idopciondnp = constantesBackbone.idEtapaNuevaEjecucion;
                        break;
                    case 'ev':
                        idopciondnp = constantesBackbone.idEtapaEvaluacion;
                        break;
                }
                return flujo.TipoObjeto && flujo.TipoObjeto.Id === estadoAplicacionServicios.tipoObjetoSeleccionado.Id && flujo.Activo !== false && flujo.IdNivelPadre.toUpperCase() === idopciondnp.toUpperCase();
            };
        }

        function mostrarModal(flujoSeleccionado) {

            autorizacionServicios.obtenerEntidadesPorRoles(flujoSeleccionado.Roles)
                .then(crearModal)
                .then(invocarCrearInstancia)
                .catch(controlarError)

            function crearModal(entidades) {

                if (!entidades || entidades.length === 0)
                    throw new Error($filter('language')('ErrorRolesSinEntidades'));

                var modal;

                var esFlujoSGR = vm.flujosInboxSGR.includes(flujoSeleccionado.IdFlujo.toUpperCase());

                if (flujoSeleccionado.TipoObjeto.Id == idTipoProyecto && !esFlujoSGR) {
                    modal = modalCrearInstanciaProyecto(entidades, flujoSeleccionado.NombreOpcion, flujoSeleccionado.IdOpcionDnp, flujoSeleccionado.IdFlujo, flujoSeleccionado.TipoTramiteId);
                    return modal;
                } else if (flujoSeleccionado.TipoObjeto.Id == idTipoProyecto && esFlujoSGR) {
                    modal = modalCrearInstanciaProyectoSGR(entidades, flujoSeleccionado.NombreOpcion, flujoSeleccionado.IdOpcionDnp, flujoSeleccionado.IdFlujo, flujoSeleccionado.TipoTramiteId);
                    return modal;
                }

            }

            function invocarCrearInstancia(modalDatos) {
                return crearInstancia(flujoSeleccionado, modalDatos);
            }

            function controlarError(error) {

                if (error) {
                    if (error.status) {
                        switch (error.status) {
                            case 401:
                                return utilidades.mensajeError($filter('language')('ErrorUsuarioSinPermisoAccion'));
                            case 404:
                                return;
                            case 500:
                                return utilidades.mensajeError($filter('language')('ErrorObtenerDatos'));
                        }
                    } else {
                        if (error.message)
                            utilidades.mensajeError(error.message);
                        else
                            utilidades.mensajeError(error);
                    }
                }
            }
        };

        function modalCrearInstanciaProyecto(entidades, nombreOpcion, idOpcionDnp, IdFlujo, TipoTramiteId) {

            var idsEntidadesMga = entidades.map(function (entidad) {
                return entidad.IdEntidadMGA;
            });

            $sessionStorage.IdFlujo = IdFlujo;
            $sessionStorage.TipoTramiteId = TipoTramiteId;
            return flujoServicios.obtenerProyectosPorEntidadesyEstados(idsEntidadesMga, idOpcionDnp, null, null, null, null, null, null, IdFlujo, TipoTramiteId).then(mostrarModalProyectos, mostrarModalProyectos);
            entidades, idOpcionDNP, estados, sectorId, entidadId, codigoBpin, proyectoNombre, tieneInstancias, flujoid, tipoTramiteId
            function mostrarModalProyectos(proyectos) {
                if (proyectos === null) {
                    utilidades.mensajeError("No hay proyectos para seleccionar.", false); return false;
                }

                else if (proyectos !== undefined && proyectos !== null) {
                    if (!Array.isArray(proyectos)) {
                        if (proyectos.status !== undefined && proyectos.status === 404) { //No se encontraron resultados                            
                            proyectos = [];
                        } else {
                            return $q.reject(proyectos); //Retornamos la excepcion ocurrida
                        }
                    }
                }

                var modalConfig = {
                    templateUrl: '/src/app/comunes/templates/modales/seleccionarProyectos/seleccionarProyectos.html',
                    controller: 'seleccionarProyectosCtrl',
                    size: 'lg',
                    openedClass: 'modal__crear__tarea',
                    resolve: {
                        proyectos: $q.resolve(proyectos),
                        nombreOpcion: $q.resolve(nombreOpcion),
                        idsEntidadesMga: $q.resolve(idsEntidadesMga),
                        idOpcionDnp: $q.resolve(idOpcionDnp)
                    }
                }

                modalConfig = Object.assign({}, modalConfigDefault, modalConfig);
                var instanciaModal = $uibModal.open(modalConfig);
                return instanciaModal.result;
            }
        }

        function modalCrearInstanciaProyectoSGR(entidades, nombreOpcion, idOpcionDnp, IdFlujo, TipoTramiteId) {

            var idsEntidadesMga = entidades.map(function (entidad) {
                return entidad.IdEntidadMGA;
            });

            $sessionStorage.IdFlujo = IdFlujo;
            $sessionStorage.TipoTramiteId = TipoTramiteId;
            //return flujoServicios.obtenerProyectosSGRApriorizar().then(mostrarModalProyectos, mostrarModalProyectos);
            //entidades, idOpcionDNP, estados, sectorId, entidadId, codigoBpin, proyectoNombre, tieneInstancias, flujoid, tipoTramiteId
            return flujoServicios.obtenerProyectosPorEntidadesyEstadosSGR(idsEntidadesMga, idOpcionDnp, null, null, null, null, null, null, IdFlujo, TipoTramiteId, $sessionStorage.usuario.permisos.IdUsuarioDNP).then(mostrarModalProyectos, mostrarModalProyectos);
            entidades, idOpcionDNP, estados, sectorId, entidadId, codigoBpin, proyectoNombre, tieneInstancias, flujoid, tipoTramiteId
            function mostrarModalProyectos(proyectos) {
                if (proyectos === null) {
                    utilidades.mensajeError("No hay proyectos para seleccionar.", false); return false;
                }

                else if (proyectos !== undefined && proyectos !== null) {
                    if (!Array.isArray(proyectos)) {
                        if (proyectos.status !== undefined && proyectos.status === 404) { //No se encontraron resultados                            
                            proyectos = [];
                        } else {
                            return $q.reject(proyectos); //Retornamos la excepcion ocurrida
                        }
                    }
                }

                var modalConfig = {
                    templateUrl: '/src/app/comunes/templates/modales/seleccionarProyectos/seleccionarProyectosSGR.html',
                    controller: 'seleccionarProyectosCtrlSGR',
                    size: 'lg',
                    openedClass: 'modal__crear__tarea',
                    resolve: {
                        proyectos: $q.resolve(proyectos),
                        nombreOpcion: $q.resolve(nombreOpcion),
                        idsEntidadesMga: $q.resolve(idsEntidadesMga),
                        idOpcionDnp: $q.resolve(idOpcionDnp)
                    }
                }

                modalConfig = Object.assign({}, modalConfigDefault, modalConfig);
                var instanciaModal = $uibModal.open(modalConfig);
                return instanciaModal.result;
            }
        }

        function crearInstancia(flujo, datos) {
            if (datos.EntidadId == 0 || datos.EntidadId == "0") {
                utilidades.mensajeError('No se puede crear la instancia porque el id de la entidad es cero.');
                return;
            }

            var usuarioRolId = sesionServicios.obtenerUsuarioIdsRoles()[0];

            var instanciaDto = {
                FlujoId: vm.flujosInboxSGR ? datos.FlujoId : flujo.IdOpcionDnp,
                ObjetoId: datos.CodigoBpin/* || datos.IdEntidadMGA*/,
                UsuarioId: usuarioDNP,
                RolId: usuarioRolId,
                TipoObjetoId: flujo.TipoObjeto.Id,
                ListaEntidades: [datos.EntidadId/* || datos.IdEntidadMGA*/]
            }

            flujoServicios.crearInstancia(instanciaDto).then(
                function (resultado) {

                    if (!resultado.length || resultado == null) {
                        utilidades.mensajeError('No se creó instancia');
                        return;
                    }

                    var instanciasFallidas = resultado.filter(function (instancia) {
                        return !instancia.Exitoso;
                    });

                    var cantidadInstanciasFallidas = instanciasFallidas.length;

                    if (cantidadInstanciasFallidas) {
                        utilidades.mensajeError('Se crearon ' + (resultado.length - cantidadInstanciasFallidas).toString() + ' instancias de ' + resultado.length.toString());
                    }
                    else {
                        var esFlujoSGR = vm.flujosInboxSGR.includes((vm.flujosInboxSGR ? datos.FlujoId.toUpperCase() : flujo.IdOpcionDnptoUpperCase()).toUpperCase());

                        if (!esFlujoSGR) {
                            if (resultado != null & resultado[0].NumeroTramite != null)
                                utilidades.mensajeSuccess('Se crearon instancias exitosamente.<br/>Proceso No. ' + resultado[0].NumeroTramite, false, cargueInicial, '', 'El proceso fue creado con éxito.');
                            else
                                utilidades.mensajeSuccess('Se crearon instancias exitosamente.<br/>Proceso No. ', false, cargueInicial, '', 'El proceso fue creado con éxito.');
                        }
                        else {
                            var proyectoProceso = {
                                BPIN: datos.CodigoBpin,
                                ProcesoId: datos.ProcesoId,
                                InstanciaId: resultado[0].InstanciaId,
                                FlujoId: datos.FlujoId,
                            }
                            flujoServicios.GuardarProyectoPermisosProcesoSGR(proyectoProceso).then(
                                function (response) {
                                    if (response.data || response.statusText === "OK") {
                                        if (resultado != null & resultado[0].NumeroTramite != null) {
                                            if (flujo.NombreOpcion.toLowerCase().includes('priorización')) {
                                                utilidades.mensajeSuccess('El subproceso de priorización para la metodología seleccionada fue creado con éxito. Código asignado: <b>' + resultado[0].NumeroTramite + '</b>. Para continuar con su diligenciamiento, acceda a él desde "Mis Procesos" o la "Consola de Procesos".', false, cargueInicial, '', 'El proceso fue creado con éxito.');
                                            } else {
                                                utilidades.mensajeSuccess('Código asignado: <b>' + resultado[0].NumeroTramite + '</b><br/><br/>Para continuar con su diligenciamiento, acceda a este desde la opción de menú  "Mis procesos"', false, cargueInicial, '', 'El proceso fue creado con éxito.');
                                            }
                                            

                                        }
                                    } else {
                                        swal('', "Error al realizar asignar los permisos", 'error');
                                    }
                                }
                            );
                        }
                    }
                },
                function (error) {
                    if (error) {
                        utilidades.mensajeError(error);
                    }
                }
            );
        }

        //#endregion

    }

    function obtenerIdAP(roles) {
        var rta = roles.map(function (item, index) { return index === 0 ? item.IdAplicacion : "" });
        return rta[0];
    }

    angular.module('backbone')
        .component('inbox', {
            templateUrl: "/src/app/panelPrincial/componentes/inbox/inbox.template.html",
            controller: 'inboxController',
            controllerAs: 'vm'
        });

})();