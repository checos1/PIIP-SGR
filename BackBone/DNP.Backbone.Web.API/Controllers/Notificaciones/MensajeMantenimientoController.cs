using DNP.Backbone.Comunes.Dto;
using DNP.Backbone.Comunes.Excepciones;
using DNP.Backbone.Dominio.Dto.MensajeMantenimiento;
using DNP.Backbone.Servicios.Interfaces.Autorizacion;
using DNP.Backbone.Servicios.Interfaces.MensajesMantenimiento;
using DNP.Backbone.Web.API.Controllers.Base;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace DNP.Backbone.Web.API.Controllers
{
    public class MensajeMantenimientoController : BackboneBase
    {
        private readonly IMensajeMantenimientoServicio _mensajeMantenimientoServicio;

        public MensajeMantenimientoController(IAutorizacionServicios autorizacionUtilidades, IMensajeMantenimientoServicio mensajeMantenimientoServicio) : base(autorizacionUtilidades)
        {
            _mensajeMantenimientoServicio = mensajeMantenimientoServicio;
        }

        /// <summary>
        /// Listar las mensajes de mantenimiento
        /// </summary>
        /// <returns></returns>
        [Route("api/MensajeMantenimiento/ObtenerListaMensajes")]
        [HttpPost]
        public async Task<IHttpActionResult> ObtenerListaMensajes(ParametrosMensajeMantenimiento parametros)
        {
            try
            {
                parametros.ParametrosDto.IdUsuarioDNP = UsuarioLogadoDto.IdUsuario;
                var result = await Task.Run(() => _mensajeMantenimientoServicio.ObtenerListaMensajes(parametros));
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
        /// Crear nueva mensaje de mantenimiento o actualizar mensaje existente
        /// </summary>
        /// <param name="mensaje"></param>
        /// <returns></returns>
        [Route("api/MensajeMantenimiento/CrearActualizar")]
        [HttpPost]
        public async Task<IHttpActionResult> CrearActualizar(ParametrosMensajeMantenimiento parametros)
        {
            try
            {
                var result = await Task.Run(() => _mensajeMantenimientoServicio.CrearActualizarMensaje(parametros));
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
        /// Eliminar mensaje de mantenimiento por una lista de ids
        /// </summary>
        /// </param name="idsMensajes"></param>
        /// <returns></returns>
        [Route("api/MensajeMantenimiento/Eliminar")]
        [HttpDelete]
        public async Task<IHttpActionResult> EliminarMensaje(ParametrosMensajeMantenimiento parametros)
        {
            try
            {
                await Task.Run(() => _mensajeMantenimientoServicio.EliminarMensaje(parametros));
                return Ok();
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
    }
}