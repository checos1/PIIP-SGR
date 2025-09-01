(function () {

    angular.module('backbone.formulario').controller('modificarFilaPrevisualizarController', modificarFilaPrevisualizarController);

    modificarFilaPrevisualizarController.$inject = ['$scope', '$timeout', '$route', 'archivoServicios', '$filter', 'appSettings', 'utilidades'];

    function modificarFilaPrevisualizarController($scope, $timeout, $route, archivoServicios, $filter, appSettings, utilidades) {
        var vm = this;

        vm.editarFila = true;
        vm.confirmarCancelar = confirmarCancelar;
        vm.confirmarEdicionFila = confirmarEdicionFila;
        vm.ajustarAnchoColumnas = ajustarAnchoColumnas;
        vm.habilitarResizingCol = habilitarResizingCol;
        vm.parseador = [];
        vm.manejarCambio = manejarCambio;
        vm.filaAEditarLocal = null;
        vm.parseador['string'] = "text";
        vm.parseador['boolean'] = "checkbox";
        vm.parseador['number'] = "number";
        vm.parseador['integer'] = "number";
        vm.parseador['array'] = "text";
        vm.parseador['date-time'] = "text";


        function confirmarCancelar() {
            utilidades.mensajeWarning($filter('language')('ConfirmaCancelarEdicionFilaPrevisualizar'), function () {

                if (vm.filaAEditar.inline) {
                    vm.filaAEditar.editable = false;
                    vm.filaAEditar.editarFila = false;

                    vm.grillaActualizar.options.data.splice(0, 1);
                    vm.habilitarResizingCol();
                    $scope.$apply();
                    $route.reload();

                } else {
                    vm.filaAEditar = vm.datosAntesDeCancelarFila;
                    for (var posicion = 0; posicion < vm.filaAEditar.columnasVisibles.length; posicion++) {
                        if (vm.filaAEditar.columnasVisibles[posicion].habilitarEdicion) {
                            vm.filaAEditar.columnasVisibles[posicion].habilitarEdicion = false;
                            vm.filaAEditar.columnasVisibles[posicion].enableColumnResizing = true;
                        }
                    }

                    syncronizarModelos();
                    vm.editarFila = false;
                    vm.filaAEditar.editable = false;
                    vm.datosAntesDeCancelarFila.editarFila = false;

                    for (var i = 0; i < vm.nuevaGrid.length; i++) {
                        if (vm.nuevaGrid[i].$$hashKey == vm.rowKey) {
                            vm.nuevaGrid[i].editable = false;
                            vm.nuevaGrid[i].editarFila = false;
                            if (!validarFilasHabilitadas()) {
                                for (var j = 0; j < vm.nuevaGrid[i].columnasVisibles.length; j++) {
                                    vm.nuevaGrid[i].columnasVisibles[j].enableColumnResizing = true;
                                }
                                vm.habilitarResizingCol();
                                vm.grillaActualizar.appScope.vm.actualizarTotalizacion();
                            }
                            break;
                        }
                    }
                    vm.grillaActualizar.appScope.vm.deshabilitarAccionesColumnaAcciones = false;
                  
                    $scope.$apply();

                }
            });
        }

        function validarFilasHabilitadas() {
            var exiteFilaHabilitada = false;
            if (vm.nuevaGrid != undefined) {
                for (var i = 0; i < vm.nuevaGrid.length; i++) {
                    if (vm.nuevaGrid[i].editable) {
                        exiteFilaHabilitada = true
                        break;
                    }
                }
            }
            return exiteFilaHabilitada;
        }

        function confirmarEdicionFila() {

            if (vm.filaAEditar.inline) {
                vm.filaAEditar.editable = false;
                $route.reload();

            } else {
                syncronizarModelos();
                vm.editarFila = false;
                vm.filaAEditar.editable = false;
                vm.filaAEditar.editarFila = false;
                if (!validarFilasHabilitadas()) {
                    vm.habilitarResizingCol();
                }
            }
            vm.grillaActualizar.appScope.vm.deshabilitarAccionesColumnaAcciones = false;

        }

        function findObjectByKey(array, key, value) {
            for (var i = 0; i < array.length; i++) {
                if (array[i][key] === value) {
                    return array[i];
                }
            }
            return null;
        }

        function syncronizarModelos() {
            for (var i = 0; i < vm.filaAEditar.columnasVisibles.length; i++) {
                if (obtenerTipoColumna(vm.filaAEditar.columnasVisibles[i].field) !== "boolean") {
                    if (vm.filaAEditar.columnasVisibles[i].habilitarEdicion === true) {
                        vm.filaAEditar[vm.filaAEditar.columnasVisibles[i].field] = vm.filaAEditarLocal[vm.filaAEditar.columnasVisibles[i].field];
                    }
                }
            }
        }

        function manejarCambio(nombreColumna) {
            // TO-DO refactor pendiente.
            if (obtenerTipoColumna(nombreColumna) === "boolean") {
                vm.filaAEditar[nombreColumna] = !vm.filaAEditar[nombreColumna];
            }
        }

        function obtenerTipoColumna(nombreColumna) {
            if (vm.filaAEditar.columnasVisibles) {
                for (var i = 0; i < vm.filaAEditar.columnasVisibles.length; i++) {
                    if (vm.filaAEditar.columnasVisibles[i].field === nombreColumna) {
                        return vm.filaAEditar.columnasVisibles[i].type;
                    }
                }
            }
        }

        $scope.$watch("vm.editarFila", function () {
            vm.datosAntesDeCancelarFila = angular.copy(vm.filaAEditar);
        });

        this.$onInit = function () {

            vm.filaAEditarLocal = angular.copy(vm.filaAEditar);
            vm.ajustarAnchoColumnas();
        };

        function tipoDeAtributo(obj) {
            return ({}).toString.call(obj).match(/\s([a-z|A-Z]+)/)[1].toLowerCase();
        }

        function habilitarResizingCol() {
            if (vm.filaAEditar.columnasVisibles
                && vm.filaAEditar.columnasVisibles.length > 0
                && vm.grillaActualizar.columns.length > 0) {
                for (var colVisible = 0; colVisible < vm.filaAEditar.columnasVisibles.length; colVisible++) {
                    vm.filaAEditar.columnasVisibles[colVisible].enableColumnResizing = true;
                }
            }
        };

        function ajustarAnchoColumnas() {
            if (vm.filaAEditar.columnasVisibles
                && vm.filaAEditar.columnasVisibles.length > 0
                && vm.grillaActualizar.columns.length > 0) {
                //recorre las columnas de la grilla Visibles
                for (var colVisible = 0; colVisible < vm.filaAEditar.columnasVisibles.length; colVisible++) {
                    vm.filaAEditar.columnasVisibles[colVisible].enableColumnResizing = false;
                    //recorre las columnas de la grilla 
                    for (var colGrilla = 0; colGrilla < vm.grillaActualizar.columns.length; colGrilla++) {
                        if (vm.filaAEditar.columnasVisibles[colVisible].name === vm.grillaActualizar.columns[colGrilla].name
                            && vm.filaAEditar.columnasVisibles[colVisible].field === vm.grillaActualizar.columns[colGrilla].field) {
                            vm.filaAEditar.columnasVisibles[colVisible].width = vm.grillaActualizar.columns[colGrilla].width;
                            colGrilla = vm.grillaActualizar.columns.length;
                        }
                    }
                }
            }
        };
    }
    
    angular.module('backbone.formulario')
        .component('modificarFilaPrevisualizar', {
            templateUrl: "/src/app/formulario/componentes/modificarFilaPrevisualizar/modificarFilaPrevisualizar.html",
            controller: 'modificarFilaPrevisualizarController',
            controllerAs: 'vm',
            bindings: {
                colContainer: '=',
                filaAEditar: '=',
                nuevaGrid: '=',
                grillaActualizar: '=',
                rowKey: '=',
                retornoCancelar: '&',
                retornoGuardar: '&'
            }
        });

})();