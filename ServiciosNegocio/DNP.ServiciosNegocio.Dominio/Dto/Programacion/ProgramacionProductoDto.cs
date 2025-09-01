using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.ServiciosNegocio.Dominio.Dto.Programacion
{
    public class ProgramacionProductoDto
    {
        public int TramiteId { get; set; }
        public string NivelId { get; set; }
        public int SeccionCapitulo { get; set; }
        public List<ProgramacionProductos> ProgramacionProductos { get; set; }

    }

    public class ProgramacionProductos
    {
        public int ProductCatalogId { get; set; }
        public decimal? Meta { get; set; }
        public decimal? Recurso { get; set; }

    }

   
}
