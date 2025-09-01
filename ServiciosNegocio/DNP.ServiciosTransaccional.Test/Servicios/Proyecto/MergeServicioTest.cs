namespace DNP.ServiciosTransaccional.Test.Servicios.Proyecto
{
    using Configuracion;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using ServiciosNegocio.Comunes.Dto;
    using ServiciosNegocio.Comunes.Dto.Formulario;
    using ServiciosNegocio.Comunes.Excepciones;
    using ServiciosNegocio.Dominio.Dto.Transferencias;
    using ServiciosTransaccional.Servicios.Interfaces.Proyectos;
    using Unity;

    [TestClass]
    public class MergeServicioTest
    {
        private IMergeServicio _mergeServicio;

        [TestInitialize]
        public void Init()
        {
            var contenedor = UnityConfig.Container;
            _mergeServicio = contenedor.Resolve<IMergeServicio>();
        }

        [TestMethod]
        public void AplicarMergeProyectoServicioTest_Valido()
        {
            var parametrosGuardar = new ParametrosGuardarDto<ObjetoNegocio>
            {
                Contenido = new ObjetoNegocio
                {
                    ObjetoNegocioId = "20214"
                }
            };

            var parametrosAuditoria = new ParametrosAuditoriaDto();
            var resultado = _mergeServicio.AplicarMerge(parametrosGuardar, parametrosAuditoria);
            Assert.IsNotNull(resultado);
        }

        [TestMethod]
        public void AplicarMergeProyectoServicioTest_NoValido()
        {
            var parametrosGuardar = new ParametrosGuardarDto<ObjetoNegocio>
            {
                Contenido = new ObjetoNegocio
                {
                    ObjetoNegocioId = "1007003"
                }
            };

            var parametrosAuditoria = new ParametrosAuditoriaDto();
            var resultado = _mergeServicio.AplicarMerge(parametrosGuardar, parametrosAuditoria);
            Assert.IsFalse((bool)resultado);
        }
    }
}
