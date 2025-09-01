namespace DNP.Backbone.Test.Servicios
{
    using DNP.Backbone.Servicios.Interfaces.Focalizacion;
    using Microsoft.Practices.Unity;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class FocalizacionIndPoliticaServiciosTest
    {
        private IIndicadoresPolitica _indicadoresPoliticaServicios;

        [TestInitialize]
        public void Init()
        {
            this._indicadoresPoliticaServicios = Config.UnityConfig.Container.Resolve<IIndicadoresPolitica>();
        }

        [TestMethod]
        public void ObtenerIndicadoresPolitica_Ok()
        {
            var actionResult = _indicadoresPoliticaServicios.ObtenerIndicadoresPolitica("202100000000007", "CC505050", "");
            Assert.IsNotNull(actionResult.Result.ToString());
            Assert.IsTrue(string.IsNullOrEmpty(actionResult.Result));
        }
    }
}
