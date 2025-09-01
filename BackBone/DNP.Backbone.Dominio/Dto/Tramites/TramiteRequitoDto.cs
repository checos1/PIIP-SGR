using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.Backbone.Dominio.Dto.Tramites
{
    public class TramiteRequitoDto
    {
        public string Descripcion { get; set; }
        public DateTime FechaCDP { get; set; }
        public int IdPresupuestoValoresCDP { get; set; }
        public int IdPresupuestoValoresAportaCDP { get; set; }
        public int IdProyectoRequisitoTramite { get; set; }
        public int IdProyectoTramite { get; set; }
        public int IdTipoRequisito { get; set; }
        public string NumeroCDP { get; set; }
        public string NumeroContratoCDP { get; set; }
        public int Tipo { get; set; }
        public string UnidadEjecutora { get; set; }
        public double ValorCDP { get; set; }
        public double ValorTotalCDP { get; set; }
        public int IdValorTotalCDP { get; set; }
        public int IdValorAportaCDP { get; set; }

        public int IdTramite { get; set; }
        public int IdProyecto { get; set; }

        public int IdTipoRol { get; set; }
        public Guid IdRol { get; set; }

    }
}
