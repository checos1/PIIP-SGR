namespace DNP.ServiciosNegocio.Persistencia.Interfaces.Productos
{
    using Comunes.Dto.Formulario;
    using Dominio.Dto.Proyectos;

    public interface IProductoPersistencia
    {
        ProyectoDto ObtenerProductoDefinitivo(
            ParametrosConsultaDto parametrosConsultaDto);
        void GuardarDefinitivamente(ParametrosGuardarDto<ProyectoDto> parametrosGuardar, string usuario);
        ProyectoDto ObtenerProductosPreview();

    }
    
}
