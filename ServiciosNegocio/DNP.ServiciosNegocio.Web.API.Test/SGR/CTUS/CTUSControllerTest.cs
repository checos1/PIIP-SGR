using System;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Security.Principal;
using System.Threading.Tasks;
using DNP.ServiciosNegocio.Comunes.Autorizacion;
using DNP.ServiciosNegocio.Servicios.Interfaces.SGR.Viabilidad;
using DNP.ServiciosNegocio.Web.API.Controllers.SGR.Viabilidad;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Unity;
using System.Web.Http.Results;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DNP.ServiciosNegocio.Web.API.Test.SGR.CTUS
{
    [TestClass]
    public class CTUSControllerTest : IDisposable
    {
        private ICTUSServicio _ctusServicio { get; set; }
        private IAutorizacionUtilidades _autorizacionUtilidades { get; set; }
        private CTUSController _ctusController;

        [TestInitialize]
        public void Init()
        {
            var contenedor = Configuracion.UnityConfig.Container;
            _ctusServicio = contenedor.Resolve<ICTUSServicio>();
            _autorizacionUtilidades = contenedor.Resolve<IAutorizacionUtilidades>();
            _ctusController = new CTUSController(_ctusServicio, _autorizacionUtilidades);
            _ctusController.ControllerContext.Request = new HttpRequestMessage();
            _ctusController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _ctusController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:Backbone");
            _ctusController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });
        }

        [TestMethod]
        public async Task ValidarInstanciaCTUSNoFinalizada_NoNulo()
        {
            int idProyecto = 617586;
            var result = (OkNegotiatedContentResult<string>)await _ctusController.ValidarInstanciaCTUSNoFinalizada(idProyecto);
            var respuesta = JObject.Parse(result.Content);
            Assert.IsTrue(respuesta.ContainsKey("InstanciaCTUSNoFinalizada"));
            Assert.IsTrue(respuesta.ContainsKey("MensajeRespuesta"));
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                // dispose managed resources
                _ctusController.Dispose();
            }
            // free native resources
        }

        public void Dispose()
        {
            Dispose(true);
        }

    }
}
