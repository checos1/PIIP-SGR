using DNP.ServiciosNegocio.Dominio.Dto.Transversales;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.ServiciosNegocio.Dominio.Dto.Tramites
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
