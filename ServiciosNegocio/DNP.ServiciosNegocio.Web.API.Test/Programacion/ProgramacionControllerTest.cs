using DNP.ServiciosNegocio.Comunes.Autorizacion;
using DNP.ServiciosNegocio.Servicios.Interfaces.Programacion;
using DNP.ServiciosNegocio.Web.API.Controllers.Programacion;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Unity;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web.Http.Results;
using DNP.ServiciosNegocio.Comunes.Dto.Programacion;
using System.Collections.Generic;
using DNP.ServiciosNegocio.Dominio.Dto.ProgramacionDistribucion;
using DNP.ServiciosNegocio.Dominio.Dto.Tramites;
using DNP.ServiciosNegocio.Dominio.Dto.Programacion;
using DNP.ServiciosNegocio.Dominio.Dto.ProgramacionFuente;

namespace DNP.ServiciosNegocio.Web.API.Test.Programacion
{
    [TestClass]
    public class ProgramacionControllerTest : IDisposable
    {
        private IProgramacionServicio ProgramacionServicio { get; set; }
        private IAutorizacionUtilidades AutorizacionUtilizades { get; set; }
        private ProgramacionController _programacionController;

        [TestInitialize]
        public void Init()
        {
            var contenedor = Configuracion.UnityConfig.Container;
            ProgramacionServicio = contenedor.Resolve<IProgramacionServicio>();
            AutorizacionUtilizades = contenedor.Resolve<IAutorizacionUtilidades>();
            _programacionController = new ProgramacionController(ProgramacionServicio, AutorizacionUtilizades);
            _programacionController.ControllerContext.Request = new HttpRequestMessage();
            _programacionController.ControllerContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "Z3VlY2hldmVycnlAZG5wLmdvdi5jbzoxMjM0");
            _programacionController.ControllerContext.Request.Headers.Add("piip-idAplicacion", "AP:Backbone");
            _programacionController.User = new GenericPrincipal(new GenericIdentity("jdelgado", "Qwer1234"), new[] { "" });
        }

        [TestMethod]
        public async Task ValidarCalendarioProgramacion_NoNulo()
        {
            var entityTypeCatalogOptionId = 63;
            var nivelId = "00000000-0000-0000-0000-000000000000";
            var seccionCapituloId = 10;
            var result = (OkNegotiatedContentResult<bool>)await _programacionController.ValidarCalendarioProgramacion(entityTypeCatalogOptionId, new Guid(nivelId), seccionCapituloId);
            Assert.IsNotNull(result.Content);
        }

        [TestMethod]
        public async Task ObtenerCargaMasivaCreditos_NoNulo()
        {
            var result = (OkNegotiatedContentResult<string>)await _programacionController.ObtenerCargaMasivaCreditos();
            Assert.IsNotNull(result.Content);
        }

        [TestMethod]
        public async Task ObtenerProgramacionProyectosSinPresupuestal_NoNulo()
        {
            var sectorId = 10;
            var entidadId = 63;
            var proyectoId = "97706";
            var result = (OkNegotiatedContentResult<string>)await _programacionController.ObtenerProgramacionProyectosSinPresupuestal(sectorId, entidadId, proyectoId);
            Assert.IsNotNull(result.Content);
        }

        [TestMethod]
        public async Task ObtenerCargaMasivaCuotas_NoNulo()
        {
            var Vigencia = 2024;
            var EntityTypeCatalogOptionId = 37;
            var result = (OkNegotiatedContentResult<string>)await _programacionController.ObtenerCargaMasivaCuotas(Vigencia, EntityTypeCatalogOptionId);
            Assert.IsNotNull(result.Content);
        }

        [TestMethod]
        public async Task ObtenerProgramacionSectores_NoNulo()
        {
            var sectorId = 14;
            var result = (OkNegotiatedContentResult<string>)await _programacionController.ObtenerProgramacionSectores(sectorId);
            Assert.IsNotNull(result.Content);
        }

        [TestMethod]
        public async Task ObtenerProgramacionEntidadesSector_NoNulo()
        {
            var sectorId = 14;
            var result = (OkNegotiatedContentResult<string>)await _programacionController.ObtenerProgramacionEntidadesSector(sectorId);
            Assert.IsNotNull(result.Content);
        }

        [TestMethod]
        public async Task ObtenerCalendarioProgramacion_NoNulo()
        {
            var flujoId = "00000000-0000-0000-0000-000000000000";
            var result = (OkNegotiatedContentResult<string>)await _programacionController.ObtenerCalendarioProgramacion(new Guid(flujoId));
            Assert.IsNotNull(result.Content);
        }

        [TestMethod]
        public void RegistrarCargaMasivaCreditos_Test()
        {
            List<CargueCreditoDto> lstCargueCreditoDto = new List<CargueCreditoDto>();
            CargueCreditoDto cargueCreditoDto = new CargueCreditoDto()
            {
                Id = 39,
                EntityTypeCatalogOptionId = 62,
                CodigoEntidad = "240101",
                Entidad = "Ministerio De Transporte - Gestion General",
                Codigo = "108",
                EstadoId = 1,
                NombreEstadoCredito = "Firmado",
                NombreCredito = "Digitalización del Senado de República",
                Monto = 39450000000,
                Vigencia = 2024,
                TipoId = 1,
                TipoCredito = "Credito"
            };
            lstCargueCreditoDto.Add(cargueCreditoDto);

            string result = "OK";

            try
            {
                _ = _programacionController.RegistrarCargaMasivaCreditos(lstCargueCreditoDto);
            }
            catch
            {
                result = "ERROR";
            }

            Assert.IsNotNull(result);
        }

        public async Task ObtenerDatosProgramacionEncabezado_NoNulo()
        {
            int EntidadDestinoId = 155;
            int TramiteId = 2290;
            string origen = "Distribucion";
            var result = (OkNegotiatedContentResult<string>)await _programacionController.ObtenerDatosProgramacionEncabezado(EntidadDestinoId, TramiteId, origen);
            Assert.IsNotNull(result.Content);
        }

        public void GuardarDatosProgramacionDistribucion_test()
        {
            List<ValoresProgramacion> listaValoresProgramcion = new List<ValoresProgramacion>();
            ValoresProgramacion valoresProgramacion = new ValoresProgramacion
            {
                ProyectoId = 8492,
                DistribucionCuotaComunicadaNacionCSF = 8492,
                DistribucionCuotaComunicadaNacionSSF = 0,
                DistribucionCuotaComunicadaPropios = 0
            };
            listaValoresProgramcion.Add(valoresProgramacion);
            valoresProgramacion = new ValoresProgramacion
            {
                ProyectoId = 31618,
                DistribucionCuotaComunicadaNacionCSF = 31618,
                DistribucionCuotaComunicadaNacionSSF = 0,
                DistribucionCuotaComunicadaPropios = 0
            };
            listaValoresProgramcion.Add(valoresProgramacion);
            valoresProgramacion = new ValoresProgramacion
            {
                ProyectoId = 111511,
                DistribucionCuotaComunicadaNacionCSF = 111511,
                DistribucionCuotaComunicadaNacionSSF = 0,
                DistribucionCuotaComunicadaPropios = 0
            };
            listaValoresProgramcion.Add(valoresProgramacion);

            ProgramacionDistribucionDto ProgramacionDistribucion = new ProgramacionDistribucionDto
            {
                TramiteId = 2290,
                NivelId = "592B40A9-EE35-4544-8300-031E0F6C249D",
                SeccionCapitulo = 737,
                ValoresProgramacion = listaValoresProgramcion

            };
            string result = "OK";

            try
            {
                _ = _programacionController.GuardarDatosProgramacionDistribucion(ProgramacionDistribucion);
            }
            catch
            {
                result = "ERROR";
            }
            Assert.IsNotNull(result);
        }

        public async Task ObtenerDatosProgramacionDetalle_NoNulo()
        {
            int tramiteidProyectoId = 8450;
            string origen = "FUENTE";
            var result = (OkNegotiatedContentResult<string>)await _programacionController.ObtenerDatosProgramacionDetalle(tramiteidProyectoId, origen);
            Assert.IsNotNull(result.Content);

        }

        [TestMethod]
        public async Task ValidarCargaMasivaCreditos_NoNulo()
        {
            List<CargueCreditoDto> lstCargueCreditoDto = new List<CargueCreditoDto>();
            CargueCreditoDto cargueCreditoDto = new CargueCreditoDto()
            {
                Id = 39,
                EntityTypeCatalogOptionId = 62,
                CodigoEntidad = "240101",
                Entidad = "Ministerio De Transporte - Gestion General",
                Codigo = "108",
                EstadoId = 1,
                NombreEstadoCredito = "Firmado",
                NombreCredito = "Digitalización del Senado de República",
                Monto = 39450000000,
                Vigencia = 2024,
                TipoId = 1,
                TipoCredito = "Credito"
            };
            lstCargueCreditoDto.Add(cargueCreditoDto);
            var result = (OkNegotiatedContentResult<string>)await _programacionController.ValidarCargaMasivaCreditos(lstCargueCreditoDto);
            Assert.IsNotNull(result.Content);
        }

        [TestMethod]
        public void RegistrarCargaMasivaCuota_Test()
        {
            List<CargueCuotaDto> lstCargueCuotaDto = new List<CargueCuotaDto>();
            CargueCuotaDto cargueCuotaDto = new CargueCuotaDto()
            {
                EntityTypeCatalogOptionId = 62,
                Codigo = "108",
                Entidad = "Ministerio De Transporte - Gestion General",
                Sector = "Transporte",
                Propios = 540000,
                NacionSSF = 530000,
                NacionCSF = 520000,
                CuotaEntidadProgramacionId = 1,
                CuotaEntidadId = 1,
                Vigencia = 2024
            };
            lstCargueCuotaDto.Add(cargueCuotaDto);

            string result = "OK";

            try
            {
                _ = _programacionController.RegistrarCargaMasivaCuota(lstCargueCuotaDto);
            }
            catch
            {
                result = "ERROR";
            }

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void RegistrarProyectosSinPresupuestal_Test()
        {
            List<ProyectoSinPresupuestalDto> lstProyectoSinPresupuestalDto = new List<ProyectoSinPresupuestalDto>();
            ProyectoSinPresupuestalDto proyectoSinPresupuestalDto = new ProyectoSinPresupuestalDto()
            {
                CodigoPresupuestal = "1301012408060000010000",
                ProyectoId = 4535,
                EntityTypeCatalogOptionId = 185,
                FecDesde = DateTime.Now,
                FecHasta = DateTime.Now,
                CodigoEntidad = "130101",
                Programa = "2408",
                Subprograma = "0600",
                Consecutivo = 1
            };
            lstProyectoSinPresupuestalDto.Add(proyectoSinPresupuestalDto);

            string result = "OK";

            try
            {
                _ = _programacionController.RegistrarProyectosSinPresupuestal(lstProyectoSinPresupuestalDto);
            }
            catch
            {
                result = "ERROR";
            }

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GuardarDatosProgramacionFuente_test()
        {
            List<ValoresFuente> listaValoresProgramcionFuentes = new List<ValoresFuente>();
            List<ValoresCredito> listaValoresProgramcionCredito = new List<ValoresCredito>();
            ValoresFuente valoresProgramacion = new ValoresFuente
            {
                FuenteId = 1,
                NacionCSF = 8492,
                NacionSSF = 0,
                Propios = 0
            };
            listaValoresProgramcionFuentes.Add(valoresProgramacion);
            valoresProgramacion = new ValoresFuente
            {
                FuenteId = 3,
                NacionCSF = 31618,
                NacionSSF = 0,
                Propios = 0
            };
            listaValoresProgramcionFuentes.Add(valoresProgramacion);
            ValoresCredito valoresProgramacionFuente = new ValoresCredito
            {
                CreditoId = 122,
                FuenteId = 3,
                NacionCSF = 31618,
                NacionSSF = 0,
                Propios = 0
            };
            listaValoresProgramcionCredito.Add(valoresProgramacionFuente);
            valoresProgramacionFuente = new ValoresCredito
            {
                CreditoId = 111,
                FuenteId = 3,
                NacionCSF = 31618,
                NacionSSF = 0,
                Propios = 0
            };
            listaValoresProgramcionCredito.Add(valoresProgramacionFuente);


            ProgramacionFuenteDto ProgramacionFuente = new ProgramacionFuenteDto
            {
                TramiteProyectoId = 8450,
                NivelId = "592B40A9-EE35-4544-8300-031E0F6C249D",
                SeccionCapitulo = 737,
                ValoresFuente = listaValoresProgramcionFuentes,
                ValoresCredito = listaValoresProgramcionCredito

            };
            string result = "OK";

            try
            {
                _ = _programacionController.GuardarDatosProgramacionFuentes(ProgramacionFuente);
            }
            catch
            {
                result = "ERROR";
            }
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void RegistrarCalendarioProgramacion_Test()
        {
            List<CalendarioProgramacionDto> lstCalendarioProgramacionDto = new List<CalendarioProgramacionDto>();
            CalendarioProgramacionDto calendarioProgramacionDto = new CalendarioProgramacionDto()
            {
                NombrePaso = "Programacion",
                NombreSeccion = "Recursos",
                FechaDesde = DateTime.Now,
                FechaHasta = DateTime.Now,
                MacroprocesoSeccionId = 294,
                SeccionId = 65,
                AccionesFlujosId = 6,
                FaseId = 69,
                CalendarioId = 9
            };
            lstCalendarioProgramacionDto.Add(calendarioProgramacionDto);

            string result = "OK";

            try
            {
                _ = _programacionController.RegistrarCalendarioProgramacion(lstCalendarioProgramacionDto);
            }
            catch
            {
                result = "ERROR";
            }

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task ValidarConsecutivoPresupuestal_NoNulo()
        {
            List<ProyectoSinPresupuestalDto> lstProyectoSinPresupuestalDto = new List<ProyectoSinPresupuestalDto>();
            ProyectoSinPresupuestalDto proyectoSinPresupuestalDto = new ProyectoSinPresupuestalDto()
            {
                CodigoPresupuestal = "1301012408060000010000",
                ProyectoId = 4535,
                EntityTypeCatalogOptionId = 185,
                FecDesde = DateTime.Now,
                FecHasta = DateTime.Now,
                CodigoEntidad = "130101",
                Programa = "2408",
                Subprograma = "0600",
                Consecutivo = 1
            };
            lstProyectoSinPresupuestalDto.Add(proyectoSinPresupuestalDto);
            var result = (OkNegotiatedContentResult<string>)await _programacionController.ValidarConsecutivoPresupuestal(lstProyectoSinPresupuestalDto);
            Assert.IsNotNull(result.Content);
        }

        [TestMethod]
        public async Task ValidarCargaMasivaCuotas_NoNulo()
        {
            List<CargueCuotaDto> lstCargueCuotaDto = new List<CargueCuotaDto>();
            CargueCuotaDto cargueCuotaDto = new CargueCuotaDto()
            {
                EntityTypeCatalogOptionId = 62,
                Codigo = "108",
                Entidad = "Ministerio De Transporte - Gestion General",
                Sector = "Transporte",
                Propios = 540000,
                NacionSSF = 530000,
                NacionCSF = 520000,
                CuotaEntidadProgramacionId = 1,
                CuotaEntidadId = 1,
                Vigencia = 2024
            };
            lstCargueCuotaDto.Add(cargueCuotaDto);
            var result = (OkNegotiatedContentResult<string>)await _programacionController.ValidarCargaMasivaCuotas(lstCargueCuotaDto);
            Assert.IsNotNull(result.Content);
        }
        [TestMethod]
        public async Task ObtenerDatosProgramacionProducto_NoNulo()
        {
            int TramiteId = 2290;
            var result = (OkNegotiatedContentResult<string>)await _programacionController.ObtenerDatostProgramacionProducto(TramiteId);
            Assert.IsNotNull(result.Content);
        }

        [TestMethod]
        public void GuardarDatosProgramacionProducto_NoNulo()
        {
            List<ProgramacionProductos> listaValoresProgramacionProductos = new List<ProgramacionProductos>();

            ProgramacionProductos ProgramcionProductos = new ProgramacionProductos
            {
                ProductCatalogId = 1213,
                Meta = 111,
                Recurso = 1236
            };
            listaValoresProgramacionProductos.Add(ProgramcionProductos);
            ProgramcionProductos = new ProgramacionProductos
            {
                ProductCatalogId = 2615,
                Meta = 2615,
                Recurso = 2615
            };
            listaValoresProgramacionProductos.Add(ProgramcionProductos);
            ProgramcionProductos = new ProgramacionProductos
            {
                ProductCatalogId = 856,
                Meta = 856,
                Recurso = 856
            };
            listaValoresProgramacionProductos.Add(ProgramcionProductos);


            ProgramacionProductoDto ProgramacionProducto = new ProgramacionProductoDto
            {
                TramiteId = 2290,
                NivelId = "592B40A9-EE35-4544-8300-031E0F6C249D",
                SeccionCapitulo = 737,
                ProgramacionProductos = listaValoresProgramacionProductos

            };
            string result = "OK";

            try
            {
                _ = _programacionController.GuardarDatosProgramacionProducto(ProgramacionProducto);
            }
            catch
            {
                result = "ERROR";
            }
            Assert.IsNotNull(result);
        }
        [TestMethod]
        public void GuardarDatosProgramacionIniciativa_NoNulo()
        {
            List<Iniciativa> listaValoresIniciativa = new List<Iniciativa>();

            Iniciativa ProgramcionIniciativa = new Iniciativa
            {
                Id = 0,
                IniciativaId = 10
            };
            listaValoresIniciativa.Add(ProgramcionIniciativa);
            ProgramcionIniciativa = new Iniciativa
            {
                Id = 0,
                IniciativaId = 15
            };
            listaValoresIniciativa.Add(ProgramcionIniciativa);



            ProgramacionIniciativaDto ProgramacionIniciativa = new ProgramacionIniciativaDto
            {
                TramiteProyectoId = 8650,
                SeccionCapitulo = 737,
                Iniciativa = listaValoresIniciativa

            };
            string result = "OK";

            try
            {
                _ = _programacionController.GuardarDatosProgramacionIniciativa(ProgramacionIniciativa);
            }
            catch
            {
                result = "ERROR";
            }
            Assert.IsNotNull(result);
        }
        [TestMethod]
        public void BorrarTramiteProyecto_NoNulo()
        {
            List<ValoresProgramacion> listaValoresProgramcion = new List<ValoresProgramacion>();
            ValoresProgramacion valoresProgramacion = new ValoresProgramacion
            {
                ProyectoId = 8492,
                DistribucionCuotaComunicadaNacionCSF = 0,
                DistribucionCuotaComunicadaNacionSSF = 0,
                DistribucionCuotaComunicadaPropios = 0
            };
            listaValoresProgramcion.Add(valoresProgramacion);
            valoresProgramacion = new ValoresProgramacion
            {
                ProyectoId = 31618,
                DistribucionCuotaComunicadaNacionCSF = 0,
                DistribucionCuotaComunicadaNacionSSF = 0,
                DistribucionCuotaComunicadaPropios = 0
            };
            listaValoresProgramcion.Add(valoresProgramacion);
            valoresProgramacion = new ValoresProgramacion
            {
                ProyectoId = 111511,
                DistribucionCuotaComunicadaNacionCSF = 111511,
                DistribucionCuotaComunicadaNacionSSF = 0,
                DistribucionCuotaComunicadaPropios = 0
            };
            listaValoresProgramcion.Add(valoresProgramacion);

            ProgramacionDistribucionDto ProgramacionDistribucion = new ProgramacionDistribucionDto
            {
                TramiteId = 2290,
                NivelId = "",
                SeccionCapitulo = 0,
                ValoresProgramacion = listaValoresProgramcion

            };
            string result = "OK";

            try
            {
                _ = _programacionController.GuardarDatosProgramacionDistribucion(ProgramacionDistribucion);
            }
            catch
            {
                result = "ERROR";
            }
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task ObtenerProgramacionBuscarProyecto_NoNulo()
        {
            int EntidadDestinoId = 204;
            int tramiteid = 2291;
            string bpin = "";
            string NombreProyecto = "";
            var result = (OkNegotiatedContentResult<string>)await _programacionController.ObtenerProgramacionBuscarProyecto(EntidadDestinoId, tramiteid, bpin, NombreProyecto);
            Assert.IsNotNull(result.Content);

        }

        [TestMethod]
        public void GuardarPoliticasTransversalesCategoriasProgramacion()
        {
            PoliticasTransversalesCategoriasProgramacionDto politicasTransversalesCategoriasProgramacionDto = new PoliticasTransversalesCategoriasProgramacionDto();

            politicasTransversalesCategoriasProgramacionDto.DatosDimension = new List<DatosDimension>();

            politicasTransversalesCategoriasProgramacionDto.DatosDimension.Add(new DatosDimension() { DimensionId = 0 });
            string result = "OK";

            try
            {
                _ = _programacionController.GuardarPoliticasTransversalesCategoriasProgramacion(politicasTransversalesCategoriasProgramacionDto);
            }
            catch
            {
                result = "ERROR";
            }
            Assert.IsNotNull(result);
        }
       

       
        [TestMethod]
        public void GuardarDatosInclusion_test()
        {
            List<ValoresProgramacion> listaValoresProgramcion = new List<ValoresProgramacion>();
            ValoresProgramacion valoresProgramacion = new ValoresProgramacion
            {
                ProyectoId = 111913,
                DistribucionCuotaComunicadaNacionCSF = 0,
                DistribucionCuotaComunicadaNacionSSF = 0,
                DistribucionCuotaComunicadaPropios = 0
            };
            listaValoresProgramcion.Add(valoresProgramacion);
          

            ProgramacionDistribucionDto ProgramacionDistribucion = new ProgramacionDistribucionDto
            {
                TramiteId = 2313,
                NivelId = "",
                SeccionCapitulo = 0,
                ValoresProgramacion = listaValoresProgramcion

            };
            string result = "OK";

            try
            {
                _ = _programacionController.GuardarDatosInclusion(ProgramacionDistribucion);
            }
            catch
            {
                result = "ERROR";
            }
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void EliminarCategoriasProyectoProgramacion()
        {
            EliminarCategoriasProyectoProgramacionDto eliminarCategoriasProyectoProgramacionDto = new EliminarCategoriasProyectoProgramacionDto();

            eliminarCategoriasProyectoProgramacionDto.DimensionId = 1;
            string result = "OK";

            try
            {
                _ = _programacionController.EliminarCategoriasProyectoProgramacion(eliminarCategoriasProyectoProgramacionDto);
            }
            catch
            {
                result = "ERROR";
            }
            Assert.IsNotNull(result);
        }



        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                // dispose managed resources
                _programacionController.Dispose();
            }
            // free native resources
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
}
