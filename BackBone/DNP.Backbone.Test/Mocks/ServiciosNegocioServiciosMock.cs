namespace DNP.Backbone.Test.Mocks
{
    using DNP.Backbone.Comunes.Dto;
    using DNP.Backbone.Comunes.Enums;
    using DNP.Backbone.Dominio.Dto;
    using DNP.Backbone.Dominio.Dto.Conpes;
    using DNP.Backbone.Dominio.Dto.Orfeo;
    using DNP.Backbone.Dominio.Dto.Proyecto;
    using DNP.Backbone.Dominio.Dto.Tramites;
    using DNP.Backbone.Dominio.Dto.Tramites.Proyectos;
    using DNP.Backbone.Dominio.Dto.Transversal;
    using DNP.Backbone.Dominio.Dto.VigenciasFuturas;
    using DNP.Backbone.Servicios.Interfaces.ServiciosNegocio;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Web.Http;
    using DNP.Backbone.Dominio.Dto.PoliticasTransversalesCrucePoliticas;
    using DNP.Backbone.Dominio.Dto.Tramites.VigenciaFutura;
    using Newtonsoft.Json;
    using DNP.Backbone.Dominio.Dto.FuenteFinanciacion;
	using System.Configuration;
	using DNP.Backbone.Servicios.Interfaces;
    using DNP.Backbone.Dominio.Dto.Productos;
    using DNP.ServiciosNegocio.Dominio.Dto.FuenteFinanciacion;
    using DNP.ServiciosNegocio.Dominio.Dto.Tramites;
    using DNP.Backbone.Dominio.Dto.Transferencias;
    using DNP.Backbone.Dominio.Dto.Acciones;
    using DNP.Backbone.Dominio.Dto.Tramites.Reprogramacion;
    using DNP.Backbone.Dominio.Dto.Tramites.ProgramacionDistribucion;
    using DNP.ServiciosNegocio.Dominio.Dto.TramiteIncorporacion;

    public class ServiciosNegocioServiciosMock : IServiciosNegocioServicios
    {
        public Task<object> InsertAuditoriaEntidadProyecto(Dominio.Dto.Proyecto.AuditoriaEntidadDto auditoriaEntidad, string idUsuarioDNP)
        {
            throw new System.NotImplementedException();
        }

        public Task<object> ObtenerAuditoriaEntidadProyecto(int proyectoId, string idUsuarioDNP)
        {
            throw new System.NotImplementedException();
        }

        public Task<List<CatalogoDto>> ObtenerListaCatalogo(ProyectoParametrosDto peticionObtenerProyecto, CatalogoEnum catalogoEnum)
        {
            return Task.FromResult(new List<CatalogoDto>() { new CatalogoDto() });
        }

        public Task<List<EstadoDto>> ObtenerListaEstado(ProyectoParametrosDto peticion)
        {
            return Task.FromResult(new List<EstadoDto>());
        }

        public Task<List<Dominio.Dto.ProyectoDto>> ObtenerListaProyectoPorTramite(ParametrosProyectosDto parametros)
        {
            return Task.FromResult(new List<Dominio.Dto.ProyectoDto>());
        }

        public Task<List<ProyectosEntidadesDto>> ObtenerProyectos(ParametrosProyectosDto dto, string idUsuarioDnp)
        {
            return Task.FromResult(new List<ProyectosEntidadesDto>());
        }

        public Task<ProyectosTramitesDTO> ObtenerProyectosTramite(InstanciaTramiteDto instanciaTramiteDto)
        {
            return Task.FromResult(new ProyectosTramitesDTO());
        }

        public Task<List<TramiteDto>> ObtenerTramites(InstanciaTramiteDto instanciaTramiteDto)
        {
            return Task.FromResult(new List<TramiteDto>());
        }

        Task<RespuestaGeneralDto> IServiciosNegocioServicios.EliminarProyectoTramiteNegocio(InstanciaTramiteDto instanciaTramiteDto)
        {
            throw new System.NotImplementedException();
        }

        Task<RespuestaGeneralDto> IServiciosNegocioServicios.GuardarProyectosTramiteNegocio(DatosTramiteProyectosDto parametros, string usuarioDnp)
        {
            throw new System.NotImplementedException();
        }

        Task<object> IServiciosNegocioServicios.InsertAuditoriaEntidadProyecto(Dominio.Dto.Proyecto.AuditoriaEntidadDto auditoriaEntidad, string idUsuarioDNP)
        {
            throw new System.NotImplementedException();
        }

        Task<object> IServiciosNegocioServicios.ObtenerAuditoriaEntidadProyecto(int proyectoId, string idUsuarioDNP)
        {
            throw new System.NotImplementedException();
        }

        Task<List<CatalogoDto>> IServiciosNegocioServicios.ObtenerListaCatalogo(ProyectoParametrosDto peticionObtenerProyecto, CatalogoEnum catalogoEnum)
        {
            throw new System.NotImplementedException();
        }

        Task<string> IServiciosNegocioServicios.ObtenerListaCatalogoEntidades(ProyectoParametrosDto peticion, CatalogoEnum catalogoEnum)
        {
            throw new System.NotImplementedException();
        }

        Task<List<EstadoDto>> IServiciosNegocioServicios.ObtenerListaEstado(ProyectoParametrosDto peticion)
        {
            throw new System.NotImplementedException();
        }

        Task<List<ConfiguracionUnidadMatrizDTO>> ObtenerMatrizEntidadDestino(ListMatrizEntidadDestinoDto peticion)
        {
            throw new System.NotImplementedException();
        }

        Task<List<Dominio.Dto.ProyectoDto>> IServiciosNegocioServicios.ObtenerListaProyectoPorTramite(ParametrosProyectosDto parametros)
        {
            throw new System.NotImplementedException();
        }

        Task<List<ProyectosEntidadesDto>> IServiciosNegocioServicios.ObtenerProyectos(ParametrosProyectosDto dto, string idUsuarioDnp)
        {
            throw new System.NotImplementedException();
        }

        Task<List<ProyectosEnTramiteDto>> IServiciosNegocioServicios.ObtenerProyectosTramiteNegocio(int TramiteId, string usuarioDnpo, string tokenAutorizacion)
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

        public Task<List<JustificacionTramiteProyectoDto>> ObtenerPreguntasJustificacion(int TramiteId, int ProyectoId, int TipoTramiteId, int TipoRolId, string usuarioDnp, string tokenAutorizacion)
        {
            throw new System.NotImplementedException();
        }

        public Task<RespuestaGeneralDto> ActualizarValoresProyecto(ProyectosTramiteDto parametros, string usuarioDnp)
        {
            throw new System.NotImplementedException();
        }
        public Task<RespuestaGeneralDto> GuardarTramiteInformacionPresupuestal(List<TramiteFuentePresupuestalDto> parametros, string usuarioDnp)
        {
            throw new System.NotImplementedException();
        }
        public Task<RespuestaGeneralDto> GuardarTramiteTipoRequisito(List<TramiteRequitoDto> parametros, string usuarioDnp)
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

        public Task<List<JustificacionTramiteProyectoDto>> ObtenerPreguntasProyectoActualizacion(int TramiteId, int ProyectoId, int TipoTramiteId, int TipoRolId, string usuarioDnp, string tokenAutorizacion)
        {
            throw new System.NotImplementedException();
        }

        public Task<List<ProyectosEnTramiteDto>> ObtenerProyectosTramiteNegocioAprobacion(int TramiteId, int TipoRolId, string usuarioDnpo, string tokenAutorizacion)
        {
            throw new System.NotImplementedException();
        }

        public Task<List<FuentesTramiteProyectoAprobacionDto>> ObtenerFuentesTramiteProyectoAprobacion(int tramiteId, int proyectoId, string pTipoProyecto, string usuarioDnp, string tokenAutorizacion)
        {
            throw new System.NotImplementedException();
        }

        public Task<RespuestaGeneralDto> GuardarFuentesTramiteProyectoAprobacion(List<FuentesTramiteProyectoAprobacionDto> parametros, string usuarioDnp)
        {
            throw new System.NotImplementedException();
        }

        public Task<List<TipoDocumentoTramiteDto>> ObtenerTipoDocumentoTramite(int TipoTramiteId ,  string Rol, int tramiteId, string usuarioDnpo, string tokenAutorizacion, string nivelId)
        {
            throw new System.NotImplementedException();
        }

        public Task<CodigoPresupuestalDto> ObtenerCodigoPresupuestal(int proyectoId, int entidadId, int tramiteId, string usuarioDnp)
        {
            throw new System.NotImplementedException();
        }

        public Task<RespuestaGeneralDto> ActualizarCodigoPresupuestal(int proyectoId, int entidadId, int tramiteId, string usuarioDnp)
        {
            throw new System.NotImplementedException();
        }

        public Task<List<EntidadCatalogoSTDto>> ObtenerListaCatalogoDT(ProyectoParametrosDto peticionObtenerProyecto, CatalogoEnum catalogoEnum)
        {
            throw new System.NotImplementedException();
        }

        public async Task<ResponseDto<EnvioSubDireccionDto>> SolicitarConcepto(ProyectoParametrosDto peticionObtenerProyecto)
        {
            ResponseDto<EnvioSubDireccionDto> respuesta = new ResponseDto<EnvioSubDireccionDto>();
            await Task.Run(() =>
            {
                respuesta.Estado = true;
                respuesta.Mensaje = "Exitoso";
                EnvioSubDireccionDto envio = new EnvioSubDireccionDto();
                envio.Activo = true;
                envio.Usuario = peticionObtenerProyecto.IdUsuario;
                respuesta.Data = envio;
            });


            return respuesta;
        }

        public Task<List<EnvioSubDireccionDto>> ObtenerSolicitarConcepto(ProyectoParametrosDto peticionObtenerProyecto)
        {
            throw new System.NotImplementedException();
        }

        public Task<TramiteValoresProyectoDto> ObtenerValoresProyectos(int proyectoId, int tramiteId, int entidadId, string usuarioDnp)
        {
            throw new System.NotImplementedException();
        }

        public Task<List<TramitesProyectosDto>> ObtenerTarmitesPorProyectoEntidad(int proyectoId, int tramiteId, string usuarioDnp)
        {
            throw new System.NotImplementedException();
        }

        public Task<string> ObtenerProyectoListaLocalizaciones(string bpin, string IdUsuario, string tokenAutorizacion)
        {
            throw new System.NotImplementedException();
        }

        public Task<List<JustificacionTramiteProyectoDto>> ObtenerPreguntasJustificacion(int TramiteId, int ProyectoId, int TipoTramiteId, int TipoRolId, string usuarioDnp, string IdNivel, string tokenAutorizacion)
        {
            throw new System.NotImplementedException();
        }

        public Task<RespuestaGeneralDto> DevolverProyecto(DevolverProyectoDto parametros, string usuarioDnp)
        {
            throw new NotImplementedException();
        }

        public Task<List<ConceptoDireccionTecnicaTramiteDto>> ObtenerConceptoDireccionTecnicaTramite(int tramiteId1, Guid nivelid, string usuario)
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
        Task<string> IServiciosNegocioServicios.ObtenerCartaConceptoDatosDespedida(int tramiteId, string IdUsuario, string tokenAutorizacion)
        {
            throw new NotImplementedException();
        }
        Task<RespuestaGeneralDto> IServiciosNegocioServicios.ActualizarCartaConceptoDatosDespedida(DatosConceptoDespedidaDto parametros, string usuarioDnp)
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
        public Task<string> ObtenerCartaConceptoDatosDespedida(int tramiteId, string IdUsuario, string tokenAutorizacion)
        {
            throw new NotImplementedException();
        }

        public Task<DatosConceptoDespedidaDto> ActualizarCartaConceptoDatosDespedida(DatosTramiteProyectosDto parametros, string usuarioDnp)
        {
            throw new NotImplementedException();
        }

        public Task<List<UsuarioTramite>> ObtenerUsuariosRegistrados(int tramiteId, string numeroTramite, string usuarioDnp)
        {
            throw new NotImplementedException();
        }

        public Task<List<JustificacionTematicaDto>> ObtenerPreguntasProyectoActualizacion(int TramiteId, int ProyectoId, int TipoTramiteId, Guid IdNivel, int TipoRolId, string usuarioDnp, string tokenAutorizacion)
        {
            throw new NotImplementedException();
        }

        RespuestaDocumentoCONPES IServiciosNegocioServicios.ObtenerProyectoConpes(string conpes, string idUsuario)
        {
            throw new NotImplementedException();
        }

        public Task<List<ProyectoConpes>> ObtenerUsuariosRegistrados(string proyectoid, string idUsuario)
        {
            throw new NotImplementedException();
        }

        public Task<EncabezadoGeneralDto> ObtenerEncabezadoGeneral(ProyectoParametrosEncabezadoDto parametros, string usuarioDnp)
        {
            throw new NotImplementedException();
        }

        public Task<CapituloConpes> CargarProyectoConpes(string proyectoid, Guid InstanciaId, string GuiMacroproceso, string idUsuario,string NivelId, string FlujoId)
        {
            throw new NotImplementedException();
        }

        public Task<string> ObtenerDesagregarRegionalizacion(string bpin, string IdUsuario, string tokenAutorizacion)
        {
            throw new NotImplementedException();
        }

        public Task<RespuestaGeneralDto> ActualizarDesagregarRegionalizacion(Dominio.Dto.Proyecto.DesagregarRegionalizacionDto parametros, string usuarioDnp)
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

        public Task<RespuestaGeneralDto> AdicionarProyectoConpes(CapituloConpes conpes, string idUsuario)
        {
            throw new NotImplementedException();
        }

        public Task<List<DocumentoCONPES>> EliminarProyectoConpes(string proyectoid, string conpesid, string idUsuario)
        {
            throw new NotImplementedException();
        }

        public Task<List<SeccionCapituloDto>> SeccionesCapitulosModificadosByMacroproceso(string guiMacroproceso, int IdProyecto, string IdInstancia, string idUsuario)
        {
            throw new NotImplementedException();
        }

        public Task<List<SeccionCapituloDto>> SeccionesCapitulosByMacroproceso(string guiMacroproceso, string idUsuario, string NivelId,string FlujoId)
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

        public Task<List<RelacionPlanificacionDto>> CambiosRelacionPlanificacion(int IdProyecto, string usuarioDnp)
        {
            throw new NotImplementedException();
        }

        public Task<RespuestaGeneralDto> GuardarCambiosRelacionPlanificacion(CapituloModificado parametros, string usuarioDnp)
        {
            throw new NotImplementedException();
        }

        public Task<RespuestaGeneralDto> ValidarSeccionesCapitulosByMacroproceso(string guiMacroproceso, int IdProyecto, string IdInstancia, string usuarioDnp)
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
            throw new System.NotImplementedException();
        }

        Task<ResponseDto<bool>> IServiciosNegocioServicios.CargarDocumentoElectronicoOrfeo(DatosDocumentoElectronicoDSDto datosDocumentoElectronicoDSDto, string usuarioDnp)
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseDto<bool>> ConsultarRadicado(string radicado, string usuarioDnp)
        {
            ResponseDto<bool> respuesta = new ResponseDto<bool>();
            await Task.Run(() =>
            {
                respuesta.Estado = true;
                respuesta.Mensaje = "202206630177221";
            });


            return respuesta;
        }

        public Task<ResponseDto<bool>> CerrarRadicado(CerrarRadicadoDto radicado, string usuarioDnp)
        {
            throw new NotImplementedException();
        }

        public Task<RespuestaGeneralDto> GuardarCambiosJustificacionHorizonte(CapituloModificado parametros, string usuarioDnp)
        {
            throw new NotImplementedException();
        }

        public Task<List<JustificacionHorizontenDto>> ObtenerJustificacionHorizonte(int IdProyecto, string usuarioDnp)
        {
            throw new NotImplementedException();
        }


        public Task<CapituloModificado> ObtenerCapitulosModificadosCapitoSeccion(string guiMacroproceso, int idProyecto, Guid idInstancia, string capitulo, string seccion, string usuarioDNP)
        {
            throw new NotImplementedException();
        }

        public Task<List<ErroresProyectoDto>> ObtenerErroresProyecto(string guiMacroproceso, int IdProyecto, string IdInstancia, string usuarioDnp)
        {
            throw new NotImplementedException();
        }

        public async Task<List<ErroresTramiteDto>> ObtenerErroresViabilidad(string guiMacroproceso, int IdProyecto, string IdNivel, string IdInstancia, string usuarioDnp)
        {
            List<ErroresTramiteDto> lista = new List<ErroresTramiteDto>();
            await Task.Run(() =>
            {
                ErroresTramiteDto error = new ErroresTramiteDto();
                error.Seccion = "viabilidadtecnico";
                error.Capitulo = "generales";
                error.Errores = "";
                lista.Add(error);
            });

            return lista;
        }

        public Task<int> TramiteEnPasoUno(Guid InstanciaId, string usuarioDnp)
        {
            throw new NotImplementedException();
        }
        Task<string> IServiciosNegocioServicios.ObtenerFocalizacionPoliticasTransversalesFuentes(string bpin, string usuarioDnp, string tokenAutorizacion)
        {
            throw new NotImplementedException();
        }

        public Task<string> ObtenerDetalleAjustesFuenteFinanciacion(string bpin, string usuarioDNP)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDto<List<TramiteConpesDetailDto>>> ObtenerConpesTramite(int tramiteId, string usuarioDnp)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDto<bool>> AsociarConpesTramite(AsociarConpesTramiteRequestDto model, string usuarioDnp)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDto<bool>> RemoverAsociacionConpesTramite(RemoverAsociacionConpesTramiteDto model, string usuarioDnp)
        {
            throw new NotImplementedException();
        }

        public Task<List<ErroresTramiteDto>> ObtenerErroresTramite(string guiMacroproceso, string IdInstancia, string accionid, string usuarioDnp,bool tieneCDP)
        {
            throw new NotImplementedException();
        }


        public Task<ResponseDto<PeriodoPresidencialDto>> ObtenerPeriodoPresidencial(string usuarioDnp)
        {
            throw new NotImplementedException();
        }

		public Task<IHttpActionResult> guardarLocalizacion(LocalizacionProyectoAjusteDto objLocalizacion, string usuarioDNP, string tokenAutorizacion)
		{
			throw new NotImplementedException();
		}

		public Task<List<DepartamentoCatalogoDto>> obtenerDepartamento(string usuarioDNP)
		{
			throw new NotImplementedException();
		}

		public Task<IHttpActionResult> obtenerMunicipio(ProyectoParametrosDto peticion)
		{
			throw new NotImplementedException();
		}

        public Task<string> EliminarAsociacionVFO(EliminacionAsociacionDto eliminacionAsociacionDto, string usuarioDnp)
        {
            throw new NotImplementedException();
        }

        Task<ResultadoProcedimientoDto> IServiciosNegocioServicios.guardarLocalizacion(LocalizacionProyectoAjusteDto objLocalizacion, string usuarioDNP, string tokenAutorizacion)
        {
            throw new NotImplementedException();
        }

        public Task<List<SeccionesTramiteDto>> ObtenerSeccionesTramite(string IdMacroproceso, string IdInstancia, string FaseId, string usuarioDnp)
        {
            throw new NotImplementedException();
        }

        public Task<List<TramiteProyectoVFODto>> ObtenerProyectoAsociacionVFO(string bpin, int tramite, string tipoTramite, string usuarioDnp)
        {
            throw new NotImplementedException();
        }

        public Task<string> AsociarProyectoVFO(TramiteProyectoVFODto tramiteProyectoVFODto, string usuarioDnp)
        {
            throw new NotImplementedException();
        }

        public Task<SeccionCapituloDto> ObtenerSeccionCapitulo(string faseGuid, string capitulo, string seccion, string idUsuario,string NivelId, string FlujoId)
        {
            throw new NotImplementedException();
        }

        public Task<string> ObtenerPoliticasTransversalesCrucePoliticas(string Bpin, int IdFuente, string usuarioDnp, string tokenAutorizacion)
        {
            throw new NotImplementedException();
        }

        public Task<RespuestaGeneralDto> ActualizarPoliticasTransversalesCrucePoliticas(PoliticasTCrucePoliticasDto parametros, string usuarioDnp)
        {
            throw new NotImplementedException();
        }

        public Task<DatosProyectoTramiteDto> ObtenerDatosProyectoTramite(int tramiteId, string usuarioDnp)
        {
            throw new NotImplementedException();
        }
 	    public Task<string> ObtenerCatalogoReferencia(EntidadesPorCodigoParametrosDto peticion, string nombreCatalogo, int idCatalogo, string nombreCatalogoReferencia)
	    {
		    throw new NotImplementedException();
	    }
	

	    public Task<dynamic> CrearRadicadoEntradaTramite(int tramiteId, string usuarioDnp)
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
            throw new NotImplementedException();
        }

        public Task<List<JustificacionPasoDto>> ObtenerPreguntasProyectoActualizacionPaso(int TramiteId, int ProyectoId, int TipoTramiteId, Guid IdNivel, int TipoRolId, string usuarioDnp)
        {
            throw new System.NotImplementedException();
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

            return null;
        }

        public async Task<string> GuardarInformacionPresupuestalValores(InformacionPresupuestalValoresDto informacionPresupuestalValoresDto, string usuarioDnp)
        {
            try
            {
                string resultado = string.Empty;
                var result = string.Empty;//consulta de base de datos

                await Task.Run(() =>
                {
                    if (string.IsNullOrEmpty(result))
                    {
                        resultado = "OK";

                    }
                    else
                    {
                        var mensajeError = Convert.ToString(result);
                        resultado = mensajeError;
                        throw new Exception(mensajeError);
                    }
                });
                return resultado;

            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion Vigencias Futuras 

        public async Task<List<EnvioSubDireccionDto>> ObtenerSolicitarConceptoPorTramite(int tramiteId, string usuarioDnp)
        {
            List<EnvioSubDireccionDto> lista = new List<EnvioSubDireccionDto>();
            await Task.Run(() =>
            {
                EnvioSubDireccionDto concepto = new EnvioSubDireccionDto();
                concepto.TramiteId = 573;
                concepto.Usuario = "auracanuta@gmail.com";
                concepto.NombreUsuarioDNP = "Aura Carolina Fernández";
                concepto.Enviado = true;
                concepto.NombreEntidad = "SDG - Subdirección de Crédito";
                lista.Add(concepto);
            });


            return lista;
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

                await Task.Run(() =>
                {
                    deflactores = JsonConvert.DeserializeObject<List<TramiteDeflactoresDto>>(jsonString);
                });
                return deflactores;

            }
            catch (Exception e)
            {
                throw;
            }
        }

        public async Task<List<TramiteProyectoDto>> ObtenerProyectoTramite(int ProyectoId, int TramiteId, string usuarioDnp)
        {
            try
            {
                var proyectoTramite = new List<TramiteProyectoDto>();
                string jsonString = "[{'Id':739,'TramiteId':356,'ProyectoId':97706,'EntidadId':186,'PeriodoProyectoId':1315,'Accion':'D','Estado':true,'TipoProyecto':'Credito','NombreProyecto':'Construcción Ampliación de Infraestructura para Generación de Cupos en Los Establecimientos de Reclusión del Orden - Nacional Programa: 1206\n\rSubprograma: 0802','EsConstante':true,'Constante':1,'AnioBase':2015}]";

                await Task.Run(() =>
                {
                    proyectoTramite = JsonConvert.DeserializeObject<List<TramiteProyectoDto>>(jsonString);
                });
                
                return proyectoTramite;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public async Task<string> ActualizaVigenciaFuturaProyectoTramite(TramiteProyectoDto tramiteProyectoDto, string usuarioDnp)
        {
            try
            {
                string resultado = string.Empty;
                var result = string.Empty;//consulta de base de datos

                await Task.Run(() =>
                {
                    if (string.IsNullOrEmpty(result))
                    {
                        resultado = "OK";
                        
                    }
                    else
                    {
                        var mensajeError = Convert.ToString(result);
                        resultado = mensajeError;
                        throw new Exception(mensajeError);
                    }
                });
                return resultado;

            }
            catch (Exception ex)
            {
                throw;
            }
        }

		public async Task<List<AgrupacionCodeDto>> ConsultarAgrupacionesCompleta(string usuarioDNP)
		{
			try
			{
				var lista = new List<AgrupacionCodeDto>();
				string jsonString = "[{'Created':null,'Modified':null,'CreatedBy':'','ModifiedBy':'','Code':'272451575','IsActive':null,'MunicipalityId':404,'TipoAgrupacionId':2,'Id':238,'Name':'Abejero'},{'Created':null,'Modified':null,'CreatedBy':'MigradoWVV','ModifiedBy':null,'Code':'735631804','IsActive':null,'MunicipalityId':990,'TipoAgrupacionId':2,'Id':854,'Name':'Aco Viejo Patio Bonito'},{'Created':null,'Modified':null,'CreatedBy':'','ModifiedBy':'','Code':'867571512','IsActive':null,'MunicipalityId':1096,'TipoAgrupacionId':2,'Id':577,'Name':'Afilador - campoalegre'},{'Created':null,'Modified':null,'CreatedBy':'MigradoWVV','ModifiedBy':null,'Code':'863201774','IsActive':null,'MunicipalityId':1089,'TipoAgrupacionId':2,'Id':877,'Name':'Agua Blanca'},{'Created':null,'Modified':null,'CreatedBy':'','ModifiedBy':'','Code':'270251191','IsActive':null,'MunicipalityId':394,'TipoAgrupacionId':2,'Id':203,'Name':'Agua Clara-bellaluz'},{'Created':null,'Modified':null,'CreatedBy':'','ModifiedBy':'','Code':'194731135','IsActive':null,'MunicipalityId':371,'TipoAgrupacionId':2,'Id':114,'Name':'Agua Negra'},{'Created':null,'Modified':null,'CreatedBy':'','ModifiedBy':'','Code':'865731398','IsActive':null,'MunicipalityId':1093,'TipoAgrupacionId':2,'Id':561,'Name':'Agua Negra'},{'Created':null,'Modified':null,'CreatedBy':'','ModifiedBy':'','Code':'184601076','IsActive':null,'MunicipalityId':1079,'TipoAgrupacionId':2,'Id':53,'Name':'Aguanegra'}]";

                await Task.Run(() =>
                {
                    lista = JsonConvert.DeserializeObject<List<AgrupacionCodeDto>>(jsonString);
                });

                return lista;
			}
			catch (Exception e)
			{
				throw;
            }
        }

        public Task<RespuestaGeneralDto> EliminarCapitulosModificados(CapituloModificado capituloModificado, string usuarioDnp)
        {
            RespuestaGeneralDto resultado = new RespuestaGeneralDto();
            var result = string.Empty;//consulta de base de datos


            capituloModificado.InstanciaId = new Guid("cc912a51-10f9-4b3f-a81a-00b94a8b913d");
            capituloModificado.SeccionCapituloId = 4;
            capituloModificado.ProyectoId = 0;

            if (string.IsNullOrEmpty(result))
            {
                resultado.Exito = true;
                resultado.Mensaje = "Capitulo modificado Eliminado Exitosamente!";

            }
            else
            {
                resultado.Exito = false;
                resultado.Mensaje = "No se elimino la información";

            }
            return Task.FromResult(resultado);
        }

        Task<VigenciaFuturaCorrienteDto> IServiciosNegocioServicios.ObtenerFuentesFinanciacionVigenciaFuturaCorriente(string bpin, string usuarioDnp)
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
           return Task.FromResult(proyectoTramite);
        }

        public async Task<int> EliminarPermisosAccionesUsuarios(string usuarioDestino, int tramiteId, string aliasNivel, string usuarioDnp, Guid InstanciaId = default(Guid))
        {

            await Task.Run(() =>
            {
                return 1;
            });
            return 0;
        }
        public async Task<AccionDto> ObtenerAccionActualyFinal(int tramiteId, string bpin, string usuarioDnp)
        {
            AccionDto respuesta = new AccionDto();
            await Task.Run(() =>
            {
                respuesta.IdAccionActual = new Guid();
                respuesta.IdAccionFinal = new Guid();
                respuesta.NombreAccionActual = "Inicio";
                respuesta.NombreAccionFinal = "Final";
                respuesta.IdFlujo = new Guid();
            });
            return respuesta;
        }

        async Task<VigenciaFuturaResponse> IServiciosNegocioServicios.ActualizarVigenciaFuturaFuente(VigenciaFuturaCorrienteFuenteDto fuente, string usuarioDnp)
        {
            VigenciaFuturaResponse response = new VigenciaFuturaResponse();

            await Task.Run(() =>
            {
                response.Exito = true;
                response.Mensaje = "OK";
            });
            return response;
        }

        async Task<VigenciaFuturaConstanteDto> IServiciosNegocioServicios.ObtenerFuentesFinanciacionVigenciaFuturaConstante(string bpin, int tramiteId, string usuarioDnp)
        {
            try
            {
                var proyectoTramite = new VigenciaFuturaConstanteDto();

                await Task.Run(() =>
                {
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
                });
                return proyectoTramite;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public Task<string> ObtenerTiposRecursosEntidad(ProyectoParametrosDto peticion, int entityTypeCatalogId)
        {
            throw new NotImplementedException();
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

        Task<List<TramiteModalidadContratacionDto>> IServiciosNegocioServicios.ObtenerModalidadesContratacion(int mostrar, string usuarioDnp)
        {
            throw new NotImplementedException();
        }

        Task<ActividadPreContractualDto> IServiciosNegocioServicios.ActualizarActividadesCronograma(ActividadPreContractualDto ModalidadContratacionIdstring, string usuarioDnp)
        {
            throw new NotImplementedException();
        }

        Task<ActividadPreContractualDto> IServiciosNegocioServicios.ObtenerActividadesPrecontractualesProyectoTramite(int ModalidadContratacionId, int ProyectoId, int TramiteId, bool eliminarActividades, string usuarioDnp)
        {
            throw new NotImplementedException();
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

        public Task<string> ObtenerDetalleAjustesJustificaionFacalizacionPT(string bpin, string usuarioDNP)
        {
            return Task.FromResult(string.Empty);
        }

        public Task<List<TipoDocumentoTramiteDto>> ObtenerTipoDocumentoTramitePorNivel(int tipoTramiteId, string nivelId, string rolId, string usuarioDnp)
        {
            var jsonString = "[{'Id':1, 'TipoDocumentoId': 1, 'TipoDocumento': 'Análisis Ecónomico (Supuestos de Costeo)' ,'TipoTramiteId': 2,'Obligatorio':true}]";

            var TipoDocumentos = JsonConvert.DeserializeObject<List<TipoDocumentoTramiteDto>>(jsonString);

            return Task.FromResult(TipoDocumentos);

        }

        public Task<List<int?>> ObtenerListaVigenciasProyecto(ProyectoParametrosDto peticion)
        {
            List<int?> ls = new List<int?>();
            ls.Add(2022);
            ls.Add(2021);
            ls.Add(2020);
            return Task.FromResult(ls);
        }
        public Task<List<DatosUsuarioDto>> ObtenerDatosUsuario(string idUsuarioDnp, int idEntidad, Guid idAccion, Guid idIntancia, string usuarioDnp)
        {
            var jsonString = "[{'IdUsuario':0E935E9E-E124-4B29-BB82-00BD1C6F8F1F', 'NombreUsuario': 'Juan3 Perez'," +
                " 'Cuenta': 'jeruiz' ,'EntidadId': '37C799DD-B3F4-4B0D-9A50-840EA2E9EB0C','RolId':'DA595AA3-CF59-46D3-A22A-0D96DA5C7371'}]" +
                " 'Entidad': 'RAMA JUDICIAL - TRIBUNALES Y JUZGADOS'";

            var datosUsuarioDto = JsonConvert.DeserializeObject<List<DatosUsuarioDto>>(jsonString);
            return Task.FromResult(datosUsuarioDto);

        }

        public Task<dynamic> CerrarRadicadosTramite(string numeroTramite, string usuarioDnp)
        {
            throw new NotImplementedException();
        }

        public Task<ModificacionLeyendaDto> ObtenerModificacionLeyenda(int tramiteId, int ProyectoId, string usuarioDnp)
        {
           ModificacionLeyendaDto modificacionLeyenda = new ModificacionLeyendaDto();
           return Task.FromResult(modificacionLeyenda);
        }

        public Task<string> ActualizarModificacionLeyenda(ModificacionLeyendaDto modificacionLeyendaDto, string usuarioDnp)
        {
            return Task.FromResult("OK");
        }

        public Task<List<EntidadCatalogoDTDto>> ObtenerListaDirecionesDNP(Guid idEntididad, string usuarioDnp)
        {
            List<EntidadCatalogoDTDto> lista = new List<EntidadCatalogoDTDto>();
            EntidadCatalogoDTDto data = new EntidadCatalogoDTDto();
            data.Id = 105;
            data.Name = "Catalogo direccciones";
            lista.Add(data);
            if (idEntididad == Guid.Empty)
                return null;
            else
            {
                return Task.FromResult(lista);
            }
        }

        public Task<List<EntidadCatalogoDTDto>> ObtenerListaSubdirecionesPorParentId(int idEntididadType, string usuarioDnp)
        {
            List<EntidadCatalogoDTDto> lista = new List<EntidadCatalogoDTDto>();
            EntidadCatalogoDTDto data = new EntidadCatalogoDTDto();
            data.Id = 105;
            data.Name = "Catalogo direccciones";
            lista.Add(data);
            if (idEntididadType == 0)
                return null;
            else
            {
                return Task.FromResult(lista);
            }
        }

        public Task<RespuestaGeneralDto> BorrarFirma(string idUsuario)
        {
            RespuestaGeneralDto response = new RespuestaGeneralDto();
            response.Exito = true;
            response.Mensaje = "Exitoso";
            if (string.IsNullOrEmpty(idUsuario))
                return null;
            else
            {
                return Task.FromResult(response);
            }
        }

        Task<List<ConfiguracionUnidadMatrizDTO>> IServiciosNegocioServicios.ObtenerMatrizEntidadDestino(ListMatrizEntidadDestinoDto peticion)
        {
            List<ConfiguracionUnidadMatrizDTO> response = new List<ConfiguracionUnidadMatrizDTO>();
            return Task.FromResult(response);
        }

        public Task<RespuestaGeneralDto> ActualizarMatrizEntidadDestino(ListaMatrizEntidadUnidadDto peticion, string idUsuarioDNP)
        {
            RespuestaGeneralDto response = new RespuestaGeneralDto();
            response.Exito = true;
            response.Mensaje = "Exitoso";
            if (string.IsNullOrEmpty(idUsuarioDNP))
                return null;
            else
            {
                return Task.FromResult(response);
            }
        }
        public Task<ProyectosCartaDto> ObtenerProyectosCartaTramite(int TramiteId, string usuarioDnp)
        {            
            if (TramiteId==0)
                return null;
            else
            {
                ProyectosCartaDto response = new ProyectosCartaDto
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
                return Task.FromResult(response);
            }
        }

        public Task<DetalleCartaConceptoALDto> ObtenerDetalleCartaAL(int TramiteId, string usuarioDnp)
        {
            if (TramiteId == 0)
                return null;
            else
            {
                DetalleCartaConceptoALDto response = new DetalleCartaConceptoALDto
                {
                    Aclaracion = "PROYECTO 1 NUEVO", 
                    NombreActual = "PROYECTO 1"
                };
                return Task.FromResult(response);
            }
        }

        public Task<int> ObtenerAmpliarDevolucionTramite(int ProyectoId, int TramiteId, string usuarioDnp)
        {
            var resultado = 1;
            return Task.FromResult(resultado);
        }

        public Task<DatosProyectoTramiteDto> ObtenerDatosProyectoConceptoPorInstancia(Guid instanciaId, string usuarioDnp)
        {
            if (instanciaId == new Guid())
                return null;
            else
            {
                DatosProyectoTramiteDto resultado = new DatosProyectoTramiteDto
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
                return Task.FromResult(resultado);
            }           
        }

        public Task<List<TramiteLiberacionVfDto>> ObtenerLiberacionVigenciasFuturas(int ProyectoId, int TramiteId, string usuarioDnp)
        {
            var jsonString = "[{'CodigoProceso':'EJ-TP-TL-171800-0001', 'NombreProceso': 'Tramite de Traslado', 'Fecha': '2022-08-10 10:19:04.000', 'Objeto': 'Tramite de Traslado', 'FechaAutorizacion': null, 'CodigoAutorizacion': null }]";

            var datos = JsonConvert.DeserializeObject<List<TramiteLiberacionVfDto>>(jsonString);
            return Task.FromResult(datos);
        }

        public Task<RespuestaGeneralDto> FocalizacionActualizaPoliticasModificadas(JustificacionPoliticaModificada parametros, string idUsuarioDNP)
        {
            RespuestaGeneralDto response = new RespuestaGeneralDto();
            response.Exito = true;
            response.Mensaje = "Exitoso";
            if (string.IsNullOrEmpty(idUsuarioDNP))
                return null;
            else
            {
                return Task.FromResult(response);
            }
        }

        public Task<VigenciaFuturaResponse> InsertaAutorizacionVigenciasFuturas(TramiteALiberarVfDto autorizacion, string usuarioDnp)
        {
            VigenciaFuturaResponse response = new VigenciaFuturaResponse();
            response.Exito = true;
            response.Mensaje = "Exitoso";
            return Task.FromResult(response);
        }

        public Task<VigenciaFuturaResponse> InsertaValoresUtilizadosLiberacionVF(TramiteALiberarVfDto autorizacion, string usuarioDnp)
        {
            VigenciaFuturaResponse response = new VigenciaFuturaResponse();
            response.Exito = true;
            response.Mensaje = "Exitoso";
            return Task.FromResult(response);
        }

        public Task<List<ProyectoTramiteFuenteDto>> ObtenerListaProyectosFuentes(int tramiteId, string usuarioDnp)
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
           return Task.FromResult(response);
        }

        public Task<List<EntidadesAsociarComunDto>> obtenerEntidadAsociarProyecto(Guid InstanciaId, string AccionTramiteProyecto, string usuarioDnp)
        {
            if (AccionTramiteProyecto != null && InstanciaId != new Guid())
            {
                var jsonString = "[{'Id':'1', 'NombreEntidad': 'Mincultura' },{'Id':'2', 'NombreEntidad': 'Minjusticia' }]";

                var datos = JsonConvert.DeserializeObject<List<EntidadesAsociarComunDto>>(jsonString);
                return Task.FromResult(datos);
            }
            else
                return null;
        }

        public Task<CartaConcepto> ConsultarCartaConcepto(int tramiteId, string usuarioDnp)
        {
            var jsonString = "[{" +
                " 'Id' : '2'," +
                "'FaseId' : '42'," +
                "'TramiteId':'902'," +
                " 'RadicadoEntrada' :'309097'," +
                "'RadicadoSalida':3090976," +
                "'FechaCreacion':''25/08/2022'," +
                "'CreadoPor':'Prueba'," +
                "'FechaModificacion':'25/08/2022'," +
                "'ModificadoPor':'prueba'," +
                "'ExpedienteId':'A3987676676'}]";
            var datos = JsonConvert.DeserializeObject<CartaConcepto>(jsonString);
            return Task.FromResult(datos);
        }

        public Task<int> ValidacionPeriodoPresidencial(int tramiteId, string usuarioDnp)
        {
            var jsonString = "1";

            var datos = JsonConvert.DeserializeObject<int>(jsonString);
            return Task.FromResult(datos);
        }

        public async Task<string> GuardarMontosTramite(List<ProyectosEnTramiteDto> proyectosEnTramiteDto, string usuarioDnp)
        {
            try
            {
                string resultado = string.Empty;
                var result = string.Empty;//consulta de base de datos

                await Task.Run(() =>
                {
                    if (string.IsNullOrEmpty(result))
                    {
                        resultado = "OK";

                    }
                    else
                    {
                        var mensajeError = Convert.ToString(result);
                        resultado = mensajeError;
                        throw new Exception(mensajeError);
                    }
                });
                return resultado;

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

        public Task<List<ResumenLiberacionVfDto>> ObtenerResumenLiberacionVigenciasFuturas(int ProyectoId, int TramiteId, string usuarioDnp)
        {
            string jsonString = "[{'TramiteId':906,'ProyectoId':97833,'TotalValoresUtilizados':4939.4700,'ValoresAutorizadosUtilizados':[{'Vigencia':2022,'AprobadosNacion':12854000000.0000,'AprobadosNPropios':10795600000.0000,'UtilizadoNacion':605.8400,'UtilizadoPropios':4333.6300},{'Vigencia':2023,'AprobadosNacion':12000000000.0000,'AprobadosNPropios':10000000000.0000,'UtilizadoNacion':0.0000,'UtilizadoPropios':0.0000},{'Vigencia':2024,'AprobadosNacion':11150000000.0000,'AprobadosNPropios':9210000000.0000,'UtilizadoNacion':0.0000,'UtilizadoPropios':0.0000},{'Vigencia':2025,'AprobadosNacion':11104000000.0000,'AprobadosNPropios':8425600000.0000,'UtilizadoNacion':0.0000,'UtilizadoPropios':0.0000}]}]";// Contexto.uspGetLiberacionVF(ProyectoId, TramiteId).SingleOrDefault();

            var datos = JsonConvert.DeserializeObject<List<ResumenLiberacionVfDto>>(jsonString);
            return Task.FromResult(datos);
        }

        public Task<ValoresUtilizadosLiberacionVfDto> ObtenerValUtilizadosLiberacionVigenciasFuturas(int ProyectoId, int TramiteId, string usuariodnp)
        {
            string jsonString = "{'TramiteId':906,'ProyectoId':97833,'ValoresUtilizadosGeneral':[{'CodigoProceso':'Proceso 1','FechaAutorizacion':'0001-01-01T00:00:00','CodigoAutorizacion':'010101','TotalValorSolicitado':30.0,'TotalTotalValorUtilizado':30.0,'TotalValorUtilizadoProducto':30.0,'ValoresUtilizadosCorrientesProceso':[{'EsConstante':false,'Vigencia':2022,'ValorSolicitado':10.0,'TotalValorUtilizado':10.0,'ValorUtilizadoProducto':10.0},{'EsConstante':false,'Vigencia':2023,'ValorSolicitado':15.0,'TotalValorUtilizado':15.0,'ValorUtilizadoProducto':15.0}],'ValoresUtilizadosConstantesProceso':[{'EsConstante':true,'Vigencia':2022,'AnioBase':2020,'Deflactor':67890.0,'ValorSolicitado':10.0,'TotalValorUtilizado':10.0,'ValorUtilizadoProducto':10.0},{'EsConstante':true,'Vigencia':2023,'AnioBase':0,'Deflactor':0.0,'ValorSolicitado':15.0,'TotalValorUtilizado':15.0,'ValorUtilizadoProducto':15.0}],'ValoresUtilizadosConstantesObjetivo':[{'Objetivo':'Objetivo 1','Productos':[{'NombreProducto':'Producto 1','Etapa':'Etapa1','FechaInicial':'0001-01-01T00:00:00','FechaFinal':'0001-01-01T00:00:00','VigenciasCorrientes':[{'EsConstante':false,'Vigencia':2022,'ValorSolicitado':10.0,'ValorAprobado':10.0,'ValorUtilizado':10.0},{'EsConstante':false,'Vigencia':2023,'ValorSolicitado':15.0,'ValorAprobado':15.0,'ValorUtilizado':15.0}],'VigenciasConstantes':[{'EsConstante':true,'Vigencia':2022,'AnioBase':2020,'Deflactor':5678.0,'ValorSolicitado':20.0,'ValorAprobado':20.0,'ValorUtilizado':20.0},{'EsConstante':true,'Vigencia':2023,'AnioBase':0,'Deflactor':0.0,'ValorSolicitado':25.0,'ValorAprobado':25.0,'ValorUtilizado':25.0}]},{'NombreProducto':'Producto 2','Etapa':'Etapa 2','FechaInicial':'0001-01-01T00:00:00','FechaFinal':'0001-01-01T00:00:00','VigenciasCorrientes':[{'EsConstante':false,'Vigencia':2022,'ValorSolicitado':20.0,'ValorAprobado':20.0,'ValorUtilizado':20.0},{'EsConstante':false,'Vigencia':2023,'ValorSolicitado':25.0,'ValorAprobado':25.0,'ValorUtilizado':25.0}],'VigenciasConstantes':[{'EsConstante':false,'Vigencia':0,'AnioBase':0,'Deflactor':0.0,'ValorSolicitado':0.0,'ValorAprobado':0.0,'ValorUtilizado':0.0},{'EsConstante':false,'Vigencia':0,'AnioBase':0,'Deflactor':0.0,'ValorSolicitado':0.0,'ValorAprobado':0.0,'ValorUtilizado':0.0}]}]},{'Objetivo':'Objetivo 2','Productos':[{'NombreProducto':'Producto 1','Etapa':'Etapa1','FechaInicial':'0001-01-01T00:00:00','FechaFinal':'0001-01-01T00:00:00','VigenciasCorrientes':[{'EsConstante':false,'Vigencia':2022,'ValorSolicitado':10.0,'ValorAprobado':10.0,'ValorUtilizado':10.0},{'EsConstante':false,'Vigencia':2023,'ValorSolicitado':15.0,'ValorAprobado':15.0,'ValorUtilizado':15.0}],'VigenciasConstantes':[{'EsConstante':true,'Vigencia':2022,'AnioBase':2020,'Deflactor':5678.0,'ValorSolicitado':20.0,'ValorAprobado':20.0,'ValorUtilizado':20.0},{'EsConstante':true,'Vigencia':2023,'AnioBase':0,'Deflactor':0.0,'ValorSolicitado':25.0,'ValorAprobado':25.0,'ValorUtilizado':25.0}]},{'NombreProducto':'Producto 2','Etapa':'Etapa 2','FechaInicial':'0001-01-01T00:00:00','FechaFinal':'0001-01-01T00:00:00','VigenciasCorrientes':[{'EsConstante':false,'Vigencia':2022,'ValorSolicitado':20.0,'ValorAprobado':20.0,'ValorUtilizado':20.0},{'EsConstante':false,'Vigencia':2023,'ValorSolicitado':25.0,'ValorAprobado':25.0,'ValorUtilizado':25.0}],'VigenciasConstantes':[{'EsConstante':false,'Vigencia':0,'AnioBase':0,'Deflactor':0.0,'ValorSolicitado':0.0,'ValorAprobado':0.0,'ValorUtilizado':0.0},{'EsConstante':false,'Vigencia':0,'AnioBase':0,'Deflactor':0.0,'ValorSolicitado':0.0,'ValorAprobado':0.0,'ValorUtilizado':0.0}]}]}]},{'CodigoProceso':'Proceso 2','FechaAutorizacion':'0001-01-01T00:00:00','CodigoAutorizacion':'020202','TotalValorSolicitado':30.0,'TotalTotalValorUtilizado':30.0,'TotalValorUtilizadoProducto':30.0,'ValoresUtilizadosCorrientesProceso':[{'EsConstante':false,'Vigencia':2022,'ValorSolicitado':10.0,'TotalValorUtilizado':10.0,'ValorUtilizadoProducto':10.0},{'EsConstante':false,'Vigencia':2023,'ValorSolicitado':15.0,'TotalValorUtilizado':15.0,'ValorUtilizadoProducto':15.0}],'ValoresUtilizadosConstantesProceso':[{'EsConstante':true,'Vigencia':2022,'AnioBase':2020,'Deflactor':67890.0,'ValorSolicitado':10.0,'TotalValorUtilizado':10.0,'ValorUtilizadoProducto':10.0},{'EsConstante':true,'Vigencia':2023,'AnioBase':0,'Deflactor':0.0,'ValorSolicitado':15.0,'TotalValorUtilizado':15.0,'ValorUtilizadoProducto':15.0}],'ValoresUtilizadosConstantesObjetivo':[{'Objetivo':'Objetivo 1','Productos':[{'NombreProducto':'Producto 1','Etapa':'Etapa1','FechaInicial':'0001-01-01T00:00:00','FechaFinal':'0001-01-01T00:00:00','VigenciasCorrientes':[{'EsConstante':false,'Vigencia':2022,'ValorSolicitado':10.0,'ValorAprobado':10.0,'ValorUtilizado':10.0},{'EsConstante':false,'Vigencia':2023,'ValorSolicitado':15.0,'ValorAprobado':15.0,'ValorUtilizado':15.0}],'VigenciasConstantes':[{'EsConstante':true,'Vigencia':2022,'AnioBase':2020,'Deflactor':5678.0,'ValorSolicitado':20.0,'ValorAprobado':20.0,'ValorUtilizado':20.0},{'EsConstante':true,'Vigencia':2023,'AnioBase':0,'Deflactor':0.0,'ValorSolicitado':25.0,'ValorAprobado':25.0,'ValorUtilizado':25.0}]},{'NombreProducto':'Producto 2','Etapa':'Etapa 2','FechaInicial':'0001-01-01T00:00:00','FechaFinal':'0001-01-01T00:00:00','VigenciasCorrientes':[{'EsConstante':false,'Vigencia':2022,'ValorSolicitado':20.0,'ValorAprobado':20.0,'ValorUtilizado':20.0},{'EsConstante':false,'Vigencia':2023,'ValorSolicitado':25.0,'ValorAprobado':25.0,'ValorUtilizado':25.0}],'VigenciasConstantes':[{'EsConstante':false,'Vigencia':0,'AnioBase':0,'Deflactor':0.0,'ValorSolicitado':0.0,'ValorAprobado':0.0,'ValorUtilizado':0.0},{'EsConstante':false,'Vigencia':0,'AnioBase':0,'Deflactor':0.0,'ValorSolicitado':0.0,'ValorAprobado':0.0,'ValorUtilizado':0.0}]}]},{'Objetivo':'Objetivo 2','Productos':[{'NombreProducto':'Producto 1','Etapa':'Etapa1','FechaInicial':'0001-01-01T00:00:00','FechaFinal':'0001-01-01T00:00:00','VigenciasCorrientes':[{'EsConstante':false,'Vigencia':2022,'ValorSolicitado':10.0,'ValorAprobado':10.0,'ValorUtilizado':10.0},{'EsConstante':false,'Vigencia':2023,'ValorSolicitado':15.0,'ValorAprobado':15.0,'ValorUtilizado':15.0}],'VigenciasConstantes':[{'EsConstante':true,'Vigencia':2022,'AnioBase':2020,'Deflactor':5678.0,'ValorSolicitado':20.0,'ValorAprobado':20.0,'ValorUtilizado':20.0},{'EsConstante':true,'Vigencia':2023,'AnioBase':0,'Deflactor':0.0,'ValorSolicitado':25.0,'ValorAprobado':25.0,'ValorUtilizado':25.0}]},{'NombreProducto':'Producto 2','Etapa':'Etapa 2','FechaInicial':'0001-01-01T00:00:00','FechaFinal':'0001-01-01T00:00:00','VigenciasCorrientes':[{'EsConstante':false,'Vigencia':2022,'ValorSolicitado':20.0,'ValorAprobado':20.0,'ValorUtilizado':20.0},{'EsConstante':false,'Vigencia':2023,'ValorSolicitado':25.0,'ValorAprobado':25.0,'ValorUtilizado':25.0}],'VigenciasConstantes':[{'EsConstante':false,'Vigencia':0,'AnioBase':0,'Deflactor':0.0,'ValorSolicitado':0.0,'ValorAprobado':0.0,'ValorUtilizado':0.0},{'EsConstante':false,'Vigencia':0,'AnioBase':0,'Deflactor':0.0,'ValorSolicitado':0.0,'ValorAprobado':0.0,'ValorUtilizado':0.0}]}]}]}]}";// Contexto.uspGetLiberacionVF(ProyectoId, TramiteId).SingleOrDefault();

            var datos = JsonConvert.DeserializeObject<ValoresUtilizadosLiberacionVfDto>(jsonString);
            return Task.FromResult(datos);
        }

        public Task<int> TramiteAjusteEnPasoUno(int tramiteId, int proyectoId, string usuarioDnp)
        {
            var resultado = 1;
            return Task.FromResult(resultado);
        }

        public Task<List<ProyectoTramiteFuenteDto>> ObtenerListaProyectosFuentesAprobado(int tramiteId, string usuarioDnp)
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
            return Task.FromResult(response);
        }

        public Task<AlcanceTramiteMGADto> CrearAlcanceTramite(AlcanceTramiteDto alcanceTramite, string usuarioDnp)
        {
            AlcanceTramiteMGADto resultado = new AlcanceTramiteMGADto();

            return Task.FromResult(resultado);
        }

        public Task<List<TipoMotivoAnulacionDto>> ObtenerTiposMotivoAnulacion(string usuarioDnp)
        {
            List<TipoMotivoAnulacionDto> resultado = new List<TipoMotivoAnulacionDto>();

            return Task.FromResult(resultado);
        }

        public Task<RespuestaGeneralDto> ActualizarCargueMasivo(ObjetoNegocioDto contenido, string usuarioDnp)
        {
            throw new NotImplementedException();
        }

        public Task<string> ConsultarCargueExcel(ObjetoNegocioDto contenido, string usuarioDnp)
        {
            throw new NotImplementedException();
        }

        public Task<VigenciaFuturaResponse> InsertaValoresproductosLiberacionVFCorrientes(DetalleProductosCorrientesDto productosCorrientes, string usuarioDnp)
        {
            VigenciaFuturaResponse response = new VigenciaFuturaResponse();
            response.Exito = true;
            response.Mensaje = "OK";

            return Task.FromResult(response);
        }

        public Task<VigenciaFuturaResponse> InsertaValoresproductosLiberacionVFConstantes(DetalleProductosConstantesDto productosConstantes, string usuarioDnp)
        {
            VigenciaFuturaResponse response = new VigenciaFuturaResponse();
            response.Exito = true;
            response.Mensaje = "OK";

            return Task.FromResult(response);
        }

        public Task<List<EntidadesAsociarComunDto>> ObtenerEntidadTramite(string numeroTramite, string usuarioDnp)
        {
            if (!string.IsNullOrEmpty(numeroTramite))
            {
                var jsonString = "[{'Id':'1', 'NombreEntidad': 'Mincultura' },{'Id':'2', 'NombreEntidad': 'Minjusticia' }]";

                var datos = JsonConvert.DeserializeObject<List<EntidadesAsociarComunDto>>(jsonString);
                return Task.FromResult(datos);
            }
            else
                return null;
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

        public Task<List<ErroresProyectoDto>> ObtenerErroresSeguimiento(string guiMacroproceso, int IdProyecto, string IdInstancia, string usuarioDnp)
        {
            List<ErroresProyectoDto> response = new List<ErroresProyectoDto>();
            ErroresProyectoDto r = new ErroresProyectoDto();
            response.Add(r);

            return Task.FromResult(response);
        }

        public Task<string> PermisosAccionPaso(AccionFlujoDto parametros)
        {
            throw new NotImplementedException();
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

        public Task<SystemConfigurationDto> ConsultarSystemConfiguracion(string VariableKey, string Separador, string usuarioDnp)
        {
            throw new NotImplementedException();
        }

        public Task<string> ObtenerResumenReprogramacionPorProductoVigencia(Guid InstanciaId, int TramiteId, int? ProyectoId, string usuarioDnp)
        {
            string jsonString = "{'ProyectoId':114913,'TramiteId':2201,'TramiteLiberarId':1932,'CodigoProceso':'EJ-TP-VFO-360101-0002','FechaAutorizacion':'2022-11-30','CodigoAutorizacion':'2-2022-056215','EsConstante':false,'ResumenTramite':[{'añoBase':0,'Valores':[{'Vigencia':2023,'Deflactor':null,'UtilizadoNacion':4251542110.00,'UtilizadoPropios':4251542110.00,'ReprogramadoNacion':0.00,'ReprogramadoPropios':0.00,'ReprogramadoNacionPorProducto':0.00,'ReprogramadoPropiosPorProducto':0.00}],'ValoresCorrientes':null}],'Objetivos':[{'ObjetivoEspecificoId':3786,'ObjetivoEspecifico':'Fortalecer el proceso de atención al ciudadano y mejorar los tiempos de respuesta','Productos':[{'ProductoId':8537,'NombreProducto':'Servicios de información actualizados','Etapa':'Inversión','Valores':[{'PeriodoProyectoId':13969,'Vigencia':2022,'Deflactor':null,'UtilizadoNacion':41086834.00,'UtilizadoPropios':41086834.00,'ReprogramadoNacion':null,'ReprogramadoPropios':null},{'PeriodoProyectoId':13970,'Vigencia':2023,'Deflactor':null,'UtilizadoNacion':2601864066.00,'UtilizadoPropios':2601864066.00,'ReprogramadoNacion':0.00,'ReprogramadoPropios':0.00},{'PeriodoProyectoId':13971,'Vigencia':2024,'Deflactor':null,'UtilizadoNacion':1785289220.00,'UtilizadoPropios':1785289220.00,'ReprogramadoNacion':null,'ReprogramadoPropios':null},{'PeriodoProyectoId':13972,'Vigencia':2025,'Deflactor':null,'UtilizadoNacion':0.00,'UtilizadoPropios':0.00,'ReprogramadoNacion':null,'ReprogramadoPropios':null}],'ValoresCorrientes':null}]},{'ObjetivoEspecificoId':3991,'ObjetivoEspecifico':'Mejorar los niveles de trazabilidad, consulta y almacenamiento de los documentos del ministerio.','Productos':[{'ProductoId':9548,'NombreProducto':'Servicio de Gestión Documental','Etapa':'Inversión','Valores':[{'PeriodoProyectoId':13969,'Vigencia':2022,'Deflactor':null,'UtilizadoNacion':102928325.00,'UtilizadoPropios':102928325.00,'ReprogramadoNacion':null,'ReprogramadoPropios':null},{'PeriodoProyectoId':13970,'Vigencia':2023,'Deflactor':null,'UtilizadoNacion':1649678044.00,'UtilizadoPropios':1649678044.00,'ReprogramadoNacion':0.00,'ReprogramadoPropios':0.00},{'PeriodoProyectoId':13971,'Vigencia':2024,'Deflactor':null,'UtilizadoNacion':1751760480.00,'UtilizadoPropios':1751760480.00,'ReprogramadoNacion':null,'ReprogramadoPropios':null},{'PeriodoProyectoId':13972,'Vigencia':2025,'Deflactor':null,'UtilizadoNacion':1169811142.00,'UtilizadoPropios':1169811142.00,'ReprogramadoNacion':null,'ReprogramadoPropios':null},{'PeriodoProyectoId':13973,'Vigencia':2026,'Deflactor':null,'UtilizadoNacion':665390105.00,'UtilizadoPropios':665390105.00,'ReprogramadoNacion':null,'ReprogramadoPropios':null}],'ValoresCorrientes':null}]}]}";

            return Task.FromResult(jsonString);
        }

        public Task<int> ObtenerModalidadContratacionVigenciasFuturas(int ProyectoId, int TramiteId, string usuarioDn)
        {
            int modalidad = 2;
            return Task.FromResult(modalidad);
        }

        public Task<List<TramiteRVFAutorizacionDto>> ObtenerAutorizacionesParaReprogramacion(string bpin, int tramiteId, string tipoTramite, string usuarioDnp)
        {
            // throw new System.NotImplementedException();

            if (tramiteId == 0)
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

        public Task<TramiteRVFAutorizacionDto> ObtenerAutorizacionAsociada(int tramiteId, string usuarioDnp)
        {
            // throw new System.NotImplementedException();

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
            string resultado = string.Empty;
            if (string.IsNullOrEmpty(usuarioDnp))
                resultado = "OK";
            else
            {
                var mensajeError = Convert.ToString("no se encontraron datos para eliminar");
                resultado = mensajeError;
                throw new Exception(mensajeError);
            }
            return Task.FromResult(resultado);
        }

        public Task<string> ConsultarDatosProgramacionDistribucion(int EntidadDestinoId, int TramiteId, string usuarioDnp)
        {
            string jsonString = "{'TramiteId':1179,'ResumenProyectos':[{'TipoOperacion':'Contracredito','TotalTipoOperacion':0,'Proyectos':[{'ProyectoId':113417,'CodigoBpin':'2018011000678','NombreProyecto':'FORTALECIMIENTO DEL SISTEMA DE SEGURIDAD INTEGRAL MARÍTIMA Y FLUVIAL A NIVEL  NACIONAL','NombreProyectoCorto':'FORTALECIMIENTO DEL SISTEMA DE SEGURIDAD INTEGRAL MARÍTIMA Y FLUVIAL A NIVEL  NA','CodigoPresupuestal':'1501121504010000100000','TotalSolicitadoNacionCSF':0.00,'TotalSolicitadoPropiosCSF':0.00,'TotalSolicitadoNacionSSF':0.00,'TotalSolicitadoPropiosSSF':0.00,'TotalAprobadoNacion':0.00,'TotalAprobadoPropios':0.00}]},{'TipoOperacion':'Credito','TotalTipoOperacion':0,'Proyectos':[{'ProyectoId':111992,'CodigoBpin':'2018011000607','NombreProyecto':'IMPLEMENTACIÓN DEL PLAN NACIONAL DE INFRAESTRUCTURA A NIVEL  NACIONAL','NombreProyectoCorto':'IMPLEMENTACIÓN DEL PLAN NACIONAL DE INFRAESTRUCTURA A NIVEL  NACIONAL','CodigoPresupuestal':'1501121504010000080000','TotalSolicitadoNacionCSF':0.00,'TotalSolicitadoPropiosCSF':0.00,'TotalSolicitadoNacionSSF':0.00,'TotalSolicitadoPropiosSSF':0.00,'TotalAprobadoNacion':0.00,'TotalAprobadoPropios':0.00}]}],'ProyectosAsociados':[{'ProyectoId':111992,'CodigoBpin':'2018011000607','NombreProyecto':'IMPLEMENTACIÓN DEL PLAN NACIONAL DE INFRAESTRUCTURA A NIVEL  NACIONAL','NombreProyectoCorto':'IMPLEMENTACIÓN DEL PLAN NACIONAL DE INFRAESTRUCTURA A NIVEL  NACIONAL','EntidadFinanciadora':'MINISTERIO DE DEFENSA NACIONAL - DIRECCION GENERAL MARITIMA - DIMAR','NombreSector':'Defensa y Policía','TipoProyecto':'Credito','CodigoPresupuestal':'1501121504010000080000','VigenciaInicial':2019,'VigenciaFinal':2029,'TotalApropiacionInicialNacion':0.00,'TotalApropiacionInicialPropios':0.00,'TotalApropiacionVigenteNacion':0.00,'TotalApropiacionVigentePropios':0.00,'TotalVigenciasFuturasNacion':0.00,'TotalVigenciasFuturasPropios':0.00,'MontoTramiteNacion':0.00,'MontoTramitePropios':0.00,'DetalleFuentes':[{'TipoRecursoId':6,'NombreTipoRecurso':'15-Donaciones-Nación','ValorInicialCSF':0.00,'ValorInicialSSF':0.00,'ValorVigenteCSF':0.00,'ValorVigenteSSF':0.00,'ValorIncorporarCSF':0.00,'ValorIncorporarSSF':0.00,'ValorIncorporarAprobadoCSF':0.00,'ValorIncorporarAprobadoSSF':0.00},{'TipoRecursoId':14,'NombreTipoRecurso':'25-Donaciones-Propios','ValorInicialCSF':0.00,'ValorInicialSSF':0.00,'ValorVigenteCSF':0.00,'ValorVigenteSSF':0.00,'ValorIncorporarCSF':0.00,'ValorIncorporarSSF':0.00,'ValorIncorporarAprobadoCSF':0.00,'ValorIncorporarAprobadoSSF':0.00}]},{'ProyectoId':111992,'CodigoBpin':'2018011000607','NombreProyecto':'IMPLEMENTACIÓN DEL PLAN NACIONAL DE INFRAESTRUCTURA A NIVEL  NACIONAL','NombreProyectoCorto':'IMPLEMENTACIÓN DEL PLAN NACIONAL DE INFRAESTRUCTURA A NIVEL  NACIONAL','EntidadFinanciadora':'MINISTERIO DE DEFENSA NACIONAL - DIRECCION GENERAL MARITIMA - DIMAR','NombreSector':'Defensa y Policía','TipoProyecto':'Credito','CodigoPresupuestal':'1501121504010000080000','VigenciaInicial':2019,'VigenciaFinal':2029,'TotalApropiacionInicialNacion':0.00,'TotalApropiacionInicialPropios':0.00,'TotalApropiacionVigenteNacion':0.00,'TotalApropiacionVigentePropios':0.00,'TotalVigenciasFuturasNacion':0.00,'TotalVigenciasFuturasPropios':0.00,'MontoTramiteNacion':3200000000.00,'MontoTramitePropios':0.00,'DetalleFuentes':[{'TipoRecursoId':6,'NombreTipoRecurso':'15-Donaciones-Nación','ValorInicialCSF':0.00,'ValorInicialSSF':0.00,'ValorVigenteCSF':0.00,'ValorVigenteSSF':0.00,'ValorIncorporarCSF':0.00,'ValorIncorporarSSF':0.00,'ValorIncorporarAprobadoCSF':0.00,'ValorIncorporarAprobadoSSF':0.00},{'TipoRecursoId':14,'NombreTipoRecurso':'25-Donaciones-Propios','ValorInicialCSF':0.00,'ValorInicialSSF':0.00,'ValorVigenteCSF':0.00,'ValorVigenteSSF':0.00,'ValorIncorporarCSF':0.00,'ValorIncorporarSSF':0.00,'ValorIncorporarAprobadoCSF':0.00,'ValorIncorporarAprobadoSSF':0.00}]}],'ProyectosAportantes':null}";

            return Task.FromResult(jsonString);
        }

        public async Task<RespuestaGeneralDto> GuardarDatosProgramacionDistribucion(ProgramacionDistribucionDto programacionDistribucion, string usuarioDnp)
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

        public async Task<List<ErroresTramiteDto>> ObtenerErroresProgramacion(string IdNivel, string IdInstancia, string usuarioDnp)
        {
            List<ErroresTramiteDto> lista = new List<ErroresTramiteDto>();
            await Task.Run(() =>
            {
                ErroresTramiteDto error = new ErroresTramiteDto();
                error.Seccion = "viabilidadtecnico";
                error.Capitulo = "generales";
                error.Errores = "";
                lista.Add(error);
            });

            return lista;
        }

        public async Task<RespuestaGeneralDto> GuardarDatosInclusion(ProgramacionDistribucionDto programacionDistribucion, string usuarioDnp)
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
        public Task<EncabezadoSGRDto> ObtenerEncabezadoSGR(ProyectoParametrosEncabezadoDto parametros, string usuarioDnp)
        {
            EncabezadoSGRDto response = new EncabezadoSGRDto();
            response.CodigoBPIN = "98104";
            response.Id = 98104;
            response.Nombre = "Proyecto de prueba 1";
            response.EntidadDestino = "Meta";
            response.EntidadDestino = "Meta";
            response.VigenciaInicial = 2022;
            response.VigenciaFinal = 2024;
            response.Valor = 1000000;
            response.AnioEstudio = 2022;
            response.Sector = "Empleo público";
            response.TipoEntidadPresenta = "Departamentos";
            response.ProyectoTipo = "";
            response.ObjetivoGeneral = "Objetivo general de prueba";
            response.PoblacionObjetivo = 473;
            response.Alcance = "Alcance de prueba";
            response.AjustesRealizados = 0;
            response.PuntajeSEP = 0;
            response.ValorTotalInversion = 0;
            response.ValorTotalOperacion = 0;
            response.ValorTotalPreInversion = 0;
            response.FechaPresentacion = new DateTime(2022, 11, 26);
            response.ProgramaPresupuestal = "Programa presupuestal de prueba";
            response.SubProgramaPresupuestal = "Subprograma presupuestal de prueba";

            return Task.FromResult(response);
        }

        public Task<EncabezadoSGPDto> ObtenerEncabezadoSGP(ProyectoParametrosEncabezadoDto parametros, string usuarioDnp)
        {
            EncabezadoSGPDto response = new EncabezadoSGPDto();
            response.CodigoBPIN = "98104";
            response.Id = 98104;
            response.Nombre = "Proyecto de prueba 1";
            response.EntidadDestino = "Meta";
            response.EntidadDestino = "Meta";
            response.VigenciaInicial = 2022;
            response.VigenciaFinal = 2024;
            response.Valor = 1000000;
            response.AnioEstudio = 2022;
            response.Sector = "Empleo público";
            response.TipoEntidadPresenta = "Departamentos";
            response.ProyectoTipo = "";
            response.ObjetivoGeneral = "Objetivo general de prueba";
            response.PoblacionObjetivo = 473;
            response.Alcance = "Alcance de prueba";
            response.AjustesRealizados = 0;
            response.PuntajeSEP = 0;
            response.ValorTotalInversion = 0;
            response.ValorTotalOperacion = 0;
            response.ValorTotalPreInversion = 0;
            response.FechaPresentacion = new DateTime(2022, 11, 26);
            response.ProgramaPresupuestal = "Programa presupuestal de prueba";
            response.SubProgramaPresupuestal = "Subprograma presupuestal de prueba";

            return Task.FromResult(response);
        }

        //TramiteSGP

        public Task<RespuestaGeneralDto> ActualizarEstadoAjusteProyectoSGP(string tipoDevolucion, string objetoNegocioId, int tramiteId, string observacion, string usuarioDnp)
        {
            throw new NotImplementedException();
        }

        public Task<RespuestaGeneralDto> EliminarProyectoTramiteNegocioSGP(InstanciaTramiteDto instanciaTramiteDto)
        {
            throw new System.NotImplementedException();
        }

        //TramiteSGP - Información Presupuestal

        public Task<RespuestaGeneralDto> GuardarTramiteInformacionPresupuestalSGP(List<TramiteFuentePresupuestalDto> parametros, string usuarioDnp)
        {
            throw new System.NotImplementedException();
        }

        public Task<List<ProyectoTramiteFuenteDto>> ObtenerListaProyectosFuentesSGP(int tramiteId, string usuarioDnp)
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
            return Task.FromResult(response);
        }

        public Task<RespuestaGeneralDto> GuardarFuentesTramiteProyectoAprobacionSGP(List<FuentesTramiteProyectoAprobacionDto> parametros, string usuarioDnp)
        {
            throw new System.NotImplementedException();
        }

        public Task<List<ProyectoTramiteFuenteDto>> ObtenerListaProyectosFuentesAprobadoSGP(int tramiteId, string usuarioDnp)
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
            return Task.FromResult(response);
        }

        public Task<RespuestaGeneralDto> GuardarTramiteTipoRequisitoSGP(List<TramiteRequitoDto> parametros, string usuarioDnp)
        {
            throw new System.NotImplementedException();
        }

        public Task<List<Dominio.Dto.Tramites.Proyectos.ProyectoRequisitoDto>> ObtenerProyectoRequisitosPorTramiteSGP(int pTramiteProyectoId, int? pProyectoRequisitoId, string usuarioDnp, bool isCDP)
        {
            throw new System.NotImplementedException();
        }

        public Task<List<ProyectosEnTramiteDto>> ObtenerProyectosTramiteNegocioSGP(int TramiteId, string usuarioDnpo, string TokenAutorizacion)
        {
            throw new System.NotImplementedException();
        }

        public Task<RespuestaGeneralDto> GuardarProyectosTramiteNegocioSGP(DatosTramiteProyectosDto parametros, string usuarioDnp)
        {
            throw new System.NotImplementedException();
        }

        public Task<string> ValidacionProyectosTramiteNegocio(int TramiteId, string usuarioDnpo, string TokenAutorizacion)
        {
            throw new System.NotImplementedException();
        }

        public  Task<string> ObtenerDatosAdicionSgp(int tramiteId, string usuarioDnp)
        {
            throw new System.NotImplementedException();
        }


        public  Task<string> GuardarDatosAdicionSgp(ConvenioDonanteDto objConvenioDonanteDto, string usuario)
        {
            throw new System.NotImplementedException();
        }


        public  Task<string> EiliminarDatosAdicionSgp(ConvenioDonanteDto objConvenioDonanteDto, string usuario)
        {
            throw new System.NotImplementedException();
        }
    }
}

