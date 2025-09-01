using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using DNP.Backbone.Comunes.Enums;

namespace DNP.Backbone.Comunes.Dto
{
    [ExcludeFromCodeCoverage]
    //Se excluyen los DTO por ser clases POCO. Las clases POCO solo tienen propiedades y no tienen métodos para ser testeados.
    public class AccionesFlujosSubPasosDto
    {
        public Guid Id { get; set; }
        public Guid AccionFlujoId { get; set; }
        public int SubPasoId { get; set; }
        public string NombreSubPaso { get; set; }
        public Guid IdRol { get; set; }
        public int SubPasoPadreId { get; set; }
        public int IndiceCreacion { get; set; }
        public bool EnviaSiguientePaso { get; set; }
        public string CreadoPor { get; set; }
        public DateTime FechaCreacion { get; set; }
        public bool Activo { get; set; }
        public bool MostrarBtnEnviar { get; set; }
    }
}
