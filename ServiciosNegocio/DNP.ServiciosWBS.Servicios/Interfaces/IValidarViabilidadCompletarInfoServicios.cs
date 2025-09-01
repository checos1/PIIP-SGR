namespace DNP.ServiciosWBS.Servicios.Interfaces
{
    using System.Net.Http;
    using ServiciosNegocio.Comunes.Dto;
    using ServiciosNegocio.Comunes.Dto.Formulario;
    using ServiciosNegocio.Dominio.Dto.ValidarViabilidadCompletarInfo;

    public interface IValidarViabilidadCompletarInfoServicios
    {
        ValidarViabilidadCompletarInfoDto ObtenerValidarViabilidadCompletarInfo(ParametrosConsultaDto parametrosConsulta);
        ValidarViabilidadCompletarInfoDto ObtenerValidarViabilidadCompletarInfoPreview();
        void Guardar(ParametrosGuardarDto<ValidarViabilidadCompletarInfoDto> parametrosGuardar, ParametrosAuditoriaDto parametrosAuditoria, bool guardarTemporalmente);
        ParametrosGuardarDto<ValidarViabilidadCompletarInfoDto> ConstruirParametrosGuardado(HttpRequestMessage request, ValidarViabilidadCompletarInfoDto contenido);
    }
}
