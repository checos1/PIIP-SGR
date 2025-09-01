namespace DNP.ServiciosWBS.Servicios.Interfaces
{
    using ServiciosNegocio.Comunes.Dto;
    using ServiciosNegocio.Comunes.Dto.Formulario;
    using ServiciosNegocio.Dominio.Dto.Proyectos;
    using System.Net.Http;

    public interface IProductosServicio
    {
        ProyectoDto ObtenerProductos(ParametrosConsultaDto parametrosConsulta);
        ProyectoDto ObtenerProductosPreview();
        void Guardar(ParametrosGuardarDto<ProyectoDto> parametrosGuardar, ParametrosAuditoriaDto parametrosAuditoria, bool guardarTemporalmente);
        ParametrosGuardarDto<ProyectoDto> ConstruirParametrosGuardado(HttpRequestMessage request, ProyectoDto contenido);
        ProyectoDto Obtener(ParametrosConsultaDto parametrosConsultaDto);
    }
}