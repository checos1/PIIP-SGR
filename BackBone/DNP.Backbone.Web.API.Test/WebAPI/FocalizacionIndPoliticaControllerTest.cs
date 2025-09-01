using DNP.Backbone.Servicios.Interfaces.Focalizacion;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.Backbone.Web.API.Test.WebAPI
{
    public class FocalizacionIndPoliticaControllerTest
    {
        private IIndicadoresPolitica _indicadoresPoliticaServicios;

        [TestInitialize]
        public void Init()
        {
            this._indicadoresPoliticaServicios = Config.UnityConfig.Container.Resolve<IIndicadoresPolitica>();
        }

        [TestMethod]
        public void ObtenerAdherencia_Ok()
        {
            var actionResult = _indicadoresPoliticaServicios.ObtenerIndicadoresPolitica("202100000000007", "CC505050", "");
            Assert.IsNotNull(actionResult.Result.ToString());
            Assert.IsTrue(string.IsNullOrEmpty(actionResult.Result));
        }
    }
}
