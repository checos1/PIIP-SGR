(function () {
    'use strict';

    modalTipoInvitacionUsuarioTerritorioController.$inject = [
        'actions',
        '$uibModalInstance'
    ];

    function modalTipoInvitacionUsuarioTerritorioController(
        actions,
        $uibModalInstance
    ) {
        var vm = this;

        vm.init = init;
        vm.tipoUsuario = 0;
        vm.cerrar = $uibModalInstance.dismiss;
        vm.cambiaTipoValor = cambiaTipoValor;
        vm.isConstante = false;

        vm.tiposUsuario = [
            {id: 1, name: 'Usuario DNP'},
            {id: 2, name: 'Usuario externo' }
        ];

        vm.submitAbrirFormularioInvitarUsuario = submitAbrirFormularioInvitarUsuario;

        function init() {

        }
        function cambiaTipoValor(tipoValor) {
            if (tipoValor) {
                switch (tipoValor) {
                    case 1:
                        actions.abrirModalInvitarUsuarioDNP()
                        break;
                    case 2:
                        actions.abrirModalInvitarUsuarioExterno()
                        break;
                }
                vm.cerrar();
            }

        }
        function submitAbrirFormularioInvitarUsuario() {
            if (vm.tipoUsuario) {
                switch (vm.tipoUsuario) {
                    case 1:
                        actions.abrirModalInvitarUsuarioDNP()
                        break;
                    case 2:
                        actions.abrirModalInvitarUsuarioExterno()
                        break;
                }
                vm.cerrar();
            }
        }

    }

    angular.module('backbone.usuarios').controller('modalTipoInvitacionUsuarioTerritorioController', modalTipoInvitacionUsuarioTerritorioController);
})();
