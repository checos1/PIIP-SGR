namespace DNP.ServiciosNegocio.Test.Servicios.Transferencias
{
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using Comunes.Dto;
    using Configuracion;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Persistencia.Implementaciones;
    using Persistencia.Implementaciones.Formulario;
    using Persistencia.Implementaciones.Genericos;
    using Persistencia.Implementaciones.Transferencias;
    using Persistencia.Interfaces.Formulario;
    using Persistencia.Interfaces.Genericos;
    using Persistencia.Interfaces.Transferencias;
    using Persistencia.Modelo;
    using ServiciosNegocio.Servicios.Implementaciones.Formulario;
    using ServiciosNegocio.Servicios.Implementaciones.Transferencias;
    using ServiciosNegocio.Servicios.Implementaciones.Transversales;
    using ServiciosNegocio.Servicios.Interfaces.Transversales;
    using Unity;

    [TestClass]
    public class TransferenciaServicioTest
    {
        private ITransferenciaPersistencia TransferenciaPersistencia { get; set; }
        private TransferenciaServicio TransferenciaServicios { get; set; }
        private ParametrosAuditoriaDto ParametrosAuditoriaDto { get; set; }
        private IAuditoriaServicios AuditoriaServicio { get; set; }

        [TestInitialize]
        public void Init()
        {

            var contenedor = UnityConfig.Container;
            ParametrosAuditoriaDto = new ParametrosAuditoriaDto()
            {
                Usuario = "jdelgado",
                Ip = "127.0.0.1"
            };

            //TransferenciaPersistencia = new TransferenciaPersistencia(new ContextoFactory());
            //AuditoriaServicio = new AuditoriaServicios();
            TransferenciaPersistencia = contenedor.Resolve<ITransferenciaPersistencia>();
            AuditoriaServicio = contenedor.Resolve<IAuditoriaServicios>();

            TransferenciaServicios = new TransferenciaServicio(TransferenciaPersistencia, AuditoriaServicio);
        }

        [TestMethod]
        public void TransferenciaServicioTest_Valido()
        {
            var resultado = TransferenciaServicios.IdentificarEntidadDestino(20214, 105, ParametrosAuditoriaDto);
            Assert.IsNotNull(resultado);
            Assert.IsTrue(string.IsNullOrEmpty(resultado.MensajeError));
        }


        [TestMethod]
        public void TransferenciaServicioTest_InValido()
        {
            var resultado = TransferenciaServicios.IdentificarEntidadDestino(0, 105, ParametrosAuditoriaDto);
            Assert.IsNotNull(resultado);
            Assert.IsFalse(string.IsNullOrEmpty(resultado.MensajeError));
        }
    }
}
