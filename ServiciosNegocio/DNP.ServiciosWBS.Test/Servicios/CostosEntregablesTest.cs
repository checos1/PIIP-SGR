namespace DNP.ServiciosWBS.Test.Servicios
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data.Entity;
    using System.Data.Entity.Core.Objects;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Net.Http;
    using Configuracion;
    using DNP.ServiciosNegocio.Dominio.Dto.CostosEntregables;
    using DNP.ServiciosWBS.Servicios.Interfaces;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Mocks;
    using Moq;
    using Persistencia.Implementaciones;
    using Persistencia.Implementaciones.Transversales;
    using Persistencia.Interfaces;
    using Persistencia.Interfaces.Transversales;
    using Persistencia.Modelo;
    using ServiciosNegocio.Comunes.Dto;
    using ServiciosNegocio.Comunes.Dto.Formulario;
    using ServiciosNegocio.Test.Mock;
    using ServiciosWBS.Servicios.Implementaciones;
    using ServiciosWBS.Servicios.Implementaciones.Transversales;
    using ServiciosWBS.Servicios.Interfaces.Transversales;
    using Unity;

    [TestClass]
    public class CostosEntregablesServicioTest
    {
        private string BpinBase { get; set; }
        private ICostosEntregablesServicios _costosEntregablesServicios { get; set; }
        private CostosEntregablesServicios CostosEntregablesServicio { get; set; }
        private readonly Mock<MGAWebContexto> _mockContext = new Mock<MGAWebContexto>();
        private readonly Mock<IContextoFactory> _mockContextFactory = new Mock<IContextoFactory>();
        private ICostosEntregablesPersistencia CostosEntregablesPersistencia { get; set; }
        private IPersistenciaTemporal PersistenciaTemporal { get; set; }
        private IAuditoriaServicios AuditoriaServicio { get; set; }
        private readonly Mock<DbSet<AlmacenamientoTemporal>> _mockSetAlmacenamientoTemporal = new Mock<DbSet<AlmacenamientoTemporal>>();

        [TestInitialize]
        public void Init()
        {
            BpinBase = "202000000000005";
            var contenedor = UnityConfig.Container;
            _costosEntregablesServicios = contenedor.Resolve<ICostosEntregablesServicios>();
            CostosEntregablesPersistencia = UnityContainerExtensions.Resolve<ICostosEntregablesPersistencia>(contenedor);
            PersistenciaTemporal = contenedor.Resolve<IPersistenciaTemporal>();
            AuditoriaServicio = contenedor.Resolve<IAuditoriaServicios>();
            CostosEntregablesServicio = new CostosEntregablesServicios(CostosEntregablesPersistencia, PersistenciaTemporal, AuditoriaServicio);

            var dataAlmacenamientoTemporal = new List<AlmacenamientoTemporal>()
                                           {
                                               new AlmacenamientoTemporal()
                                               {
                                                   AccionId = new Guid("E00485B4-34D9-495A-82CF-7F7F477FD16F"),
                                                   InstanciaId =    new Guid("76A7958B-D20A-48AD-87C9-917D3B388E21"),
                                                   Json = "{\"AcumulaIndicadorPrincipal\": true,\"AlternativaId\" : 2,\"Cantidad\" : 5,\"Complemento\" : \"Prubea\",\"CondicionDeseadaId\" : 4,\"CPCId\" : 5,\r              \"FuenteVerificacion\" : \"PRUEB fuenteVerificacion\",\"IndicadorPrincipal\" : \"PRUEBA INDICADOR PRINCIPAL\",\"Meta\" : 10,\"MetaTotalIndicadorPrincipal\" : 15,\r                \"ProductoIndicador\" :\r[{\r\"AcumulaIndicadorSecundario\" : true,\r\"IndicadorSecundarioId\":2,\r \"IndicadorSecundario\":\"PRUEBA INDICADOR SECUNDARIO\",\r          \"MetaTotalIndicadorSecundario\":10,\r\"UnidadMedidaIndicadorSecundario\":4,\r\"ProductoIndicadorDetalle\":[\r{\r\"DetalleIndicadorProductoId\":4,\r\"MetaVigente\":5,\r\"Vigencia\":4,\r\"RegionalizacionMetas\":[{\r\"AgrupacionId\":2,\r\"DepartamentoId\":1,\r\"FechaCreacion\":\"2018/02/03\",\r\"MunicipioId\":2,\r\"RegionalizacionMetaId\":3,\r\"RegionId\":1,\r\"TotalMetaVigente\":58,\r\"VigenciaRegionalizada\":2\r},{\r\"AgrupacionId\":3,\r\"DepartamentoId\":2,\r\"FechaCreacion\":\"2018/02/03\",\r\"MunicipioId\":1,\r\"RegionalizacionMetaId\":4,\r\"RegionId\":5,\r\"TotalMetaVigente\":3,\r\"VigenciaRegionalizada\":2\r}]\r }]\r\r},{\r\"AcumulaIndicadorSecundario\" : true,\r\"IndicadorSecundarioId\":2,\r\"IndicadorSecundario\":\"PRUEBA INDICADOR SECUNDARIO\",\r\"MetaTotalIndicadorSecundario\":10,\r\"UnidadMedidaIndicadorSecundario\":4,\r\"ProductoIndicadorDetalle\":[\r{\r\"DetalleIndicadorProductoId\":4,\r\"MetaVigente\":5,\r\"Vigencia\":4,\r\"RegionalizacionMetas\":[{\r\"AgrupacionId\":2,\r\"DepartamentoId\":1,\r\"FechaCreacion\":\"2018/02/03\",\r\"MunicipioId\":2,\r\"RegionalizacionMetaId\":3,\r\"RegionId\":1,\r\"TotalMetaVigente\":58,\r\"VigenciaRegionalizada\":2\r},{\r\"AgrupacionId\":3,\r\"DepartamentoId\":2,\r\"FechaCreacion\":\"2018/02/03\",\r\"MunicipioId\":1,\r\"RegionalizacionMetaId\":4,\r\"RegionId\":5,\r\"TotalMetaVigente\":3,\r\"VigenciaRegionalizada\":2\r}]\r}]\r} ]\r}"
                                               }
                                           }.AsQueryable();

            string objetoRetornoCostos = "";
            var mockCostos = new Mock<ObjectResult<string>>();
            mockCostos.SetupReturn(objetoRetornoCostos);

            _mockSetAlmacenamientoTemporal.As<IQueryable<AlmacenamientoTemporal>>().Setup(m => m.Provider).Returns(dataAlmacenamientoTemporal.Provider);
            _mockSetAlmacenamientoTemporal.As<IQueryable<AlmacenamientoTemporal>>().Setup(m => m.Expression).Returns(dataAlmacenamientoTemporal.Expression);
            _mockSetAlmacenamientoTemporal.As<IQueryable<AlmacenamientoTemporal>>().Setup(m => m.ElementType).Returns(dataAlmacenamientoTemporal.ElementType);
            _mockSetAlmacenamientoTemporal.As<IQueryable<AlmacenamientoTemporal>>().Setup(m => m.GetEnumerator()).Returns(dataAlmacenamientoTemporal.GetEnumerator());

            _mockContext.Setup(m => m.AlmacenamientoTemporal).Returns(_mockSetAlmacenamientoTemporal.Object);
            _mockContext.Setup(mc => mc.UspGetRegionalizaicion_JSON(BpinBase)).Returns(mockCostos.Object);

            _mockContextFactory.Setup(mcf => mcf.CrearContextoConConexion(ConfigurationManager.ConnectionStrings["MGAWebContexto"].ConnectionString)).Returns(_mockContext.Object);
        }

        [TestMethod]
        public void CostosEntregables()
        {
            _costosEntregablesServicios = new CostosEntregablesServicios(new CostosEntregablesPersistencia(_mockContextFactory.Object), new PersistenciaTemporal(_mockContextFactory.Object), new AuditoriaServicios());
            var result = _costosEntregablesServicios.ObtenerCostosEntregables(new ParametrosConsultaDto
            {
                Bpin = BpinBase,
                AccionId = new Guid("E00485B4-34D9-495A-82CF-7F7F477FD16F"),
                InstanciaId = new Guid("76A7958B-D20A-48AD-87C9-917D3B388E21")
            });
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void CostosEntregablesPreviewTest()
        {

            var result = _costosEntregablesServicios.ObtenerCostosEntregablesPreview();
            Assert.IsNotNull(result);
            Assert.IsTrue(result.vigencias.Count >= 1);
        }

        [TestMethod]
        public void CostosEntregables_ObtenerDefinitivo_Test()
        {
            CostosEntregablesServicio = new CostosEntregablesServicios(new CostosEntregablesPersistencia(_mockContextFactory.Object), new PersistenciaTemporal(_mockContextFactory.Object), new AuditoriaServicios());
            var parametrosConsulta = new ParametrosConsultaDto()
            {
                AccionId = new Guid("E00485B4-34D9-495A-82CF-7F7F477FD16F"),
                InstanciaId = new Guid("76A7958B-D20A-48AD-87C9-917D3B388E21"),
                Bpin = "202000000000005"
            };

            var resultado = CostosEntregablesServicio.Obtener(parametrosConsulta);
            Assert.IsNotNull(resultado);
        }

        [TestMethod]
        public void CostosEntregables_Obtener_Test()
        {
            CostosEntregablesServicio = new CostosEntregablesServicios(new CostosEntregablesPersistencia(_mockContextFactory.Object), new PersistenciaTemporal(_mockContextFactory.Object), new AuditoriaServicios());
            var parametrosConsulta = new ParametrosConsultaDto()
            {
                AccionId = new Guid("E00485B4-34D9-495A-82CF-7F7F477FD16F"),
                InstanciaId = new Guid("76A7958B-D20A-48AD-87C9-917D3B388E21"),
                Bpin = "202000000000005"
            };

            var resultado = CostosEntregablesServicio.ObtenerCostosEntregables(parametrosConsulta);
            Assert.IsNotNull(resultado);
        }

        [TestMethod]
        public void CostosEntregables_GuardarDefinitivo()
        {
            var parametrosGuardar = new ParametrosGuardarDto<CostosEntregablesDto>()
            {
                AccionId = new Guid("E00485B4-34D9-495A-82CF-7F7F477FD16F"),
                InstanciaId = new Guid("76A7958B-D20A-48AD-87C9-917D3B388E21"),
                Usuario = "Leticia01",
                Contenido = new CostosEntregablesDto()
                {
                    ProyectoId = 72210,
                    BPIN = "202000000000005",

                    vigencias = new List<Vigencia> {
                        new Vigencia()
                        {
                          vigencia = 2020
                        }
                    }
                }
            };

        
    

        var parametroAuditoria = new ParametrosAuditoriaDto()
        {
            Usuario = "Leticia01",
            Ip = "localhost"
        };

        CostosEntregablesServicio.Guardar(parametrosGuardar, parametroAuditoria, false);
        }
}
}
