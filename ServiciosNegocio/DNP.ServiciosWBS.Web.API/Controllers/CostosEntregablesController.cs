namespace DNP.ServiciosWBS.Web.API.Controllers
{
    using System;
    using System.Configuration;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web.Http;
    using Servicios.Interfaces;
    using ServiciosNegocio.Comunes;
    using ServiciosNegocio.Comunes.Autorizacion;
    using ServiciosNegocio.Comunes.Dto;
    using ServiciosNegocio.Comunes.Dto.Formulario;
    using ServiciosNegocio.Comunes.Excepciones;
    using ServiciosNegocio.Comunes.Utilidades;
    using Swashbuckle.Swagger.Annotations;
    using DNP.ServiciosNegocio.Dominio.Dto.CostosEntregables;

    public class CostosEntregablesController : ApiController
    {
        private readonly ICostosEntregablesServicios _costosEntregablesServicios;
        private readonly IAutorizacionUtilidades _autorizacionUtilidades;

        public CostosEntregablesController(ICostosEntregablesServicios costosEntregablesServicios, IAutorizacionUtilidades autorizacionUtilidades)
        {
            _costosEntregablesServicios = costosEntregablesServicios;
            _autorizacionUtilidades = autorizacionUtilidades;
        }

        [Route("api/DNP_SN_Eje_Ajustes_CostosEntregablesDto")]
        [SwaggerResponse(HttpStatusCode.OK, "Retorna costo de las entregables", typeof(CostosEntregablesDto))]
        [HttpGet]
        public async Task<IHttpActionResult> Consultar(string bpin)
        {
            var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                                               RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                                               ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                                               ConfigurationManager.AppSettings["serviciosWBS"]).Result;

            if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

            ValidacionParametro(bpin, Request);


            var result = await Task.Run(() => _costosEntregablesServicios.ObtenerCostosEntregables(
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

            long outBpin;
            if (!long.TryParse(bpin, out outBpin) || bpin.Length > 100)
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

        [Route("api/DNP_SN_Eje_Ajustes_CostosEntregablesDto/Preview")]
        [SwaggerResponse(HttpStatusCode.OK, "Retorna costo de Entregables", typeof(CostosEntregablesDto))]
        [HttpGet]
        public async Task<IHttpActionResult> Preview()
        {
            var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                                               RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                                               ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                                               ConfigurationManager.AppSettings["serviciosWBS"]).Result;

            if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

            var result = await Task.Run(() => _costosEntregablesServicios.ObtenerCostosEntregablesPreview());
            return Ok(result);
        }

        [Route("api/DNP_SN_Eje_Ajustes_CostosEntregablesDto")]
        [SwaggerResponse(HttpStatusCode.OK, "Informacion devuelta por guardado definitivo", typeof(HttpResponseMessage))]
        [HttpPost]
        public async Task<IHttpActionResult> Definitivo(CostosEntregablesDto costosEntregablesDto)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name, 
                                                                                    RequestContext.Principal.Identity.GetHashCode().ToString(), 
                                                                                    ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"], 
                                                                                    ConfigurationManager.AppSettings["serviciosWBS"]).Result;
                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                var parametrosGuardar = _costosEntregablesServicios.ConstruirParametrosGuardado(Request, costosEntregablesDto);

                var parametrosAuditoria = new ParametrosAuditoriaDto
                {
                    Usuario = RequestContext.Principal.Identity.Name,
                    Ip = UtilidadesApi.GetClientIp(Request)
                };

                await Task.Run(() => _costosEntregablesServicios.Guardar(parametrosGuardar, parametrosAuditoria, false));

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

        [Route("api/DNP_SN_Eje_Ajustes_CostosEntregablesDto/Temporal")]
        [SwaggerResponse(HttpStatusCode.OK, "Informacion devuelta por guardado definitivo", typeof(HttpResponseMessage))]
        [HttpPost]
        public async Task<IHttpActionResult> Temporal(CostosEntregablesDto costosEntregablesDto)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name, 
                                                                                    RequestContext.Principal.Identity.GetHashCode().ToString(), 
                                                                                    ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"], 
                                                                                    ConfigurationManager.AppSettings["serviciosWBS"]).Result;
                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                var parametrosGuardar = _costosEntregablesServicios.ConstruirParametrosGuardado(Request, costosEntregablesDto);

                var parametrosAuditoria = new ParametrosAuditoriaDto
                {
                    Usuario = RequestContext.Principal.Identity.Name,
                    Ip = UtilidadesApi.GetClientIp(Request)
                };

                await Task.Run(() => _costosEntregablesServicios.Guardar(parametrosGuardar, parametrosAuditoria, true));

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