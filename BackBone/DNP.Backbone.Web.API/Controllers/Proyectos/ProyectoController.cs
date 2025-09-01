namespace DNP.Backbone.Web.API.Controllers
{
    using Comunes.Dto;
    using Comunes.Excepciones;
    using DNP.Backbone.Comunes.Enums;
    using DNP.Backbone.Dominio.Dto;
    using DNP.Backbone.Dominio.Dto.Beneficiarios;
    using DNP.Backbone.Dominio.Dto.CadenaValor;
    using DNP.Backbone.Dominio.Dto.CostoActividades;
    using DNP.Backbone.Dominio.Dto.Focalizacion;
    using DNP.Backbone.Dominio.Dto.Proyecto;
    using DNP.Backbone.Dominio.Dto.Transversales;
    using DNP.Backbone.Dominio.Enums;
    using DNP.Backbone.Servicios.Interfaces.Proyectos;
    using DNP.Backbone.Servicios.Interfaces.ServiciosNegocio;
    using Servicios.Interfaces.Autorizacion;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;
    using System.Web.Http;
    using System.Web.Mvc.Html;

    /// <summary>
    /// Clase responsable de la gestión de proyectos
    /// </summary>
    public class ProyectoController : Base.BackboneBase
    {
        private readonly IProyectoServicios _proyectoServicios;
        private readonly IAutorizacionServicios _autorizacionUtilidades;
        private readonly IServiciosNegocioServicios _serviciosNegocioServicios;
        private readonly IFlujoServicios _flujoServicios;

        /// <summary>
        /// Constructor de clases
        /// </summary>
        /// <param name="proyectoServicios">Instancia de servicios de proyectos</param>
        /// <param name="autorizacionUtilidades">Instancia de servicios de autorizacion</param>
        /// <param name="serviciosNegocioServicios">Instancia de servicios de Negocio Servicios</param>
        public ProyectoController(IProyectoServicios proyectoServicios,
            IAutorizacionServicios autorizacionUtilidades,
            IServiciosNegocioServicios serviciosNegocioServicios,
            IFlujoServicios flujoServicios)
            : base(autorizacionUtilidades)
        {
            _proyectoServicios = proyectoServicios;
            _autorizacionUtilidades = autorizacionUtilidades;
            _serviciosNegocioServicios = serviciosNegocioServicios;
            _flujoServicios = flujoServicios;
        }

        /// <summary>
        /// Validacion previa a la devolución de un paso
        /// </summary>     
        /// <param name="instanciaId"></param>   
        /// <param name="accionId"></param>   
        /// <param name="accionDevolucionId"></param>   
        /// <returns>string</returns> 
        [Route("api/Proyecto/ValidacionDevolucionPaso")]
        [HttpGet]
        public async Task<IHttpActionResult> ValidacionDevolucionPaso(Guid instanciaId, Guid accionId, Guid accionDevolucionId)
        {
            try
            {
                var respuestasalida = await Task.Run(() => _proyectoServicios.ValidacionDevolucionPaso(instanciaId, accionId, accionDevolucionId, User.Identity.Name));
                return Ok(respuestasalida);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        /// <summary>
        /// Api para obtención de datos de proyectos en Excel.
        /// </summary>
        /// <param name="instanciaProyectoDto">Contiene informacion de autorizacion, filtro e columnas visibles</param>
        /// <returns>Excel con datos generados</returns>
        [Route("api/Proyecto/ObtenerExcel")]
        [HttpPost]
        public async Task<HttpResponseMessage> ObtenerExcel(InstanciaProyectoDto instanciaProyectoDto)
        {
            try
            {
                HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);

                var _result = await Task.Run(() => _proyectoServicios.ObtenerProyectos(instanciaProyectoDto.ProyectoParametrosDto, Request.Headers.Authorization.Parameter, instanciaProyectoDto.ProyectoFiltroDto, null));
                _result.ColumnasVisibles = instanciaProyectoDto.ColumnasVisibles;

                result.StatusCode = HttpStatusCode.OK;
                result.Content = ExcelUtilidades.ObtenerExcellProyecto(_result, instanciaProyectoDto.ProyectoFiltroDto);
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
        /// Api para obtención de datos de proyectos por idUsuario, idObjeto y aplicación.
        /// </summary>
        /// <param name="instanciaProyectoDto"></param>
        /// <returns>Objeto con propiedades para realizar consulta de datos de proyectos.</returns>
        [Route("api/Proyecto/ObtenerProyectos")]
        [HttpPost]
        public async Task<IHttpActionResult> ObtenerProyectos(InstanciaProyectoDto instanciaProyectoDto)
        {
            try
            {
                if (!await ValidarParametrosRequest(instanciaProyectoDto.ProyectoParametrosDto).ConfigureAwait(false)) return ResponseMessage(base.RespuestaAutorizacion);
                var result = await _proyectoServicios.ObtenerProyectos(instanciaProyectoDto.ProyectoParametrosDto, Request.Headers.Authorization.Parameter, instanciaProyectoDto.ProyectoFiltroDto, User.Identity.Name).ConfigureAwait(false);
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        /// <summary>
        /// Api para obtención de datos de proyectos por idUsuario, idObjeto y aplicación.
        /// </summary>
        /// <param name="instanciaProyectoDto"></param>
        /// <returns>Objeto con propiedades para realizar consulta de datos de proyectos.</returns>
        [Route("api/Proyecto/ObtenerProyectosTodos")]
        [HttpPost]
        public async Task<IHttpActionResult> ObtenerProyectosTodos(InstanciaProyectoDto instanciaProyectoDto)
        {
            try
            {
                if (!await ValidarParametrosRequest(instanciaProyectoDto.ProyectoParametrosDto).ConfigureAwait(false)) return ResponseMessage(base.RespuestaAutorizacion);
                var result = await _proyectoServicios.ObtenerProyectosTodos(instanciaProyectoDto.ProyectoParametrosDto, Request.Headers.Authorization.Parameter, instanciaProyectoDto.ProyectoFiltroDto).ConfigureAwait(false);
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        /// <summary>
        /// Api para lista de entidades.
        /// </summary>
        /// <param name="peticionObtenerProyecto">Contiene informacion de autorizacion</param>
        /// <returns>Lista de entidades</returns>
        [Route("api/Proyecto/ObtenerListaEntidades")]
        [HttpPost]
        public async Task<IHttpActionResult> ObtenerListaEntidades(ProyectoParametrosDto peticionObtenerProyecto)
        {
            try
            {
                if (!await ValidarParametrosRequest(peticionObtenerProyecto).ConfigureAwait(false)) return ResponseMessage(base.RespuestaAutorizacion);
                var result = await _serviciosNegocioServicios.ObtenerListaCatalogo(peticionObtenerProyecto, CatalogoEnum.Entidades).ConfigureAwait(false);

                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/Proyecto/ObtenerListaEntidadesTotal")]
        [HttpPost]
        public async Task<IHttpActionResult> ObtenerListaEntidadesTotal(ProyectoParametrosDto peticionObtenerProyecto)
        {
            try
            {
                if (!await ValidarParametrosRequest(peticionObtenerProyecto).ConfigureAwait(false)) return ResponseMessage(base.RespuestaAutorizacion);
                var result = await _serviciosNegocioServicios.ObtenerListaCatalogoEntidades(peticionObtenerProyecto, CatalogoEnum.Entidades).ConfigureAwait(false);

                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        /// <summary>
        /// Api para lista de entidades.
        /// </summary>
        /// <param name="peticionObtenerProyecto">Contiene informacion de autorizacion</param>
        /// <returns>Lista de entidades</returns>
        [Route("api/Proyecto/ObtenerListaEtapas")]
        [HttpPost]
        public async Task<IHttpActionResult> ObtenerListaEtapas(ProyectoParametrosDto peticionObtenerProyecto)
        {
            try
            {
                if (!await ValidarParametrosRequest(peticionObtenerProyecto).ConfigureAwait(false)) return ResponseMessage(base.RespuestaAutorizacion);
                var result = await _serviciosNegocioServicios.ObtenerListaCatalogo(peticionObtenerProyecto, CatalogoEnum.Etapas).ConfigureAwait(false);

                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/Proyecto/ObtenerListaTipoEntidad")]
        [HttpPost]
        public async Task<IHttpActionResult> ObtenerListaTipoEntidad(ProyectoParametrosDto peticionObtenerProyecto)
        {
            try
            {
                if (!await ValidarParametrosRequest(peticionObtenerProyecto).ConfigureAwait(false)) return ResponseMessage(base.RespuestaAutorizacion);
                var result = await _serviciosNegocioServicios.ObtenerListaCatalogo(peticionObtenerProyecto, CatalogoEnum.TodosTiposEntidades).ConfigureAwait(false);

                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/Proyecto/ObtenerListaTiposRecursos")]
        [HttpPost]
        public async Task<IHttpActionResult> ObtenerListaTiposRecursos(ProyectoParametrosDto peticionObtenerProyecto)
        {
            try
            {
                if (!await ValidarParametrosRequest(peticionObtenerProyecto).ConfigureAwait(false))
                    return ResponseMessage(base.RespuestaAutorizacion);
                var result = await _serviciosNegocioServicios.ObtenerListaCatalogoEntidades(peticionObtenerProyecto, CatalogoEnum.TiposRecursos).ConfigureAwait(false);

                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/Proyecto/ObtenerListaTiposRecursosxEntidad")]
        [HttpPost]
        public async Task<IHttpActionResult> ObtenerListaTiposRecursosxEntidad(ProyectoParametrosDto peticionObtenerProyecto, int entityTypeCatalogId)
        {
            try
            {
                if (!await ValidarParametrosRequest(peticionObtenerProyecto).ConfigureAwait(false))
                    return ResponseMessage(base.RespuestaAutorizacion);
                var result = await _serviciosNegocioServicios.ObtenerTiposRecursosEntidad(peticionObtenerProyecto, entityTypeCatalogId).ConfigureAwait(false);

                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }


        /// <summary>
        /// Api para lista de sectores.
        /// </summary>
        /// <param name="peticion">Contiene informacion de autorizacion</param>
        /// <returns>Lista de sectores</returns>
        [Route("api/Proyecto/ObtenerListaSectores")]
        [HttpPost]
        public async Task<IHttpActionResult> ObtenerListaSectores(ProyectoParametrosDto peticion)
        {
            try
            {
                if (!await ValidarParametrosRequest(peticion).ConfigureAwait(false)) return ResponseMessage(base.RespuestaAutorizacion);
                var result = await _serviciosNegocioServicios.ObtenerListaCatalogo(peticion, CatalogoEnum.Sectores).ConfigureAwait(false);

                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/Proyecto/ObtenerListaSectoresEntity")]
        [HttpPost]
        public async Task<IHttpActionResult> ObtenerListaSectoresEntity(ProyectoParametrosDto peticion)
        {
            try
            {
                if (!await ValidarParametrosRequest(peticion).ConfigureAwait(false)) return ResponseMessage(base.RespuestaAutorizacion);
                var result = await _serviciosNegocioServicios.ObtenerListaCatalogo(peticion, CatalogoEnum.SectoresEntity).ConfigureAwait(false);

                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        /// <summary>
        /// Api para lista de criticidad.
        /// </summary>
        /// <param name="peticion">Contiene informacion de autorizacion</param>
        /// <returns>Lista de criticidad</returns>
        [Route("api/Proyecto/ObtenerListaCriticidad")]
        [HttpPost]
        public async Task<IHttpActionResult> ObtenerListaCriticidad(ProyectoParametrosDto peticion)
        {
            try
            {
                if (!await ValidarParametrosRequest(peticion).ConfigureAwait(false)) return ResponseMessage(base.RespuestaAutorizacion);

                var result = await Task.Run(() => new[] {
                    new { Id = Criticidad.Baja.ToString(), Name = Criticidad.Baja.ToString() },
                    new { Id = Criticidad.Media.ToString(), Name = Criticidad.Media.ToString() },
                    new { Id = Criticidad.Alta.ToString(), Name = Criticidad.Alta.ToString() },
                }.ToList());

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
        [Route("api/Proyecto/ObtenerListaEstadoProyecto")]
        [HttpPost]
        public async Task<IHttpActionResult> ObtenerListaEstadoProyecto(ProyectoParametrosDto peticion)
        {
            try
            {
                if (!await ValidarParametrosRequest(peticion).ConfigureAwait(false)) return ResponseMessage(base.RespuestaAutorizacion);
                var result = await _serviciosNegocioServicios.ObtenerListaEstado(peticion).ConfigureAwait(false);

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
        [Route("api/Proyecto/ObtenerMatrizEntidadDestino")]
        [HttpPost]
        public async Task<IHttpActionResult> ObtenerMatrizEntidadDestino(ListMatrizEntidadDestinoDto peticion)
        {
            try
            {
                peticion.IdUsuario = UsuarioLogadoDto.IdUsuario;
                var result = await _serviciosNegocioServicios.ObtenerMatrizEntidadDestino(peticion).ConfigureAwait(false);

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
        [Route("api/Proyecto/ActualizarMatrizEntidadDestino")]
        [HttpPost]
        public async Task<IHttpActionResult> ActualizarMatrizEntidadDestino(ListaMatrizEntidadUnidadDto peticion)
        {
            try
            {
                var result = await _serviciosNegocioServicios.ActualizarMatrizEntidadDestino(peticion, UsuarioLogadoDto.IdUsuario).ConfigureAwait(false);

                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        /// <summary>
        /// Api para obtención de datos de proyectos por idEntidad
        /// </summary>
        /// <param name="dto"></param>
        /// <returns>Objeto con propiedades para realizar consulta de datos de proyectos.</returns>
        [Route("api/Proyecto/ObtenerProyectosPorEntidad")]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerProyectosPorEntidad([FromUri] Dominio.Dto.AutorizacionNegocio.EntidadFiltroDto dto)
        {
            //dto.IdEntidad = Guid.Parse("1534A1CB-2748-4DCF-98EB-2B4F5242D8B1");
            var proyectos = new List<Dominio.Dto.ProyectosEntidadesDto>();

            try
            {
                var autorizacionResult = await Task.Run(() => _autorizacionUtilidades.ObtenerEntidadPorId(dto, UsuarioLogadoDto.IdUsuario));
                if (autorizacionResult is null || !autorizacionResult.EntityTypeCatalogOptionId.HasValue)
                {
                    return Ok(proyectos);
                }

                var negocioResult = await Task.Run(() => _serviciosNegocioServicios.ObtenerProyectos(new ParametrosProyectosDto
                {
                    IdsEntidades = new List<int> { autorizacionResult.EntityTypeCatalogOptionId.Value },
                    NombresEstadosProyectos = new List<string>
                    {
                        ProyectoEstadoMga.Formulado.DescriptionAttr(),
                        ProyectoEstadoMga.Viable.DescriptionAttr(),
                        ProyectoEstadoMga.Aprobado.DescriptionAttr(),
                        ProyectoEstadoMga.EnEjecucion.DescriptionAttr()
                     }
                }, UsuarioLogadoDto.IdUsuario));
                if (negocioResult is null)
                {
                    return Ok(proyectos);
                }

                var tipoProyecto = Guid.Parse(System.Configuration.ConfigurationManager.AppSettings["IdTipoProyecto"]);

                var flujosResult = await Task.Run(() => _flujoServicios.ValidarProyectosConInstanciasActivas(new Dominio.Dto.Proyecto.ValidarProyectosDto
                {
                    IdTipoObjetoNegocio = tipoProyecto,
                    Bpins = negocioResult.Select(x => x.CodigoBpin).ToList(),
                    IdUsuarioDNP = UsuarioLogadoDto.IdUsuario
                }));

                foreach (var item in flujosResult.Where(x => x.InstanciasActivas > 0))
                {
                    var proyecto = negocioResult.First(x => x.CodigoBpin == item.Bpin);
                    proyectos.Add(new Dominio.Dto.ProyectosEntidadesDto
                    {
                        ProyectoId = proyecto.ProyectoId,
                        ProyectoNombre = proyecto.ProyectoNombre,
                        CodigoBpin = item.Bpin
                    });
                }

                return Ok(proyectos);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        /// <summary>
        /// Api para lista de localizaciones del proyectos.
        /// </summary>
        /// <param name="peticion">Contiene informacion de autorizacion</param>
        /// <returns>Lista de estado del proyectos</returns>
        [Route("api/Proyecto/ObtenerProyectoListaLocalizaciones")]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerProyectoListaLocalizaciones(string bpin)
        {
            try
            {
                var result = await Task.Run(() => _serviciosNegocioServicios.ObtenerProyectoListaLocalizaciones(bpin, UsuarioLogadoDto.IdUsuario, Request.Headers.Authorization.Parameter));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        /// <summary>
        /// Api para obtención de datos de proyectos por idUsuario, idObjeto y aplicación.
        /// </summary>
        /// <param name="instanciaProyectoDto"></param>
        /// <returns></returns>
        [Route("api/Proyecto/ObtenerInfoPDF")]
        [HttpPost]
        public async Task<IHttpActionResult> ObtenerProyectosTramitesPDF(InstanciaProyectoDto instanciaProyectoDto)
        {
            try
            {
                if (!await ValidarParametrosRequest(instanciaProyectoDto.ProyectoParametrosDto).ConfigureAwait(false)) return ResponseMessage(base.RespuestaAutorizacion);
                var result = await _proyectoServicios.ObtenerProyectos(instanciaProyectoDto.ProyectoParametrosDto, Request.Headers.Authorization.Parameter, instanciaProyectoDto.ProyectoFiltroDto, null).ConfigureAwait(false);

                if (result != null)
                    result.ColumnasVisibles = instanciaProyectoDto.ColumnasVisibles;

                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }


        /// <summary>
        /// Api para lista de estado instancia.
        /// </summary>
        /// <param name="peticion">Contiene informacion de autorizacion</param>
        /// <returns>Lista de estado instancia</returns>
        [Route("api/Proyecto/ObtenerListaEstadoInstancia")]
        [HttpPost]
        public async Task<IHttpActionResult> ObtenerListaEstadoInstancia(ProyectoParametrosDto peticion)
        {
            try
            {
                if (!await ValidarParametrosRequest(peticion).ConfigureAwait(false)) return ResponseMessage(base.RespuestaAutorizacion);

                var result = await Task.Run(() => new[] {
                    new { Id = ((int)EstadoInstancias.Activo).ToString(), Estado = EstadoInstancias.Activo.GetDescription() },
                    new { Id = ((int)EstadoInstancias.AnuladoPorAlcance).ToString(), Estado = EstadoInstancias.AnuladoPorAlcance.GetDescription() },
                    new { Id = ((int)EstadoInstancias.Completado).ToString(), Estado = EstadoInstancias.Completado.GetDescription() },
                    new { Id = ((int)EstadoInstancias.Pausado).ToString(), Estado = EstadoInstancias.Pausado.GetDescription() },
                    new { Id = ((int)EstadoInstancias.Cancelado).ToString(), Estado = EstadoInstancias.Cancelado.GetDescription() },
                    new { Id = ((int)EstadoInstancias.Anulado).ToString(), Estado = EstadoInstancias.Anulado.GetDescription() },
                }.ToList());

                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }


        /// <summary>
        /// Api para activar instancia.
        /// </summary>
        /// <param name="instanciaProyectoDto"></param>
        /// <returns>Objeto con propiedades para activar instancia.</returns>
        [Route("api/Proyecto/ActivarInstancia")]
        [HttpPost]
        public async Task<IHttpActionResult> ActivarInstancia(InstanciaProyectoDto instanciaProyectoDto)
        {
            try
            {
                if (!await ValidarParametrosRequest(instanciaProyectoDto.ProyectoParametrosDto).ConfigureAwait(false)) return ResponseMessage(base.RespuestaAutorizacion);
                var result = await _proyectoServicios.ActivarInstancia(instanciaProyectoDto.ProyectoParametrosDto, Request.Headers.Authorization.Parameter).ConfigureAwait(false);
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        /// <summary>
        /// Api para pausar instancia.
        /// </summary>
        /// <param name="instanciaProyectoDto"></param>
        /// <returns>Objeto con propiedades para pausar instancia.</returns>
        [Route("api/Proyecto/PausarInstancia")]
        [HttpPost]
        public async Task<IHttpActionResult> PausarInstancia(InstanciaProyectoDto instanciaProyectoDto)
        {
            try
            {
                if (!await ValidarParametrosRequest(instanciaProyectoDto.ProyectoParametrosDto)) return ResponseMessage(base.RespuestaAutorizacion);
                var result = await _proyectoServicios.PausarInstancia(instanciaProyectoDto.ProyectoParametrosDto, Request.Headers.Authorization.Parameter).ConfigureAwait(false);
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        /// <summary>
        /// Api para detener instancia.
        /// </summary>
        /// <param name="instanciaProyectoDto"></param>
        /// <returns>Objeto con propiedades para detener instancia.</returns>
        [Route("api/Proyecto/DetenerInstancia")]
        [HttpPost]
        public async Task<IHttpActionResult> DetenerInstancia(InstanciaProyectoDto instanciaProyectoDto)
        {
            try
            {
                if (!await ValidarParametrosRequest(instanciaProyectoDto.ProyectoParametrosDto)) return ResponseMessage(base.RespuestaAutorizacion);
                var result = await _proyectoServicios.DetenerInstancia(instanciaProyectoDto.ProyectoParametrosDto, Request.Headers.Authorization.Parameter).ConfigureAwait(false);
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        /// <summary>
        /// Api para cancelar instancia.
        /// </summary>
        /// <param name="instanciaProyectoDto"></param>
        /// <returns>Objeto con propiedades para cancelar una instancia en paso 1.</returns>
        [Route("api/Proyecto/CancelarInstanciaMisProcesos")]
        [HttpPost]
        public async Task<IHttpActionResult> CancelarInstanciaMisProcesos(InstanciaProyectoDto instanciaProyectoDto)
        {
            try
            {
                if (!await ValidarParametrosRequest(instanciaProyectoDto.ProyectoParametrosDto)) return ResponseMessage(base.RespuestaAutorizacion);
                var result = await _proyectoServicios.CancelarInstanciaMisProcesos(instanciaProyectoDto.ProyectoParametrosDto, Request.Headers.Authorization.Parameter).ConfigureAwait(false);
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        /// <summary>
        /// Obtiene la lista de proyectos para contracredito
        /// </summary>
        /// <param name="prm">Información para filtra el resultado</param>
        /// <returns>Lista de proyectos filtrada</returns>
        [Route("api/Proyecto/ObtenerContracredito")]
        [HttpPost]
        //public async Task<IHttpActionResult> ObtenerContracredito(InstanciaProyectoDto instanciaProyectoDto, ProyectoCreditoParametroDto prm)
        public async Task<IHttpActionResult> ObtenerContracredito(ProyectoCreditoParametroDto prm)
        {
            try
            {
                //if (!ValidarParametrosRequest(instanciaProyectoDto.ProyectoParametrosDto)) return ResponseMessage(base.RespuestaAutorizacion);
                var result = await Task.Run(() => _proyectoServicios.ObtenerContracreditos(prm, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        /// <summary>
        /// Ontiene la lista de proyectos para creditos
        /// </summary>
        /// <param name="instanciaProyectoDto">Datos para autorización</param>
        /// <param name="prm">Información para filtrar la información</param>
        /// <returns>Lista de proyectos filtrado</returns>
        [Route("api/Proyecto/ObtenerCredito")]
        [HttpPost]
        //public async Task<IHttpActionResult> ObtenerCredito(InstanciaProyectoDto instanciaProyectoDto, ProyectoCreditoParametroDto prm)
        public async Task<IHttpActionResult> ObtenerCredito(ProyectoCreditoParametroDto prm)
        {
            try
            {
                //if (!ValidarParametrosRequest(instanciaProyectoDto.ProyectoParametrosDto)) return ResponseMessage(base.RespuestaAutorizacion);
                var result = await Task.Run(() => _proyectoServicios.ObtenerCreditos(prm, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }
        /// <summary>
        /// Guarda la lista de proyectos para creditos
        /// </summary>
        /// <param name="instanciaProyectoDto">Datos para autorización</param>
        /// <param name="prm">Información para filtrar la información</param>
        /// <returns>Lista de proyectos filtrado</returns>
        [Route("api/Proyecto/GuardarProyectos")]
        [HttpPost]
        public async Task<IHttpActionResult> GuardarProyectos(ParametroProyectoTramiteDto ppt)
        {
            try
            {
                var result = await Task.Run(() => _proyectoServicios.GuardarProyectos(ppt, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        /// <summary>
        /// Api para obtención de datos de proyectos por idUsuario, idObjeto y aplicación.
        /// </summary>
        /// <param name="instanciaProyectoDto"></param>
        /// <returns>Objeto con propiedades para realizar consulta de datos de proyectos.</returns>
        [Route("api/Proyecto/ObtenerProyectosConsolaProcesos")]
        [HttpPost]
        public async Task<IHttpActionResult> ObtenerProyectosConsolaProcesos(InstanciaProyectoDto instanciaProyectoDto)
        {
            try
            {
                //if (!await ValidarParametrosRequest(instanciaProyectoDto.ProyectoParametrosDto)) return ResponseMessage(base.RespuestaAutorizacion);
                var result = await Task.Run(() => _proyectoServicios.ObtenerProyectosConsolaProcesos(instanciaProyectoDto.ProyectoParametrosDto, Request.Headers.Authorization.Parameter, instanciaProyectoDto.ProyectoFiltroDto, User.Identity.Name));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }


        /// <summary>
        /// Api para obtención de datos de proyectos en Excel.
        /// </summary>
        /// <param name="instanciaProyectoDto">Contiene informacion de autorizacion, filtro e columnas visibles</param>
        /// <returns>Excel con datos generados</returns>
        [Route("api/Proyecto/ObtenerExcelConsolaProcesos")]
        [HttpPost]
        public async Task<HttpResponseMessage> ObtenerExcelConsolaProcesos(InstanciaProyectoDto instanciaProyectoDto)
        {
            try
            {
                HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);

                var _result = await Task.Run(() => _proyectoServicios.ObtenerProyectosConsolaProcesos(instanciaProyectoDto.ProyectoParametrosDto, Request.Headers.Authorization.Parameter, instanciaProyectoDto.ProyectoFiltroDto, User.Identity.Name));
                _result.ColumnasVisibles = instanciaProyectoDto.ColumnasVisibles;

                result.StatusCode = HttpStatusCode.OK;
                result.Content = ExcelUtilidades.ObtenerExcellProyectoConsola(_result);
                result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");

                return result;
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        /// <summary>
        /// Api para obtención de datos de proyectos por idUsuario, idObjeto y aplicación.
        /// </summary>
        /// <param name="instanciaProyectoDto"></param>
        /// <returns></returns>
        [Route("api/Proyecto/ObtenerInfoPDFConsolaProcesos")]
        [HttpPost]
        public async Task<IHttpActionResult> ObtenerProyectosTramitesPDFConsolaProcesos(InstanciaProyectoDto instanciaProyectoDto)
        {
            try
            {
                if (!await ValidarParametrosRequest(instanciaProyectoDto.ProyectoParametrosDto)) return ResponseMessage(base.RespuestaAutorizacion);
                var result = await Task.Run(() => _proyectoServicios.ObtenerProyectosConsolaProcesos(instanciaProyectoDto.ProyectoParametrosDto, Request.Headers.Authorization.Parameter, instanciaProyectoDto.ProyectoFiltroDto, User.Identity.Name));

                if (result != null)
                    result.ColumnasVisibles = instanciaProyectoDto.ColumnasVisibles;

                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        /// <summary>
        /// Api para obtención de datos de proyectos por idUsuario, idObjeto y aplicación.
        /// </summary>
        /// <param name="instanciaProyectoDto"></param>
        /// <returns></returns>
        [Route("api/Proyecto/ObtenerTokenMGA")]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerTokenMGA(string bpin, string tipoUsuario)
        {
            try
            {
                var result = await Task.Run(() => _proyectoServicios.ObtenerTokenMGA(bpin, UsuarioLogadoDto, tipoUsuario, Request.Headers.Authorization.Parameter));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        /// <summary>
        /// obtiene las direcciones tecnicas
        /// </summary>
        /// <param name="peticionObtenerProyecto">Contiene informacion de autorizacion</param>
        /// <returns>Lista de direcciones tecnicas</returns>
        [Route("api/Proyecto/ObtenerListaDireccionTecnica")]
        [HttpPost]
        public async Task<IHttpActionResult> ObtenerListaDireccionTecnica(ProyectoParametrosDto peticionObtenerProyecto)
        {
            try
            {
                if (!await ValidarParametrosRequest(peticionObtenerProyecto).ConfigureAwait(false)) return ResponseMessage(base.RespuestaAutorizacion);
                var result = await _serviciosNegocioServicios.ObtenerListaCatalogoDT(peticionObtenerProyecto, CatalogoEnum.DireccionTecnica).ConfigureAwait(false);

                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }
        /// <summary>
        /// obtiene las subdirecciones tecnicas
        /// </summary>
        /// <param name="peticionObtenerProyecto">Contiene informacion de autorizacion</param>
        /// <returns>lista de subdireeciones tecnicas</returns>
        [Route("api/Proyecto/ObtenerListaSubDireccionTecnica")]
        [HttpPost]
        public async Task<IHttpActionResult> ObtenerListaSubDireccionTecnica(ProyectoParametrosDto peticionObtenerProyecto)
        {
            try
            {
                if (!await ValidarParametrosRequest(peticionObtenerProyecto).ConfigureAwait(false)) return ResponseMessage(base.RespuestaAutorizacion);
                var result = await _serviciosNegocioServicios.ObtenerListaCatalogoDT(peticionObtenerProyecto, CatalogoEnum.SubDireccionTecnica).ConfigureAwait(false);
                if (!string.IsNullOrEmpty(peticionObtenerProyecto.IdFiltro))
                {
                    int filtro = Convert.ToInt32(peticionObtenerProyecto.IdFiltro);
                    result = result.Where(x => x.DireccionTecnicaId == filtro).ToList();
                }
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        /// <summary>
        /// obtiene los analistas segun la subdirecciones tecnicas
        /// </summary>
        /// <param name="peticionObtenerProyecto">Contiene informacion de autorizacion</param>
        /// <returns>lista de subdireeciones tecnicas</returns>
        [Route("api/Proyecto/ObtenerListaAnalistasSubDireccionTecnica")]
        [HttpPost]
        public async Task<IHttpActionResult> ObtenerListaAnalistasSubDireccionTecnica(ProyectoParametrosDto peticionObtenerProyecto)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                                              RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                                              ConfigurationManager.AppSettings["IdAplicacionBackbone"],
                                                                              ConfigurationManager.AppSettings["idCambiarEstadoConfiguracionRolSector"]).Result;

                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                var result = await Task.Run(() => _autorizacionUtilidades.ObtenerUsuariosPorSubDireccionTecnica(peticionObtenerProyecto));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/Proyecto/SolicitarConcepto")]
        [HttpPost]
        public async Task<IHttpActionResult> SolicitarConcepto(ProyectoParametrosDto peticionObtenerProyecto)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                                              RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                                              ConfigurationManager.AppSettings["IdAplicacionBackbone"],
                                                                              ConfigurationManager.AppSettings["idCambiarEstadoConfiguracionRolSector"]).Result;

                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                var result = await Task.Run(() => _serviciosNegocioServicios.SolicitarConcepto(peticionObtenerProyecto));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/Proyecto/ObtenerSolicitarConcepto")]
        [HttpPost]
        public async Task<IHttpActionResult> ObtenerSolicitarConcepto(ProyectoParametrosDto peticionObtenerProyecto)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                                              RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                                              ConfigurationManager.AppSettings["IdAplicacionBackbone"],
                                                                              ConfigurationManager.AppSettings["idCambiarEstadoConfiguracionRolSector"]).Result;

                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                var result = await Task.Run(() => _serviciosNegocioServicios.ObtenerSolicitarConcepto(peticionObtenerProyecto));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/proyecto/DevolverProyecto")]
        [HttpPost]
        public async Task<IHttpActionResult> DevolverProyecto(DevolverProyectoDto ppt)
        {
            try
            {
                var result = await Task.Run(() => _serviciosNegocioServicios.DevolverProyecto(ppt, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/proyecto/ObtenerEncabezadoGeneral")]
        [HttpPost]
        public async Task<IHttpActionResult> ObtenerEncabezadoGeneral(ProyectoParametrosEncabezadoDto ppt)
        {
            try
            {
                var result = await Task.Run(() => _serviciosNegocioServicios.ObtenerEncabezadoGeneral(ppt, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/proyecto/ObtenerEncabezadoSGR")]
        [HttpPost]
        public async Task<IHttpActionResult> ObtenerEncabezadoSGR(ProyectoParametrosEncabezadoDto ppt)
        {
            try
            {
                var result = await Task.Run(() => _serviciosNegocioServicios.ObtenerEncabezadoSGR(ppt, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/proyecto/ObtenerEncabezadoSGP")]
        [HttpPost]
        public async Task<IHttpActionResult> ObtenerEncabezadoSGP(ProyectoParametrosEncabezadoDto ppt)
        {
            try
            {
                var result = await Task.Run(() => _serviciosNegocioServicios.ObtenerEncabezadoSGP(ppt, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/Proyecto/actualizarHorizonte")]
        [HttpPost]
        public async Task<IHttpActionResult> actualizarHorizonte(HorizonteProyectoDto parametrosHorizonte)
        {
            try
            {
                var result = await Task.Run(() => _proyectoServicios.actualizarHorizonte(parametrosHorizonte, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        /// <summary>
        /// Api para lista de regionalizacion del proyectos.
        /// </summary>
        /// <param name="bpin">Contiene informacion del proyecto</param>
        /// <returns>Lista de Regionalizacion</returns>
        [Route("api/Proyecto/ObtenerDesagregarRegionalizacion")]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerDesagregarRegionalizacion(string bpin)
        {
            try
            {
                var result = await Task.Run(() => _serviciosNegocioServicios.ObtenerDesagregarRegionalizacion(bpin, UsuarioLogadoDto.IdUsuario, Request.Headers.Authorization.Parameter));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/Proyecto/ActualizarDesagregarRegionalizacion")]
        [HttpPost]
        public async Task<IHttpActionResult> ActualizarDesagregarRegionalizacion(DesagregarRegionalizacionDto parametros)
        {
            try
            {
                var result = await Task.Run(() => _serviciosNegocioServicios.ActualizarDesagregarRegionalizacion(parametros, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/Proyecto/ObtenerListaFondo")]
        [HttpPost]
        public async Task<IHttpActionResult> ObtenerListaFondo(ProyectoParametrosDto peticionObtenerProyecto)
        {
            try
            {
                if (!await ValidarParametrosRequest(peticionObtenerProyecto).ConfigureAwait(false)) return ResponseMessage(base.RespuestaAutorizacion);
                var result = await _serviciosNegocioServicios.ObtenerListaCatalogoEntidades(peticionObtenerProyecto, CatalogoEnum.Fondos).ConfigureAwait(false);

                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/Proyecto/ObtenerListaTipoCofinanciador")]
        [HttpPost]
        public async Task<IHttpActionResult> ObtenerListaTipoCofinanciador(ProyectoParametrosDto peticionObtenerProyecto)
        {
            try
            {
                if (!await ValidarParametrosRequest(peticionObtenerProyecto).ConfigureAwait(false)) return ResponseMessage(base.RespuestaAutorizacion);
                var result = await _serviciosNegocioServicios.ObtenerListaCatalogoEntidades(peticionObtenerProyecto, CatalogoEnum.TipoCofinanciador).ConfigureAwait(false);

                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/Proyecto/ObtenerListaRubro")]
        [HttpPost]
        public async Task<IHttpActionResult> ObtenerListaRubro(ProyectoParametrosDto peticionObtenerProyecto)
        {
            try
            {
                if (!await ValidarParametrosRequest(peticionObtenerProyecto).ConfigureAwait(false)) return ResponseMessage(base.RespuestaAutorizacion);
                var result = await _serviciosNegocioServicios.ObtenerListaCatalogoEntidades(peticionObtenerProyecto, CatalogoEnum.Rubros).ConfigureAwait(false);

                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/Proyecto/ObtenerProyectosBpin")]
        [HttpPost]
        public async Task<IHttpActionResult> ObtenerProyectosBpin(string bpin)
        {
            try
            {
                var result = await Task.Run(() => _proyectoServicios.ObtenerProyectosBpin(bpin, UsuarioLogadoDto.IdUsuario, Request.Headers.Authorization.Parameter));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/proyecto/ObtenerIndicadoresProducto")]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerIndicadoresProducto(string bpin)
        {
            try
            {
                var result = await Task.Run(() => _proyectoServicios.ObtenerIndicadoresProducto(bpin, UsuarioLogadoDto.IdUsuario, Request.Headers.Authorization.Parameter));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/proyecto/GuardarIndicadoresSecundarios")]
        [HttpPost]
        public async Task<IHttpActionResult> GuardarIndicadoresSecundarios(AgregarIndicadoresSecundariosDto parametros)
        {
            try
            {
                var result = await Task.Run(() => _proyectoServicios.GuardarIndicadoresSecundarios(parametros, User.Identity.Name));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/proyecto/EliminarIndicadorProducto")]
        [HttpGet]
        public async Task<IHttpActionResult> EliminarIndicadorProducto(int indicadorId)
        {
            try
            {
                var result = await Task.Run(() => _proyectoServicios.EliminarIndicadorProducto(indicadorId, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/proyecto/ObtenerResumenObjetivosProductosActividades")]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerResumenObjetivosProductosActividades(string bpin)
        {
            try
            {
                var result = await Task.Run(() => _proyectoServicios.ObtenerResumenObjetivosProductosActividades(bpin, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/proyecto/GuardarCostoActividades")]
        [HttpPost]
        public async Task<IHttpActionResult> GuardarCostoActividades(ProductoAjusteDto producto, string usuarioDNP)
        {
            try
            {
                var result = await Task.Run(() => _proyectoServicios.GuardarCostoActividades(producto, usuarioDNP));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/proyecto/ActualizarMetaAjusteIndicador")]
        [HttpPost]
        public async Task<IHttpActionResult> ActualizarMetaAjusteIndicador(IndicadoresIndicadorProductoDto Indicador)
        {
            try
            {
                var result = await Task.Run(() => _proyectoServicios.ActualizarMetaAjusteIndicador(Indicador, User.Identity.Name));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }


        [Route("api/proyecto/AgregarEntregable")]
        [HttpPost]
        public async Task<IHttpActionResult> AgregarEntregable(AgregarEntregable[] entregables, string usuarioDNP)
        {
            try
            {
                var result = await Task.Run(() => _proyectoServicios.AgregarEntregable(entregables, usuarioDNP));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/proyecto/EliminarEntregable")]
        [HttpPost]
        public async Task<IHttpActionResult> EliminarEntregable(EntregablesActividadesDto entregable, string usuarioDNP)
        {
            try
            {
                var result = await Task.Run(() => _proyectoServicios.EliminarEntregable(entregable, usuarioDNP));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/Proyecto/ObtenerListaAgrupaciones")]
        [HttpPost]
        public async Task<IHttpActionResult> ObtenerListaAgrupaciones(ProyectoParametrosDto peticionObtenerProyecto)
        {
            try
            {
                if (!await ValidarParametrosRequest(peticionObtenerProyecto).ConfigureAwait(false)) return ResponseMessage(base.RespuestaAutorizacion);
                var result = await _serviciosNegocioServicios.ObtenerListaCatalogoEntidades(peticionObtenerProyecto, CatalogoEnum.Agrupaciones).ConfigureAwait(false);

                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/Proyecto/ObtenerListaTipoAgrupaciones")]
        [HttpPost]
        public async Task<IHttpActionResult> ObtenerListaTipoAgrupacion(ProyectoParametrosDto peticionObtenerProyecto)
        {
            try
            {
                if (!await ValidarParametrosRequest(peticionObtenerProyecto).ConfigureAwait(false)) return ResponseMessage(base.RespuestaAutorizacion);
                var result = await _serviciosNegocioServicios.ObtenerListaCatalogoEntidades(peticionObtenerProyecto, CatalogoEnum.TiposAgrupaciones).ConfigureAwait(false);

                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        /// <summary>
        /// Api para detener instancia del proyecto.
        /// </summary>
        /// <param name="instanciaProyectoDto"></param>
        /// <returns>Objeto con propiedades para detener instancia.</returns>
        [Route("api/Proyecto/DevolverInstanciasHijas")]
        [HttpPost]
        public async Task<IHttpActionResult> DevolverInstanciasHijas(InstanciaProyectoDto instanciaProyectoDto)
        {
            try
            {
                if (!await ValidarParametrosRequest(instanciaProyectoDto.ProyectoParametrosDto)) return ResponseMessage(base.RespuestaAutorizacion);
                var result = await _proyectoServicios.DevolverInstanciasHijas(instanciaProyectoDto.ProyectoParametrosDto, Request.Headers.Authorization.Parameter).ConfigureAwait(false);
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }
        //ajuste para despliegue en servicios
        [Route("api/Localizacion/GuardarLocalizacion")]
        [HttpPost]
        public async Task<ResultadoProcedimientoDto> guardarLocalizacion(LocalizacionProyectoAjusteDto objLocalizacion, string usuarioDNP)
        {
            try
            {
                var result = await _serviciosNegocioServicios.guardarLocalizacion(objLocalizacion,usuarioDNP, Request.Headers.Authorization.Parameter).ConfigureAwait(false);

                return (result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        /// <summary>
        /// Api para lista de Departamentos.
        /// </summary>
        /// <param name="peticion">Contiene informacion de autorizacion</param>
        /// <returns>Lista de sectores</returns>
        [Route("api/Catalogo/Departamentos")]
        [HttpPost]
        public async Task<List<DepartamentoCatalogoDto>> ConsultarDepartamentosRegion()
        {
            try
            {
                var result = await _serviciosNegocioServicios.obtenerDepartamento(UsuarioLogadoDto.IdUsuario).ConfigureAwait(false);
                return result;
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        /// <summary>
        /// Api para lista de Departamentos.
        /// </summary>
        /// <param name="peticion">Contiene informacion de autorizacion</param>
        /// <returns>Lista de sectores</returns>
        [Route("api/Catalogo/Agrupaciones")]
        [HttpPost]
        public async Task<List<AgrupacionCodeDto>> ConsultarAgrupacionesCompleta()
        {
            try
            {
                var result = await _serviciosNegocioServicios.ConsultarAgrupacionesCompleta(UsuarioLogadoDto.IdUsuario).ConfigureAwait(false);
                return result;
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        /// <summary>
        /// Api para lista de Muinicipios.
        /// </summary>
        /// <param name="peticion">Contiene informacion de autorizacion</param>
        /// <returns>Lista de sectores</returns>
        [Route("api/Catalogo/Municipios")]
        [HttpPost]
        public async Task<IHttpActionResult> ObtenerMunicipios(EntidadesPorCodigoParametrosDto peticion)
        {
            try
            {
                //if (!await ValidarParametrosRequest(peticion).ConfigureAwait(false)) return ResponseMessage(base.RespuestaAutorizacion);
                var result = await _serviciosNegocioServicios.ObtenerCatalogoReferencia(peticion, CatalogoEnum.Departamentos.ToString(), peticion.IdDepartamento, CatalogoEnum.Municipios.ToString()).ConfigureAwait(false);

                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/proyecto/IndicadoresValidarCapituloModificado")]
        [HttpGet]
        public async Task<IHttpActionResult> IndicadoresValidarCapituloModificado(string bpin)
        {
            try
            {
                var result = await Task.Run(() => _proyectoServicios.IndicadoresValidarCapituloModificado(bpin, UsuarioLogadoDto.IdUsuario, Request.Headers.Authorization.Parameter));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/proyecto/RegionalizacionGeneral")]
        [HttpGet]
        public async Task<IHttpActionResult> RegionalizacionGeneral(string bpin)
        {
            try
            {
                var result = await Task.Run(() => _proyectoServicios.RegionalizacionGeneral(bpin, UsuarioLogadoDto.IdUsuario, Request.Headers.Authorization.Parameter));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/proyecto/GuardarRegionalizacionFuentesFinanciacionAjustes")]
        [HttpPost]
        public async Task<IHttpActionResult> AgregarRegionalizacionFuentesFinanciacionAjustes(List<RegionalizacionFuenteAjusteDto> regionalizacionFuenteAjuste, string usuarioDNP)
        {
            try
            {
                var result = await Task.Run(() => _proyectoServicios.GuardarRegionalizacionFuentesFinanciacionAjustes(regionalizacionFuenteAjuste, usuarioDNP));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/proyecto/ObtenerResumenObjetivosProductosActividadesJustificacion")]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerResumenObjetivosProductosActividadesJustificacion(string bpin)
        {
            try
            {
                var result = await Task.Run(() => _proyectoServicios.ObtenerResumenObjetivosProductosActividadesJustificacion(bpin, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/proyecto/ObtenerJustificacionLocalizacionProyecto")]
        [HttpGet]
        public async Task<LocalizacionJustificacionProyectoDto> ObtenerJustificacionLocalizacionProyecto(int proyectoId)
        {
            try
            {
                var result = await Task.Run(() => _proyectoServicios.ObtenerJustificacionLocalizacionProyecto(proyectoId, UsuarioLogadoDto.IdUsuario));
                return result;
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/proyecto/GuardarFocalizacionCategoriasAjustes")]
        [HttpPost]
        public async Task<RespuestaGeneralDto> GuardarFocalizacionCategoriasAjustes(List<FocalizacionCategoriasAjusteDto> focalizacionCategoriasAjuste, string usuarioDNP)
        {
            try
            {
                var result = await Task.Run(() => _proyectoServicios.GuardarFocalizacionCategoriasAjustes(focalizacionCategoriasAjuste, usuarioDNP));
                return result;
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/Proyecto/ObtenerDetalleAjustesJustificaionRegionalizacion")]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerDetalleAjustesJustificaionRegionalizacion(string bpin, string usuarioDNP, string tokenAutorizacion)
        {
            try
            {
                var result = await Task.Run(() => _proyectoServicios.ObtenerDetalleAjustesJustificaionRegionalizacion(bpin, usuarioDNP, tokenAutorizacion));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/ObtenerInstanciaProyectoTramite")]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerInstanciaProyectoTramite(string InstanciaId, string BPIN)
        {
            var result = await Task.Run(() => _proyectoServicios.ObtenerInstanciaProyectoTramite(InstanciaId, BPIN, UsuarioLogadoDto.IdUsuario));

            return Ok(result);
        }

        [Route("api/Proyecto/ObtenerSeccionOtrasPoliticasFacalizacionPT")]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerSeccionOtrasPoliticasFacalizacionPT(string bpin, string usuarioDNP, string tokenAutorizacion)
        {
            try
            {
                var result = await Task.Run(() => _proyectoServicios.ObtenerSeccionOtrasPoliticasFacalizacionPT(bpin, usuarioDNP, tokenAutorizacion));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/Proyecto/ObtenerProyectosBeneficiarios")]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerProyectosBeneficiarios(string bpin, string usuarioDNP, string tokenAutorizacion)
        {
            try
            {
                var result = await Task.Run(() => _proyectoServicios.ObtenerProyectosBeneficiarios(bpin, usuarioDNP, tokenAutorizacion));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/Proyecto/ObtenerProyectosBeneficiariosDetalle")]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerProyectosBeneficiariosDetalle(string json, string usuarioDNP, string tokenAutorizacion)
        {
            try
            {
                var result = await Task.Run(() => _proyectoServicios.ObtenerProyectosBeneficiariosDetalle(json, usuarioDNP, tokenAutorizacion));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/Proyecto/ObtenerJustificacionProyectosBeneficiarios")]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerJustificacionProyectosBeneficiarios(string bpin, string usuarioDNP, string tokenAutorizacion)
        {
            try
            {
                var result = await Task.Run(() => _proyectoServicios.ObtenerJustificacionProyectosBeneficiarios(bpin, usuarioDNP, tokenAutorizacion));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/Proyecto/GuardarBeneficiarioTotales")]
        [HttpPost]
        public async Task<IHttpActionResult> GuardarBeneficiarioTotales(BeneficiarioTotalesDto beneficiario, string usuarioDNP)
        {
            try
            {
                var result = await Task.Run(() => _proyectoServicios.GuardarBeneficiarioTotales(beneficiario, usuarioDNP));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/Proyecto/GuardarBeneficiarioProducto")]
        [HttpPost]
        public async Task<IHttpActionResult> GuardarBeneficiarioProducto(BeneficiarioProductoDto beneficiario, string usuarioDNP)
        {
            try
            {
                var result = await Task.Run(() => _proyectoServicios.GuardarBeneficiarioProducto(beneficiario, usuarioDNP));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/Proyecto/GuardarBeneficiarioProductoLocalizacion")]
        [HttpPost]
        public async Task<IHttpActionResult> GuardarBeneficiarioProductoLocalizacion(BeneficiarioProductoLocalizacionDto beneficiario, string usuarioDNP)
        {
            try
            {
                var result = await Task.Run(() => _proyectoServicios.GuardarBeneficiarioProductoLocalizacion(beneficiario, usuarioDNP));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/Proyecto/GuardarBeneficiarioProductoLocalizacionCaracterizacion")]
        [HttpPost]
        public async Task<IHttpActionResult> GuardarBeneficiarioProductoLocalizacionCaracterizacion(BeneficiarioProductoLocalizacionCaracterizacionDto beneficiario, string usuarioDNP)
        {
            try
            {
                var result = await Task.Run(() => _proyectoServicios.GuardarBeneficiarioProductoLocalizacionCaracterizacion(beneficiario, usuarioDNP));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/Proyecto/ObtenerSeccionPoliticaFocalizacionDT")]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerSeccionPoliticaFocalizacionDT(string bpin, string usuarioDNP, string tokenAutorizacion)
        {
            try
            {
                var result = await Task.Run(() => _proyectoServicios.ObtenerSeccionPoliticaFocalizacionDT(bpin, usuarioDNP, tokenAutorizacion));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        /// <summary>
        /// obtiene los analistas segun la subdirecciones por rol RValidadorPoliticaTransversal
        /// </summary>
        /// <param name="peticionObtenerProyecto">Contiene informacion de autorizacion</param>
        /// <returns>lista de subdireeciones tecnicas</returns>
        [Route("api/Proyecto/ObtenerUsuariosRValidadorPoliticaTransversal")]
        [HttpPost]
        public async Task<IHttpActionResult> ObtenerUsuariosRValidadorPoliticaTransversal(ProyectoParametrosDto peticionObtenerProyecto)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                                              RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                                              ConfigurationManager.AppSettings["IdAplicacionBackbone"],
                                                                              ConfigurationManager.AppSettings["idCambiarEstadoConfiguracionRolSector"]).Result;

                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                var result = await Task.Run(() => _autorizacionUtilidades.ObtenerUsuariosRValidadorPoliticaTransversal(peticionObtenerProyecto));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        /// <summary>
        /// obtiene los analistas segun la subdirecciones por rol RValidadorPoliticaTransversal
        /// </summary>
        /// <param name="peticionObtenerProyecto">Contiene informacion de autorizacion</param>
        /// <returns>lista de subdireeciones tecnicas</returns>
        [Route("api/Proyecto/GuardarReprogramacionPorProductoVigencia")]
        [HttpPost]
        public async Task<IHttpActionResult> GuardarReprogramacionPorProductoVigencia(List<ReprogramacionValores> reprogramacionValores)
        {
            try
            {

                var result = await Task.Run(() => _proyectoServicios.GuardarReprogramacionPorProductoVigencia(reprogramacionValores, RequestContext.Principal.Identity.Name));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        /// <summary>
        /// Obtener documento del proyecto
        /// </summary>
        /// <param name="filtroDocumentosProyecto">filtro de la consulta de documentos</param>
        /// <returns>Listado de documentos encontrados al proyecto</returns>
        /// <exception cref="HttpResponseException">Error en caso de no encontrar proyectos</exception>
        [Route("api/Proyecto/ObtenerSoportesProyecto")]
        [HttpPost]
        public async Task<IHttpActionResult> ObtenerSoportesProyecto(FiltroDocumentosDto filtroDocumentosProyecto)
        {
            try
            {
                var result = await Task.Run(() => _proyectoServicios.ObtenerDocumentosProyecto(filtroDocumentosProyecto, RequestContext.Principal.Identity.Name));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }


        /// <summary>
        /// Método que devuelve el nombre del archivo concatenando fecha y hora
        /// </summary>
        /// <param name="fuente"></param>
        /// <returns>String: Devuelve el nombre del archivo</returns>
        private string nombreDelArchivo(string fuente)
        {
            var data = $"{DateTime.Now.Year}-{DateTime.Now.Month.ToString().PadLeft(2, '0')}-{DateTime.Now.Day.ToString().PadLeft(2, '0')}";
            var hora = $"{DateTime.Now.TimeOfDay.Hours}h{DateTime.Now.TimeOfDay.Minutes}m{DateTime.Now.TimeOfDay.Seconds.ToString().PadLeft(2, '0')}";
            return $"{fuente}_{data}_{hora}";
        }

        /// <summary>
        /// Obtener información del PND
        /// </summary>
        /// <param name="idProyecto">Identificación del proyecto</param>
        /// <returns>Información del PND encontrado</returns>
        /// <exception cref="HttpResponseException">Error en caso de no encontrar un Plan</exception>
        [Route("api/Proyecto/ObtenerPlanNacionalDesarrollo")]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerPlanNacionalDesarrollo(int idProyecto)
        {
            try
            {
                var result = await Task.Run(() => _proyectoServicios.ObtenerPND(idProyecto, RequestContext.Principal.Identity.Name));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }
        /// <summary>
        /// obtiene los analistas segun la subdirecciones tecnicas
        /// </summary>
        /// <param name="peticionObtenerProyecto">Contiene informacion de autorizacion</param>
        /// <returns>lista de subdireeciones tecnicas</returns>
        [Route("api/Proyecto/ObtenerUsuariosBasicosPorRolEntidad")]
        [HttpPost]
        public async Task<IHttpActionResult> ObtenerUsuariosBasicosPorRolEntidad(EntidadRolDto entidadRol)
        {
            try
            {
                var respuestaAutorizacion = await _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                                              RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                                              ConfigurationManager.AppSettings["IdAplicacionBackbone"],
                                                                              ConfigurationManager.AppSettings["idObtenerUsuariosBasicosPorRolEntidad"]);

                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                var result = await Task.Run(() => _autorizacionUtilidades.ObtenerUsuariosBasicosPorRolEntidad(entidadRol, User.Identity.Name));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        /// <summary>
        /// Api para obtención de datos de proyectos por idUsuario, idObjeto y aplicación.
        /// </summary>
        /// <param name="instanciaProyectoDto"></param>
        /// <returns>Objeto con propiedades para realizar consulta de datos de proyectos.</returns>
        [Route("api/Proyecto/ObtenerProyectosVerificacionOcadPazSgr")]
        [HttpPost]
        public async Task<IHttpActionResult> ObtenerProyectosVerificacionOcadPazSgr(InstanciaProyectoVerificacionOcadPazSgrDto instanciaProyectoDto)
        {
            try
            {
                if (!await ValidarParametrosRequest(instanciaProyectoDto.ProyectoParametrosDto)) return ResponseMessage(base.RespuestaAutorizacion);
                var result = await Task.Run(() => _proyectoServicios.ObtenerProyectosVerificacionOcadPazSgr(instanciaProyectoDto.ProyectoParametrosDto, Request.Headers.Authorization.Parameter, instanciaProyectoDto.ProyectoFiltroDto));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

    }

}