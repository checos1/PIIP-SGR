using DNP.ServiciosNegocio.Persistencia.Implementaciones.Transversales;
using DNP.ServiciosNegocio.Persistencia.Interfaces;
using DNP.ServiciosNegocio.Persistencia.Interfaces.Transversales;
using DNP.ServiciosNegocio.Persistencia.Modelo;
using DNP.ServiciosNegocio.Servicios.Implementaciones.Transversales;
using DNP.ServiciosNegocio.Test.Configuracion;
using DNP.ServiciosNegocio.Test.Mock;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Core.Objects;
using Unity;

namespace DNP.ServiciosNegocio.Test.Servicios.Transversales
{
    [TestClass]
    public class VigenciaServicioTest
    {
        private IVigenciaPersistencia VigenciaPersistencia { get; set; }

        private readonly Mock<MGAWebContexto> _mockContext = new Mock<MGAWebContexto>();
        private readonly Mock<IContextoFactory> _mockContextFactory = new Mock<IContextoFactory>();

        [TestInitialize]
        public void Init()
        {
            var contenedor = UnityConfig.Container;
            VigenciaPersistencia = contenedor.Resolve<IVigenciaPersistencia>();
            var mockEstados = new Mock<ObjectResult<int?>>();

            mockEstados.SetupReturn(0);

            _mockContext.Setup(mc => mc.uspGetVigencias()).Returns(mockEstados.Object);

            _mockContextFactory.Setup(mcf => mcf.CrearContextoConConexion(ConfigurationManager.ConnectionStrings["MGAWebContexto"].ConnectionString)).Returns(_mockContext.Object);
        }
        [TestMethod]
        public void ObtenerVigencias_Test()
        {
            var resultado = new VigenciaServicio(new VigenciaPersistencia(_mockContextFactory.Object));
            var result = resultado.ObtenerVigencias();

            Assert.IsNotNull(result);
        }
    }
}
