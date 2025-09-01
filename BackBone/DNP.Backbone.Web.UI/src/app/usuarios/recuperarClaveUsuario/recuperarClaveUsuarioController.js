(function () {
    'use strict';

    recuperarClaveUsuarioController.$inject = [
    //    'objInvitarUsuario',
    //    'actions',
    //    'utilidades',
    //    'servicioUsuarios'
    ];

    function recuperarClaveUsuarioController(
    //    objInvitarUsuario,
    //    actions,
    //    utilidades,
    //    servicioUsuarios
    ) {
        var vm = this;

        vm.btnEnviarActivo = false;
        vm.esFormularioValido = false;

        vm.tiposIdentificacion = [
            { id: 'CC', name: 'Cédula' },
            { id: 'NI', name: 'NIT' },
            { id: 'PA', name: 'Pasaporte' }
        ];

        vm.init = function () {
            vm.btnEnviarActivo = false;
            vm.esFormularioValido = false;

            vm.model = {
                tipoDocumento: '',
                numeroDocumento: '',
            }
        }
    }

    angular.module('backbone.usuarios').controller('recuperarClaveUsuarioController', recuperarClaveUsuarioController);
})();
