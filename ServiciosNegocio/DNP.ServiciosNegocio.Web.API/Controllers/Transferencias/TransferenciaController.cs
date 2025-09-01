namespace DNP.ServiciosNegocio.Web.API.Controllers.Transferencias
{
    using System.Configuration;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web.Http;
    using Comunes;
    using Comunes.Autorizacion;
    using Comunes.Dto;
    using Comunes.Excepciones;
    using Dominio.Dto.Transferencias;
    using Servicios.Interfaces.Transferencias;

    public class TransferenciaController : ApiController
    {
        private readonly ITransferenciaServicio _transferenciaServicio;
        private readonly IAutorizacionUtilidades _autorizacionUtilidades;

        public TransferenciaController(ITransferenciaServicio transferenciaServicios, IAutorizacionUtilidades autorizacionUtilidades)
        {
            _transferenciaServicio = transferenciaServicios;
            _autorizacionUtilidades = autorizacionUtilidades;
        }

        [Route("api/Transferencia/IdentificarEntidadDestino")]
        [HttpPost]
        public async Task<IHttpActionResult> IdentificarEntidadDestino(TransferenciaEntidadDto entidadDto)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.
                                            ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                           RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                           ConfigurationManager.AppSettings
                                                               ["idAdministracionEnAutorizaciones"],
                                                           ConfigurationManager.AppSettings
                                                               ["identificarEntidadDestinoTransferencias"]).
                                            Result;

                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                var parametrosAuditoria = new ParametrosAuditoriaDto
                {
                    Usuario = RequestContext.Principal.Identity.Name,
                    Ip = UtilidadesApi.GetClientIp(Request)
                };

                await Task.Run(() => _transferenciaServicio.IdentificarEntidadDestino(entidadDto.ProyectoId, entidadDto.EntidadTransfiereId, parametrosAuditoria));

                var respuesta = new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    ReasonPhrase = ServiciosNegocioRecursos.PostExitoso
                };

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