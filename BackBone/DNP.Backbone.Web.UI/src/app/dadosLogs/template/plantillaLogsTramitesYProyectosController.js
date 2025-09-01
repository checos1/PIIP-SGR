let plantillaLogTramiteProyectosCtrl = null;

(function () {

    'use strict';

    angular.module('backbone').controller('plantillaLogsTramitesYProyectosController', plantillaLogsTramitesYProyectosController);
    plantillaLogsTramitesYProyectosController.$inject = [
        '$scope',
        'constantesAutorizacion',
        'configurarEntidadRolSectorServicio',
        'backboneServicios',
        'sesionServicios',
        '$uibModal',
        '$localStorage',
        '$location',
        'flujoServicios',
        'constantesTipoFiltro',
        'servicioPanelPrincipal',
        'FileSaver'
    ];

    function plantillaLogsTramitesYProyectosController(
        $scope,
        constantesAutorizacion,
        configurarEntidadRolSectorServicio,
        backboneServicios,
        sesionServicios,
        $uibModal,
        $localStorage,
        $location,
        flujoServicios,
        constantesTipoFiltro,
        servicioPanelPrincipal,
        FileSaver
    ) {

        var vm = this;
        plantillaLogTramiteProyectosCtrl = vm ;

        //#region Variables
        vm.tipoFiltro = constantesTipoFiltro.proyecto;
        vm.columnas = ['CODIGO', 'FECHA', 'ENTIDAD', 'BPIN', 'DESCRIPCIÓN', 'ESTADO', 'USUARIO'];
        vm.columnasDisponiblesPorAgregar = [];
        vm.instanciasLog = [];
        vm.tipoEntidad = null;
        vm.mostrarFiltro = false;
        vm.peticion = {};
        vm.roles = obtenerRoles();
        vm.titulo = 'PROYECTOS'
        vm.entidades = [];

        vm.puedeVerFiltroCodigo;
        vm.puedeVerFiltroFechaInicio;
        vm.puedeVerFechaFin;
        vm.puedeVerFiltroEntidad;
        vm.puedeVerFiltroBPIN;
        vm.puedeVerFiltroDescripcion;
        vm.puedeVerFiltroEstado;

        vm.filtro = {
            Codigo: null,
            FechaInicio: null,
            FechaFin: null,
            BPIN: null,
            Descripcion: null,
            Estado: null,
            UsuarioId: null,
            EntityTypeCatalogOptionId: null,
            esTramite: null
        }

        let columnasDef = [{
            field: 'Id',
            displayName: "CODIGO",
            width: "8%",
            headerCellClass: 'ui-grid-cell-header',
            cellTooltip: (row, col) => row.entity[col.field]
        },
        {
            field: 'Fecha',
            displayName: "FECHA",
            width: "15%",
            type: "date",
            cellFilter: 'date:"dd/MM/yyyy hh:mm"',
            headerCellClass: 'ui-grid-cell-header ui-grid-cell-align-center',
            cellClass: 'ui-grid-cell-align-center',
            cellTooltip: (row, col) => row.entity[col.field]
        },
        {
            field: 'Entidad',
            displayName: "ENTIDAD",
            width: "19%",
            headerCellClass: 'ui-grid-cell-header ui-grid-cell-align-center',
            cellClass: 'ui-grid-cell-align-center',
            cellTooltip: (row, col) => row.entity[col.field]
        },
        {
            field: 'BPIN',
            displayName: "BPIN",
            width: "12%",
            headerCellClass: 'ui-grid-cell-header',
            cellTooltip: (row, col) => row.entity[col.field]
        },
        {
            field: 'Descripcion',
            displayName: "DESCRIPCIÓN",
            headerCellClass: 'ui-grid-cell-header ui-grid-cell-align-center',
            cellClass: 'ui-grid-cell-align-center',
            width: "20%",
            cellTooltip: (row, col) => row.entity[col.field]
        },
        {
            field: 'Estado',
            displayName: "ESTADO",
            headerCellClass: 'ui-grid-cell-header ui-grid-cell-align-center',
            cellClass: 'ui-grid-cell-align-center',
            width: "12%",
            cellTooltip: (row, col) => row.entity[col.field]
        },
        {
            field: 'NombreUsuario',
            displayName: "USUARIO",
            headerCellClass: 'ui-grid-cell-header ui-grid-cell-align-center',
            cellClass: 'ui-grid-cell-align-center',
            width: "13%",
            cellTooltip: (row, col) => row.entity[col.field]
        }
        ];

        columnasDef = esTramite() ? columnasDef.filter(p => p.field !== 'BPIN') : columnasDef;

        vm.gridOptions = {
            enableColumnMenus: false,
            paginationPageSizes: [5, 10, 25, 50, 100],
            paginationPageSize: 5,
            columnDefs: columnasDef,
            onRegisterApi: onRegisterApi,
            data: []
        };

        vm.todasColumnasDefinicion = Object.assign([], columnasDef);
        //#endregion

        //#region Métodos

        vm.listaConfiguracionesRolSector = obtenerConfiguracionesRolSector();
        vm.conmutadorFiltro = conmutadorFiltro;
        vm.init = init;
        vm.limpiarCamposFiltro = limpiarCamposFiltro;
        vm.cambioTipoEntidad = cambioTipoEntidad;
        vm.esTramite = esTramite;
        vm.downloadExcel = downloadExcel;
        vm.downloadPdf = downloadPdf;
        vm.buscar = buscar;
        vm.abrirModalAdicionarColumnas = abrirModalAdicionarColumnas;
        //#endregion

        function esTramite() {
            if ($location.$$url == "/logs/tramites") {
                vm.titulo = 'TRAMITES'
                return true;
            }
            return false;
        }

        function downloadExcel() {

            servicioPanelPrincipal.obtenerExcelLogInstancia(vm.gridOptions.data).then(function (retorno) {
                var blob = new Blob([retorno.data], {
                    type: "application/octet-stream"
                });
                FileSaver.saveAs(blob, "LogInstancias.xls");
            }, function (error) {
                vm.Mensaje = error.data;
                mostrarMensajeRespuesta();
            });
        }

        function downloadPdf() {
            flujoServicios.obtenerPdfLogInstancias(vm.gridOptions.data).then(function (retorno) {
                var blob = new Blob([retorno.data], {
                    type: "application/octet-stream"
                });
                FileSaver.saveAs(blob, nombreDelArchivo(retorno));
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

        function obtenerRoles() {
            var roles = sesionServicios.obtenerUsuarioIdsRoles();

            if (backboneServicios.estaAutorizado() && roles != null && roles.length > 0) {
                vm.peticion = {
                    IdUsuarioDNP: usuarioDNP,
                    Aplicacion: nombreAplicacionBackbone,
                    IdsRoles: roles
                };
            }
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

        function tipoEntidadPresenteEnLaConfiguracion(tipoEntidad) {
            var tipoConfiguracion = _.find(vm.listaConfiguracionesRolSector, {
                TipoEntidad: tipoEntidad
            });
            return tipoConfiguracion ? true : false;
        }

        function cambioTipoEntidad(tipoEntidad) {
            limpiarCamposFiltro();
            vm.tipoEntidad = tipoEntidad;
            vm.filtro.TipoEntidad = tipoEntidad;

             vm.uiSelect.selected = null;
            _listaConsolaLogs();
        }

        function conmutadorFiltro() {
            limpiarCamposFiltro();
            vm.mostrarFiltro = !vm.mostrarFiltro;
        }

        function limpiarCamposFiltro() {
            for (const prop in vm.filtro)
                vm.filtro[prop] = null;
        }

        async function init() {
            if (!vm.tipoEntidad) {
                vm.tipoEntidad = constantesAutorizacion.tipoEntidadNacional;
            }

            vm.filtro.esTramite = vm.esTramite();

            vm.columnas = vm.filtro.esTramite ? vm.columnas.filter(p => p !== 'BPIN') : vm.columnas;
            columnasDef = vm.filtro.esTramite ? columnasDef.filter(p => p.field !== 'BPIN') : columnasDef;
            vm.puedeVerFiltroBPIN = vm.filtro.esTramite ? false : true;

            _listaConsolaLogs(true);
            buscarColumnasLocalStorage();
            return;
        }

        function buscarEntidad(entidades, entityTypeCatalogOptionId) {
            let entidad = entidades.find(p => p.EntityTypeCatalogOptionId === entityTypeCatalogOptionId);
            if (entidad)
                return entidad.Entidad;
            return null;
        }

        function onRegisterApi(gridApi) {
            $scope.gridApi = gridApi;
        }

        vm.uiSelect = {
            selected: null
        };

        async function _listaConsolaLogs(cargarEntidades = true) {
            let entidadesConId;
            if (cargarEntidades) {
                let entidades = await flujoServicios.obtenerEntidades(vm.tipoEntidad);
                entidadesConId = entidades.data.filter(p => p.EntityTypeCatalogOptionId !== null);
                vm.entidades = [];
            }

            vm.filtro.esTramite = vm.esTramite();
            return flujoServicios.obtenerLogInstancia(vm.filtro).then((respuesta) => {
                var listaAuxLogs = [];
                if (cargarEntidades) {
                    
                    vm.entidades = entidadesConId;
                }

                respuesta.forEach(element => {
                    let entidad = buscarEntidad(vm.entidades, element.EntityCatalogOptionId);
                    if (entidad) {
                        listaAuxLogs.push({
                            Descripcion: element.Descripcion,
                            BPIN: element.BPIN,
                            EntityCatalogOptionId: element.EntityCatalogOptionId,
                            Estado: element.Estado,
                            Fecha: element.Fecha ? moment.utc(element.Fecha).local().toDate() : null,
                            Id: element.Id,
                            Entidad: entidad,
                            TipoObjetoId: element.TipoObjetoId,
                            UsuarioId: element.UsuarioId,
                            NombreUsuario: element.NombreUsuario
                        })
                    }
                });
                vm.instanciasLog = listaAuxLogs;
                vm.gridOptions.data = listaAuxLogs;

                return;
            })
                .catch(error => {
                    vm.gridOptions.data = [];
                    console.log(error);
                    toastr.error("Hubo un error al cargar los logs");
                });
        }

        function buscar() {
            _listaConsolaLogs(false);
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
        }

        function agregarColumnas() {
            let lista = vm.instanciasLog;

            var addColId;
            var addColFecha;
            var addColEntidad;
            var addColBPIN;
            var addColDescripcion;
            var addColEstado;
            var addColNombreUsuario;

            for (let j = 0; j < vm.columnas.length; j++) {
                var col = vm.columnas[j];

                if (col == 'CODIGO') {
                    if (columnasDef.map(x => x.displayName).indexOf('CODIGO') == -1) {
                        addColId = vm.todasColumnasDefinicion.filter(x => x.displayName == 'CODIGO')[0];
                        vm.gridOptions.columnDefs.push(addColId);
                    }
                }

                if (col == 'FECHA') {
                    if (columnasDef.map(x => x.displayName).indexOf('FECHA') == -1) {
                        addColFecha = vm.todasColumnasDefinicion.filter(x => x.displayName == 'FECHA')[0];
                        vm.gridOptions.columnDefs.push(addColFecha);
                    }
                }

                if (col == 'ENTIDAD') {
                    if (columnasDef.map(x => x.displayName).indexOf('ENTIDAD') == -1) {
                        addColEntidad = vm.todasColumnasDefinicion.filter(x => x.displayName == 'ENTIDAD')[0];
                        vm.gridOptions.columnDefs.push(addColEntidad);
                    }
                }

                if (col == 'BPIN') {
                    if (columnasDef.map(x => x.displayName).indexOf('BPIN') == -1) {
                        addColBPIN = vm.todasColumnasDefinicion.filter(x => x.displayName == 'BPIN')[0];
                        vm.gridOptions.columnDefs.push(addColBPIN);
                    }
                }

                if (col == 'DESCRIPCIÓN') {
                    if (columnasDef.map(x => x.displayName).indexOf('DESCRIPCIÓN') == -1) {
                        addColDescripcion = vm.todasColumnasDefinicion.filter(x => x.displayName == 'DESCRIPCIÓN')[0];
                        vm.gridOptions.columnDefs.push(addColDescripcion);
                    }
                }

                if (col == 'ESTADO') {
                    if (columnasDef.map(x => x.displayName).indexOf('ESTADO') == -1) {
                        addColEstado = vm.todasColumnasDefinicion.filter(x => x.displayName == 'ESTADO')[0];
                        vm.gridOptions.columnDefs.push(addColEstado);
                    }
                }

                if (col == 'USUARIO') {
                    if (columnasDef.map(x => x.displayName).indexOf('USUARIO') == -1) {
                        addColNombreUsuario = vm.todasColumnasDefinicion.filter(x => x.displayName == 'USUARIO')[0];
                        vm.gridOptions.columnDefs.push(addColNombreUsuario);
                    }
                }
            }
        }

        function borrarColumnas() {
            for (var j = 0, lenCol = vm.columnasDisponiblesPorAgregar.length; j < lenCol; j++) {
                var indexEliminar = -1;
                var col = vm.columnasDisponiblesPorAgregar[j];


                indexEliminar = vm.gridOptions.columnDefs.map(x => x.displayName).indexOf(col);

                if (indexEliminar >= 0) vm.gridOptions.columnDefs.splice(indexEliminar, 1);
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
                if ($localStorage.tipoFiltro.consola_proyectos) {
                    vm.columnas = $localStorage.tipoFiltro.consola_proyectos.columnasActivas;
                    vm.columnasDisponiblesPorAgregar = $localStorage.tipoFiltro.consola_proyectos.columnasDisponibles;
                }
            }

            vm.puedeVerFiltroCodigo = vm.columnas.indexOf('CODIGO') > -1;
            vm.puedeVerFiltroFechaInicio = vm.columnas.indexOf('FECHA') > -1;
            vm.puedeVerFechaFin = vm.columnas.indexOf('FECHA') > -1;
            vm.puedeVerFiltroEntidad = vm.columnas.indexOf('ENTIDAD') > -1;
            vm.puedeVerFiltroBPIN = vm.columnas.indexOf('BPIN') > -1;
            vm.puedeVerFiltroDescripcion = vm.columnas.indexOf('DESCRIPCIÓN') > -1;
            vm.puedeVerFiltroEstado = vm.columnas.indexOf('ESTADO') > -1;
            vm.puedeVerFiltroUsuarioId = vm.columnas.indexOf('USUARIO') > -1;
        }
    }
})();