(function () {
    'use strict';

    recursosSinTramiteSgpController.$inject = [
        '$scope',
        '$sessionStorage',
        'constantesBackbone',
        'recursosSinTramiteSgpServicio',
    ];



    function recursosSinTramiteSgpController(
        $scope,
        $sessionStorage,
        constantesBackbone,
        recursosSinTramiteSgpServicio
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

    angular.module('backbone').component('recursosSinTramiteSgp', {

        templateUrl: "src/app/formulario/ventanas/SGP/ajustesProyectoSGP/componentes/recursosSGP/recursosSinTramiteSgp.html",
        controller: recursosSinTramiteSgpController,
        controllerAs: "vm",
        bindings: {
            notificacionvalidacion: '&'
        }
    });

})();