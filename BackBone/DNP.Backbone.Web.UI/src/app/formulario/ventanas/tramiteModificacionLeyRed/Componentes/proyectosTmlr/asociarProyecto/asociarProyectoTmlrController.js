(function () {
    'use strict';

    asociarProyectoTmlrController.$inject = [
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

    function asociarProyectoTmlrController(
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

    angular.module('backbone').component('asociarProyectoTmlr', {
        templateUrl: "src/app/formulario/ventanas/tramiteModificacionLeyRed/Componentes/proyectosTmlr/asociarProyecto/asociarProyectoTmlr.html",
        controller: asociarProyectoTmlrController,
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