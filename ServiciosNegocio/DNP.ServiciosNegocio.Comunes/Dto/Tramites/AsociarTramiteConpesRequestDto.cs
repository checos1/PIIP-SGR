

using DNP.ServiciosNegocio.Dominio.Dto.Conpes;
using System.Collections.Generic;

namespace DNP.ServiciosNegocio.Comunes.Dto.Tramites
{
    public  class AsociarTramiteConpesRequestDto
    {
        public int TramiteId { get; set; } 

        public List<TramiteConpesDto> Conpes { get; set; } 
    }
}
