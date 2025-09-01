
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
    using ServiciosNegocio.Dominio.Dto.Poblacion;
    using Unity;

    [TestClass]
    public class CuantificacionLocalizacioControllerTest
    {

        private ICuantificacionLocalizacionServicios CuantificacionLocalizacionServicios { get; set; }
        private IAutorizacionUtilidades AutorizacionUtilizades { get; set; }
        private CuantificacionLocalizacionController _cuantificacionLocalizacionController;
        private string Bpin { get; set; }


        [TestInitialize]
        public void Init()
        {
            Bpin = "2017761220016";
            var contenedor = Configuracion.UnityConfig.Container;
            CuantificacionLocalizacionServicios = contenedor.Resolve<ICuantificacionLocalizacionServicios>();
            AutorizacionUtilizades = contenedor.Resolve<IAutorizacionUtilidades>();
            _cuantificacionLocalizacionController = new CuantificacionLocalizacionController(CuantificacionLocalizacionServicios, AutorizacionUtilizades);
            _cuantificacionLocalizacionController.ControllerContext.Request = new HttpRequestMessage();
        }

        [TestMethod]
        public async Task CuantificacionLocalizacionBpinValido()
        {
            _cuantificacionLocalizacionController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _cuantificacionLocalizacionController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });
            _cuantificacionLocalizacionController.ControllerContext.Request.Headers.Add("piip-idAccion", "E3C1849C-FE24-4C07-9762-036FA72AF10C");
            _cuantificacionLocalizacionController.ControllerContext.Request.Headers.Add("piip-idInstanciaFlujo", "4C2E62CD-CEAD-48EF-88C6-A50AB5913716");

            var result = (OkNegotiatedContentResult<PoblacionDto>)await _cuantificacionLocalizacionController.Consultar(Bpin);
            Assert.IsTrue(result.Content.Vigencias.Count > 0);
        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException))]
        public async Task CuantificacionLocalizacionsinBpin()
        {
            _cuantificacionLocalizacionController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _cuantificacionLocalizacionController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });
            _cuantificacionLocalizacionController.ControllerContext.Request.Headers.Add("piip-idAccion", "E3C1849C-FE24-4C07-9762-036FA72AF10C");
            _cuantificacionLocalizacionController.ControllerContext.Request.Headers.Add("piip-idInstanciaFlujo", "4C2E62CD-CEAD-48EF-88C6-A50AB5913716");
            await _cuantificacionLocalizacionController.Consultar(string.Empty);
        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException))]
        public async Task CuantificacionLocalizacionSinBpinTexto()
        {
            _cuantificacionLocalizacionController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _cuantificacionLocalizacionController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });
            _cuantificacionLocalizacionController.ControllerContext.Request.Headers.Add("piip-idAccion", "E3C1849C-FE24-4C07-9762-036FA72AF10C");
            _cuantificacionLocalizacionController.ControllerContext.Request.Headers.Add("piip-idInstanciaFlujo", "4C2E62CD-CEAD-48EF-88C6-A50AB5913716");

            await _cuantificacionLocalizacionController.Consultar("Hola");
        }


        [TestMethod]
        [ExpectedException(typeof(HttpResponseException))]
        public async Task CuantificacionLocalizacionSinBpinErroneo()
        {
            _cuantificacionLocalizacionController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _cuantificacionLocalizacionController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });
            _cuantificacionLocalizacionController.ControllerContext.Request.Headers.Add("piip-idAccion", "E3C1849C-FE24-4C07-9762-036FA72AF10C");
            _cuantificacionLocalizacionController.ControllerContext.Request.Headers.Add("piip-idInstanciaFlujo", "4C2E62CD-CEAD-48EF-88C6-A50AB5913716");

            await _cuantificacionLocalizacionController.Consultar("12345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901");
        }


        [TestMethod]
        public async Task CuantificacionLocalizacionConsultar_Ok()
        {
            _cuantificacionLocalizacionController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _cuantificacionLocalizacionController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:Backbone");
            _cuantificacionLocalizacionController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });

            _cuantificacionLocalizacionController.ControllerContext.Request.Headers.Add("piip-idAccion", "E3C1849C-FE24-4C07-9762-036FA72AF10C");
            _cuantificacionLocalizacionController.ControllerContext.Request.Headers.Add("piip-idInstanciaFlujo", "4C2E62CD-CEAD-48EF-88C6-A50AB5913716");
            var result = (OkNegotiatedContentResult<PoblacionDto>)await _cuantificacionLocalizacionController.Consultar(Bpin);
            Assert.IsNotNull(result.Content);
        }


        [TestMethod]
        public async Task CuantificacionLocalizacionConsultar_Failed()
        {
            _cuantificacionLocalizacionController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _cuantificacionLocalizacionController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:Backbone");
            _cuantificacionLocalizacionController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });

            _cuantificacionLocalizacionController.ControllerContext.Request.Headers.Add("piip-idAccion", "E3C1849C-FE24-4C07-9762-036FA72AF10C");
            _cuantificacionLocalizacionController.ControllerContext.Request.Headers.Add("piip-idInstanciaFlujo", "4C2E62CD-CEAD-48EF-88C6-A50AB5913716");
            var result = (ResponseMessageResult)await _cuantificacionLocalizacionController.Consultar(Bpin + "1");
            Assert.AreEqual(HttpStatusCode.NotFound, result.Response.StatusCode);
        }



        [TestMethod]
        public async Task CuantificacionLocalizacionPreview()
        {
            _cuantificacionLocalizacionController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _cuantificacionLocalizacionController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:Backbone");
            _cuantificacionLocalizacionController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });

            var result = (OkNegotiatedContentResult<PoblacionDto>)await _cuantificacionLocalizacionController.Preview();
            Assert.IsNotNull(result.Content);
        }


        [TestMethod]
        public async Task CuantificacionLocalizacionGuardarTemporal_OK()
        {
            _cuantificacionLocalizacionController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _cuantificacionLocalizacionController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:Backbone");
            _cuantificacionLocalizacionController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });

            _cuantificacionLocalizacionController.ControllerContext.Request.Headers.Add("piip-idAccion", "E3C1849C-FE24-4C07-9762-036FA72AF10C");
            _cuantificacionLocalizacionController.ControllerContext.Request.Headers.Add("piip-idInstanciaFlujo", "4C2E62CD-CEAD-48EF-88C6-A50AB5913716");

            var cuantificacionLocalizacion = new PoblacionDto();
            var result = (OkNegotiatedContentResult<HttpResponseMessage>)await _cuantificacionLocalizacionController.Temporal(cuantificacionLocalizacion);
            Assert.AreEqual(HttpStatusCode.OK, result.Content.StatusCode);
        }


        [TestMethod]
        [ExpectedException(typeof(HttpResponseException), "Parámetro de entrada piip-idAccion inválido.")]
        public async Task CuantificacionLocalizacionGuardarTemporalConidAccionInvalido__RetornaMensajeInvalido()
        {
            _cuantificacionLocalizacionController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _cuantificacionLocalizacionController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:Backbone");
            _cuantificacionLocalizacionController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });

            _cuantificacionLocalizacionController.Request.Headers.Add("piip-idAccion", "a2cffeb0-42a6-PRUEBA-ad4b-d674d65c40b1");
            _cuantificacionLocalizacionController.Request.Headers.Add("piip-idInstanciaFlujo", "");
            await _cuantificacionLocalizacionController.Temporal(new PoblacionDto());

        }


        [TestMethod]
        [ExpectedException(typeof(HttpResponseException), "Parámetro piip-idAccion no recibido.")]
        public async Task CuantificacionLocalizacionGuardarTemporalConidAccionVacio__RetornaMensajeNoRecibido()
        {
            _cuantificacionLocalizacionController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _cuantificacionLocalizacionController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:Backbone");
            _cuantificacionLocalizacionController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });

            _cuantificacionLocalizacionController.Request.Headers.Add("piip-idInstanciaFlujo", Guid.NewGuid().ToString());
            await _cuantificacionLocalizacionController.Temporal(new PoblacionDto());

        }


        [TestMethod]
        [ExpectedException(typeof(HttpResponseException), "Parámetro de entrada piip-idInstanciaFlujo inválido.")]
        public async Task CuantificacionLocalizacionGuardarTemporalConidInstanciaFlujoInvalido__RetornaMensajeInvalido()
        {
            _cuantificacionLocalizacionController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _cuantificacionLocalizacionController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:Backbone");
            _cuantificacionLocalizacionController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });

            _cuantificacionLocalizacionController.Request.Headers.Add("piip-idAccion", "");
            _cuantificacionLocalizacionController.Request.Headers.Add("piip-idInstanciaFlujo", "a2cffeb0-42a6-PRUEBA-ad4b-d674d65c40b1");
            await _cuantificacionLocalizacionController.Temporal(new PoblacionDto());

        }


        [TestMethod]
        [ExpectedException(typeof(HttpResponseException), "Parámetro piip-idInstanciaFlujo no recibido.")]
        public async Task CuantificacionLocalizacionGuardarTemporalConidInstanciaFlujoVacio__RetornaMensajeNoRecibido()
        {
            _cuantificacionLocalizacionController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _cuantificacionLocalizacionController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:Backbone");
            _cuantificacionLocalizacionController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });

            _cuantificacionLocalizacionController.Request.Headers.Add("piip-idAccion", Guid.NewGuid().ToString());
            await _cuantificacionLocalizacionController.Temporal(new PoblacionDto());
        }


        [TestMethod]
        public async Task CuantificacionLocalizacionGuardarDefinitivo_OK()
        {
            _cuantificacionLocalizacionController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _cuantificacionLocalizacionController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:Backbone");
            _cuantificacionLocalizacionController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });

            _cuantificacionLocalizacionController.ControllerContext.Request.Headers.Add("piip-idAccion", "E3C1849C-FE24-4C07-9762-036FA72AF10C");
            _cuantificacionLocalizacionController.ControllerContext.Request.Headers.Add("piip-idInstanciaFlujo", "4C2E62CD-CEAD-48EF-88C6-A50AB5913716");

            var cuantificacionLocalizacion = new PoblacionDto();
            var result = (OkNegotiatedContentResult<HttpResponseMessage>)await _cuantificacionLocalizacionController.Definitivo(cuantificacionLocalizacion);
            Assert.AreEqual(HttpStatusCode.OK, result.Content.StatusCode);
        }


        [TestMethod]
        [ExpectedException(typeof(HttpResponseException), "Parámetro de entrada piip-idAccion inválido.")]
        public async Task CuantificacionLocalizacionGuardarDefinitivoConidAccionInvalido__RetornaMensajeInvalido()
        {
            _cuantificacionLocalizacionController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _cuantificacionLocalizacionController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:Backbone");
            _cuantificacionLocalizacionController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });

            _cuantificacionLocalizacionController.Request.Headers.Add("piip-idAccion", "a2cffeb0-42a6-PRUEBA-ad4b-d674d65c40b1");
            _cuantificacionLocalizacionController.Request.Headers.Add("piip-idInstanciaFlujo", "");
            await _cuantificacionLocalizacionController.Temporal(new PoblacionDto());

        }


        [TestMethod]
        [ExpectedException(typeof(HttpResponseException), "Parámetro piip-idAccion no recibido.")]
        public async Task CuantificacionLocalizacionGuardarDefinitivoConidAccionVacio__RetornaMensajeNoRecibido()
        {
            _cuantificacionLocalizacionController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _cuantificacionLocalizacionController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:Backbone");
            _cuantificacionLocalizacionController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });

            _cuantificacionLocalizacionController.Request.Headers.Add("piip-idInstanciaFlujo", Guid.NewGuid().ToString());
            await _cuantificacionLocalizacionController.Temporal(new PoblacionDto());

        }


        [TestMethod]
        [ExpectedException(typeof(HttpResponseException), "Parámetro de entrada piip-idInstanciaFlujo inválido.")]
        public async Task CuantificacionLocalizacionGuardarDefinitivoConidInstanciaFlujoInvalido__RetornaMensajeInvalido()
        {
            _cuantificacionLocalizacionController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _cuantificacionLocalizacionController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:Backbone");
            _cuantificacionLocalizacionController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });

            _cuantificacionLocalizacionController.Request.Headers.Add("piip-idAccion", "");
            _cuantificacionLocalizacionController.Request.Headers.Add("piip-idInstanciaFlujo", "a2cffeb0-42a6-PRUEBA-ad4b-d674d65c40b1");
            await _cuantificacionLocalizacionController.Temporal(new PoblacionDto());

        }

        
        [TestMethod]
        [ExpectedException(typeof(HttpResponseException), "Parámetro piip-idInstanciaFlujo no recibido.")]
        public async Task CuantificacionLocalizacionGuardarDefinitivoConidInstanciaFlujoVacio__RetornaMensajeNoRecibido()
        {
            _cuantificacionLocalizacionController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _cuantificacionLocalizacionController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:Backbone");
            _cuantificacionLocalizacionController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });

            _cuantificacionLocalizacionController.Request.Headers.Add("piip-idAccion", Guid.NewGuid().ToString());
            await _cuantificacionLocalizacionController.Temporal(new PoblacionDto());
        }



        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                // dispose managed resources
                _cuantificacionLocalizacionController.Dispose();
            }
            // free native resources
        }

        public void Dispose()
        {
            Dispose(true);
        }



    }
}
