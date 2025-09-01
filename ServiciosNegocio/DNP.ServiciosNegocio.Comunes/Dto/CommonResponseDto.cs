using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.ServiciosNegocio.Comunes.Dto
{
    public class CommonResponseDto<T>
    {
        public bool Estado { get; set; }

        public string Mensaje { get; set; }

        public T Data { get; set; }

        public CommonResponseDto() {
            Estado = false;
        }
    }
}
