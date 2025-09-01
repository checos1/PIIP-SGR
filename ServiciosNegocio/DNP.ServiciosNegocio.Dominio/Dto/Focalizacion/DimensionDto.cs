using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace DNP.ServiciosNegocio.Dominio.Dto.Focalizacion
{
    [ExcludeFromCodeCoverage]
    public class DimensionDto
    {
        public int? DimensionId{ get; set; }
        public string Dimension{ get; set; }
        public decimal? Solicitado { get; set; }
        public decimal? ApropiacionInicial { get; set; }
        public decimal? ApropiacionVigente { get; set; }
        public int? PoliticaId { get; set; }

        public int? FocalizacionRecursosId { get; set; }


    }
}
