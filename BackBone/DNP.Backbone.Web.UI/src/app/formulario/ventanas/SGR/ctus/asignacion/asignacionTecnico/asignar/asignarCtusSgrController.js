(function () {
    'use strict';
    asignarCtusSgrController.$inject = [
        'utilidades',
        '$sessionStorage',
        'justificacionCambiosServicio',
        'ctusAsignacionSgrServicio',
        'solicitarCtusSgrServicio'
    ];

    function asignarCtusSgrController(
        utilidades,
        $sessionStorage,
        justificacionCambiosServicio,
        ctusAsignacionSgrServicio,
        solicitarCtusSgrServicio
    ) {
        var vm = this;
        vm.user = {};
        vm.lang = "es";
        vm.nombreComponente = 'sgrctusasignacionasignar';
        vm.seccionCapitulo = null; //para guardar los capitulos modificados y que se llenen las lunas
        vm.tipoVisualizacionAsignar = 'tipo1';
        vm.actualizadoDelegar = false;
        vm.Bpin = $sessionStorage.idObjetoNegocio;
        vm.IdNivel = $sessionStorage.idNivel;
        vm.idInstancia = $sessionStorage.idInstancia;
        vm.idAccion = $sessionStorage.idAccion;
        vm.proyectoId = $sessionStorage.proyectoId;
        vm.idInstanciaPadre = $sessionStorage.InstanciaSeleccionada != undefined ? $sessionStorage.InstanciaSeleccionada.InstanciaPadreId : null;
        vm.ProyectoCtusId = null;
        vm.disabledAsig = false;
        vm.activar = true;
        vm.desactivar = true
        vm.usuarioEncargado = '';
        vm.data = {
            IdUsuarioDNP: ''
        };

        vm.init = function () {
            vm.notificacionvalidacion({ handler: vm.notificacionValidacionPadre, nombreComponente: vm.nombreComponente, esValido: true });
            ObtenerProyectoCtus(vm.proyectoId, vm.idInstanciaPadre);
            vm.disabledAsig = $sessionStorage.soloLectura;
        };

        ctusAsignacionSgrServicio.registrarObservador(function (datos) {
            if (datos.actualizarCapitulo === 'true') {
                guardarCapituloModificado();
            } else if (datos.tipoVisualizacion !== undefined)
                vm.tipoVisualizacionAsignar = datos.tipoVisualizacion;
            vm.actualizadoDelegar = true;
        });

        function ObtenerProyectoCtus(proyectoId, instanciaId) {
            solicitarCtusSgrServicio.SGR_Proyectos_LeerProyectoCtus(proyectoId, instanciaId)
                .then(function (response) {
                    if (response.data != null) {
                        vm.ProyectoCtusId = response.data.id;
                        vm.IdRol = response.data.RolTecnicoId;
                        vm.IdEntidad = response.data.EntidadDestino;
                        ObtenerUsuariosEncargadosCtus();
                    }
                }, function (error) {
                    utilidades.mensajeError('Ocurrió un problema al leer la información del CTUS.');
                    return "";
                });
        }

        function ObtenerUsuariosEncargadosCtus() {
            var parametros = {
                EntidadId: vm.IdEntidad,
                RolId: vm.IdRol,
                InstanciaId: vm.idInstancia
            }
            ctusAsignacionSgrServicio.ObtenerUsuariosEncargadosCtus(parametros)
                .then(function (response) {
                    if (response.data != null) {
                        vm.usuariosEncargados = response.data;
                        ctusAsignacionSgrServicio.SGR_CTUS_LeerProyectoCtusUsuarioEncargado(vm.ProyectoCtusId, vm.idInstancia)
                            .then(function (response) {
                                if (response.data) {
                                    if (!$sessionStorage.soloLectura)
                                        vm.disabledAsig = false;
                                    else
                                        vm.disabledAsig = true;
                                    vm.usuarioEncargado = response.data;
                                    vm.data.IdUsuarioDNP = vm.usuarioEncargado.UsuarioEncargado;
                                    vm.tipoVisualizacionAsignar = 'tipo2';
                                    ctusAsignacionSgrServicio.notificarCambio({ usuarioDelegado: 'true' });
                                    ctusAsignacionSgrServicio.notificarCambio({ reqEntidad: false });
                                }
                            }, function (error) {
                                utilidades.mensajeError(error);
                            });
                    } else {
                        if (!vm.actualizadoDelegar)
                            vm.tipoVisualizacionAsignar = 'tipo1';
                    }
                }, function (error) {
                    utilidades.mensajeError('Ocurrió un problema al obtener la inforamción de los usuarios encargados del CTUS.');
                    return;
                });
        }

        vm.ActivarEditar = function () {
            if (vm.activar == true) {
                $("#EditarG").html("CANCELAR");
                vm.activar = false;
                ctusAsignacionSgrServicio.notificarCambio({ usuarioDelegado: 'true' });
                if (vm.usuarioEncargado != '') {
                    vm.data.IdUsuarioDNP = vm.usuarioEncargado.UsuarioEncargado;
                    vm.datosUsuarioSeleccionado();
                }
            }
            else {
                utilidades.mensajeWarning("¿Está seguro de continuar?", function funcionContinuar() {
                    OkCancelar();
                    if (vm.usuarioEncargado != '')
                        vm.data.IdUsuarioDNP = vm.usuarioEncargado.UsuarioEncargado;
                    else {
                        vm.data.IdUsuarioDNP = '';
                        ctusAsignacionSgrServicio.notificarCambio({ usuarioDelegado: 'false' });
                    }
                    $("#EditarG").html("EDITAR");
                    vm.activar = true;
                    $("#ddlusuariosEncargadosSgr").val("");
                }, function funcionCancelar(reason) {
                    return;
                }, null, null, "Los datos que posiblemente haya diligenciado se perderán");
            }
        }

        function OkCancelar() {
            setTimeout(function () {
                utilidades.mensajeSuccess("", false, false, false, "La edición ha sido cancelada con éxito.");
            }, 500);
        }

        vm.datosUsuarioSeleccionado = function () {
            vm.usuarioSeleccionado = vm.usuariosEncargados.find(x => x.IdUsuarioDNP == vm.data.IdUsuarioDNP);
        }

        vm.guardar = function () {
            if (vm.data.IdUsuarioDNP === null || vm.data.IdUsuarioDNP === undefined || vm.data.IdUsuarioDNP === "") {
                utilidades.mensajeError('El campo Funcionario encargado de la elaboración del CTUS es obligatorio', false);
                return;
            }

            var data = {
                ProyectoCtusId: vm.ProyectoCtusId,
                InstanciaId: vm.idInstancia,
                UsuarioEncargado: vm.data.IdUsuarioDNP,
                RolUsuarioEncargadoId: vm.IdRol,
                Tipo: 'ctus'
            }

            Guardar(data);
        }

        function Guardar(data) {
            return ctusAsignacionSgrServicio.SGR_CTUS_GuardarAsignacionUsuarioEncargado(data).then(
                function (response) {
                    if (response.data || response.statusText === "OK") {
                        if (response.data.Exito) {
                            parent.postMessage("cerrarModal", "*");
                            guardarCapituloModificado();
                            utilidades.mensajeSuccess(`Se ha asignado la elaboración del CTUS a ${vm.usuarioSeleccionado.NombreUsuario} - ${vm.usuarioSeleccionado.Correo}`, false, false, false, "Los datos fueron guardados con éxito.");
                            $("#EditarG").html("EDITAR");
                            vm.activar = true;
                            vm.disabledAsig = true;
                        } else {
                            swal('', response.data.Mensaje, 'warning');
                        }

                    } else {
                        swal('', "Error al realizar la operación", 'error');
                    }
                }
            );
        }

        //para guardar los capitulos modificados y que se llenen las lunas
        function guardarCapituloModificado() {
            ObtenerSeccionCapitulo();
            var data = {
                ProyectoId: $sessionStorage.InstanciaSeleccionada.ProyectoId,
                Justificacion: "",
                SeccionCapituloId: vm.seccionCapitulo,
                InstanciaId: vm.idInstancia,
                Modificado: 1,
                cuenta: 1
            }
            justificacionCambiosServicio.guardarCambiosFirme(data)
                .then(function (response) {
                    if (response.data.Exito) {
                        vm.guardadoevent({ nombreComponenteHijo: vm.nombreComponente });
                    }
                    else {
                        utilidades.mensajeError(response.data.Mensaje + " Capitulo Modificado");
                    }
                });
        }

        //para guardar los capitulos modificados y que se llenen las lunas
        function ObtenerSeccionCapitulo() {
            const span = document.getElementById('id-capitulo-' + vm.nombreComponente);
            vm.seccionCapitulo = span.textContent;
        }

        /* ------------------------ Validaciones ---------------------------------*/

        vm.notificacionValidacionPadre = function (errores) {
            //Remplazar por cada capitulo
            var tipError = 'ASIGCTUS';
            vm.limpiarErrores();
            if (errores != undefined) {
                var erroresRelacionconlapl = errores.find(p => (p.Seccion + p.Capitulo) == vm.nombreComponente);
                var isValid = true;
                if (erroresRelacionconlapl != undefined) {
                    var erroresJson = erroresRelacionconlapl.Errores == "" ? [] : JSON.parse(erroresRelacionconlapl.Errores);
                    isValid = (erroresJson == null || erroresJson.length == 0);
                    if (!isValid) {
                        erroresJson[vm.nombreComponente].forEach(p => {
                            var nameArr = p.Error.split('-');
                            var TipoError = nameArr[0].toString();
                            if (TipoError == tipError) {
                                vm.validarSeccion(TipoError, nameArr[1].toString(), p.Descripcion, false);
                            }
                            else {
                                vm.validarValores(nameArr[0].toString(), p.Descripcion, false);
                            }
                        });
                    }
                }
                else {
                    var idSpanAlertComponentAlert = document.getElementById("alert-" + vm.nombreComponente);
                    var idSpanAlertComponent = document.getElementById(tipError + vm.nombreComponente)
                    if (idSpanAlertComponent != null)
                        idSpanAlertComponent.classList.add('hidden');

                    if (idSpanAlertComponentAlert != null)
                        idSpanAlertComponentAlert.classList.remove("ico-advertencia");
                }
                vm.notificacionestado({ nombreComponente: vm.nombreComponente, esValido: isValid });
            }
        }

        vm.validarValores = function (pregunta, errores, esValido) {
            var idSpanAlertComponent = document.getElementById("alert-" + vm.nombreComponente + pregunta);
            if (idSpanAlertComponent != undefined) {
                if (!esValido) {
                    idSpanAlertComponent.classList.add("ico-advertencia");
                } else {
                    idSpanAlertComponent.classList.remove("ico-advertencia");
                }
            }
        }

        vm.validarSeccion = function (tipoError, seccion, errores, esValido) {
            var campomensajeerror = document.getElementById(tipoError + seccion);
            if (campomensajeerror != undefined) {
                if (!esValido) {
                    campomensajeerror.innerHTML = "<span class='d-inline-block ico-advertencia'></span><span class='pl-2'>" + errores + "</span>";
                    campomensajeerror.classList.remove('hidden');
                } else {
                    campomensajeerror.classList.remove("ico-advertencia");
                }
            }
        }

        vm.limpiarErrores = function () {
            var errorElements = document.getElementsByClassName('errorSeccionInformacionGeneralViabilidad');
            var testDivs = Array.prototype.filter.call(errorElements, function (errorElement) {
                errorElement.innerHTML = "";
                errorElement.classList.add('hidden');
            });
        }
    }


    angular.module('backbone').component('asignarCtusSgr', {
        templateUrl: "/src/app/formulario/ventanas/SGR/ctus/asignacion/asignacionTecnico/asignar/asignarCtusSgr.html",
        controller: asignarCtusSgrController,
        controllerAs: "vm",
        bindings: {
            callback: '&',
            guardadoevent: '&',
            notificacionvalidacion: '&',
            notificacionestado: '&',
            namecomponent: '<'
        }
    })
        .directive('stringToNumber', function () {
            return {
                require: 'ngModel',
                link: function (scope, element, attrs, ngModel) {
                    ngModel.$parsers.push(function (value) {

                        return '' + value;
                    });
                    ngModel.$formatters.push(function (value) {
                        return parseFloat(value);
                    });
                }
            };
        });;
})();