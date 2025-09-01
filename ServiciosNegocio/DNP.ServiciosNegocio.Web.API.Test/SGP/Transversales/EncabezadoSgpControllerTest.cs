using DNP.ServiciosNegocio.Servicios.Interfaces.Priorizacion;
using DNP.ServiciosNegocio.Web.API.Controllers.Priorizacion;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Unity;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web.Http.Results;
using DNP.ServiciosNegocio.Comunes.Dto.Viabilidad;
using DNP.ServiciosNegocio.Comunes.Autorizacion;
using DNP.ServiciosNegocio.Servicios.Interfaces.SGP.Transversales;
using DNP.ServiciosNegocio.Web.API.Controllers.SGP.Transversales;
using DNP.ServiciosNegocio.Dominio.Dto.SGP.Transversales;

namespace DNP.ServiciosNegocio.Web.API.Test.Priorizacion
{
    [TestClass]
    public class EncabezadoSgpControllerTest : IDisposable
    {
        private ITransversalServicioSGP TransversalServicioSGP { get; set; }
        private IAutorizacionUtilidades AutorizacionUtilidades { get; set; }
        private EncabezadoSgpController _encabezadoSgpController;

        [TestInitialize]
        public void Init()
        {
            var contenedor = Configuracion.UnityConfig.Container;
            TransversalServicioSGP = contenedor.Resolve<ITransversalServicioSGP>();
            AutorizacionUtilidades = contenedor.Resolve<IAutorizacionUtilidades>();
            _encabezadoSgpController = new EncabezadoSgpController(AutorizacionUtilidades, TransversalServicioSGP);
            _encabezadoSgpController.ControllerContext.Request = new HttpRequestMessage();
            _encabezadoSgpController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _encabezadoSgpController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:Backbone");
            _encabezadoSgpController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });
        }

        [TestMethod]
        public async Task ObtenerEncabezadoSGP_NoNulo()
        {
            Guid idInstancia = new Guid("d5813141-94b4-445e-ac39-23186cd9b25b");
            Guid idFlujo = new Guid("5e5b4600-caf9-46ab-a404-73f0758b47db");
            Guid idNivel = new Guid("050B63FB-83DB-4160-B358-4A4061B79B15");
            int idProyecto = 617399;
            string tramite = "";

            ParametrosEncabezadoSGP objEncabezado = new ParametrosEncabezadoSGP();
            objEncabezado.IdInstancia = idInstancia;
            objEncabezado.IdFlujo = idFlujo;
            objEncabezado.IdNivel = idNivel;
            objEncabezado.IdProyecto = idProyecto;
            objEncabezado.Tramite = tramite;

            var result = (OkNegotiatedContentResult<EncabezadoSGPDto>)await _encabezadoSgpController.ObtenerEncabezadoSGP(objEncabezado);
            Assert.IsNotNull(result.Content);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                // dispose managed resources
                _encabezadoSgpController.Dispose();
            }
            // free native resources
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
}