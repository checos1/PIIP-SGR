(function () {
    'use strict';

    recursosController.$inject = [
        '$scope',
        '$sessionStorage',
        'constantesBackbone',
        'recursosServicio',
    ];



    function recursosController(
        $scope,
        $sessionStorage,
        constantesBackbone,
        recursosServicio
    ) {
        var vm = this; 
        vm.lang = "es";

        //Inicio
        vm.init = function () {
            vm.notificacionvalidacion({ handler: vm.notificacionValidacionEvent, nombreComponente: vm.nombreComponente });
        };

        vm.notificacionValidacionEvent = function (errores, estado) {
            vm.notificacionestado({ nombreComponente: vm.nombreComponente, estado: estado, errores: errores });
        }
    }

    angular.module('backbone').component('recursos', {

        templateUrl: "src/app/formulario/ventanas/ajustes/componentes/recursos/recursos.html",
        controller: recursosController,
        controllerAs: "vm",
        bindings: {
            notificacionvalidacion: '&'
        }
    });

})();