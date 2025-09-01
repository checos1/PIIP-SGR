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
    using ServiciosNegocio.Dominio.Dto.Focalizacion;
    using ServiciosNegocio.Test.Mock;
    using ServiciosWBS.Servicios.Implementaciones;
    using ServiciosWBS.Servicios.Implementaciones.Transversales;
    using ServiciosWBS.Servicios.Interfaces.Transversales;
    using Unity;

    [TestClass]
    public class FocalizacionProyectoServicioTest
    {
        private IFocalizacionProyectoPersistencia FocalizacionProyectoPersistencia { get; set; }
        private IPersistenciaTemporal PersistenciaTemporal { get; set; }
        private IAuditoriaServicios AuditoriaServicio { get; set; }
        private FocalizacionProyectoServicios FocalizacionProyectoServicios { get; set; }

        private string Bpin { get; set; }
        private readonly Mock<MockableDbSetWithExtensions<AlmacenamientoTemporal>> _mockSet = new Mock<MockableDbSetWithExtensions<AlmacenamientoTemporal>>();
        private readonly Mock<MGAWebContexto> _mockContext = new Mock<MGAWebContexto>();
        private readonly Mock<IContextoFactory> _mockContextFactory = new Mock<IContextoFactory>();

        [TestInitialize]
        public void Init()
        {
            Bpin = "2017011000236";
            var contenedor = UnityConfig.Container;
            FocalizacionProyectoPersistencia = contenedor.Resolve<IFocalizacionProyectoPersistencia>();
            PersistenciaTemporal = contenedor.Resolve<IPersistenciaTemporal>();
            AuditoriaServicio = contenedor.Resolve<IAuditoriaServicios>();
            FocalizacionProyectoServicios = new FocalizacionProyectoServicios(FocalizacionProyectoPersistencia, PersistenciaTemporal, AuditoriaServicio);

            var data = new List<AlmacenamientoTemporal>().AsQueryable();

            var objetoRetornoRecursosFocalizacion = new uspGetRecursosFocalizacionVA_Result();
            var mockRecursosFocalizacion = new Mock<ObjectResult<uspGetRecursosFocalizacionVA_Result>>();
            mockRecursosFocalizacion.SetupReturn(objetoRetornoRecursosFocalizacion);

            _mockSet.As<IQueryable<AlmacenamientoTemporal>>().Setup(m => m.Provider).Returns(data.Provider);
            _mockSet.As<IQueryable<AlmacenamientoTemporal>>().Setup(m => m.Expression).Returns(data.Expression);
            _mockSet.As<IQueryable<AlmacenamientoTemporal>>().Setup(m => m.ElementType).Returns(data.ElementType);
            _mockSet.As<IQueryable<AlmacenamientoTemporal>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            _mockContext.Setup(m => m.AlmacenamientoTemporal).Returns(_mockSet.Object);
            _mockContext.Setup(mc => mc.uspGetRecursosFocalizacionVA(Bpin)).Returns(mockRecursosFocalizacion.Object);

            _mockContextFactory.Setup(mcf => mcf.CrearContextoConConexion(ConfigurationManager.ConnectionStrings["MGAWebContexto"].ConnectionString)).Returns(_mockContext.Object);
        }

        [TestMethod]
        public void FocalizacionProyectoServicios_Guardar_MarcadoComoTemporal_InsertaRegistro()
        {
            FocalizacionProyectoServicios = new FocalizacionProyectoServicios(new FocalizacionProyectoPersistencia(_mockContextFactory.Object), new PersistenciaTemporal(_mockContextFactory.Object), AuditoriaServicio);
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
            FocalizacionProyectoServicios = new FocalizacionProyectoServicios(new FocalizacionProyectoPersistencia(_mockContextFactory.Object), new PersistenciaTemporal(_mockContextFactory.Object), AuditoriaServicio);
            var parametrosConsulta = new ParametrosConsultaDto()
            {
                Bpin = Bpin,
                AccionId = Guid.NewGuid(),
                InstanciaId = Guid.NewGuid()
            };
            var result = FocalizacionProyectoServicios.ObtenerFocalizacion(parametrosConsulta);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void FocalizacionPreviewMockTest()
        {
            var result = FocalizacionProyectoServicios.ObtenerFocalizacionPreview();
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void FocalizacionTest()
        {
            FocalizacionProyectoServicios = new FocalizacionProyectoServicios(new FocalizacionProyectoPersistencia(_mockContextFactory.Object), new PersistenciaTemporal(_mockContextFactory.Object), AuditoriaServicio);
            var parametrosConsulta = new ParametrosConsultaDto()
            {
                Bpin = Bpin,
                AccionId = Guid.NewGuid(),
                InstanciaId = Guid.NewGuid()
            };
            var result = FocalizacionProyectoServicios.ObtenerFocalizacion(parametrosConsulta);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void FocalizacionPreviewTest()
        {
            var result = FocalizacionProyectoServicios.ObtenerFocalizacionPreview();
            Assert.IsNotNull(result);
        }

        [TestMethod]
        [ExpectedException(typeof(ServiciosNegocioException), "Parámetro piip-idInstanciaFlujoFlujo no recibido.")]
        public void _FocalizacionProyectoServicios_ConstruirParametrosGuardado_IdInstanciaFlujoNoEnviado_Excepcion()
        {
            FocalizacionProyectoServicios = new FocalizacionProyectoServicios(new FocalizacionProyectoPersistencia(_mockContextFactory.Object), new PersistenciaTemporal(_mockContextFactory.Object), AuditoriaServicio);
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
            FocalizacionProyectoServicios = new FocalizacionProyectoServicios(new FocalizacionProyectoPersistencia(_mockContextFactory.Object), new PersistenciaTemporal(_mockContextFactory.Object), AuditoriaServicio);
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
            FocalizacionProyectoServicios = new FocalizacionProyectoServicios(new FocalizacionProyectoPersistencia(_mockContextFactory.Object), new PersistenciaTemporal(_mockContextFactory.Object), AuditoriaServicio);
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
            FocalizacionProyectoServicios = new FocalizacionProyectoServicios(new FocalizacionProyectoPersistencia(_mockContextFactory.Object), new PersistenciaTemporal(_mockContextFactory.Object), AuditoriaServicio);
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
            FocalizacionProyectoServicios = new FocalizacionProyectoServicios(new FocalizacionProyectoPersistencia(_mockContextFactory.Object), new PersistenciaTemporal(_mockContextFactory.Object), AuditoriaServicio);
            //Escenario: AccionId inválido
            var contenido = new FocalizacionProyectoDto();

            HttpRequestMessage request = new HttpRequestMessage();
            request.Headers.Add("piip-idAccion", Guid.Empty.ToString());

            //Ejecucion
            FocalizacionProyectoServicios.ConstruirParametrosGuardado(request, contenido);
        }
    }
}