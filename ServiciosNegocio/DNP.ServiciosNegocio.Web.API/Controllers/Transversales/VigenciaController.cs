using DNP.ServiciosNegocio.Comunes;
using DNP.ServiciosNegocio.Comunes.Autorizacion;
using DNP.ServiciosNegocio.Servicios.Interfaces.Transversales;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace DNP.ServiciosNegocio.Web.API.Controllers.Transversales
{
    public class VigenciaController : ApiController
    {
        private readonly IVigenciaServicio _vigenciaServicio;
        private readonly IAutorizacionUtilidades _autorizacionUtilidades;
        /// <summary>
        /// Constructor del controlador
        /// </summary>
        /// <param name="vigenciaServicio">servicio de vigencias</param>
        /// /// <param name="autorizacionUtilidades">Instancia de servicios de autorizacion</param>       
        public VigenciaController(IVigenciaServicio vigenciaServicio, IAutorizacionUtilidades autorizacionUtilidades)
        {
            _vigenciaServicio = vigenciaServicio;
            _autorizacionUtilidades = autorizacionUtilidades;
        }
        /// <summary>
        /// Api para obtención de vigencias de los proyectos.
        /// </summary>        
        /// <returns>Lista de estados del proyectos.</returns>
        [Route("api/Vigencia")]
        [SwaggerResponse(HttpStatusCode.OK, "Retorna vigencias", typeof(List<int?>))]
        [HttpGet]
        public async Task<IHttpActionResult> ConsultarVigencias()
        {
            var result = await Task.Run(() => _vigenciaServicio.ObtenerVigencias());

            return Responder(result);
        }
        /// <summary>
        /// tratamiento para lista de estados
        /// </summary>        
        /// <returns>Lista de estados del proyectos.</returns>
        private IHttpActionResult Responder(List<int?> listaVigencias)
        {
            return listaVigencias != null ? Ok(listaVigencias) : CrearRespuestaNoFound();
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
