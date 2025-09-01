using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.Backbone.Dominio.Dto.Monitoreo
{
    [ExcludeFromCodeCoverage]
    public class ProyectoResumenDto
    {
        public string Mensaje { get; set; }
        public List<GrupoEntidadProyectoResumenDto> GruposEntidades { get; set; }
        public string[] ColumnasVisibles { get; set; }
    }
}
