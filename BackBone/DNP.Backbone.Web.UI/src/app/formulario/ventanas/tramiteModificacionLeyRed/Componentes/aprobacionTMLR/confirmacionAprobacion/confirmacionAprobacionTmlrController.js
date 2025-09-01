(function () {
    'use strict';

    confirmacionAprobacionTmlrController.$inject = [
        '$sessionStorage',
        'archivoServicios',
        '$scope',
        'servicioAcciones',
        'trasladosServicio',
        'constantesCondicionFiltro',
        'sesionServicios',
        '$routeParams',
        'constantesBackbone',
        'utilidades'
    ];

    function confirmacionAprobacionTmlrController(
        $sessionStorage,
        archivoServicios,
        $scope,
        servicioAcciones,
        trasladosServicio,
        constantesCondicionFiltro,
        sesionServicios,
        $routeParams,
        constantesBackbone,
        utilidades
    ) {
        var vm = this;
        vm.lang = "es";



    }

    angular.module('backbone').component('confirmacionAprobacionTmlr', {
        templateUrl: "src/app/formulario/ventanas/tramiteModificacionLeyRed/Componentes/aprobacionTmlr/confirmacionAprobacion/confirmacionAprobacionTmlr.html",
        controller: confirmacionAprobacionTmlrController,
        controllerAs: "vm",
        bindings: {
            disabled: '=',
            callback: '&',
            notificacionvalidacion: '&',
            notificacionestado: '&',
            guardadocomponent: '&',
            guardadoevent: '&',
            tipotramiteid: '@',
            tramiteid: '@',
            nivel: '@',
            rol: '@',
            section: '@',
            nombrecomponentepaso: '@',
            deshabilitar: '@',
            rolanalista: '@',
        }
    });

})();