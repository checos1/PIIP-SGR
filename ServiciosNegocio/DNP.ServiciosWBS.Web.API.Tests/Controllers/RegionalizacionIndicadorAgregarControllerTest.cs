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
    public class RegionalizacionIndicadorAgregarControllerTest
    {

        private IRegionalizacionIndicadorAgregarServicios RegionalizacionIndicadorAgregarServicios { get; set; }
        private IAutorizacionUtilidades AutorizacionUtilizades { get; set; }
        private RegionalizacionIndicadorAgregarController _regionalizacionIndicadorAgregarController;
        private string Bpin { get; set; }


        [TestInitialize]
        public void Init()
        {
            Bpin = "2017761220016";
            var contenedor = Configuracion.UnityConfig.Container;
            RegionalizacionIndicadorAgregarServicios = contenedor.Resolve<IRegionalizacionIndicadorAgregarServicios>();
            AutorizacionUtilizades = contenedor.Resolve<IAutorizacionUtilidades>();
            _regionalizacionIndicadorAgregarController = new RegionalizacionIndicadorAgregarController(RegionalizacionIndicadorAgregarServicios, AutorizacionUtilizades);
            _regionalizacionIndicadorAgregarController.ControllerContext.Request = new HttpRequestMessage();
        }



        [TestMethod]
        public async Task RegionalizacionIndicadorAgregarBpinValido()
        {
            _regionalizacionIndicadorAgregarController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _regionalizacionIndicadorAgregarController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });
            _regionalizacionIndicadorAgregarController.ControllerContext.Request.Headers.Add("piip-idAccion", "E3C1849C-FE24-4C07-9762-036FA72AF10C");
            _regionalizacionIndicadorAgregarController.ControllerContext.Request.Headers.Add("piip-idInstanciaFlujo", "4C2E62CD-CEAD-48EF-88C6-A50AB5913716");

            var result = (OkNegotiatedContentResult<RegionalizacionIndicadorAgregarDto>)await _regionalizacionIndicadorAgregarController.Consultar(Bpin);
            Assert.IsTrue(result.Content.Objetivos.Count > 0);
        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException))]
        public async Task RegionalizacionIndicadorAgregarsinBpin()
        {
            _regionalizacionIndicadorAgregarController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _regionalizacionIndicadorAgregarController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });
            _regionalizacionIndicadorAgregarController.ControllerContext.Request.Headers.Add("piip-idAccion", "E3C1849C-FE24-4C07-9762-036FA72AF10C");
            _regionalizacionIndicadorAgregarController.ControllerContext.Request.Headers.Add("piip-idInstanciaFlujo", "4C2E62CD-CEAD-48EF-88C6-A50AB5913716");
            await _regionalizacionIndicadorAgregarController.Consultar(string.Empty);
        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException))]
        public async Task RegionalizacionIndicadorAgregarSinBpinTexto()
        {
            _regionalizacionIndicadorAgregarController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _regionalizacionIndicadorAgregarController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });
            _regionalizacionIndicadorAgregarController.ControllerContext.Request.Headers.Add("piip-idAccion", "E3C1849C-FE24-4C07-9762-036FA72AF10C");
            _regionalizacionIndicadorAgregarController.ControllerContext.Request.Headers.Add("piip-idInstanciaFlujo", "4C2E62CD-CEAD-48EF-88C6-A50AB5913716");

            await _regionalizacionIndicadorAgregarController.Consultar("Hola");
        }


        [TestMethod]
        [ExpectedException(typeof(HttpResponseException))]
        public async Task RegionalizacionIndicadorAgregarSinBpinErroneo()
        {
            _regionalizacionIndicadorAgregarController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _regionalizacionIndicadorAgregarController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });
            _regionalizacionIndicadorAgregarController.ControllerContext.Request.Headers.Add("piip-idAccion", "E3C1849C-FE24-4C07-9762-036FA72AF10C");
            _regionalizacionIndicadorAgregarController.ControllerContext.Request.Headers.Add("piip-idInstanciaFlujo", "4C2E62CD-CEAD-48EF-88C6-A50AB5913716");

            await _regionalizacionIndicadorAgregarController.Consultar("12345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901");
        }


        [TestMethod]
        public async Task RegionalizacionIndicadorAgregarConsultar_Ok()
        {
            _regionalizacionIndicadorAgregarController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _regionalizacionIndicadorAgregarController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:Backbone");
            _regionalizacionIndicadorAgregarController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });

            _regionalizacionIndicadorAgregarController.ControllerContext.Request.Headers.Add("piip-idAccion", "E3C1849C-FE24-4C07-9762-036FA72AF10C");
            _regionalizacionIndicadorAgregarController.ControllerContext.Request.Headers.Add("piip-idInstanciaFlujo", "4C2E62CD-CEAD-48EF-88C6-A50AB5913716");
            var result = (OkNegotiatedContentResult<RegionalizacionIndicadorAgregarDto>)await _regionalizacionIndicadorAgregarController.Consultar(Bpin);
            Assert.IsNotNull(result.Content);
        }


        [TestMethod]
        public async Task RegionalizacionIndicadorAgregarConsultar_Failed()
        {
            _regionalizacionIndicadorAgregarController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _regionalizacionIndicadorAgregarController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:Backbone");
            _regionalizacionIndicadorAgregarController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });

            _regionalizacionIndicadorAgregarController.ControllerContext.Request.Headers.Add("piip-idAccion", "E3C1849C-FE24-4C07-9762-036FA72AF10C");
            _regionalizacionIndicadorAgregarController.ControllerContext.Request.Headers.Add("piip-idInstanciaFlujo", "4C2E62CD-CEAD-48EF-88C6-A50AB5913716");
            var result = (ResponseMessageResult)await _regionalizacionIndicadorAgregarController.Consultar(Bpin + "1");
            Assert.AreEqual(HttpStatusCode.NotFound, result.Response.StatusCode);
        }



        [TestMethod]
        public async Task RegionalizacionIndicadorAgregarPreview()
        {
            _regionalizacionIndicadorAgregarController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _regionalizacionIndicadorAgregarController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:Backbone");
            _regionalizacionIndicadorAgregarController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });

            var result = (OkNegotiatedContentResult<RegionalizacionIndicadorAgregarDto>)await _regionalizacionIndicadorAgregarController.Preview();
            Assert.IsNotNull(result.Content);
        }


        [TestMethod]
        public async Task RegionalizacionIndicadorAgregarGuardarTemporal_OK()
        {
            _regionalizacionIndicadorAgregarController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _regionalizacionIndicadorAgregarController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:Backbone");
            _regionalizacionIndicadorAgregarController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });

            _regionalizacionIndicadorAgregarController.ControllerContext.Request.Headers.Add("piip-idAccion", "E3C1849C-FE24-4C07-9762-036FA72AF10C");
            _regionalizacionIndicadorAgregarController.ControllerContext.Request.Headers.Add("piip-idInstanciaFlujo", "4C2E62CD-CEAD-48EF-88C6-A50AB5913716");

            var RegionalizacionIndicadorAgregar = new RegionalizacionIndicadorAgregarDto();
            var result = (OkNegotiatedContentResult<HttpResponseMessage>)await _regionalizacionIndicadorAgregarController.Temporal(RegionalizacionIndicadorAgregar);
            Assert.AreEqual(HttpStatusCode.OK, result.Content.StatusCode);
        }


        [TestMethod]
        [ExpectedException(typeof(HttpResponseException), "Parámetro de entrada piip-idAccion inválido.")]
        public async Task RegionalizacionIndicadorAgregarGuardarTemporalConidAccionInvalido__RetornaMensajeInvalido()
        {
            _regionalizacionIndicadorAgregarController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _regionalizacionIndicadorAgregarController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:Backbone");
            _regionalizacionIndicadorAgregarController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });

            _regionalizacionIndicadorAgregarController.Request.Headers.Add("piip-idAccion", "a2cffeb0-42a6-PRUEBA-ad4b-d674d65c40b1");
            _regionalizacionIndicadorAgregarController.Request.Headers.Add("piip-idInstanciaFlujo", "");
            await _regionalizacionIndicadorAgregarController.Temporal(new RegionalizacionIndicadorAgregarDto());

        }


        [TestMethod]
        [ExpectedException(typeof(HttpResponseException), "Parámetro piip-idAccion no recibido.")]
        public async Task RegionalizacionIndicadorAgregarGuardarTemporalConidAccionVacio__RetornaMensajeNoRecibido()
        {
            _regionalizacionIndicadorAgregarController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _regionalizacionIndicadorAgregarController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:Backbone");
            _regionalizacionIndicadorAgregarController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });

            _regionalizacionIndicadorAgregarController.Request.Headers.Add("piip-idInstanciaFlujo", Guid.NewGuid().ToString());
            await _regionalizacionIndicadorAgregarController.Temporal(new RegionalizacionIndicadorAgregarDto());

        }


        [TestMethod]
        [ExpectedException(typeof(HttpResponseException), "Parámetro de entrada piip-idInstanciaFlujo inválido.")]
        public async Task RegionalizacionIndicadorAgregarGuardarTemporalConidInstanciaFlujoInvalido__RetornaMensajeInvalido()
        {
            _regionalizacionIndicadorAgregarController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _regionalizacionIndicadorAgregarController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:Backbone");
            _regionalizacionIndicadorAgregarController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });

            _regionalizacionIndicadorAgregarController.Request.Headers.Add("piip-idAccion", "");
            _regionalizacionIndicadorAgregarController.Request.Headers.Add("piip-idInstanciaFlujo", "a2cffeb0-42a6-PRUEBA-ad4b-d674d65c40b1");
            await _regionalizacionIndicadorAgregarController.Temporal(new RegionalizacionIndicadorAgregarDto());

        }


        [TestMethod]
        [ExpectedException(typeof(HttpResponseException), "Parámetro piip-idInstanciaFlujo no recibido.")]
        public async Task RegionalizacionIndicadorAgregarGuardarTemporalConidInstanciaFlujoVacio__RetornaMensajeNoRecibido()
        {
            _regionalizacionIndicadorAgregarController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _regionalizacionIndicadorAgregarController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:Backbone");
            _regionalizacionIndicadorAgregarController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });

            _regionalizacionIndicadorAgregarController.Request.Headers.Add("piip-idAccion", Guid.NewGuid().ToString());
            await _regionalizacionIndicadorAgregarController.Temporal(new RegionalizacionIndicadorAgregarDto());
        }


        [TestMethod]
        public async Task RegionalizacionIndicadorAgregarGuardarDefinitivo_OK()
        {
            _regionalizacionIndicadorAgregarController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _regionalizacionIndicadorAgregarController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:Backbone");
            _regionalizacionIndicadorAgregarController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });

            _regionalizacionIndicadorAgregarController.ControllerContext.Request.Headers.Add("piip-idAccion", "E3C1849C-FE24-4C07-9762-036FA72AF10C");
            _regionalizacionIndicadorAgregarController.ControllerContext.Request.Headers.Add("piip-idInstanciaFlujo", "4C2E62CD-CEAD-48EF-88C6-A50AB5913716");

            var RegionalizacionIndicadorAgregar = new RegionalizacionIndicadorAgregarDto();
            var result = (OkNegotiatedContentResult<HttpResponseMessage>)await _regionalizacionIndicadorAgregarController.Definitivo(RegionalizacionIndicadorAgregar);
            Assert.AreEqual(HttpStatusCode.OK, result.Content.StatusCode);
        }


        [TestMethod]
        [ExpectedException(typeof(HttpResponseException), "Parámetro de entrada piip-idAccion inválido.")]
        public async Task RegionalizacionIndicadorAgregarGuardarDefinitivoConidAccionInvalido__RetornaMensajeInvalido()
        {
            _regionalizacionIndicadorAgregarController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _regionalizacionIndicadorAgregarController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:Backbone");
            _regionalizacionIndicadorAgregarController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });

            _regionalizacionIndicadorAgregarController.Request.Headers.Add("piip-idAccion", "a2cffeb0-42a6-PRUEBA-ad4b-d674d65c40b1");
            _regionalizacionIndicadorAgregarController.Request.Headers.Add("piip-idInstanciaFlujo", "");
            await _regionalizacionIndicadorAgregarController.Temporal(new RegionalizacionIndicadorAgregarDto());

        }


        [TestMethod]
        [ExpectedException(typeof(HttpResponseException), "Parámetro piip-idAccion no recibido.")]
        public async Task RegionalizacionIndicadorAgregarGuardarDefinitivoConidAccionVacio__RetornaMensajeNoRecibido()
        {
            _regionalizacionIndicadorAgregarController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _regionalizacionIndicadorAgregarController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:Backbone");
            _regionalizacionIndicadorAgregarController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });

            _regionalizacionIndicadorAgregarController.Request.Headers.Add("piip-idInstanciaFlujo", Guid.NewGuid().ToString());
            await _regionalizacionIndicadorAgregarController.Temporal(new RegionalizacionIndicadorAgregarDto());

        }


        [TestMethod]
        [ExpectedException(typeof(HttpResponseException), "Parámetro de entrada piip-idInstanciaFlujo inválido.")]
        public async Task RegionalizacionIndicadorAgregarGuardarDefinitivoConidInstanciaFlujoInvalido__RetornaMensajeInvalido()
        {
            _regionalizacionIndicadorAgregarController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _regionalizacionIndicadorAgregarController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:Backbone");
            _regionalizacionIndicadorAgregarController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });

            _regionalizacionIndicadorAgregarController.Request.Headers.Add("piip-idAccion", "");
            _regionalizacionIndicadorAgregarController.Request.Headers.Add("piip-idInstanciaFlujo", "a2cffeb0-42a6-PRUEBA-ad4b-d674d65c40b1");
            await _regionalizacionIndicadorAgregarController.Temporal(new RegionalizacionIndicadorAgregarDto());

        }


        [TestMethod]
        [ExpectedException(typeof(HttpResponseException), "Parámetro piip-idInstanciaFlujo no recibido.")]
        public async Task RegionalizacionIndicadorAgregarGuardarDefinitivoConidInstanciaFlujoVacio__RetornaMensajeNoRecibido()
        {
            _regionalizacionIndicadorAgregarController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _regionalizacionIndicadorAgregarController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:Backbone");
            _regionalizacionIndicadorAgregarController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });

            _regionalizacionIndicadorAgregarController.Request.Headers.Add("piip-idAccion", Guid.NewGuid().ToString());
            await _regionalizacionIndicadorAgregarController.Temporal(new RegionalizacionIndicadorAgregarDto());
        }



        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                // dispose managed resources
                _regionalizacionIndicadorAgregarController.Dispose();
            }
            // free native resources
        }

        public void Dispose()
        {
            Dispose(true);
        }


    }
}
