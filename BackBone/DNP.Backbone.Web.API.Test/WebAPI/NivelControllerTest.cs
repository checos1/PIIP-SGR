using DNP.Autorizacion.Dominio.Dto;
using DNP.Backbone.Comunes.Dto;
using DNP.Backbone.Dominio.Dto.AutorizacionNegocio;
using DNP.Backbone.Dominio.Dto.Nivel;
using DNP.Backbone.Dominio.Dto.Usuario;
using DNP.Backbone.Servicios.Interfaces.Autorizacion;
using DNP.Backbone.Servicios.Interfaces.Identidad;
using DNP.Backbone.Servicios.Interfaces.Nivel;
using DNP.Backbone.Servicios.Interfaces.ServiciosNegocio;
using DNP.Backbone.Web.API.Controllers.Usuarios;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Principal;
using System.Web.Http.Results;

namespace DNP.Backbone.Web.API.Test.WebAPI
{
    [TestClass]
    public class NivelControllerTest
    {
        private IIdentidadServicios _identidadServicios;
        private IAutorizacionServicios _autorizacionServicios;
        private INivelServicios _nivelServicios;
        private IFlujoServicios _flujoServicios;

        [TestInitialize]
        public void Init()
        {
            _identidadServicios = Config.UnityConfig.Container.Resolve<IIdentidadServicios>();
            _autorizacionServicios = Config.UnityConfig.Container.Resolve<IAutorizacionServicios>();
            _nivelServicios = Config.UnityConfig.Container.Resolve<INivelServicios>();
            _flujoServicios = Config.UnityConfig.Container.Resolve<IFlujoServicios>();
        }

        [TestMethod]
        public void ObtenerPorIdPadreIdNivelTipo_Ok()
        {
            var actionResult = this._nivelServicios.ObtenerPorIdPadreIdNivelTipo(Guid.NewGuid(), "MACROPROCESO", "jdelgado");

            Assert.IsNotNull(actionResult.Result);
            Assert.IsTrue((actionResult.Result as List<NivelDto>).Any());
        }
    }
}
