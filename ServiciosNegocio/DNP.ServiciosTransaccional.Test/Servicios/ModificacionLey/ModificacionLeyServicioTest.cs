using Microsoft.VisualStudio.TestTools.UnitTesting;
using DNP.ServiciosNegocio.Comunes.Dto;
using DNP.ServiciosNegocio.Comunes.Dto.Formulario;
using DNP.ServiciosNegocio.Comunes.Dto.ObjetosNegocio;
using DNP.ServiciosNegocio.Comunes.Excepciones;
using DNP.ServiciosNegocio.Test.Configuracion;
using DNP.ServiciosTransaccional.Servicios.Interfaces.ModificacionLey;
using Unity;

namespace DNP.ServiciosTransaccional.Test.Servicios.ModificacionLey
{
    public class ModificacionLeyServicioTest
    {
        private IModificacionLeyServicio _modificacionLeyServicio;

        [TestInitialize]
        public void Init()
        {
            var contenedor = UnityConfig.Container;
            _modificacionLeyServicio = contenedor.Resolve<IModificacionLeyServicio>();
        }

        [TestMethod]
        public void ActualizarValoresPoliticasMLServicioTest_Valido()
        {
            var parametrosGuardar = new ParametrosGuardarDto<ObjetoNegocio>
            {
                Contenido = new ObjetoNegocio
                {
                    ObjetoNegocioId = "ABC123"
                }
            };

            var parametrosAuditoria = new ParametrosAuditoriaDto();
            var resultado = _modificacionLeyServicio.ActualizarValoresPoliticasML(parametrosGuardar, parametrosAuditoria);
            Assert.IsNotNull(resultado);
        }

        [TestMethod]
        [ExpectedException(typeof(ServiciosNegocioException))]
        public void ActualizarValoresPoliticasMLServicioTest_NoValido()
        {
            var parametrosGuardar = new ParametrosGuardarDto<ObjetoNegocio>
            {
                Contenido = new ObjetoNegocio
                {
                    ObjetoNegocioId = "BPIN"
                }
            };

            var parametrosAuditoria = new ParametrosAuditoriaDto();
            _modificacionLeyServicio.ActualizarValoresPoliticasML(parametrosGuardar, parametrosAuditoria);
        }
    }
}
