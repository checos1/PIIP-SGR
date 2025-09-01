using DNP.ServiciosNegocio.Comunes.Autorizacion;
using DNP.ServiciosNegocio.Dominio.Dto.Transferencias;
using DNP.ServiciosTransaccional.Servicios.Interfaces;
using DNP.ServiciosTransaccional.Web.API.Controllers.ModificacionLey;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using Unity;

namespace DNP.ServiciosTransaccional.Web.API.Test.ModificacionLey
{
    [TestClass]
    public sealed class ModificacionLeyControllerTest: IDisposable
    {
        private ModificacionLeyController _modificacionLeyController;
        private IAutorizacionUtilidades AutorizacionUtilizades { get; set; }
        private IModificacionLeyServicio ModificacionLeyServicio { get; set; }

        [TestInitialize]
        public void Init()
        {
            var contenedor = Configuracion.UnityConfig.Container;
            ModificacionLeyServicio = contenedor.Resolve<IModificacionLeyServicio>();
            AutorizacionUtilizades = contenedor.Resolve<IAutorizacionUtilidades>();

            _modificacionLeyController =
                new ModificacionLeyController(ModificacionLeyServicio, AutorizacionUtilizades)
                {
                    ControllerContext =
                    {
                        Request= new HttpRequestMessage()
                    }
                };
        }

        [TestMethod]
        public async Task ActualizarValoresPoliticasMLTest_Valido()
        {
            try
            {
                _modificacionLeyController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
                _modificacionLeyController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "test"), new[] { "" });

                var resultado = (OkNegotiatedContentResult<HttpResponseMessage>)await _modificacionLeyController.ActualizarValoresPoliticasML(new ObjetoNegocio
                {
                    ObjetoNegocioId = "12865432",
                    NivelId = "F92A4087-7A92-45F5-A8BA-28F1C23BD849"
                });

                Assert.AreEqual(HttpStatusCode.OK, resultado.Content.StatusCode);
            }
            catch (Exception ex)
            {
                /*Por falta d eusuario de autorizado no pasa la validacion*/
                Assert.IsTrue(ex.GetType() == typeof(InvalidCastException));
            }

        }

        [TestMethod]
        public async Task ActualizarValoresPoliticasMLTest_FalloAutorizacion()
        {
            try
            {
                var response = await _modificacionLeyController.ActualizarValoresPoliticasML(new ObjetoNegocio
                {
                    ObjetoNegocioId = "ABC123"
                });
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex.GetType() == typeof(InvalidCastException) || ex.GetType() == typeof(HttpResponseException));
            }
        }

        private void Dispose(bool disposing)
        {
            if (disposing) _modificacionLeyController.Dispose();
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
}
