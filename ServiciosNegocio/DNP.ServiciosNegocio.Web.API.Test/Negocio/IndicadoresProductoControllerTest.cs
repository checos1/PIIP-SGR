using DNP.ServiciosNegocio.Servicios.Interfaces.FuenteFinanciacion;
using DNP.ServiciosNegocio.Web.API.Controllers.Negocio;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Unity;
using System.Threading.Tasks;
using System.Web.Http.Results;
using DNP.ServiciosNegocio.Dominio.Dto.FuenteFinanciacion;
using System;
using DNP.ServiciosNegocio.Comunes.Autorizacion;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Principal;
using System.Net;
using DNP.ServiciosNegocio.Servicios.Interfaces.CadenaValor;
using DNP.ServiciosNegocio.Dominio.Dto.CadenaValor;
using System.Collections.Generic;

namespace DNP.ServiciosNegocio.Web.API.Test.Negocio
{
    [TestClass]
    public sealed class IndicadoresProductoControllerTest : IDisposable
    {
        private IIndicadoresProductoServicio IndicadoresProductoServicio { get; set; }
        private IAutorizacionUtilidades AutorizacionUtilizades { get; set; }
        private IndicadoresProductoController _indicadoresProductoController;
        private string Bpin { get; set; }

        [TestInitialize]
        public void Init()
        {
            Bpin = "202200000000037";
            var contenedor = Configuracion.UnityConfig.Container;
            IndicadoresProductoServicio = contenedor.Resolve<IIndicadoresProductoServicio>();
            AutorizacionUtilizades = contenedor.Resolve<IAutorizacionUtilidades>();

            _indicadoresProductoController =
                new IndicadoresProductoController(IndicadoresProductoServicio, AutorizacionUtilizades)
                {
                    ControllerContext
                        =
                        {
                            Request
                                = new
                                    HttpRequestMessage()
                        }
                };

            _indicadoresProductoController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _indicadoresProductoController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:PIIP");
            _indicadoresProductoController.ControllerContext.Request.Headers.Add("piip-idAccion", "D2BA19EB-0487-4C94-8960-3A6047B81409");
            _indicadoresProductoController.ControllerContext.Request.Headers.Add("piip-idInstanciaFlujo", "BC135467-5041-4C0F-8AB7-EC2F09E02AAF");
            _indicadoresProductoController.ControllerContext.Request.Headers.Add("piip-idFormulario", "ADA6BAAC-F2DD-4664-AAB7-9E8AB61405CC");
            _indicadoresProductoController.User = new GenericPrincipal(new GenericIdentity("jcastano@dnp.gov.co", "Jocas123*"), new[] { "" });
        }

        [TestMethod]
        public async Task IndicadoresValidarCapituloModificado()
        {
            var resultados = (OkNegotiatedContentResult<List<IndicadorCapituloModificadoDto>>)await _indicadoresProductoController.IndicadoresValidarCapituloModificado(Bpin);
            Assert.IsNotNull(resultados.Content);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                // dispose managed resources
                _indicadoresProductoController.Dispose();
            }
            // free native resources
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
}
