namespace DNP.ServiciosWBS.Web.API
{
    using DNP.ServiciosNegocio.Comunes.Autorizacion;
    using DNP.ServiciosWBS.Persistencia.Implementaciones;
    using DNP.ServiciosWBS.Persistencia.Implementaciones.Transversales;
    using DNP.ServiciosWBS.Persistencia.Interfaces;
    using DNP.ServiciosWBS.Persistencia.Interfaces.Transversales;
    using DNP.ServiciosWBS.Servicios.Implementaciones;
    using DNP.ServiciosWBS.Servicios.Implementaciones.Transversales;
    using DNP.ServiciosWBS.Servicios.Interfaces;
    using DNP.ServiciosWBS.Servicios.Interfaces.Transversales;
    using System.Web.Http;
    using Unity;
    using Unity.WebApi;

    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
            var container = new UnityContainer();

            container.RegisterType<IContextoFactory, ContextoFactory>().
                      RegisterType<IAuditoriaServicios, AuditoriaServicios>().
                      RegisterType<IAutorizacionUtilidades, AutorizacionUtilidades>().
                      RegisterType<IProductoPersistencia, ProductoPersistencia>().
                      RegisterType<IPersistenciaTemporal, PersistenciaTemporal>().
                      RegisterType<IProductosServicio, ProductosServicio>().
                      RegisterType<ICadenaValorPersistencia, CadenaValorPersistencia>().
                      RegisterType<ICadenaValorServicios, CadenaValorServicios>().
                      RegisterType<IRegionalizacionProyectoServicios, RegionalizacionProyectoServicios>().
                      RegisterType<IRegionalizacionProyectoPersistencia, RegionalizacionProyectoPersistencia>().
                      RegisterType<IRegionalizacionIndicadoresServicios, RegionalizacionIndicadoresServicios>().
                      RegisterType<IRegionalizacionIndicadoresPersistencia, RegionalizacionIndicadoresPersistencia>().
                      RegisterType<IFocalizacionProyectoServicios, FocalizacionProyectoServicios>().
                      RegisterType<IFocalizacionProyectoPersistencia, FocalizacionProyectoPersistencia>().
                      RegisterType<IFocalizacionServicios, FocalizacionServicios>().
                      RegisterType<IFocalizacionPersistencia, FocalizacionPersistencia>().
                      RegisterType<ICuantificacionLocalizacionServicios, CuantificacionLocalizacionServicios>().
                      RegisterType<ICuantificacionLocalizacionPersistencia, CuantificacionLocalizacionPersistencia>().
                      RegisterType<IFocalizacionCuantificacionLocalizacionServicios, FocalizacionCuantificacionLocalizacionServicios>().
                      RegisterType<IFocalizacionCuantificacionLocalizacionPersistencia, FocalizacionCuantificacionLocalizacionPersistencia>().
                      RegisterType<IIndicadorProductoAgregarServicios, IndicadorProductoAgregarServicios>().
                      RegisterType<IIndicadorProductoAgregarPersistencia, IndicadorProductoAgregarPersistencia>().
                      RegisterType<IRegionalizacionIndicadorAgregarServicios, RegionalizacionIndicadorAgregarServicios>().
                      RegisterType<IRegionalizacionIndicadorAgregarPersistencia, RegionalizacionIndicadorAgregarPersistencia>().
                      RegisterType<IValidarViabilidadCompletarInfoServicios, ValidarViabilidadCompletarInfoServicios>().
                      RegisterType<IValidarViabilidadCompletarInfoPersistencia, ValidarViabilidadCompletarInfoPersistencia>().
                      RegisterType<IDiligenciarFuentes, DiligenciarFuentesServicios>().
                      RegisterType<IDiligenciarFuentesPersistencia, DiligenciarFuentesPersistencia>().
                      RegisterType<ICuantificacionBeneficiarioServicios, CuantificacionBeneficiarioServicios>().
                      RegisterType<ICuantificacionBeneficiarioPersistencia, CuantificacionBeneficiarioPersistencia>().
                      RegisterType<IFocalizacionPoliticasTransversalesServicios, FocalizacionPoliticasTransversalesServicios>().
                      RegisterType<IFocalizacionPoliticasTransversalesPersistencia, FocalizacionPoliticasTransversalesPersistencia>().
                      RegisterType<IRegionalizacionMetasyRecursosServicios, RegionalizacionMetasyRecursosServicios>().
                      RegisterType<IRegionalizacionMetasyRecursosPersistencia, RegionalizacionMetasyRecursosPersistencia>().
                      RegisterType<ILocalizacionServicios, LocalizacionServicios>().
                      RegisterType<ILocalizacionPersistencia, LocalizacionPersistencia>().
                      RegisterType<IPoliticaTransversalCategoriaServicios, PoliticaTransversalCategoriaServicios>().
                      RegisterType<IPoliticaTransversalCategoriaPersistencia, PoliticaTransversalCategoriaPersistencia>().
                      RegisterType<IPoliticaTransversalBeneficiarioServicios, PoliticaTransversalBeneficiarioServicios>().
                      RegisterType<IPoliticaTransversalBeneficiarioPersistencia, PoliticaTransversalBeneficiarioPersistencia>().
                      RegisterType<IFocalizacionPoliticasTransversalesMetasServicios, FocalizacionPoliticasTransversalesMetasServicios>().
                      RegisterType<IFocalizacionPoliticasTransversalesMetasPersistencia, FocalizacionPoliticasTransversalesMetasPersistencia>().
                      RegisterType<IFocalizacionPoliticasTransversalesFuentesServicios, FocalizacionPoliticasTransversalesFuentesServicios>().
                      RegisterType<IFocalizacionPoliticasTransversalesFuentesPersistencia, FocalizacionPoliticasTransversalesFuentesPersistencia>().
                      RegisterType<IIncluirPoliticasServicios, IncluirPoliticasServicios>().
                      RegisterType<IIncluirPoliticasPersistencia, IncluirPoliticasPersistencia>().
                      RegisterType<IRegionalizaFuentesPersistencia, RegionalizaFuentesPersistencia>().
                      RegisterType<IRegionalizaFuentesServicio, RegionalizaFuentesServicio>().
                      RegisterType<IFocalizacionPoliticasTransversalesAsociacionIndicadoresServicios, FocalizacionPoliticasTransversalesAsociacionIndicadoresServicios>().
                      RegisterType<IFocalizacionPoliticasTransversalesAsociacionIndicadoresPersistencia, FocalizacionPoliticasTransversalesAsociacionIndicadoresPersistencia>().
                      RegisterType<IFocalizacionPoliticasTransversalesRelacionadasServicios, FocalizacionPoliticasTransversalesRelacionadasServicios>().
                      RegisterType<IFocalizacionPoliticasTransversalesRelacionadasPersistencia, FocalizacionPoliticasTransversalesRelacionadasPersistencia>().
                      RegisterType<IAjustesUbicacionServicios, AjustesUbicacionServicios>().
                      RegisterType<IAjustesRegionalizaFuentesPersistencia, AjustesRegionalizaFuentesPersistencia>().
                      RegisterType<IAjustesRegionalizaFuentesServicios, AjustesRegionalizaFuentesServicios>().
                      RegisterType<IAjustesUbicacionPersistencia, AjustesUbicacionPersistencia>().
                      RegisterType<IAjusteDiligenciarFuentesFinanciacionPersistencia, AjusteDiligenciarFuentesFinanciacionPersistencia>().
                      RegisterType<IAjusteDiligenciarFuentesFinanciacionServicios, AjusteDiligenciarFuentesFinanciacionServicios>().
                      RegisterType<IAjusteRegionalizacionMetasyRecursosServicios, AjusteRegionalizacionMetasyRecursosServicios>().
                      RegisterType<IAjusteRegionalizacionMetasyRecursosPersistencia, AjusteRegionalizacionMetasyRecursosPersistencia>().
                      RegisterType<IAjustesPoliticaTransversalBeneficiarioServicios, AjustesPoliticaTransversalBeneficiarioServicios>().
                      RegisterType<IAjustesPoliticaTransversalBeneficiarioPersistencia, AjustesPoliticaTransversalBeneficiarioPersistencia>().
                      RegisterType<IAjustesCuantificacionBeneficiarioServicios, AjustesCuantificacionBeneficiarioServicios>().
                      RegisterType<IAjustesCuantificacionBeneficiarioPersistencia, AjustesCuantificacionBeneficiarioPersistencia>().
                      RegisterType<IAjustesPoliticaTransversalCategoriaServicios, AjustesPoliticaTransversalCategoriaServicios>().
                      RegisterType<IAjustesPoliticaTransversalCategoriaPersistencia, AjustesPoliticaTransversalCategoriaPersistencia>().
                      RegisterType<IAjusteFocalizacionPoliticasTransversalesAsociacionIndicadoresServicios, AjusteFocalizacionPoliticasTransversalesAsociacionIndicadoresServicios>().
                      RegisterType<IAjusteFocalizacionPoliticasTransversalesAsociacionIndicadoresPersistencia, AjusteFocalizacionPoliticasTransversalesAsociacionIndicadoresPersistencia>().
                      RegisterType<ICostosActividadesServicios, CostosActividadesServicios>().
                      RegisterType<ICostosActividadesPersistencia, CostosActividadesPersistencia>().

                      RegisterType<IFocalizacionPoliticasTransversalesRelacionadasAjustesServicios, FocalizacionPoliticasTransversalesRelacionadasAjutesServicios>().
                      RegisterType<IFocalizacionPoliticasTransversalesRelacionadasAjustesPersistencia, FocalizacionPoliticasTransversalesRelacionadasAjustesPersistencia>().

                      RegisterType<IAjustesPoliticasTransversalesMetasServicios, AjustesPoliticasTransversalesMetasServicios>().
                      RegisterType<IAjustesPoliticasTransversalesMetasPersistencia, AjustesPoliticasTransversalesMetasPersistencia>()


                      .RegisterType<ICostosEntregablesServicios, CostosEntregablesServicios>()
                      .RegisterType<ICostosEntregablesPersistencia, CostosEntregablesPersistencia>()
                      .RegisterType<IDesagregarRegionalizacionServicios, DesagregarRegionalizacionServicios>()
                      .RegisterType<IDesagregarRegionalizacionPersistencia, DesagregarRegionalizacionPersistencia>()
                      //Tramites
                      .RegisterType<ICartaCuerpoDatosDespedidaServicios, CartaConceptoDatosDespediaServicios>()
                      .RegisterType<ICartaConceptoDatosDespediaPersistencia, CartaConceptoDatosDespediaPersistencia>()
                      .RegisterType<ISeccionCapituloPersistencia, SeccionCapituloPersistencia>()
                      .RegisterType<IFasePersistencia, FasePersistencia>()
                       //Politicas cruce politicas
                       .RegisterType<IPoliticasTransversalesCrucePoliticasServicios, PoliticasTransversalesCrucePoliticasServicios>()
                       .RegisterType<IPoliticasTransversalesCrucePoliticasPersistencia, PoliticasTransversalesCrucePoliticasPersistencia>()

                      ;


            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}