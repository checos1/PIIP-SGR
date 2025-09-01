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
    using ServiciosNegocio.Dominio.Dto.Focalizacion;
    using ServiciosNegocio.Test.Mock;
    using ServiciosWBS.Servicios.Implementaciones;
    using ServiciosWBS.Servicios.Implementaciones.Transversales;
    using ServiciosWBS.Servicios.Interfaces.Transversales;
    using Unity;

    [TestClass]
    public class PoliticaTransversalCategoriaServicioTest
    {
        private string Bpin { get; set; }
        private IPoliticaTransversalCategoriaPersistencia PoliticaTransversalCategoriaPersistencia { get; set; }
        private IPersistenciaTemporal PersistenciaTemporal { get; set; }
        private IAuditoriaServicios AuditoriaServicio { get; set; }
        private PoliticaTransversalCategoriaServicios PoliticaTransversalCategoriaServicios { get; set; }

        private readonly Mock<MockableDbSetWithExtensions<AlmacenamientoTemporal>> _mockSet = new Mock<MockableDbSetWithExtensions<AlmacenamientoTemporal>>();
        private readonly Mock<MGAWebContexto> _mockContext = new Mock<MGAWebContexto>();
        private readonly Mock<IContextoFactory> _mockContextFactory = new Mock<IContextoFactory>();

        [TestInitialize]
        public void Init()
        {
            Bpin = "202000000000005";
            var contenedor = UnityConfig.Container;
            PoliticaTransversalCategoriaPersistencia = contenedor.Resolve<IPoliticaTransversalCategoriaPersistencia>();
            PersistenciaTemporal = contenedor.Resolve<IPersistenciaTemporal>();
            AuditoriaServicio = contenedor.Resolve<IAuditoriaServicios>();
            PoliticaTransversalCategoriaServicios = new PoliticaTransversalCategoriaServicios(PoliticaTransversalCategoriaPersistencia, PersistenciaTemporal, AuditoriaServicio);

            var data = new List<AlmacenamientoTemporal>().AsQueryable();

            var objetoRetornoPoliticaTransversalCategoria = "";
            var mockPoliticaTransversalCategoria = new Mock<ObjectResult<string>>();
            mockPoliticaTransversalCategoria.SetupReturn(objetoRetornoPoliticaTransversalCategoria);

            _mockSet.As<IQueryable<AlmacenamientoTemporal>>().Setup(m => m.Provider).Returns(data.Provider);
            _mockSet.As<IQueryable<AlmacenamientoTemporal>>().Setup(m => m.Expression).Returns(data.Expression);
            _mockSet.As<IQueryable<AlmacenamientoTemporal>>().Setup(m => m.ElementType).Returns(data.ElementType);
            _mockSet.As<IQueryable<AlmacenamientoTemporal>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            _mockContext.Setup(m => m.AlmacenamientoTemporal).Returns(_mockSet.Object);
            _mockContext.Setup(mc => mc.uspGetPoliticasCategorias_JSON(Bpin)).Returns(mockPoliticaTransversalCategoria.Object);

            _mockContextFactory.Setup(mcf => mcf.CrearContextoConConexion(ConfigurationManager.ConnectionStrings["MGAWebContexto"].ConnectionString)).Returns(_mockContext.Object);
        }

        [TestMethod]
        public void PoliticaTransversalCategoriaTest()
        {
            try
            {
                PoliticaTransversalCategoriaServicios = new PoliticaTransversalCategoriaServicios(new PoliticaTransversalCategoriaPersistencia(_mockContextFactory.Object), new PersistenciaTemporal(_mockContextFactory.Object), AuditoriaServicio);
                var parametrosConsulta = new ParametrosConsultaDto()
                {
                    Bpin = Bpin,
                    AccionId = new Guid("48ACE8E7-37DB-BF71-7CE5-4996ED17DEA1"),
                    InstanciaId = new Guid("1CC3A855-12F1-4113-A044-014886298AA3")
                };
                var result = PoliticaTransversalCategoriaServicios.ObtenerPoliticaTransversalCategoria(parametrosConsulta);
                Assert.IsNull(result);
            }
            catch (Exception ex)
            {
                Assert.IsTrue(!string.IsNullOrEmpty(ex.Message));
            }
           
        }

        [TestMethod]
        public void PoliticaTransversalCategoriaMockTest()
        {
            try
            {
                PoliticaTransversalCategoriaServicios = new PoliticaTransversalCategoriaServicios(new PoliticaTransversalCategoriaPersistencia(_mockContextFactory.Object), new PersistenciaTemporal(_mockContextFactory.Object), AuditoriaServicio);
                var parametrosConsulta = new ParametrosConsultaDto()
                {
                    Bpin = Bpin,
                    AccionId = new Guid("48ACE8E7-37DB-BF71-7CE5-4996ED17DEA1"),
                    InstanciaId = new Guid("1CC3A855-12F1-4113-A044-014886298AA3")
                };
                var result = PoliticaTransversalCategoriaServicios.ObtenerPoliticaTransversalCategoria(parametrosConsulta);
                Assert.IsNull(result);
            }
            catch (Exception ex)
            {
                Assert.IsTrue(!string.IsNullOrEmpty(ex.Message));
            }
           
        }

        [TestMethod]
        public void PoliticaTransversalCategoriaPreviewTest()
        {
            var result = PoliticaTransversalCategoriaServicios.ObtenerPoliticaTransversalCategoriaPreview();
            Assert.IsNotNull(result);
        }
    }
}
