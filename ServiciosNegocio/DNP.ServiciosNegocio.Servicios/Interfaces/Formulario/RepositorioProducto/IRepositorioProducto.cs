namespace DNP.ServiciosNegocio.Servicios.Interfaces.Formulario.RepositorioProducto
{
    using Comunes.Dto.Formulario;
    using Dominio.Dto.Productos;

    public interface IRepositorioProducto
    {
        ProductoDto Obtener(ParametrosConsultaProductoDto parametrosConsultaProductoDto);
    }
}