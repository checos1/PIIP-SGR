(function () {
    'use strict';

    previosSGPController.$inject = [
        '$scope',
        'utilidades',
        '$sessionStorage',
        'previosSGPServicio',
        'justificacionCambiosServicio',
        'utilsValidacionSeccionCapitulosServicio',
        'viabilidadSGPServicio'
    ];

    function previosSGPController(
        $scope,
        utilidades,
        $sessionStorage,
        previosSGPServicio,
        justificacionCambiosServicio,
        utilsValidacionSeccionCapitulosServicio,
        viabilidadSGPServicio
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

        vm.notificarGuardado = notificarGuardado;
        vm.eventoValidar = eventoValidar;
        vm.SeccionIdViabilidad = "";

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
            { id: 1, componente: 'viabilidad', handlerValidacion: null, handlerCambios: null, esValido: true, errores: null },
            { id: 2, componente: 'sgpviabilidadpreviosrecursos', handlerValidacion: null, handlerCambios: null, esValido: true, errores: null },
            { id: 3, componente: 'sgpviabilidadpreviosoperacioncredito', handlerValidacion: null, handlerCambios: null, esValido: true, errores: null },
            { id: 4, componente: 'sgpviabilidadpreviossoporte', handlerValidacion: null, handlerCambios: null, esValido: true, errores: null },
            { id: 5, componente: 'sgpviabilidadpreviosregionalizacion', handlerValidacion: null, handlerCambios: null, esValido: true, errores: null },
            { id: 6, componente: 'sgpviabilidadpreviosfocalizacion', handlerValidacion: null, handlerCambios: null, esValido: true, errores: null },
            { id: 7, componente: 'sgpviabilidadpreviosejecutor', handlerValidacion: null, handlerCambios: null, esValido: true, errores: null },
            { id: 8, componente: 'sgpviabilidadpreviosdatosgenerales', handlerValidacion: null, handlerCambios: null, esValido: true, errores: null },
        ];

        vm.handlerComponentesChecked = {};

        vm.inicializarComponenteCheck = function (estado) {
            vm.siguienteDisabled = true;
            vm.handlerComponentesChecked = {
                'viabilidad': true,
                'sgpviabilidadpreviosrecursos': true,
                'sgpviabilidadpreviosoperacioncredito': true,
                'sgpviabilidadpreviossoporte': true,
                'sgpviabilidadpreviosregionalizacion': true,
                'sgpviabilidadpreviosfocalizacion': true,
                'sgpviabilidadpreviosejecutor': true,
                'sgpviabilidadpreviosdatosgenerales': true,
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
            previosSGPServicio.obtenerErroresviabilidadSGP(vm.guiMacroproceso, vm.ProyectoId, vm.IdNivel, vm.idInstancia).then(function (respuesta) {
                const dataArchivo = {
                    idnivel: vm.IdNivel,
                    idinstancia: vm.idInstancia,
                    idaccion: $sessionStorage.idAccion
                };

                viabilidadSGPServicio.SGP_Viabilidad_ValidarCargueDocumentoObligatorio(dataArchivo, "tramites").then(function (response) {
                    if (respuesta.data) {
                        if (response.data != null) {
                            var seccion = 'sgpviabilidadpreviossoporte';
                            var capitulo = 'alojararchivo';

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
                        vm.notificacionValidacionHijos(respuesta.data);
                        var indexobs = respuesta.data.findIndex(p => p.Capitulo == 'observaciones');
                        var errorObservacion = false;
                        if (indexobs != -1)
                            errorObservacion = respuesta.data[indexobs].Errores == null ? false : true;

                        var findErrors = respuesta.data.findIndex(p => p.Errores != null);
                        vm.visualizarAlerta((findErrors != -1), errorObservacion);
                        error = (findErrors != -1);
                    }
                });
            });
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
    }

    angular.module('backbone').component('previosSGP', {
        templateUrl: "/src/app/formulario/ventanas/SGP/viabilidadSGP/previosSGP.html",
        controller: previosSGPController,
        controllerAs: "vm",
        bindings: {
            callback: '&'
        }
    });
})();