namespace DNP.ServiciosWBS.Servicios.Implementaciones
{
    using Interfaces;
    using Interfaces.Transversales;
    using Persistencia.Interfaces;
    using Persistencia.Interfaces.Transversales;
    using ServiciosNegocio.Dominio.Dto.Productos;
    using ServiciosNegocio.Comunes.Dto.Formulario;

    public class DesagregarRegionalizacionServicios : ServicioBase<DesagregarRegionalizacionDto>, IDesagregarRegionalizacionServicios
    {
        private readonly IDesagregarRegionalizacionPersistencia _desagregarRegionalizacionPersistencia;

        public DesagregarRegionalizacionServicios(IDesagregarRegionalizacionPersistencia desagregarRegionalizacionPersistencia,
                         IPersistenciaTemporal persistenciaTemporal, IAuditoriaServicios auditoriaServicios) :
        base(persistenciaTemporal, auditoriaServicios)
        {
            _desagregarRegionalizacionPersistencia = desagregarRegionalizacionPersistencia;
        }

        public DesagregarRegionalizacionDto ObtenerDesagregarRegionalizacion(ParametrosConsultaDto parametrosConsultaDto)
        {
            return ObtenerDefinitivo(parametrosConsultaDto);
        }

        protected override DesagregarRegionalizacionDto ObtenerDefinitivo(ParametrosConsultaDto parametrosConsultaDto)
        {
            DesagregarRegionalizacionDto infoPersistencia = _desagregarRegionalizacionPersistencia.ObtenerDesagregarRegionalizacion(parametrosConsultaDto.Bpin);
            return infoPersistencia;
        }

        public DesagregarRegionalizacionDto ObtenerDesagregarRegionalizacionPreview()
        {
            return _desagregarRegionalizacionPersistencia.ObtenerDesagregarRegionalizacionPreview();
        }

        protected override void GuardadoDefinitivo(ParametrosGuardarDto<DesagregarRegionalizacionDto> parametrosGuardar, string usuario)
        {
            _desagregarRegionalizacionPersistencia.GuardarDefinitivamente(parametrosGuardar, usuario);
        }
    }
}
