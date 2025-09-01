namespace DNP.ServiciosWBS.Web.API.Tests.Controllers
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Security.Principal;
    using System.Threading.Tasks;
    using System.Web.Http;
    using System.Web.Http.Results;
    using API.Controllers;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Servicios.Interfaces;
    using ServiciosNegocio.Comunes.Autorizacion;
    using ServiciosNegocio.Dominio.Dto.Formulario;
    using ServiciosNegocio.Dominio.Dto.Proyectos;
    using Unity;

    [TestClass]
    public class LocalizacionControllerTest
    {
        private ILocalizacionServicios LocalizacionServicios { get; set; }
        private IAutorizacionUtilidades AutorizacionUtilizades { get; set; }
        private LocalizacionController _localizacionController;
        private string Bpin { get; set; }

        [TestInitialize]
        public void Init()
        {
            Bpin = "202000000000005";
            var contenedor = Configuracion.UnityConfig.Container;
            LocalizacionServicios = contenedor.Resolve<ILocalizacionServicios>();
            AutorizacionUtilizades = contenedor.Resolve<IAutorizacionUtilidades>();

            _localizacionController = new LocalizacionController(LocalizacionServicios, AutorizacionUtilizades);
            _localizacionController.ControllerContext.Request = new HttpRequestMessage();

            _localizacionController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _localizacionController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:PIIP");
            _localizacionController.ControllerContext.Request.Headers.Add("piip-idAccion", "E1166DAB-B9C8-42D7-B51C-433D99A4765F");
            _localizacionController.ControllerContext.Request.Headers.Add("piip-idInstanciaFlujo", "DE4AB25A-0A06-4C00-B57B-BE9CFF3AB320");
            _localizacionController.ControllerContext.Request.Headers.Add("piip-idFormulario", "EE0892B7-76F8-409D-BEBB-8EB64CC13123");
            _localizacionController.User = new GenericPrincipal(new GenericIdentity("jcastano@dnp.gov.co", "Jocas123*"), new[] { "" });
        }

        [TestMethod]
        public async Task LocalizacionBpinValido()
        {
            var result = (OkNegotiatedContentResult<LocalizacionProyectoDto>)await _localizacionController.Consultar(Bpin);
            Assert.IsTrue(result.Content.NuevaLocalizacion.Count > 0);
        }

        [TestMethod]
        public async Task LocalizacionResultadoVacio()
        {
            var resultados = await _localizacionController.Consultar("202000000000003");
            Assert.IsNull(((ResponseMessageResult)resultados).Response.Content);
        }

        [TestMethod]
        public async Task LocalizacionPreview()
        {
            var result = (OkNegotiatedContentResult<LocalizacionProyectoDto>)await _localizacionController.Preview();
            Assert.IsNotNull(result.Content);
        }

        [TestMethod]
        public async Task LocalizacionGuardarTemporal()
        {
            _localizacionController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _localizacionController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:PIIP");
            _localizacionController.User = new GenericPrincipal(new GenericIdentity("jcastano@dnp.gov.co", "Jocas123*"), new[] { "" });

            _localizacionController.ControllerContext.Request.Headers.Add("piip-idAccion", "E1166DAB-B9C8-42D7-B51C-433D99A4765F");
            _localizacionController.ControllerContext.Request.Headers.Add("piip-idInstanciaFlujo", "DE4AB25A-0A06-4C00-B57B-BE9CFF3AB320");

            var localizacion = new LocalizacionProyectoDto();
            var result = (OkNegotiatedContentResult<HttpResponseMessage>)await _localizacionController.Temporal(localizacion);
            Assert.AreEqual(HttpStatusCode.OK, result.Content.StatusCode);
        }

        [TestMethod]
        public async Task LocalizacionGuardarDefinitivo()
        {
            _localizacionController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _localizacionController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:PIIP");
            _localizacionController.User = new GenericPrincipal(new GenericIdentity("jcastano@dnp.gov.co", "Jocas123*"), new[] { "" });

            _localizacionController.ControllerContext.Request.Headers.Add("piip-idAccion", "E1166DAB-B9C8-42D7-B51C-433D99A4765F");
            _localizacionController.ControllerContext.Request.Headers.Add("piip-idInstanciaFlujo", "DE4AB25A-0A06-4C00-B57B-BE9CFF3AB320");

            var localizacion = new LocalizacionProyectoDto();
            var result = (OkNegotiatedContentResult<HttpResponseMessage>)await _localizacionController.Definitivo(localizacion);
            Assert.AreEqual(HttpStatusCode.OK, result.Content.StatusCode);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                // dispose managed resources
                _localizacionController.Dispose();
            }
            // free native resources
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
}
