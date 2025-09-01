namespace DNP.ServiciosWBS.Web.API.Tests.Configuracion
{
    using Mocks;
    using Servicios.Interfaces;
    using ServiciosNegocio.Comunes.Autorizacion;
    using System.Diagnostics.CodeAnalysis;
    using Unity;

    [ExcludeFromCodeCoverage]
    public class UnityConfig
    {
        public static IUnityContainer Container
        {
            get
            {
                var container = new UnityContainer();

                container.RegisterType<IAutorizacionUtilidades, AutorizacionUtilidadesMock>().
                          RegisterType<IProductosServicio, ProductoServicioMock>().
                          RegisterType<ICadenaValorServicios, CadenaValorServiciosMock>().
                          RegisterType<IRegionalizacionProyectoServicios, RegionalizacionProyectoServiciosMock>().
                          RegisterType<IRegionalizacionIndicadoresServicios, RegionalizacionIndicadoresServiciosMock>().
                          RegisterType<IRegionalizacionProyectoServicios, RegionalizacionProyectoServiciosMock>().
                          RegisterType<IFocalizacionProyectoServicios, FocalizacionProyectoServiciosMock>().
                          RegisterType<ICuantificacionLocalizacionServicios, CuantificacionLocalizacionServiciosMock>().
                          RegisterType<IFocalizacionServicios, FocalizacionServiciosMock>().
                          RegisterType<IFocalizacionCuantificacionLocalizacionServicios, FocalizacionCuantificacionLocalizacionServiciosMock>().
                          RegisterType<IIndicadorProductoAgregarServicios, IndicadorProductoAgregarServiciosMock>().
                          RegisterType<IRegionalizacionIndicadorAgregarServicios, RegionalizacionIndicadorAgregarServiciosMock>().
                          RegisterType<IValidarViabilidadCompletarInfoServicios, ValidarViabilidadCompletarInfoServiciosMock>().
                          RegisterType<ICuantificacionBeneficiarioServicios, CuantificacionBeneficiarioServiciosMock>().
                          RegisterType<ILocalizacionServicios, LocalizacionServiciosMock>().
                          RegisterType<IPoliticaTransversalCategoriaServicios, PoliticaTransversalCategoriaServiciosMock>().
                          RegisterType<IPoliticaTransversalBeneficiarioServicios, PoliticaTransversalBeneficiarioServiciosMock>().
                          RegisterType<IFocalizacionPoliticasTransversalesMetasServicios, FocalizacionPoliticasTransversalesMetasMock>().
                          RegisterType<IIncluirPoliticasServicios, IncluirPoliticasMock>().
                          RegisterType<IRegionalizaFuentesServicio, RegionalizaFuentesServicioMock>().
                          RegisterType<IFocalizacionPoliticasTransversalesAsociacionIndicadoresServicios, FocalizacionPoliticasTransversalesAsociacionIndicadoresMock>().
                          RegisterType<IFocalizacionPoliticasTransversalesRelacionadasServicios, FocalizacionPoliticasTransversalesRelacionadasServiciosMock>().
                          RegisterType<IAjustesUbicacionServicios, AjustesUbicacionServiciosMock>().
                          RegisterType<IAjustesRegionalizaFuentesServicios, AjustesRegionalizaFuentesServiciosMock>().

                          RegisterType<IAjusteRegionalizacionMetasyRecursosServicios, AjusteRegionalizacionMetasYRecursosMock>().
                          RegisterType<IAjustesPoliticaTransversalBeneficiarioServicios, AjustesPoliticaTransversalBeneficiarioServiciosMock>().
                          RegisterType<IAjusteDiligenciarFuentesFinanciacionServicios, FuentesFinanciacionAjusteMock>().
                          RegisterType<IAjustesCuantificacionBeneficiarioServicios, AjustesCuantificacionBeneficiarioServiciosMock>().
                          RegisterType<IAjustesPoliticaTransversalCategoriaServicios, AjustesPoliticaTransversalCategoriaServiciosMock>().
                          RegisterType<IAjustesPoliticasTransversalesMetasServicios, AjustesPoliticasTransversalesMetasServiciosMock>().
                          RegisterType<IAjusteFocalizacionPoliticasTransversalesAsociacionIndicadoresServicios, AjusteFocalizacionPoliticasTransversalesAsociacionIndicadoresMock>().
                          RegisterType<ICostosActividadesServicios, CostosActividadesServiciosMock>()

                          .RegisterType<ICostosEntregablesServicios, CostosEntregablesServiciosMock>()
                          .RegisterType<IFocalizacionPoliticasTransversalesRelacionadasAjustesServicios, FocalizacionPoliticasTransversalesRelacionadasServiciosAjustesMock>()
                          ;


                return container;
            }
        }
    }
}
