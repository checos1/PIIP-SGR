using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using DNP.Backbone.Comunes.Enums;

namespace DNP.Backbone.Comunes.Dto
{
    [ExcludeFromCodeCoverage]
    //Se excluyen los DTO por ser clases POCO. Las clases POCO solo tienen propiedades y no tienen métodos para ser testeados.
    public class EnrutamientoDto
    {
        public Guid Id { get; set; }
        public string UrlServicio { get; set; }
        public string IdServicio { get; set; }
        public string EntidadSeleccionada { get; set; }
        public OperadorLogico OperadorLogico { get; set; }
        public Guid AccionesFlujosId { get; set; }
        public List<CondicionEnrutamientoDto> CondicionesEnrutamiento { get; set; }
    }
}
