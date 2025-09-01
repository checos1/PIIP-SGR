namespace DNP.ServiciosWBS.Servicios.Implementaciones
{
    using Interfaces;
    using Interfaces.Transversales;
    using Persistencia.Interfaces;
    using Persistencia.Interfaces.Transversales;
    using ServiciosNegocio.Comunes;
    using ServiciosNegocio.Dominio.Dto.CuantificacionBeneficiario;
    using ServiciosNegocio.Comunes.Dto.Formulario;

    public class CuantificacionBeneficiarioServicios : ServicioBase<PoblacionDto>, ICuantificacionBeneficiarioServicios
    {
        private readonly ICuantificacionBeneficiarioPersistencia _cuantificacionBeneficiarioPersistencia;

        public CuantificacionBeneficiarioServicios(ICuantificacionBeneficiarioPersistencia cuantificacionBeneficiarioPersistencia,
                         IPersistenciaTemporal persistenciaTemporal, IAuditoriaServicios auditoriaServicios) :
        base(persistenciaTemporal, auditoriaServicios)
        {
            _cuantificacionBeneficiarioPersistencia = cuantificacionBeneficiarioPersistencia;
        }

        public PoblacionDto ObtenerCuantificacionBeneficiario(ParametrosConsultaDto parametrosConsultaDto)
        {
            _cuantificacionBeneficiarioPersistencia.ActualizarTemporal(parametrosConsultaDto);
            return Obtener(parametrosConsultaDto);
        }

        protected override PoblacionDto ObtenerDefinitivo(ParametrosConsultaDto parametrosConsultaDto)
        {
            PoblacionDto infoPersistencia = _cuantificacionBeneficiarioPersistencia.ObtenerCuantificacionBeneficiario(parametrosConsultaDto.Bpin);
            return infoPersistencia;
        }

        public PoblacionDto ObtenerCuantificacionBeneficiarioPreview()
        {
            return _cuantificacionBeneficiarioPersistencia.ObtenerCuantificacionBeneficiarioPreview();
        }

        protected override void GuardadoDefinitivo(ParametrosGuardarDto<PoblacionDto> parametrosGuardar, string usuario)
        {
            _cuantificacionBeneficiarioPersistencia.GuardarDefinitivamente(parametrosGuardar, usuario);
        }
    }
}
