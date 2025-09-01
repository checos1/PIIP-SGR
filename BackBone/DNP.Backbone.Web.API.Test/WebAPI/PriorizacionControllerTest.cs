using DNP.Backbone.Servicios.Interfaces.Autorizacion;
using DNP.Backbone.Servicios.Interfaces.Priorizacion;
using DNP.Backbone.Web.API.Controllers.Priorizacion;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Principal;
using Microsoft.Practices.Unity;
using System.Threading.Tasks;
using System.Web.Http.Results;
using System.Collections.Generic;
using DNP.Backbone.Dominio.Dto.Priorizacion;
using DNP.Backbone.Comunes.Dto;
using System;
using DNP.Backbone.Dominio.Dto.Priorizacion.Viabilidad;

namespace DNP.Backbone.Web.API.Test.WebAPI
{
    [TestClass]
    public class PriorizacionControllerTest
    {
        private IPriorizacionServicios _priorizacionServicios;
        private IAutorizacionServicios _autorizacionServicios;
        private PriorizacionController _priorizacionController;

        [TestInitialize]
        public void Init()
        {
            _priorizacionServicios = Config.UnityConfig.Container.Resolve<IPriorizacionServicios>();
            _autorizacionServicios = Config.UnityConfig.Container.Resolve<IAutorizacionServicios>();

            _priorizacionController = new PriorizacionController(_priorizacionServicios, _autorizacionServicios);
            _priorizacionController.ControllerContext.Request = new HttpRequestMessage();
            _priorizacionController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _priorizacionController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:Backbone");
            _priorizacionController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });
        }

        [TestMethod]
        public async Task ConsultarProyecto_Ok()
        {
            var bpin = "97902";
            var result = (OkNegotiatedContentResult<List<PriorizacionDatosBasicosDto>>)await _priorizacionController.ObtenerProyectosPorBPINs(new BPINsProyectosDto()
            {
                BPINs = new List<string>() { bpin }

            });
            Assert.IsNotNull(result.Content);
        }

        [TestMethod]
        public void ObtenerFuentesSGR_Ok()
        {
            var bpin = "97954";
            var instanciaId = "00000000-0000-0000-0000-000000000000";
            var actionResult = _priorizacionController.ObtenerFuentesSGR(bpin, new Guid(instanciaId)).Result;
            Assert.IsNotNull(actionResult);
        }

        [TestMethod]
        public void RegistrarFuentesSGR_Test()
        {
            List<VigenciasSGRDto> objVigencia = new List<VigenciasSGRDto>();
            List<EtapaSGRDto> lstEtapaSGRDto = new List<EtapaSGRDto>();
            EtapaSGRDto etapaSGRDto = new EtapaSGRDto()
            {
                EtapaId = 3,
                ProyectoId = 97954,
                Vigencias = objVigencia
            };
            lstEtapaSGRDto.Add(etapaSGRDto);

            string result = "OK";

            try
            {
                _priorizacionController.RegistrarFuentesSGR(lstEtapaSGRDto);
            }
            catch
            {
                result = "ERROR";
            }

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void ObtenerFuentesNoSGR_Ok()
        {
            var bpin = "97954";
            var instanciaId = "00000000-0000-0000-0000-000000000000";
            var actionResult = _priorizacionController.ObtenerFuentesNoSGR(bpin, new Guid(instanciaId)).Result;
            Assert.IsNotNull(actionResult);
        }

        [TestMethod]
        public void RegistrarFuentesNoSGR_Test()
        {
            List<VigenciasNoSGRDto> objVigencia = new List<VigenciasNoSGRDto>();
            List<EtapaNoSGRDto> lstEtapaNoSGRDto = new List<EtapaNoSGRDto>();
            EtapaNoSGRDto etapaNoSGRDto = new EtapaNoSGRDto()
            {
                EtapaId = 3,
                ProyectoId = 97954,
                Vigencias = objVigencia
            };
            lstEtapaNoSGRDto.Add(etapaNoSGRDto);


            string result = "OK";

            try
            {
                _priorizacionController.RegistrarFuentesNoSGR(lstEtapaNoSGRDto);
            }
            catch
            {
                result = "ERROR";
            }

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void ObtenerResumenFuentesCostos_Ok()
        {
            var bpin = "97954";
            var instanciaId = "00000000-0000-0000-0000-000000000000";
            var actionResult = _priorizacionController.ObtenerResumenFuentesCostos(bpin, new Guid(instanciaId)).Result;
            Assert.IsNotNull(actionResult);
        }

        [TestMethod]
        public void ObtenerTiposCofinanciaciones_Ok()
        {            
            var actionResult = _priorizacionController.ConsultarTiposCofinanciaciones().Result;
            Assert.IsNotNull(actionResult);
        }

        [TestMethod]
        public void RegistrarDatosAdicionalesCofinanciadorFuentesNoSGR_Test()
        {
            //List<DatosAdicionalesCofinanciadorDto> lstDatosCofinanciador = new List<DatosAdicionalesCofinanciadorDto>();
            List<VigenciasFuentesNoSGRDto> lstVigenciasFuentesNoSGRDto = new List<VigenciasFuentesNoSGRDto>();
            DatosAdicionalesCofinanciadorDto vigenciaFuenteDto = new DatosAdicionalesCofinanciadorDto
            {
                ProyectoId = 98095,
                Vigencia = 2023,
                VigenciasFuentes = lstVigenciasFuentesNoSGRDto
            };
            //lstDatosCofinanciador.Add(vigenciaFuenteDto);


            string result = "OK";

            try
            {
                _ = _priorizacionController.RegistrarDatosAdicionalesCofinanciadorFuentesNoSGR(vigenciaFuenteDto);
            }
            catch
            {
                result = "ERROR";
            }

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void ObtenerDatosAdicionalesCofinanciadorNoSGR_Ok()
        {
            var bpin = "97954";
            var vigencia = 2023;
            var vigenciaFuente = 2037;
            var actionResult = _priorizacionController.ObtenerDatosAdicionalesCofinanciadorNoSGR(bpin, vigencia, vigenciaFuente).Result;
            Assert.IsNotNull(actionResult);
        }
    }
}
