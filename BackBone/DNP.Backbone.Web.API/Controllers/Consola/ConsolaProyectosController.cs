using DNP.Backbone.Comunes.Dto;
using DNP.Backbone.Comunes.Enums;
using DNP.Backbone.Comunes.Excepciones;
using DNP.Backbone.Dominio.Dto.Proyecto;
using DNP.Backbone.Servicios.Interfaces.Autorizacion;
using DNP.Backbone.Servicios.Interfaces.Consola;
using DNP.Backbone.Servicios.Interfaces.Proyectos;
using DNP.Backbone.Servicios.Interfaces.ServiciosNegocio;
using DNP.Backbone.Web.API.Controllers.Base;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;

namespace DNP.Backbone.Web.API.Controllers
{
    public class ConsolaProyectosController : BackboneBase
    {
        #region Atributos

        private readonly IAutorizacionServicios _autorizacionUtilidades;
        private readonly IConsolaProyectosServicio _consolaProyectoServicio;
        private readonly IServiciosNegocioServicios _servicioNegocioServicios;

        #endregion Atributos

        #region Constructor

        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="consolaProyectoServicios">Instancia de servicios de consola de proyectos</param>
        /// <param name="autorizacionUtilidades">Instancia de sericios de autorización</param>
        /// <param name="serviciosNegocioServicios">Instancia de servicios de Negocio de Servicios</param>
        public ConsolaProyectosController(IConsolaProyectosServicio consolaProyectoServicios,
            IAutorizacionServicios autorizacionUtilidades,
            IServiciosNegocioServicios serviciosNegocioServicios)
            : base(autorizacionUtilidades)
        {
            this._consolaProyectoServicio = consolaProyectoServicios;
            this._autorizacionUtilidades = autorizacionUtilidades;
            this._servicioNegocioServicios = serviciosNegocioServicios;
        }

        #endregion Constructor

        #region Métodos

        /// <summary>
        /// Api para obtención de datos por idUsuario, idObjeto y aplicación.
        /// </summary>
        /// <param name="instanciaProyectoDto"></param>
        /// <returns>Objeto con propiedades para realizar consulta de datos de proyectos.</returns>
        [Route("api/ConsolaProyecto/ObtenerProyectos")]
        [HttpPost]
        public async Task<IHttpActionResult> ObtenerProyectos(InstanciaProyectoDto instanciaProyectoDto)
        {
            try
            {
                //if (!await ValidarParametrosRequest(instanciaProyectoDto.ProyectoParametrosDto).ConfigureAwait(false))
                //    return ResponseMessage(base.RespuestaAutorizacion);
                var result = await _consolaProyectoServicio.ObtenerProyectos(instanciaProyectoDto.ProyectoParametrosDto, Request.Headers.Authorization.Parameter, instanciaProyectoDto.ProyectoFiltroDto).ConfigureAwait(false);
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        /// <summary>
        ///  Obtiene un content result de un archivo binario de un Excel de los datos de proyectos /consola
        /// </summary>
        /// <param name="datos">Informacion  a obtener en el archivo Excel como una instancia de la clase <see cref="Dominio.Dto.Proyecto.ProyectoDto"/>.</param>
        /// <returns></returns>
        [Route("api/ConsolaProyecto/ExcelConsolaProyectos")]
        [HttpPost]
        public IHttpActionResult ObtenerExcelConsolaProyectos(Dominio.Dto.Proyecto.ProyectoDto datos)
        {
            try
            {
                // obtener el binario del archivo excel.
                var archivo       = ExcelUtilidades.ObtenerExcelConsolaProyecto(datos);
                var nombreArchivo = $"{nombreDelArchivo("ConsolaProyectos")}.xlsx";

                return Ok(new { 
                    Datos = new { 
                        FileContent = archivo,
                        ContentType = System.Net.Mime.MediaTypeNames.Application.Octet,
                        FileName    = nombreArchivo,
                    },
                    EsExcepcion = false
                });
            }
            catch (Exception excepcion) {
                return Ok(new { 
                    EsExcepcion = true,
                    ExcepcionMensaje = $"ConsolaProyectos.ObtenerExcelConsolaProyectos: {excepcion.Message}\\n{excepcion.InnerException ? .Message ?? String.Empty}"
                });
            }
        }

        /// <summary>
        /// Método que devuelve el nombre del archivo concatenando fecha y hora
        /// </summary>
        /// <param name="fuente"></param>
        /// <returns>String: Devuelve el nombre del archivo</returns>
        private string nombreDelArchivo(string fuente)
        {
            var data = $"{ DateTime.Now.Year }-{ DateTime.Now.Month.ToString().PadLeft(2, '0')}-{ DateTime.Now.Day.ToString().PadLeft(2, '0')}";
            var hora = $"{ DateTime.Now.TimeOfDay.Hours}h{ DateTime.Now.TimeOfDay.Minutes}m{DateTime.Now.TimeOfDay.Seconds.ToString().PadLeft(2, '0')}";
            return $"{fuente}_{data}_{hora}";
        }

        /// <summary>
        /// Api para lista de entidades.
        /// </summary>
        /// <param name="peticionObtenerProyecto">Contiene informacion de autorizacion</param>
        /// <returns>Lista de entidades</returns>
        [Route("api/ConsolaProyecto/ObtenerListaEntidades")]
        [HttpPost]
        public async Task<IHttpActionResult> ObtenerListaEntidades(ProyectoParametrosDto peticionObtenerProyecto)
        {
            try
            {
                if (!await ValidarParametrosRequest(peticionObtenerProyecto).ConfigureAwait(false))
                    return ResponseMessage(base.RespuestaAutorizacion);
                var result = await _servicioNegocioServicios.ObtenerListaCatalogo(peticionObtenerProyecto, CatalogoEnum.Entidades).ConfigureAwait(false);

                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }


        /// <summary>
        ///  Inserta un nuevo registro de cambio de entidades del proyecto actual
        /// </summary>
        /// <returns></returns>
        [Route("api/ConsolaProyecto/InsertarAuditoria")]
        [HttpPost]
        public async Task<IHttpActionResult> InsertAuditoriaEntidadProyecto([FromBody] JObject values) {
            try
            {
                var tipoAnonimo = new { EntidadOrigenId = 0, EntidadOrigen = String.Empty, EntidadDestinoId = 0, EntidadDestino = String.Empty, ProyectoId = 0, SectorId = 0 };
                tipoAnonimo = Newtonsoft.Json.JsonConvert.DeserializeAnonymousType(values["auditoria"].ToString(), tipoAnonimo);

                var tipoAnonimoUsuario = new { IdPIIP = new Guid(), NombreCuenta = String.Empty };
                tipoAnonimoUsuario     = Newtonsoft.Json.JsonConvert.DeserializeAnonymousType(values["usuario"].ToString(), tipoAnonimoUsuario);

                var historial = new Dominio.Dto.Proyecto.AuditoriaEntidadDto {

                    EntidadOrigenId  = tipoAnonimo.EntidadOrigenId,
                    EntidadOrigen    = tipoAnonimo.EntidadOrigen,
                    EntidadDestinoId = tipoAnonimo.EntidadDestinoId,
                    EntidadDestino = tipoAnonimo.EntidadDestino,
                    Proyecto = new Dominio.Dto.Proyecto.ProyectoEntidadDto {
                        ProyectoId = tipoAnonimo.ProyectoId,
                        SectorId   = tipoAnonimo.SectorId,
                    },
                    UsuarioId = tipoAnonimoUsuario.IdPIIP,
                    Usuario   = tipoAnonimoUsuario.NombreCuenta,
                };

                var result = await _consolaProyectoServicio.InsertarAuditoria(historial, historial.Usuario).ConfigureAwait(false);
                return Ok(result);
            }
            catch (BackboneException exception) {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, exception));
            }
        }

        /// <summary>
        ///     Obtiene el historial del cambio de entidades del proyecto actual
        /// </summary>
        /// <param name="proyectoId">Identificador del proyecto actual</param>
        /// <param name="usuarioDNP">Nombre de la cuenta de usuario que ejecuta el cambio</param>
        /// <returns></returns>
        [Route("api/ConsolaProyecto/ObtenerAuditoriaEntidad")]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerAuditoriaEntidadProyecto(int proyectoId, String usuarioDNP) {
            try {
                var result = await _consolaProyectoServicio.ObtenerAuditoriaEntidadProyecto(proyectoId, usuarioDNP).ConfigureAwait(false);
                return Ok(result);
            }
            catch (BackboneException exception)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, exception));
            }
        }

        /// <summary>
        /// Api para lista de sectores.
        /// </summary>
        /// <param name="peticion">Contiene informacion de autorizacion</param>
        /// <returns>Lista de sectores</returns>
        [Route("api/ConsolaProyecto/ObtenerListaSectores")]
        [HttpPost]
        public async Task<IHttpActionResult> ObtenerListaSectores(ProyectoParametrosDto peticion)
        {
            try
            {
                if (!await ValidarParametrosRequest(peticion).ConfigureAwait(false)) return ResponseMessage(base.RespuestaAutorizacion);
                var result = await _servicioNegocioServicios.ObtenerListaCatalogo(peticion, CatalogoEnum.Sectores).ConfigureAwait(false);

                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        /// <summary>
        /// Api para lista de estado del proyectos.
        /// </summary>
        /// <param name="peticion">Contiene informacion de autorizacion</param>
        /// <returns>Lista de estado del proyectos</returns>
        [Route("api/ConsolaProyecto/ObtenerListaEstadoProyecto")]
        [HttpPost]
        public async Task<IHttpActionResult> ObtenerListaEstadoProyecto(ProyectoParametrosDto peticion)
        {
            try
            {
                if (!await ValidarParametrosRequest(peticion).ConfigureAwait(false)) return ResponseMessage(base.RespuestaAutorizacion);
                var result = await _servicioNegocioServicios.ObtenerListaEstado(peticion).ConfigureAwait(false);

                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        /// <summary>
        /// Api para lista de vigencias de los proyectos.
        /// </summary>
        /// <param name="peticion">Contiene informacion de autorizacion</param>
        /// <returns>Lista de estado del proyectos</returns>
        [Route("api/ConsolaProyecto/ObtenerVigenciasProyectos")]
        [HttpPost]
        public async Task<IHttpActionResult> ObtenerVigenciasProyecto(ProyectoParametrosDto peticion)
        {
            try
            {
                if (!await ValidarParametrosRequest(peticion).ConfigureAwait(false)) return ResponseMessage(base.RespuestaAutorizacion);
                var result = await _servicioNegocioServicios.ObtenerListaVigenciasProyecto(peticion).ConfigureAwait(false);

                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        /// <summary>
        /// Api para obtención de datos por idUsuario, idObjeto y aplicación.
        /// </summary>
        /// <param name="instanciaProyectoDto"></param>
        /// <returns>Objeto con propiedades para realizar consulta de datos de proyectos.</returns>
        [Route("api/ConsolaProyecto/ObtenerInstanciasProyectos")]
        [HttpPost]
        public async Task<IHttpActionResult> ObtenerInstanciasProyectos(InstanciaProyectoDto instanciaProyectoDto)
        {
            try
            {
                if (!await ValidarParametrosRequest(instanciaProyectoDto.ProyectoParametrosDto).ConfigureAwait(false))
                    return ResponseMessage(RespuestaAutorizacion);
                var result = await _consolaProyectoServicio.ObtenerInstanciasProyectos(instanciaProyectoDto.ProyectoParametrosDto, Request.Headers.Authorization.Parameter, instanciaProyectoDto.ProyectoFiltroDto).ConfigureAwait(false);
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }


        /// <summary>
        ///     Obtiene lista docummentos adjuntos
        /// </summary>
        /// <param name="proyectoId">Identificador del proyecto actual</param>
        /// <param name="usuarioDNP">Nombre de la cuenta de usuario que ejecuta el cambio</param>
        /// <returns></returns>
        [Route("api/ConsolaProyecto/ObtenerDocumentosAdjuntos")]
        [HttpGet]
        public IHttpActionResult ObtenerDocumentosAdjuntos(string idProyecto)
        {
            try
            {
                idProyecto = $"/projects/{idProyecto}";

                var result = _consolaProyectoServicio.ObtenerDocumentosAdjuntos(idProyecto);

                return Ok(result);
            }
            catch (BackboneException exception)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, exception));
            }
        }
        
        /// <summary>
        /// Obtiene PDF
        /// </summary>
        /// <param name="proyectoId">Identificador del proyecto actual</param>
        /// <param name="usuarioDNP">Nombre de la cuenta de usuario que ejecuta el cambio</param>
        /// <returns></returns>
        [Route("api/ConsolaProyecto/ObtenerDocumentoAdjunto")]
        [HttpPost]
        public HttpResponseMessage ObtenerDocumentoAdjunto(ProjectDocumentSimpleDto dto)
        {
            try
            {
                dto.IdProyecto = $"/projects/{dto.IdProyecto}";

                HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);

                //result.Content = ExcelUtilidades.ObtenerExcellComum(_result);
                result.Content = _consolaProyectoServicio.ObtenerDocumentoAdjunto(dto.IdProyecto, dto.NombreArchivo);
                result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
                return result;


              
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        /// <summary>
        /// Obtiene el idAplicacion partiendo del BPIN
        /// </summary>
        /// <param name="idObjetoNegocio">BPIN</param>
        /// <returns>IdAplicacion</returns>
        [Route("api/ConsolaProyecto/ObtenerIdAplicacionPorBpin")]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerIdAplicacionPorBpin([FromUri] string idObjetoNegocio)
        {
            var result = await Task.Run(() => _consolaProyectoServicio.ObtenerIdAplicacionPorBpin(idObjetoNegocio, UsuarioLogadoDto.IdUsuario));
            return Ok(result);
        }
        #endregion Métodos
    }
}