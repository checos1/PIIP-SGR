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
    using ServiciosNegocio.Dominio.Dto.ValidarViabilidadCompletarInfo;
    using Unity;

    [TestClass]
    public sealed class ValidarViabilidadCompletarInfoControllerTest : IDisposable
    {
        private IValidarViabilidadCompletarInfoServicios ValidarViabilidadCompletarInfoServicios { get; set; }
        private IAutorizacionUtilidades AutorizacionUtilizades { get; set; }
        private ValidarViabilidadCompletarInfoController _validarViabilidadCompletarInfoController;
        private string Bpin { get; set; }

        [TestInitialize]
        public void Init()
        {
            Bpin = "2017011000236";
            var contenedor = Configuracion.UnityConfig.Container;
            ValidarViabilidadCompletarInfoServicios = contenedor.Resolve<IValidarViabilidadCompletarInfoServicios>();
            AutorizacionUtilizades = contenedor.Resolve<IAutorizacionUtilidades>();
            _validarViabilidadCompletarInfoController = new ValidarViabilidadCompletarInfoController(ValidarViabilidadCompletarInfoServicios, AutorizacionUtilizades);
            _validarViabilidadCompletarInfoController.ControllerContext.Request = new HttpRequestMessage();
        }

        [TestMethod]
        public async Task PValidarViabilidadCompletarInfoBpinValido()
        {
            _validarViabilidadCompletarInfoController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _validarViabilidadCompletarInfoController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });
            _validarViabilidadCompletarInfoController.ControllerContext.Request.Headers.Add("piip-idAccion", "E3C1849C-FE24-4C07-9762-036FA72AF10C");
            _validarViabilidadCompletarInfoController.ControllerContext.Request.Headers.Add("piip-idInstanciaFlujo", "4C2E62CD-CEAD-48EF-88C6-A50AB5913716");
            _validarViabilidadCompletarInfoController.ControllerContext.Request.Headers.Add("piip-idFormulario", "4C2E62CD-CEAD-48EF-88C6-A50AB5913716");

            var result = (OkNegotiatedContentResult<ValidarViabilidadCompletarInfoDto>)await _validarViabilidadCompletarInfoController.Consultar(Bpin);
            Assert.IsTrue(result.Content.Tematicas.Count > 0);
        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException))]
        public async Task PValidarViabilidadCompletarInfosinBpin()
        {
            _validarViabilidadCompletarInfoController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _validarViabilidadCompletarInfoController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });
            _validarViabilidadCompletarInfoController.ControllerContext.Request.Headers.Add("piip-idAccion", "E3C1849C-FE24-4C07-9762-036FA72AF10C");
            _validarViabilidadCompletarInfoController.ControllerContext.Request.Headers.Add("piip-idInstanciaFlujo", "4C2E62CD-CEAD-48EF-88C6-A50AB5913716");
            await _validarViabilidadCompletarInfoController.Consultar(string.Empty);
        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException))]
        public async Task PValidarViabilidadCompletarInfoSinBpinTexto()
        {
            _validarViabilidadCompletarInfoController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _validarViabilidadCompletarInfoController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });
            _validarViabilidadCompletarInfoController.ControllerContext.Request.Headers.Add("piip-idAccion", "E3C1849C-FE24-4C07-9762-036FA72AF10C");
            _validarViabilidadCompletarInfoController.ControllerContext.Request.Headers.Add("piip-idInstanciaFlujo", "4C2E62CD-CEAD-48EF-88C6-A50AB5913716");

            await _validarViabilidadCompletarInfoController.Consultar("Hola");
        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException))]
        public async Task PValidarViabilidadCompletarInfoSinBpinErroneo()
        {
            _validarViabilidadCompletarInfoController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _validarViabilidadCompletarInfoController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });
            _validarViabilidadCompletarInfoController.ControllerContext.Request.Headers.Add("piip-idAccion", "E3C1849C-FE24-4C07-9762-036FA72AF10C");
            _validarViabilidadCompletarInfoController.ControllerContext.Request.Headers.Add("piip-idInstanciaFlujo", "4C2E62CD-CEAD-48EF-88C6-A50AB5913716");

            await _validarViabilidadCompletarInfoController.Consultar("12345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901");
        }


        [TestMethod]
        public async Task PValidarViabilidadCompletarInfoPreview()
        {
            _validarViabilidadCompletarInfoController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _validarViabilidadCompletarInfoController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:Backbone");
            _validarViabilidadCompletarInfoController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });

            var result = (OkNegotiatedContentResult<ValidarViabilidadCompletarInfoDto>)await _validarViabilidadCompletarInfoController.Preview();
            Assert.IsNotNull(result.Content);
        }

        [TestMethod]
        public async Task PValidarViabilidadCompletarInfoConsultar_Ok()
        {
            _validarViabilidadCompletarInfoController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _validarViabilidadCompletarInfoController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:Backbone");
            _validarViabilidadCompletarInfoController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });

            _validarViabilidadCompletarInfoController.ControllerContext.Request.Headers.Add("piip-idAccion", "E3C1849C-FE24-4C07-9762-036FA72AF10C");
            _validarViabilidadCompletarInfoController.ControllerContext.Request.Headers.Add("piip-idInstanciaFlujo", "4C2E62CD-CEAD-48EF-88C6-A50AB5913716");
            _validarViabilidadCompletarInfoController.ControllerContext.Request.Headers.Add("piip-idFormulario", "4C2E62CD-CEAD-48EF-88C6-A50AB5913716");
            var result = (OkNegotiatedContentResult<ValidarViabilidadCompletarInfoDto>)await _validarViabilidadCompletarInfoController.Consultar(Bpin);
            Assert.IsNotNull(result.Content);
        }

        [TestMethod]
        public async Task PValidarViabilidadCompletarInfoConsultar_Failed()
        {
            _validarViabilidadCompletarInfoController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _validarViabilidadCompletarInfoController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:Backbone");
            _validarViabilidadCompletarInfoController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });

            _validarViabilidadCompletarInfoController.ControllerContext.Request.Headers.Add("piip-idAccion", "E3C1849C-FE24-4C07-9762-036FA72AF10C");
            _validarViabilidadCompletarInfoController.ControllerContext.Request.Headers.Add("piip-idInstanciaFlujo", "4C2E62CD-CEAD-48EF-88C6-A50AB5913716");
            _validarViabilidadCompletarInfoController.ControllerContext.Request.Headers.Add("piip-idFormulario", "4C2E62CD-CEAD-48EF-88C6-A50AB5913716");
            var result = (ResponseMessageResult)await _validarViabilidadCompletarInfoController.Consultar(Bpin + "1");
            Assert.AreEqual(HttpStatusCode.NotFound, result.Response.StatusCode);
        }

        [TestMethod]
        public async Task PValidarViabilidadCompletarInfoGuardarTemporal_OK()
        {
            _validarViabilidadCompletarInfoController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _validarViabilidadCompletarInfoController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:Backbone");
            _validarViabilidadCompletarInfoController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });

            _validarViabilidadCompletarInfoController.ControllerContext.Request.Headers.Add("piip-idAccion", "E3C1849C-FE24-4C07-9762-036FA72AF10C");
            _validarViabilidadCompletarInfoController.ControllerContext.Request.Headers.Add("piip-idInstanciaFlujo", "4C2E62CD-CEAD-48EF-88C6-A50AB5913716");

            var focalizacionProyecto = new ValidarViabilidadCompletarInfoDto();
            var result = (OkNegotiatedContentResult<HttpResponseMessage>)await _validarViabilidadCompletarInfoController.Temporal(focalizacionProyecto);
            Assert.AreEqual(HttpStatusCode.OK, result.Content.StatusCode);
        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException), "Parámetro de entrada piip-idAccion inválido.")]
        public async Task PValidarViabilidadCompletarInfoGuardarTemporalConidAccionInvalido__RetornaMensajeInvalido()
        {
            _validarViabilidadCompletarInfoController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _validarViabilidadCompletarInfoController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:Backbone");
            _validarViabilidadCompletarInfoController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });

            _validarViabilidadCompletarInfoController.Request.Headers.Add("piip-idAccion", "a2cffeb0-42a6-PRUEBA-ad4b-d674d65c40b1");
            _validarViabilidadCompletarInfoController.Request.Headers.Add("piip-idInstanciaFlujo", "");
            await _validarViabilidadCompletarInfoController.Temporal(new ValidarViabilidadCompletarInfoDto());

        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException), "Parámetro piip-idAccion no recibido.")]
        public async Task PValidarViabilidadCompletarInfoGuardarTemporalConidAccionVacio__RetornaMensajeNoRecibido()
        {
            _validarViabilidadCompletarInfoController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _validarViabilidadCompletarInfoController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:Backbone");
            _validarViabilidadCompletarInfoController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });

            _validarViabilidadCompletarInfoController.Request.Headers.Add("piip-idInstanciaFlujo", Guid.NewGuid().ToString());
            await _validarViabilidadCompletarInfoController.Temporal(new ValidarViabilidadCompletarInfoDto());

        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException), "Parámetro de entrada piip-idInstanciaFlujo inválido.")]
        public async Task PValidarViabilidadCompletarInfoGuardarTemporalConidInstanciaFlujoInvalido__RetornaMensajeInvalido()
        {
            _validarViabilidadCompletarInfoController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _validarViabilidadCompletarInfoController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:Backbone");
            _validarViabilidadCompletarInfoController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });

            _validarViabilidadCompletarInfoController.Request.Headers.Add("piip-idAccion", "");
            _validarViabilidadCompletarInfoController.Request.Headers.Add("piip-idInstanciaFlujo", "a2cffeb0-42a6-PRUEBA-ad4b-d674d65c40b1");
            await _validarViabilidadCompletarInfoController.Temporal(new ValidarViabilidadCompletarInfoDto());

        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException), "Parámetro piip-idInstanciaFlujo no recibido.")]
        public async Task PValidarViabilidadCompletarInfoGuardarTemporalConidInstanciaFlujoVacio__RetornaMensajeNoRecibido()
        {
            _validarViabilidadCompletarInfoController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _validarViabilidadCompletarInfoController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:Backbone");
            _validarViabilidadCompletarInfoController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });

            _validarViabilidadCompletarInfoController.Request.Headers.Add("piip-idAccion", Guid.NewGuid().ToString());
            await _validarViabilidadCompletarInfoController.Temporal(new ValidarViabilidadCompletarInfoDto());
        }

        [TestMethod]
        public async Task PValidarViabilidadCompletarInfoGuardarDefinitivo_OK()
        {
            _validarViabilidadCompletarInfoController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _validarViabilidadCompletarInfoController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:Backbone");
            _validarViabilidadCompletarInfoController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });

            _validarViabilidadCompletarInfoController.ControllerContext.Request.Headers.Add("piip-idAccion", "E3C1849C-FE24-4C07-9762-036FA72AF10C");
            _validarViabilidadCompletarInfoController.ControllerContext.Request.Headers.Add("piip-idInstanciaFlujo", "4C2E62CD-CEAD-48EF-88C6-A50AB5913716");

            var focalizacionProyecto = new ValidarViabilidadCompletarInfoDto();
            var result = (OkNegotiatedContentResult<HttpResponseMessage>)await _validarViabilidadCompletarInfoController.Definitivo(focalizacionProyecto);
            Assert.AreEqual(HttpStatusCode.OK, result.Content.StatusCode);
        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException), "Parámetro de entrada piip-idAccion inválido.")]
        public async Task PValidarViabilidadCompletarInfoGuardarDefinitivoConidAccionInvalido__RetornaMensajeInvalido()
        {
            _validarViabilidadCompletarInfoController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _validarViabilidadCompletarInfoController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:Backbone");
            _validarViabilidadCompletarInfoController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });

            _validarViabilidadCompletarInfoController.Request.Headers.Add("piip-idAccion", "a2cffeb0-42a6-PRUEBA-ad4b-d674d65c40b1");
            _validarViabilidadCompletarInfoController.Request.Headers.Add("piip-idInstanciaFlujo", "");
            await _validarViabilidadCompletarInfoController.Temporal(new ValidarViabilidadCompletarInfoDto());

        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException), "Parámetro piip-idAccion no recibido.")]
        public async Task PValidarViabilidadCompletarInfoGuardarDefinitivoConidAccionVacio__RetornaMensajeNoRecibido()
        {
            _validarViabilidadCompletarInfoController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _validarViabilidadCompletarInfoController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:Backbone");
            _validarViabilidadCompletarInfoController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });

            _validarViabilidadCompletarInfoController.Request.Headers.Add("piip-idInstanciaFlujo", Guid.NewGuid().ToString());
            await _validarViabilidadCompletarInfoController.Temporal(new ValidarViabilidadCompletarInfoDto());

        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException), "Parámetro de entrada piip-idInstanciaFlujo inválido.")]
        public async Task PValidarViabilidadCompletarInfoGuardarDefinitivoConidInstanciaFlujoInvalido__RetornaMensajeInvalido()
        {
            _validarViabilidadCompletarInfoController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _validarViabilidadCompletarInfoController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:Backbone");
            _validarViabilidadCompletarInfoController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });

            _validarViabilidadCompletarInfoController.Request.Headers.Add("piip-idAccion", "");
            _validarViabilidadCompletarInfoController.Request.Headers.Add("piip-idInstanciaFlujo", "a2cffeb0-42a6-PRUEBA-ad4b-d674d65c40b1");
            await _validarViabilidadCompletarInfoController.Temporal(new ValidarViabilidadCompletarInfoDto());

        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException), "Parámetro piip-idInstanciaFlujo no recibido.")]
        public async Task PValidarViabilidadCompletarInfoGuardarDefinitivoConidInstanciaFlujoVacio__RetornaMensajeNoRecibido()
        {
            _validarViabilidadCompletarInfoController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _validarViabilidadCompletarInfoController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:Backbone");
            _validarViabilidadCompletarInfoController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });

            _validarViabilidadCompletarInfoController.Request.Headers.Add("piip-idAccion", Guid.NewGuid().ToString());
            await _validarViabilidadCompletarInfoController.Temporal(new ValidarViabilidadCompletarInfoDto());
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                // dispose managed resources
                _validarViabilidadCompletarInfoController.Dispose();
            }
            // free native resources
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
}
