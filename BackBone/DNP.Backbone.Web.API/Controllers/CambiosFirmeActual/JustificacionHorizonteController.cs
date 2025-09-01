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
    public class JustificacionHorizonteController : BackboneBase
    {
        private readonly IServiciosNegocioServicios _serviciosNegocioServicios;

        /// <summary>
        /// Constructor de clases SeccionesCapitulosController
        /// </summary>
        /// <param name="autorizacionUtilidades">Instancia de servicios de autorizacion</param>
        /// <param name="serviciosNegocioServicios">Instancia de servicios de Negocio Servicios</param>
        public JustificacionHorizonteController(
            IAutorizacionServicios autorizacionUtilidades,
            IServiciosNegocioServicios serviciosNegocioServicios)
            : base(autorizacionUtilidades)
        {
            _serviciosNegocioServicios = serviciosNegocioServicios;
        }

        [Route("api/CambiosFirme/ObtenerJustificacionHorizonte")]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerJustificacionHorizonte(int IdProyecto)
        {
            try
            {
                var seccionesCapitulos = await _serviciosNegocioServicios.ObtenerJustificacionHorizonte(IdProyecto, UsuarioLogadoDto.IdUsuario);
                var result = new JustificacionHorizonteCambiosDto();
                if (seccionesCapitulos != null)
                {

                    result = new JustificacionHorizonteCambiosDto()
                    {
                        Vigencia = seccionesCapitulos,
                        VigenciaFirme = seccionesCapitulos
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

        [Route("api/CambiosFirme/guardarCambiosFirmeJustificacionHorizonte")]
        [HttpPost]
        public async Task<IHttpActionResult> guardarCambiosFirmeJustificacionHorizonte(CapituloModificado parametros)
        {
            try
            {
                return Ok(await _serviciosNegocioServicios.GuardarCambiosJustificacionHorizonte(parametros, UsuarioLogadoDto.IdUsuario));
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

    }
}