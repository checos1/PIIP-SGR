using System;

namespace DNP.Backbone.Dominio.Dto.AutorizacionNegocio
{
    public class RolNegocioEntidadDestino
    {
        public Guid Id { get; set; }

        public Guid RolNegocioEntidadId { get; set; }

        public int SectorId { get; set; }

        public int EntityTypeCatalogOptionId { get; set; }

        public bool Activo { get; set; }
    }
}
