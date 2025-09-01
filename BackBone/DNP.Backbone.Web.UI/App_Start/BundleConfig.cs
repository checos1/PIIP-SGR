namespace DNP.Backbone.Web.UI
{
    using Microsoft.SharePoint.Client;
    using Microsoft.Win32;
    using System.CodeDom.Compiler;
    using System.Diagnostics.CodeAnalysis;
    using System.Web.Optimization;
 
    [ExcludeFromCodeCoverage]
    //Autogenerado o de Configuración. Se excluye de la cobertura porque este código se autogenero con la instalación de alguna librería y/o es una clase de configuración para el funcionamiento de la aplicación.
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {


            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                       "~/Scripts/jqueryui/jquery-ui.js",
                       "~/Scripts/raphael/raphael.js",
                        "~/Scripts/zoompan/raphael.pan-zoom.js",
                        "~/Scripts/FileSaver.js",
                        "~/Scripts/xlsx.full.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/powerbi").Include(
            "~/Scripts/powerbi.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js",
                      "~/Scripts/popper.min.js",
                      "~/Scripts/ui-bootstrap-tpls-1.3.3.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/ui-grid.css",
                      "~/Content/site.css",
                      "~/Content/ui-accordion.css",
                      "~/Content/sweetalert.css",
                      "~/Content/main.css",
                      "~/Content/tooltipCustom.css",
                      "~/Content/datetimepicker.css",
                      "~/Content/angular-block-ui.css",
                      "~/Content/font-awesome.css",
                      "~/Content/Slick/slick.css",
                      "~/Content/Slick/slick-theme.css",
                      "~/Content/angular-multi-select.css",
                      "~/Content/isteven-multi-select.css",
                      "~/Content/select.min.css",
                      "~/Content/sAlert/sAlert.css",
                      "~/Content/toastr.min.css",
                      "~/Content/quilljs/1.3.6/themes/quill.snow.css",
                      "~/Content/angular-chosen/1.8.7/chosen.min.css",
                      "~/Content/container.css",
                      "~/Content/container-screens.css",
                      "~/Scripts/zoompan/raphael.pan-zoom.css",
                      "~/Content/css/ventanas/ui-tramites.css",
                      "~/Content/css/ventanas/ui-general.css",
                      "~/Content/textangular/textAngular.css",
                      "~/Content/css/comunes/comunes.css",
                      "~/Content/tableDNP.css",
                      "~/Content/flujo.css"
                      ));

            bundles.Add(new ScriptBundle("~/bundles/datetimepicker").Include(
                "~/Scripts/datetimepicker.js",
                "~/Scripts/datetimepicker.templates.js",
                "~/Scripts/ng-bs-daterangepicker.js",
                "~/Scripts/daterangepicker.js",
                "~/Scripts/angular-date-time-input.js"));

            bundles.Add(new ScriptBundle("~/bundles/moment").Include(
                "~/Scripts/moment/moment.min.js",
                "~/Scripts/moment/moment-range.js",
                "~/Scripts/moment/moment-timezone-with-data.js",
                "~/Scripts/moment/es.js"));

            bundles.Add(new ScriptBundle("~/bundles/dropzone").Include(
                                        "~/Scripts/dropzone.js"));

            bundles.Add(new ScriptBundle("~/bundles/jspath").Include(
                                        "~/Scripts/jspath.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/angular").Include(
                "~/Scripts/angular.js",
                "~/Scripts/angular-animate.js",
                "~/Scripts/angular-route.js",
                "~/Scripts/ngStorage.js",
                "~/Scripts/underscore.js",
                "~/Scripts/angular-block-ui.js",
                "~/Scripts/ui-grid/csv.js",
                "~/Scripts/ui-grid/pdfmake.js",
                "~/Scripts/ui-grid/vfs_fonts.js",
                "~/Scripts/ui-grid.js",
                "~/Scripts/sweetalert.min.js",
                "~/Scripts/toastr.min.js",
                "~/Scripts/moment.min.js",
                "~/Scripts/moment-with-locales.min.js",
                "~/Scripts/Slick/slick.js",
                "~/Scripts/Slick/angular-slick.min.js",
                "~/Scripts/Slick/angular-slick.js",
                "~/Scripts/angular-multi-select.js",
                "~/Scripts/angularjs-dropdown-multiselect.min.js",
                "~/Scripts/isteven-multi-select.js",
                "~/Scripts/angular-messages.js",
                "~/Scripts/angular-ui-tree.min.js",
                "~/Scripts/select.js",
                "~/Scripts/angular-sanitize.js",
                "~/Scripts/jquery.mCustomScrollbar.concat.min.js",
                "~/Scripts/angular-ui/ui-bootstrap-tpls.js",
                "~/Scripts/sAlert/sAlert.js",
                "~/Scripts/angular-input-masks-standalone.min.js",
                "~/Scripts/mask.min.js",
                "~/Scripts/draggable-rows.js",
                "~/Scripts/quilljs/1.3.6/quill.min.js",
                "~/Scripts/angular-bootstrap-multiselect/angular-bootstrap-multiselect.min.js",
                "~/Scripts/angular-chosen/1.8.7/chosen.jquery.min.js",
                "~/Scripts/angular-chosen/1.8.7/chosen.proto.min.js",
                "~/Scripts/angular-chosen/1.8.7/angular-chosen.min.js",
                "~/Scripts/textangular/textAngular-rangy.min.js",
                "~/Scripts/textangular/textAngular-sanitize.js",
                "~/Scripts/textangular/textAngular-sanitize.min.js",
                "~/Scripts/textangular/textAngular.js",
                "~/Scripts/textangular/textAngular.min.js",
                "~/Scripts/textangular/textAngular.umd.js",
                "~/Scripts/textangular/textAngularSetup.js",
                "~/src/app/app.js",


            #region Administracion
                "~/src/app/administracion/ejecutor/ejecutorController.js",
                "~/src/app/administracion/ejecutor/ejecutorServicio.js",
                "~/src/app/administracion/documentos/documentoController.js",
                "~/src/app/administracion/documentos/documentoServicio.js",
                "~/src/app/administracion/documentos/modaluso/modalusoController.js",
                "~/src/app/administracion/documentos/modaluso/modalusoServicio.js",
            #endregion Administracion

            #region Documento Soporte

                "~/src/app/formulario/ventanas/comun/documentoSoporte/documentoSoporteServicios.js",
                "~/src/app/formulario/ventanas/comun/documentoSoporte/archivosFormularioTrvController.js",
                "~/src/app/formulario/ventanas/comun/documentoSoporte/modalVersionCargarDoc/cargarVersionDocModalController.js",
                "~/src/app/formulario/ventanas/comun/documentoSoporte/modalCargarDoc/cargarDocModalController.js",
                "~/src/app/formulario/ventanas/comun/documentoSoporte/modalHistoricoVersiones/historicoArhivosSoporteModalController.js",
                "~/src/app/formulario/ventanas/comun/documentoSoporte/modalHistoricoVersiones/historicoVersionesAnterioresModalController.js",

            #endregion Documento Soporte

            #region Comunes

                "~/src/app/comunes/serviciosApp.js",
                "~/src/app/comunes/constantesApp.js",
                "~/src/app/comunes/core/utilidades.js",
                "~/src/app/comunes/core/core.module.js",
                "~/src/app/comunes/core/interceptor.js",
                "~/src/app/comunes/core/core.filtros.js",
                "~/src/app/comunes/core/llamarFuncionConTecla.js",
                "~/src/app/comunes/signOut/controladorSignOut.js",
                "~/src/app/comunes/templates/modales/seleccionarEntidades/seleccionarEntidadesController.js",
                "~/src/app/comunes/templates/modales/seleccionarProyectos/seleccionarProyectos.js",
                "~/src/app/comunes/templates/modales/seleccionarProyectos/seleccionarProyectosSGR.js",
                "~/src/app/comunes/templates/modales/seleccionarColumnas/controladorSeleccionarColumnas.js",
                "~/src/app/comunes/templates/modales/adicionarNuevosItens/controladorAdicionarNuevosItens.js",
                "~/src/app/comunes/templates/modales/agregarPreguntas/controladorAgregarPreguntas.js",
                "~/src/app/comunes/rutasApp.js",
                "~/src/app/comunes/notificaciones/serviciosComponenteNotificaciones.js",
                "~/src/app/comunes/notificaciones/componenteNotificaciones.js",
                "~/src/app/comunes/notificaciones/notificacionCantidadProyectos/componenteNotificacionCantidadProyectos.js",
                "~/src/app/comunes/notificaciones/notificacionCantidadProyectos/serviciosComponenteNotificacionCantidadProyectos.js",
                "~/src/app/comunes/notificaciones/notificacionCantidadAlertas/componenteNotificacionCantidadAlertas.js",
                "~/src/app/comunes/notificaciones/notificacionCantidadAlertas/serviciosComponenteNotificacionCantidadAlertas.js",
                "~/src/app/comunes/notificaciones/notificacionCantidadTramites/componenteNotificacionCantidadTramites.js",
                "~/src/app/comunes/notificaciones/notificacionCantidadTramites/serviciosComponenteNotificacionCantidadTramites.js",
                "~/src/app/comunes/notificaciones/notificacionCantidadMensajesMtto/componenteNotificacionCantidadMensajesMtto.js",
                "~/src/app/comunes/notificaciones/notificacionCantidadMensajesMtto/modales/modalMensajesMttoController.js",
                "~/src/app/comunes/notificaciones/notificacionCantidadMisProcesos/componenteNotificacionCantidadMisProcesos.js",
                "~/src/app/comunes/notificaciones/modales/modalCrearEditarTareaSolicitudController.js",
                "~/src/app/comunes/componentes/mensajesMantenimiento/mensajesMantenimiento.js",
                "~/src/app/comunes/templates/modales/seleccionarProyectos/modales/modalInstanciasSeleccionarProyecto.js",
                "~/src/app/comunes/templates/modales/seleccionarProyectos/servicioSeleccionarProyectos.js",
                "~/src/app/comunes/componentes/consultaConpes/consultaConpesController.js",
                "~/src/app/formulario/ventanas/comun/certificadoDisponibilidadPresupuestal/componentes/cargaMasivaCdpService.js",
                "~/src/app/formulario/ventanas/comun/certificadoDisponibilidadPresupuestal/certificadoDisponibilidadPresupuestal.js",
                "~/src/app/formulario/ventanas/comun/certificadoDisponibilidadPresupuestal/componentes/agregarCdpProyecto/agregarCdpProyecto.js",
                "~/src/app/formulario/ventanas/comun/certificadoDisponibilidadPresupuestal/componentes/agregarCdpProyectoModal/agregarCdpProyectoModal.js",
                "~/src/app/comunes/log/modalLogInstancias.js",
                "~/src/app/comunes/log/logInstanciasServicio.js",
                "~/src/app/comunes/log/modalLogInstanciasSubpasos.js",
                "~/src/app/comunes/log/logInstanciasSubpasosServicio.js",
                "~/src/app/comunes/catalogos/catalogoServicio.js",
                "~/src/app/formulario/ventanas/comun/consultaConpes/consultaConpesFormulario.js",
                 "~/src/app/formulario/ventanas/SGR/comun/validacionArchivosServicio.js",

            #endregion

                "~/src/app/panelPrincial/controladorPanelPrincipal.js",
                "~/src/app/panelPrincial/controladorPanelProyectosPorTramite.js",
                "~/src/app/panelPrincial/componentes/inbox/controladorConsolaProcesosDefault.js",
                "~/src/app/panelPrincial/componentes/inbox/controladorInbox.js",
                "~/src/app/panelPrincial/componentes/inbox/controladorTramites.js",
                "~/src/app/panelPrincial/componentes/inbox/controladorProgramacionMP.js",
                "~/src/app/panelPrincial/componentes/inbox/controladorProyectosPorTramite.js",
                "~/src/app/panelPrincial/modales/subpasoOcad/observacionModalController.js",
                "~/src/app/panelPrincial/modales/logs/logsModalController.js",
                "~/src/app/panelPrincial/modales/logs/servicioLogsModal.js",
                "~/src/app/panelPrincial/modales/logs/historicoObservacionesModalController.js",
                "~/src/app/panelPrincial/modales/logs/historicoObservacionesModalServicio.js",
                "~/src/app/panelPrincial/modales/tramite/alcanceModalController.js",
                "~/src/app/panelPrincial/modales/tramite/servicioAlcanceModal.js",
                "~/src/app/panelPrincial/modales/trazabilidad/trazabilidadModalController.js",
                "~/src/app/panelPrincial/modales/trazabilidad/trazabilidadModalServicio.js",
                "~/src/app/panelPrincial/componentes/inbox/controladorMisProcesos.js",
                "~/src/app/panelNotificaciones/controladorPanelNotificaciones.js",
                "~/src/app/inicio/controladorInicio.js",
                "~/src/app/inicio/controladorEjemplo.js",
                "~/src/app/autorizando/controladorAutorizando.js",
                "~/src/app/configurarColumnas/controladorConfigurarColumnas.js",
                "~/src/app/panelPrincial/servicioPanelPrincipal.js",
                "~/src/app/panelEjecucionDeAccion/servicioPanelEjecucionDeAccion.js",
                "~/src/app/panelEjecucionDeAccion/controladorPanelEjecucionDeAccion.js",
                "~/src/app/servicios/backboneServiciosNegocio.js",
                "~/src/app/servicios/flujoServicios.js",
                "~/src/app/servicios/estadoAplicacionServicios.js",
                "~/src/app/servicios/proyectoServicio.js",
                "~/src/app/servicios/CacheServicios.js",
                "~/src/app/servicios/manejadorArchivosServicios.js",
                "~/src/app/autorizacion/validarAutorizacionController.js",
                "~/src/app/autorizacion/configurarEntidadRolSectorController.js",
                "~/src/app/autorizacion/componentes/crearModificarConfiguracion/crearModificarConfiguracionController.js",
                "~/src/app/autorizacion/servicios/configurarEntidadRolSectorServicio.js",
                "~/src/app/autorizacion/servicios/constantesAutorizacion.js",
                "~/src/app/autorizacion/servicios/autorizacionServicios.js",
                "~/src/app/autorizacion/servicios/sesionServicios.js",
                "~/src/app/acciones/consultarAcciones/componentes/simbologia.js",
                "~/src/app/acciones/consultarAcciones/consultarAccionesController.js",
                "~/src/app/acciones/consultarAcciones/modales/modalFlujoController.js",
                "~/src/app/acciones/consultarAcciones/ejecucionFormulariosController.js",
                "~/src/app/acciones/consultarAcciones/previsualizadorFormulariosController.js",
                "~/src/app/formulario/formulario.module.js",
                "~/src/app/formulario/servicioCreditos.js",
                "~/src/app/formulario/componentes/modificarFilaPrevisualizar/modificarFilaPrevisualizarController.js",
                "~/src/app/formulario/componentes/WBS/servicios/templates.servicio.js",
                "~/src/app/formulario/componentes/WBS/servicios/wbs.servicio.js",
                "~/src/app/formulario/componentes/WBS/directivas/jerarquia/jerarquia.directiva.js",
                "~/src/app/formulario/componentes/WBS/directivas/tabla/tabla.directiva.js",
                "~/src/app/formulario/componentes/WBS/directivas/adicionar/adicionar.directiva.js",
                "~/src/app/formulario/componentes/WBS/wbs.directiva.js",
                "~/src/app/acciones/servicios/servicioVisualizacionFormulario.js",
                "~/src/app/acciones/servicios/ejecutorReglasServicios.js",
                "~/src/app/acciones/servicios/schemaFormServicios.js",
                "~/src/app/acciones/servicios/formularioServicios.js",
                "~/src/app/acciones/servicios/ServicioAcciones.js",
                "~/src/app/archivos/archivo.module.js",
                "~/src/app/archivos/constantesArchivos.js",
                "~/src/app/archivos/directivas/directivasArchivo.js",
                "~/src/app/archivos/directivas/dropZoneCargaDirectiva.js",
                "~/src/app/archivos/cargaarchivos/cargaArchivosController.js",
                "~/src/app/archivos/template/modales/consultarMetadatosController.js",
                "~/src/app/archivos/servicios/archivoServicios.js",
                "~/src/app/panelEjecucionDeAccion/listarAccionesAnterioresController.js",
                "~/src/app/comunes/core/directive.module.js",
                "~/src/app/panelPrincial/componentes/inbox/controladorConsolaProcesos.js",
                "~/src/app/panelPrincial/componentes/inbox/controladorConsolaProcesosTramites.js",
                "~/src/app/panelPrincial/componentes/inbox/controladorConsolaProcesosProgramacion.js",
                "~/src/app/fichasProyectos/servicios/servicioFichasProyectos.js",
                "~/src/app/fichasProyectos/template/modales/fichaTemplateController.js",
                "~/src/app/formulario/ventanas/seguimientoControl/componentes/DiagramaGantt/reporteGanttController.js",
                "~/src/app/formulario/ventanas/seguimientoControl/componentes/DiagramaGantt/reporteGanttServicio.js",
                "~/src/app/formulario/ventanas/seguimientoControl/componentes/DiagramaEstructuraActividades/reporteEstructuraActividadesController.js",
                "~/src/app/formulario/ventanas/seguimientoControl/componentes/DiagramaEstructuraActividades/reporteEstructuraActividadesServicio.js",
                "~/src/app/formulario/ventanas/seguimientoControl/componentes/reporteRegionalizacion/reporteRegionalizacionController.js",
                "~/src/app/formulario/ventanas/seguimientoControl/componentes/reporteRegionalizacion/reporteRegionalizacionServicio.js",
                "~/src/app/formulario/ventanas/seguimientoControl/componentes/reporteRegionalizacion/componentes/reporteAvanceRegionalizacion/reporteAvanceRegionalizacionCapController.js",
                "~/src/app/formulario/ventanas/seguimientoControl/componentes/reporteRegionalizacion/componentes/reporteAvanceRegionalizacion/reporteAvanceRegionalizacionCapServicio.js",
                "~/src/app/formulario/ventanas/seguimientoControl/componentes/reporteRegionalizacion/componentes/resumenAvanceRegionalizacion/resumenAvanceRegionalizacionCapController.js",
                "~/src/app/formulario/ventanas/seguimientoControl/componentes/reporteRegionalizacion/componentes/resumenAvanceRegionalizacion/resumenAvanceRegionalizacionCapServicio.js",
                "~/src/app/formulario/ventanas/seguimientoControl/componentes/seguimientoFocalizacion/seguimientoFocalizacionController.js",
                "~/src/app/formulario/ventanas/seguimientoControl/componentes/seguimientoFocalizacion/seguimientoFocalizacionServicio.js",
                "~/src/app/formulario/ventanas/seguimientoControl/componentes/seguimientoFocalizacion/componentes/segIndicadorPoliticas/indicadorPoliticas/indicadorPoliticasController.js",
                "~/src/app/formulario/ventanas/seguimientoControl/componentes/seguimientoFocalizacion/componentes/segIndicadorPoliticas/indicadorPoliticas/indicadorPoliticasServicio.js",
                "~/src/app/formulario/ventanas/seguimientoControl/componentes/seguimientoFocalizacion/componentes/segIndicadorPoliticas/listadoPoliticas/listadoPoliticasController.js",
                "~/src/app/formulario/ventanas/seguimientoControl/componentes/seguimientoFocalizacion/componentes/segIndicadorPoliticas/listadoPoliticas/listadoPoliticasServicio.js",

                "~/src/app/formulario/ventanas/seguimientoControl/componentes/seguimientoFocalizacion/componentes/segFocalizacionPoliticas/focalizacionPoliticas/focalizacionPoliticasController.js",
                "~/src/app/formulario/ventanas/seguimientoControl/componentes/seguimientoFocalizacion/componentes/segFocalizacionPoliticas/focalizacionPoliticas/focalizacionPoliticasServicio.js",

                "~/src/app/formulario/ventanas/seguimientoControl/componentes/seguimientoFocalizacion/componentes/segCrucePoliticas/crucePoliticas/crucePoliticasController.js",
                "~/src/app/formulario/ventanas/seguimientoControl/componentes/seguimientoFocalizacion/componentes/segCrucePoliticas/crucePoliticas/crucePoliticasServicio.js",

            #region Directives
                "~/src/app/comunes/directives/numbersOnly.directive.js",
                "~/src/app/comunes/directives/mask-regex/maskRegex.directive.js",
                "~/src/app/comunes/directives/modal-dialog/modalDialog.directive.js",
                "~/src/app/comunes/directives/hasClaim.directive.js",
                "~/src/app/comunes/directives/chosen/chosenKeyPress.directive.js",
                "~/src/app/comunes/directives/directivasUtilidades.js",
                "~/src/app/comunes/directives/numbers/numbers.directive.js",
            #endregion

            #region Usuarios
                "~/src/app/usuarios/usuarios.module.js",
                "~/src/app/usuarios/servicioUsuarios.js",
                "~/src/app/usuarios/asignarClaveUsuario/asignarClaveUsuarioController.js",
                "~/src/app/usuarios/recuperarClaveUsuario/recuperarClaveUsuarioController.js",
                "~/src/app/usuarios/usuarioPerfil/usuarioPerfilController.js",
                "~/src/app/usuarios/usuarioPerfil/modales/roles/modalRolesPerfilController.js",
                "~/src/app/usuarios/usuarioPerfil/modales/proyectos/modalProyectosPerfilController.js",
                "~/src/app/usuarios/usuarioPerfil/modales/modalAccionUsuarioPerfilController.js",
                "~/src/app/usuarios/usuarios/usuariosController.js",
                "~/src/app/usuarios/usuarios/usuariosTerritorioController.js",
                "~/src/app/usuarios/usuarios/modales/modalTipoInvitacionUsuarioController.js",
                "~/src/app/usuarios/usuarios/modales/modalTipoInvitacionUsuarioTerritorioController.js",
                //"~/src/app/usuarios/usuarios/modales/modalInvitarUsuarioController.js",
                "~/src/app/usuarios/usuarios/modales/modalInvitarUsuarioDNPController.js",
                "~/src/app/usuarios/usuarios/modales/modalInvitarUsuarioExternoController.js",
                "~/src/app/usuarios/usuarios/modales/modalInvitarUsuarioExternoTerritorioController.js",
                "~/src/app/usuarios/usuarios/modales/modalAccionUsuarioController.js",
                 "~/src/app/usuarios/usuarios/modales/modalPerfilesUsuarioController.js",
                 "~/src/app/usuarios/usuarios/modales/modalPerfilesUsuarioTerritorioController.js",
                  "~/src/app/usuarios/usuarios/modales/modalRolesXPerfilController.js",
                "~/src/app/usuarios/perfiles/perfilesController.js",
                "~/src/app/usuarios/perfiles/modales/modalAccionPerfilController.js",
                "~/src/app/usuarios/roles/rolesController.js",
                "~/src/app/usuarios/roles/modales/modalAccionRoleController.js",
                "~/src/app/usuarios/componentes/informacionesUsuario/informacionesUsuario.js",
                "~/src/app/usuarios/modales/cambiarContrasena/cambiarContrasenaController.js",
            #endregion Usuários

            #region Entidades

                "~/src/app/entidades/entidades.module.js",
                "~/src/app/entidades/servicioEntidades.js",
                "~/src/app/entidades/servicioCargaDatos.js",
                "~/src/app/entidades/servicioEntidadProyectos.js",
                "~/src/app/entidades/entidades/entidadesController.js",
                "~/src/app/entidades/entidades/unidadResponsableController.js",
                "~/src/app/entidades/entidades/tipoEntidad/entidades.directive.js",
                "~/src/app/entidades/entidades/tipoEntidad/nacionalController.js",
                "~/src/app/entidades/entidades/tipoEntidad/territorialController.js",
                "~/src/app/entidades/entidades/tipoEntidad/sgrController.js",
                "~/src/app/entidades/entidades/tipoEntidad/publicasController.js",
                "~/src/app/entidades/entidades/tipoEntidad/privadasController.js",
                "~/src/app/entidades/entidades/modales/modalCrearEditarController.js",
                "~/src/app/entidades/entidades/modales/flujosViabilidadModalController.js",
                "~/src/app/entidades/entidades/tipoEntidad/entidad-sgr/modales/adherencia/adherenciaModalController.js",
                "~/src/app/entidades/entidades/tipoEntidad/entidad-sgr/modales/adherencia/servicioAdherencia.js",
                "~/src/app/entidades/entidades/tipoEntidad/entidad-sgr/modales/sub-entidad/subEntidadModalController.js",
                "~/src/app/entidades/entidades/tipoEntidad/entidad-sgr/modales/flujos-viabilidad/flujosViabilidadModalController.js",
                "~/src/app/entidades/entidades/tipoEntidad/entidad-sgr/modales/delegado/delegadoModalController.js",
                "~/src/app/entidades/entidades/tipoEntidad/entidad-sgr/modales/delegado/servicioDelegado.js",
                "~/src/app/entidades/entidades/tipoEntidad/entidad-sgr/modales/crearEditarEntidad/crearEditarEntidadModalController.js",
                "~/src/app/entidades/entidades/tipoEntidad/entidad-sgr/inflexibilidades/inflexibilidadesLista/inflexibilidadesListaController.js",
                "~/src/app/entidades/entidades/tipoEntidad/entidad-sgr/inflexibilidades/inflexibilidadServicio.js",
                "~/src/app/entidades/entidades/tipoEntidad/entidad-sgr/modales/inflexibilidad/modalCrearEditarInflexibilidadController.js",
                "~/src/app/entidades/entidades/tipoEntidad/entidad-sgr/modales/consultarPagos/consultarPagosModalController.js",
                "~/src/app/entidades/cargaDatos/modales/modalCrearCargaDatosController.js",
                "~/src/app/entidades/cargaDatos/modales/modalVerCargaDatosController.js",
                "~/src/app/entidades/cargaDatos/cargaDatosController.js",
                "~/src/app/entidades/cargarDatos/cargarDatosController.js",
                "~/src/app/entidades/proyectos/entidadProyectosController.js",

            #endregion


                "~/src/app/consola/tramites/modales/modalDocumentosController.js",
                //"~/src/app/consola/tramites/modales/modalArchivosTramitesController.js",

                "~/src/app/comunes/notificaciones/notificacionCantidadTramites/serviciosComponenteNotificacionCantidadTramites.js",
                "~/src/app/comunes/componentes/tablaEntidad/componenteTablaEntidad.js",
                "~/src/app/panelPrincial/test/controladorTabla.js",
                "~/src/app/consola/proyectos/componentes/proyectos/modales/modalInstanciasProyecto.js",
                "~/src/app/consola/proyectos/componentes/proyectos/modales/modalDocumentosAdjuntosController.js",
                "~/src/app/consola/proyectos/componentes/proyectos/modales/modalDocumentosProyectosController.js",
                "~/src/app/inicio/modalEjemploController.js",
                 "~/src/app/inicio/modal2EjemploController.js",
                "~/src/app/consola/modales/fichas/modalFichasController.js",
                "~/src/app/archivos/cargaarchivos/cargarArchivo.js",

                "~/src/app/formulario/ventanas/transversal/justificacionCambios/componentes/focalizacion/categoriaspoliticastransversales/focalizacioncategoriaspolitController.js",
                "~/src/app/formulario/ventanas/transversal/justificacionCambios/componentes/focalizacion/categoriaspoliticastransversales/focalizacioncategoriaspolitServicio.js",
                "~/src/app/formulario/ventanas/comun/servicios/comunesServicio.js",

            #region "Consola Monitoreo"
                "~/src/app/comunes/componentes/powerBI/componentePowerBI.js",
                "~/src/app/monitoreo/consolaMonitoreoController.js",
                "~/src/app/monitoreo/servicioConsolaMonitoreo.js",
                "~/src/app/monitoreo/consolaAlertasController.js",
                "~/src/app/monitoreo/servicioConsolaAlertas.js",
                "~/src/app/monitoreo/template/modales/crearAlertaConfiguracionController.js",
                "~/src/app/monitoreo/template/modales/alertas/alertasGeneradasController.js",
                "~/src/app/monitoreo/servicioConsolaMonitoreo.js",
                "~/src/app/monitoreo/consolaAlertasController.js",
                "~/src/app/monitoreo/servicioConsolaAlertas.js",
                "~/src/app/monitoreo/template/consolaMonitoreoEntidadController.js",
                "~/src/app/monitoreo/componentes/consolaMonitoreoEntidadComponenteController.js",
                "~/src/app/monitoreo/componentes/reportes/proyecto/consolaProyectoComponenteController.js",
                "~/src/app/monitoreo/template/reportes/proyecto/consolaMonitoreoProyecto.js",

            #endregion "Consola Monitoreo"

            #region "Tablero de Control"

                "~/src/app/tableroControl/tableroControlComponenteController.js",
                "~/src/app/tableroControl/serviciosComponenteTableroControl.js",

            #endregion "Tablero de Control"

            #region Extensions
                "~/src/app/extensiones/extensions.module.js",
                "~/src/app/extensiones/array-extensions.js",
                "~/src/app/extensiones/object-extensiones.js",
            #endregion

            #region Models
                "~/src/app/model/model.module.js",
                "~/src/app/model/base/ModelBase.js",
                "~/src/app/model/AlertaMonitoreoConfigModel.js",
                "~/src/app/model/AlertaMonitoreoReglaModel.js",
                "~/src/app/model/AlertaMonitoreoGeneradaModel.js",
                "~/src/app/model/MapColumnaModel.js",
                "~/src/app/model/MensajeMantenimientoModel.js",
                "~/src/app/model/RoleModel.js",
                "~/src/app/model/MensajeEntidadModel.js",
                "~/src/app/model/EntidadModel.js",
                "~/src/app/model/AyudaTemaListaItemModel.js",
                "~/src/app/model/ProcedimientoAlmacenadoModel.js",
                "~/src/app/model/UsuarioNotificacionConfigModel.js",
                "~/src/app/model/UsuarioNotificacionModel.js",
            #endregion

            #region Constantes
                "~/src/app/model/Constantes/CondicionalConstante.js",
                "~/src/app/model/Constantes/OperadorConstante.js",
                "~/src/app/model/Constantes/TipoValorColumnaConstante.js",
                "~/src/app/model/Constantes/TipoAlertaConstante.js",
                "~/src/app/model/Constantes/TipoColumnaConstante.js",
                "~/src/app/model/Constantes/ClassificacionAlertaConstante.js",
                "~/src/app/model/Constantes/OpcionesContante.js",
                "~/src/app/model/Constantes/AyudaTipoConstante.js",
            #endregion

            #region Mensajes
                "~/src/app/mensajesMantenimiento/template/crearActualizarMensaje/crearActualizarMensajeController.js",
                "~/src/app/mensajesMantenimiento/template/modales/visualizarMensajePopUp/visualizarMensajePopUpController.js",
                "~/src/app/mensajesMantenimiento/componentes/visualizarMensajeDisclaimer/visualizarMensajeDisclaimer.js",
                "~/src/app/mensajesMantenimiento/servicios/mensajeServicio.js",
                "~/src/app/dadosLogs/template/plantillaLogsTramitesYProyectosController.js",
                "~/src/app/mensajesMantenimiento/template/listaMensajes/listaMensajesMantenimientoController.js",
            #endregion

            #region Consola
                "~/src/app/consola/proyectos/consolaProyectosController.js",
                "~/src/app/consola/proyectos/servicioConsolaProyectos.js",
                "~/src/app/consola/proyectos/componentes/proyectos/controladorProyecto.js",
                 "~/src/app/consola/proyectos/modales/modalIntercambioController.js",
                "~/src/app/consola/tramites/consolaTramitesController.js",
                "~/src/app/consola/tramites/servicioConsolaTramites.js",
                "~/src/app/consola/tramites/controladorPanelProyectosPorTramiteConsola.js",
                "~/src/app/consola/tramites/controladorProyectosPorTramite.js",
            #endregion

            #region  Formularios
                 "~/src/app/formulario/modales/selecionarProyectosCreditosController.js",
                 "~/src/app/formulario/modales/seleccionDeProyectosController.js",
                 "~/src/app/formulario/modales/resumenDeProyectosController.js",
                 "~/src/app/formulario/modales/servicioResumenDeProyectos.js",
                 "~/src/app/acciones/consultarAcciones/formularios/FormularioPruebaController.js",
                 "~/src/app/acciones/consultarAcciones/formularios/FormularioTramitePruebaController.js",
                 "~/src/app/formulario/ventanas/tramites/componentes/carta/principalCarta.js",
                 "~/src/app/formulario/ventanas/tramites/componentes/carta/datosIniciales.js",
                 "~/src/app/formulario/ventanas/tramites/componentes/carta/datosDespedida.js",
                 "~/src/app/formulario/ventanas/tramites/componentes/carta/cartaServicio.js",
                 "~/src/app/formulario/ventanas/tramites/componentes/carta/cuerpoConcepto.js",
            #endregion

            #region  VentanasMedida
                  "~/src/app/formulario/ventanas/comun/pestanasFormulario.js",
                  "~/src/app/formulario/ventanas/comun/pestanasFormularioServicio.js",
                  "~/src/app/formulario/ventanas/tramites/trasladosController.js",
                  "~/src/app/formulario/ventanas/tramites/trasladosServicio.js",
                  "~/src/app/formulario/ventanas/comun/botonesFormulario.js",
                  "~/src/app/formulario/ventanas/comun/observacionesFormulario.js",
                  "~/src/app/formulario/ventanas/comun/encabezadoGeneral.js",
                  "~/src/app/formulario/ventanas/comun/encabezadoSGR/encabezadoSGR.js",
                  "~/src/app/formulario/ventanas/comun/encabezadoSGR/encabezadoSGRServicio.js",
                  "~/src/app/formulario/ventanas/comun/encabezadoSGP/encabezadoSgp.js",
                  "~/src/app/formulario/ventanas/comun/encabezadoSGP/encabezadoSgpServicio.js",
                  "~/src/app/formulario/ventanas/comun/informacionPresupuestal/informacionPresupuestalFormulario.js",
                  "~/src/app/formulario/ventanas/comun/documentoSoporte/archivosFormulario.js",
                  "~/src/app/formulario/ventanas/comun/documentoSoporte/archivosFormularioPeriodo.js",
                  "~/src/app/formulario/ventanas/comun/firmaConcepto/firmaConceptoFormulario.js",
                  "~/src/app/formulario/ventanas/comun/solicitarConcepto/solicitudConceptoFormulario.js",
                  "~/src/app/formulario/ventanas/comun/elaborarConcepto/datosInicialesFormulario.js",
                  "~/src/app/formulario/ventanas/comun/elaborarConcepto/cuerpoConceptoFormulario.js",
                  "~/src/app/formulario/ventanas/comun/elaborarConcepto/datosDespedidaFormulario.js",
                  "~/src/app/formulario/ventanas/comun/asociarMuchosProyectos/asociarMuchosProyectosFormulario.js",
                  "~/src/app/formulario/ventanas/comun/asociarMuchosProyectos/detalleMuchosProyectos/detalleMuchosProyectosFormulario.js",
                  "~/src/app/formulario/ventanas/comun/asociarMuchosProyectos/asociarMuchosProyectosFormularioml.js",
                  "~/src/app/formulario/ventanas/comun/asociarMuchosProyectos/detalleMuchosProyectos/detalleMuchosProyectosFormularioml.js",
                  "~/src/app/formulario/ventanas/comun/asociarMuchosProyectos/asociarMuchosUnTipo/asociarMuchosUnTipoFormulario.js",
                  "~/src/app/formulario/ventanas/comun/asociarMuchosProyectos/asociarMuchosUnTipo/detalleMuchosUnTipo/detalleMuchosUnTipoFormulario.js",
                  "~/src/app/formulario/ventanas/comun/asociarMuchosProyectos/asociarProyectosUnTipo/asociarProyectosUnTipoFormulario.js",
                  "~/src/app/formulario/ventanas/comun/asociarMuchosProyectos/asociarProyectosUnTipo/detalleProyectosUnTipo/detalleProyectosUnTipoFormulario.js",
                  "~/src/app/formulario/ventanas/comun/asociarMuchosProyectos/asociarProyectosUnTipo/asociarProyectosUnTipoFormularioSgp.js",
                  "~/src/app/formulario/ventanas/comun/asociarMuchosProyectos/asociarProyectosUnTipo/detalleProyectosUnTipo/detalleProyectosUnTipoFormularioSgp.js",
                  "~/src/app/formulario/ventanas/comun/asociarProyecto/asociarProyectoFormulario.js",
                  "~/src/app/formulario/ventanas/comun/justificacion/justificacionFormularioController.js",
                  "~/src/app/formulario/ventanas/comun/informacionPresupuestalAprobacion/informacionPresupuestalAproFormulario.js",
                  "~/src/app/formulario/ventanas/comun/justificacionAprobacion/justificacionAproFormularioController.js",
                  "~/src/app/formulario/ventanas/comun/asociarMuchosProyectos/asociarMuchosUnTipo/asociarMuchosUnTipoFormularioa.js",
                  "~/src/app/formulario/ventanas/comun/asociarMuchosProyectos/asociarMuchosUnTipo/detalleMuchosUnTipo/detalleMuchosUnTipoFormularioa.js",
                  "~/src/app/formulario/ventanas/comun/asociarMuchosProyectos/asociarMuchosUnTipo/asociarMuchosUnTipoFormulariop.js",
                  "~/src/app/formulario/ventanas/comun/asociarMuchosProyectos/asociarMuchosUnTipo/detalleMuchosUnTipo/detalleMuchosUnTipoFormulariop.js",
                  "~/src/app/formulario/ventanas/comun/tablaResumen/tablaResumenFormulario.js",
                  "~/src/app/formulario/ventanas/comun/tablaVigenciaFuturaProducto/tablaVigenciaFuturaProductoFormulario.js",
                  "~/src/app/formulario/ventanas/comun/cronograma/cronogramaController.js",
                  "~/src/app/formulario/ventanas/comun/cronograma/cronogramaModificarFormularioController.js",

                  "~/src/app/formulario/ventanas/tramites/trasladoTramite.js",
                  "~/src/app/formulario/ventanas/tramites/trasladoAprobacion.js",
                  "~/src/app/formulario/ventanas/tramites/trasladosAccionesController.js",
                  "~/src/app/formulario/ventanas/tramites/componentes/justificacion/justificacion.js",
                  "~/src/app/formulario/ventanas/tramites/componentes/requerimientos/requerimientosTramite.js",
                  "~/src/app/formulario/ventanas/tramites/requerimientosTramites/requerimientosTramitesServicio.js",
                  "~/src/app/formulario/ventanas/tramites/seleccionProyectosTramiteController.js",
                  "~/src/app/formulario/ventanas/tramites/componentes/justificacion/justificacionProyecto.js",
                  "~/src/app/formulario/ventanas/tramites/componentes/documentoSoporte/documentoSoporte.js",
                  "~/src/app/formulario/ventanas/tramites/trasladosAprobacionFuentes.js",
                  "~/src/app/formulario/ventanas/tramites/requisitosAprobacion.js",
                  "~/src/app/formulario/ventanas/tramites/componentes/aprobacionFuentes.js",
                  "~/src/app/formulario/ventanas/tramites/componentes/cdp/cdpTramite.js",
                  "~/src/app/formulario/ventanas/tramites/componentes/solicitarconcepto/solicitarconceptoController.js",
                  "~/src/app/formulario/ventanas/tramites/componentes/solicitarconcepto/solicitarconceptoServicio.js",
                  "~/src/app/formulario/ventanas/gestionRecursos/gestionRecursosController.js",
                  "~/src/app/formulario/ventanas/gestionRecursos/gestionRecursosServicio.js",

                  "~/src/app/formulario/ventanas/gestionRecursos/componentes/datosgenerales/datosgeneralesGrController.js",
                  "~/src/app/formulario/ventanas/gestionRecursos/componentes/datosgenerales/localizacion/localizacion.js",


                  "~/src/app/formulario/ventanas/gestionRecursos/componentes/recursos/recursosGrController.js",
                  "~/src/app/formulario/ventanas/gestionRecursos/componentes/recursos/fuentes/fuentes.js",
                  "~/src/app/formulario/ventanas/gestionRecursos/componentes/recursos/fuentes/modalAgregarFuenteController.js",
                  "~/src/app/formulario/ventanas/gestionRecursos/componentes/recursos/fuentes/modalAgregarDatosAdicionalesController.js",
                  "~/src/app/formulario/ventanas/gestionRecursos/componentes/recursos/regionalizacionFuentes/regionalizacionFuentes.js",
                  "~/src/app/formulario/ventanas/gestionRecursos/componentes/recursos/enviarprogramacion/enviarProgramacionController.js",
                  "~/src/app/formulario/ventanas/gestionRecursos/componentes/recursos/enviarprogramacion/enviarProgramacionDetalle/enviarProgramaciondetController.js",

                  "~/src/app/formulario/ventanas/gestionRecursos/componentes/recursos/aprobaciongr/aprobacionGrcontroller.js",
                  "~/src/app/formulario/ventanas/gestionRecursos/componentes/recursos/aprobaciongr/aprobacionGrdetalle/aprobacionGrdetController.js",



                  "~/src/app/formulario/ventanas/tramites/componentes/conceptodirecciontecnica/conceptodirecciontecnicaController.js",
                  "~/src/app/formulario/ventanas/tramites/componentes/conceptodirecciontecnica/conceptodirecciontecnicaServicio.js",

                  "~/src/app/formulario/ventanas/ajustes/ajustesController.js",
                  "~/src/app/formulario/ventanas/ajustes/ajustesServicio.js",
                  "~/src/app/formulario/ventanas/ajustes/componentes/datosgenerales/datosgeneralesController.js",
                  "~/src/app/formulario/ventanas/ajustes/componentes/datosgenerales/datosgeneralesServicio.js",
                  "~/src/app/formulario/ventanas/ajustes/componentes/datosgenerales/conpes/conpesController.js",
                  "~/src/app/formulario/ventanas/ajustes/componentes/datosgenerales/conpes/conpesServicio.js",
                  "~/src/app/formulario/ventanas/ajustes/componentes/datosgenerales/horizonte/horizonteController.js",
                  "~/src/app/formulario/ventanas/ajustes/componentes/datosgenerales/horizonte/horizonteServicio.js",

                  "~/src/app/formulario/ventanas/ajustes/componentes/datosgenerales/beneficiarios/beneficiariosTotalesController.js",
                  "~/src/app/formulario/ventanas/ajustes/componentes/datosgenerales/beneficiarios/beneficiariosServicio.js",

                  "~/src/app/formulario/ventanas/ajustes/componentes/recursos/recursosAjustesController.js",
                  "~/src/app/formulario/ventanas/ajustes/componentes/recursos/recursosAjustesServicio.js",

                  "~/src/app/formulario/ventanas/ajustes/componentes/focalizacion/focalizacionAjustesServicio.js",
                  "~/src/app/formulario/ventanas/ajustes/componentes/focalizacion/focalizacionAjustesController.js",

                  "~/src/app/formulario/ventanas/ajustes/componentes/focalizacion/politicasTransversales/politicasTransversalesController.js",
                  "~/src/app/formulario/ventanas/ajustes/componentes/focalizacion/politicasTransversales/politicasTransversalesServicio.js",
                  "~/src/app/formulario/ventanas/ajustes/componentes/focalizacion/politicasTransversales/modalAgregarPoliticaTransversalController.js",
                  "~/src/app/formulario/ventanas/ajustes/componentes/focalizacion/ResumenFocalizacion/ResumenFocalizacionController.js",
                  "~/src/app/formulario/ventanas/ajustes/componentes/focalizacion/politicasTransversales/modalAgregarPoliticaTransversalAjustesController.js",
                  "~/src/app/formulario/ventanas/ajustes/componentes/focalizacion/ResumenFocalizacion/ResumenFocalizacionCategoria/ResumenFocalizacionCategoriaController.js",
                  "~/src/app/formulario/ventanas/ajustes/componentes/focalizacion/ResumenFocalizacion/ResumenFocalizacionCategoria/ResumenFocalizacionCatPasoTresFormularioController.js",
                  "~/src/app/formulario/ventanas/ajustes/componentes/focalizacion/Resumen/ResumenController.js",
                  "~/src/app/formulario/ventanas/ajustes/componentes/focalizacion/CrucePoliticasTransversales/crucePoliticasTransversalesController.js",
                  "~/src/app/formulario/ventanas/ajustes/componentes/focalizacion/Concepto/focalizacionconceptoController.js",
                  "~/src/app/formulario/ventanas/ajustes/componentes/focalizacion/ConsultarAprobarConceptoDT/consultaraprobarconceptodtController.js",

                  "~/src/app/formulario/ventanas/ajustes/componentes/focalizacion/ResumenFocalizacion/FocalizacionCategoria/modalAgregarCategoriaPoliticaController.js",
                 "~/src/app/formulario/ventanas/ajustes/componentes/focalizacion/ResumenFocalizacion/ResumenFocalizacionCategoria/Modal/modalAgregarIndicadorController.js",

                  "~/src/app/formulario/ventanas/ajustes/componentes/recursos/Fuentes/totalesFuentes/resumenFuentesServicio.js",
                  "~/src/app/formulario/ventanas/ajustes/componentes/recursos/Fuentes/totalesFuentes/rFuentesController.js",

                  "~/src/app/formulario/ventanas/ajustes/componentes/recursos/Fuentes/ResumenCostosAjustes/costosFuentesAjustesController.js",
                  "~/src/app/formulario/ventanas/ajustes/componentes/recursos/Fuentes/ResumenCostosAjustes/costosFuentesAjustesServicio.js",

                  "~/src/app/formulario/ventanas/ajustes/componentes/recursos/costoActividades/costoActividadesController.js",
                  "~/src/app/formulario/ventanas/ajustes/componentes/recursos/costoActividades/costoActividadesServicio.js",
                  "~/src/app/formulario/ventanas/ajustes/componentes/recursos/costoActividades/modal/agregarEntregableModalController.js",
                  "~/src/app/formulario/ventanas/ajustes/componentes/recursos/costoActividades/modal/agregarEntregableModal.js",
                  "~/src/app/formulario/ventanas/ajustes/componentes/recursos/costoActividades/ResumenCostosFuentesAjustes/resumenCostosFuentesAjustesController.js",

                  "~/src/app/formulario/ventanas/tramites/componentes/conceptotecnico/conceptoTecnicoController.js",
                  "~/src/app/formulario/ventanas/tramites/componentes/conceptotecnico/conceptoTecnicoServicio.js",

                  "~/src/app/formulario/ventanas/ajustes/componentes/recursos/regionalizacion/regionalizacionController.js",


                   "~/src/app/formulario/ventanas/tramites/trasladoFirma.js",
                   "~/src/app/formulario/ventanas/tramites/componentes/firma/firmaTramite.js",

                   "~/src/app/formulario/ventanas/ajustes/componentes/datosgenerales/indicadores/indicadoresController.js",
                  "~/src/app/formulario/ventanas/ajustes/componentes/datosgenerales/indicadores/indicadoresServicio.js",
                  "~/src/app/formulario/ventanas/gestionRecursos/componentes/recursos/resumenCostosSolicitado/resumenCostosSolicitadoController.js",
                  "~/src/app/formulario/ventanas/ajustes/componentes/datosgenerales/indicadores/modal/agregarIndicadorSecModalController.js",
                  "~/src/app/formulario/ventanas/ajustes/componentes/datosgenerales/indicadores/modal/agregarIndicadorSecModalServicio.js",
                  "~/src/app/formulario/ventanas/ajustes/componentes/datosgenerales/indicadores/modal/objetivosIndicadorModalController.js",
                  "~/src/app/formulario/ventanas/tramites/componentes/modalActualizaEstadoAjusteProyecto/modalActualizaEstadoAjusteProyectoController.js",
                "~/src/app/formulario/ventanas/tramites/componentes/modalActualizaEstadoAjusteProyecto/modalActualizaEstadoAjusteProyectoServicio.js",

                "~/src/app/formulario/ventanas/tramiteVigenciaFutura/tramiteVigenciaFuturaController.js",
                "~/src/app/formulario/ventanas/tramiteVigenciaFutura/tramiteVigenciaFuturaServicio.js",
                "~/src/app/formulario/ventanas/tramiteVigenciaFutura/componentes/justificacionvf/justificacionvfController.js",
                "~/src/app/formulario/ventanas/tramiteVigenciaFutura/componentes/informacionPresupuestal/informacionPresupuestalController.js",
                "~/src/app/formulario/ventanas/tramiteVigenciaFutura/componentes/informacionPresupuestal/resumenInformacionPresupuestal/resumenInformacionPresupuestalController.js",
                "~/src/app/formulario/ventanas/tramiteVigenciaFutura/componentes/informacionPresupuestal/resumenInformacionPresupuestalAprobados/resumenInformacionPresupuestalAprobadosController.js",
                 "~/src/app/formulario/ventanas/ajustes/componentes/datosgenerales/localizacionJustificacion/localizacionJustificacionController.js",
                 "~/src/app/formulario/ventanas/ajustes/componentes/datosgenerales/localizacionJustificacion/localizacionJustificacionServicio.js",

                 "~/src/app/formulario/ventanas/ajustes/componentes/recursos/Fuentes/ajustesFuentes/ajustesFuentes.js",
                 "~/src/app/formulario/ventanas/ajustes/componentes/recursos/Fuentes/ajustesFuentes/ajustesModalAgregarDatosAdicionalesController.js",
                 "~/src/app/formulario/ventanas/ajustes/componentes/recursos/Fuentes/ajustesFuentes/ajustesModalAgregarFuenteController.js",

                 "~/src/app/formulario/ventanas/gestionRecursos/componentes/focalizacion/focalizacionController.js",

                "~/src/app/formulario/ventanas/tramiteVigenciaFutura/componentes/documentoSoporte/documentoSoportevfController.js",
                "~/src/app/formulario/ventanas/tramiteVigenciaFutura/componentes/documentoSoporte/cargaArchivo/cargaArchivosvfController.js",

                "~/src/app/formulario/ventanas/tramiteVigenciaFutura/componentes/documentoSoporte/consultaArchivo/consultaArchivosvfController.js",

                "~/src/app/formulario/ventanas/tramiteVigenciaFutura/componentes/documentoSoporte/modal/modal1Controller.js",
                "~/src/app/formulario/ventanas/tramiteVigenciaFutura/componentes/documentoSoporte/modal/modalInstanceController.js",

                 "~/src/app/formulario/ventanas/tramiteVigenciaFutura/componentes/cdp/cdpVigenciasFuturasController.js",
                 "~/src/app/formulario/ventanas/tramiteVigenciaFutura/componentes/cdp/modalEditarCdpController.js",
                 "~/src/app/formulario/ventanas/tramiteVigenciaFutura/componentes/informacionPresupuestal/crp/crpVigenciasFuturasController.js",
                 "~/src/app/formulario/ventanas/tramiteVigenciaFutura/componentes/informacionPresupuestal/crp/modalEditarCrpController.js",

                "~/src/app/formulario/ventanas/tramiteVfaopc/tramiteVfaopcController.js",



                "~/src/app/formulario/ventanas/ajustes/componentes/datosgenerales/localizacionJustificacion/modal/modalAgregarLocalizacionController.js",

                "~/src/app/formulario/ventanas/tramiteLeyenda/aclaracionLeyendaServicio.js",
                "~/src/app/formulario/ventanas/tramiteLeyenda/aclaracionLeyendaController.js",
                "~/src/app/formulario/ventanas/tramiteLeyenda/componentes/asociarProyectoal/asociarProyectoalController.js",
                "~/src/app/formulario/ventanas/tramiteLeyenda/componentes/asociarProyectoal/seleccionProyecto/seleccionProyectosalController.js",
                "~/src/app/formulario/ventanas/tramiteLeyenda/componentes/asociarProyectoal/seleccionProyecto/buscarProyecto/buscarProyectoalController.js",
                "~/src/app/formulario/ventanas/tramiteLeyenda/componentes/conceptos/conceptosController.js",
                 "~/src/app/formulario/ventanas/tramiteLeyenda/componentes/documentos/documentoSoporteAlController.js",
                 "~/src/app/formulario/ventanas/tramiteLeyenda/componentes/informacionPresupuestal/informacionPresupuestalalController.js",
                 "~/src/app/formulario/ventanas/tramiteLeyenda/componentes/informacionPresupuestal/solicitudModificacion/solicitudModificacionalController.js",
                 "~/src/app/formulario/ventanas/tramiteLeyenda/componentes/conceptos/elaborarConcepto/elaborarConceptoal.js",
                 "~/src/app/formulario/ventanas/tramiteLeyenda/componentes/conceptos/elaborarConcepto/Carta/cuerpoAl.js",

                "~/src/app/formulario/ventanas/tramiteLeyenda/componentes/aprobacionEntidad/aprobacionEntidadServicio.js",

                "~/src/app/formulario/ventanas/tramiteLeyenda/componentes/aprobacionEntidad/aprobacionEntidadController.js",
                "~/src/app/formulario/ventanas/tramiteLeyenda/componentes/aprobacionEntidad/aprobacionEntidadDetalle/aprobacionEntdetController.js",
                "~/src/app/formulario/ventanas/tramiteLeyenda/componentes/justificacionAl/justificacionAlController.js",
                "~/src/app/formulario/ventanas/tramiteLeyenda/componentes/justificacionAl/justificacionAlDetalle/justificacionAlDetalleController.js",


                 "~/src/app/formulario/ventanas/tramiteLeyenda/componentes/aprobacion/aprobacionController.js",
                 "~/src/app/formulario/ventanas/tramiteLeyenda/componentes/aprobacion/aprobacionconfirmacion/aprobacionconfirmacionController.js",
                 "~/src/app/formulario/ventanas/tramiteLeyenda/componentes/firmaTramite/firmaController.js",
            #region tramite liberacion
                 "~/src/app/formulario/ventanas/tramiteLiberacion/tramiteLiberacionController.js",
                 "~/src/app/formulario/ventanas/tramiteLiberacion/tramiteLiberacionServicio.js",
                 "~/src/app/formulario/ventanas/tramiteLiberacion/componentes/asociarProyectoLiberacion/asociarProyectoLiberacionController.js",
                 "~/src/app/formulario/ventanas/tramiteLiberacion/componentes/seleccionarVigencias/seleccionarVigenciasController.js",
                 "~/src/app/formulario/ventanas/tramiteLiberacion/componentes/autorizacionliberacion/autorizacionliberacionController.js",
                 "~/src/app/formulario/ventanas/tramiteLiberacion/componentes/autorizacionliberacion/autorizacionliberacionServicio.js",
                 "~/src/app/formulario/ventanas/tramiteLiberacion/componentes/utilizadoliberacion/utilizadoliberacionController.js",
                 "~/src/app/formulario/ventanas/tramiteLiberacion/componentes/utilizadoliberacion/utilizadoliberacionServicio.js",
                 "~/src/app/formulario/ventanas/tramiteLiberacion/componentes/verficadoliberacion/verificadoliberacionController.js",
                 "~/src/app/formulario/ventanas/tramiteLiberacion/componentes/verficadoliberacion/verificadoliberacionServicio.js",
                 "~/src/app/formulario/ventanas/tramiteLiberacion/componentes/resumenliberacion/resumenliberacionController.js",
                 "~/src/app/formulario/ventanas/tramiteLiberacion/componentes/resumenliberacion/resumenliberacionServicio.js",
                 "~/src/app/formulario/ventanas/tramiteLiberacion/componentes/productoliberacion/productoliberacionController.js",
                 "~/src/app/formulario/ventanas/tramiteLiberacion/componentes/productoliberacion/productoliberacionServicio.js",
                 "~/src/app/formulario/ventanas/tramiteLiberacion/componentes/aprobacionliberacionvf/aprobacionliberacionvfController.js",
                 "~/src/app/formulario/ventanas/tramiteLiberacion/componentes/pasotresliberacionvf/pasotresliberacionvfController.js",
            #endregion

            #region tramite traslado ordinario
                 "~/src/app/formulario/ventanas/tramiteTrasladoOrdinario/tramiteTrasladoOrdinarioController.js",
                 "~/src/app/formulario/ventanas/tramiteTrasladoOrdinario/componentes/asociarProyectoTraslado/asociarProyectoTrasladoController.js",
                 "~/src/app/formulario/ventanas/tramiteTrasladoOrdinario/componentes/informacionPresupuestalTraslado/informacionPresupuestalTraslado.js",
                 "~/src/app/formulario/ventanas/tramiteTrasladoOrdinario/componentes/conceptosTraslado/conceptosTrasladoController.js",
                 "~/src/app/formulario/ventanas/tramiteTrasladoOrdinario/componentes/firmaTraslado/firmaTrasladoController.js",
                 "~/src/app/formulario/ventanas/tramiteTrasladoOrdinario/componentes/documentos/documentoSoporteTramiteTrasladoOrdinarioController.js",
                 "~/src/app/formulario/ventanas/tramiteTrasladoOrdinario/componentes/justificacion/justificacionTrasladoController.js",
                 "~/src/app/formulario/ventanas/tramiteTrasladoOrdinario/componentes/aprobacionEntidad/aproentOrdinarioController.js",
                 "~/src/app/formulario/ventanas/tramiteTrasladoOrdinario/componentes/aprobacionSupervisor/aprosuperOrdinarioController.js",
                 "~/src/app/formulario/ventanas/tramiteTrasladoOrdinario/componentes/conceptosTraslado/elaborarConcepto/elaborarConceptoTrasladoController.js",
                 "~/src/app/formulario/ventanas/tramiteTrasladoOrdinario/tramiteTrasladoOrdinarioServicio.js",
            #endregion


            #region tramite distribucion
                 "~/src/app/formulario/ventanas/tramiteDistribucion/tramiteDistribucionServicio.js",
                 "~/src/app/formulario/ventanas/tramiteDistribucion/tramiteDistribucionController.js",
                 "~/src/app/formulario/ventanas/tramiteDistribucion/componentes/informacionPresupuestalDistribucion/informacionPresupuestalDistribucion.js",
                 "~/src/app/formulario/ventanas/tramiteDistribucion/componentes/conceptosDistribucion/conceptosDistribucionController.js",
                 "~/src/app/formulario/ventanas/tramiteDistribucion/componentes/asociarProyectoDistribucion/asociarProyectoDistribucionController.js",
                 "~/src/app/formulario/ventanas/tramiteDistribucion/componentes/firmaDistribucion/firmaDistribucionController.js",
                 "~/src/app/formulario/ventanas/tramiteDistribucion/componentes/documentos/documentoSoporteTramiteDistribucionController.js",
                 "~/src/app/formulario/ventanas/tramiteDistribucion/componentes/justificacion/justificacionDistribucionController.js",
                 "~/src/app/formulario/ventanas/tramiteDistribucion/componentes/aprobacionEntidad/aproentDistribucionController.js",
                 "~/src/app/formulario/ventanas/tramiteDistribucion/componentes/aprobacionSupervisor/aproSuperDistribucionController.js",
                 "~/src/app/formulario/ventanas/tramiteDistribucion/componentes/conceptosDistribucion/elaborarConcepto/elaborarConceptoDistribucionController.js",
            #endregion

            #region tramite ley

                "~/src/app/formulario/ventanas/tramiteTrasladoLey/tramiteTrasladoLeyServicio.js",
                "~/src/app/formulario/ventanas/tramiteTrasladoLey/tramiteTrasladoLeyController.js",
                "~/src/app/formulario/ventanas/tramiteTrasladoLey/componentes/informacionPresupuestalLey/informacionPresupuestalLey.js",
                "~/src/app/formulario/ventanas/tramiteTrasladoLey/componentes/conceptosLey/conceptosLeyController.js",
                "~/src/app/formulario/ventanas/tramiteTrasladoLey/componentes/asociarProyectoLey/asociarProyectoLeyController.js",
                "~/src/app/formulario/ventanas/tramiteTrasladoLey/componentes/firmaLey/firmaLeyController.js",
                "~/src/app/formulario/ventanas/tramiteTrasladoLey/componentes/justificacion/justificacionLeyController.js",
                "~/src/app/formulario/ventanas/tramiteTrasladoLey/componentes/aprobacionEntidad/aproentLeyController.js",
                "~/src/app/formulario/ventanas/tramiteTrasladoLey/componentes/documentos/documentoSoporteTramiteTrasladoLeyController.js",
                "~/src/app/formulario/ventanas/tramiteTrasladoLey/componentes/aprobacionSupervisor/aproSuperLeyController.js",
                "~/src/app/formulario/ventanas/tramiteTrasladoLey/componentes/conceptosLey/elaborarConcepto/elaborarConceptoLeyController.js",

            #endregion

            #region tramite Adicion Donacion
                "~/src/app/formulario/ventanas/tramiteAdicionDonacion/tramiteAdicionDonacionServicio.js",
                "~/src/app/formulario/ventanas/tramiteAdicionDonacion/tramiteAdicionDonacionController.js",
                "~/src/app/formulario/ventanas/tramiteAdicionDonacion/tramiteAdicionDonacionPasoDosController.js",
                "~/src/app/formulario/ventanas/tramiteAdicionDonacion/tramiteAdicionDonacionPasoTresController.js",
                "~/src/app/formulario/ventanas/tramiteAdicionDonacion/tramiteAdicionDonacionPasoCuatroController.js",
                "~/src/app/formulario/ventanas/tramiteAdicionDonacion/tramiteAdicionDonacionPasoCincoController.js",
                "~/src/app/formulario/ventanas/tramiteAdicionDonacion/Componentes/proyectosTad/proyectosTadController.js",
                "~/src/app/formulario/ventanas/tramiteAdicionDonacion/Componentes/proyectosTad/proyectosTadPasoTresController.js",
                "~/src/app/formulario/ventanas/tramiteAdicionDonacion/Componentes/informacionPresupuestalTad/informacionPresupuestalTadController.js",
                "~/src/app/formulario/ventanas/tramiteAdicionDonacion/Componentes/justificacionTad/justificacionTadController.js",
                "~/src/app/formulario/ventanas/tramiteAdicionDonacion/Componentes/justificacionTad/justificacionTadPasoTresController.js",
                "~/src/app/formulario/ventanas/tramiteAdicionDonacion/Componentes/soporteTad/soporteTadController.js",
                "~/src/app/formulario/ventanas/tramiteAdicionDonacion/Componentes/aprobacionEntidadTad/aproentTadController.js",
                "~/src/app/formulario/ventanas/tramiteAdicionDonacion/Componentes/conceptoTad/conceptoTadController.js",
                "~/src/app/formulario/ventanas/tramiteAdicionDonacion/Componentes/conceptoTad/elaborarConcepto/elaborarConceptoTadController.js",
                "~/src/app/formulario/ventanas/tramiteAdicionDonacion/Componentes/conceptoTad/elaborarConcepto/Carta/datosInicialesTadController.js",
                "~/src/app/formulario/ventanas/tramiteAdicionDonacion/Componentes/conceptoTad/elaborarConcepto/Carta/cuerpoTadController.js",
                "~/src/app/formulario/ventanas/tramiteAdicionDonacion/Componentes/aprobacionSupervisorTad/aproSuperTadController.js",
                "~/src/app/formulario/ventanas/tramiteAdicionDonacion/Componentes/firmaTad/firmaTadController.js",
                "~/src/app/formulario/ventanas/tramiteAdicionDonacion/Componentes/informacionPresupuestalTad/informacionPresupuestalTadPasoTresController.js",


                "~/src/app/formulario/ventanas/comun/informacionPresupuestalSolicitudTad/solicitudAdicionDonacionFormularioController.js",
                "~/src/app/formulario/ventanas/comun/informacionPresupuestalAprobacionTad/aprobacionAdicionDonacionFormularioController.js",

                 "~/src/app/formulario/ventanas/tramiteAdicionDonacion/Componentes/proyectosTad/datosAdicionDonacion/datosAdicionDonacionServicio.js",
                 "~/src/app/formulario/ventanas/tramiteAdicionDonacion/Componentes/proyectosTad/datosAdicionDonacion/datosAdicionDonacionController.js",
                 "~/src/app/formulario/ventanas/tramiteAdicionDonacion/Componentes/proyectosTad/datosAdicionDonacion/modalDatosAdicionDonacionController.js",


            #endregion

            #region tramite incorporacion

                "~/src/app/formulario/ventanas/tramiteIncorporacion/tramiteIncorporacionController.js",
                "~/src/app/formulario/ventanas/tramiteIncorporacion/tramiteIncorporacionServicio.js",
                "~/src/app/formulario/ventanas/tramiteIncorporacion/componentes/proyectosInc/proyectosIncController.js",
                "~/src/app/formulario/ventanas/tramiteIncorporacion/componentes/informacionPresupuestalInc/informacionPresupuestalIncController.js",
                "~/src/app/formulario/ventanas/tramiteIncorporacion/componentes/justificacionInc/justificacionIncController.js",
                "~/src/app/formulario/ventanas/tramiteIncorporacion/componentes/soporteInc/soporteIncController.js",
                "~/src/app/formulario/ventanas/tramiteIncorporacion/componentes/aprobacionEntidadInc/aproentIncController.js",
                "~/src/app/formulario/ventanas/tramiteIncorporacion/componentes/conceptoInc/conceptoIncController.js",
                "~/src/app/formulario/ventanas/tramiteIncorporacion/componentes/aprobacionSupervisorInc/aproSuperIncController.js",
                "~/src/app/formulario/ventanas/tramiteIncorporacion/componentes/firmaInc/firmaIncController.js",
                 "~/src/app/formulario/ventanas/tramiteIncorporacion/componentes/informacionPresupuestalInc/solicitudincorporacion/solicitudincorporacionController.js",
                 "~/src/app/formulario/ventanas/tramiteIncorporacion/componentes/informacionPresupuestalInc/solicitudincorporacion/solicitudincorporacionServicio.js",
                 "~/src/app/formulario/ventanas/tramiteIncorporacion/componentes/informacionPresupuestalInc/aprobacionincorporacion/aprobacionincorporacionController.js",
                 "~/src/app/formulario/ventanas/tramiteIncorporacion/componentes/informacionPresupuestalInc/aprobacionincorporacion/aprobacionincorporacionServicio.js",

                 "~/src/app/formulario/ventanas/tramiteIncorporacion/componentes/proyectosInc/datosIncorporacion/datosIncorporacionServicio.js",
                 "~/src/app/formulario/ventanas/tramiteIncorporacion/componentes/proyectosInc/datosIncorporacion/datosIncorporacionController.js",
                 "~/src/app/formulario/ventanas/tramiteIncorporacion/componentes/proyectosInc/datosIncorporacion/modalDatosIncorporacionController.js",
                 "~/src/app/formulario/ventanas/tramiteIncorporacion/componentes/conceptoInc/elaborarConcepto/elaborarConceptoIncController.js",

            #endregion

            #region tramite Reprogramacion de vigencias futuras
                "~/src/app/formulario/ventanas/tramiteReprogramacionVF/tramiteReprogramacionVfServicio.js",
                "~/src/app/formulario/ventanas/tramiteReprogramacionVF/tramiteReprogramacionVfPasoUnoController.js",
                "~/src/app/formulario/ventanas/tramiteReprogramacionVF/tramiteReprogramacionVfPasoDosController.js",
                "~/src/app/formulario/ventanas/tramiteReprogramacionVF/tramiteReprogramacionVfPasoTresController.js",
                "~/src/app/formulario/ventanas/tramiteReprogramacionVF/tramiteReprogramacionVfPasoCuatroController.js",
                "~/src/app/formulario/ventanas/tramiteReprogramacionVF/tramiteReprogramacionVfPasoCincoController.js",
                "~/src/app/formulario/ventanas/tramiteReprogramacionVF/Componentes/proyectosRvf/proyectosRvfController.js",
                "~/src/app/formulario/ventanas/tramiteReprogramacionVF/Componentes/proyectosRvf/proyectosRvfPasoTresController.js",
                "~/src/app/formulario/ventanas/tramiteReprogramacionVF/Componentes/informacionPresupuestalRvf/informacionPresupuestalRvfController.js",
                "~/src/app/formulario/ventanas/tramiteReprogramacionVF/Componentes/informacionPresupuestalRvf/informacionPresupuestalRvfPasoTresController.js",
                "~/src/app/formulario/ventanas/tramiteReprogramacionVF/Componentes/justificacionRvf/justificacionRvfController.js",
                "~/src/app/formulario/ventanas/tramiteReprogramacionVF/Componentes/justificacionRvf/justificacionRvfPasoTresController.js",
                "~/src/app/formulario/ventanas/tramiteReprogramacionVF/Componentes/soporteRvf/soporteRvfController.js",
                "~/src/app/formulario/ventanas/tramiteReprogramacionVF/Componentes/aprobacionEntidadRvf/aprobacionEntidadRvfController.js",
                "~/src/app/formulario/ventanas/tramiteReprogramacionVF/Componentes/conceptoRvf/conceptoRvfController.js",
                "~/src/app/formulario/ventanas/tramiteReprogramacionVF/Componentes/conceptoRvf/elaborarConcepto/elaborarConceptoRvfController.js",
                "~/src/app/formulario/ventanas/tramiteReprogramacionVF/Componentes/conceptoRvf/elaborarConcepto/Carta/cuerpoRvfController.js",
                "~/src/app/formulario/ventanas/tramiteReprogramacionVF/Componentes/aprobacionSupervisorRvf/aprobacionSupervisorRvfController.js",
                "~/src/app/formulario/ventanas/tramiteReprogramacionVF/Componentes/firmaRvf/firmaRvfController.js",
                "~/src/app/formulario/ventanas/tramiteReprogramacionVF/Componentes/conpesRvf/conpesRvfController.js",
                "~/src/app/formulario/ventanas/tramiteReprogramacionVF/componentes/seleccionProyectoRvf/seleccionProyectosRvfController.js",
                "~/src/app/formulario/ventanas/tramiteReprogramacionVF/componentes/seleccionProyectoRvf/datosProyectoRvf/datosProyectoRvfController.js",
                "~/src/app/formulario/ventanas/tramiteReprogramacionVF/componentes/seleccionProyectoRvf/buscarProyectoRvf/buscarProyectoRvfController.js",
                "~/src/app/formulario/ventanas/tramiteReprogramacionVF/Componentes/resumenRvf/resumenRvfController.js",
                "~/src/app/formulario/ventanas/tramiteReprogramacionVF/Componentes/firmaRvf/firmaRvfController.js",
                "~/src/app/formulario/ventanas/tramiteReprogramacionVF/componentes/seleccionProyectoRvf/buscarAutorizacionRvf/buscarAutorizacionRvfController.js",
                "~/src/app/formulario/ventanas/tramiteReprogramacionVF/componentes/seleccionProyectoRvf/datosAutorizacionRvf/datosAutorizacionRvfController.js",



            #endregion


            #region proceso Programacion de vigencias futuras


                "~/src/app/formulario/ventanas/comun/programacionRecursos/listaProyectosRegistradosController.js",
                "~/src/app/formulario/ventanas/comun/programacionRecursos/listaProyectosRegistradosPasoTresController.js",
                "~/src/app/formulario/ventanas/comun/programacionRecursos/tablaResumenDistribucionController.js",
                "~/src/app/formulario/ventanas/comun/programacionRecursos/tablaResumenDistribucionPasoTresController.js",
                "~/src/app/formulario/ventanas/comun/programacionRecursos/fuentes/programacionFuentesFormulario.js",
                "~/src/app/formulario/ventanas/comun/programacionRecursos/fuentes/programacionFuentesFormularioPasoTres.js",
                "~/src/app/formulario/ventanas/comun/programacionRecursos/iniciativas/programacionIniciativasFormulario.js",
                "~/src/app/formulario/ventanas/comun/programacionRecursos/productos/programacionProductosFormulario.js",
                "~/src/app/formulario/ventanas/comun/programacionRecursos/productos/programacionProductosFormularioPasoTres.js",
                "~/src/app/formulario/ventanas/comun/programacionRecursos/politicas/detallePolitica/politica/politicasTransversalesFormulario.js",
                "~/src/app/formulario/ventanas/comun/programacionRecursos/politicas/detallePolitica/politica/modal/modalAgregarPoliticaTransversalController.js",
                "~/src/app/formulario/ventanas/comun/programacionRecursos/politicas/detallePolitica/detallePoliticaControlador.js",
                "~/src/app/formulario/ventanas/comun/programacionRecursos/politicas/detallePolitica/transversales/ResumenFocalizacionFormularioController.js",
                "~/src/app/formulario/ventanas/comun/programacionRecursos/politicas/detallePolitica/cruce/crucePoliticasTransversalesProgramacion.js",
                "~/src/app/formulario/ventanas/comun/programacionRecursos/regionalizacion/modal/modalAgregarLocalizacionRegionalizacionController.js",
                "~/src/app/formulario/ventanas/comun/programacionRecursos/regionalizacion/programacionRegionalizacionFormulario.js",
                "~/src/app/formulario/ventanas/comun/programacionRecursos/politicas/detallePolitica/detallePoliticaPasoTresFormularioController.js",
                "~/src/app/formulario/ventanas/comun/programacionRecursos/politicas/detallePolitica/cruce/crucePoliticasPrPasoTresFormularioController.js",
                "~/src/app/formulario/ventanas/comun/programacionRecursos/politicas/detallePolitica/transversales/ResumenFocalizacionPasoTresFormularioController.js",
                "~/src/app/formulario/ventanas/comun/programacionRecursos/politicas/detallePolitica/politica/politicasTransversalesPasoTresFormularioController.js",

            #endregion tramite Reprogramacion de vigencias futuras

            #region Seguimiento y control

                   "~/src/app/formulario/ventanas/seguimientoControl/planearEjecucion/seguimientoPlanearController.js",
                   "~/src/app/formulario/ventanas/seguimientoControl/reporteAvance/seguimientoReporteController.js",
                    "~/src/app/formulario/ventanas/seguimientoControl/reporteAvance/seguimientoReportePasoDosController.js",
                    "~/src/app/formulario/ventanas/seguimientoControl/seguimientoServicio.js",

                    "~/src/app/formulario/ventanas/seguimientoControl/componentes/desagregarEdt/desagregarEdtController.js",
                    "~/src/app/formulario/ventanas/seguimientoControl/componentes/desagregarEdt/desagregarEdtServicio.js",

                    "~/src/app/formulario/ventanas/seguimientoControl/componentes/programarActiv/programarActivController.js",
                    "~/src/app/formulario/ventanas/seguimientoControl/componentes/programarActiv/programarActivServicio.js",

                    "~/src/app/formulario/ventanas/seguimientoControl/componentes/programarProd/programarProdController.js",
                    "~/src/app/formulario/ventanas/seguimientoControl/componentes/programarProd/programarProdServicio.js",

                    "~/src/app/formulario/ventanas/seguimientoControl/componentes/desagregarEdt/componentes/desagregarEdtCap/desagregarEdtCapController.js",
                    "~/src/app/formulario/ventanas/seguimientoControl/componentes/desagregarEdt/componentes/desagregarEdtCap/desagregarEdtCapServicio.js",

                    "~/src/app/formulario/ventanas/seguimientoControl/componentes/programarActiv/componentes/programarActCap/programarActCapController.js",
                    "~/src/app/formulario/ventanas/seguimientoControl/componentes/programarActiv/componentes/programarActCap/programarActCapServicio.js",

                    "~/src/app/formulario/ventanas/seguimientoControl/componentes/programarActiv/componentes/listadoProgramarActCap/listadoProgramarActCapController.js",
                    "~/src/app/formulario/ventanas/seguimientoControl/componentes/programarActiv/componentes/listadoProgramarActCap/listadoProgramarActCapServicio.js",
                    "~/src/app/formulario/ventanas/seguimientoControl/componentes/programarActiv/componentes/editarActividadModal/editarActividadModalController.js",
                    "~/src/app/formulario/ventanas/seguimientoControl/componentes/programarActiv/componentes/editarActividadModal/editarActividadModalServicio.js",

                    "~/src/app/formulario/ventanas/seguimientoControl/componentes/programarProd/componentes/programarProdCap/programarProdCapController.js",
                    "~/src/app/formulario/ventanas/seguimientoControl/componentes/programarProd/componentes/programarProdCap/programarProdCapServicio.js",

                    "~/src/app/formulario/ventanas/seguimientoControl/componentes/desagregarEdt/componentes/agregarNivelModal/agregarNivelModalController.js",
                    "~/src/app/formulario/ventanas/seguimientoControl/componentes/desagregarEdt/componentes/agregarNivelModal/agregarNivelModalServicio.js",
                    "~/src/app/formulario/ventanas/seguimientoControl/componentes/desagregarEdt/componentes/agregarActividadModal/agregarActividadModalController.js",
                    "~/src/app/formulario/ventanas/seguimientoControl/componentes/desagregarEdt/componentes/agregarActividadModal/agregarActividadModalServicio.js",

                    "~/src/app/formulario/ventanas/seguimientoControl/componentes/reporteFinanciero/reporteFinancieroController.js",
                    "~/src/app/formulario/ventanas/seguimientoControl/componentes/reporteFinanciero/reporteFinancieroServicio.js",
                    "~/src/app/formulario/ventanas/seguimientoControl/componentes/reporteFinanciero/componentes/avanceFinanciero/avanceFinancieroController.js",

                    "~/src/app/formulario/ventanas/seguimientoControl/componentes/documentoSoporteSC/documentoSoporteScController.js",

                      "~/src/app/formulario/ventanas/seguimientoControl/componentes/metaProducto/metaProductoController.js",
                      "~/src/app/formulario/ventanas/seguimientoControl/componentes/metaProducto/metaProductoServicio.js",

                      "~/src/app/formulario/ventanas/seguimientoControl/componentes/metaProducto/componentes/avanceMeta/avanceMetaCapController.js",
                      "~/src/app/formulario/ventanas/seguimientoControl/componentes/metaProducto/componentes/avanceMeta/avanceMetaCapServicio.js",

                       "~/src/app/formulario/ventanas/seguimientoControl/componentes/aprobacionEntidadReporte/aproentRepController.js",

            #endregion

            #region Reporte actividades
                    "~/src/app/formulario/ventanas/seguimientoControl/componentes/reporteFinanciero/componentes/avancePresupuestal/reporteActCap/reporteActCapController.js",
                    "~/src/app/formulario/ventanas/seguimientoControl/componentes/reporteFinanciero/componentes/avancePresupuestal/reporteActCap/reporteActCapServicio.js",
                    "~/src/app/formulario/ventanas/seguimientoControl/componentes/reporteFinanciero/componentes/avancePresupuestal/listadoReporteActCap/listadoReporteActCapController.js",
                    "~/src/app/formulario/ventanas/seguimientoControl/componentes/reporteFinanciero/componentes/avancePresupuestal/listadoReporteActCap/listadoReporteActCapServicio.js",
                    "~/src/app/formulario/ventanas/seguimientoControl/componentes/reporteFinanciero/componentes/avancePresupuestal/editarActividadModal/editarActividadPresupuestalModalController.js",
                    "~/src/app/formulario/ventanas/seguimientoControl/componentes/reporteFinanciero/componentes/avancePresupuestal/editarActividadModal/editarActividadPresupuestalModalServicio.js",

            #endregion

            #region tramite Programacion de Recursos PGN

                "~/src/app/formulario/ventanas/tramiteProgramacionRecursos/tramiteProgramacionRecursosServicio.js",
                "~/src/app/formulario/ventanas/tramiteProgramacionRecursos/tramiteProgramacionRecursosPasoUnoController.js",
                "~/src/app/formulario/ventanas/tramiteProgramacionRecursos/tramiteProgramacionRecursosPasoDosController.js",
                "~/src/app/formulario/ventanas/tramiteProgramacionRecursos/tramiteProgramacionRecursosPasoTresController.js",
                "~/src/app/formulario/ventanas/tramiteProgramacionRecursos/tramiteProgramacionRecursosPasoCuatroController.js",
                "~/src/app/formulario/ventanas/tramiteProgramacionRecursos/Componentes/distribucionPr/distribucionPrController.js",
                "~/src/app/formulario/ventanas/tramiteProgramacionRecursos/Componentes/inclusionProyectosPr/inclusionProyectosPrController.js",
                "~/src/app/formulario/ventanas/tramiteProgramacionRecursos/Componentes/inclusionProyectosPr/modal/modalAgregarProyectoController.js",
                "~/src/app/formulario/ventanas/tramiteProgramacionRecursos/Componentes/fuentesPr/fuentesPrController.js",
                "~/src/app/formulario/ventanas/tramiteProgramacionRecursos/Componentes/fuentesPr/fuentesPrPasoTresController.js",
                "~/src/app/formulario/ventanas/tramiteProgramacionRecursos/Componentes/iniciativasPr/iniciativasPrController.js",
                "~/src/app/formulario/ventanas/tramiteProgramacionRecursos/Componentes/politicasTransversalesPr/politicasTransversalesPrController.js",
                "~/src/app/formulario/ventanas/tramiteProgramacionRecursos/Componentes/politicasTransversalesPr/politicasTransversalesPrPasoTresController.js",
                "~/src/app/formulario/ventanas/tramiteProgramacionRecursos/Componentes/productosPr/productosPrController.js",
                "~/src/app/formulario/ventanas/tramiteProgramacionRecursos/Componentes/productosPr/productosPrPasoTresController.js",
                "~/src/app/formulario/ventanas/tramiteProgramacionRecursos/Componentes/regionalizacionPr/regionalizacionPrController.js",
                "~/src/app/formulario/ventanas/tramiteProgramacionRecursos/Componentes/regionalizacionPr/regionalizacionPrPasoTresController.js",
                "~/src/app/formulario/ventanas/tramiteProgramacionRecursos/Componentes/aprobacionEntidadPr/aprobacionEntidadPrController.js",
                "~/src/app/formulario/ventanas/tramiteProgramacionRecursos/Componentes/aprobacionSupervisorPr/aprobacionSupervisorPrController.js",
                 "~/src/app/formulario/ventanas/comun/programacionRecursos/regionalizacion/programacionRegionalizacionFormularioPasoTres.js",
            #endregion

                "~/src/app/formulario/ventanas/tramiteVigenciaFutura/componentes/conpesVigenciaFutura/conpesVigenciaFutura.js",
                "~/src/app/formulario/ventanas/tramiteVigenciaFutura/componentes/asociarProyectovf/seleccionProyecto/seleccionProyectosvfController.js",
               "~/src/app/formulario/ventanas/tramiteVigenciaFutura/componentes/justificacionvf/justificacionvfDetalle/justificacionvfDetalleController.js",
                "~/src/app/formulario/ventanas/transversal/justificacionCambios/componentes/recursos/fuentes/Componentes/resumenRecursosfuentesdefinancController.js",
                "~/src/app/formulario/ventanas/transversal/justificacionCambios/componentes/recursos/costoactividades/justificacioncostoactividadesController.js",
                "~/src/app/formulario/ventanas/transversal/justificacionCambios/componentes/recursos/costoactividades/justificacioncostoactividadesServicio.js",
                "~/src/app/formulario/ventanas/tramiteVigenciaFutura/componentes/asociarProyectovf/asociarProyectovfController.js",
                "~/src/app/formulario/ventanas/tramiteVigenciaFutura/componentes/asociarProyectovf/seleccionProyecto/buscarProyecto/buscarProyectovfController.js",
                "~/src/app/formulario/ventanas/tramiteVigenciaFutura/componentes/asociarProyectovf/seleccionProyecto/datosProyectovf/datosProyectovfController.js",
                "~/src/app/formulario/ventanas/tramiteVigenciaFutura/aprobacion/aprobacionTramiteVigenciaFuturaController.js",
                "~/src/app/formulario/ventanas/tramiteVigenciaFutura/aprobacion/componentes/concepto/conceptovfController.js",
                "~/src/app/formulario/ventanas/tramiteVigenciaFutura/aprobacion/componentes/concepto/solicitarConcepto/solicitarConceptovfController.js",
                "~/src/app/formulario/ventanas/tramiteVigenciaFutura/componentes/informacionPresupuestal/cronogramavf/cronogramavfController.js",
                "~/src/app/formulario/ventanas/tramiteVigenciaFutura/aprobacion/componentes/concepto/observaciones/observacionesvfController.js",
                "~/src/app/formulario/ventanas/tramiteVigenciaFutura/aprobacion/componentes/concepto/elaborarConcepto/elaborarConceptovfController.js",

                "~/src/app/formulario/ventanas/tramiteVigenciaFutura/aprobacion/componentes/concepto/elaborarConcepto/carta/datosInicialesvf.js",
                "~/src/app/formulario/ventanas/tramiteVigenciaFutura/aprobacion/componentes/concepto/elaborarConcepto/carta/cuerpoConceptovf.js",
                "~/src/app/formulario/ventanas/tramiteVigenciaFutura/aprobacion/componentes/concepto/elaborarConcepto/carta/datosDespedidavf.js",


                 "~/src/app/formulario/ventanas/gestionRecursos/componentes/indicadoresPolitica/constantesIndicadorPolitica.js",
                "~/src/app/formulario/ventanas/gestionRecursos/componentes/indicadoresPolitica/indicadoresPoliticaServicio.js",
                "~/src/app/formulario/ventanas/gestionRecursos/componentes/indicadoresPolitica/indicadoresPoliticaController.js",
                "~/src/app/formulario/ventanas/gestionRecursos/componentes/politicacrucepolitica/politicaCrucePoliticaController.js",
                "~/src/app/formulario/ventanas/tramiteVigenciaFutura/componentes/justificacionvfConsulta/justificacionvfConsultaController.js",
                "~/src/app/formulario/ventanas/tramiteVigenciaFutura/componentes/justificacionvfConsulta/justificacionvfConsultadet/justificacionvfConsultadetController.js",
                "~/src/app/formulario/ventanas/comun/programacionRecursos/politicas/concepto/conceptoVictimasFormulario.js",
                "~/src/app/formulario/ventanas/comun/programacionRecursos/politicas/aprobarConcepto/aprobarConceptoVictimasFormulario.js",
                "~/src/app/formulario/ventanas/gestionRecursos/componentes/categoriaProductosPolitica/constantesCategoriaProductosPolitica.js",
                "~/src/app/formulario/ventanas/gestionRecursos/componentes/categoriaProductosPolitica/categoriaProductosPoliticaServicio.js",
                "~/src/app/formulario/ventanas/gestionRecursos/componentes/categoriaProductosPolitica/categoriaProductosPoliticaController.js",
                "~/src/app/formulario/ventanas/comun/programacionRecursos/politicas/aprobarConcepto/aprobarConceptoVictimasPasoTresFormulario.js",
                "~/src/app/formulario/ventanas/comun/programacionRecursos/politicas/concepto/conceptoVictimasPasoTresFormulario.js",


                "~/src/app/formulario/ventanas/ajustesConTramite/ajustesConTramiteController.js",
                "~/src/app/formulario/ventanas/ajustesConTramite/ajustesConTramiteServicio.js",
                "~/src/app/formulario/ventanas/ajustesConTramite/componentes/vigenciasFuturas/ajustesvigenciasfuturasController.js",
                "~/src/app/formulario/ventanas/ajustesConTramite/componentes/vigenciasFuturas/ajustesvigenciasfuturasServicio.js",
                "~/src/app/formulario/ventanas/ajustesConTramite/componentes/vigenciasFuturas/solicitudVigenciaFutura/ajustesolicitudvigfuturaController.js",
                "~/src/app/formulario/ventanas/ajustesConTramite/componentes/vigenciasFuturas/solicitudVigenciaFutura/ajustesolicitudvigfuturaServicio.js",
                "~/src/app/formulario/ventanas/ajustesConTramite/componentes/vigenciasFuturas/valoresCorrientes/vigfuturavalcorrientesController.js",
                "~/src/app/formulario/ventanas/ajustesConTramite/componentes/vigenciasFuturas/valoresCorrientes/vigfuturavalcorrientesServicio.js",
                "~/src/app/formulario/ventanas/ajustesConTramite/componentes/vigenciasFuturas/valoresConstantes/vigfuturavalconstantesController.js",
                "~/src/app/formulario/ventanas/ajustesConTramite/componentes/vigenciasFuturas/valoresConstantes/vigfuturavalconstantesServicio.js",
                "~/src/app/formulario/ventanas/ajustesConTramite/componentes/vigenciasFuturas/cronograma/cronogramavigfuturaController.js",
                "~/src/app/formulario/ventanas/ajustesConTramite/componentes/vigenciasFuturas/cronograma/cronogramavigfuturaServicio.js",
                "~/src/app/formulario/ventanas/ajustesConTramite/componentes/vigenciasFuturas/vigenciaFuturaProductos/vigfuturaproductosController.js",
                "~/src/app/formulario/ventanas/ajustesConTramite/componentes/vigenciasFuturas/vigenciaFuturaProductos/vigfuturaproductosServicio.js",
                "~/src/app/formulario/ventanas/ajustesConTramite/componentes/reprogramacion/reprogramacionvigfuturaController.js",
                "~/src/app/formulario/ventanas/ajustesConTramite/componentes/reprogramacion/reprogramacionproductovigfuturaController.js",

            #region  Priorizacion
                    "~/src/app/formulario/ventanas/SGR/Priorizacion/priorizacionSgrServicio.js",
                    "~/src/app/formulario/ventanas/SGR/Priorizacion/M1/priorizacionM1SgrController.js",
                    "~/src/app/formulario/ventanas/SGR/Priorizacion/M1/priorizacion/priorizacionM1PriorizaSgrController.js",
                    "~/src/app/formulario/ventanas/SGR/Priorizacion/M1/priorizacion/estado/estadoPriorizacionSgrController.js",
                    "~/src/app/formulario/ventanas/SGR/Priorizacion/M1/priorizacion/registro/registroPriorizacionSgrController.js",
                    "~/src/app/formulario/ventanas/SGR/Priorizacion/M2/priorizacionM2SgrController.js",
                    "~/src/app/formulario/ventanas/SGR/Priorizacion/M2/priorizacion/priorizacionM2PriorizaSgrController.js",
                    "~/src/app/formulario/ventanas/SGR/Priorizacion/M2/priorizacion/registro/registroPriorizacionSgrM2Controller.js",
                    "~/src/app/formulario/ventanas/SGR/Priorizacion/M2/priorizacion/estado/estadoPriorizacionM2SgrController.js",
                    "~/src/app/formulario/ventanas/SGR/Priorizacion/M3/priorizacionM3SgrController.js",
                    "~/src/app/formulario/ventanas/SGR/Priorizacion/M3/priorizacion/priorizacionM3PriorizaSgrController.js",
                    "~/src/app/formulario/ventanas/SGR/Priorizacion/M3/priorizacion/registro/registroPriorizacionSgrM3Controller.js",
                    "~/src/app/formulario/ventanas/SGR/Priorizacion/M3/priorizacion/estado/estadoPriorizacionM3SgrController.js",

            #endregion

            #region Procesos SGR
                    "~/src/app/formulario/ventanas/SGR/Procesos/procesosSgrServicio.js",
            #endregion

            #region  ViabilidadSGR
                     
                  "~/src/app/formulario/ventanas/SGR/comun/transversalSgrServicio.js",                 
                  "~/src/app/formulario/ventanas/SGR/viabilidadSGR/viabilidadSgrServicio.js",
                  "~/src/app/formulario/ventanas/SGR/viabilidadSGR/viabilidadSgrController.js",
                  "~/src/app/formulario/ventanas/SGR/viabilidadSGR/previosSgrServicio.js",
                  "~/src/app/formulario/ventanas/SGR/viabilidadSGR/previosSgrController.js",
                  "~/src/app/formulario/ventanas/SGR/viabilidadSGR/requisitosSgrServicio.js",
                  "~/src/app/formulario/ventanas/SGR/viabilidadSGR/requisitosSgrController.js",
                  "~/src/app/formulario/ventanas/SGR/viabilidadSGR/firmaEmisionSgrController.js",
                  "~/src/app/formulario/ventanas/SGR/viabilidadSGR/firmaEmisionSgrServicio.js",
                  "~/src/app/formulario/ventanas/SGR/viabilidadSGR/requisitos/verificacion/verificacionSgrController.js",
                  "~/src/app/formulario/ventanas/SGR/viabilidadSGR/requisitos/verificacion/generales/generalesSgrController.js",
                  "~/src/app/formulario/ventanas/SGR/viabilidadSGR/requisitos/datosGenerales/requisitosDatosGeneralesSgrController.js",
                  "~/src/app/formulario/ventanas/SGR/viabilidadSGR/requisitos/datosGenerales/agregarSectores/agregarSectoresSgrController.js",
                  "~/src/app/formulario/ventanas/SGR/viabilidadSGR/requisitos/datosGenerales/datosIniciales/datosInicialesSgrController.js",
                   "~/src/app/formulario/ventanas/SGR/viabilidadSGR/requisitos/datosGenerales/datosAdicionales/datosAdicionalesSgrController.js",
                  "~/src/app/formulario/ventanas/SGR/viabilidadSGR/requisitos/soportes/alojarDocumentosRequisitosSgrController.js",

                  "~/src/app/formulario/ventanas/SGR/viabilidadSGR/viabilidad/soportes/alojarDocumentosViabilidadSgrController.js",
                  "~/src/app/formulario/ventanas/SGR/comun/documentoSoporte/archivosFormularioSgrController.js",
                  "~/src/app/formulario/ventanas/SGR/viabilidadSGR/previos/recursos/recursosSgrController.js",
                  "~/src/app/formulario/ventanas/SGR/viabilidadSGR/previos/recursos/fuentes/fuentesSgrController.js",
                  "~/src/app/formulario/ventanas/SGR/viabilidadSGR/previos/recursos/fuentesNoSgr/fuentesNoSgrController.js",
                  "~/src/app/formulario/ventanas/SGR/viabilidadSGR/previos/recursos/fuentesNoSgr/addCofinanciacionSgrController.js",
                  "~/src/app/formulario/ventanas/SGR/viabilidadSGR/previos/operacionesCredito/operacionesCreditoSgrController.js",
                  "~/src/app/formulario/ventanas/SGR/viabilidadSGR/previos/operacionesCredito/operacionesCreditoSgrServicio.js",
                  "~/src/app/formulario/ventanas/SGR/viabilidadSGR/previos/operacionesCredito/datosGenerales/datosGeneralesSgrController.js",
                  "~/src/app/formulario/ventanas/SGR/viabilidadSGR/previos/operacionesCredito/informacionDetallada/informacionDetalladaSgrController.js",
                  "~/src/app/formulario/ventanas/SGR/viabilidadSGR/previos/operacionesCredito/informacionDetallada/informacionDetalladaSgrModalAgregarFuenteController.js",
                  "~/src/app/formulario/ventanas/SGR/viabilidadSGR/previos/documentos/documentoSoporteSgrController.js",
                  "~/src/app/formulario/ventanas/SGR/viabilidadSGR/previos/recursos/resumenFuentesCostos/resumenFuentesCostosSgrController.js",
                  "~/src/app/formulario/ventanas/SGR/viabilidadSGR/previos/ejecutorViabilidad/ejecutorViabilidadSgrController.js",
                  
                  "~/src/app/formulario/ventanas/SGR/viabilidadSGR/previos/ejecutorViabilidad/ejecutor/ejecutorProSgrController.js",
                  "~/src/app/formulario/ventanas/SGR/viabilidadSGR/previos/ejecutorViabilidad/ejecutor/ejecutorProSgrServicio.js",                  
                  
                  "~/src/app/formulario/ventanas/SGR/viabilidadSGR/previos/datosGeneralesPrevios/datosGeneralesPreviosSgrController.js",
                  "~/src/app/formulario/ventanas/SGR/viabilidadSGR/previos/datosGeneralesPrevios/datosPresentacion/datosPresentacionPreviosSgrController.js",
                  "~/src/app/formulario/ventanas/SGR/viabilidadSGR/previos/regionalizacion/regionalizacionSgrController.js",
                  "~/src/app/formulario/ventanas/SGR/viabilidadSGR/previos/regionalizacion/regionalizacionSgr/regionalizacionSgrSgrController.js",
                  "~/src/app/formulario/ventanas/SGR/viabilidadSGR/previos/focalizacion/PoliticasTransversales/politicasTransversalesSgrController.js",
                  "~/src/app/formulario/ventanas/SGR/viabilidadSGR/previos/focalizacion/ResumenFocalizacion/ResumenFocalizacionSgrController.js",
                  "~/src/app/formulario/ventanas/SGR/viabilidadSGR/previos/focalizacion/CrucePoliticasTransversales/crucePoliticasTransversalesSgrController.js",
                  "~/src/app/formulario/ventanas/SGR/viabilidadSGR/previos/focalizacion/Resumen/ResumenSgrController.js",
                  "~/src/app/formulario/ventanas/SGR/viabilidadSGR/previos/focalizacion/ResumenFocalizacion/ResumenFocalizacionCategoria/ResumenFocalizacionCatergoriaSgrController.js",
                  "~/src/app/formulario/ventanas/SGR/viabilidadSGR/previos/focalizacion/focalizacionAjustesSgrController.js",
                  "~/src/app/formulario/ventanas/SGR/viabilidadSGR/previos/focalizacion/focalizacionAjustesSgrServicio.js",
                  "~/src/app/formulario/ventanas/SGR/viabilidadSGR/viabilidad/viabilidad/viabilidadViabilidadSgrController.js",
                  "~/src/app/formulario/ventanas/SGR/viabilidadSGR/viabilidad/viabilidad/cuestionarioViabilidad/viabilidadGeneralesSgrController.js",
                  "~/src/app/formulario/ventanas/SGR/viabilidadSGR/viabilidad/viabilidad/cuestionarioViabilidad/cuestionarioViabilidadSgrController.js",
                  "~/src/app/formulario/ventanas/SGR/viabilidadSGR/viabilidad/informacionGeneral/informacionGeneralViabilidadSgrController.js",
                  "~/src/app/formulario/ventanas/SGR/viabilidadSGR/viabilidad/informacionGeneral/informacionBasica/informacionBasicaViabilidadSgrController.js",
                  "~/src/app/formulario/ventanas/SGR/viabilidadSGR/viabilidad/informacionGeneral/conceptosPrevios/conceptosPreviosEmitidosViabilidadSgrController.js",
                  "~/src/app/formulario/ventanas/SGR/viabilidadSGR/viabilidad/informacionGeneral/usuariosInvolucrados/usuariosInvolucradosViabilidadSgrController.js",
                  "~/src/app/formulario/ventanas/SGR/comun/usuariosInvolucrados/usuariosInvolucradosSgrServicio.js",
                  "~/src/app/formulario/ventanas/SGR/comun/usuariosInvolucrados/usuariosInvolucradosFormularioSgrController.js",
                  "~/src/app/formulario/ventanas/SGR/comun/usuariosInvolucrados/modalAgregarInvolucradosSgrController.js",
                  "~/src/app/formulario/ventanas/SGR/viabilidadSGR/firmaEmision/procesoFirma/procesoFirmaViabilidadSgrController.js",
                  "~/src/app/formulario/ventanas/SGR/viabilidadSGR/firmaEmision/procesoFirma/procesoFirmaSgr/procesoFirmaViabilidadSgrSgrController.js",
                  "~/src/app/formulario/ventanas/SGR/comun/firma/firmaFormularioSgrController.js",
                  "~/src/app/formulario/ventanas/SGR/comun/firma/firmaFormularioSgrServicio.js",

            #endregion

            #region CTUS SGR
                  "~/src/app/formulario/ventanas/SGR/comun/conceptosPrevios/conceptosPreviosEmitidosSgrController.js",
                  "~/src/app/formulario/ventanas/SGR/comun/conceptosPrevios/conceptosPreviosEmitidosSgrServicio.js",
                  "~/src/app/formulario/ventanas/SGR/ctus/ctusAsignacionSgrServicio.js",
                  "~/src/app/formulario/ventanas/SGR/ctus/ctusAsignacionSgrController.js",
                  "~/src/app/formulario/ventanas/SGR/ctus/asignacion/asignacionTecnico/asignacionTecnicoCtusSgrController.js",
                  "~/src/app/formulario/ventanas/SGR/ctus/asignacion/asignacionTecnico/asignar/asignarCtusSgrController.js",
                  "~/src/app/formulario/ventanas/SGR/ctus/asignacion/asignacionTecnico/delegar/delegarCtusSgrController.js",
                  "~/src/app/formulario/ventanas/SGR/ctus/ctusElaboracionSgrServicio.js",
                  "~/src/app/formulario/ventanas/SGR/ctus/ctusElaboracionSgrController.js",
                  "~/src/app/formulario/ventanas/SGR/ctus/elaboracion/informacionGeneral/informacionGeneralCtusSgrController.js",
                  "~/src/app/formulario/ventanas/SGR/ctus/elaboracion/informacionGeneral/informacionBasica/informacionBasicaCtusSgrController.js",
                  "~/src/app/formulario/ventanas/SGR/ctus/elaboracion/informacionGeneral/conceptosPrevios/conceptosPreviosEmitidosCtusSgrController.js",
                  "~/src/app/formulario/ventanas/SGR/ctus/elaboracion/informacionGeneral/usuariosInvolucrados/usuariosInvolucradosCtusSgrController.js",
                  "~/src/app/formulario/ventanas/SGR/ctus/elaboracion/ctus/ctusSgrController.js",
                  "~/src/app/formulario/ventanas/SGR/ctus/elaboracion/ctus/concepto/conceptoCtusSgrController.js",
                  "~/src/app/formulario/ventanas/SGR/ctus/elaboracion/ctus/resultadoconcepto/resultadoConceptoCtusSgrController.js",
                  "~/src/app/formulario/ventanas/SGR/ctus/elaboracion/datosVerificacion/datosVerificacionCtusSgrController.js",
                  "~/src/app/formulario/ventanas/SGR/ctus/elaboracion/datosVerificacion/agregarSectores/agregarSectoresCtusSgrController.js",
                  "~/src/app/formulario/ventanas/SGR/ctus/elaboracion/datosVerificacion/datosAdicionales/datosAdicionalesCtusSgrController.js",
                  "~/src/app/formulario/ventanas/SGR/ctus/elaboracion/soportes/alojarDocumentosCtusSgrController.js",
                  "~/src/app/formulario/ventanas/SGR/ctus/elaboracion/verificacion/verificacionCtusSgrController.js",
                  "~/src/app/formulario/ventanas/SGR/ctus/elaboracion/verificacion/generales/generalesCtusSgrController.js",
                  "~/src/app/formulario/ventanas/SGR/ctus/ctusFirmaEmisionSGRController.js",
                  "~/src/app/formulario/ventanas/SGR/ctus/ctusFirmaEmisionSGRServicio.js",
                  "~/src/app/formulario/ventanas/SGR/ctus/firmaEmision/procesoFirma/procesoFirmaCtusSgrController.js",
                  "~/src/app/formulario/ventanas/SGR/ctus/firmaEmision/procesoFirma/procesoFirmaSgr/procesoFirmaCtusSgrSgrController.js",
            #endregion

            #region OCAD Paz SGR                

                  "~/src/app/formulario/ventanas/SGR/ocadPaz/solicitarCtusSgrServicio.js",
                  "~/src/app/formulario/ventanas/SGR/ocadPaz/solicitarCtusSgrController.js",
                  "~/src/app/formulario/ventanas/SGR/ocadPaz/SolicitarCtus/solicitarCtusController.js",
                  "~/src/app/formulario/ventanas/SGR/ocadPaz/SolicitarCtus/SolicitarConceptoUnicoSectorial/solicitarConceptoUnicoSectorialSgrController.js",
                  "~/src/app/formulario/ventanas/SGR/ocadPaz/requisitosPazSgrController.js",
                  "~/src/app/formulario/ventanas/SGR/ocadPaz/requisitosPazSgrServicio.js",
                  "~/src/app/formulario/ventanas/SGR/ocadPaz/requisitos/verificacion/verificacionPazSgrController.js",
                  "~/src/app/formulario/ventanas/SGR/ocadPaz/requisitos/verificacion/generales/generalesPazSgrController.js",
                  "~/src/app/formulario/ventanas/SGR/ocadPaz/viabilidadOcadPazSgrServicio.js",
                  "~/src/app/formulario/ventanas/SGR/ocadPaz/viabilidadOcadPazSgrController.js",
                  "~/src/app/formulario/ventanas/SGR/ocadPaz/viabilidad/viabilidad/viabilidadViabilidadOcadPazSgrController.js",
                  "~/src/app/formulario/ventanas/SGR/ocadPaz/viabilidad/viabilidad/registroVotos/registroVotosViabilidadOcadPazSgrController.js",
                  "~/src/app/formulario/ventanas/SGR/ocadPaz/viabilidad/viabilidad/viabilidadSepOcadPazSgrController.js",
                  "~/src/app/formulario/ventanas/SGR/ocadPaz/viabilidad/viabilidad/sep/sepOcadPazSgrController.js",
            #endregion

            #region Entidad Nacional                 
                  "~/src/app/formulario/ventanas/SGR/entidadNacional/asignacionEntidadNacionalSgrController.js",
                  "~/src/app/formulario/ventanas/SGR/entidadNacional/asignacionEntidadNacionalSgrServicio.js",
                  "~/src/app/formulario/ventanas/SGR/entidadNacional/asignacionViabilidad/asignacionTecnico/asignacionTecnicoNacionalSgrController.js",
                  "~/src/app/formulario/ventanas/SGR/entidadNacional/asignacionViabilidad/asignacionTecnico/delegar/delegarViabilidadNacionalSgrController.js",
                  "~/src/app/formulario/ventanas/SGR/entidadNacional/asignacionViabilidad/asignacionTecnico/asignar/asignarViabilidadNacionalSgrController.js",

                  "~/src/app/formulario/ventanas/SGR/entidadNacional/elaboracionEntidadNacionalSgrController.js",
                  "~/src/app/formulario/ventanas/SGR/entidadNacional/elaboracionEntidadNacionalSgrServicio.js",
            #endregion Entidad Nacional

            #region CTEI
                  "~/src/app/formulario/ventanas/SGR/ctei/requisitosViabilidadCteiSgrServicio.js",
                  "~/src/app/formulario/ventanas/SGR/ctei/requisitosViabilidadCteiSgrController.js",
                  "~/src/app/formulario/ventanas/SGR/ctei/requisitos/datosCtei/requisitosDatosCteiSgrController.js",
                  "~/src/app/formulario/ventanas/SGR/ctei/requisitos/datosCtei/datosAdicionales/datosAdicionalesCteiSgrController.js",
                  "~/src/app/formulario/ventanas/SGR/ctei/viabilidadVotosSgrController.js",
                  "~/src/app/formulario/ventanas/SGR/ctei/viabilidadVotosSgrServicio.js",
                  "~/src/app/formulario/ventanas/SGR/ctei/viabilidad/viabilidad/viabilidadViabilidadVotosSgrController.js",
                  "~/src/app/formulario/ventanas/SGR/ctei/viabilidad/viabilidad/registroVotos/registroVotosViabilidadSgrController.js",
                  "~/src/app/formulario/ventanas/SGR/ctei/requisitos/datosCtei/usuariosInvolucrados/usuariosInvolucradosCteiSgrController.js",
            #endregion CTEI

            #region Aval de Uso
                  //Abuelo Paso
                  "~/src/app/formulario/ventanas/SGR/EntidadNacionalFlujo9/avalusoViabilidadSgrController.js",
                  "~/src/app/formulario/ventanas/SGR/EntidadNacionalFlujo9/avalusoViabilidadSgrServicio.js",

                  //Papà e Hijo Registro 
                  "~/src/app/formulario/ventanas/SGR/EntidadNacionalFlujo9/registro/registroAvalSgrController.js",
                  "~/src/app/formulario/ventanas/SGR/EntidadNacionalFlujo9/registro/avaluso/avalUsoSgrController.js",

                  //Papà e Hijo Soporte
                  "~/src/app/formulario/ventanas/SGR/EntidadNacionalFlujo9/soportes/soportesAvalUsoSgrController.js",
                  "~/src/app/formulario/ventanas/SGR/EntidadNacionalFlujo9/soportes/alojararchivo/alojarArchivoController.js",
            #endregion Aval de Uso

            #region CTUS Integrado

                  "~/src/app/formulario/ventanas/SGR/ctusIntegrado/ctusIntegradoElaboracionSGRController.js",
                  "~/src/app/formulario/ventanas/SGR/ctusIntegrado/ctusIntegradoElaboracionSGRServicio.js",
                  "~/src/app/formulario/ventanas/SGR/ctusIntegrado/elaboracion/informacionGeneral/informacionGeneralCtusIntegradoSgrController.js",
                  "~/src/app/formulario/ventanas/SGR/ctusIntegrado/elaboracion/informacionGeneral/conceptosPrevios/conceptosPreviosEmitidosCtusIntegradoSgrController.js",
                  "~/src/app/formulario/ventanas/SGR/ctusIntegrado/elaboracion/informacionGeneral/informacionBasica/informacionBasicaCtusIntegradoSgrController.js",
                  "~/src/app/formulario/ventanas/SGR/ctusIntegrado/elaboracion/informacionGeneral/usuariosInvolucrados/usuariosInvolucradosCtusIntegradoSgrController.js",
                  "~/src/app/formulario/ventanas/SGR/ctusIntegrado/elaboracion/datosVerificacion/datosVerificacionCtusISgrController.js",
                  "~/src/app/formulario/ventanas/SGR/ctusIntegrado/elaboracion/datosVerificacion/agregarSectores/agregarSectoresCtusISgrController.js",
                  "~/src/app/formulario/ventanas/SGR/ctusIntegrado/elaboracion/datosVerificacion/datosAdicionales/datosAdicionalesCtusISgrController.js",
                  "~/src/app/formulario/ventanas/SGR/ctusIntegrado/elaboracion/verificacion/verificacionCtusISgrController.js",
                  "~/src/app/formulario/ventanas/SGR/ctusIntegrado/elaboracion/verificacion/generales/generalesCtusISgrController.js",
                  "~/src/app/formulario/ventanas/SGR/ctusIntegrado/ctusIntegradoSolicitarSgrController.js",
                  "~/src/app/formulario/ventanas/SGR/ctusIntegrado/Solicitar/solicitarCtusIntegradoController.js",
                  "~/src/app/formulario/ventanas/SGR/ctusIntegrado/Solicitar/SolicitarCtusIntegrado/solicitarIntegradoSgrController.js",
                  "~/src/app/formulario/ventanas/SGR/ctusIntegrado/ctusIntegradoAsignacionSgrServicio.js",
                  "~/src/app/formulario/ventanas/SGR/ctusIntegrado/ctusIntegradoAsignacionSgrController.js",
                  "~/src/app/formulario/ventanas/SGR/ctusIntegrado/asignacion/asignacionTecnico/asignacionTecnicoCtusIntegradoSgrController.js",
                  "~/src/app/formulario/ventanas/SGR/ctusIntegrado/asignacion/asignacionTecnico/delegar/delegarCtusIntegradoSgrController.js",
                  "~/src/app/formulario/ventanas/SGR/ctusIntegrado/asignacion/asignacionTecnico/asignar/asignarCtusIntegradoSgrController.js",

                  "~/src/app/formulario/ventanas/SGR/ctusIntegrado/elaboracion/ctus/ctusISgrController.js",
                  "~/src/app/formulario/ventanas/SGR/ctusIntegrado/elaboracion/ctus/resultadoconcepto/resultadoConceptoCtusISgrController.js",
                  "~/src/app/formulario/ventanas/SGR/ctusIntegrado/elaboracion/ctus/concepto/conceptoCtusISgrController.js",
                  "~/src/app/formulario/ventanas/SGR/ctusIntegrado/elaboracion/viabilidad/viabilidadCtusIntegradoSgrController.js",
                  "~/src/app/formulario/ventanas/SGR/ctusIntegrado/elaboracion/viabilidad/cuestionarioViabilidad/cuestionarioCtusIntegradoSgrController.js",

                  "~/src/app/formulario/ventanas/SGR/ctusIntegrado/ctusIntegradoFirmaSgrController.js",
                  "~/src/app/formulario/ventanas/SGR/ctusIntegrado/firmaEmision/procesoFirma/procesoFirmaCtusIntegradoSgrController.js",
                  "~/src/app/formulario/ventanas/SGR/ctusIntegrado/firmaEmision/procesoFirma/procesoFirmaSgr/firmaCtusIntegradoSgrController.js",

                  "~/src/app/formulario/ventanas/SGR/ctusIntegrado/elaboracion/soportes/alojarDocumentosCtusIntegradoSgrController.js",

            #endregion

            #region Cormagdalena SGR

                  "~/src/app/formulario/ventanas/SGR/CormagdalenaFlujo10/asignacionCormagdalenaSgrController.js",
                  "~/src/app/formulario/ventanas/SGR/CormagdalenaFlujo10/elaboracionCormagdalenaSgrController.js",
                  "~/src/app/formulario/ventanas/SGR/CormagdalenaFlujo10/firmaCormagdalenaSgrController.js",

            #endregion Cormagdalena SGR

            #region Ejecutor SGR
                  "~/src/app/formulario/ventanas/SGR/gestionRecursos/ejecutor/designarEjecutorSgrServicio.js",
                  "~/src/app/formulario/ventanas/SGR/gestionRecursos/ejecutor/designacionEjecutorSgrController.js",
                  "~/src/app/formulario/ventanas/SGR/gestionRecursos/ejecutor/entEjecutora/entEjecutoraSgrController.js",
                  "~/src/app/formulario/ventanas/SGR/gestionRecursos/ejecutor/entEjecutora/registro/registroEjecutorSgrController.js",
                  "~/src/app/formulario/ventanas/SGR/gestionRecursos/ejecutor/CostosEstructuracion/ejecutorDesignacionCostosSgrController.js",
                  "~/src/app/formulario/ventanas/SGR/gestionRecursos/ejecutor/costosEstructuracion/giroReconocimientoSgr/giroReconocimientoSgrController.js",
                  "~/src/app/formulario/ventanas/SGR/gestionRecursos/ejecutor/costosEstructuracion/reconocimientoCostosSgr/reconocimientoCostosSgrController.js",
                  "~/src/app/formulario/ventanas/SGR/gestionRecursos/ejecutor/costosEstructuracion/valorEstructuracionSgr/valorEstructuracionSgrController.js",
                  "~/src/app/formulario/ventanas/SGR/gestionRecursos/ejecutor/soportes/alojarDocumentosEjecutorSgrController.js",

            #endregion Ejecutor SGR

            #region Interventor SGR
                  "~/src/app/formulario/ventanas/SGR/gestionRecursos/ejecutor/entInterventora/entInterventoraSgrController.js",
                  "~/src/app/formulario/ventanas/SGR/gestionRecursos/ejecutor/entInterventora/registro/registroInterventorSgrController.js",
                  "~/src/app/formulario/ventanas/SGR/gestionRecursos/ejecutor/entInterventora/supervisorRegistroDesignacion/supervisorRegistroDesignacionSgrController.js",
                  "~/src/app/formulario/ventanas/SGR/gestionRecursos/ejecutor/entInterventora/valorInterventoriaSgr/valorInterventoriaSgrController.js",
                  "~/src/app/formulario/ventanas/SGR/gestionRecursos/ejecutor/entInterventora/valorSupervisorSgr/valorSupervisorSgrController.js",
            #endregion Interventor SGR

            #region Priorizacion SGR
                  "~/src/app/formulario/ventanas/SGR/Priorizacion/M1/priorizacionM1SgrController.js",
                  "~/src/app/formulario/ventanas/SGR/Priorizacion/M1/soportes/alojarDocumentosPriorizacionSgrController.js",
            #endregion Priorizacion SGR

            #region Aprobacion SGR
                  "~/src/app/formulario/ventanas/SGR/gestionRecursos/aprobacion/aprobacionSgrServicio.js",
                  "~/src/app/formulario/ventanas/SGR/gestionRecursos/aprobacion/F1/aprobacionF1SgrController.js",
                  "~/src/app/formulario/ventanas/SGR/gestionRecursos/aprobacion/F1/aprobacion/aprobacionF1ApruebaSgrController.js",
                  "~/src/app/formulario/ventanas/SGR/gestionRecursos/aprobacion/F1/aprobacion/aprobacionF1ApruebaOpcSgrController.js",
                  "~/src/app/formulario/ventanas/SGR/gestionRecursos/aprobacion/F1/soportes/alojarDocumentosAprobacionSgrController.js",
                  "~/src/app/formulario/ventanas/SGR/gestionRecursos/aprobacion/F1/aprobacion/registro/registroAprobacionSgrController.js",
                  "~/src/app/formulario/ventanas/SGR/gestionRecursos/aprobacion/F1/aprobacion/credito/registroAprobacionOpcSgrController.js",
                  "~/src/app/formulario/ventanas/SGR/gestionRecursos/aprobacion/F1/aprobacion/resumen/resumenAprobacionSgrController.js",
                  "~/src/app/formulario/ventanas/SGR/gestionRecursos/aprobacion/F1/aprobacion/resumenparcial/resumenAprobacionParcialSgrController.js",
                  "~/src/app/formulario/ventanas/SGR/gestionRecursos/aprobacion/F1/aprobacion/resumencredito/resumenCreditoAprobacionSgrController.js",
            #endregion Aprobacion SGR

            #region Verificación SGR
                  "~/src/app/comunes/notificaciones/notificacionCantidadMisProcesosAsignarVerificacionSgr/componenteNotificacionCantidadMisProcesosAsignarVerificacionSgr.js",
                  "~/src/app/panelPrincial/componentes/inbox/controladorProyectosAsignarVerificacionSgr.js",
                  "~/src/app/formulario/ventanas/SGR/ocadPaz/verificacion/asignacion/modalAsignarTecnicoSgrController.js",
                  "~/src/app/comunes/notificaciones/notificacionCantidadMisProcesosSeguimientoControlVerificacionSgr/componenteNotificacionCantidadMisProcesosSeguimientoControlVerificacionSgr.js",
                  "~/src/app/panelPrincial/componentes/inbox/controladorProyectosSeguimientoControlVerificacionSgr.js",
                  "~/src/app/comunes/notificaciones/notificacionCantidadMisProcesosRevisionCoordinadorVerificacionSgr/componenteNotificacionCantidadMisProcesosRevisionCoordinadorVerificacionSgr.js",
                  "~/src/app/panelPrincial/componentes/inbox/controladorProyectosRevisionCoordinadorVerificacionSgr.js",
                  "~/src/app/comunes/notificaciones/notificacionCantidadMisProcesosSubdirectorVerificacionSgr/componenteNotificacionCantidadMisProcesosSubdirectorVerificacionSgr.js",
                  "~/src/app/panelPrincial/componentes/inbox/controladorProyectosSubdirectorVerificacionSgr.js",
            #endregion

            #region Viabilizacion

                  "~/src/app/formulario/ventanas/viabilidad/viabilidadServicio.js",
                  "~/src/app/formulario/ventanas/viabilidad/viabilidadController.js",
                  "~/src/app/formulario/ventanas/viabilidad/componentes/preguntasTecnico/preguntasTecnicoController.js",
                  "~/src/app/formulario/ventanas/viabilidad/componentes/preguntasTecnico/especificas/tecnicoEspecificasController.js",
                  "~/src/app/formulario/ventanas/viabilidad/componentes/preguntasTecnico/generales/tecnicoGeneralesController.js",
                  "~/src/app/formulario/ventanas/viabilidad/componentes/preguntasLider/preguntasLiderController.js",
                  "~/src/app/formulario/ventanas/viabilidad/componentes/preguntasLider/especificas/liderEspecificasController.js",
                  "~/src/app/formulario/ventanas/viabilidad/componentes/preguntasLider/generales/liderGeneralesController.js",
                  "~/src/app/formulario/ventanas/viabilidad/componentes/preguntasPersonalizadasVerificacionController.js",
                  "~/src/app/formulario/ventanas/viabilidad/componentes/documentosSoporteVi/documentosSoporteViController.js",
                  "~/src/app/formulario/ventanas/SGR/viabilidadSGR/viabilidadSgrServicio.js",
                  "~/src/app/formulario/ventanas/SGR/viabilidadSGR/viabilidadSgrController.js",


            #endregion

            #region vigenciaFuturasExcepcionales
                  "~/src/app/formulario/ventanas/tramiteVFexcep/tramiteVfexcepController.js",
                  "~/src/app/formulario/ventanas/tramiteVFexcep/componentes/documentoSoporte/documentoSoporteVfexcepController.js",
                  "~/src/app/formulario/ventanas/tramiteVFexcep/componentes/aprobacionEntidad/aproentVfexcepController.js",
                  "~/src/app/formulario/ventanas/tramiteVFexcep/componentes/aprobacionEntidad/aprobacionEntidadDet/aproentVfexcepDetController.js",
                  "~/src/app/formulario/ventanas/tramiteVFexcep/componentes/informacionPresupuestalVfexcep/informacionPresupuestalVfexcepController.js",
                  "~/src/app/formulario/ventanas/tramiteVFexcep/componentes/aprobacionSupervisor/aprosuperVfexcepController.js",
                  "~/src/app/formulario/ventanas/tramiteVFexcep/componentes/firmaVfexcep/firmaVfexcepController.js",
                  "~/src/app/formulario/ventanas/tramiteVFexcep/componentes/conceptoVfexcep/conceptoVfexcepController.js",
                  "~/src/app/formulario/ventanas/tramiteVFexcep/componentes/conceptoVfexcep/elaborarConcepto/elaborarConceptoVfexcpController.js",
            #endregion

            #region tramite Cargue Masivo
                  "~/src/app/formulario/ventanas/tramiteCargueMasivo/tramiteCargueMasivoController.js",
                  "~/src/app/formulario/ventanas/tramiteCargueMasivo/tramiteCargueMasivoServicio.js",

                  "~/src/app/formulario/ventanas/tramiteCargueMasivo/componentes/seleccionarCargue/seleccionarCargueController.js",
                  "~/src/app/formulario/ventanas/tramiteCargueMasivo/componentes/seleccionarCargue/seleccionarCargueServicio.js",

                  "~/src/app/formulario/ventanas/tramiteCargueMasivo/componentes/seleccionarCargue/componentes/cargueArchivo/cargueArchivoCapController.js",
                  "~/src/app/formulario/ventanas/tramiteCargueMasivo/componentes/seleccionarCargue/componentes/cargueArchivo/cargueArchivoCapServicio.js",
            #endregion

            #endregion

            #region Comun
                    "~/src/app/formulario/ventanas/comun/preguntas/preguntasController.js",
                    "~/src/app/formulario/ventanas/comun/aprobacion/aprobacionController.js",
                    "~/src/app/formulario/ventanas/comun/informacionPresupuestal/informacionPresupuestalFormulario.js",
                    "~/src/app/formulario/ventanas/comun/informacionDistribuciones/informacionDistribucionesFormulario.js",
                    "~/src/app/formulario/ventanas/comun/archivoProcesoActual/archivoProcesoActualController.js",
                    "~/src/app/formulario/ventanas/comun/JustificacionTramite/justificacionTramiteController.js",

                    "~/src/app/formulario/ventanas/comun/informacionPresupuestalRpRvf/agregarRegistroPresupuestalFormularioController.js",
                    "~/src/app/formulario/ventanas/comun/informacionPresupuestalRpRvf/modalEditarCrpController.js",
                    "~/src/app/formulario/ventanas/comun/InformacionPresupuestalResumenRvf/resumenReprogramacionVfFormulario.js",

                    "~/src/app/formulario/ventanas/comun/tablaResumen/tablaResumenPorVigenciaFormulario.js",
                    "~/src/app/formulario/ventanas/comun/modalPND/modalPNDController.js",
                    "~/src/app/formulario/ventanas/comun/modalPND/modalPNDServicio.js",

            #endregion

            #region Transversales
                 "~/src/app/formulario/ventanas/transversal/validacionSeccionCapitulos/utilsValidacionSeccionCapitulosServicio.js",
                "~/src/app/formulario/ventanas/transversal/justificacionCambios/componentes/datosgenerales/relacionPlanificacion/relacionPlanificacionController.js",
                "~/src/app/formulario/ventanas/transversal/justificacionCambios/componentes/datosgenerales/relacionPlanificacion/relacionPlanificacionServicio.js",
                "~/src/app/formulario/ventanas/transversal/justificacionCambios/justificacionCambiosController.js",
                "~/src/app/formulario/ventanas/transversal/justificacionCambios/justificacionCambiosServicio.js",
                 "~/src/app/formulario/ventanas/transversal/justificacionCambios/componentes/datosgenerales/justificacionHorizonte/justificacionHorizonteServicio.js",
                 "~/src/app/formulario/ventanas/transversal/justificacionCambios/componentes/datosgenerales/justificacionHorizonte/justificacionHorizonteController.js",
                 "~/src/app/formulario/ventanas/transversal/justificacionCambios/componentes/datosgenerales/justificacionBeneficiarios/justificacionBeneficiariosController.js",
                 "~/src/app/formulario/ventanas/transversal/justificacionCambios/componentes/datosgenerales/justificacionBeneficiarios/justificacionBeneficiariosServicio.js",
                "~/src/app/formulario/ventanas/transversal/justificacionCambios/componentes/recursos/fuentes/recursosfuentesdefinancController.js",
                "~/src/app/formulario/ventanas/transversal/justificacionCambios/componentes/recursos/fuentes/recursosfuentesdefinancServicio.js",
                "~/src/app/formulario/ventanas/transversal/justificacionCambios/componentes/datosgenerales/justificacionIndicadores/justificacionIndicadoresController.js",
                "~/src/app/formulario/ventanas/transversal/justificacionCambios/componentes/datosgenerales/justificacionIndicadores/justificacionIndicadoresServicio.js",
                "~/src/app/formulario/ventanas/transversal/justificacionCambios/componentes/datosgenerales/justificacionLocalizacion/JustificacionLocalizacionController.js",
                "~/src/app/formulario/ventanas/transversal/justificacionCambios/componentes/datosgenerales/justificacionLocalizacion/JustificacionLocalizacionServicio.js",
                "~/src/app/formulario/ventanas/transversal/justificacionCambios/componentes/recursos/regionalizacion/recursosregionalizacionController.js",
                "~/src/app/formulario/ventanas/transversal/justificacionCambios/componentes/recursos/regionalizacion/recursosregionalizacionServicio.js",

            #endregion

            #region Programacion
                "~/src/app/programacion/programacionController.js",
                "~/src/app/programacion/servicioProgramacion.js",
                "~/src/app/programacion/componentes/controladorProgramacion.js",
                "~/src/app/programacion/componentes/cargaMasiva/creditoController.js",
                "~/src/app/programacion/componentes/cargaMasiva/saldosController.js",
                "~/src/app/programacion/componentes/cargaMasiva/creditoServicio.js",
                "~/src/app/programacion/componentes/cargaMasiva/cuotaController.js",
                "~/src/app/programacion/componentes/cargaMasiva/cuotaServicio.js",
                "~/src/app/programacion/modales/tramite/agregarTramiteModalController.js",
                "~/src/app/programacion/modales/tramite/servicioAgregarTramiteModal.js",
                "~/src/app/programacion/modales/excepciones/agregarExcepcionModalController.js",
                "~/src/app/programacion/modales/excepciones/servicioAgregarExcepcionModal.js",
                "~/src/app/programacion/modales/ActualizarFechas/actualizarFechasModalController.js",
                "~/src/app/programacion/modales/ActualizarFechas/servicioActualizarFechasModal.js",
                "~/src/app/programacion/modales/configurarMensajeModal/configurarMensajeModalController.js",
                "~/src/app/programacion/componentes/generarPresupuestal/presupuestalController.js",
                "~/src/app/programacion/componentes/generarPresupuestal/presupuestalServicio.js",
                "~/src/app/programacion/componentes/calendario/calendarioController.js",
                "~/src/app/programacion/componentes/calendario/calendarioServicio.js",
                 "~/src/app/programacion/componentes/cargaMasiva/saldosServicio.js",
            #endregion Programacion

            #region Notificaciones Mantenimiento
                "~/src/app/notificacionesMantenimiento/listaNotificaciones/listaNotificacionesMantenimientoController.js",
                "~/src/app/notificacionesMantenimiento/servicioNotificacionesMantenimiento.js",
                "~/src/app/notificacionesMantenimiento/listaNotificacionesMensajes/listaNotificacionesMensajesController.js",
                "~/src/app/notificacionesMantenimiento/listaNotificacionesMensajes/componentes/controladorListaNotificacionesMensajes.js",
                "~/src/app/notificacionesMantenimiento/listaNotificacionesMensajes/servicioNotificacionesMensajes.js",
                "~/src/app/notificacionesMantenimiento/crearActualizarNotificacion/crearActualizarNotificacionController.js",
                "~/src/app/notificacionesMantenimiento/modales/visualizarNotificacion/visualizarNotificacionController.js",
            #endregion

            #region Centro de Ayuda
                "~/src/app/centroAyuda/servicioCentroAyuda.js",
                "~/src/app/centroAyuda/centroAyudaLista/centroAyudaController.js",
                "~/src/app/centroAyuda/centroAyudaMenu/centroAyudaMenuController.js",
                "~/src/app/centroAyuda/modales/crearEditarcentroAyudaItem/crearEditarCentroAyudaItemController.js",
                "~/src/app/centroAyuda/modales/exhibicionContenidoAyudaItem/exhibicionContenidoAyudaItemController.js",
            #endregion Centro de Ayuda

            #region modificacion traslado presupuesto preliminar

                "~/src/app/formulario/ventanas/tramiteModificacionTraslado/tramiteModificacionTrasladoServicio.js",
                "~/src/app/formulario/ventanas/tramiteModificacionTraslado/presupuestoPrel/tramiteModificacionTrasladoPasoUnoController.js",
                "~/src/app/formulario/ventanas/tramiteModificacionTraslado/presupuestoPrel/tramiteModificacionTrasladoPasoDosController.js",
                "~/src/app/formulario/ventanas/tramiteModificacionTraslado/presupuestoPrel/tramiteModificacionTrasladoPasoTresController.js",
                "~/src/app/formulario/ventanas/tramiteModificacionTraslado/presupuestoPrel/tramiteModificacionTrasladoPasoCuatroController.js",
                "~/src/app/formulario/ventanas/tramiteModificacionTraslado/Componentes/asociarProyectoTr/asociarProyectoTrController.js",
                "~/src/app/formulario/ventanas/tramiteModificacionTraslado/Componentes/aprobacionEntidadTr/aprobacionEntidadTrController.js",
                "~/src/app/formulario/ventanas/tramiteModificacionTraslado/Componentes/informacionPresupuestalTr/informacionPresupuestalTrController.js",
                "~/src/app/formulario/ventanas/tramiteModificacionTraslado/Componentes/aprobacionSupervisor/aproSuperTrController.js",

                "~/src/app/formulario/ventanas/comun/modificacionLey/informacionPresupuestalMltrController.js",
                "~/src/app/formulario/ventanas/comun/modificacionLey/creditos/modificacionLeyTrCreditosController.js",
            #endregion

            #region modificacion traslado Analista

                "~/src/app/formulario/ventanas/tramiteModificacionTraslado/analista/tramiteModificacionTrasladoAPasoTresController.js",
                "~/src/app/formulario/ventanas/tramiteModificacionTraslado/analista/tramiteModificacionTrasladoAPasoCuatroController.js",
                "~/src/app/formulario/ventanas/tramiteModificacionTraslado/componentes/aprobacionAnalistaTr/aproAnalistaTrController.js",

                "~/src/app/formulario/ventanas/comun/modificacionLey/creditos/modificacionLeyTrCreditosPasoTresController.js",

            #endregion

            #region modificacion adicion presupuesto preliminar                

                "~/src/app/formulario/ventanas/tramiteModificacionAdicion/tramiteModificacionAdicionServicio.js",
                "~/src/app/formulario/ventanas/tramiteModificacionAdicion/presupuestoPrel/tramiteModificacionAdicionPasoUnoController.js",
                "~/src/app/formulario/ventanas/tramiteModificacionAdicion/presupuestoPrel/tramiteModificacionAdicionPasoDosController.js",
                "~/src/app/formulario/ventanas/tramiteModificacionAdicion/presupuestoPrel/tramiteModificacionAdicionPasoTresController.js",
                "~/src/app/formulario/ventanas/tramiteModificacionAdicion/presupuestoPrel/tramiteModificacionAdicionPasoCuatroController.js",
                "~/src/app/formulario/ventanas/tramiteModificacionAdicion/Componentes/asociarProyectoAd/asociarProyectoAdController.js",
                "~/src/app/formulario/ventanas/tramiteModificacionAdicion/Componentes/aprobacionEntidadAd/aprobacionEntidadAdController.js",
                "~/src/app/formulario/ventanas/tramiteModificacionAdicion/Componentes/informacionPresupuestalAd/informacionPresupuestalAdController.js",
                "~/src/app/formulario/ventanas/tramiteModificacionAdicion/Componentes/informacionPresupuestalAd/informacionPresupuestalAdPasoTresController.js",
                "~/src/app/formulario/ventanas/tramiteModificacionAdicion/Componentes/aprobacionSupervisor/aproSuperAdController.js",

                "~/src/app/formulario/ventanas/comun/modificacionLey/modificacionLeyServicio.js",
                "~/src/app/formulario/ventanas/comun/modificacionLey/informacionPresupuestalMlaController.js",
                "~/src/app/formulario/ventanas/comun/modificacionLey/creditos/modificacionLeyCreditosController.js",
                "~/src/app/formulario/ventanas/comun/modificacionLey/creditos/modificacionLeyCreditosPasoTresController.js",

            #endregion

            #region modificacion adicion Analista

                "~/src/app/formulario/ventanas/tramiteModificacionAdicion/analista/tramiteModificacionAdicionAPasoTresController.js",
                "~/src/app/formulario/ventanas/tramiteModificacionAdicion/analista/tramiteModificacionAdicionAPasoCuatroController.js",
                "~/src/app/formulario/ventanas/tramiteModificacionAdicion/Componentes/aprobacionAnalista/aproAnalistaAdController.js",

                "~/src/app/formulario/ventanas/comun/modificacionLey/creditos/modificacionLeyCreditosPasoTresAController.js",

            #endregion

            #region Tramite de modificación ley Reduccion presupuesto preliminar

                "~/src/app/formulario/ventanas/tramiteModificacionLeyRed/tramiteModificacionLeyRedServicio.js",
                "~/src/app/formulario/ventanas/tramiteModificacionLeyRed/presupuestoPrel/tramiteModificacionLeyRedPasoUnoController.js",
                "~/src/app/formulario/ventanas/tramiteModificacionLeyRed/presupuestoPrel/tramiteModificacionLeyRedPasoDosController.js",
                "~/src/app/formulario/ventanas/tramiteModificacionLeyRed/presupuestoPrel/tramiteModificacionLeyRedPasoTresController.js",
                "~/src/app/formulario/ventanas/tramiteModificacionLeyRed/presupuestoPrel/tramiteModificacionLeyRedPasoCuatroController.js",
                "~/src/app/formulario/ventanas/tramiteModificacionLeyRed/Componentes/aprobacionTmlr/aprobacionTmlrController.js",
                "~/src/app/formulario/ventanas/tramiteModificacionLeyRed/Componentes/informacionPresupuestalTmlr/informacionPresupuestalTmlrController.js",
                "~/src/app/formulario/ventanas/tramiteModificacionLeyRed/Componentes/informacionPresupuestalTmlr/informacionPresupuestalTmlrPasoTresController.js",
                "~/src/app/formulario/ventanas/tramiteModificacionLeyRed/Componentes/proyectosTmlr/proyectosTmlrController.js",
                "~/src/app/formulario/ventanas/tramiteModificacionLeyRed/Componentes/proyectosTmlr/asociarProyecto/asociarProyectoTmlrController.js",
                "~/src/app/formulario/ventanas/tramiteModificacionLeyRed/Componentes/informacionPresupuestalTmlr/solicitudModificacion/solicitudModificacionTmlrController.js",
                "~/src/app/formulario/ventanas/tramiteModificacionLeyRed/Componentes/aprobacionTmlr/confirmacionAprobacion/confirmacionAprobacionTmlrController.js",
                "~/src/app/formulario/ventanas/tramiteModificacionLeyRed/Componentes/aprobacionSupervisorTmlr/aproSuperTmlrController.js",

            #endregion Tramite de modificación ley Reduccion presupuesto preliminar


            #region Tramite de modificación ley Reduccion analista
                "~/src/app/formulario/ventanas/tramiteModificacionLeyRed/analista/tramiteModificacionLeyRedPasoATresController.js",
                "~/src/app/formulario/ventanas/tramiteModificacionLeyRed/analista/tramiteModificacionLeyRedPasoACuatroController.js",
                "~/src/app/formulario/ventanas/tramiteModificacionLeyRed/componentes/aprobacionAnalistaTmlr/aproAnalistaTmlrController.js",



            #endregion Tramite de modificación ley Reduccion Analista


            #region Tramite de modificación ley Politicas transversales presupuesto preliminar
                "~/src/app/formulario/ventanas/tramiteModificacionLeyPoliticas/tramiteModificacionLeyPoliticasServicio.js",
                "~/src/app/formulario/ventanas/tramiteModificacionLeyPoliticas/presupuestoPrel/tramiteModificacionLeyPoliticasPasoUnoController.js",
                 "~/src/app/formulario/ventanas/tramiteModificacionLeyPoliticas/analista/tramiteModificacionLeyPoliticasPasoAUnoController.js",
                "~/src/app/formulario/ventanas/tramiteModificacionLeyPoliticas/Componentes/proyectosPoliticas/proyectosPoliticasController.js",
                "~/src/app/formulario/ventanas/tramiteModificacionLeyPoliticas/Componentes/proyectosPoliticas/asociarProyecto/asociarProyectoPoliticasController.js",
                "~/src/app/formulario/ventanas/tramiteModificacionLeyPoliticas/Componentes/politicasTransversalesMl/politicasTransversalesMlController.js",
                "~/src/app/formulario/ventanas/comun/modificacionLey/politicas/detallePolitica/detallePoliticaFormularioController.js",
                "~/src/app/formulario/ventanas/comun/modificacionLey/politicas/detallePolitica/transversales/ResumenFocalizacionMlFormularioController.js",
                "~/src/app/formulario/ventanas/comun/modificacionLey/politicas/detallePolitica/transversales/ResumenFocalizacionCategoria/ResumenFocalizacionCategoriaMlController.js",
                "~/src/app/formulario/ventanas/comun/modificacionLey/politicas/detallePolitica/transversales/FocalizacionCategoria/modalAgregarCategoriaPoliticaMlController.js",
                "~/src/app/formulario/ventanas/comun/modificacionLey/politicas/detallePolitica/transversales/ResumenFocalizacionCategoria/Modal/modalAgregarIndicadorMlController.js",
                "~/src/app/formulario/ventanas/comun/modificacionLey/politicas/detallePolitica/politica/detallePoliticasTransversalesMlController.js",
                "~/src/app/formulario/ventanas/comun/modificacionLey/politicas/detallePolitica/politica/politicasTransversalesMlFormulario.js",
                "~/src/app/formulario/ventanas/comun/modificacionLey/politicas/detallePolitica/politica/modal/modalAgregarPoliticaTransversalMlController.js",
                "~/src/app/formulario/ventanas/comun/modificacionLey/politicas/concepto/conceptoVictimasMlFormulario.js",
                "~/src/app/formulario/ventanas/comun/modificacionLey/politicas/aprobarConcepto/aprobarConceptoVictimasMlFormulario.js",
                "~/src/app/formulario/ventanas/comun/modificacionLey/politicas/infoPresupuestalPoliticasMlFormulario.js",
                "~/src/app/formulario/ventanas/comun/modificacionLey/politicas/valores/conceptoValoresMlFormulario.js",
                "~/src/app/formulario/ventanas/comun/modificacionLey/politicas/aprobarValores/aprobarConceptoValoresMlFormulario.js",
            #endregion Tramite de modificación ley Politicas transversales presupuesto preliminar

            #region modificacion  decreto traslado presupuesto preliminar

                "~/src/app/formulario/ventanas/tramiteModificacionDecretoTraslado/tramiteModificacionDTrasladoServicio.js",
                "~/src/app/formulario/ventanas/tramiteModificacionDecretoTraslado/presupuestoPrel/tramiteModificacionDTrasladoPasoUnoController.js",
                "~/src/app/formulario/ventanas/tramiteModificacionDecretoTraslado/presupuestoPrel/tramiteModificacionDTrasladoPasoDosController.js",
                "~/src/app/formulario/ventanas/tramiteModificacionDecretoTraslado/presupuestoPrel/tramiteModificacionDTrasladoPasoTresController.js",
                "~/src/app/formulario/ventanas/tramiteModificacionDecretoTraslado/presupuestoPrel/tramiteModificacionDTrasladoPasoCuatroController.js",
                "~/src/app/formulario/ventanas/tramiteModificacionDecretoTraslado/Componentes/asociarProyectoTr/asociarProyectoDTrController.js",
                "~/src/app/formulario/ventanas/tramiteModificacionDecretoTraslado/Componentes/aprobacionEntidadTr/aprobacionEntidadDTrController.js",
                "~/src/app/formulario/ventanas/tramiteModificacionDecretoTraslado/Componentes/informacionPresupuestalTr/informacionPresupuestalDTrController.js",
                "~/src/app/formulario/ventanas/tramiteModificacionDecretoTraslado/Componentes/aprobacionSupervisor/aproSuperDTrController.js",


            #endregion

            #region modificacion decreto traslado Analista

                "~/src/app/formulario/ventanas/tramiteModificacionDecretoTraslado/analista/tramiteModificacionDTrasladoAPasoTresController.js",
                "~/src/app/formulario/ventanas/tramiteModificacionDecretoTraslado/analista/tramiteModificacionDTrasladoAPasoCuatroController.js",
                "~/src/app/formulario/ventanas/tramiteModificacionDecretoTraslado/componentes/aprobacionAnalistaTr/aproAnalistaDTrController.js",

            #endregion


            #region SGP
                "~/src/app/formulario/ventanas/SGP/comun/transversalSGPServicio.js",
                "~/src/app/formulario/ventanas/SGP/gestionRecursosSGP/gestionRecursosSgpServicio.js",
                "~/src/app/formulario/ventanas/SGP/gestionRecursosSGP/gestionRecursosSgpController.js",
                "~/src/app/formulario/ventanas/SGP/gestionRecursosSGP/componentes/recursos/enviarProgramacion/enviarProgramacionSgpController.js",
                "~/src/app/formulario/ventanas/SGP/gestionRecursosSGP/componentes/recursos/enviarProgramacion/enviarProgramacionDetalle/enviarProgramaciondetSgpController.js",
                "~/src/app/formulario/ventanas/SGP/gestionRecursosSGP/componentes/recursos/resumenCostosSolicitado/resumenCostosSolicitadoSgpController.js",
                "~/src/app/formulario/ventanas/SGP/gestionRecursosSGP/componentes/datosGenerales/datosgeneralesSgpController.js",
                "~/src/app/formulario/ventanas/SGP/gestionRecursosSGP/componentes/datosGenerales/localizacion/localizacionSgpController.js",
                "~/src/app/formulario/ventanas/SGP/gestionRecursosSGP/componentes/recursos/recursosSgpController.js",
                "~/src/app/formulario/ventanas/SGP/gestionRecursosSGP/componentes/recursos/regionalizacionFuentes/regionalizacionFuentesSgpController.js",
                "~/src/app/formulario/ventanas/SGP/gestionRecursosSGP/componentes/focalizacion/focalizacionSgpController.js",
                "~/src/app/formulario/ventanas/SGP/gestionRecursosSGP/componentes/focalizacion/ResumenFocalizacion/FocalizacionCategoria/modalAgregarCategoriaPoliticaSgpController.js",
                "~/src/app/formulario/ventanas/SGP/gestionRecursosSGP/componentes/focalizacion/focalizacionAjustesSgpController.js",
                "~/src/app/formulario/ventanas/SGP/gestionRecursosSGP/componentes/focalizacion/PoliticasTransversales/politicasTransversalesSgpController.js",
                "~/src/app/formulario/ventanas/SGP/gestionRecursosSGP/componentes/focalizacion/ResumenFocalizacion/ResumenFocalizacionCategoria/ResumenFocalizacionCategoriaSgpController.js",
                "~/src/app/formulario/ventanas/SGP/gestionRecursosSGP/componentes/focalizacion/ResumenFocalizacion/ResumenFocalizacionSgpController.js",
                "~/src/app/formulario/ventanas/SGP/gestionRecursosSGP/componentes/recursos/fuentes/modalAgregarFuenteSgpController.js",
                "~/src/app/formulario/ventanas/SGP/gestionRecursosSGP/componentes/recursos/fuentes/modalAgregarDatosAdicionalesSgpController.js",
                "~/src/app/formulario/ventanas/SGP/gestionRecursosSGP/componentes/focalizacion/ResumenFocalizacion/ResumenFocalizacionCategoria/Modal/modalAgregarIndicadorSgpController.js",
                "~/src/app/formulario/ventanas/SGP/gestionRecursosSGP/componentes/focalizacion/categoriaProductosPoliticaSgp/categoriaProductosPoliticaSgpController.js",
                "~/src/app/formulario/ventanas/SGP/gestionRecursosSGP/componentes/focalizacion/indicadoresPoliticaSgp/indicadoresPoliticaSgpController.js",
                "~/src/app/formulario/ventanas/SGP/gestionRecursosSGP/componentes/focalizacion/politicaCrucePoliticaSgp/politicaCrucePoliticaSgpController.js",
                "~/src/app/formulario/ventanas/SGP/gestionRecursosSGP/componentes/focalizacion/focalizacionAjustesSGPServicio.js",
                "~/src/app/formulario/ventanas/SGP/gestionRecursosSGP/componentes/focalizacion/CrucePoliticasTransversales/crucePoliticasTransversalesSgpController.js",
                "~/src/app/formulario/ventanas/SGP/gestionRecursosSGP/componentes/focalizacion/Resumen/ResumenSgpController.js",
                "~/src/app/formulario/ventanas/SGP/viabilidadSGP/previos/regionalizacion/regionalizacionSGPController.js",
                "~/src/app/formulario/ventanas/SGP/viabilidadSGP/previos/regionalizacion/regionalizacionSGP/regionalizacionRecSGPController.js",
                "~/src/app/formulario/ventanas/SGP/viabilidadSGP/previosSGPController.js",

                "~/src/app/formulario/ventanas/SGP/viabilidadSGP/previos/regionalizacion/regionalizacionSGPController.js",
                "~/src/app/formulario/ventanas/SGP/viabilidadSGP/previos/regionalizacion/regionalizacionSGP/regionalizacionRecSGPController.js",
                "~/src/app/formulario/ventanas/SGP/viabilidadSGP/previosSGPController.js",
                "~/src/app/formulario/ventanas/SGP/viabilidadSGP/datosGenerales/agregarSectores/agregarSectoresSgpController.js",
                "~/src/app/formulario/ventanas/SGP/viabilidadSGP/datosGenerales/requisitosDatosGeneralesSgpController.js",
                "~/src/app/formulario/ventanas/SGP/viabilidadSGP/requisitosSgpServicio.js",
                "~/src/app/formulario/ventanas/SGP/viabilidadSGP/requisitosSgpController.js",
                "~/src/app/formulario/ventanas/SGP/viabilidadSGP/requisitos/verificacion/verificacionSgpController.js",
                "~/src/app/formulario/ventanas/SGP/viabilidadSGP/requisitos/verificacion/generales/generalesSgpController.js",
                "~/src/app/formulario/ventanas/SGP/viabilidadSGP/requisitos/soportes/alojarDocumentosRequisitosSgpController.js",
                "~/src/app/formulario/ventanas/SGP/comun/documentoSoporte/archivosFormularioSgpController.js",
                "~/src/app/formulario/ventanas/SGP/comun/usuariosInvolucrados/usuariosInvolucradosFormularioSgpController.js",
                "~/src/app/formulario/ventanas/SGP/comun/usuariosInvolucrados/usuariosInvolucradosSgpServicio.js",
                "~/src/app/formulario/ventanas/SGP/comun/usuariosInvolucrados/modalAgregarInvolucradosSgpController.js",
                "~/src/app/formulario/ventanas/SGP/viabilidadSGP/viabilidad/informacionGeneral/usuariosInvolucrados/usuariosInvolucradosViabilidadSgpController.js",
                "~/src/app/formulario/ventanas/SGP/viabilidadSGP/viabilidad/informacionGeneral/viabilidadSectorial/viabilidadSectorialSgpController.js",
                "~/src/app/formulario/ventanas/SGP/comun/cuestionarioSectorialSgp/cuestionarioViabilidadSectorialSgpController.js",
                "~/src/app/formulario/ventanas/SGP/viabilidadSGP/viabilidad/informacionGeneral/informacionGeneralViabilidadSgpController.js",
                "~/src/app/formulario/ventanas/SGP/viabilidadSGP/viabilidadSgpController.js",
                "~/src/app/formulario/ventanas/SGP/viabilidadSGP/viabilidadSgpServicio.js",
                "~/src/app/formulario/ventanas/SGP/viabilidadSGP/viabilidad/viabilidad/viabilidadViabilidadSgpController.js",
                "~/src/app/formulario/ventanas/SGP/viabilidadSGP/viabilidad/viabilidad/cuestionarioViabilidad/cuestionarioViabilidadSgpController.js",
                "~/src/app/formulario/ventanas/SGP/viabilidadSGP/viabilidad/soportes/alojarDocumentosViabilidadSgpController.js",
                "~/src/app/formulario/ventanas/SGP/viabilidadSGP/firmaEmisionSgpController.js",
                "~/src/app/formulario/ventanas/SGP/viabilidadSGP/firmaEmisionSgpServicio.js",
                "~/src/app/formulario/ventanas/SGP/viabilidadSGP/firmaEmision/procesoFirma/procesoFirmaViabilidadSgpController.js",
                "~/src/app/formulario/ventanas/SGP/viabilidadSGP/firmaEmision/procesoFirma/procesoFirmaSgp/procesoFirmaViabilidadSgpSgpController.js",
                "~/src/app/formulario/ventanas/SGP/comun/firma/firmaFormularioSgpController.js",
                "~/src/app/formulario/ventanas/SGP/comun/firma/firmaFormularioSgpServicio.js",
                "~/src/app/formulario/ventanas/SGP/gestionRecursosSGP/componentes/recursos/fuentes/fuentesSgpController.js",
                "~/src/app/formulario/ventanas/SGP/gestionRecursosSGP/componentes/usuariosInvolucradosGestionRecursos/usuariosInvolucradosGestionRecursosSgpController.js",

                "~/src/app/formulario/ventanas/SGP/gestionRecursosSGP/componentes/firmaEmisionGestion/firmaGestionRecursosSgpController.js",
                "~/src/app/formulario/ventanas/SGP/gestionRecursosSGP/componentes/firmaEmisionGestion/firmaGestionRecursosSgpServicio.js",
                "~/src/app/formulario/ventanas/SGP/gestionRecursosSGP/componentes/firmaEmisionGestion/procesoFirma/procesoFirmaGestionRecursosSgpController.js",
                "~/src/app/formulario/ventanas/SGP/gestionRecursosSGP/componentes/firmaEmisionGestion/procesoFirma/procesoFirmaGestion/procesoFirmaGestionRecursosFirmaSgpController.js",
                "~/src/app/formulario/ventanas/SGP/comun/usuariosInvolucrados/usuariosInvolucradosFormularioGestionRecSgpController.js",
                "~/src/app/formulario/ventanas/SGP/comun/usuariosInvolucrados/modalAgregarInvolucradosGestionRecursosSgpController.js",
                "~/src/app/formulario/ventanas/SGP/gestionRecursosSGP/componentes/informacionGeneral/informacionGeneralUsuariosInvolucradosSgpController.js",
                "~/src/app/formulario/ventanas/SGP/comun/firma/firmaFormularioGestionRecursosSgpController.js",

            #region SGP Ajustes Sin tramite
                "~/src/app/formulario/ventanas/SGP/ajustesProyectoSGP/ajustesProyectoSinTramiteSgpController.js",
                "~/src/app/formulario/ventanas/SGP/ajustesProyectoSGP/ajustesProyectoSinTramiteSgpServicio.js",
                "~/src/app/formulario/ventanas/SGP/ajustesProyectoSGP/componentes/datosgenerales/datosgeneralesSinTramiteSgpController.js",
                "~/src/app/formulario/ventanas/SGP/ajustesProyectoSGP/componentes/datosgenerales/datosgeneralesSinTramiteSgpServicio.js",
                "~/src/app/formulario/ventanas/SGP/ajustesProyectoSGP/componentes/datosgenerales/conpes/conpesSgpController.js",
                "~/src/app/formulario/ventanas/SGP/ajustesProyectoSGP/componentes/datosgenerales/conpes/conpesSgpServicio.js",
                "~/src/app/formulario/ventanas/SGP/ajustesProyectoSGP/componentes/datosgenerales/beneficiarios/beneficiariosTotalesSinTramiteSgpController.js",
                "~/src/app/formulario/ventanas/SGP/ajustesProyectoSGP/componentes/datosgenerales/beneficiarios/beneficiariosSinTramiteSgpServicio.js",
                "~/src/app/formulario/ventanas/SGP/ajustesProyectoSGP/componentes/datosgenerales/horizonte/horizonteSinTramiteSgpController.js",
                "~/src/app/formulario/ventanas/SGP/ajustesProyectoSGP/componentes/datosgenerales/horizonte/horizonteSinTramiteSgpServicio.js",
                "~/src/app/formulario/ventanas/SGP/ajustesProyectoSGP/componentes/datosgenerales/indicadores/indicadoresSinTramiteSgpController.js",
                "~/src/app/formulario/ventanas/SGP/ajustesProyectoSGP/componentes/datosgenerales/indicadores/indicadoresSinTramiteSgpServicio.js",
                "~/src/app/formulario/ventanas/SGP/ajustesProyectoSGP/componentes/datosgenerales/indicadores/modal/agregarIndicadorSecModalSinTramiteSgpController.js",
                "~/src/app/formulario/ventanas/SGP/ajustesProyectoSGP/componentes/datosgenerales/indicadores/modal/agregarIndicadorSecModalSinTramiteSgpServicio.js",
                "~/src/app/formulario/ventanas/SGP/ajustesProyectoSGP/componentes/datosgenerales/indicadores/modal/objetivosIndicadorModalSinTramiteSgpController.js",
                "~/src/app/formulario/ventanas/SGP/ajustesProyectoSGP/componentes/datosgenerales/localizacionJustificacion/localizacionJustificacionSinTramiteSgpController.js",
                "~/src/app/formulario/ventanas/SGP/ajustesProyectoSGP/componentes/datosgenerales/localizacionJustificacion/localizacionJustificacionSinTramiteSgpServicio.js",
                "~/src/app/formulario/ventanas/SGP/ajustesProyectoSGP/componentes/datosgenerales/localizacionJustificacion/modal/modalAgregarLocalizacionSinTramiteSgpController.js",
                "~/src/app/formulario/ventanas/SGP/ajustesProyectoSGP/componentes/focalizacion/PoliticasTransversales/politicasTransversalesSinTramiteSgpController.js",
                "~/src/app/formulario/ventanas/SGP/ajustesProyectoSGP/componentes/focalizacion/PoliticasTransversales/modalAgregarPoliticaTransversalAjustesSinTramiteSgpController.js",
                "~/src/app/formulario/ventanas/SGP/ajustesProyectoSGP/componentes/focalizacion/PoliticasTransversales/politicasTransversalesSinTramiteSgpServicio.js",
                "~/src/app/formulario/ventanas/SGP/ajustesProyectoSGP/componentes/focalizacion/Resumen/ResumenSinTramiteSgpController.js",
                "~/src/app/formulario/ventanas/SGP/ajustesProyectoSGP/componentes/focalizacion/ResumenFocalizacion/ResumenFocalizacionCategoria/ResumenFocalizacionCategoriaSinTramiteSgpController.js",
                "~/src/app/formulario/ventanas/SGP/ajustesProyectoSGP/componentes/focalizacion/ResumenFocalizacion/ResumenFocalizacionSinTramiteSgpController.js",
                "~/src/app/formulario/ventanas/SGP/ajustesProyectoSGP/componentes/focalizacion/ResumenFocalizacion/ResumenFocalizacionCategoria/Modal/modalAgregarIndicadorSinTramiteSgp.js",
                "~/src/app/formulario/ventanas/SGP/ajustesProyectoSGP/componentes/focalizacion/ResumenFocalizacion/FocalizacionCategoria/modalAgregarCategoriaPoliticaSinTramiteSgpController.js",
                "~/src/app/formulario/ventanas/SGP/ajustesProyectoSGP/componentes/focalizacion/ResumenFocalizacion/ResumenFocalizacionCategoria/Modal/modalAgregarIndicadorSinTramiteSgpController.js",
                "~/src/app/formulario/ventanas/SGP/ajustesProyectoSGP/componentes/focalizacion/focalizacionAjustesSinTramiteSgpController.js",
                "~/src/app/formulario/ventanas/SGP/ajustesProyectoSGP/componentes/focalizacion/focalizacionAjustesSinTramiteSgpServicio.js",
                "~/src/app/formulario/ventanas/SGP/ajustesProyectoSGP/componentes/focalizacion/CrucePoliticasTransversales/crucePoliticasSinTramiteSgpController.js",
                "~/src/app/formulario/ventanas/SGP/ajustesProyectoSGP/componentes/recursosSGP/recursosSinTramiteSgpController.js",
                "~/src/app/formulario/ventanas/SGP/ajustesProyectoSGP/componentes/recursosSGP/recursosSinTramiteSgpServicio.js",
                "~/src/app/formulario/ventanas/SGP/ajustesProyectoSGP/componentes/recursosSGP/recursosAjustesSinTramiteSgpController.js",
                "~/src/app/formulario/ventanas/SGP/ajustesProyectoSGP/componentes/recursosSGP/recursosAjustesSinTramiteSgpServicio.js",
                "~/src/app/formulario/ventanas/SGP/ajustesProyectoSGP/componentes/recursosSGP/regionalizacion/regionalizacionSinTramiteSgpController.js",
                "~/src/app/formulario/ventanas/SGP/ajustesProyectoSGP/componentes/recursosSGP/Fuentes/totalesFuentes/resumenFuentesSinTramiteSgpController.js",
                "~/src/app/formulario/ventanas/SGP/ajustesProyectoSGP/componentes/recursosSGP/Fuentes/totalesFuentes/resumenFuentesSinTramiteSgpServicio.js",
                "~/src/app/formulario/ventanas/SGP/ajustesProyectoSGP/componentes/recursosSGP/Fuentes/ResumenCostosAjustes/costosFuentesAjustesSinTramiteSgpController.js",
                "~/src/app/formulario/ventanas/SGP/ajustesProyectoSGP/componentes/recursosSGP/Fuentes/ResumenCostosAjustes/costosFuentesAjustesSinTramiteSgpServicio.js",
                "~/src/app/formulario/ventanas/SGP/ajustesProyectoSGP/componentes/recursosSGP/Fuentes/ajustesFuentes/ajustesFuentesSinTramiteSgp.js",
                "~/src/app/formulario/ventanas/SGP/ajustesProyectoSGP/componentes/recursosSGP/Fuentes/ajustesFuentes/ajustesModalAgregarDatosAdicionalesSinTramiteSgpController.js",
                "~/src/app/formulario/ventanas/SGP/ajustesProyectoSGP/componentes/recursosSGP/Fuentes/ajustesFuentes/ajustesModalAgregarFuenteSinTramiteSgpController.js",
                "~/src/app/formulario/ventanas/SGP/ajustesProyectoSGP/componentes/recursosSGP/costoActividades/costoActividadesSinTramiteSgpController.js",
                "~/src/app/formulario/ventanas/SGP/ajustesProyectoSGP/componentes/recursosSGP/costoActividades/costoActividadesSinTramiteSgpServicio.js",
                "~/src/app/formulario/ventanas/SGP/ajustesProyectoSGP/componentes/recursosSGP/costoActividades/ResumenCostosFuentesAjustes/resumenCostosFuentesAjustesSinTramiteSgpController.js",
                "~/src/app/formulario/ventanas/SGP/ajustesProyectoSGP/componentes/recursosSGP/costoActividades/modal/agregarEntregableModalSinTramiteSgpController.js",
                "~/src/app/formulario/ventanas/SGP/comun/documentoSoporte/documentosSoporteViSinTramiteSgpController.js",
                "~/src/app/formulario/ventanas/SGP/comun/justificacionCambios/justificacionCambiosSinTramiteSgpController.js",
                "~/src/app/formulario/ventanas/SGP/comun/justificacionCambios/justificacionCambiosSinTramiteSgpServicio.js",
                "~/src/app/formulario/ventanas/SGP/comun/justificacionCambios/componentes/datosgenerales/justificacionBeneficiarios/justificacionBeneficiariosSinTramiteSgpController.js",
                "~/src/app/formulario/ventanas/SGP/comun/justificacionCambios/componentes/datosgenerales/justificacionBeneficiarios/justificacionBeneficiariosSinTramiteSgpServicio.js",
                "~/src/app/formulario/ventanas/SGP/comun/justificacionCambios/componentes/datosgenerales/justificacionHorizonte/justificacionHorizonteSinTramiteSgpController.js",
                "~/src/app/formulario/ventanas/SGP/comun/justificacionCambios/componentes/datosgenerales/justificacionHorizonte/justificacionHorizonteSinTramiteSgpServicio.js",
                "~/src/app/formulario/ventanas/SGP/comun/justificacionCambios/componentes/datosgenerales/justificacionIndicadores/justificacionIndicadoresSinTramiteSgpController.js",
                "~/src/app/formulario/ventanas/SGP/comun/justificacionCambios/componentes/datosgenerales/justificacionIndicadores/justificacionIndicadoresSinTramiteSgpServicio.js",
                "~/src/app/formulario/ventanas/SGP/comun/justificacionCambios/componentes/datosgenerales/justificacionLocalizacion/justificacionLocalizacionSinTramiteSgpController.js",
                "~/src/app/formulario/ventanas/SGP/comun/justificacionCambios/componentes/datosgenerales/justificacionLocalizacion/justificacionLocalizacionSinTramiteSgpServicio.js",
                "~/src/app/formulario/ventanas/SGP/comun/justificacionCambios/componentes/focalizacion/categoriaspoliticastransversales/focalizacioncategoriaspolitSinTramiteSgpController.js",
                "~/src/app/formulario/ventanas/SGP/comun/justificacionCambios/componentes/focalizacion/categoriaspoliticastransversales/focalizacioncategoriaspolitSinTramiteSgpServicio.js",
                "~/src/app/formulario/ventanas/SGP/comun/justificacionCambios/componentes/recursosSGP/costoactividades/justificacioncostoactividadesSinTramiteSgpController.js",
                "~/src/app/formulario/ventanas/SGP/comun/justificacionCambios/componentes/recursosSGP/costoactividades/justificacioncostoactividadesSinTramiteSgpServicio.js",
                "~/src/app/formulario/ventanas/SGP/comun/justificacionCambios/componentes/recursosSGP/fuentes/recursosfuentesdefinancSinTramiteSgpController.js",
                "~/src/app/formulario/ventanas/SGP/comun/justificacionCambios/componentes/recursosSGP/fuentes/recursosfuentesdefinancSinTramiteSgpServicio.js",
                "~/src/app/formulario/ventanas/SGP/comun/justificacionCambios/componentes/recursosSGP/fuentes/Componentes/resumenRecursosfuentesdefinancSinTramiteSgpController.js",
                "~/src/app/formulario/ventanas/SGP/comun/justificacionCambios/componentes/recursosSGP/regionalizacion/recursosregionalizacionSinTramiteSgpController.js",
                "~/src/app/formulario/ventanas/SGP/comun/justificacionCambios/componentes/recursosSGP/regionalizacion/recursosregionalizacionSinTramiteSgpServicio.js",
                "~/src/app/formulario/ventanas/SGP/comun/documentoSoporte/documentosSoporteViSinTramiteSgpController.js",

                "~/src/app/formulario/ventanas/SGP/ajustesProyectoSGP/requisitosSinTramiteSgpController.js",
                "~/src/app/formulario/ventanas/SGP/ajustesProyectoSGP/requisitosSinTramiteSgpServicio.js",
                "~/src/app/formulario/ventanas/SGP/ajustesProyectoSGP/requisitosAjustesSinTramite/verificacion/verificacionAjustesSinTramiteSgpController.js",
                "~/src/app/formulario/ventanas/SGP/ajustesProyectoSGP/requisitosAjustesSinTramite/verificacion/generales/generalesAjustesSinTramiteSgpController.js",
                "~/src/app/formulario/ventanas/SGP/ajustesProyectoSGP/requisitosAjustesSinTramite/verificacion/soportes/alojarDocumentosReqAjustesSinTramiteSgpController.js",
                "~/src/app/formulario/ventanas/SGP/ajustesProyectoSGP/viabilidadAjustesSinTramite/viabilidad/cuestionarioViabilidad/cuestionarioViabilidadSinTramiteSgpController.js",
                "~/src/app/formulario/ventanas/SGP/ajustesProyectoSGP/viabilidadAjustesSinTramite/viabilidad/viabilidadDetalleSinTramiteSgpController.js",
                "~/src/app/formulario/ventanas/SGP/ajustesProyectoSGP/viabilidadSinTramiteSgpController.js",
                "~/src/app/formulario/ventanas/SGP/ajustesProyectoSGP/viabilidadSinTramiteSgpServicio.js",
                "~/src/app/formulario/ventanas/SGP/ajustesProyectoSGP/viabilidadAjustesSinTramite/informacionGeneral/usuariosInvolucrados/usuariosInvolucradosSinTramiteSgpController.js",
                "~/src/app/formulario/ventanas/SGP/ajustesProyectoSGP/viabilidadAjustesSinTramite/informacionGeneral/viabilidadSectorial/viabilidadSectorialSinTramiteSgpController.js",
                "~/src/app/formulario/ventanas/SGP/ajustesProyectoSGP/viabilidadAjustesSinTramite/informacionGeneral/informacionGeneralViabilidadSinTramiteSgpController.js",
                "~/src/app/formulario/ventanas/SGP/ajustesProyectoSGP/viabilidadAjustesSinTramite/soportes/alojarDocumentosViabilidadSinTramiteSgpController.js",
                "~/src/app/formulario/ventanas/SGP/ajustesProyectoSGP/firmaEmisionSinTramiteSgpController.js",
                "~/src/app/formulario/ventanas/SGP/ajustesProyectoSGP/firmaEmisionSinTramiteSgpServicio.js",
                "~/src/app/formulario/ventanas/SGP/ajustesProyectoSGP/firmaEmisionAjustesSinTramite/procesoFirmaAjustesSinTramiteSgpController.js",
                "~/src/app/formulario/ventanas/SGP/ajustesProyectoSGP/firmaEmisionAjustesSinTramite/firmar/firmarAjustesSinTramiteSgpController.js",
                "~/src/app/formulario/ventanas/SGP/comun/usuariosInvolucrados/usuariosInvolucradosFormularioSinTramiteSgpController.js",
                "~/src/app/formulario/ventanas/SGP/comun/usuariosInvolucrados/modalAgregarInvolucradosSinTramiteSgpController.js",
                "~/src/app/formulario/ventanas/SGP/comun/firma/firmaFormularioAjusteSinTramitesSgpController.js",
                "~/src/app/formulario/ventanas/SGP/comun/cuestionarioSectorialSgp/cuestionarioViabilidadSectorialSinTramiteSgpController.js",

            #endregion

            #region tramite traslado SGP

                "~/src/app/formulario/ventanas/SGP/tramites/traslado/tramiteTrasladoSgpController.js",
                "~/src/app/formulario/ventanas/SGP/tramites/traslado/tramiteTrasladoSgpServicio.js",
                "~/src/app/formulario/ventanas/SGP/tramites/traslado/componentes/asociarProyectoTrasladoSgp/asociarProyectoTrasladoSgpController.js",
                "~/src/app/formulario/ventanas/SGP/tramites/tramiteSGPSevicio.js",
                "~/src/app/formulario/ventanas/comun/asociarMuchosProyectos/asociarMuchosProyectosSgpFormulario.js",
                "~/src/app/formulario/ventanas/comun/asociarMuchosProyectos/detalleMuchosProyectos/detalleMuchosProyectosSgpFormulario.js",
                "~/src/app/formulario/ventanas/SGP/tramites/traslado/componentes/informacionPresupuestalTrasladoSgp/infoPresupuestalTrasladoSgpController.js",
                "~/src/app/formulario/ventanas/SGP/tramites/adicion/componentes/informacionPresupuestalAdicionSgp/infoPresupuestalAdicionSgpController.js",
                "~/src/app/formulario/ventanas/comun/informacionPresupuestalAprobacion/informacionPresupuestalAproFormulario.js",
                "~/src/app/formulario/ventanas/comun/informacionPresupuestalSgp/informacionPresupuestalSgpFormulario.js",

                "~/src/app/formulario/ventanas/comun/certificadoDisponibilidadPresupuestalSgp/componentes/certificadoDisponibilidadPresupuestalSgpFormulario.js",
                "~/src/app/formulario/ventanas/comun/certificadoDisponibilidadPresupuestalSgp/componentes/agregarCdpProyectoSgp/agregarCdpProyectoSgp.js",
                "~/src/app/formulario/ventanas/comun/certificadoDisponibilidadPresupuestalSgp/componentes/agregarCdpProyectoSgpModal/agregarCdpProyectoSgpModal.js",
                "~/src/app/formulario/ventanas/comun/certificadoDisponibilidadPresupuestalSgp/componentes/cargaMasivaCdpSgpService.js",

                "~/src/app/formulario/ventanas/SGP/comun/firma/firmaFormularioAjusteSinTramitesSgpController.js",
                "~/src/app/formulario/ventanas/SGP/comun/usuariosInvolucrados/usuariosInvolucradosTramiteTrasladoSgpController.js",
                "~/src/app/formulario/ventanas/SGP/comun/usuariosInvolucrados/modalAgregarInvolucradosTramiteTrasladoSgpController.js",
                "~/src/app/formulario/ventanas/SGP/tramites/traslado/componentes/documentosTrasladoSgp/documentosSoporteTrasladoSgpController.js",
                "~/src/app/formulario/ventanas/SGP/tramites/traslado/componentes/firmaTrasladoSgp/firmar/firmarTramiteTrasladoSgpController.js",
                "~/src/app/formulario/ventanas/SGP/tramites/traslado/componentes/firmaTrasladoSgp/procesoFirmaTramiteTrasladoSgpController.js",
                "~/src/app/formulario/ventanas/SGP/tramites/traslado/componentes/informacionUsuarios/informacionUsuariosInvolucradosTramiteSgpController.js",
                "~/src/app/formulario/ventanas/SGP/tramites/traslado/componentes/justificacionTrasladoSgp/justificacionTrasladoSgpController.js",
                "~/src/app/formulario/ventanas/SGP/tramites/traslado/componentes/usuariosInvolucradosTraslado/usuariosInvolucradosTrasladoSgpController.js",
                "~/src/app/formulario/ventanas/SGP/tramites/traslado/firmaEmisionTramiteTrasladoSgpController.js",
                "~/src/app/formulario/ventanas/SGP/tramites/traslado/firmaEmisionTramiteTrasladoSgpServicio.js",
                "~/src/app/formulario/ventanas/SGP/tramites/traslado/usuariosTramiteTrasladoSgpController.js",
                "~/src/app/formulario/ventanas/SGP/tramites/traslado/usuariosTramiteTrasladoSgpServicio.js",
                "~/src/app/formulario/ventanas/comun/asociarMuchosProyectos/proyectosAsociadosSgpFormulario.js",
                "~/src/app/formulario/ventanas/comun/asociarMuchosProyectos/asociarProyectosSgpFormulario.js",
                "~/src/app/formulario/ventanas/comun/asociarMuchosProyectos/detalleMuchosProyectos/detalleAsociarProyectosSgpFormulario.js",
                "~/src/app/formulario/ventanas/comun/asociarMuchosProyectos/detalleMuchosProyectos/detalleProyectosAsociadosSgpFormulario.js",
                "~/src/app/formulario/ventanas/SGP/tramites/traslado/componentes/aprobacionEntidadSgp/aprobacionTramiteEntidadSgpController.js",
                "~/src/app/formulario/ventanas/SGP/comun/aprobacionSgp/aprobacionTramiteSgpController.js",
                "~/src/app/formulario/ventanas/SGP/comun/firma/firmaFormularioTramiteTrasladoSgpController.js",
                "~/src/app/formulario/ventanas/SGP/tramites/adicion/firmaEmisionTramiteAdicionSgpController.js",
                "~/src/app/formulario/ventanas/SGP/tramites/adicion/firmaEmisionTramiteAdicionSgpServicio.js",
                "~/src/app/formulario/ventanas/SGP/tramites/adicion/usuariosTramiteAdicionSgpController.js",
                "~/src/app/formulario/ventanas/SGP/tramites/adicion/usuariosTramiteAdicionSgpServicio.js",
                "~/src/app/formulario/ventanas/SGP/tramites/adicion/tramiteAdicionSgpController.js",
                "~/src/app/formulario/ventanas/SGP/tramites/adicion/tramiteAdicionSgpServicio.js",
                "~/src/app/formulario/ventanas/SGP/tramites/adicion/componentes/asociarProyectoAdicionSgp/datosAdicion/datosAdicionSgpController.js",
                "~/src/app/formulario/ventanas/SGP/tramites/adicion/componentes/asociarProyectoAdicionSgp/datosAdicion/datosAdicionSgpServicio.js",
                "~/src/app/formulario/ventanas/SGP/tramites/adicion/componentes/asociarProyectoAdicionSgp/datosAdicion/modalDatosAdicionSgpController.js",
                "~/src/app/formulario/ventanas/SGP/tramites/adicion/componentes/asociarProyectoAdicionSgp/asociarProyectoAdicionSgpController.js",
                "~/src/app/formulario/ventanas/SGP/tramites/adicion/componentes/justificacionAdicionSgp/justificacionAdicionSgpController.js",
                "~/src/app/formulario/ventanas/SGP/tramites/adicion/componentes/firmaAdicionSgp/procesoFirmaTramiteAdicionSgpController.js",
                "~/src/app/formulario/ventanas/SGP/tramites/adicion/componentes/firmaAdicionSgp/firmar/firmarTramiteAdicionSgpController.js",
                "~/src/app/formulario/ventanas/SGP/tramites/adicion/componentes/informacionUsuariosSgp/informacionUsuariosInvolucradosAdicionSgpController.js",
                "~/src/app/formulario/ventanas/SGP/tramites/adicion/componentes/usuariosInvolucradosAdicionSgp/usuariosInvolucradosAdicionSgpController.js",
                "~/src/app/formulario/ventanas/SGP/tramites/adicion/componentes/documentosAdicionSgp/documentosSoporteAdicionSgpController.js",
                "~/src/app/formulario/ventanas/SGP/comun/firma/firmaFormularioTramiteAdicionSgpController.js",
                "~/src/app/formulario/ventanas/SGP/comun/justificacionTramites/justificacionTramiteSgpController.js",
                "~/src/app/formulario/ventanas/SGP/comun/usuariosInvolucrados/usuariosInvolucradosTramiteAdicionSgpController.js",
                "~/src/app/formulario/ventanas/SGP/comun/usuariosInvolucrados/modalAgregarInvolucradosTramiteAdicionSgpController.js",


            #endregion

            #region ajustes con tramite SGP
                "~/src/app/formulario/ventanas/SGP/ajustesConTramiteProyectoSGP/ajustesProyectoConTramiteSgpController.js",
                "~/src/app/formulario/ventanas/SGP/ajustesConTramiteProyectoSGP/ajustesProyectoConTramiteSgpServicio.js",
                "~/src/app/formulario/ventanas/SGP/ajustesConTramiteProyectoSGP/firmaEmisionConTramiteSgpController.js",
                "~/src/app/formulario/ventanas/SGP/ajustesConTramiteProyectoSGP/firmaEmisionConTramiteSgpServicio.js",
                "~/src/app/formulario/ventanas/SGP/ajustesConTramiteProyectoSGP/requisitosConTramiteSgpController.js",
                "~/src/app/formulario/ventanas/SGP/ajustesConTramiteProyectoSGP/requisitosConTramiteSgpServicio.js",
                "~/src/app/formulario/ventanas/SGP/ajustesConTramiteProyectoSGP/viabilidadConTramiteSgpController.js",
                "~/src/app/formulario/ventanas/SGP/ajustesConTramiteProyectoSGP/viabilidadConTramiteSgpServicio.js",

            #endregion

            #endregion

            #region reportesPIIP

                "~/src/app/reportesPIIP/consolaReportesController.js",
                "~/src/app/reportesPIIP/servicioConsolaReportes.js",
                "~/src/app/reportesPIIP/modalFiltrosController.js"

            #endregion



                ));
            bundles.Add(new ScriptBundle("~/bundles/schemaform").Include(
                "~/Scripts/schema-form/tv4.js",
                "~/Scripts/schema-form/ObjectPath.js",
                "~/Scripts/pickadate/lib/picker.js",
                "~/Scripts/pickadate/lib/picker.date.js",
                "~/Scripts/pickadate/translations/nl_NL.js",
                "~/Scripts/schema-form/schema-form.js",
                "~/Scripts/schema-form/bootstrap-decorator.min.js",
                "~/Scripts/schema-form/generalField.js",
                "~/Scripts/schema-form/angular-schema-form-dynamic-select.js",
                "~/Scripts/angular-schema-form-datepicker/bootstrap-datepicker.min.js",
                "~/Scripts/schema-form/angular-schema-form-grid.js",
                "~/Scripts/schema-form/angular-schema-form-accordion.js",
                "~/Scripts/schema-form/angular-schema-form-title.js",
                "~/Scripts/schema-form/angular-schema-form-fecha.js",
                "~/Scripts/schema-form/angular-schema-form-url.js",
                "~/Scripts/schema-form/angular-schema-form-lista-autocompletable.js",
                "~/Scripts/schema-form/angular-schema-form-lista-multiple.js",
                "~/Scripts/schema-form/angular-schema-form-lista-seleccion-multiple.js",
                "~/Scripts/schema-form/angular-schema-form-barra-progreso.js",
                "~/Scripts/schema-form/angular-schema-form-dropdownlist.js",
                "~/Scripts/schema-form/angular-schema-form-numerico.js",
                "~/Scripts/schema-form/angular-schema-form-texto.js",
                "~/Scripts/schema-form/angular-schema-form-textoLargo.js",
                "~/Scripts/schema-form/angular-schema-form-number.js",
                "~/Scripts/schema-form/angular-schema-form-texto.js",
                "~/Scripts/schema-form/angular-schema-form-seleccion-unica.js",
                "~/Scripts/schema-form/angular-schema-form-input-accion.js",
                "~/Scripts/schema-form/angular-schema-form-boton.js",
                "~/Scripts/schema-form/angular-schema-form-boton-requerido.js",
                "~/Scripts/schema-form/angular-schema-form-archivo.js",
                "~/Scripts/schema-form/angular-schema-form-label.js",
                "~/Scripts/schema-form/angular-schema-form-espacioBlanco.js",
                "~/Scripts/schema-form/angular-schema-form-wbs.js",
                "~/Scripts/schema-form/angular-schema-form-power-bi.js",
                "~/Scripts/schema-form/directives/modales/modalEvento.js",

                "~/src/app/formulario/ventanas/comun/paginador/paginadorFormulario.js"

            ));

            BundleTable.EnableOptimizations = false;

        }
    }
}