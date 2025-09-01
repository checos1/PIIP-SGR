using DNP.ServiciosNegocio.Comunes;
using DNP.ServiciosNegocio.Comunes.Autorizacion;
using DNP.ServiciosNegocio.Comunes.Excepciones;
using DNP.ServiciosNegocio.Dominio.Dto.Transferencias;
using DNP.ServiciosTransaccional.Servicios.Interfaces.Priorizacion;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace DNP.ServiciosTransaccional.Web.API.Controllers.Priorizacion
{
    public class PriorizacionController : ApiController
    {
        private readonly IAutorizacionUtilidades _autorizacionUtilidades;
        private readonly IPriorizacionServicio _priorizacionServicio;

        public PriorizacionController(IPriorizacionServicio priorizacionServicio, IAutorizacionUtilidades autorizacionUtilidades)
        {
            _priorizacionServicio = priorizacionServicio;
            _autorizacionUtilidades = autorizacionUtilidades;
        }

        [Route("api/Priorizacion/RegistrarInstancia")]
        [HttpPost]
        public async Task<IHttpActionResult> RegistrarInstancia(ObjetoNegocio objetoNegocio)
        {
            try
            {
                string usuarioDNP = RequestContext.Principal.Identity.Name;
                var respuestaAutorizacion = await _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                           RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                           ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                           ConfigurationManager.AppSettings["actualizarEstadoProyecto"]);

                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                var response = await Task.Run(() => _priorizacionServicio.RegistrarInstanciaPriorizacion(usuarioDNP, objetoNegocio));

                var respuesta = new HttpResponseMessage();
                respuesta.StatusCode = HttpStatusCode.OK;
                respuesta.ReasonPhrase = ServiciosNegocioRecursos.PostExitoso;

                return Ok(respuesta);
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
