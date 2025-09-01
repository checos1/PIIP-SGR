namespace DNP.ServiciosNegocio.Servicios.Implementaciones.Formulario
{
    using System.Net;
    using Comunes;
    using Comunes.Dto.Formulario;
    using Comunes.Excepciones;
    using Dominio.Dto.Proyectos;
    using Interfaces.Formulario;
    using Interfaces.Transversales;
    using Persistencia.Interfaces.Genericos;
    using Persistencia.Interfaces.Productos;

    public class ProductosServicio : ServicioBase<ProyectoDto>, IProductosServicio
    {
        private readonly IProductoPersistencia _productoPersistencia;

        public ProductosServicio(IProductoPersistencia productoPersistencia,
                                 IPersistenciaTemporal persistenciaTemporal, IAuditoriaServicios auditoriaServicios) :
            base(persistenciaTemporal, auditoriaServicios)
        {
            _productoPersistencia = productoPersistencia;
        }

        public ProyectoDto ObtenerProductosPreview()
        {
            return _productoPersistencia.ObtenerProductosPreview();
        }

        protected override ProyectoDto ObtenerDefinitivo(ParametrosConsultaDto parametrosConsultaDto)
        {
            var proyecto = _productoPersistencia.ObtenerProductoDefinitivo(parametrosConsultaDto);

            return proyecto.Bpin != null ? proyecto  : throw new ServiciosNegocioException(string.Format(ServiciosNegocioRecursos.ParametroNoExiste,"bpin: "+ parametrosConsultaDto.Bpin));
        }

        protected override void GuardadoDefinitivo(ParametrosGuardarDto<ProyectoDto> parametrosGuardar , string usuario)
        {
            _productoPersistencia.GuardarDefinitivamente(parametrosGuardar,usuario);
        }



    }
}