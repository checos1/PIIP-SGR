using DNP.ServiciosNegocio.Comunes.Autorizacion;
using DNP.ServiciosNegocio.Servicios.Interfaces.SGP.GestionRecursos;
using DNP.ServiciosNegocio.Web.API.Controllers.SGP.GestionRecursos;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Unity;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web.Http.Results;
using DNP.ServiciosNegocio.Comunes.Dto.Viabilidad;
using DNP.ServiciosNegocio.Dominio.Dto.Programacion;
using DNP.ServiciosNegocio.Comunes.Dto.Formulario;
using DNP.ServiciosNegocio.Dominio.Dto.Focalizacion;

namespace DNP.ServiciosNegocio.Web.API.Test.SGP.GestionRecursos
{
    [TestClass]
    public class GestionRecursosSgpControllerTest : IDisposable
    {
        private IGestionRecursosSgpServicio GestionRecursosSgpServicio { get; set; }
        private IAutorizacionUtilidades AutorizacionUtilidades { get; set; }
        private GestionRecursosSgpController _gestionRecursosSgpController;


        [TestInitialize]
        public void Init()
        {
            var contenedor = Configuracion.UnityConfig.Container;
            GestionRecursosSgpServicio = contenedor.Resolve<IGestionRecursosSgpServicio>();
            AutorizacionUtilidades = contenedor.Resolve<IAutorizacionUtilidades>();
            AutorizacionUtilidades = contenedor.Resolve<IAutorizacionUtilidades>();
            _gestionRecursosSgpController = new GestionRecursosSgpController(GestionRecursosSgpServicio, AutorizacionUtilidades);
            _gestionRecursosSgpController.ControllerContext.Request = new HttpRequestMessage();
            _gestionRecursosSgpController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _gestionRecursosSgpController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:Backbone");
            _gestionRecursosSgpController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });

        }

        [TestMethod]
        public async Task ObtenerLocalizacionProyectosSgp_NoNulo()
        {
            var bpin = "2017011000096";
            var result = (OkNegotiatedContentResult<string>)await _gestionRecursosSgpController.LocalizacionProyectoSgp(bpin);
            Assert.IsNotNull(result.Content);
        }

        [TestMethod]
        public async Task ObtenerFocalizacionPoliticasTransversalesFuentesSgp_NoNulo()
        {
            var bpin = "2017011000096";
            var result = (OkNegotiatedContentResult<string>)await _gestionRecursosSgpController.ObtenerFocalizacionPoliticasTransversalesFuentesSgp(bpin);
            Assert.IsNotNull(result.Content);
        }

        [TestMethod]
        public async Task ObtenerPoliticasTransversalesProyectoSgp_NoNulo()
        {
            var bpin = "2017011000096";
            var result = (OkNegotiatedContentResult<string>)await _gestionRecursosSgpController.ObtenerPoliticasTransversalesProyectoSgp(bpin);
            Assert.IsNotNull(result.Content);
        }

        [TestMethod]
        public void EliminarPoliticasProyectoSgp_NoNulo()
        {
            
            string result = "OK";

            try
            {
                _ = _gestionRecursosSgpController.EliminarPoliticasProyectoSgp(1,1);
            }
            catch
            {
                result = "ERROR";
            }
            Assert.IsNotNull(result);
        }
        [TestMethod]
        public async Task ConsultarPoliticasCategoriasIndicadoresSgp_NoNulo()
        {
            var instanciaId = "00000000-0000-0000-0000-000000000000";
            var result = (OkNegotiatedContentResult<string>)await _gestionRecursosSgpController.ConsultarPoliticasCategoriasIndicadoresSgp(new Guid(instanciaId));
            Assert.IsNotNull(result.Content);
        }

        [TestMethod]
        public async Task ObtenerPoliticasTransversalesCategoriasSgp_NoNulo()
        {
            string instanciaId = "00000000-0000-0000-0000-000000000000";
            var result = (OkNegotiatedContentResult<string>)await _gestionRecursosSgpController.ObtenerPoliticasTransversalesCategoriasSgp(instanciaId);
            Assert.IsNotNull(result.Content);
        }
        [TestMethod]
        public void EliminarCategoriasPoliticasProyectoSgp_NoNulo()
        {

            string result = "OK";

            try
            {
                _ = _gestionRecursosSgpController.EliminarCategoriasPoliticasProyectoSgp(1, 1,1);
            }
            catch
            {
                result = "ERROR";
            }
            Assert.IsNotNull(result);
        }
        [TestMethod]
        public async Task GetCategoriasSubcategoriasSgp_NoNulo()
        {
           
            var result = (OkNegotiatedContentResult<string>)await _gestionRecursosSgpController.GetCategoriasSubcategoriasSgp(1,1,1,1);
            Assert.IsNotNull(result.Content);
        }
        [TestMethod]
        public async Task ObtenerCrucePoliticasAjustesSgp_NoNulo()
        {
            var instanciaId = "00000000-0000-0000-0000-000000000000";
            var result = (OkNegotiatedContentResult<string>)await _gestionRecursosSgpController.ObtenerCrucePoliticasAjustesSgp(new Guid(instanciaId));
            Assert.IsNotNull(result.Content);
        }
        [TestMethod]
        public async Task ObtenerPoliticasTransversalesResumenSgp_NoNulo()
        {
            var instanciaId = "00000000-0000-0000-0000-000000000000";
            var result = (OkNegotiatedContentResult<string>)await _gestionRecursosSgpController.ObtenerPoliticasTransversalesResumenSgp(new Guid(instanciaId));
            Assert.IsNotNull(result.Content);
        }

        [TestMethod]
        public async Task ObtenerDesagregarRegionalizacionSgp_NoNulo()
        {
            var bpin = "2017011000096";
            var result = (OkNegotiatedContentResult<string>)await _gestionRecursosSgpController.ObtenerDesagregarRegionalizacionSgp(bpin);
            Assert.IsNotNull(result.Content);
        }

        [TestMethod]
        public async Task ObtenerFuenteFinanciacionVigenciaSgp_NoNulo()
        {
            var bpin = "2017011000096";
            var result = (OkNegotiatedContentResult<string>)await _gestionRecursosSgpController.ConsultarFuenteFinanciacionVigenciaSgp(bpin);
            Assert.IsNotNull(result.Content);
        }
        [TestMethod]
        public async Task ConsultarFuentesProgramarSolicitadoSgp_NoNulo()
        {
            var bpin = "2017011000096";
            var result = (OkNegotiatedContentResult<string>)await _gestionRecursosSgpController.ConsultarFuentesProgramarSolicitadoSgp(bpin);
            Assert.IsNotNull(result.Content);
        }
        [TestMethod]
        public void EliminarFuentesFinanciacionProyectoSGP_NoNulo()
        {

            string result = "OK";

            try
            {
                _ = _gestionRecursosSgpController.EliminarFuentesFinanciacionProyectoSGP(1);
            }
            catch
            {
                result = "ERROR";
            }
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task ObtenerDatosAdicionalesFuenteFinanciacionSgp_NoNulo()
        {
            int fuenteId = 12;
            var result = (OkNegotiatedContentResult<string>)await _gestionRecursosSgpController.ObtenerDatosAdicionalesFuenteFinanciacionSgp(fuenteId);
            Assert.IsNotNull(result.Content);
        }
        [TestMethod]
        public void EliminarDatosAdicionalesSgp_NoNulo()
        {

            string result = "OK";

            try
            {
                _ = _gestionRecursosSgpController.EliminarDatosAdicionalesSgp(1);
            }
            catch
            {
                result = "ERROR";
            }
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task ObtenerDatosIndicadoresPoliticaSgp_NoNulo()
        {
            var bpin = "2017011000096";
            var result = (OkNegotiatedContentResult<string>)await _gestionRecursosSgpController.ObtenerDatosIndicadoresPoliticaSgp(bpin);
            Assert.IsNotNull(result.Content);
        }

        [TestMethod]
        public async Task ObtenerDatosCategoriaProductosPoliticaSgp_NoNulo()
        {
            var bpin = "2017011000096";
            int fuenteId = 1;
            int politicaId = 6;
            var result = (OkNegotiatedContentResult<string>)await _gestionRecursosSgpController.ObtenerDatosCategoriaProductosPoliticaSgp(bpin, fuenteId, politicaId);
            Assert.IsNotNull(result.Content);
        }

        [TestMethod]
        public void GuardarDatosSolicitudRecursosSgp_NoNulo()
        {

            string result = "OK";
            CategoriaProductoPoliticaDto objCategoria = new CategoriaProductoPoliticaDto();
            try
            {
                _ = _gestionRecursosSgpController.GuardarDatosSolicitudRecursosSGP(objCategoria);
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
                _gestionRecursosSgpController.Dispose();
            }
            // free native resources
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
}
