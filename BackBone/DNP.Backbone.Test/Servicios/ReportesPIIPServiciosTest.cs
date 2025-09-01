namespace DNP.Backbone.Test.Servicios
{
    using DNP.Backbone.Servicios.Interfaces.Focalizacion;
    using DNP.Backbone.Servicios.Interfaces.ReportePIIP;
    using Microsoft.Practices.Unity;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;

    [TestClass]
    public class ReportesPIIPServiciosTest
    {
        private IReportesPIIPServicio _IReportesPIIPServicio;


        [TestInitialize]
        public void Init()
        {
            this._IReportesPIIPServicio = Config.UnityConfig.Container.Resolve<IReportesPIIPServicio>();
        }

        [TestMethod]
        public void ObtenerDatosReportePIIP_Ok()
        {
            Guid idReporte = new Guid("0c82d915-4ba8-418e-831a-bc670d364bd2"); //
            string filtros = string.Empty;
            var actionResult = _IReportesPIIPServicio.ObtenerDatosReportePIIP(idReporte, filtros, "CC505050", "f92320bf-2c52-42e3-a2f0-11f456921abb,fbb8bab4-868b-4422-84f7-58b16f556ad6,f049d35c-8a88-4093-a1a4-8410f3a83ee0", "" );
            Assert.IsNotNull(actionResult.Result.ToString());
            Assert.IsTrue(string.IsNullOrEmpty(actionResult.Result));
        }

    }
}
