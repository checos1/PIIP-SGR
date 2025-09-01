using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.Backbone.Dominio.Filtros
{
    public class FiltroGeneral
    {
        public string IdAplicacion { get; set; }
        public string IdOpcionDnp { get; set; }
        public string NombreOpcion { get; set; }
        public bool? Activo { get; set; }
        public int? TipoOpcion { get; set; }
        public int NumeroPagina { get; set; }
        public int TamanoPagina { get; set; }
    }
}
