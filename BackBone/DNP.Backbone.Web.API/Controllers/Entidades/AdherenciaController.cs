using DNP.Backbone.Comunes.Excepciones;
using DNP.Backbone.Dominio.Dto;
using DNP.Backbone.Dominio.Dto.AutorizacionNegocio;
using DNP.Backbone.Servicios.Interfaces.Autorizacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;

namespace DNP.Backbone.Web.API.Controllers.Entidades
{
    public class AdherenciaController : Base.BackboneBase
    {

        private readonly IAutorizacionServicios _autorizacionServicios;

        public AdherenciaController(IAutorizacionServicios autorizacionServicios) : base(autorizacionServicios)
        {
            _autorizacionServicios = autorizacionServicios;
        }

        [HttpGet]
        [Route("api/Adherencia/ObtenerAdherenciasPorEntidadId")]
        public async Task<IHttpActionResult> ObtenerEntidadPorEntidadId([FromUri] Guid idEntidad)
        {
            var result = await Task.Run(() => _autorizacionServicios.ObtenerAdherenciasPorEntidadId(idEntidad, UsuarioLogadoDto.IdUsuario));
            return Ok(result);
        }
        
        [HttpPost]
        [Route("api/Adherencia/Guardar")]
        public async Task<IHttpActionResult> Guardar(AdherenciaDto adherenciaDto)
        {
            try
            {
                var respuesta = await Task.Run(() => _autorizacionServicios.GuardarAdherencia(adherenciaDto, UsuarioLogadoDto.IdUsuario));
                return Ok(respuesta);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [HttpPost]
        [Route("api/Adherencia/Eliminar")]
        public async Task<IHttpActionResult> Eliminar([FromUri] int idAdherencia)
        {
            try
            {
                var respuesta = await Task.Run(() => _autorizacionServicios.EliminarAdherencia(idAdherencia, UsuarioLogadoDto.IdUsuario));
                if (!respuesta.Exito)
                {
                    return Content(HttpStatusCode.Conflict, respuesta);
                }

                return Ok(respuesta);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }



    }
}