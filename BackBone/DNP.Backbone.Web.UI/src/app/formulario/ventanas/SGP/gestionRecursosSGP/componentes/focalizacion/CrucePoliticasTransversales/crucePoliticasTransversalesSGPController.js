(function () {
    'use strict';

    crucePoliticasTransversalesSgpController.$inject = ['$scope', '$sessionStorage', '$uibModal', 'utilidades',
        'constantesBackbone', '$timeout', 'focalizacionAjustesSGPServicio', 'transversalSgpServicio'
    ];

    function crucePoliticasTransversalesSgpController(
        $scope,
        $sessionStorage,
        $uibModal,
        utilidades,
        constantesBackbone,
        $timeout,
        focalizacionAjustesSGPServicio,
        transversalSgpServicio
    ) {
        var listaFuentesBase = [];

        var vm = this;
        vm.init = init;
        vm.nombreComponente = "sgpsolicitudrecursosfocalizacioncrucepoliticassgp";
        vm.listaPoliticasProyecto = [];
        vm.listaPoliticasCategorias = null;
        vm.BPIN = $sessionStorage.idObjetoNegocio;


        vm.notificacionErrores = null;
        vm.erroresActivos = [];
        vm.arregloGeneralPTResumen = null;
        vm.verTablaResumen = false;


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
        vm.ValoresPoliticaVictimas = [];
        vm.componentesRefresh = [
            'sgpsolicitudrecursosfocalizacionpoliticassgp',
            'sgpsolicitudrecursosfocalizacioncategoriapoliticassgp',
            'sgprecursosfuentesfinanciacionsgp'
        ];

        vm.ObtenerhabilitadorNAPP = ObtenerhabilitadorNAPP;
        vm.ObtenerhabilitadorNAPD = ObtenerhabilitadorNAPD;
        vm.habilitadorNA = 0;
        vm.soloLectura = false;
        const numberFormatter = new Intl.NumberFormat('es-co', {
            minimumFractionDigits: 2,
        });

        vm.sumPersonasGruposEtnicos = new Map();

        function init() {
            vm.model = {
                modulos: {
                    administracion: false,
                    backbone: true
                }
            }
            vm.ObtenerCrucePoliticasAjustes(vm.BPIN);
            vm.notificacionvalidacion({ handler: vm.notificacionValidacionPadre, nombreComponente: vm.nombreComponente, esValido: true });
            vm.notificarrefresco({ handler: vm.refrescarResumenCostos, nombreComponente: vm.nombreComponente });
            vm.notificacioncambios({ handler: vm.notificacionCambiosCapitulos, nombreComponente: vm.nombreComponente });
            localStorage.setItem('ErroresAJUFOC', '');
            transversalSgpServicio.registrarObservador(function (datos) {
                habilitaCapitulo(datos.enableRegionalizacion);
            });
        }

        vm.ObtenerCrucePoliticasAjustes = function (bpin) {

            return focalizacionAjustesSGPServicio.ObtenerCrucePoliticasAjustesSGP($sessionStorage.idInstancia, usuarioDNP, $sessionStorage.idNivel).then(
                function (respuesta) {
                    if (respuesta.data != null && respuesta.data != "") {
                        var arreglolistas = jQuery.parseJSON(respuesta.data);
                        vm.CrucePoliticasAjustes = jQuery.parseJSON(arreglolistas);

                        vm.CantidadFuentes = vm.CrucePoliticasAjustes.Fuentes.length;

                        vm.ValoresPoliticaVictimas = []

                        vm.CrucePoliticasAjustes.Fuentes.forEach(f => {
                            f.PoliticaPrincipal.forEach(pp => {
                                if (pp.PoliticaId == 20) {
                                    pp.Localizaciones.forEach(loc => {
                                        loc.RelacionPoliticas.forEach(rp => {
                                            if (rp.PoliticaDependienteId == 9 || rp.PoliticaDependienteId == 10 || rp.PoliticaDependienteId == 11 || rp.PoliticaDependienteId == 12
                                                || rp.PoliticaDependienteId == 13 || rp.PoliticaDependienteId == 14) {
                                                rp.CrucePoliticasVigencias.forEach(cpv => {
                                                    var valorrrr = 0;
                                                    var valoresVictimas = {
                                                        LocalizacionId: loc.LocalizacionId,
                                                        Vigencia: cpv.Vigencia,
                                                        Valor: cpv.ValorCruceDependientePrincipal,
                                                        Personas: cpv.PersonaCruce,
                                                        Politica: rp.PoliticaDependiente,
                                                    };

                                                    vm.ValoresPoliticaVictimas.push(valoresVictimas);
                                                });
                                            }
                                        });
                                    });
                                }
                            });
                        });
                        var valor = 0;
                        vm.soloLectura = $sessionStorage.soloLectura;
                    }
                });
        }

        function habilitaCapitulo(activar) {
            var seccionNoHabilitadaFocalizacionCruceCategorias = document.getElementById('seccionNoHabilitadaFocalizacionCruceCategorias');
            var seccionHabilitadaFocalizacionCruceCategorias = document.getElementById('seccionHabilitadaFocalizacionCruceCategorias');

            if (!activar) {
                if (seccionNoHabilitadaFocalizacionCruceCategorias != undefined) {
                    seccionNoHabilitadaFocalizacionCruceCategorias.classList.remove('hidden');
                    vm.disabled = true;
                }

                if (seccionHabilitadaFocalizacionCruceCategorias != undefined) {
                    seccionHabilitadaFocalizacionCruceCategorias.classList.add('hidden');
                }
            } else {
                if (seccionHabilitadaFocalizacionCruceCategorias != undefined) {
                    seccionHabilitadaFocalizacionCruceCategorias.classList.remove('hidden');
                    vm.disabled = false;
                }

                if (seccionNoHabilitadaFocalizacionCruceCategorias != undefined) {
                    seccionNoHabilitadaFocalizacionCruceCategorias.classList.add('hidden');
                }
            }
        }

        function calcularTotales(politicaContenida, tab, fuenteID, indexFuentes, politicaId, indexpp, localizacionId, indexlocalizacion, politicaDependienteId, indexpoliticacontenida, event, cantidadDecimales) {
            var valorPP = 0;
            var valorPC = 0;
            var personasPP = 0;
            var personasPC = 0;
            var mensajevalidacion = "";
            var mensajefinal = "";            

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

                            if (politicaId == 20) {

                                if (politicaDependienteId == 9 || politicaDependienteId == 10 || politicaDependienteId == 11 || politicaDependienteId == 12 || politicaDependienteId == 13 || politicaDependienteId == 14) {

                                    let fuenteIDFilter = [fuenteID];
                                    let localizacionIDFilter = [localizacionId];
                                    let personasValidaGE = 0;
                                    let validPoliticaDependienteIds = [9, 10, 11, 12, 13, 14]; //politicas grupos étnicos
                                    let fuenteIDSet = new Set(fuenteIDFilter);
                                    let localizacionIDSet = new Set(localizacionIDFilter);
                                    vm.sumPersonasGruposEtnicos = new Map();

                                    vm.CrucePoliticasAjustes.Fuentes.forEach(fuente => {
                                        // Aplicar filtro por FuenteID
                                        if (fuenteIDFilter.length > 0 && !fuenteIDSet.has(fuente.FuenteID)) {
                                            return;
                                        }
                                        fuente.PoliticaPrincipal.forEach(principal => {
                                            if (principal.PoliticaId === 20) {
                                                principal.Localizaciones.forEach(localizacion => {
                                                    // Aplicar filtro por LocalizacionId
                                                    if (localizacionIDFilter.length > 0 && !localizacionIDSet.has(localizacion.LocalizacionId)) {
                                                        return;
                                                    }
                                                    localizacion.RelacionPoliticas.forEach(relacion => {
                                                        if (relacion.CategoriaDependiente === politicaContenida.CategoriaDependiente && relacion.SubCategoriaDependiente === politicaContenida.SubCategoriaDependiente) {
                                                            if (validPoliticaDependienteIds.includes(relacion.PoliticaDependienteId)) {
                                                                relacion.CrucePoliticasVigencias.forEach(vigencia => {
                                                                    // Crear clave compuesta para el map
                                                                    const key = `${fuente.FuenteID}_${localizacion.LocalizacionId}_${vigencia.Vigencia}`;

                                                                    // Actualizar la suma en el map
                                                                    if (!vm.sumPersonasGruposEtnicos.has(key)) {
                                                                        vm.sumPersonasGruposEtnicos.set(key, {
                                                                            FuenteID: fuente.FuenteID,
                                                                            LocalizacionId: localizacion.LocalizacionId,
                                                                            Vigencia: vigencia.Vigencia,
                                                                            PersonaCruce: 0
                                                                        });
                                                                    }

                                                                    const existing = vm.sumPersonasGruposEtnicos.get(key);
                                                                    existing.PersonaCruce += Number(vigencia.PersonaCruce);
                                                                    vm.sumPersonasGruposEtnicos.set(key, existing);
                                                                });
                                                            }
                                                        }
                                                    });
                                                });
                                            }
                                        });
                                    });

                                    const arrPersonasGE = Array.from(vm.sumPersonasGruposEtnicos.values());

                                    let personasCruceGE = arrPersonasGE.filter(item =>
                                        item.FuenteID === fuenteID &&
                                        item.LocalizacionId === localizacionId &&
                                        item.Vigencia === politicaContenida.CrucePoliticasVigencias[i].Vigencia
                                    );

                                    if (personasCruceGE) {
                                        if (personasCruceGE.length > 0) {
                                            personasValidaGE = personasCruceGE[0].PersonaCruce;
                                        }
                                    }


                                    if (parseFloat(politicaContenida.CrucePoliticasVigencias[i].PersonaPoliticaPrincipal) < parseFloat(personasValidaGE)) {
                                        mensajevalidacion += " En fila " + [i + 1] + " Columna 5, el valor de la sumatoria de las personas registradas para los grupos étnicos: " + personasValidaGE + " supera el valor registrado en la categoría de la política Víctimas para la vigencia " + politicaContenida.CrucePoliticasVigencias[i].Vigencia + "; ";
                                        //mensajevalidacion += " En fila " + [i + 1] + " Columna 5, el valor " + parseFloat(politicaContenida.CrucePoliticasVigencias[i].PersonaCruce) + " no puede ser superior a " + parseFloat(politicaContenida.CrucePoliticasVigencias[i].PersonaPoliticaPrincipal) + "; ";
                                        $("#inputpersonascp" + fuenteID + "-" + indexFuentes + "-" + politicaId + "-" + indexpp + "-" + localizacionId + "-" + indexlocalizacion + "-" + politicaDependienteId + "-" + indexpoliticacontenida + "-" + i).css("border-color", "red");

                                    }
                                    else {
                                        $("#inputpersonascp" + fuenteID + "-" + indexFuentes + "-" + politicaId + "-" + indexpp + "-" + localizacionId + "-" + indexlocalizacion + "-" + politicaDependienteId + "-" + indexpoliticacontenida + "-" + i).css("border-color", "");
                                    }

                                    //if ((parseFloat(Valor) + parseFloat(politicaContenida.CrucePoliticasVigencias[i].ValorCruceDependientePrincipal)) > parseFloat(politicaContenida.CrucePoliticasVigencias[i].ValorPoliticaPrincipal)) {
                                    //    mensajevalidacion += " La sumatoria de los recursos registrados para los grupos étnicos: " + PoliticasValor + " supera el valor registrado en la categoría de la política Víctimas para la vigencia " + politicaContenida.CrucePoliticasVigencias[i].Vigencia + "; ";
                                    //    $("#inputvalorcp" + fuenteID + "-" + indexFuentes + "-" + politicaId + "-" + indexpp + "-" + localizacionId + "-" + indexlocalizacion + "-" + politicaDependienteId + "-" + indexpoliticacontenida + "-" + i).css("border-color", "red");
                                    //}
                                }
                            }
                            else {
                                if (parseFloat(politicaContenida.CrucePoliticasVigencias[i].PersonaPoliticaPrincipal) < parseFloat(politicaContenida.CrucePoliticasVigencias[i].PersonaCruce)) {
                                    mensajevalidacion += " En fila " + [i + 1] + " Columna 5, el valor  " + parseFloat(politicaContenida.CrucePoliticasVigencias[i].PersonaCruce) + " no puede ser superior a " + parseFloat(politicaContenida.CrucePoliticasVigencias[i].PersonaPoliticaPrincipal) + "; ";
                                    $("#inputpersonascp" + fuenteID + "-" + indexFuentes + "-" + politicaId + "-" + indexpp + "-" + localizacionId + "-" + indexlocalizacion + "-" + politicaDependienteId + "-" + indexpoliticacontenida + "-" + i).css("border-color", "red");

                                }
                                else {
                                    $("#inputpersonascp" + fuenteID + "-" + indexFuentes + "-" + politicaId + "-" + indexpp + "-" + localizacionId + "-" + indexlocalizacion + "-" + politicaDependienteId + "-" + indexpoliticacontenida + "-" + i).css("border-color", "");
                                }
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
                            $("#Guardar" + fuenteID + indexFuentes + politicaId + indexpp + localizacionId + indexlocalizacion + politicaDependienteId + indexpoliticacontenida).attr('disabled', true);
                        }
                        else {
                            $("#errortottal" + fuenteID + "-" + indexFuentes + "-" + politicaId + "-" + indexpp + "-" + localizacionId + "-" + indexlocalizacion + "-" + politicaDependienteId + "-" + indexpoliticacontenida).attr('disabled', true);
                            $("#errortottal" + fuenteID + "-" + indexFuentes + "-" + politicaId + "-" + indexpp + "-" + localizacionId + "-" + indexlocalizacion + "-" + politicaDependienteId + "-" + indexpoliticacontenida).fadeOut();
                            $("#errortottalmsn" + fuenteID + "-" + indexFuentes + "-" + politicaId + "-" + indexpp + "-" + localizacionId + "-" + indexlocalizacion + "-" + politicaDependienteId + "-" + indexpoliticacontenida).attr('disabled', true);
                            $("#errortottalmsn" + fuenteID + "-" + indexFuentes + "-" + politicaId + "-" + indexpp + "-" + localizacionId + "-" + indexlocalizacion + "-" + politicaDependienteId + "-" + indexpoliticacontenida).fadeOut();
                            $("#Guardar" + fuenteID + indexFuentes + politicaId + indexpp + localizacionId + indexlocalizacion + politicaDependienteId + indexpoliticacontenida).attr('disabled', false);

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

            if (event) {
                event.target.value = cantidadDecimales == 0 ? event.target.value.replace(".", "") : procesarNumero(event.target.value, cantidadDecimales);

                const val = event.target.value.toString();
                const decimalCnt = val.split('.')[1] ? val.split('.')[1].length : 0;
                var total = event.target.value = decimalCnt && decimalCnt > cantidadDecimales ? event.target.value : parseFloat(val).toFixed(cantidadDecimales);
                event.target.value = Intl.NumberFormat('es-co', { minimumFractionDigits: cantidadDecimales, }).format(total);
            }

            politicaContenida.valorPP = valorPP;
            politicaContenida.valorPC = valorPC;
            politicaContenida.personasPP = personasPP;
            politicaContenida.personasPC = personasPC;
            politicaContenida.politicaDependienteId = politicaDependienteId;
        }

        function procesarNumero(value, cantidadDecimales, convertFloat = true) {
            if (!Number(value)) {
                value = limpiaNumero(value, cantidadDecimales);

            } else if (!convertFloat) {
                value = value.replace(",", ".");
            } else {
                value = parseFloat(value.replace(",", "."));
            }

            return value;
        }

        function limpiaNumero(valor, cantidadDecimales) {
            if (valor == "0.00" || valor == "0") return 0;
            if (`${valor.toLocaleString().split(",")[1]}` == 'undefined') return `${valor.toLocaleString().split(",")[0].toString()}`;
            return `${valor.toLocaleString().split(",")[0].toString().replaceAll(".", "")}.${valor.toLocaleString().split(",")[1].toString()}`;
        }

        vm.actualizaFila = function (politicaContenida, tab, fuenteID, indexFuentes, politicaId, indexpp, localizacionId, indexlocalizacion, politicaDependienteId, indexpoliticacontenida, event, cantidadDecimales) {
            calcularTotales(politicaContenida, tab, fuenteID, indexFuentes, politicaId, indexpp, localizacionId, indexlocalizacion, politicaDependienteId, indexpoliticacontenida, event, cantidadDecimales);
        }

        function ConvertirNumero(numero) {
            return numberFormatter.format(numero);
        }

        function ConvertirNumero2decimales(numero) {
            return numberFormatter.format(numero);
        }

        vm.validateFormat = function (event, cantidad) {

            if ((event.keyCode < 48 || event.keyCode > 57) && event.keyCode != 44) {
                event.preventDefault();
            }
        }

        vm.validarTamanio = function (event, cantidad) {

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

            event.target.value = procesarNumero(event.target.value, null, false);

            var newValue = event.target.value;
            var spiltArray = String(newValue).split("");
            var tamanioPermitido = 15;
            var tamanio = event.target.value.length;
            var permitido = false;
            permitido = event.target.value.toString().includes(".");
            if (permitido) {
                tamanioPermitido = 18;

                var n = String(newValue).split(".");
                if (n[1]) {
                    var n2 = n[1].slice(0, cantidad);
                    newValue = [n[0], n2].join(".");
                    if (spiltArray.length === 0) return;
                    if (spiltArray.length === 1 && (spiltArray[0] == '-' || spiltArray[0] === '.')) return;
                    if (spiltArray.length === 2 && newValue === '-.') return;


                    if (n[1].length > cantidad) {
                        tamanioPermitido = n[0].length + cantidad;
                        event.target.value = n[0] + '.' + n[1].slice(0, cantidad);
                        return;
                    }

                    if (cantidad == 2) {
                        if ((n[1].length == 1 && n[1] > 9) || (n[1].length > 1 && n[1] > 99)) {
                            event.preventDefault();
                        }
                    }
                    else {
                        if ((n[1].length == 3 && n[1] > 999) || (n[1].length > 3 && n[1] > 9999)) {
                            event.preventDefault();
                        }
                    }
                }
                else {
                    var n2 = "";
                    newValue = [n[0], n2].join(".");
                    event.target.value = newValue;
                }
            }
            else {
                if (tamanio > tamanioPermitido && event.keyCode != 44 && event.keyCode != 188) {
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

            setTimeout(function () {
                PintarValidaciones();
            }, 500);
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

                if (vm.imgmasFuentePolAnt != undefined && vm.imgmenosFuentePolAnt != undefined) {
                    if (vm.imgmasFuentePolAnt !== imgmas) {
                        $("#ico-" + vm.FuenteConsultada + "-" + vm.indexFuentesConsultada + "-" + vm.PoliticaConsultada + "-" + vm.indexpoliticaConsultada).html('+');
                        vm.imgmasFuentePolAnt.style.display = 'block';
                        vm.imgmenosFuentePolAnt.style.display = 'none';
                    }
                }

            } else {
                $("#ico-" + fuenteId + "-" + indexFuentes + "-" + politicaid + "-" + indexpolitica).html('+');
                imgmas.style.display = 'block';
                imgmenos.style.display = 'none';
            }

            vm.imgmasFuentePolAnt = imgmas;
            vm.imgmenosFuentePolAnt = imgmenos;
            vm.FuenteConsultada = fuenteId;
            vm.PoliticaConsultada = politicaid;
            vm.indexpoliticaConsultada = indexpolitica;
            vm.indexFuentesConsultada = indexFuentes;

            if (variable === "-") {
                vm.FuenteConsultada = undefined;
                vm.PoliticaConsultada = undefined;
            }

            setTimeout(function () {
                PintarValidaciones();
            }, 500);

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


                if (vm.imgmasLocalizaAnt != undefined && vm.imgmenosLocalizaAnt != undefined) {
                    if (vm.imgmasLocalizaAnt !== imgmas) {
                        $("#ico-" + vm.FuenteConsultada + "-" + vm.indexFuentesConsultada + "-" + vm.PoliticaConsultada + "-" + vm.indexpoliticaConsultada + "-" + vm.LocalizacionConsultada + "-" + vm.indexlocalizacionConsultada).html('+');
                        vm.imgmasLocalizaAnt.style.display = 'block';
                        vm.imgmenosLocalizaAnt.style.display = 'none';
                    }
                }

            } else {
                $("#ico-" + fuenteId + "-" + indexFuentes + "-" + politicaid + "-" + indexpolitica + "-" + localizacionid + "-" + indexlocalizacion).html('+');
                imgmas.style.display = 'block';
                imgmenos.style.display = 'none';
            }



            vm.imgmasLocalizaAnt = imgmas;
            vm.imgmenosLocalizaAnt = imgmenos;
            vm.FuenteConsultada = fuenteId;
            vm.PoliticaConsultada = politicaid;
            vm.LocalizacionConsultada = localizacionid;
            vm.indexpoliticaConsultada = indexpolitica;
            vm.indexFuentesConsultada = indexFuentes;
            vm.indexlocalizacionConsultada = indexlocalizacion;

            setTimeout(function () {
                PintarValidaciones();
            }, 500);

        }

        vm.AbrilNivel4 = function (fuenteId, indexFuentes, politicaid, indexpolitica, localizacionid, indexlocalizacion, politicaDependienteId, indexpoliticaDependiente, dimensionid) {
            var grilla = document.getElementById("sec-" + fuenteId + "-" + indexFuentes + "-" + politicaid + "-" + indexpolitica + "-" + localizacionid + "-" + indexlocalizacion + "-" + politicaDependienteId + "-" + indexpoliticaDependiente);
            if (grilla != undefined) {
                grilla.classList.remove('hidden');
            }

            vm.mostrarTab(1, fuenteId, indexFuentes, politicaid, indexpolitica, localizacionid, indexlocalizacion, politicaDependienteId, indexpoliticaDependiente, dimensionid);

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

        function PintarValidaciones() {

            var valuelocalStorage = localStorage.getItem('ErroresAJUFOC');
            if (valuelocalStorage != null && valuelocalStorage != undefined && valuelocalStorage != []) {
                var valor = 0;
                vm.ERRORCRUCEAJU(valuelocalStorage);
            }
        }

        vm.mostrarTab = function (origen, fuenteid, indexFuentes, politicaid, indexpp, localizacionid, indexlocalizacion, politicaDependienteId, indexpoliticacontenida, dimensionid) {

            var politicaContenida = null;
            vm.CrucePoliticasAjustes.Fuentes.forEach(fuentes => {
                if (fuentes.FuenteID == fuenteid) {
                    fuentes.PoliticaPrincipal.forEach(pp => {
                        if (pp.PoliticaId == politicaid) {
                            pp.Localizaciones.forEach(localizaciones => {
                                if (localizaciones.LocalizacionId == localizacionid) {
                                    localizaciones.RelacionPoliticas.forEach(pr => {
                                        if (pr.PoliticaDependienteId == politicaDependienteId && pr.DimensionId == dimensionid) {
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
                        $("#Editar" + fuenteid + '-' + indexFuentes + '-' + politicaid + '-' + indexpp + '-' + localizacionid + '-' + indexlocalizacion + '-' + politicaDependienteId + '-' + indexpoliticacontenida).attr('disabled', false);
                        /*$("#Guardar" + fuenteid + indexFuentes + politicaid + indexpp + localizacionid + indexlocalizacion + politicaDependienteId + indexpoliticacontenida).attr('disabled', false);*/
                        //$("#Guardar" + fuenteid + '-' + indexFuentes + '-' + politicaid + '-' + indexpp + '-' + localizacionid + '-' + indexlocalizacion + '-' + politicaDependienteId + '-' + indexpoliticacontenida).fadeIn();
                        break;
                    }
                case 2:
                    {
                        $("#Editar" + fuenteid + '-' + indexFuentes + '-' + politicaid + '-' + indexpp + '-' + localizacionid + '-' + indexlocalizacion + '-' + politicaDependienteId + '-' + indexpoliticacontenida).attr('disabled', true);
                        //$("#Guardar" + fuenteid + '-' + indexFuentes + '-' + politicaid + '-' + indexpp + '-' + localizacionid + '-' + indexlocalizacion + '-' + politicaDependienteId + '-' + indexpoliticacontenida).fadeOut();						

                        break;
                    }
            }
            calcularTotales(politicaContenida, origen, fuenteid, indexFuentes, politicaid, indexpp, localizacionid, indexlocalizacion, politicaDependienteId, indexpoliticacontenida, null, null);

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
            $("#Guardar" + fuenteid + indexFuentes + politicaid + indexpp + localizacionid + indexlocalizacion + politicaDependienteId + indexpoliticacontenida).attr('disabled', true);
        }

        vm.habilitarEditar = function (fuenteid, indexFuentes, politicaId, indexpp, localizacionId, indexlocalizacion, politicaContenida, politicaDependienteId, indexpoliticacontenida) {
            politicaContenida.HabilitaEditarLocalizador = true;
            $("#Guardar" + fuenteid + indexFuentes + politicaId + indexpp + localizacionId + indexlocalizacion + politicaDependienteId + indexpoliticacontenida).attr('disabled', false);
            vm.CrucePoliticasAjustesOrigen = "";
            vm.CrucePoliticasAjustesOrigen = JSON.stringify(vm.CrucePoliticasAjustes);

            angular.forEach(politicaContenida.CrucePoliticasVigencias, function (series) {
                series.ValorCruceDependientePrincipalOriginal = series.ValorCruceDependientePrincipal;
                series.PersonaCruceOriginal = series.PersonaCruce;
            });
        }

        vm.cancelarEdicion = function (fuenteid, indexFuentes, politicaId, indexpp, localizacionId, indexlocalizacion, politicaContenida, politicaDependienteId, indexpoliticacontenida, dimensionid) {
            utilidades.mensajeWarning("¿Está seguro de continuar?", function funcionContinuar() {
                politicaContenida.HabilitaEditarLocalizador = false;
                asignarValoresOriginales(fuenteid, indexFuentes, politicaId, indexpp, localizacionId, indexlocalizacion, politicaContenida, politicaDependienteId, indexpoliticacontenida, dimensionid);
                $("#Guardar" + fuenteid + indexFuentes + politicaId + indexpp + localizacionId + indexlocalizacion + politicaDependienteId + indexpoliticacontenida).attr('disabled', true);
                new utilidades.mensajeSuccess("", false, false, false, "Se ha cancelado la edición.");
            }, function funcionCancelar(reason) {
                //console.log("reason", reason);
            }, 'Aceptar', 'Cancelar', 'Los posibles datos que haya diligenciado en la tabla se perderán.');
        }

        function asignarValoresOriginales(fuenteid, indexFuentes, politicaId, indexpp, localizacionId, indexlocalizacion, politicaContenida, politicaDependienteId, indexpoliticacontenida, dimensionid) {

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
                                        if (prorigen.PoliticaDependienteId == politicaDependienteId && prorigen.DimensionId == dimensionid) {
                                            politicaContenidaorigen = prorigen;
                                        }
                                    });
                                }
                            });
                        }
                    });
                }
            });

            angular.forEach(politicaContenida.CrucePoliticasVigencias, function (series) {
                series.ValorCruceDependientePrincipal = series.ValorCruceDependientePrincipalOriginal;
                series.PersonaCruce = series.PersonaCruceOriginal;
            });

            politicaContenidaorigen.HabilitaEditarLocalizador = false;
            calcularTotales(politicaContenidaorigen, 1, fuenteid, indexFuentes, politicaId, indexpp, localizacionId, indexlocalizacion, politicaDependienteId, indexpoliticacontenida, null, null);
        }

        vm.GuardarAjustes = function (FuenteID, PoliticaId, LocalizacionId, PoliticaDependienteId, CrucePoliticasVigencias, DimensionId) {

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
                    PersonaCruce: cpv.PersonaCruce,
                    DimensionId: DimensionId

                };
                vm.CrucePoliticasGuardar.push(valorescpv);
            });

            return focalizacionAjustesSGPServicio.GuardarCrucePoliticasAjustesSGP(vm.CrucePoliticasGuardar, $sessionStorage.usuario.permisos.IdUsuarioDNP).then(function (response) {
                if (response.statusText === "OK" || response.status === 200) {
                    if (response.data.Mensaje == null) {
                        init();
                        vm.CrucePoliticasGuardar = [];
                        utilidades.mensajeSuccess('', false, false, false, "Los datos fueron guardados con éxito.");
                    }
                    else {
                        var mensajeError = JSON.parse(response.data.Mensaje);

                        var mensajeReturn = "<span class='anttituhori'><lu>";
                        console.log(mensajeError);
                        try {
                            for (var i = 0; i < mensajeError.ListaErrores.length; i++) {
                                mensajeReturn += '<li>' + mensajeError.ListaErrores[i].Error + '</li>';
                            }
                            mensajeReturn += "</lu></span>";
                            utilidades.mensajeWarning(mensajeReturn, null, null, null, null);
                        }
                        catch {
                            mensajeReturn = mensajeError.Mensaje;
                        }
                    }
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


            return focalizacionAjustesSGPServicio.ObtenerPoliticasTransversalesResumenSGP(bpin, usuarioDNP, idInstancia).then(
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

        vm.notificacionCambiosCapitulos = function (nombreCapituloHijo) {
            console.log("vm.notificacionCambiosCapitulos Cruce politias", nombreCapituloHijo)
            if (vm.componentesRefresh.includes(nombreCapituloHijo)) {
                vm.init();
            }
        }

        vm.notificacionValidacionHijo = function (handler) {
            vm.notificacionErrores = handler;
        }

        vm.validacionGuardadoHijo = function (handler) {
            vm.validacionGuardado = handler;
        }

        function ObtenerhabilitadorNAPP(politicaPrincipalId) {
            vm.habilitadorNA = 0;

            switch (politicaPrincipalId) {
                case 3:
                    vm.habilitadorNA = 1;
                    break;
                case 6:
                    vm.habilitadorNA = 1;
                    break;
                case 7:
                    vm.habilitadorNA = 1;
                    break;
                case 8:
                    vm.habilitadorNA = 1;
                    break;
                case 9:
                    vm.habilitadorNA = 1;
                    break;
                case 10:
                    vm.habilitadorNA = 1;
                    break;
                case 11:
                    vm.habilitadorNA = 1;
                    break;
                case 12:
                    vm.habilitadorNA = 1;
                    break;
                case 13:
                    vm.habilitadorNA = 1;
                    break;
                case 14:
                    vm.habilitadorNA = 1;
                    break;
                case 16:
                    vm.habilitadorNA = 1;
                    break;
                case 17:
                    vm.habilitadorNA = 1;
                    break;
                case 20:
                    vm.habilitadorNA = 1;
                    break;
                case 1508:
                    vm.habilitadorNA = 1;
                    break;
                default:
                    vm.habilitadorNA = 0;
            }

            return vm.habilitadorNA;
        }

        function ObtenerhabilitadorNAPD(politicaDependienteId) {
            vm.habilitadorNA = 0;

            switch (politicaDependienteId) {
                case 3:
                    vm.habilitadorNA = 1;
                    break;
                case 6:
                    vm.habilitadorNA = 1;
                    break;
                case 7:
                    vm.habilitadorNA = 1;
                    break;
                case 8:
                    vm.habilitadorNA = 1;
                    break;
                case 9:
                    vm.habilitadorNA = 1;
                    break;
                case 10:
                    vm.habilitadorNA = 1;
                    break;
                case 11:
                    vm.habilitadorNA = 1;
                    break;
                case 12:
                    vm.habilitadorNA = 1;
                    break;
                case 13:
                    vm.habilitadorNA = 1;
                    break;
                case 14:
                    vm.habilitadorNA = 1;
                    break;
                case 16:
                    vm.habilitadorNA = 1;
                    break;
                case 17:
                    vm.habilitadorNA = 1;
                    break;
                case 20:
                    vm.habilitadorNA = 1;
                    break;
                case 1508:
                    vm.habilitadorNA = 1;
                    break;
                default:
                    vm.habilitadorNA = 0;
            }

            return vm.habilitadorNA;
        }

        function ObtenerhabilitadorNAPD(politicaDependienteId) {
            vm.habilitadorNA = 0;

            switch (politicaDependienteId) {
                case 3:
                    vm.habilitadorNA = 1;
                    break;
                case 6:
                    vm.habilitadorNA = 1;
                    break;
                case 7:
                    vm.habilitadorNA = 1;
                    break;
                case 8:
                    vm.habilitadorNA = 1;
                    break;
                case 9:
                    vm.habilitadorNA = 1;
                    break;
                case 10:
                    vm.habilitadorNA = 1;
                    break;
                case 11:
                    vm.habilitadorNA = 1;
                    break;
                case 12:
                    vm.habilitadorNA = 1;
                    break;
                case 13:
                    vm.habilitadorNA = 1;
                    break;
                case 14:
                    vm.habilitadorNA = 1;
                    break;
                case 16:
                    vm.habilitadorNA = 1;
                    break;
                case 17:
                    vm.habilitadorNA = 1;
                    break;
                case 20:
                    vm.habilitadorNA = 1;
                    break;
                case 1508:
                    vm.habilitadorNA = 1;
                    break;
                default:
                    vm.habilitadorNA = 0;
            }

            return vm.habilitadorNA;
        }

        function ObtenerhabilitadorNAPP(politicaPrincipalId) {
            vm.habilitadorNA = 0;

            switch (politicaPrincipalId) {
                case 3:
                    vm.habilitadorNA = 1;
                    break;
                case 6:
                    vm.habilitadorNA = 1;
                    break;
                case 7:
                    vm.habilitadorNA = 1;
                    break;
                case 8:
                    vm.habilitadorNA = 1;
                    break;
                case 9:
                    vm.habilitadorNA = 1;
                    break;
                case 10:
                    vm.habilitadorNA = 1;
                    break;
                case 11:
                    vm.habilitadorNA = 1;
                    break;
                case 12:
                    vm.habilitadorNA = 1;
                    break;
                case 13:
                    vm.habilitadorNA = 1;
                    break;
                case 14:
                    vm.habilitadorNA = 1;
                    break;
                case 16:
                    vm.habilitadorNA = 1;
                    break;
                case 17:
                    vm.habilitadorNA = 1;
                    break;
                case 20:
                    vm.habilitadorNA = 1;
                    break;
                case 1508:
                    vm.habilitadorNA = 1;
                    break;
                default:
                    vm.habilitadorNA = 0;
            }

            return vm.habilitadorNA;
        }
        /* ------------------------ Validaciones ---------------------------------*/

        vm.notificacionValidacionPadre = function (errores) {
            console.log("Validación  - Gestion Recursos Fuentes de financiación");
            vm.limpiarErrores();
            if (errores != undefined) {
                var erroresRelacionconlapl = errores.find(p => (p.Seccion + p.Capitulo) == vm.nombreComponente);

                var isValid = true;

                if (erroresRelacionconlapl != undefined) {

                    var erroresJson = erroresRelacionconlapl.Errores == "" ? [] : JSON.parse(erroresRelacionconlapl.Errores);

                    if (vm.notificacionErrores != null && erroresJson != null) {
                        vm.notificacionErrores(erroresJson[vm.nombreComponente]);
                    }

                    isValid = (erroresJson == null || erroresJson.length == 0);
                    if (!isValid) {
                        localStorage.removeItem('ErroresAJUFOC');
                        let arrErr = [];
                        erroresJson[vm.nombreComponente].forEach(p => {
                            arrErr.push(p.Descripcion);
                            if (vm.errores[p.Error] != undefined) vm.errores[p.Error](p.Descripcion);
                            vm.validarSeccion(p.Error, p.Descripcion, false);
                        });
                        localStorage.setItem('ErroresAJUFOC', JSON.stringify(arrErr));
                        setTimeout(function () {
                            PintarValidaciones();
                        }, 500);
                    }
                }
                vm.notificacionestado({ nombreComponente: vm.nombreComponente, esValido: isValid });

            }
        };

        vm.validarSeccion = function (tipoError, errores, esValido) {
            let mensajeErr = errores.split(';');
            var campomensajeerror = document.getElementById(tipoError);
            if (campomensajeerror != undefined) {
                if (!esValido) {
                    campomensajeerror.innerHTML = "<span class='d-inline-block ico-advertencia'></span><span class='pl-2'>" + mensajeErr[0] + "</span>";
                    campomensajeerror.classList.remove('hidden');
                } else {
                    campomensajeerror.classList.remove("ico-advertencia");
                }
            }
        }

        vm.limpiarErrores = function () {
            var valor = 0;
            if (vm.CrucePoliticasAjustes != undefined && vm.CrucePoliticasAjustes != null) {
                if (vm.CrucePoliticasAjustes.Fuentes != null && vm.CrucePoliticasAjustes.Fuentes != undefined && vm.CrucePoliticasAjustes.Fuentes.length > 0) {
                    vm.CrucePoliticasAjustes.Fuentes.forEach(f => {

                        var seccionf = document.getElementById("errorcrucepfuente-" + f.FuenteID);
                        if (seccionf != undefined) {
                            seccionf.classList.add('hidden');
                        }

                        f.PoliticaPrincipal.forEach(pp => {
                            var seccionpp = document.getElementById("errorcrucepfuentepp-" + f.FuenteID + "-" + pp.PoliticaId);
                            if (seccionpp != undefined) {
                                seccionpp.classList.add('hidden');
                            }

                            pp.Localizaciones.forEach(l => {
                                var seccionloc = document.getElementById("errorcrucepfuentepploc-" + f.FuenteID + "-" + pp.PoliticaId + "-" + l.LocalizacionId);
                                if (seccionloc != undefined) {
                                    seccionloc.classList.add('hidden');
                                }

                                l.RelacionPoliticas.forEach(pd => {
                                    var seccionpd = document.getElementById("errorcrucepfuentepploc-" + f.FuenteID + "-" + pp.PoliticaId + "-" + l.LocalizacionId + "-" + pd.PoliticaDependienteId);
                                    if (seccionpd != undefined) {
                                        seccionpd.classList.add('hidden');
                                    }

                                    var seccionpd = document.getElementById("errorcrucepfuentepplocpdv-" + f.FuenteID + "-" + pp.PoliticaId + "-" + l.LocalizacionId + "-" + pd.PoliticaDependienteId + "-" + pd.DimensionId);
                                    if (seccionpd != undefined) {
                                        seccionpd.classList.add('hidden');
                                    }
                                });
                            });
                        });
                    });
                }
            }
        }

        vm.ERRORCRUCEAJU = function (errores) {
            var valor = 0;

            var valores = errores.split(';');
            valores.forEach(v => {
                var err = v.split('|');

                var seccionf = document.getElementById("errorcrucepfuente-" + err[0]);
                if (seccionf != undefined) {
                    seccionf.classList.remove('hidden');
                }
                var seccionpp = document.getElementById("errorcrucepfuentepp-" + err[0] + "-" + err[1]);
                if (seccionpp != undefined) {
                    seccionpp.classList.remove('hidden');
                }

                var seccionloc = document.getElementById("errorcrucepfuentepploc-" + err[0] + "-" + err[1] + "-" + err[2]);
                if (seccionloc != undefined) {
                    seccionloc.classList.remove('hidden');
                }

                var seccionpd = document.getElementById("errorcrucepfuentepplocpd-" + err[0] + "-" + err[1] + "-" + err[2] + "-" + err[3]);
                if (seccionpd != undefined) {
                    seccionpd.classList.remove('hidden');
                }

                var seccionpd = document.getElementById("errorcrucepfuentepplocpdv-" + err[0] + "-" + err[1] + "-" + err[2] + "-" + err[3] + "-" + err[5]);
                if (seccionpd != undefined) {
                    seccionpd.classList.remove('hidden');
                }
            });
        }

        vm.errores = {
            'ERRORCRUCEAJU': vm.ERRORCRUCEAJU
        }
    }

    angular.module('backbone').component('crucePoliticasTransversalesSgp', {
        templateUrl: "/src/app/formulario/ventanas/SGP/gestionRecursosSGP/componentes/focalizacion/CrucePoliticasTransversales/crucePoliticasTransversalesSgp.html",
        controller: crucePoliticasTransversalesSgpController,
        controllerAs: "vm",
        bindings: {
            callback: '&',
            notificacionvalidacion: '&',
            notificacionestado: '&',
            guardadoevent: '&',
            notificarrefresco: '&',
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