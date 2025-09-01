(function () {
    'use strict';

    angular.module('backbone')
        .controller('objetivosIndicadorModalSinTramiteSgpController', objetivosIndicadorModalSinTramiteSgpController);

    objetivosIndicadorModalSinTramiteSgpController.$inject = [
        '$uibModalInstance',
        'Objetivo',
        'IdObjetivo',
        'Tipo',
        'Titulo'
    ];

    function objetivosIndicadorModalSinTramiteSgpController(
        $uibModalInstance,
        Objetivo,
        IdObjetivo,
        Tipo,
        Titulo
    ) {
        const vm = this;

        //#region Variables
        vm.Objetivo = Objetivo;
        vm.IdObjetivo = IdObjetivo;
        vm.Tipo = Tipo;
        vm.Titulo = Titulo;
        //#endregion

        //#region Metodos
        function cerrar() {
            $uibModalInstance.close();
        }

        async function init() {
        }

        vm.init = init;
        vm.cerrar = cerrar;
        //#endregion
    }
})();