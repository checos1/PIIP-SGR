(function () {
    'use strict';

    justificacionCambiosSinTramiteSgpController.$inject = [
        '$scope',        
        '$sessionStorage',
        '$element',
        'justificacionCambiosSinTramiteSgpServicio'
    ];

    function justificacionCambiosSinTramiteSgpController(
        $scope,        
        $sessionStorage,
        $element,
        justificacionCambiosSinTramiteSgpServicio
    ) {
        var vm = this;

        vm.lang = "es";
        vm.arrowIcoDown = "glyphicon-chevron-down";
        vm.arrowIcoUp = "glyphicon-chevron-up";
        vm.nombreComponente = "justificacionsintramitesgp";
        vm.capitulosModificados = [];
        vm.esValido = true;

        vm.handlerComponentes = [
            { id: 1, componente: 'datosgeneraleshorizonte', handlerValidacion: null, handlerCambios: null, esValido: true},
            { id: 2, componente: 'datosgeneralesbeneficiarios', handlerValidacion: null, handlerCambios: null, esValido: true},
            { id: 3, componente: 'recursosfuentesdefinanc', handlerValidacion: null, handlerCambios: null, esValido: true },
            { id: 4, componente: 'datosgeneralesindicadoresdepr', handlerValidacion: null, handlerCambios: null, esValido: true },
            { id: 5, componente: 'recursoscostosdelasacti', handlerValidacion: null, handlerCambios: null, esValido: true },
            { id: 6, componente: 'datosgeneraleslocalizaciones', handlerValidacion: null, handlerCambios: null, esValido: true },
            { id: 7, componente: 'recursosregionalizacion', handlerValidacion: null, handlerCambios: null, esValido: true },
            { id: 8, componente: 'focalizacioncategoriaspolit', handlerValidacion: null, handlerCambios: null, esValido: true }
        ];

        vm.handlerComponentesChecked = {};

        vm.init = function () {
            vm.loadCapitulosModificados();
            vm.inicializarComponenteCheck();
            vm.notificacioncambios({ handler: vm.notificacionCambiosJustificacion, nombreComponente: vm.nombreComponente });
            vm.notificacionvalidacion({ handler: vm.notificacionValidacionEvent, nombreComponente: vm.nombreComponente });
        };

        vm.loadCapitulosModificados = function () {
            var guidMacroproceso = justificacionCambiosSinTramiteSgpServicio.getIdEtapa($sessionStorage.etapa);
            var idProyecto = $sessionStorage.proyectoId;
            var idInstancia = $sessionStorage.idInstancia;
            vm.obtenerCapitulosModificados(guidMacroproceso, idProyecto, idInstancia);
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

            setTimeout(function () {
                var contenido = $('#divresumenrff');
                var altura = contenido.height();
                $('#resumenrff').height(altura + 200);
            }, 200
            );
        }

        vm.obtenerCapitulosModificados = function (guiMacroproceso, idProyecto, idInstancia) {
            justificacionCambiosSinTramiteSgpServicio.obtenerCapitulosModificados(guiMacroproceso, idProyecto, idInstancia)
                .then(function (response) {
                    vm.capitulosModificados = response.data.filter(item1 =>
                        vm.handlerComponentes.some(item2 => item2.componente === item1.NombreModificado)
                    );
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
                'datosgeneraleshorizonte': true,
                'datosgeneralesbeneficiarios': true,
                'recursosfuentesdefinanc': true,
                'datosgeneralesindicadoresdepr': true,
                'recursoscostosdelasacti': true,
                'datosgeneraleslocalizaciones': true,
                'recursosregionalizacion': true,
                'focalizacioncategoriaspolit': true,
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
            //if (erroresList.length > 0) {
                for (var i = 0; i < vm.handlerComponentes.length; i++) {
                    if (vm.handlerComponentes[i].handlerValidacion != undefined && vm.handlerComponentes[i].handlerValidacion != null) vm.handlerComponentes[i].handlerValidacion(erroresList);
                }
            //}
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
                idSpanAlertComponent.innerHTML = (!esValido) ? '<img class="img-advertencia mr-2" src="Img/u11.svg"/>' : '';
            }
        }

        /* --------------------------------- Justificación ---------------------------*/


        /**
         * Función handler que contiene la referencia del binding notificacioncambios del componente justificacionCambios
         * @param {any} param0
         */
        vm.notificacionCambiosJustificacion = function ({ nombreComponente, nombreComponenteHijo }) {
            vm.loadCapitulosModificados();
        };
        
    }

    angular.module('backbone').component('justificacionCambiosSinTramiteSgp', {
        templateUrl: "src/app/formulario/ventanas/SGP/comun/justificacionCambios/justificacionCambiosSinTramiteSgp.html",
        controller: justificacionCambiosSinTramiteSgpController,
        controllerAs: "vm",
        bindings: {
            notificacioncambios: '&',
            notificacionvalidacion: '&',
            notificacionestado: '&'

        }
    });

})();