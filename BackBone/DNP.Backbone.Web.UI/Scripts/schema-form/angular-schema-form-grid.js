angular.module("backbone").config(["schemaFormProvider",
    "schemaFormDecoratorsProvider", "sfPathProvider", "constantesFormularios",
    function (schemaFormProvider, schemaFormDecoratorsProvider, sfPathProvider, constantesFormularios) {


        var grid = function (name, schema, options) {
            if ((schema.type === 'grid') && schema.format == 'table') {
                var f = schemaFormProvider.stdFormObj(name, schema, options);
                f.key = options.path;
                f.type = 'grid';
                options.lookup[sfPathProvider.stringify(options.path)] = f;

                return f;
            }
        };

        schemaFormProvider.defaults.string.unshift(grid);

        schemaFormDecoratorsProvider.addMapping(
            'bootstrapDecorator',
            'grid',
            '/Scripts/schema-form/directives/grid.html'
        );
    }
]);

angular.module('schemaForm').controller("gridController", ["$scope", "$sce", '$route', '$http', '$timeout', '$uibModal', "uiGridConstants", "appSettings", "$filter", "utilidades", 'uiGridTreeBaseService', 'CacheServicios',
    function ($scope, $sce, $route, $http, $timeout, $uibModal, uiGridConstants, appSettings, $filter, utilidades, uiGridTreeBaseService, CacheServicios) {

        var vm = this;

        vm.tieneCabecera = false;
        vm.getCabecera = getCabecera;
        vm.getJierarquiaPreguntas = getJierarquiaPreguntas;
        vm.isPreguntasGenerales = isPreguntasGenerales;
        vm.obtenerNombreGrilla = obtenerNombreGrilla;
        $scope.form.showTreeExpandNoChildren = false;
        vm.isPreguntas = isPreguntas;
        if ($scope.form.data) {
            if ($scope.form.originalData) {
                $scope.form.data = angular.copy($scope.form.originalData);
                $scope.form.columnDefs = angular.copy($scope.form.originalColumnDefs);
            }
            else {
                $scope.form.originalData = angular.copy($scope.form.data);
                $scope.form.originalColumnDefs = angular.copy($scope.form.columnDefs);
            }

            vm.cabeceras = $scope.form.data = vm.getCabecera($scope.form.data, $scope.form.columnDefs);

            if (vm.tieneCabecera && $scope.form.columnDefs) {
                var columnDefs = [];
                $scope.form.columnDefs.forEach(function (detalle) {
                    detalle.visible = (detalle.cabecera || detalle.detalle);
                    columnDefs.push(detalle)
                })
                $scope.form.columnDefs = columnDefs;
            }
        }


        $scope.form.enableFiltering = false;
        $scope.mostrarDivPaginacion = true;
        vm.mostrarFiltro = false;
        vm.filtroHabilitado = $scope.form.habilitarFiltrado;
        vm.columnsDefsVisibles = [];
        vm.insertarRegistro = false;
        vm.templateNuevoRegistroEnLinea = '';
        vm.columnaAcciones = {};
        vm.grid = $scope.form;
        vm.subGrids = [];
        vm.subGridsApis = [];
        vm.deshabilitarAccionesColumnaAcciones = false;
        vm.filasFiltradas = $scope.form.data ? $scope.form.data.length : 0;
        vm.filtroCambio = false;
        vm.opciones = {
            deshabilitarControles: false
        };

        vm.editarFila = editarFila;
        vm.disabledColumnResizing = disabledColumnResizing;
        vm.confirmarEliminarFila = confirmarEliminarFila;
        vm.noHayRegistros = noHayRegistros;
        vm.validarBoleano = validarBoleano;
        vm.nuevoRegistro = nuevoRegistro;
        vm.actualizarTotalizacion = actualizarTotalizacion;
        vm.deshabilitarBotones = deshabilitarBotones;
        vm.conmutadorFiltro = conmutadorFiltro;
        vm.abrirModalAdicionarColumnas = abrirModalAdicionarColumnas;
        vm.montarFiltros = montarFiltros;
        vm.buscar = buscar;
        vm.limpiarCamposFiltro = limpiarCamposFiltro;
        vm.camposGrillaVisibles = [];
        vm.columnasComFiltro = [];
        vm.getCache = getCache;
        vm.getTiposCatalogo = getTiposCatalogo;

        vm.abrirModalPreguntas = abrirModalPreguntas;
        vm.abrirModalNuevosItens = abrirModalNuevosItens;
        vm.nuevosItensModalInstance = {};
        vm.preguntasModalInstance = {};
        vm.habilitarEdicion = habilitarEdicion;
        vm.obtenerOcultarExportar = obtenerOcultarExportar;

        $scope.$watch('gridApi.grid.columns[0]', function (temColumnas) {
            if (temColumnas) {
                if (!vm.isPreguntas()) {
                    ajustarVisibilidadColumnasDeLaGrilla();
                    ajustaTamanhoColumnas();
                }
                montarFiltros();

                return $timeout(function () {
                    applyRedis($scope.form.data, $scope.form.columnDefs)
                }, 2000);
            }

        });

        function obtenerOcultarExportar() {
            return true;
        }

        function loadPreguntas() {
            if (vm.isPreguntas()) {
                if ($scope.form.originalData)
                    $scope.form.data = angular.copy($scope.form.originalData);
                else
                    $scope.form.originalData = angular.copy($scope.form.data);

                $scope.form.data = vm.getJierarquiaPreguntas($scope.form.data, $scope.form.columnDefs,
                    vm.isPreguntasGenerales() ? 'Tematica' : ['Sector', 'Clasificacion']);
            }
        }

        function isPreguntasGenerales() {
            return $scope.form.key.includes('PreguntasGenerales');
        }

        function habilitarEdicion() {
            return $scope.form.habilitarEdicion && !($scope.form.key.includes('PreguntasGenerales') || $scope.form.key.includes('PreguntasEspecificas'));
        }

        function isPreguntas() {
            return ($scope.form.key.includes('PreguntasGenerales') || $scope.form.key.includes('PreguntasEspecificas'));
        }

        function obtenerNombreGrilla() {
            return $scope.form.key;
        }

        function applyRedis(datos, cols) {
            if (!datos) return datos;
            datos.forEach(function (item) {
                cols.forEach(function (col) {
                    if (col.rCache && !isNaN(item[col.field])) {
                        item[col.field + "RCacheOriginalValue"] = item[col.field]
                        item[col.field] = getCache(item[col.field], col)
                    }
                })
            })
        }

        function getCache(id, item) {
            var dato = {};
            dato = (vm.listaRCache || []).find(p => Object.keys(p) == item.rCache);
            var list = dato ? dato[item.rCache] : [];


            return (list.find(p => p.Id == id) || {}).Name
        }

        function ajustaTamanhoColumnas() {
            vm.grid.columnDefs.forEach(x => x.width = '*');
        }

        function ajustarColumnaAcciones() {
            if ((vm.grid.habilitarEdicion == true && vm.grid.tipoEdicion === 'boton' || vm.grid.habilitarEliminacion) && !($scope.form.key.includes('PreguntasGenerales') || $scope.form.key.includes('PreguntasEspecificas'))) {
                let columnaAcciones = $scope.gridApi.grid.columns.filter(c => c.name === "ColumnaAcciones")[0];
                columnaAcciones.minWidth = '200';
                columnaAcciones.maxWidth = '200';
                columnaAcciones.width = '200';
                columnaAcciones.drawnWidth = '200';
                let columnaAccionesGrid = vm.grid.columnDefs.find(x => x.name === "ColumnaAcciones");
                columnaAccionesGrid.minWidth = '200';
                columnaAccionesGrid.maxWidth = '200';
                columnaAccionesGrid.width = '200';
                columnaAccionesGrid.drawnWidth = '200';
                columnaAcciones.showColumn();
            }
        }

        function obtenerCamposDeLaGrilla(cols) {
            vm.camposGrillaVisibles = cols
                .filter((column, index, self) => { return column.visible === true && self.indexOf(self.find(x => x.name === column.name)) === index })
                .map(x => {
                    let column = {};
                    column.displayName = x.displayName;
                    column.visible = x.visible;
                    column.name = x.name;
                    return column;
                });
        }


        function obterListaRCache() {
            var catalogos = [];
            $scope.form.columnDefs.forEach(function (item) {
                if (item.rCache && !(catalogos.filter(p => Object.keys(p) == item.rCache).length > 0)) {
                    CacheServicios.obtenerCatalogo(item.rCache).then(function (datos) {
                        if (datos) {
                            catalogos.push({ [item.rCache]: datos });
                            vm.listaRCache = catalogos;
                        }

                    });
                }
            });
        }


        function getCache(id, item) {
            var dato = {};
            dato = (vm.listaRCache || []).find(p => Object.keys(p) == item.rCache);
            var list = dato ? dato[item.rCache] : [];


            return (list.find(p => p.Id == id) || {}).Name
        }


        function getTiposCatalogo(id, item, model) {
            return vm.listaRCache ? (vm.listaRCache[0] ? vm.listaRCache[0]["Catalagos"][item.rCache] : []) : [];
        }

        function ajustarVisibilidadColumnasDeLaGrilla() {
            vm.grid.columnDefs.forEach(x => x.visible = false);
            $scope.gridApi.grid.columns.forEach(x => x.hideColumn());


            vm.camposGrillaVisibles.forEach((x, index) => {

                if (index < 5)
                    $scope.gridApi.grid.columns.filter(c => c.name === x.name)[0].showColumn();
                else
                    x.visible = false;
            });

            ajustarColumnaAcciones()

            $scope.gridApi.core.notifyDataChange(uiGridConstants.dataChange.COLUMN);
        }

        function alterarColumnasDeLaGrid(columnas) {
            let grids = [];

            if (isPreguntas())
                grids = vm.subGrids;
            else
                grids.push(vm.grid);


            vm.camposGrillaVisibles = columnas;

            grids.forEach(x => {
                x.columnDefs.forEach(c => {
                    let campo = vm.camposGrillaVisibles.find(cgv => c.name === cgv.name);
                    if (campo)
                        c.visible = campo.visible;
                })

            })


            //vm.camposGrillaVisibles.forEach((x, index) => {
            //    if (x.visible === true || x.name === "ColumnaAcciones")
            //        $scope.gridApi.grid.columns.filter(c => c.displayName === x.displayName)[0].showColumn();
            //    else
            //        $scope.gridApi.grid.columns.filter(c => c.displayName === x.displayName)[0].hideColumn();
            //});


            //$scope.gridApi.grid.columns.filter(column => { return column.visible === true }).forEach(x => x.enableFiltering = false);
            //vm.grid.enableFiltering = false;

            vm.subGridsApis.forEach(x => {
                x.core.notifyDataChange(uiGridConstants.dataChange.ALL);
                x.core.refresh();
            })

            $scope.gridApi.core.notifyDataChange(uiGridConstants.dataChange.ALL);
            $scope.gridApi.core.refresh();
            montarFiltros();
        }

        function buscar() {

            let gridsApis = [];

            if (isPreguntas())
                grids = vm.subGridsApis;
            else
                grids.push($scope.gridApi);


            vm.columnasComFiltro.forEach(x => {
                let input = document.getElementById(`${$scope.form.key}_${x.name}_filter`)
                grids.forEach(g => {
                    g.grid.columns.forEach(c => {
                        if (`${$scope.form.key}_${c.name}_filter` === input.id) {
                            c.filters[0].term = input.value || '*';
                        }
                    })
                })
            });


            grids.forEach(x => x.grid.options.enableFiltering = true)
            grids.forEach(x => x.grid.refresh());
        }

        function limpiarCamposFiltro() {

            let gridsApis = [];

            if (isPreguntas())
                grids = vm.subGridsApis;
            else
                grids.push($scope.gridApi);

            grids.forEach(g => g.grid.columns.forEach(c => {
                c.filters[0].term = "";
            }));

            vm.columnasComFiltro.forEach(x => {
                let input = document.getElementById(`${$scope.form.key}_${x.name}_filter`)
                input.value = "";
            });

            grids.forEach(x => x.grid.options.enableFiltering = true)
            grids.forEach(x => x.grid.refresh());
        }

        function conmutadorFiltro() {
            vm.mostrarFiltro = !vm.mostrarFiltro;
        }

        function montarFiltros() {
            vm.columnasComFiltro = vm.camposGrillaVisibles
                .filter(column => { return column.visible === true })
                .map(x => {
                    let column = {};
                    column.displayName = x.displayName;
                    column.visible = x.visible;
                    column.name = x.name;
                    return column;
                });

            if (vm.isPreguntas())
                vm.subGridsApis.forEach(x => x.columns.filter(column => { return column.visible === true }).forEach(x => x.enableFiltering = false));
            else
                $scope.gridApi.grid.columns.filter(column => { return column.visible === true }).forEach(x => x.enableFiltering = false);

            if (vm.columnasComFiltro.length > 0) {
                if (vm.isPreguntas()) {
                    vm.subGrids.forEach(x => x.enableFiltering = true);
                    vm.subGridsApis.forEach(x => x.grid.options.enableFiltering = true);
                }
                else {
                    vm.grid.enableFiltering = true;
                    $scope.gridApi.grid.options.enableFiltering = true;
                }
            }
        }

        function abrirModalAdicionarColumnas() {

            var modalInstance = $uibModal.open({
                animation: $scope.animationsEnabled,
                templateUrl: 'src/app/comunes/templates/modales/seleccionarColumnas/seleccionarColumnas.html',
                controller: 'controladorSeleccionarColumnas',
                controllerAs: "vm",
                size: 'lg',
                resolve: {
                    items: function () {
                        return {
                            'campos': vm.camposGrillaVisibles
                        };
                    }
                }
            });

            modalInstance.result.then(function (selectedItem) {

                alterarColumnasDeLaGrid(selectedItem);
            });
        };

        function agregarItem(item) {
            console.log(item)
        }

        function abrirModalNuevosItens(datos) {
            var campos = $scope.form.columnDefs
            vm.nuevosItensModalInstance = $uibModal.open({
                animation: $scope.animationsEnabled,
                templateUrl: 'src/app/comunes/templates/modales/adicionarNuevosItens/adicionarNuevosItens.html',
                controller: 'controladorAdicionarNuevosItens',
                controllerAs: "vm",
                size: 'lg',
                resolve: {
                    items: function () {
                        return {
                            'agregarItem': function (item) {
                                console.log(item);
                            },
                            'campos': campos,
                            datos: datos
                        };
                    }
                }
            });

            vm.nuevosItensModalInstance.result.then(function (obj) {
                if (obj.datos) {
                    var dst = angular.copy(vm.filaEnEdicion);
                    var mergedObject = angular.extend(dst, obj.nuevosItem)
                    $scope.form.data[$scope.form.data.indexOf(vm.filaEnEdicion)] = mergedObject;
                }
                else
                    $scope.form.data.push(obj.nuevosItem)

                //$scope.gridApi.core.notifyDataChange(uiGridConstants.dataChange.ALL);
                $scope.gridApi.core.refresh();
                //$scope.$applyAsync();
            });
        }


        function abrirModalPreguntas() {
            var campos = $scope.form.columnDefs

            vm.preguntasModalInstance = $uibModal.open({
                animation: $scope.animationsEnabled,
                templateUrl: 'src/app/comunes/templates/modales/agregarPreguntas/agregarPreguntas.html',
                controller: 'controladorAgregarPreguntas',
                controllerAs: "vm",
                size: 'lg',
                resolve: {
                    items: function () {
                        return {
                            'campos': campos,
                            'data': $scope.form.agregarPreguntasRequisitos,
                            'selecionados': vm.selecionados || []
                        };
                    }
                }
            });

            vm.preguntasModalInstance.result.then(function (obj) {
                for (var i = 0; i < obj.preguntas.length; i++) {
                    var p = obj.preguntas[i];
                    var item = JSON.parse(JSON.stringify($scope.form.originalData[0]));//angular.copy($scope.form.originalData[0]);
                    item.Pregunta = p.Pregunta;
                    item.Acuerdo = p.Acuerdo;
                    item.Clasificacion = p.Clasificacion;
                    item.Sector = p.Sector;
                    item.IdPregunta = p.IdPregunta;
                    item.OpcionesRespuestas = p.OpcionesRespuestas;
                    item.ObservacionPregunta = p.ObservacionPregunta;
                    if (!$scope.form.originalData.find(o => o.IdPregunta === p.IdPregunta))
                        $scope.form.originalData.push(item)

                    $scope.form.originalData.AgregarRequisitos = true;
                }
                vm.selecionados = obj.selecionados
                for (var i = 0; i < obj.removerPreguntas.length; i++) {
                    var item = obj.removerPreguntas[i];
                    $scope.form.originalData = $scope.form.originalData.filter(p => p.IdPregunta !== item.IdPregunta)
                }
                loadPreguntas();

                $scope.gridApi.core.refresh();
            });
        }

        this.$onInit = function () {
            vm.opciones.deshabilitarControles = $scope.form.deshabilitarControles ? $scope.form.deshabilitarControles : false;
            //vm.opciones.deshabilitarControles = true;
            if (vm.isPreguntas()) loadPreguntas();

            configurarEdicionTabla();
            let type = vm.isPreguntasGenerales() ? 'Tematica' : ['Sector', 'Clasificacion'];
            obtenerCamposDeLaGrilla($scope.form.originalColumnDefs.filter(x => x.field !== type && x.visible === true));
            if (vm.isPreguntas()) {
                $scope.form.data.forEach(x => verificarCamposListas(x.subGridOptions.columnDefs))
            }
            else
                verificarCamposListas($scope.form.columnDefs);

            obterListaRCache($http);
            $timeout(function () {
                applyRedis($scope.form.data, $scope.form.columnDefs)
            }, 2000);
        };

        function constructor() {
            configurarFormatoNumeros();
            habilitarTotalizacion();
            habilitarPaginacion();
            habilitarControlesCheckBox();
            habilitarControlerFecha();
            $scope.form.rowTemplate = 'src/app/formulario/template/filaEditablePrevisualizar.html';
            $scope.form.headerTemplate = 'src/app/formulario/template/header-template.html';
            if ($scope.form.columnDefs)
                $scope.form.columnDefs.forEach(c => {
                    if (!c.displayName === 'Acciones')
                        c.cellTemplate = 'src/app/formulario/template/cellTemplate.html';
                });
        }

        $scope.expandAll = function () {
            $scope.gridApi.treeBase.expandAllRows();
        };

        $scope.toggleRow = function (row, evt) {
            uiGridTreeBaseService.toggleRowTreeState($scope.gridApi.grid, row, evt);
        };

        $scope.toggleExpandNoChildren = function () {
            $scope.gridOptions.showTreeExpandNoChildren = !$scope.gridOptions.showTreeExpandNoChildren;
            $scope.gridApi.grid.refresh();
        };

        $scope.form.onRegisterApi = function (gridApi) {
            //set gridApi on scope
            $scope.gridApi = gridApi;
            if ($scope.form.habilitarEdicion) {
                gridApi.edit.on.afterCellEdit($scope, function (rowEntity, colDef, newValue, oldValue) {
                    $scope.$apply();
                });
            }

            gridApi.core.on.filterChanged($scope, function () {
                vm.filtroCambio = true;
            });

            gridApi.core.on.rowsVisibleChanged($scope, function () {
                var filaEnEdicion = _.find($scope.gridApi.grid.rows, function (o) { return !o.visible && o.entity.editable; });
                if (filaEnEdicion)
                    filaEnEdicion.visible = true;

                if (!vm.filtroCambio) {
                    return;
                }

                vm.filtroCambio = false;
                var filtered = _.filter($scope.gridApi.grid.rows, function (o) { return o.visible; });
                vm.filasFiltradas = filtered.length;
            });

            gridApi.draggableRows.on.rowDragged($scope, function (info, rowElement) {
                console.log("Dragged", info);
            });

            $scope.gridApi.treeBase.on.rowExpanded($scope, function (row) {
                console.log(row)
            });
        };

        function verificarCamposListas(cols) {
            cols.filter(function (columna) {
                if (columna.field === 'ObservacionPregunta') {
                    columna.cellTemplate = `<div><input type="text" id="{{rowRenderIndex}}" ng-model="row.entity.${columna.field}"></div>`;
                    columna.allowCellFocus = false;
                }
                if (columna.esLista) {

                    var guidNombre = utilidades.generarGuid();
                    var deshabilitarcontroles = !columna.enableCellEdit; //enableCellEdit es modificada en configurarEdicionTabla
                    columna.enableCellEdit = false;
                    if (vm.isPreguntas())
                        deshabilitarcontroles = false;
                    var templateLista = '';
                    templateLista += '<div class="radio radiofix radio-primary">';
                    templateLista += '<label ng-repeat="data in row.entity.' + columna.field + ' track by $index" class="radio-inline" >';
                    templateLista += '      <input tabindex="-1" ng-disabled="' + deshabilitarcontroles + '" ng-model="row.entity.' + columna.field + 'Seleccionado" type="radio" name="opcion_{{row.entity.IdPregunta}}" value="{{data[\'' + (columna.clave || (columna.lista || [null])[0]) + '\']}}" /> ';
                    templateLista += '      <span class="circle"></span><span class="check"></span>{{ data["' + (columna.valor || (columna.lista || [null, null])[1]) + '"] }}';
                    templateLista += '</label> ';
                    templateLista += '</div> ';
                    columna.cellTemplate = templateLista;

                    templateLista = '';
                    templateLista += '<div uigridlistaeditable class="radio radiofix radio-primary">';
                    templateLista += '<label ng-repeat="data in row.entity.' + columna.field + ' track by $index" class="radio-inline" >';
                    templateLista += '      <input ng-disabled="' + deshabilitarcontroles + '" ng-model="row.entity.' + columna.field + 'Seleccionado" type="radio" name="opcionesEditables_{{row.entity.IdPregunta}}" value="{{data[\'' + (columna.clave || (columna.lista || [null])[0]) + '\']}}" /> ';
                    templateLista += '      <span class="circle"></span><span class="check"></span>{{ data["' + (columna.valor || (columna.lista || [null, null])[1]) + '"] }}';
                    templateLista += '</label> ';
                    templateLista += '</div> ';
                    columna.editableCellTemplate = templateLista;
                }
            });
        }

        function configurarFormatoNumeros() {
            for (var i = 0; i < $scope.form.columnDefs.length; i++) {
                var columna = $scope.form.columnDefs[i];
                if (columna.type && columna.type === 'number' && columna.cellFilter !== 'number : 2') {
                    columna.cellFilter = 'number : 2';
                }
            }
        }

        function habilitarControlesCheckBox() {
            var plantillaCheckbox = '';
            for (var i = 0; i < $scope.form.columnDefs.length; i++) {
                var columna = $scope.form.columnDefs[i];
                if (columna.originalType == undefined) {
                    if (columna.type === "boolean") {
                        plantillaCheckbox = '<div class="text-center"><input type="checkbox" disabled="true" ng-model="row.entity.' + columna.field + '">{{vm.habilitarEdicion}}</div>';
                    }
                }
                else {
                    if (columna.originalType === "boolean") {
                        if ($scope.form.habilitarEdicion) {
                            if ($scope.form.tipoEdicion === "tabla" && columna.habilitarEdicion) {
                                if (columna.type === "string") {
                                    columna.editableCellTemplate = '<input type="text" id="{{rowRenderIndex}}" minlength="4" maxlength="5" ng-model="row.entity.' + columna.field + '" onblur="">';
                                    plantillaCheckbox = '<input type="text" ng-model="row.entity.' + columna.field + '">{{vm.habilitarEdicion}}';
                                } else {
                                    plantillaCheckbox = '<div class="text-center"><input type="checkbox" ng-model="row.entity.' + columna.field + '">{{vm.habilitarEdicion}}</div>';
                                }
                            }
                            else {
                                if (columna.type === 'string') {
                                    plantillaCheckbox = '<input type="text" disabled="true" ng-model="row.entity.' + columna.field + '">{{vm.habilitarEdicion}}';
                                } else {
                                    plantillaCheckbox = '<div class="text-center"><input type="checkbox" disabled="true" ng-model="row.entity.' + columna.field + '">{{vm.habilitarEdicion}}</div>';
                                }
                            }
                        }
                        else {
                            if (columna.type === 'string') {
                                plantillaCheckbox = '<input type="text" disabled="true" ng-model="row.entity.' + columna.field + '">{{vm.habilitarEdicion}}';
                            } else {
                                plantillaCheckbox = '<div class="text-center"><input type="checkbox" disabled="true" ng-model="row.entity.' + columna.field + '">{{vm.habilitarEdicion}}</div>';
                            }
                        }
                        columna.cellTemplate = plantillaCheckbox;
                    }
                }
            }
        }

        function habilitarControlerFecha() {
            for (var i = 0; i < $scope.form.columnDefs.length; i++) {
                var columna = $scope.form.columnDefs[i];
                if (columna.originalType === "date-time") {
                    columna.editableCellTemplate =
                        '<div class= "dropdown dropdown{{rowRenderIndex}}">' +
                        '<a class="dropdown-toggle" id="dropdown{{rowRenderIndex}}" role="button" data-toggle="dropdown" data-target=".dropdown{{rowRenderIndex}}">' +
                        '<div class="input-group date">' +
                        '<input type="text" class="form-control no-border-right" ng-model="row.entity.' + columna.field + '" date-time-input="D/M/YYYY hh:mm:ss" name="{{rowRenderIndex}}" id="{{rowRenderIndex}}" /> ' +
                        '<span class="input-group-addon"><i class="icon-calendar"></i></span>' +
                        '</div>' +
                        '</a>' +
                        '<ul class="dropdown-menu" role="menu" aria-labelledby="dLabel">' +
                        '<datetimepicker data-ng-model="row.entity.' + columna.field + '" data-datetimepicker-config="{ dropdownSelector: \'#dropdown\' + rowRenderIndex }" />' +
                        '</ul>' +
                        '</div>';
                    if ($scope.form.habilitarEdicion) {

                        if ($scope.form.tipoEdicion === "tabla" && columna.habilitarEdicion) {
                            if (columna.type === "string") {
                                columna.editableCellTemplate = '<input type="text" class="form-control no-border-right" ng-model="row.entity.' + columna.field + '" date-time-input="D/M/YYYY hh:mm:ss" name="{{rowRenderIndex}}" id="{{rowRenderIndex}}" /> ';
                            }
                            columna.cellTemplate = '<input type="text" class="form-control no-border-right" ng-model="row.entity.' + columna.field + '" date-time-input="D/M/YYYY hh:mm:ss" name="{{rowRenderIndex}}" id="{{rowRenderIndex}}" /> ';
                        } else {
                            columna.editableCellTemplate = '';
                            columna.cellTemplate = '<input type="text" disabled class="form-control no-border-right" ng-model="row.entity.' + columna.field + '" date-time-input="D/M/YYYY hh:mm:ss" name="{{rowRenderIndex}}" id="{{rowRenderIndex}}" /> ';
                        }
                    }
                    else {
                        columna.cellTemplate = '<input type="text" disabled class="form-control no-border-right" ng-model="row.entity.' + columna.field + '" date-time-input="D/M/YYYY hh:mm:ss" name="{{rowRenderIndex}}" id="{{rowRenderIndex}}" /> ';
                    }
                }
            }
        }

        function configurarEdicionTabla() {
            var habilitarEdicionTabla = false;

            if (vm.opciones.deshabilitarControles) {
                habilitarEdicionTabla = false;
                vm.deshabilitarAccionesColumnaAcciones = true;
            } else {
                habilitarEdicionTabla = $scope.form.habilitarEdicion; //Usar configuracion de diseno de formulario
                vm.deshabilitarAccionesColumnaAcciones = false;
            }


            if ($scope.form.tipoEdicion === "tabla") {
                for (var posicion = 0; posicion < $scope.form.columnDefs.length; posicion++) {
                    $scope.form.columnDefs[posicion].enableCellEdit = ($scope.form.columnDefs[posicion].habilitarEdicion === true && habilitarEdicionTabla);

                    if ($scope.form.columnDefs[posicion].esLista) {
                        $scope.form.columnDefs[posicion].enableCellEditOnFocus = ($scope.form.columnDefs[posicion].habilitarEdicion === true && habilitarEdicionTabla);;
                    }
                }
            }
            else if ($scope.form.tipoEdicion === "boton") {
                for (var posicion = 0; posicion < $scope.form.columnDefs.length; posicion++) {
                    if ($scope.form.columnDefs[posicion].esLista) {
                        $scope.form.columnDefs[posicion].enableCellEditOnFocus = ($scope.form.columnDefs[posicion].habilitarEdicion === true && habilitarEdicionTabla);
                        $scope.form.columnDefs[posicion].enableCellEdit = ($scope.form.columnDefs[posicion].habilitarEdicion === true && habilitarEdicionTabla);
                    } else {
                        $scope.form.columnDefs[posicion].enableCellEdit = false;
                    }
                    //$scope.form.columnDefs[posicion].enableColumnResizing = false;
                }
            }
        }

        function disabledColumnResizing(entity) {
            for (var colVisible = 0; colVisible < entity.columnasVisibles.length; colVisible++) {
                entity.columnasVisibles[colVisible].enableColumnResizing = false;
            }
        }

        function deshabilitarBotones(estado) {
            vm.deshabilitarAccionesColumnaAcciones = estado;
            vm.deshabilitadoPorAdicion = estado;
        }

        function editarFila(entity) {
            //columnsDefsVisibles();
            //entity.editable = true;
            //entity.editarFila = true;
            //entity.inline = false;
            //deshabilitarBotones(true);
            vm.abrirModalNuevosItens(entity)
            entity.columnasVisibles = vm.columnsDefsVisibles;
            vm.filaEnEdicion = entity;
            $scope.gridApi.core.notifyDataChange(uiGridConstants.dataChange.EDIT);
            $scope.gridApi.core.notifyDataChange(uiGridConstants.dataChange.COLUMN);
        }

        function actualizarTotalizacion() {
            $scope.gridApi.core.notifyDataChange(uiGridConstants.dataChange.EDIT);
            $scope.gridApi.core.notifyDataChange(uiGridConstants.dataChange.COLUMN);
        }

        function columnsDefsVisibles() {
            vm.columnsDefsVisibles = [];
            for (var posicion = 0; posicion < $scope.form.columnDefs.length; posicion++) {
                if ($scope.form.columnDefs[posicion].displayName !== "Acciones" && $scope.form.columnDefs[posicion].visible) {
                    vm.columnsDefsVisibles.push($scope.form.columnDefs[posicion]);
                }
            }
        }

        function verSiTieneValorEnLaPropiedad(objeto, nombrePropiedad) {
            var keys = [];
            for (var key in objeto) {
                if (key === nombrePropiedad) {
                    if (objeto.hasOwnProperty(key)) keys.push(key);
                }
            }
            for (var i = 0; i < keys.length; i++) {
                var propiedad = objeto[keys[i]];
                if (propiedad !== null && propiedad !== undefined && propiedad !== "")
                    return true;
            }
            return false;
        }

        function habilitarTotalizacion() {
            if ($scope.form.habilitarTotalizacion) {
                $scope.form.showColumnFooter = true;

                for (var i = 0; i < $scope.form.columnDefs.length; i++) {

                    var columnDef = $scope.form.columnDefs[i];
                    if (columnDef.visible && columnDef.tipoCalculo === 'SUM') {
                        columnDef.aggregationType = uiGridConstants.aggregationTypes.sum;
                        columnDef.footerCellTemplate = obtenerTemplateTotalizar(columnDef.type, columnDef.tipoCalculo);
                    } else if (columnDef.visible && columnDef.tipoCalculo === 'AVG') {
                        columnDef.aggregationType = uiGridConstants.aggregationTypes.avg;
                        columnDef.footerCellTemplate = obtenerTemplateTotalizar(columnDef.type, columnDef.tipoCalculo);
                    } else if (columnDef.visible && columnDef.tipoCalculo === 'COUNT') {
                        columnDef.aggregationType = function (filas, columnDef) {
                            var contador = 0;
                            for (var j = 0; j < filas.length; j++) {
                                var entidad = filas[j].entity;
                                if (verSiTieneValorEnLaPropiedad(entidad, columnDef.field)) {
                                    contador++;
                                }
                            }
                            return "Conteo: " + contador.toString();
                        };
                    }
                }
            }

            function obtenerTemplateTotalizar(tipoColumna, tipoCalculo) {
                var templateBase = '<div class="ui-grid-cell-contents" >TIPO_CALCULO: {{col.getAggregationValue() FILTROS}}</div>';

                var filtroNumeros = '| number : CANTIDAD_DECIMALES';

                var template = templateBase;

                switch (tipoCalculo) {
                    case 'SUM':
                        template = template.replace("TIPO_CALCULO", "Suma"); break;
                    case 'AVG':
                        template = template.replace("TIPO_CALCULO", "Promedio"); break;
                    case 'COUNT':
                        template = template.replace("TIPO_CALCULO", "Conteo"); break;
                    default:
                        template = template.replace("TIPO_CALCULO", tipoCalculo); break;
                }

                switch (tipoColumna) {
                    case 'number':
                        template = template.replace("FILTROS", filtroNumeros);
                        template = template.replace("CANTIDAD_DECIMALES", "2");
                        break;
                    case 'integer':
                        template = template.replace("FILTROS", filtroNumeros);
                        template = template.replace("CANTIDAD_DECIMALES", "0");
                        break;
                    default:
                        template = template.replace("FILTROS", "");
                        break;
                }

                return template;
            }
        }

        function habilitarPaginacion(parameters) {
            //if ($scope.form.habilitarPaginacion) {
            $scope.mostrarDivPaginacion = true;
            $scope.form.showGridFooter = true;
            $scope.form.gridFooterTemplate = 'src/app/comunes/templates/gridFooterTemplate.html';
            $scope.form.useExternalPagination = false;
            $scope.form.useExternalSorting = false;

            $scope.form.paginationPageSizes = [5, 10, 25]
            $scope.form.enablePaginationControls = true;

            $scope.form.numeroPorPagina = appSettings.topePaginacionConsultaAplicaciones;
            $scope.form.paginationPageSize = parseInt($scope.form.numeroPorPagina);
            $scope.form.totalRegistros = $scope.form.data.length;

            $scope.form.paginationCurrentPage = 1;
            $scope.form.onRegisterApi = function (gridApi) {
                $scope.gridApi = gridApi;
                //gridApi.core.on.filterChanged($scope, function () {

                //});
                gridApi.core.on.rowsRendered($scope, function () {
                    $scope.form.totalDeItems = $scope.form.totalItems;
                    vm.totalRegistros = $scope.form.data.length;
                    vm.filasFiltradas = $scope.gridApi.core.getVisibleRows($scope.gridApi.grid).length;
                    var seleccionadas = _.filter($scope.gridApi.core.getVisibleRows($scope.gridApi.grid), function (o) { return o.entity.Seleccionado; });
                    var datosFiltrados = $scope.gridApi.core.getVisibleRows($scope.gridApi.grid);;

                    $scope.filasFiltradasRegistros = [];
                    _.each(datosFiltrados, function (data) {
                        $scope.filasFiltradasRegistros.push(data.entity);

                    });

                    _.each($scope.gridApi.grid.rows, function (value) {
                        value.entity.Seleccionado = false;

                        var lista = _.filter(seleccionadas, function (o) { return o.entity.Id === value.entity.Id });
                        var previas = _.filter(vm.listaCategoriasPorAplicacion, function (o) { return o.Id === value.entity.Id });
                        if (lista.length > 0 || previas.length > 0) {
                            value.entity.Seleccionado = true;
                        }
                    });


                });
            }
            //}
        }

        function noHayRegistros() {
            return $scope.form.totalItems < 1;
        }

        function confirmarEliminarFila(fila) {
            $scope.filaAEliminar = fila;
            utilidades.mensajeWarning(
                $filter('language')('ConfirmarEliminarRegistro'),
                function () {
                    $scope.form.data.splice($scope.form.data.indexOf($scope.filaAEliminar), 1);
                    $scope.$apply();
                    var filtered = _.filter($scope.gridApi.grid.rows, function (o) { return o.visible; });
                    vm.filasFiltradas = filtered.length;
                },
                function () { },
                $filter('language')('Continuar')
            );
        }

        function validarBoleano(id) {
            var texto = document.getElementById(id);
            if (texto !== "true" || texto !== "false") {
                return false;
            }
        }

        function nuevoRegistro() {
            vm.filasFiltradas = !vm.filasFiltradas || vm.filasFiltradas === 0 ? 1 : vm.filasFiltradas; //Para que aparescan los campos de adicion
            $scope.form.data.unshift({});
            deshabilitarBotones(true);
            $scope.form.data[0].editable = true;
            $scope.form.data[0].inline = true;
            $scope.$applyAsync();
        }

        function getCabecera(datos, cols) {
            if (!datos || !tieneCabecera(cols)) return datos;
            var isCabecera = false;
            var hashs = [];
            var result = [];
            var cabeceras = [];
            var detalles = [];
            datos.forEach(function (item) {
                var hash = "";
                cols.forEach(function (col) {
                    if (col.cabecera) {
                        isCabecera = true;
                        hash += item[col.field]
                    }
                })

                var detalle = Object.assign({}, item);
                detalle.cabeceraHash = hash;
                detalle.$$treeLevel = 1;
                detalle.items = null;
                detalles.push(detalle)

                if (!hashs.includes(hash)) {
                    hashs.push(hash)
                    item.cabeceraHash = hash;
                    item.isCabecera = true;
                    item.$$treeLevel = 0;
                    item.items = null;
                    cabeceras.push(item);
                }
            })

            if (!isCabecera)
                return datos;
            vm.tieneCabecera = true;
            cabeceras.forEach(function (item) {
                cols.forEach(function (col) {
                    if (!col.cabecera) {
                        item[col.field] = null;
                    }
                })
                result.push(item);
                //$scope.gridOptions.data.push(item);
                const items = detalles.filter(function (detalle) {
                    return detalle.cabeceraHash === item.cabeceraHash;
                })
                items.forEach(function (detalle) {
                    cols.forEach(function (col) {
                        if (col.cabecera) {
                            detalle[col.field] = null;
                        }
                    })
                    result.push(detalle);
                })
            })

            return result;
        }

        function getJierarquiaPreguntas(datos, cols, types) {
            var result = [];
            var type = Array.isArray(types) && types.length > 0 ? types[0] : types;

            $scope.form.columnDefs = [
                {
                    name: $scope.form.title,
                    width: '100%',
                    field: type,
                    visible: true,
                }
            ];

            $scope.form.originalColumnDefs = $scope.form.originalColumnDefs.filter(x => x.name !== "ColumnaAcciones");

            $scope.form.originalColumnDefs.forEach(x => {
                x.width = '220';
            });
            let colPregunta = $scope.form.originalColumnDefs.find(x => x.field === 'Pregunta');
            if (colPregunta) {
                colPregunta = angular.copy($scope.form.originalColumnDefs.find(x => x.field === 'Pregunta'));
                $scope.form.originalColumnDefs = $scope.form.originalColumnDefs.filter(x => x.field !== 'Pregunta');
                colPregunta.width = '30%';
                $scope.form.originalColumnDefs.unshift(colPregunta);
            }

            let colExplicacion = $scope.form.originalColumnDefs.find(x => x.field === 'Explicacion');
            if (colExplicacion) {
                colExplicacion.width = '30%';
            }



            $scope.form.onRegisterApi = function (gridApi) {
                $scope.gridApi = gridApi;
                gridApi.expandable.on.rowExpandedStateChanged($scope, function (row) {
                    if (row.isExpanded) {
                        console.log('expanded')
                    }
                });
            }


            $scope.form.expandableRowTemplate = '<div ui-grid-cellnav class="subgrid" ui-grid-edit ui-grid="row.entity.subGridOptions"></div>';

            datos.forEach(x => {
                let tipo = angular.copy(x[type])
                delete x[type];


                if (result.find(x => x[type] === tipo)) {
                    result.find(x => x[type] === tipo).subGridOptions.data.push(x);
                }
                else {
                    let d = [];
                    d.push(x);
                    let subgrid = {
                        data: d,
                        columnDefs: $scope.form.originalColumnDefs.filter(x => x.field !== type),
                    };

                    subgrid.onRegisterApi = function (gridApi) {
                        vm.subGridsApis.push(gridApi);
                    }

                    vm.subGrids.push(subgrid)

                    result.push({
                        [type]: tipo,
                        subGridOptions: subgrid
                    })
                }
            });

            return result;
        }

        function tieneCabecera(cols) {
            if (!cols) return false;
            var tiene = false;
            cols.forEach(function (col) {
                if (col.cabecera)
                    tiene = true;
            })
            return tiene;
        }

        vm.verLog = function () {
            $uibModal.open({
                animation: $scope.animationsEnabled,
                templateUrl: 'src/app/panelPrincial/modales/logs/logsModal.html',
                controller: 'logsModalController',
                controllerAs: "vm",
                size: 'lg',
                openedClass: "entidad-modal-adherencia",
                resolve: {
                    IdInstancia: () => null
                },
            });
        }

        constructor();
    }
]);

angular.module('schemaForm').directive('uigridlistaeditable', ['gridUtil', 'uiGridConstants', 'uiGridEditConstants', '$timeout', 'uiGridEditService',
    function (gridUtil, uiGridConstants, uiGridEditConstants, $timeout, uiGridEditService) {
        return {
            scope: true,
            require: ['?^uiGrid', '?^uiGridRenderContainer'],
            compile: function () {
                return {
                    pre: function ($scope, $elm, $attrs) {

                    },
                    post: function ($scope, $elm, $attrs, controllers) {
                        var uiGridCtrl, renderContainerCtrl, ngModel;
                        if (controllers[0]) { uiGridCtrl = controllers[0]; }
                        if (controllers[1]) { renderContainerCtrl = controllers[1]; }
                        if (controllers[2]) { ngModel = controllers[2]; }

                        $scope.deepEdit = false;

                        // set focus at start of edit
                        $scope.$on(uiGridEditConstants.events.BEGIN_CELL_EDIT, function (data, evt) {
                            // must be in a timeout since it requires a new digest cycle
                            $scope.deepEdit = true;
                            $timeout(function () {
                                $scope.deepEdit = true;
                                var elements = angular.element($elm[0]).find("input");

                                var selectedElement = undefined;
                                for (var i = 0; i < elements.length; i++) {
                                    if (elements[i].checked)
                                        selectedElement = elements[i];
                                }

                                if (selectedElement !== undefined)
                                    selectedElement.focus();
                                else
                                    elements[0].focus();

                                $scope.$applyAsync(function () {
                                    $scope.grid.disableScrolling = true;
                                });

                            });

                            if (evt !== undefined && evt.type === 'click') {
                                //Si es por click, entonces dejar de editar
                                $timeout(function () {
                                    $scope.stopEdit(evt);
                                }, 200);
                            }
                        });

                        $scope.stopEdit = function (evt) {
                            if ($scope.inputForm && !$scope.inputForm.$valid) {
                                evt.stopPropagation();
                                $scope.$emit(uiGridEditConstants.events.CANCEL_CELL_EDIT);
                            }
                            else {
                                try {
                                    $timeout(function () {
                                        $scope.$emit(uiGridEditConstants.events.END_CELL_EDIT);
                                    });
                                } catch (e) {
                                    console.log(e);
                                }
                            }
                            $scope.deepEdit = false;
                        };

                        $elm.on('keydown', function (evt) {
                            switch (evt.keyCode) {
                                case uiGridConstants.keymap.ESC:
                                    evt.stopPropagation();
                                    $scope.$emit(uiGridEditConstants.events.CANCEL_CELL_EDIT);
                                    break;
                            }

                            if ($scope.deepEdit &&
                                (evt.keyCode === uiGridConstants.keymap.LEFT ||
                                    evt.keyCode === uiGridConstants.keymap.RIGHT ||
                                    evt.keyCode === uiGridConstants.keymap.UP ||
                                    evt.keyCode === uiGridConstants.keymap.DOWN)) {
                                evt.stopPropagation();
                            }
                            // Pass the keydown event off to the cellNav service, if it exists
                            else if (uiGridCtrl && uiGridCtrl.grid.api.cellNav) {
                                evt.uiGridTargetRenderContainerId = renderContainerCtrl.containerId;
                                if (uiGridCtrl.cellNav.handleKeyDown(evt) !== null) {
                                    $scope.stopEdit(evt);
                                }
                            }
                            else {
                                // handle enter and tab for editing not using cellNav
                                switch (evt.keyCode) {
                                    case uiGridConstants.keymap.ENTER: // Enter (Leave Field)
                                    case uiGridConstants.keymap.TAB:
                                        evt.stopPropagation();
                                        evt.preventDefault();
                                        $scope.stopEdit(evt);
                                        break;
                                }
                            }

                            return true;
                        })

                        $scope.$on('$destroy', function unbindEvents() {
                            // unbind all jquery events in order to avoid memory leaks
                            $elm.off();
                        });
                    }
                };
            }
        };
    }]);

angular.module('schemaForm').directive('navegarBotonesCelda', ['gridUtil', 'uiGridConstants', 'uiGridCellNavConstants', '$timeout', 'uiGridEditService',
    function (gridUtil, uiGridConstants, uiGridCellNavConstants, $timeout, uiGridEditService) {
        return {
            scope: true,
            require: ['?^uiGrid', '?^uiGridRenderContainer'],
            compile: function () {
                return {
                    pre: function ($scope, $elm, $attrs) {

                    },
                    post: function ($scope, $elm, $attrs, controllers) {
                        var uiGridCtrl, renderContainerCtrl, ngModel;
                        if (controllers[0]) { uiGridCtrl = controllers[0]; }
                        if (controllers[1]) { renderContainerCtrl = controllers[1]; }
                        if (controllers[2]) { ngModel = controllers[2]; }
                        var botonEnfocado = undefined;
                        // set focus at start of edit
                        $scope.$on(uiGridCellNavConstants.CELL_NAV_EVENT, seleccionarPrimerBoton);

                        function seleccionarPrimerBoton(data, navigatedColumn, arg, evt) {
                            if (evt && evt.type && evt.type === "keydown") {
                                var isFocused = $scope.grid.cellNav.focusedCells.some(function (focusedRowCol, index) {
                                    return (focusedRowCol.row === $scope.row && focusedRowCol.col === $scope.col);
                                });

                                if (isFocused) {
                                    var elements = angular.element($elm[0]).find("button");
                                    $timeout(function () {
                                        botonEnfocado = elements[0];
                                        elements[0].focus();
                                    });
                                }
                            }
                        }

                        $elm.on('keydown', function (evt) {
                            if ((evt.keyCode === uiGridConstants.keymap.LEFT ||
                                evt.keyCode === uiGridConstants.keymap.RIGHT ||
                                evt.keyCode === uiGridConstants.keymap.UP ||
                                evt.keyCode === uiGridConstants.keymap.DOWN ||
                                evt.keyCode === uiGridConstants.keymap.ENTER
                            )) {
                                evt.stopPropagation();

                                var elements = angular.element($elm[0]).find("button");

                                switch (evt.keyCode) {
                                    case uiGridConstants.keymap.LEFT:
                                        if (botonEnfocado === elements[0]) {
                                            botonEnfocado = elements[elements.length - 1];
                                        } else {
                                            var indice = elements.index(botonEnfocado);
                                            botonEnfocado = elements[elements.length - indice - 1];
                                        }
                                        break;
                                    case uiGridConstants.keymap.RIGHT:
                                        if (botonEnfocado === elements[elements.length - 1]) {
                                            botonEnfocado = elements[0];
                                        } else {
                                            var indice = elements.index(botonEnfocado);
                                            botonEnfocado = elements[indice + 1];
                                        }
                                        break;
                                    case uiGridConstants.keymap.ENTER: // Enter (Leave Field)
                                        botonEnfocado.click();
                                        break;
                                }
                                botonEnfocado.focus();
                            }
                            // Pass the keydown event off to the cellNav service, if it exists
                            else if (uiGridCtrl && uiGridCtrl.grid.api.cellNav) {
                                evt.uiGridTargetRenderContainerId = renderContainerCtrl.containerId;
                                if (uiGridCtrl.cellNav.handleKeyDown(evt) !== null) {
                                    if (botonEnfocado)
                                        botonEnfocado.blur();
                                }
                            }
                            else {
                                // handle enter and tab for editing not using cellNav
                                switch (evt.keyCode) {
                                    //case uiGridConstants.keymap.ENTER:
                                    case uiGridConstants.keymap.TAB:
                                        evt.stopPropagation();
                                        evt.preventDefault();
                                        //$scope.stopEdit(evt);
                                        break;
                                }
                            }

                            return true;
                        });

                        $scope.$on('$destroy', function unbindEvents() {
                            // unbind all jquery events in order to avoid memory leaks
                            $elm.off();
                        });
                    }
                };
            }
        };
    }]);
