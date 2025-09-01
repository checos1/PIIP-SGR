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

namespace DNP.ServiciosNegocio.Web.API.Test.Negocio
{
    [TestClass]
    public sealed class CategoriaControllerTest : IDisposable
    {
        private ICategoriaProductosPoliticaServicio _categoriaProductosPoliticaServicio { get; set; }
        private string Bpin { get; set; }

        [TestInitialize]
        public void Init()
        {
           
        }

        //[TestMethod]
        //public async Task ObtenerDatosCategoriaProductosPolitica()
        //{

        //}

        //[TestMethod]
        //public async Task GuardarDatosSolicitudRecursos()
        //{
        //}

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                // dispose managed resources
            }
            // free native resources
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
}
