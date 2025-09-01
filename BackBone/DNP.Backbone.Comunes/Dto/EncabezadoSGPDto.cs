using System;
using System.Diagnostics.CodeAnalysis;

namespace DNP.Backbone.Comunes.Dto
{
    [ExcludeFromCodeCoverage]
    public class EncabezadoSGPDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string CodigoBPIN { get; set; }
        public string EntidadPresenta { get; set; }
        public string EntidadDestino { get; set; }
        public string Estado { get; set; }
        public string Fase { get; set; }
        public int? VigenciaInicial { get; set; }
        public int? VigenciaFinal { get; set; }
        public decimal Valor { get; set; }
        public int AnioEstudio { get; set; }
        public string Sector { get; set; }
        public string TipoEntidadPresenta { get; set; }
        public string ProyectoTipo { get; set; }
        public string ObjetivoGeneral { get; set; }
        public int PoblacionObjetivo { get; set; }
        public string Alcance { get; set; }
        public decimal ValorTotalSGP { get; set; }
        public decimal ValorTotalAjuste { get; set; }
        public int AjustesRealizados { get; set; }
        public int PuntajeSEP { get; set; }
        public decimal ValorTotalInversion { get; set; }
        public decimal ValorTotalPreInversion { get; set; }
        public decimal ValorTotalOperacion { get; set; }
        public DateTime FechaPresentacion { get; set; }
        public string ProgramaPresupuestal { get; set; }
        public string SubProgramaPresupuestal { get; set; }
        public int TipoTramiteId { get; set; }
        public string Tramite { get; set; }
        public string Descripcion { get; set; }
        public decimal ValorVigente { get; set; }

    }
}