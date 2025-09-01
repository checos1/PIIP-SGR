namespace DNP.ServiciosNegocio.Web.API.Test.TramitesProyectos
{
    using DNP.ServiciosNegocio.Dominio.Dto.Tramites.VigenciaFutura;
    using DNP.ServiciosNegocio.Dominio.Dto.Proyectos;
    using DNP.ServiciosNegocio.Servicios.Interfaces.TramitesProyectos;
    using DNP.ServiciosNegocio.Servicios.Interfaces.Transversales;
    using DNP.ServiciosNegocio.Web.API.Controllers.TramitesProyectos;
    using DNP.ServiciosNegocio.Dominio.Dto.Tramites;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Unity;
    using System.Threading.Tasks;
    using System.Web.Http.Results;
    using System;
    using DNP.ServiciosNegocio.Comunes.Autorizacion;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Security.Principal;
    using System.Collections.Generic;
    using DNP.ServiciosNegocio.Dominio.Dto.FuenteFinanciacion;
    using DNP.ServiciosNegocio.Dominio.Dto.Catalogos;
    using Newtonsoft.Json;

    [TestClass]
    public sealed class TramitesProyectosControllerTest : IDisposable
    {
        private ITramitesProyectosServicio tramitesProyectosServicio { get; set; }
        private ICambiosRelacionPlanificacionServicio relacionPlanificacionServicio { get; set; }
        private IAutorizacionUtilidades AutorizacionUtilizades { get; set; }
        private ISeccionCapituloServicio seccionCapituloServicio { get; set; }
        private ICambiosJustificacionHorizonServicio justificacionHorizonteServicio { get; set; }

        private TramitesProyectosController _tramitesProyectosController;


        [TestInitialize]
        public void Init()
        {
            var contenedor = Configuracion.UnityConfig.Container;
            tramitesProyectosServicio = contenedor.Resolve<ITramitesProyectosServicio>();
            AutorizacionUtilizades = contenedor.Resolve<IAutorizacionUtilidades>();

            _tramitesProyectosController =
                new TramitesProyectosController(tramitesProyectosServicio, AutorizacionUtilizades, relacionPlanificacionServicio, seccionCapituloServicio, justificacionHorizonteServicio)
                {
                    ControllerContext
                        =
                        {
                            Request
                                = new
                                    HttpRequestMessage()
                        }
                };

            _tramitesProyectosController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _tramitesProyectosController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:PIIP");
            _tramitesProyectosController.ControllerContext.Request.Headers.Add("piip-idAccion", "D2BA19EB-0487-4C94-8960-3A6047B81409");
            _tramitesProyectosController.ControllerContext.Request.Headers.Add("piip-idInstanciaFlujo", "BC135467-5041-4C0F-8AB7-EC2F09E02AAF");
            _tramitesProyectosController.ControllerContext.Request.Headers.Add("piip-idFormulario", "ADA6BAAC-F2DD-4664-AAB7-9E8AB61405CC");
            _tramitesProyectosController.User = new GenericPrincipal(new GenericIdentity("jcastano@dnp.gov.co", "Jocas123*"), new[] { "" });
        }

        [TestMethod]
        public async Task ObtenerlVlrConstanteVF_Ok()
        {
            int TramiteId = 25;
            var resultados = (OkNegotiatedContentResult<InformacionPresupuestalVlrConstanteDto>)await _tramitesProyectosController.ObtenerInformacionPresupuestalVlrConstanteVF(TramiteId);
            Assert.IsNotNull(resultados.Content);
        }

        [TestMethod]
        public async Task ObtenerlVlrConstanteVF_Vacio()
        {
            int TramiteId = 0;
            var resultados = (OkNegotiatedContentResult<InformacionPresupuestalVlrConstanteDto>)await _tramitesProyectosController.ObtenerInformacionPresupuestalVlrConstanteVF(TramiteId);
            Assert.IsNull(resultados.Content);
        }

        [TestMethod]
        public async Task ObtenerPreguntasProyectoActualizacionPaso_Ok()
        {
            int TramiteId = 453;
            int ProyectoId = 0;
            int TipoTramiteId = 4;
            int TipoRolId = 0;
            Guid IdNivel = new Guid("74A0F290-6760-4155-B06B-59A3D0E92DE5");

            var resultados = (OkNegotiatedContentResult<IEnumerable<JustificacionPasoDto>>)await _tramitesProyectosController.ObtenerPreguntasProyectoActualizacionPaso(TramiteId, ProyectoId, TipoTramiteId, IdNivel, TipoRolId);
            Assert.IsNotNull(resultados.Content);
        }

        [TestMethod]
        public async Task ObtenerPreguntasProyectoActualizacionPaso_vacio()
        {
            int TramiteId = 453;
            int ProyectoId = 0;
            int TipoTramiteId = 4;
            int TipoRolId = 0;
            Guid IdNivel = new Guid("74A0F290-6760-4155-B06B-59A3D0E92DE5");

            var resultados = (OkNegotiatedContentResult<IEnumerable<JustificacionPasoDto>>)await _tramitesProyectosController.ObtenerPreguntasProyectoActualizacionPaso(TramiteId, ProyectoId, TipoTramiteId, IdNivel, TipoRolId);
            Assert.IsNotNull(resultados.Content);

        }

        [TestMethod]
        public async Task  ObtenerDatosCronograma_Ok()
        {
            Guid instanciaId = new Guid("3E0750D4-DC36-4546-942F-72F8638B3E0A");

            var resultados = (OkNegotiatedContentResult<string>)await _tramitesProyectosController.ObtenerDatosCronograma(instanciaId);
           
            Assert.IsNotNull(resultados.Content);
        }

        [TestMethod]
        public async Task ObtenerDatosCronograma_Vacio()
        {
            Guid instanciaId = new Guid();

            var resultados = (OkNegotiatedContentResult<string>)await _tramitesProyectosController.ObtenerDatosCronograma(instanciaId);

            Assert.AreEqual(resultados.Content, "");
        }

        [TestMethod]
        public async Task ObtenerDeflactores()
        {
            var resultados = (OkNegotiatedContentResult<List<TramiteDeflactoresDto>>)await _tramitesProyectosController.ObtenerDeflactores();
            Assert.IsNotNull(resultados.Content);
        }

        [TestMethod]
        public async Task GetProyectoTramite()
        {
            int ProyectoId = 97706;
            int TramiteId = 356;
            var resultados = (OkNegotiatedContentResult<List<Dominio.Dto.Tramites.TramiteProyectoDto>>)await _tramitesProyectosController.ObtenerProyectoTramite(ProyectoId, TramiteId);
            Assert.IsNotNull(resultados.Content);
        }

        [TestMethod]
        public async Task ActualizaVigenciaFuturaProyectoTramite()
        {
            Dominio.Dto.Tramites.TramiteProyectoDto tramiteProyectoDto = new TramiteProyectoDto();
            tramiteProyectoDto.ProyectoId = 97706;
            tramiteProyectoDto.TramiteId = 356;
            tramiteProyectoDto.EsConstante = true;
            tramiteProyectoDto.AnioBase = 2022;
            var resultados = (OkNegotiatedContentResult<string>)await _tramitesProyectosController.ActualizaVigenciaFuturaProyectoTramite(tramiteProyectoDto);
            Assert.IsNotNull(resultados.Content);
        }

        [TestMethod]
        public void ObtenerFuentesFinanciacionVigenciaFuturaCorriente()
        {
            string bpin = "202100000000037";
            var actionResult = _tramitesProyectosController.ObtenerFuentesFinanciacionVigenciaFuturaCorriente(bpin);
            Assert.IsNotNull(actionResult);
        }

        [TestMethod]
        public async Task ActualizarVigenciaFuturaFuente()
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

            var resultados = (OkNegotiatedContentResult<VigenciaFuturaResponse>)await _tramitesProyectosController.ActualizarVigenciaFuturaFuente(fuente);
            Assert.IsNotNull(resultados.Content);
        }

        [TestMethod]
        public async Task GuardarProyectosTramiteNegocio_Ok()
        {
            _tramitesProyectosController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _tramitesProyectosController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:PIIP");
            _tramitesProyectosController.ControllerContext.Request.Headers.Add("piip-idAccion", "D2BA19EB-0487-4C94-8960-3A6047B81409");
            _tramitesProyectosController.ControllerContext.Request.Headers.Add("piip-idInstanciaFlujo", "BC135467-5041-4C0F-8AB7-EC2F09E02AAF");

            _tramitesProyectosController.User = new GenericPrincipal(new GenericIdentity("jcastano@dnp.gov.co", "Jocas123*"), new[] { "" });

            DatosTramiteProyectosDto DatosTramiteProyectosDto = new DatosTramiteProyectosDto();
            DatosTramiteProyectosDto.TramiteId = 1;
            var resultados = (OkNegotiatedContentResult<TramitesResultado>)await _tramitesProyectosController.GuardarProyectosTramiteNegocio(DatosTramiteProyectosDto);
            Assert.AreEqual(true, resultados.Content.Exito);
        }

        [TestMethod]
        public async Task GuardarProyectosTramiteNegocio_MensajeError()
        {
            _tramitesProyectosController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _tramitesProyectosController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:PIIP");
            _tramitesProyectosController.ControllerContext.Request.Headers.Add("piip-idAccion", "D2BA19EB-0487-4C94-8960-3A6047B81409");
            _tramitesProyectosController.ControllerContext.Request.Headers.Add("piip-idInstanciaFlujo", "BC135467-5041-4C0F-8AB7-EC2F09E02AAF");

            _tramitesProyectosController.User = new GenericPrincipal(new GenericIdentity("jcastano@dnp.gov.co", "Jocas123*"), new[] { "" });

            DatosTramiteProyectosDto DatosTramiteProyectosDto = new DatosTramiteProyectosDto();
            var resultados = (OkNegotiatedContentResult<TramitesResultado>)await _tramitesProyectosController.GuardarProyectosTramiteNegocio(DatosTramiteProyectosDto);
            Assert.IsNotNull(resultados.Content.Mensaje);
        }

        [TestMethod]
        public async Task ObtenerInformacionPresupuestalValores_Ok()
        {
            int TramiteId = 26;
            var resultados = (OkNegotiatedContentResult<InformacionPresupuestalValoresDto>)await _tramitesProyectosController.ObtenerInformacionPresupuestalValores(TramiteId);
            Assert.IsNotNull(resultados.Content);
        }

        [TestMethod]
        public async Task ObtenerInformacionPresupuestalValores_Vacio()
        {
            int TramiteId = 0;
            var resultados = (OkNegotiatedContentResult<InformacionPresupuestalValoresDto>)await _tramitesProyectosController.ObtenerInformacionPresupuestalValores(TramiteId);
            Assert.IsNull(resultados.Content);
        }

        [TestMethod]
        public async Task GuardarInformacionPresupuestalValores()
        {
            InformacionPresupuestalValoresDto tramiteProyectoDto = new InformacionPresupuestalValoresDto();
            tramiteProyectoDto.ProyectoId = 97833;
            tramiteProyectoDto.TramiteId = 593;
            tramiteProyectoDto.AplicaConstante = true;
            tramiteProyectoDto.AñoBase = 2022;
            var resultados = (OkNegotiatedContentResult<string>)await _tramitesProyectosController.GuardarInformacionPresupuestalValores(tramiteProyectoDto);
            Assert.IsNotNull(resultados.Content);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                // dispose managed resources
                _tramitesProyectosController.Dispose();
            }
            // free native resources
        }

        [TestMethod]
        public void ObtenerFuentesFinanciacionVigenciaFuturaConstante()
        {
            string bpin = "202200000000060";
            int tramiteId = 611;
            var actionResult = _tramitesProyectosController.ObtenerFuentesFinanciacionVigenciaFuturaConstante(bpin, 611);
            Assert.IsNotNull(actionResult);
        }

        [TestMethod]
        public void ObtenerProductosVigenciaFuturaCorriente()
        {
            string bpin = "202200000000002";
            var actionResult = _tramitesProyectosController.ObtenerProductosVigenciaFuturaCorriente(bpin, 611);
            Assert.IsNotNull(actionResult);
        }

        [TestMethod]
        public async Task ObtenerProyectoAsociarTramiteLeyenda_Ok()
        {
            int TramiteId = 26;
            string codbpin = "2022";
            var resultados = (OkNegotiatedContentResult<List<proyectoAsociarTramite>>)await _tramitesProyectosController.ObtenerProyectoAsociacionAclaracionLeyenda(codbpin,TramiteId);
            Assert.IsNotNull(resultados.Content);
        }

        [TestMethod]
        public async Task ObtenerProyectoAsociarTramiteLeyenda_Vacio()
        {
            int TramiteId = 0;
            string codbpin=null;
            var resultados = (OkNegotiatedContentResult<List<proyectoAsociarTramite>>)await _tramitesProyectosController.ObtenerProyectoAsociacionAclaracionLeyenda(codbpin,TramiteId);
            Assert.IsNull(resultados.Content);
        }

        [TestMethod]
        public async Task ObtenerModificacionLeyenda_Ok()
        {
            int tramiteId = 26;
            int ProyectoId = 26025;
            var resultados = (OkNegotiatedContentResult<ModificacionLeyendaDto>)await _tramitesProyectosController.ObtenerModificacionLeyenda(tramiteId, ProyectoId);
            Assert.IsNotNull(resultados.Content);
        }

        [TestMethod]
        public async Task ObtenerModificacionLeyenda_Vacio()
        {
            int tramiteId = 0;
            int ProyectoId = 0;
            var resultados = (OkNegotiatedContentResult<ModificacionLeyendaDto>)await _tramitesProyectosController.ObtenerModificacionLeyenda(tramiteId,ProyectoId);
            Assert.IsNull(resultados.Content);
        }

        [TestMethod]
        public void ObtenerListaDireccionesDNP()
        {
            Guid idEntidad = new Guid();
            var actionResult = _tramitesProyectosController.ObtenerListaDireccionesDNP(idEntidad);
            Assert.IsNotNull(actionResult);
        }

        [TestMethod]
        public void ObtenerListaSubdireccionesPorParentId()
        {
            int idEntityType = 105;
            var resultados = _tramitesProyectosController.ObtenerListaSubdireccionesPorParentId(idEntityType);
            Assert.IsNotNull(resultados);
        }

        [TestMethod]
        public async Task ObtenerProyectosCartaTramite_Ok()
        {
            int tramiteId = 26;
            var resultados = (OkNegotiatedContentResult<ProyectosCartaDto>)await _tramitesProyectosController.ObtenerProyectosCartaTramite(tramiteId);
            Assert.IsNotNull(resultados.Content);
        }

        [TestMethod]
        public async Task ObtenerProyectosCartaTramite_Vacio()
        {
            int tramiteId = 0;
            var resultados = (OkNegotiatedContentResult<ProyectosCartaDto>)await _tramitesProyectosController.ObtenerProyectosCartaTramite(tramiteId);
            Assert.IsNull(resultados.Content);
        }

        [TestMethod]
        public async Task ObtenerDetalleCartaAL_Ok()
        {
            int tramiteId = 26;
            var resultados = (OkNegotiatedContentResult<DetalleCartaConceptoALDto>)await _tramitesProyectosController.ObtenerDetalleCartaAL(tramiteId);
            Assert.IsNotNull(resultados.Content);
        }

        [TestMethod]
        public async Task ObtenerDetalleCartaAL_Vacio()
        {
            int tramiteId = 0;
            var resultados = (OkNegotiatedContentResult<DetalleCartaConceptoALDto>)await _tramitesProyectosController.ObtenerDetalleCartaAL(tramiteId);
            Assert.IsNull(resultados.Content);
        }

        public void Dispose()
        {
            Dispose(true);
        }

        [TestMethod]
        public async Task ObtenerAmpliarDevolucionTramite_Ok(int tramiteid, int proyectoid)
        {
            var resultados = (OkNegotiatedContentResult<int>)await _tramitesProyectosController.ObtenerAmpliarDevolucionTramite(tramiteid, proyectoid);
            Assert.IsNull(resultados.Content);
        }

        [TestMethod]
        public async Task ObtenerLiberacionVigenciasFuturas_Ok(int tramiteid, int proyectoid)
        {
            var resultados = (OkNegotiatedContentResult<int>)await _tramitesProyectosController.ObtenerLiberacionVigenciasFuturas(tramiteid, proyectoid);
            Assert.IsNull(resultados.Content);
        }

        [TestMethod]
        public async Task InsertaAutorizacionVigenciasFuturas_Ok(TramiteALiberarVfDto autorizacion, string usuario)
        {
            var resultados = (OkNegotiatedContentResult<int>)await _tramitesProyectosController.InsertaAutorizacionVigenciasFuturas(autorizacion);
            Assert.IsNull(resultados.Content);
        }

        [TestMethod]
        public async Task InsertaValoresUtilizadosLiberacionVF_Ok(TramiteALiberarVfDto autorizacion, string usuario)
        {
            var resultados = (OkNegotiatedContentResult<int>)await _tramitesProyectosController.InsertaAutorizacionVigenciasFuturas(autorizacion);
            Assert.IsNull(resultados.Content);
        }

        [TestMethod]
        public void ConsultarCartaConcepto_Ok()
        {
            int TramiteId = 902;
            var actionResult = _tramitesProyectosController.ConsultarCartaConcepto(TramiteId).Result;
            Assert.IsNotNull(actionResult);

        }
        [TestMethod]
        public void ValidacionPeriodoPresidencial_OK()
        {
            int TramiteId = 925;
            var actionResult = _tramitesProyectosController.ValidacionPeriodoPresidencial(TramiteId).Result;
            Assert.IsNotNull(actionResult);

        }

        [TestMethod]
        public async Task GuardarMontosTramite()
        {

            List<ProyectosEnTramiteDto> listaProyectosEnTramiteDto = new List<ProyectosEnTramiteDto>();
            ProyectosEnTramiteDto proyectoEnTramite = new ProyectosEnTramiteDto();
            proyectoEnTramite.BPIN = "202100000000008";
            proyectoEnTramite.CodigoPresupuestal = "1201011206000000020000";
            proyectoEnTramite.TramiteId = 901;
            proyectoEnTramite.ProyectoId = 97652;
            proyectoEnTramite.EntidadId = 186;
            proyectoEnTramite.ValorMontoTramiteNacion = 25;
            proyectoEnTramite.ValorMontoTramitePropios = 100;
            proyectoEnTramite.TipoProyecto = "Contracredito";

            listaProyectosEnTramiteDto.Add(proyectoEnTramite);
            var resultados = (OkNegotiatedContentResult<TramitesResultado>)await _tramitesProyectosController.GuardarMontosTramite(listaProyectosEnTramiteDto);
            Assert.IsNotNull(resultados.Content);


        }

        [TestMethod]
        public async Task ObtenerTramitesVFparaLiberar_Ok()
        {
            int proyecto = 9809;
            var response = await _tramitesProyectosController.ObtenerTramitesVFparaLiberar(proyecto);
            var resultados = response as OkNegotiatedContentResult<List<tramiteVFAsociarproyecto>>;
            Assert.IsNotNull(resultados.Content);
        }

        [TestMethod]
        public async Task ObtenerTramitesVFparaLiberar_Vacio()
        {
            int proyecto = 0;
            var response = await _tramitesProyectosController.ObtenerTramitesVFparaLiberar(proyecto);
            var resultados = response as OkNegotiatedContentResult<List<tramiteVFAsociarproyecto>>;
            Assert.IsNull(resultados.Content);
        }

        [TestMethod]
        public async Task GuardarLiberacionVigenciaFutura()
        {
            LiberacionVigenciasFuturasDto lib = new LiberacionVigenciasFuturasDto();
            lib.tramiteProyectoId = 123;
            lib.tramiteId = 321;
            lib.creadoPor = "CC404040";
            lib.vigenciaDesde = null;
            lib.vigenciaHasta = null;

            var response = await _tramitesProyectosController.GuardarLiberacionVigenciaFutura(lib);
            var resultados = response as OkNegotiatedContentResult<string>;
            Assert.IsNotNull(resultados.Content);


        }

        [TestMethod]
        public void ObtenerPreguntasJustificacionPorProyectosOk()
        {

            int TramiteId = 952;
            int TipoTramiteId = 1;
            int TipoRolId = 1;
            Guid IdNivel = new Guid();

            var resultados =  _tramitesProyectosController.ObtenerPreguntasJustificacionPorProyectos(TramiteId, TipoTramiteId, TipoRolId, IdNivel);
            Assert.IsNotNull(resultados);


        }



        [TestMethod]
        public async Task ObtenerPreguntasJustificacionPorProyectos_Vacio()
        {

            int TramiteId = 0;
            int TipoTramiteId = 1;
            int TipoRolId = 1;
            Guid IdNivel = new Guid();

            var resultados =await _tramitesProyectosController.ObtenerPreguntasJustificacionPorProyectos(TramiteId, TipoTramiteId, TipoRolId, IdNivel);
            Assert.IsNull(((System.Web.Http.Results.OkNegotiatedContentResult<System.Collections.Generic.IEnumerable<DNP.ServiciosNegocio.Dominio.Dto.Tramites.ProyectoJustificacioneDto>>)resultados).Content);


        }

        [TestMethod]
        public async Task ObtenerListaProyectosFuentesAprobado_Vacio() {
            int TramiteId = 0;
           

            var resultados = await _tramitesProyectosController.ObtenerListaProyectosFuentesAprobado(TramiteId);
            Assert.IsNull(((System.Web.Http.Results.OkNegotiatedContentResult<System.Collections.Generic.List<DNP.ServiciosNegocio.Dominio.Dto.Tramites.ProyectoTramiteFuenteDto>>)resultados).Content);
        }

        [TestMethod]
        public void ObtenerListaProyectosFuentesAprobado_Ok()
        {
            int TramiteId = 921;
           

            var resultados = _tramitesProyectosController.ObtenerListaProyectosFuentesAprobado(TramiteId);
            Assert.IsNotNull(resultados);
        }

        [TestMethod]
        public void InsertaValoresproductosLiberacionVFCorrientes_Ok()
        {
            DetalleProductosCorrientesDto productosCorrientes = new DetalleProductosCorrientesDto();

            var resultados = _tramitesProyectosController.InsertaValoresproductosLiberacionVFCorrientes(productosCorrientes);
            Assert.IsNotNull(resultados);
        }

        [TestMethod]
        public void InsertaValoresproductosLiberacionVFConstantes_Ok()
        {
            DetalleProductosConstantesDto productosConstantes = new DetalleProductosConstantesDto();
            
            var resultados = _tramitesProyectosController.InsertaValoresproductosLiberacionVFConstantes(productosConstantes);
            Assert.IsNotNull(resultados);
        }

        [TestMethod]
        public void ObtenerUsuariosPorInstanciaPadre_Vacio()
        {
           Guid InstanciaId = new Guid("00000000-0000-0000-0000-000000000000");

            var resultados = _tramitesProyectosController.ObtenerUsuariosPorInstanciaPadre(InstanciaId);
            Assert.IsNull(((System.Web.Http.Results.OkNegotiatedContentResult<System.Collections.Generic.List<DNP.ServiciosNegocio.Dominio.Dto.Tramites.DatosUsuarioDto>>)resultados.Result).Content);
        }

        [TestMethod]
        public void ObtenerUsuariosPorInstanciaPadre_Ok()
        {
            Guid InstanciaId = new Guid("4F8A12B7-D7B7-438D-8AFC-5A8FCC1F6D7A");

            var resultados = _tramitesProyectosController.ObtenerUsuariosPorInstanciaPadre(InstanciaId);
            Assert.IsNotNull(resultados);
        }

        public void ObtenerCalendartioPeriodo_OK()
        {
            string bpin = "2021682550006";
            var resultados = _tramitesProyectosController.ObtenerCalendartioPeriodo(bpin);
            Assert.IsNotNull(resultados);

        }

        public void ObtenerCalendartioPeriodo_Vacio()
        {
            string bpin = string.Empty;
            var resultados = _tramitesProyectosController.ObtenerCalendartioPeriodo(bpin);
            Assert.IsNotNull(resultados);

        }

        [TestMethod]
        public void ObtenerPresupuestalProyectosAsociados_Ok()
        {
            Guid InstanciaId = new Guid("DA17BE55-9F4D-4F1A-AF20-E6ED276FDCBF");

            var resultados = _tramitesProyectosController.ObtenerPresupuestalProyectosAsociados(1090, InstanciaId);
            Assert.IsNotNull(resultados);
        }

        [TestMethod]
        public void ObtenerPresupuestalProyectosAsociados_Adicion_Ok()
        {
            Guid InstanciaId = new Guid("DA17BE55-9F4D-4F1A-AF20-E6ED276FDCBF");

            var resultados = _tramitesProyectosController.ObtenerPresupuestalProyectosAsociados_Adicion(1179, InstanciaId);
            Assert.IsNotNull(resultados);
        }

        [TestMethod]
        public void GetOrigenRecursosTramite_Ok()
        {
            var resultados = _tramitesProyectosController.GetOrigenRecursosTramite(1090);
            Assert.IsNotNull(resultados);
        }

        [TestMethod]
        public void SetOrigenRecursosTramite_Ok()
        {
            OrigenRecursosDto origenRecurso = new OrigenRecursosDto();

            var resultados = _tramitesProyectosController.SetOrigenRecursosTramite(origenRecurso);
            Assert.IsNotNull(resultados);
        }
    }
}
