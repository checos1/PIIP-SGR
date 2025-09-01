using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DNP.ServiciosNegocio.Test.Servicios.Transversales
{
    using Configuracion;
    using DNP.ServiciosNegocio.Dominio.Dto.Transversales;
    using DNP.ServiciosNegocio.Persistencia.Implementaciones.TramitesProyectos;
    using DNP.ServiciosNegocio.Persistencia.Implementaciones.Transversales;
    using DNP.ServiciosNegocio.Persistencia.Interfaces.TramitesProyectos;
    using DNP.ServiciosNegocio.Persistencia.Interfaces.Transversales;
    using Mock;
    using Moq;
    using Persistencia.Interfaces;
    using Persistencia.Modelo;
    using ServiciosNegocio.Servicios.Implementaciones.Transversales;
    using System;
    using System.Configuration;
    using System.Data.Entity.Core.Objects;
    using System.Threading.Tasks;
    using Unity;

    [TestClass]
    public class CambiosJustificacionHorizonteServicioTest
    {
        private ICambiosJustificacionHorizontePersistencia CambiosJustificacionHorizontePersistencia { get; set; }
        private ISeccionCapituloPersistencia SeccionCapituloPersistencia { get; set; }
        private IFasePersistencia FasePersistencia { get; set; }
        private CambiosJustificacionHorizonteServicio CambiosJustificacionHorizonteServicio { get; set; }

        private readonly Mock<MGAWebContexto> _mockContext = new Mock<MGAWebContexto>();
        private readonly Mock<IContextoFactory> _mockContextFactory = new Mock<IContextoFactory>();

        [TestInitialize]
        public void Init()
        {
            var contenedor = UnityConfig.Container;
            CambiosJustificacionHorizontePersistencia = contenedor.Resolve<ICambiosJustificacionHorizontePersistencia>();
            SeccionCapituloPersistencia = contenedor.Resolve<ISeccionCapituloPersistencia>();
            FasePersistencia = contenedor.Resolve<IFasePersistencia>();
            CambiosJustificacionHorizonteServicio = new CambiosJustificacionHorizonteServicio(CambiosJustificacionHorizontePersistencia, SeccionCapituloPersistencia);
            var objetoRetornoCambiosJustificacionHorizonte = new upsGetEstadoProyectoHorizonte_Result();

            var mockCambiosJustificacionHorizonte = new Mock<ObjectResult<upsGetEstadoProyectoHorizonte_Result>>();

            mockCambiosJustificacionHorizonte.SetupReturn(objetoRetornoCambiosJustificacionHorizonte);

            _mockContext.Setup(mc => mc.upsGetEstadoProyectoHorizonte(1)).Returns(mockCambiosJustificacionHorizonte.Object);
            _mockContext.Setup(mc => mc.uspPostActualizaCapituloModificado(1, "jdelgado", "Test", new Guid("4C2E62CD-CEAD-48EF-88C6-A50AB5913716"), 1, 1, true, true)).Returns(1);

            _mockContextFactory.Setup(mcf => mcf.CrearContextoConConexion(ConfigurationManager.ConnectionStrings["MGAWebContexto"].ConnectionString)).Returns(_mockContext.Object);
        }

        [TestMethod]
        public async Task ObtenerCambiosJustificacionHorizonte_Test()
        {
            CambiosJustificacionHorizonteServicio = new CambiosJustificacionHorizonteServicio(new CambiosJustificacionHorizontePersistencia(_mockContextFactory.Object), new SeccionCapituloPersistencia(_mockContextFactory.Object, FasePersistencia));
            int proyectoId = 1;
            var result = await CambiosJustificacionHorizonteServicio.ObtenerCambiosJustificacionHorizonte(proyectoId);

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task GuardarJustificacionCambios_Test()
        {
            CambiosJustificacionHorizonteServicio = new CambiosJustificacionHorizonteServicio(new CambiosJustificacionHorizontePersistencia(_mockContextFactory.Object), new SeccionCapituloPersistencia(_mockContextFactory.Object, FasePersistencia));
            var parametros = new CapituloModificado
            {
                ProyectoId = 1,
                Justificacion = "Test",
                Usuario = "jdelgado",
                SeccionCapituloId = 1,
                CapituloId = 1,
                SeccionId = 1,
                Modificado = true,
                AplicaJustificacion = 1,
                Cuenta = true,
                InstanciaId = new Guid("4C2E62CD-CEAD-48EF-88C6-A50AB5913716")
            };

            var result = await CambiosJustificacionHorizonteServicio.GuardarJustificacionCambios(parametros, "jdelgado");

            Assert.IsTrue(result);
        }

    }
}
