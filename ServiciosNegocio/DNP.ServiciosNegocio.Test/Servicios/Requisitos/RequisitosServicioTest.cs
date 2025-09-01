using DNP.ServiciosNegocio.Test.Configuracion;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Unity;

namespace DNP.ServiciosNegocio.Test.Servicios.Requisitos
{
    using System;
    using System.Configuration;
    using System.Data.Entity.Core.Objects;
    using Comunes.Dto;
    using Comunes.Dto.Formulario;
    using Dominio.Dto.Requisitos;
    using Mock;
    using Moq;
    using Persistencia.Implementaciones.Requisitos;
    using Persistencia.Interfaces;
    using Persistencia.Modelo;
    using ServiciosNegocio.Servicios.Implementaciones.Requisitos;
    using ServiciosNegocio.Servicios.Implementaciones.Transversales;
    using ServiciosNegocio.Servicios.Interfaces.Requisitos;

    [TestClass]
    public class RequisitosServicioTest
    {
        private IRequisitosServicio _requisitosServicios;
        private string BpinBase { get; set; }
        private Guid NivelId { get; set; }
        private Guid InstanciaId { get; set; }
        private Guid FormularioId { get; set; }

        private ParametrosConsultaDto _parametrosConsulta;
        private readonly Mock<MGAWebContexto> _mockContext = new Mock<MGAWebContexto>();
        private readonly Mock<IContextoFactory> _mockContextFactory = new Mock<IContextoFactory>();
        [TestInitialize]
        public void Init()
        {
            _parametrosConsulta = new ParametrosConsultaDto();

            BpinBase = "2017184790011";
            NivelId = Guid.NewGuid();
            InstanciaId = Guid.NewGuid();
            FormularioId = Guid.NewGuid();

            _parametrosConsulta.Bpin = BpinBase;
            _parametrosConsulta.IdNivel = NivelId;
            _parametrosConsulta.InstanciaId = InstanciaId;
            _parametrosConsulta.FormularioId = FormularioId;

            var contenedor = UnityConfig.Container;
            _requisitosServicios = contenedor.Resolve<IRequisitosServicio>();
            var mockRequisitosAdicionales = new Mock<ObjectResult<uspGetPreguntasRequisitosAdicionales_Result>>();
            var objetoRetornoRequisitosAdicionales = new uspGetPreguntasRequisitosAdicionales_Result();
            mockRequisitosAdicionales.SetupReturn(objetoRetornoRequisitosAdicionales);

            _mockContext.Setup(mc => mc.uspGetPreguntasRequisitosAdicionales(BpinBase, NivelId, InstanciaId, FormularioId)).Returns(mockRequisitosAdicionales.Object);
            _mockContextFactory.Setup(mcf => mcf.CrearContextoConConexion(ConfigurationManager.ConnectionStrings["MGAWebContexto"].ConnectionString)).Returns(_mockContext.Object);
        }

        [TestMethod]
        public void ObtenerRequisitosServicioExitosoTest()
        {
            _requisitosServicios = new RequisitosServicio(new RequisitosPersistencia(_mockContextFactory.Object), new AuditoriaServicios());
            var result = _requisitosServicios.Obtener(_parametrosConsulta);
            Assert.IsTrue(result.ListadoAtributos.Count > 0);
        }
        [TestMethod]
        public void ObtenerRequisitosServicioNoExitosoTest()
        {
            _requisitosServicios = new RequisitosServicio(new RequisitosPersistencia(_mockContextFactory.Object), new AuditoriaServicios());
            _parametrosConsulta.Bpin = string.Empty;
            var result = _requisitosServicios.Obtener(_parametrosConsulta);
            Assert.IsFalse(result.ListadoAtributos.Count > 0);
        }
        [TestMethod]
        public void RequisitosServicio_Guardar_MarcadoComoNoTemporal_InsertaRegistro()
        {
            //Escenario: los parametros de insercion son validos y ademas viene marcado como guardado definitivo
            var parametrosGuardarProducto = new ParametrosGuardarDto<ServicioAgregarRequisitosDto>
            {
                Contenido = new ServicioAgregarRequisitosDto()
            };

            var parametrosAuditoria = new ParametrosAuditoriaDto();

            //Ejecucion
            _requisitosServicios.Guardar(parametrosGuardarProducto, parametrosAuditoria, false);
        }
    }
}