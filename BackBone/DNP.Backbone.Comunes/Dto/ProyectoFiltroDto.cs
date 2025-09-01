namespace DNP.Backbone.Comunes.Dto
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    public class ProyectoFiltroDto
    {
        public string Identificador { get; set; }
        public string Nombre { get; set; }
        public int? SectorId { get; set; }
        public int? EntidadId { get; set; }
        public string IdentificadorCR { get; set; }
        public string Horizonte { get; set; }
        public int? HorizonteInicio { get; set; }
        public int? HorizonteFin { get; set; }
        public int? PrioridadId { get; set; }
        public string EstadoProyecto { get; set; }
        public string Prioridad { get; set; }
        public int? TipoEntidadId { get; set; }
        public string Bpin { get; set; }
        public int? EstadoProyectoId { get; set; }
        public string TipoEntidad { get; set; }
        public string CodigoTramite { get; set; }
        public bool? EstadoTramite { get; set; }
        public string AvanceFinanciero { get; set; }
        public string AvanceFisico { get; set; }
        public string AvanceProyecto { get; set; }
        public string Duracion { get; set; }
        public string PeriodoEjecucion { get; set; }
        public string NombreFlujo { get; set; }
        public int[] ProyectosIds { get; set; }

        public string AccionFlujo { get; set; }
        public int? EstadoInstanciaId { get; set; }
        public string[] IdsEtapas { get; set; }
        public string SectorNombre { get; set; }
        public string MacroprocesoId { get; set; }
        public string ProcesoId { get; set; }
        public string Macroproceso { get; set; }
        public string CodigoProceso { get; set; }
        public int? VigenciaProyectoId { get; set; }
        public int? EntidadProyectoId { get; set; }
        public Guid? FlujoId { get; set; }
        public int? Vigencia { get; set; }
        public string AccionId { get; set; }

    }
}
