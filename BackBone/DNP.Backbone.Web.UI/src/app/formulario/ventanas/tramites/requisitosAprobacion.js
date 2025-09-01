(function () {
    'use strict';

    requisitosAprobacionController.$inject = ['$scope', 'backboneServicios', '$sessionStorage', '$timeout', '$location'];

    function requisitosAprobacionController(
        $scope,
        backboneServicios,
        $sessionStorage,
        $timeout,
        $location
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
        vm.retornar = retornar;
        vm.tabcdpEsActivo = tabcdpEsActivo;
        $sessionStorage.tabcdpactivo = false;
        vm.director = $sessionStorage.director ;

        vm.ocultarRequisitosTramite = false;
        vm.ocultarRequisitosProyecto = false;
        vm.TabActived = true;

        vm.inicializar = function () {
            if ($sessionStorage.accionEjecutandose === 'Ver requisitos proyecto') {
                vm.ocultarRequisitosTramite = true;
                vm.ocultarRequisitosProyecto = false;
                vm.TabActived = false;
            }
            else
                if ($sessionStorage.accionEjecutandose === 'Ver requisitos tramite') {
                    vm.ocultarRequisitosTramite = false;
                    vm.ocultarRequisitosProyecto = true;
                    vm.TabActived = true;
                }
        };
        function retornar() {
            vm.desactivartraslados();
        }

        function tabcdpEsActivo() {
            vm.tabcdpactivo = true;
            $sessionStorage.tabcdpactivo = vm.tabcdpactivo;
        }

    }

    //angular.module('backbone').controller('requisitosAprobacionController', requisitosAprobacionController);

    angular.module('backbone').component('requisitosAprobacion', {
        templateUrl: "src/app/formulario/ventanas/tramites/requisitosAprobacion.html",
        controller: requisitosAprobacionController,
        controllerAs: "vm",
        bindings: {
            desactivartraslados: '&'
        }
    });

})();