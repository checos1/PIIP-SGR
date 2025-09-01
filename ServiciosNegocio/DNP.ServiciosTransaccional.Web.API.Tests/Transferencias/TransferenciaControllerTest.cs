namespace DNP.ServiciosTransaccional.Web.API.Test.Transferencias
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Security.Principal;
    using System.Threading.Tasks;
    using System.Web.Http;
    using System.Web.Http.Results;
    using Controllers.Transferencias;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Servicios.Interfaces.Transferencias;
    using ServiciosNegocio.Comunes.Autorizacion;
    using ServiciosNegocio.Dominio.Dto.Transferencias;
    using Unity;

    [TestClass]
    public sealed class TransferenciaControllerTest : IDisposable
    {
        private TransferenciaController _transferenciaController;
        private IAutorizacionUtilidades AutorizacionUtilizades { get; set; }
        private ITransferenciaServicio TransferenciaServicio { get; set; }

        [TestInitialize]
        public void Init()
        {
            var contenedor = Configuracion.UnityConfig.Container;
            TransferenciaServicio = contenedor.Resolve<ITransferenciaServicio>();
            AutorizacionUtilizades = contenedor.Resolve<IAutorizacionUtilidades>();

            _transferenciaController =
                new TransferenciaController(TransferenciaServicio, AutorizacionUtilizades)
                {
                    ControllerContext =
                    {
                        Request= new HttpRequestMessage()
                    }
                };
        }

        [TestMethod]
        public async Task IdentificarEntidadDEstinoTest_Valido()
        {
            _transferenciaController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _transferenciaController.User = new GenericPrincipal(new GenericIdentity("jcastano@dnp.gov.co", "Qwer1234"), new[] { "" });

            var result = (OkNegotiatedContentResult<HttpResponseMessage>)await _transferenciaController.Definitivo(new ObjetoNegocio()
            {
                ObjetoNegocioId = "240"
            });
            Assert.AreEqual(HttpStatusCode.OK, result.Content.StatusCode);
        }

        [TestMethod]
        public async Task IdentificarEntidadDEstinoTest_FalloAutorizacion()
        {
            try
            {
                _transferenciaController.ControllerContext.Request.Headers.Authorization =
                    new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");

                await _transferenciaController.Definitivo(new ObjetoNegocio());
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex.GetType() == typeof(InvalidCastException) || ex.GetType() == typeof(HttpResponseException));
            }
        }

        private void Dispose(bool disposing)
        {
            if (disposing) _transferenciaController.Dispose();
        }
        public void Dispose()
        {
            Dispose(true);
        }
    }
}
