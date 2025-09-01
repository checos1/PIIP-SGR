namespace DNP.Backbone.Dominio.Dto.Tramites
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    //Se excluyen los DTO por ser clases POCO. Las clases POCO solo tienen propiedades y no tienen métodos para ser testeados.
    public class DatosUsuarioDto
    {
        public Guid IdUsuario { get; set; }
        public string NombreUsuario { get; set; }
        public string Cuenta { get; set; }
        public Guid? EntidadId { get; set; }
        public string Entidad { get; set; }

        public Guid? RolId { get; set; }

    }
}
