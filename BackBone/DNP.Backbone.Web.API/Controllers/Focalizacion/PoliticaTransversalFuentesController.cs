using System;

namespace DNP.Backbone.Web.API.Controllers
{
    using Comunes.Dto;
    using Comunes.Excepciones;
    using DNP.Backbone.Dominio.Dto;
    using DNP.Backbone.Dominio.Dto.Conpes;
    using DNP.Backbone.Servicios.Interfaces.ServiciosNegocio;
    using DNP.Backbone.Servicios.Interfaces.Focalizacion;
    using DNP.Backbone.Web.API.Controllers.Base;
    using Servicios.Interfaces.Autorizacion;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;
    using System.Web.Http;

    /// <summary>
    /// Clase Api responsable de la gestión de trámites
    /// </summary>
    public class PoliticaTransversalFuentesController : BackboneBase
    {
        private readonly IPoliticasTransversalesFuentesServicios _politicasTransversalesFuentesServicios;
        private readonly IAutorizacionServicios _autorizacionUtilidades;
        private readonly IServiciosNegocioServicios _serviciosNegocioServicios;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="politicasTransversalesFuentesServicios"></param>
        /// <param name="autorizacionUtilidades"></param>
        public PoliticaTransversalFuentesController(IPoliticasTransversalesFuentesServicios politicasTransversalesFuentesServicios, IAutorizacionServicios autorizacionUtilidades)
            : base(autorizacionUtilidades)
        {
            _politicasTransversalesFuentesServicios = politicasTransversalesFuentesServicios;
            _autorizacionUtilidades = autorizacionUtilidades;
        }


                
        [Route("api/Focalizacion/ObtenerFocalizacionPoliticasTransversalesFuentes")]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerFocalizacionPoliticasTransversalesFuentes(string bpin)
        {
            try
            {
                var result = await Task.Run(() => _politicasTransversalesFuentesServicios.ObtenerFocalizacionPoliticasTransversalesFuentes(bpin, UsuarioLogadoDto.IdUsuario, Request.Headers.Authorization.Parameter));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

      
    }


}

