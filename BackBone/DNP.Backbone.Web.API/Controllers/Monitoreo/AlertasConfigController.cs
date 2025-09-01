using DNP.Backbone.Comunes.Dto;
using DNP.Backbone.Comunes.Enums;
using DNP.Backbone.Comunes.Excepciones;
using DNP.Backbone.Servicios.Interfaces.Autorizacion;
using DNP.Backbone.Servicios.Interfaces.Monitoreo;
using DNP.Backbone.Servicios.Interfaces.ServiciosNegocio;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;

namespace DNP.Backbone.Web.API.Controllers
{
    public class AlertasConfigController : Base.BackboneBase
    {
        private readonly IAlertasConfigServicios _alertasConfigServicios;
        private readonly IAutorizacionServicios _autorizacionUtilidades;
        private readonly IServiciosNegocioServicios _serviciosNegocioServicios;

        public AlertasConfigController(IAlertasConfigServicios alertasConfigServicios, IAutorizacionServicios autorizacionUtilidades, IServiciosNegocioServicios serviciosNegocioServicios)
            : base(autorizacionUtilidades)
        {
            _alertasConfigServicios = alertasConfigServicios;
            _autorizacionUtilidades = autorizacionUtilidades;
            _serviciosNegocioServicios = serviciosNegocioServicios;
        }

        /// <summary>
        /// Api para obtención de datos de alertas config.
        /// </summary>
        /// <param name="alertasConfigFiltroDto"></param>
        /// <returns>Objeto con propiedades para realizar consulta de datos de alertas config.</returns>
        [Route("api/Monitoreo/AlertasConfig")]
        [HttpPost]
        public async Task<IHttpActionResult> ObtenerAlertasConfig(AlertasConfigFiltroDto alertasConfigFiltroDto)
        {
            try
            {
                if (!await ValidarParametrosRequest(alertasConfigFiltroDto.ParametrosDto).ConfigureAwait(false)) return ResponseMessage(base.RespuestaAutorizacion);
                var result = await _alertasConfigServicios.ObtenerAlertasConfig(alertasConfigFiltroDto).ConfigureAwait(false);
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        /// <summary>
        /// Api para creación de datos de alertas config.
        /// </summary>
        /// <param name="alertasConfigFiltroDto"></param>
        /// <returns>Objeto con datos de alertas config.</returns>
        [Route("api/Monitoreo/AlertasConfig/CrearActualizar")]
        [HttpPost]
        public async Task<IHttpActionResult> CrearActualizarAlertasConfig(AlertasConfigFiltroDto alertasConfigFiltroDto)
        {
            try
            {
                if (!await ValidarParametrosRequest(alertasConfigFiltroDto.ParametrosDto).ConfigureAwait(false)) return ResponseMessage(base.RespuestaAutorizacion);
                var result = await _alertasConfigServicios.CrearActualizarAlertasConfig(alertasConfigFiltroDto).ConfigureAwait(false);
                return Ok(result);
            }
            catch (BackboneException e)
            {
                var response = Request.CreateResponse(HttpStatusCode.BadRequest, new RespuestaViewModel(e.Erros));
                throw new HttpResponseException(response);
            }
        }

        /// <summary>
        /// Api para creación de datos de alertas config.
        /// </summary>
        /// <param name="alertasConfigFiltroDto"></param>
        /// <returns>Objeto con datos de alertas config.</returns>
        [Route("api/Monitoreo/AlertasConfig/Eliminar")]
        [HttpPost]
        public async Task<IHttpActionResult> EliminarAlertasConfig(AlertasConfigFiltroDto alertasConfigFiltroDto)
        {
            try
            {
                if (!await ValidarParametrosRequest(alertasConfigFiltroDto.ParametrosDto).ConfigureAwait(false)) return ResponseMessage(base.RespuestaAutorizacion);
                var result = await _alertasConfigServicios.EliminarAlertasConfig(alertasConfigFiltroDto).ConfigureAwait(false);
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        /// <summary>
        /// Api para obtener datos de combo tipo alerta.
        /// </summary>
        /// <param name="alertasConfigFiltroDto"></param>
        /// <returns>Objeto con datos de tipo alerta.</returns>
        [Route("api/Monitoreo/AlertasConfig/ListaTipoAlerta")]
        [HttpPost]
        public async Task<IHttpActionResult> ObtenerListaTipoAlerta(ParametrosDto parametrosDto)
        {
            try
            {
                if (!await ValidarParametrosRequest(parametrosDto).ConfigureAwait(false)) return ResponseMessage(base.RespuestaAutorizacion);

                var result = await Task.Run(() => new[] {
                    new { Id = ((int)TipoAlertaEnum.Proyecto).ToString(), Name = TipoAlertaEnum.Proyecto.ToString() },
                    new { Id = ((int)TipoAlertaEnum.Tramite).ToString(), Name = TipoAlertaEnum.Tramite.ToString() }
                }.ToList()).ConfigureAwait(false);

                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        /// <summary>
        /// Api para obtener datos de combo estado.
        /// </summary>
        /// <param name="alertasConfigFiltroDto"></param>
        /// <returns>Objeto con datos de estado.</returns>
        [Route("api/Monitoreo/AlertasConfig/ListaEstado")]
        [HttpPost]
        public async Task<IHttpActionResult> ObtenerListaEstado(ParametrosDto parametrosDto)
        {
            try
            {
                if (!await ValidarParametrosRequest(parametrosDto).ConfigureAwait(false)) return ResponseMessage(base.RespuestaAutorizacion);

                var result = await Task.Run(() => new[] {
                    new { Id = "true", Name = EstadoEnum.Activo.ToString() },
                    new { Id = "false", Name = EstadoEnum.Inactivo.ToString() }
                }.ToList());

                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        /// <summary>
        /// Api para obtención de datos del configuración de alertas en Excel.
        /// </summary>
        /// <param name="alertasConfigFiltroDto"></param>
        /// <returns>Excel con datos generados</returns>
        [Route("api/Monitoreo/AlertasConfig/ObtenerExcel")]
        [HttpPost]
        public async Task<HttpResponseMessage> ObtenerExcel(AlertasConfigFiltroDto alertasConfigFiltroDto)
        {
            try
            {
                HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);

                var alertas = await Task.Run(() => _alertasConfigServicios.ObtenerAlertasConfig(alertasConfigFiltroDto));
                result.Content = ExcelUtilidades.ObtenerExcellConsolaAlertaConfig(alertas);
                result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
                result.StatusCode = HttpStatusCode.OK;
                
                return result;
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        /// Api para obtención de datos de configuración del alerta.
        /// </summary>
        /// <param name="instanciaProyectoDto"></param>
        /// <returns>Objeto con propiedades para realizar consulta de datos de configuración de alerta.</returns>
        [Route("api/Monitoreo/AlertasConfig/ObtenerInfoPDF")]
        [HttpPost]
        public async Task<IHttpActionResult> ObtenerInfoPDF(AlertasConfigFiltroDto alertasConfigFiltroDto)
        {
            try
            {
                if (!await ValidarParametrosRequest(alertasConfigFiltroDto.ParametrosDto).ConfigureAwait(false)) return ResponseMessage(base.RespuestaAutorizacion);
                var result = await _alertasConfigServicios.ObtenerAlertasConfig(alertasConfigFiltroDto).ConfigureAwait(false);
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }
    }
}