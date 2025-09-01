using DNP.ServiciosNegocio.Comunes.Autorizacion;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Unity;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web.Http.Results;
using DNP.ServiciosNegocio.Web.API.Controllers.SGP.AdministradorEntidad;
using DNP.ServiciosNegocio.Servicios.Interfaces.AdministradorEntidad;
using DNP.ServiciosNegocio.Dominio.Dto.Proyectos;

namespace DNP.ServiciosNegocio.Web.API.Test.SGP.AdministradorEntidad
{
    [TestClass]
    public class AdministradorEntidadSgpControllerTest : IDisposable
    {
        private IAdministradorEntidadSgpServicio administradorEntidadSgpServicio { get; set; }
        private IAutorizacionUtilidades AutorizacionUtilidades { get; set; }
        private AdministradorEntidadSgpController _administradorEntidadSgpController;


        [TestInitialize]
        public void Init()
        {
            var contenedor = Configuracion.UnityConfig.Container;
            administradorEntidadSgpServicio = contenedor.Resolve<IAdministradorEntidadSgpServicio>();
            AutorizacionUtilidades = contenedor.Resolve<IAutorizacionUtilidades>();
            AutorizacionUtilidades = contenedor.Resolve<IAutorizacionUtilidades>();
            _administradorEntidadSgpController = new AdministradorEntidadSgpController(administradorEntidadSgpServicio, AutorizacionUtilidades);
            _administradorEntidadSgpController.ControllerContext.Request = new HttpRequestMessage();
            _administradorEntidadSgpController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _administradorEntidadSgpController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:Backbone");
            _administradorEntidadSgpController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });
        }

        [TestMethod]
        public async Task SGP_ObtenerSectores()
        {
            var result = (OkNegotiatedContentResult<string>)await _administradorEntidadSgpController.ObtenerSectores();
            Assert.IsNotNull(result.Content);
        }

        [TestMethod]
        public async Task SGP_ObtenerMatrizEntidadDestino()
        {
            ListMatrizEntidadDestinoDto dto = null;
            var result = (OkNegotiatedContentResult<string>)await _administradorEntidadSgpController.ObtenerMatrizEntidadDestino(dto);
            Assert.IsNotNull(result.Content);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                // dispose managed resources
                _administradorEntidadSgpController.Dispose();
            }
            // free native resources
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
}
