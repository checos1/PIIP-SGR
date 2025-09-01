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
    using DNP.ServiciosNegocio.Comunes.Utilidades;

    [TestClass]
    public class FuenteFinanciacionAgregarServicioTest
    {
        private string BpinBase { get; set; }
        private IFuenteFinanciacionAgregarServicio _fuenteFinanciacionAgregarServicio;
        private FuenteFinanciacionAgregarServicio FuenteFinanciacionAgregarServicio { get; set; }
        //private readonly Mock<DbSet<ViewFuentesFinanciacion>> _mockSet = new Mock<DbSet<ViewFuentesFinanciacion>>();
        private readonly Mock<MGAWebContexto> _mockContext = new Mock<MGAWebContexto>();
        private readonly Mock<IContextoFactory> _mockContextFactory = new Mock<IContextoFactory>();
        private IFuenteFinanciacionAgregarPersistencia FuenteFinanciacionAgregarPersistencia { get; set; }
        private IPersistenciaTemporal PersistenciaTemporal { get; set; }
        private IAuditoriaServicios AuditoriaServicio { get; set; }
        private readonly Mock<DbSet<AlmacenamientoTemporal>> _mockSetAlmacenamientoTemporal = new Mock<DbSet<AlmacenamientoTemporal>>();

        [TestInitialize]
        public void Init()
        {
            BpinBase = "2017011000042";
            var contenedor = UnityConfig.Container;
            _fuenteFinanciacionAgregarServicio = contenedor.Resolve<IFuenteFinanciacionAgregarServicio>();
            FuenteFinanciacionAgregarPersistencia = UnityContainerExtensions.Resolve<IFuenteFinanciacionAgregarPersistencia>(contenedor);
            PersistenciaTemporal = contenedor.Resolve<IPersistenciaTemporal>();
            AuditoriaServicio = contenedor.Resolve<IAuditoriaServicios>();
            //FuenteFinanciacionProyectoServicio = new FuenteFinanciacionProyectoServicio(new FuenteFinanciacionPersistencia(new ContextoFactory()), PersistenciaTemporal, AuditoriaServicio);
            FuenteFinanciacionAgregarServicio = new FuenteFinanciacionAgregarServicio(FuenteFinanciacionAgregarPersistencia, PersistenciaTemporal, AuditoriaServicio);

            var dataAlmacenamientoTemporal = new List<AlmacenamientoTemporal>()
                                           {
                                               new AlmacenamientoTemporal()
                                               {
                                                   AccionId = new Guid("E3C1849C-FE24-4C07-9762-036FA72AF10C"),
                                                   InstanciaId =    new Guid("4C2E62CD-CEAD-48EF-88C6-A50AB5913716"),
                                                   Json = "{\"AcumulaIndicadorPrincipal\": true,\"AlternativaId\" : 2,\"Cantidad\" : 5,\"Complemento\" : \"Prubea\",\"CondicionDeseadaId\" : 4,\"CPCId\" : 5,\r              \"FuenteVerificacion\" : \"PRUEB fuenteVerificacion\",\"IndicadorPrincipal\" : \"PRUEBA INDICADOR PRINCIPAL\",\"Meta\" : 10,\"MetaTotalIndicadorPrincipal\" : 15,\r                \"ProductoIndicador\" :\r[{\r\"AcumulaIndicadorSecundario\" : true,\r\"IndicadorSecundarioId\":2,\r \"IndicadorSecundario\":\"PRUEBA INDICADOR SECUNDARIO\",\r          \"MetaTotalIndicadorSecundario\":10,\r\"UnidadMedidaIndicadorSecundario\":4,\r\"ProductoIndicadorDetalle\":[\r{\r\"DetalleIndicadorProductoId\":4,\r\"MetaVigente\":5,\r\"Vigencia\":4,\r\"RegionalizacionMetas\":[{\r\"AgrupacionId\":2,\r\"DepartamentoId\":1,\r\"FechaCreacion\":\"2018/02/03\",\r\"MunicipioId\":2,\r\"RegionalizacionMetaId\":3,\r\"RegionId\":1,\r\"TotalMetaVigente\":58,\r\"VigenciaRegionalizada\":2\r},{\r\"AgrupacionId\":3,\r\"DepartamentoId\":2,\r\"FechaCreacion\":\"2018/02/03\",\r\"MunicipioId\":1,\r\"RegionalizacionMetaId\":4,\r\"RegionId\":5,\r\"TotalMetaVigente\":3,\r\"VigenciaRegionalizada\":2\r}]\r }]\r\r},{\r\"AcumulaIndicadorSecundario\" : true,\r\"IndicadorSecundarioId\":2,\r\"IndicadorSecundario\":\"PRUEBA INDICADOR SECUNDARIO\",\r\"MetaTotalIndicadorSecundario\":10,\r\"UnidadMedidaIndicadorSecundario\":4,\r\"ProductoIndicadorDetalle\":[\r{\r\"DetalleIndicadorProductoId\":4,\r\"MetaVigente\":5,\r\"Vigencia\":4,\r\"RegionalizacionMetas\":[{\r\"AgrupacionId\":2,\r\"DepartamentoId\":1,\r\"FechaCreacion\":\"2018/02/03\",\r\"MunicipioId\":2,\r\"RegionalizacionMetaId\":3,\r\"RegionId\":1,\r\"TotalMetaVigente\":58,\r\"VigenciaRegionalizada\":2\r},{\r\"AgrupacionId\":3,\r\"DepartamentoId\":2,\r\"FechaCreacion\":\"2018/02/03\",\r\"MunicipioId\":1,\r\"RegionalizacionMetaId\":4,\r\"RegionId\":5,\r\"TotalMetaVigente\":3,\r\"VigenciaRegionalizada\":2\r}]\r}]\r} ]\r}"
                                               }
                                           }.AsQueryable();

            var dataGuardarOperacionCredito = new OperacionCreditoDatosGeneralesDto
            {
                ProyectoId = 98104,
                BPIN = "98104",
                Criterios = new List<CriteriosDto>
                {
                    new CriteriosDto
                    {
                        NombreTipoValor = "Test",
                        Habilita = true,
                        Valor = 100000
                    }
                }
            };

            var objetoRetornoFuentes = new uspGetFuenteFinanciacionAgregar_Result();
            var objetoRetornoFuentesN = string.Empty;
            var objetoRetornoFuentesVigencia = string.Empty;
            var objetoRetornoResumenCostosVsSolicitado = string.Empty;
            var objetoRetornoResumenFteFinanciacion = string.Empty;
            var objetoRetornoCostosPIIPvsFuentesPIIP = string.Empty;
            var objetoRetornoDetalleAjustesFuenteFinanciacion = string.Empty;
            var objetoRetornoAjustesJustificaionFacalizacionPT = string.Empty;
            var objetoRetornoOperacionCreditoDatosGenerales = string.Empty;

            var mockFuentes = new Mock<ObjectResult<uspGetFuenteFinanciacionAgregar_Result>>();
            var mockFuentesN = new Mock<ObjectResult<string>>();
            var mockFuenteVigencia = new Mock<ObjectResult<string>>();
            var mockFuenteResumenCostosVsSolicitado = new Mock<ObjectResult<string>>();
            var mockFuenteResumenFteFinanciacion = new Mock<ObjectResult<string>>();
            var mockFuenteCostosPIIPvsFuentesPIIP = new Mock<ObjectResult<string>>();
            var mockDetalleAjustesFuenteFinanciacion = new Mock<ObjectResult<string>>();
            var mockAjustesJustificaionFacalizacionPT = new Mock<ObjectResult<string>>();
            var mockOperacionCreditoDatosGenerales = new Mock<ObjectResult<string>>();

            mockFuentes.SetupReturn(objetoRetornoFuentes);
            mockFuentesN.SetupReturn(objetoRetornoFuentesN);
            mockFuenteVigencia.SetupReturn(objetoRetornoFuentesVigencia);
            mockFuenteResumenCostosVsSolicitado.SetupReturn(objetoRetornoResumenCostosVsSolicitado);
            mockFuenteResumenFteFinanciacion.SetupReturn(objetoRetornoResumenFteFinanciacion);
            mockFuenteCostosPIIPvsFuentesPIIP.SetupReturn(objetoRetornoCostosPIIPvsFuentesPIIP);
            mockDetalleAjustesFuenteFinanciacion.SetupReturn(objetoRetornoDetalleAjustesFuenteFinanciacion);
            mockAjustesJustificaionFacalizacionPT.SetupReturn(objetoRetornoAjustesJustificaionFacalizacionPT);
            mockOperacionCreditoDatosGenerales.SetupReturn(objetoRetornoOperacionCreditoDatosGenerales);

            _mockSetAlmacenamientoTemporal.As<IQueryable<AlmacenamientoTemporal>>().Setup(m => m.Provider).Returns(dataAlmacenamientoTemporal.Provider);
            _mockSetAlmacenamientoTemporal.As<IQueryable<AlmacenamientoTemporal>>().Setup(m => m.Expression).Returns(dataAlmacenamientoTemporal.Expression);
            _mockSetAlmacenamientoTemporal.As<IQueryable<AlmacenamientoTemporal>>().Setup(m => m.ElementType).Returns(dataAlmacenamientoTemporal.ElementType);
            _mockSetAlmacenamientoTemporal.As<IQueryable<AlmacenamientoTemporal>>().Setup(m => m.GetEnumerator()).Returns(dataAlmacenamientoTemporal.GetEnumerator());

            _mockContext.Setup(m => m.AlmacenamientoTemporal).Returns(_mockSetAlmacenamientoTemporal.Object);
            _mockContext.Setup(mc => mc.uspGetFuenteFinanciacionAgregar(BpinBase)).Returns(mockFuentes.Object);
            _mockContext.Setup(mc => mc.UspGetAgregarFuente(BpinBase)).Returns(mockFuentesN.Object);
            _mockContext.Setup(mc => mc.UspGetAgregarFuenteVigencia(BpinBase)).Returns(mockFuenteVigencia.Object);
            _mockContext.Setup(mc => mc.UspGetCostosMgaVsFuentesPiip(BpinBase)).Returns(mockFuenteResumenCostosVsSolicitado.Object);
            _mockContext.Setup(mc => mc.uspGetFuentesTablasResumen_JSON(BpinBase)).Returns(mockFuenteResumenFteFinanciacion.Object);
            _mockContext.Setup(mc => mc.UspGetCostosPIIPVsFuentesPiip_JSON(BpinBase)).Returns(mockFuenteCostosPIIPvsFuentesPIIP.Object);
            _mockContext.Setup(mc => mc.uspGetFuentesFinanciacion_ObtenerDetalleAjuste(BpinBase)).Returns(mockFuenteCostosPIIPvsFuentesPIIP.Object);
            _mockContext.Setup(mc => mc.uspGetPoliticasTransversalesCategorias_ObtenerDetalleAjuste(BpinBase)).Returns(mockFuenteCostosPIIPvsFuentesPIIP.Object);
            _mockContext.Setup(mc => mc.uspGetOperacionesCredito(BpinBase, new Guid("62A02F76-7CF2-4A97-A535-B3C3F1137355"))).Returns(mockFuenteCostosPIIPvsFuentesPIIP.Object);

            _mockContextFactory.Setup(mcf => mcf.CrearContextoConConexion(ConfigurationManager.ConnectionStrings["MGAWebContexto"].ConnectionString)).Returns(_mockContext.Object);
        }

        [TestMethod]
        public void FuenteFinanciacionAgregar()
        {
            _fuenteFinanciacionAgregarServicio = new FuenteFinanciacionAgregarServicio(new FuenteFinanciacionAgregarPersistencia(_mockContextFactory.Object), new PersistenciaTemporal(_mockContextFactory.Object), new AuditoriaServicios());
            var result = _fuenteFinanciacionAgregarServicio.ObtenerFuenteFinanciacionAgregar(new ParametrosConsultaDto
            {
                Bpin = BpinBase,
                AccionId = new Guid("E3C1849C-FE24-4C07-9762-036FA72AF10C"),
                InstanciaId = new Guid("4C2E62CD-CEAD-48EF-88C6-A50AB5913716")
            });
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void FuenteFinanciacionAgregarN()
        {
            _fuenteFinanciacionAgregarServicio = new FuenteFinanciacionAgregarServicio(new FuenteFinanciacionAgregarPersistencia(_mockContextFactory.Object), new PersistenciaTemporal(_mockContextFactory.Object), new AuditoriaServicios());
            var result = _fuenteFinanciacionAgregarServicio.ObtenerFuenteFinanciacionAgregarN(BpinBase);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void FuenteFinanciacionAgregarPreviewTest()
        {

            var result = _fuenteFinanciacionAgregarServicio.ObtenerFuenteFinanciacionAgregarPreview();
            Assert.IsNotNull(result);
            Assert.IsTrue(result.FuentesFinanciacionAgregar.Count >= 2);
        }

        [TestMethod]
        public void FuenteFinanciacionAgregar_ObtenerDefinitivo_Test()
        {
            FuenteFinanciacionAgregarServicio = new FuenteFinanciacionAgregarServicio(new FuenteFinanciacionAgregarPersistencia(_mockContextFactory.Object), new PersistenciaTemporal(_mockContextFactory.Object), new AuditoriaServicios());
            var parametrosConsulta = new ParametrosConsultaDto()
            {
                AccionId = new Guid("418D76AC-F081-4D73-A05A-530CD7C6AFF6"),
                InstanciaId = new Guid("6A0E8FC3-73CF-46B5-B59C-0F683A30AE1D"),
                Bpin = "2017011000042"
            };

            var resultado = FuenteFinanciacionAgregarServicio.Obtener(parametrosConsulta);
            Assert.IsNotNull(resultado);
        }

        [TestMethod]
        public void FuenteFinanciacionAgregar_Obtener_Test()
        {
            FuenteFinanciacionAgregarServicio = new FuenteFinanciacionAgregarServicio(new FuenteFinanciacionAgregarPersistencia(_mockContextFactory.Object), new PersistenciaTemporal(_mockContextFactory.Object), new AuditoriaServicios());
            var parametrosConsulta = new ParametrosConsultaDto()
                                     {
                                         AccionId = new Guid("418D76AC-F081-4D73-A05A-530CD7C6AFF6"),
                                         InstanciaId = new Guid("6A0E8FC3-73CF-46B5-B59C-0F683A30AE1D"),
                                         Bpin = "2017011000042"
                                     };

            var resultado = FuenteFinanciacionAgregarServicio.ObtenerFuenteFinanciacionAgregar(parametrosConsulta);
            Assert.IsNotNull(resultado);
        }

        [TestMethod]
        public void FuenteFinanciacionAgregar_GuardarDefinitivo()
        {
            var parametrosGuardar = new ParametrosGuardarDto<ProyectoFuenteFinanciacionAgregarDto>()
            {
                AccionId = new Guid("418D76AC-F081-4D73-A05A-530CD7C6AFF6"),
                InstanciaId = new Guid("6A0E8FC3-73CF-46B5-B59C-0F683A30AE1D"),
                Usuario = "jdelgado",
                Contenido = new ProyectoFuenteFinanciacionAgregarDto()
                {
                    ProyectoId = 2029,
                    BPIN = "2017011000042",
                    FuentesFinanciacionAgregar = new List<FuenteFinanciacionAgregarDto> {
                    new FuenteFinanciacionAgregarDto()
                    {
                       FuenteId= 105,
                       IdGrupoRecurso= 2,
                       CodigoGrupoRecurso= "2",
                       NombreGrupoRecurso= "Empresa",
                       IdTipoEntidad= 2,
                       CodigoTipoEntidad= "2",
                       NombreTipoEntidad= "Empresas públicas",
                       IdEntidad= null,
                       CodigoEntidad= null,
                       NombreEntidad= "Centro cultural del Oriente Colombiano",
                       IdTipoRecurso= 3,
                       CodigoTipoRecurso= "3",
                       NombreTipoRecurso= "Propios"
                    }
                    }

                }
            

            };

        var parametroAuditoria = new ParametrosAuditoriaDto()
            {
                Usuario = "jdelgado",
                Ip = "localhost"
            };

            FuenteFinanciacionAgregarServicio.Guardar(parametrosGuardar, parametroAuditoria, false);
        }

        [TestMethod]
        public void ObtenerFuenteFinanciacionVigencia_Test()
        {
            _fuenteFinanciacionAgregarServicio = new FuenteFinanciacionAgregarServicio(new FuenteFinanciacionAgregarPersistencia(_mockContextFactory.Object), new PersistenciaTemporal(_mockContextFactory.Object), new AuditoriaServicios());
            var result = _fuenteFinanciacionAgregarServicio.ObtenerFuenteFinanciacionVigencia(BpinBase);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void ObtenerResumenCostosVsSolicitado_Test()
        {
            _fuenteFinanciacionAgregarServicio = new FuenteFinanciacionAgregarServicio(new FuenteFinanciacionAgregarPersistencia(_mockContextFactory.Object), new PersistenciaTemporal(_mockContextFactory.Object), new AuditoriaServicios());
            var result = _fuenteFinanciacionAgregarServicio.ObtenerResumenCostosVsSolicitado(BpinBase);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void ConsultarResumenFteFinanciacion_Test()
        {
            _fuenteFinanciacionAgregarServicio = new FuenteFinanciacionAgregarServicio(new FuenteFinanciacionAgregarPersistencia(_mockContextFactory.Object), new PersistenciaTemporal(_mockContextFactory.Object), new AuditoriaServicios());
            var result = _fuenteFinanciacionAgregarServicio.ConsultarResumenFteFinanciacion(BpinBase);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void ConsultarCostosPIIPvsFuentesPIIP_Test()
        {
            _fuenteFinanciacionAgregarServicio = new FuenteFinanciacionAgregarServicio(new FuenteFinanciacionAgregarPersistencia(_mockContextFactory.Object), new PersistenciaTemporal(_mockContextFactory.Object), new AuditoriaServicios());
            var result = _fuenteFinanciacionAgregarServicio.ConsultarCostosPIIPvsFuentesPIIP(BpinBase);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void ObtenerDetalleAjustesFuenteFinanciacion_Test()
        {
            _fuenteFinanciacionAgregarServicio = new FuenteFinanciacionAgregarServicio(new FuenteFinanciacionAgregarPersistencia(_mockContextFactory.Object), new PersistenciaTemporal(_mockContextFactory.Object), new AuditoriaServicios());
            var result = _fuenteFinanciacionAgregarServicio.ObtenerDetalleAjustesFuenteFinanciacion(BpinBase, "jdelgado");
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void ObtenerDetalleAjustesJustificaionFacalizacionPT_Test()
        {
            _fuenteFinanciacionAgregarServicio = new FuenteFinanciacionAgregarServicio(new FuenteFinanciacionAgregarPersistencia(_mockContextFactory.Object), new PersistenciaTemporal(_mockContextFactory.Object), new AuditoriaServicios());
            var result = _fuenteFinanciacionAgregarServicio.ObtenerDetalleAjustesJustificaionFacalizacionPT(BpinBase, "jdelgado");
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void ObtenerOperacionCreditoDatosGenerales_Test()
        {
            _fuenteFinanciacionAgregarServicio = new FuenteFinanciacionAgregarServicio(new FuenteFinanciacionAgregarPersistencia(_mockContextFactory.Object), new PersistenciaTemporal(_mockContextFactory.Object), new AuditoriaServicios());
            var result = _fuenteFinanciacionAgregarServicio.ObtenerOperacionCreditoDatosGenerales(BpinBase, new Guid("62A02F76-7CF2-4A97-A535-B3C3F1137355"));
            Assert.IsNull(result);
        }

    }
}
