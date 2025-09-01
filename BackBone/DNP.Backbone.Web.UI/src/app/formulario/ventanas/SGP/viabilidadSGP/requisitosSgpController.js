(function () {
    'use strict';

    requisitosSgpController.$inject = [
        '$scope',
        '$location',
        'utilidades',
        '$sessionStorage',
        'requisitosSgpServicio',
        'justificacionCambiosServicio',
        'archivoServicios',
        'utilsValidacionSeccionCapitulosServicio',
        'transversalSgpServicio'
    ];

    function requisitosSgpController(
        $scope,
        $location,
        utilidades,
        $sessionStorage,
        requisitosSgpServicio,
        justificacionCambiosServicio,
        archivoServicios,
        utilsValidacionSeccionCapitulosServicio,
        transversalSgpServicio
    ) {
        var vm = this;
        vm.user = {};
        vm.lang = "es";

        vm.guiMacroproceso = justificacionCambiosServicio.getIdEtapa($sessionStorage.etapa);
        vm.ProyectoId = $sessionStorage.idProyectoEncabezado;
        if (vm.ProyectoId === undefined)
            vm.ProyectoId = $sessionStorage.idObjetoNegocio;
        vm.tipotramiteid = $sessionStorage.tipotramiteid;
        vm.tramiteid = $sessionStorage.tipotramiteid;
        vm.idInstancia = $sessionStorage.idInstancia;
        vm.IdNivel = $sessionStorage.idNivel;
        vm.IdAccion = $sessionStorage.idAccion;
        vm.Bpin = $sessionStorage.idObjetoNegocio;
        vm.coleccion = "tramites"

        vm.notificarGuardado = notificarGuardado;
        vm.eventoValidar = eventoValidar;
        vm.SeccionIdViabilidad = "";
        vm.IdRol = null;
        vm.preguntaDocumento = 3897;
        vm.DevolverProyecto = {};
        vm.habilitarDevolverProyecto = true;

        vm.ConfiguracionEntidades = [{
            ProyectoId: 0,
            FaseId: 0,
            Fase: "",
            AplicaTecnico: ""
        }];

        vm.init = function () {
            vm.inicializarComponenteCheck(true);
            vm.callback({ validacion: { evento: vm.eventoValidar }, arg: true, aprueba: false, titulo: '' });

            utilsValidacionSeccionCapitulosServicio.getSeccionCapitulo(vm.guiMacroproceso, $sessionStorage.idNivel, $sessionStorage.idFlujoIframe).then(function (respuesta) {
                vm.setCapitulosHijos(respuesta.data);
                const span = document.getElementById('d' + respuesta.data[0].SeccionModificado);
                if (span != undefined && span != null) {
                    span.classList.add("active");
                }
            });

            transversalSgpServicio.SGPTransversalLeerParametro("IdPreguntaInsumoOtroSector")
                .then(function (respuestaParametro) {
                    if (respuestaParametro.data) {
                        vm.preguntaDocumento = respuestaParametro.data.Valor;
                    }
                }, function (error) {
                    utilidades.mensajeError(error);
            });

            var accionRequisitos = $sessionStorage.listadoAccionesTramite.find(f => f.Ventana == 'requisitosViabilidadSgp');
            if (accionRequisitos != null && accionRequisitos.Roles.length > 0) {
                vm.IdRol = accionRequisitos.Roles[0].IdRol;
            }
        };

        $scope.tab = 1;

        $scope.setTab = function (newTab) {
            $scope.tab = newTab;
        };

        $scope.isSet = function (tabNum) {
            return $scope.tab === tabNum;
        };

        vm.validarFormulario = function () {
            eventoValidar();
        };

        function notificarGuardado() {
            vm.callback({ validacion: null, arg: true, aprueba: false, titulo: '' });
        }

        /**
        * Listado de componentes hijos, obligatorio para estructura de validación
        * */
        vm.handlerComponentes = [
            { id: 1, componente: 'sgpviabilidadrequisitosdatosgenerales', handlerValidacion: null, handlerCambios: null, esValido: true, errores: null },
            { id: 2, componente: 'sgpviabilidadrequisitosverificacion', handlerValidacion: null, handlerCambios: null, esValido: true, errores: null },
            { id: 3, componente: 'sgpviabilidadrequisitossoportes', handlerValidacion: null, handlerCambios: null, esValido: true, errores: null }
        ];

        vm.handlerComponentesChecked = {};

        vm.inicializarComponenteCheck = function (estado) {
            vm.siguienteDisabled = true;
            vm.handlerComponentesChecked = {
                'sgpviabilidadrequisitosdatosgenerales': true,
                'sgpviabilidadrequisitosverificacion': true,
                'sgpviabilidadrequisitossoportes': true
            };
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

        function eventoValidar() {

            vm.inicializarComponenteCheck();
            requisitosSgpServicio.obtenerErroresviabilidadSgp(vm.guiMacroproceso, vm.ProyectoId, vm.IdNivel, vm.idInstancia).then(function (respuesta) {

                if (respuesta.data) {

                    var idTramite = $sessionStorage.tramiteId;
                    var tipoTramiteId = $sessionStorage.tipoTramiteId;

                    if (vm.idtipotramitepresupuestal !== undefined && vm.idtipotramitepresupuestal !== "") tipoTramiteId = vm.idtipotramitepresupuestal;

                    if (idTramite === undefined) {
                        idTramite = 0;
                    }

                    requisitosSgpServicio.ConsultarAccionPorInstancia(vm.idInstancia, vm.IdAccion).then(function (result) {

                        requisitosSgpServicio.obtenerPreguntasPersonalizadas(vm.Bpin, vm.IdNivel, vm.idInstancia, result.data.EstadoAccionPorInstanciaId).then(function (retorno) {

                            var respuestas = retorno.data;
                            var Elemento = respuestas.PreguntasGenerales[0].Preguntas.find(x => x.IdPregunta == vm.preguntaDocumento)
                            if (Elemento != undefined && Elemento instanceof Object) {
                                var index = respuestas.PreguntasGenerales[0].Preguntas.indexOf(Elemento)
                                vm.documentoOtroSectorObligatorio = respuestas.PreguntasGenerales[0].Preguntas[index].Respuesta == 1;
                            }

                            if (vm.documentoOtroSectorObligatorio) {
                                archivoServicios.obtenerTipoDocumentoSoportePorRol(tipoTramiteId, "A", idTramite, vm.IdNivel, vm.idInstancia, vm.IdAccion).then(function (resultado) {
                                    var listaTipoArchivosObligatorios = resultado.data.filter(a => a.Obligatorio = 1);
                                    let param
                                    if (vm.IdNivel === vm.nivelarchivo) // Cuando ingresa por esta opción es porque va a mostrar los arvhivos del paso anterior
                                        param = {
                                            idInstancia: vm.idInstancia,
                                            idNivel: vm.nivel,
                                        };
                                    else {
                                        param = {
                                            idInstancia: vm.idInstancia,
                                            section: vm.section,
                                            idAccion: vm.IdAccion,
                                            idNivel: vm.IdNivel,
                                            idRol: vm.IdRol,
                                        };
                                    }

                                    if (vm.section != null && vm.section != undefined && vm.section != '') {
                                        param.section = vm.section;
                                    }

                                    var listaArchivosFaltantes = [];

                                    archivoServicios.obtenerListadoArchivos(param, vm.coleccion).then(function (response) {
                                        vm.totalRegistrosObligatorio = [];
                                        vm.totalRegistros = 0;
                                        if (response === undefined || typeof response === 'string') {
                                            vm.tieneArchivosAdjuntos = false;
                                            vm.mensajeError = response;
                                        } else {
                                            listaTipoArchivosObligatorios.forEach(archivo => {
                                                if (response == null || !response.some(a => (a.metadatos.tipodocumentoid === archivo.TipoDocumentoId) && (a.status !== 'Eliminado'))) {
                                                    listaArchivosFaltantes.push(archivo.TipoDocumento)
                                                }
                                            });
                                        }

                                        if (listaArchivosFaltantes.length > 0) {
                                            var mensajeError = "";
                                            mensajeError = listaArchivosFaltantes.length == 1 ? "El documento " : "Los documentos ";
                                            mensajeError = mensajeError.concat(listaArchivosFaltantes.join(','));
                                            mensajeError = mensajeError.concat(listaArchivosFaltantes.length == 1 ? " no ha sido cargado, por favor adjuntarlo para continuar" : " no han sido cargados, por favor adjuntarlos para continuar")

                                            var seccion = 'sgpviabilidadrequisitossoportes'
                                            var capitulo = 'alojararchivosgp'

                                            var validacionArchivos = respuesta.data.find(p => p.Seccion == seccion);
                                            var errorValidacion = JSON.stringify({ [seccion + capitulo]: [{ 'Error': 'SGRVDP1', 'Descripcion': mensajeError, 'Completo': false }] });
                                            if (validacionArchivos != null) {
                                                validacionArchivos.Errores = errorValidacion;
                                            } else {
                                                var errorSeccion = { 'Seccion': seccion, 'Capitulo': capitulo, 'Errores': errorValidacion }
                                                respuesta.data.push(errorSeccion)
                                            }
                                        }

                                        vm.notificacionValidacionHijos(respuesta.data);

                                        var indexobs = respuesta.data.findIndex(p => p.Capitulo == 'observaciones');

                                        var errorObservacion = false;
                                        if (indexobs != -1)
                                            errorObservacion = respuesta.data[indexobs].Errores == null ? false : true;

                                        var findErrors = respuesta.data.findIndex(p => p.Errores != null && p.Seccion != '');
                                        vm.visualizarAlerta((findErrors != -1), errorObservacion);

                                    }).catch(error => {
                                        if (error.status == 400) {
                                            utilidades.mensajeError(error.data.Message || "Error al realizar la operación");
                                            return;
                                        }
                                        utilidades.mensajeError("Error al realizar la operación");
                                    });

                                }).catch(error => {
                                    if (error.status == 400) {
                                        utilidades.mensajeError(error.data.Message || "Error al realizar la operación");
                                        return;
                                    }
                                    utilidades.mensajeError("Error al realizar la operación");
                                });
                            }
                            else {

                                vm.notificacionValidacionHijos(respuesta.data);

                                var indexobs = respuesta.data.findIndex(p => p.Capitulo == 'observaciones');

                                var errorObservacion = false;
                                if (indexobs != -1)
                                    errorObservacion = respuesta.data[indexobs].Errores == null ? false : true;

                                var findErrors = respuesta.data.findIndex(p => p.Errores != null && p.Seccion != '');
                                vm.visualizarAlerta((findErrors != -1), errorObservacion);
                            }
                        }).catch(error => {
                            if (error.status == 400) {
                                utilidades.mensajeError(error.data.Message || "Error al realizar la operación");
                                return;
                            }
                            utilidades.mensajeError("Error al realizar la operación");
                        });
                    }).catch(error => {
                        if (error.status == 400) {
                            utilidades.mensajeError(error.data.Message || "Error al realizar la operación");
                            return;
                        }
                        utilidades.mensajeError("Error al realizar la operación");
                    });
                }

            }).catch(error => {
                if (error.status == 400) {
                    utilidades.mensajeError(error.data.Message || "Error al realizar la operación");
                    return;
                }
                utilidades.mensajeError("Error al realizar la operación");
            });
        }

        vm.devolverProyecto = function () {
            var Observacion = document.getElementById("observacionAprobacion");

            if (Observacion.value != null) {

                utilidades.mensajeWarning(
                    "¿Está seguro de continuar?",
                    function funcionContinuar() {
                        vm.DevolverProyecto.InstanciaId = vm.idInstancia;
                        vm.DevolverProyecto.Bpin = vm.Bpin;
                        vm.DevolverProyecto.ProyectoId = vm.Bpin;
                        vm.DevolverProyecto.Observacion = Observacion.value;
                        vm.DevolverProyecto.DevolverId = true;
                        vm.DevolverProyecto.EstadoDevolver = 7; //Returned	Solicitud de Información MGA

                        return requisitosSgpServicio.devolverProyectoSGP(vm.DevolverProyecto).then(
                            function (response) {
                                if (response.data && response.statusText === "OK") {
                                    if (response.data.Exito) {
                                        utilidades.mensajeSuccess("", false, false, false, "Este proyecto se devolvió exitosamente a la MGA");
                                        $location.url("/proyectos/pl");
                                    } else {
                                        swal('', response.data.Mensaje, 'warning');
                                    }

                                } else {
                                    swal('', "Error al realizar la operación", 'error');
                                }
                            }
                        );
                    },
                    function funcionCancelar(reason) {
                        console.log("reason", reason);
                    },
                    "Aceptar",
                    "Cancelar",
                    "Este proyecto se devolverá a la MGA");
            }
            else {
                swal('El formulario no esta diligenciado en su totalidad', 'Revise las campos señalados para que sean validados nuevamente.', 'error');
            }
        };

        vm.visualizarAlerta = function (error, errorObservacion) {
            if (error) utilidades.mensajeError("Revise los campos señalados y valide nuevamente.", null, "Hay datos que presentan inconsistencias");
            else {
                vm.habilitarDevolverProyecto = false;
                utilidades.mensajeSuccess("¡Validación exitosa!", false, false, false);
            }

            var hijosCorrectos = (error == false);

            vm.siguienteDisabled = (hijosCorrectos == false);

            var ocultarDevolver = true;

            var validacion = {
                tieneError: error,
                tieneErrorObservacion: errorObservacion
            }
            vm.callback({ validacion: validacion, arg: vm.siguienteDisabled, aprueba: false, titulo: '', ocultarDevolver: ocultarDevolver });
        }

        /* ---------------------- Validaciones ---------------*/

        vm.notificacionValidacion = function (handler, nombreComponente, handlerCapitulos) {
            var indx = vm.handlerComponentes.findIndex(p => p.componente == nombreComponente);
            vm.handlerComponentes[indx].handlerValidacion = handler;
            vm.handlerComponentes[indx].handlerCapitulos = handlerCapitulos;
        };

        vm.notificacionValidacionEstado = function (estado, nombreComponente) {
            vm.showAlertError(nombreComponente, estado);
            vm.handlerComponentesChecked[nombreComponente] = estado;
        }

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

        /**
       * Función que visualiza alerta de error tab de componente
       * @param {any} nombreComponente nombre configurado en cada uno de los componentes hijos vm.nombreComponente = 'datosgenerales'
       */
        vm.showAlertError = function (nombreComponente, esValido) {
            var nomAlerta = "";
            if (nombreComponente == "sgpviabilidadrequisitosverificacion" && vm.SeccionIdViabilidad !== '')
                nomAlerta = "alert-" + vm.SeccionIdViabilidad + "_" + nombreComponente;
            else
                nomAlerta = "alert-" + nombreComponente;

            var idSpanAlertComponent = document.getElementById(nomAlerta);
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
            vm.notificacionCambios(nombreComponente, nombreComponenteHijo);
        }

        vm.refrescarRequisitosVerificacion = null;
        vm.notificacionCambios = function (nombreComponente, nombreComponenteHijo) {
            for (var i = 0; i < vm.handlerComponentes.length; i++) {
                if (vm.handlerComponentes[i].handlerCambios != null) vm.handlerComponentes[i].handlerCambios({ nombreComponente: nombreComponente, nombreComponenteHijo: nombreComponenteHijo });
            }
            if (nombreComponente == "sgpviabilidadrequisitosverificacion") {
                vm.refrescarRequisitosVerificacion = handler;
            }
        };

        vm.notificacionCambiosAjustes = function (handler, nombreComponente) {
            for (var i = 0; i < vm.handlerComponentes.length; i++) {
                if (handler != null && vm.handlerComponentes[i].componente == nombreComponente) vm.handlerComponentes[i].handlerCambios = handler;
            }
        };

    }

    angular.module('backbone').component('requisitosSgp', {
        templateUrl: "/src/app/formulario/ventanas/SGP/viabilidadSGP/requisitosSgp.html",
        controller: requisitosSgpController,
        controllerAs: "vm",
        bindings: {
            callback: '&'
        }
    });
})();