using DNP.ServiciosNegocio.Servicios.Interfaces.FuenteFinanciacion;
using DNP.ServiciosNegocio.Web.API.Controllers.Negocio;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Unity;
using System.Threading.Tasks;
using System.Web.Http.Results;
using DNP.ServiciosNegocio.Dominio.Dto.FuenteFinanciacion;
using System;
using DNP.ServiciosNegocio.Comunes.Autorizacion;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Principal;
using System.Net;

namespace DNP.ServiciosNegocio.Web.API.Test.Negocio
{
    [TestClass]
    public sealed class FuenteFinanciacionProyectoControllerTest : IDisposable
    {
        private IFuenteFinanciacionServicios FuenteFinanciacionServicios { get; set; }
        private IAutorizacionUtilidades AutorizacionUtilizades { get; set; }
        private FuenteFinanciacionProyectoController _fuenteFinanciacionController;
        private string Bpin { get; set; }

        [TestInitialize]
        public void Init()
        {
            Bpin = "2017011000042";
            var contenedor = Configuracion.UnityConfig.Container;
            FuenteFinanciacionServicios = contenedor.Resolve<IFuenteFinanciacionServicios>();
            AutorizacionUtilizades = contenedor.Resolve<IAutorizacionUtilidades>();

            _fuenteFinanciacionController =
                new FuenteFinanciacionProyectoController(FuenteFinanciacionServicios, AutorizacionUtilizades)
                {
                    ControllerContext
                        =
                        {
                            Request
                                = new
                                    HttpRequestMessage()
                        }
                };

            _fuenteFinanciacionController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _fuenteFinanciacionController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:Backbone");
            _fuenteFinanciacionController.ControllerContext.Request.Headers.Add("piip-idAccion", "E3C1849C-FE24-4C07-9762-036FA72AF10C");
            _fuenteFinanciacionController.ControllerContext.Request.Headers.Add("piip-idInstanciaFlujo", "4C2E62CD-CEAD-48EF-88C6-A50AB5913716");
            _fuenteFinanciacionController.ControllerContext.Request.Headers.Add("piip-idFormulario", "4C2E62CD-CEAD-48EF-88C6-A50AB5913716");
            _fuenteFinanciacionController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });
        }

        [TestMethod]
        public async Task FuenteFinanciacionProyectoBpinValido()
        {
            var resultados = (OkNegotiatedContentResult<ProyectoFuenteFinanciacionDto>)await _fuenteFinanciacionController.Consultar(Bpin);
            Assert.IsNotNull(resultados.Content);
        }

        [TestMethod]
        public async Task FuenteFinanciacionProyectoResultadoVacio()
        {
            var resultados = await _fuenteFinanciacionController.Consultar("123456");
            Assert.IsNull(((ResponseMessageResult)resultados).Response.Content);
        }

        [TestMethod]
        public async Task FuenteFinanciacionProyectoPreviewTest()
        {
            var result = (OkNegotiatedContentResult<ProyectoFuenteFinanciacionDto>)await _fuenteFinanciacionController.Preview();
            Assert.IsNotNull(result.Content);
            Assert.IsTrue(result.Content.FuentesFinanciacion.Count == 4);
        }


        [TestMethod]
        public async Task FuenteFinanciacionProyectoGuardarDefinitivo_OK()
        {
            _fuenteFinanciacionController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _fuenteFinanciacionController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:Backbone");
            _fuenteFinanciacionController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });

            _fuenteFinanciacionController.ControllerContext.Request.Headers.Add("piip-idAccion", "E3C1849C-FE24-4C07-9762-036FA72AF10C");
            _fuenteFinanciacionController.ControllerContext.Request.Headers.Add("piip-idInstanciaFlujo", "4C2E62CD-CEAD-48EF-88C6-A50AB5913716");

            var proyectoFuenteFinanciacionDto = new ProyectoFuenteFinanciacionDto();
            var result = (OkNegotiatedContentResult<HttpResponseMessage>)await _fuenteFinanciacionController.Definitivo(proyectoFuenteFinanciacionDto);
            Assert.AreEqual(HttpStatusCode.OK, result.Content.StatusCode);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                // dispose managed resources
                _fuenteFinanciacionController.Dispose();
            }
            // free native resources
        }

        public void Dispose()
        {
            Dispose(true);
        }

    }
}
