namespace DNP.Backbone.Web.API.Controllers.DatosAdicionales
{
    using DNP.Backbone.Comunes.Excepciones;
    using DNP.Backbone.Servicios.Interfaces.Autorizacion;
    using DNP.Backbone.Servicios.Interfaces.DatosAdicionales;
    using DNP.ServiciosNegocio.Dominio.Dto.DatosAdicionales;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web.Http;

    public class DatosAdicionalesController : Base.BackboneBase
    {
        private readonly IDatosAdicionalesServicios _DatosAdicionalesServicios;
        private readonly IAutorizacionServicios _autorizacionUtilidades;

        public DatosAdicionalesController(IDatosAdicionalesServicios DatosAdicionalesServicios, IAutorizacionServicios autorizacionUtilidades)
            : base(autorizacionUtilidades)
        {
            _DatosAdicionalesServicios = DatosAdicionalesServicios;
            _autorizacionUtilidades = autorizacionUtilidades;
        }

        [Route("api/DatosAdicionalesObtener")]
        [HttpGet]
        public async Task<IHttpActionResult> DatosAdicionalesObtener(int fuenteId, string usuarioDNP, string tokenAutorizacion)
        {
            try
            {
                var result = await Task.Run(() => _DatosAdicionalesServicios.ObtenerDatosAdicionales(fuenteId, usuarioDNP, tokenAutorizacion));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }


        [Route("api/DatosAdicionalesAgregar")]
        [HttpPost]
        public async Task<IHttpActionResult> DatosAdicionalesAgregar(DatosAdicionalesDto objDatosAdicionalesDto, string usuarioDNP, string tokenAutorizacion)
        {
            try
            {
                var result = await Task.Run(() => _DatosAdicionalesServicios.AgregarDatosAdicionales(objDatosAdicionalesDto, usuarioDNP, tokenAutorizacion));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/DatosAdicionalesEliminar")]
        [HttpPost]
        public async Task<IHttpActionResult> DatosAdicionalesEliminar(int cofinanciadorId, string usuarioDNP, string tokenAutorizacion)
        {
            try
            {
                var result = await Task.Run(() => _DatosAdicionalesServicios.EliminarDatosAdicionales(cofinanciadorId, usuarioDNP, tokenAutorizacion));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

    }
}