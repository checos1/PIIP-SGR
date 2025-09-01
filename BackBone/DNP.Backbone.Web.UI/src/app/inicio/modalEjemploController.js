(function () {
    'use strict';

    modalEjemploController.$inject = [
        'objProyecto',
        '$uibModalInstance',
        'utilidades',
        'servicioConsolaProyectos',
        'FileSaver',
    ];

    function modalEjemploController(
        objProyecto,
        $uibModalInstance,
        utilidades,
        servicioConsolaProyectos,
        FileSaver
    ) {
        var vm = this;


        vm.tiposUsuario = [
            { id: 1, name: 'Usuario DNP' },
            { id: 2, name: 'Usuario externo' }
        ];
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

    angular.module('backbone').controller('modalEjemploController', modalEjemploController);
})();