using Microsoft.VisualStudio.TestTools.UnitTesting;
using Unity;

namespace DNP.ServiciosNegocio.Test.Servicios.ReportesPIIP
{
    using Configuracion;
    using DNP.ServiciosNegocio.Persistencia.Interfaces;
    using DNP.ServiciosNegocio.Persistencia.Interfaces.ReportesPIIP;
    using DNP.ServiciosNegocio.Servicios.Implementaciones.ReportesPIIP;
    using DNP.ServiciosNegocio.Servicios.Interfaces.Transversales;
    using Moq;
    using System;

    [TestClass]
    public class ReportesPIIPServicioTest
    {
        private IReportesPIIPPersistencia ReportesPIIPPersistencia { get; set; }
        private ICacheServicio CacheServicio { get; set; }

        [TestInitialize]
        public void Init()
        {
            var contenedor = UnityConfig.Container;
            CacheServicio = contenedor.Resolve<ICacheServicio>();
            ReportesPIIPPersistencia = contenedor.Resolve<IReportesPIIPPersistencia>();
            //ReportesPIIPServicio = new ReportesPIIPServicio(CacheServicio, ReportesPIIPServicio);
            //ReportesPIIPServicio = contenedor.Resolve<IReportesPIIPPersistencia>();
        }

        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public void ObtenerDatosReportePIIP()
        {

            throw new NotImplementedException();

            //try
            //{
            //    Guid idReporte = new Guid("0c82d915-4ba8-418e-831a-bc670d364bd2");
            //    string filtros = string.Empty;
            //    string idEntidades = "fbb8bab4-868b-4422-84f7-58b16f556ad6";
            //    var result = "fbb8bab4-868b-4422-84f7-58b16f556ad6";
            //    //ReportesPIIPPersistencia.ObtenerDatosReportePIIP(idReporte, filtros, idEntidades);
            //    //Assert.IsNotNull(result);


            //}
            //catch (Exception)
            //{
            //}
        }

    }
}
