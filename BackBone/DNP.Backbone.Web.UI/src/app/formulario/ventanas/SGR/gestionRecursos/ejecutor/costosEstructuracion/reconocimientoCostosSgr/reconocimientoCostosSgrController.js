(function () {
    'use strict';
    reconocimientoCostosSgrController.$inject = [
        'utilidades',
        '$sessionStorage',
        'justificacionCambiosServicio',
        'designarEjecutorSgrServicio'
    ];

    function reconocimientoCostosSgrController(
        utilidades,
        $sessionStorage,
        justificacionCambiosServicio,
        designarEjecutorSgrServicio
    ) {
        var vm = this;
        vm.user = {};
        vm.lang = "es";
        vm.nombreComponente = 'sgrejecutordesignacioncostosreconocimientocostos';
        vm.seccionCapitulo = null; //Para guardar los capitulos modificados y que se llenen las lunas
        vm.activar = true;
        vm.desactivar = true;
        vm.estadoBotonEdit = 'EDITAR';

        //Funciones
        vm.buscar = buscar;
        vm.onClickCancelar = onClickCancelar;
        vm.onClickAsociar = onClickAsociar;
        vm.restablecerBusqueda = restablecerBusqueda;
        vm.eliminarEjecutor = eliminarEjecutor;

        //Variables Escenciales
        vm.idInstancia = $sessionStorage.idInstancia;
        vm.proyectoId = $sessionStorage.proyectoId;

        //Variables
        vm.aplicaCostos = '';
        vm.disabledEdt = true;
        vm.disabledEli = true;
        vm.idEjecutor = 0;
        vm.listaEjecutoresAsociados = [];

        vm.init = function () {
            vm.notificacionvalidacion({ handler: vm.notificacionValidacionPadre, nombreComponente: vm.nombreComponente, esValido: true });
            vm.disabled = $sessionStorage.soloLectura;
            vm.dsblBtn = $sessionStorage.soloLectura;
            ConsultaEjecutoresAsociados();
        };

        // NOTA: controlar cada parámetro.
        designarEjecutorSgrServicio.registrarObservador(function (datos) {
            //Validacion dato recCostos
            if (datos.recCostos === true) {
                vm.tipoVisualizacion = 'tipo2';
                ConsultaEjecutoresAsociados();
            } else if (datos.recCostos === false) {
                vm.tipoVisualizacion = 'tipo1';
                eliminarCapitulosModificados();
            }

            //Validacion dato aplicaCostos
            if (datos.aplicaCostos === false) {
                if (vm.tieneEjecutor)
                    guardarReconocimiento('EntidadReconocimiento', 'NULL');
            }
        });

        function ConsultaEjecutoresAsociados() {
            designarEjecutorSgrServicio.ObtenerRespuestaEjecutorSGR('EntidadReconocimiento', vm.proyectoId)
                .then(function (response) {
                    const EntidadReconocimiento = response.data;

                    if (EntidadReconocimiento) {
                        designarEjecutorSgrServicio.ObtenerEjecutores('', '', EntidadReconocimiento)
                            .then(function (ejecutoresResponse) {
                                const ejecutores = ejecutoresResponse.data;

                                if (ejecutores && ejecutores.length > 0) {
                                    const listaTemp = {
                                        Id: ejecutores[0].Id,
                                        NitEjecutor: ejecutores[0].Nit,
                                        NombreEntidad: ejecutores[0].Entidad,
                                        TipoEntidad: ejecutores[0].TipoEntidad
                                    };

                                    vm.listaEjecutoresAsociados = [listaTemp];
                                }
                            });

                        vm.dsblBtn = true;
                        vm.tieneEjecutor = true;
                    } else {
                        vm.tieneEjecutor = false;

                        if (!vm.disabled) {
                            $("#ddlEntidadGiro1").empty();
                            vm.listaEjecutoresAsociados = null;
                            vm.dsblBtn = false;
                        }
                    }
                }
                ).catch(function (error) {
                    utilidades.mensajeError('Error en el capitulo "Entidad a la que se le realizará el reconocimiento de los costos" - servicio "ObtenerRespuestaEjecutorSGR - EntidadReconocimiento".');
                    console.error('Error en el capitulo "Entidad a la que se le realizará el reconocimiento de los costos" - servicio "ObtenerRespuestaEjecutorSGR - EntidadReconocimiento" - Error:', error);
                });
        };

        function ConsultaTodosTiposEntidades() {
            designarEjecutorSgrServicio.catalogoTodosTiposEntidades().then(
                function (response) {
                    if (response.data != null && response.data != "") {
                        vm.listaTipoEntidades = response.data;
                    } else {
                        $("#ddlEntidadGiro1").empty();
                    }
                }
            ).catch(function (error) {
                utilidades.mensajeError('Error en el capitulo "Entidad a la que se le realizará el reconocimiento de los costos" - servicio "catalogoTodosTiposEntidades".');
                console.error('Error en el capitulo "Entidad a la que se le realizará el reconocimiento de los costos" - servicio "catalogoTodosTiposEntidades" - Error:', error);
            });
        };

        function restablecerBusqueda() {
            limpiarCamposFiltro();
            buscarN(true);
            vm.listaFiltroEntidades = '';
        };

        function buscar() {
            var nit = document.getElementById("txtNit1").value;
            var tipoEntidadId = document.getElementById("ddlTipoEntidadGiro1").value;
            tipoEntidadId = tipoEntidadId.replace('number:', '');
            var entidadId = document.getElementById("ddlEntidadGiro1").value;
            entidadId = entidadId.replace('number:', '');

            designarEjecutorSgrServicio.ObtenerEjecutores(nit, tipoEntidadId, entidadId).then(
                function (response) {
                    if (response.data != null && response.data != "") {
                        vm.BusquedaRealizada = true;
                        vm.mostrarBt = true;
                        vm.listaEjecutores = response.data;
                        vm.cantidadDeProyectos = response.data.length;
                        vm.totalRegistros = response.data.length;
                    } else {
                        vm.BusquedaRealizada = true;
                        vm.listaEjecutores = null;
                        vm.mostrarBt = false;
                        vm.cantidadDeProyectos = 0;
                        utilidades.mensajeError("No se encontraron resultados para los criterios de búsqueda.");
                    }
                }
            ).catch(function (error) {
                utilidades.mensajeError('Error en el capitulo "Entidad a la que se le realizará el reconocimiento de los costos" - servicio "ObtenerEjecutores".');
                console.error('Error en el capitulo "Entidad a la que se le realizará el reconocimiento de los costos" - servicio "ObtenerEjecutores" - Error:', error);
            });
        };

        function onClickCancelar() {
            vm.listaEjecutores = null;
            vm.mostrarBt = false;
        };

        function onClickAsociar() {
            var idEjecutor = '';
            var d = document.getElementsByName('radio');
            let algunoSeleccionado = false;

            for (var i = 0; i < d.length; i++) {
                if (d[i].checked) {
                    algunoSeleccionado = true;
                    idEjecutor = d[i].value;
                    break;
                }
            };

            const ejecutor = vm.listaEjecutores.find(e => e.Id === Number(idEjecutor));

            if (!algunoSeleccionado) {
                utilidades.mensajeError('Es necesario seleccionar una entidad para asociar.', false);
                return false;
            };

            vm.disabledEli = true;
            vm.disabledEdt = true;
            restablecerBusqueda();
            limpiarCamposFiltro();
            vm.listaEjecutores = null;

            const listaTemp = {
                Id: ejecutor.Id,
                NitEjecutor: ejecutor.Nit,
                NombreEntidad: ejecutor.Entidad,
                TipoEntidad: ejecutor.TipoEntidad
            };

            vm.listaEjecutoresAsociados = [listaTemp];
            vm.dsblBtn = true;
            vm.aplicaCostos = true;
            vm.tieneEjecutor = true;
            vm.idEjecutor = ejecutor.Id;    
            vm.disabledEli = false;
        };

        function eliminarEjecutor(entity) {
            utilidades.mensajeWarning(
                "¿Está seguro de continuar?",
                function funcionContinuar() {
                    vm.listaEjecutoresAsociados = null;
                    vm.aplicaCostos = '';
                    vm.listaEjecutores = null;
                    vm.tieneEjecutor = false;
                    limpiarCamposFiltro();
                    eliminarCapitulosModificados();
                    utilidades.mensajeSuccess("La entidad a la que se le realizará el reconocimiento de los costos se eliminó con éxito.", false, "Es necesario incluir una entidad en esta tabla.", false);
                },
                function funcionCancelar(reason) {
                    console.log("reason", reason);
                },
                "Aceptar",
                "Cancelar",
                "Se eliminará la entidad a la que se le realizará el reconocimiento de los costos."
            );
        };

        function limpiarCamposFiltro() {
            if (vm.ejecutorFiltro != undefined) {
                vm.ejecutorFiltro.nit = '';
                vm.ejecutorFiltro.tipoEntidadId = null;
                vm.ejecutorFiltro.entidadId = null;
            }
        };

        vm.changeEntidad = function () {
            var idTipoE = document.getElementById("ddlTipoEntidadGiro1").value;
            idTipoE = idTipoE.replace('number:', '');

            designarEjecutorSgrServicio.ObtenerEjecutorByTipoEntidad(idTipoE).then(
                function (response) {
                    if (response.data != null && response.data != "") {
                        vm.listaFiltroEntidades = response.data;
                    }
                }
            ).catch(function (error) {
                utilidades.mensajeError('Error en el capitulo "Entidad a la que se le realizará el reconocimiento de los costos" - servicio "ObtenerEjecutorByTipoEntidad".');
                console.error('Error en el capitulo "Entidad a la que se le realizará el reconocimiento de los costos" - servicio "ObtenerEjecutorByTipoEntidad" - Error:', error);
            });
        };

        function buscarN(restablecer) {
            var iconoLimpiar = document.getElementById('icoLimpiarBusqueda1');
            iconoLimpiar.src = "Img/IcolimpiarBusq.svg";
            if (restablecer)
                vm.BusquedaRealizada = false;
            else
                vm.BusquedaRealizada = true;
        };

        vm.ActivarEditar = function () {
            if (vm.activar == true) {
                vm.estadoBotonEdit = 'CANCELAR';
                vm.activar = false;
                vm.disabledEdt = false;

                if (vm.tieneEjecutor) {
                    vm.disabledEli = false;
                }

                ConsultaTodosTiposEntidades();
            }
            else {
                utilidades.mensajeWarning("¿Está seguro de continuar?", function funcionContinuar() {
                    OkCancelar();
                    vm.estadoBotonEdit = 'EDITAR';
                    vm.activar = true;
                    restablecerBusqueda();
                    ConsultaEjecutoresAsociados();
                    vm.disabledEli = true;
                    vm.disabledEdt = true;
                }, function funcionCancelar() {
                    return;
                }, null, null, "Los datos que posiblemente haya diligenciado se perderán.");
            }
        };

        function OkCancelar() {
            setTimeout(function () {
                utilidades.mensajeSuccess("", false, false, false, "La edición ha sido cancelada con éxito.");
            }, 500);
        };

        vm.guardar = function () {

            if (vm.tieneEjecutor === false) {
                utilidades.mensajeError('Es necesario diligenciar la entidad a la que se le realizará el reconocimiento de los costos.', false);
                return false;
            }

            guardarReconocimiento('EntidadReconocimiento', vm.idEjecutor).then(function (ok1) {
                if (!ok1)
                    return handleGuardarError();
                else
                    return handleGuardarExito();
            });
        };

        function guardarReconocimiento(campo, valor) {
            var data = {
                campo: campo,
                respuesta: valor,
                proyectoId: vm.proyectoId
            };

            return designarEjecutorSgrServicio.RegistrarRespuestaEjecutorSGR(data)
                .then(function (response) {
                    return !!response.data;
                })
                .catch(function () {
                    return false;
                });
        };

        function handleGuardarExito() {
            guardarCapituloModificado();
            utilidades.mensajeSuccess('', false, false, false);
            vm.estadoBotonEdit = 'EDITAR';
            vm.activar = true;
            vm.disabledEli = true;
        };

        function handleGuardarError() {
            utilidades.mensajeError('Error al guardar capítulo "Entidad a la que se le realizará el reconocimiento de los costos".', false);
        };

        /* ------------------------ Validaciones ---------------------------------*/

        function eliminarCapitulosModificados() {
            ObtenerSeccionCapitulo();
            var data = {
                ProyectoId: $sessionStorage.InstanciaSeleccionada.ProyectoId,
                Justificacion: "",
                SeccionCapituloId: vm.seccionCapitulo,
                InstanciaId: $sessionStorage.idInstancia
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
        };

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
        };

        function ObtenerSeccionCapitulo() {
            const span = document.getElementById('id-capitulo-' + vm.nombreComponente);
            vm.seccionCapitulo = span.textContent;
        };

        vm.notificacionValidacionPadre = function (errores) {
            //Remplazar por cada capitulo
            var tipError = 'RECCOS';
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
                }
                else {
                    var idSpanAlertComponentAlert = document.getElementById("alert-" + vm.nombreComponente);
                    var idSpanAlertComponent = document.getElementById(tipError + vm.nombreComponente)
                    if (idSpanAlertComponent != null)
                        idSpanAlertComponent.classList.add('hidden');

                    if (idSpanAlertComponentAlert != null)
                        idSpanAlertComponentAlert.classList.remove("ico-advertencia");
                }
                vm.notificacionestado({ nombreComponente: vm.nombreComponente, esValido: isValid });
            }
        };

        vm.validarValores = function (pregunta, errores, esValido) {
            var idSpanAlertComponent = document.getElementById("alert-" + vm.nombreComponente + pregunta);
            if (idSpanAlertComponent != undefined) {
                if (!esValido) {
                    idSpanAlertComponent.classList.add("ico-advertencia");
                } else {
                    idSpanAlertComponent.classList.remove("ico-advertencia");
                }
            }
        };

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
        };

        vm.limpiarErrores = function () {
            var errorElements = document.getElementsByClassName('errorSeccionInformacionGeneralViabilidad');
            var testDivs = Array.prototype.filter.call(errorElements, function (errorElement) {
                errorElement.innerHTML = "";
                errorElement.classList.add('hidden');
            });
        };
    };

    angular.module('backbone').component('reconocimientoCostosSgr', {
        templateUrl: "/src/app/formulario/ventanas/SGR/gestionRecursos/ejecutor/costosEstructuracion/reconocimientoCostosSgr/reconocimientoCostosSgr.html",
        controller: reconocimientoCostosSgrController,
        controllerAs: "vm",
        bindings: {
            guardadoevent: '&',
            notificacionestado: '&',
            notificacionvalidacion: '&'
        }
    })
})();