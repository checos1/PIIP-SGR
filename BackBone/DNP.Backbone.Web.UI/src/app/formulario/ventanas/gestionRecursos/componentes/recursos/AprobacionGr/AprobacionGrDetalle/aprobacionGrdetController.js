(function () {
    'use strict';

    aprobacionGrdetController.$inject = ['$scope', 'gestionRecursosServicio', '$sessionStorage', 'utilidades', 'constantesBackbone'];

    function aprobacionGrdetController(
        $scope,
        gestionRecursosServicio,
        $sessionStorage,
        utilidades,
        constantesBackbone
    ) {

        var vm = this;
        vm.init = init;
        vm.BPIN = $sessionStorage.idObjetoNegocio;
        vm.idInstancia = $sessionStorage.idInstancia;
        vm.idFlujo = $sessionStorage.idFlujoIframe;
        vm.idNivel = $sessionStorage.idNivel;
        vm.tramiteId = $sessionStorage.TramiteId;
        vm.tipotramiteId = $sessionStorage.TipoTramiteId;
        vm.ProyectoId = $sessionStorage.proyectoId;
        vm.idFormulario = $sessionStorage.idNivel;
        vm.arrowIcoDown = "glyphicon-chevron-down";
        vm.arrowIcoUp = "glyphicon-chevron-up";

        vm.nombreComponente = "aprobaciongraprobaciongr";
        vm.eventoValidar = eventoValidar;
        vm.seccionCapitulo = null;
        vm.disabled = true;

        var listaValoresOpciones = [];
        var listaPreguntasIdsJefe = [];

        function init() {

            vm.model = {
                modulos: {
                    administracion: false,
                    backbone: true
                }
            }

            obtenerPreguntasJefePlaneacion();
            vm.notificacionvalidacion({ handler: vm.notificacionValidacionPadre, nombreComponente: vm.nombreComponente, esValido: true });
            vm.limpiarErrores();
        }

        vm.notificacionValidacionHijo = function (handler) {
            vm.notificacionErrores = handler;
        }

        vm.changeArrow = function (nombreModificado) {
            var idSpanArrow = 'arrow-' + nombreModificado;
            var arrowCapitulo = document.getElementById(idSpanArrow);
            var arrowClasses = arrowCapitulo.classList;
            for (var i = 0; i < arrowClasses.length; i++) {
                if (arrowClasses[i] == vm.arrowIcoDown) {
                    document.getElementById(idSpanArrow).classList.add(vm.arrowIcoUp);
                    document.getElementById(idSpanArrow).classList.remove(vm.arrowIcoDown);
                    break;
                } else if (arrowClasses[i] == vm.arrowIcoUp) {
                    document.getElementById(idSpanArrow).classList.add(vm.arrowIcoDown);
                    document.getElementById(idSpanArrow).classList.remove(vm.arrowIcoUp);
                    break;
                }
            }
        }

        function obtenerPreguntasJefePlaneacion(tramiteid, tipotramiteId, tipoRolId) {
            return gestionRecursosServicio.obtenerCuestionarioPreguntasJefePlaneacion(0, vm.ProyectoId, 0, vm.idNivel, vm.idFormulario).then(
                function (respuesta) {
                    if (respuesta.data != "") {
                        var arregloPreguntas = jQuery.parseJSON(respuesta.data);
                        var listaPreguntas = jQuery.parseJSON(arregloPreguntas);
                        var listaOpcionesPreguntas = [];
                        var listaOpcionesRespuestas = [];
                        var preguntas = "";
                        var respuestas = "";

                        var tempPregunta = false;

                        for (var io = 0; io < listaPreguntas.length; io++) {

                            listaPreguntasIdsJefe.push(listaPreguntas[io].IdPregunta + "-" + listaPreguntas[io].IdValorOpcion);

                            for (var s = 0; s < listaOpcionesPreguntas.length; s++) {
                                if (listaOpcionesPreguntas[s].idPregunta == listaPreguntas[io].IdPregunta) {
                                    tempPregunta = true;
                                    break;
                                }
                                else
                                    tempPregunta = false;
                            }

                            if (tempPregunta == false) {
                                preguntas = {
                                    idPregunta: listaPreguntas[io].IdPregunta,
                                    Pregunta: listaPreguntas[io].Pregunta,
                                    TipoPregunta: listaPreguntas[io].Tipo,
                                    IdValorOpcion: listaPreguntas[io].IdValorOpcion,
                                    valorIdentificador: listaPreguntas[io].IdPregunta + '-' + listaPreguntas[io].IdValorOpcion
                                }
                                listaOpcionesPreguntas.push(preguntas);
                            }
                        }
                        vm.PreguntasJefe = listaOpcionesPreguntas;

                        for (var x = 0; x < listaPreguntas.length; x++) {
                            listaValoresOpciones.push(listaPreguntas[x].IdValorOpcion);
                            var valorRespuesta = "0";

                            if (listaPreguntas[x].valoresRespuestas != null && listaPreguntas[x].valoresRespuestas.length > 0) {
                                for (var vr = 0; vr < listaPreguntas[x].valoresRespuestas.length; vr++) {
                                    if (listaPreguntas[x].IdPregunta == listaPreguntas[x].valoresRespuestas[vr].PreguntaId) {
                                        valorRespuesta = listaPreguntas[x].valoresRespuestas[vr].Respuesta;
                                        break;
                                    }
                                }
                            }




                            if (listaPreguntas[x].Respuesta != null) {
                                respuestas = {
                                    idPregunta: listaPreguntas[x].IdPregunta,
                                    idRespuesta: listaPreguntas[x].IdValorOpcion,
                                    Respuesta: listaPreguntas[x].Respuesta,
                                    valor: valorRespuesta
                                }
                                listaOpcionesRespuestas.push(respuestas);
                            }
                            else {
                                respuestas = {
                                    idPregunta: 0,
                                    idRespuesta: 0,
                                    Respuesta: 0,
                                    valor: ""
                                }
                                listaOpcionesRespuestas.push(respuestas);
                            }
                        }
                        vm.RespuestasJefe = listaOpcionesRespuestas;
                    }
                });
        }

        vm.registroRespuesta = function () {

            var preguntaId = "";
            var respuestaId = "";
            var respuestas = "";
            var listaRespuestas = [];
            var valorRespuesta = "";
            var controlId = "";
            var rolJefe = false;

            for (var jf = 0; jf < listaPreguntasIdsJefe.length; jf++) {
                controlId = listaPreguntasIdsJefe[jf];

                var idx = listaPreguntasIdsJefe[jf].indexOf('-');
                preguntaId = listaPreguntasIdsJefe[jf].substring(0, idx);
                respuestaId = listaPreguntasIdsJefe[jf].substring(idx + 1, listaPreguntasIdsJefe[jf].length);

                if ($("#" + controlId)[0] != undefined)
                    if ($("#" + controlId)[0].type == 'radio' && $("#" + controlId)[0].checked == true) {
                        rolJefe = true;
                        valorRespuesta = $("#" + controlId)[0].value;
                        respuestas = {
                            ProyectoId: vm.ProyectoId,
                            idPregunta: preguntaId,
                            idRespuesta: respuestaId,
                            valor: valorRespuesta
                        }
                        listaRespuestas.push(respuestas);
                    }
            }

            if (rolJefe == false) {
                var contadorCheck = 0;

                for (var sa = 0; sa < listaPreguntasIds.length; sa++) {
                    controlId = listaPreguntasIds[sa];
                    var idx = listaPreguntasIds[sa].indexOf('-');
                    preguntaId = listaPreguntasIds[sa].substring(0, idx);
                    respuestaId = listaPreguntasIds[sa].substring(idx + 1, listaPreguntasIds[sa].length);
                    if ($("#" + controlId)[0] != 'undefined' && $("#" + controlId)[0].type == 'radio' && $("#" + controlId)[0].checked == true) {
                        valorRespuesta = $("#" + controlId)[0].value;
                        respuestas = {
                            ProyectoId: vm.ProyectoId,
                            idPregunta: preguntaId,
                            idRespuesta: respuestaId,
                            valor: valorRespuesta
                        }
                        listaRespuestas.push(respuestas);
                        contadorCheck = contadorCheck + 1;
                    }

                    if ($("#" + controlId)[0] != 'undefined' && $("#" + controlId)[0].type != 'radio') {
                        valorRespuesta = $("#" + controlId)[0].value;

                        if ($("#" + controlId)[0].type == "date") {
                            var validacionFecha = validateDate(valorRespuesta)
                            if (validacionFecha == false) {
                                utilidades.mensajeError("Error al realizar la operación, verifique la fecha ingresada.", false);
                                return false;
                            }
                        }
                        else if (valorRespuesta.length == 0) {
                            utilidades.mensajeError("Error al realizar la operación, la observacion es obligatoria.", false);
                            return false;
                        }

                        respuestas = {
                            ProyectoId: vm.ProyectoId,
                            idPregunta: preguntaId,
                            idRespuesta: respuestaId,
                            valor: valorRespuesta
                        }
                        listaRespuestas.push(respuestas);
                    }

                }

                if (contadorCheck == 0) {
                    utilidades.mensajeError("Error al realizar la operación, verifique si esta de acuerdo o no.", false);
                    return false;
                }

                var parametros = {
                    nivelId: $sessionStorage.idNivel,
                    tramiteId: $sessionStorage.TramiteId,
                    tipoTramiteId: $sessionStorage.TramiteId,
                    proyectoId: $sessionStorage.proyectoId,
                    faseId: $sessionStorage.idNivel,
                    InstanciaId: $sessionStorage.idInstancia,
                    lstRespuestas: listaRespuestas
                }

                gestionRecursosServicio.GuardarPreguntasAprobacionRolPresupuesto(parametros, usuarioDNP, $sessionStorage.idInstancia)
                    .then(function (response) {
                        let exito = response.data;
                        if (exito) {
                            utilidades.mensajeSuccess("Operación realizada con éxito!", false, false, false);
                            guardarCapituloModificado();
                            vm.ActivarEditar();
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
            else {

                var parametros = {
                    nivelId: $sessionStorage.idNivel,
                    tramiteId: $sessionStorage.TramiteId,
                    tipoTramiteId: $sessionStorage.TramiteId,
                    proyectoId: $sessionStorage.proyectoId,
                    faseId: $sessionStorage.idNivel,
                    InstanciaId: $sessionStorage.idInstancia,
                    lstRespuestas: listaRespuestas
                }

                gestionRecursosServicio.GuardarPreguntasAprobacionRolJefePlaneacion(parametros, usuarioDNP, $sessionStorage.idInstancia)
                    .then(function (response) {
                        let exito = response.data;
                        if (exito) {
                            utilidades.mensajeSuccess("Operación realizada con éxito!", false, false, false);
                            guardarCapituloModificado();
                            vm.ActivarEditar();
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
        }

        function eventoValidar() {
            vm.inicializarComponenteCheck();
            gestionRecursosServicio.obtenerErroresProyecto(vm.guiMacroproceso, vm.ProyectoId, vm.idInstancia).then(function (respuesta) {
                vm.notificacionValidacionHijos(respuesta.data);
                var findErrors = respuesta.data.findIndex(p => p.Errores != null);
                var indexobs = respuesta.data.findIndex(p => p.Capitulo == 'observaciones');
                if (indexobs < 0) { var errorObservacion = false; }
                else { var errorObservacion = respuesta.data[indexobs].Errores == null ? false : true; }
                vm.visualizarAlerta((findErrors != -1), errorObservacion);
            });
        }

        function ObtenerSeccionCapitulo() {
            const span = document.getElementById('id-capitulo-aprobaciongraprobaciongr');
            vm.seccionCapitulo = span.textContent;
        }

        function guardarCapituloModificado() {
            ObtenerSeccionCapitulo();
            var data = {
                Justificacion: "",
                SeccionCapituloId: vm.seccionCapitulo,
                InstanciaId: $sessionStorage.idInstancia,
                Modificado: 1,
                cuenta: 1
            }

            gestionRecursosServicio.guardarCambiosFirme(data)
                .then(function (response) {
                    if (response.data.Exito) {
                        vm.guardadoevent({ nombreComponenteHijo: vm.nombreComponente });
                        // vm.callback();
                    }
                    else {
                        utilidades.mensajeError(response.data.Mensaje + " Capitulo Modificado");
                    }
                });
        }

        vm.ActivarEditar = function () {

            if (vm.disabled == true) {
                vm.esEdicion = true;
                $("#Editar1").html("CANCELAR");
                vm.disabled = false;
                $("#Guardar1").attr('class', 'btnguardarDNP');
                $("#Guardar1").attr('disabled', false);

                listaPreguntasIdsJefe.forEach(function (s) {
                    $("#" + s).attr('disabled', false);
                });


            } else {
                vm.esEdicion = false;
                $("#Editar1").html("EDITAR");
                vm.disabled = true;
                $("#Guardar1").attr('class', 'btnguardarDisabledDNP');
                $("#Guardar1").attr('disabled', true);
                listaPreguntasIdsJefe.forEach(function (s) {
                    $("#" + s).attr('disabled', true);
                });

            }

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
                        if (vm.errores[p.Error] != undefined) {
                            switch (p.Error) {
                                case "AFFR002":
                                    vm.validarAFFR002(p.Descripcion);
                                    break;
                                case "AFFR003":
                                    vm.validarAFFR003(p.Descripcion);
                                    break;
                                default:
                                    vm.limpiarErrores();
                                // code block
                            }

                        }
                    });
                }
                vm.notificacionestado({ nombreComponente: vm.nombreComponente, esValido: isValid });

            }
        };

        vm.limpiarErrores = function () {

            var validacionffr1 = document.getElementById(vm.nombreComponente + "-validacionFuentesErrores1-error");
            var ValidacionFFR1Error = document.getElementById(vm.nombreComponente + "-validacionFuentesErrores1-error-mns");
            if (validacionffr1 != undefined) {
                if (ValidacionFFR1Error != undefined) {
                    ValidacionFFR1Error.innerHTML = "";
                    validacionffr1.classList.add('hidden');
                }
            }

            var validacionffr2 = document.getElementById(vm.nombreComponente + "-validacionFuentesErrores2-error");
            var ValidacionFFR2Error = document.getElementById(vm.nombreComponente + "-validacionFuentesErrores2-error-mns");
            if (validacionffr2 != undefined) {
                if (ValidacionFFR2Error != undefined) {
                    ValidacionFFR2Error.innerHTML = "";
                    validacionffr2.classList.add('hidden');
                }
            }

        }

        vm.validarAFFR001 = function (errores) {
            var ValidacionFFR1 = document.getElementById(vm.nombreComponente + "-validacionFuentesErrores1-error");
            if (ValidacionFFR1 != undefined) {
                var ValidacionFFR1Error = document.getElementById(vm.nombreComponente + "-validacionFuentesErrores1-error-mns");
                if (ValidacionFFR1Error != undefined) {
                    ValidacionFFR1Error.innerHTML = '<span>' + errores + "</span>";
                    ValidacionFFR1.classList.remove('hidden');
                }
            }
        }

        vm.validarAFFR002 = function (errores) {
            var ValidacionFFR2 = document.getElementById(vm.nombreComponente + "-validacionFuentesErrores2-error");
            if (ValidacionFFR2 != undefined) {
                var ValidacionFFR2Error = document.getElementById(vm.nombreComponente + "-validacionFuentesErrores2-error-mns");
                if (ValidacionFFR2Error != undefined) {
                    ValidacionFFR2Error.innerHTML = '<span>' + errores + "</span>";
                    ValidacionFFR2.classList.remove('hidden');
                }
            }
        }

        vm.errores = {
            'APR001': vm.validarAFFR001,
            'APR002': vm.validarAFFR002,
        }

        function validateDate(dateString) {
            let isValidDate = Date.parse(dateString);

            if (isNaN(isValidDate)) {
                // when is not valid date logic

                return false;
            }
            return true;
        }
    }

    angular.module('backbone').component('aprobacionGrdet', {
        templateUrl: "src/app/formulario/ventanas/gestionRecursos/componentes/recursos/aprobaciongr/aprobaciongrdetalle/aprobacionGrdet.html",
        controller: aprobacionGrdetController,
        controllerAs: "vm",
        bindings: {
            callback: '&',
            notificacionvalidacion: '&',
            notificacionestado: '&',
            guardadoevent: '&',
            notificacioncambios: '&',
            guardadocomponent: '&',
        }
    })

})();
