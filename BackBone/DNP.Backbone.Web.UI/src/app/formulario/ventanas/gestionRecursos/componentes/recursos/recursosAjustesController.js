(function () {
    'use strict';

    recursosAjustesController.$inject = [
        '$scope',
        '$sessionStorage',
        'constantesBackbone',
        'recursosAjustesServicio',
    ];



    function recursosAjustesController(
        $scope,
        $sessionStorage,
        constantesBackbone,
        recursosAjustesServicio
    ) {
        var vm = this;
        vm.lang = "es";

        //Inicio
        vm.init = function () {
            $sessionStorage.esAjuste = true;
        };
    }

    angular.module('backbone').component('recursosAjustes', {

        templateUrl: "src/app/formulario/ventanas/ajustes/componentes/recursos/recursosAjustes.html",
        controller: recursosAjustesController,
        controllerAs: "vm",
        bindings: {
        }
    });

})();