using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using DNP.ServiciosNegocio.Comunes.Autorizacion;
using DNP.ServiciosNegocio.Comunes.Excepciones;
using DNP.ServiciosNegocio.Dominio.Dto.Proyectos;
using DNP.ServiciosNegocio.Servicios.Interfaces.AdministradorEntidad;
using Swashbuckle.Swagger.Annotations;


namespace DNP.ServiciosNegocio.Web.API.Controllers.SGP.AdministradorEntidad
{
    public class AdministradorEntidadSgpController : ApiController
    {
        private readonly IAdministradorEntidadSgpServicio _administradorEntidadSgpServicio;
        private readonly IAutorizacionUtilidades _autorizacionUtilidades;

        public AdministradorEntidadSgpController(IAdministradorEntidadSgpServicio administradorEntidadSgpServicio, IAutorizacionUtilidades autorizacionUtilidades)
        {
            _administradorEntidadSgpServicio = administradorEntidadSgpServicio;
            _autorizacionUtilidades = autorizacionUtilidades;
        }

        [Route("api/AdministradorEntidadSGP/ObtenerSectores")]
        [SwaggerResponse(HttpStatusCode.OK, "Informacion devuelta ObtenerSectores", typeof(HttpResponseMessage))]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerSectores()
        {
            try
            {
                var result = await Task.Run(() => _administradorEntidadSgpServicio.ObtenerSectores());
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

        [Route("api/AdministradorEntidadSGP/ObtenerFlowCatalog")]
        [SwaggerResponse(HttpStatusCode.OK, "Informacion devuelta ObtenerFlowCatalog", typeof(HttpResponseMessage))]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerFlowCatalog()
        {
            try
            {
                var result = await Task.Run(() => _administradorEntidadSgpServicio.ObtenerFlowCatalog());
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

        [Route("api/AdministradorEntidadSGP/ObtenerMatrizEntidadDestino")]
        [SwaggerResponse(HttpStatusCode.OK, "Informacion devuelta ObtenerMatrizEntidadDestino", typeof(HttpResponseMessage))]
        [HttpPost]
        public async Task<IHttpActionResult> ObtenerMatrizEntidadDestino(ListMatrizEntidadDestinoDto dto)
        {
            try
            {
                return Ok(await Task.Run(() => _administradorEntidadSgpServicio.ObtenerMatrizEntidadDestino(dto, RequestContext.Principal.Identity.Name)));
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

        [Route("api/AdministradorEntidadSGP/ActualizarMatrizEntidadDestino")]
        [SwaggerResponse(HttpStatusCode.OK, "Informacion devuelta ObtenerMatrizEntidadDestino", typeof(HttpResponseMessage))]
        [HttpPost]
        public async Task<IHttpActionResult> ActualizarMatrizEntidadDestino(ListaMatrizEntidadUnidadDto dto)
        {
            try
            {
                _administradorEntidadSgpServicio.Usuario = RequestContext?.Principal?.Identity?.Name;
                _administradorEntidadSgpServicio.Ip = UtilidadesApi.GetClientIp(Request);
                return Ok(await Task.Run(() => _administradorEntidadSgpServicio.ActualizarMatrizEntidadDestino(dto, RequestContext.Principal.Identity.Name)));
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