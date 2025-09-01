
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
    using ServiciosNegocio.Dominio.Dto.IndicadorProductoAgregar;
    using Unity;


    [TestClass]
    public class IndicadorProductoAgregarControllerTest
    {

        private IIndicadorProductoAgregarServicios IndicadorProductoAgregarServicios { get; set; }
        private IAutorizacionUtilidades AutorizacionUtilizades { get; set; }
        private IndicadorProductoAgregarController _indicadorProductoAgregarController;
        private string Bpin { get; set; }


        [TestInitialize]
        public void Init()
        {
            Bpin = "2017761220016";
            var contenedor = Configuracion.UnityConfig.Container;
            IndicadorProductoAgregarServicios = contenedor.Resolve<IIndicadorProductoAgregarServicios>();
            AutorizacionUtilizades = contenedor.Resolve<IAutorizacionUtilidades>();
            _indicadorProductoAgregarController = new IndicadorProductoAgregarController(IndicadorProductoAgregarServicios, AutorizacionUtilizades);
            _indicadorProductoAgregarController.ControllerContext.Request = new HttpRequestMessage();
        }

        [TestMethod]
        public async Task IndicadorProductoAgregarBpinValido()
        {
            _indicadorProductoAgregarController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _indicadorProductoAgregarController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });
            _indicadorProductoAgregarController.ControllerContext.Request.Headers.Add("piip-idAccion", "E3C1849C-FE24-4C07-9762-036FA72AF10C");
            _indicadorProductoAgregarController.ControllerContext.Request.Headers.Add("piip-idInstanciaFlujo", "4C2E62CD-CEAD-48EF-88C6-A50AB5913716");

            var result = (OkNegotiatedContentResult<IndicadorProductoAgregarDto>)await _indicadorProductoAgregarController.Consultar(Bpin);
            Assert.IsTrue(result.Content.Objetivos.Count > 0);
        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException))]
        public async Task IndicadorProductoAgregarsinBpin()
        {
            _indicadorProductoAgregarController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _indicadorProductoAgregarController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });
            _indicadorProductoAgregarController.ControllerContext.Request.Headers.Add("piip-idAccion", "E3C1849C-FE24-4C07-9762-036FA72AF10C");
            _indicadorProductoAgregarController.ControllerContext.Request.Headers.Add("piip-idInstanciaFlujo", "4C2E62CD-CEAD-48EF-88C6-A50AB5913716");
            await _indicadorProductoAgregarController.Consultar(string.Empty);
        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException))]
        public async Task IndicadorProductoAgregarSinBpinTexto()
        {
            _indicadorProductoAgregarController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _indicadorProductoAgregarController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });
            _indicadorProductoAgregarController.ControllerContext.Request.Headers.Add("piip-idAccion", "E3C1849C-FE24-4C07-9762-036FA72AF10C");
            _indicadorProductoAgregarController.ControllerContext.Request.Headers.Add("piip-idInstanciaFlujo", "4C2E62CD-CEAD-48EF-88C6-A50AB5913716");

            await _indicadorProductoAgregarController.Consultar("Hola");
        }


        [TestMethod]
        [ExpectedException(typeof(HttpResponseException))]
        public async Task IndicadorProductoAgregarSinBpinErroneo()
        {
            _indicadorProductoAgregarController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _indicadorProductoAgregarController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });
            _indicadorProductoAgregarController.ControllerContext.Request.Headers.Add("piip-idAccion", "E3C1849C-FE24-4C07-9762-036FA72AF10C");
            _indicadorProductoAgregarController.ControllerContext.Request.Headers.Add("piip-idInstanciaFlujo", "4C2E62CD-CEAD-48EF-88C6-A50AB5913716");

            await _indicadorProductoAgregarController.Consultar("12345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901");
        }


        [TestMethod]
        public async Task IndicadorProductoAgregarConsultar_Ok()
        {
            _indicadorProductoAgregarController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _indicadorProductoAgregarController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:Backbone");
            _indicadorProductoAgregarController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });

            _indicadorProductoAgregarController.ControllerContext.Request.Headers.Add("piip-idAccion", "E3C1849C-FE24-4C07-9762-036FA72AF10C");
            _indicadorProductoAgregarController.ControllerContext.Request.Headers.Add("piip-idInstanciaFlujo", "4C2E62CD-CEAD-48EF-88C6-A50AB5913716");
            var result = (OkNegotiatedContentResult<IndicadorProductoAgregarDto>)await _indicadorProductoAgregarController.Consultar(Bpin);
            Assert.IsNotNull(result.Content);
        }


        [TestMethod]
        public async Task IndicadorProductoAgregarConsultar_Failed()
        {
            _indicadorProductoAgregarController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _indicadorProductoAgregarController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:Backbone");
            _indicadorProductoAgregarController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });

            _indicadorProductoAgregarController.ControllerContext.Request.Headers.Add("piip-idAccion", "E3C1849C-FE24-4C07-9762-036FA72AF10C");
            _indicadorProductoAgregarController.ControllerContext.Request.Headers.Add("piip-idInstanciaFlujo", "4C2E62CD-CEAD-48EF-88C6-A50AB5913716");
            var result = (ResponseMessageResult)await _indicadorProductoAgregarController.Consultar(Bpin + "1");
            Assert.AreEqual(HttpStatusCode.NotFound, result.Response.StatusCode);
        }



        [TestMethod]
        public async Task IndicadorProductoAgregarPreview()
        {
            _indicadorProductoAgregarController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _indicadorProductoAgregarController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:Backbone");
            _indicadorProductoAgregarController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });

            var result = (OkNegotiatedContentResult<IndicadorProductoAgregarDto>)await _indicadorProductoAgregarController.Preview();
            Assert.IsNotNull(result.Content);
        }


        [TestMethod]
        public async Task IndicadorProductoAgregarGuardarTemporal_OK()
        {
            _indicadorProductoAgregarController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _indicadorProductoAgregarController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:Backbone");
            _indicadorProductoAgregarController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });

            _indicadorProductoAgregarController.ControllerContext.Request.Headers.Add("piip-idAccion", "E3C1849C-FE24-4C07-9762-036FA72AF10C");
            _indicadorProductoAgregarController.ControllerContext.Request.Headers.Add("piip-idInstanciaFlujo", "4C2E62CD-CEAD-48EF-88C6-A50AB5913716");

            var indicadorProductoAgregar = new IndicadorProductoAgregarDto();
            var result = (OkNegotiatedContentResult<HttpResponseMessage>)await _indicadorProductoAgregarController.Temporal(indicadorProductoAgregar);
            Assert.AreEqual(HttpStatusCode.OK, result.Content.StatusCode);
        }


        [TestMethod]
        [ExpectedException(typeof(HttpResponseException), "Parámetro de entrada piip-idAccion inválido.")]
        public async Task IndicadorProductoAgregarGuardarTemporalConidAccionInvalido__RetornaMensajeInvalido()
        {
            _indicadorProductoAgregarController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _indicadorProductoAgregarController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:Backbone");
            _indicadorProductoAgregarController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });

            _indicadorProductoAgregarController.Request.Headers.Add("piip-idAccion", "a2cffeb0-42a6-PRUEBA-ad4b-d674d65c40b1");
            _indicadorProductoAgregarController.Request.Headers.Add("piip-idInstanciaFlujo", "");
            await _indicadorProductoAgregarController.Temporal(new IndicadorProductoAgregarDto());

        }


        [TestMethod]
        [ExpectedException(typeof(HttpResponseException), "Parámetro piip-idAccion no recibido.")]
        public async Task IndicadorProductoAgregarGuardarTemporalConidAccionVacio__RetornaMensajeNoRecibido()
        {
            _indicadorProductoAgregarController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _indicadorProductoAgregarController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:Backbone");
            _indicadorProductoAgregarController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });

            _indicadorProductoAgregarController.Request.Headers.Add("piip-idInstanciaFlujo", Guid.NewGuid().ToString());
            await _indicadorProductoAgregarController.Temporal(new IndicadorProductoAgregarDto());

        }


        [TestMethod]
        [ExpectedException(typeof(HttpResponseException), "Parámetro de entrada piip-idInstanciaFlujo inválido.")]
        public async Task IndicadorProductoAgregarGuardarTemporalConidInstanciaFlujoInvalido__RetornaMensajeInvalido()
        {
            _indicadorProductoAgregarController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _indicadorProductoAgregarController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:Backbone");
            _indicadorProductoAgregarController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });

            _indicadorProductoAgregarController.Request.Headers.Add("piip-idAccion", "");
            _indicadorProductoAgregarController.Request.Headers.Add("piip-idInstanciaFlujo", "a2cffeb0-42a6-PRUEBA-ad4b-d674d65c40b1");
            await _indicadorProductoAgregarController.Temporal(new IndicadorProductoAgregarDto());

        }


        [TestMethod]
        [ExpectedException(typeof(HttpResponseException), "Parámetro piip-idInstanciaFlujo no recibido.")]
        public async Task IndicadorProductoAgregarGuardarTemporalConidInstanciaFlujoVacio__RetornaMensajeNoRecibido()
        {
            _indicadorProductoAgregarController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _indicadorProductoAgregarController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:Backbone");
            _indicadorProductoAgregarController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });

            _indicadorProductoAgregarController.Request.Headers.Add("piip-idAccion", Guid.NewGuid().ToString());
            await _indicadorProductoAgregarController.Temporal(new IndicadorProductoAgregarDto());
        }


        [TestMethod]
        public async Task IndicadorProductoAgregarGuardarDefinitivo_OK()
        {
            _indicadorProductoAgregarController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _indicadorProductoAgregarController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:Backbone");
            _indicadorProductoAgregarController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });

            _indicadorProductoAgregarController.ControllerContext.Request.Headers.Add("piip-idAccion", "E3C1849C-FE24-4C07-9762-036FA72AF10C");
            _indicadorProductoAgregarController.ControllerContext.Request.Headers.Add("piip-idInstanciaFlujo", "4C2E62CD-CEAD-48EF-88C6-A50AB5913716");

            var indicadorProductoAgregar = new IndicadorProductoAgregarDto();
            var result = (OkNegotiatedContentResult<HttpResponseMessage>)await _indicadorProductoAgregarController.Definitivo(indicadorProductoAgregar);
            Assert.AreEqual(HttpStatusCode.OK, result.Content.StatusCode);
        }


        [TestMethod]
        [ExpectedException(typeof(HttpResponseException), "Parámetro de entrada piip-idAccion inválido.")]
        public async Task IndicadorProductoAgregarGuardarDefinitivoConidAccionInvalido__RetornaMensajeInvalido()
        {
            _indicadorProductoAgregarController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _indicadorProductoAgregarController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:Backbone");
            _indicadorProductoAgregarController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });

            _indicadorProductoAgregarController.Request.Headers.Add("piip-idAccion", "a2cffeb0-42a6-PRUEBA-ad4b-d674d65c40b1");
            _indicadorProductoAgregarController.Request.Headers.Add("piip-idInstanciaFlujo", "");
            await _indicadorProductoAgregarController.Temporal(new IndicadorProductoAgregarDto());

        }


        [TestMethod]
        [ExpectedException(typeof(HttpResponseException), "Parámetro piip-idAccion no recibido.")]
        public async Task IndicadorProductoAgregarGuardarDefinitivoConidAccionVacio__RetornaMensajeNoRecibido()
        {
            _indicadorProductoAgregarController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _indicadorProductoAgregarController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:Backbone");
            _indicadorProductoAgregarController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });

            _indicadorProductoAgregarController.Request.Headers.Add("piip-idInstanciaFlujo", Guid.NewGuid().ToString());
            await _indicadorProductoAgregarController.Temporal(new IndicadorProductoAgregarDto());

        }


        [TestMethod]
        [ExpectedException(typeof(HttpResponseException), "Parámetro de entrada piip-idInstanciaFlujo inválido.")]
        public async Task IndicadorProductoAgregarGuardarDefinitivoConidInstanciaFlujoInvalido__RetornaMensajeInvalido()
        {
            _indicadorProductoAgregarController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _indicadorProductoAgregarController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:Backbone");
            _indicadorProductoAgregarController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });

            _indicadorProductoAgregarController.Request.Headers.Add("piip-idAccion", "");
            _indicadorProductoAgregarController.Request.Headers.Add("piip-idInstanciaFlujo", "a2cffeb0-42a6-PRUEBA-ad4b-d674d65c40b1");
            await _indicadorProductoAgregarController.Temporal(new IndicadorProductoAgregarDto());

        }


        [TestMethod]
        [ExpectedException(typeof(HttpResponseException), "Parámetro piip-idInstanciaFlujo no recibido.")]
        public async Task IndicadorProductoAgregarGuardarDefinitivoConidInstanciaFlujoVacio__RetornaMensajeNoRecibido()
        {
            _indicadorProductoAgregarController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _indicadorProductoAgregarController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:Backbone");
            _indicadorProductoAgregarController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });

            _indicadorProductoAgregarController.Request.Headers.Add("piip-idAccion", Guid.NewGuid().ToString());
            await _indicadorProductoAgregarController.Temporal(new IndicadorProductoAgregarDto());
        }



        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                // dispose managed resources
                _indicadorProductoAgregarController.Dispose();
            }
            // free native resources
        }

        public void Dispose()
        {
            Dispose(true);
        }



    }
}
