
namespace DNP.ServiciosEnrutamiento.Web.API.Controllers
{
    using DNP.ServiciosEnrutamiento.Servicios.Interfaces;
    using DNP.ServiciosNegocio.Dominio.Dto.Transferencias;
    using ServiciosNegocio.Comunes;
    using ServiciosNegocio.Comunes.Autorizacion;
    using ServiciosNegocio.Comunes.Dto;
    using ServiciosNegocio.Comunes.Excepciones;
    using ServiciosNegocio.Comunes.Utilidades;
    using System;
    using System.Configuration;
    using System.Diagnostics.CodeAnalysis;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web.Http;

    public class TallerController: ApiController
    {
        private readonly IAutorizacionUtilidades _autorizacionUtilidades;
        private readonly ITallerServicio _tallerServicio;

        public TallerController(ITallerServicio tallerServicio, IAutorizacionUtilidades autorizacionUtilidades)
        {
            _tallerServicio = tallerServicio;
            _autorizacionUtilidades = autorizacionUtilidades;
        }

        [Route("api/Taller")]
        [HttpPost]
        public async Task<IHttpActionResult> EjecutarRegla(ObjetoNegocio objetoNegocio)
        {
            try
            {
                var respuestaAutorizacion = await _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                           RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                           ConfigurationManager.AppSettings["idEnrutamientoEnAutorizaciones"],
                                                           ConfigurationManager.AppSettings["ejecutarReglaTaller"]);

                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                var parametrosAuditoria = new ParametrosAuditoriaDto
                {
                    Usuario = RequestContext.Principal.Identity.Name,
                    Ip = UtilidadesApi.GetClientIp(Request)
                };

                var response = await Task.Run(() => _tallerServicio.EjecutarReglaTaller(objetoNegocio, parametrosAuditoria));

                return Ok(response);
            }
            catch (ServiciosNegocioException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
            catch (AutorizacionException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, e));
            }
        }

        [Route("api/Taller")]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerReglas()
        {
            try
            {
                var respuestaAutorizacion = await _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                           RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                           ConfigurationManager.AppSettings["idEnrutamientoEnAutorizaciones"],
                                                           ConfigurationManager.AppSettings["obtenerReglaTaller"]);

                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                var parametrosAuditoria = new ParametrosAuditoriaDto
                {
                    Usuario = RequestContext.Principal.Identity.Name,
                    Ip = UtilidadesApi.GetClientIp(Request)
                };

                var result = await Task.Run(() => _tallerServicio.ObtenerReglasTaller());
                return Ok(result);
            }
            catch (ServiciosNegocioException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
            catch (AutorizacionException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, e));
            }
        }
    }
}