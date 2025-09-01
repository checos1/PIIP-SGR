(function () {
    'use strict';

    angular.module('backbone').config(function ($routeProvider, $locationProvider) {

        $routeProvider.caseInsensitiveMatch = true;

        $routeProvider

            .when("/", {
                controller: "validarAutorizacionController",
                templateUrl: "src/app/autorizacion/validarAutorizacionTemplate.html",
                controllerAs: "vm"
            })
            .when("/RecuperarContrasena/", {
                controller: "recuperarClaveUsuarioController",
                templateUrl: "src/app/usuarios/recuperarClaveUsuario/recuperarClaveUsuarioTemplate.html",
                controllerAs: "vm"
            })
            .when("/CambioContrasena/", {
                controller: "asignarClaveUsuarioController",
                templateUrl: "src/app/usuarios/asignarClaveUsuario/asignarClaveUsuarioTemplate.html",
                controllerAs: "vm"
            })
            .when("/CambioContrasena/:id", {
                controller: "asignarClaveUsuarioController",
                templateUrl: "src/app/usuarios/asignarClaveUsuario/asignarClaveUsuarioTemplate.html",
                controllerAs: "vm"
            })
            .when("/inicio", {
                controller: "controladorInicio",
                templateUrl: "src/app/inicio/plantillaInicio.html",
                controllerAs: "vm"
            })
            .when("/plantillaEjemplo", {
                controller: "controladorEjemplo",
                templateUrl: "src/app/inicio/plantillaEjemplo.html",
                controllerAs: "vm"
            })
            .when("/bancoproyectos/proyectos", {
                controller: "consolaProyectosController",
                templateUrl: "src/app/consola/proyectos/consolaProyectos.html",
                controllerAs: "vm"
            })
            .when("/bancoproyectos/tramites", {
                controller: "consolaTramitesController",
                templateUrl: "src/app/consola/tramites/consolaTramites.html",
                controllerAs: "vm"
            })
            .when("/proyectos/:etapa", {
                controller: "controladorPanelPrincipal",
                templateUrl: "src/app/panelPrincial/plantillaPanelPrincipal.html",
                controllerAs: "vm"
            })
            .when("/tramites/:etapa", {
                controller: "controladorPanelPrincipal",
                templateUrl: "src/app/panelPrincial/plantillaPanelTramites.html",
                controllerAs: "vm"
            })
            .when("/programacion/:etapa", {
                controller: "controladorPanelPrincipal",
                templateUrl: "src/app/panelPrincial/plantillaPanelProgramacion.html",
                controllerAs: "vm"
            })
            .when("/consolaprocesos/proyectos", {
                controller: "controladorPanelPrincipal",
                templateUrl: "src/app/panelPrincial/plantillaPanelConsolaProcesos.html",
                controllerAs: "vm"
            })
            .when("/tramite/:id/proyectos", {
                controller: "controladorPanelProyectosPorTramite",
                templateUrl: "src/app/panelPrincial/plantillaPanelProyectosPorTramite.html",
                controllerAs: "vm"
            })
            .when("/MiPerfil", {
                controller: "usuarioPerfilController",
                templateUrl: "src/app/usuarios/usuarioPerfil/usuarioPerfil.html",
                controllerAs: "vm"
            })
            .when("/usuarios/:id", {
                controller: "usuarioPerfilController",
                templateUrl: "src/app/usuarios/usuarioPerfil/usuarioPerfil.html",
                controllerAs: "vm"
            })
            .when("/entidades", {
                controller: "entidadesController",
                templateUrl: "src/app/entidades/entidades/entidades.html",
                controllerAs: "vm"
            })
            .when("/unidadResponsable", {
                controller: "unidadResponsableController",
                templateUrl: "src/app/entidades/entidades/unidadResponsable.html",
                controllerAs: "vm"
            })
            //.when("/cargarDatos", {
            //    templateUrl: "src/app/entidades/cargarDatos/CargarDatos.html",
            //    controller: 'cargarDatosController',
            //    controllerAs: "cargarDatosCtrl"
            //})
            .when("/cargaDatos", {
                templateUrl: "src/app/entidades/cargaDatos/cargaDatos.html",
                controller: 'cargaDatosController',
                controllerAs: "vm"
            })
            .when("/entidades/proyectos", {
                templateUrl: 'src/app/entidades/proyectos/EntidadProyectos.html',
                controller: 'entidadProyectosController',
                controllerAs: 'entidadProyectosCtrl'
            })
            .when("/usuarios", {
                controller: "usuariosController",
                templateUrl: "src/app/usuarios/usuarios/usuarios.html",
                controllerAs: "vm"
            })
            .when("/usuariosTerritorio", {
                controller: "usuariosTerritorioController",
                templateUrl: "src/app/usuarios/usuarios/usuariosTerritorio.html",
                controllerAs: "vm"
            })
            .when("/perfiles", {
                controller: "perfilesController",
                templateUrl: "src/app/usuarios/perfiles/perfiles.html",
                controllerAs: "vm"
            })
            .when("/roles", {
                controller: "rolesController",
                templateUrl: "src/app/usuarios/roles/roles.html",
                controllerAs: "vm"
            })
            .when("/notificaciones/:id", {
                templateUrl: "src/app/panelNotificaciones/plantillaPanelNotificaciones.html",
                controller: "controladorPanelNotificaciones",
                controllerAs: "vm"
            })
            .when("/notificaciones", {
                templateUrl: "src/app/panelNotificaciones/plantillaPanelNotificaciones.html",
                controller: "controladorPanelNotificaciones",
                controllerAs: "vm"
            })
            .when("/ejecutarAccion/:bpin/:estado", {
                templateUrl: "src/app/panelEjecucionDeAccion/plantillaPanelEjecucionDeAccion.html",
                controller: "controladorPanelEjecucionDeAccion",
                controllerAs: "vm"
            })
            .when('/Account/SignOu', {
                controller: "controladorSignOut"
            })
            .when('/Acciones/ConsultarAcciones', {
                controller: "consultarAccionesController",
                templateUrl: "/src/app/acciones/consultarAcciones/consultarAcciones.html",
                controllerAs: "vm"
            })
            .when('/Acciones/ConsultarAcciones/PrevisualizadorFormularios', {
                controller: "previsualizadorFormulariosController",
                templateUrl: "/src/app/acciones/consultarAcciones/ejecucionFormularios.html",
                controllerAs: "vm"
            })
            .when("/autorizacion/ConfigurarEntidadRolSector", {
                controller: "configurarEntidadRolSectorController",
                templateUrl: "/src/app/autorizacion/configurarEntidadRolSector.html",
                controllerAs: "vm",
                resolve: {
                    listaEntidadRolSector: ['configurarEntidadRolSectorServicio', obtenerConfiguracionesRolSector]
                }
            })
            .when('/Account/SignOut', {
                resolve: {
                    factory: ['$localStorage', cerrarPagina]
                }
            })
            .when("/nuevotabla", {
                controller: "controladorTabla",
                templateUrl: "/src/app/panelPrincial/test/plantillaTabla.html",
                controllerAs: "vm"
            })
            .when("/tableroControlProyectos", {
                controller: "consolaMonitoreoController",
                templateUrl: "src/app/monitoreo/consola.monitoreo.template.html",
                controllerAs: "vm"
            })
            .when("/ReportesPIIP", {
                controller: "consolaReportesController",
                templateUrl: "src/app/reportesPIIP/consolaReportes.html",
                controllerAs: "vm"
            })
            .when("/adminBancos", {
                controller: "bancosController",
                templateUrl: "src/app/administracion/bancos/bancos.html",
                controllerAs: "vm"
            })
            .when("/consolaMonitoreo/:proyectoid/:codigobpin/:proyectonombre", {
                controller: "consolaMonitoreoProyectoController",
                templateUrl: "src/app/monitoreo/template/reportes/proyecto/consolaMonitoreoProyecto.html",
                controllerAs: "vm"
            })
            .when("/consolaAlertas", {
                controller: "consolaAlertasController",
                templateUrl: "src/app/monitoreo/consola.alertas.template.html",
                controllerAs: "vm"
            })
            .when("/tableroControlEntidades", {
                controller: "consolaMonitoreoEntidadController",
                templateUrl: "src/app/monitoreo/template/consolaMonitoreoEntidad.html",
                controllerAs: "vm"
            })
            .when("/mensajeMantenimiento/:id?", {
                controller: "crearActualizarMensajeController",
                templateUrl: "src/app/mensajesMantenimiento/template/crearActualizarMensaje/crearActualizarMensaje.html",
                controllerAs: "vm"
            })
            .when("/consolaTramite/:id/proyectos", {
                controller: "controladorPanelProyectosPorTramiteConsola",
                templateUrl: "src/app/consola/tramites/plantillas/plantillaPanelProyectosPorTramite.html",
                controllerAs: "vm"
            })
            .when("/listaMensajeMantenimiento", {
                controller: "listaMensajesMantenimientoController",
                templateUrl: "src/app/mensajesMantenimiento/template/listaMensajes/listaMensajesMantenimiento.html",
                controllerAs: "vm"
            })
            .when("/entidad/inflexibilidades/:id", {
                controller: "inflexibilidadesListaController",
                templateUrl: "src/app/entidades/entidades/tipoEntidad/entidad-sgr/inflexibilidades/inflexibilidadesLista/inflexibilidadesLista.html",
                controllerAs: "vm"
            })
            .when("/proyectosCreditos", {
                controller: "selecionarProyectosCreditosController",
                templateUrl: "src/app/formulario/modales/selecionarProyectosCreditos.html",
                controllerAs: "vm"
            })
            .when("/programacion", {
                controller: "programacionController",
                templateUrl: "src/app/programacion/programacion.html",
                controllerAs: "vm"
            })
            .when("/logs/tramites", {
                controller: "plantillaLogsTramitesYProyectosController",
                templateUrl: "src/app/dadosLogs/template/plantillaLogsTramitesYProyectos.html",
                controllerAs: "vm"
            })
            .when("/logs/proyectos", {
                controller: "plantillaLogsTramitesYProyectosController",
                templateUrl: "src/app/dadosLogs/template/plantillaLogsTramitesYProyectos.html",
                controllerAs: "vm"
            })
            .when("/seleccionDeProyectos", {
                controller: "seleccionDeProyectosController",
                templateUrl: "src/app/formulario/modales/seleccionDeProyectos.html",
                controllerAs: "vm"
            })
            .when("/resumenDeProyectos", {
                controller: "resumenDeProyectosController",
                templateUrl: "src/app/formulario/modales/resumenDeProyectos.html",
                controllerAs: "vm"
            })
            .when("/centroAyuda", {
                controller: "centroAyudaController",
                templateUrl: "src/app/centroAyuda/centroAyudaLista/centroAyuda.html",
                controllerAs: "vm"
            })
            .when("/NotificacionesMantenimiento/ListaNotificaciones", {
                controller: "listaNotificacionesMantenimientoController",
                templateUrl: "src/app/notificacionesMantenimiento/listaNotificaciones/listaNotificacionesMantenimiento.html",
                controllerAs: "vm"
            })
            .when("/listaNotificacionesMensajes", {
                controller: "listaNotificacionesMensajesController",
                templateUrl: "src/app/notificacionesMantenimiento/listaNotificacionesMensajes/listaNotificacionesMensajes.html",
                controllerAs: "vm"
            })
            .when("/notificacionesMantenimiento/:id?", {
                controller: "crearActualizarNotificacionController",
                templateUrl: "src/app/notificacionesMantenimiento/crearActualizarNotificacion/crearActualizarNotificacion.html",
                controllerAs: "vm"
            })
            .when("/consolaprocesos/tramites", {
                controller: "controladorPanelPrincipal",
                templateUrl: "src/app/panelPrincial/plantillaPanelConsolaProcesosTramites.html",
                controllerAs: "vm"
            })
            .when("/consolaprocesos/programacion", {
                controller: "controladorPanelPrincipal",
                templateUrl: "src/app/panelPrincial/plantillaPanelConsolaProcesosProgramacion.html",
                controllerAs: "vm"
            })
            .when("/consolaprocesos/index", {
                controller: "controladorPanelPrincipal",
                templateUrl: "src/app/panelPrincial/plantillaPanelConsolaProcesosIndex.html",
                controllerAs: "vm"
            })
            .when("/tramiteTraslados", {
                controller: "trasladosTramiteController",
                templateUrl: "src/app/formulario/ventanas/tramites/traslados.html",
                controllerAs: "vm"

                //}).when("/trasladosAcciones", {
                //    controller: "trasladosAccionesController",
                //    templateUrl: "src/app/formulario/ventanas/tramites/trasladosAcciones.html",
                //    controllerAs: "vm"

            }).when("/seleccionProyectosTramite", {
                controller: "seleccionProyectosTramiteController",
                templateUrl: "src/app/formulario/ventanas/tramites/seleccionProyectosTramite.html",
                controllerAs: "vm"

            }).when("/trasladosAprobacionFuentes", {
                controller: "trasladosAprobacionFuentesController",
                templateUrl: "src/app/formulario/ventanas/tramites/trasladosAprobacionFuentes.html",
                controllerAs: "vm"

                //}).when("/requisitosAprobacion", {
                //    controller: "requisitosAprobacionController",
                //    templateUrl: "src/app/formulario/ventanas/tramites/requisitosAprobacion.html",
                //    controllerAs: "vm"

            }).when("/listaProyectosRegistrados", {
                controller: "listaProyectosRegistradosController",
                templateUrl: "src/app/formulario/ventanas/comun/programacionDistribucionRecursos/listaProyectosRegistrados.html",
                controllerAs: "vm"

            }).when("/gestionRecursos", {
                controller: "gestionRecursosController",
                templateUrl: "src/app/formulario/ventanas/gestionRecursos/gestionRecursos.html",
                controllerAs: "vm"

            }).when("/ajustes", {
                controller: "ajustesController",
                templateUrl: "src/app/formulario/ventanas/ajustes/ajustes.html",
                controllerAs: "vm"

            }).when("/ajustesConTramite", {
                controller: "ajustesConTramiteController",
                templateUrl: "src/app/formulario/ventanas/ajustesConTramite/ajustesConTramite.html",
                controllerAs: "vm"

            }).when("/reporteGantt", {
                controller: "reporteGanttController",
                templateUrl: "/src/app/formulario/ventanas/seguimientoControl/componentes/DiagramaGantt/reporteGantt.html",
                controllerAs: "vm"

            }).when("/reporteEstructuraActividades", {
                controller: "reporteEstructuraActividadesController",
                templateUrl: "/src/app/formulario/ventanas/seguimientoControl/componentes/DiagramaEstructuraActividades/reporteEstructuraActividades.html",
                controllerAs: "vm"

            }).when("/ejecutorSGR", {
                controller: "ejecutorController",
                templateUrl: "src/app/administracion/ejecutor/ejecutor.html",
                controllerAs: "vm"
            })
            .when("/documento", {
                controller: "documentoController",
                templateUrl: "src/app/administracion/documentos/documento.html",
                controllerAs: "vm"
            })
            .when("/proyectos/verificacion/asignar", {
                controller: "controladorPanelPrincipal",
                templateUrl: "src/app/panelPrincial/plantillaProyectosAsignarVerificacionIndexSgr.html",
                controllerAs: "vm"
            })
            .when("/proyectos/verificacion/seguimiento-control", {
                controller: "controladorPanelPrincipal",
                templateUrl: "src/app/panelPrincial/plantillaProyectosSeguimientoControlVerificacionIndexSgr.html",
                controllerAs: "vm"
            })
            .when("/proyectos/verificacion/revision-coordinador", {
                controller: "controladorPanelPrincipal",
                templateUrl: "src/app/panelPrincial/plantillaProyectosRevisionCoordinadorVerificacionIndexSgr.html",
                controllerAs: "vm"
            })
            .when("/proyectos/verificacion/subdirector", {
                controller: "controladorPanelPrincipal",
                templateUrl: "src/app/panelPrincial/plantillaProyectosSubdirectorVerificacionIndexSgr.html",
                controllerAs: "vm"
            });

        $locationProvider.html5Mode(true);

        function obtenerConfiguracionesRolSector(configurarEntidadRolSectorServicio) {
            var parametros = {
                usuarioDnp: usuarioDNP,
                nombreAplicacion: nombreAplicacionBackbone
            }
            return configurarEntidadRolSectorServicio.obtenerConfiguracionesRolSector(parametros);
        }

        function cerrarPagina($localStorage) {
            delete $localStorage.authorizationData;
            location.reload();
        };

    });


})();