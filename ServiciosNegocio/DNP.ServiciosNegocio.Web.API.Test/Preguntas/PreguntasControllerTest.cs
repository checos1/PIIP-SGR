using System.Threading.Tasks;
using System.Web.Http.Results;
using DNP.ServiciosNegocio.Comunes.Autorizacion;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Unity;
using System;
using DNP.ServiciosNegocio.Web.API.Controllers.Preguntas;
using DNP.ServiciosNegocio.Dominio.Dto.Preguntas;
using DNP.ServiciosNegocio.Servicios.Interfaces.Preguntas;

namespace DNP.ServiciosNegocio.Web.API.Test.Preguntas
{
    using System.Diagnostics.CodeAnalysis;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Security.Principal;
    using System.Web.Http;

    [ExcludeFromCodeCoverage]
    [TestClass]
    public class PreguntasControllerTest : IDisposable
    {

        private IPreguntasServicio _preguntasServicio;
        private IAutorizacionUtilidades _autorizacionUtilizades;
        private PreguntasController _preguntasController;
        private IUnityContainer _container;

        [TestInitialize]
        public void Init()
        {
            _container = Configuracion.UnityConfig.Container;
            _preguntasServicio = _container.Resolve<IPreguntasServicio>();
            _autorizacionUtilizades = _container.Resolve<IAutorizacionUtilidades>();
            _preguntasController = new PreguntasController(_preguntasServicio, _autorizacionUtilizades);
            _preguntasController.ControllerContext.Request = new HttpRequestMessage();
            _preguntasController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _preguntasController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:Backbone");
            _preguntasController.ControllerContext.Request.Headers.Add("piip-idInstanciaFlujo", "0bac70cd-f17f-4b24-bf91-78afd37d514e");
            _preguntasController.ControllerContext.Request.Headers.Add("piip-idFormulario", "10C9EB1F-2D06-4742-A678-CC4F836E9710");
            _preguntasController.User = new GenericPrincipal(new GenericIdentity("pedropiip", "Qwer1234"), new[] { "" });
        }

        //[TestMethod]
        //public async Task ConsultarPreguntas_NoNulo()
        //{
        //    _preguntasController.Request.RequestUri = new Uri("http://localhost:49686/api/Preguntas?bpin=2017761220010&IdNivel=332A0F8B-84E7-419F-B262-D449365167F7");
        //    var result =(OkNegotiatedContentResult<ServicioPreguntasDto>) await _preguntasController.Consultar();
        //    Assert.IsNotNull(result.Content);
        //}

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException))]
        public async Task ConsultarPreguntas_ExcepcionGuid()
        {
            _preguntasController.Request.RequestUri = new Uri("http://localhost:49686/api/Preguntas?bpin=2017761220010&IdNivel=");
            await _preguntasController.Consultar();
        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException))]
        public async Task ConsultarPreguntas_ExcepcionBpin()
        {
            _preguntasController.Request.RequestUri = new Uri("http://localhost:49686/api/Preguntas?bpin=&IdNivel=332A0F8B-84E7-419F-B262-D449365167F7");
            await _preguntasController.Consultar();
        }

        [TestMethod]
        public async Task ConsultarPreguntasPreview_NoNulo()
        {
            var result = (OkNegotiatedContentResult<ServicioPreguntasDto>)await _preguntasController.Preview();
            Assert.IsNotNull(result.Content);
        }

        [TestMethod]
        public async Task PreguntasController_Temporal__RetornaTrue()
        {
            _preguntasController.ControllerContext.Request.Headers.Authorization =
                new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");

            _preguntasController.User =
                new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });
            
            var result = (OkNegotiatedContentResult<HttpResponseMessage>)await _preguntasController.Temporal(new ServicioPreguntasDto());
            Assert.AreEqual(HttpStatusCode.OK, result.Content.StatusCode);
        }

        [TestMethod]
        public async Task ProductoController_Definitivo__RetornaTrue()
        {
            _preguntasController.ControllerContext.Request.Headers.Authorization =
                new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");

            _preguntasController.User =
                new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });

            var result = (OkNegotiatedContentResult<HttpResponseMessage>)await _preguntasController.Definitivo(new ServicioPreguntasDto());
            Assert.AreEqual(HttpStatusCode.OK, result.Content.StatusCode);
        }

        [SuppressMessage("ReSharper", "UseNullPropagation")]
        private void Dispose(bool disposing)
        {
            if (!disposing) return;
            if (_preguntasController != null)
                _preguntasController.Dispose();
            _container?.Dispose();
        }
        public void Dispose()
        {
            Dispose(true);
        }
    }
}
