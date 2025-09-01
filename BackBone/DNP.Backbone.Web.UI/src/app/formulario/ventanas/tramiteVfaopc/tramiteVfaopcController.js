(function () {
    'use strict';

    tramiteVfaopcController.$inject = [
        '$sessionStorage',
        '$scope',
        'tramiteVigenciaFuturaServicio',
        'constantesBackbone',
        'utilidades',
        'utilsValidacionSeccionCapitulosServicio',
        'archivoServicios',
    ];

    function tramiteVfaopcController(
        $sessionStorage,
        $scope,
        tramiteVigenciaFuturaServicio,
        constantesBackbone,
        utilidades,
        utilsValidacionSeccionCapitulosServicio,
        archivoServicios,
    ) {
        var vm = this;
        $sessionStorage.proyectoId = undefined;
        $sessionStorage.BPIN = undefined;
        vm.IdNivel = $sessionStorage.idNivel;

        //Esto se necesita para asignar los capitulos
        vm.guiMacroproceso = constantesBackbone.idEtapaTramitesEjecucion;
        vm.notificacionCambiosCapitulos = null;
        vm.BPIN = undefined;
        vm.instanciaId = '';
        vm.tramiteDetail = {
            tramiteId: null,
            tipoTramiteId: ''
        };
        //Métodos
        vm.mostrarOcultarFlujo = mostrarOcultarFlujo;
        vm.notificarGuardado = notificarGuardado;
        vm.eventoValidar = eventoValidar;
        vm.aprueba = false;
        $("#editarButton").hide();
        $("#validarButton").show();

        vm.eventoHabilitarEdicion = eventoHabilitarEdicion;
        $sessionStorage.sessionDocumetos = 0;
        vm.accionId = $sessionStorage.accionId;
        $sessionStorage.TieneCDP = false;

        //Inicio
        vm.init = function () {
            vm.setSessionValues();
            vm.obtenerDetalleTramite();
            vm.inicializarComponenteCheck(true);
            utilsValidacionSeccionCapitulosServicio.getSeccionCapitulo(vm.guiMacroproceso, $sessionStorage.idNivel, $sessionStorage.idFlujoIframe).then(function (respuesta) {
                vm.setCapitulosHijos(respuesta.data);
            });
            var validacion = {
                evento: vm.eventoValidar
            }
            //vm.callback({ validacion: validacion });
            vm.callback({ validacion: validacion, arg: true });

            if ($sessionStorage.usuario.roles.find(x => x.Nombre.includes('Analista')) && !$sessionStorage.soloLectura)
                vm.rolAnalista = true;

        };

        vm.validarFormulario = function () {
            eventoValidar();
        };

        vm.setSessionValues = function () {
            vm.instanciaId = $sessionStorage.idInstanciaIframe;
        };

        //vm.setDefaultValues = function () {
        //    cambiarImagen(0, "AsociarProyectoImg");
        //    cambiarImagen(0, "informacionPresupuestal");
        //    cambiarImagen(0, "justificacion");
        //    cambiarImagen(0, "documentoSoporte");

        //    cambiarImagenLapiz(0);
        //};

        vm.obtenerDetalleTramite = function () {
            tramiteVigenciaFuturaServicio.obtenerDetalleTramitePorInstancia(vm.instanciaId)
                .then(function (response) {
                    if (response?.data?.Estado) {
                        vm.tramiteDetail = {
                            tramiteId: response.data.Data.TramiteId,
                            tipoTramiteId: response.data.Data.TipoTramiteId
                        };

                        $sessionStorage.TramiteID = vm.tramiteDetail.tramiteId;
                        $sessionStorage.tipoTramiteId = vm.tramiteDetail.tipoTramiteId;

                        
                    }
                }, function (error) {
                    /*utilidades.mensajeError('No fue posible consultar el listado de conpes');*/
                });
            vm.TramiteID = $sessionStorage.TramiteID;
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

        function notificarGuardado() {
            vm.callback({ validacion: null, arg: true, aprueba: false, titulo: '' });

        }




        /**
        * Listado de componentes hijos, obligatorio para estructura de validación
        * */
        vm.handlerComponentes = [
            { id: 1, componente: 'asociarproyecto', handlerValidacion: null, handlerCambios: null, esValido: true, errores: null },
            { id: 2, componente: 'informacionpresupuestal', handlerValidacion: null, handlerCambios: null, esValido: true, errores: null },
            { id: 3, componente: 'justificacion', handlerValidacion: null, handlerCambios: null, handlerCapitulos: [] },
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
                'solicitarconcepto':true
            };
        }



        function eventoValidar() {
            // vm.validarSiguiente = false;
            vm.accionId = $sessionStorage.idAccion;
            vm.TieneCDP = $sessionStorage.TieneCDP;
            var error = false;
            tramiteVigenciaFuturaServicio.obtenerErroresTramite(vm.guiMacroproceso, vm.idInstancia, vm.accionId, vm.TieneCDP).then(function (respuesta) {
                vm.notificacionValidacionHijos(respuesta.data);
                var indexobs = respuesta.data.findIndex(p => p.Capitulo == 'observaciones');
                var errorObservacion = respuesta.data[indexobs].Errores == null ? false : true;

                var findErrors = respuesta.data.findIndex(p => p.Errores != null);
                vm.visualizarAlerta((findErrors != -1), errorObservacion);
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
            //se llena el error cuando la variable de docuemnto no esta al 100%

            let error = {};
            var erroresList = errores.filter(p => p.Seccion == "documentossoporte");
            if (vm.TieneCDP === false && $sessionStorage.sessionDocumentos >=80)
                $sessionStorage.sessionDocumentos = 100
            if (($sessionStorage.sessionDocumentos < 100 || $sessionStorage.sessionDocumentos === undefined) && $sessionStorage.idNivel.toUpperCase() === constantesBackbone.idNivelSeleccionProyectos
                && erroresList[0].Errores === null) {
                var indexobs = errores.findIndex(p => p.Seccion == "documentossoporte");
                errores.splice(indexobs, 1);

                error = {
                    Seccion: "documentossoporte",
                    Capitulo: "alojararchivo",
                    Errores: '{"documentossoportealojararchivo":[{"Error":"VFO006","Descripcion":"Diligencie los documentos obligatorios"}]}',
                }
            }
            //else {
            //    error = {
            //        Seccion: "documentossoporte",
            //        Capitulo: "alojararchivos",
            //        Errores: null,
            //    }
            //}
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
            vm.callback({ arg: !estado, aprueba: false, titulo: '' });
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


        /* ---------------------- Validaciones ---------------*/

        /**
         * Función que crea las referencias de los métodos de los hijos con el padre 
         * @param {any} handler función referenciada
         * @param {any} nombreComponente nombre configurado en cada uno de los componentes hijos vm.nombreComponente = 'datosgenerales'
         */
        //vm.notificacionValidacion = function (handler, nombreComponente) {
        //    var indx = vm.handlerComponentes.findIndex(p => p.componente == nombreComponente);
        //    vm.handlerComponentes[indx].handlerValidacion = handler;
        //};





        /**
         * Función que recibe los estados de los componentes hijos
         * @param {any} estado true: valido, false: invalido
         * @param {any} nombreComponente nombre configurado en cada uno de los componentes hijos vm.nombreComponente = 'datosgenerales'
         */


        /**
         * Función que envía listado de errores a componentes hijos 
         * por referencia configurada en vm.notificacionValidacion.
         * @param {any} errores
         */






        /* ---------------------- asociarProyecto ---------------*/


        vm.notificacionCambiosAsociarProyecton = function (nombreComponente, nombreComponenteHijo) {
            var componente = vm.handlerComponentes.find(p => p.componente == 'asociarProyecto');
            if (componente != undefined) componente.handlerCambios({ nombreComponente: nombreComponente, nombreComponenteHijo: nombreComponenteHijo });
        };

        vm.notificacionCambiosAsociarProyecton = function (handler) {
            var componente = vm.handlerComponentes.find(p => p.componente == 'asociarProyecto');
            if (componente != undefined) componente.handlerCambios = handler;
        };



        //vm.notificacionCambiosInformacionpresupuestal = function (nombreComponente, nombreComponenteHijo) {
        //    var componente = vm.handlerComponentes.find(p => p.componente == 'informacionpresupuestal');
        //    if (componente != undefined) componente.handlerCambios({ nombreComponente: nombreComponente, nombreComponenteHijo: nombreComponenteHijo });
        //};

        //vm.notificacionCambiosInformacionpresupuestal = function (handler) {
        //    var componente = vm.handlerComponentes.find(p => p.componente == 'informacionpresupuestal');
        //    if (componente != undefined) componente.handlerCambios = handler;
        //};

        ///* ---------------------- justificacionvigenciafutura ---------------*/

        //vm.guardadohijos = function (nombreComponente, nombreComponenteHijo) {
        //    vm.notificacionCambiosAsociarProyecto(nombreComponente, nombreComponenteHijo);
        //}

        //vm.notificacionCambiosJustificacionvigenciafutura = function (nombreComponente, nombreComponenteHijo) {
        //    var componente = vm.handlerComponentes.find(p => p.componente == 'justificacionvigenciafutura');
        //    if (componente != undefined) componente.handlerCambios({ nombreComponente: nombreComponente, nombreComponenteHijo: nombreComponenteHijo });
        //};

        //vm.notificacionCambiosJustificacionvigenciafutura = function (handler) {
        //    var componente = vm.handlerComponentes.find(p => p.componente == 'justificacionvigenciafutura');
        //    if (componente != undefined) componente.handlerCambios = handler;
        //};

        ///* ---------------------- documentosoportevf ---------------*/

        //vm.guardadohijos = function (nombreComponente, nombreComponenteHijo) {
        //    vm.notificacionCambiosAsociarProyecto(nombreComponente, nombreComponenteHijo);
        //}

        //vm.notificacionCambiosDocumentosoportevf = function (nombreComponente, nombreComponenteHijo) {
        //    var componente = vm.handlerComponentes.find(p => p.componente == 'documentosoportevf');
        //    if (componente != undefined) componente.handlerCambios({ nombreComponente: nombreComponente, nombreComponenteHijo: nombreComponenteHijo });
        //};

        //vm.notificacionCambiosDocumentosoportevf = function (handler) {
        //    var componente = vm.handlerComponentes.find(p => p.componente == 'documentosoportevf');
        //    if (componente != undefined) componente.handlerCambios = handler;
        //};


    }

    angular.module('backbone').component('tramiteVfaopc', {
        templateUrl: "src/app/formulario/ventanas/tramiteVfaopc/tramiteVfaopc.html",
        controller: tramiteVfaopcController,
        controllerAs: "vm",
        bindings: {
            callback: '&',
            registrarEvento: '&'
        }
    });

})();
