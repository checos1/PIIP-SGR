using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.Backbone.Dominio.Dto.Programacion
{
    public class FlujosProgramacionDto
    {
        public Guid? Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
    }
}
