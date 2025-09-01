namespace DNP.ServiciosNegocio.Web.API.Test.Transversales
{
    using Comunes.Autorizacion;
    using DNP.ServiciosNegocio.Dominio.Dto.Transversales;
    using DNP.ServiciosNegocio.Servicios.Interfaces.Transversales;
    using DNP.ServiciosNegocio.Web.API.Controllers.Transversales;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Security.Principal;
    using System.Threading.Tasks;
    using System.Web.Http.Results;
    using Unity;

    [TestClass]
    public class EstadoControllerTest : IDisposable
    {

        private IEstadoServicio EstadoServicio { get; set; }
        private IAutorizacionUtilidades AutorizacionUtilizades { get; set; }
        private EstadoController _estadoController;

        [TestInitialize]
        public void Init()
        {
            var contenedor = Configuracion.UnityConfig.Container;
            AutorizacionUtilizades = contenedor.Resolve<IAutorizacionUtilidades>();
            
            EstadoServicio = contenedor.Resolve<IEstadoServicio>();
            _estadoController = new EstadoController(EstadoServicio, AutorizacionUtilizades);

            _estadoController.ControllerContext.Request = new HttpRequestMessage();
            _estadoController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "amRlbGdhZG86MTYyNTk0NjM=");
            _estadoController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:Backbone");
            _estadoController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });
        }

        [TestMethod]
        public async Task ConsultarCatalogoEntiades_RetornaOk()
        {
            var respuesta = (OkNegotiatedContentResult<List<EstadoDto>>)await _estadoController.ConsultarEstados();
            Assert.IsNotNull(respuesta.Content);
        }
       

      

        #region Dispose

        public void Dispose()
        {
            Dispose(true);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                _estadoController.Dispose();
            }
        }

        #endregion
    }
}
