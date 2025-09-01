namespace DNP.ServiciosNegocio.Web.API.Test.Mock.Servicios
{
    using DNP.ServiciosNegocio.Comunes;
    using DNP.ServiciosNegocio.Comunes.Dto.Formulario;
    using DNP.ServiciosNegocio.Comunes.Dto.Tramites;
    using DNP.ServiciosNegocio.Comunes.Excepciones;
    using DNP.ServiciosNegocio.Dominio.Dto.Catalogos;
    using DNP.ServiciosNegocio.Dominio.Dto.Conpes;
    using DNP.ServiciosNegocio.Dominio.Dto.FuenteFinanciacion;
    using DNP.ServiciosNegocio.Dominio.Dto.Productos;
    using DNP.ServiciosNegocio.Dominio.Dto.Proyectos;
    using DNP.ServiciosNegocio.Dominio.Dto.Tramites;
    using DNP.ServiciosNegocio.Dominio.Dto.Tramites.VigenciaFutura;
    using DNP.ServiciosNegocio.Dominio.Dto.Transversales;
    using DNP.ServiciosNegocio.Servicios.Interfaces.TramitesProyectos;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using DNP.ServiciosNegocio.Persistencia.Modelo;
    using DNP.ServiciosNegocio.Dominio.Dto.Tramites.TramitesReprogramacion;

    public class TramitesProyectosServicioMock : ITramitesProyectosServicio
    {
        public ParametrosGuardarDto<DatosTramiteProyectosDto> ConstruirParametrosGuardadoVentanas(DatosTramiteProyectosDto contenido)
        {
            var parametrosGuardar = new ParametrosGuardarDto<DatosTramiteProyectosDto>();

            if (contenido != null)
                parametrosGuardar.Contenido = contenido;
            else
                throw new ServiciosNegocioException(string.Format(ServiciosNegocioRecursos.ParametroNoRecibido, "contenido"));

            return parametrosGuardar;
        }

        public ParametrosGuardarDto<InformacionPresupuestalVlrConstanteDto> ConstruirParametrosGuardado(HttpRequestMessage request, InformacionPresupuestalVlrConstanteDto contenido)
        {
            var parametrosGuardar = new ParametrosGuardarDto<InformacionPresupuestalVlrConstanteDto>();

            if (request.Headers.Contains("piip-idInstanciaFlujo"))
                if (Guid.TryParse(request.Headers.GetValues("piip-idInstanciaFlujo").First(), out var valor) && valor != Guid.Empty)
                    parametrosGuardar.InstanciaId = valor;
                else
                    throw new ServiciosNegocioException(string.Format(ServiciosNegocioRecursos.ParametroNoValidos,
                                                                      "piip-idInstanciaFlujo"));
            else
                throw new ServiciosNegocioException(string.Format(ServiciosNegocioRecursos.ParametroNoRecibido,
                                                                  "piip-idInstanciaFlujo"));

            if (request.Headers.Contains("piip-idAccion"))
                if (Guid.TryParse(request.Headers.GetValues("piip-idAccion").First(), out var valor) && valor != Guid.Empty)
                    parametrosGuardar.AccionId = valor;
                else
                    throw new ServiciosNegocioException(string.Format(ServiciosNegocioRecursos.ParametroNoValidos,
                                                                      "piip-idAccion"));
            else
                throw new ServiciosNegocioException(string.Format(ServiciosNegocioRecursos.ParametroNoRecibido,
                                                                  "piip-idAccion"));

            if (contenido != null)
                parametrosGuardar.Contenido = contenido;
            else
                throw new ServiciosNegocioException(string.Format(ServiciosNegocioRecursos.ParametroNoRecibido, "contenido"));

            return parametrosGuardar;

        }

        public TramitesResultado GuardarProyectosTramiteNegocio(DatosTramiteProyectosDto datosTramiteProyectosDto, string usuario)
        {
            var resultado = new TramitesResultado();

            if (datosTramiteProyectosDto.TramiteId != null)
            {
                resultado.Exito = true;
            }
            else
            {
                var mensajeError = "El Dto viene sin información";
                resultado.Exito = false;
                resultado.Mensaje = mensajeError;
            }

            return resultado;
        }

        public TramitesResultado GuardarTramiteInformacionPresupuestal(List<TramiteFuentePresupuestalDto> parametrosGuardar, string usuario)
        {
            throw new System.NotImplementedException();
        }

        public TramitesResultado GuardarTramiteTipoRequisito(List<TramiteRequitoDto> parametrosGuardar, string usuario)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<ProyectosEnTramiteDto> ObtenerProyectosTramiteNegocio(int TramiteId)
        {
            throw new System.NotImplementedException();
        }
        public IEnumerable<TipoDocumentoTramiteDto> ObtenerTipoDocumentoTramite(int TipoTramiteId, Guid? Rol, int tramiteId, int nivelId)
        {
            throw new System.NotImplementedException();
        }

        public void ActualizarInstanciaProyectosTramiteNegocio(ProyectoTramiteDto DatosInstanciasProyecto, string usuario)
        {
            throw new System.NotImplementedException();
        }

        protected void GuardadoDefinitivo(ParametrosGuardarDto<DatosTramiteProyectosDto> parametrosGuardar, string usuario)
        {

        }

        protected DatosTramiteProyectosDto ObtenerDefinitivo(ParametrosConsultaDto parametrosConsultaDto)
        {
            throw new System.NotImplementedException();
        }

        public TramitesResultado EliminarProyectoTramiteNegocio(int TramiteId, int ProyectoId)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<JustificacionTramiteProyectoDto> ObtenerPreguntasJustificacion(int TramiteId, int ProyectoId, int TipoTramiteId, int TipoRolId, Guid IdNivel)
        {
            throw new System.NotImplementedException();
        }

        public TramitesResultado GuardarRespuestasJustificacion(List<JustificacionTramiteProyectoDto> justificacionTramiteProyectoDto, string usuario)
        {
            throw new System.NotImplementedException();
        }

        public TramitesResultado ActualizarValoresProyectosTramiteNegocio(ProyectoTramiteDto DatosInstanciasProyecto, string usuario)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<FuentePresupuestalDto> ObtenerFuentesInformacionPresupuestal()
        {
            throw new System.NotImplementedException();
        }
        public IEnumerable<ProyectoFuentePresupuestalDto> ObtenerProyectoFuentePresupuestalPorTramite(int pProyectoId, int? pTramiteId, string pTipoProyecto)
        {
            throw new System.NotImplementedException();
        }
        public IEnumerable<ProyectoRequisitoDto> ObtenerProyectoRequisitosPorTramite(int pProyectoId, int? pTramiteId, bool isCDP)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<TipoRequisitoDto> ObtenerTiposRequisito()
        {
            throw new System.NotImplementedException();
        }

        public TramitesResultado ValidarEnviarDatosTramiteNegocio(DatosTramiteProyectosDto datosTramiteProyectosDto, string usuario)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<JustificacionTematicaDto> ObtenerPreguntasProyectoActualizacion(int TramiteId, int ProyectoId, int TipoTramiteId, Guid IdNivel, int TipoRolId)
        {
            throw new System.NotImplementedException();
        }


        public IEnumerable<ProyectosEnTramiteDto> ObtenerProyectosTramiteNegocioAprobacion(int TramiteId, int TipoRolId)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<FuentesTramiteProyectoAprobacionDto> ObtenerFuentesTramiteProyectoAprobacion(int tramiteId, int proyectoId, string pTipoProyecto)
        {
            throw new System.NotImplementedException();
        }

        public TramitesResultado GuardarFuentesTramiteProyectoAprobacion(List<FuentesTramiteProyectoAprobacionDto> fuentesTramiteProyectoAprobacion, string usuario)
        {
            throw new System.NotImplementedException();
        }

        public CodigoPresupuestalDto ObtenerCodigoPresupuestal(int proyectoId, int entidadId, int tramiteId, string usuario)
        {
            throw new System.NotImplementedException();
        }

        public TramitesResultado ActualizarCodigoPresupuestal(int proyectoId, int entidadId, int tramiteId, string usuario)
        {
            throw new System.NotImplementedException();
        }

        public bool CrearAlcanceTramite(AlcanceTramiteDto data)
        {
            throw new System.NotImplementedException();
        }


        public ResponseDto<EnvioSubDireccionDto> GuardarSolicitarConcepto(EnvioSubDireccionDto concepto)
        {
            ResponseDto<EnvioSubDireccionDto> respuesta = new ResponseDto<EnvioSubDireccionDto>();
            respuesta.Estado = true;
            respuesta.Mensaje = "Exitoso";
            respuesta.Data = concepto;
            return respuesta;

        }

        public List<EnvioSubDireccionDto> ObtenerSolicitarConcepto(int tramiteid)
        {
            throw new System.NotImplementedException();
        }

        public List<TramitesProyectosDto> ObtenerTarmitesPorProyectoEntidad(int proyectoId, int entidadId, string usuario)
        {
            throw new System.NotImplementedException();
        }

        public TramitesValoresProyectoDto ObtenerValoresProyectos(int proyectoId, int tramiteId, int entidadId, string usuario)
        {
            throw new System.NotImplementedException();
        }

        public List<ConceptoDireccionTecnicaTramite> ObtenerConceptoDireccionTecnicaTramite(int tramiteId, Guid nivelid, string usuario)
        {
            throw new System.NotImplementedException();
        }

        public TramitesResultado GuardarConceptoDireccionTecnicaTramite(List<ConceptoDireccionTecnicaTramite> lConceptoDireccionTecnicaTramite, string usuario)
        {
            throw new System.NotImplementedException();
        }

        public TramitesResultado ValidarEnviarDatosTramiteNegocioAprobacion(DatosTramiteProyectosDto datosTramiteProyectosDto,
                                                                string usuario)
        {
            throw new System.NotImplementedException();
        }

        public PlantillaCarta ObtenerPlantillaCarta(string nombreSeccion, int tipoTramite)
        {
            throw new System.NotImplementedException();
        }

        public List<Carta> ObtenerDatosCartaPorSeccion(int tramiteId, int plantillaSeccionId)
        {
            throw new System.NotImplementedException();
        }

        public List<Carta> ObtenerDatosCartaPorSeccionDespedia(int plantillaSeccionId, int tramiteId)
        {
            throw new System.NotImplementedException();
        }

        public UsuarioTramite VerificaUsuarioDestinatario(UsuarioTramite usuarioTramite)
        {
            throw new System.NotImplementedException();
        }

        public TramitesResultado ActualizarCartaDatosIniciales(Carta datosIniciales, string usuario)
        {
            throw new System.NotImplementedException();
        }

        public TramitesResultado ActualizarCartaDatosDespedida(Carta datosDespedida, string usuario)
        {
            throw new System.NotImplementedException();
        }

        public List<UsuarioTramite> ObtenerUsuariosRegistrados(int tramiteId, string numeroTramite)
        {
            throw new System.NotImplementedException();
        }

        public TramitesResultado CargarFirma(FileToUploadDto parametros)
        {
            throw new System.NotImplementedException();
        }

        public TramitesResultado ValidarSiExisteFirmaUsuario(string idUsuario)
        {
            throw new System.NotImplementedException();
        }

        public TramitesResultado Firmar(int tramiteId, string radicadoSalida, string usuario)
        {
            throw new System.NotImplementedException();
        }

        public List<CuerpoConceptoCDP> ObtenerDatosCartaConceptoCuerpoCDP(int tramiteId)
        {
            throw new System.NotImplementedException();
        }
        public List<CuerpoConceptoAutorizacion> ObtenerDatosCartaConceptoCuerpoAutorizacion(int tramiteId)
        {
            throw new System.NotImplementedException();
        }

        public Carta ConsultarCarta(int tramiteid)
        {
            throw new System.NotImplementedException();
        }

        public TramitesResultado ActualizaEstadoAjusteProyecto(string tipoDevolucion, string objetoNegocioId, int tramiteId, string observacion, string usuario)
        {
            throw new System.NotImplementedException();
        }

        public int TramiteEnPasoUno(Guid InstanciaId)
        {
            throw new System.NotImplementedException();
        }

        public ResponseDto<List<TramiteConpesDto>> ObtenerConpesTramite(int tramiteId)
        {
            throw new System.NotImplementedException();
        }

        public ResponseDto<bool> GuardarConpesTramite(AsociarTramiteConpesRequestDto model, string usuario)
        {
            throw new System.NotImplementedException();
        }

        public ResponseDto<bool> RemoverConpesTramite(RemoverAsociacionConpesTramiteDto model)
        {
            throw new System.NotImplementedException();
        }

        public ResponseDto<PeriodoPresidencialDto> ObtenerPeriodoPresidencialActual()
        {
            throw new System.NotImplementedException();
        }

        public string EliminarAsociacionVFO(EliminacionAsociacionDto eliminacionAsociacionDto)
        {
            throw new System.NotImplementedException();
        }

        public List<proyectoAsociarTramite> ObtenerProyectoAsociarTramite(string bpin, int tramiteId)
        {
            throw new System.NotImplementedException();
        }

        public string AsociarProyectoVFO(proyectoAsociarTramite proyectoDto, string usuario)
        {
            throw new System.NotImplementedException();
        }

        public DatosProyectoTramiteDto ObtenerDatosProyectoTramite(int tramiteId)
        {
            throw new System.NotImplementedException();
        }

        public List<DatosProyectoTramiteDto> ObtenerDatosProyectosPorTramite(int tramiteId)
        {
            throw new System.NotImplementedException();
        }
        #region Vigencias Futuras

        public InformacionPresupuestalVlrConstanteDto ObtenerInformacionPresupuestalVlrConstanteVF(int tramiteId)
        {
            if (tramiteId == 25)
            {
                return new InformacionPresupuestalVlrConstanteDto()
                {
                    ProyectoId = 67056,
                    BPIN = "2017011000451",
                    Vigencias = new List<InformacionPresupuestalVigencia> {
                        new InformacionPresupuestalVigencia()
                        {
                           AnoBase = 2018,
                           Valores = new List<InformacionPresupuestalVigenciaValor>()
                        }
                    },
                    ObjetivosEspecificos = new List<InformacionPresupuestalObjetivo> {
                        new InformacionPresupuestalObjetivo()
                        {
                           ObjetivoEspecificoId = 11,
                           ObjetivoEspecifico = "Optimizar los procesos y servicios de información institucional.",
                           Productos = new List<InformacionPresupuestalProducto>{
                                new InformacionPresupuestalProducto()
                                {
                                    ProductoId = 6,
                                    Producto = "Servicios de información para la gestión administrativa - ",
                                    TotalValoresConstantes = 0,
                                    TotalValoresCorrientes = 0,
                                    Vigencias = new List<InformacionPresupuestalObjetivoVigencia>()
                                }
                           }
                        }
                    }
                };
            }

            return null;
        }

        public string ObtenerDatosCronograma(Guid instanciaId)
        {
            {
                var instanciaParam = new Guid("3E0750D4-DC36-4546-942F-72F8638B3E0A");

                if (instanciaId == instanciaParam)
                {
                    string JSONresult = "{\"InstanciaId\":\"3E0750D4-DC36-4546-942F-72F8638B3E0A\",\"Actividades\":[{\"ActividadesCronogramaId\":null,\"TramiteProyectoId\":null,\"NombreActividad\":null,\"ActividadesPreContractualesId\":null,\"FechaInicial\":null,\"FechaFinal\":null,\"FechaInicialmy\":null,\"FechaFinalmy\":null}]}";
                    return (JSONresult);
                }
                return (string.Empty);
            }
        }

        public IEnumerable<JustificacionPasoDto> ObtenerPreguntasProyectoActualizacionPaso(int TramiteId, int ProyectoId, int TipoTramiteId, Guid IdNivel, int TipoRolId)
        {
            if (TramiteId == 453)
            {
                return new List<JustificacionPasoDto>() {
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
                };

            }

            return new List<JustificacionPasoDto>();
        }

        public InformacionPresupuestalValoresDto ObtenerInformacionPresupuestalValores(int tramiteId)
        {
            if (tramiteId == 26)
            {
                return new InformacionPresupuestalValoresDto()
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
                };
            }

            return null;
        }

        public string GuardarInformacionPresupuestalValores(InformacionPresupuestalValoresDto informacionPresupuestalValoresDto, string usuario)
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
                    throw new ServiciosNegocioException(mensajeError);
                }

            }
            catch (ServiciosNegocioException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #endregion Vigencias Futuras

        public List<EnvioSubDireccionDto> ObtenerSolicitarConceptoPorTramite(int tramiteId)
        {
            List<EnvioSubDireccionDto> lista = new List<EnvioSubDireccionDto>();
            EnvioSubDireccionDto concepto = new EnvioSubDireccionDto();
            concepto.TramiteId = 573;
            concepto.IdUsuarioDNP = "auracanuta@gmail.com";
            concepto.NombreUsuarioDNP = "Aura Carolina Fernández";
            concepto.Enviado = true;
            concepto.NombreEntidad = "SDG - Subdirección de Crédito";
            lista.Add(concepto);



            return lista;
        }

        public DetalleCartaConceptoDto ObtenerDetalleCartaConcepto(int tramiteId)
        {
            throw new NotImplementedException();
        }

        List<TramiteDeflactoresDto> ITramitesProyectosServicio.GetTramiteDeflactores()
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
                throw new ServiciosNegocioException(ServiciosNegocioRecursos.ErrorMapear, e);
            }
        }

        List<Dominio.Dto.Tramites.TramiteProyectoDto> ITramitesProyectosServicio.GetProyectoTramite(int ProyectoId, int TramiteId)
        {
            try
            {
                var proyectoTramite = new List<Dominio.Dto.Tramites.TramiteProyectoDto>();
                string jsonString = "[{'Id':739,'TramiteId':356,'ProyectoId':97706,'EntidadId':186,'PeriodoProyectoId':1315,'Accion':'D','Estado':true,'TipoProyecto':'Credito','NombreProyecto':'Construcción Ampliación de Infraestructura para Generación de Cupos en Los Establecimientos de Reclusión del Orden - Nacional Programa: 1206\n\rSubprograma: 0802','EsConstante':true,'Constante':1,'AnioBase':2015}]";

                proyectoTramite = JsonConvert.DeserializeObject<List<Dominio.Dto.Tramites.TramiteProyectoDto>>(jsonString);

                return proyectoTramite;
            }
            catch (Exception e)
            {
                throw new ServiciosNegocioException(ServiciosNegocioRecursos.ErrorMapear, e);
            }
        }

        string ITramitesProyectosServicio.ActualizaVigenciaFuturaProyectoTramite(Dominio.Dto.Tramites.TramiteProyectoDto tramiteProyectoDto, string usuario)
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
                    throw new ServiciosNegocioException(mensajeError);
                }

            }
            catch (ServiciosNegocioException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        VigenciaFuturaCorrienteDto ITramitesProyectosServicio.GetFuentesFinanciacionVigenciaFuturaCorriente(string bpin)
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

        public AccionDto ObtenerAccionActualyFinal(int tramiteId, string bpin)
        {
            AccionDto ac = new AccionDto();
            ac.IdAccionActual = new Guid();
            ac.IdAccionFinal = new Guid();
            ac.IdFlujo = new Guid();
            ac.NombreAccionActual = "Inicio";
            ac.NombreAccionFinal = "Final";
            return ac;
        }
        public int EliminarPermisosAccionesUsuarios(string usuarioDestino, int tramiteId, string aliasNivel, Guid InstanciaId = default(Guid))
        {
            return 1;
        }


        VigenciaFuturaResponse ITramitesProyectosServicio.ActualizarVigenciaFuturaFuente(VigenciaFuturaCorrienteFuenteDto fuente, string usuario)
        {
            VigenciaFuturaResponse response = new VigenciaFuturaResponse();
            response.Exito = true;
            response.Mensaje = "OK";

            return response;
        }

        VigenciaFuturaConstanteDto ITramitesProyectosServicio.GetFuentesFinanciacionVigenciaFuturaCoonstante(string Bpin, int TramiteId)
        {
            try
            {
                var proyectoTramite = new VigenciaFuturaConstanteDto();
                string jsonString = "{'ProyectoId':97857,'BPIN':'202200000000060','AñoInicio':2022,'AñoFin':2030,'ValorTotalVigenteFutura':508036951196.0000,'ValorTotalVigente':318298000000.0000,'Porcentaje':76205542679.400000,'Fuentes':[{'EtapaId':2,'Etapa':'Inversión','FuenteId':1724,'Fuente':'MINISTERIO DE JUSTICIA Y DEL DERECHO - GESTIÓN GENERAL','TipoEntidad':'Entidades Presupuesto Nacional - PGN','EntidadId':1,'Entidad':'PGN','TipoRecursoId':1,'TipoRecurso':'Nación','ApropiacionVigente':318298000000.0000,'Vigencias':[{'PeriodoProyectoId':0,'Vigencia':2022,'Deflactor':0.000000000000000e+000,'ValorVigenteFutura':0.0000,'ValorVigenteFuturaCorriente':0.0000},{'PeriodoProyectoId':0,'Vigencia':2023,'Deflactor':0.000000000000000e+000,'ValorVigenteFutura':0.0000,'ValorVigenteFuturaCorriente':0.0000},{'PeriodoProyectoId':0,'Vigencia':2024,'Deflactor':0.000000000000000e+000,'ValorVigenteFutura':0.0000,'ValorVigenteFuturaCorriente':0.0000},{'PeriodoProyectoId':0,'Vigencia':2025,'Deflactor':0.000000000000000e+000,'ValorVigenteFutura':0.0000,'ValorVigenteFuturaCorriente':0.0000},{'PeriodoProyectoId':0,'Vigencia':2026,'Deflactor':0.000000000000000e+000,'ValorVigenteFutura':0.0000,'ValorVigenteFuturaCorriente':0.0000},{'PeriodoProyectoId':0,'Vigencia':2027,'Deflactor':0.000000000000000e+000,'ValorVigenteFutura':0.0000,'ValorVigenteFuturaCorriente':0.0000},{'PeriodoProyectoId':0,'Vigencia':2028,'Deflactor':0.000000000000000e+000,'ValorVigenteFutura':0.0000,'ValorVigenteFuturaCorriente':0.0000},{'PeriodoProyectoId':0,'Vigencia':2029,'Deflactor':0.000000000000000e+000,'ValorVigenteFutura':0.0000,'ValorVigenteFuturaCorriente':0.0000},{'PeriodoProyectoId':0,'Vigencia':2030,'Deflactor':0.000000000000000e+000,'ValorVigenteFutura':0.0000,'ValorVigenteFuturaCorriente':0.0000}]}]}";//Contexto.FuentesFinanciacionVigenciaFuturaCorriente_JSON(Bpin).SingleOrDefault();
                proyectoTramite = JsonConvert.DeserializeObject<VigenciaFuturaConstanteDto>(jsonString);
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

        public TramitesResultado EnviarConceptoDireccionTecnicaTramite(int tramiteId, string usuario)
        {
            var resultado = new TramitesResultado();
            try
            {

                resultado.Exito = true;
                resultado.Mensaje = "Solicitud concepto enviada con exito";

                return resultado;
            }
            catch (Exception e)
            {
                throw;
            }
        }
        List<TramiteModalidadContratacionDto> ITramitesProyectosServicio.ObtenerModalidadesContratacion(int? mostrar)
        {
            List<TramiteModalidadContratacionDto> lista = new List<TramiteModalidadContratacionDto>();
            lista.Add(new TramiteModalidadContratacionDto() { Id = 1, Nombre = "Contratación Directa" });
            lista.Add(new TramiteModalidadContratacionDto() { Id = 2, Nombre = "Licitación Publica" });
            lista.Add(new TramiteModalidadContratacionDto() { Id = 3, Nombre = "Concurso de méritos" });
            lista.Add(new TramiteModalidadContratacionDto() { Id = 4, Nombre = "Mínima Cuantía" });
            lista.Add(new TramiteModalidadContratacionDto() { Id = 5, Nombre = "Selección Abreviada" });
            return lista;

        }

        ActividadPreContractualDto ITramitesProyectosServicio.ActualizarActividadesCronograma(ActividadPreContractualDto ModalidadContratacionId, string usuario)
        {
            try
            {
                ActividadPreContractualDto resultado = new ActividadPreContractualDto();
                var result = string.Empty;//consulta de base de datos


                if (string.IsNullOrEmpty(result))
                {
                    return resultado;
                }
                else
                {
                    var mensajeError = Convert.ToString(result);
                    throw new ServiciosNegocioException(mensajeError);
                }

            }
            catch (ServiciosNegocioException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        ActividadPreContractualDto ITramitesProyectosServicio.ObtenerActividadesPrecontractualesProyectoTramite(int ModalidadContratacionId, int ProyectoId, int TramiteId, bool eliminarActividades)
        {
            var jsonString = "{'ProyectoId':97861,'ActividadesPreContractuales':[{'ActividadPreContractualId':1,'Actividad':'Actividad precontractual de prueba 1','CronogramaId':4,'ModalidadContratacionId':1,'FechaInicial':'2022 - 06 - 04','FechaFinal':'2022 - 06 - 03'},{'ActividadPreContractualId':2,'Actividad':'Actividad precontractual de prueba 2','CronogramaId':5,'ModalidadContratacionId':1,'FechaInicial':'2022 - 05 - 01','FechaFinal':'2020 - 05 - 27'},{'ActividadPreContractualId':4,'Actividad':'Actividad precontractual de prueba 3','CronogramaId':19,'ModalidadContratacionId':1,'FechaInicial':'2022 - 06 - 03','FechaFinal':'2022 - 06 - 29'},{'ActividadPreContractualId':4,'Actividad':'Actividad precontractual de prueba 3','CronogramaId':20,'ModalidadContratacionId':1,'FechaInicial':'2022 - 06 - 02','FechaFinal':'2022 - 01 - 01'},{'ActividadPreContractualId':4,'Actividad':'Actividad precontractual de prueba 3','CronogramaId':21,'ModalidadContratacionId':1,'FechaInicial':'2022 - 01 - 01','FechaFinal':'2022 - 06 - 03'},{'ActividadPreContractualId':4,'Actividad':'Actividad precontractual de prueba 3','CronogramaId':22,'ModalidadContratacionId':1,'FechaInicial':'2022 - 06 - 03','FechaFinal':'2022 - 06 - 22'},{'ActividadPreContractualId':4,'Actividad':'Actividad precontractual de prueba 3','CronogramaId':23,'ModalidadContratacionId':1,'FechaInicial':'2022 - 06 - 02','FechaFinal':'2022 - 07 - 06'}],'ActividadesContractuales':[{'ActividadPreContractualId':null,'Actividad':'Actividad contractual 1','CronogramaId':12,'ModalidadContratacionId':1,'FechaInicial':'2022 - 06 - 09','FechaFinal':'2022 - 05 - 02','TramiteProyectoId':1410},{'ActividadPreContractualId':null,'Actividad':'Actividad contractual2','CronogramaId':28,'ModalidadContratacionId':1,'FechaInicial':'2022 - 06 - 06','FechaFinal':'2022 - 06 - 08','TramiteProyectoId':1410}]}";

            var actividades = JsonConvert.DeserializeObject<ActividadPreContractualDto>(jsonString);

            return actividades;
        }

        public ProductosConstantesVF GetProductosVigenciaFuturaConstante(string Bpin, int TramiteId, int AnioBase)
        {
            ProductosConstantesVF producto = new ProductosConstantesVF();

            return producto;
        }

        public VigenciaFuturaResponse ActualizarVigenciaFuturaProducto(ProductosConstantesVFDetalleProducto prod, string usuario)
        {
            VigenciaFuturaResponse response = new VigenciaFuturaResponse();
            return response;
        }

        public ProductosCorrientesVF GetProductosVigenciaFuturaCorriente(string Bpin, int TramiteId)
        {
            var jsonString = "{'ProyectoId':97750,'BPIN':'202200000000002','ResumenObjetivos':[{'ObjetivoEspecificoid':776,'ObjetivoEspecifico':'Aumentar el número de cupos penitenciarios y carcelarios para atender a la PPL','Productos':[{'ProductoId':1332,'Producto':'Infraestructura penitenciaria y carcelaria construida','Vigencias':[{'PeriodoProyectoId':1351,'Vigencia':2022,'ValorVigenteFutura':502300000000.0000},{'PeriodoProyectoId':1352,'Vigencia':2023,'ValorVigenteFutura':0.0000},{'PeriodoProyectoId':1353,'Vigencia':2024,'ValorVigenteFutura':0.0000},{'PeriodoProyectoId':1354,'Vigencia':2025,'ValorVigenteFutura':0.0000}]}]},{'ObjetivoEspecificoid':777,'ObjetivoEspecifico':'Obtener información técnica eficiente para dar viabilidad a nuevos proyectos de construcción y ampliación de cupos','Productos':[{'ProductoId':1331,'Producto':'Servicio de información penitenciaria y carcelaria para la toma de decisiones','Vigencias':[{'PeriodoProyectoId':1351,'Vigencia':2022,'ValorVigenteFutura':432000000000.0000},{'PeriodoProyectoId':1352,'Vigencia':2023,'ValorVigenteFutura':0.0000},{'PeriodoProyectoId':1353,'Vigencia':2024,'ValorVigenteFutura':0.0000},{'PeriodoProyectoId':1354,'Vigencia':2025,'ValorVigenteFutura':0.0000}]}]}],'DetalleObjetivos':[{'ObjetivoEspecificoid':776,'ObjetivoEspecifico':'Aumentar el número de cupos penitenciarios y carcelarios para atender a la PPL','Productos':[{'ProductoId':1332,'Producto':'Infraestructura penitenciaria y carcelaria construida','Vigencias':[{'PeriodoProyectoId':1351,'Vigencia':2022,'ValorSolicitado':502300000000.0000,'Decreto':0,'ValorVigente':502300000000.0000,'VigenteFuturasAnteriores':0,'TotalVigenciasFuturaSolicitada':0},{'PeriodoProyectoId':1352,'Vigencia':2023,'ValorSolicitado':0.0000,'Decreto':0,'ValorVigente':0.0000,'VigenteFuturasAnteriores':0,'TotalVigenciasFuturaSolicitada':0},{'PeriodoProyectoId':1353,'Vigencia':2024,'ValorSolicitado':0.0000,'Decreto':0,'ValorVigente':0.0000,'VigenteFuturasAnteriores':0,'TotalVigenciasFuturaSolicitada':0},{'PeriodoProyectoId':1354,'Vigencia':2025,'ValorSolicitado':0.0000,'Decreto':0,'ValorVigente':0.0000,'VigenteFuturasAnteriores':0,'TotalVigenciasFuturaSolicitada':0}]}]},{'ObjetivoEspecificoid':777,'ObjetivoEspecifico':'Obtener información técnica eficiente para dar viabilidad a nuevos proyectos de construcción y ampliación de cupos','Productos':[{'ProductoId':1331,'Producto':'Servicio de información penitenciaria y carcelaria para la toma de decisiones','Vigencias':[{'PeriodoProyectoId':1351,'Vigencia':2022,'ValorSolicitado':432000000000.0000,'Decreto':0,'ValorVigente':432000000000.0000,'VigenteFuturasAnteriores':0,'TotalVigenciasFuturaSolicitada':0},{'PeriodoProyectoId':1352,'Vigencia':2023,'ValorSolicitado':0.0000,'Decreto':0,'ValorVigente':0.0000,'VigenteFuturasAnteriores':0,'TotalVigenciasFuturaSolicitada':0},{'PeriodoProyectoId':1353,'Vigencia':2024,'ValorSolicitado':0.0000,'Decreto':0,'ValorVigente':0.0000,'VigenteFuturasAnteriores':0,'TotalVigenciasFuturaSolicitada':0},{'PeriodoProyectoId':1354,'Vigencia':2025,'ValorSolicitado':0.0000,'Decreto':0,'ValorVigente':0.0000,'VigenteFuturasAnteriores':0,'TotalVigenciasFuturaSolicitada':0}]}]}]}";

            var actividades = JsonConvert.DeserializeObject<ProductosCorrientesVF>(jsonString);

            return actividades;
        }

        public IEnumerable<TipoDocumentoTramiteDto> ObtenerTipoDocumentoTramitePorNivel(int tipoTramiteId, string nivelId, string rolId)
        {
            var jsonString = "[{'Id':1, 'TipoDocumentoId': 1, 'TipoDocumento': 'Análisis Ecónomico (Supuestos de Costeo)' ,'TipoTramiteId': 2,'Obligatorio':true}]";

            var TipoDocumentos = JsonConvert.DeserializeObject<List<TipoDocumentoTramiteDto>>(jsonString);

            return TipoDocumentos;
        }

        public IEnumerable<DatosUsuarioDto> ObtenerDatosUsuario(string idUsuarioDnp, int idEntidad, Guid idAccion, Guid idIntancia)
        {
            var jsonString = "[{'IdUsuario':0E935E9E-E124-4B29-BB82-00BD1C6F8F1F', 'NombreUsuario': 'Juan3 Perez'," +
                " 'Cuenta': 'jeruiz' ,'EntidadId': '37C799DD-B3F4-4B0D-9A50-840EA2E9EB0C','RolId':'DA595AA3-CF59-46D3-A22A-0D96DA5C7371'}]" +
                " 'Entidad': 'RAMA JUDICIAL - TRIBUNALES Y JUZGADOS'";

            var datosUsuarioDto = JsonConvert.DeserializeObject<List<DatosUsuarioDto>>(jsonString);

            return datosUsuarioDto;

        }

        public List<proyectoAsociarTramite> ObtenerProyectoAsociarTramiteLeyenda(string bpin, int tramiteId)
        {
            List<proyectoAsociarTramite> lista = new List<proyectoAsociarTramite>();
            if (bpin != null && tramiteId != 0)
            {
                lista.Add(new proyectoAsociarTramite() { EntidadId = 1, NombreProyecto = "Proyecto 1", Accion = "N", BPIN = "20220000001", PeriodoProyectoId = 1, ProyectoId = 99991, TipoProyecto = "Aclaracion Leyenda", TramiteId = 112 });
                lista.Add(new proyectoAsociarTramite() { EntidadId = 2, NombreProyecto = "Proyecto 2", Accion = "N", BPIN = "20220000002", PeriodoProyectoId = 2, ProyectoId = 99992, TipoProyecto = "Aclaracion Leyenda", TramiteId = 112 });
                lista.Add(new proyectoAsociarTramite() { EntidadId = 3, NombreProyecto = "Proyecto 3", Accion = "N", BPIN = "20220000003", PeriodoProyectoId = 3, ProyectoId = 99993, TipoProyecto = "Aclaracion Leyenda", TramiteId = 112 });
                lista.Add(new proyectoAsociarTramite() { EntidadId = 4, NombreProyecto = "Proyecto 4", Accion = "N", BPIN = "20220000004", PeriodoProyectoId = 4, ProyectoId = 99994, TipoProyecto = "Aclaracion Leyenda", TramiteId = 112 });
                lista.Add(new proyectoAsociarTramite() { EntidadId = 5, NombreProyecto = "Proyecto 5", Accion = "N", BPIN = "20220000005", PeriodoProyectoId = 5, ProyectoId = 99995, TipoProyecto = "Aclaracion Leyenda", TramiteId = 112 });
            }
            else
                lista = null;
            return lista;
        }

        public ModificacionLeyendaDto ObtenerModificacionLeyenda(int tramiteId, int ProyectoId)
        {
            ModificacionLeyendaDto resultado = new ModificacionLeyendaDto();
            if (tramiteId != 0 && ProyectoId != 0)
            {
                resultado.Id = 1; resultado.NombreProyecto = "proyecto"; resultado.Programa = "1010"; resultado.Subprograma = "0008";
                resultado.BPIN = "20220000000010"; resultado.ErrorAritmetico = false; resultado.ErrorTranscripcion = true;
            }
            else
                resultado = null;

            return resultado;
        }

        public string ActualizaModificacionLeyenda(ModificacionLeyendaDto modificacionLeyendaDto, string usuario)
        {
            string resultado;
            if (string.IsNullOrEmpty(modificacionLeyendaDto.TramiteProyectoId.ToString()))            
                resultado = "OK";
            else            
                resultado = "Ocurrio un error al insertar o modificar el registro";
            
            return resultado;
        }

        public List<EntidadCatalogoDTDto> ObtenerListaDireccionesDNP(Guid idEntididad)
        {
            var jsonString = "[{'Id':15', 'Name': 'Prueba catalogo' }]";

            var datosCatalogoDto = JsonConvert.DeserializeObject<List<EntidadCatalogoDTDto>>(jsonString);
            return (datosCatalogoDto);
        }
       
        public List<EntidadCatalogoDTDto> ObtenerListaSubdireccionesPorParentId(int idEntididadType)
        {
            var jsonString = "[{'Id':15', 'Name': 'Prueba catalogo' }]";

            var datosCatalogoDto = JsonConvert.DeserializeObject<List<EntidadCatalogoDTDto>>(jsonString);
            return datosCatalogoDto;
        }

        public TramitesResultado BorrarFirma(FileToUploadDto parametros)
        {
            TramitesResultado resuelto = new TramitesResultado();
            var jsonString = "[{'Exito':true', 'Mensaje': 'Respuesta del proceso' }]";

            var datosCatalogoDto = JsonConvert.DeserializeObject<TramitesResultado>(jsonString);
            return datosCatalogoDto;
        }

        public ProyectosCartaDto ObtenerProyectosCartaTramite(int tramiteId)
        {
            ProyectosCartaDto resultado = new ProyectosCartaDto();
            if (tramiteId != 0)
            {
                resultado = new ProyectosCartaDto
                {
                    Bpin = "20220000000012",
                    CodigoEntidad = "1258",
                    CodigoPrograma = "1234",
                    CodigoSubprograma = "4321",
                    ConsecutivoCodigoPresupuestal = 12,
                    Entidad = "MINISTERIO DEL DEPORTE",
                    NombreProyecto = "PROYECTO UNO",
                    Programa = "7895",
                    Subprogramal = "9632",
                    esConstante = null
                };
            }
            else
                resultado = null;

            return resultado;
        }

        public DetalleCartaConceptoALDto ObtenerDetalleCartaAL(int tramiteId)
        {
            DetalleCartaConceptoALDto resultado = new DetalleCartaConceptoALDto();
            if (tramiteId != 0)
            {
                resultado.Aclaracion = "PROYECTO 1 NUEVO"; resultado.NombreActual = "PROYECTO 1"; 
            }
            else
                resultado = null;

            return resultado;
        }

        public int ObtenerAmpliarDevolucionTramite(int ProyectoId, int TramiteId)
        {
            return 1;
        }

        public DatosProyectoTramiteDto ObtenerDatosProyectoConceptoPorInstancia(Guid instanciaId)
        {
            DatosProyectoTramiteDto resultado = new DatosProyectoTramiteDto();
            if (instanciaId ==  new Guid())
            {
                resultado = new DatosProyectoTramiteDto
                {
                    Sector = "Justicia",
                    EntidadDestino = "DNP",
                    BPIN = "20220000000012",
                    NombreProyecto = "Pruebas",
                    ProyectoId = 97738,
                    EstadoProyecto = "En ejecución",
                    MacroProceso = "Ejecucion",
                    FechaInicioProceso = DateTime.Now,
                    EstadoProceso = "Abierto",
                    NombrePaso = "Aprobación",
                    FechaInicioPaso = DateTime.Now
                };
            }
            else
                resultado = null;

            return resultado;
        }

        public List<TramiteLiberacionVfDto> ObtenerLiberacionVigenciasFuturas(int ProyectoId, int TramiteId)
        {
            var jsonString = "[{'CodigoProceso':'EJ-TP-TL-171800-0001', 'NombreProceso': 'Tramite de Traslado', 'Fecha': '2022-08-10 10:19:04.000', 'Objeto': 'Tramite de Traslado', 'FechaAutorizacion': null, 'CodigoAutorizacion': null }]";

            var liberaciones = JsonConvert.DeserializeObject<List<TramiteLiberacionVfDto>>(jsonString);
            return liberaciones;
        }

        public VigenciaFuturaResponse InsertaAutorizacionVigenciasFuturas(TramiteALiberarVfDto autorizacion, string usuario)
        {
            VigenciaFuturaResponse response = new VigenciaFuturaResponse();
            response.Exito = true;
            response.Mensaje = "Exitoso";
            return response;
        }

        public VigenciaFuturaResponse InsertaValoresUtilizadosLiberacionVF(TramiteALiberarVfDto autorizacion, string usuario)
        {
            VigenciaFuturaResponse response = new VigenciaFuturaResponse();
            response.Exito = true;
            response.Mensaje = "Exitoso";
            return response;
        }

        public List<ProyectoTramiteFuenteDto> ObtenerListaProyectosFuentes(int tramiteId)
        {
            List<ProyectoTramiteFuenteDto> response = new List<ProyectoTramiteFuenteDto>();
            ProyectoTramiteFuenteDto proyecto = new ProyectoTramiteFuenteDto();
            proyecto.BPIN = "200001";
            proyecto.NombreProyecto = "Prueba 1";
            proyecto.ValorTotalNacion = 100;
            proyecto.ValorTotalPropios = 200;
            proyecto.Operacion = "Operacion 1";
            proyecto.Id = 1;
            proyecto.ListaFuentes = new List<FuenteFinanciacionDto>();
            FuenteFinanciacionDto fuente = new FuenteFinanciacionDto();
            fuente.FuenteId = 1;
            fuente.NombreCompleto = "Fuente 1";
            fuente.GrupoRecurso = "CSF";
            fuente.ApropiacionInicial = 1000;
            fuente.ApropiacionVigente = 2000;
            proyecto.ListaFuentes.Add(fuente);
            response.Add(proyecto);
            return response;
           
        }

        public List<EntidadesAsociarComunDto> ObtenerEntidadAsociarProyecto(Guid InstanciaId, string AccionTramiteProyecto)
        {
            if (InstanciaId == new Guid("") && AccionTramiteProyecto == null)
                return null;
            else
            {
                var jsonString = "[{'Id':'1', 'NombreEntidad': 'Mincultura' },{'Id':'2', 'NombreEntidad': 'Minjusticia' }]";

                return JsonConvert.DeserializeObject<List<EntidadesAsociarComunDto>>(jsonString);
            }
        }
        public CartaConcepto ConsultarCartaConcepto(int tramiteId)
        {
            var jsonString = "{'Id':'2', 'FaseId': '42', 'TramiteId': '902', 'RadicadoEntrada': '309097', 'RadicadoSalida': '3090976', 'FechaCreacion': '2022/08/25','CreadoPor': 'Pruebe','FechaModificacion': '2022/08/25','ModificadoPor': 'prueba','ModificadoPor': 'A3987676676' }";

            var datos = JsonConvert.DeserializeObject<CartaConcepto>(jsonString);
            return datos;
        }

        public int ValidacionPeriodoPresidencial(int tramiteId)
        {
            var jsonString = "1";

            var datos = JsonConvert.DeserializeObject<int>(jsonString);
            return datos;
        }

        public TramitesResultado GuardarMontosTramite(List<ProyectosEnTramiteDto> parametrosGuardar, string usuario)
        {
            var resultado = new TramitesResultado();
            try
            {

                resultado.Exito = true;
                resultado.Mensaje = "Guardado exitoso";

                return resultado;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public List<tramiteVFAsociarproyecto> ObtenerTramitesVFparaLiberar(int proyectoId)
        {
            List<tramiteVFAsociarproyecto> resultado = new List<tramiteVFAsociarproyecto>();
            if (proyectoId != 0)
            {
                var r = new tramiteVFAsociarproyecto
                {Id = 1, Descripcion = "proyecto", NumeroTramite = "1010", ObjContratacion = "Objeto", tipotramiteId = 16, fecha = DateTime.Now};
                resultado.Add(r);
            }
            else
                resultado = null;

            return resultado;
        }

        public string GuardarLiberacionVigenciaFutura(LiberacionVigenciasFuturasDto liberacionVigenciasFuturasDto, string usuario)
        {
            string resultado;
            if (liberacionVigenciasFuturasDto.tramiteProyectoId > 0)
                resultado = "OK";
            else
                resultado = "Ocurrio un error al insertar o modificar el registro";

            return resultado;
        }

        public IEnumerable<ProyectoJustificacioneDto> ObtenerPreguntasJustificacionPorProyectos(int TramiteId, int TipoTramiteId, int TipoRolId, Guid IdNivel)
        {
            if (TramiteId == 0)
                return null;
            List <ProyectoJustificacioneDto> lista = new List<ProyectoJustificacioneDto>();
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
            j.InstanciaId = new Guid();
            j.JustificacionId = 1254;
            j.JustificacionRespuesta = "Aprobado";
            j.NombreNivel = "Aprobacion analista";
            j.NombreRol = "Analista";
            dato.ListaJustificacionPaso[0].justificaciones.Add(j);
            lista.Add(dato);
            return lista;
        }

        public List<ResumenLiberacionVfDto> ObtenerResumenLiberacionVigenciasFuturas(int ProyectoId, int TramiteId)
        {
            string jsonString = "[{'TramiteId':906,'ProyectoId':97833,'TotalValoresUtilizados':4939.4700,'ValoresAutorizadosUtilizados':[{'Vigencia':2022,'AprobadosNacion':12854000000.0000,'AprobadosNPropios':10795600000.0000,'UtilizadoNacion':605.8400,'UtilizadoPropios':4333.6300},{'Vigencia':2023,'AprobadosNacion':12000000000.0000,'AprobadosNPropios':10000000000.0000,'UtilizadoNacion':0.0000,'UtilizadoPropios':0.0000},{'Vigencia':2024,'AprobadosNacion':11150000000.0000,'AprobadosNPropios':9210000000.0000,'UtilizadoNacion':0.0000,'UtilizadoPropios':0.0000},{'Vigencia':2025,'AprobadosNacion':11104000000.0000,'AprobadosNPropios':8425600000.0000,'UtilizadoNacion':0.0000,'UtilizadoPropios':0.0000}]}]";// Contexto.uspGetLiberacionVF(ProyectoId, TramiteId).SingleOrDefault();

            var liberaciones = JsonConvert.DeserializeObject<List<ResumenLiberacionVfDto>>(jsonString);
            return liberaciones;
        }

        public ValoresUtilizadosLiberacionVfDto ObtenerValUtilizadosLiberacionVigenciasFuturas(int ProyectoId, int TramiteId)
        {
            string jsonString = "{'TramiteId':906,'ProyectoId':97833,'ValoresUtilizadosGeneral':[{'CodigoProceso':'Proceso 1','FechaAutorizacion':'0001-01-01T00:00:00','CodigoAutorizacion':'010101','TotalValorSolicitado':30.0,'TotalTotalValorUtilizado':30.0,'TotalValorUtilizadoProducto':30.0,'ValoresUtilizadosCorrientesProceso':[{'EsConstante':false,'Vigencia':2022,'ValorSolicitado':10.0,'TotalValorUtilizado':10.0,'ValorUtilizadoProducto':10.0},{'EsConstante':false,'Vigencia':2023,'ValorSolicitado':15.0,'TotalValorUtilizado':15.0,'ValorUtilizadoProducto':15.0}],'ValoresUtilizadosConstantesProceso':[{'EsConstante':true,'Vigencia':2022,'AnioBase':2020,'Deflactor':67890.0,'ValorSolicitado':10.0,'TotalValorUtilizado':10.0,'ValorUtilizadoProducto':10.0},{'EsConstante':true,'Vigencia':2023,'AnioBase':0,'Deflactor':0.0,'ValorSolicitado':15.0,'TotalValorUtilizado':15.0,'ValorUtilizadoProducto':15.0}],'ValoresUtilizadosConstantesObjetivo':[{'Objetivo':'Objetivo 1','Productos':[{'NombreProducto':'Producto 1','Etapa':'Etapa1','FechaInicial':'0001-01-01T00:00:00','FechaFinal':'0001-01-01T00:00:00','VigenciasCorrientes':[{'EsConstante':false,'Vigencia':2022,'ValorSolicitado':10.0,'ValorAprobado':10.0,'ValorUtilizado':10.0},{'EsConstante':false,'Vigencia':2023,'ValorSolicitado':15.0,'ValorAprobado':15.0,'ValorUtilizado':15.0}],'VigenciasConstantes':[{'EsConstante':true,'Vigencia':2022,'AnioBase':2020,'Deflactor':5678.0,'ValorSolicitado':20.0,'ValorAprobado':20.0,'ValorUtilizado':20.0},{'EsConstante':true,'Vigencia':2023,'AnioBase':0,'Deflactor':0.0,'ValorSolicitado':25.0,'ValorAprobado':25.0,'ValorUtilizado':25.0}]},{'NombreProducto':'Producto 2','Etapa':'Etapa 2','FechaInicial':'0001-01-01T00:00:00','FechaFinal':'0001-01-01T00:00:00','VigenciasCorrientes':[{'EsConstante':false,'Vigencia':2022,'ValorSolicitado':20.0,'ValorAprobado':20.0,'ValorUtilizado':20.0},{'EsConstante':false,'Vigencia':2023,'ValorSolicitado':25.0,'ValorAprobado':25.0,'ValorUtilizado':25.0}],'VigenciasConstantes':[{'EsConstante':false,'Vigencia':0,'AnioBase':0,'Deflactor':0.0,'ValorSolicitado':0.0,'ValorAprobado':0.0,'ValorUtilizado':0.0},{'EsConstante':false,'Vigencia':0,'AnioBase':0,'Deflactor':0.0,'ValorSolicitado':0.0,'ValorAprobado':0.0,'ValorUtilizado':0.0}]}]},{'Objetivo':'Objetivo 2','Productos':[{'NombreProducto':'Producto 1','Etapa':'Etapa1','FechaInicial':'0001-01-01T00:00:00','FechaFinal':'0001-01-01T00:00:00','VigenciasCorrientes':[{'EsConstante':false,'Vigencia':2022,'ValorSolicitado':10.0,'ValorAprobado':10.0,'ValorUtilizado':10.0},{'EsConstante':false,'Vigencia':2023,'ValorSolicitado':15.0,'ValorAprobado':15.0,'ValorUtilizado':15.0}],'VigenciasConstantes':[{'EsConstante':true,'Vigencia':2022,'AnioBase':2020,'Deflactor':5678.0,'ValorSolicitado':20.0,'ValorAprobado':20.0,'ValorUtilizado':20.0},{'EsConstante':true,'Vigencia':2023,'AnioBase':0,'Deflactor':0.0,'ValorSolicitado':25.0,'ValorAprobado':25.0,'ValorUtilizado':25.0}]},{'NombreProducto':'Producto 2','Etapa':'Etapa 2','FechaInicial':'0001-01-01T00:00:00','FechaFinal':'0001-01-01T00:00:00','VigenciasCorrientes':[{'EsConstante':false,'Vigencia':2022,'ValorSolicitado':20.0,'ValorAprobado':20.0,'ValorUtilizado':20.0},{'EsConstante':false,'Vigencia':2023,'ValorSolicitado':25.0,'ValorAprobado':25.0,'ValorUtilizado':25.0}],'VigenciasConstantes':[{'EsConstante':false,'Vigencia':0,'AnioBase':0,'Deflactor':0.0,'ValorSolicitado':0.0,'ValorAprobado':0.0,'ValorUtilizado':0.0},{'EsConstante':false,'Vigencia':0,'AnioBase':0,'Deflactor':0.0,'ValorSolicitado':0.0,'ValorAprobado':0.0,'ValorUtilizado':0.0}]}]}]},{'CodigoProceso':'Proceso 2','FechaAutorizacion':'0001-01-01T00:00:00','CodigoAutorizacion':'020202','TotalValorSolicitado':30.0,'TotalTotalValorUtilizado':30.0,'TotalValorUtilizadoProducto':30.0,'ValoresUtilizadosCorrientesProceso':[{'EsConstante':false,'Vigencia':2022,'ValorSolicitado':10.0,'TotalValorUtilizado':10.0,'ValorUtilizadoProducto':10.0},{'EsConstante':false,'Vigencia':2023,'ValorSolicitado':15.0,'TotalValorUtilizado':15.0,'ValorUtilizadoProducto':15.0}],'ValoresUtilizadosConstantesProceso':[{'EsConstante':true,'Vigencia':2022,'AnioBase':2020,'Deflactor':67890.0,'ValorSolicitado':10.0,'TotalValorUtilizado':10.0,'ValorUtilizadoProducto':10.0},{'EsConstante':true,'Vigencia':2023,'AnioBase':0,'Deflactor':0.0,'ValorSolicitado':15.0,'TotalValorUtilizado':15.0,'ValorUtilizadoProducto':15.0}],'ValoresUtilizadosConstantesObjetivo':[{'Objetivo':'Objetivo 1','Productos':[{'NombreProducto':'Producto 1','Etapa':'Etapa1','FechaInicial':'0001-01-01T00:00:00','FechaFinal':'0001-01-01T00:00:00','VigenciasCorrientes':[{'EsConstante':false,'Vigencia':2022,'ValorSolicitado':10.0,'ValorAprobado':10.0,'ValorUtilizado':10.0},{'EsConstante':false,'Vigencia':2023,'ValorSolicitado':15.0,'ValorAprobado':15.0,'ValorUtilizado':15.0}],'VigenciasConstantes':[{'EsConstante':true,'Vigencia':2022,'AnioBase':2020,'Deflactor':5678.0,'ValorSolicitado':20.0,'ValorAprobado':20.0,'ValorUtilizado':20.0},{'EsConstante':true,'Vigencia':2023,'AnioBase':0,'Deflactor':0.0,'ValorSolicitado':25.0,'ValorAprobado':25.0,'ValorUtilizado':25.0}]},{'NombreProducto':'Producto 2','Etapa':'Etapa 2','FechaInicial':'0001-01-01T00:00:00','FechaFinal':'0001-01-01T00:00:00','VigenciasCorrientes':[{'EsConstante':false,'Vigencia':2022,'ValorSolicitado':20.0,'ValorAprobado':20.0,'ValorUtilizado':20.0},{'EsConstante':false,'Vigencia':2023,'ValorSolicitado':25.0,'ValorAprobado':25.0,'ValorUtilizado':25.0}],'VigenciasConstantes':[{'EsConstante':false,'Vigencia':0,'AnioBase':0,'Deflactor':0.0,'ValorSolicitado':0.0,'ValorAprobado':0.0,'ValorUtilizado':0.0},{'EsConstante':false,'Vigencia':0,'AnioBase':0,'Deflactor':0.0,'ValorSolicitado':0.0,'ValorAprobado':0.0,'ValorUtilizado':0.0}]}]},{'Objetivo':'Objetivo 2','Productos':[{'NombreProducto':'Producto 1','Etapa':'Etapa1','FechaInicial':'0001-01-01T00:00:00','FechaFinal':'0001-01-01T00:00:00','VigenciasCorrientes':[{'EsConstante':false,'Vigencia':2022,'ValorSolicitado':10.0,'ValorAprobado':10.0,'ValorUtilizado':10.0},{'EsConstante':false,'Vigencia':2023,'ValorSolicitado':15.0,'ValorAprobado':15.0,'ValorUtilizado':15.0}],'VigenciasConstantes':[{'EsConstante':true,'Vigencia':2022,'AnioBase':2020,'Deflactor':5678.0,'ValorSolicitado':20.0,'ValorAprobado':20.0,'ValorUtilizado':20.0},{'EsConstante':true,'Vigencia':2023,'AnioBase':0,'Deflactor':0.0,'ValorSolicitado':25.0,'ValorAprobado':25.0,'ValorUtilizado':25.0}]},{'NombreProducto':'Producto 2','Etapa':'Etapa 2','FechaInicial':'0001-01-01T00:00:00','FechaFinal':'0001-01-01T00:00:00','VigenciasCorrientes':[{'EsConstante':false,'Vigencia':2022,'ValorSolicitado':20.0,'ValorAprobado':20.0,'ValorUtilizado':20.0},{'EsConstante':false,'Vigencia':2023,'ValorSolicitado':25.0,'ValorAprobado':25.0,'ValorUtilizado':25.0}],'VigenciasConstantes':[{'EsConstante':false,'Vigencia':0,'AnioBase':0,'Deflactor':0.0,'ValorSolicitado':0.0,'ValorAprobado':0.0,'ValorUtilizado':0.0},{'EsConstante':false,'Vigencia':0,'AnioBase':0,'Deflactor':0.0,'ValorSolicitado':0.0,'ValorAprobado':0.0,'ValorUtilizado':0.0}]}]}]}]}";// Contexto.uspGetLiberacionVF(ProyectoId, TramiteId).SingleOrDefault();

            var liberaciones = JsonConvert.DeserializeObject<ValoresUtilizadosLiberacionVfDto>(jsonString);
            return liberaciones;
        }

        public int TramiteAjusteEnPasoUno(int tramiteId, int proyectoId)
        {
            return 1;
        }

        public List<ProyectoTramiteFuenteDto> ObtenerListaProyectosFuentesAprobado(int tramiteId)
        {
            if (tramiteId == 0)
                return null;
            List<ProyectoTramiteFuenteDto> response = new List<ProyectoTramiteFuenteDto>();
            ProyectoTramiteFuenteDto proyecto = new ProyectoTramiteFuenteDto();
            proyecto.BPIN = "200001";
            proyecto.NombreProyecto = "Prueba 1";
            proyecto.ValorTotalNacion = 100;
            proyecto.ValorTotalPropios = 200;
            proyecto.Operacion = "Operacion 1";
            proyecto.Id = 1;
            proyecto.ListaFuentes = new List<FuenteFinanciacionDto>();
            FuenteFinanciacionDto fuente = new FuenteFinanciacionDto();
            fuente.FuenteId = 1;
            fuente.NombreCompleto = "Fuente 1";
            fuente.GrupoRecurso = "CSF";
            fuente.ApropiacionInicial = 1000;
            fuente.ApropiacionVigente = 2000;
            proyecto.ListaFuentes.Add(fuente);
            response.Add(proyecto);
            return response;

        }

        public VigenciaFuturaResponse InsertaValoresproductosLiberacionVFCorrientes(DetalleProductosCorrientesDto productosCorrientes, string usuario)
        {
            VigenciaFuturaResponse response = new VigenciaFuturaResponse();
            response.Exito = true;
            response.Mensaje = "OK";

            return response;
        }

        public VigenciaFuturaResponse InsertaValoresproductosLiberacionVFConstantes(DetalleProductosConstantesDto productosConstantes, string usuario)
        {
            VigenciaFuturaResponse response = new VigenciaFuturaResponse();
            response.Exito = true;
            response.Mensaje = "OK";

            return response;
        }

        public List<EntidadesAsociarComunDto> ObtenerEntidadTramite(string numeroTramite)
        {
            List<EntidadesAsociarComunDto> rta = new List<EntidadesAsociarComunDto>();
            if (string.IsNullOrEmpty(numeroTramite))
                return null;
            else
            {
                EntidadesAsociarComunDto rta1 = new EntidadesAsociarComunDto();
                rta1.CabezaSector= true;
                rta1.Id = 186;
                rta1.NombreEntidad = "Prueba";
                rta.Add(rta1);
            }
            return rta;
        }

        public VigenciaFuturaResponse EliminaLiberacionVF(LiberacionVigenciasFuturasDto tramiteEliminar)
        {
            VigenciaFuturaResponse response = new VigenciaFuturaResponse();
            if (string.IsNullOrEmpty(tramiteEliminar.tramiteId.ToString()))
            {                
                response.Exito = true;
                response.Mensaje = "OK";
            }
            else
            {
                response.Exito = false;
                response.Mensaje = "Ocurrio un error al eliminar o modificar el registro";
            }

            return response;
        }

        public List<DatosUsuarioDto> ObtenerUsuariosPorInstanciaPadre(Guid InstanciaId)
        {
            List<DatosUsuarioDto> lista = new List<DatosUsuarioDto>();
            if (InstanciaId == new Guid("00000000-0000-0000-0000-000000000000"))
                return null;
            else
            {
                DatosUsuarioDto rta1 = new DatosUsuarioDto();
                rta1.NombreUsuario  = "Andres";
                rta1.Cuenta = "andres@yopmail.com";
                rta1.UsuarioDnp = "CC202002";
                lista.Add(rta1);
            }
            return lista;
        }

        public List<CalendarioPeriodoDto> ObtenerCalendartioPeriodo(string bpin)
        {
            List<CalendarioPeriodoDto> lista = new List<CalendarioPeriodoDto>();
            if (string.IsNullOrEmpty(bpin))
                return null;
            else
            {
                CalendarioPeriodoDto rta1 = new CalendarioPeriodoDto();
                rta1.Mes = "Enero";
                rta1.FechaHasta = DateTime.Now.AddDays(+ 4);
                rta1.FechaHasta = DateTime.Now;
                lista.Add(rta1);
            }
            return lista;
        }

        public PresupuestalProyectosAsociadosDto ObtenerPresupuestalProyectosAsociados(int TramiteId, Guid InstanciaId)
        {
            var resumen = new PresupuestalProyectosAsociadosDto();
            string jsonString = "{'TramiteId':1090,'ResumenProyectos':[{'TipoOperacion':'Contrato','TotalTipoOperacion':0,'Proyectos':[{'ProyectoId':98173,'CodigoBpin':'202200000000226','NombreProyecto':'Construcción Ampliación de Infraestructura para Generación de Cupos en Los Establecimientos de Reclusión del Orden - Bogotá','NombreProyectoCorto':'Construcción Ampliación de Infraestructura para Generación de Cupos en Los Estab','CodigoPresupuestal':'0209001206080000010000','TotalSolicitadoNacion':0.00,'TotalSolicitadoPropios':0.00,'TotalAprobadoNacion':0.00,'TotalAprobadoPropios':0.00}]},{'TipoOperacion':'Credito','TotalTipoOperacion':0,'Proyectos':[{'ProyectoId':98189,'CodigoBpin':'202200000000235','NombreProyecto':'Construcción Ampliación de Infraestructura para Generación de Cupos en Los Establecimientos de Reclusión del Orden - Bogotá','NombreProyectoCorto':'Construcción Ampliación de Infraestructura para Generación de Cupos en Los Estab','CodigoPresupuestal':'1201011206080001020000','TotalSolicitadoNacion':0.00,'TotalSolicitadoPropios':0.00,'TotalAprobadoNacion':0.00,'TotalAprobadoPropios':0.00}]}],'ProyectosAsociados':[{'ProyectoId':98189,'CodigoBpin':'202200000000235','NombreProyecto':'Construcción Ampliación de Infraestructura para Generación de Cupos en Los Establecimientos de Reclusión del Orden - Bogotá','NombreProyectoCorto':'Construcción Ampliación de Infraestructura para Generación de Cupos en Los Estab','EntidadFinanciadora':'MINISTERIO DE JUSTICIA Y DEL DERECHO - GESTIÓN GENERAL','NombreSector':'Justicia y del Derecho','TipoProyecto':'Credito','CodigoPresupuestal':'1201011206080001020000','VigenciaInicial':2022,'VigenciaFinal':2025,'TotalApropiacionInicialNacion':0.00,'TotalApropiacionInicialPropios':0.00,'TotalApropiacionVigenteNacion':0.00,'TotalApropiacionVigentePropios':0.00,'TotalVigenciasFuturasNacion':0.00,'TotalVigenciasFuturasPropios':0.00,'DetalleFuentes':[{'TipoRecursoId':1,'NombreTipoRecurso':'10-Recursos corrientes-Nación','ValorInicialCSF':0.00,'ValorInicialSSF':0.00,'ValorVigenteCSF':0.00,'ValorVigenteSSF':0.00,'ValorIncorporarCSF':0.00,'ValorIncorporarSSF':0.00,'ValorIncorporarAprobadoCSF':0.00,'ValorIncorporarAprobadoSSF':0.00},{'TipoRecursoId':2,'NombreTipoRecurso':'11-Otros Recursos del Tesoro-Nación','ValorInicialCSF':0.00,'ValorInicialSSF':0.00,'ValorVigenteCSF':0.00,'ValorVigenteSSF':0.00,'ValorIncorporarCSF':0.00,'ValorIncorporarSSF':0.00,'ValorIncorporarAprobadoCSF':0.00,'ValorIncorporarAprobadoSSF':0.00},{'TipoRecursoId':3,'NombreTipoRecurso':'12-Recursos para preservar la seguridad democrática-Nación','ValorInicialCSF':0.00,'ValorInicialSSF':0.00,'ValorVigenteCSF':0.00,'ValorVigenteSSF':0.00,'ValorIncorporarCSF':0.00,'ValorIncorporarSSF':0.00,'ValorIncorporarAprobadoCSF':0.00,'ValorIncorporarAprobadoSSF':0.00},{'TipoRecursoId':4,'NombreTipoRecurso':'13-Recursos del Crédito Externo Previa Autorización-Nación','ValorInicialCSF':0.00,'ValorInicialSSF':0.00,'ValorVigenteCSF':0.00,'ValorVigenteSSF':0.00,'ValorIncorporarCSF':0.00,'ValorIncorporarSSF':0.00,'ValorIncorporarAprobadoCSF':0.00,'ValorIncorporarAprobadoSSF':0.00},{'TipoRecursoId':5,'NombreTipoRecurso':'14-Préstamos Destinación Específica-Nación','ValorInicialCSF':0.00,'ValorInicialSSF':0.00,'ValorVigenteCSF':0.00,'ValorVigenteSSF':0.00,'ValorIncorporarCSF':0.00,'ValorIncorporarSSF':0.00,'ValorIncorporarAprobadoCSF':0.00,'ValorIncorporarAprobadoSSF':0.00},{'TipoRecursoId':6,'NombreTipoRecurso':'15-Donaciones-Nación','ValorInicialCSF':0.00,'ValorInicialSSF':0.00,'ValorVigenteCSF':0.00,'ValorVigenteSSF':0.00,'ValorIncorporarCSF':0.00,'ValorIncorporarSSF':0.00,'ValorIncorporarAprobadoCSF':0.00,'ValorIncorporarAprobadoSSF':0.00},{'TipoRecursoId':7,'NombreTipoRecurso':'16-Fondos Especiales-Nación','ValorInicialCSF':0.00,'ValorInicialSSF':0.00,'ValorVigenteCSF':0.00,'ValorVigenteSSF':0.00,'ValorIncorporarCSF':0.00,'ValorIncorporarSSF':0.00,'ValorIncorporarAprobadoCSF':0.00,'ValorIncorporarAprobadoSSF':0.00},{'TipoRecursoId':8,'NombreTipoRecurso':'17-Rentas Parafiscales-Nación','ValorInicialCSF':0.00,'ValorInicialSSF':0.00,'ValorVigenteCSF':0.00,'ValorVigenteSSF':0.00,'ValorIncorporarCSF':0.00,'ValorIncorporarSSF':0.00,'ValorIncorporarAprobadoCSF':0.00,'ValorIncorporarAprobadoSSF':0.00},{'TipoRecursoId':9,'NombreTipoRecurso':'20-Ingresos Corrientes-Propios','ValorInicialCSF':0.00,'ValorInicialSSF':0.00,'ValorVigenteCSF':0.00,'ValorVigenteSSF':0.00,'ValorIncorporarCSF':0.00,'ValorIncorporarSSF':0.00,'ValorIncorporarAprobadoCSF':0.00,'ValorIncorporarAprobadoSSF':0.00},{'TipoRecursoId':10,'NombreTipoRecurso':'21-Otros Recursos de Tesorería-Propios','ValorInicialCSF':0.00,'ValorInicialSSF':0.00,'ValorVigenteCSF':0.00,'ValorVigenteSSF':0.00,'ValorIncorporarCSF':0.00,'ValorIncorporarSSF':0.00,'ValorIncorporarAprobadoCSF':0.00,'ValorIncorporarAprobadoSSF':0.00},{'TipoRecursoId':11,'NombreTipoRecurso':'22-Recursos del Crédito Interno Previa Autorización-Propios','ValorInicialCSF':0.00,'ValorInicialSSF':0.00,'ValorVigenteCSF':0.00,'ValorVigenteSSF':0.00,'ValorIncorporarCSF':0.00,'ValorIncorporarSSF':0.00,'ValorIncorporarAprobadoCSF':0.00,'ValorIncorporarAprobadoSSF':0.00},{'TipoRecursoId':12,'NombreTipoRecurso':'23-Recursos del Crédito Externo Previa Autorización-Propios','ValorInicialCSF':0.00,'ValorInicialSSF':0.00,'ValorVigenteCSF':0.00,'ValorVigenteSSF':0.00,'ValorIncorporarCSF':0.00,'ValorIncorporarSSF':0.00,'ValorIncorporarAprobadoCSF':0.00,'ValorIncorporarAprobadoSSF':0.00},{'TipoRecursoId':13,'NombreTipoRecurso':'24-Préstamos Destinación Específica-Propios','ValorInicialCSF':0.00,'ValorInicialSSF':0.00,'ValorVigenteCSF':0.00,'ValorVigenteSSF':0.00,'ValorIncorporarCSF':0.00,'ValorIncorporarSSF':0.00,'ValorIncorporarAprobadoCSF':0.00,'ValorIncorporarAprobadoSSF':0.00},{'TipoRecursoId':14,'NombreTipoRecurso':'25-Donaciones-Propios','ValorInicialCSF':0.00,'ValorInicialSSF':0.00,'ValorVigenteCSF':0.00,'ValorVigenteSSF':0.00,'ValorIncorporarCSF':0.00,'ValorIncorporarSSF':0.00,'ValorIncorporarAprobadoCSF':0.00,'ValorIncorporarAprobadoSSF':0.00},{'TipoRecursoId':15,'NombreTipoRecurso':'26-Fondos Especiales-Propios','ValorInicialCSF':0.00,'ValorInicialSSF':0.00,'ValorVigenteCSF':0.00,'ValorVigenteSSF':0.00,'ValorIncorporarCSF':0.00,'ValorIncorporarSSF':0.00,'ValorIncorporarAprobadoCSF':0.00,'ValorIncorporarAprobadoSSF':0.00},{'TipoRecursoId':16,'NombreTipoRecurso':'27-Rentas Parafiscales-Propios','ValorInicialCSF':0.00,'ValorInicialSSF':0.00,'ValorVigenteCSF':0.00,'ValorVigenteSSF':0.00,'ValorIncorporarCSF':0.00,'ValorIncorporarSSF':0.00,'ValorIncorporarAprobadoCSF':0.00,'ValorIncorporarAprobadoSSF':0.00}]}],'ProyectosAportantes':[{'ProyectoId':98173,'CodigoBpin':'202200000000226','NombreProyecto':'Construcción Ampliación de Infraestructura para Generación de Cupos en Los Establecimientos de Reclusión del Orden - Bogotá','NombreProyectoCorto':'Construcción Ampliación de Infraestructura para Generación de Cupos en Los Estab','EntidadFinanciadora':'AGENCIA PRESIDENCIAL DE COOPERACIÓN INTERNACIONAL DE COLOMBIA, APC - COLOMBIA ','NombreSector':'Presidencia De La República','TipoProyecto':'Contrato','CodigoPresupuestal':'0209001206080000010000','VigenciaInicial':2022,'VigenciaFinal':2025,'TotalApropiacionInicialNacion':0.00,'TotalApropiacionInicialPropios':0.00,'TotalApropiacionVigenteNacion':0.00,'TotalApropiacionVigentePropios':0.00,'TotalVigenciasFuturasNacion':0.00,'TotalVigenciasFuturasPropios':0.00,'DetalleFuentes':[{'TipoRecursoId':1,'NombreTipoRecurso':'10-Recursos corrientes-Nación','ValorInicialCSF':0.00,'ValorInicialSSF':0.00,'ValorVigenteCSF':0.00,'ValorVigenteSSF':0.00,'ValorIncorporarCSF':0.00,'ValorIncorporarSSF':0.00,'ValorIncorporarAprobadoCSF':0.00,'ValorIncorporarAprobadoSSF':0.00},{'TipoRecursoId':2,'NombreTipoRecurso':'11-Otros Recursos del Tesoro-Nación','ValorInicialCSF':0.00,'ValorInicialSSF':0.00,'ValorVigenteCSF':0.00,'ValorVigenteSSF':0.00,'ValorIncorporarCSF':0.00,'ValorIncorporarSSF':0.00,'ValorIncorporarAprobadoCSF':0.00,'ValorIncorporarAprobadoSSF':0.00},{'TipoRecursoId':3,'NombreTipoRecurso':'12-Recursos para preservar la seguridad democrática-Nación','ValorInicialCSF':0.00,'ValorInicialSSF':0.00,'ValorVigenteCSF':0.00,'ValorVigenteSSF':0.00,'ValorIncorporarCSF':0.00,'ValorIncorporarSSF':0.00,'ValorIncorporarAprobadoCSF':0.00,'ValorIncorporarAprobadoSSF':0.00},{'TipoRecursoId':4,'NombreTipoRecurso':'13-Recursos del Crédito Externo Previa Autorización-Nación','ValorInicialCSF':0.00,'ValorInicialSSF':0.00,'ValorVigenteCSF':0.00,'ValorVigenteSSF':0.00,'ValorIncorporarCSF':0.00,'ValorIncorporarSSF':0.00,'ValorIncorporarAprobadoCSF':0.00,'ValorIncorporarAprobadoSSF':0.00},{'TipoRecursoId':5,'NombreTipoRecurso':'14-Préstamos Destinación Específica-Nación','ValorInicialCSF':0.00,'ValorInicialSSF':0.00,'ValorVigenteCSF':0.00,'ValorVigenteSSF':0.00,'ValorIncorporarCSF':0.00,'ValorIncorporarSSF':0.00,'ValorIncorporarAprobadoCSF':0.00,'ValorIncorporarAprobadoSSF':0.00},{'TipoRecursoId':6,'NombreTipoRecurso':'15-Donaciones-Nación','ValorInicialCSF':0.00,'ValorInicialSSF':0.00,'ValorVigenteCSF':0.00,'ValorVigenteSSF':0.00,'ValorIncorporarCSF':0.00,'ValorIncorporarSSF':0.00,'ValorIncorporarAprobadoCSF':0.00,'ValorIncorporarAprobadoSSF':0.00},{'TipoRecursoId':7,'NombreTipoRecurso':'16-Fondos Especiales-Nación','ValorInicialCSF':0.00,'ValorInicialSSF':0.00,'ValorVigenteCSF':0.00,'ValorVigenteSSF':0.00,'ValorIncorporarCSF':0.00,'ValorIncorporarSSF':0.00,'ValorIncorporarAprobadoCSF':0.00,'ValorIncorporarAprobadoSSF':0.00},{'TipoRecursoId':8,'NombreTipoRecurso':'17-Rentas Parafiscales-Nación','ValorInicialCSF':0.00,'ValorInicialSSF':0.00,'ValorVigenteCSF':0.00,'ValorVigenteSSF':0.00,'ValorIncorporarCSF':0.00,'ValorIncorporarSSF':0.00,'ValorIncorporarAprobadoCSF':0.00,'ValorIncorporarAprobadoSSF':0.00},{'TipoRecursoId':9,'NombreTipoRecurso':'20-Ingresos Corrientes-Propios','ValorInicialCSF':0.00,'ValorInicialSSF':0.00,'ValorVigenteCSF':0.00,'ValorVigenteSSF':0.00,'ValorIncorporarCSF':0.00,'ValorIncorporarSSF':0.00,'ValorIncorporarAprobadoCSF':0.00,'ValorIncorporarAprobadoSSF':0.00},{'TipoRecursoId':10,'NombreTipoRecurso':'21-Otros Recursos de Tesorería-Propios','ValorInicialCSF':0.00,'ValorInicialSSF':0.00,'ValorVigenteCSF':0.00,'ValorVigenteSSF':0.00,'ValorIncorporarCSF':0.00,'ValorIncorporarSSF':0.00,'ValorIncorporarAprobadoCSF':0.00,'ValorIncorporarAprobadoSSF':0.00},{'TipoRecursoId':11,'NombreTipoRecurso':'22-Recursos del Crédito Interno Previa Autorización-Propios','ValorInicialCSF':0.00,'ValorInicialSSF':0.00,'ValorVigenteCSF':0.00,'ValorVigenteSSF':0.00,'ValorIncorporarCSF':0.00,'ValorIncorporarSSF':0.00,'ValorIncorporarAprobadoCSF':0.00,'ValorIncorporarAprobadoSSF':0.00},{'TipoRecursoId':12,'NombreTipoRecurso':'23-Recursos del Crédito Externo Previa Autorización-Propios','ValorInicialCSF':0.00,'ValorInicialSSF':0.00,'ValorVigenteCSF':0.00,'ValorVigenteSSF':0.00,'ValorIncorporarCSF':0.00,'ValorIncorporarSSF':0.00,'ValorIncorporarAprobadoCSF':0.00,'ValorIncorporarAprobadoSSF':0.00},{'TipoRecursoId':13,'NombreTipoRecurso':'24-Préstamos Destinación Específica-Propios','ValorInicialCSF':0.00,'ValorInicialSSF':0.00,'ValorVigenteCSF':0.00,'ValorVigenteSSF':0.00,'ValorIncorporarCSF':0.00,'ValorIncorporarSSF':0.00,'ValorIncorporarAprobadoCSF':0.00,'ValorIncorporarAprobadoSSF':0.00},{'TipoRecursoId':14,'NombreTipoRecurso':'25-Donaciones-Propios','ValorInicialCSF':0.00,'ValorInicialSSF':0.00,'ValorVigenteCSF':0.00,'ValorVigenteSSF':0.00,'ValorIncorporarCSF':0.00,'ValorIncorporarSSF':0.00,'ValorIncorporarAprobadoCSF':0.00,'ValorIncorporarAprobadoSSF':0.00},{'TipoRecursoId':15,'NombreTipoRecurso':'26-Fondos Especiales-Propios','ValorInicialCSF':0.00,'ValorInicialSSF':0.00,'ValorVigenteCSF':0.00,'ValorVigenteSSF':0.00,'ValorIncorporarCSF':0.00,'ValorIncorporarSSF':0.00,'ValorIncorporarAprobadoCSF':0.00,'ValorIncorporarAprobadoSSF':0.00},{'TipoRecursoId':16,'NombreTipoRecurso':'27-Rentas Parafiscales-Propios','ValorInicialCSF':0.00,'ValorInicialSSF':0.00,'ValorVigenteCSF':0.00,'ValorVigenteSSF':0.00,'ValorIncorporarCSF':0.00,'ValorIncorporarSSF':0.00,'ValorIncorporarAprobadoCSF':0.00,'ValorIncorporarAprobadoSSF':0.00}]}]}";

            resumen = JsonConvert.DeserializeObject<PresupuestalProyectosAsociadosDto>(jsonString);

            return resumen;
        }

        public string ObtenerPresupuestalProyectosAsociados_Adicion(int TramiteId, Guid InstanciaId)
        {
            string jsonString = "{'TramiteId':1179,'ResumenProyectos':[{'TipoOperacion':'Contracredito','TotalTipoOperacion':0,'Proyectos':[{'ProyectoId':113417,'CodigoBpin':'2018011000678','NombreProyecto':'FORTALECIMIENTO DEL SISTEMA DE SEGURIDAD INTEGRAL MARÍTIMA Y FLUVIAL A NIVEL  NACIONAL','NombreProyectoCorto':'FORTALECIMIENTO DEL SISTEMA DE SEGURIDAD INTEGRAL MARÍTIMA Y FLUVIAL A NIVEL  NA','CodigoPresupuestal':'1501121504010000100000','TotalSolicitadoNacionCSF':0.00,'TotalSolicitadoPropiosCSF':0.00,'TotalSolicitadoNacionSSF':0.00,'TotalSolicitadoPropiosSSF':0.00,'TotalAprobadoNacion':0.00,'TotalAprobadoPropios':0.00}]},{'TipoOperacion':'Credito','TotalTipoOperacion':0,'Proyectos':[{'ProyectoId':111992,'CodigoBpin':'2018011000607','NombreProyecto':'IMPLEMENTACIÓN DEL PLAN NACIONAL DE INFRAESTRUCTURA A NIVEL  NACIONAL','NombreProyectoCorto':'IMPLEMENTACIÓN DEL PLAN NACIONAL DE INFRAESTRUCTURA A NIVEL  NACIONAL','CodigoPresupuestal':'1501121504010000080000','TotalSolicitadoNacionCSF':0.00,'TotalSolicitadoPropiosCSF':0.00,'TotalSolicitadoNacionSSF':0.00,'TotalSolicitadoPropiosSSF':0.00,'TotalAprobadoNacion':0.00,'TotalAprobadoPropios':0.00}]}],'ProyectosAsociados':[{'ProyectoId':111992,'CodigoBpin':'2018011000607','NombreProyecto':'IMPLEMENTACIÓN DEL PLAN NACIONAL DE INFRAESTRUCTURA A NIVEL  NACIONAL','NombreProyectoCorto':'IMPLEMENTACIÓN DEL PLAN NACIONAL DE INFRAESTRUCTURA A NIVEL  NACIONAL','EntidadFinanciadora':'MINISTERIO DE DEFENSA NACIONAL - DIRECCION GENERAL MARITIMA - DIMAR','NombreSector':'Defensa y Policía','TipoProyecto':'Credito','CodigoPresupuestal':'1501121504010000080000','VigenciaInicial':2019,'VigenciaFinal':2029,'TotalApropiacionInicialNacion':0.00,'TotalApropiacionInicialPropios':0.00,'TotalApropiacionVigenteNacion':0.00,'TotalApropiacionVigentePropios':0.00,'TotalVigenciasFuturasNacion':0.00,'TotalVigenciasFuturasPropios':0.00,'MontoTramiteNacion':0.00,'MontoTramitePropios':0.00,'DetalleFuentes':[{'TipoRecursoId':6,'NombreTipoRecurso':'15-Donaciones-Nación','ValorInicialCSF':0.00,'ValorInicialSSF':0.00,'ValorVigenteCSF':0.00,'ValorVigenteSSF':0.00,'ValorIncorporarCSF':0.00,'ValorIncorporarSSF':0.00,'ValorIncorporarAprobadoCSF':0.00,'ValorIncorporarAprobadoSSF':0.00},{'TipoRecursoId':14,'NombreTipoRecurso':'25-Donaciones-Propios','ValorInicialCSF':0.00,'ValorInicialSSF':0.00,'ValorVigenteCSF':0.00,'ValorVigenteSSF':0.00,'ValorIncorporarCSF':0.00,'ValorIncorporarSSF':0.00,'ValorIncorporarAprobadoCSF':0.00,'ValorIncorporarAprobadoSSF':0.00}]},{'ProyectoId':111992,'CodigoBpin':'2018011000607','NombreProyecto':'IMPLEMENTACIÓN DEL PLAN NACIONAL DE INFRAESTRUCTURA A NIVEL  NACIONAL','NombreProyectoCorto':'IMPLEMENTACIÓN DEL PLAN NACIONAL DE INFRAESTRUCTURA A NIVEL  NACIONAL','EntidadFinanciadora':'MINISTERIO DE DEFENSA NACIONAL - DIRECCION GENERAL MARITIMA - DIMAR','NombreSector':'Defensa y Policía','TipoProyecto':'Credito','CodigoPresupuestal':'1501121504010000080000','VigenciaInicial':2019,'VigenciaFinal':2029,'TotalApropiacionInicialNacion':0.00,'TotalApropiacionInicialPropios':0.00,'TotalApropiacionVigenteNacion':0.00,'TotalApropiacionVigentePropios':0.00,'TotalVigenciasFuturasNacion':0.00,'TotalVigenciasFuturasPropios':0.00,'MontoTramiteNacion':3200000000.00,'MontoTramitePropios':0.00,'DetalleFuentes':[{'TipoRecursoId':6,'NombreTipoRecurso':'15-Donaciones-Nación','ValorInicialCSF':0.00,'ValorInicialSSF':0.00,'ValorVigenteCSF':0.00,'ValorVigenteSSF':0.00,'ValorIncorporarCSF':0.00,'ValorIncorporarSSF':0.00,'ValorIncorporarAprobadoCSF':0.00,'ValorIncorporarAprobadoSSF':0.00},{'TipoRecursoId':14,'NombreTipoRecurso':'25-Donaciones-Propios','ValorInicialCSF':0.00,'ValorInicialSSF':0.00,'ValorVigenteCSF':0.00,'ValorVigenteSSF':0.00,'ValorIncorporarCSF':0.00,'ValorIncorporarSSF':0.00,'ValorIncorporarAprobadoCSF':0.00,'ValorIncorporarAprobadoSSF':0.00}]}],'ProyectosAportantes':null}";

            return jsonString;
        }

        public OrigenRecursosDto GetOrigenRecursosTramite(int TramiteId)
        {
            var resumen = new OrigenRecursosDto();
            string jsonString = "{'TramiteId':1090, 'TipoOrigenId':2, 'Rubro':'Este es mi rubro'}";

            resumen = JsonConvert.DeserializeObject<OrigenRecursosDto>(jsonString);

            return resumen;
        }

        public VigenciaFuturaResponse SetOrigenRecursosTramite(OrigenRecursosDto origenRecurso, string usuario)
        {
            VigenciaFuturaResponse response = new VigenciaFuturaResponse();
            if (string.IsNullOrEmpty(origenRecurso.TramiteId.ToString()))
            {
                response.Exito = true;
                response.Mensaje = "OK";
            }
            else
            {
                response.Exito = false;
                response.Mensaje = "Ocurrio un error al eliminar o modificar el registro";
            }

            return response;
        }

        public int ObtenerModalidadContratacionVigenciasFuturas(int ProyectoId, int TramiteId)
        {
            int modalidad = 2;
            return modalidad;
        }

        public List<TramiteRVFAutorizacion> ObtenerAutorizacionesParaReprogramacion(string bpin, int tramiteId)
        {
            // throw new System.NotImplementedException();

            if (tramiteId == 0)
                return null;
            List<TramiteRVFAutorizacion> response = new List<TramiteRVFAutorizacion>();

            TramiteRVFAutorizacion autorizacion = new TramiteRVFAutorizacion();
            autorizacion.Id = 16;
            autorizacion.NumeroTramite = "EJ-TP-VFO-240200-0029";
            autorizacion.CodigoAutorizacion = "223345";
            autorizacion.Descripcion = "prueba descripción";
            autorizacion.TramiteLiberarId = 2198;
            autorizacion.FechaAutorizacion = DateTime.Now;

            response.Add(autorizacion);
            return response;
        }

        public string AsociarAutorizacionRVF(tramiteRVFAsociarproyecto reprogramacionDto, string usuario)
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
                    throw new ServiciosNegocioException(mensajeError);
                }

            }
            catch (ServiciosNegocioException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public TramiteRVFAutorizacion ObtenerAutorizacionAsociada(int tramiteId)
        {
            TramiteRVFAutorizacion response = new TramiteRVFAutorizacion();
            if (tramiteId != 0)
            {
                response.Id = 16;
                response.NumeroTramite = "EJ-TP-VFO-240200-0029";
                response.CodigoAutorizacion = "223345";
                response.Descripcion = "prueba descripción";
                response.TramiteLiberarId = 2198;
                response.FechaAutorizacion = DateTime.Now;
                response.ReprogramacionId = 30;
            }
            else
                response = null;

            return response;
        }

        public VigenciaFuturaResponse EliminaReprogramacionVF(ReprogramacionDto tramiteEliminar)
        {
            VigenciaFuturaResponse response = new VigenciaFuturaResponse();
            if (string.IsNullOrEmpty(tramiteEliminar.Id.ToString()))
            {
                response.Exito = true;
                response.Mensaje = "OK";
            }
            else
            {
                response.Exito = false;
                response.Mensaje = "Ocurrio un error al eliminar o modificar el registro";
            }

            return response;
        }

    }
}
