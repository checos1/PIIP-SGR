using DNP.ServiciosNegocio.Comunes;
using DNP.ServiciosNegocio.Comunes.Autorizacion;
using DNP.ServiciosNegocio.Comunes.Excepciones;
using DNP.ServiciosNegocio.Servicios.Interfaces.SGR.Transversales;
using Swashbuckle.Swagger.Annotations;
using System.Configuration;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;

namespace DNP.ServiciosNegocio.Web.API.Controllers.SGR.Transversales
{
    using DNP.ServiciosNegocio.Dominio.Dto.SGP.Transversales;
    using DNP.ServiciosNegocio.Dominio.Dto.SGR.Transversales;
    using DNP.ServiciosNegocio.Servicios.Interfaces.SGP.Transversales;
    using System.Net.Http;

    public class EncabezadoController : ApiController
    {
        private readonly ITransversalServicio _datosServicios;
        private readonly ITransversalServicioSGP _datosServiciosSGP;
        private readonly IAutorizacionUtilidades _autorizacionUtilidades;

        public EncabezadoController(ITransversalServicio datosServicios, IAutorizacionUtilidades autorizacionUtilidades, ITransversalServicioSGP datosServiciosSGP)
        {
            _datosServicios = datosServicios;
            _datosServiciosSGP = datosServiciosSGP;
            _autorizacionUtilidades = autorizacionUtilidades;
        }

        [Route("api/SGR/Encabezado/LeerEncabezado")]
        [SwaggerResponse(HttpStatusCode.OK, "Lee la información del encabezado de un proyecto de SGR.", typeof(string))]
        [HttpPost]
        public async Task<IHttpActionResult> SGR_Encabezado_LeerEncabezado([FromBody] ParametrosEncabezadoSGR parametros)
        {
            try
            {
                var respuestaAutorizacion = await _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                                                   RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                                                   ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                                                   ConfigurationManager.AppSettings["SGR_Encabezado_LeerEncabezado"]);

                if (!respuestaAutorizacion.IsSuccessStatusCode)
                    return ResponseMessage(respuestaAutorizacion);

                var result = await Task.Run(() => _datosServicios.uspGetSGR_Encabezado_LeerEncabezado(parametros));
                if (result != null) return Ok(result);

                var respuestaHttp = new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.NotFound,
                    ReasonPhrase = ServiciosNegocioRecursos.SinResultados
                };

                return ResponseMessage(respuestaHttp);
            }
            catch (ServiciosNegocioException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
            catch (AutorizacionException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, e));
            }
        }        
    }
}