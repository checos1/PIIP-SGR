(function () {
    'use strict';

    modalPerfilesUsuarioTerritorioController.$inject = [
        'obj',
        'servicioEntidades',
        '$uibModalInstance',
        'constantesBackbone',
        'servicioUsuarios',
        'utilidades',
    ];

    function modalPerfilesUsuarioTerritorioController(
        obj,
        servicioEntidades,
        $uibModalInstance,
        constantesBackbone,
        servicioUsuarios,
        utilidades,
    ) {
        var vm = this;

        /// models
        vm.cerrar = $uibModalInstance.dismiss;
        vm.guardar = GuardarUsuariosPerfiles;
        vm.IdUsuario = obj.Id;
        vm.nombre = '';
        vm.apelido = '';
        vm.idEntidad = obj.idEntidad;
        vm.nombreEntidad = obj.nombreEntidad;
        vm.idUsuarioDNP = obj.IdUsuarioDNP;
        vm.Nombres = obj.Nombre;
        vm.Correo = obj.Correo;
        vm.Activo = obj.Activo;
        vm.EstadoUsuario = '';
        vm.TipoUsuario = "";

        vm.esTipoIdentificacionValido = false;
        vm.esIdentificacionValido = false;
        vm.esNombresValido = false;
        vm.esApellidosValido = false;
        vm.esEntidadValido = false;
        vm.registrarUsuarioPIIP = registrarUsuarioPIIP;
        vm.enviarCorreoInvitacion = enviarCorreoInvitacion;

        vm.resourceGroupId = 5;

        /// Comienzo
        vm.init = function () {
            if (obj) {
                var nombreSplit = obj.Nombre.split(' ');
                var len = nombreSplit.length;

                vm.nombre = nombreSplit.slice(0, -(len - 1)).join(' ');
                if (len > 1) {
                    vm.apelido = nombreSplit.slice(1).join(' ');
                } else {
                    vm.apelido = '';
                }
                if (vm.Activo == true) {
                    vm.EstadoUsuario = "Activo";
                }
                else {
                    vm.EstadoUsuario = "Inactivo";
                }
                if (vm.Correo != null) {
                    let esCorreoInstitucional = vm.Correo.toUpperCase().includes("DNP.GOV.CO");

                    if (esCorreoInstitucional) {
                        vm.TipoUsuario = "Usuario DNP"
                    }
                    else {
                        vm.TipoUsuario = "Usuario Externo"
                    }
                }

            }
            if (vm.idEntidad) {
                //vm.obtenerListadoPerfilesXEntidad(vm.idEntidad);
                vm.obtenerListadoPerfilesXEntidadBanco(vm.idEntidad,vm.resourceGroupId);
                vm.validarTipoIdentificacion();
                vm.validarIdentificacion();
                vm.validarNombres();
                vm.validarApellidos();
                vm.validarCorreoInstitucional();
                vm.validarEntidad();
            }
        }
        vm.ListaPerfilesUsuario = [];
        vm.activarBotonInvitar = function () {
            return vm.esFormularioValido;
        }
        vm.obtenerListadoPerfilesXEntidadBanco = function (idEntidad) {
            //var promise = servicioEntidades.obtenerListadoPerfilesXEntidad(idEntidad);
            var promise = servicioEntidades.obtenerListadoPerfilesXEntidadBanco(idEntidad, vm.resourceGroupId);
            promise.then(function (response) {
                vm.listaPerfilesBackbone = response.data;
                vm.ListaPerfilesUsuario = [];
                let IdUsuario = vm.IdUsuario;
                if (IdUsuario != '') {
                    vm.obtenerListadoPerfilesXEntidadYUsuario(idEntidad, IdUsuario);
                }

            }, function (error) {
                console.log("error", error);
            });
        }




        vm.obtenerListadoPerfilesXEntidadYUsuario = function (idEntidad, idUsuario) {
            var promise = servicioUsuarios.obtenerListadoPerfilesXEntidadYUsuario(idEntidad, idUsuario);
            promise.then(function (response) {
                var prueba = response.data;
                vm.listaPerfilesUsuarioBackbone = prueba;
                vm.ListaPerfilesUsuario = [];
                vm.listaPerfilesUsuarioBackbone.forEach(function (item, i) {
                    var banderaid = vm.listaPerfilesBackbone.filter(x => x.IdPerfil == item.IdPerfil)[0];
                    if (banderaid != undefined) {
                        vm.ListaPerfilesUsuario.push(item.IdPerfil);
                    }

                });
            }, function (error) {
                console.log("error", error);
            });
        }


        vm.ValidarPerfilyaregistra = function (IdPerfil) {

            if (vm.ListaPerfilesUsuario.indexOf(IdPerfil) === -1) {
                return false
            } else if (vm.ListaPerfilesUsuario.indexOf(IdPerfil) > -1) {
                return true
            }


        }

        vm.validarTipoIdentificacion = function () {
            vm.esTipoIdentificacionValido = false;

            let esCampoValido = utilidades.isNotNull(obj.TipoIdentificacion);

            if (esCampoValido) {
                vm.esTipoIdentificacionValido = true
            }
            vm.validarFormulario();
        }
        vm.validarIdentificacion = function () {
            vm.esIdentificacionValido = false;

            let esCampoValido = utilidades.isNotNull(obj.Identificacion);

            if (esCampoValido) {
                vm.esIdentificacionValido = true
            }
            vm.validarFormulario();
        }
        vm.validarNombres = function () {
            vm.esNombresValido = false;

            let esCampoValido = utilidades.isNotNull(vm.nombre);

            if (esCampoValido) {
                vm.esNombresValido = true
            }
            vm.validarFormulario();
        }
        vm.validarApellidos = function () {
            vm.esApellidosValido = false;

            let esCampoValido = utilidades.isNotNull(vm.apelido);

            if (esCampoValido) {
                vm.esApellidosValido = true
            }
            vm.validarFormulario();
        }
        vm.validarEntidad = function () {
            vm.esEntidadValido = false;

            let esCampoValido = utilidades.isNotNull(obj.idEntidad);

            if (esCampoValido) {
                vm.esEntidadValido = true
            }

            vm.validarFormulario();
        }
        vm.validarCorreoInstitucional = function () {

            vm.esCorreoInstitucionalValido = false;
            let esCorreoValido = utilidades.validarEmail(obj.Correo);

            if (esCorreoValido) {
                vm.esCorreoInstitucionalValido = true;
            }
            vm.validarFormulario();

        }


        vm.validarFormulario = function () {

            vm.esFormularioValido = false;
            if (
                vm.esCorreoInstitucionalValido
                && vm.esTipoIdentificacionValido
                && vm.esIdentificacionValido
                && vm.esNombresValido
                && vm.esApellidosValido
                && vm.esEntidadValido
            ) {
                vm.esFormularioValido = true;
            }
        }
        function GuardarUsuariosPerfiles() {

            let modelo = {
                tipoIdentificacion: obj.TipoIdentificacion,
                identificacion: obj.Identificacion,
                correo: obj.Correo,
                nombre: vm.nombre,
                apellido: vm.apelido,
                idEntidad: obj.idEntidad,
                nombreEntidad: obj.nombreEntidad,
                idPerfilBackbone: vm.ListaPerfilesUsuario,
                nombrePerfil: vm.obtenerPerfil(vm.ListaPerfilesUsuario),
                IdUsuarioDNP: obj.IdUsuarioDNP,
                tieneModuloAdministracion: false,
                tieneModuloBackbone: true,
                tipoInvitacion: 1
            };

            vm.registrarUsuarioPIIP(modelo);
        }
        vm.toggleCurrency = function (IdPerfil) {
            if ($('#chk-' + IdPerfil).is(':checked')) {
                vm.ListaPerfilesUsuario.push(IdPerfil);
            } else {
                var toDel = vm.ListaPerfilesUsuario.indexOf(IdPerfil);
                vm.ListaPerfilesUsuario.splice(toDel, 1);
            }
        };
        vm.obtenerPerfil = function (id) {
            var NombresPerfil = '';
            if (vm.listaPerfilesBackbone.length > 0) {

                vm.ListaPerfilesUsuario.forEach(function (item, i) {
                    NombresPerfil = NombresPerfil + vm.listaPerfilesBackbone.filter(x => x.IdPerfil == item)[0].NombrePerfil + ', ';
                });
            }
            return NombresPerfil == '' ? 'Ninguno' : NombresPerfil;
        }



        function guardarUsuario() {
            var dto = {
                Nombre: vm.nombre + ' ' + vm.apelido,
                IdUsuario: vm.IdUsuario
            };

            servicioUsuarios.guardarUsuario(dto)
                .then(function (response) {
                    if (response.data) {
                        utilidades.mensajeSuccess("Operación realizada con éxito!", false, false, false);
                        vm.cerrar();
                    } else
                        utilidades.mensajeError(response.data.Mensaje, false);
                });
        }


        function registrarUsuarioPIIP(modelo) {

            servicioUsuarios.registrarUsuarioPIIP(modelo)
                .then(function (response) {
                    let exito = response.data;
                    if (utilidades.isTrue(exito)) {
                        let prepararCorreo = {}
                        if (modelo.correo.includes("@dnp.gov.co")) {
                            prepararCorreo = {
                                pAplicacion: constantesBackbone.keySTSApplication,
                                pTipoDocumento: modelo.tipoIdentificacion,
                                pNumeroDocumento: modelo.identificacion,
                                pAsunto: "Invitación usuarios PIIP",
                                pEntidad: modelo.nombreEntidad,
                                pPerfil: modelo.nombrePerfil,
                                pCorreo: modelo.correo,
                                pUsuario: modelo.nombre + " " + modelo.apellido,
                                pPassword: "No aplica",
                                pPlantilla: "usuarioDNP"
                            }

                            vm.enviarCorreoInvitacion(prepararCorreo);

                        } else {
                            prepararCorreo = {
                                pAplicacion: constantesBackbone.keySTSApplication,
                                pTipoDocumento: modelo.tipoIdentificacion,
                                pNumeroDocumento: modelo.identificacion,
                                pAsunto: "Invitación usuarios PIIP",
                                pEntidad: modelo.nombreEntidad,
                                pPerfil: modelo.nombrePerfil,
                                pCorreo: modelo.correo,
                                pUsuario: modelo.nombre + " " + modelo.apellido,
                                pPassword: modelo.tipoInvitacion == 1 ? "No aplica" : utilidades.generatePasswordRand(8, 'rand'),
                                pPlantilla: modelo.tipoInvitacion == 1 ? "usuarioExternoExistenteAppConfiable" : "usuarioExternoExistenteAppNoConfiable"
                            }
                            if (modelo.tipoInvitacion) {
                                vm.enviarCorreoInvitacion(prepararCorreo);
                            }
                        }

                    }
                    else {
                        utilidades.mensajeError("Error al realizar la operación", false);
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
        function enviarCorreoInvitacion(modelo) {

            if (modelo.pPlantilla != '') {
                servicioUsuarios.enviarCorreoInvitacionSTS(modelo)
                    .then(function (response) {
                        let exito = response.data;
                        console.log('enviarCorreoInvitacionSTS response.data ', response.data)
                        utilidades.mensajeSuccess("El usuario recibirá un mensaje por correo electrónico confirmándole las novedades de sus perfiles en la PIIP.", false, false, false, "La invitación fue enviada con éxito!");
                        vm.cerrar();
                        console.log('enviar correo')
                    })
                    .catch(error => {
                        if (error.status == 400) {
                            utilidades.mensajeError(error.data.Message || "Error al realizar la operación");
                            return;
                        }

                        utilidades.mensajeError("Error al realizar la operación");
                    });
            }

        }


    }

    angular.module('backbone.usuarios').controller('modalPerfilesUsuarioTerritorioController', modalPerfilesUsuarioTerritorioController);
})();