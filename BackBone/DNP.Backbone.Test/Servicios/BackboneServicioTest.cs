namespace DNP.Backbone.Test.Servicios
{
    using Backbone.Servicios.Implementaciones;
    using Backbone.Servicios.Interfaces;
    using Microsoft.Practices.Unity;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    
    [TestClass]
    public class BackboneServicioTest
    {

        private BackboneServicios _backboneServicios;

        [TestInitialize]
        public void Init()
        {
            _backboneServicios = new BackboneServicios();
        }
        [TestMethod]
        public void ConsultarNotificacionPorResponsable()
        {
            var resultado = _backboneServicios.ConsultarNotificacionPorResponsable("jdelgado").Result;
            Assert.IsTrue(resultado.Count == 0);
        }
    }
}
