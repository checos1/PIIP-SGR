using DNP.Backbone.Comunes.Excepciones;
using DNP.Backbone.Dominio.Dto.PoliticasTransversalesCrucePoliticas;
using DNP.Backbone.Servicios.Interfaces.Autorizacion;
using DNP.Backbone.Servicios.Interfaces.Focalizacion;
using DNP.Backbone.Servicios.Interfaces.ServiciosNegocio;
using DNP.Backbone.Web.API.Controllers.Base;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace DNP.Backbone.Web.API.Controllers.Focalizacion
{
    public class PoliticasCrucePoliticasController : BackboneBase
    {
        private readonly IPoliticasTransversalesCrucePoliticasServicios _politicasTransversalesCrucePoliticasServicios;
        private readonly IAutorizacionServicios _autorizacionUtilidades;
        private readonly IServiciosNegocioServicios _serviciosNegocioServicios;

        public PoliticasCrucePoliticasController(IPoliticasTransversalesCrucePoliticasServicios politicasTransversalesCrucePoliticasServicios, IAutorizacionServicios autorizacionUtilidades)
           : base(autorizacionUtilidades)
        {
            _politicasTransversalesCrucePoliticasServicios = politicasTransversalesCrucePoliticasServicios;
            _autorizacionUtilidades = autorizacionUtilidades;
        }

        [Route("api/Focalizacion/ObtenerPoliticasTransversalesCrucePoliticas")]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerPoliticasTransversalesCrucePoliticas(string Bpin, int IdFuente)
        {
            try
            {
                var result = await Task.Run(() => _politicasTransversalesCrucePoliticasServicios.ObtenerPoliticasTransversalesCrucePoliticas(Bpin, IdFuente, UsuarioLogadoDto.IdUsuario, Request.Headers.Authorization.Parameter));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/Focalizacion/ActualizarPoliticasTransversalesCrucePoliticas")]
        [HttpPost]
        public async Task<IHttpActionResult> ActualizarPoliticasTransversalesCrucePoliticas(PoliticasTCrucePoliticasDto parametros)
        {
            try
            {
                var result = await Task.Run(() => _politicasTransversalesCrucePoliticasServicios.ActualizarPoliticasTransversalesCrucePoliticas(parametros, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }
    }
}