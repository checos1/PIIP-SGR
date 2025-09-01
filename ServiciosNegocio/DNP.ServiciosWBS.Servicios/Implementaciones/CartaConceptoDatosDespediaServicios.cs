namespace DNP.ServiciosWBS.Servicios.Implementaciones
{
    using Interfaces;
    using Interfaces.Transversales;
    using Persistencia.Interfaces;
    using Persistencia.Interfaces.Transversales;
    using ServiciosNegocio.Dominio.Dto.Tramites;
    using ServiciosNegocio.Comunes.Dto.Formulario;

    public class CartaConceptoDatosDespediaServicios : ServicioBase<DatosConceptoDespedidaDto>, ICartaCuerpoDatosDespedidaServicios
    {
        private readonly ICartaConceptoDatosDespediaPersistencia _cartaConceptoDatosDespediaPersistencia;

        public CartaConceptoDatosDespediaServicios(ICartaConceptoDatosDespediaPersistencia cartaConceptoDatosDespediaPersistencia,
                         IPersistenciaTemporal persistenciaTemporal, IAuditoriaServicios auditoriaServicios) :
        base(persistenciaTemporal, auditoriaServicios)
        {
            _cartaConceptoDatosDespediaPersistencia = cartaConceptoDatosDespediaPersistencia;
        }

        public DatosConceptoDespedidaDto ObtenerCuerpoDatosDespedida(ParametrosConsultaDto parametrosConsultaDto)
        {
            return ObtenerDefinitivo(parametrosConsultaDto);
        }

        protected override DatosConceptoDespedidaDto ObtenerDefinitivo(ParametrosConsultaDto parametrosConsultaDto)
        {
            DatosConceptoDespedidaDto infoPersistencia = _cartaConceptoDatosDespediaPersistencia.ObtenerCartaConceptoDatosDespedida(parametrosConsultaDto.TramiteId, (int)parametrosConsultaDto.plantillaCartaSeccionId);
            return infoPersistencia;
        }

        public DatosConceptoDespedidaDto ObtenerCuerpoDatosDespedidaPreview()
        {
            return _cartaConceptoDatosDespediaPersistencia.ObtenerCartaConceptoDatosDespedidaPreview();
        }

        protected override void GuardadoDefinitivo(ParametrosGuardarDto<DatosConceptoDespedidaDto> parametrosGuardar, string usuario)
        {
            _cartaConceptoDatosDespediaPersistencia.GuardarDefinitivamente(parametrosGuardar, usuario);
        }
    }
}
