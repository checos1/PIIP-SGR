namespace DNP.ServiciosWBS.Persistencia.Interfaces
{
    using ServiciosNegocio.Dominio.Dto.Focalizacion;
    using ServiciosNegocio.Comunes.Dto.Formulario;
    public interface IFocalizacionCuantificacionLocalizacionPersistencia
    {
        FocalizacionCuantificacionLocalizacionDto ObtenerFocalizacionCuantificacionLocalizacion(string bpin);
        void ActualizarTemporal(ParametrosConsultaDto parametrosConsultaDto);
        FocalizacionCuantificacionLocalizacionDto ObtenerFocalizacionCuantificacionLocalizacionPreview();
        void GuardarDefinitivamente(ParametrosGuardarDto<FocalizacionCuantificacionLocalizacionDto> parametrosGuardar, string usuario);

    }
}
