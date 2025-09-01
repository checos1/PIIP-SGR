namespace DNP.ServiciosTransaccional.Test.Servicios.Proyecto
{
    using Configuracion;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using ServiciosNegocio.Comunes.Dto;
    using ServiciosNegocio.Comunes.Dto.Formulario;
    using ServiciosNegocio.Comunes.Excepciones;
    using ServiciosNegocio.Dominio.Dto.Transferencias;
    using ServiciosTransaccional.Servicios.Interfaces.Proyectos;
    using System;
    using System.Threading.Tasks;
    using Unity;

    [TestClass]
    public class ProyectoServicioTest
    {
        private IProyectoServicio _proyectoServicio;

        [TestInitialize]
        public void Init()
        {
            var contenedor = UnityConfig.Container;
            _proyectoServicio = contenedor.Resolve<IProyectoServicio>();
        }

        [TestMethod]
        public void ActualizarEstadoProyectoServicioTest_Valido()
        {
            var parametrosGuardar = new ParametrosGuardarDto<ObjetoNegocio>
                                    {
                                        Contenido = new ObjetoNegocio
                                        {
                                            ObjetoNegocioId = "ABC123"
                                        }
                                    };

            var parametrosAuditoria = new ParametrosAuditoriaDto();
            var resultado = _proyectoServicio.ActualizarEstado(parametrosGuardar, parametrosAuditoria);
            Assert.IsNotNull(resultado);
        }

        [TestMethod]
        [ExpectedException(typeof(ServiciosNegocioException))]
        public void ActualizarEstadoProyectoServicioTest_NoValido()
        {
            var parametrosGuardar = new ParametrosGuardarDto<ObjetoNegocio>
                                    {
                                        Contenido = new ObjetoNegocio
                                                    {
                                                        ObjetoNegocioId = "BPIN"
                                                    }
                                    };

            var parametrosAuditoria = new ParametrosAuditoriaDto();
            _proyectoServicio.ActualizarEstado(parametrosGuardar, parametrosAuditoria);
        }

        [TestMethod]
        public void ActualizarEstadoProyectoSGRServicioTest_Valido()
        {
            var parametrosGuardar = new ParametrosGuardarDto<ObjetoNegocio>
            {
                Contenido = new ObjetoNegocio
                {
                    ObjetoNegocioId = "ABC123"
                }
            };

            var parametrosAuditoria = new ParametrosAuditoriaDto();
            var resultado = _proyectoServicio.ActualizarEstadoSGR(parametrosGuardar, parametrosAuditoria);
            Assert.IsNotNull(resultado);
        }

        [TestMethod]
        [ExpectedException(typeof(ServiciosNegocioException))]
        public void ActualizarEstadoProyectoSGRServicioTest_NoValido()
        {
            var parametrosGuardar = new ParametrosGuardarDto<ObjetoNegocio>
            {
                Contenido = new ObjetoNegocio
                {
                    ObjetoNegocioId = "BPIN"
                }
            };

            var parametrosAuditoria = new ParametrosAuditoriaDto();
            _proyectoServicio.ActualizarEstadoSGR(parametrosGuardar, parametrosAuditoria);
        }

        [TestMethod]
        public void ActualizarNombreProyectoServicioTest_Valido()
        {
            var parametrosGuardar = new ParametrosGuardarDto<ObjetoNegocio>
            {
                Contenido = new ObjetoNegocio
                {
                    ObjetoNegocioId = "ABC123"
                }
            };

            var parametrosAuditoria = new ParametrosAuditoriaDto();
            var resultado = _proyectoServicio.ActualizarNombre(parametrosGuardar, parametrosAuditoria);
            Assert.IsNotNull(resultado);
        }

        [TestMethod]
        [ExpectedException(typeof(ServiciosNegocioException))]
        public void ActualizarNombreProyectoServicioTest_NoValido()
        {
            var parametrosGuardar = new ParametrosGuardarDto<ObjetoNegocio>
            {
                Contenido = new ObjetoNegocio
                {
                    ObjetoNegocioId = "BPIN"
                }
            };

            var parametrosAuditoria = new ParametrosAuditoriaDto();
            _proyectoServicio.ActualizarNombre(parametrosGuardar, parametrosAuditoria);
        }

        [TestMethod]
        public async Task GenerarMensajeEstadoProyectoTest_Valido()
        {
            var instanciaId = Guid.Parse("50bccf35-11d6-4308-a434-a0deaa5a2595");
            string usuario = "CC7012345678907";

            var resultado = await _proyectoServicio.SGR_Proyectos_GenerarMensajeEstadoProyecto(instanciaId, usuario);
            Assert.IsNotNull(resultado);
        }

        [TestMethod]
        public async Task SGR_CTUS_CrearInstanciaCtusSGRTest_Valido()
        {
            string usuario = "CC1122334455";

            var objeto = new ObjetoNegocio
            {
                InstanciaId = "46236cfd-716b-41e8-8127-5929dad4f698",
                ObjetoNegocioId = "617253",
                FlujoId = "525b5ffd-c2cc-9c26-1094-d35afbc82806",
                IdAccion = "f0a0619d-f3eb-4d25-90a0-417bfb47caab",
                IdRol = "1dd225f4-5c34-4c55-b11d-e5856a68839b"
            };

            var resultado = await _proyectoServicio.SGR_CTUS_CrearInstanciaCtusSGR(objeto, usuario);
            Assert.IsNotNull(resultado);
        }

        [TestMethod]
        public async Task GenerarFichaViabilidadSGPTest_Valido()
        {
            var parametrosGuardar = new ParametrosGuardarDto<ObjetoNegocio>
            {
                Contenido = new ObjetoNegocio
                {
                    ObjetoNegocioId = "618437",
                    InstanciaId = "d6657a3b-5284-4a5e-bdae-2b9190f14b4f",
                    IdAccion = "27acd15a-660a-4e24-a22b-998b133b6216",
                    IdRol = "0eb96a9d-bf38-4688-a2a8-5541ec54ce3c",
                    FlujoId = "a2b51530-559c-47c4-97d4-e433619268aa",
                    NivelId = "d8c0c353-3ea7-4bc5-9ca6-cc8aa0f7bb8b"
                }
            };

            var parametrosAuditoria = new ParametrosAuditoriaDto
            {
                Usuario = "CC7012345678907"
            };

            var resultado = await _proyectoServicio.GenerarFichaViabilidadSGP(parametrosGuardar, parametrosAuditoria);
            Assert.IsNotNull(resultado);
        }
    }
}
