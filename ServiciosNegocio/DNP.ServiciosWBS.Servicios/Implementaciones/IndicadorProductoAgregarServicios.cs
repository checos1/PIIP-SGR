namespace DNP.ServiciosWBS.Servicios.Implementaciones
{
    using Interfaces;
    using Interfaces.Transversales;
    using Persistencia.Interfaces;
    using Persistencia.Interfaces.Transversales;
    using ServiciosNegocio.Comunes;
    using ServiciosNegocio.Dominio.Dto.IndicadorProductoAgregar;
    using ServiciosNegocio.Comunes.Dto.Formulario;
    public class IndicadorProductoAgregarServicios : ServicioBase<IndicadorProductoAgregarDto>, IIndicadorProductoAgregarServicios
    {
        private readonly IIndicadorProductoAgregarPersistencia _indicadorProductoAgregarPersistencia;


        public IndicadorProductoAgregarServicios(IIndicadorProductoAgregarPersistencia indicadorProductoAgregarPersistencia,
                         IPersistenciaTemporal persistenciaTemporal, IAuditoriaServicios auditoriaServicios) :
        base(persistenciaTemporal, auditoriaServicios)
        {
            _indicadorProductoAgregarPersistencia = indicadorProductoAgregarPersistencia;
        }

        public IndicadorProductoAgregarDto ObtenerIndicadorProductoAgregar(ParametrosConsultaDto parametrosConsultaDto)
        {
            _indicadorProductoAgregarPersistencia.ActualizarTemporal(parametrosConsultaDto);
            return Obtener(parametrosConsultaDto);
        }

        protected override IndicadorProductoAgregarDto ObtenerDefinitivo(ParametrosConsultaDto parametrosConsultaDto)
        {
            IndicadorProductoAgregarDto infoPersistencia = _indicadorProductoAgregarPersistencia.ObtenerIndicadorProductoAgregar(parametrosConsultaDto.Bpin);
            return infoPersistencia;
        }

        public IndicadorProductoAgregarDto ObtenerIndicadorProductoAgregarPreview()
        {
            return _indicadorProductoAgregarPersistencia.ObtenerIndicadorProductoAgregarPreview();
        }

        protected override void GuardadoDefinitivo(ParametrosGuardarDto<IndicadorProductoAgregarDto> parametrosGuardar, string usuario)
        {
            _indicadorProductoAgregarPersistencia.GuardarDefinitivamente(parametrosGuardar, usuario);
        }

    }
}
