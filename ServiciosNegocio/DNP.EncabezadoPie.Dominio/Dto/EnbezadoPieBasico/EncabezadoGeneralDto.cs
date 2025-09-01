namespace DNP.EncabezadoPie.Dominio.Dto
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    public class EncabezadoGeneralDto
    {
        public string CodigoProceso { get; set; }
        public DateTime? Fecha { get; set; }
        public string Tipo { get; set; }
        public int? ProyectoId { get; set; }
        public string NombreProyecto { get; set; }
        public string CodBPIN { get; set; }
        public int? vigenciaInicial { get; set; }
        public int? vigenciaFinal { get; set; }
        public string entidad { get; set; }
        public string Estado { get; set; }
        public string Horizonte { get; set; }
        public string sector { get; set; }
        public decimal? valorTotal { get; set; }
        public decimal? apropiacionInicial { get; set; }
        public decimal? apropiacionVigente { get; set; }
        public decimal? ValorTotalConTramiteActual { get; set; }
        public decimal? ApropiacionVigenteConTramiteActual { get; set; }
        public int? ContieneTramite { get; set; }
        public int? TramiteId { get; set; }
        public string Alcanceproyecto { get; set; }
        public decimal? CostoTotalProyecto { get; set; }
        public string Ejecutor { get; set; }
        public DateTime?  FechaRealInicio { get; set; }
        public int? AplicaEjecucionPlaneacion { get; set; }
        public Nullable<int> TipoId { get; set; }
        public Nullable<int> IdTipoTramitePresupuestal { get; set; }
        public string PeriodoAbierto { get; set; }
        public string FechaInicioReporte { get; set; }
        public string FechaLimiteReporte { get; set; }
    }
}
