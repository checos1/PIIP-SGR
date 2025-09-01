namespace DNP.Backbone.Test.Servicios
{
    using Comunes.Dto;
    using Comunes.Properties;
    using DNP.Backbone.Servicios.Interfaces.Inbox;
    using Microsoft.Practices.Unity;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Collections.Generic;

    [TestClass]
    public class InboxServiciosTest
    {
        private IInboxServicios _inboxServicios;

        [TestInitialize]
        public void Init()
        {
            _inboxServicios = Config.UnityConfig.Container.Resolve<IInboxServicios>();
        }

        [TestMethod]
        public void CuandoEnvioParametrosInbox_NoEncuentraEntidades_RetornaMensajeSinTareas()
        {
            var parametros = new ParametrosInboxDto()
            {
                Aplicacion = "AP:Backbone",
                IdUsuario = "jdelgado",
                IdObjeto = new Guid("D6880615-3CD8-4258-A0A4-821E21146124"),
                ListaIdsRoles =
                                    new List<Guid>() { Guid.NewGuid(), Guid.NewGuid() }
            };
            string tokenAutorizacionValor = "Basic amRlbGdhZG86MjI4OTE1MDA=";
            var actionResult = _inboxServicios.ObtenerInbox(parametros, tokenAutorizacionValor, new ProyectoFiltroDto());
            Assert.AreEqual(Resources.UsuarioNoTieneTareasPendientes, actionResult.Result.Mensaje);
        }

        [TestMethod]
        public void CuandoEnvioParametrosInbox_NoEncuentraObjetosNegocio_RetornaEntidadesConProyectos()
        {
            var parametros = new ParametrosInboxDto()
            {
                Aplicacion = "AP:Backbone",
                IdUsuario = "jdelgado",
                IdObjeto = new Guid("bc154cba-50a5-4209-81ce-7c0ff0aec2ce"),
                ListaIdsRoles =
                                     new List<Guid>() { Guid.NewGuid(), Guid.Parse("bdccf593-0a83-41d9-876d-ceaceb77bf25") }
            };
            string tokenAutorizacionValor = "Basic amRlbGdhZG86MjI4OTE1MDA=";
            var actionResult = _inboxServicios.ObtenerInbox(parametros,tokenAutorizacionValor, new ProyectoFiltroDto());
            Assert.IsNotNull(actionResult.Result.GruposEntidades);
            Assert.IsTrue(actionResult.Result.GruposEntidades.Count > 0);


        }

        [TestMethod]
        public void CuandoEnvioParametrosInbox_RetornaNoResultados()
        {
            var parametros = new ParametrosInboxDto()
            {
                Aplicacion = "AP:Backbone",
                IdUsuario = "jdelgado",
                IdObjeto = new Guid("D6880615-3CD8-4258-A0A4-821E21146124"),
                ListaIdsRoles =
                                     new List<Guid>() { Guid.NewGuid(), Guid.Parse("bdccf593-0a83-41d9-876d-ceaceb771234") }
            };
            string tokenAutorizacionValor = "Basic amRlbGdhZG86MjI4OTE1MDA=";
            var actionResult = _inboxServicios.ObtenerInbox(parametros, tokenAutorizacionValor, new ProyectoFiltroDto());
            Assert.AreEqual(Resources.UsuarioNoTieneTareasPendientes, actionResult.Result.Mensaje);
        }

    

    }
    
}
