using DNP.ServiciosNegocio.Comunes.Autorizacion;
using DNP.ServiciosNegocio.Servicios.Interfaces.TramitesReprogramacion;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Unity;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web.Http.Results;
using DNP.ServiciosNegocio.Web.API.Controllers.Negocio;

namespace DNP.ServiciosNegocio.Web.API.Test.TramitesReprogramacion
{
    [TestClass]
    public class TramitesReprogramacionControllerTest : IDisposable
    {
        private ITramitesReprogramacionServicio TramitesReprogramacionServicio { get; set; }
        private IAutorizacionUtilidades AutorizacionUtilizades { get; set; }
        private TramitesReprogramacionController _tramitesReprogramacionController;

        [TestInitialize]
        public void Init()
        {
            var contenedor = Configuracion.UnityConfig.Container;
            TramitesReprogramacionServicio = contenedor.Resolve<ITramitesReprogramacionServicio>();
            AutorizacionUtilizades = contenedor.Resolve<IAutorizacionUtilidades>();
            _tramitesReprogramacionController = new TramitesReprogramacionController(TramitesReprogramacionServicio, AutorizacionUtilizades);
            _tramitesReprogramacionController.ControllerContext.Request = new HttpRequestMessage();
            _tramitesReprogramacionController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _tramitesReprogramacionController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:Backbone");
            _tramitesReprogramacionController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });
        }

        [TestMethod]
        public async Task ObtenerTramitesReprogramacionAnteriores_NoNulo()
        {
            var instanciaId = "00000000-0000-0000-0000-000000000000";
            int TramiteId = 453;
            int ProyectoId = 0;
            var result = (OkNegotiatedContentResult<string>)await _tramitesReprogramacionController.ObtenerResumenReprogramacionPorVigencia(new Guid(instanciaId), ProyectoId, TramiteId);
            Assert.IsNotNull(result.Content);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                // dispose managed resources
                _tramitesReprogramacionController.Dispose();
            }
            // free native resources
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
}
