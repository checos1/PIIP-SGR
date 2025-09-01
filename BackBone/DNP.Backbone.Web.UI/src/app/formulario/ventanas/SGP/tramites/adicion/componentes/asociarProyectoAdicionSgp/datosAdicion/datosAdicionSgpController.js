/// <reference path="modaldatosadicionsgpcontroller.js" />
(function () {
    'use strict';

    datosAdicionSgpController.$inject = ['datosAdicionSgpServicio', '$sessionStorage', '$uibModal', 'utilidades', 'constantesBackbone', '$scope', 'justificacionCambiosServicio'
    ];

    function datosAdicionSgpController(
        datosAdicionSgpServicio, $sessionStorage, $uibModal, utilidades, constantesBackbone, $scope, justificacionCambiosServicio
    ) {
        var vm = this;
        vm.init = init;
        vm.nombreComponente = "proyectodatosadicionpordonacion";

        vm.ConvertirNumero = ConvertirNumero;
        vm.abrirModalAgregarDatosAdicionDonacion = abrirModalAgregarDatosAdicionDonacion;
        vm.AbrirModalEditarDatosAdicionDonacion = AbrirModalEditarDatosAdicionDonacion;
        vm.BPIN = $sessionStorage.idObjetoNegocio;
        vm.eliminarDatosAdicionDonacion = eliminarDatosAdicionDonacion;

        vm.disabled = true;
        vm.notificacionErrores = null;
        vm.erroresActivos = [];
        vm.permiteEditar = false;
        vm.seccionCapitulo = null;
        vm.eventoValidar = eventoValidar;

        vm.etapaEdicion = "";
        vm.habilitaBotones = true;
        vm.CantidadConvenios = 0;
        vm.soloLectura = false;

        function init() {
            vm.soloLectura = $sessionStorage.soloLectura;
            if (vm.tramiteid !== undefined && vm.tramiteid !== '')
                vm.obtenerDatosAdicionDonacion();
            vm.notificacionvalidacion({ handler: vm.notificacionValidacionPadre, nombreComponente: vm.nombreComponente, esValido: true });

            $scope.$watch(() => vm.tramiteid
                , (newVal, oldVal) => {
                    if (newVal) {
                        if (vm.tramiteid !== undefined && vm.tramiteid !== '') {
                            vm.obtenerDatosAdicionDonacion();
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

        vm.obtenerDatosAdicionDonacion = function () {

            var tramiteId = vm.tramiteid;
            var listaDatosAdicion = [];
            vm.CantidadConvenios = 0;

            datosAdicionSgpServicio.ObtenerDatosAdicionSgp(tramiteId)
                .then(function (response) {
                    var datosAdicion = "";
                    if (response.data != null && response.data != "") {
                        var arreglolistaDatos = jQuery.parseJSON(response.data);
                        var arreglolistaDatosAdicion = jQuery.parseJSON(arreglolistaDatos);

                        if (arreglolistaDatosAdicion[0].ConvenioId > 0) {
                            for (var ls = 0; ls < arreglolistaDatosAdicion.length; ls++) {
                                datosAdicion = {
                                    "ConvenioId": arreglolistaDatosAdicion[ls].ConvenioId,
                                    "ConvenioDonanteId": arreglolistaDatosAdicion[ls].ConvenioDonanteId,
                                    "Sector": arreglolistaDatosAdicion[ls].Sector,
                                    "NumeroConvenio": arreglolistaDatosAdicion[ls].NumeroConvenio,
                                    "ObjetoConvenio": arreglolistaDatosAdicion[ls].ObjetoConvenio,
                                    "ValorConvenio": arreglolistaDatosAdicion[ls].ValorConvenio,
                                    "ValorConvenioVigencia": arreglolistaDatosAdicion[ls].ValorConvenioVigencia,
                                    "Periodo": arreglolistaDatosAdicion[ls].periodo,
                                    "FechaInicial": arreglolistaDatosAdicion[ls].FechaInicial,
                                    "FechaFinal": arreglolistaDatosAdicion[ls].FechaFinal,
                                    "EntityId": arreglolistaDatosAdicion[ls].EntityId,
                                    "NombreDonante": arreglolistaDatosAdicion[ls].NombreDonante,
                                    "SectorId": arreglolistaDatosAdicion[ls].SectorId,
                                }
                                listaDatosAdicion.push(datosAdicion);
                            }
                            vm.CantidadConvenios = arreglolistaDatosAdicion.length;
                        }
                    }
                    vm.listaDatosAdicion = listaDatosAdicion;
                    $sessionStorage.listaDatosAdicion = vm.listaDatosAdicion;
                    if (vm.CantidadConvenios == 0)
                        eliminarCapitulosModificados();
                });
        }

        function abrirModalAgregarDatosAdicionDonacion() {

            $sessionStorage.convenioId = null;
            $sessionStorage.tramiteId = vm.tramiteid;

            let modalInstance = $uibModal.open({
                templateUrl: 'src/app/formulario/ventanas/SGP/tramites/adicion/componentes/asociarProyectoAdicionSgp/datosAdicion/modalDatosAdicionSgp.html',
                controller: 'modalDatosAdicionSgpController',
            });
            modalInstance.result.then(data => {
                vm.guardadoevent({ nombreComponenteHijo: vm.nombreComponente });
                vm.obtenerDatosAdicionDonacion();
                guardarCapituloModificado();
               
            }, function (reason) {
            }), err => {
                toastr.error("Ocurrió un error al consultar el idAplicacion");
            };
            
        }

        function AbrirModalEditarDatosAdicionDonacion(convenioId) {

            $sessionStorage.convenioId = convenioId;

            let modalInstance = $uibModal.open({
                templateUrl: 'src/app/formulario/ventanas/SGP/tramites/adicion/componentes/asociarProyectoAdicionSgp/datosAdicion/modalDatosAdicionSgp.html',
                controller: 'modalDatosAdicionSgpController',
            });
            modalInstance.result.then(data => {
                vm.guardadoevent({ nombreComponenteHijo: vm.nombreComponente });
                vm.obtenerDatosAdicionDonacion();
                guardarCapituloModificado();
            }, function (reason) {
            }), err => {
                toastr.error("Ocurrió un error al consultar el idAplicacion");
            };
            //    .result.then(function (result) {
            //    vm.guardadoevent({ nombreComponenteHijo: vm.nombreComponente });
            //    guardarCapituloModificado();
            //    vm.obtenerDatosAdicionDonacion();
            //    //init();
            //}, function (reason) {
            //    var prueba = 32;
            //}), err => {
            //    toastr.error("Ocurrió un error al consultar el idAplicacion");
            //};
        }

        function eliminarDatosAdicionDonacion(convenioId) {

            var tramiteId = vm.tramiteid;
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

            utilidades.mensajeWarning("Si se realiza esta acción, todos los posibles datos diligenciados en la sección 'Datos de la adición' se perderán. Usted deberá verficar la información incluida previamente en otros espacios sea coherente con la modificación. Esta seguro de continuar?",
                function funcionContinuar() {

                    return datosAdicionSgpServicio.eliminarDatosAdicionSgp(params, usuarioDNP)
                        .then(function (response) {
                            let exito = response.data;
                            if (exito != undefined) {
                                var respuesta = jQuery.parseJSON(exito);

                                if (respuesta.Exito) {
                                    utilidades.mensajeSuccess("", false, false, false, "Los datos del convenio se han eliminado con éxito!");
                                    vm.obtenerDatosAdicionDonacion();
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
            datosAdicionSgpServicio.obtenerErroresProyecto(vm.guiMacroproceso, vm.ProyectoId, vm.idInstancia).then(function (respuesta) {
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

        vm.verMas = function (e) {
            var ATTRIBUTES = ['titlevalue', 'textvalue'];

            var $target = $(e.target);
            var modalSelector = $target.data('target');

            ATTRIBUTES.forEach(function (attributeName) {
                var $modalAttribute = $(modalSelector + ' #modal-' + attributeName);
                var dataValue = $target.data(attributeName);
                $modalAttribute.text(dataValue || '');
            });
        }
    }

    angular.module('backbone').component('datosAdicionSgp', {
        templateUrl: 'src/app/formulario/ventanas/SGP/tramites/adicion/componentes/asociarProyectoAdicionSgp/datosAdicion/datosAdicionSgp.html',
        controller: datosAdicionSgpController,
        controllerAs: "vm",
        bindings: {
            callback: '&',
            notificacionvalidacion: '&',
            notificacionestado: '&',
            guardadocomponent: '&',
            guardadoevent: '&',
            notificacioncambios: '&',
            tramiteid: '@'
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