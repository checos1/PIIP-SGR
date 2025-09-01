(function () {
    'use strict';
    angular.module('backbone').controller('proyectoController', proyectoController);

    proyectoController.$inject = [
        '$scope',
        'servicioConsolaProyectos',
        'servicioEntidades',
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
        'Blob'
    ];

    function proyectoController(
        $scope,
        servicioConsolaProyectos,
        servicioEntidades,
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
        Blob
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
        vm.consultarAccion = consultarAccion;
        vm.abrirModalInstanciasProyecto = abrirModalInstanciasProyecto;
        vm.abrirModalDocumentosAdjuntos = abrirModalDocumentosAdjuntos;
        vm.abrirModalFichas = abrirModalFichas;
        vm.restablecerBusqueda = restablecerBusqueda;
        vm.listarFiltroEntidadesProyecto = listarFiltroEntidadesProyecto;
        
        //Filtro
        vm.arrowIcoDown2 = "glyphicon-chevron-down-busqDNP";
        vm.arrowIcoUp2 = "glyphicon-chevron-up-busqDNP";
        vm.BusquedaRealizada = false;
        //variables
        vm.tipoFiltro = constantesTipoFiltro.proyecto;
        vm.cantidadDeProyectos = 0;
        vm.mostrarFiltro = false;
        vm.busquedaNombreConId = "";
        vm.Mensaje = "";
        vm.columnas = servicioConsolaProyectos.columnasPorDefectoProyecto;
        vm.columnasDisponiblesPorAgregar = servicioConsolaProyectos.columnasDisponiblesProyecto;
        vm.gruposEntidadesProyectos = false;
        vm.mostrarMensajeProyectos = false;
        vm.idTipoProyecto = "";
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
        vm.listaFiltroVigenciasProyectos = [];

        //Plantillas para personalizar el contenido de las celdas
        vm.proyectoFilaTemplate = 'src/app/consola/proyectos/componentes/proyectos/plantillas/plantillaProyecto.html';
        vm.sectorEntidadFilaTemplate = 'src/app/consola/proyectos/componentes/proyectos/plantillas/plantillaSectorEntidad.html';
        vm.accionesFilaProyectoTemplate = 'src/app/consola/proyectos/componentes/proyectos/plantillas/plantillaAccionesProyecto.html';
        vm.consultarAccionFlujoTemplate = 'src/app/consola/proyectos/componentes/proyectos/plantillas/plantillaConsultarAccionFlujo.html';
        vm.proyectoBPINTemplate = 'src/app/consola/proyectos/componentes/proyectos/plantillas/plantillaProyectoBPIN.html';

        vm.puedeVerFiltroIdentificador;
        vm.puedeVerFiltroBpin;
        vm.puedeVerFiltroNombre;
        vm.puedeVerFiltroEntidad;
        vm.puedeVerFiltroAnoInicio;
        vm.puedeVerFiltroAnoFin;
        vm.puedeVerFiltroEstadoProyecto;
        vm.puedeVerFiltroSector;
        vm.puedeVerFiltroNombreFlujo;
        vm.puedeVerFiltroAccionFlujo;

        vm.peticionObtenerInbox;

        vm.proyectoFiltro = {
            identificador: null,
            nombre: null,
            sectorId: null,
            entidadId: null,
            horizonteInicio: null,
            horizonteFin: null,
            estadoProyecto: null,
            tipoEntidadId: null,
            bpin: null,
            estadoProyectoId: null,
            tipoEntidad: null,
            nombreFlujo: null,
            accionFlujo: null,
            vigenciaProyectoId: null,
            marca: null
        };

        vm.columnDefPrincial = [{
            field: 'entidad',
            displayName: 'Entidad',
            enableHiding: false,
            width: '96%',
            cellTemplate: vm.sectorEntidadFilaTemplate,
        }];

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

        vm.columnDef = [
            {
                field: 'IdObjetoNegocio',
                displayName: 'BPIN',
                enableHiding: false,
                width: '10%',
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
                field: 'EstadoProyecto',
                displayName: 'Estado Proyecto',
                enableHiding: false,
                width: '10%',
                cellTooltip: (row, col) => row.entity[col.field]
            },
            {
                field: 'NombreObjetoNegocio',
                displayName: 'Proyecto',
                enableHiding: false,
                width: '20%',
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
                field: 'SectorNombre',
                displayName: 'Sector',
                enableHiding: false,
                width: '20%',
                cellTooltip: (row, col) => row.entity[col.field]
            },
            {
                field: 'EstadoProyecto',
                displayName: 'Estado del Proyecto',
                enableHiding: false,
                width: '14%',
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
                width: '190'
            }
        ];

        vm.todasColumnasDefinicion = Object.assign([], vm.columnDef);
        vm.listaDatos = [];
        vm.listaEntidades = [];


        // grid main
        vm.gridOptions;

        function restablecerBusqueda() {
            vm.limpiarCamposFiltro();
            //vm.buscar(true);
            vm.cantidadDeProyectos = null;
            vm.listaEntidades = [];
            var iconoLimpiar = document.getElementById('icoLimpiarBusqueda');
            iconoLimpiar.src = "Img/IcolimpiarBusqD.svg";
        }

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

        function listarFiltroEntidadesProyecto() {
            if (vm.tipoEntidad === 'Nacional') {
                servicioConsolaProyectos.ObtenerEntidadesPorSector(vm.proyectoFiltro.sectorId, vm.tipoEntidad).then(exito, error);

                function exito(respuesta) {
                    vm.ListaEntidadesProyectos = respuesta.data || [];
                }

                function error() {
                    vm.ListaEntidadesProyectos = [];
                }
            }
        }

        function ObtenerMarcas() {

            servicioConsolaProyectos.ObtenerMarcas().then(exito, error);

            function exito(respuesta) {
                vm.ListaMarcas = respuesta.data || [];
            }

            function error() {
                vm.ListaMarcas = [];
            }
        }
        

        this.$onInit = function () {
            vm.idTipoProyecto = idTipoProyecto;

            // buscar columnas en el localstorage
            buscarColumnasLocalStorage();

            if (!vm.gridOptions) {
                vm.gridOptions = {
                    expandableRowTemplate: vm.proyectoFilaTemplate,
                    expandableRowScope: {
                        subGridVariable: 'subGridScopeVariable'
                    },
                    expandableRowHeight: 420,
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
                        IdUsuario: usuarioDNP,
                        IdObjeto: idTipoProyecto,
                        Aplicacion: nombreAplicacionBackbone,
                        ListaIdsRoles: roles
                    };

                    activar(); //Realizar la carga
                }
            }
        };

        function agregarColumnas() {
            let lista = vm.listaEntidades;
            var colAcciones;
            var addColBPIN;
            var addColSector;
            var addColEstado;
            var addColProyecto;
            var addColEntidad;

            for (var i = 0, len = lista.length; i < len; i++) {
                var entidad = lista[i];

                for (var j = 0, lenCol = vm.columnas.length; j < lenCol; j++) {
                    var col = vm.columnas[j];

                    if (col == 'Sector') {
                        if (vm.columnDef.map(x => x.displayName).indexOf('Sector') == -1) {
                            addColSector = vm.todasColumnasDefinicion.filter(x => x.displayName == 'Sector')[0];
                            colAcciones = vm.todasColumnasDefinicion.filter(x => x.field == "accion")[0];

                            entidad.subGridOptions.columnDefs.pop();
                            entidad.subGridOptions.columnDefs.push(addColSector);
                            entidad.subGridOptions.columnDefs.push(colAcciones);
                        }
                    }

                    if (col == 'Estado del Proyecto') {
                        if (vm.columnDef.map(x => x.displayName).indexOf('Estado del Proyecto') == -1) {
                            addColEstado = vm.todasColumnasDefinicion.filter(x => x.displayName == 'Estado del Proyecto')[0];
                            colAcciones = vm.todasColumnasDefinicion.filter(x => x.field == "accion")[0];

                            entidad.subGridOptions.columnDefs.pop();
                            entidad.subGridOptions.columnDefs.push(addColEstado);
                            entidad.subGridOptions.columnDefs.push(colAcciones);
                        }
                    }

                    if (col == 'Entidad') {
                        if (vm.columnDef.map(x => x.displayName).indexOf('Entidad') == -1) {
                            addColEntidad = vm.todasColumnasDefinicion.filter(x => x.displayName == 'Entidad')[0];
                            colAcciones = vm.todasColumnasDefinicion.filter(x => x.field == "accion")[0];

                            entidad.subGridOptions.columnDefs.pop();
                            entidad.subGridOptions.columnDefs.push(addColEntidad);
                            entidad.subGridOptions.columnDefs.push(colAcciones);
                        }
                    }

                    if (col == 'Proyecto') {
                        if (vm.columnDef.map(x => x.displayName).indexOf('Proyecto') == -1) {
                            addColProyecto = vm.todasColumnasDefinicion.filter(x => x.displayName == 'Proyecto')[0];
                            colAcciones = vm.todasColumnasDefinicion.filter(x => x.field == "accion")[0];

                            entidad.subGridOptions.columnDefs.pop();
                            entidad.subGridOptions.columnDefs.push(addColProyecto);
                            entidad.subGridOptions.columnDefs.push(colAcciones);
                        }
                    }

                    if (col == 'BPIN') {
                        if (vm.columnDef.map(x => x.displayName).indexOf('BPIN') == -1) {
                            addColBPIN = vm.todasColumnasDefinicion.filter(x => x.displayName == 'BPIN')[0];
                            colAcciones = vm.todasColumnasDefinicion.filter(x => x.field == "accion")[0];

                            entidad.subGridOptions.columnDefs.pop();
                            entidad.subGridOptions.columnDefs.push(addColBPIN);
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

                    indexEliminar = entidad.subGridOptions.columnDefs.map(x => x.displayName).indexOf(col);

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
            //if ($localStorage.tipoFiltro) {
            //    if ($localStorage.tipoFiltro.consola_proyectos) {
            //        vm.columnas = $localStorage.tipoFiltro.consola_proyectos.columnasActivas;
            //        vm.columnasDisponiblesPorAgregar = $localStorage.tipoFiltro.consola_proyectos.columnasDisponibles;
            //    }
            //}

            vm.puedeVerFiltroIdentificador = vm.columnas.indexOf('Identificador') > -1;
            vm.puedeVerFiltroBpin = vm.columnas.indexOf('BPIN') > -1;
            vm.puedeVerFiltroNombre = vm.columnas.indexOf('Proyecto') > -1;
            vm.puedeVerFiltroEntidad = vm.columnas.indexOf('Entidad') > -1;
            vm.puedeVerFiltroAnoInicio = vm.columnas.indexOf('Año de inicio') > -1;
            vm.puedeVerFiltroAnoFin = vm.columnas.indexOf('Año Fin') > -1;
            vm.puedeVerFiltroEstadoProyecto = vm.columnas.indexOf('Estado del Proyecto') > -1;
            vm.puedeVerFiltroSector = vm.columnas.indexOf('Sector') > -1;
            vm.puedeVerFiltroNombreFlujo = vm.columnas.indexOf('Nombre Flujo') > -1;
            vm.puedeVerFiltroAccionFlujo = vm.columnas.indexOf('Accion Flujo') > -1;
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
            vm.proyectoFiltro.vigenciaProyectoId = null;
        }

        function listarFiltroSectores() {
            servicioConsolaProyectos.obtenerSectores(vm.peticionObtenerInbox).then(exito, error);

            function exito(respuesta) {
                //let listaSectoresGrid = [];
                //vm.listaEntidades.forEach(entidade => {
                //    entidade.subGridOptions.data.forEach(item =>
                //        listaSectoresGrid.push(item.SectorNombre));
                //});
                //let listaFiltroSectores = [];
                if (respuesta.data && respuesta.data.length > 0) {
                    var listaFiltroSectores = respuesta.data;
                    vm.listaFiltroSectores = listaFiltroSectores;//.filter(item => listaSectoresGrid.includes(item.Name));
                }
            }

            function error() {
                vm.listaFiltroSectores = [];
            }
        }

        function listarFiltroEntidades() {
            servicioConsolaProyectos.obtenerEntidades(vm.peticionObtenerInbox).then(exito, error);

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

        function listarFiltroEstadoProyecto() {
            servicioConsolaProyectos.obtenerEstadoProyectos(vm.peticionObtenerInbox).then(exito, error);

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
            }

            function error() {
                vm.listaFiltroEstadoProyectos = [];
            }
        }

        function listaFiltroHorizontesProyecto() {
            servicioConsolaProyectos.obtenerVigenciasProyectos(vm.peticionObtenerInbox).then(exito, error);

            function exito(respuesta) {
                if (respuesta.data && respuesta.data.length > 0) {
                    vm.listaFiltroVigenciasProyectos = respuesta.data;
                }
            }

            function error() {
                vm.listaFiltroVigenciasProyectos = [];
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
                        'consola_proyectos': {
                            'columnasActivas': selectedItem.columnasActivas,
                            'columnasDisponibles': selectedItem.columnasDisponibles
                        }
                    };
                } else {
                    $localStorage.tipoFiltro['consola_proyectos'] = {
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
                        break;
                    }
                }
                listarFiltroSectores();
                listarFiltroEntidades();
                listarFiltroEstadoProyecto();
                mostrarMensajeRespuesta();
                listaFiltroHorizontesProyecto();
            });
        };

        $scope.$on('AutorizacionConfirmada', function () {
            $timeout(function () {
                if (vm.yaSeCargoInbox === false) {
                    var roles = sesionServicios.obtenerUsuarioIdsRoles();
                    if (roles != null && roles.length > 0) {
                        vm.peticionObtenerInbox = {
                            IdUsuario: usuarioDNP,
                            IdObjeto: idTipoProyecto,
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
            if (!vm.tipoEntidad) {
                vm.tipoEntidad = constantesAutorizacion.tipoEntidadNacional;
            }
            if (!vm.proyectoFiltro.tipoEntidad) {
                vm.proyectoFiltro.tipoEntidad = constantesAutorizacion.tipoEntidadNacional;
            }
            listarFiltroSectores();
            listarFiltroEntidades();
            listarFiltroEstadoProyecto();
            mostrarMensajeRespuesta();
            listaFiltroHorizontesProyecto();
            ObtenerMarcas();
            if (vm.tipoEntidad !== 'Nacional') {
                listarEntidadesProyecto();
            }

            //obtenerInbox(idTipoProyecto);
        }


        function listarEntidadesProyecto() {
            servicioConsolaProyectos.ObtenerEntidadesPorSector(46, vm.tipoEntidad).then(exito, error);

            function exito(respuesta) {
                vm.ListaEntidadesProyectos = respuesta.data || [];
            }

            function error() {
                vm.ListaEntidadesProyectos = [];
            }
        }


        function buscarColumnasPorColumnasFiltroSeleccionadas() {
            let listaColumnas = [];
            let columna = ''
            for (let i = 0; i < vm.columnas.length; i++) {
                var nombreColumnasSeleccionadaFiltro = vm.columnas[i];

                //if (nombreColumnasSeleccionadaFiltro == 'Nombre' || nombreColumnasSeleccionadaFiltro == 'BPIN') {
                //    nombreColumnasSeleccionadaFiltro = 'Proyecto/BPIN';
                //}

                //if (nombreColumnasSeleccionadaFiltro == 'Año de inicio' || nombreColumnasSeleccionadaFiltro == 'Año Fin') {
                //    nombreColumnasSeleccionadaFiltro = 'Horizonte';
                //}

                //if (nombreColumnasSeleccionadaFiltro == 'Nombre Flujo' || nombreColumnasSeleccionadaFiltro == 'Accion Flujo') {
                //    nombreColumnasSeleccionadaFiltro = 'Nombre/Accion Flujo';
                //}

                let columnaTemp = vm.todasColumnasDefinicion.filter(x => x.displayName.toLowerCase() == nombreColumnasSeleccionadaFiltro.toLowerCase());
                if (columnaTemp.length > 0) {
                    columna = columnaTemp[0].field;
                    if (listaColumnas.indexOf(columna) == -1) {
                        listaColumnas.push(columna);
                    }
                }
            }

            return listaColumnas;
        }

        function cargarEntidades(idObjeto) {

            const columnasVisibles = buscarColumnasPorColumnasFiltroSeleccionadas();

            return servicioConsolaProyectos.obtenerInbox(vm.peticionObtenerInbox, vm.proyectoFiltro, columnasVisibles).then(
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
                                        IdObjetoNegocio: negocio.IdObjetoNegocio,
                                        NombreObjetoNegocio: negocio.NombreObjetoNegocio,
                                        NombreEntidad: negocio.NombreEntidad,
                                        FechaCreacion: negocio.FechaCreacion,
                                        EstadoProyecto: negocio.EstadoProyecto,
                                        Horizonte: negocio.Horizonte,
                                        SectorNombre: negocio.SectorNombre,
                                        AgrupadorEntidad: negocio.AgrupadorEntidad,
                                        NombreAccion: negocio.NombreAccion,
                                        horizonteInicio: negocio.HorizonteInicio,
                                        horizonteFin: negocio.HorizonteFin,
                                        IdEntidad: negocio.IdEntidad,
                                        IdInstancia: negocio.IdInstancia,
                                        NombreFlujo: negocio.NombreFlujo,
                                        EstadoInstancia: negocio.EstadoInstancia,
                                        FechaPaso: negocio.FechaPaso
                                    });
                                });

                                for (var i = 0; i < 20; i++) {
                                    vm.listaDatos.concat(vm.listaDatos);
                                }

                                vm.listaEntidades.push({
                                    sector: nombreSector,
                                    entidad: nombreEntidade,
                                    tipoEntidad: tipoEntidad,
                                    idEntidad: idEntidad,
                                    estadoEntidad: "+",
                                    subGridOptions: {
                                        columnDefs: vm.columnDef,
                                        enableVerticalScrollbar: 1,
                                        appScopeProvider: $scope,
                                        paginationPageSizes: [5, 10, 15, 25, 50, 100],
                                        paginationPageSize: 5,

                                        data: vm.listaDatos,
                                        // rowTemplate: '<div grid="grid" class="ui-grid-draggable-row ui-grid-cell-fixed-height" draggable="true"><div ng-repeat="(colRenderIndex, col) in colContainer.renderedColumns track by col.colDef.name" class="ui-grid-cell" ng-class="{ \'ui-grid-row-header-cell\': col.isRowHeader, \'custom\': true }" ui-grid-cell></div></div>'
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
                Nombre: constantesAutorizacion.tipoEntidadPrivadas,
                Descripcion: constantesAutorizacion.tipoEntidadPrivadas,
                Deshabilitado: !tipoEntidadPresenteEnLaConfiguracion(constantesAutorizacion.tipoEntidadPrivadas)
            },
            {
                Nombre: constantesAutorizacion.tipoEntidadPublicas,
                Descripcion: "públicas",
                Deshabilitado: !tipoEntidadPresenteEnLaConfiguracion(constantesAutorizacion.tipoEntidadPublicas)
            }
            ];
        }

        function cambioTipoEntidad(tipoEntidad) {
            vm.tipoEntidad = tipoEntidad;
            vm.proyectoFiltro.tipoEntidad = vm.tipoEntidad;
            limpiarCamposFiltro();
            vm.cantidadDeProyectos = null;
            activar();
            vm.listaEntidades = [];
            //buscar().then(function () {
            //    listarFiltroSectores();
            //    listarFiltroEntidades();
            //    listarFiltroEstadoProyecto();
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

        /**
         * 
         * @description Obtiene las entidades desde Autorizacion para Negocio.
         * @param {string} tipoEntidadGeneral . Tipo de entidad actual en la pestaña.
         */
        vm.obtenerEntidadesNegocio = function (tipoEntidadGeneral) {

            try {

                if (typeof tipoEntidadGeneral !== 'string')
                    throw { exception: 'Parámetro proporcionado no válido' };

                if (tipoEntidadGeneral === '')
                    throw { exception: 'El parámetro no debe ser nulo o vacío' };

                // TODO: Hay que mejorar esto
                return servicioEntidades.obtenerEntidadesPorTipo(tipoEntidadGeneral);
            }
            catch (exception) {
                console.log('controladorProyecto.ObtenerEntidadesNegocio: ', exception);
                toastr.error('Ocurrió un error al obtener el historial del proyecto');
            }
        };

        /**
         * @description . Obtiene los datos generales de la entidad actual
         * @param {UUID} guidEntidad . Identificador único de la entidad actual
         */
        vm.obtenerDatosGeneralesEntidad = function (guidEntidad) {
            try {

                return servicioEntidades.obtenerEntidadPorEntidadId(guidEntidad);
            }
            catch (exception) {
                console.log('controladorProyecto.obtenerDatosGeneralesEntidad: ', exception);
                toastr.error('Ocurrió un error al obtener el historial del proyecto');
            }
        };

        /**
        * 
        * @description Muestra la ventana modal de la historial de cambio de proyecto entre entidades
        * @param {Object} proyecto. Elemento actual para verificar su historial
        * @param {Array} historial. Lista del historial de cambios del proyecto actual
       */
        vm.mostrarHistorialProyectoEntidad = function (proyecto, historial) {
            try {

                if (historial === undefined || historial === null)
                    throw { message: 'historial no puede ser nulo.' }

                if (proyecto === undefined || proyecto === null)
                    throw { message: 'proyecto no puede ser nulo' }

                if (Array.isArray(historial) === false)
                    throw { message: 'historial no contiene la estructura correcta.' }

                // mostrar ventana modal. Enviar el proyecto seleccionado
                let modal = $uibModal.open({
                    animation: true,
                    templateUrl: 'src/app/consola/proyectos/modales/intercambioProyectoEntidadTemplate.html',
                    controller: 'modalIntercambioController',
                    controllerAs: "modalIntercambioCtrl",
                    scope: $scope /*scope actual del controlador*/,
                    size: 'lg',

                    /*parámetros*/
                    resolve: {
                        data: () => ({

                            proyecto: angular.copy(proyecto),
                            proyectoEntidadHistorial: angular.copy(historial)
                        })
                    }
                });

                modal.result.then(this.modalProyectoEntidad_onResultThen, () => { });
            }
            catch (exception) {
                console.log('controladorProyecto.mostrarHistoriaProyectoEntidad: ', exception);
                toastr.error('Ocurrió un error al obtener el historial del proyecto');
            }
        };

        /**
        * 
        * @description Provocado al presionar el botón de accion en la subgrid
        * @param {Event} $event . Evento provocado por componente HTML origen
        * @param {Object} sender . Componente HTML origen
        */
        vm.btnActualizaProyecto_onClick = function ($event, sender) {
            try {


                // acceder al scope del botón
                const btnScope = angular.element(sender).scope();
                const entity = (btnScope !== undefined && btnScope !== null) ? /*Acceder al scope de la fila y obtener el elemento enlazado*/btnScope.$parent.$parent.row.entity : null;

                servicioConsolaProyectos.obtenerHistorialProyecto(entity.ProyectoId, vm.peticionObtenerInbox.IdUsuario).then(response => {

                    var dataResponse = response.data;

                    if (Boolean(dataResponse.EsExcepcion)) {
                        console.log('obtenerHistorialProyecto: ', String(dataResponse.MensajeExcepcion));
                        toastr.error('Error al cargar el historial de cambios');
                    }
                    else {
                        const historial = dataResponse.Datos;
                        this.mostrarHistorialProyectoEntidad(entity, historial);
                    }
                });
            }
            catch (exception) {
                console.log('controladorProyecto.btnActualizaProyecto_onClick => ', exception);
                toastr.error('Ocurrió un error al mostrar la ventana modal');
            }
        };

        /**
        * 
        * @description Evento provocado al responder la petición de la ventana modal
        * @param {Object} data .objeto - registro para insertar
        */
        vm.modalProyectoEntidad_onResultThen = function modalProyectoEntidad_onResultThen(data) {
            try {

                // aqui va la petición de guardado
                let nuevoRegistro = angular.copy(data);
                servicioConsolaProyectos.InsertarAuditoriaProyecto(nuevoRegistro, { IdPIIP: idUsuarioPIIP, NombreCuenta: usuarioDNP }).then(response => {

                    var dataResponse = response.data;
                    if (Boolean(dataResponse.EsExcepcion)) {
                        console.log('InsertarAuditoriaProyecto: ', dataResponse.MensajeExcepcion);
                        toastr.error('Error al guardar los cambios'); // TODO: cambiar este por un mensaje de erro bonito

                        return;
                    }
                    else {
                        if (Number(dataResponse.Datos) > 0)
                            // actualizar lista de proyectos
                            vm.cargarEntidades(null);
                        else
                            toastr.warning('Los cambios no fueron guardados correctamente');// TODO: cambiar este por un mensaje de erro bonito
                    }
                });

            }
            catch (exception) {
                console.log('controladorProyecto.modal_onResultThen => ', exception);
                toastr.error('Ocurriò un error al realizar ésta operación');
            }
        };

        /**
        * 
        * @description. Evento provocado al presionar el botón Exportar PDF.
        * @param {Event} $event. Evento provocado
        * @param {object} sender. Componente HTML que provocó el evento.
        */
        vm.aExportarPDF_onClick = function ($event, sender) {
            try {

                const columnasVisibles = buscarColumnasPorColumnasFiltroSeleccionadas();
                servicioConsolaProyectos.obtenerInbox(vm.peticionObtenerInbox, vm.proyectoFiltro, columnasVisibles).then(response => {

                    let dataResponse = response.data;
                    dataResponse.ColumnasVisibles = vm.columnas;
                    dataResponse.GruposEntidades = dataResponse.GruposEntidades.map(p => { p.TipoEntidad = vm.tipoEntidad; return p; })

                    vm.DescargarPDF(dataResponse);
                });
            }
            catch (exception) {
                console.log('controladorProyecto.aExportarPDF_onClick => ', exception);
                toastr.error('Ocurrió un error al ejecutar ésta acción');
            }
        };

        /**
         * 
         * @description. Evento provocado al presionar el botón Exportar EXCEL.
         * @param {Event} $event. Evento provocado
         * @param {object} sender. Componente HTML que provocó el evento.
         */
        vm.aExportarExcel_onClick = function ($event, sender) {
            try {

                const columnasVisibles = buscarColumnasPorColumnasFiltroSeleccionadas();
                servicioConsolaProyectos.obtenerInbox(vm.peticionObtenerInbox, vm.proyectoFiltro, columnasVisibles).then(response => {

                    let dataResponse = response.data;
                    dataResponse.ColumnasVisibles = vm.columnas;
                    dataResponse.GruposEntidades = dataResponse.GruposEntidades.map(p => { p.TipoEntidad = vm.tipoEntidad; return p; })

                    vm.DescargarExcel(dataResponse);
                });
            }
            catch (exception) {
                console.log('controladorProyecto.aExportarExcel_onClick => ', exception);
                toastr.error('Ocurrió un error al ejecutar ésta acción');
            }
        };

        /**
         * 
         * @desciption .Obtiene los datos actuales y los descarga creando un archivo PDF de la información
         * @param {Array} datos. Arreglo de la información a mostrar en el PDF de descarga.
        */
        vm.DescargarPDF = function (datos) {
            try {

                if (datos !== undefined && datos !== null) {

                    servicioConsolaProyectos.imprimirPDFConsolaProyectos(datos).then(responsePDF => {
                        let dataResponsePDF = responsePDF.data;

                        if (Boolean(dataResponsePDF.EsExcepcion) === true) {
                            console.log('controladorProyecto.DescargarPDF => ', String(dataResponsePDF.ExcepcionMensaje));
                            toastr.error('Ocurrió un error al descargar PDF');
                        }
                        else {
                            let file = dataResponsePDF.Datos;

                            var bytes = new Uint8Array(file.FileContents);
                            var blob = new Blob([bytes], { type: file.ContentType });

                            FileSaver.saveAs(blob, file.FileDownloadName);
                        }
                    });
                }
            }
            catch (execption) {
                console.log('controladorProyecto.DescargarPDF => ', exception);
                toastr.error('Ocurrió un error al descargar PDF');
            }
        };

        /**
         * 
         * @description . Obtiene el archivo binario del excel generado a partir de los datos proporcionados
         * @param {Object} datos. Instancia de la clase Dominio.Dto.Proyecto.ProyectoDto 
         */
        vm.DescargarExcel = function (datos) {
            try {

                if (datos !== undefined && datos !== null) {

                    servicioConsolaProyectos.obtenerExcel(datos).then(response => {
                        let dataResponse = response.data;

                        if (Boolean(dataResponse.EsExcepcion) === true) {
                            console.log('controladorProyecto.DescargarExcel => ', String(dataResponse.ExcepcionMensaje));
                            toastr.error('Ocurrió un error al descargar Excel');
                        }
                        else {
                            let file = dataResponse.Datos;

                            var bytes = Uint8Array.from(atob(String(file.FileContent)).split('').map(char => char.charCodeAt(0)));
                            var blob = new Blob([bytes], { type: String(file.ContentType) });

                            FileSaver.saveAs(blob, String(file.FileName));
                        }
                    });
                }
            }
            catch (execption) {
                console.log('controladorProyecto.DescargarExcel => ', exception);
                toastr.error('Ocurrió un error al descargar Excel');
            }
        }


        function abrirModalInstanciasProyecto(bpin = null) {

            let objProyecto = {
                bpin: bpin,
                tipoEntidad: vm.tipoEntidad
            }


            var modalInstance = $uibModal.open({
                animation: $scope.animationsEnabled,
                templateUrl: 'src/app/consola/proyectos/componentes/proyectos/modales/modalInstanciasProyecto.html',
                controller: 'modalInstanciasProyectoController',
                controllerAs: "vm",
                openedClass: "consola-modal-instancias",
                size: 'lg',
                resolve: {
                    objProyecto: objProyecto
                }
            });

            modalInstance.result.then(function (selectedItem) {


            }, function () {

            });



        }

        function abrirModalDocumentosAdjuntos(row) {

            var modalInstance = $uibModal.open({
                animation: $scope.animationsEnabled,
                templateUrl: 'src/app/consola/proyectos/componentes/proyectos/modales/modalDocumentosProyectos.html',
                controller: 'modalDocumentosProyectosController',
                controllerAs: "vm",
                openedClass: "consola-modal-soportesDNP",
                //size: 'lg',
                resolve: {
                    objProyecto: row
                }
            });

            modalInstance.result.then(function (selectedItem) {


            }, function () {

            });
        }

        function abrirModalFichas(row) {
            servicioConsolaProyectos.obtenerIdAplicacion(row.IdObjetoNegocio).then(res => {
                $sessionStorage.idAplicacion = res.data;
                $uibModal.open({
                    animation: $scope.animationsEnabled,
                    templateUrl: 'src/app/consola/modales/fichas/modalFichas.html',
                    controller: 'modalFichasController',
                    openedClass: "dialog-modal-archivo",
                    controllerAs: "vm",
                    resolve: {
                        entity: () => row,
                        esProyecto: () => true
                    }
                });
            }, err => {
                    toastr.error("Ocurrió un error al consultar el idAplicacion");
            })
        }
    }


    angular.module('backbone')
        .component('proyecto', {
            templateUrl: 'src/app/consola/proyectos/componentes/proyectos/proyecto.template.html',
            controller: 'proyectoController',
            controllerAs: 'vm'
        });
})();