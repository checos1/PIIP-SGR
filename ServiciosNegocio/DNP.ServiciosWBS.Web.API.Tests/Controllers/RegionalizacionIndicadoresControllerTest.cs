
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
    public class RegionalizacionIndicadoresControllerTest
    {

        private IRegionalizacionIndicadoresServicios RegionalizacionIndicadoresServicios { get; set; }
        private IAutorizacionUtilidades AutorizacionUtilizades { get; set; }
        private RegionalizacionIndicadoresController _regionalizacionIndicadoresController;
        private string Bpin { get; set; }


        [TestInitialize]
        public void Init()
        {
            Bpin = "2017761220016";
            var contenedor = Configuracion.UnityConfig.Container;
            RegionalizacionIndicadoresServicios = contenedor.Resolve<IRegionalizacionIndicadoresServicios>();
            AutorizacionUtilizades = contenedor.Resolve<IAutorizacionUtilidades>();
            _regionalizacionIndicadoresController = new RegionalizacionIndicadoresController(RegionalizacionIndicadoresServicios, AutorizacionUtilizades);
            _regionalizacionIndicadoresController.ControllerContext.Request = new HttpRequestMessage();
        }


        [TestMethod]
        public async Task RegionalizacionIndicadoresUnaLocalizacion()
        {
            _regionalizacionIndicadoresController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _regionalizacionIndicadoresController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });
            _regionalizacionIndicadoresController.ControllerContext.Request.Headers.Add("piip-idAccion", "E3C1849C-FE24-4C07-9762-036FA72AF10C");
            _regionalizacionIndicadoresController.ControllerContext.Request.Headers.Add("piip-idInstanciaFlujo", "4C2E62CD-CEAD-48EF-88C6-A50AB5913716");

            var result = (OkNegotiatedContentResult<RegionalizacionIndicadorDto>)await _regionalizacionIndicadoresController.Consultar(Bpin);
            Assert.IsTrue(result.Content.Vigencias.Count > 0);
        }


        [TestMethod]
        public async Task RegionalizacionIndicadoresPreview()
        {
            _regionalizacionIndicadoresController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _regionalizacionIndicadoresController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:Backbone");
            _regionalizacionIndicadoresController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });

            var result = (OkNegotiatedContentResult<RegionalizacionIndicadorDto>)await _regionalizacionIndicadoresController.Preview();
            Assert.IsNotNull(result.Content);
        }


        [TestMethod]
        public async Task RegionalizacionIndicadoresGuardarTemporal_OK()
        {
            _regionalizacionIndicadoresController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _regionalizacionIndicadoresController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:Backbone");
            _regionalizacionIndicadoresController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });

            _regionalizacionIndicadoresController.ControllerContext.Request.Headers.Add("piip-idAccion", "E3C1849C-FE24-4C07-9762-036FA72AF10C");
            _regionalizacionIndicadoresController.ControllerContext.Request.Headers.Add("piip-idInstanciaFlujo", "4C2E62CD-CEAD-48EF-88C6-A50AB5913716");

            var regionalizacionIndicadores = new RegionalizacionIndicadorDto();
            var result = (OkNegotiatedContentResult<HttpResponseMessage>)await _regionalizacionIndicadoresController.Temporal(regionalizacionIndicadores);
            Assert.AreEqual(HttpStatusCode.OK, result.Content.StatusCode);
        }


        [TestMethod]
        [ExpectedException(typeof(HttpResponseException), "Parámetro de entrada piip-idAccion inválido.")]
        public async Task RegionalizacionIndicadoresGuardarTemporalConidAccionInvalido__RetornaMensajeInvalido()
        {
            _regionalizacionIndicadoresController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _regionalizacionIndicadoresController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:Backbone");
            _regionalizacionIndicadoresController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });

            _regionalizacionIndicadoresController.Request.Headers.Add("piip-idAccion", "a2cffeb0-42a6-PRUEBA-ad4b-d674d65c40b1");
            _regionalizacionIndicadoresController.Request.Headers.Add("piip-idInstanciaFlujo", "");
            await _regionalizacionIndicadoresController.Temporal(new RegionalizacionIndicadorDto());

        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException), "Parámetro piip-idAccion no recibido.")]
        public async Task RegionalizacionIndicadoresGuardarTemporalConidAccionVacio__RetornaMensajeNoRecibido()
        {
            _regionalizacionIndicadoresController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _regionalizacionIndicadoresController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:Backbone");
            _regionalizacionIndicadoresController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });

            _regionalizacionIndicadoresController.Request.Headers.Add("piip-idInstanciaFlujo", Guid.NewGuid().ToString());
            await _regionalizacionIndicadoresController.Temporal(new RegionalizacionIndicadorDto());

        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException), "Parámetro de entrada piip-idInstanciaFlujo inválido.")]
        public async Task RegionalizacionIndicadoresGuardarTemporalConidInstanciaFlujoInvalido__RetornaMensajeInvalido()
        {
            _regionalizacionIndicadoresController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _regionalizacionIndicadoresController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:Backbone");
            _regionalizacionIndicadoresController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });

            _regionalizacionIndicadoresController.Request.Headers.Add("piip-idAccion", "");
            _regionalizacionIndicadoresController.Request.Headers.Add("piip-idInstanciaFlujo", "a2cffeb0-42a6-PRUEBA-ad4b-d674d65c40b1");
            await _regionalizacionIndicadoresController.Temporal(new RegionalizacionIndicadorDto());

        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException), "Parámetro piip-idInstanciaFlujo no recibido.")]
        public async Task RegionalizacionIndicadoresGuardarTemporalConidInstanciaFlujoVacio__RetornaMensajeNoRecibido()
        {
            _regionalizacionIndicadoresController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _regionalizacionIndicadoresController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:Backbone");
            _regionalizacionIndicadoresController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });

            _regionalizacionIndicadoresController.Request.Headers.Add("piip-idAccion", Guid.NewGuid().ToString());
            await _regionalizacionIndicadoresController.Temporal(new RegionalizacionIndicadorDto());
        }


        [TestMethod]
        public async Task RegionalizacionIndicadoresGuardarDefinitivo_OK()
        {
            _regionalizacionIndicadoresController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _regionalizacionIndicadoresController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:Backbone");
            _regionalizacionIndicadoresController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });

            _regionalizacionIndicadoresController.ControllerContext.Request.Headers.Add("piip-idAccion", "E3C1849C-FE24-4C07-9762-036FA72AF10C");
            _regionalizacionIndicadoresController.ControllerContext.Request.Headers.Add("piip-idInstanciaFlujo", "4C2E62CD-CEAD-48EF-88C6-A50AB5913716");

            var regionalizacionProyecto = new RegionalizacionIndicadorDto();
            var result = (OkNegotiatedContentResult<HttpResponseMessage>)await _regionalizacionIndicadoresController.Definitivo(regionalizacionProyecto);
            Assert.AreEqual(HttpStatusCode.OK, result.Content.StatusCode);
        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException), "Parámetro de entrada piip-idAccion inválido.")]
        public async Task RegionalizacionIndicadoresGuardarDefinitivoConidAccionInvalido__RetornaMensajeInvalido()
        {
            _regionalizacionIndicadoresController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _regionalizacionIndicadoresController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:Backbone");
            _regionalizacionIndicadoresController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });

            _regionalizacionIndicadoresController.Request.Headers.Add("piip-idAccion", "a2cffeb0-42a6-PRUEBA-ad4b-d674d65c40b1");
            _regionalizacionIndicadoresController.Request.Headers.Add("piip-idInstanciaFlujo", "");
            await _regionalizacionIndicadoresController.Temporal(new RegionalizacionIndicadorDto());

        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException), "Parámetro piip-idAccion no recibido.")]
        public async Task RegionalizacionIndicadoresGuardarDefinitivoConidAccionVacio__RetornaMensajeNoRecibido()
        {
            _regionalizacionIndicadoresController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _regionalizacionIndicadoresController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:Backbone");
            _regionalizacionIndicadoresController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });

            _regionalizacionIndicadoresController.Request.Headers.Add("piip-idInstanciaFlujo", Guid.NewGuid().ToString());
            await _regionalizacionIndicadoresController.Temporal(new RegionalizacionIndicadorDto());

        }


        [TestMethod]
        [ExpectedException(typeof(HttpResponseException), "Parámetro de entrada piip-idInstanciaFlujo inválido.")]
        public async Task RegionalizacionIndicadoresGuardarDefinitivoConidInstanciaFlujoInvalido__RetornaMensajeInvalido()
        {
            _regionalizacionIndicadoresController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _regionalizacionIndicadoresController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:Backbone");
            _regionalizacionIndicadoresController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });

            _regionalizacionIndicadoresController.Request.Headers.Add("piip-idAccion", "");
            _regionalizacionIndicadoresController.Request.Headers.Add("piip-idInstanciaFlujo", "a2cffeb0-42a6-PRUEBA-ad4b-d674d65c40b1");
            await _regionalizacionIndicadoresController.Temporal(new RegionalizacionIndicadorDto());

        }


        [TestMethod]
        [ExpectedException(typeof(HttpResponseException), "Parámetro piip-idInstanciaFlujo no recibido.")]
        public async Task RegionalizacionIndicadoresGuardarDefinitivoConidInstanciaFlujoVacio__RetornaMensajeNoRecibido()
        {
            _regionalizacionIndicadoresController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _regionalizacionIndicadoresController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:Backbone");
            _regionalizacionIndicadoresController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });

            _regionalizacionIndicadoresController.Request.Headers.Add("piip-idAccion", Guid.NewGuid().ToString());
            await _regionalizacionIndicadoresController.Temporal(new RegionalizacionIndicadorDto());
        }
        
        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                // dispose managed resources
                _regionalizacionIndicadoresController.Dispose();
            }
            // free native resources
        }

        public void Dispose()
        {
            Dispose(true);
        }


    }
}