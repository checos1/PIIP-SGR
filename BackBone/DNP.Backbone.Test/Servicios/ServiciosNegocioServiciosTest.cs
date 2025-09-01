namespace DNP.Backbone.Test.Servicios
{
    using Backbone.Servicios.Interfaces.ServiciosNegocio;
    using DNP.Backbone.Dominio.Dto.FuenteFinanciacion;
    using DNP.Backbone.Dominio.Dto.Proyecto;
    using DNP.Backbone.Dominio.Dto.Tramites;
    using DNP.Backbone.Servicios.Interfaces;
    using Microsoft.Practices.Unity;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Collections.Generic;

    [TestClass]
    public class ServiciosNegocioServiciosTest
    {
        private IServiciosNegocioServicios _serviciosNegocioServicios;
        private IClienteHttpServicios _clienteHttpServicios;

        [TestInitialize]
        public void Init()
        {
            //_clienteHttpServicios = Config.UnityConfig.Container.Resolve<IClienteHttpServicios>();
            _serviciosNegocioServicios = Config.UnityConfig.Container.Resolve<IServiciosNegocioServicios>();
        }

        [TestMethod]
        public void ObtenerListaCatalogo_Ok()
        {
            //var parametros = new ProyectoParametrosDto
            //{
            //    Aplicacion = "AP:Backbone",
            //    IdUsuario = "jdelgado",
            //    IdObjeto = new Guid("bc154cba-50a5-4209-81ce-7c0ff0aec2ce"),
            //    ListaIdsRoles =
            //                        new List<Guid>()
            //                        {
            //                            Guid.
            //                                Parse("4fe0a3de-0b14-45ed-9137-248bd206a418"),
            //                            Guid.
            //                                Parse("d76678e3-9264-4663-afe9-7bce43828024"),
            //                            Guid.
            //                                Parse("1dd225f4-5c34-4c55-b11d-e5856a68839b")
            //                        }
            //};
            
            //var actionResult = _serviciosNegocioServicios.ObtenerListaCatalogo(parametros, CatalogoEnum.Sectores).Result;

            //Assert.IsNotNull(actionResult);
        }

        [TestMethod]
        public void ObtenerListaEstado_Ok()
        {
            //    var parametros = new ProyectoParametrosDto
            //    {
            //        Aplicacion = "AP:Backbone",
            //        IdUsuario = "jdelgado",
            //        IdObjeto = new Guid("bc154cba-50a5-4209-81ce-7c0ff0aec2ce"),
            //        ListaIdsRoles =
            //                            new List<Guid>()
            //                            {
            //                                Guid.
            //                                    Parse("4fe0a3de-0b14-45ed-9137-248bd206a418"),
            //                                Guid.
            //                                    Parse("d76678e3-9264-4663-afe9-7bce43828024"),
            //                                Guid.
            //                                    Parse("1dd225f4-5c34-4c55-b11d-e5856a68839b")
            //                            }
            //    };

            //    var actionResult = _serviciosNegocioServicios.ObtenerListaEstado(parametros).Result;

            //    Assert.IsNotNull(actionResult);
        }

        [TestMethod]
        public void ObtenerListaProyectoPorTramite_Ok()
        {
            
            //var parametros = new ParametrosProyectosDto
            //{
            //    IdTramite = 1,
            //    IdUsuarioDNP = "jdelgado",
            //    TokenAutorizacion = "Basic amRlbGdhZG86MjI4OTE1MDA ="
            //};

            //var actionResult = _serviciosNegocioServicios.ObtenerListaProyectoPorTramite(parametros).Result;

            //Assert.IsNotNull(actionResult);
        }

        [TestMethod]
        public void ObtenerFocalizacionPoliticasTransversalesFuentes_Ok()
        {

            //var parametros = new ParametrosProyectosDto
            //{
            //    IdTramite = 1,
            //    IdUsuarioDNP = "jdelgado",
            //    TokenAutorizacion = "Basic amRlbGdhZG86MjI4OTE1MDA ="
            //};

            //var actionResult = _serviciosNegocioServicios.ObtenerListaProyectoPorTramite(parametros).Result;

            //Assert.IsNotNull(actionResult);
        }

        #region Vigencia Futura


        #endregion Vigencia Futura

        [TestMethod]
        public void EliminarCapitulosModificados_Ok()
        {

            string usuario = "Prueb";
            CapituloModificado CapituloModificadoDto = new CapituloModificado();
            CapituloModificadoDto.InstanciaId = new Guid("cc912a51-10f9-4b3f-a81a-00b94a8b913d");
            CapituloModificadoDto.SeccionCapituloId = 4;
            CapituloModificadoDto.ProyectoId = 0;

            var resultados = _serviciosNegocioServicios.EliminarCapitulosModificados(CapituloModificadoDto,usuario);
            Assert.IsNotNull(resultados);
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

            var resultados = _serviciosNegocioServicios.ActualizarVigenciaFuturaFuente(fuente, "usuariodnp").Result;
            Assert.IsNotNull(resultados);
        }

        [TestMethod]
        public void ObtenerListaDirecionesDNP_Ok()
        {
            Guid idEntidad = new Guid("FBB8BAB4-868B-4422-84F7-58B16F556AD6");
            string usuario = "usuarioDNP";
            var actionResult = _serviciosNegocioServicios.ObtenerListaDirecionesDNP(idEntidad, usuario).Result;
            Assert.IsNotNull(actionResult);
        }

        [TestMethod]
        public void ObtenerListaSubdirecionesPorParentId_Ok()
        {
            int idEntityType = 105;
            string usuario = "usuarioDNP";
            var resultados = _serviciosNegocioServicios.ObtenerListaSubdirecionesPorParentId(idEntityType, usuario).Result;
            Assert.IsNotNull(resultados);
        }

        [TestMethod]
        public void ObtenerListaDirecionesDNP_Vacio()
        {
            Guid idEntidad = new Guid();
            string usuario = "usuarioDNP";
            var actionResult = _serviciosNegocioServicios.ObtenerListaDirecionesDNP(idEntidad, usuario);
            Assert.IsNull(actionResult);
        }

        [TestMethod]
        public void ObtenerListaSubdirecionesPorParentId_Vacio()
        {
            int idEntityType = 0;
            string usuario = "usuarioDNP";
            var resultados = _serviciosNegocioServicios.ObtenerListaSubdirecionesPorParentId(idEntityType, usuario);
            Assert.IsNull(resultados);
        }

        [TestMethod]
        public void BorrarFirma_Vacio()
        {
            string usuario = "";
            var resultados = _serviciosNegocioServicios.BorrarFirma(usuario);
            Assert.IsNull(resultados);
        }

        [TestMethod]
        public void BorrarFirma_OK()
        {
            string usuario = "cc202006";
            var resultados = _serviciosNegocioServicios.BorrarFirma(usuario);
            Assert.IsNotNull(resultados);
        }

        [TestMethod]
        public void ObtenerProyectosCartaTramite_Vacio()
        {
            int tramiteId = 0;
            string usuario = "";
            var resultados = _serviciosNegocioServicios.ObtenerProyectosCartaTramite(tramiteId, usuario);
            Assert.IsNull(resultados);
        }

        [TestMethod]
        public void ObtenerProyectosCartaTramite_OK()
        {
            int tramiteId = 1002;
            string usuario = "cc202006";
            var resultados = _serviciosNegocioServicios.ObtenerProyectosCartaTramite(tramiteId, usuario);
            Assert.IsNotNull(resultados);
        }

        [TestMethod]
        public void ObtenerDetalleCartaAL_Vacio()
        {
            int tramiteId = 0;
            string usuario = "";
            var resultados = _serviciosNegocioServicios.ObtenerDetalleCartaAL(tramiteId, usuario);
            Assert.IsNull(resultados);
        }

        [TestMethod]
        public void ObtenerDetalleCartaAL_OK()
        {
            int tramiteId = 1002;
            string usuario = "cc202006";
            var resultados = _serviciosNegocioServicios.ObtenerDetalleCartaAL(tramiteId, usuario);
            Assert.IsNotNull(resultados);
        }

        [TestMethod]
        public void ObtenerDatosProyectoConceptoPorInstancia_Ok()
        {
            Guid instanciaId = new Guid("57C50E30-9721-409C-9D5D-FD49962F92DA");
            string usuarioDnp = "CC202002";
            var resultados = _serviciosNegocioServicios.ObtenerDatosProyectoConceptoPorInstancia(instanciaId, usuarioDnp);
            Assert.IsNotNull(resultados);
        }

        [TestMethod]
        public void ObtenerDatosProyectoConceptoPorInstancia_Vacio()
        {
            Guid instanciaId = new Guid();
            string usuarioDnp = "CC202002";
            var resultados = _serviciosNegocioServicios.ObtenerDatosProyectoConceptoPorInstancia(instanciaId, usuarioDnp);
            Assert.IsNull(resultados);
        }

        [TestMethod]
        public void ObtenerListaProyectosFuentes_Ok()
        {
            int tramiteId = 10;
            string usuarioDnp = "CC202002";
            var resultados = _serviciosNegocioServicios.ObtenerListaProyectosFuentes(tramiteId, usuarioDnp);
            Assert.IsNotNull(resultados);
        }


        [TestMethod]
        public void ObtenerListaProyectosFuentes_Vacio()
        {
            int tramiteId = 0;
            string usuarioDnp = "CC202002";
            var resultados = _serviciosNegocioServicios.ObtenerListaProyectosFuentes(tramiteId, usuarioDnp);
            Assert.IsNull(resultados);
        }

        [TestMethod]
        public void obtenerEntidadAsociarProyecto_Ok()
        {
            Guid instanciaId = new Guid("57C50E30-9721-409C-9D5D-FD49962F92DA");
            string accion = "C";
            string usuarioDnp = "CC202002";
            var resultados = _serviciosNegocioServicios.obtenerEntidadAsociarProyecto(instanciaId,accion, usuarioDnp);
            Assert.IsNotNull(resultados);
        }

        [TestMethod]
        public void obtenerEntidadAsociarProyecto_Vacio()
        {
            Guid instanciaId = new Guid();
            string accion = null;
            string usuarioDnp = "CC202002";
            var resultados = _serviciosNegocioServicios.obtenerEntidadAsociarProyecto(instanciaId, accion, usuarioDnp);
            Assert.IsNull(resultados);
        }

        [TestMethod]
        public void ObtenerEntidadTramite_Ok()
        {
            
            string numeroTramite = "Ej-00-0000";
            string usuarioDnp = "CC202002";
            var resultados = _serviciosNegocioServicios.ObtenerEntidadTramite(numeroTramite, usuarioDnp);
            Assert.IsNotNull(resultados);
        }

        [TestMethod]
        public void ObtenerEntidadTramite_Vacio()
        {
            string numeroTramite = "";
            string usuarioDnp = "CC202002";
            var resultados = _serviciosNegocioServicios.ObtenerEntidadTramite(numeroTramite, usuarioDnp);
            Assert.IsNull(resultados);
        }

        [TestMethod]
        public void EliminarLiberacionVigenciaFutura_Ok()
        {
            LiberacionVigenciasFuturasDto dto = new LiberacionVigenciasFuturasDto { tramiteId = 1020, tramiteProyectoId = 0 };
            string usuarioDnp = "CC202002";
            var resultados = _serviciosNegocioServicios.EliminarLiberacionVigenciaFutura(dto, usuarioDnp);
            Assert.IsNotNull(resultados);
        }

        [TestMethod]
        public void ObtenerCalendartioPeriodo_Ok()
        {
            string bpin = "2021682550006";
            string usuarioDnp = "CC202002";
            var resultados = _serviciosNegocioServicios.ObtenerCalendartioPeriodo(bpin, usuarioDnp);
            Assert.IsNotNull(resultados);
        }

        [TestMethod]
        public void ObtenerCalendartioPeriodo_Vacio()
        {
            string bpin = string.Empty;
            string usuarioDnp = "CC202002";
            var resultados = _serviciosNegocioServicios.ObtenerCalendartioPeriodo(bpin, usuarioDnp);
            Assert.IsNotNull(resultados);
        }

    }
}
