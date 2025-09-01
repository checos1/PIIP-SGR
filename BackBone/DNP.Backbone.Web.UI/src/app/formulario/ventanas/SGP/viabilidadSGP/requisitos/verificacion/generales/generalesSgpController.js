(function () {
    'use strict';
    generalesSgpController.$inject = [
        'utilidades',
        '$sessionStorage',
        'requisitosSgpServicio',
        'justificacionCambiosServicio',
    ];

    function generalesSgpController(
        utilidades,
        $sessionStorage,
        requisitosSgpServicio,
        justificacionCambiosServicio,
    ) {
        var vm = this;
        vm.user = {};
        vm.lang = "es";
        vm.nombreComponente = "sgpviabilidadrequisitosverificacionrequisitossgp";

        //para guardar los capitulos modificados y que se llenen las lunas
        vm.seccionCapitulo = null;

        vm.Bpin = $sessionStorage.idObjetoNegocio;
        vm.IdNivel = $sessionStorage.idNivel;
        vm.idInstancia = $sessionStorage.idInstancia;
        vm.idAccion = $sessionStorage.idAccion;

        vm.buscarGenerales = buscarGenerales;

        vm.codigoG;

        vm.soloLectura = false;
        vm.activar = true;

        vm.PreguntasPersonalizadas = [{
            CodigoBPIN: "",
            Cuestionario: 0,
            CR: 0,
            Fase: "",
            EntidadDestino: 0,
            ObservacionCuestionario: null,
            Definitivo: null,
            CumpleCuestionario: null,
            Fecha: "",
            Usuario: "",
            AgregarRequisitos: false,
            InstanciaId: "",
            Sector: "",
            Acuerdo: "",
            Clasificacion: "",
            PreguntasEspecificas: null,
            PreguntasGenerales: [
                {
                    Tematica: "",
                    OrdenTematica: 0,
                    Preguntas: [
                        {
                            IdPregunta: 0,
                            Tipo: "",
                            Subtipo: "",
                            Tematica: null,
                            OrdenTematica: null,
                            Pregunta: "",
                            OrdenPregunta: 0,
                            Explicacion: "",
                            OpcionesRespuesta: [
                                {
                                    OpcionId: 0,
                                    ValorOpcion: "",
                                }
                            ],
                            AyudaOpcionesRespuesta: "",
                            Respuesta: "",
                            ObligaObservacion: [
                                {
                                    OpcionId: 0,
                                    ValorOpcion: "",
                                }
                            ],
                            ObservacionPregunta: "",
                            AyudaObservacion: "",
                            Cabecera: "",
                            Nota: "",
                            CumpleEn: ""
                        }
                    ]
                }
            ]
        }];

        vm.componentesRefresh = [
            "sgpviabilidadrequisitosdatosgeneralesagregarsectoressgp",
            "sgpviabilidadrequisitosverificacionrequisitossgp"
        ];

        vm.componentesRefreshEliminar = [
            "sgpviabilidadrequisitosverificacionrequisitossgp"
        ];

        vm.init = function () {
            DevolverCuestionarioProyecto();
            vm.notificacioncambios({ handler: vm.notificacionCambiosCapitulos, nombreComponente: vm.nombreComponente });
            vm.notificacionvalidacion({ handler: vm.notificacionValidacionPadre, nombreComponente: vm.nombreComponente, esValido: true });
        };

        vm.notificacionCambiosCapitulos = function (nombreComponenteHijo) {
            if (vm.componentesRefresh.includes(nombreComponenteHijo)) {
                obtenerPreguntasPersonalizadas(vm.Bpin, vm.IdNivel, vm.idInstancia, "A");
            }

            if (vm.componentesRefreshEliminar.includes(nombreComponenteHijo)) {
                eliminarCapitulosModificados();
            }
        }

        vm.BtnTematica = function (campo) {
            var variable = $("#img" + campo)[0].outerHTML;
            if (variable.includes('Img/btnMas.svg')) {
                $("#img" + campo).attr('src', 'Img/btnMenos.svg');
            }
            else {
                $("#img" + campo).attr('src', 'Img/btnMas.svg');
            }
        }

        function DevolverCuestionarioProyecto() {
            requisitosSgpServicio.ConsultarAccionPorInstancia(vm.idInstancia, vm.idAccion).then(
                function (result) {
                    requisitosSgpServicio.DevolverCuestionarioProyecto(vm.IdNivel, vm.idInstancia, result.data.EstadoAccionPorInstanciaId).then(
                        function (response) {
                            if (response.data && response.data.Exito) {
                                var bPin = vm.Bpin;
                                var nivelId = vm.IdNivel;
                                var instanciaId = vm.idInstancia;
                                var listaRoles = "A";

                                obtenerPreguntasPersonalizadas(bPin, nivelId, instanciaId, listaRoles);
                            } else {

                            }
                        });
                });
        }

        vm.SinPreguntasEspecificas = false;

        function obtenerPreguntasPersonalizadas(bPin, nivelId, instanciaId, listaRoles) {
            return requisitosSgpServicio.obtenerPreguntasPersonalizadasComponenteSGP(bPin, nivelId, instanciaId, vm.nombreComponente, listaRoles).then(
                function (respuesta) {

                    vm.PreguntasPersonalizadas = respuesta.data;

                    if (vm.PreguntasPersonalizadas.PreguntasEspecificas.length == 0) {
                        vm.SinPreguntasEspecificas = true;
                    }

                    vm.PreguntasPersonalizadas.PreguntasGenerales = [
                        ...respuesta.data.PreguntasGenerales
                    ];
                    vm.PreguntasPersonalizadas.PreguntasEspecificas = [
                        ...respuesta.data.PreguntasEspecificas
                    ];
                    vm.soloLectura = $sessionStorage.soloLectura;
                }
            );
        }


        function buscarGenerales() {
            limpiarBuscarG(false);
            limpiarBuscarE(false);
            if (vm.codigoG != '') {
                var resultado = "No se encontraron resultados para esta búsqueda.";
                var contador = 0;

                vm.PreguntasPersonalizadas.PreguntasGenerales.forEach(generales => {
                    generales.Preguntas.forEach(preguntas => {

                        $("#spanId" + preguntas.IdPregunta).html(preguntas.IdPregunta);
                        $("#labelId" + preguntas.IdPregunta).html(preguntas.Pregunta);

                        if (preguntas.IdPregunta.toString() === vm.codigoG) {
                            $("#TematicaIdG" + generales.OrdenTematica).removeClass("collapse");
                            $("#u9_imgG" + generales.OrdenTematica).removeAttr("style");
                            var pregunta = "<span class='TextConFondoObseDNP'>" + vm.codigoG + "</span>";
                            $("#spanId" + preguntas.IdPregunta).html(pregunta);
                            contador++;
                        }

                        if (preguntas.Pregunta.includes(vm.codigoG)) {
                            $("#TematicaIdG" + generales.OrdenTematica).removeClass("collapse");
                            $("#u9_imgG" + generales.OrdenTematica).removeAttr("style");
                            var pregunta = preguntas.Pregunta.replace(vm.codigoG, "<span class='TextConFondoObseDNP'>" + vm.codigoG + "</span>");
                            $("#labelId" + preguntas.IdPregunta).html(pregunta);
                            contador++;
                        }
                    });
                });

                vm.PreguntasPersonalizadas.PreguntasEspecificas.forEach(especificas => {
                    especificas.Preguntas.forEach(preguntasE => {

                        $("#spanId" + preguntasE.IdPregunta).html(preguntasE.IdPregunta);
                        $("#labelId" + preguntasE.IdPregunta).html(preguntasE.Pregunta);

                        if (preguntasE.IdPregunta.toString() === vm.codigoG) {
                            $("#TematicaIdE" + especificas.OrdenTematica).removeClass("collapse");
                            $("#u9_imgE" + especificas.OrdenTematica).removeAttr("style");
                            var pregunta = "<span class='TextConFondoObseDNP'>" + vm.codigoG + "</span>";
                            $("#spanId" + preguntasE.IdPregunta).html(pregunta);
                            contador++;
                        }

                        if (preguntasE.Pregunta.includes(vm.codigoG)) {
                            $("#TematicaIdE" + especificas.OrdenTematica).removeClass("collapse");
                            $("#u9_imgE" + especificas.OrdenTematica).removeAttr("style");
                            var pregunta = preguntasE.Pregunta.replace(vm.codigoG, "<span class='TextConFondoObseDNP'>" + vm.codigoG + "</span>");
                            $("#labelId" + preguntasE.IdPregunta).html(pregunta);
                            contador++;
                        }
                    });
                });

                if (contador > 0) {
                    resultado = contador.toString() + " resultados en (Requisitos). Los desplegables con el marcador <img class='imgViabilidad' src='Img/u9.svg'> contienen alguna coincidencia.";
                }

                $("#labelResultadoG").html(resultado);
            }
        }

        function limpiarBuscarG(limpiarCampos) {

            if (limpiarCampos) {
                vm.codigo = null;
                vm.palabraclave = null;
            }

            $("#labelResultadoG").html("");

            vm.PreguntasPersonalizadas.PreguntasGenerales.forEach(generales => {
                generales.Preguntas.forEach(preguntas => {
                    $("#TematicaIdG" + generales.OrdenTematica).addClass("collapse");
                    $("#u9_imgG" + generales.OrdenTematica).hide();
                    $("#labelId" + preguntas.IdPregunta).html(preguntas.Pregunta);
                });
            });
        }

        function limpiarBuscarE(limpiarCampos) {

            if (limpiarCampos) {
                vm.codigo = null;
                vm.palabraclave = null;
            }

            $("#labelResultadoG").html("");

            vm.PreguntasPersonalizadas.PreguntasEspecificas.forEach(especificas => {
                especificas.Preguntas.forEach(preguntasE => {
                    $("#TematicaIdE" + especificas.OrdenTematica).addClass("collapse");
                    $("#u9_imgE" + especificas.OrdenTematica).hide();
                    $("#labelId" + preguntasE.IdPregunta).html(preguntasE.Pregunta);
                });
            });
        }

        vm.ActivarEditar = function () {
            if (vm.activar == true) {
                $("#EditarReq").html("CANCELAR");
                vm.activar = false;
            }
            else {
                utilidades.mensajeWarning("¿Está seguro de continuar?", function funcionContinuar() {
                    OkCancelar();

                    $("#EditarReq").html("EDITAR");
                    vm.activar = true;

                    var bPin = vm.Bpin;
                    var nivelId = vm.IdNivel;
                    var instanciaId = vm.idInstancia;
                    var listaRoles = "A";

                    obtenerPreguntasPersonalizadas(bPin, nivelId, instanciaId, listaRoles);

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

        vm.guardar = function (response) {
            var mensajeValidacion = "";
            vm.PreguntasPersonalizadas.InstanciaId = vm.idInstancia;
            vm.PreguntasPersonalizadas.Definitivo = false;
            ObtenerSeccionCapitulo(vm.nombreComponente);
            mensajeValidacion = validarCuestionario();
            if (mensajeValidacion.length > 0) {
               /* $("#ErrorG").html(mensajeValidacion);*/
            }
            else { Guardar(); }

        }

        function validarCuestionario() {
            var mensaje = "";

            vm.PreguntasPersonalizadas.PreguntasGenerales.forEach(generales => {
                generales.Preguntas.forEach(preguntas => {
                    if (preguntas.ObligaObservacion.length == 1) {
                        var obsObligatoria = document.getElementById('ObsObligatoria' + preguntas.IdPregunta);
                        var tmpJustifica = document.getElementById('justificacion' + preguntas.IdPregunta).value;
                        if (preguntas.ObligaObservacion[0].OpcionId == preguntas.Respuesta && tmpJustifica.length <= 5) {
                            if (obsObligatoria != undefined) {
                                obsObligatoria.classList.remove('hidden');
                                mensaje = "Falla Validación"
                            }
                        }
                        else {
                            if (obsObligatoria != undefined) {
                                obsObligatoria.classList.add('hidden');
                            }
                        }
                    }
                });
            });

            vm.PreguntasPersonalizadas.PreguntasEspecificas.forEach(especificas => {
                especificas.Preguntas.forEach(preguntasE => {
                    if (preguntasE.ObligaObservacion.length == 1) {
                        var obsObligatoria = document.getElementById('ObsObligatoria' + preguntasE.IdPregunta);
                        var tmpJustifica = document.getElementById('justificacion' + preguntasE.IdPregunta).value;
                        if (preguntasE.ObligaObservacion[0].OpcionId == preguntasE.Respuesta && tmpJustifica.length <= 5) {
                            if (obsObligatoria != undefined) {
                                obsObligatoria.classList.remove('hidden');
                                mensaje = "Falla Validación"
                            }
                        }
                        else {
                            if (obsObligatoria != undefined) {
                                obsObligatoria.classList.add('hidden');
                            }
                        }
                    }

                });
            });

            return mensaje;
        }

        vm.guardado = function (nombreComponenteHijo) {
            vm.guardadoevent({ nombreComponente: vm.nombreComponente, nombreComponenteHijo: nombreComponenteHijo });
        }


        function Guardar() {
            return requisitosSgpServicio.guardarRespuestasCustomSGP(vm.PreguntasPersonalizadas).then(
                function (response) {
                    if (response.data && response.status === 200) {
                        if (response.data.Exito) {
                            parent.postMessage("cerrarModal", window.location.origin);
                            guardarCapituloModificado(vm.nombreComponente);
                            if (vm.SinPreguntasEspecificas) guardarCapituloModificado('VerificacionRequisitosViabilidadSGP');
                            utilidades.mensajeSuccess("Cuestionario guardado satisfactoriamente", false, false, false);

                            $("#EditarReq").html("EDITAR");
                            vm.activar = true;
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
        function guardarCapituloModificado(nombreComponente) {
            ObtenerSeccionCapitulo(nombreComponente);
            var data = {
              /*  ProyectoId: $sessionStorage.InstanciaSeleccionada.ProyectoId,*/
                Justificacion: "",
                SeccionCapituloId: vm.seccionCapitulo,
                InstanciaId: vm.idInstancia,
                Modificado: 1,
                cuenta: 1
            }
            justificacionCambiosServicio.guardarCambiosFirme(data)
                .then(function (response) {
                    if (response.data.Exito) {
                        vm.guardadoevent({ nombreComponenteHijo: nombreComponente });
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
                   //     vm.guardadoevent({ nombreComponenteHijo: vm.nombreComponente });
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
            vm.PreguntasPersonalizadas.SeccionCapituloId = vm.seccionCapitulo;
        }

        /* ------------------------ Validaciones ---------------------------------*/

        vm.notificacionValidacionPadre = function (errores) {
            vm.limpiarErrores();
            if (errores != undefined) {
                var erroresRelacionconlapl = errores.find(p => (p.Seccion + p.Capitulo) == vm.nombreComponente);
                var erroresJson = erroresRelacionconlapl.Errores == "" ? [] : JSON.parse(erroresRelacionconlapl.Errores);
                var isValid = (erroresJson == null || erroresJson.length == 0);
                if (!isValid) {
                    erroresJson[vm.nombreComponente].forEach(p => {
                        var nameArr = p.Error.split('-');
                        var TipoError = nameArr[0].toString();
                        if (TipoError == 'ErrorG') {
                            vm.validarValoresPasoAnterior(p.Descripcion);
                        } else if (TipoError == 'REQ') {
                            vm.validarValoresTematica(nameArr[1].toString(), false);
                            vm.validarValores(nameArr[1].toString(), nameArr[2].toString(), p.Descripcion, false);
                        } else if (TipoError == 'SGRERRSEC') {
                            vm.validarSeccion(TipoError, nameArr[1].toString(), p.Descripcion, false);
                        } else {
                            vm.validarValoresTematica(nameArr[0].toString(), false);
                            vm.validarValores(nameArr[0].toString(), nameArr[1].toString(), p.Descripcion, false);
                        }
                    });
                }
                vm.notificacionestado({ nombreComponente: vm.nombreComponente, esValido: isValid });
            }
        }

        vm.validarValoresPasoAnterior = function (errores) {
            var errorG = document.getElementById('ErrorG');
            if (errorG != undefined) {
                errorG.classList.remove('hidden');
                var msgErrorG = document.getElementById('msgErrorG');
                if (msgErrorG != undefined) {
                    msgErrorG.innerHTML = errores;
                    msgErrorG.classList.remove('hidden');
                }
            }
            else {
                errorG.classList.add('hidden');
            }
        }

        vm.validarValoresTematica = function (tematica, esValido) {
            var idSpanAlertComponent = document.getElementById("alert-" + vm.nombreComponente + tematica);
            if (idSpanAlertComponent != undefined) {
                if (!esValido) {
                    idSpanAlertComponent.classList.add("ico-advertencia");
                } else {
                    idSpanAlertComponent.classList.remove("ico-advertencia");
                }
            }
        }

        vm.validarValores = function (tematica, pregunta, errores, esValido) {
            var idSpanAlertComponent = document.getElementById("alert-" + vm.nombreComponente + tematica + pregunta);
            if (idSpanAlertComponent != undefined) {
                if (!esValido) {
                    idSpanAlertComponent.classList.add("ico-advertencia");
                } else {
                    idSpanAlertComponent.classList.remove("ico-advertencia");
                }
            }

            var errorGeneral = document.getElementById('REQ-' + vm.nombreComponente + tematica + pregunta);
            if (errorGeneral != undefined) {
                errorGeneral.innerHTML = errores;
                errorGeneral.classList.remove('hidden');
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
            var errorValoresPasoAnterior = document.getElementById('ErrorG');
            errorValoresPasoAnterior.classList.add('hidden');

            var errorElements = document.getElementsByClassName('errorSeccionRequisitos');
            var testDivs = Array.prototype.filter.call(errorElements, function (errorElement) {
                errorElement.innerHTML = "";
                errorElement.classList.add('hidden');
            });

            vm.PreguntasPersonalizadas.PreguntasGenerales.forEach(generales => {
                generales.Preguntas.forEach(preguntas => {
                    if (generales.OrdenTematica !== null) {
                        var idSpanAlertComponent = document.getElementById("alert-" + vm.nombreComponente + generales.OrdenTematica + preguntas.IdPregunta);
                        idSpanAlertComponent.classList.remove("ico-advertencia");

                        var idSpanAlertComponentT = document.getElementById("alert-" + vm.nombreComponente + generales.OrdenTematica);
                        idSpanAlertComponentT.classList.remove("ico-advertencia");

                        var errorGeneral = document.getElementById('ErrorG');
                        errorGeneral.classList.add('hidden');
                    }
                });
            });

            vm.PreguntasPersonalizadas.PreguntasEspecificas.forEach(generales => {
                generales.Preguntas.forEach(preguntas => {
                    if (generales.OrdenTematica !== null) {
                        var idSpanAlertComponent = document.getElementById("alert-" + vm.nombreComponente + generales.OrdenTematica + preguntas.IdPregunta);
                        idSpanAlertComponent.classList.remove("ico-advertencia");

                        var idSpanAlertComponentT = document.getElementById("alert-" + vm.nombreComponente + generales.OrdenTematica);
                        idSpanAlertComponentT.classList.remove("ico-advertencia");

                    }
                });
            });
        }
    }

    angular.module('backbone').component('generalesSgp', {
        templateUrl: "src/app/formulario/ventanas/SGP/viabilidadSGP/requisitos/verificacion/generales/generalesSgp.html",
        controller: generalesSgpController,
        controllerAs: "vm",
        bindings: {
            callback: '&',
            guardadoevent: '&',
            notificacionvalidacion: '&',
            notificacioncambios: '&',
            notificacionestado: '&'
        }
    });
})();