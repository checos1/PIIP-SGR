using DNP.CargaArchivos.Web.API.Controllers.CargaArchivo;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DNP.CargaArchivos.Web.API.Test.CargaArchivos
{
    using System.Configuration;
    using System.Security.Principal;
    using System.Threading.Tasks;
    using DNP.ServiciosNegocio.Comunes.Excepciones;
    using Mock;
    using Servicios.Implementaciones;

    [TestClass]
    public class CargaArchivoTest
    {
        private CargaArchivoController _cargaArchivoController;


        [TestInitialize]
        public void Init()
        {
            _cargaArchivoController = new CargaArchivoController(new CargaArchivo(), new AutorizacionUtilidadesMock())
            {
                User = new GenericPrincipal(new GenericIdentity("Jdelgado", "Prueba"), new[] { "managers" })
            };
            string tokenAutorizacion = "Authorization";
            string tokenAutorizacionValor = "Basic amRlbGdhZG86MjI4OTE1MDA=";
            _cargaArchivoController.Request = new System.Net.Http.HttpRequestMessage();
            _cargaArchivoController.Request.Headers.Add(tokenAutorizacion, tokenAutorizacionValor);
            _cargaArchivoController.Request.Headers.Add("piip-idAplicacion", ConfigurationManager.AppSettings["IdAppPiip"]);
        }

        [TestMethod]
        public async Task ConsultarTest()
        {

            var result = await _cargaArchivoController.Consultar();
            Assert.IsNotNull(result);
        }
    }
}
