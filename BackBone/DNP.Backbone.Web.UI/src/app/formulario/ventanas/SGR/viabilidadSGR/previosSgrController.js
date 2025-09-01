(function () {
    'use strict';

    previosSgrController.$inject = [
        '$rootScope',
        '$scope',
        '$location',
        '$uibModal',
        'utilidades',
        '$sessionStorage',
        'previosSgrServicio',
        'justificacionCambiosServicio',
        'utilsValidacionSeccionCapitulosServicio',
        'validacionArchivosServicio'
    ];

    function previosSgrController(
        $rootScope,
        $scope,
        $location,
        $uibModal,
        utilidades,
        $sessionStorage,
        previosSgrServicio,
        justificacionCambiosServicio,
        utilsValidacionSeccionCapitulosServicio,
        validacionArchivosServicio
    ) {
        var vm = this;
        vm.user = {};
        vm.lang = "es";

        vm.guiMacroproceso = justificacionCambiosServicio.getIdEtapa($sessionStorage.etapa);
        vm.ProyectoId = $sessionStorage.idProyectoEncabezado;
        if (vm.ProyectoId === undefined)
            vm.ProyectoId = $sessionStorage.idObjetoNegocio;
        vm.idInstancia = $sessionStorage.idInstancia;
        vm.IdNivel = $sessionStorage.idNivel;
        vm.nombreAccion = $sessionStorage.nombreAccion;
        vm.buttonMGA = true;
        if (vm.nombreAccion == 'Previos viabilidad OCAD Paz') {
            vm.buttonMGA = false;
        }
        vm.coleccion = "tramites"

        vm.notificarGuardado = notificarGuardado;
        vm.eventoValidar = eventoValidar;
        vm.SeccionIdViabilidad = "";

        vm.abrirLogInstancias = abrirLogInstancias;

        vm.DevolverProyecto = {};
        vm.habilitarDevolverProyecto = true;

        vm.HabilitarGuardarPaso = true;
        vm.textoEnEsperaFinalizacionConcepto = "";

        vm.mostrarCodigoSIGP = false;

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
                var indexobs = respuesta.data.findIndex(p => p.SeccionModificado == 'viabilidad');
                vm.SeccionIdViabilidad = respuesta.data[indexobs].SeccionId;
            });

            previosSgrServicio.validarInstanciaCTUSNoFinalizada(vm.ProyectoId).then(
                
                function (respuesta) {

                    if (respuesta.data !== '') {

                        var respuestaJson = JSON.parse(respuesta.data);

                        if (typeof respuestaJson.InstanciaCTUSNoFinalizada !== "undefined") {

                            var bloquearPorCTUSPendiente = respuestaJson.InstanciaCTUSNoFinalizada;

                            if (bloquearPorCTUSPendiente) {
                                $sessionStorage.HabilitarGuardarPaso = false;
                            } else {
                                $sessionStorage.HabilitarGuardarPaso = true;
                            }

                            vm.HabilitarGuardarPaso = $sessionStorage.HabilitarGuardarPaso;
                            vm.habilitarDevolverProyecto = $sessionStorage.HabilitarGuardarPaso;

                            if (!vm.HabilitarGuardarPaso) {
                                vm.textoEnEsperaFinalizacionConcepto = respuestaJson.MensajeRespuesta;
                                $sessionStorage.soloLectura = true;
                                $rootScope.$broadcast("BloquearPorCTUSPendiente", true);
                            }
                        } else {
                            $sessionStorage.HabilitarGuardarPaso = true;
                        }
                    } else {
                        $sessionStorage.HabilitarGuardarPaso = true;
                    }
                }
            ).catch(error => {
                if (error.status == 400) {
                    utilidades.mensajeError(error.data.Message || "Error al realizar la operación");
                    return;
                }
                utilidades.mensajeError("Error al validar instancia CTUS");
            });

            var flujoCTEI = utilidades.obtenerParametroTransversal('FlujoCTEI');
            vm.mostrarCodigoSIGP = flujoCTEI.split(',').some(e => e === $sessionStorage.idFlujoIframe);
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

        vm.$onDestroy = function () {
            previosSgrServicio.limpiarObservadores();
        };
        /**
        * Listado de componentes hijos, obligatorio para estructura de validación
        * */
        vm.handlerComponentes = [
            { id: 1, componente: 'viabilidad', handlerValidacion: null, handlerCambios: null, esValido: true, errores: null },
            { id: 2, componente: 'sgrviabilidadpreviosrecursos', handlerValidacion: null, handlerCambios: null, esValido: true, errores: null, handlerInicio: null },
            { id: 3, componente: 'sgrviabilidadpreviosoperacioncredito', handlerValidacion: null, handlerCambios: null, esValido: true, errores: null, handlerInicio: null },
            { id: 4, componente: 'sgrviabilidadpreviossoporte', handlerValidacion: null, handlerCambios: null, esValido: true, errores: null },
            { id: 5, componente: 'sgrviabilidadpreviosregionalizacion', handlerValidacion: null, handlerCambios: null, esValido: true, errores: null, handlerInicio: null },
            { id: 6, componente: 'sgrviabilidadpreviosfocalizacion', handlerValidacion: null, handlerCambios: null, esValido: true, errores: null, handlerInicio: null },
            { id: 7, componente: 'sgrviabilidadpreviosejecutor', handlerValidacion: null, handlerCambios: null, esValido: true, errores: null, handlerInicio: null },
            { id: 8, componente: 'sgrviabilidadpreviosdatosgenerales', handlerValidacion: null, handlerCambios: null, esValido: true, errores: null, handlerInicio: null },
        ];

        vm.handlerComponentesChecked = {};

        vm.inicializarComponenteCheck = function (estado) {
            vm.siguienteDisabled = true;
            vm.handlerComponentesChecked = {
                'viabilidad': true,
                'sgrviabilidadpreviosrecursos': true,
                'sgrviabilidadpreviosoperacioncredito': true,
                'sgrviabilidadpreviossoporte': true,
                'sgrviabilidadpreviosregionalizacion': true,
                'sgrviabilidadpreviosfocalizacion': true,
                'sgrviabilidadpreviosejecutor': true,
                'sgrviabilidadpreviosdatosgenerales': true,
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

        //Inicio evento validación transaversal
        function eventoValidar() {
            vm.inicializarComponenteCheck();
            //Tener en cuenta para cambiar por el nombre del capitulo correcto
            const seccionCap = 'sgrviabilidadpreviossoporte';

            //validar que obtenerErroresviabilidadSgr sea del servico correcto
            previosSgrServicio.obtenerErroresviabilidadSgr(vm.guiMacroproceso, vm.ProyectoId, vm.IdNivel, vm.idInstancia)
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
                    vm.visualizarAlertaSeccion(respVal);
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
        //    previosSgrServicio.obtenerErroresviabilidadSgr(vm.guiMacroproceso, vm.ProyectoId, vm.IdNivel, vm.idInstancia).then(function (respuesta) {

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
        //                if (vm.IdNivel === vm.nivelarchivo)
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

        //                        var seccion = 'sgrviabilidadpreviossoporte';
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

        vm.devolverProyecto = function () {
            var Observacion = document.getElementById("observacionAprobacion");
            if (Observacion.value != null && Observacion.value !== "") {
                vm.DevolverProyecto.InstanciaId = vm.idInstancia;
                vm.DevolverProyecto.Bpin = vm.ProyectoId;
                vm.DevolverProyecto.ProyectoId = vm.ProyectoId;
                vm.DevolverProyecto.Observacion = Observacion.value;
                vm.DevolverProyecto.DevolverId = true;
                vm.DevolverProyecto.EstadoDevolver = 7; //Returned	Solicitud de Información MGA
                return previosSgrServicio.devolverProyecto(vm.DevolverProyecto).then(
                    function (response) {
                        if (response.data || response.statusText === "OK") {
                            if (response.data.Exito) {
                                utilidades.mensajeSuccess("Operación realizada con éxito!", false, false, false);
                                $location.url("/proyectos/pl");
                            } else {
                                swal('', response.data.Mensaje, 'warning');
                            }
                        } else {
                            swal('', "Error al realizar la operación", 'error');
                        }
                    }
                );
            }
            else {
                swal('El campo observaciones no se encuentra diligenciado.', 'Revise las campos señalados para que sean validados nuevamente.', 'error');
            }
        };

        vm.visualizarAlertaSeccion = function (errores) {
            for (var i = 0; i < vm.handlerComponentes.length; i++) {
                var indexErr = errores.findIndex(p => p.Seccion == vm.handlerComponentes[i].componente && p.Errores != null);

                if (indexErr >= 0) {
                    vm.showAlertError(vm.handlerComponentes[i].componente, false);
                }
                else {
                    vm.showAlertError(vm.handlerComponentes[i].componente, true);
                }
            }
        }
        vm.visualizarAlerta = function (error, errorObservacion) {
            if (error) {
                /*vm.habilitarDevolverProyecto = true;*/
                utilidades.mensajeError("Revise los campos señalados y valide nuevamente.", null, "Hay datos que presentan inconsistencias");
            }
            else {
               /* vm.habilitarDevolverProyecto = false;*/
                utilidades.mensajeSuccess("¡Validación éxitosa!", false, false, false);
            }

            var hijosCorrectos = (error == false);

            vm.siguienteDisabled = (hijosCorrectos == false || !vm.HabilitarGuardarPaso);

            var validacion = {
                tieneError: error,
                tieneErrorObservacion: errorObservacion
            }
            vm.callback({ validacion: validacion, arg: vm.siguienteDisabled, aprueba: false, titulo: '' });
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
                if (vm.handlerComponentes[i].handlerValidacion != null && errores != null) {
                    try {
                        vm.handlerComponentes[i].handlerValidacion({ errores });
                    } catch (error) {
                        console.error('¡¡Tiene ERRORES - handlerValidacion del componente = ' + vm.handlerComponentes[i].componente + '!!');
                    }
                }
            }
        };

        /**
       * Función que visualiza alerta de error tab de componente
       * @param {any} nombreComponente nombre configurado en cada uno de los componentes hijos vm.nombreComponente = 'datosgenerales'
       */
        vm.showAlertError = function (nombreComponente, esValido) {
            var nomAlerta = "";
            if (nombreComponente == "viabilidad")
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


        function abrirLogInstancias() {


            var modalInstance = $uibModal.open({
                animation: $scope.animationsEnabled,
                templateUrl: 'src/app/comunes/log/modalLogInstanciasSubpasos.html',
                controller: 'modalLogInstanciasSubpasosController',
                controllerAs: "vm",
                openedClass: "consola-modal-soportesDNP",
                resolve: {
                    idInstancia: () => vm.idInstancia,
                    /*
                    BPIN: () => row.IdObjetoNegocio,
                    nombreFlujo: () => row.NombreFlujo,
                    codigoProceso: () => row.CodigoProceso
                    */
                }
            });

            modalInstance.result.then(function (selectedItem) {


            }, function () {

            });
        }
    }

    angular.module('backbone').component('previosSgr', {
        templateUrl: "/src/app/formulario/ventanas/SGR/viabilidadSGR/previosSgr.html",
        controller: previosSgrController,
        controllerAs: "vm",
        bindings: {
            callback: '&'
        }
    });
})();