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
    using ServiciosNegocio.Dominio.Dto.Formulario;

    public class RegionalizacionIndicadoresServicios : ServicioBase<RegionalizacionIndicadorDto>, IRegionalizacionIndicadoresServicios
    {
        private readonly IRegionalizacionIndicadoresPersistencia _regionalizacionIndicadoresPersistencia;

        public RegionalizacionIndicadoresServicios(IRegionalizacionIndicadoresPersistencia regionalizacionIndicadoresPersistencia,
                         IPersistenciaTemporal persistenciaTemporal, IAuditoriaServicios auditoriaServicios) :
        base(persistenciaTemporal, auditoriaServicios)
        {
            _regionalizacionIndicadoresPersistencia = regionalizacionIndicadoresPersistencia;
        }


        public RegionalizacionIndicadorDto ObtenerRegionalizacionIndicadores(ParametrosConsultaDto parametrosConsultaDto)
        {
            _regionalizacionIndicadoresPersistencia.ActualizarTemporal(parametrosConsultaDto);
            return Obtener(parametrosConsultaDto);
        }

        public RegionalizacionIndicadorDto ObtenerRegionalizacionIndicadoresPreview()
        {
            return _regionalizacionIndicadoresPersistencia.ObtenerRegionalizacionIndicadoresPreview();
        }


        protected override RegionalizacionIndicadorDto ObtenerDefinitivo(ParametrosConsultaDto parametrosConsultaDto)
        {
            RegionalizacionIndicadorDto infoPersistencia = _regionalizacionIndicadoresPersistencia.ObtenerRegionalizacionIndicadores(parametrosConsultaDto.Bpin);
            return infoPersistencia;
        }

        protected override void GuardadoDefinitivo(ParametrosGuardarDto<RegionalizacionIndicadorDto> parametrosGuardar, string usuario)
        {
            _regionalizacionIndicadoresPersistencia.GuardarDefinitivamente(parametrosGuardar, usuario);
        }


    }
}
