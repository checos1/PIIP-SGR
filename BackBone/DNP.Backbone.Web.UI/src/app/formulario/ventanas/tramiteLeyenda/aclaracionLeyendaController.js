(function () {
    'use strict';

    aclaracionLeyendaController.$inject = [
        '$sessionStorage',
        '$scope',
        'aclaracionLeyendaServicio',
        'constantesBackbone',
        'utilidades',
        'utilsValidacionSeccionCapitulosServicio',
        'trasladosServicio',
        'tramiteVigenciaFuturaServicio',
        'sesionServicios'
    ];

    function aclaracionLeyendaController(
        $sessionStorage,
        $scope,
        aclaracionLeyendaServicio,
        constantesBackbone,
        utilidades,
        utilsValidacionSeccionCapitulosServicio,
        trasladosServicio,
        tramiteVigenciaFuturaServicio,
        sesionServicios
    ) {
        var vm = this;
        $sessionStorage.proyectoId = undefined;
        $sessionStorage.BPIN = undefined;
        vm.IdNivel = $sessionStorage.idNivel;
        vm.section = "requerimientosTramite";
        vm.BPIN = undefined;
        //Esto se necesita para asignar los capitulos
        vm.guiMacroproceso = constantesBackbone.idEtapaTramitesEjecucion;
        //vm.notificacionCambiosCapitulos = null;
        //vm.BPIN = undefined;
        //vm.instanciaId = '';
        vm.tramiteDetail = {
            tramiteId: null,
            tipoTramiteId: ''
        };
        vm.tieneErrorObservacion = false;

        vm.eventoValidar = eventoValidar;


        $sessionStorage.sessionDocumentos = 0;
        vm.accionId = $sessionStorage.accionId;

        //Inicio
        vm.init = function () {
            vm.setSessionValues();
            vm.inicializarComponenteCheck(true);
            if (vm.IdNivel === constantesBackbone.idNivelSeleccionProyectos.toLowerCase()) {
                var roles = sesionServicios.obtenerUsuarioIdsRoles();
                var rol = roles.find(x => x === constantesBackbone.idRPresupuesto.toLowerCase());
                if (rol !== undefined)
                    vm.IdRol = rol;
                if ($sessionStorage.usuario.roles.find(x => x.Nombre.includes('Analista')) && !$sessionStorage.soloLectura)
                    vm.rolAnalista = true;
            }

            var roles = sesionServicios.obtenerUsuarioIdsRoles();
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

        vm.prueba = prueba;
        function prueba(botonDevolver, botonSiguiente, ocultarDevolver) {
            if (botonDevolver != undefined && botonSiguiente != undefined)
                vm.callback({ validacion: null, arg: botonDevolver, aprueba: botonSiguiente, titulo: '', ocultarDevolver: ocultarDevolver });
        }
        vm.notificarGuardado = notificarGuardado;
        function notificarGuardado(botonDevolver, botonSiguiente, ocultarDevolver) {
            vm.callback({ validacion: null, arg: botonDevolver, aprueba: botonSiguiente, titulo: '', ocultarDevolver: ocultarDevolver});

        }


        vm.setSessionValues = function () {
            vm.instanciaId = $sessionStorage.idInstanciaIframe;
        };

        /**
       * Listado de componentes hijos, obligatorio para estructura de validación
       * */
        vm.handlerComponentes = [
            { id: 1, componente: 'asociarproyecto', handlerValidacion: null, handlerCambios: null, esValido: true, errores: null },
            { id: 2, componente: 'informacionpresupuestal', handlerValidacion: null, handlerCambios: null, esValido: true, errores: null },
            { id: 3, componente: 'justificacion', handlerValidacion: null, handlerCambios: null, handlerCapitulos: [] },
            { id: 4, componente: 'soporte', handlerValidacion: null, handlerCambios: null, esValido: true, errores: null },
            { id: 5, componente: 'conceptos', handlerValidacion: null, handlerCambios: null, esValido: true, errores: null },
            { id: 6, componente: 'aprobacionpaso4', handlerValidacion: null, handlerCambios: null, esValido: true, errores: null },
            { id: 7, componente: 'aprobacionfinal', handlerValidacion: null, handlerCambios: null, esValido: true, errores: null },
            { id: 8, componente: 'aprobacion', handlerValidacion: null, handlerCambios: null, esValido: true, errores: null }
            


        ];

        vm.handlerComponentesChecked = {};

        vm.inicializarComponenteCheck = function (estado) {
            vm.siguienteDisabled = true;
            vm.handlerComponentesChecked = {
                'asociarproyecto': true,
                'informacionpresupuestal': true,
                'justificacion': true,
                'soporte': true,
                'conceptos': true,
                'aprobacionpaso4': true,
                'aprobacionfinal': true,
                'aprobacion': true
            };
        }

        function eventoValidar() {
            vm.accionId = $sessionStorage.idAccion;
            vm.idInstancia = $sessionStorage.idInstancia;
            var error = false;
            tramiteVigenciaFuturaServicio.obtenerErroresTramite(vm.guiMacroproceso, vm.idInstancia, vm.accionId).then(function (respuesta) {
                vm.notificacionValidacionHijos(respuesta.data);
                var indexobs = respuesta.data.findIndex(p => p.Capitulo == 'observaciones');
                var errorObservacion = respuesta.data[indexobs].Errores == null ? false : true;

                var paso;
                var validacionPaso2 = false;
                var indexPaso2 = respuesta.data.findIndex(p => p.Capitulo == 'confirmacionapr' && p.Seccion == 'aprobacion');
                if (indexPaso2 >= 0) {
                    paso = 2;
                    var errorPaso2 = JSON.parse(respuesta.data[indexPaso2].Errores);
                    if (errorPaso2 != null)
                        if (errorPaso2.aprobacionconfirmacionapr.length > 0)
                            if (errorPaso2.aprobacionconfirmacionapr[0].Error == 'TALP2003') {
                                validacionPaso2 = true;
                            }
                }

                var validacionPaso4 = false;
                var indexPaso4 = respuesta.data.findIndex(p => p.Capitulo == 'confirmacionapr' && p.Seccion == 'aprobacionpaso4');
                if (indexPaso4 >= 0) {
                    paso = 4;
                    var errorPaso4 = JSON.parse(respuesta.data[indexPaso4].Errores);
                    if (errorPaso4 != null)
                        if (errorPaso4.aprobacionpaso4confirmacionapr.length > 0)
                            if (errorPaso4.aprobacionpaso4confirmacionapr[0].Error == 'TALP4003') {
                                validacionPaso4 = true;
                            }
                }

                var findErrors = respuesta.data.findIndex(p => p.Errores != null);
                vm.visualizarAlerta((findErrors != -1), errorObservacion, validacionPaso4, validacionPaso2, paso);
                error = (findErrors != -1);
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

        vm.visualizarAlerta = function (error, errorObservacion, validacionPaso4, validacionPaso2, paso) {

            switch (paso) {
                case 4: if (!validacionPaso4)
                    if (error) utilidades.mensajeError("Revise los campos señalados y valide nuevamente.", null, "Hay datos que presentan inconsistencias");
                    else utilidades.mensajeSuccess("¡Validación éxitosa!", false, false, false);
                    break;
                case 2: if (!validacionPaso2)
                    if (error) utilidades.mensajeError("Revise los campos señalados y valide nuevamente.", null, "Hay datos que presentan inconsistencias");
                    else utilidades.mensajeSuccess("¡Validación éxitosa!", false, false, false);
                    break;

            }
            //if (!validacionPaso4)
            //    if (error) utilidades.mensajeError("Revise los campos señalados y valide nuevamente.", null, "Hay datos que presentan inconsistencias");
            //    else utilidades.mensajeSuccess("¡Validación éxitosa!", false, false, false);

            var hijosCorrectos = (error == false);

            vm.siguienteDisabled = (hijosCorrectos == false);
            var validacion = {
                tieneError: error,
                tieneErrorObservacion: errorObservacion,
                validacionPaso4: false,
                validacionPaso2: false
            }
            vm.tieneErrorObservacion = errorObservacion;

            if (validacionPaso4) {
                vm.siguienteDisabled = true;
                vm.tieneErrorObservacion = true;
                var validacion = {
                    tieneError: true,
                    tieneErrorObservacion: false,
                    validacionPaso4: true,
                    validacionPaso2: false
                }
            }

            if (validacionPaso2) {
                vm.siguienteDisabled = true;
                vm.tieneErrorObservacion = true;
                var validacion = {
                    tieneError: true,
                    tieneErrorObservacion: false,
                    validacionPaso4: false,
                    validacionPaso2: true
                }
            }

            vm.callback({ validacion: validacion, arg: vm.siguienteDisabled, aprueba: false, titulo: '' });
        }

        vm.notificacionValidacionEstado = function (estado, nombreComponente) {
            vm.showAlertError(nombreComponente, estado);
            vm.handlerComponentesChecked[nombreComponente] = estado;
        }

        vm.notificacionValidacion = function (handler, nombreComponente, handlerCapitulos) {
            var indx = vm.handlerComponentes.findIndex(p => p.componente == nombreComponente);
            if (indx != "-1") {
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
            //se llena el error cuando la variable de docuemnto no esta al 100%
            let error = {};
            if (($sessionStorage.sessionDocumentos < 100 || $sessionStorage.sessionDocumentos === undefined) && $sessionStorage.idNivel.toUpperCase() === constantesBackbone.idNivelSeleccionProyectos) {
                error = {
                    Seccion: "soporte",
                    Capitulo: "documentopaso",
                    Errores: '{"soportedocumentopaso":[{"Error":"VFO006","Descripcion":"Diligencie los documentos obligatorios"}]}',
                }
            }
            else {
                error = {
                    Seccion: "soporte",
                    Capitulo: "documentopaso",
                    Errores: null,
                }
            }
            errores.push(error);

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

            if (!vm.tieneErrorObservacion)
                vm.callback({ arg: !estado, aprueba: false, titulo: '' });

            
        });

        vm.guardadohijos = function (nombreComponente, nombreComponenteHijo) {
            vm.notificacionCambiosAsociarProyecto(nombreComponente, nombreComponenteHijo);
        }

        vm.notificacionCambiosAsociarProyecto = function (nombreComponente, nombreComponenteHijo) {
            var componente = vm.handlerComponentes.find(p => p.componente == 'asociarproyecto');
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


        vm.notificacionCambiosAsociarProyecton = function (nombreComponente, nombreComponenteHijo) {
            var componente = vm.handlerComponentes.find(p => p.componente == 'asociarProyecto');
            if (componente != undefined) componente.handlerCambios({ nombreComponente: nombreComponente, nombreComponenteHijo: nombreComponenteHijo });
        };

        vm.notificacionCambiosAsociarProyecton = function (handler) {
            var componente = vm.handlerComponentes.find(p => p.componente == 'asociarroyecto');
            if (componente != undefined) componente.handlerCambios = handler;
        };

        vm.deshabilitarBotonDevolverT = function () {
            vm.callback({ validacion: null, arg: true, aprueba: false, titulo: '', ocultarDevolver: true });

        }

    }

    angular.module('backbone').component('aclaracionLeyenda', {
        templateUrl: "src/app/formulario/ventanas/tramiteLeyenda/aclaracionLeyenda.html",
        controller: aclaracionLeyendaController,
        controllerAs: "vm",
        bindings: {
            callback: '&',
            registrarEvento: '&'
        }
    });

})();
