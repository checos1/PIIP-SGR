namespace DNP.Backbone.Test.Servicios
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Web.Http.Results;
    using Backbone.Servicios.Implementaciones.AutorizacionNegocio;
    using Backbone.Servicios.Interfaces;
    using Backbone.Servicios.Interfaces.Cache;
    using Comunes.Excepciones;
    using DNP.Autorizacion.Dominio.Dto;
    using DNP.Backbone.Dominio.Dto;
    using DNP.Backbone.Persistencia.Implementaciones;
    using DNP.Backbone.Web.API.Test.Mocks;
    using Dominio.Dto.AutorizacionNegocio;
    using Microsoft.Practices.Unity;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class AutorizacionServiciosTest
    {
        private ICacheEntidadesNegocioServicios _cacheEntidadesNegocioServicios;
        private IClienteHttpServicios _clienteHttpServicios;

        private AutorizacionServicios _autorizacionServicios;
#pragma warning disable CS0649 // El campo 'AutorizacionServiciosTest._autorizacionPersistencia' nunca se asigna y siempre tendrá el valor predeterminado null
        private AutorizacionPersistencia _autorizacionPersistencia;
#pragma warning restore CS0649 // El campo 'AutorizacionServiciosTest._autorizacionPersistencia' nunca se asigna y siempre tendrá el valor predeterminado null
        private AutorizacionServiciosMock _autorizacionServiciosMock;

        [TestInitialize]
        public void Init()
        {
            _cacheEntidadesNegocioServicios = Config.UnityConfig.Container.Resolve<ICacheEntidadesNegocioServicios>();
            _clienteHttpServicios = Config.UnityConfig.Container.Resolve<IClienteHttpServicios>();
            _autorizacionServicios = new AutorizacionServicios(_cacheEntidadesNegocioServicios, _clienteHttpServicios, _autorizacionPersistencia);
            //_autorizacionPersistencia = new AutorizacionPersistencia();
            _autorizacionServiciosMock = new AutorizacionServiciosMock();
        }

        [TestMethod]
        public void CuandoNoEnvioNombreUsuario_ParaValidarUsuario_RetornaExcepcion()
        {
            var nombreUsuario = "";
            var hashUsuario = "1234";
            var idAplicacion = "app";
            var nombreServicio = "Servicio";

            try
            {
                var ret = _autorizacionServicios.ValidarUsuario(nombreUsuario, hashUsuario, idAplicacion, nombreServicio);
                if (ret.Exception != null)
                {
                    throw ret.Exception.InnerException;
                }
            }
            catch (BackboneResponseException ex)
            {
                Assert.AreEqual(ex.Response.StatusCode, HttpStatusCode.BadRequest);
                Assert.AreEqual(ex.Response.ReasonPhrase, "Parámetro nombreUsuario no recibido.");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [TestMethod]
        public void CuandoNoEnvioHashUsuario_ParaValidarUsuario_RetornaExcepcion()
        {
            var nombreUsuario = "jdelgado";
            var hashUsuario = "";
            var idAplicacion = "app";
            var nombreServicio = "Servicio";

            try
            {
                var ret = _autorizacionServicios.ValidarUsuario(nombreUsuario, hashUsuario, idAplicacion, nombreServicio);
                if (ret.Exception != null)
                {
                    throw ret.Exception.InnerException;
                }
            }
            catch(BackboneResponseException ex)
            {
                Assert.AreEqual(ex.Response.StatusCode, HttpStatusCode.BadRequest);
                Assert.AreEqual(ex.Response.ReasonPhrase, "Parámetro hashUsuario no recibido.");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [TestMethod]
        public void CuandoNoEnvioidAplicacion_ParaValidarUsuario_RetornaExcepcion()
        {
            var nombreUsuario = "jdelgado";
            var hashUsuario = "1234";
            var idAplicacion = "";
            var nombreServicio = "Servicio";

            try
            {
                var ret = _autorizacionServicios.ValidarUsuario(nombreUsuario, hashUsuario, idAplicacion, nombreServicio);
                if (ret.Exception != null)
                {
                    throw ret.Exception.InnerException;
                }
            }
            catch (BackboneResponseException ex)
            {
                Assert.AreEqual(ex.Response.StatusCode, HttpStatusCode.BadRequest);
                Assert.AreEqual(ex.Response.ReasonPhrase, "Parámetro idAplicacion no recibido.");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [TestMethod]
        public void CuandoNoEnvionombreServicio_ParaValidarUsuario_RetornaExcepcion()
        {
            var nombreUsuario = "jdelgado";
            var hashUsuario = "1234";
            var idAplicacion = "app";
            var nombreServicio = "";

            try
            {
                var ret = _autorizacionServicios.ValidarUsuario(nombreUsuario, hashUsuario, idAplicacion, nombreServicio);
                if (ret.Exception != null)
                {
                    throw ret.Exception.InnerException;
                }
            }
            catch (BackboneResponseException ex)
            {
                Assert.AreEqual(ex.Response.StatusCode, HttpStatusCode.BadRequest);
                Assert.AreEqual(ex.Response.ReasonPhrase, "Parámetro nombreServicio no recibido.");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [TestMethod]
        public void CuandoEnvioParametrosValidos_ParaValidarUsuario_RetornaOk()
        {
            //var nombreUsuario = "jdelgado";
            //var hashUsuario = "1234";
            //var idAplicacion = "AP:Backbone";
            //var nombreServicio = "Servicio";

            //var result = _autorizacionServicios.ValidarUsuario(nombreUsuario, hashUsuario, idAplicacion, nombreServicio).Result;
            //Assert.IsTrue(result.IsSuccessStatusCode);
        }

        [TestMethod]
        //[ExpectedException(typeof(BackboneResponseException))]
        public void CuandoEnvioParametrosValidos_ParaValidarUsuario_RetornaNoAutorizado()
        {
            //var nombreUsuario = "usuarioDnp";
            //var hashUsuario = "1234";
            //var idAplicacion = "AP:Backbone";
            //var nombreServicio = "Servicio";

            //_autorizacionServicios.ValidarUsuario(nombreUsuario, hashUsuario, idAplicacion, nombreServicio);
        }

        [TestMethod]
        public void CuandoEnvioParametros_ParaObtenerSectoresPorEntidadTerritorial_RetornaNulo()
        {
            var usuarioDnp = "usuarioDnp";
            var idEntidadTerritorial = Guid.NewGuid();

            var actionResult = _autorizacionServicios.ObtenerSectoresPorEntidadTerritorial(usuarioDnp, idEntidadTerritorial).Result;
            Assert.IsNull(actionResult);
        }

        [TestMethod]
        public void CuandoEnvioParametros_ParaObtenerSectoresPorEntidadTerritorial_RetornaResultados()
        {
            //var usuarioDnp = "jdelgado";
            //var idEntidadTerritorial = Guid.NewGuid();

            //var actionResult = _autorizacionServicios.ObtenerSectoresPorEntidadTerritorial(usuarioDnp, idEntidadTerritorial).Result;
            //Assert.IsNotNull(actionResult);
        }

        [TestMethod]
        public void CuandoEnvioParametros_ParaObtenerRolesPorEntidadTerritorial_RetornaNulo()
        {
            var usuarioDnp = "usuarioDnp";
            var idEntidadTerritorial = Guid.NewGuid();

            var actionResult = _autorizacionServicios.ObtenerRolesPorEntidadTerritorial(usuarioDnp, idEntidadTerritorial).Result;
            Assert.IsNull(actionResult);
        }

        [TestMethod]
        public void CuandoEnvioParametros_ParaObtenerRolesPorEntidadTerritorial_RetornaResultados()
        {
            //var usuarioDnp = "jdelgado";
            //var idEntidadTerritorial = Guid.NewGuid();

            //var actionResult = _autorizacionServicios.ObtenerRolesPorEntidadTerritorial(usuarioDnp, idEntidadTerritorial).Result;
            //Assert.IsNotNull(actionResult);
        }

        [TestMethod]
        public void CuandoEnvioParametros_ParaObtenerEntidadesPorSectorTerritorial_RetornaNulo()
        {
            var usuarioDnp = "usuarioDnp";
            var idEntidadTerritorial = Guid.NewGuid();
            var idSector = Guid.NewGuid();

            var actionResult = _autorizacionServicios.ObtenerEntidadesPorSectorTerritorial(usuarioDnp, idEntidadTerritorial, idSector).Result;
            Assert.IsNull(actionResult);
        }

        [TestMethod]
        public void CuandoEnvioParametros_ParaObtenerEntidadesPorSectorTerritorial_RetornaResultados()
        {
            //var usuarioDnp = "jdelgado";
            //var idEntidadTerritorial = Guid.NewGuid();
            //var idSector = Guid.NewGuid();

            //var actionResult = _autorizacionServicios.ObtenerEntidadesPorSectorTerritorial(usuarioDnp, idEntidadTerritorial, idSector).Result;
            //Assert.IsNotNull(actionResult);
        }

        [TestMethod]
        public void CuandoEnvioParametros_ParaGuardarConfiguracionRolSector_RetornaFalse()
        {
            //var parametros = new PeticionConfiguracionRolSectorDto() { UsuarioDnp = "usuarioDnp" };

            //var actionResult = _autorizacionServicios.GuardarConfiguracionRolSectorAsync(parametros).Result;
            //Assert.IsFalse(actionResult.Exito);
        }

        [TestMethod]
        public void CuandoEnvioParametros_ParaGuardarPerfil_RetornaTrue()
        {
            var parametros = new PerfilDto() { UsuarioDnp = "usuarioDnp" };

            var actionResult = _autorizacionServiciosMock.GuardarPerfil(parametros).Result;
            Assert.IsTrue(actionResult.Exito);
        }

        [TestMethod]
        public void CuandoEnvioParametros_ParaGuardarPerfil_RetornaFalse()
        {
            PerfilDto parametros = new PerfilDto();

            var actionResult = _autorizacionServiciosMock.GuardarPerfil(parametros).Result;
            Assert.IsFalse(actionResult.Exito);
        }

        [TestMethod]
        public void CuandoEnvioParametros_ParaGuardarRol_RetornaTrue()
        {
            var parametros = new RolDto() { UsuarioDnp = "usuarioDnp" };

            var actionResult = _autorizacionServiciosMock.GuardarRol(parametros).Result;
            Assert.IsTrue(actionResult.Exito);
        }

        [TestMethod]
        public void CuandoEnvioParametros_ParaGuardarRol_RetornaFalse()
        {
            RolDto parametros = new RolDto();

            var actionResult = _autorizacionServiciosMock.GuardarRol(parametros).Result;
            Assert.IsFalse(actionResult.Exito);
        }

        [TestMethod]
        public void CuandoEnvioParametros_ParaEliminarPerfil_RetornaTrue()
        {
            var parametros = new PerfilDto() { UsuarioDnp = "usuarioDnp", IdPerfil = Guid.NewGuid() };

            var actionResult = _autorizacionServiciosMock.EliminarPerfil(parametros).Result;
            Assert.IsTrue(actionResult.Exito);
        }

        [TestMethod]
        public void CuandoEnvioParametros_ParaEliminarPerfil_RetornaFalse()
        {
            var parametros = new PerfilDto();

            var actionResult = _autorizacionServiciosMock.EliminarPerfil(parametros).Result;
            Assert.IsFalse(actionResult.Exito);
        }

        [TestMethod]
        public void CuandoEnvioParametros_ParaEliminarRol_RetornaTrue()
        {
            var parametros = new RolDto() { IdRol = Guid.Parse("22BFE798-D64F-4FCB-87D5-49F584D48229"), UsuarioDnp = "usuarioDnp" };

            var actionResult = _autorizacionServiciosMock.EliminarRol(parametros, parametros.UsuarioDnp).Result;
            Assert.IsTrue(actionResult.Exito);
        }

        [TestMethod]
        public void CuandoEnvioParametros_ParaEliminarRol_RetornaFalse()
        {
            var parametros = new RolDto();

            var actionResult = _autorizacionServiciosMock.EliminarRol(parametros, parametros.UsuarioDnp).Result;
            Assert.IsFalse(actionResult.Exito);
        }

        [TestMethod]
        public void CuandoEnvioParametros_ParaGuardarConfiguracionRolSector_RetornaTrue()
        {
            //var parametros = new PeticionConfiguracionRolSectorDto() { UsuarioDnp = "jdelgado" };

            //var actionResult = _autorizacionServicios.GuardarConfiguracionRolSectorAsync(parametros).Result;
            //Assert.IsTrue(actionResult.Exito);
        }

        [TestMethod]
        public void CuandoEnvioParametros_ParaEditarConfiguracionRolSector_RetornaFalse()
        {
            //var parametros = new PeticionConfiguracionRolSectorDto() { UsuarioDnp = "usuarioDnp" };

            //var actionResult = _autorizacionServicios.EditarConfiguracionRolSector(parametros).Result;
            //Assert.IsFalse(actionResult.Exito);
        }

        [TestMethod]
        public void CuandoEnvioParametros_ParaEditarConfiguracionRolSector_RetornaTrue()
        {
            //var parametros = new PeticionConfiguracionRolSectorDto() { UsuarioDnp = "jdelgado" };

            //var actionResult = _autorizacionServicios.EditarConfiguracionRolSector(parametros).Result;
            //Assert.IsTrue(actionResult.Exito);
        }

        [TestMethod]
        public void CuandoEnvioParametros_ParaCambiarEstadoConfiguracionRolSector_RetornaFalse()
        {
            //var parametros = new PeticionCambioEstadoConfiguracionDto() { UsuarioDnp = "usuarioDnp" };

            //var actionResult = _autorizacionServicios.CambiarEstadoConfiguracionRolSector(parametros).Result;
            //Assert.IsFalse(actionResult.Exito);
        }

        [TestMethod]
        public void CuandoEnvioParametros_ParaCambiarEstadoConfiguracionRolSector_RetornaTrue()
        {
            //var parametros = new PeticionCambioEstadoConfiguracionDto() { UsuarioDnp = "jdelgado" };

            //var actionResult = _autorizacionServicios.CambiarEstadoConfiguracionRolSector(parametros).Result;
            //Assert.IsTrue(actionResult.Exito);
        }

        [TestMethod]
        public void CuandoEnvioParametros_ParaObtenerEntidadesPorListaRoles_RetornaNulo()
        {
            var usuarioDnp = "usuarioDnp";
            var idsRoles = new List<Guid>() { Guid.NewGuid() };

            var actionResult = _autorizacionServicios.ObtenerEntidadesPorListaRoles(idsRoles, usuarioDnp).Result;
            Assert.IsNull(actionResult);
        }

        [TestMethod]
        public void CuandoEnvioParametros_ParaObtenerEntidadesPorListaRoles_RetornaResultados()
        {
            //var usuarioDnp = "jdelgado";
            //var idsRoles = new List<Guid>() { Guid.Parse("bdccf593-0a83-41d9-876d-ceaceb77bf25") };

            //var actionResult = _autorizacionServicios.ObtenerEntidadesPorListaRoles(idsRoles, usuarioDnp).Result;
            //Assert.IsNotNull(actionResult);
        }

        [TestMethod]
        public void CuandoEnvioParametros_ParaObtenerConfiguracionesRolSectorConEntidadesUsuarioTerritorial_RetornaResultados()
        {
            //var usuarioDnp = "usuarioDnp";

            //var actionResult = _autorizacionServicios.ObtenerConfiguracionesRolSector(usuarioDnp).Result;
            //Assert.IsTrue(actionResult.Count > 0);
        }

        [TestMethod]
        public void CuandoEnvioParametros_ParaObtenerConfiguracionesRolSectorConEntidadesUsuarioGlobal_RetornaResultados()
        {
            //var usuarioDnp = "jdelgado";

            //var actionResult = _autorizacionServicios.ObtenerConfiguracionesRolSector(usuarioDnp).Result;
            //Assert.IsNotNull(actionResult);
        }

        [TestMethod]
        public void CuandoEnvioParametros_ParaObtenerRolesPorPerfil_RetornaResultados()
        {
            var usuarioDnp = "jdelgado";
            var idPerfil = new Guid("f945e729-9981-4ddf-809d-8cdf82d9cd8e");

            var actionResult = _autorizacionServiciosMock.ObtenerRolesPorPerfil(idPerfil, usuarioDnp).Result;
            Assert.IsNotNull(actionResult);
        }

        [TestMethod]
        public void CuandoEnvioParametros_ParaObtenerRolesPorPerfil_RetornaNulo()
        {
            var usuarioDnp = "";
            var idPerfil = new Guid("f945e729-9981-4ddf-809d-8cdf82d9cd8e");

            var actionResult = _autorizacionServiciosMock.ObtenerRolesPorPerfil(idPerfil, usuarioDnp).Result;
            Assert.IsNull(actionResult);
        }

        [TestMethod]
        public void CuandoEnvioParametros_ParaObtenerProyectosPorPerfil_RetornaResultados()
        {
            var usuarioDnp = "jdelgado";
            var idPerfil = new Guid("f945e729-9981-4ddf-809d-8cdf82d9cd8e");

            var actionResult = _autorizacionServiciosMock.ObtenerProyectosPorPerfil(idPerfil, usuarioDnp).Result;
            Assert.IsNotNull(actionResult);
        }

        [TestMethod]
        public void CuandoEnvioParametros_ParaObtenerProyectosPorPerfil_RetornaNulo()
        {
            var usuarioDnp = "";
            var idPerfil = new Guid();

            var actionResult = _autorizacionServiciosMock.ObtenerProyectosPorPerfil(idPerfil, usuarioDnp).Result;
            Assert.IsNull(actionResult);
        }

        [TestMethod]
        public void CuandoEnvioParametros_ParaObtenerRoles_RetornaResultados()
        {
            var usuarioDnp = "jdelgado";
            var aplicacion = "E6619511-C9AD-470A-83A9-58F0804BE5F3";

            var actionResult = _autorizacionServiciosMock.ObtenerRoles(usuarioDnp, aplicacion).Result;
            Assert.IsNotNull(actionResult);
        }

        [TestMethod]
        public void CuandoEnvioParametros_ParaObtenerRoles_RetornaNulo()
        {
            var usuarioDnp = "";
            var aplicacion = "";

            var actionResult = _autorizacionServiciosMock.ObtenerRoles(usuarioDnp, aplicacion).Result;
            Assert.IsNull(actionResult);
        }

        [TestMethod]
        public void CuandoEnvioParametros_ParaObtenerPerfilesPorUsuario_RetornaResultados()
        {
            var usuarioDnp = "jdelgado";
            var tipoEntidad = "Nacional";

            var actionResult = _autorizacionServiciosMock.ObtenerPerfilesPorUsuario(usuarioDnp, tipoEntidad).Result;
            Assert.IsNotNull(actionResult);
        }

        [TestMethod]
        public void CuandoEnvioParametros_ParaObtenerPerfiles_RetornaResultados()
        {
            UsuarioLogadoDto usuario = new UsuarioLogadoDto();
            var aplicacion = "E6619511-C9AD-470A-83A9-58F0804BE5F3";

            var actionResult = _autorizacionServiciosMock.ObtenerPerfiles(usuario, aplicacion).Result;
            Assert.IsNotNull(actionResult);
        }

        [TestMethod]
        public void CuandoEnvioParametros_ParaObtenerPerfiles_RetornaNulo()
        {
            var actionResult = _autorizacionServiciosMock.ObtenerPerfiles(null, string.Empty).Result;
            Assert.IsNull(actionResult);
        }

        [TestMethod]
        public void ObtenerUsuariosXEntidad_Ok()
        {
            string tipoEntidad = "Nacional";
            string usuarioDnp = "CC79884089";
            string filtro = string.Empty;
            String nombreUsuario = string.Empty;
            String cuentaUsuario = string.Empty;
            bool estado = true;
            string idUsuarioDnp = string.Empty;
            System.Security.Principal.IPrincipal principal = null;

            var actionResult = _autorizacionServiciosMock.ObtenerUsuariosXEntidad(tipoEntidad, usuarioDnp, filtro, nombreUsuario, cuentaUsuario, estado, idUsuarioDnp, principal).Result;
            Assert.IsNotNull(actionResult);
        }

        [TestMethod]

        public void ObtenerUsuariosPorEntidadSp()
        {
            string tipoEntidad = "Nacional";
            string filtro = string.Empty;
            String nombreUsuario = string.Empty;
            String cuentaUsuario = string.Empty;
            bool estado = true;
            string idUsuarioDnp = "CC79884089";

            var actionResult = _autorizacionServiciosMock.ObtenerUsuariosPorEntidadSp(tipoEntidad, filtro, nombreUsuario, cuentaUsuario, estado, idUsuarioDnp).Result;
            Assert.IsNotNull(actionResult);
        }


        [TestMethod]
        public void ObtenerUsuariosXEntidad_TipoEntidad_Nulo()
        {
            string tipoEntidad = "";
            string usuarioDnp = "CC79884089";
            string filtro = string.Empty;
            String nombreUsuario = string.Empty;
            String cuentaUsuario = string.Empty;
            bool estado = true;
            string idUsuarioDnp = string.Empty;
            System.Security.Principal.IPrincipal principal = null;

            var actionResult = _autorizacionServiciosMock.ObtenerUsuariosXEntidad(tipoEntidad, usuarioDnp, filtro, nombreUsuario, cuentaUsuario, estado, idUsuarioDnp, principal);
            Assert.IsNull(actionResult);
        }

        [TestMethod]
        public void ObtenerUsuariosXEntidad_UsuarioDNP_Nulo()
        {
            string tipoEntidad = "Nacional";
            string usuarioDnp = "";
            string filtro = string.Empty;
            String nombreUsuario = string.Empty;
            String cuentaUsuario = string.Empty;
            bool estado = true;
            string idUsuarioDnp = string.Empty;
            System.Security.Principal.IPrincipal principal = null;

            var actionResult = _autorizacionServiciosMock.ObtenerUsuariosXEntidad(tipoEntidad, usuarioDnp, filtro, nombreUsuario, cuentaUsuario, estado, idUsuarioDnp, principal);
            Assert.IsNull(actionResult);
        }

        [TestMethod]
        public void ObtenerListadoEntidadesXUsuarioAutenticado_Ok()
        {
            string usuarioDnp = "CC79884089";

            var actionResult = _autorizacionServiciosMock.ObtenerListadoEntidadesXUsuarioAutenticado(usuarioDnp).Result;
            Assert.IsNotNull(actionResult);
        }

        [TestMethod]
        public void ObtenerListadoEntidadesXUsuarioAutenticado_Nulo()
        {
            string usuarioDnp = "";

            var actionResult = _autorizacionServiciosMock.ObtenerListadoEntidadesXUsuarioAutenticado(usuarioDnp);
            Assert.IsNull(actionResult);
        }

        [TestMethod]
        public void ObtenerListadoEntidadesXTipoEntidadYUsuarioAutenticado_Ok()
        {
            string tipoEntidad = "Nacional";
            string usuarioDnp = "CC79884089";

            var actionResult = _autorizacionServiciosMock.ObtenerListadoEntidadesXTipoEntidadYUsuarioAutenticado(tipoEntidad, usuarioDnp).Result;
            Assert.IsNotNull(actionResult);
        }

        [TestMethod]
        public void ObtenerListadoEntidadesXTipoEntidadYUsuarioAutenticado_TipoEntidad_Nulo()
        {
            string tipoEntidad = "";
            string usuarioDnp = "CC79884089";

            var actionResult = _autorizacionServiciosMock.ObtenerListadoEntidadesXTipoEntidadYUsuarioAutenticado(tipoEntidad, usuarioDnp);
            Assert.IsNull(actionResult);
        }

        [TestMethod]
        public void ObtenerListadoEntidadesXTipoEntidadYUsuarioAutenticado_UsuarioDNP_Nulo()
        {
            string tipoEntidad = "Nacional";
            string usuarioDnp = "";

            var actionResult = _autorizacionServiciosMock.ObtenerListadoEntidadesXTipoEntidadYUsuarioAutenticado(tipoEntidad, usuarioDnp);
            Assert.IsNull(actionResult);
        }

        [TestMethod]
        public void ObtenerListadoPerfilesXEntidad_Ok()
        {
            string tipoEntidad = "Nacional";
            string usuarioDnp = "CC79884089";

            var actionResult = _autorizacionServiciosMock.ObtenerListadoPerfilesXEntidad(tipoEntidad, usuarioDnp).Result;
            Assert.IsNotNull(actionResult);
        }

        [TestMethod]
        public void ObtenerListadoPerfilesXEntidad_TipoEntidad_Nulo()
        {
            string tipoEntidad = "";
            string usuarioDnp = "CC79884089";

            var actionResult = _autorizacionServiciosMock.ObtenerListadoPerfilesXEntidad(tipoEntidad, usuarioDnp);
            Assert.IsNull(actionResult);
        }

        [TestMethod]
        public void ObtenerListadoPerfilesXEntidad_UsuarioDNP_Nulo()
        {
            string tipoEntidad = "Nacional";
            string usuarioDnp = "";

            var actionResult = _autorizacionServiciosMock.ObtenerListadoPerfilesXEntidad(tipoEntidad, usuarioDnp);
            Assert.IsNull(actionResult);
        }

        // ObtenerPerfilesPorUsuarioXUsuarioAutenticado(string usuarioDnp, string tipoEntidad, string usuarioDnpAutenticado)
        [TestMethod]
        public void ObtenerPerfilesPorUsuarioXUsuarioAutenticado_Ok()
        {
            string tipoEntidad = "Nacional";
            string usuarioDnp = "CC79884089";
            string usuarioDnpAutenticado = "CC79884089";

            var actionResult = _autorizacionServiciosMock.ObtenerPerfilesPorUsuarioXUsuarioAutenticado(tipoEntidad, usuarioDnp, usuarioDnpAutenticado).Result;
            Assert.IsNotNull(actionResult);
        }

        [TestMethod]
        public void ObtenerPerfilesPorUsuarioXUsuarioAutenticado_TipoEntidad_Nulo()
        {
            string tipoEntidad = "";
            string usuarioDnp = "CC79884089";
            string usuarioDnpAutenticado = "CC79884089";

            var actionResult = _autorizacionServiciosMock.ObtenerPerfilesPorUsuarioXUsuarioAutenticado(tipoEntidad, usuarioDnp, usuarioDnpAutenticado);
            Assert.IsNull(actionResult);
        }

        [TestMethod]
        public void ObtenerPerfilesPorUsuarioXUsuarioAutenticado_UsuarioDNP_Nulo()
        {
            string tipoEntidad = "Nacional";
            string usuarioDnp = "";
            string usuarioDnpAutenticado = "CC79884089";

            var actionResult = _autorizacionServiciosMock.ObtenerPerfilesPorUsuarioXUsuarioAutenticado(tipoEntidad, usuarioDnp, usuarioDnpAutenticado);
            Assert.IsNull(actionResult);
        }

        [TestMethod]
        public void ObtenerPerfilesPorUsuarioXUsuarioAutenticado_UsuarioDNPAutenticado_Nulo()
        {
            string tipoEntidad = "Nacional";
            string usuarioDnp = "CC79884089";
            string usuarioDnpAutenticado = "";

            var actionResult = _autorizacionServiciosMock.ObtenerPerfilesPorUsuarioXUsuarioAutenticado(tipoEntidad, usuarioDnp, usuarioDnpAutenticado);
            Assert.IsNull(actionResult);
        }

        [TestMethod]
        public void ObtenerCuentaUsuario_UsuarioDNPAutenticado()
        {
            string nomeCuenta = "Nacional";
            string usuarioDnp = "CC79884089";

            var actionResult = _autorizacionServiciosMock.ObtenerCuentaUsuario(nomeCuenta,usuarioDnp);
            Assert.IsNotNull(actionResult);
        }

        [TestMethod]
        public void ObtenerUsuarioPorCorreoDNP_UsuarioDNPAutenticado()
        {
            string correo = "Nacional@dnp.gov.co";
            string usuarioDnp = "CC79884089";

            var actionResult = _autorizacionServiciosMock.ObtenerUsuarioPorCorreoDNP(correo, usuarioDnp);
            Assert.IsNotNull(actionResult);
        }

        [TestMethod]
        public void ObtenerListaEntidad_UsuarioDNPAutenticado()
        {
            string usuarioDnp = "CC79884089";

            var actionResult = _autorizacionServiciosMock.ObtenerListaEntidad(usuarioDnp,null);
            Assert.IsNotNull(actionResult);
        }

        #region Adherencias

        [TestMethod]
        public void AdherenciaObtener_Ok()
        {
            var actionResult = _autorizacionServiciosMock.ObtenerAdherenciasPorEntidadId(Guid.NewGuid(), "fernando.artmann");
            Assert.IsTrue(actionResult.Result.Any());
        }

        [TestMethod]
        public void AdherenciaObtener_RetornaNulo()
        {
            var actionResult = _autorizacionServiciosMock.ObtenerAdherenciasPorEntidadId(Guid.NewGuid(), string.Empty);
            Assert.IsNull(actionResult.Result);
        }

        [TestMethod]
        public void AdherenciaGuardar_Ok()
        {
            var actionResult = _autorizacionServiciosMock.GuardarAdherencia(new AdherenciaDto(), "fernando.artmann");
            Assert.IsTrue(actionResult.Result.Exito);
        }

        [TestMethod]
        public void AdherenciaGuardar_RetornaNulo()
        {
            var actionResult = _autorizacionServiciosMock.GuardarAdherencia(new AdherenciaDto(), string.Empty);
            Assert.IsTrue(!actionResult.Result.Exito);
        }

        [TestMethod]
        public void AdherenciaEliminar_Ok()
        {
            var actionResult = _autorizacionServiciosMock.EliminarAdherencia(1, "fernando.artmann");
            Assert.IsTrue(actionResult.Result.Exito);
        }

        [TestMethod]
        public void AdherenciaEliminar_RetornaNulo()
        {
            var actionResult = _autorizacionServiciosMock.EliminarAdherencia(1, string.Empty);
            Assert.IsTrue(!actionResult.Result.Exito);
        }

        #endregion Adherencias

        #region Delegados

        [TestMethod]
        public void DelegadoObtener_Ok()
        {
            var actionResult = _autorizacionServiciosMock.ObtenerDelegadosPorEntidadId(Guid.NewGuid(), "fernando.artmann");
            Assert.IsTrue(actionResult.Result.Any());
        }

        [TestMethod]
        public void DelegadoObtener_RetornaNulo()
        {
            var actionResult = _autorizacionServiciosMock.ObtenerDelegadosPorEntidadId(Guid.NewGuid(), string.Empty);
            Assert.IsNull(actionResult.Result);
        }

        [TestMethod]
        public void DelegadoGuardar_Ok()
        {
            var actionResult = _autorizacionServiciosMock.GuardarDelegado(new DelegadoDto(), "fernando.artmann");
            Assert.IsTrue(actionResult.Result.Exito);
        }

        [TestMethod]
        public void DelegadoGuardar_RetornaNulo()
        {
            var actionResult = _autorizacionServiciosMock.GuardarDelegado(new DelegadoDto(), string.Empty);
            Assert.IsTrue(!actionResult.Result.Exito);
        }

        [TestMethod]
        public void DelegadoEliminar_Ok()
        {
            var actionResult = _autorizacionServiciosMock.EliminarDelegado(1, "fernando.artmann");
            Assert.IsTrue(actionResult.Result.Exito);
        }

        [TestMethod]
        public void DelegadoEliminar_RetornaNulo()
        {
            var actionResult = _autorizacionServiciosMock.EliminarDelegado(1, string.Empty);
            Assert.IsTrue(!actionResult.Result.Exito);
        }

        #endregion Delegados
    }

}
