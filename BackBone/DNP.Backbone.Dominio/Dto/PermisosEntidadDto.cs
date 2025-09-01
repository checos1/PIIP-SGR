using DNP.Autorizacion.Dominio.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.Backbone.Dominio.Dto
{
    public class PermisosEntidadDto
    {
        public string IdUsuarioDNP { get; set; }

        public List<string> OpcionesMenu { get; set; }

        public List<EntidadPermisoDto> Entidades { get; set; }

        public List<OpcionDto> BotonesOpciones { get; set; }
    }
}
