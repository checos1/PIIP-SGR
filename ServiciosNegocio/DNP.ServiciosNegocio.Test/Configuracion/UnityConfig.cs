namespace DNP.ServiciosNegocio.Test.Configuracion
{
    using Comunes.Interfaces;
    using Comunes.Utilidades;
    using DNP.ServiciosNegocio.Persistencia.Implementaciones.FuenteFinanciacion;
    using DNP.ServiciosNegocio.Persistencia.Implementaciones.Proyectos;
    using DNP.ServiciosNegocio.Persistencia.Implementaciones.ReportesPIIP;
    using DNP.ServiciosNegocio.Persistencia.Implementaciones.TramitesProyectos;
    using DNP.ServiciosNegocio.Persistencia.Implementaciones.Transversales;
    using DNP.ServiciosNegocio.Persistencia.Interfaces.TramitesProyectos;
    using DNP.ServiciosNegocio.Persistencia.Interfaces.ReportesPIIP;
    using DNP.ServiciosNegocio.Persistencia.Interfaces.Transversales;
    using DNP.ServiciosNegocio.Servicios.Implementaciones.Transversales;
    using Mock;
    using Persistencia.Implementaciones;
    using Persistencia.Implementaciones.Entidades;
    using Persistencia.Implementaciones.Genericos;
    using Persistencia.Interfaces;
    using Persistencia.Interfaces.Catalogos;
    using Persistencia.Interfaces.Entidades;
    using Persistencia.Interfaces.FuenteFinanciacion;
    using Persistencia.Interfaces.Genericos;
    using Persistencia.Interfaces.Preguntas;
    using Persistencia.Interfaces.Proyectos;
    using Persistencia.Interfaces.Requisitos;
    using ServiciosNegocio.Servicios.Implementaciones.Acciones;
    using ServiciosNegocio.Servicios.Implementaciones.Catalogos;
    using ServiciosNegocio.Servicios.Implementaciones.Entidades;
    using ServiciosNegocio.Servicios.Implementaciones.FuenteFinanciacion;
    using ServiciosNegocio.Servicios.Implementaciones.Preguntas;
    using ServiciosNegocio.Servicios.Implementaciones.Proyectos;
    using ServiciosNegocio.Servicios.Implementaciones.Requisitos;
    using ServiciosNegocio.Servicios.Interfaces.Acciones;
    using ServiciosNegocio.Servicios.Interfaces.Catalogos;
    using ServiciosNegocio.Servicios.Interfaces.Entidades;
    using ServiciosNegocio.Servicios.Interfaces.FuenteFinanciacion;
    using ServiciosNegocio.Servicios.Interfaces.Preguntas;
    using ServiciosNegocio.Servicios.Interfaces.Proyectos;
    using ServiciosNegocio.Servicios.Interfaces.Requisitos;
    using ServiciosNegocio.Servicios.Interfaces.Transversales;
    using Unity;

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
                         .RegisterType<IEjecucionAccionTransaccionalServicios, EjecucionAccionTransaccionalEntidadServicios>()
                         .RegisterType<IAccionUtilidades, AccionUtilidades>()
                         .RegisterType<IAuditoriaServicios, AuditoriaServiciosMock>()
                         .RegisterType<IFuenteFinanciacionServicios, FuenteFinanciacionProyectoServicio>()
                         .RegisterType<ICacheServicio, CacheServicioMock>()
                         .RegisterType<IProyectoServicio, ProyectoServicio>()
                         .RegisterType<IProyectoPersistencia, ProyectoPersistenciaMock>()
                         .RegisterType<IFuenteFinanciacionPersistencia, FuenteFinanciacionPersistenciaMock>()
                         .RegisterType<IPersistenciaTemporal, PersistenciaTemporalMock>()
                         .RegisterType<IPersistenciaTemporal, PersistenciaTemporal>()
                         .RegisterType<IPreguntasServicio, PreguntasServicio>()
                         .RegisterType<IPreguntasPersistencia, PreguntasPersistenciaMock>()
                         .RegisterType<IRequisitosServicio, RequisitosServicio>()
                         .RegisterType<IRequisitosPersistencia, RequisitosPersistenciaMock>()
                         .RegisterType<ICatalogoServicio, CatalogoServicio>()
                         .RegisterType<ICatalogoPersistencia, CatalogoPersistenciaMock>()
                         .RegisterType<IEntidadAccionesServicio, EntidadAccionesServicio>()
                         .RegisterType<IEntidadAccionesPersistencia, EntidadAccionesPersistenciaMock>()
                         .RegisterType<IDatosBasicosSGRServicio, DatosBasicosSGRServicio>()
                         .RegisterType<IDatosBasicosSGRPersistencia, DatosBasicosSGRPersistenciaMock>()
                         .RegisterType<IFuenteFinanciacionAgregarServicio, FuenteFinanciacionAgregarServicio>()
                         .RegisterType<IFuenteFinanciacionAgregarPersistencia, FuenteFinanciacionAgregarPersistenciaMock>()
                         .RegisterType<IDevolverProyectoServicio, DevolverProyectoServicio>()
                         .RegisterType<IDevolverProyectoPersistencia, DevolverProyectoPersistenciaMock>()
                         .RegisterType<IEstadoPersistencia, EstadoPersistencia>()
                         .RegisterType<IEstadoServicio, EstadoServicio>()
                         .RegisterType<IDocumentoSoportePersistencia, DocumentoSoportePersistencia>()
                         .RegisterType<IDocumentoSoporteServicio, DocumentoSoporteServicio>()
                         .RegisterType<IFuenteCofinanciacionServicio, FuenteCofinanciacionServicio>()
                         .RegisterType<IFuenteCofinanciacionPersistencia, FuenteCofinanciacionPersistenciaMock>()
                         .RegisterType<IDefinirAlcanceServicios, DefinirAlcanceServicios>()
                         .RegisterType<IDefinirAlcancePersistencia, DefinirAlcancePersistenciaMock>()
                         .RegisterType<ICofinanciacionAgregarServicio, CofinanciacionAgregarServicio>()
                         .RegisterType<ICofinanciacionAgregarPersistencia, CofinanciacionAgregarPersistenciaMock>()
                         .RegisterType<IIncluirPoliticasPersistencia, IncluirPoliticasPersistenciaMock>()
                         .RegisterType<IFuentesAprobacionPersistencia, FuentesAprobacionPersistenciaMock>()
                         .RegisterType<IVigenciaPersistencia, VigenciaPersistencia>()
                         .RegisterType<IAjusteIncluirPoliticasPersistencia, AjusteIncluirPoliticasPersistenciaMock>()
                         .RegisterType<IFuentesProgramarSolicitadoServicio, FuentesProgramarSolicitadoServicio>()
                         .RegisterType<IFuentesProgramarSolicitadoPersistencia, FuentesProgramarSolicitadoPersistencia>()
                         .RegisterType<IDatosAdicionalesServicio, DatosAdicionalesServicio>()
                         .RegisterType<IDatosAdicionalesPersistencia, DatosAdicionalesPersistencia>()
                         .RegisterType<IFuentesAprobacionServicio, FuentesAprobacionServicio>()
                         .RegisterType<IFuentesAprobacionPersistencia, FuentesAprobacionPersistencia>()
                         .RegisterType<ICambiosJustificacionHorizonServicio, CambiosJustificacionHorizonteServicio>()
                         .RegisterType<ICambiosJustificacionHorizontePersistencia, CambiosJustificacionHorizontePersistencia>()
                         .RegisterType<ISeccionCapituloPersistencia, SeccionCapituloPersistencia>()
                         .RegisterType<IFasePersistencia, FasePersistencia>()
                         .RegisterType<ICambiosRelacionPlanificacionServicio, CambiosRelacionPlanificacionServicio>()
                         .RegisterType<ICambiosRelacionPlanificacionPersistencia, CambiosRelacionPlanificacionPersistencia>()
                         .RegisterType<IReportesPIIPPersistencia, ReportesPIIPPersistencia>()
                    ;

                return container;
            }
        }
    }
}
