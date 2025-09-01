namespace DNP.Backbone.Dominio.Dto.Fichas
{
    public class ReporteDto
    {
        public string ID { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string Carpeta { get; set; }
        public string Ruta { get; set; }
        public byte[] Contenido { get; set; }
        public string RutaOrigen { get; set; }
        public string IdCategoria { get; set; }
        public string NombreCategoria { get; set; }
    }
}