using DNP.Backbone.Comunes.Dto;
using DNP.Backbone.Comunes.Excepciones;
using DNP.Backbone.Servicios.Interfaces.Autorizacion;
using DNP.Backbone.Servicios.Interfaces.Monitoreo;
using DNP.Backbone.Servicios.Interfaces.Proyectos;
using DNP.Backbone.Servicios.Interfaces.ServiciosNegocio;
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
    public class MapColumnasController : Base.BackboneBase
    {
        private readonly IMapColumnasServicios _mapColumnasServicios;
        private readonly IAutorizacionServicios _autorizacionUtilidades;

        public MapColumnasController(IMapColumnasServicios mapColumnasServicios, IAutorizacionServicios autorizacionUtilidades)
            : base(autorizacionUtilidades)
        {
            _mapColumnasServicios = mapColumnasServicios;
            _autorizacionUtilidades = autorizacionUtilidades;
        }

        /// <summary>
        /// Api para obtención de datos de map columnas.
        /// </summary>
        /// <param name="mapColumnasFiltroDto"></param>
        /// <returns>Objeto con propiedades para realizar consulta de datos de map columnas.</returns>
        [Route("api/Monitoreo/MapColumnas")]
        [HttpPost]
        public async Task<IHttpActionResult> ObtenerMapColumnas(MapColumnasFiltroDto mapColumnasFiltroDto)
        {
            try
            {
                if (!await ValidarParametrosRequest(mapColumnasFiltroDto.ParametrosDto).ConfigureAwait(false)) return ResponseMessage(base.RespuestaAutorizacion);
                var result = await _mapColumnasServicios.ObtenerMapColumnas(mapColumnasFiltroDto).ConfigureAwait(false);
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }


    }
}