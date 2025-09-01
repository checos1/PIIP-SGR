(function () {
    'use strict';

    asociarProyectoAdicionSgpController.$inject = [
        '$sessionStorage',
        'utilidades',
        'constantesBackbone',
        '$scope'
    ];

    function asociarProyectoAdicionSgpController(
        $sessionStorage,
        utilidades,
        constantesBackbone,
        $scope
    ) {
        var vm = this;
        vm.lang = "es";
        vm.nombreComponente = 'proyectostramitesgp'
        vm.nombreComponenteHijo = "proyectostramitesgpasociarproyectotramitesgp";
        //if ($sessionStorage.idNivel.toUpperCase() === constantesBackbone.idNivelSeleccionProyectos)
        //    vm.nombreComponenteHijo = "proyectostramitesgpasociarproyectotramitesgp";
        //else
        //    vm.nombreComponenteHijo = "informacionpresupuestalsgpproyectosasociadossgp";

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
        vm.arrowIcoDown = "glyphicon-chevron-down";
        vm.arrowIcoUp = "glyphicon-chevron-up";

        //para guardar los capitulos modificados y que se llenen las lunas
        vm.seccionCapituloAsociarId = null;
        vm.seccionsolicitarModifi = null;
        vm.estadoAjusteCreado = $sessionStorage.EstadoAjusteCreado;
        vm.estadoAjusteFinalizado = false;
        vm.notificacionCambiosCapitulos = null;

        vm.handlerComponentes = [
            { id: 1, componente: 'proyectostramitesgpasociarproyectotramitesgp', handlerValidacion: null, handlerCambios: null, esValido: true },
            { id: 2, componente: 'proyectostramitesgpdatosadiciontramitesgp', handlerValidacion: null, handlerCambios: null, esValido: true },
            /*{ id: 2, componente: 'informacionpresupuestalsgpproyectosasociadossgp', handlerValidacion: null, handlerCambios: null, esValido: true }*/
        ];
        vm.handlerComponentesChecked = {};

        //Validaciones
        //vm.nombreComponente = "proyectoasociarproyecto";

        vm.inicialiazarAsociacion = inicialiazarAsociacion;

        function inicialiazarAsociacion() {
            vm.inicializarComponenteCheck();
            vm.notificacioncambios({ handler: vm.notificacionCambiosCapitulos, nombreComponente: vm.nombreComponente });
            vm.notificacionvalidacion({ handler: vm.notificacionValidacionEvent, nombreComponente: vm.nombreComponente, handlerCapitulos: vm.capitulos });

            vm.abrirpanelseleccion();


            $scope.$watch(() => vm.tramiteid
                , (newVal, oldVal) => {
                    if (newVal) {
                    }
                }, true);

        }

        vm.guardado = function (nombreComponenteHijo) {
            vm.callback();
            vm.guardadocomponent({ nombreComponente: vm.nombreComponente, nombreComponenteHijo: nombreComponenteHijo });
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

        /*------------------------------------Validaciones-----------------------------------*/
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
                'proyectostramitesgpasociarproyectotramitesgp': true,
                'proyectostramitesgpdatosadiciontramitesgp': true
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

        /**
        * Función que recibe los estados de los componentes hijos
        * @param {any} esValido true: valido, false: invalido
        * @param {any} nombreComponente nombre configurado en cada uno de los componentes hijos vm.nombreComponente = 'datosgenerales'
        */
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
    }

    angular.module('backbone').component('asociarProyectoAdicionSgp', {
        templateUrl: "src/app/formulario/ventanas/SGP/tramites/adicion/componentes/asociarProyectoAdicionSgp/asociarProyectoAdicionSgp.html",
        controller: asociarProyectoAdicionSgpController,
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
        }
    });

})();