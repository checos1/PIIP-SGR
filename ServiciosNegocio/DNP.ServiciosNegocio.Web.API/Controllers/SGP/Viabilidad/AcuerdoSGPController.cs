using System.Collections.Generic;
using Swashbuckle.Swagger.Annotations;
using System.Configuration;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;

using DNP.ServiciosNegocio.Comunes;
using DNP.ServiciosNegocio.Comunes.Autorizacion;
using DNP.ServiciosNegocio.Comunes.Excepciones;
using DNP.ServiciosNegocio.Servicios.Interfaces.SGP.Viabilidad;


namespace DNP.ServiciosNegocio.Web.API.Controllers.SGP.Viabilidad
{ 
    using System.Net.Http;
    
    [Route("api/[controller]")]
    
    public class AcuerdoSGPController : ApiController
    {
        private readonly IAcuerdoSGPServicio _datosServicios;
        private readonly IAutorizacionUtilidades _autorizacionUtilidades;

        public AcuerdoSGPController(IAcuerdoSGPServicio datosServicios, IAutorizacionUtilidades autorizacionUtilidades)
        {
            _datosServicios = datosServicios;
            _autorizacionUtilidades = autorizacionUtilidades;
        }

       
    [Route("SGP/Acuerdo/LeerProyecto")]
    [SwaggerResponse(HttpStatusCode.OK, "Lee el acuerdo, sectores y clasificadores de un proyecto.", typeof(string))]
    [HttpGet]
       public async Task<IHttpActionResult> SGPAcuerdoLeerProyecto(int proyectoId, System.Guid nivelId)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                            RequestContext.Principal.Identity.GetHashCode().ToString(), ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                            ConfigurationManager.AppSettings["SGPAcuerdoLeerProyecto"]).Result;

                if (!respuestaAutorizacion.IsSuccessStatusCode)
                    return ResponseMessage(respuestaAutorizacion);

                var result = await Task.Run(() => _datosServicios.SGPAcuerdoLeerProyecto(proyectoId, nivelId));
                if (result != null) return Ok(result);

                var respuestaHttp = new System.Net.Http.HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.NotFound,
                    ReasonPhrase = ServiciosNegocioRecursos.SinResultados
                };

                return ResponseMessage(respuestaHttp);
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


        [Route("SGP/Acuerdo/GuardarProyecto")]
        [HttpPost]
        public async Task<IHttpActionResult> SGPAcuerdoGuardarProyecto(string json)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name, 
                                            RequestContext.Principal.Identity.GetHashCode().ToString(), ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"], 
                                            ConfigurationManager.AppSettings["SGPAcuerdoGuardarProyecto"]).Result;
                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                _datosServicios.Usuario = RequestContext?.Principal?.Identity?.Name;
                _datosServicios.Ip = UtilidadesApi.GetClientIp(Request);
                var result = await Task.Run(() => _datosServicios.SGPAcuerdoGuardarProyecto(json, RequestContext.Principal.Identity.Name));
                return Ok(result);
            }
            catch (ServiciosNegocioException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("SGP/Proyectos/LeerListas")]
        [SwaggerResponse(HttpStatusCode.OK, "Lee el acuerdo, sectores y clasificadores de un proyecto.", typeof(string))]
        [HttpGet]
        public async Task<IHttpActionResult> SGPProyectosLeerListas(System.Guid nivelId, int proyectoId, string nombreLista)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                            RequestContext.Principal.Identity.GetHashCode().ToString(), ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                            ConfigurationManager.AppSettings["SGPProyectosLeerListas"]).Result;

                if (!respuestaAutorizacion.IsSuccessStatusCode)
                    return ResponseMessage(respuestaAutorizacion);

                var result = await Task.Run(() => _datosServicios.SGPProyectosLeerListas(nivelId, proyectoId, nombreLista));
                if (result != null) return Ok(result);

                var respuestaHttp = new System.Net.Http.HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.NotFound,
                    ReasonPhrase = ServiciosNegocioRecursos.SinResultados
                };

                return ResponseMessage(respuestaHttp);
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