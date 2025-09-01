namespace DNP.Autorizacion.Dominio.Dto
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    //Se excluyen los DTO por ser clases POCO. Las clases POCO solo tienen propiedades y no tienen métodos para ser testeados.
    public class PerfilDto
    {
        public Guid? IdPerfil { get; set; }
        public string NombrePerfil { get; set; }
        public string RolesConcat { get; set; }       
        public List<RolDto> Roles { get; set; }
        public Guid IdAplicacion { get; set; }
        public bool TieneUsuariosAsociados { get; set; }
        public bool Activo { get; set; }
        public bool Seleccionado { get; set; }
        public bool Vigente { get; set; }
        public DateTime? FechaDesde { get; set; }
        public DateTime? FechaHasta { get; set; }
        public string UsuarioDnp { get; set; }
        
    }
}
