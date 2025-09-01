(function () {
    'use strict';

    centroAyudaController.$inject = [
        '$scope',
        '$rootScope',
        'utilidades',
        '$uibModal'
    ];

    function centroAyudaController(
        $scope,
        $rootScope,
        utilidades,
        $uibModal
    ) {

        var vm = this;
        vm.init = init;
        vm.datos = [];
        vm.tipoTab = 'Centro de Ayuda';
       
       }

    // ReSharper disable once UndeclaredGlobalVariableUsing
    angular.module('backbone').controller('bancosController', bancosController);
})();
