(function () {
    'use strict';

    requerimientosTramitesController.$inject = ['$scope'];

    function requerimientosTramitesController(
        $scope
    ) {
        var vm = this;
        vm.Bpin = $sessionStorage.BPIN;
        vm.TipoProyecto = $sessionStorage.TipoProyecto;
        vm.EntidadId = $sessionStorage.EntidadId;
        vm.TipoTramiteId = $sessionStorage.TipoTramiteId;
        vm.NombreProyecto = $sessionStorage.NombreProyecto;
        vm.numeroTramite = $sessionStorage.numeroTramite;
        vm.nombreEntidad = $sessionStorage.nombreEntidad;
        vm.nombreTipoTramite = $sessionStorage.nombreTipoTramite;              
    }

    angular.module('backbone').component('requerimientosTramite', {
        templateUrl: "src/app/formulario/ventanas/tramites/requerimientosTramites/requerimientosTramites.html",
        controller: requerimientosTramitesController,
        controllerAs: "vm",
        bindings: {
            disabled: '='
        }
    });

})();