using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.Backbone.Dominio.Dto.Programacion
{
    public class ProyectoSinPresupuestalDto
    {
        public string CodigoPresupuestal { get; set; }
        public int ProyectoId { get; set; }
        public int EntityTypeCatalogOptionId { get; set; }
        public DateTime FecDesde { get; set; }
        public DateTime FecHasta { get; set; }
        public string CodigoEntidad { get; set; }
        public string Programa { get; set; }
        public string Subprograma { get; set; }
        public Int64 Consecutivo { get; set; }
    }
}
