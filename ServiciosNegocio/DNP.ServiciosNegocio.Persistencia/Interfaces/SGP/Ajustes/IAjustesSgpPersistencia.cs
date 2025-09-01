using DNP.ServiciosNegocio.Dominio.Dto.CadenaValor;
using DNP.ServiciosNegocio.Dominio.Dto.Catalogos;
using DNP.ServiciosNegocio.Dominio.Dto.FuenteFinanciacion;
using DNP.ServiciosNegocio.Dominio.Dto.Genericos;
using DNP.ServiciosNegocio.Dominio.Dto.Proyectos;
using DNP.ServiciosNegocio.Dominio.Dto.SGP.Ajustes;
using DNP.ServiciosNegocio.Dominio.Dto.SGP.Transversales;
using System.Collections.Generic;

namespace DNP.ServiciosNegocio.Persistencia.Interfaces.SGP.Ajustes
{
    public interface IAjustesSgpPersistencia
    {
        #region Horizonte

        EncabezadoSGPDto ObtenerHorizonteSgp(ParametrosEncabezadoSGP parametros);
        RespuestaGeneralDto ActualizarHorizonteSgp(HorizonteProyectoDto datosHorizonteProyecto, string usuario);
        string ObtenerCambiosJustificacionHorizonteSgp(int IdProyecto);

        #endregion

        #region Indicadores

        IndicadorProductoDto ObtenerIndicadoresProductoSgp(string bpin);
        IndicadorResponse GuardarIndicadoresSecundariosSgp(AgregarIndicadoresSecundariosDto parametrosGuardar, string usuario);
        IndicadorResponse EliminarIndicadorProductoSgp(int indicadorId, string usuario);
        IndicadorResponse ActualizarMetaAjusteIndicadorSgp(IndicadoresIndicadorProductoDto Indicador, string usuario);

        #endregion

        #region Beneficiarios

        string ObtenerProyectosBeneficiariosSgp(string Bpin);
        string ObtenerProyectosBeneficiariosDetalleSgp(string Json);
        void GuardarBeneficiarioTotalesSgp(BeneficiarioTotalesDto beneficiario, string usuario);
        void GuardarBeneficiarioProductoSgp(BeneficiarioProductoSgpDto beneficiario, string usuario);
        void GuardarBeneficiarioProductoLocalizacionSgp(BeneficiarioProductoLocalizacionDto beneficiario, string usuario);
        void GuardarBeneficiarioProductoLocalizacionCaracterizacionSgp(BeneficiarioProductoLocalizacionCaracterizacionDto beneficiario, string usuario);
        #endregion

        #region Localizaciones

        ResultadoProcedimientoDto GuardarLocalizacionSgp(LocalizacionProyectoAjusteDto localizacionProyecto, string usuario);

        #endregion
        #region fuentefinanciacion
        string FuentesFinanciacionRecursosAjustesAgregarSgp(FuenteFinanciacionAgregarAjusteDto objFuenteFinanciacionAgregarAjusteDto, string usuario);
        #endregion
        #region costos
        ObjectivosAjusteDto ObtenerResumenObjetivosProductosActividadesSgp(string bpin);
        void GuardarAjusteCostoActividadesSgp(ProductoAjusteDto producto, string usuario);
        void AgregarEntregableSgp(AgregarEntregable[] entregables, string usuario);
        void EliminarEntregableSgp(EntregablesActividadesDto entregable);
        #endregion
        #region regionalizacion
        Dominio.Dto.CadenaValor.RegionalizacionDto RegionalizacionGeneralSgp(string bpin);
        #endregion
        #region recursos
        List<CatalogoDto> ConsultarTiposRecursosEntidadSgp(int entityTypeCatalogId, int entityType);
        #endregion
        string ObtenerCategoriasFocalizacionJustificacionSgp(string bpin);
        string ObtenerDetalleCategoriasFocalizacionJustificacionSgp(string bpin);
    }
}
