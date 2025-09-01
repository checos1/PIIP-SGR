(function () {
    'use strict';

    focalizacionAjustesSinTramiteSgpController.$inject = [
        '$scope',
        '$sessionStorage',
        'constantesBackbone',
        'focalizacionAjustesSinTramiteSgpServicio',
    ];

    function focalizacionAjustesSinTramiteSgpController(
        $scope,
        $sessionStorage,
        constantesBackbone,
        focalizacionAjustesSinTramiteSgpServicio
    ) {
        var vm = this;
        vm.lang = "es";
        vm.arrowIcoDown = "glyphicon-chevron-down";
        vm.arrowIcoUp = "glyphicon-chevron-up";

        /* ------------ Estructura necesaria para botón validar --------------- */

        vm.handlerComponentesChecked = {};
        vm.nombreComponente = "focalizacionpolsgp";
        vm.handlerComponentes = [
            { id: 1, componente: 'focalizacionpolsgppoliticastransvsintramitesgp ', handlerValidacion: null, handlerCambios: null, esValido: true },
            { id: 2, componente: 'focalizacionpolsgpcategoriapoliticassintramitesgp', handlerValidacion: null, handlerCambios: null, esValido: true },
            { id: 3, componente: 'focalizacionpolsgpresumendefocalisintramitesgp', handlerValidacion: null, handlerCambios: null, esValido: true },
            { id: 4, componente: 'focalizacionpolsgpSolicitarConAjusintramitesgp', handlerValidacion: null, handlerCambios: null, esValido: true },
            { id: 5, componente: 'focalizacionpolsgpcrucepoliticassintramitesgp', handlerValidacion: null, handlerCambios: null, esValido: true }
        ];

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

        vm.init = function () {
            $sessionStorage.esAjuste = true;
            vm.inicializarComponenteCheck();
            vm.notificacionvalidacion({ handler: vm.notificacionValidacionEvent, nombreComponente: vm.nombreComponente, handlerCapitulos: vm.capitulos });
            vm.notificacioncambios({ handler: vm.notificacionCambiosCapitulos, nombreComponente: vm.nombreComponente });
        };

        vm.inicializarComponenteCheck = function () {
            vm.handlerComponentesChecked = {
                'focalizacionpolsgppoliticastransvsintramitesgp': true,
                'focalizacionpolsgpcategoriapoliticassintramitesgp': true,
                'focalizacionpolsgpresumendefocalisintramitesgp': true,
                'focalizacionpolsgpSolicitarConAjusintramitesgp': true,
                focalizacionpolsgpcrucepoliticassintramitesgp : true
            };
        }

        vm.guardado = function (nombreComponenteHijo) {
            vm.guardadocomponent({ nombreComponente: vm.nombreComponente, nombreComponenteHijo: nombreComponenteHijo });
        }

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

       
        /**
         * Función que crea la referencia handler con los componentes hijos
         * IMPORTANTE: Es la función que crea la referencia con los componentes hijos para notificar cambios externos
         * @param {any} handler
         * @param {any} nombreComponente
         */
        vm.notificacionCambiosCapitulosExternos = function (handler, nombreComponente) {
            vm.handlerComponentes.forEach(comp => {
                if (comp.componente == nombreComponente) {
                    comp.handlerCambios = handler;
                }
            });
        };

        /**
        * Función handler que contiene la referencia del binding notificacioncambios.
        * IMPORTANTE: En esta función se realiza la notificación a todos los componentes hijos los cambios externos
        * @param {any} param0
        */
        vm.notificacionCambiosCapitulos = function ({ nombreComponente, nombreComponenteHijo }) {
            console.log(" vm.notificacionCambiosCapitulos", nombreComponenteHijo)
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


        /* --------------------------------- Notificación de Validaciones ---------------------------*/

        /**
         * Función que recibe listado de errores referentes a la sección de justificación.
         * IMPORTANTE: Envía a sus hijos el listado de errores
         * @param {any} errores
         */
        vm.notificacionValidacionEvent = function (listErrores) {
            var erroresList = listErrores.errores.filter(p => p.Seccion == vm.nombreComponente);
            vm.inicializarComponenteCheck();
            vm.esValido = true;

            for (var i = 0; i < vm.handlerComponentes.length; i++) {
                if (vm.handlerComponentes[i].handlerValidacion)
                    try {
                        vm.handlerComponentes[i].handlerValidacion(erroresList);
                    } catch (error) {
                        console.error('¡¡Tiene ERRORES - handlerValidacion del componente = ' + vm.handlerComponentes[i].componente + '!!');
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
            if (indx != -1) vm.handlerComponentes[indx].handlerValidacion = handler;
        };

        // TODO: Validar que se esté usando
        vm.notificarRefrescoFuentes = null;
        // TODO: Validar que se esté usando
        vm.notificarRefresco = function (handler, nombreComponente) {
            if (nombreComponente == "focalizacionpolsgp") {
                vm.notificarRefrescoFuentes = handler;
            }
        };

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

        /* --------------------------------- Notificación de Cambios de estado ---------------------------*/

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
            var idSpanAlertComponent = document.getElementById("alert-" + nombreComponente + "-1");
            if (idSpanAlertComponent != undefined) {
                if (!esValido) {
                    idSpanAlertComponent.classList.add("ico-advertencia");
                } else {
                    idSpanAlertComponent.classList.remove("ico-advertencia");
                }
            }
        }
    }

    angular.module('backbone').component('focalizacionAjustesSinTramiteSgp', {
        templateUrl: "src/app/formulario/ventanas/SGP/ajustesProyectoSGP/componentes/focalizacion/focalizacionAjustesSinTramiteSgp.html",
        controller: focalizacionAjustesSinTramiteSgpController,
        controllerAs: "vm",
        bindings: {
            notificacionvalidacion: '&',
            notificacioncambios: '&',
            notificacionestado: '&',
            guardadocomponent: '&'
        }
    });

})();