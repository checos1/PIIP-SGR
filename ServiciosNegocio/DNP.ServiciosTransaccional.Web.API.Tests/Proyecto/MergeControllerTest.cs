namespace DNP.ServiciosTransaccional.Web.API.Test.Proyecto
{
    using Controllers.Proyecto;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Servicios.Interfaces.Proyectos;
    using ServiciosNegocio.Comunes.Autorizacion;
    using ServiciosNegocio.Dominio.Dto.Transferencias;
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Security.Principal;
    using System.Threading.Tasks;
    using System.Web.Http;
    using System.Web.Http.Results;
    using Unity;

    [TestClass]
    public sealed class MergeControllerTest : IDisposable
    {
        private MergeController _mergeController;
        private IAutorizacionUtilidades AutorizacionUtilizades { get; set; }
        private IMergeServicio MergeServicio { get; set; }

        [TestInitialize]
        public void Init()
        {
            var contenedor = Configuracion.UnityConfig.Container;
            MergeServicio = contenedor.Resolve<IMergeServicio>();
            AutorizacionUtilizades = contenedor.Resolve<IAutorizacionUtilidades>();

            _mergeController =
                new MergeController(MergeServicio, AutorizacionUtilizades)
                {
                    ControllerContext =
                    {
                        Request= new HttpRequestMessage()
                    }
                };
        }

        [TestMethod]
        public async Task ActualizarEstadoProyectoTest_Valido()
        {
            _mergeController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _mergeController.User = new GenericPrincipal(new GenericIdentity("jcastano@dnp.gov.co", "Jocas123*"), new[] { "" });

            var resultado = (OkNegotiatedContentResult<HttpResponseMessage>)await _mergeController.AplicarMerge(new ObjetoNegocio
            {
                ObjetoNegocioId = "240"
            });
            Assert.AreEqual(HttpStatusCode.OK, resultado.Content.StatusCode);
        }

        [TestMethod]
        public async Task ActualizarEstadoProyectoTest_FalloAutorizacion()
        {
            try
            {
                _mergeController.ControllerContext.Request.Headers.Authorization =
                    new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");

                await _mergeController.AplicarMerge(new ObjetoNegocio());
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex.GetType() == typeof(InvalidCastException) || ex.GetType() == typeof(HttpResponseException));
            }
        }

        private void Dispose(bool disposing)
        {
            if (disposing) _mergeController.Dispose();
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
}
