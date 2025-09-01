namespace DNP.Backbone.Web.API.Test.WebApi
{
    using Comunes.Dto;    
    using DNP.Backbone.Dominio.Dto.SGP;
    using DNP.Backbone.Dominio.Dto.SGP.Viabilidad;
    using DNP.Backbone.Servicios.Interfaces.SGP;
    using DNP.Backbone.Web.API.Controllers.SGP;
    using Microsoft.Practices.Unity;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Servicios.Interfaces.Autorizacion;
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Security.Principal;
    using System.Web.Http.Results;

    [TestClass]
    public class SGPViabilidadControllerTest
    {
        private ISGPViabilidadServicios _sgpServicios;
        private IAutorizacionServicios _autorizacionServicios;
        private SGPViabilidadController _sgpViabilidadController;

        [TestInitialize]
        public void Init()
        {
            _sgpServicios = Config.UnityConfig.Container.Resolve<ISGPViabilidadServicios>();
            _autorizacionServicios = Config.UnityConfig.Container.Resolve<IAutorizacionServicios>();
            _sgpViabilidadController = new SGPViabilidadController(_sgpServicios, _autorizacionServicios);
            _sgpViabilidadController.ControllerContext.Request = new HttpRequestMessage();
            _sgpViabilidadController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _sgpViabilidadController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:Backbone");
            _sgpViabilidadController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });
        }

        [TestMethod]
        public void LeerInformacionGeneral_Ok()
        {
            var proyectoId = 582685;
            var instanciaId = new Guid("b04c24aa-8efe-43d8-aca5-ce6061f29113");
            var tipoConceptoViabilidadCode = "VIABILIDAD";
            var actionResult = _sgpViabilidadController.SGPViabilidadLeerInformacionGeneral(proyectoId, instanciaId, tipoConceptoViabilidadCode).Result;
            
            var contentResult = actionResult as OkNegotiatedContentResult<LeerInformacionGeneralViabilidadDto>;

            Assert.IsNotNull(contentResult);
            Assert.IsNotNull(contentResult.Content);
        }

        [TestMethod]
        public void LeerParametricasViabilidadTest()
        {

            var proyectoId = 582685;
            var nivel = new Guid("12263539-0e2f-42cc-bad3-2c14cfa9dcb3");
            var actionResult = _sgpViabilidadController.SGPViabilidadLeerParameticas(proyectoId, nivel).Result;
            
            var contentResult = actionResult as OkNegotiatedContentResult<List<LeerParametricasViabilidadDto>>;

            Assert.IsNotNull(contentResult);
            Assert.IsNotNull(contentResult.Content);
        }

        [TestMethod]
        public void SGPAcuerdoLeerProyectoTest()
        {

            var proyectoId = 582686;
            var nivel = new Guid("2FED53C9-F3C3-4113-8D53-2BCC68A933C3");
            var actionResult = _sgpViabilidadController.SGPAcuerdoLeerProyecto(proyectoId, nivel).Result;

            var contentResult = actionResult as OkNegotiatedContentResult<List<LstAcuerdoSectorClasificadorDto>>;

            Assert.IsNotNull(contentResult);
            Assert.IsNotNull(contentResult.Content);
        }

        [TestMethod]
        public void SGPProyectosLeerListasTest()
        {

            var proyectoId = 582686;
            var nivel = new Guid("2FED53C9-F3C3-4113-8D53-2BCC68A933C3");
            var nombreLista = "ListaPreviosDatosInicialesEntidades";
            var actionResult = _sgpViabilidadController.SGPProyectosLeerListas(nivel, proyectoId, nombreLista).Result;

            var contentResult = actionResult as OkNegotiatedContentResult<List<ListaDto>>;

            Assert.IsNotNull(contentResult);
            Assert.IsNotNull(contentResult.Content);
        }

        [TestMethod]
        public void GuardarInformacionViabilidadTest()
        {
            var data = new InformacionBasicaViabilidadSGPDto
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

            var actionResult = _sgpViabilidadController.SGPViabilidadGuardarInformacionBasica(data).Result;

            var contentResult = actionResult as OkNegotiatedContentResult<ResultadoProcedimientoDto>;

            Assert.IsNotNull(contentResult);
            Assert.IsNotNull(contentResult.Content);
        }
    }
}
