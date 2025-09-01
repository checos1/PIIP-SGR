using DNP.Backbone.Comunes.Excepciones;
using DNP.Backbone.Servicios.Interfaces.Autorizacion;
using DNP.Backbone.Web.API.Controllers.Base;
using System.Net.Http;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using DNP.Backbone.Dominio.Dto.ModificacionLey;
using DNP.Backbone.Servicios.Interfaces.ModificacionLey;

/// <summary>
/// Clase Api responsable de la gestión de trámites
/// </summary>
namespace DNP.Backbone.Web.API.Controllers
{
    public class ModificacionLeyController : BackboneBase
    {
        private readonly IAutorizacionServicios _autorizacionUtilidades;
        private readonly IModificacionLeyServicios _modificacionLeyServicios;

        /// <summary>
        /// Constructor de clases
        /// </summary>
        /// <param name="autorizacionUtilidades">Instancia de servicios de autorización</param>
        /// <param name="programacionServicios">Instancia de servicios de programación</param>
        public ModificacionLeyController(IAutorizacionServicios autorizacionUtilidades, IModificacionLeyServicios modificacionLeyServicios)
            : base(autorizacionUtilidades)
        {
            this._autorizacionUtilidades = autorizacionUtilidades;
            this._modificacionLeyServicios = modificacionLeyServicios;
        }

        #region ModificacionLeyAdicion

        /// <summary>
        /// Api para obtener lista de programación de distribucion.
        /// </summary>
        /// <returns>Lista de programación de distribucion</returns>
        [HttpGet]
        [Route("api/ModificacionLeyAdicion/ObtenerInformacionPresupuestalMLEncabezado")]
        public async Task<IHttpActionResult> ObtenerInformacionPresupuestalMLEncabezado(int EntidadDestinoId, int TramiteId, string origen)
        {
            try
            {
                var result = await Task.Run(() => _modificacionLeyServicios.ObtenerInformacionPresupuestalMLEncabezado(EntidadDestinoId, TramiteId, origen, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        /// <summary>
        /// Api para obtener lista de programación de distribucion detalle.
        /// </summary>
        /// <returns>Lista de programación de distribucion detalle</returns>
        [HttpGet]
        [Route("api/ModificacionLeyAdicion/ObtenerInformacionPresupuestalMLDetalle")]
        public async Task<IHttpActionResult> ObtenerInformacionPresupuestalMLDetalle(int TramiteProyectoId, string origen)
        {
            try
            {
                var result = await Task.Run(() => _modificacionLeyServicios.ObtenerInformacionPresupuestalMLDetalle(TramiteProyectoId, origen, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [HttpPost]
        [Route("api/ModificacionLeyAdicion/GuardarInformacionPresupuestalML")]
        public async Task<IHttpActionResult> GuardarInformacionPresupuestalML(InformacionPresupuestalMLDto informacionPresupuestal)
        {
            try
            {
                var result = await Task.Run(() => _modificacionLeyServicios.GuardarInformacionPresupuestalML(informacionPresupuestal, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        #endregion
    }
}