using System.Collections.Generic;
using Swashbuckle.Swagger.Annotations;
using System.Configuration;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;

using DNP.ServiciosNegocio.Comunes;
using DNP.ServiciosNegocio.Comunes.Autorizacion;
using DNP.ServiciosNegocio.Comunes.Excepciones;
using DNP.ServiciosNegocio.Servicios.Interfaces.SGR.Viabilidad;


namespace DNP.ServiciosNegocio.Web.API.Controllers.SGR.Viabilidad
{ 
    using System.Net.Http;
    
    [Route("api/[controller]")]
    
    public class AcuerdoController : ApiController
    {
        private readonly IAcuerdoServicio _datosServicios;
        private readonly IAutorizacionUtilidades _autorizacionUtilidades;

        public AcuerdoController(IAcuerdoServicio datosServicios, IAutorizacionUtilidades autorizacionUtilidades)
        {
            _datosServicios = datosServicios;
            _autorizacionUtilidades = autorizacionUtilidades;
        }

       
    [Route("SGR/Acuerdo/LeerProyecto")]
    [SwaggerResponse(HttpStatusCode.OK, "Lee el acuerdo, sectores y clasificadores de un proyecto.", typeof(string))]
    [HttpGet]
        public async Task<IHttpActionResult> SGR_Acuerdo_LeerProyecto(int proyectoId, System.Guid nivelId)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                            RequestContext.Principal.Identity.GetHashCode().ToString(), ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                            ConfigurationManager.AppSettings["SGR_Acuerdo_LeerProyecto"]).Result;

                if (!respuestaAutorizacion.IsSuccessStatusCode)
                    return ResponseMessage(respuestaAutorizacion);

                var result = await Task.Run(() => _datosServicios.SGR_Acuerdo_LeerProyecto (proyectoId, nivelId));
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


        [Route("SGR/Acuerdo/GuardarProyecto")]
        [HttpPost]
        public async Task<IHttpActionResult> SGR_Acuerdo_GuardarProyecto(string json)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name, 
                                            RequestContext.Principal.Identity.GetHashCode().ToString(), ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"], 
                                            ConfigurationManager.AppSettings["SGR_Acuerdo_GuardarProyecto"]).Result;
                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                _datosServicios.Usuario = RequestContext.Principal.Identity.Name;
                _datosServicios.Ip = UtilidadesApi.GetClientIp(Request);

                var result = await Task.Run(() => _datosServicios.SGR_Acuerdo_GuardarProyecto(json, RequestContext.Principal.Identity.Name));
                return Ok(result);
            }
            catch (ServiciosNegocioException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }
    }
}