using System;
using System.Diagnostics.CodeAnalysis;

namespace DNP.Backbone.Comunes.Dto
{
    [ExcludeFromCodeCoverage]
    //Se excluyen los DTO por ser clases POCO. Las clases POCO solo tienen propiedades y no tienen métodos para ser testeados.
    public class AccionesDevolucionDto
    {
        public string Accion { get; set; }
        public Guid IdAccionPrincipal { get; set; }
        public Guid IdAccionDevolucion { get; set; }
    }
}
