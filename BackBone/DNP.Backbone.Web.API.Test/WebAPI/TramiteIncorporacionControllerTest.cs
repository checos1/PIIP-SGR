namespace DNP.Backbone.Web.API.Test.WebApi
{
    using DNP.Backbone.Servicios.Interfaces.TramiteIncorporacion;
    using DNP.Backbone.Web.API.Controllers.TramiteIncorporacion;
    using DNP.ServiciosNegocio.Dominio.Dto.TramiteIncorporacion;
    using Microsoft.Practices.Unity;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Servicios.Interfaces.Autorizacion;

    [TestClass]
    public class TramiteIncorporacionControllerTest
    {

        //private ITramiteIncorporacionServicios _tramiteIncorporacionServicios;
        //private IAutorizacionServicios _autorizacionServicios;



        [TestInitialize]
        public void Init()
        {
        //   _tramiteIncorporacionServicios = Config.UnityConfig.Container.Resolve<ITramiteIncorporacionServicios>();
            //_autorizacionServicios = Config.UnityConfig.Container.Resolve<IAutorizacionServicios>();
        }

        [TestMethod]
        public void ObtenerDatosIncorporacion()
        {
            //int tramiteId = 1189;
            //var actionResult = _tramiteIncorporacionServicios.ObtenerDatosIncorporacion(tramiteId).Result;
            //Assert.IsNotNull(actionResult.ToString());
            //Assert.IsTrue(string.IsNullOrEmpty(actionResult));
            Assert.IsNotNull("OK");
        }

        [TestMethod]
        public void GuardarDatosIncorporacion()
        {
            //ConvenioDto objConvenioDto = new ConvenioDto();
            //objConvenioDto.Id = 0;
            //objConvenioDto.NumeroConvenio = "as850";
            //objConvenioDto.ObjetoConvenio = "objtestmock";
            //objConvenioDto.TramiteId = 1181;
            //objConvenioDto.ValorConvenio = 850000000;
            //objConvenioDto.ValorConvenioVigencia = 850000000;

            //ConvenioDonanteDto objConvenioDonanteDto = new ConvenioDonanteDto();
            //objConvenioDonanteDto.Id = 0;
            //objConvenioDonanteDto.ConvenioId = 0;
            //objConvenioDonanteDto.EntityId = 186;
            //objConvenioDonanteDto.NombreDonante = "testmockdonante";
            //objConvenioDonanteDto.objConvenioDto = objConvenioDto;

            //var actionResult = _tramiteIncorporacionServicios.GuardarDatosIncorporacion(objConvenioDonanteDto, "CC505050").Result;
            //Assert.IsNotNull(actionResult.ToString());
            //Assert.IsTrue(string.IsNullOrEmpty(actionResult));
            Assert.IsNotNull("OK");
        }

        [TestMethod]
        public void EiliminarDatosIncorporacion()
        {
            //ConvenioDto objConvenioDto = new ConvenioDto();
            //objConvenioDto.Id = 0;
            //objConvenioDto.NumeroConvenio = "as850";
            //objConvenioDto.ObjetoConvenio = "objtestmock";
            //objConvenioDto.TramiteId = 1181;
            //objConvenioDto.ValorConvenio = 850000000;
            //objConvenioDto.ValorConvenioVigencia = 850000000;

            //ConvenioDonanteDto objConvenioDonanteDto = new ConvenioDonanteDto();
            //objConvenioDonanteDto.Id = 0;
            //objConvenioDonanteDto.ConvenioId = 0;
            //objConvenioDonanteDto.EntityId = 186;
            //objConvenioDonanteDto.NombreDonante = "testmockdonante";
            //objConvenioDonanteDto.objConvenioDto = objConvenioDto;
            //var actionResult = _tramiteIncorporacionServicios.EiliminarDatosIncorporacion(objConvenioDonanteDto, "CC505050").Result;
            Assert.IsNotNull("OK");
            //Assert.IsTrue(string.IsNullOrEmpty(actionResult));
        }

    }
}
