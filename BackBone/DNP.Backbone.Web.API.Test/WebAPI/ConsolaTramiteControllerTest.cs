namespace DNP.Backbone.Web.API.Test.WebApi
{
    using Comunes.Dto;
    using Controllers;
    using DNP.Backbone.Dominio.Dto.Consola;
    using DNP.Backbone.Dominio.Dto.Tramites.Proyectos;
    using DNP.Backbone.Servicios.Interfaces.Consola;
    using Microsoft.Practices.Unity;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Servicios.Interfaces.Autorizacion;
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Security.Principal;
    using System.Web.Http.Results;

    [TestClass]
    public class ConsolaTramiteControllerTest
    {
        private IConsolaTramiteServicios _consolaTramiteServicios;
        private IAutorizacionServicios _autorizacionServicios;
        private ConsolaTramiteController _consolaTramiteController;

        [TestInitialize]
        public void Init()
        {
            _consolaTramiteServicios = Config.UnityConfig.Container.Resolve<IConsolaTramiteServicios>();
            _autorizacionServicios = Config.UnityConfig.Container.Resolve<IAutorizacionServicios>();
            _consolaTramiteController = new ConsolaTramiteController(_consolaTramiteServicios, _autorizacionServicios);
            _consolaTramiteController.ControllerContext.Request = new HttpRequestMessage();
            _consolaTramiteController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _consolaTramiteController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:Backbone");
            _consolaTramiteController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });
        }

        [TestMethod]
        public void ObtenerConsolaTramite_Ok()
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
            InstanciaTramiteDto instancia = new InstanciaTramiteDto()
            {
                ParametrosInboxDto = parametros
            };
            var actionResult = _consolaTramiteController.ObtenerConsolaTramites(instancia).Result;
            
            var contentResult = actionResult as OkNegotiatedContentResult<ConsolaTramiteDto>;

            Assert.IsNotNull(contentResult);
            Assert.IsNotNull(contentResult.Content);
        }

        [TestMethod]
        public void ObtenerProyectosTramite_Ok()
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
            InstanciaTramiteDto instancia = new InstanciaTramiteDto()
            {
                ParametrosInboxDto = parametros
            };
            var actionResult = _consolaTramiteController.ObtenerProyectosTramite(instancia).Result;

            var contentResult = actionResult as OkNegotiatedContentResult<ProyectosTramitesDTO>;

            Assert.IsNotNull(contentResult);
            Assert.IsNotNull(contentResult.Content);
        }

        [TestMethod]
        public void ObtenerIdAplicacion_Ok()
        {
            var result = _consolaTramiteController.ObtenerIdAplicacionPorInstancia(Guid.NewGuid());
            Assert.IsNotNull(result);
        } 
    }
}
