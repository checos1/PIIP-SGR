namespace DNP.ServiciosNegocio.Servicios.Interfaces.Formulario
{
    using Comunes.Dto;
    using Comunes.Dto.Formulario;
    using Dominio.Dto.Proyectos;
    using System.Net.Http;

    public interface IProductosServicio
    {

        ProyectoDto ObtenerProductosPreview();
        void Guardar(ParametrosGuardarDto<ProyectoDto> parametrosGuardar, ParametrosAuditoriaDto parametrosAuditoria, bool guardarTemporalmente);
        ParametrosGuardarDto<ProyectoDto> ConstruirParametrosGuardado(HttpRequestMessage request, ProyectoDto contenido);
        ProyectoDto Obtener(ParametrosConsultaDto parametrosConsultaDto);
    }
}