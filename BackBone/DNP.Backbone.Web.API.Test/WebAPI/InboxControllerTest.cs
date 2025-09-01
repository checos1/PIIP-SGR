namespace DNP.Backbone.Web.API.Test.WebApi
{
    using Comunes.Dto;
    using Controllers;
    using DNP.Backbone.Servicios.Interfaces.ServiciosNegocio;
    using DNP.Backbone.Servicios.Interfaces.Tramites;
    using Dominio.Dto.Inbox;
    using Microsoft.Practices.Unity;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Servicios.Interfaces.Autorizacion;
    using Servicios.Interfaces.Inbox;
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Security.Principal;
    using System.Threading.Tasks;
    using System.Web.Http;
    using System.Web.Http.Results;

    [TestClass]
    public class InboxControllerTest
    {
        private IInboxServicios _inboxServicios;
        private IAutorizacionServicios _autorizacionServicios;
        private InboxController _inboxController;
        private IServiciosNegocioServicios _serviciosNegocioServicios;
        private ITramiteServicios _tramiteServicios;

        [TestInitialize]
        public void Init()
        {
            _inboxServicios = Config.UnityConfig.Container.Resolve<IInboxServicios>();
            _serviciosNegocioServicios = Config.UnityConfig.Container.Resolve<IServiciosNegocioServicios>();
            _autorizacionServicios = Config.UnityConfig.Container.Resolve<IAutorizacionServicios>();
            _tramiteServicios = Config.UnityConfig.Container.Resolve<ITramiteServicios>();
            _inboxController = new InboxController(_inboxServicios, _autorizacionServicios, _serviciosNegocioServicios, _tramiteServicios);
            _inboxController.ControllerContext.Request = new HttpRequestMessage();
            _inboxController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _inboxController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:Backbone");
            _inboxController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });
        }

        [TestMethod]
        public async Task ObtenerInbox_ParametrosNull()
        {
            try
            {
                var response = await _inboxController.ObtenerInbox(new InstanciaInboxDto());
            }
            catch (HttpResponseException e)
            {
                Assert.AreEqual(e.Response.StatusCode, HttpStatusCode.NoContent);
            }
        }

        [TestMethod]
        public async Task ObtenerInbox_AplicacionNull()
        {

            var parametros = new ParametrosInboxDto
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
            InstanciaInboxDto instancia = new InstanciaInboxDto()
            {
                ParametrosInboxDto = parametros
            };

            try
            {
                var response = await _inboxController.ObtenerInbox(instancia);
            }
            catch (HttpResponseException e)
            {
                Assert.AreEqual(e.Response.StatusCode, HttpStatusCode.BadRequest);
            }
        }

        [TestMethod]
        public async Task ObtenerInbox_AplicacionEspacioVacio()
        {

            var parametros = new ParametrosInboxDto
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
            InstanciaInboxDto instancia = new InstanciaInboxDto()
            {
                ParametrosInboxDto = parametros
            };

            try
            {
                var response = await _inboxController.ObtenerInbox(instancia);
            }
            catch (HttpResponseException e)
            {
                Assert.AreEqual(e.Response.StatusCode, HttpStatusCode.BadRequest);
            }

        }

        [TestMethod]
        public async Task ObtenerInbox_IdUsuarioNull()
        {

            var parametros = new ParametrosInboxDto
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
            InstanciaInboxDto instancia = new InstanciaInboxDto()
            {
                ParametrosInboxDto = parametros
            };
            try
            {
                var response = await _inboxController.ObtenerInbox(instancia);
            }
            catch (HttpResponseException e)
            {
                Assert.AreEqual(e.Response.StatusCode, HttpStatusCode.BadRequest);
            }

        }

        [TestMethod]
        public async Task ObtenerInbox_IdUsuarioEspacioVacio()
        {

            var parametros = new ParametrosInboxDto
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
            InstanciaInboxDto instancia = new InstanciaInboxDto()
            {
                ParametrosInboxDto = parametros
            };

            try
            {
                var response = await _inboxController.ObtenerInbox(instancia);
            }
            catch (HttpResponseException e)
            {
                Assert.AreEqual(e.Response.StatusCode, HttpStatusCode.BadRequest);
            }
        }

        [TestMethod]
        public async Task ObtenerInbox_IObjetoNull()
        {

            var parametros = new ParametrosInboxDto
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
            InstanciaInboxDto instancia = new InstanciaInboxDto()
            {
                ParametrosInboxDto = parametros
            };

            try
            {
                var response = await _inboxController.ObtenerInbox(instancia);
            }
            catch (HttpResponseException e)
            {
                Assert.AreEqual(e.Response.StatusCode, HttpStatusCode.BadRequest);
            }
        }

        [TestMethod]
        public async Task ObtenerInbox_ListaIdsRolesNull()
        {

            var parametros = new ParametrosInboxDto
            {
                Aplicacion = "AP:Backbone",
                IdUsuario = "jdelgado",
                IdObjeto = Guid.NewGuid()
            };
            InstanciaInboxDto instancia = new InstanciaInboxDto()
            {
                ParametrosInboxDto = parametros
            };
            try
            {
                var response = await _inboxController.ObtenerInbox(instancia);
            }
            catch (HttpResponseException e)
            {
                Assert.AreEqual(e.Response.StatusCode, HttpStatusCode.BadRequest);
            }

        }

        [TestMethod]
        public async Task ObtenerInbox_ListaIdsRolesVacio()
        {
            var parametros = new ParametrosInboxDto
            {
                Aplicacion = "AP:Backbone",
                IdUsuario = "jdelgado",
                IdObjeto = Guid.NewGuid(),
                ListaIdsRoles = new List<Guid>()
            };
            InstanciaInboxDto instancia = new InstanciaInboxDto()
            {
                ParametrosInboxDto = parametros
            };

            try
            {
                var response = await _inboxController.ObtenerInbox(instancia);
            }
            catch (HttpResponseException e)
            {
                Assert.AreEqual(e.Response.StatusCode, HttpStatusCode.BadRequest);
            }

        }

        [TestMethod]
        public void ObtenerInbox_Ok()
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
            InstanciaInboxDto instancia = new InstanciaInboxDto()
            {
                ParametrosInboxDto = parametros
            };
            var actionResult = _inboxController.ObtenerInbox(instancia).Result;

            var contentResult = actionResult as OkNegotiatedContentResult<InboxDto>;

            Assert.IsNotNull(contentResult);
            Assert.IsNotNull(contentResult.Content);
            Assert.AreEqual("Prueba Ok", contentResult.Content.Mensaje);
        }



    }
}
