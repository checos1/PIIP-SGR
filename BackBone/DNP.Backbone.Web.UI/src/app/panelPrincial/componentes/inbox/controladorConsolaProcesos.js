(function () {
    'use strict';
    angular.module('backbone').controller('consolaprocesosController', consolaprocesosController);

    consolaprocesosController.$inject = [
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
        'uiGridConstants',
        'FileSaver',
        'Blob',
        '$routeParams'
    ];

    function consolaprocesosController(
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
        uiGridConstants,
        FileSaver,
        Blob,
        $routeParams) {
        var vm = this;
        //Métodos
        vm.downloadPdf = downloadPdf;
        vm.downloadExcel = downloadExcel;
        vm.obtenerProcesos = obtenerProcesos;
        vm.conmutadorFiltro = conmutadorFiltro;
        vm.obtenerInbox = obtenerInbox;
        vm.abrirModalAdicionarColumnas = abrirModalAdicionarColumnas;
        vm.cargarEntidades = cargarEntidades;
        vm.mostrarMensajeRespuesta = mostrarMensajeRespuesta;
        vm.cambioTipoEntidad = cambioTipoEntidad;
        vm.buscar = buscar;
        vm.limpiarCamposFiltro = limpiarCamposFiltro;
        vm.borrarFila = borrarFila;
        // vm.obtenerExcel = '/home/ObtenerExcel?tipoProyecto=IdTipoProyecto';
        vm.buscar = buscar;
        vm.consultarAccion = consultarAccion;
        vm.mostrarLog = mostrarLog;
        vm.restablecerBusqueda = restablecerBusqueda;
        vm.listarFiltroEntidadesProyecto = listarFiltroEntidadesProyecto;
        vm.listarFiltroAcciones = listarFiltroAcciones;

        //Filtro

        vm.arrowIcoDown2 = "glyphicon-chevron-down-busqDNP";
        vm.arrowIcoUp2 = "glyphicon-chevron-up-busqDNP";
        vm.BusquedaRealizada = false;

        //variables
        vm.tipoFiltro = constantesTipoFiltro.proyecto;
        vm.cantidadDeProyectos = 0;
        vm.cantidadDeTramites = 0;
        vm.mostrarFiltro = true;
        vm.mostrarFiltroTramites = false;
        vm.busquedaNombreConId = "";
        vm.Mensaje = "";
        vm.columnas = servicioPanelPrincipal.columnasPorDefectoProyectoConsolaProcesos;
        vm.columnasDisponiblesPorAgregar = servicioPanelPrincipal.columnasDisponiblesProyectoConsolaProcesos;
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
        vm.listaMacroprocesos = [];
        vm.listaProcesos = [];

        vm.obtenerExcel = '/home/ObtenerExcel?tipoProyecto=IdTipoProyecto';
        vm.proyectoFilaTemplate = 'src/app/panelPrincial/componentes/inbox/plantillas/plantillaProyecto.html';
        vm.sectorEntidadFilaTemplate = 'src/app/panelPrincial/componentes/inbox/plantillas/plantillaSectorEntidad.html';
        vm.accionesFilaProyectoTemplate = 'src/app/panelPrincial/componentes/inbox/plantillas/plantillaAccionesProyectoConsolaProcesos.html';
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

        vm.abrirLogInstancias = abrirLogInstancias;
        

        vm.etapa = $routeParams['etapa'];
        vm.tipoProceso = $routeParams['tipoproceso'];

        vm.cantidadDeProyectos = 0;
        //Acordeon
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

        function restablecerBusqueda() {
            vm.limpiarCamposFiltro();
            //vm.buscar(true);
            activar();
            vm.cantidadDeProyectos = 0;
            vm.listaEntidades = [];
            var iconoLimpiar = document.getElementById('icoLimpiarBusqueda');
            iconoLimpiar.src = "Img/IcolimpiarBusqD.svg";
        }
        function downloadExcel() {
            var peticionObtenerInbox = {
                IdUsuario: usuarioDNP,
                IdObjeto: idTipoProyecto,
                Aplicacion: nombreAplicacionBackbone,
                ListaIdsRoles: sesionServicios.obtenerUsuarioIdsRoles()
            };
            servicioPanelPrincipal.obtenerExcelProyetosConsolaProcesos(peticionObtenerInbox, vm.proyectoFiltro, buscarColumnasPorColumnasFiltroSeleccionadas()).then(function (retorno) {
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

        function obtenerProcesos() {
            if ($localStorage.procesosList != undefined)
                vm.listaProcesos = $localStorage.procesosList.filter(x => String(x.NivelPadreId) == vm.proyectoFiltro.macroprocesoId);
        }

        function downloadPdf() {
            var columnasSeleccionadas = buscarColumnasPorColumnasFiltroSeleccionadas();
            servicioPanelPrincipal.obtenerPdfInboxProyectosConsolaProcesos(vm.peticionObtenerInbox, vm.proyectoFiltro, columnasSeleccionadas).then(
                function (data) {
                    servicioPanelPrincipal.imprimirPdfConsolaProyectos(data.data).then(function (retorno) {
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
            macroprocesoId: null,
            procesoId: null,
            entidadProyectoId: null,
            vigencia: null,
            flujoId: null,
            accionId: null
        };

        vm.columnDefPrincial = [{
            field: 'entidad',
            displayName: 'Entidad',
            enableHiding: false,
            width: '96%',
            cellTemplate: vm.sectorEntidadFilaTemplate,
        }];

        vm.columnDef = [{
            field: 'ProyectoId',
            displayName: 'Identificador',
            enableHiding: false,
            width: '10%%'
        },
        {
            field: 'IdObjetoNegocio',
            displayName: 'BPIN',
            enableHiding: false,
            width: '10%'
        },
        {
            field: 'NombreObjetoNegocio',
            displayName: 'Nombre',
            enableHiding: false,
            width: '25%'
        },
        {
            field: 'NombreEntidad',
            displayName: 'Entidad',
            enableHiding: false,
            width: '25%'
        },
        {
            field: 'DescripcionCR',
            displayName: 'Identificador CR',
            enableHiding: false,
            width: '20%'
        },
        {
            field: 'Criticidad',
            displayName: 'Prioridad',
            enableHiding: false,
            enableColumnMenu: false,
            width: '8%',
            cellTemplate: vm.criticidadTemplate,
        },
        {
            field: 'EstadoProyecto',
            displayName: 'Estado del Proyecto',
            enableHiding: false,
            width: '10%'
        },

        {
            field: 'EstadoInstancia',
            displayName: 'Estado',
            enableHiding: false,
            width: '10%'
        },
        {
            field: 'Horizonte',
            displayName: 'Horizonte',
            enableHiding: false,
            width: '10%'
        },
        {
            field: 'SectorNombre',
            displayName: 'Sector',
            enableHiding: false,
            width: '20%'
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
            width: '200'
        }, {
            field: 'Macroproceso',
            displayName: 'Macroproceso',
            enableHiding: false,
            width: '20%'
        },
        {
            field: 'NombreFlujo',
            displayName: 'Nombre Flujo',
            enableHiding: false,
            width: '20%'
        }, {
            field: 'CodigoProceso',
            displayName: 'Codigo Proceso',
            enableHiding: false,
            width: '20%'
        }, {
            field: 'FechaCreacion',
            displayName: 'FechaCreacion',
            enableHiding: false,
            width: '20%'
        }, {
            field: 'NombreAccion',
            displayName: 'Accion Flujo',
            enableHiding: false,
            enableColumnMenu: false,
            width: '20%',
            cellTemplate: vm.consultarAccionFlujoTemplate,
        }, {
            field: 'FechaPaso',
            displayName: 'FechaPaso',
            enableHiding: false,
            width: '20%'
        }
        ];

        vm.todasColumnasDefinicion = Object.assign([], vm.columnDef);
        vm.listaDatos = [];
        vm.listaEntidades = [];


        // grid main
        vm.gridOptions;

        function consultarAccion(idInstancia, nombreProyecto, idObjetoNegocio, idEntidad, flujo, row) {
            $sessionStorage.idInstancia = $sessionStorage.idInstanciaFlujoPrincipal = idInstancia;
            $sessionStorage.nombreProyecto = nombreProyecto;
            $sessionStorage.idObjetoNegocio = idObjetoNegocio;
            $sessionStorage.idEntidad = idEntidad;
            if (row != null) {
                $sessionStorage.InstanciaSeleccionada = row;
                $sessionStorage.proyectoId = row.ProyectoId;
                $sessionStorage.flujoSeleccionado = row.FlujoId;
                //$sessionStorage.etapa = vm.etapa;
                $sessionStorage.etapa = getIdEtapa(row.IdNivel.toUpperCase());
            }
            if (flujo != null) {
                $sessionStorage.nombreFlujo = flujo;
            }
            $timeout(function () {
                $location.path('/Acciones/ConsultarAcciones');
            }, 300);
        }
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

        this.$onInit = function () {
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

            if (vm.yaSeCargoInbox === false) {

                var roles = sesionServicios.obtenerUsuarioIdsRoles();
                vm.peticionObtenerInbox = {
                    // ReSharper disable once UndeclaredGlobalVariableUsing
                    IdUsuario: usuarioDNP,
                    IdObjeto: idTipoProyecto,
                    // ReSharper disable once UndeclaredGlobalVariableUsing
                    Aplicacion: nombreAplicacionBackbone,
                    ListaIdsRoles: roles
                };
                activar();
                //if (backboneServicios.estaAutorizado() && roles != null && roles.length > 0) {


                //    activar(); //Realizar la carga
                //}
                //else {
                //    vm.Mensaje = $filter('language')('ErrorUsuarioSinPermisosAplicacion');
                //    mostrarMensajeRespuesta(idTipoProyecto);
                //}
            }

            configurarNombreEtapa();
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
            iconoLimpiar.src = "Img/IcolimpiarBusq.svg";
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
            vm.proyectoFiltro.macroprocesoId = null;
            vm.proyectoFiltro.procesoId = null;
            vm.proyectoFiltro.codigoProceso = null;
            vm.proyectoFiltro.accionId = null;
            vm.proyectoFiltro.entidadProyectoId = null;
            vm.proyectoFiltro.flujoId = null;
            vm.proyectoFiltro.vigencia = null;
            vm.BusquedaRealizada = false;
            vm.ListaEntidadesProyectos = [];
            vm.listaFiltroAccion = [];
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
                vm.listaMacroprocesos = $localStorage.macroProcesosList;
                vm.limpiarCamposFiltro();
            });
        };

        //$scope.$on('AutorizacionConfirmada', function () {
        //    $timeout(function () {
        //        if (vm.yaSeCargoInbox === false) {
        //            var roles = sesionServicios.obtenerUsuarioIdsRoles();
        //            if (roles != null && roles.length > 0) {
        //                vm.peticionObtenerInbox = {
        //                    // ReSharper disable once UndeclaredGlobalVariableUsing
        //                    IdUsuario: usuarioDNP,
        //                    IdObjeto: idTipoProyecto,
        //                    // ReSharper disable once UndeclaredGlobalVariableUsing
        //                    Aplicacion: nombreAplicacionBackbone,
        //                    ListaIdsRoles: roles
        //                };

        //                activar(); //Realizar la carga
        //            } else {
        //                vm.Mensaje = $filter('language')('ErrorUsuarioSinPermisosAplicacion');
        //                mostrarMensajeRespuesta();
        //            }
        //        }
        //    });
        //});

        function activar() {
            vm.yaSeCargoInbox = true;
            // ReSharper disable once UndeclaredGlobalVariableUsing
            //obtenerInbox(idTipoProyecto);
            if (!vm.tipoEntidad) {
                vm.tipoEntidad = constantesAutorizacion.tipoEntidadNacional;
            }
            if (!vm.proyectoFiltro.tipoEntidad) {
                vm.proyectoFiltro.tipoEntidad = constantesAutorizacion.tipoEntidadNacional;
            }
            listarFiltroSectores();
            listarFiltroEntidades();
            listarFiltroEstadoProyecto();
            listarFiltroEstadoInstancia();
            listarFiltroFlujos();
            listarFiltroVigencias();

            mostrarMensajeRespuesta();
            vm.listaMacroprocesos = $localStorage.macroProcesosList;

            if (vm.tipoEntidad !== 'Nacional') {
                listarEntidadesProyecto();
            }

            //escucharEventos();
        }

        function listarFiltroEntidadesProyecto() {
            if (vm.tipoEntidad === 'Nacional') {
                servicioPanelPrincipal.ObtenerEntidadesPorSector(vm.proyectoFiltro.sectorId, vm.tipoEntidad).then(exito, error);

                function exito(respuesta) {
                    vm.ListaEntidadesProyectos = respuesta.data || [];
                }

                function error() {
                    vm.ListaEntidadesProyectos = [];
                }
            }
        }

        function listarEntidadesProyecto() {
            servicioPanelPrincipal.ObtenerEntidadesPorSector(46, vm.tipoEntidad).then(exito, error);

            function exito(respuesta) {
                vm.ListaEntidadesProyectos = respuesta.data || [];
            }

            function error() {
                vm.ListaEntidadesProyectos = [];
            }
        }


        function listarFiltroAcciones() {

            servicioPanelPrincipal.ObtenerAccionesFlujoPorFlujoId(vm.proyectoFiltro.flujoId).then(exito, error);

            function exito(respuesta) {
                vm.listaFiltroAccion = respuesta.data || [];
            }

            function error() {
                vm.listaFiltroAccion = [];
            }
        }

        function listarFiltroFlujos() {
            servicioPanelPrincipal.ObtenerFlujosPorTipoObjeto(vm.peticionObtenerInbox.IdObjeto).then(exito, error);

            function exito(respuesta) {
                vm.listaFiltroFlujos = respuesta.data || [];
            }

            function error() {
                vm.listaFiltroFlujos = [];
            }
        }
        function listarFiltroVigencias() {
            servicioPanelPrincipal.ObtenerVigencias(vm.peticionObtenerInbox.IdObjeto).then(exito, error);

            function exito(respuesta) {
                vm.listaFiltroVigencias = respuesta.data || [];
                vm.proyectoFiltro.vigencia = 2023;
            }

            function error() {
                vm.listaFiltroVigencias = [];
            }
        }

        function buscarColumnasPorColumnasFiltroSeleccionadas() {
            let listaColumnas = [];
            let columna = ''
            for (let i = 0; i < vm.columnas.length; i++) {
                var nombreColumnasSeleccionadaFiltro = vm.columnas[i];

                if (nombreColumnasSeleccionadaFiltro == 'Año de inicio' || nombreColumnasSeleccionadaFiltro == 'Año Fin') {
                    nombreColumnasSeleccionadaFiltro = 'Horizonte';
                }

                //if (nombreColumnasSeleccionadaFiltro == 'Nombre Flujo' || nombreColumnasSeleccionadaFiltro == 'Accion Flujo') {
                //    nombreColumnasSeleccionadaFiltro = 'Nombre/Accion Flujo';
                //}

                var resultado = vm.todasColumnasDefinicion.filter(x => x.displayName == nombreColumnasSeleccionadaFiltro);
                if (resultado != null && resultado.length > 0) {
                    columna = resultado[0].field;
                    if (listaColumnas.indexOf(columna) == -1) {
                        listaColumnas.push(columna);
                    }
                }

            }

            return listaColumnas;
        }

        function cargarEntidades(idObjeto) {

            const columnasVisibles = buscarColumnasPorColumnasFiltroSeleccionadas();

            return servicioPanelPrincipal.obtenerInboxConsolaProcesos(vm.peticionObtenerInbox, vm.proyectoFiltro, columnasVisibles).then(
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

                                vm.cantidadDeProyectos += entidad.ObjetosNegocio.length;
                                vm.listaDatos = [];
                                entidad.ObjetosNegocio.forEach(negocio => {
                                    vm.listaDatos.push({
                                        ProyectoId: negocio.ProyectoId,
                                        CodigoProceso: negocio.CodigoProceso,
                                        IdObjetoNegocio: negocio.IdObjetoNegocio,
                                        NombreObjetoNegocio: negocio.NombreObjetoNegocio,
                                        NombreEntidad: negocio.NombreEntidad,
                                        NombreEntidadDestino: negocio.NombreEntidadDestino,
                                        DescripcionCR: negocio.DescripcionCR,
                                        FechaCreacion: negocio.FechaCreacion,
                                        FechaFin: negocio.FechaFin,
                                        Criticidad: negocio.Criticidad,
                                        EstadoProyecto: negocio.EstadoProyecto,
                                        Horizonte: negocio.Horizonte,
                                        SectorNombre: negocio.SectorNombre,
                                        AgrupadorEntidad: negocio.AgrupadorEntidad,
                                        FlujoId: negocio.FlujoId,
                                        NombreAccion: negocio.NombreAccion,
                                        IdEntidad: negocio.IdEntidad,
                                        IdInstancia: negocio.IdInstancia,
                                        NombreFlujo: negocio.NombreFlujo,
                                        EstadoInstancia: negocio.EstadoInstancia,
                                        IdNivel: negocio.IdNivel,
                                        FechaPaso: negocio.FechaPaso,
                                        FechaFinPaso: negocio.FechaFinPaso,
                                        CodigoTramite: negocio.CodigoTramite,
                                        Macroproceso: negocio.Macroproceso,
                                        InstanciaPadreId: negocio.InstanciaPadreId
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
                                        appScopeProvider: $scope,
                                        data: vm.listaDatos,
                                        excessRows: vm.listaDatos.length,
                                        rowTemplate: '<div grid="grid" class="ui-grid-draggable-row ui-grid-cell-fixed-height" style="min-height:37px;max-height:37px;" draggable="true"><div ng-repeat="(colRenderIndex, col) in colContainer.renderedColumns track by col.colDef.name" class="ui-grid-cell" ng-class="{ \'ui-grid-row-header-cell\': col.isRowHeader, \'custom\': true }" ui-grid-cell></div></div>'
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
                if (arrowClasses[i] === vm.arrowIcoDown2) {
                    document.getElementById(idSpanArrow).src = "Img/IcoBusqDow.svg";
                    document.getElementById(idSpanArrow).classList.add(vm.arrowIcoUp2);
                    document.getElementById(idSpanArrow).classList.remove(vm.arrowIcoDown2);
                    break;
                } else if (arrowClasses[i] === vm.arrowIcoUp2) {
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
            activar();
            vm.cantidadDeProyectos = 0;
            vm.listaEntidades = [];
            vm.BusquedaRealizada = false;
            //buscar().then(function () {
            //    listarFiltroSectores();
            //    listarFiltroEntidades();
            //    listarFiltroPrioridades();
            //    listarFiltroEstadoProyecto();
            //    listarFiltroEstadoInstancia();
            //    vm.listaMacroprocesos = $localStorage.macroProcesosList;
            //});
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

        function getIdEtapa(nivelId) {

            var idEtapapl = [constantesBackbone.idEtapaPlaneacion, constantesBackbone.idEtapaViabilidadRegistro, constantesBackbone.idEtapaAjustes, constantesBackbone.idEtapaPriorizacion];
            var idEtapapr = [constantesBackbone.idEtapaProgramacion];
            var idEtapagr = [constantesBackbone.idEtapaGestionRecursos, constantesBackbone.idEtapaSolicitudRecursos, constantesBackbone.idEtapaRevisionRequisitos, constantesBackbone.idEtapaAprobacion, constantesBackbone.idEtapaAjustesGR];
            var idEtapaej = [constantesBackbone.idEtapaNuevaEjecucion, constantesBackbone.idEtapaProgramacionEjecucion, constantesBackbone.idEtapaAjustesEjecucion, constantesBackbone.idEtapaTramitesEjecucion, constantesBackbone.idEtapaSeguimientoControl];
            var idEtapaev = [constantesBackbone.idEtapaEvaluacion, constantesBackbone.idEtapaCortoPlazo, constantesBackbone.idEtapaMedianoPlazo, constantesBackbone.idEtapaLargoPlazo];
            let indexpl = idEtapapl.find(element => element == nivelId);
            if (indexpl != undefined) {
                return 'pl';
            }
            let indexpr = idEtapapr.find(element => element == nivelId);
            if (indexpr != undefined) {
                return 'pr';
            }
            let indexgr = idEtapagr.find(element => element == nivelId);
            if (indexgr != undefined) {
                return 'gr';
            }
            let indexej = idEtapaej.find(element => element == nivelId);
            if (indexej != undefined) {
                return 'ej';
            }
            let indexev = idEtapaev.find(element => element == nivelId);
            if (indexev != undefined) {
                return 'ev';
            }
        }

        function configurarNombreEtapa() {
            switch (vm.etapa) {
                case 'pl':
                    vm.nombreEtapa = 'Planeación';
                    break;
                case 'pr':
                    vm.nombreEtapa = 'Programación';
                    break;
                case 'ej':
                    vm.nombreEtapa = 'Ejecución';
                    break;
                case 'se':
                    vm.nombreEtapa = 'Seguimiento';
                    break;
                case 'ev':
                    vm.nombreEtapa = 'Evolución';
                    break;
            }

        }
    }

    angular.module('backbone')
        .component('consolaprocesos', {
            templateUrl: "/src/app/panelPrincial/componentes/inbox/consolaprocesos.template.html",
            controller: 'consolaprocesosController',
            controllerAs: 'vm'
        });

})();