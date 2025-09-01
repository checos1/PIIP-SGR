namespace DNP.ServiciosNegocio.Test.Mock
{
    using DNP.ServiciosNegocio.Comunes.Autorizacion;
    using DNP.ServiciosNegocio.Comunes.Dto.Formulario;
    using DNP.ServiciosNegocio.Dominio.Dto.Genericos;
    using DNP.ServiciosNegocio.Dominio.Dto.TramiteIncorporacion;
    using DNP.ServiciosNegocio.Dominio.Dto.Tramites;
    using DNP.ServiciosNegocio.Dominio.Dto.Tramites.TramitesReprogramacion;
    using DNP.ServiciosNegocio.Persistencia.Interfaces.TramitesReprogramacion;
    using DNP.ServiciosNegocio.Servicios.Interfaces.TramitesReprogramacion;
    using System;

    public class TramitesReprogramacionPersistenciaMock : ITramitesReprogramacionPersistencia
    {
        private readonly ITramitesReprogramacionServicio _tramitesReprogramacionServicio;
        private readonly IAutorizacionUtilidades _autorizacionUtilidades;

        public TramitesReprogramacionPersistenciaMock(ITramitesReprogramacionServicio TramitesReprogramacionServicioServicio, IAutorizacionUtilidades autorizacionUtilidades)
        {
            _tramitesReprogramacionServicio = TramitesReprogramacionServicioServicio;
            _autorizacionUtilidades = autorizacionUtilidades;
        }

        public TramitesResultado GuardarDatosReprogramacion(ParametrosGuardarDto<DatosReprogramacionDto> parametrosGuardar, string usuario)
        {
            return _tramitesReprogramacionServicio.GuardarDatosReprogramacion(parametrosGuardar, usuario);
        }

        public string ObtenerResumenReprogramacionPorVigencia(Guid? InstanciaId, int ProyectoId, int TramiteId)
        {
            return _tramitesReprogramacionServicio.ObtenerResumenReprogramacionPorVigencia(InstanciaId, ProyectoId, TramiteId);
        }

        public string ObtenerResumenReprogramacionPorProductoVigencia(Guid? InstanciaId, int? ProyectoId, int TramiteId)
        {
            return _tramitesReprogramacionServicio.ObtenerResumenReprogramacionPorProductoVigencia(InstanciaId, ProyectoId, TramiteId);
        }
    }
}