namespace DNP.EncabezadoPie.Web.API.Controllers
{
    using Servicios.Interfaces.PriorizacionRecurso;
    using ServiciosNegocio.Comunes.Excepciones;
    using DNP.ServiciosNegocio.Comunes.Autorizacion;
    using Dominio.Dto;
    using System.Web.Http;
    using Swashbuckle.Swagger.Annotations;
    using System.Net;
    using System.Threading.Tasks;
    using System.Configuration;
    using DNP.ServiciosNegocio.Comunes;
    using System.Net.Http;
    using System;
    using System.Linq;
    using System.Diagnostics.CodeAnalysis;
    using DNP.EncabezadoPie.Dominio.Dto.PriorizacionRecurso;
    using DNP.ServiciosNegocio.Comunes.Dto.Formulario;

    public class PriorizacionRecursoController : ApiController
    {
        private readonly IPriorizacionRecursoServicio _priorizacionRecursoServicio;
        private readonly IAutorizacionUtilidades _autorizacionUtilidades;

        public PriorizacionRecursoController(IPriorizacionRecursoServicio priorizacionRecursoServicio, IAutorizacionUtilidades autorizacionUtilidades)
        {
            _priorizacionRecursoServicio = priorizacionRecursoServicio;
            _autorizacionUtilidades = autorizacionUtilidades;
        }

        [Route("api/PriorizacionRecurso")]
        [SwaggerResponse(HttpStatusCode.OK, "Retorna listado de Priorizacion Recurso", typeof(PriorizacionRecursoDto))]
        [HttpGet]
        public async Task<IHttpActionResult> Consultar(string bpin)
        {
            var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                                               RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                                               ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                                               ConfigurationManager.AppSettings["consultarPriorizacionRecurso"]).Result;

            if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

            ValidacionParametro(bpin, Request);

            var result = await Task.Run(() => _priorizacionRecursoServicio.ObtenerPriorizacionRecurso(
                                                                                                        new ParametrosConsultaDto
                                                                                                        {
                                                                                                            Bpin =
                                                                                                                bpin,
                                                                                                            AccionId
                                                                                                                = new
                                                                                                                    Guid(Request.
                                                                                                                        Headers.
                                                                                                                        GetValues("piip-idAccion").
                                                                                                                        First()),
                                                                                                            InstanciaId
                                                                                                                = new
                                                                                                                    Guid(Request.
                                                                                                                        Headers.
                                                                                                                        GetValues("piip-idInstanciaFlujo").
                                                                                                                        First())
                                                                                                        }

                                                                                                        ));
            if (result != null) return Ok(result);

            var respuestaHttp = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.NotFound,
                ReasonPhrase = ServiciosNegocioRecursos.SinResultados
            };

            return ResponseMessage(respuestaHttp);
        }

        [SuppressMessage("ReSharper", "UnusedVariable")]
        private void ValidacionParametro(string bpin, HttpRequestMessage peticion)
        {
            if (string.IsNullOrEmpty(bpin))
                throw new ServiciosNegocioException(string.Format(ServiciosNegocioRecursos.ParametroNoRecibido, nameof(bpin)));

            if (!long.TryParse(bpin, out var outBpin) || bpin.Length > 100)
                throw new ServiciosNegocioException(string.Format(ServiciosNegocioRecursos.ParametroNoValidos, nameof(bpin)));

            if (peticion.Headers.Contains("piip-idFormulario"))
            {
                if (string.IsNullOrEmpty(peticion.Headers.GetValues("piip-idFormulario").First()))
                    throw new ServiciosNegocioException(string.Format(ServiciosNegocioRecursos.ParametroNoRecibido, "piip-idFormulario"));

                Guid outGuidAccionId = Guid.Empty;

                if (!Guid.TryParse(peticion.Headers.GetValues("piip-idFormulario").First(), out outGuidAccionId))
                    throw new ServiciosNegocioException(string.Format(ServiciosNegocioRecursos.ParametroNoValidos, "piip-idFormulario"));
            }
            else
            {
                throw new ServiciosNegocioException(string.Format(ServiciosNegocioRecursos.ParametroNoRecibido,
                                                             "piip-idFormulario"));
            }
        }

        [Route("api/PriorizacionRecurso/Preview")]
        [SwaggerResponse(HttpStatusCode.OK, "Retorna listado de Priorizacion Recursos priview", typeof(PriorizacionRecursoDto))]
        [HttpGet]
        public async Task<IHttpActionResult> Preview()
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                                                   RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                                                   ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                                                   ConfigurationManager.AppSettings["previewPriorizacionRecurso"]).Result;

                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);


                var result = await Task.Run(() => _priorizacionRecursoServicio.ObtenerPriorizacionRecursoPreview());
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
    }
}