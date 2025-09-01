(function () {
    'use strict';

    solicitudModificacionTmlrController.$inject = [
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

    function solicitudModificacionTmlrController(
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

    angular.module('backbone').component('solicitudModificacionTmlr', {
        templateUrl: "src/app/formulario/ventanas/tramiteModificacionLeyRed/Componentes/informacionPresupuestalTmlr/solicitudModificacion/solicitudModificacionTmlr.html",
        controller: solicitudModificacionTmlrController,
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