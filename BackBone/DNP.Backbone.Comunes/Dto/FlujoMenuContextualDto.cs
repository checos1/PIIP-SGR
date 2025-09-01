using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace DNP.Backbone.Comunes.Dto
{
    [ExcludeFromCodeCoverage]
    //Se excluyen los DTO por ser clases POCO. Las clases POCO solo tienen propiedades y no tienen métodos para ser testeados.
    public class FlujoMenuContextualDto
    {
        public Guid Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public Guid IdInstancia { get; set; }
        public DateTime? FechaHoraCreacion { get; set; }
        public int? IdEntidad { get; set; }
        public string Entidad { get; set; }
        public string Usuario { get; set; }
        public Guid? RolId { get; set; }
        public string NumeroTramite { get; set; }
        public List<AccionesFlujosMenuContextualDto> Acciones { get; set; }
    }
}
