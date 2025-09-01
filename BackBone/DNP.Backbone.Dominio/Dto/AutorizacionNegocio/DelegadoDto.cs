using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.Backbone.Dominio.Dto.AutorizacionNegocio
{
    public class DelegadoDto
    {
        public int DelegadoId { get; set; }

        public Guid UsuarioId { get; set; }

        public Guid EntidadId { get; set; }

        public int TipoDelegado { get; set; }

        public string SubtipoDelegado { get; set; }

        public DateTime Fecha { get; set; }

        public virtual EntidadUsuarioDto Usuario { get; set; }

        public virtual EntidadNegocioDto Entidad { get; set; }
        public Guid? IdArchivoBlob { get; set; }

        public string IdArchivo { get; set; }
    }
}
