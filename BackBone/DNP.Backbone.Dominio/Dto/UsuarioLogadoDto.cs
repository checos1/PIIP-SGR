using System;
using System.Diagnostics.CodeAnalysis;

namespace DNP.Backbone.Dominio.Dto
{
    public class UsuarioLogadoDto
    {
        public string IdUsuario { get; set; }
        public string IdAplicacionBackbone { get; set; }
        public string IdNombreBackbone { get; set; }
        public string ApiAutorizacion { get; set; }
        public Guid GuidPIIPAplicacion { get; set; }
        public Guid GuidAdministracionAplicacion { get; set; }
        

    }
}
