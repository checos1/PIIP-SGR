(function () {
    'use strict';

    solicitudConceptoFormulario.$inject = [
        '$scope',
        '$sessionStorage',
        'utilidades',
        'comunesServicio',
        'sesionServicios',
        'constantesBackbone',
        'justificacionCambiosServicio',
        '$timeout',
        '$location',
        'aclaracionLeyendaServicio',
        'trasladosServicio'
    ];





    function solicitudConceptoFormulario(
        $scope,
        $sessionStorage,
        utilidades,
        comunesServicio,
        sesionServicios,
        constantesBackbone,
        justificacionCambiosServicio,
        $timeout,
        $location,
        aclaracionLeyendaServicio,
        trasladosServicio,
    ) {
        var vm = this;


        /**declara variables */
        vm.entidadDestino = undefined;
        vm.fechaHoy = formatearFecha(obtenerFechaSinHoras(new Date()));
        vm.direcciontecnicas = [];
        vm.subdireccionestecnicas = [];
        vm.analistas = [];
        vm.EntityTypeCatalogOptionId = 0;
        vm.filtro = {
            direccionestecnica: "",
            subdireccionestecnica: "",
            analista: "",
            parentId: "",
            get seleccionado() {
                if (this.analista !== "") return this.analista;
                if (this.subdireccionestecnica !== "") return this.subdireccionestecnica;
                if (this.direccionestecnica !== "") return this.direccionestecnica;
                return "";
            },
            limpiar: function () {
                this.analistas = "";
                this.subdireccionestecnicas = "";
                this.direccionestecnicas = "";
                vm.subdireccionestecnicas = [];
                vm.analistas = [];
            }
        }
        vm.peticion = {
            IdUsuario: usuarioDNP,
            IdObjeto: idTipoProyecto,
            Aplicacion: nombreAplicacionBackbone,
            ListaIdsRoles: sesionServicios.obtenerUsuarioIdsRoles(),
            IdFiltro: constantesBackbone.apiBackBoneEntidadDNP
        };
        vm.peticionobtenercdt = {
            TramiteId: 0,
            NivelId: constantesBackbone.idfaseControlPosteriorTramites,//'E8FC3694-C566-4944-A487-DAA494EB3581'
        };
        vm.estadoListasSeleccionadas = false;
        //vm.btnEnviaDisabled = false;
        vm.listaConcepto = [];
        vm.haypendientesolicitud = true;
        vm.editarConcepto = false;
        vm.listaDatosConceptoPorInstanacia = [];
        vm.instanciasProyectos = undefined;
        vm.habilitaBotones = $sessionStorage.soloLectura ? false : true;
        /**Fin declara variables  */

        /**declara metodos */

        vm.initConcepto = initConcepto;
        vm.abrirpanel = abrirpanel;
        vm.abrirTooltip = abrirTooltip;
        vm.solicitar = solicitar;
        vm.onRecuperar = onRecuperar;
        vm.onEnviar = onEnviar;
        vm.onGuardar = onGuardar;
        vm.onEditar = onEditar;
        vm.onCancelar = onCancelar;

        /**Fin declara metodos */

        /**Registro listas desplegable */

        vm.cargaranalistas = cargaranalistas;
        vm.cargarsubdirecciontecnica = cargarsubdirecciontecnica;
        vm.cambiarEstadoListasSeleccionadas = cambiarEstadoListasSeleccionadas;

        /**Fin Registro listas desplegable */



        //Validaciones
        vm.nombreComponente = undefined;

        /*Metodos*/
        $scope.$watch('vm.tramiteid', function () {
            if (vm.tramiteid !== '' && vm.tramiteid !== undefined && vm.tramiteid !== null) {
                vm.peticionobtenercdt.TramiteId = vm.tramiteid;
                
                comunesServicio.obtenerDatosConceptoPorInstancia($sessionStorage.idInstancia).
                            then(function (resultadodata) {
                                if (resultadodata.data !== null)
                                    vm.listaDatosConceptoPorInstanacia = resultadodata.data;
                            }).then(function (resutaltadoDataProyecto) {
                                cargarSolicitudesConcepto();
                            });

            }

        });


        $scope.$watch('vm.nombrecomponentepaso', function () {
            if (vm.nombrecomponentepaso !== undefined && vm.nombrecomponentepaso !== '') {
                vm.nombreComponente = vm.nombrecomponentepaso;
                vm.notificacionvalidacion({ handler: vm.notificacionValidacionPadre, nombreComponente: vm.nombreComponente, esValido: true });
                //ObtenerSeccionCapitulo();
            }
        });

        function abrirpanel() {
            var acc = document.getElementById('divsolicitudconcepto');
            var i;
            var rotated = false;
            acc.classList.toggle("active");
            var panel = acc.nextElementSibling;
            if (panel.style.maxHeight) {
                panel.style.maxHeight = null;
            } else {
                panel.style.maxHeight = panel.scrollHeight + "px";
            }
            var div = document.getElementById('u4_imgsolicitudconcepto'),
                deg = vm.rotated ? 180 : 0;
            div.style.webkitTransform = 'rotate(' + deg + 'deg)';
            div.style.mozTransform = 'rotate(' + deg + 'deg)';
            div.style.msTransform = 'rotate(' + deg + 'deg)';
            div.style.oTransform = 'rotate(' + deg + 'deg)';
            div.style.transform = 'rotate(' + deg + 'deg)';
            vm.rotated = !vm.rotated;

        }

        function abrirTooltip() {

        }

        function CargarDireccionTecnica() {
            comunesServicio.obtenerListaDireciones(vm.peticion)
                .then(resultado => {
                    vm.direcciontecnicas = resultado.data;
                })
        }

        function cargarsubdirecciontecnica(macroproceso) {
            vm.entidadDestino = undefined;
            if (macroproceso !== undefined && macroproceso !== null && macroproceso !== '') {
                vm.peticion.IdFiltro = macroproceso;
                comunesServicio.obtenerSubDireccionTecnica(vm.peticion)
                    .then(resultado => {
                        vm.subdireccionestecnicas = resultado.data;
                    })
            }
            else {
                vm.subdireccionestecnicas = [];
                vm.analistas = [];
                vm.estadoListasSeleccionadas = false;
            }
        }

        function cargaranalistas(macroproceso) {
            vm.entidadDestino = undefined;
            if (macroproceso !== undefined && macroproceso !== null && macroproceso !== '') {
                vm.peticion.IdFiltro = macroproceso;
                vm.entidadDestino = macroproceso;
                comunesServicio.obtenerAnalistasSubDireccionTecnica(vm.peticion)
                    .then(resultado => {
                        vm.analistas = resultado.data;
                    })
            }
            else {
                vm.analistas = [];
                vm.estadoListasSeleccionadas = false;
            }
        }

        function solicitar() {
            var subdireccion = vm.subdireccionestecnicas.filter(x => x.EntityTypeCatalogOptionId === vm.filtro.subdireccionestecnica);
            var nombreSubdireccion = subdireccion !== undefined && subdireccion.length > 0 ? subdireccion[0].Name : "";
            //Para solicitar el concepto el proyecto debe estar en Control posterior DNO aplicado
            if ($sessionStorage.EstadoDNpAplicado) {
                utilidades.mensajeWarning(
                    "¿Está seguro de continuar?",
                    function funcionContinuar() {
                        var nombreanalista = vm.analistas.filter(x => x.IdUsuarioDnp === vm.filtro.analista);

                        if (nombreanalista != null && nombreanalista.length > 0) {
                            //vm.filtro.analista = vm.analista;
                            vm.peticion.IdFiltro = '{"Id": 0,"TramiteId": ' + $sessionStorage.tramiteId +
                                ',"IdUsuarioDNP": "' + vm.filtro.analista +
                                '","EntityTypeCatalogOptionId":' + vm.filtro.subdireccionestecnica +
                                ',"Activo": true, "FechaCreacion": "' + vm.fechaHoy +
                                '","CreadoPor": "' + vm.peticion.IdUsuario +
                                '","FechaModificacion": "' + vm.fechaHoy +
                                '","ModificadoPor": "' + vm.peticion.IdUsuario + '","Enviado": false' +
                                ',"ParentId":' + vm.filtro.direccionestecnica + '}';

                            comunesServicio.solicitarConcepto(vm.peticion)
                                .then(function (response) {
                                    if (!response.data.Estado) {
                                        utilidades.mensajeError(response.data.Mensaje, false);

                                    }
                                    else if (response.data.Estado) {
                                        var correo = nombreanalista[0].UsuarioCuentas !== undefined && nombreanalista[0].UsuarioCuentas.length > 0 ? nombreanalista[0].UsuarioCuentas[0].Cuenta : "";
                                        var mensaje = "Usuario encargado de entregar concepto técnico:\n " + correo + ".\n Visualice el concepto técnico en la parte inferior de este capitulo.";
                                        RegistrarPermisosAccionPorUsuario();
                                        utilidades.mensajeSuccess(
                                            mensaje,
                                            false,
                                            function funcionContinuar() {
                                                vm.versolicitarconcepto = false;
                                                cargarSolicitudesConcepto();
                                                vm.btnenviadisabled = true;
                                                vm.deshabilitar = true;
                                                vm.subdireccionestecnicas = [];
                                                vm.analistas = [];
                                                vm.filtro.direccionestecnica = "";
                                                //vm.callback({ arg: false });
                                                //vm.callback({ botonDevolver: false, botonSiguiente: true, ocultarDevolver: true });

                                            },
                                            false,
                                            "La solicitud ha sido enviada a la " + nombreSubdireccion);
                                        guardarCapituloModificado();
                                        
                                        trasladosServicio.CrearReasignacionRadicadOrfeo(vm.peticion.IdUsuario, vm.filtro.analista, $sessionStorage.tramiteId);
                                    } else {
                                        swal('', "Error al realizar la operación", 'error');
                                        vm.callback({ arg: true });
                                    }
                                });
                        }

                    },
                    function funcionCancelar(reason) {
                        console.log("reason", reason);
                    },
                    "Aceptar",
                    "Cancelar",
                    "Usted enviará una solicitud de concepto técnico.");
            }
            else {
                utilidades.mensajeError("No se puede solicitar concepto, el proyecto no esta en estado 'Control Posterior DNP Aplicado'", false);
            }

        }

        function cargarSolicitudesConcepto() {
            $scope.$watch(function () {
                vm.listaConcepto = [];
            });
            vm.haypendientesolicitud = false;
            var listatmp = [];
            vm.listaConceptosRespuestaEditar = [];
            comunesServicio.cargarSolicitudesConcepto($sessionStorage.tramiteId).then(function (resultado) {

                if (resultado.data !== null) {
                    var lista = resultado.data;
                    var index = 1;
                    //if (vm.instanciasProyectos !== undefined && vm.instanciasProyectos.length > 0) {
                    //    trasladosServicio.obtenerDatosConceptoPorInstancia(vm.instanciasProyectos[0].InstanciaProyecto).
                    //        then(function (resultadodata) {
                    //            if (resultadodata.data !== null)
                    //                vm.listaDatosConceptoPorInstanacia = resultadodata.data;
                    //        });

                    //}
                    
                    lista.map(function (item) {
                        if (item.Activo) {
                            const existeregistro = listatmp.find(x => x.idUsuarioDNP === item.IdUsuarioDNP);
                            if (existeregistro === undefined) {
                                var envioSolicitud = {};
                                envioSolicitud.tramiteId = item.TramiteId;
                                envioSolicitud.nombreEntidad = item.NombreEntidad;
                                envioSolicitud.nombreUsuario = item.NombreUsuarioDNP;
                                envioSolicitud.fecha = obtenerFechaSinHorasstring(item.FechaCreacion.substr(0, 10).replaceAll('-', '/'));
                                envioSolicitud.correo = item.Correo;
                                envioSolicitud.entityTypeCatalogOptionId = item.EntityTypeCatalogOptionId;
                                envioSolicitud.parentId = item.ParentId;
                                envioSolicitud.idUsuarioDNP = item.IdUsuarioDNP;
                                envioSolicitud.id = item.Id;
                                envioSolicitud.Enviado = item.Enviado;
                                envioSolicitud.numero = 0;
                                envioSolicitud.tieneConcepto = false;
                                envioSolicitud.correoUsuarioQueEnvia = item.NombreUsuarioQueEnvia;
                                envioSolicitud.idUsuarioDNPQueEnvia = item.IdUsuarioDNPQueEnvia;
                                envioSolicitud.fechaEntrega = obtenerFechaSinHorasstring(item.FechaEntrega.substr(0, 10).replaceAll('-', '/'));;
                                envioSolicitud.listaRespuestas = [];
                                envioSolicitud.DatosConceptoPorInstanacia = vm.listaDatosConceptoPorInstanacia;
                                if (!item.Enviado) {
                                    envioSolicitud.estado = 'Pendiente';
                                    envioSolicitud.botonVisible = true;
                                    vm.haypendientesolicitud = true;
                                    vm.deshabilitarconcepto({ estado: true });
                                    vm.deshabilitar = true;
                                    vm.callback({ validacion: null, arg: true, aprueba: false, titulo: '', ocultarDevolver: true });
                                    $sessionStorage.envioSolicitudPendiente = true;
                                }
                                else {
                                    envioSolicitud.estado = 'Entregado';
                                    envioSolicitud.botonVisible = false;
                                    $sessionStorage.envioSolicitudPendiente = false;
                                    
                                }
                                listatmp.push(envioSolicitud);
                                if (envioSolicitud.idUsuarioDNP === $sessionStorage.usuario.permisos.IdUsuarioDNP
                                    && !envioSolicitud.Enviado) {
                                    vm.editarConcepto = true;
                                    vm.deshabilitar = true;
                                    vm.listaConceptosRespuestaEditar.push(envioSolicitud);
                                }
                            }
                            else {
                                if (existeregistro.Enviado !== item.Enviado && !item.Enviado) {
                                    existeregistro.Enviado = false;
                                    if (!item.Enviado) {
                                        existeregistro.estado = 'Pendiente';
                                        existeregistro.botonVisible = true;
                                        vm.haypendientesolicitud = true;
                                        vm.deshabilitar = true;
                                      
                                    }
                                    else {
                                        existeregistro.estado = 'Entregado';
                                        existeregistro.botonVisible = false;
                                        vm.deshabilitar = false;
                                        
                                    }
                                    if (existeregistro.idUsuarioDNP === $sessionStorage.usuario.permisos.IdUsuarioDNP
                                        && !existeregistro.Enviado) {
                                        vm.editarConcepto = true;
                                        vm.deshabilitar = true;
                                        vm.listaConceptosRespuestaEditar.push(existeregistro);
                                    }
                                }


                            }
                        }

                    });
                }

            }).then(function () {
                var cantidad = listatmp.length;

                listatmp.map(function (item) {
                    item.numero = cantidad;
                    cantidad--;
                });
                vm.listaConceptos = listatmp;
                vm.btnenviadisabled = vm.haypendientesolicitud;
               
                if (!vm.editarConcepto)
                    ObtenerConceptoDireccionTecnicaTramite();
                else
                    ObtenerPreguntasConceptoDireccionTecnicaTramite();
            });
        }


        function ObtenerConceptoDireccionTecnicaTramite() {
            comunesServicio.obtenerConceptoDireccionTecnicaTramite(vm.peticionobtenercdt)
                .then(resultado => {
                    vm.listaConceptosRta = resultado.data;
                    vm.listaConceptos.map(function (itemConcepto) {
                        var usuario = 0;
                        if (itemConcepto.estado !== "Pendiente") {
                            var listarespuestas = vm.listaConceptosRta.filter(x => x.Usuario === itemConcepto.idUsuarioDNP);
                            if (listarespuestas != undefined && listarespuestas.length > 0) {
                                $scope.$watch(function () {

                                    itemConcepto.listaRespuestas = listarespuestas;
                                    itemConcepto.tieneConcepto = true;
                                });
                                //itemConcepto.listaRespuestas.map(function (itemrespuesta) {
                                //    itemrespuesta.Respuesta = itemrespuesta === null ? '' : itemrespuesta.Respuesta;
                                //    itemrespuesta.Respuesta = parseInt(itemrespuesta.Respuesta);
                                //});
                                $scope.$watch(function () {
                                    vm.habilitarEditar = true;
                                    vm.habilitarEnviar = true;
                                    vm.habilitarGuardar = true;
                                });

                            }

                        }

                    })


                });

        }

        function onEnviar(usuarioDnp, usuarioDestino) {
            validarCampos();
            if (vm.cumple === true) {
                comunesServicio.enviarConceptoDireccionTecnicaTramite($sessionStorage.tramiteId, $sessionStorage.usuario.permisos.IdUsuarioDNP)
                    .then(function () {
                        comunesServicio.eliminarPermisos($sessionStorage.usuario.permisos.IdUsuarioDNP, $sessionStorage.tramiteId, 'TEC').
                            then(function (respuestaEliminar) {
                                if (respuestaEliminar !== null && respuestaEliminar.data !== null) {
                                    if (respuestaEliminar.data === 1) {
                                        $scope.$watch(function () {
                                            vm.habilitarGuardar = false;
                                            vm.habilitarEditar = false;
                                            vm.habilitarEnviar = false;
                                        });
                                        trasladosServicio.CrearReasignacionRadicadOrfeo(usuarioDnp, usuarioDestino, $sessionStorage.tramiteId);

                                        utilidades.mensajeSuccess(
                                            "Usted será redirigido a la página de inicio de la PIIP.",
                                            false,
                                            function funcionContinuar() {
                                                $location.url("/proyectos/pl");
                                            },
                                            false,
                                            "El concepto técnico fue enviado con  éxito");

                                    }
                                }
                                else {
                                    utilidades.mensajeError('Error al enviar concepto.');
                                }
                            });


                    });
            } else {
                utilidades.mensajeError('El formulario no esta diligenciado en su totalidad');
            }
        }

        function onGuardar() {
            validarCampos();
            if (vm.cumple === true) {
                //if (vm.listaConceptosRespuestaEditar[0].listaRespuestas[0].InstanciaId == null) {
                //    CargarParametrosConcepto();
                //}
                //else {
                vm.listaConceptosRespuestaEditar[0].listaRespuestas.forEach(con => {
                    con.Usuario = usuarioDNP;
                });
                //}
                comunesServicio.guardarConceptoDireccionTecnicaTramite(vm.listaConceptosRespuestaEditar[0].listaRespuestas)
                    .then(function (response) {
                        if (response.data) {
                            utilidades.mensajeSuccess(
                                "Proceda a envíar las respuestas.",
                                false,
                                function funcionContinuar() {
                                    vm.listaConceptosRespuestaEditar[0].listaRespuestas = [];
                                    ObtenerPreguntasConceptoDireccionTecnicaTramite();
                                    $scope.$watch(function () {
                                        vm.habilitarEditar = true;
                                        vm.habilitarGuardar = true;
                                        vm.activo = false;
                                        vm.habilitarEnviar = false;
                                        vm.habilitarCancelar = false;
                                        vm.diligencia = true;
                                    });
                                },
                                false,
                                "Los datos fueron  guardados con éxito'");
                            //ObtenerSolicitarConcepto();

                            //ReasignarRadicadoORFEO(usuarioDNP, "-")


                            //CargarConcepto();
                        } else {
                            swal('', "", 'Es necesario diligenciar todos los campos para guardar.');
                            //vm.callback({ arg: true });
                        }
                    });
            }
            else {
                utilidades.mensajeError('', false, 'Es necesario diligenciar todos los campos para guardar.');
                //swal('El formulario no esta diligenciado en su totalidad', 'Revise las campos nuevamente.', 'warning');
            }
        }

        function onEditar() {
            $scope.$watch(function () {
                vm.diligencia = false;
                vm.habilitarGuardar = false;
                vm.habilitarEditar = false;
                vm.habilitarCancelar = true;
                vm.habilitarEnviar = true;
            });

        }

        function onCancelar() {
            utilidades.mensajeWarning(
                "¿Está seguro de continuar?",
                function funcionContinuar() {

                    $timeout(function () {
                        utilidades.mensajeSuccess(
                            "",
                            false,
                            function funcionContinuar() {
                                vm.listaConceptosRespuestaEditar[0].listaRespuestas = [];
                                ObtenerPreguntasConceptoDireccionTecnicaTramite();

                            },
                            false,
                            "Se ha cancelado la edición.");

                        //swal({
                        //    title: "<span style='color:#069169'>Se ha cancelado la edición.<span>",
                        //    text: "<div style='padding-bottom:2rem !important;'></div><button type='button' class='swal2-close' aria-label='Close this dialog' style='display: flex;'>×</button>",
                        //    type: "success",
                        //    showCancelButton: false,
                        //    confirmButtonText: "Aceptar",
                        //    cancelButtonText: "Cancelar",
                        //    closeOnConfirm: true,
                        //    closeOnCancel: true,
                        //    customClass: 'sweet-alertTittleSucces',
                        //    html: true
                        //});

                    }, 200);
                },
                function funcionCancelar(reason) {
                    console.log("reason", reason);
                },
                "Aceptar",
                "Cancelar",
                "Los posibles datos que haya  diligenciado se perderán.");
        }

        function validarCampos() {
            vm.cumple = true;
            var i = 0
            vm.listaConceptosRespuestaEditar[0].listaRespuestas.forEach(preguntas => {
                // if (i == 0) {
                if (preguntas.EsPreguntaAbierta == 1 && (preguntas.ObservacionPregunta == null || preguntas.ObservacionPregunta == '')) {
                    vm.cumple = false;
                    i = 1;
                }
                if (preguntas.EsPreguntaAbierta == 2) {
                    if (preguntas.ObservacionPregunta == null || preguntas.ObservacionPregunta == '') {
                        vm.cumple = false;
                        i = 1;
                    }
                    //else {
                    //    vm.listaConceptosRespuestaEditar[0].Observaciones = vm.ConceptoDireccionTecnicaTramite[1].Observaciones;
                    //}
                    var tempIsNaN = Number.isNaN(preguntas.Respuesta);

                    if (tempIsNaN || preguntas.Respuesta == '0' || preguntas.Respuesta == null) {
                        vm.cumple = false;
                    }
                }
                // }
            });

            return vm.cumple
        }

        function ObtenerPreguntasConceptoDireccionTecnicaTramite() {
            var listarespuestastmp = [];
            var listaRespuestas = [];
            comunesServicio.obtenerConceptoDireccionTecnicaTramite(vm.peticionobtenercdt)
                .then(resultado => {
                    listarespuestastmp = resultado.data;
                    var usuario = 0;
                    if (listarespuestastmp.length > 0) {
                        listarespuestastmp.forEach(dt => {
                            if (dt.Usuario === $sessionStorage.usuario.permisos.IdUsuarioDNP) {
                                usuario = dt.Usuario;
                                listaRespuestas.push(dt);
                                //recuperar(false);
                            }
                        });
                        if (listaRespuestas.length > 0) {
                            $scope.$watch(function () {
                                vm.habilitarEditar = true;
                                vm.habilitarEnviar = false;
                                vm.habilitarGuardar = true;
                                vm.habilitarCancelar = false;
                                vm.diligencia = true;
                            });
                        }
                    }
                }).then(function (respuesta) {
                    if (listaRespuestas.length === 0) {
                        listarespuestastmp.map(function (dt) {
                            if (dt.Usuario === null) {
                                dt.Usuario = $sessionStorage.usuario.permisos.IdUsuarioDNP;
                                CargarParametrosConcepto(dt);
                                listaRespuestas.push(dt);

                            }

                        });
                        if (listaRespuestas.length === 0) {
                            listarespuestastmp.forEach(dt => {
                                var index = (listaRespuestas.findIndex(x => x.PreguntaId === dt.PreguntaId))
                                if (index < 0) {
                                    dt.ObservacionPregunta = null;
                                    dt.Respuesta = '';
                                    CargarParametrosConcepto(dt);
                                    listaRespuestas.push(dt);
                                }
                                //recuperar(false);
                            })

                        }


                    }
                    if (listaRespuestas[0].ObservacionPregunta !== '' &&
                        listaRespuestas[0].ObservacionPregunta !== null &&
                        listaRespuestas[0].ObservacionPregunta !== null) {
                        $scope.$watch(function () {
                            vm.listaConceptosRespuestaEditar[0].listaRespuestas = listaRespuestas;
                            vm.habilitarEditar = true;
                            vm.habilitarEnviar = false;
                            vm.habilitarGuardar = true;
                            vm.habilitarCancelar = false;
                            vm.diligencia = true;
                        });
                    }
                    else {
                        $scope.$watch(function () {
                            vm.listaConceptosRespuestaEditar[0].listaRespuestas = listaRespuestas;
                            vm.habilitarEditar = true;
                            vm.habilitarEnviar = true;
                            vm.habilitarGuardar = true;
                            vm.habilitarCancelar = false;
                            vm.diligencia = true;
                        });
                    }

                });
        }

        function RegistrarPermisosAccionPorUsuario() {

            vm.listadoUsuarios = [{
                IdUsuarioDNP: vm.filtro.seleccionado,
                NombreUsuario: vm.peticion.IdUsuario,
                IdRol: constantesBackbone.idRControlPosteriorDireccionesTecnicas,
                NombreRol: 'R_Concepto Tecnico',
                IdEntidad: 'Concepto Tecnico',
                NombreEntidad: 'Concepto Tecnico',
                IdEntidadMGA: vm.entidadDestino,
            }];

            vm.RegistrarPermisosAccionDto = {
                ObjetoNegocioId: $sessionStorage.tramiteId,
                IdAccion: $sessionStorage.idAccion,
                IdInstancia: $sessionStorage.idInstancia,
                EntityTypeCatalogOptionId: vm.entidadDestino,
                listadoUsuarios: vm.listadoUsuarios,
            };

            comunesServicio.registrarPermisosAccionPorUsuario(vm.RegistrarPermisosAccionDto).then(function (response) {

            });
        }

        function onRecuperar(envioSolicitud) {
            var subdireccion = vm.subdireccionestecnicas.filter(x => x.EntityTypeCatalogOptionId === vm.filtro.subdireccionestecnica);
            var nombreSubdireccion = subdireccion !== undefined && subdireccion.length > 0 ? subdireccion[0].Name : "";
            if (envioSolicitud.Enviado === false) {
                vm.peticion.IdFiltro = '{"Id":' + envioSolicitud.id +
                    ',"TramiteId": ' + $sessionStorage.tramiteId +
                    ',"IdUsuarioDNP": "' + envioSolicitud.idUsuarioDNP +
                    '","EntityTypeCatalogOptionId":' + envioSolicitud.entityTypeCatalogOptionId +
                    ',"Activo": false, "FechaCreacion": "' + vm.fechaHoy +
                    '","CreadoPor": "' + vm.peticion.IdUsuario +
                    '","FechaModificacion": "' + vm.fechaHoy +
                    '","ModificadoPor": "' + vm.peticion.IdUsuario + '","Enviado": false' +
                    ',"ParentId":' + envioSolicitud.parentId + '}';

                utilidades.mensajeWarning(
                    "Esto cancelará la solicitud para obtener un concepto por parte de la " + nombreSubdireccion + ". ¿Está seguro de continuar?",
                    function funcionContinuar() {
                        comunesServicio.solicitarConcepto(vm.peticion)
                            .then(function () {
                                comunesServicio.eliminarPermisos(envioSolicitud.idUsuarioDNP, $sessionStorage.tramiteId, 'TEC').
                                    then(function (respuestaEliminar) {
                                        if (respuestaEliminar !== null && respuestaEliminar.data !== null) {
                                            if (respuestaEliminar.data === 1) {
                                                utilidades.mensajeSuccess("La solicitud del concepto técnico ha sido recuperada",
                                                    false,
                                                    function funcionContinuar() {
                                                        vm.versolicitarconcepto = true;
                                                        vm.btnenviadisabled = false;
                                                        vm.deshabilitar = false;
                                                        vm.deshabilitarconcepto({ estado: false });
                                                        vm.subdireccionestecnicas = [];
                                                        vm.analistas = [];
                                                        vm.estadoListasSeleccionadas = false;
                                                        vm.filtro.direccionestecnica = "";
                                                        cargarSolicitudesConcepto();
                                                        vm.callback({ botonDevolver: false, botonSiguiente: true, ocultarDevolver: false });
                                                    },
                                                    false);
                                                eliminarCapitulosModificados();
                                                
                                                trasladosServicio.CrearReasignacionRadicadOrfeo(envioSolicitud.idUsuarioDNP, envioSolicitud.idUsuarioDNPQueEnvia, $sessionStorage.tramiteId);
                                                vm.callback({ botonDevolver: false, botonSiguiente: true, ocultarDevolver: true });
                                            } else {
                                                swal('', "Error al realizar la operación", 'error');
                                                //vm.callback({ arg: false });
                                            }
                                        }
                                    });
                            });
                    },
                    function funcionCancelar(reason) {
                        console.log("reason", reason);
                    },
                    "Aceptar",
                    "Cancelar",
                    "La solicitud del concepto técnico será recuperada. ");

            }
        }

        function CargarParametrosConcepto(preguntas) {

            preguntas.InstanciaId = constantesBackbone.idfaseControlPosteriorTramites;
            preguntas.FormularioId = '00000000-0000-0000-0000-000000000000';
            preguntas.CR = 1;
            preguntas.AgregarRequisitos = false;
            preguntas.Usuario = usuarioDNP;//vm.concepto[0].IdUsuarioDNP;
            preguntas.Fecha = vm.fechaHoy;
            preguntas.Cumple = true;
            preguntas.TramiteId = $sessionStorage.tramiteId;
            preguntas.Observaciones = '';//vm.ConceptoDireccionTecnicaTramite[1].Observaciones;
            if (preguntas.EsPreguntaAbierta == 1) {
                preguntas.ObservacionPregunta = preguntas.ObservacionPregunta;
                preguntas.Respuesta = "";
            }
            if (preguntas.EsPreguntaAbierta == 2) {
                preguntas.Respuesta = preguntas.Respuesta;
                preguntas.ObservacionPregunta = "";
            }
            return pregunta;
        }



        function cambiarEstadoListasSeleccionadas(valor) {
            if (valor !== undefined && valor !== null && valor !== '')
                vm.estadoListasSeleccionadas = true;
            else
                vm.estadoListasSeleccionadas = false;
        }

        function formatearFecha(fecha) {
            let fechaString = fecha.toISOString();
            return fechaString.substring(0, 19);
        }

        function obtenerFechaSinHoras(fecha) {
            return new Date(Date.UTC(fecha.getFullYear(), fecha.getMonth(), fecha.getDate(), fecha.getHours(), fecha.getMinutes(), fecha.getSeconds()));
        }

        function obtenerFechaSinHorasstring(fecha) {
            return fecha.substring(8, 10) + '/' + fecha.substring(5, 7) + '/' + fecha.substring(0, 4);
        }

        /*Fin Metodos*/


        function initConcepto() {
            vm.listaConceptos = [];

            comunesServicio.obtenerDetallesTramite($sessionStorage.idObjetoNegocio).then(function (result) {
                var x = result.data;
                if (x != null) {
                    vm.TramiteId = x.TramiteId;
                    $sessionStorage.tramiteId = x.TramiteId;

                }
                CargarDireccionTecnica();
                vm.existeconcepto = false;
                
            });

        }



        //para guardar los capitulos modificados y que se llenen las lunas
        function guardarCapituloModificado() {
            ObtenerSeccionCapitulo();
            var data = {
                //ProyectoId: 0,//$sessionStorage.InstanciaSeleccionada.ProyectoId,
                Justificacion: "",
                SeccionCapituloId: vm.seccionCapitulo,
                InstanciaId: $sessionStorage.idInstancia,
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


        function eliminarCapitulosModificados() {
            ObtenerSeccionCapitulo();
            var data = {
                //ProyectoId: 0,//$sessionStorage.InstanciaSeleccionada.ProyectoId,
                Justificacion: "",
                SeccionCapituloId: vm.seccionCapitulo,
                InstanciaId: $sessionStorage.idInstancia,

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


        /* ------------------------ Validaciones ---------------------------------*/

        vm.limpiarErrores = function (errores) {
            var campoObligatorioJustificacion = document.getElementById(vm.nombreComponente + "-pregunta-error");
            if (campoObligatorioJustificacion != undefined) {
                campoObligatorioJustificacion.innerHTML = "";
                campoObligatorioJustificacion.classList.add('hidden');
            }



        }

        vm.notificacionValidacionPadre = function (errores) {
            vm.limpiarErrores(errores);
            var isValid = true;
            if (errores != undefined) {
                var erroresRelacionconlapl = errores.find(p => (p.Seccion + p.Capitulo) == vm.nombreComponente);
                if (erroresRelacionconlapl != undefined) {
                    var erroresJson = erroresRelacionconlapl.Errores == "" ? [] : JSON.parse(erroresRelacionconlapl.Errores);
                    isValid = (erroresJson == null || erroresJson.length == 0);
                    if (!isValid) {
                        erroresJson[vm.nombreComponente].forEach(p => {

                            if (vm.errores[p.Error] != undefined) vm.errores[p.Error](p.Descripcion);
                        });
                    }

                }
                vm.notificacionestado({ nombreComponente: vm.nombreComponente, esValido: isValid });

            }
        }



        vm.validarValoresVigenciaSolicitarconceptoelaborarconcepto = function (errores) {
            var campoObligatorioJustificacion = document.getElementById(vm.nombreComponente + "-pregunta-error");
            if (campoObligatorioJustificacion != undefined) {
                campoObligatorioJustificacion.innerHTML = "<span style='margin-left:1.5rem;' class='d-inline-block ico-advertencia'></span><span>" + errores + "</span>";
                campoObligatorioJustificacion.classList.remove('hidden');
            }
        }


        vm.errores = {
            'VFO008': vm.validarValoresVigenciaSolicitarconceptoelaborarconcepto,

        }

        /* ------------------------ FIN--------*/

    }

    angular.module('backbone').component('solicitudConceptoFormulario', {

        templateUrl: "src/app/formulario/ventanas/comun/solicitarConcepto/solicitudConceptoFormulario.html",
        controller: solicitudConceptoFormulario,
        controllerAs: "vm",
        bindings: {
            callback: '&',
            notificacionvalidacion: '&',
            notificacionestado: '&',
            guardadoevent: '&',
            tipotramiteid: '@',
            tramiteid: '@',
            nivel: '@',
            rol: '@',
            section: '@',
            nombrecomponentepaso: '@',
            deshabilitarconcepto: '&',
            deshabilitar: '=',
            
          
        }
    });


})();