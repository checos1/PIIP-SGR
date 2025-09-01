namespace DNP.Backbone.Web.API.Controllers.FuenteFinanciacion
{
    using DNP.Backbone.Comunes.Excepciones;
    using DNP.Backbone.Dominio.Dto.Focalizacion;
    using DNP.Backbone.Servicios.Interfaces.Autorizacion;
    using DNP.Backbone.Servicios.Interfaces.FuenteFinanciacion;
    using DNP.ServiciosNegocio.Dominio.Dto.FuenteFinanciacion;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web.Http;

    public class FuentesAprobacionController : Base.BackboneBase
    {
        private readonly IFuentesAprobacionServicio _fuentesAprobacionServicio;
        private readonly IAutorizacionServicios _autorizacionUtilidades;

        public FuentesAprobacionController(IFuentesAprobacionServicio fuentesAprobacionServicio, IAutorizacionServicios autorizacionUtilidades): base(autorizacionUtilidades)
        {
            _fuentesAprobacionServicio = fuentesAprobacionServicio;
            _autorizacionUtilidades = autorizacionUtilidades;
        }

        [Route("api/FuenteGuardarPreguntasAprobacionRol")]
        [HttpPost]
        public async Task<IHttpActionResult> GuardarPreguntasAprobacionRol(PreguntasSeguimientoProyectoDto objPreguntasSeguimientoProyectoDto,  string usuarioDNP, string tokenAutorizacion)
        {
            try
            {
                var result = await Task.Run(() => _fuentesAprobacionServicio.GuardarPreguntasAprobacionRol(objPreguntasSeguimientoProyectoDto, usuarioDNP, tokenAutorizacion));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/FuenteObtenerPreguntasAprobacionRol")]
        [HttpGet]
        public async Task<IHttpActionResult> FuenteObtenerPreguntasAprobacionRol(string tramiteId, string proyectoId, string tipoTramiteId, string nivelId, string idInstancia, string usuarioDNP, string tokenAutorizacion)
        {
            try
            {
                PreguntasSeguimientoProyectoDto objPreguntasSeguimientoProyectoDto = new PreguntasSeguimientoProyectoDto();
                objPreguntasSeguimientoProyectoDto.tramiteId = int.Parse(tramiteId);
                objPreguntasSeguimientoProyectoDto.proyectoId = int.Parse(proyectoId);
                objPreguntasSeguimientoProyectoDto.tipoTramiteId = int.Parse(tipoTramiteId);
                objPreguntasSeguimientoProyectoDto.nivelId = System.Guid.Parse(nivelId);
                objPreguntasSeguimientoProyectoDto.instanciaId = System.Guid.Parse(idInstancia);

                var result = await Task.Run(() => _fuentesAprobacionServicio.ObtenerPreguntasAprobacionRol(objPreguntasSeguimientoProyectoDto, usuarioDNP, tokenAutorizacion));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/FuenteGuardarPreguntasAprobacionJefe")]
        [HttpPost]
        public async Task<IHttpActionResult> GuardarPreguntasAprobacionJefe(PreguntasSeguimientoProyectoDto objPreguntasSeguimientoProyectoDto, string usuarioDNP, string tokenAutorizacion)
        {
            try
            {
                var result = await Task.Run(() => _fuentesAprobacionServicio.GuardarPreguntasAprobacionJefe(objPreguntasSeguimientoProyectoDto, usuarioDNP, tokenAutorizacion));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/FuenteObtenerPreguntasAprobacionJefe")]
        [HttpGet]
        public async Task<IHttpActionResult> FuenteObtenerPreguntasAprobacionJefe(string tramiteId, string proyectoId, string tipoTramiteId, string nivelId, string usuarioDNP, string tokenAutorizacion)
        {
            try
            {

                PreguntasSeguimientoProyectoDto objPreguntasSeguimientoProyectoDto = new PreguntasSeguimientoProyectoDto();
                objPreguntasSeguimientoProyectoDto.tramiteId = int.Parse(tramiteId);
                objPreguntasSeguimientoProyectoDto.proyectoId = int.Parse(proyectoId);
                objPreguntasSeguimientoProyectoDto.tipoTramiteId = int.Parse(tipoTramiteId);
                objPreguntasSeguimientoProyectoDto.nivelId = System.Guid.Parse(nivelId);

                var result = await Task.Run(() => _fuentesAprobacionServicio.ObtenerPreguntasAprobacionJefe(objPreguntasSeguimientoProyectoDto, usuarioDNP, tokenAutorizacion));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

    }
}