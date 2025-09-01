namespace DNP.Backbone.Test.Servicios
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Text;
    using System.Web.Http.Results;
    using Backbone.Servicios.Implementaciones.Inbox;
    using Backbone.Servicios.Interfaces.Autorizacion;
    using Backbone.Servicios.Interfaces.ServiciosNegocio;
    using Comunes.Dto;
    using Comunes.Properties;
    using DNP.Backbone.Comunes.Enums;
    using DNP.Backbone.Comunes.Utilidades.ExtensionMethods;
    using DNP.Backbone.Dominio.Dto.Inbox;
    using DNP.Backbone.Dominio.Dto.Programacion;
    using DNP.Backbone.Servicios.Implementaciones.ServiciosNegocio;
    using DNP.Backbone.Servicios.Interfaces;
    using DNP.Backbone.Servicios.Interfaces.Nivel;
    using DNP.Backbone.Servicios.Interfaces.Programacion;
    using DNP.Backbone.Web.API.Controllers;
    using Microsoft.Practices.Unity;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ProgramacionServiciosTest
    {
        private IProgramacionServicios _programacionServicios;
        private IFlujoServicios _flujosServicios;

        [TestInitialize]
        public void Init()
        {
            _programacionServicios = Config.UnityConfig.Container.Resolve<IProgramacionServicios>();
            _flujosServicios = Config.UnityConfig.Container.Resolve<IFlujoServicios>();
        }

        [TestMethod]
        public void ObtenerProgramaciones_Ok()
        {
            var actionResult = this._programacionServicios.ObtenerProgramaciones("Nacional", null, null, null, null, "jdelgado");
            Assert.IsTrue(actionResult.Result.Any());
        }

        [TestMethod]
        public void GuardarProgramaciones_Ok()
        {
            var actionResult = this._programacionServicios.GuardarProgramacion(new Dominio.Dto.Programacion.ProgramacionDto
            {
                FlujoId = Guid.NewGuid(),
                FechaDesde = DateTime.Now,
                FechaHasta = DateTime.Now,
                IdProgramacion = 1,
                TipoEntidad = "Nacional",
            }, "jdelgado");
            Assert.IsTrue(actionResult.Result.Exito);
        }

        [TestMethod]
        public void EliminarProgramacion_Ok()
        {
            var actionResult = this._programacionServicios.EliminarProgramacion(new Dominio.Dto.Programacion.ProgramacionDto
            {
                FechaDesde = DateTime.Now,
                FechaHasta = DateTime.Now,
                IdProgramacion = 1,
                Cerrado = false,
                Creado = false,
                TipoEntidad = "Nacional"
            }, "jdelgado");
            Assert.IsTrue(actionResult.Result.Exito);
        }

        [TestMethod]
        public void ObtenerProgramacionExcepciones_Ok()
        {
            var actionResult = this._programacionServicios.ObtenerProgramacionExcepciones(1, "jdelgado");
            Assert.IsTrue(actionResult.Result.Any());
        }

        [TestMethod]
        public void GuardarProgramacionExcepcion_Ok()
        {
            var actionResult = this._programacionServicios.GuardarProgramacionExcepcion(new Dominio.Dto.Programacion.ProgramacionExcepcionDto
            {
                FechaDesde = DateTime.Now,
                FechaHasta = DateTime.Now,
                IdProgramacion = 1,
                EntidadId = 1,
                IdProgramacionExcepcion = 0
            }, "jdelgado");
            Assert.IsTrue(actionResult.Result.Exito);
        }

        [TestMethod]
        public void EditarProgramacionExcepcion_Ok()
        {
            var actionResult = this._programacionServicios.EliminarProgramacionExcepcion(new Dominio.Dto.Programacion.ProgramacionExcepcionDto
            {
                FechaDesde = DateTime.Now,
                FechaHasta = DateTime.Now,
                IdProgramacion = 1,
                EntidadId = 1,
                IdProgramacionExcepcion = 0
            }, "jdelgado");
            Assert.IsTrue(actionResult.Result.Exito);
        }

        [TestMethod]
        public void EliminarProgramacionExcepcion_Ok()
        {
            var actionResult = this._programacionServicios.EliminarProgramacionExcepcion(new Dominio.Dto.Programacion.ProgramacionExcepcionDto
            {
                FechaDesde = DateTime.Now,
                FechaHasta = DateTime.Now,
                IdProgramacion = 1,
                EntidadId = 1,
                IdProgramacionExcepcion = 0
            }, "jdelgado");
            Assert.IsTrue(actionResult.Result.Exito);
        }

        [TestMethod]
        public void ObtenerFujosTramitesNiveles_Ok()
        {
            //var actionResult = this._flujosServicios.ObtenerListaFlujosTramitePorNivel(new Guid("2A27C62C-D878-42CA-8FBF-1BD6F040751E"), "jdelgado");
            //Assert.IsTrue(actionResult.Result.Any());
        }

        [TestMethod]
        public void CrearPeriodos_Ok()
        {
            var actionResult = this._programacionServicios.CrearPeriodo("SGR", "jdelgado");
            Assert.IsTrue(actionResult.Result.Exito);
        }

        [TestMethod]
        public void IniciarProceso_Ok()
        {
            var actionResult = this._programacionServicios.IniciarProceso("SGR", "jdelgado");
            Assert.IsTrue(actionResult.Result.Exito);
        }

        [TestMethod]
        public void ObtenerDatosProgramacionEncabezado_Ok()
        {
            //var actionResult = this._programacionServicios.ObtenerDatosProgramacionEncabezado(204, 2297, "distribucion");

            //Assert.IsNotNull(actionResult.Result);
            //Assert.IsTrue((actionResult.Result as OkNegotiatedContentResult<string>).Content.Any());
        }

        [TestMethod]
        public void ObtenerDatosProgramacionDetalle_Ok()
        {
            //var actionResult = this._programacionServicios.ObtenerDatosProgramacionDetalle(204, "distribucion");

            //Assert.IsNotNull(actionResult.Result);
            //Assert.IsTrue((actionResult.Result as OkNegotiatedContentResult<string>).Content.Any());
        }

        [TestMethod]
        public void GuardarDatosProgramacionFuentes_Ok()
        {
            var actionResult = this._programacionServicios.GuardarDatosProgramacionFuentes(new Dominio.Dto.Programacion.ProgramacionFuenteDto
            {
                TramiteProyectoId = 1,
                NivelId = "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0",
                SeccionCapitulo = 1
            }, "");

            Assert.IsTrue(actionResult.Result.Exito);
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
        public void ValidarCalendarioProgramacion()
        {
            int? entityTypeCatalogOptionId = 150;
            Nullable<Guid> nivelId = new Guid("67c107aa-5f59-407a-b430-407959e38b52");
            Nullable<int> seccionCapituloId = 1;
            string usuarioDnp = "CC51919599";

            var actionResult = this._programacionServicios.ValidarCalendarioProgramacion(entityTypeCatalogOptionId, nivelId, seccionCapituloId, usuarioDnp);

            Assert.IsTrue(actionResult.Result.Exito);
        }

        [TestMethod]
        public void ValidarConsecutivoPresupuestal()
        {
            var actionResult = _programacionServicios.ValidarConsecutivoPresupuestal("", "");

            Assert.IsTrue(actionResult != null || string.IsNullOrWhiteSpace(actionResult.ToString()));
        }

        [TestMethod]
        public void ObtenerDatosProgramacionProducto_Ok()
        {
            //var actionResult = this._programacionServicios.ObtenerDatosProgramacionProducto(2290);

            //Assert.IsNotNull(actionResult.Result);
            //Assert.IsTrue((actionResult.Result as OkNegotiatedContentResult<string>).Content.Any());
        }

        [TestMethod]
        public void GuardarDatosProgramacionProducto_Ok()
        {
            var actionResult = this._programacionServicios.GuardarDatosProgramacionProducto(new Dominio.Dto.Programacion.ProgramacionProductoDto
            {
                TramiteId = 1,
                NivelId = "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0",
                SeccionCapitulo = 1
            }, "");

            Assert.IsTrue(actionResult.Result.Exito);
        }

        [TestMethod]
        public void ObtenerInboxProgramacionConsolaProcesos_Ok()
        {
            var actionResult = this._programacionServicios.ObtenerInboxProgramacionConsolaProcesos(new InstanciaTramiteDto
            {
                ColumnasVisibles = new string[0],
                EntidadesVisualizador = new List<int>(),
                InstanciaId = Guid.Empty,
                ParametrosInboxDto = new ParametrosInboxDto(),
                TramiteFiltroDto = new TramiteFiltroDto()
            }, string.Empty);

            Assert.IsTrue(actionResult.Result != null);
        }

        [TestMethod]
        public void EliminarCategoriasProyectoProgramacion_Ok()
        {
            EliminarCategoriasProyectoProgramacionDto eliminarCategoriasProyectoProgramacionDto = new EliminarCategoriasProyectoProgramacionDto();

            eliminarCategoriasProyectoProgramacionDto.DimensionId = 1;


            var actionResult = _programacionServicios.EliminarCategoriasProyectoProgramacion(eliminarCategoriasProyectoProgramacionDto, "");

            Assert.IsTrue(actionResult.Result != null);
        }

        [TestMethod]
        public void GuardarPoliticasTransversalesCategoriasModificaciones_Ok()
        {
            PoliticasTransversalesCategoriasProgramacionDto politicasTransversalesCategoriasProgramacionDto = new PoliticasTransversalesCategoriasProgramacionDto();

            politicasTransversalesCategoriasProgramacionDto.DatosDimension = new List<DatosDimension>();

            politicasTransversalesCategoriasProgramacionDto.DatosDimension.Add(new DatosDimension() { DimensionId = 0 });

            var actionResult = _programacionServicios.GuardarPoliticasTransversalesCategoriasModificaciones(politicasTransversalesCategoriasProgramacionDto, "");

            Assert.IsTrue(actionResult.Result != null);
        }

        [TestMethod]
        public void GuardarModificacionesAsociarIndicadorPolitica_Ok()
        {            
            var actionResult = _programacionServicios.GuardarModificacionesAsociarIndicadorPolitica(0, 0, 0,0, "", "");

            Assert.IsTrue(actionResult.Result != null);
        }
    }
}
