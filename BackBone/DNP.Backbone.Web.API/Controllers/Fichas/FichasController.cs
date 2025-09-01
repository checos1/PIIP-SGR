using DNP.Backbone.Comunes.Excepciones;
using DNP.Backbone.Dominio.Dto.Fichas;
using DNP.Backbone.Dominio.Dto.Transversal;
using DNP.Backbone.Servicios.Implementaciones.Fichas;
using DNP.Backbone.Servicios.Interfaces.Autorizacion;
using DNP.Backbone.Servicios.Interfaces.Fichas;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace DNP.Backbone.Web.API.Controllers.Fichas
{
    public class FichasController : Base.BackboneBase
    {
        private readonly IFichasServicios _fichasServicios;
        private readonly IAutorizacionServicios _autorizacionUtilidades;

        /// <summary>
        /// Constructor de clases
        /// </summary>
        /// <param name="fichasServicios">Instancia de servicios de fichas servicios</param>
        /// <param name="autorizacionUtilidades">Instancia de servicios de autorizacion</param>
        public FichasController(FichasServicios fichasServicios,
            IAutorizacionServicios autorizacionUtilidades)
            : base(autorizacionUtilidades)
        {
            _fichasServicios = fichasServicios;
            _autorizacionUtilidades = autorizacionUtilidades;
        }

        [Route("api/FichaFisico")]
        [HttpPost]
        public async Task<IHttpActionResult> GenerarFicha(RecibirParametrosDto parametros)
        {
            try
            {
                var result = await Task.Run(() => _fichasServicios.GenerarFicha(parametros, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/Reporte/ObtenerPlantilla/{nombre}")]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerIdFicha(string nombre)
        {
            try
            {
                var result = await Task.Run(() => _fichasServicios.ObtenerIdFicha(nombre, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/GenerarFichaManualSubFlujoSGR")]
        [HttpPost]
        public async Task<IHttpActionResult> GenerarFichaManualSubFlujoSGR(ObjetoNegocio objObjetoNegocio)
        {
            try
            {
                var result = await Task.Run(() => _fichasServicios.GenerarFichaManualSubFlujoSGR(objObjetoNegocio, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }
    }
}