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
    using DNP.ServiciosNegocio.Dominio.Dto.Transferencias;

    [TestClass]
    public class DefinirAlcanceServicioTest
    {
        private string Bpin { get; set; }
        private IDefinirAlcancePersistencia DefinirAlcancePersistencia { get; set; }
        private IPersistenciaTemporal PersistenciaTemporal { get; set; }
        private IAuditoriaServicios AuditoriaServicio { get; set; }
        private DefinirAlcanceServicios DefinirAlcanceServicios { get; set; }

        private readonly Mock<MockableDbSetWithExtensions<AlmacenamientoTemporal>> _mockSet = new Mock<MockableDbSetWithExtensions<AlmacenamientoTemporal>>();
        private readonly Mock<MGAWebContexto> _mockContext = new Mock<MGAWebContexto>();
        private readonly Mock<IContextoFactory> _mockContextFactory = new Mock<IContextoFactory>();

        [TestInitialize]
        public void Init()
        {
            Bpin = "202000000000005";
            var contenedor = UnityConfig.Container;
            DefinirAlcancePersistencia = contenedor.Resolve<IDefinirAlcancePersistencia>();
            PersistenciaTemporal = contenedor.Resolve<IPersistenciaTemporal>();
            AuditoriaServicio = contenedor.Resolve<IAuditoriaServicios>();
            DefinirAlcanceServicios = new DefinirAlcanceServicios(DefinirAlcancePersistencia, PersistenciaTemporal, AuditoriaServicio);

            var data = new List<AlmacenamientoTemporal>().AsQueryable();

            var objetoRetornoDefinirAlcance = "{\"ProyectoId\":null,\"BPIN\":\"202000000000005\",\"FechaInicioEtapaInversion\":\"1900-01-01\",\"DescripcionGeneralProyectoNueva\":[{\"FechaInicioDesarrolloProyecto\":\"1900-01-01\",\"Alcance\":\"\"}]}";
            var mockDefinirAlcance = new Mock<ObjectResult<string>>();
            mockDefinirAlcance.SetupReturn(objetoRetornoDefinirAlcance);

            _mockSet.As<IQueryable<AlmacenamientoTemporal>>().Setup(m => m.Provider).Returns(data.Provider);
            _mockSet.As<IQueryable<AlmacenamientoTemporal>>().Setup(m => m.Expression).Returns(data.Expression);
            _mockSet.As<IQueryable<AlmacenamientoTemporal>>().Setup(m => m.ElementType).Returns(data.ElementType);
            _mockSet.As<IQueryable<AlmacenamientoTemporal>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            _mockContext.Setup(m => m.AlmacenamientoTemporal).Returns(_mockSet.Object);
            _mockContext.Setup(mc => mc.UspGetAlcance_JSON(Bpin)).Returns(mockDefinirAlcance.Object);

            _mockContextFactory.Setup(mcf => mcf.CrearContextoConConexion(ConfigurationManager.ConnectionStrings["MGAWebContexto"].ConnectionString)).Returns(_mockContext.Object);
        }

        [TestMethod]
        public void DefinirAlcanceTest()
        {
            DefinirAlcanceServicios = new DefinirAlcanceServicios(new DefinirAlcancePersistencia(_mockContextFactory.Object), new PersistenciaTemporal(_mockContextFactory.Object), AuditoriaServicio);
            var parametrosConsulta = new ParametrosConsultaDto()
            {
                Bpin = Bpin,
                AccionId = new Guid("42D3660A-8B88-4341-8FA9-7C45429C6A9A"),
                InstanciaId = new Guid("A303CA4A-831D-4FA0-A74F-A39BFB962A15")
            };
            var result = DefinirAlcanceServicios.ObtenerDefinirAlcance(parametrosConsulta);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void DefinirAlcanceMockTest()
        {
            DefinirAlcanceServicios = new DefinirAlcanceServicios(new DefinirAlcancePersistencia(_mockContextFactory.Object), new PersistenciaTemporal(_mockContextFactory.Object), AuditoriaServicio);
            var parametrosConsulta = new ParametrosConsultaDto()
            {
                Bpin = Bpin,
                AccionId = new Guid("42D3660A-8B88-4341-8FA9-7C45429C6A9A"),
                InstanciaId = new Guid("A303CA4A-831D-4FA0-A74F-A39BFB962A15")
            };
            var result = DefinirAlcanceServicios.ObtenerDefinirAlcance(parametrosConsulta);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void DefinirAlcancePreviewTest()
        {
            var result = DefinirAlcanceServicios.ObtenerDefinirAlcancePreview();
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GuardarDefinitivo_Test()
        {
            var parametrosGuardar = new ParametrosGuardarDto<AlcanceDto>()
            {
                AccionId = new Guid("418D76AC-F081-4D73-A05A-530CD7C6AFF6"),
                InstanciaId = new Guid("6A0E8FC3-73CF-46B5-B59C-0F683A30AE1D"),
                Usuario = "jdelgado",
                Contenido = new AlcanceDto()
                {
                    ProyectoId = 145896,
                    BPIN = "2017011000042",
                    FechaInicioEtapaInversion = DateTime.Now
                }
            };

            var parametroAuditoria = new ParametrosAuditoriaDto()
            {
                Usuario = "jdelgado",
                Ip = "localhost"
            };

            DefinirAlcanceServicios.Guardar(parametrosGuardar, parametroAuditoria, false);
        }

        [TestMethod]
        public void ObtenerDefinitivo_Test()
        {
            DefinirAlcanceServicios = new DefinirAlcanceServicios(new DefinirAlcancePersistencia(_mockContextFactory.Object), new PersistenciaTemporal(_mockContextFactory.Object), AuditoriaServicio);
            var parametrosConsulta = new ParametrosConsultaDto()
            {
                AccionId = new Guid("418D76AC-F081-4D73-A05A-530CD7C6AFF6"),
                InstanciaId = new Guid("6A0E8FC3-73CF-46B5-B59C-0F683A30AE1D"),
                Bpin = "202000000000005"
            };

            var resultado = DefinirAlcanceServicios.Obtener(parametrosConsulta);
            Assert.IsNotNull(resultado);
        }
    }
}
