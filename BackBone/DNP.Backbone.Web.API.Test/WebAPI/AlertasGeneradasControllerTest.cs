using DNP.Backbone.Comunes.Dto;
using DNP.Backbone.Servicios.Interfaces.Autorizacion;
using DNP.Backbone.Servicios.Interfaces.Monitoreo;
using DNP.Backbone.Servicios.Interfaces.ServiciosNegocio;
using DNP.Backbone.Web.API.Controllers;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Principal;
using System.Threading.Tasks;

namespace DNP.Backbone.Web.API.Test.WebAPI
{
    [TestClass]
    public class AlertasGeneradasControllerTest
    {
        private IAutorizacionServicios _autorizacionServicios;
        private IServiciosNegocioServicios _serviciosNegocioServicios;
        private IAlertasGeneradasServicios _alertasGeneradasServicios;
        private AlertasGeneradasController _alertasGeneradasController;

        [TestInitialize]
        public void Init()
        {
            _serviciosNegocioServicios = Config.UnityConfig.Container.Resolve<IServiciosNegocioServicios>();
            _autorizacionServicios = Config.UnityConfig.Container.Resolve<IAutorizacionServicios>();
            _alertasGeneradasServicios = Config.UnityConfig.Container.Resolve<IAlertasGeneradasServicios>();
            _alertasGeneradasController = new AlertasGeneradasController(_alertasGeneradasServicios, _autorizacionServicios);
            _alertasGeneradasController.ControllerContext.Request = new HttpRequestMessage();
            _alertasGeneradasController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _alertasGeneradasController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:Backbone");
            _alertasGeneradasController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });
        }

        [TestMethod]
        public async Task ObtenerAlertasGeneradas_Ok()
        {

            var parametros = new AlertasGeneradasFiltroDto
            {
                ParametrosDto = new ParametrosDto
                {
                    Aplicacion = null,
                    IdUsuarioDNP = "jdelgado",
                    IdsRoles =
                                     new List<Guid>()
                                     {
                                             Guid.
                                                 Parse("4fe0a3de-0b14-45ed-9137-248bd206a418"),
                                             Guid.
                                                 Parse("d76678e3-9264-4663-afe9-7bce43828024"),
                                             Guid.
                                                 Parse("1dd225f4-5c34-4c55-b11d-e5856a68839b")
                                     }
                }
            };
            
            var response = await _alertasGeneradasServicios.ObtenerAlertasGeneradas(parametros);
            Assert.IsTrue(response.Count > 0);
        }

    }
}
