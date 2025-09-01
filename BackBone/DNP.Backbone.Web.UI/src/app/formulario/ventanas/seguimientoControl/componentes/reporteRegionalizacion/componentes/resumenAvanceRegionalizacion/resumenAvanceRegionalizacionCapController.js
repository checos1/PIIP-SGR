(function () {
    'use strict';

    resumenAvanceRegionalizacionCapController.$inject = [
        '$scope',
        '$sessionStorage',
        '$uibModal',
        'utilidades',
        'resumenAvanceRegionalizacionCapServicio',
        'justificacionCambiosServicio',
        'utilsValidacionSeccionCapitulosServicio'
    ];

    function resumenAvanceRegionalizacionCapController(
        $scope,
        $sessionStorage,
        $uibModal,
        utilidades,
        resumenAvanceRegionalizacionCapServicio,
        justificacionCambiosServicio,
        utilsValidacionSeccionCapitulosServicio

    ) {
        var vm = this;
        vm.lang = "es";
        vm.nombreComponente = "regionalizacionresumenregionaliza";
        vm.disabled = true;
        vm.habilitaBotones = true;
        vm.habilitarFinal = false;
        vm.registrosExcel = null;
        vm.listaDatosExcel = [];
        vm.fuentes = [];
        vm.Objetivos = [];
        vm.resumenrecursos = [];
        vm.verDatosExcel = false;
        vm.ConvertirNumero = ConvertirNumero;
        vm.ConvertirNumero4 = ConvertirNumero4;
        vm.disabled = false;
        vm.disabled2 = false;
        //vm.Guardar = Guardar;
        vm.Cancelar = Cancelar;
        vm.Editar = Editar;
        vm.copiaIndicador;
        vm.actualizaValoresAnteriores;
        vm.HabilitaEditarIndicador = false;
        vm.HabilitaGuardarIndicador = false;
        vm.soloLectura = $sessionStorage.soloLectura;
        vm.EsActualizacion = false;

        vm.init = function () {
            vm.inicializarComponenteCheck();
            vm.notificacioncambios({ handler: vm.notificacionCambiosCapitulos, nombreComponente: vm.nombreComponente });
            vm.notificacionvalidacion({ handler: vm.notificacionValidacionPadre, nombreComponente: vm.nombreComponente, esValido: true });

            vm.consultarResumenAvanceMetaProducto();
        };

        $scope.$watch('vm.refreshregionalizacion', function () {
            if (vm.refreshregionalizacion === "true") {

                vm.consultarResumenAvanceMetaProducto();

                vm.refreshregionalizacion = "false";
            }

        });

        /*------------------------------AQUI INICIA PARA LOGICA DEL NEGOCIO ------------------------------------------------------*/

        function Cancelar(localizacion, fuentes) {

            utilidades.mensajeWarning("¿Está seguro de continuar?", function funcionContinuar() {

                vm.HabilitaEditarIndicador = false;
                vm.HabilitaGuardarIndicador = false;

                $(".iconoErrorDNP").fadeOut();
                $(".iconoErrorDNPError").fadeOut();

            }, function funcionCancelar(reason) {
                //poner aquí q pasa cuando cancela
            }, null, null, "Los posibles datos que haya diligenciado en la tabla se perderán.");
        }

        function Editar(indicador) {

            vm.HabilitaEditarIndicador = true;
            vm.HabilitaGuardarIndicador = true;
        }

        vm.consultarResumenAvanceMetaProducto = function () {
            const objetoParametros = {
                instanciaId: $sessionStorage.idInstanciaIframe,
                proyectoId: $sessionStorage.proyectoId,
                codigoBpin: $sessionStorage.idObjetoNegocio
            };
            vm.listaDatosExcel = [];

            resumenAvanceRegionalizacionCapServicio.consultarResumenAvanceRegionalizacion(objetoParametros).then(
                function (response) {
                    if (response.data && (response.statusText === "OK" || response.status === 200)) {

                        var arreglolistas = jQuery.parseJSON(response.data);
                        vm.listaDatos = jQuery.parseJSON(arreglolistas);

                        $scope.datosResumenavanceRegionalizacion = vm.listaDatos;


                    } else {
                        swal('', "Error al obtener valores del avance de regionalización", 'error');
                    }
                },
                function (error) {
                    if (error) {
                        utilidades.mensajeError(error);
                    }
                }
            );
        };

        vm.abrirMensaje = function (vigencia, mes, ObservacionRecursos, ObservacionMetas) {
            var observacionRecursos = ObservacionRecursos == null || ObservacionRecursos.length == 1 ? "Sin observación" : ObservacionRecursos;
            var observacionMetas = ObservacionMetas == null || ObservacionMetas.length == 1 ? "Sin observación" : ObservacionMetas;
            var texto = " ";

            utilidades.mensajeInformacionN("", null, "<span class='anttituhori' > Observaciones </span><br /> <span class='anttituhori'>" + vigencia + "-" + mes + "</span> <br /><br /> <span class='anttituhori'>Recursos:</span> <br /><p>" + observacionRecursos + "</p><br /><span class='anttituhori'>Metas:</span> <br /><p>" + observacionMetas + "<p>", texto);
        };

        function ObtenerSeccionCapitulo() {
            const span = document.getElementById('id-capitulo-' + vm.nombreComponente);
            vm.seccionCapitulo = span.textContent;
        }

        function guardarCapituloModificado() {
            ObtenerSeccionCapitulo();
            var data = {
                ProyectoId: $sessionStorage.InstanciaSeleccionada.ProyectoId,
                Justificacion: "",
                SeccionCapituloId: vm.seccionCapitulo,
                InstanciaId: $sessionStorage.InstanciaSeleccionada.IdInstancia,
                Modificado: false
            };
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

        function ConvertirNumero(numero) {
            return new Intl.NumberFormat('es-co', {
                minimumFractionDigits: 2,
            }).format(numero);
        }

        function ConvertirNumero4(numero) {
            return new Intl.NumberFormat('es-co', {
                minimumFractionDigits: 4,
            }).format(numero);
        }

        vm.abrilNivel = function (idElement) {
            console.log(idElement);
            var elMas = document.getElementById(idElement + '-mas');
            var elMenos = document.getElementById(idElement + '-menos');

            if (elMas != null && elMenos != null) {
                if (elMas.classList.contains('hidden')) {
                    elMenos.classList.add('hidden');
                    elMas.classList.remove('hidden');
                } else {
                    elMenos.classList.remove('hidden');
                    elMas.classList.add('hidden');
                }
            }
        }

        vm.verNombreCompleto = function (idElement, indexElement) {
            var elValidacion = document.getElementById(idElement + indexElement + '-val');
            var elCorto = document.getElementById(idElement + indexElement + '-min');
            var elCompleto = document.getElementById(idElement + indexElement + '-max');

            if (elCompleto.classList.contains('hidden')) {
                elValidacion.innerHTML = 'VER MENOS';
                elCorto.classList.add('hidden');
                elCompleto.classList.remove('hidden');
            } else {
                elValidacion.innerHTML = 'VER MÁS';
                elCorto.classList.remove('hidden');
                elCompleto.classList.add('hidden');
            }
        };

        vm.abrirMensajeQueEsEsto1 = function (mensaje) {
            utilidades.mensajeInformacionN("", null, "<span class='anttituhori' > ¿Qué es esto? </span><br /> <br /> <span class='tituhori' >Resumen recursos y metas</span>", mensaje);
        };

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
                    var n2 = n[1].slice(0, 4);
                    newValue = [n[0], n2].join(".");
                    if (spiltArray.length === 0) return;
                    if (spiltArray.length === 1 && (spiltArray[0] == '-' || spiltArray[0] === '.')) return;
                    if (spiltArray.length === 2 && newValue === '-.') return;

                    if (n[1].length > 4) {
                        tamanioPermitido = n[0].length + 4;
                        event.target.value = n[0] + '.' + n[1].slice(0, 4);
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
        };

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
                if (decimales > 4) {
                }
                tamanioPermitido = 16;

                var n = String(newValue).split(".");
                if (n[1]) {
                    var n2 = n[1].slice(0, 4);
                    newValue = [n[0], n2].join(".");
                    if (spiltArray.length === 0) return;
                    if (spiltArray.length === 1 && (spiltArray[0] == '-' || spiltArray[0] === '.')) return;
                    if (spiltArray.length === 2 && newValue === '-.') return;

                    if (n[1].length > 4) {
                        tamanioPermitido = n[0].length + 4;
                        event.target.value = n[0] + '.' + n[1].slice(0, 4);
                        return;
                    }

                    if ((n[1].length == 3 && n[1] > 999) || (n[1].length > 3 && n[1] > 9999)) {
                        event.preventDefault();
                    }
                }
            }
        };

        vm.actualizaObservacion = function (indicador) {
            angular.forEach(indicador.ResumenCantidades, function (vigencia) {
                angular.forEach(vigencia.DetalleVigencia, function (mes) {
                    angular.forEach(indicador.PeriodosActivos, function (series) {
                        if (series.PeriodosPeriodicidadId == mes.PeriodoPeriodicidadId && series.Vigencia == vigencia.Vigencia) {
                            mes.Observacion = series.Observacion;
                        }
                    });
                });
            });
        };

        vm.actualizaValoresAnteriores = function (indicador) {
            var acumula = 0;
            angular.forEach(indicador.PeriodosActivos, function (series) {
                acumula = acumula + parseFloat(series.CantidadEjecutada);
                indicador.TotalAvanceReportado = parseFloat(acumula.toFixed(4));
            });

            angular.forEach(indicador.ResumenCantidades, function (vigencia) {
                angular.forEach(vigencia.DetalleVigencia, function (mes) {
                    angular.forEach(indicador.PeriodosActivos, function (series) {
                        acumula = acumula + parseFloat(series.CantidadEjecutada);
                        if (series.PeriodosPeriodicidadId == mes.PeriodoPeriodicidadId && series.Vigencia == vigencia.Vigencia) {
                            mes.AvanceEjecutadoMes = series.CantidadEjecutada;
                            mes.Observacion = series.Observacion;
                        }
                    });
                });
            });


            var cantidadperiodos = 0;
            angular.forEach(indicador.ResumenCantidades, function (vigencia) {
                cantidadperiodos = 0;
                angular.forEach(indicador.PeriodosActivos, function (series) {
                    if (series.Vigencia == vigencia.Vigencia) {
                        cantidadperiodos = series.PeriodosPeriodicidadId;
                    }
                });
                vigencia.cantidadperiodos = cantidadperiodos;
            });



            var acumulameses = 0;
            if (indicador.UnidadMedidaId == 15) {
                angular.forEach(indicador.ResumenCantidades, function (vigencia) {
                    acumulameses = 0;
                    angular.forEach(vigencia.DetalleVigencia, function (mes) {
                        acumulameses = acumulameses + parseFloat(mes.AvanceEjecutadoMes);
                        if (vigencia.cantidadperiodos !== 0) {
                            vigencia.TotalMetaEjecutada = parseFloat(acumulameses.toFixed(4) / vigencia.cantidadperiodos);
                            vigencia.AcumuladoVigencia = parseFloat(acumulameses.toFixed(4) / vigencia.cantidadperiodos);
                        }
                    });
                });
            }
            else {
                angular.forEach(indicador.ResumenCantidades, function (vigencia) {
                    acumulameses = 0;
                    angular.forEach(vigencia.DetalleVigencia, function (mes) {
                        acumulameses = acumulameses + parseFloat(mes.AvanceEjecutadoMes);
                        vigencia.TotalMetaEjecutada = parseFloat(acumulameses.toFixed(4));
                        vigencia.AcumuladoVigencia = parseFloat(acumulameses.toFixed(4));
                    });
                });
            }


        };

        /*------------------------------AQUI TERMINA PARA LOGICA DEL NEGOCIO ------------------------------------------------------*/

        vm.inicializarComponenteCheck = function () {
            vm.handlerComponentesChecked = {
                'regionalizacionavanceregionaliza': true
            };
        }

        vm.notificacionCambiosCapitulos = function (nombreCapituloHijo) {
            if (vm.componentesRefresh.includes(nombreCapituloHijo)) {
                vm.refreshComponente();
            }
        };

        /*--------------------- Validaciones ---------------------*/

        vm.notificacionValidacionPadre = function (errores) {
            //debugger;
            if (errores != undefined) {
                var erroresFiltrados = utilsValidacionSeccionCapitulosServicio.getErroresValidados(vm.nombreComponente, errores);
                vm.erroresActivos = erroresFiltrados.erroresActivos;

                vm.ejecutarErrores();

                var isValid = (vm.erroresActivos.length <= 0);
                vm.notificacionestado({ nombreComponente: vm.nombreComponente, esValido: isValid });
            }
        };
    }

    angular.module('backbone').component('resumenAvanceRegionalizacion', {
        templateUrl: "src/app/formulario/ventanas/seguimientoControl/componentes/reporteRegionalizacion/componentes/resumenAvanceRegionalizacion/resumenAvanceRegionalizacionCap.html",
        controller: resumenAvanceRegionalizacionCapController,
        controllerAs: "vm",
        bindings: {
            bpin: '@',
            guardadoevent: '&',
            notificacionvalidacion: '&',
            notificacioncambios: '&',
            notificacionestado: '&',
            refreshregionalizacion: '=',
        }
    });

})();