using DNP.ServiciosNegocio.Dominio.Dto.SeguimientoControl;
using DNP.ServiciosNegocio.Servicios.Implementaciones.SeguimientoControl;
using DNP.ServiciosNegocio.Web.API.Test.Mock;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

namespace DNP.ServiciosNegocio.Web.API.Test.SeguimientoControl
{
    [TestClass]
    public class DesagregarEdtTest
    {
        [TestMethod]
        public void ObtenerListadoObjProdNivelesTest()
        {
            var service = new DesagregarEdtServicio(new DesagregarEdtServiceMock());
            service.ObtenerListadoObjProdNiveles(new ConsultaObjetivosProyecto("97869"));
        }
    }
}
