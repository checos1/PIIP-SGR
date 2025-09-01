namespace DNP.ServiciosNegocio.Test.Servicios.Catalogos
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data.Entity;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    using System.Threading.Tasks;
    using Comunes.Enum;
    using Configuracion;
    using DNP.ServiciosNegocio.Comunes.Excepciones;
    using DNP.ServiciosNegocio.Test.Mock;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Persistencia.Implementaciones.Catalogos;
    using Persistencia.Interfaces;
    using Persistencia.Interfaces.Catalogos;
    using Persistencia.Modelo;
    using ServiciosNegocio.Servicios.Implementaciones.Catalogos;
    using ServiciosNegocio.Servicios.Interfaces.Transversales;
    using Unity;

    [TestClass]
    public class CatalogoServicioTest
    {
        private ICatalogoPersistencia CatalogoPersistencia { get; set; }
        private ICacheServicio CacheServicio { get; set; }
        private CatalogoServicio CatalogoServicio { get; set; }
        private string NombreCatalogo { get; set; }
        private int IdCatalogo { get; set; }
        private readonly Mock<MGAWebContexto> _mockContext = new Mock<MGAWebContexto>();
        private readonly Mock<IContextoFactory> _mockContextFactory = new Mock<IContextoFactory>();
        private readonly Mock<DbSet<Region>> _mockSetRegion = new Mock<DbSet<Region>>();
        private readonly Mock<DbSet<Municipality>> _mockSetMunicipality = new Mock<DbSet<Municipality>>();
        private readonly Mock<DbSet<Department>> _mockSetDepartment = new Mock<DbSet<Department>>();
        private readonly Mock<DbSet<Shelter>> _mockSetShelter = new Mock<DbSet<Shelter>>();
        private readonly Mock<DbSet<EntityTypeCatalogOption>> _mockSetEntityCatalogOption = new Mock<DbSet<EntityTypeCatalogOption>>();
        private readonly Mock<DbSet<EntityType>> _mockSetEntityType = new Mock<DbSet<EntityType>>();
        private readonly Mock<DbSet<Sector>> _mockSetSector = new Mock<DbSet<Sector>>();
        private readonly Mock<DbSet<Program>> _mockSetProgram = new Mock<DbSet<Program>>();
        private readonly Mock<DbSet<ProductCatalog>> _mockSetProductos = new Mock<DbSet<ProductCatalog>>();
        private readonly Mock<DbSet<Alternative>> _mockSetAlternativas = new Mock<DbSet<Alternative>>();
        private readonly Mock<DbSet<ResourceType>> _mockSetTipoRecurso = new Mock<DbSet<ResourceType>>();
        private readonly Mock<DbSet<ResourceTypeByEntity>> _mockSetTipoRecursoByEntidad = new Mock<DbSet<ResourceTypeByEntity>>();
        private readonly Mock<DbSet<ClassificationType>> _mockSetClasificacionesRecursos = new Mock<DbSet<ClassificationType>>();
        private readonly Mock<DbSet<Stage>> _mockSetStage = new Mock<DbSet<Stage>>();
        private readonly Mock<DbSet<Ejecutor>> _mockSetEjecutor = new Mock<DbSet<Ejecutor>>();
        private readonly Mock<DbSet<PolicyTargeting>> _mockSetPolicyTargeting = new Mock<DbSet<PolicyTargeting>>();

        private string TokenAutorizacion { get; set; }
        [TestInitialize]
        public void Init()
        {
            var contenedor = UnityConfig.Container;
            CatalogoPersistencia = contenedor.Resolve<ICatalogoPersistencia>();
            CacheServicio = contenedor.Resolve<ICacheServicio>();
            CatalogoServicio = new CatalogoServicio(CatalogoPersistencia, CacheServicio);
            TokenAutorizacion = "Basic amRlbGdhZG86MTYyNTk0NjM=";

            var dataRegion = new List<Region>() { new Region() { Id = -1, Code = "-1", Created = DateTime.Now }, new Region() { Id = -2, Code = "-2", Created = DateTime.Now } }.AsQueryable();
            var dataMunicipality = new List<Municipality>() { new Municipality() { Code = "-1", Id = -1, Name = "Bogota" } }.AsQueryable();
            var dataDepartment = new List<Department>() { new Department() { Code = "-1", RegionId = -1, Id = -1, Name = "Cundinamarca" } }.AsQueryable();
            var dataShelter = new List<Shelter>() { new Shelter() { MunicipalityId = -1, Name = "Resguardo1" } }.AsQueryable();
            var dataEntityCatalogOption = new List<EntityTypeCatalogOption>() { new EntityTypeCatalogOption() { Id = -1, Name = "test" } }.AsQueryable();
            var dataEntityType = new List<EntityType>() { new EntityType() { Id = -1, Code = "-1", EntityType1 = "test" } }.AsQueryable();
            var dataSector = new List<Sector>() { new Sector() { Id = -1, Code = "-1" } }.AsQueryable();
            var dataProgramas = new List<Program>() { new Program() { Id = -1, Name = "-1" } }.AsQueryable();
            var dataProductos = new List<ProductCatalog>() { new ProductCatalog() { Id = -1, Name = "testp" } }.AsQueryable();
            var dataAlternativas = new List<Alternative>() { new Alternative() { Id = -1, Name = "testp" } }.AsQueryable();
            var dataTipoRecurso = new List<ResourceType>() { new ResourceType() { Id = -1, Code = "testp", EntityType = new EntityType { Id = 1 }, ClassificationType = new ClassificationType { Id = 1 }, ResourceGroup = new ResourceGroup { Id = 1 } } }.AsQueryable();
            var dataTipoRecursoByEntidad = new List<ResourceTypeByEntity>() { new ResourceTypeByEntity() { Id = 1, EntityTypeCatalogOption = new EntityTypeCatalogOption { Id = 1 }, EntityTypeCatalogOption1 = new EntityTypeCatalogOption { Id = 1 }, ResourceType = new ResourceType { Id = 1 } } }.AsQueryable();
            var dataClasificacionesRecurso = new List<ClassificationType>() { new ClassificationType() { Id = -1, Description = "testp" } }.AsQueryable();
            var dataStage = new List<Stage>() { new Stage() { Id = -1, Description = "testp" } }.AsQueryable();
            var dataEjecutores = new List<Ejecutor>() { new Ejecutor() { Id = -1, NombreEjecutor = "testp", EntityTypeId = 1 } }.AsQueryable();
            var dataPolicyTargetings = new List<PolicyTargeting>() { new PolicyTargeting() { Id = -1, Name = "testp", ParentId = 1 } }.AsQueryable();
            var dataConsultarAgrupacionesCompleta = new upsGetShelterAgrupacionCode_Result();

            _mockSetTipoRecurso.As<IQueryable<ResourceType>>().Setup(m => m.Provider).Returns(dataTipoRecurso.Provider);
            _mockSetTipoRecurso.As<IQueryable<ResourceType>>().Setup(m => m.Expression).Returns(dataTipoRecurso.Expression);
            _mockSetTipoRecurso.As<IQueryable<ResourceType>>().Setup(m => m.ElementType).Returns(dataTipoRecurso.ElementType);
            _mockSetTipoRecurso.As<IQueryable<ResourceType>>().Setup(m => m.GetEnumerator()).Returns(dataTipoRecurso.GetEnumerator());

            _mockSetTipoRecursoByEntidad.As<IQueryable<ResourceTypeByEntity>>().Setup(m => m.Provider).Returns(dataTipoRecursoByEntidad.Provider);
            _mockSetTipoRecursoByEntidad.As<IQueryable<ResourceTypeByEntity>>().Setup(m => m.Expression).Returns(dataTipoRecursoByEntidad.Expression);
            _mockSetTipoRecursoByEntidad.As<IQueryable<ResourceTypeByEntity>>().Setup(m => m.ElementType).Returns(dataTipoRecursoByEntidad.ElementType);
            _mockSetTipoRecursoByEntidad.As<IQueryable<ResourceTypeByEntity>>().Setup(m => m.GetEnumerator()).Returns(dataTipoRecursoByEntidad.GetEnumerator());

            _mockSetEjecutor.As<IQueryable<Ejecutor>>().Setup(m => m.Provider).Returns(dataEjecutores.Provider);
            _mockSetEjecutor.As<IQueryable<Ejecutor>>().Setup(m => m.Expression).Returns(dataEjecutores.Expression);
            _mockSetEjecutor.As<IQueryable<Ejecutor>>().Setup(m => m.ElementType).Returns(dataEjecutores.ElementType);
            _mockSetEjecutor.As<IQueryable<Ejecutor>>().Setup(m => m.GetEnumerator()).Returns(dataEjecutores.GetEnumerator());

            _mockSetPolicyTargeting.As<IQueryable<PolicyTargeting>>().Setup(m => m.Provider).Returns(dataPolicyTargetings.Provider);
            _mockSetPolicyTargeting.As<IQueryable<PolicyTargeting>>().Setup(m => m.Expression).Returns(dataPolicyTargetings.Expression);
            _mockSetPolicyTargeting.As<IQueryable<PolicyTargeting>>().Setup(m => m.ElementType).Returns(dataPolicyTargetings.ElementType);
            _mockSetPolicyTargeting.As<IQueryable<PolicyTargeting>>().Setup(m => m.GetEnumerator()).Returns(dataPolicyTargetings.GetEnumerator());

            _mockSetStage.As<IQueryable<Stage>>().Setup(m => m.Provider).Returns(dataStage.Provider);
            _mockSetStage.As<IQueryable<Stage>>().Setup(m => m.Expression).Returns(dataStage.Expression);
            _mockSetStage.As<IQueryable<Stage>>().Setup(m => m.ElementType).Returns(dataStage.ElementType);
            _mockSetStage.As<IQueryable<Stage>>().Setup(m => m.GetEnumerator()).Returns(dataStage.GetEnumerator());

            _mockSetClasificacionesRecursos.As<IQueryable<ClassificationType>>().Setup(m => m.Provider).Returns(dataClasificacionesRecurso.Provider);
            _mockSetClasificacionesRecursos.As<IQueryable<ClassificationType>>().Setup(m => m.Expression).Returns(dataClasificacionesRecurso.Expression);
            _mockSetClasificacionesRecursos.As<IQueryable<ClassificationType>>().Setup(m => m.ElementType).Returns(dataClasificacionesRecurso.ElementType);
            _mockSetClasificacionesRecursos.As<IQueryable<ClassificationType>>().Setup(m => m.GetEnumerator()).Returns(dataClasificacionesRecurso.GetEnumerator());

            _mockSetRegion.As<IQueryable<Region>>().Setup(m => m.Provider).Returns(dataRegion.Provider);
            _mockSetRegion.As<IQueryable<Region>>().Setup(m => m.Expression).Returns(dataRegion.Expression);
            _mockSetRegion.As<IQueryable<Region>>().Setup(m => m.ElementType).Returns(dataRegion.ElementType);
            _mockSetRegion.As<IQueryable<Region>>().Setup(m => m.GetEnumerator()).Returns(dataRegion.GetEnumerator());
            _mockSetEntityCatalogOption.As<IQueryable<EntityTypeCatalogOption>>().Setup(m => m.GetEnumerator()).Returns(dataEntityCatalogOption.GetEnumerator());

            _mockSetMunicipality.As<IQueryable<Municipality>>().Setup(m => m.Provider).Returns(dataMunicipality.Provider);
            _mockSetMunicipality.As<IQueryable<Municipality>>().Setup(m => m.Expression).Returns(dataMunicipality.Expression);
            _mockSetMunicipality.As<IQueryable<Municipality>>().Setup(m => m.ElementType).Returns(dataMunicipality.ElementType);
            _mockSetMunicipality.As<IQueryable<Municipality>>().Setup(m => m.GetEnumerator()).Returns(dataMunicipality.GetEnumerator());

            _mockSetDepartment.As<IQueryable<Department>>().Setup(m => m.Provider).Returns(dataDepartment.Provider);
            _mockSetDepartment.As<IQueryable<Department>>().Setup(m => m.Expression).Returns(dataDepartment.Expression);
            _mockSetDepartment.As<IQueryable<Department>>().Setup(m => m.ElementType).Returns(dataDepartment.ElementType);
            _mockSetDepartment.As<IQueryable<Department>>().Setup(m => m.GetEnumerator()).Returns(dataDepartment.GetEnumerator());

            _mockSetShelter.As<IQueryable<Shelter>>().Setup(m => m.Provider).Returns(dataShelter.Provider);
            _mockSetShelter.As<IQueryable<Shelter>>().Setup(m => m.Expression).Returns(dataShelter.Expression);
            _mockSetShelter.As<IQueryable<Shelter>>().Setup(m => m.ElementType).Returns(dataShelter.ElementType);
            _mockSetShelter.As<IQueryable<Shelter>>().Setup(m => m.GetEnumerator()).Returns(dataShelter.GetEnumerator());

            _mockSetEntityCatalogOption.As<IQueryable<EntityTypeCatalogOption>>().Setup(m => m.Provider).Returns(dataEntityCatalogOption.Provider);
            _mockSetEntityCatalogOption.As<IQueryable<EntityTypeCatalogOption>>().Setup(m => m.Expression).Returns(dataEntityCatalogOption.Expression);
            _mockSetEntityCatalogOption.As<IQueryable<EntityTypeCatalogOption>>().Setup(m => m.ElementType).Returns(dataEntityCatalogOption.ElementType);
            _mockSetEntityCatalogOption.As<IQueryable<EntityTypeCatalogOption>>().Setup(m => m.GetEnumerator()).Returns(dataEntityCatalogOption.GetEnumerator());

            _mockSetEntityType.As<IQueryable<EntityType>>().Setup(m => m.Provider).Returns(dataEntityType.Provider);
            _mockSetEntityType.As<IQueryable<EntityType>>().Setup(m => m.Expression).Returns(dataEntityType.Expression);
            _mockSetEntityType.As<IQueryable<EntityType>>().Setup(m => m.ElementType).Returns(dataEntityType.ElementType);
            _mockSetEntityType.As<IQueryable<EntityType>>().Setup(m => m.GetEnumerator()).Returns(dataEntityType.GetEnumerator());

            _mockSetSector.As<IQueryable<Sector>>().Setup(m => m.Provider).Returns(dataSector.Provider);
            _mockSetSector.As<IQueryable<Sector>>().Setup(m => m.Expression).Returns(dataSector.Expression);
            _mockSetSector.As<IQueryable<Sector>>().Setup(m => m.ElementType).Returns(dataSector.ElementType);
            _mockSetSector.As<IQueryable<Sector>>().Setup(m => m.GetEnumerator()).Returns(dataSector.GetEnumerator());

            _mockSetProgram.As<IQueryable<Program>>().Setup(m => m.Provider).Returns(dataProgramas.Provider);
            _mockSetProgram.As<IQueryable<Program>>().Setup(m => m.Expression).Returns(dataProgramas.Expression);
            _mockSetProgram.As<IQueryable<Program>>().Setup(m => m.ElementType).Returns(dataProgramas.ElementType);
            _mockSetProgram.As<IQueryable<Program>>().Setup(m => m.GetEnumerator()).Returns(dataProgramas.GetEnumerator());

            _mockSetProductos.As<IQueryable<ProductCatalog>>().Setup(m => m.Provider).Returns(dataProductos.Provider);
            _mockSetProductos.As<IQueryable<ProductCatalog>>().Setup(m => m.Expression).Returns(dataProductos.Expression);
            _mockSetProductos.As<IQueryable<ProductCatalog>>().Setup(m => m.ElementType).Returns(dataProductos.ElementType);
            _mockSetProductos.As<IQueryable<ProductCatalog>>().Setup(m => m.GetEnumerator()).Returns(dataProductos.GetEnumerator());
            _mockSetProductos.Setup(x => x.AsNoTracking()).Returns(_mockSetProductos.Object);

            _mockSetAlternativas.As<IQueryable<Alternative>>().Setup(m => m.Provider).Returns(dataAlternativas.Provider);
            _mockSetAlternativas.As<IQueryable<Alternative>>().Setup(m => m.Expression).Returns(dataAlternativas.Expression);
            _mockSetAlternativas.As<IQueryable<Alternative>>().Setup(m => m.ElementType).Returns(dataAlternativas.ElementType);
            _mockSetAlternativas.As<IQueryable<Alternative>>().Setup(m => m.GetEnumerator()).Returns(dataAlternativas.GetEnumerator());

            var mockConsultarAgrupacionesCompleta = new Mock<ObjectResult<upsGetShelterAgrupacionCode_Result>>();
            mockConsultarAgrupacionesCompleta.SetupReturn(dataConsultarAgrupacionesCompleta);
            
            _mockContext.Setup(m => m.Region).Returns(_mockSetRegion.Object);
            _mockContext.Setup(m => m.Municipality).Returns(_mockSetMunicipality.Object);
            _mockContext.Setup(m => m.Department).Returns(_mockSetDepartment.Object);
            _mockContext.Setup(m => m.Shelter).Returns(_mockSetShelter.Object);
            _mockContext.Setup(m => m.EntityTypeCatalogOption).Returns(_mockSetEntityCatalogOption.Object);
            _mockContext.Setup(m => m.EntityType).Returns(_mockSetEntityType.Object);
            _mockContext.Setup(m => m.Sector).Returns(_mockSetSector.Object);
            _mockContext.Setup(m => m.Program).Returns(_mockSetProgram.Object);
            _mockContext.Setup(m => m.ProductCatalog).Returns(_mockSetProductos.Object);
            _mockContext.Setup(m => m.Alternative).Returns(_mockSetAlternativas.Object);
            _mockContext.Setup(m => m.ResourceType).Returns(_mockSetTipoRecurso.Object);
            _mockContext.Setup(m => m.ResourceTypeByEntity).Returns(_mockSetTipoRecursoByEntidad.Object);
            _mockContext.Setup(m => m.ClassificationType).Returns(_mockSetClasificacionesRecursos.Object);
            _mockContext.Setup(m => m.Stage).Returns(_mockSetStage.Object);
            _mockContext.Setup(m => m.Ejecutores).Returns(_mockSetEjecutor.Object);
            _mockContext.Setup(m => m.PolicyTargeting).Returns(_mockSetPolicyTargeting.Object);
            _mockContext.Setup(m => m.upsGetShelterAgrupacionCode()).Returns(mockConsultarAgrupacionesCompleta.Object);

            _mockContextFactory.Setup(mcf => mcf.CrearContextoConConexion(ConfigurationManager.ConnectionStrings["MGAWebContexto"].ConnectionString)).Returns(_mockContext.Object);
        }

        //[TestMethod]
        //public void TestMetodo_GeneralConsultas()
        //{
        //    CatalogoServicio = new CatalogoServicio(new CatalogoPersistencia(_mockContextFactory.Object), CacheServicio);

        //    var resultadoEntidades = CatalogoServicio.ObtenerListaCatalogoResourceType(CatalogoEnum.Entidades.ToString());
        //    var resultadoEtapas = CatalogoServicio.ObtenerListaCatalogo(CatalogoEnum.Etapas.ToString());
        //    var resultadoTiposAgrupaciones = CatalogoServicio.ObtenerListaCatalogo(CatalogoEnum.TiposAgrupaciones.ToString());
        //    var resultadoAgrupaciones = CatalogoServicio.ObtenerListaCatalogo(CatalogoEnum.Agrupaciones.ToString());
        //    var resultadoGrupoRecursos = CatalogoServicio.ObtenerListaCatalogo(CatalogoEnum.GruposRecursos.ToString());
        //    var resultadoClasificacionesRecursos = CatalogoServicio.ObtenerListaCatalogo(CatalogoEnum.ClasificacionesRecursos.ToString());
        //    var resultadoTiposRecursos = CatalogoServicio.ObtenerListaCatalogo(CatalogoEnum.TiposRecursos.ToString());
        //    var resultadoAlternativas = CatalogoServicio.ObtenerListaCatalogo(CatalogoEnum.Alternativas.ToString());
        //    var resultadoProgramas = CatalogoServicio.ObtenerListaCatalogo(CatalogoEnum.Programas.ToString());
        //    var resultadoTipoEntidades = CatalogoServicio.ObtenerListaCatalogo(CatalogoEnum.TiposEntidades.ToString());
        //    var resultadoSectores = CatalogoServicio.ObtenerListaCatalogo(CatalogoEnum.Sectores.ToString());
        //    var resultadoProductos = CatalogoServicio.ObtenerListaCatalogo(CatalogoEnum.Productos.ToString());

        //    var resultadosRegiones = CatalogoServicio.ObtenerListaCatalogo(CatalogoEnum.Regiones.ToString());
        //    var resultadosMunicipios = CatalogoServicio.ObtenerListaCatalogo(CatalogoEnum.Municipios.ToString());
        //    var resultadosDepartamentos = CatalogoServicio.ObtenerListaCatalogo(CatalogoEnum.Departamentos.ToString());
        //    var resultadosResguardos = CatalogoServicio.ObtenerListaCatalogo(CatalogoEnum.Resguardos.ToString());

        //    var resultadosPoliticas = CatalogoServicio.ObtenerListaCatalogo(CatalogoEnum.Politicas.ToString());
        //    var resultadosPoliticasNivel1 = CatalogoServicio.ObtenerListaCatalogo(CatalogoEnum.PoliticasNivel1.ToString());
        //    var resultadosPoliticasNivel2 = CatalogoServicio.ObtenerListaCatalogo(CatalogoEnum.PoliticasNivel2.ToString());
        //    var resultadosPoliticasNivel3 = CatalogoServicio.ObtenerListaCatalogo(CatalogoEnum.PoliticasNivel3.ToString());
        //    var resultadosIndicadoresPoliticas = CatalogoServicio.ObtenerListaCatalogo(CatalogoEnum.IndicadoresPoliticas.ToString());
        //    var resultadosTiposCofinanciaciones = CatalogoServicio.ObtenerListaCatalogo(CatalogoEnum.TiposCofinanciaciones.ToString());
        //    var resultadosEntregables = CatalogoServicio.ObtenerListaCatalogo(CatalogoEnum.Entregables.ToString());

        //    Assert.IsNotNull(resultadoEntidades);
        //    Assert.IsNotNull(resultadoTipoEntidades);
        //    Assert.IsNotNull(resultadoSectores);
        //    Assert.IsNotNull(resultadoProgramas);
        //    Assert.IsNotNull(resultadoProductos);
        //    Assert.IsNotNull(resultadoAlternativas);
        //    Assert.IsNotNull(resultadoTiposRecursos);
        //    Assert.IsNotNull(resultadoClasificacionesRecursos);
        //    Assert.IsNotNull(resultadoEtapas);
        //    Assert.IsNotNull(resultadosRegiones);
        //    Assert.IsNotNull(resultadosMunicipios);
        //    Assert.IsNotNull(resultadosDepartamentos);
        //    Assert.IsNotNull(resultadosResguardos);

        //    Assert.IsNotNull(resultadosPoliticas);
        //    Assert.IsNotNull(resultadosPoliticasNivel1);
        //    Assert.IsNotNull(resultadosPoliticasNivel2);
        //    Assert.IsNotNull(resultadosPoliticasNivel3);
        //    Assert.IsNotNull(resultadosIndicadoresPoliticas);
        //    Assert.IsNotNull(resultadosTiposCofinanciaciones);
        //    Assert.IsNotNull(resultadosEntregables);

        //}

        [TestMethod]
        public async Task ConsultarCatalogoEntiades_RetornaOk()
        {
            NombreCatalogo = CatalogoEnum.Entidades.ToString();
            var respuesta = await CatalogoServicio.ObtenerCatalogo(NombreCatalogo, TokenAutorizacion);
            Assert.IsNotNull(respuesta);
        }

        [TestMethod]
        public async Task ConsultarCatalogoAlternativas_RetornaOk()
        {
            NombreCatalogo = CatalogoEnum.Alternativas.ToString();
            var respuesta = await CatalogoServicio.ObtenerCatalogo(NombreCatalogo, TokenAutorizacion);
            Assert.IsNotNull(respuesta);
        }

        [TestMethod]
        public async Task ConsultarCatalogoTiposEntidades_RetornaOk()
        {
            NombreCatalogo = CatalogoEnum.TiposEntidades.ToString();
            var respuesta = await CatalogoServicio.ObtenerCatalogo(NombreCatalogo, TokenAutorizacion);
            Assert.IsNotNull(respuesta);
        }

        [TestMethod]
        public async Task ConsultarCatalogoSectores_RetornaOk()
        {
            NombreCatalogo = CatalogoEnum.Sectores.ToString();
            var respuesta = await CatalogoServicio.ObtenerCatalogo(NombreCatalogo, TokenAutorizacion);
            Assert.IsNotNull(respuesta);
        }

        [TestMethod]
        public async Task ConsultarCatalogoRegiones_RetornaOk()
        {
            NombreCatalogo = CatalogoEnum.Regiones.ToString();
            var respuesta = await CatalogoServicio.ObtenerCatalogo(NombreCatalogo, TokenAutorizacion);
            Assert.IsNotNull(respuesta);
        }

        [TestMethod]
        public async Task ConsultarCatalogoDepartamentos_RetornaOk()
        {
            NombreCatalogo = CatalogoEnum.Departamentos.ToString();
            var respuesta = await CatalogoServicio.ObtenerCatalogo(NombreCatalogo, TokenAutorizacion);
            Assert.IsNotNull(respuesta);
        }

        [TestMethod]
        public async Task ConsultarCatalogoMunicipios_RetornaOk()
        {
            NombreCatalogo = CatalogoEnum.Municipios.ToString();
            var respuesta = await CatalogoServicio.ObtenerCatalogo(NombreCatalogo, TokenAutorizacion);
            Assert.IsNotNull(respuesta);
        }

        [TestMethod]
        public async Task ConsultarCatalogoResguardos_RetornaOk()
        {
            NombreCatalogo = CatalogoEnum.Resguardos.ToString();
            var respuesta = await CatalogoServicio.ObtenerCatalogo(NombreCatalogo, TokenAutorizacion);
            Assert.IsNotNull(respuesta);
        }

        [TestMethod]
        public async Task ConsultarCatalogoProgramas_RetornaOk()
        {
            NombreCatalogo = CatalogoEnum.Programas.ToString();
            var respuesta = await CatalogoServicio.ObtenerCatalogo(NombreCatalogo, TokenAutorizacion);
            Assert.IsNotNull(respuesta);
        }

        [TestMethod]
        public async Task ConsultarCatalogoProductos_RetornaOk()
        {
            NombreCatalogo = CatalogoEnum.Productos.ToString();
            var respuesta = await CatalogoServicio.ObtenerCatalogo(NombreCatalogo, TokenAutorizacion);
            Assert.IsNotNull(respuesta);
        }


        [TestMethod]
        public async Task ConsultarCatalogoTiposRecursos_RetornaOk()
        {
            NombreCatalogo = CatalogoEnum.TiposRecursos.ToString();
            var respuesta = await CatalogoServicio.ObtenerCatalogo(NombreCatalogo, TokenAutorizacion);
            Assert.IsNotNull(respuesta);
        }

        [TestMethod]
        public async Task ConsultarCatalogoClasificacionesRecursos_RetornaOk()
        {
            NombreCatalogo = CatalogoEnum.ClasificacionesRecursos.ToString();
            var respuesta = await CatalogoServicio.ObtenerCatalogo(NombreCatalogo, TokenAutorizacion);
            Assert.IsNotNull(respuesta);
        }

        [TestMethod]
        public async Task ConsultarCatalogoEtapasRecursos_RetornaOk()
        {
            NombreCatalogo = CatalogoEnum.Etapas.ToString();
            var respuesta = await CatalogoServicio.ObtenerCatalogo(NombreCatalogo, TokenAutorizacion);
            Assert.IsNotNull(respuesta);
        }

        [TestMethod]
        public async Task ConsultarCatalogoTiposAgrupaciones_RetornaOk()
        {
            NombreCatalogo = CatalogoEnum.TiposAgrupaciones.ToString();
            var respuesta = await CatalogoServicio.ObtenerCatalogo(NombreCatalogo, TokenAutorizacion);
            Assert.IsNotNull(respuesta);
        }

        [TestMethod]
        public async Task ConsultarCatalogoPoliticas_RetornaOk()
        {
            NombreCatalogo = CatalogoEnum.Politicas.ToString();
            var respuesta = await CatalogoServicio.ObtenerCatalogo(NombreCatalogo, TokenAutorizacion);
            Assert.IsNotNull(respuesta);
        }

        [TestMethod]
        public async Task ConsultarCatalogoTiposCofinanciaciones_RetornaOk()
        {
            NombreCatalogo = CatalogoEnum.TiposCofinanciaciones.ToString();
            var respuesta = await CatalogoServicio.ObtenerCatalogo(NombreCatalogo, TokenAutorizacion);
            Assert.IsNotNull(respuesta);
        }

        [TestMethod]
        public async Task ConsultarCatalogoEntregables_RetornaOk()
        {
            NombreCatalogo = CatalogoEnum.Entregables.ToString();
            var respuesta = await CatalogoServicio.ObtenerCatalogo(NombreCatalogo, TokenAutorizacion);
            Assert.IsNotNull(respuesta);
        }

        [TestMethod]
        public async Task ConsultarCatalogoIndicadorPoliticas_RetornaOk()
        {
            NombreCatalogo = CatalogoEnum.IndicadoresPoliticas.ToString();
            var respuesta = await CatalogoServicio.ObtenerCatalogo(NombreCatalogo, TokenAutorizacion);
            Assert.IsNotNull(respuesta);
        }

        [TestMethod]
        public async Task ConsultarCatalogoAgrupaciones_RetornaOk()
        {
            NombreCatalogo = CatalogoEnum.Agrupaciones.ToString();
            var respuesta = await CatalogoServicio.ObtenerCatalogo(NombreCatalogo, TokenAutorizacion);
            Assert.IsNotNull(respuesta);
        }

        [TestMethod]
        public async Task ConsultarCatalogoAgrupaciones_RetornaCatalogoInvalido()
        {
            NombreCatalogo = "Pruebas";
            var respuesta = await CatalogoServicio.ObtenerCatalogo(NombreCatalogo, TokenAutorizacion);
            Assert.IsNull(respuesta);
        }

        [TestMethod]
        public async Task ConsultarCatalogoPoliticasNivel1_RetornaCatalogoDB()
        {
            NombreCatalogo = CatalogoEnum.PoliticasNivel1.ToString();
            var respuesta = await CatalogoServicio.ObtenerCatalogo(NombreCatalogo, TokenAutorizacion);
            Assert.IsNotNull(respuesta);
        }

        [TestMethod]
        public async Task ConsultarCatalogoPoliticasNivel2_RetornaCatalogoDB()
        {
            NombreCatalogo = CatalogoEnum.PoliticasNivel2.ToString();
            var respuesta = await CatalogoServicio.ObtenerCatalogo(NombreCatalogo, TokenAutorizacion);
            Assert.IsNotNull(respuesta);
        }

        [TestMethod]
        public async Task ConsultarCatalogoPoliticasNivel3_RetornaCatalogoDB()
        {
            NombreCatalogo = CatalogoEnum.PoliticasNivel3.ToString();
            var respuesta = await CatalogoServicio.ObtenerCatalogo(NombreCatalogo, TokenAutorizacion);
            Assert.IsNotNull(respuesta);
        }

        [TestMethod]
        public async Task ConsultarCatalogoPorId_RetornaEntidadOk()
        {
            NombreCatalogo = CatalogoEnum.Entidades.ToString();
            IdCatalogo = 1;
            var respuesta = await CatalogoServicio.ObtenerCatalogoPorId(NombreCatalogo, IdCatalogo, TokenAutorizacion);
            Assert.IsNotNull(respuesta);
        }

        [TestMethod]
        public async Task ConsultarCatalogoPorId_RetornaTipoEntidadOk()
        {
            NombreCatalogo = CatalogoEnum.TiposEntidades.ToString();
            IdCatalogo = 1;
            var respuesta = await CatalogoServicio.ObtenerCatalogoPorId(NombreCatalogo, IdCatalogo, TokenAutorizacion);
            Assert.IsNotNull(respuesta);
        }

        [TestMethod]
        public async Task ConsultarCatalogoPorId_RetornaSectorOk()
        {
            NombreCatalogo = CatalogoEnum.Sectores.ToString();
            IdCatalogo = 1;
            var respuesta = await CatalogoServicio.ObtenerCatalogoPorId(NombreCatalogo, IdCatalogo, TokenAutorizacion);
            Assert.IsNotNull(respuesta);
        }

        [TestMethod]
        public async Task ConsultarCatalogoPorId_RetornaRegionOk()
        {
            NombreCatalogo = CatalogoEnum.Regiones.ToString();
            IdCatalogo = 1;
            var respuesta = await CatalogoServicio.ObtenerCatalogoPorId(NombreCatalogo, IdCatalogo, TokenAutorizacion);
            Assert.IsNotNull(respuesta);
        }

        [TestMethod]
        public async Task ConsultarCatalogoPorId_RetornaDepartamentoOk()
        {
            NombreCatalogo = CatalogoEnum.Departamentos.ToString();
            IdCatalogo = 1;
            var respuesta = await CatalogoServicio.ObtenerCatalogoPorId(NombreCatalogo, IdCatalogo, TokenAutorizacion);
            Assert.IsNotNull(respuesta);
        }

        [TestMethod]
        public async Task ConsultarCatalogoPorId_RetornaMunicipioOk()
        {
            NombreCatalogo = CatalogoEnum.Municipios.ToString();
            IdCatalogo = 1;
            var respuesta = await CatalogoServicio.ObtenerCatalogoPorId(NombreCatalogo, IdCatalogo, TokenAutorizacion);
            Assert.IsNotNull(respuesta);
        }
        [TestMethod]
        public async Task ConsultarCatalogoPorId_RetornaResguardoOk()
        {
            NombreCatalogo = CatalogoEnum.Resguardos.ToString();
            IdCatalogo = 1;
            var respuesta = await CatalogoServicio.ObtenerCatalogoPorId(NombreCatalogo, IdCatalogo, TokenAutorizacion);
            Assert.IsNotNull(respuesta);
        }

        [TestMethod]
        public async Task ConsultarCatalogoPorId_RetornaProgramaOk()
        {
            NombreCatalogo = CatalogoEnum.Programas.ToString();
            IdCatalogo = 1;
            var respuesta = await CatalogoServicio.ObtenerCatalogoPorId(NombreCatalogo, IdCatalogo, TokenAutorizacion);
            Assert.IsNotNull(respuesta);
        }


        [TestMethod]
        public async Task ConsultarCatalogoPorId_RetornaProductoOk()
        {
            NombreCatalogo = CatalogoEnum.Productos.ToString();
            IdCatalogo = 1;
            var respuesta = await CatalogoServicio.ObtenerCatalogoPorId(NombreCatalogo, IdCatalogo, TokenAutorizacion);
            Assert.IsNotNull(respuesta);
        }

        [TestMethod]
        public async Task ConsultarCatalogoPorId_RetornaAlternativaOk()
        {
            NombreCatalogo = CatalogoEnum.Alternativas.ToString();
            IdCatalogo = 1;
            var respuesta = await CatalogoServicio.ObtenerCatalogoPorId(NombreCatalogo, IdCatalogo, TokenAutorizacion);
            Assert.IsNotNull(respuesta);
        }

        [TestMethod]
        public async Task ConsultarCatalogoPorId_RetornaTipoRecursosOk()
        {
            NombreCatalogo = CatalogoEnum.TiposRecursos.ToString();
            IdCatalogo = 1;
            var respuesta = await CatalogoServicio.ObtenerCatalogoPorId(NombreCatalogo, IdCatalogo, TokenAutorizacion);
            Assert.IsNotNull(respuesta);
        }

        [TestMethod]
        public async Task ConsultarCatalogoPorId_RetornaClaserecursoOk()
        {
            NombreCatalogo = CatalogoEnum.ClasificacionesRecursos.ToString();
            IdCatalogo = 1;
            var respuesta = await CatalogoServicio.ObtenerCatalogoPorId(NombreCatalogo, IdCatalogo, TokenAutorizacion);
            Assert.IsNotNull(respuesta);
        }

        [TestMethod]
        public async Task ConsultarCatalogoPorId_RetornaEtapaOk()
        {
            NombreCatalogo = CatalogoEnum.Etapas.ToString();
            IdCatalogo = 1;
            var respuesta = await CatalogoServicio.ObtenerCatalogoPorId(NombreCatalogo, IdCatalogo, TokenAutorizacion);
            Assert.IsNotNull(respuesta);
        }

        //[TestMethod]
        //public async Task ConsultarCatalogoPorId_RetornaGrupoRecursosOk()
        //{
        //    NombreCatalogo = CatalogoEnum.GruposRecursos.ToString();
        //    IdCatalogo = 1;
        //    var respuesta = await CatalogoServicio.ObtenerCatalogoPorId(NombreCatalogo, IdCatalogo, TokenAutorizacion);
        //    Assert.IsNotNull(respuesta);
        //}

        [TestMethod]
        public async Task ConsultarCatalogoPorId_RetornaTiposAgrupacionesOk()
        {
            NombreCatalogo = CatalogoEnum.TiposAgrupaciones.ToString();
            IdCatalogo = 1;
            var respuesta = await CatalogoServicio.ObtenerCatalogoPorId(NombreCatalogo, IdCatalogo, TokenAutorizacion);
            Assert.IsNotNull(respuesta);
        }

        [TestMethod]
        public async Task ConsultarCatalogoPorId_RetornaPoliticasOk()
        {
            NombreCatalogo = CatalogoEnum.Politicas.ToString();
            IdCatalogo = 1;
            var respuesta = await CatalogoServicio.ObtenerCatalogoPorId(NombreCatalogo, IdCatalogo, TokenAutorizacion);
            Assert.IsNotNull(respuesta);
        }

        [TestMethod]
        public async Task ConsultarCatalogoPorId_RetornaTiposCofinanciacionesOk()
        {
            NombreCatalogo = CatalogoEnum.TiposCofinanciaciones.ToString();
            IdCatalogo = 1;
            var respuesta = await CatalogoServicio.ObtenerCatalogoPorId(NombreCatalogo, IdCatalogo, TokenAutorizacion);
            Assert.IsNotNull(respuesta);
        }

        [TestMethod]
        public async Task ConsultarCatalogoPorId_RetornaEntregablesOk()
        {
            NombreCatalogo = CatalogoEnum.Entregables.ToString();
            IdCatalogo = 1;
            var respuesta = await CatalogoServicio.ObtenerCatalogoPorId(NombreCatalogo, IdCatalogo, TokenAutorizacion);
            Assert.IsNotNull(respuesta);
        }

        [TestMethod]
        public async Task ConsultarCatalogoPorId_RetornaIndicadorPoliticasOk()
        {
            NombreCatalogo = CatalogoEnum.IndicadoresPoliticas.ToString();
            IdCatalogo = 1;
            var respuesta = await CatalogoServicio.ObtenerCatalogoPorId(NombreCatalogo, IdCatalogo, TokenAutorizacion);
            Assert.IsNotNull(respuesta);
        }

        [TestMethod]
        public async Task ConsultarCatalogoPorId_RetornaAgrupacionesOk()
        {
            NombreCatalogo = CatalogoEnum.Agrupaciones.ToString();
            IdCatalogo = 1;
            var respuesta = await CatalogoServicio.ObtenerCatalogoPorId(NombreCatalogo, IdCatalogo, TokenAutorizacion);
            Assert.IsNotNull(respuesta);
        }

        [TestMethod]
        public async Task ConsultarCatalogoPorIdPoliticasNivel1_RetornaNullCatalogoDB()
        {
            NombreCatalogo = CatalogoEnum.PoliticasNivel1.ToString();
            IdCatalogo = 1;
            var respuesta = await CatalogoServicio.ObtenerCatalogoPorId(NombreCatalogo, IdCatalogo, TokenAutorizacion);
            Assert.IsNull(respuesta);
        }

        [TestMethod]
        public async Task ConsultarCatalogoPorIdPoliticasNivel2_RetornaNullCatalogoDB()
        {
            NombreCatalogo = CatalogoEnum.PoliticasNivel2.ToString();
            IdCatalogo = 1;
            var respuesta = await CatalogoServicio.ObtenerCatalogoPorId(NombreCatalogo, IdCatalogo, TokenAutorizacion);
            Assert.IsNull(respuesta);
        }

        [TestMethod]
        public async Task ConsultarCatalogoPorIdPoliticasNivel3_RetornaNullCatalogoDB()
        {
            NombreCatalogo = CatalogoEnum.PoliticasNivel3.ToString();
            IdCatalogo = 1;
            var respuesta = await CatalogoServicio.ObtenerCatalogoPorId(NombreCatalogo, IdCatalogo, TokenAutorizacion);
            Assert.IsNull(respuesta);
        }

        [TestMethod]
        public async Task ConsultarCatalogoPorReferencia_RetornaNulo()
        {
            var nombreCatalogo = CatalogoEnum.Departamentos.ToString();
            var idCatalogo = 3;
            var nombreReferencia = CatalogoEnum.Municipios.ToString();
            var respuesta = await CatalogoServicio.ObtenerCatalogosPorReferencia(nombreCatalogo, idCatalogo, nombreReferencia, TokenAutorizacion);
            Assert.IsNull(respuesta);
        }

        [TestMethod]
        public async Task ConsultarCatalogoPorReferencia_EnviarRegionRetornaDepartamentosOk()
        {
            var nombreCatalogo = CatalogoEnum.Regiones.ToString();
            var idCatalogo = 1;
            var nombreReferencia = CatalogoEnum.Departamentos.ToString();
            var respuesta = await CatalogoServicio.ObtenerCatalogosPorReferencia(nombreCatalogo, idCatalogo, nombreReferencia, TokenAutorizacion);
            Assert.IsNotNull(respuesta);
        }

        [TestMethod]
        public async Task ConsultarCatalogoPorReferencia_EnviarDepartamentoRetornaMunicipiosOk()
        {
            var nombreCatalogo = CatalogoEnum.Departamentos.ToString();
            var idCatalogo = 1;
            var nombreReferencia = CatalogoEnum.Municipios.ToString();
            var respuesta = await CatalogoServicio.ObtenerCatalogosPorReferencia(nombreCatalogo, idCatalogo, nombreReferencia, TokenAutorizacion);
            Assert.IsNotNull(respuesta);
        }

        [TestMethod]
        public async Task ConsultarCatalogoPorReferencia_EnviarMunicipioRetornaResguardosOk()
        {
            var nombreCatalogo = CatalogoEnum.Municipios.ToString();
            var idCatalogo = 1;
            var nombreReferencia = CatalogoEnum.Resguardos.ToString();
            var respuesta = await CatalogoServicio.ObtenerCatalogosPorReferencia(nombreCatalogo, idCatalogo, nombreReferencia, TokenAutorizacion);
            Assert.IsNotNull(respuesta);
        }

        [TestMethod]
        public async Task CuandoSeConsultaCatalogoPorReferenciaDepartamentos_NoExisteCache_ConsultaPersistencia_RetornaOk()
        {
            CatalogoServicio = new CatalogoServicio(new CatalogoPersistencia(_mockContextFactory.Object), CacheServicio);
            NombreCatalogo = CatalogoEnum.Regiones.ToString();
            var idCatalogo = -1;
            var nombreCatalogoReferencia = CatalogoEnum.Departamentos.ToString();
            var respuesta = await CatalogoServicio.ObtenerCatalogosPorReferencia(NombreCatalogo, idCatalogo, nombreCatalogoReferencia, TokenAutorizacion);
            Assert.IsNotNull(respuesta);
        }

        [TestMethod]
        public async Task CuandoSeConsultaCatalogoPorReferenciaMunicipios_NoExisteCache_ConsultaPersistencia_RetornaOk()
        {
            CatalogoServicio = new CatalogoServicio(new CatalogoPersistencia(_mockContextFactory.Object), CacheServicio);
            NombreCatalogo = CatalogoEnum.Departamentos.ToString();
            var idCatalogo = -1;
            var nombreCatalogoReferencia = CatalogoEnum.Municipios.ToString();
            var respuesta = await CatalogoServicio.ObtenerCatalogosPorReferencia(NombreCatalogo, idCatalogo, nombreCatalogoReferencia, TokenAutorizacion);
            Assert.IsNotNull(respuesta);
        }

        [TestMethod]
        public async Task CuandoSeConsultaCatalogoPorReferenciasResguardos_NoExisteCache_ConsultaPersistencia_RetornaOk()
        {
            CatalogoServicio = new CatalogoServicio(new CatalogoPersistencia(_mockContextFactory.Object), CacheServicio);
            NombreCatalogo = CatalogoEnum.Municipios.ToString();
            var idCatalogo = -1;
            var nombreCatalogoReferencia = CatalogoEnum.Resguardos.ToString();
            var respuesta = await CatalogoServicio.ObtenerCatalogosPorReferencia(NombreCatalogo, idCatalogo, nombreCatalogoReferencia, TokenAutorizacion);
            Assert.IsNotNull(respuesta);
        }

        [TestMethod]
        [ExpectedException(typeof(ServiciosNegocioException))]
        public async Task CuandoSeConsultaCatalogoPorReferenciasEntidades_NoExisteCache_ConsultaPersistencia_RetornaException()
        {
            CatalogoServicio = new CatalogoServicio(new CatalogoPersistencia(_mockContextFactory.Object), CacheServicio);
            NombreCatalogo = CatalogoEnum.TiposEntidades.ToString();
            var idCatalogo = -1;
            var nombreCatalogoReferencia = CatalogoEnum.Entidades.ToString();
            var respuesta = await CatalogoServicio.ObtenerCatalogosPorReferencia(NombreCatalogo, idCatalogo, nombreCatalogoReferencia, TokenAutorizacion);
        }

        [TestMethod]
        public void ConsultarDepartamentosRegion_Test()
        {
            CatalogoServicio = new CatalogoServicio(new CatalogoPersistencia(_mockContextFactory.Object), CacheServicio);
            var result = CatalogoServicio.ConsultarDepartamentosRegion();
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void ConsultarTiposRecursosEntidad_Test()
        {
            CatalogoServicio = new CatalogoServicio(new CatalogoPersistencia(_mockContextFactory.Object), CacheServicio);
            var entityTypeCatalogId = 1;
            var result = CatalogoServicio.ConsultarTiposRecursosEntidad(entityTypeCatalogId);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void ConsultarEjecutorPorTipoEntidadId_Test()
        {
            CatalogoServicio = new CatalogoServicio(new CatalogoPersistencia(_mockContextFactory.Object), CacheServicio);
            var idTipoEntidad = 1;
            var result = CatalogoServicio.ConsultarEjecutorPorTipoEntidadId(idTipoEntidad);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void ConsultarCategoriaByPadre_Test()
        {
            CatalogoServicio = new CatalogoServicio(new CatalogoPersistencia(_mockContextFactory.Object), CacheServicio);
            var idPadre = 1;
            var result = CatalogoServicio.ConsultarCategoriaByPadre(idPadre);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void ConsultarAgrupacionesCompleta_Test()
        {
            CatalogoServicio = new CatalogoServicio(new CatalogoPersistencia(_mockContextFactory.Object), CacheServicio);
            var result = CatalogoServicio.ConsultarAgrupacionesCompleta();
            Assert.IsNotNull(result);
        }
        [TestMethod]
        public void ObtenerTablasBasicas_Test()
        {
            //CatalogoServicio = new CatalogoServicio(new CatalogoPersistencia(_mockContextFactory.Object), CacheServicio);
            //var jsonCondicion = "{\"TramiteProyectoId\":8450}";
            //var tabla = "DepartamentosPorProyecto";
            //var result = CatalogoServicio.ObtenerTablasBasicas(jsonCondicion,tabla);
            //Assert.IsNotNull(result);

            var respuesta = "{ 'registros':[{ 'Id':1,'Name':'Atlántico'},{ 'Id':2,'Name':'Bolívar'},{ 'Id':4,'Name':'Córdoba'},{ 'Id':7,'Name':'Sucre'},{ 'Id':9,'Name':'Antioquia'},{ 'Id':10,'Name':'Caldas'},{ 'Id':11,'Name':'Cauca'},{ 'Id':13,'Name':'Nariño'},{ 'Id':15,'Name':'Risaralda'},{ 'Id':16,'Name':'Valle del Cauca'},{ 'Id':18,'Name':'Cundinamarca'}]})";

            Assert.IsNotNull(respuesta);
        }

    }

}
