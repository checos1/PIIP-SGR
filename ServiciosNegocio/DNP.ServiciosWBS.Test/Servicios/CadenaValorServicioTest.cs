namespace DNP.ServiciosWBS.Test.Servicios
{
    using Configuracion;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Persistencia.Interfaces;
    using Persistencia.Interfaces.Transversales;
    using ServiciosNegocio.Comunes.Dto;
    using ServiciosNegocio.Comunes.Dto.Formulario;
    using ServiciosNegocio.Comunes.Excepciones;
    using ServiciosNegocio.Dominio.Dto.Formulario;
    using ServiciosWBS.Servicios.Implementaciones;
    using ServiciosWBS.Servicios.Interfaces.Transversales;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    using System.Net.Http;
    using Mocks;
    using Moq;
    using Persistencia.Implementaciones;
    using Persistencia.Implementaciones.Transversales;
    using Persistencia.Modelo;
    using ServiciosNegocio.Test.Mock;
    using Unity;

    [TestClass]
    public class CadenaValorServicioTest
    {
        private ICadenaValorPersistencia CadenaValorPersistencia { get; set; }
        private IPersistenciaTemporal PersistenciaTemporal { get; set; }
        private IAuditoriaServicios AuditoriaServicio { get; set; }
        private CadenaValorServicios CadenaValorServicio { get; set; }
        private readonly Mock<MGAWebContexto> _mockContext = new Mock<MGAWebContexto>();
        private readonly Mock<IContextoFactory> _mockContextFactory = new Mock<IContextoFactory>();
        private readonly Mock<MockableDbSetWithExtensions<AlmacenamientoTemporal>> _mockSetAlmacenamientoTemporal = new Mock<MockableDbSetWithExtensions<AlmacenamientoTemporal>>();

        [TestInitialize]
        public void Init()
        {
            var contenedor = UnityConfig.Container;
            CadenaValorPersistencia = UnityContainerExtensions.Resolve<ICadenaValorPersistencia>(contenedor);
            PersistenciaTemporal = contenedor.Resolve<IPersistenciaTemporal>();
            AuditoriaServicio = contenedor.Resolve<IAuditoriaServicios>();
            CadenaValorServicio = new CadenaValorServicios(CadenaValorPersistencia, PersistenciaTemporal, AuditoriaServicio);

            var objetoRetornoFuentes = new uspGetCadenaValor_Result();
            var mockFuentes = new Mock<ObjectResult<uspGetCadenaValor_Result>>();
            mockFuentes.SetupReturn(objetoRetornoFuentes);

            var dataAlmacenamientoTemporal = new List<AlmacenamientoTemporal>()
                                           {
                                               new AlmacenamientoTemporal()
                                               {
                                                   AccionId = new Guid("E3C1849C-FE24-4C07-9762-036FA72AF10C"),
                                                   InstanciaId =    new Guid("4C2E62CD-CEAD-48EF-88C6-A50AB5913716"),
                                                   Json = "{\"AcumulaIndicadorPrincipal\": true,\"AlternativaId\" : 2,\"Cantidad\" : 5,\"Complemento\" : \"Prubea\",\"CondicionDeseadaId\" : 4,\"CPCId\" : 5,\r              \"FuenteVerificacion\" : \"PRUEB fuenteVerificacion\",\"IndicadorPrincipal\" : \"PRUEBA INDICADOR PRINCIPAL\",\"Meta\" : 10,\"MetaTotalIndicadorPrincipal\" : 15,\r                \"ProductoIndicador\" :\r[{\r\"AcumulaIndicadorSecundario\" : true,\r\"IndicadorSecundarioId\":2,\r \"IndicadorSecundario\":\"PRUEBA INDICADOR SECUNDARIO\",\r          \"MetaTotalIndicadorSecundario\":10,\r\"UnidadMedidaIndicadorSecundario\":4,\r\"ProductoIndicadorDetalle\":[\r{\r\"DetalleIndicadorProductoId\":4,\r\"MetaVigente\":5,\r\"Vigencia\":4,\r\"RegionalizacionMetas\":[{\r\"AgrupacionId\":2,\r\"DepartamentoId\":1,\r\"FechaCreacion\":\"2018/02/03\",\r\"MunicipioId\":2,\r\"RegionalizacionMetaId\":3,\r\"RegionId\":1,\r\"TotalMetaVigente\":58,\r\"VigenciaRegionalizada\":2\r},{\r\"AgrupacionId\":3,\r\"DepartamentoId\":2,\r\"FechaCreacion\":\"2018/02/03\",\r\"MunicipioId\":1,\r\"RegionalizacionMetaId\":4,\r\"RegionId\":5,\r\"TotalMetaVigente\":3,\r\"VigenciaRegionalizada\":2\r}]\r }]\r\r},{\r\"AcumulaIndicadorSecundario\" : true,\r\"IndicadorSecundarioId\":2,\r\"IndicadorSecundario\":\"PRUEBA INDICADOR SECUNDARIO\",\r\"MetaTotalIndicadorSecundario\":10,\r\"UnidadMedidaIndicadorSecundario\":4,\r\"ProductoIndicadorDetalle\":[\r{\r\"DetalleIndicadorProductoId\":4,\r\"MetaVigente\":5,\r\"Vigencia\":4,\r\"RegionalizacionMetas\":[{\r\"AgrupacionId\":2,\r\"DepartamentoId\":1,\r\"FechaCreacion\":\"2018/02/03\",\r\"MunicipioId\":2,\r\"RegionalizacionMetaId\":3,\r\"RegionId\":1,\r\"TotalMetaVigente\":58,\r\"VigenciaRegionalizada\":2\r},{\r\"AgrupacionId\":3,\r\"DepartamentoId\":2,\r\"FechaCreacion\":\"2018/02/03\",\r\"MunicipioId\":1,\r\"RegionalizacionMetaId\":4,\r\"RegionId\":5,\r\"TotalMetaVigente\":3,\r\"VigenciaRegionalizada\":2\r}]\r}]\r} ]\r}"
                                               }
                                           }.AsQueryable();

            _mockSetAlmacenamientoTemporal.As<IQueryable<AlmacenamientoTemporal>>().Setup(m => m.Provider).Returns(dataAlmacenamientoTemporal.Provider);
            _mockSetAlmacenamientoTemporal.As<IQueryable<AlmacenamientoTemporal>>().Setup(m => m.Expression).Returns(dataAlmacenamientoTemporal.Expression);
            _mockSetAlmacenamientoTemporal.As<IQueryable<AlmacenamientoTemporal>>().Setup(m => m.ElementType).Returns(dataAlmacenamientoTemporal.ElementType);
            _mockSetAlmacenamientoTemporal.As<IQueryable<AlmacenamientoTemporal>>().Setup(m => m.GetEnumerator()).Returns(dataAlmacenamientoTemporal.GetEnumerator());

            _mockContext.Setup(m => m.AlmacenamientoTemporal).Returns(_mockSetAlmacenamientoTemporal.Object);
            _mockContext.Setup(mc => mc.uspGetCadenaValor("2017002700002")).Returns(mockFuentes.Object);

            _mockContextFactory.Setup(mcf => mcf.CrearContextoConConexion(ConfigurationManager.ConnectionStrings["MGAWebContexto"].ConnectionString)).Returns(_mockContext.Object);
        }


        [TestMethod]
        public void CadenaValorPreview()
        {
            var result = CadenaValorServicio.ObtenerCadenaValorPreview();
            Assert.IsNotNull(result);
        }

        [TestMethod]
        [ExpectedException(typeof(ServiciosNegocioException), "Parámetro piip-idInstanciaFlujoFlujo no recibido.")]
        public void _cadenaValorServicio_ConstruirParametrosGuardado_IdInstanciaFlujoNoEnviado_Excepcion()
        {
            //Escenario: InstanciaId no enviado
            var contenido = new CadenaValorDto();

            HttpRequestMessage request = new HttpRequestMessage();
            request.Headers.Add("piip-idAccion", Guid.NewGuid().ToString());

            //Ejecucion
            CadenaValorServicio.ConstruirParametrosGuardado(request, contenido);
        }

        [TestMethod]
        [ExpectedException(typeof(ServiciosNegocioException), "Parámetro piip-idAccion no recibido.")]
        public void _cadenaValorServicio_Guardar_IdAccionNoEnviado_Excepcion()
        {
            //Escenario: AccionId no enviado
            var contenido = new CadenaValorDto();

            HttpRequestMessage request = new HttpRequestMessage();
            request.Headers.Add("piip-idInstanciaFlujo", Guid.NewGuid().ToString());

            //Ejecucion
            CadenaValorServicio.ConstruirParametrosGuardado(request, contenido);
        }

        [TestMethod]
        [ExpectedException(typeof(ServiciosNegocioException), "Parámetro contenido no recibido.")]
        public void _cadenaValorServicio_Guardar_CadenaValorDtoNoEnviado_Excepcion()
        {
            //Escenario: Contenido no enviado
            HttpRequestMessage request = new HttpRequestMessage();
            request.Headers.Add("piip-idAccion", Guid.NewGuid().ToString());
            request.Headers.Add("piip-idInstanciaFlujo", Guid.NewGuid().ToString());

            //Ejecucion
            CadenaValorServicio.ConstruirParametrosGuardado(request, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ServiciosNegocioException), "Parámetro piip-idInstanciaFlujo inválido")]
        public void _cadenaValorServicio_Guardar_IdInstanciaFlujoConValorInvalido_Excepcion()
        {
            //Escenario: InstanciaId inválido
            var contenido = new CadenaValorDto();

            HttpRequestMessage request = new HttpRequestMessage();
            request.Headers.Add("piip-idInstanciaFlujo", Guid.Empty.ToString());

            //Ejecucion
            CadenaValorServicio.ConstruirParametrosGuardado(request, contenido);
        }

        [TestMethod]
        [ExpectedException(typeof(ServiciosNegocioException), "Parámetro piip-idAccion inválido")]
        public void _cadenaValorServicio_Guardar_IdAccionConValorInvalido_Excepcion()
        {
            //Escenario: AccionId inválido
            var contenido = new CadenaValorDto();

            HttpRequestMessage request = new HttpRequestMessage();
            request.Headers.Add("piip-idAccion", Guid.Empty.ToString());

            //Ejecucion
            CadenaValorServicio.ConstruirParametrosGuardado(request, contenido);
        }

        [TestMethod]
        public void CadenaValor_Guardar_MarcadoComoTemporal_InsertaRegistro()
        {
            CadenaValorServicio = new CadenaValorServicios(new CadenaValorPersistencia(_mockContextFactory.Object), new PersistenciaTemporal(_mockContextFactory.Object), AuditoriaServicio);
            //Escenario: los parametros de insercion son validos y ademas viene marcado como guardado temporal
            var parametrosGuardarProducto = new ParametrosGuardarDto<CadenaValorDto>
            {
                InstanciaId = Guid.NewGuid(),
                AccionId = Guid.NewGuid(),
                Contenido = new CadenaValorDto()
            };

            var parametrosAuditoria = new ParametrosAuditoriaDto();

            //Ejecucion
            CadenaValorServicio.Guardar(parametrosGuardarProducto, parametrosAuditoria, true);
        }

        [TestMethod]
        public void CadenaValor_ObtenerDefinitivo()
        {
            CadenaValorServicio = new CadenaValorServicios(new CadenaValorPersistencia(_mockContextFactory.Object), new PersistenciaTemporal(_mockContextFactory.Object), AuditoriaServicio);
            var parametrosConsulta = new ParametrosConsultaDto()
            {
                AccionId = new Guid("418D76AC-F081-4D73-A05A-530CD7C6AFF6"),
                InstanciaId = new Guid("6A0E8FC3-73CF-46B5-B59C-0F683A30AE1D"),
                Bpin = "2017002700002"
            };

            var resultado = CadenaValorServicio.Obtener(parametrosConsulta);
            Assert.IsNotNull(resultado);
        }

        [TestMethod]
        public void CadenaValor_GuardarDefinitivo()
        {
            //CadenaValorServicio = new CadenaValorServicios(new CadenaValorPersistencia(_mockContextFactory.Object), new PersistenciaTemporal(_mockContextFactory.Object), AuditoriaServicio);
            var auxVigencias = new List<VigenciaCadenaValorDto>();
            var auxMes = new List<MesCadenaValorDto>();
            var auxGrupoRecurso = new List<GrupoRecursoDto>();
            var auxObjectivoEspecifico = new List<ObjetivoEspecificoCadenaValorDto>();
            var auxProductos = new List<ProductoCadenaValorDto>();
            var auxActividades = new List<ActividadCadenaValorDto>();

            auxActividades.Add(new ActividadCadenaValorDto()
            {
                Id = 1,
                ActividadId = 3,
                NombreActividad = "Actividda 1",
                Etapa = "Preinversión",
                TipoInsumoId = 1,
                ValorSolicitado = 1515,
                ValorInicial = 1000,
                ValorVigente = 566,
                Compromiso = 444,
                Obligacion = 336,
                Pago = 1000,
                Observacion = "Sin Observación"
            });
            auxProductos.Add(new ProductoCadenaValorDto()
            {
                Id = 7,
                CatalogoProductoId = 949,
                Nombre = "Vía urbana pavimentada",
                TipoMedidaId = 773,
                Cantidad = 6400000,
                Actividad = auxActividades
            });
            auxObjectivoEspecifico.Add(new ObjetivoEspecificoCadenaValorDto()
            {
                Id = 3,
                ObjetivoEspecifico = "mejoramiento de paso de peatones y vehiculos",
                Productos = auxProductos
            });
            auxGrupoRecurso.Add(new GrupoRecursoDto()
            {
                GrupoRecursoId = 5,
                GrupoRecurso = "Territorial",
                ValorSolicitado = 20600000,
                ValorInicial = 20600000,
                ValorVigente = null,
                Compromiso = null,
                Obligacion = null,
                Pago = null,
                ObjetivoEspecifico = auxObjectivoEspecifico
            });
            auxMes.Add(new MesCadenaValorDto()
            {
                Mes = 0,
                NombreMes = "Programación",
                GrupoRecurso = auxGrupoRecurso

            });
            auxVigencias.Add(new VigenciaCadenaValorDto()
            {
                Vigencia = 2017,
                Mes = auxMes,
            });
            var parametrosGuardar = new ParametrosGuardarDto<CadenaValorDto>()
            {
                AccionId = new Guid("418D76AC-F081-4D73-A05A-530CD7C6AFF6"),
                InstanciaId = new Guid("6A0E8FC3-73CF-46B5-B59C-0F683A30AE1D"),
                Usuario = "jdelgado",
                Contenido = new CadenaValorDto()
                {
                    Bpin = "2017005950118",
                    Vigencias = auxVigencias
                }
            };

            var parametroAuditoria = new ParametrosAuditoriaDto()
            {
                Usuario = "jdelgado",
                Ip = "localhost"
            };
            CadenaValorServicio.Guardar(parametrosGuardar, parametroAuditoria, false);
        }
    }
}
