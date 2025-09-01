(function () {
    'use strict';
    valorEstructuracionSgrController.$inject = [
        'utilidades',
        '$sessionStorage',
        'justificacionCambiosServicio',
        'designarEjecutorSgrServicio'
    ];

    function valorEstructuracionSgrController(
        utilidades,
        $sessionStorage,
        justificacionCambiosServicio,
        designarEjecutorSgrServicio
    ) {
        var vm = this;
        vm.user = {};
        vm.lang = "es";
        vm.nombreComponente = 'sgrejecutordesignacioncostosvalorestructuracion';
        vm.seccionCapitulo = null; //Para guardar los capitulos modificados y que se llenen las lunas
        vm.activar = true;
        vm.desactivar = true;
        vm.estadoBotonEdit = 'EDITAR';

        //Variables Escenciales
        vm.idInstancia = $sessionStorage.idInstancia;
        vm.proyectoId = $sessionStorage.proyectoId;

        //Variables
        vm.valoresAprobacion = [];
        vm.valoresAprobacionTemp = [];
        vm.totalValorAprobado = 0;
        vm.totalValorEstructuracion = 0;
        vm.valorEstructuracionViabilidad = 0;

        vm.init = function () {
            vm.notificacionvalidacion({ handler: vm.notificacionValidacionPadre, nombreComponente: vm.nombreComponente, esValido: true });
            vm.disabled = $sessionStorage.soloLectura;
        };

        // NOTA: controlar cada parámetro.
        designarEjecutorSgrServicio.registrarObservador(function (datos) {
            //Validacion dato recCostos
            if (datos.recCostos === true) {
                vm.tipoVisualizacion = 'tipo2';
                ConsultaValoresAprobacion();
            } else if (datos.recCostos === false) {
                vm.tipoVisualizacion = 'tipo1';
                eliminarCapitulosModificados();
            }

            //Validacion dato aplicaCostos
            if (datos.aplicaCostos === false) {
                ConsultaValoresAprobacion();
                GuardarValoresEstructuracion('No');
            }
        });

        function ConsultaValoresAprobacion() {
            designarEjecutorSgrServicio.LeerValoresAprobacionSGR(vm.proyectoId)
                .then(function (response) {
                    if (response.data != null && response.data != "") {
                        let datosProcesados = JSON.parse(response.data).map(item => ({
                            Id: item.Id,
                            Etapa: item.Etapa,
                            TipoEnEntidad: item.TipoEnEntidad,
                            TipoRecurso: item.TipoRecurso,
                            Bienio: item.Bienio,
                            ValorAprobado: item.ValorAprobado,
                            ValorEstructuracion: item.ValorEstructuracion
                        }));

                        vm.valoresAprobacion = datosProcesados;
                        vm.totalValorAprobado = vm.valoresAprobacion.reduce((sum, item) => sum + item.ValorAprobado, 0);
                        vm.totalValorEstructuracion = vm.valoresAprobacion.reduce((sum, item) => sum + item.ValorEstructuracion, 0);
                        ValidarValoresEstructuracionViabilidad();
                    }
                }).catch(function (error) {
                    utilidades.mensajeError('Error en el capitulo "Valor de la Estructuración" - servicio "LeerValoresAprobacionSGR".', error);
                    console.error('Error en el capitulo "Valor de la Estructuración" - servicio "LeerValoresAprobacionSGR" - Error:', error);
                });
        };

        vm.actualizarValor = function (itemEditado) {
            itemEditado.ValorEstructuracion = itemEditado.ValorEstructuracion.replace(/[^0-9.]/g, '');
            const partes = itemEditado.ValorEstructuracion.split('.');

            if (partes.length > 2) {
                itemEditado.ValorEstructuracion = partes[0] + '.' + partes.slice(1).join('');
            }

            if (partes.length === 2) {
                partes[1] = partes[1].substring(0, 2);
                itemEditado.ValorEstructuracion = partes[0] + '.' + partes[1];
            }

            const estructuracion = parseFloat(itemEditado.ValorEstructuracion);
            const aprobado = parseFloat(itemEditado.ValorAprobado);

            if (!isNaN(estructuracion) && !isNaN(aprobado) && estructuracion > aprobado) {
                utilidades.mensajeError("El valor de estructuración no puede ser mayor al valor aprobado.", false);
                itemEditado.ValorEstructuracion = 0.00; ///aprobado.toFixed(2);
            }

            vm.totalValorEstructuracion = parseFloat(
                vm.valoresAprobacion.reduce((sum, item) => {
                    const val = parseFloat(item.ValorEstructuracion);
                    return sum + (isNaN(val) ? 0 : val);
                }, 0).toFixed(2)
            );
        };

        vm.ActivarEditar = function () {
            if (vm.activar == true) {
                vm.valoresAprobacionTemp = angular.copy(vm.valoresAprobacion);
                vm.estadoBotonEdit = 'CANCELAR';
                vm.activar = false;
            }
            else {
                utilidades.mensajeWarning("¿Está seguro de continuar?",
                    function funcionContinuar() {
                        OkCancelar();
                        vm.estadoBotonEdit = 'EDITAR';
                        vm.activar = true;
                        vm.valoresAprobacion = angular.copy(vm.valoresAprobacionTemp);
                    }, function funcionCancelar() {
                        return;
                    },
                    null, null, "Los datos que posiblemente haya diligenciado se perderán.");
            }
        };

        function OkCancelar() {
            setTimeout(function () {
                utilidades.mensajeSuccess("", false, false, false, "La edición ha sido cancelada con éxito.");
            }, 500);
        };

        vm.guardar = function () {
            if (vm.totalValorEstructuracion == 0) {
                utilidades.mensajeError('El total de "Valor de la estructuración" debe ser mayor a cero.', false);
                return false;
            }

            if (parseFloat(vm.totalValorEstructuracion) !== parseFloat(vm.valorEstructuracionViabilidad)) {
                const valorEstructuracionViabilidadCop = new Intl.NumberFormat('es-CO', {
                    style: 'currency',
                    currency: 'COP'
                }).format(vm.valorEstructuracionViabilidad);

                const totalValorEstructuracionCop = new Intl.NumberFormat('es-CO', {
                    style: 'currency',
                    currency: 'COP'
                }).format(vm.totalValorEstructuracion);

                utilidades.mensajeWarning('El valor total de la estructuración ' + totalValorEstructuracionCop + ' NO corresponde a los valores diligenciados en los conceptos emitidos para el proyecto en el proceso de viabilidad ' + valorEstructuracionViabilidadCop + '.',
                    function funcionContinuar() {
                        GuardarValoresEstructuracion('Ok').then(function (ok1) {
                            if (!ok1)
                                return handleGuardarError();
                            else {
                                vm.valoresAprobacionTemp = angular.copy(vm.valoresAprobacion);
                                return handleGuardarExito();
                            }
                        });
                    }, function funcionCancelar() {
                        return;
                    },
                    null, null, "Advertencia");
            } else {
                GuardarValoresEstructuracion('Ok').then(function (ok1) {
                    if (!ok1)
                        return handleGuardarError();
                    else {
                        vm.valoresAprobacionTemp = angular.copy(vm.valoresAprobacion);
                        return handleGuardarExito();
                    }
                });
            }
        };

        function ValidarValoresEstructuracionViabilidad() {
            designarEjecutorSgrServicio.ObtenerValorCostosEstructuracionViabilidadSGR(vm.idInstancia).then(
                function (response) {
                    if (response.data != null && response.data != "")
                        vm.valorEstructuracionViabilidad = parseFloat(response.data);
                    else
                        valorEstructuracionViabilidad = 0.00;
                }
            ).catch(function (error) {
                utilidades.mensajeError('Error en el capitulo "Valor de la Estructuración" - servicio "ObtenerValorCostosEstructuracionViabilidadSGR".');
                console.error('Error en el capitulo "Valor de la Estructuración" - servicio "ObtenerValorCostosEstructuracionViabilidadSGR" - Error:', error);
                return false;
            });
        };

        function GuardarValoresEstructuracion(uso) {
            let datosAEnviar = [];

            if (uso == 'No') {
                datosAEnviar = {
                    campo: 'ValorEstructuracion',
                    listaValores: vm.valoresAprobacion.map(item => ({
                        Id: item.Id,
                        Valor: 0
                    }))
                };
            } else {
                datosAEnviar = {
                    campo: 'ValorEstructuracion',
                    listaValores: vm.valoresAprobacion.map(item => ({
                        Id: item.Id,
                        Valor: parseFloat(item.ValorEstructuracion) || 0
                    }))
                };
            }

            return designarEjecutorSgrServicio.ActualizarValorEjecutorSGR(datosAEnviar)
                .then(function (response) {
                    return !!response.data;
                })
                .catch(function (error) {
                    return false;
                });
        };

        function handleGuardarExito() {
            guardarCapituloModificado();
            utilidades.mensajeSuccess('', false, false, false);
            vm.estadoBotonEdit = 'EDITAR';
            vm.activar = true;
        };

        function handleGuardarError() {
            utilidades.mensajeError('Error al guardar capítulo "Valor de la Estructuración".', false);
        };

        /* ------------------------ Validaciones ---------------------------------*/
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
        };

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
        };

        function ObtenerSeccionCapitulo() {
            const span = document.getElementById('id-capitulo-' + vm.nombreComponente);
            vm.seccionCapitulo = span.textContent;
        };

        vm.notificacionValidacionPadre = function (errores) {
            //Remplazar por cada capitulo
            var tipError = 'VALEST';
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
                            if (TipoError == tipError) {
                                vm.validarSeccion(TipoError, nameArr[1].toString(), p.Descripcion, false);
                            }
                            else {
                                vm.validarValores(nameArr[0].toString(), p.Descripcion, false);
                            }
                        });
                    }
                }
                else {
                    var idSpanAlertComponentAlert = document.getElementById("alert-" + vm.nombreComponente);
                    var idSpanAlertComponent = document.getElementById(tipError + vm.nombreComponente)
                    if (idSpanAlertComponent != null)
                        idSpanAlertComponent.classList.add('hidden');

                    if (idSpanAlertComponentAlert != null)
                        idSpanAlertComponentAlert.classList.remove("ico-advertencia");
                }
                vm.notificacionestado({ nombreComponente: vm.nombreComponente, esValido: isValid });
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
        };

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
        };

        vm.limpiarErrores = function () {
            var errorElements = document.getElementsByClassName('errorSeccionInformacionGeneralViabilidad');
            var testDivs = Array.prototype.filter.call(errorElements, function (errorElement) {
                errorElement.innerHTML = "";
                errorElement.classList.add('hidden');
            });
        };
    };

    angular.module('backbone').component('valorEstructuracionSgr', {
        templateUrl: "/src/app/formulario/ventanas/SGR/gestionRecursos/ejecutor/costosEstructuracion/valorEstructuracionSgr/valorEstructuracionSgr.html",
        controller: valorEstructuracionSgrController,
        controllerAs: "vm",
        bindings: {
            guardadoevent: '&',
            notificacionestado: '&',
            notificacionvalidacion: '&'
        }
    })
})();