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
    public class PoliticaTransversalBeneficiarioServicioTest
    {
        private string Bpin { get; set; }
        private IPoliticaTransversalBeneficiarioPersistencia PoliticaTransversalBeneficiarioPersistencia { get; set; }
        private IPersistenciaTemporal PersistenciaTemporal { get; set; }
        private IAuditoriaServicios AuditoriaServicio { get; set; }
        private PoliticaTransversalBeneficiarioServicios PoliticaTransversalBeneficiarioServicios { get; set; }

        private readonly Mock<MockableDbSetWithExtensions<AlmacenamientoTemporal>> _mockSet = new Mock<MockableDbSetWithExtensions<AlmacenamientoTemporal>>();
        private readonly Mock<MGAWebContexto> _mockContext = new Mock<MGAWebContexto>();
        private readonly Mock<IContextoFactory> _mockContextFactory = new Mock<IContextoFactory>();

        [TestInitialize]
        public void Init()
        {
            Bpin = "202000000000005";
            var contenedor = UnityConfig.Container;
            PoliticaTransversalBeneficiarioPersistencia = contenedor.Resolve<IPoliticaTransversalBeneficiarioPersistencia>();
            PersistenciaTemporal = contenedor.Resolve<IPersistenciaTemporal>();
            AuditoriaServicio = contenedor.Resolve<IAuditoriaServicios>();
            PoliticaTransversalBeneficiarioServicios = new PoliticaTransversalBeneficiarioServicios(PoliticaTransversalBeneficiarioPersistencia, PersistenciaTemporal, AuditoriaServicio);

            var data = new List<AlmacenamientoTemporal>().AsQueryable();

            var objetoRetornoPoliticaTransversalBeneficiario = "";
            var mockPoliticaTransversalBeneficiario = new Mock<ObjectResult<string>>();
            mockPoliticaTransversalBeneficiario.SetupReturn(objetoRetornoPoliticaTransversalBeneficiario);

            _mockSet.As<IQueryable<AlmacenamientoTemporal>>().Setup(m => m.Provider).Returns(data.Provider);
            _mockSet.As<IQueryable<AlmacenamientoTemporal>>().Setup(m => m.Expression).Returns(data.Expression);
            _mockSet.As<IQueryable<AlmacenamientoTemporal>>().Setup(m => m.ElementType).Returns(data.ElementType);
            _mockSet.As<IQueryable<AlmacenamientoTemporal>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            _mockContext.Setup(m => m.AlmacenamientoTemporal).Returns(_mockSet.Object);
            _mockContext.Setup(mc => mc.UspGetPoliticasTransversalesBeneficiarios_JSON(Bpin)).Returns(mockPoliticaTransversalBeneficiario.Object);

            _mockContextFactory.Setup(mcf => mcf.CrearContextoConConexion(ConfigurationManager.ConnectionStrings["MGAWebContexto"].ConnectionString)).Returns(_mockContext.Object);
        }

        [TestMethod]
        public void PoliticaTransversalBeneficiarioTest()
        {
            PoliticaTransversalBeneficiarioServicios = new PoliticaTransversalBeneficiarioServicios(new PoliticaTransversalBeneficiarioPersistencia(_mockContextFactory.Object), new PersistenciaTemporal(_mockContextFactory.Object), AuditoriaServicio);
            var parametrosConsulta = new ParametrosConsultaDto()
            {
                Bpin = Bpin,
                AccionId = new Guid("D8251078-90BC-4EC3-9496-114F128B694F"),
                InstanciaId = new Guid("D6BD89EF-49AB-4D23-B89D-B730D7A5EDF8")
            };
            var result = PoliticaTransversalBeneficiarioServicios.ObtenerPoliticaTransversalBeneficiario(parametrosConsulta);
            Assert.IsNull(result);
        }

        [TestMethod]
        public void PoliticaTransversalBeneficiarioMockTest()
        {
            PoliticaTransversalBeneficiarioServicios = new PoliticaTransversalBeneficiarioServicios(new PoliticaTransversalBeneficiarioPersistencia(_mockContextFactory.Object), new PersistenciaTemporal(_mockContextFactory.Object), AuditoriaServicio);
            var parametrosConsulta = new ParametrosConsultaDto()
            {
                Bpin = Bpin,
                AccionId = new Guid("D8251078-90BC-4EC3-9496-114F128B694F"),
                InstanciaId = new Guid("D6BD89EF-49AB-4D23-B89D-B730D7A5EDF8")
            };
            var result = PoliticaTransversalBeneficiarioServicios.ObtenerPoliticaTransversalBeneficiario(parametrosConsulta);
            Assert.IsNull(result);
        }

        [TestMethod]
        public void PoliticaTransversalBeneficiarioPreviewTest()
        {
            var result = PoliticaTransversalBeneficiarioServicios.ObtenerPoliticaTransversalBeneficiarioPreview();
            Assert.IsNotNull(result);
        }
    }
}
