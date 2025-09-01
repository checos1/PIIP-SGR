namespace DNP.ServiciosWBS.Servicios.Interfaces
{
    using System.Net.Http;
    using ServiciosNegocio.Comunes.Dto;
    using ServiciosNegocio.Dominio.Dto.Proyectos;
    using ServiciosNegocio.Comunes.Dto.Formulario;
    public interface IAjustesCuantificacionBeneficiarioServicios
    {
        AjustesCuantificacionBeneficiarioDto ObtenerAjustesCuantificacionBeneficiario(ParametrosConsultaDto parametrosConsulta);
        AjustesCuantificacionBeneficiarioDto ObtenerAjustesCuantificacionBeneficiarioPreview();
        void Guardar(ParametrosGuardarDto<AjustesCuantificacionBeneficiarioDto> parametrosGuardar, ParametrosAuditoriaDto parametrosAuditoria, bool guardarTemporalmente);
        ParametrosGuardarDto<AjustesCuantificacionBeneficiarioDto> ConstruirParametrosGuardado(HttpRequestMessage request, AjustesCuantificacionBeneficiarioDto contenido);
    }
}
