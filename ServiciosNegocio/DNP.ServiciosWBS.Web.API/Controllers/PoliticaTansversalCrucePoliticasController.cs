using DNP.ServiciosNegocio.Comunes;
using DNP.ServiciosNegocio.Comunes.Autorizacion;
using DNP.ServiciosNegocio.Comunes.Dto;
using DNP.ServiciosNegocio.Comunes.Dto.Formulario;
using DNP.ServiciosNegocio.Comunes.Excepciones;
using DNP.ServiciosNegocio.Comunes.Utilidades;
using DNP.ServiciosNegocio.Dominio.Dto.Genericos;
using DNP.ServiciosNegocio.Dominio.Dto.PoliticasTransversalesCrucePoliticas;
using DNP.ServiciosWBS.Persistencia.Interfaces;
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
    public class PoliticaTansversalCrucePoliticasController : ApiController
    {
        private readonly IPoliticasTransversalesCrucePoliticasServicios _politicasTransversalesCrucePoliticasServicios;
        private readonly IAutorizacionUtilidades _autorizacionUtilidades;

        public PoliticaTansversalCrucePoliticasController(IPoliticasTransversalesCrucePoliticasServicios politicasTransversalesCrucePoliticasServicios, IAutorizacionUtilidades autorizacionUtilidades)
        {
            _politicasTransversalesCrucePoliticasServicios = politicasTransversalesCrucePoliticasServicios;
            _autorizacionUtilidades = autorizacionUtilidades;
        }

        [Route("api/DNPPoliticasTCrucePoliticasDto")]
        [SwaggerResponse(HttpStatusCode.OK, "Informacion Politicas Cruce Politicas", typeof(PoliticasTCrucePoliticasDto))]
        [HttpGet]
        public async Task<IHttpActionResult> Consultar(string Bpin, int IdFuente)
        {
            try
            {
                //var respuestaAutorizacion = _autorizacionUtilidades.
                //                            ValidarUsuario(RequestContext.Principal.Identity.Name,
                //                                           RequestContext.Principal.Identity.GetHashCode().ToString(),
                //                                           ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                //                                           ConfigurationManager.AppSettings["consultarPoliticasCrucePoliticas"]).
                //                            Result;

               // if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                var result =
                    await Task.Run(() => _politicasTransversalesCrucePoliticasServicios.ObtenerPoliticasTransversalesCrucePoliticas(new ParametrosConsultaDto { Bpin = Bpin, IdFuente = IdFuente }));

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
        private void ValidacionParametro(string bpin, HttpRequestMessage peticion)
        {
            if (string.IsNullOrEmpty(bpin))
                throw new ServiciosNegocioException(string.Format(ServiciosNegocioRecursos.ParametroNoRecibido, nameof(bpin)));

            long outBpin;
            if (!long.TryParse(bpin, out outBpin) || bpin.Length > 100)
                throw new ServiciosNegocioException(string.Format(ServiciosNegocioRecursos.ParametroNoValidos, nameof(bpin)));

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

        [Route("api/DNP_SN_GR_Focalizacion_PoliticasTCrucePoliticasDto/Preview")]
        [SwaggerResponse(HttpStatusCode.OK, "Informacion de Datos Politicas cruce Politicas dummy", typeof(PoliticasTCrucePoliticasDto))]
        [HttpGet]
        public async Task<IHttpActionResult> Preview()
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.
                                            ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                           RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                           ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                           ConfigurationManager.AppSettings["previewPoliticasCrucePoliticas"]).
                                            Result;

                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                var result = await Task.Run(() => _politicasTransversalesCrucePoliticasServicios.ObtenerPoliticasTransversalesCrucePoliticasPreview());

                return Ok<PoliticasTCrucePoliticasDto>(result);
            }
            catch (AutorizacionException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, e));
            }
        }

        [Route("api/DNP_GR_Focalizacion_PoliticasTCrucePoliticasDto")]
        [SwaggerResponse(HttpStatusCode.OK, "Informacion guardado definitivo", typeof(HttpResponseMessage))]
        [HttpPost]
        public async Task<IHttpActionResult> Definitivo([FromBody] PoliticasTCrucePoliticasDto datosPoliticaCrucePolitica)
        {
            try
            {
                //var respuestaAutorizacion = _autorizacionUtilidades.
                //                            ValidarUsuario(RequestContext.Principal.Identity.Name,
                //                                           RequestContext.Principal.Identity.GetHashCode().ToString(),
                //                                           ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                //                                           ConfigurationManager.AppSettings["postPoliticaCrucePoliticaDefinitivo"]).
                //                            Result;

                //if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                var parametrosGuardar = new ParametrosGuardarDto<PoliticasTCrucePoliticasDto>();

                parametrosGuardar.Contenido = datosPoliticaCrucePolitica;

                var parametrosAuditoria = new ParametrosAuditoriaDto
                {
                    Usuario = RequestContext.
                              Principal.Identity.Name,
                    Ip = UtilidadesApi.GetClientIp(Request)
                };

                await Task.Run((Action)(() => _politicasTransversalesCrucePoliticasServicios.Guardar(parametrosGuardar, parametrosAuditoria, false)));

                //var respuesta = new HttpResponseMessage();
                ////respuesta.StatusCode = HttpStatusCode.OK;
                //respuesta.ReasonPhrase = ServiciosNegocioRecursos.PostExitoso;
                return Ok(new RespuestaGeneralDto { Exito = true });

                //return Ok(respuesta);
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