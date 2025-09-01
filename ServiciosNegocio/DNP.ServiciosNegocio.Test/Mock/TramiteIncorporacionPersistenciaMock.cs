namespace DNP.ServiciosNegocio.Test.Mock
{
    using DNP.ServiciosNegocio.Comunes.Autorizacion;
    using DNP.ServiciosNegocio.Comunes.Dto.Formulario;
    using DNP.ServiciosNegocio.Dominio.Dto.Genericos;
    using DNP.ServiciosNegocio.Dominio.Dto.TramiteIncorporacion;
    using DNP.ServiciosNegocio.Persistencia.Interfaces.TramiteIncorporacion;
    using DNP.ServiciosNegocio.Servicios.Interfaces.TramiteIncorporacion;

    public class TramiteIncorporacionPersistenciaMock : ITramiteIncorporacionPersistencia
    {
        private readonly ITramiteIncorporacionServicio _tramiteIncorporacionServicio;
        private readonly IAutorizacionUtilidades _autorizacionUtilidades;

        public TramiteIncorporacionPersistenciaMock(ITramiteIncorporacionServicio tramiteIncorporacionServicioServicio, IAutorizacionUtilidades autorizacionUtilidades)
        {
            _tramiteIncorporacionServicio = tramiteIncorporacionServicioServicio;
            _autorizacionUtilidades = autorizacionUtilidades;
        }

        public RespuestaGeneralDto EiliminarDatosIncorporacion(ParametrosGuardarDto<ConvenioDonanteDto> parametrosGuardar, string usuario)
        {
            return _tramiteIncorporacionServicio.EiliminarDatosIncorporacion(parametrosGuardar,usuario);
        }

        public RespuestaGeneralDto GuardarDatosIncorporacion(ParametrosGuardarDto<ConvenioDonanteDto> parametrosGuardar, string usuario)
        {
            return _tramiteIncorporacionServicio.GuardarDatosIncorporacion(parametrosGuardar,usuario);
        }

        public string ObtenerDatosIncorporacion(int TramiteId)
        {
            return _tramiteIncorporacionServicio.ObtenerDatosIncorporacion(TramiteId);
        }
    }
}
