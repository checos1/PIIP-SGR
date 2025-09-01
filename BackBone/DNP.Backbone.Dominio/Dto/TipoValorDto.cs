using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.Backbone.Dominio.Dto
{
    [ExcludeFromCodeCoverage]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class TipoValorDto
    {
        public int Id { get; set; }
        public string TipoValorFuente  { get; set; }
        public int FaseId  { get; set; }
        public string Tematica  { get; set; }
    }
}
