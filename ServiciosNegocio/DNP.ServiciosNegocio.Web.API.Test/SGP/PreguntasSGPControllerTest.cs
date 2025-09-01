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
using DNP.ServiciosNegocio.Web.API.Controllers.Preguntas;

namespace DNP.ServiciosNegocio.Web.API.Test.SGP
{
    [TestClass]
    public class PreguntasSGPControllerTest : IDisposable
    {
        private IPreguntasPersonalizadasSGPServicio PreguntasServicio { get; set; }
        private IAutorizacionUtilidades AutorizacionUtilidades { get; set; }
        private PreguntasPersonalizadasSGPController _preguntasPersonalizadasSGPController;

        [TestInitialize]
        public void Init()
        {
            var contenedor = Configuracion.UnityConfig.Container;
            PreguntasServicio = contenedor.Resolve<IPreguntasPersonalizadasSGPServicio>();
            AutorizacionUtilidades = contenedor.Resolve<IAutorizacionUtilidades>();
            _preguntasPersonalizadasSGPController = new PreguntasPersonalizadasSGPController(PreguntasServicio, AutorizacionUtilidades);
            _preguntasPersonalizadasSGPController.ControllerContext.Request = new HttpRequestMessage();
            _preguntasPersonalizadasSGPController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _preguntasPersonalizadasSGPController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:Backbone");
            _preguntasPersonalizadasSGPController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });
        }
        
        [TestMethod]
        public async Task SGPAcuerdoLeerProyecto()
        {
            var bPin = "2017011000456";
            System.Guid nivelId = new Guid("00000000-0000-0000-0000-000000000000");
            System.Guid instanciaId = new Guid("00000000-0000-0000-0000-000000000000");
            var nombreComponente = "Preguntas";
            var listaRoles = "Ppto";
            var result = (OkNegotiatedContentResult<string>)await _preguntasPersonalizadasSGPController.ObtenerPreguntasPersonalizadasComponenteSGP(bPin, nivelId, instanciaId, nombreComponente, listaRoles);
            Assert.IsNotNull(result.Content);
        }
        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                // dispose managed resources
                _preguntasPersonalizadasSGPController.Dispose();
            }
            // free native resources
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
}
