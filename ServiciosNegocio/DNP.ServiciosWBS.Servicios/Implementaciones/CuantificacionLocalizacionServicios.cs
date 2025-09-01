namespace DNP.ServiciosWBS.Servicios.Implementaciones
{
    using Interfaces;
    using Interfaces.Transversales;
    using Persistencia.Interfaces;
    using Persistencia.Interfaces.Transversales;
    using ServiciosNegocio.Comunes;
    using ServiciosNegocio.Dominio.Dto.Poblacion;
    using ServiciosNegocio.Comunes.Dto.Formulario;

    public class CuantificacionLocalizacionServicios : ServicioBase<PoblacionDto>, ICuantificacionLocalizacionServicios
    {
        private readonly ICuantificacionLocalizacionPersistencia _cuantificacionLocalizacionPersistencia;

        public CuantificacionLocalizacionServicios(ICuantificacionLocalizacionPersistencia cuantificacionLocalizacionPersistencia,
                         IPersistenciaTemporal persistenciaTemporal, IAuditoriaServicios auditoriaServicios) :
        base(persistenciaTemporal, auditoriaServicios)
        {
            _cuantificacionLocalizacionPersistencia = cuantificacionLocalizacionPersistencia;
        }
        
        public PoblacionDto ObtenerCuantificacionLocalizacion(ParametrosConsultaDto parametrosConsultaDto)
        {
            _cuantificacionLocalizacionPersistencia.ActualizarTemporal(parametrosConsultaDto);
            return Obtener(parametrosConsultaDto);
        }

        protected override PoblacionDto ObtenerDefinitivo(ParametrosConsultaDto parametrosConsultaDto)
        {
            PoblacionDto infoPersistencia = _cuantificacionLocalizacionPersistencia.ObtenerCuantificacionLocalizacion(parametrosConsultaDto.Bpin);
            return infoPersistencia;
        }

        public PoblacionDto ObtenerCuantificacionLocalizacionPreview()
        {
            return _cuantificacionLocalizacionPersistencia.ObtenerCuantificacionLocalizacionPreview();
        }


        protected override void GuardadoDefinitivo(ParametrosGuardarDto<PoblacionDto> parametrosGuardar, string usuario)
        {
            _cuantificacionLocalizacionPersistencia.GuardarDefinitivamente(parametrosGuardar, usuario);
        }
    }
}
