namespace DNP.ServiciosWBS.Test.Servicios
{
    using Configuracion;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using ServiciosNegocio.Comunes.Dto;
    using ServiciosNegocio.Comunes.Dto.Formulario;
    using ServiciosNegocio.Comunes.Excepciones;
    using ServiciosNegocio.Dominio.Dto.Proyectos;
    using ServiciosWBS.Servicios.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    using System.Net.Http;
    using Mocks;
    using Moq;
    using Persistencia.Implementaciones;
    using Persistencia.Implementaciones.Transversales;
    using Persistencia.Interfaces;
    using Persistencia.Modelo;
    using ServiciosNegocio.Test.Mock;
    using ServiciosWBS.Servicios.Implementaciones;
    using ServiciosWBS.Servicios.Implementaciones.Transversales;
    using Unity;

    [TestClass]
    public class ProductosServicioTest
    {
        private IProductosServicio ProductosServicio { get; set; }
        private readonly Mock<MGAWebContexto> _mockContext = new Mock<MGAWebContexto>();
        private readonly Mock<IContextoFactory> _mockContextFactory = new Mock<IContextoFactory>();
        private readonly Mock<MockableDbSetWithExtensions<AlmacenamientoTemporal>> _mockSetAlmacenamientoTemporal = new Mock<MockableDbSetWithExtensions<AlmacenamientoTemporal>>();

        [TestInitialize]
        public void Init()
        {
            var contenedor = UnityConfig.Container;
            ProductosServicio = contenedor.Resolve<IProductosServicio>();

            var objetoRetornoProductosPorProyectp = new uspGetProductosPorProyecto_Result()
            {
                ProyectoBPIN = "2017005950118",
                ProyectoId = 1

            };
            var mockProductosPorProyecto = new Mock<ObjectResult<uspGetProductosPorProyecto_Result>>();
            mockProductosPorProyecto.SetupReturn(objetoRetornoProductosPorProyectp);
            var dataAlmacenamientoTemporal = new List<AlmacenamientoTemporal>()
                                           {
                                               new AlmacenamientoTemporal()
                                               {
                                                   AccionId = new Guid("E3C1849C-FE24-4C07-9762-036FA72AF10C"),
                                                   InstanciaId =    new Guid("4C2E62CD-CEAD-48EF-88C6-A50AB5913716"),
                                                   Json = "{\"AcumulaIndicadorPrincipal\": true,\"AlternativaId\" : 2,\"Cantidad\" : 5,\"Complemento\" : \"Prubea\",\"CondicionDeseadaId\" : 4,\"CPCId\" : 5,\r              \"FuenteVerificacion\" : \"PRUEB fuenteVerificacion\",\"IndicadorPrincipal\" : \"PRUEBA INDICADOR PRINCIPAL\",\"Meta\" : 10,\"MetaTotalIndicadorPrincipal\" : 15,\r                \"ProductoIndicador\" :\r[{\r\"AcumulaIndicadorSecundario\" : true,\r\"IndicadorSecundarioId\":2,\r \"IndicadorSecundario\":\"PRUEBA INDICADOR SECUNDARIO\",\r          \"MetaTotalIndicadorSecundario\":10,\r\"UnidadMedidaIndicadorSecundario\":4,\r\"ProductoIndicadorDetalle\":[\r{\r\"DetalleIndicadorProductoId\":4,\r\"MetaVigente\":5,\r\"Vigencia\":4,\r\"RegionalizacionMetas\":[{\r\"AgrupacionId\":2,\r\"DepartamentoId\":1,\r\"FechaCreacion\":\"2018/02/03\",\r\"MunicipioId\":2,\r\"RegionalizacionMetaId\":3,\r\"RegionId\":1,\r\"TotalMetaVigente\":58,\r\"VigenciaRegionalizada\":2\r},{\r\"AgrupacionId\":3,\r\"DepartamentoId\":2,\r\"FechaCreacion\":\"2018/02/03\",\r\"MunicipioId\":1,\r\"RegionalizacionMetaId\":4,\r\"RegionId\":5,\r\"TotalMetaVigente\":3,\r\"VigenciaRegionalizada\":2\r}]\r }]\r\r},{\r\"AcumulaIndicadorSecundario\" : true,\r\"IndicadorSecundarioId\":2,\r\"IndicadorSecundario\":\"PRUEBA INDICADOR SECUNDARIO\",\r\"MetaTotalIndicadorSecundario\":10,\r\"UnidadMedidaIndicadorSecundario\":4,\r\"ProductoIndicadorDetalle\":[\r{\r\"DetalleIndicadorProductoId\":4,\r\"MetaVigente\":5,\r\"Vigencia\":4,\r\"RegionalizacionMetas\":[{\r\"AgrupacionId\":2,\r\"DepartamentoId\":1,\r\"FechaCreacion\":\"2018/02/03\",\r\"MunicipioId\":2,\r\"RegionalizacionMetaId\":3,\r\"RegionId\":1,\r\"TotalMetaVigente\":58,\r\"VigenciaRegionalizada\":2\r},{\r\"AgrupacionId\":3,\r\"DepartamentoId\":2,\r\"FechaCreacion\":\"2018/02/03\",\r\"MunicipioId\":1,\r\"RegionalizacionMetaId\":4,\r\"RegionId\":5,\r\"TotalMetaVigente\":3,\r\"VigenciaRegionalizada\":2\r}]\r}]\r} ]\r}"
                                               }
                                           }.AsQueryable();

            _mockSetAlmacenamientoTemporal.As<IQueryable<AlmacenamientoTemporal>>().Setup(m => m.Provider).Returns(dataAlmacenamientoTemporal.Provider);
            _mockSetAlmacenamientoTemporal.As<IQueryable<AlmacenamientoTemporal>>().Setup(m => m.Expression).Returns(dataAlmacenamientoTemporal.Expression);
            _mockSetAlmacenamientoTemporal.As<IQueryable<AlmacenamientoTemporal>>().Setup(m => m.ElementType).Returns(dataAlmacenamientoTemporal.ElementType);
            _mockSetAlmacenamientoTemporal.As<IQueryable<AlmacenamientoTemporal>>().Setup(m => m.GetEnumerator()).Returns(dataAlmacenamientoTemporal.GetEnumerator());

            _mockContext.Setup(mc => mc.uspGetProductosPorProyecto("2017115950118")).Returns(mockProductosPorProyecto.Object);

            _mockContext.Setup(m => m.AlmacenamientoTemporal).Returns(_mockSetAlmacenamientoTemporal.Object);
            _mockContextFactory.Setup(mcf => mcf.CrearContextoConConexion(ConfigurationManager.ConnectionStrings["MGAWebContexto"].ConnectionString)).Returns(_mockContext.Object);
        }


        [TestMethod]
        public void ObtenerProductoTemporal___RetornaDto()
        {
            ProductosServicio = new ProductosServicio(new ProductoPersistencia(_mockContextFactory.Object), new PersistenciaTemporal(_mockContextFactory.Object), new AuditoriaServicios());
            var parametrosConsultaProducto = new ParametrosConsultaDto() { Bpin = "2017005950118", InstanciaId = Guid.Parse("4C2E62CD-CEAD-48EF-88C6-A50AB5913716"), AccionId = Guid.Parse("E3C1849C-FE24-4C07-9762-036FA72AF10C") };
            var result = ProductosServicio.Obtener(parametrosConsultaProducto);
            Assert.IsNotNull(result);
        }

        /*
        [TestMethod]
        public void ObtenerProductoDefinitivo___RetornaDto()
        {
            ProductosServicio = new ProductosServicio(new ProductoPersistencia(_mockContextFactory.Object), new PersistenciaTemporal(_mockContextFactory.Object), new AuditoriaServicios());
            var parametrosConsultaProducto = new ParametrosConsultaDto() { Bpin = "2017115950118", InstanciaId = Guid.Parse("E3C1849C-CEAD-48EF-88C6-A50AB5913716"), AccionId = Guid.Parse("E3C1849C-FE24-4C07-9762-036FA72AF10C") };
            var result = ProductosServicio.Obtener(parametrosConsultaProducto);
            Assert.IsNotNull(result);
        }*/

        [TestMethod]
        public void ObtenerProductoPreview___RetornaDto()
        {

            var result = ProductosServicio.ObtenerProductosPreview();
            Assert.IsNotNull(result);
        }


        [TestMethod]
        public void ObtenerProductoRegionalizacionIndicadores___RetornaDto()
        {
            ProductosServicio = new ProductosServicio(new ProductoPersistencia(_mockContextFactory.Object), new PersistenciaTemporal(_mockContextFactory.Object), new AuditoriaServicios());
            var parametrosConsultaProducto = new ParametrosConsultaDto() { Bpin = "2017011000092", InstanciaId = Guid.Parse("4C2E62CD-CEAD-48EF-88C6-A50AB5913716"), AccionId = Guid.Parse("E3C1849C-FE24-4C07-9762-036FA72AF10C") };
            var result = ProductosServicio.ObtenerProductos(parametrosConsultaProducto);
            Assert.IsNotNull(result);
        }


        [TestMethod]
        [ExpectedException(typeof(ServiciosNegocioException), "Parámetro piip-idInstanciaFlujoflujo no recibido.")]
        public void ProductosServicio_ConstruirParametrosGuardado_IdInstanciaFlujoNoEnviado_Excepcion()
        {
            //Escenario: InstanciaId no enviado
            var contenido = new ProyectoDto();

            HttpRequestMessage request = new HttpRequestMessage();
            request.Headers.Add("piip-idAccion", Guid.NewGuid().ToString());

            //Ejecucion
            ProductosServicio.ConstruirParametrosGuardado(request, contenido);
        }

        [TestMethod]
        [ExpectedException(typeof(ServiciosNegocioException), "Parámetro piip-idAccion no recibido.")]
        public void ProductosServicio_Guardar_IdAccionNoEnviado_Excepcion()
        {
            //Escenario: AccionId no enviado
            var contenido = new ProyectoDto();

            HttpRequestMessage request = new HttpRequestMessage();
            request.Headers.Add("piip-idInstanciaFlujoflujo", Guid.NewGuid().ToString());

            //Ejecucion
            ProductosServicio.ConstruirParametrosGuardado(request, contenido);
        }

        [TestMethod]
        [ExpectedException(typeof(ServiciosNegocioException), "Parámetro contenido no recibido.")]
        public void ProductosServicio_Guardar_ProductoDtoNoEnviado_Excepcion()
        {
            //Escenario: Contenido no enviado
            HttpRequestMessage request = new HttpRequestMessage();
            request.Headers.Add("piip-idAccion", Guid.NewGuid().ToString());
            request.Headers.Add("piip-idInstanciaFlujoflujo", Guid.NewGuid().ToString());

            //Ejecucion
            ProductosServicio.ConstruirParametrosGuardado(request, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ServiciosNegocioException), "Parámetro piip-idInstanciaFlujoflujo inválido")]
        public void ProductosServicio_Guardar_IdInstanciaFlujoConValorInvalido_Excepcion()
        {
            //Escenario: InstanciaId inválido
            var contenido = new ProyectoDto();

            HttpRequestMessage request = new HttpRequestMessage();
            request.Headers.Add("piip-idInstanciaFlujoflujo", Guid.Empty.ToString());

            //Ejecucion
            ProductosServicio.ConstruirParametrosGuardado(request, contenido);
        }

        [TestMethod]
        [ExpectedException(typeof(ServiciosNegocioException), "Parámetro piip-idAccion inválido")]
        public void ProductosServicio_Guardar_IdAccionConValorInvalido_Excepcion()
        {
            //Escenario: AccionId inválido
            var contenido = new ProyectoDto();

            HttpRequestMessage request = new HttpRequestMessage();
            request.Headers.Add("piip-idAccion", Guid.Empty.ToString());

            //Ejecucion
            ProductosServicio.ConstruirParametrosGuardado(request, contenido);
        }

        [TestMethod]
        public void ProductosServicio_Guardar_MarcadoComoTemporal_InsertaRegistro()
        {
            ProductosServicio = new ProductosServicio(new ProductoPersistencia(_mockContextFactory.Object), new PersistenciaTemporal(_mockContextFactory.Object), new AuditoriaServicios());
            //Escenario: los parametros de insercion son validos y ademas viene marcado como guardado temporal
            var parametrosGuardarProducto = new ParametrosGuardarDto<ProyectoDto>
            {
                InstanciaId = Guid.NewGuid(),
                AccionId = Guid.NewGuid(),
                Contenido = new ProyectoDto()
            };

            var parametrosAuditoria = new ParametrosAuditoriaDto();

            //Ejecucion
            ProductosServicio.Guardar(parametrosGuardarProducto, parametrosAuditoria, true);
        }
    }
}