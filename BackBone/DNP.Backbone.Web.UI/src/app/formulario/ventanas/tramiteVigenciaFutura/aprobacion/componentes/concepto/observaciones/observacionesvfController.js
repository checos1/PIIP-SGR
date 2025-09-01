(function () {
    'use strict';

    observacionesvfController.$inject = [
        '$sessionStorage',
        'archivoServicios',
        '$scope',
        'servicioAcciones',
        'trasladosServicio',
        'constantesCondicionFiltro',
        'sesionServicios',
        '$routeParams',
        'constantesBackbone'
    ];

    function observacionesvfController(
        $sessionStorage,
        archivoServicios,
        $scope,
        servicioAcciones,
        trasladosServicio,
        constantesCondicionFiltro,
        sesionServicios,
        $routeParams,
        constantesBackbone
    ) {
        var vm = this;
        vm.lang = "es";
        vm.extension = "";
        vm.filename = "";

        //declarar metodos
        vm.initObservaciones = initObservaciones;



        //Metodos
        function initObservaciones()
        {
        }
    }

    angular.module('backbone').component('observacionesvf', {
        templateUrl: "src/app/formulario/ventanas/tramiteVigenciaFutura/aprobacion/componentes/concepto/observacionesvf.html",
        controller: observacionesvfController,
        controllerAs: "vm",
        bindings: {
            callback: '&',
            notificacionvalidacion: '&',
            notificacionestado: '&',
            guardadocomponent: '&'
        }
    });

})();