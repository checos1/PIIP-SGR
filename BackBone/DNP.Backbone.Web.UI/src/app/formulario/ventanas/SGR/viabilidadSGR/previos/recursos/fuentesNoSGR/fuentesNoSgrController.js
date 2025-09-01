(function () {
    'use strict';

    fuentesNoSgrController.$inject = ['$scope', 'previosSgrServicio', 'justificacionCambiosServicio', 'utilidades', '$sessionStorage', '$uibModal', '$timeout',];

    function fuentesNoSgrController(
        $scope,
        previosSgrServicio,
        justificacionCambiosServicio,
        utilidades,
        $sessionStorage,
        $uibModal,
        $timeout,
    ) {

        var vm = this;
        vm.nombreComponente = "sgrviabilidadpreviosrecursosfuentesnosgr";
        vm.notificacionCambiosCapitulos = null;
        vm.disabled = false;
        vm.disabled2 = false;
        vm.activar = true;
        vm.listaFuentesEtapaNoSGR = null;
        vm.etapaIdNoSGR = null;
        vm.nombreEtapaNoSGR = null;
        vm.entidadEtapaNoSGR = "";
        vm.tipoRecursoNoSGR = "";
        vm.Editar = "EDITAR";
        vm.totalFuentes = 0;
        vm.totalFuentePreInversion = 0;
        vm.totalFuenteInversion = 0;
        vm.totalFuenteOperacion = 0;
        vm.totalFuenteActiva = 0;
        vm.totalCostosPreInversion = 0;
        vm.totalCostosInversion = 0;
        vm.totalCostosOperacion = 0;
        vm.Bpin = null;
        vm.AddVigencia = false;
        /*vm.btnAddVisible = false;*/
        vm.totalFuentesVigencia = 0;
        vm.guardarVisible = true;
        vm.erroresComponente = [];
        vm.ConvertirNumero = ConvertirNumero;
        vm.ConvertirNumero2 = ConvertirNumero2;
        vm.componentesRefresh = [
            "sgrviabilidadpreviosrecursosfuentessgr"
        ];

        vm.init = function () {

            vm.notificacioncambios({ handler: vm.notificacionCambiosCapitulos, nombreComponente: vm.nombreComponente });
            vm.notificacionvalidacion({ handler: vm.notificacionValidacionPadre, nombreComponente: vm.nombreComponente, esValido: true });
            vm.notificacioninicio({ handlerInicio: vm.notificacionInicioPadre, nombreComponente: vm.nombreComponente, esValido: true });



        };
        vm.notificacionInicioPadre = function (nombreModificado, errores) {
            if (nombreModificado == vm.nombreComponente) {

                if (errores != null && errores != undefined) {
                    vm.erroresComponente = errores;
                }

                consultarFuentesNoSGR();
                consultarFuentesSGR();
                vm.disabled = $sessionStorage.soloLectura;

                vm.flujoaprobacion = utilidades.obtenerParametroTransversal('FlujoAprobacion1');

                if (vm.flujoaprobacion === $sessionStorage.InstanciaSeleccionada.FlujoId.toUpperCase()) {
                    bloquearControles();
                }
            }
        }
        vm.ActivarEditar = function () {
            if (vm.activar == true) {
                vm.Editar = "CANCELAR";
                vm.activar = false;
                vm.AddVigencia = false;
            }
            else {
                vm.Editar = "EDITAR";
                vm.activar = true;
                vm.AddVigencia = true;
                vm.Cancelar();
            }
        };

        vm.Cancelar = function () {
            vm.listaFuentesNoSGR = angular.copy(vm.listaFuentesNoSGRTemp);
            vm.filtrar();
            vm.CalcularTotalFuentes();
        };

        $scope.$on("BloquearPorCTUSPendiente", function (evt, data) {
            bloquearControles();
        });

        function bloquearControles() {
            vm.disabled = true;
        }

        function consultarFuentesNoSGR() {
            //vm.Bpin = $sessionStorage.InstanciaSeleccionada != undefined ? $sessionStorage.InstanciaSeleccionada.ProyectoId : "";

            var flujoaprobacion = utilidades.obtenerParametroTransversal('FlujoAprobacion1');

            if (flujoaprobacion !== $sessionStorage.InstanciaSeleccionada.FlujoId.toUpperCase()) {
                vm.Bpin = $sessionStorage.InstanciaSeleccionada != undefined ? $sessionStorage.InstanciaSeleccionada.ProyectoId : "";
            }
            else {
                vm.Bpin = $sessionStorage.InstanciaSeleccionada.IdObjetoNegocio;
            }

            let instanciaId = $sessionStorage.InstanciaSeleccionada != undefined ? $sessionStorage.InstanciaSeleccionada.IdInstancia : "";
            return previosSgrServicio.consultarFuentesNoSGR(vm.Bpin, instanciaId)
                .then(respuesta => {
                    if (respuesta.data) {
                        vm.listaFuentesNoSGR = angular.copy(jQuery.parseJSON(jQuery.parseJSON(respuesta.data)));
                        vm.listaFuentesNoSGRTemp = jQuery.parseJSON(jQuery.parseJSON(respuesta.data));
                        if (vm.erroresComponente != null && vm.erroresComponente != undefined) {
                            $timeout(function () {
                                vm.notificacionValidacionPadre(vm.erroresComponente);
                            }, 600);
                        }
                    }
                })
                .catch(error => {
                    console.log(error);
                    utilidades.mensajeError("Hubo un error al cargar la lista de fuentes");
                });
        }

        function consultarFuentesSGR() {
            //vm.Bpin = $sessionStorage.InstanciaSeleccionada != undefined ? $sessionStorage.InstanciaSeleccionada.ProyectoId : "";

            var flujoaprobacion = utilidades.obtenerParametroTransversal('FlujoAprobacion1');

            if (flujoaprobacion !== $sessionStorage.InstanciaSeleccionada.FlujoId.toUpperCase()) {
                vm.Bpin = $sessionStorage.InstanciaSeleccionada != undefined ? $sessionStorage.InstanciaSeleccionada.ProyectoId : "";
            }
            else {
                vm.Bpin = $sessionStorage.InstanciaSeleccionada.IdObjetoNegocio;
            }

            let instanciaId = $sessionStorage.InstanciaSeleccionada != undefined ? $sessionStorage.InstanciaSeleccionada.IdInstancia : "";
            return previosSgrServicio.consultarFuentesSGR(vm.Bpin, instanciaId)
                .then(respuesta => {
                    if (respuesta.data) {
                        vm.listaFuentes = angular.copy(jQuery.parseJSON(jQuery.parseJSON(respuesta.data)));
                        vm.listaFuentesTemp = jQuery.parseJSON(jQuery.parseJSON(respuesta.data));
                    }

                })
                .catch(error => {
                    console.log(error);
                    utilidades.mensajeError("Hubo un error al cargar la lista de fuentes");
                });
        }

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
                        vm.guardadocomponent({ nombreComponenteHijo: vm.nombreComponente });
                    }
                    else {
                        utilidades.mensajeError(response.data.Mensaje + " Capitulo Modificado");
                    }
                });
        }

        $scope.$watch('vm.listaFuentesNoSGR', function () {
            if (vm.listaFuentesNoSGR != null) {
                CalcularTotales();
            }
        });

        $scope.$watch('vm.listaFuentesEtapaNoSGR', function () {
            if (vm.listaFuentesEtapaNoSGR != null) {
                vm.CalcularTotalFuenteActiva();
                CalcularTotal();
            }
        }, true);

        vm.Guardar = function () {
            let validaCampos = false;
            //Validar campos obligatorios
            if (vm.listaFuentesEtapaNoSGR[0].Vigencias) {
                vm.listaFuentesEtapaNoSGR[0].Vigencias.some(vigencia => {
                    if (vigencia.Entidades) {
                        vigencia.Entidades.some(entidad => {
                            if (entidad.Vigencias) {
                                entidad.Vigencias.some(vig => {
                                    if (vig) {
                                        vig.ValorSolicitado = vig.ValorSolicitado === 0 ? 0 : typeof vig.ValorSolicitado == "string" ? parseFloat(vig.ValorSolicitado.replace(",", ".")) : vig.ValorSolicitado;
                                        if (vig.Vigencia == "0" || vig.Vigencia == null || vig.Vigencia == undefined) {
                                            swal("Error al realizar la operación", "El campo Vigencia es obligatorio. Por favor validar.", 'error');
                                            validaCampos = true;
                                        }

                                        //if (vig.ValorSolicitado == "" || vig.ValorSolicitado == null || vig.ValorSolicitado == undefined) {
                                        //    swal("Error al realizar la operación", "El campo Valor Solicitado es obligatorio. Por favor validar.", 'error');
                                        //    validaCampos = true;
                                        //}
                                    }
                                });
                            }
                        });
                    }
                });
            }

            //Validar Vigencias
            if (vm.listaFuentesEtapaNoSGR[0].Vigencias) {
                for (let i = 0; i < vm.listaFuentesEtapaNoSGR[0].Vigencias.length; i++) {
                    if (vm.listaFuentesEtapaNoSGR[0].Vigencias[i].Entidades) {
                        for (var j = 0; j < vm.listaFuentesEtapaNoSGR[0].Vigencias[i].Entidades.length; j++) {
                            if (vm.listaFuentesEtapaNoSGR[0].Vigencias[i].Entidades[j].Vigencias) {
                                for (var k = 0; k < vm.listaFuentesEtapaNoSGR[0].Vigencias[i].Entidades[j].Vigencias.length; k++) {
                                    let valida = vm.ValidarVigencias(i, j, vm.listaFuentesEtapaNoSGR[0].Vigencias[i].Entidades[j].Vigencias[k].Vigencia);
                                    if (!valida) {
                                        return;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            //Validar Vigencias
            if (validaCampos) {
                return;
            }

            //Validar Valor Vigencia no sea mayor a Costo
            if (!vm.ValidarValorVigenciaCostos()) {
                swal("Error al realizar la operación", "El Valor de las vigencias no puede ser mayor al costo. Por favor validar.", 'error');
                return;
            }

            vm.ValidarValorCostosTotalesProyectoPorVigencia();
            if (vm.isValidTotalesProyectoPorVigencia == false) {
                swal({
                    title: "Se han presentado inconsistencias por favor verifique.",
                    text: vm.validacion,
                    type: "error",
                    html: true,
                    confirmButtonText: "Aceptar",
                    closeOnConfirm: true,
                });
                return;
            }

            vm.listaFuentesEtapaNoSGR[0].ProyectoId = vm.Bpin;
            vm.listaFuentesNoSGR.Etapas[vm.listaFuentesEtapaNoSGR[0].EtapaId - 1].ProyectoId = vm.Bpin;
            vm.listaFuentesNoSGR.Etapas[vm.listaFuentesEtapaNoSGR[0].EtapaId - 1] = vm.listaFuentesEtapaNoSGR[0];
            previosSgrServicio.registrarFuentesNoSGR(vm.listaFuentesNoSGR.Etapas)
                .then(resultado => {
                    if (resultado.data != undefined) {
                        if (resultado.data.Status) {
                            guardarCapituloModificado();
                            vm.Editar = "EDITAR";
                            vm.activar = true;
                            vm.AddVigencia = true;
                            /*vm.listaFuentesNoSGRTemp = angular.copy(vm.listaFuentesNoSGR);*/
                            consultarFuentesNoSGR();
                            utilidades.mensajeSuccess('', false, false, false, "Los datos fueron guardados con éxito.");
                            vm.limpiarErrores();
                            cerrar('ok');
                        } else {
                            swal("Error al realizar la operación", resultado.data.Message, 'error');
                        }
                    } else {
                        swal('', "Error al realizar la operación", 'error');
                    }
                });
        };

        vm.notificacionCambiosCapitulos = function (nombreCapituloHijo) {
            if (vm.componentesRefresh.includes(nombreCapituloHijo)) {
                consultarFuentesNoSGR();
                consultarFuentesSGR();
            }
        }

        vm.AdicionarVigencia = function (indiceVigencia, indiceEntidad) {
            if (!vm.listaFuentesEtapaNoSGR[0].Vigencias[indiceVigencia].Entidades[indiceEntidad].Vigencias) {
                vm.listaFuentesEtapaNoSGR[0].Vigencias[indiceVigencia].Entidades[indiceEntidad].Vigencias = [];
            }
            vm.listaFuentesEtapaNoSGR[0].Vigencias[indiceVigencia].Entidades[indiceEntidad].Vigencias.push({ Vigencia: "0", ValorSolicitado: null });
            if (vm.listaFuentesEtapaNoSGR[0].Vigencias[indiceVigencia].Entidades[indiceEntidad].Vigencias.length > 0) {
                vm.AddVigencia = false;
            }
        };

        vm.EliminarVigencia = function (indiceVigencia, indiceEntidad, vigenciaIndex) {
            let valorSolicita = vm.listaFuentesEtapaNoSGR[0].Vigencias[indiceVigencia].Entidades[indiceEntidad].Vigencias[vigenciaIndex].ValorSolicitado;
            if (!valorSolicita) {
                vm.totalFuentes = vm.totalFuentes - parseFloat(valorSolicita === undefined ? 0 : valorSolicita);
            }
            vm.listaFuentesEtapaNoSGR[0].Vigencias[indiceVigencia].Entidades[indiceEntidad].Vigencias.splice(vigenciaIndex, 1);
        };

        vm.ValidarVigencias = function (indiceVigencia, indiceEntidad, vigencia) {
            let vig = vm.listaFuentesEtapaNoSGR[0].Vigencias[indiceVigencia].Entidades[indiceEntidad].Vigencias.filter(x => x.Vigencia === vigencia);
            if (vig.length > 1) {
                swal("Error al realizar la operación", "Ya existe el registro de la vigencia para la entidad y tipo recurso. Por favor validar.", 'error');
                return false;
            }
            return true;
        };

        vm.CalcularTotalFuentes = function () {
            vm.totalFuentes = 0;
            vm.totalFuentes = vm.totalFuentePreInversion + vm.totalFuenteInversion + vm.totalFuenteOperacion;
        };

        vm.CalcularTotalFuenteActiva = function () {
            vm.totalFuenteActiva = 0;

            if (vm.listaFuentesEtapaNoSGR[0].Vigencias) {
                vm.listaFuentesEtapaNoSGR[0].Vigencias.forEach(vigencia => {
                    if (vigencia.Entidades) {
                        vigencia.Entidades.forEach(entidad => {
                            if (entidad.Vigencias) {
                                entidad.Vigencias.forEach(vigencia => {
                                    if (vigencia) {
                                        vm.totalFuenteActiva += parseFloat(vigencia.ValorSolicitado === undefined ? 0 : vigencia.ValorSolicitado);
                                    }
                                });
                            }
                        });
                    }
                });
            }
        };

        function CalcularTotales() {
            if (vm.nombreEtapa == null) {
                CalcularTotalFuentesPreInversion();
                CalcularTotalFuentesInversion();
                CalcularTotalFuentesOperacion();

                CalcularTotalCostosPreInversion();
                CalcularTotalCostosInversion();
                CalcularTotalCostosOperacion();
            }

            CalcularTotal();

            if (vm.listaFuentesEtapaNoSGR != null) {
                if (vm.listaFuentesEtapaNoSGR[0]) {
                    if (vm.listaFuentesEtapaNoSGR[0].ComboEntidades) {
                        vm.guardarVisible = true;
                    }
                    else
                        vm.guardarVisible = false;
                }
                else
                    vm.guardarVisible = false;
            }
        }

        function CalcularTotal() {
            if (vm.nombreEtapa == null) {
                vm.totalFuentes = vm.totalFuentePreInversion + vm.totalFuenteInversion + vm.totalFuenteOperacion;
                return;
            }

            if (vm.nombreEtapa == "Preinversión") {
                vm.totalFuentes = vm.totalFuenteActiva + vm.totalFuenteInversion + vm.totalFuenteOperacion;
                return;
            }

            if (vm.nombreEtapa == "Inversión") {
                vm.totalFuentes = vm.totalFuentePreInversion + vm.totalFuenteActiva + vm.totalFuenteOperacion;
                return;
            }

            if (vm.nombreEtapa == "Operación") {
                vm.totalFuentes = vm.totalFuentePreInversion + vm.totalFuenteInversion + vm.totalFuenteActiva;
            }
        }

        function CalcularTotalFuentesPreInversion() {
            vm.totalFuentePreInversion = 0;

            if (vm.listaFuentesNoSGR.Etapas[0].Vigencias) {
                vm.listaFuentesNoSGR.Etapas[0].Vigencias.forEach(vigencia => {
                    if (vigencia.Entidades) {
                        vigencia.Entidades.forEach(entidad => {
                            if (entidad.Vigencias) {
                                entidad.Vigencias.forEach(vigencia => {
                                    if (vigencia) {
                                        vm.totalFuentePreInversion += parseFloat(vigencia.ValorSolicitado === undefined ? 0 : vigencia.ValorSolicitado);
                                    }
                                });
                            }
                        });
                    }
                });
            }
        }

        function CalcularTotalFuentesInversion() {
            vm.totalFuenteInversion = 0;

            if (vm.listaFuentesNoSGR.Etapas[1].Vigencias) {
                vm.listaFuentesNoSGR.Etapas[1].Vigencias.forEach(vigencia => {
                    if (vigencia.Entidades) {
                        vigencia.Entidades.forEach(entidad => {
                            if (entidad.Vigencias) {
                                entidad.Vigencias.forEach(vigencia => {
                                    if (vigencia) {
                                        vm.totalFuenteInversion += parseFloat(vigencia.ValorSolicitado === undefined ? 0 : vigencia.ValorSolicitado);
                                    }
                                });
                            }
                        });
                    }
                });
            }
        }

        function CalcularTotalFuentesOperacion() {
            vm.totalFuenteOperacion = 0;

            if (vm.listaFuentesNoSGR.Etapas[2].Vigencias.Vigencias) {
                vm.listaFuentesNoSGR.Etapas[2].Vigencias.Vigencias.forEach(vigencia => {
                    if (vigencia.Entidades) {
                        vigencia.Entidades.forEach(entidad => {
                            if (entidad.Vigencias) {
                                entidad.Vigencias.forEach(vigencia => {
                                    if (vigencia) {
                                        vm.totalFuenteOperacion += parseFloat(vigencia.ValorSolicitado === undefined ? 0 : vigencia.ValorSolicitado);
                                    }
                                });
                            }
                        });
                    }
                });
            }
        }

        function CalcularTotalCostosPreInversion() {
            vm.totalCostosPreInversion = 0;

            if (vm.listaFuentesNoSGR.Etapas[0].Vigencias) {
                vm.listaFuentesNoSGR.Etapas[0].Vigencias.forEach(vigencia => {
                    if (vigencia.Costo) {
                        vm.totalCostosPreInversion += parseFloat(vigencia.Costo === undefined ? 0 : vigencia.Costo);
                    }
                });
            }
        }

        function CalcularTotalCostosInversion() {
            vm.totalCostosInversion = 0;

            if (vm.listaFuentesNoSGR.Etapas[1].Vigencias) {
                vm.listaFuentesNoSGR.Etapas[1].Vigencias.forEach(vigencia => {
                    if (vigencia.Costo) {
                        vm.totalCostosInversion += parseFloat(vigencia.Costo === undefined ? 0 : vigencia.Costo);
                    }
                });
            }
        }

        function CalcularTotalCostosOperacion() {
            vm.totalCostosOperacion = 0;

            if (vm.listaFuentesNoSGR.Etapas[2].Vigencias) {
                vm.listaFuentesNoSGR.Etapas[2].Vigencias.forEach(vigencia => {
                    if (vigencia.Costo) {
                        vm.totalCostosOperacion += parseFloat(vigencia.Costo === undefined ? 0 : vigencia.Costo);
                    }
                });
            }
        }

        vm.ValidarValorVigenciaCostos = function () {

            vm.totalFuentesVigencia = 0;

            if (vm.listaFuentesEtapaNoSGR[0].Vigencias) {
                for (let i = 0; i < vm.listaFuentesEtapaNoSGR[0].Vigencias.length; i++) {
                    if (vm.listaFuentesEtapaNoSGR[0].Vigencias[i].Entidades) {
                        for (var j = 0; j < vm.listaFuentesEtapaNoSGR[0].Vigencias[i].Entidades.length; j++) {
                            if (vm.listaFuentesEtapaNoSGR[0].Vigencias[i].Entidades[j].Vigencias) {
                                for (var k = 0; k < vm.listaFuentesEtapaNoSGR[0].Vigencias[i].Entidades[j].Vigencias.length; k++) {
                                    vm.totalFuentesVigencia += parseFloat(vm.listaFuentesEtapaNoSGR[0].Vigencias[i].Entidades[j].Vigencias[k].ValorSolicitado);
                                }
                            }
                            if (vm.totalFuentesVigencia > vm.listaFuentesEtapaNoSGR[0].Vigencias[i].Costo) {
                                return false;
                            }
                            vm.totalFuentesVigencia = 0;
                        }
                    }
                }
            }
            return true;
        };

        vm.ValidarValorCostosTotalesProyectoPorVigencia = function () {
            vm.isValidTotalesProyectoPorVigencia = true;
            vm.totalFuentesVigenciaProyectoSGR = 0;
            vm.totalFuentesVigenciaProyectoNoSGR = 0;
            vm.validacion = "<ul>";

            if (vm.listaFuentes.Etapas) {
                vm.listaFuentes.Etapas.forEach(etapaSGR => {
                    if (etapaSGR.Vigencias) {
                        etapaSGR.Vigencias.forEach(vigenciaSGR => {
                            if (vigenciaSGR.Entidades) {
                                vigenciaSGR.Entidades.forEach(entidadSGR => {
                                    if (entidadSGR.Bienios) {
                                        entidadSGR.Bienios.forEach(bienioSGR => {
                                            if (bienioSGR) {
                                                vm.totalFuentesVigenciaProyectoSGR += parseFloat(bienioSGR.ValorSolicitado === undefined ? 0 : bienioSGR.ValorSolicitado);


                                            }
                                        });
                                    }
                                });

                                if (vm.listaFuentesEtapaNoSGR) {
                                    vm.listaFuentesEtapaNoSGR.forEach(etapaNoSGR => {
                                        if (etapaNoSGR.Vigencias) {
                                            etapaNoSGR.Vigencias.forEach(vigenciaNoSGR => {
                                                if (vigenciaNoSGR.Entidades) {
                                                    vigenciaNoSGR.Entidades.forEach(entidadNoSGR => {
                                                        if (entidadNoSGR.Vigencias) {
                                                            entidadNoSGR.Vigencias.forEach(vigenciaNoSGR => {
                                                                if (vigenciaNoSGR) {
                                                                    if (vigenciaNoSGR.Vigencia == vigenciaSGR.Vigencia)
                                                                        vm.totalFuentesVigenciaProyectoNoSGR += parseFloat(vigenciaNoSGR.ValorSolicitado === undefined ? 0 : vigenciaNoSGR.ValorSolicitado);
                                                                }
                                                            });
                                                        }
                                                    });
                                                }
                                            });
                                        }

                                    });
                                }

                                if ((vm.totalFuentesVigenciaProyectoSGR + vm.totalFuentesVigenciaProyectoNoSGR) > vigenciaSGR.Costo) {
                                    vm.isValidTotalesProyectoPorVigencia = false;
                                    vm.validacion = vm.validacion + "<li>Para la vigencia " + vigenciaSGR.Vigencia + " los valores solicitados SGR y NO SGR: $"
                                        + ConvertirNumero(vm.totalFuentesVigenciaProyectoSGR + vm.totalFuentesVigenciaProyectoNoSGR) + " superan los costos para la vigencia: "
                                        + ConvertirNumero(vigenciaSGR.Costo) + "</li>";
                                }

                                vm.totalFuentesVigenciaProyectoSGR = 0;
                                vm.totalFuentesVigenciaProyectoNoSGR = 0;
                                //********************* */
                            }
                        });
                    }
                });


            }
        };

        vm.filtrar = function () {
            if (vm.listaFuentesNoSGR) {
                vm.listaFuentesEtapaNoSGR = vm.listaFuentesNoSGR.Etapas.filter(function (a) {
                    return a.EtapaId === vm.etapaIdNoSGR;
                });
            }
            //if (vm.entidadEtapaNoSGR != "") {
            //    const entidadFilter = [];
            //    entidadFilter.push(vm.entidadEtapaNoSGR);
            //    vm.listaFuentesEtapaNoSGR = vm.listaFuentesEtapaNoSGR.filter(d => d.Vigencias === null ? false : d.Vigencias.every(c => c.Entidades === null ? false : c.Entidades.some(b => entidadFilter.includes(b.EntidadId))));
            //}
            //if (vm.tipoRecursoNoSGR != "") {
            //    const tipoRecursoFilter = [];
            //    tipoRecursoFilter.push(vm.tipoRecursoNoSGR);
            //    vm.listaFuentesEtapaNoSGR = vm.listaFuentesEtapaNoSGR.filter(d => d.Vigencias.every(c => c.Entidades.some(b => tipoRecursoFilter.includes(b.TipoRecursoId))));
            //}

            if (vm.listaFuentesEtapaNoSGR[0].Vigencias) {
                vm.listaFuentesEtapaNoSGR[0].Vigencias.forEach(vigencia => {
                    if (vigencia.Entidades) {
                        vigencia.Entidades.forEach(entidad => {
                            if (entidad.EntidadId) {
                                if (entidad.EntidadId == vm.entidadEtapa || (vm.entidadEtapa == null || vm.entidadEtapa == '')) {
                                    entidad.filtrarEntidad = null;
                                }
                                else {
                                    entidad.filtrarEntidad = false;
                                }
                            }
                        });
                    }
                });
            }
        };

        vm.verMas = function (Entidadad) {
            Entidadad.EntidadVerMas = (Entidadad.EntidadVerMas == null || Entidadad.EntidadVerMas === false) ? true : false;
        }

        vm.verMasRecurso = function (Recurso) {
            Recurso.RecursoVerMas = (Recurso.RecursoVerMas == null || Recurso.RecursoVerMas === false) ? true : false;
        }

        vm.mostrarTab = function (origen) {
            vm.etapaIdNoSGR = origen;
            vm.listaFuentesEtapaNoSGR = null;
            vm.mensaje = "";
            vm.activar = true;
            vm.Editar = "EDITAR";
            vm.listaFuentesNoSGR = angular.copy(vm.listaFuentesNoSGRTemp);
            vm.totalFuenteActiva = 0;
            switch (origen) {
                case 1:
                    {
                        vm.nombreEtapa = "Preinversión";
                        if (vm.listaFuentesNoSGR) {

                            if (vm.totalCostosPreInversion == 0) return;
                            vm.etapaId = origen;
                            vm.totalFuenteActiva = vm.totalFuentePreInversion;
                            vm.listaFuentesEtapaNoSGR = vm.listaFuentesNoSGR.Etapas.filter(function (a) {
                                return a.EtapaId === origen;
                            });
                        }
                        break;
                    }
                case 2:
                    {
                        vm.nombreEtapa = "Inversión";
                        if (vm.listaFuentesNoSGR) {
                            if (vm.totalCostosInversion == 0) return;
                            vm.etapaId = origen;
                            vm.totalFuenteActiva = vm.totalFuenteInversion;
                            vm.listaFuentesEtapaNoSGR = vm.listaFuentesNoSGR.Etapas.filter(function (a) {
                                return a.EtapaId === origen;
                            });
                        }
                        break;
                    }
                case 3:
                    {
                        vm.nombreEtapa = "Operación";
                        if (vm.listaFuentesNoSGR) {
                            if (vm.totalCostosOperacion == 0) return;
                            vm.etapaId = origen;
                            vm.totalFuenteActiva = vm.totalFuenteOperacion;
                            vm.listaFuentesEtapaNoSGR = vm.listaFuentesNoSGR.Etapas.filter(function (a) {
                                return a.EtapaId === origen;
                            });
                        }
                        break;
                    }
            }

            console.log(vm.listaFuentesNoSGR);
            let contadorEntidades = 0;
            if (vm.listaFuentesEtapaNoSGR[0].Vigencias) {
                vm.listaFuentesEtapaNoSGR[0].Vigencias.forEach(function (vigencia, indexVig) {
                    if (vigencia.Entidades) {
                        contadorEntidades++;
                        vigencia.Entidades.forEach(function (entidad, indexEntidad) {
                            if (!entidad.Vigencias) {
                                if (!vm.listaFuentesEtapaNoSGR[0].Vigencias[indexVig].Entidades[indexEntidad].Vigencias) {
                                    vm.listaFuentesEtapaNoSGR[0].Vigencias[indexVig].Entidades[indexEntidad].Vigencias = [];
                                    vm.AddVigencia = true;
                                    //vm.btnAddVisible = true;
                                }
                            }
                        });
                    }
                });
            }
            contadorEntidades == 0 ? vm.listaFuentesEtapaNoSGR = [] : vm.CalcularTotalFuentes();
        }
        vm.abrirModalDocumentosAdicionales = abrirModalDocumentosAdicionales;
        //vm.abrirModalDocumentosAdicionales = function (indiceVigencia, indiceEntidad, vigenciaIndex) {
        function abrirModalDocumentosAdicionales(indiceVigencia, indiceEntidad, vigenciaIndex) {
            if (vm.listaFuentesEtapaNoSGR[0].Vigencias[indiceVigencia].Entidades[indiceEntidad].Vigencias[vigenciaIndex].ValorSolicitado <= 0) {
                swal('Error al realizar la operación"', "Para poder agregar cofinanciadores el valor solicitado para la vigencia debe ser mayor a cero", 'error');
                return;
            }

            //debugger;
            //vm.listaFuentesEtapaNoSGR[0].Vigencias[indiceVigencia].Entidades[indiceEntidad].Vigencias;
            let mmodalInstance = $uibModal.open({
                animation: $scope.animationsEnabled,
                templateUrl: "/src/app/formulario/ventanas/SGR/viabilidadSGR/previos/recursos/fuentesNoSGR/addCofinanciacionSgr.html",
                controller: 'addCofinanciacionSgrController',
                controllerAs: "vm",
                openedClass: "modal-contentDNP",
                size: 'xl',
                resolve: {
                    vigencia: function () {
                        return vm.listaFuentesEtapaNoSGR[0].Vigencias[indiceVigencia].Vigencia;
                    },
                    vigenciaFuentesNoSgr: function () {
                        return vm.listaFuentesEtapaNoSGR[0].Vigencias[indiceVigencia].Entidades[indiceEntidad].Vigencias[vigenciaIndex].Vigencia;
                    },
                    valorSolicitadoFuentesNoSgr: function () {
                        return vm.listaFuentesEtapaNoSGR[0].Vigencias[indiceVigencia].Entidades[indiceEntidad].Vigencias[vigenciaIndex].ValorSolicitado;
                    },
                    bpin: function () {
                        return vm.Bpin;
                    }
                }

            });

            mmodalInstance.result.then(data => {
                if (data != null) {
                    //guardarCapituloModificado();
                    //consultar();
                    //vm.guardadoevent({ nombreComponenteHijo: vm.nombreComponente });
                }
            });
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
        function ConvertirNumero2(numero) {
            if (typeof (numero) == 'number') {
                return new Intl.NumberFormat('es-co', {
                    minimumFractionDigits: 2,
                }).format(numero);
            } else {
                return numero;
            };
        }
        vm.actualizaFila = function (event) {
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
        }

        vm.notificacionValidacionPadre = function (errores) {
            console.log("Validación  - Fuentes no SGR");
            vm.limpiarErrores();
            if (errores != undefined) {
                var erroresRelacionconlapl = errores.find(p => (p.Seccion + p.Capitulo) == vm.nombreComponente);
                if (erroresRelacionconlapl != undefined) {
                    var isValid = true;
                    var erroresJson = erroresRelacionconlapl.Errores == "" ? [] : JSON.parse(erroresRelacionconlapl.Errores);
                    var isValid = (erroresJson == null || erroresJson.length == 0);
                    if (!isValid) {
                        try {
                            erroresJson[vm.nombreComponente].forEach(p => {
                                var nameArr = p.Error.split('-');
                                var TipoError = nameArr[0].toString();
                                if (TipoError == 'CRCVSSNOSGR' || TipoError == 'CRCVSSNOSGRCOF') {
                                    vm.validarValoresVigencia(TipoError, nameArr[1].toString(), nameArr[2].toString(), p.Descripcion);
                                } else if (TipoError == 'RECNOSGR') {
                                    vm.validarSeccion(TipoError, nameArr[1].toString(), p.Descripcion, false);
                                }
                                else {
                                    vm.validarValores(nameArr[0].toString(), p.Descripcion, false);
                                }
                            });
                        }
                        catch (error) {
                            console.error('¡¡Tiene ERRORES - handlerValidacion del componente = ' + vm.handlerComponentes[i].componente + '!!');
                        }
                    }
                }
                vm.notificacionestado({ nombreComponente: vm.nombreComponente, esValido: isValid });

                if (erroresRelacionconlapl == undefined) {
                    var campomensajeerror = document.getElementById("alert-" + vm.nombreComponente);
                    if (campomensajeerror != undefined) {
                        campomensajeerror.classList.remove("ico-advertencia");
                    }
                }
            }
        };

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

        vm.validarValoresVigencia = function (TipoError, etapa, vigencia, errores) {

            var campoObligatorioJustificacion = document.getElementById(TipoError + etapa + vigencia);
            if (campoObligatorioJustificacion != undefined) {
                campoObligatorioJustificacion.innerHTML = "<span class='d-inline-block ico-advertencia'></span><span>" + errores + "</span>";
                campoObligatorioJustificacion.classList.remove('hidden');
            }
        }

        vm.limpiarErrores = function () {

            var errorElements = document.getElementsByClassName('messagealerttableDNP');
            var testDivs = Array.prototype.filter.call(errorElements, function (errorElement) {
                errorElement.innerHTML = "";
                errorElement.classList.add('hidden');
            });

            var campo0 = document.getElementById('alert-sgrviabilidadpreviosrecursosfuentesnosgr');
            if (campo0 != undefined) {
                campo0.innerHTML = "";
                campo0.classList.add('hidden');
            }
            campo0 = document.getElementById('RECNOSGRsgrviabilidadpreviosrecursosfuentesnosgr');
            if (campo0 != undefined) {
                campo0.innerHTML = "";
                campo0.classList.add('hidden');
            }
            if (vm.listaFuentesTemp != null && vm.listaFuentesTemp != undefined) {
                vm.listaFuentesTemp.Etapas.forEach(etapa => {
                    if (etapa.Vigencias) {
                        etapa.Vigencias.forEach(objVigencia => {
                            if (objVigencia.Vigencia) {
                                var campo = document.getElementById('CRCVSSNOSGR' + etapa.EtapaId + objVigencia.Vigencia);
                                if (campo != undefined) {
                                    campo.innerHTML = "";
                                    campo.classList.add('hidden');
                                }
                                campo = document.getElementById('CRCVSSNOSGRCOF' + etapa.EtapaId + objVigencia.Vigencia);
                                if (campo != undefined) {
                                    campo.innerHTML = "";
                                    campo.classList.add('hidden');
                                }
                            }
                        });

                    }
                });
            }
        }
    }

    angular.module('backbone').component('fuentesNoSgr', {
        templateUrl: "/src/app/formulario/ventanas/SGR/viabilidadSGR/previos/recursos/fuentesNoSGR/fuentesNoSgr.html",

        controller: fuentesNoSgrController,
        controllerAs: "vm",
        bindings: {
            callback: '&',
            notificacionvalidacion: '&',
            notificacionestado: '&',
            notificacioncambios: '&',
            notificacioninicio: '&',
            guardadocomponent: '&',
        },
    }).directive('stringToNumber', function () {
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