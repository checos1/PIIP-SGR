namespace DNP.Backbone.Web.API.Test.WebApi
{
    using Comunes.Dto;
    using DNP.Backbone.Dominio.Dto.Preguntas;
    using DNP.Backbone.Servicios.Interfaces.Preguntas;
    using DNP.Backbone.Web.API.Controllers.Preguntas;
    using Microsoft.Practices.Unity;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Servicios.Interfaces.Autorizacion;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Security.Principal;
    using System.Web.Http.Results;

    [TestClass]
    public class PreguntasControllerTest
    {
        private IPreguntasPersonalizadasServicios _preguntasServicios;
        private IAutorizacionServicios _autorizacionServicios;
        private PreguntasPersonalizadasController _preguntasController;

        [TestInitialize]
        public void Init()
        {
            _preguntasServicios = Config.UnityConfig.Container.Resolve<IPreguntasPersonalizadasServicios>();
            _autorizacionServicios = Config.UnityConfig.Container.Resolve<IAutorizacionServicios>();
            _preguntasController = new PreguntasPersonalizadasController(_autorizacionServicios, _preguntasServicios);
            _preguntasController.ControllerContext.Request = new HttpRequestMessage();
            _preguntasController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _preguntasController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:Backbone");
            _preguntasController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });
        }

        [TestMethod]
        public void ObtenerPreguntasPersonalizadasComponenteSGRTest()
        {
            var bpin = "582687";
            var nivel = new System.Guid("2FED53C9-F3C3-4113-8D53-2BCC68A933C3");
            var instancia = new System.Guid("A90972A6-21F3-437B-9E47-CFBF99D7825D");
            var nombreComponente = "sgrviabilidadrequisitosverificacionrequisitos";

            var actionResult = _preguntasController.ObtenerPreguntasPersonalizadasComponenteSGR(bpin, nivel, instancia, nombreComponente, "A").Result;

            var contentResult = actionResult as OkNegotiatedContentResult<ServicioPreguntasPersonalizadasDto>;

            Assert.IsNotNull(contentResult);
            Assert.IsNotNull(contentResult.Content);
        }

        [TestMethod]
        public void ObtenerPreguntasPersonalizadasComponenteTest()
        {
            
            var bpin = "582687";
            var nivel = new System.Guid("2FED53C9-F3C3-4113-8D53-2BCC68A933C3");
            var instancia = new System.Guid("A90972A6-21F3-437B-9E47-CFBF99D7825D");
            var nombreComponente = "sgrviabilidadrequisitosverificacionrequisitos";

            var actionResult = _preguntasController.ObtenerPreguntasPersonalizadasComponente(bpin, nivel, instancia, nombreComponente, "A").Result;

            var contentResult = actionResult as OkNegotiatedContentResult<ServicioPreguntasPersonalizadasDto>;

            Assert.IsNotNull(contentResult);
            Assert.IsNotNull(contentResult.Content);
        }
    }
}
