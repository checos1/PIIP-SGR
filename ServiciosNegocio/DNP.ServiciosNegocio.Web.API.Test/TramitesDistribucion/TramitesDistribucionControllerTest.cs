using DNP.ServiciosNegocio.Comunes.Autorizacion;
using DNP.ServiciosNegocio.Servicios.Interfaces.TramitesDistribucion;
using DNP.ServiciosNegocio.Web.API.Controllers.TramitesDistribucion;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Unity;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web.Http.Results;

namespace DNP.ServiciosNegocio.Web.API.Test.TramitesDistribucion
{
    [TestClass]
    public class TramitesDistribucionControllerTest : IDisposable
    {
        private ITramitesDistribucionServicio TramitesDistribucionServicio { get; set; }
        private IAutorizacionUtilidades AutorizacionUtilizades { get; set; }
        private TramitesDistribucionController _tramitesDistribucionController;

        [TestInitialize]
        public void Init()
        {
            var contenedor = Configuracion.UnityConfig.Container;
            TramitesDistribucionServicio = contenedor.Resolve<ITramitesDistribucionServicio>();
            AutorizacionUtilizades = contenedor.Resolve<IAutorizacionUtilidades>();
            _tramitesDistribucionController = new TramitesDistribucionController(TramitesDistribucionServicio, AutorizacionUtilizades);
            _tramitesDistribucionController.ControllerContext.Request = new HttpRequestMessage();
            _tramitesDistribucionController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _tramitesDistribucionController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:Backbone");
            _tramitesDistribucionController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });
        }

        [TestMethod]
        public async Task ObtenerTramitesDistribucionAnteriores_NoNulo()
        {
            var instanciaId = "00000000-0000-0000-0000-000000000000";
            var result = (OkNegotiatedContentResult<string>)await _tramitesDistribucionController.ObtenerTramitesDistribucionAnteriores(new Guid(instanciaId));
            Assert.IsNotNull(result.Content);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                // dispose managed resources
                _tramitesDistribucionController.Dispose();
            }
            // free native resources
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
}
