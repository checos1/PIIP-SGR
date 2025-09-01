using DNP.ServiciosNegocio.Dominio.Dto.FuenteFinanciacion;
using DNP.ServiciosNegocio.Persistencia.Interfaces.FuenteFinanciacion;
using DNP.ServiciosNegocio.Servicios.Interfaces.FuenteFinanciacion;

namespace DNP.ServiciosNegocio.Servicios.Implementaciones.TramiteIncorporacion
{
    using DNP.ServiciosNegocio.Comunes.Dto.Formulario;
    using DNP.ServiciosNegocio.Dominio.Dto.Genericos;
    using DNP.ServiciosNegocio.Dominio.Dto.TramiteIncorporacion;
    using DNP.ServiciosNegocio.Persistencia.Interfaces.TramiteIncorporacion;
    using DNP.ServiciosNegocio.Servicios.Interfaces.TramiteIncorporacion;
    using Interfaces.Transversales;
    using Persistencia.Interfaces.Genericos;

    public class TramiteIncorporacionServicio : ServicioBase<ConvenioDto>, ITramiteIncorporacionServicio
    {
        private readonly ITramiteIncorporacionPersistencia _tramiteIncorporacionPersistencia;

        public TramiteIncorporacionServicio(ITramiteIncorporacionPersistencia objTramiteIncorporacionPersistencia, IPersistenciaTemporal persistenciaTemporal, IAuditoriaServicios auditoriaServicios) : base(persistenciaTemporal, auditoriaServicios)
        {
            _tramiteIncorporacionPersistencia = objTramiteIncorporacionPersistencia;
        }

        public RespuestaGeneralDto EiliminarDatosIncorporacion(ParametrosGuardarDto<ConvenioDonanteDto> parametrosGuardar, string usuario)
        {
            return _tramiteIncorporacionPersistencia.EiliminarDatosIncorporacion(parametrosGuardar, usuario);
        }

        public RespuestaGeneralDto GuardarDatosIncorporacion(ParametrosGuardarDto<ConvenioDonanteDto> parametrosGuardar, string usuario)
        {
            return _tramiteIncorporacionPersistencia.GuardarDatosIncorporacion(parametrosGuardar, usuario);
        }

        public string ObtenerDatosIncorporacion(int tramiteId)
        {
            return _tramiteIncorporacionPersistencia.ObtenerDatosIncorporacion(tramiteId);
        }

        protected override void GuardadoDefinitivo(ParametrosGuardarDto<ConvenioDto> parametrosGuardar, string usuario)
        {
            throw new System.NotImplementedException();
        }

        protected override ConvenioDto ObtenerDefinitivo(ParametrosConsultaDto parametrosConsultaDto)
        {
            throw new System.NotImplementedException();
        }

    }
}
