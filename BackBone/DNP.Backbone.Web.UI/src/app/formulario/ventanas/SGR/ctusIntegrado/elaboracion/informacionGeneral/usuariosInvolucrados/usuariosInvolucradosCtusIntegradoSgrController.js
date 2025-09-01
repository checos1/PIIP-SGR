(function () {
    'use strict';

    usuariosInvolucradosCtusIntegradoSgrController.$inject = [
        '$scope',
        '$sessionStorage',
    ];

    function usuariosInvolucradosCtusIntegradoSgrController(
        $scope,
        $sessionStorage,
    ) {
        var vm = this;
        vm.IdNivel = $sessionStorage.idNivel;
        vm.IdObjetoNegocio = $sessionStorage.InstanciaSeleccionada != undefined ? $sessionStorage.InstanciaSeleccionada.IdObjetoNegocio : "";
        vm.nombreComponente = "sgrctusintegradoelaboracioninfogeneralusuariosinvolucrados";
        vm.tipotramiteid = $sessionStorage.tipotramiteid;
        vm.tramiteid = $sessionStorage.tipotramiteid;
        vm.nombreComponenteHijo = "sgrctusintegradoelaboracioninfogeneralusuariosinvolucrados";
        vm.notificacionCambiosCapitulos = null;
        vm.tipoConceptoViabilidadId = 3;
        vm.arrowIcoDown = "glyphicon-chevron-down";
        vm.arrowIcoUp = "glyphicon-chevron-up";

        vm.handlerComponentes = [
            { id: 1, componente: 'sgrctusintegradoelaboracioninfogeneralusuariosinvolucrados', handlerValidacion: null, handlerCambios: null, esValido: true }
        ];
        vm.handlerComponentesChecked = {};

        vm.init = function () {
            vm.inicializarComponenteCheck();
            vm.notificacioncambios({ handler: vm.notificacionCambiosCapitulos, nombreComponente: vm.nombreComponente });
            vm.notificacionvalidacion({ handler: vm.notificacionValidacionEvent, nombreComponente: vm.nombreComponente, handlerCapitulos: vm.capitulos });
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
            vm.guardadoevent({ nombreComponenteHijo: vm.nombreComponente });
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
                'soportealojararchivo': true
            };
        }

        vm.notificacionCambiosCapitulos = function ({ nombreComponente, nombreComponenteHijo }) {
            var x = 0;
            for (var i = 0; i < vm.handlerComponentes.length; i++) {
                if (vm.handlerComponentes[i].handlerCambios != null) {
                    vm.handlerComponentes[i].handlerCambios(nombreComponenteHijo);
                }
            }
        };

        vm.notificacionCambiosCapitulosExternos = function (handler, nombreComponente) {
            for (var i = 0; i < vm.handlerComponentes.length; i++) {
                if (vm.handlerComponentes[i].componente != nombreComponente) {
                    vm.handlerComponentes[i].handlerCambios = handler;
                    break;
                }
            }
        };

        /**
        * Función que recibe listado de errores referentes a la sección de justificación
        * envía a sus hijos el listado de errores
        * @param {any} errores
        */
        vm.notificacionValidacionEvent = function (listErrores) {
            if (listErrores != undefined) {
                var erroresRelacionconlapl = listErrores.find(p => (p.Seccion + p.Capitulo) == vm.nombreComponente);
                var isValid = true;
                if (erroresRelacionconlapl != undefined) {
                    var erroresJson = erroresRelacionconlapl.Errores == "" ? [] : JSON.parse(erroresRelacionconlapl.Errores);
                    isValid = (erroresJson == null || erroresJson.length == 0);
                    if (!isValid) {
                        erroresJson[vm.nombreComponente].forEach(p => {
                            var nameArr = p.Error.split('-');
                            var TipoError = nameArr[0].toString();
                            if (TipoError == 'SGRERRSEC') {
                                vm.validarSeccion(TipoError, nameArr[1].toString(), p.Descripcion, false);
                            }
                            else {
                                vm.validarValores(nameArr[0].toString(), p.Descripcion, false);
                            }
                        });
                    }
                }
                vm.notificacionestado({ nombreComponente: vm.nombreComponente, esValido: isValid });
            }
        }

        vm.validarValores = function (pregunta, errores, esValido) {
            var idSpanAlertComponent = document.getElementById("alert-" + vm.nombreComponente + pregunta);
            if (idSpanAlertComponent != undefined) {
                if (!esValido) {
                    idSpanAlertComponent.classList.add("ico-advertencia");
                } else {
                    idSpanAlertComponent.classList.remove("ico-advertencia");
                }
            }
        }

        vm.validarSeccion = function (tipoError, seccion, errores, esValido) {
            var campomensajeerror = document.getElementById(tipoError + seccion);
            if (campomensajeerror != undefined) {
                if (!esValido) {
                    campomensajeerror.innerHTML = "<span class='d-inline-block ico-advertencia'></span><span class='pl-2'>" + errores + "</span>";
                    campomensajeerror.classList.remove('hidden');
                } else {
                    campomensajeerror.classList.remove("ico-advertencia");
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
            var idSpanAlertComponent = document.getElementById("alert-" + nombreComponente);
            if (idSpanAlertComponent != undefined) {
                if (!esValido) {
                    idSpanAlertComponent.classList.add("ico-advertencia");
                } else {
                    idSpanAlertComponent.classList.remove("ico-advertencia");
                }
            }
        }





        /*------------------------------------Fin Validaciones-----------------------------------*/

    }



    angular.module('backbone').component('usuariosInvolucradosCtusIntegradoSgr', {
        templateUrl: "/src/app/formulario/ventanas/SGR/ctusIntegrado/elaboracion/informacionGeneral/usuariosInvolucrados/usuariosInvolucradosCtusIntegradoSgr.html",
        controller: usuariosInvolucradosCtusIntegradoSgrController,
        controllerAs: "vm",
        bindings: {
            callback: '&',
            notificacionvalidacion: '&',
            notificacionestado: '&',
            notificacioncambios: '&',
            guardadocomponent: '&',
            nivel: '@',
            section: '@',
            rol: '@'
        },

    });

})();