(function () {
    'use strict';

    priorizacionM3SgrController.$inject = ['$scope', 'priorizacionSgrServicio', 'utilidades', '$sessionStorage', 'sesionServicios', 'utilsValidacionSeccionCapitulosServicio', 'constantesBackbone', 'validacionArchivosServicio'];

    function priorizacionM3SgrController(
        $scope,
        priorizacionSgrServicio,
        utilidades,
        $sessionStorage,
        sesionServicios,
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
        vm.eventoValidar = eventoValidar;

        vm.init = function () {

            var roles = sesionServicios.obtenerUsuarioIdsRoles();
            var rol = roles.find(x => x === constantesBackbone.idRPriorizacion.toLowerCase());
            if (rol !== undefined)
                vm.IdRol = rol;

            vm.inicializarComponenteCheck(true);
            vm.callback({ validacion: { evento: vm.eventoValidar }, arg: true, aprueba: false, titulo: '', ocultarDevolver: false });
            utilsValidacionSeccionCapitulosServicio.getSeccionCapitulo(vm.guiMacroproceso, $sessionStorage.idNivel, $sessionStorage.idFlujoIframe).then(function (respuesta) {

                vm.setCapitulosHijos(respuesta.data);
            });
        };

        vm.handlerComponentesChecked = {};

        vm.handlerComponentes = [
            { id: 1, componente: 'sgrpriorizacionm3priorizacion', handlerValidacion: null, handlerCambios: null, esValido: true, errores: null },
            { id: 2, componente: 'sgrpriorizacionm1soportes', handlerValidacion: null, handlerCambios: null, esValido: true, errores: null },
        ];

        vm.inicializarComponenteCheck = function (estado) {
            vm.siguienteDisabled = true;
            vm.handlerComponentesChecked = {
                'sgrpriorizacionM3priorizacion': true,
                'sgrpriorizacionM3soportes': true,
            };
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
        vm.validarFormulario = function () {
            eventoValidar();
        };

        vm.setCapitulosHijos = function (listadoCapitulos) {
            for (var i = 0; i < vm.handlerComponentes.length; i++) {
                if (vm.handlerComponentes[i].handlerCapitulos != null) vm.handlerComponentes[i].handlerCapitulos(listadoCapitulos);
            }
        };

        //function eventoValidar() {
        //    vm.inicializarComponenteCheck();
        //    priorizacionSgrServicio.obtenerErroresProyecto(vm.guiMacroproceso, vm.ProyectoId, vm.IdNivel, vm.idInstancia).then(function (respuesta) {

        //        vm.cumple = '';
        //        vm.notificacionValidacionHijos(respuesta.data);
        //        var errorObservacion = false;
        //        var indexobs = respuesta.data.findIndex(p => p.Capitulo == 'observaciones');
        //        if (indexobs != -1)
        //            errorObservacion = respuesta.data[indexobs].Errores == null ? false : true;

        //        var findErrors = respuesta.data.findIndex(p => p.Errores != null);
        //        var error = respuesta.data.findIndex(p => p.Errores != null && p.Seccion != '');
        //        var erroresCumple = respuesta.data.filter(p => p.Errores != null && p.Seccion == '');
        //        var erroresJson = erroresCumple.length == 0 ? [] : JSON.parse(erroresCumple[0].Errores);

        //        vm.visualizarAlerta((findErrors != -1 || error != -1), errorObservacion);
        //    });
        //}

        vm.notificacionValidacionHijos = function (errores) {
            for (var i = 0; i < vm.handlerComponentes.length; i++) {
                if (vm.handlerComponentes[i].handlerValidacion != null) vm.handlerComponentes[i].handlerValidacion({ errores });
            }
        };

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
            const seccionCap = 'sgrpriorizacionm1soportes';

            //validar que obtenerErroresviabilidadSgr sea del servico correcto
            priorizacionSgrServicio.obtenerErroresProyecto(vm.guiMacroproceso, vm.ProyectoId, vm.IdNivel, vm.idInstancia)
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
                    const errorObservacion = respVal.some(p => p.Capitulo === 'observaciones' && p.Errores);
                    const hayErrores = respVal.some(p => p.Errores && p.Seccion !== '');
                    vm.visualizarAlerta(hayErrores, errorObservacion);
                })
                .catch(manejarError);
        }

        function manejarError(error) {
            const mensaje = error.status === 400 ? (error.data.Message || "Error al realizar la operación") : "Error al realizar la operación";
            utilidades.mensajeError(mensaje);
        }

        //Fin evento validación transaversal

        //function eventoValidar() {
        //    vm.inicializarComponenteCheck();
        //    priorizacionSgrServicio.obtenerErroresProyecto(vm.guiMacroproceso, vm.ProyectoId, vm.IdNivel, vm.idInstancia).then(function (respuesta) {

        //        if (respuesta.data) {

        //            var idTramite = $sessionStorage.tramiteId;
        //            var tipoTramiteId = $sessionStorage.tipoTramiteId;
        //            vm.IdAccion = $sessionStorage.idAccion

        //            if (vm.idtipotramitepresupuestal !== undefined && vm.idtipotramitepresupuestal !== "") tipoTramiteId = vm.idtipotramitepresupuestal;

        //            if (idTramite === undefined) {
        //                idTramite = 0;
        //            }

        //            archivoServicios.obtenerTipoDocumentoSoportePorRol(tipoTramiteId, "A", idTramite, vm.IdNivel, vm.idInstancia, vm.IdAccion).then(function (resultado) {

        //                var listaTipoArchivosObligatorios = resultado.data.filter(a => a.Obligatorio);

        //                let param
        //                if (vm.IdNivel === vm.nivelarchivo) // Cuando ingresa por esta opción es porque va a mostrar los arvhivos del paso anterior
        //                    param = {
        //                        idInstancia: vm.idInstancia,
        //                        idNivel: vm.nivel,
        //                    };
        //                else {
        //                    param = {
        //                        idInstancia: vm.idInstancia,
        //                        section: vm.section,
        //                        idAccion: vm.IdAccion,
        //                        idNivel: vm.IdNivel,
        //                        idRol: vm.IdRol,
        //                    };
        //                }

        //                if (vm.section != null && vm.section != undefined && vm.section != '') {
        //                    param.section = vm.section;
        //                }

        //                var listaArchivosFaltantes = [];

        //                archivoServicios.obtenerListadoArchivos(param, vm.coleccion).then(function (response) {
        //                    vm.totalRegistrosObligatorio = [];
        //                    vm.totalRegistros = 0;
        //                    if (response === undefined || typeof response === 'string') {
        //                        vm.tieneArchivosAdjuntos = false;
        //                        vm.mensajeError = response;
        //                    } else {
        //                        listaTipoArchivosObligatorios.forEach(archivo => {
        //                            if (response == null || !response.some(a => (a.metadatos.tipodocumentoid === archivo.TipoDocumentoId) && (a.status !== 'Eliminado'))) {
        //                                listaArchivosFaltantes.push(archivo.TipoDocumento)
        //                            }
        //                        });
        //                    }

        //                    if (listaArchivosFaltantes.length > 0) {
        //                        var mensajeError = "";
        //                        mensajeError = listaArchivosFaltantes.length == 1 ? "El documento " : "Los documentos ";
        //                        mensajeError = mensajeError.concat(listaArchivosFaltantes.join(','));
        //                        mensajeError = mensajeError.concat(listaArchivosFaltantes.length == 1 ? " no ha sido cargado, por favor adjuntarlo para continuar" : " no han sido cargados, por favor adjuntarlos para continuar")

        //                        var seccion = 'sgrpriorizacionM1soportes';
        //                        var capitulo = 'alojararchivo';

        //                        var validacionArchivos = respuesta.data.find(p => p.Seccion == seccion);
        //                        var errorValidacion = JSON.stringify({ [seccion + capitulo]: [{ 'Error': 'SGRVDP1', 'Descripcion': mensajeError, 'Completo': false }] });
        //                        if (validacionArchivos != null) {
        //                            validacionArchivos.Errores = errorValidacion;
        //                        } else {
        //                            var errorSeccion = { 'Seccion': seccion, 'Capitulo': capitulo, 'Errores': errorValidacion }
        //                            respuesta.data.push(errorSeccion)
        //                        }
        //                    }

        //                    vm.notificacionValidacionHijos(respuesta.data);

        //                    var indexobs = respuesta.data.findIndex(p => p.Capitulo == 'observaciones');

        //                    var errorObservacion = false;
        //                    if (indexobs != -1)
        //                        errorObservacion = respuesta.data[indexobs].Errores == null ? false : true;

        //                    var findErrors = respuesta.data.findIndex(p => p.Errores != null && p.Seccion != '');
        //                    vm.visualizarAlerta((findErrors != -1), errorObservacion);

        //                }).catch(error => {
        //                    if (error.status == 400) {
        //                        utilidades.mensajeError(error.data.Message || "Error al realizar la operación");
        //                        return;
        //                    }
        //                    utilidades.mensajeError("Error al realizar la operación");
        //                });

        //            }).catch(error => {
        //                if (error.status == 400) {
        //                    utilidades.mensajeError(error.data.Message || "Error al realizar la operación");
        //                    return;
        //                }
        //                utilidades.mensajeError("Error al realizar la operación");
        //            });
        //        }

        //    }).catch(error => {
        //        if (error.status == 400) {
        //            utilidades.mensajeError(error.data.Message || "Error al realizar la operación");
        //            return;
        //        }
        //        utilidades.mensajeError("Error al realizar la operación");
        //    });
        //}
    }


    angular.module('backbone').component('priorizacionM3Sgr', {
        templateUrl: "/src/app/formulario/ventanas/SGR/Priorizacion/M3/priorizacionM3Sgr.html",

        controller: priorizacionM3SgrController,
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