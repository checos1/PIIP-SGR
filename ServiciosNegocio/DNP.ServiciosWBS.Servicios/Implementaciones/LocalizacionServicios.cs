namespace DNP.ServiciosWBS.Servicios.Implementaciones
{
    using Interfaces;
    using Interfaces.Transversales;
    using Persistencia.Interfaces;
    using Persistencia.Interfaces.Transversales;
    using ServiciosNegocio.Comunes;
    using ServiciosNegocio.Dominio.Dto.Proyectos;
    using ServiciosNegocio.Comunes.Dto.Formulario;
    using DNP.ServiciosNegocio.Dominio.Dto.Tramites;
    using System.Collections.Generic;

    public class LocalizacionServicios : ServicioBase<LocalizacionProyectoDto>, ILocalizacionServicios
    {
        private readonly ILocalizacionPersistencia _localizacionPersistencia;

        public LocalizacionServicios(ILocalizacionPersistencia localizacionPersistencia,
                         IPersistenciaTemporal persistenciaTemporal, IAuditoriaServicios auditoriaServicios) :
        base(persistenciaTemporal, auditoriaServicios)
        {
            _localizacionPersistencia = localizacionPersistencia;
        }

        public LocalizacionProyectoDto ObtenerLocalizacion(ParametrosConsultaDto parametrosConsultaDto)
        {
            _localizacionPersistencia.ActualizarTemporal(parametrosConsultaDto);
            return Obtener(parametrosConsultaDto);
        }

        public LocalizacionProyectoDto ObtenerLocalizacionProyectos(string bpin)
        {
            LocalizacionProyectoDto infoPersistencia = _localizacionPersistencia.Obtenerlocalizacion(bpin);
            return infoPersistencia;
        }


        protected override LocalizacionProyectoDto ObtenerDefinitivo(ParametrosConsultaDto parametrosConsultaDto)
        {
            LocalizacionProyectoDto infoPersistencia = _localizacionPersistencia.Obtenerlocalizacion(parametrosConsultaDto.Bpin);
            return infoPersistencia;
        }

        public LocalizacionProyectoDto ObtenerLocalizacionPreview()
        {
            return _localizacionPersistencia.ObtenerlocalizacionPreview();
        }

        protected override void GuardadoDefinitivo(ParametrosGuardarDto<LocalizacionProyectoDto> parametrosGuardar, string usuario)
        {
            _localizacionPersistencia.GuardarDefinitivamente(parametrosGuardar, usuario);
        }

        public ResultadoProcedimientoDto GuardarLocalizacion(LocalizacionProyectoAjusteDto localizacionProyecto, string usuario)
        {
            return _localizacionPersistencia.GuardarLocalizacion(localizacionProyecto, usuario);
        }
    }
}
