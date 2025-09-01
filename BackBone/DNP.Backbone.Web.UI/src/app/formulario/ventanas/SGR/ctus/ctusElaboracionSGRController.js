(function () {
    'use strict';

    ctusElaboracionSgrController.$inject = [
        '$scope',
        'utilidades',
        '$sessionStorage',
        'ctusElaboracionSgrServicio',
        'justificacionCambiosServicio',
        'utilsValidacionSeccionCapitulosServicio',
        'constantesBackbone',
        'solicitarCtusSgrServicio',
        'transversalSgrServicio',
        'validacionArchivosServicio'
    ];

    function ctusElaboracionSgrController(
        $scope,
        utilidades,
        $sessionStorage,
        ctusElaboracionSgrServicio,
        justificacionCambiosServicio,
        utilsValidacionSeccionCapitulosServicio,
        constantesBackbone,
        solicitarCtusSgrServicio,
        transversalSgrServicio,
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
        vm.IdRol = constantesBackbone.idRolViabilidadDefinitiva;
        vm.idInstanciaPadre = $sessionStorage.InstanciaSeleccionada != undefined ? $sessionStorage.InstanciaSeleccionada.InstanciaPadreId : null;
        vm.coleccion = "tramites"

        vm.notificarGuardado = notificarGuardado;
        vm.eventoValidar = eventoValidar;

        vm.ConfiguracionEntidades = [{
            ProyectoId: 0,
            FaseId: 0,
            Fase: "",
            AplicaTecnico: ""
        }];

        vm.init = function () {
            vm.inicializarComponenteCheck(true);
            vm.callback({ validacion: { evento: vm.eventoValidar }, arg: true, aprueba: false, titulo: '' });
            vm.accionPendiente = $sessionStorage.listadoAccionesTramite.some(x => x.AccionInstanciaId === null)
            utilsValidacionSeccionCapitulosServicio.getSeccionCapitulo(vm.guiMacroproceso, $sessionStorage.idNivel, $sessionStorage.idFlujoIframe).then(function (respuesta) {
                vm.setCapitulosHijos(respuesta.data);
            });
            ObtenerProyectoCtus(vm.ProyectoId, vm.idInstanciaPadre);
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
            { id: 1, componente: 'sgrctuselaboracioninfogeneral', handlerValidacion: null, handlerCambios: null, esValido: true, errores: null },
            { id: 2, componente: 'sgrctuselaboraciondatosverificacion', handlerValidacion: null, handlerCambios: null, esValido: true, errores: null },
            { id: 3, componente: 'sgrctuselaboracionverificacion', handlerValidacion: null, handlerCambios: null, esValido: true, errores: null, handlerInicio: null },
            { id: 4, componente: 'sgrctuselaboracionctus', handlerValidacion: null, handlerCambios: null, esValido: true, errores: null },
            { id: 5, componente: 'sgrctussoportes', handlerValidacion: null, handlerCambios: null, esValido: true, errores: null },

        ];

        vm.handlerComponentesChecked = {};

        vm.inicializarComponenteCheck = function (estado) {
            vm.siguienteDisabled = true;
            vm.handlerComponentesChecked = {
                'sgrctuselaboracioninfogeneral': true,
                'sgrctuselaboraciondatosverificacion': true,
                'sgrctuselaboracionverificacion': true,
                'sgrctuselaboracionctus': true,
                'sgrctussoportes': true,
            };
        }

        function ObtenerProyectoCtus(proyectoId, instanciaId) {
            solicitarCtusSgrServicio.SGR_Proyectos_LeerProyectoCtus(proyectoId, instanciaId)
                .then(function (response) {
                    if (response.data != null) {
                        transversalSgrServicio.notificarCambio({ proyectoCtus: response.data });
                    }
                }, function (error) {
                    utilidades.mensajeError('Ocurrió un problema al leer la información del CTUS.');
                    return "";
                });
        }

        vm.obtenerDetalleTramiteyRol = function () {

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
            const seccionCap = 'sgrctussoportes';

            //validar que obtenerErroresviabilidadSgr sea del servico correcto
            ctusElaboracionSgrServicio.obtenerErroresviabilidadSgr(vm.guiMacroproceso, vm.ProyectoId, vm.IdNivel, vm.idInstancia)
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
        //    ctusElaboracionSgrServicio.obtenerErroresviabilidadSgr(vm.guiMacroproceso, vm.ProyectoId, vm.IdNivel, vm.idInstancia).then(function (respuesta) {

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
        //                        idNivel: vm.IdNivel
        //                        /*idRol: vm.IdRol,*/
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

        //                        var seccion = 'sgrctussoportes';
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

        vm.iconCumple = function (imagenSi, imagenNo, imagenObs, imagenNvObs) {
            if (vm.cumple == undefined || vm.cumple == '')
                return "";
            else if (vm.cumple == "v")
                return imagenSi;
            else if (vm.cumple == "obs")
                return imagenObs;
            else if (vm.cumple == "nvobs")
                return imagenNvObs;
            else
                return imagenNo;
        }

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
            if (error) utilidades.mensajeError("Revise los campos señalados y valide nuevamente.", null, "Hay datos que presentan inconsistencias");
            else utilidades.mensajeSuccess("¡Validación exitosa!", false, false, false);

            var hijosCorrectos = (error == false);

            vm.siguienteDisabled = (hijosCorrectos == false);

            var ocultarDevolver = $sessionStorage.listadoAccionesTramite.some(x => x.AccionInstanciaId === null);
            ;

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
            /*var nomAlerta = "alert-" + nombreComponente;*/

            var nomAlerta = "";
            if (nombreComponente == "sgrctuselaboracionverificacion" && vm.SeccionIdViabilidad !== '')
                nomAlerta = "alert-" + nombreComponente;
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

            if (nombreComponente === "sgrviabilidadsoportes") {
                var idDevError = document.getElementById("sgrviabilidadsoportesalojararchivo-archivo-error");
                if (idDevError != undefined) {
                    if (esValido) {
                        idDevError.classList.add("hidden");
                    } else {
                        idDevError.classList.remove("hidden");
                    }
                }

                idDevError = document.getElementById("alert-sgrviabilidadsoportesalojararchivo");
                if (idDevError != undefined) {
                    if (esValido) {
                        idDevError.classList.remove("ico-advertencia");
                    } else {
                        idDevError.classList.add("ico-advertencia");
                    }
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
            if (nombreComponente == "sgrctuselaboracionverificacion") {
                vm.refrescarRequisitosVerificacion = handler;
            }
        };

        vm.notificacionCambiosAjustes = function (handler, nombreComponente) {
            for (var i = 0; i < vm.handlerComponentes.length; i++) {
                if (handler != null && vm.handlerComponentes[i].componente == nombreComponente) vm.handlerComponentes[i].handlerCambios = handler;
            }
        };
    }

    angular.module('backbone').component('ctusElaboracionSgr', {
        templateUrl: "src/app/formulario/ventanas/SGR/ctus/ctusElaboracionSgr.html",
        controller: ctusElaboracionSgrController,
        controllerAs: "vm",
        bindings: {
            callback: '&'
        }
    });
})();