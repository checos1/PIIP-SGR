(function () {
    'use strict';

    designacionEjecutorSgrController.$inject = ['$scope', 'designarEjecutorSgrServicio', 'utilidades', '$sessionStorage', 'utilsValidacionSeccionCapitulosServicio', 'constantesBackbone', 'validacionArchivosServicio'];

    function designacionEjecutorSgrController(
        $scope,
        designarEjecutorSgrServicio,
        utilidades,
        $sessionStorage,
        utilsValidacionSeccionCapitulosServicio,
        constantesBackbone,
        validacionArchivosServicio
    ) {
        var vm = this;
        vm.notificacionCambiosCapitulos = null;
        vm.eventoValidar = eventoValidar;
        vm.disabled = false;
        vm.disabled2 = false;
        $scope.respuesta = "";
        $scope.justificacionPriorizacion = "";
        vm.ProyectoId = $sessionStorage.idProyectoEncabezado;
        vm.idInstancia = $sessionStorage.idInstancia;
        vm.notificarGuardado = notificarGuardado;

        vm.IdNivel = $sessionStorage.idNivel;
        vm.coleccion = "tramites"

        vm.IdObjetoNegocio = $sessionStorage.InstanciaSeleccionada != undefined ? $sessionStorage.InstanciaSeleccionada.IdObjetoNegocio : "";
        vm.TipoTramiteId = $sessionStorage.tipoTramiteId;
        //Esto se necesita para asignar los capitulos
        vm.guiMacroproceso = constantesBackbone.idEtapaGestionRecursos;

        vm.accionId = $sessionStorage.accionId;

        vm.ProyectoId = $sessionStorage.idProyectoEncabezado;
        if (vm.ProyectoId === undefined)
            vm.ProyectoId = $sessionStorage.idObjetoNegocio;
        vm.idInstancia = $sessionStorage.idInstancia;     

        vm.init = function () {

            //var roles = sesionServicios.obtenerUsuarioIdsRoles();
            //var rol = roles.find(x => x === constantesBackbone.idRPriorizacion.toLowerCase());
            //if (rol !== undefined)
            //    vm.IdRol = rol;

            vm.inicializarComponenteCheck(true);
            vm.callback({ validacion: { evento: vm.eventoValidar }, arg: true, aprueba: false, titulo: '', ocultarDevolver: false });
            utilsValidacionSeccionCapitulosServicio.getSeccionCapitulo(vm.guiMacroproceso, $sessionStorage.idNivel, $sessionStorage.idFlujoIframe).then(function (respuesta) {                
                vm.setCapitulosHijos(respuesta.data);
            });
        };

        vm.$onDestroy = function () {
            designarEjecutorSgrServicio.limpiarObservadores();
        };

        vm.handlerComponentesChecked = {};

        vm.handlerComponentes = [     
            { id: 1, componente: 'sgrejecutordesignacionentejecutora', handlerValidacion: null, handlerCambios: null, esValido: true, errores: null },
            { id: 2, componente: 'sgrejecutordesignacionentinterventor', handlerValidacion: null, handlerCambios: null, esValido: true, errores: null },
            { id: 3, componente: 'sgrejecutordesignacioncostos', handlerValidacion: null, handlerCambios: null, esValido: true, errores: null },
            { id: 4, componente: 'sgrejecutordesignacionsoporte', handlerValidacion: null, handlerCambios: null, esValido: true, errores: null }   
        ];

        vm.inicializarComponenteCheck = function (estado) {
            vm.siguienteDisabled = true;
            vm.handlerComponentesChecked = {
                'sgrejecutordesignacionentejecutora': true,
                'sgrejecutordesignacionentinterventor': true,
                'sgrejecutordesignacioncostos': true,
                'sgrejecutordesignacionsoporte': true
            };
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

        vm.validarFormulario = function () {
            eventoValidar();
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

        function notificarGuardado() {
            vm.callback({ validacion: null, arg: true, aprueba: false, titulo: '' });
        }

        //Inicio evento validación transaversal
        function eventoValidar() {
            vm.inicializarComponenteCheck();
            //Tener en cuenta para cambiar por el nombre del capitulo correcto
            const seccionCap = 'sgrejecutordesignacionsoporte';
            designarEjecutorSgrServicio.obtenerErroresProyecto(vm.guiMacroproceso, vm.ProyectoId, vm.IdNivel, vm.idInstancia)
                .then(respuesta => {
                    if (!respuesta.data)
                        return;

                    return validacionArchivosServicio.validarArchivosAdjuntos(
                        respuesta.data, vm.section, vm.IdRol, vm.nivelarchivo, vm.idtipotramitepresupuestal, seccionCap
                    );
                })
                .then(respVal => {
                    if (!respVal)
                        return;

                    vm.notificacionValidacionHijos(respVal);

                    //Proceso de validacion observaciones
                    let descObservacion = undefined;
                    const errorObservacion = respVal.some(p => p.Capitulo === 'observaciones' && p.Errores);

                    if (errorObservacion) {                 
                        const item = respVal.find(p => p.Capitulo === 'observaciones');
                        var errores = JSON.parse(item.Errores);
                        descObservacion = errores.observacionesAsociarProyecto[0].Descripcion;                    
                    }
                    
                    const hayErrores = respVal.some(p => p.Errores && p.Seccion !== '');
                    vm.visualizarAlerta(hayErrores, errorObservacion, descObservacion);
                })
                .catch(manejarError);
        }

        vm.visualizarAlerta = function (error, errorObservacion, descObservacion) {
            if (error)
                utilidades.mensajeError("Revise los campos señalados y valide nuevamente.", null, "Hay datos que presentan inconsistencias");
            else
                utilidades.mensajeSuccess("¡Validación éxitosa!", false, false, false);

            var hijosCorrectos = (error == false);
            vm.siguienteDisabled = (hijosCorrectos == false);

            var validacion = {
                tieneError: error,
                tieneErrorObservacion: errorObservacion,
                descObservacion: descObservacion
            }

            vm.callback({ validacion: validacion, arg: vm.siguienteDisabled, aprueba: false, titulo: '' });
        }

        function manejarError(error) {
            const mensaje = error.status === 400 ? (error.data.Message || "Error al realizar la operación") : "Error al realizar la operación";
            utilidades.mensajeError(mensaje);
        }
    }

    angular.module('backbone').component('designacionEjecutorSgr', {
        templateUrl: "/src/app/formulario/ventanas/SGR/gestionRecursos/ejecutor/designacionEjecutorSgr.html",
        controller: designacionEjecutorSgrController,
        controllerAs: "vm",
        bindings: {
            callback: '&',
            notificacionvalidacion: '&',
            notificacionestado: '&',
            notificacioncambios: '&',
            guardadocomponent: '&',
        },
    });

})();