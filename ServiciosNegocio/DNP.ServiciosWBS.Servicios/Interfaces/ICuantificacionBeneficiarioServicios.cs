namespace DNP.ServiciosWBS.Servicios.Interfaces
{
    using System.Net.Http;
    using ServiciosNegocio.Comunes.Dto;
    using ServiciosNegocio.Dominio.Dto.CuantificacionBeneficiario;
    using ServiciosNegocio.Comunes.Dto.Formulario;
    public interface ICuantificacionBeneficiarioServicios
    {
        PoblacionDto ObtenerCuantificacionBeneficiario(ParametrosConsultaDto parametrosConsulta);
        PoblacionDto ObtenerCuantificacionBeneficiarioPreview();
        void Guardar(ParametrosGuardarDto<PoblacionDto> parametrosGuardar, ParametrosAuditoriaDto parametrosAuditoria, bool guardarTemporalmente);
        ParametrosGuardarDto<PoblacionDto> ConstruirParametrosGuardado(HttpRequestMessage request, PoblacionDto contenido);
    }
}
