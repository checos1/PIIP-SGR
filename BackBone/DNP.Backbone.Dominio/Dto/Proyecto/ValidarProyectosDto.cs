using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.Backbone.Dominio.Dto.Proyecto
{
    public class ValidarProyectosDto
    {
        public Guid IdTipoObjetoNegocio { get; set; }

        public List<string> Bpins { get; set; }

        public string IdUsuarioDNP { get; set; }
    }
}
