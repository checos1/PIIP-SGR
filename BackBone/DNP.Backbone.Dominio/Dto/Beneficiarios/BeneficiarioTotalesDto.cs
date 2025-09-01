using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.Backbone.Dominio.Dto.Beneficiarios
{
    public class BeneficiarioTotalesDto
    {
        public int ProyectoId { get; set; }
        public string BPIN { get; set; }
        public int NumeroPersonalAjuste { get; set; }
    }
}
