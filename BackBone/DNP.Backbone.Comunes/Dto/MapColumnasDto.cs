using DNP.Backbone.Comunes.Dto.Base;
using DNP.Backbone.Comunes.Enums;

namespace DNP.Backbone.Comunes.Dto
{
    public class MapColumnasDto : DtoBase<int>
    {
        public string NombreColumna { get; set; }
        public TipoColumna TipoColumna { get; set; }
        public bool Estado { get; set; }
    }
}
