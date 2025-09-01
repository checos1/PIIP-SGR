using DNP.ServiciosNegocio.Comunes;
using DNP.ServiciosNegocio.Comunes.Autorizacion;
using DNP.ServiciosNegocio.Comunes.Excepciones;
using DNP.ServiciosNegocio.Dominio.Dto.Administracion;
using DNP.ServiciosNegocio.Servicios.Interfaces.Administracion;
using Swashbuckle.Swagger.Annotations;
using System.Configuration;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;

namespace DNP.ServiciosNegocio.Web.API.Controllers.Negocio
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using System.Net.Http;
    using Comunes.Dto;
    using Comunes.Dto.Formulario;

    public class AdministrarDocumentoController : ApiController
    {
        private readonly IAdministrarDocumentoServicio _datosServicios;
        private readonly IAutorizacionUtilidades _autorizacionUtilidades;


        public AdministrarDocumentoController(IAdministrarDocumentoServicio datosServicios, IAutorizacionUtilidades autorizacionUtilidades)
        {
            _datosServicios = datosServicios;
            _autorizacionUtilidades = autorizacionUtilidades;
        }

        [Route("api/administracion/ConsultarDocumento")]
        [SwaggerResponse(HttpStatusCode.OK, "Consultar documentos")]
        [HttpGet]
        public async Task<IHttpActionResult> DocumentoConsultar(string NombreDocumento = null)
        {
           var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name, RequestContext.Principal.Identity.GetHashCode().ToString(), ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"], ConfigurationManager.AppSettings["Administracion_ConsultarEjecutor"]).Result;

            if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);


            var result = await Task.Run(() => _datosServicios.AdministrarDocumentoConsultar(NombreDocumento));
            if (result != null) return Ok(result);

            var respuestaHttp = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.NotFound,
                ReasonPhrase = ServiciosNegocioRecursos.SinResultados
            };

            return ResponseMessage(respuestaHttp);
        }
        
        [Route("api/administracion/CrearDocumento")]
        [SwaggerResponse(HttpStatusCode.OK, "Crear una referencia aun documento", typeof(HttpResponseMessage))]
        [HttpPost]
        public async Task<IHttpActionResult> DocumentoCrear(AdministracionDocumentoDto Documento)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name, RequestContext.Principal.Identity.GetHashCode().ToString(), ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"], ConfigurationManager.AppSettings["Administracion_GuardarEjecutor"]).Result;
                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);
               
                Documento.ModificadoPor = RequestContext.Principal.Identity.Name;
                var resultado = await Task.Run(() => _datosServicios.AdministrarDocumentoCrear(Documento));

                var respuesta = new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    ReasonPhrase = ServiciosNegocioRecursos.PostExitoso,
                };

                return Ok(resultado);
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

        [Route("api/administracion/ActualizarDocumento")]
        [SwaggerResponse(HttpStatusCode.OK, "Actulizar un documento", typeof(HttpResponseMessage))]
        [HttpPut]
        public async Task<IHttpActionResult> DocumentoActualizar(AdministracionDocumentoDto Documento)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name, RequestContext.Principal.Identity.GetHashCode().ToString(), ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"], ConfigurationManager.AppSettings["Administracion_GuardarEjecutor"]).Result;
                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                Documento.ModificadoPor = RequestContext.Principal.Identity.Name;
                var resultado = await Task.Run(() => _datosServicios.AdministrarDocumentoActualizar(Documento));

                var respuesta = new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    ReasonPhrase = ServiciosNegocioRecursos.PostExitoso,
                };

                return Ok(resultado);
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
        [Route("api/administracion/EliminarDocumento")]
        [SwaggerResponse(HttpStatusCode.OK, "Eliminar Documento", typeof(HttpResponseMessage))]
        [HttpDelete]
        public async Task<IHttpActionResult> DocumentoEliminar(string IdDocumento)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name, RequestContext.Principal.Identity.GetHashCode().ToString(), ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"], ConfigurationManager.AppSettings["Administracion_GuardarEjecutor"]).Result;
                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                var resultado = await Task.Run(() => _datosServicios.AdministrarDocumentoEliminar(IdDocumento));

                var respuesta = new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    ReasonPhrase = ServiciosNegocioRecursos.PostExitoso,
                };

                return Ok(resultado);
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
        [Route("api/administracion/EstadoDocumento")]
        [SwaggerResponse(HttpStatusCode.OK, "Crear una referencia aun documento", typeof(HttpResponseMessage))]
        [HttpPost]
        public async Task<IHttpActionResult> DocumentoEstado(AdministracionDocumentoDto Documento)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name, RequestContext.Principal.Identity.GetHashCode().ToString(), ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"], ConfigurationManager.AppSettings["Administracion_GuardarEjecutor"]).Result;
                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                Documento.ModificadoPor = RequestContext.Principal.Identity.Name;
                var resultado = await Task.Run(() => _datosServicios.AdministrarDocumentoEstado(Documento));

                var respuesta = new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    ReasonPhrase = ServiciosNegocioRecursos.PostExitoso,
                };

                return Ok(resultado);
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

        [Route("api/administracion/ReferenciasDocumento")]
        [SwaggerResponse(HttpStatusCode.OK, "Referencias Documento", typeof(HttpResponseMessage))]
        [HttpGet]
        public async Task<IHttpActionResult> DocumentoReferencias()
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name, RequestContext.Principal.Identity.GetHashCode().ToString(), ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"], ConfigurationManager.AppSettings["Administracion_GuardarEjecutor"]).Result;
                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                var resultado = await Task.Run(() => _datosServicios.AdministrarDocumentoReferencias());

                var respuesta = new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    ReasonPhrase = ServiciosNegocioRecursos.PostExitoso,
                };

                return Ok(resultado);
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


        /** 
         * Usos Documento
         * */
        
        [Route("api/administracion/ConsultarUsoDocumento")]
        [SwaggerResponse(HttpStatusCode.OK, "Consultar documentos uso")]
        [HttpGet]
        public async Task<IHttpActionResult> DocumentoConsultarUso()
        {
            var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name, RequestContext.Principal.Identity.GetHashCode().ToString(), ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"], ConfigurationManager.AppSettings["Administracion_ConsultarEjecutor"]).Result;

            if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);


            var result = await Task.Run(() => _datosServicios.AdministrarDocumentoConsultarUso());
            if (result != null) return Ok(result);

            var respuestaHttp = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.NotFound,
                ReasonPhrase = ServiciosNegocioRecursos.SinResultados
            };

            return ResponseMessage(respuestaHttp);
        }

        [Route("api/administracion/CrearUsoDocumento")]
        [SwaggerResponse(HttpStatusCode.OK, "Crear Uso de Documento", typeof(HttpResponseMessage))]
        [HttpPost]
        public async Task<IHttpActionResult> DocumentoUso(AdministracionDocumentoUsoDto Documento)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name, RequestContext.Principal.Identity.GetHashCode().ToString(), ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"], ConfigurationManager.AppSettings["Administracion_GuardarEjecutor"]).Result;
                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                
                Documento.ModificadoPor = RequestContext.Principal.Identity.Name;
                var resultado = await Task.Run(() => _datosServicios.AdministrarCrearUsoDocumento(Documento));
                
                var respuesta = new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    ReasonPhrase = ServiciosNegocioRecursos.PostExitoso,
                };
                
                return Ok(resultado);
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

        [Route("api/administracion/ActualizarUsoDocumento")]
        [SwaggerResponse(HttpStatusCode.OK, "Actualizar Uso de Documento", typeof(HttpResponseMessage))]
        [HttpPut]
        public async Task<IHttpActionResult> DocumentoActualizarUso(AdministracionDocumentoUsoDto Documento)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name, RequestContext.Principal.Identity.GetHashCode().ToString(), ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"], ConfigurationManager.AppSettings["Administracion_GuardarEjecutor"]).Result;
                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);


                Documento.ModificadoPor = RequestContext.Principal.Identity.Name;
                var resultado = await Task.Run(() => _datosServicios.AdministrarActualizarUsoDocumento(Documento));

                var respuesta = new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    ReasonPhrase = ServiciosNegocioRecursos.PostExitoso,
                };

                return Ok(resultado);
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



        [Route("api/administracion/EliminarUsoDocumento")]
        [SwaggerResponse(HttpStatusCode.OK, "Eliminar Uso Documento", typeof(HttpResponseMessage))]
        [HttpDelete]
        public async Task<IHttpActionResult> DocumentoUsoEliminar(string Id)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name, RequestContext.Principal.Identity.GetHashCode().ToString(), ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"], ConfigurationManager.AppSettings["Administracion_GuardarEjecutor"]).Result;
                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                var resultado = await Task.Run(() => _datosServicios.AdministrarDocumentoUsoEliminar(Id));

                var respuesta = new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    ReasonPhrase = ServiciosNegocioRecursos.PostExitoso,
                };

                return Ok(resultado);
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
        
        [Route("api/administracion/EstadoUsoDocumento")]
        [SwaggerResponse(HttpStatusCode.OK, "Actualizar estado uso documento", typeof(HttpResponseMessage))]
        [HttpPost]
        public async Task<IHttpActionResult> DocumentoUsoEstado(AdministracionDocumentoDto Documento)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name, RequestContext.Principal.Identity.GetHashCode().ToString(), ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"], ConfigurationManager.AppSettings["Administracion_GuardarEjecutor"]).Result;
                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                Documento.ModificadoPor = RequestContext.Principal.Identity.Name;
                var resultado = await Task.Run(() => _datosServicios.AdministrarDocumentoEstado(Documento));

                var respuesta = new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    ReasonPhrase = ServiciosNegocioRecursos.PostExitoso,
                };

                return Ok(resultado);
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