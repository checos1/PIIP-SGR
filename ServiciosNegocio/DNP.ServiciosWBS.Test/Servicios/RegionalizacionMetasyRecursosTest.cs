

namespace DNP.ServiciosWBS.Test.Servicios
{
    using Configuracion;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Mocks;
    using Moq;
    using Persistencia.Interfaces;
    using Persistencia.Interfaces.Transversales;
    using Persistencia.Modelo;
    using ServiciosNegocio.Test.Mock;
    using ServiciosWBS.Servicios.Implementaciones;
    using ServiciosWBS.Servicios.Interfaces.Transversales;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    using Unity;




    /// <summary>
    /// Summary description for RegionalizacionMetasyRecursosTest
    /// </summary>
    [TestClass]
    public class RegionalizacionMetasyRecursosTest
    {


        private IPersistenciaTemporal PersistenciaTemporal { get; set; }
        private IAuditoriaServicios AuditoriaServicio { get; set; }
        private IRegionalizacionMetasyRecursosPersistencia _regionalizacionMetasyRecursosPersistencia { get; set; }
        private RegionalizacionMetasyRecursosServicios _regionalizacionMetasyRecursosServicios { get; set; }



        private string Bpin { get; set; }
        private readonly Mock<MockableDbSetWithExtensions<AlmacenamientoTemporal>> _mockSet = new Mock<MockableDbSetWithExtensions<AlmacenamientoTemporal>>();
        private readonly Mock<MGAWebContexto> _mockContext = new Mock<MGAWebContexto>();
        private readonly Mock<IContextoFactory> _mockContextFactory = new Mock<IContextoFactory>();


        [TestInitialize]
        public void Init()
        {
            Bpin = "202000000000005";
            var contenedor = UnityConfig.Container;
            _regionalizacionMetasyRecursosPersistencia = contenedor.Resolve<IRegionalizacionMetasyRecursosPersistencia>();
            PersistenciaTemporal = contenedor.Resolve<IPersistenciaTemporal>();
            AuditoriaServicio = contenedor.Resolve<IAuditoriaServicios>();
            _regionalizacionMetasyRecursosServicios = new RegionalizacionMetasyRecursosServicios(_regionalizacionMetasyRecursosPersistencia, PersistenciaTemporal, AuditoriaServicio);

            var data = new List<AlmacenamientoTemporal>().AsQueryable();

            var objetoRetornoRecursosFocalizacion = new uspGetProyectoFocalizacionVA_Result();
            var mockProyectoFocalizacion = new Mock<ObjectResult<uspGetProyectoFocalizacionVA_Result>>();
            mockProyectoFocalizacion.SetupReturn(objetoRetornoRecursosFocalizacion);

            _mockSet.As<IQueryable<AlmacenamientoTemporal>>().Setup(m => m.Provider).Returns(data.Provider);
            _mockSet.As<IQueryable<AlmacenamientoTemporal>>().Setup(m => m.Expression).Returns(data.Expression);
            _mockSet.As<IQueryable<AlmacenamientoTemporal>>().Setup(m => m.ElementType).Returns(data.ElementType);
            _mockSet.As<IQueryable<AlmacenamientoTemporal>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            _mockContext.Setup(m => m.AlmacenamientoTemporal).Returns(_mockSet.Object);
            _mockContext.Setup(mc => mc.uspGetProyectoFocalizacionVA(Bpin)).Returns(mockProyectoFocalizacion.Object);

            _mockContextFactory.Setup(mcf => mcf.CrearContextoConConexion(ConfigurationManager.ConnectionStrings["MGAWebContexto"].ConnectionString)).Returns(_mockContext.Object);
        }

        public RegionalizacionMetasyRecursosTest()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void TestMethod1()
        {
            //
            // TODO: Add test logic here
            //
        }
    }
}
