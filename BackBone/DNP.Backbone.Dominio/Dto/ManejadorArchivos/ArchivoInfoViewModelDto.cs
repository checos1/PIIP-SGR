using System;
using System.Collections.Generic;

namespace DNP.Backbone.Dominio.Dto.ManejadorArchivos
{
    public class ArchivoInfoViewModelDto
    {
        public DateTime Fecha { get; set; }
        public string Nombre { get; set; }
        public string UrlArchivo { get; set; }
        public string Usuario { get; set; }
        public string Id { get; set; }
        public Dictionary<string, object> Metadatos { get; set; }
        public string Status { get; set; }
        public string Coleccion { get; set; }
    }
}
