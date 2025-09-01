(function () {
    'use strict';

    datosgeneralesController.$inject = [
        '$scope',
        '$sessionStorage',
        'constantesBackbone',
        'datosgeneralesServicio',
    ];

    function datosgeneralesController(
        $scope,
        $sessionStorage,
        constantesBackbone,
        datosgeneralesServicio
    ) {
        var vm = this;
        vm.lang = "es";
        vm.nombreComponente = "datosgenerales";
        vm.arrowIcoDown = "glyphicon-chevron-down";
        vm.arrowIcoUp = "glyphicon-chevron-up";
        vm.notificacionCambiosCapitulos = null;

        vm.handlerComponentes = [
            { id: 1, componente: 'datosgeneralesrelacionconlapl', handlerValidacion: null, handlerCambios: null, esValido: true },
            { id: 2, componente: 'datosgeneraleshorizonte', handlerValidacion: null, handlerCambios: null, esValido: true },
            { id: 3, componente: 'datosgeneralesindicadoresdepr', handlerValidacion: null, handlerCambios: null, esValido: true },
            { id: 4, componente: 'datosgeneraleslocalizaciones', handlerValidacion: null, handlerCambios: null, esValido: true },
            { id: 5, componente: 'datosgeneralesbeneficiariosTotales', handlerValidacion: null, handlerCambios: null, esValido: true }
        ];
        vm.handlerComponentesChecked = {};

        //Inicio
        vm.init = function () {
            vm.inicializarComponenteCheck();
            vm.notificacioncambios({ handler: vm.notificacionCambiosCapitulos, nombreComponente: vm.nombreComponente });
            vm.notificacionvalidacion({ handler: vm.notificacionValidacionEvent, nombreComponente: vm.nombreComponente, handlerCapitulos: vm.capitulos});
        };

        $scope.$watch('modelo', function () {
            if (vm.modelo != undefined)
                vm.infoArchivo = vm.modelo;
            vm.modelo = vm.infoArchivoDatos;
        });

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
            vm.guardadocomponent({ nombreComponente: vm.nombreComponente, nombreComponenteHijo: nombreComponenteHijo});
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
                'datosgeneralesrelacionconlapl': true,
                'datosgeneraleshorizonte': true,
                'datosgeneralesindicadoresdepr': true,
                'datosgeneraleslocalizaciones': true,
                'datosgeneralesbeneficiariosTotales': true,
            };
        }

        /**
        * Función handler que contiene la referencia del binding notificacioncambios.
        * @param {any} param0
        */
        vm.notificacionCambiosCapitulos = function ({nombreComponente, nombreComponenteHijo }) {
           for (var i = 0; i < vm.handlerComponentes.length; i++) {
                if (vm.handlerComponentes[i].handlerCambios != null) {
                    try {
                        vm.handlerComponentes[i].handlerCambios(nombreComponenteHijo);
                    } catch (error) {
                        console.error('¡¡Tiene ERRORES - handlerCambios del componente = ' + vm.handlerComponentes[i].componente + '!!');
                    }
                }
            }
        };

        /**
         * Función que crea la referencia hanlder con los componentes hijos
         * IMPORTANTE:Es la función que se ejecuta cuando algún componente externo guarda cambios.
         * @param {any} handler
         * @param {any} nombreComponente
         */
        vm.notificacionCambiosCapitulosExternos = function (handler, nombreComponente) {
            for (var i = 0; i < vm.handlerComponentes.length; i++) {                
                if (vm.handlerComponentes[i].componente == nombreComponente) {
                    vm.handlerComponentes[i].handlerCambios = handler;
                    break;
                }
            }
        };

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
            for (var i = 0; i < vm.handlerComponentes.length; i++) {
                if (vm.handlerComponentes[i].handlerValidacion) vm.handlerComponentes[i].handlerValidacion(erroresList);
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
                idSpanAlertComponent.innerHTML = (!esValido) ? '<img class="img-advertencia mr-2" src="Img/u11.svg"></img>' : '';
            }
        }
    }

    angular.module('backbone').component('datosgenerales', {

        templateUrl: "src/app/formulario/ventanas/ajustes/componentes/datosgenerales/datosgenerales.html",
        controller: datosgeneralesController,
        controllerAs: "vm",
        bindings: {
            verificacion: '&',
            modelo: '=',
            notificacioncambios: '&',
            notificacionvalidacion: '&',
            notificacionestado: '&',
            guardadocomponent: '&'
        }
    });

})();