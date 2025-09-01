namespace DNP.Backbone.Comunes.Dto
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    //Se excluyen los DTO por ser clases POCO. Las clases POCO solo tienen propiedades y no tienen métodos para ser testeados.
    public class AutorizacionPermisoDto
    {
        public List<int> Estados { get; set; }

        public bool Permiso { get; set; }

    }
}
