namespace DNP.ServiciosNegocio.Web.API.Test.Catalogos
{
    using System;
    using System.Net;
    using Controllers.Catalogos;
    using Comunes.Autorizacion;
    using Servicios.Interfaces.Catalogos;
    using System.Net.Http;
    using Unity;
    using System.Threading.Tasks;
    using System.Net.Http.Headers;
    using System.Security.Principal;
    using System.Web.Http.Results;
    using Comunes.Enum;
    using Comunes;
    using Dominio.Dto.Catalogos;
    using System.Collections.Generic;
    using Comunes.Excepciones;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class CatalogoControllerTest : IDisposable
    {

        private ICatalogoServicio CatalogoServicio { get; set; }
        private IAutorizacionUtilidades AutorizacionUtilizades { get; set; }

        private CatalogoController _catalogoController;
        private string NombreCatalogo { get; set; }
        private int IdCatalogo { get; set; }

        [TestInitialize]
        public void Init()
        {
            var contenedor = Configuracion.UnityConfig.Container;
            CatalogoServicio = contenedor.Resolve<ICatalogoServicio>();
            AutorizacionUtilizades = contenedor.Resolve<IAutorizacionUtilidades>();
            _catalogoController = new CatalogoController(CatalogoServicio, AutorizacionUtilizades);
            _catalogoController.ControllerContext.Request = new HttpRequestMessage();
            _catalogoController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "amRlbGdhZG86MTYyNTk0NjM=");
            _catalogoController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:Backbone");
            _catalogoController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });
        }

        [TestMethod]
        [ExpectedException(typeof(ServiciosNegocioHttpResponseException))]
        public async Task ConsultarCatalogoParametroVacio_RetornaExcepcionBadRequest()
        {
            NombreCatalogo = string.Empty;
            await _catalogoController.Consultar(NombreCatalogo);
        }

        [TestMethod]
        [ExpectedException(typeof(ServiciosNegocioHttpResponseException))]
        public async Task ConsultarCatalogoParametroInvalido_RetornaExcepcionBadRequest()
        {
            NombreCatalogo = " ";
            await _catalogoController.Consultar(NombreCatalogo);
        }

        //[TestMethod]
        //public async Task ConsultarCatalogoInexistente_RetornaExcepcionNoFound()
        //{
        //    NombreCatalogo = "xxxx";
        //    var respuesta = (ResponseMessageResult) await _catalogoController.Consultar(NombreCatalogo);
        //    Assert.AreEqual(HttpStatusCode.NotFound, respuesta.Response.StatusCode);
        //    Assert.AreEqual(ServiciosNegocioRecursos.SinResultados, respuesta.Response.ReasonPhrase);
        //}

        [TestMethod]
        public async Task ConsultarCatalogoEntiades_RetornaOk()
        {
            NombreCatalogo = CatalogoEnum.Entidades.ToString();
            var respuesta = (OkNegotiatedContentResult<IEnumerable<CatalogoDto>>)await _catalogoController.Consultar(NombreCatalogo);
            Assert.IsNotNull(respuesta.Content);
        }

        [TestMethod]
        public async Task ConsultarCatalogoAlternativas_RetornaOk()
        {
            NombreCatalogo = CatalogoEnum.Alternativas.ToString();
            var respuesta = (OkNegotiatedContentResult<IEnumerable<CatalogoDto>>)await _catalogoController.Consultar(NombreCatalogo);
            Assert.IsNotNull(respuesta.Content);
        }

        [TestMethod]
        public async Task ConsultarCatalogoTiposEntidades_RetornaOk()
        {
            NombreCatalogo = CatalogoEnum.TiposEntidades.ToString();
            var respuesta = (OkNegotiatedContentResult<IEnumerable<CatalogoDto>>)await _catalogoController.Consultar(NombreCatalogo);
            Assert.IsNotNull(respuesta.Content);
        }

        [TestMethod]
        public async Task ConsultarCatalogoSectores_RetornaOk()
        {
            NombreCatalogo = CatalogoEnum.Sectores.ToString();
            var respuesta = (OkNegotiatedContentResult<IEnumerable<CatalogoDto>>)await _catalogoController.Consultar(NombreCatalogo);
            Assert.IsNotNull(respuesta.Content);
        }

        [TestMethod]
        public async Task ConsultarCatalogoRegiones_RetornaOk()
        {
            NombreCatalogo = CatalogoEnum.Regiones.ToString();
            var respuesta = (OkNegotiatedContentResult<IEnumerable<CatalogoDto>>)await _catalogoController.Consultar(NombreCatalogo);
            Assert.IsNotNull(respuesta.Content);
        }

        [TestMethod]
        public async Task ConsultarCatalogoDepartamentos_RetornaOk()
        {
            NombreCatalogo = CatalogoEnum.Departamentos.ToString();
            var respuesta = (OkNegotiatedContentResult<IEnumerable<CatalogoDto>>)await _catalogoController.Consultar(NombreCatalogo);
            Assert.IsNotNull(respuesta.Content);
        }

        [TestMethod]
        public async Task ConsultarCatalogoMunicipios_RetornaOk()
        {
            NombreCatalogo = CatalogoEnum.Municipios.ToString();
            var respuesta = (OkNegotiatedContentResult<IEnumerable<CatalogoDto>>)await _catalogoController.Consultar(NombreCatalogo);
            Assert.IsNotNull(respuesta.Content);
        }

        [TestMethod]
        public async Task ConsultarCatalogoResguardos_RetornaOk()
        {
            NombreCatalogo = CatalogoEnum.Resguardos.ToString();
            var respuesta = (OkNegotiatedContentResult<IEnumerable<CatalogoDto>>)await _catalogoController.Consultar(NombreCatalogo);
            Assert.IsNotNull(respuesta.Content);
        }

        [TestMethod]
        public async Task ConsultarCatalogoProgramas_RetornaOk()
        {
            NombreCatalogo = CatalogoEnum.Programas.ToString();
            var respuesta = (OkNegotiatedContentResult<IEnumerable<CatalogoDto>>)await _catalogoController.Consultar(NombreCatalogo);
            Assert.IsNotNull(respuesta.Content);
        }

        [TestMethod]
        public async Task ConsultarCatalogoProductos_RetornaOk()
        {
            NombreCatalogo = CatalogoEnum.Productos.ToString();
            var respuesta = (OkNegotiatedContentResult<IEnumerable<CatalogoDto>>)await _catalogoController.Consultar(NombreCatalogo);
            Assert.IsNotNull(respuesta.Content);
        }


        [TestMethod]
        public async Task ConsultarCatalogoTiposRecursos_RetornaOk()
        {
            NombreCatalogo = CatalogoEnum.TiposRecursos.ToString();
            var respuesta = (OkNegotiatedContentResult<IEnumerable<CatalogoDto>>)await _catalogoController.Consultar(NombreCatalogo);
            Assert.IsNotNull(respuesta.Content);
        }

        [TestMethod]
        public async Task ConsultarCatalogoClasificacionesRecursos_RetornaOk()
        {
            NombreCatalogo = CatalogoEnum.ClasificacionesRecursos.ToString();
            var respuesta = (OkNegotiatedContentResult<IEnumerable<CatalogoDto>>)await _catalogoController.Consultar(NombreCatalogo);
            Assert.IsNotNull(respuesta.Content);
        }

        [TestMethod]
        public async Task ConsultarCatalogoEtapasRecursos_RetornaOk()
        {
            NombreCatalogo = CatalogoEnum.Etapas.ToString();
            var respuesta = (OkNegotiatedContentResult<IEnumerable<CatalogoDto>>)await _catalogoController.Consultar(NombreCatalogo);
            Assert.IsNotNull(respuesta.Content);
        }

        [TestMethod]
        public async Task ConsultarCatalogoTiposAgrupaciones_RetornaOk()
        {
            NombreCatalogo = CatalogoEnum.TiposAgrupaciones.ToString();
            var respuesta = (OkNegotiatedContentResult<IEnumerable<CatalogoDto>>)await _catalogoController.Consultar(NombreCatalogo);
            Assert.IsNotNull(respuesta.Content);
        }

        [TestMethod]
        public async Task ConsultarCatalogoPoliticas_RetornaOk()
        {
            NombreCatalogo = CatalogoEnum.Politicas.ToString();
            var respuesta = (OkNegotiatedContentResult<IEnumerable<CatalogoDto>>)await _catalogoController.Consultar(NombreCatalogo);
            Assert.IsNotNull(respuesta.Content);
        }

        [TestMethod]
        public async Task ConsultarCatalogoEntregables_RetornaOk()
        {
            NombreCatalogo = CatalogoEnum.Entregables.ToString();
            var respuesta = (OkNegotiatedContentResult<IEnumerable<CatalogoDto>>)await _catalogoController.Consultar(NombreCatalogo);
            Assert.IsNotNull(respuesta.Content);
        }

        [TestMethod]
        public async Task ConsultarCatalogoIndicadorPoliticas_RetornaOk()
        {
            NombreCatalogo = CatalogoEnum.IndicadoresPoliticas.ToString();
            var respuesta = (OkNegotiatedContentResult<IEnumerable<CatalogoDto>>)await _catalogoController.Consultar(NombreCatalogo);
            Assert.IsNotNull(respuesta.Content);
        }

        [TestMethod]
        public async Task ConsultarCatalogoAgrupaciones_RetornaOk()
        {
            NombreCatalogo = CatalogoEnum.Agrupaciones.ToString();
            var respuesta = (OkNegotiatedContentResult<IEnumerable<CatalogoDto>>)await _catalogoController.Consultar(NombreCatalogo);
            Assert.IsNotNull(respuesta.Content);
        }

        [TestMethod]
        public async Task ConsultarCatalogoGrupoRecursos_RetornaOk()
        {
            NombreCatalogo = CatalogoEnum.GruposRecursos.ToString();
            var respuesta = (OkNegotiatedContentResult<IEnumerable<CatalogoDto>>)await _catalogoController.Consultar(NombreCatalogo);
            Assert.IsNotNull(respuesta.Content);
        }

        [TestMethod]
        [ExpectedException(typeof(ServiciosNegocioHttpResponseException))]
        public async Task ConsultarCatalogoPorIdParametroNombreVacio_RetornaExcepcionBadRequest()
        {
            NombreCatalogo = string.Empty;
            IdCatalogo = 1;
            await _catalogoController.ConsultarCatalogoPorId(NombreCatalogo, IdCatalogo);
        }

        [TestMethod]
        [ExpectedException(typeof(ServiciosNegocioHttpResponseException))]
        public async Task ConsultarCatalogoPorIdParametroNombreInvalido_RetornaExcepcionBadRequest()
        {
            NombreCatalogo = " ";
            IdCatalogo = 1;
            await _catalogoController.ConsultarCatalogoPorId(NombreCatalogo, IdCatalogo);
        }

        [TestMethod]
        public async Task ConsultarCatalogoPorIdEntidad_RetornaExcepcionNoFound()
        {
            NombreCatalogo = CatalogoEnum.Entidades.ToString();
            IdCatalogo = 4;
            var respuesta = (ResponseMessageResult)await _catalogoController.ConsultarCatalogoPorId(NombreCatalogo, IdCatalogo);
            Assert.AreEqual(HttpStatusCode.NotFound, respuesta.Response.StatusCode);
            Assert.AreEqual(ServiciosNegocioRecursos.SinResultados, respuesta.Response.ReasonPhrase);
        }

        [TestMethod]
        public async Task ConsultarCatalogoPorIdTipoEntidadTask_RetornaExcepcionNoFound()
        {
            NombreCatalogo = CatalogoEnum.TiposEntidades.ToString();
            IdCatalogo = 4;
            var respuesta = (ResponseMessageResult)await _catalogoController.ConsultarCatalogoPorId(NombreCatalogo, IdCatalogo);
            Assert.AreEqual(HttpStatusCode.NotFound, respuesta.Response.StatusCode);
            Assert.AreEqual(ServiciosNegocioRecursos.SinResultados, respuesta.Response.ReasonPhrase);
        }

        [TestMethod]
        public async Task ConsultarCatalogoPorIdSector_RetornaExcepcionNoFound()
        {
            NombreCatalogo = CatalogoEnum.Sectores.ToString();
            IdCatalogo = 4;
            var respuesta = (ResponseMessageResult)await _catalogoController.ConsultarCatalogoPorId(NombreCatalogo, IdCatalogo);
            Assert.AreEqual(HttpStatusCode.NotFound, respuesta.Response.StatusCode);
            Assert.AreEqual(ServiciosNegocioRecursos.SinResultados, respuesta.Response.ReasonPhrase);
        }

        [TestMethod]
        public async Task ConsultarCatalogoPorIdRegion_RetornaExcepcionNoFound()
        {
            NombreCatalogo = CatalogoEnum.Regiones.ToString();
            IdCatalogo = 4;
            var respuesta = (ResponseMessageResult)await _catalogoController.ConsultarCatalogoPorId(NombreCatalogo, IdCatalogo);
            Assert.AreEqual(HttpStatusCode.NotFound, respuesta.Response.StatusCode);
            Assert.AreEqual(ServiciosNegocioRecursos.SinResultados, respuesta.Response.ReasonPhrase);
        }

        [TestMethod]
        public async Task ConsultarCatalogoPorIdDepartamento_RetornaExcepcionNoFound()
        {
            NombreCatalogo = CatalogoEnum.Departamentos.ToString();
            IdCatalogo = 4;
            var respuesta = (ResponseMessageResult)await _catalogoController.ConsultarCatalogoPorId(NombreCatalogo, IdCatalogo);
            Assert.AreEqual(HttpStatusCode.NotFound, respuesta.Response.StatusCode);
            Assert.AreEqual(ServiciosNegocioRecursos.SinResultados, respuesta.Response.ReasonPhrase);
        }

        [TestMethod]
        public async Task ConsultarCatalogoPorIdMunicipio_RetornaExcepcionNoFound()
        {
            NombreCatalogo = CatalogoEnum.Municipios.ToString();
            IdCatalogo = 4;
            var respuesta = (ResponseMessageResult)await _catalogoController.ConsultarCatalogoPorId(NombreCatalogo, IdCatalogo);
            Assert.AreEqual(HttpStatusCode.NotFound, respuesta.Response.StatusCode);
            Assert.AreEqual(ServiciosNegocioRecursos.SinResultados, respuesta.Response.ReasonPhrase);
        }

        [TestMethod]
        public async Task ConsultarCatalogoPorIdResguardo_RetornaExcepcionNoFound()
        {
            NombreCatalogo = CatalogoEnum.Resguardos.ToString();
            IdCatalogo = 4;
            var respuesta = (ResponseMessageResult)await _catalogoController.ConsultarCatalogoPorId(NombreCatalogo, IdCatalogo);
            Assert.AreEqual(HttpStatusCode.NotFound, respuesta.Response.StatusCode);
            Assert.AreEqual(ServiciosNegocioRecursos.SinResultados, respuesta.Response.ReasonPhrase);
        }

        [TestMethod]
        public async Task ConsultarCatalogoPorIdPrograma_RetornaExcepcionNoFound()
        {
            NombreCatalogo = CatalogoEnum.Programas.ToString();
            IdCatalogo = 4;
            var respuesta = (ResponseMessageResult)await _catalogoController.ConsultarCatalogoPorId(NombreCatalogo, IdCatalogo);
            Assert.AreEqual(HttpStatusCode.NotFound, respuesta.Response.StatusCode);
            Assert.AreEqual(ServiciosNegocioRecursos.SinResultados, respuesta.Response.ReasonPhrase);
        }

        [TestMethod]
        public async Task ConsultarCatalogoPorIdEtapa_RetornaExcepcionNoFound()
        {
            NombreCatalogo = CatalogoEnum.Etapas.ToString();
            IdCatalogo = 4;
            var respuesta = (ResponseMessageResult)await _catalogoController.ConsultarCatalogoPorId(NombreCatalogo, IdCatalogo);
            Assert.AreEqual(HttpStatusCode.NotFound, respuesta.Response.StatusCode);
            Assert.AreEqual(ServiciosNegocioRecursos.SinResultados, respuesta.Response.ReasonPhrase);
        }

        [TestMethod]
        public async Task ConsultarCatalogoPorIdTipoAgrupacion_RetornaExcepcionNoFound()
        {
            NombreCatalogo = CatalogoEnum.TiposAgrupaciones.ToString();
            IdCatalogo = 4;
            var respuesta = (ResponseMessageResult)await _catalogoController.ConsultarCatalogoPorId(NombreCatalogo, IdCatalogo);
            Assert.AreEqual(HttpStatusCode.NotFound, respuesta.Response.StatusCode);
            Assert.AreEqual(ServiciosNegocioRecursos.SinResultados, respuesta.Response.ReasonPhrase);
        }

        [TestMethod]
        public async Task ConsultarCatalogoPorIdPoltica_RetornaExcepcionNoFound()
        {
            NombreCatalogo = CatalogoEnum.Politicas.ToString();
            IdCatalogo = 4;
            var respuesta = (ResponseMessageResult)await _catalogoController.ConsultarCatalogoPorId(NombreCatalogo, IdCatalogo);
            Assert.AreEqual(HttpStatusCode.NotFound, respuesta.Response.StatusCode);
            Assert.AreEqual(ServiciosNegocioRecursos.SinResultados, respuesta.Response.ReasonPhrase);
        }

        [TestMethod]
        public async Task ConsultarCatalogoPorIdTiposCofinanciaciones_RetornaExcepcionNoFound()
        {
            NombreCatalogo = CatalogoEnum.TiposCofinanciaciones.ToString();
            IdCatalogo = 4;
            var respuesta = (ResponseMessageResult)await _catalogoController.ConsultarCatalogoPorId(NombreCatalogo, IdCatalogo);
            Assert.AreEqual(HttpStatusCode.NotFound, respuesta.Response.StatusCode);
            Assert.AreEqual(ServiciosNegocioRecursos.SinResultados, respuesta.Response.ReasonPhrase);
        }

        [TestMethod]
        public async Task ConsultarCatalogoPorIdEntregables_RetornaExcepcionNoFound()
        {
            NombreCatalogo = CatalogoEnum.Entregables.ToString();
            IdCatalogo = 4;
            var respuesta = (ResponseMessageResult)await _catalogoController.ConsultarCatalogoPorId(NombreCatalogo, IdCatalogo);
            Assert.AreEqual(HttpStatusCode.NotFound, respuesta.Response.StatusCode);
            Assert.AreEqual(ServiciosNegocioRecursos.SinResultados, respuesta.Response.ReasonPhrase);
        }

        [TestMethod]
        public async Task ConsultarCatalogoPorIdIndicadorPoltica_RetornaExcepcionNoFound()
        {
            NombreCatalogo = CatalogoEnum.IndicadoresPoliticas.ToString();
            IdCatalogo = 4;
            var respuesta = (ResponseMessageResult)await _catalogoController.ConsultarCatalogoPorId(NombreCatalogo, IdCatalogo);
            Assert.AreEqual(HttpStatusCode.NotFound, respuesta.Response.StatusCode);
            Assert.AreEqual(ServiciosNegocioRecursos.SinResultados, respuesta.Response.ReasonPhrase);
        }

        [TestMethod]
        public async Task ConsultarCatalogoPorIdAgrupacion_RetornaExcepcionNoFound()
        {
            NombreCatalogo = CatalogoEnum.Agrupaciones.ToString();
            IdCatalogo = 4;
            var respuesta = (ResponseMessageResult)await _catalogoController.ConsultarCatalogoPorId(NombreCatalogo, IdCatalogo);
            Assert.AreEqual(HttpStatusCode.NotFound, respuesta.Response.StatusCode);
            Assert.AreEqual(ServiciosNegocioRecursos.SinResultados, respuesta.Response.ReasonPhrase);
        }

        [TestMethod]
        public async Task ConsultarCatalogoPorIdGrupoRecurso_RetornaExcepcionNoFound()
        {
            NombreCatalogo = CatalogoEnum.GruposRecursos.ToString();
            IdCatalogo = 4;
            var respuesta = (ResponseMessageResult)await _catalogoController.ConsultarCatalogoPorId(NombreCatalogo, IdCatalogo);
            Assert.AreEqual(HttpStatusCode.NotFound, respuesta.Response.StatusCode);
            Assert.AreEqual(ServiciosNegocioRecursos.SinResultados, respuesta.Response.ReasonPhrase);
        }

        [TestMethod]
        public async Task ConsultarCatalogoPorIdProducto_RetornaExcepcionNoFound()
        {
            NombreCatalogo = CatalogoEnum.Productos.ToString();
            IdCatalogo = 4;
            var respuesta = (ResponseMessageResult)await _catalogoController.ConsultarCatalogoPorId(NombreCatalogo, IdCatalogo);
            Assert.AreEqual(HttpStatusCode.NotFound, respuesta.Response.StatusCode);
            Assert.AreEqual(ServiciosNegocioRecursos.SinResultados, respuesta.Response.ReasonPhrase);
        }

        [TestMethod]
        public async Task ConsultarCatalogoPorIdAlternativa_RetornaExcepcionNoFound()
        {
            NombreCatalogo = CatalogoEnum.Alternativas.ToString();
            IdCatalogo = 4;
            var respuesta = (ResponseMessageResult)await _catalogoController.ConsultarCatalogoPorId(NombreCatalogo, IdCatalogo);
            Assert.AreEqual(HttpStatusCode.NotFound, respuesta.Response.StatusCode);
            Assert.AreEqual(ServiciosNegocioRecursos.SinResultados, respuesta.Response.ReasonPhrase);
        }

        [TestMethod]
        public async Task ConsultarCatalogoPorIdTipoRecurso_RetornaExcepcionNoFound()
        {
            NombreCatalogo = CatalogoEnum.TiposRecursos.ToString();
            IdCatalogo = 4;
            var respuesta = (ResponseMessageResult)await _catalogoController.ConsultarCatalogoPorId(NombreCatalogo, IdCatalogo);
            Assert.AreEqual(HttpStatusCode.NotFound, respuesta.Response.StatusCode);
            Assert.AreEqual(ServiciosNegocioRecursos.SinResultados, respuesta.Response.ReasonPhrase);
        }

        [TestMethod]
        public async Task ConsultarCatalogoPorIdClaseRecurso_RetornaExcepcionNoFound()
        {
            NombreCatalogo = CatalogoEnum.ClasificacionesRecursos.ToString();
            IdCatalogo = 4;
            var respuesta = (ResponseMessageResult)await _catalogoController.ConsultarCatalogoPorId(NombreCatalogo, IdCatalogo);
            Assert.AreEqual(HttpStatusCode.NotFound, respuesta.Response.StatusCode);
            Assert.AreEqual(ServiciosNegocioRecursos.SinResultados, respuesta.Response.ReasonPhrase);
        }

        [TestMethod]
        public async Task ConsultarCatalogoPorId_RetornaEntidadOk()
        {
            NombreCatalogo = CatalogoEnum.Entidades.ToString();
            IdCatalogo = 1;
            var respuesta = (OkNegotiatedContentResult<CatalogoDto>)await _catalogoController.ConsultarCatalogoPorId(NombreCatalogo, IdCatalogo);
            Assert.IsNotNull(respuesta.Content);
        }

        [TestMethod]
        public async Task ConsultarCatalogoPorId_RetornaTipoEntidadOk()
        {
            NombreCatalogo = CatalogoEnum.TiposEntidades.ToString();
            IdCatalogo = 1;
            var respuesta = (OkNegotiatedContentResult<CatalogoDto>)await _catalogoController.ConsultarCatalogoPorId(NombreCatalogo, IdCatalogo);
            Assert.IsNotNull(respuesta.Content);
        }

        [TestMethod]
        public async Task ConsultarCatalogoPorId_RetornaSectorOk()
        {
            NombreCatalogo = CatalogoEnum.Sectores.ToString();
            IdCatalogo = 1;
            var respuesta = (OkNegotiatedContentResult<CatalogoDto>)await _catalogoController.ConsultarCatalogoPorId(NombreCatalogo, IdCatalogo);
            Assert.IsNotNull(respuesta.Content);
        }

        [TestMethod]
        public async Task ConsultarCatalogoPorId_RetornaRegionOk()
        {
            NombreCatalogo = CatalogoEnum.Regiones.ToString();
            IdCatalogo = 1;
            var respuesta = (OkNegotiatedContentResult<CatalogoDto>)await _catalogoController.ConsultarCatalogoPorId(NombreCatalogo, IdCatalogo);
            Assert.IsNotNull(respuesta.Content);
        }

        [TestMethod]
        public async Task ConsultarCatalogoPorId_RetornaDepartamentoOk()
        {
            NombreCatalogo = CatalogoEnum.Departamentos.ToString();
            IdCatalogo = 1;
            var respuesta = (OkNegotiatedContentResult<CatalogoDto>)await _catalogoController.ConsultarCatalogoPorId(NombreCatalogo, IdCatalogo);
            Assert.IsNotNull(respuesta.Content);
        }

        [TestMethod]
        public async Task ConsultarCatalogoPorId_RetornaMunicipioOk()
        {
            NombreCatalogo = CatalogoEnum.Municipios.ToString();
            IdCatalogo = 1;
            var respuesta = (OkNegotiatedContentResult<CatalogoDto>)await _catalogoController.ConsultarCatalogoPorId(NombreCatalogo, IdCatalogo);
            Assert.IsNotNull(respuesta.Content);
        }
        [TestMethod]
        public async Task ConsultarCatalogoPorId_RetornaResguardoOk()
        {
            NombreCatalogo = CatalogoEnum.Resguardos.ToString();
            IdCatalogo = 1;
            var respuesta = (OkNegotiatedContentResult<CatalogoDto>)await _catalogoController.ConsultarCatalogoPorId(NombreCatalogo, IdCatalogo);
            Assert.IsNotNull(respuesta.Content);
        }

        [TestMethod]
        public async Task ConsultarCatalogoPorId_RetornaProgramaOk()
        {
            NombreCatalogo = CatalogoEnum.Programas.ToString();
            IdCatalogo = 1;
            var respuesta = (OkNegotiatedContentResult<CatalogoDto>)await _catalogoController.ConsultarCatalogoPorId(NombreCatalogo, IdCatalogo);
            Assert.IsNotNull(respuesta.Content);
        }


        [TestMethod]
        public async Task ConsultarCatalogoPorId_RetornaProductoOk()
        {
            NombreCatalogo = CatalogoEnum.Productos.ToString();
            IdCatalogo = 1;
            var respuesta = (OkNegotiatedContentResult<CatalogoDto>)await _catalogoController.ConsultarCatalogoPorId(NombreCatalogo, IdCatalogo);
            Assert.IsNotNull(respuesta.Content);
        }

        [TestMethod]
        public async Task ConsultarCatalogoPorId_RetornaAlternativaOk()
        {
            NombreCatalogo = CatalogoEnum.Alternativas.ToString();
            IdCatalogo = 1;
            var respuesta = (OkNegotiatedContentResult<CatalogoDto>)await _catalogoController.ConsultarCatalogoPorId(NombreCatalogo, IdCatalogo);
            Assert.IsNotNull(respuesta.Content);
        }

        [TestMethod]
        public async Task ConsultarCatalogoPorId_RetornaTipoRecursosOk()
        {
            NombreCatalogo = CatalogoEnum.TiposRecursos.ToString();
            IdCatalogo = 1;
            var respuesta = (OkNegotiatedContentResult<CatalogoDto>)await _catalogoController.ConsultarCatalogoPorId(NombreCatalogo, IdCatalogo);
            Assert.IsNotNull(respuesta.Content);
        }

        [TestMethod]
        public async Task ConsultarCatalogoPorId_RetornaClaserecursoOk()
        {
            NombreCatalogo = CatalogoEnum.ClasificacionesRecursos.ToString();
            IdCatalogo = 1;
            var respuesta = (OkNegotiatedContentResult<CatalogoDto>)await _catalogoController.ConsultarCatalogoPorId(NombreCatalogo, IdCatalogo);
            Assert.IsNotNull(respuesta.Content);
        }

        [TestMethod]
        public async Task ConsultarCatalogoPorId_RetornaEtapaOk()
        {
            NombreCatalogo = CatalogoEnum.Etapas.ToString();
            IdCatalogo = 1;
            var respuesta = (OkNegotiatedContentResult<CatalogoDto>)await _catalogoController.ConsultarCatalogoPorId(NombreCatalogo, IdCatalogo);
            Assert.IsNotNull(respuesta.Content);
        }

        [TestMethod]
        public async Task ConsultarCatalogoPorId_RetornaTipoAgrupacionOk()
        {
            NombreCatalogo = CatalogoEnum.TiposAgrupaciones.ToString();
            IdCatalogo = 1;
            var respuesta = (OkNegotiatedContentResult<CatalogoDto>)await _catalogoController.ConsultarCatalogoPorId(NombreCatalogo, IdCatalogo);
            Assert.IsNotNull(respuesta.Content);
        }

        [TestMethod]
        public async Task ConsultarCatalogoPorId_RetornaPoliticaOk()
        {
            NombreCatalogo = CatalogoEnum.Politicas.ToString();
            IdCatalogo = 1;
            var respuesta = (OkNegotiatedContentResult<CatalogoDto>)await _catalogoController.ConsultarCatalogoPorId(NombreCatalogo, IdCatalogo);
            Assert.IsNotNull(respuesta.Content);
        }

        [TestMethod]
        public async Task ConsultarCatalogoPorId_RetornaTiposCofinanciacionesOk()
        {
            NombreCatalogo = CatalogoEnum.TiposCofinanciaciones.ToString();
            IdCatalogo = 1;
            var respuesta = (OkNegotiatedContentResult<CatalogoDto>)await _catalogoController.ConsultarCatalogoPorId(NombreCatalogo, IdCatalogo);
            Assert.IsNotNull(respuesta.Content);
        }

        [TestMethod]
        public async Task ConsultarCatalogoPorId_RetornaEntregablesOk()
        {
            NombreCatalogo = CatalogoEnum.Entregables.ToString();
            IdCatalogo = 1;
            var respuesta = (OkNegotiatedContentResult<CatalogoDto>)await _catalogoController.ConsultarCatalogoPorId(NombreCatalogo, IdCatalogo);
            Assert.IsNotNull(respuesta.Content);
        }

        [TestMethod]
        public async Task ConsultarCatalogoPorId_RetornaIndicadorPoliticaOk()
        {
            NombreCatalogo = CatalogoEnum.IndicadoresPoliticas.ToString();
            IdCatalogo = 1;
            var respuesta = (OkNegotiatedContentResult<CatalogoDto>)await _catalogoController.ConsultarCatalogoPorId(NombreCatalogo, IdCatalogo);
            Assert.IsNotNull(respuesta.Content);
        }

        [TestMethod]
        public async Task ConsultarCatalogoPorId_RetornaAgrupacionOk()
        {
            NombreCatalogo = CatalogoEnum.Agrupaciones.ToString();
            IdCatalogo = 1;
            var respuesta = (OkNegotiatedContentResult<CatalogoDto>)await _catalogoController.ConsultarCatalogoPorId(NombreCatalogo, IdCatalogo);
            Assert.IsNotNull(respuesta.Content);
        }

        [TestMethod]
        public async Task ConsultarCatalogoPorId_RetornaGrupoRecursoOk()
        {
            NombreCatalogo = CatalogoEnum.GruposRecursos.ToString();
            IdCatalogo = 1;
            var respuesta = (OkNegotiatedContentResult<CatalogoDto>)await _catalogoController.ConsultarCatalogoPorId(NombreCatalogo, IdCatalogo);
            Assert.IsNotNull(respuesta.Content);
        }

        [TestMethod]
        [ExpectedException(typeof(ServiciosNegocioHttpResponseException))]
        public async Task ConsultarCatalogoPorReferenciaParametroNombreVacio_RetornaExcepcionBadRequest()
        {
            var nombreCatalogo = string.Empty;
            var idCatalogo = 1;
            var nombreReferencia = CatalogoEnum.Departamentos.ToString();
            await _catalogoController.ConsultarPorReferencia(nombreCatalogo, idCatalogo, nombreReferencia);
        }

        [TestMethod]
        [ExpectedException(typeof(ServiciosNegocioHttpResponseException))]
        public async Task ConsultarCatalogoPorReferenciaParametroNombreInvalido_RetornaExcepcionBadRequest()
        {
            var nombreCatalogo = " ";
            var idCatalogo = 1;
            var nombreReferencia = CatalogoEnum.Departamentos.ToString();
            await _catalogoController.ConsultarPorReferencia(nombreCatalogo, idCatalogo, nombreReferencia);
        }

        [TestMethod]
        [ExpectedException(typeof(ServiciosNegocioHttpResponseException))]
        public async Task ConsultarCatalogoPorReferenciaParametroReferenciaVacio_RetornaExcepcionBadRequest()
        {
            var nombreCatalogo = CatalogoEnum.Regiones.ToString();
            var idCatalogo = 1;
            var nombreReferencia = string.Empty;
            await _catalogoController.ConsultarPorReferencia(nombreCatalogo, idCatalogo, nombreReferencia);
        }

        [TestMethod]
        [ExpectedException(typeof(ServiciosNegocioHttpResponseException))]
        public async Task ConsultarCatalogoPorReferenciaParametroReferenciaInvalido_RetornaExcepcionBadRequest()
        {
            var nombreCatalogo = CatalogoEnum.Regiones.ToString();
            var idCatalogo = 1;
            var nombreReferencia = " ";
            await _catalogoController.ConsultarPorReferencia(nombreCatalogo, idCatalogo, nombreReferencia);
        }

        [TestMethod]
        public async Task ConsultarCatalogoPorReferencia_RetornaNoFound()
        {
            var nombreCatalogo = CatalogoEnum.Departamentos.ToString();
            var idCatalogo = 3;
            var nombreReferencia = "xxxxxxx";
            var respuesta = (ResponseMessageResult)await _catalogoController.ConsultarPorReferencia(nombreCatalogo, idCatalogo, nombreReferencia);
            Assert.AreEqual(HttpStatusCode.NotFound, respuesta.Response.StatusCode);
        }

        [TestMethod]
        public async Task ConsultarCatalogoPorReferencia_EnviarRegionRetornaDepartamentosOk()
        {
            var nombreCatalogo = CatalogoEnum.Regiones.ToString();
            var idCatalogo = 1;
            var nombreReferencia = CatalogoEnum.Departamentos.ToString();
            var respuesta = (OkNegotiatedContentResult<CatalogoDto>)await _catalogoController.ConsultarPorReferencia(nombreCatalogo, idCatalogo, nombreReferencia);
            Assert.IsNotNull(respuesta.Content);
        }

        [TestMethod]
        public async Task ConsultarCatalogoPorReferencia_EnviarDepartamentoRetornaMunicipiosOk()
        {
            var nombreCatalogo = CatalogoEnum.Departamentos.ToString();
            var idCatalogo = 1;
            var nombreReferencia = CatalogoEnum.Municipios.ToString();
            var respuesta = (OkNegotiatedContentResult<CatalogoDto>)await _catalogoController.ConsultarPorReferencia(nombreCatalogo, idCatalogo, nombreReferencia);
            Assert.IsNotNull(respuesta.Content);
        }

        [TestMethod]
        public async Task ConsultarCatalogoPorReferencia_EnviarMunicipioRetornaResguardosOk()
        {
            var nombreCatalogo = CatalogoEnum.Municipios.ToString();
            var idCatalogo = 1;
            var nombreReferencia = CatalogoEnum.Resguardos.ToString();
            var respuesta = (OkNegotiatedContentResult<CatalogoDto>)await _catalogoController.ConsultarPorReferencia(nombreCatalogo, idCatalogo, nombreReferencia);
            Assert.IsNotNull(respuesta.Content);
        }

        [TestMethod]
        public async Task ObtenerTablasBasicas()
        {
            string jsonCondicion = "{'TramiteProyectoId':8450}";
            string Tabla = "DepartamentosPorProyecto";
            var respuesta = (OkNegotiatedContentResult<string>)await _catalogoController.ObtenerTablasBasicas(jsonCondicion, Tabla);
            Assert.IsNotNull(respuesta.Content);
        }

        #region Dispose

        public void Dispose()
        {
            Dispose(true);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                _catalogoController.Dispose();
            }
        }

        #endregion
    }
}
