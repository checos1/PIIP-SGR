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
    public class TransferenciaServicioTest
    {
        private ITransferenciaServicio _transferenciaServicio;

        [TestInitialize]
        public void Init()
        {
            var contenedor = UnityConfig.Container;
            _transferenciaServicio = contenedor.Resolve<ITransferenciaServicio>();
        }

        [TestMethod]
        public void TransferenciaServicioTest_Valido()
        {
            //Escenario: los parametros de insercion son validos y ademas viene marcado como guardado definitivo
            var parametrosGuardar = new ParametrosGuardarDto<ObjetoNegocio>
                                        {
                                            Contenido = new ObjetoNegocio()
                                        };

            var parametrosAuditoria = new ParametrosAuditoriaDto();

            //Ejecucion
            var resultado = _transferenciaServicio.Guardar(parametrosGuardar, parametrosAuditoria);
            Assert.IsNotNull(resultado);
        }
    }
}
