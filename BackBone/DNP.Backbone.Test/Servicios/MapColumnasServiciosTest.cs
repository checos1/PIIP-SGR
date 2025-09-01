namespace DNP.Backbone.Test.Servicios
{
    using DNP.Backbone.Comunes.Dto;
    using DNP.Backbone.Servicios.Interfaces.Monitoreo;
    using Microsoft.Practices.Unity;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class MapColumnasServiciosTest
    {
        private IMapColumnasServicios _mapColumnasServicios;

        [TestInitialize]
        public void Init()
        {
            _mapColumnasServicios = Config.UnityConfig.Container.Resolve<IMapColumnasServicios>();          
        }

        [TestMethod]
        public void CuandoNoEnvioUsuarioObtenerMapColumnas_RetornaExcepcion()
        {
            var actionResult = _mapColumnasServicios.ObtenerMapColumnas(new MapColumnasFiltroDto());
            Assert.IsNull(actionResult.Result);
        }

        [TestMethod]
        public void ObtenerMapColumnas_OK()
        {
            var actionResult = _mapColumnasServicios.ObtenerMapColumnas(new MapColumnasFiltroDto { 
                ParametrosDto = new ParametrosDto { IdUsuarioDNP = "jdelgado" } });

            Assert.IsNotNull(actionResult.Result);
        }

    }
}
