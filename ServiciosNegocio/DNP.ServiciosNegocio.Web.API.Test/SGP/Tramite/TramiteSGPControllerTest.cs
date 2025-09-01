using DNP.ServiciosNegocio.Comunes.Autorizacion;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Security.Principal;
using Unity;
using DNP.ServiciosNegocio.Web.API.Controllers.SGP.Tramite;
using DNP.ServiciosNegocio.Servicios.Interfaces.SGP.Tramite;

namespace DNP.ServiciosNegocio.Web.API.Test.SGP.Tramite
{
    [TestClass]
    public sealed class TramiteSGPControllerTest : IDisposable
    {
        private ITramiteSGPServicio tramiteSGPServicio { get; set; }
        private IAutorizacionUtilidades AutorizacionUtilidades { get; set; }
        private TramiteSGPController _tramiteSGPController;

        [TestInitialize]
        public void Init()
        {
            var contenedor = Configuracion.UnityConfig.Container;
            tramiteSGPServicio = contenedor.Resolve<ITramiteSGPServicio>();
            AutorizacionUtilidades = contenedor.Resolve<IAutorizacionUtilidades>();

            _tramiteSGPController =
                new TramiteSGPController(tramiteSGPServicio, AutorizacionUtilidades)
                {
                    ControllerContext
                    =
                    {
                        Request
                        = new
                                    HttpRequestMessage()
                    }
                };

            _tramiteSGPController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _tramiteSGPController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:PIIP");
            _tramiteSGPController.ControllerContext.Request.Headers.Add("piip-idAccion", "D2BA19EB-0487-4C94-8960-3A6047B81409");
            _tramiteSGPController.ControllerContext.Request.Headers.Add("piip-idInstanciaFlujo", "BC135467-5041-4C0F-8AB7-EC2F09E02AAF");
            _tramiteSGPController.ControllerContext.Request.Headers.Add("piip-idFormulario", "ADA6BAAC-F2DD-4664-AAB7-9E8AB61405CC");
            _tramiteSGPController.User = new GenericPrincipal(new GenericIdentity("jcastano@dnp.gov.co", "Jocas123*"), new[] { "" });
        }



        public void Dispose()
        {
            Dispose(true);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                // dispose managed resources
                _tramiteSGPController.Dispose();
            }
            // free native resources
        }
    }
}
