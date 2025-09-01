namespace DNP.Backbone.Web.API.Test.WebApi
{
    using Comunes.Dto;
    using Controllers;
    using DNP.Backbone.Servicios.Interfaces.Monitoreo;
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
    public class MapColumnasControllerTest
    {
        private IAutorizacionServicios _autorizacionServicios;
        private IMapColumnasServicios _mapColumnasServicios;
        private MapColumnasController _mapColumnasController;

        [TestInitialize]
        public void Init()
        {
            _autorizacionServicios = Config.UnityConfig.Container.Resolve<IAutorizacionServicios>();
            _mapColumnasServicios = Config.UnityConfig.Container.Resolve<IMapColumnasServicios>();
            _mapColumnasController = new MapColumnasController(_mapColumnasServicios, _autorizacionServicios);
            _mapColumnasController.ControllerContext.Request = new HttpRequestMessage();
            _mapColumnasController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _mapColumnasController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:Backbone");
            _mapColumnasController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });
        }

        [TestMethod]
        public void CuandoNoEnvioUsuarioObtenerMapColumnas_RetornaExcepcion()
        {
            MapColumnasFiltroDto filtroDto = new MapColumnasFiltroDto
            {
                ParametrosDto = new ParametrosDto
                {
                    Aplicacion = "xxx",
                    IdUsuarioDNP = "xxx",
                    IdsRoles = new List<Guid>()
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

            var actionResult = _mapColumnasController.ObtenerMapColumnas(filtroDto).Result;
            var contentResult = actionResult as OkNegotiatedContentResult<List<MapColumnasDto>>;

            Assert.IsNull(contentResult.Content);
        }

        [TestMethod]
        public void ObtenerMapColumnas_Ok()
        {
            MapColumnasFiltroDto filtroDto = new MapColumnasFiltroDto
            {
                ParametrosDto = new ParametrosDto
                {
                    Aplicacion = "AP:Backbone",
                    IdUsuarioDNP = "jdelgado",
                    IdsRoles = new List<Guid>()
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

            var actionResult = _mapColumnasController.ObtenerMapColumnas(filtroDto).Result;

            var contentResult = actionResult as OkNegotiatedContentResult<List<MapColumnasDto>>;

            Assert.IsNotNull(contentResult);
            Assert.IsNotNull(contentResult.Content);
        }


    }
}
