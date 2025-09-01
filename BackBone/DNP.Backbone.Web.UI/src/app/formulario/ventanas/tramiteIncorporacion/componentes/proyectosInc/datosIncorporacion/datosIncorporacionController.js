/// <reference path="modaldatosincorporacioncontroller.js" />
(function () {
    'use strict';

    datosIncorporacionController.$inject = ['datosIncorporacionServicio', '$sessionStorage', '$uibModal', 'utilidades', 'constantesBackbone', '$scope', 'justificacionCambiosServicio'
    ];

    function datosIncorporacionController(
        datosIncorporacionServicio, $sessionStorage, $uibModal, utilidades, constantesBackbone, $scope, justificacionCambiosServicio
    ) {
        var vm = this;
        vm.init = init;
        vm.nombreComponente = "proyectodatosincorporacion";

        vm.ConvertirNumero = ConvertirNumero;
        vm.abrirModalAgregarDatosIncorporacion = abrirModalAgregarDatosIncorporacion;
        vm.AbrirModalEditarDatosIncorporacion = AbrirModalEditarDatosIncorporacion;
        vm.BPIN = $sessionStorage.idObjetoNegocio;
        vm.eliminarDatosIncorporacion = eliminarDatosIncorporacion;

        vm.disabled = true;
        vm.notificacionErrores = null;
        vm.erroresActivos = [];
        vm.permiteEditar = false;
        vm.seccionCapitulo = null;
        vm.eventoValidar = eventoValidar;
        var currentYear = new Date().getFullYear();
        var listaDatosIncorporacion = [];

        vm.etapaEdicion = "";
        vm.habilitaBotones = true;
        vm.CantidadConvenios = 0;
        var cancelaEdicion = false;
        var es_edicion = false;

        

        function init() {
            if ($sessionStorage.tramiteId !== undefined && $sessionStorage.tramiteId !== '')
                vm.obtenerDatosIncorporacion();
            vm.notificacionvalidacion({ handler: vm.notificacionValidacionPadre, nombreComponente: vm.nombreComponente, esValido: true });

            $scope.$watch(() => $sessionStorage.tramiteId
                , (newVal, oldVal) => {
                    if (newVal) {
                        if ($sessionStorage.tramiteId !== undefined && $sessionStorage.tramiteId !== '') {
                            vm.obtenerDatosIncorporacion();
                        }
                    }
                }, true);
        }

        vm.notificacionCambiosCapitulos = function (nombreCapituloHijo) {
            vm.recargaGuardado();
        }

        vm.notificacionValidacionHijo = function (handler) {
            vm.notificacionErrores = handler;
        }

        vm.recargaresumen = function (handler) {
            vm.recargaGuardado = handler;
        }

        vm.obtenerDatosIncorporacion = function () {

            var tramiteId = $sessionStorage.tramiteId;
            var listaDatosIncorporacion = [];
            vm.CantidadConvenios = 0;

            datosIncorporacionServicio.ObtenerDatosIncorporacion(tramiteId)
                .then(function (response) {
                    var datosIncorporacion = "";
                    if (response.data != null && response.data != "") {
                        var arreglolistaDatos = jQuery.parseJSON(response.data);
                        var arreglolistaDatosIncorporacion = jQuery.parseJSON(arreglolistaDatos);

                        if (arreglolistaDatosIncorporacion[0].ConvenioId > 0) {
                            for (var ls = 0; ls < arreglolistaDatosIncorporacion.length; ls++) {
                                datosIncorporacion = {
                                    "ConvenioId": arreglolistaDatosIncorporacion[ls].ConvenioId,
                                    "ConvenioDonanteId": arreglolistaDatosIncorporacion[ls].ConvenioDonanteId,
                                    "Sector": arreglolistaDatosIncorporacion[ls].Sector,
                                    "NumeroConvenio": arreglolistaDatosIncorporacion[ls].NumeroConvenio,
                                    "ObjetoConvenio": arreglolistaDatosIncorporacion[ls].ObjetoConvenio,
                                    "ValorConvenio": arreglolistaDatosIncorporacion[ls].ValorConvenio,
                                    "ValorConvenioVigencia": arreglolistaDatosIncorporacion[ls].ValorConvenioVigencia,
                                    "Periodo": arreglolistaDatosIncorporacion[ls].periodo,
                                    "FechaInicial": arreglolistaDatosIncorporacion[ls].FechaInicial,
                                    "FechaFinal": arreglolistaDatosIncorporacion[ls].FechaFinal,
                                    "EntityId": arreglolistaDatosIncorporacion[ls].EntityId,
                                    "NombreDonante": arreglolistaDatosIncorporacion[ls].NombreDonante,
                                    "SectorId": arreglolistaDatosIncorporacion[ls].SectorId,
                                }
                                listaDatosIncorporacion.push(datosIncorporacion);
                            }
                            vm.CantidadConvenios = arreglolistaDatosIncorporacion.length;
                        }
                    }
                    vm.listaDatosIncorporacion = listaDatosIncorporacion;
                    $sessionStorage.listaDatosIncorporacion = vm.listaDatosIncorporacion;
                    if (vm.CantidadConvenios == 0)
                        eliminarCapitulosModificados();
                });
        }

        function abrirModalAgregarDatosIncorporacion() {
            $sessionStorage.convenioId = null;

            $uibModal.open({
                templateUrl: 'src/app/formulario/ventanas/tramiteIncorporacion/componentes/proyectosInc/datosIncorporacion/modalDatosIncorporacion.html',
                controller: 'modalDatosIncorporacionController',
            }).result.then(function (result) {
                vm.guardadoevent({ nombreComponenteHijo: vm.nombreComponente });
                guardarCapituloModificado();
                init();
            }, function (reason) {

            }), err => {
                toastr.error("Ocurrió un error al consultar el idAplicacion");
            };

        }

        function AbrirModalEditarDatosIncorporacion(convenioId) {

            $sessionStorage.convenioId = convenioId;

            $uibModal.open({
                templateUrl: 'src/app/formulario/ventanas/tramiteIncorporacion/componentes/proyectosInc/datosIncorporacion/modalDatosIncorporacion.html',
                controller: 'modalDatosIncorporacionController',
            }).result.then(function (result) {
                vm.guardadoevent({ nombreComponenteHijo: vm.nombreComponente });
                guardarCapituloModificado();
                init();
            }, function (reason) {

            }), err => {
                toastr.error("Ocurrió un error al consultar el idAplicacion");
            };

        }

        function eliminarDatosIncorporacion(convenioId) {

            var tramiteId = $sessionStorage.tramiteId;
            var params = {
                Id: convenioId,
                ConvenioId: convenioId,
                NombreDonante: "",
                EntityId: 0,
                objConvenioDto: {
                    Id: 0,
                    TramiteId: tramiteId,
                    NumeroConvenio: "",
                    ObjetoConvenio: "",
                    ValorConvenio: 0,
                    ValorConvenioVigencia: 0,
                    FechaInicial: "",
                    FechaFinal: "",
                }
            }

            utilidades.mensajeWarning("Si se realiza esta acción, todos los posibles datos diligenciados en la sección 'información presupuestal' se perderán. Usted deberá verficar la información incluida previamente en otros espacios sea coherente con la modificación. Esta seguro de continuar?",
                function funcionContinuar() {


                    return datosIncorporacionServicio.EiliminarDatosIncorporacion(params, usuarioDNP)
                        .then(function (response) {
                            let exito = response.data;
                            if (exito != undefined) {
                                var respuesta = jQuery.parseJSON(exito);

                                if (respuesta.Exito) {
                                    utilidades.mensajeSuccess("Operación realizada con éxito!", false, false, false);
                                    init();
                                }
                                else {
                                    utilidades.mensajeError("Se presento el siguiente error realizar la operación: " + respuesta.Mensaje, false);
                                }
                            } else
                                utilidades.mensajeError("Se presento un error realizar la operación.", false);
                        })
                        .catch(error => {
                            if (error.status == 400) {
                                utilidades.mensajeError(error.data.Message || "Error al realizar la operación");
                                vm.limpiarErrores();
                                return;
                            }
                            utilidades.mensajeError("Error al realizar la operación");
                        }
                            , function funcionCancelar(reason) {
                                console.log("reason", reason);
                            }
                        );
                },
                null,
                "Aceptar",
                "Cancelar",
                "Se eliminarán los datos del convenio."
            );
        }

        function ConvertirNumero(numero) {
            return new Intl.NumberFormat('es-co', {
                minimumFractionDigits: 2,
            }).format(numero);
        }

        function ObtenerSeccionCapitulo() {
            const span = document.getElementById('id-capitulo-' + vm.nombreComponente);
            vm.seccionCapitulo = span.textContent;
        }

        function guardarCapituloModificado() {
            ObtenerSeccionCapitulo();
            var data = {
                Justificacion: "",
                SeccionCapituloId: vm.seccionCapitulo,
                InstanciaId: $sessionStorage.idInstancia,
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
                Justificacion: "",
                SeccionCapituloId: vm.seccionCapitulo,
                InstanciaId: $sessionStorage.idInstancia,

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

        function eventoValidar() {
            vm.inicializarComponenteCheck();
            datosIncorporacionServicio.obtenerErroresProyecto(vm.guiMacroproceso, vm.ProyectoId, vm.idInstancia).then(function (respuesta) {
                vm.notificacionValidacionHijos(respuesta.data);
                var findErrors = respuesta.data.findIndex(p => p.Errores != null);
                vm.visualizarAlerta((findErrors != -1))
            });

        }

        /* ------------------------ Validaciones ---------------------------------*/

        vm.notificacionValidacionPadre = function (errores) {
            vm.limpiarErrores(errores);
            var isValid = true;
            if (errores != undefined) {
                var erroresRelacionconlapl = errores.find(p => (p.Seccion + p.Capitulo) == vm.nombreComponente);
                if (erroresRelacionconlapl != undefined) {
                    var erroresJson = erroresRelacionconlapl.Errores == "" ? [] : JSON.parse(erroresRelacionconlapl.Errores);
                    if (erroresJson != undefined) {
                        isValid = (erroresJson == null || erroresJson.length == 0);
                        if (!isValid) {
                            erroresJson[vm.nombreComponente].forEach(p => {

                                if (vm.errores[p.Error] != undefined) vm.errores[p.Error](p.Descripcion);
                            });
                        }
                    }
                }
                vm.notificacionestado({ nombreComponente: vm.nombreComponente, esValido: isValid });
            }
        }

        vm.limpiarErrores = function () {


            var campoObligatorioJustificacion = document.getElementById("DI001-error");
            if (campoObligatorioJustificacion != undefined) {
                campoObligatorioJustificacion.innerHTML = "";
                campoObligatorioJustificacion.classList.add('hidden');
            }

        }


        vm.DI001 = function (errores) {
            var campoObligatorioJustificacion = document.getElementById("DI001-error");
            if (campoObligatorioJustificacion != undefined) {
                campoObligatorioJustificacion.innerHTML = "<span class='d-inline-block ico-advertencia'></span><span> " + errores + "</span>";
                campoObligatorioJustificacion.classList.remove('hidden');
            }
        }

        vm.errores = {
            'DI001': vm.DI001,
        }
    }

    angular.module('backbone').component('datosIncorporacion', {
        templateUrl: 'src/app/formulario/ventanas/tramiteIncorporacion/componentes/proyectosInc/datosIncorporacion/datosIncorporacion.html',
        controller: datosIncorporacionController,
        controllerAs: "vm",
        bindings: {
            callback: '&',
            notificacionvalidacion: '&',
            notificacionestado: '&',
            guardadocomponent: '&',
            guardadoevent: '&',
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
        });

})();