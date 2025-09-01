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
    using ServiciosNegocio.Dominio.Dto.Formulario;
    using ServiciosNegocio.Test.Mock;
    using ServiciosWBS.Servicios.Implementaciones;
    using ServiciosWBS.Servicios.Implementaciones.Transversales;
    using ServiciosWBS.Servicios.Interfaces.Transversales;
    using Unity;

    [TestClass]
    public class RegionalizacionProyectoServicioTest
    {
        private IRegionalizacionProyectoPersistencia RegionalizacionProyectoPersistencia { get; set; }
        private IPersistenciaTemporal PersistenciaTemporal { get; set; }
        private IAuditoriaServicios AuditoriaServicio { get; set; }
        private RegionalizacionProyectoServicios RegionalizacionProyectoServicios { get; set; }

        private string Bpin { get; set; }
        private readonly Mock<MockableDbSetWithExtensions<AlmacenamientoTemporal>> _mockSet = new Mock<MockableDbSetWithExtensions<AlmacenamientoTemporal>>();
        private readonly Mock<MGAWebContexto> _mockContext = new Mock<MGAWebContexto>();
        private readonly Mock<IContextoFactory> _mockContextFactory = new Mock<IContextoFactory>();

        [TestInitialize]
        public void Init()
        {
            Bpin = "2017011000236";
            var contenedor = UnityConfig.Container;
            RegionalizacionProyectoPersistencia = contenedor.Resolve<IRegionalizacionProyectoPersistencia>();
            PersistenciaTemporal = contenedor.Resolve<IPersistenciaTemporal>();
            AuditoriaServicio = contenedor.Resolve<IAuditoriaServicios>();
            RegionalizacionProyectoServicios = new RegionalizacionProyectoServicios(RegionalizacionProyectoPersistencia, PersistenciaTemporal, AuditoriaServicio);

            var data = new List<AlmacenamientoTemporal>().AsQueryable();

            var objetoRetornoRecursosRegionalizacion = new uspGetRecursosRegionalizacion_Result();
            var mockRecursosRegionalizacion = new Mock<ObjectResult<uspGetRecursosRegionalizacion_Result>>();
            mockRecursosRegionalizacion.SetupReturn(objetoRetornoRecursosRegionalizacion);

            _mockSet.As<IQueryable<AlmacenamientoTemporal>>().Setup(m => m.Provider).Returns(data.Provider);
            _mockSet.As<IQueryable<AlmacenamientoTemporal>>().Setup(m => m.Expression).Returns(data.Expression);
            _mockSet.As<IQueryable<AlmacenamientoTemporal>>().Setup(m => m.ElementType).Returns(data.ElementType);
            _mockSet.As<IQueryable<AlmacenamientoTemporal>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            _mockContext.Setup(m => m.AlmacenamientoTemporal).Returns(_mockSet.Object);
            _mockContext.Setup(mc => mc.uspGetRecursosRegionalizacion(Bpin)).Returns(mockRecursosRegionalizacion.Object);

            _mockContextFactory.Setup(mcf => mcf.CrearContextoConConexion(ConfigurationManager.ConnectionStrings["MGAWebContexto"].ConnectionString)).Returns(_mockContext.Object);
        }

        [TestMethod]
        public void RegionalizacionProyectoServicios_Guardar_MarcadoComoTemporal_InsertaRegistro()
        {
            RegionalizacionProyectoServicios = new RegionalizacionProyectoServicios(new RegionalizacionProyectoPersistencia(_mockContextFactory.Object), new PersistenciaTemporal(_mockContextFactory.Object), AuditoriaServicio);
            //Escenario: los parametros de insercion son validos y ademas viene marcado como guardado temporal
            var parametrosGuardarProducto = new ParametrosGuardarDto<RegionalizacionProyectoDto>
            {
                InstanciaId = Guid.NewGuid(),
                AccionId = Guid.NewGuid(),
                Contenido = new RegionalizacionProyectoDto()
            };

            var parametrosAuditoria = new ParametrosAuditoriaDto();

            //Ejecucion
            RegionalizacionProyectoServicios.Guardar(parametrosGuardarProducto, parametrosAuditoria, true);
        }

        [TestMethod]
        public void RegionalizacionMockTest()
        {
            RegionalizacionProyectoServicios = new RegionalizacionProyectoServicios(new RegionalizacionProyectoPersistencia(_mockContextFactory.Object), new PersistenciaTemporal(_mockContextFactory.Object), AuditoriaServicio);
            var parametrosConsulta = new ParametrosConsultaDto()
            {
                Bpin = Bpin,
                AccionId = Guid.NewGuid(),
                InstanciaId = Guid.NewGuid()
            };
            var result = RegionalizacionProyectoServicios.ObtenerRegionalizacion(parametrosConsulta);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void RegionalizacionPreviewMockTest()
        {
            var result = RegionalizacionProyectoServicios.ObtenerRegionalizacionPreview();
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void RegionalizacionTest()
        {
            RegionalizacionProyectoServicios = new RegionalizacionProyectoServicios(new RegionalizacionProyectoPersistencia(_mockContextFactory.Object), new PersistenciaTemporal(_mockContextFactory.Object), AuditoriaServicio);
            var parametrosConsulta = new ParametrosConsultaDto()
            {
                Bpin = Bpin,
                AccionId = Guid.NewGuid(),
                InstanciaId = Guid.NewGuid()
            };
            var result = RegionalizacionProyectoServicios.ObtenerRegionalizacion(parametrosConsulta);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void RegionalizacionPreviewTest()
        {
            var result = RegionalizacionProyectoServicios.ObtenerRegionalizacionPreview();
            Assert.IsNotNull(result);
        }

        [TestMethod]
        [ExpectedException(typeof(ServiciosNegocioException), "Parámetro piip-idInstanciaFlujoFlujo no recibido.")]
        public void _RegionalizacionProyectoServicios_ConstruirParametrosGuardado_IdInstanciaFlujoNoEnviado_Excepcion()
        {
            RegionalizacionProyectoServicios = new RegionalizacionProyectoServicios(new RegionalizacionProyectoPersistencia(_mockContextFactory.Object), new PersistenciaTemporal(_mockContextFactory.Object), AuditoriaServicio);
            //Escenario: InstanciaId no enviado
            var contenido = new RegionalizacionProyectoDto();

            HttpRequestMessage request = new HttpRequestMessage();
            request.Headers.Add("piip-idAccion", Guid.NewGuid().ToString());

            //Ejecucion
            RegionalizacionProyectoServicios.ConstruirParametrosGuardado(request, contenido);
        }

        [TestMethod]
        [ExpectedException(typeof(ServiciosNegocioException), "Parámetro piip-idAccion no recibido.")]
        public void _RegionalizacionProyectoServicios_Guardar_IdAccionNoEnviado_Excepcion()
        {
            RegionalizacionProyectoServicios = new RegionalizacionProyectoServicios(new RegionalizacionProyectoPersistencia(_mockContextFactory.Object), new PersistenciaTemporal(_mockContextFactory.Object), AuditoriaServicio);
            //Escenario: AccionId no enviado
            var contenido = new RegionalizacionProyectoDto();

            HttpRequestMessage request = new HttpRequestMessage();
            request.Headers.Add("piip-idInstanciaFlujo", Guid.NewGuid().ToString());

            //Ejecucion
            RegionalizacionProyectoServicios.ConstruirParametrosGuardado(request, contenido);
        }

        [TestMethod]
        [ExpectedException(typeof(ServiciosNegocioException), "Parámetro contenido no recibido.")]
        public void _RegionalizacionProyectoServicios_Guardar_RegionalizacionProyectoDtoNoEnviado_Excepcion()
        {
            RegionalizacionProyectoServicios = new RegionalizacionProyectoServicios(new RegionalizacionProyectoPersistencia(_mockContextFactory.Object), new PersistenciaTemporal(_mockContextFactory.Object), AuditoriaServicio);
            //Escenario: Contenido no enviado
            HttpRequestMessage request = new HttpRequestMessage();
            request.Headers.Add("piip-idAccion", Guid.NewGuid().ToString());
            request.Headers.Add("piip-idInstanciaFlujo", Guid.NewGuid().ToString());

            //Ejecucion
            RegionalizacionProyectoServicios.ConstruirParametrosGuardado(request, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ServiciosNegocioException), "Parámetro piip-idInstanciaFlujo inválido")]
        public void _RegionalizacionProyectoServicios_Guardar_IdInstanciaFlujoConValorInvalido_Excepcion()
        {
            RegionalizacionProyectoServicios = new RegionalizacionProyectoServicios(new RegionalizacionProyectoPersistencia(_mockContextFactory.Object), new PersistenciaTemporal(_mockContextFactory.Object), AuditoriaServicio);
            //Escenario: InstanciaId inválido
            var contenido = new RegionalizacionProyectoDto();

            HttpRequestMessage request = new HttpRequestMessage();
            request.Headers.Add("piip-idInstanciaFlujo", Guid.Empty.ToString());

            //Ejecucion
            RegionalizacionProyectoServicios.ConstruirParametrosGuardado(request, contenido);
        }

        [TestMethod]
        [ExpectedException(typeof(ServiciosNegocioException), "Parámetro piip-idAccion inválido")]
        public void _RegionalizacionProyectoServicios_Guardar_IdAccionConValorInvalido_Excepcion()
        {
            RegionalizacionProyectoServicios = new RegionalizacionProyectoServicios(new RegionalizacionProyectoPersistencia(_mockContextFactory.Object), new PersistenciaTemporal(_mockContextFactory.Object), AuditoriaServicio);
            //Escenario: AccionId inválido
            var contenido = new RegionalizacionProyectoDto();

            HttpRequestMessage request = new HttpRequestMessage();
            request.Headers.Add("piip-idAccion", Guid.Empty.ToString());

            //Ejecucion
            RegionalizacionProyectoServicios.ConstruirParametrosGuardado(request, contenido);
        }
    }
}