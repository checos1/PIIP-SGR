(function () {
    'use strict';
    datosGeneralesSgrController.$inject = [
        'utilidades',
        '$sessionStorage',
        'operacionesCreditoSgrServicio',
        'justificacionCambiosServicio',
        'previosSgrServicio',
        '$timeout'
    ];

    function datosGeneralesSgrController(
        utilidades,
        $sessionStorage,
        operacionesCreditoSgrServicio,
        justificacionCambiosServicio,
        previosSgrServicio,
        $timeout
    ) {
        var vm = this;
        vm.user = {};
        vm.lang = "es";
        vm.nombreComponente = "sgrviabilidadpreviosoperacioncreditoinformaciongeneralcredito";
        vm.Bpin = $sessionStorage.idObjetoNegocio;
        vm.IdNivel = $sessionStorage.idNivel;
        vm.idInstancia = $sessionStorage.idInstancia;
        vm.idAccion = $sessionStorage.idAccion;
        vm.proyectoId = $sessionStorage.proyectoId;
        vm.preguntaCredito = 3816;

        vm.disabled = false;
        vm.activar = true;
        vm.permiteEditar = false;
        vm.respuestaPreguntaCredito = 0;
        vm.habilitaOperacionCredito = false;
        vm.erroresComponente = [];
        vm.flujoaprobacion = "";
        vm.Valores =
        {
            ProyectoId: 0,
            BPIN: "",
            Criterios: [
                {
                    NombreTipoValor: "",
                    Habilita: false,
                    Valor: 0
                }
            ]
        };

        vm.ConvertirNumero = ConvertirNumero;

        vm.componentesRefresh = [
            "sgrviabilidadpreviosrecursosfuentessgr",
            "sgrviabilidadpreviosdatosgeneralesdatospresentacion"
        ];

        vm.init = function () {
            vm.notificacioncambios({ handler: vm.notificacionCambiosCapitulos, nombreComponente: vm.nombreComponente });
            vm.notificacionvalidacion({ handler: vm.notificacionValidacionPadre, nombreComponente: vm.nombreComponente, esValido: true });
            vm.notificacioninicio({ handlerInicio: vm.notificacionInicioPadre, nombreComponente: vm.nombreComponente, esValido: true });

        };
        vm.notificacionInicioPadre = function (nombreModificado, errores) {
            if (nombreModificado == vm.nombreComponente) {

                if (errores != null && errores != undefined) {
                    vm.erroresComponente = errores;
                }

                vm.flujoaprobacion = utilidades.obtenerParametroTransversal('FlujoAprobacion1');

                if (vm.flujoaprobacion !== $sessionStorage.InstanciaSeleccionada.FlujoId.toUpperCase()) {
                    vm.Bpin = $sessionStorage.InstanciaSeleccionada != undefined ? $sessionStorage.InstanciaSeleccionada.ProyectoId : "";
                    vm.disabled = $sessionStorage.soloLectura;
                }
                else {
                    vm.Bpin = $sessionStorage.InstanciaSeleccionada.IdObjetoNegocio;
                    vm.permiteEditar = false;
                    vm.activar = true;
                    vm.disabled = true;
                }

                ObtenerOperacionesCredito();
            }
        }

        previosSgrServicio.registrarObservador(function (datos) {
            if (datos.Capitulo === 'sgrviabilidadpreviosdatosgeneralesdatospresentacion')
                ObtenerOperacionesCredito();
            if (typeof datos.RespuestaCredito !== 'undefined') {
                vm.respuestaPreguntaCredito = datos.RespuestaCredito;
                vm.habilitaOperacionCredito = vm.respuestaPreguntaCredito == 1 && vm.Valores.FuentesCredito > 0;
                vm.disabled = $sessionStorage.soloLectura || !vm.habilitaOperacionCredito;
                // Product Backlog Item 66633: Gestión Integral y Validación de Datos de Operaciones de Crédito
                if (vm.respuestaPreguntaCredito == 2) {

                    eliminarOperacionCredito();
                    eliminarCapitulosModificados();
                    vm.limpiarErrores();
                    vm.permiteEditar = false;
                    $("#EditarDG").html("EDITAR");
                    vm.activar = true;
                }
            }
        });

        function ObtenerOperacionesCredito() {
            operacionesCreditoSgrServicio.obtenerOperacionCreditoDatosGenerales(vm.Bpin, vm.idInstancia).then(
                function (response) {
                    if (response.data != null && response.data != "") {
                        vm.Valores = response.data;
                        if (typeof (vm.Valores.Criterios) === 'object' && vm.Valores.Criterios.length > 1) { 
                            vm.Valores.Criterios[1].Valor = ConvertirNumero(vm.Valores.Criterios[1].Valor);
                            vm.Valores.Criterios[3].Valor = ConvertirNumero(vm.Valores.Criterios[3].Valor);
                            vm.Valores.Criterios[4].Valor = ConvertirNumero(vm.Valores.Criterios[4].Valor);
                        }
                        if (vm.erroresComponente != null && vm.erroresComponente != undefined) {
                            $timeout(function () {
                                vm.notificacionValidacionPadre(vm.erroresComponente);
                            }, 600);
                        }
                    }
                }
            ).catch(error => {
                if (error.status == 400) {
                    utilidades.mensajeError(error.data.Message || "Error al realizar la operación");
                    return;
                }
                utilidades.mensajeError("Error al realizar la operación");
            });
            previosSgrServicio.ConsultarAccionPorInstancia(vm.idInstancia, vm.idAccion).then(
                function (result) {
                    previosSgrServicio.obtenerPreguntasPersonalizadas(vm.Bpin, vm.IdNivel, vm.idInstancia, result.data.EstadoAccionPorInstanciaId).then(
                        function (respuesta) {
                            var respuestas = respuesta.data;
                            vm.preguntaCredito = utilidades.obtenerParametroTransversal("IdPreguntaOperacionesCredito");
                            var respuestasGenerales = respuestas.PreguntasGenerales.find(p => p.Preguntas.find(o => o.IdPregunta == vm.preguntaCredito));
                            var elemento = typeof respuestasGenerales !== 'undefined' ? respuestasGenerales.Preguntas.find(x => x.IdPregunta == vm.preguntaCredito) : undefined;
                            var respuestaCredito = typeof elemento !== 'undefined' ? elemento.Respuesta : undefined;
                            if (typeof respuestaCredito !== 'undefined') {
                                vm.respuestaPreguntaCredito = respuestaCredito;
                                vm.habilitaOperacionCredito = vm.respuestaPreguntaCredito == 1 && vm.Valores.FuentesCredito > 0;
                                vm.disabled = $sessionStorage.soloLectura || !vm.habilitaOperacionCredito;
                            }
                        }
                    ).catch(error => {
                        if (error.status == 400) {
                            utilidades.mensajeError(error.data.Message || "Error al realizar la operación");
                            return;
                        }
                        utilidades.mensajeError("Error al obtener las preguntas personalizadas");
                    });
                }
            ).catch(error => {
                if (error.status == 400) {
                    utilidades.mensajeError(error.data.Message || "Error al realizar la operación");
                    return;
                }
                utilidades.mensajeError("Error al realizar la operación");
            });
        }

        function eliminarOperacionCredito() {
            operacionesCreditoSgrServicio.eliminarOperacionCredito(vm.proyectoId).then(
                function (response) {
                    if (response.data != null && response.data != "") {
                        vm.Valores =
                        {
                            ProyectoId: 0,
                            BPIN: "",
                            Criterios: [
                                {
                                    NombreTipoValor: "",
                                    Habilita: false,
                                    Valor: 0
                                }
                            ]
                        };
                    }
                }
            ).catch(error => {
                if (error.status == 400) {
                    utilidades.mensajeError(error.data.Message || "Error al realizar la operación");
                    return;
                }
                utilidades.mensajeError("Error al realizar la operación");
            });

        }

        vm.ActivarEditar = function () {
            if (vm.activar == true) {
                if (vm.respuestaPreguntaCredito == 1 && vm.Valores.FuentesCredito < 1) {
                    utilidades.mensajeWarning("Las fuentes de financiación del proyecto NO se permiten para aplicar al mecanismo de Operaciones de crédito público. Para aclarar dudas se recomienda consultar la normatividad vigente del SGR");
                }
                else { 
                    vm.permiteEditar = true;
                    $("#EditarDG").html("CANCELAR");
                    vm.activar = false;
                }
            }
            else {
                utilidades.mensajeWarning("¿Está seguro de continuar?", function funcionContinuar() {
                    OkCancelar();

                    RestablecerModoNoEdicion();
                    vm.init();

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

        function RestablecerModoNoEdicion() {
            vm.permiteEditar = false;
            $("#EditarDG").html("EDITAR");
            vm.activar = true;
        }

        vm.actualizainput2 = function (event) {
            var sum = 0;
            var ValorFinanciero = 0;
            var ValorTotalProyecto = 0;
            var ValorPatrimonio = 0;

            $(event.target).val(function (index, value) {

                ValorPatrimonio = ObtenerNumero(value);

                ValorFinanciero = ObtenerNumero(vm.Valores.Criterios[4].Valor);
                ValorTotalProyecto = ObtenerNumero(vm.Valores.Criterios[0].Valor);
                sum = ValorTotalProyecto + ValorFinanciero + ValorPatrimonio;
                vm.Valores.Criterios[5].Valor = sum;

                return value;
            });
        }

        vm.actualizainput4 = function (event) {
            var sum = 0;
            var ValorFinanciero = 0;
            var ValorCredito = 0;

            $(event.target).val(function (index, value) {

                ValorCredito = ObtenerNumero(value);

                ValorFinanciero = ObtenerNumero(vm.Valores.Criterios[4].Valor);
                sum = ValorFinanciero + ValorCredito;
                vm.Valores.Criterios[2].Valor = sum;

                return value;
            });
        }

        vm.actualizainput5 = function (event) {
            var sum = 0;
            var ValorPatrimonio = 0;
            var ValorCredito = 0;
            var ValorTotalProyecto = 0;
            var ValorFinanciero = 0;

            $(event.target).val(function (index, value) {

                ValorFinanciero = ObtenerNumero(value);

                ValorCredito = ObtenerNumero(vm.Valores.Criterios[3].Valor);
                sum = ValorCredito + ValorFinanciero;
                vm.Valores.Criterios[2].Valor = sum;
                ValorTotalProyecto = ObtenerNumero(vm.Valores.Criterios[0].Valor);
                ValorPatrimonio = !vm.Valores.Criterios[1].Habilita ? 0 : ObtenerNumero(vm.Valores.Criterios[1].Valor);
                sum = ValorTotalProyecto + ValorPatrimonio + ValorFinanciero;
                vm.Valores.Criterios[5].Valor = sum;

                return value;
            });
        }


        vm.guardar = function (response) {
            Guardar();
        }

        function ObtenerValoresGuardar() {

            var validar = true;

            var ValorTotalProyecto = 0;
            var ValorPatrimonio = 0;
            var ValorServicioDeuda = 0;
            var ValorCredito = 0;
            var ValorFinanciero = 0;
            var ValorFinanciar = 0;
            var ValorFuentesCredito = 0;

            ValorTotalProyecto = ObtenerNumero(vm.Valores.Criterios[0].Valor);
            ValorPatrimonio = !vm.Valores.Criterios[1].Habilita ? 0 : ObtenerNumero(vm.Valores.Criterios[1].Valor);
            ValorCredito = ObtenerNumero(vm.Valores.Criterios[3].Valor);
            ValorFinanciero = ObtenerNumero(vm.Valores.Criterios[4].Valor);
            ValorFuentesCredito = ObtenerNumero(vm.Valores.Criterios[6].Valor);

            ValorServicioDeuda = (ValorCredito + ValorFinanciero).toFixed(2);
            ValorFinanciar = (ValorTotalProyecto + ValorPatrimonio + ValorFinanciero).toFixed(2);

            if (ValorCredito > ValorFuentesCredito) {
                utilidades.mensajeError('El valor del crédito no puede ser mayor a la sumatoria del valor solicitado en las fuentes de financiación comprometidas en la operación del crédito.');
                validar = false;
            }

            // Revisar si esta validación aplica
            if (ValorFinanciero > ValorCredito) {
                utilidades.mensajeError('El costo financiero debe ser igual o menor al valor del crédito.');
                validar = false;
            }

            if (ValorPatrimonio > ValorCredito) {
                utilidades.mensajeError('El costo de administración del patrimonio autónomo debe ser igual o menor al valor del crédito.');
                validar = false;
            }

            if (!validar) {

                return validar;

            } else {

                var ValoresGuardar = angular.copy(vm.Valores);

                ValoresGuardar.Criterios[1].Valor = ValorPatrimonio;
                ValoresGuardar.Criterios[2].Valor = ValorServicioDeuda;

                ValoresGuardar.Criterios[3].Valor = ValorCredito;

                ValoresGuardar.Criterios[4].Valor = ValorFinanciero;
                ValoresGuardar.Criterios[5].Valor = ValorFinanciar;

                return ValoresGuardar;
            }
        }

        function Guardar() {

            var valores = ObtenerValoresGuardar();

            if (!valores) {

                return;

            } else { 

                return operacionesCreditoSgrServicio.guardarOperacionCreditoDatosGenerales(valores)
                    .then(function (response) {
                        if (response.data != '') {
                            var respuesta = jQuery.parseJSON(response.data)
                            if (respuesta.StatusCode == "200") {
                                parent.postMessage("cerrarModal", window.location.origin);
                                guardarCapituloModificado();
                                vm.limpiarErrores();
                                utilidades.mensajeSuccess("Usted puede continuar especificando datos en el capítulo \"Detalle del crédito\"", false, false, false, "Los datos se han guardado con éxito");
                                vm.permiteEditar = false;
                                $("#EditarDG").html("EDITAR");
                                vm.activar = true;
                            }
                            else {
                                let data = JSON.parse(respuesta.ReasonPhrase);
                                let alertMessage = data.Mensaje + '\n';

                                data.ListaErrores.forEach(function (errorObj) {
                                    alertMessage += '- ' + errorObj.Error + '\n';
                                });

                                utilidades.mensajeError("Error al realizar la operación, " + alertMessage, false);
                            }
                        }
                        else {
                            utilidades.mensajeError("Error al realizar la operación" + respuesta.ReasonPhrase, false);
                        }
                    }).catch(error => {
                        if (error.status == 400) {
                            utilidades.mensajeError(error.data.Message || "Error al realizar la operación");
                            return;
                        }
                        utilidades.mensajeError("Error al realizar la operación");
                    });
            }
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

        function eliminarCapitulosModificados() {
            ObtenerSeccionCapitulo();
            var data = {
                ProyectoId: $sessionStorage.InstanciaSeleccionada.ProyectoId,
                Justificacion: "",
                SeccionCapituloId: vm.seccionCapitulo,
                InstanciaId: $sessionStorage.idInstancia
            }

            justificacionCambiosServicio.eliminarCapitulosModificados(data)
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

        vm.validateFormat = function (event) {

            if ((event.keyCode < 48 || event.keyCode > 57) && event.keyCode != 44) {
                event.preventDefault();
            }
        }

        function ConvertirNumero(numero) {
            if (typeof (numero) == 'number') {
                return new Intl.NumberFormat('es-co', {
                    minimumFractionDigits: 2,
                }).format(numero);
            } else {
                return numero;
            };
        }

        function ObtenerNumero(cadenaNumero) {
            if (typeof (cadenaNumero) == 'string') {
                return parseFloat(cadenaNumero.replaceAll(".", "").replace(",", "."));
            } else {
                return cadenaNumero;
            }
        }

        /* ------------------------ Validaciones ---------------------------------*/

        vm.notificacionValidacionPadre = function (errores) {
            vm.limpiarErrores();
            if (errores != undefined) {
                var erroresRelacionconlapl = errores.find(p => (p.Seccion + p.Capitulo) == vm.nombreComponente);
                var erroresJson = (erroresRelacionconlapl == undefined || erroresRelacionconlapl.Errores == "") ? [] : JSON.parse(erroresRelacionconlapl.Errores);
                var isValid = (erroresJson == null || erroresJson.length == 0);
                if (!isValid) {
                    erroresJson[vm.nombreComponente].forEach(p => {
                        var nameArr = p.Error.split('-');
                        var TipoError = nameArr[0].toString();
                        if (p.Error == 'OPCPRE1' || p.Error == 'OPCPRE2') {
                            vm.validarErroresPreguntas(p.Error, p.Descripcion, false);
                            vm.validarValores(p.Error, false);
                        } else if (TipoError == 'SGRERRSEC') {
                            vm.validarSeccion(TipoError, nameArr[1].toString(), p.Descripcion, false);
                        } else {
                            vm.validarValores(p.Error, false);
                        }
                    });
                }
                vm.notificacionestado({ nombreComponente: vm.nombreComponente, esValido: isValid });
            }
        }

        vm.validarErroresPreguntas = function (error, Descripcion, esValido) {
            var campomensajeerror = document.getElementById(vm.nombreComponente + "-mensaje-error");
            if (campomensajeerror != undefined) {
                campomensajeerror.innerHTML = "<span class='d-inline-block ico-advertencia'></span><span>" + Descripcion + "</span>";
                campomensajeerror.classList.remove('hidden');
            }
        }

        vm.validarValores = function (error, esValido) {
            var idSpanAlertComponent = document.getElementById("alert-" + error);
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
            var idSpanAlertComponent = document.getElementById("alert-OCDG_VTC");
            idSpanAlertComponent.classList.remove("ico-advertencia");

            var idSpanAlertComponent1 = document.getElementById("alert-OCDG_VF");
            idSpanAlertComponent1.classList.remove("ico-advertencia");

            var idSpanAlertComponent2 = document.getElementById("alert-OCDG_VP");
            idSpanAlertComponent2.classList.remove("ico-advertencia");

            vm.validarValores(vm.nombreComponente, true);

            var errorElements = document.getElementsByClassName('errorSeccionOperacionCreditoPrevios');
            var testDivs = Array.prototype.filter.call(errorElements, function (errorElement) {
                errorElement.innerHTML = "";
                errorElement.classList.add('hidden');
            });

            var campomensajeerror2 = document.getElementById(vm.nombreComponente + "-mensaje-error");
            if (campomensajeerror2 != undefined) {
                campomensajeerror2.innerHTML = "";
                campomensajeerror2.classList.add('hidden');
            }
        }

        vm.notificacionCambiosCapitulos = function (nombreCapituloHijo) {
            if (vm.componentesRefresh.includes(nombreCapituloHijo)) {
                ObtenerOperacionesCredito();
                eliminarCapitulosModificados();
            }
        }
    }

    angular.module('backbone').component('datosGeneralesSgr', {
        templateUrl: "src/app/formulario/ventanas/SGR/viabilidadSGR/previos/operacionesCredito/datosGenerales/datosGeneralesSgr.html",
        controller: datosGeneralesSgrController,
        controllerAs: "vm",
        bindings: {
            callback: '&',
            guardadoevent: '&',
            notificacionvalidacion: '&',
            notificacionestado: '&',
            notificacioninicio: '&',
            notificacioncambios: '&'
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