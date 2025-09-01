namespace DNP.ServiciosWBS.Servicios.Interfaces
{
    using System.Net.Http;
    using ServiciosNegocio.Comunes.Dto;
    using ServiciosNegocio.Dominio.Dto.Productos;
    using ServiciosNegocio.Comunes.Dto.Formulario;
    public interface IDesagregarRegionalizacionServicios
    {
        DesagregarRegionalizacionDto ObtenerDesagregarRegionalizacion(ParametrosConsultaDto parametrosConsultaDto);
        DesagregarRegionalizacionDto ObtenerDesagregarRegionalizacionPreview();
        void Guardar(ParametrosGuardarDto<DesagregarRegionalizacionDto> parametrosGuardar, ParametrosAuditoriaDto parametrosAuditoria, bool guardarTemporalmente);
        ParametrosGuardarDto<DesagregarRegionalizacionDto> ConstruirParametrosGuardado(HttpRequestMessage request, DesagregarRegionalizacionDto contenido);
    }
}
