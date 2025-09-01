using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DNP.ServiciosNegocio.Comunes.Autorizacion;
using DNP.ServiciosNegocio.Servicios.Interfaces.FuenteFinanciacion;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DNP.ServiciosNegocio.Web.API.Controllers.Negocio;
using DNP.ServiciosNegocio.Servicios.Interfaces.Preguntas;
using DNP.ServiciosNegocio.Web.API.Controllers.Preguntas;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Security.Principal;
using Unity;
using DNP.ServiciosNegocio.Dominio.Dto.Preguntas;
using System.Web.Http.Results;
using DNP.ServiciosNegocio.Dominio.Dto.FuenteFinanciacion;
using System.Web.Http;

namespace DNP.ServiciosNegocio.Web.API.Test.Negocio
{
    [TestClass]

    public class FuenteFinanciacionAgregarControllerTest
    {
        private IFuenteFinanciacionAgregarServicio _fuenteFinanciacionAgregarServicio { get; set; }
        private IAutorizacionUtilidades _autorizacionUtilidades { get; set; }
        private FuenteFinanciacionAgregarController _fuenteFinanciacionAgregarController;

        [TestInitialize]
        public void Init()
        {
            var contenedor = Configuracion.UnityConfig.Container;
            _fuenteFinanciacionAgregarServicio = contenedor.Resolve<IFuenteFinanciacionAgregarServicio>();
            _autorizacionUtilidades = contenedor.Resolve<IAutorizacionUtilidades>();
            _fuenteFinanciacionAgregarController = new FuenteFinanciacionAgregarController(_fuenteFinanciacionAgregarServicio, _autorizacionUtilidades);
            _fuenteFinanciacionAgregarController.ControllerContext.Request = new HttpRequestMessage();
            _fuenteFinanciacionAgregarController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _fuenteFinanciacionAgregarController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:Backbone");
            _fuenteFinanciacionAgregarController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });
        }


        [TestMethod]
        public async Task ObtenerOperacionCreditoDatosGenerales_NoNulo()
        {
            string bpin = "617586";
            var result = (OkNegotiatedContentResult<OperacionCreditoDatosGeneralesDto>)await _fuenteFinanciacionAgregarController.ObtenerOperacionCreditoDatosGenerales(bpin);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task ObtenerOperacionesCreditoDetalles_NoNulo()
        {
            string bpin = "617586";
            var result = (OkNegotiatedContentResult<OperacionCreditoDetallesDto>)await _fuenteFinanciacionAgregarController.ObtenerOperacionCreditoDetalles(bpin);
            Assert.IsNotNull(result);
        }

    }
}
