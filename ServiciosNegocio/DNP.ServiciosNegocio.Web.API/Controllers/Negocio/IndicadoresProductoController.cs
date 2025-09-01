// ReSharper disable ParameterOnlyUsedForPreconditionCheck.Local
namespace DNP.ServiciosNegocio.Web.API.Controllers.Negocio
{
    using System.Collections.Generic;
    using System.Configuration;
    using System.Net;
    using System.Net.Http;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Http;
    using Comunes;
    using Comunes.Autorizacion;
    using Comunes.Dto.ObjetosNegocio;
    using Comunes.Excepciones;
    using Dominio.Dto.Proyectos;
    using Servicios.Interfaces.Proyectos;
    using Swashbuckle.Swagger.Annotations;
    using System;
    using DNP.ServiciosNegocio.Dominio.Dto.Genericos;
    using Newtonsoft.Json.Linq;
    using DNP.ServiciosNegocio.Servicios.Interfaces.CadenaValor;
    using DNP.ServiciosNegocio.Dominio.Dto.CadenaValor;
    using DNP.ServiciosNegocio.Dominio.Dto.RegMetasRecursosDtoAjuste;
    using DNP.ServiciosNegocio.Dominio.Dto.Focalizacion;

    public class IndicadoresProductoController : ApiController
    {
        private readonly IIndicadoresProductoServicio _indicadoresProductoServicio;
        private readonly IAutorizacionUtilidades _autorizacionUtilidades;

        public IndicadoresProductoController(IIndicadoresProductoServicio indicadoresProductoServicio, IAutorizacionUtilidades autorizacionUtilidades)
        {
            _indicadoresProductoServicio = indicadoresProductoServicio;
            _autorizacionUtilidades = autorizacionUtilidades;
        }

        [Route("api/IndicadoresProducto/ObtenerIndicadoresProducto/{bpin}")]
        [SwaggerResponse(HttpStatusCode.OK, "Retorna IndicadorProductoDto por bpin", typeof(IndicadorProductoDto))]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerIndicadoresProducto(string bpin)
        {
            /*var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                                               RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                                               ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                                               ConfigurationManager.AppSettings["ObtenerIndicadoresProducto"]).Result;

            if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);*/

            //ValidacionParametro(bpin);  --se pone en comentario ya que en este campo puede venir el id de la instancia

            /* var tokenAutorizacion = Request.Headers.Authorization.Parameter;*/

            var result = await Task.Run(() => _indicadoresProductoServicio.ObtenerIndicadoresProducto(bpin));

            return Ok(result);
        }

        private void ValidacionParametro(string bpin)
        {
            if (string.IsNullOrEmpty(bpin))
                throw new ServiciosNegocioHttpResponseException(HttpStatusCode.BadRequest, string.Format(ServiciosNegocioRecursos.ParametroNoRecibido, nameof(bpin)));

            // ReSharper disable once UnusedVariable
            if (!long.TryParse(bpin, out var outBpin) || bpin.Length > 100)
                throw new ServiciosNegocioHttpResponseException(HttpStatusCode.BadRequest, string.Format(ServiciosNegocioRecursos.ParametroNoValidos, nameof(bpin)));
        }

        [Route("api/IndicadoresProducto/GuardarIndicadoresSecundarios")]
        [SwaggerResponse(HttpStatusCode.OK, "Informacion devuelta por guardado", typeof(HttpResponseMessage))]
        [HttpPost]
        public async Task<IHttpActionResult> GuardarTramiteInformacionPresupuestal([FromBody] AgregarIndicadoresSecundariosDto listaIndicadoresSecundarios)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name, RequestContext.Principal.Identity.GetHashCode().ToString(), ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"], ConfigurationManager.AppSettings["GestionTramitesProyectos"]).Result;
                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                var parametrosGuardar = listaIndicadoresSecundarios;

                var resultado = await Task.Run(() => _indicadoresProductoServicio.GuardarIndicadoresSecundarios(parametrosGuardar, RequestContext.Principal.Identity.Name));

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

        [Route("api/IndicadoresProducto/EliminarIndicadorProducto/{indicadorId}")]
        [SwaggerResponse(HttpStatusCode.OK, "Retorna IndicadorProductoDto por bpin", typeof(IndicadorProductoDto))]
        [HttpGet]
        public async Task<IHttpActionResult> EliminarIndicadorProducto(int indicadorId)
        {
            var result = await Task.Run(() => _indicadoresProductoServicio.EliminarIndicadorProducto(indicadorId, RequestContext.Principal.Identity.Name));

            return Ok(result);
        }

        [Route("api/IndicadoresProducto/ActualizarMetaAjusteIndicador")]
        [SwaggerResponse(HttpStatusCode.OK, "Informacion devuelta por guardado", typeof(HttpResponseMessage))]
        [HttpPost]
        public async Task<IHttpActionResult> ActualizarMetaAjusteIndicador(IndicadoresIndicadorProductoDto Indicador)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name, RequestContext.Principal.Identity.GetHashCode().ToString(), ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"], ConfigurationManager.AppSettings["GestionTramitesProyectos"]).Result;
                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                var resultado = await Task.Run(() => _indicadoresProductoServicio.ActualizarMetaAjusteIndicador(Indicador, RequestContext.Principal.Identity.Name));

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

        [Route("api/IndicadoresProducto/IndicadoresValidarCapituloModificado/{bpin}")]
        [SwaggerResponse(HttpStatusCode.OK, "Retorna Indicadores Modificados en el capitulo por bpin", typeof(List<IndicadorCapituloModificadoDto>))]
        [HttpGet]
        public async Task<IHttpActionResult> IndicadoresValidarCapituloModificado(string bpin)
        {
            /*var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                                               RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                                               ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                                               ConfigurationManager.AppSettings["ObtenerIndicadoresProducto"]).Result;

            if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);*/

            //ValidacionParametro(bpin);  --se pone en comentario ya que en este campo puede venir el id de la instancia

            /* var tokenAutorizacion = Request.Headers.Authorization.Parameter;*/

            var result = await Task.Run(() => _indicadoresProductoServicio.IndicadoresValidarCapituloModificado(bpin));

            return Ok(result);
        }

        [Route("api/IndicadoresProducto/RegionalizacionGeneral/{bpin}")]
        [SwaggerResponse(HttpStatusCode.OK, "Retorna Regionalización por bpin", typeof(RegionalizacionDto))]
        [HttpGet]
        public async Task<IHttpActionResult> RegionalizacionGeneral(string bpin)
        {

            //ValidacionParametro(bpin); --se pone en comentario ya que en este campo puede venir el id de la instancia

            var result = await Task.Run(() => _indicadoresProductoServicio.RegionalizacionGeneral(bpin));

            return Ok(result);
        }

        [Route("api/IndicadoresProducto/GuardarRegionalizacionFuentesFinanciacionAjustes")]
        [SwaggerResponse(HttpStatusCode.OK, "Informacion devuelta por guardado", typeof(HttpResponseMessage))]
        [HttpPost]
        public async Task<IHttpActionResult> GuardarRegionalizacionFuentesFinanciacionAjustes(List<RegionalizacionFuenteAjusteDto> regionalizacionFuenteAjuste)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name, RequestContext.Principal.Identity.GetHashCode().ToString(), ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"], ConfigurationManager.AppSettings["GestionTramitesProyectos"]).Result;
                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                var resultado = await Task.Run(() => _indicadoresProductoServicio.GuardarRegionalizacionFuentesFinanciacionAjustes(regionalizacionFuenteAjuste, RequestContext.Principal.Identity.Name));

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

        [Route("api/IndicadoresProducto/GuardarFocalizacionCategoriasAjustes")]
        [SwaggerResponse(HttpStatusCode.OK, "Informacion devuelta por guardado", typeof(HttpResponseMessage))]
        [HttpPost]
        public async Task<RespuestaGeneralDto> GuardarFocalizacionCategoriasAjustes(List<FocalizacionCategoriasAjusteDto> focalizacionCategoriasAjuste)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                    RequestContext.Principal.Identity.GetHashCode().ToString(), ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                    ConfigurationManager.AppSettings["postDatosAdicionalesAgregar"]).Result;
                var resultado = new RespuestaGeneralDto();

                if (!respuestaAutorizacion.IsSuccessStatusCode)
                {
                    resultado.Exito = false;
                    resultado.Mensaje = respuestaAutorizacion.ToString();
                }
                //return ResponseMessage(respuestaAutorizacion);

                resultado = await Task.Run(() => _indicadoresProductoServicio.GuardarFocalizacionCategoriasAjustes(focalizacionCategoriasAjuste, RequestContext.Principal.Identity.Name));

                /*
                var respuesta = new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    ReasonPhrase = ServiciosNegocioRecursos.PostExitoso,
                };
                */
                //return Ok(resultado);
                return (resultado);
            }
            catch (Exception e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
            /*
            catch (ServiciosNegocioException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
            catch (AutorizacionException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, e));
            }
            */
        }

        [Route("api/ObtenerDetalleAjustesJustificaionRegionalizacion")]
        [SwaggerResponse(HttpStatusCode.OK, "Retorna listado de Fuentes de Financiacion", typeof(string))]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerDetalleAjustesJustificaionRegionalizacion(string bpin)
        {
            var result = await Task.Run(() => _indicadoresProductoServicio.ObtenerDetalleAjustesJustificaionRegionalizacion(bpin));
            if (result != null) return Ok(result);

            var respuestaHttp = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.NotFound,
                ReasonPhrase = ServiciosNegocioRecursos.SinResultados
            };

            return ResponseMessage(respuestaHttp);
        }

        [Route("api/ObtenerSeccionOtrasPoliticasFacalizacionPT")]
        [SwaggerResponse(HttpStatusCode.OK, "Retorna listado de Fuentes de Financiacion", typeof(string))]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerSeccionOtrasPoliticasFacalizacionPT(string bpin)
        {
            var result = await Task.Run(() => _indicadoresProductoServicio.ObtenerSeccionOtrasPoliticasFacalizacionPT(bpin));
            if (result != null) return Ok(result);

            var respuestaHttp = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.NotFound,
                ReasonPhrase = ServiciosNegocioRecursos.SinResultados
            };

            return ResponseMessage(respuestaHttp);
        }

        [Route("api/ObtenerSeccionPoliticaFocalizacionDT")]
        [SwaggerResponse(HttpStatusCode.OK, "Retorna listado de Fuentes de Financiacion", typeof(string))]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerSeccionPoliticaFocalizacionDT(string bpin)
        {
            var result = await Task.Run(() => _indicadoresProductoServicio.ObtenerSeccionPoliticaFocalizacionDT(bpin));
            if (result != null) return Ok(result);

            var respuestaHttp = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.NotFound,
                ReasonPhrase = ServiciosNegocioRecursos.SinResultados
            };

            return ResponseMessage(respuestaHttp);
        }
    }
}