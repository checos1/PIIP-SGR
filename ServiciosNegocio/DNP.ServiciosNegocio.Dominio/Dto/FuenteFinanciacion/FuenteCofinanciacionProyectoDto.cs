using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace DNP.ServiciosNegocio.Dominio.Dto.FuenteFinanciacion
{
    using System;

    [ExcludeFromCodeCoverage]
    public class FuenteCofinanciacionProyectoDto
    {
        public int? ProyectoId { get; set; }
        public string CodigoBPIN { get; set; }
        public int? CR { get; set; }
        public List<FuenteCofinanciacionDto> Cofinanciacion { get; set; }
    }
}
