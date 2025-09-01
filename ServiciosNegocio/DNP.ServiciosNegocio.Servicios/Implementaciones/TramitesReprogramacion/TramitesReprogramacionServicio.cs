using DNP.ServiciosNegocio.Dominio.Dto.FuenteFinanciacion;
using DNP.ServiciosNegocio.Persistencia.Interfaces.FuenteFinanciacion;
using DNP.ServiciosNegocio.Servicios.Interfaces.FuenteFinanciacion;

namespace DNP.ServiciosNegocio.Servicios.Implementaciones.TramitesReprogramacion
{
    using DNP.ServiciosNegocio.Comunes.Dto.Formulario;
    using DNP.ServiciosNegocio.Dominio.Dto.Genericos;
    using DNP.ServiciosNegocio.Dominio.Dto.TramiteIncorporacion;
    using DNP.ServiciosNegocio.Dominio.Dto.Tramites;
    using DNP.ServiciosNegocio.Dominio.Dto.Tramites.TramitesReprogramacion;
    using DNP.ServiciosNegocio.Persistencia.Interfaces.TramitesReprogramacion;
    using DNP.ServiciosNegocio.Servicios.Interfaces.TramitesReprogramacion;
    using Interfaces.Transversales;
    using Persistencia.Interfaces.Genericos;
    using System;

    public class TramitesReprogramacionServicio : ServicioBase<ConvenioDto>, ITramitesReprogramacionServicio
    {
        private readonly ITramitesReprogramacionPersistencia _TramitesReprogramacionPersistencia;

        public TramitesReprogramacionServicio(ITramitesReprogramacionPersistencia objTramitesReprogramacionPersistencia, IPersistenciaTemporal persistenciaTemporal, IAuditoriaServicios auditoriaServicios) : base(persistenciaTemporal, auditoriaServicios)
        {
            _TramitesReprogramacionPersistencia = objTramitesReprogramacionPersistencia;
        }

        public TramitesResultado GuardarDatosReprogramacion(ParametrosGuardarDto<DatosReprogramacionDto> parametrosGuardar, string usuario)
        {
            return _TramitesReprogramacionPersistencia.GuardarDatosReprogramacion(parametrosGuardar, usuario);
        }

        public string ObtenerResumenReprogramacionPorVigencia(Guid? InstanciaId, int ProyectoId, int TramiteId)
        {
            return _TramitesReprogramacionPersistencia.ObtenerResumenReprogramacionPorVigencia(InstanciaId, ProyectoId, TramiteId);
        }

        protected override void GuardadoDefinitivo(ParametrosGuardarDto<ConvenioDto> parametrosGuardar, string usuario)
        {
            throw new System.NotImplementedException();
        }

        protected override ConvenioDto ObtenerDefinitivo(ParametrosConsultaDto parametrosConsultaDto)
        {
            throw new System.NotImplementedException();
        }
        public string ObtenerResumenReprogramacionPorProductoVigencia(Guid? InstanciaId, int? ProyectoId, int TramiteId)
        {
            return _TramitesReprogramacionPersistencia.ObtenerResumenReprogramacionPorProductoVigencia(InstanciaId, ProyectoId, TramiteId);
        }
    }
}
