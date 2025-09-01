(function () {
    'use strict';

    tablaResumenDistribucionPasoTresController.$inject = [
        '$scope',
        '$sessionStorage',
        'utilidades',
        '$timeout',
        'justificacionCambiosServicio',
        'comunesServicio'
    ];

    function tablaResumenDistribucionPasoTresController(
        $scope,
        $sessionStorage,
        utilidades,
        $timeout,
        justificacionCambiosServicio,
        comunesServicio
    ) {
        /*Varibales */

        var vm = this;
        vm.lang = "es";
        vm.instanciaId = $sessionStorage.idInstancia;
        vm.proyectoId = $sessionStorage.proyectoId;
        vm.EntidadDestinoId = $sessionStorage.InstanciaSeleccionada.entidadId;
        vm.tramiteid = $sessionStorage.InstanciaSeleccionada.tramiteId;
        //vm.Origen = 0;
        //vm.origen1 = 'distribucion';

        vm.nombreComponente = "distribuciondistribucionrecursos";
        vm.notificacionCambiosCapitulos = null;

        vm.handlerComponentes = [];
        vm.handlerComponentesChecked = {};
        vm.habilitaBotones = !$sessionStorage.soloLectura ? true : false;// habilita solo en paso 1
        vm.seccionCapitulo = null;

        vm.errores = {
            constante: false,
            corriente: false
        };

        vm.pagina = 1;

        /*declara metodos*/

        vm.ConvertirNumero = ConvertirNumero;
        vm.modificodatos = '0';
        vm.TotalRecursosSolicitados = 0;
        vm.TotalCuotaComunicada = 0;
        vm.TotalDistribucionCuotaComunicada = 0;
        vm.TotalRecursosAprobados = 0;
        vm.TotalSaldo = 0;


        $scope.$watch('$sessionStorage.TramiteId', function () {
            if ($sessionStorage.TramiteId !== '' && $sessionStorage.TramiteId !== undefined && $sessionStorage.TramiteId !== null) {
                vm.tramiteid = $sessionStorage.TramiteId;
                cargaVariables();
            }

        });

        //$scope.$watch('$sessionStorage.EntidadDestinoId', function () {
        //    if ($sessionStorage.EntidadDestinoId !== '' && $sessionStorage.EntidadDestinoId !== undefined && $sessionStorage.EntidadDestinoId !== null) {
        //        vm.EntidadDestinoId = $sessionStorage.EntidadDestinoId;
        //        cargaVariables();
        //    }

        //});



        $scope.$watch('vm.modificardistribucion', function () {
            if (vm.modificardistribucion === '1') {
                cargaVariables();
                vm.modificardistribucion = '2';
            }

        });

        $scope.$watch('vm.section', function () {
            cargaVariables();
        });

        function cargaVariables() {
            if (vm.tramiteid !== undefined && vm.EntidadDestinoId !== undefined)
                ObtenerProgramacionDistribucion();
        }

        vm.init = function () {
            ObtenerProgramacionDistribucion();

            //vm.inicializarComponenteCheck();
            //vm.notificacionvalidacion({ handler: vm.notificacionValidacionPadre, nombreComponente: vm.nombreComponente });
            //vm.notificacioncambios({ handler: vm.notificacionCambiosCapitulos, nombreComponente: vm.nombreComponente });
        };

        /*Funciones*/

        function ObtenerProgramacionDistribucion() {
            if ($sessionStorage.TramiteId !== '' && $sessionStorage.TramiteId !== undefined && $sessionStorage.TramiteId !== null)
                return comunesServicio.ObtenerDatosProgramacionEncabezado(vm.EntidadDestinoId, vm.tramiteid, vm.section).then(
                    function (respuesta) {
                        if (respuesta.data !== '') {
                            $scope.datos = jQuery.parseJSON(jQuery.parseJSON(respuesta.data));
                            vm.datosTotales = $scope.datos;
                            vm.TotalRecursosSolicitados = vm.datosTotales.TRSNacion + vm.datosTotales.TRSPropios;
                            vm.TotalCuotaComunicada = vm.datosTotales.TCNacionCSF + vm.datosTotales.TCCNacionSSF + vm.datosTotales.TCCPropios;
                            vm.TotalDistribucionCuotaComunicada = vm.datosTotales.TDCCNacionCSF + vm.datosTotales.TDCCaNacionSSF + vm.datosTotales.TDCCPropios;
                            vm.TotalRecursosAprobados = vm.datosTotales.TDCCNacionCSFA + vm.datosTotales.TDCCaNacionSSFA + vm.datosTotales.TDCCPropiosA;
                            vm.TotalSaldo = vm.TotalCuotaComunicada - vm.TotalRecursosAprobados;
                        }
                        else {
                            $scope.datos = [];
                        }
                    });
        }

        vm.abrirTooltip = function () {
            utilidades.mensajeInformacion('Tabla de resumen que presenta la distribución de recursos para el proceso de programación'
                , false, "Programación")
            // vm.modificodatos = 1;
        }



        //para guardar los capitulos modificados y que se llenen las lunas

        function ObtenerSeccionCapitulo() {
            const span = document.getElementById('id-capitulo-' + vm.nombreComponente);
            vm.seccionCapitulo = span.textContent;
        }

        function guardarCapituloModificado() {
            ObtenerSeccionCapitulo();
            var data = {
                ProyectoId: $sessionStorage.proyectoId,
                Justificacion: "",
                //SeccionCapituloId: vm.SeccionCapituloId,
                SeccionCapituloId: vm.seccionCapitulo,
                InstanciaId: $sessionStorage.idInstancia,
                Modificado: false,
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


        $scope.$watchCollection("vm.handlerComponentesChecked", function (newValue, oldValue) {
            var estado = true;
            var listHijos = Object.keys(vm.handlerComponentesChecked);
            if (listHijos.length == 0 || newValue === oldValue) {
                return;
            }
            listHijos.forEach(p => {
                if (vm.handlerComponentesChecked[p] == false) {
                    estado = false;
                }
            });
            vm.notificacionestado({ estado: estado, nombreComponente: vm.nombreComponente });
        });

        vm.inicializarComponenteCheck = function () {
            vm.handlerComponentesChecked = {
            };
        }

        vm.validateFormat = function (event) {

            if ((event.keyCode < 48 || event.keyCode > 57) && event.keyCode != 44) {
                event.preventDefault();
            }

            var newValue = event.target.value;
            var spiltArray = String(newValue).split("");
            var tamanioPermitido = 12;
            var tamanio = event.target.value.length;
            var permitido = false;
            permitido = event.target.value.toString().includes(".");
            if (permitido) {
                tamanioPermitido = 15;

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
                if (tamanio > tamanioPermitido || tamanio > 15) {
                    event.preventDefault();
                }
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

            var newValue = event.target.value;
            var spiltArray = String(newValue).split("");
            var tamanioPermitido = 11;
            var tamanio = event.target.value.length;
            var permitido = false;
            permitido = event.target.value.toString().includes(".");
            if (permitido) {
                var indicePunto = event.target.value.toString().indexOf(".");
                var decimales = event.target.value.toString().substring(indicePunto, tamanio).length;
                tamanioPermitido = 16;

                var n = String(newValue).split(".");
                if (n[1]) {
                    var n2 = n[1].slice(0, 2);
                    newValue = [n[0], n2].join(".");
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
            }
        }



        function ConvertirNumero(numero) {
            return new Intl.NumberFormat('es-co', {
                minimumFractionDigits: 2,
            }).format(numero);
        }

        /* ------------------------ Validaciones ---------------------------------*/

        /**
        * Función que recibe los estados de los componentes hijos
        * @param {any} esValido true: valido, false: invalido
        * @param {any} nombreComponente nombre configurado en cada uno de los componentes hijos vm.nombreComponente = 'datosgenerales'
        */
        vm.notificacionEstado = function (nombreComponente, esValido) {
            var indx = vm.handlerComponentes.findIndex(p => p.componente == nombreComponente);
            vm.esValido = !esValido ? false : vm.esValido;
            vm.handlerComponentes[indx].esValido = esValido;
            vm.handlerComponentesChecked[nombreComponente] = esValido;
            vm.showAlertError(nombreComponente, esValido);
        }

        /**
         * Función que visualiza alerta de error tab de componente
         * @param {any} nombreComponente nombre configurado en cada uno de los componentes hijos vm.nombreComponente = 'datosgenerales'
         */
        vm.showAlertError = function (nombreComponente, esValido) {
            var idSpanAlertComponent = document.getElementById("alert-" + nombreComponente);
            if (idSpanAlertComponent != undefined) {
                if (!esValido) {
                    idSpanAlertComponent.classList.add("ico-advertencia");
                } else {
                    idSpanAlertComponent.classList.remove("ico-advertencia");
                }
            }
        }

        vm.notificacionValidacionPadre = function (errores) {
            vm.limpiarErrores();
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
            vm.errores.constante = false;
            vm.errores.corriente = false;
            var campoObligatorioConstante = document.getElementById(vm.nombreComponente + "-error");
            if (campoObligatorioConstante != undefined) {
                campoObligatorioConstante.innerHTML = "";
                campoObligatorioConstante.classList.add('hidden');
            }
        }



        /* ------------------------ FIN Validaciones ---------------------------------*/
    }

    angular.module('backbone').component('tablaResumenDistribucionPasoTres', {

        templateUrl: "src/app/formulario/ventanas/comun/programacionRecursos/tablaResumenDistribucionPasoTres.html",
        controller: tablaResumenDistribucionPasoTresController,
        controllerAs: "vm",
        bindings: {
            guardadoevent: '&',
            notificacioncambios: '&',
            notificacionvalidacion: '&',
            notificacionestado: '&',
            notificarrefresco: '&',
            callback: '&',
            nivel: '@',
            rol: '@',
            section: '@',
            actualizacomponentes: '@',
            modificardistribucion: '=',


        }
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
    })
        .directive('nksOnlyNumber', function () {
            return {
                restrict: 'EA',
                require: 'ngModel',
                link: function (scope, element, attrs, ngModel) {
                    scope.$watch(attrs.ngModel, function (newValue, oldValue) {
                        var spiltArray = String(newValue).split("");

                        if (attrs.allowNegative == "false") {
                            if (spiltArray[0] == '-') {
                                newValue = newValue.replace("-", "");
                                ngModel.$setViewValue(newValue);
                                ngModel.$render();
                            }
                        }

                        if (attrs.allowDecimal == "false") {
                            newValue = parseInt(newValue);
                            ngModel.$setViewValue(newValue);
                            ngModel.$render();
                        }

                        if (attrs.allowDecimal != "false") {
                            if (attrs.decimalUpto) {
                                var n = String(newValue).split(".");
                                if (n[1]) {
                                    var n2 = n[1].slice(0, attrs.decimalUpto);
                                    newValue = [n[0], n2].join(".");
                                    ngModel.$setViewValue(newValue);
                                    ngModel.$render();
                                }
                            }
                        }

                        if (spiltArray.length === 0) return;
                        if (spiltArray.length === 1 && (spiltArray[0] == '-' || spiltArray[0] === '.')) return;
                        if (spiltArray.length === 2 && newValue === '-.') return;

                        /*Check it is number or not.*/
                        if (isNaN(newValue)) {
                            ngModel.$setViewValue(oldValue || '');
                            ngModel.$render();
                        }
                    });
                }
            };
        });
})();