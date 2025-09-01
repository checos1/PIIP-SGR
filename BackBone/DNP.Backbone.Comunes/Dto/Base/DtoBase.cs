using System;

namespace DNP.Backbone.Comunes.Dto.Base
{
    public class DtoBase<TKey>
    {
        public TKey Id { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string CreadoPor { get; set; }
        public string ModificadoPor { get; set; }
    }
}
