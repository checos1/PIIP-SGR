namespace DNP.ServiciosWBS.Persistencia.Interfaces
{
    using ServiciosNegocio.Dominio.Dto.Proyectos;
    using ServiciosNegocio.Comunes.Dto.Formulario;
    public interface IAjustesUbicacionPersistencia
    {
        AjustesUbicacionDto ObtenerAjustesUbicacion(string bpin);
        void ActualizarTemporal(ParametrosConsultaDto parametrosConsultaDto);
        AjustesUbicacionDto ObtenerAjustesUbicacionPreview();
        void GuardarDefinitivamente(ParametrosGuardarDto<AjustesUbicacionDto> parametrosGuardar, string usuario);
    }
}
