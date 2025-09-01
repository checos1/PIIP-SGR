using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.ServiciosNegocio.Comunes.Dto.Tramites
{
    public  class RemoverAsociacionConpesTramiteDto
    {
        public int TramiteId { get; set; }

        public string numeroConpes { get; set; }

        public int Id { get; set; }
    }
}
