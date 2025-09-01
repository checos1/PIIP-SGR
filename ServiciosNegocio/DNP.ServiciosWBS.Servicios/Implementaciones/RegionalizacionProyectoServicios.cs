namespace DNP.ServiciosWBS.Servicios.Implementaciones
{
    using System.Collections.Generic;
    using System.Linq;
    using Interfaces;
    using Interfaces.Transversales;
    using Persistencia.Interfaces;
    using Persistencia.Interfaces.Transversales;
    using ServiciosNegocio.Comunes.Dto.Formulario;
    using ServiciosNegocio.Dominio.Dto.Formulario;

    public class RegionalizacionProyectoServicios : ServicioBase<RegionalizacionProyectoDto>, IRegionalizacionProyectoServicios
    {
        private readonly IRegionalizacionProyectoPersistencia _regionalizacionPersistencia;

        public RegionalizacionProyectoServicios(IRegionalizacionProyectoPersistencia regionalizacionPersistencia, IPersistenciaTemporal persistenciaTemporal, IAuditoriaServicios auditoriaServicios) : base(persistenciaTemporal, auditoriaServicios)
        {
            _regionalizacionPersistencia = regionalizacionPersistencia;
        }

        public RegionalizacionProyectoDto ObtenerRegionalizacion(ParametrosConsultaDto parametrosConsulta)
        {
            _regionalizacionPersistencia.ActualizarTemporal(parametrosConsulta);
            return Obtener(parametrosConsulta);
        }

        public RegionalizacionProyectoDto ObtenerRegionalizacionPreview()
        {
            return _regionalizacionPersistencia.ObtenerRegionalizacionPreview();
        }

        protected override RegionalizacionProyectoDto ObtenerDefinitivo(ParametrosConsultaDto parametrosConsultaDto)
        {
            RegionalizacionProyectoDto infoPersistencia = _regionalizacionPersistencia.ObtenerRegionalizacion(parametrosConsultaDto.Bpin);
            return infoPersistencia;
        }

        protected override void GuardadoDefinitivo(ParametrosGuardarDto<RegionalizacionProyectoDto> parametrosGuardar,
                                                   string usuario)
        {
            _regionalizacionPersistencia.GuardarDefinitivamente(parametrosGuardar, usuario);
        }
    }
}
