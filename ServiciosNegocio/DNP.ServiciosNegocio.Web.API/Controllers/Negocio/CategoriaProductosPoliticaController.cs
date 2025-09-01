using DNP.ServiciosNegocio.Comunes;
using DNP.ServiciosNegocio.Comunes.Autorizacion;
using DNP.ServiciosNegocio.Comunes.Dto;
using DNP.ServiciosNegocio.Comunes.Dto.Formulario;
using DNP.ServiciosNegocio.Comunes.Excepciones;
using DNP.ServiciosNegocio.Dominio.Dto.Focalizacion;
using DNP.ServiciosNegocio.Dominio.Dto.IndicadoresPolitica;
using DNP.ServiciosNegocio.Servicios.Interfaces.IndicadoresPolitica;
using Swashbuckle.Swagger.Annotations;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;


namespace DNP.ServiciosNegocio.Web.API.Controllers.Negocio
{
    public class CategoriaProductosPoliticaController : ApiController
    {
        private readonly ICategoriaProductosPoliticaServicio _CategoriaProductosPoliticaServicio;
        private readonly IAutorizacionUtilidades _autorizacionUtilidades;

        public CategoriaProductosPoliticaController(ICategoriaProductosPoliticaServicio CategoriaProductosPoliticaServicio,
            IAutorizacionUtilidades autorizacionUtilidades)
        {
            _CategoriaProductosPoliticaServicio = CategoriaProductosPoliticaServicio;
            _autorizacionUtilidades = autorizacionUtilidades;
        }


        #region Consultar Productos Politica

        /// <summary>
        /// Consultar Productos Politica
        /// </summary>
        /// <param name="BPIN"></param>
        /// <param name="fuenteId"></param>
        /// <returns></returns>
        /// <exception cref="HttpResponseException"></exception>
        [Route("api/Focalizacion/ConsultarProductosPolitica")]
        [SwaggerResponse(HttpStatusCode.OK, "Retorna listado de categoria de productos por politica.", typeof(IndicadoresPoliticaDto))]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerDatosCategoriaProductosPolitic(string BPIN, int fuenteId, int politicaId)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                                                   RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                                                   ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                                                   ConfigurationManager.AppSettings["ObtenerDatosAdicionalesFuente"]).Result;

                if (!respuestaAutorizacion.IsSuccessStatusCode)
                    return ResponseMessage(respuestaAutorizacion);

                var result = await Task.Run(() => _CategoriaProductosPoliticaServicio.ObtenerDatosCategoriaProductosPolitica(BPIN, fuenteId, politicaId));
                if (result != null) return Ok(result);

                var respuestaHttp = new System.Net.Http.HttpResponseMessage()
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

        #endregion

        #region Guardar Datos SolicitudRecursos

        /// <summary>
        /// Guardar Datos SolicitudRecursos
        /// </summary>
        /// <param name="BPIN"></param>
        /// <param name="fuenteId"></param>
        /// <returns></returns>
        /// <exception cref="HttpResponseException"></exception>
        [Route("api/Focalizacion/GuardarDatosSolicitudRecursos")]
        [SwaggerResponse(HttpStatusCode.OK, "Retorna listado de categoria de productos por politica.", typeof(IndicadoresPoliticaDto))]
        [HttpPost]
        public async Task<IHttpActionResult> GuardarDatosSolicitudRecursos(CategoriaProductoPoliticaDto categoriaProductoPoliticaDto)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                                                   RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                                                   ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                                                   ConfigurationManager.AppSettings["ObtenerDatosAdicionalesFuente"]).Result;

                if (!respuestaAutorizacion.IsSuccessStatusCode)
                    return ResponseMessage(respuestaAutorizacion);

                var parametrosGuardar = new ParametrosGuardarDto<CategoriaProductoPoliticaDto>();
                parametrosGuardar.Contenido = categoriaProductoPoliticaDto;

                var parametrosAuditoria = new ParametrosAuditoriaDto
                {
                    Usuario = RequestContext.Principal.Identity.Name,
                    Ip = UtilidadesApi.GetClientIp(Request)
                };

                var result = await Task.Run(() => 
                _CategoriaProductosPoliticaServicio
                .GuardarDatosSolicitudRecursos(parametrosGuardar, parametrosAuditoria.Usuario));

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

        #endregion
    }
}