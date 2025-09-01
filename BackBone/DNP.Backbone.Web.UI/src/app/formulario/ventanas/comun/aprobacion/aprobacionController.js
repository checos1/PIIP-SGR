(function () {
    'use strict';

    aprobacionController.$inject = [
        '$sessionStorage',
        '$scope',
        'requerimientosTramitesServicio',
        'utilidades',
        'justificacionCambiosServicio',
        'constantesBackbone',

    ];

    function aprobacionController(
        $sessionStorage,
        $scope,
        requerimientosTramitesServicio,
        utilidades,
        justificacionCambiosServicio,
        constantesBackbone,
    ) {
        var vm = this;
        vm.lang = "es";
        vm.extension = "";
        vm.filename = "";
        vm.numeroTramite = $sessionStorage.idObjetoNegocio;
        vm.etapa = "ej";
        vm.usuario = $sessionStorage.usuario.permisos.IdUsuarioDNP;
        vm.instanciaId = $sessionStorage.idInstancia;
        vm.IdNivel = $sessionStorage.idNivel;
        vm.erroresActivos = null;
        vm.MostarEntidad = false;
        vm.MostarSupervisor = false;
        vm.habilitaBotones = $sessionStorage.soloLectura ? false : true;

        //vm.nombreComponente = "aprobacionpaso4confirmacionapr";
        vm.notificacionCambiosCapitulos = null;
        vm.hasclain ='tramite:aprobacionEntidad'

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

        $scope.$watch('vm.nombrecomponentepaso', function () {
            if (vm.nombrecomponentepaso !== undefined && vm.nombrecomponentepaso !== '') {
                vm.nombreComponente = vm.nombrecomponentepaso;
                vm.notificacionvalidacion({ handler: vm.notificacionValidacionPadre, nombreComponente: vm.nombreComponente, esValido: true });
              
            }
        });

        $scope.$watch('vm.tramiteid', function () {
            if (vm.tramiteid !== undefined && vm.tramiteid !== '') {
                obtenerPreguntas();

            }
        });

       

        vm.init = function () {
           
            vm.prueba({ botonDevolver: false, botonSiguiente: true, ocultarDevolver: true });
            vm.MostarEntidad = vm.IdNivel.toLowerCase() === constantesBackbone.idNivelAprobacionEntidad.toLowerCase();
            vm.MostarSupervisor = vm.IdNivel.toLowerCase() === constantesBackbone.idNivelRevisionConcepto.toLowerCase();
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
                error = true;
            }

            if ($scope.justificacion == "" || $scope.justificacion == null || $scope.justificacion == undefined) {
                var descripcionError = 'El campo de Justificación es Obligatorio.'
                vm.validacionTALP4002(descripcionError, '');
                error = true;
            }

            if (error)
                return;

            if ($scope.respuesta === "2")
                $sessionStorage.Respuesta = false;
            else {
                $sessionStorage.Respuesta = true;
            }

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
                            
                        }
                        $sessionStorage.GuardarAprobacionEntidadVFExec = true;
                        guardarCapituloModificado();
                        vm.disabled = false;
                        vm.disabled2 = false;
                        utilidades.mensajeSuccess("Operación realizada con éxito!", false, false, false);
                    } else {
                        swal('', "Error al realizar la operación", 'error');
                    }
                }

            );
        }

        function obtenerPreguntas() {
            let idTramite = vm.tramiteid;
            let tipoTramiteId = vm.tipotramiteid;
            let tipoRolId = 0
            let proyectoid = 0
            if (vm.proyectoid != undefined) proyectoid = vm.proyectoid

            return requerimientosTramitesServicio.obtenerPreguntasJustificacion(idTramite, proyectoid, tipoTramiteId, tipoRolId, vm.IdNivel).then(
                function (respuesta) {
                    if (respuesta.data != null) {
                        vm.Preguntas = respuesta.data[0];
                        vm.Copia = respuesta.data[0];
                        vm.Justificaciones = respuesta.data;

                        if (vm.Preguntas.JustificacionRespuesta==="2")
                            $sessionStorage.Respuesta = false;
                        else if (vm.Preguntas.JustificacionRespuesta === "1"){
                            $sessionStorage.Respuesta = true;
                        }
                        $scope.respuesta = vm.Preguntas.JustificacionRespuesta;
          
                        $scope.justificacion = vm.Preguntas.ObservacionPregunta;
                    }
                });
                  
        } 

        function ObtenerSeccionCapitulo() {
            const span = document.getElementById('id-capitulo-' + vm.nombreComponente);
            vm.seccionCapitulo = span.textContent;
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
            var isValid = true;
            if (errores != undefined) {
                var erroresRelacionconlapl = errores.find(p => (p.Seccion + p.Capitulo) == vm.nombreComponente);
                if (erroresRelacionconlapl != undefined) {
                    var erroresJson = erroresRelacionconlapl.Errores == "" ? [] : JSON.parse(erroresRelacionconlapl.Errores);
                    if (erroresJson != undefined) {
                        isValid = (erroresJson == null || erroresJson.length == 0);
                        if (!isValid) {
                            erroresJson[vm.nombreComponente].forEach(p => {

                                if (vm.errores[p.Error] != undefined) vm.errores[p.Error](p.Descripcion);
                            });
                        }
                    }
                }
                vm.notificacionestado({ nombreComponente: vm.nombreComponente, esValido: isValid });
            }
        };

        vm.limpiarErrores = function () {
            var campoObligatorioJustificacion = document.getElementById("respuesta-pregunta-" + vm.nombreComponente);
            var ValidacionFFR1Error = document.getElementById("respuesta-pregunta-mensaje-"+vm.nombreComponente);

            if (campoObligatorioJustificacion != undefined) {
                if (ValidacionFFR1Error != undefined) {
                    ValidacionFFR1Error.innerHTML = '';
                    campoObligatorioJustificacion.classList.add('hidden');
                }
            }

            var campoObligatorioJustificacion2 = document.getElementById("observacion-pregunta-" + vm.nombreComponente);
            var ValidacionFFR1Error2 = document.getElementById("observacion-pregunta-mensaje-" + vm.nombreComponente);

            if (campoObligatorioJustificacion2 != undefined) {
                if (ValidacionFFR1Error2 != undefined) {
                    ValidacionFFR1Error2.innerHTML = '';
                    campoObligatorioJustificacion2.classList.add('hidden');
                }
            }

            
        }

        vm.validacionTALP4001 = function (descripcionError) {

            var campoObligatorioJustificacion = document.getElementById("respuesta-pregunta-" + vm.nombreComponente);
            var ValidacionFFR1Error = document.getElementById("respuesta-pregunta-mensaje-" + vm.nombreComponente);

            if (campoObligatorioJustificacion != undefined) {
                if (ValidacionFFR1Error != undefined) {
                    ValidacionFFR1Error.innerHTML = '<span>' + descripcionError + "</span>";
                    campoObligatorioJustificacion.classList.remove('hidden');
                }
            }


        }


        vm.validacionTALP4002 = function (descripcionError) {
           
            var campoObligatorioJustificacion = document.getElementById("observacion-pregunta-" + vm.nombreComponente);
            var ValidacionFFR1Error = document.getElementById("observacion-pregunta-mensaje-" + vm.nombreComponente);

            if (campoObligatorioJustificacion != undefined) {
                if (ValidacionFFR1Error != undefined) {
                    ValidacionFFR1Error.innerHTML = '<span>' + descripcionError + "</span>";
                    campoObligatorioJustificacion.classList.remove('hidden');
                }
            }

        }


        vm.errores = {
            'TALP4001': vm.validacionTALP4001,
            'TALP4002': vm.validacionTALP4002,
            
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

    angular.module('backbone').component('aprobacion', {
        templateUrl: "src/app/formulario/ventanas/comun/aprobacion/aprobacion.html",
        controller: aprobacionController,
        controllerAs: "vm",
        bindings: {
            prueba: '&',
            guardadoevent: '&',
            notificacioncambios: '&',
            notificacionvalidacion: '&',
            notificacionestado: '&',
            notificarrefresco: '&',
            callback: '&',
            nombrecomponentepaso: '@',
            tramiteid: '@',
            tipotramiteid: '@',
            proyectoid: '@',
        }
    });
})();