using DNP.ServiciosNegocio.Comunes;
using DNP.ServiciosNegocio.Comunes.Autorizacion;
using DNP.ServiciosNegocio.Comunes.Excepciones;
using DNP.ServiciosNegocio.Dominio.Dto.FuenteFinanciacion;
using DNP.ServiciosNegocio.Servicios.Interfaces.FuenteFinanciacion;
using Swashbuckle.Swagger.Annotations;
using System.Configuration;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;

namespace DNP.ServiciosNegocio.Web.API.Controllers.Negocio
{
    using System;
    using System.Linq;
    using System.Net.Http;
    using Comunes.Dto;
    using Comunes.Dto.Formulario;
    using DNP.ServiciosNegocio.Persistencia.Interfaces.FuenteFinanciacion;

    public class DatosAdicionalesController : ApiController
    {
        private readonly IDatosAdicionalesServicio _DatosAdicionalesServicio;
        private readonly IAutorizacionUtilidades _autorizacionUtilidades;


        public DatosAdicionalesController(IDatosAdicionalesServicio DatosAdicionalesServicio, IAutorizacionUtilidades autorizacionUtilidades)
        {
            _DatosAdicionalesServicio = DatosAdicionalesServicio;
            _autorizacionUtilidades = autorizacionUtilidades;
        }

        [Route("api/FuenteFinanciacionDatosAdicionales/Consultar")]
        [SwaggerResponse(HttpStatusCode.OK, "Retorna listado con los Datos Adicionales de una Fuentes de Financiacion", typeof(DatosAdicionalesDto))]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerDatosAdicionalesFuenteFinanciacion(int fuenteId)
        {
            var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                                               RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                                               ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                                               ConfigurationManager.AppSettings["ObtenerDatosAdicionalesFuente"]).Result;

            if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

            var result = await Task.Run(() => _DatosAdicionalesServicio.ObtenerDatosAdicionalesFuenteFinanciacion(fuenteId));
            if (result != null) return Ok(result);

            var respuestaHttp = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.NotFound,
                ReasonPhrase = ServiciosNegocioRecursos.SinResultados
            };

            return ResponseMessage(respuestaHttp);
        }

        [Route("api/FuenteFinanciacionDatosAdicionales/Agregar")]
        [SwaggerResponse(HttpStatusCode.OK, "Informacion devuelta por guardado definitivo", typeof(HttpResponseMessage))]
        [HttpPost]
        public async Task<IHttpActionResult> Definitivo(DatosAdicionalesDto objDatosAdicionalesDto)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                    RequestContext.Principal.Identity.GetHashCode().ToString(), ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                    ConfigurationManager.AppSettings["postDatosAdicionalesAgregar"]).Result;
                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                var parametrosGuardar = new ParametrosGuardarDto<DatosAdicionalesDto>();
                parametrosGuardar.Contenido = objDatosAdicionalesDto;


                var parametrosAuditoria = new ParametrosAuditoriaDto
                {
                    Usuario = RequestContext.Principal.Identity.Name,
                    Ip = UtilidadesApi.GetClientIp(Request)
                };

                var result = await Task.Run(() => _DatosAdicionalesServicio.GuardarDatosAdicionales(parametrosGuardar, RequestContext.Principal.Identity.Name));

                var respuesta = new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    ReasonPhrase = ServiciosNegocioRecursos.PostExitoso
                };

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

        [Route("api/FuenteFinanciacionDatosAdicionales/Eliminar")]
        [SwaggerResponse(HttpStatusCode.OK, "Eliminar Datos adicionales de la fuente de Financiacion", typeof(HttpResponseMessage))]
        [HttpPost]
        public async Task<IHttpActionResult> Eliminar(int coFinanciadorId)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name, 
                    RequestContext.Principal.Identity.GetHashCode().ToString(), ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                    ConfigurationManager.AppSettings["postDatosAdicionalesEliminar"]).Result;
                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                var parametrosAuditoria = new ParametrosAuditoriaDto
                {
                    Usuario = RequestContext.Principal.Identity.Name,
                    Ip = UtilidadesApi.GetClientIp(Request)
                };

                await Task.Run(() => _DatosAdicionalesServicio.EliminarDatosAdicionales(coFinanciadorId));

                var respuesta = new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    ReasonPhrase = ServiciosNegocioRecursos.PostExitoso
                };

                return Ok(respuesta);
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