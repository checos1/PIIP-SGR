using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.Backbone.Dominio.Dto.SGP.Viabilidad
{
    [ExcludeFromCodeCoverage]
    public class AcuerdoSectorClasificadorSGPDto
    {
        public int Id { get; set; }
        public int ProyectoId { get; set; }
        public int AcuerdoNivelId { get; set; }
        public int AcuerdoSectorClasificadorId { get; set; }
        public bool Activo { get; set; }
        public int TipoConcepto { get; set; }
    }
}