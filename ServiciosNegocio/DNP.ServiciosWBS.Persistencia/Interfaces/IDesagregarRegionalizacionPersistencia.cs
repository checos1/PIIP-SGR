namespace DNP.ServiciosWBS.Persistencia.Interfaces
{
    using ServiciosNegocio.Dominio.Dto.Productos;
    using ServiciosNegocio.Comunes.Dto.Formulario;
    public interface IDesagregarRegionalizacionPersistencia
    {
        DesagregarRegionalizacionDto ObtenerDesagregarRegionalizacion(string bpin);
        DesagregarRegionalizacionDto ObtenerDesagregarRegionalizacionPreview();
        void GuardarDefinitivamente(ParametrosGuardarDto<DesagregarRegionalizacionDto> parametrosGuardar, string usuario);
    }
}
