namespace DNP.Backbone.Web.API.Test.WebApi
{
    using DNP.Backbone.Comunes.Dto;
    using DNP.Backbone.Dominio.Dto;
    using DNP.Backbone.Dominio.Dto.Flujos;
    using DNP.Backbone.Servicios.Interfaces.ServiciosNegocio;
    using DNP.Backbone.Web.API.Controllers.Flujos;
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
    public class FlujoControllerTest
    {
        private IFlujoServicios _flujoServicios;
        private FlujoController _flujoController;
        private IAutorizacionServicios _autorizacionServicios;

        [TestInitialize]
        public void Init()
        {
            _flujoServicios = Config.UnityConfig.Container.Resolve<IFlujoServicios>();
            _autorizacionServicios = Config.UnityConfig.Container.Resolve<IAutorizacionServicios>();

            _flujoController = new FlujoController(_flujoServicios, _autorizacionServicios);
            _flujoController.ControllerContext.Request = new HttpRequestMessage();
            _flujoController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _flujoController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:Backbone");
            _flujoController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });
        }


        [TestMethod]
        public void GenerarInstancias_Ok()
        {
            var parametros = new ParametrosInstanciaDto
            {
                IdUsuarioDNP = "jdelgado",
                FlujoId = new Guid("bc154cba-50a5-4209-81ce-7c0ff0aec2ce"),
                RolId = Guid.Parse("4fe0a3de-0b14-45ed-9137-248bd206a418"),
                Proyectos = new List<Dominio.Dto.Proyecto.NegocioDto>() { new Dominio.Dto.Proyecto.NegocioDto() }
            };
            
            var actionResult = _flujoController.GenerarInstancias(parametros).Result;
            var contentResult = actionResult as OkNegotiatedContentResult<List<DNP.Backbone.Dominio.Dto.InstanciaResultado>>;

            Assert.IsNotNull(contentResult);
            Assert.IsNotNull(contentResult.Content);
        }

        [TestMethod]
        public void ObtenerLog()
        {
            var result = _flujoController.ObtenerLog(Guid.Parse("BE35A03F-1D13-476C-B14E-96524FACBE1F"));
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void CrearInstancia_OK()
        {
            
            ParametrosInstanciaFlujoDto parametrosInstanciaDto = new ParametrosInstanciaFlujoDto();
            parametrosInstanciaDto.UsuarioId = "CC202002";
            parametrosInstanciaDto.RolId = new Guid("DA595AA3-CF59-46D3-A22A-0D96DA5C7371");
            parametrosInstanciaDto.ObjetoId = "EJ-TP-TO-120101-0150";
            parametrosInstanciaDto.TipoObjetoId = new Guid("9C5EF8C1-DA05-48B9-BA29-00C9EFD7A774");
            parametrosInstanciaDto.FlujoId = new Guid("23EE582A-08C2-43F5-A60C-425608FF9D81");
            parametrosInstanciaDto.Descripcion = "Prueba Definitiva del Trámite de Traslado Ordinario";
                
            var result = _flujoController.CrearInstancia(parametrosInstanciaDto);
            Assert.IsNotNull(result);


         
        }

        [TestMethod]
        public void ObtenerPermisosFlujosPorAplicacionYRoles_OK()
        {
            FiltroConsultaOpcionesDto filtroConsulta = new FiltroConsultaOpcionesDto();
            filtroConsulta.IdAplicacion = "App:ipp";
            var result = _flujoController.ObtenerFlujosPorRoles(filtroConsulta);
            Assert.IsNotNull(result);


        }

        [TestMethod]
        public void ConsultarProyectosEntidadesSinInstanciasActivas_OK()
        {
            ParametrosProyectosFlujosDto parametros = new ParametrosProyectosFlujosDto();
            parametros.CodigoBpin = "20220000000150";
            parametros.IdUsuarioDNP = "CC202002";
       
           
            var result = _flujoController.ConsultarProyectosEntidadesSinInstanciasActivas(parametros);
            Assert.IsNotNull(result);


        }

    }
}
