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
    using DNP.ServiciosNegocio.Dominio.Dto.Genericos;
    using Dominio.Dto.Preguntas;
    using Servicios.Interfaces.SGP.Viabilidad;
    public class PreguntasPersonalizadasSGPController : ApiController
    {
        private readonly IPreguntasPersonalizadasSGPServicio _preguntasPersonalizadasServicio;
        private readonly IAutorizacionUtilidades _autorizacionUtilidades;

        public PreguntasPersonalizadasSGPController(IPreguntasPersonalizadasSGPServicio preguntasPersonalizadasServicio, IAutorizacionUtilidades autorizacionUtilidades)
        {
            _preguntasPersonalizadasServicio = preguntasPersonalizadasServicio;
            _autorizacionUtilidades = autorizacionUtilidades;
        }

        [Route("SGP/PreguntasPersonalizadas/ConsultarPreguntasSGP")]
        [SwaggerResponse(HttpStatusCode.OK, "Retorna los listados de preguntas personalizadas", typeof(ServicioPreguntasPersonalizadasDto))]
        [HttpGet]
        public async Task<IHttpActionResult> ConsultarPreguntasSGP()
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                                                   RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                                                   ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                                                   ConfigurationManager.AppSettings["ConsultarPreguntasSGP"]).Result;

                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                ValidacionParametros(Request);
                ValidacionHeaders(Request);

                var parametrosConsulta = _preguntasPersonalizadasServicio.ConstruirParametrosConsulta(Request);
                parametrosConsulta.Token = Request.Headers.Authorization.ToString();

                var result = await Task.Run(() => _preguntasPersonalizadasServicio.Obtener(parametrosConsulta));

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

        [Route("SGP/PreguntasPersonalizadas/ObtenerPreguntasPersonalizadas")]
        [SwaggerResponse(HttpStatusCode.OK, "Retorna los listados de preguntas personalizadas", typeof(ServicioPreguntasPersonalizadasDto))]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerPreguntasPersonalizadas([FromUri] string bPin, Guid nivelId, Guid instanciaId, string listaRoles)
        {
            var result = await Task.Run(() => _preguntasPersonalizadasServicio.ObtenerPreguntasPersonalizadas(bPin, nivelId, instanciaId, listaRoles));

            return Ok(result);
        }

        [Route("SGP/PreguntasPersonalizadas/ObtenerPreguntasPersonalizadasComponente")]
        [SwaggerResponse(HttpStatusCode.OK, "Retorna los listados de preguntas personalizadas", typeof(ServicioPreguntasPersonalizadasDto))]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerPreguntasPersonalizadasComponente([FromUri] string bPin, Guid nivelId, Guid instanciaId, string nombreComponente, string listaRoles)
        {
            var result = await Task.Run(() => _preguntasPersonalizadasServicio.ObtenerPreguntasPersonalizadasComponente(bPin, nivelId, instanciaId, nombreComponente, listaRoles));
            return Ok(result);
        }

        [Route("SGP/PreguntasPersonalizadas/ObtenerPreguntasPersonalizadasComponenteSGP")]
        [SwaggerResponse(HttpStatusCode.OK, "Retorna los listados de preguntas personalizadas", typeof(ServicioPreguntasPersonalizadasDto))]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerPreguntasPersonalizadasComponenteSGP([FromUri] string bPin, Guid nivelId, Guid instanciaId, string nombreComponente, string listaRoles)
        {
            var result = await Task.Run(() => _preguntasPersonalizadasServicio.ObtenerPreguntasPersonalizadasComponenteSGP(bPin, nivelId, instanciaId, nombreComponente, listaRoles));
            return Ok(result);
        }

        [Route("SGP/PreguntasPersonalizadas/ObtenerDatosGeneralesProyecto")]
        [SwaggerResponse(HttpStatusCode.OK, "Retorna información general del proyecto", typeof(DatosGeneralesProyectosDto))]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerDatosGeneralesProyecto([FromUri] int? ProyectoId, Guid NivelId)
        {
            var result = await Task.Run(() => _preguntasPersonalizadasServicio.ObtenerDatosGeneralesProyecto(ProyectoId, NivelId));

            return Ok(result);
        }

        [Route("SGP/PreguntasPersonalizadas/GuardarPreguntasPersonalizadas")]
        [SwaggerResponse(HttpStatusCode.OK, "Informacion devuelta por guardado de Preguntas Personalizadas", typeof(HttpResponseMessage))]
        [HttpPost]
        public async Task<IHttpActionResult> GuardarPreguntasPersonalizadas([FromBody] ServicioPreguntasPersonalizadasDto contenido)
        {
            try
            {
                var parametrosGuardar = _preguntasPersonalizadasServicio.ConstruirParametrosGuardar(Request);

                if (contenido != null)
                {
                    parametrosGuardar.Contenido = contenido;
                }
                else
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NoContent, new Exception(string.Format(ServiciosNegocioRecursos.ParametrosNoRecibidos))));

                var parametrosAuditoria = new ParametrosAuditoriaDto
                {
                    Usuario = RequestContext.Principal.Identity.Name,
                    Ip = UtilidadesApi.GetClientIp(Request)
                };
                _preguntasPersonalizadasServicio.Usuario = RequestContext?.Principal?.Identity?.Name;
                _preguntasPersonalizadasServicio.Ip = UtilidadesApi.GetClientIp(Request);
                await Task.Run(() => _preguntasPersonalizadasServicio.Guardar(parametrosGuardar, parametrosAuditoria, false));

                return Ok(new RespuestaGeneralDto { Exito = true });
            }
            catch (ServiciosNegocioException e)
            {
                return Ok(new
                {
                    EsExcepcion = true,
                    ExcepcionMensaje = $"PreguntasPersonalizadas.GuardarPreguntasPersonalizadas [{contenido.ToString()}] => {e.Message}\\n{e.InnerException?.Message ?? String.Empty}"
                });
                //throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
            catch (AutorizacionException e)
            {
                return Ok(new
                {
                    EsExcepcion = true,
                    ExcepcionMensaje = $"PreguntasPersonalizadas.GuardarPreguntasPersonalizadas [{contenido.ToString()}] => {e.Message}\\n{e.InnerException?.Message ?? String.Empty}"
                });
                //throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, e));
            }
        }

        [Route("SGP/PreguntasPersonalizadas/GuardarPreguntasPersonalizadasCustomSGP")]
        [SwaggerResponse(HttpStatusCode.OK, "Informacion devuelta por guardado de Preguntas Personalizadas", typeof(HttpResponseMessage))]
        [HttpPost]
        public async Task<IHttpActionResult> GuardarPreguntasPersonalizadasCustomSGP([FromBody] ServicioPreguntasPersonalizadasDto contenido)
        {
            try
            {
                var parametrosGuardar = _preguntasPersonalizadasServicio.ConstruirParametrosGuardar(Request);

                if (contenido != null)
                {
                    parametrosGuardar.Contenido = contenido;
                }
                else
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NoContent, new Exception(string.Format(ServiciosNegocioRecursos.ParametrosNoRecibidos))));

                var parametrosAuditoria = new ParametrosAuditoriaDto
                {
                    Usuario = RequestContext.Principal.Identity.Name,
                    Ip = UtilidadesApi.GetClientIp(Request)
                };
                _preguntasPersonalizadasServicio.Usuario = RequestContext?.Principal?.Identity?.Name;
                _preguntasPersonalizadasServicio.Ip = UtilidadesApi.GetClientIp(Request);
                await Task.Run(() => _preguntasPersonalizadasServicio.GuardarCustomSGP(parametrosGuardar, parametrosAuditoria, false));

                return Ok(new RespuestaGeneralDto { Exito = true });
            }
            catch (ServiciosNegocioException e)
            {
                return Ok(new
                {
                    EsExcepcion = true,
                    ExcepcionMensaje = $"PreguntasPersonalizadas.GuardarPreguntasPersonalizadas [{contenido.ToString()}] => {e.Message}\\n{e.InnerException?.Message ?? String.Empty}"
                });
                //throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
            catch (AutorizacionException e)
            {
                return Ok(new
                {
                    EsExcepcion = true,
                    ExcepcionMensaje = $"PreguntasPersonalizadas.GuardarPreguntasPersonalizadas [{contenido.ToString()}] => {e.Message}\\n{e.InnerException?.Message ?? String.Empty}"
                });
                //throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, e));
            }
        }

        [Route("SGP/PreguntasPersonalizadas/DevolverCuestionarioProyecto")]
        [SwaggerResponse(HttpStatusCode.OK, "Informacion que actualiza el campo definitivo de CuestionarioProyecto", typeof(HttpResponseMessage))]
        [HttpPost]
        public async Task<IHttpActionResult> DevolverCuestionarioProyecto([FromUri] Guid nivelId, Guid instanciaId, int estadoAccionesPorInstancia)
        {
            try
            {             
                await Task.Run(() => _preguntasPersonalizadasServicio.DevolverCuestionarioProyecto(nivelId, instanciaId, estadoAccionesPorInstancia));

                return Ok(new RespuestaGeneralDto { Exito = true });
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

        [Route("SGP/PreguntasPersonalizadas/ObtenerConfiguracionEntidades")]
        [SwaggerResponse(HttpStatusCode.OK, "Retorna la configuracion de las entidades", typeof(ConfiguracionEntidadDto))]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerConfiguracionEntidades([FromUri] int? ProyectoId, Guid NivelId)
        {
            var result = await Task.Run(() => _preguntasPersonalizadasServicio.ObtenerConfiguracionEntidades(ProyectoId, NivelId));

            return Ok(result);
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