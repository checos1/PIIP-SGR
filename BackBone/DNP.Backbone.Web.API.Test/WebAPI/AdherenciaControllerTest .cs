namespace DNP.Backbone.Web.API.Test.WebAPI
{
    using Comunes.Dto;
    using Controllers;
    using DNP.Backbone.Dominio.Dto;
    using DNP.Backbone.Dominio.Dto.AutorizacionNegocio;
    using DNP.Backbone.Servicios.Interfaces.Consola;
    using DNP.Backbone.Servicios.Interfaces.Proyectos;
    using DNP.Backbone.Servicios.Interfaces.ServiciosNegocio;
    using DNP.Backbone.Web.API.Controllers.Entidades;
    using Microsoft.Practices.Unity;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Servicios.Interfaces.Autorizacion;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Security.Principal;
    using System.Web.Http.Results;

    [TestClass]
    public class AdherenciaControllerTest
    {
        private IAutorizacionServicios _autorizacionServicios;
        private AdherenciaController _adherenciaController;

        [TestInitialize]
        public void Init()
        {
            this._autorizacionServicios = Config.UnityConfig.Container.Resolve<IAutorizacionServicios>();
            this._adherenciaController = new AdherenciaController(this._autorizacionServicios);
            this._adherenciaController.ControllerContext.Request = new HttpRequestMessage();
            this._adherenciaController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            this._adherenciaController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:Backbone");
            this._adherenciaController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });

        }

        [TestMethod]
        public void ObtenerAdherencia_Ok()
        {
            var actionResult = this._adherenciaController.ObtenerEntidadPorEntidadId(Guid.NewGuid());

            Assert.IsNotNull(actionResult.Result);
            Assert.IsTrue((actionResult.Result as OkNegotiatedContentResult<List<DNP.Backbone.Dominio.Dto.AutorizacionNegocio.AdherenciaDto>>).Content.Any());
        }

        [TestMethod]
        public void GuardarAdherencia_Ok()
        {
            var actionResult = this._adherenciaController.Guardar(new AdherenciaDto());

            Assert.IsNotNull(actionResult.Result);
            Assert.IsTrue((actionResult.Result as OkNegotiatedContentResult<RespuestaGeneralDto>).Content.Exito);
        }

        [TestMethod]
        public void EliminarAdherencia_Ok()
        {
            var actionResult = this._adherenciaController.Eliminar(1);

            Assert.IsNotNull(actionResult.Result);
            Assert.IsTrue((actionResult.Result as OkNegotiatedContentResult<RespuestaGeneralDto>).Content.Exito);
        }

    }
}
