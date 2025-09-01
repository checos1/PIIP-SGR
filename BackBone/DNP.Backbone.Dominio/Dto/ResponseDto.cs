using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.Backbone.Dominio.Dto
{
    [ExcludeFromCodeCoverage]
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
