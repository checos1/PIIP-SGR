using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.Backbone.Dominio.Dto.Transversal
{
    public class TipoMotivoAnulacionDto
    {
        public int Id { get; set; }

        public string Descripcion { get; set; }

        public bool Estado { get; set; }
    }
}
