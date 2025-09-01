(function () {
    'use strict';

    asignarClaveUsuarioController.$inject = [
        '$routeParams',
        'utilidades',
        'servicioUsuarios'
    ];

    function asignarClaveUsuarioController(
        $routeParams,
        utilidades,
        servicioUsuarios
    ) {
        var vm = this;

        vm.esCampoClaveActualValido = false;
        vm.esCampoNuevaClaveValido = false;
        vm.esCampoConfirmacionClaveValido = false;
        vm.btnGuardarActivo = false;
        vm.mostrarFormulario = false;
        vm.mostrarMensajeURLInvalida = false;
        vm.esFormularioValido = false;
        vm.esKeyValido = false;

        vm.key = '';
        vm.keyLimpia = '';
        vm.keyArrayContent = [];

        vm.tiposIdentificacion = [
            { id: 'CC', name: 'Cédula' },
            { id: 'NI', name: 'NIT' },
            { id: 'PA', name: 'Pasaporte' }
        ];

        vm.init = function () {
            vm.esCampoClaveActualValido = false;
            vm.esCampoNuevaClaveValido = false;
            vm.esCampoConfirmarClaveValido = false;
            vm.btnGuardarActivo = false;
            vm.mostrarFormulario = false;
            vm.mostrarMensajeURLInvalida = false;
            vm.esFormularioValido = false;
            vm.esKeyValido = false;

            vm.key = '';
            vm.keyLimpia = '';
            vm.keyArrayContent = [];

            vm.model = {
                tipoDocumento: '',
                numeroDocumento: '',
                claveActual: '',
                nuevaClave: '',
                confirmarClave: ''
            }

            vm.validarKey();
            vm.precargarFormulario();

        }

        vm.validarKey = function () {
            vm.key = '';

            if ($routeParams.id) {
                vm.key = $routeParams.id;
            }

            if (vm.key == '') {
                vm.mostrarMensajeURLInvalida = true;
                return
            }

            vm.keyLimpia = window.atob(vm.key);

            if (!vm.keyLimpia.toString().includes('|fechaValida:')) {
                vm.mostrarMensajeURLInvalida = true;
                return
            }

            vm.keyArrayContent = vm.keyLimpia.toString().split('|')

            if (vm.keyArrayContent.length != 3) {
                vm.mostrarMensajeURLInvalida = true;
                return
            }


            if (!vm.keyArrayContent[0].toString().includes('tipoDocumento:') || !vm.keyArrayContent[1].toString().includes('numeroDocumento:') || !vm.keyArrayContent[2].toString().includes('fechaValida:')) {
                vm.mostrarMensajeURLInvalida = true;
                return
            }

            let fechaValida = new Date(vm.keyArrayContent[2].toString().split(':')[1] + " ")
            let fechaActual = new Date()

            if (fechaValida > fechaActual) {
                vm.mostrarMensajeURLInvalida = true;
                return
            }

        }

        vm.precargarFormulario = function () {
            vm.mostrarFormulario = false;

            if (vm.mostrarMensajeURLInvalida) {
                return
            }

            vm.mostrarFormulario = true;

            vm.keyLimpia = window.atob(vm.key);
            vm.keyArrayContent = vm.keyLimpia.toString().split('|')

            vm.model.tipoDocumento = vm.keyArrayContent[0].toString().split(':')[1]
            vm.model.numeroDocumento = vm.keyArrayContent[1].toString().split(':')[1]

        }

        vm.validarCampoClaveActual = function () {
            vm.esCampoClaveActualValido = false;

            let esCampoValido = utilidades.isNotNull(vm.model.claveActual);

            if (esCampoValido) {
                vm.esCampoClaveActualValido = true
            }
            vm.validarFormulario();
        }

        vm.validarCampoNuevaClave = function () {
            vm.esCampoNuevaClaveValido = false;

            let esCampoValido = utilidades.isNotNull(vm.model.nuevaClave);

            if (esCampoValido) {
                vm.esCampoNuevaClaveValido = true
            }
            vm.validarFormulario();
        }

        vm.validarCampoConfirmarClave = function () {
            vm.esCampoConfirmarClave = false;

            let esCampoValido = utilidades.isNotNull(vm.model.confirmarClave);

            if (esCampoValido) {
                vm.esCampoConfirmarClave = true
            }
            vm.validarFormulario();
        }

        vm.validarFormulario = function () {

            vm.esFormularioValido = false;
            if (
                vm.esCampoClaveActualValido
                && vm.esCampoNuevaClaveValido
                && vm.esCampoConfirmarClave
            ) {
                vm.esFormularioValido = true;
            }
        }


    }

    angular.module('backbone.usuarios').controller('asignarClaveUsuarioController', asignarClaveUsuarioController);
})();
