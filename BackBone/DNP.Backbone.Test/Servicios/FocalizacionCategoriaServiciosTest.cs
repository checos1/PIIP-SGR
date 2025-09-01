namespace DNP.Backbone.Test.Servicios
{
    using DNP.Backbone.Servicios.Interfaces.Focalizacion;
    using Microsoft.Practices.Unity;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class FocalizacionCategoriaServiciosTest
    {
        private ICategoriaProductosPoliticaServicios _categoriaProductosPoliticaServicios;

        [TestInitialize]
        public void Init()
        {
            _categoriaProductosPoliticaServicios = Config.UnityConfig.Container.Resolve<ICategoriaProductosPoliticaServicios>();
        }

        [TestMethod]
        public void ObtenerCategoriaProductosPolitica_Ok()
        {
            var actionResult = _categoriaProductosPoliticaServicios.ObtenerCategoriaProductosPolitica("202100000000007", 1281, 5, "CC505050", "");
            Assert.IsNotNull(actionResult.Result.ToString());
            Assert.IsTrue(string.IsNullOrEmpty(actionResult.Result));
        }

        [TestMethod]
        public void GuardarDatosSolicitudRecursos_Ok()
        {
            var actionResult = _categoriaProductosPoliticaServicios.GuardarDatosSolicitudRecursos(new Dominio.Dto.Focalizacion.CategoriaProductoPoliticaDto {  BPIN = "202100000000007" }, "CC505050", "");
            Assert.IsNotNull(actionResult.Result.ToString());
            Assert.IsTrue(string.IsNullOrEmpty(actionResult.Result));
        }
    }
}
