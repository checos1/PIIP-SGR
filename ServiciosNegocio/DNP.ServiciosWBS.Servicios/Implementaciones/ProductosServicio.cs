namespace DNP.ServiciosWBS.Servicios.Implementaciones
{
    using Interfaces;
    using Interfaces.Transversales;
    using Persistencia.Interfaces;
    using Persistencia.Interfaces.Transversales;
    using ServiciosNegocio.Comunes;
    using ServiciosNegocio.Comunes.Dto.Formulario;
    using ServiciosNegocio.Comunes.Excepciones;
    using ServiciosNegocio.Dominio.Dto.Proyectos;

    public class ProductosServicio : ServicioBase<ProyectoDto>, IProductosServicio
    {
        private readonly IProductoPersistencia _productoPersistencia;

        public ProductosServicio(IProductoPersistencia productoPersistencia,
                                 IPersistenciaTemporal persistenciaTemporal, IAuditoriaServicios auditoriaServicios) :
            base(persistenciaTemporal, auditoriaServicios)
        {
            _productoPersistencia = productoPersistencia;
        }

        public ProyectoDto ObtenerProductos(ParametrosConsultaDto parametrosConsulta)
        {
            return Obtener(parametrosConsulta);
        }

        public ProyectoDto ObtenerProductosPreview()
        {
            return _productoPersistencia.ObtenerProductosPreview();
        }

        protected override ProyectoDto ObtenerDefinitivo(ParametrosConsultaDto parametrosConsultaDto)
        {
            var proyecto = _productoPersistencia.ObtenerProductoDefinitivo(parametrosConsultaDto);

            return proyecto.Bpin != null ? proyecto : throw new ServiciosNegocioException(string.Format(ServiciosNegocioRecursos.ParametroNoExiste, "bpin: " + parametrosConsultaDto.Bpin));
        }

        protected override void GuardadoDefinitivo(ParametrosGuardarDto<ProyectoDto> parametrosGuardar, string usuario)
        {
            _productoPersistencia.GuardarDefinitivamente(parametrosGuardar, usuario);
        }
    }
}
