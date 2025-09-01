namespace DNP.ServiciosWBS.Servicios.Implementaciones
{
    using Interfaces;
    using Interfaces.Transversales;
    using Persistencia.Interfaces;
    using Persistencia.Interfaces.Transversales;
    using ServiciosNegocio.Comunes;
    using ServiciosNegocio.Dominio.Dto.Proyectos;
    using ServiciosNegocio.Comunes.Dto.Formulario;

    public class AjustesUbicacionServicios : ServicioBase<AjustesUbicacionDto>, IAjustesUbicacionServicios
    {
        private readonly IAjustesUbicacionPersistencia _ajustesUbicacionPersistencia;

        public AjustesUbicacionServicios(IAjustesUbicacionPersistencia ajustesUbicacionPersistencia,
                         IPersistenciaTemporal persistenciaTemporal, IAuditoriaServicios auditoriaServicios) :
        base(persistenciaTemporal, auditoriaServicios)
        {
            _ajustesUbicacionPersistencia = ajustesUbicacionPersistencia;
        }

        public AjustesUbicacionDto ObtenerAjustesUbicacion(ParametrosConsultaDto parametrosConsultaDto)
        {
            _ajustesUbicacionPersistencia.ActualizarTemporal(parametrosConsultaDto);
            return Obtener(parametrosConsultaDto);
        }

        protected override AjustesUbicacionDto ObtenerDefinitivo(ParametrosConsultaDto parametrosConsultaDto)
        {
            AjustesUbicacionDto infoPersistencia = _ajustesUbicacionPersistencia.ObtenerAjustesUbicacion(parametrosConsultaDto.Bpin);
            return infoPersistencia;
        }

        public AjustesUbicacionDto ObtenerAjustesUbicacionPreview()
        {
            return _ajustesUbicacionPersistencia.ObtenerAjustesUbicacionPreview();
        }

        protected override void GuardadoDefinitivo(ParametrosGuardarDto<AjustesUbicacionDto> parametrosGuardar, string usuario)
        {
            _ajustesUbicacionPersistencia.GuardarDefinitivamente(parametrosGuardar, usuario);
        }
    }
}
