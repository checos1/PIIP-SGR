(function () {
    'use strict';

    viabilidadConTramiteSgpController.$inject = [
        '$scope',
        'utilidades',
        '$sessionStorage',
        'viabilidadConTramiteSgpServicio',
        'justificacionCambiosServicio',
        'utilsValidacionSeccionCapitulosServicio',
        'constantesBackbone',
        'transversalSgrServicio'
    ];

    function viabilidadConTramiteSgpController(
        $scope,
        utilidades,
        $sessionStorage,
        viabilidadConTramiteSgpServicio,
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
        vm.IdRol = constantesBackbone.idRolViabilidadDefinitiva;

        vm.notificarGuardado = notificarGuardado;
        vm.eventoValidar = eventoValidar;
        vm.SeccionIdViabilidad = "";
        vm.detalleViable = "";
        vm.textEstadoViable = "";
        vm.accionPendiente = false;
        vm.validaCuestionario = false;

        vm.ConfiguracionEntidades = [{
            ProyectoId: 0,
            FaseId: 0,
            Fase: "",
            AplicaTecnico: ""
        }];

        vm.init = function () {
            vm.tipoConceptoId = "7";
            vm.inicializarComponenteCheck(true);
            vm.callback({ validacion: { evento: vm.eventoValidar }, arg: true, aprueba: false, titulo: '', ocultarDevolver: true });
            vm.accionPendiente = $sessionStorage.listadoAccionesTramite.some(x => x.AccionInstanciaId === null)
            utilsValidacionSeccionCapitulosServicio.getSeccionCapitulo(vm.guiMacroproceso, $sessionStorage.idNivel, $sessionStorage.idFlujoIframe).then(function (respuesta) {
                vm.setCapitulosHijos(respuesta.data);
                var indexobs = respuesta.data.findIndex(p => p.SeccionModificado == 'sgpviabilidad');
                vm.SeccionIdViabilidad = respuesta.data[indexobs].SeccionId;
            });

            transversalSgrServicio.SGR_Transversal_LeerParametro("MensajeProyectoVIBLSGP")
                .then(function (respuestaParametro) {
                    if (respuestaParametro !== undefined) {
                        if (respuestaParametro.data) {
                            vm.textEstadoViable = respuestaParametro.data.Valor;
                        }
                    }                    
                }, function (error) {
                    utilidades.mensajeError(error);
                });


            transversalSgrServicio.SGR_Transversal_LeerParametro("MensajeProyectoViableDetSGP")
                .then(function (respuestaParametro) {
                    if (respuestaParametro !== undefined) {
                        if (respuestaParametro.data) {
                            vm.detalleViable = respuestaParametro.data.Valor;
                        }
                    }
                }, function (error) {
                    utilidades.mensajeError(error);
                });
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
            vm.callback({ validacion: null, arg: true, aprueba: false, titulo: '', ocultarDevolver: true });
        }

        /**
        * Listado de componentes hijos, obligatorio para estructura de validación
        * */
        vm.handlerComponentes = [
            { id: 1, componente: 'sgpviabilidadviabilidadInformacionBasica', handlerValidacion: null, handlerCambios: null, esValido: true, errores: null },
            { id: 2, componente: 'sgpviabilidad', handlerValidacion: null, handlerCambios: null, esValido: true, errores: null },
            { id: 3, componente: 'sgpviabilidadsoportes', handlerValidacion: null, handlerCambios: null, esValido: true, errores: null },

        ];

        vm.handlerComponentesChecked = {};

        vm.inicializarComponenteCheck = function (estado) {
            vm.siguienteDisabled = true;
            vm.handlerComponentesChecked = {
                'sgpviabilidadviabilidadInformacionBasica': true,
                'sgpviabilidad': true,
                'sgpviabilidadsoportes': true
            };
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

        function eventoValidar() {
            vm.inicializarComponenteCheck();
            vm.validaCuestionario = false;

            viabilidadConTramiteSgpServicio.obtenerErroresviabilidadSgp(vm.guiMacroproceso, vm.ProyectoId, vm.IdNivel, vm.idInstancia).then(function (respuesta) {
               
                const dataArchivo = {
                    idnivel: vm.IdNivel,
                    idinstancia: vm.idInstancia,
                    idaccion: $sessionStorage.idAccion
                };

                viabilidadConTramiteSgpServicio.SGPViabilidadValidarCargueDocumentoObligatorio(dataArchivo, "tramites").then(function (response) {
                    if (respuesta.data) {

                        var indexrecursos = respuesta.data.findIndex(p => p.Seccion == 'recursos');
                        if (indexrecursos != -1)
                            respuesta.data = [...respuesta.data.slice(0, indexrecursos), ...respuesta.data.slice(indexrecursos += 1, respuesta.data.lenght)]

                        //Validar respuestas cuestionario viabilidad
                        let objCodigoErrores = ['SGRERRVIAVB', 'SGRERRVIAVB'];

                        objCodigoErrores.some(objCodigo => {
                            respuesta.data.some((objeto, index) => {
                                if (objeto) {
                                    if (objeto.Errores) {
                                        if (objeto.Errores.includes(objCodigo)) {
                                            vm.validaCuestionario = true;
                                            respuesta.data.splice(index, 1);
                                        }
                                    }
                                }
                            });
                        });

                        if (response.data != null) {

                            var seccion = 'sgpviabilidadsoportes';
                            var capitulo = 'alojararchivossintramitesgp';

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

                        var findErrors = respuesta.data.findIndex(p => p.Errores != null && p.Seccion != '');
                        var error = respuesta.data.findIndex(p => p.Errores != null && p.Seccion != '');

                        if (!vm.accionPendiente) {
                            var erroresCumple = respuesta.data.filter(p => p.Errores != null && p.Seccion == '');
                            var erroresJson = erroresCumple.length == 0 ? [] : JSON.parse(erroresCumple[0].Errores);

                            if (error == -1 &&
                                (vm.visualizarCumple || !vm.visualizarCumple)) {
                                vm.cumple = erroresJson.length == 0 ? 'v' : erroresJson.Cumple[0].Error;
                                vm.textEstadoValidacion = erroresJson.length == 0 ? vm.textEstadoViable : erroresJson.Cumple.length > 1 ? erroresJson.Cumple[1].Descripcion : erroresJson.Cumple[0].Descripcion;
                                vm.textDetalleValidacion = erroresJson.length == 0 ? vm.detalleViable : erroresJson.Cumple.length > 1 ? erroresJson.Cumple[1].Detalle : erroresJson.Cumple[0].Detalle;
                                vm.siguienteDisabled = vm.cumple == 'obs';
                                vm.callback({ validacion: null, arg: vm.siguienteDisabled, aprueba: false, titulo: '' });
                                return;
                            }
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

            let ocultarDevolver = true;
            var validacion = {
                tieneError: error,
                tieneErrorObservacion: errorObservacion
            }

            if (error) {
                utilidades.mensajeError("Revise los campos señalados y valide nuevamente.", null, "Hay datos que presentan inconsistencias");
            }
            else {
                if (vm.validaCuestionario) {
                    vm.siguienteDisabled = true;
                    ocultarDevolver = false;
                    utilidades.mensajeSuccess("Usted puede ajustar los campos del formulario para hacer una nueva viabilidad, o devolverlo a verificación de requisitos, o a la MGA.", false, false, false, `Tras la validación, la viabilidad ha sido evaluada  <br /> <span style="color: #A80521">NO FAVORABLE</span>`);
                }
                else {
                    utilidades.mensajeSuccess("Usted puede ajustar los campos del formulario para hacer una nueva viabilidad, enviarlo a firmas y emisión.", false, false, false, `Tras la validación, la viabilidad ha sido evaluada  <br /> <span> VIABLE </span>`);

                    var hijosCorrectos = (error == false);

                    vm.siguienteDisabled = (hijosCorrectos == false);

                    ocultarDevolver = $sessionStorage.listadoAccionesTramite.some(x => x.AccionInstanciaId === null);
                }
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
            var nomAlerta = "alert-" + nombreComponente.toLowerCase();

            var idSpanAlertComponent = document.getElementById(nomAlerta);
            if (idSpanAlertComponent != undefined) {
                if (!esValido) {
                    idSpanAlertComponent.classList.add("ico-advertencia");
                } else {
                    idSpanAlertComponent.classList.remove("ico-advertencia");
                }
            }

            if (nombreComponente === "sgpviabilidadsoportes") {
                var idDevError = document.getElementById("sgpviabilidadsoportesalojararchivo-archivo-error");
                if (idDevError != undefined) {
                    if (esValido) {
                        idDevError.classList.add("hidden");
                    } else {
                        idDevError.classList.remove("hidden");
                    }
                }

                idDevError = document.getElementById("alert-sgpviabilidadsoportesalojararchivo");
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
            vm.callback({ arg: !estado, aprueba: false, titulo: '', ocultarDevolver: true });
        });

        vm.guardadohijos = function (nombreComponente, nombreComponenteHijo) {
            //vm.notificacionCambiosAsociarProyecto(nombreComponente, nombreComponenteHijo);
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

    angular.module('backbone').component('viabilidadConTramiteSgp', {
        templateUrl: "src/app/formulario/ventanas/SGP/ajustesConTramiteProyectoSGP/viabilidadConTramiteSgp.html",
        controller: viabilidadConTramiteSgpController,
        controllerAs: "vm",
        bindings: {
            callback: '&',
            tipoconceptoid: '@'
        }
    });
})();