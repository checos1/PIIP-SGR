namespace DNP.Backbone.Web.API.Test.WebApi
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Security.Principal;
    using System.Web.Http.Results;
    using Comunes.Dto;
    using Comunes.Excepciones;
    using Controllers.AutorizacionNegocio;
    using Dominio.Dto.AutorizacionNegocio;
    using Microsoft.Practices.Unity;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Servicios.Interfaces.Autorizacion;

    [TestClass]
    public class ConfiguracionEntidadRolSectorControllerTest
    {
        private IAutorizacionServicios _autorizacionNegocioServicios;
        private ConfiguracionEntidadRolSectorController _configuracionEntidadRolSectorController;

        [TestInitialize]
        public void Init()
        {
            _autorizacionNegocioServicios = Config.UnityConfig.Container.Resolve<IAutorizacionServicios>();
            _configuracionEntidadRolSectorController = new ConfiguracionEntidadRolSectorController(_autorizacionNegocioServicios);
            _configuracionEntidadRolSectorController.ControllerContext.Request = new HttpRequestMessage();
            _configuracionEntidadRolSectorController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _configuracionEntidadRolSectorController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:Backbone");
            _configuracionEntidadRolSectorController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });
        }

        [TestMethod]
        public void CuandoNoEnvioUsuarioObtenerConfiguracionesRolSector_RetornaExcepcion()
        {
            var responsable = "xxxxxxx";
            var nombreAplicacion = "xxxxxxx";

            var actionResult = _configuracionEntidadRolSectorController.ObtenerConfiguracionesRolSector(responsable, nombreAplicacion).Result;
            var result = ((OkNegotiatedContentResult<List<TipoEntidadDto>>)actionResult).Content;

            Assert.IsNull(result);
        }

        [TestMethod]
        public void CuandoEnvioUsuarioObtenerConfiguracionesRolSector_RetornaResultados()
        {
            var responsable = "jdelgado";
            var nombreAplicacion = "AP:Backbone";

            var actionResult = _configuracionEntidadRolSectorController.ObtenerConfiguracionesRolSector(responsable, nombreAplicacion).Result;
            var result = ((OkNegotiatedContentResult<List<TipoEntidadDto>>)actionResult).Content;

            Assert.IsTrue(result.Count > 0);
        }

        [TestMethod]
        public void CuandoNoEnvioUsuarioObtenerRolesPorEntidadTerritorial_RetornaExcepcion()
        {
            var responsable = "xxxxxxx";
            var nombreAplicacion = "xxxxxxx";
            var idEntidadTerritorial = Guid.Empty;

            var actionResult = _configuracionEntidadRolSectorController.ObtenerRolesPorEntidadTerritorial(responsable, nombreAplicacion, idEntidadTerritorial).Result;
            var result = ((OkNegotiatedContentResult<List<RolNegocioDto>>)actionResult).Content;

            Assert.IsNull(result);
        }

        [TestMethod]
        public void CuandoEnvioUsuarioObtenerRolesPorEntidadTerritorial_RetornaResultados()
        {
            var responsable = "jdelgado";
            var nombreAplicacion = "AP:Backbone";
            var idEntidadTerritorial = Guid.NewGuid();

            var actionResult = _configuracionEntidadRolSectorController.ObtenerRolesPorEntidadTerritorial(responsable, nombreAplicacion, idEntidadTerritorial).Result;
            var result = ((OkNegotiatedContentResult<List<RolNegocioDto>>)actionResult).Content;

            Assert.IsTrue(result.Count > 0);
        }

        [TestMethod]
        public void CuandoNoEnvioUsuarioObtenerSectoresPorEntidadTerritorial_RetornaExcepcion()
        {
            var responsable = "xxxxxxx";
            var nombreAplicacion = "xxxxxxx";
            var idEntidadTerritorial = Guid.Empty;

            var actionResult = _configuracionEntidadRolSectorController.ObtenerSectoresPorEntidadTerritorial(responsable, nombreAplicacion, idEntidadTerritorial).Result;
            var result = ((OkNegotiatedContentResult<List<SectorNegocioDto>>)actionResult).Content;

            Assert.IsNull(result);
        }

        [TestMethod]
        public void CuandoEnvioUsuarioObtenerSectoresPorEntidadTerritorial_RetornaResultados()
        {
            var responsable = "jdelgado";
            var nombreAplicacion = "AP:Backbone";
            var idEntidadTerritorial = Guid.NewGuid();

            var actionResult = _configuracionEntidadRolSectorController.ObtenerSectoresPorEntidadTerritorial(responsable, nombreAplicacion, idEntidadTerritorial).Result;
            var result = ((OkNegotiatedContentResult<List<SectorNegocioDto>>)actionResult).Content;

            Assert.IsTrue(result.Count > 0);
        }

        [TestMethod]
        public void CuandoNoEnvioUsuarioObtenerEntidadesPorSectorTerritorial_RetornaExcepcion()
        {
            var responsable = "xxxxxxx";
            var nombreAplicacion = "xxxxxxx";
            var idEntidadTerritorial = Guid.Empty;
            var idSector = Guid.Empty;

            var actionResult = _configuracionEntidadRolSectorController.ObtenerEntidadesPorSectorTerritorial(responsable, nombreAplicacion, idEntidadTerritorial, idSector).Result;
            var result = ((OkNegotiatedContentResult<List<EntidadNegocioDto>>)actionResult).Content;

            Assert.IsNull(result);
        }

        [TestMethod]
        public void CuandoEnvioUsuarioObtenerEntidadesPorSectorTerritorial_RetornaResultados()
        {
            var responsable = "jdelgado";
            var nombreAplicacion = "AP:Backbone";
            var idEntidadTerritorial = Guid.NewGuid();
            var idSector = Guid.NewGuid();

            var actionResult = _configuracionEntidadRolSectorController.ObtenerEntidadesPorSectorTerritorial(responsable, nombreAplicacion, idEntidadTerritorial, idSector).Result;
            var result = ((OkNegotiatedContentResult<List<EntidadNegocioDto>>)actionResult).Content;

            Assert.IsTrue(result.Count > 0);
        }

        [TestMethod]
        public void CuandoEnvioNoParametrosGuardarConfiguracionRolSector_NoRetornaResultados()
        {
            var actionResult = _configuracionEntidadRolSectorController.GuardarConfiguracionRolSector(null).Result;
            var result = ((OkNegotiatedContentResult<RespuestaGeneralDto>)actionResult).Content;

            Assert.IsNull(result);
        }

        [TestMethod]
        public void CuandoEnvioParametrosGuardarConfiguracionRolSector_RetornaResultados()
        {
            var parametros = new PeticionConfiguracionRolSectorDto();
            var actionResult = _configuracionEntidadRolSectorController.GuardarConfiguracionRolSector(parametros).Result;
            var result = ((OkNegotiatedContentResult<RespuestaGeneralDto>)actionResult).Content;

            Assert.IsNotNull(result);
            Assert.AreEqual(true, result.Exito);
            Assert.AreEqual("Exito", result.Mensaje);
        }

        [TestMethod]
        public void CuandoEnvioNoParametrosEditarConfiguracionRolSector_NoRetornaResultados()
        {
            var actionResult = _configuracionEntidadRolSectorController.EditarConfiguracionRolSector(null).Result;
            var result = ((OkNegotiatedContentResult<RespuestaGeneralDto>)actionResult).Content;

            Assert.IsNull(result);
        }

        [TestMethod]
        public void CuandoEnvioParametrosEditarConfiguracionRolSector_RetornaResultados()
        {
            var parametros = new PeticionConfiguracionRolSectorDto();
            var actionResult = _configuracionEntidadRolSectorController.EditarConfiguracionRolSector(parametros).Result;
            var result = ((OkNegotiatedContentResult<RespuestaGeneralDto>)actionResult).Content;

            Assert.IsNotNull(result);
            Assert.AreEqual(true, result.Exito);
            Assert.AreEqual("Exito", result.Mensaje);
        }

        [TestMethod]
        public void CuandoEnvioNoParametrosCambiarEstadoConfiguracionRolSector_NoRetornaResultados()
        {
            var actionResult = _configuracionEntidadRolSectorController.CambiarEstadoConfiguracionRolSector(null).Result;
            var result = ((OkNegotiatedContentResult<RespuestaGeneralDto>)actionResult).Content;

            Assert.IsNull(result);
        }

        [TestMethod]
        public void CuandoEnvioParametrosCambiarEstadoConfiguracionRolSector_RetornaResultados()
        {
            var parametros = new PeticionCambioEstadoConfiguracionDto();
            var actionResult = _configuracionEntidadRolSectorController.CambiarEstadoConfiguracionRolSector(parametros).Result;
            var result = ((OkNegotiatedContentResult<RespuestaGeneralDto>)actionResult).Content;

            Assert.IsNotNull(result);
            Assert.AreEqual(true, result.Exito);
            Assert.AreEqual("Exito", result.Mensaje);
        }
    }
}
