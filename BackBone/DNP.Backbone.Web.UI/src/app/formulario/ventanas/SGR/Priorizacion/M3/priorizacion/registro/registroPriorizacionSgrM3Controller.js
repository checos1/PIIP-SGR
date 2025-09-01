(function () {
    'use strict';
    registroPriorizacionM3SgrController.$inject = [
        'utilidades',
        '$sessionStorage',
        'priorizacionSgrServicio',
        'justificacionCambiosServicio'
    ];

    function registroPriorizacionM3SgrController(
        utilidades,
        $sessionStorage,
        priorizacionSgrServicio,
        justificacionCambiosServicio
    ) {
        var vm = this;
        vm.user = {};
        vm.lang = "es";
        vm.nombreComponente = 'sgrpriorizacionm3priorizacionregistropriorizacion';
        vm.nombreComponenteEstado = "sgrpriorizacionm3priorizacionestadopriorizacion";

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
            vm.disabled = $sessionStorage.soloLectura;
        };

        function ObtenerPriorizionProyectoDetalle(idInstancia) {
            return priorizacionSgrServicio.obtenerPriorizionProyectoDetalleSGR(idInstancia).then(
                function (PriorizionProyectoDetalle) {
                    vm.PriorizionProyectoDetalle = PriorizionProyectoDetalle.data[0];
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
                }
                /* else {
                     ObtenerSeccionCapitulo(vm.nombreComponente);
                     Guardar();
                 }*/
            }

        }
        function validar() {
            var valida = true;

            function validarCampo(valor, elementoId) {
                var elemento = document.getElementById(elementoId);
                var invalido = (valor === null || valor === undefined || valor === "");

                if (elemento) {
                    elemento.classList.toggle('hidden', !invalido);
                }

                return !invalido;
            }

            valida &= validarCampo(vm.PriorizionProyectoDetalle.TieneMetodologiaCierreBrechas, 'PreguntaObligatoria1');
            valida &= validarCampo(vm.PriorizionProyectoDetalle.MagnitudBrecha, 'PreguntaObligatoria2');
            valida &= validarCampo(vm.PriorizionProyectoDetalle.PosicionBrecha, 'PreguntaObligatoria3');
            valida &= validarCampo(vm.PriorizionProyectoDetalle.Priorizado, 'PreguntaObligatoria4');

            return !!valida; // Asegura que sea booleano
        }

        function Guardar() {
            return priorizacionSgrServicio.GuardarPriorizionProyectoDetalleSGR(vm.PriorizionProyectoDetalle)
                .then(response => {
                    if (!response.data || response.statusText !== "OK") {
                        swal('', "Error al realizar la operación", 'error');
                        return;
                    }
                    procesarGuardado();
                    
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

        //vm.validarSeccion = function (tipoError, errores, esValido) {
        //    var campomensajeerror = document.getElementById(tipoError);
        //    if (campomensajeerror != undefined) {
        //        if (!esValido) {
        //            campomensajeerror.innerHTML = "<span class='d-inline-block ico-advertencia'></span><span class='pl-2'>" + errores + "</span>";
        //            campomensajeerror.classList.remove('hidden');
        //        } else {
        //            campomensajeerror.classList.remove("ico-advertencia");
        //        }
        //    }
        //}

        vm.validarSeccion = function (tipoError, errores, esValido) {
            var campo = document.getElementById(tipoError);
            if (!campo) return;

            if (!esValido) {
                campo.innerHTML = `
                    <span class="d-inline-block ico-advertencia"></span>
                    <span class="pl-2">${errores}</span>`;
                campo.classList.remove('hidden');
            } else {
                campo.classList.add('hidden'); // Ocultamos el mensaje si es válido
            }
        };

        vm.limpiarErrores = function () {

            Array.from(document.getElementsByClassName('errorSeccionInformacionGeneralViabilidad')).forEach(el => {
                el.innerHTML = "";
                el.classList.add('hidden');
            });

            document.querySelectorAll('.messagealerttableDNP').forEach(el => {
                let contenedor = el.closest('div');
                if (contenedor) contenedor.classList.add('hidden');
            });

            //['PreguntaObligatoria1', 'PreguntaObligatoria2', 'PreguntaObligatoria3', 'PreguntaObligatoria4', 'CuestionarioConSi'].forEach(id => {
            //    var el = document.getElementById(id);
            //    if (el) el.classList.add('hidden');
            //});
        }
    }


    angular.module('backbone').component('registroPriorizacionM3Sgr', {
        templateUrl: "/src/app/formulario/ventanas/SGR/Priorizacion/M3/priorizacion/registro/registroPriorizacionM3Sgr.html",
        controller: registroPriorizacionM3SgrController,
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