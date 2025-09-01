using System;

namespace DNP.Backbone.Web.API.Controllers
{
    using Comunes.Dto;
    using Comunes.Excepciones;
    using Comunes.Properties;
    using System.Configuration;
    using System.Net;
    using System.Threading.Tasks;
    using System.Web.Http;
    using Servicios.Interfaces.Autorizacion;
    using System.Net.Http;
    using DNP.Backbone.Servicios.Interfaces.ServiciosNegocio;
    using DNP.Backbone.Servicios.Interfaces.Tramites;
    using DNP.Backbone.Web.API.Controllers.Base;
    using System.Net.Http.Headers;
    using System.Web.Management;
    using DNP.Backbone.Servicios.Interfaces.Monitoreo;
    using DNP.Backbone.Dominio.Dto.Proyecto;
    using DNP.Backbone.Dominio.Dto.Tramites;
    using DNP.Backbone.Dominio.Dto.Tramites.Proyectos;
    using System.Collections.Generic;
    using DNP.Backbone.Servicios.Interfaces.Catalogo;

    /// <summary>
    /// Clase Api responsable de la gestión de trámites
    /// </summary>
    public class CatalogoController : BackboneBase
    {
        private readonly ICatalogoServicio _catalogoServicio;
        private readonly IAutorizacionServicios _autorizacionUtilidades;

        /// <summary>
        /// Constructor de clases
        /// </summary>
        /// <param name="tramiteServicios">Instancia de servicios de trámites</param>
        /// <param name="autorizacionUtilidades">Instancia de servicios de autorizacion</param>
        /// <param name="serviciosNegocioServicios">Instancia de servicios de Negocio Servicios</param>
        public CatalogoController(ICatalogoServicio catalogoServicios, IAutorizacionServicios autorizacionUtilidades)
            : base(autorizacionUtilidades)
        {
            _catalogoServicio = catalogoServicios;
            _autorizacionUtilidades = autorizacionUtilidades;
        }

        /// <summary>
        /// Api para obtención de datos de tramites.
        /// </summary>
        /// <param name="instanciaTramiteDto">Contiene informacion de autorizacion, filtro e columnas visibles </param>
        /// <returns>Objeto con propiedades para realizar consulta de datos trámite.</returns>
        [Route("api/Catalogo/ObtenerCatalogo")]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerCatalogo(string catalogo)
        {
            try
            {                
                var result = await _catalogoServicio.ObtenerCatalogo(catalogo, Request.Headers.Authorization.Parameter, "api/Catalogo/").ConfigureAwait(false);
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }
    }
}