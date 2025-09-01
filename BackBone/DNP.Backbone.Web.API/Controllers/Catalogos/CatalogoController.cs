using System.Threading.Tasks;
using System.Web.Http;

namespace DNP.Backbone.Web.API.Controllers.Catalogos
{
    using DNP.Backbone.Comunes.Excepciones;
    using DNP.Backbone.Comunes.Enums;
    using Servicios.Interfaces.Autorizacion;
    using DNP.Backbone.Servicios.Interfaces.Catalogos;
    using System.Net;
    using System.Net.Http;

    public class CatalogoController : ApiController
    {
        public HttpResponseMessage RespuestaAutorizacion;
        private readonly ICatalogoServicio _ObjServicios;
        private readonly IAutorizacionServicios _autorizacionUtilidades;

        public CatalogoController(ICatalogoServicio ObjServicios, IAutorizacionServicios autorizacionUtilidades)
        {
            _ObjServicios = ObjServicios;
            _autorizacionUtilidades = autorizacionUtilidades;
        }

       
        [Route("api/Catalogo/TodosTiposEntidades")]
        [HttpGet]
        public async Task<IHttpActionResult> CatalogoTodosTiposEntidades()
        {
            try
            {
                var result = await Task.Run(() => _ObjServicios.consultarCatalogo(User.Identity.Name, CatalogoEnum.TodosTiposEntidades));

                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/Catalogo/ObtenerTablasBasicas")]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerTablasBasicas(string jsonCondicion, string Tabla)
        {
            try
            {
                var result = await Task.Run(() => _ObjServicios.ObtenerTablasBasicas(jsonCondicion, Tabla, User.Identity.Name));

                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }
    }
}