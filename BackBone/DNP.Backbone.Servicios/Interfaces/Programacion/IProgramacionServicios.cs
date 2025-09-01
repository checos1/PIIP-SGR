using DNP.Backbone.Comunes.Dto;
using DNP.Backbone.Dominio.Dto.Programacion;
using DNP.Backbone.Dominio.Dto.Tramites.ProgramacionDistribucion;
using DNP.Backbone.Dominio.Dto.SeguimientoControl;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using DNP.Backbone.Dominio.Dto.Focalizacion;
using DNP.Backbone.Dominio.Dto.Tramites;

namespace DNP.Backbone.Servicios.Interfaces.Programacion
{
    public interface IProgramacionServicios
    {
        Task<RespuestaGeneralDto> GuardarProgramacion(ProgramacionDto programacionDto, string usuarioDnp);
        Task<RespuestaGeneralDto> GuardarProgramacionExcepcion(ProgramacionExcepcionDto programacionDto, string usuarioDnp);
        Task<RespuestaGeneralDto> EliminarProgramacion(ProgramacionDto programacionDto, string usuarioDnp);
        Task<IEnumerable<ProgramacionDto>> ObtenerProgramaciones(string tipoEntidad, Guid? capituloId, DateTime? fechaInicio, DateTime? fechaFin, Dominio.Enums.EstadoProceso? estado, string usuarioDnp);
        Task<IEnumerable<ProgramacionExcepcionDto>> ObtenerProgramacionExcepciones(int idProgramacion, string usuarioDnp);
        Task<RespuestaGeneralDto> EditarProgramacionExcepcion(ProgramacionExcepcionDto programacionDto, string usuarioDnp);
        Task<RespuestaGeneralDto> EliminarProgramacionExcepcion(ProgramacionExcepcionDto programacionDto, string usuarioDnp);
        Task<RespuestaGeneralDto> CrearPeriodo(string tipoEntidad, string usuarioDnp);
        Task<RespuestaGeneralDto> IniciarProceso(string tipoEntidad, string usuarioDnp);
        Task<RespuestaGeneralDto> GuardarConfiguracionMensaje(dynamic configuracionMensaje, string usuarioDnp);
        Task<IEnumerable> ObtenerTipoEstadoProceso();
        Task<string> ObtenerDatosProgramacionEncabezado(int EntidadDestinoId, int TramiteId, string origen, string usuarioDnp);
        Task<string> ObtenerDatosProgramacionDetalle(int TramiteProyectoId, string origen, string usuarioDnp);
        Task<RespuestaGeneralDto> GuardarDatosProgramacionDistribucion(ProgramacionDistribucionDto programacionDistribucion, string usuarioDnp);
        Task<RespuestaGeneralDto> GuardarDatosProgramacionFuentes(ProgramacionFuenteDto programacionFuente, string usuarioDnp);
        Task<string> ObtenerCargaMasivaCreditos(string usuarioDnp);
        Task<string> ValidarCargaMasivaCreditos(dynamic validarCargaMasivaCreditos, string usuarioDnp);
        Task<ReponseHttp> RegistrarCargaMasivaCreditos(dynamic registrarCargaMasivaCreditos, string usuarioDnp);
        Task<string> ObtenerCargaMasivaCuotas(int? Vigencia, int? EntityTypeCatalogOptionId, string usuarioDnp);
        Task<string> ValidarCargaMasivaCuotas(dynamic validarCargaMasivaCuotas, string usuarioDnp);
        Task<ReponseHttp> RegistrarCargaMasivaCuotas(dynamic registrarCargaMasivaCuotas, string usuarioDnp);
        Task<string> ConsultarProyectoGenerarPresupuestal(int sectorId, int entidadId, string proyectoId, string usuarioDnp);
        Task<string> ObtenerProgramacionSectores(int sectorId, string usuarioDnp);
        Task<string> ObtenerProgramacionEntidadesSector(int sectorId, string usuarioDnp);
        Task<string> ObtenerCalendarioProgramacion(Guid FlujoId, string usuarioDnp);
        Task<TramitesResultado> RegistrarProyectosSinPresupuestal(List<ProyectoSinPresupuestalDto> proyectoSinPresupuestalDto, string usuarioDNP);
        Task<RespuestaGeneralDto> RegistrarCalendarioProgramacion(List<CalendarioProgramacionDto> calendarioProgramacionDto, string usuarioDNP);
        Task<RespuestaGeneralDto> ValidarCalendarioProgramacion(int? entityTypeCatalogOptionId, Nullable<Guid> nivelId, Nullable<int> seccionCapituloId, string usuarioDnp);
        Task<string> ValidarConsecutivoPresupuestal(dynamic validarConsecutivoPresupuestal, string usuarioDnp);
        Task<RespuestaGeneralDto> GuardarDatosProgramacionIniciativa(ProgramacionIniciativaDto programacionIniciativa, string usuarioDnp);
        Task<RespuestaGeneralDto> GuardarProgramacionRegionalizacion(ProgramacionRegionalizacionDto programacionRegionalizacionDto, string usuarioDnp);
        Task<string> ConsultarPoliticasTransversalesProgramacion(string Bpin, string usuarioDnp);
        Task<RespuestaGeneralDto> AgregarPoliticasTransversalesProgramacion(IncluirPoliticasDto objIncluirPoliticasDto, string usuarioDnp);
        Task<string> ConsultarPoliticasTransversalesCategoriasProgramacion(string Bpin, string usuarioDnp);
        Task<RespuestaGeneralDto> EliminarPoliticasProyectoProgramacion(int tramiteidProyectoId, int politicaId, string usuarioDnp);
        Task<RespuestaGeneralDto> AgregarCategoriasPoliticaTransversalesProgramacion(FocalizacionCategoriasDto objIncluirPoliticasDto, string usuarioDnp);
        Task<RespuestaGeneralDto> EliminarCategoriaPoliticasProyectoProgramacion(int proyectoId, int politicaId, int categoriaId, string usuarioDnp);
        Task<string> ObtenerCrucePoliticasProgramacion(string Bpin, string usuarioDnp);
        Task<string> PoliticasSolicitudConceptoProgramacion(string Bpin, string usuarioDnp);
        Task<RespuestaGeneralDto> GuardarCrucePoliticasProgramacion(List<CrucePoliticasAjustesDto> parametrosGuardar, string usuarioDnp);
        Task<RespuestaGeneralDto> SolicitarConceptoDTProgramacion(List<FocalizacionSolicitarConceptoDto> parametrosGuardar, string usuarioDnp);
        Task<string> ObtenerResumenSolicitudConceptoProgramacion(string Bpin, string usuarioDnp);
        Task<string> ObtenerDatosProgramacionProducto(int TramiteId, string usuarioDnp);
        Task<RespuestaGeneralDto> GuardarDatosProgramacionProducto(ProgramacionProductoDto programacionProducto, string usuarioDnp);
        Task<string> ObtenerProgramacionBuscarProyecto(int EntidadDestinoId, int tramiteid, string bpin, string NombreProyecto, string usuarioDnp);
        Task<RespuestaGeneralDto> BorrarTramiteProyecto(ProgramacionDistribucionDto programacionDistribucion, string usuarioDnp);
        Task<InboxTramite> ObtenerInboxProgramacionConsolaProcesos(InstanciaTramiteDto instanciaTramiteDto, string usuarioDNP);
        Task<RespuestaGeneralDto> GuardarPoliticasTransversalesCategoriasProgramacion(PoliticasTransversalesCategoriasProgramacionDto objIncluirPoliticasDto, string usuarioDnp);
        Task<RespuestaGeneralDto> EliminarCategoriasProyectoProgramacion(EliminarCategoriasProyectoProgramacionDto objIncluirPoliticasDto, string usuarioDnp);
        Task<RespuestaGeneralDto> GuardarDatosInclusion(ProgramacionDistribucionDto programacionDistribucion, string usuarioDnp);
        Task<string> ConsultarPoliticasTransversalesCategoriasModificaciones(string Bpin, string usuarioDnp);
        Task<RespuestaGeneralDto> GuardarPoliticasTransversalesCategoriasModificaciones(PoliticasTransversalesCategoriasProgramacionDto objIncluirPoliticasDto, string usuarioDnp);

        #region cargue masivo saldos

        Task<string> ConsultarPoliticasTransversalesAprobacionesModificaciones(string Bpin, string usuarioDnp);
        Task<RespuestaGeneralDto> RegistrarCargaMasivaSaldos(int TipoCargueId, string usuarioDnp);
        Task<string> ObtenerLogErrorCargaMasivaSaldos(int? TipoCargueDetalleId, int? CarguesIntegracionId, string usuarioDnp);
        Task<string> ObtenerCargaMasivaSaldos(string TipoCargue, string usuarioDnp);
        Task<string> ObtenerTipoCargaMasiva(string TipoCargue, string usuarioDnp);
        Task<RespuestaGeneralDto> ValidarCargaMasiva(dynamic jsonListaRegistros, string usuarioDnp);
        Task<string> ObtenerDetalleCargaMasivaSaldos(int? CargueId, string usuarioDnp);

        #endregion cargue masivo saldos


        Task<string> ConsultarCatalogoIndicadoresPolitica(string PoliticaId, string Criterio, string usuarioDnp);
        Task<RespuestaGeneralDto> GuardarModificacionesAsociarIndicadorPolitica(int proyectoId, int politicaId, int categoriaId, int indicadorId, string accion, string usuarioDnp);

    }
}
