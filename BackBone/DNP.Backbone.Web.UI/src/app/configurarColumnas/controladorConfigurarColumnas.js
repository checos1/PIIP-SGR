(function () {
    'use strict';


    controladorConfigurarColumnas.$inject = ['$scope', '$uibModalInstance', 'items', '$localStorage', 'constantesTipoFiltro'];

    function controladorConfigurarColumnas($scope, $uibModalInstance, items, $localStorage, constantesTipoFiltro) {
        var vm = this;
        vm.items = angular.copy(items);
        vm.columnaSeleccionada = null;
        vm.ladoSeleccionado = null;
        vm.seleccionarColumna = function (columna, lado) {
            vm.columnaSeleccionada = columna;
            vm.ladoSeleccionado = lado;
        }

        vm.moverDeIzquierdaADerecha = function () {
            if (!vm.columnaSeleccionada || !vm.ladoSeleccionado) return;
            if (vm.ladoSeleccionado === 'izquierda') {
                var index = vm.items.columnasActivas.indexOf(vm.columnaSeleccionada);
                vm.items.columnasDisponibles.push(vm.columnaSeleccionada);
                vm.columnaSeleccionada = null;
                vm.ladoSeleccionado = undefined;
                if (index > -1) {
                    vm.items.columnasActivas.splice(index, 1);
                }
            } else {
                vm.columnaSeleccionada = null;
                vm.ladoSeleccionado = undefined;
            }

        }
            
            vm.moverDeDerechaAIzquierda = function () {
                if (!vm.columnaSeleccionada || !vm.ladoSeleccionado) return;
                if (vm.ladoSeleccionado === 'derecha') {
                    var index =  vm.items.columnasDisponibles.indexOf(vm.columnaSeleccionada);
                    vm.items.columnasActivas.push(vm.columnaSeleccionada);
                    vm.columnaSeleccionada = null;
                    vm.ladoSeleccionado = undefined;
                    if (index > -1) {
                        vm.items.columnasDisponibles.splice(index, 1);
                    }
                } else {
                    vm.columnaSeleccionada = null;
                    vm.ladoSeleccionado = undefined;
                }
            }
        $scope.ok = function () {
            $uibModalInstance.close(vm.items);
        };

        $scope.cancel = function () {
            $uibModalInstance.dismiss('cancel');
        };

    }

    angular.module('backbone').controller('controladorConfigurarColumnas', controladorConfigurarColumnas);
})();


