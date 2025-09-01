(function(usuarioDNP, idTipoProyecto, idTipoTramite) {

    angular.module('backbone').component('notificacionCantidadMensajesMtto', {
        templateUrl: '/src/app/comunes/notificaciones/notificacionCantidadMensajesMtto/pantallaCantidadMensajesMtto.html',
        controller: controladorNotificacionCantidadMensajesMtto,
        controllerAs: 'vm'
    });

    controladorNotificacionCantidadMensajesMtto.$inject = [
        '$q',
        '$scope',
        'backboneServicios',
        'sesionServicios',
        'mensajeServicio',
        '$timeout',
        '$uibModal',
    ];

    function controladorNotificacionCantidadMensajesMtto(
        $q,
        $scope,
        backboneServicios,
        sesionServicios,
        mensajeServicio,
        $timeout,
        $uibModal
    ) {
        var vm = this;

        //#region Variables
        vm.peticion = {};
        vm.cantidadDeMensajesMtto = 0;
        vm.notificacionesYaFueronCargadas = false;
        vm.listaMensajeMtto = [];
        vm.filtro = {
            NombreMensaje: null,
            FechaCreacionInicio: null,
            FechaCreacionFin: null,
            EstadoMensaje: null,
            TipoMensaje: null,
            TieneRestringeAcesso: null,
            MensajeTemplate: null,
            TipoEntidad: null
        }

        vm.abrirModalMensajesMtto = abrirModalMensajesMtto;

        //#endregion

        vm.$onInit = function() {

            var roles = sesionServicios.obtenerUsuarioIdsRoles();

            if (backboneServicios.estaAutorizado() && roles != null && roles.length > 0 && vm.notificacionesYaFueronCargadas == false) {

                vm.notificacionesYaFueronCargadas = true;

                vm.peticion = {
                    IdUsuarioDNP: usuarioDNP,
                    Aplicacion: nombreAplicacionBackbone,
                    IdsRoles: roles
                };

                obtenerNotificacionesCantidadDeMensajesMtto(false);
            }

        };

        $scope.$on('AutorizacionConfirmada', function() {
            if (vm.notificacionesYaFueronCargadas == false) {
                vm.notificacionesYaFueronCargadas = true;
                obtenerNotificacionesCantidadDeMensajesMtto();
            }
        });

        function abrirModalMensajesMtto() {

            // Se vuelve a cargar la lista de los mensajes para mostrar los mensajes actualizados
            obtenerNotificacionesCantidadDeMensajesMtto(true);

        };

        function obtenerNotificacionesCantidadDeMensajesMtto(mostrarModalMensajesMtto) {

            vm.listaMensajeMtto = [];

            mensajeServicio.obtenerListaMensaje(vm.peticion, vm.filtro).then(
                function(result) {

                    // se recorre la lista de Mensajes y se cuentan solo los que tiene estado Habilitado (1)
                    result.data.forEach(function(msgMtto) {
                                                
                        if (msgMtto.EstadoMensaje == 1) {
                            vm.listaMensajeMtto.push(msgMtto)
                        }
                    });

                    vm.cantidadDeMensajesMtto = vm.listaMensajeMtto.length;

                    if (mostrarModalMensajesMtto && vm.cantidadDeMensajesMtto > 0) {

                        $uibModal.open({
                            templateUrl: 'src/app/comunes/notificaciones/notificacionCantidadMensajesMtto/modales/modalMensajesMtto.html',
                            controller: 'modalMensajesMttoController',
                            windowClass: "modal-mensajes-mtto",
                            scope: $scope,
                            resolve: {
                                listaMensajeMtto: $q.resolve(vm.listaMensajeMtto),
                                scope: $scope
                            }
                        });

                    }

                },
                function(error) {
                    vm.cantidadDeMensajesMtto = 0;
                }
            );
        }
    }
})(usuarioDNP, idTipoProyecto, idTipoTramite);