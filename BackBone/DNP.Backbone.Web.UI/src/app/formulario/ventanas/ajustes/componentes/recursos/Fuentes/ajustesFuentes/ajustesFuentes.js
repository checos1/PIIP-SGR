(function() {
    'use strict';

    ajustesFuentesController.$inject = ['$scope', 'recursosAjustesServicio', '$sessionStorage', '$uibModal', 'utilidades',
        'constantesBackbone', '$timeout', 'resumenFuentesServicio',  'justificacionCambiosServicio'
    ];

    function ajustesFuentesController(
        $scope,
        recursosAjustesServicio,
        $sessionStorage,
        $uibModal,
        utilidades,
        constantesBackbone,
        $timeout,
        resumenFuentesServicio,
        justificacionCambiosServicio
    ) {
        var listaFuentesBase = [];

        var vm = this;
        vm.init = init;
        vm.nombreComponente = "recursosfuentesdefinanc";

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

            vm.obtenerFuentes(vm.BPIN);
            vm.esAjuste = $sessionStorage.esAjuste;
           // ObtenerSeccionCapitulo();
        }

        vm.notificacionCambiosCapitulos = function (nombreCapituloHijo) {
            if (nombreCapituloHijo == 'datosgeneraleshorizonte') {
                vm.init();
                vm.recargaGuardado();
                vm.recargaGuardadoCostos();
            }
        }

        vm.refrescarResumenCostos = function () {

            vm.recargaGuardado();

            return resumenFuentesServicio.obtenerResumenFuentesFinanciacion(vm.BPIN, vm.idUsuario, vm.idInstancia).then(
                function (resp) {
                    if (resp.data != null && resp.data != "") {
                        vm.validacionGuardado();
                    }
                }
            );
        }

        vm.notificacionValidacionHijo = function(handler) {
            vm.notificacionErrores = handler;
        }

        vm.validacionGuardadoHijo = function(handler) {
            vm.validacionGuardado = handler;
        }
        vm.recargaresumen = function(handler) {
            vm.recargaGuardado = handler;
        }
        vm.recargaresumencostos = function (handler) {
            vm.recargaGuardadoCostos = handler;
        }

        vm.obtenerFuentes = function(bpin) {

            var idInstancia = $sessionStorage.idNivel;
            var idAccion = $sessionStorage.idNivel;
            var idFormulario = $sessionStorage.idNivel;
            var idUsuario = $sessionStorage.usuario.permisos.IdUsuarioDNP;

            //consultarDatosCofinanciador();

            return recursosAjustesServicio.obtenerFuentesFinanciacionVigenciaProyecto($sessionStorage.idInstancia, usuarioDNP, idInstancia).then(
                function(respuesta) {
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
                                                    Valor: arregloDatosGenerales[ls].Confinanciadores[cf].VigenciaCofinanciador[vc].Valor == null ? 0 : arregloDatosGenerales[ls].Confinanciadores[cf].VigenciaCofinanciador[vc].Valor,
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
                                            codigoCofinanciador: vm.getNombreReducido(arregloDatosGenerales[ls].Confinanciadores[cf].CodigoCofinanciador,60),
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
                                                Valor: arregloDatosGenerales[ls].Vigencia[vf].Valor == null ? 0 : arregloDatosGenerales[ls].Vigencia[vf].Valor,
                                                ValorOriginal: arregloDatosGenerales[ls].Vigencia[vf].Valor == null ? 0 : arregloDatosGenerales[ls].Vigencia[vf].Valor,
                                                permiteHabilitar: permiteHabilitar
                                            }
                                            valorTotalFuente += arregloDatosGenerales[ls].Vigencia[vf].Valor == null ? 0 : arregloDatosGenerales[ls].Vigencia[vf].Valor;
                                            listaVigenciaFuente.push(vigenciaFuente);
                                            permiteHabilitar = true;
                                        }
                                    }
                                }

                                var fuente = {
                                    Id: arregloDatosGenerales[ls].FuenteId,
                                    Etapa: arregloDatosGenerales[ls].Etapa,
                                    TipoFinanciador: arregloDatosGenerales[ls].TipoFinanciador,
                                    Financiador: vm.getNombreReducido(arregloDatosGenerales[ls].Financiador, 60),
                                    FinanciadorFull: arregloDatosGenerales[ls].Financiador,
                                    Recurso: arregloDatosGenerales[ls].Recurso,
                                    Sector: arregloDatosGenerales[ls].Sector,
                                    DatosAdicionales: arregloDatosGenerales[ls].Adicional,
                                    FuenteFirme: arregloDatosGenerales[ls].FuenteFirme,
                                    vigenciaFuente: listaVigenciaFuente,
                                    Cofinanciador: listaCofinanciadores,
                                    contieneCofinanciador: contieneCofinanciador,
                                    valorTotalFuente: valorTotalFuente
                                };

                                // PBI38857 
                                //if (!(valorTotalFuente == 0 && valorTotalFuenteCofinanciador == 0) || arregloDatosGenerales[ls].FuenteFirme == 0) {
                                //    listaFuentes.push(fuente);
                                //}
                               
                                listaFuentes.push(fuente);
                               

                                listaCofinanciadores = [];
                                listaVigenciaFuente = [];
                                contieneCofinanciador = false;
                                valorTotalFuente = 0;
                            }
                        }                     
                        vm.listaFuentes = angular.copy(listaFuentes);
                        listaFuentesBase = listaFuentes;
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

        vm.validaEstado = function(esAjuste, tieneCofinanciador, tieneAdicional) {
            if (tieneCofinanciador && !tieneAdicional)
                return true;
        }

        function ConvertirNumero(numero) {
            return new Intl.NumberFormat('es-co', {
                minimumFractionDigits: 2,
            }).format(numero);
        }

        vm.ActivarEditar = function(btnid, tipoTipoFinanciador, etapa) {
            if (vm.disabled == true) {               

                vm.permiteEditar = true;
                vm.ClasesbtnGuardar = "btn btn-default btn-mdHorizonte";
                vm.habilitarFinal = true;
                vm.disabled = false;
                $("#Guardar" + btnid + etapa).attr('disabled', false);              
                //mostrarOcultarCofinanciador(btnid, tipoTipoFinanciador, etapa);
                setTimeout(function() {
                    $(".vigenciaInput").each(function() {
                        var element = $(this);

                        if (element.attr('permiteHabilitar') === 'true') {
                            element.attr("disabled", false);
                        }
                    });
                }, 1000);
            } else {
                vm.permiteEditar = false;
                vm.ClasesbtnGuardar = "btn btnguardarHorizonte";
                vm.habilitarFinal = false;
                vm.disabled = true;
                $("#Guardar" + btnid + etapa).attr('disabled', true);
                asignaValoresOriginales(1);
                $(".vigenciaInput").attr("disabled", true);
            }
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
                templateUrl: 'src/app/formulario/ventanas/ajustes/componentes/recursos/Fuentes/ajustesFuentes/ajustesModalAgregarFuente.html',
                controller: 'ajustesModalAgregarFuenteController',
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

            return recursosAjustesServicio.eliminarFuentesFinanciacionProyecto(fuenteId, usuarioDNP, idInstancia)
                .then(function(response) {
                    let exito = response.data;
                    if (exito) {
                        utilidades.mensajeSuccess("La fuente de financiación fue eliminada con éxito!", false, false, false);
                        guardarCapituloModificado();
                        init();
                        vm.recargaGuardado();
                        return resumenFuentesServicio.obtenerResumenFuentesFinanciacion(vm.BPIN, vm.idUsuario, vm.idInstancia).then(
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
                templateUrl: 'src/app/formulario/ventanas/ajustes/componentes/recursos/Fuentes/ajustesFuentes/ajustesModalAgregarDatosAdicionales.html',
                controller: 'ajustesModalAgregarDatosAdicionalesController',
            }).result.then(function(result) {
                guardarCapituloModificado();
                init();
            }, function(reason) {

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
            //var FaseGuid = constantesBackbone.idEtapaNuevaEjecucion;
            //var Capitulo = 'Fuentes de financiación';
            //var Seccion = 'Recursos';

            //return justificacionCambiosServicio.ObtenerSeccionCapitulo(FaseGuid, Capitulo, Seccion, $sessionStorage.usuario.permisos.IdUsuarioDNP, $sessionStorage.idNivel, $sessionStorage.idFlujoIframe).then(
            //    function (respuesta) {
            //        if (respuesta.data != null && respuesta.data != "") {
            //            vm.seccionCapitulo = respuesta.data;
            //        }
            //    });
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
                Modificado: false,
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

            return recursosAjustesServicio.guardarFuentesFinanciacionRecursosAjustes(parametros, usuarioDNP)
                .then(function(response) {
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

                        vm.recargaGuardado();

                        return resumenFuentesServicio.obtenerResumenFuentesFinanciacion(vm.BPIN, vm.idUsuario, vm.idInstancia).then(
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

        vm.onKeyPress_st = function(e) {
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

            if ((event.keyCode < 48 || event.keyCode > 57) && event.keyCode != 44) {
                event.preventDefault();
            }

            var newValue = event.target.value;
            var spiltArray = String(newValue).split("");
            var tamanioPermitido = 14;
            var tamanio = event.target.value.length;
            var permitido = false;
            permitido = event.target.value.toString().includes(".");
            if (permitido) {
                tamanioPermitido = 18;

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

                    if ((n[1].length == 2 && n[1] > 99) || (n[1].length > 2 && n[1] > 99)) {
                        event.preventDefault();
                    }
                }
            } else {
                if (tamanio > 14 && event.keyCode != 44) {
                    event.preventDefault();
                }
            }

            if (event.keyCode === 44 && tamanio == 14) {
            }
            else {
                if (tamanio > tamanioPermitido || tamanio > 18) {
                    event.preventDefault();
                }
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

            var newValue = event.target.value;
            var spiltArray = String(newValue).split("");
            var tamanioPermitido = 14;
            var tamanio = event.target.value.length;
            var permitido = false;
            permitido = event.target.value.toString().includes(".");
            if (permitido) {
                var indicePunto = event.target.value.toString().indexOf(".");
                var decimales = event.target.value.toString().substring(indicePunto, tamanio).length;
                if (decimales > 2) {
                }
                tamanioPermitido = 18;

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

                    if ((n[1].length == 2 && n[1] > 99) || (n[1].length > 2 && n[1] > 99)) {
                        event.preventDefault();
                    }
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

            event.target.value = parseFloat(event.target.value.replace(",", "."));
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
                                    totalVigenciasCofinanciador += parseFloat(objVigenciasCof.Valor);
                                });

                                objCofin.valorTotalCof = totalVigenciasCofinanciador;
                            });
                        } else {
                            var totalVigenciasFuente = 0;
                            objFuents.vigenciaFuente.forEach(objVigenciasFuentes => {
                                totalVigenciasFuente += parseFloat(objVigenciasFuentes.Valor);
                            });

                            objFuents.valorTotalFuente = totalVigenciasFuente;
                        }
                    }
                });
            }
            //$(event.target).val(function (index, value) {
            //    var valorTotal = parseFloat(limpiaNumero(value)) + parseFloat(limpiaNumero(TotalInversionVigencia));
            //    $("#inp-" + fuenteId + "-" + vigencia + "-" + etapa + "-" + cofinanciadorId).html(formatearNumero(value));
            //    $("#ipnTotal-" + fuenteId).html(formatearNumero(valorTotal));
            //    return formatearNumero(value === '' ? 0 : parseFloat(limpiaNumero(value).substring(0, 12)));
            //});
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

        vm.notificacionValidacionPadre = function(errores) {
            console.log("Validación  - Gestion Recursos Fuentes de financiación");
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

        vm.limpiarErrores = function() {

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

        vm.validarAFFR001 = function(errores) {}

        vm.validarAFFR002 = function(errores) {}

        vm.validarAFFR003 = function(errores) {}

        vm.errores = {
            //'FUE001': vm.validarExitenciaFuentes;
            'AFFR001': vm.validarAFFR001,
            'AFFR002': vm.validarAFFR002,
            'AFFR003': vm.validarAFFR003,
        }
    }

    angular.module('backbone').component('ajustesFuentes', {
        templateUrl: "src/app/formulario/ventanas/ajustes/componentes/recursos/Fuentes/ajustesFuentes/ajustesFuentes.html",
        controller: ajustesFuentesController,
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