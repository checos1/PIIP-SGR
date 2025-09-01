using DNP.ServiciosNegocio.Dominio.Dto.SeguimientoControl;
using DNP.ServiciosNegocio.Servicios.Implementaciones.SeguimientoControl;
using DNP.ServiciosNegocio.Web.API.Test.Mock;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

namespace DNP.ServiciosNegocio.Web.API.Test.SeguimientoControl
{
    [TestClass]
    public class ProgramarProductosTest
    {
        [TestMethod]
        public void ObtenerListadoObjProdNiveles()
        {
            string bpin = "202200000000202";
            var service = new ProgramarProductosServicio(new ProgramarProductoServiceMock());
            service.ObtenerListadoObjProdNiveles(bpin);
        }
    }
}
