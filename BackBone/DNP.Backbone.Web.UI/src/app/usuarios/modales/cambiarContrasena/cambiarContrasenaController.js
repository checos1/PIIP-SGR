(function () {
    'use strict';

    angular.module('backbone')
        .controller('cambiarContrasenaController', cambiarContrasenaController);

    cambiarContrasenaController.$inject = [
        '$window',
        '$uibModalInstance',
        'servicioUsuarios',
        'constantesBackbone',
        'params'
    ];

    function cambiarContrasenaController(
        $window,
        $uibModalInstance,
        servicioUsuarios,
        constantesBackbone,
        params
    ) {
        const vm = this;

        vm.esUsuarioExterno = false;
        vm.nombreUsuario = '';
        vm.labelTipoDocumento = '';

        vm.listadoLetrasMinusculas = "abcdefghyjklmnñopqrstuvwxyz";
        vm.listadoLetrasMayusculas = "ABCDEFGHYJKLMNÑOPQRSTUVWXYZ";
        vm.listadoNumeros = "0123456789";

        vm.politicaContrasenaActual = false;

        vm.model = {
            TipoIdentificacion: null,
            NumeroIdentificacion: null,
            UsuarioGuid: $window.usuarioGuid,
            ContrasenaActual: null,
            ContrasenaNueva: null,
            ContrasenaNuevaConfirmar: null
        };

        vm.modelValidaciones = {
            contrasenaNuevaIgualActual: false,
            contrasenaNuevaConfirmacionNoCoincide: false,
            contrasenaNuevaLongitudNoPermitida: false,
            contrasenaNuevaSinValorNumericoOEspecial: false,
            contrasenaNuevaLimiteSecuenciaLetrasNumeros: false,
            contrasenaNuevaAltasBajasLetras: false,
            contrasenaActualSTS: false
        };

        const _validadoresFormulario = [
            (model) => ([{ invalido: !model.ContrasenaNuevaConfirmar, mensaje: "Ingrese la confirmación de la nueva contraseña" }]),
            (model) => ([{ invalido: !model.ContrasenaNueva, mensaje: "Ingrese una nueva contrasenã" }]),
            (model) => ([{ invalido: !model.ContrasenaActual, mensaje: "Ingrese su contraseña actual" }]),
            (model) => ([{ invalido: !model.NumeroIdentificacion, mensaje: "Es necesario proporcionar un número de identificación para el usuario" }]),
            (model) => ([{ invalido: !model.TipoIdentificacion, mensaje: "Es necesario proporcionar un tipo de identificación para el usuario" }]),
        ];        

        const _validadoresContrasena = [
            (model) => ([{ invalido: !model.contrasenaNuevaIgualActual, mensaje: "No puede usar la misma contraseña actual." }]),
            (model) => ([{ invalido: !model.contrasenaNuevaConfirmacionNoCoincide, mensaje: "Las contraseñas no coinciden" }]),
            (model) => ([{ invalido: !model.contrasenaNuevaLongitudNoPermitida, mensaje: "La contraseña de contener mínimo(8) ocho y máximo(12) doce carácteres." }]),
            (model) => ([{ invalido: !model.contrasenaNuevaSinValorNumericoOEspecial, mensaje: "La contraseña de contener al menos (1) un número y (1) un carácter especial." }]),
            (model) => ([{ invalido: !model.contrasenaNuevaLimiteSecuenciaLetrasNumeros, mensaje: "La contraseña de contener máximo (3) letras o números secuenciales." }]),
            (model) => ([{ invalido: !model.contrasenaNuevaAltasBajasLetras, mensaje: "La contraseña de contener al menos (1) una letra minúscula y (1) una mayúscula." }]),
            (model) => ([{ invalido: !model.contrasenaActualSTS, mensaje: "La contraseña actual no coincide con la registrada en el sistema." }]),
        ];

        vm.init = function () {
            if (!params.usuarioDNP)
                throw 'Se requiere un UsuarioDNP para cambiar la contrasena';

            vm.nombreUsuario = params.nombreUsuario;
            vm.esUsuarioExterno = params.esUsuarioExterno;

            vm.usuarioDNP = params.usuarioDNP;
            vm.formatearIdentificacionUsuario();
            
        };

        vm.formatearIdentificacionUsuario = function () {
            vm.model.TipoIdentificacion = vm.obtenerTipoDocumento();
            vm.model.NumeroIdentificacion = vm.obtenerNumeroDocumento();
            vm.labelTipoDocumento = vm.obtenerLabelTipoDocumento();
        }

        vm.obtenerTipoDocumento = function () {
            return vm.usuarioDNP.substring(0, 2);
        }

        vm.obtenerNumeroDocumento = function () {
            return vm.usuarioDNP.substring(2, vm.usuarioDNP.length);
        }

        vm.obtenerLabelTipoDocumento = function () {
            let tipoDocumento = vm.model.TipoIdentificacion;
            let labelTipoDocumento = "Cédula de ciudadanía";

            switch (tipoDocumento) {
                case 'CC':
                    labelTipoDocumento = "Cédula de ciudadanía";
                    break;
                case 'NI':
                    labelTipoDocumento = "Número de identificación tributaria";
                    break;
                case 'PA':
                    labelTipoDocumento = "Pasaporte";
                    break;
            }

            return labelTipoDocumento;
        }

        vm.validarCambioContrasena = async function () {
            let contrasenaValida = true;
            let politicaContrasenaValida;

            vm.modelValidaciones.contrasenaNuevaIgualActual = true;
            vm.modelValidaciones.contrasenaNuevaConfirmacionNoCoincide = true;
            vm.modelValidaciones.contrasenaNuevaLongitudNoPermitida = true;
            vm.modelValidaciones.contrasenaNuevaSinValorNumericoOEspecial = true; 
            vm.modelValidaciones.contrasenaNuevaLimiteSecuenciaLetrasNumeros = true;
            vm.modelValidaciones.contrasenaNuevaAltasBajasLetras = true;
            vm.modelValidaciones.contrasenaActualSTS = true;

            if (contrasenaValida) {
                politicaContrasenaValida = vm.validarDiferenciaContrasenaActualNueva();
                if (!politicaContrasenaValida) {
                    vm.modelValidaciones.contrasenaNuevaIgualActual = false;
                    contrasenaValida = false;
                }
            }

            if (contrasenaValida) {
                politicaContrasenaValida = vm.validarIgualdadContrasenaNuevaConfirmacion();
                if (!politicaContrasenaValida) {
                    vm.modelValidaciones.contrasenaNuevaConfirmacionNoCoincide = false;
                    contrasenaValida = false;
                }
            }

            if (contrasenaValida) {
                politicaContrasenaValida = vm.validarLongitudContrasena();
                if (!politicaContrasenaValida) {
                    vm.modelValidaciones.contrasenaNuevaLongitudNoPermitida = false;
                    contrasenaValida = false;
                }
            }

            if (contrasenaValida) {
                politicaContrasenaValida = vm.validarValorNumericoEspecialContrasena();
                if (!politicaContrasenaValida) {
                    vm.modelValidaciones.contrasenaNuevaSinValorNumericoOEspecial = false; 
                    contrasenaValida = false;
                }
            }

            if (contrasenaValida) {
                politicaContrasenaValida = vm.validarLimiteSecuenciaLetrasNumerosContrasena();
                if (!politicaContrasenaValida) {
                    vm.modelValidaciones.contrasenaNuevaLimiteSecuenciaLetrasNumeros = false; 
                    contrasenaValida = false;
                }
            }

            if (contrasenaValida) {
                politicaContrasenaValida = vm.validarAltasBajasLetrasContrasena();
                if (!politicaContrasenaValida) {
                    vm.modelValidaciones.contrasenaNuevaAltasBajasLetras = false;
                    contrasenaValida = false;
                }
            }

            if (contrasenaValida) {
                await vm.validarContrasenaActualSTS();
                if (!vm.politicaContrasenaActual) {
                    vm.modelValidaciones.contrasenaActualSTS = false;
                    contrasenaValida = false;
                }
            }

        }

        vm.validarDiferenciaContrasenaActualNueva = function ()
        {
            let politicaContrasenaValida = vm.model.ContrasenaActual != vm.model.ContrasenaNueva;
            return politicaContrasenaValida;
        }

        vm.validarIgualdadContrasenaNuevaConfirmacion = function ()
        {
            let politicaContrasenaValida = vm.model.ContrasenaNueva == vm.model.ContrasenaNuevaConfirmar;
            return politicaContrasenaValida;
        }

        vm.validarLongitudContrasena = function ()
        {
            let politicaContrasenaValida = vm.model.ContrasenaNueva.length >= 8 && vm.model.ContrasenaNueva.length <= 12;
            return politicaContrasenaValida;
        }

        vm.validarValorNumericoEspecialContrasena = function ()
        {
            let politicaContrasenaValida = vm.buscarCaracterLetraNumero(vm.listadoNumeros, vm.model.ContrasenaNueva) && vm.buscarCaracterEspecial(vm.listadoNumeros + vm.listadoLetrasMinusculas + vm.listadoLetrasMayusculas, vm.model.ContrasenaNueva);
            return politicaContrasenaValida;
        }

        vm.validarLimiteSecuenciaLetrasNumerosContrasena = function ()
        {
            let politicaContrasenaValida = vm.buscarSecuencia(vm.listadoLetrasMinusculas, vm.model.ContrasenaNueva) && vm.buscarSecuencia(vm.listadoLetrasMayusculas, vm.model.ContrasenaNueva) && vm.buscarSecuencia(vm.listadoNumeros, vm.model.ContrasenaNueva);
            return politicaContrasenaValida;
        }

        vm.validarAltasBajasLetrasContrasena = function ()
        {
            let politicaContrasenaValida = vm.buscarCaracterLetraNumero(vm.listadoLetrasMinusculas, vm.model.ContrasenaNueva) && vm.buscarCaracterLetraNumero(vm.listadoLetrasMayusculas, vm.model.ContrasenaNueva);
            return politicaContrasenaValida;
        }

        vm.validarContrasenaActualSTS = async function validarContrasenaActualSTS() {

            let modelo = {
                pAplicacion: constantesBackbone.keySTSApplication,
                pTipoDocumento: vm.model.TipoIdentificacion,
                pNumeroDocumento: vm.model.NumeroIdentificacion,
                pPassword: vm.model.ContrasenaActual,
                pCorreo: ''
            };

            vm.politicaContrasenaActual = false
            await servicioUsuarios.validarContrasenaActualSTS(modelo)
                .then(function (response) {
                    let exito = response.data;
                    if (exito) {
                        vm.politicaContrasenaActual = true
                    }
                })
                .catch(error => {
                    if (error.status == 400) {
                        utilidades.mensajeError(error.data.Message || "Error al realizar la operación");
                        return;
                    }

                    utilidades.mensajeError("Error al realizar la operación");
                });
        }

        vm.buscarCaracterLetraNumero = function(cadena, contrasena)
        {
            let contieneCaracter = false;

            for (let i = 0; i < contrasena.length; i++) {
                if (cadena.indexOf(contrasena[i], 0) != -1) {
                    contieneCaracter = true;
                }
            }

            return contieneCaracter;
        }

        vm.buscarCaracterEspecial = function(cadena, contrasena)
        {
            let contieneCaracter = false;

            for (let i = 0; i < contrasena.length; i++) {
                if (cadena.indexOf(contrasena[i], 0) == -1) {
                    contieneCaracter = true;
                }
            }

            return contieneCaracter;
        }

        vm.buscarSecuencia = function(cadena, contrasena)
        {
            let contieneCaracter = true;

            for (let i = 0; i < contrasena.length - 3; i++) {
                if (cadena.includes(contrasena.substring(i, i + 4))) {
                    contieneCaracter = false;
                }
            }

            return contieneCaracter;
        }

        vm.validarFormulario = async function () {
            try {
                const mensajes = [];
                _validadoresFormulario.forEach(validator => {
                    validator(vm.model).forEach(resultado => {
                        if (resultado.invalido)
                            mensajes.push(resultado.mensaje);
                    });
                })

                if (mensajes.length == 0) {
                    await vm.validarCambioContrasena();

                    _validadoresContrasena.forEach(validator => {
                        validator(vm.modelValidaciones).forEach(resultado => {
                            if (resultado.invalido)
                                mensajes.push(resultado.mensaje);
                        });
                    })
                }

                if (mensajes.length)
                    vm.mostrarMensajes(mensajes);

                return mensajes.length == 0;
            }
            catch (e) {
                console.log(e);
                toastr.error("Error inesperado al validar la información del usuario");
                return false;
            }
        }

        vm.mostrarMensajes = function (toastMessages = []) {
            toastMessages.forEach(message => {
                if (!message)
                    return;

                toastr.warning(message);
            })
        }

        vm.cambiarContrasena = async function () {
            const valido = await vm.validarFormulario();
            if (!valido) return;

            var modelo = {
                pAplicacion: constantesBackbone.keySTSApplication,
                pTipoDocumento: vm.model.TipoIdentificacion,
                pNumeroDocumento: vm.model.NumeroIdentificacion,
                pPassword: vm.model.ContrasenaNueva,
                pCorreo: ''
            };

            await servicioUsuarios.cambiarContrasenaSTS(modelo)
                .then(() => {
                    toastr.success("Contraseña cambiada con exito")
                })
                .catch(error => {
                    toastr.error(error);
                });
        };

        vm.cerrar = function () {
            $uibModalInstance.close(false);
        };
    }
})();