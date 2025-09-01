namespace DNP.Backbone.Web.API.Test.Mocks
{
    using DNP.Backbone.Servicios.Interfaces.TramiteIncorporacion;
    using DNP.ServiciosNegocio.Dominio.Dto.TramiteIncorporacion;
    using System.Threading.Tasks;

    public class TramiteIncorporacionServiciosMock : ITramiteIncorporacionServicios
    {

        private readonly ITramiteIncorporacionServicios _tramiteIncorporacionServicios;

        public TramiteIncorporacionServiciosMock(ITramiteIncorporacionServicios tramiteIncorporacionServicios)
        {
            _tramiteIncorporacionServicios = tramiteIncorporacionServicios;
        }

       public Task<string> ObtenerDatosIncorporacion(int tramiteId, string usuario)
       {
           return _tramiteIncorporacionServicios.ObtenerDatosIncorporacion(tramiteId, usuario);
       }

      public Task<string> GuardarDatosIncorporacion(ConvenioDonanteDto objConvenioDonanteDto, string usuario)
      {
          return _tramiteIncorporacionServicios.GuardarDatosIncorporacion(objConvenioDonanteDto, usuario);
      }

      public Task<string> EiliminarDatosIncorporacion(ConvenioDonanteDto objConvenioDonanteDto, string usuario)
      {
          return _tramiteIncorporacionServicios.EiliminarDatosIncorporacion(objConvenioDonanteDto, usuario);
      }

    }
}
