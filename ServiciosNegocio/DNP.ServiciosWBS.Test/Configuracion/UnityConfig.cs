namespace DNP.ServiciosWBS.Test.Configuracion
{
    using Mocks;
    using Persistencia.Implementaciones;
    using Persistencia.Interfaces;
    using Persistencia.Interfaces.Transversales;
    using ServiciosWBS.Servicios.Implementaciones;
    using ServiciosWBS.Servicios.Interfaces;
    using ServiciosWBS.Servicios.Interfaces.Transversales;
    using Unity;

    public class UnityConfig
    {
        public static IUnityContainer Container
        {
            get
            {
                var container = new UnityContainer();

                container.RegisterType<IAuditoriaServicios, AuditoriaServiciosMock>().
                          RegisterType<IContextoFactory, ContextoFactory>().
                          RegisterType<IProductoPersistencia, ProductoPersistenciaMock>().
                          RegisterType<IPersistenciaTemporal, PersistenciaTemporalMock>().
                          RegisterType<IProductosServicio, ProductosServicio>().
                          RegisterType<ICadenaValorServicios, CadenaValorServicios>().
                          RegisterType<ICadenaValorPersistencia, CadenaValorPersistenciaMock>().
                          RegisterType<IRegionalizacionProyectoServicios, RegionalizacionProyectoServicios>().
                          RegisterType<IRegionalizacionProyectoPersistencia, RegionalizacionPersistenciaMock>().
                          RegisterType<IRegionalizacionIndicadoresServicios, RegionalizacionIndicadoresServicios>().
                          RegisterType<IRegionalizacionIndicadoresPersistencia, RegionalizacionIndicadoresPersistenciaMock>().
                          RegisterType<IFocalizacionProyectoServicios, FocalizacionProyectoServicios>().
                          RegisterType<IFocalizacionProyectoPersistencia, FocalizacionPersistenciaMock>().
                          RegisterType<IFocalizacionServicios, FocalizacionServicios>().
                          RegisterType<IFocalizacionPersistencia, FocalizacionProyectoPersistenciaMock>().
                          RegisterType<ICuantificacionLocalizacionServicios, CuantificacionLocalizacionServicios>().
                          RegisterType<ICuantificacionLocalizacionPersistencia, CuantificacionLocalizacionPersistenciaMock>().
                          RegisterType<IFocalizacionCuantificacionLocalizacionServicios, FocalizacionCuantificacionLocalizacionServicios>().
                          RegisterType<IFocalizacionCuantificacionLocalizacionPersistencia, FocalizacionCuantificacionLocalizacionPersistenciaMock>().
                          RegisterType<IIndicadorProductoAgregarServicios, IndicadorProductoAgregarServicios>().
                          RegisterType<IIndicadorProductoAgregarPersistencia, IndicadorProductoAgregarPersistenciaMock>().
                          RegisterType<IRegionalizacionIndicadorAgregarServicios, RegionalizacionIndicadorAgregarServicios>().
                          RegisterType<IRegionalizacionIndicadorAgregarPersistencia, RegionalizacionIndicadorAgregarPersistenciaMock>().
                          RegisterType<IValidarViabilidadCompletarInfoServicios, ValidarViabilidadCompletarInfoServicios>().
                          RegisterType<IValidarViabilidadCompletarInfoPersistencia, ValidarViabilidadCompletarInfoPersistenciaMock>().
                          RegisterType<IDiligenciarFuentes, DiligenciarFuentesServicios>().
                          RegisterType<IDiligenciarFuentesPersistencia, DiligenciarFuentesPersistencia>().
                          RegisterType<IValidarViabilidadCompletarInfoServicios, ValidarViabilidadCompletarInfoServicios>().
                          RegisterType<IValidarViabilidadCompletarInfoPersistencia, ValidarViabilidadCompletarInfoPersistenciaMock>().
                          RegisterType<ICuantificacionBeneficiarioServicios, CuantificacionBeneficiarioServicios>().
                          RegisterType<ICuantificacionBeneficiarioPersistencia, CuantificacionBeneficiarioPersistenciaMock>().
                          RegisterType<IFocalizacionPoliticasTransversalesServicios, FocalizacionPoliticasTransversalesServicios>().
                          RegisterType<IFocalizacionPoliticasTransversalesPersistencia, FocalizacionPoliticasTransversalesPersistencia>().
                          RegisterType<IRegionalizacionMetasyRecursosServicios, RegionalizacionMetasyRecursosServicios>().
                          RegisterType<IRegionalizacionMetasyRecursosPersistencia, RegionalizacionMetasyRecursosPersistencia>().
                          RegisterType<ILocalizacionServicios, LocalizacionServicios>().
                          RegisterType<ILocalizacionPersistencia, LocalizacionPersistenciaMock>().
                          RegisterType<IPoliticaTransversalCategoriaServicios, PoliticaTransversalCategoriaServicios>().
                          RegisterType<IPoliticaTransversalCategoriaPersistencia, PoliticaTransversalCategoriaPersistenciaMock>().
                          RegisterType<IRegionalizaFuentesServicio, RegionalizaFuentesServicio>().
                          RegisterType<IRegionalizaFuentesPersistencia, RegionalizaFuentesPersistenciaMock>().
                          RegisterType<IPoliticaTransversalBeneficiarioServicios, PoliticaTransversalBeneficiarioServicios>().
                          RegisterType<IPoliticaTransversalBeneficiarioPersistencia, PoliticaTransversalBeneficiarioPersistenciaMock>().
                          RegisterType<IAjustesUbicacionServicios, AjustesUbicacionServicios>().
                          RegisterType<IAjustesUbicacionPersistencia, AjustesUbicacionPersistenciaMock>().
                          RegisterType<IAjustesRegionalizaFuentesServicios, AjustesRegionalizaFuentesServicios>().
                          RegisterType<IAjustesRegionalizaFuentesPersistencia, AjustesRegionalizaFuentesPersistenciaMock>().
                          RegisterType<IAjustesPoliticaTransversalBeneficiarioServicios, AjustesPoliticaTransversalBeneficiarioServicios>().
                          RegisterType<IAjustesPoliticaTransversalBeneficiarioPersistencia, AjustesPoliticaTransversalBeneficiarioPersistenciaMock>().
                  
                         
                          RegisterType<IAjustesCuantificacionBeneficiarioServicios, AjustesCuantificacionBeneficiarioServicios>().
                          RegisterType<IAjustesCuantificacionBeneficiarioPersistencia, AjustesCuantificacionBeneficiarioPersistenciaMock>().
                          RegisterType<IAjustesPoliticaTransversalCategoriaServicios, AjustesPoliticaTransversalCategoriaServicios>().
                          RegisterType<IAjustesPoliticaTransversalCategoriaPersistencia, AjustesPoliticaTransversalCategoriaPersistenciaMock>().
                          RegisterType<IAjustesPoliticasTransversalesMetasServicios, AjustesPoliticasTransversalesMetasServicios>().
                          RegisterType<IAjustesPoliticasTransversalesMetasPersistencia, AjustesPoliticasTransversalesMetasPersistenciaMock>().
                          RegisterType<ICostosActividadesServicios, CostosActividadesServicios>().
                          RegisterType<ICostosActividadesPersistencia, CostosActividadesPersistenciaMock>()

                          .RegisterType<ICostosEntregablesServicios, CostosEntregablesServicios>().
                          RegisterType<ICostosEntregablesPersistencia, CostosEntregablesPersistenciaMock>()
                          ;

                return container;
            }
        }
    }
}
