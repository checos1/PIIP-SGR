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
    public sealed class CreacionBpinControllerTest : IDisposable
    {
        private CreacionBpinController _creacionBpinController;
        private IAutorizacionUtilidades AutorizacionUtilizades { get; set; }
        private ICreacionBpinServicio CreacionBpinServicio { get; set; }

        [TestInitialize]
        public void Init()
        {
            var contenedor = Configuracion.UnityConfig.Container;
            CreacionBpinServicio = contenedor.Resolve<ICreacionBpinServicio>();
            AutorizacionUtilizades = contenedor.Resolve<IAutorizacionUtilidades>();

            _creacionBpinController =
                new CreacionBpinController(CreacionBpinServicio, AutorizacionUtilizades)
                {
                    ControllerContext =
                    {
                        Request= new HttpRequestMessage()
                    }
                };
        }

        [TestMethod]
        public async Task ValidarTransferencia_Valido()
        {
            _creacionBpinController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _creacionBpinController.User = new GenericPrincipal(new GenericIdentity("jcastano@dnp.gov.co", "CC202002"), new[] { "" });

            var result = (OkNegotiatedContentResult<HttpResponseMessage>)await _creacionBpinController.Definitivo(new ObjetoNegocio()
            {
                ObjetoNegocioId = "240"
            });
            Assert.AreEqual(HttpStatusCode.OK, result.Content.StatusCode);
        }

        [TestMethod]
        public async Task ValidarTransferencia_FalloAutorizacion()
        {
            try
            {
                _creacionBpinController.ControllerContext.Request.Headers.Authorization =
                    new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");

                await _creacionBpinController.Definitivo(new ObjetoNegocio());
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex.GetType() == typeof(InvalidCastException) || ex.GetType() == typeof(HttpResponseException));
            }
        }

        private void Dispose(bool disposing)
        {
            if (disposing) _creacionBpinController.Dispose();
        }
        public void Dispose()
        {
            Dispose(true);
        }
    }
}
