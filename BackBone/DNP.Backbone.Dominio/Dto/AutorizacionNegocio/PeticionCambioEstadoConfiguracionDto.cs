using System;
using System.Diagnostics.CodeAnalysis;

namespace DNP.Backbone.Dominio.Dto.AutorizacionNegocio
{
    [ExcludeFromCodeCoverage]
    //Se excluyen los DTO por ser clases POCO. Las clases POCO solo tienen propiedades y no tienen métodos para ser testeados.
    public class PeticionCambioEstadoConfiguracionDto
    {
        public string UsuarioDnp { get; set; }
        public Guid IdConfiguracion { get; set; }
        public bool Estado { get; set; }
        public string NombreAplicacion { get; set; }
    }
}
