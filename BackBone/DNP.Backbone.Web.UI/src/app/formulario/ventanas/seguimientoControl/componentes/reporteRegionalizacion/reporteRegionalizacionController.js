(function () {
    'use strict';

    reporteRegionalizacionController.$inject = [
        '$scope',
        '$sessionStorage',
        'constantesBackbone',
        'justificacionCambiosServicio'
        //'regionalizacionServicio'
    ];

    function reporteRegionalizacionController(
        $scope,
        $sessionStorage,
        constantesBackbone,
        justificacionCambiosServicio
        //metaProductoServicio
    ) {
        var vm = this;
        vm.lang = "es";
        vm.nombreComponente = "regionalizacion";
        vm.arrowIcoDown = "glyphicon-chevron-down";
        vm.arrowIcoUp = "glyphicon-chevron-up";
        //vm.notificacionCambiosCapitulos = null;
        vm.idNivel = $sessionStorage.idNivel;
        vm.refreshregionalizacion = 'false';
        //vm.refreshregionalizacion1 = 'false';

        vm.handlerComponentes = [
            { id: 1, componente: 'regionalizacionavanceregionaliza', handlerValidacion: null, handlerCambios: null, esValido: true },
            { id: 2, componente: 'regionalizacionavancefocaliza', handlerValidacion: null, handlerCambios: null, esValido: true },
            /*{ id: 3, componente: 'regionalizacionresumenregionaliza', handlerValidacion: null, handlerCambios: null, esValido: true },*/

        ];
        vm.handlerComponentesChecked = {};

        //Inicio
        vm.init = function () {
            $sessionStorage.esAjuste = false;
            vm.inicializarComponenteCheck();

            vm.notificacioncambios({ handler: vm.notificacionCambiosCapitulos, nombreComponente: vm.nombreComponente });
            vm.notificacionvalidacion({ handler: vm.notificacionValidacionEvent, nombreComponente: vm.nombreComponente, handlerCapitulos: vm.capitulos });

        };

        //$scope.$watch('vm.refreshregionalizacion', function () {
        //    if (vm.refreshregionalizacion === "true") {
        //        vm.refreshregionalizacion = 'true';
        //    }
        //    else if (vm.refreshregionalizacion1 === "false") {
        //        vm.refreshregionalizacion = 'false';            }

        //});

        /**
        * Función handler que contiene la referencia del binding notificacioncambios.
        * IMPORTANTE: En esta función se realiza la notificación a todos los componentes hijos los cambios externos
            * @param { any } param0
        */
        vm.notificacionCambiosCapitulos = function ({ nombreComponente, nombreComponenteHijo }) {

            vm.handlerComponentes.forEach(comp => {
                if (comp.handlerCambios != null) {
                    try {
                        comp.handlerCambios(nombreComponenteHijo);
                    } catch (error) {
                        console.error('¡¡Tiene ERRORES - handlerCambios del componente = ' + comp.componente + '!!');
                    }
                }
            });
        };

        /*------------------------------------Validaciones-----------------------------------*/
        /**
       * Listado de componentes hijos, obligatorio para estructura de validación
       * */

        vm.changeArrow = function (nombreModificado) {
            var idSpanArrow = 'arrow-' + nombreModificado;
            var arrowCapitulo = document.getElementById(idSpanArrow);
            var arrowClasses = arrowCapitulo.classList;
            for (var i = 0; i < arrowClasses.length; i++) {
                if (arrowClasses[i] == vm.arrowIcoDown) {
                    document.getElementById(idSpanArrow).classList.add(vm.arrowIcoUp);
                    document.getElementById(idSpanArrow).classList.remove(vm.arrowIcoDown);
                    break;
                } else if (arrowClasses[i] == vm.arrowIcoUp) {
                    document.getElementById(idSpanArrow).classList.add(vm.arrowIcoDown);
                    document.getElementById(idSpanArrow).classList.remove(vm.arrowIcoUp);
                    break;
                }
            }
        }

        vm.guardado = function (nombreComponenteHijo) {
            vm.callback();
            vm.guardadocomponent({ nombreComponente: vm.nombreComponente, nombreComponenteHijo: nombreComponenteHijo });
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
                'regionalizacionavanceregionaliza': true,
                'regionalizacionavancefocaliza': true//,
                /*'regionalizacionresumenregionaliza': true*/
            };
        }

        /* --------------------------------- Validaciones ---------------------------*/

        /**
         * Función que recibe listado de errores referentes a la sección de justificación
         * envía a sus hijos el listado de errores
         * @param {any} errores
         */
        vm.notificacionValidacionEvent = function (listErrores) {
            var erroresList = listErrores.errores.filter(p => p.Seccion == vm.nombreComponente);
            vm.inicializarComponenteCheck();
            vm.esValido = true;
            if (erroresList.length > 0) {
                for (var i = 0; i < vm.handlerComponentes.length; i++) {
                    if (vm.handlerComponentes[i].handlerValidacion) vm.handlerComponentes[i].handlerValidacion(erroresList);
                }
            }
        }

        /**
         * Función que crea las referencias de los métodos de los hijos con el padre. Este es llamado cuando se inicializa el componente hijo.
         * @param {any} handler función referenciada
         * @param {any} nombreComponente nombre configurado en cada uno de los componentes hijos vm.nombreComponente = 'datosgenerales'
         */
        //vm.notificacionValidacionHijos = function (handler, nombreComponente) {
        //    var indx = vm.handlerComponentes.findIndex(p => p.componente == nombreComponente);
        //    vm.handlerComponentes[indx].handlerValidacion = handler;
        //};

        vm.notificacionValidacionHijos = function (handler, nombreComponente) {
            var indx = vm.handlerComponentes.findIndex(p => p.componente == nombreComponente);
            if (indx > -1) vm.handlerComponentes[indx].handlerValidacion = handler;

        };

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

        vm.capitulos = function (listadoCapitulos) {

            var listadoCapRecursos = listadoCapitulos.filter(p => p.SeccionModificado == vm.nombreComponente)
            listadoCapRecursos.forEach(function (item) {
                var el = document.getElementById("name-capitulo-" + item.nombreComponente);
                var elidSeccionCapitulo = document.getElementById("id-capitulo-" + item.nombreComponente);
                var elAccordion = document.getElementById("accordion-" + item.nombreComponente);
                if (el != undefined && el != null) {
                    el.innerHTML = item.Capitulo;
                }
                if (elAccordion != undefined && elAccordion != null) {
                    elAccordion.classList.remove("hidden");
                }
                if (elidSeccionCapitulo != undefined && elidSeccionCapitulo != null) {
                    elidSeccionCapitulo.innerHTML = item.SeccionCapituloId;
                }
            });

        };
    }

    angular.module('backbone').component('reporteRegionalizacion', {
        templateUrl: "src/app/formulario/ventanas/seguimientoControl/componentes/reporteRegionalizacion/reporteRegionalizacion.html",
        controller: reporteRegionalizacionController,
        controllerAs: "vm",
        bindings: {
            callback: '&',
            tramiteid: '@',
            verificacion: '&',
            modelo: '=',
            notificacionvalidacion: '&',
            notificacionestado: '&',
            guardadocomponent: '&',
            notificacioncambios: '&',
            tipotramiteid: '@',
            refreshregionalizacion: '@',
        }
    });

})();