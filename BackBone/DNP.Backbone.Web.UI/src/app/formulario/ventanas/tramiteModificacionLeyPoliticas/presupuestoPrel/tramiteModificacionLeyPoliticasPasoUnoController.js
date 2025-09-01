(function () {
    'use strict';

    tramiteModificacionLeyPoliticasPasoUnoController.$inject = [
        '$sessionStorage',
        '$scope',
        'tramiteVigenciaFuturaServicio',
        'constantesBackbone',
        'utilidades',
        'utilsValidacionSeccionCapitulosServicio',
        'sesionServicios',
        'trasladosServicio',
        'tramiteModificacionLeyPoliticasServicio',
        'tramiteProgramacionRecursosServicio'

    ];

    function tramiteModificacionLeyPoliticasPasoUnoController(
        $sessionStorage,
        $scope,
        tramiteVigenciaFuturaServicio,
        constantesBackbone,
        utilidades,
        utilsValidacionSeccionCapitulosServicio,
        sesionServicios,
        trasladosServicio,
        tramiteModificacionLeyPoliticasServicio,
        tramiteProgramacionRecursosServicio

    ) {
        var vm = this;
        $sessionStorage.proyectoId = undefined;
        $sessionStorage.BPIN = undefined;
        vm.IdNivel = $sessionStorage.idNivel;
        vm.modificoDatos = '0';

        //Esto se necesita para asignar los capitulos
        vm.guiMacroproceso = constantesBackbone.idEtapaProgramacionEjecucion;

        vm.instanciaId = '';
        vm.tramiteDetail = {
            tramiteId: null,
            tipoTramiteId: ''
        };
        //Métodos
        vm.notificarGuardado = notificarGuardado;
        vm.eventoValidar = eventoValidar;

        //vm.eventoHabilitarEdicion = eventoHabilitarEdicion;
        $sessionStorage.sessionDocumentos = 0;
        vm.accionId = $sessionStorage.accionId;
        $sessionStorage.TieneCDP = false;

        //Inicio
        vm.initModificacionLey = function () {
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

            vm.callback({ validacion: validacion, arg: true });

        };

        /**
        * Listado de componentes hijos, obligatorio para estructura de validación
        * */
        vm.handlerComponentes = [
            { id: 1, componente: 'proyecto', handlerValidacion: null, handlerCambios: null, esValido: true, errores: null },
            { id: 2, componente: 'focalizacion', handlerValidacion: null, handlerCambios: null, esValido: true, errores: null },
            //{ id: 3, componente: 'regionalizacion', handlerValidacion: null, handlerCambios: null, handlerCapitulos: [] },
            //{ id: 3, componente: 'iniciativas', handlerValidacion: null, handlerCambios: null, esValido: true, errores: null },
            //{ id: 5, componente: 'politicastransversales', handlerValidacion: null, handlerCambios: null, esValido: true, errores: null },
            //{ id: 4, componente: 'productos', handlerValidacion: null, handlerCambios: null, esValido: true, errores: null },
            //{ id: 7, componente: 'aprobacionpaso4', handlerValidacion: null, handlerCambios: null, esValido: true, errores: null },
            //{ id: 8, componente: 'aprobacionfinal', handlerValidacion: null, handlerCambios: null, esValido: true, errores: null },


        ];

        vm.handlerComponentesChecked = {};

        vm.inicializarComponenteCheck = function (estado) {
            vm.siguienteDisabled = true;
            vm.handlerComponentesChecked = {
                'proyecto': true,
                'focalizacion': true
            };
        }


        function eventoValidar() {
            // vm.validarSiguiente = false;
            vm.accionId = $sessionStorage.idAccion;
            //vm.inicializarComponenteCheck();
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

        //function eventoHabilitarEdicion() {
        //    $("#editarButton").hide();
        //    $("#validarButton").show();
        //}

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
            vm.rolAnalista = roles.find(x => x === constantesBackbone.idRAnalistaDIFP.toLowerCase()) === constantesBackbone.idRAnalistaDIFP.toLowerCase() ? true : false;
            vm.muestracapitulospaso3 = !$sessionStorage.soloLectura ? true : false;
            trasladosServicio.obtenerDetallesTramite($sessionStorage.numeroTramite).then(function (result) {
                var x = result.data;
                if (x != null) {
                    vm.tramiteId = x.TramiteId;
                    vm.tipoTramiteId = x.TipoTramiteId;
                    $sessionStorage.tramiteId = x.TramiteId;
                    vm.tramiteDetail.tramiteId = x.TramiteId;
                    vm.tramiteDetail.tipoTramiteId = x.TipoTramiteId;
                    $sessionStorage.tipoTramiteId;

                }
            });
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
                && (vm.IdNivel.toLowerCase() === constantesBackbone.idNivelAprobacionEntidad.toLowerCase() || vm.IdNivel.toLowerCase() === constantesBackbone.idNivelRevisionConcepto.toLowerCase())) {
                vocultarDevolver = $sessionStorage.Respuesta;
                voculatarSiguiente = voculatarSiguiente === false ? !$sessionStorage.Respuesta : voculatarSiguiente;
            }
            else if ($sessionStorage.Respuesta === undefined
                && (vm.IdNivel.toLowerCase() === constantesBackbone.idNivelAprobacionEntidad.toLowerCase() || vm.IdNivel.toLowerCase() === constantesBackbone.idNivelRevisionConcepto.toLowerCase())) {
                vocultarDevolver = true;
                voculatarSiguiente = true;
            }
            //Si hay errores deja el Boton devolver deshabilitado
            if ((vm.IdNivel.toLowerCase() === constantesBackbone.idNivelAprobacionEntidad.toLowerCase() || vm.IdNivel.toLowerCase() === constantesBackbone.idNivelRevisionConcepto.toLowerCase()) && vm.siguienteDisabled) {
                vocultarDevolver = true;
            }
            if (vm.deshabilitar === true) {
                voculatarSiguiente = true;
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
            vm.handlerComponentes[indx].handlerValidacion = handler;
            vm.handlerComponentes[indx].handlerCapitulos = handlerCapitulos;
        };

        vm.guardadohijos = function (nombreComponente, nombreComponenteHijo) {
            vm.notificacionCambiosAsociarProyecto(nombreComponente, nombreComponenteHijo);
        }


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
            if (vm.deshabilitar === true) {
                vm.callback({ validacion: validacion, arg: true, aprueba: false, titulo: '', ocultarDevolver: true });
            }
            else {
                if ($sessionStorage.Respuesta !== undefined
                    && (vm.IdNivel.toLowerCase() === constantesBackbone.idNivelAprobacionEntidad.toLowerCase() || vm.IdNivel.toLowerCase() === constantesBackbone.idNivelRevisionConcepto.toLowerCase())) {
                    vocultarDevolver = $sessionStorage.Respuesta;
                    voculatarSiguiente = voculatarSiguiente === false ? !$sessionStorage.Respuesta : voculatarSiguiente;
                }
                else if ($sessionStorage.Respuesta === undefined
                    && (vm.IdNivel.toLowerCase() === constantesBackbone.idNivelAprobacionEntidad.toLowerCase() || vm.IdNivel.toLowerCase() === constantesBackbone.idNivelRevisionConcepto.toLowerCase())) {
                    vocultarDevolver = true;
                    voculatarSiguiente = true;
                }

                vm.callback({ validacion: validacion, arg: voculatarSiguiente, aprueba: false, titulo: '', ocultarDevolver: vocultarDevolver });
            }
        });



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


        function notificarGuardado(botonDevolver, botonSiguiente, ocultarDevolver) {

            let vocultarDevolver = ocultarDevolver;
            let vocultarSiguiente = false;

            if ($sessionStorage.GuardarAprobacionEntidadVFExec !== undefined) {
                vocultarDevolver = $sessionStorage.GuardarAprobacionEntidadVFExec;
                vocultarSiguiente = $sessionStorage.GuardarAprobacionEntidadVFExec;
            }
            if (vm.deshabilitar === true) {
                vocultarDevolver = true;
            }
            vm.callback({ validacion: null, arg: vocultarSiguiente, aprueba: false, titulo: '', ocultarDevolver: vocultarDevolver });
        }

        function eventoValidar() {
            // vm.validarSiguiente = false;
            vm.accionId = $sessionStorage.idAccion;
            //vm.inicializarComponenteCheck();
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

    }

    angular.module('backbone').component('tramiteModificacionLeyPoliticasPasoUno', {
        templateUrl: "src/app/formulario/ventanas/tramiteModificacionLeyPoliticas/presupuestoPrel/tramiteModificacionLeyPoliticasPasoUno.html",
        controller: tramiteModificacionLeyPoliticasPasoUnoController,
        controllerAs: "vm",
        bindings: {
            callback: '&',
            registrarEvento: '&'
        }
    });


})();
