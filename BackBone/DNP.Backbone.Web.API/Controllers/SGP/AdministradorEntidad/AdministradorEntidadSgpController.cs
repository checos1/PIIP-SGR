using DNP.Backbone.Comunes.Excepciones;
using DNP.Backbone.Servicios.Interfaces.Autorizacion;
using DNP.Backbone.Servicios.Interfaces.SGP.AdministradorEntidad;
using DNP.Backbone.Web.API.Controllers.Base;
using System.Net.Http;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using DNP.Backbone.Comunes.Dto;

namespace DNP.Backbone.Web.API.Controllers
{
    public class AdministradorEntidadSgpController : BackboneBase
    {
        private readonly IAutorizacionServicios _autorizacionUtilidades;
        private readonly IAdministradorEntidadSgpServicios _administradorEntidadSgpServicios;

        /// <summary>
        /// Constructor de clases
        /// </summary>
        /// <param name="autorizacionUtilidades">Instancia de servicios de autorización</param>
        /// <param name="administradorEntidadSgpServicios">Instancia de servicios de administrador de Entidad</param>
        public AdministradorEntidadSgpController(IAutorizacionServicios autorizacionUtilidades, IAdministradorEntidadSgpServicios administradorEntidadSgpServicios)
            : base(autorizacionUtilidades)
        {
            this._autorizacionUtilidades = autorizacionUtilidades;
            this._administradorEntidadSgpServicios = administradorEntidadSgpServicios;
        }

        /// <summary>
        /// Api para obtener lista de sectores para las entidades.
        /// </summary>
        /// <returns>Lista de sectores para las entidades</returns>
        [HttpGet]
        [Route("api/AdministradorEntidadSGP/ObtenerSectores")]
        public async Task<IHttpActionResult> ObtenerSectores()
        {
            try
            {
                var result = await Task.Run(() => _administradorEntidadSgpServicios.ObtenerSectores(UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        /// <summary>
        /// Api para obtener lista de catalogos.
        /// </summary>
        /// <returns>Lista de catalogos</returns>
        [HttpGet]
        [Route("api/AdministradorEntidadSGP/ObtenerFlowCatalog")]
        public async Task<IHttpActionResult> ObtenerFlowCatalog()
        {
            try
            {
                var result = await Task.Run(() => _administradorEntidadSgpServicios.ObtenerFlowCatalog(UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        /// <summary>
        /// Api para obtener la Matriz de entidades.
        /// </summary>
        /// <param name="peticion">Contiene informacion de autorizacion</param>
        /// <returns>Matriz de entidades</returns>
        [Route("api/AdministradorEntidadSGP/ObtenerMatrizEntidadDestino")]
        [HttpPost]
        public async Task<IHttpActionResult> ObtenerMatrizEntidadDestino(ListMatrizEntidadDestinoDto peticion)
        {
            try
            {
                peticion.IdUsuario = UsuarioLogadoDto.IdUsuario;
                var result = await _administradorEntidadSgpServicios.ObtenerMatrizEntidadDestino(peticion).ConfigureAwait(false);

                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        /// <summary>
        /// Api para actualizar la Matriz de entidades.
        /// </summary>
        /// <param name="peticion">Contiene informacion de autorizacion</param>
        /// <returns>Matriz de entidades</returns>
        [Route("api/AdministradorEntidadSGP/ActualizarMatrizEntidadDestino")]
        [HttpPost]
        public async Task<IHttpActionResult> ActualizarMatrizEntidadDestino(ListaMatrizEntidadUnidadDto peticion)
        {
            try
            {
                var result = await _administradorEntidadSgpServicios.ActualizarMatrizEntidadDestino(peticion, UsuarioLogadoDto.IdUsuario).ConfigureAwait(false);

                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }
    }
}