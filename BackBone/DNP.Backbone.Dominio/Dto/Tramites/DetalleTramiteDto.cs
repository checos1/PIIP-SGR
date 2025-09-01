using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.Backbone.Dominio.Dto.Tramites
{
    [ExcludeFromCodeCoverage]
    public class DetalleTramiteDto
    {
        public int TramiteId { get; set; }

        public int SectorId { get; set; }

        public string NombreSector { get; set; }

        public int EntidadId { get; set; }

        public string NombreEntidad { get; set; }

        public int TipoTramiteId { get; set; }

        public string TipoTramite { get; set; }
    }
}
