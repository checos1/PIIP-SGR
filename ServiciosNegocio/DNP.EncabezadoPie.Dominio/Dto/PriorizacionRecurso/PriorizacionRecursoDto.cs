using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace DNP.EncabezadoPie.Dominio.Dto.PriorizacionRecurso
{
    using System;

    [ExcludeFromCodeCoverage]
    public class PriorizacionRecursoDto
    {
        public string BPIN { get; set; }
        public int? ProyectoId { get; set; }
        public string Proyecto { get; set; }
        public int? CR { get; set; }
        public int? VigenciaInicial { get; set; }
        public int? VigenciaFinal { get; set; }
        public int? EntidadId { get; set; }
        public string Entidad { get; set; }
        public int? SectorId { get; set; }
        public string SectorInversion { get; set; }
        public decimal? RecursosSolEje { get; set; }
        public decimal? RecursosSolPro { get; set; }
    }
}
