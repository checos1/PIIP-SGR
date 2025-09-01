(function () {
    'use strict';
    angular.module('backbone').controller('consolaprocesostramitesController', consolaprocesostramitesController);

    consolaprocesostramitesController.$inject = [
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
        'constantesCondicionFiltro',
        '$routeParams',
        'flujoServicios',
        'utilidades',
        'autorizacionServicios'
    ];

    function consolaprocesostramitesController(
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
        constantesCondicionFiltro,
        $routeParams,
        flujoServicios,
        utilidades,
        autorizacionServicios) {

        var vm = this;

        //Métodos
        vm.conmutadorFiltro = conmutadorFiltro;
        // vm.conmutadorFiltroTramites = conmutadorFiltroTramites;
        // vm.filtrarPorNombreObjetoNegocio = filtrarPorNombreObjetoNegocio;
        // vm.mostrarInformacionDePropiedad = mostrarInformacionDePropiedad;
        // vm.cambiarEstado = cambiarEstado;
        vm.obtenerInbox = obtenerInbox;
        //vm.mostrarModal = mostrarModal;
        vm.abrirModalAdicionarColumnas = abrirModalAdicionarColumnas;
        vm.cargarEntidades = cargarEntidades;
        //vm.contarCantidadObjetosNegocio = contarCantidadObjetosNegocio;
        vm.mostrarMensajeRespuesta = mostrarMensajeRespuesta;
        //vm.consultarAccion = consultarAccion;
        //vm.seleccionarTipoObjeto = seleccionarTipoObjeto;
        vm.cambioTipoEntidad = cambioTipoEntidad;
        vm.buscar = buscar;
        vm.limpiarCamposFiltro = limpiarCamposFiltro;
        vm.consultarAccion = consultarAccion;
        vm.mostrarLog = mostrarLog;
        vm.alcanceTramite = alcanceTramite;
        vm.detenerTramite = detenerTramite;
        vm.restablecerBusqueda = restablecerBusqueda;
        vm.obtenerExcel = '/home/ObtenerExcel?tipoProyecto=IdTipoTramite';
        vm.listarFiltroEntidadesProyecto = listarFiltroEntidadesProyecto;
        vm.listarFiltroAcciones = listarFiltroAcciones;

        //Filtro
        vm.arrowIcoDown2 = "glyphicon-chevron-down-busqDNP";
        vm.arrowIcoUp2 = "glyphicon-chevron-up-busqDNP";
        vm.BusquedaRealizada = false;

        //variables
        vm.tipoFiltro = constantesTipoFiltro.tramites;
        vm.cantidadDeProyectos = 0;
        vm.cantidadDeTramites = 0;
        vm.mostrarFiltro = false;
        vm.mostrarFiltroTramites = false;
        vm.busquedaNombreConId = "";
        vm.Mensaje = "";
        vm.columnas = servicioPanelPrincipal.columnasPorDefectoTramites;
        vm.columnasDisponiblesPorAgregar = servicioPanelPrincipal.columnasDisponiblesTramites;
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

        vm.abrirLogInstancias = abrirLogInstancias;

        vm.listaTramitesCompleta = [];
        vm.listaEntidadesGeneral = [];
        vm.cantidadDeTramites = 0;
        vm.AbrilNivel = function (idEntidad) {
            vm.listaEntidadesGeneral.forEach(function (value, index) {
                if (value.entidadId == idEntidad) {
                    if (value.estadoEntidad == '+')
                        value.estadoEntidad = '-';
                    else
                        value.estadoEntidad = '+';
                }
            });
        }

        vm.plantillaTramites = 'src/app/panelPrincial/componentes/inbox/plantillas/plantillaTramites.html';
        vm.plantillaFilaTramites = 'src/app/panelPrincial/componentes/inbox/plantillas/plantillaFilaTramites.html';
        vm.sectorEntidadFilaTemplate = 'src/app/panelPrincial/componentes/inbox/plantillas/plantillaSectorEntidad.html';
        vm.accionesFilaTramitesTemplate = 'src/app/panelPrincial/componentes/inbox/plantillas/plantillaAccionesTramiteConsolaProcesos.html';
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
        vm.columnasReporte = ['numeroTramite', 'nombreFlujo', 'tipoTramite', 'sector', 'entidad', 'estadoTramite', 'fecha', 'nombreAccion', 'fechaPaso'];
        vm.etapa = $routeParams['etapa'];
        vm.tipoProceso = 'tramites';
        
        var roles = sesionServicios.obtenerUsuarioIdsRoles();
        vm.presupuestoPreliminar = false;

        console.log('roles');
        console.log(roles);

        let habilitado = roles.filter(function (e) {
            return e == '0c0cca1b-8fba-4371-8bf6-4ee2fe6afbaf';
        });

        var roles = sesionServicios.obtenerUsuarioIdsRoles();
        var rol = roles.find(x => x === constantesBackbone.idRPresupuesto.toLowerCase());

        vm.opcionCrearAlcance = false;
        if (habilitado.length > 0)
            vm.opcionCrearAlcance = true;

        vm.peticionObtenerInbox = {
            // ReSharper disable once UndeclaredGlobalVariableUsing
            IdUsuario: usuarioDNP,
            IdObjeto: idTipoTramite,
            // ReSharper disable once UndeclaredGlobalVariableUsing
            Aplicacion: nombreAplicacionBackbone,
            ListaIdsRoles: roles,
            IdsEtapas: getIdEtapa()
        };

        //#region Componente Crear Tramites

        vm.isOpen = false;
        vm.tramites = [];
        vm.cargarFlujosTramites = cargarFlujosTramites;
        vm.listaEntidades = [];
        vm.listarFiltroEntidades = listarFiltroEntidades;
        vm.guardarSolicitud = guardarSolicitud;
        vm.cerrarTramite = cerrarTramite;

        vm.popOverOptionsCrearTramite = {
            isOpen: false,
            templateUrl: 'myPopoverTemplate.html',
            toggle: function () {
                vm.popOverOptionsCrearTramite.isOpen = !vm.popOverOptionsCrearTramite.isOpen;
            }
        };

        vm.objSolicitud = {
            codTramite: null,
            codEntidade: null,
            descripcion: null
        };


        //#endregion

        function downloadExcel() {

            servicioPanelPrincipal.obtenerExcelTramitesConsolaProcesos(vm.peticionObtenerInbox, vm.tramiteFiltro, vm.columnasReporte).then(function (retorno) {
                var blob = new Blob([retorno.data], {
                    type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
                });
                // FileSaver.saveAs(blob,nombreDelArchivo(retorno));
                FileSaver.saveAs(blob, "Tramites.xlsx");
            }, function (error) {
                vm.Mensaje = error.data.Message;
                mostrarMensajeRespuesta();
            });

        }

        function restablecerBusqueda() {
            vm.limpiarCamposFiltro();
            //vm.buscar(true);
            var iconoLimpiar = document.getElementById('icoLimpiarBusqueda');
            iconoLimpiar.src = "Img/IcolimpiarBusqD.svg";
            vm.cantidadDeTramites = null;
            vm.listaEntidades = [];
            vm.listaEntidadesGeneral = [];
            vm.listaTramites = [];
            vm.listaDatos = [];
            vm.listaTramitesCompleta = [];
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
                    codigoProceso: () => row.IdObjetoNegocio
                }
            });

            modalInstance.result.then(function (selectedItem) {


            }, function () {

            });
        }

        function downloadPdf() {
            var peticionObtenerInbox = {
                IdUsuario: usuarioDNP,
                IdObjeto: idTipoTramite,
                Aplicacion: nombreAplicacionBackbone,
                ListaIdsRoles: sesionServicios.obtenerUsuarioIdsRoles()
            };
            servicioPanelPrincipal.obtenerPdfInboxTramitesConsolaProcesos(peticionObtenerInbox, vm.tramiteFiltro, vm.columnasReporte).then(
                function (data) {
                    servicioPanelPrincipal.imprimirPdfTramites(data.data).then(function (retorno) {
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
            estadoId: {
                campo: 'EstadoId',
                valor: null,
                tipo: constantesCondicionFiltro.igual
            },
            tipoEntidad: {
                campo: 'NombreTipoEntidad',
                valor: "Nacional",
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
            },
            estado: {
                campo: 'DescEstado',
                valor: null,
                tipo: constantesCondicionFiltro.contiene
            },
            tipoTramiteId: {
                campo: 'TipoTramiteId',
                valor: null,
                tipo: constantesCondicionFiltro.igual
            },
            estadoInstanciaId: {
                campo: 'EstadoInstanciaId',
                valor: null,
                tipo: constantesCondicionFiltro.igual
            },
            numeroTramite: {
                campo: 'NumeroTramite',
                valor: null,
                tipo: constantesCondicionFiltro.igual
            }, 
            entidadProyectoId: {
                campo: 'EntidadProyectoId',
                valor: null,
                tipo: constantesCondicionFiltro.igual
            },
            vigencia: {
                campo: 'Vigencia',
                valor: null,
                tipo: constantesCondicionFiltro.igual
            },
            flujoId: {
                campo: 'FlujoId',
                valor: null,
                tipo: constantesCondicionFiltro.igual
            },
            accionId: {
                campo: 'AccionId',
                valor: null,
                tipo: constantesCondicionFiltro.igual
            }
        };

        //const tipoEntidad = entidad.GrupoTramites[0].NombreTipoEntidad;
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
            {
                field: 'numeroTramite',
                displayName: 'numeroTramite',
                enableHiding: false,
                width: '39%'
            },
            {
                field: 'tipoTramite',
                displayName: 'tipoTramite',
                enableHiding: false,
                width: '9%',
                type: "date",
                cellFilter: 'date:\'dd/MM/yyyy\''
            },
            {
                field: 'sector',
                displayName: 'sector',
                enableHiding: false,
                width: '9%'
            },
            {
                field: 'estadoTramite',
                displayName: 'estadoTramite',
                enableHiding: false,
                enableColumnMenu: false,
                width: '30%',
                cellTemplate: vm.consultarAccionFlujoTemplate,
            },
            {
                field: 'fecha',
                displayName: 'fecha',
                enableHiding: false,
                width: '9%'

            },
            {
                field: 'nombreAccion',
                displayName: 'nombreAccion',
                enableFiltering: false,
                enableHiding: false,
                enableSorting: false,
                enableColumnMenu: false,
                // pinnedRight: true,
                cellTemplate: vm.accionesFilaTramitesTemplate,
                width: '200'
            }, {
                field: 'fechaPaso',
                displayName: 'fechaPaso',
                enableHiding: false,
                width: '9%'

            }, {
                field: 'nombreFlujo',
                displayName: 'nombreFlujo',
                enableHiding: false,
                width: '9%'

            }, {
                field: 'entidad',
                displayName: 'entidad',
                enableHiding: false,
                width: '9%'

            }
        ];

        vm.todasColumnasDefinicion = Object.assign([], vm.columnDef);
        vm.listaEntidades = [];
        vm.listaTramites = [];
        vm.listaDatos = [];

        function onRegisterApi(gridApi) {
            $scope.gridApi = gridApi;
            $scope.gridApi.expandable.on.rowExpandedBeforeStateChanged($scope, function (row) {
                if (row.entity.subGridOptions.data.length === 1) {
                    $scope.vm.gridOptions.expandableRowHeight = 80;
                    if (row.isExpanded === true) {
                        row.expandedRowHeight = row.expandedRowHeight - 80;
                    }
                    else {
                        row.expandedRowHeight = 80;
                    }


                }
                else {
                    $scope.vm.gridOptions.expandableRowHeight = 50 + row.entity.subGridOptions.data.length * 30;
                    var heigrow = 50 + row.entity.subGridOptions.data.length * 30;
                    if (row.isExpanded === true) {
                        row.expandedRowHeight = row.expandedRowHeight - heigrow;
                    }
                    else {
                        row.expandedRowHeight = 50 + row.entity.subGridOptions.data.length * 30;
                    }

                }
            })
        }
        function onRegisterApi2(gridApi) {
            $scope.gridApi = gridApi;
            $scope.gridApi.expandable.on.rowExpandedBeforeStateChanged($scope, function (row) {
                if (row.entity.subGridOptions.data.length === 1) {
                    row.grid.options.expandableRowHeight = 80;
                    var heigrow = 50 + row.entity.subGridOptions.data.length * 30;
                    if (row.isExpanded === true) {
                        row.grid.parentRow.expandedRowHeight = row.grid.parentRow.expandedRowHeight - 80;
                    }
                    else {
                        row.grid.parentRow.expandedRowHeight = row.grid.parentRow.expandedRowHeight + 80;
                    }
                }
                else {
                    row.grid.options.expandableRowHeight = 50 + row.entity.subGridOptions.data.length * 30;
                    var heigrow = 50 + row.entity.subGridOptions.data.length * 30;
                    if (row.isExpanded === true) {
                        row.grid.parentRow.expandedRowHeight = row.grid.parentRow.expandedRowHeight - heigrow;
                    }
                    else {
                        row.grid.parentRow.expandedRowHeight = row.grid.parentRow.expandedRowHeight + 50 + row.entity.subGridOptions.data.length * 30;
                    }

                }
            })
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
                    } else {


                        //if (col == 'Identificador') {
                        //    if (vm.columnDef.map(x => x.displayName).indexOf('Tipo de Trámite') == -1) {
                        //        addCol = vm.todasColumnasDefinicion.filter(x => x.displayName == 'Tipo de Trámite')[0]
                        //        colAcciones = vm.todasColumnasDefinicion.filter(x => x.field == 'accion')[0]

                        //        tramite.subGridOptions.columnDefs.pop();
                        //        tramite.subGridOptions.columnDefs.push(addCol);
                        //        tramite.subGridOptions.columnDefs.push(colAcciones);
                        //    }
                        //} else {
                        //if (vm.columnDef.map(x => x.displayName).indexOf(col) == -1) {
                        //    addCol = vm.todasColumnasDefinicion.filter(x => x.displayName == col)[0]
                        //    colAcciones = vm.todasColumnasDefinicion.filter(x => x.field == 'accion')[0]

                        //    tramite.subGridOptions.columnDefs.pop();
                        //    tramite.subGridOptions.columnDefs.push(addCol);
                        //    tramite.subGridOptions.columnDefs.push(colAcciones);
                        //}
                        //}
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

                    //if (col == 'Identificador') {
                    //    if (tramite.subGridOptions.columnDefs.map(x => x.displayName).indexOf('Tipo de Trámite') > -1) {
                    //        indexEliminar = tramite.subGridOptions.columnDefs.map(x => x.displayName).indexOf('Tipo de Trámite');
                    //    }
                    //} else {
                    indexEliminar = tramite.subGridOptions.columnDefs.map(x => x.displayName).indexOf(col);
                    //}

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
                if ($localStorage.tipoFiltro.tramites) {
                    vm.columnas = $localStorage.tipoFiltro.tramites.columnasActivas;
                    vm.columnasDisponiblesPorAgregar = $localStorage.tipoFiltro.tramites.columnasDisponibles;
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
            //mostrarMensajeRespuesta();
            //listarFiltroSectores();
            listarFiltroEntidades();
            listarFiltroEstadoTramites();
            listarFiltroTipoTramites();
            vm.cantidadDeTramites = null;
            vm.BusquedaRealizada = false;

            vm.listaEntidades = [];
            vm.listaEntidadesGeneral = [];
            vm.listaTramites = [];
            vm.listaDatos = [];
            vm.listaTramitesCompleta = [];
            limpiarCamposFiltro();
            //cargarEntidades(idTipoProyecto).then(function () {
            //    mostrarMensajeRespuesta();
            //    listarFiltroSectores();
            //    listarFiltroEntidades();
            //    listarFiltroEstadoTramites();
            //    listarFiltroTipoTramites();
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
            //vm.tramiteFiltro.codigo.valor = null;
            vm.tramiteFiltro.descripcion.valor = null;
            vm.tramiteFiltro.fechaDesde.valor = null;
            vm.tramiteFiltro.fechaHasta.valor = null;
            vm.tramiteFiltro.sectorId.valor = null;
            vm.tramiteFiltro.entidadId.valor = null;
            vm.tramiteFiltro.nombreFlujo.valor = null;
            vm.tramiteFiltro.accionFlujo.valor = null;
            vm.tramiteFiltro.numeroTramite.valor = null;
            vm.tramiteFiltro.entidadProyectoId.valor= null;
            vm.tramiteFiltro.vigencia.valor= null;
            vm.tramiteFiltro.flujoId.valor= null;
            vm.tramiteFiltro.accionId.valor = null;
            vm.tramiteFiltro.estadoInstanciaId.valor = null;
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
            function error(error) { alert("Error"); }
        }
        function listarFiltroEntidadesProyecto() {

            servicioPanelPrincipal.ObtenerEntidadesPorSector(vm.tramiteFiltro.sectorId.valor, vm.tramiteFiltro.tipoEntidad.valor).then(exito, error);

            function exito(respuesta) {
                vm.ListaEntidadesProyectos = respuesta.data || [];
            }

            function error() {
                vm.ListaEntidadesProyectos = [];
            }
        }

        function listarFiltroEntidadesCombo() {

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
                vm.listaFiltroEntidades = listaFiltroEntidades;//.filter(item => listaEntidadesGrid.includes(item.Name));
            }

            function error() {
                vm.listaFiltroEntidades = [];
            }

        }

        function listarFiltroEstadoTramites() {
            //let listaEstadoTramitesGrid = [];

            //vm.listaEntidades.forEach(entidade => {
            //    if (entidade.tipoEntidad == vm.tipoEntidad) {
            //        entidade.subGridOptions.data.forEach(item => {

            //            item.subGridOptions.data.forEach(tramite => {

            //                listaEstadoTramitesGrid.push({ value: tramite.estadoTramite, text: tramite.estadoTramite });
            //            });

            //        });
            //    }
            //});

            //const seen = new Set();
            vm.listaFiltroEstadoTramites = [];
            vm.listaFiltroEstadoTramites.push({ value: "Activo", text: "Activo" });
            vm.listaFiltroEstadoTramites.push({ value: "Anulado por alcance", text: "Anulado por alcance" });
            vm.listaFiltroEstadoTramites.push({ value: "Completado", text: "Completado" });
            vm.listaFiltroEstadoTramites.push({ value: "Pausado", text: "Pausado" });
            vm.listaFiltroEstadoTramites.push({ value: "Cancelado", text: "Cancelado" });
            vm.listaFiltroEstadoTramites.push({ value: "Anulado", text: "Anulado" });
            //vm.listaFiltroEstadoTramites = listaEstadoTramitesGrid.filter(el => {
            //    const duplicate = seen.has(el.value);
            //    seen.add(el.value);
            //    return !duplicate;
            //});

        }

        function listarFiltroTipoTramites() {
            servicioPanelPrincipal.obtenerTiposTramite(vm.peticionObtenerInbox, vm.tramiteFiltro).then(exito, error);

            function exito(respuesta) {
                let listaFiltroTipoTramites = [];
                if (respuesta.data && respuesta.data.length > 0) {
                    listaFiltroTipoTramites = respuesta.data;
                }
                vm.listaFiltroTipoTramites = listaFiltroTipoTramites;
            }

            function error(error) {
                vm.listaFiltroTipoTramites = [];
            }
            //let listaTipoTramitesGrid = [];

            //vm.listaEntidades.forEach(entidade => {
            //    if (entidade.tipoEntidad == vm.tipoEntidad) {
            //        entidade.subGridOptions.data.forEach(item => {

            //            item.subGridOptions.data.forEach(tramite => {
            //                if (tramite.tipoTramiteId != null)
            //                    listaTipoTramitesGrid.push({ value: tramite.tipoTramiteId, text: tramite.tipoTramite });
            //            });

            //        });
            //    }
            //});

            //const seen = new Set();
            //vm.listaFiltroTipoTramites = [];
            //vm.listaFiltroTipoTramites = listaTipoTramitesGrid.filter(el => {
            //    const duplicate = seen.has(el.value);
            //    seen.add(el.value);
            //    return !duplicate;
            //});

        }

        //Implementaciones
        //$scope.$on('AutorizacionConfirmada', function () {
        //    $timeout(function () {
        //        if (vm.yaSeCargoInbox === false) {
        //            var roles = sesionServicios.obtenerUsuarioIdsRoles();
        //            if (roles != null && roles.length > 0) {
        //                activar(); //Realizar la carga
        //            } else {
        //                vm.Mensaje = $filter('language')('ErrorUsuarioSinPermisosAplicacion');
        //                mostrarMensajeRespuesta(idTipoProyecto);
        //            }
        //        }
        //    });
        //});

        this.$onInit = function () {
            // buscar columnas en el localstorage
            //buscarColumnasLocalStorage();

            if (!vm.gridOptions) {
                vm.gridOptions = {
                    expandableRowTemplate: vm.plantillaTramites,
                    expandableRowScope: {
                        subGridVariable: 'subGridScopeVariable'
                    },
                    enableFiltering: false,
                    showHeader: false,
                    paginationPageSizes: [10, 15, 25, 50, 100],
                    paginationPageSize: 10,
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
            configurarNombreEtapa();

            //listarFiltroEntidades();
            cargarFlujosTramites();
        };


        function activar() {
            vm.yaSeCargoInbox = true;
            // ReSharper disable once UndeclaredGlobalVariableUsing
            // obtenerInbox(idTipoTramite).then(function () {
            //     // ReSharper disable once UndeclaredGlobalVariableUsing
            //     return obtenerInbox(vm.idTipoProyecto);
            // });
            //escucharEventos();


            obtenerInbox(idTipoProyecto); // PARA TESTAR
            //obtenerInbox(idTipoTramite); CORRETO
        }

        function buscarColumnasPorColumnasFiltroSeleccionadas() {
            let listaColumnas = [];
            let columna = ''

            for (let i = 0; i < vm.columnas.length; i++) {
                var nombreColumnasSeleccionadaFiltro = vm.columnas[i];

                //if (nombreColumnasSeleccionadaFiltro == 'Nombre Flujo' || nombreColumnasSeleccionadaFiltro == 'Accion Flujo') {
                //    nombreColumnasSeleccionadaFiltro = 'Nombre/Accion Flujo';
                //}
                var result = vm.todasColumnasDefinicion.filter(x => x.displayName == nombreColumnasSeleccionadaFiltro);
                if (result != null && result.length > 0) {
                    columna = result[0].field;
                    if (listaColumnas.indexOf(columna) == -1) {
                        listaColumnas.push(columna);
                    }
                }
            }

            return listaColumnas;
        }

        function cargarEntidades(idObjeto) {
            //const columnasVisibles = vm.listaTramites[0].subGridOptions.columnDefs.map(x => x.field).filter(x => x != 'accion');
            const columnasVisibles = buscarColumnasPorColumnasFiltroSeleccionadas();

            return servicioPanelPrincipal.obtenerTramitesConsolaProcesos(vm.peticionObtenerInbox, vm.tramiteFiltro, columnasVisibles).then(
                function (respuesta) {
                    // ReSharper disable once UndeclaredGlobalVariableUsing
                    // if (idObjeto === vm.idTipoProyecto) {
                    //     vm.gruposEntidadesProyectos = respuesta.data.GruposEntidades;
                    // } else {
                    //     vm.gruposEntidadesTramites = respuesta.data.GruposEntidades;
                    // }
                    vm.cantidadDeTramites = 0;
                    vm.listaEntidades = [];
                    vm.listaEntidadesGeneral = [];
                    vm.listaTramites = [];
                    vm.listaDatos = [];
                    vm.listaTramitesCompleta = [];

                    if (respuesta.data.ListaGrupoTramiteEntidad && respuesta.data.ListaGrupoTramiteEntidad.length > 0) {
                        vm.listaEntidades = [];
                        vm.listaEntidadesGeneral = [];
                        const listaGrupoEntidades = respuesta.data.ListaGrupoTramiteEntidad;

                        listaGrupoEntidades.forEach(entidad => {
                            const nombreEntidad = entidad.NombreEntidad;
                            const tipoEntidad = entidad.GrupoTramites[0].NombreTipoEntidad;
                            const nombreSector = entidad.Sector;
                            const idSector = entidad.IdSector;
                            const idEntidad = entidad.EntidadId;
                            

                            vm.listaTramitesCompleta = [];
                            vm.listaTramites = [];
                            entidad.GrupoTramites.forEach(tramite => {
                                vm.cantidadDeTramites += tramite.ListaTramites.length;
                                const nombreTipoTramite = tramite.NombreTipoTramite;

                                vm.listaDatos = [];
                                tramite.ListaTramites.forEach(instancia => {

                                    const valorDeshabilitar = instancia.NombreAccion === undefined || instancia.NombreAccion === null || !instancia.NombreAccion.startsWith("1.") || instancia.EstadoId === 2 || instancia.EstadoId === 5;

                                    if (rol !== undefined && rol !== null) {
                                        vm.presupuestoPreliminar = true;
                                    }

                                    vm.listaTramitesCompleta.push({
                                        tipoTramite: nombreTipoTramite,
                                        codigo: instancia.Id,
                                        descripcion: instancia.Descripcion,
                                        fecha: instancia.FechaCreacion,
                                        valorProprio: instancia.ValorProprio,
                                        valorSGR: instancia.ValorSGP,
                                        tipoTramite: instancia.NombreTipoTramite,
                                        entidad: nombreEntidad,
                                        entidadDestino: instancia.NombreEntidadDestino,
                                        identificadorCR: instancia.IdentificadorCR,
                                        estadoTramite: instancia.DescEstado,
                                        sector: instancia.NombreSector,
                                        estadoId: instancia.EstadoId,
                                        tipoTramiteId: instancia.TipoTramiteId,
                                        IdObjetoNegocio: instancia.IdObjetoNegocio,
                                        NombreObjetoNegocio: instancia.NombreObjetoNegocio,
                                        IdFlujo: instancia.FlujoId,
                                        NombreAccion: instancia.NombreAccion,
                                        IdInstancia: instancia.IdInstancia,
                                        NombreFlujo: instancia.NombreFlujo,
                                        entidadId: idEntidad,
                                        fechaPaso: instancia.FechaCreacion,
                                        numeroTramite: instancia.NumeroTramite,
                                        macroproceso: instancia.Macroproceso,
                                        permiteEliminar: (vm.presupuestoPreliminar && !valorDeshabilitar)
                                    });

                                    vm.listaDatos.push({
                                        codigo: instancia.Id,
                                        descripcion: instancia.Descripcion,
                                        fecha: instancia.FechaCreacion,
                                        valorProprio: instancia.ValorProprio,
                                        valorSGR: instancia.ValorSGP,
                                        tipoTramite: instancia.NombreTipoTramite,
                                        entidad: nombreEntidad,
                                        entidadDestino: instancia.NombreEntidadDestino,
                                        identificadorCR: instancia.IdentificadorCR,
                                        estadoTramite: instancia.DescEstado,
                                        sector: instancia.NombreSector,
                                        estadoId: instancia.EstadoId,
                                        anio: instancia.FechaCreacion.split("-")[0],
                                        anio_actual: new Date().getFullYear().toString(),
                                        tipoTramiteId: instancia.TipoTramiteId,
                                        IdObjetoNegocio: instancia.IdObjetoNegocio,
                                        NombreObjetoNegocio: instancia.NombreObjetoNegocio,
                                        IdFlujo: instancia.FlujoId,
                                        NombreAccion: instancia.NombreAccion,
                                        IdInstancia: instancia.IdInstancia,
                                        NombreFlujo: instancia.NombreTipoTramite,
                                        entidadId: idEntidad,
                                        TramiteId: instancia.TramiteId,
                                        permiteEliminar: (vm.presupuestoPreliminar && !valorDeshabilitar)
                                    });

                                    console.log('vm.listaDatos');
                                    console.log(vm.listaDatos);
                                });

                                vm.listaTramites.push({
                                    sector: 'Tramite',
                                    entidad: nombreTipoTramite,
                                    subGridOptions: {
                                        columnDefs: vm.columnDef,
                                        appScopeProvider: $scope,
                                        paginationPageSizes: [5, 10, 15, 25, 50, 100],
                                        paginationPageSize: 5,
                                        data: vm.listaDatos,
                                        excessRows: vm.listaDatos.length
                                    }
                                });
                            });
                            vm.listaEntidadesGeneral.push({
                                sector: nombreSector,
                                entidad: nombreEntidad,
                                tipoEntidad: tipoEntidad,
                                idSector: idSector,
                                entidadId: idEntidad,
                                estadoEntidad: "+",
                                subGridOptions: {
                                    data: vm.listaTramitesCompleta,
                                    excessRows: vm.listaTramitesCompleta.length,
                                }
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
                                    enableFiltering: false,
                                    showHeader: false,
                                    data: vm.listaTramites,
                                    excessRows: vm.listaTramites.length,
                                    onRegisterApi: onRegisterApi2
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

        function mostrarMensajeRespuesta(idObjeto) {
            // ReSharper disable once UndeclaredGlobalVariableUsing
            if (idObjeto === vm.idTipoProyecto && vm.cantidadDeProyectos === 0) {
                vm.mostrarMensajeProyectos = true;
            } else {
                vm.mostrarMensajeProyectos = false;
            }

            // ReSharper disable once UndeclaredGlobalVariableUsing
            if (idObjeto === idTipoTramite && vm.cantidadDeTramites === 0) {
                vm.mostrarMensajeTramites = true;
            } else {
                vm.mostrarMensajeTramites = false;
            }
        }


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

        function obtenerInbox(idObjeto) {
            limpiarCamposFiltro();
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
            listarFiltroEntidadesCombo();
            listarFiltroEstadoInstancia();
            listarFiltroFlujos();
            listarFiltroVigencias();
            //mostrarMensajeRespuesta(idObjeto);
            //return cargarEntidades(idObjeto).then(function () {


            //});
        };

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
                vm.tramiteFiltro.vigencia.valor = 2023;
            }

            function error() {
                vm.listaFiltroVigencias = [];
            }
        }
        function listarFiltroEstadoInstancia() {
            servicioPanelPrincipal.obtenerEstadoInstancia(vm.peticionObtenerInbox).then(exito, error);

            function exito(respuesta) {
                vm.listaFiltroEstadoInstancias = respuesta.data || [];
                vm.tramiteFiltro.estadoInstanciaId.valor = "1"; //Defecto Activo
            }

            function error() {
                vm.listaFiltroEstadoInstancias = [];
            }
        }

        function listarFiltroAcciones() {

            servicioPanelPrincipal.ObtenerAccionesFlujoPorFlujoId(vm.tramiteFiltro.flujoId.valor).then(exito, error);

            function exito(respuesta) {
                vm.listaFiltroAccion = respuesta.data || [];
            }

            function error() {
                vm.listaFiltroAccion = [];
            }
        }

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
                        'tramites': {
                            'columnasActivas': selectedItem.columnasActivas,
                            'columnasDisponibles': selectedItem.columnasDisponibles
                        }
                    };
                } else {
                    $localStorage.tipoFiltro['tramites'] = {
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

        function consultarAccion(idInstancia, nombreProyecto, idObjetoNegocio, idEntidad, nombreflujo, row) {

            $sessionStorage.idInstancia = $sessionStorage.idInstanciaFlujoPrincipal = idInstancia;
            $sessionStorage.nombreProyecto = nombreProyecto;
            $sessionStorage.idObjetoNegocio = idObjetoNegocio;
            $sessionStorage.idEntidad = idEntidad;
            if (nombreflujo !== null) {
                $sessionStorage.InstanciaSeleccionada = row;
                $sessionStorage.nombreFlujo = nombreflujo;
            }
            $timeout(function () {
                $location.path('/Acciones/ConsultarAcciones');
            }, 300);
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

        function alcanceTramite(row) {
            $uibModal.open({
                animation: $scope.animationsEnabled,
                templateUrl: 'src/app/panelPrincial/modales/tramite/alcanceModal.html',
                controller: 'alcanceModalController',
                controllerAs: "vm",
                size: 'lg',
                openedClass: "entidad-modal-adherencia",
                resolve: {
                    IdInstancia: () => row.IdInstancia,
                    TramiteId: () => row.TramiteId
                },
            });
        }

        function detenerTramite(tramite) {
            // solo rol presupuesto
            //var cumpleRol = $sessionStorage.usuario.roles.filter(x => x.Nombre.includes("Presupuesto") == true);
            // solo rol presupuesto - preliminar
            var cumpleRol = $sessionStorage.usuario.roles.filter(x => x.Nombre.includes("Presupuesto - preliminar") == true);
            if (cumpleRol.length > 0) {
                if (!tramite.NombreAccion.startsWith('1.')) {
                    utilidades.mensajeError("El trámite No se encuentra en el Paso 1, por lo tanto, no se puede eliminar.");
                }
                else {
                    utilidades.mensajeWarning("¿Desea eliminar el trámite " + tramite.IdObjetoNegocio + "? <br>Si elimina el trámite, se eliminará toda la información que se ha diligenciado.", function funcionContinuar() {

                                servicioPanelPrincipal.detenerInstancia(vm.peticionObtenerInbox, tramite.IdInstancia).then(
                                    function (respuesta) {
                                        //servicioPanelPrincipal.devolverInstanciasHijas(vm.peticionObtenerInbox, tramite.IdInstancia).then(
                                        //    function (respuestahijos) {

                                        //},

                                        //    function (error) {
                                        //        if (error) {
                                        //            if (error.status) {
                                        //                switch (error.status) {
                                        //                    case 401:
                                        //                        vm.Mensaje = $filter('language')('ErrorUsuarioSinPermisosAplicacion');
                                        //                        break;
                                        //                    case 500:
                                        //                        vm.Mensaje = $filter('language')('ErrorObtenerDatos');
                                        //                    default:
                                        //                        vm.Mensaje = error.statusText;
                                        //                        break;
                                        //                }
                                        //            }
                                        //        }

                                        //        mostrarMensajeRespuesta();
                                        //    }
                                        //);
                                        mostrarMensajeRespuesta();
                                        cargarEntidades();
                                        utilidades.mensajeSuccess("El trámite ha sido eliminado", false, false, false);

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
            }
            else {
                vm.presupuestoPreliminar = false;
                utilidades.mensajeError("Esta acción solo puede ser ejecutada por el rol de Presupuesto Preliminar.");
            }

        }


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

        //#region Componente Crear Tramites

        function listarFiltroEntidades() {
            autorizacionServicios.obtenerListaEntidad(usuarioDNP).then(exito, error);

            function exito(respuesta) {

                var listaAuxEntidades = [];

                respuesta.forEach(element => {
                    listaAuxEntidades.push({
                        id: element.EntityTypeCatalogOptionId,
                        nombre: element.Entidad,
                        tipoEntidad: element.TipoEntidad
                    })
                });
                vm.listaEntidades = listaAuxEntidades;

                listarFiltroEntidadesCombo();
            }

            function error() {
                vm.listaEntidades = [];
            }
        }

        function cargarFlujosTramites() {

            return flujoServicios.obtenerFlujosPorRoles().then(
                function (flujos) {

                    flujos = flujos.filter(filtrarFlujosPorTipoObjetoSeleccionado);

                    var listaAuxFlujos = [];

                    flujos.forEach(element => {
                        listaAuxFlujos.push({
                            id: element.IdOpcionDnp,
                            nombre: element.NombreOpcion
                        })
                    });
                    vm.flujos = flujos;
                    vm.tramites = listaAuxFlujos;
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
            function filtrarFlujosPorTipoObjetoSeleccionado(flujo) {
                return flujo.TipoObjeto && flujo.TipoObjeto.Id === idTipoTramite;
            };
        }

        function guardarSolicitud() {

            if (!vm.objSolicitud.codTramite) {
                utilidades.mensajeError('El campo Tipo Tramite es obligatorio', false);
                return;
            }
            if (!vm.objSolicitud.codEntidade) {
                utilidades.mensajeError('El campo Entidad es obligatorio', false);
                return;
            }

            var usuarioRolId = sesionServicios.obtenerUsuarioIdsRoles()[0];

            var instanciaDto = {
                FlujoId: vm.objSolicitud.codTramite,
                ObjetoId: vm.objSolicitud.codEntidade,
                UsuarioId: usuarioDNP,
                RolId: usuarioRolId,
                TipoObjetoId: idTipoTramite,
                ListaEntidades: [vm.objSolicitud.codEntidade],
                Descripcion: vm.objSolicitud.descripcion
            }

            flujoServicios.crearInstancia(instanciaDto).then(
                function (resultado) {
                    if (!resultado.length) {
                        utilidades.mensajeError('No se creó instancia');
                        return;
                    }

                    vm.cerrarTramite();
                    utilidades.mensajeSuccess('Se crearon instancias exitosamente');
                    $("#tramits").scope().vm.obtenerInbox(idTipoTramite);
                    $("#cantidadTramites").scope().vm.obtenerNotificacionesCantidadDeTramites();
                    $("#tramitesCantidad").scope().vm.obtenerNotificacionesCantidadDeTramites();
                },
                function (error) {
                    if (error) {
                        utilidades.mensajeError(error);
                    }
                }
            );
        }

        function cerrarTramite() {
            vm.objSolicitud = {
                codTramite: null,
                codEntidade: null,
                descripcion: null
            };
            vm.popOverOptionsCrearTramite.toggle();
        }

        //#endregion
    }

    angular.module('backbone')
        .component('consolaprocesostramites', {
            templateUrl: "/src/app/panelPrincial/componentes/inbox/consolaprocesostramites.template.html",
            controller: 'consolaprocesostramitesController',
            controllerAs: 'vm'
        });

})();