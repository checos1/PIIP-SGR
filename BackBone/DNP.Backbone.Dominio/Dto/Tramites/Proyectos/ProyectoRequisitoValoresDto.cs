
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.Backbone.Dominio.Dto.Tramites.Proyectos
{
    [ExcludeFromCodeCoverage]
    public class ProyectoRequisitoValoresDto
    {
        public int Id { get; set; }
        public int ProyectosRequisitoId { get; set; }
        public TipoValorDto TipoValor { get; set; }
        public decimal? Valor { get; set; }

    }
}
