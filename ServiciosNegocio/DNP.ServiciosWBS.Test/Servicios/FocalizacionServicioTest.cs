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
    using ServiciosNegocio.Dominio.Dto.FocalizacionProyecto;
    using ServiciosNegocio.Test.Mock;
    using ServiciosWBS.Servicios.Implementaciones;
    using ServiciosWBS.Servicios.Implementaciones.Transversales;
    using ServiciosWBS.Servicios.Interfaces.Transversales;
    using Unity;

    [TestClass]
    public class FocalizacionServicioTest
    {
        private IFocalizacionPersistencia FocalizacionProyectoPersistencia { get; set; }
        private IPersistenciaTemporal PersistenciaTemporal { get; set; }
        private IAuditoriaServicios AuditoriaServicio { get; set; }
        private FocalizacionServicios FocalizacionProyectoServicios { get; set; }

        private string Bpin { get; set; }
        private readonly Mock<MockableDbSetWithExtensions<AlmacenamientoTemporal>> _mockSet = new Mock<MockableDbSetWithExtensions<AlmacenamientoTemporal>>();
        private readonly Mock<MGAWebContexto> _mockContext = new Mock<MGAWebContexto>();
        private readonly Mock<IContextoFactory> _mockContextFactory = new Mock<IContextoFactory>();

        [TestInitialize]
        public void Init()
        {
            Bpin = "2017011000236";
            var contenedor = UnityConfig.Container;
            FocalizacionProyectoPersistencia = contenedor.Resolve<IFocalizacionPersistencia>();
            PersistenciaTemporal = contenedor.Resolve<IPersistenciaTemporal>();
            AuditoriaServicio = contenedor.Resolve<IAuditoriaServicios>();
            FocalizacionProyectoServicios = new FocalizacionServicios(FocalizacionProyectoPersistencia, PersistenciaTemporal, AuditoriaServicio);

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

        [TestMethod]
        public void FocalizacionServicios_Guardar_MarcadoComoTemporal_InsertaRegistro()
        {
            FocalizacionProyectoServicios = new FocalizacionServicios(new FocalizacionPersistencia(_mockContextFactory.Object), new PersistenciaTemporal(_mockContextFactory.Object), AuditoriaServicio);
            //Escenario: los parametros de insercion son validos y ademas viene marcado como guardado temporal
            var parametrosGuardarProducto = new ParametrosGuardarDto<FocalizacionProyectoDto>
            {
                InstanciaId = Guid.NewGuid(),
                AccionId = Guid.NewGuid(),
                Contenido = new FocalizacionProyectoDto()
            };

            var parametrosAuditoria = new ParametrosAuditoriaDto();
            //Ejecucion
            FocalizacionProyectoServicios.Guardar(parametrosGuardarProducto, parametrosAuditoria, true);
        }

        [TestMethod]
        public void FocalizacionMockTest()
        {
            FocalizacionProyectoServicios = new FocalizacionServicios(new FocalizacionPersistencia(_mockContextFactory.Object), new PersistenciaTemporal(_mockContextFactory.Object), AuditoriaServicio);
            var parametrosConsulta = new ParametrosConsultaDto()
            {
                Bpin = Bpin,
                AccionId = Guid.NewGuid(),
                InstanciaId = Guid.NewGuid()
            };
            var result = FocalizacionProyectoServicios.ObtenerProyectoFocalizacion(parametrosConsulta);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void FocalizacionPreviewMockTest()
        {
            var result = FocalizacionProyectoServicios.ObtenerProyectoFocalizacionPreview();
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void FocalizacionTest()
        {
            FocalizacionProyectoServicios = new FocalizacionServicios(new FocalizacionPersistencia(_mockContextFactory.Object), new PersistenciaTemporal(_mockContextFactory.Object), AuditoriaServicio);
            var parametrosConsulta = new ParametrosConsultaDto()
            {
                Bpin = Bpin,
                AccionId = Guid.NewGuid(),
                InstanciaId = Guid.NewGuid()
            };
            var result = FocalizacionProyectoServicios.ObtenerProyectoFocalizacion(parametrosConsulta);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void FocalizacionPreviewTest()
        {
            var result = FocalizacionProyectoServicios.ObtenerProyectoFocalizacionPreview();
            Assert.IsNotNull(result);
        }

        [TestMethod]
        [ExpectedException(typeof(ServiciosNegocioException), "Parámetro piip-idInstanciaFlujoFlujo no recibido.")]
        public void _FocalizacionProyectoServicios_ConstruirParametrosGuardado_IdInstanciaFlujoNoEnviado_Excepcion()
        {
            FocalizacionProyectoServicios = new FocalizacionServicios(new FocalizacionPersistencia(_mockContextFactory.Object), new PersistenciaTemporal(_mockContextFactory.Object), AuditoriaServicio);
            //Escenario: InstanciaId no enviado
            var contenido = new FocalizacionProyectoDto();

            HttpRequestMessage request = new HttpRequestMessage();
            request.Headers.Add("piip-idAccion", Guid.NewGuid().ToString());

            //Ejecucion
            FocalizacionProyectoServicios.ConstruirParametrosGuardado(request, contenido);
        }

        [TestMethod]
        [ExpectedException(typeof(ServiciosNegocioException), "Parámetro piip-idAccion no recibido.")]
        public void _FocalizacionProyectoServicios_Guardar_IdAccionNoEnviado_Excepcion()
        {
            FocalizacionProyectoServicios = new FocalizacionServicios(new FocalizacionPersistencia(_mockContextFactory.Object), new PersistenciaTemporal(_mockContextFactory.Object), AuditoriaServicio);
            //Escenario: AccionId no enviado
            var contenido = new FocalizacionProyectoDto();

            HttpRequestMessage request = new HttpRequestMessage();
            request.Headers.Add("piip-idInstanciaFlujo", Guid.NewGuid().ToString());

            //Ejecucion
            FocalizacionProyectoServicios.ConstruirParametrosGuardado(request, contenido);
        }

        [TestMethod]
        [ExpectedException(typeof(ServiciosNegocioException), "Parámetro contenido no recibido.")]
        public void _FocalizacionProyectoServicios_Guardar_FocalizacionProyectoDtoNoEnviado_Excepcion()
        {
            FocalizacionProyectoServicios = new FocalizacionServicios(new FocalizacionPersistencia(_mockContextFactory.Object), new PersistenciaTemporal(_mockContextFactory.Object), AuditoriaServicio);
            //Escenario: Contenido no enviado
            HttpRequestMessage request = new HttpRequestMessage();
            request.Headers.Add("piip-idAccion", Guid.NewGuid().ToString());
            request.Headers.Add("piip-idInstanciaFlujo", Guid.NewGuid().ToString());

            //Ejecucion
            FocalizacionProyectoServicios.ConstruirParametrosGuardado(request, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ServiciosNegocioException), "Parámetro piip-idInstanciaFlujo inválido")]
        public void _FocalizacionProyectoServicios_Guardar_IdInstanciaFlujoConValorInvalido_Excepcion()
        {
            FocalizacionProyectoServicios = new FocalizacionServicios(new FocalizacionPersistencia(_mockContextFactory.Object), new PersistenciaTemporal(_mockContextFactory.Object), AuditoriaServicio);
            //Escenario: InstanciaId inválido
            var contenido = new FocalizacionProyectoDto();

            HttpRequestMessage request = new HttpRequestMessage();
            request.Headers.Add("piip-idInstanciaFlujo", Guid.Empty.ToString());

            //Ejecucion
            FocalizacionProyectoServicios.ConstruirParametrosGuardado(request, contenido);
        }

        [TestMethod]
        [ExpectedException(typeof(ServiciosNegocioException), "Parámetro piip-idAccion inválido")]
        public void _FocalizacionProyectoServicios_Guardar_IdAccionConValorInvalido_Excepcion()
        {
            FocalizacionProyectoServicios = new FocalizacionServicios(new FocalizacionPersistencia(_mockContextFactory.Object), new PersistenciaTemporal(_mockContextFactory.Object), AuditoriaServicio);
            //Escenario: AccionId inválido
            var contenido = new FocalizacionProyectoDto();

            HttpRequestMessage request = new HttpRequestMessage();
            request.Headers.Add("piip-idAccion", Guid.Empty.ToString());

            //Ejecucion
            FocalizacionProyectoServicios.ConstruirParametrosGuardado(request, contenido);
        }
    }
}