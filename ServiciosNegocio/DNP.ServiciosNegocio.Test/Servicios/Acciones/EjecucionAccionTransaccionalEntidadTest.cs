using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using DNP.ServiciosNegocio.Comunes.Excepciones;
using DNP.ServiciosNegocio.Dominio.Dto.Acciones;
using DNP.ServiciosNegocio.Servicios.Interfaces.Acciones;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Unity;
using DNP.ServiciosNegocio.Comunes.Interfaces;
using DNP.ServiciosNegocio.Persistencia.Implementaciones.Entidades;
using DNP.ServiciosNegocio.Persistencia.Interfaces;
using DNP.ServiciosNegocio.Persistencia.Modelo;
using DNP.ServiciosNegocio.Servicios.Implementaciones.Acciones;
using DNP.ServiciosNegocio.Servicios.Implementaciones.Entidades;
using System.Linq;
using DNP.ServiciosNegocio.Comunes.Utilidades;
using DNP.ServiciosNegocio.Servicios.Implementaciones.Transversales;

namespace DNP.ServiciosNegocio.Test.Servicios.Acciones
{
    using Configuracion;

    [TestClass]
    public class EjecucionAccionTransaccionalEntidadTest
    {
        #region Objetos

        public IEjecucionAccionTransaccionalServicios EjecucionAccionTransaccionalServicios { get; private set; }
        public string EntidadJson { get; set; }
        public string EntidadJsonCompleja { get; set; }
        public string Usuario { get; set; }
        private AccionFormularioDto _accionObjeto = new AccionFormularioDto();

        public Mock<DbSet<EntityTypeCatalogOption>> MockSet = new Mock<DbSet<EntityTypeCatalogOption>>();
        public Mock<MGAWebContexto> MockContext = new Mock<MGAWebContexto>();
        public Mock<IContextoFactory> MockContextFactory = new Mock<IContextoFactory>();

        public Mock<IAccionUtilidades> MockUtilidades = new Mock<IAccionUtilidades>();
        #endregion

        #region Inicializacion

        [TestInitialize]
        public void Init()
        {
            var contenedor = UnityConfig.Container;
            EjecucionAccionTransaccionalServicios = contenedor.Resolve<IEjecucionAccionTransaccionalServicios>();

            EntidadJson = "{\n  \"IsActive\": false,\n  \"Code\": \"codigoTest\",\n  \"ParentId\": null,\n  \"EntityTypeId\": 1,\n  \"Name\": \"EntidadTest\",\n  \"Id\": 1777\n}";

            EntidadJsonCompleja = "{\"AtributosEntidad\": {\n    \"ModificadoPor\": \"David\",\n    \"CreadoPor\": \"DAvid\",\n    \"FechaModificacion\": \"2018-01-18\",\n    \"FechaCreacion\": \"2018-01-18\",\n    \"SectorId\": null,\n    \"Orden\": \"Nacional\",\n    \"CabeceraSector\": false,\n    \"Id\": 1234\n  },\n  \"IsActive\": false,\n  \"Code\": \"codigoTest\",\n  \"ParentId\": null,\n  \"EntityTypeId\": 1,\n  \"Name\": \"EntidadTest\",\n  \"Id\": 1778\n}";
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

        #region Metodos de prueba

        [TestMethod]
        public void TestSinAtributosGuardado_Valido()
        {
            var ejecucionAccionTransaccionalServicios = new EjecucionAccionTransaccionalEntidadServicios(new OpcionCatalogoTipoEntidadServicios(new EntidadPersistencia(MockContextFactory.Object)), MockUtilidades.Object, new AuditoriaServicios()) { Usuario = Usuario };
            ejecucionAccionTransaccionalServicios.EjecutarAccion(_accionObjeto);

            MockSet.Verify(m => m.Add(It.IsAny<EntityTypeCatalogOption>()), Times.Once());
            MockContext.Verify(m => m.SaveChanges(), Times.Once());
        }


        [TestMethod]
        public void TestConAtributosGuardado_Valido()
        {

            var ejecucionAccionTransaccionalServicios = new EjecucionAccionTransaccionalEntidadServicios(new OpcionCatalogoTipoEntidadServicios(new EntidadPersistencia(MockContextFactory.Object)), MockUtilidades.Object, new AuditoriaServicios()) { Usuario = Usuario };
            _accionObjeto.ObjetoDatos.ObjetoJson = EntidadJsonCompleja;
            ejecucionAccionTransaccionalServicios.EjecutarAccion(_accionObjeto);


            MockSet.Verify(m => m.Add(It.IsAny<EntityTypeCatalogOption>()), Times.Once());
            MockContext.Verify(m => m.SaveChanges(), Times.Once());

        }

        [TestMethod]
        [ExpectedException(typeof(AccionException))]
        public void TestInstanciaInexistente()
        {
            var ejecucionAccionTransaccionalServicios = new EjecucionAccionTransaccionalEntidadServicios(new OpcionCatalogoTipoEntidadServicios(new EntidadPersistencia(MockContextFactory.Object)), new AccionUtilidades(), new AuditoriaServicios()) { Usuario = Usuario };
            _accionObjeto.ObjetoDatos.ObjetoJson = EntidadJsonCompleja;
            ejecucionAccionTransaccionalServicios.EjecutarAccion(_accionObjeto);
        }


        [TestMethod]
        [ExpectedException(typeof(AccionException))]
        public void TestJsonVacio()
        {
            var ejecucionAccionTransaccionalServicios = new EjecucionAccionTransaccionalEntidadServicios(new OpcionCatalogoTipoEntidadServicios(new EntidadPersistencia(MockContextFactory.Object)), MockUtilidades.Object, new AuditoriaServicios()) { Usuario = Usuario };
            _accionObjeto.ObjetoDatos.ObjetoJson = null;
            ejecucionAccionTransaccionalServicios.EjecutarAccion(_accionObjeto);
        }

        #endregion


    }
}

