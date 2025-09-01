using DNP.ServiciosNegocio.Comunes;
using DNP.ServiciosNegocio.Comunes.Autorizacion;
using DNP.ServiciosNegocio.Comunes.Dto;
using DNP.ServiciosNegocio.Comunes.Dto.Tramites;
using DNP.ServiciosNegocio.Comunes.Excepciones;
using DNP.ServiciosNegocio.Dominio.Dto.Transferencias;
using DNP.ServiciosTransaccional.Servicios.Interfaces.Programacion;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace DNP.ServiciosTransaccional.Web.API.Controllers.Tramites
{
    public class ProgramacionController : ApiController
    {
        private readonly IAutorizacionUtilidades _autorizacionUtilidades;
        private readonly IProgramacionServicio _programacionServicios;

        public ProgramacionController(IProgramacionServicio programacionServicios, IAutorizacionUtilidades autorizacionUtilidades)
        {
            _autorizacionUtilidades = autorizacionUtilidades;
            _programacionServicios = programacionServicios;
        }

        [HttpPost]
        [Route("api/GuardarDatosProgramacionDistribucion")]
        public async Task<IHttpActionResult> GuardarDatosProgramacionDistribucion(ObjetoNegocio datosNegocio)
        {
            try
            {
                var respuestaAutorizacion = await _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                            RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                            ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                            ConfigurationManager.AppSettings["PostGenerarRadicadoEntrada"]);

                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

               
                var rta =  _programacionServicios.GuardarDatosProgramacionDistribucion(datosNegocio.ObjetoNegocioId, RequestContext.Principal.Identity.Name);

                if (rta.Exito)
                {
                    return Ok(rta);
                }
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, rta.Mensaje));
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

        [HttpPost]
        [Route("api/InclusionFuentesProgramacion")]
        public async Task<IHttpActionResult> InclusionFuentesProgramacion(ObjetoNegocio datosNegocio)
        {
            try
            {
                var respuestaAutorizacion = await _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                            RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                            ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                            ConfigurationManager.AppSettings["PostInclusionFuentesProgramacion"]);

                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);


                var rta = _programacionServicios.InclusionFuentesProgramacion(datosNegocio.ObjetoNegocioId, RequestContext.Principal.Identity.Name);

                if (rta.Exito)
                {
                    return Ok(rta);
                }
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, rta.Mensaje));
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