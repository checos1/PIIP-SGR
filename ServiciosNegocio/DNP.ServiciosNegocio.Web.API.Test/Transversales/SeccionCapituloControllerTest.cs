using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using DNP.ServiciosNegocio.Web.API.Controllers.Transversales;
using DNP.ServiciosNegocio.Servicios.Interfaces.Transversales;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DNP.ServiciosNegocio.Dominio.Dto.Transversales;
using System.Web.Http.Results;
using System.Net.Http.Headers;
using System.Net.Http;
using DNP.ServiciosNegocio.Comunes.Autorizacion;
using Unity;

namespace DNP.ServiciosNegocio.Web.API.Test.Transversales
{
    [TestClass]
    public sealed class SeccionCapituloPControllerTest : IDisposable
    {
        private ISeccionCapituloServicio _seccionCapituloServicio { get; set; }
        private IAutorizacionUtilidades _autorizacionUtilidades { get; set; }

        private SeccionCapituloController _SeccionCapituloController;
        [TestInitialize]
        public void Init()
        {
            var contenedor = Configuracion.UnityConfig.Container;

            _seccionCapituloServicio = contenedor.Resolve<ISeccionCapituloServicio>();
            _autorizacionUtilidades = contenedor.Resolve<IAutorizacionUtilidades>();

            _SeccionCapituloController =
                new SeccionCapituloController(_autorizacionUtilidades, _seccionCapituloServicio)
                {
                    ControllerContext
                        =
                        {
                            Request
                                = new
                                    HttpRequestMessage()
                        }
                };

            _SeccionCapituloController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _SeccionCapituloController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:PIIP");
            _SeccionCapituloController.ControllerContext.Request.Headers.Add("piip-idAccion", "D2BA19EB-0487-4C94-8960-3A6047B81409");
            _SeccionCapituloController.ControllerContext.Request.Headers.Add("piip-idInstanciaFlujo", "BC135467-5041-4C0F-8AB7-EC2F09E02AAF");
            _SeccionCapituloController.ControllerContext.Request.Headers.Add("piip-idFormulario", "ADA6BAAC-F2DD-4664-AAB7-9E8AB61405CC");
            _SeccionCapituloController.User = new GenericPrincipal(new GenericIdentity("jcastano@dnp.gov.co", "Jocas123*"), new[] { "" });
        }
        [TestMethod]
        public async Task EliminarCapitulosModificados_Ok()
        {
            _SeccionCapituloController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _SeccionCapituloController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:PIIP");
            _SeccionCapituloController.ControllerContext.Request.Headers.Add("piip-idAccion", "D2BA19EB-0487-4C94-8960-3A6047B81409");
            _SeccionCapituloController.ControllerContext.Request.Headers.Add("piip-idInstanciaFlujo", "BC135467-5041-4C0F-8AB7-EC2F09E02AAF");

            _SeccionCapituloController.User = new GenericPrincipal(new GenericIdentity("jcastano@dnp.gov.co", "Jocas123*"), new[] { "" });

            CapituloModificado CapituloModificadoDto = new CapituloModificado();
            CapituloModificadoDto.InstanciaId = new Guid("cc912a51-10f9-4b3f-a81a-00b94a8b913d");
            CapituloModificadoDto.SeccionCapituloId = 4;
            CapituloModificadoDto.ProyectoId = 0;

            var resultados = (OkNegotiatedContentResult<SeccionesCapitulos>)await _SeccionCapituloController.EliminarCapitulosModificados(CapituloModificadoDto);
            Assert.AreEqual(true, resultados.Content.Exito);
        }

        [TestMethod]
        public async Task EliminarCapitulosModificados_MensajeError()
        {
            _SeccionCapituloController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _SeccionCapituloController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:PIIP");
            _SeccionCapituloController.ControllerContext.Request.Headers.Add("piip-idAccion", "D2BA19EB-0487-4C94-8960-3A6047B81409");
            _SeccionCapituloController.ControllerContext.Request.Headers.Add("piip-idInstanciaFlujo", "BC135467-5041-4C0F-8AB7-EC2F09E02AAF");


            _SeccionCapituloController.User = new GenericPrincipal(new GenericIdentity("jcastano@dnp.gov.co", "Jocas123*"), new[] { "" });


            CapituloModificado CapituloModificadoDto = new CapituloModificado();


            var resultados = (OkNegotiatedContentResult<SeccionesCapitulos>)await _SeccionCapituloController.EliminarCapitulosModificados(CapituloModificadoDto);
            Assert.IsNotNull(resultados.Content.Mensaje);

        }

        [TestMethod]
        public async Task ObtenerErroresPrograma_Test()
        {
            string instanciaId = "C05B2241-8F33-4F86-898A-F12CCA74973E";
            string accionId = "3AEE26E2-E4BD-E748-3567-92B3F50C98E1";
            var resultados = await _SeccionCapituloController.ObtenerErroresProgramacion(instanciaId, accionId);
            Assert.IsNotNull(resultados);
        }





        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                // dispose managed resources
                _SeccionCapituloController.Dispose();
            }
            // free native resources
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
}
