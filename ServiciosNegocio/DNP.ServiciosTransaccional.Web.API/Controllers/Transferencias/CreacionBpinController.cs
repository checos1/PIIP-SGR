namespace DNP.ServiciosTransaccional.Web.API.Controllers.Transferencias
{
    using System;
    using System.Configuration;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web.Http;
    using Servicios.Interfaces.Transferencias;
    using ServiciosNegocio.Comunes;
    using ServiciosNegocio.Comunes.Autorizacion;
    using ServiciosNegocio.Comunes.Dto;
    using ServiciosNegocio.Comunes.Dto.Formulario;
    using ServiciosNegocio.Comunes.Excepciones;
    using ServiciosNegocio.Dominio.Dto.Transferencias;


    public class CreacionBpinController : ApiController
    {
        private readonly ICreacionBpinServicio _creacionBpinServicio;
        private readonly IAutorizacionUtilidades _autorizacionUtilidades;

        public CreacionBpinController(ICreacionBpinServicio creacionBpinServicio, IAutorizacionUtilidades autorizacionUtilidades)
        {
            _creacionBpinServicio = creacionBpinServicio;
            _autorizacionUtilidades = autorizacionUtilidades;
        }

        [Route("api/CreacionBpin")]
        [HttpPost]
        public async Task<IHttpActionResult> Definitivo(ObjetoNegocio contenido)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.
                                            ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                           RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                           ConfigurationManager.AppSettings
                                                               ["idAdministracionEnAutorizaciones"],
                                                           ConfigurationManager.AppSettings
                                                               ["CreacionBpin"]).
                                            Result;

                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                var parametrosGuardar = new ParametrosGuardarDto<ObjetoNegocio>();
                if (contenido != null)
                    parametrosGuardar.Contenido = contenido;
                else
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NoContent, new Exception(string.Format(ServiciosNegocioRecursos.ParametrosNoRecibidos))));

                var parametrosAuditoria = new ParametrosAuditoriaDto
                                          {
                                              Usuario = RequestContext.Principal.Identity.Name,
                                              Ip = UtilidadesApi.GetClientIp(Request)
                                          };

                var response = await Task.Run(() => _creacionBpinServicio.Guardar(parametrosGuardar, parametrosAuditoria));

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