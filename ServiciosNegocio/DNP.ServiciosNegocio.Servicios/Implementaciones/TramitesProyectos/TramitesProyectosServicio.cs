
namespace DNP.ServiciosNegocio.Servicios.Implementaciones.TramitesProyectos
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Threading.Tasks;
    using Comunes.Dto.ObjetosNegocio;
    using DNP.ServiciosNegocio.Dominio.Dto.Genericos;
    using DNP.ServiciosNegocio.Dominio.Dto.Proyectos;
    using DNP.ServiciosNegocio.Servicios.Interfaces.Proyectos;
    using Interfaces.TramitesProyectos;
    using Interfaces.Transversales;
    using Persistencia.Interfaces.TramitesProyectos;
    using DNP.ServiciosNegocio.Comunes.Dto.Formulario;
    using Persistencia.Interfaces.Genericos;
    using DNP.ServiciosNegocio.Dominio.Dto.Transversales;
    using DNP.ServiciosNegocio.Dominio.Dto.Tramites;
    using DNP.ServiciosNegocio.Comunes.Dto.Tramites;
    using DNP.ServiciosNegocio.Dominio.Dto.Conpes;
    using DNP.ServiciosNegocio.Dominio.Dto.Tramites.VigenciaFutura;
    using DNP.ServiciosNegocio.Dominio.Dto.FuenteFinanciacion;
    using DNP.ServiciosNegocio.Dominio.Dto.Productos;
    using DNP.ServiciosNegocio.Dominio.Dto.Catalogos;
    using DNP.ServiciosNegocio.Persistencia.Modelo;
    using DNP.ServiciosNegocio.Dominio.Dto.Tramites.TramitesReprogramacion;

    public class TramitesProyectosServicio : ServicioBase<DatosTramiteProyectosDto>, ITramitesProyectosServicio
    {

        private readonly ICacheServicio _cacheServicio;
        private readonly ITramitesProyectosPersistencia _tramitesProyectoPersistencia;

        public TramitesProyectosServicio(ICacheServicio cacheServicio, ITramitesProyectosPersistencia tramitesProyectoPersistencia, IPersistenciaTemporal persistenciaTemporal, IAuditoriaServicios auditoriaServicios) : base(persistenciaTemporal, auditoriaServicios)
        {
            _cacheServicio = cacheServicio;
            _tramitesProyectoPersistencia = tramitesProyectoPersistencia;
        }


        public TramitesResultado GuardarProyectosTramiteNegocio(DatosTramiteProyectosDto datosTramiteProyectosDto, string usuario)
        {
            return _tramitesProyectoPersistencia.GuardarProyectosTramiteNegocio(datosTramiteProyectosDto, usuario);
        }

        public TramitesResultado GuardarTramiteInformacionPresupuestal (List<TramiteFuentePresupuestalDto> parametrosGuardar, string usuario)
        {
            return _tramitesProyectoPersistencia.GuardarTramiteInformacionPresupuestal(parametrosGuardar, usuario);
        }

        public TramitesResultado GuardarTramiteTipoRequisito(List<TramiteRequitoDto> parametrosGuardar, string usuario)
        {
            return _tramitesProyectoPersistencia.GuardarTramiteTipoRequisito(parametrosGuardar, usuario);
        }

        public IEnumerable<ProyectosEnTramiteDto> ObtenerProyectosTramiteNegocio(int TramiteId)
        {
            return _tramitesProyectoPersistencia.ObtenerProyectosTramiteNegocio(TramiteId);
        }
        public IEnumerable<TipoDocumentoTramiteDto> ObtenerTipoDocumentoTramite(int TipoTramiteId, Guid? Rol, int tramiteId, int nivelId)
        {
            return _tramitesProyectoPersistencia.ObtenerTipoDocumentoTramite(TipoTramiteId, Rol,  tramiteId, nivelId);
        }

        public void ActualizarInstanciaProyectosTramiteNegocio(ProyectoTramiteDto DatosInstanciasProyecto, string usuario)
        {
            _tramitesProyectoPersistencia.ActualizarInstanciaProyectosTramiteNegocio(DatosInstanciasProyecto, usuario);
        }

        protected override void GuardadoDefinitivo(ParametrosGuardarDto<DatosTramiteProyectosDto> parametrosGuardar, string usuario)
        {
            throw new NotImplementedException();
        }

        protected override DatosTramiteProyectosDto ObtenerDefinitivo(ParametrosConsultaDto parametrosConsultaDto)
        {
            throw new NotImplementedException();
        }

        public TramitesResultado EliminarProyectoTramiteNegocio(int TramiteId, int ProyectoId)
        {
            return _tramitesProyectoPersistencia.EliminarProyectoTramiteNegocio(TramiteId, ProyectoId);
        }

        public IEnumerable<JustificacionTramiteProyectoDto> ObtenerPreguntasJustificacion(int TramiteId, int ProyectoId, int TipoTramiteId, int TipoRolId, Guid IdNivel)
        {
            return _tramitesProyectoPersistencia.ObtenerPreguntasJustificacion(TramiteId, ProyectoId, TipoTramiteId, TipoRolId, IdNivel);
        }

        public TramitesResultado GuardarRespuestasJustificacion(List<JustificacionTramiteProyectoDto> justificacionTramiteProyectoDto, string usuario)
        {
            return _tramitesProyectoPersistencia.GuardarRespuestasJustificacion(justificacionTramiteProyectoDto, usuario);
        }

        public TramitesResultado ActualizarValoresProyectosTramiteNegocio(ProyectoTramiteDto DatosInstanciasProyecto, string usuario)
        {
            return _tramitesProyectoPersistencia.ActualizarValoresProyectosTramiteNegocio(DatosInstanciasProyecto, usuario);
        }

        public IEnumerable<FuentePresupuestalDto> ObtenerFuentesInformacionPresupuestal()
        {
            return _tramitesProyectoPersistencia.ObtenerFuentesInformacionPresupuestal();
        }
        public IEnumerable<ProyectoFuentePresupuestalDto> ObtenerProyectoFuentePresupuestalPorTramite(int pProyectoId, int? pTramiteId, string pTipoProyecto)
        {
            return _tramitesProyectoPersistencia.ObtenerProyectoFuentePresupuestalPorTramite(pProyectoId, pTramiteId, pTipoProyecto);
        }
        public IEnumerable<ProyectoRequisitoDto> ObtenerProyectoRequisitosPorTramite(int pProyectoId, int? pTramiteId, bool isCDP)
        {
            return _tramitesProyectoPersistencia.ObtenerProyectoRequisitosPorTramite(pProyectoId, pTramiteId, isCDP);
        }

        public IEnumerable<TipoRequisitoDto> ObtenerTiposRequisito()
        {
            return _tramitesProyectoPersistencia.ObtenerTiposRequisito();
        }

        public TramitesResultado ValidarEnviarDatosTramiteNegocio(DatosTramiteProyectosDto datosTramiteProyectosDto, string usuario)
        {
            return _tramitesProyectoPersistencia.ValidarEnviarDatosTramiteNegocio(datosTramiteProyectosDto, usuario);
        }

        public IEnumerable<JustificacionTematicaDto> ObtenerPreguntasProyectoActualizacion(int TramiteId, int ProyectoId, int TipoTramiteId, Guid IdNivel, int TipoRolId)
        {
            return _tramitesProyectoPersistencia.ObtenerPreguntasProyectoActualizacion(TramiteId, ProyectoId, TipoTramiteId, IdNivel, TipoRolId);
        }

    
        public IEnumerable<ProyectosEnTramiteDto> ObtenerProyectosTramiteNegocioAprobacion(int TramiteId, int TipoRolId)
        {
            return _tramitesProyectoPersistencia.ObtenerProyectosTramiteNegocioAprobacion(TramiteId, TipoRolId);
        }

        public IEnumerable<FuentesTramiteProyectoAprobacionDto> ObtenerFuentesTramiteProyectoAprobacion(int tramiteId, int proyectoId, string pTipoProyecto)
        {
            return _tramitesProyectoPersistencia.ObtenerFuentesTramiteProyectoAprobacion(tramiteId, proyectoId, pTipoProyecto);
        }

        public TramitesResultado GuardarFuentesTramiteProyectoAprobacion(List<FuentesTramiteProyectoAprobacionDto> fuentesTramiteProyectoAprobacion, string usuario)
        {
            return _tramitesProyectoPersistencia.GuardarFuentesTramiteProyectoAprobacion(fuentesTramiteProyectoAprobacion, usuario);
        }

        public CodigoPresupuestalDto ObtenerCodigoPresupuestal(int proyectoId, int entidadId, int tramiteId, string usuario)
        {
            return _tramitesProyectoPersistencia.ObtenerCodigoPresupuestal(proyectoId, entidadId, tramiteId, usuario);
        }

        public TramitesResultado ActualizarCodigoPresupuestal(int proyectoId, int entidadId, int tramiteId, string usuario)
        {
            return _tramitesProyectoPersistencia.ActualizarCodigoPresupuestal(proyectoId, entidadId, tramiteId, usuario);
        }

        public bool CrearAlcanceTramite(AlcanceTramiteDto data)
        {
            return _tramitesProyectoPersistencia.CrearAlcanceTramite(data);
        }


        public ResponseDto<EnvioSubDireccionDto> GuardarSolicitarConcepto(EnvioSubDireccionDto concepto)
        {
            return _tramitesProyectoPersistencia.GuardarSolicitarConcepto(concepto);
        }

        public List<EnvioSubDireccionDto> ObtenerSolicitarConcepto(int tramiteid)
        {
            return _tramitesProyectoPersistencia.ObtenerSolicitarConcepto(tramiteid);
        }

        public List<TramitesProyectosDto> ObtenerTarmitesPorProyectoEntidad(int proyectoId, int entidadId, string usuario)
        {
            return _tramitesProyectoPersistencia.ObtenerTarmitesPorProyectoEntidad(proyectoId, entidadId, usuario);
        }

        public TramitesValoresProyectoDto ObtenerValoresProyectos(int proyectoId, int tramiteId, int entidadId,  string usuario)
        {
            return _tramitesProyectoPersistencia.ObtenerValoresProyectos(proyectoId, tramiteId,  entidadId,  usuario);
        }

        public List<ConceptoDireccionTecnicaTramite> ObtenerConceptoDireccionTecnicaTramite(int tramiteId, Guid nivelid, string usuario)
        {
            return _tramitesProyectoPersistencia.ObtenerConceptoDireccionTecnicaTramite(tramiteId, nivelid, usuario);
        }

        public TramitesResultado GuardarConceptoDireccionTecnicaTramite(List<ConceptoDireccionTecnicaTramite> lConceptoDireccionTecnicaTramite, string usuario)
        {
           return _tramitesProyectoPersistencia.GuardarConceptoDireccionTecnicaTramite(lConceptoDireccionTecnicaTramite, usuario);
        }

        public TramitesResultado ValidarEnviarDatosTramiteNegocioAprobacion(DatosTramiteProyectosDto datosTramiteProyectosDto,
                                                                string usuario)
        {
            return _tramitesProyectoPersistencia.ValidarEnviarDatosTramiteNegocioAprobacion(datosTramiteProyectosDto, usuario);
        }

        public PlantillaCarta ObtenerPlantillaCarta(string nombreSeccion, int tipoTramite)
        {
           return _tramitesProyectoPersistencia.ObtenerPlantillaCarta(nombreSeccion, tipoTramite);
        }

        public List<Carta> ObtenerDatosCartaPorSeccion(int tramiteId, int plantillaSeccionId)
        {
            return _tramitesProyectoPersistencia.ObtenerDatosCartaPorSeccion(tramiteId, plantillaSeccionId);
        }

        public List<Carta> ObtenerDatosCartaPorSeccionDespedia(int plantillaSeccionId, int tramiteId)
        {
            return _tramitesProyectoPersistencia.ObtenerDatosCartaPorSeccionDespedia(plantillaSeccionId,tramiteId);
        }
        ////Alejandro
        //public List<Carta> ObtenerDatosCartaConceptoDespedida(int tramiteId)
        //{
        //    return _tramitesProyectoPersistencia.ObtenerDatosCartaConceptoDespedida(tramiteId);
        //}

        public UsuarioTramite VerificaUsuarioDestinatario(UsuarioTramite usuarioTramite)
        {
            return _tramitesProyectoPersistencia.VerificaUsuarioDestinatario(usuarioTramite);
        }

        public TramitesResultado ActualizarCartaDatosIniciales(Carta datosIniciales, string usuario)
        {
            return _tramitesProyectoPersistencia.ActualizarCartaDatosIniciales(datosIniciales, usuario);
        }

        public TramitesResultado ActualizarCartaDatosDespedida(Carta datosDespedida, string usuario)
        {
            return _tramitesProyectoPersistencia.ActualizarCartaDatosDespedida(datosDespedida, usuario);
        }

        public List<UsuarioTramite> ObtenerUsuariosRegistrados(int tramiteId, string numeroTramite)
        {
            return _tramitesProyectoPersistencia.ObtenerUsuariosRegistrados(tramiteId, numeroTramite);
        }

        public TramitesResultado CargarFirma(FileToUploadDto parametros)
        {
            return _tramitesProyectoPersistencia.CargarFirma(parametros);
        }

        public TramitesResultado ValidarSiExisteFirmaUsuario(string idUsuario)
        {
            return _tramitesProyectoPersistencia.ValidarSiExisteFirmaUsuario(idUsuario);
        }

        public TramitesResultado Firmar(int tramiteId, string radicadoSalida, string usuario)
        {
            return _tramitesProyectoPersistencia.Firmar(tramiteId, radicadoSalida, usuario);
        }

        public List<CuerpoConceptoCDP> ObtenerDatosCartaConceptoCuerpoCDP(int tramiteId)
        {
            return _tramitesProyectoPersistencia.ObtenerDatosCartaConceptoCuerpoCDP(tramiteId);
        }
        public List<CuerpoConceptoAutorizacion> ObtenerDatosCartaConceptoCuerpoAutorizacion(int tramiteId)
        {
            return _tramitesProyectoPersistencia.ObtenerDatosCartaConceptoCuerpoAutorizacion(tramiteId);
        }

        public Carta ConsultarCarta(int tramiteid)
        {
            return _tramitesProyectoPersistencia.ConsultarCarta(tramiteid);
        }

        public TramitesResultado ActualizaEstadoAjusteProyecto(string tipoDevolucion, string objetoNegocioId, int tramiteId, string observacion, string usuario)
        {
            return _tramitesProyectoPersistencia.ActualizaEstadoAjusteProyecto(tipoDevolucion, objetoNegocioId, tramiteId, observacion,  usuario);
        }

        public int TramiteEnPasoUno(Guid InstanciaId)
        {
            return _tramitesProyectoPersistencia.TramiteEnPasoUno(InstanciaId);
        }

        public ResponseDto<List<TramiteConpesDto>> ObtenerConpesTramite(int tramiteId)
        {
            var response = new ResponseDto<List<TramiteConpesDto>>();

            try {
                var query = _tramitesProyectoPersistencia.ObtenerConpesTramite(tramiteId);

                response.Estado = true;
                response.Data = query.Select(w => new TramiteConpesDto {
                    Id = w.Id,
                    NumeroCONPES = w.NumeroConpes,
                    Titulo = w.NombreConpes
                }).ToList();

            } catch (Exception ex) {
                response.Mensaje = ex.Message;
            }

            return response;
        }

        public ResponseDto<bool> GuardarConpesTramite(AsociarTramiteConpesRequestDto model, string usuario)
        {
            var response = new ResponseDto<bool>();

            try {
                _tramitesProyectoPersistencia.GuardarConpesTramites(
                    model.TramiteId,
                    usuario,
                    model.Conpes
                );
                response.Estado = true;
            }
            catch (Exception ex) {
                response.Mensaje = ex.Message;
            }

            return response;
        }

        public ResponseDto<bool> RemoverConpesTramite(RemoverAsociacionConpesTramiteDto model)
        {
            var response = new ResponseDto<bool>();

            try {
                _tramitesProyectoPersistencia.RemoverConpesTramites(model);
                response.Estado = true;
            }
            catch (Exception ex) {
                response.Mensaje = ex.Message;
            }

            return response;
        }

        public ResponseDto<PeriodoPresidencialDto> ObtenerPeriodoPresidencialActual()
        {
            var response = new ResponseDto<PeriodoPresidencialDto>();

            try {
                var periodo = _tramitesProyectoPersistencia.ObtenerPeriodoPresidencialActual();
                response.Data = periodo == null ? null : new PeriodoPresidencialDto {
                    AnoInicial = periodo.AñoInicial.Value,
                    AnoFinal = periodo.AñoFinal.Value,
                    FechaFinal = periodo.FechaFinal.Value,
                    FechaInicial = periodo.FechaInicial.Value
                };

                response.Estado = true;
            }
            catch (Exception ex) {
                response.Mensaje = ex.Message;
            }

            return response;
        }

        public string EliminarAsociacionVFO(EliminacionAsociacionDto eliminacionAsociacionDto)
        {
            var resulEliminacion = _tramitesProyectoPersistencia.EliminarAsociacionVFO(eliminacionAsociacionDto);

            return resulEliminacion;
        }

        public List<proyectoAsociarTramite> ObtenerProyectoAsociarTramite(string bpin, int tramiteId)
        {
            return _tramitesProyectoPersistencia.ObtenerProyectoAsociarTramite(bpin, tramiteId);
        }

        public string AsociarProyectoVFO(proyectoAsociarTramite proyectoDto, string usuario)
        {
            return _tramitesProyectoPersistencia.AsociarProyectoVFO(proyectoDto, usuario);
        }

        public DatosProyectoTramiteDto ObtenerDatosProyectoTramite(int tramiteId)
        {
            return _tramitesProyectoPersistencia.ObtenerDatosProyectoTramite(tramiteId);
        }

        public DetalleCartaConceptoDto ObtenerDetalleCartaConcepto(int tramiteId)
        {
            var query = _tramitesProyectoPersistencia.ObtenerDetalleCartaConcepto(tramiteId);

            return query == null? null: new DetalleCartaConceptoDto
            {
                TramiteId = query.TramiteId,
                RadicadoEntrada = query.RadicadoEntrada,
                RadicadoSalida = query.RadicadoSalida,
                FaseId = query.FaseId,
                ExpedienteId = query.ExpedienteId
            };
        }

        public List<DatosProyectoTramiteDto> ObtenerDatosProyectosPorTramite(int tramiteId)
        {
            return _tramitesProyectoPersistencia.ObtenerDatosProyectosPorTramite(tramiteId);
        }

        #region Vigencias Futuras

        public InformacionPresupuestalVlrConstanteDto ObtenerInformacionPresupuestalVlrConstanteVF(int tramiteId)
        {
            return _tramitesProyectoPersistencia.ObtenerInformacionPresupuestalVlrConstanteVF(tramiteId);
        }

        public string ObtenerDatosCronograma(Guid instanciaId)
        {
            return _tramitesProyectoPersistencia.ObtenerDatosCronograma(instanciaId);
        }

        public IEnumerable<JustificacionPasoDto> ObtenerPreguntasProyectoActualizacionPaso(int TramiteId, int ProyectoId, int TipoTramiteId, Guid IdNivel, int TipoRolId)
        {
            return _tramitesProyectoPersistencia.ObtenerPreguntasProyectoActualizacionPaso(TramiteId, ProyectoId, TipoTramiteId, IdNivel, TipoRolId);
        }

        public List<TramiteDeflactoresDto> GetTramiteDeflactores()
        {
            return _tramitesProyectoPersistencia.GetTramiteDeflactores();
        }

        public List<Dominio.Dto.Tramites.TramiteProyectoDto> GetProyectoTramite(int ProyectoId, int TramiteId)
        {
            return _tramitesProyectoPersistencia.GetProyectoTramite(ProyectoId, TramiteId);
        }

        public string ActualizaVigenciaFuturaProyectoTramite(Dominio.Dto.Tramites.TramiteProyectoDto tramiteProyectoDto, string usuario)
        {
            return _tramitesProyectoPersistencia.ActualizaVigenciaFuturaProyectoTramite(tramiteProyectoDto, usuario);
        }
        public InformacionPresupuestalValoresDto ObtenerInformacionPresupuestalValores(int tramiteId)
        {
            return _tramitesProyectoPersistencia.ObtenerInformacionPresupuestalValores(tramiteId);
        }

        public string GuardarInformacionPresupuestalValores(InformacionPresupuestalValoresDto informacionPresupuestalValoresDto, string usuario)
        {
            return _tramitesProyectoPersistencia.GuardarInformacionPresupuestalValores(informacionPresupuestalValoresDto, usuario);
        }
    
        #endregion Vigencias Futuras

        public List<EnvioSubDireccionDto> ObtenerSolicitarConceptoPorTramite(int tramiteId)
        {
            return _tramitesProyectoPersistencia.ObtenerSolicitarConceptoPorTramite(tramiteId);
        }

        public VigenciaFuturaCorrienteDto GetFuentesFinanciacionVigenciaFuturaCorriente(string Bpin)
        {
            return _tramitesProyectoPersistencia.GetFuentesFinanciacionVigenciaFuturaCorriente(Bpin);
        }

        public VigenciaFuturaConstanteDto GetFuentesFinanciacionVigenciaFuturaCoonstante(string Bpin, int TramiteId)
        {
            return _tramitesProyectoPersistencia.GetFuentesFinanciacionVigenciaFuturaCoonstante(Bpin, TramiteId);
        }

        public AccionDto ObtenerAccionActualyFinal(int tramiteId, string bpin)
        {
            return _tramitesProyectoPersistencia.ObtenerAccionActualyFinal(tramiteId, bpin);
        }
        public int EliminarPermisosAccionesUsuarios(string usuarioDestino, int tramiteId, string aliasNivel, Guid InstanciaId = default(Guid))
        {
            return _tramitesProyectoPersistencia.EliminarPermisosAccionesUsuarios(usuarioDestino, tramiteId, aliasNivel, InstanciaId);
        }

        public VigenciaFuturaResponse ActualizarVigenciaFuturaFuente(VigenciaFuturaCorrienteFuenteDto fuente, string usuario)
        {
            return _tramitesProyectoPersistencia.ActualizarVigenciaFuturaFuente(fuente, usuario);
        }

        public VigenciaFuturaResponse ActualizarVigenciaFuturaProducto(ProductosConstantesVFDetalleProducto prod, string usuario)
        {
            return _tramitesProyectoPersistencia.ActualizarVigenciaFuturaProducto(prod, usuario);
        }

        public List<TramiteModalidadContratacionDto> ObtenerModalidadesContratacion(int? mostrar)
        {
            return _tramitesProyectoPersistencia.ObtenerModalidadesContratacion(mostrar);
        }
        public ActividadPreContractualDto ActualizarActividadesCronograma(ActividadPreContractualDto actividades, string usuario)
        {
            return _tramitesProyectoPersistencia.ActualizarActividadesCronograma(actividades, usuario);
        }
        public ActividadPreContractualDto ObtenerActividadesPrecontractualesProyectoTramite(int ModalidadContratacionId, int ProyectoId, int TramiteId, bool eliminarActividades)
        {
            return _tramitesProyectoPersistencia.ObtenerActividadesPrecontractualesProyectoTramite(ModalidadContratacionId,  ProyectoId,  TramiteId, eliminarActividades);
        }

        public TramitesResultado EnviarConceptoDireccionTecnicaTramite(int tramiteId, string usuario)
        {
            return _tramitesProyectoPersistencia.EnviarConceptoDireccionTecnicaTramite(tramiteId, usuario);
        }

        public ProductosConstantesVF GetProductosVigenciaFuturaConstante(string Bpin, int TramiteId, int AnioBase)
        {
            return _tramitesProyectoPersistencia.GetProductosVigenciaFuturaConstante(Bpin, TramiteId, AnioBase);
        }

        public ProductosCorrientesVF GetProductosVigenciaFuturaCorriente(string Bpin, int TramiteId)
        {
            return _tramitesProyectoPersistencia.GetProductosVigenciaFuturaCorriente(Bpin, TramiteId);
        }

        public IEnumerable<TipoDocumentoTramiteDto> ObtenerTipoDocumentoTramitePorNivel(int tipoTramiteId, string nivelId, string rolId)
        {
            return _tramitesProyectoPersistencia.ObtenerTipoDocumentoTramitePorNivel(tipoTramiteId, nivelId, rolId);
        }

        public IEnumerable<DatosUsuarioDto> ObtenerDatosUsuario(string idUsuarioDnp, int idEntidad, Guid idAccion, Guid idIntancia)
        {
            return _tramitesProyectoPersistencia.ObtenerDatosUsuario(idUsuarioDnp, idEntidad, idAccion, idIntancia);
        }

        public List<proyectoAsociarTramite> ObtenerProyectoAsociarTramiteLeyenda(string bpin, int tramiteId)
        {
            return _tramitesProyectoPersistencia.ObtenerProyectoAsociarTramiteLeyenda(bpin, tramiteId);
        }

        public ModificacionLeyendaDto ObtenerModificacionLeyenda(int tramiteId, int ProyectoId)
        {
            return _tramitesProyectoPersistencia.ObtenerModificacionLeyenda(tramiteId, ProyectoId);
        }

        public string ActualizaModificacionLeyenda(ModificacionLeyendaDto modificacionLeyendaDto, string usuario)
        {
            return _tramitesProyectoPersistencia.ActualizaModificacionLeyenda(modificacionLeyendaDto,usuario);
        }


        public List<EntidadCatalogoDTDto> ObtenerListaDireccionesDNP(Guid IdEntidad)
        {
            return _tramitesProyectoPersistencia.ObtenerListaDireccionesDNP(IdEntidad);
        }


        public List<EntidadCatalogoDTDto> ObtenerListaSubdireccionesPorParentId(int idEntididadType)
        {
            return _tramitesProyectoPersistencia.ObtenerListaSubdireccionesPorParentId(idEntididadType);
        }
        public TramitesResultado BorrarFirma(FileToUploadDto parametros)
        {
            return _tramitesProyectoPersistencia.BorrarFirma(parametros);
        }

        public ProyectosCartaDto ObtenerProyectosCartaTramite(int tramiteId)
        {
            return _tramitesProyectoPersistencia.ObtenerProyectosCartaTramite(tramiteId);
        }
        public DetalleCartaConceptoALDto ObtenerDetalleCartaAL(int tramiteId)
        {
            return _tramitesProyectoPersistencia.ObtenerDetalleCartaAL(tramiteId);
        }

        public int ObtenerAmpliarDevolucionTramite(int ProyectoId, int TramiteId)
        {
            return _tramitesProyectoPersistencia.ObtenerAmpliarDevolucionTramite(ProyectoId, TramiteId);
        }

        public DatosProyectoTramiteDto ObtenerDatosProyectoConceptoPorInstancia(Guid instanciaId)
        {
            return _tramitesProyectoPersistencia.ObtenerDatosProyectoConceptoPorInstancia(instanciaId);
        }

        public List<TramiteLiberacionVfDto> ObtenerLiberacionVigenciasFuturas(int ProyectoId, int TramiteId)
        {
            return _tramitesProyectoPersistencia.ObtenerLiberacionVigenciasFuturas(ProyectoId, TramiteId);
        }

        public VigenciaFuturaResponse InsertaAutorizacionVigenciasFuturas(TramiteALiberarVfDto autorizacion, string usuario)
        {
            return _tramitesProyectoPersistencia.InsertaAutorizacionVigenciasFuturas(autorizacion, usuario);
        }

        public VigenciaFuturaResponse InsertaValoresUtilizadosLiberacionVF(TramiteALiberarVfDto autorizacion, string usuario)
        {
            return _tramitesProyectoPersistencia.InsertaValoresUtilizadosLiberacionVF(autorizacion, usuario);
        }

        public List<ProyectoTramiteFuenteDto> ObtenerListaProyectosFuentes(int tramiteId)
        {
            return _tramitesProyectoPersistencia.ObtenerListaProyectosFuentes(tramiteId);
        }

        public List<EntidadesAsociarComunDto> ObtenerEntidadAsociarProyecto(Guid InstanciaId, string AccionTramiteProyecto)
        {
            return _tramitesProyectoPersistencia.ObtenerEntidadAsociarProyecto(InstanciaId, AccionTramiteProyecto);
        }

        public CartaConcepto ConsultarCartaConcepto(int tramiteid)
        {
            return _tramitesProyectoPersistencia.ConsultarCartaConcepto(tramiteid);

        }

        public int ValidacionPeriodoPresidencial(int tramiteid)
        {
            return _tramitesProyectoPersistencia.ValidacionPeriodoPresidencial(tramiteid);

        }

        public TramitesResultado GuardarMontosTramite(List<ProyectosEnTramiteDto> parametrosGuardar, string usuario)
        {
            return _tramitesProyectoPersistencia.GuardarMontosTramite(parametrosGuardar, usuario);
        }

        public List<tramiteVFAsociarproyecto> ObtenerTramitesVFparaLiberar(int proyectoId)
        {
            return _tramitesProyectoPersistencia.ObtenerTramitesVFparaLiberar(proyectoId);
        }
        public string GuardarLiberacionVigenciaFutura(LiberacionVigenciasFuturasDto liberacionVigenciasFuturasDto, string usuario)
        {
            return _tramitesProyectoPersistencia.GuardarLiberacionVigenciaFutura(liberacionVigenciasFuturasDto, usuario);
        }

        public IEnumerable<ProyectoJustificacioneDto> ObtenerPreguntasJustificacionPorProyectos(int TramiteId, int TipoTramiteId, int TipoRolId, Guid IdNivel)
        {
            return _tramitesProyectoPersistencia.ObtenerPreguntasJustificacionPorProyectos(TramiteId, TipoTramiteId, TipoRolId, IdNivel);
        }

        public List<ResumenLiberacionVfDto> ObtenerResumenLiberacionVigenciasFuturas(int ProyectoId, int TramiteId)
        {
            return _tramitesProyectoPersistencia.ObtenerResumenLiberacionVigenciasFuturas(ProyectoId, TramiteId);
        }

        public ValoresUtilizadosLiberacionVfDto ObtenerValUtilizadosLiberacionVigenciasFuturas(int ProyectoId, int TramiteId)
        {
            return _tramitesProyectoPersistencia.ObtenerValUtilizadosLiberacionVigenciasFuturas(ProyectoId, TramiteId);
        }

        public int TramiteAjusteEnPasoUno(int tramiteId, int proyectoId)
        {
            return _tramitesProyectoPersistencia.TramiteAjusteEnPasoUno(tramiteId, proyectoId);
        }

        public List<ProyectoTramiteFuenteDto> ObtenerListaProyectosFuentesAprobado(int tramiteId)
        {
            return _tramitesProyectoPersistencia.ObtenerListaProyectosFuentesAprobado(tramiteId);
        }

        public VigenciaFuturaResponse InsertaValoresproductosLiberacionVFCorrientes(DetalleProductosCorrientesDto productosCorrientes, string usuario)
        {
            return _tramitesProyectoPersistencia.InsertaValoresproductosLiberacionVFCorrientes(productosCorrientes, usuario);
        }

        public VigenciaFuturaResponse InsertaValoresproductosLiberacionVFConstantes(DetalleProductosConstantesDto productosConstantes, string usuario)
        {
            return _tramitesProyectoPersistencia.InsertaValoresproductosLiberacionVFConstantes(productosConstantes, usuario);
        }

        public List<EntidadesAsociarComunDto> ObtenerEntidadTramite(string numeroTramite)
        {
            return _tramitesProyectoPersistencia.ObtenerEntidadTramite(numeroTramite);
        }

        public VigenciaFuturaResponse EliminaLiberacionVF(LiberacionVigenciasFuturasDto tramiteEliminar)
        {
            return _tramitesProyectoPersistencia.EliminaLiberacionVF(tramiteEliminar);
        }
        public List<DatosUsuarioDto> ObtenerUsuariosPorInstanciaPadre(Guid InstanciaId)
        {
            return _tramitesProyectoPersistencia.ObtenerUsuariosPorInstanciaPadre(InstanciaId);
        }

        public List<CalendarioPeriodoDto> ObtenerCalendartioPeriodo(string bpin)
        {
            return _tramitesProyectoPersistencia.ObtenerCalendartioPeriodo(bpin);
        }

        public PresupuestalProyectosAsociadosDto ObtenerPresupuestalProyectosAsociados(int TramiteId, Guid InstanciaId)
        {
            return _tramitesProyectoPersistencia.ObtenerPresupuestalProyectosAsociados(TramiteId, InstanciaId);
        }

        public string ObtenerPresupuestalProyectosAsociados_Adicion(int TramiteId, Guid InstanciaId)
        {
            return _tramitesProyectoPersistencia.ObtenerPresupuestalProyectosAsociados_Adicion(TramiteId, InstanciaId);
        }

        public OrigenRecursosDto GetOrigenRecursosTramite(int TramiteId)
        {
            return _tramitesProyectoPersistencia.GetOrigenRecursosTramite(TramiteId);
        }

        public VigenciaFuturaResponse SetOrigenRecursosTramite(OrigenRecursosDto origenRecurso, string usuario)
        {
            return _tramitesProyectoPersistencia.SetOrigenRecursosTramite(origenRecurso, usuario);
        }

        public int ObtenerModalidadContratacionVigenciasFuturas( int ProyectoId, int TramiteId)
        {

            return _tramitesProyectoPersistencia.ObtenerModalidadContratacionVigenciasFuturas(ProyectoId, TramiteId);
        }

        public List<TramiteRVFAutorizacion> ObtenerAutorizacionesParaReprogramacion(string bpin, int tramiteId)
        {
            return _tramitesProyectoPersistencia.ObtenerAutorizacionesParaReprogramacion(bpin, tramiteId);
        }

        public string AsociarAutorizacionRVF(tramiteRVFAsociarproyecto reprogramacionDto, string usuario)
        {
            return _tramitesProyectoPersistencia.AsociarAutorizacionRVF(reprogramacionDto, usuario);
        }

        public TramiteRVFAutorizacion ObtenerAutorizacionAsociada(int tramiteId)
        {
            return _tramitesProyectoPersistencia.ObtenerAutorizacionAsociada(tramiteId);
        }

        public VigenciaFuturaResponse EliminaReprogramacionVF(ReprogramacionDto tramiteEliminar)
        {
            return _tramitesProyectoPersistencia.EliminaReprogramacionVF(tramiteEliminar);
        }
    }
}
