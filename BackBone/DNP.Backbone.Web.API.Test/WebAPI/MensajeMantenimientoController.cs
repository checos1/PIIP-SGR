using DNP.Backbone.Servicios.Interfaces.Autorizacion;
using DNP.Backbone.Servicios.Interfaces.MensajesMantenimiento;
using DNP.Backbone.Web.API.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using System.Web.Http.Results;
using DNP.Backbone.Dominio.Dto.MensajeMantenimiento;
using DNP.Backbone.Comunes.Dto;
using DNP.Backbone.Dominio.Filtros.MensajeMantenimiento;

namespace DNP.Backbone.Web.API.Test.WebAPI
{
    [TestClass]
    public class MensajeMantenimientoControllerTest
    {
        private IMensajeMantenimientoServicio _mensajeMantenimientoServicio;
        private IAutorizacionServicios _autorizacionServicios;

        private MensajeMantenimientoController _mensajeMantenimientoController;

        [TestInitialize]
        public void InitTests()
        {
            var container = Config.UnityConfig.Container;
            _mensajeMantenimientoServicio = container.Resolve<IMensajeMantenimientoServicio>();
            _autorizacionServicios = container.Resolve<IAutorizacionServicios>();
            _mensajeMantenimientoController = new MensajeMantenimientoController(
                _autorizacionServicios,
                _mensajeMantenimientoServicio
            );
        }

        [TestMethod]
        public async Task CrearActualizarTest()
        {
            var parametros = new ParametrosMensajeMantenimiento
            {
                ParametrosDto = new ParametrosDto(),
                FiltroDto = new MensajeMantenimientoFiltroDto(),
                MensajeMantenimientoDto = new MensajeMantenimientoDto()
            };

            var mensajeDto = (OkNegotiatedContentResult<MensajeMantenimientoDto>) await _mensajeMantenimientoController.CrearActualizar(parametros);
            Assert.IsTrue(mensajeDto.Content.Id > 0);
            Assert.IsTrue(mensajeDto.Content.Roles.Count > 0);
        }

        [TestMethod]
        public async Task EliminarMensajeTest()
        {
            var parametros = new ParametrosMensajeMantenimiento
            {
                ParametrosDto = new ParametrosDto(),
                FiltroDto = new MensajeMantenimientoFiltroDto(),
                MensajeMantenimientoDto = new MensajeMantenimientoDto()
            };

            var resultado = (OkResult) await _mensajeMantenimientoController.EliminarMensaje(parametros);
            Assert.IsNotNull(resultado);
        }

        [TestMethod]
        public async Task ObtenerListaMensajesTest()
        {
            var idsFiltro = new int?[] { 1, 2 };
            var parametros = new ParametrosMensajeMantenimiento
            {
                ParametrosDto = new ParametrosDto(),
                FiltroDto = new MensajeMantenimientoFiltroDto { Ids = idsFiltro },
                MensajeMantenimientoDto = new MensajeMantenimientoDto()
            };

            var resultado = (OkNegotiatedContentResult<IEnumerable<MensajeMantenimientoDto>>)await _mensajeMantenimientoController.ObtenerListaMensajes(parametros);
            Assert.AreEqual(idsFiltro.Length, resultado.Content.Count());
        }
    }
}
