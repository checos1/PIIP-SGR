namespace DNP.ServiciosWBS.Persistencia.Interfaces
{
    using ServiciosNegocio.Comunes.Dto.Formulario;
    using ServiciosNegocio.Dominio.Dto.Proyectos;

    public interface IProductoPersistencia
    {
        ProyectoDto ObtenerProductoDefinitivo(
            ParametrosConsultaDto parametrosConsultaDto);
        void GuardarDefinitivamente(ParametrosGuardarDto<ProyectoDto> parametrosGuardar, string usuario);
        ProyectoDto ObtenerProductosPreview();

    }
}