using DNP.ServiciosNegocio.Comunes;
using DNP.ServiciosNegocio.Comunes.Autorizacion;
using DNP.ServiciosNegocio.Servicios.Interfaces.ModificacionLey;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Configuration;
using System.Net.Http;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using DNP.ServiciosNegocio.Dominio.Dto.ModificacionLey;

namespace DNP.ServiciosNegocio.Web.API.Controllers.ModificacionLey
{
    public class ModificacionLeyController : ApiController
    {
        private readonly IModificacionLeyServicio _modificacionLeyServicio;
        private readonly IAutorizacionUtilidades _autorizacionUtilidades;

        public ModificacionLeyController(IModificacionLeyServicio modificacionLeyServicio, IAutorizacionUtilidades autorizacionUtilidades)
        {
            _modificacionLeyServicio = modificacionLeyServicio;
            _autorizacionUtilidades = autorizacionUtilidades;
        }

        [Route("api/ModificacionLey/ObtenerInformacionPresupuestalMLEncabezado")]
        [SwaggerResponse(HttpStatusCode.OK, "Retorna listado de Datos Encabezado de la Informacion Presupuestal", typeof(string))]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerInformacionPresupuestalMLEncabezado(int EntidadDestinoId, int TramiteId, string origen)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                                                    RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                                                    ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                                                    ConfigurationManager.AppSettings["ObtenerInformacionPresupuestalMLEncabezado"]).Result;
                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);
                var result = await Task.Run(() => _modificacionLeyServicio.ObtenerInformacionPresupuestalMLEncabezado(EntidadDestinoId, TramiteId, origen));
                if (result != null) return Ok(result);

                var respuestaHttp = new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.NotFound,
                    ReasonPhrase = ServiciosNegocioRecursos.SinResultados
                };

                return ResponseMessage(respuestaHttp);
            }
            catch (Exception e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/ModificacionLey/ObtenerInformacionPresupuestalMLDetalle")]
        [SwaggerResponse(HttpStatusCode.OK, "Retorna listado de Datos detalle de la Informacion Presupuestal", typeof(string))]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerInformacionPresupuestalMLDetalle(int tramiteidProyectoId, string origen)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                                                    RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                                                    ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                                                    ConfigurationManager.AppSettings["ObtenerInformacionPresupuestalMLDetalle"]).Result;
                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                var result = await Task.Run(() => _modificacionLeyServicio.ObtenerInformacionPresupuestalMLDetalle(tramiteidProyectoId, origen));
                if (result != null) return Ok(result);

                var respuestaHttp = new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.NotFound,
                    ReasonPhrase = ServiciosNegocioRecursos.SinResultados
                };

                return ResponseMessage(respuestaHttp);
            }
            catch (Exception e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/ModificacionLey/GuardarInformacionPresupuestalML")]
        [SwaggerResponse(HttpStatusCode.OK, " Funcion para Registrar los Datos de la Informacion Presupuestal", typeof(InformacionPresupuestalMLDto))]
        [HttpPost]
        public async Task<IHttpActionResult> GuardarInformacionPresupuestalML(InformacionPresupuestalMLDto objInformacionPresupuestalDto)
        {
            var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                                                    RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                                                    ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                                                    ConfigurationManager.AppSettings["GuardarInformacionPresupuestalML"]).Result;
            if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);
            string usuario = RequestContext.Principal.Identity.Name;

            var result = await Task.Run(() => _modificacionLeyServicio.GuardarInformacionPresupuestalML(objInformacionPresupuestalDto, usuario, objInformacionPresupuestalDto.Origen));
            if (result != null) return Ok(result);

            var respuestaHttp = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.NotFound,
                ReasonPhrase = ServiciosNegocioRecursos.SinResultados
            };

            return ResponseMessage(respuestaHttp);
        }
    }
}