using System;
using System.Collections.Generic;

namespace DNP.Backbone.Dominio.Dto.ManejadorArchivos
{
    public class ArchivoInfoDto
    {
        public DateTime fecha { get; set; }
        public string nombre { get; set; }
        public string urlArchivo { get; set; }
        public string usuario { get; set; }
        public string id { get; set; }
        public Dictionary<string, object> metadatos { get; set; }
        public string status { get; set; }
        public string coleccion { get; set; }
    }
}
