(function () {
    'use strict';
    tecnicoGeneralesController.$inject = [
        'utilidades',
        '$sessionStorage',
        'viabilidadServicio',
        'justificacionCambiosServicio'
    ];

    function tecnicoGeneralesController(
        utilidades,
        $sessionStorage,
        viabilidadServicio,
        justificacionCambiosServicio
    ) {
        var vm = this;
        vm.user = {};
        vm.lang = "es";
        vm.nombreComponente = "viabilidadtecnicogenerales";

        //para guardar los capitulos modificados y que se llenen las lunas
        vm.seccionCapitulo = null;

        vm.Bpin = $sessionStorage.idObjetoNegocio;
        vm.IdNivel = $sessionStorage.idNivel;
        vm.idInstancia = $sessionStorage.idInstancia;
        vm.idAccion = $sessionStorage.idAccion;
        vm.accionDeshabilitada = $sessionStorage.accionDeshabilitada;

        vm.buscarGenerales = buscarGenerales;        

        vm.codigoG;
        vm.Fase;

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
                            Sector: "",
                            Acuerdo: "",
                            Clasificacion: ""
                        }
                    ]
                }
            ]
        }];        

        vm.init = function () {
            DevolverCuestionarioProyecto();
            vm.notificacionvalidacion({ handler: vm.notificacionValidacionPadre, nombreComponente: vm.nombreComponente, esValido: true });
            vm.notificacioncambios({ handler: vm.notificacionCambiosCapitulos, nombreComponente: vm.nombreComponente });
        };

        vm.notificacionCambiosCapitulos = function (nombreCapituloHijo) {
            vm.init();
        }

        vm.BtnTematica = function (campo) {
            var variable = $("#imgT" + campo)[0].outerHTML;
            if (variable.includes('Img/btnMas.svg')) {
                $("#imgT" + campo).attr('src', 'Img/btnMenos.svg');
            }
            else {
                $("#imgT" + campo).attr('src', 'Img/btnMas.svg');
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

                                obtenerPreguntasPersonalizadas(bPin, nivelId, instanciaId, listaRoles);
                            } else {

                            }
                        });
                });
        }

        function obtenerPreguntasPersonalizadas(bPin, nivelId, instanciaId, listaRoles) {
            return viabilidadServicio.obtenerPreguntasPersonalizadas(bPin, nivelId, instanciaId, listaRoles).then(
                function (respuesta) {
                    vm.PreguntasPersonalizadas = respuesta.data;
                    vm.Fase = $sessionStorage.Fase;
                    vm.disabled = $sessionStorage.disabledTecnico;
                }
            );
        }

        function buscarGenerales() {
            limpiarBuscarG(false);
            var resultado = "No se encontraron resultados para esta búsqueda en los contenidos de " + vm.Fase + " técnica.";
            var contador = 0;

            vm.PreguntasPersonalizadas.PreguntasGenerales.forEach(generales => {
                generales.Preguntas.forEach(preguntas => {

                    $("#spanIdT" + preguntas.IdPregunta).html(preguntas.IdPregunta);
                    $("#labelIdT" + preguntas.IdPregunta).html(preguntas.Pregunta);

                    if (preguntas.IdPregunta.toString() === vm.codigoG) {
                        $("#TematicaIdTG" + generales.OrdenTematica).removeClass("collapse");
                        $("#u9_imgTG" + generales.OrdenTematica).removeAttr("style");
                        var pregunta = "<span class='TextConFondoObseDNP'>" + vm.codigoG + "</span>";
                        $("#spanIdT" + preguntas.IdPregunta).html(pregunta);
                        contador++;
                    }

                    if (preguntas.Pregunta.includes(vm.codigoG)) {
                        $("#TematicaIdTG" + generales.OrdenTematica).removeClass("collapse");
                        $("#u9_imgTG" + generales.OrdenTematica).removeAttr("style");
                        var pregunta = preguntas.Pregunta.replace(vm.codigoG, "<span class='TextConFondoObseDNP'>" + vm.codigoG + "</span>");
                        $("#labelIdT" + preguntas.IdPregunta).html(pregunta);
                        contador++;
                    }
                });
            });

            if (contador > 0) {
                resultado = contador.toString() + " resultados en " + vm.Fase + " técnica (Generales). Los desplegables con el marcador <img class='imgViabilidad' src='Img/u9.svg'> contienen alguna coincidencia.";
            }

            $("#labelResultadoTG").html(resultado);
        }

        function limpiarBuscarG(limpiarCampos) {

            if (limpiarCampos) {
                vm.codigo = null;
                vm.palabraclave = null;
            }

            $("#labelResultadoTG").html("");

            vm.PreguntasPersonalizadas.PreguntasGenerales.forEach(generales => {
                generales.Preguntas.forEach(preguntas => {
                    $("#TematicaIdTG" + generales.OrdenTematica).addClass("collapse");
                    $("#u9_imgTG" + generales.OrdenTematica).hide();
                    $("#labelIdT" + preguntas.IdPregunta).html(preguntas.Pregunta);
                });
            });
        }

        vm.ActivarEditar = function () {
            if (vm.activar == true) {
                $("#EditarTG").html("CANCELAR");
                vm.activar = false;
            }
            else {
                $("#EditarTG").html("EDITAR");
                vm.activar = true;
            }
        }        

        vm.guardarTecnico = function (response) {
            vm.PreguntasPersonalizadas.InstanciaId = vm.idInstancia;
            vm.PreguntasPersonalizadas.PreguntasEspecificas = null;
            Guardar();
        }

        function Guardar() {
            return viabilidadServicio.guardarRespuestas(vm.PreguntasPersonalizadas).then(
                function (response) {
                    if (response.data && (response.statusText === "OK" || response.status === 200)) {
                        if (response.data.Exito) {
                            parent.postMessage("cerrarModal", window.location.origin);
                            guardarCapituloModificado();
                            utilidades.mensajeSuccess("Cuestionario guardado satisfactoriamente", false, false, false);

                            $("#EditarTG").html("EDITAR");
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
                        vm.validarValoresTematica(nameArr[0].toString(), false);
                        vm.validarValores(nameArr[0].toString(), nameArr[1].toString(), p.Descripcion, false);
                    });
                }
                vm.notificacionestado({ nombreComponente: vm.nombreComponente, esValido: isValid });
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
        }

        vm.limpiarErrores = function () {
            vm.PreguntasPersonalizadas.PreguntasGenerales.forEach(generales => {
                generales.Preguntas.forEach(preguntas => {
                    var idSpanAlertComponent = document.getElementById("alert-" + vm.nombreComponente + generales.OrdenTematica + preguntas.IdPregunta);
                    idSpanAlertComponent.classList.remove("ico-advertencia");

                    var idSpanAlertComponentT = document.getElementById("alert-" + vm.nombreComponente + generales.OrdenTematica);
                    idSpanAlertComponentT.classList.remove("ico-advertencia");
                });
            });
        }
    }

    angular.module('backbone').component('tecnicoGenerales', {
        templateUrl: "src/app/formulario/ventanas/viabilidad/componentes/preguntasTecnico/generales/tecnicoGenerales.html",
        controller: tecnicoGeneralesController,
        controllerAs: "vm",
        bindings: {
            callback: '&',
            guardadoevent: '&',
            notificacionvalidacion: '&',
            notificacionestado: '&',
            notificacioncambios: '&'
        }
    });
})();