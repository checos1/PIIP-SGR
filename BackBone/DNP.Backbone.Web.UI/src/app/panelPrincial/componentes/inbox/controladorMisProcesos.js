(function () {
    'use strict';
    angular.module('backbone').controller('misprocesosController', misprocesosController);

    misprocesosController.$inject = [
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

    function misprocesosController(
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

        vm.peticionObtenerInbox;

        vm.etapa = $routeParams['etapa'];
        vm.tipoProceso = $routeParams['tipoproceso'];;
        console.log($routeParams);

        function downloadExcel() {
            var peticionObtenerInbox = {
                IdUsuario: usuarioDNP,
                IdObjeto: idTipoProyecto,
                Aplicacion: nombreAplicacionBackbone,
                ListaIdsRoles: sesionServicios.obtenerUsuarioIdsRoles()
            };
            servicioPanelPrincipal.obtenerExcelProyetos(peticionObtenerInbox, vm.proyectoFiltro, buscarColumnasPorColumnasFiltroSeleccionadas()).then(function (retorno) {
                var blob = new Blob([retorno.data], {
                    type: "application/octet-stream"
                });
                // FileSaver.saveAs(blob,nombreDelArchivo(retorno));
                FileSaver.saveAs(blob, "Proyectos.xls");
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
            servicioPanelPrincipal.obtenerPdfInboxProyectos(peticionObtenerInbox, vm.proyectoFiltro, buscarColumnasPorColumnasFiltroSeleccionadas()).then(
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
                displayName: 'Estado do Proyecto',
                enableHiding: false,                
                width: '10%'
            },
            {
                field: 'accionFlujo',
                displayName: 'Nombre/Accion Flujo',
                enableHiding: false,
                enableColumnMenu: false,                
                width: '20%',
                cellTemplate: vm.consultarAccionFlujoTemplate,
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
            }
        ];

        vm.todasColumnasDefinicion = Object.assign([], vm.columnDef);
        vm.listaDatos = [];
        vm.listaEntidades = [];


        // grid main
        vm.gridOptions;

        function consultarAccion(idInstancia, nombreProyecto, idObjetoNegocio, idEntidad) {

            $sessionStorage.idInstancia = $sessionStorage.idInstanciaFlujoPrincipal = idInstancia;
            $sessionStorage.nombreProyecto = nombreProyecto;
            $sessionStorage.idObjetoNegocio = idObjetoNegocio;
            $sessionStorage.idEntidad = idEntidad;
            $timeout(function () {
                $location.path('/Acciones/ConsultarAcciones');
            }, 300);
        }

        function onRegisterApi(gridApi) {
            $scope.gridApi = gridApi;
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
                };

                vm.gridOptions.columnDefs = vm.columnDefPrincial;
                vm.gridOptions.data = vm.listaEntidades;
            }

            if (vm.yaSeCargoInbox === false) {
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
            vm.puedeVerFiltroEstadoProyecto = vm.columnas.indexOf('Estado do Proyecto') > -1;
            vm.puedeVerFiltroSector = vm.columnas.indexOf('Sector') > -1;
            vm.puedeVerFiltroNombreFlujo = vm.columnas.indexOf('Nombre Flujo') > -1;
            vm.puedeVerFiltroAccionFlujo = vm.columnas.indexOf('Accion Flujo') > -1;
        }

        function buscar() {
            return cargarEntidades(idTipoProyecto).then(function () {
                mostrarMensajeRespuesta();
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
        }

        function listarFiltroSectores() {
            servicioPanelPrincipal.obtenerSectores(vm.peticionObtenerInbox).then(exito, error);

            function exito(respuesta) {
                let listaSectoresGrid = [];
                vm.listaEntidades.forEach(entidade => {
                    entidade.subGridOptions.data.forEach(item =>
                        listaSectoresGrid.push(item.SectorNombre));
                });
                let listaFiltroSectores = [];
                if (respuesta.data && respuesta.data.length > 0) {
                    listaFiltroSectores = respuesta.data;
                    vm.listaFiltroSectores = listaFiltroSectores.filter(item => listaSectoresGrid.includes(item.Name));
                }
            }

            function error() {
                vm.listaFiltroSectores = [];
            }
        }

        function listarFiltroEntidades() {
            servicioPanelPrincipal.obtenerEntidades(vm.peticionObtenerInbox).then(exito, error);

            function exito(respuesta) {
                let listaEntidadesGrid = [];
                vm.listaEntidades.forEach(entidade => {
                    entidade.subGridOptions.data.forEach(item =>
                        listaEntidadesGrid.push(item.NombreEntidad));
                });
                let listaFiltroEntidades = [];
                if (respuesta.data && respuesta.data.length > 0) {
                    listaFiltroEntidades = respuesta.data;
                }
                vm.listaFiltroEntidades = listaFiltroEntidades.filter(item => listaEntidadesGrid.includes(item.Name));
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

                columna = vm.todasColumnasDefinicion.filter(x => x.displayName == nombreColumnasSeleccionadaFiltro)[0].field;
                if (listaColumnas.indexOf(columna) == -1) {
                    listaColumnas.push(columna);
                }
            }

            return listaColumnas;
        }

        function cargarEntidades(idObjeto) {

            const columnasVisibles = buscarColumnasPorColumnasFiltroSeleccionadas();
            
            return servicioPanelPrincipal.obtenerInbox(vm.peticionObtenerInbox, vm.proyectoFiltro, columnasVisibles).then(
                function (respuesta) {

                    vm.listaEntidades = [];                    

                    if (respuesta.data.GruposEntidades && respuesta.data.GruposEntidades.length > 0) {
                        const listaGrupoEntidades = respuesta.data.GruposEntidades;
                        listaGrupoEntidades.forEach(grupoEntidade => {
                            grupoEntidade.ListaEntidades.forEach(entidad => {
                                const nombreEntidade = entidad.NombreEntidad;
                                const tipoEntidad = entidad.TipoEntidad;
                                const nombreSector = entidad.ObjetosNegocio[0].SectorNombre;

                                vm.listaDatos = [];
                                entidad.ObjetosNegocio.forEach(negocio => {
                                    vm.listaDatos.push({
                                        ProyectoId: negocio.ProyectoId,
                                        IdObjetoNegocio: negocio.IdObjetoNegocio,
                                        NombreObjetoNegocio: negocio.NombreObjetoNegocio,
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
                                        NombreFlujo: negocio.NombreFlujo
                                    });   
                                });


                                vm.listaEntidades.push({
                                    sector: nombreSector,
                                    entidad: nombreEntidade,
                                    tipoEntidad: tipoEntidad,
                                    subGridOptions: {
                                        columnDefs: vm.columnDef,
                                        enableVerticalScrollbar: 1,
                                        appScopeProvider: $scope,
                                        paginationPageSizes: [5, 10, 15, 25, 50, 100],
                                        paginationPageSize: 5,
                                        data: vm.listaDatos,
                                        rowTemplate: '<div grid="grid" class="ui-grid-draggable-row ui-grid-cell-fixed-height" draggable="true"><div ng-repeat="(colRenderIndex, col) in colContainer.renderedColumns track by col.colDef.name" class="ui-grid-cell" ng-class="{ \'ui-grid-row-header-cell\': col.isRowHeader, \'custom\': true }" ui-grid-cell></div></div>'
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
        .component('misprocesos', {
            templateUrl: "/src/app/panelPrincial/componentes/inbox/misprocesos.template.html",
            controller: 'misprocesosController',
            controllerAs: 'vm'
        });

})();