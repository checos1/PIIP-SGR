

namespace DNP.Backbone.Test.Servicios
{
    using Backbone.Servicios.Interfaces.Autorizacion;
    using Comunes.Dto;
    using Comunes.Properties;
    using DNP.Backbone.Dominio.Dto.FuenteFinanciacion;
    using DNP.Backbone.Dominio.Dto.Tramites;
    using DNP.Backbone.Servicios.Interfaces.Tramites;
    using Microsoft.Practices.Unity;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Summary description for TramiteServicioTest
    /// </summary>
    [TestClass]
    public class TramiteServicioTest
    {
        private IAutorizacionServicios _autorizacionServicios;
        private ITramiteServicios _tramiteServicios;

        [TestInitialize]
        public void Init()
        {
            _tramiteServicios = Config.UnityConfig.Container.Resolve<ITramiteServicios>();
            _autorizacionServicios = Config.UnityConfig.Container.Resolve<IAutorizacionServicios>();
        }

        [TestMethod]
        public void CuandoEnvioParametrosTramite_NoEncuentraEntidades()
        {
            var parametros = new ParametrosInboxDto()
            {
                Aplicacion = "AP:Backbone",
                IdUsuario = "jdelgado",
                IdObjeto = new Guid("D6880615-3CD8-4258-A0A4-821E21146124"),
                ListaIdsRoles =
                                   new List<Guid>() { Guid.NewGuid(), Guid.NewGuid() }
            };
            string tokenAutorizacionValor = "Basic amRlbGdhZG86MjI4OTE1MDA=";
            var instancia = new InstanciaTramiteDto() { ParametrosInboxDto = parametros, TramiteFiltroDto = new TramiteFiltroDto() { IdUsuarioDNP = "jdelgado", TokenAutorizacion = tokenAutorizacionValor, FiltroGradeDtos = new List<FiltroGradeDto>() } };
            var actionResult = _tramiteServicios.ObtenerInboxTramites(instancia).Result;
            Assert.IsTrue(actionResult.ListaGrupoTramiteEntidad.Count == 0);
        }

        [TestMethod]
        public void ActualizarVigenciaFuturaFuente()
        {
            VigenciaFuturaCorrienteFuenteVigenciaDto vigencia2022 = new VigenciaFuturaCorrienteFuenteVigenciaDto();
            vigencia2022.PeriodoProyectoId = 2022;
            vigencia2022.ValorVigenteFutura = 123000000;

            VigenciaFuturaCorrienteFuenteVigenciaDto vigencia2023 = new VigenciaFuturaCorrienteFuenteVigenciaDto();
            vigencia2022.PeriodoProyectoId = 2023;
            vigencia2022.ValorVigenteFutura = 456000000;

            List<VigenciaFuturaCorrienteFuenteVigenciaDto> vigencias = new List<VigenciaFuturaCorrienteFuenteVigenciaDto>();
            vigencias.Add(vigencia2022);
            vigencias.Add(vigencia2023);

            VigenciaFuturaCorrienteFuenteDto fuente = new VigenciaFuturaCorrienteFuenteDto();
            fuente.FuenteId = 1215;
            fuente.TramiteId = 356;
            fuente.ProyectoId = 97706;
            fuente.Vigencias = vigencias;

            var resultados = _tramiteServicios.ActualizarVigenciaFuturaFuente(fuente, "usuariodnp").Result;
            Assert.IsNotNull(resultados);
        }

        [TestMethod]
        public void ObtenerFuentesFinanciacionVigenciaFuturaConstante()
        {
            string bpin = "202200000000060";
            int tramiteId = 611;
            var actionResult = _tramiteServicios.ObtenerFuentesFinanciacionVigenciaFuturaConstante(bpin, tramiteId, "usuariodnp").Result;
            Assert.IsNotNull(actionResult);
        }

        [TestMethod]
        public void ObtenerProductosVigenciaFuturaCorriente()
        {
            string bpin = "202200000000002";
            int tramiteId = 611;
            var actionResult = _tramiteServicios.ObtenerProductosVigenciaFuturaCorriente(bpin, tramiteId, "usuariodnp").Result;
            Assert.IsNotNull(bpin);
        }


        [TestMethod]
        public void ObtenerModificacionLeyenda()
        {
            int tramiteId = 732;
            int proyectoId = 9776;
            var actionResult = _tramiteServicios.ObtenerModificacionLeyenda( tramiteId, proyectoId, "usuariodnp").Result;
            Assert.IsNotNull(actionResult);
        }

        [TestMethod]
        public void ActualizarModificacionLeyenda()
        {
            var modificacionLeyendaDto = new ModificacionLeyendaDto();
            modificacionLeyendaDto.TramiteProyectoId = 1727;
            modificacionLeyendaDto.ErrorAritmetico = false;
            var actionResult = _tramiteServicios.ActualizarModificacionLeyenda(modificacionLeyendaDto, "usuariodnp").Result;
            Assert.IsNotNull(actionResult);
        }

        [TestMethod]
        public void ObtenerListaDirecionesDNP()
        {
            Guid identity = new Guid("FBB8BAB4-868B-4422-84F7-58B16F556AD6");
            string usuarioDnp = "usuariodnp";
            var actionResult = _tramiteServicios.ObtenerListaDirecionesDNP(identity, usuarioDnp).Result;
            Assert.IsNotNull(actionResult);
        }

        [TestMethod]
        public void ObtenerListaSubdirecionesPorParentId()
        {
            int idEntididadType = 105;
            string usuarioDnp = "usuariodnp";
            var actionResult = _tramiteServicios.ObtenerListaSubdirecionesPorParentId(idEntididadType, usuarioDnp).Result;
            Assert.IsNotNull(actionResult);
        }

        [TestMethod]
        public void BorrarFirma()
        {
            string usuarioDnp = "cc202006";
            var actionResult = _tramiteServicios.BorrarFirma(usuarioDnp).Result;
            Assert.IsNotNull(actionResult);
        }

        [TestMethod]
        public void ObtenerProyectosCartaTramite()
        {
            int tramiteId = 20211;
            string usuarioDnp = "cc202006";
            var actionResult = _tramiteServicios.ObtenerProyectosCartaTramite(tramiteId,usuarioDnp).Result;
            Assert.IsNotNull(actionResult);
        }

        [TestMethod]
        public void ObtenerDetalleCartaAL()
        {
            int tramiteId = 20211;
            string usuarioDnp = "cc202006";
            var actionResult = _tramiteServicios.ObtenerDetalleCartaAL(tramiteId,usuarioDnp).Result;
            Assert.IsNotNull(actionResult);
        }

        [TestMethod]
        public void obtenerEntidadAsociarProyecto()
        {
            Guid instanciaId = new Guid("0BB22784-24B5-410E-82AC-55B39989A8B0");
            string accion = "C";
            string usuarioDnp = "cc202006";
            var actionResult = _tramiteServicios.obtenerEntidadAsociarProyecto(instanciaId, accion,usuarioDnp).Result;
            Assert.IsNotNull(actionResult);
        }

        [TestMethod]
        public void EliminarLiberacionVigenciaFutura()
        {
            LiberacionVigenciasFuturasDto dto = new LiberacionVigenciasFuturasDto{tramiteId=1002,tramiteProyectoId=0,creadoPor="",vigenciaDesde=null,vigenciaHasta=null};
            string usuarioDnp = "cc202006";
            var actionResult = _tramiteServicios.EliminarLiberacionVigenciaFutura(dto, usuarioDnp).Result;
            Assert.IsNotNull(actionResult);
        }

        [TestMethod]
        public void ObtenerCalendartioPeriodo()
        {
            string bpin = "2021682550006";
            string usuarioDnp = "cc202006";
            var actionResult = _tramiteServicios.ObtenerCalendartioPeriodo(bpin, usuarioDnp).Result;
            Assert.IsNotNull(actionResult);
        }
    }
}
