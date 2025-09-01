(function () {
    'use strict';

    fuentesSgrController.$inject = ['$scope', 'previosSgrServicio', 'justificacionCambiosServicio', 'utilidades', '$sessionStorage', '$timeout'];

    function fuentesSgrController(
        $scope,
        previosSgrServicio,
        justificacionCambiosServicio,
        utilidades,
        $sessionStorage,
        $timeout
    ) {
        var vm = this;
        vm.nombreComponente = "sgrviabilidadpreviosrecursosfuentessgr";
        vm.notificacionCambiosCapitulos = null;
        vm.disabled = false;
        vm.activar = true;
        vm.listaFuentesEtapa = null;
        vm.etapaId = null;
        vm.nombreEtapa = null;
        vm.entidadEtapa = "";
        vm.tipoRecurso = "";
        vm.bienioEntidad = [];
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
        vm.totalFuentesBienio = 0;
        vm.DevolverProyecto = {};
        vm.disabled = false;
        vm.flujoaprobacion = "";
        vm.erroresComponente = [];
        vm.ConvertirNumero = ConvertirNumero;
        vm.ConvertirNumero2 = ConvertirNumero2;
        vm.componentesRefresh = [
            "sgrviabilidadpreviosrecursosfuentesnosgr"
        ];

        vm.idInstancia = $sessionStorage.idInstancia;

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

                vm.flujoaprobacion = utilidades.obtenerParametroTransversal('FlujoAprobacion1');

                consultarFuentesSGR();
                consultarFuentesNoSGR();
               
                vm.disabled = $sessionStorage.soloLectura;

                if (vm.flujoaprobacion === $sessionStorage.InstanciaSeleccionada.FlujoId.toUpperCase()) {
                    bloquearControles();
                }

                if ($scope.$parent !== undefined && $scope.$parent.$parent !== undefined && $scope.$parent.$parent.vm !== undefined && $scope.$parent.$parent.vm.HabilitarGuardarPaso !== undefined && !$scope.$parent.$parent.vm.HabilitarGuardarPaso) {
                    bloquearControles();
                }
                vm.AddVigencia = true;
            }
        }
        $scope.$on("BloquearPorCTUSPendiente", function (evt, data) {
            bloquearControles();
        });

        function bloquearControles() {
            vm.disabled = true;
            vm.activar = true;
            vm.AddVigencia = true;
        }

        function consultarFuentesSGR() {

            if (vm.flujoaprobacion !== $sessionStorage.InstanciaSeleccionada.FlujoId.toUpperCase()) {
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

        function consultarFuentesNoSGR() {

            if (vm.flujoaprobacion !== $sessionStorage.InstanciaSeleccionada.FlujoId.toUpperCase()) {
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
                    }
                })
                .catch(error => {
                    console.log(error);
                    utilidades.mensajeError("Hubo un error al cargar la lista de fuentes");
                });
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
                        vm.guardadocomponent({ nombreComponenteHijo: vm.nombreComponente });
                    }
                    else {
                        utilidades.mensajeError(response.data.Mensaje + " Capitulo Modificado");
                    }
                });
        }

        $scope.$watch('vm.listaFuentes', function () {
            if (vm.listaFuentes != null) {
                CalcularTotales();
            }
        });

        $scope.$watch('vm.listaFuentesEtapa', function () {
            if (vm.listaFuentesEtapa != null) {
                vm.CalcularTotalFuenteActiva();
                CalcularTotal();
            }
        }, true);

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
            vm.listaFuentes = angular.copy(vm.listaFuentesTemp);
            vm.filtrar();
            /*            vm.CalcularTotalFuentes();*/
        };

        vm.devolverProyecto = function () {
            var Observacion = document.getElementById("observacionAprobacion");
            if (Observacion.value != null && Observacion.value !== "") {
                vm.DevolverProyecto.InstanciaId = vm.idInstancia;
                vm.DevolverProyecto.Bpin = vm.Bpin;
                vm.DevolverProyecto.ProyectoId = vm.Bpin;
                vm.DevolverProyecto.Observacion = Observacion.value;
                vm.DevolverProyecto.DevolverId = true;
                vm.DevolverProyecto.EstadoDevolver = 7; //Returned	Solicitud de Información MGA
                return previosSgrServicio.devolverProyecto(vm.DevolverProyecto).then(
                    function (response) {
                        if (response.data || response.statusText === "OK") {
                            if (response.data.Exito) {
                                utilidades.mensajeSuccess("Operación realizada con éxito!", false, false, false);
                                $location.url("/proyectos/pl");
                            } else {
                                swal('', response.data.Mensaje, 'warning');
                            }
                        } else {
                            swal('', "Error al realizar la operación", 'error');
                        }
                    }
                );
            }
            else {
                swal('El campo observaciones no se encuentra diligenciado.', 'Revise las campos señalados para que sean validados nuevamente.', 'error');
            }
        };

        vm.guardado = function (nombreComponenteHijo) {
            vm.guardadocomponent({ nombreComponente: vm.nombreComponente, nombreComponenteHijo: nombreComponenteHijo });
        }

        vm.Guardar = function () {
            let validaCampos = false;
            //Validar campos obligatorios
            if (vm.listaFuentesEtapa[0].Vigencias) {
                vm.listaFuentesEtapa[0].Vigencias.some(vigencia => {
                    if (vigencia.Entidades) {
                        vigencia.Entidades.some(entidad => {
                            if (entidad.Bienios) {
                                entidad.Bienios.some(bienio => {
                                    if (bienio) {
                                        bienio.ValorSolicitado = bienio.ValorSolicitado === 0 ? 0 : typeof bienio.ValorSolicitado == "string" ? parseFloat(bienio.ValorSolicitado.replace(",", ".")) : bienio.ValorSolicitado;
                                        if (bienio.BieniodId == "0" || bienio.BieniodId == null || bienio.BieniodId == undefined) {
                                            swal("Error al realizar la operación", "El campo Bienio es obligatorio. Por favor validar.", 'error');
                                            validaCampos = true;
                                        }

                                        if (bienio.ValorSolicitado == "" || bienio.ValorSolicitado == null || bienio.ValorSolicitado == undefined) {
                                            swal("Error al realizar la operación", "El campo Valor Solicitado es obligatorio. Por favor validar.", 'error');
                                            validaCampos = true;
                                        }
                                    }
                                });
                            }
                        });
                    }
                });
            }

            //Validar Bienios
            if (vm.listaFuentesEtapa[0].Vigencias) {
                for (let i = 0; i < vm.listaFuentesEtapa[0].Vigencias.length; i++) {
                    if (vm.listaFuentesEtapa[0].Vigencias[i].Entidades) {
                        for (var j = 0; j < vm.listaFuentesEtapa[0].Vigencias[i].Entidades.length; j++) {
                            if (vm.listaFuentesEtapa[0].Vigencias[i].Entidades[j].Bienios) {
                                for (var k = 0; k < vm.listaFuentesEtapa[0].Vigencias[i].Entidades[j].Bienios.length; k++) {
                                    let valida = vm.ValidarBienios(i, j, vm.listaFuentesEtapa[0].Vigencias[i].Entidades[j].Bienios[k].BieniodId);
                                    if (!valida) {
                                        return;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            //Validar Bienios
            if (validaCampos) {
                return;
            }

            //Validar Valor Bienio no sea mayor a Costo
            if (!vm.ValidarValorBienioCostos()) {
                swal("Error al realizar la operación", "El Valor de los bienios no puede ser mayor al costo. Por favor validar.", 'error');
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

            vm.listaFuentesEtapa[0].ProyectoId = vm.Bpin;
            vm.listaFuentes.Etapas[vm.listaFuentesEtapa[0].EtapaId - 1].ProyectoId = vm.Bpin;
            vm.listaFuentes.Etapas[vm.listaFuentesEtapa[0].EtapaId - 1] = vm.listaFuentesEtapa[0];
            previosSgrServicio.registrarFuentesSGR(vm.listaFuentes.Etapas)
                .then(resultado => {
                    if (resultado.data != undefined) {
                        if (resultado.data.Status) {
                            guardarCapituloModificado();
                            vm.Editar = "EDITAR";
                            vm.activar = true;
                            vm.AddVigencia = true;
                            /*                            vm.listaFuentes = angular.copy(vm.listaFuentes);*/
                            /*vm.listaFuentesTemp = angular.copy(vm.listaFuentes);*/
                            consultarFuentesSGR();
                            utilidades.mensajeSuccess('', false, false, false, "Los datos fueron guardados con éxito.");
                        } else {
                            swal("Error al realizar la operación", resultado.data.Message, 'error');
                        }
                    } else {
                        swal('', "Error al realizar la operación", 'error');
                    }
                });
        };

        vm.AdicionarBienio = function (indiceVigencia, indiceEntidad) {
            if (!vm.listaFuentesEtapa[0].Vigencias[indiceVigencia].Entidades[indiceEntidad].Bienios) {
                vm.listaFuentesEtapa[0].Vigencias[indiceVigencia].Entidades[indiceEntidad].Bienios = [];
            }
            vm.listaFuentesEtapa[0].Vigencias[indiceVigencia].Entidades[indiceEntidad].Bienios.push({ BieniodId: "0", ValorSolicitado: 0 });
            if (vm.listaFuentesEtapa[0].Vigencias[indiceVigencia].Entidades[indiceEntidad].Bienios.length > 0) {
                vm.AddVigencia = false;
            }
        };

        vm.EliminarBienio = function (indiceVigencia, indiceEntidad, bienioIndex) {
            let valorSolicita = vm.listaFuentesEtapa[0].Vigencias[indiceVigencia].Entidades[indiceEntidad].Bienios[bienioIndex].ValorSolicitado;
            if (!valorSolicita) {
                vm.totalFuentes = vm.totalFuentes - parseFloat(valorSolicita === undefined ? 0 : valorSolicita);
            }
            vm.listaFuentesEtapa[0].Vigencias[indiceVigencia].Entidades[indiceEntidad].Bienios.splice(bienioIndex, 1);
        };

        vm.ValidarBienios = function (indiceVigencia, indiceEntidad, bienioId) {
            let bienios = vm.listaFuentesEtapa[0].Vigencias[indiceVigencia].Entidades[indiceEntidad].Bienios.filter(x => x.BieniodId === bienioId);
            if (bienios.length > 1) {
                swal("Error al realizar la operación", "Ya existe el registro del bienio para la entidad, tipo recurso y vigencia. Por favor validar.", 'error');
                return false;
            }
            return true;
        };

        vm.CalcularTotalFuentes = function () {
            vm.totalFuentes = 0;
            vm.totalFuentes = vm.totalFuentePreInversion + vm.totalFuenteInversion + vm.totalFuenteOperacion;
        };

        vm.notificacionCambiosCapitulos = function (nombreCapituloHijo) {
            if (vm.componentesRefresh.includes(nombreCapituloHijo)) {
                consultarFuentesSGR();
                consultarFuentesNoSGR();
            }
        }

        vm.CalcularTotalFuenteActiva = function () {
            vm.totalFuenteActiva = 0;

            if (vm.listaFuentesEtapa[0].Vigencias) {
                vm.listaFuentesEtapa[0].Vigencias.forEach(vigencia => {
                    if (vigencia.Entidades) {
                        vigencia.Entidades.forEach(entidad => {
                            if (entidad.Bienios) {
                                entidad.Bienios.forEach(bienio => {
                                    if (bienio) {
                                        vm.totalFuenteActiva += parseFloat(bienio.ValorSolicitado === undefined ? 0 : bienio.ValorSolicitado);
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

            if (vm.listaFuentes.Etapas[0].Vigencias) {
                vm.listaFuentes.Etapas[0].Vigencias.forEach(vigencia => {
                    if (vigencia.Entidades) {
                        vigencia.Entidades.forEach(entidad => {
                            if (entidad.Bienios) {
                                entidad.Bienios.forEach(bienio => {
                                    if (bienio) {
                                        vm.totalFuentePreInversion += parseFloat(bienio.ValorSolicitado === undefined ? 0 : bienio.ValorSolicitado);
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

            if (vm.listaFuentes.Etapas[1].Vigencias) {
                vm.listaFuentes.Etapas[1].Vigencias.forEach(vigencia => {
                    if (vigencia.Entidades) {
                        vigencia.Entidades.forEach(entidad => {
                            if (entidad.Bienios) {
                                entidad.Bienios.forEach(bienio => {
                                    if (bienio) {
                                        vm.totalFuenteInversion += parseFloat(bienio.ValorSolicitado === undefined ? 0 : bienio.ValorSolicitado);
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

            if (vm.listaFuentes.Etapas[2].Vigencias.Vigencias) {
                vm.listaFuentes.Etapas[2].Vigencias.Vigencias.forEach(vigencia => {
                    if (vigencia.Entidades) {
                        vigencia.Entidades.forEach(entidad => {
                            if (entidad.Bienios) {
                                entidad.Bienios.forEach(bienio => {
                                    if (bienio) {
                                        vm.totalFuenteOperacion += parseFloat(bienio.ValorSolicitado === undefined ? 0 : bienio.ValorSolicitado);
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

            if (vm.listaFuentes.Etapas[0].Vigencias) {
                vm.listaFuentes.Etapas[0].Vigencias.forEach(vigencia => {
                    if (vigencia.Costo) {
                        vm.totalCostosPreInversion += parseFloat(vigencia.Costo === undefined ? 0 : vigencia.Costo);
                    }
                });
            }
        }

        function CalcularTotalCostosInversion() {
            vm.totalCostosInversion = 0;

            if (vm.listaFuentes.Etapas[1].Vigencias) {
                vm.listaFuentes.Etapas[1].Vigencias.forEach(vigencia => {
                    if (vigencia.Costo) {
                        vm.totalCostosInversion += parseFloat(vigencia.Costo === undefined ? 0 : vigencia.Costo);
                    }
                });
            }
        }

        function CalcularTotalCostosOperacion() {
            vm.totalCostosOperacion = 0;

            if (vm.listaFuentes.Etapas[2].Vigencias) {
                vm.listaFuentes.Etapas[2].Vigencias.forEach(vigencia => {
                    if (vigencia.Costo) {
                        vm.totalCostosOperacion += parseFloat(vigencia.Costo === undefined ? 0 : vigencia.Costo);
                    }
                });
            }
        }

        vm.ValidarValorBienioCostos = function () {

            vm.totalFuentesBienio = 0;

            if (vm.listaFuentesEtapa[0].Vigencias) {
                for (let i = 0; i < vm.listaFuentesEtapa[0].Vigencias.length; i++) {
                    if (vm.listaFuentesEtapa[0].Vigencias[i].Entidades) {
                        for (var j = 0; j < vm.listaFuentesEtapa[0].Vigencias[i].Entidades.length; j++) {
                            if (vm.listaFuentesEtapa[0].Vigencias[i].Entidades[j].Bienios) {
                                for (var k = 0; k < vm.listaFuentesEtapa[0].Vigencias[i].Entidades[j].Bienios.length; k++) {
                                    vm.totalFuentesBienio += parseFloat(vm.listaFuentesEtapa[0].Vigencias[i].Entidades[j].Bienios[k].ValorSolicitado);
                                }
                            }
                            if (vm.totalFuentesBienio > vm.listaFuentesEtapa[0].Vigencias[i].Costo) {
                                return false;
                            }
                            vm.totalFuentesBienio = 0;
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

            if (vm.listaFuentesEtapa) {
                vm.listaFuentesEtapa.forEach(etapaSGR => {
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

                                //************ */

                                if (vm.listaFuentesNoSGR.Etapas) {
                                    vm.listaFuentesNoSGR.Etapas.forEach(etapaNoSGR => {
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
            if (vm.listaFuentes) {
                vm.listaFuentesEtapa = vm.listaFuentes.Etapas.filter(function (a) {
                    return a.EtapaId === vm.etapaId;
                });
            }
            //if (vm.entidadEtapa != "") {
            //    const entidadFilter = [];
            //    entidadFilter.push(vm.entidadEtapa);
            //    vm.listaFuentesEtapa = vm.listaFuentesEtapa.filter(d => d.Vigencias.every(c => c.Entidades.some(b => entidadFilter.includes(b.EntidadId))));
            //}
            //if (vm.tipoRecurso != "") {
            //    const tipoRecursoFilter = [];
            //    tipoRecursoFilter.push(vm.tipoRecurso);
            //    vm.listaFuentesEtapa = vm.listaFuentesEtapa.filter(d => d.Vigencias.every(c => c.Entidades.some(b => tipoRecursoFilter.includes(b.TipoRecursoId))));
            //}

            if (vm.listaFuentesEtapa[0].Vigencias) {
                vm.listaFuentesEtapa[0].Vigencias.forEach(vigencia => {
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

            vm.listaFuentesEtapa = null;
            vm.mensaje = "";
            vm.activar = true;
            vm.Editar = "EDITAR";
            vm.listaFuentes = angular.copy(vm.listaFuentesTemp);
            vm.totalFuenteActiva = 0;
            switch (origen) {
                case 1:
                    {
                        vm.nombreEtapa = "Preinversión";
                        if (vm.listaFuentes) {

                            if (vm.totalCostosPreInversion == 0) return;
                            vm.etapaId = origen;
                            vm.totalFuenteActiva = vm.totalFuentePreInversion;
                            vm.listaFuentesEtapa = vm.listaFuentes.Etapas.filter(function (a) {
                                return a.EtapaId === origen;
                            });
                        }
                        break;
                    }
                case 2:
                    {
                        vm.nombreEtapa = "Inversión";
                        if (vm.listaFuentes) {
                            if (vm.totalCostosInversion == 0) return;
                            vm.etapaId = origen;
                            vm.totalFuenteActiva = vm.totalFuenteInversion;
                            vm.listaFuentesEtapa = vm.listaFuentes.Etapas.filter(function (a) {
                                return a.EtapaId === origen;
                            });

                            vm.listaFuentesEtapa.map((etapa) => {
                                if (etapa.EtapaId === origen) {

                                    var vigencia = etapa.Vigencias[0].Vigencia;
                                    var bienioEncontrado = etapa.ComboBienios.find(function (bienio) {
                                        var inicioVigencia = parseInt(bienio.Bienio.split('-')[0]);
                                        var finVigencia = parseInt(bienio.Bienio.split('-')[1] || bienio.Bienio.split('-')[0]);
                                        return inicioVigencia <= vigencia && vigencia <= finVigencia;
                                    });

                                    var ComboBieniosFiltrados = bienioEncontrado ?
                                        etapa.ComboBienios.filter(function (_, index) {
                                            return index >= etapa.ComboBienios.indexOf(bienioEncontrado) && index < etapa.ComboBienios.indexOf(bienioEncontrado) + 4;
                                        }) :
                                        etapa.ComboBienios.slice();

                                    etapa.ComboBienios = ComboBieniosFiltrados;
                                }
                                return etapa;
                            });
                        }
                        break;
                    }
                case 3:
                    {
                        vm.nombreEtapa = "Operación";
                        if (vm.listaFuentes) {
                            if (vm.totalCostosOperacion == 0) return;
                            vm.etapaId = origen;
                            vm.totalFuenteActiva = vm.totalFuenteOperacion;
                            vm.listaFuentesEtapa = vm.listaFuentes.Etapas.filter(function (a) {
                                return a.EtapaId === origen;
                            });
                        }
                        break;
                    }
            }

            if (vm.listaFuentesEtapa[0].Vigencias) {
                vm.listaFuentesEtapa[0].Vigencias.forEach(function (vigencia, indexVig) {
                    if (vigencia.Entidades) {
                        vigencia.Entidades.forEach(function (entidad, indexEntidad) {
                            if (!entidad.Bienios) {
                                if (!vm.listaFuentesEtapa[0].Vigencias[indexVig].Entidades[indexEntidad].Bienios) {
                                    vm.listaFuentesEtapa[0].Vigencias[indexVig].Entidades[indexEntidad].Bienios = [];
                                    vm.AddVigencia = true;
                                }
                            }
                        });
                    }
                });
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

        vm.actualizaFila = function (event) {


            event.target.value = parseFloat(event.target.value.replace(".", ","));
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

        vm.actualizaFilaObjeto = function (event, objBienios) {
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

            objBienios.ValorSolicitado = parseFloat(event.target.value.replace(",", "."));
        }

        vm.notificacionValidacionPadre = function (errores) {
            console.log("Validación  - Fuentes SGR");
            vm.limpiarErrores();
            if (errores != undefined) {
                var erroresRelacionconlapl = errores.find(p => (p.Seccion + p.Capitulo) == vm.nombreComponente);

                if (erroresRelacionconlapl != undefined)
                    var erroresJson = erroresRelacionconlapl.Errores == "" ? [] : JSON.parse(erroresRelacionconlapl.Errores);
                else
                    var erroresJson = null;

                var isValid = (erroresJson == null || erroresJson.length == 0);
                if (!isValid) {
                    try {
                        erroresJson[vm.nombreComponente].forEach(p => {
                            var nameArr = p.Error.split('-');
                            var TipoError = nameArr[0].toString();
                            if (TipoError == 'CRCVSSSGR') {
                                vm.validarValoresVigencia('CRCVSSSGR', nameArr[1].toString(), nameArr[2].toString(), p.Descripcion);
                            }
                            if (TipoError == 'CRCVSSSGRTODAS') {
                                vm.validar('CRCVSSSGRTODAS', p.Descripcion);
                            }
                        });
                    }
                    catch (error) {
                        console.error('¡¡Tiene ERRORES - handlerValidacion del componente = ' + vm.handlerComponentes[i].componente + '!!');
                    }
                }
                vm.notificacionestado({ esValido: isValid, nombreComponente: vm.nombreComponente });
            }
        };

        vm.validarValoresVigencia = function (TipoError, etapa, vigencia, errores) {

            var campo = document.getElementById(TipoError + etapa + vigencia);
            if (campo != undefined) {
                campo.innerHTML = "<span class='d-inline-block ico-advertencia'></span><span>" + errores + "</span>";
                campo.classList.remove('hidden');
            }
        }

        vm.validar = function (TipoError, errores) {

            var campo = document.getElementById(TipoError);
            if (campo != undefined) {
                campo.innerHTML = "<span class='d-inline-block ico-advertencia'></span><span>" + errores + "</span>";
                campo.classList.remove('hidden');
            }
        }

        vm.limpiarErrores = function () {
            var campoTodas = document.getElementById('CRCVSSSGRTODAS');
            if (campoTodas != undefined) {
                campoTodas.innerHTML = "";
                campoTodas.classList.add('hidden');
            }
            if (vm.listaFuentesTemp != null && vm.listaFuentesTemp != undefined) {
                vm.listaFuentesTemp.Etapas.forEach(etapa => {
                    if (etapa.Vigencias) {
                        etapa.Vigencias.forEach(objVigencia => {
                            if (objVigencia.Vigencia) {
                                var campo = document.getElementById('CRCVSSSGR' + etapa.EtapaId + objVigencia.Vigencia);
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

        vm.modificarSelectDespuesDeCargar = function (listaFuentesEtapa) {
            var primeraVigencia = listaFuentesEtapa && listaFuentesEtapa[0] && listaFuentesEtapa[0].Vigencias && listaFuentesEtapa[0].Vigencias[0];

            var comboBienios = primeraVigencia.ComboBienios;

            var vigencia = primeraVigencia && primeraVigencia.vigencia;

            var primeraEntidad = primeraVigencia && primeraVigencia.Entidades && primeraVigencia.Entidades[0];

            var primerBienio = primeraEntidad && primeraEntidad.Bienios && primeraEntidad.Bienios[0];

            var bieniodId = primerBienio && primerBienio.BieniodId;
            var bienio = primerBienio && primerBienio.Bienio;

            if (bieniodId !== undefined && comboBienios > 0) {
                // Obtener el índice del bienio con BienioId específico en listadoBienios
                var indexBienio = comboBienios.findIndex(function (bienio) {
                    return bienio.BienioId === objBienios.BienioId;
                });

                // Si se encuentra el bienio en la lista
                if (indexBienio !== -1) {
                    // Eliminar los bienios anteriores al bienio con BienioId especificado
                    primeraVigencia.comboBienios.splice(0, indexBienio);
                    listaFuentesEtapa[0].Vigencias[0].comboBienios = primeraVigencia.comboBienios;
                }
            }

            return listaFuentesEtapa;
        };
    }

    angular.module('backbone').component('fuentesSgr', {
        templateUrl: "/src/app/formulario/ventanas/SGR/viabilidadSGR/previos/recursos/fuentes/fuentesSGR.html",

        controller: fuentesSgrController,
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
    });

})();