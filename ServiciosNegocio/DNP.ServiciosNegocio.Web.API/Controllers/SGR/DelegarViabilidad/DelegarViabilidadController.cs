using DNP.ServiciosNegocio.Comunes;
using DNP.ServiciosNegocio.Comunes.Autorizacion;
using DNP.ServiciosNegocio.Comunes.Dto.Viabilidad;
using DNP.ServiciosNegocio.Comunes.Excepciones;
using DNP.ServiciosNegocio.Servicios.Interfaces.SGR.DelegarViabilidad;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace DNP.ServiciosNegocio.Web.API.Controllers.SGR.DelegarViabilidad
{
    [Route("api/[controller]")]
    public class DelegarViabilidadController : ApiController
    {
        private readonly IDelegarViabilidadServicio _delegarViabilidadServicio;
        private readonly IAutorizacionUtilidades _autorizacionUtilidades;

        public DelegarViabilidadController(IDelegarViabilidadServicio delegarViabilidadServicio, IAutorizacionUtilidades autorizacionUtilidades)
        {
            _delegarViabilidadServicio = delegarViabilidadServicio;
            _autorizacionUtilidades = autorizacionUtilidades;
        }

        [Route("SGR/DelegarViabilidad/ObtenerProyecto")]
        [SwaggerResponse(HttpStatusCode.OK, "Obtener información proyecto delegar viabilidad", typeof(HttpResponseMessage))]
        [HttpGet]
        public async Task<IHttpActionResult> SGR_DelegarViabilidad_ObtenerProyecto(string bpin, Nullable<Guid> instanciaId)
        {
            var result = await Task.Run(() => _delegarViabilidadServicio.SGR_DelegarViabilidad_ObtenerProyecto(bpin, instanciaId));

            return Ok(result);
        }

        [Route("SGR/DelegarViabilidad/ObtenerEntidadesDelegar")]
        [SwaggerResponse(HttpStatusCode.OK, "Obtener información entidades delegar viabilidad", typeof(HttpResponseMessage))]
        [HttpGet]
        public async Task<IHttpActionResult> SGR_DelegarViabilidad_ObtenerEntidades(string bpin)
        {
            var result = await Task.Run(() => _delegarViabilidadServicio.SGR_DelegarViabilidad_ObtenerEntidades(bpin));

            return Ok(result);
        }

        [Route("SGR/DelegarViabilidad/RegistrarDelegarViabilidad")]
        [SwaggerResponse(HttpStatusCode.OK, "Registrar información delegar viabilidad", typeof(HttpResponseMessage))]
        [HttpPost]
        public async Task<IHttpActionResult> SGR_DelegarViabilidad_Registrar([FromBody] DelegarViabilidadDto json)
        {
            try
            {
                var usuario = RequestContext.Principal.Identity.Name;
                var response = await Task.Run(() => _delegarViabilidadServicio.SGR_DelegarViabilidad_Registrar(json, usuario));
                return Ok(response);
            }
            catch (ServiciosNegocioException e)
            {
                return CrearRespuestaError(e.Message);
            }
        }

        #region Respuestas Servicio        
        private IHttpActionResult Responder(object listaProyecto)
        {
            return listaProyecto != null ? Ok(listaProyecto) : CrearRespuestaNoFound();
        }

        private IHttpActionResult CrearRespuestaNoFound()
        {
            var respuestaHttp = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.NotFound,
                ReasonPhrase = ServiciosNegocioRecursos.SinResultados
            };

            return ResponseMessage(respuestaHttp);
        }

        private IHttpActionResult CrearRespuestaError(string message)
        {
            var respuestaHttp = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.BadRequest,
                ReasonPhrase = message
            };

            return ResponseMessage(respuestaHttp);
        }
        #endregion
    }
}