using DNP.ServiciosNegocio.Comunes;
using DNP.ServiciosNegocio.Comunes.Autorizacion;
using DNP.ServiciosNegocio.Comunes.Excepciones;
using DNP.ServiciosNegocio.Dominio.Dto.FuenteFinanciacion;
using DNP.ServiciosNegocio.Servicios.Interfaces.FuenteFinanciacion;
using Swashbuckle.Swagger.Annotations;
using System.Configuration;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;

namespace DNP.ServiciosNegocio.Web.API.Controllers.Negocio
{
    using System;
    using System.Linq;
    using System.Net.Http;
    using Comunes.Dto;
    using Comunes.Dto.Formulario;
    using DNP.ServiciosNegocio.Servicios.Implementaciones.FuenteFinanciacion;

    public class FuenteFinanciacionAgregarController : ApiController
    {
        private readonly IFuenteFinanciacionAgregarServicio _fuenteFinanciacionAgregarServicio;
        private readonly IAutorizacionUtilidades _autorizacionUtilidades;

        public FuenteFinanciacionAgregarController(IFuenteFinanciacionAgregarServicio fuenteFinanciacionAgregarServicio, IAutorizacionUtilidades autorizacionUtilidades)
        {
            _fuenteFinanciacionAgregarServicio = fuenteFinanciacionAgregarServicio;
            _autorizacionUtilidades = autorizacionUtilidades;
        }

        [Route("api/FuenteFinanciacionAgregar")]
        [SwaggerResponse(HttpStatusCode.OK, "Retorna listado de Fuentes de Financiacion", typeof(ProyectoFuenteFinanciacionAgregarDto))]
        [HttpGet]
        public async Task<IHttpActionResult> Consultar(string bpin)
        {
            var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                                               RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                                               ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                                               ConfigurationManager.AppSettings["consultarFuentesFinanciacionAgregar"]).Result;

            if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

            ValidacionParametro(bpin, Request);


            var result = await Task.Run(() => _fuenteFinanciacionAgregarServicio.ObtenerFuenteFinanciacionAgregar(
                                                                                                             new ParametrosConsultaDto
                                                                                                             {
                                                                                                                 Bpin =
                                                                                                                     bpin,
                                                                                                                 AccionId
                                                                                                                     = new
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
                                                                                                             }

                                                                                                             ));
            if (result != null) return Ok(result);

            var respuestaHttp = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.NotFound,
                ReasonPhrase = ServiciosNegocioRecursos.SinResultados
            };

            return ResponseMessage(respuestaHttp);
        }

        [Route("api/FuenteFinanciacion/Consultar")]
        [SwaggerResponse(HttpStatusCode.OK, "Retorna listado de Fuentes de Financiacion", typeof(ProyectoFuenteFinanciacionAgregarDto))]
        [HttpGet]
        public async Task<IHttpActionResult> ConsultarFuenteFinanciacionN(string bpin)
        {
            var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                                               RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                                               ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                                               ConfigurationManager.AppSettings["consultarFuentesFinanciacionAgregarN"]).Result;

            if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

            //ValidacionParametro(bpin, Request);


            var result = await Task.Run(() => _fuenteFinanciacionAgregarServicio.ObtenerFuenteFinanciacionAgregarN(bpin));
            if (result != null) return Ok(result);

            var respuestaHttp = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.NotFound,
                ReasonPhrase = ServiciosNegocioRecursos.SinResultados
            };

            return ResponseMessage(respuestaHttp);
        }

        [Route("api/FuenteFinanciacion/ConsultarFuenteFinanciacionVigencia")]
        [SwaggerResponse(HttpStatusCode.OK, "Retorna listado de Fuentes de Financiacion, cofinanciador, vigencia", typeof(ProyectoFuenteFinanciacionAgregarDto))]
        [HttpGet]
        public async Task<IHttpActionResult> ConsultarFuenteFinanciacionVigencia(string bpin)
        {
            var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                                               RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                                               ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                                               ConfigurationManager.AppSettings["consultarFuentesFinanciacionAgregarN"]).Result;

            if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

            var result = await Task.Run(() => _fuenteFinanciacionAgregarServicio.ObtenerFuenteFinanciacionVigencia(bpin));
            if (result != null) return Ok(result);

            var respuestaHttp = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.NotFound,
                ReasonPhrase = ServiciosNegocioRecursos.SinResultados
            };

            return ResponseMessage(respuestaHttp);
        }

        [SuppressMessage("ReSharper", "UnusedVariable")]
        private void ValidacionParametro(string bpin, HttpRequestMessage peticion)
        {
            if (string.IsNullOrEmpty(bpin))
                throw new ServiciosNegocioException(string.Format(ServiciosNegocioRecursos.ParametroNoRecibido, nameof(bpin)));

            if (!long.TryParse(bpin, out var outBpin) || bpin.Length > 100)
                throw new ServiciosNegocioException(string.Format(ServiciosNegocioRecursos.ParametroNoValidos, nameof(bpin)));

            if (peticion.Headers.Contains("piip-idFormulario"))
            {
                if (string.IsNullOrEmpty(peticion.Headers.GetValues("piip-idFormulario").First()))
                    throw new ServiciosNegocioException(string.Format(ServiciosNegocioRecursos.ParametroNoRecibido, "piip-idFormulario"));

                Guid outGuidAccionId = Guid.Empty;

                if (!Guid.TryParse(peticion.Headers.GetValues("piip-idFormulario").First(), out outGuidAccionId))
                    throw new ServiciosNegocioException(string.Format(ServiciosNegocioRecursos.ParametroNoValidos, "piip-idFormulario"));
            }
            else
            {
                throw new ServiciosNegocioException(string.Format(ServiciosNegocioRecursos.ParametroNoRecibido,
                                                             "piip-idFormulario"));
            }
        }

        [Route("api/FuenteFinanciacionAgregar/Preview")]
        [SwaggerResponse(HttpStatusCode.OK, "Retorna listado de Fuentes de Financiacion dummy", typeof(ProyectoFuenteFinanciacionAgregarDto))]
        [HttpGet]
        public async Task<IHttpActionResult> Preview()
        {
            var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                                               RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                                               ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                                               ConfigurationManager.AppSettings["previewFuentesFinanciacionAgregar"]).Result;

            if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

            var result = await Task.Run(() => _fuenteFinanciacionAgregarServicio.ObtenerFuenteFinanciacionAgregarPreview());
            return Ok(result);
        }

        [Route("api/FuenteFinanciacionAgregar")]
        [SwaggerResponse(HttpStatusCode.OK, "Informacion devuelta por guardado definitivo", typeof(HttpResponseMessage))]
        [HttpPost]
        public async Task<IHttpActionResult> Definitivo(ProyectoFuenteFinanciacionAgregarDto proyectoFuenteFinanciacionAgregarDto)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name, RequestContext.Principal.Identity.GetHashCode().ToString(), ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"], ConfigurationManager.AppSettings["postFuentesFinanciacionAgregar"]).Result;
                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                var parametrosGuardar = _fuenteFinanciacionAgregarServicio.ConstruirParametrosGuardado(Request, proyectoFuenteFinanciacionAgregarDto);

                var parametrosAuditoria = new ParametrosAuditoriaDto
                {
                    Usuario = RequestContext.Principal.Identity.Name,
                    Ip = UtilidadesApi.GetClientIp(Request)
                };

                await Task.Run(() => _fuenteFinanciacionAgregarServicio.Guardar(parametrosGuardar, parametrosAuditoria, false));

                var respuesta = new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    ReasonPhrase = ServiciosNegocioRecursos.PostExitoso
                };

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

        [Route("api/FuenteFinanciacionAgregar/Temporal")]
        [SwaggerResponse(HttpStatusCode.OK, "Informacion devuelta por guardado definitivo", typeof(HttpResponseMessage))]
        [HttpPost]
        public async Task<IHttpActionResult> Temporal(ProyectoFuenteFinanciacionAgregarDto proyectoFuenteFinanciacionAgregarDto)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name, RequestContext.Principal.Identity.GetHashCode().ToString(), ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"], ConfigurationManager.AppSettings["postFuentesFinanciacionAgregarTemporal"]).Result;
                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                var parametrosGuardar = _fuenteFinanciacionAgregarServicio.ConstruirParametrosGuardado(Request, proyectoFuenteFinanciacionAgregarDto);

                var parametrosAuditoria = new ParametrosAuditoriaDto
                {
                    Usuario = RequestContext.Principal.Identity.Name,
                    Ip = UtilidadesApi.GetClientIp(Request)
                };

                await Task.Run(() => _fuenteFinanciacionAgregarServicio.Guardar(parametrosGuardar, parametrosAuditoria, true));

                var respuesta = new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    ReasonPhrase = ServiciosNegocioRecursos.PostExitoso
                };

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

        [Route("api/FuenteFinanciacionAgregarN")]
        [SwaggerResponse(HttpStatusCode.OK, "Informacion devuelta por guardado definitivo", typeof(HttpResponseMessage))]
        [HttpPost]
        public async Task<IHttpActionResult> GuardarFuenteFinanciacion(ProyectoFuenteFinanciacionAgregarDto proyectoFuenteFinanciacionAgregarDto)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name, RequestContext.Principal.Identity.GetHashCode().ToString(), ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"], ConfigurationManager.AppSettings["postFuentesFinanciacionAgregar"]).Result;
                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);
                var parametrosGuardar = new ParametrosGuardarDto<ProyectoFuenteFinanciacionAgregarDto>();
                parametrosGuardar.Contenido = proyectoFuenteFinanciacionAgregarDto;

                await Task.Run(() => _fuenteFinanciacionAgregarServicio.GuardarFuenteFinanciacion(parametrosGuardar, RequestContext.Principal.Identity.Name));

                var respuesta = new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    ReasonPhrase = ServiciosNegocioRecursos.PostExitoso
                };

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

        [Route("api/FuenteFinanciacion/Eliminar")]
        [SwaggerResponse(HttpStatusCode.OK, "Eliminar la fuente de Financiacion", typeof(HttpResponseMessage))]
        [HttpPost]
        public async Task<IHttpActionResult> Eliminar(int fuenteFinanciacionId)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name, RequestContext.Principal.Identity.GetHashCode().ToString(), ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"], ConfigurationManager.AppSettings["postFuenteFinanciacion"]).Result;
                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                var parametrosAuditoria = new ParametrosAuditoriaDto
                {
                    Usuario = RequestContext.Principal.Identity.Name,
                    Ip = UtilidadesApi.GetClientIp(Request)
                };

                await Task.Run(() => _fuenteFinanciacionAgregarServicio.EliminarFuentesFinanciacionProyecto(fuenteFinanciacionId));
                var respuesta = new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    ReasonPhrase = ServiciosNegocioRecursos.PostExitoso
                };
                return Ok(respuesta);

            }
            catch (ServiciosNegocioException e)
            {
                //throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
                var respuesta = new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.NoContent,
                    ReasonPhrase = e.Message,
                };
                return Ok(respuesta);
            }
            catch (AutorizacionException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, e));
            }
        }

        [Route("api/CostosVsSolicitado/Consultar")]
        [SwaggerResponse(HttpStatusCode.OK, "Retorna listado de Fuentes de Financiacion", typeof(ResumenCostosVsSolicitado))]
        [HttpGet]
        public async Task<IHttpActionResult> ConsultarCostosVsSolicitado(string bpin)
        {
            //var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
            //                                                                   RequestContext.Principal.Identity.GetHashCode().ToString(),
            //                                                                   ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
            //                                                                   ConfigurationManager.AppSettings["consultarResumenCostosVsSolicitado"]).Result;

            //if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

            //ValidacionParametro(bpin, Request);


            var result = await Task.Run(() => _fuenteFinanciacionAgregarServicio.ObtenerResumenCostosVsSolicitado(bpin));
            if (result != null) return Ok(result);

            var respuestaHttp = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.NotFound,
                ReasonPhrase = ServiciosNegocioRecursos.SinResultados
            };

            return ResponseMessage(respuestaHttp);
        }

        [Route("api/ConsultarResumenFteFinanciacion")]
        [SwaggerResponse(HttpStatusCode.OK, "Retorna listado de Fuentes de Financiacion", typeof(ProyectoFuenteFinanciacionAgregarDto))]
        [HttpGet]
        public async Task<IHttpActionResult> ConsultarResumenFteFinanciacion(string bpin)
        {
            //var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
            //                                                                   RequestContext.Principal.Identity.GetHashCode().ToString(),
            //                                                                   ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
            //                                                                   ConfigurationManager.AppSettings["ConsultarResumenFteFinanciacion"]).Result;

            //if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

            //ValidacionParametro(bpin, Request);


            var result = await Task.Run(() => _fuenteFinanciacionAgregarServicio.ConsultarResumenFteFinanciacion(bpin));
            if (result != null) return Ok(result);

            var respuestaHttp = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.NotFound,
                ReasonPhrase = ServiciosNegocioRecursos.SinResultados
            };

            return ResponseMessage(respuestaHttp);
        }

        [Route("api/ConsultarCostosPIIPvsFuentesPIIP")]
        [SwaggerResponse(HttpStatusCode.OK, "Retorna el resumen de fuentes de financiación en ajuste vs costo de actividad", typeof(ProyectoFuenteFinanciacionAgregarDto))]
        [HttpGet]
        public async Task<IHttpActionResult> ConsultarCostosPIIPvsFuentesPIIP(string bpin)
        {
            var result = await Task.Run(() => _fuenteFinanciacionAgregarServicio.ConsultarCostosPIIPvsFuentesPIIP(bpin));
            if (result != null) return Ok(result);

            var respuestaHttp = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.NotFound,
                ReasonPhrase = ServiciosNegocioRecursos.SinResultados
            };

            return ResponseMessage(respuestaHttp);
        }

        [Route("api/FuentesFinanciacionRecursosAjustes/Agregar")]
        [SwaggerResponse(HttpStatusCode.OK, "Informacion devuelta por guardado definitivo", typeof(HttpResponseMessage))]
        [HttpPost]
        public async Task<IHttpActionResult> FuentesFinanciacionRecursosAjustesAgregar(FuenteFinanciacionAgregarAjusteDto objFuenteFinanciacionAgregarAjusteDto)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name, RequestContext.Principal.Identity.GetHashCode().ToString(), ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"], ConfigurationManager.AppSettings["postFuentesFinanciacionAgregar"]).Result;
                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                string resp = await Task.Run(() => _fuenteFinanciacionAgregarServicio.FuentesFinanciacionRecursosAjustesAgregar(objFuenteFinanciacionAgregarAjusteDto, RequestContext.Principal.Identity.Name));

                var respuesta = new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    ReasonPhrase = ServiciosNegocioRecursos.PostExitoso
                };

                return Ok(respuesta);
            }
            catch (ServiciosNegocioException e)
            {
                //throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
                var respuesta = new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.NoContent,
                    ReasonPhrase = e.Message,
                };
                return Ok(respuesta);
            }
            catch (AutorizacionException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, e));
            }
        }

        [Route("api/ObtenerDetalleAjustesFuenteFinanciacion")]
        [HttpPost]
        public async Task<IHttpActionResult> ObtenerDetalleAjustesFuenteFinanciacion(string Bpin)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name, RequestContext.Principal.Identity.GetHashCode().ToString(), ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"], ConfigurationManager.AppSettings["GestionTramitesProyectos"]).Result;
                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                var result = await Task.Run(() => _fuenteFinanciacionAgregarServicio.ObtenerDetalleAjustesFuenteFinanciacion(Bpin, RequestContext.Principal.Identity.Name));

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

        [Route("api/ObtenerDetalleAjustesJustificaionFacalizacionPT")]
        [HttpPost]
        public async Task<IHttpActionResult> ObtenerDetalleAjustesJustificaionFacalizacionPT(string Bpin)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name, RequestContext.Principal.Identity.GetHashCode().ToString(), ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"], ConfigurationManager.AppSettings["GestionTramitesProyectos"]).Result;
                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                var result = await Task.Run(() => _fuenteFinanciacionAgregarServicio.ObtenerDetalleAjustesJustificaionFacalizacionPT(Bpin, RequestContext.Principal.Identity.Name));

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

        [Route("api/FuenteFinanciacion/ObtenerOperacionCreditoDatosGenerales")]
        [SwaggerResponse(HttpStatusCode.OK, "Obtener Operacion Credito Datos Generales", typeof(OperacionCreditoDatosGeneralesDto))]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerOperacionCreditoDatosGenerales(string bpin)
        {
            //var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
            //                                                                   RequestContext.Principal.Identity.GetHashCode().ToString(),
            //                                                                   ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
            //                                                                   ConfigurationManager.AppSettings["obtenerOperacionCreditoDatosGenerales"]).Result;

            //if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

            var result = await Task.Run(() => _fuenteFinanciacionAgregarServicio.ObtenerOperacionCreditoDatosGenerales(bpin, null));
            if (result != null) return Ok(result);

            var respuestaHttp = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.NotFound,
                ReasonPhrase = ServiciosNegocioRecursos.SinResultados
            };

            return ResponseMessage(respuestaHttp);
        }

        [Route("api/FuenteFinanciacion/GuardarOperacionCreditoDatosGenerales")]
        [SwaggerResponse(HttpStatusCode.OK, "Guardar Operacion Credito Datos Generales", typeof(HttpResponseMessage))]
        [HttpPost]
        public async Task<IHttpActionResult> GuardarOperacionCreditoDatosGenerales(OperacionCreditoDatosGeneralesDto OperacionCreditoDatosGeneralesDto)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name, 
                                                                                   RequestContext.Principal.Identity.GetHashCode().ToString(), 
                                                                                   ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"], 
                                                                                   ConfigurationManager.AppSettings["postOperacionCreditoDatosGenerales"]).Result;
                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                var parametrosAuditoria = new ParametrosAuditoriaDto
                {
                    Usuario = RequestContext.Principal.Identity.Name,
                    Ip = UtilidadesApi.GetClientIp(Request)
                };

                _fuenteFinanciacionAgregarServicio.Usuario = RequestContext?.Principal?.Identity?.Name;
                _fuenteFinanciacionAgregarServicio.Ip = UtilidadesApi.GetClientIp(Request);

                await Task.Run(() => _fuenteFinanciacionAgregarServicio.GuardarOperacionCreditoDatosGenerales(OperacionCreditoDatosGeneralesDto, RequestContext.Principal.Identity.Name));
                var respuesta = new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    ReasonPhrase = ServiciosNegocioRecursos.PostExitoso
                };
                return Ok(respuesta);

            }
            catch (ServiciosNegocioException e)
            {
                var respuesta = new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.NoContent,
                    ReasonPhrase = e.Message,
                };
                return Ok(respuesta);
            }
            catch (AutorizacionException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, e));
            }
        }

        [Route("api/FuenteFinanciacion/ObtenerOperacionCreditoDetalles")]
        [SwaggerResponse(HttpStatusCode.OK, "Obtener Operacion Credito Detalles", typeof(OperacionCreditoDetallesDto))]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerOperacionCreditoDetalles(string bpin)
        {
            var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                                               RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                                               ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                                               ConfigurationManager.AppSettings["obtenerOperacionCreditoDetalles"]).Result;

            if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

            var result = await Task.Run(() => _fuenteFinanciacionAgregarServicio.ObtenerOperacionCreditoDetalles(bpin, null));
            if (result != null) return Ok(result);

            var respuestaHttp = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.NotFound,
                ReasonPhrase = ServiciosNegocioRecursos.SinResultados
            };

            return ResponseMessage(respuestaHttp);
        }

        [Route("api/FuenteFinanciacion/GuardarOperacionCreditoDetalles")]
        [SwaggerResponse(HttpStatusCode.OK, "Guardar Operacion Credito Detalles", typeof(HttpResponseMessage))]
        [HttpPost]
        public async Task<IHttpActionResult> GuardarOperacionCreditoDetalles(OperacionCreditoDetallesDto OperacionCreditoDetallesDto)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                                                   RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                                                   ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                                                   ConfigurationManager.AppSettings["postOperacionCreditoDetalles"]).Result;
                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                var parametrosAuditoria = new ParametrosAuditoriaDto
                {
                    Usuario = RequestContext.Principal.Identity.Name,
                    Ip = UtilidadesApi.GetClientIp(Request)
                };

                _fuenteFinanciacionAgregarServicio.Ip = UtilidadesApi.GetClientIp(Request);
                _fuenteFinanciacionAgregarServicio.Usuario = RequestContext.Principal.Identity.Name;


                await Task.Run(() => _fuenteFinanciacionAgregarServicio.GuardarOperacionCreditoDetalles(OperacionCreditoDetallesDto, RequestContext.Principal.Identity.Name));
                var respuesta = new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    ReasonPhrase = ServiciosNegocioRecursos.PostExitoso
                };
                return Ok(respuesta);

            }
            catch (ServiciosNegocioException e)
            {
                var respuesta = new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.NoContent,
                    ReasonPhrase = e.Message,
                };
                return Ok(respuesta);
            }
            catch (AutorizacionException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, e));
            }
        }
    }
}