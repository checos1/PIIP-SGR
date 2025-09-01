(function () {
    'use strict';

    modalInvitarUsuarioExternoController.$inject = [
        'objInvitarUsuario',
        'actions',
        '$uibModalInstance',
        '$timeout',
        'utilidades',
        'constantesBackbone',
        'servicioUsuarios'
    ];

    function modalInvitarUsuarioExternoController(
        objInvitarUsuario,
        actions,
        $uibModalInstance,
        $timeout,
        utilidades,
        constantesBackbone,
        servicioUsuarios
    ) {
        var vm = this;

        vm.init = init;
        vm.cerrar = $uibModalInstance.dismiss;

        function init() {
            vm.cambiaTipoValor = cambiaTipoValor;
            vm.tiposIdentificacion = objInvitarUsuario.tiposIdentificacion;
            vm.listaEntidades = objInvitarUsuario.listaEntidades;
            vm.listaPerfilesBackbone = objInvitarUsuario.listaPerfilesBackbone;
            vm.esCorreoInstitucionalValido = false;
            vm.btnBuscarActivo = false;
            vm.btnInvitarActivo = false;
            vm.mostrarFormulario = false;
            vm.mostrarFormularioPerfilamiento = false;
            vm.esTipoIdentificacionValido = false;
            vm.esIdentificacionValido = false;
            vm.esNombresValido = false;
            vm.mostrarMensaje1 = false;
            vm.mostrarMensaje2 = false;
            vm.mostrarMensaje3 = false;
            vm.BanderaDivPerfiles = false;
            vm.BanderaDivPerfiles2 = false;
            vm.esApellidosValido = false;
            vm.esEntidadValido = false;
            vm.esPerfilValido = false;
            vm.encontroUsuarioPIIP = false;
            vm.encontroDatosUsuarioPIIP = false;
            vm.encontroUsuarioSTSAplicacion = false;
            vm.encontroUsuarioSTSAplicacionMGA = false;
            vm.encontroUsuarioSTSAplicacionConfiable = false;
            vm.encontroUsuarioSTS = false;
            vm.esFormularioValido = false;
            vm.activarFormulario = false;
            vm.activarBotonInvitarPIIPNoSTS = false;
            vm.BanderaInputnumeros = false;
            vm.tiposUsuario = [
                { id: 1, name: 'Usuario DNP' },
                { id: 2, name: 'Usuario externo' }
            ];
            vm.tipoUsuario = 0;
            vm.model = {
                correoInstitucional: '',
                tipoIdentificacion: '',
                identificacion: '',
                nombres: '',
                apellidos: '',
                idEntidad: '',
                idPerfilBackbone: ''
            }

            vm.usuarioPIIP = {
                IdUsuario: '',
                IdUsuarioDnp: '',
                TipoIdentificacion: '',
                Identificacion: '',
                Nombre: ''
            }

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
        vm.validarTipoIdentificacion = function () {
            vm.mostrarFormulario = false;
            vm.btnBuscarActivo = false;
            vm.esTipoIdentificacionValido = false;

            vm.limpiarFormulario();

            let esCampoValido = utilidades.isNotNull(vm.model.tipoIdentificacion);

            if (esCampoValido) {
                vm.model.identificacion = '';
                if (vm.model.tipoIdentificacion == "CC" || vm.model.tipoIdentificacion == "NI") {
                    vm.BanderaInputnumeros = true;
                }
                else {
                    vm.BanderaInputnumeros = false;
                }
                vm.esTipoIdentificacionValido = true;
                vm.validarIdentificacion();
                if (vm.esIdentificacionValido) {
                    vm.btnBuscarActivo = true;
                }
            }
             vm.validarFormulario();
        }

        vm.validarIdentificacion = function () {
            vm.mostrarFormulario = false;
            vm.btnBuscarActivo = false;
            vm.esIdentificacionValido = false;

            vm.limpiarFormulario();

            let esCampoValido = utilidades.isNotNull(vm.model.identificacion);

            if (esCampoValido) {
                vm.esIdentificacionValido = true
                if (vm.esTipoIdentificacionValido) {
                    vm.btnBuscarActivo = true;
                }
            }
            vm.validarFormulario();
        }

        vm.validarCorreoInstitucional = function () {
            vm.esCorreoInstitucionalValido = false;

            let esCorreoValido = utilidades.validarEmail(vm.model.correoInstitucional);

            if (esCorreoValido) {
                vm.esCorreoInstitucionalValido = true;
            }
            vm.validarFormulario();
        }

        vm.validarNombres = function () {
            vm.esNombresValido = false;
            let esCampoValido = utilidades.isNotNull(vm.model.nombres);

            if (esCampoValido) {
                vm.esNombresValido = true
            }
            vm.validarFormulario();
        }

        vm.validarApellidos = function () {
            vm.esApellidosValido = false;
            let esCampoValido = utilidades.isNotNull(vm.model.apellidos);

            if (esCampoValido) {
                vm.esApellidosValido = true
            }
            vm.validarFormulario();
        }

        vm.validarEntidad = function () {
            vm.esEntidadValido = false;
            let esCampoValido = utilidades.isNotNull(vm.model.idEntidad);

            if (esCampoValido) {
                vm.esEntidadValido = true
                let entidad = vm.obtenerEntidad(vm.model.idEntidad)
                vm.obtenerListadoPerfilesXEntidad(entidad.PadreId);
               
            }
            else {
                vm.BanderaDivPerfiles2 = false;
            }
            vm.validarPerfil();
            vm.validarFormulario();
        }

        vm.validarPerfil = function () {

            vm.esPerfilValido = false;
            //let esCampoValido = utilidades.isNotNull(vm.model.idPerfilBackbone);

            if (vm.ListaPerfilesUsuario.length > 0) {
                vm.esPerfilValido = true
            }
            vm.validarFormulario();
        }

        vm.validarFormulario = function () {

            vm.esFormularioValido = false;
            if (
                vm.esTipoIdentificacionValido
                && vm.esIdentificacionValido
                && vm.esCorreoInstitucionalValido
                && vm.esNombresValido
                && vm.esApellidosValido
                && vm.esEntidadValido
                && vm.esPerfilValido
                && ((vm.esEntidadValido )|| (vm.encontroUsuarioPIIP && !vm.encontroUsuarioSTSAplicacion))
            ) {
                vm.esFormularioValido = true;
            }
        }

        vm.activarBotonInvitar = function () {
            return vm.esFormularioValido && vm.btnInvitarActivo && (vm.activarFormulario || vm.activarBotonInvitarPIIPNoSTS || vm.activarFormularioPerfilamiento);
        }

        vm.limpiarFormulario = function () {
            vm.model.correoInstitucional = '';
            vm.model.nombres = '';
            vm.model.apellidos = '';
            vm.model.idEntidad = null;
            vm.model.idPerfilBackbone = '';
           
        }

        vm.buscarUsuario = function () {
            vm.activarCorreo = true;
            vm.esFormularioValido = false;
            vm.btnInvitarActivo = false;
            vm.activarFormulario = false;
            vm.mostrarFormulario = false;
            vm.mostrarFormularioPerfilamiento = false;

            vm.encontroUsuarioPIIP = false;
            vm.encontroDatosUsuarioPIIP = false;
            vm.encontroUsuarioSTSAplicacion = false;
            vm.encontroUsuarioSTSAplicacionMGA = false;
            vm.encontroUsuarioSTSAplicacionConfiable = false;
            vm.encontroUsuarioSTS = false;

            vm.limpiarFormulario();
            vm.validarFormulario();

            vm.buscarUsuarioPIIP();
        }

        vm.MostrarDivPerfilesUsuario = function (idbandera) {
            if (idbandera == 1) {
                vm.BanderaDivPerfiles = true;
                vm.btnBuscarActivo = false;
                vm.btnInvitarActivo = false;
            }
            else {
                vm.BanderaDivPerfiles = false;
                vm.btnBuscarActivo = true;
                vm.btnInvitarActivo = true;
            }
        }
        vm.buscarUsuarioPIIP = function () {

            let usuario = vm.model.tipoIdentificacion + vm.model.identificacion;
            servicioUsuarios.autorizacionObtenerUsuarioPorIdUsuarioDnp(usuario).then(function (response) {
                if (response.data) {
                    const data = response.data;

                    vm.encontroUsuarioPIIP = utilidades.isNotNull(data.IdUsuarioDnp)

                    if (vm.encontroUsuarioPIIP) {

                        vm.usuarioPIIP.IdUsuario = data.IdUsuario ? data.IdUsuario : ''
                        vm.usuarioPIIP.IdUsuarioDnp = data.IdUsuarioDnp ? data.IdUsuarioDnp : ''
                        vm.usuarioPIIP.TipoIdentificacion = data.TipoIdentificacion ? data.TipoIdentificacion : ''
                        vm.usuarioPIIP.Identificacion = data.Identificacion ? data.Identificacion : ''
                        vm.usuarioPIIP.Nombre = data.Nombre ? data.Nombre : ''

                        let conformacionUsuario = vm.usuarioPIIP.Nombre.split(":");

                        vm.model.nombres = conformacionUsuario[0];
                        if (conformacionUsuario[1])  vm.model.apellidos = conformacionUsuario[1];

                        vm.validarCorreoInstitucional();
                        vm.validarNombres();
                        vm.validarApellidos();
                        vm.validarEntidad();
                        vm.validarPerfil();

                    }
                    else {
                        vm.usuarioPIIP.IdUsuario = '';

                    }

                }
                else {
                    vm.usuarioPIIP.IdUsuario = '';

                }
                vm.validarCorreoInstitucional();
                vm.validarNombres();
                vm.validarApellidos();
                vm.validarEntidad();
                vm.validarFormulario();
                vm.buscarDatosUsuarioPIIP();
                vm.BanderaDivPerfiles = false;
                vm.BanderaDivPerfiles2 = false;
                vm.ListaPerfilesUsuario = [];

            }, function (error) {
                let mensaje = "Hubo un error en el servicio de autorizacion para obtener los datos del usuario";
                toastr.error(mensaje);
            });

        }

        vm.buscarDatosUsuarioPIIP = function () {

            let usuario = vm.model.tipoIdentificacion + vm.model.identificacion;
            servicioUsuarios.autorizacionObtenerDatosUsuarioPIIP(usuario).then(function (response) {
                if (response.data) {
                    const data = response.data;

                    vm.encontroDatosUsuarioPIIP = utilidades.isNotNull(data.IdUsuario)

                    if (vm.encontroDatosUsuarioPIIP) {

                        vm.model.correoInstitucional = data.Cuenta;
                        vm.validarCorreoInstitucional();
                    }

                }

                vm.validarFormulario();
                vm.buscarUsuarioSTSAplicacion('PIIP');

            }, function (error) {
                let mensaje = "Hubo un error en el servicio de autorizacion para obtener los datos de la cuenta del usuario";
                toastr.error(mensaje);
            });

        }

        vm.buscarUsuarioSTSAplicacion = function (aplicacion) {
            servicioUsuarios.apiIdentidadVerificarExistenciaUsuarioSTSAplicacion(aplicacion, vm.model.tipoIdentificacion, vm.model.identificacion).then(function (response) {
                console.log('buscarUsuarioSTSAplicacion: ', response)
                if (response.data) {
                    const data = response.data;
                    if (aplicacion == 'PIIP') {
                        vm.encontroUsuarioSTSAplicacion = utilidades.isTrue(data);
                        console.log(vm.encontroUsuarioSTSAplicacion)
                    } else {
                        vm.encontroUsuarioSTSAplicacionMGA = utilidades.isTrue(data);
                        console.log(vm.encontroUsuarioSTSAplicacionMGA)
                    }
                }

                if (aplicacion == 'PIIP') vm.buscarUsuarioSTSAplicacionConfiable();

            }, function (error) {
                let mensaje = "Hubo un error en el servicio de identidad para verificar existencia usuario";
                toastr.error(mensaje);
            });

        }

        vm.buscarUsuarioSTSAplicacionConfiable = function () {

            servicioUsuarios.apiIdentidadObtenerAplicacionesConfiablesExistenciaUsuarioSTS(vm.model.tipoIdentificacion, vm.model.identificacion).then(function (response) {
                console.log('buscarUsuarioSTSAplicacionConfiable: ', response)
                if (response.data) {
                    const data = response.data;
                    vm.encontroUsuarioSTSAplicacionConfiable = utilidades.arrayHasRows(data)
                    console.log(vm.encontroUsuarioSTSAplicacionConfiable)
                }

                vm.buscarUsuarioSTSTotalAplicaciones();

            }, function (error) {
                let mensaje = "Hubo un error en el servicio de identidad para verificar existencia usuario";
                toastr.error(mensaje);
            });

        }

        vm.buscarUsuarioSTSTotalAplicaciones = function () {

            servicioUsuarios.apiIdentidadObtenerAplicacionesExistenciaUsuarioSTS(vm.model.tipoIdentificacion, vm.model.identificacion).then(function (response) {
                console.log('buscarUsuarioSTSTotalAplicaciones: ', response)
                if (response.data) {
                    const data = response.data;
                    vm.encontroUsuarioSTS = utilidades.arrayHasRows(data)
                    console.log(vm.encontroUsuarioSTS)
                }
                vm.buscarUsuarioSTSAplicacion('MGA');
                vm.calcularMensajeBusqueda();

            }, function (error) {
                let mensaje = "Hubo un error en el servicio de identidad para verificar existencia usuario";
                toastr.error(mensaje);
            });

        }

        vm.calcularMensajeBusqueda = function () {
            vm.mostrarFormulario = false;
            vm.mostrarFormularioPerfilamiento = false;
            vm.activarFormulario = false;
            vm.activarFormularioPerfilamiento = false;
            vm.btnInvitarActivo = false;
            vm.mostrarMensaje1 = false;
            vm.mostrarMensaje2 = false;
            vm.mostrarMensaje3 = false;

            // Si existe en la PIIP y en el STS para la aplicación PIIP (Debe activar el formulario de perfilamiento)
            if (vm.encontroUsuarioPIIP && vm.encontroUsuarioSTSAplicacion) {
                vm.mostrarFormulario = true;
                vm.mostrarFormularioPerfilamiento = true;
                vm.activarFormularioPerfilamiento = true;
                vm.btnInvitarActivo = true;

                vm.verificarApellidos();

                vm.validarFormulario();
                vm.mostrarMensaje2 = true;
                //utilidades.mensajeSuccess(
                //    "Para gestionar la información del usuario, puede hacerlo desde la opción de administración de usuarios.",
                //    false,
                //    null,
                //    null,
                //    "El usuario ya cuenta con un registro completo en la PIIP"
                //);

                return;
            }

            //Si existe en la PIIP y no en el STS (Crear usuario en el STS, asignar clave y enviar correo)
            if (vm.encontroUsuarioPIIP && !vm.encontroUsuarioSTSAplicacion ) {
                vm.mostrarFormulario = true;
                vm.activarFormulario = true;
                vm.mostrarFormularioPerfilamiento = true;
                vm.activarFormularioPerfilamiento = true;
                vm.btnInvitarActivo = true;
                vm.activarBotonInvitarPIIPNoSTS = false;

                vm.validarFormulario();
                vm.mostrarMensaje3 = true;
                //utilidades.mensajeSuccess(
                //    "El usuario existe en la PIIP pero no tiene su registro completo, para completar el registro diligenciar el siguiente formulario.",
                //    false,
                //    null,
                //    null,
                //    "El usuario ya existe en la PIIP con registro incompleto"
                //);
                return;
            }

            //Si no existe en la PIIP y si en el STS para otras aplicaciones (Crear usuario en el STS, asignar clave y enviar correo; crear usuario en la PIIP)
            if (!vm.encontroUsuarioPIIP) {
                vm.mostrarFormulario = true;
                vm.mostrarFormularioPerfilamiento = true;
                vm.activarFormularioPerfilamiento = true;
                vm.activarFormulario = true;
                vm.btnInvitarActivo = true;

                vm.validarFormulario();
                vm.mostrarMensaje1 = true;
                //utilidades.mensajeSuccess(
                //    "Para invitar al usuario al PIIP, completar el diligenciamiento del siguiente formulario.",
                //    false,
                //    null,
                //    null,
                //    "Usuario no existente en el PIIP"
                //);
                return;
            }

        }

        vm.verificarApellidos = function () {
            if (vm.model.apellidos == '') {
                vm.activarFormulario = true;
            }
        }

        vm.obtenerListadoPerfilesXEntidad = function (idEntidad) {
            var promise = actions.obtenerListadoPerfilesXEntidad(idEntidad);
            promise.then(function (response) {
                vm.listaPerfilesBackbone = response.data;
                if (vm.listaPerfilesBackbone.length > 0) {

                    let IdUsuario = vm.usuarioPIIP.IdUsuario;
                    let IdEntidadUsuario = vm.model.idEntidad;
                    if (IdUsuario != '') {
                        vm.obtenerListadoPerfilesXEntidadYUsuario(IdEntidadUsuario, IdUsuario);
                    }
                    vm.BanderaDivPerfiles2 = true;
                }
            }, function (error) {
                console.log("error", error);
            });
        }
        vm.ListaPerfilesUsuario = [];
        vm.ValidarPerfilyaregistra = function (IdPerfil) {

            //var rueba = vm.listaPerfilesUsuarioBackbone.filter(x => x.Id == IdPerfil);
            //if (rueba.length > 0) {
            //    return true            
            //}
            if (vm.ListaPerfilesUsuario.indexOf(IdPerfil) === -1) {
                return false
            } else if (vm.ListaPerfilesUsuario.indexOf(IdPerfil) > -1) {
                return true
            }


        }
        vm.toggleCurrency = function (IdPerfil) {
            if ($('#chk-' + IdPerfil).is(':checked')) {
                vm.ListaPerfilesUsuario.push(IdPerfil);
            } else {
                var toDel = vm.ListaPerfilesUsuario.indexOf(IdPerfil);
                vm.ListaPerfilesUsuario.splice(toDel, 1);
            }
            vm.validarPerfil();
        };
        vm.obtenerListadoPerfilesXEntidadYUsuario = function (idEntidad, idUsuario) {
            var promise = actions.obtenerListadoPerfilesXEntidadYUsuario(idEntidad, idUsuario);
            promise.then(function (response) {
                var prueba = response.data;
                vm.listaPerfilesUsuarioBackbone = prueba;//prueba.filter(x => x.IdEntidad == idEntidad)[0].Perfiles;
                vm.ListaPerfilesUsuario = [];
                vm.listaPerfilesUsuarioBackbone.forEach(function (item, i) {
                    var banderaid = vm.listaPerfilesBackbone.filter(x => x.IdPerfil == item.IdPerfil)[0];
                    if (banderaid != undefined) {
                        vm.ListaPerfilesUsuario.push(item.IdPerfil);
                    }

                });
                vm.validarPerfil();
            }, function (error) {
                console.log("error", error);
            });
        }

        vm.obtenerEntidad = function (id) {
            return vm.listaEntidades.filter(x => x.Id == id)[0];
        }

        vm.obtenerPerfil = function (id) {
            /*     return vm.listaPerfilesBackbone.filter(x => x.IdPerfil == id)[0];*/
            var NombresPerfil = '';
            if (vm.listaPerfilesBackbone.length > 0) {

                vm.ListaPerfilesUsuario.forEach(function (item, i) {
                    NombresPerfil = NombresPerfil + vm.listaPerfilesBackbone.filter(x => x.IdPerfil == item)[0].NombrePerfil + ', ';
                });
            }
            return NombresPerfil == '' ? 'Ninguno' : NombresPerfil;
        }

        vm.invitarUsuario = function () {

            vm.activarCorreo = false;

            let modelo = {
                tipoIdentificacion: vm.model.tipoIdentificacion,
                identificacion: vm.model.identificacion,
                correo: vm.model.correoInstitucional,
                nombre: vm.model.nombres,
                apellido: vm.model.apellidos,
                idEntidad: vm.model.idEntidad,
                nombreEntidad: vm.obtenerEntidad(vm.model.idEntidad).Nombre,
                idPerfilBackbone: vm.ListaPerfilesUsuario,
                nombrePerfil: vm.obtenerPerfil(vm.model.idPerfilBackbone),
                IdUsuarioDNP: vm.model.tipoIdentificacion + vm.model.identificacion,
                tieneModuloAdministracion: false,
                tieneModuloBackbone: true,
                tipoInvitacion: vm.encontroUsuarioSTSAplicacion ? 1 : 0
            };

            console.log('PIIP parametros: ', modelo)

            actions.registrarUsuarioPIIP(modelo);
            
            // Si no encontro usuario en STS para la aplicación crear o relacionar usuario
            if (!vm.encontroUsuarioSTSAplicacion) {

                let usuarioSTSGestionado = false
                let parametros = {
                    pAplicacion: constantesBackbone.keySTSApplication,
                    pTipoDocumento: vm.model.tipoIdentificacion,
                    pNumeroDocumento: vm.model.identificacion,
                    pUsuario: vm.model.nombres + " " + vm.model.apellidos,
                    pPassword: utilidades.generatePasswordRand(8, 'rand'),
                    pCorreo: vm.model.correoInstitucional,
                    nombreEntidad: vm.model.idEntidad ? vm.obtenerEntidad(vm.model.idEntidad).Nombre : '',
                    nombrePerfil: vm.ListaPerfilesUsuario ? vm.obtenerPerfil(vm.model.idPerfilBackbone) : '',
                    pPlantilla: ''
                }

                console.log('STS parametros: ', parametros)

                // Si encontro usuario en el STS con relación aplicación confiable relacionar usuario
                if (vm.encontroUsuarioSTSAplicacionConfiable && !usuarioSTSGestionado) {
                    parametros.pPlantilla = 'usuarioExternoExistenteAppConfiable'
                    //if (vm.encontroUsuarioPIIP) {
                    //    parametros.pPlantilla = 'usuarioExternoExistenteAppConfiableConPIIP'
                    //}
                    if (!vm.encontroUsuarioPIIP) {
                        parametros.pPlantilla = ''
                    }
                    actions.registrarUsuarioAPPSTS(parametros)
                    usuarioSTSGestionado = true
                }

                // Si encontro usuario en el STS sin relación aplicación confiable relacionar usuario y asignar contraseña
                if (vm.encontroUsuarioSTS && !usuarioSTSGestionado) {
                    parametros.pPlantilla = 'usuarioExternoExistenteAppNoConfiable'
                    //if (vm.encontroUsuarioPIIP) {
                    //    parametros.pPlantilla = 'usuarioExternoExistenteAppNoConfiableConPIIP'
                    //}
                    if (!vm.encontroUsuarioPIIP) {
                        parametros.pPlantilla = ''
                    }
                    actions.registrarUsuarioAPPSTS(parametros)
                    usuarioSTSGestionado = true
                }

                //Si no encontro usuario en el STS crear usuario y asignar contraseña 
                if (!usuarioSTSGestionado) {
                    parametros.pPlantilla = 'usuarioExternoNuevo'
                    //if (vm.encontroUsuarioPIIP) {
                    //    parametros.pPlantilla = 'usuarioExternoNuevoConPIIP'
                    //}
                    //if (!vm.encontroUsuarioPIIP) {
                    //    parametros.pPlantilla = ''
                    //}
                    actions.registrarUsuarioSTS(parametros)
                    usuarioSTSGestionado = true
                }

            }

            // Si no encontro usuario en STS para la aplicación crear o relacionar usuario
            if (!vm.encontroUsuarioSTSAplicacionMGA) {
                let parametros = {
                    pAplicacion: constantesBackbone.keySTSApplicationMGA,
                    pTipoDocumento: vm.model.tipoIdentificacion,
                    pNumeroDocumento: vm.model.identificacion,
                    pUsuario: vm.model.nombres + " " + vm.model.apellidos,
                    pPassword: utilidades.generatePasswordRand(8, 'rand'),
                    pCorreo: vm.model.correoInstitucional,
                    nombreEntidad: '',
                    nombrePerfil: '',
                    pPlantilla: ''
                }

                $timeout(function () { actions.registrarUsuarioAPPSTS(parametros) }, 5000);
                
            }
            vm.mostrarMensaje3 = false;
            vm.mostrarMensaje1 = false;
            vm.mostrarMensaje2 = true;
           
            vm.activarFormulario = false
            vm.activarFormularioPerfilamiento = false;
            vm.activarBotonInvitarPIIPNoSTS = false
            vm.btnInvitarActivo = false;
            vm.BanderaDivPerfiles2 = false;
            vm.BanderaDivPerfiles = false;

        }

    }

    angular.module('backbone.usuarios').controller('modalInvitarUsuarioExternoController', modalInvitarUsuarioExternoController);
})();
                        