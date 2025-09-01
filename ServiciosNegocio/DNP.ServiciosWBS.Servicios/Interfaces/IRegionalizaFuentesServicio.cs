namespace DNP.ServiciosWBS.Servicios.Interfaces
{
    using System.Net.Http;
    using ServiciosNegocio.Comunes.Dto;
    using ServiciosNegocio.Dominio.Dto.FuenteFinanciacion;
    using ServiciosNegocio.Comunes.Dto.Formulario;
    public interface IRegionalizaFuentesServicio
    {
        FuenteFinanciacionRegionalizacionDto ObtenerFuenteFinanciacionRegionalizacion(ParametrosConsultaDto parametrosConsulta);
        FuenteFinanciacionRegionalizacionDto ObtenerFuenteFinanciacionRegionalizacionPreview();
        ParametrosGuardarDto<FuenteFinanciacionRegionalizacionDto> ConstruirParametrosGuardado(HttpRequestMessage request, FuenteFinanciacionRegionalizacionDto contenido);
        void Guardar(ParametrosGuardarDto<FuenteFinanciacionRegionalizacionDto> parametrosGuardar, ParametrosAuditoriaDto parametrosAuditoria, bool guardarTemporalmente);
    }
}
