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
    using DNP.ServiciosNegocio.Dominio.Dto.Transferencias;

    [TestClass]
    public class DevolverProyectoServicioTest
    {

        private string BpinBase { get; set; }
        private IDevolverProyectoServicio _devolverProyectoServicio;
        private DevolverProyectoServicio DevolverProyectoServicio { get; set; }
        //private readonly Mock<DbSet<ViewFuentesFinanciacion>> _mockSet = new Mock<DbSet<ViewFuentesFinanciacion>>();
        private readonly Mock<MGAWebContexto> _mockContext = new Mock<MGAWebContexto>();
        private readonly Mock<IContextoFactory> _mockContextFactory = new Mock<IContextoFactory>();
        private IDevolverProyectoPersistencia DevolverProyectoPersistencia { get; set; }
        private IPersistenciaTemporal PersistenciaTemporal { get; set; }
        private IAuditoriaServicios AuditoriaServicio { get; set; }
        private readonly Mock<DbSet<AlmacenamientoTemporal>> _mockSetAlmacenamientoTemporal = new Mock<DbSet<AlmacenamientoTemporal>>();
        private readonly Mock<DbSet<Bienio>> _mockSetBienio = new Mock<DbSet<Bienio>>();


        [TestInitialize]
        public void Init()
        {
            BpinBase = "2017011000042";
            var contenedor = UnityConfig.Container;
            _devolverProyectoServicio = contenedor.Resolve<IDevolverProyectoServicio>();
            DevolverProyectoPersistencia = UnityContainerExtensions.Resolve<IDevolverProyectoPersistencia>(contenedor);
            PersistenciaTemporal = contenedor.Resolve<IPersistenciaTemporal>();
            AuditoriaServicio = contenedor.Resolve<IAuditoriaServicios>();
            DevolverProyectoServicio = new DevolverProyectoServicio(DevolverProyectoPersistencia, PersistenciaTemporal, AuditoriaServicio);

            var dataAlmacenamientoTemporal = new List<AlmacenamientoTemporal>()
                                           {
                                               new AlmacenamientoTemporal()
                                               {
                                                   AccionId = new Guid("E3C1849C-FE24-4C07-9762-036FA72AF10C"),
                                                   InstanciaId =    new Guid("4C2E62CD-CEAD-48EF-88C6-A50AB5913716"),
                                                   Json = "{\"AcumulaIndicadorPrincipal\": true,\"AlternativaId\" : 2,\"Cantidad\" : 5,\"Complemento\" : \"Prubea\",\"CondicionDeseadaId\" : 4,\"CPCId\" : 5,\r              \"FuenteVerificacion\" : \"PRUEB fuenteVerificacion\",\"IndicadorPrincipal\" : \"PRUEBA INDICADOR PRINCIPAL\",\"Meta\" : 10,\"MetaTotalIndicadorPrincipal\" : 15,\r                \"ProductoIndicador\" :\r[{\r\"AcumulaIndicadorSecundario\" : true,\r\"IndicadorSecundarioId\":2,\r \"IndicadorSecundario\":\"PRUEBA INDICADOR SECUNDARIO\",\r          \"MetaTotalIndicadorSecundario\":10,\r\"UnidadMedidaIndicadorSecundario\":4,\r\"ProductoIndicadorDetalle\":[\r{\r\"DetalleIndicadorProductoId\":4,\r\"MetaVigente\":5,\r\"Vigencia\":4,\r\"RegionalizacionMetas\":[{\r\"AgrupacionId\":2,\r\"DepartamentoId\":1,\r\"FechaCreacion\":\"2018/02/03\",\r\"MunicipioId\":2,\r\"RegionalizacionMetaId\":3,\r\"RegionId\":1,\r\"TotalMetaVigente\":58,\r\"VigenciaRegionalizada\":2\r},{\r\"AgrupacionId\":3,\r\"DepartamentoId\":2,\r\"FechaCreacion\":\"2018/02/03\",\r\"MunicipioId\":1,\r\"RegionalizacionMetaId\":4,\r\"RegionId\":5,\r\"TotalMetaVigente\":3,\r\"VigenciaRegionalizada\":2\r}]\r }]\r\r},{\r\"AcumulaIndicadorSecundario\" : true,\r\"IndicadorSecundarioId\":2,\r\"IndicadorSecundario\":\"PRUEBA INDICADOR SECUNDARIO\",\r\"MetaTotalIndicadorSecundario\":10,\r\"UnidadMedidaIndicadorSecundario\":4,\r\"ProductoIndicadorDetalle\":[\r{\r\"DetalleIndicadorProductoId\":4,\r\"MetaVigente\":5,\r\"Vigencia\":4,\r\"RegionalizacionMetas\":[{\r\"AgrupacionId\":2,\r\"DepartamentoId\":1,\r\"FechaCreacion\":\"2018/02/03\",\r\"MunicipioId\":2,\r\"RegionalizacionMetaId\":3,\r\"RegionId\":1,\r\"TotalMetaVigente\":58,\r\"VigenciaRegionalizada\":2\r},{\r\"AgrupacionId\":3,\r\"DepartamentoId\":2,\r\"FechaCreacion\":\"2018/02/03\",\r\"MunicipioId\":1,\r\"RegionalizacionMetaId\":4,\r\"RegionId\":5,\r\"TotalMetaVigente\":3,\r\"VigenciaRegionalizada\":2\r}]\r}]\r} ]\r}"
                                               }
                                           }.AsQueryable();



            var objetoRetornoDevolverProyecto = new uspGetDevolverProyectoMga_Result();
            var mockDevolverProyecto = new Mock<ObjectResult<uspGetDevolverProyectoMga_Result>>();

            mockDevolverProyecto.SetupReturn(objetoRetornoDevolverProyecto);

            _mockSetAlmacenamientoTemporal.As<IQueryable<AlmacenamientoTemporal>>().Setup(m => m.Provider).Returns(dataAlmacenamientoTemporal.Provider);
            _mockSetAlmacenamientoTemporal.As<IQueryable<AlmacenamientoTemporal>>().Setup(m => m.Expression).Returns(dataAlmacenamientoTemporal.Expression);
            _mockSetAlmacenamientoTemporal.As<IQueryable<AlmacenamientoTemporal>>().Setup(m => m.ElementType).Returns(dataAlmacenamientoTemporal.ElementType);
            _mockSetAlmacenamientoTemporal.As<IQueryable<AlmacenamientoTemporal>>().Setup(m => m.GetEnumerator()).Returns(dataAlmacenamientoTemporal.GetEnumerator());

            _mockContext.Setup(m => m.AlmacenamientoTemporal).Returns(_mockSetAlmacenamientoTemporal.Object);
            _mockContext.Setup(mc => mc.uspGetDevolverProyectoMga(BpinBase)).Returns(mockDevolverProyecto.Object);


            _mockContextFactory.Setup(mcf => mcf.CrearContextoConConexion(ConfigurationManager.ConnectionStrings["MGAWebContexto"].ConnectionString)).Returns(_mockContext.Object);
        }
        

        [TestMethod]
        public void DevolverProyectoTest()
        {
            _devolverProyectoServicio = new DevolverProyectoServicio(new DevolverProyectoPersistencia(_mockContextFactory.Object), new PersistenciaTemporal(_mockContextFactory.Object), new AuditoriaServicios());
            var result = _devolverProyectoServicio.ObtenerDevolverProyecto(new ParametrosConsultaDto
            {
                Bpin = BpinBase,
                AccionId = new Guid("E3C1849C-FE24-4C07-9762-036FA72AF10C"),
                InstanciaId = new Guid("4C2E62CD-CEAD-48EF-88C6-A50AB5913716")
            });
            Assert.IsNotNull(result);
        }


        [TestMethod]
        public void DevolverProyecto_ObtenerDefinitivo_Test()
        {
            DevolverProyectoServicio = new DevolverProyectoServicio(new DevolverProyectoPersistencia(_mockContextFactory.Object), new PersistenciaTemporal(_mockContextFactory.Object), new AuditoriaServicios());
            var parametrosConsulta = new ParametrosConsultaDto()
            {
                AccionId = new Guid("418D76AC-F081-4D73-A05A-530CD7C6AFF6"),
                InstanciaId = new Guid("6A0E8FC3-73CF-46B5-B59C-0F683A30AE1D"),
                Bpin = "2017011000042"
            };

            var resultado = DevolverProyectoServicio.Obtener(parametrosConsulta);
            Assert.IsNotNull(resultado);
        }


        [TestMethod]
        public void DevolverProyecto_GuardarDefinitivo()
        {
            var parametrosGuardar = new ParametrosGuardarDto<DevolverProyectoDto>()
            {
                AccionId = new Guid("418D76AC-F081-4D73-A05A-530CD7C6AFF6"),
                InstanciaId = new Guid("6A0E8FC3-73CF-46B5-B59C-0F683A30AE1D"),
                Usuario = "jdelgado",
                Contenido = new DevolverProyectoDto()
                {                   
                    ProyectoId = 145896,
                    Bpin = "2017011000042",
                    Observacion = "Prueba ",            
                    DevolverId = true,
                    EstadoDevolver = 7

                }
            };

            var parametroAuditoria = new ParametrosAuditoriaDto()
            {
                Usuario = "jdelgado",
                Ip = "localhost"
            };

            DevolverProyectoServicio.Guardar(parametrosGuardar, parametroAuditoria, false);
        }


    }
}
