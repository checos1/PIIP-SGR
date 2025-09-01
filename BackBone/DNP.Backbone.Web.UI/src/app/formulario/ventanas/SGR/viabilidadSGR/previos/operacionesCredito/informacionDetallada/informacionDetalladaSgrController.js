(function () {
    'use strict';
    informacionDetalladaSgrController.$inject = [
        'utilidades',
        '$sessionStorage',
        'operacionesCreditoSgrServicio',
        'justificacionCambiosServicio',
        '$uibModal',
        'previosSgrServicio',
        '$timeout'
    ];

    function informacionDetalladaSgrController(
        utilidades,
        $sessionStorage,
        operacionesCreditoSgrServicio,
        justificacionCambiosServicio,
        $uibModal,
        previosSgrServicio,
        $timeout
    ) {
        var vm = this;
        vm.user = {};
        vm.lang = "es";
        vm.nombreComponente = "sgrviabilidadpreviosoperacioncreditoinformaciondetalladacredito";

        vm.disabled = false;
        vm.activar = true;
        vm.permiteEditar = false;
        vm.preguntaCredito = 3816;

        vm.abrirModalAgregarFuente = abrirModalAgregarFuente;

        vm.Bpin = $sessionStorage.idObjetoNegocio;
        vm.IdNivel = $sessionStorage.idNivel;
        vm.idInstancia = $sessionStorage.idInstancia;
        vm.idAccion = $sessionStorage.idAccion;
        vm.IdInstanciaViabiliad = $sessionStorage.IdInstanciaViabiliad
        vm.IdInstanciaCuestionario = ""
        vm.NivelPreviosId = ""
        vm.NivelCuestionario = ""
        vm.erroresComponente = [];
        vm.ProyectoId
        vm.ValoresCredito = [];
        vm.ValoresCreditoTemp = [];
        vm.ValorTotalCredito = 0;
        vm.CostoFinanciero = 0;
        vm.CostoPatrimonio = 0;

        vm.FuentesAdicionales = [];
        vm.FuentesAdicionalesTemp = [];

        vm.Valores = {};

        vm.SumaValorCredito = 0;
        vm.SumaCostoFinanciero = 0;
        vm.SumaCostoPatrimonio = 0;
        vm.SumaServicioDeuda = 0;
        vm.SumaValorFinanciar = 0;
        vm.flujoaprobacion = "";

        vm.ConvertirNumero = ConvertirNumero;

        vm.componentesRefresh = [
            "sgrviabilidadpreviosrecursosfuentessgr",
            'sgrviabilidadpreviosoperacioncreditoinformaciongeneralcredito'
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

                vm.flujoaprobacion = utilidades.obtenerParametroTransversal('FlujoAprobacion1');

                if (vm.flujoaprobacion !== $sessionStorage.InstanciaSeleccionada.FlujoId.toUpperCase()) {

                    vm.Bpin = $sessionStorage.InstanciaSeleccionada != undefined ? $sessionStorage.InstanciaSeleccionada.ProyectoId : "";
                    vm.IdInstanciaCuestionario = vm.idInstancia;
                    vm.NivelCuestionario = vm.IdNivel
                    vm.disabled = $sessionStorage.soloLectura;
                }
                else {

                    vm.Bpin = $sessionStorage.InstanciaSeleccionada.IdObjetoNegocio;
                    vm.IdInstanciaCuestionario = vm.IdInstanciaViabiliad;
                    vm.NivelCuestionario = utilidades.obtenerParametroTransversal('NivelPreviosId');

                    vm.permiteEditar = false;
                    vm.activar = true;
                    vm.disabled = true;
                }

                ObtenerRespuestaPreguntaCredito();
                ObtenerDetallesOperacionCredito();
            }
        }
        previosSgrServicio.registrarObservador(function (datos) {
            if (datos.Capitulo === 'sgrviabilidadpreviosdatosgeneralesdatospresentacion')
                ObtenerDetallesOperacionCredito();
            if (typeof datos.RespuestaCredito !== 'undefined') {
                vm.respuestaPreguntaCredito = datos.RespuestaCredito;
                vm.habilitaOperacionCredito = vm.respuestaPreguntaCredito == 1 && vm.Valores.FuentesCredito > 0;
                vm.disabled = $sessionStorage.soloLectura || !vm.habilitaOperacionCredito;
                // Product Backlog Item 66633: Gestión Integral y Validación de Datos de Operaciones de Crédito
                if (vm.respuestaPreguntaCredito == 2) {

                    vm.ValoresCredito = [];
                    vm.FuentesAdicionales = [];
                    vm.ValorTotalCredito = 0;
                    vm.CostoFinanciero = 0;
                    vm.CostoPatrimonio = 0;

                    eliminarCapitulosModificados();
                    vm.limpiarErrores();
                    vm.permiteEditar = false;
                    $("#EditarDG").html("EDITAR");
                    vm.activar = true;
                }
            }
        });

        function ObtenerDetallesOperacionCredito() {
            operacionesCreditoSgrServicio.obtenerOperacionCreditoDetalles(vm.Bpin, vm.idInstancia).then(
                function (response) {
                    if (response.data != null && response.data != "") {
                        vm.ProyectoId = response.data.ProyectoId;
                        vm.ValoresCredito = response.data.ValoresCredito;
                        vm.FuentesAdicionales = response.data.FuentesAdicionales;
                        vm.ValorTotalCredito = response.data.ValorTotalCredito;
                        vm.CostoFinanciero = response.data.CostoFinanciero;
                        vm.CostoPatrimonio = response.data.CostoPatrimonio;

                        vm.ValoresCredito = vm.ValoresCredito.reduce((e, o) => {
                            e.push({
                                ...o,
                                ValorCredito: ConvertirNumero(o.ValorCredito),
                                CostoFinanciero: ConvertirNumero(o.CostoFinanciero),
                                CostoPatrimonio: ConvertirNumero(o.CostoPatrimonio)
                            });
                            return e;
                        }, []);

                        vm.ValoresCreditoTemp = angular.copy(vm.ValoresCredito);

                        vm.FuentesAdicionales = vm.FuentesAdicionales.reduce((e, o) => {
                            e.push({
                                ...o,
                                CostoFinanciero: ConvertirNumero(o.CostoFinanciero),
                                CostoPatrimonio: ConvertirNumero(o.CostoPatrimonio)
                            });
                            return e;
                        }, []);

                        vm.FuentesAdicionalesTemp = angular.copy(vm.FuentesAdicionales);

                        CalcularTotales();
                    }
                }
            ).catch(error => {
                if (error.status == 400) {
                    utilidades.mensajeError(error.data.Message || "Error al realizar la operación");
                    return;
                }
                utilidades.mensajeError("Error al realizar la operación");
            });
            operacionesCreditoSgrServicio.obtenerOperacionCreditoDatosGenerales(vm.Bpin, vm.idInstancia).then(
                function (response) {
                    if (response.data != null && response.data != "") {
                        vm.Valores = response.data;
                        if (typeof vm.respuestaPreguntaCredito !== 'undefined') {
                            vm.habilitaOperacionCredito = vm.respuestaPreguntaCredito == 1 && vm.Valores.FuentesCredito > 0;
                            if (vm.flujoaprobacion !== $sessionStorage.InstanciaSeleccionada.FlujoId.toUpperCase()) {
                                vm.disabled = $sessionStorage.soloLectura || !vm.habilitaOperacionCredito;
                            }
                        }
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

        function ObtenerRespuestaPreguntaCredito() {
            previosSgrServicio.ConsultarAccionPorInstancia(vm.idInstancia, vm.idAccion).then(
                function (result) {
                    previosSgrServicio.obtenerPreguntasPersonalizadas(vm.Bpin, vm.NivelCuestionario, vm.IdInstanciaCuestionario, result.data.EstadoAccionPorInstanciaId).then(
                        function (respuesta) {
                            var respuestas = respuesta.data;
                            var Elemento = respuestas.PreguntasGenerales[0].Preguntas.find(x => x.IdPregunta == vm.preguntaCredito)
                            if (Elemento != undefined && Elemento instanceof Object) {
                                var index = respuestas.PreguntasGenerales[0].Preguntas.indexOf(Elemento)
                                if (typeof vm.Valores.FuentesCredito !== 'undefined') {
                                    vm.habilitaOperacionCredito = respuestas.PreguntasGenerales[0].Preguntas[index].Respuesta == 1 && vm.Valores.FuentesCredito > 0;
                                    if (vm.flujoaprobacion !== $sessionStorage.InstanciaSeleccionada.FlujoId.toUpperCase()) {
                                        vm.disabled = $sessionStorage.soloLectura || !vm.habilitaOperacionCredito;
                                    }
                                }
                            }
                            if (vm.erroresComponente != null && vm.erroresComponente != undefined) {
                                $timeout(function () {
                                    vm.notificacionValidacionPadre(vm.erroresComponente);
                                }, 600);
                            }
                        }
                    ).catch(error => {
                        if (error.status == 400) {
                            utilidades.mensajeError(error.data.Message || "Error al realizar la operación");
                            return;
                        }
                        utilidades.mensajeError("Error al obtener respuestas de preguntas personalizadas");
                    });
                }
            ).catch(error => {
                if (error.status == 400) {
                    utilidades.mensajeError(error.data.Message || "Error al realizar la operación");
                    return;
                }
                utilidades.mensajeError("Error al realizar la operación");
            });
        }

        function CalcularTotales() {

            vm.SumaValorCredito = 0;
            vm.SumaCostoFinanciero = 0;
            vm.SumaCostoPatrimonio = 0;
            vm.SumaServicioDeuda = 0;
            vm.SumaValorFinanciar = 0;

            for (var index = 0; index < vm.ValoresCredito.length; index++) {
                var valorCreditoFuente = ObtenerValor(vm.ValoresCredito[index].ValorCredito);
                var costoFinancieroFuente = ObtenerValor(vm.ValoresCredito[index].CostoFinanciero);
                var costoPatrimonioFuente = ObtenerValor(vm.ValoresCredito[index].CostoPatrimonio);

                vm.SumaValorCredito += valorCreditoFuente;
                vm.SumaCostoFinanciero += costoFinancieroFuente;
                vm.SumaCostoPatrimonio += costoPatrimonioFuente;
                vm.SumaServicioDeuda += (valorCreditoFuente + costoFinancieroFuente);
                vm.SumaValorFinanciar += (vm.ValoresCredito[index].ValorSolicitado + costoFinancieroFuente + costoPatrimonioFuente);
            }

            for (var index = 0; index < vm.FuentesAdicionales.length; index++) {
                var costoFinancieroFuenteAdicional = ObtenerValor(vm.FuentesAdicionales[index].CostoFinanciero);
                var costoPatrimonioFuenteAdicional = ObtenerValor(vm.FuentesAdicionales[index].CostoPatrimonio);

                vm.SumaCostoFinanciero += costoFinancieroFuenteAdicional;
                vm.SumaCostoPatrimonio += costoPatrimonioFuenteAdicional;
                vm.SumaServicioDeuda += costoFinancieroFuenteAdicional;
                vm.SumaValorFinanciar += (costoFinancieroFuenteAdicional + costoPatrimonioFuenteAdicional);
            }
        }

        vm.notificacionCambiosCapitulos = function (nombreCapituloHijo) {
            if (vm.componentesRefresh.includes(nombreCapituloHijo)) {
                ObtenerDetallesOperacionCredito();
            }
        }

        vm.ActivarEditar = function () {
            if (vm.activar == true) {
                vm.permiteEditar = true;
                $("#EditarID").html("CANCELAR");
                vm.activar = false;
            }
            else {
                utilidades.mensajeWarning("¿Está seguro de continuar?", function funcionContinuar() {
                    OkCancelar();

                    vm.permiteEditar = false;
                    $("#EditarID").html("EDITAR");
                    vm.activar = true;
                    vm.init();

                }, function funcionCancelar(reason) {
                    return;
                }, null, null, "Los datos que posiblemente haya diligenciado se perderán");
            }
        }

        function OkCancelar() {
            setTimeout(function () {
                utilidades.mensajeSuccess("", false, false, false, "La edición ha sido cancelada con éxito.");
            }, 500);
        }

        vm.actualizaValorCredito = function (event, index) {
            var sum = 0;
            var sumaValorCredito = 0;
            var costoFinanciero = 0;
            var valorCredito = 0;

            $(event.target).val(function (idx, value) {

                valorCredito = ObtenerValor(value);

                costoFinanciero = ObtenerValor(vm.ValoresCredito[index].CostoFinanciero);
                sum = valorCredito + costoFinanciero;
                vm.ValoresCredito[index].ValorServicioDeuda = sum;

                CalcularTotales();

                return value;
            });
        }

        vm.actualizaCostoFinanciero = function (event, index) {
            var sum = 0;
            var sumaCostoFinanciero = 0;
            var costoPatrimonio = 0;
            var valorCredito = 0;
            var costoFinanciero = 0;

            $(event.target).val(function (idx, value) {

                costoFinanciero = ObtenerValor(value);

                costoPatrimonio = ObtenerValor(vm.ValoresCredito[index].CostoPatrimonio);
                sum = vm.ValoresCredito[index].ValorSolicitado + costoFinanciero + costoPatrimonio;
                vm.ValoresCredito[index].ValorTotalCredito = sum;
                valorCredito = ObtenerValor(vm.ValoresCredito[index].ValorCredito);
                sum = costoFinanciero + valorCredito;
                vm.ValoresCredito[index].ValorServicioDeuda = sum;

                CalcularTotales();

                return value;
            });
        }

        vm.actualizaCostoPatrimonio = function (event,index) {
            var sum = 0;
            var sumaCostoPatrimonio = 0;
            var costoFinanciero = 0;
            var costoPatrimonio = 0;

            $(event.target).val(function (idx, value) {

                costoPatrimonio = ObtenerValor(value);

                costoFinanciero = ObtenerValor(vm.ValoresCredito[index].CostoFinanciero);
                sum = vm.ValoresCredito[index].ValorSolicitado + costoFinanciero + costoPatrimonio;
                vm.ValoresCredito[index].ValorTotalCredito = sum;

                CalcularTotales();

                return value;
            });
        }

        vm.actualizaCostoFinancieroAdicional = function (event, index) {
            var sum = 0;
            var sumaCostoFinanciero = 0;
            var costoPatrimonio = 0;
            var valorCredito = 0;
            var costoFinanciero = 0;

            $(event.target).val(function (idx, value) {

                costoFinanciero = ObtenerValor(value);

                costoPatrimonio = ObtenerValor(vm.FuentesAdicionales[index].CostoPatrimonio);
                sum = vm.FuentesAdicionales[index].ValorSolicitado + costoFinanciero + costoPatrimonio;
                vm.FuentesAdicionales[index].ValorTotalCredito = sum;
                valorCredito = ObtenerValor(vm.FuentesAdicionales[index].ValorCredito);
                sum = costoFinanciero + valorCredito;
                vm.FuentesAdicionales[index].ValorServicioDeuda = sum;

                CalcularTotales();

                return value;
            });
        }

        vm.actualizaCostoPatrimonioAdicional = function (event, index) {
            var sum = 0;
            var sumaCostoPatrimonio = 0;
            var costoFinanciero = 0;
            var costoPatrimonio = 0;

            $(event.target).val(function (idx, value) {

                costoPatrimonio = 0;

                costoFinanciero = ObtenerValor(vm.FuentesAdicionales[index].CostoFinanciero);
                sum = vm.FuentesAdicionales[index].ValorSolicitado + costoFinanciero + costoPatrimonio;
                vm.FuentesAdicionales[index].ValorTotalCredito = sum;

                CalcularTotales();

                return value;
            });
        }

        vm.guardar = function (response) {
            var ObjetoGuardar = crearObjetoGuardar();
            if (ValidarGuardar(ObjetoGuardar)) {
                Guardar(ObjetoGuardar);
            }
        }

        function ValidarGuardar(ObjetoGuardar) {
            var resultado = true;

            var ValorTotalCredito = vm.Valores.Criterios[3].Valor;
            var ValorFinanciero = vm.Valores.Criterios[4].Valor;
            var ValorPatrimonio = vm.Valores.Criterios[1].Valor;

            var valorCreditoFuente = 0;
            var valorFuente = 0;

            var parametros = {
                valorCreditoFuente: '',
                valorFuente: '',
                fuente: ''
            };

            // 66808 - Validación de valores de crédito por fuente/entidad
            for (let tipoRecurso of ObjetoGuardar.ValoresCredito) {
                valorCreditoFuente = ObtenerValor(tipoRecurso.ValorCredito);
                valorFuente = ObtenerValor(tipoRecurso.ValorSolicitado);
                if (valorCreditoFuente > valorFuente) {
                    parametros.valorCreditoFuente = ConvertirNumero(valorCreditoFuente);
                    parametros.valorFuente = ConvertirNumero(valorFuente);
                    parametros.fuente = tipoRecurso.TipoRecurso + ' - ' + tipoRecurso.Entidad;
                    utilidades.mensajeError(replaceParametersInTextContent('El valor del crédito {fuente} {valorCreditoFuente} no puede ser mayor al valor solicitado en cada fuente de financiación {valorFuente}.', parametros));
                    resultado = false;
                }
            }

            var SumaValorCredito = SumaValores(ObjetoGuardar.ValoresCredito, "ValorCredito")
            if (SumaValorCredito < ValorTotalCredito) {
                utilidades.mensajeError('La suma del valor crédito por fuente no puede ser menor al valor crédito del proyecto.');
                resultado = false;
            }
            if (SumaValorCredito > ValorTotalCredito) {
                utilidades.mensajeError('La suma del valor crédito por fuente no puede ser mayor al valor crédito del proyecto.');
                resultado = false;
            }

            var SumaCostoFinanciero = SumaValores(ObjetoGuardar.ValoresCredito, "CostoFinanciero") + SumaValores(ObjetoGuardar.FuentesAdicionales, "CostoFinanciero");
            if (SumaCostoFinanciero < ValorFinanciero) {
                utilidades.mensajeError('La suma del costo financiero por fuente no puede ser menor al costo financiero del proyecto.');
                resultado = false;
            }
            if (SumaCostoFinanciero > ValorFinanciero) {
                utilidades.mensajeError('La suma del costo financiero por fuente no puede ser menor al costo financiero del proyecto.');
                resultado = false;
            }

            var SumaCostoPatrimonio = SumaValores(ObjetoGuardar.ValoresCredito, "CostoPatrimonio") + SumaValores(ObjetoGuardar.FuentesAdicionales, "CostoPatrimonio");
            if (SumaCostoPatrimonio < ValorPatrimonio) {
                utilidades.mensajeError('La suma del costo patrimonio por fuente no puede ser menor al costo patrimonio del proyecto.');
                resultado = false;
            }
            if (SumaCostoPatrimonio > ValorPatrimonio) {
                utilidades.mensajeError('La suma del costo patrimonio por fuente no puede ser mayor al costo patrimonio del proyecto.');
                resultado = false;
            }

            return resultado;
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

        function Guardar(ObjetoGuardar) {
            return operacionesCreditoSgrServicio.guardarOperacionCreditoDetalles(ObjetoGuardar).then(
                function (response) {
                    if (response.data || response.statusText === "OK") {
                        parent.postMessage("cerrarModal", "*");
                        guardarCapituloModificado();
                        vm.limpiarErrores();
                        utilidades.mensajeSuccess("Operación realizada con éxito!", false, false, false);

                        vm.ValoresCreditoTemp = angular.copy(vm.ValoresCredito);
                        vm.FuentesAdicionales = angular.copy(vm.FuentesAdicionales);

                        vm.permiteEditar = false;
                        $("#EditarID").html("EDITAR");
                        vm.init();
                        vm.activar = true;
                    } else {
                        swal('', "Error al realizar la operación", 'error');
                    }
                }
            );
        }

        function crearObjetoGuardar() {
            var objetoGuardar = {};
            objetoGuardar.BPIN = vm.Bpin;
            objetoGuardar.ProyectoId = vm.ProyectoId;
            objetoGuardar.ValorTotalCredito = vm.ValorTotalCredito;
            objetoGuardar.CostoFinanciero = vm.CostoFinanciero;
            objetoGuardar.CostoPatrimonio = vm.CostoPatrimonio;

            var ValoresGuardar = angular.copy(vm.ValoresCredito);

            for (var index = 0; index < ValoresGuardar.length; index++) {
                var valorCredito = ObtenerValor(vm.ValoresCredito[index].ValorCredito);
                ValoresGuardar[index].ValorCredito = valorCredito;

                var costoFinanciero = ObtenerValor(vm.ValoresCredito[index].CostoFinanciero);
                ValoresGuardar[index].CostoFinanciero = costoFinanciero;

                var costoPatrimonio = ObtenerValor(vm.ValoresCredito[index].CostoPatrimonio);
                ValoresGuardar[index].CostoPatrimonio = costoPatrimonio;
            }

            objetoGuardar.ValoresCredito = ValoresGuardar;

            ValoresGuardar = angular.copy(vm.FuentesAdicionales);

            for (var index = 0; index < ValoresGuardar.length; index++) {
                var costoFinanciero = ObtenerValor(vm.FuentesAdicionales[index].CostoFinanciero);
                ValoresGuardar[index].CostoFinanciero = costoFinanciero;

                var costoPatrimonio = ObtenerValor(vm.FuentesAdicionales[index].CostoPatrimonio);
                ValoresGuardar[index].CostoPatrimonio = costoPatrimonio;
            }

            objetoGuardar.FuentesAdicionales = ValoresGuardar;

            return objetoGuardar;
        }

        function abrirModalAgregarFuente() {
            $uibModal.open({
                templateUrl: 'src/app/formulario/ventanas/SGR/viabilidadSGR/previos/operacionesCredito/informacionDetallada/informacionDetalladaSgrModalAgregarFuente.html',
                controller: 'informacionDetalladaSgrModalAgregarFuenteController',
            }).result.then(function (result) {
                vm.guardadoevent({ nombreComponenteHijo: vm.nombreComponente });
                vm.FuentesAdicionales.push(result);
            }, function (reason) {

            }), err => {
                toastr.error("Ocurrió un error al consultar el idAplicacion");
            };

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

        //para guardar los capitulos modificados y que se llenen las lunas
        function ObtenerSeccionCapitulo() {
            const span = document.getElementById('id-capitulo-' + vm.nombreComponente);
            vm.seccionCapitulo = span.textContent;
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

        function ObtenerValor(cadenaNumero) {
            if (typeof (cadenaNumero) == 'string') {
                return parseFloat(cadenaNumero.replaceAll(".", "").replace(",", "."));
            } else {
                return cadenaNumero;
            }
        }

        function SumaValores(arr, field) {
            var sum = 0;
            sum = arr
                .map(
                    (current) => typeof current[field] === 'string' ? parseFloat(current[field].replaceAll(".", "").replace(",", ".")) : current[field]
                    
                )
                .reduce(
                    (total, current) => total + current, 0
                );
            return sum;
        }

        function SumaOtros(arr, index, field) {
            var sum = 0;
            sum = arr
                .map(
                    (current) => typeof current[field] === 'string' ? parseFloat(current[field].replaceAll(".", "").replace(",", ".")) : current[field]
                )
                .reduce(
                    (total, current, i) => i !== index ? total + current : total, 0
                );
            return sum;
        }
        /* ------------------------ Validaciones ---------------------------------*/

        vm.notificacionValidacionPadre = function (errores) {
            vm.limpiarErrores();
            if (errores != undefined) {
                var erroresRelacionconlapl = errores.find(p => (p.Seccion + p.Capitulo) == vm.nombreComponente);
                var erroresJson = (erroresRelacionconlapl == undefined || erroresRelacionconlapl.Errores == "") ? [] : JSON.parse(erroresRelacionconlapl.Errores);
                var isValid = (erroresJson == null || erroresJson.length == 0);
                if (!isValid) {
                    erroresJson[vm.nombreComponente].forEach(p => {
                        var nameArr = p.Error.split('-');
                        var TipoError = nameArr[0].toString();
                        if (p.Error == 'OPCPRE1') {
                            vm.validarErroresPreguntas(p.Error, p.Descripcion, false);
                            vm.validarValores(p.Error, false);
                        } else {
                            vm.validarValores(p.Error, false);
                        }
                    });
                }
                vm.notificacionestado({ nombreComponente: vm.nombreComponente, esValido: isValid });
            }
        }

        vm.validarErroresPreguntas = function (error, Descripcion, esValido) {
            var campomensajeerror = document.getElementById(vm.nombreComponente + "-mensaje-error");
            if (campomensajeerror != undefined) {
                campomensajeerror.innerHTML = "<span class='d-inline-block ico-advertencia'></span><span>" + Descripcion + "</span>";
                campomensajeerror.classList.remove('hidden');
            }
        }

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

        vm.limpiarErrores = function () {
            var campomensajeerror = document.getElementById(vm.nombreComponente + "-mensaje-error");
            if (campomensajeerror != undefined) {
                campomensajeerror.innerHTML = "";
                campomensajeerror.classList.add('hidden');
            }
        }
    }

    angular.module('backbone').component('informacionDetalladaSgr', {
        templateUrl: "src/app/formulario/ventanas/SGR/viabilidadSGR/previos/operacionesCredito/informacionDetallada/informacionDetalladaSgr.html",
        controller: informacionDetalladaSgrController,
        controllerAs: "vm",
        bindings: {
            callback: '&',
            guardadoevent: '&',
            notificacionvalidacion: '&',
            notificacionestado: '&',
            notificacioncambios: '&',
            notificacioninicio: '&',
        }
    })
})();