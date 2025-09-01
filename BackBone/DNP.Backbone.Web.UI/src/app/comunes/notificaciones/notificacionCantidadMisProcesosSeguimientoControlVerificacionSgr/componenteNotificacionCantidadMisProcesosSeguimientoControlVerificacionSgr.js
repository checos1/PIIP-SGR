(function (usuarioDNP, idTipoProyecto, idTipoTramite) {

    angular.module('backbone').component('notificacionCantidadMisProcesosSeguimientoControlVerificacionSgr', {
        templateUrl: '/src/app/comunes/notificaciones/notificacionCantidadMisProcesosSeguimientoControlVerificacionSgr/pantallaCantidadMisProcesosSeguimientoControlVerificacionSgr.html',
        controller: controladorNotificacionCantidadMisProcesosSeguimientoControlVerificacionSgr,
        controllerAs: 'vm',
        bindings: {
            useHtmlMenu: '=',
            useHtmlNotification: '='
        }
    });

    controladorNotificacionCantidadMisProcesosSeguimientoControlVerificacionSgr.$inject = [
        '$q',
        'serviciosComponenteNotificacionCantidadProyectos',
        'serviciosComponenteNotificacionCantidadTramites',
        '$scope',
        'backboneServicios',
        '$timeout',
        '$localStorage',
        '$interval',
        'constantesBackbone'
    ];

    function controladorNotificacionCantidadMisProcesosSeguimientoControlVerificacionSgr(
        $q,
        serviciosComponenteNotificacionCantidadProyectos,
        serviciosComponenteNotificacionCantidadTramites,
        $scope,
        backboneServicios,
        $timeout,
        $localStorage,
        $interval,
        constantesBackbone
    ) {
        var vm = this;
        vm.cantidadDeProyectos = 0;
        vm.cantidadDeTramites = 0;
        vm.notificacionesYaFueronCargadas = false;
        vm.peticionTramitesFinalizada = false;
        vm.peticionProyectosFinalizada = false;
        vm.tramites = null;
        vm.proyectos = null;
        vm.totalProyectos = 0;

        $interval( () => procesarInformacion(), 1800);

        function procesarInformacion() {
            if ($localStorage.cantidadesMisproyectos != undefined) {
                vm.totalProyectos = $localStorage.cantidadesMisproyectos[0].PProyecto + $localStorage.cantidadesMisproyectos[0].GRProyecto + $localStorage.cantidadesMisproyectos[0].EJProyecto + $localStorage.cantidadesMisproyectos[0].EVProyecto;
                vm.totalTramites = $localStorage.cantidadesMisproyectos[0].EJTramite + $localStorage.cantidadesMisproyectos[0].EJProgramacion;
                vm.totalProcesos = vm.totalProyectos + vm.totalTramites;
            }
        }

        vm.$onInit = function () {
        };

    }
})(usuarioDNP, idTipoProyecto, idTipoTramite);