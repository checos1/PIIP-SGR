using DNP.ServiciosNegocio.Comunes.Autorizacion;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Unity;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web.Http.Results;
using DNP.ServiciosNegocio.Servicios.Interfaces.SGP.Ajustes;
using DNP.ServiciosNegocio.Web.API.Controllers.SGP.Ajustes;
using DNP.ServiciosNegocio.Dominio.Dto.SGP.Transversales;
using DNP.ServiciosNegocio.Web.API.Controllers.SGP.GestionRecursos;
using DNP.ServiciosNegocio.Dominio.Dto.Proyectos;
using DNP.ServiciosNegocio.Dominio.Dto.CadenaValor;
using DNP.ServiciosNegocio.Dominio.Dto.SGP.Ajustes;

namespace DNP.ServiciosNegocio.Web.API.Test.SGP.Ajustes
{
    [TestClass]
    public class AjustesSgpControllerTest : IDisposable
    {
        private IAjustesSgpServicio IAjustesSgpServicio { get; set; }
        private IAutorizacionUtilidades AutorizacionUtilidades { get; set; }
        private AjustesSgpController _ajustesSgpController;

        [TestInitialize]
        public void Init()
        {
            var contenedor = Configuracion.UnityConfig.Container;
            IAjustesSgpServicio = contenedor.Resolve<IAjustesSgpServicio>();
            AutorizacionUtilidades = contenedor.Resolve<IAutorizacionUtilidades>();
            AutorizacionUtilidades = contenedor.Resolve<IAutorizacionUtilidades>();
            _ajustesSgpController = new AjustesSgpController(IAjustesSgpServicio, AutorizacionUtilidades);
            _ajustesSgpController.ControllerContext.Request = new HttpRequestMessage();
            _ajustesSgpController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _ajustesSgpController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:Backbone");
            _ajustesSgpController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });

        }


        #region Horizonte

        [TestMethod]
        public async Task ObtenerHorizonteSgp_NoNulo()
        {
            ParametrosEncabezadoSGP parametros = new ParametrosEncabezadoSGP();
            var result = (OkNegotiatedContentResult<string>)await _ajustesSgpController.ObtenerHorizonteSgp(parametros);
            Assert.IsNotNull(result.Content);
        }

        [TestMethod]
        public void ActualizarHorizonteProyectoSgp_NoNulo()
        {
            HorizonteProyectoDto DatosHorizonteProyecto = new HorizonteProyectoDto();
            string result = "OK";
            try
            {
                _ = _ajustesSgpController.ActualizarHorizonteProyectoSgp(DatosHorizonteProyecto);
            }
            catch
            {
                result = "ERROR";
            }
            Assert.IsNotNull(result);
        }
        
        [TestMethod]
        public async Task ObtenerCambiosJustificacionHorizonteSgp_NoNulo()
        {
            int IdProyecto = 616617;
            var result = (OkNegotiatedContentResult<string>)await _ajustesSgpController.ObtenerJustificacionHorizonteSgp(IdProyecto);
            Assert.IsNotNull(result.Content);
        }

        #endregion

        #region Indicadores

        [TestMethod]
        public void ObtenerIndicadoresProductoSgp_NoNulo()
        {
            var bpin = "2017011000096";
            string result = "OK";
            try
            {
                _ = _ajustesSgpController.ObtenerIndicadoresProductoSgp(bpin);
            }
            catch
            {
                result = "ERROR";
            }
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GuardarIndicadoresSecundariosSgp_NoNulo()
        {
            AgregarIndicadoresSecundariosDto objIndicador = new AgregarIndicadoresSecundariosDto();
            string result = "OK";
            try
            {
                _ = _ajustesSgpController.GuardarIndicadoresSecundariosSgp(objIndicador);
            }
            catch
            {
                result = "ERROR";
            }
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void EliminarIndicadorProductoSgp_NoNulo()
        {
            int indicadorId = 1;
            string result = "OK";
            try
            {
                _ = _ajustesSgpController.EliminarIndicadorProductoSgp(indicadorId);
            }
            catch
            {
                result = "ERROR";
            }
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void ActualizarMetaAjusteIndicadorSgp_NoNulo()
        {
            IndicadoresIndicadorProductoDto objIndicador = new IndicadoresIndicadorProductoDto();
            string result = "OK";
            try
            {
                _ = _ajustesSgpController.ActualizarMetaAjusteIndicadorSgp(objIndicador);
            }
            catch
            {
                result = "ERROR";
            }
            Assert.IsNotNull(result);
        }

        #endregion

        #region Beneficiarios
        
        [TestMethod]
        public async Task ObtenerProyectosBeneficiariosSgp_NoNulo()
        {
            var bpin = "2017011000096";
            var result = (OkNegotiatedContentResult<string>)await _ajustesSgpController.ObtenerProyectosBeneficiariosSgp(bpin);
            Assert.IsNotNull(result.Content);
        }

        [TestMethod]
        public async Task ObtenerProyectosBeneficiariosDetalleSgp_NoNulo()
        {
            var bpin = "2017011000096";
            var result = (OkNegotiatedContentResult<string>)await _ajustesSgpController.ObtenerProyectosBeneficiariosDetalleSgp(bpin);
            Assert.IsNotNull(result.Content);
        }

        [TestMethod]
        public void GuardarBeneficiarioTotalesSgp_NoNulo()
        {
            BeneficiarioTotalesDto objBeneficiarios = new BeneficiarioTotalesDto();
            string result = "OK";
            try
            {
                _ = _ajustesSgpController.GuardarBeneficiarioTotalesSgp(objBeneficiarios);
            }
            catch
            {
                result = "ERROR";
            }
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GuardarBeneficiarioProductoSgp_NoNulo()
        {
            BeneficiarioProductoSgpDto objBeneficiarios = new BeneficiarioProductoSgpDto();
            string result = "OK";
            try
            {
                _ = _ajustesSgpController.GuardarBeneficiarioProductoSgp(objBeneficiarios);
            }
            catch
            {
                result = "ERROR";
            }
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GuardarBeneficiarioProductoLocalizacionSgp_NoNulo()
        {
            BeneficiarioProductoLocalizacionDto objBeneficiarios = new BeneficiarioProductoLocalizacionDto();
            string result = "OK";
            try
            {
                _ = _ajustesSgpController.GuardarBeneficiarioProductoLocalizacionSgp(objBeneficiarios);
            }
            catch
            {
                result = "ERROR";
            }
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GuardarBeneficiarioProductoLocalizacionCaracterizacionSgp_NoNulo()
        {
            BeneficiarioProductoLocalizacionCaracterizacionDto objBeneficiarios = new BeneficiarioProductoLocalizacionCaracterizacionDto();
            string result = "OK";
            try
            {
                _ = _ajustesSgpController.GuardarBeneficiarioProductoLocalizacionCaracterizacionSgp(objBeneficiarios);
            }
            catch
            {
                result = "ERROR";
            }
            Assert.IsNotNull(result);
        }

        #endregion

        #region Localizaciones

        [TestMethod]
        public void GuardarLocalizacionSgp_NoNulo()
        {
            LocalizacionProyectoAjusteDto objLocaliza = new LocalizacionProyectoAjusteDto();
            string result = "OK";
            try
            {
                _ = _ajustesSgpController.GuardarLocalizacionSgp(objLocaliza);
            }
            catch
            {
                result = "ERROR";
            }
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task ObtenerCategoriasFocalizacionJustificacionSgp()
        {
            var bpin = "202400000000275";
            var result = (OkNegotiatedContentResult<string>)await _ajustesSgpController.ObtenerCategoriasFocalizacionJustificacionSgp(bpin);
            Assert.IsNotNull(result.Content);
        }

        [TestMethod]
        public async Task ObtenerDetalleCategoriasFocalizacionJustificacionSgp()
        {
            var bpin = "202400000000275";
            var result = (OkNegotiatedContentResult<string>)await _ajustesSgpController.ObtenerDetalleCategoriasFocalizacionJustificacionSgp(bpin);
            Assert.IsNotNull(result.Content);
        }


        #endregion

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                // dispose managed resources
                _ajustesSgpController.Dispose();
            }
            // free native resources
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
}
