using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.Backbone.Dominio.Dto.Transversal
{
    [ExcludeFromCodeCoverage]
    public class SeccionesTramiteDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string NombrePestana { get; set; }
        public string NombreModificado { get; set; }
        public int? Porcentaje { get; set; }
    }
}