using DNP.Backbone.Servicios.Interfaces.Autorizacion;
using DNP.Backbone.Servicios.Interfaces.FuenteFinanciacion;
using DNP.Backbone.Servicios.Interfaces.ReportePIIP;
using DNP.ServiciosNegocio.Dominio.Dto.FuenteFinanciacion;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.Backbone.Web.API.Test.WebAPI
{
    
    public class ReportesPIIPControllerTest
    {

        private IReportesPIIPServicio _ReportesPIIPServicio;
        private IAutorizacionServicios _autorizacionUtilidades;

        [TestInitialize]
        public void Init()
        {
            _ReportesPIIPServicio  = Config.UnityConfig.Container.Resolve<IReportesPIIPServicio>(); 
            _autorizacionUtilidades =  Config.UnityConfig.Container.Resolve<IAutorizacionServicios>();
        }

        [TestMethod]
        public void ObtenerPreguntasAprobacionRol()
        {
            Guid idReporte = new Guid("0c82d915-4ba8-418e-831a-bc670d364bd2");
            string filtros = string.Empty;
            string idEntidades = "fbb8bab4-868b-4422-84f7-58b16f556ad6";

            var actionResult = _ReportesPIIPServicio.ObtenerDatosReportePIIP(idReporte, filtros, "CC505050", idEntidades, "");
            Assert.IsNotNull(actionResult.Result.ToString());
            Assert.IsTrue(string.IsNullOrEmpty(actionResult.Result));
        }
    }
}
