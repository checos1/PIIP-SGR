namespace DNP.ServiciosNegocio.Servicios.Interfaces.Proyectos
{
    using System.Net.Http;
    using ServiciosNegocio.Comunes.Dto;
    using ServiciosNegocio.Dominio.Dto.Proyectos;
    using ServiciosNegocio.Comunes.Dto.Formulario;
    public interface IDefinirAlcanceServicios
    {
        AlcanceDto ObtenerDefinirAlcance(ParametrosConsultaDto parametrosConsultaDto);
        AlcanceDto ObtenerDefinirAlcancePreview();
        void Guardar(ParametrosGuardarDto<AlcanceDto> parametrosGuardar, ParametrosAuditoriaDto parametrosAuditoria, bool guardarTemporalmente);
        ParametrosGuardarDto<AlcanceDto> ConstruirParametrosGuardado(HttpRequestMessage request, AlcanceDto contenido);
    }
}
