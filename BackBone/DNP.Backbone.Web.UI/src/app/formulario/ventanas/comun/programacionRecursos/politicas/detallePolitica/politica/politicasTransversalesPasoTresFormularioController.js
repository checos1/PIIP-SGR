var politicasTransversalesPasoTresFormularioCtrl = null;

(function () {
    'use strict';
    angular.module('backbone').controller('politicasTransversalesPasoTresFormularioController', politicasTransversalesPasoTresFormularioController);
    politicasTransversalesPasoTresFormularioController.$inject = [
        '$scope',
        '$uibModal',
        '$log',
        '$q',
        '$sessionStorage',
        '$localStorage',
        '$timeout',
        '$location',
        '$filter',
        'focalizacionAjustesServicio',
        'trasladosServicio',
        'comunesServicio',
        'utilidades'
    ];

    function politicasTransversalesPasoTresFormularioController(
        $scope,
        $uibModal,
        $log,
        $q,
        $sessionStorage,
        $localStorage,
        $timeout,
        $location,
        $filter,
        focalizacionAjustesServicio,
        trasladosServicio,
        comunesServicio,
        utilidades) {
        var vm = this;
        politicasTransversalesPasoTresFormularioCtrl = vm;
        //variables
        vm.lang = "es";
        vm.TabActivo = 1;
        vm.nombreComponente = "politicastransversalespoliticastransv";
        vm.listaPoliticasProyecto = [];
        vm.abrirModalAgregarPoliticaTrans = abrirModalAgregarPoliticaTrans;
        vm.eliminarPolitica = eliminarPolitica;
        vm.abrirMensajePoliticasProyecto = abrirMensajePoliticasProyecto;
        vm.obtenerPoliticasTransversales = this.obtenerPoliticasTransversales;
        var listaFuentesBase = [];
        vm.notificacionErrores = null;
        vm.erroresActivos = [];
        vm.validacionGuardado = null;
        vm.recargaGuardado = null;
        vm.recargaGuardadoCostos = null;
        vm.seccionCapitulo = null;

        vm.habilitaBotones = !$sessionStorage.soloLectura ? true : false;// habilita solo en paso 1

        $scope.$watch('vm.tramiteproyectoid', function () {
            if (vm.tramiteproyectoid != '') {
                var proyectoCargado = comunesServicio.getProyectoCargado();
                if (proyectoCargado.toString() === vm.proyectoid) {
                    obtenerPoliticasTransversales();
                    vm.mostrarpoliticas = true;
                }
            }
        });

        $scope.$watch('vm.calendariopoliticastransversales', function () {
            if (vm.calendariopoliticastransversales !== undefined && vm.calendariopoliticastransversales !== '')
                vm.habilitaBotones = vm.calendariopoliticastransversales === 'true' && !$sessionStorage.soloLectura ? true : false;
        });

        vm.init = function () {
            vm.notificarrefresco({ handler: vm.refrescarPoliticas, nombreComponente: vm.nombreComponente });
        };

        vm.refrescarPoliticas = function () {
            obtenerPoliticasTransversales();
        }

        function obtenerPoliticasTransversales() {
            return comunesServicio.consultarPoliticasTransversalesProgramacion(vm.tramiteproyectoid).then(
                function (respuesta) {
                    if (respuesta.data != null && respuesta.data != "") {
                        var arreglolistas = jQuery.parseJSON(respuesta.data);
                        var arregloGeneral = jQuery.parseJSON(arreglolistas);
                        var arregloDatosGenerales = arregloGeneral.Politicas;
                        var listaPoliticasProy = [];

                        for (var pl = 0; pl < arregloDatosGenerales.length; pl++) {
                            var politicasProyecto = {
                                politicaId: arregloDatosGenerales[pl].PoliticaId,
                                politica: arregloDatosGenerales[pl].Politica,
                                enProyecto: arregloDatosGenerales[pl].EnProyecto,
                                enSeguimiento: arregloDatosGenerales[pl].EnSeguimiento,
                                TieneConceptoPendiente: arregloDatosGenerales[pl].TieneConceptoPendiente,
                                EnFirme: arregloDatosGenerales[pl].EnFirme
                            }

                            if (arregloDatosGenerales[pl].EnProyecto || arregloDatosGenerales[pl].EnSeguimiento || arregloDatosGenerales[pl].EnFirme)
                                listaPoliticasProy.push(politicasProyecto);
                        }

                        vm.listaPoliticasProyectos = angular.copy(listaPoliticasProy);
                    }
                });
        }

        function ObtenerSeccionCapitulo() {
            const span = document.getElementById('id-capitulo-' + vm.nombreComponente);
            vm.seccionCapitulo = span.textContent;
        }

        function abrirMensajePoliticasProyecto() {
            utilidades.mensajeInformacionN("", null, "<span class='anttituhori' > ¿Qué es esto? </span><br /> <span class='tituhori' >Políticas transversales asociadas</span>");
        }

        vm.getNombreReducido = function (data, maxCaracteres) {
            if (data.length > maxCaracteres) {
                var dataNueva = data.slice(0, maxCaracteres);
                return dataNueva + '...';
            } else return data
        }

        function abrirModalAgregarPoliticaTrans() {
            $sessionStorage.tramiteproyectoid = vm.tramiteproyectoid;
            $sessionStorage.proyectoid = vm.proyectoid;
            $uibModal.open({
                templateUrl: 'src/app/formulario/ventanas/comun/programacionRecursos/politicas/detallePolitica/politica/modal/modalAgregarPoliticaTransversal.html',
                controller: 'modalAgregarPoliticaTransversalController',
                bindings: {
                    tramiteproyectoid: '@',
                    proyectoid: '@'
                }
            }

            ).result.then(function (result) {
                obtenerPoliticasTransversales();
                vm.guardadoevent({ nombreComponenteHijo: vm.nombreComponente });
            }, function (reason) {

            }), err => {
                //toastr.error("Ocurrió un error al consultar el idAplicacion");
            };
        }

        function eliminarPolitica(politicaId, politica) {
            var proyectoId = $sessionStorage.idProyectoEncabezado;

            utilidades.mensajeWarning("¿Está seguro de continuar?", function funcionContinuar() {
                const mensaje3 = "Los datos fueron eliminados con éxito.";

                return comunesServicio.eliminarPoliticasProyectoProgramacion(vm.proyectoid, politicaId).then(
                    function (respuesta) {
                        if (respuesta.data && (respuesta.statusText === "OK" || respuesta.status === 200)) {
                            obtenerPoliticasTransversales();
                            $timeout(function () {
                                utilidades.mensajeSuccess("", false, function funcionContinuar() {
                                    vm.guardadoevent({ nombreComponenteHijo: vm.nombreComponente });
                                }, false, mensaje3);

                            }, 600);
                        } else {
                            new utilidades.mensajeError("No se puede eliminar la politica seleccionada, se encuentra en la versión del proyecto firme.");
                        }
                    });
            }, function funcionCancelar(reason) {
                //console.log("reason", reason);
            }, null, null, "Es posible que la política a desvincular tenga focalización registrada en sus categorías, al confirmar la desvinculación, se eliminarán los datos registrados.");
        }

        /* ------------------------ Validaciones ---------------------------------*/

        vm.limpiarErrores = function () {
            var campoObligatorioJustificacion = document.getElementById(vm.nombreComponente + "-categoriapolitica-error");
            if (campoObligatorioJustificacion != undefined) {
                campoObligatorioJustificacion.innerHTML = "";
                campoObligatorioJustificacion.classList.add('hidden');
            }
            var campoObligatorioProyectos = document.getElementById(vm.nombreComponente + "-valorcategoriapolitica-error");
            if (campoObligatorioProyectos != undefined) {
                campoObligatorioProyectos.innerHTML = "";
                campoObligatorioProyectos.classList.add('hidden');
            }
        }

        vm.validarCategoriasPoliticas = function (errores) {
            var campoObligatorioJustificacion = document.getElementById(vm.nombreComponente + "-categoriapolitica-error");
            if (campoObligatorioJustificacion != undefined) {
                campoObligatorioJustificacion.innerHTML = "<span class='d-inline-block ico-advertencia'></span><span>" + errores + "</span>";
                campoObligatorioJustificacion.classList.remove('hidden');
            }
        }

        vm.validarValoresCategorias = function (errores) {
            var campoObligatorioJustificacion = document.getElementById(vm.nombreComponente + "-valorcategoriapolitica-error");
            if (campoObligatorioJustificacion != undefined) {
                campoObligatorioJustificacion.innerHTML = "<span class='d-inline-block ico-advertencia'></span><span>" + errores + "</span>";
                campoObligatorioJustificacion.classList.remove('hidden');
            }
        }

        vm.errores = {
            'FOCPOL001': vm.validarCategoriasPoliticas,
            'FOCPOL002': vm.validarValoresCategorias,
        }

    }
    angular.module('backbone')
        .component('politicasTransversalesPasoTresFormulario', {
            templateUrl: 'src/app/formulario/ventanas/comun/programacionRecursos/politicas/detallePolitica/politica/politicasTransversalesPasoTresFormulario.html',
            controller: 'politicasTransversalesPasoTresFormularioController',
            controllerAs: 'vm',
            bindings: {
                callback: '&',
                notificacionvalidacion: '&',
                notificacionestado: '&',
                guardadoevent: '&',
                notificarrefresco: '&',
                notificacioncambios: '&',
                calendariopoliticastransversales: '@',
                tramiteproyectoid: '@',
                proyectoid: '@',
            },
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