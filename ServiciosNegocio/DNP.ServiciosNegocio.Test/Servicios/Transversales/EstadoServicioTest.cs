using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DNP.ServiciosNegocio.Test.Servicios.Transversales
{
    using Configuracion;
    using DNP.ServiciosNegocio.Persistencia.Implementaciones.Transversales;
    using DNP.ServiciosNegocio.Persistencia.Interfaces.Transversales;
    using Mock;
    using Moq;
    using Persistencia.Interfaces;
    using Persistencia.Modelo;
    using ServiciosNegocio.Servicios.Implementaciones.Transversales;
    using System.Configuration;
    using System.Data.Entity.Core.Objects;
    using Unity;

    [TestClass]
    public class EstadoServicioTest
    {
        private IEstadoPersistencia EstadoPersistencia { get; set; }
        private EstadoServicio EstadoServicio { get; set; }

        private readonly Mock<MGAWebContexto> _mockContext = new Mock<MGAWebContexto>();
        private readonly Mock<IContextoFactory> _mockContextFactory = new Mock<IContextoFactory>();

        [TestInitialize]
        public void Init()
        {
            var contenedor = UnityConfig.Container;
            EstadoPersistencia = contenedor.Resolve<IEstadoPersistencia>();
            EstadoServicio = new EstadoServicio(EstadoPersistencia);
            var objetoRetornoEstados = new uspGetEstados_Result();

            var mockEstados = new Mock<ObjectResult<uspGetEstados_Result>>();

            mockEstados.SetupReturn(objetoRetornoEstados);

            _mockContext.Setup(mc => mc.uspGetEstados()).Returns(mockEstados.Object);

            _mockContextFactory.Setup(mcf => mcf.CrearContextoConConexion(ConfigurationManager.ConnectionStrings["MGAWebContexto"].ConnectionString)).Returns(_mockContext.Object);
        }

        [TestMethod]
        public void ConsultarEstados_Test()
        {
            EstadoServicio = new EstadoServicio(new EstadoPersistencia(_mockContextFactory.Object));
            var result = EstadoServicio.ConsultarEstados();

            Assert.IsNotNull(result);
        }

      

    }
}
