// ReSharper disable ParameterOnlyUsedForPreconditionCheck.Local
namespace DNP.ServiciosNegocio.Web.API.Controllers.Transversales
{
    using Comunes;
    using Comunes.Autorizacion;
    using DNP.ServiciosNegocio.Dominio.Dto.SGR;
    using DNP.ServiciosNegocio.Servicios.Interfaces.SGR.DelegarViabilidad;
    using Swashbuckle.Swagger.Annotations;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web.Http;

    /// <summary>
    /// Clase Api responsable de la gestión o estado del proyectos
    /// </summary>
    public class EjecutorEntidadController : ApiController
    {
        private readonly IEjecutorEntidadServicio _ejecutorEntidadServicio;
        private readonly IAutorizacionUtilidades _autorizacionUtilidades;

        /// <summary>
        /// Constructor de clases
        /// </summary>
        /// <param name="estadoServicio">Instancia de servicios de estado</param>
        /// <param name="autorizacionUtilidades">Instancia de servicios de autorizacion</param>       
        public EjecutorEntidadController(IEjecutorEntidadServicio ejecutorEntidadServicio, IAutorizacionUtilidades autorizacionUtilidades)
        {
            _ejecutorEntidadServicio = ejecutorEntidadServicio;
            _autorizacionUtilidades = autorizacionUtilidades;
        }

        /// <summary>
        /// Api para obtención de estados del proyectos.
        /// </summary>        
        /// <returns>Lista de estados del proyectos.</returns>
        [Route("api/ConsultarListadoEjecutores")]
        [SwaggerResponse(HttpStatusCode.OK, "Retorna ejecutores", typeof(EjecutorEntidadDto))]
        [HttpGet]
        public async Task<IHttpActionResult> ConsultarListadoEjecutores(string nit = "", int? tipoEntidadId = null, int? entidadId = null)
        {
            var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                                               RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                                               ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                                               ConfigurationManager.AppSettings["ConsultarProyectosPorEntidadesYEstados"]).Result;

            if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

            var result = await Task.Run(() => _ejecutorEntidadServicio.ConsultarListadoEjecutores(nit, tipoEntidadId, entidadId));

            return Responder(result);
        }

        /// <summary>
        /// Obtiene los ejecutores asociados al proyecto
        /// </summary>
        /// <param name="proyectoId"></param>
        /// <returns></returns>
        [Route("api/ConsultarListadoEjecutoresAsociados")]
        [SwaggerResponse(HttpStatusCode.OK, "Retorna ejecutores", typeof(EjecutorEntidadAsociado))]
        [HttpGet]
        public async Task<IHttpActionResult> ConsultarListadoEjecutoresAsociados(int proyectoId)
        {
            var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                                               RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                                               ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                                               ConfigurationManager.AppSettings["ConsultarProyectosPorEntidadesYEstados"]).Result;

            if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

            var result = await Task.Run(() => _ejecutorEntidadServicio.ObtenerListadoEjecutoresAsociados(proyectoId));

            return Responder(result);
        }

        /// <summary>
        /// Crear un nuevo ejecutor asociado
        /// </summary>
        /// <param name="proyectoId"></param>
        /// <returns></returns>
        [Route("api/CrearEjecutorAsociado")]
        [SwaggerResponse(HttpStatusCode.OK, "Retorna ejecutores", typeof(bool))]
        [HttpPost]
        public async Task<IHttpActionResult> CrearEjecutorAsociado(int proyectoId, int ejecutorId, string usuario, int tipoEjecutorId)
        {
            var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                                               RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                                               ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                                               ConfigurationManager.AppSettings["ConsultarProyectosPorEntidadesYEstados"]).Result;

            if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

            var result = await Task.Run(() => _ejecutorEntidadServicio.CrearEjecutorAsociado(proyectoId, ejecutorId, usuario, tipoEjecutorId));

            return Responder(result);
        }

        [Route("api/EliminarEjecutorAsociado")]
        [SwaggerResponse(HttpStatusCode.OK, "Elimina ejecutores", typeof(SeccionesEjecutorEntidad))]
        [HttpPost]
        public async Task<IHttpActionResult> EliminarEjecutorAsociado(int EjecutorAsociadoId, string usuario)
        {
            var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                                               RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                                               ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                                               ConfigurationManager.AppSettings["ConsultarProyectosPorEntidadesYEstados"]).Result;

            if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

            var result = await Task.Run(() => _ejecutorEntidadServicio.EliminarEjecutorAsociado(EjecutorAsociadoId, usuario));

            return Responder(result);
        }

        private IHttpActionResult Responder(SeccionesEjecutorEntidad result)
        {
            return result != null ? Ok(result) : CrearRespuestaNoFound();
        }

        private IHttpActionResult Responder(bool result)
        {
            return Ok(result);
        }

        private IHttpActionResult Responder(List<EjecutorEntidadAsociado> result)
        {
            return result != null ? Ok(result) : CrearRespuestaNoFound();
        }

        /// <summary>
        /// tratamiento para lista de estados
        /// </summary>        
        /// <returns>Lista de estados del proyectos.</returns>
        private IHttpActionResult Responder(List<EjecutorEntidadDto> listaEstado)
        {
            return listaEstado != null ? Ok(listaEstado) : CrearRespuestaNoFound();
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
    }
}