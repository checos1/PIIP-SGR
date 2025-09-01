using DNP.ServiciosNegocio.Servicios.Interfaces.Entidades;
using DNP.ServiciosNegocio.Persistencia.Implementaciones;
using DNP.ServiciosNegocio.Persistencia.Implementaciones.Entidades;
using DNP.ServiciosNegocio.Persistencia.Interfaces;
using DNP.ServiciosNegocio.Servicios.Implementaciones.Acciones;
using DNP.ServiciosNegocio.Servicios.Implementaciones.Entidades;
using DNP.ServiciosNegocio.Servicios.Implementaciones.Transversales;
using DNP.ServiciosNegocio.Servicios.Interfaces.Acciones;
using DNP.ServiciosNegocio.Servicios.Interfaces.Transversales;
using Unity;
using DNP.ServiciosNegocio.Persistencia.Interfaces.Entidades;
using DNP.ServiciosNegocio.Comunes.Interfaces;
using DNP.ServiciosNegocio.Comunes.Utilidades;
using DNP.ServiciosNegocio.Servicios.Interfaces.FuenteFinanciacion;
using DNP.ServiciosNegocio.Comunes.Autorizacion;
using DNP.ServiciosNegocio.Servicios.Interfaces.Proyectos;
using DNP.ServiciosNegocio.Web.API.Test.Mock;
using DNP.ServiciosNegocio.Persistencia.Interfaces.Preguntas;
using DNP.ServiciosNegocio.Servicios.Implementaciones.Preguntas;
using DNP.ServiciosNegocio.Servicios.Interfaces.Preguntas;
using DNP.ServiciosNegocio.Web.API.Test.Mock.Servicios;
using System.Diagnostics.CodeAnalysis;
using DNP.ServiciosNegocio.Persistencia.Interfaces.Proyectos;
using DNP.ServiciosNegocio.Servicios.Implementaciones.Proyectos;

namespace DNP.ServiciosNegocio.Web.API.Test.Configuracion
{
    using Persistencia.Interfaces.Requisitos;
    using Servicios.Implementaciones.Requisitos;
    using Servicios.Interfaces.Catalogos;
    using Servicios.Interfaces.Requisitos;
    using Servicios.Interfaces.Proyectos;
    using DNP.ServiciosNegocio.Servicios.Interfaces.CadenaValor;
    using DNP.ServiciosNegocio.Servicios.Interfaces.TramitesProyectos;
    using DNP.ServiciosNegocio.Servicios.Interfaces.IndicadoresPolitica;
    using DNP.ServiciosNegocio.Servicios.Interfaces.Transversales;
    using DNP.ServiciosNegocio.Servicios.Interfaces.Priorizacion;
    using DNP.ServiciosNegocio.Servicios.Interfaces.Administracion;
    using DNP.ServiciosNegocio.Servicios.Implementaciones.Administracion;
    using DNP.ServiciosNegocio.Persistencia.Interfaces.Administracion;
    using DNP.ServiciosNegocio.Persistencia.Implementaciones.Administracion;
    using DNP.ServiciosNegocio.Servicios.Interfaces.TramitesDistribucion;
    using DNP.ServiciosNegocio.Servicios.Interfaces.TramitesReprogramacion;
    using DNP.ServiciosNegocio.Servicios.Interfaces.Programacion;
    using DNP.ServiciosNegocio.Servicios.Interfaces.ModificacionLey;
    using DNP.ServiciosNegocio.Servicios.Interfaces.ReportesPIIP;
    using DNP.ServiciosNegocio.Servicios.Implementaciones.ReportesPIIP;
    using DNP.ServiciosNegocio.Servicios.Interfaces.SGR.DelegarViabilidad;
    using DNP.ServiciosNegocio.Servicios.Interfaces.SGP.GestionRecursos;
    using DNP.ServiciosNegocio.Servicios.Interfaces.AdministradorEntidad;
    using DNP.ServiciosNegocio.Web.API.Test.Mock.AdministradorEntidad;
    using DNP.ServiciosNegocio.Servicios.Interfaces.SGP.Transversales;
    using DNP.ServiciosNegocio.Servicios.Interfaces.SGR.Viabilidad;
    using DNP.ServiciosNegocio.Servicios.Interfaces.SGP.Ajustes;
    using DNP.ServiciosNegocio.Servicios.Interfaces.SGP.Tramite;
    using DNP.ServiciosNegocio.Web.API.Test.Mock.SGP.Tramite;
    using DNP.ServiciosNegocio.Servicios.Interfaces.SGP.Viabilidad;
    using DNP.ServiciosNegocio.Servicios.Implementaciones.SGP.Viabilidad;
    using DNP.ServiciosNegocio.Persistencia.Interfaces.SGP.Viabilidad;
    using DNP.ServiciosNegocio.Persistencia.Implementaciones.SGP.Viabilidad;

    [ExcludeFromCodeCoverage]
    public class UnityConfig
    {
        public static IUnityContainer Container
        {
            get
            {
                var container = new UnityContainer();

                container.RegisterType<IEntidadServicios, OpcionCatalogoTipoEntidadServicios>()
                         .RegisterType<IEntidadPersistencia, EntidadPersistencia>()
                         .RegisterType<IContextoFactory, ContextoFactory>()
                         .RegisterType<IEjecucionAccionTransaccionalServicios,EjecucionAccionTransaccionalEntidadServicios>()
                         .RegisterType<IAccionUtilidades, AccionUtilidades>()
                         .RegisterType<IAuditoriaServicios, AuditoriaServicios>()
                         .RegisterType<IFuenteFinanciacionServicios, FuenteFinanciacionProyectoServicioMock>()
                         .RegisterType<IProyectoServicio, ProyectoServicioMock>()
                         .RegisterType<IAutorizacionUtilidades, AutorizacionUtilidadesMock>()
                         .RegisterType<IPreguntasServicio, PreguntasServicio>()
                         .RegisterType<IPreguntasPersistencia, PreguntasServicioMock>()
                         .RegisterType<IRequisitosServicio, RequisitosServicio>()
                         .RegisterType<IRequisitosPersistencia, RequisitosServicioMock>()
                         .RegisterType<ICatalogoServicio, CatalogoServicioMock>()
                         .RegisterType<IEntidadAccionesServicio, EntidadAccionesServicio>()
                         .RegisterType<IEntidadAccionesPersistencia, EntidadesAccionesServicioMock>()
                         .RegisterType<IDevolverProyectoServicio, DevolverProyectoServicioMock>()                     
                         .RegisterType<IEstadoServicio, EstadoServicioMock>()
                         .RegisterType<IFuenteCofinanciacionServicio, FuenteCofinanciacionServicioMock>()
                         .RegisterType<IDefinirAlcanceServicios, DefinirAlcanceServiciosMock>()
                         .RegisterType<ICofinanciacionAgregarServicio, CofinanciacionAgregarServicioMock>()
                         .RegisterType<IIncluirPoliticasServicios, IncluirPoliticasServicioMock>()
                         .RegisterType<IIndicadoresProductoServicio, IndicadoresProductoServicioMock>()
                         .RegisterType<ITramitesProyectosServicio, TramitesProyectosServicioMock>()
                         .RegisterType<IIndicadoresPoliticaServicio, IndicadoresPoliticaMock>()
                         .RegisterType<ICategoriaProductosPoliticaServicio, CategoriaProductosPoliticaMock>()
                         .RegisterType<ISeccionCapituloServicio, SeccionCapituloServicioMock>()
                         .RegisterType<IPriorizacionServicio, PriorizacionServicioMock>()
                         .RegisterType<IEjecutorServicio, EjecutorServicio>()
                         .RegisterType<IEjecutorPersistencia, EjecutorPersistencia>()
                         .RegisterType<IAdministrarDocumentoServicio, AdministrarDocumentoServicio>()
                         .RegisterType<IAdministrarDocumentoPersistencia, AdministrarDocumentoPersistencia>()
                         .RegisterType<ITramitesDistribucionServicio, TramitesDistribucionServicioMock>()
                         .RegisterType<ITramitesReprogramacionServicio, TramitesReprogramacionServicioMock>()
                         .RegisterType<IProgramacionServicio, ProgramacionServicioMock>()
                         .RegisterType<IModificacionLeyServicio, ModificacionLeyServicioMock>()
                         .RegisterType<IDelegarViabilidadServicio, DelegarViabilidadServicioMock>()
                         .RegisterType<IGestionRecursosSgpServicio, GestionRecursosSgpServicioMock>()
                         .RegisterType<IAdministradorEntidadSgpServicio, AdministradorEntidadSgpServicioMock>()
                         .RegisterType<ITransversalServicioSGP, TransversalSgpServicioMock>()
                         .RegisterType<ICTUSServicio, CTUSServicioMock>()
                         .RegisterType<IPreguntasPersonalizadasServicio, PreguntasPersonalizadasServicioMock>()
                         .RegisterType<IFuenteFinanciacionAgregarServicio, FuenteFinanciacionAgregarServicioMock>()
                         .RegisterType<IAjustesSgpServicio, AjustesSgpServicioMock>()
                         .RegisterType<ITramiteProyectoSGPServicio, TramiteProyectoSGPServicioMock>()
                         .RegisterType<IViabilidadSGPServicio, ViabilidadSGPServicio>()
                         .RegisterType<IViabilidadSgpPersistencia, ViabilidadSgpPersistencia>()
                         .RegisterType<IContextoFactorySGR, ContextoFactorySGR>()
                    ;

                return container;
            }
        }
    }
}
