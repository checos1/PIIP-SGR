using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.ServiciosNegocio.Dominio.Dto.Programacion
{
    public class EliminarCategoriasProyectoProgramacionDto
    {
        public int TramiteId { get; set; }
        public int ProyectoId { get; set; }
        public int PoliticaId { get; set; }
        public int DimensionId { get; set; }
    }
}
