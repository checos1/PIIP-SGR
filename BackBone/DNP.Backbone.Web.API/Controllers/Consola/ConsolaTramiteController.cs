namespace DNP.Backbone.Web.API.Controllers
{
    using Comunes.Dto;
    using Comunes.Excepciones;
    using DNP.Backbone.Servicios.Interfaces.Consola;
    using DNP.Backbone.Web.API.Controllers.Base;
    using Servicios.Interfaces.Autorizacion;
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;
    using System.Web.Http;

    /// <summary>
    /// Clase Api responsable de la gestión de trámites
    /// </summary>
    public class ConsolaTramiteController : BackboneBase
    {
        private readonly IConsolaTramiteServicios _consolaTramiteServicios;
        private readonly IAutorizacionServicios _autorizacionUtilidades;

        /// <summary>
        /// Constructor de clases
        /// </summary>
        /// <param name="consolaTramiteServicios">Instancia de servicios de trámites</param>
        /// <param name="autorizacionUtilidades">Instancia de servicios de autorizacion</param>
        /// <param name="serviciosNegocioServicios">Instancia de servicios de Negocio Servicios</param>
        public ConsolaTramiteController(IConsolaTramiteServicios consolaTramiteServicios, IAutorizacionServicios autorizacionUtilidades)
            : base(autorizacionUtilidades)
        {
            _consolaTramiteServicios = consolaTramiteServicios;
            _autorizacionUtilidades = autorizacionUtilidades;
        }

        /// <summary>
        /// Api para obtención de datos de tramites.
        /// </summary>
        /// <param name="instanciaTramiteDto">Contiene informacion de autorizacion, filtro e columnas visibles </param>
        /// <returns>Objeto con propiedades para realizar consulta de datos trámite.</returns>
        [Route("api/ConsolaTramite/ObtenerConsolaTramites")]
        [HttpPost]
        public async Task<IHttpActionResult> ObtenerConsolaTramites(InstanciaTramiteDto instanciaTramiteDto)
        {
            try
            {
                if (!await ValidarParametrosRequest(instanciaTramiteDto.ParametrosInboxDto).ConfigureAwait(false)) return ResponseMessage(base.RespuestaAutorizacion);

                var result = await _consolaTramiteServicios.ObtenerConsolaTramites(instanciaTramiteDto).ConfigureAwait(false);
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        /// <summary>
        /// Api para obtención de datos de tramites.
        /// </summary>
        /// <param name="instanciaTramiteDto">Contiene informacion de autorizacion, filtro e columnas visibles</param>
        /// <returns>Objeto con propiedades para realizar consulta de datos trámite.</returns>
        [Route("api/ConsolaTramite/ObtenerInfoPDF")]
        [HttpPost]
        public async Task<IHttpActionResult> ObtenerInfoPDF(InstanciaTramiteDto instanciaTramiteDto)
        {
            try
            {
                if (!await ValidarParametrosRequest(instanciaTramiteDto.ParametrosInboxDto).ConfigureAwait(false)) return ResponseMessage(base.RespuestaAutorizacion);
                var result = await _consolaTramiteServicios.ObtenerConsolaTramites(instanciaTramiteDto).ConfigureAwait(false);

                if (result != null)
                    result.ColumnasVisibles = instanciaTramiteDto.ColumnasVisibles;

                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        /// <summary>
        /// Api para generar trámites en excel
        /// </summary>
        /// <param name="instanciaTramiteDto">Contiene informacion de autorizacion, filtro e columnas visibles</param>
        /// <returns>Excel con datos generados</returns>
        [Route("api/ConsolaTramite/ObtenerConsolaTramitesExcel")]
        [HttpPost]
        public async Task<HttpResponseMessage> ObtenerTramitesExcel(InstanciaTramiteDto instanciaTramiteDto)
        {
            try
            {
                HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);

                var _result = await Task.Run(() => _consolaTramiteServicios.ObtenerConsolaTramites(instanciaTramiteDto));

                if (_result != null)
                    _result.ColumnasVisibles = instanciaTramiteDto.ColumnasVisibles;

                result.Content = ExcelUtilidades.ObtenerExcellConsolaTramites(_result);
                result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
                result.Content.Headers.ContentDisposition.FileName = "ConsolaTramite.xlsx";
                return result;
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        /// <summary>
        /// Api para obtención de datos de proyectos do tramite.
        /// </summary>
        /// <param name="instanciaTramiteDto">Contiene informacion de autorizacion, filtro e columnas visibles</param>
        /// <returns>Objeto con propiedades para realizar consulta dos datos.</returns>
        [Route("api/ConsolaTramite/ObtenerProyectos")]
        [HttpPost]
        public async Task<IHttpActionResult> ObtenerProyectosTramite(InstanciaTramiteDto instanciaTramiteDto)
        {
            try
            {
                if (!await ValidarParametrosRequest(instanciaTramiteDto.ParametrosInboxDto).ConfigureAwait(false)) return ResponseMessage(base.RespuestaAutorizacion);
                var result = await _consolaTramiteServicios.ObtenerProyectosTramite(instanciaTramiteDto).ConfigureAwait(false);
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        /// <summary>
        /// Api para obtención de Archivos.
        /// </summary>
        /// <param name="instanciaTramiteDto">Contiene informacion de autorizacion, filtro e columnas visibles</param>
        /// <returns>Objeto con propiedades para realizar consulta dos datos.</returns>
        [Route("api/ConsolaTramite/ObtenerDocumentos")]
        [HttpPost]
        public async Task<IHttpActionResult> ObtenerDocumentos([FromUri] string idInstancia)
        {
            try
            {
                var result = await Task.Run(() => _consolaTramiteServicios.ObtenerDocumentos(idInstancia, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }
    

        [Route("api/ConsolaTramite/ObtenerProyectosExcel")]
        [HttpPost]
        public async Task<HttpResponseMessage> ObtenerProyectosExcel(InstanciaTramiteDto instanciaTramiteDto)
        {
            try
            {
                HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);

                var _result = await Task.Run(() => _consolaTramiteServicios.ObtenerProyectosTramite(instanciaTramiteDto));

                result.Content = ExcelUtilidades.ObtenerExcellConsolaTramitesProyectos(_result);
                result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
                result.Content.Headers.ContentDisposition.FileName = "ConsolaTramitesProyectos.xlsx";
                return result;
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        /// <summary>
        /// Obtiene el IdAplicacion a partir del identificador de la instancia
        /// </summary>
        /// <param name="idInstancia">Identificador de la instancia</param>
        /// <returns>IdAplicacion</returns>
        [Route("api/ConsolaProyecto/ObtenerIdAplicacionPorInstancia")]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerIdAplicacionPorInstancia([FromUri] Guid idInstancia)
        {
            var result = await Task.Run(() => _consolaTramiteServicios.ObtenerIdAplicacionPorInstancia(idInstancia, UsuarioLogadoDto.IdUsuario));
            return Ok(result);
        }


        [Route("api/ConsolaProyecto/ObtenerMacroProcesosCantidad")]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerMacroProcesosCantidad()
        {
            var result = await Task.Run(() => _consolaTramiteServicios.ObtenerMacroProcesosCantidad(UsuarioLogadoDto.IdUsuario));
            return Ok(result);
        }

        [Route("api/ConsolaProyecto/ObtenerMacroProcesos")]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerMacroProcesos()
        {
            var result = await Task.Run(() => _consolaTramiteServicios.ObtenerMacroProcesos(UsuarioLogadoDto.IdUsuario));
            return Ok(result);
        }

        [Route("api/ConsolaProyecto/ObtenerProcesos")]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerProcesos()
        {
            var result = await Task.Run(() => _consolaTramiteServicios.ObtenerProcesos(UsuarioLogadoDto.IdUsuario));
            return Ok(result);
        }

    }
}