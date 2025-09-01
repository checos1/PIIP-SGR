using System;

namespace DNP.Backbone.Dominio.Dto.SGP.Transversal
{
    public class TipoDocumentoSoporteDto
    {
        public int Id { get; set; }
        public int TipoDocumentoId { get; set; }
        public string TipoDocumentoCodigo { get; set; }
        public string TipoDocumento { get; set; }
        public bool Obligatorio { get; set; }
        public string CampoValidacion { get; set; }
        public DateTime? FechaValidacion { get; set; }
    }
}
