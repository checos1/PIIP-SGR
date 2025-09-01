(function () {
    'use strict';
    delegarViabilidadNacionalSgrController.$inject = [
        '$location',
        'utilidades',
        '$sessionStorage',
        'justificacionCambiosServicio',
        'asignacionEntidadNacionalSgrServicio'
    ];

    function delegarViabilidadNacionalSgrController(
        $location,
        utilidades,
        $sessionStorage,
        justificacionCambiosServicio,
        asignacionEntidadNacionalSgrServicio
    ) {
        var vm = this;
        vm.user = {};
        vm.lang = "es";
        vm.nombreComponente = 'sgrentidadnacionalasignaciondelegarviabilidad';
        vm.seccionCapitulo = null; //para guardar los capitulos modificados y que se llenen las lunas
        vm.tipoVisualizacionDelegar = '';
        vm.Bpin = $sessionStorage.idObjetoNegocio;
        vm.IdNivel = $sessionStorage.idNivel;
        vm.idInstancia = $sessionStorage.idInstancia;
        vm.idAccion = $sessionStorage.idAccion;
        vm.proyectoId = $sessionStorage.proyectoId;
        vm.usuario = $sessionStorage.usuario.permisos.IdUsuarioDNP;
        vm.idEntidadAdscrita = $sessionStorage.EntidadAdscritaId;
        //vm.parametrosAcceso = $filter('filter')($filter('filter')($sessionStorage.listadoAccionesTramite, { Id: vm.idAccion })[0].Usuarios, { Usuario: vm.usuario });
        //vm.idEntidadAcceso = vm.parametrosAcceso.length != 0 ? vm.parametrosAcceso[0].IdEntidad : 0;
        vm.idInstanciaPadre = $sessionStorage.InstanciaSeleccionada != undefined ? $sessionStorage.InstanciaSeleccionada.InstanciaPadreId : null;
        vm.estadoBotonEdit = 'EDITAR';
        vm.delegado = false;
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
            vm.notificacionvalidacion({ handler: vm.notificacionValidacionPadre, nombreComponente: vm.nombreComponente, esValido: true });
            ValidarEntidadAdscrita(vm.proyectoId);
            vm.disabled = $sessionStorage.soloLectura;
        };

        // Registra un observador para manejar eventos de comunicación      
        // NOTA: controlar cada parámetro.
        asignacionEntidadNacionalSgrServicio.registrarObservador(function (datos) {
            // Verifica si se debe actualizar el capítulo
            if (datos.usuarioDelegado === 'true') {
                vm.disabled = true;
                vm.asignacionTecnico.delegaViabilidad = false;
            } else if (datos.usuarioDelegado === 'false')
                vm.disabled = false;
        });

        vm.delegarConceptoENChange = function () {
            if (vm.asignacionTecnico.delegaViabilidad == false) {
                vm.asignacionTecnico.entidadConcepto = 0;
                asignacionEntidadNacionalSgrServicio.notificarCambio({ reqEntidad: false });
            } else {
                asignacionEntidadNacionalSgrServicio.notificarCambio({ reqEntidad: true });
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

        function ValidarEntidadAdscrita(proyectoId) {
            asignacionEntidadNacionalSgrServicio.SGR_Proyectos_ValidarEntidadDelegada(proyectoId, 'nacional')
                .then(function (response) {
                    if (response.data.Exito) {
                        vm.tipoVisualizacionDelegar = 'tipo1';
                        asignacionEntidadNacionalSgrServicio.notificarCambio({ tipoVisualizacion: 'tipo2' });
                        var entidadDelegada = JSON.parse(response.data.Mensaje)[0];
                        vm.nombreEntidad = entidadDelegada.NombreEntidad;
                        vm.fechaCreacion = new Date(entidadDelegada.FechaCreacion).toLocaleDateString();
                        asignacionEntidadNacionalSgrServicio.notificarCambio({ reqEntidad: false });
                    } else {
                        vm.tipoVisualizacionDelegar = 'tipo2';
                        ObtenerEntidadesAdscritas(proyectoId);
                    }
                })
                .catch(function (error) {
                    utilidades.mensajeError('Ocurrió un problema al validar la entidad adscrita. Error: ' + error);
                });
        }

        function ObtenerEntidadesAdscritas(proyectoId) {
            return asignacionEntidadNacionalSgrServicio.SGR_Proyectos_LeerEntidadesAdscritas(proyectoId, '1')
                .then(function (response) {
                    if (response.data.length == 1) {
                        var entidadDel = response.data[0];
                        if (entidadDel.Id == 0) {
                            vm.nombreEntidad = entidadDel.NombreEntidad;
                            vm.tipoVisualizacionDelegar = 'tipo1';
                            asignacionEntidadNacionalSgrServicio.notificarCambio({ tipoVisualizacion: 'tipo2' });
                            asignacionEntidadNacionalSgrServicio.notificarCambio({ reqEntidad: false });
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
                    asignacionEntidadNacionalSgrServicio.notificarCambio({ tipoVisualizacion: 'tipo1' });
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
                asignacionEntidadNacionalSgrServicio.notificarCambio({ tipoVisualizacion: 'tipo1' });
            else if (vm.asignacionTecnico.delegaViabilidad === false) {
                vm.asignacionTecnico.entidadSele = 0;
                asignacionEntidadNacionalSgrServicio.notificarCambio({ tipoVisualizacion: 'tipo2' });
                vm.estadoBotonEdit = 'EDITAR';
                vm.activar = true;
            }
            else if (vm.asignacionTecnico.delegaViabilidad === true) {
                vm.delegado = true;
                asignacionEntidadNacionalSgrServicio.notificarCambio({ tipoVisualizacion: 'tipo1' });

                utilidades.mensajeWarning("¿Está seguro de continuar?", function funcionContinuar() {
                    Guardar(vm.proyectoId, vm.asignacionTecnico.entidadSele, vm.delegado);
                }, function funcionCancelar() {
                    return;
                }, null, null, "Esta operación es irreversible.");
            }
        }

        function Guardar(proyectoId, entityId, delegado) {
            return asignacionEntidadNacionalSgrServicio.SGR_Proyectos_ActualizarEntidadAdscrita(proyectoId, entityId, delegado)
                .then(function (response) {
                    if (response) {
                        asignacionEntidadNacionalSgrServicio.notificarCambio({ actualizarCapitulo: 'true' });
                        guardarCapituloModificado();
                        vm.estadoBotonEdit = 'EDITAR';
                        vm.disabled = true;
                        vm.activar = true;
                        utilidades.mensajeSuccessRedirect(`Se ha delegado la elaboración del concepto de viabilidad a la entidad ${vm.nombreEntidadSeleccionada}.`,
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
            var tipError = 'DELEGEN';
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

    angular.module('backbone').component('delegarViabilidadNacionalSgr', {
        templateUrl: "/src/app/formulario/ventanas/SGR/entidadNacional/asignacionViabilidad/asignacionTecnico/delegar/delegarViabilidadNacionalSgr.html",
        controller: delegarViabilidadNacionalSgrController,
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