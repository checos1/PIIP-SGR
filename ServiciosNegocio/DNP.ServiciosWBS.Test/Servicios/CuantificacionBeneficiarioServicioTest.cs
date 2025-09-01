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
    using ServiciosNegocio.Dominio.Dto.CuantificacionBeneficiario;
    using ServiciosNegocio.Test.Mock;
    using ServiciosWBS.Servicios.Implementaciones;
    using ServiciosWBS.Servicios.Implementaciones.Transversales;
    using ServiciosWBS.Servicios.Interfaces.Transversales;
    using Unity;

    [TestClass]
    public class CuantificacionBeneficiarioServicioTest
    {
        private string Bpin { get; set; }
        private ICuantificacionBeneficiarioPersistencia CuantificacionBeneficiarioPersistencia { get; set; }
        private IPersistenciaTemporal PersistenciaTemporal { get; set; }
        private IAuditoriaServicios AuditoriaServicio { get; set; }
        private CuantificacionBeneficiarioServicios CuantificacionBeneficiarioServicios { get; set; }
        
        private readonly Mock<MockableDbSetWithExtensions<AlmacenamientoTemporal>> _mockSet = new Mock<MockableDbSetWithExtensions<AlmacenamientoTemporal>>();
        private readonly Mock<MGAWebContexto> _mockContext = new Mock<MGAWebContexto>();
        private readonly Mock<IContextoFactory> _mockContextFactory = new Mock<IContextoFactory>();

        [TestInitialize]
        public void Init()
        {
            Bpin = "202000000000005";
            var contenedor = UnityConfig.Container;
            CuantificacionBeneficiarioPersistencia = contenedor.Resolve<ICuantificacionBeneficiarioPersistencia>();
            PersistenciaTemporal = contenedor.Resolve<IPersistenciaTemporal>();
            AuditoriaServicio = contenedor.Resolve<IAuditoriaServicios>();
            CuantificacionBeneficiarioServicios = new CuantificacionBeneficiarioServicios(CuantificacionBeneficiarioPersistencia, PersistenciaTemporal, AuditoriaServicio);

            var data = new List<AlmacenamientoTemporal>().AsQueryable();

            var objetoRetornoCuantificacionBeneficiario = "";
            var mockCuantificacionBeneficiario = new Mock<ObjectResult<string>>();
            mockCuantificacionBeneficiario.SetupReturn(objetoRetornoCuantificacionBeneficiario);

            _mockSet.As<IQueryable<AlmacenamientoTemporal>>().Setup(m => m.Provider).Returns(data.Provider);
            _mockSet.As<IQueryable<AlmacenamientoTemporal>>().Setup(m => m.Expression).Returns(data.Expression);
            _mockSet.As<IQueryable<AlmacenamientoTemporal>>().Setup(m => m.ElementType).Returns(data.ElementType);
            _mockSet.As<IQueryable<AlmacenamientoTemporal>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            _mockContext.Setup(m => m.AlmacenamientoTemporal).Returns(_mockSet.Object);
            _mockContext.Setup(mc => mc.uspGetCuantificacionBeneficiarios_JSON(Bpin)).Returns(mockCuantificacionBeneficiario.Object);

            _mockContextFactory.Setup(mcf => mcf.CrearContextoConConexion(ConfigurationManager.ConnectionStrings["MGAWebContexto"].ConnectionString)).Returns(_mockContext.Object);
        }

        [TestMethod]
        public void CuantificacionBeneficiarioTest()
        {
            CuantificacionBeneficiarioServicios = new CuantificacionBeneficiarioServicios(new CuantificacionBeneficiarioPersistencia(_mockContextFactory.Object), new PersistenciaTemporal(_mockContextFactory.Object), AuditoriaServicio);
            var parametrosConsulta = new ParametrosConsultaDto()
            {
                Bpin = Bpin,
                AccionId = new Guid("48ACE8E7-37DB-BF71-7CE5-4996ED17DEA1"),
                InstanciaId = new Guid("1CC3A855-12F1-4113-A044-014886298AA3")
            };
            var result = CuantificacionBeneficiarioServicios.ObtenerCuantificacionBeneficiario(parametrosConsulta);
            Assert.IsNull(result);
        }

        [TestMethod]
        public void CuantificacionBeneficiarioMockTest()
        {
            CuantificacionBeneficiarioServicios = new CuantificacionBeneficiarioServicios(new CuantificacionBeneficiarioPersistencia(_mockContextFactory.Object), new PersistenciaTemporal(_mockContextFactory.Object), AuditoriaServicio);
            var parametrosConsulta = new ParametrosConsultaDto()
            {
                Bpin = Bpin,
                AccionId = new Guid("48ACE8E7-37DB-BF71-7CE5-4996ED17DEA1"),
                InstanciaId = new Guid("1CC3A855-12F1-4113-A044-014886298AA3")
            };
            var result = CuantificacionBeneficiarioServicios.ObtenerCuantificacionBeneficiario(parametrosConsulta);
            Assert.IsNull(result);
        }

        [TestMethod]
        public void CuantificacionBeneficiarioPreviewTest()
        {
            var result = CuantificacionBeneficiarioServicios.ObtenerCuantificacionBeneficiarioPreview();
            Assert.IsNotNull(result);
        }
    }
}
