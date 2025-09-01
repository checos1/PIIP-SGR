using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.Backbone.Dominio.Dto.Flujos
{
    [ExcludeFromCodeCoverage]
    public class LogsInstanciasDto
    {
        public int Id { get; set; }
        public DateTime? Fecha { get; set; }
        public Guid? EntidadId { get; set; }
        public string NombreEntidad { get; set; }
        public int? EntityCatalogOptionId { get; set; }
        public string BPIN { get; set; }
        public string Descripcion { get; set; }
        public string Estado { get; set; }
        public string NombreUsuario { get; set; }
        public Guid? TipoObjetoId { get; set; }
        public Guid? InstanciaId { get; set; }
    }
}
