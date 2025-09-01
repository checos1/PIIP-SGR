using System;

namespace DNP.Backbone.Dominio.Dto.Consola
{
    public class DocumentoDto
    {
        public DateTime Fecha { get; set; }
        public string Nombre { get; set; }
        public string UrlArchivo { get; set; }
        public string Usuario { get; set; }
        public string Id { get; set; }
        public Metadatos Metadatos { get; set; }
        public string Status { get; set; }
        public string Coleccion { get; set; }
    }

    public class Metadatos
    {
        public string Proyecto { get; set; }
        public string NombreAccion { get; set; }
        public string IdAplicacion { get; set; }
        public string IdNivel { get; set; }
        public string IdInstancia { get; set; }
        public string IdAccion { get; set; }
        public string IdInstanciaFlujoPrincipal { get; set; }
        public string IdObjetoNegocio { get; set; }
        public int Size { get; set; }
        public string Extension { get; set; }
        public DateTime FechaCreacionArchivo { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string CategoriaId { get; set; }
        public string Bpin { get; set; }
        public string IdArchivoBlob { get; set; }
        public string ContentType { get; set; }
    }


}
