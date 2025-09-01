using DNP.ServiciosNegocio.Comunes.Autorizacion;
using DNP.ServiciosNegocio.Servicios.Interfaces.FuenteFinanciacion;
using DNP.ServiciosNegocio.Web.API.Controllers.Negocio;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Net.Http;
using Unity;
using System.Threading.Tasks;
using System.Web.Http.Results;


namespace DNP.ServiciosNegocio.Web.API.Test.Negocio
{
    public class FuentesAprobacionControllerTest : IDisposable
    {
        private IFuentesAprobacionServicio FuentesAprobacionServicio { get; set; }
        private IAutorizacionUtilidades AutorizacionUtilizades { get; set; }

        private FuentesAprobacionController _FuentesAprobacionController;
        private string Bpin { get; set; }


        [TestInitialize]
        public void Init()
        {
            Bpin = "2017011000042";
            var contenedor = Configuracion.UnityConfig.Container;
            FuentesAprobacionServicio = contenedor.Resolve<IFuentesAprobacionServicio>();
            AutorizacionUtilizades = contenedor.Resolve<IAutorizacionUtilidades>();

            _FuentesAprobacionController =
                new FuentesAprobacionController(FuentesAprobacionServicio, AutorizacionUtilizades)
                {
                    ControllerContext
                        =
                        {
                            Request
                                = new
                                    HttpRequestMessage()
                        }
                };

        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
