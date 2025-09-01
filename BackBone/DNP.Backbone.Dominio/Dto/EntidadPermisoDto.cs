using System;
using System.Collections.Generic;

namespace DNP.Backbone.Dominio.Dto
{
    public class EntidadPermisoDto
    {
        public Guid IdEntidad { get; set; }
        public bool EsSubEntidad { get; set; }
        public string NombreEntidad { get; set; }
        public string TipoEntidad { get; set; }
        public List<RolEntidadPermisoDto> Roles { get; set; }
        public List<OpcionEntidadPermisoDto> Opciones { get; set; }
    }
}
