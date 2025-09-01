namespace DNP.Backbone.Web.API.Test.WebAPI
{
    using Comunes.Dto;
    using Controllers;
    using DNP.Backbone.Dominio.Dto;
    using DNP.Backbone.Servicios.Interfaces.Consola;
    using DNP.Backbone.Servicios.Interfaces.Proyectos;
    using DNP.Backbone.Servicios.Interfaces.ServiciosNegocio;
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
    public class ConsolaProyectoControllerTest
    {
        private IConsolaProyectosServicio _consolaProyectoServicios;
        private IAutorizacionServicios _autorizacionServicios;
        private ConsolaProyectosController _consolaProyectoController;
        private IServiciosNegocioServicios _serviciosNegocioServicios;

        [TestInitialize]
        public void Init()
        {
            this._consolaProyectoServicios = Config.UnityConfig.Container.Resolve<IConsolaProyectosServicio>();
            this._serviciosNegocioServicios = Config.UnityConfig.Container.Resolve<IServiciosNegocioServicios>();
            this._autorizacionServicios = Config.UnityConfig.Container.Resolve<IAutorizacionServicios>();
            this._consolaProyectoController = new ConsolaProyectosController(this._consolaProyectoServicios, this._autorizacionServicios, this._serviciosNegocioServicios);
            this._consolaProyectoController.ControllerContext.Request = new HttpRequestMessage();
            this._consolaProyectoController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            this._consolaProyectoController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:Backbone");
            this._consolaProyectoController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });
        }

        [TestMethod]
        public void ObtenerProyecto_Ok()
        {
            //var parametros = new ProyectoParametrosDto
            //{
            //    Aplicacion = "AP:Backbone",
            //    IdUsuario = "jdelgado",
            //    IdObjeto = new Guid("bc154cba-50a5-4209-81ce-7c0ff0aec2ce"),
            //    ListaIdsRoles =
            //                        new List<Guid>()
            //                        {
            //                            Guid.
            //                                Parse("4fe0a3de-0b14-45ed-9137-248bd206a418"),
            //                            Guid.
            //                                Parse("d76678e3-9264-4663-afe9-7bce43828024"),
            //                            Guid.
            //                                Parse("1dd225f4-5c34-4c55-b11d-e5856a68839b")
            //                        }
            //};
            //InstanciaProyectoDto instancia = new InstanciaProyectoDto()
            //{
            //    ProyectoParametrosDto = parametros
            //};
            //var actionResult = _consolaProyectoController.ObtenerProyectos(instancia).Result;
            //var contentResult = actionResult as OkNegotiatedContentResult<Dominio.Dto.Proyecto.ProyectoDto>;

            //Assert.IsNotNull(contentResult);
            //Assert.IsNotNull(contentResult.Content);
        }

        [TestMethod]
        public void ObtenerIdAplicacion_Ok()
        {
            var result = _consolaProyectoController.ObtenerIdAplicacionPorBpin("2017004150078");
            Assert.IsNotNull(result);
        }
    }
}
