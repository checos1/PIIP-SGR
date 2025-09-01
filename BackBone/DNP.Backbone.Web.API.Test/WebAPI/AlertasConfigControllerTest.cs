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
    public class AlertasConfigControllerTest
    {
        private IAutorizacionServicios _autorizacionServicios;
        private IServiciosNegocioServicios _serviciosNegocioServicios;
        private IAlertasConfigServicios _alertasConfigServicios;
        private AlertasConfigController _alertasConfigController;

        [TestInitialize]
        public void Init()
        {
            _serviciosNegocioServicios = Config.UnityConfig.Container.Resolve<IServiciosNegocioServicios>();
            _autorizacionServicios = Config.UnityConfig.Container.Resolve<IAutorizacionServicios>();
            _alertasConfigServicios = Config.UnityConfig.Container.Resolve<IAlertasConfigServicios>();
            _alertasConfigController = new AlertasConfigController(_alertasConfigServicios, _autorizacionServicios, _serviciosNegocioServicios);
            _alertasConfigController.ControllerContext.Request = new HttpRequestMessage();
            _alertasConfigController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _alertasConfigController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:Backbone");
            _alertasConfigController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });
        }

        [TestMethod]
        public async Task ObtenerAlertasConfig_Ok()
        {

            var parametros = new AlertasConfigFiltroDto
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
            
            var response = await _alertasConfigServicios.ObtenerAlertasConfig(parametros);
            Assert.IsTrue(response.Count > 0);
        }

        [TestMethod]
        public async Task CrearActualizarAlertasConfig_Ok()
        {

            var parametros = new AlertasConfigFiltroDto
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
                },
                AlertasConfigDto = new AlertasConfigDto
                {
                    NombreAlerta = "teste",
                    MensajeAlerta = "teste"
                }
            };

            var response = await _alertasConfigServicios.CrearActualizarAlertasConfig(parametros);
            Assert.IsNotNull(response);
        }

        [TestMethod]
        public async Task EliminarAlertasConfig_Ok()
        {

            var parametros = new AlertasConfigFiltroDto
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
                },
                AlertasConfigDto = new AlertasConfigDto
                {
                    Id = 1
                }
            };

            var response = await _alertasConfigServicios.EliminarAlertasConfig(parametros);
            Assert.IsNotNull(response);
        }
    }
}
