namespace DNP.ServiciosWBS.Servicios.Interfaces
{
    using System.Net.Http;
    using ServiciosNegocio.Comunes.Dto;
    using ServiciosNegocio.Dominio.Dto.FuenteFinanciacion;
    using ServiciosNegocio.Comunes.Dto.Formulario;
    public interface IAjustesRegionalizaFuentesServicios
    {
        AjustesRegionalizaFuentesDto ObtenerAjustesRegionalizaFuentes(ParametrosConsultaDto parametrosConsulta);
        AjustesRegionalizaFuentesDto ObtenerAjustesRegionalizaFuentesPreview();
        ParametrosGuardarDto<AjustesRegionalizaFuentesDto> ConstruirParametrosGuardado(HttpRequestMessage request, AjustesRegionalizaFuentesDto contenido);
        void Guardar(ParametrosGuardarDto<AjustesRegionalizaFuentesDto> parametrosGuardar, ParametrosAuditoriaDto parametrosAuditoria, bool guardarTemporalmente);
    }
}
