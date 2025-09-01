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
    public sealed class RegionalizacionProyectoControllerTest : IDisposable
    {
        private IRegionalizacionProyectoServicios RegionalizacionServicios { get; set; }
        private IAutorizacionUtilidades AutorizacionUtilizades { get; set; }
        private RegionalizacionProyectoController _regionalizacionController;
        private string Bpin { get; set; }

        [TestInitialize]
        public void Init()
        {
            Bpin = "2017011000236";
            var contenedor = Configuracion.UnityConfig.Container;
            RegionalizacionServicios = contenedor.Resolve<IRegionalizacionProyectoServicios>();
            AutorizacionUtilizades = contenedor.Resolve<IAutorizacionUtilidades>();
            _regionalizacionController = new RegionalizacionProyectoController(RegionalizacionServicios, AutorizacionUtilizades);
            _regionalizacionController.ControllerContext.Request = new HttpRequestMessage();
        }

        [TestMethod]
        public async Task RegionalizacionBpinValido()
        {
            _regionalizacionController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _regionalizacionController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });
            _regionalizacionController.ControllerContext.Request.Headers.Add("piip-idAccion", "E3C1849C-FE24-4C07-9762-036FA72AF10C");
            _regionalizacionController.ControllerContext.Request.Headers.Add("piip-idInstanciaFlujo", "4C2E62CD-CEAD-48EF-88C6-A50AB5913716");

            var result = (OkNegotiatedContentResult<RegionalizacionProyectoDto>)await _regionalizacionController.Consultar(Bpin);
            Assert.IsTrue(result.Content.Vigencias.Count > 0);
        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException))]
        public async Task RegionalizacionsinBpin()
        {
            _regionalizacionController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _regionalizacionController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });
            _regionalizacionController.ControllerContext.Request.Headers.Add("piip-idAccion", "E3C1849C-FE24-4C07-9762-036FA72AF10C");
            _regionalizacionController.ControllerContext.Request.Headers.Add("piip-idInstanciaFlujo", "4C2E62CD-CEAD-48EF-88C6-A50AB5913716");
            await _regionalizacionController.Consultar(string.Empty);
        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException))]
        public async Task RegionalizacionProyectoSinBpinTexto()
        {
            _regionalizacionController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _regionalizacionController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });
            _regionalizacionController.ControllerContext.Request.Headers.Add("piip-idAccion", "E3C1849C-FE24-4C07-9762-036FA72AF10C");
            _regionalizacionController.ControllerContext.Request.Headers.Add("piip-idInstanciaFlujo", "4C2E62CD-CEAD-48EF-88C6-A50AB5913716");

            await _regionalizacionController.Consultar("Hola");
        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException))]
        public async Task RegionalizacionProyectoSinBpinErroneo()
        {
            _regionalizacionController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _regionalizacionController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });
            _regionalizacionController.ControllerContext.Request.Headers.Add("piip-idAccion", "E3C1849C-FE24-4C07-9762-036FA72AF10C");
            _regionalizacionController.ControllerContext.Request.Headers.Add("piip-idInstanciaFlujo", "4C2E62CD-CEAD-48EF-88C6-A50AB5913716");

            await _regionalizacionController.Consultar("12345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901");
        }


        [TestMethod]
        public async Task RegionalizacionProyectoPreview()
        {
            _regionalizacionController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _regionalizacionController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:Backbone");
            _regionalizacionController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });

            var result = (OkNegotiatedContentResult<RegionalizacionProyectoDto>)await _regionalizacionController.Preview();
            Assert.IsNotNull(result.Content);
        }

        [TestMethod]
        public async Task RegionalizacionProyectoConsultar_Ok()
        {
            _regionalizacionController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _regionalizacionController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:Backbone");
            _regionalizacionController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });

            _regionalizacionController.ControllerContext.Request.Headers.Add("piip-idAccion", "E3C1849C-FE24-4C07-9762-036FA72AF10C");
            _regionalizacionController.ControllerContext.Request.Headers.Add("piip-idInstanciaFlujo", "4C2E62CD-CEAD-48EF-88C6-A50AB5913716");
            var result = (OkNegotiatedContentResult<RegionalizacionProyectoDto>)await _regionalizacionController.Consultar(Bpin);
            Assert.IsNotNull(result.Content);
        }

        [TestMethod]
        public async Task RegionalizacionProyectoConsultar_Failed()
        {
            _regionalizacionController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _regionalizacionController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:Backbone");
            _regionalizacionController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });

            _regionalizacionController.ControllerContext.Request.Headers.Add("piip-idAccion", "E3C1849C-FE24-4C07-9762-036FA72AF10C");
            _regionalizacionController.ControllerContext.Request.Headers.Add("piip-idInstanciaFlujo", "4C2E62CD-CEAD-48EF-88C6-A50AB5913716");
            var result = (ResponseMessageResult)await _regionalizacionController.Consultar(Bpin + "1");
            Assert.AreEqual(HttpStatusCode.NotFound, result.Response.StatusCode);
        }

        [TestMethod]
        public async Task RegionalizacionProyectoGuardarTemporal_OK()
        {
            _regionalizacionController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _regionalizacionController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:Backbone");
            _regionalizacionController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });

            _regionalizacionController.ControllerContext.Request.Headers.Add("piip-idAccion", "E3C1849C-FE24-4C07-9762-036FA72AF10C");
            _regionalizacionController.ControllerContext.Request.Headers.Add("piip-idInstanciaFlujo", "4C2E62CD-CEAD-48EF-88C6-A50AB5913716");

            var regionalizacionProyecto = new RegionalizacionProyectoDto();
            var result = (OkNegotiatedContentResult<HttpResponseMessage>)await _regionalizacionController.Temporal(regionalizacionProyecto);
            Assert.AreEqual(HttpStatusCode.OK, result.Content.StatusCode);
        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException), "Parámetro de entrada piip-idAccion inválido.")]
        public async Task RegionalizacionProyectoGuardarTemporalConidAccionInvalido__RetornaMensajeInvalido()
        {
            _regionalizacionController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _regionalizacionController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:Backbone");
            _regionalizacionController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });

            _regionalizacionController.Request.Headers.Add("piip-idAccion", "a2cffeb0-42a6-PRUEBA-ad4b-d674d65c40b1");
            _regionalizacionController.Request.Headers.Add("piip-idInstanciaFlujo", "");
            await _regionalizacionController.Temporal(new RegionalizacionProyectoDto());

        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException), "Parámetro piip-idAccion no recibido.")]
        public async Task RegionalizacionProyectoGuardarTemporalConidAccionVacio__RetornaMensajeNoRecibido()
        {
            _regionalizacionController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _regionalizacionController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:Backbone");
            _regionalizacionController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });

            _regionalizacionController.Request.Headers.Add("piip-idInstanciaFlujo", Guid.NewGuid().ToString());
            await _regionalizacionController.Temporal(new RegionalizacionProyectoDto());

        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException), "Parámetro de entrada piip-idInstanciaFlujo inválido.")]
        public async Task RegionalizacionProyectoGuardarTemporalConidInstanciaFlujoInvalido__RetornaMensajeInvalido()
        {
            _regionalizacionController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _regionalizacionController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:Backbone");
            _regionalizacionController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });

            _regionalizacionController.Request.Headers.Add("piip-idAccion", "");
            _regionalizacionController.Request.Headers.Add("piip-idInstanciaFlujo", "a2cffeb0-42a6-PRUEBA-ad4b-d674d65c40b1");
            await _regionalizacionController.Temporal(new RegionalizacionProyectoDto());

        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException), "Parámetro piip-idInstanciaFlujo no recibido.")]
        public async Task RegionalizacionProyectoGuardarTemporalConidInstanciaFlujoVacio__RetornaMensajeNoRecibido()
        {
            _regionalizacionController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _regionalizacionController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:Backbone");
            _regionalizacionController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });

            _regionalizacionController.Request.Headers.Add("piip-idAccion", Guid.NewGuid().ToString());
            await _regionalizacionController.Temporal(new RegionalizacionProyectoDto());
        }

        [TestMethod]
        public async Task RegionalizacionProyectoGuardarDefinitivo_OK()
        {
            _regionalizacionController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _regionalizacionController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:Backbone");
            _regionalizacionController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });

            _regionalizacionController.ControllerContext.Request.Headers.Add("piip-idAccion", "E3C1849C-FE24-4C07-9762-036FA72AF10C");
            _regionalizacionController.ControllerContext.Request.Headers.Add("piip-idInstanciaFlujo", "4C2E62CD-CEAD-48EF-88C6-A50AB5913716");

            var regionalizacionProyecto = new RegionalizacionProyectoDto();
            var result = (OkNegotiatedContentResult<HttpResponseMessage>)await _regionalizacionController.Definitivo(regionalizacionProyecto);
            Assert.AreEqual(HttpStatusCode.OK, result.Content.StatusCode);
        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException), "Parámetro de entrada piip-idAccion inválido.")]
        public async Task RegionalizacionProyectoGuardarDefinitivoConidAccionInvalido__RetornaMensajeInvalido()
        {
            _regionalizacionController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _regionalizacionController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:Backbone");
            _regionalizacionController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });

            _regionalizacionController.Request.Headers.Add("piip-idAccion", "a2cffeb0-42a6-PRUEBA-ad4b-d674d65c40b1");
            _regionalizacionController.Request.Headers.Add("piip-idInstanciaFlujo", "");
            await _regionalizacionController.Temporal(new RegionalizacionProyectoDto());

        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException), "Parámetro piip-idAccion no recibido.")]
        public async Task RegionalizacionProyectoGuardarDefinitivoConidAccionVacio__RetornaMensajeNoRecibido()
        {
            _regionalizacionController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _regionalizacionController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:Backbone");
            _regionalizacionController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });

            _regionalizacionController.Request.Headers.Add("piip-idInstanciaFlujo", Guid.NewGuid().ToString());
            await _regionalizacionController.Temporal(new RegionalizacionProyectoDto());

        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException), "Parámetro de entrada piip-idInstanciaFlujo inválido.")]
        public async Task RegionalizacionProyectoGuardarDefinitivoConidInstanciaFlujoInvalido__RetornaMensajeInvalido()
        {
            _regionalizacionController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _regionalizacionController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:Backbone");
            _regionalizacionController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });

            _regionalizacionController.Request.Headers.Add("piip-idAccion", "");
            _regionalizacionController.Request.Headers.Add("piip-idInstanciaFlujo", "a2cffeb0-42a6-PRUEBA-ad4b-d674d65c40b1");
            await _regionalizacionController.Temporal(new RegionalizacionProyectoDto());

        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException), "Parámetro piip-idInstanciaFlujo no recibido.")]
        public async Task RegionalizacionProyectoGuardarDefinitivoConidInstanciaFlujoVacio__RetornaMensajeNoRecibido()
        {
            _regionalizacionController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _regionalizacionController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:Backbone");
            _regionalizacionController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });

            _regionalizacionController.Request.Headers.Add("piip-idAccion", Guid.NewGuid().ToString());
            await _regionalizacionController.Temporal(new RegionalizacionProyectoDto());
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                // dispose managed resources
                _regionalizacionController.Dispose();
            }
            // free native resources
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
}
