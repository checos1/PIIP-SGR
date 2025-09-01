using System;

namespace DNP.Backbone.Comunes.Dto
{
    public class EntidadRolDto
    {
        public int EntidadId { get; set; }
        public Guid RolId { get; set; }
        public Guid? InstanciaId { get; set; }
    }
}
