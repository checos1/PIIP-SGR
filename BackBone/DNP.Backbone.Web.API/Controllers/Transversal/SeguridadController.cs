using DNP.Backbone.Comunes.Dto;
using DNP.Backbone.Comunes.Excepciones;
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
using DNP.Backbone.Servicios.Interfaces.Autorizacion;
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
using DNP.Backbone.Dominio.Dto.Acciones;

namespace DNP.Backbone.Web.API.Controllers.Transversal
{
    public class SeguridadController : Base.BackboneBase
    {

        private readonly IServiciosNegocioServicios _serviciosNegocioServicios;
        /// <summary>
        /// Constructor de clases SeccionesCapitulosController
        /// </summary>
        public SeguridadController(
            IAutorizacionServicios autorizacionUtilidades,
            IServiciosNegocioServicios serviciosNegocioServicios)
            : base(autorizacionUtilidades)
        {
            _serviciosNegocioServicios = serviciosNegocioServicios;
        }



        /// <summary>
        /// Api para obtención de datos de proyectos do tramite.
        /// </summary>
        /// <param name="instanciaTramiteDto">Contiene informacion de autorizacion, filtro e columnas visibles</param>
        /// <returns>Objeto con propiedades para realizar consulta dos datos.</returns>
        [Route("api/Seguridad/PermisosAccionPaso")]
        [HttpPost]
        public async Task<IHttpActionResult> PermisosAccionPaso(AccionFlujoDto parametros)
        {
            try
            {
                var result = await Task.Run(() => _serviciosNegocioServicios.PermisosAccionPaso(parametros));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

    }
}