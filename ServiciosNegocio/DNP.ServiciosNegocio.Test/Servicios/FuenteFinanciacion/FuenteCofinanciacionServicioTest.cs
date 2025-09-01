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

namespace DNP.ServiciosNegocio.Test.Servicios.FuenteCofinanciacion
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
    public class FuenteCofinanciacionServicioTest
    {
        private string BpinBase { get; set; }
        private IFuenteCofinanciacionServicio _fuenteCofinanciacionServicio;
        private FuenteCofinanciacionServicio FuenteCofinanciacionServicio { get; set; }
        private readonly Mock<MGAWebContexto> _mockContext = new Mock<MGAWebContexto>();
        private readonly Mock<IContextoFactory> _mockContextFactory = new Mock<IContextoFactory>();
        private IFuenteCofinanciacionPersistencia FuenteCofinanciacionPersistencia { get; set; }
        private IPersistenciaTemporal PersistenciaTemporal { get; set; }
        private IAuditoriaServicios AuditoriaServicio { get; set; }
        private readonly Mock<DbSet<AlmacenamientoTemporal>> _mockSetAlmacenamientoTemporal = new Mock<DbSet<AlmacenamientoTemporal>>();

        [TestInitialize]
        public void Init()
        {
            BpinBase = "202000000000005";
            var contenedor = UnityConfig.Container;
            _fuenteCofinanciacionServicio = contenedor.Resolve<IFuenteCofinanciacionServicio>();
            FuenteCofinanciacionPersistencia = UnityContainerExtensions.Resolve<IFuenteCofinanciacionPersistencia>(contenedor);
            PersistenciaTemporal = contenedor.Resolve<IPersistenciaTemporal>();
            AuditoriaServicio = contenedor.Resolve<IAuditoriaServicios>();
            FuenteCofinanciacionServicio = new FuenteCofinanciacionServicio(FuenteCofinanciacionPersistencia, PersistenciaTemporal, AuditoriaServicio);

            var dataAlmacenamientoTemporal = new List<AlmacenamientoTemporal>()
                                           {
                                               new AlmacenamientoTemporal()
                                               {
                                                   AccionId = new Guid("48ACE8E7-37DB-BF71-7CE5-4996ED17DEA1"),
                                                   InstanciaId =    new Guid("1CC3A855-12F1-4113-A044-014886298AA3"),
                                                   Json = "{\"AcumulaIndicadorPrincipal\": true,\"AlternativaId\" : 2,\"Cantidad\" : 5,\"Complemento\" : \"Prubea\",\"CondicionDeseadaId\" : 4,\"CPCId\" : 5,\r              \"FuenteVerificacion\" : \"PRUEB fuenteVerificacion\",\"IndicadorPrincipal\" : \"PRUEBA INDICADOR PRINCIPAL\",\"Meta\" : 10,\"MetaTotalIndicadorPrincipal\" : 15,\r                \"ProductoIndicador\" :\r[{\r\"AcumulaIndicadorSecundario\" : true,\r\"IndicadorSecundarioId\":2,\r \"IndicadorSecundario\":\"PRUEBA INDICADOR SECUNDARIO\",\r          \"MetaTotalIndicadorSecundario\":10,\r\"UnidadMedidaIndicadorSecundario\":4,\r\"ProductoIndicadorDetalle\":[\r{\r\"DetalleIndicadorProductoId\":4,\r\"MetaVigente\":5,\r\"Vigencia\":4,\r\"RegionalizacionMetas\":[{\r\"AgrupacionId\":2,\r\"DepartamentoId\":1,\r\"FechaCreacion\":\"2018/02/03\",\r\"MunicipioId\":2,\r\"RegionalizacionMetaId\":3,\r\"RegionId\":1,\r\"TotalMetaVigente\":58,\r\"VigenciaRegionalizada\":2\r},{\r\"AgrupacionId\":3,\r\"DepartamentoId\":2,\r\"FechaCreacion\":\"2018/02/03\",\r\"MunicipioId\":1,\r\"RegionalizacionMetaId\":4,\r\"RegionId\":5,\r\"TotalMetaVigente\":3,\r\"VigenciaRegionalizada\":2\r}]\r }]\r\r},{\r\"AcumulaIndicadorSecundario\" : true,\r\"IndicadorSecundarioId\":2,\r\"IndicadorSecundario\":\"PRUEBA INDICADOR SECUNDARIO\",\r\"MetaTotalIndicadorSecundario\":10,\r\"UnidadMedidaIndicadorSecundario\":4,\r\"ProductoIndicadorDetalle\":[\r{\r\"DetalleIndicadorProductoId\":4,\r\"MetaVigente\":5,\r\"Vigencia\":4,\r\"RegionalizacionMetas\":[{\r\"AgrupacionId\":2,\r\"DepartamentoId\":1,\r\"FechaCreacion\":\"2018/02/03\",\r\"MunicipioId\":2,\r\"RegionalizacionMetaId\":3,\r\"RegionId\":1,\r\"TotalMetaVigente\":58,\r\"VigenciaRegionalizada\":2\r},{\r\"AgrupacionId\":3,\r\"DepartamentoId\":2,\r\"FechaCreacion\":\"2018/02/03\",\r\"MunicipioId\":1,\r\"RegionalizacionMetaId\":4,\r\"RegionId\":5,\r\"TotalMetaVigente\":3,\r\"VigenciaRegionalizada\":2\r}]\r}]\r} ]\r}"
                                               }
                                           }.AsQueryable();

            string objetoRetornoFuentes = "{\"ProyectoId\":null,\"CodigoBPIN\":\"202000000000005\",\"CR\":null,\"Cofinanciacion\":[{\"CofinanciadorId\":0,\"TipoCofinanciadorId\":0,\"TipoCofinanciador\":\"\",\"Cofinanciador\":\"\",\"Fuentes\":[{\"FuenteId\":0,\"Fuente\":\"\",\"TipoEntidadId\":0,\"TipoEntidad\":\"\",\"EntidadId\":0,\"Entidad\":\"\",\"TipoRecursoId\":0,\"TipoRecurso\":\"\",\"Seleccionado\":0}]}]}";
            var mockFuentes = new Mock<ObjectResult<string>>();
            mockFuentes.SetupReturn(objetoRetornoFuentes);

            _mockSetAlmacenamientoTemporal.As<IQueryable<AlmacenamientoTemporal>>().Setup(m => m.Provider).Returns(dataAlmacenamientoTemporal.Provider);
            _mockSetAlmacenamientoTemporal.As<IQueryable<AlmacenamientoTemporal>>().Setup(m => m.Expression).Returns(dataAlmacenamientoTemporal.Expression);
            _mockSetAlmacenamientoTemporal.As<IQueryable<AlmacenamientoTemporal>>().Setup(m => m.ElementType).Returns(dataAlmacenamientoTemporal.ElementType);
            _mockSetAlmacenamientoTemporal.As<IQueryable<AlmacenamientoTemporal>>().Setup(m => m.GetEnumerator()).Returns(dataAlmacenamientoTemporal.GetEnumerator());

            _mockContext.Setup(m => m.AlmacenamientoTemporal).Returns(_mockSetAlmacenamientoTemporal.Object);
            _mockContext.Setup(mc => mc.uspGetFuentesFinanciacion_Cofinanciacion_JSON(BpinBase)).Returns(mockFuentes.Object);

            _mockContextFactory.Setup(mcf => mcf.CrearContextoConConexion(ConfigurationManager.ConnectionStrings["MGAWebContexto"].ConnectionString)).Returns(_mockContext.Object);
        }

        [TestMethod]
        public void FuenteCofinanciacion()
        {
            _fuenteCofinanciacionServicio = new FuenteCofinanciacionServicio(new FuenteCofinanciacionPersistencia(_mockContextFactory.Object), new PersistenciaTemporal(_mockContextFactory.Object), new AuditoriaServicios());
            var result = _fuenteCofinanciacionServicio.ObtenerFuenteCofinanciacionProyecto(new ParametrosConsultaDto
            {
                Bpin = BpinBase,
                AccionId = new Guid("48ACE8E7-37DB-BF71-7CE5-4996ED17DEA1"),
                InstanciaId = new Guid("1CC3A855-12F1-4113-A044-014886298AA3")
            });
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void FuenteCofinanciacionPreviewTest()
        {

            var result = _fuenteCofinanciacionServicio.ObtenerFuenteCofinanciacionProyectoPreview();
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Cofinanciacion.Count >= 1);
        }

        [TestMethod]
        public void FuenteCofinanciacion_ObtenerDefinitivo_TestTemporal()
        {
            FuenteCofinanciacionServicio = new FuenteCofinanciacionServicio(new FuenteCofinanciacionPersistencia(_mockContextFactory.Object), new PersistenciaTemporal(_mockContextFactory.Object), new AuditoriaServicios());
            var parametrosConsulta = new ParametrosConsultaDto()
            {
                AccionId = new Guid("48ACE8E7-37DB-BF71-7CE5-4996ED17DEA1"),
                InstanciaId = new Guid("1CC3A855-12F1-4113-A044-014886298AA3"),
                Bpin = "202000000000005"
            };

            var resultado = FuenteCofinanciacionServicio.Obtener(parametrosConsulta);
            Assert.IsNotNull(resultado);
        }

        [TestMethod]
        public void FuenteCofinanciacion_ObtenerDefinitivo_TestPersistencia()
        {
            FuenteCofinanciacionServicio = new FuenteCofinanciacionServicio(new FuenteCofinanciacionPersistencia(_mockContextFactory.Object), new PersistenciaTemporal(_mockContextFactory.Object), new AuditoriaServicios());
            var parametrosConsulta = new ParametrosConsultaDto()
            {
                AccionId = new Guid("418D76AC-F081-4D73-A05A-530CD7C6AFF6"),
                InstanciaId = new Guid("6A0E8FC3-73CF-46B5-B59C-0F683A30AE1D"),
                Bpin = "202000000000005"
            };

            var resultado = FuenteCofinanciacionServicio.Obtener(parametrosConsulta);
            Assert.IsNotNull(resultado);
        }

        [TestMethod]
        public void FuenteCofinanciacion_Obtener_Test()
        {
            FuenteCofinanciacionServicio = new FuenteCofinanciacionServicio(new FuenteCofinanciacionPersistencia(_mockContextFactory.Object), new PersistenciaTemporal(_mockContextFactory.Object), new AuditoriaServicios());
            var parametrosConsulta = new ParametrosConsultaDto()
            {
                AccionId = new Guid("48ACE8E7-37DB-BF71-7CE5-4996ED17DEA1"),
                InstanciaId = new Guid("1CC3A855-12F1-4113-A044-014886298AA3"),
                Bpin = "202000000000005"
            };

            var resultado = FuenteCofinanciacionServicio.ObtenerFuenteCofinanciacionProyecto(parametrosConsulta);
            Assert.IsNotNull(resultado);
        }

        [TestMethod]
        public void FuenteCofinanciacion_GuardarDefinitivo()
        {
            var parametrosGuardar = new ParametrosGuardarDto<FuenteCofinanciacionProyectoDto>()
            {
                AccionId = new Guid("48ACE8E7-37DB-BF71-7CE5-4996ED17DEA1"),
                InstanciaId = new Guid("1CC3A855-12F1-4113-A044-014886298AA3"),
                Usuario = "Leticia01",
                Contenido = new FuenteCofinanciacionProyectoDto()
                {
                    ProyectoId = 72210,
                    CodigoBPIN = "202000000000005",
                    CR = 2,
                    Cofinanciacion = new List<FuenteCofinanciacionDto> {
                        new FuenteCofinanciacionDto()
                        {
                           CofinanciadorId = 10,
                           TipoCofinanciadorId = 2,
                           TipoCofinanciador = "Rubro",
                           Cofinanciador = "RF-2021-MA"
                        }
                    },
                }
            };

            var parametroAuditoria = new ParametrosAuditoriaDto()
            {
                Usuario = "Leticia01",
                Ip = "localhost"
            };

            FuenteCofinanciacionServicio.Guardar(parametrosGuardar, parametroAuditoria, false);
        }
    }
}
