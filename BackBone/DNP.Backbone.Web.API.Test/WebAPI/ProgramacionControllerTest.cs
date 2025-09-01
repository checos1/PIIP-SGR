using DNP.Autorizacion.Dominio.Dto;
using DNP.Backbone.Comunes.Dto;
using DNP.Backbone.Dominio.Dto.AutorizacionNegocio;
using DNP.Backbone.Dominio.Dto.Nivel;
using DNP.Backbone.Dominio.Dto.Programacion;
using DNP.Backbone.Dominio.Dto.Usuario;
using DNP.Backbone.Servicios.Interfaces.Autorizacion;
using DNP.Backbone.Servicios.Interfaces.Identidad;
using DNP.Backbone.Servicios.Interfaces.Nivel;
using DNP.Backbone.Servicios.Interfaces.Programacion;
using DNP.Backbone.Servicios.Interfaces.ServiciosNegocio;
using DNP.Backbone.Web.API.Controllers;
using DNP.Backbone.Web.API.Controllers.Usuarios;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Principal;
using System.Web.Http.Results;

namespace DNP.Backbone.Web.API.Test.WebAPI
{
    [TestClass]
    public class ProgramacionControllerTest
    {
        private IIdentidadServicios _identidadServicios;
        private IAutorizacionServicios _autorizacionServicios;
        private IProgramacionServicios _programacionServicios;
        private IFlujoServicios _flujosServicios;
        private ProgramacionController _programacionController;

        [TestInitialize]
        public void Init()
        {
            _identidadServicios = Config.UnityConfig.Container.Resolve<IIdentidadServicios>();
            _autorizacionServicios = Config.UnityConfig.Container.Resolve<IAutorizacionServicios>();
            _programacionServicios = Config.UnityConfig.Container.Resolve<IProgramacionServicios>();
            _flujosServicios = Config.UnityConfig.Container.Resolve<IFlujoServicios>();

            _programacionController = new ProgramacionController(_flujosServicios, _autorizacionServicios, _programacionServicios);
            _programacionController.ControllerContext.Request = new HttpRequestMessage();
            _programacionController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _programacionController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:Backbone");
            _programacionController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });
        }

        [TestMethod]
        public void ObtenerProgramaciones_Ok()
        {
            var actionResult =_programacionController.ObtenerProgramacionesPorTipoEntidad("{tipoEntidad: 'Nacional', ProcesoTipo: '0'}");

            Assert.IsNotNull(actionResult.Result);
            Assert.IsTrue((actionResult.Result as OkNegotiatedContentResult<IEnumerable<ProgramacionDto>>).Content.Any());
        }

        [TestMethod]
        public void GuardarProgramaciones_Ok()
        {
            var actionResult = _programacionController.GuardarProgramacion(new Dominio.Dto.Programacion.ProgramacionDto
            {
                FlujoId = Guid.NewGuid(),
                FechaDesde = DateTime.Now,
                FechaHasta = DateTime.Now,
                IdProgramacion = 1,
                TipoEntidad = "Nacional",
            });

            Assert.IsNotNull(actionResult.Result);
            Assert.IsTrue((actionResult.Result as OkNegotiatedContentResult<RespuestaGeneralDto>).Content.Exito);
        }


        [TestMethod]
        public void EliminarProgramacion_Ok()
        {
            var actionResult = _programacionController.EliminarProgramacion(new Dominio.Dto.Programacion.ProgramacionDto
            {
                FechaDesde = DateTime.Now,
                FechaHasta = DateTime.Now,
                IdProgramacion = 1,
                Cerrado = false,
                Creado = false,
                TipoEntidad = "Nacional"
            });

            Assert.IsNotNull(actionResult.Result);
            Assert.IsTrue((actionResult.Result as OkNegotiatedContentResult<RespuestaGeneralDto>).Content.Exito);
        }

        [TestMethod]
        public void ObtenerProgramacionExcepciones_Ok()
        {
            var actionResult = _programacionController.ObtenerProgramacionExcepciones(1);

            Assert.IsNotNull(actionResult.Result);
            Assert.IsTrue((actionResult.Result as OkNegotiatedContentResult<IEnumerable<ProgramacionExcepcionDto>>).Content.Any());
        }

        [TestMethod]
        public void GuardarProgramacionExcepcion_Ok()
        {
            var actionResult = _programacionController.Guardar(new Dominio.Dto.Programacion.ProgramacionExcepcionDto
            {
                FechaDesde = DateTime.Now,
                FechaHasta = DateTime.Now,
                IdProgramacion = 1,
                EntidadId = 1,
                IdProgramacionExcepcion = 0
            });

            Assert.IsNotNull(actionResult.Result);
            Assert.IsTrue((actionResult.Result as OkNegotiatedContentResult<RespuestaGeneralDto>).Content.Exito);
        }

        [TestMethod]
        public void EditarProgramacionExcepcion_Ok()
        {
            var actionResult = _programacionController.EditarExcepcion(new Dominio.Dto.Programacion.ProgramacionExcepcionDto
            {
                FechaDesde = DateTime.Now,
                FechaHasta = DateTime.Now,
                IdProgramacion = 1,
                EntidadId = 1,
                IdProgramacionExcepcion = 1
            });

            Assert.IsNotNull(actionResult.Result);
            Assert.IsTrue((actionResult.Result as OkNegotiatedContentResult<RespuestaGeneralDto>).Content.Exito);
        }

        [TestMethod]
        public void EliminarProgramacionExcepcion_Ok()
        {
            var actionResult = _programacionController.EliminarExcepcion(new Dominio.Dto.Programacion.ProgramacionExcepcionDto
            {
                FechaDesde = DateTime.Now,
                FechaHasta = DateTime.Now,
                IdProgramacion = 1,
                EntidadId = 1,
                IdProgramacionExcepcion = 1
            });

            Assert.IsNotNull(actionResult.Result);
            Assert.IsTrue((actionResult.Result as OkNegotiatedContentResult<RespuestaGeneralDto>).Content.Exito);
        }

        [TestMethod]
        public void ObtenerFujosTramitesNiveles_Ok()
        {
            var actionResult = _programacionController.ObtenerPorIdPadreIdNivelTipo(new Guid("2A27C62C-D878-42CA-8FBF-1BD6F040751E"));

            Assert.IsNotNull(actionResult.Result);
            Assert.IsTrue((actionResult.Result as OkNegotiatedContentResult<IEnumerable<FlujosProgramacionDto>>).Content.Any());
        }

        [TestMethod]
        public void CrearPeriodo_Ok()
        {
            var actionResult = _programacionController.CrearPeriodo("Nacional");

            Assert.IsNotNull(actionResult.Result);
            Assert.IsTrue((actionResult.Result as OkNegotiatedContentResult<RespuestaGeneralDto>).Content.Exito);
        }

        [TestMethod]
        public void IniciarProceso_Ok()
        {
            var actionResult = _programacionController.IniciarProceso("Nacional");

            Assert.IsNotNull(actionResult.Result);
            Assert.IsTrue((actionResult.Result as OkNegotiatedContentResult<RespuestaGeneralDto>).Content.Exito);
        }

        [TestMethod]
        public void ObtenerDatosProgramacionEncabezado_Ok()
        {
            var actionResult = _programacionController.ObtenerDatosProgramacionEncabezado(204, 2297, "distribucion");

            Assert.IsNotNull(actionResult.Result);
            Assert.IsTrue((actionResult.Result as OkNegotiatedContentResult<string>).Content.Any());
        }

        [TestMethod]
        public void ObtenerDatosProgramacionDetalle_Ok()
        {
            var actionResult = _programacionController.ObtenerDatosProgramacionDetalle(204, "distribucion");

            Assert.IsNotNull(actionResult.Result);
            Assert.IsTrue((actionResult.Result as OkNegotiatedContentResult<string>).Content.Any());
        }

        [TestMethod]
        public void GuardarDatosProgramacionFuentes_Ok()
        {
            var actionResult = _programacionController.GuardarDatosProgramacionFuentes(new Dominio.Dto.Programacion.ProgramacionFuenteDto
            {
                TramiteProyectoId = 1,
                NivelId = "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0",
                SeccionCapitulo = 1
            });

            Assert.IsNotNull(actionResult.Result);
            Assert.IsTrue((actionResult.Result as OkNegotiatedContentResult<RespuestaGeneralDto>).Content.Exito);
        }

        [TestMethod]
        public void GuardarDatosProgramacionIniciativa_Ok()
        {
            var actionResult = this._programacionServicios.GuardarDatosProgramacionIniciativa(new Dominio.Dto.Programacion.ProgramacionIniciativaDto
            {
                TramiteProyectoId = 1,
                SeccionCapitulo = 1
            }, "");

            Assert.IsTrue(actionResult.Result.Exito);
        }

        [TestMethod]
        public void ValidarConsecutivoPresupuestal_Ok()
        {
            var actionResult = _programacionController.ValidarConsecutivoPresupuestal("");

            Assert.IsNotNull(actionResult);
            Assert.IsTrue(actionResult != null || string.IsNullOrWhiteSpace(actionResult.ToString()));
        }

        [TestMethod]
        public void ObtenerDatosProgramacionProducto_Ok()
        {
            var actionResult = _programacionController.ObtenerDatosProgramacionProducto(2290);

            Assert.IsNotNull(actionResult.Result);
            Assert.IsTrue((actionResult.Result as OkNegotiatedContentResult<string>).Content.Any());
        }

        [TestMethod]
        public void GuardarDatosProgramacionProducto_Ok()
        {
            var actionResult = _programacionController.GuardarDatosProgramacionProducto(new Dominio.Dto.Programacion.ProgramacionProductoDto
            {
                TramiteId = 1,
                NivelId = "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0",
                SeccionCapitulo = 1
            });

            Assert.IsNotNull(actionResult.Result);
            Assert.IsTrue((actionResult.Result as OkNegotiatedContentResult<RespuestaGeneralDto>).Content.Exito);
        }

        [TestMethod]
        public void ObtenerInboxProgramacionConsolaProcesos_Ok()
        {
            var actionResult = _programacionController.ObtenerProgramacionConsolaProcesos(new InstanciaTramiteDto
            {
                ColumnasVisibles = new string[0],
                EntidadesVisualizador = new List<int>(),
                InstanciaId = Guid.Empty,
                ParametrosInboxDto = new ParametrosInboxDto(),
                TramiteFiltroDto = new TramiteFiltroDto()
            });

            Assert.IsTrue(actionResult.Result != null);
        }
        [TestMethod]
        public void GuardarPoliticasTransversalesCategoriasProgramacion()
        {
            PoliticasTransversalesCategoriasProgramacionDto politicasTransversalesCategoriasProgramacionDto = new PoliticasTransversalesCategoriasProgramacionDto();

            politicasTransversalesCategoriasProgramacionDto.DatosDimension = new List<DatosDimension>();

            politicasTransversalesCategoriasProgramacionDto.DatosDimension.Add(new DatosDimension() { DimensionId = 0 });

            var actionResult = _programacionController.GuardarPoliticasTransversalesCategoriasProgramacion(politicasTransversalesCategoriasProgramacionDto);

            Assert.IsNotNull(actionResult.Result);
            Assert.IsTrue((actionResult.Result as OkNegotiatedContentResult<RespuestaGeneralDto>).Content.Exito);
        }

        [TestMethod]
        public void EliminarCategoriasProyectoProgramacion()
        {
            EliminarCategoriasProyectoProgramacionDto eliminarCategoriasProyectoProgramacionDto = new EliminarCategoriasProyectoProgramacionDto();

            eliminarCategoriasProyectoProgramacionDto.DimensionId = 1;


            var actionResult = _programacionController.EliminarCategoriasProyectoProgramacion(eliminarCategoriasProyectoProgramacionDto);

            Assert.IsNotNull(actionResult.Result);
            Assert.IsTrue((actionResult.Result as OkNegotiatedContentResult<RespuestaGeneralDto>).Content.Exito);
        }
        [TestMethod]
        public void GuardarPoliticasTransversalesCategoriasModificaciones()
        {
            PoliticasTransversalesCategoriasProgramacionDto politicasTransversalesCategoriasProgramacionDto = new PoliticasTransversalesCategoriasProgramacionDto();

            politicasTransversalesCategoriasProgramacionDto.DatosDimension = new List<DatosDimension>();

            politicasTransversalesCategoriasProgramacionDto.DatosDimension.Add(new DatosDimension() { DimensionId = 0 });

            var actionResult = _programacionController.GuardarPoliticasTransversalesCategoriasModificaciones(politicasTransversalesCategoriasProgramacionDto);

            Assert.IsNotNull(actionResult.Result);
            Assert.IsTrue((actionResult.Result as OkNegotiatedContentResult<RespuestaGeneralDto>).Content.Exito);
        }

        [TestMethod]
        public void GuardarModificacionesAsociarIndicadorPolitica()
        {
            var actionResult = _programacionServicios.GuardarModificacionesAsociarIndicadorPolitica(0, 0, 0, 0, "", "");

            Assert.IsNotNull(actionResult.Result);
            
        }
    }
}
