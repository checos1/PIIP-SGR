

namespace DNP.EncabezadoPie.Web.API.Controllers
{
    using Servicios.Interfaces.EncabezadoPieBasico;
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

    public class EncabezadoPieController : ApiController
    {
        private readonly IEncabezadPieoBasicoServicio _encabezadoPieServicio;
        private readonly IAutorizacionUtilidades _autorizacionUtilidades;

        public EncabezadoPieController(IEncabezadPieoBasicoServicio encabezadoPieServicio, IAutorizacionUtilidades autorizacionUtilidades)
        {
            _encabezadoPieServicio = encabezadoPieServicio;
            _autorizacionUtilidades = autorizacionUtilidades;
        }

        [Route("api/EncabezadoPieBasico")]
        [SwaggerResponse(HttpStatusCode.OK, "Retorna Encabezado/Pie Basico", typeof(EncabezadoPieBasicoDto))]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerEncabezadoPieBasico([FromUri]ParametrosEncabezadoPieDto parametros)
        {
            var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                                               RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                                               ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                                               ConfigurationManager.AppSettings["ObtenerArchivos"]).Result;

            if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

            var result = await Task.Run(() => _encabezadoPieServicio.ConsultarEncabezadoPieBasico(parametros));
            if (result != null)
                return Ok(result);

            throw new ServiciosNegocioException(string.Format(ServiciosNegocioRecursos.RespuestaSinResultados));
        }

        [Route("api/EncabezadoPieBasico/Preview")]
        [SwaggerResponse(HttpStatusCode.OK, "Informacion del producto preview", typeof(EncabezadoPieBasicoDto))]
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

                
                var result = await Task.Run(() => _encabezadoPieServicio.ConsultarEncabezadoPieBasicoPreview());
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

        [Route("api/EncabezadoPieBasico/General")]
        [SwaggerResponse(HttpStatusCode.OK, "Retorna Encabezado General para formularios", typeof(EncabezadoGeneralDto))]
        [HttpPost]
        public async Task<IHttpActionResult> ObtenerEncabezadoGeneral([FromBody] ParametrosEncabezadoGeneral parametros)
        {
            var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                                               RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                                               ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                                               ConfigurationManager.AppSettings["previewEncabezado"]).Result;

            if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

            var result = await Task.Run(() => _encabezadoPieServicio.ObtenerEncabezadoGeneral(parametros));
            if (result != null)
                return Ok(result);

            throw new ServiciosNegocioException(string.Format(ServiciosNegocioRecursos.RespuestaSinResultados));
        }
    }
}