namespace DNP.ServiciosNegocio.Web.API.Test.SGP.Tramite
{
    using DNP.ServiciosNegocio.Dominio.Dto.Proyectos;
    using DNP.ServiciosNegocio.Dominio.Dto.Tramites;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Unity;
    using System.Threading.Tasks;
    using System.Web.Http.Results;
    using System;
    using DNP.ServiciosNegocio.Comunes.Autorizacion;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Security.Principal;
    using DNP.ServiciosNegocio.Web.API.Controllers.SGP.Tramite;
    using DNP.ServiciosNegocio.Servicios.Interfaces.SGP.Tramite;

    [TestClass]
    public sealed class TramiteProyectoSGPControllerTest : IDisposable
    {
        private ITramiteProyectoSGPServicio tramiteProyectoSGPServicio { get; set; }
        private IAutorizacionUtilidades AutorizacionUtilizades { get; set; }

        private TramiteProyectoSGPController _tramiteProyectoSGPController;

        [TestInitialize]
        public void Init()
        {
            var contenedor = Configuracion.UnityConfig.Container;
            tramiteProyectoSGPServicio = contenedor.Resolve<ITramiteProyectoSGPServicio>();
            AutorizacionUtilizades = contenedor.Resolve<IAutorizacionUtilidades>();

            _tramiteProyectoSGPController =
                new TramiteProyectoSGPController(tramiteProyectoSGPServicio, AutorizacionUtilizades)
                {
                    ControllerContext
                    =
                    {
                            Request
                        = new
                        HttpRequestMessage()
                    }
                };

            _tramiteProyectoSGPController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _tramiteProyectoSGPController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:PIIP");
            _tramiteProyectoSGPController.ControllerContext.Request.Headers.Add("piip-idAccion", "D2BA19EB-0487-4C94-8960-3A6047B81409");
            _tramiteProyectoSGPController.ControllerContext.Request.Headers.Add("piip-idInstanciaFlujo", "BC135467-5041-4C0F-8AB7-EC2F09E02AAF");
            _tramiteProyectoSGPController.ControllerContext.Request.Headers.Add("piip-idFormulario", "ADA6BAAC-F2DD-4664-AAB7-9E8AB61405CC");
            _tramiteProyectoSGPController.User = new GenericPrincipal(new GenericIdentity("jcastano@dnp.gov.co", "Jocas123*"), new[] { "" });
        }

        [TestMethod]
        public async Task GuardarProyectosTramiteNegocio_Ok()
        {
            _tramiteProyectoSGPController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _tramiteProyectoSGPController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:PIIP");
            _tramiteProyectoSGPController.ControllerContext.Request.Headers.Add("piip-idAccion", "D2BA19EB-0487-4C94-8960-3A6047B81409");
            _tramiteProyectoSGPController.ControllerContext.Request.Headers.Add("piip-idInstanciaFlujo", "BC135467-5041-4C0F-8AB7-EC2F09E02AAF");

            _tramiteProyectoSGPController.User = new GenericPrincipal(new GenericIdentity("jcastano@dnp.gov.co", "Jocas123*"), new[] { "" });

            DatosTramiteProyectosDto DatosTramiteProyectosDto = new DatosTramiteProyectosDto();
            DatosTramiteProyectosDto.TramiteId = 1;
            var resultados = (OkNegotiatedContentResult<TramitesResultado>)await _tramiteProyectoSGPController.GuardarProyectosTramiteNegocio(DatosTramiteProyectosDto);
            Assert.AreEqual(true, resultados.Content.Exito);
        }

        [TestMethod]
        public async Task GuardarProyectosTramiteNegocio_MensajeError()
        {
            _tramiteProyectoSGPController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _tramiteProyectoSGPController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:PIIP");
            _tramiteProyectoSGPController.ControllerContext.Request.Headers.Add("piip-idAccion", "D2BA19EB-0487-4C94-8960-3A6047B81409");
            _tramiteProyectoSGPController.ControllerContext.Request.Headers.Add("piip-idInstanciaFlujo", "BC135467-5041-4C0F-8AB7-EC2F09E02AAF");

            _tramiteProyectoSGPController.User = new GenericPrincipal(new GenericIdentity("jcastano@dnp.gov.co", "Jocas123*"), new[] { "" });

            DatosTramiteProyectosDto DatosTramiteProyectosDto = new DatosTramiteProyectosDto();
            var resultados = (OkNegotiatedContentResult<TramitesResultado>)await _tramiteProyectoSGPController.GuardarProyectosTramiteNegocio(DatosTramiteProyectosDto);
            Assert.IsNotNull(resultados.Content.Mensaje);
        }

        public void Dispose()
        {
            Dispose(true);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                _tramiteProyectoSGPController.Dispose();
            }
        }
    }
}
