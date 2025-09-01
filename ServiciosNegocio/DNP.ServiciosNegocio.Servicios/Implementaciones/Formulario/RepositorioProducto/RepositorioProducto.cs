namespace DNP.ServiciosNegocio.Servicios.Implementaciones.Formulario.RepositorioProducto
{
    using Comunes.Dto.Formulario;
    using Dominio.Dto.Productos;
    using Interfaces.Formulario.RepositorioProducto;
    using Persistencia.Interfaces.Productos;

    public class RepositorioProducto : IRepositorioProducto
    {
        private readonly IProductoPersistencia _productoPersistencia;

        public RepositorioProducto(IProductoPersistencia productoPersistencia)
        {
            _productoPersistencia = productoPersistencia;

        }

        public ProductoDto Obtener(ParametrosConsultaProductoDto parametrosConsultaProductoDto)
        {

            return _productoPersistencia.ObtenerProductoTemporal(parametrosConsultaProductoDto) != null
                       ?  Newtonsoft.Json.JsonConvert.DeserializeObject<ProductoDto>(_productoPersistencia.
                                                                                     ObtenerProductoTemporal(parametrosConsultaProductoDto).Json)
                       : _productoPersistencia.ObtenerProductoDefinitivo(parametrosConsultaProductoDto);
        }
    }
}