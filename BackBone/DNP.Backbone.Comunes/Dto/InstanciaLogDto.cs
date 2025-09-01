using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.Backbone.Comunes.Dto
{
    public class InstanciaLogDto
    {
        public Guid Id { get; set; }
        public DateTime? Fecha { get; set; }
        public Guid EntidadId { get; set; }
        public string NombreEntidad { get; set; }
        public int? EntityCatalogOptionId { get; set; }
        public string BPIN { get; set; }
        public string Descripcion { get; set; }
        public int? Estado { get; set; }
        public Guid UsuarioId { get; set; }
        public string NombreUsuario { get; set; }
        public int? TipoObjetoId { get; set; }

        public string[] ColumnasVisibles { get; set; }
    }
}
