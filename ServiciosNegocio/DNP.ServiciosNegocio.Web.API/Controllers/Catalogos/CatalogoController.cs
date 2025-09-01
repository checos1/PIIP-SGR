namespace DNP.ServiciosNegocio.Web.API.Controllers.Catalogos
{
    using Comunes;
    using Comunes.Autorizacion;
    using Comunes.Excepciones;
    using Comunes.Utilidades;
    using Dominio.Dto.Catalogos;
    using Servicios.Interfaces.Catalogos;
    using Swashbuckle.Swagger.Annotations;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web.Http;

    public class CatalogoController : ApiController
    {
        private readonly ICatalogoServicio _catalogoServicio;
        private readonly IAutorizacionUtilidades _autorizacionUtilidades;

        public CatalogoController(ICatalogoServicio catalogoServicio, IAutorizacionUtilidades autorizacionUtilidades)
        {
            _catalogoServicio = catalogoServicio;
            _autorizacionUtilidades = autorizacionUtilidades;
        }

        [Route("api/Catalogo/{nombreCatalogo}")]
        [HttpGet]
        public async Task<IHttpActionResult> Consultar(string nombreCatalogo)
        {

            var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                                               RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                                               ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                                               ConfigurationManager.AppSettings["consultarCatalogo"]).Result;

            if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

            ValidarParametros(nombreCatalogo);

            var tokenAutorizacion = Request.Headers.Authorization.Parameter;

            var result = await Task.Run(() => _catalogoServicio.ObtenerListaCatalogo(nombreCatalogo));

            return Responder(result);

        }

        [Route("api/Catalogo/Departamentos")]
        [HttpGet]
        public async Task<List<DepartamentoCatalogoDto>>  ConsultarDepartamentosRegion()
        {
            var result = await Task.Run(() => _catalogoServicio.ConsultarDepartamentosRegion());
            return result;
        }

        [Route("api/Catalogo/Agrupaciones")]
        [HttpGet]
        public async Task<List<AgrupacionCodeDto>> ConsultarAgrupacionesCompleta()
        {
            var result = await Task.Run(() => _catalogoServicio.ConsultarAgrupacionesCompleta());
            return result;
        }

        [Route("api/Catalogo/{nombreCatalogo}/{idCatalogo:int}")]
        [HttpGet]
        public async Task<IHttpActionResult> ConsultarCatalogoPorId(string nombreCatalogo, int idCatalogo)
        {

            var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                                               RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                                               ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                                               ConfigurationManager.AppSettings["consultarCatalogoPorId"]).Result;

            if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

            ValidarParametros(nombreCatalogo);

            var tokenAutorizacion = Request.Headers.Authorization.Parameter;

            var result = await Task.Run(() => _catalogoServicio.ObtenerCatalogoPorId(nombreCatalogo, idCatalogo, tokenAutorizacion));

            return Responder(result);

        }

        [Route("api/Catalogo/{nombreCatalogo}/{idCatalogo:int}/{nombreCatalogoRelacion}")]
        [HttpGet]
        public async Task<IHttpActionResult> ConsultarPorReferencia(string nombreCatalogo, int idCatalogo, string nombreCatalogoRelacion)
        {

            var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                                               RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                                               ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                                               ConfigurationManager.AppSettings["consultarRelacionCatalogo"]).Result;

            if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

            ValidarParametros(nombreCatalogo);
            ValidarParametros(nombreCatalogoRelacion);

            var tokenAutorizacion = Request.Headers.Authorization.Parameter;

            var result = await Task.Run(() => _catalogoServicio.ObtenerCatalogosPorReferencia(nombreCatalogo, idCatalogo, nombreCatalogoRelacion, tokenAutorizacion));

            return Responder(result);

        }

        [Route("api/Catalogo/TiposRecursosEntidad")]
        [HttpGet]
        public async Task<List<CatalogoDto>> ConsultaTiposRecursosEntidad(int entityTypeCatalogId)
        {
            var result = await Task.Run(() => _catalogoServicio.ConsultarTiposRecursosEntidad(entityTypeCatalogId));
            return result;
        }

        [Route("api/Catalogo/PoliticasCategoriaPorPadre")]
        [HttpGet]
        public async Task<List<CatalogoDto>> ConsultarCategoriaByPadre(int idPadre)
        {
            var result = await Task.Run(() => _catalogoServicio.ConsultarCategoriaByPadre(idPadre));
            return result;
        }

        [Route("api/Catalogo/EjecutorPorTipoEntidadId")]
        [HttpGet]
        public async Task<List<CatalogoDto>> ConsultarEjecutorPorTipoEntidadId(int idTipoEntidad)
        {
            var result = await Task.Run(() => _catalogoServicio.ConsultarEjecutorPorTipoEntidadId(idTipoEntidad));
            return result;
        }

        [Route("api/Catalogo/ObtenerTablasBasicas")]
        [SwaggerResponse(HttpStatusCode.OK, "Retorna TablasBasicas", typeof(string))]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerTablasBasicas(string jsonCondicion, string Tabla)
        {
            try
            {
                //var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                //                                                                    RequestContext.Principal.Identity.GetHashCode().ToString(),
                //                                                                    ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                //                                                                    ConfigurationManager.AppSettings["ObtenerDatostProgramacionProducto"]).Result;
                //if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                var result = await Task.Run(() => _catalogoServicio.ObtenerTablasBasicas(jsonCondicion, Tabla));
                if (result != null) return Ok(result);

                var respuestaHttp = new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.NotFound,
                    ReasonPhrase = ServiciosNegocioRecursos.SinResultados
                };

                return ResponseMessage(respuestaHttp);
            }
            catch (Exception e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/Catalogo/TiposRecursosEntidadPorGrupoRecursos")]
        [HttpGet]
        public async Task<List<CatalogoDto>> ConsultarTiposRecursosEntidadPorGrupoRecursos(int entityTypeCatalogId, int resourceGroupId, bool incluir)
        {
            var result = await Task.Run(() => _catalogoServicio.ConsultarTiposRecursosEntidadPorGrupoRecursos(entityTypeCatalogId, resourceGroupId, incluir));
            return result;
        }
        #region Métodos privados

        private static void ValidarParametros(string nombreCatalogo)
        {
            if (!ValidadorParametros.ValidarString(nombreCatalogo))
                throw new ServiciosNegocioHttpResponseException(HttpStatusCode.BadRequest, string.Format(ServiciosNegocioRecursos.ParametroNoValidos, "nombreCatalogo"));
        }

        private IHttpActionResult Responder(IEnumerable<CatalogoDto> result)
        {
            return result != null ? Ok(result) : CrearRespuestaNoFound();
        }

        private IHttpActionResult Responder(CatalogoDto result)
        {
            return result != null ? Ok(result) : CrearRespuestaNoFound();
        }

        private IHttpActionResult CrearRespuestaNoFound()
        {
            var respuestaHttp = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.NotFound,
                ReasonPhrase = ServiciosNegocioRecursos.SinResultados
            };

            return ResponseMessage(respuestaHttp);
        }

        #endregion
    }
}