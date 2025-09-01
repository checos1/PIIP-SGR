using System.Collections.Generic;
using DNP.ServiciosNegocio.Persistencia.Interfaces;
using DNP.ServiciosNegocio.Persistencia.Modelo;
using DNP.ServiciosNegocio.Servicios.Implementaciones.Proyectos;
using DNP.ServiciosNegocio.Servicios.Interfaces.Proyectos;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Configuration;
using System.Data.Entity;
using Unity;

namespace DNP.ServiciosNegocio.Test.Servicios.Proyectos
{
    using System;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    using Comunes.Dto;
    using Comunes.Dto.Formulario;
    using Configuracion;
    using Persistencia.Interfaces.Genericos;
    using ServiciosNegocio.Servicios.Interfaces.Transversales;
    using Dominio.Dto.Proyectos;
    using Mock;
    using Persistencia.Implementaciones.Proyectos;
    using Persistencia.Implementaciones.Genericos;
    using Persistencia.Interfaces.Proyectos;
    using ServiciosNegocio.Servicios.Implementaciones.Transversales;
    using DNP.ServiciosNegocio.Test.Mocks;

    [TestClass]
    public class IncluirPoliticasServicioTest
    {
        private string Bpin { get; set; }
        private IIncluirPoliticasPersistencia IncluirPoliticasPersistencia { get; set; }
        private IPersistenciaTemporal PersistenciaTemporal { get; set; }
        private IAuditoriaServicios AuditoriaServicio { get; set; }
        private IncluirPoliticasServicios incluirPoliticasServicios { get; set; }

        private readonly Mock<MockableDbSetWithExtensions<AlmacenamientoTemporal>> _mockSet = new Mock<MockableDbSetWithExtensions<AlmacenamientoTemporal>>();
        private readonly Mock<MGAWebContexto> _mockContext = new Mock<MGAWebContexto>();
        private readonly Mock<IContextoFactory> _mockContextFactory = new Mock<IContextoFactory>();

        [TestInitialize]
        public void Init()
        {
            Bpin = "202000000000005";
            var contenedor = UnityConfig.Container;
            IncluirPoliticasPersistencia = contenedor.Resolve<IIncluirPoliticasPersistencia>();
            PersistenciaTemporal = contenedor.Resolve<IPersistenciaTemporal>();
            AuditoriaServicio = contenedor.Resolve<IAuditoriaServicios>();
            incluirPoliticasServicios = new IncluirPoliticasServicios(IncluirPoliticasPersistencia, PersistenciaTemporal, AuditoriaServicio);

            var data = new List<AlmacenamientoTemporal>().AsQueryable();

            var objetoRetornoIncluirPoliticas = "{\"ProyectoId\":null,\"BPIN\":\"202000000000005\",\"Politicas\":[{\"PoliticaId\":0,\"Politica\":\"\",\"Dimension1Id\":0,\"Dimension1\":\"\",\"Dimension2Id\":0,\"Dimension2\":\"\"}]}";
            var mockIncluirPoliticas = new Mock<ObjectResult<string>>();
            mockIncluirPoliticas.SetupReturn(objetoRetornoIncluirPoliticas);

            _mockSet.As<IQueryable<AlmacenamientoTemporal>>().Setup(m => m.Provider).Returns(data.Provider);
            _mockSet.As<IQueryable<AlmacenamientoTemporal>>().Setup(m => m.Expression).Returns(data.Expression);
            _mockSet.As<IQueryable<AlmacenamientoTemporal>>().Setup(m => m.ElementType).Returns(data.ElementType);
            _mockSet.As<IQueryable<AlmacenamientoTemporal>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            _mockContext.Setup(m => m.AlmacenamientoTemporal).Returns(_mockSet.Object);
            _mockContext.Setup(mc => mc.uspGetAgregarPoliticasTransversalesJerarquia_JSON(Bpin)).Returns(mockIncluirPoliticas.Object);

            _mockContextFactory.Setup(mcf => mcf.CrearContextoConConexion(ConfigurationManager.ConnectionStrings["MGAWebContexto"].ConnectionString)).Returns(_mockContext.Object);
        }

        [TestMethod]
        public void IncluirPoliticasTest()
        {
            incluirPoliticasServicios = new IncluirPoliticasServicios(new IncluirPoliticasPersistencia(_mockContextFactory.Object), new PersistenciaTemporal(_mockContextFactory.Object), AuditoriaServicio);
            var parametrosConsulta = new ParametrosConsultaDto()
            {
                Bpin = Bpin,
                AccionId = new Guid("712FEC8A-B5AD-4F03-8CFF-228846D495BB"),
                InstanciaId = new Guid("C6676732-5896-4B65-9B49-E51B908ED324")
            };
            var result = incluirPoliticasServicios.ObtenerIncluirPoliticas(parametrosConsulta);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void IncluirPoliticasMockTest()
        {
            incluirPoliticasServicios = new IncluirPoliticasServicios(new IncluirPoliticasPersistencia(_mockContextFactory.Object), new PersistenciaTemporal(_mockContextFactory.Object), AuditoriaServicio);
            var parametrosConsulta = new ParametrosConsultaDto()
            {
                Bpin = Bpin,
                AccionId = new Guid("712FEC8A-B5AD-4F03-8CFF-228846D495BB"),
                InstanciaId = new Guid("C6676732-5896-4B65-9B49-E51B908ED324")
            };
            var result = incluirPoliticasServicios.ObtenerIncluirPoliticas(parametrosConsulta);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void IncluirPoliticasPreviewTest()
        {
            var result = incluirPoliticasServicios.ObtenerIncluirPoliticasPreview();
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void ObtenerDefinitivo_Test()
        {
            incluirPoliticasServicios = new IncluirPoliticasServicios(new IncluirPoliticasPersistencia(_mockContextFactory.Object), new PersistenciaTemporal(_mockContextFactory.Object), AuditoriaServicio);
            var parametrosConsulta = new ParametrosConsultaDto()
            {
                AccionId = new Guid("418D76AC-F081-4D73-A05A-530CD7C6AFF6"),
                InstanciaId = new Guid("6A0E8FC3-73CF-46B5-B59C-0F683A30AE1D"),
                Bpin = "202000000000005"
            };

            var resultado = incluirPoliticasServicios.Obtener(parametrosConsulta);
            Assert.IsNotNull(resultado);
        }
    }
}
