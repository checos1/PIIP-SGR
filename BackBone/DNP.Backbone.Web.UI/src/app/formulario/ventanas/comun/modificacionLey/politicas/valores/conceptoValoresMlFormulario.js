var conceptoValoresMlCtrl = null;

(function () {
    'use strict';
    angular.module('backbone').controller('conceptoValoresMl', conceptoValoresMl);
    conceptoValoresMl.$inject = [
        '$scope',
        '$sessionStorage',
        '$timeout',
        'focalizacionAjustesServicio',
        'trasladosServicio',
        'sesionServicios',
        'archivoServicios',
        'solicitarconceptoServicio',
        'serviciosComponenteNotificaciones',
        'comunesServicio',
        'constantesBackbone',
        'utilidades'
    ];

    function conceptoValoresMl(
        $scope,
        $sessionStorage,
        $timeout,
        focalizacionAjustesServicio,
        trasladosServicio,
        sesionServicios,
        archivoServicios,
        solicitarconceptoServicio,
        serviciosComponenteNotificaciones,
        comunesServicio,
        constantesBackbone,
        utilidades) {
        var vm = this;
        conceptoValoresMlCtrl = vm;
        //variables
        vm.lang = "es";
        vm.TabActivo = 1;
        vm.adjuntarArchivo = adjuntarArchivo;
        vm.limpiarArchivo = limpiarArchivo;
        vm.nombrearchivoConcepto = "Seleccione Archivo";
        vm.listaTipoArchivos = [];
        vm.listaArchivos = [];
        vm.archivoDescarga = [];
        vm.archivo = undefined;
        vm.coleccion = "tramites";
        vm.rolValidadorPolitica = false;
        vm.PreguntasEnvioPoliticaSubDireccion = [];
        vm.PreguntasEnvioPoliticaSubDireccionOrigen = [];
        vm.ResumenSolicitudConcepto = [];
        vm.TienePoliticas = 0;
        vm.nombreComponente = "conceptoValoresMlFormulario";
        vm.activar = false;
        vm.EnviosSubdireccion = [];
        vm.habilitaBotones = !$sessionStorage.soloLectura ? true : false;// habilita solo en paso 1

        $scope.$watch('vm.tramiteproyectoid', function () {
            if (vm.tramiteproyectoid != '') {
                var proyectoCargado = comunesServicio.getProyectoCargado();
                if (proyectoCargado.toString() === vm.proyectoid) {
                    vm.ObtenerPoliticasModificaciones(vm.tramiteproyectoid);
                    vm.cargarInicio();
                }
            }
        });

        $scope.$watch('vm.calendariopoliticastransversales', function () {
            if (vm.calendariopoliticastransversales !== undefined && vm.calendariopoliticastransversales !== '')
                vm.habilitaBotones = vm.calendariopoliticastransversales === 'true' && !$sessionStorage.soloLectura ? true : false;
        });

        vm.init = function () {
            vm.notificarrefresco({ handler: vm.refrescarPoliticas, nombreComponente: vm.nombreComponente });
        };

        vm.refrescarPoliticas = function () {
            vm.cargarInicio();
            vm.ObtenerPoliticasModificaciones(vm.tramiteproyectoid);
        }

        vm.cargarInicio = function () {
            setTimeout(function () {
                vm.activarControles('inicio');
                /*vm.ObtenerPoliticasModificaciones(vm.tramiteproyectoid);*/
                vm.ObtenerPoliticasSolicitudConcepto($sessionStorage.idInstancia);
                vm.ObtenerDireccionesTecnicasPoliticas();
                vm.ObtenerResumenSolicitudConcepto(vm.codigobpin);
                vm.ConsultarArchivosConcepto();
                vm.ObtenerPreguntasEnvioPoliticaSubDireccion();
                vm.ValidaRol();
                limpiarArchivo();
            }, 1000);
        }

        vm.ActivarTab = function (tab) {
            vm.TabActivo = tab;
        }

        vm.ValidaRol = function () {
            if ($sessionStorage.usuario.roles.find(x => x.IdRol.toUpperCase() == constantesBackbone.idRControlValidadorPoliticaTransversal)) {
                vm.rolValidadorPolitica = true;
            }
        }

        vm.activarControles = function (evento) {
            switch (evento) {
                case "inicio":
                    $("#btnCargaConceptoSeleccionado").attr('disabled', true);
                    if (vm.conceptoPoliticaValores) {
                        document.getElementById('fileArchivoConcepto_' + vm.conceptoPoliticaValores).value = "";
                    }
                    vm.nombrearchivoConcepto = "";
                    break;
                case "cargaarchivo":
                    $("#btnCargaConceptoSeleccionado").attr('disabled', true);
                    break;
                case "validado":
                    $("#btnCargaConceptoSeleccionado").attr('disabled', false);

                    vm.HabilitaEditarBandera = true;
                    break;
                default:
            }
        }

        $scope.ChangeArchivoSet = function () {
            if (vm.nombrearchivoConcepto == "") {

            }
        };

        $scope.fileArchivoConceptoNameChanged = function (input) {
            if (input.files.length == 1) {

                var fileSize = Math.trunc(input.files[0].size / (1024 * 1024));
                if (fileSize > 5) {
                    utilidades.mensajeError('El tamaño del archivo no puede ser superior a 5 MB. Por favor validar.');
                    vm.activarControles('inicio');
                    return;
                }

                if (!validarExtension(obtenerExtension(input.files[0].name))) {
                    utilidades.mensajeError('Solo se permite la carga de archivos tipo: doc, docx y pdf. Por favor validar.');
                    vm.activarControles('inicio');
                    return;
                }
                else {
                    vm.nombrearchivoConcepto = input.files[0].name;
                    vm.archivo = input.files[0];
                    vm.activarControles('validado');
                }
            }
            else {
                vm.activarControles('inicio');
            }
        }

        function adjuntarArchivo() {

            document.getElementById('fileArchivoConcepto_' + vm.conceptoPoliticaValores).value = "";
            document.getElementById('fileArchivoConcepto_' + vm.conceptoPoliticaValores).click();
            if (vm.listaTipoArchivos != [] || vm.listaTipoArchivos != undefined) {
                vm.listaTipoArchivos = [];
                vm.ConsultarTipoDocumento();
            }
        }

        function limpiarArchivo() {
            $scope.filesArchivoConcepto = [];
            if (vm.conceptoPoliticaValores) {
                document.getElementById('fileArchivoConcepto_' + vm.conceptoPoliticaValores).value = "";
            }
            vm.archivo = undefined;
            vm.activarControles('inicio');
        }

        function NotificacionUsuarios(politicaId, recuperar) {
            vm.LstNotificacionFlujo = [];

            var PoliticaTransversal = "";
            vm.Politicas.PoliticaPrincipal.forEach(pol => {
                if (pol.PoliticaId == politicaId) {
                    PoliticaTransversal = pol.Politica;
                }
            });

            $scope.mydate = new Date();

            if (vm.analistas.length > 0) {
                vm.listadoUsuarios = [];
                vm.analistas.forEach(anl => {

                    var NotificacionFlujo = {
                        IdUsuarioDNP: anl.IdUsuarioDnp,
                        NombreNotificacion: "Notificar a Dirección Técnica",
                        FechaInicio: new Date(),
                        FechaFin: new Date(),
                        ContenidoNotificacion: recuperar == true ? "la solicitud de aprobación de modificación de política fue recuperada por el formulador, por lo cual ya no se encuentra pendiente de respuesta." : "El formulador ha modificado la política transversal " + PoliticaTransversal + " y se envía a su bandeja para su aprobación.",
                        IdUsuario: $sessionStorage.usuario.permisos.IdUsuarioDNP,
                    }
                    vm.LstNotificacionFlujo.push(NotificacionFlujo);
                });
                serviciosComponenteNotificaciones.NotificarUsuarios(vm.LstNotificacionFlujo, $sessionStorage.usuario.permisos.IdUsuarioDNP).then(function (response) {
                    if (response.statusText === "OK" || response.status === 200) {

                    }
                });
            }
        }

        function RegistrarPermisosAccionPorUsuario() {

            if (vm.analistas.length > 0) {
                vm.listadoUsuarios = [];
                vm.analistas.forEach(anl => {

                    var usuario = {
                        IdUsuarioDNP: anl.IdUsuarioDnp,
                        NombreUsuario: vm.peticion.IdUsuario,
                        IdRol: constantesBackbone.idRControlValidadorPoliticaTransversal,
                        NombreRol: 'R_Validador politica transversal',
                        IdEntidad: 'Concepto Tecnico',
                        NombreEntidad: 'Formulador – Dirección Técnica',
                        IdEntidadMGA: $sessionStorage.idEntidad,
                    }

                    vm.listadoUsuarios.push(usuario);
                });
            }

            vm.RegistrarPermisosAccionDto = {
                ObjetoNegocioId: vm.codigobpin,
                IdAccion: $sessionStorage.idAccion,
                IdInstancia: $sessionStorage.idInstancia,
                EntityTypeCatalogOptionId: $sessionStorage.idEntidad,
                listadoUsuarios: vm.listadoUsuarios,
            };

            trasladosServicio.RegistrarPermisosAccionPorUsuario(vm.RegistrarPermisosAccionDto).then(function (response) {

            });

        }

        vm.ObtenerPoliticasModificaciones = function (bpin) {
            return comunesServicio.consultarPoliticasTransversalesAprobacionesModificaciones(bpin, usuarioDNP, $sessionStorage.idNivel).then(
                function (respuesta) {
                    if (respuesta.data != null && respuesta.data != "") {
                        var arreglolista = jQuery.parseJSON(respuesta.data);
                        vm.lstPoliticasModifica = jQuery.parseJSON(arreglolista);
                    }
                }
            );
        }

        vm.ObtenerPreguntasEnvioPoliticaSubDireccion = function () {

            var PreguntasPoliticasSubdireccion = {
                IdInstancia: $sessionStorage.idInstancia,
                IdProyecto: vm.proyectoid,
                IdUsuarioDNP: $sessionStorage.usuario.permisos.IdUsuarioDNP,
                IdNivel: $sessionStorage.idNivel
            };

            return focalizacionAjustesServicio.ObtenerPreguntasEnvioPoliticaSubDireccion(PreguntasPoliticasSubdireccion).then(

                function (respuesta) {
                    if (respuesta.data != null && respuesta.data != "") {
                        vm.PreguntasEnvioPoliticaSubDireccion = [];
                        vm.TienePoliticas = 0;
                        let arreglolistas = jQuery.parseJSON(respuesta.data);
                        vm.PreguntasEnvioPoliticaSubDireccion = jQuery.parseJSON(arreglolistas);
                        if (vm.PreguntasEnvioPoliticaSubDireccion.Politicas.length > 0) {
                            vm.PreguntasEnvioPoliticaSubDireccion.Politicas.forEach(pol => {
                                if (pol.PoliticaId == vm.conceptoPoliticaValores) {
                                    vm.TienePoliticas = vm.PreguntasEnvioPoliticaSubDireccion.Politicas.length;
                                    pol.VerPolitica = 0;

                                    pol.EnviosSubdireccion.forEach(es => {
                                        es.Preguntas.forEach(function (p, index, obj) {
                                            if (p.NombreRol === "20") {
                                                obj.splice(index, 1);
                                            }
                                        })
                                    });

                                    vm.EnviosSubdireccion = [];
                                    vm.EnviosSubdireccion = pol.EnviosSubdireccion.filter(x => x.RespuestaEnviada == false && x.Activo == true);
                                    if (vm.EnviosSubdireccion.length <= 0) {
                                        vm.EnviosSubdireccion = [];
                                        let solicitudesActivas = pol.EnviosSubdireccion.filter(x => x.RespuestaEnviada == true && x.Activo == true);
                                        let result = solicitudesActivas.reduce((a, b) => a.FechaHoraSolicitud > b.FechaHoraSolicitud ? a : b);
                                        if (result) {
                                            vm.EnviosSubdireccion.push(result);
                                        }

                                        vm.EnviosSubdireccion.forEach(envs => {
                                            envs.VerPregunta = 1;
                                            pol.VerPolitica = 1;
                                            envs.Preguntas.forEach(pre => {
                                                pre.OpcionesRespuesta = jQuery.parseJSON(pre.OpcionesRespuesta);
                                                pre.Editar = 0;
                                            });
                                        });
                                    } else {
                                        vm.EnviosSubdireccion.forEach(envs => {
                                            envs.VerPregunta = 0;
                                            if (envs.Activo) {
                                                pol.VerPolitica = 1;
                                                envs.VerPregunta = 1;
                                                envs.Preguntas.forEach(pre => {
                                                    pre.OpcionesRespuesta = jQuery.parseJSON(pre.OpcionesRespuesta);
                                                    pre.Editar = 0;
                                                });
                                            }
                                        });
                                    }
                                }
                            });

                            vm.PreguntasEnvioPoliticaSubDireccionOrigen = jQuery.parseJSON(arreglolistas);
                        }
                    }
                });
        }

        vm.ObtenerPoliticasSolicitudConcepto = function (bpin) {
            return focalizacionAjustesServicio.ObtenerPoliticasSolicitudConcepto(bpin, usuarioDNP, $sessionStorage.idNivel).then(
                function (respuesta) {
                    if (respuesta.data != null && respuesta.data != "") {

                        var politicaId = 0;
                        var arreglolistas = jQuery.parseJSON(respuesta.data);
                        vm.Politicas = jQuery.parseJSON(arreglolistas);
                        var i = 0;
                        vm.Politicas.PoliticaPrincipal.forEach(pol => {
                            if (pol.PoliticaId > 0)
                                i++;
                            if (pol.PoliticaId != politicaId) {
                                pol.VerPolitica = 1;
                            }
                            else {
                                pol.VerPolitica = 0;
                            }
                            politicaId = pol.PoliticaId;
                        });
                        vm.TienePoliticas = i;
                    }
                });
        }

        vm.ObtenerResumenSolicitudConcepto = function (bpin) {
            vm.activar = false;
            return focalizacionAjustesServicio.ObtenerResumenSolicitudConcepto($sessionStorage.idInstancia, usuarioDNP, $sessionStorage.idNivel).then(
                function (respuesta) {
                    if (respuesta.data != null && respuesta.data != "") {
                        vm.ResumenSolicitudConcepto = [];
                        vm.ResumenSolicitudDetalle = [];
                        var arreglolistas = jQuery.parseJSON(respuesta.data);
                        vm.ResumenSolicitudConcepto = jQuery.parseJSON(arreglolistas);
                        var i = 1;
                        var politicaId = 0;
                        vm.ResumenSolicitudConcepto.forEach(rs => {
                            if (rs.Resumen) {
                                let conceptoValores = rs.Resumen.filter(x => x.PoliticaId == vm.conceptoPoliticaValores);
                                if (conceptoValores && conceptoValores.length > 0) {
                                    vm.ResumenSolicitudDetalle = conceptoValores.filter(x => x.Aprobacion == "Pendiente");
                                    if (vm.ResumenSolicitudDetalle.length <= 0) {
                                        vm.ResumenSolicitudDetalle = [];
                                        let result = conceptoValores.reduce((a, b) => a.FechaHoraRegistro > b.FechaHoraRegistro ? a : b);
                                        if (result) {
                                            vm.ResumenSolicitudDetalle.push(result);
                                        }
                                    }
                                    conceptoValores.forEach(res => {
                                        if (res.Aprobacion == "Pendiente") {
                                            vm.activar = true;
                                        }
                                        else {
                                            vm.activar = false;
                                        }
                                        if (res.PoliticaId != politicaId) {
                                            res.VerPolitica = 1;
                                            i = 1;
                                            res.index = i;

                                        }
                                        else {
                                            res.VerPolitica = 0;
                                            res.index = i;
                                        }
                                        i++;
                                        politicaId = res.PoliticaId;
                                    });
                                }
                            }
                            else {
                                vm.ResumenSolicitudDetalle = [];
                            }
                        });
                    }
                });
        }

        vm.ObtenerDireccionesTecnicasPoliticas = function () {

            return focalizacionAjustesServicio.ObtenerDireccionesTecnicasPoliticasFocalizacion(usuarioDNP, $sessionStorage.idNivel).then(
                function (respuesta) {
                    if (respuesta.data != null && respuesta.data != "") {
                        var arreglolistas = jQuery.parseJSON(respuesta.data);
                        vm.DireccionesTecnicasPoliticas = jQuery.parseJSON(arreglolistas);
                    }
                    else {
                        utilidades.mensajeWarning('Se debe debe configurar transversal DireccionesTecnicasPoliticas.');
                        return false;
                    }
                });
        }

        vm.GuardarConceptoVictimas = function () {
            let politicaId = vm.conceptoPoliticaValores;

            if (vm.descripcion == '' || vm.descripcion == undefined) {
                utilidades.mensajeError('Debe ingresar una descripción.');
                return false;
            }

            utilidades.mensajeWarning("¿Esta seguro de continuar?", function funcionContinuar() {

                //Eliminar documento si existe
                vm.eliminarArchivoBlob();

                var EntityTypeCatalogOptionId = 0;

                vm.DireccionesTecnicasPoliticas.forEach(dirt => {
                    if (dirt.PolicitTargetingId == politicaId) {
                        EntityTypeCatalogOptionId = dirt.EntityTypeCatalogOptionId;
                    }
                });

                vm.peticion = {
                    IdUsuario: $sessionStorage.usuario.permisos.IdUsuarioDNP,
                    IdFiltro: EntityTypeCatalogOptionId,
                };

                if (vm.archivo != undefined) {
                    vm.CargarDocumentoConceptoVictimas();
                }

                focalizacionAjustesServicio.ObtenerUsuariosRValidadorPoliticaTransversal(vm.peticion)
                    .then(resultado => {
                        vm.FocalizacionSolicitarConcepto = [];
                        vm.analistas = resultado.data;
                        if (vm.analistas.length > 0) {
                            vm.analistas.forEach(anl => {

                                var concepto = {
                                    Id: 0,
                                    InstanciaId: $sessionStorage.idInstancia,
                                    ProyectoId: vm.proyectoid,
                                    PoliticaId: politicaId,
                                    Descripcion: vm.descripcion,
                                    IdUsuarioDNP: anl.IdUsuarioDnp,
                                    Activo: true,
                                    Enviado: false,
                                    EntityTypeCatalogOptionId: EntityTypeCatalogOptionId,
                                    CreadoPor: $sessionStorage.usuario.permisos.IdUsuarioDNP,
                                };

                                vm.FocalizacionSolicitarConcepto.push(concepto);

                            });

                            focalizacionAjustesServicio.FocalizacionSolicitarConceptoDT(vm.FocalizacionSolicitarConcepto).then(function (response) {
                                if (response.statusText === "OK" || response.status === 200) {
                                    setTimeout(function () {
                                        NotificacionUsuarios(politicaId, false);
                                        RegistrarPermisosAccionPorUsuario();
                                        vm.descripcion = "";
                                        vm.cargarInicio();
                                        utilidades.mensajeSuccess('', false, false, false, "La solicitud ha sido enviada con éxito.");
                                    }, 1000);

                                    vm.guardadoevent({ nombreComponenteHijo: vm.nombreComponente });
                                }
                                else {
                                    utilidades.mensajeError(response.data.Mensaje);
                                }
                            });

                        }
                        else {
                            utilidades.mensajeWarning('No existen analistas configurados para la dirección técnica');
                        }
                    });

            }, function funcionCancelar(reason) {
                console.log("reason", reason);
            }, null, null, "Usted enviará una solicitud de aprobación de política.");

        }

        vm.Recuperar = function () {
            utilidades.mensajeWarning("¿Esta seguro de continuar?", function funcionContinuar() {
                let politicaId = vm.conceptoPoliticaValores;

                var EntityTypeCatalogOptionId = 0;

                vm.DireccionesTecnicasPoliticas.forEach(dirt => {
                    if (dirt.PolicitTargetingId == politicaId) {
                        EntityTypeCatalogOptionId = dirt.EntityTypeCatalogOptionId;
                    }
                });

                vm.peticion = {
                    IdUsuario: $sessionStorage.usuario.permisos.IdUsuarioDNP,
                    IdFiltro: EntityTypeCatalogOptionId,
                };

                focalizacionAjustesServicio.ObtenerUsuariosRValidadorPoliticaTransversal(vm.peticion)
                    .then(resultado => {
                        vm.FocalizacionSolicitarConcepto = [];
                        vm.analistas = resultado.data;
                        if (vm.analistas.length > 0) {
                            vm.analistas.forEach(anl => {

                                solicitarconceptoServicio.eliminarPermisos(anl.IdUsuarioDnp, $sessionStorage.InstanciaSeleccionada.tramiteId, 'TEC', $sessionStorage.idInstancia);

                                var concepto = {
                                    Id: 0,
                                    InstanciaId: $sessionStorage.idInstancia,
                                    ProyectoId: vm.proyectoid,
                                    PoliticaId: politicaId,
                                    Descripcion: "",
                                    IdUsuarioDNP: anl.IdUsuarioDnp,
                                    Activo: false,
                                    Enviado: false,
                                    EntityTypeCatalogOptionId: vm.DireccionesTecnicasPoliticas[0].EntityTypeCatalogOptionId,
                                };

                                vm.FocalizacionSolicitarConcepto.push(concepto);

                            });

                            focalizacionAjustesServicio.FocalizacionSolicitarConceptoDT(vm.FocalizacionSolicitarConcepto).then(function (response) {
                                if (response.statusText === "OK" || response.status === 200) {
                                    setTimeout(function () {
                                        NotificacionUsuarios(politicaId, true);
                                        if (vm.Archivos.tieneArchivosAdjuntos[String(vm.proyectoid)]) {
                                            vm.eliminarArchivoBlob();
                                        }
                                        vm.cargarInicio();
                                        utilidades.mensajeSuccess('', false, false, false, "La solicitud de aprobación de la política ha sido recuperada con éxito.");
                                    }, 1000);

                                    vm.guardadoevent({ nombreComponenteHijo: vm.nombreComponente });
                                }
                                else {
                                    utilidades.mensajeError(response.data.Mensaje);
                                }
                            });
                        }
                        else {
                            utilidades.mensajeWarning('No existen analistas configurados para la dirección técnica');
                        }
                    });

            }, function funcionCancelar(reason) {
                console.log("reason", reason);
            }, null, null, "La solicitud de aprobación de política será recuperada.");

        }

        //Cargar y descargar documento concepto
        vm.ConsultarTipoDocumento = function () {
            var idTramite = $sessionStorage.InstanciaSeleccionada.tramiteId;
            var tipoTramiteId = $sessionStorage.InstanciaSeleccionada.tipoTramiteId;

            if ((vm.rol === undefined || vm.rol === "") || vm.rol == '00000000-0000-0000-0000-000000000000') {
                vm.rol = '00000000-0000-0000-0000-000000000000';

                var roles = sesionServicios.obtenerUsuarioIdsRoles();

                archivoServicios.obtenerTipoDocumentoTramitePorRol(tipoTramiteId, '', idTramite, 55).then(function (resultado) {
                    resultado.data.map(function (item) {
                        roles.map(function (item2) {
                            if (item.RolId === item2) {
                                vm.listaTipoArchivos.push(item);
                            }
                        });
                    });
                });
            } else {
                if (vm.idtipotramitepresupuestal !== undefined && vm.idtipotramitepresupuestal !== "") vm.tipotramiteid = vm.idtipotramitepresupuestal;
                archivoServicios.obtenerTipoDocumentoTramitePorRol(tipoTramiteId, vm.rol, idTramite, null).then(function (resultado) {

                    vm.listaTipoArchivos = resultado.data;
                    cargaTipoArchivosObligatorios(vm.listaTipoArchivos);

                });
            }
        }

        vm.CargarDocumentoConceptoVictimas = function () {

            vm.listaArchivos.push({
                codigoProceso: '',
                descripcionTramite: '',
                fecha: '',
                nombreArchivo: '',
                tipoDocumento: '',
                tipoDocumentoId: '',
                idArchivoBlob: '',
                ContenType: '',
                idMongo: ''
            });

            let tipoArchivo = undefined;

            if (vm.listaTipoArchivos.length > 0) {
                tipoArchivo = vm.listaTipoArchivos[0];
            }
            trasladosServicio.obtenerDatosUsuario($sessionStorage.usuario.permisos.IdUsuarioDNP, $sessionStorage.idEntidad,
                $sessionStorage.idAccion, $sessionStorage.idInstancia).
                then(function (respuesta) {
                    var rol = $sessionStorage.usuario.roles.find(x => x.IdRol === vm.rol);
                    var usuariotemporal = {};
                    if (respuesta.data !== null) {

                        if (respuesta.data.length > 0) {
                            var usuariotemporal = respuesta.data[0];
                        }
                    }

                    var Cuenta = '';
                    var Entidad = '';
                    var NombreUsuario = '';
                    if (usuariotemporal) {
                        Cuenta = usuariotemporal.Cuenta === null ? '' : usuariotemporal.Cuenta;
                        Entidad = usuariotemporal.Entidad === null ? '' : usuariotemporal.Entidad;
                        NombreUsuario = usuariotemporal.NombreUsuario === null ? '' : usuariotemporal.NombreUsuario;
                    }
                    var dato = formatearFecha(obtenerFechaSinHoras(new Date())) + '. ' + NombreUsuario + ' ' + Cuenta + ' ' + Entidad;

                    if (vm.archivo) {
                        vm.extension = obtenerExtension(vm.archivo.name);
                        let archivo = {
                            FormFile: vm.archivo,
                            Nombre: vm.archivo.name.trimEnd(),
                            Metadatos: {
                                extension: vm.extension,
                                idInstancia: $sessionStorage.idInstancia,
                                idAccion: $sessionStorage.idAccion,
                                proyectoId: vm.proyectoid,
                                tipoDocumento: tipoArchivo.TipoDocumento,
                                tipoDocumentoId: tipoArchivo.TipoDocumentoId,
                                idNivel: $sessionStorage.nivel,
                                idRol: tipoArchivo.RolId,
                                idAccion: $sessionStorage.idAccion,
                                datosdocumento: dato,
                                tipoDocumentoSoporte: tipoArchivo.TipoDocumento,
                                politicaId: vm.conceptoPoliticaValores,
                                proceso: "conceptoValores"
                            }
                        };

                        archivoServicios.cargarArchivo(archivo, vm.coleccion).then(function (response) {
                            if (response === undefined || typeof response === 'string') {
                                vm.mensajeError = response;
                                utilidades.mensajeError(response);
                            } else {
                                vm.desactivarGuardar = false;
                                utilidades.mensajeSuccess(
                                    '',
                                    false,
                                    function funcionContinuar() {
                                        $timeout(function () {
                                            vm.archivo = undefined;
                                            vm.nombrearchivoConcepto = undefined;
                                        }).then(function () { renderPanel(); });
                                    },
                                    false,
                                    "El documento se ha agregado con éxito a la solicitud.");
                            }
                        }, error => {
                            console.log(error);
                        });
                    }
                });
        }

        vm.ConsultarArchivosConcepto = function () {

            let param = {
                proyectoId: vm.proyectoid,
                politicaId: vm.conceptoPoliticaValores,
                status: "Nuevo",
                proceso: "conceptoValores"
            };
            if (!vm.Archivos) {
                vm.Archivos = {
                    tieneArchivosAdjuntos: [], idArchivoBlobDescarga: [], nombreArchivoDescarga: [], contentType: [], idMongo: []
                }
            }

            archivoServicios.obtenerListadoArchivos(param, vm.coleccion).then(function (response) {
                vm.Archivos.tieneArchivosAdjuntos[String(vm.proyectoid)] = false;
                vm.Archivos.idArchivoBlobDescarga[String(vm.proyectoid)] = "";
                vm.Archivos.nombreArchivoDescarga[String(vm.proyectoid)] = "";
                vm.Archivos.contentType[String(vm.proyectoid)] = "";
                vm.Archivos.idMongo[String(vm.proyectoid)] = "";

                if (response === undefined || typeof response === 'string') {
                    vm.tieneArchivosAdjuntos = false;
                    vm.mensajeError = response;
                    utilidades.mensajeError(response);
                } else {
                    if (response.length > 0) {
                        vm.Archivos.tieneArchivosAdjuntos[String(vm.proyectoid)] = true;
                        vm.Archivos.idArchivoBlobDescarga[String(vm.proyectoid)] = response[0].metadatos.idarchivoblob;
                        vm.Archivos.nombreArchivoDescarga[String(vm.proyectoid)] = response[0].nombre;
                        vm.Archivos.contentType[String(vm.proyectoid)] = response[0].metadatos.contenttype;
                        vm.Archivos.idMongo[String(vm.proyectoid)] = response[0].id;

                    }
                    else {
                        vm.Archivos.tieneArchivosAdjuntos[String(vm.proyectoid)] = false;
                        vm.Archivos.idArchivoBlobDescarga[String(vm.proyectoid)] = "";
                        vm.Archivos.nombreArchivoDescarga[String(vm.proyectoid)] = "";
                        vm.Archivos.contentType[String(vm.proyectoid)] = "";
                        vm.Archivos.idMongo[String(vm.proyectoid)] = "";
                    }
                }
            });
        }

        vm.descargarArchivoBlob = function () {
            archivoServicios.obtenerArchivoBytes(vm.Archivos.idArchivoBlobDescarga[String(vm.proyectoid)], vm.coleccion).then(function (retorno) {
                const blob = utilidades.base64toBlob(retorno, vm.Archivos.contentType[String(vm.proyectoid)]);
                const downloadUrl = URL.createObjectURL(blob);
                const a = document.createElement("a");
                a.href = downloadUrl;
                a.download = vm.Archivos.nombreArchivoDescarga[String(vm.proyectoid)].trimEnd();
                document.body.appendChild(a);
                a.click();
            }, function (error) {
                utilidades.mensajeError("Error inesperado al descargar");
            });
        }

        vm.eliminarArchivoBlob = function () {
            if (vm.Archivos.idMongo[String(vm.proyectoid)]) {
                archivoServicios.eliminarArchivo(vm.Archivos.idMongo[String(vm.proyectoid)], 'Eliminado', vm.coleccion).then(function (response) {
                    if (response === undefined) {
                        utilidades.mensajeError("Hubo un error al eliminar el archivo");
                    }
                });
            }
        }

        vm.cambiarConcepto = function (politica) {
            vm.conceptoPoliticaValores = politica;
            vm.cargarInicio();
        }

        function formatearFecha(fecha) {
            let fechaString = fecha.toISOString();
            return fechaString.substring(0, 19);
        }

        function obtenerFechaSinHoras(fecha) {
            return new Date(Date.UTC(fecha.getFullYear(), fecha.getMonth(), fecha.getDate(), fecha.getHours(), fecha.getMinutes(), fecha.getSeconds()));
        }

        function obtenerExtension(nombreArchivo) {
            let partes = nombreArchivo.split('.');
            return partes[partes.length - 1];
        }

        function validarExtension(extension) {
            switch (extension.toLowerCase()) {
                case 'pdf': case 'doc': case 'docx': return true;
                default: return false;
            }
        }
    }

    angular.module('backbone')
        .component('conceptoValoresMl', {
            templateUrl: 'src/app/formulario/ventanas/comun/modificacionLey/politicas/valores/conceptoValoresMlFormulario.html',
            controller: 'conceptoValoresMl',
            controllerAs: 'vm',
            bindings: {
                callback: '&',
                notificacionvalidacion: '&',
                notificacionestado: '&',
                notificacioncambios: '&',
                guardadocomponent: '&',
                proyectoid: '@',
                codigobpin: '@',
                tramiteproyectoid: '@',
                guardadoevent: '&',
                notificarrefresco: '&',
                calendariopoliticastransversales: '@',
                mostrarconcepto: '@'
            },
        });
})();