(function () {
    'use strict';

    tramiteTrasladoOrdinarioController.$inject = [
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

    function tramiteTrasladoOrdinarioController(
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
        vm.section = "asociarproyecto";
        vm.BPIN = undefined;
        vm.actualizacomponentes = '';
        //Esto se necesita para asignar los capitulos
        vm.guiMacroproceso = constantesBackbone.idEtapaTramitesEjecucion;
        $sessionStorage.IdMacroproceso = vm.guiMacroproceso;
        //vm.notificacionCambiosCapitulos = null;
        //vm.BPIN = undefined;
        vm.idInstancia = $sessionStorage.idInstancia;
        vm.tramiteDetail = {
            tramiteId: null,
            tipoTramiteId: ''
        };
        vm.tieneErrorObservacion = false;
        vm.rolAnalista = false;
        vm.muestracapitulospaso3 = false;
        vm.btnenviadisabled = false;

        vm.eventoValidar = eventoValidar;
        vm.nombreComponente =

            $sessionStorage.sessionDocumentos = 0;
        vm.accionId = $sessionStorage.accionId;

        $scope.$watch(() => vm.actualizacomponentes
            , (newVal, oldVal) => {
                if (newVal) {
                    if (vm.tramiteid !== '' && vm.tramiteid !== undefined && vm.tramiteid !== null) {
                        
                    }
                }
            }, true);

        $scope.$watch(() => $sessionStorage.usuario
            , (newVal, oldVal) => {
                if (newVal) {
                    if ($sessionStorage.usuario.roles.find(x => x.Nombre.includes('Analista')) && !$sessionStorage.soloLectura)
                        vm.rolAnalista = true;
                }
            }, true);

        $scope.$watch('vm.deshabilitar', function () {
            if (vm.deshabilitar === true) {
                vm.callback({ validacion: null, arg: true, aprueba: false, titulo: '', ocultarDevolver: true });
            }
        });
      

        //Inicio
        vm.init = function () {
            

            vm.setSessionValues();
            vm.inicializarComponenteCheck(true);
            if (vm.IdNivel === constantesBackbone.idNivelSeleccionProyectos.toLowerCase()) {
                var roles = sesionServicios.obtenerUsuarioIdsRoles();
                var rol = roles.find(x => x === constantesBackbone.idRPresupuesto.toLowerCase());
                if (rol !== undefined)
                    vm.IdRol = rol;
            }
            
            var roles = sesionServicios.obtenerUsuarioIdsRoles();
            vm.rolAnalista = roles.find(x => x === constantesBackbone.idRAnalistaDIFP.toLowerCase()) === constantesBackbone.idRAnalistaDIFP ? true : false;
            vm.muestracapitulospaso3 = !$sessionStorage.soloLectura ? true : false;
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
            let ocultarDevolver = false;
            let oculatarSiguiente = true;
            if ((vm.IdNivel.toLowerCase() === constantesBackbone.idNivelAprobacionEntidad.toLowerCase() || vm.IdNivel.toLowerCase() === constantesBackbone.idNivelRevisionConcepto.toLowerCase())) {
                ocultarDevolver = true;
                oculatarSiguiente = true;
                $sessionStorage.GuardarAprobacionEntidadVFExec = true;
            }
            vm.callback({ validacion: validacion, arg: oculatarSiguiente, aprueba: false, titulo: '', ocultarDevolver: ocultarDevolver });

        };


        vm.notificarGuardado = notificarGuardado;
        function notificarGuardado(botonDevolver, botonSiguiente, ocultarDevolver) {
            
            let vocultarDevolver = ocultarDevolver
            if ($sessionStorage.GuardarAprobacionEntidadVFExec !== undefined)
                vocultarDevolver = $sessionStorage.GuardarAprobacionEntidadVFExec;
            if (vm.deshabilitar === true) {
                vocultarDevolver = true;
            }

            vm.callback({ validacion: null, arg: true, aprueba: false, titulo: '', ocultarDevolver: vocultarDevolver });

        }


        vm.setSessionValues = function () {
            vm.instanciaId = $sessionStorage.idInstanciaIframe;
        };

        /**
       * Listado de componentes hijos, obligatorio para estructura de validación
       * */
        vm.handlerComponentes = [
            { id: 1, componente: 'proyecto', handlerValidacion: null, handlerCambios: null, esValido: true, errores: null },
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
                'proyecto': true,
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
            // vm.validarSiguiente = false;
            vm.accionId = $sessionStorage.idAccion;
            //vm.inicializarComponenteCheck();
            var error = false;
            tramiteVigenciaFuturaServicio.obtenerErroresTramite(vm.guiMacroproceso, vm.instanciaId, vm.accionId, vm.TieneCDP).then(function (respuesta) {
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

        vm.visualizarAlerta = function (error, errorObservacion) {
            if (error) utilidades.mensajeError("Revise los campos señalados y valide nuevamente.", null, "Hay datos que presentan inconsistencias");
            else utilidades.mensajeSuccess("¡Validación éxitosa!", false, false, false);

            var hijosCorrectos = (error == false);

            vm.siguienteDisabled = (hijosCorrectos == false) ;
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
            if (indx > -1) {
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
            if (vm.deshabilitar === true) {
                vm.callback({ validacion: null, arg: true, aprueba: false, titulo: '', ocultarDevolver: true });
            }
            else {
                let voculatarSiguiente = false;
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
                    
                vm.callback({ validacion: null, arg: voculatarSiguiente, aprueba: false, titulo: '', ocultarDevolver: vocultarDevolver });
            }
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


      
    }

    angular.module('backbone').component('tramiteTrasladoOrdinario', {
        templateUrl: "src/app/formulario/ventanas/tramiteTrasladoOrdinario/tramiteTrasladoOrdinario.html",
        controller: tramiteTrasladoOrdinarioController,
        controllerAs: "vm",
        bindings: {
            callback: '&',
            registrarEvento: '&',
           
        }
    });

})();