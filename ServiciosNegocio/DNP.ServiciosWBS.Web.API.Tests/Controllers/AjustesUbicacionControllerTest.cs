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
    public class AjustesUbicacionControllerTest
    {
        private IAjustesUbicacionServicios AjustesUbicacionServicios { get; set; }
        private IAutorizacionUtilidades AutorizacionUtilizades { get; set; }
        private AjustesUbicacionController _ajustesUbicacionController;
        private string Bpin { get; set; }

        [TestInitialize]
        public void Init()
        {
            Bpin = "202000000000005";
            var contenedor = Configuracion.UnityConfig.Container;
            AjustesUbicacionServicios = contenedor.Resolve<IAjustesUbicacionServicios>();
            AutorizacionUtilizades = contenedor.Resolve<IAutorizacionUtilidades>();

            _ajustesUbicacionController = new AjustesUbicacionController(AjustesUbicacionServicios, AutorizacionUtilizades);
            _ajustesUbicacionController.ControllerContext.Request = new HttpRequestMessage();

            _ajustesUbicacionController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _ajustesUbicacionController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:PIIP");
            _ajustesUbicacionController.ControllerContext.Request.Headers.Add("piip-idAccion", "8E22189D-8FDE-47E1-883A-DEBD3C15A314");
            _ajustesUbicacionController.ControllerContext.Request.Headers.Add("piip-idInstanciaFlujo", "2C7A9B06-C342-4060-9EBC-0F9EC17AF398");
            _ajustesUbicacionController.ControllerContext.Request.Headers.Add("piip-idFormulario", "53A3EA11-D2C6-49FE-8498-2D2A7C139D69");
            _ajustesUbicacionController.User = new GenericPrincipal(new GenericIdentity("jcastano@dnp.gov.co", "Jocas123*"), new[] { "" });
        }

        [TestMethod]
        public async Task AjustesUbicacionBpinValido()
        {
            var result = (OkNegotiatedContentResult<AjustesUbicacionDto>)await _ajustesUbicacionController.Consultar(Bpin);
            Assert.IsTrue(result.Content.NuevaLocalizacion.Count > 0);
        }

        [TestMethod]
        public async Task AjustesUbicacionResultadoVacio()
        {
            var resultados = await _ajustesUbicacionController.Consultar("202000000000003");
            Assert.IsNull(((ResponseMessageResult)resultados).Response.Content);
        }

        [TestMethod]
        public async Task AjustesUbicacionPreview()
        {
            var result = (OkNegotiatedContentResult<AjustesUbicacionDto>)await _ajustesUbicacionController.Preview();
            Assert.IsNotNull(result.Content);
        }

        [TestMethod]
        public async Task AjustesUbicacionGuardarTemporal()
        {
            _ajustesUbicacionController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _ajustesUbicacionController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:PIIP");
            _ajustesUbicacionController.User = new GenericPrincipal(new GenericIdentity("jcastano@dnp.gov.co", "Jocas123*"), new[] { "" });

            _ajustesUbicacionController.ControllerContext.Request.Headers.Add("piip-idAccion", "8E22189D-8FDE-47E1-883A-DEBD3C15A314");
            _ajustesUbicacionController.ControllerContext.Request.Headers.Add("piip-idInstanciaFlujo", "2C7A9B06-C342-4060-9EBC-0F9EC17AF398");

            var localizacion = new AjustesUbicacionDto();
            var result = (OkNegotiatedContentResult<HttpResponseMessage>)await _ajustesUbicacionController.Temporal(localizacion);
            Assert.AreEqual(HttpStatusCode.OK, result.Content.StatusCode);
        }

        [TestMethod]
        public async Task AjustesUbicacionGuardarDefinitivo()
        {
            _ajustesUbicacionController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _ajustesUbicacionController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:PIIP");
            _ajustesUbicacionController.User = new GenericPrincipal(new GenericIdentity("jcastano@dnp.gov.co", "Jocas123*"), new[] { "" });

            _ajustesUbicacionController.ControllerContext.Request.Headers.Add("piip-idAccion", "8E22189D-8FDE-47E1-883A-DEBD3C15A314");
            _ajustesUbicacionController.ControllerContext.Request.Headers.Add("piip-idInstanciaFlujo", "2C7A9B06-C342-4060-9EBC-0F9EC17AF398");

            var localizacion = new AjustesUbicacionDto();
            var result = (OkNegotiatedContentResult<HttpResponseMessage>)await _ajustesUbicacionController.Definitivo(localizacion);
            Assert.AreEqual(HttpStatusCode.OK, result.Content.StatusCode);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                // dispose managed resources
                _ajustesUbicacionController.Dispose();
            }
            // free native resources
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
}
