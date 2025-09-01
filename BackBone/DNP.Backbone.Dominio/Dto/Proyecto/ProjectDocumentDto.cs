using System;
using SPClient = Microsoft.SharePoint.Client;

namespace DNP.Backbone.Dominio.Dto.Proyecto
{
    public class ProjectDocumentDto
    {
        public string DocumentName { get; set; }
        public string DocumentPath { get; set; }
        public DateTime Created { get; set; }

        public SPClient.ClientResult<System.IO.Stream> FileStream { get; set; }

        public byte[] BytesArray { get; set; }
        public string Comment { get; set; }
        public string Category { get; set; }
        public string Size { get; set; }
        public string Extension { get; set; }
        public int Order { get; set; }
    }

    public class ProjectDocumentSimpleDto
    {
        public string IdProyecto { get; set; }
        public string NombreArchivo { get; set; }
        public string DocumentName { get; set; }
        public DateTime Created { get; set; }
        public string Size { get; set; }
        public string Extension { get; set; }
        public string Category { get; set; }
    }
}
