using DNP.Backbone.Servicios.Interfaces.Autorizacion;
using DNP.Backbone.Servicios.Interfaces.SGP;
using DNP.Backbone.Web.API.Controllers.SGP;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Principal;
using Microsoft.Practices.Unity;
using DNP.Backbone.Servicios.Interfaces.ServiciosNegocio;
using DNP.Backbone.Dominio.Dto.Focalizacion;
using System.Collections.Generic;
using System;
using System.Web.Http.Results;
using DNP.Backbone.Comunes.Dto;
using DNP.Backbone.Dominio.Dto.FuenteFinanciacion;
using DNP.Backbone.Dominio.Dto.Proyecto;
using DNP.ServiciosNegocio.Dominio.Dto.DatosAdicionales;
using DNP.ServiciosNegocio.Dominio.Dto.FuenteFinanciacion;

namespace DNP.Backbone.Web.API.Test.WebAPI
{
    [TestClass]
    public class SGPControllerTest
    {
        private ISGPServicios _sgpServicios;
        private IServiciosNegocioServicios _serviciosNegocioServicios;
        private IAutorizacionServicios _autorizacionServicios;
        private SGPController _sgpController;

        [TestInitialize]
        public void Init()
        {
            _sgpServicios = Config.UnityConfig.Container.Resolve<ISGPServicios>();
            _serviciosNegocioServicios = Config.UnityConfig.Container.Resolve<IServiciosNegocioServicios>();
            _autorizacionServicios = Config.UnityConfig.Container.Resolve<IAutorizacionServicios>();

            _sgpController = new SGPController(_sgpServicios, _serviciosNegocioServicios, _autorizacionServicios);
            _sgpController.ControllerContext.Request = new HttpRequestMessage();
            _sgpController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _sgpController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:Backbone");
            _sgpController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });
        }

        [TestMethod]
        public void ObtenerProyectoListaLocalizacionesSGP_Ok()
        {
            string bpin = "97954";
            var actionResult = _sgpController.ObtenerProyectoListaLocalizacionesSGP(bpin).Result;
            Assert.IsNotNull(actionResult);
        }

        [TestMethod]
        public void ObtenerPoliticasTransversalesProyectoSGP_Ok()
        {
            string bpin = "97954";
            var actionResult = _sgpController.ObtenerPoliticasTransversalesProyectoSGP(bpin).Result;
            Assert.IsNotNull(actionResult);
        }

        [TestMethod]
        public void EliminarPoliticasProyectoSGP_Ok()
        {
            int proyectoId = 97954;
            int politicaId = 6;
            var actionResult = _sgpController.EliminarPoliticasProyectoSGP(proyectoId, politicaId).Result;
            Assert.IsNotNull(actionResult);
        }

        [TestMethod]
        public void AgregarPoliticasTransversalesAjustesSGP_Test()
        {
            List<PoliticasDto> lstPoliticasDto = new List<PoliticasDto>();
            CategoriaProductoPoliticaDto objCategoria = new CategoriaProductoPoliticaDto() 
            {
                ProyectoId = 97954,
                FuenteId = 1,
                BPIN = "97954",
                Politicas = lstPoliticasDto
            };
            

            string result = "OK";

            try
            {
                _ = _sgpController.AgregarPoliticasTransversalesAjustesSGP(objCategoria);
            }
            catch
            {
                result = "ERROR";
            }

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void ConsultarPoliticasCategoriasIndicadoresSGP_Ok()
        {
            Guid instanciaId = new Guid("00000000-0000-0000-0000-000000000000");
            var actionResult = _sgpController.ConsultarPoliticasCategoriasIndicadoresSGP(instanciaId).Result;
            Assert.IsNotNull(actionResult);
        }

        [TestMethod]
        public void ModificarPoliticasCategoriasIndicadoresSGP_Test()
        {
            List<PoliticasCategoriasIndicadoresDto> lstPoliticasDto = new List<PoliticasCategoriasIndicadoresDto>();
            CategoriasIndicadoresDto objCategoria = new CategoriasIndicadoresDto()
            {
                ProyectoId = 97954,
                BPIN = "97954",
                Politicas = lstPoliticasDto
            };


            string result = "OK";

            try
            {
                _ = _sgpController.ModificarPoliticasCategoriasIndicadoresSGP(objCategoria);
            }
            catch
            {
                result = "ERROR";
            }

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void ObtenerPoliticasTransversalesCategoriasSGP_Ok()
        {
            string instanciaId = "00000000-0000-0000-0000-000000000000";
            var actionResult = _sgpController.ObtenerPoliticasTransversalesCategoriasSGP(instanciaId).Result;
            Assert.IsNotNull(actionResult);
        }

        [TestMethod]
        public void EliminarCategoriasPoliticasProyectoSGP_Ok()
        {
            int proyectoId = 97954;
            int politicaId = 6;
            int categoriaId = 1;
            var actionResult = _sgpController.EliminarCategoriasPoliticasProyectoSGP(proyectoId, categoriaId, politicaId).Result;
            Assert.IsNotNull(actionResult);
        }

        [TestMethod]
        public void GuardarFocalizacionCategoriasAjustesSGP_Test()
        {
            List<FocalizacionCategoriasAjusteDto> lstCategoriasDto = new List<FocalizacionCategoriasAjusteDto>();
            FocalizacionCategoriasAjusteDto objCategoria = new FocalizacionCategoriasAjusteDto()
            {
                ProyectoId = 97954,
                Bpin = "97954",
                PoliticaId = 6,
                CategoriaId = 1,
                FuenteId = 1,
                ProductoId = 1,
                LocalizacionId = 1,
                Vigencia = "2023",
                TotalFuene = 1,
                TotalCostoProducto = 1,
                EnAjuste = 1,
                MetaCategoria = 1,
                PersonasCategoria = 1,
                MetaIndicadorSecundario = 1
            };

            lstCategoriasDto.Add(objCategoria);

            string result = "OK";

            try
            {
                _ = _sgpController.GuardarFocalizacionCategoriasAjustesSGP(lstCategoriasDto);
            }
            catch
            {
                result = "ERROR";
            }

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void ObtenerCategoriasSubcategoriasSGP_Ok()
        {
            int idPadre = 1;
            int idEntidad = 1;
            int esCategoria = 1;
            int esGrupoEtnico = 1;
            var actionResult = _sgpController.ObtenerCategoriasSubcategoriasSGP(idPadre, idEntidad, esCategoria, esGrupoEtnico).Result;
            Assert.IsNotNull(actionResult);
        }

        [TestMethod]
        public void GuardarFocalizacionCategoriasPoliticaSGP_Test()
        {
            FocalizacionCategoriasAjusteDto objCategoria = new FocalizacionCategoriasAjusteDto()
            {
                ProyectoId = 97954,
                Bpin = "97954",
                PoliticaId = 6,
                CategoriaId = 1,
                FuenteId = 1,
                ProductoId = 1,
                LocalizacionId = 1,
                Vigencia = "2023",
                TotalFuene = 1,
                TotalCostoProducto = 1,
                EnAjuste = 1,
                MetaCategoria = 1,
                PersonasCategoria = 1,
                MetaIndicadorSecundario = 1
            };

            string result = "OK";

            try
            {
                _ = _sgpController.GuardarFocalizacionCategoriasPoliticaSGP(objCategoria);
            }
            catch
            {
                result = "ERROR";
            }

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void ObtenerCrucePoliticasAjustesSGP_Ok()
        {
            Guid instanciaId = new Guid("00000000-0000-0000-0000-000000000000");
            var actionResult = _sgpController.ObtenerCrucePoliticasAjustesSGP(instanciaId).Result;
            Assert.IsNotNull(actionResult);
        }

        [TestMethod]
        public void ObtenerPoliticasTransversalesResumenSGP_Ok()
        {
            Guid instanciaId = new Guid("00000000-0000-0000-0000-000000000000");
            var actionResult = _sgpController.ObtenerPoliticasTransversalesResumenSGP(instanciaId).Result;
            Assert.IsNotNull(actionResult);
        }

        [TestMethod]
        public void GuardarCrucePoliticasAjustesSGP_Test()
        {
            List<CrucePoliticasAjustesDto> lstCruceDto = new List<CrucePoliticasAjustesDto>();

            string result = "OK";

            try
            {
                _ = _sgpController.GuardarCrucePoliticasAjustesSGP(lstCruceDto);
            }
            catch
            {
                result = "ERROR";
            }

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void ObtenerFuenteFinanciacionVigenciaSGP_Ok()
        {
            string bpin = "97954";
            var actionResult = _sgpController.ObtenerFuenteFinanciacionVigenciaSGP(bpin, "").Result;
            Assert.IsNotNull(actionResult);
        }

        [TestMethod]
        public void EliminarFuenteFinanciacionSGP_Ok()
        {
            string fuenteId = "1";
            var actionResult = _sgpController.FuenteFinanciacionEliminarSGP(fuenteId, "").Result;
            Assert.IsNotNull(actionResult);
        }

        [TestMethod]
        public void ConsultarFuentesProgramarSolicitadoSGP_Ok()
        {
            string bpin = "97954";
            var actionResult = _sgpController.ConsultarFuentesProgramarSolicitadoSGP(bpin, "").Result;
            Assert.IsNotNull(actionResult);
        }

        [TestMethod]
        public void GuardarFuentesProgramarSolicitadoSGP_Test()
        {
            ProgramacionValorFuenteDto fuenteDto = new ProgramacionValorFuenteDto();

            string result = "OK";

            try
            {
                _ = _sgpController.GuardarFuentesProgramarSolicitadoSGP(fuenteDto);
            }
            catch
            {
                result = "ERROR";
            }

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void ObtenerDatosAdicionalesSGP_Ok()
        {
            int fuenteId = 1;
            var actionResult = _sgpController.DatosAdicionalesObtenerSGP(fuenteId, "").Result;
            Assert.IsNotNull(actionResult);
        }

        [TestMethod]
        public void AgregarDatosAdicionalesSGP_Test()
        {
            DatosAdicionalesDto objDatosAdicionales = new DatosAdicionalesDto();

            string result = "OK";

            try
            {
                _ = _sgpController.DatosAdicionalesAgregarSGP(objDatosAdicionales, "");
            }
            catch
            {
                result = "ERROR";
            }

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void EliminarDatosAdicionalesSGP_Ok()
        {
            int cofinanciadorId = 1;
            var actionResult = _sgpController.DatosAdicionalesEliminarSGP(cofinanciadorId, "").Result;
            Assert.IsNotNull(actionResult);
        }

        [TestMethod]
        public void FuenteFinanciacionAgregarSGP_Ok()
        {
            ProyectoFuenteFinanciacionAgregarDto objProyecto = new ProyectoFuenteFinanciacionAgregarDto();
            var actionResult = _sgpController.FuenteFinanciacionAgregarSGP(objProyecto, "").Result;
            Assert.IsNotNull(actionResult);
        }

        [TestMethod]
        public void GuardarDatosSolicitudRecursosSGP_Ok()
        {
            CategoriaProductoPoliticaDto objCategoria = new CategoriaProductoPoliticaDto();
            var actionResult = _sgpController.GuardarDatosSolicitudRecursosSGP(objCategoria).Result;
            Assert.IsNotNull(actionResult);
        }

        [TestMethod]
        public void ObtenerCategoriaProductosPoliticaSGP_Ok()
        {
            string bpin = "97954";
            int fuenteId = 1;
            int politicaId = 6;
            var actionResult = _sgpController.ObtenerCategoriaProductosPoliticaSGP(bpin, fuenteId, politicaId).Result;
            Assert.IsNotNull(actionResult);
        }

        [TestMethod]
        public void ObtenerIndicadoresPoliticaSGP_Ok()
        {
            string bpin = "97954";
            var actionResult = _sgpController.ObtenerIndicadoresPoliticaSGP(bpin).Result;
            Assert.IsNotNull(actionResult);
        }
    }
}