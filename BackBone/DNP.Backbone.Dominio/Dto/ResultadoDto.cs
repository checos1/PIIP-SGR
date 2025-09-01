using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.Backbone.Dominio.Dto
{
        public class ResultadoDto
        {
            public string Mensaje { get; set; }
            public int Codigo { get; set; }

            public ResultadoDto()
            {
            }
        }

        public class ResultadoDto<T>
        {
            public string Mensaje { get; set; }
            public int Codigo { get; set; }

            public T Data { get; set; }

            public ResultadoDto()
            {
            }

            public ResultadoDto(T data)
            {
                Data = data;
            }
        }
}
