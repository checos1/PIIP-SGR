(function () {
    'use strict';

    supervisorRegistroDesignacionSgrController.$inject = [
        'utilidades',
        '$sessionStorage',
        'designarEjecutorSgrServicio',
        'justificacionCambiosServicio',
        '$scope',
        '$q',
    ];

    function supervisorRegistroDesignacionSgrController(
        utilidades,
        $sessionStorage,
        designarEjecutorSgrServicio,
        justificacionCambiosServicio,
        $scope,
        $q
    ) {
        var vm = this;
        vm.user = {};
        vm.lang = "es";
        //vm.nombreComponente = 'sgrejecutordesignacionentinterventorregistrovalorctei'; sgrejecutordesignacionentinterventorregistrovalorctei
        
        vm.nombreComponente = 'sgrejecutordesignacionentinterventorregistrovalorctei';
        vm.Bpin = $sessionStorage.idObjetoNegocio;
        vm.idInstancia = $sessionStorage.idInstancia;
        vm.proyectoId = $sessionStorage.proyectoId;
        vm.flujoId = $sessionStorage.idFlujoIframe;

        vm.AprobacionSupervisor = '';
        vm.entidadSupervisor = false;

        vm.buscar = buscar;
        vm.onClickCancelar = onClickCancelar;
        vm.onClickAsociar = onClickAsociar;
        vm.restablecerBusqueda = restablecerBusqueda;
        vm.eliminarEjecutor = eliminarEjecutor;


        vm.idSupervisorP = null;
        vm.idSupervisorD = null;

        vm.mostrarBt = false;
        vm.disabled = true;
        vm.activar = true;
        vm.activar2 = true;
        vm.permiteEditar = false;
        vm.recursoCtei = false;
        vm.bloquearBtn = true;
        vm.habilitaOperacionCredito = false;

        vm.registroRespuestaProyecto = {

        }


        //************************
        //RASTREAR DE ESTA SECCION
        //************************

        vm.desactivar = true;
        vm.seccionCapitulo = null;
        vm.editar = false;

        

        //Variables
        //vm.aplicaCostos = '';
        vm.disabled = true;
        vm.activarRes = true;
        
        vm.idEjecutor = 0;

        vm.supervisorFiltro = {
            nit: '',
            tipoEntidadId: null,
            entidadId: null
        };

        vm.estadoBotonEdit = 'EDITAR';
        
        vm.init = function () {

            vm.notificacionvalidacion({ handler: vm.notificacionValidacionPadre, nombreComponente: vm.nombreComponente, esValido: true });
            ConsultaTodosTiposEntidadesSupervisor(); 
            ConsultaSupervisorAsociadosPropuesto();
            ConsultaSupervisorAsociadoDesignado();            
            ConsultaTieneSupervisor();
            ConsultaValoresAprobacion();
            vm.parametrosRecursosCTEI = utilidades.obtenerParametroTransversal('RecursosCTeI');            
            vm.disabled = false;
            
            vm.eliminacionPendiente = false;
            vm.supervisorMarcadoParaEliminacion = null;
            
        };

        designarEjecutorSgrServicio.registrarObservador(function (datos) {            

            if (datos.regEjecutor === true) {                
                ConsultaSupervisorAsociadosPropuesto();
            }
        });

        function ConsultaValoresAprobacion() {
            designarEjecutorSgrServicio.LeerValoresAprobacionSGR(vm.proyectoId)
                .then(function (response) {
                    if (response.data != null && response.data != "") {
                        let datosProcesados = JSON.parse(response.data).map(item => ({
                            Id: item.Id,
                            Etapa: item.Etapa,
                            TipoEnEntidad: item.TipoEnEntidad,
                            TipoRecurso: item.TipoRecurso,
                            TipoRecursoId: item.TipoRecursoId,
                            Bienio: item.Bienio,
                            ValorAprobado: item.ValorAprobado,
                            Valorsupervisor: item.ValorSupervisor
                        }));                        

                        vm.recursoCtei = datosProcesados.some(item =>
                            vm.parametrosRecursosCTEI.includes(item.TipoRecursoId)
                        );
                       
                    }
                });
        };



        vm.ActivarEditar = function () {
            if (vm.activar == true) {
                vm.permiteEditar = true;
                vm.estadoBotonEdit = 'CANCELAR';
                vm.activar = false;
                //vm.mostrarBt = true;
                //vm.editar = true;
                vm.limpiarErrores();
            }
            else {
                utilidades.mensajeWarning("¿Está seguro de continuar?", function funcionContinuar() {
                    var tieneSupervisorParaRestaurar = vm.supervisorMarcadoParaEliminacion != null;
                    
                    if (tieneSupervisorParaRestaurar) {
                        restaurarSupervisorEliminado();
                        
                        OkCancelar();
                        vm.permiteEditar = false;
                        vm.estadoBotonEdit = 'EDITAR';
                        vm.activar = true;
                        vm.showBtn = false;
                        
                        ConsultaTieneSupervisor();
                    } else {
                        if (vm.listaSupervisorAsociadosD && vm.listaSupervisorAsociadosD.length > 0) {
                            var tieneSupervisorTemporal = vm.listaSupervisorAsociadosD.some(function(item) {
                                return item.temporal === true;
                            });
                            
                            if (tieneSupervisorTemporal) {
                                vm.listaSupervisorAsociadosD = vm.listaSupervisorAsociadosD.filter(function(item) {
                                    return item.temporal !== true;
                                });
                                
                                if (vm.listaSupervisorAsociadosD.length === 0) {
                                    vm.listaSupervisorAsociadosD = null;
                                    vm.idSupervisorD = null;
                                }
                                
                                vm.supervisorSeleccionadoTemp = null;
                            }
                        }
                        
                        limpiarCamposFiltro();
                        
                        OkCancelar();
                        vm.permiteEditar = false;
                        vm.estadoBotonEdit = 'EDITAR';
                        vm.activar = true;
                        vm.showBtn = false;
                        
                        vm.init();
                    }
                }, function funcionCancelar() {
                    return;
                }, null, null, "Los datos que posiblemente haya diligenciado se perderán.");
            }
        };

        /** RASTREAR: Funcion */

        vm.guardar = function () {
            if (vm.supervisorMarcadoParaEliminacion) {
                eliminarSupervisorFisicamente(vm.supervisorMarcadoParaEliminacion);
                return;
            }
            
            if (!validar()) {
                utilidades.mensajeError('Es necesario responder la pregunta "El proyecto aplica para los valores de interventoría"');
                return;
            }
            else {
                if (vm.AprobacionSupervisor === false) {
                    if (vm.listaSupervisorAsociadosD != undefined) {
                        eliminarEjecutorInt(vm.listaSupervisorAsociadosD[0]);
                    }
                    else {
                        vm.listaEjecutoresI = null;
                        vm.estadoBotonEdit = 'EDITAR';
                        vm.activar = true;
                        vm.activar2 = true;
                        vm.limpiarErrores();
                        vm.idInterventorP = null;
                        vm.idInterventorD = null;
                        vm.disabled = false;
                        GuardarRespuesta('0');

                        vm.showBtn = false;

                        designarEjecutorSgrServicio.notificarCambio({ regSupervisor: false });
                        bloquearControles();
                    }
                }
                else {
                    vm.activar2 = false;
                    vm.showBtn = true;
                    if (vm.EntidadSupervisor === true) {
                        GuardarInt();
                    } else if (vm.AprobacionSupervisor === true && vm.supervisorSeleccionadoTemp) {
                        // Lógica para asociar supervisor seleccionado desde búsqueda
                        asociarSupervisorSeleccionado();
                    } else if (vm.AprobacionSupervisor === true && (!vm.listaSupervisorAsociadosD || vm.listaSupervisorAsociadosD.length === 0)) {
                        utilidades.mensajeError('Es necesario seleccionar un supervisor para asociar.');
                    } else if (vm.AprobacionSupervisor === true) {
                        utilidades.mensajeError('No ha registrado la entidad supervisora aprobada.');
                    }
                }
            }
        }

        function asociarSupervisorSeleccionado() {
            var idEjecutor = vm.supervisorSeleccionadoTemp;
            if (!idEjecutor) {
                utilidades.mensajeError('Seleccione un supervisor para asociar.', false);
                return;
            }

            var promEliminar;
            if (vm.listaSupervisorAsociadosD && vm.listaSupervisorAsociadosD.length) {
                var existente = vm.listaSupervisorAsociadosD[0];
                promEliminar = designarEjecutorSgrServicio
                    .EliminarEjecutorAsociado(existente.Id)
                    .catch(function (err) {
                        console.warn('Error al eliminar previo (se ignorará):', err);
                        return;
                    });
            } else {
                promEliminar = $q.when();
            }

            var promCrear = promEliminar.then(function () {
                return designarEjecutorSgrServicio.CrearEjecutorAsociado(vm.proyectoId, idEjecutor, 5);
            });

            promCrear
                .then(function (response) {
                    ConsultaSupervisorAsociadoDesignado();
                    guardarCapituloModificado();
                    limpiarCamposFiltro();
                    vm.limpiarErrores();
                    vm.estadoBotonEdit = 'EDITAR';
                    vm.entidadSupervisor = false;
                    vm.BusquedaRealizada = false;
                    vm.listaEjecutoresAsociados = null;
                    vm.activar = true;
                    vm.activar2 = true;
                    vm.disabled = false;
                    vm.bloquearBtn = true;

                    GuardarRespuesta('1');
                    designarEjecutorSgrServicio.notificarCambio({ regSupervisor: true });

                    vm.listaEjecutores = null;
                    utilidades.mensajeSuccess('Supervisor asociado correctamente', false, false, false);
                })
                .catch(function (err) {
                    utilidades.mensajeError('Ocurrió un error al guardar el supervisor.', false);
                });

            vm.disabledEli = false;
        }

        vm.SolicitaAprobacionChange = function () {

            if (vm.AprobacionSupervisor === true) {
                vm.activar2 = false;
                //vm.showBtn = true;
            }
            else {
                vm.activar2 = true;
                vm.EntidadInterventoria = '';
                vm.showBtn = false;
            }
        }

        /** --Fin Rastrear */

        function ConsultaTieneSupervisor() {
            var campoConsulta = "AprobadoSupervisorCtei";
            designarEjecutorSgrServicio.ObtenerRespuestaEjecutorSGR(campoConsulta, vm.proyectoId).then(
                function (response) {                    
                    if (response.data != null && response.data != "") {
                        vm.AprobacionSupervisor = (response.data === "true" || response.data === "True");
                        if (vm.AprobacionSupervisor === true) {
                            designarEjecutorSgrServicio.notificarCambio({ regSupervisor: true });
                            ConsultaEjecutorPropuestoDesignado();
                        }
                        else {
                            designarEjecutorSgrServicio.notificarCambio({ regSupervisor: false });
                        }
                    } else {
                        vm.AprobacionSupervisor = '';
                        designarEjecutorSgrServicio.notificarCambio({ regSupervisor: false });
                    }
                });
        }
        function GuardarRespuesta(respuesta) {

            vm.registroRespuestaProyecto.campo = "AprobadoSupervisorCtei";
            vm.registroRespuestaProyecto.respuesta = respuesta;
            vm.registroRespuestaProyecto.proyectoId = vm.proyectoId;

            return designarEjecutorSgrServicio.RegistrarRespuestaEjecutorSGR(vm.registroRespuestaProyecto).then(
                function (response) {
                    if (response.data || response.statusText === "OK") {
                        guardarCapituloModificado();
                        // Mensaje removido para evitar duplicados
                        vm.estadoBotonEdit = 'EDITAR';
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

            var idEjecutor = "";

            //ConsultaInterventorAsociadosDesignado();

            if (vm.listaInterventorAsociadosD != null && vm.listaInterventorAsociadosD != "") {

                idEjecutor = vm.listaInterventorAsociadosD[0].EjecutorId;
                designarEjecutorSgrServicio.CrearEjecutorAsociado(vm.proyectoId, idEjecutor, 4).then(
                    function (response) {
                        if (response.data != null && response.data != "") {
                            vm.listaTipoEntidades = response.data;
                        } else {
                            ConsultaSupervisorAsociadoDesignado();
                            guardarCapituloModificado();
                            //limpiarCamposFiltro();
                            vm.limpiarErrores();
                            vm.estadoBotonEdit = 'EDITAR';
                            vm.activar = true;
                            vm.disabled = false;
                            GuardarRespuesta('1');
                            designarEjecutorSgrServicio.notificarCambio({ regSupervisor: true });
                            /*
                             
                                ConsultaTodosTiposEntidadesSupervisor();
            ConsultaSupervisorAsociadosPropuesto();
            ConsultaSupervisorAsociadoDesignado();
            ConsultaTieneSupervisor();

                             */
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
                vm.AprobacionSupervisor = '';
                vm.EntidadInterventoria = '';
            }
        }



        vm.SupervisionDesignacionChange = function () {
            /* Verificar que tenga la validación si tiene supervisión */
            if (!vm.AprobacionSupervisor) {
                vm.entidadSupervisor = false;
                vm.EntidadSupervisor = null;
                vm.showBtn = false;
                vm.restablecerBusqueda();
            }
            else {
                vm.entidadSupervisor = true;
                vm.EntidadSupervisor = null;
                vm.activar2 = false;
                vm.showBtn = true;
            }
        };

        function OkCancelar() {
            setTimeout(function () {
                utilidades.mensajeSuccess("", false, false, false, "La edición ha sido cancelada con éxito.");
            }, 500);
        };

        function restablecerBusqueda() {
            limpiarCamposFiltro();
            buscarN(true);
            var iconoLimpiar = document.getElementById('icoLimpiarBusqueda');
            iconoLimpiar.src = "Img/IcolimpiarBusqD.svg";
        }


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

        function ObtenerSeccionCapitulo() {
            const span = document.getElementById('id-capitulo-' + vm.nombreComponente);
            if (!span) {
                console.log("No se encontró el elemento con id");
                return;
            }
            vm.seccionCapitulo = span.textContent;
        };

        vm.changeEntidad = function () {

            var idTipoE = vm.supervisorFiltro.tipoEntidadId;
            if (!idTipoE) return;
            designarEjecutorSgrServicio.ObtenerEjecutorByTipoEntidad(idTipoE).then(
                function (response) {
                    if (response.data != null && response.data != "") {
                        vm.listaFiltroEntidades = response.data;
                    }
                }
            );
        };

        vm.notificacionValidacionPadre = function (errores) {
            //Remplazar por cada capitulo
            var tipError = 'REGSUP';
            console.log("Validacion ", tipError);
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

        function validar() {
            var valida = true;
            var PreguntaObligatoria1 = document.getElementById('PreguntaObligatoria1');

            if (vm.AprobacionSupervisor === null || vm.AprobacionSupervisor === undefined || vm.AprobacionSupervisor === "") {
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
        };

        vm.limpiarErrores = function () {
            var errorElements = document.getElementsByClassName('errorSeccionInformacionGeneralViabilidad');
            var testDivs = Array.prototype.filter.call(errorElements, function (errorElement) {
                errorElement.innerHTML = "";
                errorElement.classList.add('hidden');
            });
        };

        function buscarN(restablecer) {
            var iconoLimpiar = document.getElementById('icoLimpiarBusqueda');
            iconoLimpiar.src = "Img/IcolimpiarBusq.svg";
            if (restablecer)
                vm.BusquedaRealizada = false;
            else
                vm.BusquedaRealizada = true;
        };

        function limpiarCamposFiltro() {
            if (vm.supervisorFiltro != undefined) {
                vm.supervisorFiltro.nit = '';
                vm.supervisorFiltro.tipoEntidadId = null;
                vm.supervisorFiltro.entidadId = null;
            }
        };

        $scope.$on("BloquearPorCTUSPendiente", function (evt, data) {
            bloquearControles();
        });

        function bloquearControles() {
            $("#txtNITI").attr('disabled', true);
            $("#ddlTipoEntidadI").attr('disabled', true);
            $("#ddlEntidadI").attr('disabled', true);
            $("#btnAsociarI").attr('disabled', true);
            $("#btnEliminarI").attr('disabled', true);
            vm.disabled = true;
            vm.showBtn = false;
            vm.dsblBtn = true;
        }

        function ConsultaSupervisorAsociadoDesignado() {
            designarEjecutorSgrServicio.SGR_Procesos_ConsultarEjecutorbyTipo(vm.proyectoId, 5).then(
                function (response) {
                    if (response.data != null && response.data != "") {
                        vm.listaSupervisorAsociadosD = response.data;
                        vm.idSupervisorD = vm.listaSupervisorAsociadosD[0];
                        vm.bloquearBtn = true;
                        //vm.dsblBtn = true;
                        //vm.showBtn = false;
                        ConsultaEjecutorPropuestoDesignado();
                    } else {
                        vm.bloquearBtn = false
                        if (!vm.activar) {
                            $("#ddlEntidadI").empty();
                            vm.listaSupervisorAsociadosD = null;
                            vm.idSupervisorD = null;
                            //vm.dsblBtn = false;
                            //vm.showBtn = true;
                        }
                    }
                });
        }

        function ConsultaEjecutorPropuestoDesignado() {
            if (vm.idSupervisorD && vm.idSupervisorP && 
                vm.idSupervisorD.EjecutorId != null && vm.idSupervisorD.EjecutorId != "" && 
                vm.idSupervisorD.EjecutorId === vm.idSupervisorP.EjecutorId && 
                vm.idSupervisorP.EjecutorId != "" && vm.idSupervisorP.EjecutorId != null) {
                vm.EntidadSupervisor = true;
            } else {
                vm.EntidadSupervisor = false;
            }

            if (vm.AprobacionSupervisor === '' || vm.AprobacionSupervisor === false || vm.AprobacionSupervisor === null) {
                vm.EntidadSupervisor = false;
            }
        }
        function ConsultaSupervisorAsociadosPropuesto() {
            designarEjecutorSgrServicio.SGR_Procesos_ConsultarEjecutorbyTipo(vm.proyectoId, 2).then(
                function (response) {
                    if (response.data != null && response.data != "") {
                        vm.listaISupervisorAsociadosP = response.data;
                        vm.idSupervisorP = vm.listaISupervisorAsociadosP[0];
                        ConsultaEjecutorPropuestoDesignado();
                        //vm.dsblBtn = true;
                        //vm.showBtn = false;
                    } else {
                        if (!vm.disabled) {
                            $("#ddlEntidadI").empty();
                            vm.listaISupervisorAsociadosP = null;
                            vm.idEntEjecutorP = null;
                            //vm.dsblBtn = false;
                            //vm.showBtn = true;
                        }
                    }
                });
        }

        function ConsultaTodosTiposEntidadesSupervisor() {
            designarEjecutorSgrServicio.catalogoTodosTiposEntidades().then(
                function (response) {
                    if (response.data != null && response.data != "") {
                        vm.listaTipoEntidades = response.data;
                    } else {
                        $("#ddlEntidadI").empty();
                    }
                });
        }

        function eliminarEjecutorInt(entity) {
            var proyectoEjecutorId = entity.Id;

            designarEjecutorSgrServicio.EliminarEjecutorAsociado(proyectoEjecutorId).then(function () {

                vm.listaEjecutoresI = null;

                vm.estadoBotonEdit = 'EDITAR';
                vm.activar = true;
                vm.activar2 = true;
                vm.limpiarErrores();

                vm.idSupervisorP = null;
                vm.idSupervisorD = null;
                vm.disabled = false;

                GuardarRespuesta('0');
                designarEjecutorSgrServicio.notificarCambio({ regSupervisor: false });
                bloquearControles();

                vm.listaInterventorAsociadosD = null;

                //limpiarCamposFiltro();
                vm.bloquearBtn = false;
                ConsultaInterventorAsociadosDesignado();
                eliminarCapitulosModificados();

            });
        }

        function eliminarEjecutor(entity) {
            utilidades.mensajeWarning(
                "¿Está seguro de continuar?",
                function funcionContinuar() {
                    // Solo ejecutar la eliminación si confirma con "Aceptar"
                    
                    // Guardar el supervisor para poder restaurarlo si se cancela la edición
                    vm.supervisorMarcadoParaEliminacion = entity;
                    
                    // Quitar completamente de la tabla (no solo ocultar)
                    vm.listaSupervisorAsociadosD = vm.listaSupervisorAsociadosD.filter(function(item) {
                        return item.Id !== entity.Id;
                    });
                    
                    vm.idSupervisorD = null;
                    
                    // Limpiar las preguntas básicas del formulario
                    vm.AprobacionSupervisor = '';
                    vm.EntidadSupervisor = false;
                    
                    // Marcar que hay una eliminación pendiente para habilitar el botón guardar
                    vm.eliminacionPendiente = true;
                    
                    // Mensaje informativo de confirmación
                    utilidades.mensajeSuccess("Supervisor eliminado temporalmente. Guarde los cambios para confirmar la eliminación.", false, false, false);
                },
                function funcionCancelar(reason) {
                    // Si cancela, no hacer nada - solo cerrar el modal
                    console.log("Eliminación cancelada por el usuario");
                },
                "Aceptar",
                "Cancelar",
                "El supervisor será eliminado temporalmente hasta que guarde los cambios."
            );
        };

        vm.eliminarEjecutor = function (entity) {
            eliminarEjecutor(entity);
        }

        function eliminarSupervisorFisicamente(entity) {
            var proyectoEjecutorId = entity.Id;

            designarEjecutorSgrServicio.EliminarEjecutorAsociado(proyectoEjecutorId).then(function () {
                eliminarCapitulosModificados();
                vm.listaSupervisorAsociadosD = null;
                vm.supervisorMarcadoParaEliminacion = null;
                vm.eliminacionPendiente = false;
                
                vm.idSupervisorP = null;
                vm.idSupervisorD = null;
                
                vm.AprobacionSupervisor = '';
                vm.EntidadSupervisor = false;
                
                GuardarRespuesta('null');
                designarEjecutorSgrServicio.notificarCambio({ regSupervisor: false });
                
                vm.estadoBotonEdit = 'EDITAR';
                vm.activar = true;
                vm.activar2 = true;
                vm.disabled = false;
                vm.limpiarErrores();
                
                bloquearControles();
                
                utilidades.mensajeSuccess("La entidad encargada de supervisar se eliminó con éxito.", false, "Es necesario incluir una entidad en esta tabla.", false);
                ConsultaSupervisorAsociadoDesignado();
            })
            .catch(function(error) {
                utilidades.mensajeError('Error al eliminar el supervisor.', false);
                console.error('Error eliminando supervisor:', error);
            });
        }

        function restaurarSupervisorEliminado() {
            if (vm.supervisorMarcadoParaEliminacion) {
                if (!vm.listaSupervisorAsociadosD) {
                    vm.listaSupervisorAsociadosD = [];
                }
                
                var yaExiste = vm.listaSupervisorAsociadosD.some(function(item) {
                    return item.Id === vm.supervisorMarcadoParaEliminacion.Id;
                });
                
                if (!yaExiste) {
                    vm.listaSupervisorAsociadosD.push(vm.supervisorMarcadoParaEliminacion);
                }
                
                vm.idSupervisorD = vm.supervisorMarcadoParaEliminacion;
                
                vm.supervisorMarcadoParaEliminacion = null;
                vm.eliminacionPendiente = false;
                
                setTimeout(function() {
                    $scope.$apply();
                    utilidades.mensajeSuccess("El supervisor ha sido restaurado.", false, false, false);
                }, 100);
            }
        }

        function restablecerBusqueda() {
            limpiarCamposFiltro();
            buscarN(true);
            vm.listaFiltroEntidades = '';
            var iconoLimpiar = document.getElementById('icoLimpiarBusqueda');
            iconoLimpiar.src = "Img/IcolimpiarBusqD.svg";
        };

        function onClickAsociar() {
            
            var idEjecutor = '';
            angular.forEach(document.getElementsByName('radio'), function (r) {
                if (r.checked) { idEjecutor = r.value; }
            });
            if (!idEjecutor) {
                utilidades.mensajeError('Seleccione un supervisor para asociar.', false);
                return;
            }
            
            var supervisorSeleccionado = vm.listaEjecutoresAsociados.find(function(supervisor) {
                return supervisor.Id == idEjecutor;
            });
            
            if (supervisorSeleccionado) {
                vm.supervisorSeleccionadoTemp = idEjecutor;
                
                vm.listaSupervisorAsociadosD = [{
                    Id: null, // Sin ID porque aún no se ha guardado en BD
                    EjecutorId: supervisorSeleccionado.Id,
                    NitEjecutor: supervisorSeleccionado.Nit,
                    NombreEntidad: supervisorSeleccionado.Entidad,
                    TipoEntidad: supervisorSeleccionado.TipoEntidad,
                    temporal: true 
                }];
                
                vm.idSupervisorD = vm.listaSupervisorAsociadosD[0];
                
                // Limpiar campos de búsqueda
                vm.listaEjecutoresAsociados = null;
                vm.BusquedaRealizada = false;
                limpiarCamposFiltro();
                restablecerBusqueda();
            }
            vm.mostrarBt = false;
        };

        function onClickCancelar() {
            vm.listaEjecutoresAsociados = null;
            vm.mostrarBt = false;
            vm.supervisorSeleccionadoTemp = null;
            
            // Limpiar tabla temporal si existe
            if (vm.listaSupervisorAsociadosD && vm.listaSupervisorAsociadosD[0] && vm.listaSupervisorAsociadosD[0].temporal) {
                vm.listaSupervisorAsociadosD = null;
                vm.idSupervisorD = null;
            }
            
            if (vm.listaSupervisorAsociadosD && vm.listaSupervisorAsociadosD.length > 0) {
                var tieneSupervisorTemporal = vm.listaSupervisorAsociadosD.some(function(item) {
                    return item.temporal === true;
                });
                
                if (tieneSupervisorTemporal) {
                    vm.listaSupervisorAsociadosD = vm.listaSupervisorAsociadosD.filter(function(item) {
                        return item.temporal !== true;
                    });
                    
                    if (vm.listaSupervisorAsociadosD.length === 0) {
                        vm.listaSupervisorAsociadosD = null;
                        vm.idSupervisorD = null;
                    }
                    
                    vm.supervisorSeleccionadoTemp = null;
                }
            }
            
            setTimeout(function () {
                utilidades.mensajeSuccess("", false, false, false, "La edición ha sido cancelada con éxito.");
            }, 500);
        };

        function buscar() {            
            var nit = vm.supervisorFiltro.nit || '';
            var tipoEntidadId = vm.supervisorFiltro.tipoEntidadId || '';
            var entidadId = vm.supervisorFiltro.entidadId || '';


            designarEjecutorSgrServicio.ObtenerEjecutores(nit, tipoEntidadId, entidadId).then(
                function (response) {
                    
                    if (response.data != null && response.data != "") {
                        vm.BusquedaRealizada = true;
                        vm.mostrarBt = true;
                        vm.listaEjecutoresAsociados = response.data;
                        vm.cantidadDeProyectos = response.data.length;
                        vm.totalRegistros = response.data.length;
                       
                    } else {
                        vm.BusquedaRealizada = true;
                        vm.listaEjecutoresAsociados = null;
                        vm.mostrarBt = false;
                        vm.cantidadDeProyectos = 0;
                        utilidades.mensajeError("No se encontraron resultados para los criterios de búsqueda.");
                    }
                }
            );
        };
    };

    angular.module('backbone')
        .component('supervisorRegistroDesignacionSgr', {
            templateUrl: '/src/app/formulario/ventanas/SGR/gestionRecursos/ejecutor/entInterventora/supervisorRegistroDesignacion/supervisorRegistroDesignacionSgr.html',
        controller: supervisorRegistroDesignacionSgrController,
        controllerAs: "vm",
        bindings: {
            guardadoevent: '&',
            notificacionestado: '&',
            notificacionvalidacion: '&'
        }
    });
})();