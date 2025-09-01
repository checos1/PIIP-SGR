(function () {
    'use strict';

    fuentesSgpController.$inject = [
        'gestionRecursosSGPServicio',
        '$sessionStorage',
        '$uibModal',
        'utilidades',
        'transversalSgpServicio'
    ];

    function fuentesSgpController(
        gestionRecursosSGPServicio,
        $sessionStorage,
        $uibModal,
        utilidades,
        transversalSgpServicio
    ) {
        var vm = this;
        vm.init = init;
        vm.nombreComponente = "sgprecursosfuentesfinanciacionsgp";

        vm.abrirModalAgregarFuente = abrirModalAgregarFuente;
        vm.abrirModalAgregarDatosAdicionales = abrirModalAgregarDatosAdicionales;
        vm.BPIN = $sessionStorage.idObjetoNegocio;
        vm.eliminarFuente = eliminarFuente;
        vm.mostrarOcultarCofinanciador = mostrarOcultarCofinanciador;
        vm.actualizarVigencia = actualizarVigencia;
        vm.disabled = true;
        vm.TotalInversionVigencia = 0;
        vm.notificacionErrores = null;
        vm.erroresActivos = [];
        vm.listaSinCofin = [];
        vm.listaConCofin = [];
        vm.ff = [];
        vm.recargaGuardado = null;
        vm.permiteEditar = false;
        vm.validateFormat = validateFormat;
        vm.ConvertirNumero = ConvertirNumero;
        vm.seccionCapitulo = null;
        vm.eventoValidar = eventoValidar;
        var currentYear = new Date().getFullYear();
        var listaFuentesBase = [];

        vm.etapaEdicion = "";
        vm.habilitaBotones = true;
        var cancelaEdicion = false;
        var es_edicion = false;
        vm.soloLectura = false;

        //vm.Regionalizacion = null;

        function init() {
            vm.permiteEditar = false;
            vm.model = {
                modulos: {
                    administracion: false,
                    backbone: true
                }
            }
            vm.notificacionvalidacion({ handler: vm.notificacionValidacionPadre, nombreComponente: vm.nombreComponente, esValido: true });
            vm.obtenerFuentes(vm.BPIN);
            vm.limpiarErrores();
            ObtenerSeccionCapitulo();
            vm.notificacioncambios({ handler: vm.notificacionCambiosCapitulos, nombreComponente: vm.nombreComponente });
        }

        vm.notificacionCambiosCapitulos = function (nombreCapituloHijo) {
            vm.recargaGuardado();
            //if (nombreCapituloHijo == 'datosgeneraleshorizonte') {
            //    vm.init();
            //    vm.recargaGuardado();
            //    vm.recargaGuardadoCostos();
            //}
        }

        vm.notificacionValidacionHijo = function (handler) {
            vm.notificacionErrores = handler;
        }

        vm.recargaresumen = function (handler) {
            vm.recargaGuardado = handler;
        }

        vm.validarfinanciador = function (financiador) {
            if (financiador === 'Privadas') return true;
            else return false;
        }
        //vm.recargaregionalizacion = function (handler) {
        //    vm.Regionalizacion = handler;
        //}

        vm.obtenerFuentes = function (bpin) {

            var idInstancia = $sessionStorage.idInstancia;
            var idAccion = $sessionStorage.idAccion;
            var idFormulario = $sessionStorage.idNivel;
            var idUsuario = $sessionStorage.usuario.permisos.IdUsuarioDNP;
            var entityTypeProyecto = $sessionStorage.idEntidad;

            return gestionRecursosSGPServicio.obtenerFuentesFinanciacionVigenciaProyectoSGP($sessionStorage.idInstancia, usuarioDNP, idInstancia).then(
                function (respuesta) {
                    if (respuesta.data != null && respuesta.data != "") {

                        var arreglolistas = jQuery.parseJSON(respuesta.data);
                        var arregloGeneral = jQuery.parseJSON(arreglolistas)
                        var arregloDatosGenerales = arregloGeneral.Datos_Generales;
                        sessionStorage.setItem("listaFuenteFinanciacion", arreglolistas);
                        var contieneCofinanciador = false;

                        var listaFuentes = [];
                        var listaCofinanciadores = [];
                        var listaVigenciaCofinanciador = [];
                        var listaVigenciaFuente = [];
                        var valorTotalFuente = 0;
                        var permiteHabilitar = true;

                        var valorTotalFuenteCofinanciador = 0;

                        for (var ls = 0; ls < arregloDatosGenerales.length; ls++) {
                            if (arregloGeneral.Datos_Generales[0].TipoFinanciadorId != 0 && arregloGeneral.Datos_Generales[0].Financiador != "") {
                                if (arregloDatosGenerales[ls].Confinanciadores != null) {
                                    contieneCofinanciador = true;
                                    listaVigenciaFuente = null;
                                    var valorTotalCofinanciador = 0;
                                    for (var cf = 0; cf < arregloDatosGenerales[ls].Confinanciadores.length; cf++) {

                                        if (arregloDatosGenerales[ls].Confinanciadores[cf].VigenciaCofinanciador.length >= 0) {
                                            for (var vc = 0; vc < arregloDatosGenerales[ls].Confinanciadores[cf].VigenciaCofinanciador.length; vc++) {
                                                permiteHabilitar = arregloDatosGenerales[ls].Confinanciadores[cf].VigenciaCofinanciador[vc].Vigencia < currentYear ? false : true;
                                                var vigenciaCofinanciador = {
                                                    Vigencia: arregloDatosGenerales[ls].Confinanciadores[cf].VigenciaCofinanciador[vc].Vigencia,
                                                    Valor: parseFloat(arregloDatosGenerales[ls].Confinanciadores[cf].VigenciaCofinanciador[vc].Valor == null ? 0 : arregloDatosGenerales[ls].Confinanciadores[cf].VigenciaCofinanciador[vc].Valor).toFixed(2),
                                                    permiteHabilitar: permiteHabilitar
                                                }
                                                valorTotalCofinanciador += arregloDatosGenerales[ls].Confinanciadores[cf].VigenciaCofinanciador[vc].Valor == null ? 0 : arregloDatosGenerales[ls].Confinanciadores[cf].VigenciaCofinanciador[vc].Valor;
                                                listaVigenciaCofinanciador.push(vigenciaCofinanciador);
                                                valorTotalFuente += arregloDatosGenerales[ls].Confinanciadores[cf].VigenciaCofinanciador[vc].Valor == null ? 0 : arregloDatosGenerales[ls].Confinanciadores[cf].VigenciaCofinanciador[vc].Valor;
                                                permiteHabilitar = true;
                                            }
                                        }

                                        var cofinanciador = {
                                            cofinanciadorId: arregloDatosGenerales[ls].Confinanciadores[cf].CofinanciadorId,
                                            tipoCofinanciador: arregloDatosGenerales[ls].Confinanciadores[cf].TipoCofinanciador,
                                            codigo: arregloDatosGenerales[ls].Confinanciadores[cf].Codigo,
                                            codigoCofinanciador: arregloDatosGenerales[ls].Confinanciadores[cf].CodigoCofinanciador,
                                            listaVigenciasCofinanciador: listaVigenciaCofinanciador,
                                            valorTotalCof: valorTotalCofinanciador
                                        }
                                        listaCofinanciadores.push(cofinanciador);
                                        listaVigenciaCofinanciador = [];
                                        valorTotalFuenteCofinanciador += valorTotalCofinanciador;
                                        valorTotalCofinanciador = 0;
                                    }
                                } else {
                                    if (arregloDatosGenerales[ls].Vigencia.length >= 0) {

                                        for (var vf = 0; vf < arregloDatosGenerales[ls].Vigencia.length; vf++) {
                                            permiteHabilitar = arregloDatosGenerales[ls].Vigencia[vf].Vigencia < currentYear ? false : true;
                                            var vigenciaFuente = {
                                                Vigencia: arregloDatosGenerales[ls].Vigencia[vf].Vigencia,
                                                Valor: parseFloat(arregloDatosGenerales[ls].Vigencia[vf].Valor == null ? 0 : arregloDatosGenerales[ls].Vigencia[vf].Valor).toFixed(2),
                                                permiteHabilitar: permiteHabilitar
                                            }
                                            valorTotalFuente += arregloDatosGenerales[ls].Vigencia[vf].Valor == null ? 0 : arregloDatosGenerales[ls].Vigencia[vf].Valor;
                                            listaVigenciaFuente.push(vigenciaFuente);
                                            permiteHabilitar = true;
                                        }
                                    }
                                }

                                //var habilitaDatosAdicionales = true;
                                //if (arregloDatosGenerales[ls].TipoFinanciador.search("PGN")> 0)
                                //    habilitaDatosAdicionales = false;
                                //else if (entityTypeProyecto != null && entityTypeProyecto == arregloDatosGenerales[ls].EntityTypeCatalogOptionId && arregloDatosGenerales[ls].crtypeid == arregloDatosGenerales[ls].TipoFinanciadorId)
                                //    habilitaDatosAdicionales = false;

                                var habilitaDatosAdicionales = true; //101 -102
                                if (arregloDatosGenerales[ls].CrtypeId == 1) {
                                    if (arregloDatosGenerales[ls].TipoFinanciador.search("PGN") > 0)
                                        habilitaDatosAdicionales = false;
                                }
                                else if (entityTypeProyecto != null && entityTypeProyecto == arregloDatosGenerales[ls].EntityTypeCatalogOptionId)
                                    habilitaDatosAdicionales = false;

                                var fuente = {
                                    Id: arregloDatosGenerales[ls].FuenteId,
                                    Etapa: arregloDatosGenerales[ls].Etapa,
                                    TipoFinanciador: arregloDatosGenerales[ls].TipoFinanciador,
                                    Financiador: arregloDatosGenerales[ls].Financiador,
                                    Recurso: arregloDatosGenerales[ls].Recurso,
                                    Sector: arregloDatosGenerales[ls].Sector,
                                    DatosAdicionales: arregloDatosGenerales[ls].Adicional,
                                    habilitaDatosAdicionales: habilitaDatosAdicionales,
                                    vigenciaFuente: listaVigenciaFuente,
                                    Cofinanciador: listaCofinanciadores,
                                    contieneCofinanciador: contieneCofinanciador,
                                    valorTotalFuente: valorTotalFuente
                                }
                                if (!(valorTotalFuente == 0 && valorTotalFuenteCofinanciador == 0) || arregloDatosGenerales[ls].FuenteFirme == 0) {
                                    listaFuentes.push(fuente);
                                }

                                listaCofinanciadores = [];
                                listaVigenciaFuente = [];
                                contieneCofinanciador = false;
                                valorTotalFuente = 0;
                            }
                        }
                        vm.listaFuentes = angular.copy(listaFuentes);
                        listaFuentesBase = listaFuentes;

                        if (listaFuentes.length == 0) {
                            transversalSgpServicio.notificarCambio({ enableRegionalizacion: false });
                            eliminarCapitulosModificados();
                        }
                        else {
                            const enableRegionalizacion = listaFuentes.some(fuente => fuente.valorTotalFuente > 0);
                            transversalSgpServicio.notificarCambio({ enableRegionalizacion: enableRegionalizacion });
                            guardarCapituloModificado();
                        }
                        vm.soloLectura = $sessionStorage.soloLectura;
                    }
                    
                }
            );
        }

        vm.validaEstado = function (datosAdicionales, tieneCofinanciador) {
            if (datosAdicionales && tieneCofinanciador)
                return true;
        }

        vm.ActivarEditar = function (btnid, tipoTipoFinanciador, etapa, contieneCofinanciador, habilitaDatosAdicionales) {
            if (tipoTipoFinanciador !== 'Privadas' && habilitaDatosAdicionales && !vm.validaEstado(habilitaDatosAdicionales, contieneCofinanciador)) {
                utilidades.mensajeError("Debe agregar al menos un cofinanciador para editar las vigencias.");
                return;
            }
            if (vm.disabled == true) {
                $("#Editar" + btnid).html("CANCELAR");
                vm.disabled = false;
                $("#Guardar" + btnid + etapa).attr('class', 'btnguardarDNP');
                $("#Guardar" + btnid + etapa).attr('disabled', false);
                es_edicion = true;
                $("#inp-" + btnid + "-" + etapa).css("display", "block");
                $("#lbl-" + btnid + "-" + etapa).css("display", "none");
                $(`#add-cof-${btnid}`).css("pointer-events", "none");
                // mostrarOcultarCofinanciador(btnid, tipoTipoFinanciador, etapa);

            } else {
                vm.permiteEditar = false;
                $("#inp-" + btnid + "-" + etapa).css("display", "none");
                $("#lbl-" + btnid + "-" + etapa).css("display", "block");
                $("#Editar" + btnid).html("EDITAR");
                vm.disabled = true;
                $("#Guardar" + btnid + etapa).attr('disabled', true);
                vm.listaFuentes = angular.copy(listaFuentesBase);
                $(".vigenciaInput").attr("disabled", true);
                $(`#add-cof-${btnid}`).css("pointer-events", "auto");
                cancelaEdicion = true;
                //mostrarOcultarCofinanciador(btnid, tipoTipoFinanciador, etapa);
            }

        }

        function ConvertirNumero(numero) {
            return new Intl.NumberFormat('es-co', {
                minimumFractionDigits: 2,
            }).format(numero);
        }

        function abrirModalAgregarFuente() {
            $uibModal.open({
                templateUrl: 'src/app/formulario/ventanas/SGP/gestionRecursosSGP/componentes/recursos/fuentes/modalAgregarFuenteSgp.html',
                controller: 'modalAgregarFuenteSgpController',
            }).result.then(function (result) {
                vm.guardadoevent({ nombreComponenteHijo: vm.nombreComponente });
                init();

                //vm.Regionalizacion();
            }, function (reason) {

            }), err => {
                toastr.error("Ocurrió un error al consultar el idAplicacion");
            };

        }

        function eliminarFuente(fuenteId) {

            var idInstancia = $sessionStorage.idNivel;

            return gestionRecursosSGPServicio.eliminarFuentesFinanciacionProyectoSGP(fuenteId, usuarioDNP, idInstancia)
                .then(function (response) {
                    var respuesta = jQuery.parseJSON(response.data)
                    if (respuesta.StatusCode == "200") {
                        utilidades.mensajeSuccess("Se elimino la fuente", false, false, false);
                        vm.guardadoevent({ nombreComponenteHijo: vm.nombreComponente });
                        init();
                        eliminarCapitulosModificados();
                    } else {
                        utilidades.mensajeError("Error al realizar la operación " + respuesta.ReasonPhrase, false);
                        init();
                    }
                })
                .catch(error => {
                    if (error.status == 400) {
                        utilidades.mensajeError(error.data.Message || "Error al realizar la operación");
                        vm.limpiarErrores();
                        return;
                    }
                    utilidades.mensajeError("Error al realizar la operación");
                });
        }

        function abrirModalAgregarDatosAdicionales(idFuente) {
            $sessionStorage.idFuente = idFuente;
            vm.idFuente = idFuente;
            $uibModal.open({
                templateUrl: 'src/app/formulario/ventanas/SGP/gestionRecursosSGP/componentes/recursos/fuentes/modalAgregarDatosAdicionalesSgp.html',
                controller: 'modalAgregarDatosAdicionalesSgpController',
            }).result.then(function (result) {
                vm.guardadoevent({ nombreComponenteHijo: vm.nombreComponente });
                init();
            }, function (reason) {

            }), err => {
                toastr.error("Ocurrió un error al consultar el idAplicacion");
            };
        }

        function consultarDatosCofinanciador() {

            return gestionRecursosSGPServicio.obtenerFuentesProgramarSolicitadoSGP($sessionStorage.idInstancia, usuarioDNP, $sessionStorage.idNivel)
                .then(respuesta => {
                    if (!respuesta.data)
                        return;

                    var arreglolistaDatos = jQuery.parseJSON(respuesta.data);
                    var arregloDatos = jQuery.parseJSON(arreglolistaDatos);
                    var listaSin = arregloDatos.SIN_COFINANCIADOR;
                    var listaCon = arregloDatos.CON_COFINANCIADOR;

                    if (listaSin != null)
                        listaSin.forEach(objLista => {
                            vm.listaSinCofin.push({
                                Etapa: objLista.Etapa,
                                FuenteId: objLista.FuenteId,
                                vigencias: objLista.vigencias
                            });
                        });

                    if (listaCon != null)
                        listaCon.forEach(objListaCon => {
                            vm.listaConCofin.push({
                                Etapa: objListaCon.Etapa,
                                FuenteId: objListaCon.FuenteId,
                                tipoCofinanciadorId: objListaCon.TipoCofinanciadorId,
                                tipoCofinanciador: objListaCon.TipoCofinanciador,
                                financiador: objListaCon.Financiador,
                                vigencias: objListaCon.vigencias,
                            });
                        });
                })
                .catch(error => {
                    console.log(error);
                    toastr.error("Hubo un error al cargar los datos adicionales de la fuente de financiacion");
                });
        }

        function mostrarOcultarCofinanciador(fuenteId, cofinanciadorId, etapa) {
            vm.habilitaBotones = true;
            var variable = $("#ico" + fuenteId + "-" + etapa)[0].src.search("btnMas.svg")
            if (variable > 0)
                $("#ico" + fuenteId + "-" + etapa)[0].src = "Img/btnMenos.svg";
            else {
                $("#ico" + fuenteId + "-" + etapa)[0].src = "Img/btnMas.svg";
                cancelaEdicion = false;
                es_edicion = false;
            }
        }

        function actualizarVigencia(etapa, listaFuentes, FuenteId) {
            vm.etapaEdicion = etapa;
            vm.ff = [];

            listaFuentes.forEach(ff => {
                if (ff.Id == FuenteId && ff.Etapa == etapa) {
                    if (ff.DatosAdicionales == 0) {
                        if (!ff.vigenciaFuente) {
                            ff.vigenciaFuente = [];
                        }
                        ff.vigenciaFuente.forEach(vf => {
                            vf.Valor = vf.Valor;
                        });
                    } else {
                        ff.Cofinanciador.forEach(vcf => {
                            vcf.listaVigenciasCofinanciador.forEach(lvc => {
                                if (lvc.Valor == undefined || lvc.Valor == "0.00")
                                    lvc.Valor = 0;
                                lvc.Valor = lvc.Valor;
                            });
                        });
                    }
                    vm.ff.push(ff);
                }
            });

            var parametros = {
                bpin: vm.BPIN,
                etapa: etapa,
                valores: vm.ff,
                fuenteId: FuenteId
            };

            return gestionRecursosSGPServicio.agregarFuentesProgramarSolicitadoSGP(parametros, usuarioDNP)
                .then(function (response) {
                    var respuesta = jQuery.parseJSON(response.data)
                    if (respuesta.StatusCode == "200") {
                        utilidades.mensajeSuccess("Información guardada satisfactoriamente", false, false, false);
                        vm.recargaGuardado();
                        vm.ActivarEditar(FuenteId, "", etapa);
                        vm.obtenerFuentes(vm.BPIN);
                        vm.guardadoevent({ nombreComponenteHijo: vm.nombreComponente });

                    } else {
                        var ValidacionGuardar = document.getElementById(vm.nombreComponente + "-validacionguardar-error");
                        if (ValidacionGuardar != undefined) {
                            ValidacionGuardar.innerHTML = "<span class='d-inline-block ico-advertencia'></span><span>" + respuesta.ReasonPhrase + "</span>";
                            ValidacionGuardar.classList.remove('hidden');
                        }
                        utilidades.mensajeError("Error al realizar la operación" + respuesta.ReasonPhrase, false);
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

        function formatearNumero(value) {
            var numerotmp = value.toString().replaceAll('.', '');
            return parseInt(numerotmp).toString().replace(/\B(?<!\.\d*)(?=(\d{3})+(?!\d))/g, ".");
        }

        vm.onKeyPress_st = function (e) {
            const charCode = e.which ? e.which : e.keyCode;

            if (charCode !== 44 && charCode > 31 && (charCode < 48 || charCode > 57)) {
                e.preventDefault();
            }

            if (charCode == 13) {
                e.preventDefault();
            }
        }

        vm.actualizaFila = function (event, fuenteId, vigencia, TotalInversionVigencia, cofinanciadorId, etapa) {
            if (Number.isNaN(event.target.value)) {
                return new Intl.NumberFormat('es-co', { minimumFractionDigits: 2, }).format(0.00);
            }

            if (event.target.value == null) {
                return new Intl.NumberFormat('es-co', { minimumFractionDigits: 2, }).format(0.00);
            }

            if (event.target.value == "") {
                return new Intl.NumberFormat('es-co', { minimumFractionDigits: 2, }).format(0.00);
            }

            event.target.value = procesarNumero(event.target.value);

            if (event.currentTarget.value == '') {
                var idInput = event.currentTarget.id;
                $('#' + idInput).focus();
            } else {
                vm.listaFuentes.forEach(objFuents => {
                    if (objFuents.Id == fuenteId && objFuents.Etapa == etapa) {
                        if (objFuents.contieneCofinanciador) {
                            objFuents.Cofinanciador.forEach(objCofin => {
                                var totalVigenciasCofinanciador = 0;
                                objCofin.listaVigenciasCofinanciador.forEach(objVigenciasCof => {
                                    totalVigenciasCofinanciador += parseFloat(procesarNumero(objVigenciasCof.Valor));
                                });

                                objCofin.valorTotalCof = totalVigenciasCofinanciador;
                                objFuents.valorTotalFuente = totalVigenciasCofinanciador;
                            });
                        } else {
                            var totalVigenciasFuente = 0;
                            objFuents.vigenciaFuente.forEach(objVigenciasFuentes => {
                                totalVigenciasFuente += parseFloat(procesarNumero(objVigenciasFuentes.Valor));
                            });

                            objFuents.valorTotalFuente = totalVigenciasFuente;
                        }
                    }
                });
            }
            const val = event.target.value.toString();
            const decimalCnt = val.split('.')[1] ? val.split('.')[1].length : 0;
            var total = event.target.value = decimalCnt && decimalCnt > 2 ? event.target.value : parseFloat(val).toFixed(2);
            event.target.value = new Intl.NumberFormat('es-co', { minimumFractionDigits: 2, }).format(total);
        }

        function procesarNumero(value, convertFloat = true) {
            if (!Number(value)) {
                value = limpiaNumero(value);

            } else if (!convertFloat) {
                value = value.replace(",", ".");
            } else {
                value = parseFloat(value.replace(",", "."));
            }

            return value;
        }

        function limpiaNumero(valor) {
            if (valor == "0.00" || valor == "0") return 0;
            if (`${valor.toLocaleString().split(",")[1]}` == 'undefined') return `${valor.toLocaleString().split(",")[0].toString()}`;
            return `${valor.toLocaleString().split(",")[0].toString().replaceAll(".", "")}.${valor.toLocaleString().split(",")[1].toString()}`;
        }

        function validateFormat(event) {

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

            event.target.value = procesarNumero(event.target.value, false);

            var newValue = event.target.value;
            var spiltArray = String(newValue).split("");
            var tamanioPermitido = 15;
            var tamanio = event.target.value.length;
            var decimal = false;
            decimal = event.target.value.toString().includes(".");
            if (decimal) {
                tamanioPermitido = 18;

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

        function ObtenerSeccionCapitulo() {
            const span = document.getElementById('id-capitulo-sgprecursosfuentesfinanciacionsgp');
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

            gestionRecursosSGPServicio.guardarCambiosFirmeSGP(data)
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
                Justificacion: "",
                SeccionCapituloId: vm.seccionCapitulo,
                InstanciaId: $sessionStorage.idInstancia,

            }
            gestionRecursosSGPServicio.eliminarCapitulosModificadosSGP(data)
                .then(function (response) {
                    if (response.data.Exito) {
                        vm.guardadoevent({ nombreComponenteHijo: vm.nombreComponente });
                    }
                    else {
                        utilidades.mensajeError(response.data.Mensaje + " Capitulo Modificado");
                    }
                });
        }

        function eventoValidar() {
            vm.inicializarComponenteCheck();
            gestionRecursosSGPServicio.obtenerErroresProyectoSGP(vm.guiMacroproceso, vm.ProyectoId, vm.idInstancia).then(function (respuesta) {
                vm.notificacionValidacionHijos(respuesta.data);
                var findErrors = respuesta.data.findIndex(p => p.Errores != null);
                vm.visualizarAlerta((findErrors != -1))
            });

        }

        /* ------------------------ Validaciones ---------------------------------*/

        vm.notificacionValidacionPadre = function (errores) {
            vm.limpiarErrores();
            var isValid = true;
            if (errores != undefined) {
                var erroresRelacionconlapl = errores.find(p => (p.Seccion + p.Capitulo) == vm.nombreComponente);
                if (erroresRelacionconlapl != undefined) {
                    var erroresJson = erroresRelacionconlapl.Errores == "" ? [] : JSON.parse(erroresRelacionconlapl.Errores);
                    isValid = (erroresJson == null || erroresJson.length == 0);
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
                                    case "COST003":
                                        vm.validarCOST003(p.Descripcion);
                                        break;
                                    default:
                                        vm.limpiarErrores();
                                    // code block
                                }

                            }
                        });
                    }



                }
                var erroresRelacionconlapl2 = errores.find(p => (p.Seccion + p.Capitulo) == 'recursoscostosdelasacti');
                if (erroresRelacionconlapl2 != undefined) {
                    var erroresJson = erroresRelacionconlapl2.Errores == "" ? [] : JSON.parse(erroresRelacionconlapl2.Errores);
                    isValid = (erroresJson == null || erroresJson.length == 0);
                    if (!isValid) {
                        erroresJson["recursoscostosdelasacti"].forEach(p => {
                            if (vm.errores[p.Error] != undefined) {
                                switch (p.Error) {
                                    case "AFFR002":
                                        vm.validarAFFR002(p.Descripcion);
                                        break;
                                    case "AFFR003":
                                        vm.validarAFFR003(p.Descripcion);
                                        break;
                                    case "COST003":
                                        vm.validarCOST003(p.Descripcion);
                                        break;
                                    default:
                                        vm.limpiarErrores();
                                    // code block
                                }

                            }
                        });

                    }
                }
                vm.notificacionestado({ nombreComponente: vm.nombreComponente, esValido: isValid });

            }
        };

        vm.limpiarErrores = function () {

            var validacionffr2 = document.getElementById(vm.nombreComponente + "-validacionFuentesErrores2-error");
            var ValidacionFFR2Error = document.getElementById(vm.nombreComponente + "-validacionFuentesErrores2-error-mns");
            if (validacionffr2 != undefined) {
                if (ValidacionFFR2Error != undefined) {
                    ValidacionFFR2Error.innerHTML = "";
                    validacionffr2.classList.add('hidden');
                }
            }

            var validacionffr3 = document.getElementById(vm.nombreComponente + "-validacionFuentesErrores3-error");
            var ValidacionFFR3Error = document.getElementById(vm.nombreComponente + "-validacionFuentesErrores3-error-mns");
            if (validacionffr3 != undefined) {
                if (ValidacionFFR3Error != undefined) {
                    ValidacionFFR3Error.innerHTML = "";
                    validacionffr3.classList.add('hidden');
                }
            }

        }

        vm.validarAFFR001 = function (errores) {

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

        vm.validarAFFR003 = function (errores) {
            var ValidacionFFR3 = document.getElementById(vm.nombreComponente + "-validacionFuentesErrores3-error");
            if (ValidacionFFR3 != undefined) {
                var ValidacionFFR3Error = document.getElementById(vm.nombreComponente + "-validacionFuentesErrores3-error-mns");
                if (ValidacionFFR3Error != undefined) {
                    ValidacionFFR3Error.innerHTML = '<span>' + errores + "</span>";
                    ValidacionFFR3.classList.remove('hidden');
                }
            }
        }

        vm.validarCOST003 = function (errores) {
            var ValidacionFFR3 = document.getElementById(vm.nombreComponente + "-validacionFuentesErrores3-error");
            if (ValidacionFFR3 != undefined) {
                var ValidacionFFR3Error = document.getElementById(vm.nombreComponente + "-validacionFuentesErrores3-error-mns");
                if (ValidacionFFR3Error != undefined) {
                    ValidacionFFR3Error.innerHTML = '<span>' + errores + "</span>";
                    ValidacionFFR3.classList.remove('hidden');
                }
            }
        }

        vm.errores = {
            'AFFR001': vm.validarAFFR001,
            'AFFR002': vm.validarAFFR002,
            'AFFR003': vm.validarAFFR003,
            'COST003': vm.validarAFFR003,
        }
    }

    angular.module('backbone').component('fuentesSgp', {
        templateUrl: "src/app/formulario/ventanas/SGP/gestionRecursosSGP/componentes/recursos/fuentes/fuentesSgp.html",
        controller: fuentesSgpController,
        controllerAs: "vm",
        bindings: {
            callback: '&',
            notificacionvalidacion: '&',
            notificacionestado: '&',
            guardadocomponent: '&',
            guardadoevent: '&',
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
        });

})();