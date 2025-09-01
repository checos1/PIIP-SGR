
namespace DNP.Backbone.Servicios.Interfaces.TramiteIncorporacion
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Web.Http;
    using Comunes.Dto;
    using DNP.Backbone.Dominio.Dto.Focalizacion;
    using DNP.Backbone.Dominio.Dto.Monitoreo;
    using DNP.ServiciosNegocio.Dominio.Dto.FuenteFinanciacion;
    using DNP.ServiciosNegocio.Dominio.Dto.TramiteIncorporacion;
    using Dominio.Dto.Proyecto;

    public interface ITramiteIncorporacionServicios
    {
        Task<string> ObtenerDatosIncorporacion(int tramiteId, string usuarioDnp);

        Task<string> GuardarDatosIncorporacion(ConvenioDonanteDto objConvenioDonanteDto, string usuario);

        Task<string> EiliminarDatosIncorporacion(ConvenioDonanteDto objConvenioDonanteDto, string usuario);

    }
}
