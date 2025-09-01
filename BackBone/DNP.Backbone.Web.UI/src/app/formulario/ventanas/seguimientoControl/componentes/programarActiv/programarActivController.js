(function () {
    'use strict';

    programarActivController.$inject = [
        '$scope',
        '$sessionStorage',
        'justificacionCambiosServicio',
    ];

    function programarActivController(
        $scope,
        $sessionStorage,
        justificacionCambiosServicio
    ) {
        var vm = this;
        vm.lang = "es";
        vm.nombreComponente = "programaractiv";
        vm.arrowIcoDown = "glyphicon-chevron-down";
        vm.arrowIcoUp = "glyphicon-chevron-up";
        vm.globalVariables = {
            guiMacroproceso: justificacionCambiosServicio.getIdEtapa($sessionStorage.etapa),
            proyectoId: $sessionStorage.proyectoId,
            idNivel: $sessionStorage.idNivel,
            idInstancia: $sessionStorage.idInstancia,
            bpin: $sessionStorage.idObjetoNegocio
        }

        vm.notificacionCambiosCapitulos = null;
        vm.handlerComponentesChecked = {};
        vm.handlerComponentes = [{ id: 1, componente: 'programaractivprogramaractcap', handlerValidacion: null, handlerCambios: null, esValido: true }];

        // Watching de los componentes hijos vm.handlerComponentes
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

        //Inicio
        vm.init = function () {
            vm.inicializarComponenteCheck();
            vm.notificacioncambios({ handler: vm.notificacionCambiosCapitulos, nombreComponente: vm.nombreComponente });
            vm.notificacionvalidacion({ handler: vm.notificacionValidacionEvent, nombreComponente: vm.nombreComponente, handlerCapitulos: vm.capitulos });
        };

        vm.inicializarComponenteCheck = function () {
            vm.handlerComponentesChecked = {
                'programaractivprogramaractcap': true
            };
        }

        /**
         * Función que se acciona al momento del guardado de info de algún hijo
         * @param {any} nombreComponenteHijo
         */
        vm.guardadoevent = function (nombreComponenteHijo) {
            vm.guardadocomponent({ nombreComponente: vm.nombreComponente, nombreComponenteHijo: nombreComponenteHijo });
        }
        
        /**
        * Función handler que contiene la referencia del binding notificacioncambios del componente justificacionCambios
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
            if (erroresList.length > 0) {
                for (var i = 0; i < vm.handlerComponentes.length; i++) {
                    if (vm.handlerComponentes[i].handlerValidacion) vm.handlerComponentes[i].handlerValidacion(erroresList);
                }
            } 
            else {
                var idSpanAlertComponent = document.getElementById("alert-programarprodprogramarprodcap");
                if (idSpanAlertComponent != undefined) {
                    idSpanAlertComponent.innerHTML = '';
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

        /**
         * Función que permite visualizar el nombre de los capítulos configurados en BD
         * @param {any} listadoCapitulos
         */
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

        /* ------------- Eventos de interfaz ----------- */

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
    }

    angular.module('backbone').component('programaractiv', {

        templateUrl: "src/app/formulario/ventanas/seguimientoControl/componentes/programarActiv/programarActiv.html",
        controller: programarActivController,
        controllerAs: "vm",
        bindings: {
            verificacion: '&',
            notificacioncambios: '&',
            notificacionvalidacion: '&',
            notificacionestado: '&',
            guardadocomponent: '&',
            bpin: '@'
        }
    });

})();