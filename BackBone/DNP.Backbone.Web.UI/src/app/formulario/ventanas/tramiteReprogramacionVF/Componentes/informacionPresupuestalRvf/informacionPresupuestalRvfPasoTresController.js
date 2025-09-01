(function () {
    'use strict';

    informacionPresupuestalRvfPasoTresController.$inject = [
        '$scope',
        'tramiteVigenciaFuturaServicio'
    ];

    function informacionPresupuestalRvfPasoTresController(
        $scope,
        tramiteVigenciaFuturaServicio
    ) {
        var vm = this;
        vm.lang = "es";
        vm.nombreComponente = "informacionpresupuestal";
        //vm.arrowIcoDown = "glyphicon-chevron-down";
        //vm.arrowIcoUp = "glyphicon-chevron-up";
        ////vm.notificacionCambiosCapitulos = null;
        ////vm.idNivel = $sessionStorage.idNivel;
        vm.showConpes = false;

        vm.handlerComponentes = [
            { id: 1, componente: 'informacionpresupuestalresumenreprogramacion', handlerValidacion: null, handlerCambios: null, esValido: true },
        ];
        vm.handlerComponentesChecked = {};



        //Inicio
        vm.init = function () {
            vm.setPageEvents();
            vm.inicializarComponenteCheck();

            vm.notificacioncambios({ handler: vm.notificacionCambiosCapitulos, nombreComponente: vm.nombreComponente });
            vm.notificacionvalidacion({ handler: vm.notificacionValidacionEvent, nombreComponente: vm.nombreComponente, handlerCapitulos: vm.capitulos });

        };

        vm.setPageEvents = function () {
            $scope.$watch('vm.tramiteid', function () {
                if (vm.tramiteid && vm.tramiteid !== null && vm.tramiteid !== '') {
                    vm.validaAplicaConpes();
                }
            });
        };

        vm.validaAplicaConpes = function () {            
            tramiteVigenciaFuturaServicio.ValidacionPeriodoPresidencial(vm.tramiteid)
                .then(function (response) {
                    if (response?.data) {
                        if (response.data == 1) {
                            vm.showConpes = true;
                        }

                        //para guardar los capitulos modificados y que se llenen las lunas
                        if (response.data == 0) {
                            guardarCapituloModificado()
                        }
                    }

                }, function (error) {
                    utilidades.mensajeError('No fue posible habilitar la sección CONPES');
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
                'informacionpresupuestalresumenreprogramacion': true,
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
        vm.notificacionValidacionHijos = function (handler, nombreComponente) {
            var indx = vm.handlerComponentes.findIndex(p => p.componente == nombreComponente);
            vm.handlerComponentes[indx].handlerValidacion = handler;
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

        /*------------------------------------Fin Validaciones-----------------------------------*/
    }

    angular.module('backbone').component('informacionPresupuestalRvfPasoTres', {
        templateUrl: "src/app/formulario/ventanas/tramiteReprogramacionVF/componentes/informacionPresupuestalRvf/informacionPresupuestalRvfPasoTres.html",


        controller: informacionPresupuestalRvfPasoTresController,
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
            rolanalista: '@',
        }
    });

})();