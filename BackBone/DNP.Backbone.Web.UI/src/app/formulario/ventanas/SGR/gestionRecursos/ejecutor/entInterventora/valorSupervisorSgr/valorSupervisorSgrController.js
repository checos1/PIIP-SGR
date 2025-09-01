(function () {
    'use strict';
    valorSupervisorSgrController.$inject = [
        '$scope',
        'utilidades',
        '$sessionStorage',
        'justificacionCambiosServicio',
        'viabilidadSgrServicio',
        'designarEjecutorSgrServicio'
    ];

    function valorSupervisorSgrController(
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
        
        vm.nombreComponente = 'sgrejecutordesignacionentinterventorregistrovalorsupervisor';
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
        //IMPORTANTE: Revisar este concepto

        //Variables
        vm.valoresAprobacion = [];
        vm.totalValorAprobado = 0;
        vm.totalValorsupervisor = 0;

        vm.data;

        vm.init = function () {
            
            vm.notificacionvalidacion({ handler: vm.notificacionValidacionPadre, nombreComponente: vm.nombreComponente, esValido: true });
            
            LeerInformacionGeneral(vm.proyectoId, vm.idInstanciaViabilidad, vm.tipoConceptoViabilidad);
            vm.disabled = $sessionStorage.soloLectura;
        };

        // NOTA: controlar cada parámetro.
        designarEjecutorSgrServicio.registrarObservador(function (datos) {
            //Validacion dato recCostos            
            if (datos.regSupervisor === true) {
                vm.tipoVisualizacion = 'tipo2';
                ConsultaValoresAprobacion();
            } else
                if (datos.regSupervisor === false) {
                    vm.tipoVisualizacion = 'tipo1';
                    ConsultaValoresAprobacion();
                    guardarValoresSupervisor('No');
                }            
        });

        function LeerInformacionGeneral(proyectoId, instanciaId, tipoConceptoViabilidadCode) {
            viabilidadSgrServicio.SGR_Viabilidad_LeerInformacionGeneral(proyectoId, instanciaId, tipoConceptoViabilidadCode)
                .then(function (response) {                    
                    if (response.data != null) {
                        vm.data = response.data;           
                        vm.data.ValorSupervisor = parseFloat(vm.data.ValorApoyoSupervision);
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
                            Valorsupervisor: item.ValorSupervisor
                        }));                        
                        vm.valoresAprobacion = datosProcesados;
                        vm.totalValorAprobado = vm.valoresAprobacion.reduce((sum, item) => sum + item.ValorAprobado, 0);
                        vm.totalValorsupervisor = vm.valoresAprobacion.reduce((sum, item) => sum + item.Valorsupervisor, 0);                        
                    }
                });
        };

        vm.actualizarValor = function (itemEditado) {            
            itemEditado.Valorsupervisor = itemEditado.Valorsupervisor.replace(/[^0-9.]/g, '');
            const partes = itemEditado.Valorsupervisor.split('.');

            if (partes.length > 2) {
                itemEditado.Valorsupervisor = partes[0] + '.' + partes.slice(1).join('');
            }

            if (partes.length === 2) {
                partes[1] = partes[1].substring(0, 2);
                itemEditado.Valorsupervisor = partes[0] + '.' + partes[1];
            }

            const Supervisor = parseFloat(itemEditado.Valorsupervisor);
            const aprobado = parseFloat(itemEditado.ValorAprobado);

            if (!isNaN(Supervisor) && !isNaN(aprobado) && Supervisor > aprobado) {
                utilidades.mensajeError("El valor de supervisión no puede ser mayor al valor aprobado.", false);
                itemEditado.Valorsupervisor = 0.00; ///aprobado.toFixed(2); 
            }

            vm.totalValorsupervisor = parseFloat(
                vm.valoresAprobacion.reduce((sum, item) => {
                    const val = parseFloat(item.Valorsupervisor);
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
            if (vm.totalValorsupervisor == 0) {
                utilidades.mensajeError('El total de "Valor de la supervisión" debe ser mayor a cero.', false);                
                return false;
            }

            if (!isNaN(vm.totalValorsupervisor) && !isNaN(vm.data.ValorSupervisor) && vm.totalValorsupervisor !== vm.data.ValorSupervisor) {
                handleAdvertencia();
                utilidades.mensajeWarning(vm.mensajeWarning, function funcionContinuar() {
                    guardarValoresSupervisor('Ok').then(function (ok1) {
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
                guardarValoresSupervisor('Ok').then(function (ok1) {
                    if (!ok1)
                        return handleGuardarError();
                    else
                        handleGuardarExito();
                })
            }


        };

        function guardarValoresSupervisor(uso) {            
            let datosAEnviar = [];

            if (uso == 'No') {
                
                datosAEnviar = {
                    campo: 'ValorSupervisor',
                    listaValores: vm.valoresAprobacion.map(item => ({
                        Id: item.Id,
                        Valor: 0
                    }))
                };                

            } else {
                
                datosAEnviar = {
                    campo: 'ValorSupervisor',
                    listaValores: vm.valoresAprobacion.map(item => ({
                        Id: item.Id,
                        Valor: parseFloat(item.Valorsupervisor) || 0
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
            }).format(vm.totalValorsupervisor);

            const formattedValor = new Intl.NumberFormat('es-CO', {
                style: 'currency',
                currency: 'COP'
            }).format(vm.data.ValorSupervisor);

            // Construcción del mensaje con valores formateados
            vm.mensajeWarning = `El valor total de la supervisión (${formattedTotal}) NO corresponde a los valores diligenciados en los conceptos emitidos para el proyecto en el proceso de viabilidad (${formattedValor}).`.replace(/\s+/g, ' ').trim();

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
            var tipError = 'VALSUP';            
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

    angular.module('backbone').component('valorSupervisorSgr', {
        templateUrl: "/src/app/formulario/ventanas/SGR/gestionRecursos/ejecutor/entInterventora/valorSupervisorSgr/valorSupervisorSgr.html",
        controller: valorSupervisorSgrController,
        controllerAs: "vm",
        bindings: {
            guardadoevent: '&',
            notificacionestado: '&',
            notificacionvalidacion: '&'
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
        });;
})();