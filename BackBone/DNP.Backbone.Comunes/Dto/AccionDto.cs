using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using DNP.Backbone.Comunes.Enums;

namespace DNP.Backbone.Comunes.Dto
{
    [ExcludeFromCodeCoverage]
    //Se excluyen los DTO por ser clases POCO. Las clases POCO solo tienen propiedades y no tienen métodos para ser testeados.
    public class AccionDto
    {

        public Guid IdAccionActual { get; set; }
        public Guid IdAccionFinal { get; set; }
        public string NombreAccionActual { get; set; }
        public string NombreAccionFinal { get; set; }
        public Guid IdFlujo { get; set; }
    }
}
