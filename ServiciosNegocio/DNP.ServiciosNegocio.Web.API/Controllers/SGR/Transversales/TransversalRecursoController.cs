using DNP.ServiciosNegocio.Comunes;
using DNP.ServiciosNegocio.Comunes.Autorizacion;
using DNP.ServiciosNegocio.Comunes.Dto;
using DNP.ServiciosNegocio.Comunes.Dto.Formulario;
using DNP.ServiciosNegocio.Comunes.Excepciones;
using DNP.ServiciosNegocio.Dominio.Dto.Focalizacion;
using DNP.ServiciosNegocio.Dominio.Dto.Genericos;
using DNP.ServiciosNegocio.Dominio.Dto.IndicadoresPolitica;
using DNP.ServiciosNegocio.Dominio.Dto.PoliticasTransversalesCrucePoliticas;
using DNP.ServiciosNegocio.Dominio.Dto.Preguntas;
using DNP.ServiciosNegocio.Dominio.Dto.Productos;
using DNP.ServiciosNegocio.Dominio.Dto.Proyectos;
using DNP.ServiciosNegocio.Dominio.Dto.SGR;
using DNP.ServiciosNegocio.Servicios.Interfaces.SGR.Transversales;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace DNP.ServiciosNegocio.Web.API.Controllers.SGR.Transversales
{
    public class TransversalRecursoController : ApiController
    {
        private readonly ITransversalRecursoServicio _transversalRecursoServicio;
        private readonly IAutorizacionUtilidades _autorizacionUtilidades;

        public TransversalRecursoController(ITransversalRecursoServicio transversalRecursoServicio, IAutorizacionUtilidades autorizacionUtilidades)
        {
            _transversalRecursoServicio = transversalRecursoServicio;
            _autorizacionUtilidades = autorizacionUtilidades;
        }

        [Route("SGR/Transversales/ObtenerDesagregarRegionalizacionSgr")]
        [SwaggerResponse(HttpStatusCode.OK, "Retorna Regionalizacion por bpin", typeof(DesagregarRegionalizacionDto))]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerDesagregarRegionalizacionSgr(string bpin)
        {

            ValidacionParametro(bpin);
            var result = await Task.Run(() => _transversalRecursoServicio.ObtenerDesagregarRegionalizacionSgr(bpin));
            return ResponderRegionaliza(result);
        }

        //[Route("SGR/Transversales/ObtenerDatosGeneralesProyectoSgr")]
        //[SwaggerResponse(HttpStatusCode.OK, "Retorna los datos del proyecto por id y nivel", typeof(DatosGeneralesProyectosDto))]
        //[HttpGet]
        //public async Task<IHttpActionResult> ObtenerDatosGeneralesProyectoSgr(int? pProyectoId, Guid pNivelId)
        //{

        //    var result = await Task.Run(() => _transversalRecursoServicio.ObtenerDatosGeneralesProyectoSgr(pProyectoId, pNivelId));

        //    if (result != null) return Ok(result);

        //    var respuestaHttp = new HttpResponseMessage()
        //    {
        //        StatusCode = HttpStatusCode.NotFound,
        //        ReasonPhrase = ServiciosNegocioRecursos.SinResultados
        //    };

        //    return ResponseMessage(respuestaHttp);
        //}

        [Route("SGR/Transversales/ObtenerFocalizacionPoliticasTransversalesFuentesSgr")]
        [SwaggerResponse(HttpStatusCode.OK, "Retorna la focalizacion en politicas transversales por bpin", typeof(FocalizacionPoliticaSgrDto))]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerFocalizacionPoliticasTransversalesFuentesSgr(string bpin)
        {
            ValidacionParametro(bpin);
            var result = await Task.Run(() => _transversalRecursoServicio.ObtenerFocalizacionPoliticasTransversalesFuentesSgr(bpin));
            return ResponderFocaliza(result);
        }

        [Route("SGR/Transversales/GuardarFocalizacionCategoriasAjustesSgr")]
        [SwaggerResponse(HttpStatusCode.OK, "Guarda información de focalizacion de politicas transversales", typeof(HttpResponseMessage))]
        [HttpPost]
        public async Task<IHttpActionResult> GuardarFocalizacionCategoriasAjustesSgr(List<FocalizacionCategoriasAjusteDto> focalizacionCategoriasAjuste)
        {
            try
            {
                var respuestaAutorizacion = await _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name, RequestContext.Principal.Identity.GetHashCode().ToString(), ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"], ConfigurationManager.AppSettings["GestionTramitesProyectos"]);
                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                var resultado = await Task.Run(() => _transversalRecursoServicio.GuardarFocalizacionCategoriasAjustesSgr(focalizacionCategoriasAjuste, RequestContext.Principal.Identity.Name));

                var respuesta = new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    ReasonPhrase = ServiciosNegocioRecursos.PostExitoso,
                };

                return Ok(resultado);
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

        //[Route("SGR/Transversales/ObtenerPoliticasTransversalesCrucePoliticasSgr")]
        //[SwaggerResponse(HttpStatusCode.OK, "Retorna listado de Politicas transversales con politicas que agregan informacion al proyecto por bpin y fuente de financiacion", typeof(PoliticasTCrucePoliticasDto))]
        //[HttpGet]
        //public async Task<IHttpActionResult> ObtenerPoliticasTransversalesCrucePoliticasSgr(string bpin, int IdFuente)
        //{
        //    var result = await Task.Run(() => _transversalRecursoServicio.ObtenerPoliticasTransversalesCrucePoliticasSgr(bpin, IdFuente));
        //    if (result != null) return Ok(result);

        //    var respuestaHttp = new HttpResponseMessage()
        //    {
        //        StatusCode = HttpStatusCode.NotFound,
        //        ReasonPhrase = ServiciosNegocioRecursos.SinResultados
        //    };

        //    return ResponseMessage(respuestaHttp);
        //}

        //[Route("SGR/Transversales/ObtenerDatosIndicadoresPoliticaSgr")]
        //[SwaggerResponse(HttpStatusCode.OK, "Retorna listado de indicadores por cada politica del proyecto", typeof(IndicadoresPoliticaDto))]
        //[HttpGet]
        //public async Task<IHttpActionResult> ObtenerDatosIndicadoresPoliticaSgr(string bpin)
        //{
        //    var result = await Task.Run(() => _transversalRecursoServicio.ObtenerDatosIndicadoresPoliticaSgr(bpin));
        //    if (result != null) return Ok(result);

        //    var respuestaHttp = new HttpResponseMessage()
        //    {
        //        StatusCode = HttpStatusCode.NotFound,
        //        ReasonPhrase = ServiciosNegocioRecursos.SinResultados
        //    };

        //    return ResponseMessage(respuestaHttp);
        //}

        //[Route("SGR/Transversales/ObtenerDatosCategoriaProductosPoliticaSgr")]
        //[SwaggerResponse(HttpStatusCode.OK, "Retorna listado de categoria de productos por politica.", typeof(IndicadoresPoliticaDto))]
        //[HttpGet]
        //public async Task<IHttpActionResult> ObtenerDatosCategoriaProductosPoliticaSgr(string bpin, int fuenteId, int politicaId)
        //{

        //    var result = await Task.Run(() => _transversalRecursoServicio.ObtenerDatosCategoriaProductosPoliticaSgr(bpin, fuenteId, politicaId));
        //    if (result != null) return Ok(result);

        //    var respuestaHttp = new System.Net.Http.HttpResponseMessage()
        //    {
        //        StatusCode = HttpStatusCode.NotFound,
        //        ReasonPhrase = ServiciosNegocioRecursos.SinResultados
        //    };

        //    return ResponseMessage(respuestaHttp);
        //}

        [Route("SGR/Transversales/ObtenerPoliticasTransversalesProyectoSgr")]
        [SwaggerResponse(HttpStatusCode.OK, "Retorna string con listado de politicas transversales por proyecto.", typeof(HttpResponseMessage))]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerPoliticasTransversalesProyectoSgr(string bpin)
        {
            //var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
            //                                                                    RequestContext.Principal.Identity.GetHashCode().ToString(),
            //                                                                    ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
            //                                                                    ConfigurationManager.AppSettings["ObtenerDatosAdicionalesFuente"]).Result;

            //if (!respuestaAutorizacion.IsSuccessStatusCode)
            //    return ResponseMessage(respuestaAutorizacion);

            var result = await Task.Run(() => _transversalRecursoServicio.ObtenerPoliticasTransversalesProyectoSgr(bpin));
            if (result != null) return Ok(result);

            var respuestaHttp = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.NotFound,
                ReasonPhrase = ServiciosNegocioRecursos.SinResultados
            };

            return ResponseMessage(respuestaHttp);
        }
        [Route("SGR/Transversales/EliminarPoliticasProyectoSgr")]
        [SwaggerResponse(HttpStatusCode.OK, "Eliminar politica de un proyecto", typeof(HttpResponseMessage))]
        [HttpPost]
        public async Task<IHttpActionResult> EliminarPoliticasProyectoSgr(int proyectoId, int politicaId)
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

                await Task.Run(() => _transversalRecursoServicio.EliminarPoliticasProyectoSgr(proyectoId, politicaId));
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
        [Route("SGR/Transversales/AgregarPoliticasTransversalesAjustesSgr")]
        [SwaggerResponse(HttpStatusCode.OK, "Informacion devuelta por guardado definitivo", typeof(HttpResponseMessage))]
        [HttpPost]
        public async Task<IHttpActionResult> AgregarPoliticasTransversalesAjustesSgr(IncluirPoliticasDto objIncluirPoliticasDto)
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

                var result = await Task.Run(() => _transversalRecursoServicio.AgregarPoliticasTransversalesAjustesSgr(parametrosGuardar, RequestContext.Principal.Identity.Name));

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
        [System.Web.Http.Route("SGR/Transversales/ConsultarPoliticasCategoriasIndicadoresSgr")]
        [SwaggerResponse(HttpStatusCode.OK, "Informacion Politicas indicadores", typeof(HttpResponseMessage))]
        [System.Web.Http.HttpGet]
        public async Task<IHttpActionResult> ConsultarPoliticasCategoriasIndicadoresSgr(Guid instanciaId)
        {
            try
            {
                var result =
                   await Task.Run(() => _transversalRecursoServicio.ConsultarPoliticasCategoriasIndicadoresSgr(instanciaId));
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
        [Route("SGR/Transversales/ObtenerPoliticasTransversalesCategoriasSgr")]
        [SwaggerResponse(HttpStatusCode.OK, "Retorna string con listado de politicas transversales por proyecto.", typeof(HttpResponseMessage))]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerPoliticasTransversalesCategoriasSgr(Guid instanciaId)
        {
            //var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
            //                                                                    RequestContext.Principal.Identity.GetHashCode().ToString(),
            //                                                                    ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
            //                                                                    ConfigurationManager.AppSettings["ObtenerDatosAdicionalesFuente"]).Result;

            //if (!respuestaAutorizacion.IsSuccessStatusCode)
            //    return ResponseMessage(respuestaAutorizacion);

            var result = await Task.Run(() => _transversalRecursoServicio.ObtenerPoliticasTransversalesCategoriasSgr(instanciaId));
            if (result != null) return Ok(result);

            var respuestaHttp = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.NotFound,
                ReasonPhrase = ServiciosNegocioRecursos.SinResultados
            };

            return ResponseMessage(respuestaHttp);
        }
        [Route("SGR/Transversales/EliminarCategoriasPoliticasProyectoSgr")]
        [SwaggerResponse(HttpStatusCode.OK, "Eliminar politica de un proyecto", typeof(HttpResponseMessage))]
        [HttpPost]
        public async Task<IHttpActionResult> EliminarCategoriasPoliticasProyectoSgr(int proyectoId, int politicaId, int categoriaId)
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

                await Task.Run(() => _transversalRecursoServicio.EliminarCategoriasPoliticasProyectoSgr(proyectoId, politicaId, categoriaId));
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

        [Route("SGR/Transversales/ModificarPoliticasCategoriasIndicadoresSgr")]
        [SwaggerResponse(HttpStatusCode.OK, "Inserta y elimina los indicadores", typeof(HttpResponseMessage))]
        [HttpPost]
        public async Task<IHttpActionResult> ModificarPoliticasCategoriasIndicadoresSgr(CategoriasIndicadoresDto parametrosGuardar, string usuario)
        {
            try
            {
                var result = await Task.Run(() => _transversalRecursoServicio.ModificarPoliticasCategoriasIndicadoresSgr(parametrosGuardar, RequestContext.Principal.Identity.Name));

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
        [Route("SGR/Transversales/ObtenerCrucePoliticasAjustesSgr")]
        [SwaggerResponse(HttpStatusCode.OK, "Retorna string con listado de politicas transversales por proyecto.", typeof(HttpResponseMessage))]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerCrucePoliticasAjustesSgr(Guid instanciaId)
        {
            var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                                                RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                                                ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                                                ConfigurationManager.AppSettings["SGR_Transversal_ObtenerCrucePoliticas"]).Result;

            if (!respuestaAutorizacion.IsSuccessStatusCode)
                return ResponseMessage(respuestaAutorizacion);

            var result = await Task.Run(() => _transversalRecursoServicio.ObtenerCrucePoliticasAjustesSgr(instanciaId));
            if (result != null) return Ok(result);

            var respuestaHttp = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.NotFound,
                ReasonPhrase = ServiciosNegocioRecursos.SinResultados
            };

            return ResponseMessage(respuestaHttp);
        }
        [Route("SGR/Transversales/GuardarCrucePoliticasAjustesSgr")]
        [SwaggerResponse(HttpStatusCode.OK, "Informacion devuelta por guardado definitivo", typeof(HttpResponseMessage))]
        [HttpPost]
        public async Task<IHttpActionResult> GuardarCrucePoliticasAjustesSgr(List<CrucePoliticasAjustesDto> objIncluirPoliticasDto)
        {
            try
            {
                var parametrosGuardar = new ParametrosGuardarDto<List<CrucePoliticasAjustesDto>>();
                parametrosGuardar.Contenido = objIncluirPoliticasDto;

                var parametrosAuditoria = new ParametrosAuditoriaDto
                {
                    Usuario = RequestContext.Principal.Identity.Name,
                    Ip = UtilidadesApi.GetClientIp(Request)
                };

                var result = await Task.Run(() => _transversalRecursoServicio.GuardarCrucePoliticasAjustesSgr(parametrosGuardar, RequestContext.Principal.Identity.Name));

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
        [Route("SGR/Transversales/ObtenerPoliticasTransversalesResumenSgr")]
        [SwaggerResponse(HttpStatusCode.OK, "Retorna string con listado de politicas transversales por proyecto.", typeof(HttpResponseMessage))]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerPoliticasTransversalesResumenSgr(Guid instanciaId)
        {
            //var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
            //                                                                    RequestContext.Principal.Identity.GetHashCode().ToString(),
            //                                                                    ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
            //                                                                    ConfigurationManager.AppSettings["ObtenerDatosAdicionalesFuente"]).Result;

            //if (!respuestaAutorizacion.IsSuccessStatusCode)
            //    return ResponseMessage(respuestaAutorizacion);

            var result = await Task.Run(() => _transversalRecursoServicio.ObtenerPoliticasTransversalesResumenSgr(instanciaId));
            if (result != null) return Ok(result);

            var respuestaHttp = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.NotFound,
                ReasonPhrase = ServiciosNegocioRecursos.SinResultados
            };

            return ResponseMessage(respuestaHttp);
        }


        private void ValidacionParametro(string bpin)
        {
            if (string.IsNullOrEmpty(bpin))
                throw new ServiciosNegocioHttpResponseException(HttpStatusCode.BadRequest, string.Format(ServiciosNegocioRecursos.ParametroNoRecibido, nameof(bpin)));            
        }
        private IHttpActionResult ResponderRegionaliza(DesagregarRegionalizacionDto result)
        {
            return result != null ? Ok(result) : CrearRespuestaNoFound();
        }
        private IHttpActionResult ResponderFocaliza(FocalizacionPoliticaSgrDto result)
        {
            return result != null ? Ok(result) : CrearRespuestaNoFound();
        }
        private IHttpActionResult CrearRespuestaNoFound()
        {
            var respuestaHttp = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.NotFound,
                ReasonPhrase = ServiciosNegocioRecursos.SinResultados
            };

            return ResponseMessage(respuestaHttp);
        }
    }
}