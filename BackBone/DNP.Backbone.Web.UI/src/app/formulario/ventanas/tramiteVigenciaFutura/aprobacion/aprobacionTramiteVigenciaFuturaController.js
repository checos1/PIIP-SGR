
(function () {
    'use strict';

    aprobacionTramiteVigenciaFuturaController.$inject = [
        '$sessionStorage',
        '$scope',
        'tramiteVigenciaFuturaServicio',
        'constantesBackbone',
        'utilidades',
        '$timeout',
        'utilsValidacionSeccionCapitulosServicio',
    ];

    function aprobacionTramiteVigenciaFuturaController(
        $sessionStorage,
        $scope,
        tramiteVigenciaFuturaServicio,
        constantesBackbone,
        utilidades,
        $timeout,
        utilsValidacionSeccionCapitulosServicio,
    ) {
        var vm = this;
        $sessionStorage.proyectoId = undefined;
        $sessionStorage.BPIN = undefined;
        vm.IdNivel = $sessionStorage.idNivel;
        vm.BPIN = undefined;
        vm.instanciaId = '';
        vm.tramiteDetail = {
            tramiteId: null,
            tipoTramiteId: ''
        };
        //Métodos
        vm.mostrarOcultarFlujo = mostrarOcultarFlujo;
        vm.cambiarImagen = cambiarImagen;
        vm.eventoValidar = eventoValidar;
        vm.aprueba = false;
        $("#editarButton").hide();
        $("#validarButton").show();

        vm.eventoHabilitarEdicion = eventoHabilitarEdicion;
        $sessionStorage.sessionDocumentos = 0;
        vm.accionId = $sessionStorage.accionId;
        vm.devolverBoton = false;

        vm.notificarGuardado = notificarGuardado;

        //Inicio
        vm.init = function () {
            vm.model = {
                modulos: {
                    administracion: false,
                    backbone: true
                }
            }
            //vm.setDefaultValues();
            vm.setSessionValues();
            obtenerDetalleTramite();
            vm.inicializarComponenteCheck(true);
            utilsValidacionSeccionCapitulosServicio.getSeccionCapitulo(vm.guiMacroproceso, $sessionStorage.idNivel, $sessionStorage.idFlujoIframe).then(function (respuesta) {
                vm.setCapitulosHijos(respuesta.data);
            });
            var validacion = {
                evento: vm.eventoValidar
            }
            vm.callback({ validacion: validacion, arg: true });
        };

        vm.validarFormulario = function () {
            eventoValidar();
        };

        vm.setSessionValues = function () {
            vm.instanciaId = $sessionStorage.idInstanciaIframe;
        };

        vm.setDefaultValues = function () {
            cambiarImagen(0, "AsociarProyectoImg");
            cambiarImagen(0, "informacionPresupuestal");
            cambiarImagen(0, "justificacion");
            cambiarImagen(0, "documentoSoporte");
            cambiarImagen(0, "solicitarconcepto");
            cambiarImagenLapiz(0);
        };

        function obtenerDetalleTramite () {
            tramiteVigenciaFuturaServicio.obtenerDetalleTramitePorInstancia(vm.instanciaId)
                .then(function (response) {
                    if (response?.data?.Estado) {
                        vm.tramiteDetail = {
                            tramiteId: response.data.Data.TramiteId,
                            tipoTramiteId: response.data.Data.TipoTramiteId
                        };
                        
                        $sessionStorage.tramiteId = vm.tramiteDetail.tramiteId;
                        $sessionStorage.tipoTramiteId = vm.tramiteDetail.tipoTramiteId;
                    }
                    if ($sessionStorage.usuario.roles.find(x => x.Nombre.includes('Analista')) && !$sessionStorage.soloLectura)
                        vm.rolAnalista = true;

                }, function (error) {
                    /*utilidades.mensajeError('No fue posible consultar el listado de conpes');*/
                });
            vm.tramiteId = $sessionStorage.TramiteId;
            vm.guiMacroproceso = constantesBackbone.idEtapaTramitesEjecucion;
            $sessionStorage.IdMacroproceso = vm.guiMacroproceso;
            vm.idInstancia = $sessionStorage.idInstancia;

        };


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

        function cambiarImagen(Porcentaje, opcion) {
            switch (Porcentaje) {
                case 0:
                    document.getElementById(opcion).src = "Img/etapas/luna_1.svg";
                    break;
                case 25:
                    document.getElementById(opcion).src = "Img/etapas/luna_2.svg";
                    break;
                case 50:
                    document.getElementById(opcion).src = "Img/etapas/luna_3.svg";
                    break;
                case 100:
                    document.getElementById(opcion).src = "Img/etapas/luna_4.svg";
                    break;
            }

        }

        function cambiarImagenLapiz(TipoPermiso) {
            switch (TipoPermiso) {
                case 0: // no ha hecho nada
                    document.getElementById("IconoLapiz").src = "Img/barra/permiso1_barra_pest.svg";
                    break;
                case 25: //Ya termino y se encuentra en otra etapa
                    document.getElementById("IconoLapiz").src = "Img/barra/permiso2_barra_pest.svg";
                    break;
                case 50:// Ha diligenciado por lo menos una opción
                    document.getElementById("IconoLapiz").src = "Img/barra/permiso3_barra_pest.svg";
                    break;
                case 100: //No tiene permisos
                    document.getElementById("IconoLapiz").src = "Img/barra/permiso4_barra_pest.svg";
                    break;
            }

        }


        /**
        * Listado de componentes hijos, obligatorio para estructura de validación
        * */
        vm.handlerComponentes = [
            { id: 1, componente: 'asociarproyecto', handlerValidacion: null, handlerCambios: null, esValido: true, errores: null },
            { id: 2, componente: 'informacionpresupuestal', handlerValidacion: null, handlerCambios: null, esValido: true, errores: null },
            { id: 3, componente: 'justificacion', handlerValidacion: null, handlerCambios: null, esValido: true, errores: null },
            { id: 4, componente: 'documentossoporte', handlerValidacion: null, handlerCambios: null, esValido: true, errores: null },
            { id: 5, componente: 'solicitarconcepto', handlerValidacion: null, handlerCambios: null, esValido: true, errores: null }
        ];

        vm.handlerComponentesChecked = {};

        vm.inicializarComponenteCheck = function (estado) {
            vm.siguienteDisabled = true;
            vm.handlerComponentesChecked = {
                'asociarproyecto': true,
                'informacionpresupuestal': true,
                'justificacion': true,
                'documentossoporte': true,
                'solicitarconcepto':true,
            };
        }



        //function eventoValidar() {
        //    // vm.validarSiguiente = false;
        //    vm.accionId = $sessionStorage.idAccion;
        //    vm.inicializarComponenteCheck();
        //    var error = false;
        //    tramiteVigenciaFuturaServicio.obtenerErroresTramite(vm.guiMacroproceso, vm.idInstancia, vm.accionId).then(function (respuesta) {
        //        vm.notificacionValidacionHijos(respuesta.data);
        //        var findErrors = respuesta.data.findIndex(p => p.Errores != null);
        //        vm.visualizarAlerta((findErrors != -1));
        //        error = (findErrors != -1);
        //    });

        //}

        function eventoValidar() {
            // vm.validarSiguiente = false;
            vm.accionId = $sessionStorage.idAccion;
            //vm.inicializarComponenteCheck();
            var error = false;
            tramiteVigenciaFuturaServicio.obtenerErroresTramite(vm.guiMacroproceso, vm.idInstancia, vm.accionId).then(function (respuesta) {
                vm.notificacionValidacionHijos(respuesta.data);
                var indexobs = respuesta.data.findIndex(p => p.Capitulo == 'observaciones');
                var errorObservacion = respuesta.data[indexobs].Errores == null ? false : true;

                var indexConecpto = respuesta.data.findIndex(p => p.Capitulo == 'elaborarconcepto');
                var errorConcepto = respuesta.data[indexConecpto].Errores == null ? false : true;

                var findErrors = respuesta.data.findIndex(p => p.Errores != null);
                vm.visualizarAlerta((findErrors != -1), errorObservacion, errorConcepto);
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

        //vm.visualizarAlerta = function (error) {
        //    if (error) utilidades.mensajeError("Revise los campos señalados y valide nuevamente.", null, "Hay datos que presentan inconsistencias");
        //    else utilidades.mensajeSuccess("¡Validación éxitosa!", false, false, false);

        //    var hijosCorrectos = (error == false);

        //    vm.siguienteDisabled = (hijosCorrectos == false);
        //    var validacion = {
        //        tieneError: error
        //    }
        //    vm.callback({ validacion: validacion, arg: vm.siguienteDisabled, aprueba: false, titulo: '' });
        //}


        vm.visualizarAlerta = function (error, errorObservacion, errorConcepto) {
            if (error) utilidades.mensajeError("Revise los campos señalados y valide nuevamente.", null, "Hay datos que presentan inconsistencias");
            else utilidades.mensajeSuccess("¡Validación éxitosa!", false, false, false);

            var hijosCorrectos = (error == false);

            vm.siguienteDisabled = (hijosCorrectos == false);
            var validacion = {
                tieneError: error,
                tieneErrorObservacion: errorObservacion
            }
            let vocultarDevolver = false;
            if (($sessionStorage.EstadoDNpAplicado !== undefined && !$sessionStorage.EstadoDNpAplicado) || (errorConcepto)) {
                vocultarDevolver = true;
            }
            vm.callback({ validacion: validacion, arg: vm.siguienteDisabled, aprueba: false, titulo: '', ocultarDevolver: vocultarDevolver });
            
   
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
           // vm.callback({ arg: !estado, aprueba: false, titulo: '' });
            //$sessionStorage.edicionConpes = !estado;
        });

        /* ---------------------- Validaciones ---------------*/

        /**
         * Función que crea las referencias de los métodos de los hijos con el padre 
         * @param {any} handler función referenciada
         * @param {any} nombreComponente nombre configurado en cada uno de los componentes hijos vm.nombreComponente = 'datosgenerales'
         */
       

        vm.notificacionValidacion = function (handler, nombreComponente, handlerCapitulos) {
            var indx = vm.handlerComponentes.findIndex(p => p.componente == nombreComponente);
            vm.handlerComponentes[indx].handlerValidacion = handler;
            vm.handlerComponentes[indx].handlerCapitulos = handlerCapitulos;
        };



        /**
         * Función que recibe los estados de los componentes hijos
         * @param {any} estado true: valido, false: invalido
         * @param {any} nombreComponente nombre configurado en cada uno de los componentes hijos vm.nombreComponente = 'datosgenerales'
         */
        vm.notificacionValidacionEstado = function (estado, nombreComponente) {
            vm.showAlertError(nombreComponente, estado);
            vm.handlerComponentesChecked[nombreComponente] = estado;
        }

        /**
         * Función que envía listado de errores a componentes hijos 
         * por referencia configurada en vm.notificacionValidacion.
         * @param {any} errores
         */
        vm.notificacionValidacionHijos = function (errores) {
            //se llena el error cuando la variable de docuemnto no esta al 100%
            let error = {};
           
                error = {
                    Seccion: "documentossoporte",
                    Capitulo: "alojararchivos",
                    Errores: null,
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

        vm.setCapitulosHijos = function (listadoCapitulos) {
            for (var i = 0; i < vm.handlerComponentes.length; i++) {
                if (vm.handlerComponentes[i].handlerCapitulos != null) vm.handlerComponentes[i].handlerCapitulos(listadoCapitulos);
            }
        };

        function notificarGuardado(botonDevolver, botonSiguiente, ocultarDevolver) {
            if (botonDevolver === undefined && botonSiguiente === undefined )
                vm.callback({ validacion: null, arg: true, aprueba: false, titulo: '' });
            else
                vm.callback({ validacion: null, arg: botonDevolver, aprueba: botonSiguiente, titulo: '', ocultarDevolver: ocultarDevolver });

        }

        /* ---------------------- asociarProyecto ---------------*/

        vm.guardadohijos = function (nombreComponente, nombreComponenteHijo) {
            vm.notificacionCambiosAsociarProyecto(nombreComponente, nombreComponenteHijo);
        }

        vm.notificacionCambiosAsociarProyecto = function (nombreComponente, nombreComponenteHijo) {
            var componente = vm.handlerComponentes.find(p => p.componente == 'asociarProyecto');
            if (componente != undefined) componente.handlerCambios({ nombreComponente: nombreComponente, nombreComponenteHijo: nombreComponenteHijo });
        };

        vm.notificacionCambiosAsociarProyecton = function (handler) {
            var componente = vm.handlerComponentes.find(p => p.componente == 'asociarProyecto');
            if (componente != undefined) componente.handlerCambios = handler;
        };

        /* ---------------------- Informacion Presupuestal ---------------*/
        vm.guardadohijos = function (nombreComponente, nombreComponenteHijo) {
            vm.notificacionCambiosInformacionpresupuestal(nombreComponente, nombreComponenteHijo);
        }

        vm.notificacionCambiosInformacionpresupuestal = function (nombreComponente, nombreComponenteHijo) {
            var componente = vm.handlerComponentes.find(p => p.componente == 'informacionpresupuestal');
            if (componente != undefined) componente.handlerCambios({ nombreComponente: nombreComponente, nombreComponenteHijo: nombreComponenteHijo });
        };

        vm.notificacionCambiosInformacionpresupuestal = function (handler) {
            var componente = vm.handlerComponentes.find(p => p.componente == 'informacionpresupuestal');
            if (componente != undefined) componente.handlerCambios = handler;
        };

        /* ---------------------- justificacionvigenciafutura ---------------*/

        vm.guardadohijos = function (nombreComponente, nombreComponenteHijo) {
            vm.notificacionCambiosAsociarProyecto(nombreComponente, nombreComponenteHijo);
        }

        vm.notificacionCambiosJustificacionvigenciafutura = function (nombreComponente, nombreComponenteHijo) {
            var componente = vm.handlerComponentes.find(p => p.componente == 'justificacionvigenciafutura');
            if (componente != undefined) componente.handlerCambios({ nombreComponente: nombreComponente, nombreComponenteHijo: nombreComponenteHijo });
        };

        vm.notificacionCambiosJustificacionvigenciafutura = function (handler) {
            var componente = vm.handlerComponentes.find(p => p.componente == 'justificacionvigenciafutura');
            if (componente != undefined) componente.handlerCambios = handler;
        };

        /* ---------------------- documentosoportevf ---------------*/

        vm.guardadohijos = function (nombreComponente, nombreComponenteHijo) {
            vm.notificacionCambiosAsociarProyecto(nombreComponente, nombreComponenteHijo);
        }

        vm.notificacionCambiosDocumentosoportevf = function (nombreComponente, nombreComponenteHijo) {
            var componente = vm.handlerComponentes.find(p => p.componente == 'documentosoportevf');
            if (componente != undefined) componente.handlerCambios({ nombreComponente: nombreComponente, nombreComponenteHijo: nombreComponenteHijo });
        };

        vm.notificacionCambiosDocumentosoportevf = function (handler) {
            var componente = vm.handlerComponentes.find(p => p.componente == 'documentosoportevf');
            if (componente != undefined) componente.handlerCambios = handler;
        };
       
        vm.deshabilitarBotonDevolverT = function () {
            vm.callback({ validacion: null, arg: true, aprueba: false, titulo: '', ocultarDevolver: true });
           

        }



    }

    angular.module('backbone').component('aprobacionTramiteVigenciaFutura', {
        templateUrl: "src/app/formulario/ventanas/tramiteVigenciaFutura/aprobacion/aprobacionTramiteVigenciaFutura.html",
        controller: aprobacionTramiteVigenciaFuturaController,
        controllerAs: "vm",
        bindings: {
            callback: '&',
            registrarEvento: '&'
        }
    });

})();