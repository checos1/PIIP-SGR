using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.ServiciosNegocio.Dominio.Dto.Programacion
{
    public class ProgramacionIniciativaDto
    {
        public int? TramiteProyectoId { get; set; }
        public int? SeccionCapitulo { get; set; }
        public List<Iniciativa> Iniciativa { get; set; }
    }

    public class Iniciativa
    {
        public int Id { get; set; }
        public int IniciativaId { get; set; }
    }
}
