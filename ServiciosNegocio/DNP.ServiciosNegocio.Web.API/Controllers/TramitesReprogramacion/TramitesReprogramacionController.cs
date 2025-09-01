using DNP.ServiciosNegocio.Comunes;
using DNP.ServiciosNegocio.Comunes.Autorizacion;
using Swashbuckle.Swagger.Annotations;
using System.Configuration;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;

namespace DNP.ServiciosNegocio.Web.API.Controllers.Negocio
{
    using Comunes.Dto.Formulario;
    using DNP.ServiciosNegocio.Dominio.Dto.TramiteIncorporacion;
    using DNP.ServiciosNegocio.Dominio.Dto.Tramites.TramitesReprogramacion;
    using DNP.ServiciosNegocio.Servicios.Interfaces.TramitesReprogramacion;
    using System;
    using System.Net.Http;

    public class TramitesReprogramacionController : ApiController
    {
        private readonly ITramitesReprogramacionServicio _TramitesReprogramacionServicio;
        private readonly IAutorizacionUtilidades _autorizacionUtilidades;

        public TramitesReprogramacionController(ITramitesReprogramacionServicio TramitesReprogramacionServicioServicio, IAutorizacionUtilidades autorizacionUtilidades)
        {
            _TramitesReprogramacionServicio = TramitesReprogramacionServicioServicio;
            _autorizacionUtilidades = autorizacionUtilidades;
        }

        [Route("api/TramitesReprogramacion/ObtenerResumenReprogramacionPorVigencia")]
        [SwaggerResponse(HttpStatusCode.OK, "Retorna listado de Datos Reprogramacion", typeof(string))]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerResumenReprogramacionPorVigencia(Nullable<Guid> instanciaId, int proyectoId, int tramiteId)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                                               RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                                               ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                                               ConfigurationManager.AppSettings["ObtenerResumenReprogramacionPorVigencia"]).Result;

                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                var result = await Task.Run(() => _TramitesReprogramacionServicio.ObtenerResumenReprogramacionPorVigencia(instanciaId, proyectoId, tramiteId));
                if (result != null) return Ok(result);

                var respuestaHttp = new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.NotFound,
                    ReasonPhrase = ServiciosNegocioRecursos.SinResultados
                };

                return ResponseMessage(respuestaHttp);
            }
            catch (Exception e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/TramitesReprogramacion/GuardarDatosReprogramacion")]
        [SwaggerResponse(HttpStatusCode.OK, " Funcion para Registrar los Datos Reprogramacion", typeof(DatosReprogramacionDto))]
        [HttpPost]
        public async Task<IHttpActionResult> GuardarDatosReprogramacion(DatosReprogramacionDto objConvenioDonanteDto)
        {
            string usuario = RequestContext.Principal.Identity.Name;

            var parametrosGuardar = new ParametrosGuardarDto<DatosReprogramacionDto>();
            parametrosGuardar.Contenido = objConvenioDonanteDto;

            var result = await Task.Run(() => _TramitesReprogramacionServicio.GuardarDatosReprogramacion(parametrosGuardar, usuario));
            if (result != null) return Ok(result);

            var respuestaHttp = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.NotFound,
                ReasonPhrase = ServiciosNegocioRecursos.SinResultados
            };

            return ResponseMessage(respuestaHttp);
        }

        [Route("api/TramitesReprogramacion/ObtenerResumenReprogramacionPorProductoVigencia")]
        [SwaggerResponse(HttpStatusCode.OK, "Retorna listado de datos reprogramacion por producto vigencia", typeof(string))]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerResumenReprogramacionPorProductoVigencia(Nullable<Guid> instanciaId, int? proyectoId, int tramiteId)
        {
            try
            {
                //var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                //                                                               RequestContext.Principal.Identity.GetHashCode().ToString(),
                //                                                               ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                //                                                               ConfigurationManager.AppSettings["ObtenerResumenReprogramacionPorProductoVigencia"]).Result;

                //if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);
                if(proyectoId == 0)
                {
                    proyectoId = null;
                }
                var result = await Task.Run(() => _TramitesReprogramacionServicio.ObtenerResumenReprogramacionPorProductoVigencia(instanciaId, proyectoId, tramiteId));

                return Ok(result);
            }
            catch (Exception e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }
    }
}