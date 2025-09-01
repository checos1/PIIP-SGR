(function () {
    'use strict';

    crucePoliticasTransversalesSgrController.$inject = ['$scope', '$sessionStorage', 'utilidades', 'focalizacionAjustesSgrServicio', 'justificacionCambiosServicio', '$timeout'
    ];

    function crucePoliticasTransversalesSgrController(
        $scope,
        $sessionStorage,
        utilidades,
        focalizacionAjustesSgrServicio,
        justificacionCambiosServicio,
        $timeout
    ) {
        var listaFuentesBase = [];

        var vm = this;
        vm.init = init;
        vm.nombreComponente = "sgrviabilidadpreviosfocalizacioncrucepoliticas";
        vm.listaPoliticasProyecto = [];
        vm.listaPoliticasCategorias = null;
        vm.BPIN = $sessionStorage.idObjetoNegocio;
        vm.notificacionErrores = null;
        vm.erroresActivos = [];
        vm.arregloGeneralPTResumen = null;
        vm.verTablaResumen = false;
        vm.disabled = false;
        vm.activar = true;
        vm.validacionGuardado = null;
        vm.seccionCapitulo = null;

        vm.CrucePoliticasAjustes = null;
        vm.CrucePoliticasAjustesOrigen = null;
        vm.CantidadFuentes = 0;
        vm.CrucePolitcasVigencia = null;
        vm.CrucePolitcasFirme = null;
        vm.currentYear = new Date().getFullYear();
        vm.ConvertirNumero2decimales = ConvertirNumero2decimales;
        vm.ConvertirNumero = ConvertirNumero;
        vm.PoliticaDependienteId = 0;
        vm.PoliticaContenida = null;
        vm.CrucePoliticasGuardar = [];
        vm.erroresComponente = [];
        vm.componentesRefresh = [
            'sgrviabilidadpreviosfocalizacionpoliticas',
            'sgrviabilidadpreviosfocalizacioncategoriapoliticas'
        ];
        vm.componentesRefreshEliminar = [
            'sgrviabilidadpreviosrecursosfuentessgr',
        ];
        //vm.MensajeError = "";

        function init() {
            //vm.permiteEditar = false;
          

            vm.notificacionvalidacion({ handler: vm.notificacionValidacionPadre, nombreComponente: vm.nombreComponente, esValido: true });
            vm.notificarrefresco({ handler: vm.refrescarResumenCostos, nombreComponente: vm.nombreComponente });
            vm.notificacioncambios({ handler: vm.notificacionCambiosCapitulos, nombreComponente: vm.nombreComponente });
            vm.notificacioninicio({ handlerInicio: vm.notificacionInicioPadre, nombreComponente: vm.nombreComponente, esValido: true });
            
        }
        vm.notificacionInicioPadre = function (nombreModificado, errores) {
            if (nombreModificado == vm.nombreComponente + '-1') {

                if (errores != null && errores != undefined) {
                    vm.erroresComponente = errores;
                }

                vm.model = {
                    modulos: {
                        administracion: false,
                        backbone: true
                    }
                }
                vm.ObtenerCrucePoliticasAjustes($sessionStorage.idInstancia);

                if ($scope.$parent !== undefined && $scope.$parent.$parent !== undefined && $scope.$parent.$parent.vm !== undefined && $scope.$parent.$parent.vm.HabilitarGuardarPaso !== undefined && !$scope.$parent.$parent.vm.HabilitarGuardarPaso) {
                    bloquearControles();
                }
            }
        }
        $scope.$on("BloquearPorCTUSPendiente", function (evt, data) {
            bloquearControles();
        });

        function bloquearControles() {
            vm.disabled = true;
            vm.activar = true;
        }

        vm.ObtenerCrucePoliticasAjustes = function (idInstancia) {
            return focalizacionAjustesSgrServicio.ObtenerCrucePoliticasAjustes(idInstancia, usuarioDNP, $sessionStorage.idNivel).then(
                function (respuesta) {
                    if (respuesta.data != null && respuesta.data != "") {
                        var arreglolistas = jQuery.parseJSON(respuesta.data);
                        vm.CrucePoliticasAjustes = jQuery.parseJSON(arreglolistas);
                        if (vm.CrucePoliticasAjustes.Fuentes != null) {
                            vm.CantidadFuentes = vm.CrucePoliticasAjustes.Fuentes.length;
                        } else {
                            guardarCapituloModificado();
                        }
                        if (vm.erroresComponente != null && vm.erroresComponente != undefined) {
                            $timeout(function () {
                                vm.notificacionValidacionPadre(vm.erroresComponente);
                            }, 600);
                        }
                    }
                });

        }

        function calcularTotales(politicaContenida, tab, fuenteID, indexFuentes, politicaId, indexpp, localizacionId, indexlocalizacion, politicaDependienteId, indexpoliticacontenida) {
            var valorPP = 0;
            var valorPC = 0;
            var personasPP = 0;
            var personasPC = 0;
            var mensajevalidacion = "";
            var mensajefinal = "";
            //vm.MensajeError = "";
            switch (tab) {
                case 1:
                    {
                        for (var i = 0; i < politicaContenida.CrucePoliticasVigencias.length; i++) {
                            valorPP += politicaContenida.CrucePoliticasVigencias[i].ValorPoliticaPrincipal == null ? 0 : parseFloat(politicaContenida.CrucePoliticasVigencias[i].ValorPoliticaPrincipal);
                            valorPC += politicaContenida.CrucePoliticasVigencias[i].ValorCruceDependientePrincipal == null ? 0 : parseFloat(politicaContenida.CrucePoliticasVigencias[i].ValorCruceDependientePrincipal);
                            personasPP += politicaContenida.CrucePoliticasVigencias[i].PersonaPoliticaPrincipal == null ? 0 : parseFloat(politicaContenida.CrucePoliticasVigencias[i].PersonaPoliticaPrincipal);
                            personasPC += politicaContenida.CrucePoliticasVigencias[i].PersonaCruce == null ? 0 : parseFloat(politicaContenida.CrucePoliticasVigencias[i].PersonaCruce);

                            mensajevalidacion = "";

                            if (parseFloat(politicaContenida.CrucePoliticasVigencias[i].ValorPoliticaPrincipal) < parseFloat(politicaContenida.CrucePoliticasVigencias[i].ValorCruceDependientePrincipal)) {
                                mensajevalidacion += " En fila " + [i + 1] + " Columna 3, el valor  " + parseFloat(politicaContenida.CrucePoliticasVigencias[i].ValorCruceDependientePrincipal) + " no puede ser superior a " + parseFloat(politicaContenida.CrucePoliticasVigencias[i].ValorPoliticaPrincipal) + "; ";
                                $("#inputvalorcp" + fuenteID + "-" + indexFuentes + "-" + politicaId + "-" + indexpp + "-" + localizacionId + "-" + indexlocalizacion + "-" + politicaDependienteId + "-" + indexpoliticacontenida + "-" + i).css("border-color", "red");
                            }
                            else {
                                $("#inputvalorcp" + fuenteID + "-" + indexFuentes + "-" + politicaId + "-" + indexpp + "-" + localizacionId + "-" + indexlocalizacion + "-" + politicaDependienteId + "-" + indexpoliticacontenida + "-" + i).css("border-color", "");
                            }
                            if (parseFloat(politicaContenida.CrucePoliticasVigencias[i].PersonaPoliticaPrincipal) < parseFloat(politicaContenida.CrucePoliticasVigencias[i].PersonaCruce)) {
                                mensajevalidacion += " En fila " + [i + 1] + " Columna 5, el valor  " + parseFloat(politicaContenida.CrucePoliticasVigencias[i].PersonaCruce) + " no puede ser superior a " + parseFloat(politicaContenida.CrucePoliticasVigencias[i].PersonaPoliticaPrincipal) + "; ";
                                $("#inputpersonascp" + fuenteID + "-" + indexFuentes + "-" + politicaId + "-" + indexpp + "-" + localizacionId + "-" + indexlocalizacion + "-" + politicaDependienteId + "-" + indexpoliticacontenida + "-" + i).css("border-color", "red");
                            }
                            else {
                                $("#inputpersonascp" + fuenteID + "-" + indexFuentes + "-" + politicaId + "-" + indexpp + "-" + localizacionId + "-" + indexlocalizacion + "-" + politicaDependienteId + "-" + indexpoliticacontenida + "-" + i).css("border-color", "");
                            }

                            if (parseFloat(politicaContenida.CrucePoliticasVigencias[i].ValorCruceDependientePrincipal) > parseFloat(politicaContenida.CrucePoliticasVigencias[i].ValorPoliticaDependiente)) {
                                mensajevalidacion += " El valor registrado en el campo " + politicaContenida.PoliticaDependiente + " $, vigencia " + politicaContenida.CrucePoliticasVigencias[i].Vigencia + "  no puede ser superior al focalizado en la política; ";
                                $("#inputvalorcp" + fuenteID + "-" + indexFuentes + "-" + politicaId + "-" + indexpp + "-" + localizacionId + "-" + indexlocalizacion + "-" + politicaDependienteId + "-" + indexpoliticacontenida + "-" + i).css("border-color", "red");
                            }
                            else {
                                $("#inputvalorcp" + fuenteID + "-" + indexFuentes + "-" + politicaId + "-" + indexpp + "-" + localizacionId + "-" + indexlocalizacion + "-" + politicaDependienteId + "-" + indexpoliticacontenida + "-" + i).css("border-color", "");
                            }

                            if (parseFloat(politicaContenida.CrucePoliticasVigencias[i].PersonaCruce) > parseFloat(politicaContenida.CrucePoliticasVigencias[i].PersonasPoliticaDependiente)) {
                                mensajevalidacion += " El valor registrado en el campo Personas " + politicaContenida.PoliticaDependiente + ", vigencia " + politicaContenida.CrucePoliticasVigencias[i].Vigencia + "  no puede ser superior al focalizado en la política; ";
                                $("#inputpersonascp" + fuenteID + "-" + indexFuentes + "-" + politicaId + "-" + indexpp + "-" + localizacionId + "-" + indexlocalizacion + "-" + politicaDependienteId + "-" + indexpoliticacontenida + "-" + i).css("border-color", "red");
                            }
                            else {
                                $("#inputpersonascp" + fuenteID + "-" + indexFuentes + "-" + politicaId + "-" + indexpp + "-" + localizacionId + "-" + indexlocalizacion + "-" + politicaDependienteId + "-" + indexpoliticacontenida + "-" + i).css("border-color", "");
                            }

                            if (mensajevalidacion != "") {
                                $("#imgcptj" + politicaContenida.CrucePoliticasVigencias[i].Vigencia + fuenteID + indexFuentes + politicaId + indexpp + localizacionId + indexlocalizacion + politicaDependienteId + indexpoliticacontenida).attr('disabled', false);
                                $("#imgcptj" + politicaContenida.CrucePoliticasVigencias[i].Vigencia + fuenteID + indexFuentes + politicaId + indexpp + localizacionId + indexlocalizacion + politicaDependienteId + indexpoliticacontenida).fadeIn();
                                $("#icocptj" + politicaContenida.CrucePoliticasVigencias[i].Vigencia + fuenteID + indexFuentes + politicaId + indexpp + localizacionId + indexlocalizacion + politicaDependienteId + indexpoliticacontenida).attr('disabled', false);
                                $("#icocptj" + politicaContenida.CrucePoliticasVigencias[i].Vigencia + fuenteID + indexFuentes + politicaId + indexpp + localizacionId + indexlocalizacion + politicaDependienteId + indexpoliticacontenida).fadeIn();
                            }
                            else {
                                $("#imgcptj" + politicaContenida.CrucePoliticasVigencias[i].Vigencia + fuenteID + indexFuentes + politicaId + indexpp + localizacionId + indexlocalizacion + politicaDependienteId + indexpoliticacontenida).attr('disabled', true);
                                $("#imgcptj" + politicaContenida.CrucePoliticasVigencias[i].Vigencia + fuenteID + indexFuentes + politicaId + indexpp + localizacionId + indexlocalizacion + politicaDependienteId + indexpoliticacontenida).fadeOut();
                                $("#icocptj" + politicaContenida.CrucePoliticasVigencias[i].Vigencia + fuenteID + indexFuentes + politicaId + indexpp + localizacionId + indexlocalizacion + politicaDependienteId + indexpoliticacontenida).attr('disabled', true);
                                $("#icocptj" + politicaContenida.CrucePoliticasVigencias[i].Vigencia + fuenteID + indexFuentes + politicaId + indexpp + localizacionId + indexlocalizacion + politicaDependienteId + indexpoliticacontenida).fadeOut();
                            }
                            mensajefinal += mensajevalidacion;
                        }

                        if (mensajefinal != "") {
                            $("#errortottal" + fuenteID + "-" + indexFuentes + "-" + politicaId + "-" + indexpp + "-" + localizacionId + "-" + indexlocalizacion + "-" + politicaDependienteId + "-" + indexpoliticacontenida).attr('disabled', false);
                            $("#errortottal" + fuenteID + "-" + indexFuentes + "-" + politicaId + "-" + indexpp + "-" + localizacionId + "-" + indexlocalizacion + "-" + politicaDependienteId + "-" + indexpoliticacontenida).fadeIn();
                            $("#errortottalmsn" + fuenteID + "-" + indexFuentes + "-" + politicaId + "-" + indexpp + "-" + localizacionId + "-" + indexlocalizacion + "-" + politicaDependienteId + "-" + indexpoliticacontenida).attr('disabled', false);
                            $("#errortottalmsn" + fuenteID + "-" + indexFuentes + "-" + politicaId + "-" + indexpp + "-" + localizacionId + "-" + indexlocalizacion + "-" + politicaDependienteId + "-" + indexpoliticacontenida).fadeIn();
                           // $("#Guardar" + fuenteID + indexFuentes + politicaId + indexpp + localizacionId + indexlocalizacion + politicaDependienteId + indexpoliticacontenida).fadeOut();
                        }
                        else {
                            $("#errortottal" + fuenteID + "-" + indexFuentes + "-" + politicaId + "-" + indexpp + "-" + localizacionId + "-" + indexlocalizacion + "-" + politicaDependienteId + "-" + indexpoliticacontenida).attr('disabled', true);
                            $("#errortottal" + fuenteID + "-" + indexFuentes + "-" + politicaId + "-" + indexpp + "-" + localizacionId + "-" + indexlocalizacion + "-" + politicaDependienteId + "-" + indexpoliticacontenida).fadeOut();
                            $("#errortottalmsn" + fuenteID + "-" + indexFuentes + "-" + politicaId + "-" + indexpp + "-" + localizacionId + "-" + indexlocalizacion + "-" + politicaDependienteId + "-" + indexpoliticacontenida).attr('disabled', true);
                            $("#errortottalmsn" + fuenteID + "-" + indexFuentes + "-" + politicaId + "-" + indexpp + "-" + localizacionId + "-" + indexlocalizacion + "-" + politicaDependienteId + "-" + indexpoliticacontenida).fadeOut();
                           // $("#Guardar" + fuenteID + indexFuentes + politicaId + indexpp + localizacionId + indexlocalizacion + politicaDependienteId + indexpoliticacontenida).fadeIn();

                        }
                        var Errormsn = document.getElementById("errortottalmsn" + fuenteID + "-" + indexFuentes + "-" + politicaId + "-" + indexpp + "-" + localizacionId + "-" + indexlocalizacion + "-" + politicaDependienteId + "-" + indexpoliticacontenida);
                        if (Errormsn != undefined) {
                            Errormsn.innerHTML = '<span>' + mensajefinal + "</span>";

                        }
                        break;
                    }
                case 2:
                    {
                        for (var i = 0; i < politicaContenida.CrucePoliticasVigenciasFirme.length; i++) {
                            valorPP += politicaContenida.CrucePoliticasVigenciasFirme[i].ValorPoliticaPrincipal == null ? 0 : parseFloat(politicaContenida.CrucePoliticasVigenciasFirme[i].ValorPoliticaPrincipal);
                            valorPC += politicaContenida.CrucePoliticasVigenciasFirme[i].ValorCruceDependientePrincipal == null ? 0 : parseFloat(politicaContenida.CrucePoliticasVigenciasFirme[i].ValorCruceDependientePrincipal);
                            personasPP += politicaContenida.CrucePoliticasVigenciasFirme[i].PersonaPoliticaPrincipal == null ? 0 : parseFloat(politicaContenida.CrucePoliticasVigenciasFirme[i].PersonaPoliticaPrincipal);
                            personasPC += politicaContenida.CrucePoliticasVigenciasFirme[i].PersonaCruce == null ? 0 : parseFloat(politicaContenida.CrucePoliticasVigenciasFirme[i].PersonaCruce);
                        }
                        break;
                    }
            }

            politicaContenida.valorPP = valorPP;
            politicaContenida.valorPC = valorPC;
            politicaContenida.personasPP = personasPP;
            politicaContenida.personasPC = personasPC;
        }

        vm.actualizaFila = function (politicaContenida, tab, fuenteID, indexFuentes, politicaId, indexpp, localizacionId, indexlocalizacion, politicaDependienteId, indexpoliticacontenida) {
            calcularTotales(politicaContenida, tab, fuenteID, indexFuentes, politicaId, indexpp, localizacionId, indexlocalizacion, politicaDependienteId, indexpoliticacontenida);
        }

        function ConvertirNumero(numero) {
            return new Intl.NumberFormat('es-co', {
                minimumFractionDigits: 0,
            }).format(numero);
        }

        function ConvertirNumero2decimales(numero) {
            return new Intl.NumberFormat('es-co', {
                minimumFractionDigits: 2,
            }).format(numero);
        }

        vm.validateFormat = function (event) {

            if ((event.keyCode < 48 || event.keyCode > 57) && event.keyCode != 44) {
                event.preventDefault();
            }
        }

        vm.validarTamanio = function (event, cantidadDecimales) {

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

        vm.volver = function () {
            $(window).scrollTop($('#fuentes').position().top);
        }

        vm.TienePoliticas = function (itemFuentes) {
            let tiene = itemFuentes.PoliticaPrincipal != null && itemFuentes.PoliticaPrincipal.length > 0;
            return tiene;
        }

        vm.AbrilNivel1 = function (fuenteId, indexFuentes) {
            var variable = $("#ico-" + fuenteId + "-" + indexFuentes)[0].innerText;
            variable = variable.replace(/ /g, "");
            var imgmas = document.getElementById("imgmas-" + fuenteId + "-" + indexFuentes);
            var imgmenos = document.getElementById("imgmenos-" + fuenteId + "-" + indexFuentes);
            if (variable === "+") {
                $("#ico-" + fuenteId + "-" + indexFuentes).html('-');
                imgmas.style.display = 'none';
                imgmenos.style.display = 'block';

            } else {
                $("#ico-" + fuenteId + "-" + indexFuentes).html('+');
                imgmas.style.display = 'block';
                imgmenos.style.display = 'none';
            }
        }

        vm.AbrilNivel2 = function (fuenteId, indexFuentes, politicaid, indexpolitica) {
            var variable = $("#ico-" + fuenteId + "-" + indexFuentes + "-" + politicaid + "-" + indexpolitica)[0].innerText;
            variable = variable.replace(/ /g, "");
            var imgmas = document.getElementById("imgmas-" + fuenteId + "-" + indexFuentes + "-" + politicaid + "-" + indexpolitica);
            var imgmenos = document.getElementById("imgmenos-" + fuenteId + "-" + indexFuentes + "-" + politicaid + "-" + indexpolitica);
            if (variable === "+") {
                $("#ico-" + fuenteId + "-" + indexFuentes + "-" + politicaid + "-" + indexpolitica).html('-');
                imgmas.style.display = 'none';
                imgmenos.style.display = 'block';

            } else {
                $("#ico-" + fuenteId + "-" + indexFuentes + "-" + politicaid + "-" + indexpolitica).html('+');
                imgmas.style.display = 'block';
                imgmenos.style.display = 'none';
            }
        }

        vm.AbrilNivel3 = function (fuenteId, indexFuentes, politicaid, indexpolitica, localizacionid, indexlocalizacion) {
            var variable = $("#ico-" + fuenteId + "-" + indexFuentes + "-" + politicaid + "-" + indexpolitica + "-" + localizacionid + "-" + indexlocalizacion)[0].innerText;
            variable = variable.replace(/ /g, "");
            var imgmas = document.getElementById("imgmas-" + fuenteId + "-" + indexFuentes + "-" + politicaid + "-" + indexpolitica + "-" + localizacionid + "-" + indexlocalizacion);
            var imgmenos = document.getElementById("imgmenos-" + fuenteId + "-" + indexFuentes + "-" + politicaid + "-" + indexpolitica + "-" + localizacionid + "-" + indexlocalizacion);
            if (variable === "+") {
                $("#ico-" + fuenteId + "-" + indexFuentes + "-" + politicaid + "-" + indexpolitica + "-" + localizacionid + "-" + indexlocalizacion).html('-');
                imgmas.style.display = 'none';
                imgmenos.style.display = 'block';

            } else {
                $("#ico-" + fuenteId + "-" + indexFuentes + "-" + politicaid + "-" + indexpolitica + "-" + localizacionid + "-" + indexlocalizacion).html('+');
                imgmas.style.display = 'block';
                imgmenos.style.display = 'none';
            }
        }

        vm.AbrilNivel4 = function (fuenteId, indexFuentes, politicaid, indexpolitica, localizacionid, indexlocalizacion, politicaDependienteId, indexpoliticaDependiente) {

            var grilla = document.getElementById("sec-" + fuenteId + "-" + indexFuentes + "-" + politicaid + "-" + indexpolitica + "-" + localizacionid + "-" + indexlocalizacion + "-" + politicaDependienteId + "-" + indexpoliticaDependiente);
            if (grilla != undefined) {
                grilla.classList.remove('hidden');
            }

            vm.mostrarTab(1, fuenteId, indexFuentes, politicaid, indexpolitica, localizacionid, indexlocalizacion, politicaDependienteId, indexpoliticaDependiente);


            var variable = $("#ico-" + fuenteId + "-" + indexFuentes + "-" + politicaid + "-" + indexpolitica + "-" + localizacionid + "-" + indexlocalizacion + "-" + politicaDependienteId + "-" + indexpoliticaDependiente)[0].innerText;
            variable = variable.replace(/ /g, "");
            var imgmas = document.getElementById("imgmas-" + fuenteId + "-" + indexFuentes + "-" + politicaid + "-" + indexpolitica + "-" + localizacionid + "-" + indexlocalizacion + "-" + politicaDependienteId + "-" + indexpoliticaDependiente);
            var imgmenos = document.getElementById("imgmenos-" + fuenteId + "-" + indexFuentes + "-" + politicaid + "-" + indexpolitica + "-" + localizacionid + "-" + indexlocalizacion + "-" + politicaDependienteId + "-" + indexpoliticaDependiente);
            if (variable === "+") {
                $("#ico-" + fuenteId + "-" + indexFuentes + "-" + politicaid + "-" + indexpolitica + "-" + localizacionid + "-" + indexlocalizacion + "-" + politicaDependienteId + "-" + indexpoliticaDependiente).html('-');
                imgmas.style.display = 'none';
                imgmenos.style.display = 'block';

            } else {
                $("#ico-" + fuenteId + "-" + indexFuentes + "-" + politicaid + "-" + indexpolitica + "-" + localizacionid + "-" + indexlocalizacion + "-" + politicaDependienteId + "-" + indexpoliticaDependiente).html('+');
                imgmas.style.display = 'block';
                imgmenos.style.display = 'none';
            }
        }

        vm.mostrarTab = function (origen, fuenteid, indexFuentes, politicaid, indexpp, localizacionid, indexlocalizacion, politicaDependienteId, indexpoliticacontenida) {

            var politicaContenida = null;
            vm.CrucePoliticasAjustes.Fuentes.forEach(fuentes => {
                if (fuentes.FuenteID == fuenteid) {
                    fuentes.PoliticaPrincipal.forEach(pp => {
                        if (pp.PoliticaId == politicaid) {
                            pp.Localizaciones.forEach(localizaciones => {
                                if (localizaciones.LocalizacionId == localizacionid) {
                                    localizaciones.RelacionPoliticas.forEach(pr => {
                                        if (pr.PoliticaDependienteId == politicaDependienteId) {
                                            politicaContenida = pr;
                                        }
                                    });
                                }
                            });
                        }
                    });
                }
            });

            switch (origen) {
                case 1:
                    {
                        if (!vm.disabled) {
                            $("#Editar" + fuenteid + '-' + indexFuentes + '-' + politicaid + '-' + indexpp + '-' + localizacionid + '-' + indexlocalizacion + '-' + politicaDependienteId + '-' + indexpoliticacontenida).attr('disabled', false);
                            $("#Guardar" + fuenteid + '-' + indexFuentes + '-' + politicaid + '-' + indexpp + '-' + localizacionid + '-' + indexlocalizacion + '-' + politicaDependienteId + '-' + indexpoliticacontenida).fadeIn();
                        }
                        break;
                    }
                case 2:
                    {
                        $("#Editar" + fuenteid + '-' + indexFuentes + '-' + politicaid + '-' + indexpp + '-' + localizacionid + '-' + indexlocalizacion + '-' + politicaDependienteId + '-' + indexpoliticacontenida).attr('disabled', true);
                        $("#Guardar" + fuenteid + '-' + indexFuentes + '-' + politicaid + '-' + indexpp + '-' + localizacionid + '-' + indexlocalizacion + '-' + politicaDependienteId + '-' + indexpoliticacontenida).fadeOut();

                        break;
                    }
            }
            calcularTotales(politicaContenida, origen, fuenteid, indexFuentes, politicaid, indexpp, localizacionid, indexlocalizacion, politicaDependienteId, indexpoliticacontenida);

            vm.CrucePoliticasAjustes.Fuentes.forEach(fuentes => {
                if (fuentes.FuenteID == fuenteid) {
                    fuentes.PoliticaPrincipal.forEach(pp => {
                        if (pp.PoliticaId == politicaid) {
                            pp.Localizaciones.forEach(localizaciones => {
                                if (localizaciones.LocalizacionId == localizacionid) {
                                    localizaciones.RelacionPoliticas.forEach(pr => {
                                        if (pr.PoliticaDependienteId == politicaDependienteId) {
                                            pr.valorPP = politicaContenida.valorPP;
                                            pr.valorPC = politicaContenida.valorPC;
                                            pr.personasPP = politicaContenida.personasPP;
                                            pr.personasPC = politicaContenida.personasPC;
                                        }
                                    });
                                }
                            });
                        }
                    });
                }
            });

            var valor = 0;
        }

        vm.habilitarEditar = function (fuenteid, indexFuentes, politicaId, indexpp, localizacionId, indexlocalizacion, politicaContenida, politicaDependienteId, indexpoliticacontenida) {
            politicaContenida.HabilitaEditarLocalizador = true;
            $("#Guardar" + fuenteid + indexFuentes + politicaId + indexpp + localizacionId + indexlocalizacion + politicaDependienteId + indexpoliticacontenida).attr('disabled', false);
            vm.CrucePoliticasAjustesOrigen = JSON.stringify(vm.CrucePoliticasAjustes);
        }

        vm.cancelarEdicion = function (fuenteid, indexFuentes, politicaId, indexpp, localizacionId, indexlocalizacion, politicaContenida, politicaDependienteId, indexpoliticacontenida) {
            utilidades.mensajeWarning("¿Está seguro de continuar?", function funcionContinuar() {
                politicaContenida.HabilitaEditarLocalizador = false;
                $("#Guardar" + fuenteid + indexFuentes + politicaId + indexpp + localizacionId + indexlocalizacion + politicaDependienteId + indexpoliticacontenida).attr('disabled', true);
                asignarValoresOriginales(fuenteid, indexFuentes, politicaId, indexpp, localizacionId, indexlocalizacion, politicaContenida, politicaDependienteId, indexpoliticacontenida);
                new utilidades.mensajeSuccess("", false, false, false, "Se ha cancelado la edición.");
                return focalizacionAjustesSgrServicio.ObtenerCrucePoliticasAjustes(vm.BPIN, usuarioDNP, $sessionStorage.idNivel).then(
                    function (respuesta) {
                        utilidades.mensajeSuccess("", false, false, false, "Se ha cancelado la edición.")
                    });

            }, function funcionCancelar(reason) {
                //console.log("reason", reason);
            }, 'Aceptar', 'Cancelar', 'Los posibles datos que haya diligenciado en la tabla se perderán.');
        }

        function asignarValoresOriginales(fuenteid, indexFuentes, politicaId, indexpp, localizacionId, indexlocalizacion, politicaContenida, politicaDependienteId, indexpoliticacontenida) {

            var politicaContenidaorigen = null;
            vm.CrucePoliticasAjustes = JSON.parse(vm.CrucePoliticasAjustesOrigen);
            politicaContenida.HabilitaEditarLocalizador = false;

            vm.CrucePoliticasAjustes.Fuentes.forEach(fuentesorigen => {
                if (fuentesorigen.FuenteID == fuenteid) {
                    fuentesorigen.PoliticaPrincipal.forEach(pporigen => {
                        if (pporigen.PoliticaId == politicaId) {
                            pporigen.Localizaciones.forEach(localizacionesorigen => {
                                if (localizacionesorigen.LocalizacionId == localizacionId) {
                                    localizacionesorigen.RelacionPoliticas.forEach(prorigen => {
                                        if (prorigen.PoliticaDependienteId == politicaDependienteId) {
                                            politicaContenidaorigen = prorigen;
                                        }
                                    });
                                }
                            });
                        }
                    });
                }
            });
            politicaContenidaorigen.HabilitaEditarLocalizador = false;

            calcularTotales(politicaContenidaorigen, 1, fuenteid, indexFuentes, politicaId, indexpp, localizacionId, indexlocalizacion, politicaDependienteId, indexpoliticacontenida);
        }

        vm.GuardarAjustes = function (FuenteID, indexFuentes, PoliticaId, indexpp, LocalizacionId, indexlocalizacion, politicaContenida, PoliticaDependienteId, CrucePoliticasVigencias, indexpoliticacontenida) {

            vm.CrucePoliticasGuardar = [];

            CrucePoliticasVigencias.forEach(cpv => {
                var valorescpv = {
                    ProyectoId: vm.CrucePoliticasAjustes.ProyectoId,
                    Bpin: vm.CrucePoliticasAjustes.BPIN,
                    FuenteId: FuenteID,
                    PoliticaId: PoliticaId,
                    LocalizacionId: LocalizacionId,
                    PoliticaDependienteId: PoliticaDependienteId,
                    PeriodoProyectoId: cpv.PeriodoProyectoId,
                    Vigencia: cpv.Vigencia,
                    ValorPoliticaPrincipal: cpv.ValorPoliticaPrincipal,
                    ValorCruceDependientePrincipal: cpv.ValorCruceDependientePrincipal,
                    PersonaPoliticaPrincipal: cpv.PersonaPoliticaPrincipal,
                    PersonaCruce: cpv.PersonaCruce

                };
                vm.CrucePoliticasGuardar.push(valorescpv);
            });

            return focalizacionAjustesSgrServicio.GuardarCrucePoliticasAjustes(vm.CrucePoliticasGuardar, vm.idUsuario).then(function (response) {
                if (response.statusText == "OK") {
                    guardarCapituloModificado();
                    politicaContenida.HabilitaEditarLocalizador = false;
                    $("#Guardar" + FuenteID + indexFuentes + PoliticaId + indexpp + LocalizacionId + indexlocalizacion + PoliticaDependienteId + indexpoliticacontenida).attr('disabled', true);
                    vm.CrucePoliticasGuardar = [];
                    utilidades.mensajeSuccess('', false, false, false, "Los datos fueron guardados con éxito.");
                }
                else {
                    //var ValidacionGuardar = document.getElementById(vm.nombreComponente + "-validacionguardar-error");
                    //if (ValidacionGuardar != undefined) {
                    //    var ValidacionFFR1Error = document.getElementById(vm.nombreComponente + "-validacionguardar-error-mns");
                    //    if (ValidacionFFR1Error != undefined) {
                    //        ValidacionFFR1Error.innerHTML = '<span>' + errores + "</span>";
                    //        ValidacionFFR1.classList.remove('hidden');
                    //    }
                    //}
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

        vm.obtenerPoliticasTransversalesResumen = function (bpin) {

            var idInstancia = $sessionStorage.idNivel;
            var idAccion = $sessionStorage.idNivel;
            var idFormulario = $sessionStorage.idNivel;
            var idUsuario = $sessionStorage.usuario.permisos.IdUsuarioDNP;

            return focalizacionAjustesSgrServicio.ObtenerPoliticasTransversalesResumen(idInstancia, usuarioDNP, idInstancia).then(
                function (respuesta) {
                    if (respuesta.data != null && respuesta.data != "") {
                        var arreglolistas = jQuery.parseJSON(respuesta.data);

                        vm.arregloGeneralPTResumen = jQuery.parseJSON(arreglolistas);

                        if (vm.arregloGeneralPTResumen.Politicas.length > 1) {
                            vm.verTablaResumen = true;
                        }
                    }
                });
        }

        function ObtenerSeccionCapitulo() {
            const span = document.getElementById('id-capitulo-' + vm.nombreComponente);
            vm.seccionCapitulo = span.textContent;
        }

        vm.notificacionCambiosCapitulos = function (nombreCapituloHijo) {
            if (vm.componentesRefresh.includes(nombreCapituloHijo)) {
                vm.ObtenerCrucePoliticasAjustes($sessionStorage.idInstancia);
            }
            if (vm.componentesRefreshEliminar.includes(nombreCapituloHijo)) {
                eliminarCapitulosModificados();
            }
        }

        vm.notificacionValidacionHijo = function (handler) {
            vm.notificacionErrores = handler;
        }

        vm.validacionGuardadoHijo = function (handler) {
            vm.validacionGuardado = handler;
        }



        /* ------------------------ Validaciones ---------------------------------*/

        vm.notificacionValidacionPadre = function (errores) {
            console.log("Validación  - Cruce de Políticas transversales");
            vm.limpiarErrores();
            if (errores != undefined) {
                var erroresRelacionconlapl = errores.find(p => (p.Seccion + p.Capitulo) == vm.nombreComponente);

                var isValid = true;

                if (erroresRelacionconlapl != undefined) {
                    var erroresJson = erroresRelacionconlapl.Errores == "" ? [] : JSON.parse(erroresRelacionconlapl.Errores);
                    if (vm.notificacionErrores != null && erroresJson != null) vm.notificacionErrores(erroresJson[vm.nombreComponente]);

                    isValid = (erroresJson == null || erroresJson.length == 0);
                    if (!isValid) {
                        erroresJson[vm.nombreComponente].forEach(p => {
                            if (vm.errores[p.Error] != undefined) vm.errores[p.Error](p.Descripcion);
                        });
                    }
                }
                vm.notificacionestado({ nombreComponente: vm.nombreComponente, esValido: isValid });

            }
        };

        vm.limpiarErrores = function () {

            var validacionffr1 = document.getElementById(vm.nombreComponente + "-validacionffr1-error");
            var ValidacionFFR1Error = document.getElementById(vm.nombreComponente + "-validacionffr1-error-mns");
            if (validacionffr1 != undefined) {
                if (ValidacionFFR1Error != undefined) {
                    ValidacionFFR1Error.innerHTML = "";
                    validacionffr1.classList.add('hidden');
                }
            }
            var validacionffr2 = document.getElementById(vm.nombreComponente + "-validacionffr2-error");
            var ValidacionFFR2Error = document.getElementById(vm.nombreComponente + "-validacionffr2-error-mns");
            if (validacionffr2 != undefined) {
                if (ValidacionFFR2Error != undefined) {
                    ValidacionFFR2Error.innerHTML = "";
                    validacionffr2.classList.add('hidden');
                }
            }
            var validacionffr3 = document.getElementById(vm.nombreComponente + "-validacionffr3-error");
            var ValidacionFFR3Error = document.getElementById(vm.nombreComponente + "-validacionffr3-error-mns");
            if (validacionffr3 != undefined) {
                if (ValidacionFFR3Error != undefined) {
                    ValidacionFFR3Error.innerHTML = "";
                    validacionffr3.classList.add('hidden');
                }
            }

            var drecursosA = document.getElementById(vm.nombreComponente + "-drecursosA");
            if (drecursosA != undefined) {
                drecursosA.innerHTML = "";
                drecursosA.classList.add('hidden');
            }
        }

        vm.validarAFFR001 = function (errores) { }

        vm.validarAFFR002 = function (errores) { }

        vm.validarAFFR003 = function (errores) { }

        vm.errores = {
            //'FUE001': vm.validarExitenciaFuentes;
            'AFFR001': vm.validarAFFR001,
            'AFFR002': vm.validarAFFR002,
            'AFFR003': vm.validarAFFR003,
        }

        //para guardar los capitulos modificados y que se llenen las lunas
        function ObtenerSeccionCapitulo() {
            const span = document.getElementById('id-capitulo-' + vm.nombreComponente);
            vm.seccionCapitulo = span.textContent;
        }

        //para guardar los capitulos modificados y que se llenen las lunas
        function guardarCapituloModificado() {
            ObtenerSeccionCapitulo();

            var data = {
                ProyectoId: $sessionStorage.InstanciaSeleccionada.ProyectoId,
                Justificacion: "",
                SeccionCapituloId: vm.seccionCapitulo,
                InstanciaId: $sessionStorage.idInstancia,
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
    }

    angular.module('backbone').component('crucePoliticasTransversalesSgr', {
        templateUrl: "/src/app/formulario/ventanas/SGR/viabilidadSGR/previos/focalizacion/CrucePoliticasTransversales/crucePoliticasTransversalesSgr.html",
        controller: crucePoliticasTransversalesSgrController,
        controllerAs: "vm",
        bindings: {
            callback: '&',
            notificacionvalidacion: '&',
            notificacionestado: '&',
            guardadoevent: '&',
            notificarrefresco: '&',
            notificacioncambios: '&',
            notificacioninicio: '&'
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
        .directive('nksOnlyNumber', function () {
            return {
                restrict: 'EA',
                require: 'ngModel',
                link: function (scope, element, attrs, ngModel) {
                    scope.$watch(attrs.ngModel, function (newValue, oldValue) {
                        var spiltArray = String(newValue).split("");

                        if (attrs.allowNegative == "false") {
                            if (spiltArray[0] == '-') {
                                newValue = newValue.replace("-", "");
                                ngModel.$setViewValue(newValue);
                                ngModel.$render();
                            }
                        }

                        if (attrs.allowDecimal == "false") {
                            newValue = parseInt(newValue);
                            ngModel.$setViewValue(newValue);
                            ngModel.$render();
                        }

                        if (attrs.allowDecimal != "false") {
                            if (attrs.decimalUpto) {
                                var n = String(newValue).split(".");
                                if (n[1]) {
                                    var n2 = n[1].slice(0, attrs.decimalUpto);
                                    newValue = [n[0], n2].join(".");
                                    ngModel.$setViewValue(newValue);
                                    ngModel.$render();
                                }
                            }
                        }


                        if (spiltArray.length === 0) return;
                        if (spiltArray.length === 1 && (spiltArray[0] == '-' || spiltArray[0] === '.')) return;
                        if (spiltArray.length === 2 && newValue === '-.') return;

                        /*Check it is number or not.*/
                        if (isNaN(newValue)) {
                            ngModel.$setViewValue(oldValue || '');
                            ngModel.$render();
                        }
                    });
                }
            };
        });
})();