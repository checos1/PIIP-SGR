(function () {
    'use strict';
    liderEspecificasController.$inject = [
        'utilidades',
        '$sessionStorage',
        'viabilidadServicio',
        'justificacionCambiosServicio',
    ];

    function liderEspecificasController(
        utilidades,
        $sessionStorage,
        viabilidadServicio,
        justificacionCambiosServicio,
    ) {
        var vm = this;
        vm.user = {};
        vm.lang = "es";
        vm.nombreComponente = "viabilidadespecificas";

        //para guardar los capitulos modificados y que se llenen las lunas
        vm.seccionCapitulo = null;

        vm.Bpin = $sessionStorage.idObjetoNegocio;
        vm.IdNivel = $sessionStorage.idNivel;
        vm.idInstancia = $sessionStorage.idInstancia;
        vm.idAccion = $sessionStorage.idAccion;

        vm.buscarEspecificas = buscarEspecificas;

        vm.codigoE;

        vm.disabled = false;
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
            PreguntasEspecificas: [
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
                            Sector: "",
                            Acuerdo: "",
                            Clasificacion: ""
                        }
                    ]
                }
            ],
            PreguntasGenerales: null
        }];

        vm.init = function () {
            DevolverCuestionarioProyecto();
            vm.notificacionvalidacion({ handler: vm.notificacionValidacionPadre, nombreComponente: vm.nombreComponente, esValido: true });
        };

        vm.BtnTematica = function (campo) {
            var variable = $("#imgE" + campo)[0].outerHTML;
            if (variable.includes('Img/btnMas.svg')) {
                $("#imgE" + campo).attr('src', 'Img/btnMenos.svg');
            }
            else {
                $("#imgE" + campo).attr('src', 'Img/btnMas.svg');
            }
        }

        function DevolverCuestionarioProyecto() {
            viabilidadServicio.ConsultarAccionPorInstancia(vm.idInstancia, vm.idAccion).then(
                function (result) {
                    viabilidadServicio.DevolverCuestionarioProyecto(vm.IdNivel, vm.idInstancia, result.data.EstadoAccionPorInstanciaId).then(
                        function (response) {
                            if (response.data && (response.statusText === "OK" || response.status === 200)) {
                                var bPin = vm.Bpin;
                                var nivelId = vm.IdNivel;
                                var instanciaId = vm.idInstancia;
                                var listaRoles = "A";

                                obtenerPreguntasPersonalizadasLider(bPin, nivelId, instanciaId, listaRoles);
                            } else {

                            }
                        });
                });
        }

        function obtenerPreguntasPersonalizadasLider(bPin, nivelId, instanciaId, listaRoles) {
            return viabilidadServicio.obtenerPreguntasPersonalizadas(bPin, nivelId, instanciaId, listaRoles).then(
                function (respuesta) {
                    vm.PreguntasPersonalizadas = respuesta.data;
                    vm.disabled = $sessionStorage.soloLectura;
                }
            );
        }

        function buscarEspecificas() {
            limpiarBuscarE(false);
            if (vm.codigoE != '') {
                var resultado = "No se encontraron resultados para esta búsqueda.";
                var contador = 0;

                vm.PreguntasPersonalizadas.PreguntasEspecificas.forEach(especificas => {
                    especificas.Preguntas.forEach(preguntas => {

                        $("#spanId" + preguntas.IdPregunta).html(preguntas.IdPregunta);
                        $("#labelId" + preguntas.IdPregunta).html(preguntas.Pregunta);

                        if (preguntas.IdPregunta.toString() === vm.codigoE) {
                            $("#TematicaIdE" + especificas.OrdenTematica).removeClass("collapse");
                            $("#u9_imgE" + especificas.OrdenTematica).removeAttr("style");
                            var pregunta = "<span class='TextConFondoObseDNP'>" + vm.codigoE + "</span>";
                            $("#spanId" + preguntas.IdPregunta).html(pregunta);
                            contador++;
                        }

                        if (preguntas.Pregunta.includes(vm.codigoE)) {
                            $("#TematicaIdE" + especificas.OrdenTematica).removeClass("collapse");
                            $("#u9_imgE" + especificas.OrdenTematica).removeAttr("style");
                            var pregunta = preguntas.Pregunta.replace(vm.codigoE, "<span class='TextConFondoObseDNP'>" + vm.codigoE + "</span>");
                            $("#labelId" + preguntas.IdPregunta).html(pregunta);
                            contador++;
                        }
                    });
                });

                if (contador > 0) {
                    resultado = contador.toString() + " resultados en (Específicas). Los desplegables con el marcador <img class='imgViabilidad' src='Img/u9.svg'> contienen alguna coincidencia.";
                }

                $("#labelResultadoE").html(resultado);
            }
        }

        function limpiarBuscarE(limpiarCampos) {

            if (limpiarCampos) {
                vm.codigo = null;
                vm.palabraclave = null;
            }

            $("#labelResultadoE").html("");

            vm.PreguntasPersonalizadas.PreguntasEspecificas.forEach(especificas => {
                especificas.Preguntas.forEach(preguntas => {
                    $("#TematicaIdE" + especificas.OrdenTematica).addClass("collapse");
                    $("#u9_imgE" + especificas.OrdenTematica).hide();
                    $("#labelId" + preguntas.IdPregunta).html(preguntas.Pregunta);
                });
            });
        }

        vm.ActivarEditar = function () {
            if (vm.activar == true) {
                $("#EditarE").html("CANCELAR");
                vm.activar = false;
            }
            else {
                $("#EditarE").html("EDITAR");
                vm.activar = true;
            }
        }

        vm.guardarLider = function (response) {
            vm.PreguntasPersonalizadas.InstanciaId = vm.idInstancia;
            vm.PreguntasPersonalizadas.PreguntasGenerales = null;
            vm.PreguntasPersonalizadas.Definitivo = false;
            Guardar();
        }

        function Guardar() {
            return viabilidadServicio.guardarRespuestas(vm.PreguntasPersonalizadas).then(
                function (response) {
                    if (response.data && response.status === 200) {
                        if (response.data.Exito) {
                            parent.postMessage("cerrarModal");
                            guardarCapituloModificado();
                            utilidades.mensajeSuccess("Cuestionario guardado satisfactoriamente", false, false, false);

                            $("#EditarE").html("EDITAR");
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
        function guardarCapituloModificado() {
            ObtenerSeccionCapitulo();
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
            vm.limpiarErrores();
            if (errores != undefined) {
                var erroresRelacionconlapl = errores.find(p => (p.Seccion + p.Capitulo) == vm.nombreComponente);
                var erroresJson = erroresRelacionconlapl.Errores == "" ? [] : JSON.parse(erroresRelacionconlapl.Errores);
                var isValid = (erroresJson == null || erroresJson.length == 0);
                if (!isValid) {
                    erroresJson[vm.nombreComponente].forEach(p => {
                        var nameArr = p.Error.split('-');
                        var TipoError = nameArr[0].toString();
                        if (TipoError == 'ErrorE') {
                            vm.validarValoresPasoAnterior(p.Descripcion);
                        }
                        else {
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

            var campoObligatorioJustificacion = document.getElementById('ErrorE' + tematica + pregunta);
            if (campoObligatorioJustificacion != undefined) {
                campoObligatorioJustificacion.innerHTML = errores;
                campoObligatorioJustificacion.classList.remove('hidden');
            }
        }

        vm.limpiarErrores = function () {
            var errorValoresPasoAnterior = document.getElementById('ErrorE');
            errorValoresPasoAnterior.classList.add('hidden');

            if (vm.PreguntasPersonalizadas.PreguntasEspecificas != undefined)
                vm.PreguntasPersonalizadas.PreguntasEspecificas.forEach(especificas => {
                    especificas.Preguntas.forEach(preguntas => {
                        var idSpanAlertComponent = document.getElementById("alert-" + vm.nombreComponente + especificas.OrdenTematica + preguntas.IdPregunta);
                        idSpanAlertComponent.classList.remove("ico-advertencia");

                        var idSpanAlertComponentT = document.getElementById("alert-" + vm.nombreComponente + especificas.OrdenTematica);
                        idSpanAlertComponentT.classList.remove("ico-advertencia");

                        var campoObligatorioJustificacion = document.getElementById('ErrorE' + especificas.OrdenTematica + preguntas.IdPregunta);
                        campoObligatorioJustificacion.classList.add('hidden');
                    });
                });
        }
    }

    angular.module('backbone').component('liderEspecificas', {
        templateUrl: "src/app/formulario/ventanas/viabilidad/componentes/preguntasLider/especificas/liderEspecificas.html",
        controller: liderEspecificasController,
        controllerAs: "vm",
        bindings: {
            callback: '&',
            guardadoevent: '&',
            notificacionvalidacion: '&',
            notificacionestado: '&'
        }
    });
})();