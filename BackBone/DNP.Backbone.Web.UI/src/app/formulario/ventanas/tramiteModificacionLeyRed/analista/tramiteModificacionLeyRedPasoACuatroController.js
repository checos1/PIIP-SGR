(function () {
    'use strict';

    tramiteModificacionLeyRedPasoACuatroController.$inject = [
        '$sessionStorage',
        '$scope',
        'tramiteProgramacionRecursosServicio',
        'constantesBackbone',
        'utilidades',
        'utilsValidacionSeccionCapitulosServicio',
        'sesionServicios',
        'trasladosServicio',

    ];

    function tramiteModificacionLeyRedPasoACuatroController(
        $sessionStorage,
        $scope,
        tramiteProgramacionRecursosServicio,
        constantesBackbone,
        utilidades,
        utilsValidacionSeccionCapitulosServicio,
        sesionServicios,
        trasladosServicio

    ) {
        var vm = this;
        $sessionStorage.proyectoId = undefined;
        $sessionStorage.BPIN = undefined;
        vm.IdNivel = $sessionStorage.idNivel;

        //Esto se necesita para asignar los capitulos
        vm.guiMacroproceso = constantesBackbone.idEtapaProgramacionEjecucion;

        vm.instanciaId = '';
        vm.tramiteDetail = {
            tramiteId: null,
            tipoTramiteId: ''
        };
        //Métodos
        vm.mostrarOcultarFlujo = mostrarOcultarFlujo;
        vm.notificarGuardado = notificarGuardado;
        vm.eventoValidar = eventoValidar;

        $sessionStorage.sessionDocumentos = 100;
        vm.accionId = $sessionStorage.accionId;

        //Inicio
        vm.init = function () {
            vm.setSessionValues();
            vm.obtenerDetalleTramiteyRol();
            vm.inicializarComponenteCheck(true);
            utilsValidacionSeccionCapitulosServicio.getSeccionCapitulo(vm.guiMacroproceso, $sessionStorage.idNivel, $sessionStorage.idFlujoIframe).then(function (respuesta) {
                vm.setCapitulosHijos(respuesta.data);

                const span = document.getElementById('d' + respuesta.data[0].SeccionModificado);
                if (span != undefined && span != null) {
                    span.classList.add("active");
                }
            });
            var validacion = {
                evento: vm.eventoValidar
            }

            let ocultarDevolver = false;
            let oculatarSiguiente = true;
            if ((vm.IdNivel.toLowerCase() === constantesBackbone.idNivelAprobacionSupervisorProgramacion.toLowerCase() || vm.IdNivel.toLowerCase() === constantesBackbone.idNivelRevisionConcepto.toLowerCase())) {
                ocultarDevolver = true;
                oculatarSiguiente = true;
                $sessionStorage.GuardarAprobacionEntidadVFExec = true;
            }
            vm.callback({ validacion: validacion, arg: oculatarSiguiente, aprueba: false, titulo: '', ocultarDevolver: ocultarDevolver });
        };

        vm.validarFormulario = function () {
            eventoValidar();
        };

        vm.setSessionValues = function () {
            vm.instanciaId = $sessionStorage.idInstanciaIframe;
        };

        vm.obtenerDetalleTramiteyRol = function () {

            if (vm.IdNivel === constantesBackbone.idNivelSeleccionProyectos.toLowerCase()) {
                var roles = sesionServicios.obtenerUsuarioIdsRoles();
                var rol = roles.find(x => x === constantesBackbone.idRPresupuesto.toLowerCase());
                if (rol !== undefined)
                    vm.IdRol = rol;
            }

            var roles = sesionServicios.obtenerUsuarioIdsRoles();
            if ($sessionStorage.InstanciaSeleccionada.tramiteId !== undefined) {
                vm.tramiteId = $sessionStorage.InstanciaSeleccionada.tramiteId;
                vm.tipoTramiteId = $sessionStorage.InstanciaSeleccionada.tipoTramiteId;
                vm.tramiteDetail.tramiteId = vm.tramiteId;
                vm.tramiteDetail.tipoTramiteId = vm.tipoTramiteId;
            }
            else
                trasladosServicio.obtenerDetallesTramite($sessionStorage.numeroTramite).then(function (result) {
                    var x = result.data;
                    if (x != null) {
                        vm.tramiteId = x.TramiteId;
                        vm.tipoTramiteId = x.TipoTramiteId;
                        $sessionStorage.tramiteId = x.TramiteId;
                        vm.tramiteDetail.tramiteId = x.TramiteId;
                        vm.tramiteDetail.tipoTramiteId = x.TipoTramiteId;
                    }
                });
        }

        function mostrarOcultarFlujo() {
            vm.mostrarFlujo = !vm.mostrarFlujo;
        }

        $scope.tab = 1;

        $scope.setTab = function (newTab) {
            $scope.tab = newTab;
        };

        $scope.isSet = function (tabNum) {
            return $scope.tab === tabNum;
        };

        function notificarGuardado() {
            let vocultarDevolver = false
            if ($sessionStorage.GuardarAprobacionEntidadVFExec !== undefined)
                vocultarDevolver = $sessionStorage.GuardarAprobacionEntidadVFExec;

            vm.callback({ validacion: null, arg: true, aprueba: false, titulo: '', ocultarDevolver: vocultarDevolver });

        }

        /**
        * Listado de componentes hijos, obligatorio para estructura de validación
        * */
        vm.handlerComponentes = [
            { id: 1, componente: 'aprobacion', handlerValidacion: null, handlerCambios: null, esValido: true, errores: null },
        ];

        vm.handlerComponentesChecked = {};

        vm.inicializarComponenteCheck = function (estado) {
            vm.siguienteDisabled = true;
            vm.handlerComponentesChecked = {
                'aprobacion': true
            };
        }

        function eventoValidar() {
            vm.accionId = $sessionStorage.idAccion;
            var error = false;
            tramiteProgramacionRecursosServicio.obtenerErroresProgramacion(vm.instanciaId, vm.accionId).then(function (respuesta) {
                vm.notificacionValidacionHijos(respuesta.data);
                var indexobs = respuesta.data.findIndex(p => p.Capitulo == 'observaciones');
                var errorObservacion = respuesta.data[indexobs].Errores == null ? false : true;

                var findErrors = respuesta.data.findIndex(p => p.Errores != null);
                vm.visualizarAlerta((findErrors != -1), errorObservacion);

                error = (findErrors != -1);
            });
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
            let vocultarDevolver = false;


            let voculatarSiguiente = vm.siguienteDisabled;
            //Si Deja los botones habilitados dependiendo de las respuesta del paso 2 y 4
            if ($sessionStorage.Respuesta !== undefined
                && (vm.IdNivel.toLowerCase() === constantesBackbone.idNivelAprobacionSupervisorProgramacion.toLowerCase() || vm.IdNivel.toLowerCase() === constantesBackbone.idNivelRevisionConcepto.toLowerCase())) {
                vocultarDevolver = $sessionStorage.Respuesta;
                voculatarSiguiente = voculatarSiguiente === false ? !$sessionStorage.Respuesta : voculatarSiguiente;
            }
            else if ($sessionStorage.Respuesta === undefined
                && (vm.IdNivel.toLowerCase() === constantesBackbone.idNivelAprobacionSupervisorProgramacion.toLowerCase() || vm.IdNivel.toLowerCase() === constantesBackbone.idNivelRevisionConcepto.toLowerCase())) {
                vocultarDevolver = true;
                voculatarSiguiente = true;
            }
            //Si hay errores deja el Boton devolver deshabilitado
            if ((vm.IdNivel.toLowerCase() === constantesBackbone.idNivelAprobacionSupervisorProgramacion.toLowerCase() || vm.IdNivel.toLowerCase() === constantesBackbone.idNivelRevisionConcepto.toLowerCase()) && vm.siguienteDisabled) {
                vocultarDevolver = true;
            }
            vm.callback({ validacion: validacion, arg: voculatarSiguiente, aprueba: false, titulo: '', ocultarDevolver: vocultarDevolver });


        }

        vm.notificacionValidacionEstado = function (estado, nombreComponente) {
            vm.showAlertError(nombreComponente, estado);
            vm.handlerComponentesChecked[nombreComponente] = estado;
        }

        vm.notificacionValidacion = function (handler, nombreComponente, handlerCapitulos) {
            var indx = vm.handlerComponentes.findIndex(p => p.componente == nombreComponente);
            if (indx !== -1) {
                vm.handlerComponentes[indx].handlerValidacion = handler;
                vm.handlerComponentes[indx].handlerCapitulos = handlerCapitulos;
            }
        };



        vm.setCapitulosHijos = function (listadoCapitulos) {
            for (var i = 0; i < vm.handlerComponentes.length; i++) {
                if (vm.handlerComponentes[i].handlerCapitulos != null) vm.handlerComponentes[i].handlerCapitulos(listadoCapitulos);
            }
        };

        vm.notificacionValidacionHijos = function (errores) {

            for (var i = 0; i < vm.handlerComponentes.length; i++) {
                if (vm.handlerComponentes[i].handlerValidacion != null) vm.handlerComponentes[i].handlerValidacion({ errores });
            }
        };

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
            if ($sessionStorage.Respuesta !== undefined
                && (vm.IdNivel.toLowerCase() === constantesBackbone.idNivelAprobacionSupervisorProgramacion.toLowerCase() || vm.IdNivel.toLowerCase() === constantesBackbone.idNivelRevisionConcepto.toLowerCase())) {
                vocultarDevolver = $sessionStorage.Respuesta;
                voculatarSiguiente = voculatarSiguiente === false ? !$sessionStorage.Respuesta : voculatarSiguiente;
            }
            else if ($sessionStorage.Respuesta === undefined
                && (vm.IdNivel.toLowerCase() === constantesBackbone.idNivelAprobacionSupervisorProgramacion.toLowerCase() || vm.IdNivel.toLowerCase() === constantesBackbone.idNivelRevisionConcepto.toLowerCase())) {
                vocultarDevolver = true;
                voculatarSiguiente = true;
            }
            vm.callback({ validacion: validacion, arg: voculatarSiguiente, aprueba: false, titulo: '', ocultarDevolver: vocultarDevolver });
            //$sessionStorage.edicionConpes = !estado;
        });


        vm.guardadohijos = function (nombreComponente, nombreComponenteHijo) {
            vm.notificacionCambiosAsociarProyecto(nombreComponente, nombreComponenteHijo);
        }

        vm.notificacionCambiosAsociarProyecto = function (nombreComponente, nombreComponenteHijo) {
            var componente = vm.handlerComponentes.find(p => p.componente == 'asociarProyecto');
            if (componente != undefined) componente.handlerCambios({ nombreComponente: nombreComponente, nombreComponenteHijo: nombreComponenteHijo });
        };


        vm.notificacionCambios = function (nombreComponente, nombreComponenteHijo) {
            for (var i = 0; i < vm.handlerComponentes.length; i++) {
                if (vm.handlerComponentes[i].handlerCambios != null) vm.handlerComponentes[i].handlerCambios({ nombreComponente: nombreComponente, nombreComponenteHijo: nombreComponenteHijo });
            }
        };

        vm.notificacionCambiosAjustes = function (handler, nombreComponente) {
            for (var i = 0; i < vm.handlerComponentes.length; i++) {
                if (handler != null && vm.handlerComponentes[i].componente == nombreComponente) vm.handlerComponentes[i].handlerCambios = handler;
            }
        };

        /* ---------------------- asociarProyecto ---------------*/


        vm.notificacionCambiosAsociarProyecton = function (nombreComponente, nombreComponenteHijo) {
            var componente = vm.handlerComponentes.find(p => p.componente == 'asociarProyecto');
            if (componente != undefined) componente.handlerCambios({ nombreComponente: nombreComponente, nombreComponenteHijo: nombreComponenteHijo });
        };

        vm.notificacionCambiosAsociarProyecton = function (handler) {
            var componente = vm.handlerComponentes.find(p => p.componente == 'asociarProyecto');
            if (componente != undefined) componente.handlerCambios = handler;
        };
    }

    angular.module('backbone').component('tramiteModificacionLeyRedPasoACuatro', {
        templateUrl: "src/app/formulario/ventanas/tramiteModificacionLeyRed/analista/tramiteModificacionLeyRedPasoACuatro.html",
        controller: tramiteModificacionLeyRedPasoACuatroController,
        controllerAs: "vm",
        bindings: {
            callback: '&',
            registrarEvento: '&'
        }
    });

})();