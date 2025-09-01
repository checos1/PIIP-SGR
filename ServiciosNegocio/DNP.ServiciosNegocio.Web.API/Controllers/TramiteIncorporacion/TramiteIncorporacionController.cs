using DNP.ServiciosNegocio.Comunes;
using DNP.ServiciosNegocio.Comunes.Autorizacion;
using DNP.ServiciosNegocio.Dominio.Dto.FuenteFinanciacion;
using DNP.ServiciosNegocio.Servicios.Interfaces.FuenteFinanciacion;
using Swashbuckle.Swagger.Annotations;
using System.Configuration;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;

namespace DNP.ServiciosNegocio.Web.API.Controllers.Negocio
{
    using Comunes.Dto.Formulario;
    using DNP.ServiciosNegocio.Dominio.Dto.TramiteIncorporacion;
    using DNP.ServiciosNegocio.Servicios.Interfaces.TramiteIncorporacion;
    using System.Net.Http;

    public class TramiteIncorporacionController : ApiController
    {
        private readonly ITramiteIncorporacionServicio _tramiteIncorporacionServicio;
        private readonly IAutorizacionUtilidades _autorizacionUtilidades;

        public TramiteIncorporacionController(ITramiteIncorporacionServicio tramiteIncorporacionServicioServicio, IAutorizacionUtilidades autorizacionUtilidades)
        {
            _tramiteIncorporacionServicio = tramiteIncorporacionServicioServicio;
            _autorizacionUtilidades = autorizacionUtilidades;
        }

        [Route("api/TramiteIncorporacion/ObtenerDatosIncorporacion")]
        [SwaggerResponse(HttpStatusCode.OK, "Retorna listado de Datos Incorporacion", typeof(ConvenioDonanteDto))]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerDatosIncorporacion(int tramiteId)
        {
            var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                                               RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                                               ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                                               ConfigurationManager.AppSettings["ObtenerDatosIncorporacion"]).Result;

            if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

            var result = await Task.Run(() => _tramiteIncorporacionServicio.ObtenerDatosIncorporacion(tramiteId));
            if (result != null) return Ok(result);

            var respuestaHttp = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.NotFound,
                ReasonPhrase = ServiciosNegocioRecursos.SinResultados
            };

            return ResponseMessage(respuestaHttp);
        }


        [Route("api/TramiteIncorporacion/GuardarDatosIncorporacion")]
        [SwaggerResponse(HttpStatusCode.OK, " Funcion para Registrar los Datos Incorporacion", typeof(ConvenioDonanteDto))]
        [HttpPost]
        public async Task<IHttpActionResult> GuardarDatosIncorporacion(ConvenioDonanteDto objConvenioDonanteDto)
        {
            var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                                               RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                                               ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                                               ConfigurationManager.AppSettings["GuardarDatosIncorporacion"]).Result;

            if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

            string usuario =RequestContext.Principal.Identity.Name;

            var parametrosGuardar = new ParametrosGuardarDto<ConvenioDonanteDto>();
            parametrosGuardar.Contenido = objConvenioDonanteDto;

            var result = await Task.Run(() => _tramiteIncorporacionServicio.GuardarDatosIncorporacion(parametrosGuardar, usuario)); 
            if (result != null) return Ok(result);

            var respuestaHttp = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.NotFound,
                ReasonPhrase = ServiciosNegocioRecursos.SinResultados
            };

            return ResponseMessage(respuestaHttp);
        }

        [Route("api/TramiteIncorporacion/EiliminarDatosIncorporacion")]
        [SwaggerResponse(HttpStatusCode.OK, " Funcion para Eiliminar los Datos Incorporacion", typeof(ConvenioDonanteDto))]
        [HttpPost]
        public async Task<IHttpActionResult> EiliminarDatosIncorporacion(ConvenioDonanteDto objConvenioDonanteDto)
        {
            var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                                               RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                                               ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                                               ConfigurationManager.AppSettings["EiliminarDatosIncorporacion"]).Result;

            if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);
            string usuario = RequestContext.Principal.Identity.Name;
            var parametrosGuardar = new ParametrosGuardarDto<ConvenioDonanteDto>();
            parametrosGuardar.Contenido = objConvenioDonanteDto;

            var result = await Task.Run(() => _tramiteIncorporacionServicio.EiliminarDatosIncorporacion(parametrosGuardar, usuario));
            if (result != null) return Ok(result);

            var respuestaHttp = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.NotFound,
                ReasonPhrase = ServiciosNegocioRecursos.SinResultados
            };

            return ResponseMessage(respuestaHttp);
        }

    }
}