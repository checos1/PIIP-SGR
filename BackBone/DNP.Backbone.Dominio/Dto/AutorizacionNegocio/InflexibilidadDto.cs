using DNP.Backbone.Dominio.Enums;
using System;
using System.Collections.Generic;

namespace DNP.Backbone.Dominio.Dto.AutorizacionNegocio
{
    public class InflexibilidadDto
    {
        public int IdInflexibilidad { get; set; }
        public int Id{ get; set; }
        public string NombreInflexibilidad { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public double ValorTotal { get; set; }
        public double ValorPagado { get; set; }
        public string Estado { get; set; }
        public string Periodo { get { return FechaInicio.ToString("dd/MM/yyyy") + " - " + FechaFin.ToString("dd/MM/yyyy"); } }
        public string PeriodoExcel { get; set; }
        public TipoInflexibilidadEnum TipoInflexibilidad { get; set; }
        public Guid? IdEntidad { get; set; }
        public List<string> Columnas { get; set; }
        public List<string> ColumnasHeader { get; set; }
    }
}
