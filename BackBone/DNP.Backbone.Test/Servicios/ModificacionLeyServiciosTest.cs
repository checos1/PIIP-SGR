using DNP.Backbone.Dominio.Dto.ModificacionLey;
using DNP.Backbone.Servicios.Interfaces.ModificacionLey;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DNP.Backbone.Test.Servicios
{
    [TestClass]
    public class ModificacionLeyServiciosTest
    {
        private IModificacionLeyServicios _modificacionLeyServicios;

        [TestInitialize]
        public void Init()
        {
            _modificacionLeyServicios = Config.UnityConfig.Container.Resolve<IModificacionLeyServicios>();
        }

        [TestMethod]
        public void ObtenerInformacionPresupuestalMLEncabezado_Ok()
        {
            //var actionResult = this._programacionServicios.ObtenerDatosProgramacionEncabezado(204, 2297, "distribucion");

            //Assert.IsNotNull(actionResult.Result);
            //Assert.IsTrue((actionResult.Result as OkNegotiatedContentResult<string>).Content.Any());
        }

        [TestMethod]
        public void ObtenerInformacionPresupuestalMLDetalle_Ok()
        {
            //var actionResult = this._programacionServicios.ObtenerDatosProgramacionDetalle(204, "distribucion");

            //Assert.IsNotNull(actionResult.Result);
            //Assert.IsTrue((actionResult.Result as OkNegotiatedContentResult<string>).Content.Any());
        }

        [TestMethod]
        public void GuardarInformacionPresupuestalML_Ok()
        {
            var actionResult = this._modificacionLeyServicios.GuardarInformacionPresupuestalML(new InformacionPresupuestalMLDto
            {
                TramiteProyectoId = 1,
                NivelId = "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0",
                SeccionCapitulo = 1
            }, "");

            Assert.IsTrue(actionResult.Result.Exito);
        }
    }
}
