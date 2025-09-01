namespace DNP.ServiciosWBS.Servicios.Implementaciones
{
    using Interfaces;
    using Interfaces.Transversales;
    using Persistencia.Interfaces;
    using Persistencia.Interfaces.Transversales;
    using ServiciosNegocio.Comunes;
    using ServiciosNegocio.Dominio.Dto.IndicadorProductoAgregar;
    using ServiciosNegocio.Comunes.Dto.Formulario;

    public class RegionalizacionIndicadorAgregarServicios : ServicioBase<RegionalizacionIndicadorAgregarDto>, IRegionalizacionIndicadorAgregarServicios
    {

        private readonly IRegionalizacionIndicadorAgregarPersistencia _regionalizacionIndicadorAgregarPersistencia;


        public RegionalizacionIndicadorAgregarServicios(IRegionalizacionIndicadorAgregarPersistencia regionalizacionIndicadorAgregarPersistencia,
                         IPersistenciaTemporal persistenciaTemporal, IAuditoriaServicios auditoriaServicios) :
        base(persistenciaTemporal, auditoriaServicios)
        {
            _regionalizacionIndicadorAgregarPersistencia = regionalizacionIndicadorAgregarPersistencia;
        }

        public RegionalizacionIndicadorAgregarDto ObtenerRegionalizacionIndicadorAgregar(ParametrosConsultaDto parametrosConsultaDto)
        {
            _regionalizacionIndicadorAgregarPersistencia.ActualizarTemporal(parametrosConsultaDto);
            return Obtener(parametrosConsultaDto);
        }

        protected override RegionalizacionIndicadorAgregarDto ObtenerDefinitivo(ParametrosConsultaDto parametrosConsultaDto)
        {
            RegionalizacionIndicadorAgregarDto infoPersistencia = _regionalizacionIndicadorAgregarPersistencia.ObtenerRegionalizacionIndicadorAgregar(parametrosConsultaDto.Bpin);
            return infoPersistencia;
        }

        public RegionalizacionIndicadorAgregarDto ObtenerRegionalizacionIndicadorAgregarPreview()
        {
            return _regionalizacionIndicadorAgregarPersistencia.ObtenerRegionalizacionIndicadorAgregarPreview();
        }

        protected override void GuardadoDefinitivo(ParametrosGuardarDto<RegionalizacionIndicadorAgregarDto> parametrosGuardar, string usuario)
        {
            _regionalizacionIndicadorAgregarPersistencia.GuardarDefinitivamente(parametrosGuardar, usuario);
        }

    }
}
