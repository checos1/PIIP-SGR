(function () {
    'use strict';

    asociarProyectoPoliticasController.$inject = [
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

    function asociarProyectoPoliticasController(
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

    angular.module('backbone').component('asociarProyectoPoliticas', {
        templateUrl: "src/app/formulario/ventanas/tramiteModificacionLeyPoliticas/Componentes/proyectosPoliticas/asociarProyecto/asociarProyectoPoliticas.html",
        controller: asociarProyectoPoliticasController,
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