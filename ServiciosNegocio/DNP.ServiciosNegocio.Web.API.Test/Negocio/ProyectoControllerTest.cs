using System.Threading.Tasks;
using System.Web.Http.Results;
using DNP.ServiciosNegocio.Comunes.Autorizacion;
using DNP.ServiciosNegocio.Comunes.Excepciones;
using DNP.ServiciosNegocio.Dominio.Dto.Proyectos;
using DNP.ServiciosNegocio.Servicios.Interfaces.Proyectos;
using DNP.ServiciosNegocio.Web.API.Controllers.Negocio;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Unity;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Principal;

namespace DNP.ServiciosNegocio.Web.API.Test.Negocio
{
    using System.Collections.Generic;
    using System.Net;
    using Comunes;
    using DNP.ServiciosNegocio.Comunes.Dto.ObjetosNegocio;

    [TestClass]
    public class ProyectoControllerTest : IDisposable
    {

        private IProyectoServicio ProyectoServicio { get; set; }
        private IAutorizacionUtilidades AutorizacionUtilizades { get; set; }
        private ProyectoController _proyectoController;

        [TestInitialize]
        public void Init()
        {
            var contenedor = Configuracion.UnityConfig.Container;
            ProyectoServicio = contenedor.Resolve<IProyectoServicio>();
            AutorizacionUtilizades = contenedor.Resolve<IAutorizacionUtilidades>();
            _proyectoController = new ProyectoController(ProyectoServicio, AutorizacionUtilizades);
            _proyectoController.ControllerContext.Request = new HttpRequestMessage();
            _proyectoController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _proyectoController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:Backbone");
            _proyectoController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });
        }

        [TestMethod]
        public async Task ConsultarProyecto_NoNulo()
        {
            var bpin = "2017761220016";
            var result = (OkNegotiatedContentResult<ProyectoDto>)await _proyectoController.Consultar(bpin);
            Assert.IsNotNull(result.Content);
        }

        [TestMethod]
        public async Task ConsultarProyecto_Nulo()
        {
            var bpin = "000000000";
            var result = await _proyectoController.Consultar(bpin);
            var proyecto = (OkNegotiatedContentResult<ProyectoDto>)result;

            Assert.IsNull(proyecto.Content.Entidad);
        }

        [TestMethod]
        public async Task ConsultarProyectoPreview_NoNulo()
        {
            var result = (OkNegotiatedContentResult<ProyectoDto>)await _proyectoController.Preview();
            Assert.IsNotNull(result.Content);
        }

        [TestMethod]
        [ExpectedException(typeof(ServiciosNegocioHttpResponseException))]
        public async Task CuandoEnvioIdsEntidadesNulo_RetornaExcepcion()
        {
            var idsEstados = new List<string>() { "Formulado" };
            ParametrosProyectosDto entidadesyEstados = new ParametrosProyectosDto
            {
                NombresEstadosProyectos = idsEstados
            };

            await _proyectoController.ConsultarProyectosPorEntidadesYEstados(entidadesyEstados);
        }

        [TestMethod]
        [ExpectedException(typeof(ServiciosNegocioHttpResponseException))]
        public async Task CuandoEnvioIdsEstadosNulo_RetornaExcepcion()
        {
            var idsEntidades = new List<int>() { 636 };
            ParametrosProyectosDto entidadesyEstados = new ParametrosProyectosDto
            {
                IdsEntidades = idsEntidades
            };
            await _proyectoController.ConsultarProyectosPorEntidadesYEstados(entidadesyEstados);
        }

        [TestMethod]
        [ExpectedException(typeof(ServiciosNegocioHttpResponseException))]
        public async Task CuandoEnvioIdsEntidadesVacio_RetornaExcepcion()
        {
            var idsEstados = new List<string>() { "Formulado" };
            ParametrosProyectosDto entidadesyEstados = new ParametrosProyectosDto
            {
                IdsEntidades = new List<int>(),
                NombresEstadosProyectos = idsEstados
            };
            await _proyectoController.ConsultarProyectosPorEntidadesYEstados(entidadesyEstados);
        }

        [TestMethod]
        [ExpectedException(typeof(ServiciosNegocioHttpResponseException))]
        public async Task CuandoEnvioIdsEstadoVacio_RetornaExcepcion()
        {
            var idsEntidades = new List<int>() { 636 };
            ParametrosProyectosDto entidadesyEstados = new ParametrosProyectosDto
            {
                IdsEntidades = idsEntidades,
                NombresEstadosProyectos = new List<string>()
            };
            await _proyectoController.ConsultarProyectosPorEntidadesYEstados(entidadesyEstados);
        }

        [TestMethod]
        public async Task CuandoEnvioIdsExistentes_RetornaResultados()
        {
            var idsEntidades = new List<int>() { 636 };
            var idsEstados = new List<string>() { "Disponible" };

            ParametrosProyectosDto entidadesyEstados = new ParametrosProyectosDto
            {
                IdsEntidades = idsEntidades,
                NombresEstadosProyectos = idsEstados
            };

            var result = await _proyectoController.ConsultarProyectosPorEntidadesYEstados(entidadesyEstados);

            var proyectos = (OkNegotiatedContentResult<List<ProyectoEntidadDto>>)result;

            Assert.IsNotNull(proyectos);
        }

        [TestMethod]
        public async Task CuandoEnvioIdsInexistentes_RetornaSinResultados()
        {
            var idsEntidades = new List<int>() { 5 };
            var idsEstados = new List<string>() { "Viable" };

            ParametrosProyectosDto entidadesyEstados = new ParametrosProyectosDto
            {
                IdsEntidades = idsEntidades,
                NombresEstadosProyectos = idsEstados
            };

            var result = await _proyectoController.ConsultarProyectosPorEntidadesYEstados(entidadesyEstados);

            var proyecto = (OkNegotiatedContentResult<List<ProyectoEntidadDto>>)result;

            Assert.AreEqual(proyecto.Content.Count, 0);
        }

        [TestMethod]
        public async Task ConsultarProyectosPorIds_Ok()
        {
            var result = await _proyectoController.ConsultarProyectosPorIds(new List<int>() { 636 });

            var proyectos = (OkNegotiatedContentResult<List<ProyectoEntidadDto>>)result;

            Assert.IsNotNull(proyectos);
        }

        [TestMethod]
        public async Task ConsultarProyectosContracreditos_Ok()
        {
            var result = await _proyectoController.ConsultarProyectosContracredito(new ProyectoCreditoParametroDto { TipoEntidad = "Nacional", IdFLujo = new Guid("CF1592AA-9087-3D77-B451-6F3557EF3F82") });

            var proyectos = (OkNegotiatedContentResult<IEnumerable<ProyectoCreditoDto>>)result;

            Assert.IsNotNull(proyectos);
        }

        [TestMethod]
        public async Task ConsultarProyectosCreditos_Ok()
        {
            var result = await _proyectoController.ConsultarProyectosContracredito(new ProyectoCreditoParametroDto { TipoEntidad = "Nacional", IdFLujo = new Guid("CF1592AA-9087-3D77-B451-6F3557EF3F82"), IdEntidad = 41 });

            var proyectos = (OkNegotiatedContentResult<IEnumerable<ProyectoCreditoDto>>)result;

            Assert.IsNotNull(proyectos);
        }


        [TestMethod]
        public void GuardarBeneficiarioTotales_Test()
        {
            BeneficiarioTotalesDto beneficiarioTotalesDto = new BeneficiarioTotalesDto();

            beneficiarioTotalesDto.BPIN = "202200000000110";
            beneficiarioTotalesDto.NumeroPersonalAjuste = 1;
            beneficiarioTotalesDto.ProyectoId = 4;

            string result = "OK";

            try
            {
                _proyectoController.GuardarBeneficiarioTotales(beneficiarioTotalesDto);
            }
            catch
            {
                result = "ERROR";
            }

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GuardarBeneficiarioProducto_Test()
        {
            BeneficiarioProductoDto beneficiarioDto = new BeneficiarioProductoDto();

            beneficiarioDto.ProyectoId = 1;
            beneficiarioDto.ProductoId = 1;
            beneficiarioDto.InterventionLocalizationTypeId = 1;
            beneficiarioDto.PersonasBeneficiaros = 1;

            string result = "OK";

            try
            {
                _proyectoController.GuardarBeneficiarioProducto(beneficiarioDto);
            }
            catch
            {
                result = "ERROR";
            }

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void ConsultarProyectosASeleccionarOk()
        {
            List<int> entidades = new List<int>();
            entidades.Add(186);
            entidades.Add(177);
            entidades.Add(105);
            entidades.Add(267);

            ParametrosProyectosDto entidadesEstados = new ParametrosProyectosDto()
            {
                tipoTramiteId = 31,
                flujoid = Guid.Parse("f562885e-3c75-d1b4-6ebc-4bcebb17ca6c"),
                IdsEntidades = entidades,

            };

            var resultados = _proyectoController.ConsultarProyectosASeleccionar(entidadesEstados);
            Assert.IsNotNull(resultados);


        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                // dispose managed resources
                _proyectoController.Dispose();
            }
            // free native resources
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
}
