namespace DNP.ServiciosTransaccional.Web.API.Test.Proyecto
{
    using Controllers.Proyecto;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Servicios.Interfaces.Proyectos;
    using ServiciosNegocio.Comunes.Autorizacion;
    using ServiciosNegocio.Dominio.Dto.Transferencias;
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Security.Principal;
    using System.Threading.Tasks;
    using System.Web.Http;
    using System.Web.Http.Results;
    using Unity;

    [TestClass]
    public sealed class BpinControllerTest : IDisposable
    {
        private BpinController _bpinController;
        private IAutorizacionUtilidades AutorizacionUtilizades { get; set; }
        private IBpinServicio BpinServicio { get; set; }

        [TestInitialize]
        public void Init()
        {
            var contenedor = Configuracion.UnityConfig.Container;
            BpinServicio = contenedor.Resolve<IBpinServicio>();
            AutorizacionUtilizades = contenedor.Resolve<IAutorizacionUtilidades>();

            _bpinController =
                new BpinController(BpinServicio, AutorizacionUtilizades)
                {
                    ControllerContext =
                    {
                        Request= new HttpRequestMessage()
                    }
                };
        }

        [TestMethod]
        public async Task GenerarBPINProyectoTest_Valido()
        {
            _bpinController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _bpinController.User = new GenericPrincipal(new GenericIdentity("jcastano@dnp.gov.co", "Jocas123*"), new[] { "" });

            var result = (OkNegotiatedContentResult<HttpResponseMessage>)await _bpinController.GenerarBPIN(new ObjetoNegocio
            {
                ObjetoNegocioId = "240"
            });
            Assert.AreEqual(HttpStatusCode.OK, result.Content.StatusCode);
        }

        [TestMethod]
        public async Task GenerarBPINProyectoTest_FalloAutorizacion()
        {
            try
            {
                _bpinController.ControllerContext.Request.Headers.Authorization =
                    new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");

                await _bpinController.GenerarBPIN(new ObjetoNegocio());
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex.GetType() == typeof(InvalidCastException) || ex.GetType() == typeof(HttpResponseException));
            }
        }

        [TestMethod]
        public async Task GenerarBPINProyectoSGRTest_Valido()
        {
            _bpinController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _bpinController.User = new GenericPrincipal(new GenericIdentity("jcastano@dnp.gov.co", "Jocas123*"), new[] { "" });

            var result = (OkNegotiatedContentResult<HttpResponseMessage>)await _bpinController.GenerarBPINSgr(new ObjetoNegocio
            {
                ObjetoNegocioId = "240"
            });
            Assert.AreEqual(HttpStatusCode.OK, result.Content.StatusCode);
        }

        [TestMethod]
        public async Task GenerarBPINProyectoSGRTest_FalloAutorizacion()
        {
            try
            {
                _bpinController.ControllerContext.Request.Headers.Authorization =
                    new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");

                await _bpinController.GenerarBPINSgr(new ObjetoNegocio());
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex.GetType() == typeof(InvalidCastException) || ex.GetType() == typeof(HttpResponseException));
            }
        }

        private void Dispose(bool disposing)
        {
            if (disposing) _bpinController.Dispose();
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
}
