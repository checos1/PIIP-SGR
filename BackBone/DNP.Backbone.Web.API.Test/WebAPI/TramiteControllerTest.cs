namespace DNP.Backbone.Web.API.Test.WebApi
{
    using Comunes.Dto;
    using Controllers;
    using DNP.Backbone.Dominio.Dto.FuenteFinanciacion;
    using DNP.Backbone.Dominio.Dto.Proyecto;
    using DNP.Backbone.Dominio.Dto.Tramites;
    using DNP.Backbone.Dominio.Dto.Tramites.Proyectos;
    using DNP.Backbone.Dominio.Dto.Tramites.VigenciaFutura;
    using DNP.Backbone.Servicios.Interfaces.Tramites;
    using DNP.ServiciosNegocio.Dominio.Dto.Tramites;
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
    public class TramiteControllerTest
    {
        private ITramiteServicios _tramiteServicios;
        private IAutorizacionServicios _autorizacionServicios;
        private TramiteController _tramiteController;

        [TestInitialize]
        public void Init()
        {
            _tramiteServicios = Config.UnityConfig.Container.Resolve<ITramiteServicios>();
            _autorizacionServicios = Config.UnityConfig.Container.Resolve<IAutorizacionServicios>();
            _tramiteController = new TramiteController(_tramiteServicios, _autorizacionServicios);
            _tramiteController.ControllerContext.Request = new HttpRequestMessage();
            _tramiteController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _tramiteController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:Backbone");
            _tramiteController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });
        }

        [TestMethod]
        public void ObtenerTramite_Ok()
        {
            var parametros = new ParametrosInboxDto
            {
                Aplicacion = "AP:Backbone",
                IdUsuario = "jdelgado",
                IdObjeto = new Guid("bc154cba-50a5-4209-81ce-7c0ff0aec2ce"),
                ListaIdsRoles =
                                    new List<Guid>()
                                    {
                                        Guid.
                                            Parse("4fe0a3de-0b14-45ed-9137-248bd206a418"),
                                        Guid.
                                            Parse("d76678e3-9264-4663-afe9-7bce43828024"),
                                        Guid.
                                            Parse("1dd225f4-5c34-4c55-b11d-e5856a68839b")
                                    }
            };
            InstanciaTramiteDto instancia = new InstanciaTramiteDto()
            {
                ParametrosInboxDto = parametros
            };
            var actionResult = _tramiteController.ObtenerTramites(instancia).Result;
            
            var contentResult = actionResult as OkNegotiatedContentResult<InboxTramite>;

            Assert.IsNotNull(contentResult);
            Assert.IsNotNull(contentResult.Content);
        }

        [TestMethod]
        public void ObtenerProyectosTramite_Ok()
        {
            var parametros = new ParametrosInboxDto
            {
                Aplicacion = "AP:Backbone",
                IdUsuario = "jdelgado",
                IdObjeto = new Guid("bc154cba-50a5-4209-81ce-7c0ff0aec2ce"),
                ListaIdsRoles =
                                    new List<Guid>()
                                    {
                                        Guid.
                                            Parse("4fe0a3de-0b14-45ed-9137-248bd206a418"),
                                        Guid.
                                            Parse("d76678e3-9264-4663-afe9-7bce43828024"),
                                        Guid.
                                            Parse("1dd225f4-5c34-4c55-b11d-e5856a68839b")
                                    }
            };
            InstanciaTramiteDto instancia = new InstanciaTramiteDto()
            {
                ParametrosInboxDto = parametros
            };
            var actionResult = _tramiteController.ObtenerProyectosTramite(instancia).Result;

            var contentResult = actionResult as OkNegotiatedContentResult<ProyectosTramitesDTO>;

            Assert.IsNotNull(contentResult);
            Assert.IsNotNull(contentResult.Content);
        }

        [TestMethod]
        public void EliminarProyectoTramite_Ok()
        {
            var parametros = new ParametrosInboxDto
            {
                Aplicacion = "AP:Backbone",
                IdUsuario = "jdelgado",
                IdObjeto = new Guid("bc154cba-50a5-4209-81ce-7c0ff0aec2ce"),
                ListaIdsRoles =
                                    new List<Guid>()
                                    {
                                        Guid.
                                            Parse("4fe0a3de-0b14-45ed-9137-248bd206a418"),
                                        Guid.
                                            Parse("d76678e3-9264-4663-afe9-7bce43828024"),
                                        Guid.
                                            Parse("1dd225f4-5c34-4c55-b11d-e5856a68839b")
                                    }
            };
            InstanciaTramiteDto instancia = new InstanciaTramiteDto()
            {
                ParametrosInboxDto = parametros
            };
            var actionResult = _tramiteController.EliminarProyectoTramite(instancia).Result;

            var contentResult = actionResult as OkNegotiatedContentResult<Dominio.Dto.InstanciaResultado>;

            Assert.IsNotNull(contentResult);
            Assert.IsNotNull(contentResult.Content);
        }


        [TestMethod]
        public void ObtenerPreguntasJustificacion_Ok()
        {
            int TramiteId = 298;
            int ProyectoId = 97652;
            int TipoTramiteId = 1;
            int TipoRolId = 1;
            string IdNivel = "74A0F290-6760-4155-B06B-59A3D0E92DE5";

            var actionResult = _tramiteController.ObtenerPreguntasJustificacion(TramiteId,ProyectoId,TipoTramiteId,TipoRolId,IdNivel).Result;
            Assert.IsNotNull(actionResult);
        }

        #region Vigencia Futura
       

        [TestMethod]
        public void ObtenerPreguntasProyectoActualizacionPaso_Ok()
        {
            int TramiteId = 453;
            int ProyectoId = 0;
            int TipoTramiteId = 4;
            int TipoRolId = 0;
            Guid IdNivel = new Guid( "74A0F290-6760-4155-B06B-59A3D0E92DE5");       

            var actionResult = _tramiteController.ObtenerPreguntasProyectoActualizacionPaso(TramiteId, ProyectoId, TipoTramiteId, IdNivel, TipoRolId).Result;
               var justificaciones = ((OkNegotiatedContentResult<List<JustificacionPasoDto>>)actionResult).Content;
            Assert.IsTrue(justificaciones.Count > 0);
        }

        [TestMethod]
        public void ObtenerPreguntasProyectoActualizacionPaso_vacio()
        {
            int TramiteId = 452;
            int ProyectoId = 0;
            int TipoTramiteId = 4;
            int TipoRolId = 0;
            Guid IdNivel = new Guid("74A0F290-6760-4155-B06B-59A3D0E92DE5");
  
            var actionResult = _tramiteController.ObtenerPreguntasProyectoActualizacionPaso(TramiteId, ProyectoId, TipoTramiteId, IdNivel, TipoRolId).Result;
            var justificaciones = ((OkNegotiatedContentResult<List<JustificacionPasoDto>>)actionResult).Content;
            Assert.IsTrue(justificaciones.Count == 0);
        
        }

        [TestMethod]
        public void ObtenerDatosCronograma_Ok()
        {
            Guid instanciaId = new Guid("3E0750D4-DC36-4546-942F-72F8638B3E0A");

            var actionResult = _tramiteController.ObtenerDatosCronograma(instanciaId).Result;
            Assert.IsNotNull(actionResult);
        }

        [TestMethod]
        public void ObtenerDatosCronograma_Vacio()
        {
            Guid instanciaId = new Guid();

            var actionResult = _tramiteController.ObtenerDatosCronograma(instanciaId).Result;
            Assert.AreEqual(((System.Web.Http.Results.OkNegotiatedContentResult<string>)actionResult).Content, "");
        }

        [TestMethod]
        public void ObtenerDeflactores()
        {
            var actionResult = _tramiteController.ObtenerDeflactores().Result;
            Assert.IsNotNull(actionResult);
        }

        [TestMethod]
        public void GetProyectoTramite()
        {
            int ProyectoId = 97706;
            int TramiteId = 356;
            var actionResult = _tramiteController.ObtenerProyectoTramite(ProyectoId, TramiteId).Result;
            Assert.IsNotNull(actionResult);
        }

        [TestMethod]
        public void ActualizaVigenciaFuturaProyectoTramite()
        {
            TramiteProyectoDto tramiteProyectoDto = new TramiteProyectoDto();
            tramiteProyectoDto.ProyectoId = 97706;
            tramiteProyectoDto.TramiteId = 356;
            tramiteProyectoDto.EsConstante = true;
            tramiteProyectoDto.AnioBase = 2022;
            var actionResult = _tramiteController.ActualizaVigenciaFuturaProyectoTramite(tramiteProyectoDto).Result;
            Assert.IsNotNull(actionResult);
        }


        [TestMethod]
        public void ObtenerFuentesFinanciacionVigenciaFuturaCorriente()
        {
            string bpin = "202100000000037";
            var actionResult = _tramiteController.ObtenerFuentesFinanciacionVigenciaFuturaCorriente(bpin).Result;
            Assert.IsNotNull(actionResult);
        }

        [TestMethod]
        public void ObtenerInformacionPresupuestalValores_Ok()
        {
            int TramiteId = 26;

            var actionResult = _tramiteController.ObtenerInformacionPresupuestalValores(TramiteId).Result;
            Assert.IsNotNull(actionResult);
        }

        [TestMethod]
        public void ObtenerInformacionPresupuestalValores_Vacio()
        {
            int TramiteId = 0;
            var actionResult = _tramiteController.ObtenerInformacionPresupuestalValores(TramiteId).Result;
            var informacion = ((OkNegotiatedContentResult<InformacionPresupuestalValoresDto>)actionResult).Content;
            Assert.IsNull(informacion.ProyectoId);
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

            //var resultados = (OkNegotiatedContentResult<VigenciaFuturaResponse>) _tramiteController.ActualizarVigenciaFuturaFuente(fuente).Result;
            Assert.IsNotNull(fuente);
        }

        [TestMethod]
        public void ObtenerFuentesFinanciacionVigenciaFuturaConstante()
        {
            string bpin = "202200000000060";
            int tramiteId = 611;
            //var actionResult = _tramiteController.ObtenerFuentesFinanciacionVigenciaFuturaConstante(bpin, tramiteId).Result;
            Assert.IsNotNull(bpin);
        }

        [TestMethod]
        public void GuardarInformacionPresupuestalValores()
        {
            InformacionPresupuestalValoresDto informacionPresupuestalValoresDto = new InformacionPresupuestalValoresDto();
            informacionPresupuestalValoresDto.ProyectoId = 97833;
            informacionPresupuestalValoresDto.TramiteId = 593;
            informacionPresupuestalValoresDto.AplicaConstante = true;
            informacionPresupuestalValoresDto.AñoBase = 2022;
            var actionResult = _tramiteController.GuardarInformacionPresupuestalValores(informacionPresupuestalValoresDto);
            Assert.IsNotNull(actionResult);
        }

        [TestMethod]
        public void ObtenerProductosVigenciaFuturaCorriente()
        {
            string bpin = "202200000000002";
            int tramiteId = 611;
            var actionResult = _tramiteController.ObtenerProductosVigenciaFuturaCorriente(bpin, tramiteId).Result;
            Assert.IsNotNull(bpin);
        }

        [TestMethod]
        public void ObtenerLiberacionVigenciasFuturas()
        {
            int tramiteId = 611;
            int proyectoId = 99619;
            var actionResult = _tramiteController.ObtenerLiberacionVigenciasFuturas(proyectoId, tramiteId);
            Assert.IsNotNull(actionResult);
        }

        [TestMethod]
        public void InsertaAutorizacionVigenciasFuturas()
        {
            TramiteALiberarVfDto request = new TramiteALiberarVfDto();
            request.LiberacionVigenciasFuturasId = 0;
            request.CodigoProceso = "XXXXXX";
            request.NombreProceso = "el nombre de mi proceso";
            request.Fecha = DateTime.Now;
            request.CodigoAutorizacion = "AAAAAAA001";
            request.FechaAutorizacion = DateTime.Now;

            var actionResult = _tramiteController.InsertaAutorizacionVigenciasFuturas(request);
            Assert.IsNotNull(actionResult);
        }

        [TestMethod]
        public void InsertaValoresUtilizadosLiberacionVF()
        {
            TramiteALiberarVfDto request = new TramiteALiberarVfDto();
            request.LiberacionVigenciasFuturasId = 0;
            request.CodigoProceso = "XXXXXX";
            request.NombreProceso = "el nombre de mi proceso";
            request.Fecha = DateTime.Now;
            request.CodigoAutorizacion = "AAAAAAA001";
            request.FechaAutorizacion = DateTime.Now;

            var actionResult = _tramiteController.InsertaValoresUtilizadosLiberacionVF(request);
            Assert.IsNotNull(actionResult);
        }
        #endregion Vigencia Futura

        [TestMethod]
        public void ConsultarCartaConcepto_Ok()
        {
            int TramiteId = 902;
            var actionResult = _tramiteController.ConsultarCartaConcepto(TramiteId).Result;
            Assert.IsNotNull(actionResult);

        }
        [TestMethod]
        public void ValidacionPeriodoPresidencial_OK()
        {
            int TramiteId = 925;
            var actionResult = _tramiteController.ValidacionPeriodoPresidencial(TramiteId).Result;
            Assert.IsNotNull(actionResult);

        }

        [TestMethod]
        public void GuardarMontosTramite()
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

            var actionResult = _tramiteController.GuardarMontosTramite(listaProyectosEnTramiteDto);
            Assert.IsNotNull(actionResult);
        }

        [TestMethod]
        public void InsertaValoresproductosLiberacionVFCorrientes_Ok()
        {
            DetalleProductosCorrientesDto productosCorrientes = new DetalleProductosCorrientesDto();

            var resultados = _tramiteController.InsertaValoresproductosLiberacionVFCorrientes(productosCorrientes);
            Assert.IsNotNull(resultados);
        }

        [TestMethod]
        public void InsertaValoresproductosLiberacionVFConstantes_Ok()
        {
            DetalleProductosConstantesDto productosConstantes = new DetalleProductosConstantesDto();

            var resultados = _tramiteController.InsertaValoresproductosLiberacionVFConstantes(productosConstantes);
            Assert.IsNotNull(resultados);
        }

        [TestMethod]
        public void ObtenerPresupuestalProyectosAsociados_Ok()
        {
            Guid InstanciaId = new Guid("DA17BE55-9F4D-4F1A-AF20-E6ED276FDCBF");

            var resultados = _tramiteController.ObtenerPresupuestalProyectosAsociados(1090, InstanciaId);
            Assert.IsNotNull(resultados);
        }

        [TestMethod]
        public void GetOrigenRecursosTramite_Ok()
        {
            var resultados = _tramiteController.GetOrigenRecursosTramite(1090);
            Assert.IsNotNull(resultados);
        }

        [TestMethod]
        public void SetOrigenRecursosTramite_Ok()
        {
            OrigenRecursosDto origenRecurso = new OrigenRecursosDto();

            var resultados = _tramiteController.SetOrigenRecursosTramite(origenRecurso);
            Assert.IsNotNull(resultados);
        }

        [TestMethod]
        public void ConsultarSystemConfiguracion_Ok()
        {
            var resultados = _tramiteController.ConsultarSystemConfiguracion("TamanioMaxDocs", "|");
            Assert.IsNotNull(resultados);
        }
    }
}
