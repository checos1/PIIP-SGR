using DNP.Backbone.Comunes.Dto;
using DNP.Backbone.Comunes.Excepciones;
using DNP.Backbone.Dominio.Dto;
using DNP.Backbone.Dominio.Dto.UsuarioNotificacion;
using DNP.Backbone.Servicios.Interfaces.Autorizacion;
using DNP.Backbone.Servicios.Interfaces.UsuarioNotificacion;
using DNP.Backbone.Web.API.Controllers.Base;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;

namespace DNP.Backbone.Web.API.Controllers.Notificaciones
{
    [RoutePrefix("api/UsuarioNotificacion")]
    public class UsuarioNotificacionConfigController : BackboneBase
    {
        private readonly IUsuarioNotificacionConfigServicio _servicio;

        public UsuarioNotificacionConfigController(IAutorizacionServicios autorizacionUtilidades, IUsuarioNotificacionConfigServicio servicio) : base(autorizacionUtilidades)
        {
            _servicio = servicio;
        }

        /// <summary>
        /// Listar las configuraciones de notificaciones
        /// </summary>
        /// <returns></returns>
        [Route("ObtenerListaConfig")]
        [HttpPost]
        public async Task<IHttpActionResult> ObtenerListaConfig([FromBody]UsuarioNotificacionConfigFiltroDto filtro)
        {
            try
            {
                var result = await Task.Run(() => _servicio.OtenerConfigNotificaciones(filtro, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException ex)
            {
                var response = Request.CreateResponse(HttpStatusCode.BadRequest, new RespuestaViewModel(ex.Erros, HttpStatusCode.BadRequest));
                throw new HttpResponseException(response);
            }
            catch (Exception ex)
            {
                var response = Request.CreateResponse(HttpStatusCode.InternalServerError, new RespuestaViewModel(ex.Message, HttpStatusCode.InternalServerError));
                throw new HttpResponseException(response);
            }
        }

        /// <summary>
        /// Crear nueva configuración de notificación o actualizar una configuración existente
        /// </summary>
        /// <param name="mensaje"></param>
        /// <returns></returns>
        [Route("CrearActualizar")]
        [HttpPost]
        public async Task<IHttpActionResult> CrearActualizar(UsuarioNotificacionConfigDto dto)
        {
            try
            {
                var result = await Task.Run(() => _servicio.CrearActualizarConfigNotificacion(dto, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException ex)
            {
                var response = Request.CreateResponse(HttpStatusCode.BadRequest, new RespuestaViewModel(ex.Erros, HttpStatusCode.BadRequest));
                throw new HttpResponseException(response);
            }
            catch (Exception ex)
            {
                var response = Request.CreateResponse(HttpStatusCode.InternalServerError, new RespuestaViewModel(ex.Message, HttpStatusCode.InternalServerError));
                throw new HttpResponseException(response);
            }
        }

        /// <summary>
        /// Eliminar configuración de notificación por una lista de ids
        /// </summary>
        /// </param name="idsMensajes"></param>
        /// <returns></returns>
        [Route("Eliminar")]
        [HttpDelete]
        public async Task<IHttpActionResult> Eliminar([FromUri]int[] ids)
        {
            try
            {
                var respuesta = await Task.Run(() => _servicio.EliminarConfigNotificacion(UsuarioLogadoDto.IdUsuario, ids));
                return Ok(respuesta);
            }
            catch (BackboneException ex)
            {
                var response = Request.CreateResponse(HttpStatusCode.BadRequest, new RespuestaViewModel(ex.Erros, HttpStatusCode.BadRequest));
                throw new HttpResponseException(response);
            }
            catch (Exception ex)
            {
                var response = Request.CreateResponse(HttpStatusCode.InternalServerError, new RespuestaViewModel(ex.Message, HttpStatusCode.InternalServerError));
                throw new HttpResponseException(response);
            }
        }

        /// <summary>
        /// Marca una lista de notificaciones del usuários por filtro
        /// </summary>
        /// <returns></returns>
        [Route("MarcarNotificacionComoLeida")]
        [HttpPost]
        public async Task<IHttpActionResult> MarcarNotificacionComoLeida(UsuarioNotificacionFiltroDto filtro)
        {
            try
            {
                var result = await Task.Run(() => _servicio.MarcarNotificacionComoLeida(filtro, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException ex)
            {
                var response = Request.CreateResponse(HttpStatusCode.BadRequest, new RespuestaViewModel(ex.Erros, HttpStatusCode.BadRequest));
                throw new HttpResponseException(response);
            }
            catch (Exception ex)
            {
                var response = Request.CreateResponse(HttpStatusCode.InternalServerError, new RespuestaViewModel(ex.Message, HttpStatusCode.InternalServerError));
                throw new HttpResponseException(response);
            }
        }

        /// <summary>
        /// Obtener todos los procedimientos de almacenados disponibles
        /// </summary>
        /// <param name="IdUsuarioDNP"></param>
        /// <returns></returns>
        [Route("ObtenerProcedimentosAlmacenados")]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerProcedimentosAlmacenados()
        {
            try
            {
                var response = await Task.Run(() => _servicio.ObtenerProcedimentosAlmacenados(UsuarioLogadoDto.IdUsuario));

                return Ok(response);
            }
            catch (BackboneException ex)
            {
                var response = Request.CreateResponse(HttpStatusCode.BadRequest, new RespuestaViewModel(ex.Erros, HttpStatusCode.BadRequest));
                throw new HttpResponseException(response);
            }
            catch (Exception ex)
            {
                var response = Request.CreateResponse(HttpStatusCode.InternalServerError, new RespuestaViewModel(ex.Message, HttpStatusCode.InternalServerError));
                throw new HttpResponseException(response);
            }
        }

        /// <summary>
        /// Regresa una lista de usuarios a partir de um procedimiento
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("ObtenerUsuariosPorProcedimentoAlmacenado/{id}")]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerUsuariosPorProcedimentoAlmacenado(string id)
        {
            try
            {
                var usuarios = await Task.Run(() => _servicio.ObtenerUsuariosPorProcedimentoAlmacenado(id, UsuarioLogadoDto.IdUsuario));
                return Ok(usuarios);
            }
            catch (BackboneException ex)
            {
                var response = Request.CreateResponse(HttpStatusCode.BadRequest, new RespuestaViewModel(ex.Erros, HttpStatusCode.BadRequest));
                throw new HttpResponseException(response);
            }
            catch (Exception ex)
            {
                var response = Request.CreateResponse(HttpStatusCode.InternalServerError, new RespuestaViewModel(ex.Message, HttpStatusCode.InternalServerError));
                throw new HttpResponseException(response);
            }
        }

        [Route("ObtenerExcelNotificaciones")]
        [HttpPost]
        public async Task<HttpResponseMessage> ObtenerExcelNotificaciones(IList<UsuarioNotificacionConfigDto> parametros)
        {

            try
            {
                HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);

                var _result = parametros;

                result.StatusCode = HttpStatusCode.OK;
                result.Content = ExcelUtilidades.ObtenerExcellNotificaciones(_result);
                result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
                //result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment; filename = Proyectos.xlsx");

                return result;
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }
    }
}