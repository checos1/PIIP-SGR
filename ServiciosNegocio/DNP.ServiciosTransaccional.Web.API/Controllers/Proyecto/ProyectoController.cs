namespace DNP.ServiciosTransaccional.Web.API.Controllers.Proyecto
{
    using DNP.ServiciosNegocio.Comunes.Enum;
    using DNP.ServiciosTransaccional.Servicios.Dto;
    using DNP.ServiciosTransaccional.Servicios.Interfaces;
    using DNP.ServiciosTransaccional.Servicios.Interfaces.Fichas;
    using Microsoft.SqlServer.Server;
    using Servicios.Interfaces.Proyectos;
    using ServiciosNegocio.Comunes;
    using ServiciosNegocio.Comunes.Autorizacion;
    using ServiciosNegocio.Comunes.Dto;
    using ServiciosNegocio.Comunes.Dto.Formulario;
    using ServiciosNegocio.Comunes.Excepciones;
    using ServiciosNegocio.Comunes.Utilidades;
    using ServiciosNegocio.Dominio.Dto.Transferencias;
    using System;
    using System.Collections.ObjectModel;
    using System.Configuration;
    using System.Diagnostics.CodeAnalysis;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web.Helpers;
    using System.Web.Http;

    public class ProyectoController : ApiController
    {
        private readonly IAutorizacionUtilidades _autorizacionUtilidades;
        private readonly IProyectoServicio _proyectoServicio;

        public ProyectoController(IProyectoServicio proyectoServicio, IAutorizacionUtilidades autorizacionUtilidades)
        {
            _proyectoServicio = proyectoServicio;
            _autorizacionUtilidades = autorizacionUtilidades;
        }

        [Route("api/Proyecto/ActualizarEstado")]
        [HttpPost]
        public async Task<IHttpActionResult> ActualizarEstado(ObjetoNegocio contenido)
        {
            try
            {
                var respuestaAutorizacion = await _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                           RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                           ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                           ConfigurationManager.AppSettings["actualizarEstadoProyecto"]);

                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                if (contenido == null)
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NoContent, new Exception(string.Format(ServiciosNegocioRecursos.ParametrosNoRecibidos))));

                ValidarParametros(contenido);

                var parametrosActualizar = new ParametrosGuardarDto<ObjetoNegocio>
                {
                    Contenido = contenido
                };
                var parametrosAuditoria = new ParametrosAuditoriaDto
                {
                    Usuario = RequestContext.Principal.Identity.Name,
                    Ip = UtilidadesApi.GetClientIp(Request)
                };
                var response = await Task.Run(() => _proyectoServicio.ActualizarEstado(parametrosActualizar, parametrosAuditoria));

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

        [Route("api/SGR/Proyecto/ActualizarEstado")]
        [HttpPost]
        public async Task<IHttpActionResult> ActualizarEstadoSGR(ObjetoNegocio contenido)
        {
            try
            {
                var respuestaAutorizacion = await _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                           RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                           ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                           ConfigurationManager.AppSettings["actualizarEstadoProyecto"]);

                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                if (contenido == null)
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NoContent, new Exception(string.Format(ServiciosNegocioRecursos.ParametrosNoRecibidos))));

                ValidarParametros(contenido);

                var parametrosActualizar = new ParametrosGuardarDto<ObjetoNegocio>
                {
                    Contenido = contenido
                };
                var parametrosAuditoria = new ParametrosAuditoriaDto
                {
                    Usuario = RequestContext.Principal.Identity.Name,
                    Ip = UtilidadesApi.GetClientIp(Request)
                };
                var response = await Task.Run(() => _proyectoServicio.ActualizarEstadoSGR(parametrosActualizar, parametrosAuditoria));

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

        [Route("api/SGR/Proyecto/IniciarFlujo")]
        [HttpPost]
        public async Task<IHttpActionResult> IniciarFlujoSGR(ObjetoNegocio contenido)
        {
            try
            {
                var respuestaAutorizacion = await _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                           RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                           ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                           ConfigurationManager.AppSettings["actualizarEstadoProyecto"]);

                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                if (contenido == null)
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NoContent, new Exception(string.Format(ServiciosNegocioRecursos.ParametrosNoRecibidos))));

                ValidarParametros(contenido);

                var parametrosActualizar = new ParametrosGuardarDto<ObjetoNegocio>
                {
                    Contenido = contenido
                };
                var parametrosAuditoria = new ParametrosAuditoriaDto
                {
                    Usuario = RequestContext.Principal.Identity.Name,
                    Ip = UtilidadesApi.GetClientIp(Request)
                };
                var response = await Task.Run(() => _proyectoServicio.IniciarFlujoSGR(parametrosActualizar, parametrosAuditoria));

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

        [SuppressMessage("ReSharper", "UnusedVariable")]
        private static void ValidarParametros(ObjetoNegocio datosNegocio)
        {
            var ObjetoNegocioId = datosNegocio.ObjetoNegocioId;
            var NivelId = datosNegocio.NivelId;

            if (string.IsNullOrWhiteSpace(ObjetoNegocioId) && string.IsNullOrWhiteSpace(NivelId))
                throw new ServiciosNegocioException(string.Format(ServiciosNegocioRecursos.ParametrosNoRecibidos));

            if (string.IsNullOrWhiteSpace(ObjetoNegocioId))
                throw new ServiciosNegocioException(string.Format(ServiciosNegocioRecursos.ParametroNoRecibido, nameof(ObjetoNegocioId)));

            long negocioIdResult;
            if (!long.TryParse(ObjetoNegocioId, out negocioIdResult) || ObjetoNegocioId.Length > 100)
                throw new ServiciosNegocioException(string.Format(ServiciosNegocioRecursos.ParametroNoValidos, nameof(ObjetoNegocioId)));

            if (string.IsNullOrWhiteSpace(NivelId))
                throw new ServiciosNegocioException(string.Format(ServiciosNegocioRecursos.ParametroNoRecibido, nameof(NivelId)));

            Guid nivelIdResult;
            if (!Guid.TryParse(NivelId, out nivelIdResult))
                throw new ServiciosNegocioException(string.Format(ServiciosNegocioRecursos.ParametroNoValidos, nameof(NivelId)));

        }

        [Route("api/Proyecto/ActualizarNombre")]
        [HttpPost]
        public async Task<IHttpActionResult> ActualizarNombre(ObjetoNegocio contenido)
        {
            try
            {
                var respuestaAutorizacion = await _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                           RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                           ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                           ConfigurationManager.AppSettings["actualizarEstadoProyecto"]);

                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                if (contenido == null)
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NoContent, new Exception(string.Format(ServiciosNegocioRecursos.ParametrosNoRecibidos))));

                //ValidarParametros(contenido);
                if (string.IsNullOrWhiteSpace(contenido.ObjetoNegocioId))
                    throw new ServiciosNegocioException(string.Format(ServiciosNegocioRecursos.ParametroNoValidos, nameof(contenido.ObjetoNegocioId)));

                var parametrosActualizar = new ParametrosGuardarDto<ObjetoNegocio>
                {
                    Contenido = contenido
                };
                var parametrosAuditoria = new ParametrosAuditoriaDto
                {
                    Usuario = RequestContext.Principal.Identity.Name,
                    Ip = UtilidadesApi.GetClientIp(Request)
                };
                var response = await Task.Run(() => _proyectoServicio.ActualizarNombre(parametrosActualizar, parametrosAuditoria));

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

        [Route("api/Proyecto/AplicarFlujoSGR")]
        [HttpPost]
        public async Task<IHttpActionResult> AplicarFlujoSGR(ObjetoNegocio contenido)
        {
            try
            {
                var respuestaAutorizacion = await _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                           RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                           ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                           ConfigurationManager.AppSettings["actualizarEstadoProyecto"]);

                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                if (contenido == null)
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NoContent, new Exception(string.Format(ServiciosNegocioRecursos.ParametrosNoRecibidos))));

                var parametrosActualizar = new ParametrosGuardarDto<ObjetoNegocio>
                {
                    Contenido = contenido
                };
                var parametrosAuditoria = new ParametrosAuditoriaDto
                {
                    Usuario = RequestContext.Principal.Identity.Name,
                    Ip = UtilidadesApi.GetClientIp(Request)
                };

                await Task.Run(() => _proyectoServicio.GenerarFichaViabilidadSGR(parametrosActualizar, parametrosAuditoria));

                await Task.Run(() => _proyectoServicio.SGR_Proyectos_NotificarUsuariosViabilidad(Guid.Parse(parametrosActualizar.Contenido.InstanciaId), parametrosActualizar.Contenido.ObjetoNegocioId, RequestContext.Principal.Identity.Name));

                var response = await Task.Run(() => _proyectoServicio.SGR_Proyectos_GenerarMensajeEstadoProyecto(Guid.Parse(parametrosActualizar.Contenido.InstanciaId), RequestContext.Principal.Identity.Name));

                await Task.Run(() => _proyectoServicio.SGR_Proyectos_PostAplicarFlujoSGR(parametrosActualizar.Contenido.FlujoId, parametrosActualizar.Contenido.ObjetoNegocioId, Guid.Parse(parametrosActualizar.Contenido.InstanciaId), RequestContext.Principal.Identity.Name));

                var respuesta = new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    ReasonPhrase = response
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

        [Route("api/Proyecto/DevolverFlujoSGR")]
        [HttpPost]
        public async Task<IHttpActionResult> DevolverFlujoSGR(ObjetoNegocio contenido)
        {
            try
            {
                var respuestaAutorizacion = await _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                           RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                           ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                           ConfigurationManager.AppSettings["actualizarEstadoProyecto"]);

                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                if (contenido == null)
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NoContent, new Exception(string.Format(ServiciosNegocioRecursos.ParametrosNoRecibidos))));

                //ValidarParametros(contenido);

                var parametrosActualizar = new ParametrosGuardarDto<ObjetoNegocio>
                {
                    Contenido = contenido
                };
                var parametrosAuditoria = new ParametrosAuditoriaDto
                {
                    Usuario = RequestContext.Principal.Identity.Name,
                    Ip = UtilidadesApi.GetClientIp(Request)
                };

                await Task.Run(() => _proyectoServicio.SGR_Proyectos_NotificarUsuariosViabilidad(Guid.Parse(parametrosActualizar.Contenido.InstanciaId), parametrosActualizar.Contenido.ObjetoNegocioId, RequestContext.Principal.Identity.Name));

                //var response = await Task.Run(() => _proyectoServicio.GenerarFichaViabilidadSGR(parametrosActualizar, parametrosAuditoria));

                await Task.Run(() => _proyectoServicio.SGR_Proyectos_PostDevolverFlujoSGR(parametrosActualizar.Contenido.FlujoId, parametrosActualizar.Contenido.ObjetoNegocioId, Guid.Parse(parametrosActualizar.Contenido.InstanciaId), RequestContext.Principal.Identity.Name));

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

        [Route("api/Proyecto/CrearInstanciaCtusSGR")]
        [HttpPost]
        public async Task<IHttpActionResult> CrearInstanciaCtusSGR(ObjetoNegocio contenido)
        {
            try
            {
                var respuestaAutorizacion = await _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                           RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                           ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                           ConfigurationManager.AppSettings["SGR_CTUS_CrearInstanciaCtusSGR"]);

                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                if (contenido == null)
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NoContent, new Exception(string.Format(ServiciosNegocioRecursos.ParametrosNoRecibidos))));

                _proyectoServicio.Usuario = RequestContext?.Principal?.Identity?.Name;
                _proyectoServicio.Ip = UtilidadesApi.GetClientIp(Request);

                var response = await Task.Run(() => _proyectoServicio.SGR_CTUS_CrearInstanciaCtusSGR(contenido, RequestContext.Principal.Identity.Name));

                var respuesta = new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    ReasonPhrase = response
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

        [Route("api/Proyecto/AplicarFlujoCTUSSGR")]
        [HttpPost]
        public async Task<IHttpActionResult> AplicarFlujoCTUSSGR(ObjetoNegocio contenido)
        {
            try
            {
                var respuestaAutorizacion = await _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                           RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                           ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                           ConfigurationManager.AppSettings["SGR_CTUS_AplicarFlujoCTUSSGR"]);

                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                if (contenido == null)
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NoContent, new Exception(string.Format(ServiciosNegocioRecursos.ParametrosNoRecibidos))));

                ValidarParametros(contenido);

                var parametrosActualizar = new ParametrosGuardarDto<ObjetoNegocio>
                {
                    Contenido = contenido
                };
                var parametrosAuditoria = new ParametrosAuditoriaDto
                {
                    Usuario = RequestContext.Principal.Identity.Name,
                    Ip = UtilidadesApi.GetClientIp(Request)
                };

                //await Task.Run(() => _proyectoServicio.GenerarFichaCTUSSGR(parametrosActualizar, parametrosAuditoria));
                await Task.Run(() => _proyectoServicio.GenerarFichaGenerico(parametrosActualizar, parametrosAuditoria, 1));

                await Task.Run(() => _proyectoServicio.SGR_Proyectos_NotificarUsuariosViabilidad(Guid.Parse(parametrosActualizar.Contenido.InstanciaId), parametrosActualizar.Contenido.ObjetoNegocioId, RequestContext.Principal.Identity.Name));

                var response = await Task.Run(() => _proyectoServicio.SGR_Proyectos_GenerarMensajeEstadoProyecto(Guid.Parse(parametrosActualizar.Contenido.InstanciaId), RequestContext.Principal.Identity.Name));

                await Task.Run(() => _proyectoServicio.SGR_Proyectos_PostAplicarFlujoSGR(parametrosActualizar.Contenido.FlujoId, parametrosActualizar.Contenido.ObjetoNegocioId, Guid.Parse(parametrosActualizar.Contenido.InstanciaId), RequestContext.Principal.Identity.Name));

                var respuesta = new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    ReasonPhrase = response
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

        [Route("api/Proyecto/DevolverFlujoCTUSSGR")]
        [HttpPost]
        public async Task<IHttpActionResult> DevolverFlujoCTUSSGR(ObjetoNegocio contenido)
        {
            try
            {
                var respuestaAutorizacion = await _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                           RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                           ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                           ConfigurationManager.AppSettings["SGR_CTUS_DevolverFlujoCTUSSGR"]);

                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                if (contenido == null)
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NoContent, new Exception(string.Format(ServiciosNegocioRecursos.ParametrosNoRecibidos))));

                var parametrosActualizar = new ParametrosGuardarDto<ObjetoNegocio>
                {
                    Contenido = contenido
                };
                var parametrosAuditoria = new ParametrosAuditoriaDto
                {
                    Usuario = RequestContext.Principal.Identity.Name,
                    Ip = UtilidadesApi.GetClientIp(Request)
                };

                await Task.Run(() => _proyectoServicio.SGR_Proyectos_NotificarUsuariosViabilidad(Guid.Parse(parametrosActualizar.Contenido.InstanciaId), parametrosActualizar.Contenido.ObjetoNegocioId, RequestContext.Principal.Identity.Name));

                //Se quita la generación de la ficha según el ajuste del BPI 
                //var response = await Task.Run(() => _proyectoServicio.GenerarFichaCTUSSGR(parametrosActualizar, parametrosAuditoria));

                await Task.Run(() => _proyectoServicio.SGR_Proyectos_PostDevolverFlujoSGR(parametrosActualizar.Contenido.FlujoId, parametrosActualizar.Contenido.ObjetoNegocioId, Guid.Parse(parametrosActualizar.Contenido.InstanciaId), RequestContext.Principal.Identity.Name));

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

        [Route("api/Proyecto/AplicarFlujoOCADPazSGR")]
        [HttpPost]
        public async Task<IHttpActionResult> AplicarFlujoOCADPazSGR(ObjetoNegocio contenido)
        {
            try
            {
                var respuestaAutorizacion = await _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                           RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                           ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                           ConfigurationManager.AppSettings["SGR_OCADPaz_AplicarFlujoSGR"]);

                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                if (contenido == null)
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NoContent, new Exception(string.Format(ServiciosNegocioRecursos.ParametrosNoRecibidos))));

                var parametrosActualizar = new ParametrosGuardarDto<ObjetoNegocio>
                {
                    Contenido = contenido
                };
                var parametrosAuditoria = new ParametrosAuditoriaDto
                {
                    Usuario = RequestContext.Principal.Identity.Name,
                    Ip = UtilidadesApi.GetClientIp(Request)
                };

                await Task.Run(() => _proyectoServicio.SGR_Proyectos_NotificarUsuariosViabilidad(Guid.Parse(parametrosActualizar.Contenido.InstanciaId), parametrosActualizar.Contenido.ObjetoNegocioId, RequestContext.Principal.Identity.Name));

                var response = await Task.Run(() => _proyectoServicio.SGR_Proyectos_GenerarMensajeEstadoProyecto(Guid.Parse(parametrosActualizar.Contenido.InstanciaId), RequestContext.Principal.Identity.Name));

                var respuesta = new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    ReasonPhrase = response
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

        [Route("api/Proyecto/GenerarFichaManualSubFlujoSGR")]
        [HttpPost]
        public async Task<IHttpActionResult> GenerarFichaManualSubFlujoSGR(ObjetoNegocio contenido)
        {
            try
            {
                var respuestaAutorizacion = await _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                           RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                           ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                           ConfigurationManager.AppSettings["SGR_CTUS_AplicarFlujoCTUSSGR"]);

                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                if (contenido == null)
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NoContent, new Exception(string.Format(ServiciosNegocioRecursos.ParametrosNoRecibidos))));

                var parametrosActualizar = new ParametrosGuardarDto<ObjetoNegocio>
                {
                    Contenido = contenido
                };
                var parametrosAuditoria = new ParametrosAuditoriaDto
                {
                    Usuario = RequestContext.Principal.Identity.Name,
                    Ip = UtilidadesApi.GetClientIp(Request)
                };

                var response = await Task.Run(() => _proyectoServicio.GenerarFichaGenerico(parametrosActualizar, parametrosAuditoria, 2));
                //var response = await Task.Run(() => _proyectoServicio.GenerarAdjuntarFichaManualSGR(parametrosActualizar, parametrosAuditoria));

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

        [Route("api/Proyecto/GenerarFichaManualFlujoSGR")]
        [HttpPost]
        public async Task<IHttpActionResult> GenerarFichaManualFlujoSGR(ObjetoNegocio contenido)
        {
            try
            {
                var respuestaAutorizacion = await _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                           RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                           ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                           ConfigurationManager.AppSettings["actualizarEstadoProyecto"]);

                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                if (contenido == null)
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NoContent, new Exception(string.Format(ServiciosNegocioRecursos.ParametrosNoRecibidos))));

                var parametrosActualizar = new ParametrosGuardarDto<ObjetoNegocio>
                {
                    Contenido = contenido
                };
                var parametrosAuditoria = new ParametrosAuditoriaDto
                {
                    Usuario = RequestContext.Principal.Identity.Name,
                    Ip = UtilidadesApi.GetClientIp(Request)
                };

                await Task.Run(() => _proyectoServicio.SGR_Proyectos_NotificarUsuariosViabilidad(Guid.Parse(parametrosActualizar.Contenido.InstanciaId), parametrosActualizar.Contenido.ObjetoNegocioId, RequestContext.Principal.Identity.Name));

                var response = await Task.Run(() => _proyectoServicio.GenerarFichaGenerico(parametrosActualizar, parametrosAuditoria, 2));
                //var response = await Task.Run(() => _proyectoServicio.GenerarAdjuntarFichaManualSGR(parametrosActualizar, parametrosAuditoria));

                await Task.Run(() => _proyectoServicio.SGR_Proyectos_PostDevolverFlujoSGR(parametrosActualizar.Contenido.FlujoId, parametrosActualizar.Contenido.ObjetoNegocioId, Guid.Parse(parametrosActualizar.Contenido.InstanciaId), RequestContext.Principal.Identity.Name));

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

        [Route("api/Proyecto/GenerarFichaAutomaticaFlujoSGR")]
        [HttpPost]
        public async Task<IHttpActionResult> GenerarFichaAutomaticaFlujoSGR(ObjetoNegocio contenido)
        {
            try
            {
                var respuestaAutorizacion = await _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                           RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                           ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                           ConfigurationManager.AppSettings["actualizarEstadoProyecto"]);

                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                if (contenido == null)
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NoContent, new Exception(string.Format(ServiciosNegocioRecursos.ParametrosNoRecibidos))));

                var parametrosActualizar = new ParametrosGuardarDto<ObjetoNegocio>
                {
                    Contenido = contenido
                };
                var parametrosAuditoria = new ParametrosAuditoriaDto
                {
                    Usuario = RequestContext.Principal.Identity.Name,
                    Ip = UtilidadesApi.GetClientIp(Request)
                };

                var response = await Task.Run(() => _proyectoServicio.GenerarFichaGenerico(parametrosActualizar, parametrosAuditoria, 2));
                //var response = await Task.Run(() => _proyectoServicio.GenerarAdjuntarFichaManualSGR(parametrosActualizar, parametrosAuditoria));

                var respuesta = new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    ReasonPhrase = (string)response
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

        [Route("api/Proyecto/NotificarUsuariosViabilidadSGR")]
        [HttpPost]
        public async Task<IHttpActionResult> NotificarUsuariosViabilidadSGR(ObjetoNegocio contenido)
        {
            try
            {
                var respuestaAutorizacion = await _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                           RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                           ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                           ConfigurationManager.AppSettings["actualizarEstadoProyecto"]);

                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                if (contenido == null)
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NoContent, new Exception(string.Format(ServiciosNegocioRecursos.ParametrosNoRecibidos))));

                var parametrosActualizar = new ParametrosGuardarDto<ObjetoNegocio>
                {
                    Contenido = contenido
                };
                var parametrosAuditoria = new ParametrosAuditoriaDto
                {
                    Usuario = RequestContext.Principal.Identity.Name,
                    Ip = UtilidadesApi.GetClientIp(Request)
                };

                await Task.Run(() => _proyectoServicio.SGR_Proyectos_NotificarUsuariosViabilidad(Guid.Parse(parametrosActualizar.Contenido.InstanciaId), parametrosActualizar.Contenido.ObjetoNegocioId, RequestContext.Principal.Identity.Name));

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

        [Route("api/Proyecto/CrearInstanciaCtusAutomaticaSGR")]
        [HttpPost]
        public async Task<IHttpActionResult> CrearInstanciaCtusAutomaticaSGR(ObjetoNegocio contenido)
        {
            try
            {
                var respuestaAutorizacion = await _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                           RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                           ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                           ConfigurationManager.AppSettings["SGR_CTUS_CrearInstanciaCtusAutomaticaSGR"]);

                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                if (contenido == null)
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NoContent, new Exception(string.Format(ServiciosNegocioRecursos.ParametrosNoRecibidos))));

                _proyectoServicio.Usuario = RequestContext?.Principal?.Identity?.Name;
                _proyectoServicio.Ip = UtilidadesApi.GetClientIp(Request);

                var response = await Task.Run(() => _proyectoServicio.SGR_CTUS_CrearInstanciaCtusAutomaticaSGR(contenido, RequestContext.Principal.Identity.Name));

                var respuesta = new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    ReasonPhrase = response
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

        [Route("api/SGP/Proyecto/IniciarFlujoSGP")]
        [HttpPost]
        public async Task<IHttpActionResult> IniciarFlujoSGP(ObjetoNegocio contenido)
        {
            try
            {
                var respuestaAutorizacion = await _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                           RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                           ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                           ConfigurationManager.AppSettings["actualizarEstadoProyecto"]);

                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                if (contenido == null)
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NoContent, new Exception(string.Format(ServiciosNegocioRecursos.ParametrosNoRecibidos))));

                ValidarParametros(contenido);

                var parametrosActualizar = new ParametrosGuardarDto<ObjetoNegocio>
                {
                    Contenido = contenido
                };
                var parametrosAuditoria = new ParametrosAuditoriaDto
                {
                    Usuario = RequestContext.Principal.Identity.Name,
                    Ip = UtilidadesApi.GetClientIp(Request)
                };
                var response = await Task.Run(() => _proyectoServicio.IniciarFlujoSGP(parametrosActualizar, parametrosAuditoria));

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

        [Route("api/Proyecto/AplicarFlujoViabilidadSGP")]
        [HttpPost]
        public async Task<IHttpActionResult> AplicarFlujoViabilidadSGP(ObjetoNegocio contenido)
        {
            try
            {
                var respuestaAutorizacion = await _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                           RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                           ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                           ConfigurationManager.AppSettings["actualizarEstadoProyecto"]);

                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                if (contenido == null)
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NoContent, new Exception(string.Format(ServiciosNegocioRecursos.ParametrosNoRecibidos))));

                //ValidarParametros(contenido);

                var parametrosActualizar = new ParametrosGuardarDto<ObjetoNegocio>
                {
                    Contenido = contenido
                };
                var parametrosAuditoria = new ParametrosAuditoriaDto
                {
                    Usuario = RequestContext.Principal.Identity.Name,
                    Ip = UtilidadesApi.GetClientIp(Request)
                };

                await Task.Run(() => _proyectoServicio.GenerarFichaViabilidadSGP(parametrosActualizar, parametrosAuditoria));

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
    }
}