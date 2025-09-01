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
    public sealed class IndicadoresPoliticaControllerTest
    {
        private IIndicadoresPoliticaServicio _indicadoresPoliticaServicio { get; set; }
        private string Bpin { get; set; }

        [TestInitialize]
        public void Init()
        {
            Bpin = "202100000000007";
            _indicadoresPoliticaServicio = Configuracion.UnityConfig.Container.Resolve<IIndicadoresPoliticaServicio>();
        }

        [TestMethod]
        public void ObtenerDatosIndicadoresPolitica_ok()
        {
            var actionResult = _indicadoresPoliticaServicio.ObtenerDatosIndicadoresPolitica(Bpin);
            Assert.IsNotNull(actionResult.ToString());
            Assert.IsTrue(string.IsNullOrEmpty(actionResult));
        }
    }
}
