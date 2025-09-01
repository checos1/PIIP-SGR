(function () {
    'use strict';
    registroAprobacionOpcSgrController.$inject = [
        'utilidades',
        '$sessionStorage',
        'aprobacionSgrServicio',
        'justificacionCambiosServicio',
    ];

    function registroAprobacionOpcSgrController(
        utilidades,
        $sessionStorage,
        aprobacionSgrServicio,
        justificacionCambiosServicio,
    ) {
        var vm = this;
        vm.user = {};
        vm.lang = "es";
        vm.nombreComponente = 'sgraprobacion1aprobacionopcregistroaprobacionopc';
        vm.nombreComponenteResumen = "sgraprobacion1aprobacionresumenaprobacionopc";

        //para guardar los capitulos modificados y que se llenen las lunas
        vm.seccionCapitulo = null;
        vm.seccionCapituloEstado = null;

        vm.Bpin = $sessionStorage.idObjetoNegocio;
        vm.IdNivel = $sessionStorage.idNivel;
        vm.idInstancia = $sessionStorage.idInstancia;
        vm.idAccion = $sessionStorage.idAccion;
        vm.proyectoId = $sessionStorage.proyectoId;
        vm.fechahoy = new Date();

        vm.ConvertirNumero = ConvertirNumero;
        vm.ObtenerNumero = ObtenerNumero;

        vm.disabled = false;
        vm.activar = true;
        vm.activarAprobado = true;
        vm.desactivar = true;
        vm.AddVigencia = false;

        vm.data;

        vm.listaRecursosEtapa = {};
        vm.listaRecursosEtapaTemp = {};

        vm.init = function () {
            obtenerListaRecursosEtapa();
            vm.notificacionvalidacion({ handler: vm.notificacionValidacionPadre, nombreComponente: vm.nombreComponente, esValido: true });
            vm.disabled = $sessionStorage.soloLectura;
            bloquearControles();
        }

        function bloquearControles() {
            vm.activar = true;
            vm.activarAprobado = true;
            vm.AddVigencia = true;
        }

        function obtenerListaRecursosEtapa() {

            var idEntidad = $sessionStorage.listadoAccionesTramite.find(x => x.Id == $sessionStorage.idAccion).IdEntidad;

            aprobacionSgrServicio.obtenerAprobacionProyectoCredito(vm.idInstancia, idEntidad).then(
                function (response) {
                    var respuestaJson = response.data;
                    vm.listaRecursosEtapa = respuestaJson;

                    if (Object.keys(vm.listaRecursosEtapa).length > 0) {

                        if (typeof vm.listaRecursosEtapa.IEstrategica !== "undefined") { vm.listaRecursosEtapa.IEstrategica = vm.listaRecursosEtapa.IEstrategica.toString(); }
                        if (typeof vm.listaRecursosEtapa.Aprobado !== "undefined") { vm.listaRecursosEtapa.Aprobado = vm.listaRecursosEtapa.Aprobado.toString(); }

                        vm.listaRecursosEtapa.TipoRecursos = vm.listaRecursosEtapa.TipoRecursos.reduce((e, o) => {
                            e.push({
                                ...o,
                                ValorSolicitado: ConvertirNumero(o.ValorSolicitado),
                                Bienios: o.Bienios.reduce((f, p) => {
                                    f.push({
                                        ...p,
                                        VrCreditoAprobado: ConvertirNumero(p.VrCreditoAprobado),
                                        VrCostoFinanciero: ConvertirNumero(p.VrCostoFinanciero),
                                        VrPatrimonioAutonomo: ConvertirNumero(p.VrPatrimonioAutonomo)
                                    });
                                    return f;
                                }, [])
                            });
                            return e;
                        }, []);
                    }

                    vm.listaRecursosEtapa.FechaAprobacion = vm.listaRecursosEtapa.FechaAprobacion ? new Date(vm.listaRecursosEtapa.FechaAprobacion) : null;
                    vm.listaRecursosEtapa.FechaActoAdmtvo = vm.listaRecursosEtapa.FechaActoAdmtvo ? new Date(vm.listaRecursosEtapa.FechaActoAdmtvo) : null;

                    vm.listaRecursosEtapaTemp = angular.copy(vm.listaRecursosEtapa);
                }
            ).catch(error => {
                if (error.status == 400) {
                    utilidades.mensajeError(error.data.Message || "Error al realizar la operación");
                    return;
                }
                utilidades.mensajeError("Error al realizar la operación");
            });
        }

        vm.ActivarEditar = function () {
            if (vm.activar == true) {
                $("#EditarOpcG").html("CANCELAR");
                vm.activar = false;

                if (typeof (vm.listaRecursosEtapa.Aprobado) !== 'undefined' && vm.listaRecursosEtapa.Aprobado != 2) {
                    vm.activarAprobado = false;
                }

                vm.AddVigencia = false;
            }
            else {
                utilidades.mensajeWarning("¿Está seguro de continuar?", function funcionContinuar() {
                    OkCancelar();

                    $("#EditarOpcG").html("EDITAR");
                    vm.activar = true;
                    vm.activarAprobado = true;
                    vm.AddVigencia = true;
                }, function funcionCancelar(reason) {
                    return;
                }, null, null, "Los datos que posiblemente haya diligenciado se perderán");
            }
        }

        vm.iEstrategicaChange = function () {
            if (vm.listaRecursosEtapa.IEstrategica == 2) { 
                vm.listaRecursosEtapa.TipoRecursos = vm.listaRecursosEtapa.TipoRecursos.reduce((e, o) => {
                    e.push({
                        ...o,
                        Bienios: o.Bienios.slice(0, 2)
                    });
                    return e;
                }, []);
            }
        }

        vm.aprobadoChange = function () {
            if (vm.listaRecursosEtapa.Aprobado == 2) {

                // borrar datos ingresados
                vm.listaRecursosEtapa.IEstrategica = 0
                vm.listaRecursosEtapa.FechaActoAdmtvo = null;

                vm.listaRecursosEtapa.TipoRecursos = vm.listaRecursosEtapa.TipoRecursos.reduce((e, o) => {
                    e.push({
                        ...o,
                        Bienios: o.Bienios.slice(0, 1)
                    });
                    return e;
                }, []);

                vm.listaRecursosEtapa.TipoRecursos = vm.listaRecursosEtapa.TipoRecursos.reduce((e, o) => {
                    e.push({
                        ...o,
                        Bienios: o.Bienios.reduce((f, p) => {
                            f.push({
                                ...p,
                                VrCreditoAprobado: 0,
                                VrCostoFinanciero: 0,
                                VrPatrimonioAutonomo: 0
                            });
                            return f;
                        }, [])
                    });
                    return e;
                }, []);

                vm.listaRecursosEtapaTemp = angular.copy(vm.listaRecursosEtapa);

                vm.activarAprobado = true;
            } else {
                vm.activarAprobado = false;
            }
        }

        vm.obtenerBienios = function() {
            var bienios = [];
            if (vm.listaRecursosEtapa.IEstrategica == 1) {
                bienios = vm.listaRecursosEtapa.ComboBienios.slice(0, 4);
            } else if (vm.listaRecursosEtapa.IEstrategica == 2) {
                bienios = vm.listaRecursosEtapa.ComboBienios.slice(0, 2);
            }
            return bienios;
        }

        vm.obtenerBienio = function (BienioId) {
            return vm.listaRecursosEtapa.ComboBienios.find(x => x.BienioId == BienioId).Bienio;
        }

        vm.AdicionarBienio = function (recursoIndex) {
            if ((vm.listaRecursosEtapa.IEstrategica == 1 && vm.listaRecursosEtapa.TipoRecursos[recursoIndex].Bienios.length < 4) || (vm.listaRecursosEtapa.IEstrategica == 2 && vm.listaRecursosEtapa.TipoRecursos[recursoIndex].Bienios.length < 2)) {
                var bienioIdActual = vm.listaRecursosEtapa.TipoRecursos[recursoIndex].Bienios[vm.listaRecursosEtapa.TipoRecursos[recursoIndex].Bienios.length - 1].BienioId;
                var nuevoBienioId = vm.listaRecursosEtapa.ComboBienios[vm.listaRecursosEtapa.ComboBienios.findIndex(x => x.BienioId == bienioIdActual) + 1].BienioId;
                vm.listaRecursosEtapa.TipoRecursos[recursoIndex].Bienios = [...vm.listaRecursosEtapa.TipoRecursos[recursoIndex].Bienios, { BienioId: nuevoBienioId, VrCreditoAprobado: 0, VrCostoFinanciero: 0, VrPatrimonioAutonomo: 0 }];
            }
        };

        vm.EliminarBienio = function (recursoIndex) {
            if (vm.listaRecursosEtapa.TipoRecursos[recursoIndex].Bienios.length > 1) {
                vm.listaRecursosEtapa.TipoRecursos[recursoIndex].Bienios.splice(-1, 1);
            }
        }

        function OkCancelar() {
            setTimeout(function () {
                utilidades.mensajeSuccess("", false, false, false, "La edición ha sido cancelada con éxito.");
                vm.listaRecursosEtapa = angular.copy(vm.listaRecursosEtapaTemp);
            }, 500);
        }

        function ConvertirNumero(numero) {
            if (typeof (numero) == 'number') {
                return new Intl.NumberFormat('es-co', {
                    minimumFractionDigits: 2,
                }).format(numero);
            } else {
                return numero;
            };
        }

        function ObtenerNumero(cadenaNumero) {
            if (typeof (cadenaNumero) == 'string') {
                return parseFloat(cadenaNumero.replaceAll(".", "").replace(",", "."));
            } else {
                return cadenaNumero;
            }
        }

        vm.Guardar = function () {
            if (!validar()) {
                return;
            }
            else {
                Guardar();
            }

        }

        function validar() {

            vm.limpiarErrores();

            var valida = true;
            var MensajePreguntaAprobado = document.getElementById('MensajePreguntaAprobado');
            var MensajePreguntaIEstrategica = document.getElementById('MensajePreguntaIEstrategica');
            var MensajeFechaAprobacion = document.getElementById('MensajeFechaAprobacion');
            var MensajeValorCreditoIgualASolicitado = document.getElementById('MensajeValorCreditoIgualASolicitado');
            var MensajeCostoFinancieroIgualASolicitado = document.getElementById('MensajeCostoFinancieroIgualASolicitado');
            var MensajePatrimonioAutonomoIgualASolicitado = document.getElementById('MensajePatrimonioAutonomoIgualASolicitado');
            var MensajeFechaActoAdmtvo = document.getElementById('MensajeFechaActoAdmtvo');
            var MensajeFechaActoAdmtvoVsFechaAprobacion = document.getElementById('MensajeFechaActoAdmtvoVsFechaAprobacion');
            const MensajeErrorValorCredito = 'La sumatoria de los valores del crédito {sumaValorCredito} debe ser igual al valor diligenciado para la operación de crédito de la fuente "{fuente}" por valor de {valorSolicitadoCredito}';
            const MensajeErrorCostoFinanciero = 'La sumatoria de los valores del costo financiero por {sumaCostoFinanciero} debe ser igual al valor diligenciado para la operación de crédito de la fuente "{fuente}" por valor de {valorSolicitadoCostoFinanciero}';
            const MensajeErrorPatrimonioAutonomo = 'La sumatoria de los valores del patrimonio autónomo por {sumaPatrimonioAutonomo} debe ser igual al valor diligenciado para la operación de crédito de la fuente "{fuente}" por valor de {valorSolicitadoPatrimonioAutonomo}';
            var diferenciaDias = 0;

            // no se ha respondido la pregunta de aprobación
            if (vm.listaRecursosEtapa.Aprobado === null || vm.listaRecursosEtapa.Aprobado === undefined || vm.listaRecursosEtapa.Aprobado === '' || vm.listaRecursosEtapa.Aprobado === '0') {
                if (MensajePreguntaAprobado != undefined) {
                    MensajePreguntaAprobado.classList.remove('hidden');
                }
                valida = false;
            }
            else {
                // pregunta de aprobación respondida
                if (MensajePreguntaAprobado != undefined && !MensajePreguntaAprobado.classList.contains("hidden")) {
                    MensajePreguntaAprobado.classList.add('hidden');
                }
                // aprobado
                if (vm.listaRecursosEtapa.Aprobado == 1) {

                    if (vm.listaRecursosEtapa.IEstrategica === null || vm.listaRecursosEtapa.IEstrategica === undefined || vm.listaRecursosEtapa.IEstrategica === '' || vm.listaRecursosEtapa.IEstrategica == 0) {
                        if (MensajePreguntaIEstrategica != undefined) {
                            MensajePreguntaIEstrategica.classList.remove('hidden');
                        }
                        valida = false;
                    }
                    else {
                        if (MensajePreguntaIEstrategica != undefined && !MensajePreguntaIEstrategica.classList.contains("hidden")) {
                            MensajePreguntaIEstrategica.classList.add('hidden');
                        }
                    }

                } else {
                    if (MensajePreguntaIEstrategica != undefined && !MensajePreguntaIEstrategica.classList.contains("hidden")) {
                        MensajePreguntaIEstrategica.classList.add('hidden');
                    }
                }
            }

            // la fecha de aprobación es obligatoria sin importar si es aprobado o no
            if (vm.listaRecursosEtapa.FechaAprobacion === null || vm.listaRecursosEtapa.FechaAprobacion === undefined || vm.listaRecursosEtapa.FechaAprobacion === "") {
                if (MensajeFechaAprobacion != undefined && MensajeFechaAprobacion.classList.contains("hidden")) {
                    MensajeFechaAprobacion.classList.remove('hidden');
                }
                valida = false;
            }
            else {
                // la fecha de aprobación no puede ser superior a la actual
                diferenciaDias = diferenciaEnDias(vm.fechahoy, vm.listaRecursosEtapa.FechaAprobacion);
                if (diferenciaDias > 0) {
                    if (MensajeFechaAprobacion && MensajeFechaAprobacion.classList.contains("hidden")) {
                        MensajeFechaAprobacion.classList.remove('hidden');
                    }
                    valida = false;
                } else {
                    if (MensajeFechaAprobacion && !MensajeFechaAprobacion.classList.contains("hidden")) {
                        MensajeFechaAprobacion.classList.add('hidden');
                    }
                }
            }

            if (typeof (vm.listaRecursosEtapa.FechaActoAdmtvo) !== 'undefined' && vm.listaRecursosEtapa.FechaActoAdmtvo !== undefined && vm.listaRecursosEtapa.FechaActoAdmtvo !== null && vm.listaRecursosEtapa.FechaActoAdmtvo !== "")
            {
                // la fecha de acto administrativo no puede ser superior a la actual
                diferenciaDias = diferenciaEnDias(vm.fechahoy, vm.listaRecursosEtapa.FechaActoAdmtvo);
                if (diferenciaDias > 0) {
                    if (MensajeFechaActoAdmtvo && MensajeFechaActoAdmtvo.classList.contains("hidden")) {
                        MensajeFechaActoAdmtvo.classList.remove('hidden');
                    }
                    valida = false;
                }
                else {
                    if (MensajeFechaActoAdmtvo && !MensajeFechaActoAdmtvo.classList.contains("hidden")) {
                        MensajeFechaActoAdmtvo.classList.add('hidden');
                    }
                }
                // la fecha de acto administrativo no puede ser inferior a la fecha de aprobación
                diferenciaDias = diferenciaEnDias(vm.listaRecursosEtapa.FechaAprobacion, vm.listaRecursosEtapa.FechaActoAdmtvo);
                if (diferenciaDias < 0) {
                    if (MensajeFechaActoAdmtvoVsFechaAprobacion && MensajeFechaActoAdmtvoVsFechaAprobacion.classList.contains("hidden")) {
                        MensajeFechaActoAdmtvoVsFechaAprobacion.classList.remove('hidden');
                    }
                    valida = false;
                }
                else {
                    if (MensajeFechaActoAdmtvoVsFechaAprobacion && !MensajeFechaActoAdmtvoVsFechaAprobacion.classList.contains("hidden")) {
                        MensajeFechaActoAdmtvoVsFechaAprobacion.classList.add('hidden');
                    }
                }
            }

            //limpiar
            if (MensajeValorCreditoIgualASolicitado != undefined) {
                removeAllSpanChildren(MensajeValorCreditoIgualASolicitado);
                MensajeValorCreditoIgualASolicitado.classList.add("hidden");
            }
            if (MensajeCostoFinancieroIgualASolicitado != undefined) {
                removeAllSpanChildren(MensajeCostoFinancieroIgualASolicitado);
                MensajeCostoFinancieroIgualASolicitado.classList.add("hidden");
            }
            if (MensajePatrimonioAutonomoIgualASolicitado != undefined) {
                removeAllSpanChildren(MensajePatrimonioAutonomoIgualASolicitado);
                MensajePatrimonioAutonomoIgualASolicitado.classList.add("hidden");
            }

            if (vm.listaRecursosEtapa.Aprobado !== null && typeof (vm.listaRecursosEtapa.Aprobado) !== 'undefined' && vm.listaRecursosEtapa.Aprobado == 1) {

                var valorSolicitadoCredito = 0;
                var sumaValorCredito = 0;
                var valorSolicitadoCostoFinanciero = 0;
                var sumaCostoFinanciero = 0;
                var valorSolicitadoPatrimonioAutonomo = 0;
                var sumaPatrimonioAutonomo = 0;

                var parametros = {
                    valorSolicitadoCredito: '',
                    sumaValorCredito: '',
                    valorSolicitadoCostoFinanciero: '',
                    sumaCostoFinanciero: '',
                    fuente: ''
                };

                for (let tipoRecurso of vm.listaRecursosEtapa.TipoRecursos) {

                    parametros.fuente = tipoRecurso.TipoRecurso;

                    sumaValorCredito = vm.listaRecursosEtapa.TipoRecursos.reduce((s, i) => {
                        return i.TipoRecursoId == tipoRecurso.TipoRecursoId ? s + i.Bienios.reduce((t, j) => {
                            return t + ObtenerNumero(j.VrCreditoAprobado);
                        }, 0) : s;
                    }, 0);

                    valorSolicitadoCredito = ObtenerNumero(tipoRecurso.ValorCredito);
                    if (sumaValorCredito != valorSolicitadoCredito) {
                        valida = false;
                        if (MensajeValorCreditoIgualASolicitado != undefined) {
                            parametros.valorSolicitadoCredito = ConvertirNumero(valorSolicitadoCredito);
                            parametros.sumaValorCredito = ConvertirNumero(sumaValorCredito);
                            let mensajeError = replaceParametersInTextContent(MensajeErrorValorCredito, parametros);
                            agregarError(MensajeValorCreditoIgualASolicitado, mensajeError);
                            MensajeValorCreditoIgualASolicitado.classList.remove('hidden');
                        }
                    }

                    sumaCostoFinanciero = vm.listaRecursosEtapa.TipoRecursos.reduce((s, i) => {
                        return i.TipoRecursoId == tipoRecurso.TipoRecursoId ? s + i.Bienios.reduce((t, j) => {
                            return t + ObtenerNumero(j.VrCostoFinanciero);
                        }, 0) : s;
                    }, 0);

                    valorSolicitadoCostoFinanciero = ObtenerNumero(tipoRecurso.CostoFinanciero);
                    if (sumaCostoFinanciero != valorSolicitadoCostoFinanciero) {
                        valida = false;
                        if (MensajeCostoFinancieroIgualASolicitado != undefined) {
                            parametros.valorSolicitadoCostoFinanciero = ConvertirNumero(valorSolicitadoCostoFinanciero);
                            parametros.sumaCostoFinanciero = ConvertirNumero(sumaCostoFinanciero);
                            let mensajeError = replaceParametersInTextContent(MensajeErrorCostoFinanciero, parametros);
                            agregarError(MensajeCostoFinancieroIgualASolicitado, mensajeError);
                            MensajeCostoFinancieroIgualASolicitado.classList.remove('hidden');
                        }
                    }

                    sumaPatrimonioAutonomo = vm.listaRecursosEtapa.TipoRecursos.reduce((s, i) => {
                        return i.TipoRecursoId == tipoRecurso.TipoRecursoId ? s + i.Bienios.reduce((t, j) => {
                            return t + ObtenerNumero(j.VrPatrimonioAutonomo);
                        }, 0) : s;
                    }, 0);

                    valorSolicitadoPatrimonioAutonomo = ObtenerNumero(tipoRecurso.PatrimonioAutonomo);
                    if (sumaPatrimonioAutonomo != valorSolicitadoPatrimonioAutonomo) {
                        valida = false;
                        if (MensajePatrimonioAutonomoIgualASolicitado != undefined) {
                            parametros.valorSolicitadoPatrimonioAutonomo = ConvertirNumero(valorSolicitadoPatrimonioAutonomo);
                            parametros.sumaPatrimonioAutonomo = ConvertirNumero(sumaPatrimonioAutonomo);
                            let mensajeError = replaceParametersInTextContent(MensajeErrorPatrimonioAutonomo, parametros);
                            agregarError(MensajePatrimonioAutonomoIgualASolicitado, mensajeError);
                            MensajePatrimonioAutonomoIgualASolicitado.classList.remove('hidden');
                        }
                    }
                }
            }

            return valida;
        }

        function diferenciaEnDias(fechaInicio, fechaFin) {
            const unDiaEnMs = 1000 * 60 * 60 * 24;

            // Convertir a fechas UTC en medianoche
            const utcInicio = Date.UTC(fechaInicio.getFullYear(), fechaInicio.getMonth(), fechaInicio.getDate());
            const utcFin = Date.UTC(fechaFin.getFullYear(), fechaFin.getMonth(), fechaFin.getDate());

            // Calcular la diferencia en milisegundos y convertir a días
            return Math.floor((utcFin - utcInicio) / unDiaEnMs);
        }

        function removeAllSpanChildren(element) {
            if (!element) return;
            element.querySelectorAll('span').forEach(span => span.remove());
        }

        function replaceParametersInTextContent(text, params) {

            let result = text;

            // Replace each parameter in the textContent
            for (const [key, value] of Object.entries(params)) {
                const placeholder = `{${key}}`;
                result = result.replaceAll(placeholder, value);
            }

            return result;
        }

        function agregarError(elemento, mensaje) {
            // Create a new span element
            const newSpan = document.createElement('span');

            // Set the text content of the span
            newSpan.textContent = mensaje;

            // Optionally, add a class or other attributes to the span
            newSpan.className = 'messagealerttableDNP';
            newSpan.setAttribute('style', 'text-align: left;font-family: \'Work Sans\'; font-size: 14px; font-style: normal; display: block;');

            // Append the span to the target div
            elemento.appendChild(newSpan);
        }

        function Guardar() {

            var aprobacionProyectoCredito = angular.copy(vm.listaRecursosEtapa);

            aprobacionProyectoCredito.TipoRecursos = aprobacionProyectoCredito.TipoRecursos.reduce((e, o) => {
                e.push({
                    ...o,
                    ValorSolicitado: ObtenerNumero(o.ValorSolicitado),
                    Bienios: o.Bienios.reduce((f, p) => {
                        f.push({
                            ...p,
                            VrCreditoAprobado: ObtenerNumero(p.VrCreditoAprobado),
                            VrCostoFinanciero: ObtenerNumero(p.VrCostoFinanciero),
                            VrPatrimonioAutonomo: ObtenerNumero(p.VrPatrimonioAutonomo)
                        });
                        return f;
                    }, [])
                });
                return e;
            }, []);

            return aprobacionSgrServicio.guardarAprobacionProyectoCredito(aprobacionProyectoCredito).then(
                function (response) {
                    if (response.data || response.statusText === "OK") {
                        guardarCapituloModificado();
                        utilidades.mensajeSuccess("Operación realizada con éxito!", false, false, false);

                        vm.listaRecursosEtapaTemp = angular.copy(aprobacionProyectoCredito);

                        $("#EditarOpcG").html("EDITAR");
                        vm.activar = true;
                        vm.activarAprobado = true;
                        vm.AddVigencia = true;
                        //vm.limpiarErrores();
                    } else {
                        swal('', "Error al realizar la operación", 'error');
                    }
                }
            ).catch(error => {
                if (error.status == 400) {
                    utilidades.mensajeError(error.data.Message || "Error al realizar la operación");
                    return;
                }
                utilidades.mensajeError("Error al realizar la operación");
            });
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
                        vm.guardadoevent({ nombreComponenteHijo: vm.nombreComponente });
                    }
                    else {
                        utilidades.mensajeError(response.data.Mensaje + " Capitulo Modificado");
                    }
                });
        }

        function guardarCapituloModificadoResumen() {
            ObtenerSeccionCapituloResumen();
            var data = {
                ProyectoId: $sessionStorage.InstanciaSeleccionada.ProyectoId,
                Justificacion: "",
                SeccionCapituloId: vm.seccionCapituloEstado,
                InstanciaId: vm.idInstancia,
                Modificado: 1,
                cuenta: 1
            }
            justificacionCambiosServicio.guardarCambiosFirme(data)
                .then(function (response) {
                    if (response.data.Exito) {
                        vm.guardadoevent({ nombreComponenteHijo: vm.nombreComponenteResumen });
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
        }
        function ObtenerSeccionCapituloResumen() {
            const span = document.getElementById('id-capitulo-' + vm.nombreComponenteResumen);
            vm.seccionCapituloEstado = span.textContent;
        }

        /* ------------------------ Validaciones ---------------------------------*/

        vm.notificacionValidacionPadre = function (errores) {
            vm.limpiarErrores();
            if (errores != undefined) {
                var erroresRelacionconlapl = errores.find(p => (p.Seccion + p.Capitulo) == vm.nombreComponente);
                var isValid = true;
                if (erroresRelacionconlapl != undefined) {
                    var erroresJson = erroresRelacionconlapl.Errores == "" ? [] : JSON.parse(erroresRelacionconlapl.Errores);
                    isValid = (erroresJson == null || erroresJson.length == 0);
                    if (!isValid) {
                        erroresJson[vm.nombreComponente].forEach(p => {
                            var nameArr = p.Error.split('-');
                            var TipoError = nameArr[0].toString();
                            vm.validarSeccion(TipoError,p.Descripcion, false);
                        });
                    }
                }
                vm.notificacionestado({ nombreComponente: vm.nombreComponente, esValido: isValid }); 

            }
        }

        vm.validarSeccion = function (tipoError, errores, esValido) {
            var campomensajeerror = document.getElementById(tipoError);
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
            var errorElements = document.getElementsByClassName('errorSeccionInformacionGeneralViabilidad');
            var testDivs = Array.prototype.filter.call(errorElements, function (errorElement) {
                errorElement.innerHTML = "";
                errorElement.classList.add('hidden');
            });

            errorElements = document.getElementsByClassName('messagealerttableDNP');
            testDivs = Array.prototype.filter.call(errorElements, function (errorElement) {
                if (!errorElement.parentElement.classList.contains('hidden')) {
                    errorElement.parentElement.classList.add('hidden');
                }
            });
        }
    }


    angular.module('backbone').component('registroAprobacionOpcSgr', {
        templateUrl: "/src/app/formulario/ventanas/SGR/gestionRecursos/aprobacion/F1/aprobacion/credito/registroAprobacionOpcSgr.html",
        controller: registroAprobacionOpcSgrController,
        controllerAs: "vm",
        bindings: {
            callback: '&',
            guardadoevent: '&',
            notificacionvalidacion: '&',
            notificacionestado: '&',
            namecomponent: '<'
        }
    })       
})();