namespace DNP.Backbone.Web.API.Test.Mocks
{
    using DNP.Backbone.Comunes.Dto;
    using DNP.Backbone.Comunes.Properties;
    using DNP.Backbone.Dominio.Dto;
    using DNP.Backbone.Dominio.Dto.Conpes;
    using DNP.Backbone.Dominio.Dto.FuenteFinanciacion;
    using DNP.Backbone.Dominio.Dto.Orfeo;
    using DNP.Backbone.Dominio.Dto.Productos;
    using DNP.Backbone.Dominio.Dto.Proyecto;
    using DNP.Backbone.Dominio.Dto.Tramites;
    using DNP.Backbone.Dominio.Dto.Tramites.Proyectos;
    using DNP.Backbone.Dominio.Dto.Tramites.Reprogramacion;
    using DNP.Backbone.Dominio.Dto.Tramites.VigenciaFutura;
    using DNP.Backbone.Dominio.Dto.Transferencias;
    using DNP.Backbone.Dominio.Dto.Transversal;
    using DNP.Backbone.Dominio.Dto.VigenciasFuturas;
    using DNP.Backbone.Servicios.Interfaces.Tramites;
    using DNP.ServiciosNegocio.Dominio.Dto.Tramites;
    using Dominio.Dto.Inbox;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class TramiteServiciosMock : ITramiteServicios
    {
        private readonly ITramiteServicios _tramiteServicios;
        public Task<Dominio.Dto.InstanciaResultado> EliminarProyectoTramite(InstanciaTramiteDto instanciaTramiteDto)
        {
            return Task.FromResult(new Dominio.Dto.InstanciaResultado());
        }

        public Task<InboxTramite> ObtenerInboxTramites(InstanciaTramiteDto instanciaTramiteDto)
        {
            return Task.FromResult(new InboxTramite() { Mensaje = Resources.UsuarioNoTieneTareasPendientes });
        }

        public Task<InboxTramite> ObtenerInboxTramitesConsolaProcesos(InstanciaTramiteDto instanciaTramiteDto, string usuarioDNP)
        {
            throw new System.NotImplementedException();
        }

        public Task<InboxDto> ObtenerInfoPDF(InstanciaInboxDto datosConsulta, string token)
        {
            throw new System.NotImplementedException();
        }

        public Task<ProyectosTramitesDTO> ObtenerProyectosTramite(InstanciaTramiteDto instanciaTramiteDto)
        {
            return Task.FromResult(new ProyectosTramitesDTO());
        }

        public Task<List<TramiteDto>> ObtenerTramites(InstanciaTramiteDto instanciaTramiteDto)
        {
            return Task.FromResult(new List<TramiteDto>());
        }

        Task<InstanciaResultado> ITramiteServicios.EliminarProyectoTramite(InstanciaTramiteDto instanciaTramiteDto)
        {
            return Task.FromResult(new InstanciaResultado());
        }
        Task<RespuestaGeneralDto> ITramiteServicios.GuardarProyectosTramiteNegocio(DatosTramiteProyectosDto parametros, string usuarioDnp)
        {
            throw new System.NotImplementedException();
        }

        Task<InboxTramite> ITramiteServicios.ObtenerInboxTramites(InstanciaTramiteDto instanciaTramiteDto)
        {
            return Task.FromResult(new InboxTramite());
        }

        Task<InboxTramite> ITramiteServicios.ObtenerInboxTramitesConsolaProcesos(InstanciaTramiteDto instanciaTramiteDto, string usuarioDNP)
        {
            throw new System.NotImplementedException();
        }

        Task<ProyectosTramitesDTO> ITramiteServicios.ObtenerProyectosTramite(InstanciaTramiteDto instanciaTramiteDto)
        {
            return Task.FromResult(new ProyectosTramitesDTO());
        }

        Task<List<ProyectosEnTramiteDto>> ITramiteServicios.ObtenerProyectosTramiteNegocio(int TramiteId, string usuarioDnpo, string TokenAutorizacion)
        {
            throw new System.NotImplementedException();
        }

        Task<List<TramiteDto>> ITramiteServicios.ObtenerTramites(InstanciaTramiteDto instanciaTramiteDto)
        {
            throw new System.NotImplementedException();
        }

        public Task<RespuestaGeneralDto> ActualizarInstanciaProyecto(ProyectosTramiteDto parametros, string usuarioDnp)
        {
            throw new System.NotImplementedException();
        }

        public Task<List<JustificacionTramiteProyectoDto>> ObtenerPreguntasJustificacion(int TramiteId, int ProyectoId, string usuarioDnp, string TokenAutorizacion)
        {
            throw new System.NotImplementedException();

        }

        public Task<RespuestaGeneralDto> GuardarRespuestasJustificacion(List<JustificacionTramiteProyectoDto> parametros, string usuarioDnp)
        {
            throw new System.NotImplementedException();
        }


        public Task<List<FuentePresupuestalDto>> ObtenerFuentesInformacionPresupuestal(string usuarioDnp)
        {
            throw new System.NotImplementedException();
        }

        public Task<List<ProyectoFuentePresupuestalDto>> ObtenerProyectoFuentePresupuestalPorTramite(int pTramiteProyectoId, int? pProyectoFuentePresupuestalId, string pTipoProyecto, string usuarioDnp)
        {
            throw new System.NotImplementedException();
        }

        public Task<List<Dominio.Dto.Tramites.Proyectos.ProyectoRequisitoDto>> ObtenerProyectoRequisitosPorTramite(int pTramiteProyectoId, int? pProyectoRequisitoId, string usuarioDnp, bool isCDP)
        {
            throw new System.NotImplementedException();
        }

        public Task<RespuestaGeneralDto> EliminarProyectoTramiteNegocio(InstanciaTramiteDto instanciaTramiteDto)
        {
            throw new System.NotImplementedException();
        }

        public Task<List<JustificacionTramiteProyectoDto>> ObtenerPreguntasJustificacion(int TramiteId, int ProyectoId, int TipoTramiteId, int TipoRolId, string usuarioDnp, string TokenAutorizacion)
        {
            throw new System.NotImplementedException();
        }

        public Task<RespuestaGeneralDto> ActualizarValoresProyecto(ProyectosTramiteDto parametros, string usuarioDnp)
        {
            throw new System.NotImplementedException();
        }

        public Task<RespuestaGeneralDto> GuardarTramiteTipoRequisito(List<TramiteRequitoDto> parametros, string usuarioDnp)
        {
            throw new System.NotImplementedException();
        }

        public Task<RespuestaGeneralDto> GuardarTramiteInformacionPresupuestal(List<TramiteFuentePresupuestalDto> parametros, string usuarioDnp)
        {
            throw new System.NotImplementedException();
        }

        public Task<List<TipoRequisitoDto>> ObtenerTiposRequisito(string usuarioDnp)
        {
            throw new System.NotImplementedException();
        }

        public Task<RespuestaGeneralDto> ValidarEnviarDatosTramiteNegocio(DatosTramiteProyectosDto parametros, string usuarioDnp)
        {
            throw new System.NotImplementedException();
        }

        public Task<List<JustificacionTramiteProyectoDto>> ObtenerPreguntasProyectoActualizacion(int TramiteId, int ProyectoId, int TipoTramiteId, int TipoRolId, string usuarioDnp, string TokenAutorizacion)
        {
            throw new System.NotImplementedException();
        }

        public Task<List<ProyectosEnTramiteDto>> ObtenerProyectosTramiteNegocioAprobacion(int TramiteId, int TipoRolId, string usuarioDnpo, string TokenAutorizacion)
        {
            throw new System.NotImplementedException();
        }

        public Task<List<FuentesTramiteProyectoAprobacionDto>> ObtenerFuentesTramiteProyectoAprobacion(int tramiteId, int proyectoId, string pTipoProyecto, string usuarioDnpo, string TokenAutorizacion)
        {
            throw new System.NotImplementedException();
        }

        public Task<RespuestaGeneralDto> GuardarFuentesTramiteProyectoAprobacion(List<FuentesTramiteProyectoAprobacionDto> parametros, string usuarioDnp)
        {
            throw new System.NotImplementedException();
        }

        public Task<List<TipoDocumentoTramiteDto>> ObtenerTipoDocumentoTramite(int TipoTramiteId, string Rol, int tramiteId, string usuarioDnpo, string TokenAutorizacion, string nivelId)
        {
            throw new System.NotImplementedException();
        }

        public Task<CodigoPresupuestalDto> ObtenerCodigoPresupuestal(int proyectoId, int entidadId, int tramiteId, string usuarioDnp)
        {
            throw new System.NotImplementedException();
        }

        public Task<RespuestaGeneralDto> ActualizarCodigoPresupuestal(int proyectoId, int entidadId, int tramiteId, string usuario)
        {
            throw new System.NotImplementedException();
        }

        public Task<List<TramitesProyectosDto>> ObtenerTarmitesPorProyectoEntidad(int proyectoId, int entidadId, string usuario)
        {
            throw new System.NotImplementedException();
        }


        public Task<TramiteValoresProyectoDto> ObtenerValoresProyectos(int proyectoId, int tramiteId, int entidadId, string usuarioDnp)
        {
            throw new System.NotImplementedException();
        }

        public Task<List<JustificacionTramiteProyectoDto>> ObtenerPreguntasJustificacion(int TramiteId, int ProyectoId, int TipoTramiteId, int TipoRolId, string usuarioDnp, string IdNivel, string TokenAutorizacion)
        {
            //throw new System.NotImplementedException();

            var JustificacionTramiteProyectoDtoMock = new List<JustificacionTramiteProyectoDto>();
            JustificacionTramiteProyectoDtoMock.Add(new JustificacionTramiteProyectoDto
            {
                NivelId = null,
                InstanciaId = null,
                TramiteId = 298,
                ProyectoId = 97652,
                JustificacionId = null,
                JustificacionPreguntaId = 3709,
                OrdenJustificacionPregunta = null,
                JustificacionPregunta = "¿Cómo está relacionado el traslado con los cambios en la(s) política(s) pública(s) del sector?",
                JustificacionRespuesta = "Respuesta 1  justificación tramite 298",
                ObservacionPregunta = null,
                ObservacionRespuesta = null,
                Tematica = "Tramite",
                OrdenTematica = 1,
                NombreRol = "Tramite",
                NombreNivel = null,
                CuestionarioId = 375,

            });
            return Task.FromResult(JustificacionTramiteProyectoDtoMock.ToList());
        }

        public Task<List<ConceptoDireccionTecnicaTramiteDto>> ObtenerConceptoDireccionTecnicaTramite(int tramiteId, Guid nivelid, string usuario)
        {
            throw new NotImplementedException();
        }

        public Task<RespuestaGeneralDto> GuardarConceptoDireccionTecnicaTramite(List<ConceptoDireccionTecnicaTramiteDto> parametros, string usuario)
        {
            throw new NotImplementedException();
        }

        public Task<List<ProyectoConpesDto>> ObtenerProyectoConpes(string conpes, string idUsuario)
        {
            throw new NotImplementedException();
        }

        public Task<RespuestaGeneralDto> ValidarEnviarDatosTramiteNegocioAprobacion(DatosTramiteProyectosDto parametros, string usuarioDnp)
        {
            throw new NotImplementedException();
        }

        public Task<PlantillaCarta> ObtenerPlantillaCarta(string nombreSeccion, int tipoTramite, string usuarioDnp)
        {
            throw new NotImplementedException();
        }

        public Task<List<Carta>> ObtenerDatosCartaPorSeccion(int tramiteId, int plantillaSeccionId, string usuarioDnp)
        {
            throw new NotImplementedException();
        }

        //Alejandro
        async Task<string> ITramiteServicios.ObtenerCartaConceptoDatosDespedida(int tramiteId, int plantillaCartaSeccionId, string IdUsuario, string tokenAutorizacion)
        {
            try
            {
                string resultado = string.Empty;
                var result = string.Empty;//consulta de base de datos


                if (string.IsNullOrEmpty(result))
                {
                    resultado = "OK";
                    return resultado;
                }
                else
                {
                    var mensajeError = Convert.ToString(result);
                    resultado = mensajeError;
                    throw new Exception(mensajeError);
                }

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        Task<RespuestaGeneralDto> ITramiteServicios.ActualizarCartaConceptoDatosDespedida(DatosConceptoDespedidaDto parametros, string usuarioDnp)

        {
            throw new NotImplementedException();
        }

        public Task<UsuarioTramite> VerificaUsuarioDestinatario(UsuarioTramite parametros, string usuarioDnp)
        {
            throw new NotImplementedException();
        }

        public Task<RespuestaGeneralDto> ActualizarCartaDatosIniciales(Carta parametros, string usuarioDnp)
        {
            throw new NotImplementedException();
        }

        public Task<RespuestaGeneralDto> ActualizarCartaDatosDespedida(Carta parametros, string usuarioDnp)
        {
            throw new NotImplementedException();
        }

        public Task<List<UsuarioTramite>> ObtenerUsuariosRegistrados(int tramiteId, string numeroTramite, string usuarioDnp)
        {
            throw new NotImplementedException();
        }

        public Task<List<JustificacionTematicaDto>> ObtenerPreguntasProyectoActualizacion(int TramiteId, int ProyectoId, int TipoTramiteId, Guid IdNivel, int TipoRolId, string usuarioDnp, string TokenAutorizacion)
        {
            throw new NotImplementedException();
        }

        RespuestaDocumentoCONPES ITramiteServicios.ObtenerProyectoConpes(string conpes, string idUsuario)
        {
            throw new NotImplementedException();
        }

        public Task<CapituloConpes> CargarProyectoConpes(string proyectoid, Guid InstanciaId, string GuiMacroproceso, string idUsuario, string NivelId, string FlujoId)
        {
            throw new NotImplementedException();
        }

        public Task<RespuestaGeneralDto> AdicionarProyectoConpes(CapituloConpes conpes, string idUsuario)
        {
            throw new NotImplementedException();
        }

        public Task<List<DocumentoCONPES>> EliminarProyectoConpes(string proyectoid, string conpesid, string idUsuario)
        {
            throw new NotImplementedException();
        }

        public Task<RespuestaGeneralDto> CargarFirma(string firma, string rolId, string idUsuario)
        {
            throw new NotImplementedException();
        }

        public Task<RespuestaGeneralDto> ValidarSiExisteFirmaUsuario(string idUsuario)
        {
            throw new NotImplementedException();
        }

        public Task<RespuestaGeneralDto> Firmar(int tramiteId, string radicadoSalida, string idUsuario)
        {
            throw new NotImplementedException();
        }

        public Task<List<CuerpoConceptoCDP>> ObtenerCuerpoConceptoCDP(int tramiteId, string usuarioDnp)
        {
            throw new NotImplementedException();
        }

        public Task<List<CuerpoConceptoAutorizacion>> ObtenerCuerpoConceptoAutorizacion(int tramiteId, string usuarioDnp)
        {
            throw new NotImplementedException();
        }

        public Task<Carta> ConsultarCarta(int tramiteId, string usuarioDnp)
        {
            throw new NotImplementedException();
        }

        public Task<string> ReasignarRadicadoORFEO(ReasignacionRadicadoDto parametros, string usuario)
        {
            throw new NotImplementedException();
        }

        public Task<bool> CargarDocumentoElectronicoOrfeo(DatosDocumentoElectronicoDSDto datosDocumentoElectronicoDSDto, string usuarioDnp)
        {
            throw new NotImplementedException();
        }
        public Task<RespuestaGeneralDto> ActualizarEstadoAjusteProyecto(string tipoDevolucion, string objetoNegocioId, int tramiteId, string observacion, string usuarioDnp)
        {
            throw new NotImplementedException();
        }

        Task<ResponseDto<bool>> ITramiteServicios.CargarDocumentoElectronicoOrfeo(DatosDocumentoElectronicoDSDto datosDocumentoElectronicoDSDto, string usuarioDnp)
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseDto<bool>> ConsultarRadicado(string radicado, string usuarioDnp)
        {
            ResponseDto<bool> respuesta = new ResponseDto<bool>();
            await Task.Run(() =>
            {
                respuesta.Estado = true;
                respuesta.Mensaje = "Exitoso";
            });


            return respuesta;
        }

        public Task<ResponseDto<bool>> CerrarRadicado(CerrarRadicadoDto radicado, string usuarioDnp)
        {
            throw new NotImplementedException();
        }

        public Task<int> TramiteEnPasoUno(Guid InstanciaId, string usuarioDnp)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDto<List<TramiteConpesDetailDto>>> ObtenerTramiteConpes(int tramiteId, string usuarioDnp)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDto<bool>> AsociarTramiteConpes(AsociarConpesTramiteRequestDto model, string usuarioDnp)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDto<bool>> RemoverAsociacionConpes(RemoverAsociacionConpesTramiteDto model, string usuarioDnp)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDto<DetalleTramiteDto>> ObtenerDetalleTramitePorInstancia(string instanciaId, string usuarioDnp)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDto<bool>> ValidarConpesTramiteVigenciaFutura(string tramiteId, string usuarioDnp)
        {
            throw new NotImplementedException();
        }

        public Task<string> EliminarAsociacionVFO(EliminacionAsociacionDto eliminacionAsociacionDto, string usuario)
        {
            throw new NotImplementedException();
        }

        public Task<List<TramiteProyectoVFODto>> ObtenerProyectoAsociacionVFO(string bpin, int tramite, string tipoTramite, string usuario)
        {
            throw new NotImplementedException();
        }

        public Task<string> AsociarProyectoVFO(TramiteProyectoVFODto tramiteProyectoVFODto, string usuarioDnp)
        {
            throw new NotImplementedException();
        }

        public Task<DatosProyectoTramiteDto> ObtenerDatosProyectoTramite(int tramiteId, string usuarioDnp)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDto<bool>> CrearRadicadoEntradaTramite(int tramiteId, string usuarioDnp)
        {
            throw new NotImplementedException();
        }

        public Task<List<DatosProyectoTramiteDto>> ObtenerDatosProyectosPorTramite(int tramiteId, string usuarioDnp)
        {
            throw new NotImplementedException();
        }
        #region Vigencias Futuras

        public Task<string> ObtenerDatosCronograma(Guid instanciaId, string usuarioDnp)
        {

            var instanciaParam = new Guid("3E0750D4-DC36-4546-942F-72F8638B3E0A");

            if (instanciaId == instanciaParam)
            {
                string JSONresult = "{\"InstanciaId\":\"3E0750D4-DC36-4546-942F-72F8638B3E0A\",\"Actividades\":[{\"ActividadesCronogramaId\":null,\"TramiteProyectoId\":null,\"NombreActividad\":null,\"ActividadesPreContractualesId\":null,\"FechaInicial\":null,\"FechaFinal\":null,\"FechaInicialmy\":null,\"FechaFinalmy\":null}]}";
                return Task.FromResult(JSONresult);
            }

            return Task.FromResult(string.Empty);
        }

        public Task<List<JustificacionPasoDto>> ObtenerPreguntasProyectoActualizacionPaso(int TramiteId, int ProyectoId, int TipoTramiteId, Guid IdNivel, int TipoRolId, string usuarioDnp)
        {
            if (TramiteId == 453)
            {
                return Task.FromResult(new List<JustificacionPasoDto>() {
                    new JustificacionPasoDto()
                        {

                            Paso = "Paso 1",
                            NombreUsuario = "Usuario ejemplo",
                            Cuenta = "prueba@dnp.gov.co",
                            FechaEnvio = Convert.ToDateTime("06/04/2022"),
                            justificaciones = new List<JustificacionTramiteProyectoDto> {
                                new JustificacionTramiteProyectoDto()
                                { TramiteId = 12,
                                    ProyectoId = 333,
                                    JustificacionId = 0,
                                    JustificacionPreguntaId = 0,
                                    OrdenJustificacionPregunta = 0,
                                    JustificacionPregunta = "Ejemplo 1",
                                    JustificacionRespuesta = "respuesta 1",
                                    ObservacionPregunta = "",
                                    ObservacionRespuesta = "",
                                    Tematica = "",
                                    OrdenTematica = 0,
                                    NombreRol = "",
                                    NombreNivel = "",
                                    CuestionarioId = 0,
                                    Usuario = "Ejemplo",
                                    FechaEnvio = Convert.ToDateTime("05/04/2022"),
                                    Paso = "Paso 1",
                                    NombreUsuario = "Usuario ejemplo",
                                    Cuenta = "prueba@dnp.gov.co"
                                },
                                new JustificacionTramiteProyectoDto()
                                {
                                    TramiteId = 12,
                                    ProyectoId = 333,
                                    JustificacionId = 0,
                                    JustificacionPreguntaId = 0,
                                    OrdenJustificacionPregunta = 0,
                                    JustificacionPregunta = "Ejemplo 2",
                                    JustificacionRespuesta = "respuesta 2",
                                    ObservacionPregunta = "",
                                    ObservacionRespuesta = "",
                                    Tematica = "",
                                    OrdenTematica = 0,
                                    NombreRol = "",
                                    NombreNivel = "",
                                    CuestionarioId = 0,
                                    Usuario = "Ejemplo 2",
                                    FechaEnvio = Convert.ToDateTime("06/04/2022"),
                                    Paso = "Paso 1",
                                    NombreUsuario = "Usuario ejemplo2",
                                    Cuenta = "prueba@dnp.gov.co"
                                }

                            }
                        }
                    });
            }


            return Task.FromResult(new List<JustificacionPasoDto>());
        }

        public Task<InformacionPresupuestalValoresDto> ObtenerInformacionPresupuestalValores(int tramiteId, string usuarioDnp)
        {
            if (tramiteId == 26)
            {
                return Task.FromResult(new InformacionPresupuestalValoresDto()
                {
                    ProyectoId = 97746,
                    BPIN = "202100000000044",
                    AplicaConstante = false,
                    AñoBase = 2020,
                    ResumensolicitadoFuentesVigenciaFutura = new List<InformacionPresupuestalResumenSolicitado> {
                        new InformacionPresupuestalResumenSolicitado()
                        {
                            Vigencia = 2022,
                            Deflactor= 0,
                            ValorFuentesNacion = 51000,
                            ValorFuentesPropios = 0,
                            ValorAprobadoNacion = 0,
                            ValorAprobadoPropios = 0
                        }
                    },
                    DetalleProductosVigenciaFutura = new List<InformacionDetalleProductos> {
                        new InformacionDetalleProductos()
                        {
                            ObjetivoEspecificoId= 772,
                            ObjetivoEspecifico= "Obtener información técnica eficiente para dar viabilidad a nuevos proyectos de construcción y ampliación de cupos",
                            Productos = new List<InformacionProducto>{
                                new InformacionProducto()
                                {
                                    ProductoId = 1328,
                                    NombreProducto = "Servicio de información penitenciaria y carcelaria para la toma de decisiones - ",
                                    TotalValores = 172583765,
                                    Vigencias = new List<InformacionVigencia>{
                                        new InformacionVigencia()
                                        {
                                            //Deflactor= decimal.Parse("1.0161"),
                                            Deflactor=0,
                                            PeriodoProyectoId= 1344,
                                            Vigencia= 2022,
                                            ValorSolicitadoVF= 73818000
                                        }
                                    }
                                }
                           }

                        }
                    }
                });
            }

            return Task.FromResult(new InformacionPresupuestalValoresDto());
        }

        public Task<string> GuardarInformacionPresupuestalValores(InformacionPresupuestalValoresDto informacionPresupuestalValoresDto, string usuario)
        {
            try
            {
                string resultado = string.Empty;
                var result = string.Empty;//consulta de base de datos


                if (string.IsNullOrEmpty(result))
                {
                    resultado = "OK";
                    return Task.FromResult(resultado);
                }
                else
                {
                    var mensajeError = Convert.ToString(result);
                    resultado = mensajeError;
                    throw new Exception(mensajeError);
                }

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #endregion Vigencias Futuras 

        public async Task<List<EnvioSubDireccionDto>> ObtenerSolicitarConceptoPorTramite(int tramiteId, string usuarioDnp)
        {

            var result = await Task.Run(() => _tramiteServicios.ObtenerSolicitarConceptoPorTramite(tramiteId, usuarioDnp));
            return result;

        }

        public async Task<CrearRadicadoResponseDto> CrearRadicadoSalida(RadicadoSalidaRequestDto radicado, string usuarioDnp)
        {
            CrearRadicadoResponseDto respuesta = new CrearRadicadoResponseDto();
            await Task.Run(() =>
            {
                respuesta.ExpedienteId = "123456789";
                respuesta.RadicadoId = "202206630177221";
            });


            return respuesta;
        }

        public async Task<List<TramiteDeflactoresDto>> ObtenerDeflactores(string usuarioDnp)
        {
            try
            {
                var deflactores = new List<TramiteDeflactoresDto>();
                string jsonString = "[{'Id':1313,'AnioBase':2022,'AnioConstante':1990,'Valor':1.496671498407543e+001,'IPC':'2.8'},{'Id':1314,'AnioBase':2022,'AnioConstante':1991,'Valor':1.180154154240296e+001,'IPC':'2.8'},{'Id':1315,'AnioBase':2022,'AnioConstante':1992,'Valor':9.431424552387886e+000,'IPC':'2.8'},{'Id':1316,'AnioBase':2022,'AnioConstante':1993,'Valor':7.692842212388163e+000,'IPC':'2.8'},{'Id':1317,'AnioBase':2022,'AnioConstante':1994,'Valor':6.275260798097853e+000,'IPC':'2.8'},{'Id':1318,'AnioBase':2022,'AnioConstante':1995,'Valor':5.253022600115400e+000,'IPC':'2.8'},{'Id':1319,'AnioBase':2022,'AnioConstante':1996,'Valor':4.318854394569924e+000,'IPC':'2.8'},{'Id':1320,'AnioBase':2022,'AnioConstante':1997,'Valor':3.669858370631433e+000,'IPC':'2.8'},{'Id':1321,'AnioBase':2022,'AnioConstante':1998,'Valor':3.144606010417689e+000,'IPC':'2.8'},{'Id':1322,'AnioBase':2022,'AnioConstante':1999,'Valor':2.878841336845408e+000,'IPC':'2.8'},{'Id':1323,'AnioBase':2022,'AnioConstante':2000,'Valor':2.647204385879913e+000,'IPC':'2.8'},{'Id':1324,'AnioBase':2022,'AnioConstante':2001,'Valor':2.459123169873537e+000,'IPC':'2.8'},{'Id':1325,'AnioBase':2022,'AnioConstante':2002,'Valor':2.298421225885805e+000,'IPC':'2.8'},{'Id':1326,'AnioBase':2022,'AnioConstante':2003,'Valor':2.158317057023739e+000,'IPC':'2.8'},{'Id':1327,'AnioBase':2022,'AnioConstante':2004,'Valor':2.045856667377642e+000,'IPC':'2.8'},{'Id':1328,'AnioBase':2022,'AnioConstante':2005,'Valor':1.951126259969317e+000,'IPC':'2.8'},{'Id':1329,'AnioBase':2022,'AnioConstante':2006,'Valor':1.867500549812710e+000,'IPC':'2.8'},{'Id':1330,'AnioBase':2022,'AnioConstante':2007,'Valor':1.766885916380141e+000,'IPC':'2.8'},{'Id':1331,'AnioBase':2022,'AnioConstante':2008,'Valor':1.640953082626019e+000,'IPC':'2.8'},{'Id':1332,'AnioBase':2022,'AnioConstante':2009,'Valor':1.608739930677639e+000,'IPC':'2.8'},{'Id':1333,'AnioBase':2022,'AnioConstante':2010,'Valor':1.559300143495842e+000,'IPC':'2.8'},{'Id':1334,'AnioBase':2022,'AnioConstante':2011,'Valor':1.503254930950915e+000,'IPC':'2.8'},{'Id':1335,'AnioBase':2022,'AnioConstante':2012,'Valor':1.467449171174263e+000,'IPC':'2.8'},{'Id':1336,'AnioBase':2022,'AnioConstante':2013,'Valor':1.439522435917464e+000,'IPC':'2.8'},{'Id':1337,'AnioBase':2022,'AnioConstante':2014,'Valor':1.388696156586402e+000,'IPC':'2.8'},{'Id':1338,'AnioBase':2022,'AnioConstante':2015,'Valor':1.300642649233307e+000,'IPC':'2.8'},{'Id':1339,'AnioBase':2022,'AnioConstante':2016,'Valor':1.229922126934569e+000,'IPC':'2.8'},{'Id':1340,'AnioBase':2022,'AnioConstante':2017,'Valor':1.181594895700422e+000,'IPC':'2.8'},{'Id':1341,'AnioBase':2022,'AnioConstante':2018,'Valor':1.145178228048480e+000,'IPC':'2.8'},{'Id':1342,'AnioBase':2022,'AnioConstante':2019,'Valor':1.103254554960000e+000,'IPC':'2.8'},{'Id':1343,'AnioBase':2022,'AnioConstante':2020,'Valor':1.085773600000000e+000,'IPC':'2.8'},{'Id':1344,'AnioBase':2022,'AnioConstante':2021,'Valor':1.028000000000000e+000,'IPC':'2.8'},{'Id':1345,'AnioBase':2022,'AnioConstante':2022,'Valor':1.000000000000000e+000,'IPC':'2.8'},{'Id':1346,'AnioBase':2022,'AnioConstante':2023,'Valor':9.708737864077670e-001,'IPC':'2.8'},{'Id':1347,'AnioBase':2022,'AnioConstante':2024,'Valor':9.425959091337544e-001,'IPC':'2.8'},{'Id':1348,'AnioBase':2022,'AnioConstante':2025,'Valor':9.151416593531595e-001,'IPC':'2.8'},{'Id':1349,'AnioBase':2022,'AnioConstante':2026,'Valor':8.884870479156888e-001,'IPC':'2.8'},{'Id':1350,'AnioBase':2022,'AnioConstante':2027,'Valor':8.626087843841639e-001,'IPC':'2.8'},{'Id':1351,'AnioBase':2022,'AnioConstante':2028,'Valor':8.374842566836542e-001,'IPC':'2.8'},{'Id':1352,'AnioBase':2022,'AnioConstante':2029,'Valor':8.130915113433536e-001,'IPC':'2.8'},{'Id':1353,'AnioBase':2022,'AnioConstante':2030,'Valor':7.894092343139355e-001,'IPC':'2.8'}]";

                deflactores = JsonConvert.DeserializeObject<List<TramiteDeflactoresDto>>(jsonString);

                return deflactores;

            }
            catch (Exception e)
            {
                throw;
            }
        }

        public async Task<List<TramiteProyectoDto>> ObtenerProyectoTramite(int ProyectoId, int TramiteId, string usuario)
        {
            try
            {
                var proyectoTramite = new List<TramiteProyectoDto>();
                string jsonString = "[{'Id':739,'TramiteId':356,'ProyectoId':97706,'EntidadId':186,'PeriodoProyectoId':1315,'Accion':'D','Estado':true,'TipoProyecto':'Credito','NombreProyecto':'Construcción Ampliación de Infraestructura para Generación de Cupos en Los Establecimientos de Reclusión del Orden - Nacional Programa: 1206\n\rSubprograma: 0802','EsConstante':true,'Constante':1,'AnioBase':2015}]";

                proyectoTramite = JsonConvert.DeserializeObject<List<TramiteProyectoDto>>(jsonString);

                return proyectoTramite;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public async Task<string> ActualizaVigenciaFuturaProyectoTramite(TramiteProyectoDto tramiteProyectoDto, string usuario)
        {
            try
            {
                string resultado = string.Empty;
                var result = string.Empty;//consulta de base de datos


                if (string.IsNullOrEmpty(result))
                {
                    resultado = "OK";
                    return resultado;
                }
                else
                {
                    var mensajeError = Convert.ToString(result);
                    resultado = mensajeError;
                    throw new Exception(mensajeError);
                }

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<VigenciaFuturaCorrienteDto> ObtenerFuentesFinanciacionVigenciaFuturaCorriente(string bpin, string usuarioDnp)
        {
            try
            {
                var proyectoTramite = new VigenciaFuturaCorrienteDto();
                string jsonString = "{'ProyectoId':97706,'BPIN':'202100000000037','AñoInicio':2022,'AñoFin':2024,'ValorTotalVigenteFutura':251755071156.0000,'ValorTotalVigente':256290880000.0000,'Porcentaje':37763260673.400000,'Fuentes':[{'EtapaId':2,'Etapa':'Inversión','FuenteId':1214,'Fuente':'MINISTERIO DE JUSTICIA Y DEL DERECHO - GESTIÓN GENERAL','TipoEntidad':'Entidades Presupuesto Nacional - PGN','EntidadId':1,'Entidad':'PGN','TipoRecursoId':1,'TipoRecurso':'Nación','ApropiacionVigente':50000000000.0000,'Vigencias':[{'PeriodoProyectoId':0,'Vigencia':2021,'ValorVigenteFutura':0.0000},{'PeriodoProyectoId':0,'Vigencia':2022,'ValorVigenteFutura':0.0000},{'PeriodoProyectoId':0,'Vigencia':2023,'ValorVigenteFutura':0.0000},{'PeriodoProyectoId':0,'Vigencia':2024,'ValorVigenteFutura':0.0000}]},{'EtapaId':2,'Etapa':'Inversión','FuenteId':1215,'Fuente':'MINISTERIO DE JUSTICIA Y DEL DERECHO - GESTIÓN GENERAL','TipoEntidad':'Entidades Presupuesto Nacional - PGN','EntidadId':1,'Entidad':'PGN','TipoRecursoId':2,'TipoRecurso':'Propios','ApropiacionVigente':50000000000.0000,'Vigencias':[{'PeriodoProyectoId':0,'Vigencia':2021,'ValorVigenteFutura':0.0000},{'PeriodoProyectoId':0,'Vigencia':2022,'ValorVigenteFutura':0.0000},{'PeriodoProyectoId':0,'Vigencia':2023,'ValorVigenteFutura':0.0000},{'PeriodoProyectoId':0,'Vigencia':2024,'ValorVigenteFutura':0.0000}]},{'EtapaId':2,'Etapa':'Inversión','FuenteId':1216,'Fuente':'DIRECCION NACIONAL DE ESTUPEFACIENTES','TipoEntidad':'Entidades Presupuesto Nacional - PGN','EntidadId':1,'Entidad':'PGN','TipoRecursoId':1,'TipoRecurso':'Nación','ApropiacionVigente':17298000000.0000,'Vigencias':[{'PeriodoProyectoId':0,'Vigencia':2021,'ValorVigenteFutura':0.0000},{'PeriodoProyectoId':0,'Vigencia':2022,'ValorVigenteFutura':0.0000},{'PeriodoProyectoId':0,'Vigencia':2023,'ValorVigenteFutura':0.0000},{'PeriodoProyectoId':0,'Vigencia':2024,'ValorVigenteFutura':0.0000}]},{'EtapaId':2,'Etapa':'Inversión','FuenteId':1217,'Fuente':'DIRECCION NACIONAL DE ESTUPEFACIENTES','TipoEntidad':'Entidades Presupuesto Nacional - PGN','EntidadId':1,'Entidad':'PGN','TipoRecursoId':2,'TipoRecurso':'Propios','ApropiacionVigente':100000000000.0000,'Vigencias':[{'PeriodoProyectoId':0,'Vigencia':2021,'ValorVigenteFutura':0.0000},{'PeriodoProyectoId':0,'Vigencia':2022,'ValorVigenteFutura':0.0000},{'PeriodoProyectoId':0,'Vigencia':2023,'ValorVigenteFutura':0.0000},{'PeriodoProyectoId':0,'Vigencia':2024,'ValorVigenteFutura':0.0000}]}]}";//Contexto.uspGetProyectoTramite(ProyectoId, TramiteId).SingleOrDefault();
                proyectoTramite = JsonConvert.DeserializeObject<VigenciaFuturaCorrienteDto>(jsonString);
                bool cumple = false;

                if (proyectoTramite.ValorTotalVigente > proyectoTramite.Porcentaje)
                {
                    cumple = true;
                }

                int AnioActual = DateTime.Now.Year;

                proyectoTramite.cumple = cumple;
                proyectoTramite.AnioInicio = proyectoTramite.AñoInicio;
                proyectoTramite.AnioFin = proyectoTramite.AñoFin;

                foreach (var item in proyectoTramite.Fuentes)
                {
                    double? valorTotalVigenciaFutura = 0;
                    foreach (var item2 in item.Vigencias)
                    {
                        valorTotalVigenciaFutura = valorTotalVigenciaFutura + item2.ValorVigenteFutura;
                    }
                    item.ValorTotalVigenciaFutura = valorTotalVigenciaFutura;
                }
                return proyectoTramite;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public async Task<int> EliminarPermisosAccionesUsuarios(string usuarioDestino, int tramiteId, string aliasNivel, string usuarioDnp, Guid InstanciaId = default(Guid))
        {
            return await _tramiteServicios.EliminarPermisosAccionesUsuarios(usuarioDestino, tramiteId, aliasNivel, usuarioDnp, InstanciaId);
        }

        public async Task<AccionDto> ObtenerAccionActualyFinal(int tramiteId, string bpin, string usuarioDnp)
        {
            return await _tramiteServicios.ObtenerAccionActualyFinal(tramiteId, bpin, usuarioDnp);
        }

        async Task<VigenciaFuturaResponse> ITramiteServicios.ActualizarVigenciaFuturaFuente(VigenciaFuturaCorrienteFuenteDto fuente, string usuarioDnp)
        {
            return await _tramiteServicios.ActualizarVigenciaFuturaFuente(fuente, usuarioDnp);
        }

        async Task<VigenciaFuturaConstanteDto> ITramiteServicios.ObtenerFuentesFinanciacionVigenciaFuturaConstante(string bpin, int tramiteId, string usuarioDnp)
        {
            return await _tramiteServicios.ObtenerFuentesFinanciacionVigenciaFuturaConstante(bpin, tramiteId, usuarioDnp);
        }

        public async Task<RespuestaGeneralDto> EnviarConceptoDireccionTecnicaTramite(int tramiteId, string usuarioDnp, string usuarioLogueado)
        {
            RespuestaGeneralDto response = new RespuestaGeneralDto();
            response.Exito = true;
            response.Mensaje = "OK";

            await Task.Run(() =>
            {
                response.Exito = true;
                response.Mensaje = "OK";
            });

            return response;
        }

        Task<List<TramiteModalidadContratacionDto>> ITramiteServicios.ObtenerModalidadesContratacion(int mostrar, string usuarioDnp)
        {
            List<TramiteModalidadContratacionDto> lista = new List<TramiteModalidadContratacionDto>();
            lista.Add(new TramiteModalidadContratacionDto() { Id = 1, Nombre = "Contratación Directa" });
            lista.Add(new TramiteModalidadContratacionDto() { Id = 2, Nombre = "Licitación Publica" });
            lista.Add(new TramiteModalidadContratacionDto() { Id = 3, Nombre = "Concurso de méritos" });
            lista.Add(new TramiteModalidadContratacionDto() { Id = 4, Nombre = "Mínima Cuantía" });
            lista.Add(new TramiteModalidadContratacionDto() { Id = 5, Nombre = "Selección Abreviada" });
            return Task.FromResult(lista);
        }

        Task<ActividadPreContractualDto> ITramiteServicios.ActualizarActividadesCronograma(ActividadPreContractualDto ModalidadContratacionId, string usuarioDnp)
        {
            ActividadPreContractualDto response = new ActividadPreContractualDto();


            return Task.FromResult(response);
        }

        Task<ActividadPreContractualDto> ITramiteServicios.ObtenerActividadesPrecontractualesProyectoTramite(int ModalidadContratacionId, int ProyectoId, int TramiteId, bool eliminarActividades, string usuarioDnp)
        {
            var jsonString = "{'ProyectoId':97861,'ActividadesPreContractuales':[{'ActividadPreContractualId':1,'Actividad':'Actividad precontractual de prueba 1','CronogramaId':4,'ModalidadContratacionId':1,'FechaInicial':'2022 - 06 - 04','FechaFinal':'2022 - 06 - 03'},{'ActividadPreContractualId':2,'Actividad':'Actividad precontractual de prueba 2','CronogramaId':5,'ModalidadContratacionId':1,'FechaInicial':'2022 - 05 - 01','FechaFinal':'2020 - 05 - 27'},{'ActividadPreContractualId':4,'Actividad':'Actividad precontractual de prueba 3','CronogramaId':19,'ModalidadContratacionId':1,'FechaInicial':'2022 - 06 - 03','FechaFinal':'2022 - 06 - 29'},{'ActividadPreContractualId':4,'Actividad':'Actividad precontractual de prueba 3','CronogramaId':20,'ModalidadContratacionId':1,'FechaInicial':'2022 - 06 - 02','FechaFinal':'2022 - 01 - 01'},{'ActividadPreContractualId':4,'Actividad':'Actividad precontractual de prueba 3','CronogramaId':21,'ModalidadContratacionId':1,'FechaInicial':'2022 - 01 - 01','FechaFinal':'2022 - 06 - 03'},{'ActividadPreContractualId':4,'Actividad':'Actividad precontractual de prueba 3','CronogramaId':22,'ModalidadContratacionId':1,'FechaInicial':'2022 - 06 - 03','FechaFinal':'2022 - 06 - 22'},{'ActividadPreContractualId':4,'Actividad':'Actividad precontractual de prueba 3','CronogramaId':23,'ModalidadContratacionId':1,'FechaInicial':'2022 - 06 - 02','FechaFinal':'2022 - 07 - 06'}],'ActividadesContractuales':[{'ActividadPreContractualId':null,'Actividad':'Actividad contractual 1','CronogramaId':12,'ModalidadContratacionId':1,'FechaInicial':'2022 - 06 - 09','FechaFinal':'2022 - 05 - 02','TramiteProyectoId':1410},{'ActividadPreContractualId':null,'Actividad':'Actividad contractual2','CronogramaId':28,'ModalidadContratacionId':1,'FechaInicial':'2022 - 06 - 06','FechaFinal':'2022 - 06 - 08','TramiteProyectoId':1410}]}";

            var actividades = JsonConvert.DeserializeObject<ActividadPreContractualDto>(jsonString);

            return Task.FromResult(actividades);
        }

        public Task<ProductosConstantesVF> ObtenerProductosVigenciaFuturaConstante(string Bpin, int TramiteId, int AnioBase, string usuarioDnp)
        {
            ProductosConstantesVF response = new ProductosConstantesVF();
            return Task.FromResult(response);
        }

        public Task<VigenciaFuturaResponse> ActualizarVigenciaFuturaProducto(ProductosConstantesVFDetalleProducto prod, string usuarioDnp)
        {
            VigenciaFuturaResponse response = new VigenciaFuturaResponse();
            return Task.FromResult(response);
        }

        public Task<ProductosCorrientesVF> ObtenerProductosVigenciaFuturaCorriente(string Bpin, int TramiteId, string usuarioDnp)
        {

            var jsonString = "{'ProyectoId':97750,'BPIN':'202200000000002','ResumenObjetivos':[{'ObjetivoEspecificoid':776,'ObjetivoEspecifico':'Aumentar el número de cupos penitenciarios y carcelarios para atender a la PPL','Productos':[{'ProductoId':1332,'Producto':'Infraestructura penitenciaria y carcelaria construida','Vigencias':[{'PeriodoProyectoId':1351,'Vigencia':2022,'ValorVigenteFutura':502300000000.0000},{'PeriodoProyectoId':1352,'Vigencia':2023,'ValorVigenteFutura':0.0000},{'PeriodoProyectoId':1353,'Vigencia':2024,'ValorVigenteFutura':0.0000},{'PeriodoProyectoId':1354,'Vigencia':2025,'ValorVigenteFutura':0.0000}]}]},{'ObjetivoEspecificoid':777,'ObjetivoEspecifico':'Obtener información técnica eficiente para dar viabilidad a nuevos proyectos de construcción y ampliación de cupos','Productos':[{'ProductoId':1331,'Producto':'Servicio de información penitenciaria y carcelaria para la toma de decisiones','Vigencias':[{'PeriodoProyectoId':1351,'Vigencia':2022,'ValorVigenteFutura':432000000000.0000},{'PeriodoProyectoId':1352,'Vigencia':2023,'ValorVigenteFutura':0.0000},{'PeriodoProyectoId':1353,'Vigencia':2024,'ValorVigenteFutura':0.0000},{'PeriodoProyectoId':1354,'Vigencia':2025,'ValorVigenteFutura':0.0000}]}]}],'DetalleObjetivos':[{'ObjetivoEspecificoid':776,'ObjetivoEspecifico':'Aumentar el número de cupos penitenciarios y carcelarios para atender a la PPL','Productos':[{'ProductoId':1332,'Producto':'Infraestructura penitenciaria y carcelaria construida','Vigencias':[{'PeriodoProyectoId':1351,'Vigencia':2022,'ValorSolicitado':502300000000.0000,'Decreto':0,'ValorVigente':502300000000.0000,'VigenteFuturasAnteriores':0,'TotalVigenciasFuturaSolicitada':0},{'PeriodoProyectoId':1352,'Vigencia':2023,'ValorSolicitado':0.0000,'Decreto':0,'ValorVigente':0.0000,'VigenteFuturasAnteriores':0,'TotalVigenciasFuturaSolicitada':0},{'PeriodoProyectoId':1353,'Vigencia':2024,'ValorSolicitado':0.0000,'Decreto':0,'ValorVigente':0.0000,'VigenteFuturasAnteriores':0,'TotalVigenciasFuturaSolicitada':0},{'PeriodoProyectoId':1354,'Vigencia':2025,'ValorSolicitado':0.0000,'Decreto':0,'ValorVigente':0.0000,'VigenteFuturasAnteriores':0,'TotalVigenciasFuturaSolicitada':0}]}]},{'ObjetivoEspecificoid':777,'ObjetivoEspecifico':'Obtener información técnica eficiente para dar viabilidad a nuevos proyectos de construcción y ampliación de cupos','Productos':[{'ProductoId':1331,'Producto':'Servicio de información penitenciaria y carcelaria para la toma de decisiones','Vigencias':[{'PeriodoProyectoId':1351,'Vigencia':2022,'ValorSolicitado':432000000000.0000,'Decreto':0,'ValorVigente':432000000000.0000,'VigenteFuturasAnteriores':0,'TotalVigenciasFuturaSolicitada':0},{'PeriodoProyectoId':1352,'Vigencia':2023,'ValorSolicitado':0.0000,'Decreto':0,'ValorVigente':0.0000,'VigenteFuturasAnteriores':0,'TotalVigenciasFuturaSolicitada':0},{'PeriodoProyectoId':1353,'Vigencia':2024,'ValorSolicitado':0.0000,'Decreto':0,'ValorVigente':0.0000,'VigenteFuturasAnteriores':0,'TotalVigenciasFuturaSolicitada':0},{'PeriodoProyectoId':1354,'Vigencia':2025,'ValorSolicitado':0.0000,'Decreto':0,'ValorVigente':0.0000,'VigenteFuturasAnteriores':0,'TotalVigenciasFuturaSolicitada':0}]}]}]}";

            var actividades = JsonConvert.DeserializeObject<ProductosCorrientesVF>(jsonString);

            return Task.FromResult(actividades);
        }
        public Task<List<TipoDocumentoTramiteDto>> ObtenerTipoDocumentoTramitePorNivel(int tipoTramiteId, string nivelId, string rolId, string usuarioDnp)
        {
            var jsonString = "[{'Id':1, 'TipoDocumentoId': 1, 'TipoDocumento': 'Análisis Ecónomico (Supuestos de Costeo)' ,'TipoTramiteId': 2,'Obligatorio':true}]";

            var TipoDocumentos = JsonConvert.DeserializeObject<List<TipoDocumentoTramiteDto>>(jsonString);

            return Task.FromResult(TipoDocumentos);
        }

        public Task<ResponseDto<bool>> CerrarRadicadosTramite(string numeroTramite, string usuarioDnp)
        {
            throw new NotImplementedException();
        }

        Task<dynamic> ITramiteServicios.CerrarRadicadosTramite(string numeroTramite, string usuarioDnp)
        {
            throw new NotImplementedException();
        }
        public async Task<List<DatosUsuarioDto>> ObtenerDatosUsuario(string idUsuarioDnp, int idEntidad, Guid idAccion, Guid idIntancia, string usuarioDNP)
        {
            var result = await Task.Run(() => _tramiteServicios.ObtenerDatosUsuario(idUsuarioDnp, idEntidad, idAccion, idIntancia, usuarioDNP));
            return result;

        }

        public async Task<ModificacionLeyendaDto> ObtenerModificacionLeyenda(int tramiteId, int ProyectoId, string usuarioDNP)
        {
            var result = await Task.Run(() => _tramiteServicios.ObtenerModificacionLeyenda(tramiteId, ProyectoId, usuarioDNP));
            return result;
        }

        public async Task<string> ActualizarModificacionLeyenda(ModificacionLeyendaDto modificacionLeyendaDto, string usuario)
        {
            var result = await Task.Run(() => _tramiteServicios.ActualizarModificacionLeyenda(modificacionLeyendaDto, usuario));
            return result;
        }

        public async Task<List<EntidadCatalogoDTDto>> ObtenerListaDirecionesDNP(Guid idEntididad, string usuarioDnp)
        {
            var result = await Task.Run(() => _tramiteServicios.ObtenerListaDirecionesDNP(idEntididad, usuarioDnp));
            return result;
        }

        public async Task<List<EntidadCatalogoDTDto>> ObtenerListaSubdirecionesPorParentId(int idEntididadType, string usuarioDnp)
        {
            var result = await Task.Run(() => _tramiteServicios.ObtenerListaSubdirecionesPorParentId(idEntididadType, usuarioDnp));
            return result;
        }

        public async Task<RespuestaGeneralDto> BorrarFirma(string idUsuario)
        {
            var result = await Task.Run(() => _tramiteServicios.BorrarFirma(idUsuario));
            return result;
        }

        public async Task<ProyectosCartaDto> ObtenerProyectosCartaTramite(int TramiteId, string usuarioDnp)
        {
            var result = await Task.Run(() => _tramiteServicios.ObtenerProyectosCartaTramite(TramiteId, usuarioDnp));
            return result;
        }

        public async Task<DetalleCartaConceptoALDto> ObtenerDetalleCartaAL(int TramiteId, string usuarioDnp)
        {
            var result = await Task.Run(() => _tramiteServicios.ObtenerDetalleCartaAL(TramiteId, usuarioDnp));
            return result;
        }

        public async Task<int> ObtenerAmpliarDevolucionTramite(int ProyectoId, int TramiteId, string usuarioDnp)
        {
            var result = await Task.Run(() => _tramiteServicios.ObtenerAmpliarDevolucionTramite(ProyectoId, TramiteId, usuarioDnp));
            return result;
        }

        public async Task<DatosProyectoTramiteDto> ObtenerDatosProyectoConceptoPorInstancia(Guid instanciaId, string usuarioDnp)
        {
            var result = await Task.Run(() => _tramiteServicios.ObtenerDatosProyectoConceptoPorInstancia(instanciaId, usuarioDnp));
            return result;
        }

        public async Task<List<TramiteLiberacionVfDto>> ObtenerLiberacionVigenciasFuturas(int ProyectoId, int TramiteId, string usuarioDnp)
        {
            var result = await Task.Run(() => _tramiteServicios.ObtenerLiberacionVigenciasFuturas(ProyectoId, TramiteId, usuarioDnp));
            return result;
        }

        public async Task<VigenciaFuturaResponse> InsertaAutorizacionVigenciasFuturas(TramiteALiberarVfDto autorizacion, string usuarioDnp)
        {

            TramiteALiberarVfDto request = new TramiteALiberarVfDto();
            request.LiberacionVigenciasFuturasId = 0;
            request.CodigoProceso = "XXXXXX";
            request.NombreProceso = "el nombre de mi proceso";
            request.Fecha = DateTime.Now;
            request.CodigoAutorizacion = "AAAAAAA001";
            request.FechaAutorizacion = DateTime.Now;

            var result = await Task.Run(() => _tramiteServicios.InsertaAutorizacionVigenciasFuturas(request, usuarioDnp));
            return result;
        }

        public async Task<VigenciaFuturaResponse> InsertaValoresUtilizadosLiberacionVF(TramiteALiberarVfDto autorizacion, string usuarioDnp)
        {

            TramiteALiberarVfDto request = new TramiteALiberarVfDto();
            request.LiberacionVigenciasFuturasId = 0;
            request.CodigoProceso = "XXXXXX";
            request.NombreProceso = "el nombre de mi proceso";
            request.Fecha = DateTime.Now;
            request.CodigoAutorizacion = "AAAAAAA001";
            request.FechaAutorizacion = DateTime.Now;

            var result = await Task.Run(() => _tramiteServicios.InsertaAutorizacionVigenciasFuturas(request, usuarioDnp));
            return result;
        }

        public async Task<List<ProyectoTramiteFuenteDto>> ObtenerListaProyectosFuentes(int tramiteId, string usuarioDnp)
        {
            return await _tramiteServicios.ObtenerListaProyectosFuentes(tramiteId, usuarioDnp);
        }


        public async Task<List<EntidadesAsociarComunDto>> obtenerEntidadAsociarProyecto(Guid InstanciaId, string AccionTramiteProyecto, string usuarioDnp)
        {
            var result = await Task.Run(() => _tramiteServicios.obtenerEntidadAsociarProyecto(InstanciaId, AccionTramiteProyecto, usuarioDnp));
            return result;
        }
        public Task<CartaConcepto> ConsultarCartaConcepto(int tramiteId, string usuarioDnp)
        {
            var jsonString = "{'Id':'2', 'FaseId': '42', 'TramiteId': '902', 'RadicadoEntrada': '309097', 'RadicadoSalida': '3090976', 'FechaCreacion': '2022/08/25','CreadoPor': 'Pruebe','FechaModificacion': '2022/08/25','ModificadoPor': 'prueba','ModificadoPor': 'A3987676676' }";

            var datos = JsonConvert.DeserializeObject<CartaConcepto>(jsonString);
            return Task.FromResult(datos);
        }

        public Task<int> ValidacionPeriodoPresidencial(int tramiteId, string usuarioDnp)
        {
            var jsonString = "1";

            var datos = JsonConvert.DeserializeObject<int>(jsonString);
            return Task.FromResult(datos);
        }

        public Task<string> GuardarMontosTramite(List<ProyectosEnTramiteDto> proyectosEnTramiteDto, string usuario)
        {
            try
            {
                string resultado = string.Empty;
                var result = string.Empty;//consulta de base de datos


                if (string.IsNullOrEmpty(result))
                {
                    resultado = "OK";
                    return Task.FromResult(resultado);
                }
                else
                {
                    var mensajeError = Convert.ToString(result);
                    resultado = mensajeError;
                    throw new Exception(mensajeError);
                }

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public Task<List<ProyectoJustificacioneDto>> ObtenerPreguntasJustificacionPorProyectos(int TramiteId, int TipoTramiteId, int TipoRolId, Guid IdNivel, string usuarioDnp)
        {
            List<ProyectoJustificacioneDto> lista = new List<ProyectoJustificacioneDto>();
            ProyectoJustificacioneDto dato = new ProyectoJustificacioneDto();
            dato.ProyectoId = 9889;
            dato.ListaJustificacionPaso = new List<JustificacionPasoDto>();
            JustificacionPasoDto jp = new JustificacionPasoDto();
            jp.Paso = "viabilidad";
            dato.ListaJustificacionPaso.Add(jp);
            JustificacionTramiteProyectoDto j = new JustificacionTramiteProyectoDto();
            j.Tematica = "Proyecto";
            j.Paso = "aprobacion analista";
            j.Cuenta = "Analista";
            j.CuestionarioId = 15241;
            j.FechaEnvio = DateTime.Now;
            j.InstanciaId = new Guid().ToString();
            j.JustificacionId = 1254;
            j.JustificacionRespuesta = "Aprobado";
            j.NombreNivel = "Aprobacion analista";
            j.NombreRol = "Analista";
            dato.ListaJustificacionPaso[0].justificaciones.Add(j);
            lista.Add(dato);
            if (TramiteId == 0)
                return null;
            else
            {
                return Task.FromResult(lista);
            }
        }

        public Task<List<tramiteVFAsociarproyecto>> ObtenerTramitesVFparaLiberar(string numTramite, string usuarioDnp)
        {
            if (string.IsNullOrEmpty(numTramite))
                return null;
            else
            {
                List<tramiteVFAsociarproyecto> datos = new List<tramiteVFAsociarproyecto>();
                tramiteVFAsociarproyecto dato = new tramiteVFAsociarproyecto
                {
                    Descripcion = "tramites",
                    Id = 1,
                    NumeroTramite = "EJ-TP",
                    ObjContratacion = "Objeto",
                    tipotramiteId = 18
                };
                datos.Add(dato);
                return Task.FromResult(datos);
            }
        }

        public Task<string> GuardarLiberacionVigenciaFutura(LiberacionVigenciasFuturasDto liberacionVigenciasFuturasDto, string usuarioDnp)
        {
            string resultado = string.Empty;
            if (string.IsNullOrEmpty(usuarioDnp))
                resultado = "OK";
            else
            {
                var mensajeError = Convert.ToString("no se encontraron datos para guardar");
                resultado = mensajeError;
                throw new Exception(mensajeError);
            }
            return Task.FromResult(resultado);
        }

        public async Task<List<ResumenLiberacionVfDto>> ObtenerResumenLiberacionVigenciasFuturas(int ProyectoId, int TramiteId, string usuarioDnp)
        {
            var result = await Task.Run(() => _tramiteServicios.ObtenerResumenLiberacionVigenciasFuturas(ProyectoId, TramiteId, usuarioDnp));
            return result;
        }

        public async Task<ValoresUtilizadosLiberacionVfDto> ObtenerValUtilizadosLiberacionVigenciasFuturas(int ProyectoId, int TramiteId, string usuarioDnp)
        {
            var result = await Task.Run(() => _tramiteServicios.ObtenerValUtilizadosLiberacionVigenciasFuturas(ProyectoId, TramiteId, usuarioDnp));
            return result;
        }

        public Task<int> TramiteAjusteEnPasoUno(int tramiteId, int proyectoId, string usuarioDnp)
        {
            int resultado = 1;
            return Task.FromResult(resultado);
        }

        public async Task<List<ProyectoTramiteFuenteDto>> ObtenerListaProyectosFuentesAprobado(int tramiteId, string usuarioDnp)
        {
            return await _tramiteServicios.ObtenerListaProyectosFuentesAprobado(tramiteId, usuarioDnp);

        }

        public Task<RespuestaGeneralDto> ActualizarCargueMasivo(ObjetoNegocioDto contenido, string usuarioDnp)
        {
            return _tramiteServicios.ActualizarCargueMasivo(contenido, usuarioDnp);
        }

        public Task<string> ConsultarCargueExcel(ObjetoNegocioDto contenido, string usuarioDnp)
        {
            return _tramiteServicios.ConsultarCargueExcel(contenido, usuarioDnp);
        }

        public Task<VigenciaFuturaResponse> InsertaValoresproductosLiberacionVFCorrientes(DetalleProductosCorrientesDto productosCorrientes, string usuario)
        {
            VigenciaFuturaResponse response = new VigenciaFuturaResponse();
            response.Exito = true;
            response.Mensaje = "OK";

            return Task.FromResult(response);
        }

        public Task<VigenciaFuturaResponse> InsertaValoresproductosLiberacionVFConstantes(DetalleProductosConstantesDto productosConstantes, string usuario)
        {
            VigenciaFuturaResponse response = new VigenciaFuturaResponse();
            response.Exito = true;
            response.Mensaje = "OK";

            return Task.FromResult(response);
        }

        public async Task<List<EntidadesAsociarComunDto>> ObtenerEntidadTramite(string numeroTramite, string usuarioDnp)
        {
            var result = await Task.Run(() => _tramiteServicios.ObtenerEntidadTramite(numeroTramite, usuarioDnp));
            return result;
        }

        public Task<VigenciaFuturaResponse> EliminarLiberacionVigenciaFutura(LiberacionVigenciasFuturasDto eliminarLiberacionVigenciasFuturasDto, string usuarioDnp)
        {
            VigenciaFuturaResponse response = new VigenciaFuturaResponse();
            response.Exito = true;
            response.Mensaje = "OK";

            return Task.FromResult(response);
        }
        public Task<List<CalendarioPeriodoDto>> ObtenerCalendartioPeriodo(string bpin, string usuarioDnp)
        {
            List<CalendarioPeriodoDto> response = new List<CalendarioPeriodoDto>();
            CalendarioPeriodoDto r = new CalendarioPeriodoDto();
            response.Add(r);

            return Task.FromResult(response);
        }

        public Task<PresupuestalProyectosAsociadosDto> ObtenerPresupuestalProyectosAsociados(int TramiteId, Guid InstanciaId, string usuarioDnp)
        {
            var resumen = new PresupuestalProyectosAsociadosDto();
            string jsonString = "{'TramiteId':1090,'ResumenProyectos':[{'TipoOperacion':'Contrato','TotalTipoOperacion':0,'Proyectos':[{'ProyectoId':98173,'CodigoBpin':'202200000000226','NombreProyecto':'Construcción Ampliación de Infraestructura para Generación de Cupos en Los Establecimientos de Reclusión del Orden - Bogotá','NombreProyectoCorto':'Construcción Ampliación de Infraestructura para Generación de Cupos en Los Estab','CodigoPresupuestal':'0209001206080000010000','TotalSolicitadoNacion':0.00,'TotalSolicitadoPropios':0.00,'TotalAprobadoNacion':0.00,'TotalAprobadoPropios':0.00}]},{'TipoOperacion':'Credito','TotalTipoOperacion':0,'Proyectos':[{'ProyectoId':98189,'CodigoBpin':'202200000000235','NombreProyecto':'Construcción Ampliación de Infraestructura para Generación de Cupos en Los Establecimientos de Reclusión del Orden - Bogotá','NombreProyectoCorto':'Construcción Ampliación de Infraestructura para Generación de Cupos en Los Estab','CodigoPresupuestal':'1201011206080001020000','TotalSolicitadoNacion':0.00,'TotalSolicitadoPropios':0.00,'TotalAprobadoNacion':0.00,'TotalAprobadoPropios':0.00}]}],'ProyectosAsociados':[{'ProyectoId':98189,'CodigoBpin':'202200000000235','NombreProyecto':'Construcción Ampliación de Infraestructura para Generación de Cupos en Los Establecimientos de Reclusión del Orden - Bogotá','NombreProyectoCorto':'Construcción Ampliación de Infraestructura para Generación de Cupos en Los Estab','EntidadFinanciadora':'MINISTERIO DE JUSTICIA Y DEL DERECHO - GESTIÓN GENERAL','NombreSector':'Justicia y del Derecho','TipoProyecto':'Credito','CodigoPresupuestal':'1201011206080001020000','VigenciaInicial':2022,'VigenciaFinal':2025,'TotalApropiacionInicialNacion':0.00,'TotalApropiacionInicialPropios':0.00,'TotalApropiacionVigenteNacion':0.00,'TotalApropiacionVigentePropios':0.00,'TotalVigenciasFuturasNacion':0.00,'TotalVigenciasFuturasPropios':0.00,'DetalleFuentes':[{'TipoRecursoId':1,'NombreTipoRecurso':'10-Recursos corrientes-Nación','ValorInicialCSF':0.00,'ValorInicialSSF':0.00,'ValorVigenteCSF':0.00,'ValorVigenteSSF':0.00,'ValorIncorporarCSF':0.00,'ValorIncorporarSSF':0.00,'ValorIncorporarAprobadoCSF':0.00,'ValorIncorporarAprobadoSSF':0.00},{'TipoRecursoId':2,'NombreTipoRecurso':'11-Otros Recursos del Tesoro-Nación','ValorInicialCSF':0.00,'ValorInicialSSF':0.00,'ValorVigenteCSF':0.00,'ValorVigenteSSF':0.00,'ValorIncorporarCSF':0.00,'ValorIncorporarSSF':0.00,'ValorIncorporarAprobadoCSF':0.00,'ValorIncorporarAprobadoSSF':0.00},{'TipoRecursoId':3,'NombreTipoRecurso':'12-Recursos para preservar la seguridad democrática-Nación','ValorInicialCSF':0.00,'ValorInicialSSF':0.00,'ValorVigenteCSF':0.00,'ValorVigenteSSF':0.00,'ValorIncorporarCSF':0.00,'ValorIncorporarSSF':0.00,'ValorIncorporarAprobadoCSF':0.00,'ValorIncorporarAprobadoSSF':0.00},{'TipoRecursoId':4,'NombreTipoRecurso':'13-Recursos del Crédito Externo Previa Autorización-Nación','ValorInicialCSF':0.00,'ValorInicialSSF':0.00,'ValorVigenteCSF':0.00,'ValorVigenteSSF':0.00,'ValorIncorporarCSF':0.00,'ValorIncorporarSSF':0.00,'ValorIncorporarAprobadoCSF':0.00,'ValorIncorporarAprobadoSSF':0.00},{'TipoRecursoId':5,'NombreTipoRecurso':'14-Préstamos Destinación Específica-Nación','ValorInicialCSF':0.00,'ValorInicialSSF':0.00,'ValorVigenteCSF':0.00,'ValorVigenteSSF':0.00,'ValorIncorporarCSF':0.00,'ValorIncorporarSSF':0.00,'ValorIncorporarAprobadoCSF':0.00,'ValorIncorporarAprobadoSSF':0.00},{'TipoRecursoId':6,'NombreTipoRecurso':'15-Donaciones-Nación','ValorInicialCSF':0.00,'ValorInicialSSF':0.00,'ValorVigenteCSF':0.00,'ValorVigenteSSF':0.00,'ValorIncorporarCSF':0.00,'ValorIncorporarSSF':0.00,'ValorIncorporarAprobadoCSF':0.00,'ValorIncorporarAprobadoSSF':0.00},{'TipoRecursoId':7,'NombreTipoRecurso':'16-Fondos Especiales-Nación','ValorInicialCSF':0.00,'ValorInicialSSF':0.00,'ValorVigenteCSF':0.00,'ValorVigenteSSF':0.00,'ValorIncorporarCSF':0.00,'ValorIncorporarSSF':0.00,'ValorIncorporarAprobadoCSF':0.00,'ValorIncorporarAprobadoSSF':0.00},{'TipoRecursoId':8,'NombreTipoRecurso':'17-Rentas Parafiscales-Nación','ValorInicialCSF':0.00,'ValorInicialSSF':0.00,'ValorVigenteCSF':0.00,'ValorVigenteSSF':0.00,'ValorIncorporarCSF':0.00,'ValorIncorporarSSF':0.00,'ValorIncorporarAprobadoCSF':0.00,'ValorIncorporarAprobadoSSF':0.00},{'TipoRecursoId':9,'NombreTipoRecurso':'20-Ingresos Corrientes-Propios','ValorInicialCSF':0.00,'ValorInicialSSF':0.00,'ValorVigenteCSF':0.00,'ValorVigenteSSF':0.00,'ValorIncorporarCSF':0.00,'ValorIncorporarSSF':0.00,'ValorIncorporarAprobadoCSF':0.00,'ValorIncorporarAprobadoSSF':0.00},{'TipoRecursoId':10,'NombreTipoRecurso':'21-Otros Recursos de Tesorería-Propios','ValorInicialCSF':0.00,'ValorInicialSSF':0.00,'ValorVigenteCSF':0.00,'ValorVigenteSSF':0.00,'ValorIncorporarCSF':0.00,'ValorIncorporarSSF':0.00,'ValorIncorporarAprobadoCSF':0.00,'ValorIncorporarAprobadoSSF':0.00},{'TipoRecursoId':11,'NombreTipoRecurso':'22-Recursos del Crédito Interno Previa Autorización-Propios','ValorInicialCSF':0.00,'ValorInicialSSF':0.00,'ValorVigenteCSF':0.00,'ValorVigenteSSF':0.00,'ValorIncorporarCSF':0.00,'ValorIncorporarSSF':0.00,'ValorIncorporarAprobadoCSF':0.00,'ValorIncorporarAprobadoSSF':0.00},{'TipoRecursoId':12,'NombreTipoRecurso':'23-Recursos del Crédito Externo Previa Autorización-Propios','ValorInicialCSF':0.00,'ValorInicialSSF':0.00,'ValorVigenteCSF':0.00,'ValorVigenteSSF':0.00,'ValorIncorporarCSF':0.00,'ValorIncorporarSSF':0.00,'ValorIncorporarAprobadoCSF':0.00,'ValorIncorporarAprobadoSSF':0.00},{'TipoRecursoId':13,'NombreTipoRecurso':'24-Préstamos Destinación Específica-Propios','ValorInicialCSF':0.00,'ValorInicialSSF':0.00,'ValorVigenteCSF':0.00,'ValorVigenteSSF':0.00,'ValorIncorporarCSF':0.00,'ValorIncorporarSSF':0.00,'ValorIncorporarAprobadoCSF':0.00,'ValorIncorporarAprobadoSSF':0.00},{'TipoRecursoId':14,'NombreTipoRecurso':'25-Donaciones-Propios','ValorInicialCSF':0.00,'ValorInicialSSF':0.00,'ValorVigenteCSF':0.00,'ValorVigenteSSF':0.00,'ValorIncorporarCSF':0.00,'ValorIncorporarSSF':0.00,'ValorIncorporarAprobadoCSF':0.00,'ValorIncorporarAprobadoSSF':0.00},{'TipoRecursoId':15,'NombreTipoRecurso':'26-Fondos Especiales-Propios','ValorInicialCSF':0.00,'ValorInicialSSF':0.00,'ValorVigenteCSF':0.00,'ValorVigenteSSF':0.00,'ValorIncorporarCSF':0.00,'ValorIncorporarSSF':0.00,'ValorIncorporarAprobadoCSF':0.00,'ValorIncorporarAprobadoSSF':0.00},{'TipoRecursoId':16,'NombreTipoRecurso':'27-Rentas Parafiscales-Propios','ValorInicialCSF':0.00,'ValorInicialSSF':0.00,'ValorVigenteCSF':0.00,'ValorVigenteSSF':0.00,'ValorIncorporarCSF':0.00,'ValorIncorporarSSF':0.00,'ValorIncorporarAprobadoCSF':0.00,'ValorIncorporarAprobadoSSF':0.00}]}],'ProyectosAportantes':[{'ProyectoId':98173,'CodigoBpin':'202200000000226','NombreProyecto':'Construcción Ampliación de Infraestructura para Generación de Cupos en Los Establecimientos de Reclusión del Orden - Bogotá','NombreProyectoCorto':'Construcción Ampliación de Infraestructura para Generación de Cupos en Los Estab','EntidadFinanciadora':'AGENCIA PRESIDENCIAL DE COOPERACIÓN INTERNACIONAL DE COLOMBIA, APC - COLOMBIA ','NombreSector':'Presidencia De La República','TipoProyecto':'Contrato','CodigoPresupuestal':'0209001206080000010000','VigenciaInicial':2022,'VigenciaFinal':2025,'TotalApropiacionInicialNacion':0.00,'TotalApropiacionInicialPropios':0.00,'TotalApropiacionVigenteNacion':0.00,'TotalApropiacionVigentePropios':0.00,'TotalVigenciasFuturasNacion':0.00,'TotalVigenciasFuturasPropios':0.00,'DetalleFuentes':[{'TipoRecursoId':1,'NombreTipoRecurso':'10-Recursos corrientes-Nación','ValorInicialCSF':0.00,'ValorInicialSSF':0.00,'ValorVigenteCSF':0.00,'ValorVigenteSSF':0.00,'ValorIncorporarCSF':0.00,'ValorIncorporarSSF':0.00,'ValorIncorporarAprobadoCSF':0.00,'ValorIncorporarAprobadoSSF':0.00},{'TipoRecursoId':2,'NombreTipoRecurso':'11-Otros Recursos del Tesoro-Nación','ValorInicialCSF':0.00,'ValorInicialSSF':0.00,'ValorVigenteCSF':0.00,'ValorVigenteSSF':0.00,'ValorIncorporarCSF':0.00,'ValorIncorporarSSF':0.00,'ValorIncorporarAprobadoCSF':0.00,'ValorIncorporarAprobadoSSF':0.00},{'TipoRecursoId':3,'NombreTipoRecurso':'12-Recursos para preservar la seguridad democrática-Nación','ValorInicialCSF':0.00,'ValorInicialSSF':0.00,'ValorVigenteCSF':0.00,'ValorVigenteSSF':0.00,'ValorIncorporarCSF':0.00,'ValorIncorporarSSF':0.00,'ValorIncorporarAprobadoCSF':0.00,'ValorIncorporarAprobadoSSF':0.00},{'TipoRecursoId':4,'NombreTipoRecurso':'13-Recursos del Crédito Externo Previa Autorización-Nación','ValorInicialCSF':0.00,'ValorInicialSSF':0.00,'ValorVigenteCSF':0.00,'ValorVigenteSSF':0.00,'ValorIncorporarCSF':0.00,'ValorIncorporarSSF':0.00,'ValorIncorporarAprobadoCSF':0.00,'ValorIncorporarAprobadoSSF':0.00},{'TipoRecursoId':5,'NombreTipoRecurso':'14-Préstamos Destinación Específica-Nación','ValorInicialCSF':0.00,'ValorInicialSSF':0.00,'ValorVigenteCSF':0.00,'ValorVigenteSSF':0.00,'ValorIncorporarCSF':0.00,'ValorIncorporarSSF':0.00,'ValorIncorporarAprobadoCSF':0.00,'ValorIncorporarAprobadoSSF':0.00},{'TipoRecursoId':6,'NombreTipoRecurso':'15-Donaciones-Nación','ValorInicialCSF':0.00,'ValorInicialSSF':0.00,'ValorVigenteCSF':0.00,'ValorVigenteSSF':0.00,'ValorIncorporarCSF':0.00,'ValorIncorporarSSF':0.00,'ValorIncorporarAprobadoCSF':0.00,'ValorIncorporarAprobadoSSF':0.00},{'TipoRecursoId':7,'NombreTipoRecurso':'16-Fondos Especiales-Nación','ValorInicialCSF':0.00,'ValorInicialSSF':0.00,'ValorVigenteCSF':0.00,'ValorVigenteSSF':0.00,'ValorIncorporarCSF':0.00,'ValorIncorporarSSF':0.00,'ValorIncorporarAprobadoCSF':0.00,'ValorIncorporarAprobadoSSF':0.00},{'TipoRecursoId':8,'NombreTipoRecurso':'17-Rentas Parafiscales-Nación','ValorInicialCSF':0.00,'ValorInicialSSF':0.00,'ValorVigenteCSF':0.00,'ValorVigenteSSF':0.00,'ValorIncorporarCSF':0.00,'ValorIncorporarSSF':0.00,'ValorIncorporarAprobadoCSF':0.00,'ValorIncorporarAprobadoSSF':0.00},{'TipoRecursoId':9,'NombreTipoRecurso':'20-Ingresos Corrientes-Propios','ValorInicialCSF':0.00,'ValorInicialSSF':0.00,'ValorVigenteCSF':0.00,'ValorVigenteSSF':0.00,'ValorIncorporarCSF':0.00,'ValorIncorporarSSF':0.00,'ValorIncorporarAprobadoCSF':0.00,'ValorIncorporarAprobadoSSF':0.00},{'TipoRecursoId':10,'NombreTipoRecurso':'21-Otros Recursos de Tesorería-Propios','ValorInicialCSF':0.00,'ValorInicialSSF':0.00,'ValorVigenteCSF':0.00,'ValorVigenteSSF':0.00,'ValorIncorporarCSF':0.00,'ValorIncorporarSSF':0.00,'ValorIncorporarAprobadoCSF':0.00,'ValorIncorporarAprobadoSSF':0.00},{'TipoRecursoId':11,'NombreTipoRecurso':'22-Recursos del Crédito Interno Previa Autorización-Propios','ValorInicialCSF':0.00,'ValorInicialSSF':0.00,'ValorVigenteCSF':0.00,'ValorVigenteSSF':0.00,'ValorIncorporarCSF':0.00,'ValorIncorporarSSF':0.00,'ValorIncorporarAprobadoCSF':0.00,'ValorIncorporarAprobadoSSF':0.00},{'TipoRecursoId':12,'NombreTipoRecurso':'23-Recursos del Crédito Externo Previa Autorización-Propios','ValorInicialCSF':0.00,'ValorInicialSSF':0.00,'ValorVigenteCSF':0.00,'ValorVigenteSSF':0.00,'ValorIncorporarCSF':0.00,'ValorIncorporarSSF':0.00,'ValorIncorporarAprobadoCSF':0.00,'ValorIncorporarAprobadoSSF':0.00},{'TipoRecursoId':13,'NombreTipoRecurso':'24-Préstamos Destinación Específica-Propios','ValorInicialCSF':0.00,'ValorInicialSSF':0.00,'ValorVigenteCSF':0.00,'ValorVigenteSSF':0.00,'ValorIncorporarCSF':0.00,'ValorIncorporarSSF':0.00,'ValorIncorporarAprobadoCSF':0.00,'ValorIncorporarAprobadoSSF':0.00},{'TipoRecursoId':14,'NombreTipoRecurso':'25-Donaciones-Propios','ValorInicialCSF':0.00,'ValorInicialSSF':0.00,'ValorVigenteCSF':0.00,'ValorVigenteSSF':0.00,'ValorIncorporarCSF':0.00,'ValorIncorporarSSF':0.00,'ValorIncorporarAprobadoCSF':0.00,'ValorIncorporarAprobadoSSF':0.00},{'TipoRecursoId':15,'NombreTipoRecurso':'26-Fondos Especiales-Propios','ValorInicialCSF':0.00,'ValorInicialSSF':0.00,'ValorVigenteCSF':0.00,'ValorVigenteSSF':0.00,'ValorIncorporarCSF':0.00,'ValorIncorporarSSF':0.00,'ValorIncorporarAprobadoCSF':0.00,'ValorIncorporarAprobadoSSF':0.00},{'TipoRecursoId':16,'NombreTipoRecurso':'27-Rentas Parafiscales-Propios','ValorInicialCSF':0.00,'ValorInicialSSF':0.00,'ValorVigenteCSF':0.00,'ValorVigenteSSF':0.00,'ValorIncorporarCSF':0.00,'ValorIncorporarSSF':0.00,'ValorIncorporarAprobadoCSF':0.00,'ValorIncorporarAprobadoSSF':0.00}]}]}";

            resumen = JsonConvert.DeserializeObject<PresupuestalProyectosAsociadosDto>(jsonString);

            return Task.FromResult(resumen);
        }

        public Task<string> ObtenerPresupuestalProyectosAsociados_Adicion(int TramiteId, Guid InstanciaId, string usuarioDnp)
        {
            string jsonString = "{'TramiteId':1179,'ResumenProyectos':[{'TipoOperacion':'Contracredito','TotalTipoOperacion':0,'Proyectos':[{'ProyectoId':113417,'CodigoBpin':'2018011000678','NombreProyecto':'FORTALECIMIENTO DEL SISTEMA DE SEGURIDAD INTEGRAL MARÍTIMA Y FLUVIAL A NIVEL  NACIONAL','NombreProyectoCorto':'FORTALECIMIENTO DEL SISTEMA DE SEGURIDAD INTEGRAL MARÍTIMA Y FLUVIAL A NIVEL  NA','CodigoPresupuestal':'1501121504010000100000','TotalSolicitadoNacionCSF':0.00,'TotalSolicitadoPropiosCSF':0.00,'TotalSolicitadoNacionSSF':0.00,'TotalSolicitadoPropiosSSF':0.00,'TotalAprobadoNacion':0.00,'TotalAprobadoPropios':0.00}]},{'TipoOperacion':'Credito','TotalTipoOperacion':0,'Proyectos':[{'ProyectoId':111992,'CodigoBpin':'2018011000607','NombreProyecto':'IMPLEMENTACIÓN DEL PLAN NACIONAL DE INFRAESTRUCTURA A NIVEL  NACIONAL','NombreProyectoCorto':'IMPLEMENTACIÓN DEL PLAN NACIONAL DE INFRAESTRUCTURA A NIVEL  NACIONAL','CodigoPresupuestal':'1501121504010000080000','TotalSolicitadoNacionCSF':0.00,'TotalSolicitadoPropiosCSF':0.00,'TotalSolicitadoNacionSSF':0.00,'TotalSolicitadoPropiosSSF':0.00,'TotalAprobadoNacion':0.00,'TotalAprobadoPropios':0.00}]}],'ProyectosAsociados':[{'ProyectoId':111992,'CodigoBpin':'2018011000607','NombreProyecto':'IMPLEMENTACIÓN DEL PLAN NACIONAL DE INFRAESTRUCTURA A NIVEL  NACIONAL','NombreProyectoCorto':'IMPLEMENTACIÓN DEL PLAN NACIONAL DE INFRAESTRUCTURA A NIVEL  NACIONAL','EntidadFinanciadora':'MINISTERIO DE DEFENSA NACIONAL - DIRECCION GENERAL MARITIMA - DIMAR','NombreSector':'Defensa y Policía','TipoProyecto':'Credito','CodigoPresupuestal':'1501121504010000080000','VigenciaInicial':2019,'VigenciaFinal':2029,'TotalApropiacionInicialNacion':0.00,'TotalApropiacionInicialPropios':0.00,'TotalApropiacionVigenteNacion':0.00,'TotalApropiacionVigentePropios':0.00,'TotalVigenciasFuturasNacion':0.00,'TotalVigenciasFuturasPropios':0.00,'MontoTramiteNacion':0.00,'MontoTramitePropios':0.00,'DetalleFuentes':[{'TipoRecursoId':6,'NombreTipoRecurso':'15-Donaciones-Nación','ValorInicialCSF':0.00,'ValorInicialSSF':0.00,'ValorVigenteCSF':0.00,'ValorVigenteSSF':0.00,'ValorIncorporarCSF':0.00,'ValorIncorporarSSF':0.00,'ValorIncorporarAprobadoCSF':0.00,'ValorIncorporarAprobadoSSF':0.00},{'TipoRecursoId':14,'NombreTipoRecurso':'25-Donaciones-Propios','ValorInicialCSF':0.00,'ValorInicialSSF':0.00,'ValorVigenteCSF':0.00,'ValorVigenteSSF':0.00,'ValorIncorporarCSF':0.00,'ValorIncorporarSSF':0.00,'ValorIncorporarAprobadoCSF':0.00,'ValorIncorporarAprobadoSSF':0.00}]},{'ProyectoId':111992,'CodigoBpin':'2018011000607','NombreProyecto':'IMPLEMENTACIÓN DEL PLAN NACIONAL DE INFRAESTRUCTURA A NIVEL  NACIONAL','NombreProyectoCorto':'IMPLEMENTACIÓN DEL PLAN NACIONAL DE INFRAESTRUCTURA A NIVEL  NACIONAL','EntidadFinanciadora':'MINISTERIO DE DEFENSA NACIONAL - DIRECCION GENERAL MARITIMA - DIMAR','NombreSector':'Defensa y Policía','TipoProyecto':'Credito','CodigoPresupuestal':'1501121504010000080000','VigenciaInicial':2019,'VigenciaFinal':2029,'TotalApropiacionInicialNacion':0.00,'TotalApropiacionInicialPropios':0.00,'TotalApropiacionVigenteNacion':0.00,'TotalApropiacionVigentePropios':0.00,'TotalVigenciasFuturasNacion':0.00,'TotalVigenciasFuturasPropios':0.00,'MontoTramiteNacion':3200000000.00,'MontoTramitePropios':0.00,'DetalleFuentes':[{'TipoRecursoId':6,'NombreTipoRecurso':'15-Donaciones-Nación','ValorInicialCSF':0.00,'ValorInicialSSF':0.00,'ValorVigenteCSF':0.00,'ValorVigenteSSF':0.00,'ValorIncorporarCSF':0.00,'ValorIncorporarSSF':0.00,'ValorIncorporarAprobadoCSF':0.00,'ValorIncorporarAprobadoSSF':0.00},{'TipoRecursoId':14,'NombreTipoRecurso':'25-Donaciones-Propios','ValorInicialCSF':0.00,'ValorInicialSSF':0.00,'ValorVigenteCSF':0.00,'ValorVigenteSSF':0.00,'ValorIncorporarCSF':0.00,'ValorIncorporarSSF':0.00,'ValorIncorporarAprobadoCSF':0.00,'ValorIncorporarAprobadoSSF':0.00}]}],'ProyectosAportantes':null}";

            return Task.FromResult(jsonString);
        }

        public Task<string> ObtenerResumenReprogramacionPorVigencia(int TramiteId, Guid InstanciaId, int ProyectoId, string usuarioDnp)
        {
            string jsonString = "{'TramiteId':1179,'ResumenProyectos':[{'TipoOperacion':'Contracredito','TotalTipoOperacion':0,'Proyectos':[{'ProyectoId':113417,'CodigoBpin':'2018011000678','NombreProyecto':'FORTALECIMIENTO DEL SISTEMA DE SEGURIDAD INTEGRAL MARÍTIMA Y FLUVIAL A NIVEL  NACIONAL','NombreProyectoCorto':'FORTALECIMIENTO DEL SISTEMA DE SEGURIDAD INTEGRAL MARÍTIMA Y FLUVIAL A NIVEL  NA','CodigoPresupuestal':'1501121504010000100000','TotalSolicitadoNacionCSF':0.00,'TotalSolicitadoPropiosCSF':0.00,'TotalSolicitadoNacionSSF':0.00,'TotalSolicitadoPropiosSSF':0.00,'TotalAprobadoNacion':0.00,'TotalAprobadoPropios':0.00}]},{'TipoOperacion':'Credito','TotalTipoOperacion':0,'Proyectos':[{'ProyectoId':111992,'CodigoBpin':'2018011000607','NombreProyecto':'IMPLEMENTACIÓN DEL PLAN NACIONAL DE INFRAESTRUCTURA A NIVEL  NACIONAL','NombreProyectoCorto':'IMPLEMENTACIÓN DEL PLAN NACIONAL DE INFRAESTRUCTURA A NIVEL  NACIONAL','CodigoPresupuestal':'1501121504010000080000','TotalSolicitadoNacionCSF':0.00,'TotalSolicitadoPropiosCSF':0.00,'TotalSolicitadoNacionSSF':0.00,'TotalSolicitadoPropiosSSF':0.00,'TotalAprobadoNacion':0.00,'TotalAprobadoPropios':0.00}]}],'ProyectosAsociados':[{'ProyectoId':111992,'CodigoBpin':'2018011000607','NombreProyecto':'IMPLEMENTACIÓN DEL PLAN NACIONAL DE INFRAESTRUCTURA A NIVEL  NACIONAL','NombreProyectoCorto':'IMPLEMENTACIÓN DEL PLAN NACIONAL DE INFRAESTRUCTURA A NIVEL  NACIONAL','EntidadFinanciadora':'MINISTERIO DE DEFENSA NACIONAL - DIRECCION GENERAL MARITIMA - DIMAR','NombreSector':'Defensa y Policía','TipoProyecto':'Credito','CodigoPresupuestal':'1501121504010000080000','VigenciaInicial':2019,'VigenciaFinal':2029,'TotalApropiacionInicialNacion':0.00,'TotalApropiacionInicialPropios':0.00,'TotalApropiacionVigenteNacion':0.00,'TotalApropiacionVigentePropios':0.00,'TotalVigenciasFuturasNacion':0.00,'TotalVigenciasFuturasPropios':0.00,'MontoTramiteNacion':0.00,'MontoTramitePropios':0.00,'DetalleFuentes':[{'TipoRecursoId':6,'NombreTipoRecurso':'15-Donaciones-Nación','ValorInicialCSF':0.00,'ValorInicialSSF':0.00,'ValorVigenteCSF':0.00,'ValorVigenteSSF':0.00,'ValorIncorporarCSF':0.00,'ValorIncorporarSSF':0.00,'ValorIncorporarAprobadoCSF':0.00,'ValorIncorporarAprobadoSSF':0.00},{'TipoRecursoId':14,'NombreTipoRecurso':'25-Donaciones-Propios','ValorInicialCSF':0.00,'ValorInicialSSF':0.00,'ValorVigenteCSF':0.00,'ValorVigenteSSF':0.00,'ValorIncorporarCSF':0.00,'ValorIncorporarSSF':0.00,'ValorIncorporarAprobadoCSF':0.00,'ValorIncorporarAprobadoSSF':0.00}]},{'ProyectoId':111992,'CodigoBpin':'2018011000607','NombreProyecto':'IMPLEMENTACIÓN DEL PLAN NACIONAL DE INFRAESTRUCTURA A NIVEL  NACIONAL','NombreProyectoCorto':'IMPLEMENTACIÓN DEL PLAN NACIONAL DE INFRAESTRUCTURA A NIVEL  NACIONAL','EntidadFinanciadora':'MINISTERIO DE DEFENSA NACIONAL - DIRECCION GENERAL MARITIMA - DIMAR','NombreSector':'Defensa y Policía','TipoProyecto':'Credito','CodigoPresupuestal':'1501121504010000080000','VigenciaInicial':2019,'VigenciaFinal':2029,'TotalApropiacionInicialNacion':0.00,'TotalApropiacionInicialPropios':0.00,'TotalApropiacionVigenteNacion':0.00,'TotalApropiacionVigentePropios':0.00,'TotalVigenciasFuturasNacion':0.00,'TotalVigenciasFuturasPropios':0.00,'MontoTramiteNacion':3200000000.00,'MontoTramitePropios':0.00,'DetalleFuentes':[{'TipoRecursoId':6,'NombreTipoRecurso':'15-Donaciones-Nación','ValorInicialCSF':0.00,'ValorInicialSSF':0.00,'ValorVigenteCSF':0.00,'ValorVigenteSSF':0.00,'ValorIncorporarCSF':0.00,'ValorIncorporarSSF':0.00,'ValorIncorporarAprobadoCSF':0.00,'ValorIncorporarAprobadoSSF':0.00},{'TipoRecursoId':14,'NombreTipoRecurso':'25-Donaciones-Propios','ValorInicialCSF':0.00,'ValorInicialSSF':0.00,'ValorVigenteCSF':0.00,'ValorVigenteSSF':0.00,'ValorIncorporarCSF':0.00,'ValorIncorporarSSF':0.00,'ValorIncorporarAprobadoCSF':0.00,'ValorIncorporarAprobadoSSF':0.00}]}],'ProyectosAportantes':null}";

            return Task.FromResult(jsonString);
        }

        public async Task<RespuestaGeneralDto> GuardarDatosReprogramacion(DatosReprogramacionDto Reprogramacion, string usuarioDnp)
        {
            RespuestaGeneralDto response = new RespuestaGeneralDto();
            response.Exito = true;
            response.Mensaje = "OK";

            await Task.Run(() =>
            {
                response.Exito = true;
                response.Mensaje = "OK";
            });

            return response;
        }

        public Task<OrigenRecursosDto> GetOrigenRecursosTramite(int TramiteId, string usuarioDnp)
        {
            var resumen = new OrigenRecursosDto();
            string jsonString = "{'TramiteId':1090, 'TipoOrigenId':2, 'Rubro':'Este es mi rubro'}";

            resumen = JsonConvert.DeserializeObject<OrigenRecursosDto>(jsonString);

            return Task.FromResult(resumen);
        }

        public Task<VigenciaFuturaResponse> SetOrigenRecursosTramite(OrigenRecursosDto origenRecurso, string usuarioDnp)
        {
            VigenciaFuturaResponse response = new VigenciaFuturaResponse();
            response.Exito = true;
            response.Mensaje = "OK";

            return Task.FromResult(response);
        }

        public Task<List<TipoTramiteDto>> ObtenerTiposTramites(InstanciaTramiteDto instanciaTramiteDto, string usuarioDNP)
        {
            List<TipoTramiteDto> tipos = new List<TipoTramiteDto>();
            tipos.Add(new TipoTramiteDto() { Id = 1, Nombre = "Vigencias" });
            tipos.Add(new TipoTramiteDto() { Id = 1, Nombre = "Tramites" });
            return Task.FromResult(tipos);
        }

        public Task<string> ObtenerResumenReprogramacionPorProductoVigencia(Guid InstanciaId, int TramiteId, int? ProyectoId, string usuarioDnp)
        {
            string jsonString = "{'ProyectoId':114913,'TramiteId':2201,'TramiteLiberarId':1932,'CodigoProceso':'EJ-TP-VFO-360101-0002','FechaAutorizacion':'2022-11-30','CodigoAutorizacion':'2-2022-056215','EsConstante':false,'ResumenTramite':[{'añoBase':0,'Valores':[{'Vigencia':2023,'Deflactor':null,'UtilizadoNacion':4251542110.00,'UtilizadoPropios':4251542110.00,'ReprogramadoNacion':0.00,'ReprogramadoPropios':0.00,'ReprogramadoNacionPorProducto':0.00,'ReprogramadoPropiosPorProducto':0.00}],'ValoresCorrientes':null}],'Objetivos':[{'ObjetivoEspecificoId':3786,'ObjetivoEspecifico':'Fortalecer el proceso de atención al ciudadano y mejorar los tiempos de respuesta','Productos':[{'ProductoId':8537,'NombreProducto':'Servicios de información actualizados','Etapa':'Inversión','Valores':[{'PeriodoProyectoId':13969,'Vigencia':2022,'Deflactor':null,'UtilizadoNacion':41086834.00,'UtilizadoPropios':41086834.00,'ReprogramadoNacion':null,'ReprogramadoPropios':null},{'PeriodoProyectoId':13970,'Vigencia':2023,'Deflactor':null,'UtilizadoNacion':2601864066.00,'UtilizadoPropios':2601864066.00,'ReprogramadoNacion':0.00,'ReprogramadoPropios':0.00},{'PeriodoProyectoId':13971,'Vigencia':2024,'Deflactor':null,'UtilizadoNacion':1785289220.00,'UtilizadoPropios':1785289220.00,'ReprogramadoNacion':null,'ReprogramadoPropios':null},{'PeriodoProyectoId':13972,'Vigencia':2025,'Deflactor':null,'UtilizadoNacion':0.00,'UtilizadoPropios':0.00,'ReprogramadoNacion':null,'ReprogramadoPropios':null}],'ValoresCorrientes':null}]},{'ObjetivoEspecificoId':3991,'ObjetivoEspecifico':'Mejorar los niveles de trazabilidad, consulta y almacenamiento de los documentos del ministerio.','Productos':[{'ProductoId':9548,'NombreProducto':'Servicio de Gestión Documental','Etapa':'Inversión','Valores':[{'PeriodoProyectoId':13969,'Vigencia':2022,'Deflactor':null,'UtilizadoNacion':102928325.00,'UtilizadoPropios':102928325.00,'ReprogramadoNacion':null,'ReprogramadoPropios':null},{'PeriodoProyectoId':13970,'Vigencia':2023,'Deflactor':null,'UtilizadoNacion':1649678044.00,'UtilizadoPropios':1649678044.00,'ReprogramadoNacion':0.00,'ReprogramadoPropios':0.00},{'PeriodoProyectoId':13971,'Vigencia':2024,'Deflactor':null,'UtilizadoNacion':1751760480.00,'UtilizadoPropios':1751760480.00,'ReprogramadoNacion':null,'ReprogramadoPropios':null},{'PeriodoProyectoId':13972,'Vigencia':2025,'Deflactor':null,'UtilizadoNacion':1169811142.00,'UtilizadoPropios':1169811142.00,'ReprogramadoNacion':null,'ReprogramadoPropios':null},{'PeriodoProyectoId':13973,'Vigencia':2026,'Deflactor':null,'UtilizadoNacion':665390105.00,'UtilizadoPropios':665390105.00,'ReprogramadoNacion':null,'ReprogramadoPropios':null}],'ValoresCorrientes':null}]}]}";

            return Task.FromResult(jsonString);
        }

        public Task<int> ObtenerModalidadContratacionVigenciasFuturas(int ProyectoId, int TramiteId, string usuarioDnp)
        {
            int modalidad = 2;
            return Task.FromResult( modalidad);
        }

        public Task<List<TramiteRVFAutorizacionDto>> ObtenerAutorizacionesParaReprogramacion(string bpin, int tramite, string tipoTramite, string usuario)
        {
            if (tramite == 0)
                return null;
            List<TramiteRVFAutorizacionDto> response = new List<TramiteRVFAutorizacionDto>();

            TramiteRVFAutorizacionDto autorizacion = new TramiteRVFAutorizacionDto();
            autorizacion.Id = 16;
            autorizacion.NumeroTramite = "EJ-TP-VFO-240200-0029";
            autorizacion.CodigoAutorizacion = "223345";
            autorizacion.Descripcion = "prueba descripción";
            autorizacion.TramiteLiberarId = 2198;
            autorizacion.FechaAutorizacion = DateTime.Now;

            response.Add(autorizacion);
            return Task.FromResult(response);
        }

        public Task<string> AsociarAutorizacionRVF(tramiteRVFAsociarproyecto reprogramacionDto, string usuarioDnp)
        {
            try
            {
                string resultado = string.Empty;
                var result = string.Empty;//consulta de base de datos


                if (string.IsNullOrEmpty(result))
                {
                    resultado = "OK";
                    return Task.FromResult(resultado);
                }
                else
                {
                    var mensajeError = Convert.ToString(result);
                    resultado = mensajeError;
                    throw new Exception(mensajeError);
                }

            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<TramiteRVFAutorizacionDto> ObtenerAutorizacionAsociada(int tramiteId, string usuario)
        {
            if (tramiteId == 0)
                return null;

            TramiteRVFAutorizacionDto autorizacion = new TramiteRVFAutorizacionDto();
            autorizacion.Id = 16;
            autorizacion.NumeroTramite = "EJ-TP-VFO-240200-0029";
            autorizacion.CodigoAutorizacion = "223345";
            autorizacion.Descripcion = "prueba descripción";
            autorizacion.TramiteLiberarId = 2198;
            autorizacion.FechaAutorizacion = DateTime.Now;
            autorizacion.ReprogramacionId = 30;

            return Task.FromResult(autorizacion);
        }

        public Task<string> EliminaReprogramacionVF(ReprogramacionDto reprogramacionDto, string usuarioDnp)
        {
            try
            {
                string resultado = string.Empty;
                var result = string.Empty;//consulta de base de datos


                if (string.IsNullOrEmpty(result))
                {
                    resultado = "OK";
                    return Task.FromResult(resultado);
                }
                else
                {
                    var mensajeError = Convert.ToString(result);
                    resultado = mensajeError;
                    throw new Exception(mensajeError);
                }

            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<InboxTramite> ObtenerInboxTramitesProgramacion(InstanciaTramiteDto instanciaTramiteDto)
        {
            return Task.FromResult(new InboxTramite() { Mensaje = Resources.UsuarioNoTieneTareasPendientes });
        }

        public Task<SystemConfigurationDto> ConsultarSystemConfiguracion(string VariableKey, string Separador, string usuarioDnp)
        {
            throw new NotImplementedException();
        }
    }
}
