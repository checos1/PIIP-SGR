using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using DNP.ServiciosNegocio.Comunes.Autorizacion;
using DNP.ServiciosNegocio.Servicios.Interfaces.Preguntas;
using DNP.ServiciosNegocio.Servicios.Interfaces.SGR.Viabilidad;
using DNP.ServiciosNegocio.Web.API.Controllers.SGR.Viabilidad;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Unity;
using DNP.ServiciosNegocio.Web.API.Controllers.Preguntas;
using System.Web.Http.Results;
using DNP.ServiciosNegocio.Dominio.Dto.Preguntas;

namespace DNP.ServiciosNegocio.Web.API.Test.Preguntas
{
    [TestClass]
    public class PreguntasPersonalizadasControllerTest
    {

        private IPreguntasPersonalizadasServicio _preguntasPersonalizadasServicio { get; set; }
        private IAutorizacionUtilidades _autorizacionUtilidades { get; set; }
        private PreguntasPersonalizadasController _preguntasPersonalizadasController;

        [TestInitialize]
        public void Init()
        {
            var contenedor = Configuracion.UnityConfig.Container;
            _preguntasPersonalizadasServicio = contenedor.Resolve<IPreguntasPersonalizadasServicio>();
            _autorizacionUtilidades = contenedor.Resolve<IAutorizacionUtilidades>();
            _preguntasPersonalizadasController = new PreguntasPersonalizadasController(_preguntasPersonalizadasServicio, _autorizacionUtilidades);
            _preguntasPersonalizadasController.ControllerContext.Request = new HttpRequestMessage();
            _preguntasPersonalizadasController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _preguntasPersonalizadasController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:Backbone");
            _preguntasPersonalizadasController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });
        }

        [TestMethod]
        public async Task ObtenerConceptosPreviosEmitidos_NoNulo()
        {
            string bpin = "617586";
            int tipoConcepto = 1;
            var result = (OkNegotiatedContentResult<ConceptosPreviosEmitidosDto>)await _preguntasPersonalizadasController.ObtenerConceptosPreviosEmitidos(bpin, tipoConcepto);
            Assert.IsNotNull(result);
        }

    }
}
