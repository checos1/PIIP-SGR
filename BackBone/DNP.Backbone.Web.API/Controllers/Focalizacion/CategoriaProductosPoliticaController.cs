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
    using DNP.Backbone.Dominio.Dto.Focalizacion;

    /// <summary>
    /// Clase Api responsable de la gestión de trámites
    /// </summary>
    public class CategoriaProductosPoliticaController : Base.BackboneBase
    {
        private readonly ICategoriaProductosPoliticaServicios _categoriaProductosPoliticaServicios;
        private readonly IAutorizacionServicios _autorizacionUtilidades;

        /// <summary>
        /// Categoria Productos Politica
        /// </summary>
        /// <param name="politicasTransversalesFuentesServicios"></param>
        /// <param name="autorizacionUtilidades"></param>
        public CategoriaProductosPoliticaController(ICategoriaProductosPoliticaServicios categoriaProductosPoliticaServicios, 
            IAutorizacionServicios autorizacionUtilidades)
            : base(autorizacionUtilidades)
        {
            _categoriaProductosPoliticaServicios = categoriaProductosPoliticaServicios;
            _autorizacionUtilidades = autorizacionUtilidades;
        }

        #region Obtener Categoria Productos Politica

        /// <summary>
        /// Obtener Categoria Productos Politica
        /// </summary>
        /// <param name="bpin"></param>
        /// <returns></returns>
        /// <exception cref="HttpResponseException"></exception>
        [Route("api/Focalizacion/ObtenerCategoriaProductosPolitica")]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerCategoriaProductosPolitica(string bpin, int fuenteId, int politicaId)
        {
            try
            {
                var result = await Task.Run(() =>
                _categoriaProductosPoliticaServicios.ObtenerCategoriaProductosPolitica(bpin, fuenteId, politicaId, UsuarioLogadoDto.IdUsuario, 
                Request.Headers.Authorization.Parameter));

                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        #endregion

        #region Guardar datos Solicitud de recursos.

        /// <summary>
        /// Guardar datos Solicitud de recursos.
        /// </summary>
        /// <param name="bpin"></param>
        /// <returns></returns>
        /// <exception cref="HttpResponseException"></exception>
        [Route("api/Focalizacion/guardarDatosSolicitudRecursos")]
        [HttpPost]
        public async Task<IHttpActionResult> GuardarDatosSolicitudRecursos(CategoriaProductoPoliticaDto categoriaProductoPoliticaDto)
        {
            try
            {
                var result = await Task.Run(() =>
                _categoriaProductosPoliticaServicios.GuardarDatosSolicitudRecursos(categoriaProductoPoliticaDto, UsuarioLogadoDto.IdUsuario,
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

