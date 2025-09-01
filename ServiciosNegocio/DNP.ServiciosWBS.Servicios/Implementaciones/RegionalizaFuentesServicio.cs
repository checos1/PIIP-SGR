namespace DNP.ServiciosWBS.Servicios.Implementaciones
{
    using Interfaces;
    using Interfaces.Transversales;
    using Persistencia.Interfaces;
    using Persistencia.Interfaces.Transversales;
    using ServiciosNegocio.Comunes;
    using ServiciosNegocio.Dominio.Dto.FuenteFinanciacion;
    using ServiciosNegocio.Comunes.Dto.Formulario;

    //[ExcludeFromCodeCoverage]
    public class RegionalizaFuentesServicio : ServicioBase<FuenteFinanciacionRegionalizacionDto>, IRegionalizaFuentesServicio
    {
        private readonly IRegionalizaFuentesPersistencia _regionalizacionPersistencia;

        public RegionalizaFuentesServicio(IRegionalizaFuentesPersistencia regionalizacionPersistencia, IPersistenciaTemporal persistenciaTemporal, IAuditoriaServicios auditoriaServicios) : base(persistenciaTemporal, auditoriaServicios)
        {
            _regionalizacionPersistencia = regionalizacionPersistencia;
        }

        public FuenteFinanciacionRegionalizacionDto ObtenerFuenteFinanciacionRegionalizacion(ParametrosConsultaDto parametrosConsulta)
        {
            _regionalizacionPersistencia.ActualizarTemporal(parametrosConsulta);
            return Obtener(parametrosConsulta);
        }

        protected override FuenteFinanciacionRegionalizacionDto ObtenerDefinitivo(ParametrosConsultaDto parametrosConsultaDto)
        {
            FuenteFinanciacionRegionalizacionDto infoPersistencia = _regionalizacionPersistencia.ObtenerFuenteFinanciacionRegionalizacion(parametrosConsultaDto.Bpin);
            return infoPersistencia;
        }

        public FuenteFinanciacionRegionalizacionDto ObtenerFuenteFinanciacionRegionalizacionPreview()
        {
            return _regionalizacionPersistencia.ObtenerFuenteFinanciacionRegionalizacionPreview();
        }

        protected override void GuardadoDefinitivo(ParametrosGuardarDto<FuenteFinanciacionRegionalizacionDto> parametrosGuardar, string usuario)
        {
            _regionalizacionPersistencia.GuardarDefinitivamente(parametrosGuardar, usuario);
        }
    }
}
