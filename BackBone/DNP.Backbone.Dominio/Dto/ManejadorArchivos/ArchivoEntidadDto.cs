using System.IO;

namespace DNP.Backbone.Dominio.Dto.ManejadorArchivos
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
