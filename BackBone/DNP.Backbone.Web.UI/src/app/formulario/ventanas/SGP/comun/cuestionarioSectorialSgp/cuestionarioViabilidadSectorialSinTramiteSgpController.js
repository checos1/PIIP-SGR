(function () {
    'use strict';
    cuestionarioViabilidadSectorialSinTramiteSgpController.$inject = [
        'utilidades',
        '$sessionStorage',
        'viabilidadSgpServicio',
        'justificacionCambiosServicio',
    ];

    function cuestionarioViabilidadSectorialSinTramiteSgpController(
        utilidades,
        $sessionStorage,
        viabilidadSgpServicio,
        justificacionCambiosServicio,
    ) {
        var vm = this;
        vm.user = {};
        vm.lang = "es";
        vm.nombreComponente = "cuestionarioViabilidadSectorialSinTramiteSgp";

        //para guardar los capitulos modificados y que se llenen las lunas
        vm.seccionCapitulo = null;

        vm.Bpin = $sessionStorage.idObjetoNegocio;
        vm.IdNivel = $sessionStorage.idNivel;
        vm.idInstancia = $sessionStorage.idInstancia;
        vm.idAccion = $sessionStorage.idAccion;

        vm.buscarGenerales = buscarGenerales;

        vm.codigoG;

        vm.disabled = false;
        vm.activar = true;
        vm.EsDependiente = false;
        vm.SinPreguntasEspecificas = false;

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
                            CumpleEn: "",
                            NivelJer: 0
                        }
                    ]
                }
            ]
        }];

        vm.init = function () {
            DevolverCuestionarioProyecto();
            //vm.notificacionvalidacion({ handler: vm.notificacionValidacionPadre, nombreComponente: vm.nombreComponente, esValido: true });
        };

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
            viabilidadSgpServicio.ConsultarAccionPorInstancia(vm.idInstancia, vm.idAccion).then(
                function (result) {
                    viabilidadSgpServicio.DevolverCuestionarioProyecto(vm.IdNivel, vm.idInstancia, result.data.EstadoAccionPorInstanciaId).then(
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
       
        function obtenerPreguntasPersonalizadas(bPin, nivelId, instanciaId, listaRoles) {
            return viabilidadSgpServicio.obtenerPreguntasPersonalizadasComponente(bPin, nivelId, instanciaId, vm.nombreComponente, listaRoles).then(
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

                    if (vm.PreguntasPersonalizadas.PreguntasGenerales) {
                        vm.PreguntasPersonalizadas.PreguntasGenerales.forEach(generales => {
                            if (generales.Preguntas[0].Respuesta == 1) {
                                generales.Preguntas[1].NivelJer = 0;
                            }
                            else {
                                generales.Preguntas[1].NivelJer = 2;
                            }
                        });
                    }                    
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

        vm.MostrarOcultarDependiente = function (value, IdPreguntaPadre) {
            if (!IdPreguntaPadre) {
                if (IdPreguntaPadre <= 0) {
                    if (value === 'SI') {
                        vm.PreguntasPersonalizadas.PreguntasGenerales.forEach(generales => {
                            generales.Preguntas.forEach(preguntas => {
                                let resObligatoria = document.getElementById('ResObligatoria' + preguntas.IdPregunta);
                                resObligatoria.classList.add('hidden');
                                if (preguntas.IdPreguntaPadre) {
                                    preguntas.NivelJer = 0;
                                }
                            });
                        });
                    }
                    else {
                        vm.PreguntasPersonalizadas.PreguntasGenerales.forEach(generales => {
                            generales.Preguntas.forEach(preguntas => {
                                let resObligatoria = document.getElementById('ResObligatoria' + preguntas.IdPregunta);
                                resObligatoria.classList.add('hidden');
                                if (preguntas.IdPreguntaPadre) {
                                    preguntas.NivelJer = 2;
                                }
                            });
                        });
                    }
                }
            }
            else {
                if (value) {
                    vm.PreguntasPersonalizadas.PreguntasGenerales.forEach(generales => {
                        generales.Preguntas.forEach(preguntas => {
                            let resObligatoria = document.getElementById('ResObligatoria' + preguntas.IdPregunta);
                            resObligatoria.classList.add('hidden');
                        });
                    });
                }
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
            ObtenerSeccionCapitulo("sgpviabilidadviabilidadInformacionBasicaviabilidadsectorialsintramitesgp");
            mensajeValidacion = validarCuestionario();
            if (mensajeValidacion.length > 0) {
               /* $("#ErrorG").html(mensajeValidacion);*/
                utilidades.mensajeError("Cuestionario incompleto");
            }
            else { Guardar(); }

        }

        function validarCuestionario() {
            var mensaje = "";

            vm.PreguntasPersonalizadas.PreguntasGenerales.forEach(generales => {
                generales.Preguntas.forEach(preguntas => {
                    if (preguntas.ObligaObservacion.length > 0) {
                        var validopregunta = false;
                        preguntas.ObligaObservacion.forEach(ObligaObservacionValue => {
                            var obsObligatoria = document.getElementById('ObsObligatoria' + preguntas.IdPregunta);
                            var tmpJustifica = document.getElementById('justificacion' + preguntas.IdPregunta).value;
                            if (ObligaObservacionValue.OpcionId == preguntas.Respuesta && tmpJustifica.length <= 5) {
                                if (obsObligatoria != undefined) {
                                    obsObligatoria.classList.remove('hidden');
                                    mensaje = "Falla Validación"
                                    validopregunta = true;
                                }
                            }
                            else {
                                if (obsObligatoria != undefined && validopregunta == false) {
                                    obsObligatoria.classList.add('hidden');
                                }
                            }
                        });
                    }
                });
            });

            let idPreguntaPrincipalGral = 0;
            let respuestaPreguntaPrincipalGral = null;

            vm.PreguntasPersonalizadas.PreguntasGenerales.forEach(generales => {
                generales.Preguntas.forEach(pregunta => {
                    let resObligatoria = document.getElementById('ResObligatoria' + pregunta.IdPregunta);
                    
                    if (!pregunta.IdPreguntaPadre) {
                        idPreguntaPrincipalGral = pregunta.IdPregunta;
                        respuestaPreguntaPrincipalGral = pregunta.Respuesta;
                    }

                    if (idPreguntaPrincipalGral != pregunta.IdPreguntaPadre) {
                        if (!pregunta.Respuesta) {
                            if (resObligatoria != undefined) {
                                resObligatoria.classList.remove('hidden');
                                mensaje = "Falla Validación";
                            }
                        }
                        else {
                            resObligatoria.classList.add('hidden');
                        }
                    }
                    else {
                        if (respuestaPreguntaPrincipalGral) {
                            if (respuestaPreguntaPrincipalGral == '1') {
                                if (!pregunta.Respuesta) {
                                    if (resObligatoria != undefined) {
                                        resObligatoria.classList.remove('hidden');
                                        mensaje = "Falla Validación";
                                    }
                                }
                                else {
                                    resObligatoria.classList.add('hidden');
                                }
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


        function Guardar() {
            return viabilidadSgpServicio.guardarRespuestasCustomSGP(vm.PreguntasPersonalizadas).then(
                function (response) {
                    if (response.data && response.status === 200) {
                        if (response.data.Exito) {
                            parent.postMessage("cerrarModal", window.location.origin);
                            guardarCapituloModificado(vm.nombreComponente);
                            /*                  if (vm.SinPreguntasEspecificas) guardarCapituloModificado();*/
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
            ObtenerSeccionCapitulo("sgpviabilidadviabilidadInformacionBasicaviabilidadsectorialsintramitesgp");
            var data = {
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

        //para guardar los capitulos modificados y que se llenen las lunas
        function ObtenerSeccionCapitulo(nombreComponente) {
            const span = document.getElementById('id-capitulo-' + nombreComponente);
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
                        } else if (TipoError == 'SGRERRVIABSECT') {
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
            var campoObligatorioJustificacion = document.getElementById('ErrorG');
            if (campoObligatorioJustificacion != undefined) {
                campoObligatorioJustificacion.innerHTML = errores;
                campoObligatorioJustificacion.classList.remove('hidden');
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

            var campoObligatorioJustificacion = document.getElementById('ErrorG' + tematica + pregunta);
            if (campoObligatorioJustificacion != undefined) {
                campoObligatorioJustificacion.innerHTML = errores;
                campoObligatorioJustificacion.classList.remove('hidden');
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

            var errorElements = document.getElementsByClassName('errorSeccionViabilidad');
            var testDivs = Array.prototype.filter.call(errorElements, function (errorElement) {
                errorElement.innerHTML = "";
                errorElement.classList.add('hidden');
            });

            vm.PreguntasPersonalizadas.PreguntasGenerales.forEach(generales => {
                generales.Preguntas.forEach(preguntas => {
                    if (generales.OrdenTematica !== null) {
                        var idSpanAlertComponent = document.getElementById("alert-" + vm.nombreComponente + generales.OrdenTematica + preguntas.IdPregunta);
                        if (idSpanAlertComponent != undefined) {
                            idSpanAlertComponent.classList.remove("ico-advertencia");
                        }

                        var idSpanAlertComponentT = document.getElementById("alert-" + vm.nombreComponente + generales.OrdenTematica);
                        if (idSpanAlertComponentT != undefined) {
                            idSpanAlertComponentT.classList.remove("ico-advertencia");
                        }

                        var campoObligatorioJustificacion = document.getElementById('ErrorG' + generales.OrdenTematica + preguntas.IdPregunta);
                        if (campoObligatorioJustificacion != undefined) {
                            campoObligatorioJustificacion.classList.add('hidden');
                        }
                    }
                });
            });
        }



    }

    angular.module('backbone').component('cuestionarioViabilidadSectorialSinTramiteSgp', {
        templateUrl: "/src/app/formulario/ventanas/SGP/comun/cuestionarioSectorialSgp/cuestionarioViabilidadSectorialSinTramiteSgp.html",
        controller: cuestionarioViabilidadSectorialSinTramiteSgpController,
        controllerAs: "vm",
        bindings: {
            callback: '&',
            guardadoevent: '&',
            notificacionvalidacion: '&',
            notificacionestado: '&'
        }
    });
})();