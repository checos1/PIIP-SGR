(function () {
    'use strict';
    registroPriorizacionM2SgrController.$inject = [
        'utilidades',
        '$sessionStorage',
        'priorizacionSgrServicio',
        'justificacionCambiosServicio',
        'previosSgrServicio'
    ];

    function registroPriorizacionM2SgrController(
        utilidades,
        $sessionStorage,
        priorizacionSgrServicio,
        justificacionCambiosServicio,
        previosSgrServicio
    ) {
        var vm = this;
        vm.user = {};
        vm.lang = "es";
        vm.nombreComponente = 'sgrpriorizacionm2priorizacionregistropriorizacion';
        vm.nombreComponenteEstado = "sgrpriorizacionm2priorizacionestadopriorizacion";

        //para guardar los capitulos modificados y que se llenen las lunas
        vm.seccionCapitulo = null;
        vm.seccionCapituloEstado = null;

        vm.Bpin = $sessionStorage.idObjetoNegocio;
        vm.IdNivel = $sessionStorage.idNivel;
        vm.idInstancia = $sessionStorage.idInstancia;
        vm.idAccion = $sessionStorage.idAccion;
        vm.proyectoId = $sessionStorage.proyectoId;

        vm.disabled = false;
        vm.activar = true;
        vm.desactivar = true
        vm.data;

        vm.PriorizionProyectoDetalle = {

        }

        vm.init = function () {
            ObtenerPriorizionProyectoDetalle(vm.idInstancia);
            vm.notificacionvalidacion({ handler: vm.notificacionValidacionPadre, nombreComponente: vm.nombreComponente, esValido: true });

            var bPin = vm.Bpin;
            var nivelId = vm.IdNivel;
            var instanciaId = vm.idInstancia;
            var listaRoles = "A";

            obtenerPreguntasPersonalizadas(bPin, nivelId, instanciaId, listaRoles);
            vm.disabled = $sessionStorage.soloLectura;
        };

        function ObtenerPriorizionProyectoDetalle(idInstancia) {
            return priorizacionSgrServicio.obtenerPriorizionProyectoDetalleSGR(idInstancia).then(
                function (PriorizionProyectoDetalle) {
                    vm.PriorizionProyectoDetalle = PriorizionProyectoDetalle.data[0];
                }
            );
        }

        function obtenerPreguntasPersonalizadas(bPin, nivelId, instanciaId, listaRoles) {
            return previosSgrServicio.obtenerPreguntasPersonalizadasComponente(bPin, nivelId, instanciaId, vm.nombreComponente, listaRoles).then(
                function (respuesta) {
                    vm.PreguntasPersonalizadas = respuesta.data;
                    vm.disabled = $sessionStorage.soloLectura;
                    var idPreAnt = 0;
                    var idPrePadAnt = 0;
                    respuesta.data.PreguntasGenerales.forEach(generales => {
                        generales.Preguntas.forEach(preguntas => {
                            if (preguntas.IdPreguntaPadre != 0) {
                                if (idPreAnt == preguntas.IdPreguntaPadre && idPrePadAnt != 0)
                                    preguntas.NivelJer = 2;
                            }
                            idPreAnt = preguntas.IdPregunta;
                            idPrePadAnt = preguntas.IdPreguntaPadre;
                        });
                    });
                }
            );
        }

        vm.ActivarEditar = function () {
            if (vm.activar == true) {
                $("#EditarG").html("CANCELAR");
                vm.activar = false;
            }
            else {
                utilidades.mensajeWarning("¿Está seguro de continuar?", function funcionContinuar() {
                    OkCancelar();

                    $("#EditarG").html("EDITAR");
                    vm.activar = true;
                    ObtenerPriorizionProyectoDetalle(vm.idInstancia);

                    var bPin = vm.Bpin;
                    var nivelId = vm.IdNivel;
                    var instanciaId = vm.idInstancia;
                    var listaRoles = "A";

                    obtenerPreguntasPersonalizadas(bPin, nivelId, instanciaId, listaRoles);
                }, function funcionCancelar(reason) {
                    return;
                }, null, null, "Los datos que posiblemente haya diligenciado se perderán");
            }
        }

        function OkCancelar() {
            setTimeout(function () {
                utilidades.mensajeSuccess("", false, false, false, "La edición ha sido cancelada con éxito.");
                vm.limpiarErrores();
            }, 500);
        }

        vm.Guardar = function () {
            if (!validar()) {
                return;
            }
            else {
                if (vm.PriorizionProyectoDetalle.Priorizado == 'true' || vm.PriorizionProyectoDetalle.Priorizado == true) {
                    utilidades.mensajeWarning(`Por la entidad o instancia ${$sessionStorage.InstanciaSeleccionada.NombreEntidad}. ¿Está seguro de continuar?`, function funcionContinuar() {
                        ObtenerSeccionCapitulo(vm.nombreComponente);
                        Guardar();
                    }, function funcionCancelar(reason) {
                        return;
                    }, null, null, `El proyecto quedará priorizado cuando de clic en finalizar a este flujo.`);
                } else if (vm.PriorizionProyectoDetalle.Priorizado == 'false' || vm.PriorizionProyectoDetalle.Priorizado == false) {
                    utilidades.mensajeWarning(`Por la entidad o instancia ${$sessionStorage.InstanciaSeleccionada.NombreEntidad} , en consecuencia, se archivará. ¿Desea continuar?`, function funcionContinuar() {
                        ObtenerSeccionCapitulo(vm.nombreComponente);
                        Guardar();
                    }, function funcionCancelar(reason) {
                        return;
                    }, null, null, `El proyecto quedará NO Priorizado.`);
                } /*else {
                    ObtenerSeccionCapitulo(vm.nombreComponente);
                    Guardar();
                }*/
            }

        }

        function validar() {
            var valida = true;
            var PreguntaObligatoria = document.getElementById('PreguntaObligatoria');
            var CuestionarioConSi = document.getElementById('CuestionarioConSi');

            if (vm.PriorizionProyectoDetalle.Priorizado === null || vm.PriorizionProyectoDetalle.Priorizado === undefined || vm.PriorizionProyectoDetalle.Priorizado === "") {
                if (PreguntaObligatoria != undefined) {
                    PreguntaObligatoria.classList.remove('hidden');
                }
                valida = false;
            }
            else {
                if (PreguntaObligatoria != undefined) {
                    PreguntaObligatoria.classList.add('hidden');
                }
            }

            if (vm.PriorizionProyectoDetalle.Priorizado == 'true' || vm.PriorizionProyectoDetalle.Priorizado == true) {
                const almenosUnaPreguntaMarcada = vm.PreguntasPersonalizadas.PreguntasGenerales[0].Preguntas
                    .some(pregunta => pregunta.Respuesta === '1');

                if (!almenosUnaPreguntaMarcada) {
                    CuestionarioConSi.classList.remove('hidden');
                    valida = false;
                }
                else {
                    CuestionarioConSi.classList.add('hidden');
                }
            }
            else {
                if (CuestionarioConSi != undefined) {
                    CuestionarioConSi.classList.add('hidden');
                }
            }

            
            return valida;
        }

        function Guardar() {
            return priorizacionSgrServicio.GuardarPriorizionProyectoDetalleSGR(vm.PriorizionProyectoDetalle)
                .then(response => {
                    if (!response.data || response.statusText !== "OK") {
                        swal('', "Error al realizar la operación", 'error');
                        return;
                    }

                    if (vm.PriorizionProyectoDetalle.Priorizado == 'true' || vm.PriorizionProyectoDetalle.Priorizado == true) {

                        vm.PreguntasPersonalizadas.PreguntasGenerales[0].Preguntas =
                            vm.PreguntasPersonalizadas.PreguntasGenerales[0].Preguntas.map(pregunta => ({
                                ...pregunta,
                                Respuesta: pregunta.Respuesta === null ? 2 : pregunta.Respuesta
                            }));
                    }
                    else {
                        vm.PreguntasPersonalizadas.PreguntasGenerales[0].Preguntas =
                            vm.PreguntasPersonalizadas.PreguntasGenerales[0].Preguntas.map(pregunta => ({
                                ...pregunta,
                                Respuesta: null
                            }));
                    }


                    vm.PreguntasPersonalizadas.InstanciaId = vm.idInstancia;
                    return previosSgrServicio.guardarRespuestasCustomSGR(vm.PreguntasPersonalizadas)
                        .then(respuesta => {
                            if (!respuesta.data || respuesta.statusText !== "OK") {
                                swal('', "Error al realizar la operación", 'error');
                            } else  {
                                procesarGuardado();
                            }
                        });

                    
                })
                .catch(error => {
                    swal('', "Error al realizar la operación", 'error');
                });
        }

        function procesarGuardado() {
            guardarCapituloModificado();
            guardarCapituloModificadoEstado();
            utilidades.mensajeSuccess("", false, false, false);

            $("#EditarG").html("EDITAR");
            vm.activar = true;
            vm.limpiarErrores();
            ObtenerPriorizionProyectoDetalle(vm.idInstancia);
            var bPin = vm.Bpin;
            var nivelId = vm.IdNivel;
            var instanciaId = vm.idInstancia;
            var listaRoles = "A";

            obtenerPreguntasPersonalizadas(bPin, nivelId, instanciaId, listaRoles);
        }
        //para guardar los capitulos modificados y que se llenen las lunas
        function guardarCapituloModificado() {
            ObtenerSeccionCapitulo();
            var data = {
                ProyectoId: $sessionStorage.InstanciaSeleccionada.ProyectoId,
                Justificacion: "",
                SeccionCapituloId: vm.seccionCapitulo,
                InstanciaId: vm.idInstancia,
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

        function guardarCapituloModificadoEstado() {
            ObtenerSeccionCapituloEstado();
            var data = {
                ProyectoId: $sessionStorage.InstanciaSeleccionada.ProyectoId,
                Justificacion: "",
                SeccionCapituloId: vm.seccionCapituloEstado,
                InstanciaId: vm.idInstancia,
                Modificado: 1,
                cuenta: 1
            }
            justificacionCambiosServicio.guardarCambiosFirme(data)
                .then(function (response) {
                    if (response.data.Exito) {
                        vm.guardadoevent({ nombreComponenteHijo: vm.nombreComponenteEstado });
                    }
                    else {
                        utilidades.mensajeError(response.data.Mensaje + " Capitulo Modificado");
                    }
                });
        }

        //para guardar los capitulos modificados y que se llenen las lunas
        function ObtenerSeccionCapitulo() {
            const span = document.getElementById('id-capitulo-' + vm.nombreComponente);
            vm.seccionCapitulo = span.textContent;
            vm.PreguntasPersonalizadas.SeccionCapituloId = vm.seccionCapitulo;
        }
        function ObtenerSeccionCapituloEstado() {
            const span = document.getElementById('id-capitulo-' + vm.nombreComponenteEstado);
            vm.seccionCapituloEstado = span.textContent;
        }

        /* ------------------------ Validaciones ---------------------------------*/

        vm.notificacionValidacionPadre = function (errores) {
            vm.limpiarErrores();
            if (errores != undefined) {
                var erroresRelacionconlapl = errores.find(p => (p.Seccion + p.Capitulo) == vm.nombreComponente);
                var isValid = true;
                if (erroresRelacionconlapl != undefined) {
                    var erroresJson = erroresRelacionconlapl.Errores == "" ? [] : JSON.parse(erroresRelacionconlapl.Errores);
                    isValid = (erroresJson == null || erroresJson.length == 0);
                    if (!isValid) {
                        erroresJson[vm.nombreComponente].forEach(p => {
                            var nameArr = p.Error.split('-');
                            var TipoError = nameArr[0].toString();
                            vm.validarSeccion(TipoError,p.Descripcion, false);
                        });
                    }
                }
                vm.notificacionestado({ nombreComponente: vm.nombreComponente, esValido: isValid }); 

            }
        }

        vm.validarSeccion = function (tipoError, errores, esValido) {
            var campomensajeerror = document.getElementById(tipoError);
            if (campomensajeerror != undefined) {
                if (!esValido) {
                    campomensajeerror.innerHTML = "<span class='d-inline-block ico-advertencia'></span><span class='pl-2'>" + errores + "</span>";
                    campomensajeerror.classList.remove('hidden');
                } else {
                    campomensajeerror.classList.remove("ico-advertencia");
                }
            }
        }

        vm.limpiarErrores = function () {
            var errorElements = document.getElementsByClassName('errorSeccionInformacionGeneralViabilidad');
            var testDivs = Array.prototype.filter.call(errorElements, function (errorElement) {
                errorElement.innerHTML = "";
                errorElement.classList.add('hidden');
            });

            var PreguntaObligatoria = document.getElementById('PreguntaObligatoria');
            var CuestionarioConSi = document.getElementById('CuestionarioConSi');

            if (PreguntaObligatoria != undefined) {
                PreguntaObligatoria.classList.add('hidden');
            }

            if (CuestionarioConSi != undefined) {
                CuestionarioConSi.classList.add('hidden');
            }
        }
    }


    angular.module('backbone').component('registroPriorizacionM2Sgr', {
        templateUrl: "/src/app/formulario/ventanas/SGR/Priorizacion/M2/priorizacion/registro/registroPriorizacionM2Sgr.html",
        controller: registroPriorizacionM2SgrController,
        controllerAs: "vm",
        bindings: {
            callback: '&',
            guardadoevent: '&',
            notificacionvalidacion: '&',
            notificacionestado: '&',
            namecomponent: '<'
        }
    })       
})();