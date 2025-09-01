(function () {
    'use strict';
    asignarViabilidadNacionalSgrController.$inject = [
        'utilidades',
        '$sessionStorage',
        'justificacionCambiosServicio',
        'transversalSgrServicio',
        'asignacionEntidadNacionalSgrServicio'
    ];

    function asignarViabilidadNacionalSgrController(
        utilidades,
        $sessionStorage,
        justificacionCambiosServicio,
        transversalSgrServicio,
        asignacionEntidadNacionalSgrServicio
    ) {
        var vm = this;
        vm.user = {};
        vm.lang = "es";
        vm.nombreComponente = 'sgrentidadnacionalasignacionasignarviabilidad';
        vm.seccionCapitulo = null; //para guardar los capitulos modificados y que se llenen las lunas
        vm.Bpin = $sessionStorage.idObjetoNegocio;
        vm.IdNivel = $sessionStorage.idNivel;
        vm.idInstancia = $sessionStorage.idInstancia;
        vm.idAccion = $sessionStorage.idAccion;
        vm.proyectoId = $sessionStorage.proyectoId;
        vm.idInstanciaPadre = $sessionStorage.InstanciaSeleccionada != undefined ? $sessionStorage.InstanciaSeleccionada.InstanciaPadreId : null;
        vm.idEntidadAdscrita = $sessionStorage.EntidadAdscritaId;
        vm.tipoVisualizacionAsignar = 'tipo1';
        vm.actualizadoDelegar = false;
        vm.rolAsignarId = '';
        vm.disabledAsig = false;
        vm.activar = true;
        vm.desactivar = true;
        vm.usuarioEncargado = '';
        vm.estadoBotonEdit = 'EDITAR';
        vm.data = {
            IdUsuarioDNP: ''
        };

        vm.init = function () {
            vm.notificacionvalidacion({ handler: vm.notificacionValidacionPadre, nombreComponente: vm.nombreComponente, esValido: true });
            ValidarUsuarioEncargado(vm.proyectoId, vm.idInstancia);
            vm.disabledAsig = $sessionStorage.soloLectura;
        };

        // Registra un observador para manejar eventos de comunicación
        // NOTA: controlar cada parámetro.
        asignacionEntidadNacionalSgrServicio.registrarObservador(function (datos) {
            // Verifica si se debe actualizar el capítulo
            if (datos.actualizarCapitulo === 'true')
                guardarCapituloModificado();
            else if (datos.tipoVisualizacion !== undefined) {
                vm.tipoVisualizacionAsignar = datos.tipoVisualizacion;
                vm.actualizadoDelegar = true;
            }
        });

        vm.datosUsuarioSeleccionado = function () {
            if (vm.data.IdUsuarioDNP != undefined)
                vm.usuarioSeleccionado = vm.usuariosEncargados.find(x => x.IdUsuarioDNP == vm.data.IdUsuarioDNP);
        }

        function ObtenerParametros() {
            transversalSgrServicio.SGR_Transversal_LeerParametro("RolEntidadNacionalTecnicoSGR")
                .then(function (respuestaParametro) {
                    if (respuestaParametro.data)
                        ObtenerUsuariosEntidad(respuestaParametro.data.Valor);
                }, function () {
                    utilidades.mensajeError('Ocurrió un problema al obtener los parametros.');
                    return;
                });
        }

        function ValidarUsuarioEncargado(proyectoId, idInstancia) {
            asignacionEntidadNacionalSgrServicio.SGR_Proyectos_LeerAsignacionUsuarioEncargado(proyectoId, idInstancia)
                .then(function (response) {
                    if (response.data.length != 0) {
                        if (!$sessionStorage.soloLectura)
                            vm.disabledAsig = false;
                        else
                            vm.disabledAsig = true;
                        vm.usuarioEncargado = response.data[0];
                        ObtenerParametros();
                        vm.data.IdUsuarioDNP = vm.usuarioEncargado.IdUsuario;
                        vm.tipoVisualizacionAsignar = 'tipo2';
                        asignacionEntidadNacionalSgrServicio.notificarCambio({ usuarioDelegado: 'true' });
                        asignacionEntidadNacionalSgrServicio.notificarCambio({ reqEntidad: false });
                    }
                    else {
                        if (!vm.actualizadoDelegar)
                            vm.tipoVisualizacionAsignar = 'tipo1';
                        ObtenerParametros();
                    }
                }, function () {
                    utilidades.mensajeError('Ocurrió un problema al validar el usuario encargado.');
                });
        }

        function ObtenerUsuariosEntidad(rolId) {

            vm.rolAsignarId = rolId;

            var parametros = {
                EntidadId: vm.idEntidadAdscrita,
                RolId: rolId,
                InstanciaId: vm.idInstancia
            }

            asignacionEntidadNacionalSgrServicio.ObtenerUsuariosEntidadAdscrita(parametros)
                .then(function (response) {
                    if (response.data != null)
                        vm.usuariosEncargados = response.data;
                }, function () {
                    utilidades.mensajeError('Ocurrió un problema al obtener la información de los funcionarios encargados.');
                    return;
                });
        }

        vm.ActivarEditar = function () {
            if (vm.activar == true) {
                vm.estadoBotonEdit = 'CANCELAR';
                vm.activar = false;
                asignacionEntidadNacionalSgrServicio.notificarCambio({ usuarioDelegado: 'true' });
                if (vm.usuarioEncargado != '') {
                    vm.data.IdUsuarioDNP = vm.usuarioEncargado.IdUsuario;
                    vm.datosUsuarioSeleccionado();
                }
            }
            else {
                utilidades.mensajeWarning("¿Está seguro de continuar?", function funcionContinuar() {
                    OkCancelar();
                    if (vm.usuarioEncargado != '')
                        vm.data.IdUsuarioDNP = vm.usuarioEncargado.IdUsuario;
                    else {
                        vm.data.IdUsuarioDNP = '';
                        asignacionEntidadNacionalSgrServicio.notificarCambio({ usuarioDelegado: 'false' });
                    }
                    vm.estadoBotonEdit = 'EDITAR';
                    vm.activar = true;
                }, function funcionCancelar() {
                    return;
                }, null, null, "Los datos que posiblemente haya diligenciado se perderán.");
            }
        }

        function OkCancelar() {
            setTimeout(function () {
                utilidades.mensajeSuccess("", false, false, false, "La edición ha sido cancelada con éxito.");
            }, 500);
        }

        vm.guardar = function () {
            if (vm.data.IdUsuarioDNP === null || vm.data.IdUsuarioDNP === undefined || vm.data.IdUsuarioDNP === "") {
                utilidades.mensajeError('El campo funcionario encargado de la elaboración del concepto de viabilidad es obligatorio.', false);
                return false;
            }

            var data = {
                ProyectoId: vm.proyectoId,
                InstanciaId: vm.idInstancia,
                UsuarioEncargado: vm.data.IdUsuarioDNP,
                RolUsuarioEncargadoId: vm.rolAsignarId
            }

            Guardar(data);
        }

        function Guardar(data) {
            return asignacionEntidadNacionalSgrServicio.SGR_Proyectos_GuardarAsignacionUsuarioEncargado(data).then(function (response) {
                if (response.data) {
                    guardarCapituloModificado();
                    utilidades.mensajeSuccess(`Se ha asignado la elaboración de concepto de viabilidad a ${vm.usuarioSeleccionado.NombreUsuario} - ${vm.usuarioSeleccionado.Correo}.`, false, false, false);
                    vm.estadoBotonEdit = 'EDITAR';
                    vm.activar = true;
                    vm.disabledAsig = true;
                } else
                    utilidades.mensajeError("Error al realizar la operación.", false);
            }, function () {
                utilidades.mensajeError('Ocurrió un problema al guardar el usuario encargado.');
            });
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
            var tipError = 'ASIGEN';
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

    angular.module('backbone').component('asignarViabilidadNacionalSgr', {
        templateUrl: "/src/app/formulario/ventanas/SGR/entidadNacional/asignacionViabilidad/asignacionTecnico/asignar/asignarViabilidadNacionalSgr.html",
        controller: asignarViabilidadNacionalSgrController,
        controllerAs: "vm",
        bindings: {
            guardadoevent: '&',
            notificacionestado: '&',
            notificacionvalidacion: '&'
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