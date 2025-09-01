namespace DNP.Backbone.Web.UI
{
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


            #endregion

                "~/src/app/panelPrincial/controladorPanelPrincipal.js",
                "~/src/app/panelPrincial/controladorPanelProyectosPorTramite.js",
                "~/src/app/panelPrincial/componentes/inbox/controladorConsolaProcesosDefault.js",
                "~/src/app/panelPrincial/componentes/inbox/controladorInbox.js",
                "~/src/app/panelPrincial/componentes/inbox/controladorTramites.js",
                "~/src/app/panelPrincial/componentes/inbox/controladorProyectosPorTramite.js",
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
                "~/src/app/fichasProyectos/servicios/servicioFichasProyectos.js",
                "~/src/app/fichasProyectos/template/modales/fichaTemplateController.js",


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
                "~/src/app/usuarios/usuarios/modales/modalTipoInvitacionUsuarioController.js",
                //"~/src/app/usuarios/usuarios/modales/modalInvitarUsuarioController.js",
                "~/src/app/usuarios/usuarios/modales/modalInvitarUsuarioDNPController.js",
                "~/src/app/usuarios/usuarios/modales/modalInvitarUsuarioExternoController.js",
                "~/src/app/usuarios/usuarios/modales/modalAccionUsuarioController.js",
                 "~/src/app/usuarios/usuarios/modales/modalPerfilesUsuarioController.js",
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
                  "~/src/app/formulario/ventanas/comun/informacionPresupuestal/informacionPresupuestalFormulario.js",
                  "~/src/app/formulario/ventanas/comun/documentoSoporte/archivosFormulario.js",
                  "~/src/app/formulario/ventanas/comun/firmaConcepto/firmaConceptoFormulario.js",
                  "~/src/app/formulario/ventanas/comun/solicitarConcepto/solicitudConceptoFormulario.js",
                  "~/src/app/formulario/ventanas/comun/elaborarConcepto/datosInicialesFormulario.js",
                  "~/src/app/formulario/ventanas/comun/elaborarConcepto/cuerpoConceptoFormulario.js",
                  "~/src/app/formulario/ventanas/comun/elaborarConcepto/datosDespedidaFormulario.js",
                  "~/src/app/formulario/ventanas/comun/asociarMuchosProyectos/asociarMuchosProyectosFormulario.js",
                  "~/src/app/formulario/ventanas/comun/asociarMuchosProyectos/detalleMuchosProyectos/detalleMuchosProyectosFormulario.js",
                  "~/src/app/formulario/ventanas/comun/asociarMuchosProyectos/asociarProyectoFormulario.js",

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
                  "~/src/app/formulario/ventanas/comun/programacionRecursos/politicas/detallePolitica/politica/modal/modalAgregarPoliticaTransversalController.js",
                  "~/src/app/formulario/ventanas/ajustes/componentes/focalizacion/politicasTransversales/politicasTransversalesController.js",
                  "~/src/app/formulario/ventanas/ajustes/componentes/focalizacion/politicasTransversales/politicasTransversalesServicio.js",
                  "~/src/app/formulario/ventanas/ajustes/componentes/focalizacion/politicasTransversales/modalAgregarPoliticaTransversalAjustesController.js",
                  "~/src/app/formulario/ventanas/ajustes/componentes/focalizacion/resumenFocalizacion/ResumenFocalizacionController.js",
                  "~/src/app/formulario/ventanas/ajustes/componentes/focalizacion/ResumenFocalizacion/ResumenFocalizacionCategoria/ResumenFocalizacionCategoriaController.js",
                  "~/src/app/formulario/ventanas/ajustes/componentes/focalizacion/Resumen/ResumenController.js",
                  "~/src/app/formulario/ventanas/ajustes/componentes/focalizacion/CrucePoliticasTransversales/crucePoliticasTransversalesController.js",
                  "~/src/app/formulario/ventanas/ajustes/componentes/focalizacion/Concepto/focalizacionconceptoController.js",

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
            #endregion
                 "~/src/app/formulario/ventanas/tramiteTrasladoOrdinario/componentes/documentos/documentoSoporteTramiteTrasladoOrdinarioController.js",

            #region tramite distribucion
                 "~/src/app/formulario/ventanas/tramiteDistribucion/tramiteDistribucionServicio.js",
                 "~/src/app/formulario/ventanas/tramiteDistribucion/tramiteDistribucionController.js",
                 "~/src/app/formulario/ventanas/tramiteDistribucion/componentes/informacionPresupuestalDistribucion/informacionPresupuestalDistribucion.js",
                 "~/src/app/formulario/ventanas/tramiteDistribucion/componentes/conceptosDistribucion/conceptosDistribucionController.js",
                 "~/src/app/formulario/ventanas/tramiteDistribucion/componentes/asociarProyectoDistribucion/asociarProyectoDistribucionController.js",
                 "~/src/app/formulario/ventanas/tramiteDistribucion/componentes/firmaDistribucion/firmaDistribucionController.js",
            #endregion
                 "~/src/app/formulario/ventanas/tramiteDistribucion/componentes/documentos/documentoSoporteTramiteDistribucionController.js",

            #region tramite ley

                "~/src/app/formulario/ventanas/tramiteTrasladoLey/tramiteTrasladoLeyServicio.js",
                "~/src/app/formulario/ventanas/tramiteTrasladoLey/tramiteTrasladoLeyController.js",
                "~/src/app/formulario/ventanas/tramiteTrasladoLey/componentes/informacionPresupuestalLey/informacionPresupuestalLey.js",
                "~/src/app/formulario/ventanas/tramiteTrasladoLey/componentes/conceptosLey/conceptosLeyController.js",
                "~/src/app/formulario/ventanas/tramiteTrasladoLey/componentes/asociarProyectoLey/asociarProyectoLeyController.js",
                "~/src/app/formulario/ventanas/tramiteTrasladoLey/componentes/firmaLey/firmaLeyController.js",

            #endregion
                 "~/src/app/formulario/ventanas/tramiteTrasladoLey/componentes/documentos/documentoSoporteTramiteTrasladoLeyController.js",

            #region Seguimiento y control

                   "~/src/app/formulario/ventanas/seguimientoControl/planearEjecucion/seguimientoPlanearController.js",
                   "~/src/app/formulario/ventanas/seguimientoControl/reporteAvance/seguimientoReporteController.js",
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


            #endregion

            #region Reporte actividades
                    "~/src/app/formulario/ventanas/seguimientoControl/componentes/reporteFinanciero/componentes/avancePresupuestal/reporteActCap/reporteActCapController.js",
                    "~/src/app/formulario/ventanas/seguimientoControl/componentes/reporteFinanciero/componentes/avancePresupuestal/reporteActCap/reporteActCapServicio.js",
                    "~/src/app/formulario/ventanas/seguimientoControl/componentes/reporteFinanciero/componentes/avancePresupuestal/listadoReporteActCap/listadoReporteActCapController.js",
                    "~/src/app/formulario/ventanas/seguimientoControl/componentes/reporteFinanciero/componentes/avancePresupuestal/listadoReporteActCap/listadoReporteActCapServicio.js",
                    "~/src/app/formulario/ventanas/seguimientoControl/componentes/reporteFinanciero/componentes/avancePresupuestal/editarActividadModal/editarActividadPresupuestalModalController.js",
                    "~/src/app/formulario/ventanas/seguimientoControl/componentes/reporteFinanciero/componentes/avancePresupuestal/editarActividadModal/editarActividadPresupuestalModalServicio.js",
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

                "~/src/app/formulario/ventanas/gestionRecursos/componentes/categoriaProductosPolitica/constantesCategoriaProductosPolitica.js",
                "~/src/app/formulario/ventanas/gestionRecursos/componentes/categoriaProductosPolitica/categoriaProductosPoliticaServicio.js",
                "~/src/app/formulario/ventanas/gestionRecursos/componentes/categoriaProductosPolitica/categoriaProductosPoliticaController.js",

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

            #region  Priorizacion
                    "~/src/app/formulario/ventanas/SGR/Priorizacion/priorizacionController.js",
                    "~/src/app/formulario/ventanas/SGR/Priorizacion/priorizacionServicio.js",
                    "~/src/app/formulario/ventanas/SGR/Priorizacion/componentes/priorizacion/priorizacionRegistrarController.js",
                    "~/src/app/formulario/ventanas/SGR/Priorizacion/componentes/Soporte/soportePriorizacionController.js",
                    "~/src/app/formulario/ventanas/SGR/Priorizacion/componentes/priorizacion/detallePriorizacion/detallePriorizacionController.js",

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
                  "~/src/app/formulario/ventanas/SGR/viabilidadSGR/viabilidadSgrServicio.js",
                  "~/src/app/formulario/ventanas/SGR/viabilidadSGR/viabilidadSgrController.js",
                  "~/src/app/formulario/ventanas/SGR/viabilidadSGR/componentes/preguntas/preguntasSgrController.js",
                  "~/src/app/formulario/ventanas/SGR/viabilidadSGR/componentes/preguntas/generales/generalesSgrController.js",
                  "~/src/app/formulario/ventanas/SGR/viabilidadSGR/componentes/recursos/recursosSgrController.js",
                  "~/src/app/formulario/ventanas/SGR/viabilidadSGR/componentes/recursos/fuentes/fuentesSgrController.js",
                  "~/src/app/formulario/ventanas/SGR/viabilidadSGR/componentes/recursos/fuentesNoSgr/fuentesNoSgrController.js",
                  "~/src/app/formulario/ventanas/SGR/viabilidadSGR/componentes/recursos/fuentesNoSgr/addCofinanciacionController.js",
                  "~/src/app/formulario/ventanas/SGR/viabilidadSGR/componentes/operacionesCredito/operacionesCreditoController.js",
                  "~/src/app/formulario/ventanas/SGR/viabilidadSGR/componentes/operacionesCredito/datosGenerales/datosGeneralesSgrController.js",
                  "~/src/app/formulario/ventanas/SGR/viabilidadSGR/componentes/operacionesCredito/informacionDetallada/informacionDetalladaSgrController.js",
                  "~/src/app/formulario/ventanas/SGR/viabilidadSGR/componentes/delegarViabilidad/ejecutorPropuesto/ejecutorPropuestoSgrServicio.js",
                  "~/src/app/formulario/ventanas/SGR/viabilidadSGR/componentes/delegarViabilidad/ejecutorPropuesto/ejecutorPropuestoSgrController.js",
                  
                  "~/src/app/formulario/ventanas/SGR/viabilidadSGR/componentes/regionalizacionSgr/regionalizacionsgrController.js",
                  "~/src/app/formulario/ventanas/SGR/viabilidadSGR/componentes/focalizacion/PoliticasTransversales/politicasTransversalesSgrController.js",
                  "~/src/app/formulario/ventanas/SGR/viabilidadSGR/componentes/focalizacion/CrucePoliticasTransversales/crucePoliticasTransversalesSgrController.js",
                  "~/src/app/formulario/ventanas/SGR/viabilidadSGR/componentes/focalizacion/Resumen/ResumenSgrController.js",
                  "~/src/app/formulario/ventanas/SGR/viabilidadSGR/componentes/focalizacion/ResumenFocalizacion/ResumenFocalizacionCategoria/ResumenFocalizacionCatergoriaSgrController.js",
                  "~/src/app/formulario/ventanas/SGR/viabilidadSGR/componentes/focalizacion/focalizacionAjustesSgrController.js",
                  "~/src/app/formulario/ventanas/SGR/viabilidadSGR/componentes/focalizacion/focalizacionAjustesSgrServicio.js",

            #endregion

            #region vigenciaFuturasExcepcionales
                  "~/src/app/formulario/ventanas/tramiteVfexcep/tramiteVfexcepController.js",
                  "~/src/app/formulario/ventanas/tramiteVFexcep/componentes/documentoSoporte/documentoSoporteVfexcepController.js",
                  "~/src/app/formulario/ventanas/tramiteVFexcep/componentes/aprobacionEntidad/aproentVfexcepController.js",
                  "~/src/app/formulario/ventanas/tramiteVFexcep/componentes/aprobacionEntidad/aprobacionEntidadDet/aproentVfexcepDetController.js",
                  "~/src/app/formulario/ventanas/tramiteVFexcep/componentes/informacionPresupuestalVfexcep/informacionPresupuestalVfexcepController.js",
                  "~/src/app/formulario/ventanas/tramiteVFexcep/componentes/aprobacionSupervisor/aprosuperVfexcepController.js",
                  "~/src/app/formulario/ventanas/tramiteVFexcep/componentes/firmaVfexcep/firmaVfexcepController.js",
                  "~/src/app/formulario/ventanas/tramiteVFexcep/componentes/conceptoVfexcep/conceptoVfexcepController.js",
                  "~/src/app/formulario/ventanas/tramiteVFexcep/componentes/conceptoVfexcep/elaborarConcepto/elaborarConceptoVfexcpController.js",
            #endregion

            #endregion

            #region Comun
                    "~/src/app/formulario/ventanas/comun/preguntas/preguntasController.js",
                    "~/src/app/formulario/ventanas/comun/aprobacion/aprobacionController.js",
                    "~/src/app/formulario/ventanas/comun/informacionPresupuestal/informacionPresupuestalFormulario.js",
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
                "~/src/app/programacion/modales/tramite/agregarTramiteModalController.js",
                "~/src/app/programacion/modales/tramite/servicioAgregarTramiteModal.js",
                "~/src/app/programacion/modales/excepciones/agregarExcepcionModalController.js",
                "~/src/app/programacion/modales/excepciones/servicioAgregarExcepcionModal.js",
                "~/src/app/programacion/modales/ActualizarFechas/actualizarFechasModalController.js",
                "~/src/app/programacion/modales/ActualizarFechas/servicioActualizarFechasModal.js",
                "~/src/app/programacion/modales/configurarMensajeModal/configurarMensajeModalController.js",
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

            #region TarmiteIncorporacion
                "~/src/app/formulario/ventanas/tramiteIncorporacion/componentes/proyectosInc/datosIncorporacion/datosIncorporacionServicio.js",
                "~/src/app/formulario/ventanas/tramiteIncorporacion/componentes/proyectosInc/datosIncorporacion/datosIncorporacionController.js",
                "~/src/app/formulario/ventanas/tramiteIncorporacion/componentes/proyectosInc/datosIncorporacion/modalDatosIncorporacionController.js",
            #endregion TarmiteIncorporacion

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
                "~/Scripts/schema-form/directives/modales/modalEvento.js"
            ));

            BundleTable.EnableOptimizations = false;
        }
    }
}