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
    public class BpinServicioTest
    {
        private IBpinServicio _bpinServicio;

        [TestInitialize]
        public void Init()
        {
            var contenedor = UnityConfig.Container;
            _bpinServicio = contenedor.Resolve<IBpinServicio>();
        }

        [TestMethod]
        public void GenerarBPINProyectoServicioTest_Valido()
        {
            var parametrosGuardar = new ParametrosGuardarDto<ObjetoNegocio>
            {
                Contenido = new ObjetoNegocio
                {
                    ObjetoNegocioId = "20214"
                }
            };

            var parametrosAuditoria = new ParametrosAuditoriaDto();
            var resultado = _bpinServicio.GenerarBPIN(parametrosGuardar, parametrosAuditoria);
            Assert.IsNotNull(resultado);
        }

        [TestMethod]
        public void GenerarBPINProyectoServicioTest_NoValido()
        {
            var parametrosGuardar = new ParametrosGuardarDto<ObjetoNegocio>
            {
                Contenido = new ObjetoNegocio
                {
                    ObjetoNegocioId = "1007003"
                }
            };

            var parametrosAuditoria = new ParametrosAuditoriaDto();
            var resultado = _bpinServicio.GenerarBPIN(parametrosGuardar, parametrosAuditoria);
            Assert.IsFalse((bool)resultado);
        }

        [TestMethod]
        public void GenerarBPINProyectoSgrServicioTest_Valido()
        {
            var parametrosGuardar = new ParametrosGuardarDto<ObjetoNegocio>
            {
                Contenido = new ObjetoNegocio
                {
                    ObjetoNegocioId = "20214"
                }
            };

            var parametrosAuditoria = new ParametrosAuditoriaDto();
            var resultado = _bpinServicio.GenerarBPINSgr(parametrosGuardar, parametrosAuditoria);
            Assert.IsNotNull(resultado);
        }

        [TestMethod]
        public void GenerarBPINProyectoSgrServicioTest_NoValido()
        {
            var parametrosGuardar = new ParametrosGuardarDto<ObjetoNegocio>
            {
                Contenido = new ObjetoNegocio
                {
                    ObjetoNegocioId = "1007003"
                }
            };

            var parametrosAuditoria = new ParametrosAuditoriaDto();
            var resultado = _bpinServicio.GenerarBPINSgr(parametrosGuardar, parametrosAuditoria);
            Assert.IsFalse((bool)resultado);
        }
    }
}
