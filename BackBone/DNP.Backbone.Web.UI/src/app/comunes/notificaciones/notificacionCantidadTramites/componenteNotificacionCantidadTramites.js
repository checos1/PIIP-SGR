(function (usuarioDNP, idTipoProyecto, idTipoTramite) {

    angular.module('backbone').component('notificacionCantidadTramites', {
        templateUrl: '/src/app/comunes/notificaciones/notificacionCantidadTramites/pantallaCantidadTramites.html',
        controller: controladorNotificacionCantidadTramites,
        controllerAs: 'vm',
        bindings: {
            useHtmlMenu: '=',
            useHtmlNotification: '='
        }
    });

    controladorNotificacionCantidadTramites.$inject = [
        '$q',
        'serviciosComponenteNotificacionCantidadTramites',
        '$scope',
        'backboneServicios',
        '$timeout'
    ];

    function controladorNotificacionCantidadTramites(
        $q,
        serviciosComponenteNotificacionCantidadTramites,
        $scope,
        backboneServicios,
        $timeout
    ) {
        var vm = this;
        vm.cantidadDeTramites = 0;
        vm.notificacionesYaFueronCargadas = false;
        vm.obtenerNotificacionesCantidadDeTramites = obtenerNotificacionesCantidadDeTramites;

        vm.$onInit = function () {
            if (backboneServicios.estaAutorizado() && vm.notificacionesYaFueronCargadas == false) {
                vm.notificacionesYaFueronCargadas = true;
                obtenerNotificacionesCantidadDeTramites();
            }
        };

        $scope.$on('AutorizacionConfirmada', function () {
            if (vm.notificacionesYaFueronCargadas == false) {
                vm.notificacionesYaFueronCargadas = true;
                obtenerNotificacionesCantidadDeTramites();
            }
        });

        function obtenerNotificacionesCantidadDeTramites() {
            vm.cantidadDeTramites = 0;
            $timeout(function () {
                serviciosComponenteNotificacionCantidadTramites.obtenerNotificacionesCantidadDeTramites().then(
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