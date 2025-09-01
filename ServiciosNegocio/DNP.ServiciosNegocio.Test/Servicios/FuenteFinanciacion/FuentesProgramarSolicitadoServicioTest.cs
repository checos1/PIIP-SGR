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

namespace DNP.ServiciosNegocio.Test.Servicios.FuentesProgramarSolicitado
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

    [TestClass]
    public class FuentesProgramarSolicitadoServicioTest
    {
        private string BpinBase { get; set; }
        private IFuentesProgramarSolicitadoServicio _fuenteProgramarSolicitadoServicio;
        private FuentesProgramarSolicitadoServicio FuentesProgramarSolicitadoServicio { get; set; }
        private readonly Mock<MGAWebContexto> _mockContext = new Mock<MGAWebContexto>();
        private readonly Mock<IContextoFactory> _mockContextFactory = new Mock<IContextoFactory>();
        private IFuentesProgramarSolicitadoPersistencia FuentesProgramarSolicitadoPersistencia { get; set; }
        private IPersistenciaTemporal PersistenciaTemporal { get; set; }
        private IAuditoriaServicios AuditoriaServicio { get; set; }
        private readonly Mock<DbSet<AlmacenamientoTemporal>> _mockSetAlmacenamientoTemporal = new Mock<DbSet<AlmacenamientoTemporal>>();

        [TestInitialize]
        public void Init()
        {
            BpinBase = "202000000000005";
            var contenedor = UnityConfig.Container;
            _fuenteProgramarSolicitadoServicio = contenedor.Resolve<IFuentesProgramarSolicitadoServicio>();
            FuentesProgramarSolicitadoPersistencia = UnityContainerExtensions.Resolve<IFuentesProgramarSolicitadoPersistencia>(contenedor);
            PersistenciaTemporal = contenedor.Resolve<IPersistenciaTemporal>();
            AuditoriaServicio = contenedor.Resolve<IAuditoriaServicios>();
            FuentesProgramarSolicitadoServicio = new FuentesProgramarSolicitadoServicio(FuentesProgramarSolicitadoPersistencia, PersistenciaTemporal, AuditoriaServicio);

            var dataAlmacenamientoTemporal = new List<AlmacenamientoTemporal>()
                                           {
                                               new AlmacenamientoTemporal()
                                               {
                                                   AccionId = new Guid("48ACE8E7-37DB-BF71-7CE5-4996ED17DEA1"),
                                                   InstanciaId =    new Guid("1CC3A855-12F1-4113-A044-014886298AA3"),
                                                   Json = "{\"AcumulaIndicadorPrincipal\": true,\"AlternativaId\" : 2,\"Cantidad\" : 5,\"Complemento\" : \"Prubea\",\"CondicionDeseadaId\" : 4,\"CPCId\" : 5,\r              \"FuenteVerificacion\" : \"PRUEB fuenteVerificacion\",\"IndicadorPrincipal\" : \"PRUEBA INDICADOR PRINCIPAL\",\"Meta\" : 10,\"MetaTotalIndicadorPrincipal\" : 15,\r                \"ProductoIndicador\" :\r[{\r\"AcumulaIndicadorSecundario\" : true,\r\"IndicadorSecundarioId\":2,\r \"IndicadorSecundario\":\"PRUEBA INDICADOR SECUNDARIO\",\r          \"MetaTotalIndicadorSecundario\":10,\r\"UnidadMedidaIndicadorSecundario\":4,\r\"ProductoIndicadorDetalle\":[\r{\r\"DetalleIndicadorProductoId\":4,\r\"MetaVigente\":5,\r\"Vigencia\":4,\r\"RegionalizacionMetas\":[{\r\"AgrupacionId\":2,\r\"DepartamentoId\":1,\r\"FechaCreacion\":\"2018/02/03\",\r\"MunicipioId\":2,\r\"RegionalizacionMetaId\":3,\r\"RegionId\":1,\r\"TotalMetaVigente\":58,\r\"VigenciaRegionalizada\":2\r},{\r\"AgrupacionId\":3,\r\"DepartamentoId\":2,\r\"FechaCreacion\":\"2018/02/03\",\r\"MunicipioId\":1,\r\"RegionalizacionMetaId\":4,\r\"RegionId\":5,\r\"TotalMetaVigente\":3,\r\"VigenciaRegionalizada\":2\r}]\r }]\r\r},{\r\"AcumulaIndicadorSecundario\" : true,\r\"IndicadorSecundarioId\":2,\r\"IndicadorSecundario\":\"PRUEBA INDICADOR SECUNDARIO\",\r\"MetaTotalIndicadorSecundario\":10,\r\"UnidadMedidaIndicadorSecundario\":4,\r\"ProductoIndicadorDetalle\":[\r{\r\"DetalleIndicadorProductoId\":4,\r\"MetaVigente\":5,\r\"Vigencia\":4,\r\"RegionalizacionMetas\":[{\r\"AgrupacionId\":2,\r\"DepartamentoId\":1,\r\"FechaCreacion\":\"2018/02/03\",\r\"MunicipioId\":2,\r\"RegionalizacionMetaId\":3,\r\"RegionId\":1,\r\"TotalMetaVigente\":58,\r\"VigenciaRegionalizada\":2\r},{\r\"AgrupacionId\":3,\r\"DepartamentoId\":2,\r\"FechaCreacion\":\"2018/02/03\",\r\"MunicipioId\":1,\r\"RegionalizacionMetaId\":4,\r\"RegionId\":5,\r\"TotalMetaVigente\":3,\r\"VigenciaRegionalizada\":2\r}]\r}]\r} ]\r}"
                                               }
                                           }.AsQueryable();

            string objetoRetornoFuentes = "{\"ProyectoId\":null,\"CodigoBPIN\":\"202000000000005\",\"CR\":null,\"Cofinanciacion\":[{\"CofinanciadorId\":0,\"TipoCofinanciadorId\":0,\"TipoCofinanciador\":\"\",\"Cofinanciador\":\"\",\"Fuentes\":[{\"FuenteId\":0,\"Fuente\":\"\",\"TipoEntidadId\":0,\"TipoEntidad\":\"\",\"EntidadId\":0,\"Entidad\":\"\",\"TipoRecursoId\":0,\"TipoRecurso\":\"\",\"Seleccionado\":0}]}]}";
            var mockFuentes = new Mock<ObjectResult<string>>();
            mockFuentes.SetupReturn(objetoRetornoFuentes);

            _mockSetAlmacenamientoTemporal.As<IQueryable<AlmacenamientoTemporal>>().Setup(m => m.Provider).Returns(dataAlmacenamientoTemporal.Provider);
            _mockSetAlmacenamientoTemporal.As<IQueryable<AlmacenamientoTemporal>>().Setup(m => m.Expression).Returns(dataAlmacenamientoTemporal.Expression);
            _mockSetAlmacenamientoTemporal.As<IQueryable<AlmacenamientoTemporal>>().Setup(m => m.ElementType).Returns(dataAlmacenamientoTemporal.ElementType);
            _mockSetAlmacenamientoTemporal.As<IQueryable<AlmacenamientoTemporal>>().Setup(m => m.GetEnumerator()).Returns(dataAlmacenamientoTemporal.GetEnumerator());

            _mockContext.Setup(m => m.AlmacenamientoTemporal).Returns(_mockSetAlmacenamientoTemporal.Object);
            _mockContext.Setup(mc => mc.uspGetFuentesProgramarSolicitado_JSON(BpinBase)).Returns(mockFuentes.Object);

            _mockContextFactory.Setup(mcf => mcf.CrearContextoConConexion(ConfigurationManager.ConnectionStrings["MGAWebContexto"].ConnectionString)).Returns(_mockContext.Object);
        }

        [TestMethod]
        public void ObtenerFuentesProgramarSolicitado_Test()
        {
            _fuenteProgramarSolicitadoServicio = new FuentesProgramarSolicitadoServicio(new FuentesProgramarSolicitadoPersistencia(_mockContextFactory.Object), new PersistenciaTemporal(_mockContextFactory.Object), new AuditoriaServicios());
            var result = _fuenteProgramarSolicitadoServicio.ObtenerFuentesProgramarSolicitado(BpinBase);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public void ObtenerDefinitivo_Test()
        {
            FuentesProgramarSolicitadoServicio = new FuentesProgramarSolicitadoServicio(new FuentesProgramarSolicitadoPersistencia(_mockContextFactory.Object), new PersistenciaTemporal(_mockContextFactory.Object), new AuditoriaServicios());
            var parametros = new ParametrosConsultaDto();
            FuentesProgramarSolicitadoServicio.Obtener(parametros);
        }

        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public void GuardadoDefinitivo_Test()
        {
            FuentesProgramarSolicitadoServicio = new FuentesProgramarSolicitadoServicio(new FuentesProgramarSolicitadoPersistencia(_mockContextFactory.Object), new PersistenciaTemporal(_mockContextFactory.Object), new AuditoriaServicios());
            var parametros = new ParametrosGuardarDto<FuentesProgramarSolicitadoDto>();
            var parametroAuditoria = new ParametrosAuditoriaDto()
            {
                Usuario = "Leticia01",
                Ip = "localhost"
            };
            FuentesProgramarSolicitadoServicio.Guardar(parametros, parametroAuditoria, false);
        }
    }
}
