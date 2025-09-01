using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.Backbone.Dominio.Dto.AutorizacionNegocio
{
    [ExcludeFromCodeCoverage]
    public class AdherenciaDto
    {
        public int AdherenciaId { get; set; }

        public Guid EntidadId { get; set; }

        public Guid AdherenciaEntidadId { get; set; }

        public string TipoEntidad { get; set; }

        public DateTime FechaInicio { get; set; }

        public DateTime FechaFin { get; set; }

        public EntidadNegocioDto Entidad { get; set; }

        public EntidadNegocioDto AdherenciaEntidad { get; set; }

    }
}
