using DNP.ServiciosNegocio.Test.Configuracion;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Unity;

namespace DNP.ServiciosNegocio.Test.Servicios.Preguntas
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data.Entity;
    using System.Data.Entity.Core.Objects;
    using System.Data.Entity.Infrastructure;
    using Comunes.Dto;
    using Comunes.Dto.Formulario;
    using Comunes.Utilidades;
    using Dominio.Dto.Preguntas;
    using Mock;
    using Moq;
    using Persistencia.Implementaciones.Preguntas;
    using Persistencia.Interfaces;
    using Persistencia.Modelo;
    using ServiciosNegocio.Servicios.Implementaciones.Preguntas;
    using ServiciosNegocio.Servicios.Implementaciones.Transversales;
    using ServiciosNegocio.Servicios.Interfaces.Preguntas;

    [TestClass]
    public class PreguntasServicioTest
    {
        private IPreguntasServicio _preguntasServicios;
        private string BpinBase { get; set; }
        private Guid NivelId { get; set; }
        private Guid InstanciaId { get; set; }
        private Guid FormularioId { get; set; }

        private ParametrosConsultaDto _parametrosConsulta;
        private readonly Mock<MGAWebContexto> _mockContext = new Mock<MGAWebContexto>();
        private readonly Mock<IContextoFactory> _mockContextFactory = new Mock<IContextoFactory>();
        private ParametrosGuardarDto<ServicioPreguntasDto> _parametrosGuardarProducto;
        private ParametrosAuditoriaDto _parametrosAuditoria;
        [TestInitialize]
        public void Init()
        {
            _parametrosConsulta = new ParametrosConsultaDto();

            BpinBase = "2017011000236";
            NivelId = Guid.NewGuid();
            InstanciaId = Guid.NewGuid();
            FormularioId = Guid.Parse("fd7180ed-b8b3-4c55-b666-9e1158138725");

            _parametrosConsulta.Bpin = BpinBase;
            _parametrosConsulta.IdNivel = NivelId;
            _parametrosConsulta.InstanciaId = InstanciaId;
            _parametrosConsulta.FormularioId = FormularioId;
            _parametrosConsulta.Token = "Basic cGVkcm9waWlwOjIyODkxNTAw";

            var contenedor = UnityConfig.Container;

            _parametrosGuardarProducto = new ParametrosGuardarDto<ServicioPreguntasDto>
            {
                Contenido = new ServicioPreguntasDto()
            };

            _parametrosAuditoria = new ParametrosAuditoriaDto();

            _preguntasServicios = contenedor.Resolve<IPreguntasServicio>();

            var mockPreguntasGenerales = new Mock<ObjectResult<uspGetObtenerPreguntasGenerales_Result>>();
            var mockPreguntasEspecificas = new Mock<ObjectResult<uspGetObtenerPreguntasEspecificas_Result>>();
            var mockAgregarPreguntasDto = new Mock<ObjectResult<AgregarPreguntasDto>>();

            var objetoRetornoPreguntasGenerales = new uspGetObtenerPreguntasGenerales_Result();
            var objetoRetornoPreguntasEspecificas = new uspGetObtenerPreguntasEspecificas_Result();
            var objetoAgregarPreguntasDto = new AgregarPreguntasDto();
            ObjectParameter resultado = new ObjectParameter("resultado", typeof(string));
            mockPreguntasGenerales.SetupReturn(objetoRetornoPreguntasGenerales);
            mockPreguntasEspecificas.SetupReturn(objetoRetornoPreguntasEspecificas);
            mockAgregarPreguntasDto.SetupReturn(objetoAgregarPreguntasDto);

            _mockContext.Setup(mc => mc.uspGetObtenerPreguntasGenerales(BpinBase, NivelId, InstanciaId, FormularioId)).Returns(mockPreguntasGenerales.Object);
            _mockContext.Setup(mc => mc.uspGetObtenerPreguntasEspecificas(BpinBase, NivelId, InstanciaId, FormularioId)).Returns(mockPreguntasEspecificas.Object);
            _mockContext.Setup(mc => mc.SqlAgregarPreguntasDto()).Returns(mockAgregarPreguntasDto.Object);

            _mockContext.Setup(mc => mc.uspPostGeneracionCuestionarioPreguntas(JsonUtilidades.ACadenaJson(_parametrosGuardarProducto.Contenido), _parametrosAuditoria.Usuario, true, InstanciaId, FormularioId, resultado)).Returns(1);

            _mockContextFactory.Setup(mcf => mcf.CrearContextoConConexion(ConfigurationManager.ConnectionStrings["MGAWebContexto"].ConnectionString)).Returns(_mockContext.Object);
        }

        //[TestMethod]
        //public void ObtenerPreguntasServicioExitosoTest()
        //{
        //    _preguntasServicios = new PreguntasServicio(new PreguntasPersistencia(_mockContextFactory.Object), new AuditoriaServicios());
        //    var parametrosConsulta = new ParametrosConsultaDto
        //    {
        //        Bpin = "202200000000038",
        //        IdNivel = new Guid("5E03D2F8-BB24-4E72-92E7-4560E04B9F2E"),
        //        InstanciaId = new Guid("44432199-7668-4611-8CBE-DD0D71171296"),
        //        FormularioId = new Guid("5932710F-CDB4-4B20-9435-74216610D482"),
        //        Token = "Basic cGVkcm9waWlwOjIyODkxNTAw"
        //    };

        //    var result = _preguntasServicios.Obtener(parametrosConsulta);
        //    Assert.IsTrue(
        //        (result.PreguntasEspecificas.Count > 0 || result.PreguntasGenerales.Count > 0)
        //        && result.AgregarPreguntasRequisitos.Count > 0
        //    );
        //}
        //[TestMethod]
        //public void ObtenerPreguntasServicioNoExitosoTest()
        //{
        //    _parametrosConsulta.Bpin = string.Empty;
        //    _preguntasServicios = new PreguntasServicio(new PreguntasPersistencia(_mockContextFactory.Object), new AuditoriaServicios());
        //    var result = _preguntasServicios.Obtener(_parametrosConsulta);
        //    Assert.IsFalse(result.PreguntasEspecificas.Count > 0 || result.PreguntasGenerales.Count > 0);
        //}

        [TestMethod]
        public void ObtenerPreguntasPreview___RetornaDto()
        {
            var result = _preguntasServicios.ObtenerPreguntasPreview();
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void PreguntasServicio_Guardar_MarcadoComoTemporal_InsertaRegistro()
        {
            //Ejecucion
            _preguntasServicios.Guardar(_parametrosGuardarProducto, _parametrosAuditoria, true);
        }
        [TestMethod]
        public void PreguntasServicio_Guardar_MarcadoComoNoTemporal_InsertaRegistro()
        {
            //Ejecucion
            _preguntasServicios.Guardar(_parametrosGuardarProducto, _parametrosAuditoria, false);
        }
    }
}