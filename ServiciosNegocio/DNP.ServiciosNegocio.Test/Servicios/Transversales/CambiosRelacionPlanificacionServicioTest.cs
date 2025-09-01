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
    public class CambiosRelacionPlanificacionServicioTest
    {
        private ICambiosRelacionPlanificacionPersistencia CambiosRelacionPlanificacionPersistencia { get; set; }
        private ISeccionCapituloPersistencia SeccionCapituloPersistencia { get; set; }
        private IFasePersistencia FasePersistencia { get; set; }
        private CambiosRelacionPlanificacionServicio CambiosRelacionPlanificacionServicio { get; set; }

        private readonly Mock<MGAWebContexto> _mockContext = new Mock<MGAWebContexto>();
        private readonly Mock<IContextoFactory> _mockContextFactory = new Mock<IContextoFactory>();

        [TestInitialize]
        public void Init()
        {
            var contenedor = UnityConfig.Container;
            CambiosRelacionPlanificacionPersistencia = contenedor.Resolve<ICambiosRelacionPlanificacionPersistencia>();
            SeccionCapituloPersistencia = contenedor.Resolve<ISeccionCapituloPersistencia>();
            FasePersistencia = contenedor.Resolve<IFasePersistencia>();
            CambiosRelacionPlanificacionServicio = new CambiosRelacionPlanificacionServicio(CambiosRelacionPlanificacionPersistencia, SeccionCapituloPersistencia);
            var objetoRetornoCambiosRelacionPlanificacion = new upsGetEstadoProyectoConpes_Result
            {
                ConpesID = 1,
                Estado = "Test",
                NombreConpes = "Test Compes"
            };

            var mockCambiosRelacionPlanificacion = new Mock<ObjectResult<upsGetEstadoProyectoConpes_Result>>();

            mockCambiosRelacionPlanificacion.SetupReturn(objetoRetornoCambiosRelacionPlanificacion);

            _mockContext.Setup(mc => mc.upsGetEstadoProyectoConpes(1)).Returns(mockCambiosRelacionPlanificacion.Object);
            _mockContext.Setup(mc => mc.uspPostActualizaCapituloModificado(1, "jdelgado", "Test", new Guid("4C2E62CD-CEAD-48EF-88C6-A50AB5913716"), 1, 1, true, true)).Returns(1);
            _mockContext.Setup(mc => mc.uspPostFocalizacionActualizaPoliticasModificadas(1, "jdelgado", "Test", new Guid("4C2E62CD-CEAD-48EF-88C6-A50AB5913716"), 1, 1)).Returns(1);

            _mockContextFactory.Setup(mcf => mcf.CrearContextoConConexion(ConfigurationManager.ConnectionStrings["MGAWebContexto"].ConnectionString)).Returns(_mockContext.Object);
        }

        [TestMethod]
        public async Task ConsultarCambiosConpes_Test()
        {
            CambiosRelacionPlanificacionServicio = new CambiosRelacionPlanificacionServicio(new CambiosRelacionPlanificacionPersistencia(_mockContextFactory.Object), new SeccionCapituloPersistencia(_mockContextFactory.Object, FasePersistencia));
            int proyectoId = 1;
            var result = await CambiosRelacionPlanificacionServicio.ConsultarCambiosConpes(proyectoId);

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task GuardarJustificacionCambios_Test()
        {
            CambiosRelacionPlanificacionServicio = new CambiosRelacionPlanificacionServicio(new CambiosRelacionPlanificacionPersistencia(_mockContextFactory.Object), new SeccionCapituloPersistencia(_mockContextFactory.Object, FasePersistencia));
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

            var result = await CambiosRelacionPlanificacionServicio.GuardarJustificacionCambios(parametros, "jdelgado");

            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task FocalizacionActualizaPoliticasModificadas_Test()
        {
            CambiosRelacionPlanificacionServicio = new CambiosRelacionPlanificacionServicio(new CambiosRelacionPlanificacionPersistencia(_mockContextFactory.Object), new SeccionCapituloPersistencia(_mockContextFactory.Object, FasePersistencia));
            var parametros = new JustificacionPoliticaModificada
            {
                ProyectoId = 1,
                Justificacion = "Test",
                Usuario = "jdelgado",
                SeccionCapituloId = 1,
                InstanciaId = new Guid("4C2E62CD-CEAD-48EF-88C6-A50AB5913716"),
                PoliticaId = 1
            };

            var result = await CambiosRelacionPlanificacionServicio.FocalizacionActualizaPoliticasModificadas(parametros, "jdelgado");

            Assert.IsTrue(result);
        }

    }
}
