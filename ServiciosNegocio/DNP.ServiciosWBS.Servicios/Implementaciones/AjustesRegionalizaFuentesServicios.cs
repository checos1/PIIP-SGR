namespace DNP.ServiciosWBS.Servicios.Implementaciones
{
    using Interfaces;
    using Interfaces.Transversales;
    using Persistencia.Interfaces;
    using Persistencia.Interfaces.Transversales;
    using ServiciosNegocio.Comunes;
    using ServiciosNegocio.Dominio.Dto.FuenteFinanciacion;
    using ServiciosNegocio.Comunes.Dto.Formulario;

    public class AjustesRegionalizaFuentesServicios : ServicioBase<AjustesRegionalizaFuentesDto>, IAjustesRegionalizaFuentesServicios
    {
        private readonly IAjustesRegionalizaFuentesPersistencia _ajustesRegionalizacionPersistencia;

        public AjustesRegionalizaFuentesServicios(IAjustesRegionalizaFuentesPersistencia ajustesRegionalizacionPersistencia, IPersistenciaTemporal persistenciaTemporal, IAuditoriaServicios auditoriaServicios) : base(persistenciaTemporal, auditoriaServicios)
        {
            _ajustesRegionalizacionPersistencia = ajustesRegionalizacionPersistencia;
        }

        public AjustesRegionalizaFuentesDto ObtenerAjustesRegionalizaFuentes(ParametrosConsultaDto parametrosConsulta)
        {
            _ajustesRegionalizacionPersistencia.ActualizarTemporal(parametrosConsulta);
            return Obtener(parametrosConsulta);
        }

        protected override AjustesRegionalizaFuentesDto ObtenerDefinitivo(ParametrosConsultaDto parametrosConsultaDto)
        {
            AjustesRegionalizaFuentesDto infoPersistencia = _ajustesRegionalizacionPersistencia.ObtenerAjustesRegionalizaFuentes(parametrosConsultaDto.Bpin);
            return infoPersistencia;
        }

        public AjustesRegionalizaFuentesDto ObtenerAjustesRegionalizaFuentesPreview()
        {
            return _ajustesRegionalizacionPersistencia.ObtenerAjustesRegionalizaFuentesPreview();
        }

        protected override void GuardadoDefinitivo(ParametrosGuardarDto<AjustesRegionalizaFuentesDto> parametrosGuardar, string usuario)
        {
            _ajustesRegionalizacionPersistencia.GuardarDefinitivamente(parametrosGuardar, usuario);
        }
    }
}
