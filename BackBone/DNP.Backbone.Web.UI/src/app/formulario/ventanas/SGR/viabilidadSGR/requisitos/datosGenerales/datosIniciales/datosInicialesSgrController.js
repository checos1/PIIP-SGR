(function () {
    'use strict';
    datosInicialesSgrController.$inject = [
        '$scope',
        'utilidades',
        '$sessionStorage',
        'requisitosSgrServicio',
        'justificacionCambiosServicio',
    ];

    function datosInicialesSgrController(
        $scope,
        utilidades,
        $sessionStorage,
        requisitosSgrServicio,
        justificacionCambiosServicio,
    ) {
        var vm = this;
        vm.user = {};
        vm.lang = "es";
        vm.nombreComponente = "sgrviabilidadrequisitosdatosgeneralesdatosinicialesverificacion";

        //para guardar los capitulos modificados y que se llenen las lunas
        vm.seccionCapitulo = null;
        vm.proyectoId = $sessionStorage.proyectoId;
        vm.Bpin = $sessionStorage.idObjetoNegocio;
        vm.IdNivel = $sessionStorage.idNivel;
        vm.idInstancia = $sessionStorage.idInstancia;
        vm.idAccion = $sessionStorage.idAccion;
        vm.habilitaBotones = $sessionStorage.soloLectura ? false : true;

        vm.buscarGenerales = buscarGenerales;
        vm.ConvertirNumero = ConvertirNumero;
        vm.limpiarFiltro = limpiarFiltro;
        vm.nombreLista;
        vm.activarSelect = false;
        vm.lista;

        vm.codigoG;
        vm.exitoFind = false;
        vm.wFiltro = false;
        vm.preguntaCredito = 3816;
        $sessionStorage.habilitaOperacionCredito = false;

        vm.disabled = false;
        vm.activar = true;
        vm.desactivar = true


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
                            Clasificacion: ""
                        }
                    ]
                }
            ]
        }];

        vm.init = function () {
            DevolverCuestionarioProyecto();
            vm.notificacionvalidacion({ handler: vm.notificacionValidacionPadre, nombreComponente: vm.nombreComponente, esValido: true });
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
            requisitosSgrServicio.ConsultarAccionPorInstancia(vm.idInstancia, vm.idAccion).then(
                function (result) {
                    requisitosSgrServicio.DevolverCuestionarioProyecto(vm.IdNivel, vm.idInstancia, result.data.EstadoAccionPorInstanciaId).then(
                        function (response) {
                            if (response.data || response.statusText === "OK") {
                                var bPin = vm.Bpin;
                                var nivelId = vm.IdNivel;
                                var instanciaId = vm.idInstancia;
                                var listaRoles = "A";

                                obtenerPreguntasPersonalizadasComponente(bPin, nivelId, instanciaId, vm.nombreComponente, listaRoles);
                            } else {

                            }
                        });
                });
        }

        $scope.$on("HabilitarGuardarPaso", function (evt, data) {
            vm.habilitaBotones = data;
        });

        function obtenerPreguntasPersonalizadasComponente(bPin, nivelId, instanciaId, nombreComponente, listaRoles) {
            return requisitosSgrServicio.obtenerPreguntasPersonalizadasComponente(bPin, nivelId, instanciaId, nombreComponente, listaRoles).then(
                function (respuesta) {

                    vm.PreguntasPersonalizadas = respuesta.data;
                    vm.disabled = $sessionStorage.soloLectura;
                    respuesta.data.PreguntasGenerales.forEach(generales => {
                        generales.Preguntas.forEach(preguntas => {

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
            if (nombreLista.length > 0) {
                requisitosSgrServicio.SGR_Proyectos_LeerListas(vm.IdNivel, vm.proyectoId, nombreLista)
                    .then(function (response) {
                        if (response.data != null) {
                            vm.lista = response.data;
                        }
                    }, function (error) {
                        utilidades.mensajeError('Ocurrió un problema al leer los elementos de la lista.');
                        vm.lista = "";
                    });
            }
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

            if (fila == vm.preguntaCredito && habilita == 'SI') {
                $sessionStorage.habilitaOperacionCredito = true;
            } else if (fila == vm.preguntaCredito && habilita == 'NO') {
                $sessionStorage.habilitaOperacionCredito = false;
            }

            vm.PreguntasPersonalizadas.PreguntasGenerales.forEach(generales => {
                generales.Preguntas.forEach(preguntas => {

                    if (preguntas.IdPreguntaPadre == fila) {
                        if (preguntas.HabilitaEn == habilita) {
                            $("#pregunta" + preguntas.IdPregunta).removeAttr('disabled');
                            preguntas.habilitaCo = true;
                            $("input[name=Respuesta" + preguntas.IdPregunta + "]").removeAttr('disabled');
                            preguntas.Respuesta = "";
                            vm.nombreLista = preguntas.Nota.toString();
                            ObtenerLista(vm.nombreLista);
                        }
                        else {
                            $("#pregunta" + preguntas.IdPregunta).attr('disabled', 'disabled');
                            preguntas.habilitaCo = false;
                            $("input[name=Respuesta" + preguntas.IdPregunta + "]").attr("disabled", true);
                            $("#pregunta" + preguntas.IdPregunta)[0].value = '0';
                            preguntas.Respuesta = "";
                            vm.nombreLista = preguntas.Nota.toString();
                            ObtenerLista(vm.nombreLista);
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

                    if (preguntas.Nota != null) {
                        vm.activarSelect = false;
                    }
                });
            });
        }

        function HabilitarDependientes() {
            vm.PreguntasPersonalizadas.PreguntasGenerales.forEach(generales => {
                generales.Preguntas.forEach(preguntas => {

                    if (preguntas.IdPreguntaPadre != null && (preguntas.IdPreguntaPadre != 0 && $("input[name=Respuesta" + preguntas.IdPreguntaPadre + "]:checked")[0].value != 1)) {
                        $("#pregunta" + preguntas.IdPregunta).attr('disabled', 'disabled');
                        preguntas.habilitaCo = false;
                        $("input[name=Respuesta" + preguntas.IdPregunta + "]").attr("disabled", true);
                        if (preguntas.Nota != null) {
                            vm.activarSelect = false;
                        }
                    }
                    else {
                        $("#pregunta" + preguntas.IdPregunta).removeAttr('disabled');
                        preguntas.habilitaCo = true;
                        $("input[name=Respuesta" + preguntas.IdPregunta + "]").removeAttr('disabled');
                        if (preguntas.Nota != null) {
                            vm.activarSelect = true;
                        }
                    }


                });
            });
        }

        vm.guardar = function (response) {
            vm.PreguntasPersonalizadas.InstanciaId = vm.idInstancia;
            vm.PreguntasPersonalizadas.PreguntasEspecificas = null;
            vm.PreguntasPersonalizadas.Definitivo = false;
            ObtenerSeccionCapitulo();
            Guardar();
        }

        function Guardar() {
            return requisitosSgrServicio.guardarRespuestasCustomSGR(vm.PreguntasPersonalizadas).then(
                function (response) {
                    if (response.data || response.statusText === "OK") {
                        if (response.data.Exito) {
                            parent.postMessage("cerrarModal", "*");
                            guardarCapituloModificado();
                            utilidades.mensajeSuccess("", false, false, false);

                            $("#EditarG").html("EDITAR");
                            vm.activar = true;
                            DeshabilitarDependientes();
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
                        vm.guardadocomponent({ nombreComponenteHijo: vm.nombreComponente });
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

            var newValue = event.target.value;
            var spiltArray = String(newValue).split("");
            var tamanioPermitido = 11;
            var tamanio = event.target.value.length;
            var permitido = false;
            permitido = event.target.value.toString().includes(".");
            if (permitido) {
                var indicePunto = event.target.value.toString().indexOf(".");
                var decimales = event.target.value.toString().substring(indicePunto, tamanio).length;
                if (decimales > 2) {
                }
                tamanioPermitido = 16;

                var n = String(newValue).split(".");
                if (n[1]) {
                    var n2 = n[1].slice(0, 2);
                    newValue = [n[0], n2].join(".");
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
            }
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

            var newValue = event.target.value;
            var spiltArray = String(newValue).split("");
            var tamanioPermitido = 11;
            var tamanio = event.target.value.length;
            var permitido = false;
            permitido = event.target.value.toString().includes(".");
            if (permitido) {
                tamanioPermitido = 16;

                var n = String(newValue).split(".");
                if (n[1]) {
                    var n2 = n[1].slice(0, 2);
                    newValue = [n[0], n2].join(".");
                    if (spiltArray.length === 0) return;
                    if (spiltArray.length === 1 && (spiltArray[0] == '-' || spiltArray[0] === '.')) return;
                    if (spiltArray.length === 2 && newValue === '-.') return;

                    if (n[1].length > 4) {
                        tamanioPermitido = n[0].length + 2;
                        event.target.value = n[0] + '.' + n[1].slice(0, 2);
                        return;
                    }

                    if ((n[1].length == 3 && n[1] > 999) || (n[1].length > 3 && n[1] > 9999)) {
                        event.preventDefault();
                    }
                }
            } else {
                if (tamanio > 12 && event.keyCode != 44) {
                    event.preventDefault();
                }
            }

            if (event.keyCode === 44 && tamanio == 12) {
            }
            else {
                if (tamanio > tamanioPermitido || tamanio > 16) {
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
                if (erroresRelacionconlapl != undefined) {
                    var erroresJson = erroresRelacionconlapl.Errores == "" ? [] : JSON.parse(erroresRelacionconlapl.Errores);
                    var isValid = (erroresJson == null || erroresJson.length == 0);
                    if (!isValid) {
                        erroresJson[vm.nombreComponente].forEach(p => {
                            var nameArr = p.Error.split('-');
                            var TipoError = nameArr[0].toString();
                            if (TipoError == 'SGRERRSEC') {
                                vm.validarSeccion(TipoError, nameArr[1].toString(), p.Descripcion, false);
                            } else {
                                vm.validarValores(nameArr[0].toString(), p.Descripcion, false);
                            }
                        });
                    }
                    vm.notificacionestado({ nombreComponente: vm.nombreComponente, esValido: isValid });
                }
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
            vm.PreguntasPersonalizadas.PreguntasGenerales.forEach(generales => {
                generales.Preguntas.forEach(preguntas => {
                    var idSpanAlertComponent = document.getElementById("alert-" + vm.nombreComponente + preguntas.IdPregunta);
                    if (idSpanAlertComponent != null) {
                        idSpanAlertComponent.classList.remove("ico-advertencia");
                    }
                });
            });
        }
    }

    angular.module('backbone').component('datosInicialesSgr', {
        templateUrl: "/src/app/formulario/ventanas/SGR/viabilidadSGR/requisitos/datosGenerales/datosIniciales/datosInicialesSgr.html",
        controller: datosInicialesSgrController,
        controllerAs: "vm",
        bindings: {
            callback: '&',
            guardadocomponent: '&',
            notificacionvalidacion: '&',
            notificacionestado: '&',
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