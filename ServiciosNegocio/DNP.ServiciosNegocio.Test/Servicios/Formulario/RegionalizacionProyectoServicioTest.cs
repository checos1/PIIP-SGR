using DNP.ServiciosNegocio.Servicios.Implementaciones.Formulario;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Unity;
using DNP.ServiciosNegocio.Test.Configuracion;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Net.Http;
using DNP.ServiciosNegocio.Comunes.Dto;
using DNP.ServiciosNegocio.Comunes.Dto.Formulario;
using DNP.ServiciosNegocio.Comunes.Excepciones;
using DNP.ServiciosNegocio.Dominio.Dto.Formulario;
using Moq;
using DNP.ServiciosNegocio.Persistencia.Interfaces.Formulario;
using DNP.ServiciosNegocio.Persistencia.Interfaces.Genericos;
using DNP.ServiciosNegocio.Persistencia.Interfaces;
using DNP.ServiciosNegocio.Persistencia.Modelo;
using DNP.ServiciosNegocio.Servicios.Interfaces.Transversales;

namespace DNP.ServiciosNegocio.Test.Servicios.Formulario
{
    [TestClass]
    public class RegionalizacionProyectoServicioTest
    {
        private IRegionalizacionProyectoPersistencia RegionalizacionProyectoPersistencia { get; set; }
        private IPersistenciaTemporal PersistenciaTemporal { get; set; }
        private IAuditoriaServicios AuditoriaServicio { get; set; }
        private RegionalizacionProyectoServicios RegionalizacionProyectoServicios { get; set; }

        private string Bpin { get; set; }
        private readonly Mock<DbSet<AlmacenamientoTemporal>> _mockSet = new Mock<DbSet<AlmacenamientoTemporal>>();
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
            _mockSet.As<IQueryable<AlmacenamientoTemporal>>().Setup(m => m.Provider).Returns(data.Provider);
            _mockSet.As<IQueryable<AlmacenamientoTemporal>>().Setup(m => m.Expression).Returns(data.Expression);
            _mockSet.As<IQueryable<AlmacenamientoTemporal>>().Setup(m => m.ElementType).Returns(data.ElementType);
            _mockSet.As<IQueryable<AlmacenamientoTemporal>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
            _mockContext.Setup(m => m.AlmacenamientoTemporal).Returns(_mockSet.Object);
            _mockContextFactory.Setup(mcf => mcf.CrearContextoConConexion(ConfigurationManager.ConnectionStrings["MGAWebContexto"].ConnectionString)).Returns(_mockContext.Object);
        }

        [TestMethod]
        public void RegionalizacionMockTest()
        {
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
            //Escenario: AccionId inválido
            var contenido = new RegionalizacionProyectoDto();

            HttpRequestMessage request = new HttpRequestMessage();
            request.Headers.Add("piip-idAccion", Guid.Empty.ToString());

            //Ejecucion
            RegionalizacionProyectoServicios.ConstruirParametrosGuardado(request, contenido);
        }

        [TestMethod]
        public void RegionalizacionProyectoServicios_Guardar_MarcadoComoTemporal_InsertaRegistro()
        {
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
    }
}