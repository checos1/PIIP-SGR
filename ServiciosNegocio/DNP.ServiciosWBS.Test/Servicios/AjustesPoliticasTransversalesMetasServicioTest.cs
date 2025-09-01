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
    public class AjustesPoliticasTransversalesMetasServicioTest
    {
        private string Bpin { get; set; }
        private IPersistenciaTemporal PersistenciaTemporal { get; set; }
        private IAuditoriaServicios AuditoriaServicio { get; set; }
        private IAjustesPoliticasTransversalesMetasPersistencia _ajustesPoliticasTransversalesMetasPersistencia { get; set; }
        private AjustesPoliticasTransversalesMetasServicios _ajustesPoliticasTransversalesMetasServicios { get; set; }
                
        private readonly Mock<MockableDbSetWithExtensions<AlmacenamientoTemporal>> _mockSet = new Mock<MockableDbSetWithExtensions<AlmacenamientoTemporal>>();
        private readonly Mock<MGAWebContexto> _mockContext = new Mock<MGAWebContexto>();
        private readonly Mock<IContextoFactory> _mockContextFactory = new Mock<IContextoFactory>();

        [TestInitialize]
        public void Init()
        {
            Bpin = "202000000000005";
            var contenedor = UnityConfig.Container;
            _ajustesPoliticasTransversalesMetasPersistencia = contenedor.Resolve<IAjustesPoliticasTransversalesMetasPersistencia>();
            PersistenciaTemporal = contenedor.Resolve<IPersistenciaTemporal>();
            AuditoriaServicio = contenedor.Resolve<IAuditoriaServicios>();
            _ajustesPoliticasTransversalesMetasServicios = new AjustesPoliticasTransversalesMetasServicios(_ajustesPoliticasTransversalesMetasPersistencia, PersistenciaTemporal, AuditoriaServicio);

            var data = new List<AlmacenamientoTemporal>().AsQueryable();

            var objetoRetornoAjustesPoliticaTransversalCategoria = "";
            var mockAjustesPoliticasTransversalesMetas = new Mock<ObjectResult<string>>();
            mockAjustesPoliticasTransversalesMetas.SetupReturn(objetoRetornoAjustesPoliticaTransversalCategoria);

            _mockSet.As<IQueryable<AlmacenamientoTemporal>>().Setup(m => m.Provider).Returns(data.Provider);
            _mockSet.As<IQueryable<AlmacenamientoTemporal>>().Setup(m => m.Expression).Returns(data.Expression);
            _mockSet.As<IQueryable<AlmacenamientoTemporal>>().Setup(m => m.ElementType).Returns(data.ElementType);
            _mockSet.As<IQueryable<AlmacenamientoTemporal>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            _mockContext.Setup(m => m.AlmacenamientoTemporal).Returns(_mockSet.Object);
            _mockContext.Setup(mc => mc.UspGetPoliticasTransversalesMetas_Ajustar_JSON(Bpin)).Returns(mockAjustesPoliticasTransversalesMetas.Object);

            _mockContextFactory.Setup(mcf => mcf.CrearContextoConConexion(ConfigurationManager.ConnectionStrings["MGAWebContexto"].ConnectionString)).Returns(_mockContext.Object);
        }

        [TestMethod]
        public void AjustesPoliticasTransversalesMetasTest()
        {
            _ajustesPoliticasTransversalesMetasServicios = new AjustesPoliticasTransversalesMetasServicios(new AjustesPoliticasTransversalesMetasPersistencia(_mockContextFactory.Object), new PersistenciaTemporal(_mockContextFactory.Object), AuditoriaServicio);
            var parametrosConsulta = new ParametrosConsultaDto()
            {
                Bpin = Bpin,
                AccionId = new Guid("B3C505D2-573D-455D-BB9F-7D7390A68CB6"),
                InstanciaId = new Guid("FF02D036-FD30-4295-A760-DE10C2B25F4C")
            };
            var result = _ajustesPoliticasTransversalesMetasServicios.ObtenerAjustesPoliticasTransversalesMetas(parametrosConsulta);
            Assert.IsNull(result);
        }

        [TestMethod]
        public void AjustesPoliticasTransversalesMetasMockTest()
        {
            _ajustesPoliticasTransversalesMetasServicios = new AjustesPoliticasTransversalesMetasServicios(new AjustesPoliticasTransversalesMetasPersistencia(_mockContextFactory.Object), new PersistenciaTemporal(_mockContextFactory.Object), AuditoriaServicio);
            var parametrosConsulta = new ParametrosConsultaDto()
            {
                Bpin = Bpin,
                AccionId = new Guid("B3C505D2-573D-455D-BB9F-7D7390A68CB6"),
                InstanciaId = new Guid("FF02D036-FD30-4295-A760-DE10C2B25F4C")
            };
            var result = _ajustesPoliticasTransversalesMetasServicios.ObtenerAjustesPoliticasTransversalesMetas(parametrosConsulta);
            Assert.IsNull(result);
        }

        [TestMethod]
        public void AjustesPoliticasTransversalesMetasPreviewTest()
        {
            var result = _ajustesPoliticasTransversalesMetasServicios.ObtenerAjustesPoliticasTransversalesMetasPreview();
            Assert.IsNotNull(result);
        }
    }
}
