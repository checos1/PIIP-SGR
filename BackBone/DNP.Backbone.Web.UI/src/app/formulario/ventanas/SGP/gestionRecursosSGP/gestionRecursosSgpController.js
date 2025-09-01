(function () {
    'use strict';

    gestionRecursosSgpController.$inject = [
        '$sessionStorage',
        'utilidades',
        'gestionRecursosServicio',
        'justificacionCambiosServicio',
        '$scope',
        'utilsValidacionSeccionCapitulosServicio',
        'transversalSgpServicio'
    ];

    function gestionRecursosSgpController(
        $sessionStorage,
        utilidades,
        gestionRecursosServicio,
        justificacionCambiosServicio,
        $scope,
        utilsValidacionSeccionCapitulosServicio,
        transversalSgpServicio
    ) {
        var vm = this;
        vm.lang = "es";
        vm.guiMacroproceso = justificacionCambiosServicio.getIdEtapa($sessionStorage.etapa);
        vm.ProyectoId = $sessionStorage.proyectoId;
        vm.idInstancia = $sessionStorage.idInstancia;
        vm.IdNivel = $sessionStorage.idNivel;
        vm.BPIN = $sessionStorage.idObjetoNegocio;
        vm.validaCuestionario = false;
        vm.eventoValidar = eventoValidar;
        vm.eventoHabilitarEdicion = eventoHabilitarEdicion;

        vm.notificacionCambiosCapitulos = null;

        $("#editarButton").hide();
        $("#validarButton").show();

        //Métodos
        vm.mostrarOcultarFlujo = mostrarOcultarFlujo;

        /**
       * Listado de componentes hijos, obligatorio para estructura de validación
       * */
        vm.handlerComponentes = [
            { id: 1, componente: 'sgpsolicitudrecursosdatosgenerales', handlerValidacion: null, handlerCambios: null, handlerCapitulos: [] },
            { id: 2, componente: 'sgprecursos', handlerValidacion: null, handlerCambios: null, handlerCapitulos: [] },
            { id: 3, componente: 'sgpsolicitudrecursosfocalizacion', handlerValidacion: null, handlerCambios: null, handlerCapitulos: [] },
            { id: 4, componente: 'sgpgraprobaciondefinitivausuarios', handlerValidacion: null, handlerCambios: null, handlerCapitulos: [] },
            { id: 5, componente: 'sgpgraprobaciondefinitiva', handlerValidacion: null, handlerCambios: null, handlerCapitulos: [] }
        ];

        vm.handlerComponentesChecked = {};


        //Inicio
        vm.init = function () {
            $sessionStorage.IdMacroproceso = vm.guiMacroproceso;
            vm.inicializarComponenteCheck(true);
            vm.ObtenerLocalizacionProyecto(vm.BPIN);

            vm.callback({ validacion: { evento: vm.eventoValidar }, arg: true, aprueba: false, titulo: '', ocultarDevolver: true });

            utilsValidacionSeccionCapitulosServicio.getSeccionCapitulo(vm.guiMacroproceso, $sessionStorage.idNivel, $sessionStorage.idFlujoIframe).then(function (respuesta) {
                vm.setCapitulosHijos(respuesta.data);

                const span = document.getElementById('d' + respuesta.data[0].SeccionModificado);
                if (span != undefined && span != null) {
                    span.classList.add("active");
                }
            });
        };

        vm.$onDestroy = function () {
            transversalSgpServicio.limpiarObservadores();
        };

        vm.obtenerDatosGeneralesProyecto = function (ProyectoId, NivelId) {
            return gestionRecursosServicio.obtenerDatosGeneralesProyecto(ProyectoId, NivelId).then(
                //return gestionRecursosServicio.obtenerDatosGeneralesProyecto(ProyectoId, '88ea329d-f240-4868-9df7-86c74fb2ecfa').then(
                function (respuesta) {
                    vm.DatosGeneralesProyectos = respuesta.data;
                }
            );
        }

        vm.ObtenerLocalizacionProyecto = function (Bpin) {
            return gestionRecursosServicio.ObtenerLocalizacionProyecto(Bpin).then(
                function (respuesta) {
                    if (respuesta.data != null && respuesta.data != "") {
                        var arreglolistaLocalizaciones = jQuery.parseJSON(respuesta.data);
                        for (var ls = 0; ls < arreglolistaLocalizaciones.Localizacion.length; ls++) {
                            vm.obtenerDatosGeneralesProyecto(arreglolistaLocalizaciones.ProyectoId, vm.IdNivel);
                            $sessionStorage.ProyectoId = arreglolistaLocalizaciones.ProyectoId;
                            break;
                        }
                    }
                }
            );
        }

        vm.abrirMGA = function () {
            gestionRecursosServicio.ObtenerTokenMGA(vm.BPIN, tipoUsuarioAutenticado).then(function (respuesta) {
                window.open(respuesta.data, '_blank').focus();
            });
        };

        function mostrarOcultarFlujo() {
            vm.mostrarFlujo = !vm.mostrarFlujo;
        }

        function mostrarOcultarFlujo() {
            vm.mostrarFlujo = !vm.mostrarFlujo;
            if (vm.mostrarFlujo) {
                $("#ver").html('Ocultar qué es esto');
            }
            else {
                $("#ver").html('Ver qué es esto');
            }
        }

        $scope.tab = 1;

        $scope.setTab = function (newTab) {
            $scope.tab = newTab;
        };

        $scope.isSet = function (tabNum) {
            return $scope.tab === tabNum;
        };

        vm.siguienteDisabled = false;



        vm.inicializarComponenteCheck = function (estado) {
            vm.siguienteDisabled = true;
            vm.handlerComponentesChecked = {
                'sgpsolicitudrecursosdatosgenerales': true,
                'sgprecursos': true,
                'sgpsolicitudrecursosfocalizacion': true,
                'sgpgraprobaciondefinitivausuarios': true,
                'sgpgraprobaciondefinitiva': true
            };
        }

        function eventoValidar() {
            vm.inicializarComponenteCheck();
            vm.validaCuestionario = false;
            gestionRecursosServicio.obtenerErroresProyecto(vm.guiMacroproceso, vm.ProyectoId, vm.idInstancia).then(function (respuesta) {
                if (respuesta.data) {
                    if (respuesta.data != null) {

                        let validaAprobacionDefinitiva = ['No se puede seguir al siguiente paso debido a que respondió NO en el formulario'];

                        validaAprobacionDefinitiva.some(objCodigo => {
                            respuesta.data.some((objeto) => {
                                if (objeto) {
                                    if (objeto.Errores) {
                                        if (objeto.Errores.includes(objCodigo)) {
                                            vm.validaCuestionario = true;
                                        }
                                    }
                                }
                            });
                        });

                        vm.notificacionValidacionHijos(respuesta.data);
                        var findErrors = respuesta.data.findIndex(p => p.Errores != null);
                        var indexobs = respuesta.data.findIndex(p => p.Capitulo == 'observaciones');
                        if (indexobs < 0) { var errorObservacion = false; }
                        else { var errorObservacion = respuesta.data[indexobs].Errores == null ? false : true; }
                        vm.visualizarAlerta((findErrors != -1), errorObservacion);
                        //vm.visualizarAlerta((findErrors != -1))
                    }
                }
            });
        }

        function eventoHabilitarEdicion() {
            $("#editarButton").hide();
            $("#validarButton").show();
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

        vm.visualizarAlerta = function (error, errorObservacion) {
            let ocultarDevolver = true;
            var validacion = {
                tieneError: error,
                tieneErrorObservacion: errorObservacion
            }
            if (error) {
                utilidades.mensajeError("Revise los campos señalados y valide nuevamente.", null, "Hay datos que presentan inconsistencias");
                if (vm.validaCuestionario) {
                    vm.siguienteDisabled = true;
                    ocultarDevolver = false;
                }
            }
            else 
            {
                utilidades.mensajeSuccess("¡Validación éxitosa!", false, false, false);
                var hijosCorrectos = (error == false);

                vm.siguienteDisabled = (hijosCorrectos == false);

                ocultarDevolver = true;
                /*ocultarDevolver = $sessionStorage.listadoAccionesTramite.some(x => x.AccionInstanciaId === null);*/
            }
            vm.callback({ validacion: validacion, arg: vm.siguienteDisabled, aprueba: false, titulo: '', ocultarDevolver });
        }

        /* ---------------------- Validaciones ---------------*/

        vm.notificacionValidacionEstado = function (estado, nombreComponente) {
            vm.showAlertError(nombreComponente, estado);
            vm.handlerComponentesChecked[nombreComponente] = estado;
        }

        vm.notificacionValidacion = function (handler, nombreComponente, handlerCapitulos) {
            var indx = vm.handlerComponentes.findIndex(p => p.componente == nombreComponente);
            vm.handlerComponentes[indx].handlerValidacion = handler;
            vm.handlerComponentes[indx].handlerCapitulos = handlerCapitulos;
        };

        vm.setCapitulosHijos = function (listadoCapitulos) {
            for (var i = 0; i < vm.handlerComponentes.length; i++) {
                if (vm.handlerComponentes[i].handlerCapitulos != null) vm.handlerComponentes[i].handlerCapitulos(listadoCapitulos);
            }
        };

        vm.notificacionValidacionHijos = function (errores) {
            for (var i = 0; i < vm.handlerComponentes.length; i++) {
                if (vm.handlerComponentes[i].handlerValidacion != null && errores != null) vm.handlerComponentes[i].handlerValidacion({ errores });
            }
        };


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

        $scope.$watchCollection("vm.handlerComponentesChecked", function (newValue, oldValue) {
            let ocultarDevolver = true;
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
            vm.validarSiguiente = estado;
            if (vm.validaCuestionario) {
                vm.siguienteDisabled = true;
                ocultarDevolver = false;
            }
            vm.callback({ arg: !estado, aprueba: false, titulo: '', ocultarDevolver});
            //$sessionStorage.edicionConpes = !estado;
        });


        vm.guardadohijos = function (nombreComponente, nombreComponenteHijo) {
            vm.siguienteDisabled = true
            vm.callback({ arg: vm.siguienteDisabled, aprueba: false, titulo: '', ocultarDevolver: true });
            vm.notificacionCambiosJustificacion(nombreComponente, nombreComponenteHijo);
        }

        vm.notificacionCambios = function (nombreComponente, nombreComponenteHijo) {
            for (var i = 0; i < vm.handlerComponentes.length; i++) {
                if (vm.handlerComponentes[i].handlerCambios != null) vm.handlerComponentes[i].handlerCambios({ nombreComponente: nombreComponente, nombreComponenteHijo: nombreComponenteHijo });
            }
        };








        /* ---------------------- Recursos ---------------*/

        vm.guardadohijos = function (nombreComponente, nombreComponenteHijo) {
            vm.siguienteDisabled = true
            vm.callback({ arg: vm.siguienteDisabled, aprueba: false, titulo: '', ocultarDevolver: true });
            vm.notificacionCambiosJustificacion(nombreComponente, nombreComponenteHijo);
        }

        vm.notificacionCambiosJustificacion = function (nombreComponente, nombreComponenteHijo) {
            var componente = vm.handlerComponentes.find(p => p.componente == 'recursosgr');
            if (componente != undefined) componente.handlerCambios({ nombreComponente: nombreComponente, nombreComponenteHijo: nombreComponenteHijo });
        };

        //vm.notificacionCambiosAjustes = function (handler) {
        //    var componente = vm.handlerComponentes.find(p => p.componente == 'recursosgr');
        //    if (componente != undefined) componente.handlerCambios = handler;
        //};

        /* ---------------------- Focalizacion ---------------*/
        vm.guardadohijos = function (nombreComponente, nombreComponenteHijo) {
            //vm.notificacionCambiosFocalizacion(nombreComponente, nombreComponenteHijo);
            vm.siguienteDisabled = true
            vm.callback({ arg: vm.siguienteDisabled, aprueba: false, titulo: '', ocultarDevolver: true });
            vm.notificacionCambios(nombreComponente, nombreComponenteHijo);
        }

        vm.notificacionCambios = function (nombreComponente, nombreComponenteHijo) {
            for (var i = 0; i < vm.handlerComponentes.length; i++) {
                if (vm.handlerComponentes[i].handlerCambios != null) vm.handlerComponentes[i].handlerCambios({ nombreComponente: nombreComponente, nombreComponenteHijo: nombreComponenteHijo });
            }
        };

        vm.notificacionCambiosgr = function (handler, nombreComponente) {
            for (var i = 0; i < vm.handlerComponentes.length; i++) {
                if (handler != null && vm.handlerComponentes[i].componente == nombreComponente) vm.handlerComponentes[i].handlerCambios = handler;
            }
        };




        //vm.notificacionCambiosFocalizacion = function (nombreComponente, nombreComponenteHijo) {
        //    var componente = vm.handlerComponentes.find(p => p.componente == nombreComponenteHijo);
        //    if (componente != undefined) componente.handlerCambios({ nombreComponente: nombreComponente, nombreComponenteHijo: nombreComponenteHijo });
        //};

        //vm.notificacionCambiosFocalizacion = function (handler) {
        //    var componente = vm.handlerComponentes.find(p => p.componente == 'focalizaciongr');
        //    if (componente != undefined) componente.handlerCambios = handler;
        //};

        vm.notificarGuardado = function () {
            vm.callback({ validacion: null, arg: true, aprueba: false, titulo: '', ocultarDevolver: true });
        }

    }

    angular.module('backbone').component('gestionRecursosSgp', {
        templateUrl: "/src/app/formulario/ventanas/SGP/gestionRecursosSGP/gestionRecursosSgp.html",
        controller: gestionRecursosSgpController,
        controllerAs: "vm",
        bindings: {
            callback: '&',
        }
    });

})();