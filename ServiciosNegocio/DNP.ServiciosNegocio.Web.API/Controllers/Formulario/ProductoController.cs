namespace DNP.ServiciosNegocio.Web.API.Controllers.Formulario
{
    using Comunes;
    using Comunes.Autorizacion;
    using Comunes.Dto;
    using Comunes.Dto.Formulario;
    using Comunes.Excepciones;
    using Dominio.Dto.Proyectos;
    using Servicios.Interfaces.Formulario;
    using Swashbuckle.Swagger.Annotations;
    using System;
    using System.Configuration;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Http;
    using System.Web.Util;

    public class ProductoController : ApiController
    {
        private readonly IProductosServicio _productosServicio;
        private readonly IAutorizacionUtilidades _autorizacionUtilidades;


        public ProductoController(IProductosServicio productosServicio, IAutorizacionUtilidades autorizacionUtilidades)
        {
            _productosServicio = productosServicio;
            _autorizacionUtilidades = autorizacionUtilidades;
            HttpEncoder.Current = HttpEncoder.Default;

        }


        [Route("api/Producto")]
        [SwaggerResponse(HttpStatusCode.OK, "Informacion del producto", typeof(ProyectoDto))]
        [HttpGet]
        public async Task<IHttpActionResult> Consultar()
        {
            try
            {

                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                                                   RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                                                   ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                                                   ConfigurationManager.AppSettings["consultarProducto"]).Result;

                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);


                ValidarParametrosEstandarGet(Request);

                var parametrosConsultaDto = new ParametrosConsultaDto
                                            {
                                                AccionId =
                                                    Guid.Parse(Request.
                                                               Headers.GetValues("piip-idAccion").
                                                               ToList()[0]),
                                                InstanciaId =
                                                    Guid.Parse(Request.
                                                               Headers.
                                                               GetValues("piip-idInstanciaFlujo").
                                                               ToList()[0]),
                                                Bpin = HttpUtility.ParseQueryString(Request.RequestUri.Query).Get("bpin")
                                            };


                var result = await Task.Run(() => _productosServicio.Obtener(parametrosConsultaDto));
                return Ok(result);
            }
            catch (ServiciosNegocioException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest,e)) ;
            }
            catch (AutorizacionException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, e));
            }

        }


        [Route("api/Producto/Preview")]
        [SwaggerResponse(HttpStatusCode.OK, "Informacion del producto preview", typeof(ProyectoDto))]
        [HttpGet]
        public async Task<IHttpActionResult> Preview()
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                                                   RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                                                   ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                                                   ConfigurationManager.AppSettings["previewProducto"]).Result;

                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);


                var result = await Task.Run(() => _productosServicio.ObtenerProductosPreview());
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

        [Route("api/Producto")]
        [SwaggerResponse(HttpStatusCode.OK, "Informacion devuelta por guardado definitivo", typeof(bool))]
        [HttpPost]
        public async Task<IHttpActionResult> Definitivo(ProyectoDto contenido)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                                                   RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                                                   ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                                                   ConfigurationManager.AppSettings["postProductoDefinitivo"]).Result;

                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);
                var parametrosGuardar = _productosServicio.ConstruirParametrosGuardado(Request, contenido);

                var parametrosAuditoria = new ParametrosAuditoriaDto
                                          {
                                              Usuario = RequestContext.Principal.Identity.Name,
                                              Ip = UtilidadesApi.GetClientIp(Request)
                                          };

                await Task.Run(() => _productosServicio.Guardar(parametrosGuardar, parametrosAuditoria, false));

                return Ok(true);
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

        [Route("api/Producto/Temporal")]
        [SwaggerResponse(HttpStatusCode.OK, "Informacion devuelta por guardado temporal", typeof(bool))]
        [HttpPost]
        public async Task<IHttpActionResult> Temporal(ProyectoDto contenido)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                                                   RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                                                   ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                                                   ConfigurationManager.AppSettings["postProductoTemporal"]).Result;

                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);
                var parametrosGuardar = _productosServicio.ConstruirParametrosGuardado(Request, contenido);

                var parametrosAuditoria = new ParametrosAuditoriaDto
                                          {
                                              Usuario = RequestContext.Principal.Identity.Name,
                                              Ip = UtilidadesApi.GetClientIp(Request)
                                          };

                await Task.Run(() => _productosServicio.Guardar(parametrosGuardar, parametrosAuditoria, true));

                return Ok(true);
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

        private void ValidarParametrosEstandarGet( HttpRequestMessage peticion)
        {

            if (string.IsNullOrEmpty(HttpUtility.ParseQueryString(peticion.RequestUri.Query).Get("bpin")))
                throw new ServiciosNegocioException(string.Format(ServiciosNegocioRecursos.ParametroNoRecibido, "bpin"));

            if (!peticion.Headers.Contains("piip-idInstanciaFlujo"))
                throw new  ServiciosNegocioException(string.Format(ServiciosNegocioRecursos.ParametroNoRecibido, "piip-idInstanciaFlujo"));

            if (!peticion.Headers.Contains("piip-idAccion"))
                throw new ServiciosNegocioException(string.Format(ServiciosNegocioRecursos.ParametroNoRecibido, "piip-idAccion"));

            // ReSharper disable once UnusedVariable
            if (!long.TryParse(HttpUtility.ParseQueryString(peticion.RequestUri.Query).Get("bpin"), out var outGuidBpin))
                throw new ServiciosNegocioException(string.Format(ServiciosNegocioRecursos.ParametroNoValidos, "bpin"));

            if (string.IsNullOrEmpty(peticion.Headers.GetValues("piip-idInstanciaFlujo").ToList()[0]))
            throw new  ServiciosNegocioException(string.Format(ServiciosNegocioRecursos.ParametroNoRecibido, "piip-idInstanciaFlujo"));

            // ReSharper disable once UnusedVariable
            if (!Guid.TryParse(peticion.Headers.GetValues("piip-idInstanciaFlujo").ToList()[0], out var outGuid))
                throw new  ServiciosNegocioException(string.Format(ServiciosNegocioRecursos.ParametroNoValidos, "piip-idInstanciaFlujo"));

            if (string.IsNullOrEmpty(peticion.Headers.GetValues("piip-idAccion").ToList()[0]))
                throw new  ServiciosNegocioException(string.Format(ServiciosNegocioRecursos.ParametroNoRecibido, "piip-idAccion"));

            // ReSharper disable once UnusedVariable
            if (!Guid.TryParse(peticion.Headers.GetValues("piip-idAccion").ToList()[0], out var outGuidAccionId))
                throw new ServiciosNegocioException(string.Format(ServiciosNegocioRecursos.ParametroNoValidos, "piip-idAccion"));

        }
    }
}
