using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.ServiciosNegocio.Dominio.Dto.SGR.Viabilidad
{
    [ExcludeFromCodeCoverage]
    public class AcuerdoSectorClasificadorDto
    {
        public int Id { get; set; }
        public int ProyectoId { get; set; }
        public int AcuerdoNivelId { get; set; }
        public int AcuerdoSectorClasificadorId { get; set; }
        public bool Activo { get; set; }
    }
}