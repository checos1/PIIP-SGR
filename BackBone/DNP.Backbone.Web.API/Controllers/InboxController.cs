namespace DNP.Backbone.Web.API.Controllers
{
    using System;
    using Comunes.Dto;
    using Comunes.Excepciones;
    using Comunes.Properties;
    using System.Configuration;
    using System.Net;
    using System.Threading.Tasks;
    using System.Web.Http;
    using Servicios.Interfaces.Autorizacion;
    using Servicios.Interfaces.Inbox;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using DNP.Backbone.Servicios.Interfaces.ServiciosNegocio;
    using DNP.Backbone.Comunes.Enums;
    using System.Linq;
    using DNP.Backbone.Servicios.Interfaces.Tramites;
    using DNP.Backbone.Dominio.Dto.Inbox;

    /// <summary>
    /// Clase responsable de la gestión de proyectos
    /// </summary>
    public class InboxController : Base.BackboneBase
    {
        private readonly IInboxServicios _inboxServicios;
        private readonly IAutorizacionServicios _autorizacionUtilidades;
        private readonly ITramiteServicios _tramiteServicios;
        private readonly IServiciosNegocioServicios _serviciosNegocioServicios;

        /// <summary>
        /// Constructor de clases
        /// </summary>
        /// <param name="inboxServicios">Instancia de servicios de inbox</param>
        /// <param name="autorizacionUtilidades">Instancia de servicios de autorizacion</param>
        /// <param name="serviciosNegocioServicios">Instancia de servicios de Negocio Servicios</param>
        public InboxController(IInboxServicios inboxServicios, IAutorizacionServicios autorizacionUtilidades, IServiciosNegocioServicios serviciosNegocioServicios, ITramiteServicios tramiteServicios)
            : base(autorizacionUtilidades)
        {
            _inboxServicios = inboxServicios;
            _autorizacionUtilidades = autorizacionUtilidades;
            _tramiteServicios = tramiteServicios;
            _serviciosNegocioServicios = serviciosNegocioServicios;
        }

        /// <summary>
        /// Api para obtención de datos de proyectos en Excel.
        /// </summary>
        /// <param name="instanciaInboxDto">Contiene informacion de autorizacion, filtro e columnas visibles</param>
        /// <returns>Excel con datos generados</returns>
        [Route("api/Inbox/ObtenerExcel")]
        [HttpPost]
        public async Task<HttpResponseMessage> ObtenerExcel(InstanciaInboxDto instanciaInboxDto)
        {
            try
            {
                HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);

                //ValidarParametrosRequest(peticionObtenerInbox);
                var _result = await Task.Run(() => _inboxServicios.ObtenerInbox(instanciaInboxDto.ParametrosInboxDto, Request.Headers.Authorization.Parameter, instanciaInboxDto.ProyectoFiltroDto));
                _result.ColumnasVisibles = instanciaInboxDto.ColumnasVisibles;

                result.StatusCode = HttpStatusCode.OK;
                result.Content = ExcelUtilidades.ObtenerExcell(_result);
                result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
                //result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment; filename = Proyectos.xlsx");

                return result;
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }            
        }

        /// <summary>
        /// Api para obtención de datos inbox por idUsuario, idObjeto y aplicación.
        /// </summary>
        /// <param name="peticionObtenerInbox"></param>
        /// <returns>Objeto con propiedades para realizar consulta de datos inbox.</returns>
        [Route("api/Inbox/ObtenerInbox")]
        [HttpPost]
        public async Task<IHttpActionResult> ObtenerInbox(InstanciaInboxDto instanciaInboxDto)
        {
            try
            {
                if (!await ValidarParametrosRequest(instanciaInboxDto.ParametrosInboxDto).ConfigureAwait(false)) return ResponseMessage(base.RespuestaAutorizacion);
                var result = await _inboxServicios.ObtenerInbox(instanciaInboxDto.ParametrosInboxDto, Request.Headers.Authorization.Parameter, instanciaInboxDto.ProyectoFiltroDto).ConfigureAwait(false);
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }
        
        /// <summary>
        /// Api para obtención de datos inbox por idUsuario, idObjeto y aplicación.
        /// </summary>
        /// <param name="instanciaInboxDto"></param>
        /// <returns></returns>
        [Route("api/Inbox/ObtenerInfoPDF")]
        [HttpPost]
        public async Task<IHttpActionResult> ObtenerProyectosTramitesPDF(InstanciaInboxDto instanciaInboxDto)
        {
            try
            {
                if (!await ValidarParametrosRequest(instanciaInboxDto.ParametrosInboxDto).ConfigureAwait(false)) return ResponseMessage(base.RespuestaAutorizacion);
                var result = await _inboxServicios.ObtenerInbox(instanciaInboxDto.ParametrosInboxDto, Request.Headers.Authorization.Parameter, instanciaInboxDto.ProyectoFiltroDto).ConfigureAwait(false);

                if (result != null)
                    result.ColumnasVisibles = instanciaInboxDto.ColumnasVisibles;

                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }
    }
}