namespace DNP.ServiciosWBS.Persistencia.Interfaces
{
    using ServiciosNegocio.Dominio.Dto.Focalizacion;
    using ServiciosNegocio.Comunes.Dto.Formulario;

    public interface IAjustesPoliticaTransversalBeneficiarioPersistencia
    {
        AjustesPoliticaTBeneficiarioDto ObtenerAjustesPoliticaTransversalBeneficiario(string bpin);
        void ActualizarTemporal(ParametrosConsultaDto parametrosConsultaDto);
        AjustesPoliticaTBeneficiarioDto ObtenerAjustesPoliticaTransversalBeneficiarioPreview();
        void GuardarDefinitivamente(ParametrosGuardarDto<AjustesPoliticaTBeneficiarioDto> parametrosGuardar, string usuario);
    }
}
