using System;
using System.Diagnostics.CodeAnalysis;

namespace DNP.Backbone.Comunes.Dto
{
    [ExcludeFromCodeCoverage]
    //Se excluyen los DTO por ser clases POCO. Las clases POCO solo tienen propiedades y no tienen métodos para ser testeados.
    public class ObjetoContextoDto
    {
        public Guid IdRol { get; set; }
        public string IdUsuario { get; set; }
        public bool PostDefinitivo { get; set; }
    }
}
