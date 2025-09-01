namespace DNP.Backbone.Web.API.Test.WebAPI
{
    using DNP.Backbone.Dominio.Dto.AutorizacionNegocio;
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
    public class AutorizacionControllerTest
    {
        private IAutorizacionServicios _autorizacionServicios;
        private EntidadController _entidadController;

        [TestInitialize]
        public void Init()
        {
            this._autorizacionServicios = Config.UnityConfig.Container.Resolve<IAutorizacionServicios>();
            this._entidadController = new EntidadController(this._autorizacionServicios);
            this._entidadController.ControllerContext.Request = new HttpRequestMessage();
            this._entidadController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            this._entidadController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:Backbone");
            this._entidadController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });

        }

        [TestMethod]
        public void ObtenerEntidadesPorUnidadesResponsables_Ok()
        {
            var actionResult = this._autorizacionServicios.ObtenerEntidadesPorUnidadesResponsables("");

            Assert.IsNotNull(actionResult.Result);
        }

        [TestMethod]
        public void ObtenerSectoresParaEntidades_Ok()
        {
            var actionResult = this._autorizacionServicios.ObtenerSectoresParaEntidades("");

            Assert.IsNotNull(actionResult.Result);
        }

        [TestMethod]
        public void ActualizarUnidadResponsable_Ok()
        {
            var actionResult = this._autorizacionServicios.ActualizarUnidadResponsable(new EntidadNegocioDto(), "jdelgado");

            Assert.IsNotNull(actionResult.Result);
        }

        [TestMethod]
        public void ObtenerEncabezadoListadoReportesPIIP()
        {
            string usuarioDnp = "jdelgado";
            var actionResult = this._autorizacionServicios.ObtenerEncabezadoListadoReportesPIIP(usuarioDnp);
            Assert.IsNotNull(actionResult.Result);
        }

        [TestMethod]
        public void ObtenerListadoReportesPIIP()
        {
            //List<Guid> idRoles = new List<Guid>();
            //Guid idrol = new Guid("256A1BF8-7FAE-46DE-8549-0A3D528AD9D0");
            string idrol = "256A1BF8-7FAE-46DE-8549-0A3D528AD9D0";
            //idRoles.Add(idrol);
            var actionResult = this._autorizacionServicios.ObtenerListadoReportesPIIP("jdelgado", idrol);
            Assert.IsNotNull(actionResult.Result);
        }

        [TestMethod]
        public void ObtenerFiltrosReportesPIIP()
        {
            string usuarioDnp = "jdelgado";
            Guid idReporte = new Guid();
            var actionResult = this._autorizacionServicios.ObtenerFiltrosReportesPIIP(idReporte, usuarioDnp);
            Assert.IsNotNull(actionResult.Result);
        }

        [TestMethod]
        public void ObtenerDatosReportePIIP()
        {
            string usuarioDnp = "cc2023202303";
            string filtros = String.Empty;
            string procedimiento = String.Empty;
            Guid idReporte = new Guid();
            string idEntidades = "";
            var actionResult = this._autorizacionServicios.ObtenerDatosReportePIIP(idReporte, filtros, usuarioDnp, idEntidades);
            Assert.IsNotNull(actionResult.Result);
        }
    }
}
