using System.Configuration;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using Swashbuckle.Swagger.Annotations;
using DNP.ServiciosNegocio.Dominio.Dto.Transversales;
using DNP.ServiciosNegocio.Servicios.Interfaces.Transversales;
using DNP.ServiciosNegocio.Comunes.Autorizacion;
using System.Collections.Generic;
using System.Net.Http;
using DNP.ServiciosNegocio.Comunes;
using System;
using DNP.ServiciosNegocio.Comunes.Excepciones;

namespace DNP.ServiciosNegocio.Web.API.Controllers.Transversales
{
    public class SeccionCapituloController : ApiController
    {
        private readonly ISeccionCapituloServicio _seccionCapituloServicio;
        private readonly IAutorizacionUtilidades _autorizacionUtilidades;

        /// <summary>
        /// Constructor SeccionCapituloController
        /// </summary>
        /// <param name="seccionCapituloServicio">Instancia de servicios de sección cápitulos</param>
        /// <param name="autorizacionUtilidades">Instancia de servicios de autorizacion</param>       
        public SeccionCapituloController(
            IAutorizacionUtilidades autorizacionUtilidades,
            ISeccionCapituloServicio seccionCapituloServicio)
        {
            _autorizacionUtilidades = autorizacionUtilidades;
            _seccionCapituloServicio = seccionCapituloServicio;
        }

        /// <summary>
        /// Método que obtiene el listado de secciones con sus cápitulos consultando por GUID de la tabla [Transversal].[fase]
        /// </summary>
        /// <param name="guiMacroproceso">Identificador GUID de la tabla [Transversal].[fase]</param>
        /// <returns></returns>
        [Route("api/SeccionCapitulo/ObtenerCapitulosModificadosByMacroproceso")]
        [SwaggerResponse(HttpStatusCode.OK, "Retorna listado de secciones con sus cápitulos", typeof(SeccionCapituloDto))]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerCapitulosModificadosByMacroproceso(string guiMacroproceso, int IdProyecto, string IdInstancia)
        {
            var result = await Task.Run(() => _seccionCapituloServicio.ConsultarSeccionCapitulos(guiMacroproceso, IdProyecto, IdInstancia));
            return Responder(result);
        }

        /// <summary>
        /// Método que obtiene el listado de secciones con sus cápitulos consultando por GUID de la tabla [Transversal].[fase]
        /// </summary>
        /// <param name="guiMacroproceso">Identificador GUID de la tabla [Transversal].[fase]</param>
        /// <returns></returns>
        [Route("api/SeccionCapitulo/ObtenerSeccionCapitulosByMacroproceso")]
        [SwaggerResponse(HttpStatusCode.OK, "Retorna listado de secciones con sus cápitulos", typeof(SeccionCapituloDto))]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerSeccionCapitulosByMacroproceso(string guiMacroproceso,string NivelId, string FlujoId)
        {
            var result = await Task.Run(() => _seccionCapituloServicio.ConsultarSeccionCapitulosByMacroproceso(guiMacroproceso, NivelId, FlujoId));
            return Responder(result);
        }

        [Route("api/SeccionCapitulo/ValidarCapitulosByMacroproceso")]
        [SwaggerResponse(HttpStatusCode.OK, "Informacion devuelta por guardado", typeof(HttpResponseMessage))]
        [HttpGet]
        public async Task<IHttpActionResult> AdicionarProyectoConpes(string guiMacroproceso, int IdProyecto, string IdInstancia)
        {
            try
            {
                var result = await Task.Run(() => _seccionCapituloServicio.ValidarSeccionCapitulos(guiMacroproceso, IdProyecto, IdInstancia));

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

        /// <summary>
        /// tratamiento para lista de estados
        /// </summary>        
        /// <returns>Lista de estados del proyectos.</returns>
        private IHttpActionResult Responder(List<SeccionCapituloDto> listaSeccionCapitulo)
        {
            return listaSeccionCapitulo != null ? Ok(listaSeccionCapitulo) : CrearRespuestaNoFound();
        }

        /// <summary>
        /// tratamiento para HTTP Status Code 404
        /// </summary>        
        /// <returns>IHttpActionResult</returns>
        private IHttpActionResult CrearRespuestaNoFound()
        {
            var respuestaHttp = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.NotFound,
                ReasonPhrase = ServiciosNegocioRecursos.SinResultados
            };

            return ResponseMessage(respuestaHttp);
        }

        [Route("api/SeccionCapitulo/ObtenerCapitulosModificadosCapitoSeccion")]
        [SwaggerResponse(HttpStatusCode.OK, "Retorna cápitulos Modificados", typeof(CapituloModificado))]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerCapitulosModificados(string guiMacroproceso, int idProyecto, Guid idInstancia, string capitulo, string seccion)
        {
            var result = await Task.Run(() => _seccionCapituloServicio.ObtenerCapitulosModificados(capitulo, seccion, guiMacroproceso, idProyecto, idInstancia.ToString()));
            return Ok(result);
        }

        [Route("api/SeccionCapitulo/ObtenerErroresProyecto")]
        [SwaggerResponse(HttpStatusCode.OK, "Retorna listado de errores del proyecto", typeof(List<ErroresProyectoDto>))]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerErroresProyecto(string guidMacroproceso, string guidInstancia, int idProyecto)
        {
            var result = await Task.Run(() => _seccionCapituloServicio.ObtenerErroresProyecto(guidMacroproceso, idProyecto, guidInstancia));
            return Ok(result);
        }

        [Route("api/SeccionCapitulo/ObtenerErroresSeguimiento")]
        [SwaggerResponse(HttpStatusCode.OK, "Retorna listado de errores del proyecto", typeof(List<ErroresProyectoDto>))]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerErroresSeguimiento(string guidMacroproceso, string guidInstancia, int idProyecto)
        {
            var result = await Task.Run(() => _seccionCapituloServicio.ObtenerErroresSeguimiento(guidMacroproceso, idProyecto, guidInstancia));
            return Ok(result);
        }

        [Route("api/SeccionCapitulo/ObtenerErroresTramite")]
        [SwaggerResponse(HttpStatusCode.OK, "Retorna listado de errores del tramite", typeof(List<ErroresProyectoDto>))]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerErroresTramite(string guidMacroproceso, string guidInstancia, string accionId, string usuarioDNP, bool tieneCDP)
        {
            var result = await Task.Run(() => _seccionCapituloServicio.ObtenerErroresTramite(guidMacroproceso, guidInstancia, accionId, usuarioDNP, tieneCDP));
            return Ok(result);
        }

        [Route("api/SeccionCapitulo/ObtenerErroresViabilidad")]
        [SwaggerResponse(HttpStatusCode.OK, "Retorna listado de errores de Viabilidad", typeof(List<ErroresProyectoDto>))]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerErroresViabilidad(string GuiMacroproceso, int ProyectoId, string NivelId, string InstanciaId)
        {
            var result = await Task.Run(() => _seccionCapituloServicio.ObtenerErroresViabilidad(GuiMacroproceso, ProyectoId, NivelId, InstanciaId));
            return Ok(result);
        }

        [Route("api/SeccionCapitulo/ObtenerSeccionesTramite")]
        [SwaggerResponse(HttpStatusCode.OK, "Retorna listado de secciones del tramite", typeof(List<SeccionesTramiteDto>))]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerSeccionesTramite(string guidMacroproceso, string guidInstancia)
        {
            var result = await Task.Run(() => _seccionCapituloServicio.ObtenerSeccionesTramite(guidMacroproceso, guidInstancia));
            return Ok(result);
        }

        [Route("api/SeccionCapitulo/ObtenerSeccionesPorFase")]
        [SwaggerResponse(HttpStatusCode.OK, "Retorna listado de secciones por el Nivel o Fase para el caso es lo mismo", typeof(List<SeccionesTramiteDto>))]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerSeccionesPorFase(string guidInstancia, string guidFaseNivel)
        {
            var result = await Task.Run(() => _seccionCapituloServicio.ObtenerSeccionesPorFase(guidInstancia, guidFaseNivel));
            return Ok(result);
        }

        [Route("api/SeccionCapitulo/eliminarCapitulosModificados")]
        [SwaggerResponse(HttpStatusCode.OK, "Informacion devuelta por borrado", typeof(HttpResponseMessage))]
        [HttpPost]
        public async Task<IHttpActionResult> EliminarCapitulosModificados([FromBody] CapituloModificado capituloModificado)
        {
            try
            {
                var usuario = RequestContext.Principal.Identity.Name;
                var resultActualizacion = await Task.Run(() => _seccionCapituloServicio.EliminarCapituloModificado(capituloModificado));
                var result = new SeccionesCapitulos()
                {
                    Exito = resultActualizacion.Exito,
                    Mensaje = resultActualizacion.Mensaje != "" ? "Los datos fueron eliminados con éxito" : "No fue posible eliminar la información"
                };
                return Ok(result);
            }
            catch (Exception e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/SeccionCapitulo/ObtenerErroresAprobacionRol")]
        [SwaggerResponse(HttpStatusCode.OK, "Retorna listado de errores de Aprobacion Rol", typeof(List<ErroresProyectoDto>))]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerErroresAprobacionRol(string GuiMacroproceso, int idProyecto, string GuidInstancia)
        {
            var result = await Task.Run(() => _seccionCapituloServicio.ObtenerErroresAprobacionRol(GuiMacroproceso, idProyecto, GuidInstancia));
            return Ok(result);
        }
        [Route("api/SeccionCapitulo/ObtenerErroresProgramacion")]
        [SwaggerResponse(HttpStatusCode.OK, "Retorna listado de errores de la progranacion", typeof(List<ErroresProyectoDto>))]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerErroresProgramacion(string guidInstancia, string accionId)
        {
            var result = await Task.Run(() => _seccionCapituloServicio.ObtenerErroresProgramacion(guidInstancia, accionId));
            return Ok(result);
        }
    }
}