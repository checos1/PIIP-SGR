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
    public class DelegadoController : Base.BackboneBase
    {

        private readonly IAutorizacionServicios _autorizacionServicios;

        public DelegadoController(IAutorizacionServicios autorizacionServicios) : base(autorizacionServicios)
        {
            _autorizacionServicios = autorizacionServicios;
        }

        [HttpGet]
        [Route("api/Delegado/ObtenerPorEntidadId")]
        public async Task<IHttpActionResult> ObtenerPorEntidadId([FromUri] Guid idEntidad)
        {
            var result = await Task.Run(() => _autorizacionServicios.ObtenerDelegadosPorEntidadId(idEntidad, UsuarioLogadoDto.IdUsuario));
            return Ok(result);
        }
        
        [HttpPost]
        [Route("api/Delegado/Guardar")]
        public async Task<IHttpActionResult> Guardar(DelegadoDto delegadoDto)
        {
            try
            {
                var respuesta = await Task.Run(() => _autorizacionServicios.GuardarDelegado(delegadoDto, UsuarioLogadoDto.IdUsuario));
                return Ok(respuesta);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [HttpPost]
        [Route("api/Delegado/Eliminar")]
        public async Task<IHttpActionResult> Eliminar([FromUri] int idDelegado)
        {
            try
            {
                var respuesta = await Task.Run(() => _autorizacionServicios.EliminarDelegado(idDelegado, UsuarioLogadoDto.IdUsuario));
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