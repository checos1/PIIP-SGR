
namespace DNP.Backbone.Web.API.Controllers.FuenteFinanciacion
{

    using DNP.Backbone.Servicios.Interfaces.Autorizacion;
    using DNP.Backbone.Servicios.Interfaces.FuenteFinanciacion;
    using DNP.Backbone.Comunes.Excepciones;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web.Http;
    using DNP.Backbone.Dominio.Dto.FuenteFinanciacion;

    public class ProgramarSolicitadoController : Base.BackboneBase
    {
        private readonly IProgramarSolicitadoServicio _programarSolicitadoServicio;
        private readonly IAutorizacionServicios _autorizacionUtilidades;



        public ProgramarSolicitadoController(IProgramarSolicitadoServicio programarSolicitadoServicio, IAutorizacionServicios autorizacionUtilidades)
            : base(autorizacionUtilidades)
        {
            _programarSolicitadoServicio = programarSolicitadoServicio;
            _autorizacionUtilidades = autorizacionUtilidades;
        }

        [Route("api/ConsultarFuentesProgramarSolicitado")]
        [HttpGet]
        public async Task<IHttpActionResult> ConsultarFuentesProgramarSolicitado(string bpin, string usuarioDNP, string tokenAutorizacion)
        {
            try
            {
                var result = await Task.Run(() => _programarSolicitadoServicio.ConsultarFuentesProgramarSolicitado(bpin, usuarioDNP, tokenAutorizacion));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/GuardarFuentesProgramarSolicitado")]
        [HttpPost]
        public async Task<IHttpActionResult> GuardarFuentesProgramarSolicitado(ProgramacionValorFuenteDto objProgramacionValorFuenteDto, string usuarioDNP)
        {
            try
            {
                var result = await Task.Run(() => _programarSolicitadoServicio.GuardarFuentesProgramarSolicitado(objProgramacionValorFuenteDto, usuarioDNP));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/guardarFuentesFinanciacionRecursosAjustes")]
        [HttpPost]
        public async Task<IHttpActionResult> guardarFuentesFinanciacionRecursosAjustes(FuenteFinanciacionAgregarAjusteDto objFuenteFinanciacionAgregarAjusteDto, string usuarioDNP)
        {
            try
            {
                var result = await Task.Run(() => _programarSolicitadoServicio.guardarFuentesFinanciacionRecursosAjustes(objFuenteFinanciacionAgregarAjusteDto, usuarioDNP));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }
    }
}