(function () {
    'use strict';


    controladorPanelEjecucionDeAccion.$inject = ['$scope', 'servicioPanelEjecucionDeAccion', '$routeParams'];

    function controladorPanelEjecucionDeAccion($scope, servicioPanelEjecucionDeAccion, $routeParams) {
        var vm = this;
        vm.bpin = $routeParams.bpin;
        vm.estado = $routeParams.estado;
     
        vm.cancelarEjecucionDeAccion = function () {
            window.location.href = "/";
        }

        vm.guardarConfiguracionDeEstado = function () {
            servicioPanelEjecucionDeAccion.actualizarEstadoDelProyecto(vm.bpin, vm.estado).then(function () {
                console.log("Exito");
            },
            function () {
                console.log("fail");
            });
            window.location.href = "/";
        }
    }

    angular.module('backbone').controller('controladorPanelEjecucionDeAccion', controladorPanelEjecucionDeAccion);
})();