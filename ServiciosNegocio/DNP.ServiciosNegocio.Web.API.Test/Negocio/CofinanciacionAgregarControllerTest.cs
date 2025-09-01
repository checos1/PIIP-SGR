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
    public sealed class CofinanciacionAgregarControllerTest : IDisposable
    {
        private ICofinanciacionAgregarServicio CofinanciacionAgregarServicio { get; set; }
        private IAutorizacionUtilidades AutorizacionUtilizades { get; set; }
        private CofinanciacionAgregarController _cofinanciacionAgregarController;
        private string Bpin { get; set; }

        [TestInitialize]
        public void Init()
        {
            Bpin = "202000000000005";
            var contenedor = Configuracion.UnityConfig.Container;
            CofinanciacionAgregarServicio = contenedor.Resolve<ICofinanciacionAgregarServicio>();
            AutorizacionUtilizades = contenedor.Resolve<IAutorizacionUtilidades>();

            _cofinanciacionAgregarController =
                new CofinanciacionAgregarController(CofinanciacionAgregarServicio, AutorizacionUtilizades)
                {
                    ControllerContext
                        =
                        {
                            Request
                                = new
                                    HttpRequestMessage()
                        }
                };

            _cofinanciacionAgregarController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _cofinanciacionAgregarController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:PIIP");
            _cofinanciacionAgregarController.ControllerContext.Request.Headers.Add("piip-idAccion", "D2BA19EB-0487-4C94-8960-3A6047B81409");
            _cofinanciacionAgregarController.ControllerContext.Request.Headers.Add("piip-idInstanciaFlujo", "BC135467-5041-4C0F-8AB7-EC2F09E02AAF");
            _cofinanciacionAgregarController.ControllerContext.Request.Headers.Add("piip-idFormulario", "ADA6BAAC-F2DD-4664-AAB7-9E8AB61405CC");
            _cofinanciacionAgregarController.User = new GenericPrincipal(new GenericIdentity("jcastano@dnp.gov.co", "Jocas123*"), new[] { "" });
        }

        [TestMethod]
        public async Task CofinanciacionAgregarProyectoBpinValido()
        {
            var resultados = (OkNegotiatedContentResult<CofinanciacionProyectoDto>)await _cofinanciacionAgregarController.Consultar(Bpin);
            Assert.IsNotNull(resultados.Content);
        }

        [TestMethod]
        public async Task CofinanciacionAgregarProyectoResultadoVacio()
        {
            var resultados = await _cofinanciacionAgregarController.Consultar("202000000000004");
            Assert.IsNull(((ResponseMessageResult)resultados).Response.Content);
        }

        [TestMethod]
        public async Task CofinanciacionAgregarProyectoPreviewTest()
        {
            var result = (OkNegotiatedContentResult<CofinanciacionProyectoDto>)await _cofinanciacionAgregarController.Preview();
            Assert.IsNotNull(result.Content);
            Assert.IsTrue(result.Content.Cofinanciacion.Count == 1);
        }

        [TestMethod]
        public async Task CofinanciacionAgregarProyectoGuardarDefinitivo_OK()
        {
            _cofinanciacionAgregarController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _cofinanciacionAgregarController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:PIIP");
            _cofinanciacionAgregarController.User = new GenericPrincipal(new GenericIdentity("jcastano@dnp.gov.co", "Jocas123*"), new[] { "" });

            _cofinanciacionAgregarController.ControllerContext.Request.Headers.Add("piip-idAccion", "D2BA19EB-0487-4C94-8960-3A6047B81409");
            _cofinanciacionAgregarController.ControllerContext.Request.Headers.Add("piip-idInstanciaFlujo", "BC135467-5041-4C0F-8AB7-EC2F09E02AAF");

            var cofinanciacionAgregarDto = new CofinanciacionProyectoDto();
            var result = (OkNegotiatedContentResult<HttpResponseMessage>)await _cofinanciacionAgregarController.Definitivo(cofinanciacionAgregarDto);
            Assert.AreEqual(HttpStatusCode.OK, result.Content.StatusCode);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                // dispose managed resources
                _cofinanciacionAgregarController.Dispose();
            }
            // free native resources
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
}
