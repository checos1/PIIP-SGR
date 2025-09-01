(function () {
    'use strict';

    registroInterventorSgrController.$inject = [
        'utilidades',
        '$sessionStorage',
        'designarEjecutorSgrServicio',
        'justificacionCambiosServicio',
        '$scope'
    ];

    function registroInterventorSgrController(
        utilidades,
        $sessionStorage,
        designarEjecutorSgrServicio,
        justificacionCambiosServicio,
        $scope
    ) {
        var vm = this;
        vm.user = {};
        vm.lang = "es";
        vm.nombreComponente = "sgrejecutordesignacionentinterventorregistrodesignacioninterventor";
        vm.Bpin = $sessionStorage.idObjetoNegocio;

        vm.idInstancia = $sessionStorage.idInstancia;

        vm.proyectoId = $sessionStorage.proyectoId;

        vm.AprobacionInterventoria = '';
        vm.EntidadInterventoria = '';

        vm.buscar = buscar;
        vm.onClickCancelar = onClickCancelar;
        vm.onClickAsociar = onClickAsociar;
        vm.restablecerBusqueda = restablecerBusqueda;
        vm.eliminarEjecutor = eliminarEjecutor;

        vm.idInterventorP = null;
        vm.idInterventorD = null;

        vm.mostrarBt = false;
        vm.disabled = true;
        vm.activar = true;
        vm.activar2 = true;
        vm.permiteEditar = false;

        vm.habilitaOperacionCredito = false;

        vm.registroRespuestaProyecto = {

        }


        vm.init = function () {
            //vm.notificacioncambios({ handler: vm.notificacionCambiosCapitulos, nombreComponente: vm.nombreComponente });
            vm.notificacionvalidacion({ handler: vm.notificacionValidacionPadre, nombreComponente: vm.nombreComponente, esValido: true });
            //vm.disabled = $sessionStorage.soloLectura;
            //vm.dsblBtn = $sessionStorage.soloLectura;

            ConsultaTodosTiposEntidadesI();
            ConsultaInterventorAsociadosPropuesto();
            ConsultaInterventorAsociadosDesignado();
            ConsultaTieneInterventor();

            vm.disabled = false;
            vm.eliminacionPendiente = false; // Nueva propiedad para controlar eliminaciones pendientes

            // Inicializar el objeto de filtro
            vm.interventorFiltro = {
                nitI: "",
                tipoEntidadIdI: null,
                entidadIdI: null
            };
            
            // Variable para almacenar el interventor eliminado temporalmente
            vm.interventorEliminadoTemp = null;

            //if ($scope.$parent !== undefined && $scope.$parent.$parent !== undefined && $scope.$parent.$parent.vm !== undefined && $scope.$parent.$parent.vm.HabilitarGuardarPaso !== undefined && !$scope.$parent.$parent.vm.HabilitarGuardarPaso) {
            //    bloquearControles();
            //}
            //vm.ActivarEditar();   
        };

        // NOTA: controlar cada parámetro.
        designarEjecutorSgrServicio.registrarObservador(function (datos) {
            //Validacion dato recCostos

            //if (datos.recCostos === true) {
            //    vm.tipoVisualizacion = 'tipo2';
            //    ConsultaInterventorAsociadosPropuesto();
            //} else if (datos.recCostos === false)
            //    vm.tipoVisualizacion = 'tipo1';

            if (datos.regEjecutor === true) {
                /*if (vm.tieneEjecutor)*/
                ConsultaInterventorAsociadosPropuesto();
            }
        });
        
        vm.ActivarEditar = function () {
            if (vm.activar == true) {
                vm.permiteEditar = true;
                $("#EditarI").html("CANCELAR");
                vm.activar = false;
                vm.mostrarBt = true;
                vm.limpiarErrores();
                
                // Limpiar panel de búsqueda al editar
                limpiarCamposFiltro();
                
                // Habilitar controles al editar (con lógica de interventor asociado)
                habilitarControles();
            }
            else {
                utilidades.mensajeWarning("¿Está seguro de continuar?", function funcionContinuar() {
                    if (vm.interventorEliminadoTemp) {
                        restaurarInterventorEliminado();
                    }
                    
                    if (vm.listaInterventorAsociadosD && vm.listaInterventorAsociadosD.length > 0) {
                        var tieneInterventorTemporal = vm.listaInterventorAsociadosD.some(function(item) {
                            return item.temporal === true;
                        });
                        
                        if (tieneInterventorTemporal) {
                            vm.listaInterventorAsociadosD = vm.listaInterventorAsociadosD.filter(function(item) {
                                return item.temporal !== true;
                            });
                            
                            if (vm.listaInterventorAsociadosD.length === 0) {
                                vm.listaInterventorAsociadosD = null;
                                vm.idInterventorD = null;
                            }
                            
                            vm.interventorSeleccionadoTemp = null;
                        }
                    }
                    
                    limpiarCamposFiltro();
                    
                    OkCancelar();
                    vm.permiteEditar = false;
                    $("#EditarI").html("EDITAR");
                    vm.activar = true;
                    vm.activar2 = true;
                    vm.showBtn = true;
                    vm.mostrarBt = false;
                    
                    // Desactivar controles al cancelar
                    desactivarControles();
                    
                    ConsultaTieneInterventor();
                }, function funcionCancelar(reason) {
                    return;
                }, null, null, "Los datos que posiblemente haya diligenciado se perderán.");
            }
        }

        vm.Guardar = function () {
            if (!validar()) {
                utilidades.mensajeError('Es necesario responder la pregunta "El proyecto aplica para los valores de interventoría"');
                return;
            }
            if (vm.interventorEliminadoTemp) {
                eliminarEjecutorFisicamente(vm.interventorEliminadoTemp);
                return;
            }
            
            if ((!vm.listaInterventorAsociadosD || vm.listaInterventorAsociadosD.length === 0) &&
                (vm.AprobacionInterventoria === null || vm.AprobacionInterventoria === undefined || vm.AprobacionInterventoria === "")) {
                utilidades.mensajeError('Es necesario responder la pregunta "El proyecto aplica para los valores de interventoría"');
                return;
            }
            
            // TERCERA PRIORIDAD: Lógica normal de guardado
            if (vm.AprobacionInterventoria === false) {
                if (vm.listaInterventorAsociadosD != undefined) {
                    eliminarEjecutorInt(vm.listaInterventorAsociadosD[0]);
                }
                else {
                    vm.listaEjecutoresI = null;
                    $("#EditarI").html("EDITAR");
                    vm.activar = true;
                    vm.activar2 = true;
                    vm.limpiarErrores();
                    vm.idInterventorP = null;
                    vm.idInterventorD = null;
                    GuardarRespuesta('0');
                    designarEjecutorSgrServicio.notificarCambio({ regInterventor: false });
                    desactivarControles();
                }
            }
            else {
                if ((vm.AprobacionInterventoria === null || vm.AprobacionInterventoria === undefined || vm.AprobacionInterventoria === "") &&
                    (!vm.listaInterventorAsociadosD || vm.listaInterventorAsociadosD.length === 0) &&
                    (vm.interventorSeleccionadoTemp || vm.EntidadInterventoria === true)) {
                    utilidades.mensajeError('Es necesario responder la pregunta "El proyecto aplica para los valores de interventoría"');
                    return;
                }
                
                vm.activar2 = false;
                vm.showBtn = true;
                if (vm.EntidadInterventoria === true) {
                    GuardarInt();
                } else if (vm.AprobacionInterventoria === true && vm.interventorSeleccionadoTemp) {
                    asociarInterventorSeleccionado();
                } else if (vm.AprobacionInterventoria === true) {
                    utilidades.mensajeError('Es necesario seleccionar un interventor para asociar.');
                }
            }
        }

        function asociarInterventorSeleccionado() {
            if (vm.AprobacionInterventoria === null || vm.AprobacionInterventoria === undefined || vm.AprobacionInterventoria === "") {
                utilidades.mensajeError('Es necesario responder la pregunta "El proyecto aplica para los valores de interventoría"');
                return;
            }
            
            var idEjecutor = vm.interventorSeleccionadoTemp;
            if (!idEjecutor) {
                utilidades.mensajeError('Seleccione un interventor para asociar.', false);
                return;
            }

            designarEjecutorSgrServicio.CrearEjecutorAsociado(vm.proyectoId, idEjecutor, 4).then(
                function (response) {
                    if (response.data != null && response.data != "") {
                        utilidades.mensajeError('Error al asociar el interventor.', false);
                    } else {
                        // Primero guardar la respuesta y hacer los cambios de estado
                        GuardarRespuesta('1').then(function() {
                            // Esperar un momento para que se propague el cambio y luego consultar
                            setTimeout(function() {
                                ConsultaInterventorAsociadosDesignado();
                            }, 500);
                            guardarCapituloModificado();
                            
                            utilidades.mensajeSuccess('');
                            limpiarCamposFiltro();
                            vm.limpiarErrores();
                            $("#EditarI").html("EDITAR");
                            vm.activar = true;
                            vm.activar2 = true;
                            vm.disabled = false;
                            designarEjecutorSgrServicio.notificarCambio({ regInterventor: true });
                            vm.listaEjecutores = null;
                            restablecerBusqueda();
                            bloquearControles();
                            vm.interventorSeleccionadoParaAsociar = false;
                            vm.interventorSeleccionadoTemp = null;
                        });
                    }
                })
                .catch(function (error) {
                    utilidades.mensajeError('Error al asociar el interventor.', false);
                    console.error('Error asociando interventor:', error);
                });
        }

        vm.SolicitaAprobacionChange = function () {

            if (vm.AprobacionInterventoria === true) {
                vm.activar2 = false;
                //vm.showBtn = true;
            }
            else {
                vm.activar2 = true;
                vm.EntidadInterventoria = '';
                vm.showBtn = false;
            }
        }

        function ConsultaTieneInterventor() {

            var campoConsulta = "TieneInterventor";

            designarEjecutorSgrServicio.ObtenerRespuestaEjecutorSGR(campoConsulta, vm.proyectoId).then(
                function (response) {
                    if (response.data != null && response.data != "") {
                        vm.AprobacionInterventoria = (response.data === "true" || response.data === "True");
                        if (vm.AprobacionInterventoria === true) {
                            designarEjecutorSgrServicio.notificarCambio({ regInterventor: true });
                            ConsultaEjecutorPropuestoDesignado();
                        }
                        else {
                            designarEjecutorSgrServicio.notificarCambio({ regInterventor: false });
                        }
                    } else {
                        vm.AprobacionInterventoria = '';
                        designarEjecutorSgrServicio.notificarCambio({ regInterventor: false });
                    }
                });
        }
        function GuardarRespuesta(respuesta) {

            vm.registroRespuestaProyecto.campo = "TieneInterventor";
            vm.registroRespuestaProyecto.respuesta = respuesta;
            vm.registroRespuestaProyecto.proyectoId = vm.proyectoId;

            return designarEjecutorSgrServicio.RegistrarRespuestaEjecutorSGR(vm.registroRespuestaProyecto).then(
                function (response) {
                    if (response.data || response.statusText === "OK") {
                        guardarCapituloModificado();
                        utilidades.mensajeSuccess("", false, false, false);
                        $("#EditarI").html("EDITAR");
                        vm.activar = true;
                        vm.activar2 = true;
                        vm.showBtn = false;
                        vm.limpiarErrores();
                        vm.disabled = false;
                    } else {
                        swal('', "Error al realizar la operación", 'error');
                    }
                }
            );
        }

        function GuardarInt() {
            if (vm.AprobacionInterventoria === null || vm.AprobacionInterventoria === undefined || vm.AprobacionInterventoria === "") {
                utilidades.mensajeError('Es necesario responder la pregunta "El proyecto aplica para los valores de interventoría"');
                return;
            }

            var idEjecutor = "";

            //ConsultaInterventorAsociadosDesignado();
            
            if (vm.listaIinterventorAsociadosP != null && vm.listaIinterventorAsociadosP != "") {

                idEjecutor = vm.listaIinterventorAsociadosP[0].EjecutorId;
                designarEjecutorSgrServicio.CrearEjecutorAsociado(vm.proyectoId, idEjecutor, 4).then(
                    function (response) {
                        if (response.data != null && response.data != "") {
                            vm.listaTipoEntidades = response.data;
                        } else {
                            utilidades.mensajeSuccess('', false, false, false);
                            ConsultaInterventorAsociadosDesignado();
                            guardarCapituloModificado();
                            //limpiarCamposFiltro();
                            vm.limpiarErrores();
                            $("#EditarE").html("EDITAR");
                            vm.activar = true;
                            vm.disabled = true;
                            GuardarRespuesta('1');
                            designarEjecutorSgrServicio.notificarCambio({ regInterventor: true });
                            //vm.listaEjecutores = null;
                            //restablecerBusqueda();
                            bloquearControles();
                            //vm.init();
                        }
                    });
            }
            else {
                //utilidades.mensajeSuccess("", false, false, false, "La entidad ejecutora aún no ha sido definida.");
                utilidades.mensajeError("La entidad ejecutora aún no ha sido definida.");
                vm.AprobacionInterventoria = '';
                vm.EntidadInterventoria = '';
            }
        }

        vm.EntidadInterventoraChange = function () {
            if (vm.EntidadInterventoria === true) {
                vm.showBtn = false;
            }
            else {
                vm.showBtn = true;
            }
        }
        function OkCancelar() {
            setTimeout(function () {
                utilidades.mensajeSuccess("", false, false, false, "La edición ha sido cancelada con éxito.");
            }, 500);
        }

        function RestablecerModoNoEdicion() {
            vm.permiteEditar = false;
            $("#EditarI").html("EDITAR");
            vm.activar = true;
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
            var idTipoE = document.getElementById("ddlTipoEntidadI").value;
            idTipoE = idTipoE.replace('number:', '');

            designarEjecutorSgrServicio.ObtenerEjecutorByTipoEntidad(idTipoE).then(
                function (response) {
                    if (response.data != null && response.data != "") {
                        vm.listaFiltroEntidadesI = response.data;
                    }
                });
        }

        /* ------------------------ Validaciones ---------------------------------*/

        //vm.notificacionValidacionPadre = function (errores) {
        //    vm.limpiarErrores();
        //    if (errores != undefined) {
        //        var erroresRelacionconlapl = errores.find(p => (p.Seccion + p.Capitulo) == vm.nombreComponente);
        //        var erroresJson = (erroresRelacionconlapl == undefined || erroresRelacionconlapl.Errores == "") ? [] : JSON.parse(erroresRelacionconlapl.Errores);
        //        var isValid = (erroresJson == null || erroresJson.length == 0);
        //        if (!isValid) {
        //            erroresJson[vm.nombreComponente].forEach(p => {
        //                var nameArr = p.Error.split('-');
        //                var TipoError = nameArr[0].toString();
        //                if (p.Error == 'EJECPRE1') {
        //                    vm.validarErroresPreguntas(p.Error, p.Descripcion, false);
        //                    vm.validarValores(p.Error, false);
        //                } else if (TipoError == 'SGRERRSEC') {
        //                    vm.validarSeccion(TipoError, nameArr[1].toString(), p.Descripcion, false);
        //                } else {
        //                    vm.validarValores(p.Error, false);
        //                }
        //            });
        //        }
        //        vm.notificacionestado({ nombreComponente: vm.nombreComponente, esValido: isValid });
        //    }
        //}

        vm.notificacionValidacionPadre = function (errores) {
            //Remplazar por cada capitulo
            var tipError = 'APRINT';
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

        function validar() {
            var valida = true;
            var PreguntaObligatoria1 = document.getElementById('PreguntaObligatoria1');

            if (vm.AprobacionInterventoria === null || vm.AprobacionInterventoria === undefined || vm.AprobacionInterventoria === "") {
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

            //var PreguntaObligatoria2 = document.getElementById('PreguntaObligatoria2');

            //if (vm.EntidadInterventoria === null || vm.EntidadInterventoria === undefined || vm.EntidadInterventoria === "") {
            //    if (PreguntaObligatoria2 != undefined) {
            //        PreguntaObligatoria2.classList.remove('hidden');
            //    }
            //    valida = false;
            //}
            //else {
            //    if (PreguntaObligatoria2 != undefined) {
            //        PreguntaObligatoria2.classList.add('hidden');
            //    }
            //}

            return valida;
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
            //var idSpanAlertComponent = document.getElementById("alert-OCDG_VTC");
            //idSpanAlertComponent.classList.remove("ico-advertencia");

            //var idSpanAlertComponent1 = document.getElementById("alert-OCDG_VF");
            //idSpanAlertComponent1.classList.remove("ico-advertencia");

            //var idSpanAlertComponent2 = document.getElementById("alert-OCDG_VP");
            //idSpanAlertComponent2.classList.remove("ico-advertencia");

            vm.validarValores(vm.nombreComponente, true);

            //var errorElements = document.getElementsByClassName('errorSeccionDelegarViabilidadPrevios');
            //var testDivs = Array.prototype.filter.call(errorElements, function (errorElement) {
            //    errorElement.innerHTML = "";
            //    errorElement.classList.add('hidden');
            //});

            var campomensajeerror2 = document.getElementById(vm.nombreComponente + "-mensaje-error");
            campomensajeerror2.innerHTML = "";
            campomensajeerror2.classList.add('hidden');

        }

        //vm.notificacionCambiosCapitulos = function (nombreCapituloHijo) {
        //    if (vm.componentesRefresh.includes(nombreCapituloHijo)) {
        //        ObtenerOperacionesCredito();
        //        eliminarCapitulosModificados();
        //    }
        //}

        function buscarN(restablecer) {
            var iconoLimpiar = document.getElementById('icoLimpiarBusqueda');
            iconoLimpiar.src = "Img/IcolimpiarBusq.svg";
            if (restablecer)
                vm.BusquedaRealizada = false;
            else
                vm.BusquedaRealizada = true;
        }

        function limpiarCamposFiltro() {
            vm.interventorFiltro.nitI = "";
            vm.interventorFiltro.tipoEntidadIdI = null;
            vm.interventorFiltro.entidadIdI = null;
            
            // Limpiar visualmente los campos del panel de búsqueda
            $("#txtNitI").val("");
            $("#ddlTipoEntidadI").val("0").trigger('chosen:updated');
            $("#ddlEntidadI").val("0").trigger('chosen:updated');
            
            // Limpiar resultados de búsqueda
            vm.BusquedaRealizada = false;
            vm.listaEjecutoresI = null;
            vm.cantidadDeProyectos = 0;
            vm.totalRegistros = 0;
            vm.mostrarBt = false;
            
            // Restablecer icono de limpiar
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
        }
        function ConsultaInterventorAsociadosDesignado() {
            designarEjecutorSgrServicio.SGR_Procesos_ConsultarEjecutorbyTipo(vm.proyectoId,4).then(
                function (response) {
                    if (response.data != null && response.data != "" && response.data.length > 0) {
                        vm.listaInterventorAsociadosD = response.data;
                        vm.idInterventorD = vm.listaInterventorAsociadosD[0];
                        designarEjecutorSgrServicio.notificarCambio({ regInterventor: true });
                        
                        // Si estamos en modo edición, verificar si debe deshabilitar filtros
                        if (!vm.activar) {
                            habilitarControles(); // Esto internamente verificará si hay interventor asociado
                        }
                        
                        $scope.$applyAsync(function() {
                            vm.listaInterventorAsociadosD = [...response.data];
                        });
                    } else {
                        if (!vm.activar) {
                            $("#ddlEntidadI").empty();
                            vm.listaInterventorAsociadosD = null;
                            vm.idInterventorD = null;
                            
                            // Si no hay interventor asociado y estamos editando, habilitar filtros
                            habilitarControles();
                        }
                    }
                })
                .catch(function(error) {
                    console.error('Error en ConsultaInterventorAsociadosDesignado:', error);
                });
        }

        function ConsultaEjecutorPropuestoDesignado() {
            
            if (vm.idInterventorD.EjecutorId != null && vm.idInterventorD.EjecutorId != "" && vm.idInterventorD.EjecutorId === vm.idInterventorP.EjecutorId && vm.idInterventorP.EjecutorId != "" && vm.idInterventorP.EjecutorId != null ) {
                vm.EntidadInterventoria = true;
            } else {
                vm.EntidadInterventoria = false;
            }

            if (vm.AprobacionInterventoria === '' || vm.AprobacionInterventoria === false || vm.AprobacionInterventoria === null ) {
                vm.EntidadInterventoria = '';
            }

        }

        function ConsultaInterventorAsociadosPropuesto() {
            designarEjecutorSgrServicio.SGR_Procesos_ConsultarEjecutorbyTipo(vm.proyectoId,2).then(
                function (response) {
                    if (response.data != null && response.data != "") {
                        vm.listaIinterventorAsociadosP = response.data;
                        vm.idInterventorP = vm.listaIinterventorAsociadosP[0];
                        //ConsultaEjecutorPropuestoDesignado();
                        //vm.dsblBtn = true;
                        //vm.showBtn = false;
                    } else {
                        if (!vm.disabled) {
                            $("#ddlEntidadI").empty();
                            vm.listaIinterventorAsociadosP = null;
                            vm.idEntEjecutorP = null;
                            //vm.dsblBtn = false;
                            //vm.showBtn = true;
                        }
                    }
                });
        }
        function ConsultaTodosTiposEntidadesI() {
            designarEjecutorSgrServicio.catalogoTodosTiposEntidades().then(
                function (response) {
                    if (response.data != null && response.data != "") {
                        vm.listaTipoEntidadesI = response.data;
                    } else {
                        $("#ddlEntidadI").empty();
                    }
                });
        }

        function eliminarEjecutorInt(entity) {
            var proyectoEjecutorId = entity.Id;

            designarEjecutorSgrServicio.EliminarEjecutorAsociado(proyectoEjecutorId).then(function () {

                vm.listaEjecutoresI = null;
                
                $("#EditarI").html("EDITAR");
                vm.activar = true;
                vm.activar2 = true;
                vm.limpiarErrores();

                vm.idInterventorP = null;
                vm.idInterventorD = null;
                vm.disabled = false;
                
                GuardarRespuesta('0');
                designarEjecutorSgrServicio.notificarCambio({ regInterventor: false });
                bloquearControles();

                vm.listaInterventorAsociadosD = null;

                //limpiarCamposFiltro();
                utilidades.mensajeSuccess("El interventor asociado fue eliminado con éxito.", false, false, false);
                ConsultaInterventorAsociadosDesignado();
                eliminarCapitulosModificados();

            });
        }
        function eliminarEjecutor(entity) {
            utilidades.mensajeWarning(
                "¿Está seguro de continuar?",
                function funcionContinuar() {
                    // Solo ejecutar la eliminación si confirma con "Aceptar"
                    
                    // Guardar el interventor para poder restaurarlo si se cancela la edición
                    vm.interventorEliminadoTemp = entity;
                    
                    // Quitar completamente de la tabla (no solo ocultar)
                    vm.listaInterventorAsociadosD = vm.listaInterventorAsociadosD.filter(function(item) {
                        return item.Id !== entity.Id;
                    });
                    
                    vm.idInterventorD = null;
                    
                    vm.eliminacionPendiente = true;
                    
                    // Mensaje informativo de confirmación
                    //utilidades.mensajeSuccess("Interventor eliminado temporalmente. Guarde los cambios para confirmar la eliminación.", false, false, false);
                },
                function funcionCancelar(reason) {
                    // Si cancela, no hacer nada - solo cerrar el modal
                    console.log("Eliminación cancelada por el usuario");
                },
                "Aceptar",
                "Cancelar",
                "El interventor será eliminado temporalmente hasta que guarde los cambios."
            );
        }

        function eliminarEjecutorFisicamente(entity) {
            var proyectoEjecutorId = entity.Id;

            designarEjecutorSgrServicio.EliminarEjecutorAsociado(proyectoEjecutorId).then(function () {
                eliminarCapitulosModificados();
                vm.listaEjecutoresI = null;
                vm.listaInterventorAsociadosD = null;
                vm.interventorEliminadoTemp = null;
                vm.eliminacionPendiente = false; // Limpiar eliminación pendiente
                
                vm.idInterventorP = null;
                vm.idInterventorD = null;
                
                // Limpiar las preguntas básicas del formulario
                vm.AprobacionInterventoria = '';
                vm.EntidadInterventoria = '';
                
                GuardarRespuesta('0');
                designarEjecutorSgrServicio.notificarCambio({ regInterventor: false });
                
                $("#EditarI").html("EDITAR");
                vm.activar = true;
                vm.activar2 = true;
                vm.disabled = false;
                vm.limpiarErrores();
                
                desactivarControles();
                
                utilidades.mensajeSuccess("El interventor asociado fue eliminado con éxito.", false, false, false);
                ConsultaInterventorAsociadosDesignado();
            })
            .catch(function(error) {
                utilidades.mensajeError('Error al eliminar el interventor.', false);
                console.error('Error eliminando interventor:', error);
            });
        }

        function restaurarInterventorEliminado() {
            if (vm.interventorEliminadoTemp) {
                console.log("Restaurando interventor:", vm.interventorEliminadoTemp); // Debug log
                
                // Asegurar que la lista existe
                if (!vm.listaInterventorAsociadosD) {
                    vm.listaInterventorAsociadosD = [];
                }
                
                // Verificar que el interventor no esté ya en la lista para evitar duplicados
                var yaExiste = vm.listaInterventorAsociadosD.some(function(item) {
                    return item.Id === vm.interventorEliminadoTemp.Id;
                });
                
                if (!yaExiste) {
                    // Agregar el interventor de vuelta a la lista
                    vm.listaInterventorAsociadosD.push(vm.interventorEliminadoTemp);
                    console.log("Interventor agregado a lista:", vm.listaInterventorAsociadosD); // Debug log
                }
                
                // Restaurar ID del interventor designado
                vm.idInterventorD = vm.interventorEliminadoTemp;
                
                // Limpiar las referencias de eliminación
                vm.interventorEliminadoTemp = null;
                vm.eliminacionPendiente = false;
                
                // Usar timeout para asegurar que la vista se actualice
                setTimeout(function() {
                    $scope.$apply();
                    utilidades.mensajeSuccess("El interventor ha sido restaurado.", false, false, false);
                }, 100);
            }
        }

        function habilitarControles() {
            // Si ya hay un interventor asociado, no permitir editar los filtros
            if (vm.listaInterventorAsociadosD && vm.listaInterventorAsociadosD.length > 0) {
                // Deshabilitar filtros cuando hay interventor asociado
                $("#txtNitI").attr('disabled', true);
                $("#ddlTipoEntidadI").attr('disabled', true);
                $("#ddlEntidadI").attr('disabled', true);
                vm.showBtn = false;
            } else {
                // Habilitar filtros cuando no hay interventor asociado
                $("#txtNitI").attr('disabled', false);
                $("#ddlTipoEntidadI").attr('disabled', false);
                $("#ddlEntidadI").attr('disabled', false);
                vm.showBtn = true;
            }
            
            $("#btnAsociarI").attr('disabled', false);
            $("#btnEliminarI").attr('disabled', false);
            vm.dsblBtn = false;
        }

        function desactivarControles() {
            $("#txtNitI").attr('disabled', true);
            $("#ddlTipoEntidadI").attr('disabled', true);
            $("#ddlEntidadI").attr('disabled', true);
            $("#btnAsociarI").attr('disabled', true);
            $("#btnEliminarI").attr('disabled', true);
            vm.showBtn = false;
            vm.dsblBtn = true;
        }

        function buscar() {
            var nitI = document.getElementById("txtNitI").value;
            var tipoEntidadIdI = document.getElementById("ddlTipoEntidadI").value;
            tipoEntidadIdI = tipoEntidadIdI.replace('number:', '');
            var entidadIdI = document.getElementById("ddlEntidadI").value;
            entidadIdI = entidadIdI.replace('number:', '');
                        
            designarEjecutorSgrServicio.ObtenerEjecutores(nitI, tipoEntidadIdI, entidadIdI).then(
                function (response) {
                    if (response.data != null && response.data != "") {
                        vm.BusquedaRealizada = true;
                        vm.mostrarBt = true;
                        vm.listaEjecutoresI = response.data;
                        vm.cantidadDeProyectos = response.data.length;
                        vm.totalRegistros = response.data.length;
                    } else {
                        vm.BusquedaRealizada = true;
                        vm.listaEjecutoresI = null;
                        vm.mostrarBt = false;
                        vm.cantidadDeProyectos = 0;
                        utilidades.mensajeError("No se encontraron resultados para los criterios de búsqueda.");
                    }
                });
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
                utilidades.mensajeError('Seleccione un interventor para asociar.', false);
                return;
            }
            
            var interventorSeleccionado = vm.listaEjecutoresI.find(function(interventor) {
                return interventor.Id == idEjecutor;
            });
            
            if (interventorSeleccionado) {
                vm.interventorSeleccionadoTemp = idEjecutor;
                
                vm.listaInterventorAsociadosD = [{
                    Id: null, 
                    EjecutorId: interventorSeleccionado.Id,
                    NitEjecutor: interventorSeleccionado.Nit,
                    NombreEntidad: interventorSeleccionado.Entidad,
                    TipoEntidad: interventorSeleccionado.TipoEntidad,
                    temporal: true 
                }];
                
                vm.idInterventorD = vm.listaInterventorAsociadosD[0];
                
                vm.listaEjecutoresI = null;
                vm.BusquedaRealizada = false;
                limpiarCamposFiltro();
                desactivarControles();
                restablecerBusqueda();
            }
        }

        function onClickCancelar() {
            vm.listaEjecutoresI = null;
            vm.mostrarBt = false;
            vm.interventorSeleccionadoParaAsociar = false;
            vm.interventorSeleccionadoTemp = null;
            
            if (vm.listaInterventorAsociadosD && vm.listaInterventorAsociadosD[0] && vm.listaInterventorAsociadosD[0].temporal) {
                vm.listaInterventorAsociadosD = null;
                vm.idInterventorD = null;
            }
            
            setTimeout(function () {
                utilidades.mensajeSuccess("", false, false, false, "La edición ha sido cancelada con éxito.");
            }, 500);
        }
    }
    angular.module('backbone').component('registroInterventorSgr', {
        templateUrl: "/src/app/formulario/ventanas/SGR/gestionRecursos/ejecutor/entInterventora/registro/registroInterventorSgr.html",
        controller: registroInterventorSgrController,
        controllerAs: "vm",
        bindings: {
            guardadoevent: '&',
            notificacionestado: '&',
            notificacionvalidacion: '&'
        }
    })
})();