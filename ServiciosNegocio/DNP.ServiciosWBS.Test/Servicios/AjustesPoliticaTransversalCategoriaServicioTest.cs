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
    public class AjustesPoliticaTransversalCategoriaServicioTest
    {
        private string Bpin { get; set; }
        private IAjustesPoliticaTransversalCategoriaPersistencia AjustesPoliticaTransversalCategoriaPersistencia { get; set; }
        private IPersistenciaTemporal PersistenciaTemporal { get; set; }
        private IAuditoriaServicios AuditoriaServicio { get; set; }
        private AjustesPoliticaTransversalCategoriaServicios AjustesPoliticaTransversalCategoriaServicios { get; set; }

        private readonly Mock<MockableDbSetWithExtensions<AlmacenamientoTemporal>> _mockSet = new Mock<MockableDbSetWithExtensions<AlmacenamientoTemporal>>();
        private readonly Mock<MGAWebContexto> _mockContext = new Mock<MGAWebContexto>();
        private readonly Mock<IContextoFactory> _mockContextFactory = new Mock<IContextoFactory>();

        [TestInitialize]
        public void Init()
        {
            Bpin = "202000000000005";
            var contenedor = UnityConfig.Container;
            AjustesPoliticaTransversalCategoriaPersistencia = contenedor.Resolve<IAjustesPoliticaTransversalCategoriaPersistencia>();
            PersistenciaTemporal = contenedor.Resolve<IPersistenciaTemporal>();
            AuditoriaServicio = contenedor.Resolve<IAuditoriaServicios>();
            AjustesPoliticaTransversalCategoriaServicios = new AjustesPoliticaTransversalCategoriaServicios(AjustesPoliticaTransversalCategoriaPersistencia, PersistenciaTemporal, AuditoriaServicio);

            var data = new List<AlmacenamientoTemporal>().AsQueryable();

            var objetoRetornoAjustesPoliticaTransversalCategoria = "";
            var mockAjustesPoliticaTransversalCategoria = new Mock<ObjectResult<string>>();
            mockAjustesPoliticaTransversalCategoria.SetupReturn(objetoRetornoAjustesPoliticaTransversalCategoria);

            _mockSet.As<IQueryable<AlmacenamientoTemporal>>().Setup(m => m.Provider).Returns(data.Provider);
            _mockSet.As<IQueryable<AlmacenamientoTemporal>>().Setup(m => m.Expression).Returns(data.Expression);
            _mockSet.As<IQueryable<AlmacenamientoTemporal>>().Setup(m => m.ElementType).Returns(data.ElementType);
            _mockSet.As<IQueryable<AlmacenamientoTemporal>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            _mockContext.Setup(m => m.AlmacenamientoTemporal).Returns(_mockSet.Object);
            _mockContext.Setup(mc => mc.UspGetPoliticasTransversalesBeneficiariosCategorias_Ajustes_JSON(Bpin)).Returns(mockAjustesPoliticaTransversalCategoria.Object);

            _mockContextFactory.Setup(mcf => mcf.CrearContextoConConexion(ConfigurationManager.ConnectionStrings["MGAWebContexto"].ConnectionString)).Returns(_mockContext.Object);
        }

        [TestMethod]
        public void PoliticaTransversalCategoriaTest()
        {
            AjustesPoliticaTransversalCategoriaServicios = new AjustesPoliticaTransversalCategoriaServicios(new AjustesPoliticaTransversalCategoriaPersistencia(_mockContextFactory.Object), new PersistenciaTemporal(_mockContextFactory.Object), AuditoriaServicio);
            var parametrosConsulta = new ParametrosConsultaDto()
            {
                Bpin = Bpin,
                AccionId = new Guid("A027C5CC-196F-4683-AEF9-B754B0083273"),
                InstanciaId = new Guid("CDED65AA-EE09-46BB-9AA9-C91CA7AD80AA")
            };
            var result = AjustesPoliticaTransversalCategoriaServicios.ObtenerAjustesPoliticaTransversalCategoria(parametrosConsulta);
            Assert.IsNull(result);
        }

        [TestMethod]
        public void PoliticaTransversalCategoriaMockTest()
        {
            AjustesPoliticaTransversalCategoriaServicios = new AjustesPoliticaTransversalCategoriaServicios(new AjustesPoliticaTransversalCategoriaPersistencia(_mockContextFactory.Object), new PersistenciaTemporal(_mockContextFactory.Object), AuditoriaServicio);
            var parametrosConsulta = new ParametrosConsultaDto()
            {
                Bpin = Bpin,
                AccionId = new Guid("A027C5CC-196F-4683-AEF9-B754B0083273"),
                InstanciaId = new Guid("CDED65AA-EE09-46BB-9AA9-C91CA7AD80AA")
            };
            var result = AjustesPoliticaTransversalCategoriaServicios.ObtenerAjustesPoliticaTransversalCategoria(parametrosConsulta);
            Assert.IsNull(result);
        }

        [TestMethod]
        public void AjustesPoliticaTransversalCategoriaPreviewTest()
        {
            var result = AjustesPoliticaTransversalCategoriaServicios.ObtenerAjustesPoliticaTransversalCategoriaPreview();
            Assert.IsNotNull(result);
        }
    }
}
