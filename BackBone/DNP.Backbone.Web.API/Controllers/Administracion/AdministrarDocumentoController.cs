using System.Threading.Tasks;
using System.Web.Http;

namespace DNP.Backbone.Web.API.Controllers.Administracion
{
    using DNP.Backbone.Comunes.Excepciones;
    using DNP.Backbone.Dominio.Dto.Administracion;
    using Servicios.Interfaces.Autorizacion;
    using DNP.Backbone.Servicios.Interfaces.Administracion;
    using System.Net;
    using System.Net.Http;
    using Swashbuckle.Swagger.Annotations;
    using System.Configuration;    


    public class AdministrarDocumentoController : ApiController
    {
        private readonly IAdministrarDocumentoServicio _datosServicios;
        private readonly IAutorizacionServicios _autorizacionUtilidades;


        public AdministrarDocumentoController(IAdministrarDocumentoServicio datosServicios)
        {
            _datosServicios = datosServicios;
        }

        
        [Route("api/administracion/ConsultarDocumento")]
        [HttpGet]
        public async Task<string> ConsultarDocumento(string NombreDocumento)
        {
            try
            {
                var result = await Task.Run(() => _datosServicios.AdministrarDocumentoConsultar(User.Identity.Name,NombreDocumento));

                return result;
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }
     
        [Route("api/administracion/CrearDocumento")]
        [HttpPost]
        public async Task<IHttpActionResult> DocumentoCrear(AdministracionDocumentoDto Documento)
        {
            try
            {
               
                var resultado = await Task.Run(() => _datosServicios.AdministrarDocumentoCrear(User.Identity.Name,Documento));

                var respuesta = new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,

                };

                return Ok(resultado);
            }
            catch (AutorizacionException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, e));
            }
        }

        [Route("api/administracion/ActulizarDocumento")]
        [SwaggerResponse(HttpStatusCode.OK, "Actualizar un documento.", typeof(HttpResponseMessage))]
        [HttpPut]
        public async Task<IHttpActionResult> DocumentoActualizar(AdministracionDocumentoDto Documento)
        {
            try
            {
                var resultado = await Task.Run(() => _datosServicios.AdministrarDocumentoActualizar(User.Identity.Name, Documento));

                var respuesta = new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                };

                return Ok(resultado);
            }
            catch (AutorizacionException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, e));
            }
        }

        [Route("api/administracion/EliminarDocumento")]
        [SwaggerResponse(HttpStatusCode.OK, "Elimina un documento.", typeof(HttpResponseMessage))]
        [HttpDelete]
        public async Task<IHttpActionResult> DocumentoEliminar(string IdDocumento)
        {
            try
            {
                
                var resultado = await Task.Run(() => _datosServicios.AdministrarDocumentoEliminar(User.Identity.Name,IdDocumento));

                var respuesta = new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    // ReasonPhrase = ServiciosNegocioRecursos.PostExitoso,
                };

                return Ok(resultado);
            }
            /*catch (ServiciosNegocioException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }*/
            catch (AutorizacionException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, e));
            }
        }
        [Route("api/administracion/EstadoDocumento")]
        [HttpPost]
        public async Task<IHttpActionResult> DocumentoEstado(AdministracionDocumentoDto Documento)
        {
            try
            {
                // var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name, RequestContext.Principal.Identity.GetHashCode().ToString(), ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"], ConfigurationManager.AppSettings["Administracion_GuardarEjecutor"]).Result;
                // if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                var resultado = await Task.Run(() => _datosServicios.AdministrarDocumentoEstado(User.Identity.Name, Documento));

                var respuesta = new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    //ReasonPhrase = ServiciosNegocioRecursos.PostExitoso,
                };

                return Ok(resultado);
            }
            /*catch (ServiciosNegocioException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }*/
            catch (AutorizacionException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, e));
            }
        }
        [Route("api/administracion/ReferenciasDocumento")]
        [HttpGet]
        public async Task<string> DocumentoReferencias()
        {
            try
            {
                var result = await Task.Run(() => _datosServicios.AdministrarDocumentoReferencias(User.Identity.Name));

                return result;
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }
        [Route("api/administracion/ConsultarUsoDocumento")]
        [HttpGet]
        public async Task<string> ConsultarUsoDocumento()
        {
            try
            {
                var result = await Task.Run(() => _datosServicios.AdministrarDocumentoConsultarUso(User.Identity.Name));

                return result;
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/administracion/CrearUsoDocumento")]
        [SwaggerResponse(HttpStatusCode.OK, "Crear Uso de Documento.", typeof(HttpResponseMessage))]
        [HttpPost]
        public async Task<IHttpActionResult> DocumentoUsoCrear(AdministracionDocumentoUsoDto Documento)
        {
            try
            {
                var resultado = await Task.Run(() => _datosServicios.AdministrarCrearUsoDocumento(User.Identity.Name, Documento));

                var respuesta = new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                };

                return Ok(resultado);
            }
            catch (AutorizacionException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, e));
            }
        }

        [Route("api/administracion/ActualizarUsoDocumento")]
        [SwaggerResponse(HttpStatusCode.OK, "Actualizar Usos de Documento.", typeof(HttpResponseMessage))]
        [HttpPut]
        public async Task<IHttpActionResult> DocumentoUsoActualizar(AdministracionDocumentoUsoDto Documento)
        {
            try
            {
                var resultado = await Task.Run(() => _datosServicios.AdministrarActualizarUsoDocumento(User.Identity.Name, Documento));

                var respuesta = new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                };

                return Ok(resultado);
            }
            catch (AutorizacionException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, e));
            }
        }

        [Route("api/administracion/EliminarUsoDocumento")]
        [SwaggerResponse(HttpStatusCode.OK, "Elimina un documento.", typeof(HttpResponseMessage))]
        [HttpDelete]
        public async Task<IHttpActionResult> DocumentoUsoEliminar(string Id)
        {
            try
            {
                var resultado = await Task.Run(() => _datosServicios.AdministrarEliminarUsoDocumento(User.Identity.Name, Id));
                var respuesta = new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                };
                return Ok(resultado);
            }
            catch (AutorizacionException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, e));
            }
        }
    }
}