using DNP.ServiciosNegocio.Comunes.Dto.Programacion;
using DNP.ServiciosNegocio.Dominio.Dto.Programacion;
using DNP.ServiciosNegocio.Dominio.Dto.ProgramacionDistribucion;
using DNP.ServiciosNegocio.Dominio.Dto.ProgramacionFuente;
using DNP.ServiciosNegocio.Dominio.Dto.SeguimientoControl;
using DNP.ServiciosNegocio.Dominio.Dto.Tramites;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DNP.ServiciosNegocio.Dominio.Dto.Focalizacion;
using DNP.ServiciosNegocio.Dominio.Dto.Proyectos;

namespace DNP.ServiciosNegocio.Servicios.Interfaces.Programacion
{
    public interface IProgramacionServicio
    {        
        bool ValidarCalendarioProgramacion(Nullable<int> entityTypeCatalogOptionId, Nullable<Guid> nivelId, Nullable<int> seccionCapituloId);
        string ObtenerCargaMasivaCreditos();
        string ObtenerProgramacionProyectosSinPresupuestal(int? sectorId, int? entidadId, string proyectoId);
        string ObtenerCargaMasivaCuotas(int? Vigencia, int? EntityTypeCatalogOptionId);
        string ObtenerProgramacionSectores(int? sectorId);
        string ObtenerProgramacionEntidadesSector(int? sectorId);
        string ObtenerCalendarioProgramacion(Guid FlujoId);        
        TramitesResultado RegistrarCargaMasivaCreditos(List<CargueCreditoDto> json, string usuario);
        string ObtenerDatosProgramacionEncabezado(int EntidadDestinoId, int tramiteid, string origen);
        TramitesResultado GuardarDatosProgramacionDistribucion(ProgramacionDistribucionDto ProgramacionDistribucion, string usuario);
        string ObtenerDatosProgramacionDetalle(int tramiteidProyectoId, string origen);
        string ValidarCargaMasivaCreditos(List<CargueCreditoDto> json);
        TramitesResultado RegistrarCargaMasivaCuota(List<CargueCuotaDto> json, string usuario);
        TramitesResultado RegistrarProyectosSinPresupuestal(List<ProyectoSinPresupuestalDto> json, string usuario);
        TramitesResultado RegistrarCalendarioProgramacion(List<CalendarioProgramacionDto> json, string usuario);
        TramitesResultado GuardarDatosProgramacionFuente(ProgramacionFuenteDto ProgramacionFuente, string usuario);
        string ValidarConsecutivoPresupuestal(List<ProyectoSinPresupuestalDto> json);
        string ValidarCargaMasivaCuotas(List<CargueCuotaDto> json);
        string ObtenerDatostProgramacionProducto(int tramiteiId);
        TramitesResultado GuardarDatosProgramacionProducto(ProgramacionProductoDto ProgramacionProducto, string usuario);
        TramitesResultado GuardarDatosProgramacionIniciativa(ProgramacionIniciativaDto ProgramacionIniciativa, string usuario);
        TramitesResultado GuardarProgramacionRegionalizacion(ProgramacionRegionalizacionDto ProgramacionRegionalizacion, string usuario);
        string ConsultarPoliticasTransversalesProgramacion(string Bpin);
        TramitesResultado AgregarPoliticasTransversalesProgramacion(IncluirPoliticasDto parametrosGuardar, string usuario);
        string ConsultarPoliticasTransversalesCategoriasProgramacion(string Bpin);
        TramitesResultado EliminarPoliticasProyectoProgramacion(int tramiteidProyectoId, int politicaId);
        TramitesResultado AgregarCategoriasPoliticaTransversalesProgramacion(FocalizacionCategoriasDto objIncluirPoliticasDto, string usuario);
        TramitesResultado EliminarCategoriaPoliticasProyectoProgramacion(int proyectoId, int politicaId, int categoriaId);
        string ObtenerCrucePoliticasProgramacion(string Bpin);
        string PoliticasSolicitudConceptoProgramacion(string Bpin);
        TramitesResultado GuardarCrucePoliticasProgramacion(List<CrucePoliticasAjustesDto> parametrosGuardar, string usuario);
        TramitesResultado SolicitarConceptoDTProgramacion(List<FocalizacionSolicitarConceptoDto> parametrosGuardar, string usuario);
        string ObtenerResumenSolicitudConceptoProgramacion(string Bpin);
        string ObtenerProgramacionBuscarProyecto(int EntidadDestinoId, int tramiteid, string bpin, string NombreProyecto);
        TramitesResultado BorrarTramiteProyecto(ProgramacionDistribucionDto ProgramacionDistribucion, string usuario);
        TramitesResultado GuardarPoliticasTransversalesCategoriasProgramacion(PoliticasTransversalesCategoriasProgramacionDto objIncluirPoliticasDto, string usuario);
        TramitesResultado GuardarDatosInclusion(ProgramacionDistribucionDto ProgramacionDistribucion, string usuario);
        TramitesResultado EliminarCategoriasProyectoProgramacion(EliminarCategoriasProyectoProgramacionDto objIncluirPoliticasDto, string usuario);
        string ConsultarPoliticasTransversalesCategoriasModificaciones(string Bpin);
        TramitesResultado GuardarPoliticasTransversalesCategoriasModificaciones(PoliticasTransversalesCategoriasProgramacionDto objIncluirPoliticasDto, string usuario);
        string ConsultarPoliticasTransversalesAprobacionesModificaciones(string Bpin);
        TramitesResultado RegistrarCargaMasivaSaldos(int TipoCargueId, string usuario);
        string ObtenerLogErrorCargaMasivaSaldos(int? TipoCargueDetalleId, int? CarguesIntegracionId);
        string ObtenerCargaMasivaSaldos(string TipoCargue);
        string ObtenerDetalleCargaMasivaSaldos(int? CargueId);
        string ObtenerTipoCargaMasiva(string TipoCargue);
        TramitesResultado ValidarCargaMasiva(dynamic jsonListaRegistros, string usuario);



        string ConsultarCatalogoIndicadoresPolitica(string PoliticaId, string Criterio);
        TramitesResultado GuardarModificacionesAsociarIndicadorPolitica(int proyectoId, int politicaId, int categoriaId, int indicadorId, string accion, string usuario);

    }
}
