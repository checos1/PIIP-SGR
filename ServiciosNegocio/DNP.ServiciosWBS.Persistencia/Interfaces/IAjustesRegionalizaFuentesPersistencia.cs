using DNP.ServiciosNegocio.Dominio.Dto.FuenteFinanciacion;

namespace DNP.ServiciosWBS.Persistencia.Interfaces
{
    using ServiciosNegocio.Comunes.Dto.Formulario;
    public interface IAjustesRegionalizaFuentesPersistencia
    {
        AjustesRegionalizaFuentesDto ObtenerAjustesRegionalizaFuentes(string bpin);
        void ActualizarTemporal(ParametrosConsultaDto parametrosConsultaDto);
        AjustesRegionalizaFuentesDto ObtenerAjustesRegionalizaFuentesPreview();
        void GuardarDefinitivamente(ParametrosGuardarDto<AjustesRegionalizaFuentesDto> parametrosGuardar, string usuario);
    }
}
