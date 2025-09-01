using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace DNP.Backbone.Dominio.Dto
{
    [ExcludeFromCodeCoverage]
    public sealed class EstadoDto
    {
        public int Id { get; set; }
        public string Estado { get; set; }
        public string Codigo { get; set; }
    }
}
