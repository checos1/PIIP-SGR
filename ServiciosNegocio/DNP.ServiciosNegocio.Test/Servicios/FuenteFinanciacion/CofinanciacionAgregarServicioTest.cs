using System.Collections.Generic;
using DNP.ServiciosNegocio.Persistencia.Interfaces;
using DNP.ServiciosNegocio.Persistencia.Modelo;
using DNP.ServiciosNegocio.Servicios.Implementaciones.FuenteFinanciacion;
using DNP.ServiciosNegocio.Servicios.Interfaces.FuenteFinanciacion;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Configuration;
using System.Data.Entity;
using Unity;

namespace DNP.ServiciosNegocio.Test.Servicios.FuenteFinanciacion
{
    using System;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    using Comunes.Dto;
    using Comunes.Dto.Formulario;
    using Configuracion;
    using Persistencia.Interfaces.Genericos;
    using ServiciosNegocio.Servicios.Interfaces.Transversales;
    using Dominio.Dto.FuenteFinanciacion;
    using Mock;
    using Persistencia.Implementaciones.FuenteFinanciacion;
    using Persistencia.Implementaciones.Genericos;
    using Persistencia.Interfaces.FuenteFinanciacion;
    using ServiciosNegocio.Servicios.Implementaciones.Transversales;
    
    [TestClass]
    public class CofinanciacionAgregarServicioTest
    {
        private string BpinBase { get; set; }
        private ICofinanciacionAgregarServicio _cofinanciacionAgregarServicio;
        private CofinanciacionAgregarServicio CofinanciacionAgregarServicio { get; set; }
        private readonly Mock<MGAWebContexto> _mockContext = new Mock<MGAWebContexto>();
        private readonly Mock<IContextoFactory> _mockContextFactory = new Mock<IContextoFactory>();
        private ICofinanciacionAgregarPersistencia CofinanciacionAgregarPersistencia { get; set; }
        private IPersistenciaTemporal PersistenciaTemporal { get; set; }
        private IAuditoriaServicios AuditoriaServicio { get; set; }
        private readonly Mock<DbSet<AlmacenamientoTemporal>> _mockSetAlmacenamientoTemporal = new Mock<DbSet<AlmacenamientoTemporal>>();

        [TestInitialize]
        public void Init()
        {
            BpinBase = "202000000000005";
            var contenedor = UnityConfig.Container;
            _cofinanciacionAgregarServicio = contenedor.Resolve<ICofinanciacionAgregarServicio>();
            CofinanciacionAgregarPersistencia = UnityContainerExtensions.Resolve<ICofinanciacionAgregarPersistencia>(contenedor);
            PersistenciaTemporal = contenedor.Resolve<IPersistenciaTemporal>();
            AuditoriaServicio = contenedor.Resolve<IAuditoriaServicios>();
            CofinanciacionAgregarServicio = new CofinanciacionAgregarServicio(CofinanciacionAgregarPersistencia, PersistenciaTemporal, AuditoriaServicio);

            var dataAlmacenamientoTemporal = new List<AlmacenamientoTemporal>()
                                           {
                                               new AlmacenamientoTemporal()
                                               {
                                                   AccionId = new Guid("D2BA19EB-0487-4C94-8960-3A6047B81409"),
                                                   InstanciaId =    new Guid("BC135467-5041-4C0F-8AB7-EC2F09E02AAF"),
                                                   Json = "{\"AcumulaIndicadorPrincipal\": true,\"AlternativaId\" : 2,\"Cantidad\" : 5,\"Complemento\" : \"Prubea\",\"CondicionDeseadaId\" : 4,\"CPCId\" : 5,\r              \"FuenteVerificacion\" : \"PRUEB fuenteVerificacion\",\"IndicadorPrincipal\" : \"PRUEBA INDICADOR PRINCIPAL\",\"Meta\" : 10,\"MetaTotalIndicadorPrincipal\" : 15,\r                \"ProductoIndicador\" :\r[{\r\"AcumulaIndicadorSecundario\" : true,\r\"IndicadorSecundarioId\":2,\r \"IndicadorSecundario\":\"PRUEBA INDICADOR SECUNDARIO\",\r          \"MetaTotalIndicadorSecundario\":10,\r\"UnidadMedidaIndicadorSecundario\":4,\r\"ProductoIndicadorDetalle\":[\r{\r\"DetalleIndicadorProductoId\":4,\r\"MetaVigente\":5,\r\"Vigencia\":4,\r\"RegionalizacionMetas\":[{\r\"AgrupacionId\":2,\r\"DepartamentoId\":1,\r\"FechaCreacion\":\"2018/02/03\",\r\"MunicipioId\":2,\r\"RegionalizacionMetaId\":3,\r\"RegionId\":1,\r\"TotalMetaVigente\":58,\r\"VigenciaRegionalizada\":2\r},{\r\"AgrupacionId\":3,\r\"DepartamentoId\":2,\r\"FechaCreacion\":\"2018/02/03\",\r\"MunicipioId\":1,\r\"RegionalizacionMetaId\":4,\r\"RegionId\":5,\r\"TotalMetaVigente\":3,\r\"VigenciaRegionalizada\":2\r}]\r }]\r\r},{\r\"AcumulaIndicadorSecundario\" : true,\r\"IndicadorSecundarioId\":2,\r\"IndicadorSecundario\":\"PRUEBA INDICADOR SECUNDARIO\",\r\"MetaTotalIndicadorSecundario\":10,\r\"UnidadMedidaIndicadorSecundario\":4,\r\"ProductoIndicadorDetalle\":[\r{\r\"DetalleIndicadorProductoId\":4,\r\"MetaVigente\":5,\r\"Vigencia\":4,\r\"RegionalizacionMetas\":[{\r\"AgrupacionId\":2,\r\"DepartamentoId\":1,\r\"FechaCreacion\":\"2018/02/03\",\r\"MunicipioId\":2,\r\"RegionalizacionMetaId\":3,\r\"RegionId\":1,\r\"TotalMetaVigente\":58,\r\"VigenciaRegionalizada\":2\r},{\r\"AgrupacionId\":3,\r\"DepartamentoId\":2,\r\"FechaCreacion\":\"2018/02/03\",\r\"MunicipioId\":1,\r\"RegionalizacionMetaId\":4,\r\"RegionId\":5,\r\"TotalMetaVigente\":3,\r\"VigenciaRegionalizada\":2\r}]\r}]\r} ]\r}"
                                               }
                                           }.AsQueryable();

            string objetoRetornoFuentes = "";
            string objetoRetornoObtenerCofinanciacionAgregar = "{\"ProyectoId\":null,\"CodigoBPIN\":\"202000000000005\",\"CR\":null,\"Cofinanciacion\":[{\"ProyectoCofinanciadorId\":0,\"TipoCofinanciadorId\":0,\"TipoCofinanciador\":\"\",\"CofinanciadorId\":\"\"}]}";

            var mockFuentes = new Mock<ObjectResult<string>>();
            var mockObtenerCofinanciacionAgregar = new Mock<ObjectResult<string>>();
            mockFuentes.SetupReturn(objetoRetornoFuentes);
            mockObtenerCofinanciacionAgregar.SetupReturn(objetoRetornoObtenerCofinanciacionAgregar);

            _mockSetAlmacenamientoTemporal.As<IQueryable<AlmacenamientoTemporal>>().Setup(m => m.Provider).Returns(dataAlmacenamientoTemporal.Provider);
            _mockSetAlmacenamientoTemporal.As<IQueryable<AlmacenamientoTemporal>>().Setup(m => m.Expression).Returns(dataAlmacenamientoTemporal.Expression);
            _mockSetAlmacenamientoTemporal.As<IQueryable<AlmacenamientoTemporal>>().Setup(m => m.ElementType).Returns(dataAlmacenamientoTemporal.ElementType);
            _mockSetAlmacenamientoTemporal.As<IQueryable<AlmacenamientoTemporal>>().Setup(m => m.GetEnumerator()).Returns(dataAlmacenamientoTemporal.GetEnumerator());

            _mockContext.Setup(m => m.AlmacenamientoTemporal).Returns(_mockSetAlmacenamientoTemporal.Object);
            _mockContext.Setup(mc => mc.uspGetFuentesFinanciacion_Cofinanciacion_JSON(BpinBase)).Returns(mockFuentes.Object);
            _mockContext.Setup(mc => mc.uspGetCofinanciadorAgregar_JSON(BpinBase)).Returns(mockObtenerCofinanciacionAgregar.Object);

            _mockContextFactory.Setup(mcf => mcf.CrearContextoConConexion(ConfigurationManager.ConnectionStrings["MGAWebContexto"].ConnectionString)).Returns(_mockContext.Object);
        }

        [TestMethod]
        public void CofinanciacionAgregar()
        {
            _cofinanciacionAgregarServicio = new CofinanciacionAgregarServicio(new CofinanciacionAgregarPersistencia(_mockContextFactory.Object), new PersistenciaTemporal(_mockContextFactory.Object), new AuditoriaServicios());
            var result = _cofinanciacionAgregarServicio.ObtenerCofinanciacionAgregar(new ParametrosConsultaDto
            {
                Bpin = BpinBase,
                AccionId = new Guid("D2BA19EB-0487-4C94-8960-3A6047B81409"),
                InstanciaId = new Guid("BC135467-5041-4C0F-8AB7-EC2F09E02AAF")
            });
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void CofinanciacionAgregarPreviewTest()
        {

            var result = _cofinanciacionAgregarServicio.ObtenerCofinanciacionAgregarPreview();
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Cofinanciacion.Count >= 1);
        }

        [TestMethod]
        public void CofinanciacionAgregar_ObtenerDefinitivo_TestTemporal()
        {
            CofinanciacionAgregarServicio = new CofinanciacionAgregarServicio(new CofinanciacionAgregarPersistencia(_mockContextFactory.Object), new PersistenciaTemporal(_mockContextFactory.Object), new AuditoriaServicios());
            var parametrosConsulta = new ParametrosConsultaDto()
            {
                AccionId = new Guid("D2BA19EB-0487-4C94-8960-3A6047B81409"),
                InstanciaId = new Guid("BC135467-5041-4C0F-8AB7-EC2F09E02AAF"),
                Bpin = "202000000000005"
            };

            var resultado = CofinanciacionAgregarServicio.Obtener(parametrosConsulta);
            Assert.IsNotNull(resultado);
        }

        [TestMethod]
        public void CofinanciacionAgregar_ObtenerDefinitivo_TestPersistencia()
        {
            CofinanciacionAgregarServicio = new CofinanciacionAgregarServicio(new CofinanciacionAgregarPersistencia(_mockContextFactory.Object), new PersistenciaTemporal(_mockContextFactory.Object), new AuditoriaServicios());
            var parametrosConsulta = new ParametrosConsultaDto()
            {
                AccionId = new Guid("418D76AC-F081-4D73-A05A-530CD7C6AFF6"),
                InstanciaId = new Guid("6A0E8FC3-73CF-46B5-B59C-0F683A30AE1D"),
                Bpin = "202000000000005"
            };

            var resultado = CofinanciacionAgregarServicio.Obtener(parametrosConsulta);
            Assert.IsNotNull(resultado);
        }

        [TestMethod]
        public void CofinanciacionAgregar_Obtener_Test()
        {
            CofinanciacionAgregarServicio = new CofinanciacionAgregarServicio(new CofinanciacionAgregarPersistencia(_mockContextFactory.Object), new PersistenciaTemporal(_mockContextFactory.Object), new AuditoriaServicios());
            var parametrosConsulta = new ParametrosConsultaDto()
            {
                AccionId = new Guid("D2BA19EB-0487-4C94-8960-3A6047B81409"),
                InstanciaId = new Guid("BC135467-5041-4C0F-8AB7-EC2F09E02AAF"),
                Bpin = "202000000000005"
            };

            var resultado = CofinanciacionAgregarServicio.ObtenerCofinanciacionAgregar(parametrosConsulta);
            Assert.IsNotNull(resultado);
        }

        [TestMethod]
        public void CofinanciacionAgregar_GuardarDefinitivo()
        {
            var parametrosGuardar = new ParametrosGuardarDto<CofinanciacionProyectoDto>()
            {
                AccionId = new Guid("D2BA19EB-0487-4C94-8960-3A6047B81409"),
                InstanciaId = new Guid("BC135467-5041-4C0F-8AB7-EC2F09E02AAF"),
                Usuario = "Leticia01",
                Contenido = new CofinanciacionProyectoDto()
                {
                    ProyectoId = 72210,
                    CodigoBPIN = "202000000000005",
                    CR = 2,
                    Cofinanciacion = new List<CofinanciacionDto> {
                        new CofinanciacionDto()
                        {
                           ProyectoCofinanciadorId = 1,
                           TipoCofinanciadorId = 2,
                           TipoCofinanciador = "Rubro",
                           CofinanciadorId = "1"
                        }
                    },
                }
            };

            var parametroAuditoria = new ParametrosAuditoriaDto()
            {
                Usuario = "Leticia01",
                Ip = "localhost"
            };

            CofinanciacionAgregarServicio.Guardar(parametrosGuardar, parametroAuditoria, false);
        }
    }
}
