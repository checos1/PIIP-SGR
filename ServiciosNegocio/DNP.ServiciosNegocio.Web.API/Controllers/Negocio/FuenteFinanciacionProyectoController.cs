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
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using Comunes.Dto;
    using Comunes.Dto.Formulario;
    using DNP.ServiciosNegocio.Dominio.Dto.Focalizacion;
    using DNP.ServiciosNegocio.Dominio.Dto.Genericos;
    using DNP.ServiciosNegocio.Dominio.Dto.IndicadoresPolitica;
    using DNP.ServiciosNegocio.Dominio.Dto.Proyectos;

    public class FuenteFinanciacionProyectoController : ApiController
    {
        private readonly IFuenteFinanciacionServicios _fuenteFinanciacionServicios;
        private readonly IAutorizacionUtilidades _autorizacionUtilidades;

        public FuenteFinanciacionProyectoController(IFuenteFinanciacionServicios fuenteFinanciacionServicios, IAutorizacionUtilidades autorizacionUtilidades)
        {
            _fuenteFinanciacionServicios = fuenteFinanciacionServicios;
            _autorizacionUtilidades = autorizacionUtilidades;
        }

        [Route("api/FuenteFinanciacionProyecto")]
        [SwaggerResponse(HttpStatusCode.OK, "Retorna listado de Fuentes de Financiacion", typeof(ProyectoFuenteFinanciacionDto))]
        [HttpGet]
        public async Task<IHttpActionResult> Consultar(string bpin)
        {
            var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                                               RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                                               ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                                               ConfigurationManager.AppSettings["consultarFuenteFinanciacion"]).Result;

            if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

            ValidacionParametro(bpin, Request);


            var result = await Task.Run(() => _fuenteFinanciacionServicios.ObtenerFuenteFinanciacionProyecto(
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

        [Route("api/FuenteFinanciacionProyecto/Preview")]
        [SwaggerResponse(HttpStatusCode.OK, "Retorna listado de Fuentes de Financiacion dummy", typeof(ProyectoFuenteFinanciacionDto))]
        [HttpGet]
        public async Task<IHttpActionResult> Preview()
        {
            var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                                               RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                                               ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                                               ConfigurationManager.AppSettings["previewFuenteFinanciacion"]).Result;

            if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

            var result = await Task.Run(() => _fuenteFinanciacionServicios.ObtenerFuenteFinanciacionProyectoPreview());
            return Ok(result);
        }


        [Route("api/FuenteFinanciacionProyecto")]
        [SwaggerResponse(HttpStatusCode.OK, "Informacion devuelta por guardado definitivo", typeof(HttpResponseMessage))]
        [HttpPost]
        public async Task<IHttpActionResult> Definitivo(ProyectoFuenteFinanciacionDto proyectoFuenteFinanciacionDto)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name, RequestContext.Principal.Identity.GetHashCode().ToString(), ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"], ConfigurationManager.AppSettings["postFuenteFinanciacion"]).Result;
                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                var parametrosGuardar = _fuenteFinanciacionServicios.ConstruirParametrosGuardado(Request, proyectoFuenteFinanciacionDto);

                var parametrosAuditoria = new ParametrosAuditoriaDto
                {
                    Usuario = RequestContext.Principal.Identity.Name,
                    Ip = UtilidadesApi.GetClientIp(Request)
                };

                await Task.Run(() => _fuenteFinanciacionServicios.Guardar(parametrosGuardar, parametrosAuditoria, false));

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

        [Route("api/FuenteFinanciacionProyecto/Temporal")]
        [SwaggerResponse(HttpStatusCode.OK, "Informacion devuelta por guardado definitivo", typeof(HttpResponseMessage))]
        [HttpPost]
        public async Task<IHttpActionResult> Temporal(ProyectoFuenteFinanciacionDto proyectoFuenteFinanciacionDto)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name, RequestContext.Principal.Identity.GetHashCode().ToString(), ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"], ConfigurationManager.AppSettings["postFuenteFinanciacion"]).Result;
                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                var parametrosGuardar = _fuenteFinanciacionServicios.ConstruirParametrosGuardado(Request, proyectoFuenteFinanciacionDto);

                var parametrosAuditoria = new ParametrosAuditoriaDto
                {
                    Usuario = RequestContext.Principal.Identity.Name,
                    Ip = UtilidadesApi.GetClientIp(Request)
                };

                await Task.Run(() => _fuenteFinanciacionServicios.Guardar(parametrosGuardar, parametrosAuditoria, true));

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

        [Route("api/Focalizacion/ConsultarPoliticasTransversalesAjustes")]
        [SwaggerResponse(HttpStatusCode.OK, "Retorna string con listado de politicas transversales por proyecto.", typeof(HttpResponseMessage))]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerPoliticasTransversalesAjustes(string BPIN)
        {
            var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                                                RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                                                ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                                                ConfigurationManager.AppSettings["ObtenerDatosAdicionalesFuente"]).Result;

            if (!respuestaAutorizacion.IsSuccessStatusCode)
                return ResponseMessage(respuestaAutorizacion);

            var result = await Task.Run(() => _fuenteFinanciacionServicios.ObtenerPoliticasTransversalesAjustes(BPIN));
            if (result != null) return Ok(result);

            var respuestaHttp = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.NotFound,
                ReasonPhrase = ServiciosNegocioRecursos.SinResultados
            };

            return ResponseMessage(respuestaHttp);
        }


        [Route("api/Focalizacion/AgregarPoliticasTransversalesAjustes")]
        [SwaggerResponse(HttpStatusCode.OK, "Informacion devuelta por guardado definitivo", typeof(HttpResponseMessage))]
        [HttpPost]
        public async Task<IHttpActionResult> AgregarPoliticasTransversalesAjustes(IncluirPoliticasDto objIncluirPoliticasDto)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                    RequestContext.Principal.Identity.GetHashCode().ToString(), ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                    ConfigurationManager.AppSettings["postDatosAdicionalesAgregar"]).Result;
                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                var parametrosGuardar = new ParametrosGuardarDto<IncluirPoliticasDto>();
                parametrosGuardar.Contenido = objIncluirPoliticasDto;


                var parametrosAuditoria = new ParametrosAuditoriaDto
                {
                    Usuario = RequestContext.Principal.Identity.Name,
                    Ip = UtilidadesApi.GetClientIp(Request)
                };

                var result = await Task.Run(() => _fuenteFinanciacionServicios.GuardarPoliticasTransversalesAjustes(parametrosGuardar, RequestContext.Principal.Identity.Name));

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

        [Route("api/Focalizacion/ConsultarPoliticasTransversalesCategorias")]
        [SwaggerResponse(HttpStatusCode.OK, "Retorna string con listado de politicas transversales por proyecto.", typeof(HttpResponseMessage))]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerPoliticasTransversalesCategorias(string BPIN)
        {
            var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                                                RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                                                ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                                                ConfigurationManager.AppSettings["ObtenerDatosAdicionalesFuente"]).Result;

            if (!respuestaAutorizacion.IsSuccessStatusCode)
                return ResponseMessage(respuestaAutorizacion);

            var result = await Task.Run(() => _fuenteFinanciacionServicios.ObtenerPoliticasTransversalesCategorias(BPIN));
            if (result != null) return Ok(result);

            var respuestaHttp = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.NotFound,
                ReasonPhrase = ServiciosNegocioRecursos.SinResultados
            };

            return ResponseMessage(respuestaHttp);
        }

        [Route("api/Focalizacion/EliminarPoliticasProyecto")]
        [SwaggerResponse(HttpStatusCode.OK, "Eliminar politica de un proyecto", typeof(HttpResponseMessage))]
        [HttpPost]
        public async Task<IHttpActionResult> EliminarPoliticasProyecto(int proyectoId, int politicaId)
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

                await Task.Run(() => _fuenteFinanciacionServicios.EliminarPoliticasProyecto(proyectoId, politicaId));
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

        [Route("api/Focalizacion/AgregarCategoriasPoliticaTransversalesAjustes")]
        [SwaggerResponse(HttpStatusCode.OK, "Informacion devuelta por guardado definitivo", typeof(HttpResponseMessage))]
        [HttpPost]
        public async Task<IHttpActionResult> AgregarCategoriasPoliticaTransversalesAjustes(FocalizacionCategoriasAjusteDto objIncluirPoliticasDto)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                    RequestContext.Principal.Identity.GetHashCode().ToString(), ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                    ConfigurationManager.AppSettings["postDatosAdicionalesAgregar"]).Result;
                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                var parametrosGuardar = new ParametrosGuardarDto<FocalizacionCategoriasAjusteDto>();
                parametrosGuardar.Contenido = objIncluirPoliticasDto;


                var parametrosAuditoria = new ParametrosAuditoriaDto
                {
                    Usuario = RequestContext.Principal.Identity.Name,
                    Ip = UtilidadesApi.GetClientIp(Request)
                };

                var result = await Task.Run(() => _fuenteFinanciacionServicios.GuardarCategoriasPoliticaTransversalesAjustes(parametrosGuardar, RequestContext.Principal.Identity.Name));

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

        [Route("api/Focalizacion/ObtenerPoliticasTransversalesResumen")]
        [SwaggerResponse(HttpStatusCode.OK, "Retorna string con listado de politicas transversales por proyecto.", typeof(HttpResponseMessage))]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerPoliticasTransversalesResumen(string BPIN)
        {
            var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                                                RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                                                ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                                                ConfigurationManager.AppSettings["ObtenerDatosAdicionalesFuente"]).Result;

            if (!respuestaAutorizacion.IsSuccessStatusCode)
                return ResponseMessage(respuestaAutorizacion);

            var result = await Task.Run(() => _fuenteFinanciacionServicios.ObtenerPoliticasTransversalesResumen(BPIN));
            if (result != null) return Ok(result);

            var respuestaHttp = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.NotFound,
                ReasonPhrase = ServiciosNegocioRecursos.SinResultados
            };

            return ResponseMessage(respuestaHttp);
        }


        //desarrollo pagina Indicadores Politicas Manuel
        [System.Web.Http.Route("api/Focalizacion/ConsultarPoliticasCategoriasIndicadores")]
        [SwaggerResponse(HttpStatusCode.OK, "Informacion Politicas indicadores", typeof(HttpResponseMessage))]
        [System.Web.Http.HttpGet]
        public async Task<IHttpActionResult> ConsultarPoliticasCategoriasIndicadores(string Bpin)
        {
            try
            {
                var result =
                   await Task.Run(() => _fuenteFinanciacionServicios.ObtenerPoliticasCategoriasIndicadores(Bpin));
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


        [Route("api/Focalizacion/ModificarCategoriasIndicadores")]
        [SwaggerResponse(HttpStatusCode.OK, "Inserta y elimina los indicadores", typeof(HttpResponseMessage))]
        [HttpPost]
        public async Task<IHttpActionResult> ModificarCategoriasIndicadores(CategoriasIndicadoresDto parametrosGuardar, string usuario)
        {
            try
            {
                var result = await Task.Run(() => _fuenteFinanciacionServicios.ModificarCategoriasIndicadores(parametrosGuardar, RequestContext.Principal.Identity.Name));

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

        [Route("api/Focalizacion/EliminarCategoriaPoliticasProyecto")]
        [SwaggerResponse(HttpStatusCode.OK, "Eliminar politica de un proyecto", typeof(HttpResponseMessage))]
        [HttpPost]
        public async Task<IHttpActionResult> EliminarCategoriasPoliticasProyecto(int proyectoId, int politicaId, int categoriaId)
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

                await Task.Run(() => _fuenteFinanciacionServicios.EliminarCategoriaPoliticasProyecto(proyectoId, politicaId, categoriaId));
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




        [Route("api/Focalizacion/ObtenerCrucePoliticasAjustes")]
        [SwaggerResponse(HttpStatusCode.OK, "Retorna string con listado de politicas transversales por proyecto.", typeof(HttpResponseMessage))]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerCrucePoliticasAjustes(string BPIN)
        {
            var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                                                RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                                                ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                                                ConfigurationManager.AppSettings["ObtenerDatosAdicionalesFuente"]).Result;

            if (!respuestaAutorizacion.IsSuccessStatusCode)
                return ResponseMessage(respuestaAutorizacion);

            var result = await Task.Run(() => _fuenteFinanciacionServicios.ObtenerCrucePoliticasAjustes(BPIN));
            if (result != null) return Ok(result);

            var respuestaHttp = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.NotFound,
                ReasonPhrase = ServiciosNegocioRecursos.SinResultados
            };

            return ResponseMessage(respuestaHttp);
        }

        [Route("api/Focalizacion/GuardarCrucePoliticasAjustes")]
        [SwaggerResponse(HttpStatusCode.OK, "Informacion devuelta por guardado definitivo", typeof(HttpResponseMessage))]
        [HttpPost]
        public async Task<RespuestaGeneralDto> GuardarCrucePoliticasAjustes(List<CrucePoliticasAjustesDto> objIncluirPoliticasDto)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                                                RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                                                ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                                                ConfigurationManager.AppSettings["ObtenerDatosAdicionalesFuente"]).Result;

                var result = new RespuestaGeneralDto();

                if (!respuestaAutorizacion.IsSuccessStatusCode)
                {
                    result.Exito = false;
                    result.Mensaje = respuestaAutorizacion.ToString();
                }
                //return ResponseMessage(respuestaAutorizacion);

                var parametrosGuardar = new ParametrosGuardarDto<List<CrucePoliticasAjustesDto>>();
                parametrosGuardar.Contenido = objIncluirPoliticasDto;


                var parametrosAuditoria = new ParametrosAuditoriaDto
                {
                    Usuario = RequestContext.Principal.Identity.Name,
                    Ip = UtilidadesApi.GetClientIp(Request)
                };

                result = await Task.Run(() => _fuenteFinanciacionServicios.GuardarCrucePoliticasAjustes(parametrosGuardar, RequestContext.Principal.Identity.Name));

                var respuesta = new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    ReasonPhrase = ServiciosNegocioRecursos.PostExitoso
                };

                return result;
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

        [Route("api/Focalizacion/ObtenerPoliticasSolicitudConcepto")]
        [SwaggerResponse(HttpStatusCode.OK, "Retorna string con listado de politicas transversales por proyecto.", typeof(HttpResponseMessage))]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerPoliticasSolicitudConcepto(string BPIN)
        {
            var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                                                RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                                                ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                                                ConfigurationManager.AppSettings["ObtenerDatosAdicionalesFuente"]).Result;

            if (!respuestaAutorizacion.IsSuccessStatusCode)
                return ResponseMessage(respuestaAutorizacion);

            var result = await Task.Run(() => _fuenteFinanciacionServicios.ObtenerPoliticasSolicitudConcepto(BPIN));
            if (result != null) return Ok(result);

            var respuestaHttp = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.NotFound,
                ReasonPhrase = ServiciosNegocioRecursos.SinResultados
            };

            return ResponseMessage(respuestaHttp);
        }

        [Route("api/Focalizacion/FocalizacionSolicitarConceptoDT")]
        [SwaggerResponse(HttpStatusCode.OK, "Informacion devuelta por guardado definitivo", typeof(HttpResponseMessage))]
        [HttpPost]
        public async Task<IHttpActionResult> FocalizacionSolicitarConceptoDT(List<FocalizacionSolicitarConceptoDto> objscDto)
        {
            try
            {
                var parametrosGuardar = new ParametrosGuardarDto<List<FocalizacionSolicitarConceptoDto>>();
                parametrosGuardar.Contenido = objscDto;


                var parametrosAuditoria = new ParametrosAuditoriaDto
                {
                    Usuario = RequestContext.Principal.Identity.Name,
                    Ip = UtilidadesApi.GetClientIp(Request)
                };

                var result = await Task.Run(() => _fuenteFinanciacionServicios.FocalizacionSolicitarConceptoDT(parametrosGuardar, RequestContext.Principal.Identity.Name));

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

        [Route("api/Focalizacion/ObtenerDireccionesTecnicasPoliticasFocalizacion")]
        [SwaggerResponse(HttpStatusCode.OK, "Retorna string con listado de politicas transversales por proyecto.", typeof(HttpResponseMessage))]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerDireccionesTecnicasPoliticasFocalizacion()
        {
            var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                                                RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                                                ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                                                ConfigurationManager.AppSettings["ObtenerDatosAdicionalesFuente"]).Result;

            if (!respuestaAutorizacion.IsSuccessStatusCode)
                return ResponseMessage(respuestaAutorizacion);

            var result = await Task.Run(() => _fuenteFinanciacionServicios.ObtenerDireccionesTecnicasPoliticasFocalizacion());
            if (result != null) return Ok(result);

            var respuestaHttp = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.NotFound,
                ReasonPhrase = ServiciosNegocioRecursos.SinResultados
            };

            return ResponseMessage(respuestaHttp);
        }

        [Route("api/Focalizacion/ObtenerResumenSolicitudConcepto")]
        [SwaggerResponse(HttpStatusCode.OK, "Retorna string con listado de politicas transversales por proyecto.", typeof(HttpResponseMessage))]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerResumenSolicitudConcepto(string BPIN)
        {
            var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                                                RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                                                ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                                                ConfigurationManager.AppSettings["ObtenerDatosAdicionalesFuente"]).Result;

            if (!respuestaAutorizacion.IsSuccessStatusCode)
                return ResponseMessage(respuestaAutorizacion);

            var result = await Task.Run(() => _fuenteFinanciacionServicios.ObtenerResumenSolicitudConcepto(BPIN));
            if (result != null) return Ok(result);

            var respuestaHttp = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.NotFound,
                ReasonPhrase = ServiciosNegocioRecursos.SinResultados
            };

            return ResponseMessage(respuestaHttp);
        }

        [Route("api/Focalizacion/ObtenerPreguntasEnvioPoliticaSubDireccion")]
        [SwaggerResponse(HttpStatusCode.OK, "Obtener Preguntas Envio Politica SubDireccion", typeof(HttpResponseMessage))]
        [HttpPost]
        public async Task<IHttpActionResult> ObtenerPreguntasEnvioPoliticaSubDireccion(PreguntasEnvioPoliticaSubDireccionDto PreguntasEnvioPoliticaSubDireccion)
        {
            var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                                                RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                                                ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                                                ConfigurationManager.AppSettings["ObtenerDatosAdicionalesFuente"]).Result;

            if (!respuestaAutorizacion.IsSuccessStatusCode)
                return ResponseMessage(respuestaAutorizacion);

            var result = await Task.Run(() => _fuenteFinanciacionServicios.ObtenerPreguntasEnvioPoliticaSubDireccion(PreguntasEnvioPoliticaSubDireccion.IdInstancia, PreguntasEnvioPoliticaSubDireccion.IdProyecto, PreguntasEnvioPoliticaSubDireccion.IdUsuarioDNP, PreguntasEnvioPoliticaSubDireccion.IdNivel));
            if (result != null) return Ok(result);

            var respuestaHttp = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.NotFound,
                ReasonPhrase = ServiciosNegocioRecursos.SinResultados
            };

            return ResponseMessage(respuestaHttp);
        }

        [Route("api/Focalizacion/GuardarPreguntasEnvioPoliticaSubDireccionAjustes")]
        [SwaggerResponse(HttpStatusCode.OK, "Informacion devuelta por guardado definitivo", typeof(HttpResponseMessage))]
        [HttpPost]
        public async Task<IHttpActionResult> GuardarPreguntasEnvioPoliticaSubDireccionAjustes(PreguntasEnvioPoliticaSubDireccionAjustes objscDto)
        {
            try
            {
                var parametrosGuardar = new ParametrosGuardarDto<PreguntasEnvioPoliticaSubDireccionAjustes>();
                parametrosGuardar.Contenido = objscDto;


                var parametrosAuditoria = new ParametrosAuditoriaDto
                {
                    Usuario = RequestContext.Principal.Identity.Name,
                    Ip = UtilidadesApi.GetClientIp(Request)
                };

                var result = await Task.Run(() => _fuenteFinanciacionServicios.GuardarPreguntasEnvioPoliticaSubDireccionAjustes(parametrosGuardar, RequestContext.Principal.Identity.Name));

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

        [Route("api/Focalizacion/GuardarRespuestaEnvioPoliticaSubDireccionAjustes")]
        [SwaggerResponse(HttpStatusCode.OK, "Informacion devuelta por guardado definitivo", typeof(HttpResponseMessage))]
        [HttpPost]
        public async Task<IHttpActionResult> GuardarRespuestaEnvioPoliticaSubDireccionAjustes(RespuestaEnvioPoliticaSubDireccionAjustes objscDto)
        {
            try
            {
                var parametrosGuardar = new ParametrosGuardarDto<RespuestaEnvioPoliticaSubDireccionAjustes>();
                parametrosGuardar.Contenido = objscDto;


                var parametrosAuditoria = new ParametrosAuditoriaDto
                {
                    Usuario = RequestContext.Principal.Identity.Name,
                    Ip = UtilidadesApi.GetClientIp(Request)
                };

                var result = await Task.Run(() => _fuenteFinanciacionServicios.GuardarRespuestaEnvioPoliticaSubDireccionAjustes(parametrosGuardar, RequestContext.Principal.Identity.Name));

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

    }
}