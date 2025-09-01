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
    public class IndicadoresPoliticaController : Base.BackboneBase
    {
        private readonly IIndicadoresPolitica _indicadoresPolitica;
        private readonly IAutorizacionServicios _autorizacionUtilidades;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="politicasTransversalesFuentesServicios"></param>
        /// <param name="autorizacionUtilidades"></param>
        public IndicadoresPoliticaController(IIndicadoresPolitica indicadoresPolitica, 
            IAutorizacionServicios autorizacionUtilidades)
            : base(autorizacionUtilidades)
        {
            _indicadoresPolitica = indicadoresPolitica;
            _autorizacionUtilidades = autorizacionUtilidades;
        }

        #region Obtener Indicadores Politica

        /// <summary>
        /// Obtener Indicadores Politica
        /// </summary>
        /// <param name="bpin"></param>
        /// <returns></returns>
        /// <exception cref="HttpResponseException"></exception>
        [Route("api/Focalizacion/ObtenerIndicadoresPolitica")]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerIndicadoresPolitica(string bpin)
        {
            try
            {
                var result = await Task.Run(() => 
                _indicadoresPolitica.ObtenerIndicadoresPolitica(bpin, UsuarioLogadoDto.IdUsuario, 
                Request.Headers.Authorization.Parameter));

                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        #endregion


    }


}

