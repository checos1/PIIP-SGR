using DNP.Backbone.Servicios.Interfaces.MensajesMantenimiento;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.Unity;
using DNP.Backbone.Dominio.Dto.MensajeMantenimiento;
using DNP.Backbone.Comunes.Dto;
using DNP.Backbone.Dominio.Filtros.MensajeMantenimiento;

namespace DNP.Backbone.Test.Servicios
{
    [TestClass]
    public class MensajeMantenimientoServicioTest
    {
        private IMensajeMantenimientoServicio _mensajeMantenimientoServicio;

        [TestInitialize]
        public void Init()
        {
            var container = Config.UnityConfig.Container;
            _mensajeMantenimientoServicio = container.Resolve<IMensajeMantenimientoServicio>();
        }

        [TestMethod]
        public void ObtenerListaMensajes_OK()
        {
            var actionResult = _mensajeMantenimientoServicio.ObtenerListaMensajes(new ParametrosMensajeMantenimiento
            {
                ParametrosDto = new ParametrosDto { IdUsuarioDNP = "jdelgado" }
            });

            Assert.IsNotNull(actionResult.Result);
        }

        [TestMethod]
        public void CuandoNoEnvioUsuarioObtenerListaMensajes_RetornaExcepcion()
        {
            var actionResult = _mensajeMantenimientoServicio.ObtenerListaMensajes(new ParametrosMensajeMantenimiento());
            Assert.IsNull(actionResult.Result);
        }

        [TestMethod]
        public void EliminarMensaje_OK()
        {
            var actionResult = _mensajeMantenimientoServicio.EliminarMensaje(new ParametrosMensajeMantenimiento
            {
                FiltroDto = new MensajeMantenimientoFiltroDto { Ids = new int?[] { 1 } }
            });

            Assert.IsNotNull(actionResult);
        }

        [TestMethod]
        public void CuandoNoEnvioIdentificadorEliminarMensaje_RetornaExcepcion()
        {
            var actionResult = _mensajeMantenimientoServicio.EliminarMensaje(new ParametrosMensajeMantenimiento());
            Assert.IsNotNull(actionResult.Exception);
        }

        [TestMethod]
        public void CrearActualizarMensaje_OK()
        {
            var actionResult = _mensajeMantenimientoServicio.CrearActualizarMensaje(new ParametrosMensajeMantenimiento
            {
                ParametrosDto = new ParametrosDto { IdUsuarioDNP = "usuario.test"  },
                MensajeMantenimientoDto = new MensajeMantenimientoDto { NombreMensaje = "Mensaje teste" }
            });

            Assert.IsNotNull(actionResult.Result);
        }

        [TestMethod]
        public void CuandoNoEnvioNombreMensajeCrearActualizarMensaje_RetornaExcepcion()
        {
            var actionResult = _mensajeMantenimientoServicio.CrearActualizarMensaje(new ParametrosMensajeMantenimiento
            {
                MensajeMantenimientoDto = new MensajeMantenimientoDto()
            });

            Assert.IsNull(actionResult.Result);
        }
    }
}
