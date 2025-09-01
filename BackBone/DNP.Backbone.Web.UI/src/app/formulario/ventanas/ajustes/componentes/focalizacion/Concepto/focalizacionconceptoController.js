(function () {
    'use strict';

    focalizacionconceptoController.$inject = ['$scope', '$sessionStorage', '$uibModal', 'utilidades',
        'constantesBackbone', '$timeout', 'focalizacionAjustesServicio', 'serviciosComponenteNotificaciones', 'trasladosServicio', 'solicitarconceptoServicio', 'sesionServicios', 'archivoServicios'
    ];

    function focalizacionconceptoController(
        $scope,
        $sessionStorage,
        $uibModal,
        utilidades,
        constantesBackbone,
        $timeout,
        focalizacionAjustesServicio,
        serviciosComponenteNotificaciones,
        trasladosServicio,
        solicitarconceptoServicio,
        sesionServicios,
        archivoServicios,
    ) {


        var vm = this;
        vm.init = init;
        vm.nombreComponente = "focalizacionpolSolicitarConAju";
        vm.BPIN = $sessionStorage.idObjetoNegocio;
        vm.abrirMensajeInformacionConcpeto = abrirMensajeInformacionConcpeto;

        vm.notificacionErrores = null;
        vm.erroresActivos = [];
        vm.arregloGeneralPTResumen = null;
        vm.verTablaResumen = false;
        vm.TienePoliticas = 0;
        vm.DocumentoJustificacion = "No",
        vm.NombreArchivo = "Justificación modificación política";

        vm.Politicas = [];
        vm.analistas = [];
        vm.FocalizacionSolicitarConcepto = [];
        vm.DireccionesTecnicasPoliticas = [];
        vm.ResumenSolicitudConcepto = [];
        vm.TieneRolValidadorPoliticaTransversal = 0;
        vm.UsuarioIdsRoles = sesionServicios.obtenerUsuarioIdsRoles();
        vm.PreguntasEnvioPoliticaSubDireccion = null;

        vm.componentesRefresh = [
            'recursosfuentesdefinanc',
            'datosgeneraleslocalizaciones',
            'focalizacionpoliticastransv',
            'focalizacionpolresumendefocali',
            'focalizacionpolpoliticastransv',
        ];

        function init() {
            vm.model = {
                modulos: {
                    administracion: false,
                    backbone: true
                }
            }




            vm.ObtenerPoliticasSolicitudConcepto(vm.BPIN);
            vm.ObtenerPreguntasEnvioPoliticaSubDireccion();
            vm.ObtenerDireccionesTecnicasPoliticas();
            vm.ObtenerResumenSolicitudConcepto(vm.BPIN);
            vm.consultarArchivosTramite();
            /*
            var respuesta = '{"ProyectoId":97977,"BPIN":"202200000000128","PoliticaPrincipal":[{"PoliticaId":7,"Politica":"Equidad de la mujer"},{"PoliticaId":10,"Politica":"Grupos étnicos - comunidades indígenas"},{"PoliticaId":20,"Politica":"Víctimas"}]}';
            vm.Politicas = jQuery.parseJSON(respuesta);

            vm.TienePoliticas = vm.Politicas.PoliticaPrincipal.length;
            */
            vm.notificacionvalidacion({ handler: vm.notificacionValidacionPadre, nombreComponente: vm.nombreComponente, esValido: true });
            vm.notificarrefresco({ handler: vm.refrescarResumenCostos, nombreComponente: vm.nombreComponente });
            vm.notificacioncambios({ handler: vm.notificacionCambiosCapitulos, nombreComponente: vm.nombreComponente });

            vm.TieneRolValidadorPoliticaTransversal = 0
            vm.UsuarioIdsRoles.forEach(rol => {
                if (rol.toUpperCase() == constantesBackbone.idRValidadorPoliticaTransversal) {
                    vm.TieneRolValidadorPoliticaTransversal = 1
                }
            });

        }

        vm.consultarArchivosTramite = function () {
            var valor = 0;
            var param = {
                bpin: $sessionStorage.idObjetoNegocio,
                idinstancia: $sessionStorage.idInstancia
            };

            archivoServicios.obtenerListadoArchivos(param, "proyectos").then(function (response) {
                if (response === undefined || typeof response === 'string') {
                    vm.mensajeError = response;
                    utilidades.mensajeError(response);
                } else {
                    vm.archivosLoad = [];
                    if (response != null) {
                        response.forEach(archivo => {
                            if (archivo.status != 'Eliminado') {
                                if (archivo.nombre == vm.NombreArchivo) {
                                    vm.DocumentoJustificacion = "Si";
                                }
                            }
                        });
                    }
                }
            }, error => {
                console.log(error);
            });

        }

        vm.ObtenerPreguntasEnvioPoliticaSubDireccion = function () {

            var PreguntasPoliticasSubdireccion = {
                IdInstancia: $sessionStorage.idInstancia,
                IdProyecto: $sessionStorage.idProyectoEncabezado,
                IdUsuarioDNP: $sessionStorage.usuario.permisos.IdUsuarioDNP,
                IdNivel: $sessionStorage.idNivel
            };

            return focalizacionAjustesServicio.ObtenerPreguntasEnvioPoliticaSubDireccion(PreguntasPoliticasSubdireccion).then(

                function (respuesta) {
                    if (respuesta.data != null && respuesta.data != "") {
                        vm.PreguntasEnvioPoliticaSubDireccion = null;
                        var arreglolistas = jQuery.parseJSON(respuesta.data);
                        vm.PreguntasEnvioPoliticaSubDireccion = jQuery.parseJSON(arreglolistas);
                        if (vm.PreguntasEnvioPoliticaSubDireccion.Politicas.length > 0) {
                            vm.TienePoliticas = vm.PreguntasEnvioPoliticaSubDireccion.Politicas.length;

                            vm.PreguntasEnvioPoliticaSubDireccion.Politicas.forEach(pol => {
                                pol.VerPolitica = 0;
                                pol.EnviosSubdireccion.forEach(envs => {
                                    envs.VerPregunta = 0;
                                    if (envs.RespuestaEnviada == true) {
                                        pol.VerPolitica = 1;
                                        envs.VerPregunta = 1;
                                        envs.Preguntas.forEach(pre => {
                                            pre.OpcionesRespuesta = jQuery.parseJSON(pre.OpcionesRespuesta);
                                            pre.Editar = 0;
                                        });
                                    }
                                });
                            });
                        }
                    }

                    var valor = 0;
                });
        }
        vm.ObtenerResumenSolicitudConcepto = function (bpin) {
            return focalizacionAjustesServicio.ObtenerResumenSolicitudConcepto($sessionStorage.idInstancia, usuarioDNP, $sessionStorage.idNivel).then(
                function (respuesta) {
                    if (respuesta.data != null && respuesta.data != "") {
                        vm.ResumenSolicitudConcepto = [];
                        var arreglolistas = jQuery.parseJSON(respuesta.data);
                        vm.ResumenSolicitudConcepto = jQuery.parseJSON(arreglolistas);
                        var i = 1;
                        var politicaId = 0;
                        vm.ResumenSolicitudConcepto.forEach(rs => {
                            rs.Resumen.forEach(res => {
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
                        });
                    }
                });

            /*
            var arreglolistas = '[{"BPIN":"202200000000136","ProyectoId":97986,"Resumen":[{"PoliticaId":4,"Politica":"Construcción de Paz","DireccionTecnica":"Sub. De Derechos humanos y Paz","Descripcion":"Concepto tomado desde el front","Fecha":"09\/24\/2022 21:33","Aprobacion":"Pendiente"},{"PoliticaId":11,"Politica":"Grupos étnicos - comunidades negras","DireccionTecnica":"Sub. De Derechos humanos y Paz","Descripcion":"Concepto tomado desde el front","Fecha":"09\/24\/2022 21:33","Aprobacion":"Pendiente"}]}]';
            vm.ResumenSolicitudConcepto = jQuery.parseJSON(arreglolistas);
            */

        }

        function abrirMensajeInformacionConcpeto() {
            utilidades.mensajeInformacionN("", null, "<span class='anttituhori' > Condiciones de diligenciamiento, </span><br /> <span class='tituhori' > Política para solicitud de concepto.</span>");
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

            //var arreglolistas = '[{"Id":1,"EntityTypeCatalogOptionId":10010146,"PolicitTargetingId":4},{"Id":2,"EntityTypeCatalogOptionId":10010151,"PolicitTargetingId":7},{"Id":3,"EntityTypeCatalogOptionId":10010206,"PolicitTargetingId":9},{"Id":4,"EntityTypeCatalogOptionId":10010206,"PolicitTargetingId":10},{"Id":5,"EntityTypeCatalogOptionId":10010206,"PolicitTargetingId":11},{"Id":6,"EntityTypeCatalogOptionId":10010206,"PolicitTargetingId":12},{"Id":7,"EntityTypeCatalogOptionId":10010206,"PolicitTargetingId":13},{"Id":8,"EntityTypeCatalogOptionId":10010206,"PolicitTargetingId":14},{"Id":9,"EntityTypeCatalogOptionId":10010206,"PolicitTargetingId":20}]';//jQuery.parseJSON(respuesta.data);
            //vm.DireccionesTecnicasPoliticas = jQuery.parseJSON(arreglolistas);

        }

        vm.ObtenerPoliticasSolicitudConcepto = function (bpin) {
            return focalizacionAjustesServicio.ObtenerPoliticasSolicitudConcepto($sessionStorage.idInstancia, usuarioDNP, $sessionStorage.idNivel).then(
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

                        /*
                        setTimeout(function () {
                            vm.ObtenerResumenSolicitudConcepto(vm.BPIN);
                        }, 500);

                        setTimeout(function () {
                            vm.ObtenerPreguntasEnvioPoliticaSubDireccion();
                        }, 500);
                        */
                        //vm.ObtenerPreguntasEnvioPoliticaSubDireccion();

                    }
                });
        }

        function ObtenerSeccionCapitulo() {
            //var FaseGuid = constantesBackbone.idEtapaNuevaEjecucion;
            //var Capitulo = 'Fuentes de financiación';
            //var Seccion = 'Recursos';

            //return justificacionCambiosServicio.ObtenerSeccionCapitulo(FaseGuid, Capitulo, Seccion, $sessionStorage.usuario.permisos.IdUsuarioDNP).then(
            //    function (respuesta) {
            //        if (respuesta.data != null && respuesta.data != "") {
            //            vm.seccionCapitulo = respuesta.data;
            //        }
            //    });
            const span = document.getElementById('id-capitulo-' + vm.nombreComponente);
            vm.seccionCapitulo = span.textContent;
        }

        vm.notificacionCambiosCapitulos = function (nombreCapituloHijo) {
            if (vm.componentesRefresh.includes(nombreCapituloHijo)) {
                vm.init();
            }
        }

        vm.notificacionValidacionHijo = function (handler) {
            vm.notificacionErrores = handler;
        }

        vm.validacionGuardadoHijo = function (handler) {
            vm.validacionGuardado = handler;
        }


        vm.AbrilNivel = function (politicaId, indexpoliticas) {

            var variable = $("#icojus-" + politicaId + "-" + indexpoliticas)[0].innerText;
            variable = variable.replace(/ /g, "");
            var imgmas = document.getElementById("imgmasjus-" + politicaId + "-" + indexpoliticas);
            var imgmenos = document.getElementById("imgmenosjus-" + politicaId + "-" + indexpoliticas);
            if (variable === "+") {
                $("#icojus-" + politicaId + "-" + indexpoliticas).html('-');
                imgmas.style.display = 'none';
                imgmenos.style.display = 'block';

            } else {
                $("#icojus-" + politicaId + "-" + indexpoliticas).html('+');
                imgmas.style.display = 'block';
                imgmenos.style.display = 'none';
            }

            vm.Politicas.PoliticaPrincipal.forEach(pol => {
                if (pol.PoliticaId == politicaId && pol.Enviado == false && pol.VerPolitica == 1) {
                    if (pol.Descripcion != "") {
                        document.getElementById("btnGuardarPolitica-" + pol.PoliticaId + "-" + pol.Enviado).classList.add('disabled');
                        document.getElementById("descripcionconcepto-" + pol.PoliticaId + "-" + pol.Enviado).classList.add('disabled');
                    }
                    else {
                        document.getElementById("btnGuardarPolitica-" + pol.PoliticaId + "-" + pol.Enviado).classList.remove('disabled');
                        document.getElementById("descripcionconcepto-" + pol.PoliticaId + "-" + pol.Enviado).classList.remove('disabled');
                    }
                }
            });
            /*
            if (vm.ResumenSolicitudConcepto.length > 0) {
                vm.ResumenSolicitudConcepto.forEach(rsc => {
                    if (rsc.Resumen != null) {
                        rsc.Resumen.forEach(res => {
                            if (res.PoliticaId == politicaId && res.VerPolitica == 1) {
                                if (res.Aprobacion == 'Solucionado') {
                                    var btnRecuperarConcepto = document.getElementById("btnRecuperarConcepto-" + res.PoliticaId);
                                    if (btnRecuperarConcepto != undefined) {
                                        document.getElementById("btnRecuperarConcepto-" + res.PoliticaId).classList.add('disabled');
                                    }
                                }
                                else {
                                    var btnRecuperarConcepto = document.getElementById("btnRecuperarConcepto-" + res.PoliticaId);
                                    if (btnRecuperarConcepto != undefined) {
                                       document.getElementById("btnRecuperarConcepto-" + res.PoliticaId).classList.remove('disabled');
                                    }
                                }
                            }
                        });
                    }
                });
            }
            */
            vm.PreguntasEnvioPoliticaSubDireccion.Politicas.forEach(pol => {
                if (politicaId == pol.PoliticaId) {
                    pol.EnviosSubdireccion.forEach(envs => {
                        if (envs.RespuestaEnviada == true) {
                            envs.Preguntas.forEach(pre => {
                                var radiosirp = document.getElementById("radiorp" + pol.PoliticaId + envs.EnvioPoliticaSubDireccionIdAgrupa + pre.PreguntaId + 0);
                                var radionorp = document.getElementById("radiorp" + pol.PoliticaId + envs.EnvioPoliticaSubDireccionIdAgrupa + pre.PreguntaId + 1);
                                if (radiosirp != undefined && radionorp != undefined) {
                                    if (pre.Respuesta == "1") {
                                        $('#radiorp' + pol.PoliticaId + envs.EnvioPoliticaSubDireccionIdAgrupa + pre.PreguntaId + 0).prop('checked', true);
                                    }
                                    if (pre.Respuesta == "2") {
                                        $('#radiorp' + pol.PoliticaId + envs.EnvioPoliticaSubDireccionIdAgrupa + pre.PreguntaId + 1).prop('checked', true);
                                    }
                                }

                            });
                        }
                    });
                }
            });
        }

        vm.guardarDT = function (politicaId, enviado) {

            var descripcion = document.getElementById("descripcionconcepto-" + politicaId + "-" + enviado).value;
            if (descripcion == '' || descripcion == undefined) {
                utilidades.mensajeError('Debe ingresar una descripción.');
                return false;
            }

            utilidades.mensajeWarning("¿Esta seguro de continuar?", function funcionContinuar() {



                var EntityTypeCatalogOptionId = 0;//10010151;

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

                                var concepto = {
                                    Id: 0,
                                    InstanciaId: $sessionStorage.idInstancia,
                                    ProyectoId: $sessionStorage.InstanciaSeleccionada.ProyectoId,
                                    PoliticaId: politicaId,
                                    Descripcion: descripcion,
                                    IdUsuarioDNP: anl.IdUsuarioDnp, //$sessionStorage.usuario.permisos.IdUsuarioDNP,
                                    Activo: true,
                                    Enviado: false,
                                    EntityTypeCatalogOptionId: EntityTypeCatalogOptionId,
                                    CreadoPor: $sessionStorage.usuario.permisos.IdUsuarioDNP,
                                };

                                vm.FocalizacionSolicitarConcepto.push(concepto);

                            });

                            focalizacionAjustesServicio.FocalizacionSolicitarConceptoDT(vm.FocalizacionSolicitarConcepto).then(function (response) {
                                if (response.statusText === "OK" || response.status === 200) {
                                    NotificacionUsuarios(politicaId, false);
                                    RegistrarPermisosAccionPorUsuario();
                                    //IniciarFormulario();
                                    vm.guardadoevent({ nombreComponenteHijo: vm.nombreComponente });
                                    init();
                                    utilidades.mensajeSuccess('', false, false, false, "La solicitud ha sido enviada con éxito.");
                                    document.getElementById("btnGuardarPolitica-" + politicaId + "-" + enviado).classList.add('disabled');
                                    document.getElementById("descripcionconcepto-" + politicaId + "-" + enviado).classList.add('disabled');
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
                        //EsManual:0,
                        //Tipo
                        ContenidoNotificacion: recuperar == true ? "la solicitud de aprobación de modificación de política fue recuperada por el formulador, por lo cual ya no se encuentra pendiente de respuesta." : "El formulador ha modificado la política transversal " + PoliticaTransversal + " y se envía a su bandeja para su aprobación.",
                        //NombreArchivo   
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

            var valor = 0;


            vm.RegistrarPermisosAccionDto = {
                ObjetoNegocioId: vm.BPIN,
                IdAccion: $sessionStorage.idAccion,
                IdInstancia: $sessionStorage.idInstancia,
                EntityTypeCatalogOptionId: $sessionStorage.idEntidad,
                listadoUsuarios: vm.listadoUsuarios,
            };

            var valorr = 0;

            trasladosServicio.RegistrarPermisosAccionPorUsuario(vm.RegistrarPermisosAccionDto).then(function (response) {

            });

        }




        vm.RecuperarDT = function (politicaId, indexresumenpoliticas) {
            utilidades.mensajeWarning("¿Esta seguro de continuar?", function funcionContinuar() {


                var EntityTypeCatalogOptionId = 0;//10010151;

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

                                solicitarconceptoServicio.eliminarPermisos(anl.IdUsuarioDnp, $sessionStorage.TramiteId, 'TEC', $sessionStorage.idInstancia);

                                var concepto = {
                                    Id: 0,
                                    InstanciaId: $sessionStorage.idInstancia,
                                    ProyectoId: $sessionStorage.InstanciaSeleccionada.ProyectoId,
                                    PoliticaId: politicaId,
                                    Descripcion: "",
                                    IdUsuarioDNP: anl.IdUsuarioDnp, //$sessionStorage.usuario.permisos.IdUsuarioDNP,
                                    Activo: false,
                                    Enviado: false,
                                    EntityTypeCatalogOptionId: vm.DireccionesTecnicasPoliticas[0].EntityTypeCatalogOptionId,
                                };

                                vm.FocalizacionSolicitarConcepto.push(concepto);

                            });

                            focalizacionAjustesServicio.FocalizacionSolicitarConceptoDT(vm.FocalizacionSolicitarConcepto).then(function (response) {
                                if (response.statusText === "OK" || response.status === 200) {
                                    NotificacionUsuarios(politicaId, true);
                                    utilidades.mensajeSuccess('', false, false, false, "La solicitud de aprobación de la política ha sido recuperada con éxito.");
                                    //IniciarFormulario();
                                    vm.guardadoevent({ nombreComponenteHijo: vm.nombreComponente });
                                    init();
                                    /*
                                    setTimeout(function () {
                                        vm.AbrilNivel(politicaId, indexresumenpoliticas);
                                    }, 15000
                                    )
                                    */
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


        function IniciarFormulario() {

            vm.Politicas = [];
            vm.ObtenerPoliticasSolicitudConcepto(vm.BPIN);
            vm.ResumenSolicitudConcepto = [];
            vm.ObtenerResumenSolicitudConcepto(vm.BPIN);
        }

        /* ------------------------ Validaciones ---------------------------------*/

        vm.notificacionValidacionPadre = function (errores) {
            console.log("Validación  - Gestion Recursos Fuentes de financiación");
            vm.limpiarErrores();
            var isValid = true;
            if (errores != undefined) {
                var erroresRelacionconlapl = errores.find(p => (p.Seccion + p.Capitulo) == vm.nombreComponente);
                if (erroresRelacionconlapl != null || erroresRelacionconlapl != undefined) {
                    var erroresJson = erroresRelacionconlapl.Errores == "" ? [] : JSON.parse(erroresRelacionconlapl.Errores);

                    if (vm.notificacionErrores != null && erroresJson != null) vm.notificacionErrores(erroresJson[vm.nombreComponente]);

                    isValid = (erroresJson == null || erroresJson.length == 0);
                    if (!isValid) {
                        erroresJson[vm.nombreComponente].forEach(p => {
                            if (vm.errores[p.Error] != undefined) vm.errores[p.Error](p.Descripcion);
                        });
                    }
                }
                vm.notificacionestado({ nombreComponente: vm.nombreComponente, esValido: isValid });

            }
        };

        vm.limpiarErrores = function () {

            var campoObligatorioJustificacion4 = document.getElementById("descripcionconcepto-error-4");
            var ValidacionFFR1Error4 = document.getElementById("descripcionconcepto-error-mns-4");
            if (campoObligatorioJustificacion4 != undefined) {
                if (ValidacionFFR1Error4 != undefined) {
                    ValidacionFFR1Error4.innerHTML = '';
                    campoObligatorioJustificacion4.classList.add('hidden');
                }
            }

            var campoObligatorioJustificacion7 = document.getElementById("descripcionconcepto-error-7");
            var ValidacionFFR1Error7 = document.getElementById("descripcionconcepto-error-mns-7");
            if (campoObligatorioJustificacion7 != undefined) {
                if (ValidacionFFR1Error7 != undefined) {
                    ValidacionFFR1Error7.innerHTML = '';
                    campoObligatorioJustificacion7.classList.add('hidden');
                }
            }

            var campoObligatorioJustificacion9 = document.getElementById("descripcionconcepto-error-9");
            var ValidacionFFR1Error9 = document.getElementById("descripcionconcepto-error-mns-9");
            if (campoObligatorioJustificacion9 != undefined) {
                if (ValidacionFFR1Error9 != undefined) {
                    ValidacionFFR1Error9.innerHTML = '';
                    campoObligatorioJustificacion9.classList.add('hidden');
                }
            }

            var campoObligatorioJustificacion10 = document.getElementById("descripcionconcepto-error-10");
            var ValidacionFFR1Error10 = document.getElementById("descripcionconcepto-error-mns-10");
            if (campoObligatorioJustificacion10 != undefined) {
                if (ValidacionFFR1Error10 != undefined) {
                    ValidacionFFR1Error10.innerHTML = '';
                    campoObligatorioJustificacion10.classList.add('hidden');
                }
            }

            var campoObligatorioJustificacion11 = document.getElementById("descripcionconcepto-error-11");
            var ValidacionFFR1Error11 = document.getElementById("descripcionconcepto-error-mns-11");
            if (campoObligatorioJustificacion11 != undefined) {
                if (ValidacionFFR1Error11 != undefined) {
                    ValidacionFFR1Error11.innerHTML = '';
                    campoObligatorioJustificacion11.classList.add('hidden');
                }
            }

            var campoObligatorioJustificacion12 = document.getElementById("descripcionconcepto-error-12");
            var ValidacionFFR1Error12 = document.getElementById("descripcionconcepto-error-mns-12");
            if (campoObligatorioJustificacion12 != undefined) {
                if (ValidacionFFR1Error12 != undefined) {
                    ValidacionFFR1Error12.innerHTML = '';
                    campoObligatorioJustificacion12.classList.add('hidden');
                }
            }

            var campoObligatorioJustificacion13 = document.getElementById("descripcionconcepto-error-13");
            var ValidacionFFR1Error13 = document.getElementById("descripcionconcepto-error-mns-13");
            if (campoObligatorioJustificacion13 != undefined) {
                if (ValidacionFFR1Error13 != undefined) {
                    ValidacionFFR1Error13.innerHTML = '';
                    campoObligatorioJustificacion13.classList.add('hidden');
                }
            }

            var campoObligatorioJustificacion14 = document.getElementById("descripcionconcepto-error-14");
            var ValidacionFFR1Error14 = document.getElementById("descripcionconcepto-error-mns-14");
            if (campoObligatorioJustificacion14 != undefined) {
                if (ValidacionFFR1Error14 != undefined) {
                    ValidacionFFR1Error14.innerHTML = '';
                    campoObligatorioJustificacion14.classList.add('hidden');
                }
            }

            var campoObligatorioJustificacion20 = document.getElementById("descripcionconcepto-error-20");
            var ValidacionFFR1Error20 = document.getElementById("descripcionconcepto-error-mns-20");
            if (campoObligatorioJustificacion20 != undefined) {
                if (ValidacionFFR1Error20 != undefined) {
                    ValidacionFFR1Error20.innerHTML = '';
                    campoObligatorioJustificacion20.classList.add('hidden');
                }
            }

        }

        vm.validarConceptoPoliticaConsPaz = function (errores) {
            var campoObligatorioJustificacion4 = document.getElementById("descripcionconcepto-error-4");
            var ValidacionFFR1Error4 = document.getElementById("descripcionconcepto-error-mns-4");

            if (campoObligatorioJustificacion4 != undefined) {
                if (ValidacionFFR1Error4 != undefined) {
                    ValidacionFFR1Error4.innerHTML = '<span>' + errores + "</span>";
                    campoObligatorioJustificacion4.classList.remove('hidden');
                }
            }
        }

        vm.validarConceptoPoliticaEquidadMujer = function (errores) {
            var campoObligatorioJustificacion7 = document.getElementById("descripcionconcepto-error-7");
            var ValidacionFFR1Error7 = document.getElementById("descripcionconcepto-error-mns-7");

            if (campoObligatorioJustificacion7 != undefined) {
                if (ValidacionFFR1Error7 != undefined) {
                    ValidacionFFR1Error7.innerHTML = '<span>' + errores + "</span>";
                    campoObligatorioJustificacion7.classList.remove('hidden');
                }
            }
        }

        vm.validarConceptoPoliticaGEca = function (errores) {
            var campoObligatorioJustificacion9 = document.getElementById("descripcionconcepto-error-9");
            var ValidacionFFR1Error9 = document.getElementById("descripcionconcepto-error-mns-9");

            if (campoObligatorioJustificacion9 != undefined) {
                if (ValidacionFFR1Error9 != undefined) {
                    ValidacionFFR1Error9.innerHTML = '<span>' + errores + "</span>";
                    campoObligatorioJustificacion9.classList.remove('hidden');
                }
            }
        }

        vm.validarConceptoPoliticaGEci = function (errores) {
            var campoObligatorioJustificacion10 = document.getElementById("descripcionconcepto-error-10");
            var ValidacionFFR1Error10 = document.getElementById("descripcionconcepto-error-mns-10");

            if (campoObligatorioJustificacion10 != undefined) {
                if (ValidacionFFR1Error10 != undefined) {
                    ValidacionFFR1Error10.innerHTML = '<span>' + errores + "</span>";
                    campoObligatorioJustificacion10.classList.remove('hidden');
                }
            }
        }

        vm.validarConceptoPoliticaGEcn = function (errores) {
            var campoObligatorioJustificacion11 = document.getElementById("descripcionconcepto-error-11");
            var ValidacionFFR1Error11 = document.getElementById("descripcionconcepto-error-mns-11");

            if (campoObligatorioJustificacion11 != undefined) {
                if (ValidacionFFR1Error11 != undefined) {
                    ValidacionFFR1Error11.innerHTML = '<span>' + errores + "</span>";
                    campoObligatorioJustificacion11.classList.remove('hidden');
                }
            }
        }

        vm.validarConceptoPoliticaGEcp = function (errores) {
            var campoObligatorioJustificacion12 = document.getElementById("descripcionconcepto-error-12");
            var ValidacionFFR1Error12 = document.getElementById("descripcionconcepto-error-mns-12");

            if (campoObligatorioJustificacion12 != undefined) {
                if (ValidacionFFR1Error12 != undefined) {
                    ValidacionFFR1Error12.innerHTML = '<span>' + errores + "</span>";
                    campoObligatorioJustificacion12.classList.remove('hidden');
                }
            }
        }

        vm.validarConceptoPoliticaGRcr = function (errores) {
            var campoObligatorioJustificacion13 = document.getElementById("descripcionconcepto-error-13");
            var ValidacionFFR1Error13 = document.getElementById("descripcionconcepto-error-mns-13");

            if (campoObligatorioJustificacion13 != undefined) {
                if (ValidacionFFR1Error13 != undefined) {
                    ValidacionFFR1Error13.innerHTML = '<span>' + errores + "</span>";
                    campoObligatorioJustificacion13.classList.remove('hidden');
                }
            }
        }

        vm.validarConceptoPoliticaGEpr = function (errores) {
            var campoObligatorioJustificacion14 = document.getElementById("descripcionconcepto-error-14");
            var ValidacionFFR1Error14 = document.getElementById("descripcionconcepto-error-mns-14");

            if (campoObligatorioJustificacion14 != undefined) {
                if (ValidacionFFR1Error14 != undefined) {
                    ValidacionFFR1Error14.innerHTML = '<span>' + errores + "</span>";
                    campoObligatorioJustificacion14.classList.remove('hidden');
                }
            }
        }

        vm.validarConceptoPoliticaVictimas = function (errores) {
            var campoObligatorioJustificacion20 = document.getElementById("descripcionconcepto-error-20");
            var ValidacionFFR1Error20 = document.getElementById("descripcionconcepto-error-mns-20");

            if (campoObligatorioJustificacion20 != undefined) {
                if (ValidacionFFR1Error20 != undefined) {
                    ValidacionFFR1Error20.innerHTML = '<span>' + errores + "</span>";
                    campoObligatorioJustificacion20.classList.remove('hidden');
                }
            }
        }

        vm.errores = {
            'CONCPOL4': vm.validarConceptoPoliticaConsPaz,
            'CONCPOL7': vm.validarConceptoPoliticaEquidadMujer,
            'CONCPOL9': vm.validarConceptoPoliticaGEca,
            'CONCPOL10': vm.validarConceptoPoliticaGEci,
            'CONCPOL11': vm.validarConceptoPoliticaGEcn,
            'CONCPOL12': vm.validarConceptoPoliticaGEcp,
            'CONCPOL13': vm.validarConceptoPoliticaGRcr,
            'CONCPOL14': vm.validarConceptoPoliticaGEpr,
            'CONCPOL20': vm.validarConceptoPoliticaVictimas,
        }


        vm.guardado = function (nombreComponenteHijo) {
            vm.guardadocomponent({ nombreComponente: vm.nombreComponente, nombreComponenteHijo: nombreComponenteHijo });
        }
    }

    angular.module('backbone').component('focalizacionconcepto', {
        templateUrl: "src/app/formulario/ventanas/ajustes/componentes/focalizacion/Concepto/focalizacionconcepto.html",
        controller: focalizacionconceptoController,
        controllerAs: "vm",
        bindings: {
            callback: '&',
            notificacionvalidacion: '&',
            notificacionestado: '&',
            guardadoevent: '&',
            notificarrefresco: '&',
            notificacioncambios: '&',
            guardadocomponent: '&'
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
        })
        .directive('nksOnlyNumber', function () {
            return {
                restrict: 'EA',
                require: 'ngModel',
                link: function (scope, element, attrs, ngModel) {
                    scope.$watch(attrs.ngModel, function (newValue, oldValue) {
                        var spiltArray = String(newValue).split("");

                        if (attrs.allowNegative == "false") {
                            if (spiltArray[0] == '-') {
                                newValue = newValue.replace("-", "");
                                ngModel.$setViewValue(newValue);
                                ngModel.$render();
                            }
                        }

                        if (attrs.allowDecimal == "false") {
                            newValue = parseInt(newValue);
                            ngModel.$setViewValue(newValue);
                            ngModel.$render();
                        }

                        if (attrs.allowDecimal != "false") {
                            if (attrs.decimalUpto) {
                                var n = String(newValue).split(".");
                                if (n[1]) {
                                    var n2 = n[1].slice(0, attrs.decimalUpto);
                                    newValue = [n[0], n2].join(".");
                                    ngModel.$setViewValue(newValue);
                                    ngModel.$render();
                                }
                            }
                        }


                        if (spiltArray.length === 0) return;
                        if (spiltArray.length === 1 && (spiltArray[0] == '-' || spiltArray[0] === '.')) return;
                        if (spiltArray.length === 2 && newValue === '-.') return;

                        /*Check it is number or not.*/
                        if (isNaN(newValue)) {
                            ngModel.$setViewValue(oldValue || '');
                            ngModel.$render();
                        }
                    });
                }
            };
        });
})();