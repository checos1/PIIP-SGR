(function () {
    'use strict';

    requerimientosController.$inject = ['$scope'];

    function requerimientosController(
        $scope
    ) {
        var vm = this;

    }

    angular.module('backbone').component('requerimientos', {
        templateUrl: "src/app/formulario/ventanas/tramites/requerimientosTramites/requerimientosTramites.html",
        controller: requerimientosController,
        controllerAs: "vm",
        bindings: {
            disabled: '='
        }
    });

})();