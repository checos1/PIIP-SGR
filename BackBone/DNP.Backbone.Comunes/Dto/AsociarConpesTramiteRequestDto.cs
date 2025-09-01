
using System.Collections.Generic;

namespace DNP.Backbone.Comunes.Dto
{
    public class AsociarConpesTramiteRequestDto
    {
        public int TramiteId { get; set; }

        public List<TramiteConpesDetailDto> Conpes { get; set; }
    }
}
