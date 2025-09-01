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
    public class AjustesUbicacionServicioTest
    {
        private string Bpin { get; set; }
        private IAjustesUbicacionPersistencia AjustesUbicacionPersistencia { get; set; }
        private IPersistenciaTemporal PersistenciaTemporal { get; set; }
        private IAuditoriaServicios AuditoriaServicio { get; set; }
        private AjustesUbicacionServicios AjustesUbicacionServicios { get; set; }

        private readonly Mock<MockableDbSetWithExtensions<AlmacenamientoTemporal>> _mockSet = new Mock<MockableDbSetWithExtensions<AlmacenamientoTemporal>>();
        private readonly Mock<MGAWebContexto> _mockContext = new Mock<MGAWebContexto>();
        private readonly Mock<IContextoFactory> _mockContextFactory = new Mock<IContextoFactory>();

        [TestInitialize]
        public void Init()
        {
            Bpin = "202000000000005";
            var contenedor = UnityConfig.Container;
            AjustesUbicacionPersistencia = contenedor.Resolve<IAjustesUbicacionPersistencia>();
            PersistenciaTemporal = contenedor.Resolve<IPersistenciaTemporal>();
            AuditoriaServicio = contenedor.Resolve<IAuditoriaServicios>();
            AjustesUbicacionServicios = new AjustesUbicacionServicios(AjustesUbicacionPersistencia, PersistenciaTemporal, AuditoriaServicio);

            var data = new List<AlmacenamientoTemporal>().AsQueryable();

            var objetoRetornoAjustesUbicacion = "";
            var mockAjustesUbicacion = new Mock<ObjectResult<string>>();
            mockAjustesUbicacion.SetupReturn(objetoRetornoAjustesUbicacion);

            _mockSet.As<IQueryable<AlmacenamientoTemporal>>().Setup(m => m.Provider).Returns(data.Provider);
            _mockSet.As<IQueryable<AlmacenamientoTemporal>>().Setup(m => m.Expression).Returns(data.Expression);
            _mockSet.As<IQueryable<AlmacenamientoTemporal>>().Setup(m => m.ElementType).Returns(data.ElementType);
            _mockSet.As<IQueryable<AlmacenamientoTemporal>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            _mockContext.Setup(m => m.AlmacenamientoTemporal).Returns(_mockSet.Object);
            _mockContext.Setup(mc => mc.UspGetLocalizacion_Ajustar_JSON(Bpin)).Returns(mockAjustesUbicacion.Object);

            _mockContextFactory.Setup(mcf => mcf.CrearContextoConConexion(ConfigurationManager.ConnectionStrings["MGAWebContexto"].ConnectionString)).Returns(_mockContext.Object);
        }

        [TestMethod]
        public void AjustesUbicacionTest()
        {
            AjustesUbicacionServicios = new AjustesUbicacionServicios(new AjustesUbicacionPersistencia(_mockContextFactory.Object), new PersistenciaTemporal(_mockContextFactory.Object), AuditoriaServicio);
            var parametrosConsulta = new ParametrosConsultaDto()
            {
                Bpin = Bpin,
                AccionId = new Guid("8E22189D-8FDE-47E1-883A-DEBD3C15A314"),
                InstanciaId = new Guid("2C7A9B06-C342-4060-9EBC-0F9EC17AF398")
            };
            var result = AjustesUbicacionServicios.ObtenerAjustesUbicacion(parametrosConsulta);
            Assert.IsNull(result);
        }

        [TestMethod]
        public void AjustesUbicacionMockTest()
        {
            AjustesUbicacionServicios = new AjustesUbicacionServicios(new AjustesUbicacionPersistencia(_mockContextFactory.Object), new PersistenciaTemporal(_mockContextFactory.Object), AuditoriaServicio);
            var parametrosConsulta = new ParametrosConsultaDto()
            {
                Bpin = Bpin,
                AccionId = new Guid("8E22189D-8FDE-47E1-883A-DEBD3C15A314"),
                InstanciaId = new Guid("2C7A9B06-C342-4060-9EBC-0F9EC17AF398")
            };
            var result = AjustesUbicacionServicios.ObtenerAjustesUbicacion(parametrosConsulta);
            Assert.IsNull(result);
        }

        [TestMethod]
        public void AjustesUbicacionPreviewTest()
        {
            var result = AjustesUbicacionServicios.ObtenerAjustesUbicacionPreview();
            Assert.IsNotNull(result);
        }
    }
}
