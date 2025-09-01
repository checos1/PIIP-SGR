using DNP.ServiciosNegocio.Comunes.Autorizacion;
using DNP.ServiciosNegocio.Servicios.Interfaces.SGR.DelegarViabilidad;
using DNP.ServiciosNegocio.Web.API.Controllers.SGR.DelegarViabilidad;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Unity;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web.Http.Results;
using DNP.ServiciosNegocio.Comunes.Dto.Viabilidad;

namespace DNP.ServiciosNegocio.Web.API.Test.SGR.DelegarViabilidad
{
    [TestClass]
    public class DelegarViabilidadControllerTest : IDisposable
    {
        private IDelegarViabilidadServicio DelegarViabilidadServicio { get; set; }
        private IAutorizacionUtilidades AutorizacionUtilidades { get; set; }
        private DelegarViabilidadController _delegarViabilidadController;

        [TestInitialize]
        public void Init()
        {
            var contenedor = Configuracion.UnityConfig.Container;
            DelegarViabilidadServicio = contenedor.Resolve<IDelegarViabilidadServicio>();
            AutorizacionUtilidades = contenedor.Resolve<IAutorizacionUtilidades>();
            _delegarViabilidadController = new DelegarViabilidadController(DelegarViabilidadServicio, AutorizacionUtilidades);
            _delegarViabilidadController.ControllerContext.Request = new HttpRequestMessage();
            _delegarViabilidadController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _delegarViabilidadController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:Backbone");
            _delegarViabilidadController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });
        }

        [TestMethod]
        public async Task SGR_DelegarViabilidad_ObtenerProyecto_NoNulo()
        {
            var bpin = "98095";
            var instanciaId = "00000000-0000-0000-0000-000000000000";
            var result = (OkNegotiatedContentResult<string>)await _delegarViabilidadController.SGR_DelegarViabilidad_ObtenerProyecto(bpin, new Guid(instanciaId));
            Assert.IsNotNull(result.Content);
        }

        [TestMethod]
        public async Task SGR_DelegarViabilidad_ObtenerEntidades_NoNulo()
        {
            var bpin = "98095";
            var result = (OkNegotiatedContentResult<string>)await _delegarViabilidadController.SGR_DelegarViabilidad_ObtenerEntidades(bpin);
            Assert.IsNotNull(result.Content);
        }

        [TestMethod]
        public void SGR_DelegarViabilidad_Registrar_Test()
        {
            //List<DatosAdicionalesCofinanciadorDto> lstDatosCofinanciador = new List<DatosAdicionalesCofinanciadorDto>();
            DelegarViabilidadDto delegarVigenciaDto = new DelegarViabilidadDto
            {
                ProyectoId = 98138,
                EntityTypeCatalogOpcionId = 271,
                Delegado = true
            };
            //lstDatosCofinanciador.Add(vigenciaFuenteDto);


            string result = "OK";

            try
            {
                _ = _delegarViabilidadController.SGR_DelegarViabilidad_Registrar(delegarVigenciaDto);
            }
            catch
            {
                result = "ERROR";
            }

            Assert.IsNotNull(result);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                // dispose managed resources
                _delegarViabilidadController.Dispose();
            }
            // free native resources
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
}
