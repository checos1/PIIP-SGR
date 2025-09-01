namespace DNP.ServiciosWBS.Servicios.Implementaciones
{
    using Interfaces;
    using Interfaces.Transversales;
    using Persistencia.Interfaces;
    using Persistencia.Interfaces.Transversales;
    using ServiciosNegocio.Comunes;
    using ServiciosNegocio.Dominio.Dto.Focalizacion;
    using ServiciosNegocio.Comunes.Dto.Formulario;

    public class AjustesPoliticaTransversalBeneficiarioServicios : ServicioBase<AjustesPoliticaTBeneficiarioDto>, IAjustesPoliticaTransversalBeneficiarioServicios
    {
        private readonly IAjustesPoliticaTransversalBeneficiarioPersistencia _politicaTransversalBeneficiarioPersistencia;

        public AjustesPoliticaTransversalBeneficiarioServicios(IAjustesPoliticaTransversalBeneficiarioPersistencia politicaTransversalBeneficiarioPersistencia,
                         IPersistenciaTemporal persistenciaTemporal, IAuditoriaServicios auditoriaServicios) :
        base(persistenciaTemporal, auditoriaServicios)
        {
            _politicaTransversalBeneficiarioPersistencia = politicaTransversalBeneficiarioPersistencia;
        }

        public AjustesPoliticaTBeneficiarioDto ObtenerAjustesPoliticaTransversalBeneficiario(ParametrosConsultaDto parametrosConsultaDto)
        {
            _politicaTransversalBeneficiarioPersistencia.ActualizarTemporal(parametrosConsultaDto);
            return Obtener(parametrosConsultaDto);
        }

        protected override AjustesPoliticaTBeneficiarioDto ObtenerDefinitivo(ParametrosConsultaDto parametrosConsultaDto)
        {
            AjustesPoliticaTBeneficiarioDto infoPersistencia = _politicaTransversalBeneficiarioPersistencia.ObtenerAjustesPoliticaTransversalBeneficiario(parametrosConsultaDto.Bpin);
            return infoPersistencia;
        }

        public AjustesPoliticaTBeneficiarioDto ObtenerAjustesPoliticaTransversalBeneficiarioPreview()
        {
            return _politicaTransversalBeneficiarioPersistencia.ObtenerAjustesPoliticaTransversalBeneficiarioPreview();
        }

        protected override void GuardadoDefinitivo(ParametrosGuardarDto<AjustesPoliticaTBeneficiarioDto> parametrosGuardar, string usuario)
        {
            _politicaTransversalBeneficiarioPersistencia.GuardarDefinitivamente(parametrosGuardar, usuario);
        }
    }
}
