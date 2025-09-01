(function () {
    'use strict';

    gestionRecursosController.$inject = [
        '$sessionStorage',
        'utilidades',
        'gestionRecursosServicio',
        'justificacionCambiosServicio',
        '$scope',
        'utilsValidacionSeccionCapitulosServicio',
    ];

    function gestionRecursosController(
        $sessionStorage,
        utilidades,
        gestionRecursosServicio,
        justificacionCambiosServicio,
        $scope,
        utilsValidacionSeccionCapitulosServicio,
    ) {
        var vm = this;
        vm.lang = "es";
        //vm.guiMacroproceso = '88EA329D-F240-4868-9DF7-86C74FB2ECFA';//justificacionCambiosServicio.getIdEtapa($sessionStorage.etapa);
        vm.guiMacroproceso = justificacionCambiosServicio.getIdEtapa($sessionStorage.etapa);
        vm.ProyectoId = $sessionStorage.proyectoId;
        vm.idInstancia = $sessionStorage.idInstancia;
        vm.IdNivel = $sessionStorage.idNivel;
        vm.BPIN = $sessionStorage.idObjetoNegocio;

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
            { id: 1, componente: 'datosgeneralesgr', handlerValidacion: null, handlerCambios: null, handlerCapitulos: [] },
            { id: 2, componente: 'recursosgr', handlerValidacion: null, handlerCambios: null, handlerCapitulos: [] },
            { id: 3, componente: 'focalizaciongr', handlerValidacion: null, handlerCambios: null, handlerCapitulos: [] },
            { id: 4, componente: 'aprobacionpegr', handlerValidacion: null, handlerCambios: null, handlerCapitulos: [] },
            { id: 5, componente: 'aprobaciongr', handlerValidacion: null, handlerCambios: null, handlerCapitulos: [] },
        ];

        vm.handlerComponentesChecked = {};


        //Inicio
        vm.init = function () {
            $sessionStorage.IdMacroproceso = vm.guiMacroproceso;
            vm.inicializarComponenteCheck(true);
            vm.ObtenerLocalizacionProyecto(vm.BPIN);

            vm.callback({ validacion: { evento: vm.eventoValidar }, arg: true, aprueba: false, titulo: '' });

            utilsValidacionSeccionCapitulosServicio.getSeccionCapitulo(vm.guiMacroproceso, $sessionStorage.idNivel, $sessionStorage.idFlujoIframe).then(function (respuesta) {
                vm.setCapitulosHijos(respuesta.data);
          
                const span = document.getElementById('d' + respuesta.data[0].SeccionModificado);
                if (span != undefined && span != null) {
                    span.classList.add("active");
                }
            });
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
                'datosgeneralesgr': true,
                'recursosgr': true,
                'focalizaciongr': true,
                'aprobacionpegr': true,
                'aprobaciongr': true,
            };
            
        }

        function eventoValidar() {
            vm.inicializarComponenteCheck();
            gestionRecursosServicio.obtenerErroresProyecto(vm.guiMacroproceso, vm.ProyectoId, vm.idInstancia).then(function (respuesta) {
                vm.notificacionValidacionHijos(respuesta.data);
                var findErrors = respuesta.data.findIndex(p => p.Errores != null);
                var indexobs = respuesta.data.findIndex(p => p.Capitulo == 'observaciones');
                if (indexobs < 0) { var errorObservacion = false; }
                else { var errorObservacion = respuesta.data[indexobs].Errores == null ? false : true; }
                vm.visualizarAlerta((findErrors != -1), errorObservacion);
                //vm.visualizarAlerta((findErrors != -1))
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
            if (error) utilidades.mensajeError("Revise los campos señalados y valide nuevamente.", null, "Hay datos que presentan inconsistencias");
            else utilidades.mensajeSuccess("¡Validación éxitosa!", false, false, false);

            var hijosCorrectos = (error == false);

            vm.siguienteDisabled = (hijosCorrectos == false);

            var validacion = {
                tieneError: error,
                tieneErrorObservacion: errorObservacion
            }
            vm.callback({ validacion: validacion, arg: vm.siguienteDisabled, aprueba: false, titulo: '' });

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
            vm.callback({ arg: !estado, aprueba: false, titulo: '' });
            //$sessionStorage.edicionConpes = !estado;
        });


        vm.guardadohijos = function (nombreComponente, nombreComponenteHijo) {
            vm.siguienteDisabled = true
            vm.callback({ arg: vm.siguienteDisabled, aprueba: false, titulo: '' });
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
            vm.callback({ arg: vm.siguienteDisabled, aprueba: false, titulo: '' });
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
            vm.callback();
        }

    }

    angular.module('backbone').component('gestionRecursos', {
        templateUrl: "src/app/formulario/ventanas/gestionRecursos/gestionRecursos.html",
        controller: gestionRecursosController,
        controllerAs: "vm",
        bindings: {
            callback: '&',
        }
    });

})();