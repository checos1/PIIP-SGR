

namespace DNP.Backbone.Test.Servicios
{
    using Comunes.Dto;
    using DNP.Backbone.Servicios.Interfaces.Consola;
    using Microsoft.Practices.Unity;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;

    /// <summary>
    /// Summary description for ConsolaTramiteServicioTest
    /// </summary>
    [TestClass]
    public class ConsolaTramiteServicioTest
    {
        private IConsolaTramiteServicios _consolaTramiteServicios;

        [TestInitialize]
        public void Init()
        {
            _consolaTramiteServicios = Config.UnityConfig.Container.Resolve<IConsolaTramiteServicios>();
        }

        [TestMethod]
        public void ObtenerConsolaTramites_Ok()
        {
            var actionResult = _consolaTramiteServicios.ObtenerConsolaTramites(new InstanciaTramiteDto
            {
                ParametrosInboxDto = new ParametrosInboxDto
                {
                    IdUsuario = "jdelgado"
                }
            });

            Assert.IsNotNull(actionResult.Result);
        }

        [TestMethod]
        public void CuandoNoEnvioUsuarioObtenerConsolaTramites_RetornaExcepcion()
        {
            var actionResult = _consolaTramiteServicios.ObtenerConsolaTramites(new InstanciaTramiteDto());

            Assert.IsNull(actionResult.Result);
        }

        [TestMethod]
        public void ObtenerProyectosTramite_Ok()
        {
            var actionResult = _consolaTramiteServicios.ObtenerProyectosTramite(new InstanciaTramiteDto
            {
                ParametrosInboxDto = new ParametrosInboxDto
                {
                    IdUsuario = "jdelgado"
                }
            });

            Assert.IsNotNull(actionResult.Result);
        }

        [TestMethod]
        public void CuandoNoEnvioUsuarioObtenerProyectosTramite_RetornaExcepcion()
        {
            var actionResult = _consolaTramiteServicios.ObtenerConsolaTramites(new InstanciaTramiteDto());

            Assert.IsNull(actionResult.Result);
        }

        [TestMethod]
        public void ObtenerIdAplicacion_Ok()
        {
            var result = _consolaTramiteServicios.ObtenerIdAplicacionPorInstancia(Guid.NewGuid(), "jdelgado");
            Assert.IsNotNull(result);
        }
    }
}
