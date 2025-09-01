
namespace DNP.ServiciosNegocio.Web.API.Controllers.Negocio
{

    using System;
    using System.Linq;
    using System.Net.Http;
    using Comunes.Dto;
    using Comunes.Dto.Formulario;
    using DNP.ServiciosNegocio.Comunes;
    using DNP.ServiciosNegocio.Comunes.Autorizacion;
    using DNP.ServiciosNegocio.Comunes.Excepciones;
    using DNP.ServiciosNegocio.Dominio.Dto.Transferencias;
    using DNP.ServiciosNegocio.Servicios.Interfaces.Proyectos;    
    using Swashbuckle.Swagger.Annotations;
    using System.Configuration;
    using System.Diagnostics.CodeAnalysis;
    using System.Net;
    using System.Threading.Tasks;
    using System.Web.Http;
    using System.Web;
    using DNP.ServiciosNegocio.Dominio.Dto.Genericos;

    public class DevolverProyectoController : ApiController
    {

        private readonly IDevolverProyectoServicio _devolverProyectoServicios;
        private readonly IAutorizacionUtilidades _autorizacionUtilidades;

        public DevolverProyectoController(IDevolverProyectoServicio devolverProyectoServicios, IAutorizacionUtilidades autorizacionUtilidades)
        {
            _devolverProyectoServicios = devolverProyectoServicios;
            _autorizacionUtilidades = autorizacionUtilidades;
        }



        [Route("api/DevolverProyecto")]
        [SwaggerResponse(HttpStatusCode.OK, "Retorna un proyecto a la MGA", typeof(DevolverProyectoDto))]
        [HttpGet]
        public async Task<IHttpActionResult> Consultar(string bpin)
        {
            var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                                               RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                                               ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                                               ConfigurationManager.AppSettings["consultarDevolverProyecto"]).Result;

            if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

            ValidacionParametro(bpin);

            var result = await Task.Run(() => _devolverProyectoServicios.ObtenerDevolverProyecto(
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
        private void ValidacionParametro(string bpin)
        {
            if (string.IsNullOrEmpty(bpin))
                throw new ServiciosNegocioException(string.Format(ServiciosNegocioRecursos.ParametroNoRecibido, nameof(bpin)));

            if (!long.TryParse(bpin, out var outBpin) || bpin.Length > 100)
                throw new ServiciosNegocioException(string.Format(ServiciosNegocioRecursos.ParametroNoValidos, nameof(bpin)));
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


        [Route("api/DevolverProyecto")]
        [SwaggerResponse(HttpStatusCode.OK, "Información devuelta por guardado definitivo", typeof(HttpResponseMessage))]
        [HttpPost]
        public async Task<IHttpActionResult> Definitivo(DevolverProyectoDto devolverProyectoDto)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name, RequestContext.Principal.Identity.GetHashCode().ToString(), ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"], ConfigurationManager.AppSettings["postDevolverProyecto"]).Result;
                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                var parametrosGuardar = _devolverProyectoServicios.ConstruirParametrosGuardado(Request, devolverProyectoDto);

                var parametrosAuditoria = new ParametrosAuditoriaDto
                {
                    Usuario = RequestContext.Principal.Identity.Name,
                    Ip = UtilidadesApi.GetClientIp(Request)
                };

                await Task.Run(() => _devolverProyectoServicios.Guardar(parametrosGuardar, parametrosAuditoria, false));

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

        [Route("api/DevolverProyecto/Devolver")]
        [SwaggerResponse(HttpStatusCode.OK, "Información devuelta por guardado definitivo", typeof(HttpResponseMessage))]
        [HttpPost]
        public async Task<IHttpActionResult> Devolver(DevolverProyectoDto devolverProyectoDto)
        {
            try
            {
                var parametrosGuardar = new ParametrosGuardarDto<DevolverProyectoDto>();

                if (devolverProyectoDto != null)
                {
                    parametrosGuardar.Contenido = devolverProyectoDto;
                    parametrosGuardar.InstanciaId = devolverProyectoDto.InstanciaId;
                }
                else
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NoContent, new Exception(string.Format(ServiciosNegocioRecursos.ParametrosNoRecibidos))));

                var parametrosAuditoria = new ParametrosAuditoriaDto
                {
                    Usuario = RequestContext.Principal.Identity.Name,
                    Ip = UtilidadesApi.GetClientIp(Request)
                };

                await Task.Run(() => _devolverProyectoServicios.Guardar(parametrosGuardar, parametrosAuditoria, false));

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


        [Route("api/DevolverProyecto/Temporal")]
        [SwaggerResponse(HttpStatusCode.OK, "Informacion devuelta por guardado temporal", typeof(HttpResponseMessage))]
        [HttpPost]
        public async Task<IHttpActionResult> Temporal(DevolverProyectoDto devolverProyectoDto)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name, RequestContext.Principal.Identity.GetHashCode().ToString(),
                    ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"], ConfigurationManager.AppSettings["postDevolverProyectoTemporal"]).Result;
                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                var parametrosGuardar = _devolverProyectoServicios.ConstruirParametrosGuardado(Request, devolverProyectoDto);

                var parametrosAuditoria = new ParametrosAuditoriaDto
                {
                    Usuario = RequestContext.Principal.Identity.Name,
                    Ip = UtilidadesApi.GetClientIp(Request)
                };

                await Task.Run(() => _devolverProyectoServicios.Guardar(parametrosGuardar, parametrosAuditoria, true));

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