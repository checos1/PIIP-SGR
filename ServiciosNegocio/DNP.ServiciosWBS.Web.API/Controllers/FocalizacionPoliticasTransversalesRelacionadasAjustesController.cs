using DNP.ServiciosNegocio.Comunes;
using DNP.ServiciosNegocio.Comunes.Autorizacion;
using DNP.ServiciosNegocio.Comunes.Dto;
using DNP.ServiciosNegocio.Comunes.Dto.Formulario;
using DNP.ServiciosNegocio.Comunes.Excepciones;
using DNP.ServiciosNegocio.Comunes.Utilidades;
using DNP.ServiciosNegocio.Dominio.Dto.FocalizacionPoliticaTransversaleRelacionadaAjustes;
using DNP.ServiciosNegocio.Dominio.Dto.PoliticasIndicadoresCategorias;
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
using System.Web.Mvc;

namespace DNP.ServiciosWBS.Web.API.Controllers
{
    public class FocalizacionPoliticasTransversalesRelacionadasAjustesController : ApiController
    {
        private readonly IAutorizacionUtilidades _autorizacionUtilidades;
        private readonly IFocalizacionPoliticasTransversalesRelacionadasAjustesServicios _focalizacionPoliticasTransversalesRelacionadasAjustesServicios;

        public FocalizacionPoliticasTransversalesRelacionadasAjustesController(IAutorizacionUtilidades autorizacionUtilidades, IFocalizacionPoliticasTransversalesRelacionadasAjustesServicios focalizacionPoliticasTransversalesRelacionadasAjustesServicios)
        {
            _autorizacionUtilidades = autorizacionUtilidades;
            this._focalizacionPoliticasTransversalesRelacionadasAjustesServicios = focalizacionPoliticasTransversalesRelacionadasAjustesServicios;
        }

        //[System.Web.Http.Route("api/DNP_SN_Pla_PriyAsiRec_PoliticaTRelacionadasAjustesDto")]
        [System.Web.Http.Route("api/DNP_SN_Eje_Ajustes_PoliticaTRelacionadasDto")]
        [SwaggerResponse(HttpStatusCode.OK, "Informacion del proyecto Focalización Políticas Transversales – Relación con otras Políticas Ajustes", typeof(PoliticaTransversalRelacionadaAjustesDto))]
        [System.Web.Http.HttpGet]
        public async Task<IHttpActionResult> Consultar(string bpin)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.
                                            ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                           RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                           ConfigurationManager.AppSettings
                                                               ["idAdministracionEnAutorizaciones"],
                                                           ConfigurationManager.
                                                               AppSettings["consultarFPTRelacionAjustes"]).
                                            Result;

                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                ValidacionParametro(bpin, Request);

                var result =
                    await Task.Run(() => _focalizacionPoliticasTransversalesRelacionadasAjustesServicios.ObtenerFocalizacionPoliticasTransversalesRelacionadasAjustesServicios(new ParametrosConsultaDto
                    {
                        Bpin
                                                                                                  =
                                                                                                  bpin,
                        AccionId
                                                                                                  =
                                                                                                  new
                                                                                                      Guid(Request.
                                                                                                           Headers.
                                                                                                           GetValues("piip-idAccion").
                                                                                                           First()),
                        InstanciaId
                                                                                                  = new
                                                                                                      Guid(Request.
                                                                                                           Headers.
                                                                                                           GetValues("piip-idInstanciaFlujo").
                                                                                                           First())
                    }));


                if (result?.Vigencias_Politicas_Cuantifica_Beneficiarios != null) return Ok<PoliticaTransversalRelacionadaAjustesDto>(result);

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

            if (peticion.Headers.Contains("piip-idFormulario"))
            {
                if (string.IsNullOrEmpty(peticion.Headers.GetValues("piip-idFormulario").First()))
                    throw new ServiciosNegocioException(string.Format(ServiciosNegocioRecursos.ParametroNoRecibido, "piip-idFormulario"));

                Guid outGuidAccionId = Guid.Empty;
                // ReSharper disable once UnusedVariable
                if (!Guid.TryParse(peticion.Headers.GetValues("piip-idFormulario").First(), out outGuidAccionId))
                    throw new ServiciosNegocioException(string.Format(ServiciosNegocioRecursos.ParametroNoValidos, "piip-idFormulario"));
            }
            else
            {
                throw new ServiciosNegocioException(string.Format(ServiciosNegocioRecursos.ParametroNoRecibido,
                                                             "piip-idFormulario"));
            }
        }

        //[System.Web.Http.Route("api/DNP_SN_Pla_PriyAsiRec_PoliticaTRelacionadasAjustesDto/Preview")]
        [System.Web.Http.Route("api/DNP_SN_Eje_Ajustes_PoliticaTRelacionadasDto/Preview")]
        [SwaggerResponse(HttpStatusCode.OK, "Información del proyecto detallando su focalización con información dummy", typeof(PoliticaTransversalRelacionadaAjustesDto))]
        [System.Web.Http.HttpGet]
        public async Task<IHttpActionResult> Preview()
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.
                                            ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                           RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                           ConfigurationManager.AppSettings
                                                               ["idAdministracionEnAutorizaciones"],
                                                           ConfigurationManager.AppSettings["previewFPTRelacionAjustes"]).
                                            Result;

                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                var result = await Task.Run(() => _focalizacionPoliticasTransversalesRelacionadasAjustesServicios.ObtenerFocalizacionPoliticasTransversalesRelacionadasAjustesServiciosPreview());

                return Ok<PoliticaTransversalRelacionadaAjustesDto>(result);
            }
            catch (AutorizacionException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, e));
            }
        }

        //[System.Web.Http.Route("api/DNP_SN_Pla_PriyAsiRec_PoliticaTRelacionadasAjustesDto/Temporal")]
        [System.Web.Http.Route("api/DNP_SN_Eje_Ajustes_PoliticaTRelacionadasDto/Temporal")]
        [SwaggerResponse(HttpStatusCode.OK, "Informacion devuelta por guardado temporal", typeof(HttpResponseMessage))]
        [System.Web.Http.HttpPost]
        public async Task<IHttpActionResult> Temporal(PoliticaTransversalRelacionadaAjustesDto contenido)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.
                                            ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                           RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                           ConfigurationManager.AppSettings
                                                               ["idAdministracionEnAutorizaciones"],
                                                           ConfigurationManager.AppSettings
                                                               ["postTempFPTRelacionAjustes"]).
                                            Result;

                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                var parametrosGuardar = _focalizacionPoliticasTransversalesRelacionadasAjustesServicios.ConstruirParametrosGuardado(Request, contenido);

                var parametrosAuditoria = new ParametrosAuditoriaDto
                {
                    Usuario = RequestContext.
                                                        Principal.Identity.Name,
                    Ip = UtilidadesApi.GetClientIp(Request)
                };

                await Task.Run((Action)(() => _focalizacionPoliticasTransversalesRelacionadasAjustesServicios.Guardar(parametrosGuardar, parametrosAuditoria, true)));

                var respuesta = new HttpResponseMessage();
                respuesta.StatusCode = HttpStatusCode.OK;
                respuesta.ReasonPhrase = ServiciosNegocioRecursos.PostExitoso;

                return Ok(respuesta);
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

        //[System.Web.Http.Route("api/DNP_SN_Pla_PriyAsiRec_PoliticaTRelacionadasAjustesDto")]
        [System.Web.Http.Route("api/DNP_SN_Eje_Ajustes_PoliticaTRelacionadasDto")]
        [SwaggerResponse(HttpStatusCode.OK, "Informacion devuelta por guardado definitivo", typeof(HttpResponseMessage))]
        [System.Web.Http.HttpPost]
        public async Task<IHttpActionResult> Definitivo([FromBody] PoliticaTransversalRelacionadaAjustesDto proyecto)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.
                                            ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                           RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                           ConfigurationManager.AppSettings
                                                               ["idAdministracionEnAutorizaciones"],
                                                           ConfigurationManager.AppSettings
                                                               ["postDefFPTRelacionAjustes"]).
                                            Result;

                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);


                var parametrosGuardar = _focalizacionPoliticasTransversalesRelacionadasAjustesServicios.ConstruirParametrosGuardado(Request, proyecto);

                var parametrosAuditoria = new ParametrosAuditoriaDto
                {
                    Usuario = RequestContext.
                                                        Principal.Identity.Name,
                    Ip = UtilidadesApi.GetClientIp(Request)
                };

                await Task.Run((Action)(() => _focalizacionPoliticasTransversalesRelacionadasAjustesServicios.Guardar(parametrosGuardar, parametrosAuditoria, false)));

                var respuesta = new HttpResponseMessage();
                respuesta.StatusCode = HttpStatusCode.OK;
                respuesta.ReasonPhrase = ServiciosNegocioRecursos.PostExitoso;

                return Ok(respuesta);
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