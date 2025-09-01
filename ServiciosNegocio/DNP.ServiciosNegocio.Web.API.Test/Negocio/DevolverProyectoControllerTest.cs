using System.Threading.Tasks;
using System.Web.Http.Results;
using DNP.ServiciosNegocio.Comunes.Autorizacion;
using DNP.ServiciosNegocio.Comunes.Excepciones;
using DNP.ServiciosNegocio.Dominio.Dto.Proyectos;
using DNP.ServiciosNegocio.Servicios.Interfaces.Proyectos;
using DNP.ServiciosNegocio.Web.API.Controllers.Negocio;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Unity;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Principal;

namespace DNP.ServiciosNegocio.Web.API.Test.Negocio
{

    using System.Collections.Generic;
    using System.Net;
    using Comunes;
    using DNP.ServiciosNegocio.Comunes.Dto.ObjetosNegocio;
    using ServiciosNegocio.Dominio.Dto.Transferencias;

    [TestClass]
    public class DevolverProyectoControllerTest : IDisposable
    {
        private IDevolverProyectoServicio DevolverProyectoServicio { get; set; }
        private IAutorizacionUtilidades AutorizacionUtilizades { get; set; }
        private DevolverProyectoController _devolverProyectoController;
        private string Bpin { get; set; }


        [TestInitialize]
        public void Init()
        {
            Bpin = "2017011000042";
            var contenedor = Configuracion.UnityConfig.Container;
            DevolverProyectoServicio = contenedor.Resolve<IDevolverProyectoServicio>();
            AutorizacionUtilizades = contenedor.Resolve<IAutorizacionUtilidades>();

            _devolverProyectoController =
                new DevolverProyectoController(DevolverProyectoServicio, AutorizacionUtilizades)
                {
                    ControllerContext
                        =
                        {
                            Request
                                = new
                                    HttpRequestMessage()
                        }
                };

            _devolverProyectoController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _devolverProyectoController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:Backbone");
            _devolverProyectoController.ControllerContext.Request.Headers.Add("piip-idAccion", "E3C1849C-FE24-4C07-9762-036FA72AF10C");
            _devolverProyectoController.ControllerContext.Request.Headers.Add("piip-idInstanciaFlujo", "4C2E62CD-CEAD-48EF-88C6-A50AB5913716");
            _devolverProyectoController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });
        }


        [TestMethod]
        public async Task DevolverProyectoBpinValido()
        {
            var resultados = (OkNegotiatedContentResult<DevolverProyectoDto>)await _devolverProyectoController.Consultar(Bpin);
            Assert.IsNotNull(resultados.Content);
        }



        [TestMethod]
        public async Task DevolverProyectoVacio()
        {
            var resultados = await _devolverProyectoController.Consultar("123456");
            Assert.IsNull(((ResponseMessageResult)resultados).Response.Content);
        }


        [TestMethod]
        public async Task DevolverProyectoGuardarDefinitivo_OK()
        {
            _devolverProyectoController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _devolverProyectoController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:Backbone");
            _devolverProyectoController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });

            _devolverProyectoController.ControllerContext.Request.Headers.Add("piip-idAccion", "E3C1849C-FE24-4C07-9762-036FA72AF10C");
            _devolverProyectoController.ControllerContext.Request.Headers.Add("piip-idInstanciaFlujo", "4C2E62CD-CEAD-48EF-88C6-A50AB5913716");

            var devolverProyectoDto = new DevolverProyectoDto();
            var result = (OkNegotiatedContentResult<HttpResponseMessage>)await _devolverProyectoController.Definitivo(devolverProyectoDto);
            Assert.AreEqual(HttpStatusCode.OK, result.Content.StatusCode);
        }


        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                // dispose managed resources
                _devolverProyectoController.Dispose();
            }
            // free native resources
        }

        public void Dispose()
        {
            Dispose(true);
        }

    }




}
