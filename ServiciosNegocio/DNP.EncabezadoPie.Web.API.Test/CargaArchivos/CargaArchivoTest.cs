

namespace DNP.EncabezadoPie.Web.API.Test.CargaArchivos
{
    using System.Configuration;
    using System.Security.Principal;
    using System.Threading.Tasks;
    using Mock;
    using Unity;
    using Servicios.Implementaciones;
    using Servicios.Interfaces.EncabezadoPieBasico;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Controllers;
    using DNP.EncabezadoPie.Dominio.Dto;
    using Moq;
    using DNP.ServiciosNegocio.Comunes.Autorizacion;
    using DNP.EncabezadoPie.Web.API.Test.Configuracion;
    using System.Net.Http;

    [TestClass]
    public class CargaArchivoTest : TestBase
    {
        private Mock<IEncabezadPieoBasicoServicio> _encabezadPieoBasicoServicioMock;
        private Mock<IAutorizacionUtilidades> _autorizacionUtilidades;
        private EncabezadoPieController _encabezadoPieController;

        [TestInitialize]
        public void Init()
        {
            _encabezadPieoBasicoServicioMock = new Mock<IEncabezadPieoBasicoServicio>();
            _autorizacionUtilidades = new Mock<IAutorizacionUtilidades>();
            _encabezadoPieController = new EncabezadoPieController(_encabezadPieoBasicoServicioMock.Object, _autorizacionUtilidades.Object)
            {
                User = new GenericPrincipal(new GenericIdentity("Jdelgado", "Prueba"), new[] { "managers" })
            };
            string tokenAutorizacion = "Authorization";
            string tokenAutorizacionValor = "Basic amRlbGdhZG86MjI4OTE1MDA=";
            _encabezadoPieController.Request = new System.Net.Http.HttpRequestMessage();
            _encabezadoPieController.Request.Headers.Add(tokenAutorizacion, tokenAutorizacionValor);
            _encabezadoPieController.Request.Headers.Add("piip-idAplicacion", ConfigurationManager.AppSettings["IdAppPiip"]);
        }

        protected override void Dispose(bool disposing)
        {
            if (!disposing) return;

            _encabezadoPieController.Dispose();
        }

        [TestMethod]
        public async Task ObtenerEncabezadoPieBasicoTest()
        {
            ParametrosEncabezadoPieDto parametro = new ParametrosEncabezadoPieDto();
            _autorizacionUtilidades.Setup(m => m.ValidarUsuario(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                                   .Returns(Task.FromResult(new HttpResponseMessage { StatusCode = System.Net.HttpStatusCode.OK }));
            _encabezadPieoBasicoServicioMock.Setup(m => m.ConsultarEncabezadoPieBasico(It.IsAny<ParametrosEncabezadoPieDto>()))
                                            .Returns(new EncabezadoPieBasicoDto());

            var result = await _encabezadoPieController.ObtenerEncabezadoPieBasico(parametro);
            Assert.IsNotNull(result);
        }
    }
}
