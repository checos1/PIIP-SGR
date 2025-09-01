(function (usuarioDNP, idTipoProyecto, idTipoTramite) {

    angular.module('backbone').component('notificacionCantidadProyectos', {
        templateUrl: '/src/app/comunes/notificaciones/notificacionCantidadProyectos/pantallaCantidadProyectos.html',
        controller: controladorNotificacionCantidadProyectos,
        controllerAs: 'vm',
        bindings: {
            useHtmlMenu: '=',
            useHtmlNotification: '='
        }
    });

    controladorNotificacionCantidadProyectos.$inject = [
        '$q',
        'serviciosComponenteNotificacionCantidadProyectos',
        '$scope',
        'backboneServicios',
        '$timeout'
    ];

    function controladorNotificacionCantidadProyectos(
        $q,
        serviciosComponenteNotificacionCantidadProyectos,
        $scope,
        backboneServicios,
        $timeout
    ) {
        var vm = this;
        vm.cantidadDeProyectos = 0;
        vm.notificacionesYaFueronCargadas = false;
        
        vm.$onInit = function () {
            if (backboneServicios.estaAutorizado() && vm.notificacionesYaFueronCargadas == false) {
                vm.notificacionesYaFueronCargadas = true;
                obtenerNotificacionesCantidadDeProyectos();
            }
        };

        $scope.$on('AutorizacionConfirmada', function () {
            if (vm.notificacionesYaFueronCargadas == false) {
                vm.notificacionesYaFueronCargadas = true;
                obtenerNotificacionesCantidadDeProyectos();
            }
        });

        function obtenerNotificacionesCantidadDeProyectos() {
            vm.cantidadDeProyectos = 0;
            $timeout(function () {
                serviciosComponenteNotificacionCantidadProyectos.obtenerNotificacionesCantidadDeProyectos().then(
                    function (result) {
                        if (result.data.GruposEntidades && result.data.GruposEntidades.length > 0) {
                            const listaGrupoEntidades = result.data.GruposEntidades;
                            listaGrupoEntidades.forEach(grupoEntidade => {
                                grupoEntidade.ListaEntidades.forEach(entidad => {
                                    vm.cantidadDeProyectos += entidad.ObjetosNegocio.length;
                                });
                            });
                        }
                    },
                    function (error) {
                        vm.cantidadDeProyectos = 0;
                    }
                );
            }, 3000);
        }

    }
})(usuarioDNP, idTipoProyecto, idTipoTramite);