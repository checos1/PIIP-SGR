using DNP.Backbone.Servicios.Interfaces.Autorizacion;
using DNP.Backbone.Dominio.Dto.SGP.Viabilidad;
using DNP.Backbone.Servicios.Interfaces.SGR;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using DNP.Backbone.Dominio.Dto.SGR.Viabilidad;

namespace DNP.Backbone.Test.Servicios
{
    /// <summary>
    /// Summary description for TramiteServicioTest
    /// </summary>
    [TestClass]
    public class SGPViabilidadServicioTest
    {
        private ISGRViabilidadServicios _sgpViabilidadServicios;

        [TestInitialize]
        public void Init()
        {
            _sgpViabilidadServicios = Config.UnityConfig.Container.Resolve<ISGRViabilidadServicios>();
        }

        [TestMethod]
        public void SGPViabilidadLeerInformacionGeneralTest_AntesGuardar()
        {

            var proyectoId = 582685;
            var instanciaId = new Guid("b04c24aa-8efe-43d8-aca5-ce6061f29113");
            var usuarioDNP = "usuariodnp";
            var tipoConceptoViabilidadCode = "VIABILIDAD";
            var actionResult = _sgpViabilidadServicios.SGR_Viabilidad_LeerInformacionGeneral(proyectoId, instanciaId, usuarioDNP, tipoConceptoViabilidadCode).Result;
            Assert.IsTrue(actionResult.Id > 0);
        }

        [TestMethod]
        public void LeerInformacionBasicaViabilidadTest_DespuesGuardar()
        {

            var proyectoId = 582685;
            var instanciaId = new Guid("b04c24aa-8efe-43d8-aca5-ce6061f29113");
            var usuarioDNP = "CC7012345678907";
            var tipoConceptoViabilidadCode = "VIABILIDAD";
            var actionResult = _sgpViabilidadServicios.SGR_Viabilidad_LeerInformacionGeneral(proyectoId, instanciaId, usuarioDNP, tipoConceptoViabilidadCode).Result;
            Assert.IsTrue(actionResult.RegionSgrId > 0);
        }

        [TestMethod]
        public void LeerParametricasViabilidadSGPTest()
        {

            var proyectoId = 582685;
            var nivel = new Guid("12263539-0e2f-42cc-bad3-2c14cfa9dcb3");
            var usuarioDNP = "CC7012345678907";
            var actionResult = _sgpViabilidadServicios.SGR_Viabilidad_LeerParametricas(proyectoId, nivel, usuarioDNP).Result;
            Assert.IsTrue(actionResult.Count > 0);
        }

        [TestMethod]
        public void GuardarInformacionViabilidadTest()
        {
            var data = new InformacionBasicaViabilidadDto
            {
                ProyectoId = 582685,
                InstanciaId = new Guid("12263539-0e2f-42cc-bad3-2c14cfa9dcb3"),
                CategoriasProyecto = "1,2",
                RegionSgr = 1,
                SectorApoyo1 = 1,
                SectorApoyo2 = 2,
                ValorInterventoria = 1000,
                ValorApoyoSupervision = 100000,
                AlcanceEspacial = "Cumple",
                Poblacion = "No cumple",
                NecesidadesSocioCulturales = "No aplica",
                TipoConceptoViabilidadId = 1
            };

            var usuarioDNP = "usuariodnp";
            var actionResult = _sgpViabilidadServicios.SGR_Viabilidad_GuardarInformacionBasica(data, usuarioDNP).Result;
            Assert.IsTrue(actionResult.Exito);
        }
    }
}
