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
using DNP.ServiciosNegocio.Dominio.Dto.SGP.Transversales;

namespace DNP.ServiciosNegocio.Web.API.Test.SGP
{
    [TestClass]
    public class ViabilidadSGPControllerTest : IDisposable
    {
        private IViabilidadSGPServicio ViabilidadServicio { get; set; }
        private IAutorizacionUtilidades AutorizacionUtilidades { get; set; }
        private ViabilidadSGPController _viabilidadSGPController;

        [TestInitialize]
        public void Init()
        {
            var contenedor = Configuracion.UnityConfig.Container;
            ViabilidadServicio = contenedor.Resolve<IViabilidadSGPServicio>();
            AutorizacionUtilidades = contenedor.Resolve<IAutorizacionUtilidades>();
            _viabilidadSGPController = new ViabilidadSGPController(ViabilidadServicio, AutorizacionUtilidades);
            _viabilidadSGPController.ControllerContext.Request = new HttpRequestMessage();
            _viabilidadSGPController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _viabilidadSGPController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:Backbone");
            _viabilidadSGPController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });
        }

        [TestMethod]
        public async Task SGPTransversalLeerParametro_Obtener()
        {
            var parametro = "98095";
            var result = (OkNegotiatedContentResult<string>)await _viabilidadSGPController.SGPTransversalLeerParametro(parametro);
            Assert.IsNotNull(result.Content);
        }
        [TestMethod]
        public async Task SGPViabilidadLeerInformacionGeneral()
        {
            var proyectoId = 98095;
            System.Guid instanciaId = new Guid("00000000-0000-0000-0000-000000000000");
            var tipoConceptoViabilidadCode = "1020";
            var result = (OkNegotiatedContentResult<string>)await _viabilidadSGPController.SGPViabilidadLeerInformacionGeneral(proyectoId, instanciaId, tipoConceptoViabilidadCode);
            Assert.IsNotNull(result.Content);
        }
        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                // dispose managed resources
                _viabilidadSGPController.Dispose();
            }
            // free native resources
        }

        [TestMethod]
        public async Task SGPProyectosObtenerEntidadDestinoResponsableFlujo_Test()
        {
            Guid rolId = new Guid("b9b7e381-ce0d-4b67-a81e-d78515415fc2");
            int crTypeId = 102;
            int entidadResponsable = 271;
            int proyectoId = 618453;
            var result = (OkNegotiatedContentResult<EntidadDestinoResponsableFlujoSgpDto>)await _viabilidadSGPController.SGPProyectosObtenerEntidadDestinoResponsableFlujo(rolId, crTypeId, entidadResponsable, proyectoId);
            Assert.IsNotNull(result.Content);
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
}
