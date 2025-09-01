using System.Configuration;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using Swashbuckle.Swagger.Annotations;
using DNP.ServiciosNegocio.Dominio.Dto.Transversales;
using DNP.ServiciosNegocio.Servicios.Interfaces.SeguimientoControl;
using DNP.ServiciosNegocio.Comunes.Autorizacion;
using System.Collections.Generic;
using System.Net.Http;
using DNP.ServiciosNegocio.Comunes;
using System;
using DNP.ServiciosNegocio.Comunes.Excepciones;
using DNP.ServiciosNegocio.Dominio.Dto.SeguimientoControl;

namespace DNP.ServiciosNegocio.Web.API.Controllers.SeguimientoControl
{
    public class GestionSeguimientoController : ApiController
    {
        private readonly IGestionSeguimientoServicio _seccionCapituloServicio;
        private readonly IAutorizacionUtilidades _autorizacionUtilidades;

        /// <summary>
        /// Constructor SeccionCapituloController
        /// </summary>
        /// <param name="seccionCapituloServicio">Instancia de servicios de sección cápitulos</param>
        /// <param name="autorizacionUtilidades">Instancia de servicios de autorizacion</param>       
        public GestionSeguimientoController(
            IAutorizacionUtilidades autorizacionUtilidades,
            IGestionSeguimientoServicio seccionCapituloServicio)
        {
            _autorizacionUtilidades = autorizacionUtilidades;
            _seccionCapituloServicio = seccionCapituloServicio;
        }

        [Route("api/GestionSeguimiento/ObtenerErroresSeguimiento")]
        [SwaggerResponse(HttpStatusCode.OK, "Retorna listado de errores del proyecto", typeof(List<ErroresProyectoDto>))]
        [HttpPost]
        public async Task<IHttpActionResult> ObtenerErroresProyecto([FromBody] GestionSeguimientoDto proyecto)
        {
            var result = await Task.Run(() => _seccionCapituloServicio.ObtenerErroresProyecto(proyecto));
            return Ok(result);
        }


        [Route("api/GestionSeguimiento/UnidadesMedida")]
        [SwaggerResponse(HttpStatusCode.OK, "Retorna listado de ", typeof(List<TransversalSeguimientoDto>))]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerListadoUnidadesMedida()
        {
            var result = await Task.Run(() => _seccionCapituloServicio.ObtenerListadoUnidadesMedida());
            return Ok(result);
        }
    }
}