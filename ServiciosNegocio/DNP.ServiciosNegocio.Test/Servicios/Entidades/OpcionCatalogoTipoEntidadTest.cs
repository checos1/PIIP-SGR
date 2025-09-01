using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using DNP.ServiciosNegocio.Dominio.Dto.Entidades;
using DNP.ServiciosNegocio.Persistencia.Interfaces;
using DNP.ServiciosNegocio.Servicios.Interfaces.Entidades;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Unity;
using DNP.ServiciosNegocio.Persistencia.Modelo;
using DNP.ServiciosNegocio.Servicios.Implementaciones.Entidades;
using DNP.ServiciosNegocio.Persistencia.Implementaciones.Entidades;
using DNP.ServiciosNegocio.Test.Configuracion;

namespace DNP.ServiciosNegocio.Test.Servicios.Entidades
{
    [TestClass]
    public class OpcionCatalogoTipoEntidadTest
    {
        #region Objetos

        public IEntidadServicios EntidadServicios { get; private set; }
        private OpcionCatalogoTipoEntidadDto Entidad { get; set; }

        public Mock<DbSet<EntityTypeCatalogOption>> MockSet = new Mock<DbSet<EntityTypeCatalogOption>>();
        public Mock<MGAWebContexto> MockContext = new Mock<MGAWebContexto>();
        public Mock<IContextoFactory> MockContextFactory = new Mock<IContextoFactory>();
        #endregion

        #region Inicializacion

        [TestInitialize]
        public void Init()
        {
            var contenedor = UnityConfig.Container;
            EntidadServicios = contenedor.Resolve<IEntidadServicios>();

            Entidad = new OpcionCatalogoTipoEntidadDto
            {

                Nombre = "test" + DateTime.Now.Ticks,
                CodigoEntidad = DateTime.Now.Ticks.ToString(),
                Id = 1777,
                IdTipo = 10,
                IdPadre = 0,
                EsActiva = false,
                Atributos = new AtributosEntidadDto()
                {
                    CabeceraSector = false,
                    Orden = "Nacional",
                    SectorId = 1,
                    CreadoPor = "userTest",
                    ModificadoPor = "userTest"
                }
            };

            var data = new List<EntityTypeCatalogOption>
            {
                new  EntityTypeCatalogOption
                {

                    Name = "test" + DateTime.Now.Ticks,
                    Code = DateTime.Now.Ticks.ToString(),
                    Id = 1777,
                    EntityTypeId = 10,
                    ParentId = 0,
                    IsActive = false,
                    AtributosEntidad = new AtributosEntidad()
                    {
                        CabeceraSector = false,
                        Orden = "Nacional",
                        CreadoPor = "userTest",
                        ModificadoPor = "userTest"
                    }
                }
            };

            MockSet.As<IQueryable<EntityTypeCatalogOption>>().Setup(m => m.Provider).Returns(data.AsQueryable().Provider);
            MockSet.As<IQueryable<EntityTypeCatalogOption>>().Setup(m => m.Expression).Returns(data.AsQueryable().Expression);
            MockSet.As<IQueryable<EntityTypeCatalogOption>>().Setup(m => m.ElementType).Returns(data.AsQueryable().ElementType);
            MockSet.As<IQueryable<EntityTypeCatalogOption>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            MockSet.Setup(m => m.Remove(It.IsAny<EntityTypeCatalogOption>())).Callback<EntityTypeCatalogOption>((entity) => data.Remove(entity));

            MockContext.Setup(m => m.EntityTypeCatalogOption).Returns(MockSet.Object);


            MockContextFactory.Setup(mcf => mcf.CrearContextoConConexion(ConfigurationManager.ConnectionStrings["MGAWebContexto"].ConnectionString)).Returns(MockContext.Object);
        }

        #endregion

        #region Metodos de pruebas
        [TestMethod]
        public void TestInsertBase()
        {
            var entidadServicio = new OpcionCatalogoTipoEntidadServicios(new EntidadPersistencia(MockContextFactory.Object));
            entidadServicio.InsertarEntidadBase(Entidad);

            MockSet.Verify(m => m.Add(It.IsAny<EntityTypeCatalogOption>()), Times.Once());
            MockContext.Verify(m => m.SaveChanges(), Times.Once());

            //var servicio= new OpcionCatalogoTipoEntidadServicios(new EntidadPersistencia(new ContextoFactory()));
            //servicio.InsertarEntidadBase(_entidadDto);
        }

        [TestMethod]
        public void TestUpdateBase()
        {
            var entidadServicio = new OpcionCatalogoTipoEntidadServicios(new EntidadPersistencia(MockContextFactory.Object));
            entidadServicio.InsertarEntidadBase(Entidad);

            Entidad.CodigoEntidad = "codigoModificado";
            Entidad.Atributos.Orden = "Territorial";
            entidadServicio.ActualizarEntidadBase(Entidad);

            MockSet.Verify(m => m.Add(It.IsAny<EntityTypeCatalogOption>()), Times.Once());
            MockContext.Verify(m => m.SaveChanges(), Times.Exactly(2));

            var entidadEsperada = entidadServicio.ConsultarEntidadBasePorId(Entidad.Id);
            Assert.AreEqual(((OpcionCatalogoTipoEntidadDto)entidadEsperada).CodigoEntidad, Entidad.CodigoEntidad);
           // Assert.AreEqual(((OpcionCatalogoTipoEntidadDto)entidadEsperada).Atributos.Orden, Entidad.Atributos.Orden);

        }

        [TestMethod]
        public void TestUpdateBase_EntidadInexistente()
        {
            var entidadServicio = new OpcionCatalogoTipoEntidadServicios(new EntidadPersistencia(MockContextFactory.Object));
            entidadServicio.InsertarEntidadBase(Entidad);

            Entidad.CodigoEntidad = "codigoModificado";
            Entidad.Atributos.Orden = "Territorial";
            var idOriginal = Entidad.Id;
            Entidad.Id = 1500;
            entidadServicio.ActualizarEntidadBase(Entidad);

            MockSet.Verify(m => m.Add(It.IsAny<EntityTypeCatalogOption>()), Times.Once());
            MockContext.Verify(m => m.SaveChanges(), Times.Once());

            var entidadEsperada = entidadServicio.ConsultarEntidadBasePorId(idOriginal);
            Assert.AreNotEqual(((OpcionCatalogoTipoEntidadDto)entidadEsperada).CodigoEntidad, Entidad.CodigoEntidad);
            //Assert.AreNotEqual(((OpcionCatalogoTipoEntidadDto)entidadEsperada).Atributos.Orden, Entidad.Atributos.Orden);

        }
        
        [TestMethod]
        public void TestSelectBase_porId()
        {
            var entidadServicio = new OpcionCatalogoTipoEntidadServicios(new EntidadPersistencia(MockContextFactory.Object));
            var entidad = entidadServicio.ConsultarEntidadBasePorId(Entidad.Id);
            Assert.IsNotNull(entidad);
        }
        
        [TestMethod]
        public void TestSelectBase_todos()
        {
            var entidadServicio = new OpcionCatalogoTipoEntidadServicios(new EntidadPersistencia(MockContextFactory.Object));
            var listadoEncontrado = entidadServicio.ConsultarEntidadBaseTodos();

            Assert.IsNotNull(listadoEncontrado.Count > 0);
        }
        
        [TestMethod]
        public void TestDeleteBase()
        {
            var entidadServicio = new OpcionCatalogoTipoEntidadServicios(new EntidadPersistencia(MockContextFactory.Object));
            entidadServicio.BorrarEntidadBase(Entidad);

            MockContext.Verify(x => x.SaveChanges(), Times.Once());
            MockSet.Verify(m => m.Remove(It.IsAny<EntityTypeCatalogOption>()), Times.Once());

        }
        
        #endregion
    }
}
