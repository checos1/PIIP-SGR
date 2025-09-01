namespace DNP.ServiciosWBS.Persistencia.Interfaces
{
    using DNP.ServiciosNegocio.Comunes.Dto.Formulario;
    using DNP.ServiciosNegocio.Dominio.Dto.Focalizacion;
    public interface IAjustesPoliticasTransversalesMetasPersistencia
    {
        AjustesPoliticaTMetasDto ObtenerAjustesPoliticasTransversalesMetas(string bpin);
        AjustesPoliticaTMetasDto ObtenerAjustesPoliticasTransversalesMetasPreview();
        void GuardarDefinitivamente(ParametrosGuardarDto<AjustesPoliticaTMetasDto> parametrosGuardar, string usuario);
        void ActualizarTemporal(ParametrosConsultaDto parametrosConsultaDto);
    }
}
