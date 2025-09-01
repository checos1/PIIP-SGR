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
    using ServiciosNegocio.Comunes.Excepciones;
    using ServiciosNegocio.Dominio.Dto.FuenteFinanciacion;
    using ServiciosNegocio.Test.Mock;
    using ServiciosWBS.Servicios.Implementaciones;
    using ServiciosWBS.Servicios.Implementaciones.Transversales;
    using ServiciosWBS.Servicios.Interfaces.Transversales;
    using Unity;

    [TestClass]
    public class AjustesRegionalizaFuentesServicioTest
    {
        private string BpinBase { get; set; }
        private IAjustesRegionalizaFuentesServicios _regionalizaFuentesServicio { get; set; }
        private AjustesRegionalizaFuentesServicios AjustesRegionalizaFuentesServicio { get; set; }
        private readonly Mock<MGAWebContexto> _mockContext = new Mock<MGAWebContexto>();
        private readonly Mock<IContextoFactory> _mockContextFactory = new Mock<IContextoFactory>();
        private IAjustesRegionalizaFuentesPersistencia AjustesRegionalizaFuentesPersistencia { get; set; }
        private IPersistenciaTemporal PersistenciaTemporal { get; set; }
        private IAuditoriaServicios AuditoriaServicio { get; set; }
        private readonly Mock<DbSet<AlmacenamientoTemporal>> _mockSetAlmacenamientoTemporal = new Mock<DbSet<AlmacenamientoTemporal>>();

        [TestInitialize]
        public void Init()
        {
            BpinBase = "202000000000005";
            var contenedor = UnityConfig.Container;
            _regionalizaFuentesServicio = contenedor.Resolve<IAjustesRegionalizaFuentesServicios>();
            AjustesRegionalizaFuentesPersistencia = UnityContainerExtensions.Resolve<IAjustesRegionalizaFuentesPersistencia>(contenedor);
            PersistenciaTemporal = contenedor.Resolve<IPersistenciaTemporal>();
            AuditoriaServicio = contenedor.Resolve<IAuditoriaServicios>();
            AjustesRegionalizaFuentesServicio = new AjustesRegionalizaFuentesServicios(AjustesRegionalizaFuentesPersistencia, PersistenciaTemporal, AuditoriaServicio);

            var dataAlmacenamientoTemporal = new List<AlmacenamientoTemporal>()
                                           {
                                               new AlmacenamientoTemporal()
                                               {
                                                   AccionId = new Guid("7D7FCC8B-048F-451F-9EE3-CAE2A84D1241"),
                                                   InstanciaId =    new Guid("1174193F-3F6E-4BCA-8E0E-CCBD59DB113B"),
                                                   Json = "{\"AcumulaIndicadorPrincipal\": true,\"AlternativaId\" : 2,\"Cantidad\" : 5,\"Complemento\" : \"Prubea\",\"CondicionDeseadaId\" : 4,\"CPCId\" : 5,\r              \"FuenteVerificacion\" : \"PRUEB fuenteVerificacion\",\"IndicadorPrincipal\" : \"PRUEBA INDICADOR PRINCIPAL\",\"Meta\" : 10,\"MetaTotalIndicadorPrincipal\" : 15,\r                \"ProductoIndicador\" :\r[{\r\"AcumulaIndicadorSecundario\" : true,\r\"IndicadorSecundarioId\":2,\r \"IndicadorSecundario\":\"PRUEBA INDICADOR SECUNDARIO\",\r          \"MetaTotalIndicadorSecundario\":10,\r\"UnidadMedidaIndicadorSecundario\":4,\r\"ProductoIndicadorDetalle\":[\r{\r\"DetalleIndicadorProductoId\":4,\r\"MetaVigente\":5,\r\"Vigencia\":4,\r\"RegionalizacionMetas\":[{\r\"AgrupacionId\":2,\r\"DepartamentoId\":1,\r\"FechaCreacion\":\"2018/02/03\",\r\"MunicipioId\":2,\r\"RegionalizacionMetaId\":3,\r\"RegionId\":1,\r\"TotalMetaVigente\":58,\r\"VigenciaRegionalizada\":2\r},{\r\"AgrupacionId\":3,\r\"DepartamentoId\":2,\r\"FechaCreacion\":\"2018/02/03\",\r\"MunicipioId\":1,\r\"RegionalizacionMetaId\":4,\r\"RegionId\":5,\r\"TotalMetaVigente\":3,\r\"VigenciaRegionalizada\":2\r}]\r }]\r\r},{\r\"AcumulaIndicadorSecundario\" : true,\r\"IndicadorSecundarioId\":2,\r\"IndicadorSecundario\":\"PRUEBA INDICADOR SECUNDARIO\",\r\"MetaTotalIndicadorSecundario\":10,\r\"UnidadMedidaIndicadorSecundario\":4,\r\"ProductoIndicadorDetalle\":[\r{\r\"DetalleIndicadorProductoId\":4,\r\"MetaVigente\":5,\r\"Vigencia\":4,\r\"RegionalizacionMetas\":[{\r\"AgrupacionId\":2,\r\"DepartamentoId\":1,\r\"FechaCreacion\":\"2018/02/03\",\r\"MunicipioId\":2,\r\"RegionalizacionMetaId\":3,\r\"RegionId\":1,\r\"TotalMetaVigente\":58,\r\"VigenciaRegionalizada\":2\r},{\r\"AgrupacionId\":3,\r\"DepartamentoId\":2,\r\"FechaCreacion\":\"2018/02/03\",\r\"MunicipioId\":1,\r\"RegionalizacionMetaId\":4,\r\"RegionId\":5,\r\"TotalMetaVigente\":3,\r\"VigenciaRegionalizada\":2\r}]\r}]\r} ]\r}"
                                               }
                                           }.AsQueryable();

            string objetoRetornoFuentes = "";
            var mockFuentes = new Mock<ObjectResult<string>>();
            mockFuentes.SetupReturn(objetoRetornoFuentes);

            _mockSetAlmacenamientoTemporal.As<IQueryable<AlmacenamientoTemporal>>().Setup(m => m.Provider).Returns(dataAlmacenamientoTemporal.Provider);
            _mockSetAlmacenamientoTemporal.As<IQueryable<AlmacenamientoTemporal>>().Setup(m => m.Expression).Returns(dataAlmacenamientoTemporal.Expression);
            _mockSetAlmacenamientoTemporal.As<IQueryable<AlmacenamientoTemporal>>().Setup(m => m.ElementType).Returns(dataAlmacenamientoTemporal.ElementType);
            _mockSetAlmacenamientoTemporal.As<IQueryable<AlmacenamientoTemporal>>().Setup(m => m.GetEnumerator()).Returns(dataAlmacenamientoTemporal.GetEnumerator());

            _mockContext.Setup(m => m.AlmacenamientoTemporal).Returns(_mockSetAlmacenamientoTemporal.Object);
            _mockContext.Setup(mc => mc.UspGetRegionalizacion_Ajustar_JSON(BpinBase)).Returns(mockFuentes.Object);

            _mockContextFactory.Setup(mcf => mcf.CrearContextoConConexion(ConfigurationManager.ConnectionStrings["MGAWebContexto"].ConnectionString)).Returns(_mockContext.Object);
        }

        [TestMethod]
        public void AjustesFuenteCofinanciacion()
        {
            _regionalizaFuentesServicio = new AjustesRegionalizaFuentesServicios(new AjustesRegionalizaFuentesPersistencia(_mockContextFactory.Object), new PersistenciaTemporal(_mockContextFactory.Object), new AuditoriaServicios());
            var result = _regionalizaFuentesServicio.ObtenerAjustesRegionalizaFuentes(new ParametrosConsultaDto
            {
                Bpin = BpinBase,
                AccionId = new Guid("7D7FCC8B-048F-451F-9EE3-CAE2A84D1241"),
                InstanciaId = new Guid("1174193F-3F6E-4BCA-8E0E-CCBD59DB113B")
            });
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void AjustesFuenteCofinanciacionPreviewTest()
        {

            var result = _regionalizaFuentesServicio.ObtenerAjustesRegionalizaFuentesPreview();
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Regionalizacion.Count >= 1);
        }

        [TestMethod]
        public void AjustesFuenteCofinanciacion_ObtenerDefinitivo_Test()
        {
            AjustesRegionalizaFuentesServicio = new AjustesRegionalizaFuentesServicios(new AjustesRegionalizaFuentesPersistencia(_mockContextFactory.Object), new PersistenciaTemporal(_mockContextFactory.Object), new AuditoriaServicios());
            var parametrosConsulta = new ParametrosConsultaDto()
            {
                AccionId = new Guid("7D7FCC8B-048F-451F-9EE3-CAE2A84D1241"),
                InstanciaId = new Guid("1174193F-3F6E-4BCA-8E0E-CCBD59DB113B"),
                Bpin = "202000000000005"
            };

            var resultado = AjustesRegionalizaFuentesServicio.Obtener(parametrosConsulta);
            Assert.IsNotNull(resultado);
        }

        [TestMethod]
        public void AjustesFuenteCofinanciacion_Obtener_Test()
        {
            AjustesRegionalizaFuentesServicio = new AjustesRegionalizaFuentesServicios(new AjustesRegionalizaFuentesPersistencia(_mockContextFactory.Object), new PersistenciaTemporal(_mockContextFactory.Object), new AuditoriaServicios());
            var parametrosConsulta = new ParametrosConsultaDto()
            {
                AccionId = new Guid("7D7FCC8B-048F-451F-9EE3-CAE2A84D1241"),
                InstanciaId = new Guid("1174193F-3F6E-4BCA-8E0E-CCBD59DB113B"),
                Bpin = "202000000000005"
            };

            var resultado = AjustesRegionalizaFuentesServicio.ObtenerAjustesRegionalizaFuentes(parametrosConsulta);
            Assert.IsNotNull(resultado);
        }

        [TestMethod]
        public void AjustesFuenteCofinanciacion_GuardarDefinitivo()
        {
            var parametrosGuardar = new ParametrosGuardarDto<AjustesRegionalizaFuentesDto>()
            {
                AccionId = new Guid("7D7FCC8B-048F-451F-9EE3-CAE2A84D1241"),
                InstanciaId = new Guid("1174193F-3F6E-4BCA-8E0E-CCBD59DB113B"),
                Usuario = "Leticia01",
                Contenido = new AjustesRegionalizaFuentesDto()
                {
                    ProyectoId = 72210,
                    BPIN = "202000000000005",
                    Regionalizacion = new List<AjustesRegionalizaFuentesRegionalizacionDto> {
                        new AjustesRegionalizaFuentesRegionalizacionDto()
                        {
                           Vigencia = 2019
                        }
                    },
                }
            };

            var parametroAuditoria = new ParametrosAuditoriaDto()
            {
                Usuario = "Leticia01",
                Ip = "localhost"
            };

            AjustesRegionalizaFuentesServicio.Guardar(parametrosGuardar, parametroAuditoria, false);
        }
    }
}
