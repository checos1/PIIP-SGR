

namespace DNP.Backbone.Test.Servicios
{
    using Backbone.Servicios.Interfaces.Autorizacion;
    using DNP.Backbone.Servicios.Interfaces.TramiteIncorporacion;
    using DNP.ServiciosNegocio.Dominio.Dto.TramiteIncorporacion;
    using Microsoft.Practices.Unity;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Summary description for TramiteServicioTest
    /// </summary>
    [TestClass]
    public class TramiteIncorporacionServicioTest
    {
        private ITramiteIncorporacionServicios _tramiteIncorporacionServicios;

        [TestInitialize]
        public void Init()
        {
            _tramiteIncorporacionServicios = Config.UnityConfig.Container.Resolve<ITramiteIncorporacionServicios>();
        }

        [TestMethod]
        public void EiliminarDatosIncorporacion()
        {
            ConvenioDto objConvenioDto = new ConvenioDto();
            objConvenioDto.Id = 0;
            objConvenioDto.NumeroConvenio = "as850";
            objConvenioDto.ObjetoConvenio = "objtestmock";
            objConvenioDto.TramiteId = 1181;
            objConvenioDto.ValorConvenio = 850000000;
            objConvenioDto.ValorConvenioVigencia = 850000000;

            ConvenioDonanteDto objConvenioDonanteDto = new ConvenioDonanteDto();
            objConvenioDonanteDto.Id = 0;
            objConvenioDonanteDto.ConvenioId = 0;
            objConvenioDonanteDto.EntityId = 186;
            objConvenioDonanteDto.NombreDonante = "testmockdonante";
            objConvenioDonanteDto.objConvenioDto = objConvenioDto;
            var actionResult = _tramiteIncorporacionServicios.EiliminarDatosIncorporacion(objConvenioDonanteDto, "CC505050").Result;
            Assert.IsNotNull(actionResult);
        }

        [TestMethod]
        public void GuardarDatosIncorporacion()
        {
            ConvenioDto objConvenioDto = new ConvenioDto();
            objConvenioDto.Id = 0;
            objConvenioDto.NumeroConvenio = "as850";
            objConvenioDto.ObjetoConvenio = "objtestmock";
            objConvenioDto.TramiteId = 1181;
            objConvenioDto.ValorConvenio = 850000000;
            objConvenioDto.ValorConvenioVigencia = 850000000;

            ConvenioDonanteDto objConvenioDonanteDto = new ConvenioDonanteDto();
            objConvenioDonanteDto.Id = 0;
            objConvenioDonanteDto.ConvenioId = 0;
            objConvenioDonanteDto.EntityId = 186;
            objConvenioDonanteDto.NombreDonante = "testmockdonante";
            objConvenioDonanteDto.objConvenioDto = objConvenioDto;
            var actionResult = _tramiteIncorporacionServicios.GuardarDatosIncorporacion(objConvenioDonanteDto, "CC505050").Result;
            Assert.IsNotNull(actionResult);
        }

        [TestMethod]
        public void ObtenerDatosIncorporacion()
        {
            int tramiteId = 1811;
            var actionResult = _tramiteIncorporacionServicios.ObtenerDatosIncorporacion(tramiteId, "CC505050").Result;
            Assert.IsNotNull(actionResult);
        }
        
    }
}
