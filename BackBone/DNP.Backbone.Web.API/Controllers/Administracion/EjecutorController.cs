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

    public class EjecutorController : ApiController
    {
        public HttpResponseMessage RespuestaAutorizacion;
        private readonly IEjecutorServicio _ObjServicios;
        private readonly IAutorizacionServicios _autorizacionUtilidades;

        public EjecutorController(IEjecutorServicio ObjServicios, IAutorizacionServicios autorizacionUtilidades)
        {
            _ObjServicios = ObjServicios;
            _autorizacionUtilidades = autorizacionUtilidades;
        }

       
        [Route("api/administracion/ConsultarEjecutor")]
        [HttpGet]
        public async Task<IHttpActionResult> ConsultarEjecutor(string nit)
        {
            try
            {
                var result = await Task.Run(() => _ObjServicios.ConsultarEjecutor(nit, User.Identity.Name));

                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/administracion/GuardarEjecutor")]
        [HttpPost]
        public async Task<bool> GuardarEjecutor(EjecutorDto Obj)
        {
            try
            {
                var UsuarioDNP = User.Identity.Name;
                var respuesta = await Task.Run(() => _ObjServicios.GuardarEjecutor(Obj, UsuarioDNP));
                return respuesta;

            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }
    }
}