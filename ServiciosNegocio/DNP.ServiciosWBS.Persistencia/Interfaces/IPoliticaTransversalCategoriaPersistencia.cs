namespace DNP.ServiciosWBS.Persistencia.Interfaces
{
    using ServiciosNegocio.Dominio.Dto.Focalizacion;
    using ServiciosNegocio.Comunes.Dto.Formulario;
    public interface IPoliticaTransversalCategoriaPersistencia
    {
        PoliticaTCategoriasDto ObtenerPoliticaTransversalCategoria(string bpin);
        void ActualizarTemporal(ParametrosConsultaDto parametrosConsultaDto);
        PoliticaTCategoriasDto ObtenerPoliticaTransversalCategoriaPreview();
        void GuardarDefinitivamente(ParametrosGuardarDto<PoliticaTCategoriasDto> parametrosGuardar, string usuario);
    }
}
