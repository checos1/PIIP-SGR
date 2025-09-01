(function () {
    'use strict';
    solicitarIntegradoSgrController.$inject = [
        'utilidades',
        '$sessionStorage',
        'solicitarCtusSgrServicio',
        'justificacionCambiosServicio',
    ];

    function solicitarIntegradoSgrController(
        utilidades,
        $sessionStorage,
        solicitarCtusSgrServicio,
        justificacionCambiosServicio,
    ) {
        var vm = this;
        vm.user = {};
        vm.lang = "es";
        vm.nombreComponente = 'sgrsolicitarctusintegradosolicitarintegrado';

        //para guardar los capitulos modificados y que se llenen las lunas
        vm.seccionCapitulo = null;

        vm.Bpin = $sessionStorage.idObjetoNegocio;
        vm.IdNivel = $sessionStorage.idNivel;
        vm.idInstancia = $sessionStorage.idInstancia;
        vm.idAccion = $sessionStorage.idAccion;
        vm.proyectoId = $sessionStorage.proyectoId;
        vm.TipoCtus = 3;
        vm.InstanciaParalela = false;

        vm.disabled = false;
        vm.activar = true;
        vm.desactivar = true;
        vm.data;

        vm.ProyectoCtus = {
            EntidadConcepto: 0,
            SolicitaCtus: '',
            TipoCtus: 3,
            InstanciaParalela: false
        }

        vm.categorias = [];

        vm.init = function () {
            ObtenerProyectoCtus(vm.proyectoId, vm.idInstancia);
            ObtenerEntidadesSolicitarCtus(vm.proyectoId);
            vm.notificacionvalidacion({ handler: vm.notificacionValidacionPadre, nombreComponente: vm.nombreComponente, esValido: true });
            vm.disabled = $sessionStorage.soloLectura;
        };

        function ObtenerProyectoCtus(proyectoId, idInstancia) {
            return solicitarCtusSgrServicio.SGR_Proyectos_LeerProyectoCtus(proyectoId, idInstancia).then(
                function (ProyectoCtus) {
                    vm.ProyectoCtus = ProyectoCtus.data;
                    $sessionStorage.isSolicitarCtus = vm.ProyectoCtus.SolicitaCtus;
                    $sessionStorage.entidadDestinoCtus = vm.ProyectoCtus.NombreEntidadDestino;
                    vm.ProyectoCtus.TipoCtus = vm.TipoCtus;
                    vm.ProyectoCtus.InstanciaParalela = vm.InstanciaParalela;
                }
            );
        }

        function ObtenerEntidadesSolicitarCtus(proyectoId) {
            return solicitarCtusSgrServicio.SGR_Proyectos_LeerEntidadesSolicitarCtus(proyectoId).then(
                function (ListaEntidades) {
                    vm.ListaEntidades = ListaEntidades.data;
                }
            );
        }

        vm.ActivarEditar = function () {
            if (vm.activar == true) {
                $("#EditarG").html("CANCELAR");
                vm.activar = false;
                vm.onChange();
            }
            else {
                utilidades.mensajeWarning("¿Está seguro de continuar?", function funcionContinuar() {
                    OkCancelar();

                    $("#EditarG").html("EDITAR");
                    vm.activar = true;
                    ObtenerProyectoCtus(vm.proyectoId, vm.idInstancia);
                }, function funcionCancelar(reason) {
                    return;
                }, null, null, "Los datos que posiblemente haya diligenciado se perderán");
            }
        }

        vm.solicitaConceptoChange = function () {
            if (vm.ProyectoCtus.SolicitaCtus == 'false') {
                vm.ProyectoCtus.EntidadConcepto = "0";
            }
        }

        vm.onChange = function () {
            vm.nombreEntidadSeleccionada = vm.ListaEntidades.find(x => x.id == vm.ProyectoCtus.EntidadConcepto).NombreEntidad;
        }

        function OkCancelar() {
            setTimeout(function () {
                utilidades.mensajeSuccess("", false, false, false, "La edición ha sido cancelada con éxito.");
            }, 500);
        }

        vm.Guardar = function () {
            if (!validar()) {
                return;
            }
            else {
                if (vm.ProyectoCtus.SolicitaCtus == 'true' || vm.ProyectoCtus.SolicitaCtus == true) {
                    utilidades.mensajeWarning("La solicitud no podrá ser modificada ni eliminada. ¿Está seguro de continuar?", function funcionContinuar() {
                        Guardar();
                    }, function funcionCancelar(reason) {
                        return;
                    }, null, null, `Se creará una solicitud para emitir un concepto integrado a la entidad ${vm.nombreEntidadSeleccionada}.`);
                }
                else {
                    Guardar();
                }
            }

        }

        function validar() {
            var valida = true;
            var PreguntaObligatoria = document.getElementById('PreguntaObligatoria');
            var EntidadObligatoria = document.getElementById('EntidadObligatoria');

            if (vm.ProyectoCtus.SolicitaCtus === null || vm.ProyectoCtus.SolicitaCtus === undefined || vm.ProyectoCtus.SolicitaCtus === "") {
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

            if (vm.ProyectoCtus.SolicitaCtus == 'true' || vm.ProyectoCtus.SolicitaCtus == true) {
                if (vm.ProyectoCtus.EntidadConcepto === '' || vm.ProyectoCtus.EntidadConcepto === null || vm.ProyectoCtus.EntidadConcepto === undefined
                    || vm.ProyectoCtus.EntidadConcepto === 0 || vm.ProyectoCtus.EntidadConcepto == '0') {
                    if (EntidadObligatoria != undefined) {
                        EntidadObligatoria.classList.remove('hidden');
                        valida = false;
                    }
                }
                else {
                    if (EntidadObligatoria != undefined) {
                        EntidadObligatoria.classList.add('hidden');
                    }
                }
            }

            return valida;
        }

        function Guardar() {
            vm.ProyectoCtus.EntidadConcepto = vm.ProyectoCtus.SolicitaCtus == true ? vm.ProyectoCtus.EntidadConcepto : null;
            return solicitarCtusSgrServicio.SGR_Proyectos_GuardarProyectoSolicitarCtus(vm.ProyectoCtus).then(
                function (response) {
                    if (response.data || response.statusText === "OK") {
                        guardarCapituloModificado();
                        utilidades.mensajeSuccess("", false, false, false);
                        vm.limpiarErrores();
                        $("#EditarG").html("EDITAR");
                        vm.activar = true;
                        ObtenerProyectoCtus(vm.proyectoId, vm.idInstancia);
                    } else {
                        swal('', "Error al realizar la operación", 'error');
                    }
                }
            );
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

        //para guardar los capitulos modificados y que se llenen las lunas
        function ObtenerSeccionCapitulo() {
            const span = document.getElementById('id-capitulo-' + vm.nombreComponente);
            vm.seccionCapitulo = span.textContent;
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
                            vm.validarErrores(TipoError, p.Descripcion, false);
                        });
                    }
                }
                vm.notificacionestado({ nombreComponente: vm.nombreComponente, esValido: isValid });
            }
        }

        vm.validarErrores = function (tipoError, errores, esValido) {
            var campomensajeerror = document.getElementById(tipoError);
            if (campomensajeerror != undefined) {
                if (!esValido) {
                    campomensajeerror.innerHTML = "<span class='d-inline-block ico-advertencia'></span><span class='pl-2'>" + errores + "</span>";
                    campomensajeerror.classList.remove('hidden');
                } else {
                    campomensajeerror.classList.remove('ico-advertencia');
                }
            }
        }

        vm.validarSeccion = function (tipoError, seccion, errores, esValido) {
            var campomensajeerror = document.getElementById(tipoError + seccion);
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
            var campo = document.getElementById('SCUSCTUS');
            if (campo != undefined) {
                campo.innerHTML = "";
                campo.classList.add('hidden');
            }
            var campo = document.getElementById('SGRCtusIntSol');
            if (campo != undefined) {
                campo.innerHTML = "";
                campo.classList.add('hidden');
            }
        }
    }


    angular.module('backbone').component('solicitarIntegradoSgr', {
        templateUrl: "/src/app/formulario/ventanas/SGR/ctusIntegrado/Solicitar/SolicitarCtusIntegrado/solicitarIntegradoSgr.html",
        controller: solicitarIntegradoSgrController,
        controllerAs: "vm",
        bindings: {
            callback: '&',
            guardadoevent: '&',
            notificacionvalidacion: '&',
            notificacionestado: '&',
            namecomponent: '<'
        }
    })
        .directive('stringToNumber', function () {
            return {
                require: 'ngModel',
                link: function (scope, element, attrs, ngModel) {
                    ngModel.$parsers.push(function (value) {

                        return '' + value;
                    });
                    ngModel.$formatters.push(function (value) {
                        return parseFloat(value);
                    });
                }
            };
        });;
})();