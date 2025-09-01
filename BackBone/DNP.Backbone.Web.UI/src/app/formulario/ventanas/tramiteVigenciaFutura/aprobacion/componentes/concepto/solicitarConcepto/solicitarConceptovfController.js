(function () {
    'use strict';

    solicitarConceptovfController.$inject = [
        '$scope',
        '$sessionStorage',
        'utilidades',
        'solicitarconceptoServicio',
        'sesionServicios',
        'trasladosServicio',
        'constantesBackbone',
        'conceptodirecciontecnicaServicio',
        'justificacionCambiosServicio',
        '$routeParams',
        '$timeout',
        '$location',
        '$window',
        '$http'
    ];





    function solicitarConceptovfController(
        $scope,
        $sessionStorage,
        utilidades,
        solicitarconceptoServicio,
        sesionServicios,
        trasladosServicio,
        constantesBackbone,
        conceptodirecciontecnicaServicio,
        justificacionCambiosServicio,
        $routeParams,
        $timeout,
        $location,
        $window,
        $http

    ) {
        var vm = this;
        vm.initConcepto = initConcepto;
        vm.user = {};
        vm.lang = "es";
        vm.EntityTypeCatalogOptionId = 0;
        vm.solicitar = solicitar;
        vm.existeconcepto = false;
        vm.lbldirecciontecnica = "";
        vm.lblsubdirecciontecnica = "";
        vm.lblanalista = "";
        vm.cumple = false;
        vm.salir = salir;
        vm.btnEnviaDisabled = false;
        vm.editarConcepto = false;
        vm.onEnviar = onEnviar;
        vm.onGuardar = onGuardar;
        vm.onEditar = onEditar;
        vm.onCancelar = onCancelar;
        vm.haypendientesolicitud = false;
        vm.habilitarGuardar = false;
        vm.habilitarEditar = true;
        vm.habilitarEnviar = false;
        vm.habilitarCancelar = false;
        vm.diligencia = false;
        vm.listarespuestastemporal = [];

        //Validaciones
        vm.nombreComponente = "solicitarconceptoelaborarconcepto";

        /*---*/
        vm.cargaranalistas = cargaranalistas;
        vm.analistas = [];
        vm.filtro = {
            direccionestecnica: "",
            subdireccionestecnica: "",
            analista: "",
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
                vm.analistas = [];
            }
        }

        vm.analista = '';
        vm.abrirTooltip = abrirTooltip;
        /*----*/



        vm.peticion = {
            IdUsuario: usuarioDNP,
            IdObjeto: idTipoProyecto,
            Aplicacion: nombreAplicacionBackbone,
            ListaIdsRoles: sesionServicios.obtenerUsuarioIdsRoles(),
            IdFiltro: vm.EntityTypeCatalogOptionId,
        };
        vm.concepto = {
            Id: 0,
            EntityTypeCatalogOptionId: 0,
            IdUsuarioDNP: "",
            ParentId: 0
        };

        vm.peticionobtenercdt = {
            TramiteId: 0,
            NivelId: constantesBackbone.idfaseControlPosteriorTramites,//'E8FC3694-C566-4944-A487-DAA494EB3581'
        };

        vm.isdisabled = true;
        vm.listaConceptos = [];
        vm.listaConceptosRespuestaEditar = [];
        vm.onRecuperar = onRecuperar;
        vm.fechaHoy = formatearFecha(obtenerFechaSinHoras(new Date()));



        function initConcepto() {
            vm.listaConceptos = [];
            vm.listaConceptosRespuestaEditar = [];
            trasladosServicio.obtenerDetallesTramite($sessionStorage.idObjetoNegocio).then(function (result) {
                var x = result.data;
                if (x != null) {
                    vm.TramiteId = x.TramiteId;
                    $sessionStorage.tramiteId = x.TramiteId;
                    vm.peticionobtenercdt.TramiteId = $sessionStorage.tramiteId;

                }
                cargarSolicitudesConcepto();
                cargaranalistas();
                vm.existeconcepto = false;
                //Validaciones
                var cumpleRolConcepto = $sessionStorage.usuario.roles.filter(x => x.IdRol.includes(constantesBackbone.idRControlPosteriorDireccionesTecnicas.toLowerCase()));
                if (cumpleRolConcepto.length > 0)
                    vm.callback({ botonDevolver: true, botonSiguiente: false, ocultarDevolver: true });
                vm.notificacionvalidacion({ handler: vm.notificacionValidacionPadre, nombreComponente: vm.nombreComponente, esValido: true });

            });

        }

        function abrirTooltip() {

        }

        function cargarSolicitudesConcepto() {
            $scope.$watch(function () {
                vm.listaConcepto = [];
            });
            vm.haypendientesolicitud = false;
            var listatmp = [];

            solicitarconceptoServicio.cargarSolicitudesConcepto($sessionStorage.tramiteId).then(function (resultado) {

                if (resultado.data != null) {

                    var lista = resultado.data;
                    var index = 1;
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
                                if (!item.Enviado) {
                                    envioSolicitud.estado = 'Pendiente';
                                    envioSolicitud.botonVisible = true;
                                    vm.haypendientesolicitud = true;
                                    vm.callback({ botonDevolver: true, botonSiguiente: true, ocultarDevolver: true });
                                }
                                else {
                                    envioSolicitud.estado = 'Entregado';
                                    envioSolicitud.botonVisible = false;
                                }
                                listatmp.push(envioSolicitud);
                                if (envioSolicitud.idUsuarioDNP === $sessionStorage.usuario.permisos.IdUsuarioDNP
                                    && !envioSolicitud.Enviado) {
                                    vm.editarConcepto = true;
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
                                    }
                                    else {
                                        existeregistro.estado = 'Entregado';
                                        existeregistro.botonVisible = false;
                                    }
                                    if (existeregistro.idUsuarioDNP === $sessionStorage.usuario.permisos.IdUsuarioDNP
                                        && !existeregistro.Enviado) {
                                        vm.editarConcepto = true;
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
                vm.btnEnviaDisabled = vm.haypendientesolicitud;
                if (!vm.editarConcepto)
                    ObtenerConceptoDireccionTecnicaTramite();
                else
                    ObtenerPreguntasConceptoDireccionTecnicaTramite();
            });
        }

        function cargaranalistas() {
            solicitarconceptoServicio.ObtenerDireccionTecnica(vm.peticion).then(function (resultado) {
                if (resultado.data != null && resultado.data.length > 0) {
                    vm.filtro.direccionestecnica = resultado.data[0].EntityTypeCatalogOptionId;
                    vm.peticion.IdFiltro = resultado.data[0].ParentId;
                    solicitarconceptoServicio.ObtenerSubDireccionTecnica(vm.peticion)
                        .then(resultado => {
                            if (resultado.data != null && resultado.data.length > 0) {
                                vm.filtro.subdireccionestecnica = resultado.data[0].EntityTypeCatalogOptionId;
                                vm.peticion.IdFiltro = resultado.data[0].EntityTypeCatalogOptionId;
                                solicitarconceptoServicio.ObtenerAnalistasSubDireccionTecnica(vm.peticion)
                                    .then(resultado => {
                                        vm.analistas = resultado.data;
                                    })
                            }

                        })
                }
            });

        }



        function solicitar() {
            //Para solicitar el concepto el proyecto debe estar en Control posterior DNO aplicado
            if ($sessionStorage.EstadoDNpAplicado) {
                var nombreanalista = vm.analistas.filter(x => x.IdUsuarioDnp === vm.analista);
                if (nombreanalista != null && nombreanalista.length > 0) {
                    vm.filtro.analista = vm.analista;
                    vm.peticion.IdFiltro = '{"Id": 0,"TramiteId": ' + $sessionStorage.tramiteId +
                        ',"IdUsuarioDNP": "' + vm.analista +
                        '","EntityTypeCatalogOptionId":' + vm.filtro.subdireccionestecnica +
                        ',"Activo": true, "FechaCreacion": "' + vm.fechaHoy +
                        '","CreadoPor": "' + vm.peticion.IdUsuario +
                        '","FechaModificacion": "' + vm.fechaHoy +
                        '","ModificadoPor": "' + vm.peticion.IdUsuario + '","Enviado": false' +
                        ',"ParentId":' + vm.filtro.direccionestecnica + '}';

                    solicitarconceptoServicio.SolicitarConcepto(vm.peticion)
                        .then(function (response) {
                            if (!response.data.Estado) {
                                utilidades.mensajeError(response.data.Mensaje, false);

                            }
                            else if (response.data.Estado) {
                                var mensaje = "Usuario encargado de entregar concepto técnico:\n " + nombreanalista[0].Nombre + ".\n Visualice el concepto técnico en la parte inferior de este capitulo.";
                                RegistrarPermisosAccionPorUsuario();
                                utilidades.mensajeSuccess(
                                    mensaje,
                                    false,
                                    function funcionContinuar() {
                                        vm.versolicitarconcepto = false;
                                        cargarSolicitudesConcepto();
                                        //vm.callback({ arg: false });
                                        vm.callback({ botonDevolver: false, botonSiguiente: true, ocultarDevolver: true });

                                    },
                                    false,
                                    "El trámite fue enviado a la Subdirección técnica");
                                guardarCapituloModificado();
                                trasladosServicio.CrearReasignacionRadicadOrfeo(vm.peticion.IdUsuario, vm.analista, $sessionStorage.tramiteId);
                            } else {
                                swal('', "Error al realizar la operación", 'error');
                                vm.callback({ arg: true });
                            }
                        });
                }
            }
            else {
                utilidades.mensajeError("No se puede solicitar concepto, el proyecto no esta en estado 'Control Posterior DNP Aplicado'", false);
            }

        }

        function onRecuperar(envioSolicitud) {
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
                    "Esto cancelará la solicitud para obtener un concepto por parte de la Subdirección de Crédito. ¿Está seguro de continuar?",
                    function funcionContinuar() {
                        solicitarconceptoServicio.SolicitarConcepto(vm.peticion)
                            .then(function () {
                                solicitarconceptoServicio.eliminarPermisos(envioSolicitud.idUsuarioDNP, $sessionStorage.tramiteId, 'TEC').
                                    then(function (respuestaEliminar) {
                                        if (respuestaEliminar !== null && respuestaEliminar.data !== null) {
                                            if (respuestaEliminar.data === 1) {
                                                utilidades.mensajeSuccess("La solicitud del concepto técnico ha sido recuperada",
                                                    false,
                                                    function funcionContinuar() {
                                                        vm.versolicitarconcepto = true;
                                                        vm.btnEnviaDisabled = false;
                                                        cargarSolicitudesConcepto();
                                                        vm.callback({ botonDevolver: false, botonSiguiente: true, ocultarDevolver: false });
                                                    },
                                                    false);
                                                eliminarCapitulosModificados();
                                                trasladosServicio.CrearReasignacionRadicadOrfeo(envioSolicitud.idUsuarioDNP, envioSolicitud.idUsuarioDNPQueEnvia, $sessionStorage.tramiteId);
                                                vm.callback({ arg: true });
                                            } else {
                                                swal('', "Error al realizar la operación", 'error');
                                                vm.callback({ arg: false });
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


        function ObtenerConceptoDireccionTecnicaTramite() {
            solicitarconceptoServicio.obtenerConceptoDireccionTecnicaTramite(vm.peticionobtenercdt)
                .then(resultado => {
                    vm.listaConceptosRta = resultado.data;
                    vm.listaConceptos.map(function (itemConcepto) {
                        var usuario = 0;
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



                    })


                });
            //.then(function (response) {
            //    if (vm.listaConceptos != null || vm.listaConceptos.length === 0) {
            //        vm.listaConceptos.map(function (itemConcepto) {
            //            itemConcepto.listaRespuestas = vm.listaConceptosRta;
            //            itemConcepto.tieneConcepto = false;
            //        });
            //        //$scope.$watch(function () {
            //            vm.habilitarEditar = true;
            //            vm.habilitarEnviar = false;
            //            vm.habilitarGuardar = true;
            //            vm.habilitarCancelar = true;
            //        //});

            //    }
            //});
        }

        function ObtenerPreguntasConceptoDireccionTecnicaTramite() {
            var listarespuestastmp = [];
            var listaRespuestas = [];
            solicitarconceptoServicio.obtenerConceptoDireccionTecnicaTramite(vm.peticionobtenercdt)
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

        function onEnviar(usuarioDnp, usuarioDestino) {
            validarCampos();
            if (vm.cumple === true) {
                solicitarconceptoServicio.enviarConceptoDireccionTecnicaTramite($sessionStorage.tramiteId, $sessionStorage.usuario.permisos.IdUsuarioDNP)
                    .then(function () {
                        solicitarconceptoServicio.eliminarPermisos($sessionStorage.usuario.permisos.IdUsuarioDNP, $sessionStorage.tramiteId, 'TEC').
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
                solicitarconceptoServicio.guardarConceptoDireccionTecnicaTramite(vm.listaConceptosRespuestaEditar[0].listaRespuestas)
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

        function RegistrarPermisosAccionPorUsuario() {

            vm.listadoUsuarios = [{
                IdUsuarioDNP: vm.filtro.seleccionado,
                NombreUsuario: vm.peticion.IdUsuario,
                IdRol: constantesBackbone.idRControlPosteriorDireccionesTecnicas,
                NombreRol: 'R_Concepto Tecnico',
                IdEntidad: 'Concepto Tecnico',
                NombreEntidad: 'Concepto Tecnico',
                IdEntidadMGA: $sessionStorage.idEntidad,
            }];

            vm.RegistrarPermisosAccionDto = {
                ObjetoNegocioId: $sessionStorage.tramiteId,
                IdAccion: $sessionStorage.idAccion,
                IdInstancia: $sessionStorage.idInstancia,
                EntityTypeCatalogOptionId: $sessionStorage.idEntidad,
                listadoUsuarios: vm.listadoUsuarios,
            };

            trasladosServicio.RegistrarPermisosAccionPorUsuario(vm.RegistrarPermisosAccionDto).then(function (response) {

            });
        }

        function recuperar(valor) {
            if (valor) {
                $scope.$watch(function () {
                    vm.listarespuestastemporal.map(function (item) {
                        var index = vm.listaConceptosRespuestaEditar[0].listaRespuestas.findIndex(x => x.PreguntaId === item.PreguntaId);
                        vm.listaConceptosRespuestaEditar[0].listaRespuestas[index].Observaciones = angular.copy(item.Observaciones);
                        vm.listaConceptosRespuestaEditar[0].listaRespuestas[index].ObservacionPregunta = angular.copy(item.ObservacionPregunta);
                        vm.listaConceptosRespuestaEditar[0].listaRespuestas[index].Respuesta = angular.copy(item.Respuesta);
                    });
                });
            }
            else {
                vm.listarespuestastemporal = [];
                vm.listaConceptosRespuestaEditar[0].listaRespuestas.map(function (item) {
                    var temporal = {};
                    temporal.Observaciones = angular.copy(item.Observaciones);
                    temporal.ObservacionPregunta = angular.copy(item.ObservacionPregunta);
                    temporal.Respuesta = angular.copy(item.Respuesta);
                    temporal.PreguntaId = angular.copy(item.PreguntaId);
                    vm.listarespuestastemporal.push(temporal);
                });
            }
        }

        function ReasignarRadicadoORFEO(usuarioIdOrigen, usuarioIdDestino) {

            vm.UsuarioOrigen = {
                Login: usuarioIdOrigen
            };

            vm.UsuarioDestino = {
                Login: usuarioIdDestino
            };

            vm.ReasignacionRadicadoDto = {
                UsuarioOrigen: vm.UsuarioOrigen,
                UsuarioDestino: vm.UsuarioDestino,
                TramiteId: $sessionStorage.tramiteId,
            };

            trasladosServicio.ReasignarRadicadoORFEO(vm.ReasignacionRadicadoDto).then(function (response) {

            });
        }

        function obtenerFechaSinHoras(fecha) {
            return new Date(Date.UTC(fecha.getFullYear(), fecha.getMonth(), fecha.getDate(), fecha.getHours(), fecha.getMinutes(), fecha.getSeconds()));
        }

        function formatearFecha(fecha) {
            let fechaString = fecha.toISOString();
            return fechaString.substring(0, 19);
        }

        function obtenerFechaSinHorasstring(fecha) {
            return fecha.substring(8, 10) + '/' + fecha.substring(5, 7) + '/' + fecha.substring(0, 4);
        }

        function salir() {
            vm.versolicitarconcepto = false;
            vm.callback({ arg: false });
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
            const span = document.getElementById('id-capitulo-solicitarconceptoelaborarconcep');
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
                campoObligatorioJustificacion.innerHTML = "<span class='d-inline-block ico-advertencia'></span><span>" + errores + "</span>";
                campoObligatorioJustificacion.classList.remove('hidden');
            }
        }


        vm.errores = {
            'VFO008': vm.validarValoresVigenciaSolicitarconceptoelaborarconcepto,

        }

        /* ------------------------ FIN--------*/

    }

    angular.module('backbone').component('solicitarConceptovf', {

        templateUrl: "src/app/formulario/ventanas/tramiteVigenciaFutura/aprobacion/componentes/concepto/solicitarConcepto/solicitarConceptovf.html",
        controller: solicitarConceptovfController,
        controllerAs: "vm",
        bindings: {
            disabled: '=',
            callback: '&',
            notificacionvalidacion: '&',
            notificacionestado: '&',
            guardadocomponent: '&',
            guardadoevent: '&',
        }
    });


})();