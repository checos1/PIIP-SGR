using DNP.ServiciosNegocio.Comunes;
using DNP.ServiciosNegocio.Comunes.Autorizacion;
using DNP.ServiciosNegocio.Comunes.Excepciones;
using DNP.ServiciosNegocio.Servicios.Interfaces.SGP.Ajustes;
using Swashbuckle.Swagger.Annotations;
using System.Configuration;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using DNP.ServiciosNegocio.Dominio.Dto.SGP.Transversales;
using System.Net.Http;
using DNP.ServiciosNegocio.Dominio.Dto.Proyectos;
using System;
using DNP.ServiciosNegocio.Dominio.Dto.CadenaValor;
using DNP.ServiciosNegocio.Dominio.Dto.SGP.Ajustes;
using DNP.ServiciosNegocio.Dominio.Dto.FuenteFinanciacion;
using DNP.ServiciosNegocio.Dominio.Dto.Catalogos;
using System.Collections.Generic;

namespace DNP.ServiciosNegocio.Web.API.Controllers.SGP.Ajustes
{
    public class AjustesSgpController : ApiController
    {
        private readonly IAjustesSgpServicio _ajustesSgpServicio;
        private readonly IAutorizacionUtilidades _autorizacionUtilidades;


        public AjustesSgpController(IAjustesSgpServicio ajustesSgpServicio, IAutorizacionUtilidades autorizacionUtilidades)
        {
            _ajustesSgpServicio = ajustesSgpServicio;
            _autorizacionUtilidades = autorizacionUtilidades;
        }

        #region Horizonte

        [Route("api/SGP/Ajustes/ObtenerHorizonteSgp")]
        [SwaggerResponse(HttpStatusCode.OK, "Obtiene la información de horizonte de un proyecto de SGP.", typeof(string))]
        [HttpPost]
        public async Task<IHttpActionResult> ObtenerHorizonteSgp([FromBody] ParametrosEncabezadoSGP parametros)
        {
            try
            {
                var respuestaAutorizacion = await _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                                                   RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                                                   ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                                                   ConfigurationManager.AppSettings["SGR_Encabezado_LeerEncabezado"]);

                if (!respuestaAutorizacion.IsSuccessStatusCode)
                    return ResponseMessage(respuestaAutorizacion);

                var result = await Task.Run(() => _ajustesSgpServicio.ObtenerHorizonteSgp(parametros));
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

        [Route("api/SGP/Ajustes/ActualizarHorizonteProyectoSgp")]
        [HttpPost]
        public async Task<IHttpActionResult> ActualizarHorizonteProyectoSgp([FromBody] HorizonteProyectoDto DatosHorizonteProyecto)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name, RequestContext.Principal.Identity.GetHashCode().ToString(), ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"], ConfigurationManager.AppSettings["GestionTramitesProyectos"]).Result;
                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                var result = await Task.Run(() => _ajustesSgpServicio.ActualizarHorizonteSgp(DatosHorizonteProyecto, RequestContext.Principal.Identity.Name));

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

        [Route("api/SGP/Ajustes/ObtenerJustificacionHorizonteSgp")]
        [SwaggerResponse(HttpStatusCode.OK, "Informacion devuelta por consulta", typeof(HttpResponseMessage))]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerJustificacionHorizonteSgp(int proyectoId)
        {
            try
            {
                var result = await Task.Run(() => _ajustesSgpServicio.ObtenerCambiosJustificacionHorizonteSgp(proyectoId));
                return Ok(result);
            }
            catch (Exception e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        #endregion

        #region Indicadores

        [Route("api/SGP/Ajustes/ObtenerIndicadoresProductoSgp/{bpin}")]
        [SwaggerResponse(HttpStatusCode.OK, "Retorna IndicadorProductoDto por bpin", typeof(IndicadorProductoDto))]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerIndicadoresProductoSgp(string bpin)
        {
            /*var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                                               RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                                               ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                                               ConfigurationManager.AppSettings["ObtenerIndicadoresProducto"]).Result;

            if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);*/

            //ValidacionParametro(bpin);  --se pone en comentario ya que en este campo puede venir el id de la instancia

            /* var tokenAutorizacion = Request.Headers.Authorization.Parameter;*/

            var result = await Task.Run(() => _ajustesSgpServicio.ObtenerIndicadoresProductoSgp(bpin));

            return Ok(result);
        }

        [Route("api/SGP/Ajustes/GuardarIndicadoresSecundariosSgp")]
        [SwaggerResponse(HttpStatusCode.OK, "Informacion devuelta por guardado", typeof(HttpResponseMessage))]
        [HttpPost]
        public async Task<IHttpActionResult> GuardarIndicadoresSecundariosSgp([FromBody] AgregarIndicadoresSecundariosDto listaIndicadoresSecundarios)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name, RequestContext.Principal.Identity.GetHashCode().ToString(), ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"], ConfigurationManager.AppSettings["GestionTramitesProyectos"]).Result;
                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                var parametrosGuardar = listaIndicadoresSecundarios;

                var resultado = await Task.Run(() => _ajustesSgpServicio.GuardarIndicadoresSecundariosSgp(parametrosGuardar, RequestContext.Principal.Identity.Name));

                var respuesta = new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    ReasonPhrase = resultado.Mensaje
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

        [Route("api/SGP/Ajustes/EliminarIndicadorProductoSgp/{indicadorId}")]
        [SwaggerResponse(HttpStatusCode.OK, "Retorna IndicadorProductoDto por bpin", typeof(IndicadorProductoDto))]
        [HttpGet]
        public async Task<IHttpActionResult> EliminarIndicadorProductoSgp(int indicadorId)
        {
            var result = await Task.Run(() => _ajustesSgpServicio.EliminarIndicadorProductoSgp(indicadorId, RequestContext.Principal.Identity.Name));

            return Ok(result);
        }

        [Route("api/SGP/Ajustes/ActualizarMetaAjusteIndicadorSgp")]
        [SwaggerResponse(HttpStatusCode.OK, "Informacion devuelta por guardado", typeof(HttpResponseMessage))]
        [HttpPost]
        public async Task<IHttpActionResult> ActualizarMetaAjusteIndicadorSgp(IndicadoresIndicadorProductoDto Indicador)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name, RequestContext.Principal.Identity.GetHashCode().ToString(), ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"], ConfigurationManager.AppSettings["GestionTramitesProyectos"]).Result;
                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                var resultado = await Task.Run(() => _ajustesSgpServicio.ActualizarMetaAjusteIndicadorSgp(Indicador, RequestContext.Principal.Identity.Name));

                var respuesta = new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    ReasonPhrase = resultado.Mensaje
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

        #endregion

        #region Beneficiarios

        [Route("api/SGP/Ajustes/ObtenerProyectosBeneficiariosSgp")]
        [SwaggerResponse(HttpStatusCode.OK, "Obtener Resumen Objetivos Productos Actividades", typeof(HttpResponseMessage))]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerProyectosBeneficiariosSgp(string bpin)
        {
            var result = await Task.Run(() => _ajustesSgpServicio.ObtenerProyectosBeneficiariosSgp(bpin));

            return Ok(result);
        }

        [Route("api/SGP/Ajustes/ObtenerProyectosBeneficiariosDetalleSgp")]
        [SwaggerResponse(HttpStatusCode.OK, "ObtenerProyectosBeneficiarios Detalle", typeof(HttpResponseMessage))]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerProyectosBeneficiariosDetalleSgp(string json)
        {
            var result = await Task.Run(() => _ajustesSgpServicio.ObtenerProyectosBeneficiariosDetalleSgp(json));

            return Ok(result);
        }

        [Route("api/SGP/Ajustes/GuardarBeneficiarioTotales")]
        [SwaggerResponse(HttpStatusCode.OK, "Informacion devuelta por guardado definitivo", typeof(HttpResponseMessage))]
        [HttpPost]
        public async Task<IHttpActionResult> GuardarBeneficiarioTotalesSgp(BeneficiarioTotalesDto beneficiario)
        {
            try
            {
                await Task.Run(() => _ajustesSgpServicio.GuardarBeneficiarioTotalesSgp(beneficiario, RequestContext.Principal.Identity.Name));

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

        [Route("api/SGP/Ajustes/GuardarBeneficiarioProducto")]
        [SwaggerResponse(HttpStatusCode.OK, "Informacion devuelta por guardado definitivo", typeof(HttpResponseMessage))]
        [HttpPost]
        public async Task<IHttpActionResult> GuardarBeneficiarioProductoSgp(BeneficiarioProductoSgpDto beneficiario)
        {
            try
            {
                await Task.Run(() => _ajustesSgpServicio.GuardarBeneficiarioProductoSgp(beneficiario, RequestContext.Principal.Identity.Name));

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

        [Route("api/SGP/Ajustes/GuardarBeneficiarioProductoLocalizacionSgp")]
        [SwaggerResponse(HttpStatusCode.OK, "Informacion devuelta por guardado definitivo", typeof(HttpResponseMessage))]
        [HttpPost]
        public async Task<IHttpActionResult> GuardarBeneficiarioProductoLocalizacionSgp(BeneficiarioProductoLocalizacionDto beneficiario)
        {
            try
            {
                await Task.Run(() => _ajustesSgpServicio.GuardarBeneficiarioProductoLocalizacionSgp(beneficiario, RequestContext.Principal.Identity.Name));

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

        [Route("api/SGP/Ajustes/GuardarBeneficiarioProductoLocalizacionCaracterizacionSgp")]
        [SwaggerResponse(HttpStatusCode.OK, "Informacion devuelta por guardado definitivo", typeof(HttpResponseMessage))]
        [HttpPost]
        public async Task<IHttpActionResult> GuardarBeneficiarioProductoLocalizacionCaracterizacionSgp(BeneficiarioProductoLocalizacionCaracterizacionDto beneficiario)
        {
            try
            {
                await Task.Run(() => _ajustesSgpServicio.GuardarBeneficiarioProductoLocalizacionCaracterizacionSgp(beneficiario, RequestContext.Principal.Identity.Name));

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

        #endregion

        #region Localizaciones

        [Route("api/SGP/Ajustes/Localizacion/GuardarLocalizacionSgp")]
        [HttpPost]
        public async Task<ResultadoProcedimientoDto> GuardarLocalizacionSgp(LocalizacionProyectoAjusteDto localizacionProyecto)
        {
            try
            {
                var result = await Task.Run(() => _ajustesSgpServicio.GuardarLocalizacionSgp(localizacionProyecto, RequestContext.Principal.Identity.Name));
                return (result);
            }
            catch (Exception e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        #endregion
        #region fuentefinanciacion
        [Route("api/SGP/fuentes/FuentesFinanciacionRecursosAjustes/AgregarSgp")]
        [SwaggerResponse(HttpStatusCode.OK, "Informacion devuelta por guardado definitivo", typeof(HttpResponseMessage))]
        [HttpPost]
        public async Task<IHttpActionResult> FuentesFinanciacionRecursosAjustesAgregarSgp(FuenteFinanciacionAgregarAjusteDto objFuenteFinanciacionAgregarAjusteDto)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name, RequestContext.Principal.Identity.GetHashCode().ToString(), ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"], ConfigurationManager.AppSettings["postFuentesFinanciacionAgregar"]).Result;
                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                string resp = await Task.Run(() => _ajustesSgpServicio.FuentesFinanciacionRecursosAjustesAgregarSgp(objFuenteFinanciacionAgregarAjusteDto, RequestContext.Principal.Identity.Name));

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
        #endregion
        #region costos
        [Route("api/SGP/ObtenerResumenObjetivosProductosActividadesSgp")]
        [SwaggerResponse(HttpStatusCode.OK, "Obtener Resumen Objetivos Productos Actividades", typeof(HttpResponseMessage))]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerResumenObjetivosProductosActividadesSgp(string bpin)
        {
            var result = await Task.Run(() => _ajustesSgpServicio.ObtenerResumenObjetivosProductosActividadesSgp(bpin));

            return Ok(result);
        }


        [Route("api/SGP/GuardarCostoActividadesSgp")]
        [SwaggerResponse(HttpStatusCode.OK, "Informacion devuelta por guardado definitivo", typeof(HttpResponseMessage))]
        [HttpPost]
        public async Task<IHttpActionResult> GuardarCostoActividadesSgp(ProductoAjusteDto producto)
        {
            try
            {
                var usuario = RequestContext.Principal.Identity.Name;
                var result = await Task.Run(() => _ajustesSgpServicio.GuardarAjusteCostoActividadesSgp(producto, RequestContext.Principal.Identity.Name));
                return Ok(result);
            }
            catch (Exception e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }


        [Route("api/SGP/AgregarEntregableSgp")]
        [SwaggerResponse(HttpStatusCode.OK, "Informacion devuelta por guardado definitivo", typeof(HttpResponseMessage))]
        [HttpPost]
        public async Task<IHttpActionResult> AgregarEntregableSgp(AgregarEntregable[] entregables)
        {
            try
            {
                await Task.Run(() => _ajustesSgpServicio.AgregarEntregableSgp(entregables, RequestContext.Principal.Identity.Name));

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


        [Route("api/SGP/EliminarEntregableSgp")]
        [SwaggerResponse(HttpStatusCode.OK, "Eliminar Entregable / Actividad", typeof(HttpResponseMessage))]
        [HttpPost]
        public async Task<IHttpActionResult> EliminarEntregableSgp(EntregablesActividadesDto entregable)
        {
            try
            {
                await Task.Run(() => _ajustesSgpServicio.EliminarEntregableSgp(entregable));

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
        #endregion
        #region regionalizacion
        [Route("api/IndicadoresProducto/RegionalizacionGeneralSgp")]
        [SwaggerResponse(HttpStatusCode.OK, "Retorna Regionalización por bpin", typeof(Dominio.Dto.CadenaValor.RegionalizacionDto))]
        [HttpGet]
        public async Task<IHttpActionResult> RegionalizacionGeneral(string bpin)
        {

            //ValidacionParametro(bpin); --se pone en comentario ya que en este campo puede venir el id de la instancia

            var result = await Task.Run(() => _ajustesSgpServicio.RegionalizacionGeneralSgp(bpin));

            return Ok(result);
        }
        #endregion
        #region recursos
        [Route("api/Ajustes/TiposRecursosEntidadSgp")]
        [HttpGet]
        public async Task<List<CatalogoDto>> ConsultaTiposRecursosEntidad(int entityTypeCatalogId, int entityType)
        {
            var result = await Task.Run(() => _ajustesSgpServicio.ConsultaTiposRecursosEntidadSgp(entityTypeCatalogId, entityType));
            return result;
        }
        #endregion

        [Route("api/SGP/Ajustes/ObtenerCategoriasFocalizacionJustificacionSgp")]
        [HttpGet]      
        public async Task<IHttpActionResult> ObtenerCategoriasFocalizacionJustificacionSgp(string Bpin)
        {
            try
            {                                                        
                var result = await Task.Run(() => _ajustesSgpServicio.ObtenerCategoriasFocalizacionJustificacionSgp(Bpin));
                return Ok(result);

            }
            catch (ServiciosNegocioException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
           
        }

        [Route("api/SGP/Ajustes/ObtenerDetalleCategoriasFocalizacionJustificacionSgp")]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerDetalleCategoriasFocalizacionJustificacionSgp(string Bpin)
        {
            try
            {
                var result = await Task.Run(() => _ajustesSgpServicio.ObtenerDetalleCategoriasFocalizacionJustificacionSgp(Bpin));
                return Ok(result);

            }
            catch (ServiciosNegocioException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }

        }
    }
}