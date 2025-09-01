namespace DNP.Backbone.Web.API.Controllers
{
    using Comunes.Excepciones;
    using DNP.Backbone.Dominio.Dto;
    using DNP.Backbone.Dominio.Dto.Proyecto;
    using DNP.Backbone.Servicios.Interfaces.ServiciosNegocio;
    using DNP.Backbone.Web.API.Controllers.Base;
    using Servicios.Interfaces.Autorizacion;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web.Http;

    /// <summary>
    /// Clase responsable de la gestión de proyectos
    /// </summary>
    public class RelacionPlanificacionController : BackboneBase
    {
        private readonly IServiciosNegocioServicios _serviciosNegocioServicios;

        /// <summary>
        /// Constructor de clases SeccionesCapitulosController
        /// </summary>
        /// <param name="autorizacionUtilidades">Instancia de servicios de autorizacion</param>
        /// <param name="serviciosNegocioServicios">Instancia de servicios de Negocio Servicios</param>
        public RelacionPlanificacionController(
            IAutorizacionServicios autorizacionUtilidades,
            IServiciosNegocioServicios serviciosNegocioServicios)
            : base(autorizacionUtilidades)
        {
            _serviciosNegocioServicios = serviciosNegocioServicios;
        }

        [Route("api/CambiosFirme/ObtenerRelacionPlanificacion")]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerRelacionPlanificacion(int IdProyecto)
        {
            try
            {
                var seccionesCapitulos = await _serviciosNegocioServicios.CambiosRelacionPlanificacion(IdProyecto, UsuarioLogadoDto.IdUsuario);
                var result = new RelacionPlanificacionCambiosDto();
                if (seccionesCapitulos != null)
                {
                    result = new RelacionPlanificacionCambiosDto()
                    {
                        Nuevos = seccionesCapitulos.Where(p => p.Estado == "Nuevo").ToList(),
                        Eliminados = seccionesCapitulos.Where(p => p.Estado == "Borrado").ToList()
                    };
                }
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
            catch (Exception e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/CambiosFirme/guardarCambiosFirmeRelacionPlanificacion")]
        [HttpPost]
        public async Task<IHttpActionResult> GuardarCambiosFirmeRelacionPlanificacion(CapituloModificado parametros)
        {
            try
            {
                return Ok(await _serviciosNegocioServicios.GuardarCambiosRelacionPlanificacion(parametros, UsuarioLogadoDto.IdUsuario));
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/CambiosFirme/FocalizacionActualizaPoliticasModificadas")]
        [HttpPost]
        public async Task<IHttpActionResult> FocalizacionActualizaPoliticasModificadas(JustificacionPoliticaModificada parametros)
        {
            try
            {
                return Ok(await _serviciosNegocioServicios.FocalizacionActualizaPoliticasModificadas(parametros, UsuarioLogadoDto.IdUsuario));
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

    }
}