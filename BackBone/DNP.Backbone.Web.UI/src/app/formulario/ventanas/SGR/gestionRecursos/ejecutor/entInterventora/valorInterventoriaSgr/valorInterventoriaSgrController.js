(function () {
    'use strict';
    valorInterventoriaSgrController.$inject = [
        '$scope',
        'utilidades',
        '$sessionStorage',
        'justificacionCambiosServicio',
        'viabilidadSgrServicio',
        'designarEjecutorSgrServicio'
    ];

    function valorInterventoriaSgrController(
        $scope,
        utilidades,
        $sessionStorage,
        justificacionCambiosServicio,
        viabilidadSgrServicio,
        designarEjecutorSgrServicio
    ) {
        var vm = this;
        vm.user = {};
        vm.lang = "es";
        vm.nombreComponente = 'sgrejecutordesignacionentinterventorregistrovalorinterventor';
        vm.seccionCapitulo = null; //Para guardar los capitulos modificados y que se llenen las lunas
        vm.activar = true;
        vm.desactivar = true;
        vm.estadoBotonEdit = 'EDITAR';

        vm.ConvertirNumero = ConvertirNumero;

        //Variables Esenciales
        vm.idInstancia = $sessionStorage.idInstancia;
        vm.idInstanciaViabilidad = $sessionStorage.IdInstanciaViabiliad;
        vm.proyectoId = $sessionStorage.proyectoId;
        vm.tipoConceptoViabilidad = 'VIABILIDAD';

        //Variables
        vm.valoresAprobacion = [];
        vm.totalValorAprobado = 0;
        vm.totalValorInterventor = 0;

        vm.data;

        vm.init = function () {
            vm.notificacionvalidacion({ handler: vm.notificacionValidacionPadre, nombreComponente: vm.nombreComponente, esValido: true });
            LeerInformacionGeneral(vm.proyectoId, vm.idInstanciaViabilidad, vm.tipoConceptoViabilidad);
            vm.disabled = $sessionStorage.soloLectura;
        };

        // NOTA: controlar cada parámetro.
        designarEjecutorSgrServicio.registrarObservador(function (datos) {
            //Validacion dato recCostos
            if (datos.regInterventor === true) {
                vm.tipoVisualizacion = 'tipo2';
                ConsultaValoresAprobacion();
            } else
                if (datos.regInterventor === false) {
                    vm.tipoVisualizacion = 'tipo1';
                    ConsultaValoresAprobacion();
                    guardarValoresInterventor('No');
                }
        });

        function LeerInformacionGeneral(proyectoId, instanciaId, tipoConceptoViabilidadCode) {
            viabilidadSgrServicio.SGR_Viabilidad_LeerInformacionGeneral(proyectoId, instanciaId, tipoConceptoViabilidadCode)
                .then(function (response) {
                    if (response.data != null) {
                        vm.data = response.data;
                        //vm.data.ValorInterventoria = vm.ConvertirNumero(vm.data.ValorInterventoria);
                        vm.data.ValorInterventoria = parseFloat(vm.data.ValorInterventoria);
                        vm.data.ValorApoyoSupervision = vm.ConvertirNumero(vm.data.ValorApoyoSupervision);
                    }
                });
        }

        function ConvertirNumero(numero, decimals = true) {
            return new Intl.NumberFormat('es-co', {
                minimumFractionDigits: decimals ? 2 : 0,
            }).format(numero);
        }

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
                            ValorInterventor: item.ValorInterventor
                        }));

                        vm.valoresAprobacion = datosProcesados;
                        vm.totalValorAprobado = vm.valoresAprobacion.reduce((sum, item) => sum + item.ValorAprobado, 0);
                        vm.totalValorInterventor = vm.valoresAprobacion.reduce((sum, item) => sum + item.ValorInterventor, 0);
                    }
                });
        };

        vm.actualizarValor = function (itemEditado) {
            itemEditado.ValorInterventor = itemEditado.ValorInterventor.replace(/[^0-9.]/g, '');
            const partes = itemEditado.ValorInterventor.split('.');

            if (partes.length > 2) {
                itemEditado.ValorInterventor = partes[0] + '.' + partes.slice(1).join('');
            }

            if (partes.length === 2) {
                partes[1] = partes[1].substring(0, 2);
                itemEditado.ValorInterventor = partes[0] + '.' + partes[1];
            }

            const Interventor = parseFloat(itemEditado.ValorInterventor);
            const aprobado = parseFloat(itemEditado.ValorAprobado);

            if (!isNaN(Interventor) && !isNaN(aprobado) && Interventor > aprobado) {
                utilidades.mensajeError("El valor diligenciado en Valor interventoría, no puede superar el valor aprobado por tipo de recurso y bienio", false);
                itemEditado.ValorInterventor = 0.00; ///aprobado.toFixed(2); 
            }

            vm.totalValorInterventor = parseFloat(
                vm.valoresAprobacion.reduce((sum, item) => {
                    const val = parseFloat(item.ValorInterventor);
                    return sum + (isNaN(val) ? 0 : val);
                }, 0).toFixed(2)
            );
        };

        vm.ActivarEditar = function () {
            if (vm.activar == true) {
                vm.estadoBotonEdit = 'CANCELAR';
                vm.activar = false;
            }
            else {
                utilidades.mensajeWarning("¿Está seguro de continuar?",
                    function funcionContinuar() {
                        ConsultaValoresAprobacion();
                        OkCancelar();
                        vm.estadoBotonEdit = 'EDITAR';
                        vm.activar = true;
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

            if (vm.totalValorInterventor == 0) {
                utilidades.mensajeError('El total de "Valor de la interventoría" debe ser mayor a cero.', false);
                return false;
            }
            
            //guardarValoresInterventor('Ok').then(function (ok1) {
            //    if (!ok1)
            //        return handleGuardarError();
            //    else
            //        handleGuardarExito();
            //})

            if (!isNaN(vm.totalValorInterventor) && !isNaN(vm.data.ValorInterventoria) && vm.totalValorInterventor !== vm.data.ValorInterventoria) {
                handleAdvertencia();
                utilidades.mensajeWarning(vm.mensajeWarning, function funcionContinuar() {
                    guardarValoresInterventor('Ok').then(function (ok1) {
                        if (!ok1)
                            return handleGuardarError();
                        else
                            handleGuardarExito();
                    })
                }, function funcionCancelar() {
                    return;
                });

            }
            else {
                guardarValoresInterventor('Ok').then(function (ok1) {
                    if (!ok1)
                        return handleGuardarError();
                    else
                        handleGuardarExito();
                })
            }

        };

        function guardarValoresInterventor(uso) {
            let datosAEnviar = [];

            if (uso == 'No') {
                datosAEnviar = {
                    campo: 'ValorInterventor',
                    listaValores: vm.valoresAprobacion.map(item => ({
                        Id: item.Id,
                        Valor: 0
                    }))
                };

            } else {
                datosAEnviar = {
                    campo: 'ValorInterventor',
                    listaValores: vm.valoresAprobacion.map(item => ({
                        Id: item.Id,
                        Valor: parseFloat(item.ValorInterventor) || 0
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
            vm.disabledEli = true;
        };

        function handleGuardarError() {
            utilidades.mensajeError('Error al guardar capítulo.', false);
        };

        function handleAdvertencia() {

            // Formateo de valores para mejor legibilidad
            const formattedTotal = new Intl.NumberFormat('es-CO', {
                style: 'currency',
                currency: 'COP'
            }).format(vm.totalValorInterventor);

            const formattedValor = new Intl.NumberFormat('es-CO', {
                style: 'currency',
                currency: 'COP'
            }).format(vm.data.ValorInterventoria);

            // Construcción del mensaje con valores formateados
            vm.mensajeWarning = `El valor total de la interventoría (${formattedTotal}) NO corresponde a los valores diligenciados en los conceptos emitidos para el proyecto en el proceso de viabilidad (${formattedValor}).`.replace(/\s+/g, ' ').trim();
          
        };

        /* ------------------------ Validaciones ---------------------------------*/

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
            var tipError = 'VALINT';
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

    angular.module('backbone').component('valorInterventoriaSgr', {
        templateUrl: "/src/app/formulario/ventanas/SGR/gestionRecursos/ejecutor/entInterventora/valorInterventoriaSgr/valorInterventoriaSgr.html",
        controller: valorInterventoriaSgrController,
        controllerAs: "vm",
        bindings: {
            guardadoevent: '&',
            notificacionestado: '&',
            notificacionvalidacion: '&'
        }
    })
})();