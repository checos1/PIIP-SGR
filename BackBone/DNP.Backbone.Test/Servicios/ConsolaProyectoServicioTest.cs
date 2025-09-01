namespace DNP.Backbone.Test.Servicios
{
    using Comunes.Dto;
    using DNP.Backbone.Servicios.Interfaces.Consola;
    using Microsoft.Practices.Unity;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Backbone.Servicios.Interfaces.Autorizacion;
    using Backbone.Servicios.Interfaces.ServiciosNegocio;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Summary description for ConsolaProyectoServicioTest
    /// </summary>
    [TestClass]
    public class ConsolaProyectoServicioTest
    {
        private IAutorizacionServicios _autorizacionServicios;
        private IFlujoServicios _flujoServicios;
        private IConsolaProyectosServicio _consolaProyectoServicios;

        [TestInitialize]
        public void Init()
        {
            _flujoServicios = Config.UnityConfig.Container.Resolve<IFlujoServicios>();
            _autorizacionServicios = Config.UnityConfig.Container.Resolve<IAutorizacionServicios>();
            this._consolaProyectoServicios = Config.UnityConfig.Container.Resolve<IConsolaProyectosServicio>();
        }

        [TestMethod]
        public void ObtenerConsolaProyectos_Ok()
        {
            //var parametros = new ProyectoParametrosDto()
            //{
            //    Aplicacion = "AP:Backbone",
            //    IdUsuario = "jdelgado",
            //    IdObjeto = new Guid("bc154cba-50a5-4209-81ce-7c0ff0aec2ce"),
            //    ListaIdsRoles =
            //             new List<Guid>() { Guid.NewGuid(), Guid.Parse("bdccf593-0a83-41d9-876d-ceaceb77bf25") }
            //};
            //string tokenAutorizacionValor = "Basic amRlbGdhZG86MjI4OTE1MDA=";
            //var actionResult = _consolaProyectoServicios.ObtenerProyectos(parametros, tokenAutorizacionValor);

            //Assert.IsNotNull(actionResult.Result);
        }

        [TestMethod]
        public void ObtenerConsolaProyectosFiltro_Ok()
        {
            //var parametros = new ProyectoParametrosDto()
            //{
            //    Aplicacion = "AP:Backbone",
            //    IdUsuario = "jdelgado",
            //    IdObjeto = new Guid("bc154cba-50a5-4209-81ce-7c0ff0aec2ce"),
            //    ListaIdsRoles =
            //             new List<Guid>() { Guid.NewGuid(), Guid.Parse("bdccf593-0a83-41d9-876d-ceaceb77bf25") }
            //};
            //string tokenAutorizacionValor = "Basic amRlbGdhZG86MjI4OTE1MDA=";
            //var actionResult = _consolaProyectoServicios.ObtenerProyectos(parametros, tokenAutorizacionValor, new ProyectoFiltroDto());

            //Assert.IsNotNull(actionResult.Result.GruposEntidades);
            //Assert.IsTrue(actionResult.Result.GruposEntidades.Count > 0);
        }

        [TestMethod]
        public void ObtenerIdAplicacion_Ok()
        {
            var result = _consolaProyectoServicios.ObtenerIdAplicacionPorBpin("2017004150078", "jdelgado");
            Assert.IsNotNull(result);
        }
    }
}
