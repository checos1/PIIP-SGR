using System;

namespace DNP.Backbone.Web.API.Controllers
{
    using Comunes.Dto;
    using Comunes.Excepciones;
    using DNP.Backbone.Dominio.Dto;
    using DNP.Backbone.Dominio.Dto.Conpes;
    using DNP.Backbone.Dominio.Dto.FuenteFinanciacion;
    using DNP.Backbone.Dominio.Dto.Orfeo;
    using DNP.Backbone.Dominio.Dto.Productos;
    using DNP.Backbone.Dominio.Dto.Proyecto;
    using DNP.Backbone.Dominio.Dto.Tramites;
    using DNP.Backbone.Dominio.Dto.Tramites.Proyectos;
    using DNP.Backbone.Dominio.Dto.Tramites.VigenciaFutura;
    using DNP.Backbone.Dominio.Dto.VigenciasFuturas;
    using DNP.Backbone.Servicios.Interfaces.ServiciosNegocio;
    using DNP.Backbone.Servicios.Interfaces.Tramites;
    using DNP.Backbone.Web.API.Controllers.Base;
    using Servicios.Interfaces.Autorizacion;
    using Swashbuckle.Swagger.Annotations;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;
    using System.Web.Helpers;
    using System.Web.Http;
    using DNP.Backbone.Dominio.Dto.Transferencias;
    using DNP.ServiciosNegocio.Dominio.Dto.Tramites;
    using DNP.Backbone.Dominio.Dto.Tramites.Reprogramacion;

    /// <summary>
    /// Clase Api responsable de la gestión de trámites
    /// </summary>
    public class TramiteController : BackboneBase
    {
        private readonly ITramiteServicios _tramiteServicios;
        private readonly IAutorizacionServicios _autorizacionUtilidades;
        private readonly IServiciosNegocioServicios _serviciosNegocioServicios;

        /// <summary>
        /// Constructor de clases
        /// </summary>
        /// <param name="tramiteServicios">Instancia de servicios de trámites</param>
        /// <param name="autorizacionUtilidades">Instancia de servicios de autorizacion</param>
        /// <param name="serviciosNegocioServicios">Instancia de servicios de Negocio Servicios</param>
        public TramiteController(ITramiteServicios tramiteServicios, IAutorizacionServicios autorizacionUtilidades)
            : base(autorizacionUtilidades)
        {
            _tramiteServicios = tramiteServicios;
            _autorizacionUtilidades = autorizacionUtilidades;
        }

        /// <summary>
        /// Api para obtención de datos de tramites.
        /// </summary>
        /// <param name="instanciaTramiteDto">Contiene informacion de autorizacion, filtro e columnas visibles </param>
        /// <returns>Objeto con propiedades para realizar consulta de datos trámite.</returns>
        [Route("api/Tramite/ObtenerTramites")]
        [HttpPost]
        public async Task<IHttpActionResult> ObtenerTramites(InstanciaTramiteDto instanciaTramiteDto)
        {
            try
            {
                if (!await ValidarParametrosRequest(instanciaTramiteDto.ParametrosInboxDto).ConfigureAwait(false)) return ResponseMessage(base.RespuestaAutorizacion);

                var result = await _tramiteServicios.ObtenerInboxTramites(instanciaTramiteDto).ConfigureAwait(false);
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        /// <summary>
        /// Api para obtención de datos de tramites de programacion.
        /// </summary>
        /// <param name="instanciaTramiteDto">Contiene informacion de autorizacion, filtro e columnas visibles </param>
        /// <returns>Objeto con propiedades para realizar consulta de datos trámite.</returns>
        [Route("api/Tramite/ObtenerTramitesProgramacion")]
        [HttpPost]
        public async Task<IHttpActionResult> ObtenerTramitesProgramacion(InstanciaTramiteDto instanciaTramiteDto)
        {
            try
            {
                if (!await ValidarParametrosRequest(instanciaTramiteDto.ParametrosInboxDto).ConfigureAwait(false)) return ResponseMessage(base.RespuestaAutorizacion);

                var result = await _tramiteServicios.ObtenerInboxTramitesProgramacion(instanciaTramiteDto).ConfigureAwait(false);
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
        [Route("api/Tramite/ObtenerInfoPDF")]
        [HttpPost]
        public async Task<IHttpActionResult> ObtenerInfoPDF(InstanciaTramiteDto instanciaTramiteDto)
        {
            try
            {
                if (!await ValidarParametrosRequest(instanciaTramiteDto.ParametrosInboxDto).ConfigureAwait(false)) return ResponseMessage(base.RespuestaAutorizacion);
                var result = await _tramiteServicios.ObtenerInboxTramites(instanciaTramiteDto).ConfigureAwait(false);

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
        /// Api para obtención de datos de proyectos do tramite.
        /// </summary>
        /// <param name="instanciaTramiteDto">Contiene informacion de autorizacion, filtro e columnas visibles</param>
        /// <returns>Objeto con propiedades para realizar consulta dos datos.</returns>
        [Route("api/Tramite/ObtenerProyectos")]
        [HttpPost]
        public async Task<IHttpActionResult> ObtenerProyectosTramite(InstanciaTramiteDto instanciaTramiteDto)
        {
            try
            {
                if (!await ValidarParametrosRequest(instanciaTramiteDto.ParametrosInboxDto).ConfigureAwait(false)) return ResponseMessage(base.RespuestaAutorizacion);
                var result = await _tramiteServicios.ObtenerProyectosTramite(instanciaTramiteDto).ConfigureAwait(false);
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
        [Route("api/Tramite/ObtenerTramitesExcel")]
        [HttpPost]
        public async Task<HttpResponseMessage> ObtenerTramitesExcel(InstanciaTramiteDto instanciaTramiteDto)
        {
            try
            {
                HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);

                var _result = await _tramiteServicios.ObtenerInboxTramites(instanciaTramiteDto).ConfigureAwait(false);

                if (_result != null)
                    _result.ColumnasVisibles = instanciaTramiteDto.ColumnasVisibles;

                result.Content = ExcelUtilidades.ObtenerExcellTramites(_result, instanciaTramiteDto.TramiteFiltroDto);
                result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
                result.Content.Headers.ContentDisposition.FileName = "Tramite.xlsx";
                return result;
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }
        /// <summary>
        /// Api para generar proyectos del trámite en excel
        /// </summary>
        /// <param name="instanciaTramiteDto">Contiene informacion de autorizacion, filtro e columnas visibles</param>
        /// <returns>Excel con datos generados</returns>
        [Route("api/Tramite/ObtenerProyectosExcel")]
        [HttpPost]
        public async Task<HttpResponseMessage> ObtenerProyectosExcel(InstanciaTramiteDto instanciaTramiteDto)
        {
            try
            {
                HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);

                var _result = await _tramiteServicios.ObtenerProyectosTramite(instanciaTramiteDto).ConfigureAwait(false);

                result.Content = ExcelUtilidades.ObtenerExcellTramitesProyectos(_result);
                result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
                result.Content.Headers.ContentDisposition.FileName = "TramitesProyectos.xlsx";
                return result;
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/Tramite/EliminarProyectoTramite")]
        [HttpPost]
        public async Task<IHttpActionResult> EliminarProyectoTramite(InstanciaTramiteDto instanciaTramiteDto)
        {
            try
            {
                var result = await Task.Run(() => _tramiteServicios.EliminarProyectoTramite(instanciaTramiteDto));
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
        /// <param name="instanciaTramiteDto">Contiene informacion de autorizacion, filtro e columnas visibles </param>
        /// <returns>Objeto con propiedades para realizar consulta de datos trámite.</returns>
        [Route("api/Tramite/ObtenerTramitesConsolaProcesos")]
        [HttpPost]
        public async Task<IHttpActionResult> ObtenerTramitesConsolaProcesos(InstanciaTramiteDto instanciaTramiteDto)
        {
            try
            {
                //if (!await ValidarParametrosRequest(instanciaTramiteDto.ParametrosInboxDto)) return ResponseMessage(base.RespuestaAutorizacion);

                var result = await Task.Run(() => _tramiteServicios.ObtenerInboxTramitesConsolaProcesos(instanciaTramiteDto, User.Identity.Name));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        /// <summary>
        /// Api para obtención de los tipos de tramite.
        /// </summary>
        /// <param name="instanciaTramiteDto">Contiene informacion de autorizacion, filtro e columnas visibles </param>
        /// <returns>Objeto con propiedades para realizar consulta de datos de los tipos de tramite.</returns>
        [Route("api/Tramite/ObtenerTiposTramite")]
        [HttpPost]
        public async Task<IHttpActionResult> ObtenerTiposTramite(InstanciaTramiteDto instanciaTramiteDto)
        {
            try
            {
                //if (!await ValidarParametrosRequest(instanciaTramiteDto.ParametrosInboxDto)) return ResponseMessage(base.RespuestaAutorizacion);

                var result = await Task.Run(() => _tramiteServicios.ObtenerTiposTramites(instanciaTramiteDto, User.Identity.Name));
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
        [Route("api/Tramite/ObtenerTramitesExcelConsolaProcesos")]
        [HttpPost]
        public async Task<HttpResponseMessage> ObtenerTramitesExcelConsolaProcesos(InstanciaTramiteDto instanciaTramiteDto)
        {
            try
            {
                HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);

                var _result = await Task.Run(() => _tramiteServicios.ObtenerInboxTramitesConsolaProcesos(instanciaTramiteDto, User.Identity.Name));

                if (_result != null)
                    _result.ColumnasVisibles = instanciaTramiteDto.ColumnasVisibles;

                result.Content = ExcelUtilidades.ObtenerExcellTramitesConsola(_result, instanciaTramiteDto.TramiteFiltroDto);
                result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
                result.Content.Headers.ContentDisposition.FileName = "Tramite.xlsx";
                return result;
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
        [Route("api/Tramite/ObtenerInfoPDFConsolaProcesos")]
        [HttpPost]
        public async Task<IHttpActionResult> ObtenerInfoPDFConsolaProcesos(InstanciaTramiteDto instanciaTramiteDto)
        {
            try
            {
                if (!await ValidarParametrosRequest(instanciaTramiteDto.ParametrosInboxDto)) return ResponseMessage(base.RespuestaAutorizacion);
                var result = await Task.Run(() => _tramiteServicios.ObtenerInboxTramitesConsolaProcesos(instanciaTramiteDto, User.Identity.Name));

                if (result != null)
                    result.ColumnasVisibles = instanciaTramiteDto.ColumnasVisibles;

                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [HttpGet]
        [Route("api/Tramites/ObtenerProyectosTramiteNegocio")]
        public async Task<IHttpActionResult> ObtenerProyectosTramiteNegocio(int TramiteId)
        {
            try
            {
                var result = await Task.Run(() => _tramiteServicios.ObtenerProyectosTramiteNegocio(TramiteId, User.Identity.Name, Request.Headers.Authorization.Parameter));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [HttpGet]
        [Route("api/Tramites/ObtenerTipoDocumentoTramite")]
        public async Task<IHttpActionResult> ObtenerTipoDocumentoTramite(int TipoTramiteId, int tramiteId = 0, string Rol = null, string nivelId = null)
        {
            try
            {
                string roltmp = string.IsNullOrEmpty(Rol) || Rol.Equals("undefined") ? string.Empty : Rol;
                var result = await Task.Run(() => _tramiteServicios.ObtenerTipoDocumentoTramite(TipoTramiteId, roltmp, tramiteId, User.Identity.Name, Request.Headers.Authorization.Parameter, nivelId));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/Tramites/GuardarProyectosTramiteNegocio")]
        [HttpPost]
        public async Task<IHttpActionResult> GuardarProyectosTramiteNegocio(DatosTramiteProyectosDto ppt)
        {
            try
            {
                var result = await Task.Run(() => _tramiteServicios.GuardarProyectosTramiteNegocio(ppt, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/Tramites/EliminarProyectoTramiteNegocio")]
        [HttpPost]
        public async Task<IHttpActionResult> EliminarProyectoTramiteNegocio(InstanciaTramiteDto instanciaTramiteDto)
        {
            try
            {
                var result = await Task.Run(() => _tramiteServicios.EliminarProyectoTramiteNegocio(instanciaTramiteDto));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/Tramite/ActualizarInstanciaProyecto")]
        [HttpPost]
        public async Task<IHttpActionResult> ActualizarInstanciaProyecto(ProyectosTramiteDto ppt)
        {
            try
            {
                var result = await Task.Run(() => _tramiteServicios.ActualizarInstanciaProyecto(ppt, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [HttpGet]
        [Route("api/Tramites/ObtenerPreguntasJustificacion")]
        public async Task<IHttpActionResult> ObtenerPreguntasJustificacion(int TramiteId, int ProyectoId, int TipoTramiteId, int TipoRolId, string IdNivel)
        {
            try
            {
                //string TokenAutorizacion = "";
                var result = await Task.Run(() => _tramiteServicios.ObtenerPreguntasJustificacion(TramiteId, ProyectoId, TipoTramiteId, TipoRolId, UsuarioLogadoDto.IdUsuario, IdNivel, Request.Headers.Authorization.Parameter));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }


        [Route("api/Tramite/GuardarRespuestasJustificacion")]
        [HttpPost]
        public async Task<IHttpActionResult> GuardarRespuestasJustificacion(List<JustificacionTramiteProyectoDto> parametros)
        {
            try
            {
                var result = await Task.Run(() => _tramiteServicios.GuardarRespuestasJustificacion(parametros, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }


        [Route("api/Tramites/ObtenerProyectoFuentePresupuestalPorTramite")]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerProyectoFuentePresupuestalPorTramite(int pProyectoId, int? pTramiteId, string pTipoProyecto)
        {
            try
            {
                var result = await Task.Run(() => _tramiteServicios.ObtenerProyectoFuentePresupuestalPorTramite(pProyectoId, pTramiteId, pTipoProyecto, User.Identity.Name));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/Tramites/ObtenerProyectoRequisitosPorTramite")]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerProyectoRequisitosPorTramite(int pProyectoId, int? pTramiteId, bool isCDP)
        {
            try
            {
                var result = await Task.Run(() => _tramiteServicios.ObtenerProyectoRequisitosPorTramite(pProyectoId, pTramiteId, User.Identity.Name, isCDP));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/Tramites/ObtenerFuentesInformacionPresupuestal")]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerFuentesInformacionPresupuestal()
        {
            try
            {
                var result = await Task.Run(() => _tramiteServicios.ObtenerFuentesInformacionPresupuestal(User.Identity.Name));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/Tramites/GuardarTramiteInformacionPresupuestal")]
        [HttpPost]
        public async Task<IHttpActionResult> GuardarTramiteInformacionPresupuestal(List<TramiteFuentePresupuestalDto> parametros)
        {
            try
            {
                var result = await Task.Run(() => _tramiteServicios.GuardarTramiteInformacionPresupuestal(parametros, User.Identity.Name));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }


        [Route("api/Tramites/GuardarTramiteTipoRequisito")]
        [HttpPost]
        public async Task<IHttpActionResult> GuardarTramiteTipoRequisito(List<TramiteRequitoDto> parametros)
        {
            try
            {
                var result = await Task.Run(() => _tramiteServicios.GuardarTramiteTipoRequisito(parametros, User.Identity.Name));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }




        [Route("api/Tramite/ActualizarValoresProyecto")]
        [HttpPost]
        public async Task<IHttpActionResult> ActualizarValoresProyecto(ProyectosTramiteDto ppt)
        {
            try
            {
                var result = await Task.Run(() => _tramiteServicios.ActualizarValoresProyecto(ppt, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/Tramite/ObtenerTiposRequisito")]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerTiposRequisito()
        {
            try
            {
                var result = await Task.Run(() => _tramiteServicios.ObtenerTiposRequisito(UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }

        }



        [Route("api/Tramites/ValidarEnviarDatosTramiteNegocio")]
        [HttpPost]
        public async Task<IHttpActionResult> ValidarEnviarDatosTramiteNegocio(DatosTramiteProyectosDto ppt)
        {
            try
            {
                var result = await Task.Run(() => _tramiteServicios.ValidarEnviarDatosTramiteNegocio(ppt, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [HttpGet]
        [Route("api/Tramites/ObtenerPreguntasProyectoActualizacion")]
        public async Task<IHttpActionResult> ObtenerPreguntasProyectoActualizacion(int TramiteId, int ProyectoId, int TipoTramiteId, Guid IdNivel, int TipoRolId)
        {
            try
            {
                //string TokenAutorizacion = "";
                var result = await Task.Run(() => _tramiteServicios.ObtenerPreguntasProyectoActualizacion(TramiteId, ProyectoId, TipoTramiteId, IdNivel, TipoRolId, UsuarioLogadoDto.IdUsuario, Request.Headers.Authorization.Parameter));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [HttpGet]
        [Route("api/Tramites/ObtenerProyectosTramiteNegocioAprobacion")]
        public async Task<IHttpActionResult> ObtenerProyectosTramiteNegocioAprobacion(int TramiteId, int TipoRolId)
        {
            try
            {
                var result = await Task.Run(() => _tramiteServicios.ObtenerProyectosTramiteNegocioAprobacion(TramiteId, TipoRolId, User.Identity.Name, Request.Headers.Authorization.Parameter));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [HttpGet]
        [Route("api/Tramites/ObtenerFuentesTramiteProyectoAprobacion")]
        public async Task<IHttpActionResult> ObtenerFuentesTramiteProyectoAprobacion(int tramiteId, int proyectoId, string pTipoProyecto)
        {
            try
            {
                var result = await Task.Run(() => _tramiteServicios.ObtenerFuentesTramiteProyectoAprobacion(tramiteId, proyectoId, pTipoProyecto, User.Identity.Name, Request.Headers.Authorization.Parameter));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }


        [Route("api/Tramite/GuardarFuentesTramiteProyectoAprobacion")]
        [HttpPost]
        public async Task<IHttpActionResult> GuardarFuentesTramiteProyectoAprobacion(List<FuentesTramiteProyectoAprobacionDto> parametros)
        {
            try
            {
                var result = await Task.Run(() => _tramiteServicios.GuardarFuentesTramiteProyectoAprobacion(parametros, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }


        [Route("api/Tramites/ObtenerCodigoPresupuestal")]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerCodigoPresupuestal(int proyectoId, int entidadId, int tramiteId)
        {
            try
            {
                var result = await Task.Run(() => _tramiteServicios.ObtenerCodigoPresupuestal(proyectoId, entidadId, tramiteId, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/Tramites/ActualizarCodigoPresupuestal")]
        [HttpPost]
        public async Task<IHttpActionResult> ActualizarCodigoPresupuestal(int proyectoId, int entidadId, int tramiteId)
        {
            try
            {
                var result = await Task.Run(() => _tramiteServicios.ActualizarCodigoPresupuestal(proyectoId, entidadId, tramiteId, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/Tramites/ObtenerTarmitesPorProyectoEntidad")]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerTarmitesPorProyectoEntidad(int proyectoId, int entidadId)
        {
            try
            {
                var result = await Task.Run(() => _tramiteServicios.ObtenerTarmitesPorProyectoEntidad(proyectoId, entidadId, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/Tramites/ObtenerValoresProyectos")]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerValoresProyectos(int proyectoId, int tramiteId, int entidadId)
        {
            try
            {
                var result = await Task.Run(() => _tramiteServicios.ObtenerValoresProyectos(proyectoId, tramiteId, entidadId, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        /// <summary>
        /// Api para obtener las preguntas de concepto direccion tecnica
        /// </summary>
        /// <returns>las preguntas de concepto direccion tecnica</returns>
        [Route("api/Tramites/ObtenerConceptoDireccionTecnicaTramite")]
        [HttpPost]
        public async Task<IHttpActionResult> ObtenerConceptoDireccionTecnicaTramite(ParametrosConceptoDicreccionTecnicaDto parametrosConcepto)
        {
            try
            {
                var result = await _tramiteServicios.ObtenerConceptoDireccionTecnicaTramite(parametrosConcepto.TramiteId, parametrosConcepto.NivelId, UsuarioLogadoDto.IdUsuario).ConfigureAwait(false);

                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        /// <summary>
        /// Api para guardar las preguntas de concepto direccion tecnica
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        [Route("api/Tramites/GuardarConceptoDireccionTecnicaTramite")]
        [HttpPost]
        public async Task<IHttpActionResult> GuardarConceptoDireccionTecnicaTramite(List<ConceptoDireccionTecnicaTramiteDto> parametros)
        {
            try
            {
                var result = await Task.Run(() => _tramiteServicios.GuardarConceptoDireccionTecnicaTramite(parametros, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }
        [Route("api/Tramites/ObtenerProyectoConpes")]
        [HttpGet]
        public IHttpActionResult ObtenerProyectoConpes(string conpes)
        {
            try
            {
                //var result = await Task.Run(() => _tramiteServicios.ObtenerProyectoConpes(conpes, UsuarioLogadoDto.IdUsuario));
                var result = _tramiteServicios.ObtenerProyectoConpes(conpes, UsuarioLogadoDto.IdUsuario);
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/Tramites/AdicionarProyectoConpes")]
        [HttpPost]
        public async Task<IHttpActionResult> AdicionarProyectoConpes(CapituloConpes conpes)
        {
            try
            {
                var result = await Task.Run(() => _tramiteServicios.AdicionarProyectoConpes(conpes, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/Tramites/ValidarEnviarDatosTramiteNegocioAprobacion")]
        [HttpPost]
        public async Task<IHttpActionResult> ValidarEnviarDatosTramiteNegocioAprobacion(DatosTramiteProyectosDto ppt)
        {
            try
            {
                var result = await Task.Run(() => _tramiteServicios.ValidarEnviarDatosTramiteNegocioAprobacion(ppt, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/Tramites/ObtenerPlantillaCarta")]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerPlantillaCarta(string nombreSeccion, int tipoTramite)
        {
            try
            {
                var result = await Task.Run(() => _tramiteServicios.ObtenerPlantillaCarta(nombreSeccion, tipoTramite, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/Tramites/ObtenerDatosCartaPorSeccion")]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerDatosCartaPorSeccion(int tramiteId, int plantillaSeccionId)
        {
            try
            {
                var result = await Task.Run(() => _tramiteServicios.ObtenerDatosCartaPorSeccion(tramiteId, plantillaSeccionId, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }


        [Route("api/Tramites/ObtenerCartaConceptoDatosDespedida")]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerCartaConceptoDatosDespedida(int tramiteId, int plantillaCartaSeccionId)
        {
            try
            {
                var result = await Task.Run(() => _tramiteServicios.ObtenerCartaConceptoDatosDespedida(tramiteId, plantillaCartaSeccionId, UsuarioLogadoDto.IdUsuario, Request.Headers.Authorization.Parameter));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/Tramites/ActualizarCartaConceptoDatosDespedida")]
        [HttpPost]
        public async Task<IHttpActionResult> ActualizarCartaConceptoDatosDespedida(DatosConceptoDespedidaDto parametros)
        {
            try
            {
                var result = await Task.Run(() => _tramiteServicios.ActualizarCartaConceptoDatosDespedida(parametros, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/Tramites/VerificaUsuarioDestinatario")]
        [HttpPost]
        public async Task<IHttpActionResult> VerificaUsuarioDestinatario(UsuarioTramite usuarioTramite)
        {
            try
            {
                var result = await Task.Run(() => _tramiteServicios.VerificaUsuarioDestinatario(usuarioTramite, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/Tramites/ActualizarCartaDatosIniciales")]
        [HttpPost]
        public async Task<IHttpActionResult> ActualizarCartaDatosIniciales(Carta parametros)
        {
            try
            {
                var result = await Task.Run(() => _tramiteServicios.ActualizarCartaDatosIniciales(parametros, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/Tramites/ObtenerUsuariosRegistrados")]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerUsuariosRegistrados(int tramiteId, string numeroTramite)
        {
            try
            {
                var result = await Task.Run(() => _tramiteServicios.ObtenerUsuariosRegistrados(tramiteId, numeroTramite, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }


        [Route("api/Tramites/CargarProyectoConpes")]
        [HttpGet]
        public async Task<IHttpActionResult> CargarProyectoConpes(string proyectoid, Guid InstanciaId, string GuiMacroproceso, string NivelId, string FlujoId)
        {
            try
            {
                var result = await Task.Run(() => _tramiteServicios.CargarProyectoConpes(proyectoid, InstanciaId, GuiMacroproceso, UsuarioLogadoDto.IdUsuario, NivelId, FlujoId));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        //[AllowAnonymous]
        [HttpPost]
        [Route("api/Tramites/CargarFirma")]
        public async Task<IHttpActionResult> CargarFirma(FileToUploadDto parametro)
        {
            try
            {
                var result = await Task.Run(() => _tramiteServicios.CargarFirma(parametro.FileAsBase64, parametro.RolId.ToString(), UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }



        [Route("api/Tramites/validarSiExisteFirmaUsuario")]
        [HttpPost]
        public async Task<IHttpActionResult> ValidarSiExisteFirmaUsuario()
        {
            try
            {
                var result = await Task.Run(() => _tramiteServicios.ValidarSiExisteFirmaUsuario(UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/Tramites/Firmar")]
        [HttpPost]
        public async Task<IHttpActionResult> Firmar(int TramiteId, string numeroRadicado)
        {
            RespuestaGeneralDto respuestasalida = new RespuestaGeneralDto();
            try
            {

                respuestasalida = await Task.Run(() => _tramiteServicios.Firmar(TramiteId, numeroRadicado, UsuarioLogadoDto.IdUsuario));
                return Ok(respuestasalida);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }


        [Route("api/Tramites/EliminarProyectoConpes")]
        [HttpGet]
        public async Task<IHttpActionResult> EliminarProyectoConpes(string proyectoid, string conpesid)
        {
            try
            {
                var result = await Task.Run(() => _tramiteServicios.EliminarProyectoConpes(proyectoid, conpesid, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/Tramites/ObtenerCuerpoConceptoCDP")]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerCuerpoConceptoCDP(int tramiteId)
        {
            try
            {
                var result = await Task.Run(() => _tramiteServicios.ObtenerCuerpoConceptoCDP(tramiteId, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/Tramites/ObtenerCuerpoConceptoAutorizacion")]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerCuerpoConceptoAutorizacion(int tramiteId)
        {
            try
            {
                var result = await Task.Run(() => _tramiteServicios.ObtenerCuerpoConceptoAutorizacion(tramiteId, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }


        [Route("api/Tramites/ConsultarCarta")]
        [HttpGet]
        public async Task<IHttpActionResult> ConsultarCarta(int tramiteId)
        {
            try
            {
                var result = await Task.Run(() => _tramiteServicios.ConsultarCarta(tramiteId, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/Tramites/ReasignarRadicadoORFEO")]
        [HttpPost]
        public async Task<IHttpActionResult> ReasignarRadicadoORFEO(ReasignacionRadicadoDto reasignacionRadicado)
        {
            try
            {
                var result = await Task.Run(() => _tramiteServicios.ReasignarRadicadoORFEO(reasignacionRadicado, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }
        [Route("api/Tramites/ActualizarEstadoAjusteProyecto")]
        [HttpPost]
        public async Task<IHttpActionResult> ActualizarEstadoAjusteProyecto(string tipoDevolucion, string objetoNegocioId, int tramiteId, string observacion)
        {
            try
            {
                var result = await Task.Run(() => _tramiteServicios.ActualizarEstadoAjusteProyecto(tipoDevolucion, objetoNegocioId, tramiteId, observacion, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/Tramites/TramiteEnPasoUno")]
        [HttpGet]
        public async Task<IHttpActionResult> TramiteEnPasoUno(Guid InstanciaId)
        {
            try
            {
                var result = await Task.Run(() => _tramiteServicios.TramiteEnPasoUno(InstanciaId, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }


        [Route("api/Tramites/ObtenerConpesAsociados/{tramiteId}")]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerConpesAsociados(int tramiteId)
        {
            var queryConpes = await _tramiteServicios.ObtenerTramiteConpes(tramiteId, UsuarioLogadoDto.IdUsuario);
            if (queryConpes.Estado)
            {
                return Json(queryConpes);
            }

            throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, queryConpes.Mensaje));
        }

        [Route("api/Tramites/AsociarConpesTramite")]
        [HttpPost]
        public async Task<IHttpActionResult> AsociarConpesTramite([FromBody] AsociarConpesTramiteRequestDto model)
        {
            var queryConpes = await _tramiteServicios.AsociarTramiteConpes(model, UsuarioLogadoDto.IdUsuario);
            if (queryConpes.Estado)
            {
                return Ok(queryConpes);
            }

            throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, queryConpes.Mensaje));
        }

        [Route("api/Tramites/RemoverAsociacionConpesTramite")]
        [HttpPost]
        public async Task<IHttpActionResult> RemoverAsociacionConpesTramite([FromBody] RemoverAsociacionConpesTramiteDto model)
        {
            var queryConpes = await _tramiteServicios.RemoverAsociacionConpes(model, UsuarioLogadoDto.IdUsuario);
            if (queryConpes.Estado)
            {
                return Ok(queryConpes);
            }

            throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, queryConpes.Mensaje));
        }

        [Route("api/Tramites/ObtenerTramitePorInstancia/{instanciaId}")]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerTramitePorInstancia(string instanciaId)
        {
            var tramiteDetail = await _tramiteServicios.ObtenerDetalleTramitePorInstancia(instanciaId, UsuarioLogadoDto.IdUsuario);
            if (tramiteDetail.Estado)
            {
                return Ok(tramiteDetail);
            }

            throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, tramiteDetail.Mensaje));
        }

        [Route("api/Tramites/ValidarConpesTramiteVigenciaFutura/{tramiteId}")]
        [HttpGet]
        public async Task<IHttpActionResult> ValidarConpesTramiteVigenciaFutura(string tramiteId)
        {
            var tramiteDetail = await _tramiteServicios.ValidarConpesTramiteVigenciaFutura(tramiteId, UsuarioLogadoDto.IdUsuario);
            if (tramiteDetail.Estado)
            {
                return Ok(tramiteDetail);
            }

            throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, tramiteDetail.Mensaje));
        }

        [Route("api/Tramites/EliminarAsociacionVFOPLN")]
        [HttpPost]
        public async Task<IHttpActionResult> EliminarAsociacionVFO(EliminacionAsociacionDto eliminacionAsociacionDto)
        {
            try
            {
                var result = await Task.Run(() => _tramiteServicios.EliminarAsociacionVFO(eliminacionAsociacionDto, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/Tramites/ObtenerProyectoAsociacionVFO")]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerProyectoAsociacionVFO(string bpin, int tramiteid, string tipoTramite)
        {
            try
            {
                var result = await Task.Run(() => _tramiteServicios.ObtenerProyectoAsociacionVFO(bpin, tramiteid, tipoTramite, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/Tramites/AsociarProyectoVFO")]
        [HttpPost]
        public async Task<IHttpActionResult> AsociarProyectoVFO(TramiteProyectoVFODto tramiteProyectoVFODto)
        {
            try
            {
                var result = await Task.Run(() => _tramiteServicios.AsociarProyectoVFO(tramiteProyectoVFODto, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [HttpGet]
        [Route("api/Tramites/ObtenerDatosProyectoTramite")]
        public async Task<IHttpActionResult> ObtenerDatosProyectoTramite(int tramiteId)
        {
            try
            {
                var result = await Task.Run(() => _tramiteServicios.ObtenerDatosProyectoTramite(tramiteId, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/Tramites/CrearRadicadoEntrada")]
        [HttpPost]
        public async Task<IHttpActionResult> CrearRadicadoEntrada([FromBody] CrearRadicadoRequestDto model)
        {
            try
            {
                var result = await _tramiteServicios.CrearRadicadoEntradaTramite(model.tramiteId, UsuarioLogadoDto.IdUsuario);
                return Ok();
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [HttpGet]
        [Route("api/Tramites/ObtenerDeflactores")]
        public async Task<IHttpActionResult> ObtenerDeflactores()
        {
            try
            {
                var result = await Task.Run(() => _tramiteServicios.ObtenerDeflactores(UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [HttpGet]
        [Route("api/Tramites/ObtenerProyectoTramite")]
        public async Task<IHttpActionResult> ObtenerProyectoTramite(int ProyectoId, int TramiteId)
        {
            try
            {
                var result = await Task.Run(() => _tramiteServicios.ObtenerProyectoTramite(ProyectoId, TramiteId, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [HttpPost]
        [Route("api/Tramites/ActualizaVigenciaFuturaProyectoTramite")]
        public async Task<IHttpActionResult> ActualizaVigenciaFuturaProyectoTramite(TramiteProyectoDto tramiteProyectoDto)
        {
            try
            {
                var result = await Task.Run(() => _tramiteServicios.ActualizaVigenciaFuturaProyectoTramite(tramiteProyectoDto, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [HttpGet]
        [Route("api/Tramites/ObtenerFuentesFinanciacionVigenciaFuturaCorriente")]
        public async Task<IHttpActionResult> ObtenerFuentesFinanciacionVigenciaFuturaCorriente(string bpin)
        {
            try
            {
                var result = await Task.Run(() => _tramiteServicios.ObtenerFuentesFinanciacionVigenciaFuturaCorriente(bpin, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [HttpGet]
        [Route("api/Tramites/ObtenerFuentesFinanciacionVigenciaFuturaConstante")]
        public async Task<IHttpActionResult> ObtenerFuentesFinanciacionVigenciaFuturaConstante(string bpin, int TramiteId)
        {
            try
            {
                var result = await Task.Run(() => _tramiteServicios.ObtenerFuentesFinanciacionVigenciaFuturaConstante(bpin, TramiteId, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/Tramites/ActualizarVigenciaFuturaFuente")]
        [HttpPost]
        public async Task<IHttpActionResult> ActualizarVigenciaFuturaFuente(VigenciaFuturaCorrienteFuenteDto fuente)
        {
            try
            {
                var result = await Task.Run(() => _tramiteServicios.ActualizarVigenciaFuturaFuente(fuente, User.Identity.Name));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/Tramites/ActualizarVigenciaFuturaProducto")]
        [HttpPost]
        public async Task<IHttpActionResult> ActualizarVigenciaFuturaProducto(ProductosConstantesVFDetalleProducto prod)
        {
            try
            {
                var result = await Task.Run(() => _tramiteServicios.ActualizarVigenciaFuturaProducto(prod, User.Identity.Name));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [HttpGet]
        [Route("api/Tramites/ObtenerDatosProyectosPorTramite")]
        public async Task<IHttpActionResult> ObtenerDatosProyectosPorTramite(int tramiteId)
        {
            try
            {
                var result = await Task.Run(() => _tramiteServicios.ObtenerDatosProyectosPorTramite(tramiteId, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }
        #region Vigencias Futuras

        [Route("api/Tramites/ObtenerSolicitarConceptoPorTramite")]
        [HttpPost]
        public async Task<IHttpActionResult> ObtenerSolicitarConceptoPorTramite(int tramiteId)
        {

            try
            {
                var result = await Task.Run(() => _tramiteServicios.ObtenerSolicitarConceptoPorTramite(tramiteId, UsuarioLogadoDto.IdUsuario));

                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [HttpGet]
        [Route("api/Tramites/ObtenerDatosCronograma")]
        public async Task<IHttpActionResult> ObtenerDatosCronograma(Guid instanciaId)
        {
            try
            {
                var result = await Task.Run(() => _tramiteServicios.ObtenerDatosCronograma(instanciaId, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [HttpGet]
        [Route("api/Tramites/ObtenerPreguntasProyectoActualizacionPaso")]
        public async Task<IHttpActionResult> ObtenerPreguntasProyectoActualizacionPaso(int TramiteId, int ProyectoId, int TipoTramiteId, Guid IdNivel, int TipoRolId)
        {
            try
            {

                var result = await Task.Run(() => _tramiteServicios.ObtenerPreguntasProyectoActualizacionPaso(TramiteId, ProyectoId, TipoTramiteId, IdNivel, TipoRolId, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [HttpGet]
        [Route("api/Tramites/ObtenerInformacionPresupuestalValores")]
        public async Task<IHttpActionResult> ObtenerInformacionPresupuestalValores(int tramiteId)
        {
            try
            {
                var result = await Task.Run(() => _tramiteServicios.ObtenerInformacionPresupuestalValores(tramiteId, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }

        }

        [HttpPost]
        [Route("api/Tramites/GuardarInformacionPresupuestalValores")]
        public async Task<IHttpActionResult> GuardarInformacionPresupuestalValores(InformacionPresupuestalValoresDto informacionPresupuestalValoresDto)
        {
            try
            {
                var result = await Task.Run(() => _tramiteServicios.GuardarInformacionPresupuestalValores(informacionPresupuestalValoresDto, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }
        #endregion Vigencias Futuras



        [Route("api/Tramites/GenerarRadicadoSalida")]
        [HttpGet]
        public async Task<IHttpActionResult> GenerarRadicadoSalida(string numeroTramite, string numeroRadicadoSalida)
        {
            try
            {
                ResponseDto<RadicadoSalidaRequestDto> respuestaSalida = new ResponseDto<RadicadoSalidaRequestDto>();
                RadicadoSalidaRequestDto radicadosalida = new RadicadoSalidaRequestDto();
                respuestaSalida.Data = radicadosalida;
                bool tieneradicado = false;

                if (!string.IsNullOrEmpty(numeroRadicadoSalida) && numeroRadicadoSalida != "0" && numeroRadicadoSalida != null)
                {
                    var resultadoradicado = await Task.Run(() => _tramiteServicios.ConsultarRadicado(numeroRadicadoSalida, UsuarioLogadoDto.IdUsuario));
                    if (resultadoradicado.Estado && resultadoradicado.Data)
                    {
                        respuestaSalida.Estado = true;
                        tieneradicado = true;
                        respuestaSalida.Data.RadicadoSalida = numeroRadicadoSalida;
                    }
                }
                if (!tieneradicado)
                {
                    radicadosalida.NumeroTramite = numeroTramite;
                    var resultadocrearradicado = await Task.Run(() => _tramiteServicios.CrearRadicadoSalida(radicadosalida, UsuarioLogadoDto.IdUsuario));
                    if (resultadocrearradicado != null)
                    {
                        respuestaSalida.Data.RadicadoSalida = resultadocrearradicado.RadicadoId;
                        respuestaSalida.Data.NumeroExpediente = resultadocrearradicado.ExpedienteId;
                        respuestaSalida.Data.NumeroTramite = numeroTramite;
                        respuestaSalida.Estado = true;
                        tieneradicado = true;
                    }
                    else
                    {
                        respuestaSalida.Mensaje = "No se puede generar el radicado de salida";
                        respuestaSalida.Estado = false;
                    }
                }
                if (!tieneradicado)
                {
                    respuestaSalida.Mensaje = "El trámite no tiene radicado de salida";
                    respuestaSalida.Estado = false;
                }

                return Ok(respuestaSalida);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/Tramites/Cerrar_CargarDocumentoElectronicoOrfeo")]
        [HttpPost]
        public async Task<IHttpActionResult> Cerrar_CargarDocumentoElectronicoOrfeo(DatosDocumentoElectronicoDSDto parametros)
        {
            RespuestaGeneralDto respuestasalida = new RespuestaGeneralDto();
            parametros.datosRadicadoDto.esPrincipal = true;
            string numeroRadicado = decimal.Truncate(parametros.datosRadicadoDto.NoRadicado).ToString();
            var resultado = await Task.Run(() => _tramiteServicios.CargarDocumentoElectronicoOrfeo(parametros, UsuarioLogadoDto.IdUsuario));
            if (resultado.Estado)
            {
                var resultadoCerrar = await _tramiteServicios.CerrarRadicadosTramite(parametros.NumeroTramite, UsuarioLogadoDto.IdUsuario);

                respuestasalida.Mensaje = resultado.Mensaje;
                respuestasalida.Exito = resultado.Estado;
            }
            else
            {
                respuestasalida.Mensaje = resultado.Mensaje;
                respuestasalida.Exito = resultado.Estado;
            }
            return Ok(respuestasalida);
        }

        [Route("api/Tramites/EliminarPermisosAccionesUsuarios")]
        [HttpPost]
        public async Task<IHttpActionResult> EliminarPermisosAccionesUsuarios(string usuarioDestino, int tramiteId, string aliasNivel, Guid InstanciaId = default(Guid))
        {
            try
            {
                var rta = await _tramiteServicios.EliminarPermisosAccionesUsuarios(usuarioDestino, tramiteId, aliasNivel, UsuarioLogadoDto.IdUsuario, InstanciaId);
                return Ok(rta);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/Tramites/ObtenerAccionActualyFinal")]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerAccionActualyFinal(int tramiteId, string bpin)
        {
            try
            {
                return Ok(await _tramiteServicios.ObtenerAccionActualyFinal(tramiteId, bpin, UsuarioLogadoDto.IdUsuario));
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/Tramites/EnviarConceptoDireccionTecnicaTramite")]
        [HttpPost]
        public async Task<IHttpActionResult> EnviarConceptoDireccionTecnicaTramite(int tramiteId, string usuarioDnp)
        {
            try
            {
                return Ok(await _tramiteServicios.EnviarConceptoDireccionTecnicaTramite(tramiteId, usuarioDnp, UsuarioLogadoDto.IdUsuario));
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }
        [HttpGet]
        [Route("api/Tramites/ObtenerModalidadesContratacion")]
        public async Task<IHttpActionResult> ObtenerModalidadesContratacion(int mostrar)
        {
            try
            {
                var result = await Task.Run(() => _tramiteServicios.ObtenerModalidadesContratacion(mostrar, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [HttpPost]
        [Route("api/Tramites/ActualizarActividadesCronograma")]
        public async Task<IHttpActionResult> ActualizarActividadesCronograma(ActividadPreContractualDto ModalidadContratacionId)
        {
            try
            {
                var result = await Task.Run(() => _tramiteServicios.ActualizarActividadesCronograma(ModalidadContratacionId, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }
        [HttpGet]
        [Route("api/Tramites/ObtenerActividadesPrecontractualesProyectoTramite")]
        public async Task<IHttpActionResult> ObtenerActividadesPrecontractualesProyectoTramite(int ModalidadContratacionId, int ProyectoId, int TramiteId, bool eliminarActividades)
        {
            try
            {
                var result = await Task.Run(() => _tramiteServicios.ObtenerActividadesPrecontractualesProyectoTramite(ModalidadContratacionId, ProyectoId, TramiteId, eliminarActividades, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [HttpGet]
        [Route("api/Tramites/ObtenerProductosVigenciaFuturaConstante")]
        public async Task<IHttpActionResult> ObtenerProductosVigenciaFuturaConstante(string Bpin, int TramiteId, int AnioBase)
        {
            try
            {
                var result = await Task.Run(() => _tramiteServicios.ObtenerProductosVigenciaFuturaConstante(Bpin, TramiteId, AnioBase, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [HttpGet]
        [Route("api/Tramites/ObtenerProductosVigenciaFuturaCorriente")]
        public async Task<IHttpActionResult> ObtenerProductosVigenciaFuturaCorriente(string Bpin, int TramiteId)
        {
            try
            {
                var result = await Task.Run(() => _tramiteServicios.ObtenerProductosVigenciaFuturaCorriente(Bpin, TramiteId, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [HttpGet]
        [Route("api/Tramites/ObtenerTipoDocumentoTramitePorNivel")]
        public async Task<IHttpActionResult> ObtenerTipoDocumentoTramitePorNivel(int tipoTramiteId, string nivelId, string rolId = null)
        {
            try
            {
                string roltmp = string.IsNullOrEmpty(rolId) || rolId.Equals("undefined") ? string.Empty : rolId;
                var result = await Task.Run(() => _tramiteServicios.ObtenerTipoDocumentoTramitePorNivel(tipoTramiteId, nivelId, roltmp, User.Identity.Name));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [HttpGet]
        [Route("api/Tramites/ObtenerDatosUsuario")]
        public async Task<IHttpActionResult> ObtenerDatosUsuario(string idUsuarioDnp, int idEntidad, Guid idAccion, Guid idIntancia)
        {
            try
            {
                var result = await Task.Run(() => _tramiteServicios.ObtenerDatosUsuario(idUsuarioDnp, idEntidad, idAccion, idIntancia, User.Identity.Name));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [HttpGet]
        [Route("api/Tramites/ObtenerModificacionLeyenda")]
        public async Task<IHttpActionResult> ObtenerModificacionLeyenda(int tramiteId, int proyectoId)
        {
            try
            {
                var result = await Task.Run(() => _tramiteServicios.ObtenerModificacionLeyenda(tramiteId, proyectoId, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [HttpPost]
        [Route("api/Tramites/ActualizarModificacionLeyenda")]
        public async Task<IHttpActionResult> ActualizarModificacionLeyenda(ModificacionLeyendaDto modificacionLeyendaDto)
        {
            try
            {
                var result = await Task.Run(() => _tramiteServicios.ActualizarModificacionLeyenda(modificacionLeyendaDto, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [HttpGet]
        [Route("api/Tramites/ObtenerListaDirecciones")]
        public async Task<IHttpActionResult> ObtenerListaDirecciones(Guid idEntididad)
        {
            try
            {
                var result = await Task.Run(() => _tramiteServicios.ObtenerListaDirecionesDNP(idEntididad, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [HttpGet]
        [Route("api/Tramites/ObtenerListaSubdirecciones")]
        public async Task<IHttpActionResult> ObtenerListaSubdirecciones(int idEntididadType)
        {
            {
                try
                {
                    var result = await Task.Run(() => _tramiteServicios.ObtenerListaSubdirecionesPorParentId(idEntididadType, UsuarioLogadoDto.IdUsuario));
                    return Ok(result);
                }
                catch (BackboneException e)
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
                }
            }
        }

        //[AllowAnonymous]
        [HttpPost]
        [Route("api/Tramites/BorrarFirma")]
        public async Task<IHttpActionResult> BorrarFirma(FileToUploadDto parametro)
        {
            try
            {
                var result = await Task.Run(() => _tramiteServicios.BorrarFirma(UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [HttpGet]
        [Route("api/Tramites/ObtenerProyectosCartaTramite")]
        public async Task<IHttpActionResult> ObtenerProyectosCartaTramite(int tramiteId)
        {
            {
                try
                {
                    var result = await Task.Run(() => _tramiteServicios.ObtenerProyectosCartaTramite(tramiteId, UsuarioLogadoDto.IdUsuario));
                    return Ok(result);
                }
                catch (BackboneException e)
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
                }
            }
        }

        [HttpGet]
        [Route("api/Tramites/ObtenerDetalleCartaAL")]
        public async Task<IHttpActionResult> ObtenerDetalleCartaAL(int tramiteId)
        {
            {
                try
                {
                    var result = await Task.Run(() => _tramiteServicios.ObtenerDetalleCartaAL(tramiteId, UsuarioLogadoDto.IdUsuario));
                    return Ok(result);
                }
                catch (BackboneException e)
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
                }
            }
        }

        [HttpGet]
        [Route("api/Tramites/ObtenerAmpliarDevolucionTramite")]
        public async Task<IHttpActionResult> ObtenerAmpliarDevolucionTramite(int ProyectoId, int TramiteId)
        {
            {
                try
                {
                    var result = await Task.Run(() => _tramiteServicios.ObtenerAmpliarDevolucionTramite(ProyectoId, TramiteId, UsuarioLogadoDto.IdUsuario));
                    return Ok(result);
                }
                catch (BackboneException e)
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
                }
            }
        }
        [HttpGet]
        [Route("api/Tramites/ObtenerDatosProyectoConceptoPorInstancia")]
        public async Task<IHttpActionResult> ObtenerDatosProyectoConceptoPorInstancia(Guid instanciaId)
        {
            try
            {
                var result = await Task.Run(() => _tramiteServicios.ObtenerDatosProyectoConceptoPorInstancia(instanciaId, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [HttpGet]
        [Route("api/Tramites/ObtenerLiberacionVigenciasFuturas")]
        public async Task<IHttpActionResult> ObtenerLiberacionVigenciasFuturas(int ProyectoId, int TramiteId)
        {
            try
            {
                var result = await Task.Run(() => _tramiteServicios.ObtenerLiberacionVigenciasFuturas(ProyectoId, TramiteId, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [HttpPost]
        [Route("api/Tramites/InsertaAutorizacionVigenciasFuturas")]
        public async Task<IHttpActionResult> InsertaAutorizacionVigenciasFuturas(TramiteALiberarVfDto autorizacion)
        {
            try
            {
                var result = await Task.Run(() => _tramiteServicios.InsertaAutorizacionVigenciasFuturas(autorizacion, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [HttpPost]
        [Route("api/Tramites/InsertaValoresUtilizadosLiberacionVF")]
        public async Task<IHttpActionResult> InsertaValoresUtilizadosLiberacionVF(TramiteALiberarVfDto autorizacion)
        {
            try
            {
                /*var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name, RequestContext.Principal.Identity.GetHashCode().ToString(), ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"], ConfigurationManager.AppSettings["GestionTramitesProyectos"]).Result;
                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);
                */
                string usuario = RequestContext.Principal.Identity.Name;
                var result = await Task.Run(() => _tramiteServicios.InsertaValoresUtilizadosLiberacionVF(autorizacion, usuario));
                return Ok(result);
            }
            catch (Exception e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }


        [HttpGet]
        [Route("api/TramitesProyectos/ObtenerListaProyectosFuentes")]
        public async Task<IHttpActionResult> ObtenerListaProyectosFuentes(int tramiteId)
        {
            try
            {
                var result = await Task.Run(() => _tramiteServicios.ObtenerListaProyectosFuentes(tramiteId, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }


        [HttpGet]
        [Route("api/Tramites/obtenerEntidadAsociarProyecto")]
        public async Task<IHttpActionResult> obtenerEntidadAsociarProyecto(Guid InstanciaId, string AccionTramiteProyecto)
        {
            try
            {
                var result = await Task.Run(() => _tramiteServicios.obtenerEntidadAsociarProyecto(InstanciaId, AccionTramiteProyecto, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }
        [HttpGet]
        [Route("api/TramitesProyectos/ConsultarCartaConcepto")]
        public async Task<IHttpActionResult> ConsultarCartaConcepto(int TramiteId)
        {
            {
                try
                {
                    var result = await Task.Run(() => _tramiteServicios.ConsultarCartaConcepto(TramiteId, UsuarioLogadoDto.IdUsuario));
                    return Ok(result);
                }
                catch (BackboneException e)
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
                }
            }
        }

        [HttpGet]
        [Route("api/TramitesProyectos/ValidacionPeriodoPresidencial")]

        public async Task<IHttpActionResult> ValidacionPeriodoPresidencial(int tramiteid)
        {
            try
            {
                var result = await Task.Run(() => _tramiteServicios.ValidacionPeriodoPresidencial(tramiteid, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (Exception e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [HttpPost]
        [Route("api/Tramites/GuardarMontosTramite")]
        public async Task<IHttpActionResult> GuardarMontosTramite(List<ProyectosEnTramiteDto> proyectosEnTramiteDto)
        {
            try
            {
                var result = await Task.Run(() => _tramiteServicios.GuardarMontosTramite(proyectosEnTramiteDto, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [HttpGet]
        [Route("api/Tramites/ObtenerPreguntasJustificacionPorProyectos")]
        public async Task<IHttpActionResult> ObtenerPreguntasJustificacionPorProyectos(int TramiteId, int TipoTramiteId, int TipoRolId, Guid IdNivel)
        {
            try
            {
                var result = await Task.Run(() => _tramiteServicios.ObtenerPreguntasJustificacionPorProyectos(TramiteId, TipoTramiteId, TipoRolId, IdNivel, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [HttpGet]
        [Route("api/Tramites/ObtenerTramitesVFparaLiberar")]

        public async Task<IHttpActionResult> ObtenerTramitesVFparaLiberar(string numtramite)
        {
            try
            {
                var result = await Task.Run(() => _tramiteServicios.ObtenerTramitesVFparaLiberar(numtramite, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (Exception e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [HttpPost]
        [Route("api/Tramites/GuardarLiberacionVigenciaFutura")]
        public async Task<IHttpActionResult> GuardarLiberacionVigenciaFutura(LiberacionVigenciasFuturasDto liberacionVigenciasFuturasDto)
        {
            try
            {
                var result = await Task.Run(() => _tramiteServicios.GuardarLiberacionVigenciaFutura(liberacionVigenciasFuturasDto, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [HttpGet]
        [Route("api/Tramites/ObtenerResumenLiberacionVigenciasFuturas")]
        public async Task<IHttpActionResult> ObtenerResumenLiberacionVigenciasFuturas(int ProyectoId, int TramiteId)
        {
            try
            {
                var result = await Task.Run(() => _tramiteServicios.ObtenerResumenLiberacionVigenciasFuturas(ProyectoId, TramiteId, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [HttpGet]
        [Route("api/Tramites/ObtenerValUtilizadosLiberacionVigenciasFuturas")]
        public async Task<IHttpActionResult> ObtenerValUtilizadosLiberacionVigenciasFuturas(int ProyectoId, int TramiteId)
        {
            try
            {
                var result = await Task.Run(() => _tramiteServicios.ObtenerValUtilizadosLiberacionVigenciasFuturas(ProyectoId, TramiteId, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/Tramites/TramiteAjusteEnPasoUno")]
        [HttpGet]
        public async Task<IHttpActionResult> TramiteAjusteEnPasoUno(int tramiteId, int proyectoId)
        {
            try
            {
                var result = await Task.Run(() => _tramiteServicios.TramiteAjusteEnPasoUno(tramiteId, proyectoId, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/Tramites/ObtenerListaProyectosFuentesAprobado")]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerListaProyectosFuentesAprobado(int tramiteId)
        {
            try
            {
                var result = await Task.Run(() => _tramiteServicios.ObtenerListaProyectosFuentesAprobado(tramiteId, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [HttpPost]
        [Route("api/Tramites/ActualizarCargueMasivo")]
        public async Task<IHttpActionResult> ActualizarCargueMasivo(ObjetoNegocioDto contenido)
        {
            try
            {
                var result = await Task.Run(() => _tramiteServicios.ActualizarCargueMasivo(contenido, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/Tramites/ConsultarCargueExcel")]
        [HttpPost]
        public async Task<IHttpActionResult> ConsultarCargueExcel(ObjetoNegocioDto contenido)
        {
            try
            {
                var result = await Task.Run(() => _tramiteServicios.ConsultarCargueExcel(contenido, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [HttpPost]
        [Route("api/Tramites/InsertaValoresproductosLiberacionVFCorrientes")]
        public async Task<IHttpActionResult> InsertaValoresproductosLiberacionVFCorrientes(DetalleProductosCorrientesDto productosCorrientes)
        {
            try
            {
                /*var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name, RequestContext.Principal.Identity.GetHashCode().ToString(), ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"], ConfigurationManager.AppSettings["GestionTramitesProyectos"]).Result;
                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);
                */
                string usuario = RequestContext.Principal.Identity.Name;
                var result = await Task.Run(() => _tramiteServicios.InsertaValoresproductosLiberacionVFCorrientes(productosCorrientes, usuario));
                return Ok(result);
            }
            catch (Exception e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [HttpPost]
        [Route("api/Tramites/InsertaValoresproductosLiberacionVFConstantes")]
        public async Task<IHttpActionResult> InsertaValoresproductosLiberacionVFConstantes(DetalleProductosConstantesDto productosConstantes)
        {
            try
            {
                /*var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name, RequestContext.Principal.Identity.GetHashCode().ToString(), ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"], ConfigurationManager.AppSettings["GestionTramitesProyectos"]).Result;
                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);
                */
                string usuario = RequestContext.Principal.Identity.Name;
                var result = await Task.Run(() => _tramiteServicios.InsertaValoresproductosLiberacionVFConstantes(productosConstantes, usuario));
                return Ok(result);
            }
            catch (Exception e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [HttpGet]
        [Route("api/Tramites/ObtenerEntidadTramite")]
        public async Task<IHttpActionResult> ObtenerEntidadTramite(string numeroTramite)
        {
            try
            {
                var result = await Task.Run(() => _tramiteServicios.ObtenerEntidadTramite(numeroTramite, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [HttpPost]
        [Route("api/Tramites/EliminarLiberacionVigenciaFutura")]
        public async Task<IHttpActionResult> EliminarLiberacionVigenciaFutura(LiberacionVigenciasFuturasDto eliminarLiberacionVigenciasFuturasDto)
        {
            try
            {
                var result = await Task.Run(() => _tramiteServicios.EliminarLiberacionVigenciaFutura(eliminarLiberacionVigenciasFuturasDto, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [HttpGet]
        [Route("api/Tramites/ObtenerCalendarioPeriodo")]
        public async Task<IHttpActionResult> ObtenerCalendarioPeriodo(string bpin)
        {
            try
            {
                bpin = string.IsNullOrEmpty(bpin) ? string.Empty : bpin;
                var result = await Task.Run(() => _tramiteServicios.ObtenerCalendartioPeriodo(bpin, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [HttpGet]
        [Route("api/Tramites/ObtenerPresupuestalProyectosAsociados")]
        public async Task<IHttpActionResult> ObtenerPresupuestalProyectosAsociados(int TramiteId, Guid InstanciaId)
        {
            try
            {
                var result = await Task.Run(() => _tramiteServicios.ObtenerPresupuestalProyectosAsociados(TramiteId, InstanciaId, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [HttpGet]
        [Route("api/Tramites/ObtenerPresupuestalProyectosAsociados_Adicion")]
        public async Task<IHttpActionResult> ObtenerPresupuestalProyectosAsociados_Adicion(int TramiteId, Guid InstanciaId)
        {
            try
            {
                var result = await Task.Run(() => _tramiteServicios.ObtenerPresupuestalProyectosAsociados_Adicion(TramiteId, InstanciaId, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [HttpGet]
        [Route("api/Tramites/ObtenerResumenReprogramacionPorVigencia")]
        public async Task<IHttpActionResult> ObtenerResumenReprogramacionPorVigencia(int TramiteId, Guid InstanciaId, int ProyectoId)
        {
            try
            {
                var result = await Task.Run(() => _tramiteServicios.ObtenerResumenReprogramacionPorVigencia(TramiteId, InstanciaId, ProyectoId, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [HttpPost]
        [Route("api/Tramites/GuardarDatosReprogramacion")]
        public async Task<IHttpActionResult> GuardarDatosReprogramacion(DatosReprogramacionDto Reprogramacion)
        {
            try
            {
                var result = await Task.Run(() => _tramiteServicios.GuardarDatosReprogramacion(Reprogramacion, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [HttpGet]
        [Route("api/Tramites/GetOrigenRecursosTramite")]
        public async Task<IHttpActionResult> GetOrigenRecursosTramite(int TramiteId)
        {
            try
            {
                var result = await Task.Run(() => _tramiteServicios.GetOrigenRecursosTramite(TramiteId, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [HttpGet]
        [Route("api/Tramites/ConsultarSystemConfiguracion")]
        public async Task<IHttpActionResult> ConsultarSystemConfiguracion(string VariableKey, string Separador)
        {
            try
            {
                var result = await Task.Run(() => _tramiteServicios.ConsultarSystemConfiguracion(VariableKey, Separador, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [HttpPost]
        [Route("api/Tramites/SetOrigenRecursosTramite")]
        public async Task<IHttpActionResult> SetOrigenRecursosTramite(OrigenRecursosDto origenRecurso)
        {
            try
            {
                var result = await Task.Run(() => _tramiteServicios.SetOrigenRecursosTramite(origenRecurso, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [HttpGet]
        [Route("api/Tramites/ObtenerResumenReprogramacionPorProductoVigencia")]
        public async Task<IHttpActionResult> ObtenerResumenReprogramacionPorProductoVigencia(Guid InstanciaId, int TramiteId, int? ProyectoId)
        {
            try
            {
                var result = await Task.Run(() => _tramiteServicios.ObtenerResumenReprogramacionPorProductoVigencia(InstanciaId, TramiteId, ProyectoId, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }
        [HttpGet]
        [Route("api/Tramites/ObtenerModalidadContratacionVigenciasFuturas")]
        public async Task<IHttpActionResult> ObtenerModalidadContratacionVigenciasFuturas(int ProyectoId, int TramiteId)
        {
            try
            {
                var result = await Task.Run(() => _tramiteServicios.ObtenerModalidadContratacionVigenciasFuturas(ProyectoId, TramiteId, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/Tramites/ObtenerAutorizacionesParaReprogramacion")]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerAutorizacionesParaReprogramacion(string bpin, int tramiteid, string tipoTramite)
        {
            try
            {
                var result = await Task.Run(() => _tramiteServicios.ObtenerAutorizacionesParaReprogramacion(bpin, tramiteid, tipoTramite, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/Tramites/AsociarAutorizacionRVF")]
        [HttpPost]
        public async Task<IHttpActionResult> AsociarAutorizacionRVF(tramiteRVFAsociarproyecto reprogramacionDto)
        {
            try
            {
                var result = await Task.Run(() => _tramiteServicios.AsociarAutorizacionRVF(reprogramacionDto, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [HttpGet]
        [Route("api/Tramites/ObtenerAutorizacionAsociada")]
        public async Task<IHttpActionResult> ObtenerAutorizacionAsociada(int tramiteId)
        {
            try
            {
                var result = await Task.Run(() => _tramiteServicios.ObtenerAutorizacionAsociada(tramiteId, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [HttpPost]
        [Route("api/Tramites/EliminaReprogramacionVF")]
        public async Task<IHttpActionResult> EliminaReprogramacionVF(ReprogramacionDto reprogramacionDto)
        {
            try
            {
                var result = await Task.Run(() => _tramiteServicios.EliminaReprogramacionVF(reprogramacionDto, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }

        }

    }
}

