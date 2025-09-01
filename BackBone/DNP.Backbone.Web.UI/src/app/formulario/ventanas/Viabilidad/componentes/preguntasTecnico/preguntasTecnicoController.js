(function () {
    'use strict';
    preguntasTecnicoController.$inject = [
        '$scope',
        '$sessionStorage',
        'viabilidadServicio',
        'utilidades',
    ];

    function preguntasTecnicoController(
        $scope,
        $sessionStorage,
        viabilidadServicio,
        utilidades,
    ) {
        var vm = this;
        vm.lang = "es";
        vm.nombreComponente = "viabilidadtecnico";
        vm.arrowIcoDown = "glyphicon-chevron-down";
        vm.arrowIcoUp = "glyphicon-chevron-up";
        vm.notificacionCambiosCapitulos = null;

        vm.Bpin = $sessionStorage.idObjetoNegocio;
        vm.IdNivel = $sessionStorage.idNivel;
        vm.idInstancia = $sessionStorage.idInstancia;        
        vm.nombreUsuario = $sessionStorage.usuario.permisos.IdUsuarioDNP;
        vm.idFlujo = $sessionStorage.idFlujoIframe;

        vm.cumple = false;
        vm.nocumple = false;
        vm.disabled = false;

        vm.PreguntasPersonalizadas = [];
        vm.LogsInstancias = {};

        vm.Definitivo;
        vm.Rol;
        vm.Fase;
        vm.entidad;
        vm.ContadorRol = 0;        

        vm.enviarLider = enviarLider;

        vm.handlerComponentes = [
            { id: 1, componente: 'viabilidadtecnicoespecificas', handlerValidacion: null, handlerCambios: null, esValido: true },
            { id: 2, componente: 'viabilidadtecnicogenerales', handlerValidacion: null, handlerCambios: null, esValido: true }
        ];

        vm.handlerComponentesChecked = {};        

        vm.obtener = function () {
            obtenerPreguntasPersonalizadas(vm.Bpin, vm.IdNivel, vm.idInstancia, "A");

            vm.inicializarComponenteCheck();
            vm.notificacioncambios({ handler: vm.notificacionCambiosCapitulos, nombreComponente: vm.nombreComponente });
            vm.notificacionvalidacion({ handler: vm.notificacionValidacionEvent, nombreComponente: vm.nombreComponente, handlerCapitulos: vm.capitulos });           
        };

        function obtenerPreguntasPersonalizadas(bPin, nivelId, instanciaId, listaRoles) {
            return viabilidadServicio.obtenerPreguntasPersonalizadas(bPin, nivelId, instanciaId, listaRoles).then(
                function (respuesta) {
                    vm.PreguntasPersonalizadas = respuesta.data;
                    vm.Definitivo = respuesta.data.Definitivo;

                    vm.Rol = $sessionStorage.Rol;
                    vm.Fase = $sessionStorage.Fase;
                    vm.entidad = $sessionStorage.Entidad;
                    vm.ContadorRol = $sessionStorage.ContadorRol;
                    vm.soloLectura = $sessionStorage.soloLectura;

                    if (vm.ContadorRol == 1 && vm.Rol != "Tecnico") {
                        vm.disabled = true;
                    }

                    if (vm.Definitivo === false || vm.Definitivo === true) {
                        vm.disabled = true;
                    }

                    if (vm.soloLectura == true) {
                        vm.disabled = true;
                    }

                    $sessionStorage.disabledTecnico = vm.disabled;
                }
            );
        }

        function enviarLider() {
            vm.PreguntasPersonalizadas.InstanciaId = vm.idInstancia;
            vm.PreguntasPersonalizadas.Definitivo = false;
            vm.PreguntasPersonalizadas.PreguntasEspecificas = null;
            vm.PreguntasPersonalizadas.PreguntasGenerales = null;
            Guardar();

            //vm.LogsInstancias.De = "Técnico";
            //vm.LogsInstancias.A = "Líder";

            //GuardarLog();          

            if (vm.ContadorRol > 1) {
                location.reload();
            }
            else {
                vm.disabled = true;
                location.reload();
            }
        }

        //function GuardarLog() {
        //    vm.LogsInstancias.NombreUsuario = vm.nombreUsuario;
        //    vm.LogsInstancias.FlujoId = vm.idFlujo;
        //    vm.LogsInstancias.InstanciaId = vm.idInstancia;
        //    vm.LogsInstancias.NivelId = vm.IdNivel;
        //    vm.LogsInstancias.Proceso = vm.Fase;
        //    vm.LogsInstancias.Operacion = "";
        //    vm.LogsInstancias.Rol = "Técnico";
        //    vm.LogsInstancias.Entidad = vm.entidad;
        //    vm.LogsInstancias.FechaCreacion = new Date();

        //    return viabilidadServicio.crearLogFlujo(vm.LogsInstancias).then(
        //        function (response) {
        //            if (response.data && (response.statusText === "OK" || response.status === 200)) {
        //                if (response.data.Exito) {
                            
        //                } else {

        //                }

        //            } else {

        //            }
        //        }
        //    );
        //}

        function Guardar() {
            return viabilidadServicio.guardarRespuestas(vm.PreguntasPersonalizadas).then(
                function (response) {
                    if (response.data && (response.statusText === "OK" || response.status === 200)) {
                        if (response.data.Exito) {
                            parent.postMessage("cerrarModal", window.location.origin);
                            utilidades.mensajeSuccess("El formulario ha sido enviado a " + vm.Fase + "!", false, false, false);
                        } else {
                            swal('', response.data.Mensaje, 'warning');
                        }

                    } else {
                        swal('', "Error al realizar la operación", 'error');
                    }
                }
            );
        }

        $scope.$watch('modelo', function () {
            if (vm.modelo != undefined)
                vm.infoArchivo = vm.modelo;
        });

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
                'viabilidadtecnicoespecificas': true,
                'viabilidadtecnicogenerales': true
            };
        }

        vm.notificacionCambiosCapitulos = function ({ nombreComponente, nombreComponenteHijo }) {
            for (var i = 0; i < vm.handlerComponentes.length; i++) {
                if (vm.handlerComponentes[i].handlerCambios != null) {
                    try {
                        vm.handlerComponentes[i].handlerCambios(nombreComponenteHijo);
                    } catch (error) {
                        console.error('¡¡Tiene ERRORES - handlerCambios del componente = ' + vm.handlerComponentes[i].componente + '!!');
                    }
                }
            }
        };

        vm.notificacionCambiosCapitulosExternos = function (handler, nombreComponente) {
            for (var i = 0; i < vm.handlerComponentes.length; i++) {
                if (vm.handlerComponentes[i].componente == nombreComponente) {
                    vm.handlerComponentes[i].handlerCambios = handler;
                    break;
                }
            }
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

        /* --------------------------------- Validaciones ---------------------------*/

        /**
         * Función que recibe listado de errores referentes a la sección de justificación
         * envía a sus hijos el listado de errores
         * @param {any} errores
         */
        vm.notificacionValidacionEvent = function (listErrores) {
            vm.cumple = false;
            vm.nocumple = false;
            var erroresList = listErrores.errores.filter(p => p.Seccion == vm.nombreComponente);
            var erroresCumple = listErrores.errores.filter(p => p.Seccion == '');
            var erroresJson = erroresCumple[0] == "" ? [] : JSON.parse(erroresCumple[0].Errores);
            evaluarCumple(erroresJson);
            vm.inicializarComponenteCheck();
            vm.esValido = true;
            if (erroresList.length > 0) {
                for (var i = 0; i < vm.handlerComponentes.length; i++) {
                    if (vm.handlerComponentes[i].handlerValidacion) vm.handlerComponentes[i].handlerValidacion(erroresList);
                }
            }
        }

        function evaluarCumple(valor) {
            if (valor === null) {
                if(!vm.disabled) vm.cumple = true;
            }
            else {
                vm.nocumple = true;
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

    angular.module('backbone').component('preguntasTecnico', {
        templateUrl: "src/app/formulario/ventanas/viabilidad/componentes/preguntasTecnico/preguntasTecnico.html",
        controller: preguntasTecnicoController,
        controllerAs: "vm",
        bindings: {
            callback: '&',
            notificacionvalidacion: '&',
            notificacionestado: '&',
            notificacioncambios: '&',         
            guardadocomponent: '&'
        }
    });
})();