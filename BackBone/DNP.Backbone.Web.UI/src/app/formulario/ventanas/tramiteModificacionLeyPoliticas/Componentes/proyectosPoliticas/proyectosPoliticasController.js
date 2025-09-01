(function () {
    'use strict';

    proyectosPoliticasController.$inject = [
        '$sessionStorage',
        'sesionServicios',
        'utilidades',
        'tramiteVigenciaFuturaServicio',
        'flujoServicios',
        'trasladosServicio',
        'constantesBackbone',
        'justificacionCambiosServicio',
        'aclaracionLeyendaServicio',
        '$scope'
    ];

    function proyectosPoliticasController(
        $sessionStorage,
        sesionServicios,
        utilidades,
        tramiteVigenciaFuturaServicio,
        flujoServicios,
        trasladosServicio,
        constantesBackbone,
        justificacionCambiosServicio,
        aclaracionLeyendaServicio,
        $scope
    ) {
        var vm = this;
        vm.lang = "es";
        //Validaciones
        vm.nombreComponente = 'proyecto';
      
            vm.nombreComponenteHijo = "proyectoasociarproyecto";


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
        //vm.eliminarAsociacion = eliminarAsociacion;

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

        vm.estadoAjusteCreado = $sessionStorage.EstadoAjusteCreado;
        vm.estadoAjusteFinalizado = false;



        vm.notificacionCambiosCapitulos = null;

        vm.handlerComponentes = [
            { id: 1, componente: 'proyectoasociarproyecto', handlerValidacion: null, handlerCambios: null, esValido: true }
          
        ];
        vm.handlerComponentesChecked = {};

        vm.inicaliazarAsociacion = inicaliazarAsociacion;

        function inicaliazarAsociacion() {
            vm.inicializarComponenteCheck();
            vm.notificacioncambios({ handler: vm.notificacionCambiosCapitulos, nombreComponente: vm.nombreComponente });
            vm.notificacionvalidacion({ handler: vm.notificacionValidacionEvent, nombreComponente: vm.nombreComponente, handlerCapitulos: vm.capitulos });

            vm.abrirpanelseleccion();
            aclaracionLeyendaServicio.obtenerProyectos(vm.BPIN).then(function (response) {
                var obj = JSON.parse(response.data);
                $sessionStorage.proyectoId = obj[0].ProyectoId;
            });

            $scope.$watch(function () {
                if (vm.estadoAjusteCreado != $sessionStorage.EstadoAjusteCreado)
                    vm.estadoAjusteCreado = $sessionStorage.EstadoAjusteCreado;
                return $sessionStorage;
            }, function (newVal, oldVal) {
            }, true);
        }



        vm.rotated = false;
        function abrirpanelseleccion() {

            //var acc = document.getElementById('divasociaciarproyecto');
            //var i;
            //var rotated = false;


            //acc.classList.toggle("active");
            //var panel = acc.nextElementSibling;
            //if (panel.style.maxHeight) {
            //    panel.style.maxHeight = null;
            //} else {
            //    panel.style.maxHeight = panel.scrollHeight + "px";
            //}
            //var div = document.getElementById('u4_imgcargaarchivo'),
            //    deg = vm.rotated ? 180 : 0;
            //div.style.webkitTransform = 'rotate(' + deg + 'deg)';
            //div.style.mozTransform = 'rotate(' + deg + 'deg)';
            //div.style.msTransform = 'rotate(' + deg + 'deg)';
            //div.style.oTransform = 'rotate(' + deg + 'deg)';
            //div.style.transform = 'rotate(' + deg + 'deg)';
            //vm.rotated = !vm.rotated;
        }

        function abrirTooltip() {
            utilidades.mensajeInformacion('Esta es la explicación de la carga de archivos... un hecho establecido hace '
                + 'demasiado tiempo que un lector se distraerá con el contenido del texto de '
                + 'un sitio mientras que mira su diseño.El punto de usar Lorem Ipsum es que '
                + 'tiene una distribución más o menos normal de las letras, al contrario de '
                + 'usar textos como por ejemplo "Contenido aquí, contenido aquí".', false, "Carga de archivos")
        }




        /* ------------------------ Validaciones ---------------------------------*/

        /**
      * Listado de componentes hijos, obligatorio para estructura de validación
      * */

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

        vm.guardado = function (nombreComponenteHijo) {
            vm.callback();
            vm.guardadocomponent({ nombreComponente: vm.nombreComponente, nombreComponenteHijo: nombreComponenteHijo });
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
            vm.notificacionestado({ estado: estado, nombreComponente: vm.nombreComponente });

        });

        vm.inicializarComponenteCheck = function () {
            vm.handlerComponentesChecked = {
                'proyectoasociarproyecto': true
            };
        }

        vm.notificacionCambiosCapitulos = function ({ nombreComponente, nombreComponenteHijo }) {
            var x = 0;
            for (var i = 0; i < vm.handlerComponentes.length; i++) {
                if (vm.handlerComponentes[i].handlerCambios != null) {
                    vm.handlerComponentes[i].handlerCambios(nombreComponenteHijo);
                }
            }
        };

        vm.notificacionCambiosCapitulosExternos = function (handler, nombreComponente) {
            for (var i = 0; i < vm.handlerComponentes.length; i++) {
                if (vm.handlerComponentes[i].componente != nombreComponente) {
                    vm.handlerComponentes[i].handlerCambios = handler;
                    break;
                }
            }
        };

        /**
        * Función que recibe listado de errores referentes a la sección de justificación
        * envía a sus hijos el listado de errores
        * @param {any} errores
        */
        vm.notificacionValidacionEvent = function (listErrores) {
            var erroresList = listErrores.errores.filter(p => p.Seccion == vm.nombreComponente);
            vm.inicializarComponenteCheck();
            vm.esValido = true;
            if (erroresList.length > 0) {
                for (var i = 0; i < vm.handlerComponentes.length; i++) {
                    if (vm.handlerComponentes[i].handlerValidacion) vm.handlerComponentes[i].handlerValidacion(erroresList);
                }
            }
        }

        /**
        * Función que crea las referencias de los métodos de los hijos con el padre. Este es llamado cuando se inicializa el componente hijo.
        * @param {any} handler función referenciada
        * @param {any} nombreComponente nombre configurado en cada uno de los componentes hijos vm.nombreComponente = 'datosgenerales'
        */
        vm.notificacionValidacionHijos = function (handler, nombreComponente) {
            var indx = vm.handlerComponentes.findIndex(p => p.componente == nombreComponente);
            vm.handlerComponentes[indx].handlerValidacion = handler;

        };


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

        vm.notificacionEstado = function (nombreComponente, esValido) {
            var indx = vm.handlerComponentes.findIndex(p => p.componente == nombreComponente);
            vm.esValido = !esValido ? false : vm.esValido;
            vm.handlerComponentes[indx].esValido = esValido;
            vm.handlerComponentesChecked[nombreComponente] = esValido;
            vm.showAlertError(nombreComponente, esValido);
        }

        /**
         * Función que visualiza alerta de error tab de componente
         * @param {any} nombreComponente nombre configurado en cada uno de los componentes hijos vm.nombreComponente = 'datosgenerales'
         */
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

        //vm.notificacionValidacionPadre = function (errores) {
        //    //console.log("Validación  - CD Pvigencias futuras");
        //    vm.limpiarErrores(errores);
        //    if (errores != undefined) {
        //        var erroresRelacionconlapl = errores.find(p => (p.Seccion + p.Capitulo) == vm.nombreComponente);
        //        if (erroresRelacionconlapl !== undefined) {
        //            var erroresJson = erroresRelacionconlapl.Errores == "" ? [] : JSON.parse(erroresRelacionconlapl.Errores);
        //            var isValid = (erroresJson == null || erroresJson.length == 0);
        //            if (!isValid) {
        //                erroresJson[vm.nombreComponente].forEach(p => {

        //                    if (vm.errores[p.Error] != undefined) vm.errores[p.Error](p.Descripcion);
        //                });
        //            }
        //        }
        //        vm.notificacionestado({ nombreComponente: vm.nombreComponente, esValido: isValid });
        //    }
        //}


        //vm.limpiarErrores = function (errores) {
        //    var campoObligatorioJustificacion = document.getElementById(vm.nombreComponente + "-proyecto-error");
        //    if (campoObligatorioJustificacion != undefined) {
        //        campoObligatorioJustificacion.innerHTML = "";
        //        campoObligatorioJustificacion.classList.add('hidden');
        //    }


        //}

        //vm.validarValoresVigenciaAsociarproyectoAsociarproyecto = function (errores) {
        //    var campoObligatorioJustificacion = document.getElementById(vm.nombreComponente + "-proyecto-error");
        //    if (campoObligatorioJustificacion != undefined) {
        //        campoObligatorioJustificacion.innerHTML = "<span class='d-inline-block ico-advertencia'></span><span>" + errores + "</span>";
        //        campoObligatorioJustificacion.classList.remove('hidden');
        //    }
        //}



        //vm.errores = {
        //    'AL001': vm.validarValoresVigenciaAsociarproyectoAsociarproyecto,



        //}

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
        }
        function ObtenerSeccionCapituloAsociar() {
            const span = document.getElementById('id-capitulo-asociarproyectoseleccionproyec');
            vm.seccionCapituloAsociarid = span.textContent;
        }

        vm.deshabilitarBotonDevolverSeccionProyecto = function () {
            vm.callback();

        }



    }

    angular.module('backbone').component('proyectosPoliticas', {
        templateUrl: "src/app/formulario/ventanas/tramiteModificacionLeyPoliticas/componentes/proyectosPoliticas/proyectosPoliticas.html",
        controller: proyectosPoliticasController,
        controllerAs: "vm",
        bindings: {
            callback: '&',
            guardadoevent: '&',
            notificacionvalidacion: '&',
            notificacionestado: '&',
            tramiteid: '@',
            tipotramiteid: '@',
            deshabilitarBotonDevolverAsociarProyectoVF: '&',
            guardadocomponent: '&',
            notificacioncambios: '&',
            actualizacomponentes: '=',
            deshabilitar: '@',
            rolanalista: '@',
            modificodatos: '='
        }
    });

 



})();