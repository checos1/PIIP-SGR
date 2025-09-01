namespace DNP.ServiciosWBS.Servicios.Interfaces
{
    using System.Net.Http;
    using ServiciosNegocio.Comunes.Dto;
    using ServiciosNegocio.Dominio.Dto.Focalizacion;
    using ServiciosNegocio.Comunes.Dto.Formulario;
    public interface IPoliticaTransversalBeneficiarioServicios
    {
        PoliticaTBeneficiarioDto ObtenerPoliticaTransversalBeneficiario(ParametrosConsultaDto parametrosConsultaDto);
        PoliticaTBeneficiarioDto ObtenerPoliticaTransversalBeneficiarioPreview();
        void Guardar(ParametrosGuardarDto<PoliticaTBeneficiarioDto> parametrosGuardar, ParametrosAuditoriaDto parametrosAuditoria, bool guardarTemporalmente);
        ParametrosGuardarDto<PoliticaTBeneficiarioDto> ConstruirParametrosGuardado(HttpRequestMessage request, PoliticaTBeneficiarioDto contenido);
    }
}
