namespace DNP.ServiciosWBS.Servicios.Interfaces
{
    using System.Net.Http;
    using ServiciosNegocio.Comunes.Dto;
    using ServiciosNegocio.Dominio.Dto.Proyectos;
    using ServiciosNegocio.Comunes.Dto.Formulario;
    public interface IAjustesUbicacionServicios
    {
        AjustesUbicacionDto ObtenerAjustesUbicacion(ParametrosConsultaDto parametrosConsultaDto);
        AjustesUbicacionDto ObtenerAjustesUbicacionPreview();
        void Guardar(ParametrosGuardarDto<AjustesUbicacionDto> parametrosGuardar, ParametrosAuditoriaDto parametrosAuditoria, bool guardarTemporalmente);
        ParametrosGuardarDto<AjustesUbicacionDto> ConstruirParametrosGuardado(HttpRequestMessage request, AjustesUbicacionDto contenido);
    }
}
