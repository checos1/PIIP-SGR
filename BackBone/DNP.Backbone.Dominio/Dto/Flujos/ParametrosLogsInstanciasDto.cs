using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics.CodeAnalysis;

namespace DNP.Backbone.Dominio.Dto.Flujos
{

    [ExcludeFromCodeCoverage]
    public class ParametrosLogsInstanciasDto
    {
        public int? Codigo { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public string BPIN { get; set; }
        public string Descripcion { get; set; }
        public string Estado { get; set; }
        public string UsuarioId { get; set; }
        public int? EntityTypeCatalogOptionId { get; set; }
        public Guid? TipoObjetoId { get; set; }
        public bool EsTramite { get; set; }
    }
}
