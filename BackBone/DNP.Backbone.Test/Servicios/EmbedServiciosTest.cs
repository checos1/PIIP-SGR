namespace DNP.Backbone.Test.Servicios
{
    using DNP.Backbone.Comunes.Dto.PowerBI;
    using DNP.Backbone.Servicios.Interfaces.PowerBI;
    using Microsoft.Practices.Unity;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class EmbedServiciosTest
    {
        private IEmbedServicios _embedServicios;

        [TestInitialize]
        public void Init()
        {
            _embedServicios = Config.UnityConfig.Container.Resolve<IEmbedServicios>();          
        }

        [TestMethod]
        public void Obtener_invormaciones_dashborad_powerBI_OK()
        {
            EmbedFiltroDto embed = new EmbedFiltroDto();
            var actionResult = _embedServicios.ObtenerDashboard(embed).Result;
            Assert.AreEqual(string.Empty, actionResult.ErrorMessage);
        }

        [TestMethod]
        public void Obtener_invormaciones_reportes_powerBI_OK()
        {
            EmbedFiltroDto embed = new EmbedFiltroDto();
            var actionResult = _embedServicios.ObtenerReportes(embed).Result;
            Assert.AreEqual(string.Empty, actionResult.ErrorMessage);
        }
    }
}
