(function () {
    'use strict';

    resumenliberacionController.$inject = [
        '$sessionStorage',
        '$scope',
        'resumenliberacionServicio',
        'utilidades',
        'justificacionCambiosServicio',
        'utilsValidacionSeccionCapitulosServicio',
        '$uibModal',

    ];

    function resumenliberacionController(
        $sessionStorage,
        $scope,
        resumenliberacionServicio,
        utilidades,
        justificacionCambiosServicio,
        utilsValidacionSeccionCapitulosServicio,
        $uibModal,
    ) {
        var vm = this;
        vm.lang = "es";
        vm.extension = "";
        vm.filename = "";
        vm.numeroTramite = $sessionStorage.idObjetoNegocio;
        vm.etapa = "ej";
        vm.TramiteId = $sessionStorage.tramiteId;//906
        vm.usuario = $sessionStorage.usuario.permisos.IdUsuarioDNP;
        vm.instanciaId = $sessionStorage.idInstancia;
        vm.idTipoTramite = $sessionStorage.TipoTramiteId;
        vm.IdNivel = $sessionStorage.idNivel;
        vm.erroresActivos = null;
        vm.idProyecto = 0;// $sessionStorage.proyectoId;
        vm.ObjetoVerMas = ObjetoVerMas;
        vm.nombreComponente = "resumenlibvf";
        vm.notificacionCambiosCapitulos = null;
        vm.abrirMensajeInformacion = abrirMensajeInformacion;
        vm.ConvertirNumero = ConvertirNumero;

        vm.handlerComponentes = [
        ];
        vm.handlerComponentesChecked = {};

        $scope.$watch('vm.tramiteid', function () {
            if (vm.tramiteid !== '' && vm.tramiteid !== undefined && vm.tramiteid !== null) {
                ObtenerResumenLiberacionVigenciaFutura();
            }
        });

        $scope.$watch('vm.actualizacomponentes', function () {
            
            if (vm.tramiteid !== '' && vm.tramiteid !== undefined && vm.tramiteid !== null) {
                vm.init();
            }
        });

        vm.init = function () {
            ObtenerResumenLiberacionVigenciaFutura();
            vm.inicializarComponenteCheck();
            vm.notificacionvalidacion({ handler: vm.notificacionValidacionPadre, nombreComponente: vm.nombreComponente });
            vm.notificacioncambios({ handler: vm.notificacionCambiosCapitulos, nombreComponente: vm.nombreComponente });
        };

        function ObtenerResumenLiberacionVigenciaFutura() {
            return resumenliberacionServicio.ObtenerResumenLiberacionVigenciaFutura(vm.idProyecto, vm.tramiteid).then(
                function (respuesta) {
                    //debugger;
                    //console.log(respuesta.data);
                    $scope.datos = respuesta.data[0];
                });
        }

        function ConvertirNumero(numero) {
            return new Intl.NumberFormat('es-co', {
                minimumFractionDigits: 2,
            }).format(numero);
        }

        function ObjetoVerMas(tramite) {
            let modalInstance = $uibModal.open({
                animation: $scope.animationsEnabled,
                templateUrl: 'src/app/formulario/ventanas/ajustes/componentes/datosgenerales/indicadores/modal/objetivosIndicadorModal.html',
                controller: 'objetivosIndicadorModalController',
                controllerAs: "vm",
                size: 'lg',
                openedClass: "entidad-modal-adherencia",
                resolve: {
                    Objetivo: function () {
                        return tramite.Objeto;
                    },
                    IdObjetivo: function () {
                        return '';
                    },
                    Tipo: function () {
                        return 'Objeto';
                    },
                    Titulo: function () {
                        return 'Liberación Vigencias Futuras';
                    }
                },
            });
        }

        function abrirMensajeInformacion() {
            utilidades.mensajeInformacionN("", null, "<span class='anttituhori' > ¿Qué es esto? </span><br /> <span class='tituhori' > Objetivos específicos</span>");
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

        vm.guardado = function (nombreComponenteHijo, deshabilitarRegresar, devolver) {
            vm.callback();
            vm.guardadocomponent({ nombreComponente: vm.nombreComponente, nombreComponenteHijo: nombreComponenteHijo, deshabilitarRegresar: deshabilitarRegresar });

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
            };
        }

        vm.deshabilitarBotonDevolverAsociarProyectoVF = function () {
            vm.callback();

        }

        vm.notificacionValidacionPadre = function (errores) {
            vm.limpiarErrores();
            if (errores != undefined) {
                var listadoErrores = utilsValidacionSeccionCapitulosServicio.getErroresValidados(vm.nombreComponente, errores);
                if (listadoErrores != null && listadoErrores["erroresActivos"] != undefined) {
                    var erroresActivos = listadoErrores["erroresActivos"]
                    erroresActivos.forEach(p => {
                        if (vm.erroresActivos[p.Error] != undefined) vm.erroresActivos[p.Error](p.Descripcion, p.Data);
                    });
                    vm.notificacionestado({ nombreComponente: vm.nombreComponente, esValido: isValid });
                }
            }
        };

        vm.limpiarErrores = function () {
            var campoObligatorioJustificacion = document.getElementById("respuesta-pregunta-paso4");
            var ValidacionFFR1Error = document.getElementById("respuesta-pregunta-mensajePaso4");

            if (campoObligatorioJustificacion != undefined) {
                if (ValidacionFFR1Error != undefined) {
                    ValidacionFFR1Error.innerHTML = '';
                    campoObligatorioJustificacion.classList.add('hidden');
                }
            }

            var campoObligatorioJustificacion2 = document.getElementById("observacion-pregunta-paso4");
            var ValidacionFFR1Error2 = document.getElementById("observacion-pregunta-mensajePaso4");

            if (campoObligatorioJustificacion2 != undefined) {
                if (ValidacionFFR1Error2 != undefined) {
                    ValidacionFFR1Error2.innerHTML = '';
                    campoObligatorioJustificacion2.classList.add('hidden');
                }
            }

            vm.erroresActivosArray = [];
        }

        vm.validacionTALP4001 = function (descripcionError, dataError) {
            var campoObligatorioJustificacion = document.getElementById("respuesta-pregunta-paso4");
            var ValidacionFFR1Error = document.getElementById("respuesta-pregunta-mensajePaso4");

            if (campoObligatorioJustificacion != undefined) {
                if (ValidacionFFR1Error != undefined) {
                    ValidacionFFR1Error.innerHTML = '<span>' + descripcionError + "</span>";
                    campoObligatorioJustificacion.classList.remove('hidden');
                }
            }
        }

        vm.validacionTALP4002 = function (descripcionError, dataError) {
            var campoObligatorioJustificacion = document.getElementById("observacion-pregunta-paso4");
            var ValidacionFFR1Error = document.getElementById("observacion-pregunta-mensajePaso4");

            if (campoObligatorioJustificacion != undefined) {
                if (ValidacionFFR1Error != undefined) {
                    ValidacionFFR1Error.innerHTML = '<span>' + descripcionError + "</span>";
                    campoObligatorioJustificacion.classList.remove('hidden');
                }
            }
        }

        vm.validacionTALP4003 = function (descripcionError, dataError) {
            var mensaje = "De acuerdo a la selección realizada en el capítulo \"Confirmación de aprobación\", usted puede devolver el formulario al paso 1  \"Creación del trámite\" mediante el botón \"Devolver\"";
            utilidades.mensajeSuccess(mensaje, false, false, false, "Validación realizada satisfactoriamente. Para continuar al siguiente paso, de clic en siguiente.");
        }


        vm.erroresActivos = {
            'TALP4001': vm.validacionTALP4001,
            'TALP4002': vm.validacionTALP4002,
            'TALP4003': vm.validacionTALP4003
        }

        /* --------------------------------- Validaciones ---------------------------*/

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
            //vm.showAlertError(nombreComponente, esValido, esValidoPaso4);
            vm.showAlertError(nombreComponente, esValido);
        }

        /**
         * Función que visualiza alerta de error tab de componente
         * @param {any} nombreComponente nombre configurado en cada uno de los componentes hijos vm.nombreComponente = 'datosgenerales'
         */
        //vm.showAlertError = function (nombreComponente, esValido, esValidoPaso4) {
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

        /*------------------------------------Fin Validaciones-----------------------------------*/

    }

    angular.module('backbone').component('resumenliberacion', {
        templateUrl: "src/app/formulario/ventanas/tramiteLiberacion/componentes/resumenliberacion/resumenliberacion.html",
        controller: resumenliberacionController,
        controllerAs: "vm",
        bindings: {
            guardadoevent: '&',
            notificacioncambios: '&',
            notificacionvalidacion: '&',
            notificacionestado: '&',
            notificarrefresco: '&',
            callback: '&',
            tipotramiteid: '@',
            tramiteid: '@',
            nivel: '@',
            rol: '@',
            section: '@',
            actualizacomponentes: '@'
        }
    });
})();