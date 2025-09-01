namespace DNP.ServiciosWBS.Persistencia.Interfaces
{
    using ServiciosNegocio.Dominio.Dto.Proyectos;
    using ServiciosNegocio.Comunes.Dto.Formulario;
    public interface IAjustesCuantificacionBeneficiarioPersistencia
    {
        AjustesCuantificacionBeneficiarioDto ObtenerAjustesCuantificacionBeneficiario(string bpin);
        void ActualizarTemporal(ParametrosConsultaDto parametrosConsultaDto);
        AjustesCuantificacionBeneficiarioDto ObtenerAjustesCuantificacionBeneficiarioPreview();
        void GuardarDefinitivamente(ParametrosGuardarDto<AjustesCuantificacionBeneficiarioDto> parametrosGuardar, string usuario);
    }
}
