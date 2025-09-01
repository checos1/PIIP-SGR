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
using DNP.ServiciosNegocio.Servicios.Interfaces.IndicadoresPolitica;

namespace DNP.ServiciosNegocio.Web.API.Test.Focalizacion
{
    [TestClass]
    public sealed class CategoriaProductosPoliticaControllerTest
    {
        private ICategoriaProductosPoliticaServicio _indicadoresPoliticaServicio { get; set; }
        private string Bpin { get; set; }
        private int fuenteId { get; set; }
        private int politicaId { get; set; }

        [TestInitialize]
        public void Init()
        {
            Bpin = "202100000000007";
            fuenteId = 1281;
            politicaId = 5;
            _indicadoresPoliticaServicio = Configuracion.UnityConfig.Container.Resolve<ICategoriaProductosPoliticaServicio>();
        }

        [TestMethod]
        public void ObtenerDatosIndicadoresPolitica_ok()
        {
            var actionResult = _indicadoresPoliticaServicio.ObtenerDatosCategoriaProductosPolitica(Bpin, fuenteId, politicaId);
            Assert.IsNotNull(actionResult.ToString());
            Assert.IsTrue(string.IsNullOrEmpty(actionResult));
        }
    }
}
