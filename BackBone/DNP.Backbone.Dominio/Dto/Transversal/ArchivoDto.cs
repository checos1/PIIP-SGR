using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.Backbone.Dominio.Dto.Transversal
{
    public class ArchivoDto
    {
        public Contenido Datos { get; set; }
        public bool EsExcepcion { get; set; }
    }

    public class Contenido {
        public byte[] FileContent { get; set; }
        public string ContentType { get; set; }
        public string FileName { get; set; }
    }
}
