using DNP.Backbone.Comunes.Dto;
using DNP.Backbone.Dominio.Enums;
using DNP.Backbone.Comunes.Enums;
using DNP.Backbone.Dominio.Dto.Programacion;
using DNP.Backbone.Servicios.Interfaces;
using DNP.Backbone.Servicios.Interfaces.Programacion;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DNP.Backbone.Dominio.Dto.Tramites.ProgramacionDistribucion;
using DNP.Backbone.Dominio.Dto.SeguimientoControl;
using DNP.Backbone.Dominio.Dto.Focalizacion;
using DNP.Backbone.Dominio.Dto.Tramites;
using DNP.Backbone.Servicios.Interfaces.Autorizacion;
using DNP.Backbone.Servicios.Implementaciones.AutorizacionNegocio;
using DNP.Backbone.Servicios.Interfaces.ServiciosNegocio;
using DNP.Backbone.Servicios.Implementaciones.Flujos;

namespace DNP.Backbone.Servicios.Implementaciones.Programacion
{
    public class ProgramacionServicios : IProgramacionServicios
    {
        private readonly IClienteHttpServicios _clienteHttpServicios;
        private readonly string ENDPOINT = ConfigurationManager.AppSettings["ApiMotorFlujos"];
        private readonly string urlNegocio = ConfigurationManager.AppSettings["ApiServicioNegocio"];
        private readonly string ENDPOINTSERVICIONEGOCIO = ConfigurationManager.AppSettings["ApiServicioNegocio"];
        private readonly IAutorizacionServicios _autorizacionServicios;
        private readonly IFlujoServicios _flujoServicios;
        public ProgramacionServicios(IClienteHttpServicios clienteHttpServicios, IAutorizacionServicios autorizacionServicios, IFlujoServicios flujoServicios)
        {
            this._clienteHttpServicios = clienteHttpServicios;
            _autorizacionServicios = autorizacionServicios;
            _flujoServicios = flujoServicios;
        }

        /// <summary>
        ///  Obtiene una lista del enum <see cref="EstadoProceso"/>
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable> ObtenerTipoEstadoProceso()
        {

            var lista = new List<CatalogoSimple>();
            try
            {
                lista = ((EstadoProceso[])Enum.GetValues(typeof(EstadoProceso))).Select(p => new CatalogoSimple
                {
                    Identificador = p,
                    Nombre = p.GetDescription()
                })
                .ToList();
            }
            catch (Exception exception)
            {
                throw new Exception($"ProgramacionServicio.ObtenerTipoEstadoProceso : {exception.Message}\\n{exception.InnerException?.Message ?? String.Empty}");
            }

            return await Task.FromResult(lista);
        }

        public async Task<IEnumerable<ProgramacionDto>> ObtenerProgramaciones(string tipoEntidad, Guid? capituloId, DateTime? fechaInicio, DateTime? fechaFin, EstadoProceso? estado, string usuarioDnp)
        {
            var lista = new List<ProgramacionDto>();

            var uriMetodo = ConfigurationManager.AppSettings["uriProgramacionObtenerPorTipoEntidad"];
            var parametros = $"?tipoEntidad={tipoEntidad}";

            lista = JsonConvert.DeserializeObject<IEnumerable<ProgramacionDto>>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, ENDPOINT, uriMetodo, parametros, null, usuarioDnp, useJWTAuth: false)).ToList();


            // filtrar lista por los nuevos parámetros de filtros
            lista = lista.Where(p =>
                p.FlujoTramite.Id == (capituloId ?? p.FlujoTramite.Id) &&
                p.FechaDesde == (fechaInicio ?? p.FechaDesde) &&
                p.FechaHasta == (fechaFin ?? p.FechaHasta) &&
                p.EstadoProceso == (estado ?? p.EstadoProceso)
                ).ToList();

            return lista;
        }

        public async Task<RespuestaGeneralDto> GuardarProgramacion(ProgramacionDto programacionDto, string usuarioDnp)
        {
            return programacionDto.IdProgramacion == 0 ? await CrearProgramacion(programacionDto, usuarioDnp) : await EditarProgramacion(programacionDto, usuarioDnp);
        }

        private async Task<RespuestaGeneralDto> CrearProgramacion(ProgramacionDto programacionDto, string usuarioDnp)
        {
            try
            {
                var uriMetodo = ConfigurationManager.AppSettings["uriProgramacionCrear"];

                var respuesta = JsonConvert.DeserializeObject<int>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, ENDPOINT, uriMetodo, string.Empty, programacionDto, usuarioDnp, useJWTAuth: false));

                return new RespuestaGeneralDto()
                {
                    IdRegistro = respuesta.ToString(),
                    Exito = true
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private async Task<RespuestaGeneralDto> EditarProgramacion(ProgramacionDto programacionDto, string usuarioDnp)
        {
            try
            {
                var uriMetodo = ConfigurationManager.AppSettings["uriProgramacionEditar"];

                var respuesta = JsonConvert.DeserializeObject<bool>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, ENDPOINT, uriMetodo, string.Empty, programacionDto, usuarioDnp, useJWTAuth: false));

                return new RespuestaGeneralDto()
                {
                    Exito = respuesta
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<RespuestaGeneralDto> EliminarProgramacion(ProgramacionDto programacionDto, string usuarioDnp)
        {
            try
            {
                var uriMetodo = ConfigurationManager.AppSettings["uriProgramacionEliminar"];

                var respuesta = JsonConvert.DeserializeObject<bool>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, ENDPOINT, uriMetodo, string.Empty, programacionDto, usuarioDnp, useJWTAuth: false));

                return new RespuestaGeneralDto()
                {
                    Exito = respuesta
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<ProgramacionExcepcionDto>> ObtenerProgramacionExcepciones(int idProgramacion, string usuarioDnp)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriProgramacionObtenerExcepciones"];
            var parametros = $"?idProgramacion={idProgramacion}";

            return JsonConvert.DeserializeObject<IEnumerable<ProgramacionExcepcionDto>>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, ENDPOINT, uriMetodo, parametros, null, usuarioDnp, useJWTAuth: false));
        }

        public async Task<RespuestaGeneralDto> GuardarProgramacionExcepcion(ProgramacionExcepcionDto programacionDto, string usuarioDnp)
        {
            try
            {
                var uriMetodo = ConfigurationManager.AppSettings["uriProgramacionCrearExcepcion"];

                var respuesta = JsonConvert.DeserializeObject<int>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, ENDPOINT, uriMetodo, string.Empty, programacionDto, usuarioDnp, useJWTAuth: false));

                return new RespuestaGeneralDto()
                {
                    IdRegistro = respuesta.ToString(),
                    Exito = true
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<RespuestaGeneralDto> EditarProgramacionExcepcion(ProgramacionExcepcionDto programacionDto, string usuarioDnp)
        {
            try
            {
                var uriMetodo = ConfigurationManager.AppSettings["uriProgramacionEditarExcepcion"];

                var respuesta = JsonConvert.DeserializeObject<bool>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, ENDPOINT, uriMetodo, string.Empty, programacionDto, usuarioDnp, useJWTAuth: false));

                return new RespuestaGeneralDto()
                {
                    Exito = respuesta
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<RespuestaGeneralDto> EliminarProgramacionExcepcion(ProgramacionExcepcionDto programacionDto, string usuarioDnp)
        {
            try
            {
                var uriMetodo = ConfigurationManager.AppSettings["uriProgramacionEliminarExcepcion"];

                var respuesta = JsonConvert.DeserializeObject<bool>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, ENDPOINT, uriMetodo, string.Empty, programacionDto, usuarioDnp, useJWTAuth: false));

                return new RespuestaGeneralDto()
                {
                    Exito = respuesta
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<RespuestaGeneralDto> CrearPeriodo(string tipoEntidad, string usuarioDnp)
        {
            try
            {
                var uriMetodo = ConfigurationManager.AppSettings["uriProgramacionCrearPeriodo"];

                var respuesta = JsonConvert.DeserializeObject<bool>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, ENDPOINT, uriMetodo, string.Empty, tipoEntidad, usuarioDnp, useJWTAuth: false));

                return new RespuestaGeneralDto()
                {
                    Exito = respuesta
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<RespuestaGeneralDto> IniciarProceso(string tipoEntidad, string usuarioDnp)
        {
            try
            {
                var uriMetodo = ConfigurationManager.AppSettings["uriProgramacionIniciarProceso"];

                var respuesta = JsonConvert.DeserializeObject<bool>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, ENDPOINT, uriMetodo, string.Empty, tipoEntidad, usuarioDnp, useJWTAuth: false));

                return new RespuestaGeneralDto()
                {
                    Exito = respuesta
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<RespuestaGeneralDto> GuardarConfiguracionMensaje(dynamic configuracionMensaje, string usuarioDnp)
        {
            try
            {
                var programacionDto = new ProgramacionDto();


                //var uriMetodo = ConfigurationManager.AppSettings["uriProgramacionConfiguracionMensaje"];
                var uriMetodo = ConfigurationManager.AppSettings["uriProgramacionEditar"];

                var respuesta = JsonConvert.DeserializeObject<bool>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, ENDPOINT, uriMetodo, string.Empty, programacionDto, usuarioDnp, useJWTAuth: false));


                return new RespuestaGeneralDto()
                {
                    Exito = respuesta
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<string> ObtenerDatosProgramacionEncabezado(int EntidadDestinoId, int TramiteId, string origen, string usuarioDnp)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriObtenerDatosProgramacionEncabezado"];
            uriMetodo += "?EntidadDestinoId=" + EntidadDestinoId + "&TramiteId=" + TramiteId + "&origen=" + origen;

            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlNegocio, uriMetodo, string.Empty, null, usuarioDnp, useJWTAuth: false);
            if (string.IsNullOrEmpty(respuesta)) return string.Empty;
            return respuesta;
        }

        public async Task<string> ObtenerDatosProgramacionDetalle(int TramiteProyectoId, string origen, string usuarioDnp)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriObtenerDatosProgramacionDetalle"];
            uriMetodo += "?tramiteidProyectoId=" + TramiteProyectoId + "&origen=" + origen;

            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlNegocio, uriMetodo, string.Empty, null, usuarioDnp, useJWTAuth: false);
            if (string.IsNullOrEmpty(respuesta)) return string.Empty;
            return respuesta;
        }

        public async Task<RespuestaGeneralDto> GuardarDatosProgramacionDistribucion(ProgramacionDistribucionDto programacionDistribucion, string usuarioDnp)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriGuardarDatosProgramacionDistribucion"];

            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlNegocio, uriMetodo, string.Empty, programacionDistribucion, usuarioDnp, useJWTAuth: false);
            return JsonConvert.DeserializeObject<RespuestaGeneralDto>(respuesta);
        }

        public async Task<RespuestaGeneralDto> GuardarDatosProgramacionFuentes(ProgramacionFuenteDto programacionFuente, string usuarioDnp)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriGuardarDatosProgramacionFuentes"];

            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlNegocio, uriMetodo, string.Empty, programacionFuente, usuarioDnp, useJWTAuth: false);
            return JsonConvert.DeserializeObject<RespuestaGeneralDto>(respuesta);
        }

        public async Task<RespuestaGeneralDto> GuardarDatosProgramacionIniciativa(ProgramacionIniciativaDto programacionIniciativa, string usuarioDnp)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriGuardarDatosProgramacionIniciativa"];

            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlNegocio, uriMetodo, string.Empty, programacionIniciativa, usuarioDnp, useJWTAuth: false);
            return JsonConvert.DeserializeObject<RespuestaGeneralDto>(respuesta);
        }

        public async Task<RespuestaGeneralDto> ValidarCalendarioProgramacion(int? entityTypeCatalogOptionId, Nullable<Guid> nivelId, Nullable<int> seccionCapituloId, string usuarioDnp)
        {
            RespuestaGeneralDto rta = new RespuestaGeneralDto();
            var uriMetodo = ConfigurationManager.AppSettings["uriProgramacionValidarCalendario"];

            uriMetodo += "?entityTypeCatalogOptionId=" + entityTypeCatalogOptionId;
            uriMetodo += "&nivelId=" + nivelId;
            uriMetodo += "&seccionCapituloId=" + seccionCapituloId;
            var result = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlNegocio, uriMetodo, string.Empty, null, usuarioDnp, useJWTAuth: false);
            rta.Exito = !string.IsNullOrEmpty(result) && result.Equals("true") ? true:false;
            return rta;
        }

        public async Task<string> ObtenerDatosProgramacionProducto(int TramiteId, string usuarioDnp)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriObtenerDatosProgramacionProducto"];
            uriMetodo += "?TramiteId=" + TramiteId;

            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlNegocio, uriMetodo, string.Empty, null, usuarioDnp, useJWTAuth: false);
            if (string.IsNullOrEmpty(respuesta)) return string.Empty;
            return respuesta;
        }

        public async Task<RespuestaGeneralDto> GuardarDatosProgramacionProducto(ProgramacionProductoDto programacionProducto, string usuarioDnp)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriGuardarDatosProgramacionProducto"];

            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlNegocio, uriMetodo, string.Empty, programacionProducto, usuarioDnp, useJWTAuth: false);
            return JsonConvert.DeserializeObject<RespuestaGeneralDto>(respuesta);
        }

        public async Task<string> ObtenerProgramacionBuscarProyecto(int EntidadDestinoId, int tramiteid, string bpin, string NombreProyecto, string usuarioDnp)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriObtenerProgramacionBuscarProyecto"];
            uriMetodo += "?EntidadDestinoId=" + EntidadDestinoId + "&tramiteid=" + tramiteid + "&bpin=" + bpin + "&NombreProyecto=" + NombreProyecto;

            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlNegocio, uriMetodo, string.Empty, null, usuarioDnp, useJWTAuth: false);
            if (string.IsNullOrEmpty(respuesta)) return string.Empty;
            return respuesta;
        }

        public async Task<RespuestaGeneralDto> BorrarTramiteProyecto(ProgramacionDistribucionDto programacionDistribucion, string usuarioDnp)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriBorrarTramiteProyecto"];

            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlNegocio, uriMetodo, string.Empty, programacionDistribucion, usuarioDnp, useJWTAuth: false);
            return JsonConvert.DeserializeObject<RespuestaGeneralDto>(respuesta);
        }

        public async Task<RespuestaGeneralDto> GuardarDatosInclusion(ProgramacionDistribucionDto programacionDistribucion, string usuarioDnp)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriGuardarDatosInclusion"];

            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlNegocio, uriMetodo, string.Empty, programacionDistribucion, usuarioDnp, useJWTAuth: false);
            return JsonConvert.DeserializeObject<RespuestaGeneralDto>(respuesta);
        }



        #region Programacion cargas masivas creditos

        public async Task<string> ObtenerCargaMasivaCreditos(string usuarioDnp)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriObtenerCargaMasivaCreditos"];

            return JsonConvert.DeserializeObject<string>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, ENDPOINTSERVICIONEGOCIO, uriMetodo, null, usuarioDnp, useJWTAuth: false));
        }

        public async Task<string> ValidarCargaMasivaCreditos(dynamic validarCargaMasivaCreditos, string usuarioDnp)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriValidarCargaMasivaCreditos"];

            return JsonConvert.DeserializeObject<string>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, ENDPOINTSERVICIONEGOCIO, uriMetodo, string.Empty, validarCargaMasivaCreditos, usuarioDnp, useJWTAuth: false));
        }

        public async Task<ReponseHttp> RegistrarCargaMasivaCreditos(dynamic registrarCargaMasivaCreditos, string usuarioDnp)
        {
            try
            {
                var uriMetodo = ConfigurationManager.AppSettings["uriRegistrarCargaMasivaCreditos"];

                var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, ENDPOINTSERVICIONEGOCIO, uriMetodo, string.Empty, registrarCargaMasivaCreditos, usuarioDnp, useJWTAuth: false);

                var json = JsonConvert.DeserializeObject<ReponseHttp>(respuesta);

                return json;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion Programacion cargas masivas creditos

        #region Programacion cargas masivas cuotas

        public async Task<string> ObtenerCargaMasivaCuotas(int? Vigencia, int? EntityTypeCatalogOptionId, string usuarioDnp)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriObtenerCargaMasivaCuotas"];

            uriMetodo += "?Vigencia=" + Vigencia + "&EntityTypeCatalogOptionId=" + EntityTypeCatalogOptionId;

            return JsonConvert.DeserializeObject<string>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, ENDPOINTSERVICIONEGOCIO, uriMetodo, string.Empty, null, usuarioDnp, useJWTAuth: false));
        }

        public async Task<string> ValidarCargaMasivaCuotas(dynamic validarCargaMasivaCuotas, string usuarioDnp)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriValidarCargaMasivaCuotas"];

            return JsonConvert.DeserializeObject<string>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, ENDPOINTSERVICIONEGOCIO, uriMetodo, string.Empty, validarCargaMasivaCuotas, usuarioDnp, useJWTAuth: false));
        }

        public async Task<ReponseHttp> RegistrarCargaMasivaCuotas(dynamic registrarCargaMasivaCuotas, string usuarioDnp)
        {
            try
            {
                var uriMetodo = ConfigurationManager.AppSettings["uriRegistrarCargaMasivaCuotas"];

                var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, ENDPOINTSERVICIONEGOCIO, uriMetodo, string.Empty, registrarCargaMasivaCuotas, usuarioDnp, useJWTAuth: false);

                var json = JsonConvert.DeserializeObject<ReponseHttp>(respuesta);

                return json;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion Programacion cargas masivas cuotas
        #region Programacion Preparacion Generar Presupuestal Administrar Calendario

        public async Task<string> ConsultarProyectoGenerarPresupuestal(int sectorId, int entidadId, string proyectoId, string usuarioDnp)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriProgramacionConsultarProyectoGenerarPresupuestal"];
            var response = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlNegocio, uriMetodo, $"?sectorId={sectorId}&entidadId={entidadId}&proyectoId={proyectoId}", null, usuarioDnp, useJWTAuth: false);
            return response;
        }
        public async Task<string> ObtenerProgramacionSectores(int sectorId, string usuarioDnp)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriProgramacionObtenerProgramacionSectores"];
            var response = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlNegocio, uriMetodo, $"?sectorId={sectorId}", null, usuarioDnp, useJWTAuth: false);
            return response;
        }
        public async Task<string> ObtenerProgramacionEntidadesSector(int sectorId, string usuarioDnp)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriProgramacionObtenerProgramacionEntidadesSector"];
            var response = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlNegocio, uriMetodo, $"?sectorId={sectorId}", null, usuarioDnp, useJWTAuth: false);
            return response;
        }
        public async Task<string> ObtenerCalendarioProgramacion(Guid FlujoId, string usuarioDnp)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriProgramacionObtenerCalendarioProgramacion"];
            var response = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlNegocio, uriMetodo, $"?FlujoId={FlujoId}", null, usuarioDnp, useJWTAuth: false);
            return response;
        }
        public async Task<TramitesResultado> RegistrarProyectosSinPresupuestal(List<ProyectoSinPresupuestalDto> proyectoSinPresupuestalDto, string usuarioDNP)
        {
            try
            {
                var uriMetodo = ConfigurationManager.AppSettings["uriRegistrarProyectosSinPresupuestal"];
                var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlNegocio, uriMetodo, null, proyectoSinPresupuestalDto, usuarioDNP, useJWTAuth: false);
                var json = JsonConvert.DeserializeObject<TramitesResultado>(respuesta);
                return json;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<RespuestaGeneralDto> RegistrarCalendarioProgramacion(List<CalendarioProgramacionDto> calendarioProgramacionDto, string usuarioDNP)
        {
            try
            {
                var uriMetodo = ConfigurationManager.AppSettings["uriRegistrarCalendarioProgramacion"];
                var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlNegocio, uriMetodo, null, calendarioProgramacionDto, usuarioDNP, useJWTAuth: false);
                return JsonConvert.DeserializeObject<RespuestaGeneralDto>(respuesta);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<string> ValidarConsecutivoPresupuestal(dynamic validarConsecutivoPresupuestal, string usuarioDnp)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriValidarConsecutivoPresupuestal"];

            return JsonConvert.DeserializeObject<string>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, ENDPOINTSERVICIONEGOCIO, uriMetodo, string.Empty, validarConsecutivoPresupuestal, usuarioDnp, useJWTAuth: false));
        }
        #endregion Programacion Preparacion Generar Presupuestal Administrar Calendario

        #region Programacion regionalizacion  Politicas transversales        
        public async Task<RespuestaGeneralDto> GuardarProgramacionRegionalizacion(ProgramacionRegionalizacionDto programacionRegionalizacionDto, string usuarioDnp)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriGuardarProgramacionRegionalizacion"];

            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlNegocio, uriMetodo, string.Empty, programacionRegionalizacionDto, usuarioDnp, useJWTAuth: false);
            return JsonConvert.DeserializeObject<RespuestaGeneralDto>(respuesta);
        }

        public async Task<string> ConsultarPoliticasTransversalesProgramacion(string Bpin, string usuarioDnp)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriConsultarPoliticasTransversalesProgramacion"];
            uriMetodo += "?Bpin=" + Bpin;

            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlNegocio, uriMetodo, string.Empty, null, usuarioDnp, useJWTAuth: false);
            if (string.IsNullOrEmpty(respuesta)) return string.Empty;
            return respuesta;
        }

        public async Task<RespuestaGeneralDto> AgregarPoliticasTransversalesProgramacion(IncluirPoliticasDto objIncluirPoliticasDto, string usuarioDnp)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriAgregarPoliticasTransversalesProgramacion"];

            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlNegocio, uriMetodo, string.Empty, objIncluirPoliticasDto, usuarioDnp, useJWTAuth: false);
            return JsonConvert.DeserializeObject<RespuestaGeneralDto>(respuesta);
        }

        public async Task<string> ConsultarPoliticasTransversalesCategoriasProgramacion(string Bpin, string usuarioDnp)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriConsultarPoliticasTransversalesCategoriasProgramacion"];
            uriMetodo += "?Bpin=" + Bpin;

            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlNegocio, uriMetodo, string.Empty, null, usuarioDnp, useJWTAuth: false);
            if (string.IsNullOrEmpty(respuesta)) return string.Empty;
            return respuesta;
        }

        public async Task<RespuestaGeneralDto> EliminarPoliticasProyectoProgramacion(int tramiteidProyectoId, int politicaId, string usuarioDnp)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriEliminarPoliticasProyectoProgramacion"];
            uriMetodo += "?tramiteidProyectoId=" + tramiteidProyectoId + "&politicaId=" + politicaId;

            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlNegocio, uriMetodo, string.Empty, null, usuarioDnp, useJWTAuth: false);
            return JsonConvert.DeserializeObject<RespuestaGeneralDto>(respuesta);
        }

        public async Task<RespuestaGeneralDto> AgregarCategoriasPoliticaTransversalesProgramacion(FocalizacionCategoriasDto objIncluirPoliticasDto, string usuarioDnp)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriAgregarCategoriasPoliticaTransversalesProgramacion"];

            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlNegocio, uriMetodo, string.Empty, objIncluirPoliticasDto, usuarioDnp, useJWTAuth: false);
            return JsonConvert.DeserializeObject<RespuestaGeneralDto>(respuesta);
        }

        public async Task<RespuestaGeneralDto> GuardarPoliticasTransversalesCategoriasProgramacion(PoliticasTransversalesCategoriasProgramacionDto objIncluirPoliticasDto, string usuarioDnp)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriGuardarPoliticasTransversalesCategoriasProgramacion"];

            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlNegocio, uriMetodo, string.Empty, objIncluirPoliticasDto, usuarioDnp, useJWTAuth: false);
            return JsonConvert.DeserializeObject<RespuestaGeneralDto>(respuesta);
        }

        public async Task<RespuestaGeneralDto> EliminarCategoriasProyectoProgramacion(EliminarCategoriasProyectoProgramacionDto objIncluirPoliticasDto, string usuarioDnp)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriEliminarCategoriasProyectoProgramacion"];

            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlNegocio, uriMetodo, string.Empty, objIncluirPoliticasDto, usuarioDnp, useJWTAuth: false);
            return JsonConvert.DeserializeObject<RespuestaGeneralDto>(respuesta);
        }

        public async Task<RespuestaGeneralDto> EliminarCategoriaPoliticasProyectoProgramacion(int proyectoId, int politicaId, int categoriaId, string usuarioDnp)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriEliminarCategoriaPoliticasProyectoProgramacion"];
            uriMetodo += "?proyectoId=" + proyectoId + "&politicaId=" + politicaId + "&categoriaId=" + categoriaId;

            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlNegocio, uriMetodo, string.Empty, null, usuarioDnp, useJWTAuth: false);
            return JsonConvert.DeserializeObject<RespuestaGeneralDto>(respuesta);
        }

        public async Task<string> ObtenerCrucePoliticasProgramacion(string Bpin, string usuarioDnp)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriObtenerCrucePoliticasProgramacion"];
            uriMetodo += "?Bpin=" + Bpin;

            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlNegocio, uriMetodo, string.Empty, null, usuarioDnp, useJWTAuth: false);
            if (string.IsNullOrEmpty(respuesta)) return string.Empty;
            return respuesta;
        }

        public async Task<string> PoliticasSolicitudConceptoProgramacion(string Bpin, string usuarioDnp)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriPoliticasSolicitudConceptoProgramacion"];
            uriMetodo += "?Bpin=" + Bpin;

            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlNegocio, uriMetodo, string.Empty, null, usuarioDnp, useJWTAuth: false);
            if (string.IsNullOrEmpty(respuesta)) return string.Empty;
            return respuesta;
        }

        public async Task<RespuestaGeneralDto> GuardarCrucePoliticasProgramacion(List<CrucePoliticasAjustesDto> parametrosGuardar, string usuarioDnp)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriGuardarCrucePoliticasProgramacion"];

            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlNegocio, uriMetodo, string.Empty, parametrosGuardar, usuarioDnp, useJWTAuth: false);
            return JsonConvert.DeserializeObject<RespuestaGeneralDto>(respuesta);
        }

        public async Task<RespuestaGeneralDto> SolicitarConceptoDTProgramacion(List<FocalizacionSolicitarConceptoDto> parametrosGuardar, string usuarioDnp)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriSolicitarConceptoDTProgramacion"];

            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlNegocio, uriMetodo, string.Empty, parametrosGuardar, usuarioDnp, useJWTAuth: false);
            return JsonConvert.DeserializeObject<RespuestaGeneralDto>(respuesta);
        }

        public async Task<string> ObtenerResumenSolicitudConceptoProgramacion(string Bpin, string usuarioDnp)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriObtenerResumenSolicitudConceptoProgramacion"];
            uriMetodo += "?Bpin=" + Bpin;

            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlNegocio, uriMetodo, string.Empty, null, usuarioDnp, useJWTAuth: false);
            if (string.IsNullOrEmpty(respuesta)) return string.Empty;
            return respuesta;
        }
        #endregion Programacion regionalizacion  Politicas transversales

        /// <summary>
        /// Obtención de datos de grupo de programación.
        /// </summary>
        /// <param name="instanciaTramiteDto">Contiene informacion de autorizacion, filtro e columnas visibles </param>
        /// <returns>consulta de datos de grupo de programación.</returns>
        public async Task<InboxTramite> ObtenerInboxProgramacionConsolaProcesos(InstanciaTramiteDto instanciaTramiteDto, string usuarioDNP)
        {
            var tipoEntidad = instanciaTramiteDto.TramiteFiltroDto?.FiltroGradeDtos?.FirstOrDefault(x => x.Campo.Equals("NombreTipoEntidad"));

            if (!string.IsNullOrEmpty(usuarioDNP))
            {
                var entidadesVisualizador = await _autorizacionServicios.ObtenerEntidadesConRoleVisualizador(usuarioDNP);
                var entidadesVisualizadorIds = entidadesVisualizador.Where(x => x.EntityTypeCatalogOptionId.HasValue).Select(x => x.EntityTypeCatalogOptionId.Value);

                instanciaTramiteDto.TramiteFiltroDto.EntidadesVisualizador = entidadesVisualizadorIds.ToList();
            }

            var tramites = await _flujoServicios.ObtenerProgramacionConsolaProcesos(instanciaTramiteDto);
            var inbox = new InboxTramite();
            if (tramites != null && tramites.Any())
            {
                inbox.ListaGrupoTramiteEntidad = await CrearGrupoTramitePorEntidad(tramites);
            }
            else
            {
                inbox.ListaGrupoTramiteEntidad = new List<GrupoTramiteEntidad>();
            }
            return inbox;
        }
        /// <summary>
        /// Obtención lista de grupo de tramites.
        /// </summary>
        /// <param name="tramites">lista de trámites</param>
        /// <returns>consulta de datos de lista de grupo de trámite.</returns>
        public async Task<List<GrupoTramiteEntidad>> CrearGrupoTramitePorEntidad(List<TramiteDto> tramites)
        {
            var grupoTramite = from t in tramites
                               group t by new { t.NombreTipoTramite, t.NombreEntidad, t.SectorId, t.NombreSector, t.NombreTipoEntidad, t.EntidadId, t.NombreFlujo }
                               into e
                               select new GrupoTramites()
                               {
                                   NombreTipoTramite = e.Key.NombreTipoTramite,
                                   NombreSector = e.Key.NombreSector,
                                   SectorId = e.Key.SectorId,
                                   NombreTipoEntidad = e.Key.NombreTipoEntidad,
                                   EntidadId = e.Key.EntidadId,
                                   NombreEntidad = e.Key.NombreEntidad,
                                   NombreFlujo = e.Key.NombreFlujo,
                                   ListaTramites = e.ToList()
                               };


            var groupEntidad = from g in grupoTramite
                               group g by new { g.EntidadId, g.NombreEntidad, g.SectorId, g.NombreSector }
                               into e
                               select new GrupoTramiteEntidad()
                               {
                                   IdSector = e.Key.SectorId.Value,
                                   Sector = e.Key.NombreSector,
                                   EntidadId = e.Key.EntidadId,
                                   NombreEntidad = e.Key.NombreEntidad,
                                   GrupoTramites = e.ToList()
                               };

            return await Task.Run(() => groupEntidad.ToList());
        }

        public async Task<string> ConsultarPoliticasTransversalesCategoriasModificaciones(string Bpin, string usuarioDnp)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriConsultarPoliticasTransversalesCategoriasModificaciones"];
            uriMetodo += "?Bpin=" + Bpin;

            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlNegocio, uriMetodo, string.Empty, null, usuarioDnp, useJWTAuth: false);
            if (string.IsNullOrEmpty(respuesta)) return string.Empty;
            return respuesta;
        }

        public async Task<RespuestaGeneralDto> GuardarPoliticasTransversalesCategoriasModificaciones(PoliticasTransversalesCategoriasProgramacionDto objIncluirPoliticasDto, string usuarioDnp)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriGuardarPoliticasTransversalesCategoriasModificaciones"];

            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlNegocio, uriMetodo, string.Empty, objIncluirPoliticasDto, usuarioDnp, useJWTAuth: false);
            return JsonConvert.DeserializeObject<RespuestaGeneralDto>(respuesta);
        }


        public async Task<string> ConsultarPoliticasTransversalesAprobacionesModificaciones(string Bpin, string usuarioDnp)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriConsultarPoliticasTransversalesAprobacionesModificaciones"];
            uriMetodo += "?Bpin=" + Bpin;

            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlNegocio, uriMetodo, string.Empty, null, usuarioDnp, useJWTAuth: false);
            if (string.IsNullOrEmpty(respuesta)) return string.Empty;
            return respuesta;
        }

        #region cargue masivo saldos

        public async Task<RespuestaGeneralDto> RegistrarCargaMasivaSaldos(int TipoCargueId, string usuarioDnp)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriRegistrarCargaMasivaSaldos"];
            return JsonConvert.DeserializeObject<RespuestaGeneralDto>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, ENDPOINTSERVICIONEGOCIO, uriMetodo, string.Empty, TipoCargueId, usuarioDnp, useJWTAuth: false));
        }
     
        public async Task<string> ObtenerLogErrorCargaMasivaSaldos(int? TipoCargueDetalleId, int? CarguesIntegracionId, string usuarioDnp)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriObtenerLogErrorCargaMasivaSaldos"];
            uriMetodo = uriMetodo + "?TipoCargueDetalleId=" + TipoCargueDetalleId + "&CarguesIntegracionId=" + CarguesIntegracionId;

            return JsonConvert.DeserializeObject<string>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, ENDPOINTSERVICIONEGOCIO, uriMetodo, string.Empty, null, usuarioDnp, useJWTAuth: false));
        }
        public async Task<string> ObtenerCargaMasivaSaldos(string TipoCargue, string usuarioDnp)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriObtenerCargaMasivaSaldos"];
            uriMetodo = uriMetodo + "?TipoCargue=" + TipoCargue;

            return JsonConvert.DeserializeObject<string>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, ENDPOINTSERVICIONEGOCIO, uriMetodo, string.Empty, null, usuarioDnp, useJWTAuth: false));
        }
        public async Task<string> ObtenerTipoCargaMasiva(string TipoCargue, string usuarioDnp)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriObtenerTipoCargaMasiva"];
            uriMetodo = uriMetodo + "?TipoCargue=" + TipoCargue;

            return JsonConvert.DeserializeObject<string>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, ENDPOINTSERVICIONEGOCIO, uriMetodo, string.Empty, null, usuarioDnp, useJWTAuth: false));
        }

        public async Task<RespuestaGeneralDto> ValidarCargaMasiva(dynamic jsonListaRegistros, string usuarioDnp)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriValidarCargaMasiva"];
            //uriMetodo = uriMetodo + "?jsonListaRegistros=" + jsonListaRegistros;

            return JsonConvert.DeserializeObject<RespuestaGeneralDto>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, ENDPOINTSERVICIONEGOCIO, uriMetodo, string.Empty, jsonListaRegistros, usuarioDnp, useJWTAuth: false));
        }

        public async Task<string> ObtenerDetalleCargaMasivaSaldos(int? CargueId, string usuarioDnp)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriObtenerDetalleCargaMasivaSaldos"];
            uriMetodo = uriMetodo + "?CargueId=" + CargueId;

            return JsonConvert.DeserializeObject<string>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, ENDPOINTSERVICIONEGOCIO, uriMetodo, string.Empty, null, usuarioDnp, useJWTAuth: false));
        }
        #endregion cargue masivo saldos

        public async Task<string> ConsultarCatalogoIndicadoresPolitica(string PoliticaId, string Criterio, string usuarioDnp)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriConsultarCatalogoIndicadoresPolitica"];
            uriMetodo += "?PoliticaId=" + PoliticaId + "&Criterio=" + Criterio;

            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlNegocio, uriMetodo, string.Empty, null, usuarioDnp, useJWTAuth: false);
            if (string.IsNullOrEmpty(respuesta)) return string.Empty;
            return respuesta;
        }
        public async Task<RespuestaGeneralDto> GuardarModificacionesAsociarIndicadorPolitica(int proyectoId, int politicaId, int categoriaId, int indicadorId, string accion, string usuarioDnp)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriGuardarModificacionesAsociarIndicadorPolitica"];
            uriMetodo += "?proyectoId=" + proyectoId + "&politicaId=" + politicaId + "&categoriaId=" + categoriaId + "&indicadorId=" + indicadorId + "&accion=" + accion;

            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlNegocio, uriMetodo, string.Empty, null, usuarioDnp, useJWTAuth: false);
            return JsonConvert.DeserializeObject<RespuestaGeneralDto>(respuesta);
        }

    }
}
