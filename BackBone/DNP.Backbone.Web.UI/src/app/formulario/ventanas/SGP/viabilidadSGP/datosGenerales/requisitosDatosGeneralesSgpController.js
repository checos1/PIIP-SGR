(function () {
    'use strict';

    requisitosDatosGeneralesSgpController.$inject = ['$scope', 'requisitosSgpServicio', 'utilidades', '$sessionStorage'];

    function requisitosDatosGeneralesSgpController(
        $scope,
        requisitosSgpServicio,
        utilidades,
        $sessionStorage,
    ) {
        var vm = this;
        vm.nombreComponente = "sgpviabilidadrequisitosdatosgenerales";
        vm.nombreComponenteHijo = "sgpviabilidadrequisitosdatosgeneralesagregarsectoressgp";
        vm.notificacionCambiosCapitulos = null;
        vm.notificarGuardado = notificarGuardado;

        vm.handlerComponentes = [
            { id: 1, componente: 'sgpviabilidadrequisitosdatosgeneralesagregarsectoressgp', handlerValidacion: null, handlerCambios: null, esValido: true }

        ];
        vm.handlerComponentesChecked = {};

        vm.componentesRefreshSector = [
            "sgpviabilidadrequisitosverificacionrequisitossgp"
        ];

        vm.init = function () {
            vm.inicializarComponenteCheck();
            vm.notificacioncambios({ handler: vm.notificacionCambiosCapitulos, nombreComponente: vm.nombreComponente, nombreComponenteHijo: vm.nombreComponenteHijo });
            vm.notificacionvalidacion({ handler: vm.notificacionValidacionEvent, nombreComponente: vm.nombreComponente, handlerCapitulos: vm.capitulos });
        };

        function notificarGuardado() {
            vm.callback();
        }

        vm.guardadohijos = function (nombreComponenteHijo) {
            vm.callback();
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

        vm.guardado = function (nombreComponenteHijo) {
            vm.callback();
            if (nombreComponenteHijo == "sgpviabilidadrequisitosdatosgeneralesagregarsectoressgp") {
                vm.guardadocomponent({ nombreComponente: vm.nombreComponente, nombreComponenteHijo: nombreComponenteHijo });
                vm.notificarrefrescoSector();
            }

            if (nombreComponenteHijo == "sgpviabilidadrequisitosdatosgeneralesagregarsectoressgp") {
                vm.guardadocomponent({ nombreComponente: vm.nombreComponente, nombreComponenteHijo: nombreComponenteHijo });
            }
        }

        vm.notificarrefrescoSector = null;
        vm.notificarRefresco = function (handler, nombreComponente) {
            if (nombreComponente == "sgpviabilidadrequisitosdatosgeneralesagregarsectoressgp") {
                vm.notificarrefrescoSector = handler;
            }

        };

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
                'sgpviabilidadrequisitosdatosgeneralesagregarsectoressgp': true
            };
        }

        vm.notificacionCambiosCapitulos = function ({ nombreComponente, nombreComponenteHijo }) {
            var x = 0;
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

        vm.notificacionCambiosCapitulosExternos = function (handler, nombreComponente) {
            for (var i = 0; i < vm.handlerComponentes.length; i++) {
                if (vm.handlerComponentes[i].componente == nombreComponente) {
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
            var erroresList = listErrores.errores.filter(p => p.Seccion == vm.nombreComponente);
            vm.inicializarComponenteCheck();
            vm.esValido = true;
            if (erroresList.length > 0) {

                var erroresRelacionconlapl = erroresList.find(p => (p.Seccion + p.Capitulo) == vm.nombreComponenteHijo);
                var erroresJson = erroresRelacionconlapl.Errores == "" ? [] : JSON.parse(erroresRelacionconlapl.Errores);
                var isValid = (erroresJson == null || erroresJson.length == 0);

                if (!isValid) {

                    erroresJson[vm.nombreComponenteHijo].forEach(p => {
                        var nameArr = p.Error.split('-');
                        var TipoError = nameArr[0].toString();
                      
                        var idSpanAlertComponent = document.getElementById("SGRERRSEC" + vm.nombreComponente );
                        if (idSpanAlertComponent != undefined) {
                            idSpanAlertComponent.innerHTML = p.Descripcion;
                            idSpanAlertComponent.classList.remove('hidden');
                        }
                    });

                  
                }
                vm.notificacionestado({ nombreComponente: vm.nombreComponente, esValido: isValid });


                for (var i = 0; i < vm.handlerComponentes.length; i++) {
                    if (vm.handlerComponentes[i].handlerValidacion) vm.handlerComponentes[i].handlerValidacion(erroresList);
                }
            } else {
                var errorElements = document.getElementsByClassName('errorSeccionRequisitosGenerales');
                var testDivs = Array.prototype.filter.call(errorElements, function (errorElement) {
                    errorElement.innerHTML = "";
                    errorElement.classList.add('hidden');
                });

                var campomensajeerror = document.getElementById("alert-" + vm.nombreComponenteHijo);
                if (campomensajeerror != undefined) {
                    campomensajeerror.classList.remove("ico-advertencia");
                }

                vm.notificacionestado({ estado: true, nombreComponente: vm.nombreComponente});
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

        function guardarCapituloModificado() {
            ObtenerSeccionCapitulo();
            var data = {
                Justificacion: "",
                SeccionCapituloId: vm.seccionCapitulo,
                InstanciaId: vm.idInstancia,
                Modificado: 1,
                cuenta: 1
            }
            justificacionCambiosServicio.guardarCambiosFirme(data)
                .then(function (response) {
                    if (response.data.Exito) {
                        vm.guardadocomponent({ nombreComponenteHijo: vm.nombreComponente });


                    }
                    else {
                        utilidades.mensajeError(response.data.Mensaje + " Capitulo Modificado");
                        vm.guardadocomponent({ nombreComponenteHijo: vm.nombreComponente });

                    }
                });
        }


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

    angular.module('backbone').component('requisitosDatosGeneralesSgp', {
        templateUrl: "/src/app/formulario/ventanas/SGP/viabilidadSGP/datosGenerales/requisitosDatosGeneralesSgp.html",

        controller: requisitosDatosGeneralesSgpController,
        controllerAs: "vm",
        bindings: {
            callback: '&',
            notificacionvalidacion: '&',
            notificacionestado: '&',
            notificacioncambios: '&',
            notificarrefresco: '&',
            guardadocomponent: '&',
        },
    });

})();