using System.Collections.Generic;
using DNP.ServiciosNegocio.Persistencia.Interfaces;
using DNP.ServiciosNegocio.Persistencia.Modelo;
using DNP.ServiciosNegocio.Servicios.Implementaciones.FuenteFinanciacion;
using DNP.ServiciosNegocio.Servicios.Interfaces.FuenteFinanciacion;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Configuration;
using System.Data.Entity;
using Unity;

namespace DNP.ServiciosNegocio.Test.Servicios.FuenteFinanciacion
{
    using System;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    using Comunes.Dto;
    using Comunes.Dto.Formulario;
    using Configuracion;
    using Persistencia.Interfaces.Genericos;
    using ServiciosNegocio.Servicios.Interfaces.Transversales;
    using Dominio.Dto.FuenteFinanciacion;
    using Mock;
    using Persistencia.Implementaciones.FuenteFinanciacion;
    using Persistencia.Implementaciones.Genericos;
    using Persistencia.Interfaces.FuenteFinanciacion;
    using ServiciosNegocio.Servicios.Implementaciones.Transversales;
    using DNP.ServiciosNegocio.Dominio.Dto.Focalizacion;

    [TestClass]
    public class FuenteFinanciacionServicioTest
    {
        private string BpinBase { get; set; }
        private IFuenteFinanciacionServicios _fuenteFinanciacionServicios;
        private FuenteFinanciacionProyectoServicio FuenteFinanciacionProyectoServicio { get; set; }
        private readonly Mock<DbSet<ViewFuentesFinanciacion>> _mockSet = new Mock<DbSet<ViewFuentesFinanciacion>>();
        private readonly Mock<MGAWebContexto> _mockContext = new Mock<MGAWebContexto>();
        private readonly Mock<IContextoFactory> _mockContextFactory = new Mock<IContextoFactory>();
        private IFuenteFinanciacionPersistencia FuenteFinanciacionPersistencia { get; set; }
        private IPersistenciaTemporal PersistenciaTemporal { get; set; }
        private IAuditoriaServicios AuditoriaServicio { get; set; }
        private readonly Mock<DbSet<AlmacenamientoTemporal>> _mockSetAlmacenamientoTemporal = new Mock<DbSet<AlmacenamientoTemporal>>();

        [TestInitialize]
        public void Init()
        {
            BpinBase = "2017011000042";
            var contenedor = UnityConfig.Container;
            _fuenteFinanciacionServicios = contenedor.Resolve<IFuenteFinanciacionServicios>();
            FuenteFinanciacionPersistencia = UnityContainerExtensions.Resolve<IFuenteFinanciacionPersistencia>(contenedor);
            PersistenciaTemporal = contenedor.Resolve<IPersistenciaTemporal>();
            AuditoriaServicio = contenedor.Resolve<IAuditoriaServicios>();
            //FuenteFinanciacionProyectoServicio = new FuenteFinanciacionProyectoServicio(new FuenteFinanciacionPersistencia(new ContextoFactory()), PersistenciaTemporal, AuditoriaServicio);
            FuenteFinanciacionProyectoServicio = new FuenteFinanciacionProyectoServicio(FuenteFinanciacionPersistencia, PersistenciaTemporal, AuditoriaServicio);

            var dataAlmacenamientoTemporal = new List<AlmacenamientoTemporal>()
                                           {
                                               new AlmacenamientoTemporal()
                                               {
                                                   AccionId = new Guid("E3C1849C-FE24-4C07-9762-036FA72AF10C"),
                                                   InstanciaId =    new Guid("4C2E62CD-CEAD-48EF-88C6-A50AB5913716"),
                                                   Json = "{\"AcumulaIndicadorPrincipal\": true,\"AlternativaId\" : 2,\"Cantidad\" : 5,\"Complemento\" : \"Prubea\",\"CondicionDeseadaId\" : 4,\"CPCId\" : 5,\r              \"FuenteVerificacion\" : \"PRUEB fuenteVerificacion\",\"IndicadorPrincipal\" : \"PRUEBA INDICADOR PRINCIPAL\",\"Meta\" : 10,\"MetaTotalIndicadorPrincipal\" : 15,\r                \"ProductoIndicador\" :\r[{\r\"AcumulaIndicadorSecundario\" : true,\r\"IndicadorSecundarioId\":2,\r \"IndicadorSecundario\":\"PRUEBA INDICADOR SECUNDARIO\",\r          \"MetaTotalIndicadorSecundario\":10,\r\"UnidadMedidaIndicadorSecundario\":4,\r\"ProductoIndicadorDetalle\":[\r{\r\"DetalleIndicadorProductoId\":4,\r\"MetaVigente\":5,\r\"Vigencia\":4,\r\"RegionalizacionMetas\":[{\r\"AgrupacionId\":2,\r\"DepartamentoId\":1,\r\"FechaCreacion\":\"2018/02/03\",\r\"MunicipioId\":2,\r\"RegionalizacionMetaId\":3,\r\"RegionId\":1,\r\"TotalMetaVigente\":58,\r\"VigenciaRegionalizada\":2\r},{\r\"AgrupacionId\":3,\r\"DepartamentoId\":2,\r\"FechaCreacion\":\"2018/02/03\",\r\"MunicipioId\":1,\r\"RegionalizacionMetaId\":4,\r\"RegionId\":5,\r\"TotalMetaVigente\":3,\r\"VigenciaRegionalizada\":2\r}]\r }]\r\r},{\r\"AcumulaIndicadorSecundario\" : true,\r\"IndicadorSecundarioId\":2,\r\"IndicadorSecundario\":\"PRUEBA INDICADOR SECUNDARIO\",\r\"MetaTotalIndicadorSecundario\":10,\r\"UnidadMedidaIndicadorSecundario\":4,\r\"ProductoIndicadorDetalle\":[\r{\r\"DetalleIndicadorProductoId\":4,\r\"MetaVigente\":5,\r\"Vigencia\":4,\r\"RegionalizacionMetas\":[{\r\"AgrupacionId\":2,\r\"DepartamentoId\":1,\r\"FechaCreacion\":\"2018/02/03\",\r\"MunicipioId\":2,\r\"RegionalizacionMetaId\":3,\r\"RegionId\":1,\r\"TotalMetaVigente\":58,\r\"VigenciaRegionalizada\":2\r},{\r\"AgrupacionId\":3,\r\"DepartamentoId\":2,\r\"FechaCreacion\":\"2018/02/03\",\r\"MunicipioId\":1,\r\"RegionalizacionMetaId\":4,\r\"RegionId\":5,\r\"TotalMetaVigente\":3,\r\"VigenciaRegionalizada\":2\r}]\r}]\r} ]\r}"
                                               }
                                           }.AsQueryable();

            var objetoRetornoFuentes = new uspGetFuentesFinanciacion_Result();
            var objetoRetornoObtenerPoliticasTransversalesAjustes = "";
            var objetoRetornoObtenerPoliticasTransversalesCategorias = "";
            var objetoRetornoObtenerPoliticasTransversalesResumen = "";
            var objetoRetornoObtenerPoliticasCategoriasIndicadores = "";
            var objetoRetornoObtenerCrucePoliticasAjustes = "";
            var objetoRetornoObtenerPoliticasSolicitudConcepto = "";
            var objetoRetornoObtenerDireccionesTecnicasPoliticasFocalizacion = "";
            var objetoRetornoObtenerResumenSolicitudConcepto = "";
            var objetoRetornoObtenerPreguntasEnvioPoliticaSubDireccion = "";

            var mockFuentes = new Mock<ObjectResult<uspGetFuentesFinanciacion_Result>>();
            var mockObtenerPoliticasTransversalesAjustes = new Mock<ObjectResult<string>>();
            var mockObtenerPoliticasTransversalesCategorias = new Mock<ObjectResult<string>>();
            var mockObtenerPoliticasTransversalesResumen = new Mock<ObjectResult<string>>();
            var mockObtenerPoliticasCategoriasIndicadores = new Mock<ObjectResult<string>>();
            var mockObtenerCrucePoliticasAjustes = new Mock<ObjectResult<string>>();
            var mockObtenerPoliticasSolicitudConcepto = new Mock<ObjectResult<string>>();
            var mockObtenerDireccionesTecnicasPoliticasFocalizacion = new Mock<ObjectResult<string>>();
            var mockObtenerResumenSolicitudConcepto = new Mock<ObjectResult<string>>();
            var mockObtenerPreguntasEnvioPoliticaSubDireccion = new Mock<ObjectResult<string>>();

            mockFuentes.SetupReturn(objetoRetornoFuentes);
            mockObtenerPoliticasTransversalesAjustes.SetupReturn(objetoRetornoObtenerPoliticasTransversalesAjustes);
            mockObtenerPoliticasTransversalesCategorias.SetupReturn(objetoRetornoObtenerPoliticasTransversalesCategorias);
            mockObtenerPoliticasTransversalesResumen.SetupReturn(objetoRetornoObtenerPoliticasTransversalesResumen);
            mockObtenerPoliticasCategoriasIndicadores.SetupReturn(objetoRetornoObtenerPoliticasCategoriasIndicadores);
            mockObtenerCrucePoliticasAjustes.SetupReturn(objetoRetornoObtenerCrucePoliticasAjustes);
            mockObtenerPoliticasSolicitudConcepto.SetupReturn(objetoRetornoObtenerPoliticasSolicitudConcepto);
            mockObtenerDireccionesTecnicasPoliticasFocalizacion.SetupReturn(objetoRetornoObtenerDireccionesTecnicasPoliticasFocalizacion);
            mockObtenerResumenSolicitudConcepto.SetupReturn(objetoRetornoObtenerResumenSolicitudConcepto);
            mockObtenerPreguntasEnvioPoliticaSubDireccion.SetupReturn(objetoRetornoObtenerPreguntasEnvioPoliticaSubDireccion);
            mockObtenerPreguntasEnvioPoliticaSubDireccion.SetupReturn(objetoRetornoObtenerPreguntasEnvioPoliticaSubDireccion);

            _mockSetAlmacenamientoTemporal.As<IQueryable<AlmacenamientoTemporal>>().Setup(m => m.Provider).Returns(dataAlmacenamientoTemporal.Provider);
            _mockSetAlmacenamientoTemporal.As<IQueryable<AlmacenamientoTemporal>>().Setup(m => m.Expression).Returns(dataAlmacenamientoTemporal.Expression);
            _mockSetAlmacenamientoTemporal.As<IQueryable<AlmacenamientoTemporal>>().Setup(m => m.ElementType).Returns(dataAlmacenamientoTemporal.ElementType);
            _mockSetAlmacenamientoTemporal.As<IQueryable<AlmacenamientoTemporal>>().Setup(m => m.GetEnumerator()).Returns(dataAlmacenamientoTemporal.GetEnumerator());

            _mockContext.Setup(m => m.AlmacenamientoTemporal).Returns(_mockSetAlmacenamientoTemporal.Object);
            _mockContext.Setup(mc => mc.uspGetFuentesFinanciacion(BpinBase)).Returns(mockFuentes.Object);
            _mockContext.Setup(mc => mc.uspGetPoliticasTransversales_Ajustes_JSON(BpinBase)).Returns(mockObtenerPoliticasTransversalesAjustes.Object);
            _mockContext.Setup(mc => mc.uspGetPoliticasTransversalesCategorias_Ajustes_JSON(BpinBase)).Returns(mockObtenerPoliticasTransversalesCategorias.Object);
            _mockContext.Setup(mc => mc.uspGetPoliticasTransversalesCategorias_Ajustes_Resumen_JSON(BpinBase)).Returns(mockObtenerPoliticasTransversalesCategorias.Object);
            _mockContext.Setup(mc => mc.uspGetPoliticasCategoriasIndicadores_JSON(BpinBase)).Returns(mockObtenerPoliticasCategoriasIndicadores.Object);
            _mockContext.Setup(mc => mc.uspGetCrucePoliticasAjustes_JSON(BpinBase)).Returns(mockObtenerCrucePoliticasAjustes.Object);
            _mockContext.Setup(mc => mc.uspGetPoliticasSolicitudConcepto_JSON(BpinBase)).Returns(mockObtenerPoliticasSolicitudConcepto.Object);
            _mockContext.Setup(mc => mc.UspGetDireccionesTecnicasPoliticas_JSON()).Returns(mockObtenerDireccionesTecnicasPoliticasFocalizacion.Object);
            _mockContext.Setup(mc => mc.uspGetResumenSolicitudConcepto_JSON(BpinBase)).Returns(mockObtenerResumenSolicitudConcepto.Object);
            _mockContext.Setup(mc => mc.uspGetPreguntasEnvioPoliticaSubDireccion(new Guid("4C2E62CD-CEAD-48EF-88C6-A50AB5913716"), 1, "jdelgado", new Guid("418D76AC-F081-4D73-A05A-530CD7C6AFF6"))).Returns(mockObtenerPreguntasEnvioPoliticaSubDireccion.Object);
            _mockContext.Setup(mc => mc.uspPostFocalizacionGuardarPreguntasEnvioPoliticaSubDireccionAjustes(new Guid("4C2E62CD-CEAD-48EF-88C6-A50AB5913716"), 1, "jdelgado", new Guid("418D76AC-F081-4D73-A05A-530CD7C6AFF6"), 1, "1", "Test", 1, 1, new ObjectParameter("errorValidacionNegocio", typeof(string)))).Returns(1);

            _mockContextFactory.Setup(mcf => mcf.CrearContextoConConexion(ConfigurationManager.ConnectionStrings["MGAWebContexto"].ConnectionString)).Returns(_mockContext.Object);
        }

        [TestMethod]
        public void FuenteFinanaciacionProyecto()
        {
            _fuenteFinanciacionServicios = new FuenteFinanciacionProyectoServicio(new FuenteFinanciacionPersistencia(_mockContextFactory.Object), new PersistenciaTemporal(_mockContextFactory.Object), new AuditoriaServicios());
            var result = _fuenteFinanciacionServicios.ObtenerFuenteFinanciacionProyecto(new ParametrosConsultaDto
            {
                Bpin = BpinBase,
                AccionId = new Guid("E3C1849C-FE24-4C07-9762-036FA72AF10C"),
                InstanciaId = new Guid("4C2E62CD-CEAD-48EF-88C6-A50AB5913716")
            });
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void FuenteFinanciacionProyectoPreviewTest()
        {

            var result = _fuenteFinanciacionServicios.ObtenerFuenteFinanciacionProyectoPreview();
            Assert.IsNotNull(result);
            Assert.IsTrue(result.FuentesFinanciacion.Count >= 4);
        }

        [TestMethod]
        public void FuenteFinanciacionProyecto_ObtenerDefinitivo_Test()
        {
            FuenteFinanciacionProyectoServicio = new FuenteFinanciacionProyectoServicio(new FuenteFinanciacionPersistencia(_mockContextFactory.Object), new PersistenciaTemporal(_mockContextFactory.Object), new AuditoriaServicios());
            var parametrosConsulta = new ParametrosConsultaDto()
            {
                AccionId = new Guid("418D76AC-F081-4D73-A05A-530CD7C6AFF6"),
                InstanciaId = new Guid("6A0E8FC3-73CF-46B5-B59C-0F683A30AE1D"),
                Bpin = "2017011000042"
            };

            var resultado = FuenteFinanciacionProyectoServicio.Obtener(parametrosConsulta);
            Assert.IsNotNull(resultado);
        }

        [TestMethod]
        public void FuenteFinanciacionProyecto_Obtener_Test()
        {
            FuenteFinanciacionProyectoServicio = new FuenteFinanciacionProyectoServicio(new FuenteFinanciacionPersistencia(_mockContextFactory.Object), new PersistenciaTemporal(_mockContextFactory.Object), new AuditoriaServicios());
            var parametrosConsulta = new ParametrosConsultaDto()
                                     {
                                         AccionId = new Guid("418D76AC-F081-4D73-A05A-530CD7C6AFF6"),
                                         InstanciaId = new Guid("6A0E8FC3-73CF-46B5-B59C-0F683A30AE1D"),
                                         Bpin = "2017011000042"
                                     };

            var resultado = FuenteFinanciacionProyectoServicio.ObtenerFuenteFinanciacionProyectoDto(parametrosConsulta);
            Assert.IsNotNull(resultado);
        }

        [TestMethod]
        public void FuenteFinanciacionProyecto_GuardarDefinitivo()
        {
            var parametrosGuardar = new ParametrosGuardarDto<ProyectoFuenteFinanciacionDto>()
            {
                AccionId = new Guid("418D76AC-F081-4D73-A05A-530CD7C6AFF6"),
                InstanciaId = new Guid("6A0E8FC3-73CF-46B5-B59C-0F683A30AE1D"),
                Usuario = "jdelgado",
                Contenido = new ProyectoFuenteFinanciacionDto()
                {
                    ValorTotalProyecto = 1000,
                    BPIN = "10001",
                    CR = 1,
                    FuentesFinanciacion = new List<FuenteFinanciacionDto>()
                                                                              {
                                                                                  new FuenteFinanciacionDto()
                                                                                  {
                                                                                      EjecucionId = null,
                                                                                      OtraEntidad = "",
                                                                                      EntidadId = 3,
                                                                                      Vigencia = 2017,
                                                                                      TipoRecursoId = 1,
                                                                                      TipoEntidadId = 1,
                                                                                      Mes = 1,
                                                                                      Obligacion = null,
                                                                                      GrupoRecurso =" PGN",
                                                                                      EtapaId = 1,
                                                                                      Pago = null,
                                                                                      Solicitado = 100,
                                                                                      Compromiso = 100,
                                                                                      ApropiacionVigente = 100,
                                                                                      ApropiacionInicial = 100,
                                                                                      ProgramacionId = 100,
                                                                                      FuenteId = 1
                                                                                  }
                                                                              }
                }

            };

            var parametroAuditoria = new ParametrosAuditoriaDto()
            {
                Usuario = "jdelgado",
                Ip = "localhost"
            };

            FuenteFinanciacionProyectoServicio.Guardar(parametrosGuardar, parametroAuditoria, false);
        }

        [TestMethod]
        public void ObtenerPoliticasTransversalesAjustes_Test()
        {
            FuenteFinanciacionProyectoServicio = new FuenteFinanciacionProyectoServicio(new FuenteFinanciacionPersistencia(_mockContextFactory.Object), new PersistenciaTemporal(_mockContextFactory.Object), new AuditoriaServicios());
            var result = FuenteFinanciacionProyectoServicio.ObtenerPoliticasTransversalesAjustes(BpinBase);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void ObtenerPoliticasTransversalesCategorias_Test()
        {
            FuenteFinanciacionProyectoServicio = new FuenteFinanciacionProyectoServicio(new FuenteFinanciacionPersistencia(_mockContextFactory.Object), new PersistenciaTemporal(_mockContextFactory.Object), new AuditoriaServicios());
            var result = FuenteFinanciacionProyectoServicio.ObtenerPoliticasTransversalesCategorias(BpinBase);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void ObtenerPoliticasTransversalesResumen_Test()
        {
            FuenteFinanciacionProyectoServicio = new FuenteFinanciacionProyectoServicio(new FuenteFinanciacionPersistencia(_mockContextFactory.Object), new PersistenciaTemporal(_mockContextFactory.Object), new AuditoriaServicios());
            var result = FuenteFinanciacionProyectoServicio.ObtenerPoliticasTransversalesResumen(BpinBase);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void ObtenerPoliticasCategoriasIndicadores_Test()
        {
            FuenteFinanciacionProyectoServicio = new FuenteFinanciacionProyectoServicio(new FuenteFinanciacionPersistencia(_mockContextFactory.Object), new PersistenciaTemporal(_mockContextFactory.Object), new AuditoriaServicios());
            var result = FuenteFinanciacionProyectoServicio.ObtenerPoliticasCategoriasIndicadores(BpinBase);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void ObtenerCrucePoliticasAjustes_Test()
        {
            FuenteFinanciacionProyectoServicio = new FuenteFinanciacionProyectoServicio(new FuenteFinanciacionPersistencia(_mockContextFactory.Object), new PersistenciaTemporal(_mockContextFactory.Object), new AuditoriaServicios());
            var result = FuenteFinanciacionProyectoServicio.ObtenerCrucePoliticasAjustes(BpinBase);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void ObtenerPoliticasSolicitudConcepto_Test()
        {
            FuenteFinanciacionProyectoServicio = new FuenteFinanciacionProyectoServicio(new FuenteFinanciacionPersistencia(_mockContextFactory.Object), new PersistenciaTemporal(_mockContextFactory.Object), new AuditoriaServicios());
            var result = FuenteFinanciacionProyectoServicio.ObtenerPoliticasSolicitudConcepto(BpinBase);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void ObtenerDireccionesTecnicasPoliticasFocalizacion_Test()
        {
            FuenteFinanciacionProyectoServicio = new FuenteFinanciacionProyectoServicio(new FuenteFinanciacionPersistencia(_mockContextFactory.Object), new PersistenciaTemporal(_mockContextFactory.Object), new AuditoriaServicios());
            var result = FuenteFinanciacionProyectoServicio.ObtenerDireccionesTecnicasPoliticasFocalizacion();
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void ObtenerResumenSolicitudConcepto_Test()
        {
            FuenteFinanciacionProyectoServicio = new FuenteFinanciacionProyectoServicio(new FuenteFinanciacionPersistencia(_mockContextFactory.Object), new PersistenciaTemporal(_mockContextFactory.Object), new AuditoriaServicios());
            var result = FuenteFinanciacionProyectoServicio.ObtenerResumenSolicitudConcepto(BpinBase);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void ObtenerPreguntasEnvioPoliticaSubDireccion_Test()
        {
            FuenteFinanciacionProyectoServicio = new FuenteFinanciacionProyectoServicio(new FuenteFinanciacionPersistencia(_mockContextFactory.Object), new PersistenciaTemporal(_mockContextFactory.Object), new AuditoriaServicios());
            var instanciaId = new Guid("4C2E62CD-CEAD-48EF-88C6-A50AB5913716");
            int proyectoId = 1;
            string usuarioDNP = "jdelgado";
            var nivelId = new Guid("418D76AC-F081-4D73-A05A-530CD7C6AFF6");
            var result = FuenteFinanciacionProyectoServicio.ObtenerPreguntasEnvioPoliticaSubDireccion(instanciaId, proyectoId, usuarioDNP, nivelId);
            Assert.IsNotNull(result);
        }
    }
}
