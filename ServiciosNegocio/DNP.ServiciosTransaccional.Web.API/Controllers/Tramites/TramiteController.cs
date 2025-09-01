using DNP.ServiciosNegocio.Comunes;
using DNP.ServiciosNegocio.Comunes.Autorizacion;
using DNP.ServiciosNegocio.Comunes.Dto;
using DNP.ServiciosNegocio.Comunes.Dto.Tramites;
using DNP.ServiciosNegocio.Comunes.Excepciones;
using DNP.ServiciosNegocio.Dominio.Dto.Transferencias;
using DNP.ServiciosTransaccional.Servicios.Interfaces.Tramites;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace DNP.ServiciosTransaccional.Web.API.Controllers.Tramites
{
    public class TramiteController : ApiController
    {
        private readonly IAutorizacionUtilidades _autorizacionUtilidades;
        private readonly ITramiteServicio _tramiteServicios;

        public TramiteController(ITramiteServicio tramiteServicios, IAutorizacionUtilidades autorizacionUtilidades)
        {
            _autorizacionUtilidades = autorizacionUtilidades;
            _tramiteServicios = tramiteServicios;
        }

        [Route("api/Tramites/ReasignarRadicadoORFEO")]
        [SwaggerResponse(HttpStatusCode.OK, "Informacion devuelta por guardado", typeof(HttpResponseMessage))]
        [HttpPost]
        public async Task<IHttpActionResult> ReasignarRadicadoORFEO(ReasignacionRadicadoDto reasignacionRadicado)
        {
            try
            {
                var respuestaAutorizacion = await _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                            RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                            ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                            ConfigurationManager.AppSettings["idReasignarRadicadoORFEO"]);

                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                var result = await Task.Run(() => _tramiteServicios.ReasignarRadicadoORFEO(reasignacionRadicado, RequestContext.Principal.Identity.Name));

                var respuesta = new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    ReasonPhrase = $"{ServiciosNegocioRecursos.PostExitoso} mensaje interno: '{result.Mensaje}' "
                };

                return Ok(new { estado = "ok" });
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

        [Route("api/Tramites/CargarDocumentoElectronicoORFEO")]
        [HttpPost]
        public async Task<IHttpActionResult> CargarDocumentoElectronicoORFEO(DatosDocumentoElectronicoDSDto parametros)
        {
            try
            {
                var respuestaAutorizacion = await _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                            RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                            ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                            ConfigurationManager.AppSettings["idReasignarRadicadoORFEO"]);

                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                parametros.usuarioRadica = new UsuarioRadicacionDto();
                parametros.usuarioRadica.Documento = RequestContext.Principal.Identity.Name;

                var result = await Task.Run(() => _tramiteServicios.CargarDocumentoElectronicoORFEO(parametros, RequestContext.Principal.Identity.Name));

                var respuesta = new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    ReasonPhrase = ServiciosNegocioRecursos.PostExitoso
                };

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

        [Route("api/Tramites/ConsultarRadicado")]
        [HttpPost]
        public async Task<IHttpActionResult> ConsultarRadicado(string radicadoSalida)
        {
            try
            {
                var respuestaAutorizacion = await _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                               RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                               ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                               ConfigurationManager.AppSettings["idReasignarRadicadoORFEO"]);

                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);


                var result = await Task.Run(() => _tramiteServicios.ConsultarRadicado(radicadoSalida, RequestContext.Principal.Identity.Name));

                var respuesta = new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    ReasonPhrase = ServiciosNegocioRecursos.PostExitoso
                };

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

        [Route("api/Tramites/CerrarRadicado")]
        [HttpPost]
        public async Task<IHttpActionResult> CerrarRadicado(CerrarRadicadoDto radicadoSalida)
        {
            try
            {
                var respuestaAutorizacion = await _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                               RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                               ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                               ConfigurationManager.AppSettings["idReasignarRadicadoORFEO"]);

                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);


                var result = await Task.Run(() => _tramiteServicios.CerrarRadicado(radicadoSalida, RequestContext.Principal.Identity.Name));

                var respuesta = new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    ReasonPhrase = ServiciosNegocioRecursos.PostExitoso
                };

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

        [Route("api/CerrarRadicadosFlujo")]
        [HttpPost]
        public async Task<IHttpActionResult> CerrarRadicadosFlujo(ObjetoNegocio objetoNegocio)
        {
            try
            {
                var respuestaAutorizacion = await _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                               RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                               ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                               ConfigurationManager.AppSettings["idReasignarRadicadoORFEO"]);

                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                var result = await Task.Run(() => _tramiteServicios.CerrarRadicadosTramite(objetoNegocio.ObjetoNegocioId, RequestContext.Principal.Identity.Name));

                if (result.Estado) {
                    var respuesta = new HttpResponseMessage();
                    respuesta.StatusCode = HttpStatusCode.OK;
                    respuesta.ReasonPhrase = ServiciosNegocioRecursos.PostExitoso;

                    return Ok(respuesta);
                }

                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, result.Mensaje ?? "Error en cierre de radicado"));
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

        [Route("api/CerrarRadicadosFlujoDummy")]
        [HttpPost]
        public async Task<IHttpActionResult> CerrarRadicadosFlujoDummy(ObjetoNegocio objetoNegocio)
        {
            try
            {
                var respuestaAutorizacion = await _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                               RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                               ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                               ConfigurationManager.AppSettings["idReasignarRadicadoORFEO"]);

                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                var result = await Task.Run(() => _tramiteServicios.CerrarRadicadosTramiteDummy(objetoNegocio.ObjetoNegocioId, RequestContext.Principal.Identity.Name));

                if (result.Estado)
                {
                    var respuesta = new HttpResponseMessage();
                    respuesta.StatusCode = HttpStatusCode.OK;
                    respuesta.ReasonPhrase = ServiciosNegocioRecursos.PostExitoso;

                    return Ok(respuesta);
                }

                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, result.Mensaje ?? "Error en cierre de radicado"));
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
        [Route("api/CrearRadicadoEntradaTramite")]
        public async Task<IHttpActionResult> CrearRadicadoEntradaTramite(ObjetoNegocio objetoNegocio)
        {
            try
            {
                var respuestaAutorizacion = await _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                            RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                            ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                            ConfigurationManager.AppSettings["PostGenerarRadicadoEntrada"]);

                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                var creacionRadicadoResponse = await _tramiteServicios.GenerarRadicadoEntrada(
                    objetoNegocio.ObjetoNegocioId,
                    RequestContext.Principal.Identity.Name
                );

                if (creacionRadicadoResponse.Estado)
                {
                    var respuesta = new HttpResponseMessage();
                    respuesta.StatusCode = HttpStatusCode.OK;
                    respuesta.ReasonPhrase = ServiciosNegocioRecursos.PostExitoso;

                    return Ok(respuesta);
                }

                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, creacionRadicadoResponse.Mensaje));
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
        [Route("api/CrearRadicadoEntradaTramiteDummy")]
        public async Task<IHttpActionResult> CrearRadicadoEntradaTramiteDummy(ObjetoNegocio objetoNegocio)
        {
            try
            {
                var respuestaAutorizacion = await _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                            RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                            ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                            ConfigurationManager.AppSettings["PostGenerarRadicadoEntrada"]);

                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                var creacionRadicadoResponse = await _tramiteServicios.GenerarRadicadoEntradaDummy(
                    objetoNegocio.ObjetoNegocioId,
                    RequestContext.Principal.Identity.Name
                );

                if (creacionRadicadoResponse.Estado)
                {
                    var respuesta = new HttpResponseMessage();
                    respuesta.StatusCode = HttpStatusCode.OK;
                    respuesta.ReasonPhrase = ServiciosNegocioRecursos.PostExitoso;

                    return Ok(respuesta);
                }

                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, creacionRadicadoResponse.Mensaje));
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
        [Route("api/GenerarDocumentoFirmadoFlujo")]
        public async Task<IHttpActionResult> GenerarDocumentoFirmadoFlujo(ObjetoNegocio objetoNegocio)
        {
            try
            {
                var respuestaAutorizacion = await _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                            RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                            ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                            ConfigurationManager.AppSettings["PostGenerarRadicadoEntrada"]);

                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                var generarDocumentoResponse = await _tramiteServicios.GenerarDocumentoFirmado(
                    objetoNegocio.ObjetoNegocioId,
                    RequestContext.Principal.Identity.Name
                );

                if (generarDocumentoResponse.Estado)
                {
                    var respuesta = new HttpResponseMessage();
                    respuesta.StatusCode = HttpStatusCode.OK;
                    respuesta.ReasonPhrase = ServiciosNegocioRecursos.PostExitoso;

                    return Ok(respuesta);
                }

                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, generarDocumentoResponse.Mensaje));
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
        [Route("api/GenerarDocumentoFirmadoFlujoDummy")]
        public async Task<IHttpActionResult> GenerarDocumentoFirmadoFlujoDummy(ObjetoNegocio objetoNegocio)
        {
            try
            {
                var respuestaAutorizacion = await _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                            RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                            ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                            ConfigurationManager.AppSettings["PostGenerarRadicadoEntrada"]);

                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                var respuesta = new HttpResponseMessage();
                respuesta.StatusCode = HttpStatusCode.OK;
                respuesta.ReasonPhrase = ServiciosNegocioRecursos.PostExitoso;

                return Ok(respuesta);

                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Error Dummy"));
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
        [Route("api/CrearRadicadoSalidaTramiteFlujoDummy")]
        public async Task<IHttpActionResult> CrearRadicadoSalidaTramiteFlujoDummy(ObjetoNegocio model)
        {
            try
            {
                var respuestaAutorizacion = await _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                            RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                            ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                            ConfigurationManager.AppSettings["PostGenerarRadicadoEntrada"]);

                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                var creacionRadicadoResponse = await _tramiteServicios.GenerarRadicadoSalidaDummy(
                    model.ObjetoNegocioId,
                    RequestContext.Principal.Identity.Name
                );

                if (creacionRadicadoResponse.Estado)
                {
                    var respuesta = new HttpResponseMessage();
                    respuesta.StatusCode = HttpStatusCode.OK;
                    respuesta.ReasonPhrase = ServiciosNegocioRecursos.PostExitoso;

                    return Ok(respuesta);
                }

                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, creacionRadicadoResponse.Mensaje));
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
        [Route("api/CrearRadicadoSalidaTramiteFlujo")]
        public async Task<IHttpActionResult> CrearRadicadoSalidaTramiteFlujo(ObjetoNegocio model)
        {
            try
            {
                var respuestaAutorizacion = await _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                            RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                            ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                            ConfigurationManager.AppSettings["PostGenerarRadicadoEntrada"]);

                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                var creacionRadicadoResponse = await _tramiteServicios.GenerarRadicadoSalida(
                    model.ObjetoNegocioId,
                    RequestContext.Principal.Identity.Name
                );

                if (creacionRadicadoResponse.Estado) {
                    var respuesta = new HttpResponseMessage();
                    respuesta.StatusCode = HttpStatusCode.OK;
                    respuesta.ReasonPhrase = ServiciosNegocioRecursos.PostExitoso;

                    return Ok(respuesta);
                }

                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, creacionRadicadoResponse.Mensaje));
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
        [Route("api/CrearRadicadoSalidaTramite")]
        public async Task<IHttpActionResult> CrearRadicadoSalidaTramite(RadicadoSalidaRequestDto model)
        {
            try
            {
                var respuestaAutorizacion = await _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                            RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                            ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                            ConfigurationManager.AppSettings["PostGenerarRadicadoEntrada"]);

                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                var creacionRadicadoResponse = await _tramiteServicios.GenerarRadicadoSalida(
                    model.NumeroTramite,
                    RequestContext.Principal.Identity.Name  
                );

                if (creacionRadicadoResponse.Estado) {
                    return Ok(creacionRadicadoResponse.Data);
                }

                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, creacionRadicadoResponse.Mensaje));
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
        [Route("api/FirmarCarta")]
        public async Task<IHttpActionResult> FirmarCarta(ObjetoNegocio datosNegocio)
        {
            try
            {
                var respuestaAutorizacion = await _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                            RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                            ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                            ConfigurationManager.AppSettings["PostGenerarRadicadoEntrada"]);

                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                var detalletramite = await _tramiteServicios.ObtenerDetalleTramite(datosNegocio.ObjetoNegocioId, RequestContext.Principal.Identity.Name);
                if (detalletramite == null)
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "No se encontró el tramite"));

                var carta = await _tramiteServicios.ConsultarCarta(detalletramite.TramiteId, RequestContext.Principal.Identity.Name);
                if (carta == null && string.IsNullOrEmpty(carta.RadicadoSalida))
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "No hay radicado de salida en el tramite"));


                var firma = await _tramiteServicios.FirmarCarta(detalletramite.TramiteId, carta.RadicadoSalida, RequestContext.Principal.Identity.Name);
                if (firma.Exito) 
                {
                    return Ok(firma);
                }
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, firma.Mensaje));
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
        [Route("api/CerrarInstancias")]
        public async Task<IHttpActionResult> CerrarInstancias(ObjetoNegocio datosNegocio)
        {
            try
            {
                var respuestaAutorizacion = await _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                            RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                            ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                            ConfigurationManager.AppSettings["PostGenerarRadicadoEntrada"]);

                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                // se separa la firma de la carta
                /*
                var detalletramite = await _tramiteServicios.ObtenerDetalleTramite(datosNegocio.ObjetoNegocioId, RequestContext.Principal.Identity.Name);
                if (detalletramite == null)
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "No se encontró el tramite"));

                var carta = await _tramiteServicios.ConsultarCarta(detalletramite.TramiteId, RequestContext.Principal.Identity.Name);
                if (carta == null && string.IsNullOrEmpty(carta.RadicadoSalida))
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "No hay radicado de salida en el tramite"));

                var firma = await _tramiteServicios.FirmarCarta(detalletramite.TramiteId, carta.RadicadoSalida, RequestContext.Principal.Identity.Name);
                if (carta == null && string.IsNullOrEmpty(carta.RadicadoSalida))
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "No hay radicado de salida en el tramite"));
                */

                var creacionRadicadoResponse = await _tramiteServicios.CerrarInstancias(
                    datosNegocio.ObjetoNegocioId,
                    RequestContext.Principal.Identity.Name
                );

                if (creacionRadicadoResponse.Estado)
                {
                    return Ok(creacionRadicadoResponse);
                }

                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, creacionRadicadoResponse.Mensaje));
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

        [Route("api/Tramites/ObtenerYCargarDocumentoElectronicoORFEO")]
        [HttpPost]
        public async Task<IHttpActionResult> ObtenerYCargarDocumentoElectronicoORFEO(ObjetoNegocio datosNegocio)
        {
            try
            {
                var respuestaAutorizacion = await _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                            RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                            ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                            ConfigurationManager.AppSettings["PostGenerarRadicadoEntrada"]);

                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                var detalletramite = await _tramiteServicios.ObtenerDetalleTramite(datosNegocio.ObjetoNegocioId, RequestContext.Principal.Identity.Name);
                if (detalletramite == null)
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "No se encontró el tramite"));
                
                var carta = await _tramiteServicios.ConsultarCarta(detalletramite.TramiteId, RequestContext.Principal.Identity.Name);
                if (carta == null && string.IsNullOrEmpty(carta.RadicadoSalida))
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "No hay radicado de salida en el tramite"));

                var archivo = await _tramiteServicios.ObtenerPDF(detalletramite.TramiteId, detalletramite.TipoTramiteId, RequestContext.Principal.Identity.Name);
                if(archivo.Equals("Error"))
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Error la obtener el PDF físico"));
                
                DatosDocumentoElectronicoDSDto parametros = CargarParametrosRadicadoSalida(archivo, carta.RadicadoSalida, datosNegocio.ObjetoNegocioId);
               

                parametros.usuarioRadica = new UsuarioRadicacionDto();
                parametros.usuarioRadica.Documento = RequestContext.Principal.Identity.Name;

                var result = await Task.Run(() => _tramiteServicios.CargarDocumentoElectronicoORFEO(parametros, RequestContext.Principal.Identity.Name));

                var respuesta = new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    ReasonPhrase = ServiciosNegocioRecursos.PostExitoso
                };

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

        [Route("api/NotificarUsuarios")]
        [HttpPost]
        public async Task<IHttpActionResult> NotificarUsuarios(ObjetoNegocio datosNegocio )
        {
            CommonResponseDto<bool> respuesta = new CommonResponseDto<bool>();
            var respuestaAutorizacion = await _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                            RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                            ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                            ConfigurationManager.AppSettings["PostGenerarRadicadoEntrada"]);

            if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

            var detalletramite = await _tramiteServicios.ObtenerDetalleTramite(datosNegocio.ObjetoNegocioId, RequestContext.Principal.Identity.Name);
            if (detalletramite == null)
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "No se encontró el tramite"));

            var respuestatmp = await _tramiteServicios.NotificarUsuarios(detalletramite.InstanciaId,  RequestContext.Principal.Identity.Name);
            if (respuestatmp)
            {
                respuesta.Data = true;
                respuesta.Estado = true;
                return Ok(respuesta);
            }
            else
                return Ok(respuesta);
        }

        [Route("api/EliminarMarcaPrevioProyectoVigencia")]
        [HttpPost]
        public async Task<IHttpActionResult> EliminarMarcaPrevioProyectoVigencia(ObjetoNegocio datosNegocio)
        {
            CommonResponseDto<bool> respuesta = new CommonResponseDto<bool>();
            var datosproyecto = _tramiteServicios.ObtenerDatosMarcaPrevioVigencia_Proyectos(datosNegocio.ObjetoNegocioId);
            if (datosproyecto == null)
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "No se encontró el proceso"));

            datosNegocio.Vigencia = DateTime.Now.Year.ToString();
            var rta = await _tramiteServicios.EliminarMarcaPrevioProyectoVigencia(datosNegocio.ObjetoNegocioId, datosNegocio.Vigencia, datosproyecto, RequestContext.Principal.Identity.Name);
            if (string.IsNullOrEmpty(rta) || !rta.Equals("Exitoso"))
            {
                respuesta.Data = true;
                respuesta.Estado = true;
            }
            else
            {
                respuesta.Data = false;
                respuesta.Estado = false;
               
            }
            respuesta.Mensaje = rta;
            return Ok(respuesta);
        }

        [Route("api/Tramites/ActualizarCargueMasivo")]
        [HttpPost]
        public async Task<IHttpActionResult> ActualizarCargueMasivo(ObjetoNegocio contenido)
        {
            try
            {
                var respuestaAutorizacion = await _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                           RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                           ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                           ConfigurationManager.AppSettings["actualizarCargueMasivo"]);

                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                if (contenido == null)
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NoContent, new Exception(string.Format(ServiciosNegocioRecursos.ParametrosNoRecibidos))));


                var response = await Task.Run(() => _tramiteServicios.ActualizarCargueMasivo(contenido.ObjetoNegocioId, RequestContext.Principal.Identity.Name));

                var respuesta = new HttpResponseMessage();
                if (response.Estado)
                {
                    respuesta.StatusCode = HttpStatusCode.OK;
                    respuesta.ReasonPhrase = ServiciosNegocioRecursos.PostExitoso;
                }
                else
                {
                    respuesta.StatusCode = HttpStatusCode.NoContent;
                    respuesta.ReasonPhrase = response.Mensaje;
                }
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


        [Route("api/Tramites/ConsultarCargueExcel")]
        [HttpPost]
        public async Task<IHttpActionResult> ConsultarCargueExcel(ObjetoNegocio contenido)
        {
            var result = await Task.Run(() => _tramiteServicios.ConsultarCargueExcel(contenido.ObjetoNegocioId));

            return Ok(result);
        }

        [Route("api/EnviarCorreoMarcaPrevio")]
        [HttpPost]
        public async Task<IHttpActionResult> EnviarCorreoMarcaPrevio(ObjetoNegocio datosNegocio)
        {
            CommonResponseDto<bool> respuesta = new CommonResponseDto<bool>();
            var datosproyecto = _tramiteServicios.ObtenerDatosMarcaPrevioVigencia_Proyectos(datosNegocio.ObjetoNegocioId);
            if (datosproyecto == null)
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "No se encontró el proceso"));

            if (datosproyecto.Count > 0)
            {
                if (datosproyecto[0].ValorVigente == 0)
                {
                    respuesta.Data = true;
                    respuesta.Estado = true;
                    respuesta.Mensaje = "Exitoso";
                    return Ok(respuesta);
                }
            }

            datosNegocio.Vigencia = DateTime.Now.Year.ToString();
            var rta = await _tramiteServicios.EnviarCorreoMarcaPrevio(datosNegocio.ObjetoNegocioId, datosNegocio.Vigencia, datosproyecto, RequestContext.Principal.Identity.Name);
            if (string.IsNullOrEmpty(rta) || !rta.Equals("Exitoso"))
            {
                respuesta.Data = true;
                respuesta.Estado = true;
            }
            else
            {
                respuesta.Data = false;
                respuesta.Estado = false;

            }
            respuesta.Mensaje = rta;
            return Ok(respuesta);
        }



        [Route("api/NotificarMarcaPrevio")]
        [HttpPost]
        public async Task<IHttpActionResult> NotificarMarcaPrevio(ObjetoNegocio datosNegocio)
        {
            CommonResponseDto<bool> respuesta = new CommonResponseDto<bool>();
            var datosproyecto = _tramiteServicios.ObtenerDatosMarcaPrevioVigencia_Proyectos(datosNegocio.ObjetoNegocioId);
            if (datosproyecto == null)
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "No se encontró el proceso"));

            datosNegocio.Vigencia = DateTime.Now.Year.ToString();
            var rta = await _tramiteServicios.NotificarMarcaPrevio(datosNegocio.ObjetoNegocioId, datosNegocio.Vigencia, datosproyecto, RequestContext.Principal.Identity.Name);
            if (string.IsNullOrEmpty(rta) || !rta.Equals("Exitoso"))
            {
                respuesta.Data = true;
                respuesta.Estado = true;
            }
            else
            {
                respuesta.Data = false;
                respuesta.Estado = false;

            }
            respuesta.Mensaje = rta;
            return Ok(respuesta);
        }


        private DatosDocumentoElectronicoDSDto CargarParametrosRadicadoSalida(string archivo, string radicadoSalida, string numeroTramite)
        {
            DatosDocumentoElectronicoDSDto parametros = new DatosDocumentoElectronicoDSDto();
            parametros.datosDocumentoElectronicoDto = new DatosDocumentoElectronicoDto();
            parametros.datosDocumentoElectronicoDto.fileBase64Bin = archivo;
            parametros.datosDocumentoElectronicoDto.extension = "pdf";
            parametros.datosDocumentoElectronicoDto.nombre = "TramiteCartaConcepto";
            parametros.usuarioRadica = new UsuarioRadicacionDto();
            parametros.usuarioRadica.Documento = string.Empty;
            parametros.usuarioRadica.Login = string.Empty;
            parametros.datosRadicadoDto = new DatosRadicadoDto();
            parametros.datosRadicadoDto.esPrincipal = true;
            parametros.datosRadicadoDto.observacion = "Documento  de firma trámite con número " + numeroTramite;
            parametros.datosRadicadoDto.NoRadicado = !string.IsNullOrEmpty(radicadoSalida) ? System.Convert.ToDecimal(radicadoSalida)  : 0;
            return parametros;
        }
    
    }
}