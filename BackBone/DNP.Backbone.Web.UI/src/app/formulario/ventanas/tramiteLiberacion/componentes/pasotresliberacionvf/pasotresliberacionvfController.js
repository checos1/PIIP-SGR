(function () {
    'use strict';

    pasotresliberacionvfController.$inject = [
        '$sessionStorage',
        'sesionServicios',
        'utilidades',
        'tramiteVigenciaFuturaServicio',
        'flujoServicios',
        'trasladosServicio',
        'constantesBackbone',
        'justificacionCambiosServicio',
        'tramiteLiberacionServicio',
        '$scope'
    ];

    function pasotresliberacionvfController(
        $sessionStorage,
        sesionServicios,
        utilidades,
        tramiteVigenciaFuturaServicio,
        flujoServicios,
        trasladosServicio,
        constantesBackbone,
        justificacionCambiosServicio,
        tramiteLiberacionServicio,
        $scope
    ) {
        var vm = this;
        vm.lang = "es";
        vm.tramiteProyectoId = 0;
        vm.entidad = undefined;
        vm.IdEntidadSeleccionada = $sessionStorage.idEntidad;
        vm.sector = undefined;
        vm.BPIN = $sessionStorage.BPIN;
        vm.proyecto = $sessionStorage.nombreProyecto;
        vm.vigenciaInicial = undefined;
        vm.vigenciaFinal = undefined;
        vm.listaValores = [];
        vm.abrirpanelseleccion = abrirpanelseleccion;
        vm.estadoSeleccion = undefined;

        vm.numeroTramite = $sessionStorage.idObjetoNegocio;
        vm.etapa = "ej";
        vm.TramiteId = $sessionStorage.tramiteId;
        vm.usuario = $sessionStorage.usuario.permisos.IdUsuarioDNP;
        vm.instanciaId = $sessionStorage.idInstancia;
        vm.parametros = {
            idFlujo: $sessionStorage.idFlujoIframe,
            tipoEntidad: 'Nacional',
            idInstancia: $sessionStorage.idInstancia,
            IdEntidad: vm.IdEntidadSeleccionada
        };


        //para guardar los capitulos modificados y que se llenen las lunas
        vm.seccionCapituloAsociarId = null;
        vm.seccionsolicitarModifi = null;

        vm.estadoAjusteCreado = false;
        vm.estadoAjusteFinalizado = false;


        //Validaciones
        vm.nombreComponente = "pasotresliberacionvf";

        vm.handlerComponentes = [
            { id: 1, componente: 'pasotresliberacionvfvaloresprolbvf', handlerValidacion: null, handlerCambios: null, esValido: true },
        ];
        vm.handlerComponentesChecked = {};

        vm.init = function () {
            vm.inicializarComponenteCheck();
            vm.notificacioncambios({ handler: vm.notificacionCambiosCapitulos, nombreComponente: vm.nombreComponente });
            vm.notificacionvalidacion({ handler: vm.notificacionValidacionEvent, nombreComponente: vm.nombreComponente, handlerCapitulos: vm.capitulos });

        };

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
            //vm.notificacionestado({ estado: estado, nombreComponente: vm.nombreComponente, esValidoPaso4: omitirErrorPaso4 });
            vm.notificacionestado({ estado: estado, nombreComponente: vm.nombreComponente });
        });

        vm.inicializarComponenteCheck = function () {
            vm.handlerComponentesChecked = {
                'pasotresliberacionvfvaloresprolbvf': true
            };
        }

        vm.inicialiazarAsociacion = inicialiazarAsociacion;

        function inicialiazarAsociacion() {
            //if (vm.listaValores.length == 0)
            //    vm.estadoSeleccion = "Sin asociación";
            vm.notificacionvalidacion({ handler: vm.notificacionValidacionPadre, nombreComponente: vm.nombreComponente, esValido: true });
            vm.abrirpanelseleccion();
            

            $scope.$watch(() => $sessionStorage.EstadoAjusteCreado
                , (newVal, oldVal) => {
                    if (newVal) {
                        vm.estadoAjusteCreado = $sessionStorage.EstadoAjusteCreado;
                        //if (vm.tramiteProyectoId == 0 || vm.tramiteProyectoId == undefined)
                        //    vm.cargarDatos(vm.tramiteid);
                    }
                }, true);

            $scope.$watch(() => vm.tramiteid
                , (newVal, oldVal) => {
                    if (newVal) {
                        if (vm.tramiteProyectoId == 0 || vm.tramiteProyectoId == undefined)
                            vm.cargarDatos(vm.tramiteid);
                    }
                }, true);

        }


        vm.cargarDatos = function (tramiteId) {
            if (tramiteId !== undefined && vm.proyectoId == undefined) {
                tramiteLiberacionServicio.obtenerDatosProyectoTramite(tramiteId).then(
                    function (respuesta) {
                        if (respuesta.data.ProyectoId != 0) {
                            $sessionStorage.proyectoId = respuesta.data.ProyectoId;
                            $sessionStorage.BPIN = respuesta.data.BPIN;
                           
                            vm.proyectoId = $sessionStorage.proyectoId;
                            vm.proyectoAsociado = true;
                            $sessionStorage.EstadoAsociacionVF = 'Con Asociación';
                            $sessionStorage.EstadoAjusteCreado = true;
                            vm.datosproyecto = respuesta.data;
                            var entidad = respuesta.data.EntidadId;
                            var proyecto = respuesta.data.ProyectoId;
                            
                            tramiteLiberacionServicio.obtenerTarmitesPorProyectoEntidad(entidad, proyecto).then(
                                function (respuesta) {
                                    vm.tramiteProyectoId = respuesta.data.find(x => x.TramiteId == vm.tramiteid).ProyectoId;
                                }
                            );
                           
                        }
                    }
                ).then(function () {
                    var listaproyectos = [];
                    listaproyectos.push($sessionStorage.BPIN);
                    trasladosServicio.obtenerInstanciasActivasProyectos(listaproyectos)
                        .then(function (resultado) {
                            var listaqueyatiene = resultado.data;
                            if (listaqueyatiene.length > 0) {
                                $sessionStorage.EstadoAsociacionVF = 'En Verificación';
                                $sessionStorage.EstadoAjusteCreado = true;
                            }
                        }).then(function () {
                            vm.estadoTramite = $sessionStorage.EstadoAsociacionVF;
                            vm.estadoAjusteCreado = $sessionStorage.EstadoAjusteCreado;
                            if (vm.estadoAjusteCreado) {
                                obtenerEstadoInstanciaProyecto();
                            }
                        });

                });
            }
        }

        vm.rotated = false;
        function abrirpanelseleccion() {

           
        }

        function abrirTooltip() {
            utilidades.mensajeInformacion('Esta es la explicación de la carga de archivos... un hecho establecido hace '
                + 'demasiado tiempo que un lector se distraerá con el contenido del texto de '
                + 'un sitio mientras que mira su diseño.El punto de usar Lorem Ipsum es que '
                + 'tiene una distribución más o menos normal de las letras, al contrario de '
                + 'usar textos como por ejemplo "Contenido aquí, contenido aquí".', false, "Carga de archivos")
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


        /* ------------------------ Validaciones ---------------------------------*/

        vm.notificacionValidacionEvent = function (listErrores) {
            var erroresList = listErrores.errores.filter(p => p.Seccion == vm.nombreComponente);
            vm.inicializarComponenteCheck();
            vm.esValido = true;
            for (var i = 0; i < vm.handlerComponentes.length; i++) {
                if (vm.handlerComponentes[i].handlerValidacion) vm.handlerComponentes[i].handlerValidacion(erroresList);
            }
        }

        vm.notificacionValidacionHijos = function (handler, nombreComponente) {
            var indx = vm.handlerComponentes.findIndex(p => p.componente == nombreComponente);
            if (indx > -1) vm.handlerComponentes[indx].handlerValidacion = handler;

        };

        vm.notificacionValidacionPadre = function (errores) {
            debugger;
            vm.limpiarErrores(errores);
            if (errores != undefined) {
                var erroresRelacionconlapl = errores.find(p => (p.Seccion + p.Capitulo) == vm.nombreComponente);
                if (erroresRelacionconlapl !== undefined) {
                    var erroresJson = erroresRelacionconlapl.Errores == "" ? [] : JSON.parse(erroresRelacionconlapl.Errores);
                    var isValid = (erroresJson == null || erroresJson.length == 0);
                    if (!isValid) {
                        erroresJson[vm.nombreComponente].forEach(p => {

                            if (vm.errores[p.Error] != undefined) vm.errores[p.Error](p.Descripcion);
                        });
                    }
                }
                vm.notificacionestado({ nombreComponente: vm.nombreComponente, esValido: isValid });
            }
        }


        vm.limpiarErrores = function (errores) {
            var campoObligatorioJustificacion = document.getElementById(vm.nombreComponente + "-proyecto-error");
            if (campoObligatorioJustificacion != undefined) {
                campoObligatorioJustificacion.innerHTML = "";
                campoObligatorioJustificacion.classList.add('hidden');
            }


        }

        vm.validarValoresVigenciaAsociarproyectoAsociarproyecto = function (errores) {
            var campoObligatorioJustificacion = document.getElementById(vm.nombreComponente + "-proyecto-error");
            if (campoObligatorioJustificacion != undefined) {
                campoObligatorioJustificacion.innerHTML = "<span class='d-inline-block ico-advertencia'></span><span>" + errores + "</span>";
                campoObligatorioJustificacion.classList.remove('hidden');
            }
        }



        vm.errores = {
            'AL001': vm.validarValoresVigenciaAsociarproyectoAsociarproyecto,
        }

        vm.notificacionEstado = function (nombreComponente, esValido) {
            //debugger;
            var indx = vm.handlerComponentes.findIndex(p => p.componente == nombreComponente);
            vm.esValido = !esValido ? false : vm.esValido;
            vm.handlerComponentes[indx].esValido = esValido;
            vm.handlerComponentesChecked[nombreComponente] = esValido;
            vm.showAlertError(nombreComponente, esValido);
        }

        vm.showAlertError = function (nombreComponente, esValido) {
            //debugger;
            var idSpanAlertComponent = document.getElementById("alert-" + nombreComponente);
            if (idSpanAlertComponent != undefined) {
                if (!esValido) {
                    idSpanAlertComponent.classList.add("ico-advertencia");
                } else {
                    idSpanAlertComponent.classList.remove("ico-advertencia");
                }
            }
        }

        //para guardar los capitulos modificados y que se llenen las lunas
        function guardarCapituloModificado(capituloID) {

            var data = {
                //ProyectoId: 0,//$sessionStorage.InstanciaSeleccionada.ProyectoId,
                Justificacion: "",
                SeccionCapituloId: capituloID,
                InstanciaId: $sessionStorage.idInstancia,
                Modificado: 1,
            }
            justificacionCambiosServicio.guardarCambiosFirme(data)
                .then(function (response) {
                    if (response.data.Exito) {
                        vm.guardadoevent({ nombreComponenteHijo: vm.nombreComponente });
                    }
                    else {
                        utilidades.mensajeError(response.data.Mensaje + " Capitulo Modificado");
                    }
                });
        }

        function eliminarCapitulosModificados(capituloID) {
            var data = {
                ProyectoId: 0,
                Justificacion: "",
                SeccionCapituloId: capituloID,
                InstanciaId: $sessionStorage.idInstancia,

            }
            justificacionCambiosServicio.eliminarCapitulosModificados(data)
                .then(function (response) {
                    if (response.data.Exito) {
                        vm.guardadoevent({ nombreComponenteHijo: vm.nombreComponente });
                    }
                    else {
                        utilidades.mensajeError(response.data.Mensaje + " Capitulo Modificado");
                    }
                });
        }
        //para guardar los capitulos modificados y que se llenen las lunas
        function ObtenerSeccionCapitulo() {
            ObtenerSeccionCapituloAsociar();
            ObtenerSeccionCapituloSeleccion();
        }
        function ObtenerSeccionCapituloAsociar() {
            const span = document.getElementById('id-capitulo-asociarproyectoseleccionproyec');
            vm.seccionCapituloAsociarid = span.textContent;
        }
        function ObtenerSeccionCapituloSeleccion() {
            const span = document.getElementById('id-capitulo-seleccionartramite');
            vm.seccionCapituloAsociarid = span.textContent;
        }

        vm.deshabilitarBotonDevolverSeccionProyecto = function () {
            vm.callback();

        }

        vm.capitulos = function (listadoCapitulos) {
            var listadoCapRecursos = listadoCapitulos.filter(p => p.SeccionModificado == vm.nombreComponente)
            listadoCapRecursos.forEach(function (item) {
                var el = document.getElementById("name-capitulo-" + item.nombreComponente);
                var elidSeccionCapitulo = document.getElementById("id-capitulo-" + item.nombreComponente);
                var elAccordion = document.getElementById("accordion-" + item.nombreComponente);
                if (el != undefined && el != null) {
                    el.innerHTML = item.Capitulo;
                }
                if (elAccordion != undefined && elAccordion != null) {
                    elAccordion.classList.remove("hidden");
                }
                if (elidSeccionCapitulo != undefined && elidSeccionCapitulo != null) {
                    elidSeccionCapitulo.innerHTML = item.SeccionCapituloId;
                }
            });
        };

    }

    angular.module('backbone').component('pasotresliberacionvf', {
        templateUrl: "src/app/formulario/ventanas/tramiteLiberacion/componentes/pasotresliberacionvf/pasotresliberacionvf.html",
        controller: pasotresliberacionvfController,
        controllerAs: "vm",
        bindings: {
            callback: '&',
            notificacionvalidacion: '&',
            notificacionestado: '&',
            notificacioncambios: '&',
            guardadocomponent: '&',
            tramiteid: '@',
            tipotramiteid: '@',
            deshabilitarBotonDevolverAsociarProyectoVF: '&',
        }
    });

})();