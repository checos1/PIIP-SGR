using System;
using System.Diagnostics.CodeAnalysis;

namespace DNP.Backbone.Dominio.Dto.AutorizacionNegocio
{
    [ExcludeFromCodeCoverage]
    //Se excluyen los DTO por ser clases POCO. Las clases POCO solo tienen propiedades y no tienen métodos para ser testeados.
    public class PeticionConfiguracionRolSectorDto
    {
        public Guid IdConfiguracion { get; set; }
        public string NombreAplicacion { get; set; }
        public Guid IdEntidadTerritorial { get; set; }
        public Guid IdRol { get; set; }
        public Guid IdSector { get; set; }
        public Guid IdEntidadDestino { get; set; }
        public bool Activo { get; set; }
        public string TipoEntidad { get; set; }
        public string UsuarioDnp { get; set; }
    }
}
