using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DNP.ServiciosNegocio.Test.Servicios.Proyectos
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data.Entity;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    using System.Threading.Tasks;
    using Comunes.Dto.ObjetosNegocio;
    using Configuracion;
    using DNP.ServiciosNegocio.Comunes.Utilidades;
    using DNP.ServiciosNegocio.Persistencia.Interfaces.Transversales;
    using DNP.ServiciosNegocio.Persistencia.Modelo_OnlySP;
    using Dominio.Dto.Proyectos;
    using Mock;
    using Moq;
    using Persistencia.Implementaciones.Proyectos;
    using Persistencia.Interfaces;
    using Persistencia.Interfaces.Proyectos;
    using Persistencia.Modelo;
    using ServiciosNegocio.Servicios.Implementaciones.Proyectos;
    using ServiciosNegocio.Servicios.Interfaces.Transversales;
    using Unity;

    [TestClass]
    public class ProyectoServicioTest
    {
        private IProyectoPersistencia ProyectoPersistencia { get; set; }
        private ICacheServicio CacheServicio { get; set; }
        private ProyectoServicio ProyectoServicio { get; set; }
        public string TokenAutorizacion { get; set; }
        private readonly Mock<MGAWebContexto> _mockContext = new Mock<MGAWebContexto>();
        private readonly Mock<ModelOnlySPEntities> _mockContextOnlySP = new Mock<ModelOnlySPEntities>();
        private readonly Mock<IContextoFactory> _mockContextFactory = new Mock<IContextoFactory>();
        private readonly Mock<ISeccionCapituloPersistencia> _seccionCapituloPersistencia = new Mock<ISeccionCapituloPersistencia>();
        private readonly Mock<IFasePersistencia> _fasePersistencia = new Mock<IFasePersistencia>();
        private readonly Mock<DbSet<VGetFase>> _mockSetVGetFase = new Mock<DbSet<VGetFase>>();
        private readonly Mock<DbSet<CapitulosModificados>> _mockSetCapitulosModificados = new Mock<DbSet<CapitulosModificados>>();

        [TestInitialize]
        public void Init()
        {
            var contenedor = UnityConfig.Container;
            ProyectoPersistencia = contenedor.Resolve<IProyectoPersistencia>();
            CacheServicio = contenedor.Resolve<ICacheServicio>();
            ProyectoServicio = new ProyectoServicio(CacheServicio, ProyectoPersistencia);
            TokenAutorizacion = "Basic amRlbGdhZG86MTYyNTk0NjM=";
            var dataVGetFase = new List<VGetFase>() { new VGetFase() { Id = 1, NombreFase = "Test", FaseGUID = new Guid("a562885e-3c75-d1b5-6ebc-4bcebb17ca6b") } }.AsQueryable();
            var dataCapitulosModificados = new List<CapitulosModificados>() { new CapitulosModificados() { Id = 1, InstanciaId = new Guid("3DB31A78-6E4C-4A55-B047-338E85A93A17"), ProyectoId = 1, SeccionCapituloId = 1 } }.AsQueryable();

            var objetoRetornoProyectosPorEntidades = new uspProyectosPorEntidades_Result();
            var objetoRetornoProyectosEntidades = new uspProyectosPorEstadosValidos_Result();
            var objetoRetornoProyectosPorBpins = new uspProyectosPorBPIN_Result();
            var objetoRetornoEntidades = new uspEntidades_Result();
            var objetoRetornoProyectosPorIds = new uspProyectosPorId_Result();
            var objetoRetornoProyectoContracredito = new uspGetProyectosContracredito_Result();
            var objetoRetornoProyectoCredito = new uspGetProyectosCredito_Result();
            var objetoRetornoMatrizEntidadDestinoAccion = new spGetMatrizEntidadDestinoAccion_Result();
            string objetoRetornoResumenObjetivosProductos = string.Empty;
            string objetoRetornoResumenObjetivosProductosJustificacion = string.Empty;
            string objetoRetornoJustificacionLocalizacionProyecto = string.Empty;
            var objetoRetornoInstanciaProyectoTramite = new UspGetInstanciaProyectoTramite_Result();
            string objetoRetornoProyectosBeneficiarios = string.Empty;
            string objetoRetornoCategoriaSubcategoria = string.Empty;
            string objetoRetornoConsultarProyectosASeleccionar = string.Empty;
            int objetoRetornoInsertarAuditoriaEntidad = 1;
            var objetoRetornoObtenerAuditoriaEntidad = new uspGetAuditoriaEntidadProyecto_Result
            {
                Id = 1,
                IdEntidadOrigen = 265,
                IdEntidadDestino = 265,
                NombreEntidadDestino = "Boyacá",
                NombreEntidadOrigen = "Boyacá",
                FechaMovimiento = DateTime.Now,
                IdUsuario = new Guid("3DB31A78-6E4C-4A55-B047-338E85A93A17"),
                NombreUsuario = "111210012",
                ProyectoId = 97831,
                Proyecto = "Prueba",
                SectorId = 12,
                Sector = "Transporte",
                TipoEntidadId = 1,
                TipoEntidad = "Prueba tipo entidad"
            };
            var objetoRetornoObtenerMatrizEntidadDestino = new spGetMatrizEntidadDestinoAccionUnidad_Result();
            var objetoRetornoObtenerMatrizEntidadDestinoTransfer = new spGetTransferConfigurationUnidad_Result();
            var objetoRetornoObtenerProyectoConpes = new UspGetProyectosConpes_Result
            {
                ConpesId = 1,
                NombreConpes = "Test conpes",
                ProyectoId = 1,
                NumeroConpes = "001",
                FechaAprobacion = DateTime.Now,
                Seleccionado = 1
            };
            var objetoRetornoCapitulosByMacroprocesoNivel = new UspGetCapitulosByMacroprocesoNivel_Result();

            var mockProyectoPorEntidades = new Mock<ObjectResult<uspProyectosPorEntidades_Result>>();
            var mockProyectoEntidades = new Mock<ObjectResult<uspProyectosPorEstadosValidos_Result>>();
            var mockProyectoPorBpins = new Mock<ObjectResult<uspProyectosPorBPIN_Result>>();
            var mockEntidades = new Mock<ObjectResult<uspEntidades_Result>>();
            var mockProyectoPorIds = new Mock<ObjectResult<uspProyectosPorId_Result>>();
            var mockProyectoContracredito = new Mock<ObjectResult<uspGetProyectosContracredito_Result>>();
            var mockProyectoCredito = new Mock<ObjectResult<uspGetProyectosCredito_Result>>();
            var mockMatrizEntidadDestinoAccion = new Mock<ObjectResult<spGetMatrizEntidadDestinoAccion_Result>>();
            var mockResumenObjetivosProductos = new Mock<ObjectResult<string>>();
            var mockResumenObjetivosProductosJustificacion = new Mock<ObjectResult<string>>();
            var mockResumenJustificacionLocalizacionProyecto = new Mock<ObjectResult<string>>();
            var mockResumenInstanciaProyectoTramite = new Mock<ObjectResult<UspGetInstanciaProyectoTramite_Result>>();
            var mockResumenProyectosBeneficiarios = new Mock<ObjectResult<string>>();
            var mockResumenCategoriaSubcategoria = new Mock<ObjectResult<string>>();
            var mockResumenConsultarProyectosASeleccionar = new Mock<ObjectResult<string>>();
            var mockInsertarAuditoriaEntidad = new Mock<ObjectResult<int?>>();
            var mockObtenerAuditoriaEntidad = new Mock<ObjectResult<uspGetAuditoriaEntidadProyecto_Result>>();
            var mockObtenerMatrizEntidadDestino = new Mock<ObjectResult<spGetMatrizEntidadDestinoAccionUnidad_Result>>();
            var mockObtenerMatrizEntidadDestinoTransfer = new Mock<ObjectResult<spGetTransferConfigurationUnidad_Result>>();
            var mockObtenerProyectoConpes = new Mock<ObjectResult<UspGetProyectosConpes_Result>>();
            var mockCapitulosByMacroprocesoNivel = new Mock<ObjectResult<UspGetCapitulosByMacroprocesoNivel_Result>>();

            mockProyectoPorEntidades.SetupReturn(objetoRetornoProyectosPorEntidades);
            mockProyectoEntidades.SetupReturn(objetoRetornoProyectosEntidades);
            mockProyectoPorBpins.SetupReturn(objetoRetornoProyectosPorBpins);
            mockEntidades.SetupReturn(objetoRetornoEntidades);
            mockProyectoPorIds.SetupReturn(objetoRetornoProyectosPorIds);
            mockProyectoContracredito.SetupReturn(objetoRetornoProyectoContracredito);
            mockProyectoCredito.SetupReturn(objetoRetornoProyectoCredito);
            mockMatrizEntidadDestinoAccion.SetupReturn(objetoRetornoMatrizEntidadDestinoAccion);
            mockResumenObjetivosProductos.SetupReturn(objetoRetornoResumenObjetivosProductos);
            mockResumenObjetivosProductosJustificacion.SetupReturn(objetoRetornoResumenObjetivosProductosJustificacion);
            mockResumenJustificacionLocalizacionProyecto.SetupReturn(objetoRetornoJustificacionLocalizacionProyecto);
            mockResumenInstanciaProyectoTramite.SetupReturn(objetoRetornoInstanciaProyectoTramite);
            mockResumenProyectosBeneficiarios.SetupReturn(objetoRetornoProyectosBeneficiarios);
            mockResumenCategoriaSubcategoria.SetupReturn(objetoRetornoCategoriaSubcategoria);
            mockResumenConsultarProyectosASeleccionar.SetupReturn(objetoRetornoConsultarProyectosASeleccionar);
            mockInsertarAuditoriaEntidad.SetupReturn(objetoRetornoInsertarAuditoriaEntidad);
            mockObtenerAuditoriaEntidad.SetupReturn(objetoRetornoObtenerAuditoriaEntidad);
            mockObtenerMatrizEntidadDestino.SetupReturn(objetoRetornoObtenerMatrizEntidadDestino);
            mockObtenerMatrizEntidadDestinoTransfer.SetupReturn(objetoRetornoObtenerMatrizEntidadDestinoTransfer);
            mockObtenerProyectoConpes.SetupReturn(objetoRetornoObtenerProyectoConpes);
            mockCapitulosByMacroprocesoNivel.SetupReturn(objetoRetornoCapitulosByMacroprocesoNivel);

            _mockSetVGetFase.As<IQueryable<VGetFase>>().Setup(m => m.Provider).Returns(dataVGetFase.Provider);
            _mockSetVGetFase.As<IQueryable<VGetFase>>().Setup(m => m.Expression).Returns(dataVGetFase.Expression);
            _mockSetVGetFase.As<IQueryable<VGetFase>>().Setup(m => m.ElementType).Returns(dataVGetFase.ElementType);
            _mockSetVGetFase.As<IQueryable<VGetFase>>().Setup(m => m.GetEnumerator()).Returns(dataVGetFase.GetEnumerator());

            _mockSetCapitulosModificados.As<IQueryable<CapitulosModificados>>().Setup(m => m.Provider).Returns(dataCapitulosModificados.Provider);
            _mockSetCapitulosModificados.As<IQueryable<CapitulosModificados>>().Setup(m => m.Expression).Returns(dataCapitulosModificados.Expression);
            _mockSetCapitulosModificados.As<IQueryable<CapitulosModificados>>().Setup(m => m.ElementType).Returns(dataCapitulosModificados.ElementType);
            _mockSetCapitulosModificados.As<IQueryable<CapitulosModificados>>().Setup(m => m.GetEnumerator()).Returns(dataCapitulosModificados.GetEnumerator());

            _mockContext.Setup(m => m.VGetFase).Returns(_mockSetVGetFase.Object);
            _mockContext.Setup(m => m.CapitulosModificados).Returns(_mockSetCapitulosModificados.Object);
            _mockContext.Setup(mc => mc.uspProyectosPorEntidades("1,2,3", "Disponible")).Returns(mockProyectoPorEntidades.Object);
            _mockContext.Setup(mc => mc.uspProyectosPorEstadosValidos("Disponible", "1")).Returns(mockProyectoEntidades.Object);
            _mockContext.Setup(mc => mc.uspProyectosPorBPIN("2014010101")).Returns(mockProyectoPorBpins.Object);
            _mockContext.Setup(mc => mc.uspEntidades("1")).Returns(mockEntidades.Object);
            _mockContext.Setup(mc => mc.uspProyectosPorId("23")).Returns(mockProyectoPorIds.Object);
            _mockContext.Setup(mc => mc.spGetMatrizEntidadDestinoAccion(265)).Returns(mockMatrizEntidadDestinoAccion.Object);
            _mockContext.Setup(mc => mc.UspGetResumenObjetivosProductosActividades_JSON("202200000000110")).Returns(mockResumenObjetivosProductos.Object);
            _mockContext.Setup(mc => mc.UspGetResumenObjetivosProductosActividadesJustificacion_JSON("202200000000110")).Returns(mockResumenObjetivosProductosJustificacion.Object);
            _mockContext.Setup(mc => mc.upsGetLocalizacionJustificacionProyecto(97831)).Returns(mockResumenObjetivosProductosJustificacion.Object);
            _mockContext.Setup(mc => mc.upsGetLocalizacionJustificacionProyecto(97830)).Returns(mockResumenObjetivosProductosJustificacion.Object);
            _mockContext.Setup(mc => mc.UspGetInstanciaProyectoTramite(new Guid("3DB31A78-6E4C-4A55-B047-338E85A93A17"), "202200000000125")).Returns(mockResumenInstanciaProyectoTramite.Object);
            _mockContext.Setup(mc => mc.upsGetProyectosBeneficiarios_JSON("202200000000125")).Returns(mockResumenProyectosBeneficiarios.Object);
            _mockContext.Setup(mc => mc.uspGetCategoriaSubcategoria_JSON(176, 168, 1, 1)).Returns(mockResumenCategoriaSubcategoria.Object);
            _mockContext.Setup(mc => mc.uspPostAuditoriaEntidadProyectos(265, "Boyacá", 265, "Boyacá", new Guid("3DB31A78-6E4C-4A55-B047-338E85A93A17"), "111210012", DateTime.Now, 97831, 12, 1)).Returns(mockInsertarAuditoriaEntidad.Object);
            _mockContext.Setup(mc => mc.uspGetAuditoriaEntidadProyecto(97831)).Returns(mockObtenerAuditoriaEntidad.Object);
            _mockContext.Setup(mc => mc.spGetMatrizEntidadDestinoAccionUnidad(265, JsonUtilidades.ACadenaJson(new List<SectorIdDto> { new SectorIdDto { SectorId = 24 } }), JsonUtilidades.ACadenaJson(new List<EntidadDestinoIdDto> { new EntidadDestinoIdDto { EntidadDestinoId = 265 } }))).Returns(mockObtenerMatrizEntidadDestino.Object);
            _mockContext.Setup(mc => mc.spGetTransferConfigurationUnidad(265)).Returns(mockObtenerMatrizEntidadDestinoTransfer.Object);
            _mockContext.Setup(mc => mc.UspGetProyectosConpes(97831)).Returns(mockObtenerProyectoConpes.Object);
            _mockContext.Setup(mc => mc.UspGetCapitulosByMacroprocesoNivel(1, new Guid("4DB31A78-6E4C-4A55-B047-338E85A93A15"), new Guid("f562885e-3c75-d1b4-6ebc-4bcebb17ca6c"))).Returns(mockCapitulosByMacroprocesoNivel.Object);
            _mockContext.Setup(mc => mc.spInsertMatrizEntidadDestinoAccionUnidad(265, 12, new Guid("1DD225F4-5C34-4C55-B11D-E5856A68839B"), 265, "jdelgado", new ObjectParameter("Id", typeof(int)))).Returns(1);
            _mockContextOnlySP.Setup(mc => mc.uspGetProyectosContracredito("Nacional", 41, new Guid("CF1592AA-9087-3D77-B451-6F3557EF3F82"), null, null, null)).Returns(mockProyectoContracredito.Object);
            _mockContextOnlySP.Setup(mc => mc.uspGetProyectosCredito("Nacional", 41, new Guid("CF1592AA-9087-3D77-B451-6F3557EF3F82"), null, null)).Returns(mockProyectoCredito.Object);
            _mockContextOnlySP.Setup(mc => mc.UspGetProyectosASelecionar(new Guid("F562885E-3C75-D1B4-6EBC-4BCEBB17CA6C"), "76", 31)).Returns(mockResumenConsultarProyectosASeleccionar.Object);

            _mockContextFactory.Setup(mcf => mcf.CrearContextoConConexion(ConfigurationManager.ConnectionStrings["MGAWebContexto"].ConnectionString)).Returns(_mockContext.Object);
            _mockContextFactory.Setup(mcf => mcf.CrearContextoConConexionOnlySP(ConfigurationManager.ConnectionStrings["ModelOnlySPEntities"].ConnectionString)).Returns(_mockContextOnlySP.Object);
        }

        [TestMethod]
        public async Task ConsultarProyecto_NoNulo()
        {
            var bpin = "2017761220016";
            var result = await ProyectoServicio.ObtenerProyecto(bpin, TokenAutorizacion);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task ConsultarProyecto_Nulo()
        {
            var bpin = "000000000";
            var result = await ProyectoServicio.ObtenerProyecto(bpin, TokenAutorizacion);
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task ConsultarProyectoPreview_NoNulo()
        {
            ProyectoServicio = new ProyectoServicio(CacheServicio, new ProyectoPersistencia(_mockContextFactory.Object, _seccionCapituloPersistencia.Object, _fasePersistencia.Object));
            var result = await ProyectoServicio.ObtenerProyectoPreview();
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task CuandoEnvioIdsEntidadesInexistentes_RetornaNulo()
        {
            ProyectoServicio = new ProyectoServicio(CacheServicio, new ProyectoPersistencia(_mockContextFactory.Object, _seccionCapituloPersistencia.Object, _fasePersistencia.Object));
            var parametros = new ParametrosProyectosDto()
            {
                IdsEntidades = new List<int>() { 1, 2, 3, 4 },
                NombresEstadosProyectos = new List<string>() { "Disponible" },
                TokenAutorizacion = TokenAutorizacion
            };
            var result = await ProyectoServicio.ConsultarProyectosPorEntidadesYEstados(parametros);
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task CuandoEnvioIdsEntidadesInexistentesEnCacheYEnMga_RetornaNulos()
        {
            ProyectoServicio = new ProyectoServicio(CacheServicio, new ProyectoPersistencia(_mockContextFactory.Object, _seccionCapituloPersistencia.Object, _fasePersistencia.Object));
            var parametros = new ParametrosProyectosDto()
            {
                IdsEntidades = new List<int>() { 76 },
                NombresEstadosProyectos =
                                     new List<string>() { "No Disponible" },
                TokenAutorizacion = TokenAutorizacion
            };
            var result = await ProyectoServicio.ConsultarProyectosPorEntidadesYEstados(parametros);
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task CuandoEnvioIdsEntidadesInexistentesEnCacheYExiteEnMga_GuardaEnCache_RetornaNulos()
        {
            ProyectoServicio = new ProyectoServicio(CacheServicio, new ProyectoPersistencia(_mockContextFactory.Object, _seccionCapituloPersistencia.Object, _fasePersistencia.Object));
            var parametros = new ParametrosProyectosDto()
            {
                IdsEntidades = new List<int>() { 79 },
                NombresEstadosProyectos =
                                     new List<string>() { "No Disponible" },
                TokenAutorizacion = TokenAutorizacion
            };
            var result = await ProyectoServicio.ConsultarProyectosPorEntidadesYEstados(parametros);
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task CuandoEnvioIdsEntidadesInexistentesEnCacheYExiteEnMga_RetornaNulos()
        {
            ProyectoServicio = new ProyectoServicio(CacheServicio, new ProyectoPersistencia(_mockContextFactory.Object, _seccionCapituloPersistencia.Object, _fasePersistencia.Object));
            var parametros = new ParametrosProyectosDto()
            {
                IdsEntidades = new List<int>() { 79 },
                NombresEstadosProyectos =
                                     new List<string>() { "No Disponible" },
                TokenAutorizacion = TokenAutorizacion
            };
            var result = await ProyectoServicio.ConsultarProyectosPorEntidadesYEstados(parametros);
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task CuandoEnvio_IdsEntidadesExistentesEnCacheYExiteEnMga_IdsEstadosExistentes_RetornaNoNulos()
        {
            ProyectoServicio = new ProyectoServicio(CacheServicio, new ProyectoPersistencia(_mockContextFactory.Object, _seccionCapituloPersistencia.Object, _fasePersistencia.Object));
            var parametros = new ParametrosProyectosDto()
            {
                IdsEntidades = new List<int>() { 1, 2, 3 },
                NombresEstadosProyectos = new List<string>() { "Disponible" },
                TokenAutorizacion = TokenAutorizacion
            };
            var result = await ProyectoServicio.ConsultarProyectosPorEntidadesYEstados(parametros);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void FiltrarProyectosPorEstados_Test()
        {
            ProyectoServicio = new ProyectoServicio(CacheServicio, new ProyectoPersistencia(_mockContextFactory.Object, _seccionCapituloPersistencia.Object, _fasePersistencia.Object));
            var parametros = new ParametrosProyectosDto()
            {
                IdsEntidades = new List<int>() { 1, 2, 3 },
                NombresEstadosProyectos = new List<string>() { "Disponible" },
                TokenAutorizacion = TokenAutorizacion
            };
            var result = ProyectoServicio.FiltrarProyectosPorEstados(parametros, new List<ProyectoEntidadDto>()
                                                                                 {
                                                                                     new ProyectoEntidadDto()
                                                                                     {
                                                                                         CodigoBpin = "2014010101",
                                                                                         EntidadId = 1,
                                                                                         EntidadNombre = "Test",
                                                                                         ProyectoId = 1
                                                                                     }
                                                                                 });
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void ConsultarProyectosPorBPINs_Test()
        {
            ProyectoServicio = new ProyectoServicio(CacheServicio, new ProyectoPersistencia(_mockContextFactory.Object, _seccionCapituloPersistencia.Object, _fasePersistencia.Object));

            var result = ProyectoServicio.ConsultarProyectosPorBPINs(new BPINsProyectosDto()
            {
                BPINs = new List<string>() { "2014010101" },
                TokenAutorizacion = TokenAutorizacion

            });
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task ConsultarProyectosPorBPINs_Test_Null()
        {
            ProyectoServicio = new ProyectoServicio(CacheServicio, new ProyectoPersistencia(_mockContextFactory.Object, _seccionCapituloPersistencia.Object, _fasePersistencia.Object));

            var result = await ProyectoServicio.ConsultarProyectosPorBPINs(new BPINsProyectosDto()
            {
                BPINs = new List<string>(),
                TokenAutorizacion = TokenAutorizacion

            });
            Assert.IsNull(result);
        }

        [TestMethod]
        public void ConsultarEntidadesPorIds()
        {
            ProyectoServicio = new ProyectoServicio(CacheServicio, new ProyectoPersistencia(_mockContextFactory.Object, _seccionCapituloPersistencia.Object, _fasePersistencia.Object));
            var result = ProyectoServicio.ConsultarEntidadesPorIds(new List<string>() { "1" });
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task ConsultarEntidadesPorIds_Test_Null()
        {
            ProyectoServicio = new ProyectoServicio(CacheServicio, new ProyectoPersistencia(_mockContextFactory.Object, _seccionCapituloPersistencia.Object, _fasePersistencia.Object));
            var result = await ProyectoServicio.ConsultarEntidadesPorIds(new List<string>());
            Assert.IsNull(result);
        }

        [TestMethod]
        public void ConsultarProyectosPorIds_Test()
        {
            ProyectoServicio = new ProyectoServicio(CacheServicio, new ProyectoPersistencia(_mockContextFactory.Object, _seccionCapituloPersistencia.Object, _fasePersistencia.Object));

            var result = ProyectoServicio.ConsultarProyectosPorIds(new List<int>() { 23 });
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task ConsultarProyectosPorIds_Test_Null()
        {
            ProyectoServicio = new ProyectoServicio(CacheServicio, new ProyectoPersistencia(_mockContextFactory.Object, _seccionCapituloPersistencia.Object, _fasePersistencia.Object));

            var result = await ProyectoServicio.ConsultarProyectosPorIds(new List<int>());
            Assert.IsNull(result);
        }

        [TestMethod]
        public void ConsultarProyectosContracreditos_Test()
        {
            ProyectoServicio = new ProyectoServicio(CacheServicio, new ProyectoPersistencia(_mockContextFactory.Object, _seccionCapituloPersistencia.Object, _fasePersistencia.Object));

            var result = ProyectoServicio.ObtenerProyectosContracredito("Nacional", 41, new Guid("CF1592AA-9087-3D77-B451-6F3557EF3F82"), null, null, null);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void ConsultarProyectosCreditos_Test()
        {
            ProyectoServicio = new ProyectoServicio(CacheServicio, new ProyectoPersistencia(_mockContextFactory.Object, _seccionCapituloPersistencia.Object, _fasePersistencia.Object));

            var result = ProyectoServicio.ObtenerProyectosCredito("Nacional", 41, new Guid("CF1592AA-9087-3D77-B451-6F3557EF3F82"), null, null);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GuardarBeneficiarioTotales_Test()
        {
            ProyectoServicio = new ProyectoServicio(CacheServicio, new ProyectoPersistencia(_mockContextFactory.Object, _seccionCapituloPersistencia.Object, _fasePersistencia.Object));

            BeneficiarioTotalesDto beneficiarioTotalesDto = new BeneficiarioTotalesDto();

            beneficiarioTotalesDto.BPIN = "202200000000110";
            beneficiarioTotalesDto.NumeroPersonalAjuste = 1;
            beneficiarioTotalesDto.ProyectoId = 4;

            string result = "OK";

            try
            {
                ProyectoServicio.GuardarBeneficiarioTotales(beneficiarioTotalesDto, "");
            }
            catch
            {
                result = "ERROR";
            }

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GuardarBeneficiarioProducto_Test()
        {
            ProyectoServicio = new ProyectoServicio(CacheServicio, new ProyectoPersistencia(_mockContextFactory.Object, _seccionCapituloPersistencia.Object, _fasePersistencia.Object));

            BeneficiarioProductoDto beneficiarioDto = new BeneficiarioProductoDto();

            beneficiarioDto.ProyectoId = 1;
            beneficiarioDto.ProductoId = 1;
            beneficiarioDto.InterventionLocalizationTypeId = 1;
            beneficiarioDto.PersonasBeneficiaros = 1;

            string result = "OK";

            try
            {
                ProyectoServicio.GuardarBeneficiarioProducto(beneficiarioDto, "");
            }
            catch
            {
                result = "ERROR";
            }

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GuardarBeneficiarioProductoLocalizacion_Test()
        {
            ProyectoServicio = new ProyectoServicio(CacheServicio, new ProyectoPersistencia(_mockContextFactory.Object, _seccionCapituloPersistencia.Object, _fasePersistencia.Object));

            BeneficiarioProductoLocalizacionDto beneficiarioDto = new BeneficiarioProductoLocalizacionDto();

            beneficiarioDto.ProyectoId = 1;
            beneficiarioDto.ProductoId = 1;
            beneficiarioDto.LocalizacionId = 1;

            string result = "OK";

            try
            {
                ProyectoServicio.GuardarBeneficiarioProductoLocalizacion(beneficiarioDto, "");
            }
            catch
            {
                result = "ERROR";
            }

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GuardarBeneficiarioProductoLocalizacionCaracterizacion_Test()
        {
            ProyectoServicio = new ProyectoServicio(CacheServicio, new ProyectoPersistencia(_mockContextFactory.Object, _seccionCapituloPersistencia.Object, _fasePersistencia.Object));

            BeneficiarioProductoLocalizacionCaracterizacionDto beneficiarioDto = new BeneficiarioProductoLocalizacionCaracterizacionDto();

            beneficiarioDto.ProyectoId = 1;
            beneficiarioDto.ProductoId = 1;
            beneficiarioDto.LocalizacionId = 1;
            beneficiarioDto.Vigencia = DateTime.Now.Year;

            string result = "OK";

            try
            {
                ProyectoServicio.GuardarBeneficiarioProductoLocalizacionCaracterizacion(beneficiarioDto, "");
            }
            catch
            {
                result = "ERROR";
            }

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void ConsultarProyectosASeleccionarOk()
        {
            List<int> entidades = new List<int>();
            entidades.Add(186);
            entidades.Add(177);
            entidades.Add(105);
            entidades.Add(267);

            ParametrosProyectosDto entidadesEstados = new ParametrosProyectosDto()
            {
                tipoTramiteId = 31,
                flujoid = Guid.Parse("f562885e-3c75-d1b4-6ebc-4bcebb17ca6c"),
                IdsEntidades = entidades,

            };

            var resultados = ProyectoServicio.ConsultarProyectosASeleccionar(entidadesEstados);
            Assert.IsNotNull(resultados);


        }

        [TestMethod]
        public async Task ObtenerCRType_Test()
        {
            var result = await ProyectoServicio.ObtenerCRType();
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task ObtenerFase_Test()
        {
            var result = await ProyectoServicio.ObtenerFase();
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task ObtenerMatrizFlujo_Test()
        {
            ProyectoServicio = new ProyectoServicio(CacheServicio, new ProyectoPersistencia(_mockContextFactory.Object, _seccionCapituloPersistencia.Object, _fasePersistencia.Object));
            int idEntidad = 265;
            var result = await ProyectoServicio.ObtenerMatrizFlujo(idEntidad);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void ObtenerResumenObjetivosProductosActividades_NotNull()
        {
            ProyectoServicio = new ProyectoServicio(CacheServicio, new ProyectoPersistencia(_mockContextFactory.Object, _seccionCapituloPersistencia.Object, _fasePersistencia.Object));
            string bpin = "202200000000110";
            var result = ProyectoServicio.ObtenerResumenObjetivosProductosActividades(bpin);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void ObtenerResumenObjetivosProductosActividades_Null()
        {
            ProyectoServicio = new ProyectoServicio(CacheServicio, new ProyectoPersistencia(_mockContextFactory.Object, _seccionCapituloPersistencia.Object, _fasePersistencia.Object));
            string bpin = "202200000000110";
            var result = ProyectoServicio.ObtenerResumenObjetivosProductosActividades(bpin);
            Assert.AreEqual(result.Proyectoid, 0);
        }

        [TestMethod]
        public void ObtenerResumenObjetivosProductosActividadesJustificacion_NotNull()
        {
            ProyectoServicio = new ProyectoServicio(CacheServicio, new ProyectoPersistencia(_mockContextFactory.Object, _seccionCapituloPersistencia.Object, _fasePersistencia.Object));
            string bpin = "202200000000110";
            var result = ProyectoServicio.ObtenerResumenObjetivosProductosActividadesJustificacion(bpin);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void ObtenerResumenObjetivosProductosActividadesJustificacion_Null()
        {
            ProyectoServicio = new ProyectoServicio(CacheServicio, new ProyectoPersistencia(_mockContextFactory.Object, _seccionCapituloPersistencia.Object, _fasePersistencia.Object));
            string bpin = "202200000000110";
            var result = ProyectoServicio.ObtenerResumenObjetivosProductosActividadesJustificacion(bpin);
            Assert.AreEqual(result.Proyectoid, 0);
        }

        [TestMethod]
        public void ObtenerJustificacionLocalizacionProyecto_NotNull()
        {
            ProyectoServicio = new ProyectoServicio(CacheServicio, new ProyectoPersistencia(_mockContextFactory.Object, _seccionCapituloPersistencia.Object, _fasePersistencia.Object));
            int proyectoId = 97831;
            var result = ProyectoServicio.ObtenerJustificacionLocalizacionProyecto(proyectoId);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void OObtenerJustificacionLocalizacionProyecto_Null()
        {
            ProyectoServicio = new ProyectoServicio(CacheServicio, new ProyectoPersistencia(_mockContextFactory.Object, _seccionCapituloPersistencia.Object, _fasePersistencia.Object));
            int proyectoId = 97830;
            var result = ProyectoServicio.ObtenerJustificacionLocalizacionProyecto(proyectoId);
            Assert.AreEqual(result.ProyectoId, null);
        }

        [TestMethod]
        public void ObtenerInstanciaProyectoTramite_Test()
        {
            ProyectoServicio = new ProyectoServicio(CacheServicio, new ProyectoPersistencia(_mockContextFactory.Object, _seccionCapituloPersistencia.Object, _fasePersistencia.Object));
            string instanciaId = "3DB31A78-6E4C-4A55-B047-338E85A93A17";
            string bpin = "202200000000125";
            var result = ProyectoServicio.ObtenerInstanciaProyectoTramite(instanciaId, bpin);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void ObtenerProyectosBeneficiarios_Test()
        {
            ProyectoServicio = new ProyectoServicio(CacheServicio, new ProyectoPersistencia(_mockContextFactory.Object, _seccionCapituloPersistencia.Object, _fasePersistencia.Object));
            string bpin = "202200000000125";
            var result = ProyectoServicio.ObtenerProyectosBeneficiarios(bpin);
            Assert.IsNotNull(result);
        }

        /// <summary>
        /// Validacion previa a la devolución de un paso
        /// </summary> 
        [TestMethod]
        public void ValidacionDevolucionPaso_Test()
        {
            ProyectoServicio = new ProyectoServicio(CacheServicio, new ProyectoPersistencia(_mockContextFactory.Object, _seccionCapituloPersistencia.Object, _fasePersistencia.Object));
            Guid instanciaId = new Guid("7530279c-39fb-4f58-a416-b13f64e4e792");
            Guid accionId = new Guid("bd65670a-a4f8-e4bc-ff10-c825e76f8149");
            Guid accionDevolucionId = new Guid("7549e91b-eac9-f5f3-cacf-6c3254329229");
            string usuario = "CC2024061001";
            var result = ProyectoServicio.ValidacionDevolucionPaso(instanciaId, accionId, accionDevolucionId, usuario);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void ObtenerProyectosBeneficiariosDetalle_Test()
        {
            ProyectoServicio = new ProyectoServicio(CacheServicio, new ProyectoPersistencia(_mockContextFactory.Object, _seccionCapituloPersistencia.Object, _fasePersistencia.Object));
            string json = "202200000000125";
            var result = ProyectoServicio.ObtenerProyectosBeneficiariosDetalle(json);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void ObtenerJustificacionProyectosBeneficiarios_Test()
        {
            ProyectoServicio = new ProyectoServicio(CacheServicio, new ProyectoPersistencia(_mockContextFactory.Object, _seccionCapituloPersistencia.Object, _fasePersistencia.Object));
            string bpin = "202200000000125";
            var result = string.Empty;
            //var result = ProyectoServicio.ObtenerJustificacionProyectosBeneficiarios(bpin);
            Assert.AreEqual(result, string.Empty);
        }

        [TestMethod]
        public void GetCategoriasSubcategorias_JSON_Test()
        {
            ProyectoServicio = new ProyectoServicio(CacheServicio, new ProyectoPersistencia(_mockContextFactory.Object, _seccionCapituloPersistencia.Object, _fasePersistencia.Object));
            int padreId = 176;
            int entidadId = 168;
            int esCategoria = 1;
            int esGruposEtnicos = 1;
            var result = ProyectoServicio.GetCategoriasSubcategorias_JSON(padreId, entidadId, esCategoria, esGruposEtnicos);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void ConsultarProyectosASeleccionar_Test()
        {
            ProyectoServicio = new ProyectoServicio(CacheServicio, new ProyectoPersistencia(_mockContextFactory.Object, _seccionCapituloPersistencia.Object, _fasePersistencia.Object));
            var parametros = new ParametrosProyectosDto()
            {
                IdsEntidades = new List<int>() { 76 },
                NombresEstadosProyectos =
                                     new List<string>() { "No Disponible" },
                TokenAutorizacion = TokenAutorizacion,
                tipoTramiteId = 31,
                flujoid = Guid.Parse("f562885e-3c75-d1b4-6ebc-4bcebb17ca6c"),
            };
            var result = ProyectoServicio.ConsultarProyectosASeleccionar(parametros);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void InsertarAuditoriaEntidad_Test()
        {
            ProyectoServicio = new ProyectoServicio(CacheServicio, new ProyectoPersistencia(_mockContextFactory.Object, _seccionCapituloPersistencia.Object, _fasePersistencia.Object));
            var parametros = new AuditoriaEntidadDto()
            {
                FechaMovimiento = DateTime.Now,
                EntidadOrigenId = 265,
                EntidadOrigen = "Boyacá",
                EntidadDestinoId = 265,
                EntidadDestino = "Boyacá",
                UsuarioId = new Guid("3DB31A78-6E4C-4A55-B047-338E85A93A17"),
                Usuario = "111210012",
                Proyecto = new ProyectoEntidadDto
                {
                    ProyectoId = 97831,
                    TipoEntidadId = 1,
                    SectorId = 12,
                    EntidadId = 186,
                    FechaCreacion = DateTime.Now,
                    TieneInstancia = false,
                    TieneRecurso = false
                }
            };
            var result = ProyectoServicio.InsertarAuditoriaEntidad(parametros);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task InsertarAuditoriaEntidad_ThrownException_Test()
        {
            ProyectoServicio = new ProyectoServicio(CacheServicio, new ProyectoPersistencia(_mockContextFactory.Object, _seccionCapituloPersistencia.Object, _fasePersistencia.Object));
            var parametros = new AuditoriaEntidadDto()
            {
                FechaMovimiento = DateTime.Now,
                EntidadOrigenId = 266,
                EntidadOrigen = "Boyacá",
                EntidadDestinoId = 266,
                EntidadDestino = "Boyacá",
                UsuarioId = new Guid("3DB31A78-6E4C-4A55-B047-338E85A93A17"),
                Usuario = "111210012",
                Proyecto = new ProyectoEntidadDto
                {
                    ProyectoId = 97831,
                    TipoEntidadId = 1,
                    SectorId = 12,
                    EntidadId = 186,
                    FechaCreacion = DateTime.Now,
                    TieneInstancia = false,
                    TieneRecurso = false
                }
            };
            var result = await ProyectoServicio.InsertarAuditoriaEntidad(parametros);
            Assert.IsFalse(result.Exito);
        }

        [TestMethod]
        public void ObtenerAuditoriaEntidad_Test()
        {
            ProyectoServicio = new ProyectoServicio(CacheServicio, new ProyectoPersistencia(_mockContextFactory.Object, _seccionCapituloPersistencia.Object, _fasePersistencia.Object));
            var proyectoId = 97831;
            var result = ProyectoServicio.ObtenerAuditoriaEntidad(proyectoId);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task ObtenerAuditoriaEntidad_Test_ThrownException()
        {
            ProyectoServicio = new ProyectoServicio(CacheServicio, new ProyectoPersistencia(_mockContextFactory.Object, _seccionCapituloPersistencia.Object, _fasePersistencia.Object));
            var proyectoId = 97832;
            var result = await ProyectoServicio.ObtenerAuditoriaEntidad(proyectoId);
            Assert.IsFalse(result.Exito);
        }

        [TestMethod]
        public void ObtenerProyectoConpes_Test()
        {
            ProyectoServicio = new ProyectoServicio(CacheServicio, new ProyectoPersistencia(_mockContextFactory.Object, _seccionCapituloPersistencia.Object, new FasePersistencia(_mockContextFactory.Object)));
            var proyectoId = 97831;
            var instanciaId = new Guid("3DB31A78-6E4C-4A55-B047-338E85A93A17");
            string macroproceso = "a562885e-3c75-d1b5-6ebc-4bcebb17ca6b";
            string nivelId = "4DB31A78-6E4C-4A55-B047-338E85A93A15";
            string flujoid = "f562885e-3c75-d1b4-6ebc-4bcebb17ca6c";
            var result = ProyectoServicio.ObtenerProyectoConpes(proyectoId, instanciaId, macroproceso, nivelId, flujoid);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void ObtenerMatrizEntidadDestino_Test()
        {
            ProyectoServicio = new ProyectoServicio(CacheServicio, new ProyectoPersistencia(_mockContextFactory.Object, _seccionCapituloPersistencia.Object, _fasePersistencia.Object));
            var parameters = new ListMatrizEntidadDestinoDto
            {
                IdUsuario = "111210012",
                ListMatrizEntidad = new List<MatrizEntidadParametrosDto>
                {
                    new MatrizEntidadParametrosDto
                    {
                        EntidadResponsableId = 265,
                        ListEntidadDestinoId = new List<EntidadDestinoIdDto>
                        {
                            new EntidadDestinoIdDto
                            {
                                EntidadDestinoId = 265
                            }
                        },
                        ListSectorId = new List<SectorIdDto>
                        {
                            new SectorIdDto
                            {
                                SectorId = 24
                            }
                        }
                    }
                }
            };
            var result = ProyectoServicio.ObtenerMatrizEntidadDestino(parameters, "111210012");
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task ActualizarMatrizEntidadDestino_Insertar_Test()
        {
            ProyectoServicio = new ProyectoServicio(CacheServicio, new ProyectoPersistencia(_mockContextFactory.Object, _seccionCapituloPersistencia.Object, _fasePersistencia.Object));
            var parameters = new ListaMatrizEntidadUnidadDto
            {
                TipoFlujo = new Guid("26953581-213f-952e-bb04-41399c4bd1fa"),
                MatrizEntidadUnidad = new List<MatrizEntidadUnidadDto>
                {
                    new MatrizEntidadUnidadDto
                    {
                        EntidadResponsableId = 265,
                        EntidadResponsable = "Boyacá",
                        EntidadDestinoId = 265,
                        EntidadDestinoAccion = "Boyacá",
                        CRTypeId = 1,
                        Id = 1,
                        SectorId = 12,
                        Estado = 1,
                        Sector = "Transporte",
                        RolId = new Guid("1DD225F4-5C34-4C55-B11D-E5856A68839B")
                    }
                }
            };
            var result = await ProyectoServicio.ActualizarMatrizEntidadDestino(parameters, "111210012");
            Assert.IsTrue(result.Exito);
            Assert.AreEqual(result.Mensaje, "OK");
        }

        [TestMethod]
        public async Task ActualizarMatrizEntidadDestino_Delete_Test()
        {
            ProyectoServicio = new ProyectoServicio(CacheServicio, new ProyectoPersistencia(_mockContextFactory.Object, _seccionCapituloPersistencia.Object, _fasePersistencia.Object));
            var parameters = new ListaMatrizEntidadUnidadDto
            {
                TipoFlujo = new Guid("26953581-213f-952e-bb04-41399c4bd1fa"),
                MatrizEntidadUnidad = new List<MatrizEntidadUnidadDto>
                {
                    new MatrizEntidadUnidadDto
                    {
                        EntidadResponsableId = 265,
                        EntidadResponsable = "Boyacá",
                        EntidadDestinoId = 265,
                        EntidadDestinoAccion = "Boyacá",
                        CRTypeId = 1,
                        Id = 1,
                        SectorId = 12,
                        Estado = 2,
                        Sector = "Transporte",
                        RolId = new Guid("1DD225F4-5C34-4C55-B11D-E5856A68839B")
                    }
                }
            };
            var result = await ProyectoServicio.ActualizarMatrizEntidadDestino(parameters, "111210012");
            Assert.IsTrue(result.Exito);
            Assert.AreEqual(result.Mensaje, "OK");
        }

        [TestMethod]
        public async Task ActualizarMatrizEntidadDestino_TipoFlujoDiferente_Test()
        {
            ProyectoServicio = new ProyectoServicio(CacheServicio, new ProyectoPersistencia(_mockContextFactory.Object, _seccionCapituloPersistencia.Object, _fasePersistencia.Object));
            var parameters = new ListaMatrizEntidadUnidadDto
            {
                TipoFlujo = new Guid("26953581-213f-952e-bb04-41399c4bd1fb"),
                MatrizEntidadUnidad = new List<MatrizEntidadUnidadDto>
                {
                    new MatrizEntidadUnidadDto
                    {
                        EntidadResponsableId = 265,
                        EntidadResponsable = "Boyacá",
                        EntidadDestinoId = 265,
                        EntidadDestinoAccion = "Boyacá",
                        CRTypeId = 1,
                        Id = 1,
                        SectorId = 12,
                        Estado = 1,
                        Sector = "Transporte",
                        RolId = new Guid("1DD225F4-5C34-4C55-B11D-E5856A68839B")
                    }
                }
            };
            var result = await ProyectoServicio.ActualizarMatrizEntidadDestino(parameters, "111210012");
            Assert.IsTrue(result.Exito);
            Assert.AreEqual(result.Mensaje, "OK");
        }

        [TestMethod]
        public async Task GuardarReprogramacionPorProductoVigencia_OK()
        {
            List<ReprogramacionValores> lista = new List<ReprogramacionValores>();
            ReprogramacionValores reprogramacionValores = new ReprogramacionValores();
            reprogramacionValores.PeriodoProyectoId = 2326;
            reprogramacionValores.ProductoId = 1;
            reprogramacionValores.ProyectoId = 1525;
            reprogramacionValores.ReprogramadoNacion = 250000;
            reprogramacionValores.ReprogramadoPropios = 500000;
            reprogramacionValores.TramiteId = 25;
            lista.Add(reprogramacionValores);
            string usuario = "CC202002";

            var result = await ProyectoServicio.GuardarReprogramacionPorProductoVigencia(lista,  usuario);
            Assert.IsTrue(result.Exito);
            
        }

        [TestMethod]
        public async Task GuardarReprogramacionPorProductoVigencia_Null()
        {
            List<ReprogramacionValores> lista = new List<ReprogramacionValores>();
            lista.Add(new ReprogramacionValores());
            
            string usuario = "CC202002";

            var result = await ProyectoServicio.GuardarReprogramacionPorProductoVigencia(lista, usuario);
            Assert.IsFalse(result.Exito);
           
        }


    }
}
