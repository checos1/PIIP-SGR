namespace DNP.ServiciosWBS.Persistencia.Interfaces
{
    using ServiciosNegocio.Dominio.Dto.Focalizacion;
    using ServiciosNegocio.Comunes.Dto.Formulario;
    public interface IAjustesPoliticaTransversalCategoriaPersistencia
    {
        AjustesPoliticaTCategoriasDto ObtenerAjustesPoliticaTransversalCategoria(string bpin);
        void ActualizarTemporal(ParametrosConsultaDto parametrosConsultaDto);
        AjustesPoliticaTCategoriasDto ObtenerAjustesPoliticaTransversalCategoriaPreview();
        void GuardarDefinitivamente(ParametrosGuardarDto<AjustesPoliticaTCategoriasDto> parametrosGuardar, string usuario);
    }
}
