using DNP.Backbone.Dominio.Enums;
using System.Collections.Generic;

namespace DNP.Backbone.Dominio.Dto.CentroAyuda
{
    public class AyudaTemaListaItemDto
    {
        public int Id { get; set; }

        public AyudaTipoEnum AyudaTipoEnum { get; set; }
        
        public int? TemaGeneralId { get; set; }

        public string Nombre { get; set; }

        public string Descripcion { get; set; }

        public bool Activo { get; set; }

        public string Contenido { get; set; }

        public IEnumerable<AyudaTemaListaItemDto> SubItems { get; set; }
    }
}
