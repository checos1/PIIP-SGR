namespace DNP.ServiciosNegocio.Web.API.Controllers.SGP.Tramite
{
    using System.Configuration;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web.Http;
    using Comunes.Autorizacion;
    using System;
    using Comunes.Dto;
    using DNP.ServiciosNegocio.Servicios.Interfaces.SGP.Tramite;
    using DNP.ServiciosNegocio.Comunes.Excepciones;
    using DNP.ServiciosNegocio.Comunes;
    using Swashbuckle.Swagger.Annotations;
    using DNP.ServiciosNegocio.Dominio.Dto.Proyectos;

    public class TramiteProyectoSGPController : ApiController
    {
        private readonly ITramiteProyectoSGPServicio _tramiteProyectoSgpServicio;
        private readonly IAutorizacionUtilidades _autorizacionUtilidades;


        public TramiteProyectoSGPController(ITramiteProyectoSGPServicio tramiteProyectoSgpServicio,
            IAutorizacionUtilidades autorizacionUtilidades)
        {
            _tramiteProyectoSgpServicio = tramiteProyectoSgpServicio;
            _autorizacionUtilidades = autorizacionUtilidades;
        }

        [Route("api/TramiteProyectoSGP/ObtenerProyectosTramiteNegocio")]
        [SwaggerResponse(HttpStatusCode.OK, "Retorna proyectos", typeof(ProyectosEnTramiteDto))]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerProyectosTramiteNegocio([FromUri] int TramiteId)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                                   RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                                   ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                                   ConfigurationManager.AppSettings["GestionTramitesProyectos"]).Result;

                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                var TokenAutorizacion = Request.Headers.Authorization.ToString();
                var result = await Task.Run(() => _tramiteProyectoSgpServicio.ObtenerProyectosTramiteNegocio(TramiteId));

                return Ok(result);
            }
            catch (Exception e)
            {
                throw;
            }
        }


        [Route("api/TramiteProyectoSGP/GuardarProyectosTramiteNegocio")]
        [SwaggerResponse(HttpStatusCode.OK, "Informacion devuelta por guardado", typeof(HttpResponseMessage))]
        [HttpPost]
        public async Task<IHttpActionResult> GuardarProyectosTramiteNegocio([FromBody] DatosTramiteProyectosDto DatosTramiteProyectosDto)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name, RequestContext.Principal.Identity.GetHashCode().ToString(), ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"], ConfigurationManager.AppSettings["GestionTramitesProyectos"]).Result;
                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                var parametrosGuardar = _tramiteProyectoSgpServicio.ConstruirParametrosGuardadoVentanas(DatosTramiteProyectosDto);

                var parametrosAuditoria = new ParametrosAuditoriaDto
                {
                    Usuario = RequestContext.Principal.Identity.Name,
                    Ip = UtilidadesApi.GetClientIp(Request)
                };

                var result = await Task.Run(() => _tramiteProyectoSgpServicio.GuardarProyectosTramiteNegocio(DatosTramiteProyectosDto, RequestContext.Principal.Identity.Name));

                var respuesta = new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    ReasonPhrase = ServiciosNegocioRecursos.PostExitoso
                };

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

        [Route("api/TramiteProyectoSGP/ValidacionProyectosTramiteNegocio")]
        [SwaggerResponse(HttpStatusCode.OK, "Retorna validación Proyectos Tramite Negocio", typeof(HttpResponseMessage))]
        [HttpGet]
        public async Task<IHttpActionResult> ValidacionProyectosTramiteNegocio([FromUri] int TramiteId)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                                   RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                                   ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                                   ConfigurationManager.AppSettings["GestionTramitesProyectos"]).Result;

                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                var TokenAutorizacion = Request.Headers.Authorization.ToString();
                var result = await Task.Run(() => _tramiteProyectoSgpServicio.ValidacionProyectosTramiteNegocio(TramiteId));

                return Ok(result);
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}