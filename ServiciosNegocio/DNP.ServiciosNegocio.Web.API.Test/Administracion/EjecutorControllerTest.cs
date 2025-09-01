using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http.Results;
using DNP.ServiciosNegocio.Comunes.Interfaces;
using DNP.ServiciosNegocio.Persistencia.Interfaces;
using DNP.ServiciosNegocio.Persistencia.Modelo;
using DNP.ServiciosNegocio.Servicios.Interfaces.Acciones;
using DNP.ServiciosNegocio.Web.API.Controllers.Acciones;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Unity;
using DNP.ServiciosNegocio.Dominio.Dto.Acciones;
using DNP.ServiciosNegocio.Persistencia.Implementaciones.Entidades;
using DNP.ServiciosNegocio.Servicios.Implementaciones.Acciones;
using DNP.ServiciosNegocio.Servicios.Implementaciones.Entidades;
using DNP.ServiciosNegocio.Servicios.Implementaciones.Transversales;
using DNP.ServiciosNegocio.Comunes.Autorizacion;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Principal;
using DNP.ServiciosNegocio.Servicios.Interfaces.Administracion;
using DNP.ServiciosNegocio.Web.API.Controllers.Negocio;
using DNP.ServiciosNegocio.Servicios.Implementaciones.Administracion;
using DNP.ServiciosNegocio.Persistencia.Implementaciones.Administracion;
using DNP.ServiciosNegocio.Dominio.Dto.Administracion;
using System.Net;
using DNP.ServiciosNegocio.Comunes;
using DNP.ServiciosNegocio.Dominio.Dto.Catalogos;

namespace DNP.ServiciosNegocio.Web.API.Test.Acciones
{

    [TestClass]
    public sealed class EjecutorControllerTest : IDisposable
    {
        #region Objetos

        private IEjecutorServicio EjecutorServicio { get; set; }
        private IAutorizacionUtilidades AutorizacionUtilidades { get; set; }
        private EjecutorController _accionController;

        private string EntidadJson { get; set; }
        private string Usuario { get; set; }
        private AccionFormularioDto _accionObjeto = new AccionFormularioDto();

        private Mock<DbSet<Ejecutor>> MockSet = new Mock<DbSet<Ejecutor>>();
        private Mock<MGAWebContexto> MockContext = new Mock<MGAWebContexto>();
        private Mock<IContextoFactory> MockContextFactory = new Mock<IContextoFactory>();

        private Mock<IAccionUtilidades> MockUtilidades = new Mock<IAccionUtilidades>();

        #endregion

        #region Inicializacion

        [TestInitialize]
        public void Init()
        {
            var contenedor = Configuracion.UnityConfig.Container;
            EjecutorServicio = contenedor.Resolve<IEjecutorServicio>();
            AutorizacionUtilidades = contenedor.Resolve<IAutorizacionUtilidades>();
            _accionController = new EjecutorController(EjecutorServicio, AutorizacionUtilidades);
            

            EntidadJson = "{\n  \"IsActive\": false,\n  \"Code\": \"codigoTest\",\n  \"ParentId\": null,\n  \"EntityTypeId\": 1,\n  \"Name\": \"EntidadTest\",\n  \"Id\": 1777\n}";

            Usuario = "jdelgado";


            _accionObjeto = new AccionFormularioDto()
            {
                ObjetoDatos = new ObjetoDatosDto
                {
                    IdEntidad = 1,
                    ObjetoJson = EntidadJson
                },
                IdInstanciaFlujo = new Guid("62A02F76-7CF2-4A97-A535-B3C3F1137355"),
                IdInstanciaAccion = new Guid("63667C03-CAAD-47A1-9C14-9FC4E5BB5FA6")
            };

            var data = new List<Ejecutor>
            {
                new Ejecutor
                {
                    Id = 1,
                    Nit = "899999103",
                    Digito = "1",
                    NombreEjecutor = "test",
                    EntityTypeId = 1,
                    CreadoPor = "Datos Semilla",
                    Activo = true,
                    FechaCreacion = DateTime.Now,
                    EntityType = new EntityType()
                },
                new Ejecutor
                {
                    Id = 2,
                    Nit = "899999104",
                    Digito = "2",
                    NombreEjecutor = "test1",
                    EntityTypeId = 1,
                    CreadoPor = "Datos Semilla",
                    Activo = true,
                    FechaCreacion = DateTime.Now,
                    EntityType = new EntityType()
                }
            }.AsQueryable();


            MockSet.As<IQueryable<Ejecutor>>().Setup(m => m.Provider).Returns(data.Provider);
            MockSet.As<IQueryable<Ejecutor>>().Setup(m => m.Expression).Returns(data.Expression);
            MockSet.As<IQueryable<Ejecutor>>().Setup(m => m.ElementType).Returns(data.ElementType);
            MockSet.As<IQueryable<Ejecutor>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            MockContext.Setup(m => m.Ejecutores).Returns(MockSet.Object);
            MockContextFactory.Setup(mcf => mcf.CrearContextoConConexion(ConfigurationManager.ConnectionStrings["MGAWebContexto"].ConnectionString)).Returns(MockContext.Object);
        }

        #endregion

        #region Metodos de Prueba

        [TestMethod]
        public async Task ConsultarEjecutor_Ok()
        {
            var servicio = new EjecutorServicio(new EjecutorPersistencia(MockContextFactory.Object));
            _accionController = new EjecutorController(servicio, AutorizacionUtilidades);
            _accionController.ControllerContext.Request = new HttpRequestMessage();
            _accionController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _accionController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });
            string nit = "899999103";
            var result = (OkNegotiatedContentResult<EjecutorDto>)await _accionController.ConsultarEjecutor(nit);
            Assert.IsNotNull(result.Content);
        }

        [TestMethod]
        public async Task ConsultarEjecutor_NotFound()
        {
            var servicio = new EjecutorServicio(new EjecutorPersistencia(MockContextFactory.Object));
            _accionController = new EjecutorController(servicio, AutorizacionUtilidades);
            _accionController.ControllerContext.Request = new HttpRequestMessage();
            _accionController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _accionController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });
            string nit = "899999123";
            var result = (ResponseMessageResult) await _accionController.ConsultarEjecutor(nit);
            Assert.AreEqual(HttpStatusCode.NotFound, result.Response.StatusCode);
            Assert.AreEqual(ServiciosNegocioRecursos.SinResultados, result.Response.ReasonPhrase);
        }

        [TestMethod]
        public async Task GuardarEjecutor_Ok()
        {
            var servicio = new EjecutorServicio(new EjecutorPersistencia(MockContextFactory.Object));
            _accionController = new EjecutorController(servicio, AutorizacionUtilidades);
            _accionController.ControllerContext.Request = new HttpRequestMessage();
            _accionController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _accionController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });
            
            var ejecutor = new EjecutorDto
            {
                Id = 3,
                Nit = "899999124",
                Digito = "2",
                NombreEjecutor = "Test Creación",
                EntityTypeId = 1,
                CreadoPor = "Datos Semilla",
                Activo = true,
                FechaCreacion = DateTime.Now
            };

            var result = (OkNegotiatedContentResult<bool>)await _accionController.GuardarEjecutor(ejecutor);
            Assert.IsTrue(result.Content);
        }

        [TestMethod]
        public async Task GuardarEjecutor_Unauthorized()
        {
            var servicio = new EjecutorServicio(new EjecutorPersistencia(MockContextFactory.Object));
            _accionController = new EjecutorController(servicio, AutorizacionUtilidades);
            _accionController.ControllerContext.Request = new HttpRequestMessage();
            _accionController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _accionController.User = new GenericPrincipal(new GenericIdentity("jdelgado1", "Qwer1234"), new[] { "" });

            var ejecutor = new EjecutorDto
            {
                Id = 3,
                Nit = "899999124",
                Digito = "2",
                NombreEjecutor = "Test Creación",
                EntityTypeId = 1,
                CreadoPor = "Datos Semilla",
                Activo = true,
                FechaCreacion = DateTime.Now
            };

            var result = (ResponseMessageResult) await _accionController.GuardarEjecutor(ejecutor);
            Assert.AreEqual(HttpStatusCode.Unauthorized, result.Response.StatusCode);
        }
        #endregion

        public void Dispose()
        {
            _accionController.Dispose();
        }
    }
}
