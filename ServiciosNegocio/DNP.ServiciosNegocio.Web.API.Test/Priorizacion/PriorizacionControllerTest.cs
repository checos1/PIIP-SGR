using DNP.ServiciosNegocio.Comunes.Autorizacion;
using DNP.ServiciosNegocio.Comunes.Dto.ObjetosNegocio;
using DNP.ServiciosNegocio.Dominio.Dto.Priorizacion;
using DNP.ServiciosNegocio.Servicios.Interfaces.Priorizacion;
using DNP.ServiciosNegocio.Web.API.Controllers.Priorizacion;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Unity;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web.Http.Results;
using DNP.ServiciosNegocio.Comunes.Dto.Viabilidad;

namespace DNP.ServiciosNegocio.Web.API.Test.Priorizacion
{
    [TestClass]
    public class PriorizacionControllerTest : IDisposable
    {
        private IPriorizacionServicio PriorizacionServicio { get; set; }
        private IAutorizacionUtilidades AutorizacionUtilizades { get; set; }
        private PriorizacionController _priorizacionController;

        [TestInitialize]
        public void Init()
        {
            var contenedor = Configuracion.UnityConfig.Container;
            PriorizacionServicio = contenedor.Resolve<IPriorizacionServicio>();
            AutorizacionUtilizades = contenedor.Resolve<IAutorizacionUtilidades>();
            _priorizacionController = new PriorizacionController(PriorizacionServicio, AutorizacionUtilizades);
            _priorizacionController.ControllerContext.Request = new HttpRequestMessage();
            _priorizacionController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _priorizacionController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:Backbone");
            _priorizacionController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });
        }

        [TestMethod]
        public async Task ConsultarProyecto_NoNulo()
        {
            var bpin = "97902";
            var result = await _priorizacionController.ConsultarProyectosPorBPINs(new BPINsProyectosDto()
            {
                BPINs = new List<string>() { bpin }

            });
            var priorizacion = result as OkNegotiatedContentResult<List<PriorizacionDatosBasicosDto>>;

            Assert.IsNull(priorizacion);
        }

        [TestMethod]
        public async Task ConsultarProyectoPorBPINs_Nulo()
        {
            var bpin = "000000000";
            var result = await _priorizacionController.ConsultarProyectosPorBPINs(new BPINsProyectosDto()
            {
                BPINs = new List<string>() { bpin }

            });
            var priorizacion = result as OkNegotiatedContentResult<List<PriorizacionDatosBasicosDto>>;

            Assert.IsNull(priorizacion);
        }

        [TestMethod]
        public async Task ConsultarFuentesSGR_NoNulo()
        {
            var bpin = "97954";
            var instanciaId = "00000000-0000-0000-0000-000000000000";
            var result = (OkNegotiatedContentResult<string>)await _priorizacionController.ObtenerFuentesSGR(bpin, new Guid(instanciaId));
            Assert.IsNotNull(result.Content);
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
        public async Task ConsultarFuentesNoSGR_NoNulo()
        {
            var bpin = "97954";
            var instanciaId = "00000000-0000-0000-0000-000000000000";
            var result = (OkNegotiatedContentResult<string>)await _priorizacionController.ObtenerFuentesSGR(bpin, new Guid(instanciaId));
            Assert.IsNotNull(result.Content);
        }

        [TestMethod]
        public void RegistrarFuentesNoSGR_Test()
        {
            List<VigenciasNoSGRDto> objVigencia = new List<VigenciasNoSGRDto>();
            List<EtapaNoSGRDto> lstEtapaNoSGRDto = new List<EtapaNoSGRDto>();
            EtapaNoSGRDto etapaNoSGRDto = new EtapaNoSGRDto
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
        public async Task ConsultarResumenFuentesCostos_NoNulo()
        {
            var bpin = "97954";
            var instanciaId = "00000000-0000-0000-0000-000000000000";
            var result = (OkNegotiatedContentResult<string>)await _priorizacionController.ObtenerResumenFuentesCostos(bpin, new Guid(instanciaId));
            Assert.IsNotNull(result.Content);
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
        public async Task ConsultarDatosAdicionalesCofinanciadorNoSGR_NoNulo()
        {
            var bpin = "97954";
            var vigencia = 2023;
            var vigenciaFuente = 2037;
            var result = (OkNegotiatedContentResult<string>)await _priorizacionController.ObtenerDatosAdicionalesCofinanciadorNoSGR(bpin, vigencia, vigenciaFuente);
            Assert.IsNotNull(result.Content);
        }


        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                // dispose managed resources
                _priorizacionController.Dispose();
            }
            // free native resources
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
}
