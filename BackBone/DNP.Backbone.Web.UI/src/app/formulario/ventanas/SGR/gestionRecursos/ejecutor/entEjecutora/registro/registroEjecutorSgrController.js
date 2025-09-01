(function () {
    'use strict';

    registroEjecutorSgrController.$inject = [
        'utilidades',
        '$sessionStorage',
        'designarEjecutorSgrServicio',
        'justificacionCambiosServicio',
        '$scope'
    ];

    function registroEjecutorSgrController(
        utilidades,
        $sessionStorage,
        designarEjecutorSgrServicio,
        justificacionCambiosServicio,
        $scope
    ) {
        var vm = this;
        vm.user = {};
        vm.lang = "es";
        vm.nombreComponente = "sgrejecutordesignacionentejecutoraregistrodesignacionejecutor";
        vm.Bpin = $sessionStorage.idObjetoNegocio;

        vm.idInstancia = $sessionStorage.idInstancia;

        vm.proyectoId = $sessionStorage.proyectoId;

        //vm.fechaaprobacion = null;
        vm.fechahoy = new Date();
        vm.fechaaceptacion = null;
        vm.idEntEjecutorP = "";
        vm.idEntEjecutorD = "";
        vm.checkboxvalor = "";



        vm.buscar = buscar;
        vm.onClickCancelar = onClickCancelar;
        vm.onClickAsociar = onClickAsociar;
        vm.onClickCheck = onClickCheck;
        vm.restablecerBusqueda = restablecerBusqueda;
        vm.eliminarEjecutor = eliminarEjecutor;

        vm.registroRespuestaProyecto = {

        }

        vm.Valores =
        {
            ProyectoId: 0,
            BPIN: "",
            Criterios: [
                {
                    NombreTipoValor: "",
                    Habilita: false,
                    Valor: 0
                }
            ]
        };

        //vm.ConvertirNumero = ConvertirNumero;

        vm.init = function () {
            vm.notificacionvalidacion({ handler: vm.notificacionValidacionPadre, nombreComponente: vm.nombreComponente, esValido: true });
            
            //vm.disabled = $sessionStorage.soloLectura;
            vm.mostrarBt = false;
            vm.disabled = false;
            vm.activar = true;
            vm.permite = true;
            vm.dsblBtn = true;
            vm.showBtn = false;
            vm.eliminacionPendiente = false;

            // Inicializar el objeto de filtro
            vm.ejecutorFiltro = {
                nit: "",
                tipoEntidadId: null,
                entidadId: null
            };

            vm.ejecutorMarcadoParaEliminacion = null;

            ConsultaEjecutoresAsociadosPropuesto();
            ConsultaTodosTiposEntidades();
            ConsultaEjecutoresAsociadosDesignado();
            ConsultaFechaAprobacion();

        };

        vm.ActivarEditar = function () {
            if (vm.activar == true) {
                $("#EditarE").html("CANCELAR");
                vm.activar = false;
                vm.permite = false;
                vm.mostrarBt = true;
                vm.disabled = false;
                vm.dsblBtn = false;
                vm.showBtn = true;
                
                limpiarCamposFiltro();
                habilitarControles();
                
                ConsultaEjecutoresAsociadosDesignado();
            }
            else {
                utilidades.mensajeWarning("¿Está seguro de continuar?", function funcionContinuar() {
                    if (vm.ejecutorMarcadoParaEliminacion) {
                        restaurarEjecutorEliminado();
                    }
                    
                    limpiarCamposFiltro();
                    
                    OkCancelar();
                    RestablecerModoNoEdicion();
                    
                    desactivarControles();
                    
                    vm.limpiarErrores();
                    
                    ConsultaFechaAprobacion();

                }, function funcionCancelar(reason) {
                    return;
                }, null, null, "Los datos que posiblemente haya diligenciado se perderán.");
            }
        }
        function OkCancelar() {
            setTimeout(function () {
                utilidades.mensajeSuccess("La edición ha sido cancelada con éxito.");
            }, 500);
        }

        function RestablecerModoNoEdicion() {
            $("#EditarE").html("EDITAR");
            vm.activar = true;
            vm.permite = true;
            vm.dsblBtn = true;
            vm.mostrarBt = false;
            vm.showBtn = false;
        }

        function habilitarControles() {
            if (vm.listaEjecutoresAsociadosD && vm.listaEjecutoresAsociadosD.length > 0) {
                $("#txtNit").attr('disabled', true);
                $("#ddlTipoEntidad").attr('disabled', true);
                $("#ddlEntidad").attr('disabled', true);
                vm.showBtn = false;
                vm.permite = true;
            } else {
                $("#txtNit").attr('disabled', false);
                $("#ddlTipoEntidad").attr('disabled', false);
                $("#ddlEntidad").attr('disabled', false);
                vm.showBtn = true;
            }
            
            $("#btnAsociar").attr('disabled', false);
            $("#btnEliminar").attr('disabled', false);
            vm.dsblBtn = false;
        }

        function desactivarControles() {
            $("#txtNit").attr('disabled', true);
            $("#ddlTipoEntidad").attr('disabled', true);
            $("#ddlEntidad").attr('disabled', true);
            $("#btnAsociar").attr('disabled', true);
            $("#btnEliminar").attr('disabled', true);
            vm.showBtn = false;
            vm.dsblBtn = true;
        }

        function restablecerBusqueda() {
            limpiarCamposFiltro();
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
        }

        //para guardar los capitulos modificados y que se llenen las lunas
        function ObtenerSeccionCapitulo() {
            const span = document.getElementById('id-capitulo-' + vm.nombreComponente);
            vm.seccionCapitulo = span.textContent;
        }

        vm.changeEntidad = function () {
            var idTipoE = document.getElementById("ddlTipoEntidad").value;
            idTipoE = idTipoE.replace('number:', '');

            designarEjecutorSgrServicio.ObtenerEjecutorByTipoEntidad(idTipoE).then(
                function (response) {
                    if (response.data != null && response.data != "") {
                        vm.listaFiltroEntidades = response.data;
                    }
                });
        }

        /* ------------------------ Validaciones ---------------------------------*/
                
        vm.notificacionValidacionPadre = function (errores) {
            //Remplazar por cada capitulo
            var tipError = 'FECAPRO';
            var tipError1 = 'ENTAPRO';
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
                            if (TipoError == tipError ) {
                                vm.validarSeccion(TipoError, nameArr[1].toString(), p.Descripcion, false);
                            }
                            else {
                                var idSpanAlertComponent = document.getElementById(tipError + vm.nombreComponente);
                                if (idSpanAlertComponent != null)
                                    idSpanAlertComponent.classList.add('hidden');
                                vm.validarValores(nameArr[0].toString(), p.Descripcion, false);
                            }
                            if (TipoError == tipError1) {
                                vm.validarSeccion(TipoError, nameArr[1].toString(), p.Descripcion, false);
                            }
                            else {
                                var idSpanAlertComponent1 = document.getElementById(tipError1 + vm.nombreComponente);
                                if (idSpanAlertComponent1 != null)
                                    idSpanAlertComponent1.classList.add('hidden');
                                vm.validarValores(nameArr[0].toString(), p.Descripcion, false);
                            }
                        });
                    }
                }
                else {
                    var idSpanAlertComponentAlert = document.getElementById("alert-" + vm.nombreComponente);
                    var idSpanAlertComponent = document.getElementById(tipError + vm.nombreComponente);
                    var idSpanAlertComponent1 = document.getElementById(tipError1 + vm.nombreComponente);

                    if (idSpanAlertComponent != null)
                        idSpanAlertComponent.classList.add('hidden');

                    if (idSpanAlertComponent1 != null)
                        idSpanAlertComponent1.classList.add('hidden');

                    if (idSpanAlertComponentAlert != null)
                        idSpanAlertComponentAlert.classList.remove("ico-advertencia");
                }
                vm.notificacionestado({ nombreComponente: vm.nombreComponente, esValido: isValid });
            }
        };

        vm.validarErroresPreguntas = function (error, Descripcion, esValido) {
            var campomensajeerror = document.getElementById(vm.nombreComponente + "-mensaje-error");
            if (campomensajeerror != undefined) {
                campomensajeerror.innerHTML = "<span class='d-inline-block ico-advertencia'></span><span>" + Descripcion + "</span>";
                campomensajeerror.classList.remove('hidden');
            }
        }

        vm.validarValores = function (error, esValido) {
            var idSpanAlertComponent = document.getElementById("alert-" + error);
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

            var PreguntaObligatoria1 = document.getElementById('PreguntaObligatoria1');
            PreguntaObligatoria1.classList.add('hidden');

            var errorElements = document.getElementsByClassName('errorSeccionDesignacionEjecutor');
            var testDivs = Array.prototype.filter.call(errorElements, function (errorElement) {
                errorElement.innerHTML = "";
                errorElement.classList.add('hidden');
            });

        }

        vm.GuardarEjecutor = function () {
            vm.checkboxvalor = document.getElementById("chkEntEjecutoraPropuesta").checked;

            if (!validar()) {
                    vm.checkboxvalor = false;
                    return;
                }
            else {
                if (vm.ejecutorMarcadoParaEliminacion) {
                    eliminarEjecutorFisicamente(vm.ejecutorMarcadoParaEliminacion);
                    return;
                }
                
                if (vm.idEntEjecutorD != null && vm.idEntEjecutorD != "") {
                    GuardarRespuesta();
                }
                else {
                    if (vm.checkboxvalor === true) {
                        Guardar();
                    }
                    else if (vm.ejecutorSeleccionadoTemp) {
                        asociarEjecutorSeleccionado();
                    }
                    else {
                        utilidades.mensajeError("No ha registrado la entidad ejecutora aprobada.");
                    }
                }
            }
        };

        function asociarEjecutorSeleccionado() {
            var idEjecutor = vm.ejecutorSeleccionadoTemp;
            if (!validar()) {
                return;
            }

            designarEjecutorSgrServicio.CrearEjecutorAsociado(vm.proyectoId, idEjecutor, 2).then(
                function (response) {
                    if (response.data != null && response.data != "") {
                        vm.listaTipoEntidades = response.data;
                    } else {
                        ConsultaEjecutoresAsociadosDesignado();
                        guardarCapituloModificado();
                        GuardarRespuesta();
                        designarEjecutorSgrServicio.notificarCambio({ regEjecutor: true });
                        vm.listaEjecutores = null;
                        vm.limpiarErrores();
                        vm.cantidadDeProyectos = 0;
                        limpiarCamposFiltro();
                        restablecerBusqueda();
                        bloquearControles();
                        RestablecerModoNoEdicion();
                    }
                });
        }

        function validar() {
            var valida = true;
            var PreguntaObligatoria1 = document.getElementById('PreguntaObligatoria1');

            if (vm.fechaaceptacion === null || vm.fechaaceptacion === undefined || vm.fechaaceptacion === "") {
                if (PreguntaObligatoria1 != undefined) {
                    PreguntaObligatoria1.classList.remove('hidden');
                }
                valida = false;
            }
            else {
                if (PreguntaObligatoria1 != undefined) {
                    PreguntaObligatoria1.classList.add('hidden');
                }
            }
            return valida;
        }

        function Guardar() {

            var idEjecutor = "";
            if (!validar()) {
                vm.checkboxvalor = false;
                return;
            }
            else {

                idEjecutor = vm.listaEjecutoresAsociadosP[0].EjecutorId
                designarEjecutorSgrServicio.CrearEjecutorAsociado(vm.proyectoId, idEjecutor, 2).then(
                    function (response) {
                        if (response.data != null && response.data != "") {
                            vm.listaTipoEntidades = response.data;
                        } else {
                            utilidades.mensajeSuccess('');
                            ConsultaEjecutoresAsociadosDesignado();
                            guardarCapituloModificado();
                            designarEjecutorSgrServicio.notificarCambio({ regEjecutor: true });
                            GuardarRespuesta();
                            vm.limpiarErrores();
                            RestablecerModoNoEdicion();
                            //vm.init();
                        }
                    });
            }
        }
        function EliminarRespuesta() {

            vm.registroRespuestaProyecto.campo = "FechaAceptacionEjecutor";
            vm.registroRespuestaProyecto.respuesta = 'NULL';
            vm.registroRespuestaProyecto.proyectoId = vm.proyectoId;

            return designarEjecutorSgrServicio.RegistrarRespuestaEjecutorSGR(vm.registroRespuestaProyecto).then(
                function (response) {
                    if (response.data || response.statusText === "OK") {
                        eliminarCapitulosModificados();
                        vm.fechaaceptacion = null;
                        vm.checkboxvalor = null;
                        vm.limpiarErrores();
                        RestablecerModoNoEdicion();
                    } else {
                        swal('', "Error al realizar la operación", 'error');
                    }
                }
            );
        }
        function GuardarRespuesta() {

            vm.registroRespuestaProyecto.campo = "FechaAceptacionEjecutor";
            vm.registroRespuestaProyecto.respuesta = vm.fechaaceptacion.toISOString();
            vm.registroRespuestaProyecto.proyectoId = vm.proyectoId;    

            return designarEjecutorSgrServicio.RegistrarRespuestaEjecutorSGR(vm.registroRespuestaProyecto).then(
                function (response) {
                    if (response.data || response.statusText === "OK") {
                        guardarCapituloModificado();
                        /*guardarCapituloModificadoEstado();*/
                        utilidades.mensajeSuccess("");
                        vm.limpiarErrores();
                        RestablecerModoNoEdicion();
                    } else {
                        swal('', "Error al realizar la operación", 'error');
                    }
                }
            );
        }

        //vm.notificacionCambiosCapitulos = function (nombreCapituloHijo) {
        //    if (vm.componentesRefresh.includes(nombreCapituloHijo)) {
        //        ObtenerOperacionesCredito();
        //        eliminarCapitulosModificados();
        //    }
        //}
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
        }

        function buscarN(restablecer) {
            var iconoLimpiar = document.getElementById('icoLimpiarBusqueda');
            iconoLimpiar.src = "Img/IcolimpiarBusq.svg";
            if (restablecer)
                vm.BusquedaRealizada = false;
            else
                vm.BusquedaRealizada = true;
        }

        function limpiarCamposFiltro() {
            vm.ejecutorFiltro.nit = "";
            vm.ejecutorFiltro.tipoEntidadId = null;
            vm.ejecutorFiltro.entidadId = null;
            
            // Limpiar visualmente los campos del panel de búsqueda
            $("#txtNit").val("");
            $("#ddlTipoEntidad").val("0").trigger('chosen:updated');
            $("#ddlEntidad").val("0").trigger('chosen:updated');
            
            // Limpiar resultados de búsqueda
            vm.BusquedaRealizada = false;
            vm.listaEjecutores = null;
            vm.cantidadDeProyectos = 0;
            vm.totalRegistros = 0;
            vm.mostrarBt = false;
            
            // Restablecer icono de limpiar
            var iconoLimpiar = document.getElementById('icoLimpiarBusqueda');
            if (iconoLimpiar) {
                iconoLimpiar.src = "Img/IcolimpiarBusqD.svg";
            }
        }

        function limpiarEInhabilitarFiltros() {
            vm.ejecutorFiltro.nit = "";
            vm.ejecutorFiltro.tipoEntidadId = null;
            vm.ejecutorFiltro.entidadId = null;
            
            $("#txtNit").val("");
            $("#ddlTipoEntidad").val("0").trigger('chosen:updated');
            $("#ddlEntidad").val("0").trigger('chosen:updated');
            
            $("#txtNit").attr('disabled', true);
            $("#ddlTipoEntidad").attr('disabled', true);
            $("#ddlEntidad").attr('disabled', true);
            
            vm.showBtn = false;
            
            vm.BusquedaRealizada = false;
            vm.listaEjecutores = null;
            vm.cantidadDeProyectos = 0;
            vm.totalRegistros = 0;
            vm.mostrarBt = false;
            
            var iconoLimpiar = document.getElementById('icoLimpiarBusqueda');
            if (iconoLimpiar) {
                iconoLimpiar.src = "Img/IcolimpiarBusqD.svg";
            }
        }

        $scope.$on("BloquearPorCTUSPendiente", function (evt, data) {
            bloquearControles();
        });

        function bloquearControles() {
            desactivarControles();
            restablecerBusqueda();
            vm.init();
        }
        function ConsultaEjecutoresAsociadosPropuesto() {
            designarEjecutorSgrServicio.SGR_Procesos_ConsultarEjecutorbyTipo(vm.proyectoId,1).then(
                function (response) {
                    if (response.data != null && response.data != "") {
                        vm.listaEjecutoresAsociadosP = response.data;
                        vm.idEntEjecutorP = vm.listaEjecutoresAsociadosP[0].EjecutorId;
                    } else {
                            $("#ddlEntidad").empty();
                            vm.listaEjecutoresAsociadosP = null;
                            vm.idEntEjecutorP = null;
                    }
                });
        }
        function ConsultaEjecutoresAsociadosDesignado() {
            designarEjecutorSgrServicio.SGR_Procesos_ConsultarEjecutorbyTipo(vm.proyectoId,2).then(
                function (response) {
                    if (response.data != null && response.data != "") {
                        vm.listaEjecutoresAsociadosD = response.data;
                        vm.idEntEjecutorD = vm.listaEjecutoresAsociadosD[0].EjecutorId;
                        designarEjecutorSgrServicio.notificarCambio({ regEjecutor: true });
                        vm.dsblBtn = true;
                        vm.showBtn = false;
                        
                        if (!vm.activar) {
                            habilitarControles();
                        }
                        
                        ConsultaEjecutorPropuestoDesignado();
                    } else {
                        if (!vm.disabled) {
                            $("#ddlEntidad").empty();
                            vm.listaEjecutoresAsociadosD = null;
                            vm.idEntEjecutorD = null;
                            vm.checkboxvalor = null;
                            

                            if (!vm.activar) {
                                habilitarControles();
                            }
                        }
                    }
                });
        }
        function ConsultaTodosTiposEntidades() {
            designarEjecutorSgrServicio.catalogoTodosTiposEntidades().then(
                function (response) {
                    if (response.data != null && response.data != "") {
                        vm.listaTipoEntidades = response.data;
                    } else {
                        $("#ddlEntidad").empty();
                    }
                });
        }

        function ConsultaFechaAprobacion() {

            var campoConsulta = "FechaAceptacionEjecutor";
            
            designarEjecutorSgrServicio.ObtenerRespuestaEjecutorSGR(campoConsulta, vm.proyectoId).then(
                function (response) {
                    if (response.data != null && response.data != "") {
                        vm.fechaaceptacion = new Date (response.data);
                    } else {
                        vm.fechaaceptacion = null;
                    }
                });
        }
        function ConsultaEjecutorPropuestoDesignado() {
            if (vm.idEntEjecutorD != null && vm.idEntEjecutorD != "" && vm.idEntEjecutorD === vm.idEntEjecutorP ) {
                        vm.checkboxvalor = true;
                    } else {
                        vm.checkboxvalor = false;
                    }
        }
        function eliminarEjecutor(entity) {
            utilidades.mensajeWarning(
                "¿Está seguro de continuar?",
                function funcionContinuar() {
                    vm.ejecutorMarcadoParaEliminacion = entity;
                    
                    vm.listaEjecutoresAsociadosD = vm.listaEjecutoresAsociadosD.filter(function(item) {
                        return item.Id !== entity.Id;
                    });
                    
                    vm.idEntEjecutorD = null;
                    vm.checkboxvalor = false;
                    
                    vm.eliminacionPendiente = true;
                    
                    desactivarControles();
                    
                    utilidades.mensajeSuccess("Ejecutor eliminado temporalmente. Guarde los cambios para confirmar la eliminación.", false, false, false);
                },
                function funcionCancelar(reason) {
                    console.log("Eliminación cancelada por el usuario");
                },
                "Aceptar",
                "Cancelar",
                "El ejecutor será eliminado temporalmente hasta que guarde los cambios."
            );
        }

        function eliminarEjecutorFisicamente(entity) {
            var proyectoEjecutorId = entity.Id;

            designarEjecutorSgrServicio.EliminarEjecutorAsociado(proyectoEjecutorId).then(function () {
                eliminarCapitulosModificados();
                vm.listaEjecutores = null;
                vm.listaEjecutoresAsociadosD = null;
                vm.ejecutorMarcadoParaEliminacion = null;
                vm.checkboxvalor = false;
                vm.eliminacionPendiente = false;
                
                EliminarRespuesta();
                designarEjecutorSgrServicio.notificarCambio({ regEjecutor: true });
                utilidades.mensajeSuccess("El ejecutor asociado fue eliminado con éxito.");
                vm.limpiarErrores();
                RestablecerModoNoEdicion();
                vm.init();
            })
            .catch(function(error) {
                utilidades.mensajeError('Error al eliminar el ejecutor.', false);
                console.error('Error eliminando ejecutor:', error);
            });
        }

        function restaurarEjecutorEliminado() {
            if (vm.ejecutorMarcadoParaEliminacion) {
                if (!vm.listaEjecutoresAsociadosD) {
                    vm.listaEjecutoresAsociadosD = [];
                }
                
                var yaExiste = vm.listaEjecutoresAsociadosD.some(function(item) {
                    return item.Id === vm.ejecutorMarcadoParaEliminacion.Id;
                });
                
                if (!yaExiste) {
                    vm.listaEjecutoresAsociadosD.push(vm.ejecutorMarcadoParaEliminacion);
                }
                
                vm.idEntEjecutorD = vm.ejecutorMarcadoParaEliminacion.EjecutorId;
                
                ConsultaEjecutorPropuestoDesignado();
                
                vm.ejecutorMarcadoParaEliminacion = null;
                vm.eliminacionPendiente = false;
                
                limpiarEInhabilitarFiltros();
                
                setTimeout(function() {
                    $scope.$apply();
                    utilidades.mensajeSuccess("El ejecutor ha sido restaurado.", false, false, false);
                }, 100);
            }
        }
        function restablecerBusqueda() {
            limpiarCamposFiltro();
        }
        function onClickAsociar() {
            var idEjecutor = "";
            var d = document.getElementsByName('radio');
            for (var i = 0; i < d.length; i++) {
                if (d[i].checked) {
                    idEjecutor = d[i].value;
                    vm.mostrarBt = false;
                }
            }

            if (!idEjecutor) {
                utilidades.mensajeError('Seleccione un ejecutor para asociar.', false);
                return;
            }
            
            vm.ejecutorSeleccionadoTemp = idEjecutor;
            
            if (vm.listaEjecutores) {
                var ejecutorEncontrado = vm.listaEjecutores.find(function(ejecutor) {
                    return ejecutor.Id == idEjecutor;
                });
                
                if (ejecutorEncontrado) {
                    var ejecutorParaTabla = {
                        Id: ejecutorEncontrado.Id,
                        EjecutorId: ejecutorEncontrado.Id,
                        NitEjecutor: ejecutorEncontrado.Nit,
                        NombreEntidad: ejecutorEncontrado.Entidad,
                        TipoEntidad: ejecutorEncontrado.TipoEntidad,
                        temporal: true 
                    };
                    
                    if (!vm.listaEjecutoresAsociadosD) {
                        vm.listaEjecutoresAsociadosD = [];
                    }
                    
                    vm.listaEjecutoresAsociadosD = vm.listaEjecutoresAsociadosD.filter(function(item) {
                        return !item.temporal;
                    });
                    
                    vm.listaEjecutoresAsociadosD.push(ejecutorParaTabla);
                    
                    limpiarEInhabilitarFiltros();
                }
            }
        }
        function onClickCancelar() {
            vm.listaEjecutores = null;
            vm.mostrarBt = false;
            
            if (vm.listaEjecutoresAsociadosP) {
                vm.listaEjecutoresAsociadosP = vm.listaEjecutoresAsociadosP.filter(function(item) {
                    return !item.temporal;
                });
            }
            
            if (vm.listaEjecutoresAsociadosD) {
                vm.listaEjecutoresAsociadosD = vm.listaEjecutoresAsociadosD.filter(function(item) {
                    return !item.temporal;
                });
                
                if (vm.listaEjecutoresAsociadosD.length === 0) {
                    vm.listaEjecutoresAsociadosD = null;
                    vm.idEntEjecutorD = null;
                    
                    if (!vm.activar) {
                        $("#txtNit").attr('disabled', false);
                        $("#ddlTipoEntidad").attr('disabled', false);
                        $("#ddlEntidad").attr('disabled', false);
                        vm.showBtn = true;
                    }
                }
            }
            
            vm.ejecutorSeleccionadoTemp = null;
            
            //utilidades.mensajeInformacionN("", null, null, "Entidad(es) cancelada(s)");
        }
        function onClickCheck() {

            vm.checkboxvalor = document.getElementById("chkEntEjecutoraPropuesta").checked;

            if (vm.checkboxvalor === true) {
                vm.showBtn = false;
            }
            else {
                vm.showBtn = true;
            }
        }

        function buscar() {
            var nit = document.getElementById("txtNit").value;
            var tipoEntidadId = document.getElementById("ddlTipoEntidad").value;
            tipoEntidadId = tipoEntidadId.replace('number:', '');
            var entidadId = document.getElementById("ddlEntidad").value;
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
                });
        }
    }
    angular.module('backbone').component('registroEjecutorSgr', {
        templateUrl: "/src/app/formulario/ventanas/SGR/gestionRecursos/ejecutor/entEjecutora/registro/registroEjecutorSgr.html",
        controller: registroEjecutorSgrController,
        controllerAs: "vm",
        bindings: {
            guardadoevent: '&',
            notificacionestado: '&',
            notificacionvalidacion: '&'
        }
    })
})();