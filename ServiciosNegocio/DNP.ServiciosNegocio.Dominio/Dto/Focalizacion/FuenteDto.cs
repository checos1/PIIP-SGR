using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace DNP.ServiciosNegocio.Dominio.Dto.Focalizacion
{
    [ExcludeFromCodeCoverage]
    public class FuenteDto
    {
        public int? FuenteId{ get; set; }
       
        public string Fuente { get; set; }

        public decimal?FInicial { get; set; }
        public decimal? FSolicitado { get; set; }
        public decimal? FVigente { get; set; }
        public List<PoliticaDto> Politicas { get; set; }

    }
}
