using DNP.Backbone.Comunes.Excepciones;
using DNP.Backbone.Servicios.Interfaces.Autorizacion;
using DNP.Backbone.Servicios.Interfaces.TramitesDistribucion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace DNP.Backbone.Web.API.Controllers.TramitesDistribucion
{
    public class TramitesDistribucionController : Base.BackboneBase
    {
        private readonly ITramitesDistribucionServicios _tramitesDistribucionServicios;
        private readonly IAutorizacionServicios _autorizacionUtilidades;

        /// <summary>
        /// Constructor de clases
        /// </summary>
        /// <param name="tramitesDistribucionServicios">Instancia de servicios de priorizacion</param>
        /// <param name="autorizacionUtilidades">Instancia de servicios de autorizacion</param>
        public TramitesDistribucionController(ITramitesDistribucionServicios tramitesDistribucionServicios,
            IAutorizacionServicios autorizacionUtilidades)
            : base(autorizacionUtilidades)
        {
            _tramitesDistribucionServicios = tramitesDistribucionServicios;
            _autorizacionUtilidades = autorizacionUtilidades;
        }

        [Route("api/TramitesDistribucion/ConsultarTramitesDistribucionAnteriores")]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerTramitesDistribucionAnteriores(Guid? instanciaId)
        {
            try
            {
                var result = await Task.Run(() => _tramitesDistribucionServicios.ObtenerTramitesDistribucionAnteriores(instanciaId, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }
    }
}