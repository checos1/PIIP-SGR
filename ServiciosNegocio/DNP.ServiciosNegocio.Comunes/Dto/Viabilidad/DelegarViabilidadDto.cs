using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.ServiciosNegocio.Comunes.Dto.Viabilidad
{
    public class DelegarViabilidadDto
    {
        public int ProyectoId { get; set; }
        public int EntityTypeCatalogOpcionId { get; set; }
        public bool Delegado { get; set; }
    }
}
