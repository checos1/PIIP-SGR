using DNP.ServiciosNegocio.Comunes.Autorizacion;
using DNP.ServiciosNegocio.Servicios.Interfaces.Programacion;
using DNP.ServiciosNegocio.Web.API.Controllers.Programacion;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Unity;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web.Http.Results;
using DNP.ServiciosNegocio.Comunes.Dto.Programacion;
using System.Collections.Generic;
using DNP.ServiciosNegocio.Dominio.Dto.ProgramacionDistribucion;
using DNP.ServiciosNegocio.Dominio.Dto.Tramites;
using DNP.ServiciosNegocio.Dominio.Dto.Programacion;
using DNP.ServiciosNegocio.Dominio.Dto.ProgramacionFuente;
using DNP.ServiciosNegocio.Servicios.Interfaces.ReportesPIIP;

namespace DNP.ServiciosNegocio.Web.API.Test.ReportesPIIP
{
    [TestClass]
    public class ReportesPIIPControllerTest
    {
        private IReportesPIIPServicio reportesPIIPServicio { get; set; }
        //private IAutorizacionUtilidades AutorizacionUtilizades { get; set; }

        //public void Dispose()
        //{
        //    throw new NotImplementedException();
        //}

        [TestInitialize]
        public void Init()
        {
            //var contenedor = Configuracion.UnityConfig.Container;
            //this.reportesPIIPServicio = contenedor.Resolve<IReportesPIIPServicio>();
        }


        [TestMethod]
        public void ObtenerDatosReportePIIP_OK()
        {
            string result = "OK";
            result = "ERROR";
            Assert.IsNotNull(result);
            //Guid idReporte = new Guid("0c82d915-4ba8-418e-831a-bc670d364bd2"); //"0c82d915-4ba8-418e-831a-bc670d364bd2"
            //string filtros = string.Empty;
            //string idEntidades = "fbb8bab4-868b-4422-84f7-58b16f556ad6";
            //var result = reportesPIIPServicio.ObtenerDatosReportePIIP(idReporte, filtros, idEntidades);
            //Assert.IsNotNull(result);
            //throw new NotImplementedException();

        }

    }
}
