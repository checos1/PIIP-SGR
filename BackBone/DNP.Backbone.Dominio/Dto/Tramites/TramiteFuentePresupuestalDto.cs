using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.Backbone.Dominio.Dto.Tramites
{
    public class TramiteFuentePresupuestalDto
    {

        public int IdFuente { get; set; }
        public int IdProyectoFuenteTramite { get; set; }
        public int IdProyectoTramite { get; set; }
        public double ValorContracreditoCSF { get; set; }
        public double ValorContracreditoSSF { get; set; }
        public double ValorInicial { get; set; }
        public double Valorvigente { get; set; }
        public string Accion { get; set; }

        public int IdTipoValorInicial { get; set; }
        public int IdTipoValorVigente { get; set; }
        public int IdTipoValorContracreditoCSF { get; set; }
        public int IdTipoValorContracreditoSSF { get; set; }

        public int IdTramite { get; set; }
        public int IdProyecto { get; set; }
    }
}
