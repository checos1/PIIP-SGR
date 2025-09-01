namespace DNP.ServiciosWBS.Persistencia.Interfaces
{
    using ServiciosNegocio.Dominio.Dto.Tramites;
    using ServiciosNegocio.Comunes.Dto.Formulario;
    public interface ICartaConceptoDatosDespediaPersistencia
    {
        DatosConceptoDespedidaDto ObtenerCartaConceptoDatosDespedida(int TramiteId, int plantillaCartaSeccionId);
        DatosConceptoDespedidaDto ObtenerCartaConceptoDatosDespedidaPreview();
        void GuardarDefinitivamente(ParametrosGuardarDto<DatosConceptoDespedidaDto> parametrosGuardar, string usuario);
    }
}
