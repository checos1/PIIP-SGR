using System;
using System.Diagnostics.CodeAnalysis;

namespace DNP.Backbone.Dominio.Dto.SGR.Viabilidad
{
    [ExcludeFromCodeCoverage]
    public class InformacionBasicaViabilidadDto
    {
        public int ProyectoId { get; set; }
        public Guid InstanciaId { get; set; }
        public string CategoriasProyecto { get; set; }
        public int RegionSgr { get; set; }
        public int SectorApoyo1 { get; set; }
        public int SectorApoyo2 { get; set; }
        public decimal ValorInterventoria { get; set; }
        public decimal ValorApoyoSupervision { get; set; }
        public string AlcanceEspacial { get; set; }
        public string Poblacion { get; set; }
        public string NecesidadesSocioCulturales { get; set; }
        public string NormasLineamientosAdicionales { get; set; }
        public int TipoConceptoViabilidadId { get; set; }
        public int? ProyectoPresentadoComunidad { get; set; }
        public int? ProyectoLocalizadoComunidad { get; set; }
        public string InstanciaAprobacion { get; set; }
    }
}