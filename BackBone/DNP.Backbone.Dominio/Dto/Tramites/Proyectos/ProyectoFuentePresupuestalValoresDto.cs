
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.Backbone.Dominio.Dto.Tramites.Proyectos
{
    [ExcludeFromCodeCoverage]
    public class ProyectoFuentePresupuestalValoresDto
    {
        public int Id { get; set; }
        public int ProyectoFuentePresupuestalId { get; set; }
        public TipoValorDto TipoValor { get; set; }
        public decimal? Valor { get; set; }
    }
}
