(function () {
    'use strict';

    reprogramacionproductovigfuturaController.$inject = ['$scope', 'ajustesConTramiteServicio'];

    function reprogramacionproductovigfuturaController(
        $scope,
        comunesServicio

    ) {
        var vm = this;
        vm.nombreComponente = "reprogramacionvfreprogramacionporproducto";

        vm.notificacionCambiosCapitulos = null;
        vm.modificoDatos = '0';

        vm.handlerComponentes = [
            { id: 1, componente: vm.nombreComponente, handlerValidacion: null, handlerCambios: null, esValido: true },
        ];
        vm.handlerComponentesChecked = {};




        vm.init = function () {


            this.notificacionvalidacion({ handler: this.notificacionValidacionPadre, nombreComponente: this.nombreComponente, esValido: true });
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
            vm.notificacionestado({ esValido: estado, nombreComponente: vm.nombreComponente });
        });

        vm.inicializarComponenteCheck = function () {
            vm.handlerComponentesChecked = {
                'reprogramacionvfreprogramacionporproducto': true
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
            var erroresList = listErrores.filter(p => p.Capitulo == "reprogramacionporproducto");
            vm.inicializarComponenteCheck();
            vm.esValido = true;
            if (erroresList.length > 0) {
                for (var i = 0; i < vm.handlerComponentes.length; i++) {
                    if (vm.handlerComponentes[i].handlerValidacion) vm.handlerComponentes[i].handlerValidacion(erroresList);
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
                                var error = p.Error.substring(0, 5);
                                if (vm.errores[error] != undefined) vm.errores[error](p.Error, p.Descripcion, error);
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

            var campogeneral = document.getElementsByClassName('error-vfproducto');
            for (let i = 0; i < campogeneral.length; i++) {
                if ((campogeneral[i].id.includes('RVFP-') || campogeneral[i].id.includes('RVFC-'))
                    && !campogeneral[i].classList.contains('hidden')) {
                    campogeneral[i].innerHTML = "";
                    campogeneral[i].classList.add('hidden');
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


        vm.deshabilitarBotonDevolverAsociarProyectoVF = function () {
            vm.callback();

        }


        vm.validarReprogramacionProducto = function (error, errores, nombreerror) {
            var campoObligatorioJustificacion = document.getElementById(error);
            if (campoObligatorioJustificacion != undefined) {
                campoObligatorioJustificacion.innerHTML = "<span class='d-inline-block ico-advertencia'></span>" +
                    "<span></span>";
                campoObligatorioJustificacion.classList.remove('hidden');
            }
            if (nombreerror.includes('RVFC-')) {
                var listaTmp = nombreerror.split('-');
                var nombrecampogeneral = '';
                for (var i = 0; i < listaTmp.length - 1; i++) {
                    nombrecampogeneral += (listaTmp[i] + '-');
                }
                var campogeneral = document.getElementById(nombrecampogeneral);
                if (campogeneral != undefined) {
                    campogeneral.innerHTML = "<span class='d-inline-block ico-advertencia'></span>" +
                        "<span>" + errores + "</span>";
                    campogeneral.classList.remove('hidden');
                }
            }
            else {
                var campogeneral = document.getElementById(nombreerror);
                if (campogeneral != undefined) {
                    campogeneral.innerHTML = "<span class='d-inline-block ico-advertencia'></span>" +
                        "<span>" + errores + "</span>";
                    campogeneral.classList.remove('hidden');
                }
            }
            if (error.includes('RVFC-')) {
                var listaTmp = error.split('-');
                var nombrecampogeneral = '';
                for (var i = 0; i < listaTmp.length - 1; i++) {
                    nombrecampogeneral += (listaTmp[i] + '-' + listaTmp[i + 1]);
                    break;
                }
                var campogeneral = document.getElementById(nombrecampogeneral);
                if (campogeneral != undefined) {
                    campogeneral.innerHTML = "<span class='d-inline-block ico-advertencia'></span>" +
                        "<span>" + errores + "</span>";
                    campogeneral.classList.remove('hidden');
                }
            }

        }


        function ObtenerSeccionCapitulo() {
            const span = document.getElementById('id-capitulo-' + vm.nombreComponente);
            vm.seccionCapitulo = span.textContent;


        }


        vm.errores = {
            'RVFP-': vm.validarReprogramacionProducto,
            'RVFC-': vm.validarReprogramacionProducto



        }


        /*------------------------------------Fin Validaciones-----------------------------------*/

    }



    angular.module('backbone').component('reprogramacionproductovigfutura', {
        templateUrl: "/src/app/formulario/ventanas/ajustesConTramite/componentes/reprogramacion/reprogramacionproductovigfutura.html",
        controller: reprogramacionproductovigfuturaController,
        controllerAs: "vm",
        bindings: {
            callback: '&',
            notificacionvalidacion: '&',
            notificacionestado: '&',
            notificacioncambios: '&',
            guardadocomponent: '&',
            tipotramiteid: '@',
            tramiteid: '@',
            nivel: '@',
            rol: '@',
            section: '@',
            nombrecomponentepaso: '@',
            vigenciaadicionada: "="
        },

    });

})();