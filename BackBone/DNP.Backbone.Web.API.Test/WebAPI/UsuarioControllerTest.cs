using DNP.Autorizacion.Dominio.Dto;
using DNP.Backbone.Comunes.Dto;
using DNP.Backbone.Dominio.Dto.AutorizacionNegocio;
using DNP.Backbone.Dominio.Dto.Usuario;
using DNP.Backbone.Servicios.Interfaces.Autorizacion;
using DNP.Backbone.Servicios.Interfaces.Identidad;
using DNP.Backbone.Servicios.Interfaces.ServiciosNegocio;
using DNP.Backbone.Web.API.Controllers.Usuarios;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web.Http.Results;

namespace DNP.Backbone.Web.API.Test.WebAPI
{
    [TestClass]
    public sealed class UsuarioControllerTest 
    {
        private IIdentidadServicios _identidadServicios;
        private IAutorizacionServicios _autorizacionServicios;
        private UsuarioController _usuarioController;
        private IFlujoServicios _flujoServicios;

        [TestInitialize]
        public void Init()
        {
            
            _autorizacionServicios = Config.UnityConfig.Container.Resolve<IAutorizacionServicios>();
            _identidadServicios = Config.UnityConfig.Container.Resolve<IIdentidadServicios>();
            _flujoServicios = Config.UnityConfig.Container.Resolve<IFlujoServicios>();

            _usuarioController = new UsuarioController(_autorizacionServicios, _identidadServicios, _flujoServicios);
            _usuarioController.ControllerContext.Request = new HttpRequestMessage();
            _usuarioController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _usuarioController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:Backbone");
            _usuarioController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });

        }

        [TestMethod]
        public void ObtenerRoles_Ok()
        {
            //var actionResult = _usuarioController.ObtenerRoles(string.Empty).Result;
            //var contentResult = actionResult as OkNegotiatedContentResult<List<RolDto>>;

            //Assert.IsNotNull(contentResult);
            //Assert.IsNotNull(contentResult.Content);
        }

        [TestMethod]
        public void GuardarRol_Ok()
        {
            //var rolDto = new RolDto();

            //var actionResult = _usuarioController.GuardarRol(rolDto).Result;
            //var contentResult = actionResult as OkNegotiatedContentResult<RespuestaGeneralDto>;

            //Assert.IsNotNull(contentResult);
            //Assert.IsNotNull(contentResult.Content);
        }

        [TestMethod]
        public void EliminarRol_Ok()
        {
            //var rolDto = new RolDto() { UsuarioDnp = "jdelgado", IdRol = Guid.NewGuid() };

            //var actionResult = _usuarioController.EliminarRol(rolDto).Result;
            //var contentResult = actionResult as OkNegotiatedContentResult<RespuestaGeneralDto>;

            //Assert.IsNotNull(contentResult);
            //Assert.IsNotNull(contentResult.Content);
        }

        [TestMethod]
        public void ObtenerPerfiles_Ok()
        {
            //var actionResult = _usuarioController.ObtenerPerfiles().Result;
            //var contentResult = actionResult as OkNegotiatedContentResult<List<PerfilDto>>;

            //Assert.IsNotNull(contentResult);
            //Assert.IsNotNull(contentResult.Content);
        }

        [TestMethod]

        public void ObtenerPerfilesPorAplicacion_Ok()
        {
            //var actionResult = _usuarioController.ObtenerPerfilesPorAplicacion("jdelgado", "PIIP").Result;
            //var contentResult = actionResult as OkNegotiatedContentResult<List<PerfilDto>>;

            //Assert.IsNotNull(contentResult);
            //Assert.IsNotNull(contentResult.Content);
        }

        [TestMethod]
        public void ObtenerRolesDePerfil_Ok()
        {
            //var actionResult = _usuarioController.ObtenerRolesDePerfil(Guid.NewGuid(), "jdelgado").Result;
            //var contentResult = actionResult as OkNegotiatedContentResult<List<RolDto>>;

            //Assert.IsNotNull(contentResult);
            //Assert.IsNotNull(contentResult.Content);
        }

        [TestMethod]
        public void GuardarPerfil_Ok()
        {
            //var dto = new PerfilDto() { IdPerfil = Guid.NewGuid(), UsuarioDnp = "jdelgado" };

            //var actionResult = _usuarioController.GuardarPerfil(dto).Result;
            //var contentResult = actionResult as OkNegotiatedContentResult<RespuestaGeneralDto>;

            //Assert.IsNotNull(contentResult);
            //Assert.IsNotNull(contentResult.Content);
        }

        [TestMethod]
        public void EliminarPerfil_Ok()
        {
            //var dto = new PerfilDto() { IdPerfil = Guid.NewGuid(), UsuarioDnp = "jdelgado" };

            //var actionResult = _usuarioController.EliminarPerfil(dto).Result;
            //var contentResult = actionResult as OkNegotiatedContentResult<bool>;

            //Assert.IsNotNull(contentResult);
            //Assert.IsNotNull(contentResult.Content);
        }

        [TestMethod]
        public void ObtenerProyectosDePerfil_Ok()
        {
            //var actionResult = _usuarioController.ObtenerProyectosDePerfil(Guid.NewGuid(), "jdelgado").Result;
            //var contentResult = actionResult as OkNegotiatedContentResult<List<UsuarioPerfilProyectoDto>>;

            //Assert.IsNotNull(contentResult);
            //Assert.IsNotNull(contentResult.Content);
        }

        [TestMethod]
        public async Task CambiarContrasenaSTSOk()
        {
            UsuarioSTSDto usuarioSTS = new UsuarioSTSDto();
            usuarioSTS.pAplicacion = "API:PIIP";
            usuarioSTS.pNumeroDocumento = "CC202002";
            usuarioSTS.pPassword = "Dnp2022+";
            usuarioSTS.pTipoDocumento = "CC";
            var result = await Task.Run(() => _usuarioController.CambiarContrasenaSTS(usuarioSTS));

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async  Task CambiarContrasenaSTSVacio()
        {
            UsuarioSTSDto usuarioSTS = new UsuarioSTSDto();
            usuarioSTS.pAplicacion = "API:PIIP";
            usuarioSTS.pPassword = "Dnp2022+";
            usuarioSTS.pTipoDocumento = "CC";
            var result = (System.Web.Http.Results.OkNegotiatedContentResult<bool>) await Task.Run(() => _usuarioController.CambiarContrasenaSTS(usuarioSTS));

            Assert.IsFalse(result.Content);
        }

        [TestMethod]
        public async Task EnviarCorreoInvitacionSTSOk()
        {
            NotificarInvitacionUsuarioSTSDto notificacion = new NotificarInvitacionUsuarioSTSDto();
            notificacion.pUsuario = "CC202002";
            notificacion.pNumeroDocumento = "CC202002";
            notificacion.pPassword = "Dnp2022+";
            notificacion.pTipoDocumento = "CC";
            notificacion.pCorreo = "ejemplo@dnp.gov.co";
            var result = await Task.Run(() => _usuarioController.EnviarCorreoInvitacionSTS(notificacion));

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task EnviarCorreoInvitacionSTSVacio()
        {
            NotificarInvitacionUsuarioSTSDto notificacion = new NotificarInvitacionUsuarioSTSDto();
            notificacion.pUsuario = "CC202002";
            notificacion.pPassword = "Dnp2022+";
            notificacion.pTipoDocumento = "CC";
            notificacion.pCorreo = "ejemplo@dnp.gov.co";
            var result = (System.Web.Http.Results.OkNegotiatedContentResult<string>) await Task.Run(() => _usuarioController.EnviarCorreoInvitacionSTS(notificacion));

            Assert.IsTrue(string.IsNullOrEmpty(result.Content));
        }

        [TestMethod]
        public async Task RegistrarUsuarioAPPSTSOk()
        {
            UsuarioSTSDto usuarioSTS = new UsuarioSTSDto();
            usuarioSTS.pAplicacion = "API:PIIP";
            usuarioSTS.pNumeroDocumento = "CC202002";
            usuarioSTS.pPassword = "Dnp2022+";
            usuarioSTS.pTipoDocumento = "CC";
            var result = await Task.Run(() => _usuarioController.RegistrarUsuarioAPPSTS(usuarioSTS));

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task RegistrarUsuarioAPPSTSVacio()
        {
            UsuarioSTSDto usuarioSTS = new UsuarioSTSDto();
            usuarioSTS.pAplicacion = "API:PIIP";
            usuarioSTS.pPassword = "Dnp2022+";
            usuarioSTS.pTipoDocumento = "CC";
            var result = (System.Web.Http.Results.OkNegotiatedContentResult<bool>) await Task.Run(() => _usuarioController.RegistrarUsuarioAPPSTS(usuarioSTS));

            Assert.IsFalse(result.Content);
        }

        [TestMethod]
        public async Task RegistrarUsuarioSTSOk()
        {
            UsuarioSTSDto usuarioSTS = new UsuarioSTSDto();
            usuarioSTS.pAplicacion = "API:PIIP";
            usuarioSTS.pNumeroDocumento = "CC202002";
            usuarioSTS.pPassword = "Dnp2022+";
            usuarioSTS.pTipoDocumento = "CC";
            var result = await Task.Run(() => _usuarioController.RegistrarUsuarioSTS(usuarioSTS));

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task RegistrarUsuarioSTSVacio()
        {
            UsuarioSTSDto usuarioSTS = new UsuarioSTSDto();
            usuarioSTS.pAplicacion = "API:PIIP";
            usuarioSTS.pPassword = "Dnp2022+";
            usuarioSTS.pTipoDocumento = "CC";
            var result = (System.Web.Http.Results.OkNegotiatedContentResult<bool>)await Task.Run(() => _usuarioController.RegistrarUsuarioSTS(usuarioSTS));

            Assert.IsFalse(result.Content);
        }

        [TestMethod]
        public async Task ValidarContrasenaActualSTSOk()
        {
            UsuarioSTSDto usuarioSTS = new UsuarioSTSDto();
            usuarioSTS.pAplicacion = "API:PIIP";
            usuarioSTS.pNumeroDocumento = "CC202002";
            usuarioSTS.pPassword = "Dnp2022+";
            usuarioSTS.pTipoDocumento = "CC";
            var result = await Task.Run(() => _usuarioController.ValidarContrasenaActualSTS(usuarioSTS));

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task ValidarContrasenaActualSTSVacio()
        {
            UsuarioSTSDto usuarioSTS = new UsuarioSTSDto();
            usuarioSTS.pAplicacion = "API:PIIP";
            usuarioSTS.pPassword = "Dnp2022+";
            usuarioSTS.pTipoDocumento = "CC";
            var result = (System.Web.Http.Results.OkNegotiatedContentResult<bool>) await Task.Run(() => _usuarioController.ValidarContrasenaActualSTS(usuarioSTS));

            Assert.IsFalse(result.Content);
        }

        [TestMethod]
        public async Task validarPermisoInactivarUsuario()
        {
            string usuarioDnp = "CC2023202303";
            string usuarioDnpEliminar = "CC505050";
            var result = (System.Web.Http.Results.OkNegotiatedContentResult<string>)await Task.Run(() => _usuarioController.validarPermisoInactivarUsuario(usuarioDnp, usuarioDnpEliminar));
            Assert.IsNotNull(result);
        }

    }
}
