
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
    using ServiciosNegocio.Dominio.Dto.Poblacion;
    using ServiciosNegocio.Test.Mock;
    using ServiciosWBS.Servicios.Implementaciones;
    using ServiciosWBS.Servicios.Implementaciones.Transversales;
    using ServiciosWBS.Servicios.Interfaces.Transversales;
    using Unity;



    [TestClass]
    public class CuantificacionLocalizacionServicioTest
    {

        private ICuantificacionLocalizacionPersistencia CuantificacionLocalizacionPersistencia { get; set; }
        private IPersistenciaTemporal PersistenciaTemporal { get; set; }
        private IAuditoriaServicios AuditoriaServicio { get; set; }
        private CuantificacionLocalizacionServicios CuantificacionLocalizacionServicios { get; set; }

        private string Bpin { get; set; }
        private readonly Mock<MockableDbSetWithExtensions<AlmacenamientoTemporal>> _mockSet = new Mock<MockableDbSetWithExtensions<AlmacenamientoTemporal>>();
        private readonly Mock<MGAWebContexto> _mockContext = new Mock<MGAWebContexto>();
        private readonly Mock<IContextoFactory> _mockContextFactory = new Mock<IContextoFactory>();


        [TestInitialize]
        public void Init()
        {
            Bpin = "2017761220016";
            var contenedor = UnityConfig.Container;
            CuantificacionLocalizacionPersistencia = contenedor.Resolve<ICuantificacionLocalizacionPersistencia>();
            PersistenciaTemporal = contenedor.Resolve<IPersistenciaTemporal>();
            AuditoriaServicio = contenedor.Resolve<IAuditoriaServicios>();
            CuantificacionLocalizacionServicios = new CuantificacionLocalizacionServicios(CuantificacionLocalizacionPersistencia, PersistenciaTemporal, AuditoriaServicio);

            var data = new List<AlmacenamientoTemporal>().AsQueryable();

            var objetoRetornoCuantificacionLocalizacion = new uspGetCuantificacionLocalizacionPoblacion_Result();
            var mockCuantificacionLocalizacion = new Mock<ObjectResult<uspGetCuantificacionLocalizacionPoblacion_Result>>();
            mockCuantificacionLocalizacion.SetupReturn(objetoRetornoCuantificacionLocalizacion);

            _mockSet.As<IQueryable<AlmacenamientoTemporal>>().Setup(m => m.Provider).Returns(data.Provider);
            _mockSet.As<IQueryable<AlmacenamientoTemporal>>().Setup(m => m.Expression).Returns(data.Expression);
            _mockSet.As<IQueryable<AlmacenamientoTemporal>>().Setup(m => m.ElementType).Returns(data.ElementType);
            _mockSet.As<IQueryable<AlmacenamientoTemporal>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            _mockContext.Setup(m => m.AlmacenamientoTemporal).Returns(_mockSet.Object);
            _mockContext.Setup(mc => mc.uspGetCuantificacionLocalizacionPoblacion(Bpin)).Returns(mockCuantificacionLocalizacion.Object);

            _mockContextFactory.Setup(mcf => mcf.CrearContextoConConexion(ConfigurationManager.ConnectionStrings["MGAWebContexto"].ConnectionString)).Returns(_mockContext.Object);
        }


        [TestMethod]
        public void CuantificacionLocalizacionServicios_Guardar_MarcadoComoTemporal_InsertaRegistro()
        {
            CuantificacionLocalizacionServicios = new CuantificacionLocalizacionServicios(new CuantificacionLocalizacionPersistencia(_mockContextFactory.Object), new PersistenciaTemporal(_mockContextFactory.Object), AuditoriaServicio);
            //Escenario: los parametros de insercion son validos y ademas viene marcado como guardado temporal
            var parametrosGuardarCuantificacionLocalizacion = new ParametrosGuardarDto<PoblacionDto>
            {
                InstanciaId = Guid.NewGuid(),
                AccionId = Guid.NewGuid(),
                Contenido = new PoblacionDto()
            };

            var parametrosAuditoria = new ParametrosAuditoriaDto();

            //Ejecucion
            CuantificacionLocalizacionServicios.Guardar(parametrosGuardarCuantificacionLocalizacion, parametrosAuditoria, true);
        }


        [TestMethod]
        [ExpectedException(typeof(ServiciosNegocioException), "Parámetro piip-idInstanciaFlujoFlujo no recibido.")]
        public void CuantificacionLocalizacionServicios_ConstruirParametrosGuardado_IdInstanciaFlujoNoEnviado_Excepcion()
        {
            CuantificacionLocalizacionServicios = new CuantificacionLocalizacionServicios(new CuantificacionLocalizacionPersistencia(_mockContextFactory.Object), new PersistenciaTemporal(_mockContextFactory.Object), AuditoriaServicio);
            //Escenario: InstanciaId no enviado
            var contenido = new PoblacionDto();

            HttpRequestMessage request = new HttpRequestMessage();
            request.Headers.Add("piip-idAccion", Guid.NewGuid().ToString());

            //Ejecucion
            CuantificacionLocalizacionServicios.ConstruirParametrosGuardado(request, contenido);
        }


        [TestMethod]
        [ExpectedException(typeof(ServiciosNegocioException), "Parámetro piip-idAccion no recibido.")]
        public void CuantificacionLocalizacionServicios_Guardar_IdAccionNoEnviado_Excepcion()
        {
            CuantificacionLocalizacionServicios = new CuantificacionLocalizacionServicios(new CuantificacionLocalizacionPersistencia(_mockContextFactory.Object), new PersistenciaTemporal(_mockContextFactory.Object), AuditoriaServicio);
            //Escenario: AccionId no enviado
            var contenido = new PoblacionDto();

            HttpRequestMessage request = new HttpRequestMessage();
            request.Headers.Add("piip-idInstanciaFlujo", Guid.NewGuid().ToString());

            //Ejecucion
            CuantificacionLocalizacionServicios.ConstruirParametrosGuardado(request, contenido);
        }


        [TestMethod]
        [ExpectedException(typeof(ServiciosNegocioException), "Parámetro contenido no recibido.")]
        public void CuantificacionLocalizacionServicios_Guardar_RegionalizacionIndicadoresDtoNoEnviado_Excepcion()
        {
            CuantificacionLocalizacionServicios = new CuantificacionLocalizacionServicios(new CuantificacionLocalizacionPersistencia(_mockContextFactory.Object), new PersistenciaTemporal(_mockContextFactory.Object), AuditoriaServicio);
            //Escenario: Contenido no enviado
            HttpRequestMessage request = new HttpRequestMessage();
            request.Headers.Add("piip-idAccion", Guid.NewGuid().ToString());
            request.Headers.Add("piip-idInstanciaFlujo", Guid.NewGuid().ToString());

            //Ejecucion
            CuantificacionLocalizacionServicios.ConstruirParametrosGuardado(request, null);
        }


        [TestMethod]
        [ExpectedException(typeof(ServiciosNegocioException), "Parámetro piip-idInstanciaFlujo inválido")]
        public void CuantificacionLocalizacionServicios_Guardar_IdInstanciaFlujoConValorInvalido_Excepcion()
        {
            CuantificacionLocalizacionServicios = new CuantificacionLocalizacionServicios(new CuantificacionLocalizacionPersistencia(_mockContextFactory.Object), new PersistenciaTemporal(_mockContextFactory.Object), AuditoriaServicio);
            //Escenario: InstanciaId inválido
            var contenido = new PoblacionDto();

            HttpRequestMessage request = new HttpRequestMessage();
            request.Headers.Add("piip-idInstanciaFlujo", Guid.Empty.ToString());

            //Ejecucion
            CuantificacionLocalizacionServicios.ConstruirParametrosGuardado(request, contenido);
        }


        [TestMethod]
        [ExpectedException(typeof(ServiciosNegocioException), "Parámetro piip-idAccion inválido")]
        public void CuantificacionLocalizacionServicios_Guardar_IdAccionConValorInvalido_Excepcion()
        {
            CuantificacionLocalizacionServicios = new CuantificacionLocalizacionServicios(new CuantificacionLocalizacionPersistencia(_mockContextFactory.Object), new PersistenciaTemporal(_mockContextFactory.Object), AuditoriaServicio);
            //Escenario: AccionId inválido
            var contenido = new PoblacionDto();

            HttpRequestMessage request = new HttpRequestMessage();
            request.Headers.Add("piip-idAccion", Guid.Empty.ToString());

            //Ejecucion
            CuantificacionLocalizacionServicios.ConstruirParametrosGuardado(request, contenido);
        }

        [TestMethod]
        public void CuantificacionLocalizacionTest()
        {
            CuantificacionLocalizacionServicios = new CuantificacionLocalizacionServicios(new CuantificacionLocalizacionPersistencia(_mockContextFactory.Object), new PersistenciaTemporal(_mockContextFactory.Object), AuditoriaServicio);
            var parametrosConsulta = new ParametrosConsultaDto()
            {
                Bpin = Bpin,
                AccionId = Guid.NewGuid(),
                InstanciaId = Guid.NewGuid()
            };
            var result = CuantificacionLocalizacionServicios.ObtenerCuantificacionLocalizacion(parametrosConsulta);
            Assert.IsNotNull(result);
        }


        [TestMethod]
        public void CuantificacionLocalizacionMockTest()
        {
            CuantificacionLocalizacionServicios = new CuantificacionLocalizacionServicios(new CuantificacionLocalizacionPersistencia(_mockContextFactory.Object), new PersistenciaTemporal(_mockContextFactory.Object), AuditoriaServicio);
            var parametrosConsulta = new ParametrosConsultaDto()
            {
                Bpin = Bpin,
                AccionId = Guid.NewGuid(),
                InstanciaId = Guid.NewGuid()
            };
            var result = CuantificacionLocalizacionServicios.ObtenerCuantificacionLocalizacion(parametrosConsulta);
            Assert.IsNotNull(result);
        }


        [TestMethod]
        public void CuantificacionLocalizacionPreviewMockTest()
        {
            var result = CuantificacionLocalizacionServicios.ObtenerCuantificacionLocalizacionPreview();
            Assert.IsNotNull(result);
        }


        [TestMethod]
        public void CuantificacionLocalizacionPreviewTest()
        {
            var result = CuantificacionLocalizacionServicios.ObtenerCuantificacionLocalizacionPreview();
            Assert.IsNotNull(result);
        }


    }
}
