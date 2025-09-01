
using DNP.ServiciosNegocio.Servicios.Interfaces.SGP.GestionRecursos;
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
using DNP.ServiciosNegocio.Dominio.Dto.FuenteFinanciacion;

namespace DNP.ServiciosNegocio.Web.API.Controllers.SGP.GestionRecursos
{
    public class GestionRecursosSgpController : ApiController
    {
       
        private readonly IGestionRecursosSgpServicio _gestionRecursosSgpServicio;
        private readonly IAutorizacionUtilidades _autorizacionUtilidades;
       

        public GestionRecursosSgpController(IGestionRecursosSgpServicio gestionRecursosSgpServicio, IAutorizacionUtilidades autorizacionUtilidades)
        {
            _gestionRecursosSgpServicio = gestionRecursosSgpServicio;
            _autorizacionUtilidades = autorizacionUtilidades;
        }
       
        [Route("api/SGP/ObtenerProyectoListaLocalizacionesSGP")]
        [SwaggerResponse(HttpStatusCode.OK, "Informacion de la localización", typeof(LocalizacionProyectoDto))]
        [HttpGet]
        public async Task<IHttpActionResult> LocalizacionProyectoSgp(string bpin)
        {
            try
            {

                var result = await Task.Run(() => _gestionRecursosSgpServicio.ObtenerLocalizacionProyectosSgp(bpin));


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

        [Route("api/SGP/ObtenerFocalizacionPoliticasTransversalesFuentesSGP")]
        [SwaggerResponse(HttpStatusCode.OK, "Retorna la focalizacion en politicas transversales por bpin", typeof(FocalizacionPoliticaSgrDto))]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerFocalizacionPoliticasTransversalesFuentesSgp(string bpin)
        {
            ValidacionParametro(bpin);
            var result = await Task.Run(() => _gestionRecursosSgpServicio.ObtenerFocalizacionPoliticasTransversalesFuentesSgp(bpin));
            return ResponderFocaliza(result);
        }

        [Route("api/SGP/ObtenerPoliticasTransversalesProyectoSGP")]
        [SwaggerResponse(HttpStatusCode.OK, "Retorna string con listado de politicas transversales por proyecto.", typeof(HttpResponseMessage))]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerPoliticasTransversalesProyectoSgp(string bpin)
        {
            //var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
            //                                                                    RequestContext.Principal.Identity.GetHashCode().ToString(),
            //                                                                    ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
            //                                                                    ConfigurationManager.AppSettings["ObtenerDatosAdicionalesFuente"]).Result;

            //if (!respuestaAutorizacion.IsSuccessStatusCode)
            //    return ResponseMessage(respuestaAutorizacion);

            var result = await Task.Run(() => _gestionRecursosSgpServicio.ObtenerPoliticasTransversalesProyectoSgp(bpin));
            if (result != null) return Ok(result);

            var respuestaHttp = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.NotFound,
                ReasonPhrase = ServiciosNegocioRecursos.SinResultados
            };

            return ResponseMessage(respuestaHttp);
        }
        [Route("api/SGP/EliminarPoliticasProyectoSGP")]
        [SwaggerResponse(HttpStatusCode.OK, "Eliminar politica de un proyecto", typeof(HttpResponseMessage))]
        [HttpPost]
        public async Task<IHttpActionResult> EliminarPoliticasProyectoSgp(int proyectoId, int politicaId)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name, RequestContext.Principal.Identity.GetHashCode().ToString(), ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"], ConfigurationManager.AppSettings["EliminarPoliticasProyectoProgramacion"]).Result;
                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                var parametrosAuditoria = new ParametrosAuditoriaDto
                {
                    Usuario = RequestContext.Principal.Identity.Name,
                    Ip = UtilidadesApi.GetClientIp(Request)
                };
                
                _gestionRecursosSgpServicio.Usuario = RequestContext?.Principal?.Identity?.Name;
                _gestionRecursosSgpServicio.Ip = UtilidadesApi.GetClientIp(Request);

                await Task.Run(() => _gestionRecursosSgpServicio.EliminarPoliticasProyectoSgp(proyectoId, politicaId));
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

        [Route("api/SGP/AgregarPoliticasTransversalesAjustesSGP")]
        [SwaggerResponse(HttpStatusCode.OK, "Informacion devuelta al agregar una nueva politica en programacion", typeof(HttpResponseMessage))]
        [HttpPost]
        public async Task<IHttpActionResult> AgregarPoliticasTransversalesSgp(IncluirPoliticasDto objIncluirPoliticasDto)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                    RequestContext.Principal.Identity.GetHashCode().ToString(), ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                    ConfigurationManager.AppSettings["AgregarPoliticasTransversalesProgramacion"]).Result;
                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);
                string usuario = RequestContext.Principal.Identity.Name;

                var parametrosGuardar = new ParametrosGuardarDto<IncluirPoliticasDto>();
                parametrosGuardar.Contenido = objIncluirPoliticasDto;

                _gestionRecursosSgpServicio.Usuario = RequestContext?.Principal?.Identity?.Name;
                _gestionRecursosSgpServicio.Ip = UtilidadesApi.GetClientIp(Request);
                var result = await Task.Run(() => _gestionRecursosSgpServicio.AgregarPoliticasTransversalesSgp(parametrosGuardar, RequestContext.Principal.Identity.Name));

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

        [System.Web.Http.Route("api/SGP/ConsultarPoliticasCategoriasIndicadoresSGP")]
        [SwaggerResponse(HttpStatusCode.OK, "Informacion Politicas indicadores", typeof(HttpResponseMessage))]
        [System.Web.Http.HttpGet]
        public async Task<IHttpActionResult> ConsultarPoliticasCategoriasIndicadoresSgp(Guid instanciaId)
        {
            try
            {
                var result =
                   await Task.Run(() => _gestionRecursosSgpServicio.ConsultarPoliticasCategoriasIndicadoresSgp(instanciaId));
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


        [Route("api/SGP/ModificarPoliticasCategoriasIndicadoresSGP")]
        [SwaggerResponse(HttpStatusCode.OK, "Inserta y elimina los indicadores", typeof(HttpResponseMessage))]
        [HttpPost]
        public async Task<IHttpActionResult> ModificarPoliticasCategoriasIndicadoresSgp(CategoriasIndicadoresDto parametrosGuardar, string usuario)
        {
            try
            {
                _gestionRecursosSgpServicio.Usuario = RequestContext?.Principal?.Identity?.Name;
                _gestionRecursosSgpServicio.Ip = UtilidadesApi.GetClientIp(Request);
                var result = await Task.Run(() => _gestionRecursosSgpServicio.ModificarPoliticasCategoriasIndicadoresSgp(parametrosGuardar, RequestContext.Principal.Identity.Name));

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

        [Route("api/SGP/ObtenerPoliticasTransversalesCategoriasSGP")]
        [SwaggerResponse(HttpStatusCode.OK, "Retorna string con listado de politicas transversales por proyecto.", typeof(HttpResponseMessage))]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerPoliticasTransversalesCategoriasSgp(string instanciaId)
        {
            //var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
            //                                                                    RequestContext.Principal.Identity.GetHashCode().ToString(),
            //                                                                    ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
            //                                                                    ConfigurationManager.AppSettings["ObtenerDatosAdicionalesFuente"]).Result;

            //if (!respuestaAutorizacion.IsSuccessStatusCode)
            //    return ResponseMessage(respuestaAutorizacion);

            var result = await Task.Run(() => _gestionRecursosSgpServicio.ObtenerPoliticasTransversalesCategoriasSgp(instanciaId));
            if (result != null) return Ok(result);

            var respuestaHttp = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.NotFound,
                ReasonPhrase = ServiciosNegocioRecursos.SinResultados
            };

            return ResponseMessage(respuestaHttp);
        }

        [Route("api/SGP/EliminarCategoriasPoliticasProyectoSGP")]
        [SwaggerResponse(HttpStatusCode.OK, "Eliminar politica de un proyecto", typeof(HttpResponseMessage))]
        [HttpPost]
        public async Task<IHttpActionResult> EliminarCategoriasPoliticasProyectoSgp(int proyectoId, int politicaId, int categoriaId)
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

                _gestionRecursosSgpServicio.Usuario = RequestContext?.Principal?.Identity?.Name;
                _gestionRecursosSgpServicio.Ip = UtilidadesApi.GetClientIp(Request);

                await Task.Run(() => _gestionRecursosSgpServicio.EliminarCategoriasPoliticasProyectoSgp(proyectoId, politicaId, categoriaId));
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

        [Route("api/SGP/GuardarFocalizacionCategoriasAjustesSGP")]
        [SwaggerResponse(HttpStatusCode.OK, "Guarda información de focalizacion de politicas transversales", typeof(HttpResponseMessage))]
        [HttpPost]
        public async Task<IHttpActionResult> GuardarFocalizacionCategoriasAjustesSgp(List<FocalizacionCategoriasAjusteDto> focalizacionCategoriasAjuste)
        {
            try
            {
                var respuestaAutorizacion = await _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name, RequestContext.Principal.Identity.GetHashCode().ToString(), ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"], ConfigurationManager.AppSettings["GestionTramitesProyectos"]);
                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);


                _gestionRecursosSgpServicio.Usuario = RequestContext?.Principal?.Identity?.Name;
                _gestionRecursosSgpServicio.Ip = UtilidadesApi.GetClientIp(Request);

                var resultado = await Task.Run(() => _gestionRecursosSgpServicio.GuardarFocalizacionCategoriasAjustesSgp(focalizacionCategoriasAjuste, RequestContext.Principal.Identity.Name));

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


        [Route("api/SGP/ObtenerCategoriasSubcategoriasPorPadreSGP")]
        [SwaggerResponse(HttpStatusCode.OK, "Obtener Categorias y Subcategorias por padre", typeof(HttpResponseMessage))]
        [HttpGet]
        public async Task<IHttpActionResult> GetCategoriasSubcategoriasSgp(int padreId, Nullable<int> entidadId, int esCategoria, int esGruposEtnicos)
        {
            var result = await Task.Run(() => _gestionRecursosSgpServicio.GetCategoriasSubcategoriasSgp(padreId, entidadId, esCategoria, esGruposEtnicos));

            return Ok(result);
        }


        [Route("api/SGP/AgregarCategoriasPoliticaTransversalesAjustesSGP")]
        [SwaggerResponse(HttpStatusCode.OK, "Informacion devuelta por guardado definitivo", typeof(HttpResponseMessage))]
        [HttpPost]
        public async Task<IHttpActionResult> AgregarCategoriasPoliticaTransversalesAjustesSgp(FocalizacionCategoriasAjusteDto objIncluirPoliticasDto)
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

                _gestionRecursosSgpServicio.Usuario = RequestContext?.Principal?.Identity?.Name;
                _gestionRecursosSgpServicio.Ip = UtilidadesApi.GetClientIp(Request);

                var result = await Task.Run(() => _gestionRecursosSgpServicio.GuardarCategoriasPoliticaTransversalesAjustesSgp(parametrosGuardar, RequestContext.Principal.Identity.Name));

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

        [Route("api/SGP/ObtenerCrucePoliticasAjustesSGP")]
        [SwaggerResponse(HttpStatusCode.OK, "Retorna string con listado de politicas transversales por proyecto.", typeof(HttpResponseMessage))]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerCrucePoliticasAjustesSgp(Guid instanciaId)
        {
            //var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
            //                                                                    RequestContext.Principal.Identity.GetHashCode().ToString(),
            //                                                                    ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
            //                                                                    ConfigurationManager.AppSettings["SGR_Transversal_ObtenerCrucePoliticas"]).Result;

            //if (!respuestaAutorizacion.IsSuccessStatusCode)
            //    return ResponseMessage(respuestaAutorizacion);

            var result = await Task.Run(() => _gestionRecursosSgpServicio.ObtenerCrucePoliticasAjustesSgp(instanciaId));
            if (result != null) return Ok(result);

            var respuestaHttp = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.NotFound,
                ReasonPhrase = ServiciosNegocioRecursos.SinResultados
            };

            return ResponseMessage(respuestaHttp);
        }
        [Route("api/SGP/ObtenerPoliticasTransversalesResumenSGP")]
        [SwaggerResponse(HttpStatusCode.OK, "Retorna string con listado de politicas transversales por proyecto.", typeof(HttpResponseMessage))]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerPoliticasTransversalesResumenSgp(Guid instanciaId)
        {
            //var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
            //                                                                    RequestContext.Principal.Identity.GetHashCode().ToString(),
            //                                                                    ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
            //                                                                    ConfigurationManager.AppSettings["ObtenerDatosAdicionalesFuente"]).Result;

            //if (!respuestaAutorizacion.IsSuccessStatusCode)
            //    return ResponseMessage(respuestaAutorizacion);

            var result = await Task.Run(() => _gestionRecursosSgpServicio.ObtenerPoliticasTransversalesResumenSgp(instanciaId));
            if (result != null) return Ok(result);

            var respuestaHttp = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.NotFound,
                ReasonPhrase = ServiciosNegocioRecursos.SinResultados
            };

            return ResponseMessage(respuestaHttp);
        }

        [Route("api/SGP/GuardarCrucePoliticasAjustesSGP")]
        [SwaggerResponse(HttpStatusCode.OK, "Informacion devuelta por guardado definitivo", typeof(HttpResponseMessage))]
        [HttpPost]
        public async Task<IHttpActionResult> GuardarCrucePoliticasAjustesSgp(List<CrucePoliticasAjustesDto> objIncluirPoliticasDto)
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
                _gestionRecursosSgpServicio.Usuario = RequestContext?.Principal?.Identity?.Name;
                _gestionRecursosSgpServicio.Ip = UtilidadesApi.GetClientIp(Request);
                var result = await Task.Run(() => _gestionRecursosSgpServicio.GuardarCrucePoliticasAjustesSgp(parametrosGuardar, RequestContext.Principal.Identity.Name));

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

        [Route("api/SGP/ObtenerDesagregarRegionalizacionSGP")]
        [SwaggerResponse(HttpStatusCode.OK, "Retorna Regionalizacion por bpin", typeof(HttpResponseMessage))]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerDesagregarRegionalizacionSgp(string bpin)
        {

            ValidacionParametro(bpin);
            var result = await Task.Run(() => _gestionRecursosSgpServicio.ObtenerDesagregarRegionalizacionSgp(bpin));
            return ResponderRegionaliza(result);
        }

        [Route("api/SGP/ConsultarFuenteFinanciacionVigenciaSGP")]
        [SwaggerResponse(HttpStatusCode.OK, "Retorna listado de Fuentes de Financiacion, cofinanciador, vigencia", typeof(ProyectoFuenteFinanciacionAgregarDto))]
        [HttpGet]
        public async Task<IHttpActionResult> ConsultarFuenteFinanciacionVigenciaSgp(string bpin)
        {
            var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                                               RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                                               ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                                               ConfigurationManager.AppSettings["consultarFuentesFinanciacionAgregarN"]).Result;

            if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

            var result = await Task.Run(() => _gestionRecursosSgpServicio.ObtenerFuenteFinanciacionVigenciaSgp(bpin));
            if (result != null) return Ok(result);

            var respuestaHttp = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.NotFound,
                ReasonPhrase = ServiciosNegocioRecursos.SinResultados
            };

            return ResponseMessage(respuestaHttp);
        }

        [Route("api/SGP/ConsultarFuentesProgramarSolicitadoSGP")]
        [SwaggerResponse(HttpStatusCode.OK, "Retorna listado de los cofinanciadores etapa y vigencia", typeof(ProyectoFuenteFinanciacionAgregarDto))]
        [HttpGet]
        public async Task<IHttpActionResult> ConsultarFuentesProgramarSolicitadoSgp(string bpin)
        {
            var result = await Task.Run(() => _gestionRecursosSgpServicio.ObtenerFuentesProgramarSolicitadoSgp(bpin));
            if (result != null) return Ok(result);

            var respuestaHttp = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.NotFound,
                ReasonPhrase = ServiciosNegocioRecursos.SinResultados
            };

            return ResponseMessage(respuestaHttp);
        }


        [Route("api/SGP/EliminarFuenteFinanciacionSGP")]
        [SwaggerResponse(HttpStatusCode.OK, "Eliminar la fuente de Financiacion", typeof(HttpResponseMessage))]
        [HttpPost]
        public async Task<IHttpActionResult> EliminarFuentesFinanciacionProyectoSGP(int fuenteFinanciacionId)
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

                _gestionRecursosSgpServicio.Usuario = RequestContext?.Principal?.Identity?.Name;
                _gestionRecursosSgpServicio.Ip = UtilidadesApi.GetClientIp(Request);
                await Task.Run(() => _gestionRecursosSgpServicio.EliminarFuentesFinanciacionProyectoSgp(fuenteFinanciacionId));
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

        [Route("api/SGP/GuardarFuentesProgramarSolicitadoSGP")]
        [SwaggerResponse(HttpStatusCode.OK, "Guardar Fuentes Programar Solicitado", typeof(ProgramacionValorFuenteDto))]
        [HttpPost]
        public async Task<IHttpActionResult> GuardarFuentesProgramarSolicitadoSgp(ProgramacionValorFuenteDto objProgramacionValorFuenteDto, string usuario)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name, RequestContext.Principal.Identity.GetHashCode().ToString(), ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"], ConfigurationManager.AppSettings["postFuentesFinanciacionAgregar"]).Result;
                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                _gestionRecursosSgpServicio.Usuario = RequestContext?.Principal?.Identity?.Name;
                _gestionRecursosSgpServicio.Ip = UtilidadesApi.GetClientIp(Request);

                var resp = await Task.Run(() => _gestionRecursosSgpServicio.GuardarFuentesProgramarSolicitadoSgp(objProgramacionValorFuenteDto, RequestContext.Principal.Identity.Name));

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

        [Route("api/SGP/ConsultarDatosAdicionalesSGP")]
        [SwaggerResponse(HttpStatusCode.OK, "Retorna listado con los Datos Adicionales de una Fuentes de Financiacion", typeof(DatosAdicionalesDto))]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerDatosAdicionalesFuenteFinanciacionSgp(int fuenteId)
        {
            var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                                               RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                                               ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                                               ConfigurationManager.AppSettings["ObtenerDatosAdicionalesFuente"]).Result;

            if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

            var result = await Task.Run(() => _gestionRecursosSgpServicio.ObtenerDatosAdicionalesFuenteFinanciacionSgp(fuenteId));
            if (result != null) return Ok(result);

            var respuestaHttp = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.NotFound,
                ReasonPhrase = ServiciosNegocioRecursos.SinResultados
            };

            return ResponseMessage(respuestaHttp);
        }

        [Route("api/SGP/AgregarDatosAdicionalesSGP")]
        [SwaggerResponse(HttpStatusCode.OK, "Informacion devuelta por guardado definitivo", typeof(HttpResponseMessage))]
        [HttpPost]
        public async Task<IHttpActionResult> GuardarDatosAdicionalesSgp(DatosAdicionalesDto objDatosAdicionalesDto)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                    RequestContext.Principal.Identity.GetHashCode().ToString(), ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                    ConfigurationManager.AppSettings["postDatosAdicionalesAgregar"]).Result;
                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                var parametrosGuardar = new ParametrosGuardarDto<DatosAdicionalesDto>();
                parametrosGuardar.Contenido = objDatosAdicionalesDto;


                var parametrosAuditoria = new ParametrosAuditoriaDto
                {
                    Usuario = RequestContext.Principal.Identity.Name,
                    Ip = UtilidadesApi.GetClientIp(Request)
                };

                _gestionRecursosSgpServicio.Usuario = RequestContext?.Principal?.Identity?.Name;
                _gestionRecursosSgpServicio.Ip = UtilidadesApi.GetClientIp(Request);
                var result = await Task.Run(() => _gestionRecursosSgpServicio.GuardarDatosAdicionalesSgp(parametrosGuardar, RequestContext.Principal.Identity.Name));

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


        [Route("api/SGP/EliminarDatosAdicionalesSGP")]
        [SwaggerResponse(HttpStatusCode.OK, "Eliminar Datos adicionales de la fuente de Financiacion", typeof(HttpResponseMessage))]
        [HttpPost]
        public async Task<IHttpActionResult> EliminarDatosAdicionalesSgp(int coFinanciadorId)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                    RequestContext.Principal.Identity.GetHashCode().ToString(), ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                    ConfigurationManager.AppSettings["postDatosAdicionalesEliminar"]).Result;
                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                var parametrosAuditoria = new ParametrosAuditoriaDto
                {
                    Usuario = RequestContext.Principal.Identity.Name,
                    Ip = UtilidadesApi.GetClientIp(Request)
                };
                
                _gestionRecursosSgpServicio.Usuario = RequestContext?.Principal?.Identity?.Name;
                _gestionRecursosSgpServicio.Ip = UtilidadesApi.GetClientIp(Request);

                await Task.Run(() => _gestionRecursosSgpServicio.EliminarDatosAdicionalesSgp(coFinanciadorId));

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

        [Route("api/SGP/FuenteFinanciacionAgregarSGP")]
        [SwaggerResponse(HttpStatusCode.OK, "Informacion devuelta por guardado definitivo", typeof(HttpResponseMessage))]
        [HttpPost]
        public async Task<IHttpActionResult> GuardarFuenteFinanciacionSgp(ProyectoFuenteFinanciacionAgregarDto proyectoFuenteFinanciacionAgregarDto)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name, RequestContext.Principal.Identity.GetHashCode().ToString(), ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"], ConfigurationManager.AppSettings["postFuentesFinanciacionAgregar"]).Result;
                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);
                var parametrosGuardar = new ParametrosGuardarDto<ProyectoFuenteFinanciacionAgregarDto>();
                parametrosGuardar.Contenido = proyectoFuenteFinanciacionAgregarDto;

                _gestionRecursosSgpServicio.Usuario = RequestContext?.Principal?.Identity?.Name;
                _gestionRecursosSgpServicio.Ip = UtilidadesApi.GetClientIp(Request);

                var result = await Task.Run(() => _gestionRecursosSgpServicio.GuardarFuenteFinanciacionSgp(parametrosGuardar, RequestContext.Principal.Identity.Name));

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

        [Route("api/SGP/ConsultarIndicadorPoliticaSGP")]
        [SwaggerResponse(HttpStatusCode.OK, "Retorna listado de indicadores por politica.", typeof(IndicadoresPoliticaDto))]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerDatosIndicadoresPoliticaSgp(string BPIN)
        {
            try
            {
                //var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name, 
                //                                                                   RequestContext.Principal.Identity.GetHashCode().ToString(), 
                //                                                                   ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"], 
                //                                                                   ConfigurationManager.AppSettings["ObtenerDatosAdicionalesFuente"]).Result;
                //if (!respuestaAutorizacion.IsSuccessStatusCode)
                //    return ResponseMessage(respuestaAutorizacion);

                var result = await Task.Run(() => _gestionRecursosSgpServicio.ObtenerDatosIndicadoresPoliticaSgp(BPIN));
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

        [Route("api/SGP/ConsultarProductosPoliticaSGP")]
        [SwaggerResponse(HttpStatusCode.OK, "Retorna listado de categoria de productos por politica.", typeof(IndicadoresPoliticaDto))]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerDatosCategoriaProductosPoliticaSgp(string BPIN, int fuenteId, int politicaId)
        {
            try
            {
                //var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                //                                                                   RequestContext.Principal.Identity.GetHashCode().ToString(),
                //                                                                   ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                //                                                                   ConfigurationManager.AppSettings["ObtenerDatosAdicionalesFuente"]).Result;

                //if (!respuestaAutorizacion.IsSuccessStatusCode)
                //    return ResponseMessage(respuestaAutorizacion);

                var result = await Task.Run(() => _gestionRecursosSgpServicio.ObtenerDatosCategoriaProductosPoliticaSgp(BPIN, fuenteId, politicaId));
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

        [Route("api/SGP/GuardarDatosSolicitudRecursosSGP")]
        [SwaggerResponse(HttpStatusCode.OK, "Retorna listado de categoria de productos por politica.", typeof(IndicadoresPoliticaDto))]
        [HttpPost]
        public async Task<IHttpActionResult> GuardarDatosSolicitudRecursosSGP(CategoriaProductoPoliticaDto categoriaProductoPoliticaDto)
        {
            try
            {
                //var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                //                                                                   RequestContext.Principal.Identity.GetHashCode().ToString(),
                //                                                                   ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                //                                                                   ConfigurationManager.AppSettings["ObtenerDatosAdicionalesFuente"]).Result;

                //if (!respuestaAutorizacion.IsSuccessStatusCode)
                //    return ResponseMessage(respuestaAutorizacion);

                var parametrosGuardar = new ParametrosGuardarDto<CategoriaProductoPoliticaDto>();
                parametrosGuardar.Contenido = categoriaProductoPoliticaDto;

                var parametrosAuditoria = new ParametrosAuditoriaDto
                {
                    Usuario = RequestContext.Principal.Identity.Name,
                    Ip = UtilidadesApi.GetClientIp(Request)
                };

                _gestionRecursosSgpServicio.Usuario = RequestContext?.Principal?.Identity?.Name;
                _gestionRecursosSgpServicio.Ip = UtilidadesApi.GetClientIp(Request);
                var result = await Task.Run(() => _gestionRecursosSgpServicio.GuardarDatosSolicitudRecursosSgp(parametrosGuardar, parametrosAuditoria.Usuario));

                if (result != null) return Ok(result);

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

        private IHttpActionResult ResponderFocaliza(string result)
        {
            return result != null ? Ok(result) : CrearRespuestaNoFound();
        }

        private IHttpActionResult ResponderRegionaliza(string result)
        {
            return result != null ? Ok(result) : CrearRespuestaNoFound();
        }
        private void ValidacionParametro(string bpin)
        {
            if (string.IsNullOrEmpty(bpin))
                throw new ServiciosNegocioHttpResponseException(HttpStatusCode.BadRequest, string.Format(ServiciosNegocioRecursos.ParametroNoRecibido, nameof(bpin)));
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