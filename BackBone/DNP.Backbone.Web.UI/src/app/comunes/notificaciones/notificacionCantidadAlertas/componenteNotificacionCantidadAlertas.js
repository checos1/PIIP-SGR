(function (usuarioDNP, idTipoProyecto, idTipoTramite) {

    angular.module('backbone').component('notificacionCantidadAlertas', {
        templateUrl: '/src/app/comunes/notificaciones/notificacionCantidadAlertas/pantallaCantidadAlertas.html',
        controller: controladorNotificacionCantidadAlertas,
        controllerAs: 'vm',
        bindings: {
            useHtmlMenu: '=',
            useHtmlNotification: '='
        }
    });

    controladorNotificacionCantidadAlertas.$inject = [
        '$q',
        'serviciosComponenteNotificacionCantidadAlertas',
        '$scope',
        'backboneServicios',
        '$timeout'
    ];

    function controladorNotificacionCantidadAlertas(
        $q,
        serviciosComponenteNotificacionCantidadAlertas,
        $scope,
        backboneServicios,
        $timeout
    ) {
        var vm = this;
        vm.cantidadDeTramites = 0;
        vm.notificacionesYaFueronCargadas = false;

        vm.$onInit = function () {
            if (backboneServicios.estaAutorizado() && vm.notificacionesYaFueronCargadas == false) {
                vm.notificacionesYaFueronCargadas = true;
                obtenerNotificacionesCantidadDeAlertas();
            }
        };

        $scope.$on('AutorizacionConfirmada', function () {
            if (vm.notificacionesYaFueronCargadas == false) {
                vm.notificacionesYaFueronCargadas = true;
                obtenerNotificacionesCantidadDeAlertas();
            }
        });

        function obtenerNotificacionesCantidadDeAlertas() {
            vm.cantidadDeTramites = 0;
            $timeout(function () {
                serviciosComponenteNotificacionCantidadAlertas.obtenerNotificacionesCantidadDeAlertas().then(
                    function (result) {
                        if (result.data.ListaGrupoTramiteEntidad && result.data.ListaGrupoTramiteEntidad.length > 0) {
                            const listaGrupoEntidades = result.data.ListaGrupoTramiteEntidad;
                            listaGrupoEntidades.forEach(entidad => {
                                entidad.GrupoTramites.forEach(tramite => {
                                    vm.cantidadDeTramites += tramite.ListaTramites.length;
                                });
                            });
                        }
                    },
                    function (error) {
                        vm.cantidadDeTramites = 0;
                    }
                );
            }, 3000);
        }
    }
})(usuarioDNP, idTipoProyecto, idTipoTramite);