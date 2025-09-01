namespace DNP.ServiciosWBS.Servicios.Implementaciones
{
    using Interfaces;
    using Interfaces.Transversales;
    using Persistencia.Interfaces;
    using Persistencia.Interfaces.Transversales;
    using ServiciosNegocio.Comunes;
    using ServiciosNegocio.Dominio.Dto.Focalizacion;
    using ServiciosNegocio.Comunes.Dto.Formulario;

    public class PoliticaTransversalBeneficiarioServicios : ServicioBase<PoliticaTBeneficiarioDto>, IPoliticaTransversalBeneficiarioServicios
    {
        private readonly IPoliticaTransversalBeneficiarioPersistencia _politicaTransversalBeneficiarioPersistencia;

        public PoliticaTransversalBeneficiarioServicios(IPoliticaTransversalBeneficiarioPersistencia politicaTransversalBeneficiarioPersistencia,
                         IPersistenciaTemporal persistenciaTemporal, IAuditoriaServicios auditoriaServicios) :
        base(persistenciaTemporal, auditoriaServicios)
        {
            _politicaTransversalBeneficiarioPersistencia = politicaTransversalBeneficiarioPersistencia;
        }

        public PoliticaTBeneficiarioDto ObtenerPoliticaTransversalBeneficiario(ParametrosConsultaDto parametrosConsultaDto)
        {
            _politicaTransversalBeneficiarioPersistencia.ActualizarTemporal(parametrosConsultaDto);
            return Obtener(parametrosConsultaDto);
        }

        protected override PoliticaTBeneficiarioDto ObtenerDefinitivo(ParametrosConsultaDto parametrosConsultaDto)
        {
            PoliticaTBeneficiarioDto infoPersistencia = _politicaTransversalBeneficiarioPersistencia.ObtenerPoliticaTransversalBeneficiario(parametrosConsultaDto.Bpin);
            return infoPersistencia;
        }

        public PoliticaTBeneficiarioDto ObtenerPoliticaTransversalBeneficiarioPreview()
        {
            return _politicaTransversalBeneficiarioPersistencia.ObtenerPoliticaTransversalBeneficiarioPreview();
        }

        protected override void GuardadoDefinitivo(ParametrosGuardarDto<PoliticaTBeneficiarioDto> parametrosGuardar, string usuario)
        {
            _politicaTransversalBeneficiarioPersistencia.GuardarDefinitivamente(parametrosGuardar, usuario);
        }
    }
}
