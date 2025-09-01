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

    [TestClass]
    public class DatosBasicosSGRServicioTest
    {
        private string BpinBase { get; set; }
        private IDatosBasicosSGRServicio _datosBasicosSGRServicio;
        private DatosBasicosSGRServicio DatosBasicosSGRServicio { get; set; }
        //private readonly Mock<DbSet<ViewFuentesFinanciacion>> _mockSet = new Mock<DbSet<ViewFuentesFinanciacion>>();
        private readonly Mock<MGAWebContexto> _mockContext = new Mock<MGAWebContexto>();
        private readonly Mock<IContextoFactory> _mockContextFactory = new Mock<IContextoFactory>();
        private IDatosBasicosSGRPersistencia DatosBasicosSGRPersistencia { get; set; }
        private IPersistenciaTemporal PersistenciaTemporal { get; set; }
        private IAuditoriaServicios AuditoriaServicio { get; set; }
        private readonly Mock<DbSet<AlmacenamientoTemporal>> _mockSetAlmacenamientoTemporal = new Mock<DbSet<AlmacenamientoTemporal>>();
        private readonly Mock<DbSet<Bienio>> _mockSetBienio = new Mock<DbSet<Bienio>>();


        [TestInitialize]
        public void Init()
        {
            BpinBase = "2017011000042";
            var contenedor = UnityConfig.Container;
            _datosBasicosSGRServicio = contenedor.Resolve<IDatosBasicosSGRServicio>();
            DatosBasicosSGRPersistencia = UnityContainerExtensions.Resolve<IDatosBasicosSGRPersistencia>(contenedor);
            PersistenciaTemporal = contenedor.Resolve<IPersistenciaTemporal>();
            AuditoriaServicio = contenedor.Resolve<IAuditoriaServicios>();
            //FuenteFinanciacionProyectoServicio = new FuenteFinanciacionProyectoServicio(new FuenteFinanciacionPersistencia(new ContextoFactory()), PersistenciaTemporal, AuditoriaServicio);
            DatosBasicosSGRServicio = new DatosBasicosSGRServicio(DatosBasicosSGRPersistencia, PersistenciaTemporal, AuditoriaServicio);

            var dataAlmacenamientoTemporal = new List<AlmacenamientoTemporal>()
                                           {
                                               new AlmacenamientoTemporal()
                                               {
                                                   AccionId = new Guid("E3C1849C-FE24-4C07-9762-036FA72AF10C"),
                                                   InstanciaId =    new Guid("4C2E62CD-CEAD-48EF-88C6-A50AB5913716"),
                                                   Json = "{\"AcumulaIndicadorPrincipal\": true,\"AlternativaId\" : 2,\"Cantidad\" : 5,\"Complemento\" : \"Prubea\",\"CondicionDeseadaId\" : 4,\"CPCId\" : 5,\r              \"FuenteVerificacion\" : \"PRUEB fuenteVerificacion\",\"IndicadorPrincipal\" : \"PRUEBA INDICADOR PRINCIPAL\",\"Meta\" : 10,\"MetaTotalIndicadorPrincipal\" : 15,\r                \"ProductoIndicador\" :\r[{\r\"AcumulaIndicadorSecundario\" : true,\r\"IndicadorSecundarioId\":2,\r \"IndicadorSecundario\":\"PRUEBA INDICADOR SECUNDARIO\",\r          \"MetaTotalIndicadorSecundario\":10,\r\"UnidadMedidaIndicadorSecundario\":4,\r\"ProductoIndicadorDetalle\":[\r{\r\"DetalleIndicadorProductoId\":4,\r\"MetaVigente\":5,\r\"Vigencia\":4,\r\"RegionalizacionMetas\":[{\r\"AgrupacionId\":2,\r\"DepartamentoId\":1,\r\"FechaCreacion\":\"2018/02/03\",\r\"MunicipioId\":2,\r\"RegionalizacionMetaId\":3,\r\"RegionId\":1,\r\"TotalMetaVigente\":58,\r\"VigenciaRegionalizada\":2\r},{\r\"AgrupacionId\":3,\r\"DepartamentoId\":2,\r\"FechaCreacion\":\"2018/02/03\",\r\"MunicipioId\":1,\r\"RegionalizacionMetaId\":4,\r\"RegionId\":5,\r\"TotalMetaVigente\":3,\r\"VigenciaRegionalizada\":2\r}]\r }]\r\r},{\r\"AcumulaIndicadorSecundario\" : true,\r\"IndicadorSecundarioId\":2,\r\"IndicadorSecundario\":\"PRUEBA INDICADOR SECUNDARIO\",\r\"MetaTotalIndicadorSecundario\":10,\r\"UnidadMedidaIndicadorSecundario\":4,\r\"ProductoIndicadorDetalle\":[\r{\r\"DetalleIndicadorProductoId\":4,\r\"MetaVigente\":5,\r\"Vigencia\":4,\r\"RegionalizacionMetas\":[{\r\"AgrupacionId\":2,\r\"DepartamentoId\":1,\r\"FechaCreacion\":\"2018/02/03\",\r\"MunicipioId\":2,\r\"RegionalizacionMetaId\":3,\r\"RegionId\":1,\r\"TotalMetaVigente\":58,\r\"VigenciaRegionalizada\":2\r},{\r\"AgrupacionId\":3,\r\"DepartamentoId\":2,\r\"FechaCreacion\":\"2018/02/03\",\r\"MunicipioId\":1,\r\"RegionalizacionMetaId\":4,\r\"RegionId\":5,\r\"TotalMetaVigente\":3,\r\"VigenciaRegionalizada\":2\r}]\r}]\r} ]\r}"
                                               }
                                           }.AsQueryable();

            var dataBienio = new List<Bienio>()
                                           {
                                               new Bienio()
                                               {
                                                   Id = 1,
                                                   Bienio1 =    "2020",
                                                   EsActivo = true,
                                                   EsActual= true,
                                                   Orden = 1
                                                },
                                                new Bienio()
                                               {
                                                   Id = 2,
                                                   Bienio1 =    "2022",
                                                   EsActivo = true,
                                                   EsActual= false,
                                                   Orden = 2
                                                },
                                                new Bienio()
                                               {
                                                   Id = 3,
                                                   Bienio1 =    "2024",
                                                   EsActivo = true,
                                                   EsActual= false,
                                                   Orden = 3
                                                },
                                                new Bienio()
                                               {
                                                   Id = 4,
                                                   Bienio1 =    "2026",
                                                   EsActivo = true,
                                                   EsActual= false,
                                                   Orden = 4
                                                }
                                           }.AsQueryable();

            var objetoRetornoFuentes = new uspGetDatosBasicosSGR_Result();
            var mockDatosBasicos = new Mock<ObjectResult<uspGetDatosBasicosSGR_Result>>();
            var mockBienio = new Mock<ObjectResult<Bienio>>();
            mockDatosBasicos.SetupReturn(objetoRetornoFuentes);

            _mockSetAlmacenamientoTemporal.As<IQueryable<AlmacenamientoTemporal>>().Setup(m => m.Provider).Returns(dataAlmacenamientoTemporal.Provider);
            _mockSetAlmacenamientoTemporal.As<IQueryable<AlmacenamientoTemporal>>().Setup(m => m.Expression).Returns(dataAlmacenamientoTemporal.Expression);
            _mockSetAlmacenamientoTemporal.As<IQueryable<AlmacenamientoTemporal>>().Setup(m => m.ElementType).Returns(dataAlmacenamientoTemporal.ElementType);
            _mockSetAlmacenamientoTemporal.As<IQueryable<AlmacenamientoTemporal>>().Setup(m => m.GetEnumerator()).Returns(dataAlmacenamientoTemporal.GetEnumerator());

            _mockContext.Setup(m => m.AlmacenamientoTemporal).Returns(_mockSetAlmacenamientoTemporal.Object);
            _mockContext.Setup(mc => mc.uspGetDatosBasicosSGR(BpinBase)).Returns(mockDatosBasicos.Object);

            _mockSetBienio.As<IQueryable<Bienio>>().Setup(m => m.Provider).Returns(dataBienio.Provider);
            _mockSetBienio.As<IQueryable<Bienio>>().Setup(m => m.Expression).Returns(dataBienio.Expression);
            _mockSetBienio.As<IQueryable<Bienio>>().Setup(m => m.ElementType).Returns(dataBienio.ElementType);
            _mockSetBienio.As<IQueryable<Bienio>>().Setup(m => m.GetEnumerator()).Returns(dataBienio.GetEnumerator());

            _mockContext.Setup(m => m.Bienio).Returns(_mockSetBienio.Object);

            _mockContextFactory.Setup(mcf => mcf.CrearContextoConConexion(ConfigurationManager.ConnectionStrings["MGAWebContexto"].ConnectionString)).Returns(_mockContext.Object);
        }

        [TestMethod]
        public void DatosBasicosSGR()
        {
            _datosBasicosSGRServicio = new DatosBasicosSGRServicio(new DatosBasicosSGRPersistencia(_mockContextFactory.Object), new PersistenciaTemporal(_mockContextFactory.Object), new AuditoriaServicios());
            var result = _datosBasicosSGRServicio.ObtenerDatosBasicosSGR(new ParametrosConsultaDto
            {
                Bpin = BpinBase,
                AccionId = new Guid("E3C1849C-FE24-4C07-9762-036FA72AF10C"),
                InstanciaId = new Guid("4C2E62CD-CEAD-48EF-88C6-A50AB5913716")
            });
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void DatosBasicosSGRPreviewTest()
        {

            var result = _datosBasicosSGRServicio.ObtenerDatosBasicosSGRPreview();
            Assert.IsNotNull(result);
            Assert.IsTrue(result.FuentesInterventoria.Count >= 1);
        }

        [TestMethod]
        public void DatosBasicosSGR_ObtenerDefinitivo_Test()
        {
            DatosBasicosSGRServicio = new DatosBasicosSGRServicio(new DatosBasicosSGRPersistencia(_mockContextFactory.Object), new PersistenciaTemporal(_mockContextFactory.Object), new AuditoriaServicios());
            var parametrosConsulta = new ParametrosConsultaDto()
            {
                AccionId = new Guid("418D76AC-F081-4D73-A05A-530CD7C6AFF6"),
                InstanciaId = new Guid("6A0E8FC3-73CF-46B5-B59C-0F683A30AE1D"),
                Bpin = "2017011000042"
            };

            var resultado = DatosBasicosSGRServicio.Obtener(parametrosConsulta);
            Assert.IsNotNull(resultado);
        }

        [TestMethod]
        public void DatosBasicosSGR_Obtener_Test()
        {
            DatosBasicosSGRServicio = new DatosBasicosSGRServicio(new DatosBasicosSGRPersistencia(_mockContextFactory.Object), new PersistenciaTemporal(_mockContextFactory.Object), new AuditoriaServicios());
            var parametrosConsulta = new ParametrosConsultaDto()
            {
                AccionId = new Guid("418D76AC-F081-4D73-A05A-530CD7C6AFF6"),
                InstanciaId = new Guid("6A0E8FC3-73CF-46B5-B59C-0F683A30AE1D"),
                Bpin = "2017011000042"
            };

            var resultado = DatosBasicosSGRServicio.ObtenerDatosBasicosSGR(parametrosConsulta);
            Assert.IsNotNull(resultado);
        }

        [TestMethod]
        public void DatosBasicosSGR_GuardarDefinitivo()
        {
            var parametrosGuardar = new ParametrosGuardarDto<DatosBasicosSGRDto>()
            {
                AccionId = new Guid("418D76AC-F081-4D73-A05A-530CD7C6AFF6"),
                InstanciaId = new Guid("6A0E8FC3-73CF-46B5-B59C-0F683A30AE1D"),
                Usuario = "jdelgado",
                Contenido = new DatosBasicosSGRDto()
                {
                    DatosBasicosSGRId = 1,
                    ProyectoId = 145896,
                    Bpin = "2017011000042",
                    NumeroPresentacion = "2",
                    FechaVerificacionRequisitos = new DateTime(2019, 1, 1),
                    ObjetivoSGRId = 1,
                    ObjetivoSGR = "objetivo1",
                    EjecutorPropuestoId = 2,
                    NitEjecutorPropuesto = "NitEjecutorPropuesto",
                    InterventorPropuestoId = 3,
                    NitInterventorPropuesto = "NitInterventorPropuesto",
                    InterventorPropuesto = "InterventorPropuesto",
                    TiempoEstimadoEjecucionFisicaFinanciera = 1,
                    EstimacionCostosFasesPosteriores = (decimal)1245.00,
                    FuentesInterventoria = new List<FuentesInterventoriaDto> {
                    new FuentesInterventoriaDto()
                    {
                        ProgramacionFuenteId = 2,
                        Vigencia = "2019",
                        GrupoRecurso = "PGN",
                        TipoEntidadId = 10,
                        TipoEntidad = "Nacion",
                        EntidadId = 8,
                        Entidad = "Entidad",
                        TipoRecursoId = 25,
                        TipoRecurso = "Propios",
                        Solicitado = 124578,
                        ValorAprobadoBienio1 = "258",
                        ValorAprobadoBienio2 = "268",
                        ValorAprobadoBienio3 = "278",
                        ValorAprobadoBienio4 = "288"
                    }

                    }

                }
                };

            var parametroAuditoria = new ParametrosAuditoriaDto()
            {
                Usuario = "jdelgado",
                Ip = "localhost"
            };

            DatosBasicosSGRServicio.Guardar(parametrosGuardar, parametroAuditoria, false);
        }

    }
}
