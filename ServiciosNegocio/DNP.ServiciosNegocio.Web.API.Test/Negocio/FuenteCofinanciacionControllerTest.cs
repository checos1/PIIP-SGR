using DNP.ServiciosNegocio.Servicios.Interfaces.FuenteFinanciacion;
using DNP.ServiciosNegocio.Web.API.Controllers.Negocio;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Unity;
using System.Threading.Tasks;
using System.Web.Http.Results;
using DNP.ServiciosNegocio.Dominio.Dto.FuenteFinanciacion;
using System;
using DNP.ServiciosNegocio.Comunes.Autorizacion;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Principal;
using System.Net;

namespace DNP.ServiciosNegocio.Web.API.Test.Negocio
{
    [TestClass]
    public sealed class FuenteCofinanciacionControllerTest : IDisposable
    {
        private IFuenteCofinanciacionServicio FuenteCofinanciacionServicio { get; set; }
        private IAutorizacionUtilidades AutorizacionUtilizades { get; set; }
        private FuenteCofinanciacionController _fuenteCofinanciacionController;
        private string Bpin { get; set; }

        [TestInitialize]
        public void Init()
        {
            Bpin = "202000000000005";
            var contenedor = Configuracion.UnityConfig.Container;
            FuenteCofinanciacionServicio = contenedor.Resolve<IFuenteCofinanciacionServicio>();
            AutorizacionUtilizades = contenedor.Resolve<IAutorizacionUtilidades>();

            _fuenteCofinanciacionController =
                new FuenteCofinanciacionController(FuenteCofinanciacionServicio, AutorizacionUtilizades)
                {
                    ControllerContext
                        =
                        {
                            Request
                                = new
                                    HttpRequestMessage()
                        }
                };

            _fuenteCofinanciacionController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _fuenteCofinanciacionController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:PIIP");
            _fuenteCofinanciacionController.ControllerContext.Request.Headers.Add("piip-idAccion", "48ACE8E7-37DB-BF71-7CE5-4996ED17DEA1");
            _fuenteCofinanciacionController.ControllerContext.Request.Headers.Add("piip-idInstanciaFlujo", "1CC3A855-12F1-4113-A044-014886298AA3");
            _fuenteCofinanciacionController.ControllerContext.Request.Headers.Add("piip-idFormulario", "48ACE8E7-37DB-BF71-7CE5-4996ED17DEA1");
            _fuenteCofinanciacionController.User = new GenericPrincipal(new GenericIdentity("jcastano@dnp.gov.co", "Jocas123*"), new[] { "" });
        }

        [TestMethod]
        public async Task FuenteCofinanciacionProyectoBpinValido()
        {
            var resultados = (OkNegotiatedContentResult<FuenteCofinanciacionProyectoDto>)await _fuenteCofinanciacionController.Consultar(Bpin);
            Assert.IsNotNull(resultados.Content);
        }

        [TestMethod]
        public async Task FuenteCofinanciacionProyectoResultadoVacio()
        {
            var resultados = await _fuenteCofinanciacionController.Consultar("202000000000004");
            Assert.IsNull(((ResponseMessageResult)resultados).Response.Content);
        }

        [TestMethod]
        public async Task FuenteCofinanciacionProyectoPreviewTest()
        {
            var result = (OkNegotiatedContentResult<FuenteCofinanciacionProyectoDto>)await _fuenteCofinanciacionController.Preview();
            Assert.IsNotNull(result.Content);
            Assert.IsTrue(result.Content.Cofinanciacion.Count == 1);
        }

        [TestMethod]
        public async Task FuenteCofinanciacionProyectoGuardarDefinitivo_OK()
        {
            _fuenteCofinanciacionController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _fuenteCofinanciacionController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:PIIP");
            _fuenteCofinanciacionController.User = new GenericPrincipal(new GenericIdentity("jcastano@dnp.gov.co", "Jocas123*"), new[] { "" });

            _fuenteCofinanciacionController.ControllerContext.Request.Headers.Add("piip-idAccion", "48ACE8E7-37DB-BF71-7CE5-4996ED17DEA1");
            _fuenteCofinanciacionController.ControllerContext.Request.Headers.Add("piip-idInstanciaFlujo", "1CC3A855-12F1-4113-A044-014886298AA3");

            var proyectoFuenteCofinanciacionDto = new FuenteCofinanciacionProyectoDto();
            var result = (OkNegotiatedContentResult<HttpResponseMessage>)await _fuenteCofinanciacionController.Definitivo(proyectoFuenteCofinanciacionDto);
            Assert.AreEqual(HttpStatusCode.OK, result.Content.StatusCode);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                // dispose managed resources
                _fuenteCofinanciacionController.Dispose();
            }
            // free native resources
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
}
