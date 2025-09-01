namespace DNP.ServiciosTransaccional.Test.Servicios.Transferencias
{
    using Configuracion;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using ServiciosNegocio.Comunes.Dto;
    using ServiciosNegocio.Comunes.Dto.Formulario;
    using ServiciosNegocio.Dominio.Dto.Transferencias;
    using ServiciosTransaccional.Servicios.Interfaces.Transferencias;
    using Unity;

    [TestClass]
    public class CreacionBpinTest
    {
        private ICreacionBpinServicio _creacionBpinServicio;

        [TestInitialize]
        public void Init()
        {
            var contenedor = UnityConfig.Container;
            _creacionBpinServicio = contenedor.Resolve<ICreacionBpinServicio>();
        }

        [TestMethod]
        public void ValidacionTransferencia_Test_Valido()
        {
            var parametrosGuardar = new ParametrosGuardarDto<ObjetoNegocio>
                                    {
                                        Contenido = new ObjetoNegocio()
                                                    {
                                                        ObjetoNegocioId = "20214"
                                                    }
                                        
                                    };

            var parametrosAuditoria = new ParametrosAuditoriaDto();
            var resultado = _creacionBpinServicio.Guardar(parametrosGuardar, parametrosAuditoria);
            Assert.IsTrue((bool)resultado);

        }

        [TestMethod]
        public void ValidacionTransferencia_Test_Invalido()
        {
            var parametrosGuardar = new ParametrosGuardarDto<ObjetoNegocio>
                                    {
                                        Contenido = new ObjetoNegocio()
                                                    {
                                                        ObjetoNegocioId = "1007003"
                                                    }

                                    };

            var parametrosAuditoria = new ParametrosAuditoriaDto();
            var resultado = _creacionBpinServicio.Guardar(parametrosGuardar, parametrosAuditoria);
            Assert.IsFalse((bool)resultado);

        }
    }
}
