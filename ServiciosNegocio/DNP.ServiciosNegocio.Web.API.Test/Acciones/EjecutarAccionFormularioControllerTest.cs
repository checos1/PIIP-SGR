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

namespace DNP.ServiciosNegocio.Web.API.Test.Acciones
{

    [TestClass]
    public sealed class EjecutarAccionFormularioControllerTest : IDisposable
    {
        #region Objetos

        private IEjecucionAccionTransaccionalServicios EjecucionAccionTransaccionalServicios { get; set; }
        private IAutorizacionUtilidades AutorizacionUtilidades { get; set; }
        private EjecutarAccionFormularioController _accionController;

        private string EntidadJson { get; set; }
        private string Usuario { get; set; }
        private AccionFormularioDto _accionObjeto = new AccionFormularioDto();

        private Mock<DbSet<EntityTypeCatalogOption>> MockSet = new Mock<DbSet<EntityTypeCatalogOption>>();
        private Mock<MGAWebContexto> MockContext = new Mock<MGAWebContexto>();
        private Mock<IContextoFactory> MockContextFactory = new Mock<IContextoFactory>();

        private Mock<IAccionUtilidades> MockUtilidades = new Mock<IAccionUtilidades>();

        #endregion

        #region Inicializacion

        [TestInitialize]
        public void Init()
        {
            var contenedor = Configuracion.UnityConfig.Container;
            EjecucionAccionTransaccionalServicios = contenedor.Resolve<IEjecucionAccionTransaccionalServicios>();
            AutorizacionUtilidades = contenedor.Resolve<IAutorizacionUtilidades>();
            _accionController = new EjecutarAccionFormularioController(EjecucionAccionTransaccionalServicios, AutorizacionUtilidades);
            

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

            var data = new List<EntityTypeCatalogOption>
            {
                new EntityTypeCatalogOption
                {
                    AtributosEntidad = null,
                    Id = 1777,
                    Name = "test",
                    EntityTypeId = 1,
                    Code = "testCode",
                    IsActive = false,
                    ParentId = null
                },
                new EntityTypeCatalogOption
                {
                    AtributosEntidad = null,
                    Id = 1778,
                    Name = "test",
                    EntityTypeId = 1,
                    Code = "testCode",
                    IsActive = false,
                    ParentId = null
                }

            }.AsQueryable();


            MockSet.As<IQueryable<EntityTypeCatalogOption>>().Setup(m => m.Provider).Returns(data.Provider);
            MockSet.As<IQueryable<EntityTypeCatalogOption>>().Setup(m => m.Expression).Returns(data.Expression);
            MockSet.As<IQueryable<EntityTypeCatalogOption>>().Setup(m => m.ElementType).Returns(data.ElementType);
            MockSet.As<IQueryable<EntityTypeCatalogOption>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            MockContext.Setup(m => m.EntityTypeCatalogOption).Returns(MockSet.Object);
            MockContextFactory.Setup(mcf => mcf.CrearContextoConConexion(ConfigurationManager.ConnectionStrings["MGAWebContexto"].ConnectionString)).Returns(MockContext.Object);

            MockUtilidades.Setup(a => a.ExisteInstancia(_accionObjeto.IdInstanciaFlujo, Usuario)).Returns(true);
            MockUtilidades.Setup(a => a.ExisteAccionActiva(_accionObjeto.IdInstanciaAccion, Usuario)).Returns(true);
        }

        #endregion

        #region Metodos de Prueba

        [TestMethod]
        public async Task EjecutarAccionTest_Ok()
        {
            var ejecucionAccionTransaccionalServicios = new EjecucionAccionTransaccionalEntidadServicios(new OpcionCatalogoTipoEntidadServicios(new EntidadPersistencia(MockContextFactory.Object)), MockUtilidades.Object, new AuditoriaServicios()) { Usuario = Usuario };
            _accionController = new EjecutarAccionFormularioController(ejecucionAccionTransaccionalServicios, AutorizacionUtilidades);
            _accionController.ControllerContext.Request = new HttpRequestMessage();
            _accionController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _accionController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });

            var result = (OkNegotiatedContentResult<bool>)await _accionController.EjecutarAccion(_accionObjeto);
            Assert.IsTrue(result.Content);
        }

        [TestMethod]
        public async Task EjecutarAccionTest_False()
        {
            var ejecucionAccionTransaccionalServicios = new EjecucionAccionTransaccionalEntidadServicios(new OpcionCatalogoTipoEntidadServicios(new EntidadPersistencia(MockContextFactory.Object)), MockUtilidades.Object, new AuditoriaServicios()) { Usuario = Usuario };
            _accionController = new EjecutarAccionFormularioController(ejecucionAccionTransaccionalServicios, AutorizacionUtilidades);
            _accionController.ControllerContext.Request = new HttpRequestMessage();
            _accionController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _accionController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });

            var _accionObjeto = new AccionFormularioDto()
            {
                ObjetoDatos = new ObjetoDatosDto
                {
                    IdEntidad = 2,
                    ObjetoJson = EntidadJson
                },
                IdInstanciaFlujo = new Guid("62A02F76-7CF2-4A97-A535-B3C3F1137355"),
                IdInstanciaAccion = new Guid("63667C03-CAAD-47A1-9C14-9FC4E5BB5FA6")
            };

            var result = (OkNegotiatedContentResult<bool>)await _accionController.EjecutarAccion(_accionObjeto);
            Assert.IsFalse(result.Content);
        }
        #endregion

        public void Dispose()
        {
            _accionController.Dispose();
        }
    }
}
