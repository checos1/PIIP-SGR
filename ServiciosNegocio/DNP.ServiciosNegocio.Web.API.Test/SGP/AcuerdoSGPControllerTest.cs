using DNP.ServiciosNegocio.Comunes.Autorizacion;
using DNP.ServiciosNegocio.Servicios.Interfaces.SGP.Viabilidad;
using DNP.ServiciosNegocio.Web.API.Controllers.SGP;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Unity;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web.Http.Results;
using DNP.ServiciosNegocio.Comunes.Dto.Viabilidad;
using DNP.ServiciosNegocio.Web.API.Controllers.SGP.Viabilidad;

namespace DNP.ServiciosNegocio.Web.API.Test.SGP
{
    [TestClass]
    public class AcuerdoSGPControllerTest : IDisposable
    {
        private IAcuerdoSGPServicio AcuerdoServicio { get; set; }
        private IAutorizacionUtilidades AutorizacionUtilidades { get; set; }
        private AcuerdoSGPController _acuerdoSGPController;

        [TestInitialize]
        public void Init()
        {
            var contenedor = Configuracion.UnityConfig.Container;
            AcuerdoServicio = contenedor.Resolve<IAcuerdoSGPServicio>();
            AutorizacionUtilidades = contenedor.Resolve<IAutorizacionUtilidades>();
            _acuerdoSGPController = new AcuerdoSGPController(AcuerdoServicio, AutorizacionUtilidades);
            _acuerdoSGPController.ControllerContext.Request = new HttpRequestMessage();
            _acuerdoSGPController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _acuerdoSGPController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:Backbone");
            _acuerdoSGPController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });
        }
        
        [TestMethod]
        public async Task SGPAcuerdoLeerProyecto()
        {
            var proyectoId = 98095;
            System.Guid nivelId = new Guid("00000000-0000-0000-0000-000000000000");            
            var result = (OkNegotiatedContentResult<string>)await _acuerdoSGPController.SGPAcuerdoLeerProyecto(proyectoId, nivelId);
            Assert.IsNotNull(result.Content);
        }
        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                // dispose managed resources
                _acuerdoSGPController.Dispose();
            }
            // free native resources
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
}
