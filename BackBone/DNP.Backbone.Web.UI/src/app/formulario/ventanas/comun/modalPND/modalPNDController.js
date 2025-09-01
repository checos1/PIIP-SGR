(function () {
    'use strict';
    modalPNDController.$inject = [
        '$uibModalInstance',
        'PND'
    ];
    function modalPNDController(
        $uibModalInstance,
        PND
    ) {
        var vm = this;
        vm.cerrar = $uibModalInstance.dismiss;
        vm.PND = PND;
        vm.buscar = function () {
            
        }
        vm.init = function () {
            vm.buscar();
        }
    }
    angular.module('backbone').controller('modalPNDController', modalPNDController);
    
})();