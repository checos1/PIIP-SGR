namespace DNP.Backbone.Test.Servicios
{
    using Backbone.Servicios.Interfaces.Autorizacion;
    using Backbone.Servicios.Interfaces.ServiciosNegocio;
    using Comunes.Dto;
    using DNP.Backbone.Servicios.Implementaciones.Proyectos;
    using DNP.Backbone.Servicios.Interfaces.Proyectos;
    using Microsoft.Practices.Unity;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    [TestClass]
    public class ProyectoServiciosTest
    {
        //private IAutorizacionServicios _autorizacionServicios;
        private IFlujoServicios _flujoServicios;
        private IProyectoServicios _proyectoServicios;

        [TestInitialize]
        public void Init()
        {
            _flujoServicios = Config.UnityConfig.Container.Resolve<IFlujoServicios>();
            //_autorizacionServicios = Config.UnityConfig.Container.Resolve<IAutorizacionServicios>();
            _proyectoServicios = Config.UnityConfig.Container.Resolve<IProyectoServicios>();
        }

        [TestMethod]
        public void ObtenerProyecto_Ok()
        {
            //var parametros = new ProyectoParametrosDto()
            //{
            //    Aplicacion = "AP:Backbone",
            //    IdUsuario = "jdelgado",
            //    IdObjeto = new Guid("bc154cba-50a5-4209-81ce-7c0ff0aec2ce"),
            //    ListaIdsRoles =
            //                         new List<Guid>() { Guid.NewGuid(), Guid.Parse("bdccf593-0a83-41d9-876d-ceaceb77bf25") }
            //};
            //string tokenAutorizacionValor = "Basic amRlbGdhZG86MjI4OTE1MDA=";
            //var actionResult = _proyectoServicios.ObtenerProyectos(parametros,tokenAutorizacionValor, new ProyectoFiltroDto(), null);
            
            //Assert.IsNotNull(actionResult.Result.GruposEntidades);
            //Assert.IsTrue(actionResult.Result.GruposEntidades.Count > 0);
        }

        [TestMethod]
        public void ObtenerMonitoreoProyectos_Ok()
        {
            var parametros = new ProyectoParametrosDto()
            {
                Aplicacion = "AP:Backbone",
                IdUsuario = "jdelgado",
                IdObjeto = new Guid("bc154cba-50a5-4209-81ce-7c0ff0aec2ce"),
                ListaIdsRoles =
                                     new List<Guid>() { Guid.NewGuid(), Guid.Parse("bdccf593-0a83-41d9-876d-ceaceb77bf25") }
            };
            string tokenAutorizacionValor = "Basic amRlbGdhZG86MjI4OTE1MDA=";
            var actionResult = _proyectoServicios.ObtenerMonitoreoProyectos(parametros, tokenAutorizacionValor, new ProyectoFiltroDto());

            Assert.IsNotNull(actionResult.Result);
            Assert.IsTrue(actionResult.Result.GruposEntidades.Count() > 0);
        }

        [TestMethod]
        public void ActivarInstancia_Ok()
        {
            var parametros = new ProyectoParametrosDto()
            {
                Aplicacion = "AP:Backbone",
                IdUsuario = "jdelgado",
                IdObjeto = new Guid("bc154cba-50a5-4209-81ce-7c0ff0aec2ce"),
                ListaIdsRoles = new List<Guid>() { Guid.NewGuid(), Guid.Parse("bdccf593-0a83-41d9-876d-ceaceb77bf25") },
                InstanciaId = Guid.NewGuid()
            };
            string tokenAutorizacionValor = "Basic amRlbGdhZG86MjI4OTE1MDA=";
            var actionResult = _proyectoServicios.ActivarInstancia(parametros, tokenAutorizacionValor);

            Assert.IsNotNull(actionResult.Result);
        }

        [TestMethod]
        public void PausarInstancia_Ok()
        {
            var parametros = new ProyectoParametrosDto()
            {
                Aplicacion = "AP:Backbone",
                IdUsuario = "jdelgado",
                IdObjeto = new Guid("bc154cba-50a5-4209-81ce-7c0ff0aec2ce"),
                ListaIdsRoles = new List<Guid>() { Guid.NewGuid(), Guid.Parse("bdccf593-0a83-41d9-876d-ceaceb77bf25") },
                InstanciaId = Guid.NewGuid()
            };
            string tokenAutorizacionValor = "Basic amRlbGdhZG86MjI4OTE1MDA=";
            var actionResult = _proyectoServicios.PausarInstancia(parametros, tokenAutorizacionValor);

            Assert.IsNotNull(actionResult.Result);
        }

        [TestMethod]
        public void DetenerInstancia_Ok()
        {
            var parametros = new ProyectoParametrosDto()
            {
                Aplicacion = "AP:Backbone",
                IdUsuario = "jdelgado",
                IdObjeto = new Guid("bc154cba-50a5-4209-81ce-7c0ff0aec2ce"),
                ListaIdsRoles = new List<Guid>() { Guid.NewGuid(), Guid.Parse("bdccf593-0a83-41d9-876d-ceaceb77bf25") },
                InstanciaId = Guid.NewGuid()
            };
            string tokenAutorizacionValor = "Basic amRlbGdhZG86MjI4OTE1MDA=";
            var actionResult = _proyectoServicios.DetenerInstancia(parametros, tokenAutorizacionValor);

            Assert.IsNotNull(actionResult.Result);
        }

        [TestMethod]
        public void CancelarInstanciaMisProcesos_Ok()
        {
            var parametros = new ProyectoParametrosDto()
            {
                Aplicacion = "AP:Backbone",
                IdUsuario = "jdelgado",
                IdObjeto = new Guid("bc154cba-50a5-4209-81ce-7c0ff0aec2ce"),
                ListaIdsRoles = new List<Guid>() { Guid.NewGuid(), Guid.Parse("bdccf593-0a83-41d9-876d-ceaceb77bf25") },
                InstanciaId = Guid.NewGuid()
            };
            string tokenAutorizacionValor = "Basic amRlbGdhZG86MjI4OTE1MDA=";
            var actionResult = _proyectoServicios.CancelarInstanciaMisProcesos(parametros, tokenAutorizacionValor);

            Assert.IsNotNull(actionResult.Result);
        }

        [TestMethod]
        public void ObtenerProyectoContracredito_Ok()
        {
            var actionResult = _proyectoServicios.ObtenerContracreditos(new Dominio.Dto.Proyecto.ProyectoCreditoParametroDto
            {
                TipoEntidad = "Nacional",
                IdEntidad = 41,
                IdFLujo = new Guid("CF1592AA-9087-3D77-B451-6F3557EF3F82"),
                BPIN = "2017011000332",
                NombreProyecto = null
            }, "jdelgado");

            Assert.IsTrue(actionResult.Result.Count() > 0);
        }

        [TestMethod]
        public void ObtenerProyectoCredito_Ok()
        {
            var actionResult = _proyectoServicios.ObtenerCreditos(new Dominio.Dto.Proyecto.ProyectoCreditoParametroDto
            {
                TipoEntidad = "Nacional",
                IdEntidad = 41,
                IdFLujo = new Guid("CF1592AA-9087-3D77-B451-6F3557EF3F82"),
                BPIN = "2017011000332",
                NombreProyecto = null
            }, "jdelgado");

            Assert.IsTrue(actionResult.Result.Count() > 0);
        }

        [TestMethod]
        public void ObtenerTokenMGA_Ok()
        {
            Dominio.Dto.UsuarioLogadoDto usuarioLogeado = new Dominio.Dto.UsuarioLogadoDto()
            {
                IdUsuario = "jrocha"
            };

            var actionResult = _proyectoServicios.ObtenerTokenMGA("bpin", usuarioLogeado, "externo", "token");

            Assert.IsNotNull(actionResult);
        }

        [TestMethod]
        public void ObtenerTokenMGA_usuarioLogeado_Null()
        {
            Dominio.Dto.UsuarioLogadoDto usuarioLogeado = new Dominio.Dto.UsuarioLogadoDto();

            var actionResult = _proyectoServicios.ObtenerTokenMGA("bpin", usuarioLogeado, "externo", "token");

            Assert.IsNull(actionResult);
        }

        [TestMethod]
        public void ObtenerTokenMGA_bpin_Vacio()
        {
            Dominio.Dto.UsuarioLogadoDto usuarioLogeado = new Dominio.Dto.UsuarioLogadoDto()
            {
                IdUsuario = "jrocha"
            };

            var actionResult = _proyectoServicios.ObtenerTokenMGA("", usuarioLogeado, "externo", "token");

            Assert.IsNull(actionResult);
        }

        [TestMethod]
        public void ObtenerTokenMGA_token_Vacio()
        {
            Dominio.Dto.UsuarioLogadoDto usuarioLogeado = new Dominio.Dto.UsuarioLogadoDto()
            {
                IdUsuario = "jrocha"
            };

            var actionResult = _proyectoServicios.ObtenerTokenMGA("bpin", usuarioLogeado, "externo", "");

            Assert.IsNull(actionResult);
        }

        [TestMethod]
        public void ObtenerTokenMGA_tipoUsuario_Vacio()
        {
            Dominio.Dto.UsuarioLogadoDto usuarioLogeado = new Dominio.Dto.UsuarioLogadoDto()
            {
                IdUsuario = "jrocha"
            };

            var actionResult = _proyectoServicios.ObtenerTokenMGA("bpin", usuarioLogeado, "", "token");

            Assert.IsNull(actionResult);
        }

    }
}
