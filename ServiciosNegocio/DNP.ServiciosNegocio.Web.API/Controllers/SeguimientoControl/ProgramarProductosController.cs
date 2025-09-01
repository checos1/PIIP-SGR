using DNP.ServiciosNegocio.Comunes;
using DNP.ServiciosNegocio.Comunes.Autorizacion;
using DNP.ServiciosNegocio.Comunes.Dto;
using DNP.ServiciosNegocio.Comunes.Dto.Formulario;
using DNP.ServiciosNegocio.Comunes.Excepciones;
using DNP.ServiciosNegocio.Dominio.Dto.ProgramarProducto;
using DNP.ServiciosNegocio.Dominio.Dto.Proyectos;
using DNP.ServiciosNegocio.Dominio.Dto.SeguimientoControl;
using DNP.ServiciosNegocio.Servicios.Interfaces.SeguimientoControl;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace DNP.ServiciosNegocio.Web.API.Controllers.SeguimientoControl
{
    public class ProgramarProductosController : ApiController
    {

        private readonly IProgramarProductosServicio _ProgramarProductosServicio;
        private readonly IAutorizacionUtilidades _autorizacionUtilidades;

        #region Costructor
        public ProgramarProductosController(IProgramarProductosServicio ProgramarProductosServicio,
            IAutorizacionUtilidades autorizacionUtilidades
            )
        {
            _ProgramarProductosServicio = ProgramarProductosServicio;
            _autorizacionUtilidades = autorizacionUtilidades;
        }
        #endregion

        /// <summary>
        /// Método que obtiene el listado de Objetivos - Productos - Niveles - Productos
        /// </summary>
        /// <param name="guiMacroproceso">Identificador GUID de la tabla [Transversal].[fase]</param>
        /// <returns></returns>
        [Route("api/SeguimientoControl/ProgramarProductos/ObtenerListadoObjProdNiveles")]
        [SwaggerResponse(HttpStatusCode.OK, "Programar Productos - Retorna listado de objetivos, productos y niveles para realizar seguimiento y control", typeof(HttpResponseMessage))]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerListadoObjProdNiveles(string bpin)
        {
            var result = await Task.Run(() => _ProgramarProductosServicio.ObtenerListadoObjProdNiveles(bpin));
            return result != null ? Ok(result) : CrearRespuestaNoFound();
        }

        /// <summary>
        /// tratamiento para HTTP Status Code 404
        /// </summary>        
        /// <returns>IHttpActionResult</returns>
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

        [Route("api/SeguimientoControl/ProgramarProductos/GuardarProgramarProducto")]
        [SwaggerResponse(HttpStatusCode.OK, "Informacion devuelta por guardado definitivo", typeof(HttpResponseMessage))]
        [HttpPost]
        public async Task<IHttpActionResult> GuardarProgramarProducto(ProgramarProductoDto objscDto)
        {
            try
            {
                var parametrosGuardar = new ParametrosGuardarDto<ProgramarProductoDto>();
                parametrosGuardar.Contenido = objscDto;


                var parametrosAuditoria = new ParametrosAuditoriaDto
                {
                    Usuario = RequestContext.Principal.Identity.Name,
                    Ip = UtilidadesApi.GetClientIp(Request)
                };

                var result = await Task.Run(() => _ProgramarProductosServicio.GuardarProgramarProducto(parametrosGuardar, RequestContext.Principal.Identity.Name));

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
    }
}
