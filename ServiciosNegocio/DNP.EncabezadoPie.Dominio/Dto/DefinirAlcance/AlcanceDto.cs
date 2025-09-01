using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace DNP.EncabezadoPie.Dominio.Dto.DefinirAlcance
{
    using System;

    [ExcludeFromCodeCoverage]
    public class AlcanceDto
    {
        public int? ProyectoId { get; set; }
        public string NombreProyecto { get; set; }
        public string BPIN { get; set; }
        public int? SectorId { get; set; }
        public string Sector { get; set; }
        public int? ProgramaId { get; set; }
        public string Programa { get; set; }
        public decimal? CostoTotalProyecto { get; set; }
        public decimal? CostoApropiacionVigente { get; set; }
        public string Horizonte { get; set; }
        public string Ejecutor { get; set; }

    }
}
