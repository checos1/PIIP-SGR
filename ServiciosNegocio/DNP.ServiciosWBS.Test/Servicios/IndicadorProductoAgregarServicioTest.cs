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
    using ServiciosNegocio.Dominio.Dto.IndicadorProductoAgregar;
    using ServiciosNegocio.Test.Mock;
    using ServiciosWBS.Servicios.Implementaciones;
    using ServiciosWBS.Servicios.Implementaciones.Transversales;
    using ServiciosWBS.Servicios.Interfaces.Transversales;
    using Unity;

    [TestClass]
    public class IndicadorProductoAgregarServicioTest
    {

        private IIndicadorProductoAgregarPersistencia IndicadorProductoAgregarPersistencia { get; set; }
        private IPersistenciaTemporal PersistenciaTemporal { get; set; }
        private IAuditoriaServicios AuditoriaServicio { get; set; }
        private IndicadorProductoAgregarServicios IndicadorProductoAgregarServicios { get; set; }

        private string Bpin { get; set; }
        private readonly Mock<MockableDbSetWithExtensions<AlmacenamientoTemporal>> _mockSet = new Mock<MockableDbSetWithExtensions<AlmacenamientoTemporal>>();
        private readonly Mock<MGAWebContexto> _mockContext = new Mock<MGAWebContexto>();
        private readonly Mock<IContextoFactory> _mockContextFactory = new Mock<IContextoFactory>();



        [TestInitialize]
        public void Init()
        {
            Bpin = "2017761220016";
            var contenedor = UnityConfig.Container;
            IndicadorProductoAgregarPersistencia = contenedor.Resolve<IIndicadorProductoAgregarPersistencia>();
            PersistenciaTemporal = contenedor.Resolve<IPersistenciaTemporal>();
            AuditoriaServicio = contenedor.Resolve<IAuditoriaServicios>();
            IndicadorProductoAgregarServicios = new IndicadorProductoAgregarServicios(IndicadorProductoAgregarPersistencia, PersistenciaTemporal, AuditoriaServicio);

            var data = new List<AlmacenamientoTemporal>().AsQueryable();

            var objetoRetornoIndicadorProductoAgregar = new uspGetIndicadorProductoAgregar_Result();
            var mockIndicadorProductoAgregar = new Mock<ObjectResult<uspGetIndicadorProductoAgregar_Result>>();
            mockIndicadorProductoAgregar.SetupReturn(objetoRetornoIndicadorProductoAgregar);

            _mockSet.As<IQueryable<AlmacenamientoTemporal>>().Setup(m => m.Provider).Returns(data.Provider);
            _mockSet.As<IQueryable<AlmacenamientoTemporal>>().Setup(m => m.Expression).Returns(data.Expression);
            _mockSet.As<IQueryable<AlmacenamientoTemporal>>().Setup(m => m.ElementType).Returns(data.ElementType);
            _mockSet.As<IQueryable<AlmacenamientoTemporal>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            _mockContext.Setup(m => m.AlmacenamientoTemporal).Returns(_mockSet.Object);
            _mockContext.Setup(mc => mc.uspGetIndicadorProductoAgregar(Bpin)).Returns(mockIndicadorProductoAgregar.Object);

            _mockContextFactory.Setup(mcf => mcf.CrearContextoConConexion(ConfigurationManager.ConnectionStrings["MGAWebContexto"].ConnectionString)).Returns(_mockContext.Object);
        }



        [TestMethod]
        public void IndicadorProductoAgregarServicios_Guardar_MarcadoComoTemporal_InsertaRegistro()
        {
            IndicadorProductoAgregarServicios = new IndicadorProductoAgregarServicios(new IndicadorProductoAgregarPersistencia(_mockContextFactory.Object), new PersistenciaTemporal(_mockContextFactory.Object), AuditoriaServicio);
            //Escenario: los parametros de insercion son validos y ademas viene marcado como guardado temporal
            var parametrosGuardarIndicadorProductoAgregar = new ParametrosGuardarDto<IndicadorProductoAgregarDto>
            {
                InstanciaId = Guid.NewGuid(),
                AccionId = Guid.NewGuid(),
                Contenido = new IndicadorProductoAgregarDto()
            };

            var parametrosAuditoria = new ParametrosAuditoriaDto();

            //Ejecucion
            IndicadorProductoAgregarServicios.Guardar(parametrosGuardarIndicadorProductoAgregar, parametrosAuditoria, true);
        }


        [TestMethod]
        [ExpectedException(typeof(ServiciosNegocioException), "Parámetro piip-idInstanciaFlujoFlujo no recibido.")]
        public void IndicadorProductoAgregarServicios_ConstruirParametrosGuardado_IdInstanciaFlujoNoEnviado_Excepcion()
        {
            IndicadorProductoAgregarServicios = new IndicadorProductoAgregarServicios(new IndicadorProductoAgregarPersistencia(_mockContextFactory.Object), new PersistenciaTemporal(_mockContextFactory.Object), AuditoriaServicio);
            //Escenario: InstanciaId no enviado
            var contenido = new IndicadorProductoAgregarDto();

            HttpRequestMessage request = new HttpRequestMessage();
            request.Headers.Add("piip-idAccion", Guid.NewGuid().ToString());

            //Ejecucion
            IndicadorProductoAgregarServicios.ConstruirParametrosGuardado(request, contenido);
        }


        [TestMethod]
        [ExpectedException(typeof(ServiciosNegocioException), "Parámetro piip-idAccion no recibido.")]
        public void IndicadorProductoAgregarServicios_Guardar_IdAccionNoEnviado_Excepcion()
        {
            IndicadorProductoAgregarServicios = new IndicadorProductoAgregarServicios(new IndicadorProductoAgregarPersistencia(_mockContextFactory.Object), new PersistenciaTemporal(_mockContextFactory.Object), AuditoriaServicio);
            //Escenario: AccionId no enviado
            var contenido = new IndicadorProductoAgregarDto();

            HttpRequestMessage request = new HttpRequestMessage();
            request.Headers.Add("piip-idInstanciaFlujo", Guid.NewGuid().ToString());

            //Ejecucion
            IndicadorProductoAgregarServicios.ConstruirParametrosGuardado(request, contenido);
        }


        [TestMethod]
        [ExpectedException(typeof(ServiciosNegocioException), "Parámetro contenido no recibido.")]
        public void IndicadorProductoAgregarServicios_Guardar_DtoNoEnviado_Excepcion()
        {
            IndicadorProductoAgregarServicios = new IndicadorProductoAgregarServicios(new IndicadorProductoAgregarPersistencia(_mockContextFactory.Object), new PersistenciaTemporal(_mockContextFactory.Object), AuditoriaServicio);
            //Escenario: Contenido no enviado
            HttpRequestMessage request = new HttpRequestMessage();
            request.Headers.Add("piip-idAccion", Guid.NewGuid().ToString());
            request.Headers.Add("piip-idInstanciaFlujo", Guid.NewGuid().ToString());

            //Ejecucion
            IndicadorProductoAgregarServicios.ConstruirParametrosGuardado(request, null);
        }


        [TestMethod]
        [ExpectedException(typeof(ServiciosNegocioException), "Parámetro piip-idInstanciaFlujo inválido")]
        public void IndicadorProductoAgregarServicios_Guardar_IdInstanciaFlujoConValorInvalido_Excepcion()
        {
            IndicadorProductoAgregarServicios = new IndicadorProductoAgregarServicios(new IndicadorProductoAgregarPersistencia(_mockContextFactory.Object), new PersistenciaTemporal(_mockContextFactory.Object), AuditoriaServicio);
            //Escenario: InstanciaId inválido
            var contenido = new IndicadorProductoAgregarDto();

            HttpRequestMessage request = new HttpRequestMessage();
            request.Headers.Add("piip-idInstanciaFlujo", Guid.Empty.ToString());

            //Ejecucion
            IndicadorProductoAgregarServicios.ConstruirParametrosGuardado(request, contenido);
        }


        [TestMethod]
        [ExpectedException(typeof(ServiciosNegocioException), "Parámetro piip-idAccion inválido")]
        public void IndicadorProductoAgregarServicios_Guardar_IdAccionConValorInvalido_Excepcion()
        {
            IndicadorProductoAgregarServicios = new IndicadorProductoAgregarServicios(new IndicadorProductoAgregarPersistencia(_mockContextFactory.Object), new PersistenciaTemporal(_mockContextFactory.Object), AuditoriaServicio);
            //Escenario: AccionId inválido
            var contenido = new IndicadorProductoAgregarDto();

            HttpRequestMessage request = new HttpRequestMessage();
            request.Headers.Add("piip-idAccion", Guid.Empty.ToString());

            //Ejecucion
            IndicadorProductoAgregarServicios.ConstruirParametrosGuardado(request, contenido);
        }

        [TestMethod]
        public void IndicadorProductoAgregarTest()
        {
            IndicadorProductoAgregarServicios = new IndicadorProductoAgregarServicios(new IndicadorProductoAgregarPersistencia(_mockContextFactory.Object), new PersistenciaTemporal(_mockContextFactory.Object), AuditoriaServicio);
            var parametrosConsulta = new ParametrosConsultaDto()
            {
                Bpin = Bpin,
                AccionId = Guid.NewGuid(),
                InstanciaId = Guid.NewGuid()
            };
            var result = IndicadorProductoAgregarServicios.ObtenerIndicadorProductoAgregar(parametrosConsulta);
            Assert.IsNotNull(result);
        }


        [TestMethod]
        public void IndicadorProductoAgregarMockTest()
        {
            IndicadorProductoAgregarServicios = new IndicadorProductoAgregarServicios(new IndicadorProductoAgregarPersistencia(_mockContextFactory.Object), new PersistenciaTemporal(_mockContextFactory.Object), AuditoriaServicio);
            var parametrosConsulta = new ParametrosConsultaDto()
            {
                Bpin = Bpin,
                AccionId = Guid.NewGuid(),
                InstanciaId = Guid.NewGuid()
            };
            var result = IndicadorProductoAgregarServicios.ObtenerIndicadorProductoAgregar(parametrosConsulta);
            Assert.IsNotNull(result);
        }


        [TestMethod]
        public void IndicadorProductoAgregarPreviewMockTest()
        {
            var result = IndicadorProductoAgregarServicios.ObtenerIndicadorProductoAgregarPreview();
            Assert.IsNotNull(result);
        }


        [TestMethod]
        public void IndicadorProductoAgregarPreviewTest()
        {
            var result = IndicadorProductoAgregarServicios.ObtenerIndicadorProductoAgregarPreview();
            Assert.IsNotNull(result);
        }

    }
}
