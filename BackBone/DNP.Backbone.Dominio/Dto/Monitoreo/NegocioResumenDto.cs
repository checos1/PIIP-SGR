namespace DNP.Backbone.Dominio.Dto.Monitoreo
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    //Se excluyen los DTO por ser clases POCO. Las clases POCO solo tienen propiedades y no tienen métodos para ser testeados.
    [SuppressMessage("ReSharper", "InconsistentNaming")]
	public class NegocioResumenDto
	{
		public int ProyectoId { get; set; }
		public string ProyectoNombre { get; set; }
		public string CodigoBpin { get; set; }
		public string Horizonte { get; set; }
		public string EstadoProyecto { get; set; }
		public bool TieneAlertas { get; set; }
		public bool TieneRelatorios { get; set; }
		public DateTime FechaInicial { get; set; }
		public DateTime FechaFinal { get; set; }

        public int IdEntidad { get; set; }
        public string NombreEntidad { get; set; }
        public string TipoEntidad { get; set; }
        public string SectorNombre { get; set; }
        public string DescripcionCR { get; set; }
        public string AvanceFinanciero { get; set; }
        public string AvanceFisico { get; set; }
        public string AvanceProyecto { get; set; }
        public string Duracion { get; set; }
        public string PeriodoEjecucion { get; set; }
        public int? SectorId { get; set; }
        public int? EstadoId { get; set; }
    }
}
