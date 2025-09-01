namespace DNP.ServiciosTransaccional.Web.API.Test.Tramites
{
    using Controllers.Tramites;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Servicios.Interfaces.Tramites;
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
    public sealed class TramiteControllerTest : IDisposable
    {
        private TramiteController _tramiteController;
        private IAutorizacionUtilidades AutorizacionUtilizades { get; set; }
        private ITramiteServicio TramiteServicio { get; set; }

        [TestInitialize]
        public void Init()
        {
            var contenedor = Configuracion.UnityConfig.Container;
            TramiteServicio = contenedor.Resolve<ITramiteServicio>();
            AutorizacionUtilizades = contenedor.Resolve<IAutorizacionUtilidades>();

            _tramiteController =
                new TramiteController(TramiteServicio, AutorizacionUtilizades)
                {
                    ControllerContext =
                    {
                        Request= new HttpRequestMessage()
                    }
                };
        }


        [TestMethod]
        public async Task ConsultarCargueExcelTest()
        {
            try
            {
                _tramiteController.ControllerContext.Request.Headers.Authorization =
                    new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");

                await _tramiteController.ConsultarCargueExcel(new ObjetoNegocio());
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex.GetType() == typeof(InvalidCastException) || ex.GetType() == typeof(HttpResponseException));
            }

        }


        [TestMethod]
        public async Task ActualizarCargueMasivoTest()
        {
            try
            {
                _tramiteController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
                _tramiteController.User = new GenericPrincipal(new GenericIdentity("jcastano@dnp.gov.co", "Jocas123*"), new[] { "" });
                _tramiteController.ControllerContext.Request.Headers.Authorization =
                    new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");

                await _tramiteController.ActualizarCargueMasivo(new ObjetoNegocio());
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex.GetType() == typeof(InvalidCastException) || ex.GetType() == typeof(HttpResponseException));
            }

        }


        private void Dispose(bool disposing)
        {
            if (disposing) _tramiteController.Dispose();
        }

        public void Dispose()
        {
            Dispose(true);
        }


    }
}
