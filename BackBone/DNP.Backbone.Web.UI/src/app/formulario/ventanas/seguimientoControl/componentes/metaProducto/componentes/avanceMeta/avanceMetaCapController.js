(function () {
    'use strict';

    avanceMetaCapController.$inject = [
        '$scope',
        '$sessionStorage',
        '$uibModal',
        'utilidades',
        'avanceMetaCapServicio',
        'justificacionCambiosServicio',
        'utilsValidacionSeccionCapitulosServicio'
    ];

    function avanceMetaCapController(
        $scope,
        $sessionStorage,
        $uibModal,
        utilidades,
        avanceMetaCapServicio,
        justificacionCambiosServicio,
        utilsValidacionSeccionCapitulosServicio

    ) {
        var vm = this;
        vm.lang = "es";
        vm.nombreComponente = "metaproductoavancemetacap";
        vm.disabled = true;
        vm.habilitaBotones = true;
        vm.habilitarFinal = false;
        vm.registrosExcel = null;
        vm.listaDatosExcel = [];
        vm.verDatosExcel = false;
        vm.ConvertirNumero = ConvertirNumero;
        vm.disabled = false;
        vm.disabled2 = false;
        vm.Guardar = Guardar;
        vm.Cancelar = Cancelar;
        vm.Editar = Editar;
        vm.copiaIndicador;
        vm.actualizaValoresAnteriores;
        vm.soloLectura = $sessionStorage.soloLectura;
        vm.idElement0 = '';
        vm.avanceMetaProductoSeleccionado = 0;
        vm.idElement1 = '';
        vm.avanceMetaIndicadorSeleccionado = 0;
        vm.idElement2 = '';
        vm.avanceMetaVigenciaSeleccionada = 0;

        vm.init = function () {
            vm.inicializarComponenteCheck();
            vm.notificacioncambios({ handler: vm.notificacionCambiosCapitulos, nombreComponente: vm.nombreComponente });
            vm.notificacionvalidacion({ handler: vm.notificacionValidacionEvent, nombreComponente: vm.nombreComponente, handlerCapitulos: vm.capitulos });
            vm.consultarAvanceMetaProducto();
        };


        /*------------------------------AQUI INICIA PARA LOGICA DEL NEGOCIO ------------------------------------------------------*/

        function Cancelar(indicador) {
            angular.forEach(indicador.PeriodosActivos, function (series) {
                series.CantidadEjecutada = series.CantidadEjecutadaAnterior;
                series.Observacion = series.ObservacionAnterior;
            });
            vm.actualizaValoresAnteriores(indicador);
            indicador.HabilitaEditarIndicador = false;
        }

        function Editar(indicador) {

            angular.forEach(indicador.PeriodosActivos, function (series) {
                series.CantidadEjecutadaAnterior = series.CantidadEjecutada;
                series.ObservacionAnterior = series.Observacion;
                series.CantidadEjecutadaAnteriorF = series.CantidadEjecutadaF;
            });

            indicador.HabilitaEditarIndicador = true;
        }



        vm.consultarAvanceMetaProducto = function () {
            const objetoParametros = {
                instanciaId: $sessionStorage.idInstanciaIframe,
                proyectoId: $sessionStorage.proyectoId,
                codigoBpin: '0',
                vigencia: 0,
                periodoPeriodicidad: 0
            };
            vm.listaDatosExcel = [];
            avanceMetaCapServicio.consultarAvanceMetaProducto(objetoParametros).then(
                function (response) {
                    if (response.data && (response.statusText === "OK" || response.status === 200)) {
                        var arreglolistas = jQuery.parseJSON(response.data);
                        vm.listaDatos = jQuery.parseJSON(arreglolistas);
                        $scope.datosavanceMeta = vm.listaDatos;
                    } else {
                        swal('', "Error al obtener valores del avance.", 'error');
                    }
                },
                function (error) {
                    if (error) {
                        utilidades.mensajeError(error);
                    }
                }
            );
        };


        function Guardar(indicador) {

            var continuaGuardado = 1;
            angular.forEach(indicador.PeriodosActivos, function (series) {

                if (series.CantidadEjecutada == "" || series.CantidadEjecutada == null) {
                    series.CantidadEjecutada = 0.0000;
                }

                if (series.CantidadEjecutada == 0.0000) {
                    if (series.PeriodosPeriodicidadId == 12 && (series.Observacion == "" || series.Observacion == null)) {
                        utilidades.mensajeError("Para un avance ejecutado igual a 0 al fin de vigencia, debe ingresar una observación.");
                        continuaGuardado = 0;
                    }
                }

            });

            var valorEjecutadoMesAnterior = -1;
            var nuevoValorAcumulado = -1;
            var valorEjecutadoMesAnteriorOriginal = -1;

            if (continuaGuardado == 1) {
                return avanceMetaCapServicio.actualizarAvanceMetaProducto(indicador).then(
                    function (respuesta) {
                        if (respuesta.data && (respuesta.statusText === "OK" || respuesta.status === 200)) {
                            if (respuesta.data.Status) {
                                vm.guardadoevent({ nombreComponenteHijo: vm.nombreComponente });
                                vm.consultarAvanceMetaProducto();
                                guardarCapituloModificado();
                                utilidades.mensajeSuccess("", false, false, false, "Los datos fueron guardados con éxito.");
                                indicador.HabilitaEditarIndicador = false;


                                angular.forEach(indicador.PeriodosActivos, function (series) {
                                    if (valorEjecutadoMesAnterior === -1) {
                                        valorEjecutadoMesAnteriorOriginal = series.CantidadEjecutadaAnterior;
                                        valorEjecutadoMesAnterior = series.CantidadEjecutada;

                                    } else {
                                        if (nuevoValorAcumulado === -1) {
                                            series.AcumuladoMesAnterior = parseFloat(series.AcumuladoMesAnterior) - parseFloat(valorEjecutadoMesAnteriorOriginal) + parseFloat(valorEjecutadoMesAnterior);

                                        }
                                        else {
                                            series.AcumuladoMesAnterior = parseFloat(nuevoValorAcumulado) + parseFloat(valorEjecutadoMesAnterior);
                                        }

                                        valorEjecutadoMesAnteriorOriginal = series.CantidadEjecutadaAnterior;
                                        valorEjecutadoMesAnterior = series.CantidadEjecutada;
                                        nuevoValorAcumulado = series.AcumuladoMesAnterior;
                                        series.AcumuladoMesAnteriorF = ConvertirNumero(series.AcumuladoMesAnterior);
                                    }
                                    series.CantidadEjecutadaAnterior = series.CantidadEjecutada;
                                    series.ObservacionAnterior = series.Observacion;
                                    series.CantidadEjecutadaF = ConvertirNumero(series.CantidadEjecutada);
                                    series.CantidadEjecutadaAnteriorF = ConvertirNumero(series.CantidadEjecutada);
                                });


                            } else {
                                //swal('', respuesta.data.Message, 'warning');
                                utilidades.mensajeError(respuesta.data.Message);
                            }

                        } else {
                            utilidades.mensajeError("Error al realizar la operación");
                        }

                    });
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
                //SeccionCapituloId: vm.SeccionCapituloId,
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
                minimumFractionDigits: 4,
            }).format(numero);
        }


        vm.abrilNivelProductoAvance = function (idElement, productoId) {
            vm.avanceMetaIndicadorSeleccionado = 0;
            if (vm.idElement0 != '' && vm.idElement0 != idElement) {
                vm.idElement0 = vm.idElement0.replace("null", "");
                var elMasAnterior = document.getElementById(vm.idElement0 + '-mas');
                var elMenosAnterior = document.getElementById(vm.idElement0 + '-menos');
                if (elMasAnterior != null && elMenosAnterior != null) {
                    if (elMasAnterior.classList.contains('hidden')) {
                        elMenosAnterior.classList.add('hidden');
                        elMasAnterior.classList.remove('hidden');
                    }
                }
            }

            if (vm.idElement1 != '') {
                vm.idElement1 = vm.idElement1.replace("null", "");
                var elMasAnterior2 = document.getElementById(vm.idElement1 + '-mas');
                var elMenosAnterior2 = document.getElementById(vm.idElement1 + '-menos');
                if (elMasAnterior2 != null && elMenosAnterior2 != null) {
                    if (elMasAnterior2.classList.contains('hidden')) {
                        elMenosAnterior2.classList.add('hidden');
                        elMasAnterior2.classList.remove('hidden');
                    }
                }
            }

            vm.idElement0 = idElement;
            idElement = idElement.replace("null", "");
            var elMas = document.getElementById(idElement + '-mas');
            var elMenos = document.getElementById(idElement + '-menos');

            if (elMas != null && elMenos != null) {
                if (elMas.classList.contains('hidden')) {
                    elMenos.classList.add('hidden');
                    elMas.classList.remove('hidden');
                    vm.avanceMetaProductoSeleccionado = 0;
                } else {
                    elMenos.classList.remove('hidden');
                    elMas.classList.add('hidden');
                    vm.avanceMetaProductoSeleccionado = productoId;
                }
            }
        }

        vm.abrilNivelIndicadorMeta = function (idElement, indicadorId) {
            vm.avanceMetaVigenciaSeleccionada = 0;
            if (vm.idElement1 != '' && vm.idElement1 != idElement) {
                vm.idElement1 = vm.idElement1.replace("null", "");
                var elMasAnterior = document.getElementById(vm.idElement1 + '-mas');
                var elMenosAnterior = document.getElementById(vm.idElement1 + '-menos');
                if (elMasAnterior != null && elMenosAnterior != null) {
                    if (elMasAnterior.classList.contains('hidden')) {
                        elMenosAnterior.classList.add('hidden');
                        elMasAnterior.classList.remove('hidden');
                    }
                }
            }

            if (vm.idElement2 != '') {
                vm.idElement2 = vm.idElement2.replace("null", "");
                var elMasAnterior3 = document.getElementById(vm.idElement2 + '-mas');
                var elMenosAnterior3 = document.getElementById(vm.idElement2 + '-menos');
                if (elMasAnterior3 != null && elMenosAnterior3 != null) {
                    if (elMasAnterior3.classList.contains('hidden')) {
                        elMenosAnterior3.classList.add('hidden');
                        elMasAnterior3.classList.remove('hidden');
                    }
                }
            }

            vm.idElement1 = idElement;
            idElement = idElement.replace("null", "");
            var elMas = document.getElementById(idElement + '-mas');
            var elMenos = document.getElementById(idElement + '-menos');

            if (elMas != null && elMenos != null) {
                if (elMas.classList.contains('hidden')) {
                    elMenos.classList.add('hidden');
                    elMas.classList.remove('hidden');
                    vm.avanceMetaIndicadorSeleccionado = 0;
                } else {
                    elMenos.classList.remove('hidden');
                    elMas.classList.add('hidden');
                    vm.avanceMetaIndicadorSeleccionado = indicadorId;
                }
            }
        }

        vm.abrilNivelVigenciaMeta = function (idElement, vigencia) {
            if (vm.idElement2 != '' && vm.idElement2 != idElement) {
                vm.idElement2 = vm.idElement2.replace("null", "");
                var elMasAnterior = document.getElementById(vm.idElement2 + '-mas');
                var elMenosAnterior = document.getElementById(vm.idElement2 + '-menos');
                if (elMasAnterior != null && elMenosAnterior != null) {
                    if (elMasAnterior.classList.contains('hidden')) {
                        elMenosAnterior.classList.add('hidden');
                        elMasAnterior.classList.remove('hidden');
                    }
                }
            }

            vm.idElement2 = idElement;
            idElement = idElement.replace("null", "");
            var elMas = document.getElementById(idElement + '-mas');
            var elMenos = document.getElementById(idElement + '-menos');

            if (elMas != null && elMenos != null) {
                if (elMas.classList.contains('hidden')) {
                    elMenos.classList.add('hidden');
                    elMas.classList.remove('hidden');
                    vm.avanceMetaVigenciaSeleccionada = 0;
                } else {
                    elMenos.classList.remove('hidden');
                    elMas.classList.add('hidden');
                    vm.avanceMetaVigenciaSeleccionada = vigencia;
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



        vm.abrirMensajeQueEsEsto = function (opcion, mensaje) {
            var titulo = '';
            titulo = 'Avance indicador';
            if (opcion == 2) {
                titulo = 'Resumen indicador';
            }
            utilidades.mensajeInformacionN("", null, "<span class='anttituhori' > ¿Qué es esto? </span><br /> <br /> <span class='tituhori' >" + titulo + " </span>", mensaje);
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

        vm.actualizaFila = function (event, indicador) {
            if (Number.isNaN(event.target.value)) {
                return new Intl.NumberFormat('es-co', { minimumFractionDigits: 4, }).format(0.0000);
            }
            if (event.target.value == null) {
                return new Intl.NumberFormat('es-co', { minimumFractionDigits: 4, }).format(0.0000);
            }
            if (event.target.value == "") {
                return new Intl.NumberFormat('es-co', { minimumFractionDigits: 4, }).format(0.0000);
            }
            event.target.value = parseFloat(event.target.value.replace(",", "."));
            indicador.TotalAvanceReportado = 0;

            if (indicador.EsPorcentaje == 1 && event.target.value > 100) {
                angular.forEach(indicador.PeriodosActivos, function (series) {
                    series.CantidadEjecutada = series.CantidadEjecutadaAnterior;
                    series.Observacion = series.ObservacionAnterior;
                });
                utilidades.mensajeError("La unidad de medida del indicador es Porcentaje y esta ingresando un valor mayor a 100. ");
                vm.actualizaValoresAnteriores(indicador);

                return false;
            }

            var acumula = 0;
            angular.forEach(indicador.PeriodosActivos, function (series) {
                acumula = acumula + parseFloat(series.CantidadEjecutada);
                indicador.TotalAvanceReportado = parseFloat(acumula.toFixed(4));
            });

            ///actualiza el resumen con lo ingresado 
            angular.forEach(indicador.ResumenCantidades, function (vigencia) {
                angular.forEach(vigencia.DetalleVigencia, function (mes) {
                    angular.forEach(indicador.PeriodosActivos, function (series) {
                        if (series.PeriodosPeriodicidadId == mes.PeriodoPeriodicidadId && series.Vigencia == vigencia.Vigencia) {
                            mes.AvanceEjecutadoMes = series.CantidadEjecutada;
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
            var acumulamesesTotal = 0;
            if (indicador.EsPorcentaje == 0 && indicador.EsAcumulativo == 'No') { // cuando No es porcentaje y No acumula, entonces es promedio y para el total reportado de todas las vigencias saca el maximo --2022/12/19 se cambia ahora resulta que si suma y no es promedio cuando No acumula
                angular.forEach(indicador.ResumenCantidades, function (vigencia) {
                    acumulameses = 0;
                    angular.forEach(vigencia.DetalleVigencia, function (mes) {
                        acumulameses = acumulameses + parseFloat(mes.AvanceEjecutadoMes);
                        //if (vigencia.CountPeriodosEjecutados !== 0) {
                        //    vigencia.TotalMetaEjecutada = parseFloat(acumulameses.toFixed(4) / vigencia.CountPeriodosEjecutados);
                        //    vigencia.AcumuladoVigencia = parseFloat(acumulameses.toFixed(4) / vigencia.CountPeriodosEjecutados);
                        //}
                        vigencia.TotalMetaEjecutada = parseFloat(acumulameses.toFixed(4));
                        vigencia.AcumuladoVigencia = parseFloat(acumulameses.toFixed(4));
                    });
                    if (vigencia.TotalMetaEjecutada > acumulamesesTotal) {
                        acumulamesesTotal = vigencia.TotalMetaEjecutada;
                    }
                });
                indicador.TotalAvanceReportado = parseFloat(acumulamesesTotal.toFixed(4));
            }
            else if (indicador.EsPorcentaje == 0 && indicador.EsAcumulativo == 'Si') { // cuando No es porcentaje y Si acumula, suma  y para el total reportado de todas las vigencias saca la suma
                acumulamesesTotal = 0;
                angular.forEach(indicador.ResumenCantidades, function (vigencia) {
                    acumulameses = 0;
                    angular.forEach(vigencia.DetalleVigencia, function (mes) {
                        acumulameses = acumulameses + parseFloat(mes.AvanceEjecutadoMes);
                        vigencia.TotalMetaEjecutada = parseFloat(acumulameses.toFixed(4));
                        vigencia.AcumuladoVigencia = parseFloat(acumulameses.toFixed(4));
                    });
                    acumulamesesTotal = acumulamesesTotal + vigencia.TotalMetaEjecutada;

                });
                indicador.TotalAvanceReportado = parseFloat(acumulamesesTotal.toFixed(4));
            }
            else if (indicador.EsPorcentaje == 1 && indicador.EsAcumulativo == 'No') { // cuando SI es porcentaje  entonces es el ultimo registrado  y para el total reportado de todas las vigencias saca el maximo
                acumulamesesTotal = 0;
                angular.forEach(indicador.ResumenCantidades, function (vigencia) {
                    acumulameses = 0;
                    angular.forEach(vigencia.DetalleVigencia, function (mes) {
                        if (mes.PeriodoPeriodicidadId == vigencia.CountPeriodosEjecutados) {
                            acumulameses = parseFloat(mes.AvanceEjecutadoMes);
                            vigencia.TotalMetaEjecutada = parseFloat(acumulameses.toFixed(4));
                            vigencia.AcumuladoVigencia = parseFloat(acumulameses.toFixed(4));
                        }
                    });

                    if (acumulameses > acumulamesesTotal) {
                        acumulamesesTotal = acumulameses;
                    }
                });
                indicador.TotalAvanceReportado = parseFloat(acumulamesesTotal.toFixed(4));
            }

            else if (indicador.EsPorcentaje == 1 && indicador.EsAcumulativo == 'Si') { // si el indicador Si es porcentaje y  Si acumula saca el ultimo registrado y para el total reportado de todas las vigencias saca la suma
                acumulamesesTotal = 0;
                angular.forEach(indicador.ResumenCantidades, function (vigencia) {
                    acumulameses = 0;
                    angular.forEach(vigencia.DetalleVigencia, function (mes) {
                        if (mes.PeriodoPeriodicidadId == vigencia.CountPeriodosEjecutados) {
                            acumulameses = parseFloat(mes.AvanceEjecutadoMes);
                            vigencia.TotalMetaEjecutada = parseFloat(acumulameses.toFixed(4));
                            vigencia.AcumuladoVigencia = parseFloat(acumulameses.toFixed(4));
                        }
                    });
                    acumulamesesTotal = acumulamesesTotal + vigencia.TotalMetaEjecutada;
                });
                indicador.TotalAvanceReportado = parseFloat(acumulamesesTotal.toFixed(4));
            }


            const val = event.target.value;
            const decimalCnt = val.split('.')[1] ? val.split('.')[1].length : 0;
            var total = event.target.value = decimalCnt && decimalCnt > 4 ? event.target.value : parseFloat(val).toFixed(4);
            return new Intl.NumberFormat('es-co', { minimumFractionDigits: 4, }).format(total);


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

            ///actualiza el resumen con lo ingresado 
            angular.forEach(indicador.ResumenCantidades, function (vigencia) {
                angular.forEach(vigencia.DetalleVigencia, function (mes) {
                    angular.forEach(indicador.PeriodosActivos, function (series) {
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
            var acumulamesesTotal = 0;
            if (indicador.EsPorcentaje == 0 && indicador.EsAcumulativo == 'No') { // cuando No es porcentaje y No acumula, entonces es promedio y para el total reportado de todas las vigencias saca el maximo
                angular.forEach(indicador.ResumenCantidades, function (vigencia) {
                    acumulameses = 0;
                    angular.forEach(vigencia.DetalleVigencia, function (mes) {
                        acumulameses = acumulameses + parseFloat(mes.AvanceEjecutadoMes);
                        //if (vigencia.CountPeriodosEjecutados !== 0) {
                        //    vigencia.TotalMetaEjecutada = parseFloat(acumulameses.toFixed(4) / vigencia.CountPeriodosEjecutados);
                        //    vigencia.AcumuladoVigencia = parseFloat(acumulameses.toFixed(4) / vigencia.CountPeriodosEjecutados);
                        //}
                        vigencia.TotalMetaEjecutada = parseFloat(acumulameses.toFixed(4));
                        vigencia.AcumuladoVigencia = parseFloat(acumulameses.toFixed(4));
                    });
                    if (vigencia.TotalMetaEjecutada > acumulamesesTotal) {
                        acumulamesesTotal = vigencia.TotalMetaEjecutada;
                    }
                });
                indicador.TotalAvanceReportado = parseFloat(acumulamesesTotal.toFixed(4));
            }
            else if (indicador.EsPorcentaje == 0 && indicador.EsAcumulativo == 'Si') { // cuando No es porcentaje y Si acumula, suma  y para el total reportado de todas las vigencias saca la suma
                acumulamesesTotal = 0;
                angular.forEach(indicador.ResumenCantidades, function (vigencia) {
                    acumulameses = 0;
                    angular.forEach(vigencia.DetalleVigencia, function (mes) {
                        acumulameses = acumulameses + parseFloat(mes.AvanceEjecutadoMes);
                        vigencia.TotalMetaEjecutada = parseFloat(acumulameses.toFixed(4));
                        vigencia.AcumuladoVigencia = parseFloat(acumulameses.toFixed(4));
                    });
                    acumulamesesTotal = acumulamesesTotal + vigencia.TotalMetaEjecutada;

                });
                indicador.TotalAvanceReportado = parseFloat(acumulamesesTotal.toFixed(4));
            }
            else if (indicador.EsPorcentaje == 1 && indicador.EsAcumulativo == 'No') { // cuando SI es porcentaje  entonces es el ultimo registrado  y para el total reportado de todas las vigencias saca el maximo
                acumulamesesTotal = 0;
                angular.forEach(indicador.ResumenCantidades, function (vigencia) {
                    acumulameses = 0;
                    angular.forEach(vigencia.DetalleVigencia, function (mes) {
                        if (mes.PeriodoPeriodicidadId == vigencia.CountPeriodosEjecutados) {
                            acumulameses = parseFloat(mes.AvanceEjecutadoMes);
                            vigencia.TotalMetaEjecutada = parseFloat(acumulameses.toFixed(4));
                            vigencia.AcumuladoVigencia = parseFloat(acumulameses.toFixed(4));
                        }
                    });

                    if (acumulameses > acumulamesesTotal) {
                        acumulamesesTotal = acumulameses;
                    }
                });
                indicador.TotalAvanceReportado = parseFloat(acumulamesesTotal.toFixed(4));
            }

            else if (indicador.EsPorcentaje == 1 && indicador.EsAcumulativo == 'Si') { // si el indicador Si es porcentaje y  Si acumula saca el ultimo registrado y para el total reportado de todas las vigencias saca la suma
                acumulamesesTotal = 0;
                angular.forEach(indicador.ResumenCantidades, function (vigencia) {
                    acumulameses = 0;
                    angular.forEach(vigencia.DetalleVigencia, function (mes) {
                        if (mes.PeriodoPeriodicidadId == vigencia.CountPeriodosEjecutados) {
                            acumulameses = parseFloat(mes.AvanceEjecutadoMes);
                            vigencia.TotalMetaEjecutada = parseFloat(acumulameses.toFixed(4));
                            vigencia.AcumuladoVigencia = parseFloat(acumulameses.toFixed(4));
                        }
                    });
                    acumulamesesTotal = acumulamesesTotal + vigencia.TotalMetaEjecutada;
                });
                indicador.TotalAvanceReportado = parseFloat(acumulamesesTotal.toFixed(4));
            }


        };

        /*------------------------------AQUI TERMINA PARA LOGICA DEL NEGOCIO ------------------------------------------------------*/


        vm.inicializarComponenteCheck = function () {
            vm.handlerComponentesChecked = {
                'metaproductoavancemetacap': true
            };
        }


        vm.notificacionCambiosCapitulos = function (nombreCapituloHijo) {
            if (vm.componentesRefresh.includes(nombreCapituloHijo)) {
                vm.refreshComponente();
            }
        };

        /*--------------------- Validaciones ---------------------*/

        vm.notificacionValidacionPadre = function (errores) {
            if (errores != undefined) {
                vm.erroresActivos = [];
                var erroresFiltrados = utilsValidacionSeccionCapitulosServicio.getErroresValidados(vm.nombreComponente, errores);
                vm.erroresActivos = erroresFiltrados.erroresActivos;
                console.log("erroresActivos", vm.erroresActivos)
                vm.ejecutarErrores();
                vm.notificacionestado({ nombreComponente: vm.nombreComponente, esValido: erroresFiltrados.isValid });
            }
        }

        vm.notificacionValidacionEvent = function (listErrores) {
            var erroresList = listErrores.errores.filter(p => p.Seccion == vm.nombreComponente);
            vm.inicializarComponenteCheck();
            vm.esValido = true;
            if (erroresList.length > 0) {
                for (var i = 0; i < vm.handlerComponentes.length; i++) {
                    if (vm.handlerComponentes[i].handlerValidacion) vm.handlerComponentes[i].handlerValidacion(erroresList);
                }
            }
        }



        vm.limpiarErrores = function () {
            var listadoErrores = document.getElementsByClassName("messagealerttableDNP");
            var listadoErroresContainer = document.getElementsByClassName("errores-contenedor");
            for (var i = 0; i < listadoErroresContainer.length; i++) {
                if (!listadoErroresContainer[i].classList.contains("d-none")) {
                    listadoErroresContainer[i].classList.add("d-none");

                }
            }

            for (var i = 0; i < listadoErrores.length; i++) {
                if (!listadoErrores[i].classList.contains("d-none")) {
                    listadoErrores[i].classList.add("d-none");
                    listadoErrores[i].innerHTML = '';
                }
            }
        };

        vm.ejecutarErrores = function () {
            vm.limpiarErrores();
            vm.erroresActivos.forEach(p => {
                if (vm.errores[p.Error] != undefined) {
                    vm.errores[p.Error]({
                        error: p.Error,
                        descripcion: p.Descripcion,
                        data: p.Data
                    });
                }
            });
        };


    }

    angular.module('backbone').component('avanceMetaCap', {
        templateUrl: "src/app/formulario/ventanas/seguimientoControl/componentes/metaProducto/componentes/avanceMeta/avanceMetaCap.html",
        controller: avanceMetaCapController,
        controllerAs: "vm",
        bindings: {
            bpin: '@',
            guardadoevent: '&',
            notificacionvalidacion: '&',
            notificacioncambios: '&',
            notificacionestado: '&'
        }
    });

})();