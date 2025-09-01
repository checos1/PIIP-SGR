namespace DNP.Backbone.Test.Servicios
{
    using DNP.Backbone.Comunes.Dto;
    using DNP.Backbone.Servicios.Interfaces.Monitoreo;
    using Microsoft.Practices.Unity;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class AlertasConfigServiciosTest
    {
        private IAlertasConfigServicios _alertasConfigServicios;

        [TestInitialize]
        public void Init()
        {
            _alertasConfigServicios = Config.UnityConfig.Container.Resolve<IAlertasConfigServicios>();          
        }

        [TestMethod]
        public void ObtenerAlertasConfig_OK()
        {
            var actionResult = _alertasConfigServicios.ObtenerAlertasConfig(new AlertasConfigFiltroDto {
                ParametrosDto = new ParametrosDto { IdUsuarioDNP = "jdelgado" }
            });
            Assert.IsNotNull(actionResult.Result);
        }

        [TestMethod]
        public void CuandoNoEnvioUsuarioObtenerAlertasConfig_RetornaExcepcion()
        {
            var actionResult = _alertasConfigServicios.ObtenerAlertasConfig(new AlertasConfigFiltroDto());
            Assert.IsNull(actionResult.Result);
        }

        [TestMethod]
        public void EliminarAlertasConfig_OK()
        {
            var actionResult = _alertasConfigServicios.EliminarAlertasConfig(new AlertasConfigFiltroDto
            {
                AlertasConfigDto =
                new AlertasConfigDto { Id = 1 }
            });
            Assert.IsNotNull(actionResult.Result);
        }

        [TestMethod]
        public void CuandoNoEnvioIdentificadorEliminarAlertasConfig_RetornaExcepcion()
        {
            var actionResult = _alertasConfigServicios.EliminarAlertasConfig(new AlertasConfigFiltroDto());
            Assert.IsNull(actionResult.Result);
        }

        [TestMethod]
        public void CrearActualizarAlertasConfig_OK()
        {
            var actionResult = _alertasConfigServicios.CrearActualizarAlertasConfig(new AlertasConfigFiltroDto
            {
                AlertasConfigDto =
                new AlertasConfigDto { MensajeAlerta = "teste", NombreAlerta = "teste" }
            });
            Assert.IsNotNull(actionResult.Result);
        }

        [TestMethod]
        public void CuandoNoEnvioNombreAlertaCrearActualizarAlertasConfig_RetornaExcepcion()
        {
            var actionResult = _alertasConfigServicios.CrearActualizarAlertasConfig(new AlertasConfigFiltroDto
            {
                AlertasConfigDto =
                new AlertasConfigDto()
            });
            Assert.IsNull(actionResult.Result);
        }

    }
}
