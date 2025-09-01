using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.ServiciosNegocio.Dominio.Dto.Preguntas
{
    public class ConceptosPreviosEmitidosDto
    {
        public int TotalConceptosEmitidos { get; set; }
        public string FechaSolicitudUltimoConcepto { get; set; }
        public string FechaEmisionUltimoConcepto { get; set; }
        public List<ConceptoEmitido> ConceptosEmitidos { get; set; }
    }

    public class ConceptoEmitido
    {
        public int Id { get; set; }
        public string FechaEmision { get; set; }
    }
}
