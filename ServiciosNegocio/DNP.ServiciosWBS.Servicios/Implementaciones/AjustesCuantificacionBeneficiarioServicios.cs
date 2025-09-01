namespace DNP.ServiciosWBS.Servicios.Implementaciones
{
    using Interfaces;
    using Interfaces.Transversales;
    using Persistencia.Interfaces;
    using Persistencia.Interfaces.Transversales;
    using ServiciosNegocio.Comunes;
    using ServiciosNegocio.Dominio.Dto.Proyectos;
    using ServiciosNegocio.Comunes.Dto.Formulario;

    public class AjustesCuantificacionBeneficiarioServicios : ServicioBase<AjustesCuantificacionBeneficiarioDto>, IAjustesCuantificacionBeneficiarioServicios
    {
        private readonly IAjustesCuantificacionBeneficiarioPersistencia _ajustesCuantificacionBeneficiarioPersistencia;

        public AjustesCuantificacionBeneficiarioServicios(IAjustesCuantificacionBeneficiarioPersistencia ajustesCuantificacionBeneficiarioPersistencia,
                         IPersistenciaTemporal persistenciaTemporal, IAuditoriaServicios auditoriaServicios) :
        base(persistenciaTemporal, auditoriaServicios)
        {
            _ajustesCuantificacionBeneficiarioPersistencia = ajustesCuantificacionBeneficiarioPersistencia;
        }

        public AjustesCuantificacionBeneficiarioDto ObtenerAjustesCuantificacionBeneficiario(ParametrosConsultaDto parametrosConsultaDto)
        {
            _ajustesCuantificacionBeneficiarioPersistencia.ActualizarTemporal(parametrosConsultaDto);
            return Obtener(parametrosConsultaDto);
        }

        protected override AjustesCuantificacionBeneficiarioDto ObtenerDefinitivo(ParametrosConsultaDto parametrosConsultaDto)
        {
            AjustesCuantificacionBeneficiarioDto infoPersistencia = _ajustesCuantificacionBeneficiarioPersistencia.ObtenerAjustesCuantificacionBeneficiario(parametrosConsultaDto.Bpin);
            return infoPersistencia;
        }

        public AjustesCuantificacionBeneficiarioDto ObtenerAjustesCuantificacionBeneficiarioPreview()
        {
            return _ajustesCuantificacionBeneficiarioPersistencia.ObtenerAjustesCuantificacionBeneficiarioPreview();
        }

        protected override void GuardadoDefinitivo(ParametrosGuardarDto<AjustesCuantificacionBeneficiarioDto> parametrosGuardar, string usuario)
        {
            _ajustesCuantificacionBeneficiarioPersistencia.GuardarDefinitivamente(parametrosGuardar, usuario);
        }
    }
}
