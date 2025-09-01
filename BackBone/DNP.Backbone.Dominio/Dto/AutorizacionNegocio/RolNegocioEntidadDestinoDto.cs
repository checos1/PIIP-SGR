using System;
using System.Diagnostics.CodeAnalysis;

namespace DNP.Backbone.Dominio.Dto.AutorizacionNegocio
{
    [ExcludeFromCodeCoverage]
    //Se excluyen los DTO por ser clases POCO. Las clases POCO solo tienen propiedades y no tienen métodos para ser testeados.
    public class RolNegocioEntidadDestinoDto
    {
        public Guid Id { get; set; }

        public RolNegocioEntidadDto RolNegocioEntidad { get; set; }

        public SectorNegocioDto Sector { get; set; }

        public bool Activo { get; set; }
    }
}
