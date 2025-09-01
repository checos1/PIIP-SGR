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
    using ServiciosNegocio.Dominio.Dto.Focalizacion;
    using Unity;

    public class FocalizacionCuantificacionLocalizacionControllerTest
    {
        private IFocalizacionCuantificacionLocalizacionServicios FocalizacionCuantificacionLocalizacionServicios { get; set; }
        private IAutorizacionUtilidades AutorizacionUtilizades { get; set; }
        private FocalizacionCuantificacionLocalizacionController _focalizacionCuantificacionLocalizacionController;
        private string Bpin { get; set; }



        [TestInitialize]
        public void Init()
        {
            Bpin = "2017761220016";
            var contenedor = Configuracion.UnityConfig.Container;
            FocalizacionCuantificacionLocalizacionServicios = contenedor.Resolve<IFocalizacionCuantificacionLocalizacionServicios>();
            AutorizacionUtilizades = contenedor.Resolve<IAutorizacionUtilidades>();
            _focalizacionCuantificacionLocalizacionController = new FocalizacionCuantificacionLocalizacionController(FocalizacionCuantificacionLocalizacionServicios, AutorizacionUtilizades);
            _focalizacionCuantificacionLocalizacionController.ControllerContext.Request = new HttpRequestMessage();
        }


        [TestMethod]
        public async Task FocalizacionCuantificacionLocalizacionBpinValido()
        {
            _focalizacionCuantificacionLocalizacionController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _focalizacionCuantificacionLocalizacionController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });
            _focalizacionCuantificacionLocalizacionController.ControllerContext.Request.Headers.Add("piip-idAccion", "E3C1849C-FE24-4C07-9762-036FA72AF10C");
            _focalizacionCuantificacionLocalizacionController.ControllerContext.Request.Headers.Add("piip-idInstanciaFlujo", "4C2E62CD-CEAD-48EF-88C6-A50AB5913716");

            var result = (OkNegotiatedContentResult<FocalizacionCuantificacionLocalizacionDto>)await _focalizacionCuantificacionLocalizacionController.Consultar(Bpin);
            Assert.IsTrue(result.Content.Focalizacion.Count > 0);
        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException))]
        public async Task FocalizacionCuantificacionLocalizacionsinBpin()
        {
            _focalizacionCuantificacionLocalizacionController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _focalizacionCuantificacionLocalizacionController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });
            _focalizacionCuantificacionLocalizacionController.ControllerContext.Request.Headers.Add("piip-idAccion", "E3C1849C-FE24-4C07-9762-036FA72AF10C");
            _focalizacionCuantificacionLocalizacionController.ControllerContext.Request.Headers.Add("piip-idInstanciaFlujo", "4C2E62CD-CEAD-48EF-88C6-A50AB5913716");
            await _focalizacionCuantificacionLocalizacionController.Consultar(string.Empty);
        }


        [TestMethod]
        [ExpectedException(typeof(HttpResponseException))]
        public async Task FocalizacionCuantificacionLocalizacionSinBpinTexto()
        {
            _focalizacionCuantificacionLocalizacionController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _focalizacionCuantificacionLocalizacionController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });
            _focalizacionCuantificacionLocalizacionController.ControllerContext.Request.Headers.Add("piip-idAccion", "E3C1849C-FE24-4C07-9762-036FA72AF10C");
            _focalizacionCuantificacionLocalizacionController.ControllerContext.Request.Headers.Add("piip-idInstanciaFlujo", "4C2E62CD-CEAD-48EF-88C6-A50AB5913716");

            await _focalizacionCuantificacionLocalizacionController.Consultar("Hola");
        }


        [TestMethod]
        [ExpectedException(typeof(HttpResponseException))]
        public async Task FocalizacionCuantificacionLocalizacionSinBpinErroneo()
        {
            _focalizacionCuantificacionLocalizacionController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _focalizacionCuantificacionLocalizacionController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });
            _focalizacionCuantificacionLocalizacionController.ControllerContext.Request.Headers.Add("piip-idAccion", "E3C1849C-FE24-4C07-9762-036FA72AF10C");
            _focalizacionCuantificacionLocalizacionController.ControllerContext.Request.Headers.Add("piip-idInstanciaFlujo", "4C2E62CD-CEAD-48EF-88C6-A50AB5913716");

            await _focalizacionCuantificacionLocalizacionController.Consultar("12345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901");
        }


        [TestMethod]
        public async Task FocalizacionCuantificacionLocalizacionConsultar_Ok()
        {
            _focalizacionCuantificacionLocalizacionController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _focalizacionCuantificacionLocalizacionController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:Backbone");
            _focalizacionCuantificacionLocalizacionController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });

            _focalizacionCuantificacionLocalizacionController.ControllerContext.Request.Headers.Add("piip-idAccion", "E3C1849C-FE24-4C07-9762-036FA72AF10C");
            _focalizacionCuantificacionLocalizacionController.ControllerContext.Request.Headers.Add("piip-idInstanciaFlujo", "4C2E62CD-CEAD-48EF-88C6-A50AB5913716");
            var result = (OkNegotiatedContentResult<FocalizacionCuantificacionLocalizacionDto>)await _focalizacionCuantificacionLocalizacionController.Consultar(Bpin);
            Assert.IsNotNull(result.Content);
        }

        [TestMethod]
        public async Task FocalizacionCuantificacionLocalizacionConsultar_Failed()
        {
            _focalizacionCuantificacionLocalizacionController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _focalizacionCuantificacionLocalizacionController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:Backbone");
            _focalizacionCuantificacionLocalizacionController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });

            _focalizacionCuantificacionLocalizacionController.ControllerContext.Request.Headers.Add("piip-idAccion", "E3C1849C-FE24-4C07-9762-036FA72AF10C");
            _focalizacionCuantificacionLocalizacionController.ControllerContext.Request.Headers.Add("piip-idInstanciaFlujo", "4C2E62CD-CEAD-48EF-88C6-A50AB5913716");
            var result = (ResponseMessageResult)await _focalizacionCuantificacionLocalizacionController.Consultar(Bpin + "1");
            Assert.AreEqual(HttpStatusCode.NotFound, result.Response.StatusCode);
        }



        [TestMethod]
        public async Task FocalizacionCuantificacionLocalizacionPreview()
        {
            _focalizacionCuantificacionLocalizacionController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _focalizacionCuantificacionLocalizacionController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:Backbone");
            _focalizacionCuantificacionLocalizacionController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });

            var result = (OkNegotiatedContentResult<FocalizacionCuantificacionLocalizacionDto>)await _focalizacionCuantificacionLocalizacionController.Preview();
            Assert.IsNotNull(result.Content);
        }



        [TestMethod]
        public async Task FOcalizacionCuantificacionLocalizacionGuardarTemporal_OK()
        {
            _focalizacionCuantificacionLocalizacionController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _focalizacionCuantificacionLocalizacionController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:Backbone");
            _focalizacionCuantificacionLocalizacionController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });

            _focalizacionCuantificacionLocalizacionController.ControllerContext.Request.Headers.Add("piip-idAccion", "E3C1849C-FE24-4C07-9762-036FA72AF10C");
            _focalizacionCuantificacionLocalizacionController.ControllerContext.Request.Headers.Add("piip-idInstanciaFlujo", "4C2E62CD-CEAD-48EF-88C6-A50AB5913716");

            var focalizacionCuantificacionLocalizacion = new FocalizacionCuantificacionLocalizacionDto();
            var result = (OkNegotiatedContentResult<HttpResponseMessage>)await _focalizacionCuantificacionLocalizacionController.Temporal(focalizacionCuantificacionLocalizacion);
            Assert.AreEqual(HttpStatusCode.OK, result.Content.StatusCode);
        }


        [TestMethod]
        [ExpectedException(typeof(HttpResponseException), "Parámetro de entrada piip-idAccion inválido.")]
        public async Task FocalizacionCuantificacionLocalizacionGuardarTemporalConidAccionInvalido__RetornaMensajeInvalido()
        {
            _focalizacionCuantificacionLocalizacionController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _focalizacionCuantificacionLocalizacionController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:Backbone");
            _focalizacionCuantificacionLocalizacionController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });

            _focalizacionCuantificacionLocalizacionController.Request.Headers.Add("piip-idAccion", "a2cffeb0-42a6-PRUEBA-ad4b-d674d65c40b1");
            _focalizacionCuantificacionLocalizacionController.Request.Headers.Add("piip-idInstanciaFlujo", "");
            await _focalizacionCuantificacionLocalizacionController.Temporal(new FocalizacionCuantificacionLocalizacionDto());

        }


        [TestMethod]
        [ExpectedException(typeof(HttpResponseException), "Parámetro piip-idAccion no recibido.")]
        public async Task FocalizacionCuantificacionLocalizacionGuardarTemporalConidAccionVacio__RetornaMensajeNoRecibido()
        {
            _focalizacionCuantificacionLocalizacionController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _focalizacionCuantificacionLocalizacionController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:Backbone");
            _focalizacionCuantificacionLocalizacionController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });

            _focalizacionCuantificacionLocalizacionController.Request.Headers.Add("piip-idInstanciaFlujo", Guid.NewGuid().ToString());
            await _focalizacionCuantificacionLocalizacionController.Temporal(new FocalizacionCuantificacionLocalizacionDto());

        }


        [TestMethod]
        [ExpectedException(typeof(HttpResponseException), "Parámetro de entrada piip-idInstanciaFlujo inválido.")]
        public async Task FocalizacionCuantificacionLocalizacionGuardarTemporalConidInstanciaFlujoInvalido__RetornaMensajeInvalido()
        {
            _focalizacionCuantificacionLocalizacionController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _focalizacionCuantificacionLocalizacionController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:Backbone");
            _focalizacionCuantificacionLocalizacionController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });

            _focalizacionCuantificacionLocalizacionController.Request.Headers.Add("piip-idAccion", "");
            _focalizacionCuantificacionLocalizacionController.Request.Headers.Add("piip-idInstanciaFlujo", "a2cffeb0-42a6-PRUEBA-ad4b-d674d65c40b1");
            await _focalizacionCuantificacionLocalizacionController.Temporal(new FocalizacionCuantificacionLocalizacionDto());

        }


        [TestMethod]
        [ExpectedException(typeof(HttpResponseException), "Parámetro piip-idInstanciaFlujo no recibido.")]
        public async Task FocalizacionCuantificacionLocalizacionGuardarTemporalConidInstanciaFlujoVacio__RetornaMensajeNoRecibido()
        {
            _focalizacionCuantificacionLocalizacionController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _focalizacionCuantificacionLocalizacionController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:Backbone");
            _focalizacionCuantificacionLocalizacionController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });

            _focalizacionCuantificacionLocalizacionController.Request.Headers.Add("piip-idAccion", Guid.NewGuid().ToString());
            await _focalizacionCuantificacionLocalizacionController.Temporal(new FocalizacionCuantificacionLocalizacionDto());
        }



        [TestMethod]
        public async Task FocalizacionCuantificacionLocalizacionGuardarDefinitivo_OK()
        {
            _focalizacionCuantificacionLocalizacionController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _focalizacionCuantificacionLocalizacionController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:Backbone");
            _focalizacionCuantificacionLocalizacionController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });

            _focalizacionCuantificacionLocalizacionController.ControllerContext.Request.Headers.Add("piip-idAccion", "E3C1849C-FE24-4C07-9762-036FA72AF10C");
            _focalizacionCuantificacionLocalizacionController.ControllerContext.Request.Headers.Add("piip-idInstanciaFlujo", "4C2E62CD-CEAD-48EF-88C6-A50AB5913716");

            var focalizacionCuantificacionLocalizacion = new FocalizacionCuantificacionLocalizacionDto();
            var result = (OkNegotiatedContentResult<HttpResponseMessage>)await _focalizacionCuantificacionLocalizacionController.Definitivo(focalizacionCuantificacionLocalizacion);
            Assert.AreEqual(HttpStatusCode.OK, result.Content.StatusCode);
        }


        [TestMethod]
        [ExpectedException(typeof(HttpResponseException), "Parámetro de entrada piip-idAccion inválido.")]
        public async Task FocalizacionCuantificacionLocalizacionGuardarDefinitivoConidAccionInvalido__RetornaMensajeInvalido()
        {
            _focalizacionCuantificacionLocalizacionController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _focalizacionCuantificacionLocalizacionController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:Backbone");
            _focalizacionCuantificacionLocalizacionController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });

            _focalizacionCuantificacionLocalizacionController.Request.Headers.Add("piip-idAccion", "a2cffeb0-42a6-PRUEBA-ad4b-d674d65c40b1");
            _focalizacionCuantificacionLocalizacionController.Request.Headers.Add("piip-idInstanciaFlujo", "");
            await _focalizacionCuantificacionLocalizacionController.Temporal(new FocalizacionCuantificacionLocalizacionDto());

        }


        [TestMethod]
        [ExpectedException(typeof(HttpResponseException), "Parámetro piip-idAccion no recibido.")]
        public async Task FocalizacionCuantificacionLocalizacionGuardarDefinitivoConidAccionVacio__RetornaMensajeNoRecibido()
        {
            _focalizacionCuantificacionLocalizacionController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _focalizacionCuantificacionLocalizacionController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:Backbone");
            _focalizacionCuantificacionLocalizacionController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });

            _focalizacionCuantificacionLocalizacionController.Request.Headers.Add("piip-idInstanciaFlujo", Guid.NewGuid().ToString());
            await _focalizacionCuantificacionLocalizacionController.Temporal(new FocalizacionCuantificacionLocalizacionDto());

        }


        [TestMethod]
        [ExpectedException(typeof(HttpResponseException), "Parámetro de entrada piip-idInstanciaFlujo inválido.")]
        public async Task FocalizacionCuantificacionLocalizacionGuardarDefinitivoConidInstanciaFlujoInvalido__RetornaMensajeInvalido()
        {
            _focalizacionCuantificacionLocalizacionController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _focalizacionCuantificacionLocalizacionController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:Backbone");
            _focalizacionCuantificacionLocalizacionController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });

            _focalizacionCuantificacionLocalizacionController.Request.Headers.Add("piip-idAccion", "");
            _focalizacionCuantificacionLocalizacionController.Request.Headers.Add("piip-idInstanciaFlujo", "a2cffeb0-42a6-PRUEBA-ad4b-d674d65c40b1");
            await _focalizacionCuantificacionLocalizacionController.Temporal(new FocalizacionCuantificacionLocalizacionDto());

        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException), "Parámetro piip-idInstanciaFlujo no recibido.")]
        public async Task FocalizacionCuantificacionLocalizacionGuardarDefinitivoConidInstanciaFlujoVacio__RetornaMensajeNoRecibido()
        {
            _focalizacionCuantificacionLocalizacionController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _focalizacionCuantificacionLocalizacionController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:Backbone");
            _focalizacionCuantificacionLocalizacionController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });

            _focalizacionCuantificacionLocalizacionController.Request.Headers.Add("piip-idAccion", Guid.NewGuid().ToString());
            await _focalizacionCuantificacionLocalizacionController.Temporal(new FocalizacionCuantificacionLocalizacionDto());
        }


        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                // dispose managed resources
                _focalizacionCuantificacionLocalizacionController.Dispose();
            }
            // free native resources
        }

        public void Dispose()
        {
            Dispose(true);
        }

    }
}
