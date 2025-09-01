using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.ServiciosTransaccional.Servicios.Dto
{
    public class ArchivoEntidadDto
    {
        public string collection { get; set; }
        public string contentType { get; set; }
        public string id { get; set; }
        public string idArchivoBlob { get; set; }
        public string nombre { get; set; }
        public Stream stream { get; set; }
        public string url { get; set; }
    }
}
