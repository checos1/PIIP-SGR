(function () {
    'use strict';
    datosPresentacionPreviosSgrController.$inject = [
        'utilidades',
        '$sessionStorage',
        'previosSgrServicio',
        'viabilidadSgrServicio',
        'justificacionCambiosServicio',
        '$timeout'
    ];

    function datosPresentacionPreviosSgrController(
        utilidades,
        $sessionStorage,
        previosSgrServicio,
        viabilidadSgrServicio,
        justificacionCambiosServicio,
        $timeout
    ) {
        var vm = this;
        vm.user = {};
        vm.lang = "es";
        vm.nombreComponente = 'sgrviabilidadpreviosdatosgeneralesdatospresentacion';

        //para guardar los capitulos modificados y que se llenen las lunas
        vm.seccionCapitulo = null;

        vm.Bpin = $sessionStorage.idObjetoNegocio;
        vm.IdNivel = $sessionStorage.idNivel;
        vm.idInstancia = $sessionStorage.idInstancia;
        vm.idAccion = $sessionStorage.idAccion;
        vm.proyectoId = $sessionStorage.proyectoId;

        vm.buscarGenerales = buscarGenerales;
        vm.ConvertirNumero = ConvertirNumero;
        vm.limpiarFiltro = limpiarFiltro;

        vm.codigoG;
        vm.exitoFind = false;
        vm.wFiltro = false;

        vm.disabled = false;
        vm.activar = true;
        vm.desactivar = true
        vm.lista;

        vm.erroresComponente = [];

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
            habilitaCo: false,
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
                            TipoPregunta: "",
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
                            IdPreguntaPadre: "",
                            HabilitaEn: "",
                            Sector: "",
                            Acuerdo: "",
                            Clasificacion: "",
                            NivelJer: 0
                        }
                    ]
                }
            ]
        }];

        vm.init = function () {

            vm.notificacionvalidacion({ handler: vm.notificacionValidacionPadre, nombreComponente: vm.nombreComponente, esValido: true });
            vm.notificacioninicio({ handlerInicio: vm.notificacionInicioPadre, nombreComponente: vm.nombreComponente, esValido: true });

        };

        vm.notificacionInicioPadre = function (nombreModificado, errores) {
            if (nombreModificado == vm.nombreComponente) {

                if (errores != null && errores != undefined) {
                    vm.erroresComponente = errores;
                }

                DevolverCuestionarioProyecto();
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
            previosSgrServicio.ConsultarAccionPorInstancia(vm.idInstancia, vm.idAccion).then(
                function (result) {
                    previosSgrServicio.DevolverCuestionarioProyecto(vm.IdNivel, vm.idInstancia, result.data.EstadoAccionPorInstanciaId).then(
                        function (response) {
                            if (response.data || response.statusText === "OK") {
                                var bPin = vm.Bpin;
                                var nivelId = vm.IdNivel;
                                var instanciaId = vm.idInstancia;
                                var listaRoles = "A";

                                obtenerPreguntasPersonalizadas(bPin, nivelId, instanciaId, listaRoles);
                                if (vm.erroresComponente != null && vm.erroresComponente != undefined) {
                                    $timeout(function () {
                                        vm.notificacionValidacionPadre(vm.erroresComponente);
                                    }, 600);
                                }
                            } else {

                            }
                        });
                });
        }

        function obtenerPreguntasPersonalizadas(bPin, nivelId, instanciaId, listaRoles) {
            return previosSgrServicio.obtenerPreguntasPersonalizadasComponente(bPin, nivelId, instanciaId, vm.nombreComponente, listaRoles).then(
                function (respuesta) {
                    vm.PreguntasPersonalizadas = respuesta.data;
                    vm.disabled = $sessionStorage.soloLectura;
                    var idPreAnt = 0;
                    var idPrePadAnt = 0;
                    respuesta.data.PreguntasGenerales.forEach(generales => {
                        generales.Preguntas.forEach(preguntas => {
                            if (preguntas.IdPreguntaPadre != 0) {
                                if (idPreAnt == preguntas.IdPreguntaPadre && idPrePadAnt != 0)
                                    preguntas.NivelJer = 2;
                            }
                            idPreAnt = preguntas.IdPregunta;
                            idPrePadAnt = preguntas.IdPreguntaPadre;

                            if (preguntas.Nota != null) {
                                vm.nombreLista = preguntas.Nota.toString();

                                ObtenerLista(vm.nombreLista);
                            }
                        });
                    });
                }
            );
        }

        function ObtenerLista(nombreLista) {
            vm.lista = "";

            viabilidadSgrServicio.SGR_Proyectos_LeerListas(vm.IdNivel, vm.proyectoId, nombreLista)
                .then(function (response) {
                    if (response.data != null) {
                        vm.lista = response.data;
                    }
                }, function (error) {
                    utilidades.mensajeError('Ocurrió un problema al leer los elementos de la lista.');
                    vm.lista = "";
                });

        }

        function limpiarFiltro() {
            limpiarBuscarG(false);
            vm.wFiltro = false;
        }

        function buscarGenerales() {
            limpiarBuscarG(false);
            if (vm.codigoG != '') {
                var resultado = 'No se encontraron resultados para "' + vm.codigoG + '".';
                var contador = 0;

                vm.PreguntasPersonalizadas.PreguntasGenerales.forEach(generales => {
                    generales.Preguntas.forEach(preguntas => {

                        $("#spanId" + preguntas.IdPregunta).html(preguntas.IdPregunta);
                        $("#labelId" + preguntas.IdPregunta).html(preguntas.Pregunta + '*');

                        if (preguntas.IdPregunta.toString() === vm.codigoG) {
                            $("#TematicaIdG" + generales.OrdenTematica).removeClass("collapse");
                            var pregunta = "<span class='TextConFondoObseDNP'>" + vm.codigoG + "</span>";
                            $("#spanId" + preguntas.IdPregunta).html(pregunta);
                            contador++;
                        }

                        if (preguntas.Pregunta.includes(vm.codigoG)) {
                            $("#TematicaIdG" + generales.OrdenTematica).removeClass("collapse");
                            var pregunta = (preguntas.Pregunta + '*').replace(vm.codigoG, "<span class='TextConFondoObseDNP'>" + vm.codigoG + "</span>");
                            $("#labelId" + preguntas.IdPregunta).html(pregunta);
                            contador++;
                        }
                    });
                });

                if (contador > 0) {
                    resultado = 'Se encontraron ' + contador.toString() + ' resultados para "' + vm.codigoG + '".';
                    vm.exitoFind = true;
                } else
                    vm.exitoFind = false;

                $("#labelResultadoG").html(resultado);
                vm.wFiltro = true;
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
                    var preguntaPer = preguntas.Pregunta + '*';
                    $("#labelId" + preguntas.IdPregunta).html(preguntaPer);
                });
            });
        }

        vm.HabilitarPregunta = function (event, fila, index1, index2) {
            var habilita = vm.PreguntasPersonalizadas.PreguntasGenerales[index1].Preguntas[index2].Respuesta == 1 ? 'SI' : 'NO';

            vm.PreguntasPersonalizadas.PreguntasGenerales.forEach(generales => {
                generales.Preguntas.forEach(preguntas => {

                    if (preguntas.IdPreguntaPadre == fila) {
                        if (preguntas.HabilitaEn == habilita) {
                            $("#pregunta" + preguntas.IdPregunta).removeAttr('disabled');
                            preguntas.habilitaCo = true;
                            $("input[name=Respuesta" + preguntas.IdPregunta + "]").removeAttr('disabled');
                            if (preguntas.Nota != null) {
                                vm.nombreLista = preguntas.Nota.toString();
                                ObtenerLista(vm.nombreLista);
                            }
                        }
                        else {
                            $("#pregunta" + preguntas.IdPregunta).attr('disabled', 'disabled');
                            preguntas.habilitaCo = false;
                            $("input[name=Respuesta" + preguntas.IdPregunta + "]").attr("disabled", true);
                            $("#pregunta" + preguntas.IdPregunta)[0].value = '0';
                            preguntas.Respuesta = '0';
                        }
                    } else {
                        if (preguntas.IdPreguntaPadre != null && preguntas.IdPreguntaPadre != 0 && $("#pregunta" + preguntas.IdPreguntaPadre).attr('disabled') == 'disabled') {
                            preguntas.habilitaCo = false;
                            $("#pregunta" + preguntas.IdPregunta).attr('disabled', 'disabled');
                            $("input[name=Respuesta" + preguntas.IdPregunta + "]").attr("disabled", true);
                            $("#pregunta" + preguntas.IdPregunta)[0].value = '0';
                            preguntas.Respuesta = '0';
                        }
                    }
                });
            });
        }

        vm.ActivarEditar = function () {
            if (vm.activar == true) {
                $("#EditarG").html("CANCELAR");
                vm.activar = false;
                HabilitarDependientes();
            }
            else {
                utilidades.mensajeWarning("¿Está seguro de continuar?", function funcionContinuar() {
                    OkCancelar();

                    $("#EditarG").html("EDITAR");
                    vm.activar = true;
                    DeshabilitarDependientes();
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

        function DeshabilitarDependientes() {
            vm.PreguntasPersonalizadas.PreguntasGenerales.forEach(generales => {
                generales.Preguntas.forEach(preguntas => {
                    $("#pregunta" + preguntas.IdPregunta).attr('disabled', 'disabled');
                    preguntas.habilitaCo = false;
                    $("input[name=Respuesta" + preguntas.IdPregunta + "]").attr("disabled", true);
                });
            });
        }

        function HabilitarDependientes() {
            vm.PreguntasPersonalizadas.PreguntasGenerales.forEach(generales => {
                generales.Preguntas.forEach(preguntas => {
                    if (preguntas.IdPreguntaPadre != null && (preguntas.IdPreguntaPadre != 0 && ($("input[name=Respuesta" + preguntas.IdPreguntaPadre + "]:checked")[0] == undefined || $("input[name=Respuesta" + preguntas.IdPreguntaPadre + "]:checked")[0].value != 1))) {
                        $("#pregunta" + preguntas.IdPregunta).attr('disabled', 'disabled');
                        preguntas.habilitaCo = false;
                        $("input[name=Respuesta" + preguntas.IdPregunta + "]").attr("disabled", true);
                    }
                    else {
                        $("#pregunta" + preguntas.IdPregunta).removeAttr('disabled');
                        preguntas.habilitaCo = true;
                        $("input[name=Respuesta" + preguntas.IdPregunta + "]").removeAttr('disabled');
                    }
                });
            });
        }

        vm.guardar = function (response) {
            vm.PreguntasPersonalizadas.InstanciaId = vm.idInstancia;
            vm.PreguntasPersonalizadas.PreguntasEspecificas = null;
            vm.PreguntasPersonalizadas.Definitivo = false;
            ObtenerSeccionCapitulo(vm.nombreComponente);
            Guardar();
        }

        function Guardar() {
            return previosSgrServicio.guardarRespuestasCustomSGR(vm.PreguntasPersonalizadas).then(
                function (response) {
                    if (response.data || response.statusText === "OK") {
                        if (response.data.Exito) {
                            parent.postMessage("cerrarModal", "*");
                            guardarCapituloModificado();
                            utilidades.mensajeSuccess("", false, false, false);
                            $("#EditarG").html("EDITAR");
                            vm.activar = true;
                            DeshabilitarDependientes();
                            vm.limpiarErrores();

                            var preguntaCredito = utilidades.obtenerParametroTransversal("IdPreguntaOperacionesCredito");
                            var respuestasGenerales = vm.PreguntasPersonalizadas.PreguntasGenerales.find(p => p.Preguntas.find(o => o.IdPregunta == preguntaCredito));
                            var elemento = typeof respuestasGenerales !== 'undefined' ? respuestasGenerales.Preguntas.find(x => x.IdPregunta == preguntaCredito) : undefined;
                            var respuestaCredito = typeof elemento !== 'undefined' ? elemento.Respuesta : undefined;

                            previosSgrServicio.notificarCambio({ Capitulo: vm.nombreComponente, RespuestaCredito: respuestaCredito });
                        } else {
                            swal('', response.data.Mensaje, 'warning');
                        }

                    } else {
                        swal('', "Error al realizar la operación", 'error');
                    }
                }
            );
        }

        function ObtenerSeccionCapitulo(nombreComponente) {
            const span = document.getElementById('id-capitulo-' + nombreComponente);
            vm.seccionCapitulo = span.textContent;
            vm.PreguntasPersonalizadas.SeccionCapituloId = vm.seccionCapitulo;
        }

        //para guardar los capitulos modificados y que se llenen las lunas
        function guardarCapituloModificado() {
            ObtenerSeccionCapitulo(vm.nombreComponente);
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


        vm.validarTamanioTexto = function (event, tamanioPermitido) {

            var newValue = event.target.value;
            var spiltArray = String(newValue).split("");
            var tamanio = event.target.value.length;

            if (tamanio > tamanioPermitido) {
                event.target.value = event.target.value.toString().substring(0, tamanioPermitido)
            }
        }

        vm.validateFormat = function (event) {

            if ((event.keyCode < 48 || event.keyCode > 57) && event.keyCode != 44) {
                event.preventDefault();
            }
        }

        vm.validateFormatNumeric = function (event) {
            const pattern = /[0-9\+\-\ ]/;
            let inputChar = String.fromCharCode(event.key);

            if (event.keyCode < 48 || event.keyCode > 57) {
                // invalid character, prevent input
                event.preventDefault();
            }
        }

        vm.validateFormat = function (event) {

            if ((event.keyCode < 48 || event.keyCode > 57) && event.keyCode != 44) {
                event.preventDefault();
            }
        }

        vm.validarTamanio = function (event) {

            if (Number.isNaN(event.target.value)) {
                event.target.value = "0"
                return;
            }

            if (event.target.value == null) {
                event.target.value = "0"
                return;
            }

            if (event.target.value == "") {
                event.target.value = "0"
                return;
            }

            event.target.value = event.target.value.replace(",", ".");

            var newValue = event.target.value;
            var spiltArray = String(newValue).split("");
            var tamanioPermitido = 16;
            var tamanio = event.target.value.length;
            var decimal = false;
            decimal = event.target.value.toString().includes(".");
            if (decimal) {
                tamanioPermitido = 19;

                var n = String(newValue).split(".");
                if (n[1]) {
                    var n2 = n[1].slice(0, 2);
                    newValue = [n[0], n2].join(".");
                    event.target.value = newValue;
                    if (spiltArray.length === 0) return;
                    if (spiltArray.length === 1 && (spiltArray[0] == '-' || spiltArray[0] === '.')) return;
                    if (spiltArray.length === 2 && newValue === '-.') return;

                    if (n[1].length > 2) {
                        tamanioPermitido = n[0].length + 2;
                        event.target.value = n[0] + '.' + n[1].slice(0, 2);
                        return;
                    }

                    if ((n[1].length == 3 && n[1] > 999) || (n[1].length > 3 && n[1] > 9999)) {
                        event.preventDefault();
                    }
                }
                else {
                    var n2 = "";
                    newValue = [n[0], n2].join(".");
                    event.target.value = newValue;
                }
            }
            else {
                if (tamanio > tamanioPermitido && event.keyCode != 44) {
                    event.target.value = event.target.value.slice(0, tamanioPermitido);
                    event.preventDefault();
                }
            }
        }

        function ConvertirNumero(numero) {
            return new Intl.NumberFormat('es-co', {
                minimumFractionDigits: 2,
            }).format(numero);
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
                        if (TipoError == 'SGRVDP1') {
                            vm.validarValores2(nameArr[1].toString(), p.Descripcion, false, "A");
                        }
                        else if (TipoError == 'SGRVDP2') {
                            vm.validarValores2(nameArr[1].toString(), p.Descripcion, false, "B");
                        }
                        else if (TipoError == 'SGRERRSEC') {
                            vm.validarSeccion(TipoError, nameArr[1].toString(), p.Descripcion, false);
                        }
                        else if (TipoError == 'REQ') {
                            vm.validarValoresOblig(nameArr[1].toString(), p.Descripcion, false);
                        }
                        else
                            vm.validarValores(nameArr[0].toString(), p.Descripcion, false);
                    });
                }
                vm.notificacionestado({ nombreComponente: vm.nombreComponente, esValido: isValid });
            }
        }

        vm.validarValores = function (pregunta, errores, esValido) {
            var idSpanAlertComponent = document.getElementById("alert-" + vm.nombreComponente + pregunta);
            if (idSpanAlertComponent != undefined) {
                if (!esValido) {
                    idSpanAlertComponent.classList.add("ico-advertencia");
                    var campomensajeerror = document.getElementById(vm.nombreComponente + pregunta);
                    if (campomensajeerror != undefined) {
                        campomensajeerror.innerHTML = "<span class='d-inline-block ico-advertencia'></span><span class='pl-2'>" + errores + "</span>";
                        campomensajeerror.classList.remove('hidden');
                    }

                } else {
                    idSpanAlertComponent.classList.remove("ico-advertencia");
                }
            }
        }

        vm.validarValoresOblig = function (pregunta, errores, esValido) {
            var idSpanAlertComponent = document.getElementById("alertE-" + vm.nombreComponente + pregunta);
            if (idSpanAlertComponent != undefined) {
                if (!esValido) {
                    idSpanAlertComponent.classList.add("ico-advertencia");
                    var campomensajeerror = document.getElementById("ERR-" + vm.nombreComponente + pregunta);
                    if (campomensajeerror != undefined) {
                        campomensajeerror.innerHTML = errores;
                        campomensajeerror.classList.remove('hidden');
                    }

                } else {
                    idSpanAlertComponent.classList.remove("ico-advertencia");
                }
            }
        }

        vm.validarValores2 = function (pregunta, errores, esValido, Ident) {
            var idSpanAlertComponent = document.getElementById("alert-" + vm.nombreComponente + pregunta);
            if (idSpanAlertComponent != undefined) {
                if (!esValido) {
                    idSpanAlertComponent.classList.add("ico-advertencia");
                    var campomensajeerror = document.getElementById(vm.nombreComponente + Ident + pregunta);
                    if (campomensajeerror != undefined) {
                        campomensajeerror.innerHTML = "<span class='d-inline-block ico-advertencia'></span><span>" + errores + "</span>";
                        campomensajeerror.classList.remove('hidden');
                    }

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
            if (vm.PreguntasPersonalizadas.PreguntasGenerales != null && vm.PreguntasPersonalizadas.PreguntasGenerales != undefined ) {


                vm.PreguntasPersonalizadas.PreguntasGenerales.forEach(generales => {
                    generales.Preguntas.forEach(preguntas => {
                        var idSpanAlertComponent = document.getElementById("alert-" + vm.nombreComponente + preguntas.IdPregunta.toString());
                        if (idSpanAlertComponent != undefined) {
                            idSpanAlertComponent.classList.remove("ico-advertencia");
                        }
                        var idSpanAlertComponent = document.getElementById("alertE-" + vm.nombreComponente + preguntas.IdPregunta.toString());
                        if (idSpanAlertComponent != undefined) {
                            idSpanAlertComponent.classList.remove("ico-advertencia");
                        }
                        var campomensajeerror = document.getElementById(vm.nombreComponente + preguntas.IdPregunta.toString());
                        if (campomensajeerror != undefined) {
                            campomensajeerror.innerHTML = "";
                            campomensajeerror.classList.add('hidden');
                        }
                        var campomensajeerror2 = document.getElementById(vm.nombreComponente + "A" + preguntas.IdPregunta.toString());
                        if (campomensajeerror2 != undefined) {
                            campomensajeerror2.innerHTML = "";
                            campomensajeerror2.classList.add('hidden');
                        }
                        var campomensajeerror3 = document.getElementById(vm.nombreComponente + "B" + preguntas.IdPregunta.toString());
                        if (campomensajeerror3 != undefined) {
                            campomensajeerror3.innerHTML = "";
                            campomensajeerror3.classList.add('hidden');
                        }
                        var campomensajeerror4 = document.getElementById("ERR-" + vm.nombreComponente + preguntas.IdPregunta.toString());
                        if (campomensajeerror4 != undefined) {
                            campomensajeerror4.innerHTML = "";
                            campomensajeerror4.classList.add('hidden');
                        }

                        var idSpanAlertCapitulo = document.getElementById("alert-" + vm.nombreComponente);
                        if (idSpanAlertCapitulo != undefined) {
                            idSpanAlertCapitulo.classList.remove("ico-advertencia");
                        }
                    });
                });
            }
        }
    }

    angular.module('backbone').component('datosPresentacionPreviosSgr', {
        templateUrl: "/src/app/formulario/ventanas/SGR/viabilidadSGR/previos/datosGeneralesPrevios/datosPresentacion/datosPresentacionPreviosSgr.html",
        controller: datosPresentacionPreviosSgrController,
        controllerAs: "vm",
        bindings: {
            callback: '&',
            guardadoevent: '&',
            notificacionvalidacion: '&',
            notificacionestado: '&',
            notificacioninicio: '&',
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
        })
        .directive('stringToNumeric', function () {
            return {
                require: 'ngModel',
                link: function (scope, element, attrs, ngModel) {
                    ngModel.$parsers.push(function (value) {

                        return '' + value;
                    });
                    ngModel.$formatters.push(function (value) {
                        return parseInt(value);
                    });
                }
            }
        });
})();