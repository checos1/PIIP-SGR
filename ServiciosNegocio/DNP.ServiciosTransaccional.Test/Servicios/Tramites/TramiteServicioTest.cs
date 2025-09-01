namespace DNP.ServiciosTransaccional.Test.Servicios.Tramites
{
    using Configuracion;
    using DNP.ServiciosNegocio.Test.Mock;
    using DNP.ServiciosTransaccional.Persistencia.Implementaciones.Proyecto;
    using DNP.ServiciosTransaccional.Persistencia.Implementaciones.Tramites;
    using DNP.ServiciosTransaccional.Persistencia.Interfaces;
    using DNP.ServiciosTransaccional.Persistencia.Interfaces.Tramites;
    using DNP.ServiciosTransaccional.Persistencia.Modelo;
    using DNP.ServiciosTransaccional.Servicios.Implementaciones;
    using DNP.ServiciosTransaccional.Servicios.Implementaciones.Fichas;
    using DNP.ServiciosTransaccional.Servicios.Implementaciones.Tramites;
    using DNP.ServiciosTransaccional.Servicios.Implementaciones.Transversales;
    using DNP.ServiciosTransaccional.Servicios.Interfaces;
    using DNP.ServiciosTransaccional.Servicios.Interfaces.Fichas;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using ServiciosNegocio.Comunes.Dto;
    using ServiciosNegocio.Comunes.Dto.Formulario;
    using ServiciosNegocio.Comunes.Excepciones;
    using ServiciosNegocio.Dominio.Dto.Transferencias;
    using ServiciosTransaccional.Servicios.Interfaces.Tramites;
    using System.Configuration;
    using System.Data.Entity.Core.Objects;
    using Unity;


    [TestClass]
    public class TramiteServicioTest
    {
        private ITramitePersistencia _tramitePersistencia;
        private ClienteHttpServicios _clienteHttpServicios;
        private FichaServicios _fichaServicios;
        private TramiteServicios _tramiteServicio;
        private AuditoriaServicios _auditoriaServicios;
        private readonly Mock<MGAWebContextoTransaccional> _mockContext = new Mock<MGAWebContextoTransaccional>();
        private readonly Mock<IContextoFactory> _mockContextFactory = new Mock<IContextoFactory>();

        [TestInitialize]
        public void Init()
        {
            var contenedor = UnityConfig.Container;
            _tramitePersistencia = contenedor.Resolve<ITramitePersistencia>();
            //_clienteHttpServicios = contenedor.Resolve<IClienteHttpServicios>();
            _clienteHttpServicios = new ClienteHttpServicios();
            _auditoriaServicios = new AuditoriaServicios();
            _fichaServicios = new FichaServicios(_clienteHttpServicios, _auditoriaServicios);
            _tramiteServicio = new TramiteServicios(_clienteHttpServicios, _tramitePersistencia, _fichaServicios);

            var objetoRetornoConsultarCargueExcel = string.Empty;
            var objetoActualizarCargueMasivo = 0;

            var mockConsultarCargueExcel = new Mock<ObjectResult<string>>();
            var mockActualizarCargueMasivo = new Mock<ObjectResult<int>>();

            mockConsultarCargueExcel.SetupReturn(objetoRetornoConsultarCargueExcel);
            mockActualizarCargueMasivo.SetupReturn(objetoActualizarCargueMasivo);

            _mockContext.Setup(mc => mc.uspGetConsultarCargueExcel_JSON("EJ-TP-CM-30101-0003")).Returns(mockConsultarCargueExcel.Object);
            _mockContext.Setup(mc => mc.uspPostActualizarCargueMasivo("EJ-TP-CM-30101-0003", "Pruebas", new ObjectParameter("errorValidacionNegocio", typeof(string)))).Returns(5);
            _mockContextFactory.Setup(mcf => mcf.CrearContextoConConexion(ConfigurationManager.ConnectionStrings["MGAWebContexto"].ConnectionString)).Returns(_mockContext.Object);
        }


        [TestMethod]
        public void ConsultarCargueExcelServicioTest()
        {
            _tramiteServicio = new TramiteServicios(_clienteHttpServicios, new TramitePersistencia(_mockContextFactory.Object), _fichaServicios);
            string numeroProceso = "EJ-TP-CM-30101-0003";
            var resultado = _tramiteServicio.ConsultarCargueExcel(numeroProceso);
            Assert.IsNotNull(resultado);
        }


        [TestMethod]
        public void ActualizarCargueMasivoServicioTest()
        {
            string numeroProceso = "EJ-TP-CM-30101-0003";
            var resultado = _tramiteServicio.ActualizarCargueMasivo(numeroProceso, "Pruebas");
            Assert.IsNotNull(resultado);
        }
    }
}
