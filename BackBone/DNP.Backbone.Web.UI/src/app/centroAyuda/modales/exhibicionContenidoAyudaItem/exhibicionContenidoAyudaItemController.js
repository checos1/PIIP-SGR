(function () {
    'use strict';

    exhibicionContenidoAyudaItemController.$inject = [
        '$scope',
        '$sce',
        'params',
        '$uibModalInstance',
        'AyudaTemaListaItemModel',
        'AyudaTipoConstante'
    ];

    function exhibicionContenidoAyudaItemController(
        $scope,
        $sce,
        params,
        $uibModalInstance,
        AyudaTemaListaItemModel,
        AyudaTipoConstante
    ) {
        var vm = this;
        vm.AyudaTipoConstante = AyudaTipoConstante;
        vm.params = params;
        vm.model = {};

        vm.init = function() {
            vm.model = new AyudaTemaListaItemModel(vm.params.AyudaTema);
        }

        vm.cerrar = function () {
            $uibModalInstance.dismiss('cerrar');
        }
    }

    angular.module('backbone.usuarios').controller('exhibicionContenidoAyudaItemController', exhibicionContenidoAyudaItemController);
})();