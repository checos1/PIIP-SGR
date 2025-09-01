(function () {
    'use strict';


    validarAutorizacionController.$inject = ['$scope', 'serviciosComponenteNotificaciones', '$routeParams', 'backboneServicios', '$localStorage', '$interval', 'servicioPanelPrincipal', '$timeout', 'constantesBackbone'];

    function validarAutorizacionController($scope, serviciosComponenteNotificaciones, $routeParams, backboneServicios, $localStorage, $interval, servicioPanelPrincipal, $timeout, constantesBackbone) {
        var vm = this;
        vm.init = function () {
            if (usuarioDNP != '') {
                window.location.href = "/inicio";
            }
        }
    }

    angular.module('backbone').controller('validarAutorizacionController', validarAutorizacionController);
})();