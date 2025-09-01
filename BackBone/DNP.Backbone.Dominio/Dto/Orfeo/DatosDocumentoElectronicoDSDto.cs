using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.Backbone.Dominio.Dto.Orfeo
{
    [ExcludeFromCodeCoverage]
    public class DatosDocumentoElectronicoDSDto
    {

        public DatosDocumentoElectronicoDto datosDocumentoElectronicoDto { get; set; }
        public DatosRadicadoDto datosRadicadoDto { get; set; }

        public UsuarioRadicacionDto usuarioRadica { get; set; }

        public int TramiteId { get; set; }
        public string NumeroTramite { get; set; }
    }
}
