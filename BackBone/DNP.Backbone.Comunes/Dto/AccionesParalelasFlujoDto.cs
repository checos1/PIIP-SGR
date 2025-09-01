
namespace DNP.Backbone.Comunes.Dto
{
    using System.Diagnostics.CodeAnalysis;
    using System;
    using System.Collections.Generic;
    using Enums;

    [ExcludeFromCodeCoverage]
    //Se excluyen los DTO por ser clases POCO. Las clases POCO solo tienen propiedades y no tienen métodos para ser testeados.
    public class AccionesParalelasFlujoDto
    {
        public Guid Id { get; set; }

        public bool? EsObligatoria { get; set; }

        public int? NumeroAcciones { get; set; }

        public string Estado { get; set; }

        public string Nombre { get; set; }

        public TipoAccion TipoAccion { get; set; }

        public List<AccionesFlujosMenuContextualDto> Acciones { get; set; }
    }
}
