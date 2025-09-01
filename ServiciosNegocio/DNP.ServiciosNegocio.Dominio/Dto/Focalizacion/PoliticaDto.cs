using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace DNP.ServiciosNegocio.Dominio.Dto.Focalizacion
{
    [ExcludeFromCodeCoverage]
    public class PoliticaDto
    {
        public int? PoliticaId{ get; set; }
        public string Politica{ get; set; }
        public List<DimensionDto> Dimensiones { get; set; }

    }
}
