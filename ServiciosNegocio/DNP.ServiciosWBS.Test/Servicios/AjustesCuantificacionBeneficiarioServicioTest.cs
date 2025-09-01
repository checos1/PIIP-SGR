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
    using ServiciosNegocio.Dominio.Dto.Proyectos;
    using ServiciosNegocio.Test.Mock;
    using ServiciosWBS.Servicios.Implementaciones;
    using ServiciosWBS.Servicios.Implementaciones.Transversales;
    using ServiciosWBS.Servicios.Interfaces.Transversales;
    using Unity;

    [TestClass]
    public class AjustesCuantificacionBeneficiarioServicioTest
    {
        private string Bpin { get; set; }
        private IAjustesCuantificacionBeneficiarioPersistencia AjustesCuantificacionBeneficiarioPersistencia { get; set; }
        private IPersistenciaTemporal PersistenciaTemporal { get; set; }
        private IAuditoriaServicios AuditoriaServicio { get; set; }
        private AjustesCuantificacionBeneficiarioServicios ajustesCuantificacionBeneficiarioServicios { get; set; }

        private readonly Mock<MockableDbSetWithExtensions<AlmacenamientoTemporal>> _mockSet = new Mock<MockableDbSetWithExtensions<AlmacenamientoTemporal>>();
        private readonly Mock<MGAWebContexto> _mockContext = new Mock<MGAWebContexto>();
        private readonly Mock<IContextoFactory> _mockContextFactory = new Mock<IContextoFactory>();

        [TestInitialize]
        public void Init()
        {
            Bpin = "202000000000005";
            var contenedor = UnityConfig.Container;
            AjustesCuantificacionBeneficiarioPersistencia = contenedor.Resolve<IAjustesCuantificacionBeneficiarioPersistencia>();
            PersistenciaTemporal = contenedor.Resolve<IPersistenciaTemporal>();
            AuditoriaServicio = contenedor.Resolve<IAuditoriaServicios>();
            ajustesCuantificacionBeneficiarioServicios = new AjustesCuantificacionBeneficiarioServicios(AjustesCuantificacionBeneficiarioPersistencia, PersistenciaTemporal, AuditoriaServicio);

            var data = new List<AlmacenamientoTemporal>().AsQueryable();

            var objetoRetornoCuantificacionBeneficiario = "";
            var mockCuantificacionBeneficiario = new Mock<ObjectResult<string>>();
            mockCuantificacionBeneficiario.SetupReturn(objetoRetornoCuantificacionBeneficiario);

            _mockSet.As<IQueryable<AlmacenamientoTemporal>>().Setup(m => m.Provider).Returns(data.Provider);
            _mockSet.As<IQueryable<AlmacenamientoTemporal>>().Setup(m => m.Expression).Returns(data.Expression);
            _mockSet.As<IQueryable<AlmacenamientoTemporal>>().Setup(m => m.ElementType).Returns(data.ElementType);
            _mockSet.As<IQueryable<AlmacenamientoTemporal>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            _mockContext.Setup(m => m.AlmacenamientoTemporal).Returns(_mockSet.Object);
            _mockContext.Setup(mc => mc.uspGetCuantificacionBeneficiarios_Ajustar_JSON(Bpin)).Returns(mockCuantificacionBeneficiario.Object);

            _mockContextFactory.Setup(mcf => mcf.CrearContextoConConexion(ConfigurationManager.ConnectionStrings["MGAWebContexto"].ConnectionString)).Returns(_mockContext.Object);
        }

        [TestMethod]
        public void AjustesCuantificacionBeneficiarioTest()
        {
            ajustesCuantificacionBeneficiarioServicios = new AjustesCuantificacionBeneficiarioServicios(new AjustesCuantificacionBeneficiarioPersistencia(_mockContextFactory.Object), new PersistenciaTemporal(_mockContextFactory.Object), AuditoriaServicio);
            var parametrosConsulta = new ParametrosConsultaDto()
            {
                Bpin = Bpin,
                AccionId = new Guid("CD85FEF3-53CB-4ACD-B3F3-B1EE37E13B50"),
                InstanciaId = new Guid("5D81AA9B-E14A-4158-8084-7D48BD39C224")
            };
            var result = ajustesCuantificacionBeneficiarioServicios.ObtenerAjustesCuantificacionBeneficiario(parametrosConsulta);
            Assert.IsNull(result);
        }

        [TestMethod]
        public void AjustesCuantificacionBeneficiarioMockTest()
        {
            ajustesCuantificacionBeneficiarioServicios = new AjustesCuantificacionBeneficiarioServicios(new AjustesCuantificacionBeneficiarioPersistencia(_mockContextFactory.Object), new PersistenciaTemporal(_mockContextFactory.Object), AuditoriaServicio);
            var parametrosConsulta = new ParametrosConsultaDto()
            {
                Bpin = Bpin,
                AccionId = new Guid("CD85FEF3-53CB-4ACD-B3F3-B1EE37E13B50"),
                InstanciaId = new Guid("5D81AA9B-E14A-4158-8084-7D48BD39C224")
            };
            var result = ajustesCuantificacionBeneficiarioServicios.ObtenerAjustesCuantificacionBeneficiario(parametrosConsulta);
            Assert.IsNull(result);
        }

        [TestMethod]
        public void AjustesCuantificacionBeneficiarioPreviewTest()
        {
            var result = ajustesCuantificacionBeneficiarioServicios.ObtenerAjustesCuantificacionBeneficiarioPreview();
            Assert.IsNotNull(result);
        }
    }
}
