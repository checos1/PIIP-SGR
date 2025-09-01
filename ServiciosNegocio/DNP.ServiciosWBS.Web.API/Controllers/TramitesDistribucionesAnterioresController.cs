using DNP.ServiciosNegocio.Comunes;
using DNP.ServiciosNegocio.Comunes.Autorizacion;
using DNP.ServiciosNegocio.Comunes.Dto.Formulario;
using DNP.ServiciosNegocio.Comunes.Excepciones;
using DNP.ServiciosNegocio.Dominio.Dto.Tramites.TramitesDistribucionesAnteriores;
using DNP.ServiciosWBS.Servicios.Interfaces;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace DNP.ServiciosWBS.Web.API.Controllers
{
    public class TramitesDistribucionesAnterioresController : ApiController
    {
        private readonly ITramitesDistribucionesAnterioresServicios  _tramitesDistribucionesAnterioresServicios;
        private readonly IAutorizacionUtilidades _autorizacionUtilidades;

        public TramitesDistribucionesAnterioresController(ITramitesDistribucionesAnterioresServicios tramitesDistribucionesAnterioresServicios, IAutorizacionUtilidades autorizacionUtilidades)
        {
            _tramitesDistribucionesAnterioresServicios = tramitesDistribucionesAnterioresServicios;
            _autorizacionUtilidades = autorizacionUtilidades;
        }

        [Route("api/DNP_TramitesDistribucionesAnterioresDto")]
        [SwaggerResponse(HttpStatusCode.OK, "Informacion Tramites distribucion Anteriores", typeof(TramitesDistribucionesAnterioresDto))]
        [HttpGet]
        public async Task<IHttpActionResult> Consultar(Guid instancia)
        {
            try
            {
                //var respuestaAutorizacion = _autorizacionUtilidades.
                //                            ValidarUsuario(RequestContext.Principal.Identity.Name,
                //                                           RequestContext.Principal.Identity.GetHashCode().ToString(),
                //                                           ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                //                                           ConfigurationManager.AppSettings["consultarTramitesDistribucionesAnteriores"]).
                //                            Result;

                //if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                var result =
                    await Task.Run(() => _tramitesDistribucionesAnterioresServicios.ObtenerTramitesDistribucionAnterior(new ParametrosConsultaDto {  InstanciaId = instancia }));

                if (result != null) return Ok(result);

                var respuestaHttp = new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.NotFound,
                    ReasonPhrase = ServiciosNegocioRecursos.SinResultados
                };

                return ResponseMessage(respuestaHttp);

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

        [SuppressMessage("ReSharper", "UnusedVariable")]
        private void ValidacionParametro(int tramiteId, HttpRequestMessage peticion)
        {
            if (tramiteId == 0)
                throw new ServiciosNegocioException(string.Format(ServiciosNegocioRecursos.ParametroNoRecibido, nameof(tramiteId)));

            int outTramiteId;
            if (!int.TryParse(tramiteId.ToString(), out outTramiteId) || Convert.ToString(tramiteId).Length > 100)
                throw new ServiciosNegocioException(string.Format(ServiciosNegocioRecursos.ParametroNoValidos, nameof(tramiteId)));

            if (peticion.Headers.Contains("piip-idInstanciaFlujo"))
            {
                if (string.IsNullOrEmpty(peticion.Headers.GetValues("piip-idInstanciaFlujo").First()))
                    throw new ServiciosNegocioException(string.Format(ServiciosNegocioRecursos.ParametroNoRecibido, "piip-idInstanciaFlujo"));

                Guid outGuid = Guid.Empty;
                // ReSharper disable once UnusedVariable
                if (!Guid.TryParse(peticion.Headers.GetValues("piip-idInstanciaFlujo").First(), out outGuid))
                    throw new ServiciosNegocioException(string.Format(ServiciosNegocioRecursos.ParametroNoValidos, "piip-idInstanciaFlujo"));
            }
            else
            {
                throw new ServiciosNegocioException(string.Format(ServiciosNegocioRecursos.ParametroNoRecibido,
                                                          "piip-idInstanciaFlujo"));
            }


            if (peticion.Headers.Contains("piip-idAccion"))
            {
                if (string.IsNullOrEmpty(peticion.Headers.GetValues("piip-idAccion").First()))
                    throw new ServiciosNegocioException(string.Format(ServiciosNegocioRecursos.ParametroNoRecibido, "piip-idAccion"));

                Guid outGuidAccionId = Guid.Empty;
                // ReSharper disable once UnusedVariable
                if (!Guid.TryParse(peticion.Headers.GetValues("piip-idAccion").First(), out outGuidAccionId))
                    throw new ServiciosNegocioException(string.Format(ServiciosNegocioRecursos.ParametroNoValidos, "piip-idAccion"));
            }
            else
            {
                throw new ServiciosNegocioException(string.Format(ServiciosNegocioRecursos.ParametroNoRecibido,
                                                             "piip-idAccion"));
            }
        }

        [Route("api/DNP_TramitesDistribucionesAnterioresDto/Preview")]
        [SwaggerResponse(HttpStatusCode.OK, "Informacion Tramites distribucion Anteriores dummy", typeof(TramitesDistribucionesAnterioresDto))]
        [HttpGet]
        public async Task<IHttpActionResult> Preview()
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.
                                            ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                           RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                           ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                           ConfigurationManager.AppSettings["previewTramitesDistribucionAnteriores"]).
                                            Result;

                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                var result = await Task.Run(() => _tramitesDistribucionesAnterioresServicios.ObtenertramitesDistribucionAnterioresPreview());

                return Ok<TramitesDistribucionesAnterioresDto>(result);
            }
            catch (AutorizacionException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, e));
            }
        }

    }
}