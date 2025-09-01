(function () {
    'use strict';

    modalInvitarUsuarioController.$inject = [
        'objInvitarUsuario',
        '$uibModalInstance',
        'servicioUsuarios',
        'utilidades'
    ];

    function modalInvitarUsuarioController(
        objInvitarUsuario,
        $uibModalInstance,
        servicioUsuarios,
        utilidades
    ) {
        var vm = this;

        vm.init = init;
        vm.cerrar = $uibModalInstance.dismiss;
        vm.submitInvitarUsuario = submitInvitarUsuario;
        vm.tiposIdentificacion = ['Cédula', 'NIT', 'Pasaporte'];
        vm.buscarDirectorio = buscarDirectorio;
        vm.validarCorreo = validarCorreo;
        vm.LstTipoDocumento = LstTipoDocumento;
        vm.IdentificacionCambio = IdentificacionCambio;

        function init() {

            vm.model = {
                modulos: {
                    administracion: false,
                    backbone: true
                }
            }

            obtenerListadoEntidadesXUsuarioAutenticado();
        //obtenerPerfilesBackBone();
        //obtenerEntidades();
        }

        function obtenerListadoEntidadesXUsuarioAutenticado() {
            servicioEntidades.obtenerListadoEntidadesXUsuarioAutenticado()
                .then(function (response) {
                    console.log(response.data)
                    vm.listaEntidades = response.data;
                }, function (error) {
                    console.log("error: obtenerListadoEntidadesXUsuarioAutenticado", error);
                });
        }

        function obtenerEntidades() {
            servicioUsuarios.obtenerEntidades(objInvitarUsuario.tipoEntidad)
                .then(function (response) {
                    vm.listaEntidades = response.data;
                }, function (error) {
                    console.log("error", error);
                });
        }

        function obtenerPerfilesBackBone() {
            var parametros = {
                usuario: usuarioDNP,
                aplicacion: 'backbone',
            }

            servicioUsuarios.obtenerPerfilesAutorizadosPorAplicacion(parametros)
                .then(function (response) {
                    vm.listaPerfilesBackbone = response.data;
                }, function (error) {
                    console.log("error", error);
                });
        }

        function submitInvitarUsuario() {
            if (!vm.model.modulos.backbone) {
                vm.model.idPerfilBackbone = null;
                vm.model.idEntidad = null;
            }

            var params = {
                nombre: vm.model.nombre,
                apellido: vm.model.apellido,
                correo: vm.model.correo,
                idEntidad: vm.model.idEntidad,
                tieneModuloAdministracion: vm.model.modulos.administracion,
                tieneModuloBackbone: vm.model.modulos.backbone,
                idPerfilBackbone: vm.model.idPerfilBackbone,
                tipoIdentificacion: vm.model.tipoIdentificacion,
                identificacion: vm.model.identificacion,
                IdUsuarioDNP: vm.model.usuario
            };

            if ((!params.tieneModuloBackbone && !params.tieneModuloAdministracion)) {
                utilidades.mensajeError("Seleccione al menos un módulo.", false);
                return false;
            }
            else if (!params.correo) {
                utilidades.mensajeError("Verifique el correo.", false); return false;
            }
            else if (!params.nombre || !params.apellido) {
                utilidades.mensajeError("Verifique el nombre y apellido.", false); return false;
            }
            else if (!params.tipoIdentificacion || !params.identificacion) {
                utilidades.mensajeError("Verifique el tipo y numero de identificación.", false); return false;
            }
            else if (params.tieneModuloBackbone && (!params.idPerfilBackbone || !params.idEntidad)) {
                utilidades.mensajeError("Verifique la Entidad y el Perfil de acceso.", false); return false;
            }

            servicioUsuarios.invitarUsuario(params)
                .then(function (response) {
                    let exito = response.data;
                    if (exito) {
                        utilidades.mensajeSuccess("Operación realizada con éxito!", false, false, false);
                        $uibModalInstance.close();
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

        function buscarDirectorio() {

            nombre.value = "";
            apellido.value = "";
            inputIdentificacion.value = "";
            usuario.value = "";
            vm.model.nombre = "";
            vm.model.apellido = "";
            vm.model.identificacion = "";
            vm.model.usuario = "";

            servicioUsuarios.ObtenerUsuarioDominio(correo.value).then(function (response) {
                if (!response.data)
                    return;
                const data = response.data;
                nombre.value = data.Nombre;
                apellido.value = data.Apellido;
                vm.model.nombre = data.Nombre;
                vm.model.apellido = data.Apellido;

                if (nombre.value == "")
                    utilidades.mensajeError("El usuario no existe, verificar los datos suministrados.");

            }, function (error) {
                console.log(error);
                toastr.error("Hubo un error al cargar las informaciones del usuario");
            });

        }

        function validarCorreo() {
            if (correo.value.length > 5 && (correo.value.toUpperCase().includes("DNP.GOV.CO"))) {
                $("#buscarDir").css("display", "block");
                $('#nombre')[0].disabled = true;
                $('#apellido')[0].disabled = true;
            }
            else {
                $("#buscarDir").css("display", "none");
                $('#nombre')[0].disabled = false;
                $('#apellido')[0].disabled = false;
            }
        }

        function LstTipoDocumento() {
            if (!vm.model.tipoIdentificacion)
                return;

            var tempIdentificacion = "";
            if (vm.model.identificacion)
                tempIdentificacion = vm.model.identificacion;

            var tipoId = vm.model.tipoIdentificacion;

            switch (vm.model.tipoIdentificacion) {
                case "Cédula":
                    usuario.value = "CC" + tempIdentificacion;
                    break;
                case "NIT":
                    usuario.value = "NI" + tempIdentificacion;
                    break;
                case "Pasaporte":
                    usuario.value = "PA" + tempIdentificacion;
                    break;
                default:
                    usuario.value = tipoId.substring(0, 2).toUpperCase() + tempIdentificacion;
                    break;
            }

        }

        function IdentificacionCambio() {
            var tempIdentificacion = "";
            if (vm.model.identificacion)
                tempIdentificacion = vm.model.identificacion;

            var tipoId = vm.model.tipoIdentificacion;

            if (tipoId)
                switch (vm.model.tipoIdentificacion) {
                    case "Cédula":
                        usuario.value = "CC" + tempIdentificacion;
                        vm.model.usuario = "CC" + tempIdentificacion;
                        break;
                    case "NIT":
                        usuario.value = "NI" + tempIdentificacion;
                        vm.model.usuario = "NI" + tempIdentificacion;
                        break;
                    case "Pasaporte":
                        usuario.value = "PA" + tempIdentificacion;
                        vm.model.usuario = "NI" + tempIdentificacion;
                        break;
                    default:
                        usuario.value = tipoId.substring(0, 2).toUpperCase() + tempIdentificacion;
                        vm.model.usuario = tipoId.substring(0, 2).toUpperCase() + tempIdentificacion;
                        break;
                }
            else {
                usuario.value = tempIdentificacion;
                vm.model.usuario = tempIdentificacion;
            }

        }

        function getIdEntidad(nombreEntidad) {
            for (var ls = 0; ls < vm.listaEntidades.length; ls++) {
                if (vm.listaEntidades[ls].Nombre == nombreEntidad) {
                    vm.idEntidad = vm.listaEntidades[ls].Id;
                    break;
                }
                else
                    vm.model.idEntidad = null;
            }
        }
    }

    angular.module('backbone.usuarios').controller('modalInvitarUsuarioController', modalInvitarUsuarioController);
})();
