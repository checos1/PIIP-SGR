namespace DNP.ServiciosWBS.Servicios.Interfaces
{
    using System.Net.Http;
    using ServiciosNegocio.Comunes.Dto;
    using ServiciosNegocio.Dominio.Dto.Focalizacion;
    using ServiciosNegocio.Comunes.Dto.Formulario;

    public interface IAjustesPoliticaTransversalBeneficiarioServicios
    {
        AjustesPoliticaTBeneficiarioDto ObtenerAjustesPoliticaTransversalBeneficiario(ParametrosConsultaDto parametrosConsultaDto);
        AjustesPoliticaTBeneficiarioDto ObtenerAjustesPoliticaTransversalBeneficiarioPreview();
        void Guardar(ParametrosGuardarDto<AjustesPoliticaTBeneficiarioDto> parametrosGuardar, ParametrosAuditoriaDto parametrosAuditoria, bool guardarTemporalmente);
        ParametrosGuardarDto<AjustesPoliticaTBeneficiarioDto> ConstruirParametrosGuardado(HttpRequestMessage request, AjustesPoliticaTBeneficiarioDto contenido);
    }
}
