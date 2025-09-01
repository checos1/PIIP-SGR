using DNP.Backbone.Comunes.Dto;
using DNP.Backbone.Comunes.Excepciones;
using DNP.Backbone.Dominio.Dto;
using DNP.Backbone.Dominio.Dto.CentroAyuda;
using DNP.Backbone.Servicios.Interfaces.Autorizacion;
using DNP.Backbone.Servicios.Interfaces.CentroAyuda;
using DNP.Backbone.Web.API.Controllers.Base;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace DNP.Backbone.Web.API.Controllers.Entidades
{
    public class CentroAyudaController : BackboneBase
    {

        private readonly ICentroAyudaServicio _centroAyudaServicios;

        public CentroAyudaController(ICentroAyudaServicio centroAyudaServicios, IAutorizacionServicios autorizacionUtilidades) : base(autorizacionUtilidades)
        {
            _centroAyudaServicios = centroAyudaServicios;
        }

        /// <summary>
        /// Listar los temas de ayudas por FiltroDto
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/CentroAyuda/ObtenerListaTemas")]
        public async Task<IHttpActionResult> ObtenerListaTemas([FromBody] AyudaTemaFiltroDto dto)
        {
            try
            {
                var result = await Task.Run(() => _centroAyudaServicios.ObtenerListaTemas(dto, UsuarioLogadoDto.IdUsuario));
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
        /// Crear o Actualizar un tema de ayuda
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/CentroAyuda/CrearActualizarTema")]
        public async Task<IHttpActionResult> CrearActualizarTema([FromBody] AyudaTemaListaItemDto dto)
        {
            try
            {
                var result = await Task.Run(() => _centroAyudaServicios.CrearActualizarTema(dto, UsuarioLogadoDto.IdUsuario));
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
        /// Elimina un tema por id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("api/CentroAyuda/EliminarTema/{id}")]
        public async Task<IHttpActionResult> EliminarTema(int id)
        {
            try
            {
                var result = await Task.Run(() => _centroAyudaServicios.EliminarTema(id, UsuarioLogadoDto.IdUsuario));
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
    }
}