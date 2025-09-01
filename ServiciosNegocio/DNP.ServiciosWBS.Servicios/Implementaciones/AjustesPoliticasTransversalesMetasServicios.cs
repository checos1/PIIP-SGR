namespace DNP.ServiciosWBS.Servicios.Implementaciones
{
    using Interfaces;
    using Interfaces.Transversales;
    using Persistencia.Interfaces;
    using Persistencia.Interfaces.Transversales;
    using ServiciosNegocio.Comunes;
    using ServiciosNegocio.Dominio.Dto.Focalizacion;
    using ServiciosNegocio.Comunes.Dto.Formulario;
    public class AjustesPoliticasTransversalesMetasServicios : ServicioBase<AjustesPoliticaTMetasDto>, IAjustesPoliticasTransversalesMetasServicios
    {
        private readonly IAjustesPoliticasTransversalesMetasPersistencia _ajustesPoliticasTransversalesMetasPersistencia;
        public AjustesPoliticasTransversalesMetasServicios(IAjustesPoliticasTransversalesMetasPersistencia ajustesPoliticasTransversalesMetasPersistencia, IPersistenciaTemporal persistenciaTemporal, IAuditoriaServicios auditoriaServicios) : base(persistenciaTemporal, auditoriaServicios)
        {
            _ajustesPoliticasTransversalesMetasPersistencia = ajustesPoliticasTransversalesMetasPersistencia;
        }
        public AjustesPoliticaTMetasDto ObtenerAjustesPoliticasTransversalesMetas(ParametrosConsultaDto parametrosConsulta)
        {
            _ajustesPoliticasTransversalesMetasPersistencia.ActualizarTemporal(parametrosConsulta);
            return Obtener(parametrosConsulta);
        }

        public AjustesPoliticaTMetasDto ObtenerAjustesPoliticasTransversalesMetasPreview()
        {
            return _ajustesPoliticasTransversalesMetasPersistencia.ObtenerAjustesPoliticasTransversalesMetasPreview();
        }

        protected override void GuardadoDefinitivo(ParametrosGuardarDto<AjustesPoliticaTMetasDto> parametrosGuardar, string usuario)
        {
            _ajustesPoliticasTransversalesMetasPersistencia.GuardarDefinitivamente(parametrosGuardar, usuario);
        }

        protected override AjustesPoliticaTMetasDto ObtenerDefinitivo(ParametrosConsultaDto parametrosConsultaDto)
        {
            return _ajustesPoliticasTransversalesMetasPersistencia.ObtenerAjustesPoliticasTransversalesMetas(parametrosConsultaDto.Bpin);
        }
    }
}
