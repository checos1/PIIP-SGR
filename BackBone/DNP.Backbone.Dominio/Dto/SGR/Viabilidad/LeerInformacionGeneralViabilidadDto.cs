using System;
using System.Diagnostics.CodeAnalysis;

namespace DNP.Backbone.Dominio.Dto.SGR.Viabilidad
{
    [ExcludeFromCodeCoverage]
    public class LeerInformacionGeneralViabilidadDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string CodigoBPIN { get; set; }
        public int? NumeroRevision { get; set; }
        public string Categorias { get; set; }
        public string Fase { get; set; }
        public int? RegionSgrId { get; set; }
        public string EntidadPresenta { get; set; }
        public string NombreEjecutor { get; set; }
        public string DepartamentoEjecuta { get; set; }
        public string MunicipioEjecuta { get; set; }
        public string SectorInversion { get; set; }
        public int? SectorApoyo1Id { get; set; }
        public int? SectorApoyo2Id { get; set; }
        public string InstanciaAprobacion { get; set; }
        public decimal? TiempoEjecucion { get; set; }
        public string ProyectoTipo { get; set; }
        public decimal? ValorEstructuracion { get; set; }
        public decimal? ValorEmision { get; set; }
        public decimal? ValorInterventoria { get; set; }
        public decimal? ValorApoyoSupervision { get; set; }
        public string ObjetivoGeneral { get; set; }
        public string Alcance { get; set; }
        public int? PoblacionAfectada { get; set; }
        public int? PoblacionObjetivo { get; set; }
        public bool? PresentadoComunidadIndigena { get; set; }
        public bool? PresentadoComunidadNarp { get; set; }
        public bool? PresentadoComunidadRrom { get; set; }
        public bool? LocalizacionComunidadIndigena { get; set; }
        public bool? LocalizacionComunidadNarp { get; set; }
        public bool? LocalizacionComunidadRrom { get; set; }
        public int TipoConceptoViabilidadId { get; set; }
        public string AlcanceEspacial { get; set; }
        public string Poblacion { get; set; }
        public string NecesidadesSocioCulturales { get; set; }
        public string NormasLineamientosAdicionales { get; set; }
        public int? ProyectoViabilidadId { get; set; }
    }
}
