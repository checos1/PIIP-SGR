(function () {
    'use strict';
    verificacionPazSgrController.$inject = [
        '$scope',
        '$sessionStorage',
        'requisitosSgrServicio',
        'transversalSgrServicio',
        'servicioFichasProyectos',
        'FileSaver'
    ];

    function verificacionPazSgrController(
        $scope,
        $sessionStorage,
        requisitosSgrServicio,
        transversalSgrServicio,
        servicioFichasProyectos,
        FileSaver
    ) {
        var vm = this;
        vm.lang = "es";
        vm.nombreComponente = "sgrocadpazrequisitosverificacion";
        vm.nombreComponenteHijo = "sgrocadpazrequisitosverificacionrequisitos";
        vm.arrowIcoDown = "glyphicon-chevron-down";
        vm.arrowIcoUp = "glyphicon-chevron-up";
        vm.notificacionCambiosCapitulos = null;
        vm.DevolverProyecto = {};

        vm.Bpin = $sessionStorage.idObjetoNegocio;
        vm.idInstancia = $sessionStorage.idInstancia;
        vm.notificarGuardado = notificarGuardado;

        vm.handlerComponentes = [
            { id: 1, componente: 'sgrocadpazrequisitosverificacionrequisitos', handlerValidacion: null, handlerCambios: null, esValido: true }
        ];

        vm.handlerComponentesChecked = {};

        vm.obtener = function () {
            vm.inicializarComponenteCheck();
            vm.notificacioncambios({ handler: vm.notificacionCambiosCapitulos, nombreComponente: vm.nombreComponente });
            vm.notificacionvalidacion({ handler: vm.notificacionValidacionEvent, nombreComponente: vm.nombreComponente, handlerCapitulos: vm.capitulos });

            vm.soloLectura = $sessionStorage.soloLectura;
        };

        $scope.$watch('modelo', function () {
            if (vm.modelo != undefined)
                vm.infoArchivo = vm.modelo;
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

        function notificarGuardado() {
            vm.callback({ validacion: null, arg: true, aprueba: false, titulo: '' });
        }

        vm.guardadohijos = function (nombreComponenteHijo) {
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
                'sgrocadpazrequisitosverificacionrequisitos': true
            };
        }


        vm.notificacionCambiosCapitulos = function ({ nombreComponente, nombreComponenteHijo }) {
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

        vm.verPdf = function (row) {
            transversalSgrServicio.SGR_Transversal_LeerParametro("VerificacionOcadPazNivelId")
                .then(function (respuestaParametro) {
                    const nombreFichaTramite = "sgr_ficha_Verificacion_OcadPAz";
                    const borrador = true;
                    const projectId = vm.Bpin;
                    servicioFichasProyectos.ObtenerIdFicha(nombreFichaTramite)
                        .then(function (respuestaFicha) {
                            var fichaPlantilla = {
                                NombreReporte: nombreFichaTramite,
                                IdReporte: respuestaFicha.ID,
                                PARAM_BORRADOR: true,
                                PARAM_BPIN: vm.Bpin,
                                InstanciaId: vm.idInstancia,
                                NivelId: respuestaParametro.data.Valor,
                                TramiteId: vm.Bpin
                            };
                            servicioFichasProyectos.GenerarFichaSGR($.param(fichaPlantilla))
                                .then(function (respuesta) {
                                    if (borrador) {
                                        const nombreArchivo = nombreFichaTramite.replace(/ /gi, "_") + '_' + projectId + '_' + moment().format("YYYYMMDDD_HHMMSS") + "pdf";
                                        const blob = new Blob([respuesta], { type: 'application/pdf' });
                                        const file = new File([blob], nombreArchivo, { type: 'application/pdf' });
                                        FileSaver.saveAs(file, nombreArchivo);
                                    }
                                }, function (error) {
                                    utilidades.mensajeError(error);
                                });
                        }, function (error) {
                            utilidades.mensajeError(error);
                        });
                }, function (error) {
                    utilidades.mensajeError(error);
                });
        }


        vm.refrescarRequisitosVerif = null;
        vm.notificacionCambios = function (handler, nombreComponenteHijo) {
            for (var i = 0; i < vm.handlerComponentes.length; i++) {
                if (handler != null && vm.handlerComponentes[i].componente == nombreComponenteHijo) vm.handlerComponentes[i].handlerCambios = handler;
            }

            if (nombreComponenteHijo == "sgrocadpazrequisitosverificacionrequisitos") {
                vm.refrescarRequisitosVerif = handler;
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

        vm.devolverProyecto = function () {
            var Observacion = document.getElementById("observacionAprobacion");

            if (Observacion.value != null) {
                vm.DevolverProyecto.InstanciaId = vm.idInstancia;
                vm.DevolverProyecto.Bpin = vm.Bpin;
                vm.DevolverProyecto.ProyectoId = vm.Bpin;
                vm.DevolverProyecto.Observacion = Observacion.value;
                vm.DevolverProyecto.DevolverId = true;
                vm.DevolverProyecto.EstadoDevolver = 7; //Returned	Solicitud de Información MGA

                return requisitosSgrServicio.devolverProyecto(vm.DevolverProyecto).then(
                    function (response) {
                        if (response.data || response.statusText === "OK") {
                            if (response.data.Exito) {
                                utilidades.mensajeSuccess("Operación realizada con éxito!", false, false, false);
                                $location.url("/proyectos/pl");
                            } else {
                                swal('', response.data.Mensaje, 'warning');
                            }

                        } else {
                            swal('', "Error al realizar la operación", 'error');
                        }
                    }
                );
            }
            else {
                swal('El formulario no esta diligenciado en su totalidad', 'Revise las campos señalados para que sean validados nuevamente.', 'error');
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
            } else {
                var errorElements = document.getElementsByClassName('errorSeccionRequisitos');
                var testDivs = Array.prototype.filter.call(errorElements, function (errorElement) {
                    errorElement.innerHTML = "";
                    errorElement.classList.add('hidden');
                });

                var campomensajeerror = document.getElementById("alert-" + vm.nombreComponenteHijo);
                if (campomensajeerror != undefined) {
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
    }

    angular.module('backbone').component('verificacionPazSgr', {
        templateUrl: "/src/app/formulario/ventanas/SGR/ocadPaz/requisitos/verificacion/verificacionPazSgr.html",
        controller: verificacionPazSgrController,
        controllerAs: "vm",
        bindings: {
            callback: '&',
            notificacionvalidacion: '&',
            notificacionestado: '&',
            notificacioncambios: '&',
            guardadocomponent: '&'
        }
    });
})();