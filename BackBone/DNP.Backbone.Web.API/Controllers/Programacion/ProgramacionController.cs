using System;

namespace DNP.Backbone.Web.API.Controllers
{
    using Comunes.Dto;
    using Comunes.Excepciones;
    using Comunes.Properties;
    using System.Configuration;
    using System.Net;
    using System.Threading.Tasks;
    using System.Web.Http;
    using Servicios.Interfaces.Autorizacion;
    using System.Net.Http;
    using DNP.Backbone.Servicios.Interfaces.ServiciosNegocio;
    using DNP.Backbone.Servicios.Interfaces.Tramites;
    using DNP.Backbone.Web.API.Controllers.Base;
    using System.Net.Http.Headers;
    using System.Web.Management;
    using DNP.Backbone.Servicios.Interfaces.Monitoreo;
    using DNP.Backbone.Servicios.Interfaces.Nivel;
    using DNP.Backbone.Servicios.Interfaces.Programacion;
    using DNP.Backbone.Dominio.Dto.Programacion;
    using JsonConvert = Newtonsoft.Json.JsonConvert;
    using DNP.Backbone.Dominio.Dto.Tramites.ProgramacionDistribucion;
    using System.Collections.Generic;
    using DNP.Backbone.Dominio.Dto.Focalizacion;
    using DNP.Backbone.Servicios.Implementaciones.Tramites;
    using static System.Windows.Forms.VisualStyles.VisualStyleElement;

    /// <summary>
    /// Clase Api responsable de la gestión de trámites
    /// </summary>
    public class ProgramacionController : BackboneBase
    {
        private readonly IFlujoServicios _flujoServicios;
        private readonly IAutorizacionServicios _autorizacionUtilidades;
        private readonly IProgramacionServicios _programacionServicios;

        /// <summary>
        /// Constructor de clases
        /// </summary>
        /// <param name="flujoServicios">Instancia de servicios de flujos</param>
        /// <param name="autorizacionUtilidades">Instancia de servicios de autorización</param>
        /// <param name="programacionServicios">Instancia de servicios de programación</param>
        public ProgramacionController(IFlujoServicios flujoServicios, IAutorizacionServicios autorizacionUtilidades, IProgramacionServicios programacionServicios)
            : base(autorizacionUtilidades)
        {
            this._flujoServicios = flujoServicios;
            this._autorizacionUtilidades = autorizacionUtilidades;
            this._programacionServicios = programacionServicios;
        }

        #region Trámites
        /// <summary>
        /// Obtiene la lista de trámites por nivel
        /// </summary>
        /// <param name="idNivel">Nivel seleccionado</param>
        /// <returns>Lista de trámites por nivel</returns>
        [HttpGet]
        [Route("api/programacion/ObtenerFlujoTramitesPorNivel")]
        public async Task<IHttpActionResult> ObtenerPorIdPadreIdNivelTipo([FromUri] Guid idNivel)
        {
            var result = await Task.Run(() => _flujoServicios.ObtenerListaFlujosTramitePorNivel(idNivel, UsuarioLogadoDto.IdUsuario));
            return Ok(result);
        }

        #endregion Trámites

        #region Programación

        /// <summary>
        /// Api para obtención de datos de tramites.
        /// </summary>
        /// <param name="instanciaTramiteDto">Contiene informacion de autorizacion, filtro e columnas visibles </param>
        /// <returns>Objeto con propiedades para realizar consulta de datos trámite.</returns>
        [Route("api/Programacion/ObtenerProgramacionConsolaProcesos")]
        [HttpPost]
        public async Task<IHttpActionResult> ObtenerProgramacionConsolaProcesos(InstanciaTramiteDto instanciaTramiteDto)
        {
            try
            {
                //if (!await ValidarParametrosRequest(instanciaTramiteDto.ParametrosInboxDto)) return ResponseMessage(base.RespuestaAutorizacion);

                var result = await Task.Run(() => _programacionServicios.ObtenerInboxProgramacionConsolaProcesos(instanciaTramiteDto, User.Identity.Name));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }
        /// <summary>
        /// Obtener la lista de programaciones por tipo de entidad
        /// </summary>
        /// <param name="tipoEntidad">Tipo de entidad</param>
        /// <returns>Lista de programaciones por tipo de entidad</returns>
        [HttpGet]
        [Route("api/programacion/ObtenerProgramacionesPorTipoEntidad")]
        public async Task<IHttpActionResult> ObtenerProgramacionesPorTipoEntidad([FromUri] object filtros)
        {
            // definición de tipo
            var tipoAnonimo = new
            {
                TipoEntidad = String.Empty,
                CapituloId = (Guid?)null,
                FechaInicio = (DateTime?)null,
                FechaFin = (DateTime?)null,
                ProcesoTipo = (int?)null
            };
            // deserializar parametro HTTP
            var tipoAnonimoResultado = JsonConvert.DeserializeAnonymousType(filtros.ToString(), tipoAnonimo);

            Dominio.Enums.EstadoProceso? estado = tipoAnonimoResultado.ProcesoTipo.HasValue ? (Dominio.Enums.EstadoProceso?)(tipoAnonimoResultado.ProcesoTipo) : null;

            var result = await Task.Run(() => _programacionServicios .ObtenerProgramaciones(
                                                       tipoAnonimoResultado.TipoEntidad,
                                                       tipoAnonimoResultado.CapituloId,
                                                       tipoAnonimoResultado.FechaInicio,
                                                       tipoAnonimoResultado.FechaFin,
                                                       estado,
                                                       UsuarioLogadoDto.IdUsuario));
            return Ok(result);
        }

        /// <summary>
        ///  
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("api/programacion/ObtenerEstados")]
        public async Task<IHttpActionResult> ObtenerEstadoProcesos()
        {

            try
            {
                var result = await Task.Run(() => _programacionServicios.ObtenerTipoEstadoProceso());

                return Ok(result);
            }
            catch (BackboneException exception)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, exception));
            }
        }

        /// <summary>
        /// Guarda una programación
        /// </summary>
        /// <param name="programacionDto">Datos de la programación</param>
        /// <returns>Resultado de la operación de guardado</returns>
        [HttpPost]
        [Route("api/Programacion/GuardarProgramacion")]
        public async Task<IHttpActionResult> GuardarProgramacion(ProgramacionDto programacionDto)
        {
            try
            {
                var respuesta = await Task.Run(() => _programacionServicios.GuardarProgramacion(programacionDto, UsuarioLogadoDto.IdUsuario));
                return Ok(respuesta);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        /// <summary>
        /// Eliminar una programación
        /// </summary>
        /// <param name="programacionDto">Programación a eliminar</param>
        /// <returns>Resultado de la operación eliminación</returns>
        [HttpPost]
        [Route("api/Programacion/EliminarProgramacion")]
        public async Task<IHttpActionResult> EliminarProgramacion(ProgramacionDto programacionDto)
        {
            try
            {
                var respuesta = await Task.Run(() => _programacionServicios.EliminarProgramacion(programacionDto, UsuarioLogadoDto.IdUsuario));
                return Ok(respuesta);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [HttpPost]
        [Route("api/Programacion/CrearPeriodo")]
        public async Task<IHttpActionResult> CrearPeriodo([FromUri] string tipoEntidad)
        {
            try
            {
                var respuesta = await Task.Run(() => _programacionServicios.CrearPeriodo(tipoEntidad, UsuarioLogadoDto.IdUsuario));
                return Ok(respuesta);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [HttpPost]
        [Route("api/Programacion/IniciarProceso")]
        public async Task<IHttpActionResult> IniciarProceso([FromUri] string tipoEntidad)
        {
            try
            {
                var respuesta = await Task.Run(() => _programacionServicios.IniciarProceso(tipoEntidad, UsuarioLogadoDto.IdUsuario));
                return Ok(respuesta);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [HttpPost]
        [Route("api/Programacion/GuardarConfiguracionMensaje")]
        public async Task<IHttpActionResult> GuardarConfiguracionMensaje(dynamic configuracionMensaje)
        {
            try
            {
                var respuesta = await Task.Run(() => _programacionServicios.GuardarConfiguracionMensaje(configuracionMensaje, UsuarioLogadoDto.IdUsuario));
                return Ok(respuesta);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [HttpGet]
        [Route("api/Programacion/ValidarCalendarioProgramacion")]
        public async Task<IHttpActionResult> ValidarCalendarioProgramacion(int? entityTypeCatalogOptionId, Nullable<Guid> nivelId, Nullable<int> seccionCapituloId)
        {
            try
            {
                var respuesta = await Task.Run(() => _programacionServicios.ValidarCalendarioProgramacion(entityTypeCatalogOptionId,nivelId,seccionCapituloId, UsuarioLogadoDto.IdUsuario));
                return Ok(respuesta);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }
        #endregion Programación

        #region ProgramaciónDistribución

        /// <summary>
        /// Api para obtener lista de programación de distribucion.
        /// </summary>
        /// <returns>Lista de programación de distribucion</returns>
        [HttpGet]
        [Route("api/ProgramacionDistribucion/ObtenerDatosProgramacionEncabezado")]
        public async Task<IHttpActionResult> ObtenerDatosProgramacionEncabezado(int EntidadDestinoId, int TramiteId, string origen)
        {
            try
            {
                var result = await Task.Run(() => _programacionServicios.ObtenerDatosProgramacionEncabezado(EntidadDestinoId, TramiteId, origen, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        /// <summary>
        /// Api para obtener lista de programación de distribucion detalle.
        /// </summary>
        /// <returns>Lista de programación de distribucion detalle</returns>
        [HttpGet]
        [Route("api/ProgramacionDistribucion/ObtenerDatosProgramacionDetalle")]
        public async Task<IHttpActionResult> ObtenerDatosProgramacionDetalle(int TramiteProyectoId, string origen)
        {
            try
            {
                var result = await Task.Run(() => _programacionServicios.ObtenerDatosProgramacionDetalle(TramiteProyectoId, origen, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [HttpPost]
        [Route("api/ProgramacionDistribucion/GuardarDatosProgramacionDistribucion")]
        public async Task<IHttpActionResult> GuardarDatosProgramacionDistribucion(ProgramacionDistribucionDto programacionDistribucion)
        {
            try
            {
                var result = await Task.Run(() => _programacionServicios.GuardarDatosProgramacionDistribucion(programacionDistribucion, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        #endregion

        #region Programación Fuentes

        [HttpPost]
        [Route("api/ProgramacionFuentes/GuardarDatosProgramacionFuentes")]
        public async Task<IHttpActionResult> GuardarDatosProgramacionFuentes(ProgramacionFuenteDto programacionFuente)
        {
            try
            {
                var result = await Task.Run(() => _programacionServicios.GuardarDatosProgramacionFuentes(programacionFuente, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        #endregion

        #region Programación Iniciativas

        [HttpPost]
        [Route("api/Programacion/GuardarDatosProgramacionIniciativa")]
        public async Task<IHttpActionResult> GuardarDatosProgramacionIniciativa(ProgramacionIniciativaDto programacionIniciativa)
        {
            try
            {
                var result = await Task.Run(() => _programacionServicios.GuardarDatosProgramacionIniciativa(programacionIniciativa, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        #endregion

        #region Programación Excepción

        /// <summary>
        /// Obtiene la lista de excepciones de una programación
        /// </summary>
        /// <param name="idProgramacion">Identificador de la programación</param>
        /// <returns>Lista de excepciones</returns>
        [HttpGet]
        [Route("api/programacion/ObtenerProgramacionExcepciones")]
        public async Task<IHttpActionResult> ObtenerProgramacionExcepciones([FromUri] int idProgramacion)
        {
            var result = await Task.Run(() => _programacionServicios.ObtenerProgramacionExcepciones(idProgramacion, UsuarioLogadoDto.IdUsuario));
            return Ok(result);
        }

        /// <summary>
        /// Guarda los datos de una excepción
        /// </summary>
        /// <param name="programacionExcepcionDto">Datos de la excepción</param>
        /// <returns>Resultado de la operación guardar</returns>
        [HttpPost]
        [Route("api/Programacion/GuardarExcepcion")]
        public async Task<IHttpActionResult> Guardar(ProgramacionExcepcionDto programacionExcepcionDto)
        {
            try
            {
                var respuesta = await Task.Run(() => _programacionServicios.GuardarProgramacionExcepcion(programacionExcepcionDto, UsuarioLogadoDto.IdUsuario));
                return Ok(respuesta);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        /// <summary>
        /// Edita los datos de una excepción
        /// </summary>
        /// <param name="programacionExcepcionDto">Datos de la excepción</param>
        /// <returns>Resultado de la operación editar</returns>
        [HttpPost]
        [Route("api/Programacion/EditarExcepcion")]
        public async Task<IHttpActionResult> EditarExcepcion(ProgramacionExcepcionDto programacionExcepcionDto)
        {
            try
            {
                var respuesta = await Task.Run(() => _programacionServicios.EditarProgramacionExcepcion(programacionExcepcionDto, UsuarioLogadoDto.IdUsuario));
                return Ok(respuesta);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        /// <summary>
        /// Elimina una excepción
        /// </summary>
        /// <param name="programacionExcepcionDto">Datos de la excepción a eliminar</param>
        /// <returns>Resultado de la operación eliminar</returns>
        [HttpPost]
        [Route("api/Programacion/EliminarExcepcion")]
        public async Task<IHttpActionResult> EliminarExcepcion(ProgramacionExcepcionDto programacionExcepcionDto)
        {
            try
            {
                var respuesta = await Task.Run(() => _programacionServicios.EliminarProgramacionExcepcion(programacionExcepcionDto, UsuarioLogadoDto.IdUsuario));
                return Ok(respuesta);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        #endregion Programación Excepcion


        #region Programacion cargas masivas creditos

        /// <summary>
        ///  
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("api/programacion/ObtenerCargaMasivaCreditos")]
        public async Task<IHttpActionResult> ObtenerCargaMasivaCreditos()
        {

            try
            {
                var result = await Task.Run(() => _programacionServicios.ObtenerCargaMasivaCreditos(UsuarioLogadoDto.IdUsuario));

                return Ok(result);
            }
            catch (BackboneException exception)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, exception));
            }
        }

        [HttpPost]
        [Route("api/Programacion/ValidarCargaMasivaCreditos")]
        public async Task<IHttpActionResult> ValidarCargaMasivaCreditos(dynamic validarCargaMasivaCreditos)
        {
            try
            {
                var respuesta = await Task.Run(() => _programacionServicios.ValidarCargaMasivaCreditos(validarCargaMasivaCreditos, UsuarioLogadoDto.IdUsuario));
                return Ok(respuesta);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [HttpPost]
        [Route("api/Programacion/RegistrarCargaMasivaCreditos")]
        public async Task<IHttpActionResult> RegistrarCargaMasivaCreditos(dynamic registrarCargaMasivaCreditos)
        {
            try
            {
                var respuesta = await Task.Run(() => _programacionServicios.RegistrarCargaMasivaCreditos(registrarCargaMasivaCreditos, UsuarioLogadoDto.IdUsuario));
                return Ok(respuesta);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        #endregion Programacion cargas masivas creditos

        #region Programacion cargas masivas cuotas

        /// <summary>
        ///  
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("api/programacion/ObtenerCargaMasivaCuotas")]
        public async Task<IHttpActionResult> ObtenerCargaMasivaCuotas(int? Vigencia, int? EntityTypeCatalogOptionId)
        {

            try
            {
                var result = await Task.Run(() => _programacionServicios.ObtenerCargaMasivaCuotas(Vigencia, EntityTypeCatalogOptionId, UsuarioLogadoDto.IdUsuario));

                return Ok(result);
            }
            catch (BackboneException exception)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, exception));
            }
        }

        [HttpPost]
        [Route("api/Programacion/ValidarCargaMasivaCuotas")]
        public async Task<IHttpActionResult> ValidarCargaMasivaCuotas(dynamic validarCargaMasivaCuotas)
        {
            try
            {
                var respuesta = await Task.Run(() => _programacionServicios.ValidarCargaMasivaCuotas(validarCargaMasivaCuotas, UsuarioLogadoDto.IdUsuario));
                return Ok(respuesta);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [HttpPost]
        [Route("api/Programacion/RegistrarCargaMasivaCuotas")]
        public async Task<IHttpActionResult> RegistrarCargaMasivaCuotas(dynamic registrarCargaMasivaCuotas)
        {
            try
            {
                var respuesta = await Task.Run(() => _programacionServicios.RegistrarCargaMasivaCuotas(registrarCargaMasivaCuotas, UsuarioLogadoDto.IdUsuario));
                return Ok(respuesta);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        #endregion Programacion cargas masivas cuotas
        #region Programacion Preparacion Generar Presupuestal Administrar Calendario
        
        [HttpGet]
        [Route("api/Programacion/ObtenerProgramacionProyectosSinPresupuestal")]
        public async Task<IHttpActionResult> ConsultarProyectoGenerarPresupuestal(int sectorId, int entidadId, string proyectoId)
        {
            try
            {
                var result = await Task.Run(() => _programacionServicios.ConsultarProyectoGenerarPresupuestal(sectorId, entidadId, proyectoId, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [HttpGet]
        [Route("api/Programacion/ObtenerProgramacionSectores")]
        public async Task<IHttpActionResult> ObtenerProgramacionSectores(int sectorId)
        {
            try
            {
                var result = await Task.Run(() => _programacionServicios.ObtenerProgramacionSectores(sectorId, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }
        
        [HttpGet]
        [Route("api/Programacion/ObtenerProgramacionEntidadesSector")]
        public async Task<IHttpActionResult> ObtenerProgramacionEntidadesSector(int sectorId)
        {
            try
            {
                var result = await Task.Run(() => _programacionServicios.ObtenerProgramacionEntidadesSector(sectorId, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }
        
        [HttpGet]
        [Route("api/Programacion/ObtenerCalendarioProgramacion")]
        public async Task<IHttpActionResult> ObtenerCalendarioProgramacion(Guid FlujoId)
        {
            try
            {
                var result = await Task.Run(() => _programacionServicios.ObtenerCalendarioProgramacion(FlujoId, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }
        
        [HttpPost]
        [Route("api/Programacion/RegistrarProyectosSinPresupuestal")]
        public async Task<IHttpActionResult> RegistrarProyectosSinPresupuestal(List<ProyectoSinPresupuestalDto> proyectoSinPresupuestalDto)
        {
            try
            {
                var result = await Task.Run(() => _programacionServicios.RegistrarProyectosSinPresupuestal(proyectoSinPresupuestalDto, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }
        
        [HttpPost]
        [Route("api/Programacion/RegistrarCalendarioProgramacion")]
        public async Task<IHttpActionResult> RegistrarCalendarioProgramacion(List<CalendarioProgramacionDto> calendarioProgramacionDto)
        {
            try
            {
                var result = await Task.Run(() => _programacionServicios.RegistrarCalendarioProgramacion(calendarioProgramacionDto, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [HttpPost]
        [Route("api/Programacion/ValidarConsecutivoPresupuestal")]
        public async Task<IHttpActionResult> ValidarConsecutivoPresupuestal(dynamic validarCargaMasivaCuotas)
        {
            try
            {
                var respuesta = await Task.Run(() => _programacionServicios.ValidarConsecutivoPresupuestal(validarCargaMasivaCuotas, UsuarioLogadoDto.IdUsuario));
                return Ok(respuesta);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }
        #endregion Programacion Preparacion Generar Presupuestal Administrar Calendario

        [HttpPost]
        [Route("api/Programacion/GuardarProgramacionRegionalizacion")]
        public async Task<IHttpActionResult> GuardarProgramacionRegionalizacion(ProgramacionRegionalizacionDto programacionRegionalizacion)
        {
            try
            {
                var result = await Task.Run(() => _programacionServicios.GuardarProgramacionRegionalizacion(programacionRegionalizacion, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [HttpGet]
        [Route("api/Programacion/ConsultarPoliticasTransversalesProgramacion")]
        public async Task<IHttpActionResult> ConsultarPoliticasTransversalesProgramacion(string Bpin)
        {
            try
            {
                var result = await Task.Run(() => _programacionServicios.ConsultarPoliticasTransversalesProgramacion(Bpin, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [HttpPost]
        [Route("api/Programacion/AgregarPoliticasTransversalesProgramacion")]
        public async Task<IHttpActionResult> AgregarPoliticasTransversalesProgramacion(IncluirPoliticasDto objIncluirPoliticasDto)
        {
            try
            {
                var result = await Task.Run(() => _programacionServicios.AgregarPoliticasTransversalesProgramacion(objIncluirPoliticasDto, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [HttpGet]
        [Route("api/Programacion/ConsultarPoliticasTransversalesCategoriasProgramacion")]
        public async Task<IHttpActionResult> ConsultarPoliticasTransversalesCategoriasProgramacion(string Bpin)
        {
            try
            {
                var result = await Task.Run(() => _programacionServicios.ConsultarPoliticasTransversalesCategoriasProgramacion(Bpin, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [HttpPost]
        [Route("api/Programacion/EliminarPoliticasProyectoProgramacion")]
        public async Task<IHttpActionResult> EliminarPoliticasProyectoProgramacion(int tramiteidProyectoId, int politicaId)
        {
            try
            {
                var result = await Task.Run(() => _programacionServicios.EliminarPoliticasProyectoProgramacion(tramiteidProyectoId, politicaId, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [HttpPost]
        [Route("api/Programacion/AgregarCategoriasPoliticaTransversalesProgramacion")]
        public async Task<IHttpActionResult> AgregarCategoriasPoliticaTransversalesProgramacion(FocalizacionCategoriasDto objIncluirPoliticasDto)
        {
            try
            {
                var result = await Task.Run(() => _programacionServicios.AgregarCategoriasPoliticaTransversalesProgramacion(objIncluirPoliticasDto, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [HttpPost]
        [Route("api/Programacion/GuardarPoliticasTransversalesCategoriasProgramacion")]
        public async Task<IHttpActionResult> GuardarPoliticasTransversalesCategoriasProgramacion(PoliticasTransversalesCategoriasProgramacionDto objIncluirPoliticasDto)
        {
            try
            {
                var result = await Task.Run(() => _programacionServicios.GuardarPoliticasTransversalesCategoriasProgramacion(objIncluirPoliticasDto, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }


        [HttpPost]
        [Route("api/Programacion/EliminarCategoriasProyectoProgramacion")]
        public async Task<IHttpActionResult> EliminarCategoriasProyectoProgramacion(EliminarCategoriasProyectoProgramacionDto objIncluirPoliticasDto)
        {
            try
            {
                var result = await Task.Run(() => _programacionServicios.EliminarCategoriasProyectoProgramacion(objIncluirPoliticasDto, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [HttpPost]
        [Route("api/Programacion/EliminarCategoriaPoliticasProyectoProgramacion")]
        public async Task<IHttpActionResult> EliminarCategoriaPoliticasProyectoProgramacion(int proyectoId, int politicaId, int categoriaId)
        {
            try
            {
                var result = await Task.Run(() => _programacionServicios.EliminarCategoriaPoliticasProyectoProgramacion(proyectoId, politicaId, categoriaId, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [HttpGet]
        [Route("api/Programacion/ObtenerCrucePoliticasProgramacion")]
        public async Task<IHttpActionResult> ObtenerCrucePoliticasProgramacion(string Bpin)
        {
            try
            {
                var result = await Task.Run(() => _programacionServicios.ObtenerCrucePoliticasProgramacion(Bpin, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [HttpGet]
        [Route("api/Programacion/PoliticasSolicitudConceptoProgramacion")]
        public async Task<IHttpActionResult> PoliticasSolicitudConceptoProgramacion(string Bpin)
        {
            try
            {
                var result = await Task.Run(() => _programacionServicios.PoliticasSolicitudConceptoProgramacion(Bpin, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [HttpPost]
        [Route("api/Programacion/GuardarCrucePoliticasProgramacion")]
        public async Task<IHttpActionResult> GuardarCrucePoliticasProgramacion(List<CrucePoliticasAjustesDto> parametrosGuardar)
        {
            try
            {
                var result = await Task.Run(() => _programacionServicios.GuardarCrucePoliticasProgramacion(parametrosGuardar, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [HttpPost]
        [Route("api/Programacion/SolicitarConceptoDTProgramacion")]
        public async Task<IHttpActionResult> SolicitarConceptoDTProgramacion(List<FocalizacionSolicitarConceptoDto> parametrosGuardar)
        {
            try
            {
                var result = await Task.Run(() => _programacionServicios.SolicitarConceptoDTProgramacion(parametrosGuardar, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [HttpGet]
        [Route("api/Programacion/ObtenerResumenSolicitudConceptoProgramacion")]
        public async Task<IHttpActionResult> ObtenerResumenSolicitudConceptoProgramacion(string Bpin)
        {
            try
            {
                var result = await Task.Run(() => _programacionServicios.ObtenerResumenSolicitudConceptoProgramacion(Bpin, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        #region programacion productos

        /// <summary>
        /// Api para obtener lista de programación de productos.
        /// </summary>
        /// <returns>Lista de programación de productos</returns>
        [HttpGet]
        [Route("api/Programacion/ObtenerDatosProgramacionProducto")]
        public async Task<IHttpActionResult> ObtenerDatosProgramacionProducto(int TramiteId)
        {
            try
            {
                var result = await Task.Run(() => _programacionServicios.ObtenerDatosProgramacionProducto(TramiteId, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [HttpPost]
        [Route("api/Programacion/GuardarDatosProgramacionProducto")]
        public async Task<IHttpActionResult> GuardarDatosProgramacionProducto(ProgramacionProductoDto programacionProducto)
        {
            try
            {
                var result = await Task.Run(() => _programacionServicios.GuardarDatosProgramacionProducto(programacionProducto, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        #endregion

        [HttpGet]
        [Route("api/Programacion/ObtenerProgramacionBuscarProyecto")]
        public async Task<IHttpActionResult> ObtenerProgramacionBuscarProyecto(int EntidadDestinoId, int tramiteid, string bpin, string NombreProyecto)
        {
            try
            {
                var result = await Task.Run(() => _programacionServicios.ObtenerProgramacionBuscarProyecto(EntidadDestinoId, tramiteid, bpin, NombreProyecto, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [HttpPost]
        [Route("api/Programacion/BorrarTramiteProyecto")]
        public async Task<IHttpActionResult> BorrarTramiteProyecto(ProgramacionDistribucionDto programacionDistribucion)
        {
            try
            {
                var result = await Task.Run(() => _programacionServicios.BorrarTramiteProyecto(programacionDistribucion, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [HttpPost]
        [Route("api/Programacion/GuardarDatosInclusion")]
        public async Task<IHttpActionResult> GuardarDatosInclusion(ProgramacionDistribucionDto programacionDistribucion)
        {
            try
            {
                var result = await Task.Run(() => _programacionServicios.GuardarDatosInclusion(programacionDistribucion, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [HttpGet]
        [Route("api/Programacion/ConsultarPoliticasTransversalesCategoriasModificaciones")]
        public async Task<IHttpActionResult> ConsultarPoliticasTransversalesCategoriasModificaciones(string Bpin)
        {
            try
            {
                var result = await Task.Run(() => _programacionServicios.ConsultarPoliticasTransversalesCategoriasModificaciones(Bpin, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [HttpPost]
        [Route("api/Programacion/GuardarPoliticasTransversalesCategoriasModificaciones")]
        public async Task<IHttpActionResult> GuardarPoliticasTransversalesCategoriasModificaciones(PoliticasTransversalesCategoriasProgramacionDto objIncluirPoliticasDto)
        {
            try
            {
                var result = await Task.Run(() => _programacionServicios.GuardarPoliticasTransversalesCategoriasModificaciones(objIncluirPoliticasDto, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }


        [HttpGet]
        [Route("api/Programacion/ConsultarPoliticasTransversalesAprobacionesModificaciones")]
        public async Task<IHttpActionResult> ConsultarPoliticasTransversalesAprobacionesModificaciones(string Bpin)
        {
            try
            {
                var result = await Task.Run(() => _programacionServicios.ConsultarPoliticasTransversalesAprobacionesModificaciones(Bpin, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }


        #region cargue masivo saldos

        [HttpGet]
        [Route("api/Programacion/RegistrarCargaMasivaSaldos")]
        public async Task<IHttpActionResult> RegistrarCargaMasivaSaldos(int? TipoCargueId)
        {
            try
            {
                var result = await Task.Run(() => _programacionServicios.RegistrarCargaMasivaSaldos(TipoCargueId.Value, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [HttpGet]
        [Route("api/Programacion/ObtenerLogErrorCargaMasivaSaldos")]
        public async Task<IHttpActionResult> ObtenerLogErrorCargaMasivaSaldos(int? tipoCargueDetalleId, int? carguesIntegracionId)
        {
            try
            {
                var result = await Task.Run(() => _programacionServicios.ObtenerLogErrorCargaMasivaSaldos(tipoCargueDetalleId, carguesIntegracionId, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }
        
        [HttpGet]
        [Route("api/Programacion/ObtenerCargaMasivaSaldos")]
        public async Task<IHttpActionResult> ObtenerCargaMasivaSaldos(string tipoCargue)
        {
            try
            {
                var result = await Task.Run(() => _programacionServicios.ObtenerCargaMasivaSaldos(tipoCargue, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [HttpGet]
        [Route("api/Programacion/ObtenerTipoCargaMasiva")]
        public async Task<IHttpActionResult> ObtenerTipoCargaMasiva(string TipoCargue)
        {
            try
            {
                var result = await Task.Run(() => _programacionServicios.ObtenerTipoCargaMasiva(TipoCargue, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [HttpPost]
        [Route("api/Programacion/ValidarCargaMasiva")]
        public async Task<IHttpActionResult> ValidarCargaMasiva(dynamic jsonListaRegistros)
        {
            try
            {
                var result = await Task.Run(() => _programacionServicios.ValidarCargaMasiva(jsonListaRegistros, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [HttpGet]
        [Route("api/Programacion/ObtenerDetalleCargaMasivaSaldos")]
        public async Task<IHttpActionResult> ObtenerDetalleCargaMasivaSaldos(int? CargueId)
        {
            try
            {
                var result = await Task.Run(() => _programacionServicios.ObtenerDetalleCargaMasivaSaldos(CargueId, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }
        #endregion cargue masivo saldos

       


        [HttpGet]
        [Route("api/Programacion/ConsultarCatalogoIndicadoresPolitica")]
        public async Task<IHttpActionResult> ConsultarCatalogoIndicadoresPolitica(string PoliticaId, string Criterio)
        {
            try
            {
                var result = await Task.Run(() => _programacionServicios.ConsultarCatalogoIndicadoresPolitica(PoliticaId, Criterio, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [HttpPost]
        [Route("api/Programacion/GuardarModificacionesAsociarIndicadorPolitica")]
        public async Task<IHttpActionResult> GuardarModificacionesAsociarIndicadorPolitica(int proyectoId, int politicaId, int categoriaId, int indicadorId, string accion)
        {
            try
            {
                var result = await Task.Run(() => _programacionServicios.GuardarModificacionesAsociarIndicadorPolitica(proyectoId, politicaId, categoriaId, indicadorId, accion, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

    }
}