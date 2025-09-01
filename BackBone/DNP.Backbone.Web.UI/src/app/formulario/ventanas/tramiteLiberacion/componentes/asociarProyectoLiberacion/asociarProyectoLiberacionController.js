(function () {
    'use strict';

    asociarProyectoLiberacionController.$inject = [
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

    function asociarProyectoLiberacionController(
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
        vm.actualizacomponentes = '';

        //Validaciones
        vm.nombreComponente = 'selecionarvigenciafutura';
        vm.nombreComponenteHijo = "selecionarvigenciafuturaseleccionarproyecto";

        vm.notificacionCambiosCapitulos = null;
        vm.handlerComponentes = [
            { id: 1, componente: 'selecionarvigenciafuturaseleccionarproyecto', handlerValidacion: null, handlerCambios: null, esValido: true },
            { id: 2, componente: 'selecionarvigenciafuturaseleccionarvigenciasfuturas', handlerValidacion: null, handlerCambios: null, esValido: true },
            { id: 3, componente: 'selecionarvigenciafuturaautorizacionminhacienda', handlerValidacion: null, handlerCambios: null, esValido: true },
            { id: 4, componente: 'selecionarvigenciafuturavaloresutilizados', handlerValidacion: null, handlerCambios: null, esValido: true },

        ];
        vm.handlerComponentesChecked = {};

        vm.inicialiazarAsociacion = inicialiazarAsociacion;

        vm.inicializarComponenteCheck = function () {
            vm.handlerComponentesChecked = {
                'selecionarvigenciafuturaseleccionarproyecto': true,
                'selecionarvigenciafuturaseleccionarvigenciasfuturas' : true,
                'selecionarvigenciafuturaautorizacionminhacienda': true,
                'selecionarvigenciafuturavaloresutilizados': true
            };
        }

     


        function inicialiazarAsociacion() {
           
            vm.inicializarComponenteCheck();
            vm.notificacioncambios({ handler: vm.notificacionCambiosCapitulos, nombreComponente: vm.nombreComponente });
            vm.notificacionvalidacion({ handler: vm.notificacionValidacionEvent, nombreComponente: vm.nombreComponente, handlerCapitulos: vm.capitulos });

       
            vm.abrirpanelseleccion();
            

            $scope.$watch(() => $sessionStorage.EstadoAjusteCreado
                , (newVal, oldVal) => {
                    if (newVal) {
                        vm.estadoAjusteCreado = $sessionStorage.EstadoAjusteCreado;
                        if (vm.tramiteProyectoId == 0 || vm.tramiteProyectoId == undefined)
                            vm.cargarDatos(vm.tramiteid);
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

        vm.capitulos = function (listadoCapitulos) {
            //debugger;
            var listadoCapRecursos = listadoCapitulos.filter(p => p.SeccionModificado == vm.nombreComponente)
            listadoCapRecursos.forEach(function (item) {
                //debugger;
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

        /*vm.notificacionCambiosCapitulos = function ({ nombreComponente, nombreComponenteHijo }) {
            var x = 0;
            for (var i = 0; i < vm.handlerComponentes.length; i++) {
                if (vm.handlerComponentes[i].handlerCambios != null) {
                    vm.handlerComponentes[i].handlerCambios(nombreComponenteHijo);
                }
            }
        };*/

        vm.notificacionCambiosCapitulosExternos = function (handler, nombreComponente) {
            for (var i = 0; i < vm.handlerComponentes.length; i++) {
                if (vm.handlerComponentes[i].componente != nombreComponente) {
                    vm.handlerComponentes[i].handlerCambios = handler;
                    break;
                }
            }
        };

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

        vm.notificacionValidacionEvent = function (listErrores) {
            //debugger;
            var erroresList = listErrores.errores.filter(p => p.Seccion == vm.nombreComponente);

            vm.inicializarComponenteCheck();
            vm.esValido = true;
            if (erroresList.length > 0) {
                for (var i = 0; i < vm.handlerComponentes.length; i++) {
                    if (vm.handlerComponentes[i].handlerValidacion) vm.handlerComponentes[i].handlerValidacion(erroresList);
                }
            }
        }

        vm.notificacionValidacionHijos = function (handler, nombreComponente) {
            var indx = vm.handlerComponentes.findIndex(p => p.componente == nombreComponente);
            if (indx > -1) vm.handlerComponentes[indx].handlerValidacion = handler;

        };

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

        vm.errores = {
            'AL001': vm.validarValoresVigenciaAsociarproyectoAsociarproyecto,
            'VFO004':  vm.validarValoresVigenciaAsociarproyectoAsociarproyecto,

        }

        vm.guardado = function (nombreComponenteHijo) {
            vm.callback();
            vm.guardadocomponent({ nombreComponente: vm.nombreComponente, nombreComponenteHijo: nombreComponenteHijo });
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
    }

    angular.module('backbone').component('asociarProyectoLiberacion', {
        templateUrl: "src/app/formulario/ventanas/tramiteLiberacion/componentes/asociarProyectoLiberacion/asociarProyectoLiberacion.html",
        controller: asociarProyectoLiberacionController,
        controllerAs: "vm",
        bindings: {
            callback: '&',
            guardadoevent: '&',
            notificacionvalidacion: '&',
            notificacionestado: '&',
            tramiteid: '@',
            tipotramiteid: '@',
            deshabilitarBotonDevolverAsociarProyectoVF: '&',
            notificacioncambios: '&',
        }
    });

})();