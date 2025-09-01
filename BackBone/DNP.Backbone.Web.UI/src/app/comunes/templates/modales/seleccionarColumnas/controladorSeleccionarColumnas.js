(function () {
    'use strict';


    controladorSeleccionarColumnas.$inject = ['$scope', '$uibModalInstance', 'items', '$localStorage', 'constantesTipoFiltro'];

    function controladorSeleccionarColumnas($scope, $uibModalInstance, items, $localStorage, constantesTipoFiltro) {
        var vm = this;
        vm.columnasSelecionadas = [];
        vm.items = angular.copy(items);
        vm.columnaSeleccionada = null;
        vm.ladoSeleccionado = null;
        vm.seleccionarColumna = function (columna, lado) {
            vm.columnaSeleccionada = columna;
            vm.ladoSeleccionado = lado;
        }

        $scope.ok = function () {
            $uibModalInstance.close(vm.items.campos);
        };

        $scope.cancel = function () {
            $uibModalInstance.dismiss('cancel');
        };

    }

    angular.module('backbone').controller('controladorSeleccionarColumnas', controladorSeleccionarColumnas);
})();


