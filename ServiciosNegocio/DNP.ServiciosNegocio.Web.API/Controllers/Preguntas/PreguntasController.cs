using System.Configuration;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using DNP.ServiciosNegocio.Comunes;
using DNP.ServiciosNegocio.Comunes.Autorizacion;
using DNP.ServiciosNegocio.Comunes.Excepciones;
using Swashbuckle.Swagger.Annotations;

namespace DNP.ServiciosNegocio.Web.API.Controllers.Preguntas
{
    using System;
    using System.Linq;
    using System.Net.Http;
    using System.Web;
    using Comunes.Dto;
    using Comunes.Dto.Formulario;
    using Dominio.Dto.Preguntas;
    using Servicios.Interfaces.Preguntas;

    public class PreguntasController : ApiController
    {
        private readonly IPreguntasServicio _preguntasServicio;
        private readonly IAutorizacionUtilidades _autorizacionUtilidades;

        public PreguntasController(IPreguntasServicio preguntasServicio, IAutorizacionUtilidades autorizacionUtilidades)
        {
            _preguntasServicio = preguntasServicio;
            _autorizacionUtilidades = autorizacionUtilidades;
        }

        [Route("api/Preguntas")]
        [SwaggerResponse(HttpStatusCode.OK, "Retorna los listados de preguntas", typeof(ServicioPreguntasDto))]
        [HttpGet]
        public async Task<IHttpActionResult> Consultar()
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                                                   RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                                                   ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                                                   ConfigurationManager.AppSettings["ConsultarPreguntas"]).Result;

                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                ValidacionParametros(Request);
                ValidacionHeaders(Request);

                var parametrosConsulta = _preguntasServicio.ConstruirParametrosConsulta(Request);
                parametrosConsulta.Token = Request.Headers.Authorization.ToString();

                var result = await Task.Run(() => _preguntasServicio.Obtener(parametrosConsulta));

                if (result.PreguntasEspecificas.Count > 0 || result.PreguntasGenerales.Count > 0)
                    return Ok(result);

                throw new ServiciosNegocioException(string.Format(ServiciosNegocioRecursos.SinResultados));
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

        [Route("api/Preguntas/Preview")]
        [SwaggerResponse(HttpStatusCode.OK, "Retorna los listados de preguntas preview", typeof(ServicioPreguntasDto))]
        [HttpGet]
        public async Task<IHttpActionResult> Preview()
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                                                   RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                                                   ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                                                   ConfigurationManager.AppSettings["PreviewPreguntas"]).Result;

                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);


                var result = await Task.Run(() => _preguntasServicio.ObtenerPreguntasPreview());
                return Ok(result);
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

        [Route("api/Preguntas")]
        [SwaggerResponse(HttpStatusCode.OK, "Informacion devuelta por guardado de Preguntas Definitivo", typeof(HttpResponseMessage))]
        [HttpPost]
        public async Task<IHttpActionResult> Definitivo(ServicioPreguntasDto contenido)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                                                   RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                                                   ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                                                   ConfigurationManager.AppSettings["DefinitivoPreguntas"]).Result;

                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                ValidacionHeaders(Request);
                var parametrosGuardar = _preguntasServicio.ConstruirParametrosGuardar(Request);

                if (contenido != null)
                    parametrosGuardar.Contenido = contenido;
                else
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NoContent, new Exception(string.Format(ServiciosNegocioRecursos.ParametrosNoRecibidos))));

                var parametrosAuditoria = new ParametrosAuditoriaDto
                                          {
                                              Usuario = RequestContext.Principal.Identity.Name,
                                              Ip = UtilidadesApi.GetClientIp(Request)
                                          };

                await Task.Run(() => _preguntasServicio.Guardar(parametrosGuardar, parametrosAuditoria, false));

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

        [Route("api/Preguntas/Temporal")]
        [SwaggerResponse(HttpStatusCode.OK, "Informacion devuelta por guardado de Preguntas temporal", typeof(HttpResponseMessage))]
        [HttpPost]
        public async Task<IHttpActionResult> Temporal(ServicioPreguntasDto contenido)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                                                   RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                                                   ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                                                   ConfigurationManager.AppSettings["TemporalPreguntas"]).Result;

                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                ValidacionHeaders(Request);
                var parametrosGuardar = _preguntasServicio.ConstruirParametrosGuardar(Request);
                if (contenido != null)
                    parametrosGuardar.Contenido = contenido;
                else
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NoContent, new Exception(string.Format(ServiciosNegocioRecursos.ParametrosNoRecibidos))));

                var parametrosAuditoria = new ParametrosAuditoriaDto
                                          {
                                              Usuario = RequestContext.Principal.Identity.Name,
                                              Ip = UtilidadesApi.GetClientIp(Request)
                                          };

                await Task.Run(() => _preguntasServicio.Guardar(parametrosGuardar, parametrosAuditoria, true));

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

        [SuppressMessage("ReSharper", "UnusedVariable")]
        private void ValidacionParametros(HttpRequestMessage peticion)
        {
            var bpin = HttpUtility.ParseQueryString(peticion.RequestUri.Query).Get("bpin");
            var nivelId = HttpUtility.ParseQueryString(peticion.RequestUri.Query).Get("IdNivel");

            if (string.IsNullOrWhiteSpace(bpin) && string.IsNullOrWhiteSpace(nivelId))
                throw new ServiciosNegocioException(string.Format(ServiciosNegocioRecursos.ParametrosNoRecibidos));

            if (string.IsNullOrWhiteSpace(bpin))
                throw new ServiciosNegocioException(string.Format(ServiciosNegocioRecursos.ParametroNoRecibido, nameof(bpin)));

            if (!long.TryParse(bpin, out var outBpin) || bpin.Length > 100)
                throw new ServiciosNegocioException(string.Format(ServiciosNegocioRecursos.ParametroNoValidos, nameof(bpin)));

            if (string.IsNullOrWhiteSpace(nivelId))
                throw new ServiciosNegocioException(string.Format(ServiciosNegocioRecursos.ParametroNoRecibido, nameof(nivelId)));

            if (!Guid.TryParse(nivelId, out Guid guidOutput))
                throw new ServiciosNegocioException(string.Format(ServiciosNegocioRecursos.ParametroNoValidos, nameof(nivelId)));
        }

        [SuppressMessage("ReSharper", "UnusedVariable")]
        private void ValidacionHeaders(HttpRequestMessage peticion)
        {
            if (!peticion.Headers.Contains("piip-idInstanciaFlujo") ||
                string.IsNullOrEmpty(peticion.Headers.GetValues("piip-idInstanciaFlujo").First()))
                throw new ServiciosNegocioException(string.Format(ServiciosNegocioRecursos.ParametroNoRecibido, "piip-idInstanciaFlujo"));

            if (!Guid.TryParse(peticion.Headers.GetValues("piip-idInstanciaFlujo").First(), out Guid outIdInstancia))
                throw new ServiciosNegocioException(string.Format(ServiciosNegocioRecursos.ParametroNoValidos, "piip-idInstanciaFlujo"));

            if (!peticion.Headers.Contains("piip-idFormulario") ||
                string.IsNullOrEmpty(peticion.Headers.GetValues("piip-idFormulario").First()))
                throw new ServiciosNegocioException(string.Format(ServiciosNegocioRecursos.ParametroNoRecibido, "piip-idFormulario"));

            if (!Guid.TryParse(peticion.Headers.GetValues("piip-idFormulario").First(), out Guid outIdFormulario))
                throw new ServiciosNegocioException(string.Format(ServiciosNegocioRecursos.ParametroNoValidos, "piip-idFormulario"));

        }
    }
}