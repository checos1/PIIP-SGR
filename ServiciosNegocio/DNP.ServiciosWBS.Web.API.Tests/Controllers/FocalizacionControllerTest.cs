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
    using ServiciosNegocio.Dominio.Dto.FocalizacionProyecto;
    using Unity;

    [TestClass]
    public sealed class FocalizacionControllerTest : IDisposable
    {
        private IFocalizacionServicios FocalizacionServicios { get; set; }
        private IAutorizacionUtilidades AutorizacionUtilizades { get; set; }
        private FocalizacionController _focalizacionController;
        private string Bpin { get; set; }

        [TestInitialize]
        public void Init()
        {
            Bpin = "2017011000236";
            var contenedor = Configuracion.UnityConfig.Container;
            FocalizacionServicios = contenedor.Resolve<IFocalizacionServicios>();
            AutorizacionUtilizades = contenedor.Resolve<IAutorizacionUtilidades>();
            _focalizacionController = new FocalizacionController(FocalizacionServicios, AutorizacionUtilizades);
            _focalizacionController.ControllerContext.Request = new HttpRequestMessage();
        }

        [TestMethod]
        public async Task PFocalizacionProyectoBpinValido()
        {
            _focalizacionController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _focalizacionController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });
            _focalizacionController.ControllerContext.Request.Headers.Add("piip-idAccion", "E3C1849C-FE24-4C07-9762-036FA72AF10C");
            _focalizacionController.ControllerContext.Request.Headers.Add("piip-idInstanciaFlujo", "4C2E62CD-CEAD-48EF-88C6-A50AB5913716");
            _focalizacionController.ControllerContext.Request.Headers.Add("piip-idFormulario", "4C2E62CD-CEAD-48EF-88C6-A50AB5913716");

            var result = (OkNegotiatedContentResult<FocalizacionProyectoDto>)await _focalizacionController.Consultar(Bpin);
            Assert.IsTrue(result.Content.Politicas.Count > 0);
        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException))]
        public async Task PFocalizacionProyectosinBpin()
        {
            _focalizacionController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _focalizacionController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });
            _focalizacionController.ControllerContext.Request.Headers.Add("piip-idAccion", "E3C1849C-FE24-4C07-9762-036FA72AF10C");
            _focalizacionController.ControllerContext.Request.Headers.Add("piip-idInstanciaFlujo", "4C2E62CD-CEAD-48EF-88C6-A50AB5913716");
            await _focalizacionController.Consultar(string.Empty);
        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException))]
        public async Task PFocalizacionProyectoSinBpinTexto()
        {
            _focalizacionController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _focalizacionController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });
            _focalizacionController.ControllerContext.Request.Headers.Add("piip-idAccion", "E3C1849C-FE24-4C07-9762-036FA72AF10C");
            _focalizacionController.ControllerContext.Request.Headers.Add("piip-idInstanciaFlujo", "4C2E62CD-CEAD-48EF-88C6-A50AB5913716");

            await _focalizacionController.Consultar("Hola");
        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException))]
        public async Task PFocalizacionProyectoSinBpinErroneo()
        {
            _focalizacionController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _focalizacionController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });
            _focalizacionController.ControllerContext.Request.Headers.Add("piip-idAccion", "E3C1849C-FE24-4C07-9762-036FA72AF10C");
            _focalizacionController.ControllerContext.Request.Headers.Add("piip-idInstanciaFlujo", "4C2E62CD-CEAD-48EF-88C6-A50AB5913716");

            await _focalizacionController.Consultar("12345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901");
        }


        [TestMethod]
        public async Task PFocalizacionProyectoPreview()
        {
            _focalizacionController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _focalizacionController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:Backbone");
            _focalizacionController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });

            var result = (OkNegotiatedContentResult<FocalizacionProyectoDto>)await _focalizacionController.Preview();
            Assert.IsNotNull(result.Content);
        }

        [TestMethod]
        public async Task PFocalizacionProyectoConsultar_Ok()
        {
            _focalizacionController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _focalizacionController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:Backbone");
            _focalizacionController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });

            _focalizacionController.ControllerContext.Request.Headers.Add("piip-idAccion", "E3C1849C-FE24-4C07-9762-036FA72AF10C");
            _focalizacionController.ControllerContext.Request.Headers.Add("piip-idInstanciaFlujo", "4C2E62CD-CEAD-48EF-88C6-A50AB5913716");
            _focalizacionController.ControllerContext.Request.Headers.Add("piip-idFormulario", "4C2E62CD-CEAD-48EF-88C6-A50AB5913716");
            var result = (OkNegotiatedContentResult<FocalizacionProyectoDto>)await _focalizacionController.Consultar(Bpin);
            Assert.IsNotNull(result.Content);
        }

        [TestMethod]
        public async Task PFocalizacionProyectoConsultar_Failed()
        {
            _focalizacionController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _focalizacionController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:Backbone");
            _focalizacionController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });

            _focalizacionController.ControllerContext.Request.Headers.Add("piip-idAccion", "E3C1849C-FE24-4C07-9762-036FA72AF10C");
            _focalizacionController.ControllerContext.Request.Headers.Add("piip-idInstanciaFlujo", "4C2E62CD-CEAD-48EF-88C6-A50AB5913716");
            _focalizacionController.ControllerContext.Request.Headers.Add("piip-idFormulario", "4C2E62CD-CEAD-48EF-88C6-A50AB5913716");
            var result = (ResponseMessageResult)await _focalizacionController.Consultar(Bpin + "1");
            Assert.AreEqual(HttpStatusCode.NotFound, result.Response.StatusCode);
        }

        [TestMethod]
        public async Task PFocalizacionProyectoGuardarTemporal_OK()
        {
            _focalizacionController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _focalizacionController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:Backbone");
            _focalizacionController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });

            _focalizacionController.ControllerContext.Request.Headers.Add("piip-idAccion", "E3C1849C-FE24-4C07-9762-036FA72AF10C");
            _focalizacionController.ControllerContext.Request.Headers.Add("piip-idInstanciaFlujo", "4C2E62CD-CEAD-48EF-88C6-A50AB5913716");

            var focalizacionProyecto = new FocalizacionProyectoDto();
            var result = (OkNegotiatedContentResult<HttpResponseMessage>)await _focalizacionController.Temporal(focalizacionProyecto);
            Assert.AreEqual(HttpStatusCode.OK, result.Content.StatusCode);
        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException), "Parámetro de entrada piip-idAccion inválido.")]
        public async Task PFocalizacionProyectoGuardarTemporalConidAccionInvalido__RetornaMensajeInvalido()
        {
            _focalizacionController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _focalizacionController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:Backbone");
            _focalizacionController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });

            _focalizacionController.Request.Headers.Add("piip-idAccion", "a2cffeb0-42a6-PRUEBA-ad4b-d674d65c40b1");
            _focalizacionController.Request.Headers.Add("piip-idInstanciaFlujo", "");
            await _focalizacionController.Temporal(new FocalizacionProyectoDto());

        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException), "Parámetro piip-idAccion no recibido.")]
        public async Task PFocalizacionProyectoGuardarTemporalConidAccionVacio__RetornaMensajeNoRecibido()
        {
            _focalizacionController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _focalizacionController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:Backbone");
            _focalizacionController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });

            _focalizacionController.Request.Headers.Add("piip-idInstanciaFlujo", Guid.NewGuid().ToString());
            await _focalizacionController.Temporal(new FocalizacionProyectoDto());

        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException), "Parámetro de entrada piip-idInstanciaFlujo inválido.")]
        public async Task PFocalizacionProyectoGuardarTemporalConidInstanciaFlujoInvalido__RetornaMensajeInvalido()
        {
            _focalizacionController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _focalizacionController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:Backbone");
            _focalizacionController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });

            _focalizacionController.Request.Headers.Add("piip-idAccion", "");
            _focalizacionController.Request.Headers.Add("piip-idInstanciaFlujo", "a2cffeb0-42a6-PRUEBA-ad4b-d674d65c40b1");
            await _focalizacionController.Temporal(new FocalizacionProyectoDto());

        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException), "Parámetro piip-idInstanciaFlujo no recibido.")]
        public async Task PFocalizacionProyectoGuardarTemporalConidInstanciaFlujoVacio__RetornaMensajeNoRecibido()
        {
            _focalizacionController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _focalizacionController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:Backbone");
            _focalizacionController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });

            _focalizacionController.Request.Headers.Add("piip-idAccion", Guid.NewGuid().ToString());
            await _focalizacionController.Temporal(new FocalizacionProyectoDto());
        }

        [TestMethod]
        public async Task PFocalizacionProyectoGuardarDefinitivo_OK()
        {
            _focalizacionController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _focalizacionController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:Backbone");
            _focalizacionController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });

            _focalizacionController.ControllerContext.Request.Headers.Add("piip-idAccion", "E3C1849C-FE24-4C07-9762-036FA72AF10C");
            _focalizacionController.ControllerContext.Request.Headers.Add("piip-idInstanciaFlujo", "4C2E62CD-CEAD-48EF-88C6-A50AB5913716");

            var focalizacionProyecto = new FocalizacionProyectoDto();
            var result = (OkNegotiatedContentResult<HttpResponseMessage>)await _focalizacionController.Definitivo(focalizacionProyecto);
            Assert.AreEqual(HttpStatusCode.OK, result.Content.StatusCode);
        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException), "Parámetro de entrada piip-idAccion inválido.")]
        public async Task PFocalizacionProyectoGuardarDefinitivoConidAccionInvalido__RetornaMensajeInvalido()
        {
            _focalizacionController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _focalizacionController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:Backbone");
            _focalizacionController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });

            _focalizacionController.Request.Headers.Add("piip-idAccion", "a2cffeb0-42a6-PRUEBA-ad4b-d674d65c40b1");
            _focalizacionController.Request.Headers.Add("piip-idInstanciaFlujo", "");
            await _focalizacionController.Temporal(new FocalizacionProyectoDto());

        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException), "Parámetro piip-idAccion no recibido.")]
        public async Task PFocalizacionProyectoGuardarDefinitivoConidAccionVacio__RetornaMensajeNoRecibido()
        {
            _focalizacionController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _focalizacionController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:Backbone");
            _focalizacionController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });

            _focalizacionController.Request.Headers.Add("piip-idInstanciaFlujo", Guid.NewGuid().ToString());
            await _focalizacionController.Temporal(new FocalizacionProyectoDto());

        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException), "Parámetro de entrada piip-idInstanciaFlujo inválido.")]
        public async Task PFocalizacionProyectoGuardarDefinitivoConidInstanciaFlujoInvalido__RetornaMensajeInvalido()
        {
            _focalizacionController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _focalizacionController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:Backbone");
            _focalizacionController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });

            _focalizacionController.Request.Headers.Add("piip-idAccion", "");
            _focalizacionController.Request.Headers.Add("piip-idInstanciaFlujo", "a2cffeb0-42a6-PRUEBA-ad4b-d674d65c40b1");
            await _focalizacionController.Temporal(new FocalizacionProyectoDto());

        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException), "Parámetro piip-idInstanciaFlujo no recibido.")]
        public async Task PFocalizacionProyectoGuardarDefinitivoConidInstanciaFlujoVacio__RetornaMensajeNoRecibido()
        {
            _focalizacionController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _focalizacionController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:Backbone");
            _focalizacionController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });

            _focalizacionController.Request.Headers.Add("piip-idAccion", Guid.NewGuid().ToString());
            await _focalizacionController.Temporal(new FocalizacionProyectoDto());
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                // dispose managed resources
                _focalizacionController.Dispose();
            }
            // free native resources
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
}
