// ReSharper disable ParameterOnlyUsedForPreconditionCheck.Local
namespace DNP.ServiciosNegocio.Web.API.Controllers.Transversales
{
    using System.Collections.Generic;
    using System.Configuration;
    using System.Net;
    using System.Net.Http;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Http;
    using Comunes;
    using Comunes.Autorizacion;
    using Comunes.Dto.ObjetosNegocio;
    using Comunes.Excepciones;
    using Dominio.Dto.Proyectos;
    using Servicios.Interfaces.Proyectos;
    using Swashbuckle.Swagger.Annotations;
    using DNP.ServiciosNegocio.Servicios.Interfaces.Transversales;
    using DNP.ServiciosNegocio.Dominio.Dto.Transversales;
    using System;

    /// <summary>
    /// Clase Api responsable de la gestión o estado del proyectos
    /// </summary>
    public class EstadoController : ApiController
    {
        private readonly IEstadoServicio _estadoServicio;
        private readonly IAutorizacionUtilidades _autorizacionUtilidades;

        /// <summary>
        /// Constructor de clases
        /// </summary>
        /// <param name="estadoServicio">Instancia de servicios de estado</param>
        /// <param name="autorizacionUtilidades">Instancia de servicios de autorizacion</param>       
        public EstadoController(IEstadoServicio estadoServicio, IAutorizacionUtilidades autorizacionUtilidades)
        {
            _estadoServicio = estadoServicio;
            _autorizacionUtilidades = autorizacionUtilidades;
        }

        /// <summary>
        /// Api para obtención de estados del proyectos.
        /// </summary>        
        /// <returns>Lista de estados del proyectos.</returns>
        [Route("api/Estado")]
        [SwaggerResponse(HttpStatusCode.OK, "Retorna estados", typeof(EstadoDto))]
        [HttpGet]
        public async Task<IHttpActionResult> ConsultarEstados()
        {
            var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                                               RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                                               ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                                               ConfigurationManager.AppSettings["ConsultarProyectosPorEntidadesYEstados"]).Result;

            if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);
            
            var result = await Task.Run(() => _estadoServicio.ConsultarEstados());

            return Responder(result);
        }

        /// <summary>
        /// tratamiento para lista de estados
        /// </summary>        
        /// <returns>Lista de estados del proyectos.</returns>
        private IHttpActionResult Responder(List<EstadoDto> listaEstado)
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