namespace DNP.Backbone.Test.Servicios
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Text;
    using Backbone.Servicios.Implementaciones.Inbox;
    using Backbone.Servicios.Interfaces.Autorizacion;
    using Backbone.Servicios.Interfaces.ServiciosNegocio;
    using Comunes.Dto;
    using Comunes.Properties;
    using DNP.Backbone.Comunes.Enums;
    using DNP.Backbone.Comunes.Utilidades.ExtensionMethods;
    using DNP.Backbone.Dominio.Dto.Inbox;
    using DNP.Backbone.Servicios.Implementaciones.ServiciosNegocio;
    using DNP.Backbone.Servicios.Interfaces;
    using DNP.Backbone.Servicios.Interfaces.Nivel;
    using Microsoft.Practices.Unity;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class NivelServiciosTest
    {
        private INivelServicios _nivelServicios;

        [TestInitialize]
        public void Init()
        {
            _nivelServicios = Config.UnityConfig.Container.Resolve<INivelServicios>();
        }

        [TestMethod]
        public void ObtenerListaCatalogo_Ok()
        {
            var actionResult = this._nivelServicios.ObtenerPorIdPadreIdNivelTipo(null, "MACROPROCESO", "jdelgado");
            Assert.IsTrue(actionResult.Result.Any());
        }
    }
}
