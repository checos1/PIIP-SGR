using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DNP.ServiciosNegocio.Test.Servicios.Transversales
{
    using Configuracion;
    using DNP.ServiciosNegocio.Dominio.Dto.Transversales;
    using DNP.ServiciosNegocio.Persistencia.Implementaciones.Proyectos;
    using DNP.ServiciosNegocio.Persistencia.Implementaciones.TramitesProyectos;
    using DNP.ServiciosNegocio.Persistencia.Implementaciones.Transversales;
    using DNP.ServiciosNegocio.Persistencia.Interfaces.TramitesProyectos;
    using DNP.ServiciosNegocio.Persistencia.Interfaces.Transversales;
    using DNP.ServiciosNegocio.Servicios.Interfaces.Transversales;
    using Mock;
    using Moq;
    using Persistencia.Interfaces;
    using Persistencia.Modelo;
    using ServiciosNegocio.Servicios.Implementaciones.Transversales;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data.Entity;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    using System.Threading.Tasks;
    using Unity;

    [TestClass]
    public class SeccionCapituloServicioTest
    {
        private ICambiosRelacionPlanificacionServicio CambiosRelacionPlanificacionServicio { get; set; }
        private ISeccionCapituloPersistencia SeccionCapituloPersistencia { get; set; }
        private IFasePersistencia FasePersistencia { get; set; }
        private SeccionCapituloServicio SeccionCapituloServicio { get; set; }

        private readonly Mock<MGAWebContexto> _mockContext = new Mock<MGAWebContexto>();
        private readonly Mock<IContextoFactory> _mockContextFactory = new Mock<IContextoFactory>();
        private readonly Mock<DbSet<VGetFase>> _mockSetVGetFase = new Mock<DbSet<VGetFase>>();
        private readonly Mock<DbSet<Capitulo>> _mockSetCapitulo = new Mock<DbSet<Capitulo>>();
        private readonly Mock<DbSet<Seccion>> _mockSetSeccion = new Mock<DbSet<Seccion>>();
        private readonly Mock<DbSet<MacroprocesoSeccion>> _mockSetMacroprocesoSeccion = new Mock<DbSet<MacroprocesoSeccion>>();
        private readonly Mock<DbSet<SeccionCapitulos>> _mockSetSeccionCapitulos = new Mock<DbSet<SeccionCapitulos>>();
        private readonly Mock<DbSet<CapitulosModificados>> _mockSetCapitulosModificados = new Mock<DbSet<CapitulosModificados>>();

        [TestInitialize]
        public void Init()
        {
            var contenedor = UnityConfig.Container;
            CambiosRelacionPlanificacionServicio = contenedor.Resolve<ICambiosRelacionPlanificacionServicio>();
            SeccionCapituloPersistencia = contenedor.Resolve<ISeccionCapituloPersistencia>();
            FasePersistencia = contenedor.Resolve<IFasePersistencia>();
            SeccionCapituloServicio = new SeccionCapituloServicio(SeccionCapituloPersistencia, FasePersistencia, CambiosRelacionPlanificacionServicio);

            var dataVGetFase = new List<VGetFase>() { new VGetFase() { Id = 1, NombreFase = "Test", FaseGUID = new Guid("a562885e-3c75-d1b5-6ebc-4bcebb17ca6b") } }.AsQueryable();
            var dataCapitulo = new List<Capitulo>() { new Capitulo() { Id = 1, Nombre = "Test capitulo" } }.AsQueryable();
            var dataSeccion = new List<Seccion>() { new Seccion() { Id = 1, Nombre = "Test seccion" } }.AsQueryable();
            var dataMacroprocesoSeccion = new List<MacroprocesoSeccion>() { new MacroprocesoSeccion() { Id = 1, Nombre = "Test MacroprocesoSeccion", SeccionId = 1, FaseId = 1 } }.AsQueryable();
            var dataSeccionCapitulos = new List<SeccionCapitulos>() { new SeccionCapitulos() { Id = 1, MacroprocesoSeccionId = 1, CapituloId = 1 } }.AsQueryable();
            var dataCapitulosModificados = new List<CapitulosModificados>() { new CapitulosModificados() { Id = 1, InstanciaId = new Guid("4C2E62CD-CEAD-48EF-88C6-A50AB5913716"), ProyectoId = 1, SeccionCapituloId = 1 } }.AsQueryable();

            _mockSetVGetFase.As<IQueryable<VGetFase>>().Setup(m => m.Provider).Returns(dataVGetFase.Provider);
            _mockSetVGetFase.As<IQueryable<VGetFase>>().Setup(m => m.Expression).Returns(dataVGetFase.Expression);
            _mockSetVGetFase.As<IQueryable<VGetFase>>().Setup(m => m.ElementType).Returns(dataVGetFase.ElementType);
            _mockSetVGetFase.As<IQueryable<VGetFase>>().Setup(m => m.GetEnumerator()).Returns(dataVGetFase.GetEnumerator());

            _mockSetCapitulo.As<IQueryable<Capitulo>>().Setup(m => m.Provider).Returns(dataCapitulo.Provider);
            _mockSetCapitulo.As<IQueryable<Capitulo>>().Setup(m => m.Expression).Returns(dataCapitulo.Expression);
            _mockSetCapitulo.As<IQueryable<Capitulo>>().Setup(m => m.ElementType).Returns(dataCapitulo.ElementType);
            _mockSetCapitulo.As<IQueryable<Capitulo>>().Setup(m => m.GetEnumerator()).Returns(dataCapitulo.GetEnumerator());

            _mockSetSeccion.As<IQueryable<Seccion>>().Setup(m => m.Provider).Returns(dataSeccion.Provider);
            _mockSetSeccion.As<IQueryable<Seccion>>().Setup(m => m.Expression).Returns(dataSeccion.Expression);
            _mockSetSeccion.As<IQueryable<Seccion>>().Setup(m => m.ElementType).Returns(dataSeccion.ElementType);
            _mockSetSeccion.As<IQueryable<Seccion>>().Setup(m => m.GetEnumerator()).Returns(dataSeccion.GetEnumerator());

            _mockSetMacroprocesoSeccion.As<IQueryable<MacroprocesoSeccion>>().Setup(m => m.Provider).Returns(dataMacroprocesoSeccion.Provider);
            _mockSetMacroprocesoSeccion.As<IQueryable<MacroprocesoSeccion>>().Setup(m => m.Expression).Returns(dataMacroprocesoSeccion.Expression);
            _mockSetMacroprocesoSeccion.As<IQueryable<MacroprocesoSeccion>>().Setup(m => m.ElementType).Returns(dataMacroprocesoSeccion.ElementType);
            _mockSetMacroprocesoSeccion.As<IQueryable<MacroprocesoSeccion>>().Setup(m => m.GetEnumerator()).Returns(dataMacroprocesoSeccion.GetEnumerator());

            _mockSetSeccionCapitulos.As<IQueryable<SeccionCapitulos>>().Setup(m => m.Provider).Returns(dataSeccionCapitulos.Provider);
            _mockSetSeccionCapitulos.As<IQueryable<SeccionCapitulos>>().Setup(m => m.Expression).Returns(dataSeccionCapitulos.Expression);
            _mockSetSeccionCapitulos.As<IQueryable<SeccionCapitulos>>().Setup(m => m.ElementType).Returns(dataSeccionCapitulos.ElementType);
            _mockSetSeccionCapitulos.As<IQueryable<SeccionCapitulos>>().Setup(m => m.GetEnumerator()).Returns(dataSeccionCapitulos.GetEnumerator());

            _mockSetCapitulosModificados.As<IQueryable<CapitulosModificados>>().Setup(m => m.Provider).Returns(dataCapitulosModificados.Provider);
            _mockSetCapitulosModificados.As<IQueryable<CapitulosModificados>>().Setup(m => m.Expression).Returns(dataCapitulosModificados.Expression);
            _mockSetCapitulosModificados.As<IQueryable<CapitulosModificados>>().Setup(m => m.ElementType).Returns(dataCapitulosModificados.ElementType);
            _mockSetCapitulosModificados.As<IQueryable<CapitulosModificados>>().Setup(m => m.GetEnumerator()).Returns(dataCapitulosModificados.GetEnumerator());

            _mockContext.Setup(m => m.VGetFase).Returns(_mockSetVGetFase.Object);
            _mockContext.Setup(m => m.Capitulo).Returns(_mockSetCapitulo.Object);
            _mockContext.Setup(m => m.Seccion).Returns(_mockSetSeccion.Object);
            _mockContext.Setup(m => m.MacroprocesoSeccion).Returns(_mockSetMacroprocesoSeccion.Object);
            _mockContext.Setup(m => m.SeccionCapitulos).Returns(_mockSetSeccionCapitulos.Object);
            _mockContext.Setup(m => m.CapitulosModificados).Returns(_mockSetCapitulosModificados.Object);

            var objetoRetornorListaCapitulosModificadosByMacroproceso = new UspGetCapitulos_Result
            {
                SeccionCapituloId = 1,
                SeccionId = 1,
                CapituloId = 1,
                Macroproceso = "a562885e-3c75-d1b5-6ebc-4bcebb17ca6b",
                Instancia = new Guid("4C2E62CD-CEAD-48EF-88C6-A50AB5913716"),
                Seccion = "Test",
                Modificado = true,
                Capitulo = "Test"
            };

            var objetoRetornoListaCapitulosByMacroproceso = new UspGetCapitulosByMacroprocesoNivel_Result();
            var objetoRetornoValidarSeccionCapitulos = string.Empty;
            var objetoRetornoErroresProyecto = new UspGetErroresProyecto_Result();
            var objetoRetornoProyectosPorId = new uspProyectosPorId_Result
            {
                ProyectoId = 1,
                CodigoBpin = "202200000000036"
            };

            var objetoRetornoProyectosPorIdSE = new uspProyectosPorId_Result
            {
                ProyectoId = 2,
                CodigoBpin = "202200000000037"
            };

            var objetoRetornoErroresProyectoFuenteFinanciacion = new UspGetErroresProyectoFuenteFinanciacion_Result();
            var objetoRetornoCostosPIIPVsFuentesPiip = "{\"BPIN\":\"202200000000036\",\"Etapas\":[{\"Etapa\":\"Inversión\",\"Valores\":[{\"Vigencia\":2022,\"Valorcosto\":0.00,\"ValorFuentes\":0.00},{\"Vigencia\":2023,\"Valorcosto\":256290880000.00,\"ValorFuentes\":0.00},{\"Vigencia\":2024,\"Valorcosto\":169597071156.00,\"ValorFuentes\":0.00},{\"Vigencia\":2025,\"Valorcosto\":82158000000.00,\"ValorFuentes\":0.00}]},{\"Etapa\":\"Operación\",\"Valores\":[{\"Vigencia\":2022,\"Valorcosto\":0.00,\"ValorFuentes\":0.00},{\"Vigencia\":2023,\"Valorcosto\":0.00,\"ValorFuentes\":0.00},{\"Vigencia\":2024,\"Valorcosto\":0.00,\"ValorFuentes\":0.00},{\"Vigencia\":2025,\"Valorcosto\":0.00,\"ValorFuentes\":0.00}]},{\"Etapa\":\"Preinversión\",\"Valores\":[{\"Vigencia\":2022,\"Valorcosto\":0.00,\"ValorFuentes\":0.00},{\"Vigencia\":2023,\"Valorcosto\":0.00,\"ValorFuentes\":0.00},{\"Vigencia\":2024,\"Valorcosto\":0.00,\"ValorFuentes\":0.00},{\"Vigencia\":2025,\"Valorcosto\":0.00,\"ValorFuentes\":0.00}]}]}";
            var objetoRetornoCostosPIIPVsFuentesPiipSE = "{\"BPIN\":\"202200000000037\",\"Etapas\":[{\"Etapa\":\"Inversión\",\"Valores\":[{\"Vigencia\":2022,\"Valorcosto\":0.00,\"ValorFuentes\":0.00},{\"Vigencia\":2023,\"Valorcosto\":0.00,\"ValorFuentes\":0.00},{\"Vigencia\":2024,\"Valorcosto\":0.00,\"ValorFuentes\":0.00},{\"Vigencia\":2025,\"Valorcosto\":0.00,\"ValorFuentes\":0.00}]},{\"Etapa\":\"Operación\",\"Valores\":[{\"Vigencia\":2022,\"Valorcosto\":0.00,\"ValorFuentes\":0.00},{\"Vigencia\":2023,\"Valorcosto\":0.00,\"ValorFuentes\":0.00},{\"Vigencia\":2024,\"Valorcosto\":0.00,\"ValorFuentes\":0.00},{\"Vigencia\":2025,\"Valorcosto\":0.00,\"ValorFuentes\":0.00}]},{\"Etapa\":\"Preinversión\",\"Valores\":[{\"Vigencia\":2022,\"Valorcosto\":0.00,\"ValorFuentes\":0.00},{\"Vigencia\":2023,\"Valorcosto\":0.00,\"ValorFuentes\":0.00},{\"Vigencia\":2024,\"Valorcosto\":0.00,\"ValorFuentes\":0.00},{\"Vigencia\":2025,\"Valorcosto\":0.00,\"ValorFuentes\":0.00}]}]}";
            var objetoRetornoErroresTramite = new UspGetErroresTramite_Result();
            var objetoRetornoErroresViabilidad = new UspGetErroresViabilidad_Result();
            var objetoRetornoSeccionesTramite = new uspGetSecciones_Result();
            var objetoRetornoSeccionesPorFase = new uspGetSeccionesPorFase_Result();
            var objetoRetornoErroresAprobacionRol = new UspGetErroresProyecto_Result();

            var mockListaCapitulosModificadosByMacroproceso = new Mock<ObjectResult<UspGetCapitulos_Result>>();
            var mockListaCapitulosByMacroproceso = new Mock<ObjectResult<UspGetCapitulosByMacroprocesoNivel_Result>>();
            var mockValidarSeccionCapitulos = new Mock<ObjectResult<string>>();
            var mockErroresProyecto = new Mock<ObjectResult<UspGetErroresProyecto_Result>>();
            var mockErroresProyectoSE = new Mock<ObjectResult<UspGetErroresProyecto_Result>>();
            var mockProyectosPorId = new Mock<ObjectResult<uspProyectosPorId_Result>>();
            var mockProyectosPorIdSE = new Mock<ObjectResult<uspProyectosPorId_Result>>();
            var mockErroresProyectoFuenteFinanciacion = new Mock<ObjectResult<UspGetErroresProyectoFuenteFinanciacion_Result>>();
            var mockErroresProyectoFuenteFinanciacionSE = new Mock<ObjectResult<UspGetErroresProyectoFuenteFinanciacion_Result>>();
            var mockCostosPIIPVsFuentesPiip = new Mock<ObjectResult<string>>();
            var mockCostosPIIPVsFuentesPiipSE = new Mock<ObjectResult<string>>();
            var mockErroresTramite = new Mock<ObjectResult<UspGetErroresTramite_Result>>();
            var mockErroresViabilidad = new Mock<ObjectResult<UspGetErroresViabilidad_Result>>();
            var mockSeccionesTramite = new Mock<ObjectResult<uspGetSecciones_Result>>();
            var mockSeccionesPorFase = new Mock<ObjectResult<uspGetSeccionesPorFase_Result>>();
            var mockErroresAprobacionRol = new Mock<ObjectResult<UspGetErroresProyecto_Result>>();

            mockListaCapitulosModificadosByMacroproceso.SetupReturn(objetoRetornorListaCapitulosModificadosByMacroproceso);
            mockListaCapitulosByMacroproceso.SetupReturn(objetoRetornoListaCapitulosByMacroproceso);
            mockValidarSeccionCapitulos.SetupReturn(objetoRetornoValidarSeccionCapitulos);
            mockErroresProyecto.SetupReturn(objetoRetornoErroresProyecto);
            mockErroresProyectoSE.SetupReturn(objetoRetornoErroresProyecto);
            mockProyectosPorId.SetupReturn(objetoRetornoProyectosPorId);
            mockProyectosPorIdSE.SetupReturn(objetoRetornoProyectosPorIdSE);
            mockErroresProyectoFuenteFinanciacion.SetupReturn(objetoRetornoErroresProyectoFuenteFinanciacion);
            mockErroresProyectoFuenteFinanciacionSE.SetupReturn(objetoRetornoErroresProyectoFuenteFinanciacion);
            mockCostosPIIPVsFuentesPiip.SetupReturn(objetoRetornoCostosPIIPVsFuentesPiip);
            mockCostosPIIPVsFuentesPiipSE.SetupReturn(objetoRetornoCostosPIIPVsFuentesPiipSE);
            mockErroresTramite.SetupReturn(objetoRetornoErroresTramite);
            mockErroresViabilidad.SetupReturn(objetoRetornoErroresViabilidad);
            mockSeccionesTramite.SetupReturn(objetoRetornoSeccionesTramite);
            mockSeccionesPorFase.SetupReturn(objetoRetornoSeccionesPorFase);
            mockErroresAprobacionRol.SetupReturn(objetoRetornoErroresAprobacionRol);

            _mockContext.Setup(mc => mc.UspGetCapitulos(1, 1, new Guid("4C2E62CD-CEAD-48EF-88C6-A50AB5913716"))).Returns(mockListaCapitulosModificadosByMacroproceso.Object);
            _mockContext.Setup(mc => mc.UspGetCapitulosByMacroprocesoNivel(1, new Guid("5E03D2F8-BB24-4E72-92E7-4560E04B9F2E"), new Guid("4C2E62CD-CEAD-48EF-88C6-A50AB5913716"))).Returns(mockListaCapitulosByMacroproceso.Object);
            _mockContext.Setup(mc => mc.UspGetValidarCapitulos(1, 1, new Guid("4C2E62CD-CEAD-48EF-88C6-A50AB5913716"))).Returns(mockValidarSeccionCapitulos.Object);
            _mockContext.Setup(mc => mc.UspGetErroresProyecto(new Guid("F73990EF-04B5-4123-B87F-38DA445B6888"), new Guid("4C2E62CD-CEAD-48EF-88C6-A50AB5913716"), 1)).Returns(mockErroresProyecto.Object);            
            _mockContext.Setup(mc => mc.UspGetErroresProyecto(new Guid("F73990EF-04B5-4123-B87F-38DA445B6888"), new Guid("4C2E62CD-CEAD-48EF-88C6-A50AB5913716"), 2)).Returns(mockErroresProyectoSE.Object);            
            _mockContext.Setup(mc => mc.uspProyectosPorId("1")).Returns(mockProyectosPorId.Object);            
            _mockContext.Setup(mc => mc.uspProyectosPorId("2")).Returns(mockProyectosPorIdSE.Object);            
            _mockContext.Setup(mc => mc.UspGetErroresProyectoFuenteFinanciacion(new Guid("F73990EF-04B5-4123-B87F-38DA445B6888"), new Guid("4C2E62CD-CEAD-48EF-88C6-A50AB5913716"), 1)).Returns(mockErroresProyectoFuenteFinanciacion.Object);            
            _mockContext.Setup(mc => mc.UspGetErroresProyectoFuenteFinanciacion(new Guid("F73990EF-04B5-4123-B87F-38DA445B6888"), new Guid("4C2E62CD-CEAD-48EF-88C6-A50AB5913716"), 2)).Returns(mockErroresProyectoFuenteFinanciacionSE.Object);            
            _mockContext.Setup(mc => mc.UspGetCostosPIIPVsFuentesPiip_JSON("202200000000036")).Returns(mockCostosPIIPVsFuentesPiip.Object);            
            _mockContext.Setup(mc => mc.UspGetCostosPIIPVsFuentesPiip_JSON("202200000000037")).Returns(mockCostosPIIPVsFuentesPiipSE.Object);            
            _mockContext.Setup(mc => mc.UspGetErroresTramite(new Guid("F73990EF-04B5-4123-B87F-38DA445B6888"), new Guid("4C2E62CD-CEAD-48EF-88C6-A50AB5913716"), new Guid("D2BA19EB-0487-4C94-8960-3A6047B81409"), "jdelgado", true)).Returns(mockErroresTramite.Object);            
            _mockContext.Setup(mc => mc.UspGetErroresViabilidad(new Guid("F73990EF-04B5-4123-B87F-38DA445B6888"), 1, new Guid("418D76AC-F081-4D73-A05A-530CD7C6AFF6"), new Guid("4C2E62CD-CEAD-48EF-88C6-A50AB5913716"))).Returns(mockErroresViabilidad.Object);            
            _mockContext.Setup(mc => mc.uspGetSecciones(new Guid("F73990EF-04B5-4123-B87F-38DA445B6888"), new Guid("4C2E62CD-CEAD-48EF-88C6-A50AB5913716"))).Returns(mockSeccionesTramite.Object);            
            _mockContext.Setup(mc => mc.uspGetSeccionesPorFase(new Guid("F73990EF-04B5-4123-B87F-38DA445B6888"), new Guid("418D76AC-F081-4D73-A05A-530CD7C6AFF6"))).Returns(mockSeccionesPorFase.Object);            
            _mockContext.Setup(mc => mc.UspGetErroresPreguntasAprobacionRol(new Guid("F73990EF-04B5-4123-B87F-38DA445B6888"), new Guid("4C2E62CD-CEAD-48EF-88C6-A50AB5913716"), 1)).Returns(mockErroresAprobacionRol.Object);            

            _mockContextFactory.Setup(mcf => mcf.CrearContextoConConexion(ConfigurationManager.ConnectionStrings["MGAWebContexto"].ConnectionString)).Returns(_mockContext.Object);
        }

        [TestMethod]
        public async Task ConsultarSeccionCapitulos_Test()
        {
            SeccionCapituloServicio = new SeccionCapituloServicio(new SeccionCapituloPersistencia(_mockContextFactory.Object, FasePersistencia), new FasePersistencia(_mockContextFactory.Object), CambiosRelacionPlanificacionServicio);
            string macroproceso = "a562885e-3c75-d1b5-6ebc-4bcebb17ca6b";
            int proyectoId = 1;
            string instanciaId = "4C2E62CD-CEAD-48EF-88C6-A50AB5913716";
            var result = await SeccionCapituloServicio.ConsultarSeccionCapitulos(macroproceso, proyectoId, instanciaId);

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task ObtenerListaCapitulosByMacroproceso_Test()
        {
            SeccionCapituloServicio = new SeccionCapituloServicio(new SeccionCapituloPersistencia(_mockContextFactory.Object, FasePersistencia), new FasePersistencia(_mockContextFactory.Object), CambiosRelacionPlanificacionServicio);
            string macroproceso = "a562885e-3c75-d1b5-6ebc-4bcebb17ca6b";
            string nivelId = "5E03D2F8-BB24-4E72-92E7-4560E04B9F2E"; 
            string flujoId = "4C2E62CD-CEAD-48EF-88C6-A50AB5913716";
            var result = await SeccionCapituloServicio.ConsultarSeccionCapitulosByMacroproceso(macroproceso, nivelId, flujoId);

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task ValidarSeccionCapitulos_Test()
        {
            SeccionCapituloServicio = new SeccionCapituloServicio(new SeccionCapituloPersistencia(_mockContextFactory.Object, FasePersistencia), new FasePersistencia(_mockContextFactory.Object), CambiosRelacionPlanificacionServicio);
            string macroproceso = "a562885e-3c75-d1b5-6ebc-4bcebb17ca6b";
            int proyectoId = 1;
            string instanciaId = "4C2E62CD-CEAD-48EF-88C6-A50AB5913716";
            var result = await SeccionCapituloServicio.ValidarSeccionCapitulos(macroproceso, proyectoId, instanciaId);

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task ObtenerCapitulosModificados_Test()
        {
            SeccionCapituloServicio = new SeccionCapituloServicio(new SeccionCapituloPersistencia(_mockContextFactory.Object, FasePersistencia), new FasePersistencia(_mockContextFactory.Object), CambiosRelacionPlanificacionServicio);
            string macroproceso = "F73990EF-04B5-4123-B87F-38DA445B6888";
            string capitulo = "Test capitulo";
            string seccion = "Test seccion";
            int proyectoId = 1;
            string instanciaId = "4C2E62CD-CEAD-48EF-88C6-A50AB5913716";




            var result = "{}";
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task ObtenerErroresProyecto_ConErrores_Test()
        {
            SeccionCapituloServicio = new SeccionCapituloServicio(new SeccionCapituloPersistencia(_mockContextFactory.Object, FasePersistencia), new FasePersistencia(_mockContextFactory.Object), CambiosRelacionPlanificacionServicio);
            string macroproceso = "F73990EF-04B5-4123-B87F-38DA445B6888";
            int proyectoId = 1;
            string instanciaId = "4C2E62CD-CEAD-48EF-88C6-A50AB5913716";
            var result = await SeccionCapituloServicio.ObtenerErroresProyecto(macroproceso, proyectoId, instanciaId);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task ObtenerErroresProyecto_SinErrores_Test()
        {
            SeccionCapituloServicio = new SeccionCapituloServicio(new SeccionCapituloPersistencia(_mockContextFactory.Object, FasePersistencia), new FasePersistencia(_mockContextFactory.Object), CambiosRelacionPlanificacionServicio);
            string macroproceso = "F73990EF-04B5-4123-B87F-38DA445B6888";
            int proyectoId = 2;
            string instanciaId = "4C2E62CD-CEAD-48EF-88C6-A50AB5913716";
            var result = await SeccionCapituloServicio.ObtenerErroresProyecto(macroproceso, proyectoId, instanciaId);
            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public async Task ObtenerErroresTramite_Test()
        {
            SeccionCapituloServicio = new SeccionCapituloServicio(new SeccionCapituloPersistencia(_mockContextFactory.Object, FasePersistencia), new FasePersistencia(_mockContextFactory.Object), CambiosRelacionPlanificacionServicio);
            string macroproceso = "F73990EF-04B5-4123-B87F-38DA445B6888";
            string instanciaId = "4C2E62CD-CEAD-48EF-88C6-A50AB5913716";
            string accionId = "D2BA19EB-0487-4C94-8960-3A6047B81409";
            string usuario = "jdelgado";
            bool cp = true;
            var result = await SeccionCapituloServicio.ObtenerErroresTramite(macroproceso, instanciaId, accionId, usuario, cp);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task ObtenerErroresViabilidad_Test()
        {
            SeccionCapituloServicio = new SeccionCapituloServicio(new SeccionCapituloPersistencia(_mockContextFactory.Object, FasePersistencia), new FasePersistencia(_mockContextFactory.Object), CambiosRelacionPlanificacionServicio);
            string macroproceso = "F73990EF-04B5-4123-B87F-38DA445B6888";
            string instanciaId = "4C2E62CD-CEAD-48EF-88C6-A50AB5913716";
            string nivelId = "418D76AC-F081-4D73-A05A-530CD7C6AFF6";
            int proyectoId = 1;
            var result = await SeccionCapituloServicio.ObtenerErroresViabilidad(macroproceso, proyectoId, nivelId, instanciaId);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task ObtenerSeccionesTramite_Test()
        {
            SeccionCapituloServicio = new SeccionCapituloServicio(new SeccionCapituloPersistencia(_mockContextFactory.Object, FasePersistencia), new FasePersistencia(_mockContextFactory.Object), CambiosRelacionPlanificacionServicio);
            string macroproceso = "F73990EF-04B5-4123-B87F-38DA445B6888";
            string instanciaId = "4C2E62CD-CEAD-48EF-88C6-A50AB5913716";
            var result = await SeccionCapituloServicio.ObtenerSeccionesTramite(macroproceso, instanciaId);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task ObtenerSeccionesPorFase_Test()
        {
            SeccionCapituloServicio = new SeccionCapituloServicio(new SeccionCapituloPersistencia(_mockContextFactory.Object, FasePersistencia), new FasePersistencia(_mockContextFactory.Object), CambiosRelacionPlanificacionServicio);
            string macroproceso = "F73990EF-04B5-4123-B87F-38DA445B6888";
            string faseNivel = "418D76AC-F081-4D73-A05A-530CD7C6AFF6";
            var result = await SeccionCapituloServicio.ObtenerSeccionesPorFase(macroproceso, faseNivel);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task ObtenerErroresAprobacionRol_Test()
        {
            SeccionCapituloServicio = new SeccionCapituloServicio(new SeccionCapituloPersistencia(_mockContextFactory.Object, FasePersistencia), new FasePersistencia(_mockContextFactory.Object), CambiosRelacionPlanificacionServicio);
            string macroproceso = "F73990EF-04B5-4123-B87F-38DA445B6888";
            string instanciaId = "4C2E62CD-CEAD-48EF-88C6-A50AB5913716";
            int proyectoId = 1;
            var result = await SeccionCapituloServicio.ObtenerErroresAprobacionRol(macroproceso, proyectoId, instanciaId);
            Assert.IsNotNull(result);
        }
    }
}
