(function () {
    'use strict';

    tramiteLiberacionController.$inject = [
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

    function tramiteLiberacionController(
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
            vm.callback({ validacion: null, arg: botonDevolver, aprueba: botonSiguiente, titulo: '', ocultarDevolver: ocultarDevolver });

        }


        vm.setSessionValues = function () {
            vm.instanciaId = $sessionStorage.idInstanciaIframe;
        };

        /**
       * Listado de componentes hijos, obligatorio para estructura de validación
       * */
        vm.handlerComponentes = [
            { id: 1, componente: 'selecionarvigenciafutura', handlerValidacion: null, handlerCambios: null, esValido: true, errores: null },
            { id: 2, componente: 'aprobacionlbvf', handlerValidacion: null, handlerCambios: null, esValido: true, errores: null },
            { id: 3, componente: 'pasotresliberacionvf', handlerValidacion: null, handlerCambios: null, esValido: true, errores: null }



        ];

        vm.handlerComponentesChecked = {};

        vm.inicializarComponenteCheck = function (estado) {
            vm.siguienteDisabled = true;
            vm.handlerComponentesChecked = {
                'selecionarvigenciafutura': true,
                'aprobacionlbvf': true,
                'pasotresliberacionvf': true
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
                //debugger;
                var findErrors = respuesta.data.findIndex(p => p.Errores != null && p.Capitulo != 'documentopaso');
                vm.visualizarAlerta((findErrors != -1), errorObservacion, paso);
                error = (findErrors != -1);
            });

        }

        function eventoHabilitarEdicion() {
            $("#editarButton").hide();
            $("#validarButton").show();
        }

        vm.visualizarAlerta = function (error, errorObservacion, paso) {
            //debugger;
            if (error) utilidades.mensajeError("Revise los campos señalados y valide nuevamente.", null, "Hay datos que presentan inconsistencias");
            else utilidades.mensajeSuccess("¡Validación éxitosa!", false, false, false);

            var hijosCorrectos = (error == false);

            vm.siguienteDisabled = (hijosCorrectos == false);
            var validacion = {
                tieneError: error,
                tieneErrorObservacion: errorObservacion
            }
            vm.tieneErrorObservacion = errorObservacion;
            
            vm.callback({ validacion: validacion, arg: vm.siguienteDisabled, aprueba: false, titulo: '' });
        }

        vm.notificacionValidacionEstado = function (estado, nombreComponente) {
            debugger;
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
            //debugger;
            var seccion = errores.find(x => x.Seccion == 'selecionarvigenciafutura' && x.Capitulo == 'autorizacionminhacienda');
            if (seccion != null) {
                let error = {};
                if ($sessionStorage.vigenciaHorizonte == 0 || $sessionStorage.vigenciaHorizonte == null) {
                    error = {
                        Seccion: "selecionarvigenciafutura",
                        Capitulo: "autorizacionminhaciendadocs",
                        Errores: '{"selecionarvigenciafuturaautorizacionminhaciendadocs":[{"Error":"VFO006","Descripcion":"Diligencie los documentos obligatorios"}]}',
                    }
                } else {
                    error = {
                        Seccion: "selecionarvigenciafutura",
                        Capitulo: "autorizacionminhacienda",
                        Errores: null,
                    }
                }
                errores.push(error);
            }

            var seccion2 = errores.find(x => x.Seccion == 'selecionarvigenciafutura' && x.Capitulo == 'valoresutilizados');
            if (seccion2 != null) {
                let error = {};
                if ($sessionStorage.DescripcionAccionNivel == 0 || $sessionStorage.DescripcionAccionNivel == null) {
                    error = {
                        Seccion: "selecionarvigenciafutura",
                        Capitulo: "valoresutilizadosdocs",
                        Errores: '{"selecionarvigenciafuturavaloresutilizadosdocs":[{"Error":"VFO006","Descripcion":"Diligencie los documentos obligatorios"}]}',
                    }
                } else {
                    error = {
                        Seccion: "selecionarvigenciafutura",
                        Capitulo: "valoresutilizados",
                        Errores: null,
                    }
                }
                errores.push(error);
            }
            //debugger;
            for (var i = 0; i < vm.handlerComponentes.length; i++) {
                if (vm.handlerComponentes[i].handlerValidacion != null) vm.handlerComponentes[i].handlerValidacion({ errores });
            }
        };

        /**
       * Función que visualiza alerta de error tab de componente
       * @param {any} nombreComponente nombre configurado en cada uno de los componentes hijos vm.nombreComponente = 'datosgenerales'
       */
        vm.showAlertError = function (nombreComponente, esValido) {
            debugger;
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

    angular.module('backbone').component('tramiteLiberacion', {
        templateUrl: "src/app/formulario/ventanas/tramiteLiberacion/tramiteLiberacion.html",
        controller: tramiteLiberacionController,
        controllerAs: "vm",
        bindings: {
            callback: '&',
            registrarEvento: '&'
        }
    });

})();