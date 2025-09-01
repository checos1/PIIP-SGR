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
using DNP.ServiciosNegocio.Comunes.Dto.Formulario;

namespace DNP.ServiciosNegocio.Web.API.Test.Negocio
{
    [TestClass]
    public sealed class DatosAdicionalesControllerTest : IDisposable
    {
        private IDatosAdicionalesServicio DatosAdicionalesServicios { get; set; }

        private IAutorizacionUtilidades AutorizacionUtilizades { get; set; }

        private DatosAdicionalesController _DatosAdicionalesController;

        private string Bpin { get; set; }

        [TestInitialize]
        public void Init()
        {
            //Bpin = "2017011000042";
            //var contenedor = Configuracion.UnityConfig.Container;
            //DatosAdicionalesServicios = contenedor.Resolve<IDatosAdicionalesServicio>();
            //AutorizacionUtilizades = contenedor.Resolve<IAutorizacionUtilidades>();

            //_DatosAdicionalesController =
            //    new DatosAdicionalesController(DatosAdicionalesServicios, AutorizacionUtilizades)
            //    {
            //        ControllerContext
            //            =
            //            {
            //                Request
            //                    = new
            //                        HttpRequestMessage()
            //            }
            //    };

            //_DatosAdicionalesController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            //_DatosAdicionalesController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:Backbone");
            //_DatosAdicionalesController.ControllerContext.Request.Headers.Add("piip-idAccion", "E3C1849C-FE24-4C07-9762-036FA72AF10C");
            //_DatosAdicionalesController.ControllerContext.Request.Headers.Add("piip-idInstanciaFlujo", "4C2E62CD-CEAD-48EF-88C6-A50AB5913716");
            //_DatosAdicionalesController.ControllerContext.Request.Headers.Add("piip-idFormulario", "4C2E62CD-CEAD-48EF-88C6-A50AB5913716");
            //_DatosAdicionalesController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });
        }

        [TestMethod]
        public async Task ObtenerDatosAdicionalesFuenteFinanciacion(int fuenteId)
        {
            var resultados = (OkNegotiatedContentResult<DatosAdicionalesDto>)await _DatosAdicionalesController.ObtenerDatosAdicionalesFuenteFinanciacion(fuenteId);
            Assert.IsNotNull(resultados.Content);
        }

        [TestMethod]
        public async Task GuardarDatosAdicionales(DatosAdicionalesDto parametrosGuardar) {
            var resultados = (OkNegotiatedContentResult<DatosAdicionalesDto>)await _DatosAdicionalesController.Definitivo(parametrosGuardar);
            Assert.IsNotNull(resultados.Content);
        }

        [TestMethod]
        public async Task EliminarDatosAdicionales(int coFinanciacionId)
        {
            var resultados = (OkNegotiatedContentResult<DatosAdicionalesDto>)await _DatosAdicionalesController.Eliminar(coFinanciacionId);
            Assert.IsNotNull(resultados.Content);
        }


        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                // dispose managed resources
                _DatosAdicionalesController.Dispose();
            }
            // free native resources
        }

        public void Dispose()
        {
            Dispose(true);
        }

    }
}
