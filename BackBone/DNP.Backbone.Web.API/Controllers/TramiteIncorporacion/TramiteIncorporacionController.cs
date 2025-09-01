namespace DNP.Backbone.Web.API.Controllers.TramiteIncorporacion
{
    using DNP.Backbone.Comunes.Excepciones;
    using DNP.Backbone.Dominio.Dto;
    using DNP.Backbone.Servicios.Interfaces.Autorizacion;
    using DNP.Backbone.Servicios.Interfaces.TramiteIncorporacion;
    using DNP.ServiciosNegocio.Dominio.Dto.TramiteIncorporacion;
    using System;
    using System.Configuration;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web.Http;
    using System.Web.Http.Filters;

    public class TramiteIncorporacionController : Base.BackboneBase
    {
        private readonly ITramiteIncorporacionServicios _TramiteIncorporacionServicios;
        private readonly IAutorizacionServicios _autorizacionUtilidades;

        //protected UsuarioLogadoDto UsuarioLogadoDto
        //{
        //    get
        //    {
        //        return new UsuarioLogadoDto()
        //        {
        //            IdUsuario = User.Identity.Name,
        //            IdAplicacionBackbone = ConfigurationManager.AppSettings["IdAplicacionBackbone"],
        //            IdNombreBackbone = ConfigurationManager.AppSettings["IdNombreBackbone"],
        //            GuidPIIPAplicacion = Guid.Parse(ConfigurationManager.AppSettings["GuidPIIPAplicacion"]),
        //            GuidAdministracionAplicacion = Guid.Parse(ConfigurationManager.AppSettings["GuidAdministracionAplicacion"]),
        //            ApiAutorizacion = ConfigurationManager.AppSettings["ApiAutorizacion"],
        //        };
        //    }
        //}

        public TramiteIncorporacionController(ITramiteIncorporacionServicios TramiteIncorporacionServicios, IAutorizacionServicios autorizacionUtilidades)
            : base(autorizacionUtilidades)
        {
            _TramiteIncorporacionServicios = TramiteIncorporacionServicios;
            _autorizacionUtilidades = autorizacionUtilidades;

        }

        [Route("api/TramiteIncorporacion/ObtenerDatosIncorporacion")]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerDatosIncorporacion(int tramiteId)
        {
            try
            {
                var result = await Task.Run(() => _TramiteIncorporacionServicios.ObtenerDatosIncorporacion(tramiteId, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/TramiteIncorporacion/GuardarDatosIncorporacion")]
        [HttpPost]
        public async Task<IHttpActionResult> GuardarDatosIncorporacion(ConvenioDonanteDto objConvenioDonanteDto, string usuario)
        {
            try
            {
                var result = await Task.Run(() => _TramiteIncorporacionServicios.GuardarDatosIncorporacion(objConvenioDonanteDto, usuario));
                return Ok(result);
            }
            catch (Comunes.Excepciones.BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/TramiteIncorporacion/EiliminarDatosIncorporacion")]
        [HttpPost]
        public async Task<IHttpActionResult> EiliminarDatosIncorporacion(ConvenioDonanteDto objConvenioDonanteDto, string usuario)
        {
            try
            {
                var result = await Task.Run(() => _TramiteIncorporacionServicios.EiliminarDatosIncorporacion(objConvenioDonanteDto, usuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

    }
}