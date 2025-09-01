(function () {
    'use strict';

    ajustesFuentesSinTramiteSgpController.$inject = ['$scope', 'recursosAjustesSinTramiteSgpServicio', '$sessionStorage', '$uibModal', 'utilidades',
        'constantesBackbone', '$timeout', 'resumenFuentesSinTramiteSgpServicio', 'justificacionCambiosServicio', 'transversalSgpServicio'
    ];

    function ajustesFuentesSinTramiteSgpController(
        $scope,
        recursosAjustesSinTramiteSgpServicio,
        $sessionStorage,
        $uibModal,
        utilidades,
        constantesBackbone,
        $timeout,
        resumenFuentesSinTramiteSgpServicio,
        justificacionCambiosServicio,
        transversalSgpServicio
    ) {
        var listaFuentesBase = [];

        var vm = this;
        vm.init = init;
        vm.nombreComponente = "recursossgpfuentesdefinancsintramitesgp";

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
        vm.ClasesbtnGuardar = "btn btnguardarHorizonte";
        vm.habilitaBotones = true;
        vm.habilitarFinal = false;
        var currentYear = new Date().getFullYear();
        vm.ff = [];
        vm.validacionGuardado = null;
        vm.recargaGuardado = null;
        vm.recargaGuardadoCostos = null;
        vm.permiteEditar = false;
        vm.seccionCapitulo = null;
        vm.ConvertirNumero = ConvertirNumero;
        vm.TipoAjusteSinTramite = 1;
        vm.fuentesEditar = [];
        vm.soloLectura = false;

        function init() {
            vm.permiteEditar = false;
            vm.model = {
                modulos: {
                    administracion: false,
                    backbone: true
                }
            }
            vm.notificacionvalidacion({ handler: vm.notificacionValidacionPadre, nombreComponente: vm.nombreComponente, esValido: true });
            vm.notificarrefresco({ handler: vm.refrescarResumenCostos, nombreComponente: vm.nombreComponente });
            vm.notificacioncambios({ handler: vm.notificacionCambiosCapitulos, nombreComponente: vm.nombreComponente });
            if ($sessionStorage.ventanaAnterior == "ajustesProyectoConTramiteSgp") {
                vm.TipoAjusteSinTramite = 0;
            }
            vm.obtenerFuentes(vm.BPIN);
            vm.esAjuste = $sessionStorage.esAjuste;
            // ObtenerSeccionCapitulo();
        }

        vm.notificacionCambiosCapitulos = function (nombreCapituloHijo) {
            if (nombreCapituloHijo == 'datosgeneralessgphorizontesintramitesgp') {
                vm.init();
                vm.recargaGuardado();
                vm.recargaGuardadoCostos();
            }
        }

        vm.refrescarResumenCostos = function () {

            vm.recargaGuardado();

            return resumenFuentesSinTramiteSgpServicio.obtenerResumenFuentesFinanciacion(vm.BPIN, vm.idUsuario, vm.idInstancia).then(
                function (resp) {
                    if (resp.data != null && resp.data != "") {
                        vm.validacionGuardado();
                    }
                }
            );
        }

        vm.notificacionValidacionHijo = function (handler) {
            vm.notificacionErrores = handler;
        }

        vm.validacionGuardadoHijo = function (handler) {
            vm.validacionGuardado = handler;
        }
        vm.recargaresumen = function (handler) {
            vm.recargaGuardado = handler;
        }
        vm.recargaresumencostos = function (handler) {
            vm.recargaGuardadoCostos = handler;
        }

        vm.obtenerFuentes = function (bpin) {

            var idInstancia = $sessionStorage.idNivel;
            var idAccion = $sessionStorage.idNivel;
            var idFormulario = $sessionStorage.idNivel;
            var idUsuario = $sessionStorage.usuario.permisos.IdUsuarioDNP;

            //consultarDatosCofinanciador();

            return recursosAjustesSinTramiteSgpServicio.obtenerFuentesFinanciacionVigenciaProyecto($sessionStorage.idInstancia, usuarioDNP, idInstancia).then(
                function (respuesta) {
                    if (respuesta.data != null && respuesta.data != "") {

                        var arreglolistas = jQuery.parseJSON(respuesta.data);
                        var arregloGeneral = jQuery.parseJSON(arreglolistas)
                        var arregloDatosGenerales = arregloGeneral.Datos_Generales;

                        var contieneCofinanciador = false;

                        var listaFuentes = [];
                        var listaCofinanciadores = [];
                        var listaVigenciaCofinanciador = [];
                        var listaVigenciaFuente = [];
                        var valorTotalFuente = 0;
                        var valorTotalFuenteSolicitado = 0;
                        var valorTotalFuenteFirme = 0;
                        var permiteHabilitar = true;
                        var entityTypeProyecto = $sessionStorage.idEntidad;

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
                                                if (vm.TipoAjusteSinTramite == 1) {
                                                    permiteHabilitar = arregloDatosGenerales[ls].Confinanciadores[cf].VigenciaCofinanciador[vc].Vigencia <= currentYear ? false : true;
                                                } else {
                                                    permiteHabilitar = arregloDatosGenerales[ls].Confinanciadores[cf].VigenciaCofinanciador[vc].Vigencia < currentYear ? false : true;
                                                }
                                                var vigenciaCofinanciador = {
                                                    Vigencia: arregloDatosGenerales[ls].Confinanciadores[cf].VigenciaCofinanciador[vc].Vigencia,
                                                    Valor: parseFloat(arregloDatosGenerales[ls].Confinanciadores[cf].VigenciaCofinanciador[vc].Valor == null ? 0 : arregloDatosGenerales[ls].Confinanciadores[cf].VigenciaCofinanciador[vc].Valor).toFixed(2),
                                                    ValorOriginal: arregloDatosGenerales[ls].Confinanciadores[cf].VigenciaCofinanciador[vc].Valor == null ? 0 : arregloDatosGenerales[ls].Confinanciadores[cf].VigenciaCofinanciador[vc].Valor,
                                                    permiteHabilitar: permiteHabilitar
                                                }

                                                valorTotalCofinanciador += arregloDatosGenerales[ls].Confinanciadores[cf].VigenciaCofinanciador[vc].Valor == null ? 0 : arregloDatosGenerales[ls].Confinanciadores[cf].VigenciaCofinanciador[vc].Valor;

                                                listaVigenciaCofinanciador.push(vigenciaCofinanciador);
                                                permiteHabilitar = true;
                                            }
                                        }

                                        var cofinanciador = {
                                            cofinanciadorId: arregloDatosGenerales[ls].Confinanciadores[cf].CofinanciadorId,
                                            tipoCofinanciador: arregloDatosGenerales[ls].Confinanciadores[cf].TipoCofinanciador,
                                            codigo: arregloDatosGenerales[ls].Confinanciadores[cf].Codigo,
                                            codigoCofinanciadorFull: arregloDatosGenerales[ls].Confinanciadores[cf].CodigoCofinanciador,
                                            codigoCofinanciador: vm.getNombreReducido(arregloDatosGenerales[ls].Confinanciadores[cf].CodigoCofinanciador, 60),
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
                                            if (vm.TipoAjusteSinTramite == 1) {
                                                permiteHabilitar = arregloDatosGenerales[ls].Vigencia[vf].Vigencia <= currentYear ? false : true;
                                            } else {
                                                permiteHabilitar = arregloDatosGenerales[ls].Vigencia[vf].Vigencia < currentYear ? false : true;
                                            }
                                            var vigenciaFuente = {
                                                Vigencia: arregloDatosGenerales[ls].Vigencia[vf].Vigencia,
                                                Valor: parseFloat(arregloDatosGenerales[ls].Vigencia[vf].Valor == null ? 0 : arregloDatosGenerales[ls].Vigencia[vf].Valor).toFixed(2),
                                                ValorOriginal: arregloDatosGenerales[ls].Vigencia[vf].Valor == null ? 0 : arregloDatosGenerales[ls].Vigencia[vf].Valor,
                                                ValorSolicitado: arregloDatosGenerales[ls].Vigencia[vf].ValorSolicitado == null ? 0 : arregloDatosGenerales[ls].Vigencia[vf].ValorSolicitado,
                                                ValorFirme: arregloDatosGenerales[ls].Vigencia[vf].ValorFirme == null ? 0 : arregloDatosGenerales[ls].Vigencia[vf].ValorFirme,
                                                permiteHabilitar: permiteHabilitar
                                            }
                                            valorTotalFuente += arregloDatosGenerales[ls].Vigencia[vf].Valor == null ? 0 : arregloDatosGenerales[ls].Vigencia[vf].Valor;
                                            valorTotalFuenteSolicitado += arregloDatosGenerales[ls].Vigencia[vf].ValorSolicitado == null ? 0 : arregloDatosGenerales[ls].Vigencia[vf].ValorSolicitado;
                                            valorTotalFuenteFirme += arregloDatosGenerales[ls].Vigencia[vf].ValorFirme == null ? 0 : arregloDatosGenerales[ls].Vigencia[vf].ValorFirme;
                                            listaVigenciaFuente.push(vigenciaFuente);
                                            permiteHabilitar = true;
                                        }
                                    }
                                }

                                vm.validarfinanciador = function (financiador) {
                                    if (financiador === 'Privadas') return true;
                                    else return false;
                                }

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
                                    Financiador: vm.getNombreReducido(arregloDatosGenerales[ls].Financiador, 60),
                                    FinanciadorFull: arregloDatosGenerales[ls].Financiador,
                                    Recurso: arregloDatosGenerales[ls].Recurso,
                                    Sector: arregloDatosGenerales[ls].Sector,
                                    DatosAdicionales: arregloDatosGenerales[ls].Adicional,
                                    habilitaDatosAdicionales: habilitaDatosAdicionales,
                                    FuenteFirme: arregloDatosGenerales[ls].FuenteFirme,
                                    vigenciaFuente: listaVigenciaFuente,
                                    Cofinanciador: listaCofinanciadores,
                                    contieneCofinanciador: contieneCofinanciador,
                                    valorTotalFuente: valorTotalFuente,
                                    valorTotalFuenteSolicitado: valorTotalFuenteSolicitado,
                                    valorTotalFuenteFirme: valorTotalFuenteFirme
                                };

                                listaFuentes.push(fuente);


                                listaCofinanciadores = [];
                                listaVigenciaFuente = [];
                                contieneCofinanciador = false;
                                valorTotalFuente = 0;
                                valorTotalFuenteSolicitado = 0;
                                valorTotalFuenteFirme = 0;
                            }
                        }
                        vm.listaFuentes = angular.copy(listaFuentes);
                        listaFuentesBase = listaFuentes;
                        vm.soloLectura = $sessionStorage.soloLectura;

                        if (listaFuentes.length == 0) {
                            transversalSgpServicio.notificarCambio({ enableRegionalizacion: false });
                            eliminarCapitulosModificados();
                        }
                        else {
                            const enableRegionalizacion = listaFuentes.some(fuente => fuente.valorTotalFuente > 0);
                            transversalSgpServicio.notificarCambio({ enableRegionalizacion: enableRegionalizacion });
                            guardarCapituloModificado();
                        }
                    }
                }
            );
        }

        vm.getNombreReducido = function (data, maxCaracteres) {
            if (data.length > maxCaracteres) {
                var dataNueva = data.slice(0, maxCaracteres);
                return dataNueva + '...';
            } else return data
        }

        vm.validaEstado = function (esAjuste, tieneCofinanciador, tieneAdicional) {
            if (tieneCofinanciador && !tieneAdicional)
                return true;
        }

        function ConvertirNumero(numero) {
            return new Intl.NumberFormat('es-co', {
                minimumFractionDigits: 2,
            }).format(numero);
        }
        function OkCancelar() {
            setTimeout(function () {
                utilidades.mensajeSuccess("", false, false, false, "La edición ha sido cancelada con éxito.");
            }, 500);
        }
        vm.ActivarEditar = function (btnid, tipoTipoFinanciador, etapa, cofinanciadores, vigenciaFuente, datosAdicionales) {
            var existeEdicion = vm.fuentesEditar.length > 0;
            if (!existeEdicion || $("#Editar" + btnid).html() == 'EDITAR') {
                vm.fuentesEditar.push({ fuenteId: btnid, etapa, cofinanciadores, vigenciaFuente, datosAdicionales });
            } else {
                var fuente = vm.fuentesEditar.shift();
                utilidades.mensajeWarning("Los posibles datos que haya diligenciado se perderan. ¿Está seguro de continuar?", function funcionContinuar() {
                    OkCancelar();                   
                    cancelarEdicion(fuente.fuenteId, etapa);
                    habilitarEdicionFuentesVigencias(fuente.datosAdicionales, fuente.fuenteId, fuente.etapa, fuente.cofinanciadores, fuente.vigenciaFuente, false);

                }, function funcionCancelar(reason) {
                   
                    return;
                }, null, null, "Advertencia");

                if (fuente.fuenteId === btnid) return;
                vm.fuentesEditar.push({ fuenteId: btnid, etapa, cofinanciadores, vigenciaFuente, datosAdicionales });
                vm.disabled = true;
             
            }
            if (vm.disabled == true) {

                vm.permiteEditar = true;
                vm.ClasesbtnGuardar = "btn btn-default btn-mdHorizonte";
                vm.habilitarFinal = true;
                vm.disabled = false;
                vm.cofinanciadores = cofinanciadores;
                $("#Guardar" + btnid + etapa).attr('disabled', false);
                $("#Editar" + btnid).html('CANCELAR');
                $("#Editar" + btnid).attr('uib-tooltip', 'CANCELAR');

                habilitarEdicionFuentesVigencias(datosAdicionales, btnid, etapa, cofinanciadores, vigenciaFuente, true);
            } else {

                utilidades.mensajeWarning("Los posibles datos que haya diligenciado se perderan. ¿Está seguro de continuar?", function funcionContinuar() {
                    OkCancelar();
                    cancelarEdicion(btnid, etapa);

                }, function funcionCancelar(reason) {
                   
                    return;
                }, null, null, "Advertencia");


            }
        }

        function cancelarEdicion(btnid, etapa) {
            vm.permiteEditar = false;
            vm.ClasesbtnGuardar = "btn btnguardarHorizonte";
            vm.habilitarFinal = false;
            vm.disabled = true;
            $("#Guardar" + btnid + etapa).attr('disabled', true);
            asignaValoresOriginales(1);
            $(".vigenciaInput").attr("disabled", true);
            $("#Editar" + btnid).html('EDITAR');
            $("#Editar" + btnid).attr('uib-tooltip', 'EDITAR');
        }

        function habilitarEdicionFuentesVigencias(datosAdicionales, btnid, etapa, cofinanciadores, vigenciaFuente, isEdition) {
            setTimeout(function () {
                if (datosAdicionales == 1) {
                    cofinanciadores.forEach(cof => {
                        cof.listaVigenciasCofinanciador.forEach(vig => {
                            if (vig.permiteHabilitar === true && isEdition) {
                                $(`#inp-${btnid}-${vig.Vigencia}-${etapa}-${cof.cofinanciadorId}`).prop("disabled", false);
                            } else {
                                $(`#inp-${btnid}-${vig.Vigencia}-${etapa}-${cof.cofinanciadorId}`).prop("disabled", true);
                            }
                        })
                    });
                } else {
                    vigenciaFuente.forEach(vig => {
                        if (vig.permiteHabilitar === true && isEdition) {
                            $(`#inp-${btnid}-${vig.Vigencia}-${etapa}-`).prop("disabled", false);
                        } else {
                            $(`#inp-${btnid}-${vig.Vigencia}-${etapa}-`).prop("disabled", true);
                        }
                    });
                }
            }, 500);
        }

        function asignaValoresOriginales(proceso) {
            var total = 0;
            for (var ls = 0; ls < vm.listaFuentes.length; ls++) {
                if (vm.listaFuentes[ls].contieneCofinanciador) {
                    for (var lcf = 0; lcf < vm.listaFuentes[ls].Cofinanciador.length; lcf++) {
                        for (var lcfv = 0; lcfv < vm.listaFuentes[ls].Cofinanciador[lcf].listaVigenciasCofinanciador.length; lcfv++) {
                            if (proceso === 1) {
                                vm.listaFuentes[ls].Cofinanciador[lcf].listaVigenciasCofinanciador[lcfv].Valor = vm.listaFuentes[ls].Cofinanciador[lcf].listaVigenciasCofinanciador[lcfv].ValorOriginal;
                                total += parseFloat(vm.listaFuentes[ls].Cofinanciador[lcf].listaVigenciasCofinanciador[lcfv].Valor);
                            } else {
                                vm.listaFuentes[ls].Cofinanciador[lcf].listaVigenciasCofinanciador[lcfv].ValorOriginal = vm.listaFuentes[ls].Cofinanciador[lcf].listaVigenciasCofinanciador[lcfv].Valor;
                                total += parseFloat(vm.listaFuentes[ls].Cofinanciador[lcf].listaVigenciasCofinanciador[lcfv].ValorOriginal);
                            }
                        }
                        vm.listaFuentes[ls].Cofinanciador[lcf].valorTotalCof = total;
                    }
                } else {
                    for (var lfv = 0; lfv < vm.listaFuentes[ls].vigenciaFuente.length; lfv++) {
                        if (proceso === 1) {
                            vm.listaFuentes[ls].vigenciaFuente[lfv].Valor = vm.listaFuentes[ls].vigenciaFuente[lfv].ValorOriginal;
                            total += parseFloat(vm.listaFuentes[ls].vigenciaFuente[lfv].Valor);
                        } else {
                            vm.listaFuentes[ls].vigenciaFuente[lfv].ValorOriginal = vm.listaFuentes[ls].vigenciaFuente[lfv].Valor;
                            total += parseFloat(vm.listaFuentes[ls].vigenciaFuente[lfv].ValorOriginal);
                        }
                    }
                    vm.listaFuentes[ls].valorTotalFuente = total;
                }
                total = 0;
            }
        };

        function abrirModalAgregarFuente() {
            let modalInstance = $uibModal.open({
                animation: $scope.animationsEnabled,
                templateUrl: 'src/app/formulario/ventanas/SGP/ajustesProyectoSGP/componentes/recursosSGP/Fuentes/ajustesFuentes/ajustesModalAgregarFuenteSinTramiteSgp.html',
                controller: 'ajustesModalAgregarFuenteSinTramiteSgpController',
                controllerAs: "vm",
                size: 'lg',
                openedClass: "entidad-modal-adherencia",
            });
            modalInstance.result.then(data => {
                if (data != null) {
                    guardarCapituloModificado();
                    init();
                } //else {
                //    toastr.error("Ocurrió un error al consultar el idAplicacion");
                //}
            });
        }

        function eliminarFuente(fuenteId) {

            var idInstancia = $sessionStorage.idNivel;

            utilidades.mensajeWarning("Los capitulos que pueden afectarse: Regionalización, Focalización, Ajustes vigencias futuras, ¿desea continuar?", function funcionContinuar() {

                return recursosAjustesSinTramiteSgpServicio.eliminarFuentesFinanciacionProyecto(fuenteId, usuarioDNP, idInstancia)
                    .then(function (response) {
                        let exito = response.data;
                        if (exito) {
                            utilidades.mensajeSuccess("La fuente de financiación fue eliminada con éxito!", false, false, false);
                            guardarCapituloModificado();
                            init();
                            vm.recargaGuardado();
                            return resumenFuentesSinTramiteSgpServicio.obtenerResumenFuentesFinanciacion(vm.BPIN, vm.idUsuario, vm.idInstancia).then(
                                function (resp) {
                                    if (resp.data != null && resp.data != "") {
                                        vm.validacionGuardado();
                                    }
                                }
                            );
                        } else {
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
            }, function funcionCancelar(reason) {
                console.log("reason", reason);
            },
                "Aceptar",
                "Cancelar",
                "Es posible que la fuente de financiación a eliminar tenga información en otros capitulos");
        }

        function abrirModalAgregarDatosAdicionales(idFuente) {
            $sessionStorage.idFuente = idFuente;
            //$sessionStorage.proyectoId = $sessionStorage.ProyectoId;
            vm.idFuente = idFuente;


            $uibModal.open({
                templateUrl: 'src/app/formulario/ventanas/SGP/ajustesProyectoSGP/componentes/recursosSGP/Fuentes/ajustesFuentes/ajustesModalAgregarDatosAdicionalesSinTramiteSgp.html',
                controller: 'ajustesModalAgregarDatosAdicionalesSinTramiteSgpController',
            }).result.then(function (result) {
                guardarCapituloModificado();
                init();
            }, function (reason) {

            }), err => {
                toastr.error("Ocurrió un error al consultar el idAplicacion");
            };
        }

        function mostrarOcultarCofinanciador(fuenteId, cofinanciadorId, etapa) {
            vm.habilitaBotones = true;
            var variable = $("#ico" + fuenteId + "-" + etapa)[0].innerText;
            var detail = $("#detail-" + fuenteId + "-" + etapa);
            if (variable === "+") {
                $("#ico" + fuenteId + "-" + etapa).html('-');
                if (detail != undefined) detail[0].classList.remove("hidden");
            } else {
                $("#ico" + fuenteId + "-" + etapa).html('+');
                if (detail != undefined) detail[0].classList.add("hidden");
            }
        }

        function ObtenerSeccionCapitulo() {
            const span = document.getElementById('id-capitulo-' + vm.nombreComponente);
            vm.seccionCapitulo = span.textContent;

        }

        function guardarCapituloModificado() {
            ObtenerSeccionCapitulo();
            var data = {
                ProyectoId: $sessionStorage.InstanciaSeleccionada.ProyectoId,
                Justificacion: "",
                //SeccionCapituloId: vm.seccionCapitulo.SeccionCapituloId,
                SeccionCapituloId: vm.seccionCapitulo,
                InstanciaId: $sessionStorage.InstanciaSeleccionada.IdInstancia,
                Modificado: true,
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

        function actualizarVigencia(etapa, listaFuentes, FuenteId) {

            vm.ff = [];

            listaFuentes.forEach(ff => {
                if (ff.Id == FuenteId && ff.Etapa == etapa) {
                    if (ff.DatosAdicionales == 0) {
                        ff.vigenciaFuente.forEach(vf => {
                            vf.Valor = vf.Valor;
                        });
                    } else {
                        ff.Cofinanciador.forEach(vcf => {
                            vcf.listaVigenciasCofinanciador.forEach(lvc => {
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

            return recursosAjustesSinTramiteSgpServicio.guardarFuentesFinanciacionRecursosAjustesSgp(parametros, usuarioDNP)
                .then(function (response) {
                    var respuesta = jQuery.parseJSON(response.data)

                    if (respuesta.StatusCode == "200") {
                        guardarCapituloModificado();
                        utilidades.mensajeSuccess("Se han adicionado lineas de información en la parte inferior de la tabla 'Modificar Fuentes'", false, false, false);
                        //init();
                        asignaValoresOriginales(2);
                        vm.permiteEditar = false;
                        vm.habilitarFinal = false;
                        vm.disabled = true;
                        vm.ClasesbtnGuardar = "btn btnguardarHorizonte";

                        $("#Editar" + FuenteId).html('EDITAR');
                        $("#Editar" + FuenteId).attr('uib-tooltip', 'EDITAR');

                        vm.recargaGuardado();

                        transversalSgpServicio.notificarCambio({ actualizarFuentesAjusteSinTramite: true });

                        return resumenFuentesSinTramiteSgpServicio.obtenerResumenFuentesFinanciacion(vm.BPIN, vm.idUsuario, vm.idInstancia).then(
                            function (resp) {
                                if (resp.data != null && resp.data != "") {
                                    vm.validacionGuardado();
                                }
                            }
                        );
                    } else {
                        var ValidacionGuardar = document.getElementById(vm.nombreComponente + "-validacionguardar-error");
                        if (ValidacionGuardar != undefined) {
                            var ValidacionFFR1Error = document.getElementById(vm.nombreComponente + "-validacionguardar-error-mns");
                            if (ValidacionFFR1Error != undefined) {
                                ValidacionFFR1Error.innerHTML = '<span>' + errores + "</span>";
                                ValidacionFFR1.classList.remove('hidden');
                            }
                        }
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

        vm.verNombreCof = function (fuenteId, etapa, cofinanciadorId, tipoPropio, otroTipo) {
            var nameCof = document.getElementById('cof-' + fuenteId + '-' + etapa + '-' + cofinanciadorId + '-' + otroTipo);
            var nameCofActivo = document.getElementById('cof-' + fuenteId + '-' + etapa + '-' + cofinanciadorId + '-' + tipoPropio);
            if (nameCof != undefined && nameCofActivo != undefined) {
                nameCofActivo.classList.add('hidden');
                nameCof.classList.remove('hidden');
            }
        }


        function formatearNumero(n, sep, decimals) {
            sep = sep || ","; // Default to period as decimal separator
            decimals = decimals || 2; // Default to 2 decimals

            var unidad = n.toLocaleString().split(sep)[0];
            var decimal = n.toFixed == undefined || n.toFixed(decimals).split(".")[1] == undefined ? 0 : n.toFixed(decimals).split(".")[1];

            return unidad +
                sep +
                decimal;
        }

        vm.onKeyPress_st = function (e) {
            var contador = e.currentTarget.value.length + 1;
            if (contador <= 24) {
                const charCode = e.which ? e.which : e.keyCode;

                if (charCode !== 44 && charCode > 31 && (charCode < 48 || charCode > 57)) {
                    e.preventDefault();
                }

                if (charCode == 13) {
                    e.preventDefault();
                }
            } else {
                e.preventDefault();
            }
        }

        vm.validateFormat = function (event) {
            if ((event.keyCode < 48 || event.keyCode > 57) && event.keyCode != 44 && event.keyCode != 46) {
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
                //event.preventDefault();
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
            var total = event.target.value = decimalCnt && decimalCnt > 2 ? parseFloat(event.target.value).toFixed(2) : parseFloat(val).toFixed(2);
            event.target.value = new Intl.NumberFormat('es-co', { minimumFractionDigits: 2, }).format(total);
        }

        function procesarNumero(value, convertFloat = true) {
            if (!Number(value)) {
                value = limpiaNumero1(value);

            } else if (!convertFloat) {
                value = value.replace(",", ".");
            } else {
                value = parseFloat(value.replace(",", "."));
            }

            return value;
        }

        function limpiaNumero1(valor) {
            if (valor == "0.00" || valor == "0") return 0;
            return `${valor.toLocaleString().split(",")[0].toString().replaceAll(".", "")}.${valor.toLocaleString().split(",")[1].toString()}`;
        }

        function limpiaNumero(valor) {
            return valor.toLocaleString().split(",")[0].toString().replaceAll(".", "");
        }

        function validarEntero(valor) {
            //intento convertir a entero. 
            //si era un entero no le afecta, si no lo era lo intenta convertir 
            valor = parseInt(limpiaNumero(valor))

            //Compruebo si es un valor numérico 
            if (isNaN(valor)) {
                //entonces (no es numero) devuelvo el valor cadena vacia 
                return ""
            } else {
                //En caso contrario (Si era un número) devuelvo el valor 
                return valor
            }
        }

        /* ------------------------ Validaciones ---------------------------------*/

        vm.notificacionValidacionPadre = function (errores) {
            console.log("Validación  - Recursos Fuentes de financiación");
            vm.limpiarErrores();
            var isValid = true;
            if (errores != undefined) {
                var erroresRelacionconlapl = errores.find(p => (p.Seccion + p.Capitulo) == vm.nombreComponente);
                if (erroresRelacionconlapl != undefined) {
                    var erroresJson = erroresRelacionconlapl.Errores == "" ? [] : JSON.parse(erroresRelacionconlapl.Errores);
                    if (vm.notificacionErrores != null && erroresJson != null) vm.notificacionErrores(erroresJson[vm.nombreComponente]);

                    var isValid = (erroresJson == null || erroresJson.length == 0);
                    if (!isValid) {
                        erroresJson[vm.nombreComponente].forEach(p => {
                            if (vm.errores[p.Error] != undefined) vm.errores[p.Error](p.Descripcion);
                        });
                    }
                }
            }

            vm.notificacionestado({ nombreComponente: vm.nombreComponente, esValido: isValid });
        };

        vm.limpiarErrores = function () {

            var ErrorAFS = document.getElementById("ErrorAFST001");
            var Errormsn = document.getElementById("ErroAFST001Msg");
            if (ErrorAFS != undefined) {
                if (Errormsn != undefined) {
                    Errormsn.innerHTML = "";
                    ErrorAFS.classList.add('hidden');
                }
            }

            ErrorAFS = document.getElementById("ErrorAFST002");
            Errormsn = document.getElementById("ErroAFST002Msg");
            if (ErrorAFS != undefined) {
                if (Errormsn != undefined) {
                    Errormsn.innerHTML = "";
                    ErrorAFS.classList.add('hidden');
                }
            }

            ErrorAFS = document.getElementById("ErrorAFST003");
            Errormsn = document.getElementById("ErroAFST003Msg");
            if (ErrorAFS != undefined) {
                if (Errormsn != undefined) {
                    Errormsn.innerHTML = "";
                    ErrorAFS.classList.add('hidden');
                }
            }

            ErrorAFS = document.getElementById("ErrorAFST004");
            Errormsn = document.getElementById("ErroAFST004Msg");
            if (ErrorAFS != undefined) {
                if (Errormsn != undefined) {
                    Errormsn.innerHTML = "";
                    ErrorAFS.classList.add('hidden');
                }
            }
        }

        vm.validarAFST001 = function (errores) {
            var ErrorAFS = document.getElementById("ErrorAFST001");
            var Errormsn = document.getElementById("ErroAFST001Msg");

            if (ErrorAFS != undefined) {
                if (Errormsn != undefined) {
                    Errormsn.innerHTML = '<span>' + errores + "</span>";
                    ErrorAFS.classList.remove('hidden');
                }
            }
        }

        vm.validarAFST002 = function (errores) {
            var ErrorAFS = document.getElementById("ErrorAFST002");
            var Errormsn = document.getElementById("ErroAFST002Msg");

            if (ErrorAFS != undefined) {
                if (Errormsn != undefined) {
                    Errormsn.innerHTML = '<span>' + errores + "</span>";
                    ErrorAFS.classList.remove('hidden');
                }
            }
        }

        vm.validarAFST003 = function (errores) {
            var ErrorAFS = document.getElementById("ErrorAFST003");
            var Errormsn = document.getElementById("ErroAFST003Msg");

            if (ErrorAFS != undefined) {
                if (Errormsn != undefined) {
                    Errormsn.innerHTML = '<span>' + errores + "</span>";
                    ErrorAFS.classList.remove('hidden');
                }
            }
        }

        vm.validarAFST004 = function (errores) {
            var ErrorAFS = document.getElementById("ErrorAFST004");
            var Errormsn = document.getElementById("ErroAFST004Msg");

            if (ErrorAFS != undefined) {
                if (Errormsn != undefined) {
                    Errormsn.innerHTML = '<span>' + errores + "</span>";
                    ErrorAFS.classList.remove('hidden');
                }
            }
        }

        vm.errores = {
            'AFST001': vm.validarAFST001,
            'AFST002': vm.validarAFST002,
            'AFST003': vm.validarAFST003,
            'AFST004': vm.validarAFST004
        }
    }

    angular.module('backbone').component('ajustesFuentesSinTramiteSgp', {
        templateUrl: "src/app/formulario/ventanas/SGP/ajustesProyectoSGP/componentes/recursosSGP/Fuentes/ajustesFuentes/ajustesFuentesSinTramiteSgp.html",
        controller: ajustesFuentesSinTramiteSgpController,
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