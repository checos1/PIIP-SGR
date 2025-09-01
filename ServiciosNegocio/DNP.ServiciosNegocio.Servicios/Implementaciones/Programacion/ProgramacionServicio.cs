using DNP.ServiciosNegocio.Comunes.Dto.Programacion;
using DNP.ServiciosNegocio.Comunes.Excepciones;
using DNP.ServiciosNegocio.Dominio.Dto.Programacion;
using DNP.ServiciosNegocio.Dominio.Dto.ProgramacionDistribucion;
using DNP.ServiciosNegocio.Dominio.Dto.ProgramacionFuente;
using DNP.ServiciosNegocio.Dominio.Dto.SeguimientoControl;
using DNP.ServiciosNegocio.Dominio.Dto.Tramites;
using DNP.ServiciosNegocio.Persistencia.Interfaces.Programacion;
using DNP.ServiciosNegocio.Servicios.Interfaces.Programacion;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DNP.ServiciosNegocio.Dominio.Dto.Focalizacion;
using DNP.ServiciosNegocio.Dominio.Dto.Proyectos;

namespace DNP.ServiciosNegocio.Servicios.Implementaciones.Programacion
{
    public class ProgramacionServicio : IProgramacionServicio
    {
        private readonly IProgramacionPersistencia _programacionPersistencia;

        public ProgramacionServicio(IProgramacionPersistencia programacionPersistencia)
        {
            _programacionPersistencia = programacionPersistencia;
        }

        public bool ValidarCalendarioProgramacion(int? entityTypeCatalogOptionId, Guid? nivelId, int? seccionCapituloId)
        {
            return _programacionPersistencia.ValidarCalendarioProgramacion(entityTypeCatalogOptionId, nivelId, seccionCapituloId);
        }

        public string ObtenerCargaMasivaCreditos()
        {
            return _programacionPersistencia.ObtenerCargaMasivaCreditos();
        }

        public string ObtenerProgramacionProyectosSinPresupuestal(int? sectorId, int? entidadId, string proyectoId)
        {
            return _programacionPersistencia.ObtenerProgramacionProyectosSinPresupuestal(sectorId, entidadId, proyectoId);
        }

        public string ObtenerCargaMasivaCuotas(int? Vigencia, int? EntityTypeCatalogOptionId)
        {
            return _programacionPersistencia.ObtenerCargaMasivaCuotas(Vigencia, EntityTypeCatalogOptionId);
        }

        public string ObtenerProgramacionSectores(int? sectorId)
        {
            return _programacionPersistencia.ObtenerProgramacionSectores(sectorId);
        }
        public string ObtenerProgramacionEntidadesSector(int? sectorId)
        {
            return _programacionPersistencia.ObtenerProgramacionEntidadesSector(sectorId);
        }

        public string ObtenerCalendarioProgramacion(Guid FlujoId)
        {
            return _programacionPersistencia.ObtenerCalendarioProgramacion(FlujoId);
        }

        public TramitesResultado RegistrarCargaMasivaCreditos(List<CargueCreditoDto> json, string usuario)
        {
            return _programacionPersistencia.RegistrarCargaMasivaCreditos(json, usuario);
        }

        public string ObtenerDatosProgramacionEncabezado(int EntidadDestinoId, int tramiteid, string origen)
        {
            return _programacionPersistencia.ObtenerDatosProgramacionEncabezado(EntidadDestinoId, tramiteid, origen);
        }

        public TramitesResultado GuardarDatosProgramacionDistribucion(ProgramacionDistribucionDto ProgramacionDistribucion, string usuario)
        {
            return _programacionPersistencia.GuardarDatosProgramacionDistribucion(ProgramacionDistribucion, usuario);
        }
        public string ObtenerDatosProgramacionDetalle(int tramiteidProyectoId, string origen)
        {
            return _programacionPersistencia.ObtenerDatosProgramacionDetalle(tramiteidProyectoId, origen);
        }

        public string ValidarCargaMasivaCreditos(List<CargueCreditoDto> json)
        {
            return _programacionPersistencia.ValidarCargaMasivaCreditos(json);
        }

        public TramitesResultado RegistrarCargaMasivaCuota(List<CargueCuotaDto> json, string usuario)
        {
            return _programacionPersistencia.RegistrarCargaMasivaCuota(json, usuario);
        }

        public TramitesResultado RegistrarProyectosSinPresupuestal(List<ProyectoSinPresupuestalDto> json, string usuario)
        {
            return _programacionPersistencia.RegistrarProyectosSinPresupuestal(json, usuario);
        }

        public TramitesResultado RegistrarCalendarioProgramacion(List<CalendarioProgramacionDto> json, string usuario)
        {
            return _programacionPersistencia.RegistrarCalendarioProgramacion(json, usuario);
        }
        public TramitesResultado GuardarDatosProgramacionFuente(ProgramacionFuenteDto ProgramacionFuente, string usuario)
        {
            return _programacionPersistencia.GuardarDatosProgramacionFuente(ProgramacionFuente, usuario);
        }

        public string ValidarConsecutivoPresupuestal(List<ProyectoSinPresupuestalDto> json)
        {
            return _programacionPersistencia.ValidarConsecutivoPresupuestal(json);
        }

        public string ValidarCargaMasivaCuotas(List<CargueCuotaDto> json)
        {
            return _programacionPersistencia.ValidarCargaMasivaCuotas(json);
        }


        public string ObtenerDatostProgramacionProducto(int tramiteiId)
        {
            return _programacionPersistencia.ObtenerDatostProgramacionProducto(tramiteiId);
        }
        public TramitesResultado GuardarDatosProgramacionProducto(ProgramacionProductoDto ProgramacionProducto, string usuario)
        {
            return _programacionPersistencia.GuardarDatosProgramacionProducto(ProgramacionProducto, usuario);
        }

        public TramitesResultado GuardarDatosProgramacionIniciativa(ProgramacionIniciativaDto ProgramacionIniciativa, string usuario)
        {
            return _programacionPersistencia.GuardarDatosProgramacionIniciativa(ProgramacionIniciativa, usuario);
        }

        public TramitesResultado GuardarProgramacionRegionalizacion(ProgramacionRegionalizacionDto ProgramacionRegionalizacion, string usuario)
        {
            return _programacionPersistencia.GuardarProgramacionRegionalizacion(ProgramacionRegionalizacion, usuario);
        }
        public string ConsultarPoliticasTransversalesProgramacion(string Bpin)
        {
            return _programacionPersistencia.ConsultarPoliticasTransversalesProgramacion(Bpin);
        }
        public TramitesResultado AgregarPoliticasTransversalesProgramacion(IncluirPoliticasDto parametrosGuardar, string usuario)
        {
            return _programacionPersistencia.AgregarPoliticasTransversalesProgramacion(parametrosGuardar, usuario);
        }
        public string ConsultarPoliticasTransversalesCategoriasProgramacion(string Bpin)
        {
            return _programacionPersistencia.ConsultarPoliticasTransversalesCategoriasProgramacion(Bpin);
        }
        public TramitesResultado EliminarPoliticasProyectoProgramacion(int tramiteidProyectoId, int politicaId)
        {
            return _programacionPersistencia.EliminarPoliticasProyectoProgramacion(tramiteidProyectoId, politicaId);
        }
        public TramitesResultado AgregarCategoriasPoliticaTransversalesProgramacion(FocalizacionCategoriasDto objIncluirPoliticasDto, string usuario)
        {
            return _programacionPersistencia.AgregarCategoriasPoliticaTransversalesProgramacion(objIncluirPoliticasDto, usuario);
        }

        public TramitesResultado GuardarPoliticasTransversalesCategoriasProgramacion(PoliticasTransversalesCategoriasProgramacionDto objIncluirPoliticasDto, string usuario)
        {
            return _programacionPersistencia.GuardarPoliticasTransversalesCategoriasProgramacion(objIncluirPoliticasDto, usuario);
        }

        public TramitesResultado EliminarCategoriasProyectoProgramacion(EliminarCategoriasProyectoProgramacionDto objIncluirPoliticasDto, string usuario)
        {
            return _programacionPersistencia.EliminarCategoriasProyectoProgramacion(objIncluirPoliticasDto, usuario);
        }

        public TramitesResultado EliminarCategoriaPoliticasProyectoProgramacion(int proyectoId, int politicaId, int categoriaId)
        {
            return _programacionPersistencia.EliminarCategoriaPoliticasProyectoProgramacion(proyectoId, politicaId, categoriaId);
        }
        public string ObtenerCrucePoliticasProgramacion(string Bpin)
        {
            return _programacionPersistencia.ObtenerCrucePoliticasProgramacion(Bpin);
        }
        public string PoliticasSolicitudConceptoProgramacion(string Bpin)
        {
            return _programacionPersistencia.PoliticasSolicitudConceptoProgramacion(Bpin);
        }
        public TramitesResultado GuardarCrucePoliticasProgramacion(List<CrucePoliticasAjustesDto> parametrosGuardar, string usuario)
        {
            return _programacionPersistencia.GuardarCrucePoliticasProgramacion(parametrosGuardar, usuario);
        }

       
        public TramitesResultado SolicitarConceptoDTProgramacion(List<FocalizacionSolicitarConceptoDto> parametrosGuardar, string usuario)
        {
            return _programacionPersistencia.SolicitarConceptoDTProgramacion(parametrosGuardar, usuario);
        }
        public string ObtenerResumenSolicitudConceptoProgramacion(string Bpin)
        {
            return _programacionPersistencia.ObtenerResumenSolicitudConceptoProgramacion(Bpin);
        }
        public string ObtenerProgramacionBuscarProyecto(int EntidadDestinoId, int tramiteid, string bpin, string NombreProyecto)
        {
            return _programacionPersistencia.ObtenerProgramacionBuscarProyecto(EntidadDestinoId, tramiteid, bpin, NombreProyecto);
        }
        public TramitesResultado BorrarTramiteProyecto(ProgramacionDistribucionDto ProgramacionDistribucion, string usuario)
        {
            return _programacionPersistencia.BorrarTramiteProyecto(ProgramacionDistribucion, usuario);
        }
        public TramitesResultado GuardarDatosInclusion(ProgramacionDistribucionDto ProgramacionDistribucion, string usuario)
        {
            return _programacionPersistencia.GuardarDatosInclusion(ProgramacionDistribucion, usuario);
        }
        public string ConsultarPoliticasTransversalesCategoriasModificaciones(string Bpin)
        {
            return _programacionPersistencia.ConsultarPoliticasTransversalesCategoriasModificaciones(Bpin);
        }
        public TramitesResultado GuardarPoliticasTransversalesCategoriasModificaciones(PoliticasTransversalesCategoriasProgramacionDto objIncluirPoliticasDto, string usuario)
        {
            return _programacionPersistencia.GuardarPoliticasTransversalesCategoriasModificaciones(objIncluirPoliticasDto, usuario);
        }
        public string ConsultarPoliticasTransversalesAprobacionesModificaciones(string Bpin)
        {
            return _programacionPersistencia.ConsultarPoliticasTransversalesAprobacionesModificaciones(Bpin);
        }
        public TramitesResultado RegistrarCargaMasivaSaldos(int TipoCargueId, string usuario)
        {
            return _programacionPersistencia.RegistrarCargaMasivaSaldos(TipoCargueId, usuario);
        }


        public string ObtenerLogErrorCargaMasivaSaldos(int? TipoCargueDetalleId, int? CarguesIntegracionId)
        {
            return _programacionPersistencia.ObtenerLogErrorCargaMasivaSaldos(TipoCargueDetalleId, CarguesIntegracionId);
        }
        public string ObtenerCargaMasivaSaldos(string TipoCargue)
        {
            return _programacionPersistencia.ObtenerCargaMasivaSaldos(TipoCargue);
        }
        public string ObtenerTipoCargaMasiva(string TipoCargue)
        {
            return _programacionPersistencia.ObtenerTipoCargaMasiva(TipoCargue);
        }
        public TramitesResultado ValidarCargaMasiva(dynamic jsonListaRegistros, string usuario)
        {
            return _programacionPersistencia.ValidarCargaMasiva(jsonListaRegistros, usuario);
        }
        public string ObtenerDetalleCargaMasivaSaldos(int? CargueId)
        {
            return _programacionPersistencia.ObtenerDetalleCargaMasivaSaldos(CargueId);
        }

        public string ConsultarCatalogoIndicadoresPolitica(string PoliticaId, string Criterio)
        {
            return _programacionPersistencia.ConsultarCatalogoIndicadoresPolitica(PoliticaId, Criterio);
        }
        public TramitesResultado GuardarModificacionesAsociarIndicadorPolitica(int proyectoId, int politicaId, int categoriaId, int indicadorId, string accion, string usuario)
        {
            return _programacionPersistencia.GuardarModificacionesAsociarIndicadorPolitica(proyectoId, politicaId, categoriaId, indicadorId, accion, usuario);
        }

    }
}
