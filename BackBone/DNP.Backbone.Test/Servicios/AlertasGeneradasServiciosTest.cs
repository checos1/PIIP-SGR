namespace DNP.Backbone.Test.Servicios
{
    using DNP.Backbone.Servicios.Interfaces.Monitoreo;
    using Microsoft.Practices.Unity;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class AlertasGeneradasServiciosTest
    {
        private IAlertasGeneradasServicios _alertasGeneradasServicios;

        [TestInitialize]
        public void Init()
        {
            _alertasGeneradasServicios = Config.UnityConfig.Container.Resolve<IAlertasGeneradasServicios>();          
        }

        [TestMethod]
        public void ObtenerAlertasGeneradas_OK()
        {
            var actionResult = _alertasGeneradasServicios.ObtenerAlertasGeneradas(new Comunes.Dto.AlertasGeneradasFiltroDto());
            Assert.IsNotNull(actionResult.Result);
        }

    }
}
