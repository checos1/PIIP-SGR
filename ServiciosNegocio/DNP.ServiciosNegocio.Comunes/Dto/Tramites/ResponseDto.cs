using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.ServiciosNegocio.Comunes.Dto.Tramites
{
    public class ResponseDto<T>
    {
        public bool Estado { get; set; }

        public string Mensaje { get; set; }

        public T Data { get; set; }

        public ResponseDto()
        {
            Estado = false;
        }
    }
}
