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
    using Unity;

    [TestClass]
    public class CadenaValorControllerTest : IDisposable
    {
        private ICadenaValorServicios CadenaValorServicios { get; set; }
        private IAutorizacionUtilidades AutorizacionUtilizades { get; set; }
        private CadenaValorController _cadenaValorController;
        private string Bpin { get; set; }

        [TestInitialize]
        public void Init()
        {
            Bpin = "2017011000236";
            var contenedor = Configuracion.UnityConfig.Container;
            CadenaValorServicios = contenedor.Resolve<ICadenaValorServicios>();
            AutorizacionUtilizades = contenedor.Resolve<IAutorizacionUtilidades>();
            _cadenaValorController = new CadenaValorController(CadenaValorServicios, AutorizacionUtilizades);
            _cadenaValorController.ControllerContext.Request = new HttpRequestMessage();
        }


        [TestMethod]
        [ExpectedException(typeof(HttpResponseException))]
        public async Task CadenaValorSinBpin()
        {
            _cadenaValorController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _cadenaValorController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:Backbone");
            _cadenaValorController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });
            _cadenaValorController.ControllerContext.Request.Headers.Add("piip-idAccion", "E3C1849C-FE24-4C07-9762-036FA72AF10C");
            _cadenaValorController.ControllerContext.Request.Headers.Add("piip-idInstanciaFlujo", "4C2E62CD-CEAD-48EF-88C6-A50AB5913716");

            await _cadenaValorController.Consultar(string.Empty);
        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException))]
        public async Task CadenaValorSinBpinTexto()
        {
            _cadenaValorController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _cadenaValorController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:Backbone");
            _cadenaValorController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });

            _cadenaValorController.ControllerContext.Request.Headers.Add("piip-idAccion", "E3C1849C-FE24-4C07-9762-036FA72AF10C");
            _cadenaValorController.ControllerContext.Request.Headers.Add("piip-idInstanciaFlujo", "4C2E62CD-CEAD-48EF-88C6-A50AB5913716");
            await _cadenaValorController.Consultar("Hola");
        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException))]
        public async Task CadenaValorSinBpinErroneo()
        {
            _cadenaValorController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _cadenaValorController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:Backbone");
            _cadenaValorController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });

            _cadenaValorController.ControllerContext.Request.Headers.Add("piip-idAccion", "E3C1849C-FE24-4C07-9762-036FA72AF10C");
            _cadenaValorController.ControllerContext.Request.Headers.Add("piip-idInstanciaFlujo", "4C2E62CD-CEAD-48EF-88C6-A50AB5913716");
            await _cadenaValorController.Consultar("12345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901");
        }

        [TestMethod]
        public async Task CadenaValorPreview()
        {
            _cadenaValorController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _cadenaValorController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:Backbone");
            _cadenaValorController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });

            var result = (OkNegotiatedContentResult<object>)await _cadenaValorController.Preview();
            Assert.IsNotNull(result.Content);
        }

        [TestMethod]
        public async Task CadenaValorConsultar_Ok()
        {
            _cadenaValorController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _cadenaValorController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:Backbone");
            _cadenaValorController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });

            _cadenaValorController.ControllerContext.Request.Headers.Add("piip-idAccion", "E3C1849C-FE24-4C07-9762-036FA72AF10C");
            _cadenaValorController.ControllerContext.Request.Headers.Add("piip-idInstanciaFlujo", "4C2E62CD-CEAD-48EF-88C6-A50AB5913716");
            var result = (OkNegotiatedContentResult<CadenaValorDto>)await _cadenaValorController.Consultar(Bpin);
            Assert.IsNotNull(result.Content);
        }

        [TestMethod]
        public async Task CadenaValorConsultar_NoFound()
        {
            _cadenaValorController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _cadenaValorController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:Backbone");
            _cadenaValorController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });

            _cadenaValorController.ControllerContext.Request.Headers.Add("piip-idAccion", "E3C1849C-FE24-4C07-9762-036FA72AF10C");
            _cadenaValorController.ControllerContext.Request.Headers.Add("piip-idInstanciaFlujo", "4C2E62CD-CEAD-48EF-88C6-A50AB5913716");
            var result = (ResponseMessageResult) await _cadenaValorController.Consultar(Bpin + "1");
            Assert.AreEqual(HttpStatusCode.NotFound, result.Response.StatusCode);
        }

        [TestMethod]
        public async Task CadenaValorGuardarTemporal_OK()
        {
            _cadenaValorController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _cadenaValorController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:Backbone");
            _cadenaValorController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });

            _cadenaValorController.ControllerContext.Request.Headers.Add("piip-idAccion", "E3C1849C-FE24-4C07-9762-036FA72AF10C");
            _cadenaValorController.ControllerContext.Request.Headers.Add("piip-idInstanciaFlujo", "4C2E62CD-CEAD-48EF-88C6-A50AB5913716");

            var cadenaValor = new CadenaValorDto();
            var result = (OkNegotiatedContentResult<HttpResponseMessage>)await _cadenaValorController.Temporal(cadenaValor);
            Assert.AreEqual(HttpStatusCode.OK, result.Content.StatusCode);
        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException), "Parámetro de entrada piip-idAccion inválido.")]
        public async Task CadenaValorGuardarTemporalConidAccionInvalido__RetornaMensajeInvalido()
        {
            _cadenaValorController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _cadenaValorController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:Backbone");
            _cadenaValorController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });

            _cadenaValorController.Request.Headers.Add("piip-idAccion", "a2cffeb0-42a6-PRUEBA-ad4b-d674d65c40b1");
            _cadenaValorController.Request.Headers.Add("piip-idInstanciaFlujo", "");
            await _cadenaValorController.Temporal(new CadenaValorDto());

        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException), "Parámetro piip-idAccion no recibido.")]
        public async Task CadenaValorGuardarTemporalConidAccionVacio__RetornaMensajeNoRecibido()
        {
            _cadenaValorController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _cadenaValorController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:Backbone");
            _cadenaValorController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });

            _cadenaValorController.Request.Headers.Add("piip-idInstanciaFlujo", Guid.NewGuid().ToString());
            await _cadenaValorController.Temporal(new CadenaValorDto());

        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException), "Parámetro de entrada piip-idInstanciaFlujo inválido.")]
        public async Task CadenaValorGuardarTemporalConidInstanciaFlujoInvalido__RetornaMensajeInvalido()
        {
            _cadenaValorController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _cadenaValorController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:Backbone");
            _cadenaValorController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });

            _cadenaValorController.Request.Headers.Add("piip-idAccion", "");
            _cadenaValorController.Request.Headers.Add("piip-idInstanciaFlujo", "a2cffeb0-42a6-PRUEBA-ad4b-d674d65c40b1");
            await _cadenaValorController.Temporal(new CadenaValorDto());

        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException), "Parámetro piip-idInstanciaFlujo no recibido.")]
        public async Task CadenaValorGuardarTemporalConidInstanciaFlujoVacio__RetornaMensajeNoRecibido()
        {
            _cadenaValorController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _cadenaValorController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:Backbone");
            _cadenaValorController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });

            _cadenaValorController.Request.Headers.Add("piip-idAccion", Guid.NewGuid().ToString());
            await _cadenaValorController.Temporal(new CadenaValorDto());
        }

        [TestMethod]
        public async Task CadenaValorGuardarDefinitivo_OK()
        {
            _cadenaValorController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _cadenaValorController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:Backbone");
            _cadenaValorController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });

            _cadenaValorController.ControllerContext.Request.Headers.Add("piip-idAccion", "E3C1849C-FE24-4C07-9762-036FA72AF10C");
            _cadenaValorController.ControllerContext.Request.Headers.Add("piip-idInstanciaFlujo", "4C2E62CD-CEAD-48EF-88C6-A50AB5913716");

            var cadenaValor = new CadenaValorDto();
            var result = (OkNegotiatedContentResult<HttpResponseMessage>)await _cadenaValorController.Temporal(cadenaValor);
            Assert.AreEqual(HttpStatusCode.OK, result.Content.StatusCode);
        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException), "Parámetro de entrada piip-idAccion inválido.")]
        public async Task CadenaValorGuardarDefinitivoConidAccionInvalido__RetornaMensajeInvalido()
        {
            _cadenaValorController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _cadenaValorController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:Backbone");
            _cadenaValorController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });

            _cadenaValorController.Request.Headers.Add("piip-idAccion", "a2cffeb0-42a6-PRUEBA-ad4b-d674d65c40b1");
            _cadenaValorController.Request.Headers.Add("piip-idInstanciaFlujo", "");
            await _cadenaValorController.Temporal(new CadenaValorDto());

        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException), "Parámetro piip-idAccion no recibido.")]
        public async Task CadenaValorGuardarDefinitivoConidAccionVacio__RetornaMensajeNoRecibido()
        {
            _cadenaValorController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _cadenaValorController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:Backbone");
            _cadenaValorController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });

            _cadenaValorController.Request.Headers.Add("piip-idInstanciaFlujo", Guid.NewGuid().ToString());
            await _cadenaValorController.Temporal(new CadenaValorDto());

        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException), "Parámetro de entrada piip-idInstanciaFlujo inválido.")]
        public async Task CadenaValorGuardarDefinitivoConidInstanciaFlujoInvalido__RetornaMensajeInvalido()
        {
            _cadenaValorController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _cadenaValorController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:Backbone");
            _cadenaValorController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });

            _cadenaValorController.Request.Headers.Add("piip-idAccion", "");
            _cadenaValorController.Request.Headers.Add("piip-idInstanciaFlujo", "a2cffeb0-42a6-PRUEBA-ad4b-d674d65c40b1");
            await _cadenaValorController.Temporal(new CadenaValorDto());

        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException), "Parámetro piip-idInstanciaFlujo no recibido.")]
        public async Task CadenaValorGuardarDefinitivoConidInstanciaFlujoVacio__RetornaMensajeNoRecibido()
        {
            _cadenaValorController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _cadenaValorController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:Backbone");
            _cadenaValorController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });

            _cadenaValorController.Request.Headers.Add("piip-idAccion", Guid.NewGuid().ToString());
            await _cadenaValorController.Temporal(new CadenaValorDto());
        }

        #region Dispose

        public void Dispose()
        {
            Dispose(true);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                _cadenaValorController.Dispose();
            }
        }

        #endregion
    }
}
