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
    using ServiciosNegocio.Dominio.Dto.Proyectos;
    using ServiciosNegocio.Test.Mock;
    using ServiciosWBS.Servicios.Implementaciones;
    using ServiciosWBS.Servicios.Implementaciones.Transversales;
    using ServiciosWBS.Servicios.Interfaces.Transversales;
    using Unity;

    [TestClass]
    public class LocalizacionServicioTest
    {
        private string Bpin { get; set; }
        private ILocalizacionPersistencia LocalizacionPersistencia { get; set; }
        private IPersistenciaTemporal PersistenciaTemporal { get; set; }
        private IAuditoriaServicios AuditoriaServicio { get; set; }
        private readonly ISeccionCapituloPersistencia _seccionCapituloPersistencia;
        private LocalizacionServicios LocalizacionServicios { get; set; }

        private readonly Mock<MockableDbSetWithExtensions<AlmacenamientoTemporal>> _mockSet = new Mock<MockableDbSetWithExtensions<AlmacenamientoTemporal>>();
        private readonly Mock<MGAWebContexto> _mockContext = new Mock<MGAWebContexto>();
        private readonly Mock<IContextoFactory> _mockContextFactory = new Mock<IContextoFactory>();

        [TestInitialize]
        public void Init()
        {
            Bpin = "202000000000005";
            var contenedor = UnityConfig.Container;
            LocalizacionPersistencia = contenedor.Resolve<ILocalizacionPersistencia>();
            PersistenciaTemporal = contenedor.Resolve<IPersistenciaTemporal>();
            AuditoriaServicio = contenedor.Resolve<IAuditoriaServicios>();
            LocalizacionServicios = new LocalizacionServicios(LocalizacionPersistencia, PersistenciaTemporal, AuditoriaServicio);

            var data = new List<AlmacenamientoTemporal>().AsQueryable();

            var objetoRetornoLocalizacion = "";
            var mockLocalizacion = new Mock<ObjectResult<string>>();
            mockLocalizacion.SetupReturn(objetoRetornoLocalizacion);

            _mockSet.As<IQueryable<AlmacenamientoTemporal>>().Setup(m => m.Provider).Returns(data.Provider);
            _mockSet.As<IQueryable<AlmacenamientoTemporal>>().Setup(m => m.Expression).Returns(data.Expression);
            _mockSet.As<IQueryable<AlmacenamientoTemporal>>().Setup(m => m.ElementType).Returns(data.ElementType);
            _mockSet.As<IQueryable<AlmacenamientoTemporal>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            _mockContext.Setup(m => m.AlmacenamientoTemporal).Returns(_mockSet.Object);
            _mockContext.Setup(mc => mc.UspGetLocalizacion_JSON(Bpin)).Returns(mockLocalizacion.Object);

            _mockContextFactory.Setup(mcf => mcf.CrearContextoConConexion(ConfigurationManager.ConnectionStrings["MGAWebContexto"].ConnectionString)).Returns(_mockContext.Object);
        }

        [TestMethod]
        public void LocalizacionTest()
        {
            LocalizacionServicios = new LocalizacionServicios(new LocalizacionPersistencia(_mockContextFactory.Object, _seccionCapituloPersistencia), new PersistenciaTemporal(_mockContextFactory.Object), AuditoriaServicio);
            var parametrosConsulta = new ParametrosConsultaDto()
            {
                Bpin = Bpin,
                AccionId = new Guid("E1166DAB-B9C8-42D7-B51C-433D99A4765F"),
                InstanciaId = new Guid("DE4AB25A-0A06-4C00-B57B-BE9CFF3AB320")
            };
            var result = LocalizacionServicios.ObtenerLocalizacion(parametrosConsulta);
            Assert.IsNull(result);
        }

        [TestMethod]
        public void LocalizacionMockTest()
        {
            LocalizacionServicios = new LocalizacionServicios(new LocalizacionPersistencia(_mockContextFactory.Object, _seccionCapituloPersistencia), new PersistenciaTemporal(_mockContextFactory.Object), AuditoriaServicio);
            var parametrosConsulta = new ParametrosConsultaDto()
            {
                Bpin = Bpin,
                AccionId = new Guid("E1166DAB-B9C8-42D7-B51C-433D99A4765F"),
                InstanciaId = new Guid("DE4AB25A-0A06-4C00-B57B-BE9CFF3AB320")
            };
            var result = LocalizacionServicios.ObtenerLocalizacion(parametrosConsulta);
            Assert.IsNull(result);
        }

        [TestMethod]
        public void LocalizacionPreviewTest()
        {
            var result = LocalizacionServicios.ObtenerLocalizacionPreview();
            Assert.IsNotNull(result);
        }
    }
}
