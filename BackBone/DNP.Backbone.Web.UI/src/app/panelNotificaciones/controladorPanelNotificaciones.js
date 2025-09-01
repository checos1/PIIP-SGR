(function () {
    'use strict';


    controladorPanelNotificaciones.$inject = ['$scope', 'serviciosComponenteNotificaciones', '$routeParams', 'backboneServicios'];

    function controladorPanelNotificaciones($scope, serviciosComponenteNotificaciones, $routeParams, backboneServicios) {
        var vm = this;
        vm.notificacionSeleccionada = null;
        vm.notificacionesYaFueronCargadas = false;
        
        if (backboneServicios.estaAutorizado() && vm.notificacionesYaFueronCargadas == false) {
            vm.notificacionesYaFueronCargadas = true;
            cargarNotificaciones();
        }
        
        $scope.$on('AutorizacionConfirmada', function () {
            if (vm.notificacionesYaFueronCargadas == false) {
                vm.notificacionesYaFueronCargadas = true;
                cargarNotificaciones();
            }
        });

        function cargarNotificaciones() {
            serviciosComponenteNotificaciones.obtenerNotificacionesSinLeer().then(function (result) {
                vm.notificaciones = result.data;
                angular.forEach(vm.notificaciones, function (notificacion) {
                    if (notificacion.IdNotificacion.toString() === $routeParams.id) {
                        vm.notificacionSeleccionada = notificacion;
                    }
                });
            }, function (error) { console.log(error); });
        }

        vm.seleccionarNotificacion = function (notificacion) {
            vm.notificacionSeleccionada = notificacion;
        }

        
    }

    angular.module('backbone').controller('controladorPanelNotificaciones', controladorPanelNotificaciones);
})();