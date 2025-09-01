(function () {
    'use strict';

    aprobacionconfirmacionController.$inject = [
        '$sessionStorage',
        '$scope',
        'requerimientosTramitesServicio',
        'utilidades',
        'justificacionCambiosServicio',
        'utilsValidacionSeccionCapitulosServicio',

    ];

    function aprobacionconfirmacionController(
        $sessionStorage,
        $scope,
        requerimientosTramitesServicio,
        utilidades,
        justificacionCambiosServicio,
        utilsValidacionSeccionCapitulosServicio
    ) {
        var vm = this;
        vm.lang = "es";
        vm.extension = "";
        vm.filename = "";
        vm.numeroTramite = $sessionStorage.idObjetoNegocio;
        vm.etapa = "ej";
        vm.TramiteId = $sessionStorage.tramiteId;
        vm.usuario = $sessionStorage.usuario.permisos.IdUsuarioDNP;
        vm.instanciaId = $sessionStorage.idInstancia;
        vm.idTipoTramite = $sessionStorage.TipoTramiteId;
        vm.IdNivel = $sessionStorage.idNivel;
        vm.erroresActivos = null;

        vm.nombreComponente = "aprobacionpaso4confirmacionapr";
        vm.notificacionCambiosCapitulos = null;

        vm.disabled = false;
        vm.disabled2 = false;
        vm.Cancelar = Cancelar;
        vm.Editar = Editar;
        vm.Guardar = Guardar;
        vm.Preguntas = [];
        vm.Justificaciones = [];
        vm.erroresActivosArray = [];

        $scope.respuesta = "";
        $scope.justificacion = "";

        vm.handlerComponentes = [
        ];
        vm.handlerComponentesChecked = {};

        $scope.$watch('vm.tramiteid', function () {
            if (vm.tramiteid !== '' && vm.tramiteid !== undefined && vm.tramiteid !== null) {
                obtenerPreguntas();
            }
        });


        vm.init = function () {
           
            vm.inicializarComponenteCheck();
            vm.notificacionvalidacion({ handler: vm.notificacionValidacionPadre, nombreComponente: vm.nombreComponente });
            vm.notificacioncambios({ handler: vm.notificacionCambiosCapitulos, nombreComponente: vm.nombreComponente });
            vm.prueba({ botonDevolver: false, botonSiguiente: true, ocultarDevolver: true });
        };

        function Cancelar() {
            vm.limpiarErrores();
            obtenerPreguntas();
            vm.disabled = false;
            vm.disabled2 = false;
        }

        function Editar() {
            vm.disabled = true;
            vm.disabled2 = true;
        }

        function Guardar() {
            var error = false;
            vm.limpiarErrores();
            if ($scope.respuesta == "" || $scope.respuesta == null || $scope.respuesta == undefined) {
                var descripcionError = 'El campo de respuesta a la pregunta es Obligatorio.'
                vm.validacionTALP4001(descripcionError, '');
                error = true;;
            }

            if ($scope.justificacion == "" || $scope.justificacion == null || $scope.justificacion == undefined) {
                var descripcionError = 'El campo de Justificación es Obligatorio.'
                vm.validacionTALP4002(descripcionError, '');
                error = true;
            }

            if (error)
                return;

            
            angular.forEach(vm.Justificaciones, function (series) {
                series.JustificacionRespuesta = $scope.respuesta;
                series.ObservacionPregunta = $scope.justificacion;
                series.NivelId = vm.IdNivel;
                series.ProyectoId = null;
                series['InstanciaId'] = $sessionStorage.idInstancia;
                
            });
            
            return requerimientosTramitesServicio.guardarRespuestasJustificacion(vm.Justificaciones).then(
                function (response) {
                    if (response.data && (response.statusText === "OK" || response.status === 200)) {

                        if (vm.Justificaciones.filter(p => p.JustificacionRespuesta !== null && p.JustificacionRespuesta !== "").length > 0) {
                            //guardarCapituloModificado();
                        }
                        else {
                            //eliminarCapitulosModificados();
                        }

                        vm.disabled = false;
                        vm.disabled2 = false;

                        vm.callback({ botonDevolver: false, botonSiguiente: false, ocultarDevolver: true});

                        utilidades.mensajeSuccess("Operación realizada con éxito!", false, false, false);

                    } else {
                        swal('', "Error al realizar la operación", 'error');
                    }
                }

            );
        }

        function obtenerPreguntas() {
            return requerimientosTramitesServicio.obtenerPreguntasJustificacion(vm.TramiteId, 0, 0, 0, vm.IdNivel).then(
                function (respuesta) {
                    vm.Preguntas = respuesta.data[0];
                    vm.Justificaciones = respuesta.data;

                    $scope.respuesta = vm.Preguntas.JustificacionRespuesta;
                    $scope.justificacion = vm.Preguntas.ObservacionPregunta;
                });
        }

        function ObtenerSeccionCapitulo() {
            vm.seccionCapitulo = 266;
        }

        function guardarCapituloModificado() {
            ObtenerSeccionCapitulo();
            var data = {
                Justificacion: "",
                SeccionCapituloId: vm.seccionCapitulo,
                InstanciaId: $sessionStorage.idInstancia,
                Modificado: 1,
                cuenta: 1
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

        function eliminarCapitulosModificados() {
            ObtenerSeccionCapitulo();
            var data = {
                Justificacion: "",
                SeccionCapituloId: vm.seccionCapitulo,
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
                    var isValid = (erroresActivos.length <= 0);
                    var omitirErrorPaso4 = false;
                    if (erroresActivos.length > 0) {
                        if (erroresActivos[0].Error == 'TALP4003') {
                            omitirErrorPaso4 = true;
                            isValid = true;
                        }
                    } 
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
            utilidades.mensajeSuccess(mensaje, false, false, false, "Validación Exitosa");
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

    angular.module('backbone').component('aprobacionconfirmacion', {
        templateUrl: "src/app/formulario/ventanas/tramiteLeyenda/componentes/aprobacion/aprobacionconfirmacion/aprobacionconfirmacion.html",
        controller: aprobacionconfirmacionController,
        controllerAs: "vm",
        bindings: {
            prueba: '&',
            guardadoevent: '&',
            notificacioncambios: '&',
            notificacionvalidacion: '&',
            notificacionestado: '&',
            notificarrefresco: '&',
            callback: '&',
            tramiteid: '@',
        }
    });
}) ();