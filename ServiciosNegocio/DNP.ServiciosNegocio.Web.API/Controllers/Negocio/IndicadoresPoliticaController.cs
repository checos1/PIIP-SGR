using DNP.ServiciosNegocio.Comunes;
using DNP.ServiciosNegocio.Comunes.Autorizacion;
using DNP.ServiciosNegocio.Comunes.Excepciones;
using DNP.ServiciosNegocio.Dominio.Dto.IndicadoresPolitica;
using DNP.ServiciosNegocio.Servicios.Interfaces.IndicadoresPolitica;
using Swashbuckle.Swagger.Annotations;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;


namespace DNP.ServiciosNegocio.Web.API.Controllers.Negocio
{
    public class IndicadoresPoliticaController : ApiController
    {
        private readonly IIndicadoresPoliticaServicio _IndicadoresPolitica;
        private readonly IAutorizacionUtilidades _autorizacionUtilidades;

        public IndicadoresPoliticaController(IIndicadoresPoliticaServicio IndicadoresPolitica,
            IAutorizacionUtilidades autorizacionUtilidades)
        {
            _IndicadoresPolitica = IndicadoresPolitica;
            _autorizacionUtilidades = autorizacionUtilidades;
        }

        [Route("api/Focalizacion/ConsultarIndicadorPolitica")]
        [SwaggerResponse(HttpStatusCode.OK, "Retorna listado de indicadores por politica.", typeof(IndicadoresPoliticaDto))]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerDatosIndicadoresPolitica(string BPIN)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                                                   RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                                                   ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                                                   ConfigurationManager.AppSettings["ObtenerDatosAdicionalesFuente"]).Result;

                if (!respuestaAutorizacion.IsSuccessStatusCode)
                    return ResponseMessage(respuestaAutorizacion);

                var result = await Task.Run(() => _IndicadoresPolitica.ObtenerDatosIndicadoresPolitica(BPIN));
                if (result != null) return Ok(result);

                var respuestaHttp = new System.Net.Http.HttpResponseMessage()
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