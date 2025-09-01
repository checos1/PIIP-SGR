namespace DNP.ServiciosWBS.Servicios.Implementaciones
{
    using Interfaces;
    using Interfaces.Transversales;
    using Persistencia.Interfaces;
    using Persistencia.Interfaces.Transversales;
    using ServiciosNegocio.Comunes;
    using ServiciosNegocio.Dominio.Dto.Focalizacion;
    using ServiciosNegocio.Comunes.Dto.Formulario;
    public class FocalizacionCuantificacionLocalizacionServicios : ServicioBase<FocalizacionCuantificacionLocalizacionDto>, IFocalizacionCuantificacionLocalizacionServicios
    {
        private readonly IFocalizacionCuantificacionLocalizacionPersistencia _focalizacionCuantificacionLocalizacionPersistencia;

        public FocalizacionCuantificacionLocalizacionServicios(IFocalizacionCuantificacionLocalizacionPersistencia focalizacionCuantificacionLocalizacionPersistencia,
                         IPersistenciaTemporal persistenciaTemporal, IAuditoriaServicios auditoriaServicios) :
        base(persistenciaTemporal, auditoriaServicios)
        {
            _focalizacionCuantificacionLocalizacionPersistencia = focalizacionCuantificacionLocalizacionPersistencia;
        }

        public FocalizacionCuantificacionLocalizacionDto ObtenerFocalizacionCuantificacionLocalizacion(ParametrosConsultaDto parametrosConsultaDto)
        {
            _focalizacionCuantificacionLocalizacionPersistencia.ActualizarTemporal(parametrosConsultaDto);
            return Obtener(parametrosConsultaDto);
        }

        protected override FocalizacionCuantificacionLocalizacionDto ObtenerDefinitivo(ParametrosConsultaDto parametrosConsultaDto)
        {
            FocalizacionCuantificacionLocalizacionDto infoPersistencia = _focalizacionCuantificacionLocalizacionPersistencia.ObtenerFocalizacionCuantificacionLocalizacion(parametrosConsultaDto.Bpin);
            return infoPersistencia;
        }


        public FocalizacionCuantificacionLocalizacionDto ObtenerFocalizacionCuantificacionLocalizacionPreview()
        {
            return _focalizacionCuantificacionLocalizacionPersistencia.ObtenerFocalizacionCuantificacionLocalizacionPreview();
        }

        protected override void GuardadoDefinitivo(ParametrosGuardarDto<FocalizacionCuantificacionLocalizacionDto> parametrosGuardar, string usuario)
        {
            _focalizacionCuantificacionLocalizacionPersistencia.GuardarDefinitivamente(parametrosGuardar, usuario);
        }
    }
}
