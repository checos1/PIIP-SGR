(function () {
    'use strict';

    firmaGestionRecursosSgpController.$inject = [
        '$scope',
        'utilidades',
        '$sessionStorage',
        'firmaGestionRecursosSgpServicio',
        'justificacionCambiosServicio',
        'utilsValidacionSeccionCapitulosServicio',
        'constantesBackbone',
        'transversalSgrServicio'
    ];

    function firmaGestionRecursosSgpController(
        $scope,
        utilidades,
        $sessionStorage,
        firmaGestionRecursosSgpServicio,
        justificacionCambiosServicio,
        utilsValidacionSeccionCapitulosServicio,
        constantesBackbone,
        transversalSgrServicio
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
        vm.IdRol = "4ff68a0a-dc57-4252-92db-c41ae7059e3b";

        vm.notificarGuardado = notificarGuardado;
        vm.eventoValidar = eventoValidar;
        vm.SeccionIdViabilidad = "";
        vm.detalleViable = '';
        vm.detalleViableNotificacion = '';
        vm.textEstadoViable = '';
        vm.finalizarProceso = false;

        vm.ConfiguracionEntidades = [{
            ProyectoId: 0,
            FaseId: 0,
            Fase: "",
            AplicaTecnico: ""
        }];

        vm.init = function () {
            vm.inicializarComponenteCheck(true);
            vm.callback({ validacion: { evento: vm.eventoValidar }, arg: true, aprueba: false, titulo: '', ocultarDevolver: true });

            utilsValidacionSeccionCapitulosServicio.getSeccionCapitulo(vm.guiMacroproceso, $sessionStorage.idNivel, $sessionStorage.idFlujoIframe).then(function (respuesta) {
                vm.setCapitulosHijos(respuesta.data);
                var indexobs = respuesta.data.findIndex(p => p.SeccionModificado == 'sgpsolicitudrecursosfirmaresponsables');
                vm.SeccionIdViabilidad = respuesta.data[indexobs].SeccionId;
            });

            transversalSgrServicio.SGR_Transversal_LeerParametro("MensajeProyectoGRNotificacionSGP")
                .then(function (respuestaParametro) {
                    if (respuestaParametro !== undefined) {
                        if (respuestaParametro.data) {
                            vm.detalleViableNotificacion = respuestaParametro.data.Valor;
                        }
                    }
                }, function (error) {
                    utilidades.mensajeError(error);
                });

            transversalSgrServicio.SGR_Transversal_LeerParametro("MensajeProyectoGRSGP")
                .then(function (respuestaParametro) {
                    if (respuestaParametro !== undefined) {
                        if (respuestaParametro.data) {
                            vm.textEstadoViable = respuestaParametro.data.Valor;
                        }
                    }
                }, function (error) {
                    utilidades.mensajeError(error);
                });

            transversalSgrServicio.SGR_Transversal_LeerParametro("MensajeProyectoAprobadoGRDetSGP")
                .then(function (respuestaParametro) {
                    if (respuestaParametro !== undefined) {
                        if (respuestaParametro.data) {
                            vm.detalleViable = respuestaParametro.data.Valor;
                        }
                    }
                }, function (error) {
                    utilidades.mensajeError(error);
                });

            var elementos = document.getElementsByClassName("btnValidar");
            var rolFinalizacionGestionRecursos = utilidades.obtenerParametroTransversal('RolFinalizacionGestionRecursosSGP');
            if (!$sessionStorage.usuario.roles.find(x => x.IdRol == rolFinalizacionGestionRecursos))
                elementos[0].disabled = true;
          
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
            { id: 1, componente: 'sgpsolicitudrecursosfirmaresponsables', handlerValidacion: null, handlerCambios: null, esValido: true, errores: null },

        ];

        vm.handlerComponentesChecked = {};

        vm.inicializarComponenteCheck = function (estado) {
            vm.siguienteDisabled = true;
            vm.handlerComponentesChecked = {
                'sgpsolicitudrecursosfirmaresponsables': true
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
            firmaGestionRecursosSgpServicio.obtenerErroresviabilidadSgr(vm.guiMacroproceso, vm.ProyectoId, vm.IdNivel, vm.idInstancia).then(function (respuesta) {

                const dataArchivo = {
                    idnivel: vm.IdNivel,
                    idinstancia: vm.idInstancia,
                    tipodocumentocodigo: constantesBackbone.codigoTipoDocumentoViabilidad,
                    fecha: $sessionStorage.fechaProcesoViabilidad
                };

                firmaGestionRecursosSgpServicio.SGR_Viabilidad_ValidarCargueDocumentoObligatorio(dataArchivo, "tramites").then(function (response) {
                    if (respuesta.data) {
                        if (response.data != null) {
                            var seccion = 'Soportessgpsolicitudrecursosfirmaresponsables';
                            var capitulo = 'procesofirmasgrsgp';

                            var validacionArchivos = respuesta.data.find(p => p.Seccion == seccion);
                            var indexSeccionArchivos = respuesta.data.findIndex(p => p.Seccion == seccion);
                            if (validacionArchivos != null) {
                                var erroresArchivos = JSON.parse(validacionArchivos.Errores);
                                erroresArchivos[seccion + capitulo].forEach((p, i) => {
                                    var descripcion = p.Descripcion;
                                    p.Completo = false;

                                    var arrDocumentos = descripcion.split(',');
                                    arrDocumentos = arrDocumentos.map(element => {
                                        return element.trim();
                                    });

                                    response.data.forEach(a => {
                                        var tipoDocumento = a.metadatos.tipodocumento;
                                        var indexArchivo = arrDocumentos.findIndex(td => td.includes(tipoDocumento));
                                        if (indexArchivo >= 0) {
                                            arrDocumentos[indexArchivo] = arrDocumentos[indexArchivo].replace(tipoDocumento, '');
                                        }
                                    });
                                    arrDocumentos = arrDocumentos.filter(p => p != '');
                                    descripcion = arrDocumentos.join().replace(',,', '').replace('",', '"');
                                    var indiceInicial = descripcion.indexOf('"');
                                    var indiceFinal = descripcion.lastIndexOf('"');

                                    if (indiceFinal - indiceInicial > 1) {
                                        p.Descripcion = descripcion;
                                    } else {
                                        p.Completo = true;
                                    }
                                })

                                erroresArchivos[seccion + capitulo] = erroresArchivos[seccion + capitulo].filter(f => !f.Completo);

                                if (erroresArchivos[seccion + capitulo].length == 0) {
                                    respuesta.data = [...respuesta.data.slice(0, indexSeccionArchivos), ...respuesta.data.slice(indexSeccionArchivos += 1, respuesta.data.lenght)]
                                } else {
                                    respuesta.data.forEach(p => {
                                        if (p.Seccion == seccion) {
                                            p.Errores = JSON.stringify(erroresArchivos);
                                        }
                                    });
                                }
                            }
                        }
                        vm.cumple = '';
                        vm.notificacionValidacionHijos(respuesta.data);
                        var errorObservacion = false;
                        var indexobs = respuesta.data.findIndex(p => p.Capitulo == 'observaciones');
                        if (indexobs != -1)
                            errorObservacion = respuesta.data[indexobs].Errores == null ? false : true;

                        var findErrors = respuesta.data.findIndex(p => p.Errores != null);
                        var error = respuesta.data.findIndex(p => p.Errores != null && p.Seccion != '');
                        var erroresCumple = respuesta.data.filter(p => p.Errores != null && p.Seccion == '');
                        var erroresJson = erroresCumple.length == 0 ? [] : JSON.parse(erroresCumple[0].Errores);

                        if (error == -1 &&
                            (vm.visualizarCumple || !vm.visualizarCumple)) {
                            vm.cumple = erroresJson.length == 0 ? 'v' : erroresJson.Cumple[0].Error;
                            vm.textEstadoValidacion = erroresJson.length == 0 ? vm.textEstadoViable : erroresJson.Cumple.length > 1 ? erroresJson.Cumple[1].Descripcion : erroresJson.Cumple[0].Descripcion;
                            vm.textDetalleValidacion = erroresJson.length == 0 ? vm.detalleViable : erroresJson.Cumple.length > 1 ? erroresJson.Cumple[1].Detalle : erroresJson.Cumple[0].Detalle;

                            vm.textEstadoValidacionNotificacion = erroresJson.length < 1 ? `${vm.textEstadoViable}${vm.detalleViableNotificacion}` : erroresJson.Cumple[1].Descripcion;

                            vm.visualizarAlertaEstadoProyectoViabilidad((findErrors != -1 || error != -1), errorObservacion, vm.textEstadoValidacionNotificacion);
                            vm.siguienteDisabled = vm.cumple == 'obs' || !vm.finalizarProceso;
                            vm.callback({ validacion: null, arg: vm.siguienteDisabled, aprueba: false, titulo: '' });
                            return;
                        }
                        vm.visualizarAlerta((findErrors != -1 || error != -1), errorObservacion);
                    }
                });
            });
        }

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

        vm.visualizarAlertaEstadoProyectoViabilidad = function (error, errorObservacion, notificacion) {
            utilidades.mensajeSuccess("Si requiere editar algún campo, será necesario validar nuevamente el formulario.", false, false, false, `Tras la validación, el proceso ha sido considerado <br /> ${notificacion}`);
            var hijosCorrectos = (error == false);

            vm.siguienteDisabled = (hijosCorrectos == false);

            var ocultarDevolver = true;
            vm.siguienteDisabled = true;
            
            if ($sessionStorage.usuario.roles.find(x => x.IdRol.includes(vm.IdRol))) {
                ocultarDevolver = false;
                vm.siguienteDisabled = false;
            }

            var validacion = {
                tieneError: error,
                tieneErrorObservacion: errorObservacion
            }
            vm.callback({ validacion: validacion, arg: vm.siguienteDisabled, aprueba: false, titulo: '', ocultarDevolver: ocultarDevolver  });
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
            if (nombreComponente == "sgpgraprobaciondefinitiva")
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
            //vm.notificacionCambiosAsociarProyecto(nombreComponente, nombreComponenteHijo);
            vm.notificacionCambios(nombreComponente, nombreComponenteHijo);
        }

        vm.habilitaFinalizar = function (estado) {
            vm.finalizarProceso = estado;
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
    }

    angular.module('backbone').component('firmaGestionRecursosSgp', {
        templateUrl: "/src/app/formulario/ventanas/SGP/gestionRecursosSGP/componentes/firmaEmisionGestion/firmaGestionRecursosSgp.html",
        controller: firmaGestionRecursosSgpController,
        controllerAs: "vm",
        bindings: {
            callback: '&'
        }
    });
})();