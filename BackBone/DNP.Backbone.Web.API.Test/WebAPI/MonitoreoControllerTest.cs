using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web.Http;
using DNP.Backbone.Servicios.Interfaces.Autorizacion;
using DNP.Backbone.Servicios.Interfaces.Proyectos;
using DNP.Backbone.Servicios.Interfaces.ServiciosNegocio;
using DNP.Backbone.Web.API.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.Unity;
using DNP.Backbone.Comunes.Dto;
using System.Web.Http.Results;
using DNP.Backbone.Dominio.Dto.Monitoreo;
using System.Linq;
using DNP.Backbone.Servicios.Interfaces.PowerBI;
using DNP.Backbone.Comunes.Dto.PowerBI;
using DNP.Backbone.Dominio.Dto.PowerBI;
using System.Net;

namespace DNP.Backbone.Web.API.Test.WebAPI
{
    [TestClass]
    public class MonitoreoControllerTest
    {
        private IProyectoServicios _proyectoServicios;
        private IAutorizacionServicios _autorizacionServicios;
        private MonitoreoController _monitoreoController;
        private IServiciosNegocioServicios _serviciosNegocioServicios;
        private IEmbedServicios _embedServicios;
        private IFlujoServicios _flujoServicios;

        [TestInitialize]
        public void Init()
        {
            _proyectoServicios = Config.UnityConfig.Container.Resolve<IProyectoServicios>();
            _serviciosNegocioServicios = Config.UnityConfig.Container.Resolve<IServiciosNegocioServicios>();
            _autorizacionServicios = Config.UnityConfig.Container.Resolve<IAutorizacionServicios>();
            _embedServicios = Config.UnityConfig.Container.Resolve<IEmbedServicios>();
            _flujoServicios = Config.UnityConfig.Container.Resolve<IFlujoServicios>();
            _monitoreoController = new MonitoreoController(_proyectoServicios, _autorizacionServicios, _serviciosNegocioServicios, _embedServicios, _flujoServicios);
            _monitoreoController.ControllerContext.Request = new HttpRequestMessage();
            _monitoreoController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _monitoreoController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:Backbone");
            _monitoreoController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });
        }

        //[TestMethod]
        [ExpectedException(typeof(HttpResponseException))]
        public async Task ObtenerMonitoreoProyectos_ParametrosNull()
        {
            var response = await _monitoreoController.ObtenerMonitoreoProyectos(null);
        }

        [TestMethod]
        public async Task ObtenerMonitoreoProyectos_AplicacionNull()
        {

            var parametros = new ProyectoParametrosDto
            {
                Aplicacion = null,
                IdUsuario = "jdelgado",
                IdObjeto = new Guid("bc154cba-50a5-4209-81ce-7c0ff0aec2ce"),
                ListaIdsRoles =
                                     new List<Guid>()
                                     {
                                             Guid.
                                                 Parse("4fe0a3de-0b14-45ed-9137-248bd206a418"),
                                             Guid.
                                                 Parse("d76678e3-9264-4663-afe9-7bce43828024"),
                                             Guid.
                                                 Parse("1dd225f4-5c34-4c55-b11d-e5856a68839b")
                                     }
            };
            InstanciaProyectoDto instancia = new InstanciaProyectoDto()
            {
                ProyectoParametrosDto = parametros
            };

            try
            {
                var response = await _monitoreoController.ObtenerMonitoreoProyectos(instancia);
            }
            catch (HttpResponseException e)
            {
                Assert.AreEqual(e.Response.StatusCode, HttpStatusCode.BadRequest);
            }
        }

        [TestMethod]
        public async Task ObtenerMonitoreoProyectos_AplicacionEspacioVacio()
        {

            var parametros = new ProyectoParametrosDto
            {
                Aplicacion = " ",
                IdUsuario = "jdelgado",
                IdObjeto = new Guid("bc154cba-50a5-4209-81ce-7c0ff0aec2ce"),
                ListaIdsRoles =
                                     new List<Guid>()
                                     {
                                             Guid.
                                                 Parse("4fe0a3de-0b14-45ed-9137-248bd206a418"),
                                             Guid.
                                                 Parse("d76678e3-9264-4663-afe9-7bce43828024"),
                                             Guid.
                                                 Parse("1dd225f4-5c34-4c55-b11d-e5856a68839b")
                                     }
            };
            InstanciaProyectoDto instancia = new InstanciaProyectoDto()
            {
                ProyectoParametrosDto = parametros
            };
            try
            {
                var response = await _monitoreoController.ObtenerMonitoreoProyectos(instancia);
            }
            catch (HttpResponseException e)
            {
                Assert.AreEqual(e.Response.StatusCode, HttpStatusCode.BadRequest);
            }

        }

        [TestMethod]
        public async Task ObtenerMonitoreoProyectos_IdUsuarioNull()
        {

            var parametros = new ProyectoParametrosDto
            {
                Aplicacion = "AP:Backbone",
                IdUsuario = null,
                IdObjeto = new Guid("bc154cba-50a5-4209-81ce-7c0ff0aec2ce"),
                ListaIdsRoles =
                                     new List<Guid>()
                                     {
                                             Guid.
                                                 Parse("4fe0a3de-0b14-45ed-9137-248bd206a418"),
                                             Guid.
                                                 Parse("d76678e3-9264-4663-afe9-7bce43828024"),
                                             Guid.
                                                 Parse("1dd225f4-5c34-4c55-b11d-e5856a68839b")
                                     }
            };
            InstanciaProyectoDto instancia = new InstanciaProyectoDto()
            {
                ProyectoParametrosDto = parametros
            };
            try
            {
                var response = await _monitoreoController.ObtenerMonitoreoProyectos(instancia);
            }
            catch (HttpResponseException e)
            {
                Assert.AreEqual(e.Response.StatusCode, HttpStatusCode.BadRequest);
            }

        }

        [TestMethod]
        public async Task ObtenerMonitoreoProyectos_IdUsuarioEspacioVacio()
        {

            var parametros = new ProyectoParametrosDto
            {
                Aplicacion = "AP:Backbone",
                IdUsuario = " ",
                IdObjeto = new Guid("bc154cba-50a5-4209-81ce-7c0ff0aec2ce"),
                ListaIdsRoles =
                                     new List<Guid>()
                                     {
                                             Guid.
                                                 Parse("4fe0a3de-0b14-45ed-9137-248bd206a418"),
                                             Guid.
                                                 Parse("d76678e3-9264-4663-afe9-7bce43828024"),
                                             Guid.
                                                 Parse("1dd225f4-5c34-4c55-b11d-e5856a68839b")
                                     }
            };
            InstanciaProyectoDto instancia = new InstanciaProyectoDto()
            {
                ProyectoParametrosDto = parametros
            };
            try
            {
                var response = await _monitoreoController.ObtenerMonitoreoProyectos(instancia);
            }
            catch (HttpResponseException e)
            {
                Assert.AreEqual(e.Response.StatusCode, HttpStatusCode.BadRequest);
            }
        }

        [TestMethod]
        public async Task ObtenerMonitoreoProyectos_IObjetoNull()
        {

            var parametros = new ProyectoParametrosDto
            {
                Aplicacion = "AP:Backbone",
                IdUsuario = "jdelgado",
                ListaIdsRoles =
                                     new List<Guid>()
                                     {
                                             Guid.
                                                 Parse("4fe0a3de-0b14-45ed-9137-248bd206a418"),
                                             Guid.
                                                 Parse("d76678e3-9264-4663-afe9-7bce43828024"),
                                             Guid.
                                                 Parse("1dd225f4-5c34-4c55-b11d-e5856a68839b")
                                     }
            };
            InstanciaProyectoDto instancia = new InstanciaProyectoDto()
            {
                ProyectoParametrosDto = parametros
            };
            try
            {
                var response = await _monitoreoController.ObtenerMonitoreoProyectos(instancia);
            }
            catch (HttpResponseException e)
            {
                Assert.AreEqual(e.Response.StatusCode, HttpStatusCode.BadRequest);
            }
        }

        [TestMethod]
        public async Task ObtenerMonitoreoProyectos_ListaIdsRolesNull()
        {

            var parametros = new ProyectoParametrosDto
            {
                Aplicacion = "AP:Backbone",
                IdUsuario = "jdelgado",
                IdObjeto = Guid.NewGuid()
            };
            InstanciaProyectoDto instancia = new InstanciaProyectoDto()
            {
                ProyectoParametrosDto = parametros
            };
            try
            {
                var response = await _monitoreoController.ObtenerMonitoreoProyectos(instancia);
            }
            catch (HttpResponseException e)
            {
                Assert.AreEqual(e.Response.StatusCode, HttpStatusCode.BadRequest);
            }
        }

        [TestMethod]
        public async Task ObtenerMonitoreoProyectos_ListaIdsRolesVacio()
        {
            var parametros = new ProyectoParametrosDto
            {
                Aplicacion = "AP:Backbone",
                IdUsuario = "jdelgado",
                IdObjeto = Guid.NewGuid(),
                ListaIdsRoles = new List<Guid>()
            };
            InstanciaProyectoDto instancia = new InstanciaProyectoDto()
            {
                ProyectoParametrosDto = parametros
            };
            try
            {
                var response = await _monitoreoController.ObtenerMonitoreoProyectos(instancia);
            }
            catch (HttpResponseException e)
            {
                Assert.AreEqual(e.Response.StatusCode, HttpStatusCode.BadRequest);
            }
        }

        [TestMethod]
        public void ObtenerMonitoreoProyectos_Ok()
        {
            var parametros = new ProyectoParametrosDto
            {
                Aplicacion = "AP:Backbone",
                IdUsuario = "jdelgado",
                IdObjeto = new Guid("bc154cba-50a5-4209-81ce-7c0ff0aec2ce"),
                ListaIdsRoles =
                                    new List<Guid>()
                                    {
                                        Guid.
                                            Parse("4fe0a3de-0b14-45ed-9137-248bd206a418"),
                                        Guid.
                                            Parse("d76678e3-9264-4663-afe9-7bce43828024"),
                                        Guid.
                                            Parse("1dd225f4-5c34-4c55-b11d-e5856a68839b")
                                    }
            };
            InstanciaProyectoDto instancia = new InstanciaProyectoDto()
            {
                ProyectoParametrosDto = parametros
            };
            var actionResult = _monitoreoController.ObtenerMonitoreoProyectos(instancia).Result;
            var contentResult = actionResult as OkNegotiatedContentResult<ProyectoResumenDto>;

            Assert.IsNotNull(contentResult);
            Assert.IsNotNull(contentResult.Content);
        }

        [TestMethod]
        public void ObtenerReportesPowerBI_Ok()
        {
            var parametros = new ParametrosInboxDto
            {
                Aplicacion = "AP:Backbone",
                IdUsuario = "jdelgado",
                IdObjeto = new Guid("bc154cba-50a5-4209-81ce-7c0ff0aec2ce"),
                ListaIdsRoles =
                                    new List<Guid>()
                                    {
                                        Guid.
                                            Parse("4fe0a3de-0b14-45ed-9137-248bd206a418"),
                                        Guid.
                                            Parse("d76678e3-9264-4663-afe9-7bce43828024"),
                                        Guid.
                                            Parse("1dd225f4-5c34-4c55-b11d-e5856a68839b")
                                    }
            };

            EmbedParametrosDto embed = new EmbedParametrosDto()
            {
                ParametrosInboxDto = parametros,
                EmbedFiltroDto = null
            };
            var actionResult = _monitoreoController.ObtenerReportesPowerBI(embed).Result;
            var contentResult = actionResult as OkNegotiatedContentResult<EmbedConfig>;

            Assert.IsNotNull(contentResult);
            Assert.IsNotNull(contentResult.Content);
            Assert.AreNotEqual("Prueba Ok", string.IsNullOrEmpty(contentResult.Content.ErrorMessage));
        }

        [TestMethod]
        public void ObtenerDashboardsPowerBI_Ok()
        {
            var parametros = new ParametrosInboxDto
            {
                Aplicacion = "AP:Backbone",
                IdUsuario = "jdelgado",
                IdObjeto = new Guid("bc154cba-50a5-4209-81ce-7c0ff0aec2ce"),
                ListaIdsRoles =
                                    new List<Guid>()
                                    {
                                        Guid.
                                            Parse("4fe0a3de-0b14-45ed-9137-248bd206a418"),
                                        Guid.
                                            Parse("d76678e3-9264-4663-afe9-7bce43828024"),
                                        Guid.
                                            Parse("1dd225f4-5c34-4c55-b11d-e5856a68839b")
                                    }
            };

            EmbedParametrosDto embed = new EmbedParametrosDto()
            {
                ParametrosInboxDto = parametros,
                EmbedFiltroDto = null
            };
            var actionResult = _monitoreoController.ObtenerDashboardsPowerBI(embed).Result;
            var contentResult = actionResult as OkNegotiatedContentResult<EmbedConfig>;

            Assert.IsNotNull(contentResult);
            Assert.IsNotNull(contentResult.Content);
        }
    }
}
