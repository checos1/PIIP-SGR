(function () {
    'use strict';

    angular.module('backbone.entidades').controller('inflexibilidadesListaController', inflexibilidadesListaController);
    inflexibilidadesListaController.$inject = [
        '$scope',
        '$uibModal',
        'constantesTipoFiltro',
        'uiGridConstants',
        '$routeParams',
        '$location',
        '$localStorage',
        'inflexibilidadServicio',
        'utilidades',
        'FileSaver',
        'Blob',
    ];

    function inflexibilidadesListaController(
        $scope,
        $uibModal,
        constantesTipoFiltro,
        uiGridConstants,
        $routeParams,
        $location,
        $localStorage,
        inflexibilidadServicio,
        utilidades,
        FileSaver,
        Blob,) {

        //#region Variables

        var vm = this;
        vm.listaInflexibilidades = [];
        vm.entidad = {};
        vm.mostrarFiltro = false;
        vm.peticion = {};
        //#endregion

        vm.conmutadorFiltro = conmutadorFiltro;
        vm.init = init;
        vm.limpiarCamposFiltro = limpiarCamposFiltro;
        vm.buscar = buscar;
        vm.downloadPdf = downloadPdf;
        vm.downloadExcel = downloadExcel;
        vm.eliminar = confirmarEliminar;
        vm.editar = editar;
        vm.volver = volver;
        vm.verPagamentos = verPagamentos;
        vm.obtenerClassEstado = obtenerClassEstado;
        vm.tieneColumna = tieneColumna;
        vm.agregarNuevaInflexibilidad = agregarNuevaInflexibilidad;
        vm.abrirModalAdicionarColumnas = abrirModalAdicionarColumnas;



        const accionesInflexibilidades = 'src/app/entidades/entidades/tipoEntidad/plantillas/plantillaAccionesInflexibilidades.html';
        const cellTemplateEstado = 'src/app/entidades/entidades/tipoEntidad/plantillas/plantillaEstadoInflexibilidade.html';

        const columnDef = [
            {
                field: 'NombreInflexibilidad',
                displayName: "NOMBRE DE LA INFLEXIBILIDAD",
                headerCellClass: 'ui-grid-cell-header',
                width: "30%",
            },
            {
                field: 'Periodo',
                displayName: "PERÍODO",
                width: "20%",
                headerCellClass: 'ui-grid-cell-header ui-grid-cell-align-center',
                cellClass: 'ui-grid-cell-align-center'
            },
            {
                field: 'ValorTotal',
                displayName: "VALOR TOTAL",
                width: "15%",
                headerCellClass: 'ui-grid-cell-header ui-grid-cell-align-center',
                cellClass: 'ui-grid-cell-align-center'
            },
            {
                field: 'ValorPagado',
                displayName: "VALOR PAGADO",
                width: "15%",
                headerCellClass: 'ui-grid-cell-header ui-grid-cell-align-center',
                cellClass: 'ui-grid-cell-align-center'
            },
            {
                field: 'Estado',
                displayName: "ESTADO",
                width: "9%",
                headerCellClass: 'ui-grid-cell-header ui-grid-cell-align-center',
                cellClass: 'ui-grid-cell-align-center',
                cellTemplate: cellTemplateEstado,
            },
            {
                field: 'Accion',
                displayName: 'Acción',
                enableFiltering: false,
                enableHiding: false,
                enableSorting: false,
                enableColumnMenu: false,
                cellTemplate: accionesInflexibilidades,
                width: '9%',
                headerCellClass: 'ui-grid-cell-header ui-grid-cell-align-center',
                cellClass: 'ui-grid-cell-align-center',
            }
        ];

        vm.todasColumnasDefinicion = Object.assign([], columnDef);
        vm.columnasDisponiblesPorAgregar = Object.assign([], vm.columnasDisponiblesPorAgregar);

        vm.columnas = columnDef.map(x => x.displayName);

        function agregarColumnas() {
            let lista = vm.gridOptions;
            var colAcciones;
            var addCol;

            for (var j = 0, lenCol = vm.columnas.length; j < lenCol; j++) {
                var col = vm.columnas[j];

                if (col !== "Acción" && lista.columnDefs.map(x => x.displayName).indexOf(col) == -1) {
                    addCol = vm.todasColumnasDefinicion.filter(x => x.displayName == col)[0]
                    colAcciones = vm.todasColumnasDefinicion.filter(x => x.field == 'Accion')[0]

                    lista.columnDefs.pop();
                    lista.columnDefs.push(addCol);
                    lista.columnDefs.push(colAcciones);
                }
            }

        }

        function borrarColumnas() {
            let lista = vm.gridOptions.columnDefs;
            for (var i = 0, len = lista.length; i < len; i++) {
                var inflex = lista[i];

                for (var j = 0, lenCol = vm.columnasDisponiblesPorAgregar.length; j < lenCol; j++) {
                    var indexEliminar = -1;
                    var col = vm.columnasDisponiblesPorAgregar[j];

                    indexEliminar = vm.gridOptions.columnDefs.map(x => x.displayName).indexOf(col);

                    if (indexEliminar >= 0) vm.gridOptions.columnDefs.splice(indexEliminar, 1);
                }

                //vm.listaTramites[i] = tramite;
            }
        }


        function configurarColumnas() {
            // vuelve a las columnas originales primero
            borrarColumnas();
            // agrega nuevas columnas en todas las filas del modelo
            agregarColumnas();
        }

        vm.gridOptions = {
            enableColumnMenus: false,
            paginationPageSizes: [5, 10, 15, 25, 50, 100],
            paginationPageSize: 10,
            columnDefs: columnDef,
            data: [],
            onRegisterApi: onRegisterApi
        }

        vm.tipoFiltro = constantesTipoFiltro.inflexibilidades;

        function downloadPdf() {
            let colField = [];
            let colDisplay = [];
            vm.gridOptions.columnDefs.forEach(item => {
                if (vm.columnas.indexOf(item.displayName) != -1 && item.displayName !== "Acción") {
                    colField.push(item.field);
                    colDisplay.push(item.displayName);
                }
            });

            let objExport = vm.gridOptions.data
                .map((x) => {
                    return {
                        nombreInflexibilidad: columnDef.find(x => x.field == 'NombreInflexibilidad') != null ? x.NombreInflexibilidad : null,
                        valorTotal: columnDef.find(x => x.field == 'ValorTotal') != null ? x.ValorTotal : null,
                        periodoExcel: columnDef.find(x => x.field == 'Periodo') != null ? x.Periodo : null,
                        valorPagado: columnDef.find(x => x.field == 'ValorPagado') != null ? x.ValorPagado : null,
                        estado: columnDef.find(x => x.field == 'Estado') != null ? x.Estado : null,
                        columnasHeader: colField,
                        columnas: colDisplay
                    }
                });
            inflexibilidadServicio.obtenerPdf(objExport).then(function (retorno) {
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

        /// Actions
        function downloadExcel() {

            let colField = [];
            let colDisplay = [];
            vm.gridOptions.columnDefs.forEach(item => {
                if (vm.columnas.indexOf(item.displayName) != -1 && item.displayName !== "Acción") {
                    colField.push(item.field);
                    colDisplay.push(item.displayName);
                }
            });

            let objExport = vm.gridOptions.data
                .map((x) => {
                    return {
                        nombreInflexibilidad: columnDef.find(x => x.field == 'NombreInflexibilidad') != null ? x.NombreInflexibilidad : null,
                        valorTotal: columnDef.find(x => x.field == 'ValorTotal') != null ? x.ValorTotal : null,
                        periodoExcel: columnDef.find(x => x.field == 'Periodo') != null ? x.Periodo : null,
                        valorPagado: columnDef.find(x => x.field == 'ValorPagado') != null ? x.ValorPagado : null,
                        estado: columnDef.find(x => x.field == 'Estado') != null ? x.Estado : null,
                        columnasHeader: colField,
                        columnas: colDisplay
                    }
                });
            inflexibilidadServicio.obtenerExcel(objExport).then(function (retorno) {
                var blob = new Blob([retorno.data], {
                    type: "application/octet-stream"
                });
                FileSaver.saveAs(blob, "Inflexibilidad.xls");
            }, function (error) {
                console.log(error.data);
            });
        }

        function tieneColumna(columna) {
            return vm.columnas.find(x => x == columna) != null;
        }

        vm.filtro = {
            nombreInflexibilidad: null,
            valorTotal: null,
            valorPagado: null,
            estado: null,
            anioInicio: null,
            anioFin: null
        };

        function conmutadorFiltro() {
            limpiarCamposFiltro();
            vm.mostrarFiltro = !vm.mostrarFiltro;
        }

        function limpiarCamposFiltro() {
            for (const prop in vm.filtro) {
                vm.filtro[prop] = null;
            }
        }

        async function init() {
            buscarColumnasLocalStorage();

            vm.entidad.Id = $routeParams.id;
            const { params } = $location.search();
            if (params && vm.entidad.Id) {
                vm.entidad.NombreEntidad = params;
                await _listarInflexibilidades(vm.entidad.Id);
            }

        }

        function onRegisterApi(gridApi) {
            $scope.gridApi = gridApi;
        }

        async function _listarInflexibilidades(entidadId) {
            return inflexibilidadServicio.obtenerInflexibilidadPorEntidadId(entidadId, vm.filtro).then(exito, error);

            function exito(respuesta) {
                if (respuesta && respuesta.data) {
                    vm.gridOptions.data = respuesta.data;
                    vm.listaInflexibilidades = respuesta.data;

                    configurarColumnas();
                }
            }

            function error(respuesta) {
                vm.gridOptions.data = [];
                toast.error("Hubo un error  al cargar las inflexibilidades da entidad")
            }
        }

        async function buscar() {
            await _listarInflexibilidades(vm.entidad.Id);
        }

        function confirmarEliminar(id) {
            swal({
                title: "",
                text: "Confirma la exclusión del registro?",
                type: "error",
                closeOnConfirm: true,
                html: true,
                showCancelButton: true,
            }, (isConfirm) => {
                if (isConfirm) {
                    eliminar(id);
                }
            });
        }

        function obtenerClassEstado(estado) {
            const estilos = {
                "Activo": "background-estado activo",
                'Finalizada': "background-estado inactivo"
            }

            return estilos[estado];
        }

        function abrirModalAdicionarColumnas() {

            const modalInstance = $uibModal.open({
                animation: $scope.animationsEnabled,
                templateUrl: 'src/app/configurarColumnas/plantillaConfigurarColumnas.html',
                controller: 'controladorConfigurarColumnas',
                controllerAs: "vm",
                size: 'lg',
                resolve: {
                    items: function () {
                        return {
                            columnasActivas: vm.columnas,
                            columnasDisponibles: vm.columnasDisponiblesPorAgregar,
                        };
                    }
                }
            });

            modalInstance.result.then(function (selectedItem) {
                if (!$localStorage.tipoFiltro) {
                    $localStorage.tipoFiltro = {
                        'inflexibilidades': {
                            'columnasActivas': selectedItem.columnasActivas,
                            'columnasDisponibles': selectedItem.columnasDisponibles
                        }
                    };
                } else {
                    $localStorage.tipoFiltro['inflexibilidades'] = {
                        'columnasActivas': selectedItem.columnasActivas,
                        'columnasDisponibles': selectedItem.columnasDisponibles
                    }
                }

                buscarColumnasLocalStorage();
                _listarInflexibilidades(vm.entidad.Id);
            }, function () {
                $log.info('Modal dismissed at: ' + new Date());
            });
        };

        function buscarColumnasLocalStorage() {
            if (!$localStorage.tipoFiltro)
                return;

            if ($localStorage.tipoFiltro["inflexibilidades"]) {
                vm.columnas = $localStorage.tipoFiltro["inflexibilidades"].columnasActivas;
                vm.columnasDisponiblesPorAgregar = $localStorage.tipoFiltro["inflexibilidades"].columnasDisponibles;
            }


        }

        function volver() {
            history.back();
        }

        function agregarNuevaInflexibilidad() {
            vm.inflexibilidad = {};
            //   console.log(row);
            vm.inflexibilidad.titulo = 'AGREGAR INFLEXIBILIDAD';
            vm.inflexibilidad.idEntidad = vm.entidad.Id;

            modalCrearEditar();

        }

        function editar(row) {
            vm.inflexibilidad = {};
           // console.log(row);
            vm.inflexibilidad.titulo = 'EDITAR INFLEXIBILIDAD';
            vm.inflexibilidad.idEntidad = vm.entidad.Id;
            vm.inflexibilidad.nombreInflexibilidad = row.NombreInflexibilidad;
            vm.inflexibilidad.tipoInflexibilidad = row.TipoInflexibilidad.toString();
            vm.inflexibilidad.valorTotal = row.ValorTotal;
            vm.inflexibilidad.fechaInicio = new Date(row.FechaInicio);
            vm.inflexibilidad.fechaFin = new Date(row.FechaFin);
            vm.inflexibilidad.estado = row.Estado
            vm.inflexibilidad.id = row.Id

            modalCrearEditar();
        }

        function modalCrearEditar() {
            $uibModal.open({
                templateUrl: 'src/app/entidades/entidades/tipoEntidad/entidad-sgr/modales/inflexibilidad/modalCrearEditarInflexibilidad.html',
                controller: 'modalCrearEditarInflexibilidadController',
                controllerAs: "vm",
                resolve: {
                    inflexibilidad: vm.inflexibilidad
                }
            }).result.then(function (result) {
                _listarInflexibilidades(vm.entidad.Id);
            }, function (reason) {
                _listarInflexibilidades(vm.entidad.Id);
            });
        }

        //TODO: Implement method
        function eliminar(row) {
            inflexibilidadServicio.eliminarInflexibilidad(row.Id)
                .then(function (response) {
                    if (response.data.Exito) {
                        utilidades.mensajeSuccess("Operación realizada con éxito!", false, false, false);
                        _listarInflexibilidades(vm.entidad.Id);
                    } else
                        utilidades.mensajeError(response.data.Mensaje, false);

                   // console.log(response.data);
                });
        }

        function verPagamentos(row) {
            $uibModal.open({
                animation: $scope.animationsEnabled,
                templateUrl: 'src/app/entidades/entidades/tipoEntidad/entidad-sgr/modales/consultarPagos/consultarPagosModal.html',
                controller: 'consultarPagosModalController',
                controllerAs: "vm",
                size: 'lg',
                openedClass: "entidad-modal-consultar-pagos",
                resolve: {
                    inflexibilidad: {
                        id: row.Id,
                        nombreInflexibilidad: row.NombreInflexibilidad
                    }
                }
            }).result.then(function (result) {
                _listarInflexibilidades(vm.entidad.Id);
            }, function (reason) {
                _listarInflexibilidades(vm.entidad.Id);
            });
        }
    };
})();