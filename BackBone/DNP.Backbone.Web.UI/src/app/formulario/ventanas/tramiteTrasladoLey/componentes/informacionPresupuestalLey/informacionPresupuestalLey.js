(function () {
    'use strict';

    informacionPresupuestalLey.$inject = [
        '$sessionStorage',
        'utilidades',
        '$scope',
        'constantesBackbone'

    ];

    function informacionPresupuestalLey(
        $sessionStorage,
        utilidades,
        $scope,
        constantesBackbone
    ) {
        var vm = this;
        vm.lang = "es";
        vm.IdNivel = $sessionStorage.idNivel;
        vm.titulogrilla = vm.titulosinregistros = 'Aún no se han agregado archivos al paso actual';
        vm.totalRegistros = 0;
        vm.mostrarinformacionpresupuestal = false;

        vm.nombreComponente = 'informacionpresupuestal';
        vm.nombreComponenteHijo = "informacionpresupuestalfuentes";
        vm.notificacionCambiosCapitulos = null;


        vm.handlerComponentes = [
            { id: 1, componente: 'informacionpresupuestalfuentes', handlerValidacion: null, handlerCambios: null, esValido: true },
            { id: 2, componente: 'informacionpresupuestalcdp', handlerValidacion: null, handlerCambios: null, esValido: true },

        ];
        vm.handlerComponentesChecked = {};

        vm.inicializarComponenteCheck = function () {
            vm.handlerComponentesChecked = {
                'informacionpresupuestalfuentes': true,
                'informacionpresupuestalcdp': true
            };
        }

        // Watching de los componentes hijos vm.handlerComponentes
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

        //region declarar metodos
        vm.abrirTooltip = abrirTooltip;
        vm.abrirpanel = abrirpanel;
        vm.initInformacionPresupuestal = initInformacionPresupuestal;



        function initInformacionPresupuestal() {
            vm.inicializarComponenteCheck();
            vm.notificacioncambios({ handler: vm.notificacionCambiosCapitulos, nombreComponente: vm.nombreComponente });
            vm.notificacionvalidacion({ handler: vm.notificacionValidacionEvent, nombreComponente: vm.nombreComponente, handlerCapitulos: vm.capitulos });
            if ($sessionStorage.idNivel.toUpperCase() !== constantesBackbone.idNivelSeleccionProyectos && $sessionStorage.idNivel.toUpperCase() !== constantesBackbone.idNivelAprobacionEntidad) {
                vm.mostrarinformacionpresupuestal = true;
            }
        }


        function renderPanel() {
            var acc = document.getElementById('divcargaarchivo');
            var panel = acc.nextElementSibling;
            panel.style.maxHeight = panel.scrollHeight + "px";

        }

        function abrirTooltip() {
            utilidades.mensajeInformacionV('Esta es la explicación de la carga de archivos... un hecho establecido hace '
                + 'demasiado tiempo que un lector se distraerá con el contenido del texto de '
                + 'un sitio mientras que mira su diseño.El punto de usar Lorem Ipsum es que '
                + 'tiene una distribución más o menos normal de las letras, al contrario de '
                + 'usar textos como por ejemplo "Contenido aquí, contenido aquí".', false, "Carga de archivos")
        }

        //#region Metodos

        vm.rotated = false;
        function abrirpanel() {

            var acc = document.getElementById('divcargaarchivo');
            var i;
            var rotated = false;


            acc.classList.toggle("active");
            var panel = acc.nextElementSibling;
            if (panel.style.maxHeight) {
                panel.style.maxHeight = null;
            } else {
                panel.style.maxHeight = panel.scrollHeight + "px";
            }
            var div = document.getElementById('u4_imgcargaarchivo'),
                deg = vm.rotated ? 180 : 0;
            div.style.webkitTransform = 'rotate(' + deg + 'deg)';
            div.style.mozTransform = 'rotate(' + deg + 'deg)';
            div.style.msTransform = 'rotate(' + deg + 'deg)';
            div.style.oTransform = 'rotate(' + deg + 'deg)';
            div.style.transform = 'rotate(' + deg + 'deg)';
            vm.rotated = !vm.rotated;

            //ObtenerSeccionCapitulo();
        }





        //para guardar los capitulos modificados y que se llenen las lunas
        function guardarCapituloModificado() {
            ObtenerSeccionCapitulo();

        }
        function eliminarCapitulosModificados() {
            ObtenerSeccionCapitulo();

        }
        //para guardar los capitulos modificados y que se llenen las lunas
        function ObtenerSeccionCapitulo() {
            const span = document.getElementById('id-capitulo-' + vm.nombrecomponente);
            vm.seccionCapitulo = span.textContent;


        }

        vm.notificacionCambiosCapitulos = function ({ nombreComponente, nombreComponenteHijo }) {
            for (var i = 0; i < vm.handlerComponentes.length; i++) {
                if (vm.handlerComponentes[i].handlerCambios != null) {
                    vm.handlerComponentes[i].handlerCambios(nombreComponenteHijo);
                }
            }
        };



        /* ------------------------ Validaciones ---------------------------------*/

        vm.limpiarErrores = function (errores) {
            var idSpanAlertComponent = document.getElementById("alert-" + vm.nombreComponente);
            if (idSpanAlertComponent != undefined) {
                idSpanAlertComponent.classList.remove("ico-advertencia");
            }

            var campoObligatorioJustificacion = document.getElementById(vm.nombreComponente + "-archivo-error");
            if (campoObligatorioJustificacion != undefined) {
                campoObligatorioJustificacion.innerHTML = "";
                campoObligatorioJustificacion.classList.add('hidden');
            }
        }

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
                    console.log("vm.handlerComponentes[i]", vm.handlerComponentes[i])
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
            if (indx != -1) vm.handlerComponentes[indx].handlerValidacion = handler;
        };

        /**
       * Función que permite visualizar el nombre de los capítulos configurados en BD
       * @param {any} listadoCapitulos
       */
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


        vm.notificacionValidacionPadre = function (errores) {
            vm.limpiarErrores(errores);
            if (errores != undefined) {
                var erroresRelacionconlapl = errores.find(p => (p.Seccion + p.Capitulo) == vm.nombreComponente);
                var isValid = true;
                if (erroresRelacionconlapl != undefined) {
                    var erroresJson = erroresRelacionconlapl.Errores == null ? [] : JSON.parse(erroresRelacionconlapl.Errores);
                    isValid = (erroresJson == null || erroresJson.length == 0);
                    if (!isValid) {
                        erroresJson[vm.nombreComponente].forEach(p => {

                            if (vm.errores[p.Error] != undefined) vm.errores[p.Error](p.Descripcion);
                        });
                        var idSpanAlertComponent = document.getElementById("alert-" + vm.nombreComponente);
                        if (idSpanAlertComponent != undefined) {
                            idSpanAlertComponent.classList.add("ico-advertencia");
                        }
                    }
                }
                vm.notificacionestado({ nombreComponente: vm.nombreComponente, esValido: isValid });

            }
        }

        vm.validarValoresVigenciaInformacionArchivo = function (errores) {
            var campoObligatorioJustificacion = document.getElementById(vm.nombreComponente + "-archivo-error");
            if (campoObligatorioJustificacion != undefined) {
                campoObligatorioJustificacion.innerHTML = "<span class='d-inline-block ico-advertencia'></span><span>" + errores + "</span>";
                campoObligatorioJustificacion.classList.remove('hidden');
            }
        }

        vm.errores = {
            'VFO006': vm.validarValoresVigenciaInformacionArchivo
        }

        /* ------------------------ FIN Validaciones ---------------------------------*/


    }

    angular.module('backbone').component('informacionPresupuestalLey', {
        restrict: 'E',
        transclude: true,
        scope: {
            modelo: '='
        },
        templateUrl: "src/app/formulario/ventanas/tramiteTrasladoLey/componentes/informacionPresupuestalLey/informacionPresupuestalLey.html",
        controller: informacionPresupuestalLey,
        controllerAs: "vm",
        bindings: {
            callback: '&',
            notificacionvalidacion: '&',
            notificacionestado: '&',
            guardadocomponent: '&',
            guardadoevent: '&',
            tipotramiteid: '@',
            tramiteid: '@',
            nivel: '@',
            rol: '@',
            section: '@',
            objetonegocioid: '@',
            notificacioncambios: '&',
            actualizacomponentes: '@',
            rolanalista: '@',
            deshabilitar: '@'

        }
    });

})();
