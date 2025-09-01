(function () {
    'use strict';

    modal2EjemploController.$inject = [
        'objProyecto',
        '$uibModalInstance',
        'utilidades',
        'servicioConsolaProyectos',
        'FileSaver',
    ];

    function modal2EjemploController(
        objProyecto,
        $uibModalInstance,
        utilidades,
        servicioConsolaProyectos,
        FileSaver
    ) {
        var vm = this;



        vm.listaDocumentos;
        vm.gridOptions;
        vm.MostarTabla = "1";
        vm.CambiarTabla = function () {
            if (vm.MostarTabla == '1') {
                vm.MostarTabla = "2";
            } else {
                vm.MostarTabla = "1";
            }
        }
        vm.cerrar = $uibModalInstance.dismiss;


        /// Comienzo
        vm.init = function () {

        }


    }

    angular.module('backbone').controller('modal2EjemploController', modal2EjemploController);
})();