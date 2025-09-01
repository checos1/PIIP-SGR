
namespace DNP.Backbone.Test.Servicios
{
    using System;
    using System.Collections.Generic;
    using Backbone.Servicios.Implementaciones.Flujos;
    using Backbone.Servicios.Interfaces;
    using Backbone.Servicios.Interfaces.ServiciosNegocio;
    using Comunes.Dto;
    using Comunes.Enums;
    using DNP.Backbone.Servicios.Interfaces.Autorizacion;
    using Microsoft.Practices.Unity;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Newtonsoft.Json;

    [TestClass]
    public class MenuContextoFlujoServicioTest
    {
        private IFlujoServicios _flujoServicios;
        private IClienteHttpServicios _clienteHttpServicios;
        private IAutorizacionServicios _autorizacionServicios;
        private IServiciosNegocioServicios _serviciosNegocioServicios;
        [TestInitialize]
        public void Init()
        {
            _clienteHttpServicios = Config.UnityConfig.Container.Resolve<IClienteHttpServicios>();
            _autorizacionServicios = Config.UnityConfig.Container.Resolve<IAutorizacionServicios>();
            _serviciosNegocioServicios = Config.UnityConfig.Container.Resolve<IServiciosNegocioServicios>();
            _flujoServicios = new FlujoServicios(_clienteHttpServicios, _autorizacionServicios, _serviciosNegocioServicios);
        }


        [TestMethod]
        public void CuandoEnvioParametros_ParaCrearMenu_RetornaNulo()
        {
            var usuario = "jdelgado";
            var instancia = Guid.NewGuid();
            var menuContexto = _flujoServicios.ObtenerFlujoPorInstanciaTarea(usuario, instancia).Result;
            Assert.IsNull(menuContexto);
        }

        [TestMethod]
        public void CuandoEnvioParametros_ParaCrearMenuSencillo_RetornaMenu()
        {
            //var usuario = "jdelgado";
            //var instancia = Guid.Parse("13039628-3b7c-4f03-90d0-cc05af96ca65");
            //var menuContexto = _flujoServicios.ObtenerFlujoPorInstanciaTarea(usuario, instancia).Result;
            //Assert.IsNotNull(menuContexto);
        }

        [TestMethod]
        public void CuandoEnvioParametros_ParaCrearMenuAnidado_RetornaMenu()
        {
            //var usuario = "jdelgado";
            //var instancia = Guid.Parse("18866b32-9c95-4be8-af02-a8f9f2a0e02f");
            //var menuContexto = _flujoServicios.ObtenerFlujoPorInstanciaTarea(usuario, instancia).Result;
            //Assert.IsNotNull(menuContexto);
        }

    }
}

