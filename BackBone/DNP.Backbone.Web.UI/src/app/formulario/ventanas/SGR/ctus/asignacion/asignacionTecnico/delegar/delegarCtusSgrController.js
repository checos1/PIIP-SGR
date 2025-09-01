(function () {
    'use strict';
    delegarCtusSgrController.$inject = [
        '$scope',
        '$location',
        'utilidades',
        '$sessionStorage',
        'justificacionCambiosServicio',
        'ctusAsignacionSgrServicio',
        'transversalSgrServicio',
        'solicitarCtusSgrServicio'
    ];

    function delegarCtusSgrController( 
        $scope,
        $location,
        utilidades,
        $sessionStorage,
        justificacionCambiosServicio,
        ctusAsignacionSgrServicio,
        transversalSgrServicio,
        solicitarCtusSgrServicio
    ) {
        var vm = this;
        vm.user = {};
        vm.lang = "es";
        vm.nombreComponente = 'sgrctusasignaciondelegar';
        vm.seccionCapitulo = null; //para guardar los capitulos modificados y que se llenen las lunas
        vm.tipoVisualizacionDelegar = '';
        vm.Bpin = $sessionStorage.idObjetoNegocio;
        vm.IdNivel = $sessionStorage.idNivel;
        vm.idInstancia = $sessionStorage.idInstancia;
        vm.idAccion = $sessionStorage.idAccion;
        vm.proyectoId = $sessionStorage.proyectoId;
        vm.idInstanciaPadre = $sessionStorage.InstanciaSeleccionada != undefined ? $sessionStorage.InstanciaSeleccionada.InstanciaPadreId : null;
        vm.estadoBotonEdit = 'EDITAR';
        vm.nombreEntidad;
        vm.fechaCreacion;
        vm.asignacionTecnico = {
            entidadSele: 0,
            delegaViabilidad: ''
        }

        vm.disabled = false;
        vm.activar = true;
        vm.desactivar = true;
        vm.redirectIndexPage = redirectIndexPage;
        vm.redirectConsolaProcesos = redirectConsolaProcesos;

        vm.init = function () {
            ValidarRolCtus(vm.proyectoId, vm.idInstanciaPadre);
            vm.notificacionvalidacion({ handler: vm.notificacionValidacionPadre, nombreComponente: vm.nombreComponente, esValido: true });
            vm.disabled = $sessionStorage.soloLectura;
        };

        ctusAsignacionSgrServicio.registrarObservador(function (datos) {
            if (datos.usuarioDelegado === 'true') {
                vm.disabled = true;
                vm.asignacionTecnico.delegaViabilidad = false;
            } else if (datos.usuarioDelegado === 'false')
                vm.disabled = false;
        });

        vm.delegarConceptoENChange = function () {
            if (vm.asignacionTecnico.delegaViabilidad == false) {
                vm.asignacionTecnico.entidadConcepto = 0;
                ctusAsignacionSgrServicio.notificarCambio({ reqEntidad: false });
            } else {
                ctusAsignacionSgrServicio.notificarCambio({ reqEntidad: true });
            }
        }

        function redirectIndexPage() {
            $location.url("/proyectos/pl");
        }

        function redirectConsolaProcesos() {
            $location.url("/consolaprocesos/index");
        }

        vm.onChange = function () {
            if (vm.asignacionTecnico.entidadSele != undefined)
                vm.nombreEntidadSeleccionada = vm.ListaEntidades.find(x => x.Id == vm.asignacionTecnico.entidadSele).NombreEntidad;
        }

        function ValidarRolCtus(proyectoId, instanciaId) {
            solicitarCtusSgrServicio.SGR_Proyectos_LeerProyectoCtus(proyectoId, instanciaId)
                .then(function (response) {
                    if (response.data != null)
                        ValidarEntidadAdscrita(response.data.RolDirectorId);
                }, function () {
                    utilidades.mensajeError('Ocurrió un problema al leer la información del CTUS.');
                    return;
                });
        }

        function ValidarEntidadAdscrita(rolDirectorId) {
            ctusAsignacionSgrServicio.SGR_Proyectos_ValidarEntidadDelegada(vm.proyectoId, 'ctus')
                .then(function (response) {
                    if (response.data.Exito) {
                        vm.tipoVisualizacionDelegar = 'tipo1';
                        ctusAsignacionSgrServicio.notificarCambio({ tipoVisualizacion: 'tipo2' });
                        var entidadDelegada = JSON.parse(response.data.Mensaje)[0];
                        vm.nombreEntidad = entidadDelegada.NombreEntidad;
                        vm.fechaCreacion = new Date(entidadDelegada.FechaCreacion).toLocaleDateString();
                        ctusAsignacionSgrServicio.notificarCambio({ reqEntidad: false });
                    }
                    else {
                        vm.tipoVisualizacionDelegar = 'tipo2';
                        validarRolAsignacion(rolDirectorId);
                    }
                }, function () {
                    utilidades.mensajeError('Ocurrió un problema al validar la entidad adscrita.');
                });
        }

        function validarRolAsignacion(rolProyectoCtus) {
            transversalSgrServicio.SGR_Transversal_LeerParametro("RolEntidadNacionalAsignacionSGR")
                .then(function (respuestaParametro) {
                    if (respuestaParametro.data != null)
                        validarRolVinculado(respuestaParametro.data.Valor, rolProyectoCtus, vm.proyectoId);
                }, function () {
                    utilidades.mensajeError('Ocurrió un problema al obtener los parametros.');
                    return;
                });
        }

        function validarRolVinculado(rolAsignacionEN, rolProyectoCtus, proyectoId) {
            if (rolAsignacionEN != undefined && rolProyectoCtus != undefined && rolProyectoCtus != null) {
                if (rolAsignacionEN.toLowerCase() === rolProyectoCtus.toLowerCase()) {
                    ObtenerEntidadesAdscritas(proyectoId);
                }
                else {
                    vm.tipoVisualizacionDelegar = 'tipo3';
                    ctusAsignacionSgrServicio.notificarCambio({ tipoVisualizacion: 'tipo2' });
                }
            }
            else {
                utilidades.mensajeError('Ocurrió un problema al verificar el rol.');
                vm.tipoVisualizacionDelegar = 'tipo3';
                ctusAsignacionSgrServicio.notificarCambio({ tipoVisualizacion: 'tipo2' });
            }
        }

        function ObtenerEntidadesAdscritas(proyectoId) {
            return ctusAsignacionSgrServicio.SGR_Proyectos_LeerEntidadesAdscritas(proyectoId, 'instancia')
                .then(function (response) {
                    if (response.data.length == 1) {
                        var entidadDel = response.data[0];
                        if (entidadDel.Id == 0) {
                            vm.nombreEntidad = entidadDel.NombreEntidad;
                            vm.tipoVisualizacionDelegar = 'tipo1';
                            ctusAsignacionSgrServicio.notificarCambio({ tipoVisualizacion: 'tipo2' });
                            ctusAsignacionSgrServicio.notificarCambio({ reqEntidad: false });
                        }
                        else
                            vm.ListaEntidades = response.data;
                    }
                    else
                        vm.ListaEntidades = response.data;
                }, function () {
                    utilidades.mensajeError('Ocurrió un problema al leer las entidades adscritas.');
                    vm.ListaEntidades = "";
                });
        }

        vm.ActivarEditar = function () {
            if (vm.activar == true) {
                vm.estadoBotonEdit = 'CANCELAR';
                vm.activar = false;
            }
            else {
                utilidades.mensajeWarning("¿Está seguro de continuar?", function funcionContinuar() {
                    vm.asignacionTecnico.delegaViabilidad = '';
                    vm.asignacionTecnico.entidadSele = 0;
                    ctusAsignacionSgrServicio.notificarCambio({ tipoVisualizacion: 'tipo1' });
                    OkCancelar();
                    vm.estadoBotonEdit = 'EDITAR';
                    vm.activar = true;
                }, function funcionCancelar() {
                    return;
                }, null, null, "Los datos que posiblemente haya diligenciado se perderán.");
            }
        }

        function OkCancelar() {
            setTimeout(function () {
                utilidades.mensajeSuccess("", false, false, false, "La edición ha sido cancelada con éxito.");
            }, 500);
        }

        vm.guardar = function () {
            if (vm.asignacionTecnico.delegaViabilidad == true) {
                if (vm.asignacionTecnico.entidadSele == 0 || vm.asignacionTecnico.entidadSele == undefined) {
                    utilidades.mensajeError('El campo de entidad adscrita es obligatorio.', false);
                    return;
                }
            }

            if (vm.asignacionTecnico.delegaViabilidad === '')
                ctusAsignacionSgrServicio.notificarCambio({ tipoVisualizacion: 'tipo1' });
            else if (vm.asignacionTecnico.delegaViabilidad === false) {
                vm.asignacionTecnico.entidadSele = 0;
                ctusAsignacionSgrServicio.notificarCambio({ tipoVisualizacion: 'tipo2' });
                vm.estadoBotonEdit = 'EDITAR';
                vm.activar = true;
            }
            else if (vm.asignacionTecnico.delegaViabilidad === true) {
                ctusAsignacionSgrServicio.notificarCambio({ tipoVisualizacion: 'tipo1' });
                utilidades.mensajeWarning("¿Está seguro de continuar?", function funcionContinuar() {
                    Guardar(vm.proyectoId, vm.asignacionTecnico.entidadSele);
                }, function funcionCancelar() {
                    return;
                }, null, null, "Esta operación es irreversible.");
            }
        }

        function Guardar(proyectoId, entityId) {
            return ctusAsignacionSgrServicio.SGR_Proyectos_ActualizarEntidadAdscritaCTUS(proyectoId, entityId, 'ctus')
                .then(function (response) {
                    if (response) {
                        ctusAsignacionSgrServicio.notificarCambio({ actualizarCapitulo: 'true' });
                        guardarCapituloModificado();
                        vm.estadoBotonEdit = 'EDITAR';
                        vm.activar = true;
                        utilidades.mensajeSuccessRedirect(`Se ha delegado la elaboración del CTUS a la entidad ${vm.nombreEntidadSeleccionada}.`,
                            vm.redirectConsolaProcesos,
                            vm.redirectIndexPage,
                            null,
                            "IR CONSOLA DE PROCESOS",
                            "IR A MIS PROCESOS"
                        );
                        return;
                    } else
                        utilidades.mensajeError("Error al realizar la operación.", false);
                }, function () {
                    utilidades.mensajeError('Ocurrió un problema al guardar la entidad adscrita.');
                });
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
            //Remplazar por cada capitulo
            var tipError = 'DELECTUS';
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
                            if (TipoError == tipError) {
                                vm.validarSeccion(TipoError, nameArr[1].toString(), p.Descripcion, false);
                            }
                            else {
                                vm.validarValores(nameArr[0].toString(), p.Descripcion, false);
                            }
                        });
                    }
                } else {
                    var idSpanAlertComponentAlert = document.getElementById("alert-" + vm.nombreComponente);
                    var idSpanAlertComponent = document.getElementById(tipError + vm.nombreComponente)
                    if (idSpanAlertComponent != null)
                        idSpanAlertComponent.classList.add('hidden');

                    if (idSpanAlertComponentAlert != null)
                        idSpanAlertComponentAlert.classList.remove("ico-advertencia");
                }
                vm.notificacionestado({ nombreComponente: vm.nombreComponente, esValido: isValid });
            }
        }

        vm.validarValores = function (pregunta, errores, esValido) {
            var idSpanAlertComponent = document.getElementById("alert-" + vm.nombreComponente + pregunta);
            if (idSpanAlertComponent != undefined) {
                if (!esValido) {
                    idSpanAlertComponent.classList.add("ico-advertencia");
                } else {
                    idSpanAlertComponent.classList.remove("ico-advertencia");
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
            var errorElements = document.getElementsByClassName('errorSeccionInformacionGeneralViabilidad');
            var testDivs = Array.prototype.filter.call(errorElements, function (errorElement) {
                errorElement.innerHTML = "";
                errorElement.classList.add('hidden');
            });
        }
    }

    angular.module('backbone').component('delegarCtusSgr', {
        templateUrl: "/src/app/formulario/ventanas/SGR/ctus/asignacion/asignacionTecnico/delegar/delegarCtusSgr.html",
        controller: delegarCtusSgrController,
        controllerAs: "vm",
        bindings: {
            guardadoevent: '&',
            notificacionestado: '&',
            notificacionvalidacion: '&'
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