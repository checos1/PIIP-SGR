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
    using ServiciosNegocio.Dominio.Dto.ValidarViabilidadCompletarInfo;
    using ServiciosNegocio.Test.Mock;
    using ServiciosWBS.Servicios.Implementaciones;
    using ServiciosWBS.Servicios.Implementaciones.Transversales;
    using ServiciosWBS.Servicios.Interfaces.Transversales;
    using Unity;

    [TestClass]
    public class ValidarViabilidadCompletarInfoServicioTest
    {
        private IValidarViabilidadCompletarInfoPersistencia ValidarViabilidadCompletarInfoPersistencia { get; set; }
        private IPersistenciaTemporal PersistenciaTemporal { get; set; }
        private IAuditoriaServicios AuditoriaServicio { get; set; }
        private ValidarViabilidadCompletarInfoServicios validarViabilidadCompletarInfoServicios { get; set; }

        private string Bpin { get; set; }
        private readonly Mock<MockableDbSetWithExtensions<AlmacenamientoTemporal>> _mockSet = new Mock<MockableDbSetWithExtensions<AlmacenamientoTemporal>>();
        private readonly Mock<MGAWebContexto> _mockContext = new Mock<MGAWebContexto>();
        private readonly Mock<IContextoFactory> _mockContextFactory = new Mock<IContextoFactory>();

        [TestInitialize]
        public void Init()
        {
            Bpin = "2017011000236";
            String usuario = "wvv";
            Guid instanciaId = new Guid();
            Guid accionId = new Guid();

            var contenedor = UnityConfig.Container;
            ValidarViabilidadCompletarInfoPersistencia = contenedor.Resolve<IValidarViabilidadCompletarInfoPersistencia>();
            PersistenciaTemporal = contenedor.Resolve<IPersistenciaTemporal>();
            AuditoriaServicio = contenedor.Resolve<IAuditoriaServicios>();
            validarViabilidadCompletarInfoServicios = new ValidarViabilidadCompletarInfoServicios(ValidarViabilidadCompletarInfoPersistencia, PersistenciaTemporal, AuditoriaServicio);

            var data = new List<AlmacenamientoTemporal>().AsQueryable();

            var objetoRetornoValidarViabilidadCompletarInfo = new uspGetProyectoCompletarInformacion_Result();
            var mockValidarViabilidadCompletarInfo = new Mock<ObjectResult<uspGetProyectoCompletarInformacion_Result>>();
            mockValidarViabilidadCompletarInfo.SetupReturn(objetoRetornoValidarViabilidadCompletarInfo);

            _mockSet.As<IQueryable<AlmacenamientoTemporal>>().Setup(m => m.Provider).Returns(data.Provider);
            _mockSet.As<IQueryable<AlmacenamientoTemporal>>().Setup(m => m.Expression).Returns(data.Expression);
            _mockSet.As<IQueryable<AlmacenamientoTemporal>>().Setup(m => m.ElementType).Returns(data.ElementType);
            _mockSet.As<IQueryable<AlmacenamientoTemporal>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            _mockContext.Setup(m => m.AlmacenamientoTemporal).Returns(_mockSet.Object);
            _mockContext.Setup(mc => mc.uspGetProyectoCompletarInformacion(Bpin, usuario, instanciaId, accionId)).Returns(mockValidarViabilidadCompletarInfo.Object);

            _mockContextFactory.Setup(mcf => mcf.CrearContextoConConexion(ConfigurationManager.ConnectionStrings["MGAWebContexto"].ConnectionString)).Returns(_mockContext.Object);
        }

        [TestMethod]
        public void ValidarViabilidadCompletarInfoServicios_Guardar_MarcadoComoTemporal_InsertaRegistro()
        {
            validarViabilidadCompletarInfoServicios = new ValidarViabilidadCompletarInfoServicios(new ValidarViabilidadCompletarInfoPersistencia(_mockContextFactory.Object), new PersistenciaTemporal(_mockContextFactory.Object), AuditoriaServicio);
            //Escenario: los parametros de insercion son validos y ademas viene marcado como guardado temporal
            var parametrosGuardarProducto = new ParametrosGuardarDto<ValidarViabilidadCompletarInfoDto>
            {
                InstanciaId = Guid.NewGuid(),
                AccionId = Guid.NewGuid(),
                Contenido = new ValidarViabilidadCompletarInfoDto()
            };

            var parametrosAuditoria = new ParametrosAuditoriaDto();
            //Ejecucion
            validarViabilidadCompletarInfoServicios.Guardar(parametrosGuardarProducto, parametrosAuditoria, true);
        }

        [TestMethod]
        public void ValidarViabilidadCompletarInfoMockTest()
        {
            
            var parametrosConsulta = new ParametrosConsultaDto()
            {
                Bpin = Bpin,
                AccionId = Guid.NewGuid(),
                InstanciaId = Guid.NewGuid(),
                FormularioId = Guid.NewGuid(),
                Usuario =  "wvv"
            };
            var result = validarViabilidadCompletarInfoServicios.ObtenerValidarViabilidadCompletarInfo(parametrosConsulta);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void ValidarViabilidadCompletarInfoPreviewMockTest()
        {
            var result = validarViabilidadCompletarInfoServicios.ObtenerValidarViabilidadCompletarInfoPreview();
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void ValidarViabilidadCompletarInfoTest()
        {
            
            var parametrosConsulta = new ParametrosConsultaDto()
            {
                Bpin = Bpin,
                AccionId = Guid.NewGuid(),
                InstanciaId = Guid.NewGuid(),
                FormularioId = Guid.NewGuid(),
                Usuario = "wvv"
            };
            var result = validarViabilidadCompletarInfoServicios.ObtenerValidarViabilidadCompletarInfo(parametrosConsulta);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void ValidarViabilidadCompletarInfoPreviewTest()
        {
            var result = validarViabilidadCompletarInfoServicios.ObtenerValidarViabilidadCompletarInfoPreview();
            Assert.IsNotNull(result);
        }

        [TestMethod]
        [ExpectedException(typeof(ServiciosNegocioException), "Parámetro piip-idInstanciaFlujoFlujo no recibido.")]
        public void _ValidarViabilidadCompletarInfoServicios_ConstruirParametrosGuardado_IdInstanciaFlujoNoEnviado_Excepcion()
        {
            validarViabilidadCompletarInfoServicios = new ValidarViabilidadCompletarInfoServicios(new ValidarViabilidadCompletarInfoPersistencia(_mockContextFactory.Object), new PersistenciaTemporal(_mockContextFactory.Object), AuditoriaServicio);
            //Escenario: InstanciaId no enviado
            var contenido = new ValidarViabilidadCompletarInfoDto();

            HttpRequestMessage request = new HttpRequestMessage();
            request.Headers.Add("piip-idAccion", Guid.NewGuid().ToString());

            //Ejecucion
            validarViabilidadCompletarInfoServicios.ConstruirParametrosGuardado(request, contenido);
        }

        [TestMethod]
        [ExpectedException(typeof(ServiciosNegocioException), "Parámetro piip-idAccion no recibido.")]
        public void _ValidarViabilidadCompletarInfoServicios_Guardar_IdAccionNoEnviado_Excepcion()
        {
            validarViabilidadCompletarInfoServicios = new ValidarViabilidadCompletarInfoServicios(new ValidarViabilidadCompletarInfoPersistencia(_mockContextFactory.Object), new PersistenciaTemporal(_mockContextFactory.Object), AuditoriaServicio);
            //Escenario: AccionId no enviado
            var contenido = new ValidarViabilidadCompletarInfoDto();

            HttpRequestMessage request = new HttpRequestMessage();
            request.Headers.Add("piip-idInstanciaFlujo", Guid.NewGuid().ToString());

            //Ejecucion
            validarViabilidadCompletarInfoServicios.ConstruirParametrosGuardado(request, contenido);
        }

        [TestMethod]
        [ExpectedException(typeof(ServiciosNegocioException), "Parámetro contenido no recibido.")]
        public void _ValidarViabilidadCompletarInfoServicios_Guardar_FocalizacionProyectoDtoNoEnviado_Excepcion()
        {
            validarViabilidadCompletarInfoServicios = new ValidarViabilidadCompletarInfoServicios(new ValidarViabilidadCompletarInfoPersistencia(_mockContextFactory.Object), new PersistenciaTemporal(_mockContextFactory.Object), AuditoriaServicio);
            //Escenario: Contenido no enviado
            HttpRequestMessage request = new HttpRequestMessage();
            request.Headers.Add("piip-idAccion", Guid.NewGuid().ToString());
            request.Headers.Add("piip-idInstanciaFlujo", Guid.NewGuid().ToString());

            //Ejecucion
            validarViabilidadCompletarInfoServicios.ConstruirParametrosGuardado(request, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ServiciosNegocioException), "Parámetro piip-idInstanciaFlujo inválido")]
        public void _ValidarViabilidadCompletarInfoServicios_Guardar_IdInstanciaFlujoConValorInvalido_Excepcion()
        {
            validarViabilidadCompletarInfoServicios = new ValidarViabilidadCompletarInfoServicios(new ValidarViabilidadCompletarInfoPersistencia(_mockContextFactory.Object), new PersistenciaTemporal(_mockContextFactory.Object), AuditoriaServicio);
            //Escenario: InstanciaId inválido
            var contenido = new ValidarViabilidadCompletarInfoDto();

            HttpRequestMessage request = new HttpRequestMessage();
            request.Headers.Add("piip-idInstanciaFlujo", Guid.Empty.ToString());

            //Ejecucion
            validarViabilidadCompletarInfoServicios.ConstruirParametrosGuardado(request, contenido);
        }

        [TestMethod]
        [ExpectedException(typeof(ServiciosNegocioException), "Parámetro piip-idAccion inválido")]
        public void _ValidarViabilidadCompletarInfoServicios_Guardar_IdAccionConValorInvalido_Excepcion()
        {
            validarViabilidadCompletarInfoServicios = new ValidarViabilidadCompletarInfoServicios(new ValidarViabilidadCompletarInfoPersistencia(_mockContextFactory.Object), new PersistenciaTemporal(_mockContextFactory.Object), AuditoriaServicio);
            //Escenario: AccionId inválido
            var contenido = new ValidarViabilidadCompletarInfoDto();

            HttpRequestMessage request = new HttpRequestMessage();
            request.Headers.Add("piip-idAccion", Guid.Empty.ToString());

            //Ejecucion
            validarViabilidadCompletarInfoServicios.ConstruirParametrosGuardado(request, contenido);
        }
    }
}